Imports System.IO
Public Class Weaver_Cloth_Receipt
    Implements Interface_MDIActions

    Private Declare Function ShellEx Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hWnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer
    Dim vDIC_ATTACHMENTS As New Dictionary(Of Integer, Byte())

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Other_Condition As String = "WCLRC-"
    Private Pk_Condition As String = "WCLRC-"
    Private Pk_Condition2 As String = "WBKRC-"
    Private PkCondition3_INCHK As String = "INCHK-"
    Private PkCondition4_CRCHK As String = "CRCHK-"
    Private PkCondition5_GWEWA As String = "GWEWA-"
    Private PkCondition_WFRGT As String = "WFRGT-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private EntFnYrCode As String = ""
    Private OpYrCode As String = ""
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_BobinDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_KuriDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String

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

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private prn_Status As Integer = 0

    Private vCLO_MTR_PER_PC As String = 0
    Private vCLO_MTRPERPC_QUALITY As String = ""

    Private vDGV_LEVCELNO As Integer

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.-
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        vDIC_ATTACHMENTS = New Dictionary(Of Integer, Byte())
    End Sub

    Private Sub clear()
        Dim i As Integer
        Dim dttm As Date

        lbl_Time.Text = Format(Now, "hh:mm tt")

        chk_Verified_Status.Checked = False

        New_Entry = False
        Insert_Entry = False
        pnl_io_selection.Visible = False
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_DriverDetails.Visible = False
        pnl_PartyDc_Image.Visible = False
        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        chk_Purchase.Checked = False
        chk_NoStockPosting.Checked = False
        chk_No_Weaving_Wages_Bill.Checked = False
        chk_UNLOADEDBYOUREMPLOYEE.Checked = False
        chk_ReturnStatus.Checked = False

        chk_GSTTax_Invocie.Checked = True
        msk_date.Text = ""
        dtp_Date.Text = ""
        lbl_Yarn.Text = ""
        lbl_Pavu.Text = ""
        lbl_EmptyBeam.Text = ""
        txt_Remarks.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        cbo_Sales_OrderCode_forSelection.Text = ""
        txt_Weaver_Cloth_PrefixNo.Text = ""
        cbo_Weaver_Cloth_SufixNo.Text = ""



        If dtp_Date.Enabled = False Then
            dttm = New DateTime(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4), 4, 1)
            dttm = DateAdd(DateInterval.Day, -1, dttm)
            dtp_Date.Text = dttm
        End If

        txt_LotNo.Text = ""
        cbo_Weaver.Text = ""
        cbo_Weaver.Tag = cbo_Weaver.Text
        cbo_Weaver.Tag = ""
        cbo_Cloth.Text = ""
        txt_PDcNo.Text = ""
        cbo_EndsCount.Text = ""
        lbl_WeftCount.Text = ""
        txt_EBeam.Text = ""
        txt_Dc_receipt_pcs.Text = ""
        txt_Dc_receipt_mtrs.Text = ""

        txt_NoOfPcs.Text = ""
        txt_Quantity.Text = ""
        txt_EWBNo.Text = ""
        txt_EWayBillNo.Text = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1490" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            txt_PcsNoFrom.Text = ""
            'PieceNo_From_Calculation()
        Else
            txt_PcsNoFrom.Text = "1"
        End If
        If Trim(Common_Procedures.settings.CustomerCode) = "1204" Or Trim(Common_Procedures.settings.CustomerCode) = "1267" Then
            cbo_LoomType.Text = ""
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1155" Or Trim(Common_Procedures.settings.CustomerCode) = "1428" Or Trim(Common_Procedures.settings.CustomerCode) = "1490" Then
            cbo_LoomType.Text = "AUTOLOOM"
        Else
            cbo_LoomType.Text = "POWERLOOM"
        End If
        lbl_PcsNoTo.Text = ""
        cbo_StockOff.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.OwnSort_Ac))
        txt_ReceiptMeters.Text = ""
        lbl_ConsYarn.Text = ""
        cbo_LoomNo.Text = ""
        cbo_WidthType.Text = ""
        cbo_Transport.Text = ""
        txt_Freight.Text = ""
        Txt_NoOfBundles.Text = ""
        lbl_OrderCode.Text = ""
        lbl_OrderNo.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
            txt_EBeam.Enabled = False
        End If


        lbl_LotNo_Caption.Text = Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1146" Then '----ASMITHA TEXTILES
            txt_Folding_Perc.Text = ""
        Else
            txt_Folding_Perc.Text = "100"
        End If

        dgv_Details.Rows.Clear()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1007" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1490" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1549" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            dgv_Details.AllowUserToAddRows = False
        Else
            dgv_Details.AllowUserToAddRows = True
        End If

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        cbo_LoomType.Enabled = True
        cbo_LoomType.BackColor = Color.White

        cbo_Weaver.Enabled = True
        cbo_Weaver.BackColor = Color.White

        cbo_Cloth.Enabled = True
        cbo_Cloth.BackColor = Color.White

        txt_LotNo.Enabled = True
        txt_LotNo.BackColor = Color.White

        cbo_EndsCount.Enabled = True
        cbo_EndsCount.BackColor = Color.White

        txt_Dc_receipt_pcs.Enabled = True
        txt_Dc_receipt_pcs.BackColor = Color.White
        txt_NoOfPcs.Enabled = True
        txt_NoOfPcs.BackColor = Color.White

        txt_PcsNoFrom.Enabled = True
        txt_PcsNoFrom.BackColor = Color.White

        txt_ReceiptMeters.Enabled = True
        txt_ReceiptMeters.BackColor = Color.White

        cbo_LoomNo.Enabled = False ' True
        cbo_LoomNo.BackColor = Color.White

        cbo_WidthType.Enabled = False '  True
        cbo_WidthType.BackColor = Color.White
        Set_LoomType_LoomNo_WidthType()

        lbl_Dc_receipt_pcs_Caption.BackColor = Color.LightSkyBlue
        txt_Dc_receipt_pcs.BackColor = Color.White
        lbl_caption_dc_receipt_metres.BackColor = Color.LightSkyBlue
        txt_Dc_receipt_mtrs.BackColor = Color.White

        dgv_BobinDetails.Rows.Clear()
        dgv_KuriDetails.Rows.Clear()

        pic_PartyDc_Image.BackgroundImage = Nothing

        cbo_Grid_BeamNo1.Visible = False
        cbo_Grid_BeamNo2.Visible = False
        cbo_Grid_BeamNo1.Text = ""
        cbo_Grid_BeamNo2.Text = ""


        cbo_DriverName.Text = ""
        cbo_SupervisorName.Text = ""
        cbo_DriverPhNo.Text = ""
        cbo_VehicleNo.Text = ""
        cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.Godown_Ac))

        If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then '---- BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION) 
            txt_NoOfPcs.Enabled = False
            txt_NoOfPcs.BackColor = Color.FromArgb(224, 224, 224)
            txt_ReceiptMeters.Enabled = False
            txt_ReceiptMeters.BackColor = Color.FromArgb(224, 224, 224)
            dgv_Details.Enabled = False
        End If

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Cloth.Text = ""
            txt_Filter_RecNo.Text = ""
            txt_Filter_RecNoTo.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_Cloth.SelectedIndex = -1


            dgv_Filter_Details.Rows.Clear()
        End If

        pnl_Attachments.Visible = False
        dgv_Attachments.Rows.Clear()
        vDIC_ATTACHMENTS = New Dictionary(Of Integer, Byte())

        vDGV_LEVCELNO = -1
        vCLO_MTR_PER_PC = 0
        vCLO_MTRPERPC_QUALITY = ""
        dgv_ActiveCtrl_Name = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        Dim chk As CheckBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Or TypeOf Me.ActiveControl Is CheckBox Then
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
            Msktxbx = Me.ActiveControl
            Msktxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is CheckBox Then
            chk = Me.ActiveControl
            chk.Focus()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_BeamNo1.Name Then
            cbo_Grid_BeamNo1.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_BeamNo2.Name Then
            cbo_Grid_BeamNo2.Visible = False
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
            ElseIf TypeOf Prec_ActCtrl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_KuriDetails.CurrentCell) Then dgv_KuriDetails.CurrentCell.Selected = False

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
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String
        Dim n, slno As Integer
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as WeaverName,c.Ledger_Name as Transport_Name, d.Cloth_Name , e.Ledger_Name as StockOff_Name,pc.Cloth_Name as Pro_Cloth_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo INNER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_IdNo LEFT OUTER JOIN Ledger_Head e ON a.StockOff_IdNo = e.Ledger_IdNo left outer join Cloth_Head pc on a.Processed_Cloth_IdNo = pc.Cloth_IdNo Where a.Receipt_Type = 'W' and a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and " & Other_Condition, con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_RefNo").ToString
                txt_Weaver_Cloth_PrefixNo.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_PrefixNo").ToString
                cbo_Weaver_Cloth_SufixNo.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_SuffixNo").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_date")
                msk_date.Text = dtp_Date.Text
                lbl_Time.Text = dt1.Rows(0).Item("Entry_Time").ToString

                cbo_Weaver.Text = dt1.Rows(0).Item("WeaverName").ToString
                cbo_Weaver.Tag = cbo_Weaver.Text
                cbo_Cloth.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                cbo_LoomType.Text = dt1.Rows(0).Item("Loom_Type").ToString
                txt_LotNo.Text = dt1.Rows(0).Item("Lot_No").ToString
                txt_PDcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, Val(dt1.Rows(0).Item("EndsCount_IdNo").ToString))
                lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Count_IdNo").ToString))
                txt_EBeam.Text = dt1.Rows(0).Item("empty_beam").ToString
                txt_Dc_receipt_pcs.Text = dt1.Rows(0).Item("DC_Receipt_Pcs").ToString
                txt_Dc_receipt_mtrs.Text = dt1.Rows(0).Item("Dc_Receipt_Meters").ToString
                txt_NoOfPcs.Text = Val(dt1.Rows(0).Item("noof_pcs").ToString)
                txt_PcsNoFrom.Text = dt1.Rows(0).Item("pcs_fromno").ToString
                lbl_PcsNoTo.Text = dt1.Rows(0).Item("pcs_tono").ToString
                txt_ReceiptMeters.Text = dt1.Rows(0).Item("ReceiptMeters_Receipt").ToString

                If txt_Dc_receipt_pcs.Visible = True Then
                    If Val(txt_ReceiptMeters.Text) = 0 Then
                        txt_NoOfPcs.Text = ""
                    End If
                End If

                txt_Quantity.Text = dt1.Rows(0).Item("Receipt_Quantity").ToString
                lbl_ConsYarn.Text = dt1.Rows(0).Item("ConsumedYarn_Receipt").ToString
                cbo_LoomNo.Text = Common_Procedures.Loom_IdNoToName(con, Val(dt1.Rows(0).Item("Loom_IdNo").ToString))
                cbo_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
                cbo_Transport.Text = dt1.Rows(0).Item("Transport_Name").ToString
                txt_Freight.Text = dt1.Rows(0).Item("Freight_Amount_Receipt").ToString
                Txt_NoOfBundles.Text = dt1.Rows(0).Item("No_Of_Bundles").ToString
                cbo_StockOff.Text = dt1.Rows(0).Item("StockOff_Name").ToString
                If Val(dt1.Rows(0).Item("Purchase_Status").ToString) = 1 Then chk_Purchase.Checked = True
                If Val(dt1.Rows(0).Item("No_Weaving_Wages_Bill").ToString) = 1 Then chk_No_Weaving_Wages_Bill.Checked = True
                If Val(dt1.Rows(0).Item("No_Stock_Posting_Status").ToString) = 1 Then chk_NoStockPosting.Checked = True
                If Val(dt1.Rows(0).Item("Return_Status").ToString) = 1 Then chk_ReturnStatus.Checked = True



                If Val(dt1.Rows(0)("Unloaded_By_Our_Employee").ToString) <> 0 Then
                    chk_UNLOADEDBYOUREMPLOYEE.Checked = True
                End If
                cbo_DriverName.Text = dt1.Rows(0).Item("Driver_Name").ToString
                cbo_DriverPhNo.Text = dt1.Rows(0).Item("Driver_Phone_No").ToString
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_SupervisorName.Text = dt1.Rows(0).Item("Supervisor_Name").ToString
                cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))
                txt_EWBNo.Text = dt1.Rows(0).Item("Eway_BillNo").ToString
                txt_EWayBillNo.Text = dt1.Rows(0).Item("Eway_BillNo").ToString
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                txt_Rate.Text = dt1.Rows(0).Item("Rate").ToString
                lbl_Amount.Text = dt1.Rows(0).Item("Amount").ToString

                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True

                LockSTS = False

                If IsDBNull(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                If IsDBNull(dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                If IsDBNull(dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                If IsDBNull(dt1.Rows(0).Item("Delivery_Purpose_IdNo")) = False Then
                    If Val(dt1.Rows(0).Item("Delivery_Purpose_IdNo").ToString) <> 0 Then
                        cbo_Delivery_Purpose.Text = Common_Procedures.Process_IdNoToName(con, dt1.Rows(0).Item("Delivery_Purpose_IdNo").ToString)
                    End If
                End If

                If IsDBNull(dt1.Rows(0).Item("Pro_Cloth_Name").ToString) = False Then
                    cbo_Processed_Cloth.Text = dt1.Rows(0).Item("Pro_Cloth_Name").ToString
                End If


                If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then '---- BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION) 

                    Dim vSELC_LOTCODE As String = ""

                    vSELC_LOTCODE = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

                    da2 = New SqlClient.SqlDataAdapter("select count(*) from Lot_Approved_Head Where lotcode_forSelection = '" & Trim(vSELC_LOTCODE) & "'", con)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                            If Val(dt2.Rows(0)(0).ToString) > 0 Then
                                LockSTS = True
                            End If
                        End If
                    End If
                    dt2.Clear()

                    da2 = New SqlClient.SqlDataAdapter("select count(a.Checking_Table_IdNo) from Lot_Allotment_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_ForSelection = '" & Trim(vSELC_LOTCODE) & "' and a.Checking_Table_IdNo <> 0 Having count(a.Checking_Table_IdNo) <> 0", con)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                            If Val(dt2.Rows(0)(0).ToString) > 0 Then
                                LockSTS = True
                            End If
                        End If
                    End If
                    dt2.Clear()

                    If Common_Procedures.User.IdNo <> 1 Then
                        da2 = New SqlClient.SqlDataAdapter("select count(a.Piece_Checking_Defect_IdNo) from Weaver_ClothReceipt_App_Piece_Defect_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.LotCode_Selection = '" & Trim(vSELC_LOTCODE) & "' and a.Piece_Checking_Defect_IdNo <> 0  Having count(a.Piece_Checking_Defect_IdNo) <> 0", con)
                        dt2 = New DataTable
                        da2.Fill(dt2)
                        If dt2.Rows.Count > 0 Then
                            If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                                If Val(dt2.Rows(0)(0).ToString) > 0 Then
                                    LockSTS = True
                                End If
                            End If
                        End If
                        dt2.Clear()

                        da2 = New SqlClient.SqlDataAdapter("select count(a.Piece_No) from Weaver_ClothReceipt_App_PieceChecking_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_Selection = '" & Trim(vSELC_LOTCODE) & "' and a.Piece_No <> '' and a.Total_Checking_Meters <> 0  Having count(a.Piece_No) <> 0", con)
                        dt2 = New DataTable
                        da2.Fill(dt2)
                        If dt2.Rows.Count > 0 Then
                            If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                                If Val(dt2.Rows(0)(0).ToString) > 0 Then
                                    LockSTS = True
                                End If
                            End If
                        End If
                        dt2.Clear()

                    End If

                End If
                lbl_OrderNo.Text = dt1.Rows(0).Item("Our_Order_No").ToString
                lbl_OrderCode.Text = dt1.Rows(0).Item("Own_Order_Code").ToString
                cbo_Sales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString
                txt_Folding_Perc.Text = dt1.Rows(0).Item("Folding_Receipt").ToString
                If Val(txt_Folding_Perc.Text) = 0 Then txt_Folding_Perc.Text = 100

                cmd.Connection = con

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sp_get_weaverclothreceiptpiecedetails_for_moving3"
                cmd.Parameters.Clear()
                cmd.Parameters.Add("@weaver_clothreceipt_code", SqlDbType.VarChar)
                cmd.Parameters("@weaver_clothreceipt_code").Value = Trim(Pk_Condition) & Trim(NewCode)
                cmd.Parameters.Add("@lot_code", SqlDbType.VarChar)
                cmd.Parameters("@lot_code").Value = Trim(NewCode)
                da2 = New SqlClient.SqlDataAdapter(cmd)

                'da2 = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and Create_Status = 1 Order by Sl_No, Piece_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        dgv_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Piece_No").ToString
                        dgv_Details.Rows(n).Cells(1).Value = Format(Val(dt2.Rows(i).Item("Receipt_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Loom_No").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("BeamNo_SetCode_forSelection").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("BeamNo2_SetCode_forSelection").ToString
                        dgv_Details.Rows(n).Cells(5).Value = ""
                        dgv_Details.Rows(n).Cells(6).Value = ""
                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Receipt_Dhothi_Quantity").ToString

                    Next i

                End If
                dt2.Clear()

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(0).Value = Val(dt1.Rows(0).Item("Total_Receipt_Pcs").ToString)
                    .Rows(0).Cells(1).Value = Format(Val(dt1.Rows(0).Item("Total_Receipt_Meters").ToString), "########0.00")
                End With


                da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name from Stock_Pavu_Processing_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'", con)
                da.Fill(dt3)

                dgv_BobinDetails.Rows.Clear()
                slno = 0

                If dt3.Rows.Count > 0 Then

                    For i = 0 To dt3.Rows.Count - 1

                        n = dgv_BobinDetails.Rows.Add()
                        dgv_BobinDetails.Rows(n).Cells(0).Value = dt3.Rows(i).Item("EndsCount_Name").ToString
                        dgv_BobinDetails.Rows(n).Cells(1).Value = Format(Val(dt3.Rows(i).Item("Meters").ToString), "########0.000")

                    Next i

                End If
                dt3.Clear()
                dt3.Dispose()

                da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Stock_Yarn_Processing_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'", con)
                dt4 = New DataTable
                da.Fill(dt4)

                dgv_KuriDetails.Rows.Clear()
                slno = 0

                If dt4.Rows.Count > 0 Then

                    For i = 0 To dt4.Rows.Count - 1

                        n = dgv_KuriDetails.Rows.Add()


                        dgv_KuriDetails.Rows(n).Cells(0).Value = dt4.Rows(i).Item("Count_Name").ToString
                        dgv_KuriDetails.Rows(n).Cells(1).Value = Format(Val(dt4.Rows(i).Item("Weight").ToString), "#######0.000")

                    Next i

                End If
                dt4.Clear()
                dt4.Dispose()

                da = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Attachment_Details a where a.Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt5 = New DataTable
                da.Fill(dt5)

                dgv_Attachments.Rows.Clear()
                vDIC_ATTACHMENTS = New Dictionary(Of Integer, Byte())

                If dt5.Rows.Count > 0 Then

                    For i = 0 To dt5.Rows.Count - 1

                        n = dgv_Attachments.Rows.Add()
                        dgv_Attachments.Rows(n).Cells(0).Value = n + 1
                        dgv_Attachments.Rows(n).Cells(1).Value = dt5.Rows(i).Item("file_name").ToString
                        dgv_Attachments.Rows(i).Cells(2).Value = "DOWNLOAD"
                        dgv_Attachments.Rows(i).Cells(3).Value = "DELETE"

                        If vDIC_ATTACHMENTS.ContainsKey(n) Then
                            vDIC_ATTACHMENTS(n) = dt5.Rows(i).Item("file_content")
                        Else
                            vDIC_ATTACHMENTS.Add(n, dt5.Rows(i).Item("file_content"))
                        End If

                    Next i

                End If
                dt5.Clear()
                dt5.Dispose()

            Else

                new_record()

            End If

            dt1.Clear()

            If LockSTS = True Then

                'cbo_LoomType.Enabled = False
                'cbo_LoomType.BackColor = Color.LightGray

                cbo_Weaver.Enabled = False
                cbo_Weaver.BackColor = Color.LightGray

                cbo_Cloth.Enabled = False
                cbo_Cloth.BackColor = Color.LightGray

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "--1267--" Then '---- BRT TEXTILES(SOMANUR)
                    If Common_Procedures.User.IdNo = 1 Or Trim(Common_Procedures.UR.Weaver_ClothRceipt_Entry_Edit_FABRICNAME_AFTERLOCK) <> "" Then
                        cbo_Weaver.Enabled = True
                        cbo_Cloth.Enabled = True
                    End If
                End If

                txt_LotNo.Enabled = False
                txt_LotNo.BackColor = Color.LightGray

                cbo_EndsCount.Enabled = False
                cbo_EndsCount.BackColor = Color.LightGray

                txt_NoOfPcs.Enabled = False
                txt_NoOfPcs.BackColor = Color.LightGray

                txt_PcsNoFrom.Enabled = False
                txt_PcsNoFrom.BackColor = Color.LightGray

                txt_ReceiptMeters.Enabled = False
                txt_ReceiptMeters.BackColor = Color.LightGray

                If (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1249" And Common_Procedures.User.IdNo <> 1) Or (Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1204" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1277" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1037" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1249" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1352" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1395") Then '----KRG (PALLADAM)

                    'cbo_LoomNo.Enabled = False
                    'cbo_LoomNo.BackColor = Color.LightGray

                    'cbo_WidthType.Enabled = False
                    'cbo_WidthType.BackColor = Color.LightGray

                    'cbo_LoomType.Enabled = False
                    'cbo_LoomType.BackColor = Color.LightGray

                End If

                dgv_Details.AllowUserToAddRows = False

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()

        End Try

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_KuriDetails.CurrentCell) Then dgv_KuriDetails.CurrentCell.Selected = False
    End Sub

    Private Sub Weaver_Cloth_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Weaver.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Weaver.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Cloth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LoomNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_LoomNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                Me.Text = lbl_Heading.Text & "  -  " & lbl_Company.Text

                new_record()

                'lbl_Company.Text = ""
                'lbl_Company.Tag = 0
                'Common_Procedures.CompIdNo = 0

                'Me.Text = lbl_Heading.Text

                'CompCondt = ""
                'If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                '    CompCondt = "Company_Type = 'ACCOUNT'"
                'End If

                'da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
                'dt1 = New DataTable
                'da.Fill(dt1)

                'NoofComps = 0
                'If dt1.Rows.Count > 0 Then
                '    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                '        NoofComps = Val(dt1.Rows(0)(0).ToString)
                '    End If
                'End If
                'dt1.Clear()

                'If Val(NoofComps) = 1 Then

                '    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo", con)
                '    dt1 = New DataTable
                '    da.Fill(dt1)

                '    If dt1.Rows.Count > 0 Then
                '        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                '            Common_Procedures.CompIdNo = Val(dt1.Rows(0)(0).ToString)
                '        End If

                '    End If
                '    dt1.Clear()

                'Else

                '    Dim f As New Company_Selection
                '    f.ShowDialog()

                'End If

                'If Val(Common_Procedures.CompIdNo) <> 0 Then

                '    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                '    dt1 = New DataTable
                '    da.Fill(dt1)

                '    If dt1.Rows.Count > 0 Then
                '        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                '            lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
                '            lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
                '            Me.Text = lbl_Heading.Text & "   -   " & Trim(dt1.Rows(0)(1).ToString)
                '        End If
                '    End If
                '    dt1.Clear()

                '    new_record()

                'Else
                '    MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                '    'Me.Close()
                '    Exit Sub

                'End If

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Weaver_Cloth_Receipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Me.Text = lbl_Heading.Text

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            lbl_LotNo_Caption.Text = "Ref No."
        Else
            lbl_LotNo_Caption.Text = StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- PRAKASH TEXTILES (PALLADAM)
            Other_Condition = "(Receipt_Type = 'W'  and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition3_INCHK) & "%' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition4_CRCHK) & "%' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition5_GWEWA) & "%')"

        Else
            Other_Condition = "( Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition3_INCHK) & "%' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition4_CRCHK) & "%' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition5_GWEWA) & "%')"

        End If

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        msk_date.Enabled = True
        dtp_Date.Enabled = True

        EntFnYrCode = Common_Procedures.FnYearCode
        If Trim(UCase(Common_Procedures.WeaCloRcpt_Opening_OR_Entry)) = "OPENING" Then

            EntFnYrCode = OpYrCode

            msk_date.Enabled = False
            dtp_Date.Enabled = False
        End If

        txt_NoOfPcs.Width = txt_ReceiptMeters.Width ' "211"
        lbl_ClothReceipt_Quantity_Caption.Visible = False
        txt_Quantity.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then  '-- Jeno tex or annai tex
            lbl_ClothReceipt_Quantity_Caption.Text = "Quantity"
            lbl_ClothReceipt_Quantity_Caption.Visible = True
            txt_Quantity.Text = ""
            txt_Quantity.Visible = True
            txt_NoOfPcs.Width = 70
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1204" Then '------KOHINOOR TEXTILE MILLS
            lbl_WidthType_Asterisk.Visible = True
            lbl_LoomNo_Asterisk.Visible = True
        End If



        lbl_OrderNo_Caption.Visible = False
        lbl_OrderNo.Visible = False
        btn_io_selection.Visible = False
        lbl_Sales_OrderCode_forSelection_Caption.Visible = False
        cbo_Sales_OrderCode_forSelection.Visible = False
        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then
            lbl_Sales_OrderCode_forSelection_Caption.Visible = True
            cbo_Sales_OrderCode_forSelection.Visible = True
            lbl_Sales_OrderCode_forSelection_Caption.BackColor = Color.LightSkyBlue
            cbo_Sales_OrderCode_forSelection.BackColor = Color.White
            cbo_Weaver.Width = cbo_Cloth.Width
            cbo_Cloth.Width = txt_Remarks.Width
        End If

        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("")
        cbo_WidthType.Items.Add("SINGLE")
        cbo_WidthType.Items.Add("DOUBLE")
        cbo_WidthType.Items.Add("TRIPLE")
        cbo_WidthType.Items.Add("FOURTH")
        cbo_WidthType.Items.Add("FIVE")
        cbo_WidthType.Items.Add("SIX")
        cbo_WidthType.Items.Add("SINGLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("SINGLE FABRIC FROM 2 BEAMS")
        cbo_WidthType.Items.Add("DOUBLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("DOUBLE FABRIC FROM 2 BEAMS")
        cbo_WidthType.Items.Add("TRIPLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("TRIPLE FABRIC FROM 2 BEAMS")
        cbo_WidthType.Items.Add("FOUR FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("FOUR FABRIC FROM 2 BEAMS")
        cbo_WidthType.Items.Add("FIVE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("FIVE FABRIC FROM 2 BEAMS")
        cbo_WidthType.Items.Add("SIX FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("SIX FABRIC FROM 2 BEAMS")

        cbo_LoomType.Items.Clear()
        cbo_LoomType.Items.Add("")
        cbo_LoomType.Items.Add("POWERLOOM")
        cbo_LoomType.Items.Add("AUTOLOOM")

        cbo_Verified_Sts.Items.Clear()
        cbo_Verified_Sts.Items.Add("")
        cbo_Verified_Sts.Items.Add("YES")
        cbo_Verified_Sts.Items.Add("NO")

        cbo_Weaver_Cloth_SufixNo.Items.Clear()
        cbo_Weaver_Cloth_SufixNo.Items.Add("")
        cbo_Weaver_Cloth_SufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_Weaver_Cloth_SufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))
        cbo_Weaver_Cloth_SufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate))
        cbo_Weaver_Cloth_SufixNo.Items.Add("/" & Trim(Year(Common_Procedures.Company_FromDate)) & "-" & Trim(Microsoft.VisualBasic.Right(Year(Common_Procedures.Company_ToDate), 2)))

        txt_Weaver_Cloth_PrefixNo.Visible = True

        dtp_Date.Text = ""
        msk_date.Text = ""
        txt_PDcNo.Text = ""
        txt_LotNo.Text = ""
        cbo_Weaver.Text = ""
        cbo_Weaver.Tag = cbo_Weaver.Text
        cbo_EndsCount.Text = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
            txt_EBeam.Enabled = False
            txt_Weaver_Cloth_PrefixNo.Visible = False
        End If

        lbl_StockOff_Caption.Visible = False
        cbo_StockOff.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1075" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            cbo_StockOff.Visible = True
            lbl_StockOff_Caption.Visible = True

            lbl_StockOff_Caption.Left = lbl_DeliveryPurpose_Caption.Left
            lbl_StockOff_Caption.Top = lbl_DeliveryPurpose_Caption.Top
            lbl_DeliveryPurpose_Caption.Visible = False
            cbo_StockOff.Left = cbo_Delivery_Purpose.Left
            cbo_StockOff.Top = cbo_Delivery_Purpose.Top
            cbo_StockOff.Width = cbo_Delivery_Purpose.Width
            cbo_StockOff.BackColor = Color.White
            cbo_StockOff.BringToFront()
            cbo_Delivery_Purpose.Visible = False

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
            txt_LotNo.Visible = True
            lbl_LotNoCaption.Visible = True
            cbo_Cloth.Width = 308

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then

            lbl_LotNoCaption.Visible = True
            lbl_LotNoCaption.Text = "Folding %"
            txt_Folding_Perc.Visible = True
            If txt_Folding_Perc.Visible = True Then
                cbo_Cloth.Width = 308
                txt_Folding_Perc.Width = 120
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1146" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1516" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '------ASHMITHA TEXTILE

            lbl_LotNoCaption.Visible = True
            lbl_LotNoCaption.Text = "Folding %"
            txt_Folding_Perc.Visible = True
            cbo_Cloth.Width = 308

        Else

            cbo_Cloth.Width = txt_Remarks.Width
            'cbo_Cloth.Width = cbo_Weaver.Width

        End If

        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Then '---- Kalaimagal Textiles (Avinashi)
            btn_SaveAll.Visible = True
        End If

        btn_DriverDetails.Visible = False
        chk_NoStockPosting.Visible = False
        chk_No_Weaving_Wages_Bill.Visible = False

        lbl_caption_dc_receipt_metres.Visible = False
        txt_Dc_receipt_mtrs.Visible = False
        lbl_Dc_receipt_pcs_Caption.Visible = False
        txt_Dc_receipt_pcs.Visible = False


        If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then '---- BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION) 
            btn_DriverDetails.Visible = True
            'chk_NoStockPosting.Visible = True
            chk_UNLOADEDBYOUREMPLOYEE.Visible = True

            Label10.Visible = False
            txt_EBeam.Visible = False

            lbl_caption_dc_receipt_metres.Visible = True
            lbl_caption_dc_receipt_metres.Font = New Font("Calibri", 9, FontStyle.Bold)
            txt_Dc_receipt_mtrs.Visible = True

            txt_Dc_receipt_mtrs.Left = txt_EBeam.Left
            txt_Dc_receipt_mtrs.Width = txt_EBeam.Width

            lbl_ReceiptMeters_Caption.Top = lbl_ReceiptMeters_Caption.Top - 5
            lbl_ReceiptMeters_Caption.Size = New Size(94, 34)

            lbl_ReceiptMeters_Caption.Text = "Receipt Meters" & Chr(13) & "(Approved)"
            lbl_ReceiptMeters_Caption.Font = New Font("Calibri", 9, FontStyle.Bold)

            txt_ReceiptMeters.Left = txt_ReceiptMeters.Left + 35 ' 121
            txt_ReceiptMeters.Width = txt_ReceiptMeters.Width - 35 ' 180

            txt_ReceiptMeters.Enabled = False

            lbl_Dc_receipt_pcs_Caption.Visible = True
            txt_Dc_receipt_pcs.Visible = True

            txt_Dc_receipt_pcs.BackColor = Color.White
            txt_Dc_receipt_pcs.Left = txt_Quantity.Left
            txt_Dc_receipt_pcs.Top = txt_Quantity.Top
            txt_Dc_receipt_pcs.Width = txt_Quantity.Width


            lbl_NoOfPcs_Caption.Text = "Receipt Pcs" & Chr(13) & "(Approved)"
            lbl_NoOfPcs_Caption.Font = New Font("Calibri", 9, FontStyle.Bold)
            lbl_NoOfPcs_Caption.Top = lbl_NoOfPcs_Caption.Top - 10

            txt_NoOfPcs.Enabled = False
            txt_Dc_receipt_pcs.Enabled = True
            txt_NoOfPcs.Width = 60

            dgv_Details.Enabled = False

            If Val(EntFnYrCode) <= 18 Then
                chk_No_Weaving_Wages_Bill.Visible = True
                cbo_Cloth.Width = 308
            End If

        Else

            'lbl_ReceiptMeters_Caption.Text = "Meters"
            'lbl_ReceiptMeters_Caption.Location = New Point(12, 195)
            'lbl_ReceiptMeters_Caption.Size = New Size(47, 15)

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            dgv_Details.Columns(0).ReadOnly = False
            dgv_Details.Columns(0).DefaultCellStyle.Alignment = 0
            dgv_Details.Columns(2).Visible = True

            dgv_Details.Columns(0).Width = dgv_Details.Columns(0).Width - (dgv_Details.Columns(2).Width \ 2)
            dgv_Details.Columns(1).Width = dgv_Details.Columns(1).Width - (dgv_Details.Columns(2).Width \ 2)

            dgv_Details.AllowUserToAddRows = False

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
            pnl_Weaver_Stock_Display.Visible = True
        Else
            pnl_Weaver_Stock_Display.Visible = False
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1490" Then '---- LAKSHMI  SARASWATHI EXPORTS (THIRUCHENCODE)

            lbl_PcsNoFrom_Caption.Visible = False
            txt_PcsNoFrom.Visible = False

            lbl_PcsNoTo_Caption.Visible = False
            lbl_PcsNoTo.Visible = False

            cbo_LoomNo.Visible = False
            lbl_LoomNo_Caption.Visible = False
            lbl_LoomNo_Asterisk.Visible = False

            dgv_Details.Columns(0).ReadOnly = False
            dgv_Details.Columns(0).DefaultCellStyle.Alignment = 0

            dgv_Details.AllowUserToAddRows = False

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1549" Then '---- GIRI FABRICS (VAGARAYAMPALAYAM)

            lbl_ClothReceipt_Quantity_Caption.Text = "Quantity (Dhothi)"
            lbl_ClothReceipt_Quantity_Caption.Visible = True
            txt_Quantity.Text = ""
            txt_Quantity.Visible = True
            txt_Quantity.Enabled = False
            txt_NoOfPcs.Width = 60

            dgv_Details.AllowUserToAddRows = False

            dgv_Details.Columns(0).ReadOnly = True
            dgv_Details.Columns(0).DefaultCellStyle.Alignment = 0
            dgv_Details.Columns(1).ReadOnly = True
            dgv_Details.Columns(2).Visible = True
            dgv_Details.Columns(3).Visible = True
            dgv_Details.Columns(4).Visible = False
            dgv_Details.Columns(5).Visible = True
            dgv_Details.Columns(6).Visible = True
            dgv_Details.Columns(7).Visible = True
            dgv_Details.Columns(7).ReadOnly = False

            dgv_Details.Columns(0).Width = 40
            dgv_Details.Columns(1).Width = 70
            dgv_Details.Columns(2).Width = 55
            dgv_Details.Columns(3).Width = 160
            dgv_Details.Columns(4).Width = 75
            dgv_Details.Columns(5).Width = 60
            dgv_Details.Columns(6).Width = 60
            dgv_Details.Columns(7).Width = 55

            dgv_Details.ColumnHeadersHeight = 35

            Dim vWDTH As Single = 0

            vWDTH = 0
            For i = 0 To dgv_Details.Columns.Count - 1
                dgv_Details_Total.Columns(i).Width = dgv_Details.Columns(i).Width
                dgv_Details_Total.Columns(i).Visible = dgv_Details.Columns(i).Visible

                If dgv_Details.Columns(i).Visible = True Then
                    vWDTH = vWDTH + dgv_Details.Columns(i).Width
                End If
            Next i
            dgv_Details.Width = vWDTH + 25
            dgv_Details.AllowUserToAddRows = False
            dgv_Details_Total.Width = dgv_Details.Width

            pnl_Back.Width = dgv_Details.Left + dgv_Details.Width + 15
            Me.Width = pnl_Back.Left + pnl_Back.Width + 20
            Me.Left = Me.Left - 70

            Common_Procedures.Update_SizedPavu_BeamNo_for_Selection(con)

        End If

        btn_Bobin.Visible = False
        dgv_BobinDetails.Visible = False
        dgv_KuriDetails.Visible = False

        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
            dgv_BobinDetails.Visible = True
            dgv_KuriDetails.Visible = True
            btn_Bobin.Visible = True
        End If


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        cbo_Godown_StockIN.Visible = False
        lbl_Godown_Caption.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1075" Then '---- JR TEX ( STANLEY ) ( MS FABRICS ) (SULUR)   (or)   J.R TEX ( STANLEY ) ( M.S FABRICS ) (SULUR)
            cbo_StockOff.Visible = True
            lbl_StockOff_Caption.Visible = True
            lbl_StockOff_Caption.Left = lbl_Godown_Caption.Left
            lbl_StockOff_Caption.Top = lbl_Godown_Caption.Top
            cbo_StockOff.Left = cbo_Godown_StockIN.Left
            cbo_StockOff.Top = cbo_Godown_StockIN.Top
            cbo_StockOff.Width = cbo_Weaver.Width
            cbo_StockOff.BackColor = Color.White

        ElseIf Common_Procedures.settings.Multi_Godown_Status = 1 Then
            cbo_Godown_StockIN.Visible = True
            lbl_Godown_Caption.Visible = True

            'txt_Remarks.Width = 211

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1019" Then
            '    lbl_Godown_Caption.Left = lbl_StockOff_Caption.Left
            '    cbo_Godown_StockIN.Left = cbo_StockOff.Left
            '    cbo_Godown_StockIN.Width = cbo_StockOff.Width
            'End If

        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1516" Then
            cbo_Delivery_Purpose.Visible = True
            lbl_DeliveryPurpose_Caption.Visible = True
            cbo_Processed_Cloth.Visible = True
            lbl_ProcessedCloth_Caption.Visible = True

        Else

            cbo_Delivery_Purpose.Visible = False
            lbl_DeliveryPurpose_Caption.Visible = False
            cbo_Processed_Cloth.Visible = False
            lbl_ProcessedCloth_Caption.Visible = False


            lbl_Rate_Caption.Left = lbl_ProcessedCloth_Caption.Left
            txt_Rate.Left = cbo_Processed_Cloth.Left
            txt_Rate.Width = cbo_Processed_Cloth.Width
            lbl_Amount_Caption.Left = lbl_DeliveryPurpose_Caption.Left
            lbl_Amount.Left = cbo_Delivery_Purpose.Left
            lbl_Amount.Width = cbo_Delivery_Purpose.Width


        End If

        pnl_PartyDc_Image.Visible = False
        pnl_PartyDc_Image.Top = (Me.Height - pnl_PartyDc_Image.Height) \ 2
        pnl_PartyDc_Image.Left = (Me.Width - pnl_PartyDc_Image.Width) \ 2
        pnl_PartyDc_Image.BringToFront()

        pnl_DriverDetails.Visible = False
        pnl_DriverDetails.Top = (Me.Height - pnl_DriverDetails.Height) \ 2
        pnl_DriverDetails.Left = (Me.Width - pnl_DriverDetails.Width) \ 2
        pnl_DriverDetails.BringToFront()

        pnl_io_selection.Visible = False
        pnl_io_selection.Left = (Me.Width - pnl_io_selection.Width) \ 2
        pnl_io_selection.Top = (Me.Height - pnl_io_selection.Height) \ 2
        pnl_io_selection.BringToFront()

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



        chk_ReturnStatus.Visible = False


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '-------------------Brt Textile
            chk_ReturnStatus.Visible = True
        Else
            chk_ReturnStatus.Visible = False
        End If



        pnl_Bobin.Visible = False
        pnl_Bobin.Left = (Me.Width - pnl_Bobin.Width) \ 2
        pnl_Bobin.Top = (Me.Height - pnl_Bobin.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = ((Me.Height - pnl_Print.Height) \ 2) - 100
        pnl_Print.BringToFront()

        pnl_Attachments.Visible = False
        pnl_Attachments.Left = (Me.Width - pnl_Attachments.Width) \ 2
        pnl_Attachments.Top = ((Me.Height - pnl_Attachments.Height) \ 2) - 50
        pnl_Attachments.BringToFront()


        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Dc_receipt_pcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Dc_receipt_mtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaver.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_RecNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_RecNoTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfPcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsNoFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ReceiptMeters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Quantity.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_StockOff.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_NoStockPosting.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_No_Weaving_Wages_Bill.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Purchase.GotFocus, AddressOf ControlGotFocus
        AddHandler Txt_NoOfBundles.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SupervisorName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DriverPhNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DriverName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Godown_StockIN.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_UNLOADEDBYOUREMPLOYEE.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EWayBillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Delivery_Purpose.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Processed_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_BeamNo1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_BeamNo2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaver_Cloth_SufixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weaver_Cloth_PrefixNo.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_EWayBillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Dc_receipt_pcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Dc_receipt_mtrs.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_StockOff.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weaver.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_RecNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_RecNoTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfPcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsNoFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ReceiptMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Quantity.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_NoStockPosting.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_No_Weaving_Wages_Bill.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Purchase.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_UNLOADEDBYOUREMPLOYEE.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SupervisorName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DriverPhNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DriverName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Godown_StockIN.LostFocus, AddressOf ControlLostFocus
        AddHandler Txt_NoOfBundles.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Delivery_Purpose.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Processed_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_BeamNo1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_BeamNo2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weaver_Cloth_SufixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weaver_Cloth_PrefixNo.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Dc_receipt_pcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Dc_receipt_mtrs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EBeam.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler chk_UNLOADEDBYOUREMPLOYEE.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_RecNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_RecNoTo.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_NoOfPcs.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_PDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LotNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ReceiptMeters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Quantity.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Txt_NoOfBundles.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Dc_receipt_pcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Dc_receipt_mtrs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EBeam.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler chk_UNLOADEDBYOUREMPLOYEE.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_RecNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_RecNoTo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_NoOfPcs.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_PDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LotNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ReceiptMeters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Quantity.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Txt_NoOfBundles.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler chk_ReturnStatus.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_ReturnStatus.LostFocus, AddressOf ControlLostFocus

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False

        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True

        new_record()

    End Sub

    Private Sub Weaver_Cloth_Receipt_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Weaver_Cloth_Receipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_io_selection.Visible = True Then
                    btn_Close_io_Selection_Click(sender, e)
                    Exit Sub
                ElseIf pnl_DriverDetails.Visible Then
                    btn_Close_DriverDetails_Click(sender, e)
                ElseIf pnl_Print.Visible = True Then
                    btn_Close_Print_Click(sender, e)
                    Exit Sub
                ElseIf pnl_PartyDc_Image.Visible = True Then
                    btn_Close_PartyDc_Image_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Attachments.Visible = True Then
                    btn_close_Attachments_Click(sender, e)
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

                    Dim vFIRSTCOLNO As Integer
                    Dim vLASTCOLNO As Integer
                    If .Columns(7).Visible = True Then '---- GIRI FABRICS (VAGARAYAMPALAYAM)
                        vFIRSTCOLNO = 2
                        vLASTCOLNO = 7

                    ElseIf .Columns(2).Visible = True Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
                        vFIRSTCOLNO = 0
                        vLASTCOLNO = 2

                    ElseIf .Columns(0).ReadOnly = False Then '---- LAKSHMI  SARASWATHI EXPORTS (THIRUCHENCODE)
                        vFIRSTCOLNO = 0
                        vLASTCOLNO = 1

                    Else
                        vFIRSTCOLNO = 1
                        vLASTCOLNO = 1

                    End If


                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= vLASTCOLNO Then
                            If .CurrentCell.RowIndex = .RowCount - 1 And (Trim(.CurrentRow.Cells(1).Value) = "" And Trim(.CurrentRow.Cells(1).Value) = "0") Then

                                If txt_ReceiptMeters.Enabled And txt_ReceiptMeters.Visible Then
                                    txt_ReceiptMeters.Focus()
                                Else
                                    cbo_Transport.Focus()
                                End If

                            Else

                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    If txt_ReceiptMeters.Enabled And txt_ReceiptMeters.Visible Then
                                        txt_ReceiptMeters.Focus()
                                    Else
                                        cbo_Transport.Focus()
                                    End If

                                Else
                                    '.Rows.Add()
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(vFIRSTCOLNO)

                                End If

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= vFIRSTCOLNO Then
                            If .CurrentCell.RowIndex = 0 Then
                                If txt_PcsNoFrom.Visible And txt_PcsNoFrom.Enabled Then
                                    txt_PcsNoFrom.Focus()
                                Else
                                    txt_NoOfPcs.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(vLASTCOLNO)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 7 Then
                            If .Columns(4).Visible = True Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(4)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)
                            End If

                        ElseIf .CurrentCell.ColumnIndex = 5 Then
                            If .Columns(4).Visible = True Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 2)
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
            Me.Text = lbl_Heading.Text
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
                            Me.Text = lbl_Heading.Text & "   -   " & Trim(dt1.Rows(0)(1).ToString)
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
        Dim vSELC_LOTCODE As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, New_Entry, Me, con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", NewCode, "Weaver_Cloth_Receipt_Date", "(Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "Weaver_Cloth_Receipt_Head", "Verified_Status", "(Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)
        vSELC_LOTCODE = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

        Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and  (Weaver_Wages_Code <> '' or Weaver_IR_Wages_Code <> '')", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Wages Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and  Weaver_Piece_Checking_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Piece checking prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then

            Da = New SqlClient.SqlDataAdapter("select count(*) from Lot_Allotment_Details Where Lotcode_ForSelection = '" & Trim(vSELC_LOTCODE) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already Lot Allotment prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select count(*) from Lot_Approved_Head Where Lotcode_ForSelection = '" & Trim(vSELC_LOTCODE) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already Lot approval prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_App_PieceChecking_Details Where LotCode_Selection = '" & Trim(vSELC_LOTCODE) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already Piece Folding meter entered in mobile app", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_App_Piece_Defect_Details Where LotCode_Selection = '" & Trim(vSELC_LOTCODE) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already Piece defects details entered in mobile app", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_App_PieceReceipt_Details Where LotCode_Selection = '" & Trim(vSELC_LOTCODE) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already Piece Receipt details entered in mobile app", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            Dt1.Clear()

        End If


        cmd.Connection = con

        cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
        cmd.ExecuteNonQuery()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Cloth_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , StockOff_IdNo, Cloth_IdNo,                             ClothType_IdNo                                                                                                                                                 , Folding ) " &
                                      " Select                               'CLOTH'   , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, StockOff_IdNo, Cloth_IdNo, (CASE WHEN Meters_Type1 <> 0 THEN 1  WHEN Meters_Type2 <> 0 THEN 2 WHEN Meters_Type3 <> 0 THEN 3 WHEN Meters_Type4 <> 0 THEN 4 WHEN Meters_Type5 <> 0 THEN 5 ELSE 0 END  ) as ClothtypeIDNO, Folding from Stock_Cloth_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Weaver_Cloth_Receipt_head", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Weaver_ClothReceipt_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Weaver_ClothReceipt_Piece_Details", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Piece_No,Main_PieceNo,PieceNo_OrderBy,ReceiptMeters_Receipt,Receipt_Meters, Create_Status ,StockOff_IdNo,WareHouse_IdNo", "Sl_No", "Weaver_ClothReceipt_Code, For_OrderBy, Company_IdNo, Weaver_ClothReceipt_No, Weaver_ClothReceipt_RefNo, Weaver_ClothReceipt_Date, Ledger_Idno", trans)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WFRGT) & Trim(NewCode), trans)

            If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then

                cmd.CommandText = "Delete from [Weaver_ClothReceipt_App_PieceChecking_Details] where [LotCode_Selection] = '" & Trim(vSELC_LOTCODE) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from [Weaver_ClothReceipt_App_Piece_Defect_Details] where [LotCode_Selection] = '" & Trim(vSELC_LOTCODE) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from [Weaver_ClothReceipt_App_PieceReceipt_Details] where [LotCode_Selection] = '" & Trim(vSELC_LOTCODE) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from [Lot_Allotment_Details] where [Lotcode_ForSelection] = '" & Trim(vSELC_LOTCODE) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from [LotAllotment_Head] where [Lotcode_forselection] = '" & Trim(vSELC_LOTCODE) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from [Lot_Approved_Head] where [Lotcode_ForSelection] = '" & Trim(vSELC_LOTCODE) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from [FabricPurchase_Weaver_Lot_Head] where [Creating_DOC_Ref_Code] = '" & Pk_Condition & NewCode & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Textile_Processing_Delivery_Head where ClothProcess_Delivery_Code = '" & Pk_Condition & NewCode & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Textile_Processing_Delivery_Details where Cloth_Processing_Delivery_Code = '" & Pk_Condition & NewCode & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Weaver_ClothReceipt_Attachment_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Cloth_Stock) = 1 Then
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

        If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus() Else cbo_Weaver.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select cloth_name from cloth_head order by cloth_name", con)
            da.Fill(dt2)
            cbo_Filter_Cloth.DataSource = dt2
            cbo_Filter_Cloth.DisplayMember = "cloth_name"



            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Cloth.Text = ""
            txt_Filter_RecNo.Text = ""
            txt_Filter_RecNoTo.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_Cloth.SelectedIndex = -1


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
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where  Receipt_Type = 'W' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(EntFnYrCode) & "' and " & Other_Condition & " Order by for_Orderby, Weaver_ClothReceipt_RefNo", con)
            'da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(EntFnYrCode) & "' and Receipt_Type = 'W' and Weaver_ClothReceipt_Code NOT LIKE 'GWEWA-%' Order by for_Orderby, Weaver_ClothReceipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where  Receipt_Type = 'W' and for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(EntFnYrCode) & "' and  " & Other_Condition & " Order by for_Orderby, Weaver_ClothReceipt_RefNo", con)
            'da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(EntFnYrCode) & "' and  Receipt_Type = 'W'  and Weaver_ClothReceipt_Code NOT LIKE 'GWEWA-%' Order by for_Orderby, Weaver_ClothReceipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where  Receipt_Type = 'W' and for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(EntFnYrCode) & "' and  " & Other_Condition & " Order by for_Orderby desc, Weaver_ClothReceipt_RefNo desc", con)
            'da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(EntFnYrCode) & "' and  Receipt_Type = 'W'  and Weaver_ClothReceipt_Code NOT LIKE 'GWEWA-%' Order by for_Orderby desc, Weaver_ClothReceipt_No desc", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where  Receipt_Type = 'W' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(EntFnYrCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Weaver_ClothReceipt_RefNo desc", con)
            'da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(EntFnYrCode) & "' and  Receipt_Type = 'W'  and Weaver_ClothReceipt_Code NOT LIKE 'GWEWA-%' Order by for_Orderby desc, Weaver_ClothReceipt_No desc", con)
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
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then
                lbl_RefNo.Text = GetNewNo()
            Else
                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), EntFnYrCode)
            End If

            'lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", "For_OrderBy", "(Receipt_Type = 'W'  and Weaver_ClothReceipt_Code NOT LIKE 'GWEWA-%' )", Val(lbl_Company.Tag), EntFnYrCode)

            lbl_RefNo.ForeColor = Color.Red

            If dtp_Date.Enabled = True Then
                msk_date.Text = Date.Today.ToShortDateString
                da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, Weaver_ClothReceipt_RefNo desc", con)
                dt1 = New DataTable
                da.Fill(dt1)
                If dt1.Rows.Count > 0 Then
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1204" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1267" Then
                        If dt1.Rows(0).Item("Loom_Type").ToString <> "" Then cbo_LoomType.Text = dt1.Rows(0).Item("Loom_Type").ToString
                    End If
                    If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                        If dt1.Rows(0).Item("Weaver_ClothReceipt_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
                    End If
                    If dt1.Rows(0).Item("WareHouse_IdNo").ToString <> "" Then
                        If Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString) <> 0 Then cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))
                    End If
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then
                        If dt1.Rows(0).Item("Cloth_IdNo").ToString <> "" Then cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then
                        If dt1.Rows(0).Item("Vehicle_No").ToString <> "" Then cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                    End If
                    If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False

                    If txt_Folding_Perc.Visible = True Then
                        If IsDBNull(dt1.Rows(0).Item("Folding_Receipt").ToString) = False Then
                            txt_Folding_Perc.Text = dt1.Rows(0).Item("Folding_Receipt").ToString
                            If Val(txt_Folding_Perc.Text) = 0 Then txt_Folding_Perc.Text = 100
                        End If
                    End If

                    If dt1.Rows(0).Item("Weaver_ClothReceipt_PrefixNo").ToString <> "" Then txt_Weaver_Cloth_PrefixNo.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_PrefixNo").ToString
                    If dt1.Rows(0).Item("Weaver_ClothReceipt_SuffixNo").ToString <> "" Then cbo_Weaver_Cloth_SufixNo.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_SuffixNo").ToString

                End If
                dt1.Clear()
            End If

            If msk_date.Enabled And msk_date.Visible Then
                msk_date.Focus()
                msk_date.SelectionStart = 0
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        dt1.Dispose()
        da.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim movno2 As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Lot.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where  Receipt_Type = 'W' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(RecCode) & "' and " & Other_Condition, con)
            'Da = New SqlClient.SqlDataAdapter("select Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            Da = New SqlClient.SqlDataAdapter("select Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where  Receipt_Type = 'L' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(RecCode) & "' and " & Other_Condition, con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno2 = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno2 = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(movno2) <> 0 Then
                MessageBox.Show("Lot No. already enterd in InHouse-Doffing", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show("Lot No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim movno2 As String
        Dim RecCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Lot No.", "FOR NEW REC INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where Receipt_Type = 'W' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(RecCode) & "' and " & Other_Condition, con)
            'Da = New SqlClient.SqlDataAdapter("select Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            Da = New SqlClient.SqlDataAdapter("select Weaver_ClothReceipt_RefNo from Weaver_Cloth_Receipt_Head where Receipt_Type = 'L' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(RecCode) & "' and " & Other_Condition, con)
            'Da = New SqlClient.SqlDataAdapter("select Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno2 = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno2 = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(movno2) <> 0 Then
                MessageBox.Show("LotNo already entered in InHouse-Doffing", "DOES NOT INSERT NEW REC...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Lot No", "DOES NOT INSERT NEW REC...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Pro_Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotRcptPcs As Single, vTotRcptMtrs As Double
        Dim WftCnt_ID As Integer = 0
        Dim EntID As String = 0
        Dim Dup_PcNo As String = ""
        Dim WagesCode As String = ""
        Dim PcsChkCode As String = ""
        Dim ClthName As String = ""
        Dim Nr As Integer = 0
        Dim ECnt_ID As Integer
        Dim KuriCnt_ID As Integer
        Dim StkDelvTo_ID As Integer = 0, StkRecFrm_ID As Integer = 0
        Dim Led_type As String = ""
        Dim vStkOf_Pos_IdNo As Integer = 0, StkOff_ID As Integer = 0
        Dim PavuStock_In As String
        Dim clthStock_In As String
        Dim YrnCons_For As String = ""
        Dim mtrspcs As Single
        Dim clthmtrspcs As Single
        Dim clthPcs_Mtr As Single
        Dim clthtype1_Mtr As String = 0
        Dim Purc_STS As Integer = 0
        Dim NoStkPos_Sts As Integer = 0
        Dim OurOrd_No As String = ""
        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
        Dim vGod_ID As Integer = 0
        Dim vDelv_ID As Integer = 0, vRec_ID As Integer = 0
        Dim Vchk_UNLOADED As Integer = 0
        Dim NoWeaWages_Bill_Sts As Integer = 0
        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""
        Dim vDat1 As Date = #1/1/2000#
        Dim vDat2 As Date = #2/2/2000#
        Dim vLOMNO As String = ""
        Dim vWIDTHTYPE As String = ""
        Dim vSELC_LOTCODE As String
        Dim vDC_RECMTRS As String = 0, vDC_RECPCS As String = 0, vACT_RECPCS As String = 0
        Dim vMAINPCSNO As String
        Dim vLEDShtNm As String
        Dim vPCSCODE_FORSELECTION As String
        Dim vCLO_Wgt_Mtr As String = 0
        Dim vCLO_RdSp As String = 0
        Dim vCLO_Pick As String = 0, vCLO_WIDTH As String = 0
        Dim vCLO_Weft As String = 0
        Dim vGST_Tax_Inv_Sts As Integer = 0
        Dim Del_Purpose_IdNo As Integer
        Dim Lot_IdNo As Integer = 0
        Dim Del_Led_Type As String = ""
        Dim vSETCD1 As String = "", vBMNO1 As String = ""
        Dim vSETCD2 As String = "", vBMNO2 As String = ""
        Dim vNEGATIVE_YARN_STOCK_STS As Boolean, vNEGATIVE_PAVU_STOCK_STS As Boolean
        Dim vCloRec As String
        Dim vRETURN_STS As Integer = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If msk_date.Visible = True Then

            If Trim(msk_date.Text) <> "" Then
                If Trim(msk_date.Text) <> "-  -" Then
                    If IsDate(msk_date.Text) = True Then
                        vDat1 = Convert.ToDateTime(msk_date.Text)
                    End If
                End If
            End If

            If Trim(dtp_Date.Text) <> "" Then
                If IsDate(dtp_Date.Text) = True Then
                    vDat2 = dtp_Date.Value.Date
                End If
            End If

            If IsDate(vDat1) = True And IsDate(vDat2) = True Then

                If DateDiff(DateInterval.Day, vDat1, vDat2) <> 0 Then

                    msk_date.Focus()

                    MessageBox.Show("Invalid Cloth Receipt Date", "DOES NOT SHOW REPORT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End If

        End If

        If EntFnYrCode = Common_Procedures.FnYearCode Then
            If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
                Exit Sub
            End If
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
            If Trim(txt_PDcNo.Text) = "" Then
                MessageBox.Show("Invalid Party DcNo..", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_PDcNo.Enabled And txt_PDcNo.Visible Then txt_PDcNo.Focus()
                Exit Sub
            End If
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, New_Entry, Me, con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", NewCode, "Weaver_ClothReceipt_Date", "(Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_ClothReceipt_RefNo desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "Weaver_Cloth_Receipt_Head", "Verified_Status", "(Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        Vchk_UNLOADED = 0
        If chk_UNLOADEDBYOUREMPLOYEE.Checked = True Then Vchk_UNLOADED = 1

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus()
            Exit Sub
        End If

        vLEDShtNm = ""
        If Led_ID <> 0 Then
            vLEDShtNm = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_ShortName", "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")")
            vLEDShtNm = Trim(UCase(vLEDShtNm))
        End If

        vGod_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Godown_StockIN.Text)
        If cbo_Godown_StockIN.Visible = True Then
            If vGod_ID = 0 Then
                MessageBox.Show("Invalid Fabric Godown Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Godown_StockIN.Enabled And cbo_Godown_StockIN.Visible Then cbo_Godown_StockIN.Focus()
                Exit Sub
            End If
        End If

        If Len(Trim(cbo_Delivery_Purpose.Text)) > 0 And cbo_Delivery_Purpose.Visible Then
            Del_Purpose_IdNo = Common_Procedures.Process_NameToIdNo(con, cbo_Delivery_Purpose.Text)
        End If

        If Len(Trim(cbo_Processed_Cloth.Text)) > 0 And cbo_Processed_Cloth.Visible Then
            Pro_Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Processed_Cloth.Text)
        End If

        Del_Led_Type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Godown_StockIN.Text))

        If vGod_ID <> 0 And Del_Led_Type <> "GODOWN" And Del_Purpose_IdNo = 0 Then
            MessageBox.Show("Delivery Purpose (Process Name) is Manadatory when Delivery Location is not Own Godown.", "Process Name", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Del_Purpose_IdNo <> 0 And Pro_Clo_ID = 0 Then
            MessageBox.Show("Procesed Cloth Name is Manadatory when Delivery Location is not Own Godown.", "Process Name", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If vGod_ID = 0 Then vGod_ID = Common_Procedures.CommonLedger.Godown_Ac

        If txt_Folding_Perc.Visible = True Then
            If Val(txt_Folding_Perc.Text) = 0 Then
                txt_Folding_Perc.Text = 100
            End If

        Else
            txt_Folding_Perc.Text = 100

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then

            Da = New SqlClient.SqlDataAdapter("select a.Weaver_LoomType from ledger_head a where a.Ledger_IdNo = " & Str(Val(Led_ID)), con)
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                cbo_LoomType.Text = Dt2.Rows(0).Item("Weaver_LoomType")
            Else
                cbo_LoomType.Text = ""
            End If
            Dt2.Clear()

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Or Trim(Common_Procedures.settings.CustomerCode) = "1204" Then
            If Trim(cbo_LoomType.Text) = "" Then
                MessageBox.Show("Invalid Loom Type?", "DOESN'T SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomType.Visible And cbo_LoomType.Enabled Then cbo_LoomType.Focus()
                Exit Sub
            End If

        Else

            If Trim(cbo_LoomType.Text) = "" Then
                cbo_LoomType.Text = "POWERLOOM"
            End If

        End If

        If Trim(cbo_LoomType.Text) = "" Then
            MessageBox.Show("Invalid Loom Type?", "DOESN'T SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_LoomType.Visible And cbo_LoomType.Enabled Then cbo_LoomType.Focus()
            Exit Sub
        End If

        cbo_LoomType.Text = Replace(Trim(UCase(cbo_LoomType.Text)), "  ", "")
        cbo_LoomType.Text = Replace(Trim(UCase(cbo_LoomType.Text)), " ", "")


        If lbl_OrderNo.Visible = True Then

            If Trim(lbl_OrderCode.Text) <> "" Then

                If Led_ID <> 0 Then
                    Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Own_Order_Head a INNER JOIN Own_order_Weaving_Details b ON a.Own_Order_Code =b.Own_Order_Code where a.Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "' and  b.Ledger_idno = " & Str(Val(Led_ID)), con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        OurOrd_No = Dt1.Rows(0).Item("Order_No").ToString
                    End If
                    Dt1.Clear()
                End If
                If Trim(OurOrd_No) <> Trim(lbl_OrderNo.Text) Then
                    MessageBox.Show("Invalid Mismatch Of Order No for this Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus()
                    Exit Sub
                End If

            End If
        End If



        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If Clo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        If Trim(txt_PDcNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)
            Da = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(Led_ID)) & " and Party_DcNo = '" & Trim(txt_PDcNo.Text) & "' and Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_ClothReceipt_Code <> '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Party Dc.No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_PDcNo.Enabled And txt_PDcNo.Visible Then txt_PDcNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If

        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        If EdsCnt_ID = 0 Then
            MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
            Exit Sub
        End If

        WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)
        If WftCnt_ID = 0 Then
            MessageBox.Show("Invalid Weft Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        Da = New SqlClient.SqlDataAdapter("select a.* from Cloth_EndsCount_Details a where a.Cloth_Idno = " & Str(Val(Clo_ID)) & " and a.EndsCount_IdNo =  " & Str(Val(EdsCnt_ID)), con)
        Dt2 = New DataTable
        Da.Fill(Dt2)
        If Dt2.Rows.Count <= 0 Then
            MessageBox.Show("Invalid EndsCount to this Cloth", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
            Exit Sub
        End If
        Dt2.Clear()

        vWIDTHTYPE = ""
        vLOMNO = ""
        Lm_ID = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1132" Then

            If Trim(UCase(cbo_LoomType.Text)) = "AUTOLOOM" Or Trim(UCase(cbo_LoomType.Text)) = "AUTO LOOM" Then

                If cbo_WidthType.Enabled = True And cbo_WidthType.Visible = True Then

                    vWIDTHTYPE = Trim(cbo_WidthType.Text)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1204" Then '------KOHINOOR TEXTILE MILLS
                        If Trim(vWIDTHTYPE) = "" Then
                            MessageBox.Show("Invalid Width Type?", "DOESNOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            cbo_WidthType.Focus()
                            Exit Sub
                        End If

                    Else

                        If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 0 Then
                            If Trim(vWIDTHTYPE) = "" Then
                                MessageBox.Show("Invalid Width Type?", "DOESNOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                cbo_WidthType.Focus()
                                Exit Sub
                            End If
                        End If

                    End If

                End If

                If cbo_LoomNo.Enabled = True And cbo_LoomNo.Visible = True Then
                    vLOMNO = cbo_LoomNo.Text
                    Lm_ID = Common_Procedures.Loom_NameToIdNo(con, vLOMNO)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1204" Then '------KOHINOOR TEXTILE MILLS
                        If Lm_ID = 0 Then
                            MessageBox.Show("Invalid Loom No?", "DOESN'T SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            cbo_LoomNo.Focus()
                            Exit Sub
                        End If

                    Else

                        Dim NoofBeams As Integer = 0
                        NoofBeams = 0
                        If Trim(vWIDTHTYPE) <> "" Then
                            If InStr(1, Trim(UCase(vWIDTHTYPE)), "1 BEAM") > 0 Then
                                NoofBeams = 1
                            ElseIf InStr(1, Trim(UCase(vWIDTHTYPE)), "2 BEAM") > 0 Then
                                NoofBeams = 2
                            End If
                        End If

                        If NoofBeams = 0 Then
                            If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 0 Then
                                If Lm_ID = 0 Then
                                    MessageBox.Show("Invalid Loom No?", "DOESN'T SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                    cbo_WidthType.Focus()
                                    Exit Sub
                                End If
                            End If
                        End If

                    End If

                End If

            End If

        End If

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        If Trans_ID = 0 And Val(txt_Freight.Text) <> 0 Then
            MessageBox.Show("Invalid  Transport Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Transport.Enabled And cbo_Transport.Visible Then cbo_Transport.Focus()
            Exit Sub
        End If

        StkOff_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_StockOff.Text)
        If cbo_StockOff.Visible = True Then
            If StkOff_ID = 0 Then
                MessageBox.Show("Invalid Stock Off Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_StockOff.Enabled And cbo_StockOff.Visible Then cbo_StockOff.Focus()
                Exit Sub
            End If
        End If
        If StkOff_ID = 0 Then StkOff_ID = Common_Procedures.CommonLedger.OwnSort_Ac


        Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")")

        vStkOf_Pos_IdNo = 0

        If Common_Procedures.settings.CustomerCode = "1516" Then

            vStkOf_Pos_IdNo = Common_Procedures.CommonLedger.OwnSort_Ac  ' Val(Common_Procedures.CommonLedger.Godown_Ac)

        Else

            If cbo_StockOff.Visible = True Then

                vStkOf_Pos_IdNo = StkOff_ID

            Else

                If Trim(UCase(Led_type)) = "JOBWORKER" Then
                    vStkOf_Pos_IdNo = Led_ID
                Else
                    vStkOf_Pos_IdNo = Common_Procedures.CommonLedger.OwnSort_Ac  ' Val(Common_Procedures.CommonLedger.Godown_Ac)
                End If

            End If

        End If


        Dim vPCSNoFROM As String
        Dim vPCSNoTO As String


        vPCSNoFROM = ""
        vPCSNoTO = ""
        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(1).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(0).Value) = "" Then
                        MessageBox.Show("Invalid Pcs No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .CurrentCell = .Rows(i).Cells(0)
                            .Focus()
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(UCase(Dup_PcNo)), "~" & Trim(UCase(.Rows(i).Cells(0).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate Pcs No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_PcNo = Trim(Dup_PcNo) & "~" & Trim(UCase(.Rows(i).Cells(0).Value)) & "~"

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1490" Then '---- LAKSHMI  SARASWATHI EXPORTS (THIRUCHENCODE)

                        If Val(.Rows(i).Cells(0).Value) = 0 Then
                            MessageBox.Show("Invalid Pcs No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .CurrentCell = .Rows(i).Cells(0)
                                .Focus()
                            End If
                            Exit Sub
                        End If

                        If InStr(1, Trim(UCase(.Rows(i).Cells(0).Value)), "A") = 0 And InStr(1, Trim(UCase(.Rows(i).Cells(0).Value)), "B") = 0 And InStr(1, Trim(UCase(.Rows(i).Cells(0).Value)), "C") = 0 And InStr(1, Trim(UCase(.Rows(i).Cells(0).Value)), "D") = 0 And InStr(1, Trim(UCase(.Rows(i).Cells(0).Value)), "E") = 0 And InStr(1, Trim(UCase(.Rows(i).Cells(0).Value)), "F") = 0 Then
                            MessageBox.Show("Invalid Pcs No - Should Contain A (or) B", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .CurrentCell = .Rows(i).Cells(0)
                                .Focus()
                            End If
                            Exit Sub
                        End If

                    End If

                    If txt_PcsNoFrom.Visible = False And lbl_PcsNoTo.Visible = False Then

                        If Val(vPCSNoFROM) = 0 Then vPCSNoFROM = Val(.Rows(i).Cells(0).Value)
                        vPCSNoTO = Val(.Rows(i).Cells(0).Value)

                    End If

                End If

            Next

        End With


        If cbo_Sales_OrderCode_forSelection.Visible = True Then

            If Trim(cbo_Sales_OrderCode_forSelection.Text) = "" Then
                MessageBox.Show("Invalid " & lbl_Sales_OrderCode_forSelection_Caption.Text & Chr(13) & "Select {" & lbl_Sales_OrderCode_forSelection_Caption.Text & "} in this Cloth Receipt", "DOES Not SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Sales_OrderCode_forSelection.Enabled And cbo_Sales_OrderCode_forSelection.Visible Then
                    cbo_Sales_OrderCode_forSelection.Focus()
                Else
                    If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus() Else msk_date.Focus()
                End If
                Exit Sub
            End If

            Da = New SqlClient.SqlDataAdapter("Select a.ClothSales_Order_Code from ClothSales_Order_Details a, ClothSales_Order_Head b Where a.Cloth_Idno = " & Str(Val(Clo_ID)) & " And b.ClothSales_OrderCode_forSelection = '" & Trim(cbo_Sales_OrderCode_forSelection.Text) & "' and a.ClothSales_Order_Code = b.ClothSales_Order_Code", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count <= 0 Then
                MessageBox.Show("Invalid Cloth Name  {" & Trim(cbo_Cloth.Text) & "} " & Chr(13) & "This {Cloth Name} does not belong to this Sales Order Indent No.", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Cloth.Enabled And cbo_Cloth.Visible Then
                    cbo_Cloth.Focus()
                ElseIf cbo_Sales_OrderCode_forSelection.Enabled And cbo_Sales_OrderCode_forSelection.Visible Then
                    cbo_Sales_OrderCode_forSelection.Focus()
                Else
                    If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus() Else msk_date.Focus()
                End If
                Exit Sub
            End If
            Dt1.Clear()

        End If


        If txt_PcsNoFrom.Visible = False And lbl_PcsNoTo.Visible = False Then
            txt_PcsNoFrom.Text = Val(vPCSNoFROM)
            lbl_PcsNoTo.Text = Val(vPCSNoTO)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            If Val(txt_Dc_receipt_pcs.Text) > 0 Then
                If Val(txt_PcsNoFrom.Text) = 0 Then
                    MessageBox.Show("Invalid Pcs From", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If txt_PcsNoFrom.Enabled And txt_PcsNoFrom.Visible Then txt_PcsNoFrom.Focus() Else msk_date.Focus()
                    Exit Sub
                End If
                If Val(lbl_PcsNoTo.Text) = 0 Then
                    MessageBox.Show("Invalid Pcs To", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If txt_PcsNoFrom.Enabled And txt_PcsNoFrom.Visible Then txt_PcsNoFrom.Focus() Else msk_date.Focus()
                    Exit Sub
                End If
            End If
        End If

        Total_Calculation()

        vTotRcptPcs = 0 : vTotRcptMtrs = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotRcptPcs = Val(dgv_Details_Total.Rows(0).Cells(0).Value())
            vTotRcptMtrs = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
        End If

        If Val(vTotRcptMtrs) <> 0 Then
            If Val(vTotRcptMtrs) <> Val(txt_ReceiptMeters.Text) Then
                MessageBox.Show("Mismatch of Receipt Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_ReceiptMeters.Enabled And txt_ReceiptMeters.Visible Then txt_ReceiptMeters.Focus()
                Exit Sub
            End If
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Or Trim(Common_Procedures.settings.CustomerCode) = "1282" Then
            If Trim(cbo_VehicleNo.Text) = "" Then
                MessageBox.Show("Invalid Vehicle No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_VehicleNo.Enabled And cbo_VehicleNo.Visible Then cbo_VehicleNo.Focus()
                Exit Sub
            End If
            cmd.Connection = con
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@entrydate", dtp_Date.Value.Date)

            cmd.CommandText = "select a.* from Weaver_Running_Fabrics_Head a, Weaver_Running_Fabrics_Cloth_Details b where a.Weaver_IdNo = " & Str(Val(Led_ID)) & " and b.Cloth_idno = " & Str(Val(Clo_ID)) & " and @entrydate between a.StartDate and a.EndDate and a.Weaver_Running_Fabrics_IdNo = b.Weaver_Running_Fabrics_IdNo "
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count <= 0 Then
                MessageBox.Show("Cloth name not found in Running fabric details - invalid", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
                Exit Sub
            End If
            Dt1.Clear()

            cmd.CommandText = "select a.* from Weaver_Running_Fabrics_Head a, Weaver_Running_Fabrics_EndsCount_Details b where a.Weaver_IdNo = " & Str(Val(Led_ID)) & " and b.EndsCount_idno = " & Str(Val(EdsCnt_ID)) & " and @entrydate between a.StartDate and a.EndDate and a.Weaver_Running_Fabrics_IdNo = b.Weaver_Running_Fabrics_IdNo "
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count <= 0 Then
                MessageBox.Show("Ends/Count name not found in Running fabric Ends/count details - invalid", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
                Exit Sub
            End If
            Dt1.Clear()

            cmd.CommandText = "select a.* from Weaver_Running_Fabrics_Head a, Weaver_Running_Fabrics_Count_Details b where a.Weaver_IdNo = " & Str(Val(Led_ID)) & " and b.Count_idno = " & Str(Val(WftCnt_ID)) & " and @entrydate between a.StartDate and a.EndDate and a.Weaver_Running_Fabrics_IdNo = b.Weaver_Running_Fabrics_IdNo "
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count <= 0 Then
                MessageBox.Show("Weft Count name not found in Running fabric Count details - invalid", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then

            If Trim(txt_Dc_receipt_mtrs.Text) <> "" Then

                cmd.Connection = con
                cmd.Transaction = tr
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@entrydate", dtp_Date.Value.Date)
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

                cmd.CommandText = "select * from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(Led_ID)) & " and Dc_Receipt_Meters = '" & Trim(txt_Dc_receipt_mtrs.Text) & "' AND Weaver_ClothReceipt_date = @entrydate and Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_ClothReceipt_Code <> '" & Trim(NewCode) & "'"
                Da = New SqlClient.SqlDataAdapter(cmd)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    MessageBox.Show("Duplicate Entry to this Party on this Date - Weaver_ClothReceipt_Code = " & Dt1.Rows(0).Item("Weaver_ClothReceipt_Code").ToString(), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


                    If txt_PDcNo.Enabled And txt_PDcNo.Visible Then txt_PDcNo.Focus()
                    Exit Sub
                End If
                Dt1.Clear()

            End If


        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then 'GANESH KARTHI TEXTILE
            If Trim(cbo_VehicleNo.Text) <> "" Then
                cbo_VehicleNo.Text = Common_Procedures.Vehicle_Number_Remove_Unwanted_Spaces(Trim(cbo_VehicleNo.Text))
            End If
        End If


        Purc_STS = 0
        If chk_Purchase.Checked = True Then Purc_STS = 1

        NoStkPos_Sts = 0
        If chk_NoStockPosting.Checked = True Then NoStkPos_Sts = 1

        NoWeaWages_Bill_Sts = 0
        If chk_No_Weaving_Wages_Bill.Checked = True Then NoWeaWages_Bill_Sts = 1


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1081" Then '---- S.Ravichandran Textiles (Erode)
        ConsumedPavu_Calculation()
        'End If

        ConsumedYarn_Calculation()

        If Val(lbl_ConsYarn.Text) = 0 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then

            vCLO_Wgt_Mtr = 0
            vCLO_RdSp = 0
            vCLO_Pick = 0
            vCLO_Weft = 0
            Da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name as Weft_Count, b.Resultant_Count from cloth_head a, Count_Head b Where a.cloth_idno = " & Str(Val(Clo_ID)) & " and  a.Cloth_WeftCount_IdNo = b.Count_IdNo ", con)
            Dt = New DataTable
            Da.Fill(Dt)
            If Dt.Rows.Count > 0 Then
                vCLO_Wgt_Mtr = Dt.Rows(0).Item("Weight_Meter_Weft").ToString
                vCLO_RdSp = Dt.Rows(0).Item("Cloth_ReedSpace").ToString
                vCLO_Pick = Dt.Rows(0).Item("Cloth_Pick").ToString
                vCLO_WIDTH = Dt.Rows(0).Item("Cloth_Width").ToString
                vCLO_Weft = Dt.Rows(0).Item("Resultant_Count").ToString
                If Val(vCLO_Weft) = 0 Then
                    vCLO_Weft = Dt.Rows(0).Item("Weft_Count").ToString
                End If
            End If
            Dt.Clear()

            If Val(vCLO_Wgt_Mtr) = 0 Then
                If Val(vCLO_RdSp) = 0 Or Val(vCLO_Pick) = 0 Or Val(vCLO_Weft) = 0 Then
                    MessageBox.Show("Invalid Consumed Yarn" & Chr(13) & "Invalid Weft gram in cloth master for this quality", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
                    Exit Sub
                End If
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then '----KRG TEXTILE MILLS (PALLADAM)

                If Val(vCLO_Weft) = 0 Then
                    MessageBox.Show("Invalid Weft Count" & Chr(13) & "in cloth master for this quality", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
                    Exit Sub
                End If
                If Val(vCLO_RdSp) = 0 Then
                    MessageBox.Show("Invalid ReedSpace" & Chr(13) & "in cloth master for this quality", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
                    Exit Sub
                End If
                If Val(vCLO_Pick) = 0 Then
                    MessageBox.Show("Invalid Pick" & Chr(13) & "in cloth master for this quality", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
                    Exit Sub
                End If
                If Val(vCLO_WIDTH) = 0 Then
                    MessageBox.Show("Invalid WIDTH" & Chr(13) & "in cloth master for this quality", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
                    Exit Sub
                End If

                'Dim Wgtmtr As String = ""
                'Dim NumWftCnt As Single = 0

                ''--- Consumed Yarn Formula  = "(METERS * REEDSPACE * PICK * 1.0937) / (84 * 22 * WEFT)"

                'NumWftCnt = Val(Common_Procedures.get_FieldValue(con, "count_head", "Resultant_Count", "(count_name = '" & Trim(lbl_WeftCount.Text) & "')"))
                'If Val(NumWftCnt) = 0 Then NumWftCnt = Val(lbl_WeftCount.Text)

                'Wgtmtr = 0
                'If Val(NumWftCnt) <> 0 Then
                '    Wgtmtr = (Val(vCLO_RdSp) * Val(vCLO_Pick) * 1.0937) / (84 * 22 * NumWftCnt)
                '    Wgtmtr = Format(Val(Wgtmtr), "#########0.0000")
                'End If

                'If Math.Abs(Val(vCLO_Wgt_Mtr) - Val(Wgtmtr)) > 0.05 Then
                '    MessageBox.Show("Wrong Weft gram in cloth master for this quality" & Chr(13) & "not matching with calculated value", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                '    If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
                '    Exit Sub
                'End If

            End If

        End If


        'Dim dAt As Date
        'Dim lckdt As Date

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
        '    lckdt = #12/12/2016#
        '    dAt = dtp_Date.Value.Date
        '    If DateDiff("d", lckdt, dAt) > 0 Then
        '        MessageBox.Show("Error in loading Dll's", "RECEIPT SELECTION........", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        Application.Exit()
        '    End If

        'End If


        vRETURN_STS = 0
        If chk_ReturnStatus.Checked = True Then vRETURN_STS = 1


        If txt_LotNo.Visible = True Then
            If Trim(txt_LotNo.Text) = "" Then
                MessageBox.Show("Invalid Lot No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_LotNo.Enabled And txt_LotNo.Visible Then txt_LotNo.Focus()
                Exit Sub
            End If

            If Trim(txt_LotNo.Text) <> "" Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)
                Da = New SqlClient.SqlDataAdapter("Select * from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_IdNo = " & Str(Val(Clo_ID)) & " and Lot_No = '" & Trim(txt_LotNo.Text) & "' and Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_ClothReceipt_Code <> '" & Trim(NewCode) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    MessageBox.Show("Duplicate LotNo to this Cloth", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If txt_LotNo.Enabled And txt_LotNo.Visible Then txt_LotNo.Focus()
                    Exit Sub
                End If
                Dt1.Clear()
            End If

        Else

            txt_LotNo.Text = lbl_RefNo.Text

        End If

        vCloRec = Trim(txt_Weaver_Cloth_PrefixNo.Text) & Trim(lbl_RefNo.Text) & Trim(cbo_Weaver_Cloth_SufixNo.Text)



        vDC_RECPCS = 0
        If txt_Dc_receipt_pcs.Visible = True Then
            vDC_RECPCS = txt_Dc_receipt_pcs.Text

            If Val(vDC_RECPCS) = 0 Then
                MessageBox.Show("Invalid DC Receipt pcs", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Dc_receipt_pcs.Enabled And txt_Dc_receipt_pcs.Visible Then txt_Dc_receipt_pcs.Focus()
                Exit Sub
            End If


        Else

            If Val(txt_NoOfPcs.Text) = 0 Then txt_NoOfPcs.Text = 1
            vDC_RECPCS = txt_NoOfPcs.Text
        End If

        vACT_RECPCS = txt_NoOfPcs.Text
        If Val(vACT_RECPCS) = 0 And txt_Dc_receipt_pcs.Visible = True Then
            vACT_RECPCS = Val(txt_Dc_receipt_pcs.Text)
        End If

        vDC_RECMTRS = 0
        If txt_Dc_receipt_mtrs.Visible = True Then
            vDC_RECMTRS = txt_Dc_receipt_mtrs.Text

            If Val(vDC_RECMTRS) = 0 Then
                MessageBox.Show("Invalid DC Receipt meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Dc_receipt_mtrs.Enabled And txt_Dc_receipt_mtrs.Visible Then txt_Dc_receipt_mtrs.Focus()
                Exit Sub
            End If

        Else
            vDC_RECMTRS = txt_ReceiptMeters.Text

        End If

        vGST_Tax_Inv_Sts = 0
        If chk_GSTTax_Invocie.Checked = True Then vGST_Tax_Inv_Sts = 1

        lbl_Time.Text = Format(Now, "hh:mm tt")

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            Else

                If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then
                    lbl_RefNo.Text = GetNewNo(TR)
                Else
                    lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), EntFnYrCode, tr)
                End If

                'lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), EntFnYrCode, tr)
                'lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", "For_OrderBy", "(Receipt_Type = 'W'  and Weaver_ClothReceipt_Code NOT LIKE 'GWEWA-%' )", Val(lbl_Company.Tag), EntFnYrCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            End If

            vSELC_LOTCODE = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_Code, Weaver_Wages_Code, Weaver_IR_Wages_Code from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            PcsChkCode = ""
            WagesCode = ""
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                End If
                If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                    WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
                End If
                If Trim(WagesCode) = "" Then
                    If IsDBNull(Dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString) = False Then
                        WagesCode = Dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString
                    End If
                End If
            End If
            Dt1.Clear()

            vNEGATIVE_YARN_STOCK_STS = False
            vNEGATIVE_PAVU_STOCK_STS = False


            '*************CMB BY LALITH 2025-05-31

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            '    vNEGATIVE_YARN_STOCK_STS = True
            '    vNEGATIVE_PAVU_STOCK_STS = True
            'End If

            '*************CMB BY LALITH 2025-05-31

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            Dim Lot_No As String = Val(lbl_RefNo.Text)
            Dim Lot_Code As String = lbl_Company.Tag.ToString + "/" + Lot_No.ToString + "/" + Common_Procedures.FnRange.ToString
            Dim Lot_Code_forSelection As String = Lot_No.ToString + "/" + Common_Procedures.FnYearCode.ToString + "/" + lbl_Company.Tag.ToString


            If New_Entry = True Then

                'cmd.CommandText = "Insert into Weaver_Cloth_Receipt_Head ( Receipt_Type, Weaver_ClothReceipt_Code,             Company_IdNo         ,       Weaver_ClothReceipt_No  ,                               for_OrderBy                              , Weaver_ClothReceipt_date ,           Ledger_IdNo   ,      Cloth_IdNo    ,            Lot_No             ,            Party_DcNo         ,           EndsCount_IdNo   ,          Count_IdNo        ,             empty_beam          ,           noof_pcs            ,             pcs_fromno         ,              pcs_tono        ,           ReceiptMeters_Receipt        ,  Receipt_Quantity                    ,             Receipt_Meters               ,          ConsumedYarn_Receipt       ,              Consumed_Yarn        ,         ConsumedPavu_Receipt       ,              Consumed_Pavu         ,       Loom_IdNo        ,            Width_Type     ,       Transport_IdNo      ,     Freight_Amount_Receipt   ,              Folding_Receipt        ,              Folding               ,       Total_Receipt_Pcs      ,    Total_Receipt_Meters   , BeamConsumption_Receipt, BeamConsumption_Meters, Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment, Weaver_Wages_Code, Weaver_Wages_Increment    ,            StockOff_IdNo   ,                           User_idNo      , Purchase_Status      , Our_Order_No                     , Own_Order_Code                      ,     Loom_Type                      , No_Stock_Posting_Status     ,              Driver_Name           ,               Driver_Phone_No       ,                Supervisor_Name           , Vehicle_no                           ,        WareHouse_IdNo     , No_Of_Bundles                            , Unloaded_By_Our_Employee ,    Verified_Status       ,       No_Weaving_Wages_Bill      , Weaver_IR_Wages_Code, lotcode_forSelection,                  DC_Receipt_Meters     ,                   Remarks               ,                Eway_BillNo        ,                   Rate         ,                  Amount                  ,GST_Tax_Invoice_Status             ,Delivery_Purpose_IdNo            ,Lot_IdNo)    " &
                '                    "           Values                   (     'W'     , '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @EntryDate          , " & Str(Val(Led_ID)) & "," & Val(Clo_ID) & " , '" & Trim(txt_LotNo.Text) & "', '" & Trim(txt_PDcNo.Text) & "', " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(WftCnt_ID)) & ", " & Str(Val(txt_EBeam.Text)) & ", " & Val(txt_NoOfPcs.Text) & " , " & Val(txt_PcsNoFrom.Text) & ", " & Val(lbl_PcsNoTo.Text) & ", " & Str(Val(txt_ReceiptMeters.Text)) & ", " & Str(Val(txt_Quantity.Text)) & " ,  " & Str(Val(txt_ReceiptMeters.Text)) & ", " & Str(Val(lbl_ConsYarn.Text)) & ", " & Str(Val(lbl_ConsYarn.Text)) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(Lm_ID)) & ", '" & Trim(vWIDTHTYPE) & "', " & Str(Val(Trans_ID)) & ", " & Val(txt_Freight.Text) & ", " & Val(txt_Folding_Perc.Text) & "  ,  " & Val(txt_Folding_Perc.Text) & ", " & Str(Val(vTotRcptPcs)) & ", " & Str(Val(vTotRcptMtrs)) & ",          0             ,          0            ,          ''               ,             0                  ,         ''       ,           0           ," & Str(Val(StkOff_ID)) & " , " & Val(Common_Procedures.User.IdNo) & " , " & Val(Purc_STS) & ",'" & Trim(lbl_OrderNo.Text) & "' ,    '" & Trim(lbl_OrderCode.Text) & "', '" & Trim(cbo_LoomType.Text) & "'  ,   " & Val(NoStkPos_Sts) & " ,'" & Trim(cbo_DriverName.Text) & "' , '" & Trim(cbo_DriverPhNo.Text) & "' ,  '" & Trim(cbo_SupervisorName.Text) & "' , '" & Trim(cbo_VehicleNo.Text) & "'   , " & Str(Val(vGod_ID)) & " ,  " & Val(Txt_NoOfBundles.Text) & "       ," & Val(Vchk_UNLOADED) & ", " & Val(Verified_STS) & ", " & Val(NoWeaWages_Bill_Sts) & " ,        ''            , '" & Trim(vSELC_LOTCODE) & "'," & Str(Val(vDC_RECMTRS)) & "   ,   '" & Trim(txt_Remarks.Text) & "' , '" & Trim(txt_EWayBillNo.Text) & "'  ,   " & Str(Val(txt_Rate.Text)) & " ,  " & Str(Val(lbl_Amount.Text)) & "    , " & Str(Val(vGST_Tax_Inv_Sts)) & "," & Del_Purpose_IdNo.ToString & "," & Lot_IdNo.ToString & ") "
                'cmd.ExecuteNonQuery()


                If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then

                    If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then

                        cmd.CommandText = " If not exists (Select * from Lot_Head WHERE Lot_No = '" & Lot_Code_forSelection & "') begin " &
                                          " insert into Lot_Head ([Lot_IdNo]                                      ,	[Lot_No]                      ,	[Sur_Name]                                                             ,	[Lot_Description] ,	[Lot_Main_Name]               ,	[Lot_Fn_Yr_Code]                  ,	[Auto_Created]  ,Ledger_IdNo ) " &
                                          " Values( (select isnull(max(Lot_IdNo),0)+1 from Lot_Head),'" & Lot_Code_forSelection & "','" & Common_Procedures.Remove_NonCharacters(Lot_Code_forSelection) & "' ,            ''        ,'" & Lot_Code_forSelection & "','" & Common_Procedures.FnYearCode.ToLower & "', 1                  ," & Led_ID.ToString & ") end "
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = " If exists (Select * from Lot_Head WHERE Lot_No = '" & Lot_Code_forSelection & "') begin " &
                                     " Update Lot_Head set Ledger_IdNo = " & Led_ID.ToString & " WHERE Lot_No = '" & Lot_Code_forSelection & "' end "
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "INSERT INTO [FabricPurchase_Weaver_Lot_Head] (	[FabricPurchase_Weaver_Lot_IdNo]                                                                 ,	[FabricPurchase_Weaver_Lot_No] ,	[FabricPurchase_Weaver_Lot_Code] ,	[FabricPurchase_Weaver_Lot_Code_forSelection] ,	[For_OrderBy]                                                       ,	[Sur_Name]                                            ,	[FabricPurchase_Weaver_Lot_Date] ,	[Ledger_IdNo]        ,	[Creating_DOC_Ref_Code], Cloth_IdNo                  ,Lot_IdNo)" &
                                                                  "VALUES           ((Select isnull(max([FabricPurchase_Weaver_Lot_IdNo] ),0)+1   from [FabricPurchase_Weaver_Lot_Head]) ,'" & lbl_RefNo.Text & "'         ,'" & Lot_Code & "'                   ,'" & Lot_Code_forSelection & "'                  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",'" & Common_Procedures.Remove_NonCharacters(Lot_Code) & "',   @EntryDate                   ," & Led_ID.ToString & ",'" & Pk_Condition & NewCode & "'," & Clo_ID.ToString & "," & Lot_IdNo.ToString & ")"
                        cmd.ExecuteNonQuery()


                    End If


                End If

            Else

                'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Cloth_Receipt_head", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_ClothReceipt_Code, Company_IdNo, for_OrderBy", tr)
                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Weaver_ClothReceipt_Piece_Details", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Piece_No,Main_PieceNo,PieceNo_OrderBy,ReceiptMeters_Receipt,Receipt_Meters, Create_Status ,StockOff_IdNo,WareHouse_IdNo", "Sl_No", "Weaver_ClothReceipt_Code, For_OrderBy, Company_IdNo, Weaver_ClothReceipt_No, Weaver_ClothReceipt_Date, Ledger_Idno", tr)


                'cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Receipt_Type = 'W', Weaver_ClothReceipt_date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Cloth_IdNo = " & Val(Clo_ID) & " , Lot_No = '" & Trim(txt_LotNo.Text) & "',  Party_DcNo  = '" & Trim(txt_PDcNo.Text) & "',  EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Count_IdNo = " & Str(Val(WftCnt_ID)) & ", empty_beam = " & Val(txt_EBeam.Text) & " , noof_pcs = " & Val(txt_NoOfPcs.Text) & " , pcs_fromno = " & Val(txt_PcsNoFrom.Text) & " , pcs_tono = " & Val(lbl_PcsNoTo.Text) & " , ReceiptMeters_Receipt = " & Val(txt_ReceiptMeters.Text) & ", ConsumedYarn_Receipt = " & Val(lbl_ConsYarn.Text) & ", ConsumedPavu_Receipt = " & Val(lbl_ConsPavu.Text) & ", Loom_IdNo = " & Val(Lm_ID) & ", Width_Type = '" & Trim(vWIDTHTYPE) & "', Transport_IdNo = " & Val(Trans_ID) & ", Freight_Amount_Receipt = " & Val(txt_Freight.Text) & ", Total_Receipt_Pcs = " & Str(Val(vTotRcptPcs)) & ", Total_Receipt_Meters = " & Str(Val(vTotRcptMtrs)) & " , StockOff_IdNo = " & Str(Val(StkOff_ID)) & ",  User_idNo = " & Val(Common_Procedures.User.IdNo) & " ,  Purchase_Status = " & Val(Purc_STS) & ", Our_Order_No = '" & Trim(lbl_OrderNo.Text) & "', Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "', Loom_Type =  '" & Trim(cbo_LoomType.Text) & "' , Receipt_Quantity = " & Str(Val(txt_Quantity.Text)) & " , No_Stock_Posting_Status = " & Val(NoStkPos_Sts) & " , Driver_Name = '" & Trim(cbo_DriverName.Text) & "' , Driver_Phone_No = '" & Trim(cbo_DriverPhNo.Text) & "' , Supervisor_Name = '" & Trim(cbo_SupervisorName.Text) & "' , Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' , WareHouse_IdNo = " & Str(Val(vGod_ID)) & " , No_Of_Bundles =  " & Val(Txt_NoOfBundles.Text) & ",Unloaded_By_Our_Employee=" & Val(Vchk_UNLOADED) & ",Verified_Status= " & Val(Verified_STS) & ", No_Weaving_Wages_Bill = " & Val(NoWeaWages_Bill_Sts) & " ,lotcode_forSelection='" & Trim(vSELC_LOTCODE) & "',DC_Receipt_Meters=" & Str(Val(vDC_RECMTRS)) & " , Folding_Receipt = " & Val(txt_Folding_Perc.Text) & " , Remarks =  '" & Trim(txt_Remarks.Text) & "' , Eway_BillNo =  '" & Trim(txt_EWayBillNo.Text) & "' ,  Rate =   " & Str(Val(txt_Rate.Text)) & " ,  Amount =  " & Str(Val(lbl_Amount.Text)) & " , GST_Tax_Invoice_Status = " & Str(Val(vGST_Tax_Inv_Sts)) & ",Delivery_Purpose_IdNo = " & Del_Purpose_IdNo.ToString & ",Lot_IdNo = " & Lot_IdNo.ToString & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
                'cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Receipt_Meters = " & Val(txt_ReceiptMeters.Text) & ", Consumed_Yarn = " & Val(lbl_ConsYarn.Text) & ", Consumed_Pavu = " & Val(lbl_ConsPavu.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = '' and Weaver_Wages_Code = '' and Weaver_IR_Wages_Code = ''"
                'cmd.ExecuteNonQuery()

                If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then

                    cmd.CommandText = " If not exists (Select * from Lot_Head WHERE Lot_No = '" & Lot_Code_forSelection & "') begin " &
                                      " insert into Lot_Head ([Lot_IdNo]                                      ,	[Lot_No]                      ,	[Sur_Name]                                               ,	[Lot_Description]   ,	[Lot_Main_Name]               ,	[Lot_Fn_Yr_Code]                  ,	[Auto_Created]         ,Ledger_IdNo ) " &
                                      " Values( (select isnull(max(Lot_IdNo),0)+1 from Lot_Head),'" & Lot_Code_forSelection & "','" & Common_Procedures.Remove_NonCharacters(Lot_Code_forSelection) & "' ,            ''        ,'" & Lot_Code_forSelection & "','" & Common_Procedures.FnYearCode.ToString & "', 1            ," & Led_ID.ToString & ") end "
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = " If exists (Select * from Lot_Head WHERE Lot_No = '" & Lot_Code_forSelection & "') begin " &
                                      " Update Lot_Head set Ledger_IdNo = " & Led_ID.ToString & " WHERE Lot_No = '" & Lot_Code_forSelection & "' end "
                    cmd.ExecuteNonQuery()

                    'Lot_IdNo = Common_Procedures.Lot_NoToIdNo(con, Lot_Code_forSelection, tr)

                    cmd.CommandText = "UPDATE [FabricPurchase_Weaver_Lot_Head] SET 	[FabricPurchase_Weaver_Lot_Date] = @EntryDate ,	[Ledger_IdNo]  = " & Led_ID.ToString & ", Cloth_IdNo = " & Clo_ID.ToString & ",Lot_IdNo = " & Lot_IdNo.ToString & " where [Creating_DOC_Ref_Code] = '" & Pk_Condition & NewCode & "'"
                    cmd.ExecuteNonQuery()

                End If

            End If

            Lot_IdNo = Common_Procedures.Lot_NoToIdNo(con, Lot_Code_forSelection, tr)

            Dim ms9 As New MemoryStream()
            If IsNothing(pic_PartyDc_Image.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(pic_PartyDc_Image.BackgroundImage)
                bitmp.Save(ms9, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Dim data9 As Byte() = ms9.GetBuffer()
            Dim N9 As New SqlClient.SqlParameter("@partydc_image", SqlDbType.Image)
            N9.Value = data9
            cmd.Parameters.Add(N9)
            ms9.Dispose()

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Cloth_Receipt_Head ( Receipt_Type, Weaver_ClothReceipt_Code,             Company_IdNo         ,    Weaver_ClothReceipt_RefNo  ,            Weaver_ClothReceipt_PrefixNo       ,                        Weaver_ClothReceipt_SuffixNo     ,       Weaver_ClothReceipt_No       ,                               for_OrderBy                              , Weaver_ClothReceipt_date ,           Ledger_IdNo   ,      Cloth_IdNo    ,            Lot_No             ,            Party_DcNo         ,           EndsCount_IdNo   ,          Count_IdNo        ,             empty_beam          ,           noof_pcs       ,             pcs_fromno         ,              pcs_tono        ,           ReceiptMeters_Receipt        ,  Receipt_Quantity                    ,             Receipt_Meters               ,          ConsumedYarn_Receipt       ,              Consumed_Yarn        ,         ConsumedPavu_Receipt       ,              Consumed_Pavu         ,       Loom_IdNo        ,            Width_Type     ,       Transport_IdNo      ,     Freight_Amount_Receipt   ,              Folding_Receipt        ,              Folding               ,       Total_Receipt_Pcs      ,    Total_Receipt_Meters   , BeamConsumption_Receipt, BeamConsumption_Meters, Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment, Weaver_Wages_Code, Weaver_Wages_Increment    ,            StockOff_IdNo   ,                           User_idNo      , Purchase_Status      , Our_Order_No                     , Own_Order_Code                      ,     Loom_Type                      , No_Stock_Posting_Status     ,              Driver_Name           ,               Driver_Phone_No       ,                Supervisor_Name           , Vehicle_no                           ,        WareHouse_IdNo     , No_Of_Bundles                            , Unloaded_By_Our_Employee ,    Verified_Status       ,       No_Weaving_Wages_Bill      , Weaver_IR_Wages_Code, lotcode_forSelection,                  DC_Receipt_Meters     ,                   Remarks               ,                Eway_BillNo        ,                   Rate         ,                  Amount                  ,GST_Tax_Invoice_Status             ,Delivery_Purpose_IdNo            ,Lot_IdNo                 ,        Processed_Cloth_IdNo,             Entry_Time        ,                 ClothSales_OrderCode_forSelection      ,        Return_Status    ,           DC_Receipt_Pcs     ) " &
                                    "           Values                   (     'W'     , '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', '" & Trim(txt_Weaver_Cloth_PrefixNo.Text) & "' ,          '" & Trim(cbo_Weaver_Cloth_SufixNo.Text) & "' ,       '" & Trim(vCloRec) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @EntryDate          , " & Str(Val(Led_ID)) & "," & Val(Clo_ID) & " , '" & Trim(txt_LotNo.Text) & "', '" & Trim(txt_PDcNo.Text) & "', " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(WftCnt_ID)) & ", " & Str(Val(txt_EBeam.Text)) & ", " & Val(vACT_RECPCS) & " , " & Val(txt_PcsNoFrom.Text) & ", " & Val(lbl_PcsNoTo.Text) & ", " & Str(Val(txt_ReceiptMeters.Text)) & ", " & Str(Val(txt_Quantity.Text)) & " ,  " & Str(Val(txt_ReceiptMeters.Text)) & ", " & Str(Val(lbl_ConsYarn.Text)) & ", " & Str(Val(lbl_ConsYarn.Text)) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(Lm_ID)) & ", '" & Trim(vWIDTHTYPE) & "', " & Str(Val(Trans_ID)) & ", " & Val(txt_Freight.Text) & ", " & Val(txt_Folding_Perc.Text) & "  ,  " & Val(txt_Folding_Perc.Text) & ", " & Str(Val(vTotRcptPcs)) & ", " & Str(Val(vTotRcptMtrs)) & ",          0             ,          0            ,          ''               ,             0                  ,         ''       ,           0           ," & Str(Val(StkOff_ID)) & " , " & Val(Common_Procedures.User.IdNo) & " , " & Val(Purc_STS) & ",'" & Trim(lbl_OrderNo.Text) & "' ,    '" & Trim(lbl_OrderCode.Text) & "', '" & Trim(cbo_LoomType.Text) & "'  ,   " & Val(NoStkPos_Sts) & " ,'" & Trim(cbo_DriverName.Text) & "' , '" & Trim(cbo_DriverPhNo.Text) & "' ,  '" & Trim(cbo_SupervisorName.Text) & "' , '" & Trim(cbo_VehicleNo.Text) & "'   , " & Str(Val(vGod_ID)) & " ,  " & Val(Txt_NoOfBundles.Text) & "       ," & Val(Vchk_UNLOADED) & ", " & Val(Verified_STS) & ", " & Val(NoWeaWages_Bill_Sts) & " ,        ''            , '" & Trim(vSELC_LOTCODE) & "', " & Str(Val(vDC_RECMTRS)) & "   ,   '" & Trim(txt_Remarks.Text) & "' , '" & Trim(txt_EWayBillNo.Text) & "'  ,   " & Str(Val(txt_Rate.Text)) & " ,  " & Str(Val(lbl_Amount.Text)) & "    , " & Str(Val(vGST_Tax_Inv_Sts)) & "," & Del_Purpose_IdNo.ToString & "," & Lot_IdNo.ToString & ", " & Pro_Clo_ID.ToString & ", '" & Trim(lbl_Time.Text) & "' , '" & Trim(cbo_Sales_OrderCode_forSelection.Text) & "' , " & Val(vRETURN_STS) & ", " & Str(Val(vDC_RECPCS)) & " ) "
                cmd.ExecuteNonQuery()


                'cmd.CommandText = "Insert into Weaver_Cloth_Receipt_Head ( Receipt_Type, Weaver_ClothReceipt_Code,             Company_IdNo         ,    Weaver_ClothReceipt_RefNo  , Weaver_ClothReceipt_SuffixNo,       Weaver_ClothReceipt_No  ,                               for_OrderBy                              , Weaver_ClothReceipt_date ,           Ledger_IdNo   ,      Cloth_IdNo    ,            Lot_No             ,            Party_DcNo         ,           EndsCount_IdNo   ,          Count_IdNo        ,             empty_beam          ,           noof_pcs            ,             pcs_fromno         ,              pcs_tono        ,           ReceiptMeters_Receipt        ,  Receipt_Quantity                    ,             Receipt_Meters               ,          ConsumedYarn_Receipt       ,              Consumed_Yarn        ,         ConsumedPavu_Receipt       ,              Consumed_Pavu         ,       Loom_IdNo        ,            Width_Type     ,       Transport_IdNo      ,     Freight_Amount_Receipt   ,              Folding_Receipt        ,              Folding               ,       Total_Receipt_Pcs      ,    Total_Receipt_Meters   , BeamConsumption_Receipt, BeamConsumption_Meters, Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment, Weaver_Wages_Code, Weaver_Wages_Increment    ,            StockOff_IdNo   ,                           User_idNo      , Purchase_Status      , Our_Order_No                     , Own_Order_Code                      ,     Loom_Type                      , No_Stock_Posting_Status     ,              Driver_Name           ,               Driver_Phone_No       ,                Supervisor_Name           , Vehicle_no                           ,        WareHouse_IdNo     , No_Of_Bundles                            , Unloaded_By_Our_Employee ,    Verified_Status       ,       No_Weaving_Wages_Bill      , Weaver_IR_Wages_Code, lotcode_forSelection,                  DC_Receipt_Meters     ,                   Remarks               ,                Eway_BillNo        ,                   Rate         ,                  Amount                  ,GST_Tax_Invoice_Status             ,Delivery_Purpose_IdNo            ,Lot_IdNo                 ,        Processed_Cloth_IdNo,             Entry_Time        ) " &
                '                    "           Values                   (     'W'     , '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "',          ''                 , '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @EntryDate          , " & Str(Val(Led_ID)) & "," & Val(Clo_ID) & " , '" & Trim(txt_LotNo.Text) & "', '" & Trim(txt_PDcNo.Text) & "', " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(WftCnt_ID)) & ", " & Str(Val(txt_EBeam.Text)) & ", " & Val(txt_NoOfPcs.Text) & " , " & Val(txt_PcsNoFrom.Text) & ", " & Val(lbl_PcsNoTo.Text) & ", " & Str(Val(txt_ReceiptMeters.Text)) & ", " & Str(Val(txt_Quantity.Text)) & " ,  " & Str(Val(txt_ReceiptMeters.Text)) & ", " & Str(Val(lbl_ConsYarn.Text)) & ", " & Str(Val(lbl_ConsYarn.Text)) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(Lm_ID)) & ", '" & Trim(vWIDTHTYPE) & "', " & Str(Val(Trans_ID)) & ", " & Val(txt_Freight.Text) & ", " & Val(txt_Folding_Perc.Text) & "  ,  " & Val(txt_Folding_Perc.Text) & ", " & Str(Val(vTotRcptPcs)) & ", " & Str(Val(vTotRcptMtrs)) & ",          0             ,          0            ,          ''               ,             0                  ,         ''       ,           0           ," & Str(Val(StkOff_ID)) & " , " & Val(Common_Procedures.User.IdNo) & " , " & Val(Purc_STS) & ",'" & Trim(lbl_OrderNo.Text) & "' ,    '" & Trim(lbl_OrderCode.Text) & "', '" & Trim(cbo_LoomType.Text) & "'  ,   " & Val(NoStkPos_Sts) & " ,'" & Trim(cbo_DriverName.Text) & "' , '" & Trim(cbo_DriverPhNo.Text) & "' ,  '" & Trim(cbo_SupervisorName.Text) & "' , '" & Trim(cbo_VehicleNo.Text) & "'   , " & Str(Val(vGod_ID)) & " ,  " & Val(Txt_NoOfBundles.Text) & "       ," & Val(Vchk_UNLOADED) & ", " & Val(Verified_STS) & ", " & Val(NoWeaWages_Bill_Sts) & " ,        ''            , '" & Trim(vSELC_LOTCODE) & "'," & Str(Val(vDC_RECMTRS)) & "   ,   '" & Trim(txt_Remarks.Text) & "' , '" & Trim(txt_EWayBillNo.Text) & "'  ,   " & Str(Val(txt_Rate.Text)) & " ,  " & Str(Val(lbl_Amount.Text)) & "    , " & Str(Val(vGST_Tax_Inv_Sts)) & "," & Del_Purpose_IdNo.ToString & "," & Lot_IdNo.ToString & ", " & Pro_Clo_ID.ToString & ", '" & Trim(lbl_Time.Text) & "' ) "
                'cmd.ExecuteNonQuery()

                ''cmd.CommandText = "Insert into Weaver_Cloth_Receipt_Head ( Receipt_Type, Weaver_ClothReceipt_Code,             Company_IdNo         ,    Weaver_ClothReceipt_RefNo  , Weaver_ClothReceipt_SuffixNo,       Weaver_ClothReceipt_No  ,                               for_OrderBy                              , Weaver_ClothReceipt_date ,           Ledger_IdNo   ,      Cloth_IdNo    ,            Lot_No             ,            Party_DcNo         ,           EndsCount_IdNo   ,          Count_IdNo        ,             empty_beam          ,           noof_pcs            ,             pcs_fromno         ,              pcs_tono        ,           ReceiptMeters_Receipt        ,  Receipt_Quantity                    ,             Receipt_Meters               ,          ConsumedYarn_Receipt       ,              Consumed_Yarn        ,         ConsumedPavu_Receipt       ,              Consumed_Pavu         ,       Loom_IdNo        ,            Width_Type     ,       Transport_IdNo      ,     Freight_Amount_Receipt   ,              Folding_Receipt        ,              Folding               ,       Total_Receipt_Pcs      ,    Total_Receipt_Meters   , BeamConsumption_Receipt, BeamConsumption_Meters, Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment, Weaver_Wages_Code, Weaver_Wages_Increment    ,            StockOff_IdNo   ,                           User_idNo      , Purchase_Status      , Our_Order_No                     , Own_Order_Code                      ,     Loom_Type                      , No_Stock_Posting_Status     ,              Driver_Name           ,               Driver_Phone_No       ,                Supervisor_Name           , Vehicle_no                           ,        WareHouse_IdNo     , No_Of_Bundles                            , Unloaded_By_Our_Employee ,    Verified_Status       ,       No_Weaving_Wages_Bill      , Weaver_IR_Wages_Code, lotcode_forSelection,                  DC_Receipt_Meters     ,                   Remarks               ,                Eway_BillNo        ,                   Rate         ,                  Amount                  ,GST_Tax_Invoice_Status             ,Delivery_Purpose_IdNo            ,Lot_IdNo                 ,        Processed_Cloth_IdNo, PartyDc_Document_Image ,             Entry_Time        ) " &
                ''                    "           Values                   (     'W'     , '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "',          ''                 , '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @EntryDate          , " & Str(Val(Led_ID)) & "," & Val(Clo_ID) & " , '" & Trim(txt_LotNo.Text) & "', '" & Trim(txt_PDcNo.Text) & "', " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(WftCnt_ID)) & ", " & Str(Val(txt_EBeam.Text)) & ", " & Val(txt_NoOfPcs.Text) & " , " & Val(txt_PcsNoFrom.Text) & ", " & Val(lbl_PcsNoTo.Text) & ", " & Str(Val(txt_ReceiptMeters.Text)) & ", " & Str(Val(txt_Quantity.Text)) & " ,  " & Str(Val(txt_ReceiptMeters.Text)) & ", " & Str(Val(lbl_ConsYarn.Text)) & ", " & Str(Val(lbl_ConsYarn.Text)) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(Lm_ID)) & ", '" & Trim(vWIDTHTYPE) & "', " & Str(Val(Trans_ID)) & ", " & Val(txt_Freight.Text) & ", " & Val(txt_Folding_Perc.Text) & "  ,  " & Val(txt_Folding_Perc.Text) & ", " & Str(Val(vTotRcptPcs)) & ", " & Str(Val(vTotRcptMtrs)) & ",          0             ,          0            ,          ''               ,             0                  ,         ''       ,           0           ," & Str(Val(StkOff_ID)) & " , " & Val(Common_Procedures.User.IdNo) & " , " & Val(Purc_STS) & ",'" & Trim(lbl_OrderNo.Text) & "' ,    '" & Trim(lbl_OrderCode.Text) & "', '" & Trim(cbo_LoomType.Text) & "'  ,   " & Val(NoStkPos_Sts) & " ,'" & Trim(cbo_DriverName.Text) & "' , '" & Trim(cbo_DriverPhNo.Text) & "' ,  '" & Trim(cbo_SupervisorName.Text) & "' , '" & Trim(cbo_VehicleNo.Text) & "'   , " & Str(Val(vGod_ID)) & " ,  " & Val(Txt_NoOfBundles.Text) & "       ," & Val(Vchk_UNLOADED) & ", " & Val(Verified_STS) & ", " & Val(NoWeaWages_Bill_Sts) & " ,        ''            , '" & Trim(vSELC_LOTCODE) & "'," & Str(Val(vDC_RECMTRS)) & "   ,   '" & Trim(txt_Remarks.Text) & "' , '" & Trim(txt_EWayBillNo.Text) & "'  ,   " & Str(Val(txt_Rate.Text)) & " ,  " & Str(Val(lbl_Amount.Text)) & "    , " & Str(Val(vGST_Tax_Inv_Sts)) & "," & Del_Purpose_IdNo.ToString & "," & Lot_IdNo.ToString & ", " & Pro_Clo_ID.ToString & ",   @partydc_image       , '" & Trim(lbl_Time.Text) & "' ) "
                ''cmd.ExecuteNonQuery()


                'If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then

                '    If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then

                '        cmd.CommandText = " If not exists (Select * from Lot_Head WHERE Lot_No = '" & Lot_Code_forSelection & "') begin " &
                '                          " insert into Lot_Head ([Lot_IdNo]                                      ,	[Lot_No]                      ,	[Sur_Name]                                                             ,	[Lot_Description] ,	[Lot_Main_Name]               ,	[Lot_Fn_Yr_Code]                  ,	[Auto_Created]  ,Ledger_IdNo ) " &
                '                          " Values( (select isnull(max(Lot_IdNo),0)+1 from Lot_Head),'" & Lot_Code_forSelection & "','" & Common_Procedures.Remove_NonCharacters(Lot_Code_forSelection) & "' ,            ''        ,'" & Lot_Code_forSelection & "','" & Common_Procedures.FnYearCode.ToLower & "', 1                  ," & Led_ID.ToString & ") end "
                '        cmd.ExecuteNonQuery()

                '        cmd.CommandText = " If exists (Select * from Lot_Head WHERE Lot_No = '" & Lot_Code_forSelection & "') begin " &
                '                     " Update Lot_Head set Ledger_IdNo = " & Led_ID.ToString & " WHERE Lot_No = '" & Lot_Code_forSelection & "' end "
                '        cmd.ExecuteNonQuery()

                '        cmd.CommandText = "INSERT INTO [FabricPurchase_Weaver_Lot_Head] (	[FabricPurchase_Weaver_Lot_IdNo]                                                                 ,	[FabricPurchase_Weaver_Lot_No] ,	[FabricPurchase_Weaver_Lot_Code] ,	[FabricPurchase_Weaver_Lot_Code_forSelection] ,	[For_OrderBy]                                                       ,	[Sur_Name]                                            ,	[FabricPurchase_Weaver_Lot_Date] ,	[Ledger_IdNo]        ,	[Creating_DOC_Ref_Code], Cloth_IdNo                  ,Lot_IdNo)" &
                '                                                  "VALUES           ((Select isnull(max([FabricPurchase_Weaver_Lot_IdNo] ),0)+1   from [FabricPurchase_Weaver_Lot_Head]) ,'" & lbl_RefNo.Text & "'         ,'" & Lot_Code & "'                   ,'" & Lot_Code_forSelection & "'                  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",'" & Common_Procedures.Remove_NonCharacters(Lot_Code) & "',   @EntryDate                   ," & Led_ID.ToString & ",'" & Pk_Condition & NewCode & "'," & Clo_ID.ToString & "," & Lot_IdNo.ToString & ")"
                '        cmd.ExecuteNonQuery()


                '    End If


                'End If

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Cloth_Receipt_head", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_ClothReceipt_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Weaver_ClothReceipt_Piece_Details", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Piece_No,Main_PieceNo,PieceNo_OrderBy,ReceiptMeters_Receipt,Receipt_Meters, Create_Status ,StockOff_IdNo,WareHouse_IdNo", "Sl_No", "Weaver_ClothReceipt_Code, For_OrderBy, Company_IdNo, Weaver_ClothReceipt_No, Weaver_ClothReceipt_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Receipt_Type = 'W',    Weaver_ClothReceipt_date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ",Weaver_ClothReceipt_PrefixNo = '" & Trim(txt_Weaver_Cloth_PrefixNo.Text) & "' , Weaver_ClothReceipt_SuffixNo = '" & Trim(cbo_Weaver_Cloth_SufixNo.Text) & "' , Weaver_ClothReceipt_No = '" & Trim(vCloRec) & "' , Cloth_IdNo = " & Val(Clo_ID) & " , Lot_No = '" & Trim(txt_LotNo.Text) & "',  Party_DcNo  = '" & Trim(txt_PDcNo.Text) & "',  EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Count_IdNo = " & Str(Val(WftCnt_ID)) & ", empty_beam = " & Val(txt_EBeam.Text) & " , noof_pcs = " & Val(vACT_RECPCS) & " , pcs_fromno = " & Val(txt_PcsNoFrom.Text) & " , pcs_tono = " & Val(lbl_PcsNoTo.Text) & " , ReceiptMeters_Receipt = " & Val(txt_ReceiptMeters.Text) & ", ConsumedYarn_Receipt = " & Val(lbl_ConsYarn.Text) & ", ConsumedPavu_Receipt = " & Val(lbl_ConsPavu.Text) & ", Loom_IdNo = " & Val(Lm_ID) & ", Width_Type = '" & Trim(vWIDTHTYPE) & "', Transport_IdNo = " & Val(Trans_ID) & ", Freight_Amount_Receipt = " & Val(txt_Freight.Text) & ", Total_Receipt_Pcs = " & Str(Val(vTotRcptPcs)) & ", Total_Receipt_Meters = " & Str(Val(vTotRcptMtrs)) & " , StockOff_IdNo = " & Str(Val(StkOff_ID)) & ",  User_idNo = " & Val(Common_Procedures.User.IdNo) & " ,  Purchase_Status = " & Val(Purc_STS) & ", Our_Order_No = '" & Trim(lbl_OrderNo.Text) & "', Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "', Loom_Type =  '" & Trim(cbo_LoomType.Text) & "' , Receipt_Quantity = " & Str(Val(txt_Quantity.Text)) & " , No_Stock_Posting_Status = " & Val(NoStkPos_Sts) & " , Driver_Name = '" & Trim(cbo_DriverName.Text) & "' , Driver_Phone_No = '" & Trim(cbo_DriverPhNo.Text) & "' , Supervisor_Name = '" & Trim(cbo_SupervisorName.Text) & "' , Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' , WareHouse_IdNo = " & Str(Val(vGod_ID)) & " , No_Of_Bundles =  " & Val(Txt_NoOfBundles.Text) & ",Unloaded_By_Our_Employee=" & Val(Vchk_UNLOADED) & ",Verified_Status= " & Val(Verified_STS) & ", No_Weaving_Wages_Bill = " & Val(NoWeaWages_Bill_Sts) & " ,lotcode_forSelection='" & Trim(vSELC_LOTCODE) & "',DC_Receipt_Meters=" & Str(Val(vDC_RECMTRS)) & " , Folding_Receipt = " & Val(txt_Folding_Perc.Text) & " , Remarks =  '" & Trim(txt_Remarks.Text) & "' , Eway_BillNo =  '" & Trim(txt_EWayBillNo.Text) & "' ,  Rate =   " & Str(Val(txt_Rate.Text)) & " ,  Amount =  " & Str(Val(lbl_Amount.Text)) & " , GST_Tax_Invoice_Status = " & Str(Val(vGST_Tax_Inv_Sts)) & ",Delivery_Purpose_IdNo = " & Del_Purpose_IdNo.ToString & ",Lot_IdNo = " & Lot_IdNo.ToString & ",Processed_Cloth_IdNo = " & Pro_Clo_ID.ToString & "  , ClothSales_OrderCode_forSelection = '" & Trim(cbo_Sales_OrderCode_forSelection.Text) & "' , Return_Status = " & Val(vRETURN_STS) & " , DC_Receipt_Pcs = " & Str(Val(vDC_RECPCS)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Receipt_Meters = " & Val(txt_ReceiptMeters.Text) & ", Consumed_Yarn = " & Val(lbl_ConsYarn.Text) & ", Consumed_Pavu = " & Val(lbl_ConsPavu.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = '' and Weaver_Wages_Code = '' and Weaver_IR_Wages_Code = ''"
                cmd.ExecuteNonQuery()

                'If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then

                '    cmd.CommandText = " If not exists (Select * from Lot_Head WHERE Lot_No = '" & Lot_Code_forSelection & "') begin " &
                '                      " insert into Lot_Head ([Lot_IdNo]                                      ,	[Lot_No]                      ,	[Sur_Name]                                               ,	[Lot_Description]   ,	[Lot_Main_Name]               ,	[Lot_Fn_Yr_Code]                  ,	[Auto_Created]         ,Ledger_IdNo ) " &
                '                      " Values( (select isnull(max(Lot_IdNo),0)+1 from Lot_Head),'" & Lot_Code_forSelection & "','" & Common_Procedures.Remove_NonCharacters(Lot_Code_forSelection) & "' ,            ''        ,'" & Lot_Code_forSelection & "','" & Common_Procedures.FnYearCode.ToString & "', 1            ," & Led_ID.ToString & ") end "
                '    cmd.ExecuteNonQuery()

                '    cmd.CommandText = " If exists (Select * from Lot_Head WHERE Lot_No = '" & Lot_Code_forSelection & "') begin " &
                '                      " Update Lot_Head set Ledger_IdNo = " & Led_ID.ToString & " WHERE Lot_No = '" & Lot_Code_forSelection & "' end "
                '    cmd.ExecuteNonQuery()

                '    'Lot_IdNo = Common_Procedures.Lot_NoToIdNo(con, Lot_Code_forSelection, tr)

                '    cmd.CommandText = "UPDATE [FabricPurchase_Weaver_Lot_Head] SET 	[FabricPurchase_Weaver_Lot_Date] = @EntryDate ,	[Ledger_IdNo]  = " & Led_ID.ToString & ", Cloth_IdNo = " & Clo_ID.ToString & ",Lot_IdNo = " & Lot_IdNo.ToString & " where [Creating_DOC_Ref_Code] = '" & Pk_Condition & NewCode & "'"
                '    cmd.ExecuteNonQuery()

                'End If

            End If


            cmd.CommandText = "Delete from Textile_Processing_Delivery_Head where ClothProcess_Delivery_Code = '" & Pk_Condition & NewCode & "'"
            cmd.ExecuteNonQuery()

            If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then
                'Pur_Weaver_Fabric_Lot_IdNo = Val(Common_Procedures.get_FieldValue(con, "FabricPurchase_Weaver_Lot_Head", "FabricPurchase_Weaver_Lot_IdNo", "Creating_DOC_Ref_Code= '" & Pk_Condition & NewCode & "'", , tr))
                Lot_IdNo = Common_Procedures.Lot_NoToIdNo(con, Lot_Code_forSelection, tr)
            End If

            If vGod_ID <> 0 And Del_Purpose_IdNo <> 0 Then

                cmd.CommandText = "Insert into Textile_Processing_Delivery_Head(ClothProcess_Delivery_Code, Company_IdNo                             , ClothProcess_Delivery_No  , for_OrderBy                                                         , ClothProcess_Delivery_Date, Ledger_IdNo                   , Purchase_OrderNo  , Transport_IdNo                       , Freight_Charges , Note                          ,Total_Pcs                         ,Total_Qty                 , Total_Meters                     , Total_Weight                  , Processing_Idno                    , JobOrder_No              ,  User_idNo  , Vehicle_No ,FabricPurchase_Weaver_Lot_IdNo,Lot_IdNo                 ,Folding)
                Values ('" & Pk_Condition & Trim(NewCode) & "'         , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate             , " & Str(Val(vGod_ID)) & ", ''                , 0                       ,       0         ,  '" & Trim(txt_Remarks.Text) & "'," & Str(Val(vACT_RECPCS)) & ",0                         , " & Str(Val(txt_ReceiptMeters.Text)) & ", 0 ,  " & Str(Val(Del_Purpose_IdNo)) & ",'" & lbl_RefNo.Text & "' ," & Val(Common_Procedures.User.IdNo) & " ,''      ," & Lot_IdNo.ToString & "                                                         ," & Lot_IdNo.ToString & "," & Val(txt_Folding_Perc.Text).ToString & ")"
                cmd.ExecuteNonQuery()


                cmd.CommandText = "Delete from Textile_Processing_Delivery_Details where Cloth_Processing_Delivery_Code = '" & Pk_Condition & NewCode & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into Textile_Processing_Delivery_Details(Cloth_Processing_Delivery_Code               , Company_IdNo                     , Cloth_Processing_Delivery_No      , for_OrderBy                                                             , Cloth_Processing_Delivery_Date,Sl_No, Ledger_IdNo              ,  Item_Idno             ,Item_To_Idno        , Colour_Idno ,Lot_IdNo                   ,Processing_Idno                 ,   Bales   ,  Bales_Nos  ,  Delivery_Pcs                ,Delivery_Qty ,Meter_Qty                         ,Delivery_Meters                    ,Delivery_Weight       , PackingSlip_Codes , ClothProcessing_Delivery_PackingSlno ,FabricPurchase_Weaver_Lot_IdNo ,Folding) " &
                                                                     "Values ('" & Pk_Condition & Trim(NewCode) & "'       , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'     , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate                , 1   , " & Str(Val(vGod_ID)) & " ," & Str(Val(Clo_ID)) & ", " & Str(Val(Pro_Clo_ID)) & ", 0           ," & Lot_IdNo.ToString & " , " & Val(Del_Purpose_IdNo) & " ,0           ,''           , " & Val(vACT_RECPCS) & ", 0           ,      0                            , " & Str(Val(txt_ReceiptMeters.Text)) & ", 0 ,''                 ,''                               ," & Lot_IdNo.ToString & "," & Val(txt_Folding_Perc.Text).ToString & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Delete Textile_Processing_Delivery_Head where ClothProcess_Delivery_Code = '" & Pk_Condition & NewCode & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete Textile_Processing_Delivery_Details where Cloth_Processing_Delivery_Code = '" & Pk_Condition & NewCode & "'"
                cmd.ExecuteNonQuery()

            End If


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Weaver_Cloth_Receipt_head", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_ClothReceipt_Code, Company_IdNo, for_OrderBy", tr)

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                ClthName = Microsoft.VisualBasic.Left(cbo_Cloth.Text, 10)
                Partcls = "CloRcpt :" & Trim(ClthName) & " L.No." & Trim(lbl_RefNo.Text)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
                Partcls = ""
                Partcls = "CloRcpt : " & Trim(lbl_RefNo.Text)
                If Trim(txt_PDcNo.Text) <> "" Then
                    Partcls = Trim(Partcls) & ",  P.Dc.No : " & Trim(txt_PDcNo.Text)
                End If
                Partcls = Trim(Partcls) & ",  Cloth : " & Trim(cbo_Cloth.Text) & ", Ends : " & Trim(cbo_EndsCount.Text) & ", Pcs : " & Trim(vACT_RECPCS)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Then
                Partcls = "CloRcpt : LotNo. " & Trim(lbl_RefNo.Text)
                If Trim(txt_PDcNo.Text) <> "" Then
                    Partcls = Trim(Partcls) & ",  P.Dc.No : " & Trim(txt_PDcNo.Text)
                End If
                Partcls = Trim(Partcls) & ",  Cloth : " & Trim(cbo_Cloth.Text)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1613" Then
                Partcls = "CloRcpt : " 'LotNo. " & Trim(lbl_RefNo.Text)
                'If Trim(txt_PDcNo.Text) <> "" Then
                '    Partcls = Trim(Partcls) & ",  P.Dc.No : " & Trim(txt_PDcNo.Text)
                'End If


                Dim Ends_Name() As String
                Dim Pick, width As String

                Da = New SqlClient.SqlDataAdapter("select * from cloth_Head where Cloth_Name = '" & Trim(cbo_Cloth.Text) & "' ", con)
                Da.SelectCommand.Transaction = tr
                Dt = New DataTable
                Da.Fill(Dt)

                Pick = 0
                width = 0

                If Dt.Rows.Count > 0 Then

                    Pick = Dt.Rows(0).Item("Cloth_Pick").ToString
                    width = Dt.Rows(0).Item("Cloth_Width").ToString

                End If

                Ends_Name = Split(cbo_EndsCount.Text, "/")

                Partcls = Trim(Partcls) & " " & Trim(lbl_WeftCount.Text)
                Partcls = Trim(Partcls) & ", " & Ends_Name(0) '  Endscount : " & Trim(cbo_EndsCount.Text)
                Partcls = Trim(Partcls) & ", " & Val(Pick) & " X " & Val(width) ' Cloth : " & Trim(cbo_Cloth.Text)

            Else

                Partcls = "CloRcpt : LotNo. " & Trim(lbl_RefNo.Text)
                If Trim(txt_PDcNo.Text) <> "" Then
                    Partcls = Trim(Partcls) & ",  P.Dc.No : " & Trim(txt_PDcNo.Text)
                End If

            End If

            PBlNo = Trim(lbl_RefNo.Text)

            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Report_Particulars_Receipt = '" & Trim(Partcls) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Report_Particulars = '" & Trim(Partcls) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and Create_Status = 1 and Weaver_Piece_Checking_Code = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Then

                        Sno = Sno + 1

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then
                            vMAINPCSNO = Replace(Trim(UCase(.Rows(i).Cells(0).Value)), Trim(UCase(vLEDShtNm)), "")

                        Else
                            vMAINPCSNO = Replace(Trim(UCase(.Rows(i).Cells(0).Value)), Trim(UCase(vLEDShtNm)), "")

                        End If

                        vPCSCODE_FORSELECTION = Trim(UCase(.Rows(i).Cells(0).Value)) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag)) & "/" & Trim(Pk_Condition)


                        vSETCD1 = ""
                        vBMNO1 = ""
                        If .Columns(3).Visible = True Then
                            If Trim(.Rows(i).Cells(3).Value) <> "" Then
                                Da1 = New SqlClient.SqlDataAdapter("Select set_code, beam_no from Stock_SizedPavu_Processing_Details Where BeamNo_SetCode_forSelection = '" & Trim(.Rows(i).Cells(3).Value) & "'", con)
                                Da1.SelectCommand.Transaction = tr
                                Dt1 = New DataTable
                                Da1.Fill(Dt1)
                                If Dt1.Rows.Count > 0 Then
                                    vSETCD1 = Dt1.Rows(0).Item("set_code").ToString
                                    vBMNO1 = Dt1.Rows(0).Item("beam_no").ToString
                                End If
                                Dt1.Clear()
                            End If
                        End If


                        vSETCD2 = ""
                        vBMNO2 = ""
                        If .Columns(4).Visible = True Then
                            If Trim(.Rows(i).Cells(4).Value) <> "" Then
                                Da1 = New SqlClient.SqlDataAdapter("Select set_code, beam_no from Stock_SizedPavu_Processing_Details Where BeamNo_SetCode_forSelection = '" & Trim(.Rows(i).Cells(4).Value) & "'", con)
                                Da1.SelectCommand.Transaction = tr
                                Dt1 = New DataTable
                                Da1.Fill(Dt1)
                                If Dt1.Rows.Count > 0 Then
                                    vSETCD2 = Dt1.Rows(0).Item("set_code").ToString
                                    vBMNO2 = Dt1.Rows(0).Item("beam_no").ToString
                                End If
                                Dt1.Clear()
                            End If
                        End If

                        Nr = 0
                        cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_ClothReceipt_date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Sl_No = " & Str(Val(Sno)) & ", PieceCode_for_Selection = '" & Trim(vPCSCODE_FORSELECTION) & "' , Main_PieceNo = '" & Trim(Val(.Rows(i).Cells(0).Value)) & "' , PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(0).Value))) & ", ReceiptMeters_Receipt = " & Val(.Rows(i).Cells(1).Value) & ", Loom_No = '" & Trim(.Rows(i).Cells(2).Value) & "', Create_Status = 1, StockOff_IdNo = " & Str(Val(vStkOf_Pos_IdNo)) & ", WareHouse_IdNo = " & Str(Val(vGod_ID)) & ", EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Count_IdNo = " & Str(Val(WftCnt_ID)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()
                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_ClothReceipt_Code ,            Company_IdNo          ,      Weaver_ClothReceipt_No   ,                               for_OrderBy                               , Weaver_ClothReceipt_date,           Lot_Code      ,             Lot_No            ,     Ledger_IdNo         ,            Cloth_IdNo   ,            Folding_Receipt         ,             Folding               ,         Sl_No        ,                     Piece_No                  ,                  Main_PieceNo  ,         PieceCode_for_Selection       ,                               PieceNo_OrderBy                      ,     ReceiptMeters_Receipt           ,                Receipt_Meters       ,                    Loom_No             , Create_Status ,              StockOff_IdNo       ,          WareHouse_IdNo  ,         EndsCount_IdNo     ,             Count_IdNo      ,      BeamNo_SetCode_forSelection       ,        BeamNo2_SetCode_forSelection     ,     Receipt_Dhothi_Quantity          ,          Set_Code1       ,           Beam_No1     ,          Set_Code2      ,           Beam_No2     ) " &
                            " Values                                       ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,          @EntryDate     ,  '" & Trim(NewCode) & "', '" & Trim(txt_LotNo.Text) & "', " & Str(Val(Led_ID)) & ", " & Str(Val(Clo_ID)) & ", " & Val(txt_Folding_Perc.Text) & " , " & Val(txt_Folding_Perc.Text) & ", " & Str(Val(Sno)) & ", '" & Trim(UCase(.Rows(i).Cells(0).Value)) & "', '" & Trim(Val(vMAINPCSNO)) & "', '" & Trim(vPCSCODE_FORSELECTION) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vMAINPCSNO))) & ", " & Val(.Rows(i).Cells(1).Value) & ", " & Val(.Rows(i).Cells(1).Value) & ", '" & Trim(.Rows(i).Cells(2).Value) & "',       1       , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(vGod_ID)) & ", " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(WftCnt_ID)) & " , '" & Trim(.Rows(i).Cells(3).Value) & "',  '" & Trim(.Rows(i).Cells(4).Value) & "', " & Val(.Rows(i).Cells(7).Value) & " ,  '" & Trim(vSETCD1) & "',  '" & Trim(vBMNO1) & "' ,  '" & Trim(vSETCD2) & "',  '" & Trim(vBMNO2) & "' ) "
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_ClothReceipt_Piece_Details", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Piece_No,Main_PieceNo,PieceNo_OrderBy,ReceiptMeters_Receipt,Receipt_Meters, Create_Status ,StockOff_IdNo,WareHouse_IdNo", "Sl_No", "Weaver_ClothReceipt_Code, For_OrderBy, Company_IdNo, Weaver_ClothReceipt_No, Weaver_ClothReceipt_RefNo, Weaver_ClothReceipt_Date, Ledger_Idno", tr)

            End With

            If Val(txt_EBeam.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo                     , Reference_No                  , for_OrderBy                                                            , Reference_Date, DeliveryTo_Idno                                           , ReceivedFrom_Idno       , Entry_ID             , Party_Bill_No        , Particulars            , Sl_No, Beam_Width_IdNo, Empty_Beam                       ) " &
                "Values                                    ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate    , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1    , 0              , " & Str(Val(txt_EBeam.Text)) & " )"
                cmd.ExecuteNonQuery()
            End If

            If Trim(PcsChkCode) = "" And Trim(WagesCode) = "" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1461" Then

                cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                'If Val(lbl_ConsPavu.Text) <> 0 Then
                PavuStock_In = ""
                mtrspcs = 0

                Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  EndsCount_Head Where EndsCount_IdNo = " & Str(Val(EdsCnt_ID)), con)
                Da.SelectCommand.Transaction = tr
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    PavuStock_In = Dt2.Rows(0)("Stock_In").ToString
                    mtrspcs = Val(Dt2.Rows(0)("Meters_Pcs").ToString)
                End If
                Dt2.Clear()

                If Trim(UCase(PavuStock_In)) = "PCS" Then
                    lbl_ConsPavu.Text = Val(vACT_RECPCS)
                End If

                If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then

                End If

                If Trim(UCase(EntFnYrCode)) = Trim(UCase(Common_Procedures.FnYearCode)) Then

                    If Val(Purc_STS) = 0 Then

                        vDelv_ID = 0 : vRec_ID = 0
                        If Trim(UCase(Led_type)) = "JOBWORKER" Then
                            vDelv_ID = Led_ID
                            vRec_ID = 0
                        Else
                            vDelv_ID = 0
                            vRec_ID = Led_ID
                        End If

                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (                 Reference_Code             ,                 Company_IdNo     ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,         DeliveryTo_Idno   ,      ReceivedFrom_Idno   ,          Cloth_Idno     ,           Entry_ID   ,     Party_Bill_No    ,         Particulars    ,            Sl_No     ,            EndsCount_IdNo  , Sized_Beam,                 Meters              ) " &
                                            "           Values                       ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(EdsCnt_ID)) & ",     0     , " & Str(Val(lbl_ConsPavu.Text)) & " ) "
                        cmd.ExecuteNonQuery()


                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (                Reference_Code              ,                 Company_IdNo     ,               Reference_No    ,                               for_OrderBy                              , Reference_Date,         DeliveryTo_Idno   ,       ReceivedFrom_Idno  ,          Entry_ID    ,         Particulars    ,       Party_Bill_No  , Sl_No,           Count_IdNo       , Yarn_Type, Mill_IdNo, Bags, Cones,                 Weight              ) " &
                                            "          Values                        ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "',   1  , " & Str(Val(WftCnt_ID)) & ",    'MILL',    0     ,  0  ,    0 , " & Str(Val(lbl_ConsYarn.Text)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                End If

                StkDelvTo_ID = 0 : StkRecFrm_ID = 0
                If Val(Led_ID) = Val(vGod_ID) Then
                    StkDelvTo_ID = Val(vGod_ID)
                    StkRecFrm_ID = 0

                Else
                    StkDelvTo_ID = Val(vGod_ID)
                    StkRecFrm_ID = Val(Led_ID)

                End If



                clthStock_In = ""
                clthmtrspcs = 0

                Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  Cloth_Head Where Cloth_idno = " & Str(Val(Clo_ID)), con)
                Da.SelectCommand.Transaction = tr
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    clthStock_In = Dt2.Rows(0)("Stock_In").ToString
                    clthmtrspcs = Val(Dt2.Rows(0)("Meters_Pcs").ToString)
                End If
                Dt2.Clear()

                clthPcs_Mtr = 0
                clthtype1_Mtr = 0
                If Trim(UCase(clthStock_In)) = "-*-PCS-*-" Then

                    clthPcs_Mtr = Val(vACT_RECPCS)

                    'cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (                 Reference_Code             ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,       StockOff_IdNo              ,        DeliveryTo_Idno        ,       ReceivedFrom_Idno       ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     ,              Folding               ,   UnChecked_Meters  ,  Meters_Type1                 , Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ,Lot_IdNo) " &
                    '                          "    Values                         ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(StkDelvTo_ID)) & ", " & Str(Val(StkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ",  " & Val(txt_Folding_Perc.Text) & ",                0    , " & Str(Val(clthPcs_Mtr)) & " ,       0     ,       0     ,       0     ,       0     ," & Lot_IdNo.ToString & ") "
                    'cmd.ExecuteNonQuery()

                    cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    If Val(clthPcs_Mtr) <> 0 And (Del_Purpose_IdNo = 0 Or vGod_ID = 0) Then

                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,             StockOff_IdNo        ,       DeliveryTo_Idno             ,       ReceivedFrom_Idno       ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     ,               Folding             ,             UnChecked_Meters ,  Meters_Type1             , Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ,Lot_IdNo) " &
                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate    , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(vGod_ID.ToString)) & ", " & Str(Val(StkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ", " & Str(Val(txt_Folding_Perc.Text)) & ",                0        , Str(Val(vStkOf_Pos_IdNo)) ,       0     ,       0     ,       0     ,       0      ," & Lot_IdNo.ToString & ") "
                        cmd.ExecuteNonQuery()

                    Else

                        cmd.CommandText = "Delete from  Stock_Cloth_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                        cmd.ExecuteNonQuery()

                    End If

                Else

                    clthtype1_Mtr = 0
                    clthPcs_Mtr = 0
                    If Trim(UCase(clthStock_In)) = "PCS" Then
                        If txt_Quantity.Visible Then
                            clthtype1_Mtr = Val(txt_Quantity.Text)
                        Else
                            clthPcs_Mtr = Val(vACT_RECPCS)
                        End If

                    Else
                        clthPcs_Mtr = Val(txt_ReceiptMeters.Text)
                        If Val(clthPcs_Mtr) = 0 And txt_Dc_receipt_mtrs.Visible = True Then
                            clthPcs_Mtr = Val(txt_Dc_receipt_mtrs.Text)
                        End If

                    End If

                    'cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code            ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,       StockOff_IdNo              ,        DeliveryTo_Idno        ,       ReceivedFrom_Idno       ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     ,              Folding              ,             UnChecked_Meters ,  Meters_Type1, Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ) " &
                    '"Values                                     ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(StkDelvTo_ID)) & ", " & Str(Val(StkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ", " & Val(txt_Folding_Perc.Text) & ", " & Str(Val(clthPcs_Mtr)) & ",       0      ,       0     ,       0     ,       0     ,       0      ) "
                    'cmd.ExecuteNonQuery()

                    If (Val(clthPcs_Mtr) <> 0 Or Val(clthtype1_Mtr) <> 0) And (Del_Purpose_IdNo = 0 Or vGod_ID = 0) Then

                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,                                   StockOff_IdNo           ,       DeliveryTo_Idno             ,       ReceivedFrom_Idno       ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     ,               Folding                  ,             UnChecked_Meters ,           Meters_Type1          , Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ,Lot_IdNo) " &
                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate    , " & Str(Val(vStkOf_Pos_IdNo)) & "                         , " & Str(Val(vGod_ID.ToString)) & ", " & Str(Val(StkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ", " & Str(Val(txt_Folding_Perc.Text)) & ", " & Str(Val(clthPcs_Mtr)) & ", " & Str(Val(clthtype1_Mtr)) & " ,       0     ,       0     ,       0     ,       0      ," & Lot_IdNo.ToString & ") "
                        cmd.ExecuteNonQuery()

                    Else

                        cmd.CommandText = "Delete from  Stock_Cloth_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                        cmd.ExecuteNonQuery()

                    End If

                End If


                If Val(Purc_STS) = 0 Then

                    With dgv_BobinDetails
                        Sno = 1000
                        For i = 0 To .RowCount - 1

                            If Val(.Rows(i).Cells(1).Value) <> 0 Then

                                ECnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(0).Value, tr)

                                If Val(ECnt_ID) <> 0 And Val(.Rows(i).Cells(1).Value) <> 0 Then

                                    Sno = Sno + 1
                                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters) Values ('" & Trim(Pk_Condition2) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(ECnt_ID)) & ", 0,  " & Val(.Rows(i).Cells(1).Value) & " )"
                                    cmd.ExecuteNonQuery()

                                End If

                            End If
                        Next
                    End With

                    With dgv_KuriDetails
                        Sno = 1000
                        For i = 0 To .RowCount - 1

                            If Val(.Rows(i).Cells(1).Value) <> 0 Then

                                KuriCnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(0).Value, tr)

                                If Val(KuriCnt_ID) <> 0 And Val(.Rows(i).Cells(1).Value) <> 0 Then

                                    Sno = Sno + 1

                                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight) Values ('" & Trim(Pk_Condition2) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & " , " & Str(Val(KuriCnt_ID)) & ", 'MILL', 0, 0, 0, " & Val(.Rows(i).Cells(1).Value) & "  )"
                                    cmd.ExecuteNonQuery()

                                End If

                            End If
                        Next
                    End With
                End If

            Else

                If Val(Purc_STS) = 1 Then

                    cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                End If

                vDelv_ID = 0 : vRec_ID = 0
                If Trim(UCase(Led_type)) = "JOBWORKER" Then
                    vDelv_ID = Led_ID
                    vRec_ID = 0
                Else
                    vDelv_ID = 0
                    vRec_ID = Led_ID
                End If

                cmd.CommandText = "Update Stock_Pavu_Processing_Details Set DeliveryTo_Idno = " & Str(Val(vDelv_ID)) & ",  ReceivedFrom_Idno = " & Str(Val(vRec_ID)) & ",  EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Yarn_Processing_Details Set DeliveryTo_Idno = " & Str(Val(vDelv_ID)) & ",  ReceivedFrom_Idno = " & Str(Val(vRec_ID)) & " , Count_IdNo = " & Str(Val(WftCnt_ID)) & " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                StkDelvTo_ID = 0 : StkRecFrm_ID = 0
                If Val(Led_ID) = Val(vGod_ID) Then
                    StkDelvTo_ID = Val(vGod_ID)
                    StkRecFrm_ID = 0

                Else
                    StkDelvTo_ID = Val(vGod_ID)
                    StkRecFrm_ID = Val(Led_ID)

                End If

                cmd.CommandText = "Update Stock_Cloth_Processing_Details Set StockOff_IdNo = " & Str(Val(vStkOf_Pos_IdNo)) & ",  DeliveryTo_Idno = " & Str(Val(StkDelvTo_ID)) & ",  ReceivedFrom_Idno = " & Str(Val(StkRecFrm_ID)) & " , Cloth_Idno = " & Str(Val(Clo_ID)) & ",Lot_Idno = " & Lot_IdNo.ToString & ",Folding = " & Val(txt_Folding_Perc.Text).ToString & " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Piece_Checking_Head Set Cloth_IdNo = " & Str(Val(Clo_ID)) & " Where Receipt_PkCondition = 'WCLRC-' and Piece_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Wages_Head Set Cloth_IdNo = " & Str(Val(Clo_ID)) & " Where Weaver_Cloth_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Weaver_ClothReceipt_Attachment_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Dim vATTACH_IMG_STS As Boolean = False


            Sno = 0
            vATTACH_IMG_STS = False
            For i = 0 To dgv_Attachments.Rows.Count - 1

                If Trim(dgv_Attachments.Rows(i).Cells(1).Value) <> "" Then

                    Dim vFILENAME As String = dgv_Attachments.Rows(i).Cells(1).Value
                    Dim vfileinfo As New FileInfo(vFILENAME)
                    Dim vfileextn As String = vfileinfo.Extension
                    Dim vfilebinarydata As Byte() = vDIC_ATTACHMENTS(i)

                    If Not vfilebinarydata Is Nothing Then

                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@filename", vFILENAME)
                        cmd.Parameters.AddWithValue("@fileextension", vfileextn)
                        cmd.Parameters.AddWithValue("@filedata", vfilebinarydata)

                        Sno = Sno + 1
                        cmd.CommandText = "Insert into Weaver_ClothReceipt_Attachment_Details (           Weaver_ClothReceipt_Code         ,             Company_IdNo         ,           Sl_No      , file_name, file_extension, file_content ) " &
                                            "           Values                                ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", " & Str(Val(Sno)) & ", @filename, @fileextension, @filedata    )"
                        cmd.ExecuteNonQuery()

                        vATTACH_IMG_STS = True

                    End If

                End If

            Next

            If vATTACH_IMG_STS = False Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                    Throw New ApplicationException("Invalid Party DC image attachment")
                End If
            End If

            Dim vVou_LedIdNos As String = "", vVou_ErrMsg As String = ""
            vVou_Amts = ""

            If Val(txt_Freight.Text) = 0 Then txt_Freight.Text = 0.0

            vVou_LedIdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "Wea.CloRcpt.Frgt", Val(lbl_Company.Tag), Trim(PkCondition_WFRGT) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_date.Text), Partcls, vVou_LedIdNos, vVou_Amts, vVou_ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(vVou_ErrMsg)
            End If

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Or vNEGATIVE_YARN_STOCK_STS = True Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                          " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1267" Then '----KRG TEXTILE MILLS (PALLADAM)
                    If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub
                End If

            End If

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Or vNEGATIVE_PAVU_STOCK_STS = True Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , EndsCount_IdNo ) " &
                                          " Select                               'PAVU', Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1267" Then '----KRG TEXTILE MILLS (PALLADAM)
                    If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub
                End If

            End If

            tr.Commit()

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then

                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If

            Else

                move_record(lbl_RefNo.Text)

            End If

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus() Else cbo_Weaver.Focus()

        End Try


    End Sub

    Private Sub cbo_Transport_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, Nothing, Txt_NoOfBundles, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                cbo_WidthType.Focus()
            ElseIf txt_ReceiptMeters.Visible And txt_ReceiptMeters.Enabled Then
                txt_ReceiptMeters.Focus()
            ElseIf txt_PcsNoFrom.Visible And txt_PcsNoFrom.Enabled Then
                txt_PcsNoFrom.Focus()
            ElseIf txt_NoOfPcs.Visible And txt_NoOfPcs.Enabled Then
                txt_NoOfPcs.Focus()
            ElseIf txt_Dc_receipt_mtrs.Visible And txt_Dc_receipt_mtrs.Enabled Then
                txt_Dc_receipt_mtrs.Focus()
            ElseIf txt_PDcNo.Visible And txt_PDcNo.Enabled Then
                txt_PDcNo.Focus()
            Else
                txt_ReceiptMeters.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, Txt_NoOfBundles, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

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

    Private Sub cbo_Weaver_Ente(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyDown

        If Trim(Common_Procedures.settings.CustomerCode) = "1040" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver, msk_date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver, cbo_LoomType, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        End If
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Sales_OrderCode_forSelection.Visible And cbo_Sales_OrderCode_forSelection.Enabled Then
                cbo_Sales_OrderCode_forSelection.Focus()
            Else
                cbo_Cloth.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_Weaver_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaver, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1204" Then

                Dim da As New SqlClient.SqlDataAdapter
                Dim dt As New DataTable
                Dim vLed_id As Integer

                vLed_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_Weaver.Text)

                da = New SqlClient.SqlDataAdapter("select  a.Weaver_LoomType from  ledger_head a  where a.Ledger_IdNo = " & Str(Val(vLed_id)), con)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                        cbo_LoomType.Text = dt.Rows(0).Item("Weaver_LoomType")

                    Else
                        If dt.Rows(0).Item("Weaver_LoomType") <> "" Then
                            cbo_LoomType.Text = dt.Rows(0).Item("Weaver_LoomType")
                        End If

                    End If

                Else
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                        cbo_LoomType.Text = ""
                    End If

                End If
                dt.Clear()

            End If

            If Common_Procedures.settings.Internal_Order_Entry_Status = 1 Then
                If MessageBox.Show("Do you want to select Internal Order", "FOR INTERNAL ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_io_selection_Click(sender, e)
                Else
                    If cbo_Sales_OrderCode_forSelection.Visible And cbo_Sales_OrderCode_forSelection.Enabled Then
                        cbo_Sales_OrderCode_forSelection.Focus()
                    Else
                        cbo_Cloth.Focus()
                    End If
                End If

            Else

                If cbo_Sales_OrderCode_forSelection.Visible And cbo_Sales_OrderCode_forSelection.Enabled Then
                    cbo_Sales_OrderCode_forSelection.Focus()
                Else
                    cbo_Cloth.Focus()
                End If

            End If

        End If
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Weaver.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_EndsCount_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.Enter
        Dim Clo_ID As Integer = 0

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "(EndsCount_IdNo IN (select sq1.EndsCount_IdNo from Cloth_EndsCount_Details sq1 where sq1.Cloth_Idno = " & Str(Val(Clo_ID)) & ") )", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        Dim Clo_ID As Integer = 0

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, txt_PDcNo, Nothing, "EndsCount_Head", "EndsCount_Name", "(EndsCount_IdNo IN (select sq1.EndsCount_IdNo from Cloth_EndsCount_Details sq1 where sq1.Cloth_Idno = " & Str(Val(Clo_ID)) & ") )", "(EndsCount_IdNo = 0)")
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If txt_Dc_receipt_mtrs.Visible = True And txt_Dc_receipt_mtrs.Enabled = True Then
                txt_Dc_receipt_mtrs.Focus()
            ElseIf txt_EBeam.Visible = True And txt_EBeam.Enabled Then
                txt_EBeam.Focus()
            Else
                txt_NoOfPcs.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Dim Clo_ID As Integer = 0

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, Nothing, "EndsCount_Head", "EndsCount_Name", "(EndsCount_IdNo IN (select sq1.EndsCount_IdNo from Cloth_EndsCount_Details sq1 where sq1.Cloth_Idno = " & Str(Val(Clo_ID)) & ") )", "(EndsCount_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If txt_Dc_receipt_mtrs.Visible = True And txt_Dc_receipt_mtrs.Enabled Then
                txt_Dc_receipt_mtrs.Focus()
            ElseIf txt_EBeam.Visible = True And txt_EBeam.Enabled Then
                txt_EBeam.Focus()
            Else
                txt_NoOfPcs.Focus()
            End If
        End If

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
        Dim Led_IdNo As Integer, Clo_IdNo As Integer
        Dim Condt As String = ""
        Dim Verfied_Sts As Integer = 0
        Try

            Condt = ""
            Led_IdNo = 0
            Clo_IdNo = 0


            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then

                ' Condt = "a.Weaver_ClothReceipt_date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
                Condt = "a.Weaver_ClothReceipt_date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_ClothReceipt_date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_ClothReceipt_date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_Cloth.Text) <> "" Then
                Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Cloth.Text)
            End If




            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Clo_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Cloth_IdNo = " & Str(Val(Clo_IdNo))
            End If

            If Trim(txt_Filter_RecNo.Text) <> "" And Trim(txt_Filter_RecNoTo.Text) <> "" Then
                Condt = "a.Weaver_ClothReceipt_RefNo between '" & Trim(txt_Filter_RecNo.Text) & "' and '" & Trim(txt_Filter_RecNoTo.Text) & "'"
            ElseIf Trim(txt_Filter_RecNo.Text) <> "" Then
                Condt = "a.Weaver_ClothReceipt_RefNo  = '" & Trim(txt_Filter_RecNo.Text) & "'"
            ElseIf Trim(txt_Filter_RecNoTo.Text) <> "" Then
                Condt = "a.Weaver_ClothReceipt_RefNo  = '" & Trim(txt_Filter_RecNoTo.Text) & "'"
            End If


            If cbo_Verified_Sts.Visible = True And Trim(cbo_Verified_Sts.Text) <> "" Then

                If Trim(cbo_Verified_Sts.Text) = "YES" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 1 "
                ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 0 "
                End If

            End If


            da = New SqlClient.SqlDataAdapter("Select a.*, b.Ledger_Name as WeaverName ,E.EndsCount_Name, c.*  from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Cloth_Head c ON c.Cloth_IdNo = a.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head E ON E.EndsCount_IdNo = a.EndsCount_IdNo where a.Receipt_Type = 'W' and a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code LIKE '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " and " & Other_Condition & " Order by a.for_orderby, a.Weaver_ClothReceipt_RefNo", con)
            'da = New SqlClient.SqlDataAdapter("Select a.*, b.Ledger_Name as WeaverName ,E.EndsCount_Name, c.*  from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Cloth_Head c ON c.Cloth_IdNo = a.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head E ON E.EndsCount_IdNo = a.EndsCount_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code LIKE '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " and (a.Receipt_Type = 'W' and a.Weaver_ClothReceipt_Code NOT LIKE 'GWEWA-%' ) Order by a.for_orderby, a.Weaver_ClothReceipt_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Weaver_ClothReceipt_RefNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_ClothReceipt_date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("WeaverName").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Party_DcNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                    'dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("weaver_bill_no").ToString
                    'dgv_Filter_Details.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("empty_beam").ToString)
                    'dgv_Filter_Details.Rows(n).Cells(8).Value = Val(dt2.Rows(i).Item("noof_pcs").ToString)
                    'dgv_Filter_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Total_Receipt_Meters").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("rough_consumed_yarn").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1 ) and Close_status = 0", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1 ) and Close_status = 0 ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "cloth_name", "", "(cloth_name)")

    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, cbo_Filter_PartyName, Nothing, "Cloth_Head", "cloth_name", "", "(cloth_name)")
        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If

        End If
    End Sub




    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, Nothing, "Cloth_Head", "cloth_name", "", "(cloth_name)")
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Verified_Sts, cbo_Filter_Cloth, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub cbo_Verified_Sts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Verified_Sts.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Verified_Sts, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        Dim TotMtrs As Single = 0

        Total_Calculation()

        With dgv_Details_Total
            If .RowCount > 0 Then
                TotMtrs = Val(.Rows(0).Cells(1).Value)
            End If
        End With
        If Val(txt_ReceiptMeters.Text) = 0 Then
            txt_ReceiptMeters.Text = Format(Val(TotMtrs), "#########0.00")
        End If

        dgv_Details_CellLeave(sender, e)

    End Sub

    Private Sub dgv_Details_GotFocus(sender As Object, e As EventArgs) Handles dgv_Details.GotFocus
        vCLO_MTR_PER_PC = 0
        vCLO_MTRPERPC_QUALITY = ""
        get_Cloth_Meter_per_Piece()
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim Nextvalue As Integer = 0
        Dim vPCSNO As String = ""
        Dim Led_ID As Integer = 0
        Dim vLEDShtNm As String = ""
        Dim rect As Rectangle

        With dgv_Details

            If Trim(UCase(vCLO_MTRPERPC_QUALITY)) <> Trim(UCase(cbo_Cloth.Text)) Then
                get_Cloth_Meter_per_Piece()
            End If

            PieceNo_Generation_RowWise(e.RowIndex)

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then

            '    Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
            '    vLEDShtNm = ""
            '    If Led_ID <> 0 Then
            '        vLEDShtNm = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_ShortName", "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")")
            '        vLEDShtNm = Trim(UCase(vLEDShtNm))
            '    End If

            '    If e.RowIndex = 0 Then
            '        .CurrentRow.Cells(0).Value = Trim(vLEDShtNm) & Trim(Val(txt_PcsNoFrom.Text))

            '    Else
            '        'If Val(.CurrentRow.Cells(0).Value) = 0 Then
            '        '    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            '        'End If

            '        If Val(.CurrentRow.Cells(0).Value) = 0 Then

            '            If Trim(vLEDShtNm) <> "" Then
            '                vPCSNO = Replace(Trim(UCase(.Rows(e.RowIndex - 1).Cells(0).Value)), Trim(UCase(vLEDShtNm)), "")
            '                .CurrentRow.Cells(0).Value = Trim(vLEDShtNm) & Trim(Val(vPCSNO) + 1)
            '            Else
            '                .CurrentRow.Cells(0).Value = Val(.Rows(e.RowIndex - 1).Cells(0).Value) + 1
            '            End If

            '        End If
            '    End If

            'Else

            '    If e.RowIndex = 0 Then
            '        .CurrentRow.Cells(0).Value = Val(txt_PcsNoFrom.Text)

            '    Else

            '        If Val(.CurrentRow.Cells(0).Value) = 0 Then
            '            .CurrentRow.Cells(0).Value = Val(.Rows(e.RowIndex - 1).Cells(0).Value) + 1
            '        End If

            '    End If

            'End If



            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1273" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1408" Then
                If e.ColumnIndex = 1 Then
                    If e.RowIndex > 0 Then
                        If Trim(.CurrentRow.Cells(e.ColumnIndex).Value) = "" Then
                            .CurrentRow.Cells(e.ColumnIndex).Value = .Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value
                        End If
                    End If
                End If
            End If




            'If .CurrentCell.ColumnIndex <> 0 And Val(.CurrentCell.Value) <> 0 Then
            '    If .CurrentRow.Index = .Rows.Count - 1 Then
            '        .Rows.Add()
            '    End If
            'End If


            'If e.RowIndex > 0 And e.ColumnIndex = 1 Then
            '    If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
            '        .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value)
            '        '.CurrentRow.Cells(2).Value = .Rows(e.RowIndex - 1).Cells(2).Value
            '    End If
            '    If e.ColumnIndex = 1 And e.RowIndex = .RowCount - 1 Then
            '        .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value)
            '    End If
            'End If

            'If Trim(Common_Procedures.settings.CustomerCode) = "1249" Then
            '    If e.RowIndex = 0 And e.ColumnIndex = 1 And Val(.CurrentRow.Cells(1).Value) = 0 Then
            '        .CurrentRow.Cells(1).Value = 100
            '    End If
            '    If e.ColumnIndex = 1 And e.RowIndex = .RowCount - 1 Then
            '        If e.RowIndex > 0 Then
            '            If e.ColumnIndex = 1 Then
            '                If Val(.CurrentRow.Cells(e.ColumnIndex).Value) <> 0 Then
            '                    .CurrentRow.Cells(e.ColumnIndex).Value = .Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value
            '                End If
            '            End If
            '        End If
            '    End If
            'End If




            '    If e.RowIndex > 0 And e.ColumnIndex = 1 Then
            '        If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
            '            .CurrentRow.Cells(1).Value = .Rows(e.RowIndex - 1).Cells(1).Value
            '            '.Rows.Add()
            '        End If
            '    End If


            '    If e.RowIndex > 0 Then
            '        If e.RowIndex = .Rows.Count - 1 Then
            '            If Val(.CurrentRow.Cells(1).Value) = 0 Then
            '                .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value)
            '                .Rows.Add()
            '            End If
            '        End If
            '    End If





            If e.ColumnIndex = 3 Then

                If Trim(.Rows(e.RowIndex).Cells(3).Value) = "" Then
                    Dim vBEAMNO As String = ""
                    vBEAMNO = get_SizedPavu_BeamNo_for_Selected_LoomNo(e.RowIndex, .Rows(e.RowIndex).Cells(2).Value)
                    .Rows(e.RowIndex).Cells(3).Value = vBEAMNO
                End If

                If cbo_Grid_BeamNo1.Visible = False Or Val(cbo_Grid_BeamNo1.Tag) <> e.RowIndex Then

                    cbo_Grid_BeamNo1.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select BeamNo_SetCode_forSelection from Stock_SizedPavu_Processing_Details order by BeamNo_SetCode_forSelection", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_BeamNo1.DataSource = Dt1
                    cbo_Grid_BeamNo1.DisplayMember = "BeamNo_SetCode_forSelection"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_BeamNo1.Left = .Left + rect.Left
                    cbo_Grid_BeamNo1.Top = .Top + rect.Top

                    cbo_Grid_BeamNo1.Width = rect.Width
                    cbo_Grid_BeamNo1.Height = rect.Height
                    cbo_Grid_BeamNo1.Text = .CurrentCell.Value

                    cbo_Grid_BeamNo1.Tag = Val(e.RowIndex)
                    cbo_Grid_BeamNo1.Visible = True

                    cbo_Grid_BeamNo1.BringToFront()
                    cbo_Grid_BeamNo1.Focus()

                End If

            Else
                cbo_Grid_BeamNo1.Visible = False

            End If


            If e.ColumnIndex = 4 Then

                If cbo_Grid_BeamNo2.Visible = False Or Val(cbo_Grid_BeamNo2.Tag) <> e.RowIndex Then

                    cbo_Grid_BeamNo2.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select BeamNo_SetCode_forSelection from Stock_SizedPavu_Processing_Details order by BeamNo_SetCode_forSelection", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_BeamNo2.DataSource = Dt1
                    cbo_Grid_BeamNo2.DisplayMember = "BeamNo_SetCode_forSelection"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_BeamNo2.Left = .Left + rect.Left
                    cbo_Grid_BeamNo2.Top = .Top + rect.Top

                    cbo_Grid_BeamNo2.Width = rect.Width
                    cbo_Grid_BeamNo2.Height = rect.Height
                    cbo_Grid_BeamNo2.Text = .CurrentCell.Value

                    cbo_Grid_BeamNo2.Tag = Val(e.RowIndex)
                    cbo_Grid_BeamNo2.Visible = True

                    cbo_Grid_BeamNo2.BringToFront()
                    cbo_Grid_BeamNo2.Focus()

                End If

            Else
                cbo_Grid_BeamNo2.Visible = False

            End If



            If e.ColumnIndex = 7 Then
                If vDGV_LEVCELNO = 3 Or vDGV_LEVCELNO = 4 Then
                    Dim vBEAM_TOTMTR As String, vBEAM_BALMTR As String
                    vBEAM_TOTMTR = 0 : vBEAM_BALMTR = 0
                    get_SizedPavu_TotalMeter_BalanceMeter(e.RowIndex, .Rows(e.RowIndex).Cells(3).Value, vBEAM_TOTMTR, vBEAM_BALMTR)
                    .Rows(e.RowIndex).Cells(5).Value = vBEAM_TOTMTR
                    .Rows(e.RowIndex).Cells(6).Value = vBEAM_BALMTR
                End If

            End If


        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 0 Then
                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Trim(UCase(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value))

            ElseIf .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
            vDGV_LEVCELNO = e.ColumnIndex
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim TotMtrs As Single = 0

        On Error Resume Next

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            If .Visible Then


                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 7 Then

                    If .CurrentCell.ColumnIndex = 7 Then
                        If .Columns(7).Visible = True Then
                            .Rows(e.RowIndex).Cells(1).Value = Format(Val(.Rows(e.RowIndex).Cells(7).Value) * Val(vCLO_MTR_PER_PC), "#########0.00")
                        End If
                    End If

                    Total_Calculation()

                    With dgv_Details_Total
                        If .RowCount > 0 Then
                            TotMtrs = Val(.Rows(0).Cells(1).Value)
                        End If
                    End With

                    If Val(TotMtrs) <> 0 Then txt_ReceiptMeters.Text = Format(Val(TotMtrs), "#########0.00")

                End If

            End If
        End With
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer
        Dim PcsFrmNo As Integer = 0
        Dim NewCode As String = ""
        Dim PcsChkCode As String = ""
        Dim WagesCode As String = ""

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_Code, Weaver_Wages_Code, Weaver_IR_Wages_Code from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            PcsChkCode = ""
            WagesCode = ""
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                End If
                If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                    WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
                End If
                If Trim(WagesCode) = "" Then
                    If IsDBNull(Dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString) = False Then
                        WagesCode = Dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString
                    End If
                End If
            End If
            Dt1.Clear()


            If Trim(PcsChkCode) <> "" Then
                MessageBox.Show("Piece Checking prepared", "DOES NOT DELETE PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
            If Trim(WagesCode) <> "" Then
                MessageBox.Show("Weaver wages prepared", "DOES NOT DELETE PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            With dgv_Details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

                PcsFrmNo = Val(txt_PcsNoFrom.Text)
                If Val(PcsFrmNo) = 0 Then PcsFrmNo = 1

                For i = 0 To .Rows.Count - 1
                    PieceNo_Generation_RowWise(i)
                    'If i = 0 Then
                    '    .Rows(i).Cells(0).Value = Val(PcsFrmNo)
                    'Else
                    '    .Rows(i).Cells(0).Value = Val(.Rows(i - 1).Cells(0).Value) + 1
                    'End If
                Next

            End With

            Total_Calculation()

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub


    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded

        Dim n As Integer
        On Error Resume Next

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        PieceNo_Generation_RowWise(e.RowIndex)

    End Sub

    Private Sub Total_Calculation()
        Dim TotPcs As Integer, TotMtrs As String, TOTQTY As Integer

        TotPcs = 0
        TotMtrs = 0
        TOTQTY = 0
        With dgv_Details

            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(1).Value) <> 0 Then
                    TotPcs = TotPcs + 1
                    TotMtrs = Val(TotMtrs) + Val(.Rows(i).Cells(1).Value)
                    TOTQTY = Val(TOTQTY) + Val(.Rows(i).Cells(7).Value)
                End If
            Next

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(0).Value = Val(TotPcs)
            .Rows(0).Cells(1).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(7).Value = Val(TOTQTY)
        End With

        If Val(TotMtrs) <> 0 Then txt_ReceiptMeters.Text = Format(Val(TotMtrs), "#########0.00")
        If Val(TOTQTY) <> 0 And dgv_Details.Columns(7).Visible = True Then
            txt_Quantity.Text = Val(TOTQTY)
        End If

    End Sub

    Private Sub cbo_Cloth_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.Enter
        Dim vCONDT As String

        vCONDT = get_ClothName_Listing_Condition()

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "cloth_name", vCONDT, "(cloth_idno=0)")
        cbo_Cloth.BackColor = Color.Lime
        cbo_Cloth.ForeColor = Color.Blue
        vCLO_MTR_PER_PC = 0
        vCLO_MTRPERPC_QUALITY = ""
    End Sub


    Private Sub cbo_cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Dim vCONDT As String

        vCONDT = get_ClothName_Listing_Condition()
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, cbo_Weaver, Nothing, "Cloth_Head", "cloth_name", vCONDT, "(cloth_idno=0)")
        If (e.KeyValue = 40 And cbo_Cloth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If txt_Folding_Perc.Visible And txt_Folding_Perc.Enabled = True Then
                txt_Folding_Perc.Focus()
            ElseIf txt_LotNo.Visible And txt_LotNo.Enabled = True Then
                txt_LotNo.Focus()
            Else
                txt_PDcNo.Focus()
            End If

        ElseIf (e.KeyValue = 38 And cbo_Cloth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_Sales_OrderCode_forSelection.Visible And cbo_Sales_OrderCode_forSelection.Enabled = True Then
                cbo_Sales_OrderCode_forSelection.Focus()
            ElseIf cbo_Weaver.Visible And cbo_Weaver.Enabled = True Then
                cbo_Weaver.Focus()
            Else
                msk_date.Focus()
            End If
        End If



    End Sub

    Private Sub cbo_cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim Clo_IdNo As Integer, edscnt_idno As Integer
        Dim wftcnt_idno As Integer
        Dim edscnt_id As Integer = 0
        Dim cnt_id As Integer = 0
        Dim mtrs As Single = 0
        Dim vClothRate As String = 0
        Dim vCONDT As String

        vCONDT = get_ClothName_Listing_Condition()

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, Nothing, "Cloth_Head", "cloth_name", vCONDT, "(cloth_idno=0)")

        If Asc(e.KeyChar) = 13 Then
            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

            wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
            lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, wftcnt_idno)

            If Trim(cbo_EndsCount.Text) = "" Then
                edscnt_idno = Val(Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "EndsCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & " and Sl_No = 1 )"))
                cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, edscnt_idno)
            End If

            vClothRate = 0
            txt_Rate.Text = ""
            If Val(Clo_IdNo) <> 0 Then
                vClothRate = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Sound_Rate", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
                txt_Rate.Text = vClothRate
            End If

            Consumption_Calculation()
            Grid_Cell_DeSelect()

            If txt_Folding_Perc.Visible And txt_Folding_Perc.Enabled Then
                txt_Folding_Perc.Focus()
            ElseIf txt_LotNo.Visible And txt_LotNo.Enabled = True Then
                txt_LotNo.Focus()
            Else
                txt_PDcNo.Focus()
            End If
        End If
    End Sub

    Private Function get_ClothName_Listing_Condition() As String
        Dim VndrNm_Id As String = 0
        Dim vTEX_CLOIDNO As String = 0
        Dim vCONDT As String = 0
        Dim vORD_CONDT As String

        vCONDT = "(Close_status = 0)"
        If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then '---- BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION) 
            If Trim(cbo_Weaver.Text) <> "" Then
                VndrNm_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
                If Val(VndrNm_Id) <> 0 Then
                    vCONDT = " Close_status = 0 and Cloth_IdNo IN (select sqq1.Cloth_IdNo from Weaver_Loom_Details sqq1 where sqq1.ledger_idno = " & Val(VndrNm_Id) & " ) "
                End If
            End If
        End If

        vORD_CONDT = ""
        If cbo_Sales_OrderCode_forSelection.Visible = True Then
            If Trim(cbo_Sales_OrderCode_forSelection.Text) <> "" Then
                vORD_CONDT = " Cloth_IdNo In (Select sq2.Cloth_IdNo from ClothSales_Order_Details sq2, ClothSales_Order_Head sq3 where sq3.ClothSales_OrderCode_forSelection = '" & Trim(cbo_Sales_OrderCode_forSelection.Text) & "' and sq2.ClothSales_Order_Code = sq3.ClothSales_Order_Code) "
            End If
        End If

        vCONDT = Trim(vCONDT) & IIf(Trim(vORD_CONDT) <> "", " and ", "") & Trim(vORD_CONDT)

        If Trim(vCONDT) <> "" Then
            vCONDT = "(" & Trim(vCONDT) & ")"
        End If

        Return vCONDT

    End Function

    Private Sub cbo_Cloth_Leave(sender As Object, e As EventArgs) Handles cbo_Cloth.Leave
        Dim Clo_IdNo As Integer, edscnt_idno As Integer
        Dim wftcnt_idno As Integer
        Dim edscnt_id As Integer = 0
        Dim cnt_id As Integer = 0
        Dim mtrs As Single = 0

        cbo_Cloth.BackColor = Color.White
        cbo_Cloth.ForeColor = Color.Black

        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

        wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
        lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, wftcnt_idno)

        If Trim(cbo_EndsCount.Text) = "" Then
            edscnt_idno = Val(Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "EndsCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & " and Sl_No = 1 )"))
            cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, edscnt_idno)
        End If

        Consumption_Calculation()
        Grid_Cell_DeSelect()

        cbo_Cloth.BackColor = Color.White
        cbo_Cloth.ForeColor = Color.Black

    End Sub

    Private Sub Consumption_Calculation()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim Clo_IdNo As Integer
        Dim edscnt_id As Integer = 0
        Dim cnt_id As Integer = 0
        Dim slno, n As Integer
        Dim mtrs As Single = 0
        Dim Pcs As Single = 0

        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

        mtrs = Val(txt_ReceiptMeters.Text)
        Pcs = Val(txt_NoOfPcs.Text)

        da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name  from Cloth_Bobin_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo  where a.Cloth_Idno = " & Str(Val(Clo_IdNo)), con)
        da.Fill(dt3)

        dgv_BobinDetails.Rows.Clear()
        slno = 0

        If dt3.Rows.Count > 0 Then

            For i = 0 To dt3.Rows.Count - 1

                n = dgv_BobinDetails.Rows.Add()
                dgv_BobinDetails.Rows(n).Cells(0).Value = dt3.Rows(i).Item("EndsCount_Name").ToString

                If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                    dgv_BobinDetails.Rows(n).Cells(1).Value = Format(Val(dt3.Rows(i).Item("Cloth_Consumption").ToString) * Val(Pcs), "########0.000")
                Else
                    dgv_BobinDetails.Rows(n).Cells(1).Value = Format(Val(dt3.Rows(i).Item("Cloth_Consumption").ToString) * Val(mtrs), "########0.00")
                End If

            Next i

        End If
        dt3.Clear()
        dt3.Dispose()

        da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Cloth_Kuri_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Cloth_Idno = " & Str(Val(Clo_IdNo)), con)
        da.Fill(dt4)

        dgv_KuriDetails.Rows.Clear()
        slno = 0

        If dt4.Rows.Count > 0 Then

            For i = 0 To dt4.Rows.Count - 1

                n = dgv_KuriDetails.Rows.Add()

                dgv_KuriDetails.Rows(n).Cells(0).Value = dt4.Rows(i).Item("Count_Name").ToString
                If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                    dgv_KuriDetails.Rows(n).Cells(1).Value = Format(Val(dt4.Rows(i).Item("Cloth_Consumption").ToString) * Val(Pcs), "#######0.000")
                Else
                    dgv_KuriDetails.Rows(n).Cells(1).Value = Format(Val(dt4.Rows(i).Item("Cloth_Consumption").ToString) * Val(mtrs), "#######0.000")
                End If

            Next i

        End If
        dt4.Clear()
        dt4.Dispose()

    End Sub

    Private Sub cbo_cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth.Name
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

    Private Sub cbo_Filter_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Filter_Cloth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyCode = 40 Then
            If cbo_VehicleNo.Visible Then
                cbo_VehicleNo.Focus()
            ElseIf cbo_StockOff.Visible = True Then
                cbo_StockOff.Focus()
            ElseIf cbo_Godown_StockIN.Visible Then
                cbo_Godown_StockIN.Focus()
            Else
                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    save_record()
                'Else
                '    If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
                'End If
                txt_Remarks.Focus()
            End If

        End If
        If e.KeyCode = 38 Then Txt_NoOfBundles.Focus()
    End Sub

    Private Sub txt_PcsNoFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PcsNoFrom.KeyDown
        If e.KeyCode = 40 Then
            If Trim(Common_Procedures.settings.CustomerCode) = "1040" Then
                txt_ReceiptMeters.Focus()
            ElseIf dgv_Details.Enabled And dgv_Details.Visible And dgv_Details.Rows.Count > 0 Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1549" Then '---- GIRI FABRICS (VAGARAYAMPALAYAM)
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
                Else
                    If dgv_Details.Columns(0).ReadOnly = False Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    End If
                End If
                dgv_Details.Focus()
                dgv_Details.CurrentCell.Selected = True

            ElseIf txt_ReceiptMeters.Enabled And txt_ReceiptMeters.Visible Then
                txt_ReceiptMeters.Focus()
            Else
                cbo_LoomNo.Focus()
            End If
            e.Handled = True
            e.SuppressKeyPress = True

        End If
        If e.KeyCode = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            SendKeys.Send("+{TAB}")
        End If
    End Sub


    Private Sub txt_ReceiptMeters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ReceiptMeters.KeyDown
        Dim TotMtrs As Single = 0




        If e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 Then


            TotMtrs = 0
            With dgv_Details_Total
                If .RowCount > 0 Then
                    TotMtrs = Val(.Rows(0).Cells(1).Value)
                End If
            End With
            If Val(TotMtrs) <> 0 Then e.Handled = True : e.SuppressKeyPress = True
        End If


    End Sub

    Private Sub txt_ReceiptMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ReceiptMeters.KeyPress
        Dim TotMtrs As Single = 0


        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True

        Else
            With dgv_Details_Total
                TotMtrs = 0
                If .RowCount > 0 Then
                    TotMtrs = Val(.Rows(0).Cells(1).Value)
                End If
            End With
            If Val(TotMtrs) <> 0 Then e.Handled = True

        End If




    End Sub

    Private Sub txt_weft_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub txt_Dc_receipt_mtrs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Dc_receipt_mtrs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub txt_PcsNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsNoFrom.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If Trim(Common_Procedures.settings.CustomerCode) = "1040" Then
                txt_ReceiptMeters.Focus()
            ElseIf dgv_Details.Enabled And dgv_Details.Visible And dgv_Details.Rows.Count > 0 Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1549" Then '---- GIRI FABRICS (VAGARAYAMPALAYAM)
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
                Else
                    If dgv_Details.Columns(0).ReadOnly = False Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    End If
                End If
                dgv_Details.Focus()
                dgv_Details.CurrentCell.Selected = True


            ElseIf txt_ReceiptMeters.Enabled And txt_ReceiptMeters.Visible Then
                txt_ReceiptMeters.Focus()

            ElseIf cbo_LoomNo.Enabled And cbo_LoomNo.Visible Then
                cbo_LoomNo.Focus()

            Else
                cbo_Transport.Focus()

            End If

        End If

    End Sub

    Private Sub txt_NoOfPcs_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NoOfPcs.GotFocus
        txt_NoOfPcs.Tag = txt_NoOfPcs.Text
    End Sub

    Private Sub txt_NoofPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfPcs.KeyPress

        If Asc(e.KeyChar) = 13 Then
            If Val(txt_NoOfPcs.Tag) <> Val(txt_NoOfPcs.Text) Then
                txt_NoOfPcs.Tag = txt_NoOfPcs.Text
                Design_PieceDetails_Grid()
            End If

            If txt_PcsNoFrom.Enabled And txt_PcsNoFrom.Visible Then
                txt_PcsNoFrom.Focus()
            ElseIf dgv_Details.Enabled And dgv_Details.Visible And dgv_Details.Rows.Count > 0 Then
                If dgv_Details.Columns(0).ReadOnly = False Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                End If
                dgv_Details.Focus()
                dgv_Details.CurrentCell.Selected = True
            ElseIf txt_ReceiptMeters.Enabled And txt_ReceiptMeters.Visible Then
                txt_ReceiptMeters.Focus()
            ElseIf cbo_LoomNo.Enabled And cbo_LoomNo.Visible Then
                cbo_LoomNo.Focus()
            Else
                cbo_Transport.Focus()
            End If
            e.Handled = True

        Else

            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        End If

    End Sub

    Private Sub txt_NoOfPcs_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_NoOfPcs.KeyDown

        If e.KeyCode = 40 Then

            If txt_PcsNoFrom.Enabled And txt_PcsNoFrom.Visible Then
                txt_PcsNoFrom.Focus()
            ElseIf dgv_Details.Enabled And dgv_Details.Visible And dgv_Details.Rows.Count > 0 Then
                If dgv_Details.Columns(0).ReadOnly = False Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                End If
                dgv_Details.Focus()
                dgv_Details.CurrentCell.Selected = True
            ElseIf txt_ReceiptMeters.Enabled And txt_ReceiptMeters.Visible Then
                txt_ReceiptMeters.Focus()
            ElseIf cbo_LoomNo.Enabled And cbo_LoomNo.Visible Then
                cbo_LoomNo.Focus()
            Else
                cbo_Transport.Focus()
            End If
            e.Handled = True
            e.SuppressKeyPress = True

        End If
        If e.KeyCode = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If cbo_VehicleNo.Visible Then
                cbo_VehicleNo.Focus()
            ElseIf cbo_StockOff.Visible = True Then
                cbo_StockOff.Focus()
            ElseIf cbo_Godown_StockIN.Visible Then
                cbo_Godown_StockIN.Focus()
            Else
                txt_Remarks.Focus()
                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    save_record()
                'Else
                '    If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
                'End If
            End If
        End If
    End Sub

    Private Sub txt_Filter_RecNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_RecNo.KeyPress
        If Asc(e.KeyChar) = 13 Then btn_Filter_Show_Click(sender, e)
    End Sub

    Private Sub txt_Filter_RecNoTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_RecNoTo.KeyPress
        If Asc(e.KeyChar) = 13 Then cbo_Filter_Cloth.Focus()
    End Sub

    Private Sub ConsumedYarn_Calculation()
        Dim CloID As Integer = 0
        Dim ConsYarn As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim YrnCons_For As String = ""
        Dim Clo_Mtrs_Pc As Single = 0
        Dim vStkPOS_STS As Boolean = True

        CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

        ConsYarn = 0
        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

            YrnCons_For = ""
            Da = New SqlClient.SqlDataAdapter("Select Stock_In from Cloth_Head Where Cloth_IdNo = " & Str(Val(CloID)), con)
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                YrnCons_For = Dt2.Rows(0)("Stock_In").ToString
            End If
            Dt2.Clear()

            If Trim(UCase(YrnCons_For)) = "PCS" Then
                If txt_Quantity.Visible Then
                    ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(txt_Quantity.Text))
                Else
                    ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(txt_NoOfPcs.Text))
                End If

            Else
                ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(txt_ReceiptMeters.Text))

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
            ConsYarn = Format(Val(txt_ReceiptMeters.Text), "##########0")

        Else

            vStkPOS_STS = True

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then

                Dim DtTM1 As Date
                Dim DtTM2 As Date

                DtTM1 = #12/31/2020#  '----from 01-Jan-2021 no stock posting for unchecked piece, STOCK POSTING will done only after piece checking
                DtTM2 = Convert.ToDateTime(msk_date.Text)

                If DateDiff(DateInterval.Day, DtTM1, DtTM2) > 0 Then
                    vStkPOS_STS = False
                End If

            End If

            If vStkPOS_STS = True Then
                ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(txt_ReceiptMeters.Text))
            End If

        End If

        lbl_ConsYarn.Text = Format(ConsYarn, "#########0.000")

    End Sub

    Private Sub ConsumedPavu_Calculation()
        Dim CloID As Integer = 0
        Dim ConsPavu As Single = 0
        Dim LmID As Integer = 0
        Dim Clo_Mtrs_Pc As Single = 0
        Dim vStkPOS_STS As Boolean = True

        CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        LmID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        ConsPavu = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
            Clo_Mtrs_Pc = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Meters_Pcs", "(Cloth_idno = " & Str(Val(CloID)) & ")"))
            If Val(Clo_Mtrs_Pc) = 0 Then Clo_Mtrs_Pc = 40
            ConsPavu = Format(Val(Clo_Mtrs_Pc) * Val(txt_NoOfPcs.Text), "##########0.00")

        Else

            vStkPOS_STS = True

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then

                Dim DtTM1 As Date
                Dim DtTM2 As Date

                DtTM1 = #12/31/2020#  '----from 01-Jan-2021 no stock posting for unchecked piece, STOCK POSTING will done only after piece checking
                DtTM2 = Convert.ToDateTime(msk_date.Text)

                If DateDiff(DateInterval.Day, DtTM1, DtTM2) > 0 Then
                    vStkPOS_STS = False
                End If

            End If

            If vStkPOS_STS = True Then
                ConsPavu = Common_Procedures.get_Pavu_Consumption(con, CloID, LmID, Val(txt_ReceiptMeters.Text), Trim(cbo_WidthType.Text))
            End If

            If txt_Folding_Perc.Visible = True Then
                Dim fdprec As String = 0

                fdprec = Val(txt_Folding_Perc.Text)
                If Val(fdprec) = 0 Then fdprec = 100

                ConsPavu = Format(Val(ConsPavu) * Val(fdprec) / 100, "##########0.00")

            End If

        End If

        lbl_ConsPavu.Text = Format(ConsPavu, "##########0.00")

    End Sub

    Private Sub txt_ReceiptMeters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ReceiptMeters.TextChanged
        ConsumedYarn_Calculation()
        ConsumedPavu_Calculation()
        Consumption_Calculation()
        Amount_Calculation()
    End Sub


    Private Sub PieceNo_To_Calculation()
        Dim vTotPcs As Integer = 0
        Dim vTotMtrs As Integer = 0
        Dim vPcsFrmNo As Integer = 0
        Dim vACT_RECPCS As Integer

        vACT_RECPCS = Val(txt_NoOfPcs.Text)
        If Val(vACT_RECPCS) = 0 And txt_Dc_receipt_pcs.Visible = True Then
            vACT_RECPCS = Val(txt_Dc_receipt_pcs.Text)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1490" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)

            lbl_PcsNoTo.Text = ""

            If Val(vACT_RECPCS) > 0 Then

                If Val(txt_PcsNoFrom.Text) = 0 Then
                    txt_PcsNoFrom.Text = 1
                End If

                lbl_PcsNoTo.Text = Val(txt_PcsNoFrom.Text) + Val(vACT_RECPCS) - 1

            End If

        End If

        'If Val(txt_NoOfPcs.Text) = 0 Then

        '    With dgv_Details_Total
        '        If .RowCount > 0 Then
        '            vTotPcs = Val(.Rows(0).Cells(0).Value)
        '            vTotMtrs = Val(.Rows(0).Cells(1).Value)
        '        End If
        '    End With

        '    If Val(vTotMtrs) > 0 Then
        '
        '        If Val(txt_PcsNoFrom.Text) = 0 Then
        '            vPcsFrmNo = 0
        '            With dgv_Details
        '                If .RowCount > 0 Then
        '                    vPcsFrmNo = Val(.Rows(0).Cells(0).Value)
        '                End If
        '            End With
        '            If Val(vPcsFrmNo) = 0 Then vPcsFrmNo = 1
        '            txt_PcsNoFrom.Text = Val(vPcsFrmNo)
        '        End If
        '        lbl_PcsNoTo.Text = Val(txt_PcsNoFrom.Text) + Val(vTotPcs) - 1
        '    End If
        '
        'Else
        '
        '    If Val(txt_PcsNoFrom.Text) = 0 Then txt_PcsNoFrom.Text = "1"
        '    lbl_PcsNoTo.Text = Val(txt_PcsNoFrom.Text) + Val(txt_NoOfPcs.Text) - 1
        '
        'End If

    End Sub

    Private Sub txt_NoOfPcs_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NoOfPcs.LostFocus
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim Stock_In As String
        Dim mtrspcs As Single
        Dim No_Of_Pcs As Integer = 0
        Dim q As Single = 0
        Dim Dt As New DataTable
        Dim Clo_Mtrs_Pc As Single = 0
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim Clo_IdNo As Integer = 0
        Dim edscnt_id As Integer = 0
        Dim cnt_id As Integer = 0
        Dim mtrs As Single = 0


        No_Of_Pcs = 0
        No_Of_Pcs = Val(txt_NoOfPcs.Text)

        Clo_IdNo = 0
        If Trim(cbo_Cloth.Text) <> "" Then
            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        End If

        If Val(Clo_IdNo) <> 0 And Val(No_Of_Pcs) <> 0 And dgv_Details.Columns(7).Visible = False Then

            Stock_In = ""
            mtrspcs = 0

            Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  Cloth_Head Where Cloth_idno = " & Str(Val(Clo_IdNo)), con)
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                Stock_In = Dt2.Rows(0)("Stock_In").ToString
                mtrspcs = Val(Dt2.Rows(0)("Meters_Pcs").ToString)
            End If
            Dt2.Clear()

            If Trim(UCase(Stock_In)) = "PCS" Then
                txt_ReceiptMeters.Text = Format(Val(No_Of_Pcs) * Val(mtrspcs), "########0.00")
            End If

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then '---- S.Ravichandran Textiles (Erode)
            ConsumedYarn_Calculation()
            ConsumedPavu_Calculation()
        End If

        If Val(txt_NoOfPcs.Tag) <> Val(txt_NoOfPcs.Text) Then
            txt_NoOfPcs.Tag = txt_NoOfPcs.Text
            Design_PieceDetails_Grid()
        End If

    End Sub

    Private Sub txt_NoOfPcs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_NoOfPcs.TextChanged
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
            ConsumedYarn_Calculation()
            ConsumedPavu_Calculation()
        End If
        PieceNo_To_Calculation()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1273" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1408" Then
            With dgv_Details
                If Val(txt_NoOfPcs.Text) = .Rows.Count Then
                    Exit Sub
                ElseIf Val(txt_NoOfPcs.Text) > .Rows.Count Then
                    For i = 1 To (Val(txt_NoOfPcs.Text) - .Rows.Count)
                        dgv_Details.Rows.Add()
                    Next
                End If
            End With

        End If

    End Sub

    Private Sub txt_PcsNoFrom_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_PcsNoFrom.TextChanged
        Try

            Grid_PieceNo_Generation()

        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub cbo_LoomNo_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, txt_ReceiptMeters, cbo_WidthType, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If txt_ReceiptMeters.Enabled And txt_ReceiptMeters.Visible Then
                txt_ReceiptMeters.Focus()
            Else
                txt_PcsNoFrom.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, cbo_WidthType, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_LoomNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New LoomNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LoomNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_WidthType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_LoomNo, cbo_Transport, "", "", "", "")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_LoomNo.Visible And cbo_LoomNo.Enabled Then
                cbo_LoomNo.Focus()
            ElseIf txt_ReceiptMeters.Visible And txt_ReceiptMeters.Enabled Then
                txt_ReceiptMeters.Focus()
            Else
                txt_NoOfPcs.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_WidthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, cbo_Transport, "", "", "", "")
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details
            If .Visible Then

                'If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(0).Value) = 0 Then
                '    e.Handled = True
                'End If

                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 7 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If



            End If
        End With

    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        dgv_Details_KeyUp(sender, e)
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        If e.Control = True And e.KeyValue = 13 Then
            If txt_ReceiptMeters.Enabled And txt_ReceiptMeters.Visible Then
                txt_ReceiptMeters.Focus()
            Else
                cbo_Transport.Focus()
            End If
        End If
    End Sub

    Private Sub dgv_BobinDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEndEdit
        dgv_BobinDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_BobinDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellLeave
        With dgv_BobinDetails
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
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
        If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub
        dgv_BobinDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_KuriDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellEndEdit
        dgv_KuriDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_KuriDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellLeave
        With dgv_KuriDetails
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
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
        If IsNothing(dgv_KuriDetails.CurrentCell) Then Exit Sub
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

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'If Common_Procedures.settings.CustomerCode = "1395" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then
        pnl_Back.Enabled = False
        pnl_Print.Visible = True
        If btn_Print_Receipt.Enabled And btn_Print_Receipt.Visible Then
            btn_Print_Receipt.Focus()
        End If
        'Else
        '    Printing_Receipt_CheckingReport()

        ' End If
    End Sub

    Private Sub Printing_Receipt_CheckingReport()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
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

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                'e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.5X12", 850, 1200)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

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
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                'ppd.ClientSize = New Size(600, 600)
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1
                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* ,c.* ,d.Cloth_Name, d.Cloth_Description ,E.EndsCount_Name , ig.Item_Hsn_Code from Weaver_Cloth_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno LEFT OUTER JOIN EndsCount_Head E ON E.EndsCount_IdNo = a.EndsCount_Idno INNER JOIN ItemGroup_head ig On ig.ItemGroup_Idno = d.ItemGroup_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Piece_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lot_Code = '" & Trim(NewCode) & "' and Create_Status = 1 ORDER BY PieceNo_OrderBy,Piece_No", con)
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

        If prn_Status = 1 Then
            Printing_Format_Half(e)

        Else

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
                Printing_Format2(e)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then '---- j.p.r palladam
                Printing_Format3(e)
                'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then ''BRT
                '    Printing_Format_Half(e)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then 'Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then '---- SANTHA EXPORTS (SOMANUR)
                Printing_Format4(e)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1408" Then '------AMIRTHAM
                Printing_Format_1408(e)

            Else
                Printing_Format1(e)

            End If

        End If




    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets, sno As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1 As Single, C2 As Single
        Dim pcsfr, pcsto As Integer


        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.5X12", 850, 1200)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

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

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 15
            .Right = 70
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        p1Font = New Font("Calibri", 10, FontStyle.Bold)

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

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(65) : ClAr(2) = 35 : ClAr(3) = 55 : ClAr(4) = 35 : ClAr(5) = 40 : ClAr(6) = 45 : ClAr(7) = 40 : ClAr(8) = 60 : ClAr(9) = 60 : ClAr(10) = 60 : ClAr(11) = 60 : ClAr(12) = 60 : ClAr(13) = 30 : ClAr(14) = 30
        ClAr(15) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9)
        C2 = C1 + ClAr(11)

        TxtHgt = 17



        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        sno = 0

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 5

                pcsfr = Val(prn_HdDt.Rows(0).Item("pcs_fromno").ToString)
                pcsto = Val(prn_HdDt.Rows(0).Item("pcs_tono").ToString)

                If prn_DetDt.Rows.Count > 0 Then

                    NoofItems_PerPage = 23

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets > NoofItems_PerPage Then
                            'CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PrintWidth - 10, CurY, 1, 0, pFont)

                            'NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If
                        'For I = prn_DetIndx To prn_DetDt.Rows.Count - 1


                        NoofDets = NoofDets + 1

                        'sno = sno + 1
                        'vType1 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Sound").ToString())
                        'vType2 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Seconds").ToString())
                        'vType3 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Bits").ToString())
                        'vType4 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Reject").ToString())
                        'vType5 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Others").ToString())


                        CurY = CurY + TxtHgt - 5
                        'Common_Procedures.Print_To_PrintDocument(e, Val(sno), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(vType1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(vType2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(vType3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(vType4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(vType5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Total").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt + 5
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                        'NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                        'Next I





                    Loop





                Else

                    NoofItems_PerPage = 23

                    If Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString) > 0 Then

                        Do While prn_DetIndx < Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString)

                            If NoofDets > NoofItems_PerPage Then

                                'CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued...", PrintWidth - 10, CurY, 1, 0, pFont)

                                'NoofDets = NoofDets + 1

                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            NoofDets = NoofDets + 1
                            CurY = CurY + TxtHgt - 5

                            'Common_Procedures.Print_To_PrintDocument(e, (Val(pcsfr) + Val(prn_DetIndx)), LMargin + 12, CurY, 0, 0, pFont)
                            CurY = CurY + TxtHgt + 5
                            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                            'NoofDets = NoofDets + 1

                            prn_DetIndx = prn_DetIndx + 1

                            'If Val(prn_DetIndx) > Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString) Then
                            '    Exit Do
                            'End If

                        Loop

                    End If


                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)
                If Trim(prn_InpOpts) <> "" Then
                    If prn_Count < Len(Trim(prn_InpOpts)) Then

                        'DetIndx = 0
                        prn_PageNo = 0
                        prn_DetIndx = 0
                        e.HasMorePages = True
                        Return
                    End If
                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim strHeight As Single
        Dim C1, w1, w2 As Single
        Dim NoofLooms As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING REPORT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 20

        w1 = e.Graphics.MeasureString("CHECKING PCS : ", pFont).Width
        w2 = e.Graphics.MeasureString("RECEIPT/LOT NO  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "NAME   :  " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "LOOMS", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        If Val(NoofLooms) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(NoofLooms)), LMargin + w1 + 30, CurY, 0, 0, pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT/LOT NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_No").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE  :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_Date")), "dd-MM-yyyy").ToString, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING PCS", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("noof_pcs").ToString, LMargin + w1 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT METERS", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "CHECKING", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PIECE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REC.", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LM.", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WIDTH", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "FOLD", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CHR ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SUP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MISTAKE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, 2, ClAr(15), pFont)




        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ING", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, 2, ClAr(15), pFont)



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY




    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer

        Dim w1 As Single

        For i = NoofDets + 1 To (NoofItems_PerPage - 2)
            CurY = CurY + TxtHgt - 5
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        Next

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), LnAr(3))

        If is_LastPage = True Then



            CurY = CurY + 10

            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1), CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIGNATURE ", PageWidth - 30, CurY, 1, 0, pFont)

            'vTotType1 = Val(prn_HdDt.Rows(0).Item("Total_Sound").ToString)
            'vTotType2 = Val(prn_HdDt.Rows(0).Item("Total_Seconds").ToString)
            'vTotType3 = Val(prn_HdDt.Rows(0).Item("Total_Bits").ToString)
            'vTotType4 = Val(prn_HdDt.Rows(0).Item("Total_Reject").ToString)
            'vTotType5 = Val(prn_HdDt.Rows(0).Item("Total_Others").ToString)

            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(4))


            CurY = CurY + 10

            w1 = e.Graphics.MeasureString("EXCESS METERS ", pFont).Width
            Common_Procedures.Print_To_PrintDocument(e, "SOUND METERS", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "SECOND METERS ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "REC.METERS ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "EXCESS METERS ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Excess_Short").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(5))


        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets, sno As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1 As Single, C2 As Single
        Dim pcsfr, pcsto As Integer


        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.5X12", 850, 1200)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            Exit For
        '        End If
        '    Next
        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 55
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        p1Font = New Font("Calibri", 10, FontStyle.Bold)

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

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(40) : ClAr(2) = 60 : ClAr(3) = 45 : ClAr(4) = 40 : ClAr(5) = 45
        ClAr(6) = 60 : ClAr(7) = 60 : ClAr(8) = 60 : ClAr(9) = 60 : ClAr(10) = 60 : ClAr(11) = 60 : ClAr(12) = 60 : ClAr(13) = 60
        ClAr(14) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13))

        'ClAr(1) = Val(65) : ClAr(2) = 35 : ClAr(3) = 55 : ClAr(4) = 35 : ClAr(5) = 40 : ClAr(6) = 45 : ClAr(7) = 40 : ClAr(8) = 60 : ClAr(9) = 60 : ClAr(10) = 60 : ClAr(11) = 60 : ClAr(12) = 60 : ClAr(13) = 30 : ClAr(14) = 30
        'ClAr(15) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14))

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9)
        C2 = C1 + ClAr(11)

        TxtHgt = 18

        NoofItems_PerPage = 21

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        sno = 0

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 5

                pcsfr = Val(prn_HdDt.Rows(0).Item("pcs_fromno").ToString)
                pcsto = Val(prn_HdDt.Rows(0).Item("pcs_tono").ToString)

                If prn_DetDt.Rows.Count > 0 Then

                    For I = 0 To prn_DetDt.Rows.Count - 1


                        NoofDets = NoofDets + 1


                        CurY = CurY + TxtHgt - 5
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, LMargin + 12, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) - 3, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt + 5
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Next I

                Else

                    For I = Val(pcsfr) To Val(pcsto)


                        NoofDets = NoofDets + 1
                        CurY = CurY + TxtHgt

                        'Common_Procedures.Print_To_PrintDocument(e, Val(I), LMargin + 12, CurY, 0, 0, pFont)
                        CurY = CurY + TxtHgt + 10
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1


                    Next I
                End If

                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim strHeight As Single
        Dim C1, w1, w2, w3 As Single
        Dim NoofLooms As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING REPORT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 20

        w1 = e.Graphics.MeasureString("CHECKING PCS : ", pFont).Width
        w2 = e.Graphics.MeasureString("RECEIPT/LOT NO  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "PARTY NAME  :  " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING DATE ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "LOT NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_No").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "LOT DATE  :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy").ToString, PageWidth - 15, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING PCS", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        If Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString), LMargin + w1 + 30, CurY, 0, 0, pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT METERS", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        If Val(prn_HdDt.Rows(0).Item("Receipt_Meters").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        End If

        w3 = e.Graphics.MeasureString("LOT DATE  :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy").ToString, pFont).Width

        Common_Procedures.Print_To_PrintDocument(e, "FOLDING  :  ", PageWidth - w3 - 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "PIECE", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REC.", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WIDTH", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type1) & " MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MISTAKE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)

        CurY = CurY + TxtHgt + 2
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY)
        LnAr(13) = CurY

        CurY = CurY + 2
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "A", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 5, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "B", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + 5, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "C", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + 5, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "D", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + 5, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim w1 As Single

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        Next

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(13))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(13))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(13))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(13))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), LnAr(3))

        CurY = CurY + 15

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)


        'vTotType1 = Val(prn_HdDt.Rows(0).Item("Total_Sound").ToString)
        'vTotType2 = Val(prn_HdDt.Rows(0).Item("Total_Seconds").ToString)
        'vTotType3 = Val(prn_HdDt.Rows(0).Item("Total_Bits").ToString)
        'vTotType4 = Val(prn_HdDt.Rows(0).Item("Total_Reject").ToString)
        'vTotType5 = Val(prn_HdDt.Rows(0).Item("Total_Others").ToString)

        'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY)
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY)
        LnAr(5) = CurY

        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(4))


        CurY = CurY + 15

        w1 = e.Graphics.MeasureString("EXCESS METERS ", pFont).Width
        Common_Procedures.Print_To_PrintDocument(e, "SOUND METERS", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "SECOND METERS ", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS ", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "REC.METERS ", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "EXCESS METERS ", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Excess_Short").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 15


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(5))

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If cbo_LoomType.Enabled Then
                If Trim(Common_Procedures.settings.CustomerCode) = "1040" Then
                    cbo_Weaver.Focus()
                Else
                    cbo_LoomType.Focus()
                End If
            ElseIf cbo_Weaver.Enabled Then
                cbo_Weaver.Focus()
            ElseIf txt_PDcNo.Enabled = True Then
                txt_PDcNo.Focus()

            Else
                txt_EBeam.Focus()
            End If
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            If cbo_LoomType.Enabled Then
                If Common_Procedures.settings.CustomerCode = "1040" Then
                    cbo_Weaver.Focus()
                Else
                    cbo_LoomType.Focus()
                End If
            ElseIf cbo_Weaver.Enabled Then
                cbo_Weaver.Focus()
            ElseIf txt_PDcNo.Enabled = True Then
                txt_PDcNo.Focus()
            Else
                txt_EBeam.Focus()
            End If
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            If chk_UNLOADEDBYOUREMPLOYEE.Enabled And chk_UNLOADEDBYOUREMPLOYEE.Visible Then
                chk_UNLOADEDBYOUREMPLOYEE.Focus()
            ElseIf cbo_Godown_StockIN.Enabled And cbo_Godown_StockIN.Visible Then
                cbo_Godown_StockIN.Focus()
            ElseIf cbo_StockOff.Enabled And cbo_StockOff.Visible Then
                cbo_StockOff.Focus()
            ElseIf txt_Freight.Enabled = True And txt_Freight.Visible Then
                txt_Freight.Focus()
            ElseIf Txt_NoOfBundles.Enabled = True And Txt_NoOfBundles.Visible Then
                Txt_NoOfBundles.Focus()
            ElseIf cbo_Transport.Enabled = True And cbo_Transport.Visible Then
                cbo_Transport.Focus()
            ElseIf cbo_LoomNo.Enabled = True And cbo_LoomNo.Visible Then
                cbo_LoomNo.Focus()
            ElseIf cbo_WidthType.Enabled = True And cbo_WidthType.Visible Then
                cbo_WidthType.Focus()
            ElseIf txt_ReceiptMeters.Enabled = True And txt_ReceiptMeters.Visible Then
                txt_ReceiptMeters.Focus()
            Else
                txt_PcsNoFrom.Focus()
            End If
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub cbo_StockOff_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_StockOff.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_StockOff_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_StockOff.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_StockOff, txt_Freight, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1 ) and Close_status = 0 ", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_StockOff.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_StockOff_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_StockOff.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_StockOff, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1 ) and Close_status = 0 ", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_StockOff_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_StockOff.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_StockOff.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
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
            MessageBox.Show("All entries saved Successfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets, sno As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1 As Single, C2 As Single
        Dim pcsfr, pcsto As Integer


        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.5X12", 850, 1200)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            Exit For
        '        End If
        '    Next
        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        p1Font = New Font("Calibri", 10, FontStyle.Bold)

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

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 40 : ClAr(3) = 50 : ClAr(4) = 50 : ClAr(5) = 50 : ClAr(6) = 60 : ClAr(7) = 60 : ClAr(8) = 60 : ClAr(9) = 60 : ClAr(10) = 50 : ClAr(11) = 60
        ClAr(12) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9)
        C2 = C1 + ClAr(11)

        TxtHgt = 18

        NoofItems_PerPage = 48

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        sno = 0

        'Try
        If prn_HdDt.Rows.Count > 0 Then

            Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

            NoofDets = 0

            CurY = CurY - 5

            pcsfr = Val(prn_HdDt.Rows(0).Item("pcs_fromno").ToString)
            pcsto = Val(prn_HdDt.Rows(0).Item("pcs_tono").ToString)

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                    ' For I = 0 To prn_DetDt.Rows.Count - 1

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt - 10

                        Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

                        e.HasMorePages = True
                        Return

                    End If

                    NoofDets = NoofDets + 1
                    sno = sno + 1
                    'vType1 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Sound").ToString())
                    'vType2 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Seconds").ToString())
                    'vType3 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Bits").ToString())
                    'vType4 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Reject").ToString())
                    'vType5 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Others").ToString())

                    CurY = CurY + TxtHgt - 5

                    'Common_Procedures.Print_To_PrintDocument(e, Val(sno), LMargin + 15, CurY, 0, 0, pFont)
                    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, LMargin + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 2, CurY, 1, 0, pFont)
                    End If
                    'Common_Procedures.Print_To_PrintDocument(e, Val(vType1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Val(vType2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Val(vType3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Val(vType4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Val(vType5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Total").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                    NoofDets = NoofDets + 1
                    prn_DetIndx = prn_DetIndx + 1

                    'Next I

                Loop

            Else

                For I = Val(pcsfr) To Val(pcsto)

                    NoofDets = NoofDets + 1
                    CurY = CurY + TxtHgt

                    Common_Procedures.Print_To_PrintDocument(e, Val(I), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt + 10
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                    NoofDets = NoofDets + 1
                    prn_DetIndx = prn_DetIndx + 1

                Next I
            End If

            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

        End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim strHeight As Single
        Dim C1, w1, w2 As Single
        Dim NoofLooms As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING REPORT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 20

        w1 = e.Graphics.MeasureString("CHECKING PCS : ", pFont).Width
        w2 = e.Graphics.MeasureString("RECEIPT/LOT NO  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "NAME   :  " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "LOOMS", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        If Val(NoofLooms) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(NoofLooms)), LMargin + w1 + 30, CurY, 0, 0, pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.EndsCount_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("EndsCount_IdNo").ToString))), LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING PCS", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("noof_pcs").ToString, LMargin + w1 + 30, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT/LOT NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_No").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE  :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy").ToString, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "PARTY DC NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + w1 + 30, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT METERS", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "PC", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CHE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "I", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "II", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "III", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "IV", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WIDTH", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REMARKS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "(PCS)", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), LnAr(3))

        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1), CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SIGNATURE ", PageWidth - 30, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(4))


        CurY = CurY + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_io_selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_io_selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Rate As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        'If LedIdNo = 0 Then
        '    MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
        '    Exit Sub
        'End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_io_selection
            If Val(LedIdNo) <> 0 Then

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name,c.*  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno  INNER JOIN Own_Order_Weaving_Details c ON  c.Own_Order_Code = a.Own_Order_Code LEFT OUTER JOIN Weaver_Cloth_Receipt_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'   and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Rate = 0


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString


                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = "1"
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno INNER JOIN Own_Order_Weaving_Details c ON  c.Own_Order_Code = a.Own_Order_Code   LEFT OUTER JOIN Weaver_Cloth_Receipt_Head d ON d.Weaver_ClothReceipt_Code = a.Own_Order_Code    where a.Weaver_ClothReceipt_Code = ''  and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                NR = Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("OWn_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString


                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
            Else
                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno   LEFT OUTER JOIN Weaver_Cloth_Receipt_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Rate = 0


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString

                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = "1"
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno    LEFT OUTER JOIN Weaver_Cloth_Receipt_Head d ON d.Weaver_ClothReceipt_Code = a.Own_Order_Code    where a.Weaver_ClothReceipt_Code = ''   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                NR = Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString

                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
            End If
        End With

        pnl_io_selection.Visible = True
        pnl_Back.Enabled = False
        dgv_io_selection.Focus()

    End Sub

    Private Sub dgv_io_selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_io_selection.CellClick
        Select_InternalOrder(e.RowIndex)
    End Sub

    Private Sub Select_InternalOrder(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_io_selection

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

    Private Sub dgv_io_selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_io_selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_io_selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_io_selection.CurrentCell.RowIndex

                Select_InternalOrder(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Close_io_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_io_Selection.Click
        Close_InternalOrder_Selection()
    End Sub

    Private Sub Close_InternalOrder_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        lbl_OrderNo.Text = ""
        lbl_OrderCode.Text = ""

        For i = 0 To dgv_io_selection.RowCount - 1

            If Val(dgv_io_selection.Rows(i).Cells(6).Value) = 1 Then

                ' lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(8).Value

                lbl_OrderNo.Text = dgv_io_selection.Rows(i).Cells(3).Value
                lbl_OrderCode.Text = dgv_io_selection.Rows(i).Cells(7).Value

            End If

        Next

        pnl_Back.Enabled = True
        pnl_io_selection.Visible = False

        If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()



    End Sub

    Private Sub cbo_LoomType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub
    Private Sub cbo_LoomType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomType, msk_date, cbo_Weaver, "", "", "", "")
    End Sub

    Private Sub cbo_LoomType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomType, cbo_Weaver, "", "", "", "")
    End Sub

    Private Sub cbo_Weaver_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver.LostFocus
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewNo As Integer = 0
        Dim Led_ID As Integer = 0
        Dim cmd As New SqlClient.SqlCommand
        Dim Dt As New DataTable
        Dim Dtbl1 As New DataTable
        Dim Bal As Decimal = 0
        Dim Amt As Double = 0, BillPend As Double = 0
        Dim count As String = ""
        Dim eNDS As String = ""


        If pnl_Weaver_Stock_Display.Visible = True And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1461" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1490" Then

            If Trim(UCase(cbo_Weaver.Tag)) <> Trim(UCase(cbo_Weaver.Text)) Then

                '----------- YARN
                Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

                cmd.Connection = con

                cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, name1, name2, weight1) Select a.DeliveryTo_Idno, tP.Ledger_Name, c.count_name, sum(a.Weight) from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON  a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & " and a.Weight <> 0 group by a.DeliveryTo_Idno, tP.Ledger_Name, c.count_name having sum(a.Weight) <> 0"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, name1, name2, weight1) Select a.ReceivedFrom_Idno, tP.Ledger_Name, c.count_name, -1*sum(a.Weight) from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON  a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & " and a.Weight <> 0 group by a.ReceivedFrom_Idno, tP.Ledger_Name, c.count_name having sum(a.Weight) <> 0"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, name1, name2, weight1) Select Int1, name1, name2, sum(weight1) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by Int1, name1, name2 having sum(Weight1) <> 0"
                cmd.ExecuteNonQuery()

                lbl_Yarn.Text = ""

                da = New SqlClient.SqlDataAdapter("select Int1, name1, name2, weight1 as wgt from " & Trim(Common_Procedures.ReportTempTable) & " ", con)
                Dtbl1 = New DataTable
                da.Fill(Dtbl1)
                count = ""
                If Dtbl1.Rows.Count > 0 Then
                    For i = 0 To Dtbl1.Rows.Count - 1
                        count = Trim(Dtbl1.Rows(i).Item("name2").ToString)
                        lbl_Yarn.Text = Trim(lbl_Yarn.Text) & " " & Trim(count) & " : " & Format(Val(Dtbl1.Rows(i).Item("wgt").ToString), "#######0.000")
                    Next i
                End If

                '-----------PAVU

                cmd.Connection = con

                cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, name1, name2, meters1) Select a.DeliveryTo_Idno, tP.Ledger_Name, c.endscount_name, sum(a.Meters) from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON  a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & "  and a.Meters <> 0 group by a.DeliveryTo_Idno, tP.Ledger_Name, c.endscount_name having sum(a.Meters) <> 0"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, name1, name2, meters1) Select a.ReceivedFrom_Idno, tP.Ledger_Name, c.endscount_name, -1*sum(a.Meters) from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & "  and a.Meters <> 0 group by a.ReceivedFrom_Idno, tP.Ledger_Name, c.endscount_name having sum(a.Meters) <> 0"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, name1, name3, meters1) Select Int1, name1, name2, sum(meters1) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by Int1, name1, name2 having sum(meters1) <> 0"
                cmd.ExecuteNonQuery()

                lbl_Pavu.Text = ""

                da = New SqlClient.SqlDataAdapter("select Int1, name1, name3, meters1 from " & Trim(Common_Procedures.ReportTempTable) & " ", con)
                Dtbl1 = New DataTable
                da.Fill(Dtbl1)
                eNDS = ""
                If Dtbl1.Rows.Count > 0 Then
                    For i = 0 To Dtbl1.Rows.Count - 1
                        eNDS = Trim(Dtbl1.Rows(i).Item("name3").ToString)
                        lbl_Pavu.Text = Trim(lbl_Pavu.Text) & " " & Trim(eNDS) & " : " & Format(Val(Dtbl1.Rows(i).Item("meters1").ToString), "#######0.00")
                    Next i
                End If


                '-------- Empty Beam
                cmd.Connection = con

                cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, Name1, Int2) Select a.DeliveryTo_Idno, tP.Ledger_Name,  sum(a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & " and  (a.Empty_Beam+a.Pavu_Beam) <> 0 group by a.DeliveryTo_Idno, tP.Ledger_Name having sum(a.Empty_Beam+a.Pavu_Beam) <> 0 "
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, Name1, Int2) Select a.ReceivedFrom_Idno, tP.Ledger_Name,  -1*sum(a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & " and (a.Empty_Beam+a.Pavu_Beam) <> 0 group by a.ReceivedFrom_Idno, tP.Ledger_Name having sum(a.Empty_Beam+a.Pavu_Beam) <> 0 "
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, Name1, Int2) Select Int1, Name1,  sum(Int2) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by Int1, Name1  having sum(Int2) <> 0 "
                cmd.ExecuteNonQuery()

                lbl_EmptyBeam.Text = ""

                da = New SqlClient.SqlDataAdapter("select Int1, name1, Int2 from " & Trim(Common_Procedures.ReportTempTable) & " ", con)
                Dtbl1 = New DataTable
                da.Fill(Dtbl1)

                If Dtbl1.Rows.Count > 0 Then
                    For i = 0 To Dtbl1.Rows.Count - 1
                        lbl_EmptyBeam.Text = Val(Dtbl1.Rows(i).Item("Int2").ToString) & " Beams"
                    Next i
                End If
                Dt.Dispose()
                da.Dispose()

            End If

        End If

        cbo_Weaver.Tag = cbo_Weaver.Text

    End Sub

    Private Sub btn_Close_DriverDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_DriverDetails.Click
        pnl_DriverDetails.Visible = False
        pnl_Back.Enabled = True
        If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus()
    End Sub

    Private Sub cbo_DriverName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DriverName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Cloth_Receipt_Head", "Driver_Name", "", "")
    End Sub

    Private Sub cbo_DriverName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DriverName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DriverName, cbo_SupervisorName, cbo_VehicleNo, "Weaver_Cloth_Receipt_Head", "Driver_Name", "", "")
    End Sub

    Private Sub cbo_DriverName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DriverName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DriverName, cbo_VehicleNo, "Weaver_Cloth_Receipt_Head", "Driver_Name", "", "", False)
    End Sub

    Private Sub cbo_DriverPhNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DriverPhNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Cloth_Receipt_Head", "Driver_Phone_No", "", "")
    End Sub

    Private Sub cbo_DriverPhNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DriverPhNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DriverPhNo, cbo_DriverName, cbo_SupervisorName, "Weaver_Cloth_Receipt_Head", "Driver_Phone_No", "", "")
    End Sub

    Private Sub cbo_DriverPhNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DriverPhNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DriverPhNo, cbo_SupervisorName, "Weaver_Cloth_Receipt_Head", "Driver_Phone_No", "", "", False)
    End Sub

    Private Sub cbo_SupervisorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SupervisorName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Cloth_Receipt_Head", "Supervisor_Name", "", "")
    End Sub

    Private Sub cbo_SupervisorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SupervisorName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SupervisorName, Nothing, cbo_DriverName, "Weaver_Cloth_Receipt_Head", "Supervisor_Name", "", "")
    End Sub

    Private Sub cbo_SupervisorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SupervisorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SupervisorName, cbo_DriverName, "Weaver_Cloth_Receipt_Head", "Supervisor_Name", "", "", False)
    End Sub

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Cloth_Receipt_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, txt_Freight, txt_EWayBillNo, "Weaver_Cloth_Receipt_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_EWayBillNo, "Weaver_Cloth_Receipt_Head", "Vehicle_No", "", "", False)
    End Sub

    Private Sub btn_DriverDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_DriverDetails.Click
        pnl_DriverDetails.Visible = True
        pnl_Back.Enabled = False
        If cbo_SupervisorName.Visible And cbo_SupervisorName.Enabled Then cbo_SupervisorName.Focus()
    End Sub

    Private Sub cbo_Godown_StockIN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Godown_StockIN.GotFocus

        cbo_Godown_StockIN.Tag = cbo_Godown_StockIN.Text

        If Common_Procedures.settings.Multi_Godown_Status Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " Close_status = 0 ", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_Godown_StockIN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockIN.KeyDown

        If Common_Procedures.settings.Multi_Godown_Status Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Godown_StockIN, txt_Freight, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(  Close_status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Godown_StockIN, txt_Freight, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        End If

        If (e.KeyValue = 40 And cbo_Godown_StockIN.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If (Common_Procedures.settings.CustomerCode = "1267") Then
                chk_UNLOADEDBYOUREMPLOYEE.Focus()
            ElseIf cbo_Delivery_Purpose.Visible And cbo_Delivery_Purpose.Enabled Then
                cbo_Delivery_Purpose.Focus()
            ElseIf txt_Rate.Visible Then
                txt_Rate.Focus()
            ElseIf MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
            End If

        End If

    End Sub

    Private Sub cbo_Godown_StockIN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Godown_StockIN.KeyPress

        If Common_Procedures.settings.Multi_Godown_Status Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Godown_StockIN, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( Close_status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Godown_StockIN, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        End If

        If Asc(e.KeyChar) = 13 Then
            If (Common_Procedures.settings.CustomerCode = "1267") Then
                chk_UNLOADEDBYOUREMPLOYEE.Focus()
            ElseIf cbo_Delivery_Purpose.Visible And cbo_Delivery_Purpose.Enabled Then
                cbo_Delivery_Purpose.Focus()
            ElseIf txt_Rate.Visible Then
                txt_Rate.Focus()
            ElseIf MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Godown_StockIN_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockIN.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            If Common_Procedures.settings.Multi_Godown_Status Then
                Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
                Common_Procedures.MDI_LedType = "GODOWN"
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Godown_StockIN.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            Else

                Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
                Dim f As New Ledger_Creation
                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Godown_StockIN.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If
        End If
    End Sub

    Private Sub Txt_NoOfBundles_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Txt_NoOfBundles.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub Design_PieceDetails_Grid()
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1007" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1490" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1549" Then '---- Rajeswari Weaving (Karumanthapatti) - Somanur Sizing

            If Val(txt_NoOfPcs.Text) <> 0 Then
                N = dgv_Details.Rows.Count

                If N < Val(txt_NoOfPcs.Text) Then

                    For I = N + 1 To Val(txt_NoOfPcs.Text)
                        dgv_Details.Rows.Add()
                    Next I

                Else

LOOP1:

                    For J = Val(txt_NoOfPcs.Text) - 1 To dgv_Details.Rows.Count - 1

                        If J = dgv_Details.Rows.Count - 1 Then
                            For I = 0 To dgv_Details.Columns.Count - 1
                                dgv_Details.Rows(J).Cells(I).Value = ""
                            Next

                        Else
                            dgv_Details.Rows.RemoveAt(J)
                            GoTo LOOP1

                        End If

                    Next

                End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1490" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
                    Grid_PieceNo_Generation()
                End If

            End If

        End If

    End Sub

    Private Sub Grid_PieceNo_Generation()
        Dim i As Integer = 0
        Dim PcFrmNo As Integer = 0
        Dim Led_ID As Integer = 0
        Dim vLEDShtNm As String = ""
        Dim vPCSNO As String = ""


        Try

            PieceNo_To_Calculation()

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1490" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)

                'vLEDShtNm = ""
                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
                '    Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
                '    If Led_ID <> 0 Then
                '        vLEDShtNm = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_ShortName", "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")")
                '    End If
                'End If

                With dgv_Details
                    If .Rows.Count > 0 Then

                        PcFrmNo = Val(txt_PcsNoFrom.Text)
                        If PcFrmNo = 0 Then PcFrmNo = 1

                        .Rows(0).Cells(0).Value = Trim(vLEDShtNm) & Trim(Val(PcFrmNo))

                        For i = 1 To .Rows.Count - 1
                            If Trim(vLEDShtNm) <> "" Then
                                vPCSNO = Replace(Trim(UCase(.Rows(i - 1).Cells(0).Value)), Trim(UCase(vLEDShtNm)), "")
                                .Rows(i).Cells(0).Value = Trim(vLEDShtNm) & Trim(Val(vPCSNO) + 1)
                            Else
                                .Rows(i).Cells(0).Value = Trim(Val(.Rows(i - 1).Cells(0).Value) + 1)

                            End If
                        Next

                    End If

                End With

            End If

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub txt_Folding_Perc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding_Perc.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            cbo_Cloth.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True
            txt_PDcNo.Focus()
        End If
    End Sub

    Private Sub txt_Folding_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding_Perc.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            txt_PDcNo.Focus()
        End If
    End Sub

    Private Sub txt_PDcNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PDcNo.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            If txt_Folding_Perc.Visible Then
                txt_Folding_Perc.Focus()
            ElseIf cbo_Cloth.Enabled Then
                cbo_Cloth.Focus()
            ElseIf cbo_LoomType.Enabled Then
                cbo_LoomType.Focus()
            Else
                msk_date.Focus()
            End If

        End If

        If e.KeyCode = 40 Then
            e.Handled = True
            cbo_EndsCount.Focus()
        End If

    End Sub

    Private Sub txt_PDcNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PDcNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_EndsCount.Focus()
        End If
    End Sub

    Private Sub cbo_Cloth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.TextChanged
        Dim Clo_IdNo As Integer, edscnt_idno As Integer
        Dim wftcnt_idno As Integer
        Dim edscnt_id As Integer = 0
        Dim cnt_id As Integer = 0
        Dim mtrs As Single = 0

        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

        wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
        lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, wftcnt_idno)


        edscnt_idno = Val(Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "EndsCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & " and Sl_No = 1 )"))
        cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, edscnt_idno)

        Consumption_Calculation()
        Grid_Cell_DeSelect()
    End Sub

    Private Sub chk_UNLOADEDBYOUREMPLOYEE_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles chk_UNLOADEDBYOUREMPLOYEE.KeyDown

        If e.KeyValue = 38 Then
            e.Handled = True
            cbo_Godown_StockIN.Focus()


        End If

        If e.KeyValue = 40 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
            End If
            'txt_Remarks.Focus()

        End If
    End Sub

    Private Sub chk_UNLOADEDBYOUREMPLOYEE_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles chk_UNLOADEDBYOUREMPLOYEE.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
            End If
            ' txt_Remarks.Focus()
        End If
    End Sub




    Private Sub btn_SMS_Click(sender As System.Object, e As System.EventArgs) Handles btn_SMS.Click
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = "", EndsCount As String = "", Cloth As String = ""
        Dim Led_IdNo As Integer = 0, Endscount_IdNo As Integer = 0, Cloth_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim BlNos As String = ""

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            Endscount_IdNo = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
            EndsCount = ""
            If Val(Endscount_IdNo) <> 0 Then
                EndsCount = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_name", "(EndsCount_IdNo = " & Str(Val(Endscount_IdNo)) & ")")
            End If

            Cloth_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
            Cloth = ""
            If Val(Cloth_IdNo) <> 0 Then
                Cloth = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_name", "(Cloth_IdNo = " & Str(Val(Cloth_IdNo)) & ")")
            End If
            ' If Trim(AgPNo) <> "" Then
            PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", "", "")
            ' End If

            ' smstxt = Trim(cbo_.Text) & vbCrLf
            smstxt = smstxt & " Lot No : " & Trim(lbl_RefNo.Text) & vbCrLf
            smstxt = smstxt & " Date : " & Trim(msk_date.Text) & vbCrLf

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            '    If Trim(cbo_Transport.Text) <> "" Then
            '        smstxt = smstxt & " Transport : " & Trim(cbo_Transport.Text) & vbCrLf
            '    End If

            'End If
            'If dgv_Details_Total.RowCount > 0 Then
            '    smstxt = smstxt & " BEAM: " & Val((dgv_Details_Total.Rows(0).Cells(2).Value())) & vbCrLf
            '    'smstxt = smstxt & " WEIGHT: " & Val((dgv_PavuDetails_Total.Rows(0).Cells(6).Value())) & vbCrLf


            '    smstxt = smstxt & " METERS  : " & Val(dgv_Details_Total.Rows(0).Cells(6).Value()) & vbCrLf
            'End If

            'If dgv_Details.RowCount > 0 Then
            '    ' smstxt = smstxt & " Beam No: " & Trim((dgv_PavuDetails.Rows(0).Cells(3).Value())) & vbCrLf
            '    smstxt = smstxt & "Ends Count : " & Trim((dgv_YarnDetails.Rows(0).Cells(3).Value())) & vbCrLf

            '    smstxt = smstxt & " ENDS COUNT  : " & Val(dgv_Details.Rows(0).Cells(3).Value()) & vbCrLf


            'End If
            smstxt = smstxt & " Cloth : " & Trim(Cloth) & vbCrLf
            smstxt = smstxt & " Ends Count : " & Trim(EndsCount) & vbCrLf
            'smstxt = smstxt & " Tax Amount : " & Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text) & vbCrLf
            smstxt = smstxt & " Meters : " & Trim(txt_ReceiptMeters.Text) & vbCrLf
            smstxt = smstxt & " No.Of Pcs : " & Trim(txt_NoOfPcs.Text) & vbCrLf
            smstxt = smstxt & " " & vbCrLf
            smstxt = smstxt & " Thanks! " & vbCrLf
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

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets, sno As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1 As Single, C2 As Single
        Dim pcsfr, pcsto As Integer


        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.5X12", 850, 1200)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            Exit For
        '        End If
        '    Next
        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 15
            .Right = 70
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        p1Font = New Font("Calibri", 10, FontStyle.Bold)

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

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(50) : ClAr(2) = 65 : ClAr(3) = 35 : ClAr(4) = 40 : ClAr(5) = 45 : ClAr(6) = 65 : ClAr(7) = 60 : ClAr(8) = 60 : ClAr(9) = 60 : ClAr(10) = 60 : ClAr(11) = 35 : ClAr(12) = 35
        ClAr(13) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12))

        'ClAr(1) = Val(65) : ClAr(2) = 35 : ClAr(3) = 55 : ClAr(4) = 35 : ClAr(5) = 40 : ClAr(6) = 45 : ClAr(7) = 40 : ClAr(8) = 60 : ClAr(9) = 60 : ClAr(10) = 60 : ClAr(11) = 60 : ClAr(12) = 60 : ClAr(13) = 30 : ClAr(14) = 30
        'ClAr(15) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9)
        C2 = C1 + ClAr(11)

        TxtHgt = 17



        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        sno = 0

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format4_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 5

                pcsfr = Val(prn_HdDt.Rows(0).Item("pcs_fromno").ToString)
                pcsto = Val(prn_HdDt.Rows(0).Item("pcs_tono").ToString)

                If prn_DetDt.Rows.Count > 0 Then

                    NoofItems_PerPage = 23

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets > NoofItems_PerPage Then
                            'CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PrintWidth - 10, CurY, 1, 0, pFont)

                            'NoofDets = NoofDets + 1

                            Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If
                        'For I = prn_DetIndx To prn_DetDt.Rows.Count - 1


                        NoofDets = NoofDets + 1



                        CurY = CurY + TxtHgt - 5
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString), LMargin + ClAr(1) + ClAr(2) - 5, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt + 5
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


                        prn_DetIndx = prn_DetIndx + 1


                    Loop

                Else

                    NoofItems_PerPage = 23

                    If Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString) > 0 Then

                        Do While prn_DetIndx < Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString)

                            If NoofDets > NoofItems_PerPage Then

                                'CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued...", PrintWidth - 10, CurY, 1, 0, pFont)

                                'NoofDets = NoofDets + 1

                                Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            NoofDets = NoofDets + 1
                            CurY = CurY + TxtHgt - 5

                            'Common_Procedures.Print_To_PrintDocument(e, (Val(pcsfr) + Val(prn_DetIndx)), LMargin + 12, CurY, 0, 0, pFont)
                            CurY = CurY + TxtHgt + 5
                            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                            'NoofDets = NoofDets + 1

                            prn_DetIndx = prn_DetIndx + 1

                            'If Val(prn_DetIndx) > Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString) Then
                            '    Exit Do
                            'End If

                        Loop

                    End If


                End If


                Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)
                If Trim(prn_InpOpts) <> "" Then
                    If prn_Count < Len(Trim(prn_InpOpts)) Then

                        'DetIndx = 0
                        prn_PageNo = 0
                        prn_DetIndx = 0
                        e.HasMorePages = True
                        Return
                    End If
                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format4_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim strHeight As Single
        Dim C1, w1, w2, w3 As Single
        Dim NoofLooms As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING REPORT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 20

        w1 = e.Graphics.MeasureString("CHECKING PCS : ", pFont).Width
        w2 = e.Graphics.MeasureString("RECEIPT/LOT NO  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "NAME   :  " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)



        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "LOOMS", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        If Val(NoofLooms) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(NoofLooms)), LMargin + w1 + 30, CurY, 0, 0, pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT/LOT NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_No").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT DATE  :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_Date")), "dd-MM-yyyy").ToString, PageWidth - 15, CurY, 1, 0, pFont)

        w3 = e.Graphics.MeasureString("RECEIPT.DATE  :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_Date")), "dd-MM-yyyy").ToString, pFont).Width

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING DATE : ", LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "CHECKING PCS : " & Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString), LMargin + C1 - 20, CurY, 1, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "CHECKING PCS", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("noof_pcs").ToString, LMargin + w1 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT METERS", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "FOLDING  :  ", PageWidth - w3 - 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "PIECE", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REC.", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LM.", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WIDTH", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MARK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CHR ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SUP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MISTAKE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)




        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "(Y/N)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "(Kg)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)




        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY




    End Sub

    Private Sub Printing_Format4_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer

        Dim w1 As Single

        For i = NoofDets + 1 To (NoofItems_PerPage - 2)
            CurY = CurY + TxtHgt - 5
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        Next

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), LnAr(3))

        If is_LastPage = True Then



            CurY = CurY + 10

            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1), CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIGNATURE ", PageWidth - 30, CurY, 1, 0, pFont)

            'vTotType1 = Val(prn_HdDt.Rows(0).Item("Total_Sound").ToString)
            'vTotType2 = Val(prn_HdDt.Rows(0).Item("Total_Seconds").ToString)
            'vTotType3 = Val(prn_HdDt.Rows(0).Item("Total_Bits").ToString)
            'vTotType4 = Val(prn_HdDt.Rows(0).Item("Total_Reject").ToString)
            'vTotType5 = Val(prn_HdDt.Rows(0).Item("Total_Others").ToString)

            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(4))


            CurY = CurY + 10

            w1 = e.Graphics.MeasureString("EXCESS METERS ", pFont).Width
            Common_Procedures.Print_To_PrintDocument(e, "SOUND METERS", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "SECOND METERS ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "REC.METERS ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "EXCESS METERS ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Excess_Short").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(5))


        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub
    Private Sub Printing_Format_Half(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ItmNm1 As String, ItmNm2 As String


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 30 
            .Right = 40
            .Top = 25 ' 30 ' 50 
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        NoofItems_PerPage = 3 ' 4 ' 6

        Erase LnAr
        Erase ClAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(60) : ClAr(2) = 300 : ClAr(3) = 140 : ClAr(4) = 140
        ClAr(5) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4))

        TxtHgt = 17

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_Half_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_HdDt.Rows.Count > 0 Then

                    'Do While prn_DetIndx <= prn_HdDt.Rows.Count - 1

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_Format_Half_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                        e.HasMorePages = True
                        Return

                    End If

                    ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Description").ToString)
                    If TRIM(ItmNm1) = "" Then
                        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Name").ToString)
                    End If




                    ItmNm2 = ""
                    If Len(ItmNm1) > 30 Then
                        For I = 30 To 1 Step -1
                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 30
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                    End If

                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "1", LMargin + 15, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then '---- SANTHA EXPORTS (SOMANUR)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.EndsCount_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("EndsCount_IdNo").ToString)).ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Item_Hsn_Code").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    End If

                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Dc_Receipt_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

                    'old
                    'Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Receipt_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Receipt_Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)


                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1

                    If Trim(ItmNm2) <> "" Then
                        CurY = CurY + TxtHgt - 5
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        NoofDets = NoofDets + 1
                    End If

                    'prn_DetIndx = prn_DetIndx + 1

                    'Loop

                End If

                Printing_Format_Half_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_Half_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1, w2 As Single
        Dim S1, s2 As Single
        Dim Cmp_GSTIN_No As String

        PageNo = PageNo + 1

        CurY = TMargin

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from Cloth_Purchase_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        'da2.Fill(dt2)

        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

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
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            '.BackgroundImage = Image.FromStream(ms)

                            ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY + 5, 110, 100)

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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth - 70, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, PageWidth - 40, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "WEAVER CLOTH RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)


        CurY = CurY + strHeight - 3
        p1Font = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin + 10, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SAC CODE : 998821 ( Textile manufactring service )", PageWidth - 10, CurY, 1, 0, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("E-Way Bill No.   : ", pFont).Width
        w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Party DC NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No.", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, " PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No.", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then '---- SANTHA EXPORTS (SOMANUR)
            Common_Procedures.Print_To_PrintDocument(e, "ENDSCOUNT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "HSNCODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub


    Private Sub Printing_Format_Half_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim rndoff As Double, TtAmt As Double
        Dim BmsInWrds As String

        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0

        For i = NoofDets + 1 To NoofItems_PerPage

            CurY = CurY + TxtHgt

            prn_DetIndx = prn_DetIndx + 1

        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10

        Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Dc_Receipt_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)


        CurY = CurY + TxtHgt + 10



        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        CurY = CurY + 10

        '    If (Val(prn_HdDt.Rows(0).Item("Count_IdNo").ToString)) <> 0 Then
        '        Common_Procedures.Print_To_PrintDocument(e, " Weft Count   :   " & Common_Procedures.Count_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Count_IdNo").ToString)), LMargin + 30, CurY, 0, 0, pFont)
        '        '
        '    End If


        If Val(prn_HdDt.Rows(0).Item("Rate").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Rate/Meters : " & Val(prn_HdDt.Rows(0).Item("Rate").ToString), LMargin + 30, CurY, 2, 0, pFont)
        End If

        If Val(prn_HdDt.Rows(0).Item("Amount").ToString) <> 0 Then
            ' Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount : " & Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + 185, CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Value of Goods : " & Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + 185, CurY, 2, 0, pFont)
        End If

        If Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

            If Val(prn_HdDt.Rows(0).Item("Amount").ToString) <> 0 Then

                vCgst_amt = Format((Val(prn_HdDt.Rows(0).Item("Amount").ToString) * 2.5 / 100), "############0.00")
                vSgst_amt = Format((Val(prn_HdDt.Rows(0).Item("Amount").ToString) * 2.5 / 100), "############0.00")

                Common_Procedures.Print_To_PrintDocument(e, " CGST 2.5 % : " & vCgst_amt, LMargin + 380, CurY, 2, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " SGST 2.5 % : " & vSgst_amt, LMargin + 530, CurY, 2, 0, pFont)

            End If

            CurY = CurY + TxtHgt + 10

            If Val(prn_HdDt.Rows(0).Item("Amount").ToString) <> 0 Then
                vTxamt = Val(vCgst_amt) + Val(vSgst_amt)  'Format((Val(prn_HdDt.Rows(0).Item("Amount").ToString) * 5 / 100), "############0.00")
                Common_Procedures.Print_To_PrintDocument(e, " Tax Amount : " & vTxamt, LMargin + 30, CurY, 2, 0, pFont)
            End If

            If Val(vTxamt) <> 0 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) + vTxamt, "###########0.00")
                Common_Procedures.Print_To_PrintDocument(e, " Net Amount : " & vNtAMt, LMargin + 185, CurY, 2, 0, pFont)
            End If

        End If


        CurY = CurY + 10
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_Close_Print_Click(sender As Object, e As EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Receipt_Click(sender As Object, e As EventArgs) Handles btn_Print_Receipt.Click
        prn_Status = 1
        Printing_Receipt_CheckingReport()
        btn_Close_Print_Click(sender, e)
    End Sub

    Private Sub btn_PrintDetail_Click(sender As Object, e As EventArgs) Handles btn_PrintDetail.Click
        prn_Status = 2
        Printing_Receipt_CheckingReport()
        btn_Close_Print_Click(sender, e)
    End Sub

    Private Sub Printing_Format_1408(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets, sno As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1 As Single, C2 As Single
        Dim pcsfr, pcsto As Integer


        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.5X12", 850, 1200)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            Exit For
        '        End If
        '    Next
        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 15
            .Right = 70
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        p1Font = New Font("Calibri", 10, FontStyle.Bold)

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

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(65) : ClAr(2) = 35 : ClAr(3) = 55 : ClAr(4) = 35 : ClAr(5) = 40 : ClAr(6) = 45 : ClAr(7) = 40 : ClAr(8) = 60 : ClAr(9) = 60 : ClAr(10) = 60 : ClAr(11) = 60 : ClAr(12) = 60 : ClAr(13) = 30 : ClAr(14) = 30
        ClAr(15) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14))

        ClAr(1) = Val(65) : ClAr(2) = 80 : ClAr(3) = 55 : ClAr(4) = 140 : ClAr(5) = 140 : ClAr(6) = 140 ': ClAr(7) = 40 : ClAr(8) = 60 : ClAr(9) = 60 : ClAr(10) = 60 : ClAr(11) = 60 : ClAr(12) = 60 : ClAr(13) = 30 : ClAr(14) = 30
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))


        'ClAr(4) = ClAr(4) + ClAr(5) + ClAr(6)
        'ClAr(7) = ClAr(8) + ClAr(9) + ClAr(9)

        C1 = ClAr(1) + ClAr(2) + ClAr(3) '+ 'ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9)
        C2 = C1 + ClAr(6)

        TxtHgt = 15



        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        sno = 0

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_1408_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 5

                pcsfr = Val(prn_HdDt.Rows(0).Item("pcs_fromno").ToString)
                pcsto = Val(prn_HdDt.Rows(0).Item("pcs_tono").ToString)

                If prn_DetDt.Rows.Count > 0 Then

                    NoofItems_PerPage = 29 ' 33

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets > NoofItems_PerPage Then
                            'CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PrintWidth - 10, CurY, 1, 0, pFont)

                            'NoofDets = NoofDets + 1

                            Printing_Format_1408_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If
                        'For I = prn_DetIndx To prn_DetDt.Rows.Count - 1


                        NoofDets = NoofDets + 1

                        prn_DetSNo = prn_DetSNo + 1
                        'vType1 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Sound").ToString())
                        'vType2 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Seconds").ToString())
                        'vType3 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Bits").ToString())
                        'vType4 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Reject").ToString())
                        'vType5 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Others").ToString())


                        CurY = CurY + TxtHgt - 5
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetSNo), LMargin + 15, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString), LMargin + ClAr(1) + ClAr(2) - 5, CurY, 1, 0, pFont)
                        End If

                        'Common_Procedures.Print_To_PrintDocument(e, Val(vType1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(vType2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(vType3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(vType4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(vType5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Total").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt + 5
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                        'NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                        'Next I





                    Loop





                Else

                    NoofItems_PerPage = 33

                    If Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString) > 0 Then

                        Do While prn_DetIndx < Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString)

                            If NoofDets > NoofItems_PerPage Then

                                'CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued...", PrintWidth - 10, CurY, 1, 0, pFont)

                                'NoofDets = NoofDets + 1

                                Printing_Format_1408_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            NoofDets = NoofDets + 1
                            CurY = CurY + TxtHgt - 5

                            'Common_Procedures.Print_To_PrintDocument(e, (Val(pcsfr) + Val(prn_DetIndx)), LMargin + 12, CurY, 0, 0, pFont)
                            CurY = CurY + TxtHgt + 5
                            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                            'NoofDets = NoofDets + 1

                            prn_DetIndx = prn_DetIndx + 1

                            'If Val(prn_DetIndx) > Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString) Then
                            '    Exit Do
                            'End If

                        Loop

                    End If


                End If
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 5, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Receipt_Meters").ToString), LMargin + ClAr(1) + ClAr(2) - 5, CurY, 1, 0, p1Font)
                p1Font = New Font("Calibri", 10, FontStyle.Bold)

                Printing_Format_1408_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)
                If Trim(prn_InpOpts) <> "" Then
                    If prn_Count < Len(Trim(prn_InpOpts)) Then

                        'DetIndx = 0
                        prn_PageNo = 0
                        prn_DetIndx = 0
                        e.HasMorePages = True
                        Return
                    End If
                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_1408_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim strHeight As Single
        Dim C1, w1, w2 As Single
        Dim NoofLooms As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING REPORT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20

        w1 = e.Graphics.MeasureString("CHECKING PCS : ", pFont).Width
        w2 = e.Graphics.MeasureString("RECEIPT/LOT NO  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "NAME  ", LMargin + 10, CurY, 0, 0, pFont)
        pFont = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("empty_beam").ToString, LMargin + w1 + 30, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT/LOT NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_No").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE  :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_Date")), "dd-MM-yyyy").ToString, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT METERS", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ": ", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + w1 + 40, CurY, 0, 0, pFont)



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SECONDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REMARKS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.ClothType.Type5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "CHR ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "SUP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "MISTAKE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, 2, ClAr(15), pFont)




        CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "ING", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, 2, ClAr(15), pFont)



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format_1408_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer

        Dim w1 As Single

        For i = NoofDets + 1 To (NoofItems_PerPage - 2)
            CurY = CurY + TxtHgt - 5
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        Next

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) / 2), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) / 2), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))


        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + (ClAr(5) / 2), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + (ClAr(5) / 2), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))


        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + (ClAr(6) / 2), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + (ClAr(6) / 2), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))

        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), LnAr(3))

        If is_LastPage = True Then



            CurY = CurY + 10

            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1), CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "SIGNATURE ", PageWidth - 30, CurY, 1, 0, pFont)

            'vTotType1 = Val(prn_HdDt.Rows(0).Item("Total_Sound").ToString)
            'vTotType2 = Val(prn_HdDt.Rows(0).Item("Total_Seconds").ToString)
            'vTotType3 = Val(prn_HdDt.Rows(0).Item("Total_Bits").ToString)
            'vTotType4 = Val(prn_HdDt.Rows(0).Item("Total_Reject").ToString)
            'vTotType5 = Val(prn_HdDt.Rows(0).Item("Total_Others").ToString)

            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(vTotType5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(4))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(4))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(4))


            'CurY = CurY + 10

            'w1 = e.Graphics.MeasureString("EXCESS METERS ", pFont).Width
            'Common_Procedures.Print_To_PrintDocument(e, "SOUND METERS", LMargin + 10, CurY, 0, 0, pFont)
            ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "SECOND METERS ", LMargin + 10, CurY, 0, 0, pFont)
            ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS ", LMargin + 10, CurY, 0, 0, pFont)
            ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "REC.METERS ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Receipt_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "EXCESS METERS ", LMargin + 10, CurY, 0, 0, pFont)
            ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Excess_Short").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            'CurY = CurY + TxtHgt + 10

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(5))


        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub cbo_LoomType_TextChanged(sender As Object, e As EventArgs) Handles cbo_LoomType.TextChanged
        Set_LoomType_LoomNo_WidthType()
    End Sub

    Private Sub cbo_LoomType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_LoomType.SelectedIndexChanged
        Set_LoomType_LoomNo_WidthType()
    End Sub


    Private Sub Set_LoomType_LoomNo_WidthType()

        If Trim(UCase(cbo_LoomType.Text)) = "AUTOLOOM" Or Trim(UCase(cbo_LoomType.Text)) = "AUTO LOOM" Then
            If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then
                cbo_LoomNo.Enabled = False
                cbo_WidthType.Enabled = False
            Else
                cbo_LoomNo.Enabled = True
                cbo_WidthType.Enabled = True
            End If

        Else
            cbo_LoomNo.Enabled = False
            cbo_WidthType.Enabled = False
        End If

    End Sub

    Private Sub txt_PcsNoFrom_Enter(sender As Object, e As EventArgs) Handles txt_PcsNoFrom.Enter
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim vMAXPCSNO As String = 0


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1490-" Then '---- LAKSHMI  SARASWATHI EXPORTS (THIRUCHENCODE)

            If Val(txt_PcsNoFrom.Text) = 0 Then

                vMAXPCSNO = 0
                Da1 = New SqlClient.SqlDataAdapter(" Select max(pcs_tono) from Weaver_Cloth_Receipt_Head Where COMPANY_IDNO = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '%/" & Trim(EntFnYrCode) & "'", con)
                Dt2 = New DataTable
                Da1.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
                        vMAXPCSNO = Val(Dt2.Rows(0)(0).ToString)
                    End If
                End If
                Dt2.Clear()

                vMAXPCSNO = Val(vMAXPCSNO) + 1
                txt_PcsNoFrom.Text = vMAXPCSNO

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1490" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            If Val(txt_PcsNoFrom.Text) = 0 Then
                txt_PcsNoFrom.Text = 1
            End If

        End If

    End Sub

    Private Sub PieceNo_From_Calculation()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vRolNo As String = ""
        Dim vPcNo As String = "", vPCSUBNO As String = ""

        Exit Sub

        vRolNo = ""

        Da = New SqlClient.SqlDataAdapter("select max(PieceNo_OrderBy) from Weaver_ClothReceipt_Piece_Details Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "'", con)
        'Da = New SqlClient.SqlDataAdapter("select max(PieceNo_OrderBy) from Weaver_ClothReceipt_Piece_Details Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by Weaver_ClothReceipt_date DESC, PieceNo_OrderBy DESC, Piece_No DESC", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                vRolNo = Val(Dt1.Rows(0)(0).ToString)
            End If
            'vPcNo = Dt1.Rows(0).Item("Piece_No").ToString
            'vRolNo = Val(vPcNo)
        End If
        Dt1.Clear()

        vRolNo = Trim(Val(vRolNo) + 1)

        txt_PcsNoFrom.Text = Trim(vRolNo)

    End Sub

    Private Sub PieceNo_Generation_RowWise(ByVal RWindex As Integer)
        Dim n As Integer = 0
        Dim Nextvalue As Integer = 0
        Dim vPCSNO As String = ""
        Dim Led_ID As Integer = 0
        Dim vLEDShtNm As String = ""
        Dim PcsFrmNo As String = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1490" Then

            With dgv_Details

                PcsFrmNo = Trim(Val(txt_PcsNoFrom.Text))
                If Val(PcsFrmNo) = 0 Then PcsFrmNo = 1

                If RWindex = 0 Then
                    .Rows(RWindex).Cells(0).Value = Trim(PcsFrmNo)

                Else

                    If Val(.CurrentRow.Cells(0).Value) = 0 Then
                        .Rows(RWindex).Cells(0).Value = Val(.Rows(RWindex - 1).Cells(0).Value) + 1
                    End If

                End If

            End With

        End If


    End Sub

    Private Sub cbo_Weaver_GotFocus(sender As Object, e As EventArgs) Handles cbo_Weaver.GotFocus
        cbo_Weaver.Tag = cbo_Weaver.Text
    End Sub

    Private Sub txt_Remarks_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Remarks.KeyDown

        If e.KeyCode = 38 Then
            txt_Rate.Focus()
        End If

        If e.KeyCode = 40 Then
            If cbo_Godown_StockIN.Visible Then
                cbo_Godown_StockIN.Focus()
            ElseIf txt_Rate.Visible Then
                txt_Rate.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
                End If
            End If


        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Remarks.KeyPress

        If Asc(e.KeyChar) = 13 Then

            'If cbo_Godown_StockIN.Visible Then
            '    cbo_Godown_StockIN.Focus()
            'ElseIf txt_Rate.Visible Then
            '    txt_Rate.Focus()
            'Else

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
            End If

            'End If

        End If

    End Sub

    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click
        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()

        Grp_EWB.Left = (Me.Width - pnl_Back.Width) / 2 + 250
        Grp_EWB.Top = (Me.Height - pnl_Back.Height) / 2 + 200

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
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        '   Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Val(lbl_Amount.Text) = 0 Then
            MessageBox.Show("Invalid Amount", "DOES NOT GENERATE EWB...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Rate.Enabled And txt_Rate.Visible Then txt_Rate.Focus()
            Exit Sub
        End If

        Dim da As New SqlClient.SqlDataAdapter("Select Eway_BillNo from Weaver_Cloth_Receipt_Head where Weaver_ClothReceipt_Code = '" & NewCode & "'", con)
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


        ''--------------------**************
        'MessageBox.Show("Count not generate EWB", "ERROR", MessageBoxButtons.OKCancel)
        'dt.Clear()
        'Exit Sub
        ''*******************

        Dim vRecNo As String = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
            vRecNo = " a.Party_DcNo "
        Else
            vRecNo = " a.Weaver_ClothReceipt_No "
        End If


        CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
                         "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
                         "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
                         "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
                         "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
                         " " &
                         " " &
                         "  SELECT               'I'              , '6'             ,   'JOB WORK RETURNS'        ,    'CHL'    , " & vRecNo & " , a.Weaver_ClothReceipt_date          , L.Ledger_GSTINNo, L.Ledger_MainName   , L.Ledger_Address1 +  L.Ledger_Address2 , L.Ledger_Address3 + L.Ledger_Address4 , L.City_Town ," &
                         " L.PinCode     , FS.State_Code  ,FS.State_Code    , C.Company_GSTINNo, C.Company_Name , ( c.Company_Address1 + c.Company_Address2 ) as deliveryaddress1,  ( c.Company_Address3 + c.Company_Address4 ) as deliveryaddress2, ( c.Company_City ) as city_town_name, ( c.Company_PinCode ) as pincodee, TS.State_Code, ( TDCS.State_Code ) as actual_StateCode," &
                         " 1                     , 0 , a.Amount     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
                         " a.Vehicle_No, 'R', '" & NewCode & "', c.Company_GSTinNo as ShippedTo_GSTIN, c.Company_Name as ShippedTo_LedgerName from Weaver_Cloth_Receipt_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L ON a.Ledger_IdNo = L.Ledger_IdNo  left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on c.Company_State_Idno = TDCS.State_IdNo " &
                         " where a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
        CMD.ExecuteNonQuery()


        'CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
        '                 "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
        '                 "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
        '                 "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
        '                 "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
        '                 " " &
        '                 " " &
        '                 "  SELECT               'I'              , '6'             ,   'JOB WORK RETURNS'        ,    'OTH'    , a.Weaver_ClothReceipt_No ,a.Weaver_ClothReceipt_date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
        '                 " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo, L.Ledger_MainName, ( c.Company_Address1 + c.Company_Address2 ) as deliveryaddress1,  ( c.Company_Address3 + c.Company_Address4 ) as deliveryaddress2, ( c.Company_City ) as city_town_name, ( c.Company_PinCode ) as pincodee, TS.State_Code, ( TDCS.State_Code ) as actual_StateCode," &
        '                 " 1                     , 0 , a.Amount     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
        '                 " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
        '                 " a.Vehicle_No, 'R', '" & NewCode & "', c.Company_GSTinNo as ShippedTo_GSTIN, c.Company_Name as ShippedTo_LedgerName from Weaver_Cloth_Receipt_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
        '                 " Inner Join Ledger_Head L ON a.Ledger_IdNo = L.Ledger_IdNo  left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
        '                 " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on c.Company_State_Idno = TDCS.State_IdNo " &
        '                 " where a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
        'CMD.ExecuteNonQuery()


        'CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
        '                 "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
        '                 "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
        '                 "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
        '                 "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
        '                 " " &
        '                 " " &
        '                 "  SELECT               'O'              , '6'             ,   'JOB WORK RETURNS'        ,    'CHL'    , a.Weaver_ClothReceipt_No ,a.Weaver_ClothReceipt_date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
        '                 " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo, L.Ledger_MainName, (case when a.Delivery_Idno <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.Delivery_Idno <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.Delivery_Idno <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.Delivery_Idno <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee, TS.State_Code, (case when a.Delivery_Idno <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode," &
        '                 " 1                     , 0 , a.Amount     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
        '                 " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
        '                 " a.Vehicle_No, 'R', '" & NewCode & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from Weaver_Cloth_Receipt_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
        '                 " Inner Join Ledger_Head L ON a.Ledger_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.Delivery_Idno <> 0 and a.Delivery_Idno = tDELV.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
        '                 " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo " &
        '                 " where a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
        'CMD.ExecuteNonQuery()

        'vSgst = 
        Dim vTaxPerc As String = 0

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then 'BannariAmman
        '    CMD.CommandText = " Update EWB_Head Set CGST_Value =  -1  where InvCode = '" & Trim(NewCode) & "' "
        '    CMD.ExecuteNonQuery()

        'Else

        'CMD.CommandText = " Update EWB_Head Set CGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , SGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        '    CMD.ExecuteNonQuery()


        '    CMD.CommandText = " Update EWB_Head Set TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        '    CMD.ExecuteNonQuery()

        ' End If
        Dim vCgst_Amt As String = 0
        Dim vSgst_Amt As String = 0
        Dim vIgst_AMt As String = 0

        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        Dim dt1 As New DataTable

        da = New SqlClient.SqlDataAdapter(" Select  I.Cloth_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,( Case When SD.GST_Tax_Invoice_Status =1 Then IG.Item_GST_Percentage else 0 end ), sum(SD.Amount) As TaxableAmt, sum(SD.ReceiptMeters_Receipt) as Qty, 1 , 'MTR' AS Units , tz.Company_State_IdNo , Lh.Ledger_State_Idno " &
                                          " from Weaver_Cloth_Receipt_Head SD Inner Join Cloth_Head I On SD.Cloth_IdNo = I.Cloth_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
                                          " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = Sd.Ledger_IdNo INNER JOIN Company_Head tz On tz.Company_Idno = Sd.Company_Idno Where SD.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' " &
                                          " Group By I.Cloth_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage,Lh.Ledger_Type ,Lh.Ledger_GSTINNo, tz.Company_State_IdNo , Lh.Ledger_State_Idno ,SD.GST_Tax_Invoice_Status ", con)
        dt1 = New DataTable
        da.Fill(dt1)

        'da = New SqlClient.SqlDataAdapter(" Select  I.Cloth_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.Item_GST_Percentage,sum(SD.Taxable_Value) As TaxableAmt,sum(SD.Meters) as Qty,Min(Sl_No), 'MTR' AS Units " &
        '                                  " from ClothSales_Invoice_Details SD Inner Join Cloth_Head I On SD.Cloth_IdNo = I.Cloth_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
        '                                  " Where SD.ClothSales_Invoice_Code = '" & Trim(NewCode) & "' Group By " &
        '                                  " I.Cloth_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage", con)
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

                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then 'BannariAmman
                '    vTaxPerc = -1
                'Else
                vTaxPerc = "" & dt1.Rows(I).Item(3).ToString & ""
                ' End If

                CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                                ,[QuantityUnit] ,   Tax_Perc        ,	   [CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               ,         InvCode     ,                    Cgst_Value      ,                    Sgst_Value        ,                   Igst_Value  ) " &
                                  " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",'MTR'          ,  '" & vTaxPerc & "',        0                  , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "' ,    '" & Str(Val(vCgst_Amt)) & "'      ,      '" & Str(Val(vSgst_Amt)) & "'    ,   '" & Str(Val(vIgst_AMt)) & "' )"

                CMD.ExecuteNonQuery()

            Next

        End If



        '-------

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

        '--------


        btn_GENERATEEWB.Enabled = False

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Weaver_Cloth_Receipt_Head", "Eway_BillNo", "Weaver_ClothReceipt_Code", Pk_Condition)



    End Sub

    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWayBillNo.Text, rtbEWBResponse, 0)
    End Sub

    Private Sub btn_CancelEWB_1_Click(sender As Object, e As EventArgs) Handles btn_CancelEWB_1.Click
        'Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_EWayBillNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Weaver_Cloth_Receipt_Head", "Eway_BillNo", "Weaver_ClothReceipt_Code")

    End Sub


    Private Sub btn_Close_EWB_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub

    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        txt_EWayBillNo.Text = txt_EWBNo.Text
    End Sub

    Private Sub txt_EWayBillNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_EWayBillNo.KeyDown
        If e.KeyValue = 38 Then
            cbo_VehicleNo.Focus()

        End If
        If e.KeyValue = 40 Then
            If cbo_Godown_StockIN.Visible And cbo_Godown_StockIN.Enabled Then
                cbo_Godown_StockIN.Focus()
            Else
                txt_Rate.Focus()
            End If
        End If
    End Sub

    Private Sub txt_EWayBillNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_EWayBillNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_Godown_StockIN.Visible And cbo_Godown_StockIN.Enabled Then
                cbo_Godown_StockIN.Focus()
            Else
                txt_Rate.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Bobin_Click(sender As Object, e As EventArgs) Handles btn_Bobin.Click
        pnl_Back.Enabled = False
        pnl_Bobin.Visible = True
    End Sub

    Private Sub btn_Close_Bobin_Click(sender As Object, e As EventArgs) Handles btn_Close_Bobin.Click

        pnl_Back.Enabled = True
        pnl_Bobin.Visible = False
        txt_Rate.Focus()

    End Sub

    Private Sub txt_Rate_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Rate.KeyDown
        If e.KeyCode = 38 Then
            If cbo_Delivery_Purpose.Visible Then
                cbo_Delivery_Purpose.Focus()
            ElseIf cbo_Godown_StockIN.Visible Then
                cbo_Godown_StockIN.Focus()
            ElseIf txt_EWayBillNo.Visible Then
                txt_EWayBillNo.Focus()
            Else
                cbo_VehicleNo.Focus()
            End If
        End If

        If e.KeyCode = 40 Then
            txt_Remarks.Focus()
        End If

    End Sub

    Private Sub txt_Rate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Rate.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            txt_Remarks.Focus()
        End If

    End Sub


    Private Sub Amount_Calculation()
        lbl_Amount.Text = Format(Val(txt_ReceiptMeters.Text) * Val(txt_Rate.Text), "############0.00")
    End Sub

    Private Sub txt_Rate_TextChanged(sender As Object, e As EventArgs) Handles txt_Rate.TextChanged
        Amount_Calculation()
    End Sub

    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWayBillNo.Text, rtbEWBResponse, 1)
    End Sub

    Private Function GetNewNo(Optional TRANS As SqlClient.SqlTransaction = Nothing) As String

        Dim New_No1 As Integer = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, TRANS)
        Dim New_No2 As Integer = Common_Procedures.get_MaxCode(con, "Cloth_Purchase_Receipt_Head", "Cloth_Purchase_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, TRANS)

        If New_No1 <> 0 Or New_No2 <> 0 Then
            If New_No1 > New_No2 Then
                Return New_No1.ToString
            Else
                Return New_No2.ToString
            End If
        Else
            Return ("1")
        End If

    End Function


    Private Sub cbo_Delivery_Purpose_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Delivery_Purpose.KeyDown

        If Common_Procedures.settings.CustomerCode = "1516" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delivery_Purpose, cbo_Godown_StockIN, cbo_Processed_Cloth, "Process_Head", "Process_Name", "Cloth_Delivered=1 ", "(Process_name='')")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delivery_Purpose, cbo_Godown_StockIN, cbo_Processed_Cloth, "Process_Head", "Process_Name", "  ", "(Process_name='')")
        End If

    End Sub

    Private Sub cbo_Delivery_Purpose_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Delivery_Purpose.KeyPress

        If Common_Procedures.settings.CustomerCode = "1516" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delivery_Purpose, cbo_Processed_Cloth, "Process_Head", "Process_Name", "Cloth_Delivered=1", "(Process_name='')")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delivery_Purpose, cbo_Processed_Cloth, "Process_Head", "Process_Name", "  ", "(Process_name='')")
        End If

    End Sub

    Private Sub cbo_Delivery_Purpose_Enter(sender As Object, e As EventArgs) Handles cbo_Delivery_Purpose.Enter

        cbo_Delivery_Purpose.Tag = cbo_Delivery_Purpose.Text

        If Common_Procedures.settings.CustomerCode = "1516" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "Cloth_Delivered=1", "(Process_Idno=0)")
        End If

    End Sub

    Private Sub cbo_Delivery_Purpose_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Delivery_Purpose.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Delivery_Purpose.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub


    Private Sub Enable_Disable_Delivery_Purpose()

        If Len(Trim(cbo_Godown_StockIN.Text)) = 0 Then

            cbo_Delivery_Purpose.Text = ""
            cbo_Delivery_Purpose.Enabled = False
            cbo_Processed_Cloth.Text = ""
            cbo_Processed_Cloth.Enabled = False

        Else

            Dim Led_Type As String = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Godown_StockIN.Text))

            If Led_Type = "GODOWN" Then
                cbo_Delivery_Purpose.Text = ""
                cbo_Delivery_Purpose.Enabled = False
                cbo_Processed_Cloth.Text = ""
                cbo_Processed_Cloth.Enabled = False
            Else
                cbo_Delivery_Purpose.Enabled = True
                cbo_Processed_Cloth.Enabled = True
            End If

        End If

    End Sub

    Private Sub cbo_Godown_StockIN_Leave(sender As Object, e As EventArgs) Handles cbo_Godown_StockIN.Leave
        If cbo_Godown_StockIN.Tag <> cbo_Godown_StockIN.Text Then
            Enable_Disable_Delivery_Purpose()
            cbo_Godown_StockIN.Tag = cbo_Godown_StockIN.Text
        End If
    End Sub

    Private Sub cbo_Cloth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Cloth.SelectedIndexChanged

    End Sub




    Private Sub cbo_Processed_Cloth_Enter(sender As Object, e As EventArgs) Handles cbo_Processed_Cloth.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "cloth_name", " (Cloth_Type = 'PROCESSED FABRIC' AND Close_status = 0 )", "(cloth_name='')")
    End Sub

    Private Sub cbo_Processed_Cloth_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Processed_Cloth.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Processed_Cloth, cbo_Godown_StockIN, Nothing, "Cloth_Head", "cloth_name", " (Cloth_Type = 'PROCESSED FABRIC' AND Close_status = 0 )", "(cloth_name='')")

        If (e.KeyValue = 40 And cbo_Godown_StockIN.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If (Common_Procedures.settings.CustomerCode = "1267") Then
                chk_UNLOADEDBYOUREMPLOYEE.Focus()
            ElseIf txt_Rate.Visible Then
                txt_Rate.Focus()

            ElseIf MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
            End If

        End If

    End Sub

    Private Sub cbo_Processed_Cloth_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Processed_Cloth.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Processed_Cloth, Nothing, "Cloth_Head", "cloth_name", " (Cloth_Type = 'PROCESSED FABRIC' AND Close_status = 0 )", "(cloth_name='')")

        If Asc(e.KeyChar) = 13 Then
            If (Common_Procedures.settings.CustomerCode = "1267") Then
                chk_UNLOADEDBYOUREMPLOYEE.Focus()
            ElseIf txt_Rate.Visible Then
                txt_Rate.Focus()
            ElseIf MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Grid_BeamNo1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BeamNo1.GotFocus
        Dim vSQL_CONDT As String = ""

        vSQL_CONDT = get_sql_condition_for_BeamNos()

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", vSQL_CONDT, "(BeamNo_SetCode_forSelection = '')")
    End Sub

    Private Sub cbo_Grid_BeamNo1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BeamNo1.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "", "", "", "")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")

        With dgv_Details

            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                If .Columns(4).Visible = True Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(4)
                Else
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(7)
                End If
            End If


        End With

    End Sub

    Private Sub cbo_Grid_BeamNo1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_BeamNo1.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        'Dim Cn_bag As Integer
        'Dim Wgt_Bag As Integer
        'Dim Wgt_Cn As Integer
        'Dim mill_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "", "", "", "", True)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BeamNo1.Text)
                If .Columns(4).Visible = True Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(4)
                Else
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(7)
                End If

            End With

        End If

    End Sub


    Private Sub cbo_Grid_BeamNo1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BeamNo1.TextChanged
        Try
            If cbo_Grid_BeamNo1.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_BeamNo1.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BeamNo1.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_Grid_BeamNo2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BeamNo2.GotFocus
        Dim vSQL_CONDT As String = ""

        vSQL_CONDT = get_sql_condition_for_BeamNos()

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", vSQL_CONDT, "(BeamNo_SetCode_forSelection = '')")

    End Sub

    Private Sub cbo_Grid_BeamNo2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BeamNo2.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "", "", "", "")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")

        With dgv_Details

            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(7)

            End If


        End With

    End Sub

    Private Sub cbo_Grid_BeamNo2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_BeamNo2.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "", "", "", "", True)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BeamNo2.Text)

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(7)

            End With

        End If

    End Sub


    Private Sub cbo_Grid_BeamNo2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BeamNo2.TextChanged
        Try
            If cbo_Grid_BeamNo2.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_BeamNo2.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BeamNo2.Text)
                    End If
                End With
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Function get_sql_condition_for_BeamNos() As String
        Dim vSQL_CONDT As String = ""
        Dim ENDSCNT_ID As Integer
        Dim Wea_ID As Integer
        Dim vLMNO As String = ""

        Wea_ID = 0
        If Trim(cbo_Weaver.Text) <> "" Then
            Wea_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        End If
        ENDSCNT_ID = 0
        If Trim(cbo_EndsCount.Text) <> "" Then
            ENDSCNT_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        End If
        vLMNO = ""
        If Not IsNothing(dgv_Details.CurrentCell) Then
            vLMNO = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(2).Value
        End If

        vSQL_CONDT = "(StockAt_IdNo = -99988)"
        If Wea_ID <> 0 And ENDSCNT_ID <> 0 Then
            vSQL_CONDT = "(StockAt_IdNo = " & Str(Val(Wea_ID)) & " and EndsCount_IdNo = " & Str(Val(ENDSCNT_ID)) & " and Close_Status = 0 )"
        End If

        Return vSQL_CONDT

    End Function

    Private Sub get_Cloth_Meter_per_Piece()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim Clo_IdNo As Integer
        Dim Stock_In As String
        Dim vMTR_PC As String = 0

        vCLO_MTR_PER_PC = 0
        vCLO_MTRPERPC_QUALITY = cbo_Cloth.Text

        If FrmLdSTS = True Then Exit Sub
        If Trim(cbo_Cloth.Text) = "" Then Exit Sub

        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

        If Val(Clo_IdNo) <> 0 Then

            Stock_In = ""
            vMTR_PC = 0
            da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  Cloth_Head Where Cloth_idno = " & Str(Val(Clo_IdNo)), con)
            dt2 = New DataTable
            da.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                Stock_In = dt2.Rows(0)("Stock_In").ToString
                vMTR_PC = Val(dt2.Rows(0)("Meters_Pcs").ToString)
            End If
            dt2.Clear()

            If Trim(UCase(Stock_In)) = "PCS" Then
                vCLO_MTR_PER_PC = Val(vMTR_PC)
            End If

        End If

    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_Details.TextChanged
        Try

            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
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

    Private Function get_SizedPavu_BeamNo_for_Selected_LoomNo(ByVal vROWNO As Integer, ByVal vLOOMNO As String) As String
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Wea_ID As Integer = 0
        Dim NewCode As String = ""
        Dim vSETCD As String, vBMNO As String, vBmNoSetCd_forSelection As String

        vBmNoSetCd_forSelection = ""

        If Trim(cbo_Weaver.Text) = "" Then Exit Function
        If Trim(vLOOMNO) = "" Then Exit Function

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        cmd.Connection = con
        cmd.CommandTimeout = 1000

        Wea_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        If Wea_ID = 0 Then Exit Function

        vBmNoSetCd_forSelection = ""
        For I = vROWNO - 1 To 0 Step -1
            If Trim(UCase(dgv_Details.Rows(I).Cells(2).Value)) = Trim(UCase(vLOOMNO)) And Trim(dgv_Details.Rows(I).Cells(3).Value) <> "" Then
                vBmNoSetCd_forSelection = dgv_Details.Rows(I).Cells(3).Value
                Return vBmNoSetCd_forSelection
                Exit Function
            End If
        Next

        vSETCD = ""
        vBMNO = ""
        Da1 = New SqlClient.SqlDataAdapter("Select top 1 * from Weaver_ClothReceipt_Piece_Details Where Ledger_IdNo = " & Str(Val(Wea_ID)) & " and Loom_No = '" & Trim(vLOOMNO) & "' Order by Weaver_ClothReceipt_date desc, for_OrderBy desc, Weaver_ClothReceipt_Code desc, PieceNo_OrderBy desc, Piece_No desc", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            vSETCD = Dt1.Rows(0).Item("Set_Code1").ToString
            vBMNO = Dt1.Rows(0).Item("Beam_No1").ToString
        End If
        Dt1.Clear()

        If Trim(vSETCD) = "" And Trim(vBMNO) = "" Then Exit Function

        vBmNoSetCd_forSelection = ""
        Da1 = New SqlClient.SqlDataAdapter("Select BeamNo_SetCode_forSelection from Stock_SizedPavu_Processing_Details Where set_code = '" & Trim(vSETCD) & "' and beam_no = '" & Trim(vBMNO) & "'", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            vBmNoSetCd_forSelection = Dt1.Rows(0).Item("BeamNo_SetCode_forSelection").ToString
        End If
        Dt1.Clear()

        Return vBmNoSetCd_forSelection

    End Function

    Private Sub get_SizedPavu_TotalMeter_BalanceMeter(ByVal vROWNO As Integer, ByVal vBmNoSetCd_forSelection As String, ByRef vBEAM_TOTALMTR As String, ByRef vBEAM_BALMTR As String)
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Lm_ID As Integer = 0
        Dim NewCode As String = ""
        Dim vENTPRODMtrs As String
        Dim vBEAM_PRODMTR As String
        Dim vSETCD As String, vBMNO As String

        If Trim(vBmNoSetCd_forSelection) = "" Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        cmd.Connection = con
        cmd.CommandTimeout = 1000


        vSETCD = ""
        vBMNO = ""
        vBEAM_TOTALMTR = 0
        vBEAM_BALMTR = 0
        vBEAM_PRODMTR = 0
        Da1 = New SqlClient.SqlDataAdapter("Select set_code, beam_no, Meters from Stock_SizedPavu_Processing_Details Where BeamNo_SetCode_forSelection = '" & Trim(vBmNoSetCd_forSelection) & "'", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            vSETCD = Dt1.Rows(0).Item("set_code").ToString
            vBMNO = Dt1.Rows(0).Item("beam_no").ToString
            vBEAM_TOTALMTR = Format(Val(Dt1.Rows(0).Item("Meters").ToString), "#########0.00")
        End If
        Dt1.Clear()

        If Trim(vSETCD) <> "" And Trim(vBMNO) <> "" Then

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSimpleTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSimpleTable) & "(Meters1) Select sum(a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) from Weaver_ClothReceipt_Piece_Details a Where a.Lot_Code <> '" & Trim(NewCode) & "' and a.Set_Code1 = '" & Trim(vSETCD) & "' and a.Beam_No1 = '" & Trim(vBMNO) & "' and (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) <> 0 "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSimpleTable) & "(Meters1) Select sum(a.Receipt_Meters) from Weaver_ClothReceipt_Piece_Details a Where a.Lot_Code <> '" & Trim(NewCode) & "' and a.Set_Code1 = '" & Trim(vSETCD) & "' and a.Beam_No1 = '" & Trim(vBMNO) & "' and (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) = 0 "
            cmd.ExecuteNonQuery()

            vBEAM_PRODMTR = 0
            Da1 = New SqlClient.SqlDataAdapter("Select SUM(Meters1) as prodmeters  from " & Trim(Common_Procedures.EntryTempSimpleTable), con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                vBEAM_PRODMTR = Dt1.Rows(0).Item("prodmeters").ToString
            End If
            Dt1.Clear()

        End If

        vENTPRODMtrs = 0
        For I = 0 To dgv_Details.Rows.Count - 1
            If I <> vROWNO Then
                If Trim(UCase(dgv_Details.Rows(I).Cells(3).Value)) = Trim(UCase(vBmNoSetCd_forSelection)) Or Trim(UCase(dgv_Details.Rows(I).Cells(4).Value)) = Trim(UCase(vBmNoSetCd_forSelection)) Then
                    vENTPRODMtrs = Val(vENTPRODMtrs) + Val(dgv_Details.Rows(I).Cells(1).Value)
                End If
            End If
        Next

        vBEAM_BALMTR = Format(Val(vBEAM_TOTALMTR) - Val(vBEAM_PRODMTR) - Val(vENTPRODMtrs), "#########0.00")

    End Sub

    Private Sub btn_Add_PartyDc_Image_Click(sender As Object, e As EventArgs) Handles btn_Add_PartyDc_Image.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pic_PartyDc_Image.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub btn_Delete_PartyDc_Image_Click(sender As Object, e As EventArgs) Handles btn_Delete_PartyDc_Image.Click
        pic_PartyDc_Image.BackgroundImage = Nothing
    End Sub

    Private Sub btn_Enlarge_PartyDc_Image_Click(sender As Object, e As EventArgs) Handles btn_Enlarge_PartyDc_Image.Click
        Dim f As New Enlarge_Image(pic_PartyDc_Image.BackgroundImage)
        f.MdiParent = MDIParent1
        f.Show()
    End Sub

    Private Sub btn_Show_PartyDc_Image_Click(sender As Object, e As EventArgs) Handles btn_Show_PartyDc_Image.Click
        pnl_Attachments.Visible = True

        'pnl_PartyDc_Image.Visible = True
        pnl_Back.Enabled = False
        'btn_Add_PartyDc_Image.Focus()

        If dgv_Attachments.Rows.Count > 0 Then
            dgv_Attachments.Focus()
            dgv_Attachments.CurrentCell = dgv_Attachments.Rows(0).Cells(1)
            dgv_Attachments.Select()

        Else
            btn_close_Attachments.Focus()

        End If

    End Sub

    Private Sub btn_Close_PartyDc_Image_Click(sender As Object, e As EventArgs) Handles btn_Close_PartyDc_Image.Click
        pnl_Back.Enabled = True
        pnl_PartyDc_Image.Visible = False
    End Sub

    Private Sub btn_AddNew_Attachments_Click(sender As Object, e As EventArgs) Handles btn_AddNew_Attachment.Click
        UpLoad_File()
    End Sub

    Private Sub btn_close_Attachments_Click(sender As Object, e As EventArgs) Handles btn_close_Attachments.Click
        pnl_Back.Enabled = True
        pnl_Attachments.Visible = False
    End Sub

    Private Sub btn_close_Attachments2_Click(sender As Object, e As EventArgs) Handles btn_close_Attachments2.Click
        btn_close_Attachments_Click(sender, e)
    End Sub


    Private Sub dgv_Attachments_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Attachments.KeyUp
        Dim i As Integer = 0
        Dim n As Integer = 0
        Dim nrw As Integer = 0

        With dgv_Attachments

            If e.Control = True And (UCase(Chr(e.KeyCode)) = "A" Or UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then

                n = .CurrentRow.Index

                nrw = n + 1

                .Rows.Insert(nrw, 1)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                    dgv_Attachments.Rows(i).Cells(2).Value = "DOWNLOAD"
                    dgv_Attachments.Rows(i).Cells(3).Value = "DELETE"
                Next

            End If

            If e.Control = True And (UCase(Chr(e.KeyCode)) = "D" Or e.KeyCode = Keys.Delete) Then
                dgvgrid_RowDelete(sender)
            End If

        End With

    End Sub

    Private Sub dgv_Attachments_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Attachments.CellContentClick
        Try

            With dgv_Attachments

                If .Visible Then

                    If .Rows.Count > 0 Then

                        If e.ColumnIndex = 1 Then
                            If Trim(.Rows(e.RowIndex).Cells(1).Value) <> "" Then
                                OpenAttachment_File()
                            End If
                        End If

                        If e.ColumnIndex = 2 Then
                            DownLoad_File()
                        End If
                        If e.ColumnIndex = 3 Then
                            dgvgrid_RowDelete(sender)
                        End If


                    End If


                End If

            End With

        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub dgvgrid_RowDelete(ByVal sender As Object)
        Dim i As Integer = 0
        Dim n As Integer = 0
        Dim nrw As Integer = 0

        With sender

            If .Rows.Count > 0 Then

                n = .CurrentRow.Index
                'If .Rows.Count = 1 And .CurrentCell.RowIndex = .Rows.Count - 1 Then
                '    .Rows(n).Cells(1).Value = ""
                'Else
                .Rows.RemoveAt(n)
                'End If

                For i = 0 To dgv_Attachments.Rows.Count - 1
                    dgv_Attachments.Rows(i).Cells(0).Value = i + 1
                Next

            End If

        End With

    End Sub

    Private Sub UpLoad_File()

        Using OpenFlDia As New OpenFileDialog

            OpenFlDia.CheckFileExists = True
            OpenFlDia.CheckPathExists = True
            OpenFlDia.Filter = "All Files | *.*"
            OpenFlDia.Title = "Select a File"
            OpenFlDia.Multiselect = False

            If OpenFlDia.ShowDialog = Windows.Forms.DialogResult.OK Then

                Dim n As Integer
                n = dgv_Attachments.Rows.Add()
                dgv_Attachments.Rows(n).Cells(0).Value = n + 1
                dgv_Attachments.Rows(n).Cells(2).Value = "DOWNLOAD"
                dgv_Attachments.Rows(n).Cells(3).Value = "DELETE"
                dgv_Attachments.CurrentCell = dgv_Attachments.Rows(n).Cells(1)
                dgv_Attachments.Select()

                Dim fileinfo As New FileInfo(OpenFlDia.FileName)
                Dim binarydata As Byte() = File.ReadAllBytes(OpenFlDia.FileName)

                dgv_Attachments.Rows(n).Cells(1).Value = fileinfo.Name
                If vDIC_ATTACHMENTS.ContainsKey(n) Then
                    vDIC_ATTACHMENTS(n) = binarydata
                Else
                    vDIC_ATTACHMENTS.Add(n, binarydata)
                End If

            End If

        End Using

    End Sub

    Private Sub DownLoad_File()
        Dim vFILENAME As String = dgv_Attachments.Rows(dgv_Attachments.CurrentCell.RowIndex).Cells(1).Value

        If vFILENAME = String.Empty Then
            Return
        End If

        Dim fileinfo As New FileInfo(vFILENAME)
        Dim fileextn As String = fileinfo.Extension
        Dim binarydata As Byte() = Nothing
        Using SaveFlDia As New SaveFileDialog()

            SaveFlDia.Filter = Convert.ToString((Convert.ToString("Files (") & fileextn) & ")|") & fileextn
            SaveFlDia.Title = "Save File as"
            SaveFlDia.CheckPathExists = True
            SaveFlDia.FileName = vFILENAME
            If SaveFlDia.ShowDialog() = Windows.Forms.DialogResult.OK Then
                binarydata = vDIC_ATTACHMENTS(dgv_Attachments.CurrentCell.RowIndex)
                File.WriteAllBytes(SaveFlDia.FileName, binarydata)
            End If
        End Using

    End Sub


    Private Sub OpenAttachment_File()
        Dim vFILENAME As String = dgv_Attachments.Rows(dgv_Attachments.CurrentCell.RowIndex).Cells(1).Value

        If vFILENAME = String.Empty Then
            Return
        End If

        'Common_Procedures.AppPath = Application.StartupPath
        Dim vFldrName As String = Application.StartupPath & "\Attachments"
        If System.IO.Directory.Exists(vFldrName) = False Then
            System.IO.Directory.CreateDirectory(vFldrName)
        End If

        Dim fileinfo As New FileInfo(vFILENAME)
        Dim fileextn As String = fileinfo.Extension
        Dim binarydata As Byte() = Nothing

        If System.IO.Directory.Exists(vFldrName) = True Then
            binarydata = vDIC_ATTACHMENTS(dgv_Attachments.CurrentCell.RowIndex)
            If Not binarydata Is Nothing Then

                Dim sTempFileName As String = vFldrName & "\" & vFILENAME

                File.WriteAllBytes(sTempFileName, binarydata)
                ShellEx(Me.Handle, "Open", sTempFileName, "", "", 10)
            End If
        End If


    End Sub

    Private Sub cbo_Sales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_Sales_OrderCode_forSelection.Enter
        Dim vTEX_CLOIDNO As Integer = 0
        Dim vCLO_CONDT As String = ""
        Dim vCONDT As String = ""
        Dim FnYearCode1 As String = ""
        Dim FnYearCode2 As String = ""

        FnYearCode1 = ""
        FnYearCode2 = ""
        Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)

        vCLO_CONDT = ""
        vTEX_CLOIDNO = 0
        If cbo_Cloth.Visible = True Then
            If Trim(cbo_Cloth.Text) <> "" Then
                vTEX_CLOIDNO = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
                If New_Entry = True Then
                    vCLO_CONDT = " ( ClothSales_Order_Code IN (select sq1.ClothSales_Order_Code from ClothSales_Order_Details sq1 , ClothSales_Order_Head sq2 where sq1.ClothSales_Order_Code = sq2.ClothSales_Order_Code and sq1.Cloth_IdNo = " & Str(Val(vTEX_CLOIDNO)) & " And  sq2.Order_Close_Status = 0 And sq2.Ready_Stock_Available_Status = 0  and (sq1.Order_Meters - sq1.Order_Cancel_Meters - sq1.Delivery_Meters - sq1.Invoice_Meters) > 1 )) "
                    'vCLO_CONDT = "(ClothSales_Order_Code IN (select sq1.ClothSales_Order_Code from ClothSales_Order_Details sq1 where sq1.Cloth_IdNo = " & Str(Val(vTEX_CLOIDNO)) & ") And Order_Close_Status = 0 )"
                Else
                    vCLO_CONDT = "(ClothSales_Order_Code In (Select sq1.ClothSales_Order_Code from ClothSales_Order_Details sq1 where sq1.Cloth_IdNo = " & Str(Val(vTEX_CLOIDNO)) & ") )"
                End If

            Else

                If New_Entry = True Then
                    vCLO_CONDT = " ( ClothSales_Order_Code IN (select sq1.ClothSales_Order_Code from ClothSales_Order_Details sq1 , ClothSales_Order_Head sq2 where sq1.ClothSales_Order_Code = sq2.ClothSales_Order_Code  And  sq2.Order_Close_Status = 0 And sq2.Ready_Stock_Available_Status = 0  and (sq1.Order_Meters - sq1.Order_Cancel_Meters - sq1.Delivery_Meters - sq1.Invoice_Meters) > 1 )) "

                    'vCLO_CONDT = "(Order_Close_Status = 0 And sq2.Ready_Stock_Available_Status = 0 )"
                End If

            End If

        Else
            If New_Entry = True Then
                vCLO_CONDT = " ( ClothSales_Order_Code IN (select sq1.ClothSales_Order_Code from ClothSales_Order_Details sq1 , ClothSales_Order_Head sq2 where sq1.ClothSales_Order_Code = sq2.ClothSales_Order_Code  And  sq2.Order_Close_Status = 0 And sq2.Ready_Stock_Available_Status = 0 and (sq1.Order_Meters - sq1.Order_Cancel_Meters - sq1.Delivery_Meters - sq1.Invoice_Meters) > 1 )) "

                'vCLO_CONDT = "(Order_Close_Status = 0 And sq2.Ready_Stock_Available_Status = 0 )"
            End If

        End If

        vCONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "')" & IIf(Trim(vCLO_CONDT) <> "", " and ", "") & Trim(vCLO_CONDT)

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCONDT, "(ClothSales_Order_Code = '999999/00-00')")
        cbo_Sales_OrderCode_forSelection.BackColor = Color.Lime
        cbo_Sales_OrderCode_forSelection.ForeColor = Color.Blue

    End Sub

    Private Sub cbo_Sales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Sales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_Weaver, cbo_Cloth, "", "", "", "")
    End Sub

    Private Sub cbo_Sales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Sales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, cbo_Cloth, "", "", "", "", True)
    End Sub

    Private Sub cbo_Sales_OrderCode_forSelection_Leave(sender As Object, e As EventArgs) Handles cbo_Sales_OrderCode_forSelection.Leave
        cbo_Sales_OrderCode_forSelection.BackColor = Color.White
        cbo_Sales_OrderCode_forSelection.ForeColor = Color.Black
    End Sub

     Private Sub txt_Dc_receipt_pcs_TextChanged(sender As Object, e As EventArgs) Handles txt_Dc_receipt_pcs.TextChanged
        PieceNo_To_Calculation()
    End Sub


End Class