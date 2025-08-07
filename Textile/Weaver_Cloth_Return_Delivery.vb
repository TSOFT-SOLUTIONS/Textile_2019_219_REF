Public Class Weaver_Cloth_Return_Delivery
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "WCLRD-"
    Private Pk_Condition2 As String = "WCRKR-"
    Private PkCondition_WFRGT As String = "WRDFR-"
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

    Private Prnt_HalfSheet_STS As Boolean = False
    Private vPrnt_2Copy_In_SinglePage As Integer = 0
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_PageNo1 As Integer
    Private prn_DetIndx1 As Integer
    Private prn_NoofBmDets1 As Integer
    Private prn_DetSNo1 As Integer
    Private prn_NoofBmDets As Integer
    Private prn_TotCopies As Integer = 0
    Private vprn_TotAmt As String = ""

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

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        Dim dttm As Date

        chk_Verified_Status.Checked = False

        New_Entry = False
        Insert_Entry = False
        pnl_Selection.Visible = False
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_DriverDetails.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        chk_Purchase.Checked = False
        chk_NoStockPosting.Checked = False
        chk_No_Weaving_Wages_Bill.Checked = False
        chk_UNLOADEDBYOUREMPLOYEE.Checked = False
        msk_date.Text = ""
        dtp_Date.Text = ""
        lbl_Yarn.Text = ""
        lbl_Pavu.Text = ""
        lbl_EmptyBeam.Text = ""

        If dtp_Date.Enabled = False Then
            dttm = New DateTime(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4), 4, 1)
            dttm = DateAdd(DateInterval.Day, -1, dttm)
            dtp_Date.Text = dttm
        End If

        txt_LotNo.Text = ""
        cbo_Weaver.Text = ""
        cbo_Weaver.Tag = ""
        cbo_Cloth.Text = ""
        txt_PDcNo.Text = ""
        cbo_EndsCount.Text = ""
        lbl_WeftCount.Text = ""
        txt_EBeam.Text = ""
        txt_NoOfPcs.Text = ""
        txt_Quantity.Text = ""
        txt_PcsNoFrom.Text = "1"
        If Trim(Common_Procedures.settings.CustomerCode) = "1204" Then
            cbo_LoomType.Text = ""
        Else
            cbo_LoomType.Text = "POWER LOOM"
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


        lbl_LotNo.Text = Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1146" Then '----ASMITHA TEXTILES
            txt_Folding_Perc.Text = ""
        Else
            txt_Folding_Perc.Text = "100"
        End If

        dgv_Details.Rows.Clear()
        dgv_Details.AllowUserToAddRows = True

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

        txt_NoOfPcs.Enabled = True
        txt_NoOfPcs.BackColor = Color.White

        txt_PcsNoFrom.Enabled = True
        txt_PcsNoFrom.BackColor = Color.White

        txt_ReceiptMeters.Enabled = True
        txt_ReceiptMeters.BackColor = Color.White

        cbo_LoomNo.Enabled = True
        cbo_LoomNo.BackColor = Color.White

        cbo_WidthType.Enabled = True
        cbo_WidthType.BackColor = Color.White
        dgv_BobinDetails.Rows.Clear()
        dgv_KuriDetails.Rows.Clear()

        cbo_DriverName.Text = ""
        cbo_SupervisorName.Text = ""
        cbo_DriverPhNo.Text = ""
        cbo_VehicleNo.Text = ""
        cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.Godown_Ac))

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
        Dim da As New SqlClient.SqlDataAdapter
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n, slno As Integer
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as WeaverName,c.Ledger_Name as Transport_Name, d.Cloth_Name , e.Ledger_Name as StockOff_Name from Weaver_Cloth_Delivery_Return_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo INNER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_IdNo LEFT OUTER JOIN Ledger_Head e ON a.StockOff_IdNo = e.Ledger_IdNo Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "' and a.Receipt_Type = 'W' and a.Weaver_ClothDelivery_Return_Code NOT LIKE 'GWEWA-%'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Weaver_ClothDelivery_Return_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_ClothDelivery_Return_Date")
                msk_date.Text = dtp_Date.Text
                cbo_Weaver.Text = dt1.Rows(0).Item("WeaverName").ToString
                cbo_Cloth.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                cbo_LoomType.Text = dt1.Rows(0).Item("Loom_Type").ToString
                txt_LotNo.Text = dt1.Rows(0).Item("Lot_No").ToString
                txt_PDcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, Val(dt1.Rows(0).Item("EndsCount_IdNo").ToString))
                lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Count_IdNo").ToString))
                txt_EBeam.Text = dt1.Rows(0).Item("empty_beam").ToString
                txt_NoOfPcs.Text = Val(dt1.Rows(0).Item("noof_pcs").ToString)
                txt_PcsNoFrom.Text = dt1.Rows(0).Item("pcs_fromno").ToString
                lbl_PcsNoTo.Text = dt1.Rows(0).Item("pcs_tono").ToString
                txt_ReceiptMeters.Text = dt1.Rows(0).Item("ReceiptMeters_Receipt").ToString
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




                If Val(dt1.Rows(0)("Unloaded_By_Our_Employee").ToString) <> 0 Then
                    chk_UNLOADEDBYOUREMPLOYEE.Checked = True
                End If
                cbo_DriverName.Text = dt1.Rows(0).Item("Driver_Name").ToString
                cbo_DriverPhNo.Text = dt1.Rows(0).Item("Driver_Phone_No").ToString
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_SupervisorName.Text = dt1.Rows(0).Item("Supervisor_Name").ToString
                cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))



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

                lbl_OrderNo.Text = dt1.Rows(0).Item("Our_Order_No").ToString
                lbl_OrderCode.Text = dt1.Rows(0).Item("Own_Order_Code").ToString

                txt_Folding_Perc.Text = dt1.Rows(0).Item("Folding_Receipt").ToString

                'da2 = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and Create_Status = 1 Order by Sl_No, Piece_No", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                'dgv_Details.Rows.Clear()

                'If dt2.Rows.Count > 0 Then

                '    For i = 0 To dt2.Rows.Count - 1

                '        n = dgv_Details.Rows.Add()

                '        dgv_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Piece_No").ToString
                '        dgv_Details.Rows(n).Cells(1).Value = Format(Val(dt2.Rows(i).Item("Receipt_Meters").ToString), "########0.00")

                '    Next i

                'End If
                'dt2.Clear()

                'With dgv_Details_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(0).Value = Val(dt1.Rows(0).Item("Total_Receipt_Pcs").ToString)
                '    .Rows(0).Cells(1).Value = Format(Val(dt1.Rows(0).Item("Total_Receipt_Meters").ToString), "########0.00")
                'End With


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

            Else
                new_record()

            End If

            dt1.Clear()

            If LockSTS = True Then

                cbo_LoomType.Enabled = False
                cbo_LoomType.BackColor = Color.LightGray

                cbo_Weaver.Enabled = False
                cbo_Weaver.BackColor = Color.LightGray

                cbo_Cloth.Enabled = False
                cbo_Cloth.BackColor = Color.LightGray

                txt_LotNo.Enabled = False
                txt_LotNo.BackColor = Color.LightGray

                'cbo_EndsCount.Enabled = False
                cbo_EndsCount.BackColor = Color.LightGray

                txt_NoOfPcs.Enabled = False
                txt_NoOfPcs.BackColor = Color.LightGray

                txt_PcsNoFrom.Enabled = False
                txt_PcsNoFrom.BackColor = Color.LightGray

                txt_ReceiptMeters.Enabled = False
                txt_ReceiptMeters.BackColor = Color.LightGray

                If (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1249" And Common_Procedures.User.IdNo <> 1) Or (Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1204" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1277" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1037" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1249" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1352") Then '----KRG (PALLADAM)

                    cbo_LoomNo.Enabled = False
                    cbo_LoomNo.BackColor = Color.LightGray

                    cbo_WidthType.Enabled = False
                    cbo_WidthType.BackColor = Color.LightGray

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

                Me.Text = lbl_Heading.Text

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
                            Me.Text = lbl_Heading.Text & "   -   " & Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()

                    new_record()

                Else
                    MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    'Me.Close()
                    Exit Sub

                End If

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Weaver_Cloth_Receipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lbl_LotNo.Text = StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase)

        Me.Text = lbl_Heading.Text

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

        txt_NoOfPcs.Width = "211"
        lbl_ClothReceipt_Quantity.Visible = False
        txt_Quantity.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then  '-- Jeno tex or annai tex
            lbl_ClothReceipt_Quantity.Text = "Quantity"
            lbl_ClothReceipt_Quantity.Visible = True
            txt_Quantity.Text = ""
            txt_Quantity.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1204" Then '------KOHINOOR TEXTILE MILLS
            Label42.Visible = True
            Label43.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1146" Then '------ASHMITHA TEXTILE
            lbl_LotNoCaption.Visible = True
            lbl_LotNoCaption.Text = "Folding %"
            txt_Folding_Perc.Visible = True
        End If

        con.Open()


        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("")
        cbo_WidthType.Items.Add("SINGLE")
        cbo_WidthType.Items.Add("DOUBLE")
        cbo_WidthType.Items.Add("TRIPLE")
        cbo_WidthType.Items.Add("FOURTH")


        cbo_LoomType.Items.Clear()
        cbo_LoomType.Items.Add("")
        cbo_LoomType.Items.Add("POWER LOOM")
        cbo_LoomType.Items.Add("AUTO LOOM")

        cbo_Verified_Sts.Items.Clear()
        cbo_Verified_Sts.Items.Add("")
        cbo_Verified_Sts.Items.Add("YES")
        cbo_Verified_Sts.Items.Add("NO")


        dtp_Date.Text = ""
        msk_date.Text = ""
        txt_PDcNo.Text = ""
        txt_LotNo.Text = ""
        cbo_Weaver.Text = ""
        cbo_Weaver.Tag = ""
        cbo_EndsCount.Text = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
            txt_EBeam.Enabled = False
        End If

        lbl_StockOff_Caption.Visible = False
        cbo_StockOff.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then
            cbo_StockOff.Visible = True
            lbl_StockOff_Caption.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
            txt_LotNo.Visible = True
            lbl_LotNoCaption.Visible = True
            cbo_Cloth.Width = 308

        Else
            cbo_Cloth.Width = cbo_Weaver.Width

        End If

        btn_DriverDetails.Visible = False
        chk_NoStockPosting.Visible = False
        chk_No_Weaving_Wages_Bill.Visible = False

        If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then '---- BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION) 
            btn_DriverDetails.Visible = True
            'chk_NoStockPosting.Visible = True
            chk_UNLOADEDBYOUREMPLOYEE.Visible = True

            If Val(EntFnYrCode) <= 18 Then
                chk_No_Weaving_Wages_Bill.Visible = True
                cbo_Cloth.Width = 308
            End If

        End If



        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If
        cbo_Godown_StockIN.Visible = False
        lbl_Godown_Caption.Visible = False
        If Common_Procedures.settings.Multi_Godown_Status = 1 Then
            cbo_Godown_StockIN.Visible = True
            lbl_Godown_Caption.Visible = True

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1019" Then
                lbl_Godown_Caption.Left = lbl_StockOff_Caption.Left
                cbo_Godown_StockIN.Left = cbo_StockOff.Left
                cbo_Godown_StockIN.Width = cbo_StockOff.Width
            End If

        End If

        pnl_DriverDetails.Visible = False
        pnl_DriverDetails.Top = (Me.Height - pnl_DriverDetails.Height) \ 2
        pnl_DriverDetails.Left = (Me.Width - pnl_DriverDetails.Width) \ 2
        pnl_DriverDetails.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

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

        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus



        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
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



        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
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


        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EBeam.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler chk_UNLOADEDBYOUREMPLOYEE.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_RecNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_RecNoTo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfPcs.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_PDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LotNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ReceiptMeters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Quantity.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Txt_NoOfBundles.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EBeam.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler chk_UNLOADEDBYOUREMPLOYEE.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_RecNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_RecNoTo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoOfPcs.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_PDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LotNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ReceiptMeters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Quantity.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Txt_NoOfBundles.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

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
                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub
                ElseIf pnl_DriverDetails.Visible Then
                    btn_Close_DriverDetails_Click(sender, e)
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

        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

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


                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(1).Value) = 0 Then
                                If txt_ReceiptMeters.Enabled And txt_ReceiptMeters.Visible Then
                                    txt_ReceiptMeters.Focus()
                                Else
                                    cbo_Transport.Focus()
                                End If


                            Else
                                '.Rows.Add()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                txt_PcsNoFrom.Focus()

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

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_Cloth_Return_Delivery, New_Entry, Me, con, "Weaver_Cloth_Delivery_Return_Head", "Weaver_Cloth_Receipt_Code", NewCode, "Weaver_Cloth_Receipt_Date", "(Weaver_Cloth_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "Weaver_Cloth_Delivery_Return_Head", "Verified_Status", "(Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_Cloth_Delivery_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "' and  Weaver_Piece_Checking_Code <> ''", con)
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

        Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_Cloth_Delivery_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "' and  (Weaver_Wages_Code <> '' or Weaver_IR_Wages_Code <> '')", con)
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

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Weaver_Cloth_Delivery_Return_Head", "Weaver_ClothDelivery_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Weaver_ClothDelivery_Return_Code, Company_IdNo, for_OrderBy", trans)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WFRGT) & Trim(NewCode), trans)

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

            'cmd.CommandText = "delete from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Cloth_Delivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothDelivery_Return_No from Weaver_Cloth_Delivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code like '%/" & Trim(EntFnYrCode) & "' and Receipt_Type = 'W' and Weaver_ClothDelivery_Return_Code NOT LIKE 'GWEWA-%' Order by for_Orderby, Weaver_ClothDelivery_Return_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothDelivery_Return_No from Weaver_Cloth_Delivery_Return_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code like '%/" & Trim(EntFnYrCode) & "' and  Receipt_Type = 'W'  and Weaver_ClothDelivery_Return_Code NOT LIKE 'GWEWA-%' Order by for_Orderby, Weaver_ClothDelivery_Return_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothDelivery_Return_No from Weaver_Cloth_Delivery_Return_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code like '%/" & Trim(EntFnYrCode) & "' and  Receipt_Type = 'W'  and Weaver_ClothDelivery_Return_Code NOT LIKE 'GWEWA-%' Order by for_Orderby desc, Weaver_ClothDelivery_Return_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothDelivery_Return_No from Weaver_Cloth_Delivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code like '%/" & Trim(EntFnYrCode) & "' and  Receipt_Type = 'W'  and Weaver_ClothDelivery_Return_Code NOT LIKE 'GWEWA-%' Order by for_Orderby desc, Weaver_ClothDelivery_Return_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Delivery_Return_Head", "Weaver_ClothDelivery_Return_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode)

            lbl_RefNo.ForeColor = Color.Red

            If dtp_Date.Enabled = True Then
                msk_date.Text = Date.Today.ToShortDateString
                da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Cloth_Delivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, Weaver_ClothDelivery_Return_No desc", con)
                dt1 = New DataTable
                da.Fill(dt1)
                If dt1.Rows.Count > 0 Then
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1204" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1204" Then
                        If dt1.Rows(0).Item("Loom_Type").ToString <> "" Then cbo_LoomType.Text = dt1.Rows(0).Item("Loom_Type").ToString
                    End If
                    If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                        If dt1.Rows(0).Item("Weaver_ClothDelivery_Return_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Weaver_ClothDelivery_Return_Date").ToString
                    End If
                    If dt1.Rows(0).Item("WareHouse_IdNo").ToString <> "" Then
                        If Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString) <> 0 Then cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))
                    End If
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then
                        If dt1.Rows(0).Item("Cloth_IdNo").ToString <> "" Then cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                    End If
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
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Lot.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_ClothDelivery_Return_No from Weaver_Cloth_Delivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(RecCode) & "'", con)
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
        Dim RecCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Cloth_Return_Delivery, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Lot No.", "FOR NEW REC INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_ClothDelivery_Return_No from Weaver_Cloth_Delivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Clo_ID As Integer = 0
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
        Dim Stock_In As String
        Dim clthStock_In As String
        Dim YrnCons_For As String = ""
        Dim mtrspcs As Single
        Dim clthmtrspcs As Single
        Dim clthPcs_Mtr As Single
        Dim dt2 As New DataTable
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

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Cloth_Return_Delivery, New_Entry, Me, con, "Weaver_Cloth_Delivery_Return_Head", "Weaver_Cloth_Receipt_Code", NewCode, "Weaver_Cloth_Receipt_Date", "(Weaver_Cloth_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Cloth_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Cloth_Receipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "Weaver_Cloth_Delivery_Return_Head", "Verified_Status", "(Weaver_ClothDelivery_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        If Trim(Common_Procedures.settings.CustomerCode) <> "1204" Then
            If Trim(cbo_LoomType.Text) = "" Then
                cbo_LoomType.Text = "POWER LOOM"
            End If
        End If


        If Trim(Common_Procedures.settings.CustomerCode) = "1204" Then
            If Trim(cbo_LoomType.Text) = "" Then
                MessageBox.Show("Invalid Loom Type?", "DOESN'T SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                cbo_LoomType.Focus()
                Exit Sub
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


        vGod_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Godown_StockIN.Text)
        If cbo_Godown_StockIN.Visible = True Then
            If vGod_ID = 0 Then
                MessageBox.Show("Invalid Fabric Godown Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Godown_StockIN.Enabled And cbo_Godown_StockIN.Visible Then cbo_Godown_StockIN.Focus()
                Exit Sub
            End If
        End If
        If vGod_ID = 0 Then vGod_ID = Common_Procedures.CommonLedger.Godown_Ac

        If Trim(lbl_OrderCode.Text) <> "" Then


            If Led_ID <> 0 Then

                Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Own_Order_Head a INNER JOIN Own_order_Weaving_Details b ON a.Own_Order_Code =b.Own_Order_Code where a.Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "' and  b.Ledger_idno = " & Str(Val(Led_ID)), con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    OurOrd_No = Dt1.Rows(0).Item("Order_No").ToString

                End If
            End If
            If Trim(OurOrd_No) <> Trim(lbl_OrderNo.Text) Then
                MessageBox.Show("Invalid Mismatch Of Order No for this Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus()
                Exit Sub
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
            Da = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Delivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(Led_ID)) & " and Party_DcNo = '" & Trim(txt_PDcNo.Text) & "' and Weaver_ClothDelivery_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_ClothDelivery_Return_Code <> '" & Trim(NewCode) & "'", con)
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

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1204" Then '------KOHINOOR TEXTILE MILLS
            If Lm_ID = 0 Then
                MessageBox.Show("Invalid Loom No?", "DOESN'T SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Trim(cbo_WidthType.Text) = "" Then
                MessageBox.Show("Invalid Width Type?", "DOESNOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                cbo_WidthType.Focus()
                Exit Sub
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
        If cbo_StockOff.Visible = True Then
            vStkOf_Pos_IdNo = StkOff_ID

        Else

            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                vStkOf_Pos_IdNo = Led_ID
            Else
                vStkOf_Pos_IdNo = Common_Procedures.CommonLedger.OwnSort_Ac  ' Val(Common_Procedures.CommonLedger.Godown_Ac)
            End If

        End If


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

                End If

            Next

        End With

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
                pnl_DriverDetails.Visible = True
                pnl_Back.Enabled = False
                If cbo_VehicleNo.Enabled And cbo_VehicleNo.Visible Then cbo_VehicleNo.Focus()
                Exit Sub
            End If
        End If

        If Trim(cbo_VehicleNo.Text) <> "" Then
            cbo_VehicleNo.Text = Common_Procedures.Vehicle_Number_Remove_Unwanted_Spaces(Trim(cbo_VehicleNo.Text))
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


        If txt_LotNo.Visible = True Then
            If Trim(txt_LotNo.Text) = "" Then
                MessageBox.Show("Invalid Lot No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_LotNo.Enabled And txt_LotNo.Visible Then txt_LotNo.Focus()
                Exit Sub
            End If

            If Trim(txt_LotNo.Text) <> "" Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)
                Da = New SqlClient.SqlDataAdapter("Select * from Weaver_Cloth_Delivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_IdNo = " & Str(Val(Clo_ID)) & " and Lot_No = '" & Trim(txt_LotNo.Text) & "' and Weaver_ClothDelivery_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_ClothDelivery_Return_Code <> '" & Trim(NewCode) & "'", con)
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

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Delivery_Return_Head", "Weaver_ClothDelivery_Return_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_Code, Weaver_Wages_Code, Weaver_IR_Wages_Code from Weaver_Cloth_Delivery_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "'", con)
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

            If New_Entry = True Then
                cmd.CommandText = "Insert into Weaver_Cloth_Delivery_Return_Head ( Receipt_Type, Weaver_ClothDelivery_Return_Code,             Company_IdNo         ,       Weaver_ClothDelivery_Return_No  ,                               for_OrderBy                              , Weaver_ClothDelivery_Return_Date ,           Ledger_IdNo   ,      Cloth_IdNo    ,            Lot_No             ,            Party_DcNo         ,           EndsCount_IdNo   ,          Count_IdNo        ,             empty_beam          ,           noof_pcs            ,             pcs_fromno         ,              pcs_tono        ,           ReceiptMeters_Receipt        ,  Receipt_Quantity                    ,             Receipt_Meters               ,          ConsumedYarn_Receipt       ,              Consumed_Yarn        ,         ConsumedPavu_Receipt       ,              Consumed_Pavu         ,       Loom_IdNo        ,              Width_Type           ,       Transport_IdNo      ,     Freight_Amount_Receipt   ,              Folding_Receipt        , Folding,       Total_Receipt_Pcs      ,    Total_Receipt_Meters   , BeamConsumption_Receipt, BeamConsumption_Meters, Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment, Weaver_Wages_Code, Weaver_Wages_Increment    ,            StockOff_IdNo   ,                           User_idNo      , Purchase_Status      , Our_Order_No                     , Own_Order_Code                      ,     Loom_Type                      , No_Stock_Posting_Status     ,              Driver_Name           ,               Driver_Phone_No       ,                Supervisor_Name           , Vehicle_no                           ,        WareHouse_IdNo     , No_Of_Bundles                            , Unloaded_By_Our_Employee ,    Verified_Status       ,       No_Weaving_Wages_Bill      , Weaver_IR_Wages_Code ) " &
                                    "           Values                   (     'W'     ,         '" & Trim(NewCode) & "' ,           " & Str(Val(lbl_Company.Tag)) & ",      '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @EntryDate          ,             " & Str(Val(Led_ID)) & ",   " & Val(Clo_ID) & " , '" & Trim(txt_LotNo.Text) & "', '" & Trim(txt_PDcNo.Text) & "', " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(WftCnt_ID)) & ", " & Str(Val(txt_EBeam.Text)) & ", " & Val(txt_NoOfPcs.Text) & " , " & Val(txt_PcsNoFrom.Text) & ", " & Val(lbl_PcsNoTo.Text) & ", " & Str(Val(txt_ReceiptMeters.Text)) & ", " & Str(Val(txt_Quantity.Text)) & " ,  " & Str(Val(txt_ReceiptMeters.Text)) & ", " & Str(Val(lbl_ConsYarn.Text)) & ", " & Str(Val(lbl_ConsYarn.Text)) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(Lm_ID)) & ", '" & Trim(cbo_WidthType.Text) & "', " & Str(Val(Trans_ID)) & ", " & Val(txt_Freight.Text) & ", " & Val(txt_Folding_Perc.Text) & "  ,   100  , " & Str(Val(vTotRcptPcs)) & ", " & Str(Val(vTotRcptMtrs)) & ",          0             ,          0            ,          ''               ,             0                  ,         ''       ,           0           ," & Str(Val(StkOff_ID)) & " , " & Val(Common_Procedures.User.IdNo) & " , " & Val(Purc_STS) & ",'" & Trim(lbl_OrderNo.Text) & "' ,    '" & Trim(lbl_OrderCode.Text) & "', '" & Trim(cbo_LoomType.Text) & "'  ,   " & Val(NoStkPos_Sts) & " ,'" & Trim(cbo_DriverName.Text) & "' , '" & Trim(cbo_DriverPhNo.Text) & "' ,  '" & Trim(cbo_SupervisorName.Text) & "' , '" & Trim(cbo_VehicleNo.Text) & "'   , " & Str(Val(vGod_ID)) & " ,  " & Val(Txt_NoOfBundles.Text) & "       ," & Val(Vchk_UNLOADED) & ", " & Val(Verified_STS) & ", " & Val(NoWeaWages_Bill_Sts) & " ,        ''            ) "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Cloth_Delivery_Return_Head", "Weaver_ClothDelivery_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_ClothDelivery_Return_Code, Company_IdNo, for_OrderBy", tr)

                cmd.CommandText = "Update Weaver_Cloth_Delivery_Return_Head set Receipt_Type = 'W', Weaver_ClothDelivery_Return_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Cloth_IdNo = " & Val(Clo_ID) & " , Lot_No = '" & Trim(txt_LotNo.Text) & "',  Party_DcNo  = '" & Trim(txt_PDcNo.Text) & "',  EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Count_IdNo = " & Str(Val(WftCnt_ID)) & ", empty_beam = " & Val(txt_EBeam.Text) & " , noof_pcs = " & Val(txt_NoOfPcs.Text) & " , pcs_fromno = " & Val(txt_PcsNoFrom.Text) & " , pcs_tono = " & Val(lbl_PcsNoTo.Text) & " , ReceiptMeters_Receipt = " & Val(txt_ReceiptMeters.Text) & ", ConsumedYarn_Receipt = " & Val(lbl_ConsYarn.Text) & ", ConsumedPavu_Receipt = " & Val(lbl_ConsPavu.Text) & ", Loom_IdNo = " & Val(Lm_ID) & ", Width_Type = '" & Trim(cbo_WidthType.Text) & "', Transport_IdNo = " & Val(Trans_ID) & ", Freight_Amount_Receipt = " & Val(txt_Freight.Text) & ", Total_Receipt_Pcs = " & Str(Val(vTotRcptPcs)) & ", Total_Receipt_Meters = " & Str(Val(vTotRcptMtrs)) & " , StockOff_IdNo = " & Str(Val(StkOff_ID)) & ",  User_idNo = " & Val(Common_Procedures.User.IdNo) & " ,  Purchase_Status = " & Val(Purc_STS) & ", Our_Order_No = '" & Trim(lbl_OrderNo.Text) & "', Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "', Loom_Type =  '" & Trim(cbo_LoomType.Text) & "' , Receipt_Quantity = " & Str(Val(txt_Quantity.Text)) & " , No_Stock_Posting_Status = " & Val(NoStkPos_Sts) & " , Driver_Name = '" & Trim(cbo_DriverName.Text) & "' , Driver_Phone_No = '" & Trim(cbo_DriverPhNo.Text) & "' , Supervisor_Name = '" & Trim(cbo_SupervisorName.Text) & "' , Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' , WareHouse_IdNo = " & Str(Val(vGod_ID)) & " , No_Of_Bundles =  " & Val(Txt_NoOfBundles.Text) & ",Unloaded_By_Our_Employee=" & Val(Vchk_UNLOADED) & ",Verified_Status= " & Val(Verified_STS) & ", No_Weaving_Wages_Bill = " & Val(NoWeaWages_Bill_Sts) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Cloth_Delivery_Return_Head set Receipt_Meters = " & Val(txt_ReceiptMeters.Text) & ", Consumed_Yarn = " & Val(lbl_ConsYarn.Text) & ", Consumed_Pavu = " & Val(lbl_ConsPavu.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = '' and Weaver_Wages_Code = '' and Weaver_IR_Wages_Code = ''"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Weaver_Cloth_Delivery_Return_Head", "Weaver_ClothDelivery_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_ClothDelivery_Return_Code, Company_IdNo, for_OrderBy", tr)

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
                Partcls = Trim(Partcls) & ",  Cloth : " & Trim(cbo_Cloth.Text) & ", Ends : " & Trim(cbo_EndsCount.Text) & ", Pcs : " & Trim(txt_NoOfPcs.Text)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Then
                Partcls = "CloRcpt : LotNo. " & Trim(lbl_RefNo.Text)
                If Trim(txt_PDcNo.Text) <> "" Then
                    Partcls = Trim(Partcls) & ",  P.Dc.No : " & Trim(txt_PDcNo.Text)
                End If
                Partcls = Trim(Partcls) & ",  Cloth : " & Trim(cbo_Cloth.Text)

            Else

                Partcls = "CloRcpt : LotNo. " & Trim(lbl_RefNo.Text)
                If Trim(txt_PDcNo.Text) <> "" Then
                    Partcls = Trim(Partcls) & ",  P.Dc.No : " & Trim(txt_PDcNo.Text)
                End If

            End If

            PBlNo = Trim(lbl_RefNo.Text)

            cmd.CommandText = "Update Weaver_Cloth_Delivery_Return_Head set Report_Particulars_Receipt = '" & Trim(Partcls) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Weaver_Cloth_Delivery_Return_Head set Report_Particulars = '" & Trim(Partcls) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = ''"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and Create_Status = 1 and Weaver_Piece_Checking_Code = ''"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'With dgv_Details

            '    Sno = 0
            '    For i = 0 To dgv_Details.RowCount - 1

            '        If Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Then

            '            Sno = Sno + 1

            '            Nr = 0
            '            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Weaver_ClothDelivery_Return_Date = @EntryDate, Sl_No = " & Str(Val(Sno)) & ", Main_PieceNo = '" & Trim(Val(.Rows(i).Cells(0).Value)) & "' , PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(0).Value))) & ", ReceiptMeters_Receipt = " & Val(.Rows(i).Cells(1).Value) & ", Create_Status = 1, StockOff_IdNo = " & Str(Val(vStkOf_Pos_IdNo)) & ", WareHouse_IdNo = " & Str(Val(vGod_ID)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
            '            Nr = cmd.ExecuteNonQuery()

            '            If Nr = 0 Then
            '                cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_ClothDelivery_Return_Code ,            Company_IdNo          ,      Weaver_ClothDelivery_Return_No   ,                               for_OrderBy                               , Weaver_ClothDelivery_Return_Date,           Lot_Code      ,             Lot_No            ,     Cloth_IdNo          ,            Folding_Receipt         , Folding,         Sl_No        ,                     Piece_No           ,                  Main_PieceNo               ,                               PieceNo_OrderBy                                   ,     ReceiptMeters_Receipt           ,                Receipt_Meters       , Create_Status ,              StockOff_IdNo       ,          WareHouse_IdNo    ) " &
            '                "Values                                        ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,          @EntryDate     ,  '" & Trim(NewCode) & "', '" & Trim(txt_LotNo.Text) & "', " & Str(Val(Clo_ID)) & ", " & Val(txt_Folding_Perc.Text) & " ,   100  , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(0).Value) & "', '" & Trim(Val(.Rows(i).Cells(0).Value)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(0).Value))) & ", " & Val(.Rows(i).Cells(1).Value) & ", " & Val(.Rows(i).Cells(1).Value) & ",       1       , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(vGod_ID)) & "  ) "
            '                cmd.ExecuteNonQuery()
            '            End If

            '        End If

            '    Next
            '    Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_ClothReceipt_Piece_Details", "Weaver_ClothDelivery_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Piece_No,Main_PieceNo,PieceNo_OrderBy,ReceiptMeters_Receipt,Receipt_Meters, Create_Status ,StockOff_IdNo,WareHouse_IdNo", "Sl_No", "Weaver_ClothDelivery_Return_Code, For_OrderBy, Company_IdNo, Weaver_ClothDelivery_Return_No, Weaver_ClothDelivery_Return_Date, Ledger_Idno", tr)

            'End With

            'If Val(txt_EBeam.Text) <> 0 Then
            '    cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo                     , Reference_No                  , for_OrderBy                                                            , Reference_Date, DeliveryTo_Idno                                           , ReceivedFrom_Idno       , Entry_ID             , Party_Bill_No        , Particulars            , Sl_No, Beam_Width_IdNo, Empty_Beam                       ) " &
            '    "Values                                    ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate    , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1    , 0              , " & Str(Val(txt_EBeam.Text)) & " )"
            '    cmd.ExecuteNonQuery()
            'End If

            If Trim(PcsChkCode) = "" And Trim(WagesCode) = "" Then

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
                Stock_In = ""
                mtrspcs = 0

                Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  EndsCount_Head Where EndsCount_IdNo = " & Str(Val(EdsCnt_ID)), con)
                Da.SelectCommand.Transaction = tr
                dt2 = New DataTable
                Da.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    Stock_In = dt2.Rows(0)("Stock_In").ToString
                    mtrspcs = Val(dt2.Rows(0)("Meters_Pcs").ToString)
                End If
                dt2.Clear()

                If Trim(UCase(Stock_In)) = "PCS" Then
                    lbl_ConsPavu.Text = Val(txt_NoOfPcs.Text)
                End If

                If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then

                End If

                If Trim(UCase(EntFnYrCode)) = Trim(UCase(Common_Procedures.FnYearCode)) Then

                    If Val(Purc_STS) = 0 Then

                        vDelv_ID = 0 : vRec_ID = 0
                        If Trim(UCase(Led_type)) = "JOBWORKER" Then
                            vDelv_ID = 0
                            vRec_ID = Led_ID
                        Else
                            vDelv_ID = Led_ID
                            vRec_ID = 0
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
                dt2 = New DataTable
                Da.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    clthStock_In = dt2.Rows(0)("Stock_In").ToString
                    clthmtrspcs = Val(dt2.Rows(0)("Meters_Pcs").ToString)
                End If
                dt2.Clear()

                clthPcs_Mtr = 0
                If Trim(UCase(Stock_In)) = "PCS" Then

                    clthPcs_Mtr = Val(txt_NoOfPcs.Text)

                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (                 Reference_Code             ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,       StockOff_IdNo              ,        DeliveryTo_Idno        ,       ReceivedFrom_Idno       ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     , Folding  ,   UnChecked_Meters  ,  Meters_Type1                 , Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ) " &
                                              "    Values                         ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(StkDelvTo_ID)) & ", " & Str(Val(StkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ",   100    ,                0    , " & Str(Val(clthPcs_Mtr)) & " ,       0     ,       0     ,       0     ,       0      ) "
                    cmd.ExecuteNonQuery()

                Else

                    If Trim(UCase(clthStock_In)) = "PCS" Then
                        clthPcs_Mtr = Val(txt_NoOfPcs.Text)
                    Else
                        clthPcs_Mtr = Val(txt_ReceiptMeters.Text)
                    End If

                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code            ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,       StockOff_IdNo              ,        DeliveryTo_Idno        ,       ReceivedFrom_Idno       ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     , Folding  ,             UnChecked_Meters ,  Meters_Type1, Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ) " &
                    "Values                                     ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(StkDelvTo_ID)) & ", " & Str(Val(StkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ",   100    , " & Str(Val(clthPcs_Mtr)) & ",       0      ,       0     ,       0     ,       0     ,       0      ) "
                    cmd.ExecuteNonQuery()

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
                    vDelv_ID = 0
                    vRec_ID = Led_ID
                Else
                    vDelv_ID = Led_ID
                    vRec_ID = 0
                End If

                cmd.CommandText = "Update Stock_Pavu_Processing_Details Set DeliveryTo_Idno = " & Str(Val(vDelv_ID)) & ",  ReceivedFrom_Idno = " & Str(Val(vRec_ID)) & ",  EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Yarn_Processing_Details Set DeliveryTo_Idno = " & Str(Val(vDelv_ID)) & ",  ReceivedFrom_Idno = " & Str(Val(vRec_ID)) & " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Dim vVou_LedIdNos As String = "", vVou_ErrMsg As String = ""
            vVou_Amts = ""

            If Val(txt_Freight.Text) = 0 Then txt_Freight.Text = 0.0

            vVou_LedIdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "Wea.CloDelRet.Frgt", Val(lbl_Company.Tag), Trim(PkCondition_WFRGT) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_date.Text), Partcls, vVou_LedIdNos, vVou_Amts, vVou_ErrMsg, tr) = False Then
                Throw New ApplicationException(vVou_ErrMsg)
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_WidthType, Txt_NoOfBundles, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver, cbo_LoomType, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaver, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                Dim da As New SqlClient.SqlDataAdapter
                Dim dt As New DataTable
                Dim vLed_id As Integer
                vLed_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_Weaver.Text)

                da = New SqlClient.SqlDataAdapter("select  a.Weaver_LoomType from  ledger_head a  where a.Ledger_IdNo = " & Str(Val(vLed_id)), con)
                da.Fill(dt)

                cbo_LoomType.Text = dt.Rows(0).Item("Weaver_LoomType")
                dt.Clear()
            End If

            If Common_Procedures.settings.Internal_Order_Entry_Status = 1 Then
                If MessageBox.Show("Do you want to select Internal Order", "FOR INTERNAL ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)
                Else
                    cbo_Cloth.Focus()
                End If

            Else
                cbo_Cloth.Focus()
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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, txt_PDcNo, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        If e.KeyValue = 40 Then
            If txt_EBeam.Visible = True Then
                txt_EBeam.Focus()
            Else
                txt_NoOfPcs.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If txt_EBeam.Visible = True Then
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

                ' Condt = "a.Weaver_ClothDelivery_Return_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
                Condt = "a.Weaver_ClothDelivery_Return_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_ClothDelivery_Return_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_ClothDelivery_Return_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = "a.Weaver_ClothDelivery_Return_No between '" & Trim(txt_Filter_RecNo.Text) & "' and '" & Trim(txt_Filter_RecNoTo.Text) & "'"
            ElseIf Trim(txt_Filter_RecNo.Text) <> "" Then
                Condt = "a.Weaver_ClothDelivery_Return_No  = '" & Trim(txt_Filter_RecNo.Text) & "'"
            ElseIf Trim(txt_Filter_RecNoTo.Text) <> "" Then
                Condt = "a.Weaver_ClothDelivery_Return_No  = '" & Trim(txt_Filter_RecNoTo.Text) & "'"
            End If


            If cbo_Verified_Sts.Visible = True And Trim(cbo_Verified_Sts.Text) <> "" Then

                If Trim(cbo_Verified_Sts.Text) = "YES" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 1 "
                ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 0 "
                End If

            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as WeaverName ,E.EndsCount_Name, c.*  from Weaver_Cloth_Delivery_Return_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Cloth_Head c ON c.Cloth_IdNo = a.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head E ON E.EndsCount_IdNo = a.EndsCount_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothDelivery_Return_Code LIKE '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Weaver_ClothDelivery_Return_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Weaver_ClothDelivery_Return_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_ClothDelivery_Return_Date").ToString), "dd-MM-yyyy")
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

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim n As Integer = 0
        Dim Nextvalue As Integer = 0
        With dgv_Details



            If e.RowIndex = 0 Then
                .CurrentRow.Cells(0).Value = Val(txt_PcsNoFrom.Text)
            Else
                'If Val(.CurrentRow.Cells(0).Value) = 0 Then
                '    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                'End If

                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = Val(.Rows(e.RowIndex - 1).Cells(0).Value) + 1
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

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim TotMtrs As Single = 0

        On Error Resume Next

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Then
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

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_Code, Weaver_Wages_Code, Weaver_IR_Wages_Code from Weaver_Cloth_Delivery_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "'", con)
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
                    If i = 0 Then
                        .Rows(i).Cells(0).Value = Val(PcsFrmNo)
                    Else
                        .Rows(i).Cells(0).Value = Val(.Rows(i - 1).Cells(0).Value) + 1
                    End If
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

        On Error Resume Next

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details

            If e.RowIndex = 0 Then
                .CurrentRow.Cells(0).Value = Val(txt_PcsNoFrom.Text)

            Else
                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = Val(.Rows(e.RowIndex - 1).Cells(0).Value) + 1
                End If

            End If

        End With

    End Sub

    Private Sub Total_Calculation()
        Dim TotPcs As Single, TotMtrs As Single

        TotPcs = 0
        TotMtrs = 0
        With dgv_Details

            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(1).Value) <> 0 Then
                    TotPcs = TotPcs + 1
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(1).Value)
                End If
            Next

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(0).Value = Val(TotPcs)
            .Rows(0).Cells(1).Value = Format(Val(TotMtrs), "########0.00")
        End With

        If Val(TotMtrs) <> 0 Then txt_ReceiptMeters.Text = Format(Val(TotMtrs), "#########0.00")

    End Sub

    Private Sub cbo_Cloth_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "cloth_name", "", "(cloth_name)")
    End Sub

    Private Sub cbo_cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, cbo_Weaver, Nothing, "Cloth_Head", "cloth_name", "", "(cloth_name)")
        If (e.KeyValue = 40 And cbo_Cloth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If txt_Folding_Perc.Visible And txt_Folding_Perc.Enabled = True Then
                txt_Folding_Perc.Focus()
            ElseIf txt_LotNo.Visible And txt_LotNo.Enabled = True Then
                txt_LotNo.Focus()

            Else
                txt_PDcNo.Focus()
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

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, Nothing, "Cloth_Head", "cloth_name", "", "(cloth_name)")

        If Asc(e.KeyChar) = 13 Then
            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

            wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
            lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, wftcnt_idno)

            If Trim(cbo_EndsCount.Text) = "" Then
                edscnt_idno = Val(Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "EndsCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & " and Sl_No = 1 )"))
                cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, edscnt_idno)
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
            If cbo_StockOff.Visible = True Then
                cbo_StockOff.Focus()
            ElseIf cbo_Godown_StockIN.Visible Then
                cbo_Godown_StockIN.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
                End If
            End If

        End If
        If e.KeyCode = 38 Then Txt_NoOfBundles.Focus()
    End Sub

    Private Sub txt_PcsNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PcsNoFrom.KeyDown
        If e.KeyCode = 40 Then
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.Focus()
            dgv_Details.CurrentCell.Selected = True

        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
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

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        With dgv_Details_Total
            TotMtrs = 0
            If .RowCount > 0 Then
                TotMtrs = Val(.Rows(0).Cells(1).Value)
            End If
        End With
        If Val(TotMtrs) <> 0 Then e.Handled = True

    End Sub

    Private Sub txt_weft_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_PcsNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsNoFrom.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.Focus()
            dgv_Details.CurrentCell.Selected = True
        End If

    End Sub

    Private Sub txt_NoOfPcs_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NoOfPcs.GotFocus
        txt_NoOfPcs.Tag = txt_NoOfPcs.Text
    End Sub

    Private Sub txt_NoofPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfPcs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If Val(txt_NoOfPcs.Tag) <> Val(txt_NoOfPcs.Text) Then
                txt_NoOfPcs.Tag = txt_NoOfPcs.Text
                Design_PieceDetails_Grid()
            End If
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If cbo_StockOff.Visible = True Then
                cbo_StockOff.Focus()
            ElseIf cbo_Godown_StockIN.Visible Then
                cbo_Godown_StockIN.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
                End If
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
                ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(txt_NoOfPcs.Text))
            Else
                ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(txt_ReceiptMeters.Text))
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
            ConsYarn = Format(Val(txt_ReceiptMeters.Text), "##########0")

        Else


            ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(txt_ReceiptMeters.Text))

        End If

        lbl_ConsYarn.Text = Format(ConsYarn, "#########0.000")


    End Sub

    Private Sub ConsumedPavu_Calculation()
        Dim CloID As Integer = 0
        Dim ConsPavu As Single = 0
        Dim LmID As Integer = 0
        Dim Clo_Mtrs_Pc As Single = 0

        CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        LmID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        ConsPavu = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
            Clo_Mtrs_Pc = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Meters_Pcs", "(Cloth_idno = " & Str(Val(CloID)) & ")"))
            If Val(Clo_Mtrs_Pc) = 0 Then Clo_Mtrs_Pc = 40
            ConsPavu = Format(Val(Clo_Mtrs_Pc) * Val(txt_NoOfPcs.Text), "##########0.00")

        Else

            ConsPavu = Common_Procedures.get_Pavu_Consumption(con, CloID, LmID, Val(txt_ReceiptMeters.Text), Trim(cbo_WidthType.Text))


        End If

        lbl_ConsPavu.Text = Format(ConsPavu, "##########0.00")

    End Sub

    Private Sub txt_Meters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ReceiptMeters.TextChanged
        ConsumedYarn_Calculation()
        ConsumedPavu_Calculation()
        Consumption_Calculation()
    End Sub


    Private Sub PieceNo_To_Calculation()
        Dim vTotPcs As Integer = 0
        Dim vTotMtrs As Integer = 0
        Dim vPcsFrmNo As Integer = 0

        lbl_PcsNoTo.Text = ""

        If Val(txt_NoOfPcs.Text) > 0 Then

            If Val(txt_PcsNoFrom.Text) = 0 Then txt_PcsNoFrom.Text = "1"

            lbl_PcsNoTo.Text = Val(txt_PcsNoFrom.Text) + Val(txt_NoOfPcs.Text) - 1

        End If


        'If Val(txt_NoOfPcs.Text) = 0 Then

        '    With dgv_Details_Total
        '        If .RowCount > 0 Then
        '            vTotPcs = Val(.Rows(0).Cells(0).Value)
        '            vTotMtrs = Val(.Rows(0).Cells(1).Value)
        '        End If
        '    End With

        '    If Val(vTotMtrs) > 0 Then

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


        'Else
        '    If Val(txt_PcsNoFrom.Text) = 0 Then txt_PcsNoFrom.Text = "1"

        '    lbl_PcsNoTo.Text = Val(txt_PcsNoFrom.Text) + Val(txt_NoOfPcs.Text) - 1

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

        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

        If Val(Clo_IdNo) <> 0 And Val(No_Of_Pcs) <> 0 Then
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Then '---- Rajeswari Weaving (Karumanthapatti) - Somanur Sizing
            If Val(txt_NoOfPcs.Tag) <> Val(txt_NoOfPcs.Text) Then
                txt_NoOfPcs.Tag = txt_NoOfPcs.Text
                Design_PieceDetails_Grid()
            End If
        End If


    End Sub

    Private Sub txt_NoOfPcs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_NoOfPcs.TextChanged
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
            ConsumedYarn_Calculation()
            ConsumedPavu_Calculation()
        End If
        PieceNo_To_Calculation()
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, cbo_LoomNo, cbo_Transport, "", "", "", "")

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

                If .CurrentCell.ColumnIndex = 1 Then

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

        ElseIf e.KeyValue = 46 Then
            With dgv_Details
                If .CurrentCell.ColumnIndex = 1 Then
                    .Rows(.CurrentCell.RowIndex).Cells(1).Value = ""

                End If

            End With

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
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Delivery_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "'", con)
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

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.5X12", 850, 1200)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* ,c.* ,d.* , e.*, t.Ledger_Name as Transport , Ig.Item_Hsn_Code  from Weaver_Cloth_Delivery_Return_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head T ON T.Ledger_IdNo = a.Transport_Idno LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno LEFT OUTER JOIN ItemGroup_Head IG On Ig.ItemGroup_Idno = d.ItemGroup_Idno INNER JOIN State_Head e On e.State_Idno = b.Company_State_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count = 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage


        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_Delivery_Format3(e)

    End Sub

    Private Sub Printing_Delivery_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        'Dim ps As Printing.PaperSize
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
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        'Dim PpSzSTS As Boolean = False
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0
        Dim Tot_Mtr As Integer = 0, Tot_Rt As Integer = 0
        Dim vAmt As String = 0
        Dim fldmtr As String = 0 'Double = 0
        Dim fmt As Double = 0
        Dim CurRow As Integer = 0
        set_PaperSize_For_PrintDocument1()

        'If Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Then
        '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        '    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        '    PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        '    PrintDocument1.DefaultPageSettings.Landscape = False

        'ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_A4_As_Default_PaperSize) = 1 Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            Exit For
        '        End If
        '    Next

        'ElseIf Val(vPrnt_2Copy_In_SinglePage) = 1 Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            Exit For
        '        End If
        '    Next



        'Else

        '    PpSzSTS = False

        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        'Debug.Print(ps.PaperName)
        '        If ps.Width = 800 And ps.Height = 600 Then
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '    If PpSzSTS = False Then

        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                PpSzSTS = True
        '                Exit For
        '            End If
        '        Next

        '        If PpSzSTS = False Then
        '            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                    PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                    Exit For
        '                End If
        '            Next
        '        End If

        '    End If

        'End If

        PrntCnt = 1
        If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 50
            .Top = 10
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            pFont = New Font("Calibri", 9, FontStyle.Regular)
        Else
            pFont = New Font("Calibri", 10, FontStyle.Regular)
        End If
        'pFont = New Font("Calibri", 11, FontStyle.Regular)

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

        If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
            NoofItems_PerPage = 4
        Else
            NoofItems_PerPage = 5  '17.5 '20
        End If

        If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1168" Then
        '    NoofItems_PerPage = 4
        'Else
        '    NoofItems_PerPage = 5
        'End If

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        'If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then
        '    ClAr(1) = Val(35) : ClAr(2) = 250 : ClAr(3) = 70 : ClAr(4) = 60 : ClAr(5) = 50 : ClAr(6) = 60 : ClAr(7) = 70 : ClAr(8) = 60
        '    ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))
        'Else
        ClAr(1) = Val(35) : ClAr(2) = 350 : ClAr(3) = 80 : ClAr(4) = 80 : ClAr(5) = 80
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))
        'End If


        TxtHgt = 15 '16 '18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin
        If Trim(Common_Procedures.settings.CustomerCode) = "1037" And vPrnt_2Copy_In_SinglePage = 1 Then
            PCnt = PCnt + 1
            PrntCnt = PrntCnt + 1
        End If

        For PCnt = 1 To PrntCnt

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin


                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If

            End If

            Try
                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Delivery_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_HdDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_HdDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then
                                If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then

                                    NoofDets = NoofDets + 1

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                    Printing_Delivery_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Delivery_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If


                            ItmNm1 = Trim(prn_HdDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                            If Trim(ItmNm1) = "" Then
                                ItmNm1 = Trim(prn_HdDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                            End If
                            ItmNm2 = ""
                            If Len(ItmNm1) > 40 Then
                                For I = 40 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 40
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "1", LMargin + 15, CurY, 0, 0, pFont)

                            'If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then

                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            'End If
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Item_Hsn_Code").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("No_Of_Bundles").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Noof_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)


                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Receipt_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)






                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                    Printing_Delivery_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

                End If


            Catch ex As Exception
                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_HdDt.Rows.Count > prn_NoofBmDets Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 6
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
                'prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_NoofBmDets = 0
                vprn_TotAmt = 0

                e.HasMorePages = True
                Return

            End If

        End If
    End Sub

    Private Sub Printing_Delivery_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2, da3 As New SqlClient.SqlDataAdapter
        Dim dt2, dt3 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single, strWidth As Single = 0
        Dim C1 As Single
        Dim W1, w2 As Single
        Dim S1, S2, S3 As Single
        Dim vprn_BlNos As String = ""
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim CurX As Single = 0
        Dim vPackType As String = ""
        Dim suppRefNo As String = ""
        Dim payment As String = ""
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from ClothSales_Delivery_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        suppRefNo = ""
        payment = ""
        da3 = New SqlClient.SqlDataAdapter("select a.*, b.Clothsales_Order_no,b.payment_terms from ClothSales_Delivery_Details a INNER JOIN Clothsales_Order_Head b ON a.Clothsales_Order_Code = b.Clothsales_Order_Code  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(EntryCode) & "'", con)
        dt3 = New DataTable
        da3.Fill(dt3)
        If dt3.Rows.Count > 0 Then
            suppRefNo = dt3.Rows(0).Item("ClothSales_Order_No").ToString
            payment = dt3.Rows(0).Item("payment_terms").ToString
        End If
        dt3.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("State_Code").ToString) <> "" Then
                Cmp_StateNm = Cmp_StateNm & "   CODE : " & prn_HdDt.Rows(0).Item("State_Code").ToString
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        CurY = CurY + TxtHgt - 10

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
        End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 13  ' 10

        Else

            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
            If PrintWidth > strWidth Then
                CurX = LMargin + (PrintWidth - strWidth) / 2
            Else
                CurX = LMargin
            End If

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

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt - 1

        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH RETURN DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        w2 = e.Graphics.MeasureString("DELIVERY.At : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        S2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MAInName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_ClothDelivery_Return_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_ClothDelivery_Return_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)




        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " TIN NO : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

        Else
            If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, p1Font)
            End If

        End If




        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + 5


        '  CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO  ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

        End If
        'Common_Procedures.Print_To_PrintDocument(e, "DESP.TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)


        '  CurY = CurY + TxtHgt

        '  Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Ledger_MAInName").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY .At", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ledger_MAInName").ToString), LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        End If


        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Transport").ToString) <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "AGENT ", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)


        'If Trim(prn_HdDt.Rows(0).Item("JJ_FormNo").ToString) <> "" Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No.", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JJ_FormNo").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
        'End If
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        'vprn_BlNos = ""
        'For i = 0 To prn_DetDt.Rows.Count - 1
        '    If Trim(prn_DetDt.Rows(i).Item("Bales_Nos").ToString) <> "" Then
        '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(i).Item("Bales_Nos").ToString
        '    End If
        'Next
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        'vPackType = Trim(UCase(prn_HdDt.Rows(0).Item("Packing_Type").ToString))
        'If Trim(vPackType) = "" Then vPackType = "BALE"
        'Common_Procedures.Print_To_PrintDocument(e, Trim(vPackType) & " NOS : " & vprn_BlNos, LMargin + C1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        pFont = New Font("Calibri", 10, FontStyle.Bold)
        CurY = CurY + 5

        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO OF . ", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO OF . ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)




        ' CurY = CurY + TxtHgt + 20
        Common_Procedures.Print_To_PrintDocument(e, "PCS . ", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 15, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BUNDLES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 15, 2, ClAr(5), pFont)
        CurY = CurY + TxtHgt + 20
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Delivery_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim vPackType As String = ""
        Dim BLNo1 As String = ""
        Dim BLNo2 As String = ""


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + 5
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("No_of_Bundles").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Noof_Pcs").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Receipt_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

        CurY = CurY + 10



        If Common_Procedures.User.IdNo <> 1 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 20, CurY, 0, 0, pFont)
        End If



        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 170, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 320, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub
    Private Sub set_PaperSize_For_PrintDocument1()
        Dim I As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next


        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Or Prnt_HalfSheet_STS = True Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_A4_As_Default_PaperSize) = 1 Or Val(Common_Procedures.settings.Printing_For_FullSheet_Set_A4_As_Default_PaperSize) = 1 Or Val(vPrnt_2Copy_In_SinglePage) = 1 Then

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
                cbo_LoomType.Focus()
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
                cbo_LoomType.Focus()
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
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
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

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        'If LedIdNo = 0 Then
        '    MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
        '    Exit Sub
        'End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection
            If Val(LedIdNo) <> 0 Then

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name,c.*  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno  INNER JOIN Own_Order_Weaving_Details c ON  c.Own_Order_Code = a.Own_Order_Code LEFT OUTER JOIN Weaver_Cloth_Delivery_Return_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "'   and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
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

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno INNER JOIN Own_Order_Weaving_Details c ON  c.Own_Order_Code = a.Own_Order_Code   LEFT OUTER JOIN Weaver_Cloth_Delivery_Return_Head d ON d.Weaver_ClothDelivery_Return_Code = a.Own_Order_Code    where a.Weaver_ClothDelivery_Return_Code = ''  and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
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

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno   LEFT OUTER JOIN Weaver_Cloth_Delivery_Return_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Weaver_ClothDelivery_Return_Code = '" & Trim(NewCode) & "'   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
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

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno    LEFT OUTER JOIN Weaver_Cloth_Delivery_Return_Head d ON d.Weaver_ClothDelivery_Return_Code = a.Own_Order_Code    where a.Weaver_ClothDelivery_Return_Code = ''   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
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

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

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
        Close_Receipt_Selection()
    End Sub

    Private Sub Close_Receipt_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        lbl_OrderNo.Text = ""
        lbl_OrderCode.Text = ""

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(6).Value) = 1 Then

                ' lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(8).Value

                lbl_OrderNo.Text = dgv_Selection.Rows(i).Cells(3).Value
                lbl_OrderCode.Text = dgv_Selection.Rows(i).Cells(7).Value

            End If

        Next

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

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
    End Sub

    Private Sub btn_Close_DriverDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_DriverDetails.Click
        pnl_DriverDetails.Visible = False
        pnl_Back.Enabled = True
        If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus()
    End Sub

    Private Sub cbo_DriverName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DriverName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Cloth_Delivery_Return_Head", "Driver_Name", "", "")
    End Sub

    Private Sub cbo_DriverName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DriverName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DriverName, cbo_SupervisorName, cbo_VehicleNo, "Weaver_Cloth_Delivery_Return_Head", "Driver_Name", "", "")
    End Sub

    Private Sub cbo_DriverName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DriverName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DriverName, cbo_VehicleNo, "Weaver_Cloth_Delivery_Return_Head", "Driver_Name", "", "", False)
    End Sub

    Private Sub cbo_DriverPhNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DriverPhNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Cloth_Delivery_Return_Head", "Driver_Phone_No", "", "")
    End Sub

    Private Sub cbo_DriverPhNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DriverPhNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DriverPhNo, cbo_DriverName, cbo_SupervisorName, "Weaver_Cloth_Delivery_Return_Head", "Driver_Phone_No", "", "")
    End Sub

    Private Sub cbo_DriverPhNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DriverPhNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DriverPhNo, cbo_SupervisorName, "Weaver_Cloth_Delivery_Return_Head", "Driver_Phone_No", "", "", False)
    End Sub

    Private Sub cbo_SupervisorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SupervisorName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Cloth_Delivery_Return_Head", "Supervisor_Name", "", "")
    End Sub

    Private Sub cbo_SupervisorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SupervisorName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SupervisorName, Nothing, cbo_DriverName, "Weaver_Cloth_Delivery_Return_Head", "Supervisor_Name", "", "")
    End Sub

    Private Sub cbo_SupervisorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SupervisorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SupervisorName, cbo_DriverName, "Weaver_Cloth_Delivery_Return_Head", "Supervisor_Name", "", "", False)
    End Sub

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Cloth_Delivery_Return_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_DriverName, cbo_DriverPhNo, "Weaver_Cloth_Delivery_Return_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, cbo_DriverPhNo, "Weaver_Cloth_Delivery_Return_Head", "Vehicle_No", "", "", False)
    End Sub

    Private Sub btn_DriverDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_DriverDetails.Click
        pnl_DriverDetails.Visible = True
        pnl_Back.Enabled = False
        If cbo_SupervisorName.Visible And cbo_SupervisorName.Enabled Then cbo_SupervisorName.Focus()
    End Sub

    Private Sub cbo_Godown_StockIN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Godown_StockIN.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_StockIN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockIN.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Godown_StockIN, txt_Freight, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_Godown_StockIN.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If (Common_Procedures.settings.CustomerCode = "1267") Then
                chk_UNLOADEDBYOUREMPLOYEE.Focus()


            ElseIf MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Godown_StockIN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Godown_StockIN.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Godown_StockIN, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If (Common_Procedures.settings.CustomerCode = "1267") Then
                chk_UNLOADEDBYOUREMPLOYEE.Focus()
            ElseIf MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_Weaver.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Godown_StockIN_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockIN.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Godown_StockIN.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Txt_NoOfBundles_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Txt_NoOfBundles.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub Design_PieceDetails_Grid()
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Then '---- Rajeswari Weaving (Karumanthapatti) - Somanur Sizing
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

                Grid_PieceNo_Generation()

            End If
        End If

    End Sub

    Private Sub Grid_PieceNo_Generation()
        Dim i As Integer = 0
        Dim PcFrmNo As Integer = 0

        Try

            PieceNo_To_Calculation()

            With dgv_Details
                If .Rows.Count > 0 Then

                    PcFrmNo = Val(txt_PcsNoFrom.Text)
                    If PcFrmNo = 0 Then PcFrmNo = 1

                    .Rows(0).Cells(0).Value = Val(PcFrmNo)

                    For i = 1 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = Val(.Rows(i - 1).Cells(0).Value) + 1
                    Next

                End If

            End With


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
End Class