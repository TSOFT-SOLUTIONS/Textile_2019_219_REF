Public Class JobWork_PieceDelivery_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private Pk_Condition As String = "JPCDC-"
    Private Pk_Condition2 As String = "JPCTR-"
    Private Pk_Condition3 As String = "JPOPD-"
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private Prn_PcNos1 As String
    Private Prn_PcNos2 As String
    Private Prn_PcNos3 As String
    Private Prn_PcNos4 As String
    Private prn_DetAr(500, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer
    Private TrnTo_DbName As String = ""
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Private vcbo_KeyDwnVal As Double
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private prn_HdDt_1 As New DataTable
    Private Print_PDF_Status As Boolean = False
    Private Total_Weight As Double = 0
    Private Total_Mtr As Double = 0
    Private Print_sno As Integer = 0
    Private dgv_ActCtrlName As String = ""
    Private vPRN_FOLDINGPERC As Single
    Private Total_mtrs As Single = 0
    Private dgvDet_CboBx_ColNos_Arr As Integer() = {1}
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_Count As Integer

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False


        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_Order_Selection.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        txt_DespatchTo.Text = ""
        cbo_Ledger.Text = ""
        cbo_ClothName.Text = ""
        lbl_JobDate.Text = ""
        txt_JobNo.Text = ""
        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        txt_Remarks.Text = ""
        chk_Transfer.Checked = False
        chk_Warp_Yarn_Stock_Posting.Checked = False
        cbo_Type.Text = "DIRECT"
        cbo_StockTransferParty.Text = ""
        txt_LotSelction.Text = ""
        txt_PcsSelction.Text = ""
        txt_rate.Text = ""
        lbl_total_amount.Text = ""
        cbo_DeliveryTo.Text = ""
        txt_EWayBillNo.Text = ""
        txt_EWBNo.Text = ""
        rtbEWBResponse.Text = ""


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ClothName.Text = ""
            txt_FilterPcsNo.Text = ""
            txt_FilterRollNo.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        chk_SelectAll.Checked = False
        cbo_Ledger.Tag = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_ClothName.Enabled = True
        cbo_ClothName.BackColor = Color.White

        dtp_Date.Enabled = True
        dtp_Date.BackColor = Color.White

        msk_Date.Enabled = True
        msk_Date.BackColor = Color.White

        btn_Selection.Enabled = True

        Grp_EWB.Visible = False

        txt_Piece_Delv_PrefixNo.Text = ""
        cbo_Piece_Delv_SufixNo.Text = ""
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

                    If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                        Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, Nothing, Nothing, dgvDet_CboBx_ColNos_Arr, dgtxt_Details, msk_Date, 0)

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
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        Dim TrnTo_CmpGrpIdNo As Integer = 0
        Dim DbName As String = ""

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName, c.Ledger_Name as Transport_Name, d.Cloth_Name , del.Ledger_Name as Delivery_Name from JobWork_Piece_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head del ON a.Delivery_Idno = del.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_Idno = d.cloth_idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                txt_Piece_Delv_PrefixNo.Text = dt1.Rows(0).Item("JobWork_Piece_Delivery_PrefixNo").ToString
                cbo_Piece_Delv_SufixNo.Text = dt1.Rows(0).Item("jobwork_Piece_Delivery_SuffixNo").ToString
                lbl_DcNo.Text = dt1.Rows(0).Item("JobWork_Piece_Delivery_RefNo").ToString
                dtp_Date.Text = dt1.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_StockTransferParty.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transfer_To_PartyIdno").ToString), , TrnTo_DbName)

                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                txt_DespatchTo.Text = dt1.Rows(0).Item("Despatched_To").ToString

                lbl_JobCode.Text = dt1.Rows(0).Item("JobWork_Order_Code").ToString
                txt_JobNo.Text = dt1.Rows(0).Item("JobWork_Order_No").ToString
                lbl_JobDate.Text = dt1.Rows(0).Item("JobWork_Order_Date").ToString

                cbo_ClothName.Text = dt1.Rows(0).Item("Cloth_Name").ToString

                cbo_Transport.Text = dt1.Rows(0).Item("Transport_Name").ToString

                cbo_DeliveryTo.Text = dt1.Rows(0).Item("delivery_name").ToString
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString

                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString

                txt_rate.Text = dt1.Rows(0).Item("Rate").ToString

                lbl_total_amount.Text = dt1.Rows(0).Item("Total_Amount").ToString

                If IsDBNull(dt1.Rows(0).Item("Transfer_To_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("Transfer_To_Status").ToString) = 1 Then
                        chk_Transfer.Checked = True
                    End If
                End If
                If IsDBNull(dt1.Rows(0).Item("Yarn_Stock_Posting_for_Warp_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("Yarn_Stock_Posting_for_Warp_Status").ToString) = 1 Then
                        chk_Warp_Yarn_Stock_Posting.Checked = True
                    End If
                End If

                LockSTS = False
                If IsDBNull(dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If
                If IsDBNull(dt1.Rows(0).Item("JobWork_Bill_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("JobWork_Bill_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If
                If IsDBNull(dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                txt_EWayBillNo.Text = dt1.Rows(0).Item("Eway_BillNo").ToString


                'da2 = New SqlClient.SqlDataAdapter("select a.* , b.ClothType_Name from JobWork_Production_Head a LEFT OUTER JOIN b ON a.ClothType_Idno = b.ClothType_Idno  Where JobWork_Delivery_Code = '" & Trim(NewCode) & "' Order by JobWork_Production_Date, For_OrderBy, JobWork_Production_No", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                'dgv_Details.Rows.Clear()
                'SNo = 0

                'If dt2.Rows.Count > 0 Then

                '    For i = 0 To dt2.Rows.Count - 1

                '        n = dgv_Details.Rows.Add()

                '        SNo = SNo + 1
                '        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                '        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("JobWork_Production_No").ToString
                '        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                '        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Receipt_Meters").ToString), "########0.00")
                '        dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("JobWork_Production_Code").ToString
                '        dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Entry_PkCondition").ToString

                '    Next i

                'End If

                da3 = New SqlClient.SqlDataAdapter("select a.* , b.ClothType_Name from JobWork_Piece_Delivery_Details a LEFT OUTER JOIN ClothType_Head b ON a.ClothType_Idno = b.ClothType_Idno  Where JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by sl_no, For_OrderBy,JobWork_Piece_Delivery_Date,  JobWork_Piece_Delivery_No", con)
                dt3 = New DataTable
                da3.Fill(dt3)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt3.Rows.Count > 0 Then

                    For i = 0 To dt3.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt3.Rows(i).Item("Lot_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt3.Rows(i).Item("Pcs_No").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt3.Rows(i).Item("ClothType_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt3.Rows(i).Item("Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(5).Value = dt3.Rows(i).Item("Lot_Code").ToString
                        dgv_Details.Rows(n).Cells(6).Value = dt3.Rows(i).Item("Entry_PkCondition").ToString
                        dgv_Details.Rows(n).Cells(7).Value = dt3.Rows(i).Item("Folding").ToString
                        dgv_Details.Rows(n).Cells(8).Value = dt3.Rows(i).Item("Po_No").ToString
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt3.Rows(i).Item("Weight").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(10).Value = dt3.Rows(i).Item("Weaving_JobCode_forSelection").ToString

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Total_Rolls").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Delivery_Meters").ToString), "########0.00")
                End With

                If chk_Transfer.Visible = True Then

                    TrnTo_CmpGrpIdNo = Val(Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..CompanyGroup_Head", "Transfer_To_CompanyGroupIdNo", "(CompanyGroup_IdNo = " & Str(Val(Common_Procedures.CompGroupIdNo)) & ")"))
                    DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(TrnTo_CmpGrpIdNo)))

                    da2 = New SqlClient.SqlDataAdapter("Select * from " & Trim(DbName) & "..Weaver_ClothReceipt_Piece_Details Where Transfer_From_EntryCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '')", con)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        LockSTS = True
                    End If
                    dt2.Clear()

                End If

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

                cbo_ClothName.Enabled = False
                cbo_ClothName.BackColor = Color.LightGray

                dtp_Date.Enabled = False
                dtp_Date.BackColor = Color.LightGray

                msk_Date.Enabled = False
                msk_Date.BackColor = Color.LightGray

                btn_Selection.Enabled = True

            End If

            If dgv_Details.Columns(8).Visible = False Then
                Grid_Cell_DeSelect()
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim mskdtxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
            mskdtxbx = Me.ActiveControl
            mskdtxbx.SelectionStart = 0
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
                Prec_ActCtrl.BackColor = Color.FromArgb(2, 57, 111)
                Prec_ActCtrl.ForeColor = Color.White
            ElseIf TypeOf Prec_ActCtrl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
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

    Private Sub JobWork_PieceDelivery_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
    Private Sub JobWork_PieceDelivery_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim TrnTo_CmpGrpIdNo As Integer = 0

        Me.Text = ""
        dgv_Details.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))
        dgv_Selection.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))

        con.Open()

        chk_Transfer.Visible = False
        lbl_stocktransferParty.Visible = False
        cbo_StockTransferParty.Visible = False
        TrnTo_CmpGrpIdNo = Val(Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..CompanyGroup_Head", "Transfer_To_CompanyGroupIdNo", "(CompanyGroup_IdNo = " & Str(Val(Common_Procedures.CompGroupIdNo)) & ")"))
        If Val(TrnTo_CmpGrpIdNo) <> 0 Then
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(TrnTo_CmpGrpIdNo)))
            chk_Transfer.Visible = True

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1075" Then
                lbl_stocktransferParty.Visible = True
                cbo_StockTransferParty.Visible = True
            End If

        Else
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        End If


        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1075-" Then '---- JR TEX ( STANLEY ) ( MS FABRICS ) (SULUR)   (or)   J.R TEX ( STANLEY ) ( M.S FABRICS ) (SULUR)
            btn_SaveAll.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1574" Then '---- VASSA TEXTILE MILLS PRIVATE LIMITED (PERUNDURAI)
            btn_get_Weft_CountName_from_Master.Visible = True
        End If

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        lbl_JobCode.Visible = False

        pnl_Order_Selection.Visible = False
        pnl_Order_Selection.Left = (Me.Width - pnl_Order_Selection.Width) \ 2
        pnl_Order_Selection.Top = (Me.Height - pnl_Order_Selection.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.BringToFront()
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2

        pnl_Print2.Visible = False
        pnl_Print2.BringToFront()
        pnl_Print2.Left = (Me.Width - pnl_Print2.Width) \ 2
        pnl_Print2.Top = (Me.Height - pnl_Print2.Height) \ 2



        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("ORDER")

        chk_Warp_Yarn_Stock_Posting.Visible = False
        If Common_Procedures.settings.JobWorker_Yarn_to_Fabric_Conversion_Status = 1 Then
            chk_Warp_Yarn_Stock_Posting.Visible = True
        End If


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        txt_JobNo.Enabled = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            lbl_JobNo_Caption.Text = "PO No."
            txt_JobNo.Enabled = True
            txt_JobNo.Width = cbo_Type.Width

            lbl_JobDate.Visible = False
            lbl_JobDate_Caption.Visible = False

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            lbl_rate.Visible = True
            lbl_TotalAmount_caption.Visible = True
            txt_rate.Visible = True
            lbl_total_amount.Visible = True
            dgv_Details.Columns(8).Visible = True
            dgv_Details.Columns(3).Width = 50
            dgv_Details.Columns(4).Width = 50
            dgv_Details.Columns(8).Width = 70
            dgv_Details.ReadOnly = False
            dgv_Details.Columns(8).ReadOnly = True
            dgv_Details.EditMode = DataGridViewEditMode.EditOnEnter
            txt_rate.Enabled = False
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1420" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1547" Then
            dgv_Details.Columns(9).Visible = True
            dgv_Details.Columns(9).Width = 70
            dgv_Details.Columns(9).ReadOnly = True
        End If

        If Common_Procedures.settings.Show_Weaver_JobCard_Entry_STATUS = 1 Then
            dgv_Details.Columns(10).Visible = True
            dgv_Details.Columns(10).Width = 70
            dgv_Details.Columns(10).ReadOnly = True
            dgv_Details_Total.Columns(10).Visible = True

            dgv_Details.Columns(1).Width = 65
            dgv_Details.Columns(2).Width = 65
            dgv_Details.Columns(3).Width = 65
            dgv_Details.Columns(4).Width = 65
            dgv_Details.Columns(10).Width = 90

            dgv_Details_Total.Columns(1).Width = 65
            dgv_Details_Total.Columns(2).Width = 65
            dgv_Details_Total.Columns(3).Width = 65
            dgv_Details_Total.Columns(4).Width = 65
            dgv_Details_Total.Columns(10).Width = 90

        End If

        lbl_rate.Visible = True
        lbl_TotalAmount_caption.Visible = True
        txt_rate.Visible = True
        lbl_total_amount.Visible = True

        cbo_Piece_Delv_SufixNo.Items.Clear()
        cbo_Piece_Delv_SufixNo.Items.Add("")
        cbo_Piece_Delv_SufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_Piece_Delv_SufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))
        cbo_Piece_Delv_SufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate))
        cbo_Piece_Delv_SufixNo.Items.Add("/" & Trim(Year(Common_Procedures.Company_FromDate)) & "-" & Trim(Microsoft.VisualBasic.Right(Year(Common_Procedures.Company_ToDate), 2)))
        
        
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DespatchTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Transfer.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Warp_Yarn_Stock_Posting.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FilterPcsNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FilterRollNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_StockTransferParty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LotSelction.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsSelction.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_rate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_JobNo.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Piece_Delv_PrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Piece_Delv_SufixNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DespatchTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Transfer.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Warp_Yarn_Stock_Posting.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FilterPcsNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FilterRollNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_StockTransferParty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LotSelction.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsSelction.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_JobNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Piece_Delv_PrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Piece_Delv_SufixNo.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_DespatchTo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FilterRollNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_JobNo.KeyDown, AddressOf TextBoxControlKeyDown


        '  AddHandler txt_rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DespatchTo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FilterRollNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_JobNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_EWayBillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EWayBillNo.LostFocus, AddressOf ControlLostFocus





        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub JobWork_PieceDelivery_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub JobWork_PieceDelivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Order_Selection.Visible = True Then
                    btn_OrderSelection_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_Print_Cancel_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print2.Visible = True Then
                    btn_Print_Cancel_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

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
        Dim TrnTo_CmpGrpIdNo As Integer = 0
        Dim DbName As String = ""
        Dim TrnsfrSTS As Integer = 0

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Jobwork_Piece_Delivery_Entry, New_Entry, Me, con, "JobWork_Piece_Delivery_Head", "JobWork_Piece_Delivery_Code", NewCode, "JobWork_Piece_Delivery_Date", "(JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        TrnsfrSTS = 0

        Da = New SqlClient.SqlDataAdapter("select * from JobWork_Piece_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) <> "" And Trim(Dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) <> "0" Then
                    MessageBox.Show("Already inspection Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
            If IsDBNull(Dt1.Rows(0).Item("JobWork_Bill_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("JobWork_Bill_Code").ToString) <> "" Then
                    MessageBox.Show("Already Bil prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            If IsDBNull(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                    MessageBox.Show("Already Piece Delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            If IsDBNull(Dt1.Rows(0).Item("Transfer_To_Status").ToString) = False Then
                TrnsfrSTS = Val(Dt1.Rows(0).Item("Transfer_To_Status").ToString)
            End If

        End If
        Dt1.Clear()

        If TrnsfrSTS = 1 Then   'chk_Transfer.Visible = True 

            TrnTo_CmpGrpIdNo = Val(Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..CompanyGroup_Head", "Transfer_To_CompanyGroupIdNo", "(CompanyGroup_IdNo = " & Str(Val(Common_Procedures.CompGroupIdNo)) & ")"))
            DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(TrnTo_CmpGrpIdNo)))

            Da = New SqlClient.SqlDataAdapter("Select * from " & Trim(DbName) & "..Weaver_ClothReceipt_Piece_Details Where Transfer_From_EntryCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '')", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Already Piece Delivered/Bale Prepared for transfered piece", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
            Dt1.Clear()
        End If

        Dt1.Dispose()
        Da.Dispose()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "JobWork_Piece_Delivery_Head", "JobWork_Piece_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "JobWork_Piece_Delivery_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "JobWork_Piece_Delivery_Details", "JobWork_Piece_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Lot_No, Pcs_No, ClothType_IdNo, Meters ,  Lot_Code , Entry_PkCondition ", "Sl_No", "JobWork_Piece_Delivery_Code, For_OrderBy, Company_IdNo, JobWork_Piece_Delivery_No, JobWork_Piece_Delivery_Date, Ledger_Idno, Weight", trans)

            If TrnsfrSTS = 1 Then   'chk_Transfer.Visible = True 
                cmd.CommandText = "Delete from  " & Trim(DbName) & "..Weaver_ClothReceipt_Piece_Details Where Transfer_From_EntryCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = ''"
                cmd.ExecuteNonQuery()

                ''---'---remove after save all - by thanges
                ''---cmd.CommandText = "Delete from " & Trim(DbName) & "..Stock_Cloth_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                ''---cmd.ExecuteNonQuery()
                cmd.CommandText = "Delete from " & Trim(DbName) & "..Stock_Cloth_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "' and Transfer_From_EntryCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                ''---'---remove after save all - by thanges
                ''---cmd.CommandText = "Delete from " & Trim(DbName) & "..Stock_Pavu_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                ''---cmd.ExecuteNonQuery()
                cmd.CommandText = "Delete from " & Trim(DbName) & "..Stock_Pavu_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "' and Transfer_From_EntryCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                ''---'---remove after save all - by thanges
                ''---cmd.CommandText = "Delete from " & Trim(DbName) & "..Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                ''---cmd.ExecuteNonQuery()
                cmd.CommandText = "Delete from " & Trim(DbName) & "..Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "' and Transfer_From_EntryCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1 Where PackingSlip_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1 Where PackingSlip_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1 Where PackingSlip_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1 Where PackingSlip_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1 Where PackingSlip_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update JobWork_Production_Head set JobWork_Delivery_Code = '', JobWork_Delivery_Increment = JobWork_Delivery_Increment - 1, JobWork_Delivery_Date = Null Where JobWork_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition3) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition3) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from JobWork_Piece_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

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

            da = New SqlClient.SqlDataAdapter("select Cloth_name from Cloth_Head order by cloth_name", con)
            da.Fill(dt2)
            cbo_Filter_ClothName.DataSource = dt2
            cbo_Filter_ClothName.DisplayMember = "cloth_name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Piece_Delivery_RefNo from JobWork_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_Piece_Delivery_RefNo", con)
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
        Dim OrdByNo As String = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Piece_Delivery_RefNo from JobWork_Piece_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_Piece_Delivery_RefNo", con)
            dt = New DataTable
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
        Dim OrdByNo As String = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Piece_Delivery_RefNo from JobWork_Piece_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Piece_Delivery_RefNo desc", con)
            dt = New DataTable
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
            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Piece_Delivery_RefNo from JobWork_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Piece_Delivery_RefNo desc", con)
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
            If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then
                lbl_DcNo.Text = Common_Procedures.get_Cloth_JobWork_Delivery_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            Else
                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_Piece_Delivery_Head", "JobWork_Piece_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            End If

            lbl_DcNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from JobWork_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Piece_Delivery_RefNo desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString
                End If
                If dt1.Rows(0).Item("JobWork_Piece_Delivery_PrefixNo").ToString <> "" Then
                    txt_Piece_Delv_PrefixNo.Text = dt1.Rows(0).Item("JobWork_Piece_Delivery_PrefixNo").ToString
                End If


                If dt1.Rows(0).Item("jobwork_Piece_Delivery_SuffixNo").ToString <> "" Then
                    cbo_Piece_Delv_SufixNo.Text = dt1.Rows(0).Item("jobwork_Piece_Delivery_SuffixNo").ToString
                End If
                If chk_Warp_Yarn_Stock_Posting.Visible = True Then
                    If IsDBNull(dt1.Rows(0).Item("Yarn_Stock_Posting_for_Warp_Status").ToString) = False Then
                        If Val(dt1.Rows(0).Item("Yarn_Stock_Posting_for_Warp_Status").ToString) = 1 Then
                            chk_Warp_Yarn_Stock_Posting.Checked = True
                        End If
                    End If
                End If
            End If
            dt1.Clear()

            If chk_Transfer.Visible = True Then chk_Transfer.Checked = True

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
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
        Dim vCSMovNo As String
        Dim vCSInvCode As String = ""
        Dim vYSInvCode As String = ""
        Dim vYSMovNo As String = ""

        Try

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select JobWork_Piece_Delivery_RefNo from JobWork_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(RecCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            vCSMovNo = ""
            If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then

                vCSInvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select  ClothSales_Delivery_No from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and  ClothSales_Delivery_Code = '" & Trim(vCSInvCode) & "'  ", con)
                Dt = New DataTable
                Da.Fill(Dt)

                vCSMovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vCSMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()

            End If

            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(vCSMovNo) <> 0 Then
                MessageBox.Show("This Dc No. is in Cloth DC", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show("Invoice No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If
            'If Val(movno) <> 0 Then
            '    move_record(movno)

            'Else
            '    MessageBox.Show("Inv No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            'End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String
        Dim vCSMovNo As String
        Dim vCSInvCode As String = ""
        Dim vYSInvCode As String = ""
        Dim vYSMovNo As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Jobwork_Piece_Delivery_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW DC INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select JobWork_Piece_Delivery_RefNo from JobWork_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(RecCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            vCSMovNo = ""
            If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then

                vCSInvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select  ClothSales_Delivery_No from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and  ClothSales_Delivery_Code = '" & Trim(vCSInvCode) & "'  ", con)
                Dt = New DataTable
                Da.Fill(Dt)

                vCSMovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vCSMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()

            End If

            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(vCSMovNo) <> 0 Then
                MessageBox.Show("This Dc No. is in Cloth DC", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                new_record()
                Insert_Entry = True
                lbl_DcNo.Text = Trim(UCase(inpno))
            End If
            'If Val(movno) <> 0 Then
            '    move_record(movno)

            'Else
            '    MessageBox.Show("Inv No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            'End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_id As Integer = 0
        Dim Clo_id As Integer = 0
        Dim Trans_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim vTotRls As Integer, vTotMtrs As String
        Dim Nr As Long
        Dim dCloTyp_ID As Integer = 0
        Dim T1_Mtrs As Single = 0
        Dim T2_Mtrs As Single = 0
        Dim T3_Mtrs As Single = 0
        Dim T4_Mtrs As Single = 0
        Dim T5_Mtrs As Single = 0
        Dim Transto_STS As Integer = 0
        Dim Transtkparty_Id As Integer = 0
        Dim v1STPC_FOLDPERC As String = "", vFOLDPERC As String = ""
        Dim vLm_IdNo As Integer = 0
        Dim vWdth_Type As String = ""
        Dim vCrmp_Perc As String = ""
        Dim vEndsCnt_IdNo As Integer = 0
        Dim vPvuConsMtrs As Single = 0
        Dim vLed_type As String = ""
        Dim vDelv_ID As Integer = 0, vRec_ID As Integer = 0
        Dim vWftCnt_ID As Integer = 0
        Dim vConsYarn As Single = 0
        Dim OpYrCode As String = ""
        Dim OpDate As Date
        Dim OpSTS As Boolean = False
        Dim vTotOpMtrs As String = ""
        Dim vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption As Boolean
        Dim vOrdByNo As String = ""
        Dim Delivery_ID As Integer = 0
        Dim vWARPYARN_STOCK_POSTING_STS As Integer
        Dim vWARPCOUNT_ID As Integer = 0
        Dim vCONSYARN_FORWARP As String = 0
        Dim Led_type As String
        Dim stkof_idno As Integer
        Dim Delv_ID As Integer, Rec_ID As Integer
        Dim vInvNo As String = ""
        Dim lckdt As Date = Now
        Dim dat As Date = Now

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(lbl_DcNo.Text) = "" Then
            MessageBox.Show("Invalid Dc.No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.JobWork_Delivery_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Jobwork_Piece_Delivery_Entry, New_Entry, Me, con, "JobWork_Piece_Delivery_Head", "JobWork_Piece_Delivery_Code", NewCode, "JobWork_Piece_Delivery_Date", "(JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, JobWork_Piece_Delivery_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1075" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1377-" Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1377-" Then '---- KURINJHI WEAVING MILLS (PALLADAM) 
                lckdt = #10/10/2022#
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1075" Then '---- JR Textiles (Somanur) Stantly
                lckdt = #1/1/2026#
            End If

            If IsDate(Common_Procedures.settings.Sdd) = True Then
                dat = Common_Procedures.settings.Sdd
            End If

            If DateDiff("d", lckdt.ToShortDateString, dat.ToShortDateString) > 0 Then
                MessageBox.Show("Run-time error '6': " & Chr(13) & Chr(13) & "Overflow", "DOES Not SAVE", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                Me.Close()
                Application.Exit()
            End If

        End If

        Led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Delivery_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        Clo_id = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        If Clo_id = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled And cbo_ClothName.Visible Then cbo_ClothName.Focus()
            Exit Sub
        End If

        Transtkparty_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_StockTransferParty.Text, , TrnTo_DbName)
        If chk_Transfer.Visible And cbo_StockTransferParty.Visible Then
            If chk_Transfer.Checked = True Then
                If Val(Transtkparty_Id) = 0 Then
                    MessageBox.Show("Invalid Transfer Stock To", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_StockTransferParty.Enabled Then cbo_StockTransferParty.Focus()
                    Exit Sub
                End If
            End If
        End If

        Trans_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        vWARPYARN_STOCK_POSTING_STS = 0
        If chk_Warp_Yarn_Stock_Posting.Visible = True Then
            If chk_Warp_Yarn_Stock_Posting.Checked = True Then
                vWARPYARN_STOCK_POSTING_STS = 1
            End If
        End If


        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                If Trim(dgv_Details.Rows(i).Cells(1).Value) = "" Then
                    MessageBox.Show("Invalid " & StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If Trim(dgv_Details.Rows(i).Cells(5).Value) = "" Then
                    MessageBox.Show("Invalid " & StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then dgv_Details.Focus()
                    Exit Sub
                End If

            End If

        Next

        Total_Calculation()

        vTotRls = 0 : vTotMtrs = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotRls = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
            vTotMtrs = Format(Val(dgv_Details_Total.Rows(0).Cells(4).Value), "##########0.00")
        End If



        Transto_STS = 0
        If chk_Transfer.Checked = True Then
            Transto_STS = 1
        End If



        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
        OpDate = DateAdd(DateInterval.Day, -1, OpDate)

        'Dim EWBCancel As String = "0"
        'If txt_EWB_Cancel_Status.Text = "Cancelled" Then
        '    eiCancel = "1"
        'End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else
                If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then
                    lbl_DcNo.Text = Common_Procedures.get_Cloth_JobWork_Delivery_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                Else
                    lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_Piece_Delivery_Head", "JobWork_Piece_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                End If

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            vInvNo = Trim(txt_Piece_Delv_PrefixNo.Text) & Trim(lbl_DcNo.Text) & Trim(cbo_Piece_Delv_SufixNo.Text)
            If Trim(lbl_DcNo.Text) = "" Then
                Throw New ApplicationException("Invalid Dc.No")
                Exit Sub
            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DcDate", Convert.ToDateTime(msk_Date.Text))
            cmd.Parameters.AddWithValue("@OPDate", OpDate)

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

            If New_Entry = True Then

                cmd.CommandText = "Insert into JobWork_Piece_Delivery_Head ( JobWork_Piece_Delivery_Code,               Company_IdNo       , JobWork_Piece_Delivery_No ,    JobWork_Piece_Delivery_RefNo ,           for_OrderBy     , JobWork_Piece_Delivery_Date,          Ledger_IdNo    ,      JobWork_Order_Code         ,       JobWork_Order_No        ,    JobWork_Order_Date           ,      Cloth_Idno     ,            Despatched_To           ,     Transport_IdNo        ,               Vehicle_No              ,               Remarks           ,            Total_Rolls   ,     Total_Delivery_Meters , Total_Actual_Meters, Total_ClothType1_Meters, Total_ClothType2_Meters, Total_ClothType3_Meters, Total_ClothType4_Meters, Total_ClothType5_Meters, Total_Checking_Meters, JobWork_Inspection_Code, JobWork_Inspection_Date, JobWork_Inspection_Increment, JobWork_Bill_Code, JobWork_Bill_Date, JobWork_Bill_Increment  ,         Selection_Type       ,          Transfer_To_Status  ,        Transfer_To_PartyIdno  ,              Rate,                               Total_Amount,               Delivery_Idno,                       Yarn_Stock_Posting_for_Warp_Status       ,               Eway_BillNo          ,             JobWork_Piece_Delivery_PrefixNo  ,  jobwork_Piece_Delivery_SuffixNo             ) " &
                                    " Values                               ('" & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(vInvNo) & "'   ,    '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",       @DcDate              , " & Str(Val(Led_id)) & ", '" & Trim(lbl_JobCode.Text) & "', '" & Trim(txt_JobNo.Text) & "', '" & Trim(lbl_JobDate.Text) & "', " & Str(Val(Clo_id)) & ", '" & Trim(txt_DespatchTo.Text) & "', " & Str(Val(Trans_id)) & ", '" & Trim(cbo_VehicleNo.Text) & "', '" & Trim(txt_Remarks.Text) & "', " & Str(Val(vTotRls)) & ", " & Str(Val(vTotMtrs)) & ",         0          ,         0              ,             0          ,            0           ,            0           ,             0          ,         0            ,         ''             ,        Null            ,          0                  ,          ''      ,        Null      ,          0              , '" & Trim(cbo_Type.Text) & "', " & Str(Val(Transto_STS)) & " , " & Str(Val(Transtkparty_Id)) & "," & Str(Val(txt_rate.Text)) & "," & Str(Val(lbl_total_amount.Text)) & " ," & Str(Val(Delivery_ID)) & ", " & Str(Val(vWARPYARN_STOCK_POSTING_STS)) & "   , '" & Trim(txt_EWayBillNo.Text) & "', '" & Trim(txt_Piece_Delv_PrefixNo.Text) & "' ,  '" & Trim(cbo_Piece_Delv_SufixNo.Text) & "' ) "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "JobWork_Piece_Delivery_Head", "JobWork_Piece_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "JobWork_Piece_Delivery_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "JobWork_Piece_Delivery_Details", "JobWork_Piece_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No, Pcs_No, ClothType_IdNo, Meters ,  Lot_Code , Entry_PkCondition ", "Sl_No", "JobWork_Piece_Delivery_Code, For_OrderBy, Company_IdNo, JobWork_Piece_Delivery_No, JobWork_Piece_Delivery_Date, Ledger_Idno, Weight", tr)

                If SaveAll_STS <> True And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1192" Then
                    Da = New SqlClient.SqlDataAdapter("select * from JobWork_Piece_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
                    Da.SelectCommand.Transaction = tr
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        If IsDBNull(Dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) = False Then
                            If Trim(Dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) <> "" And Trim(Dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) <> "0" Then
                                Throw New ApplicationException("Already inspection Prepared")
                                Exit Sub
                            End If
                        End If
                        If IsDBNull(Dt1.Rows(0).Item("JobWork_Bill_Code").ToString) = False Then
                            If Trim(Dt1.Rows(0).Item("JobWork_Bill_Code").ToString) <> "" Then
                                Throw New ApplicationException("Already Bil prepared")
                                Exit Sub
                            End If
                        End If
                        If IsDBNull(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                            If Trim(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                                Throw New ApplicationException("Already GatePass Prepared")
                                Exit Sub
                            End If
                        End If
                    End If
                    Dt1.Clear()
                End If

                cmd.CommandText = "Update JobWork_Piece_Delivery_Head set JobWork_Piece_Delivery_Date = @DcDate, Ledger_IdNo = " & Str(Val(Led_id)) & ", Cloth_Idno = " & Str(Val(Clo_id)) & ",Transfer_To_PartyIdno =  " & Str(Val(Transtkparty_Id)) & " ,  Despatched_To = '" & Trim(txt_DespatchTo.Text) & "', Transport_idno = " & Str(Val(Trans_id)) & ", JobWork_Order_Code = '" & Trim(lbl_JobCode.Text) & "', JobWork_Order_No = '" & Trim(txt_JobNo.Text) & "', JobWork_Order_Date = '" & Trim(lbl_JobDate.Text) & "', Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "', Remarks = '" & Trim(txt_Remarks.Text) & "', Total_Rolls = " & Str(Val(vTotRls)) & ", Total_Delivery_Meters = " & Str(Val(vTotMtrs)) & " , Selection_Type = '" & Trim(cbo_Type.Text) & "', Transfer_To_Status  = " & Str(Val(Transto_STS)) & " ,Rate=" & Str(Val(txt_rate.Text)) & ",Total_amount=" & Str(Val(lbl_total_amount.Text)) & ",Delivery_Idno=" & Str(Val(Delivery_ID)) & " , Yarn_Stock_Posting_for_Warp_Status = " & Str(Val(vWARPYARN_STOCK_POSTING_STS)) & " ,Eway_BillNo =  '" & Trim(txt_EWayBillNo.Text) & "',JobWork_Piece_Delivery_PrefixNo = '" & Trim(txt_Piece_Delv_PrefixNo.Text) & "', jobwork_Piece_Delivery_SuffixNo = '" & Trim(cbo_Piece_Delv_SufixNo.Text) & "' , JobWork_Piece_Delivery_No = '" & Trim(vInvNo) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update JobWork_Production_Head set JobWork_Delivery_Code = '', JobWork_Delivery_Increment = JobWork_Delivery_Increment - 1, JobWork_Delivery_Date = Null Where JobWork_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1 Where PackingSlip_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1 Where PackingSlip_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1 Where PackingSlip_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1 Where PackingSlip_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1 Where PackingSlip_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "JobWork_Piece_Delivery_Head", "JobWork_Piece_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "JobWork_Piece_Delivery_Code, Company_IdNo, for_OrderBy", tr)

            If Trim(lbl_DcNo.Text) = "3" Then
                Debug.Print(Trim(lbl_DcNo.Text))
            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)
            Partcls = "JbWrk Pcs Delv : Dc.No. " & Trim(lbl_DcNo.Text) & ", Meters : " & Format(Val(vTotMtrs), "##########0.00")


            cmd.CommandText = "Delete from JobWork_Piece_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' and (JobWork_Inspection_Code = '' or JobWork_Inspection_Code = '0')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition3) & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition3) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSubTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSimpleTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            T1_Mtrs = 0
            T2_Mtrs = 0
            T3_Mtrs = 0
            T4_Mtrs = 0
            T5_Mtrs = 0

            vTotOpMtrs = 0
            v1STPC_FOLDPERC = 0
            vFOLDPERC = 100
            Sno = 0
            For i = 0 To dgv_Details.RowCount - 1

                With dgv_Details

                    If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 And Trim(dgv_Details.Rows(i).Cells(5).Value) <> "" Then

                        dCloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        If Val(dCloTyp_ID) = 5 Then
                            T5_Mtrs = T5_Mtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                        ElseIf Val(dCloTyp_ID) = 4 Then
                            T4_Mtrs = T4_Mtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                        ElseIf Val(dCloTyp_ID) = 3 Then
                            T3_Mtrs = T3_Mtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                        ElseIf Val(dCloTyp_ID) = 2 Then
                            T2_Mtrs = T2_Mtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                        Else
                            T1_Mtrs = T1_Mtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                        End If

                        vFOLDPERC = Val(dgv_Details.Rows(i).Cells(7).Value)
                        If Val(vFOLDPERC) = 0 Then vFOLDPERC = 100

                        If v1STPC_FOLDPERC = 0 Then v1STPC_FOLDPERC = vFOLDPERC

                        Sno = Sno + 1

                        Nr = 0
                        cmd.CommandText = "Update JobWork_Piece_Delivery_Details set JobWork_Piece_Delivery_Date = @DcDate, Ledger_IdNo = " & Str(Val(Led_id)) & ", Sl_No = " & Str(Val(Sno)) & ", Lot_No = '" & Trim(.Rows(i).Cells(1).Value) & "', ClothType_IdNo = " & Str(Val(dCloTyp_ID)) & ", Meters = " & Str(Val(.Rows(i).Cells(4).Value)) & " , Folding = " & Str(Val(vFOLDPERC)) & " , Weight = " & Str(Val(.Rows(i).Cells(9).Value)) & " , Weaving_JobCode_forSelection = '" & Trim(.Rows(i).Cells(10).Value) & "' where JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Entry_PkCondition = '" & Trim(.Rows(i).Cells(6).Value) & "' and Lot_Code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Pcs_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                        Nr = cmd.ExecuteNonQuery
                        If Nr = 0 Then
                            cmd.CommandText = "Insert into JobWork_Piece_Delivery_Details (  JobWork_Piece_Delivery_Code,               Company_IdNo       ,   JobWork_Piece_Delivery_No  ,          for_OrderBy      , JobWork_Piece_Delivery_Date,             Ledger_IdNo ,            Sl_No      ,                     Lot_No              ,                    Pcs_No              ,           ClothType_IdNo    ,                      Meters              ,                    Lot_Code            ,                    Entry_PkCondition    ,              Folding        ,                     Po_No              ,                  Weight                   ,         Weaving_JobCode_forSelection     ) " &
                                                "           Values                        (     '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",        @DcDate             , " & Str(Val(Led_id)) & ",  " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(dCloTyp_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", '" & Trim(.Rows(i).Cells(5).Value) & "', '" & Trim(.Rows(i).Cells(6).Value) & "' , " & Str(Val(vFOLDPERC)) & " , '" & Trim(.Rows(i).Cells(8).Value) & "', " & Str(Val(.Rows(i).Cells(9).Value)) & ", '" & Trim(.Rows(i).Cells(10).Value) & "' ) "
                            cmd.ExecuteNonQuery()
                        End If

                        If Trim(UCase(dgv_Details.Rows(i).Cells(6).Value)) = "JPROD-" Then

                            Nr = 0
                            cmd.CommandText = "Update JobWork_Production_Head set JobWork_Delivery_Code = '" & Trim(NewCode) & "', JobWork_Delivery_Increment = JobWork_Delivery_Increment + 1, JobWork_Delivery_Date = @DcDate Where JobWork_Production_Code = '" & Trim(dgv_Details.Rows(i).Cells(5).Value) & "' and Ledger_IdNo = " & Str(Val(Led_id))
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                MessageBox.Show("Invalid Roll Details - Mismatch of details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                tr.Rollback()
                                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                                Exit Sub
                            End If

                        Else

                            If dCloTyp_ID = 1 Then
                                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                                cmd.ExecuteNonQuery()

                            ElseIf dCloTyp_ID = 2 Then
                                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                                cmd.ExecuteNonQuery()

                            ElseIf dCloTyp_ID = 3 Then
                                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                                cmd.ExecuteNonQuery()

                            ElseIf dCloTyp_ID = 4 Then
                                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                                cmd.ExecuteNonQuery()

                            ElseIf dCloTyp_ID = 5 Then
                                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                                cmd.ExecuteNonQuery()

                            End If

                        End If


                        OpSTS = False
                        If Common_Procedures.settings.JobWorker_Pavu_Yarn_Stock_Posting_IN_Production = 0 Then

                            vLm_IdNo = 0
                            vWdth_Type = ""
                            vCrmp_Perc = ""
                            vEndsCnt_IdNo = 0

                            If Trim(UCase(dgv_Details.Rows(i).Cells(6).Value)) = "JPROD-" Then

                                Da1 = New SqlClient.SqlDataAdapter("Select a.Loom_IdNo, a.Width_Type, a.EndsCount_IdNo, a.Folding_Percentage from JobWork_Production_Head a Where a.JobWork_Production_Code = '" & Trim(dgv_Details.Rows(i).Cells(5).Value) & "'", con)
                                Da1.SelectCommand.Transaction = tr
                                Dt1 = New DataTable
                                Da1.Fill(Dt1)
                                If Dt1.Rows.Count > 0 Then
                                    vLm_IdNo = Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)
                                    vWdth_Type = Dt1.Rows(0).Item("Width_Type").ToString
                                    vCrmp_Perc = ""
                                    vEndsCnt_IdNo = Val(Dt1.Rows(0).Item("EndsCount_IdNo").ToString)
                                End If
                                Dt1.Clear()


                            Else

                                Da1 = New SqlClient.SqlDataAdapter("Select Folding, EndsCount_IdNo, Loom_IdNo, Width_Type, Crimp_Percentage  from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(dgv_Details.Rows(i).Cells(5).Value) & "'", con)
                                Da1.SelectCommand.Transaction = tr
                                Dt1 = New DataTable
                                Da1.Fill(Dt1)

                                If Dt1.Rows.Count > 0 Then

                                    vLm_IdNo = Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)
                                    vWdth_Type = Dt1.Rows(0).Item("Width_Type").ToString
                                    vCrmp_Perc = Val(Dt1.Rows(0).Item("Crimp_Percentage").ToString)
                                    vEndsCnt_IdNo = Val(Dt1.Rows(0).Item("EndsCount_IdNo").ToString)

                                Else

                                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1192" Then '---- SURABHI TEXTILES (PALLADAM)

                                        If Microsoft.VisualBasic.Right(Trim(dgv_Details.Rows(i).Cells(5).Value), 5) = Trim(OpYrCode) Then

                                            Da1 = New SqlClient.SqlDataAdapter("Select Width_Type, Ends_CountIdNo, Folding, NoOf_Beam_InLoom from Piece_Opening_Head Where Piece_Opening_Code = '" & Trim(dgv_Details.Rows(i).Cells(5).Value) & "'", con)
                                            Da1.SelectCommand.Transaction = tr
                                            Dt1 = New DataTable
                                            Da1.Fill(Dt1)
                                            If Dt1.Rows.Count > 0 Then
                                                OpSTS = True
                                                vLm_IdNo = Common_Procedures.get_FieldValue(con, "Loom_Head", "Loom_IdNo", "(Noof_Input_Beams = " & Str(Val(Dt1.Rows(0).Item("NoOf_Beam_InLoom").ToString)) & ")", , tr)
                                                vWdth_Type = Dt1.Rows(0).Item("Width_Type").ToString
                                                vCrmp_Perc = ""
                                                vEndsCnt_IdNo = Val(Dt1.Rows(0).Item("Ends_CountIdNo").ToString)
                                            End If

                                        End If

                                    End If

                                End If
                                Dt1.Clear()

                            End If


                            If vEndsCnt_IdNo <> 0 Then

                                vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption = True
                                If Common_Procedures.settings.JobWorker_PavuWidthWiseConsumption_IN_Delivery = 1 Then
                                    vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption = False
                                End If
                                If vWARPYARN_STOCK_POSTING_STS = 1 Then
                                    vPvuConsMtrs = Val(dgv_Details.Rows(i).Cells(4).Value)
                                Else
                                    vPvuConsMtrs = Common_Procedures.get_Pavu_Consumption(con, Clo_id, vLm_IdNo, Val(dgv_Details.Rows(i).Cells(4).Value), vWdth_Type, tr, Trim(vCrmp_Perc), , vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption, Val(vFOLDPERC))
                                End If

                                If vPvuConsMtrs <> 0 Then

                                    cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, meters1) Values ( " & Str(Val(vEndsCnt_IdNo)) & ", " & Str(Val(vPvuConsMtrs)) & ")"
                                    cmd.ExecuteNonQuery()

                                    If OpSTS = True Then
                                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSimpleTable) & "(Int1, meters1) Values ( " & Str(Val(vEndsCnt_IdNo)) & ", " & Str(Val(vPvuConsMtrs)) & ")"
                                        cmd.ExecuteNonQuery()
                                        vTotOpMtrs = Val(vTotOpMtrs) + Val(dgv_Details.Rows(i).Cells(4).Value)
                                    End If

                                End If

                            End If

                        End If

                    End If

                End With

            Next i
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "JobWork_Piece_Delivery_Details", "JobWork_Piece_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No, Pcs_No, ClothType_IdNo, Meters ,  Lot_Code , Entry_PkCondition ", "Sl_No", "JobWork_Piece_Delivery_Code, For_OrderBy, Company_IdNo, JobWork_Piece_Delivery_No, JobWork_Piece_Delivery_Date, Ledger_Idno, Weight", tr)

            '---UPDATING folding 5 of 1st detailks to header.
            cmd.CommandText = "Update JobWork_Piece_Delivery_Head set Folding = " & Str(Val(v1STPC_FOLDPERC)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Led_id)) & ")",, tr)

            stkof_idno = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                stkof_idno = Led_id
            Else
                stkof_idno = Val(Common_Procedures.CommonLedger.OwnSort_Ac)
            End If

            Delv_ID = 0 : Rec_ID = 0
            If Val(Led_id) = Val(Common_Procedures.CommonLedger.Godown_Ac) Then
                Rec_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                Delv_ID = 0

            Else
                Rec_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                Delv_ID = Val(Led_id)

            End If

            If T1_Mtrs <> 0 Or T2_Mtrs <> 0 Or T3_Mtrs <> 0 Or T4_Mtrs <> 0 Or T5_Mtrs <> 0 Then

                cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No       ,          for_OrderBy      , Reference_Date,         StockOff_IdNo       ,     DeliveryTo_Idno      ,      ReceivedFrom_Idno  ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No      ,           Cloth_Idno    ,                Folding           ,          Meters_Type1    ,         Meters_Type2     ,         Meters_Type3     ,         Meters_Type4     ,         Meters_Type5      ) " &
                                        " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",     @DcDate   , " & Str(Val(stkof_idno)) & ", " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Clo_id)) & ", " & Str(Val(v1STPC_FOLDPERC)) & ", " & Str(Val(T1_Mtrs)) & ", " & Str(Val(T2_Mtrs)) & ", " & Str(Val(T3_Mtrs)) & ", " & Str(Val(T4_Mtrs)) & ", " & Str(Val(T5_Mtrs)) & " ) "
                cmd.ExecuteNonQuery()

            End If

            If Common_Procedures.settings.JobWorker_Pavu_Yarn_Stock_Posting_IN_Production = 0 Then

                vLed_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Led_id)) & ")", , tr)

                vDelv_ID = 0 : vRec_ID = 0
                If Trim(UCase(vLed_type)) = "JOBWORKER" Then
                    vDelv_ID = Led_id
                    vRec_ID = 0
                Else
                    vDelv_ID = 0
                    vRec_ID = Led_id
                End If

                vWftCnt_ID = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_IdNo = " & Str(Val(Clo_id)) & ")", , tr)
                vConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, Clo_id, Val(vTotMtrs), tr,, Val(v1STPC_FOLDPERC))

                If Val(vConsYarn) <> 0 Then

                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (          Reference_Code                    ,                Company_IdNo      ,             Reference_No     ,           for_OrderBy     , Reference_Date,        DeliveryTo_Idno    ,       ReceivedFrom_Idno  ,           Entry_ID   ,           Particulars  ,      Party_Bill_No   , Sl_No,          Count_IdNo         , Yarn_Type, Mill_IdNo, Bags, Cones,                    Weight                             ) " &
                                      "                                  Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @DcDate ,  " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "',   1  , " & Str(Val(vWftCnt_ID)) & ",    'MILL',     0    ,   0 ,    0 , " & Str(Format(Val(vConsYarn), "#########0.000")) & " ) "
                    cmd.ExecuteNonQuery()

                End If



                If Val(vTotOpMtrs) <> 0 Then
                    vWftCnt_ID = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_IdNo = " & Str(Val(Clo_id)) & ")", , tr)
                    vConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, Clo_id, Val(vTotOpMtrs), tr,, Val(v1STPC_FOLDPERC))

                    If Val(vConsYarn) <> 0 Then

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (          Reference_Code                     ,                Company_IdNo      ,             Reference_No     ,           for_OrderBy     , Reference_Date,        DeliveryTo_Idno    ,       ReceivedFrom_Idno  ,           Entry_ID   ,           Particulars  ,      Party_Bill_No   , Sl_No ,          Count_IdNo         , Yarn_Type, Mill_IdNo, Bags, Cones,                    Weight                             ) " &
                                          "                                  Values  ('" & Trim(Pk_Condition3) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @OPDate ,  " & Str(Val(vRec_ID)) & ", " & Str(Val(vDelv_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "',   1001  , " & Str(Val(vWftCnt_ID)) & ",    'MILL',     0    ,   0 ,    0 , " & Str(Format(Val(vConsYarn), "#########0.000")) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                End If

                If vWARPYARN_STOCK_POSTING_STS = 1 Then

                    vWARPCOUNT_ID = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WarpCount_IdNo", "(Cloth_IdNo = " & Str(Val(Clo_id)) & ")", , tr)
                    vCONSYARN_FORWARP = Common_Procedures.get_Warp_ConsumedYarn(con, Clo_id, Val(vTotMtrs), tr, Val(v1STPC_FOLDPERC))

                    If Val(vCONSYARN_FORWARP) <> 0 Then
                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (          Reference_Code                    ,                Company_IdNo      ,             Reference_No     ,           for_OrderBy     , Reference_Date,        DeliveryTo_Idno     ,       ReceivedFrom_Idno  ,           Entry_ID   ,           Particulars  ,      Party_Bill_No   , Sl_No ,          Count_IdNo            , Yarn_Type, Mill_IdNo, Bags, Cones,                    Weight                                     ) " &
                                  "                                  Values          ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @DcDate    ,  " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "',   501 , " & Str(Val(vWARPCOUNT_ID)) & ",    'MILL',     0    ,   0 ,    0 , " & Str(Format(Val(vCONSYARN_FORWARP), "#########0.000")) & " ) "
                        cmd.ExecuteNonQuery()
                    End If


                Else

                    Sno = 100
                    Da1 = New SqlClient.SqlDataAdapter("select Int1 as Endscount_IdNo, sum(meters1) as PavuMtrs from " & Trim(Common_Procedures.EntryTempSubTable) & " group by Int1 having sum(meters1) <> 0", con)
                    Da1.SelectCommand.Transaction = tr
                    Dt1 = New DataTable
                    Da1.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        For i = 0 To Dt1.Rows.Count - 1

                            Sno = Sno + 1

                            '      If vWARPYARN_STOCK_POSTING_STS = 1 Then

                            '          vWARPCOUNT_ID = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("Endscount_IdNo").ToString)) & ")", , tr)
                            '          vCONSYARN_FORWARP = Common_Procedures.get_Warp_ConsumedYarn(con, Clo_id, Val(Dt1.Rows(i).Item("PavuMtrs").ToString), tr)

                            '          If Val(vCONSYARN_FORWARP) <> 0 Then

                            '              cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (          Reference_Code                    ,                Company_IdNo      ,             Reference_No     ,           for_OrderBy     , Reference_Date,        DeliveryTo_Idno     ,       ReceivedFrom_Idno  ,           Entry_ID   ,           Particulars  ,      Party_Bill_No   ,          Sl_No       ,          Count_IdNo            , Yarn_Type, Mill_IdNo, Bags, Cones,                    Weight                                     ) " &
                            '                        "                                  Values          ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @DcDate    ,  " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(vWARPCOUNT_ID)) & ",    'MILL',     0    ,   0 ,    0 , " & Str(Format(Val(vCONSYARN_FORWARP), "#########0.000")) & " ) "
                            '              cmd.ExecuteNonQuery()

                            '          End If

                            '      Else

                            cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (          Reference_Code                    ,                 Company_IdNo     ,            Reference_No      ,           for_OrderBy     , Reference_Date,       DeliveryTo_Idno      ,       ReceivedFrom_Idno  ,        Cloth_Idno       ,       Entry_ID    ,      Party_Bill_No   ,          Particulars   ,           Sl_No      ,                         EndsCount_IdNo                       , Sized_Beam,                         Meters                          ) " &
                            "                                         Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @DcDate ,  " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", " & Str(Val(Clo_id)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("Endscount_IdNo").ToString)) & ",      0    , " & Str(Val(Dt1.Rows(i).Item("PavuMtrs").ToString)) & " ) "
                            cmd.ExecuteNonQuery()


                            '      End If

                        Next

                    End If

                    Dt1.Clear()

                    Sno = 2000
                    Da1 = New SqlClient.SqlDataAdapter("select Int1 as Endscount_IdNo, sum(meters1) as PavuMtrs from " & Trim(Common_Procedures.EntryTempSimpleTable) & " group by Int1 having sum(meters1) <> 0", con)
                    Da1.SelectCommand.Transaction = tr
                    Dt1 = New DataTable
                    Da1.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        For i = 0 To Dt1.Rows.Count - 1

                            Sno = Sno + 1

                            cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (          Reference_Code                     ,                 Company_IdNo     ,            Reference_No      ,           for_OrderBy     , Reference_Date,       DeliveryTo_Idno      ,       ReceivedFrom_Idno  ,        Cloth_Idno       ,          Entry_ID                                  ,      Party_Bill_No    ,          Particulars   ,           Sl_No      ,                         EndsCount_IdNo                       , Sized_Beam,                         Meters                          ) " &
                                              "                                  Values  ('" & Trim(Pk_Condition3) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @OPDate    ,  " & Str(Val(vRec_ID)) & ", " & Str(Val(vDelv_ID)) & ", " & Str(Val(Clo_id)) & ", '" & Trim(Pk_Condition3) & Trim(lbl_DcNo.Text) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("Endscount_IdNo").ToString)) & ",      0    , " & Str(Val(Dt1.Rows(i).Item("PavuMtrs").ToString)) & " ) "
                            cmd.ExecuteNonQuery()

                        Next
                    End If
                    Dt1.Clear()

                End If


            End If

            If chk_Transfer.Visible = True Then
                Transfer_Entry_To_Another_Company(tr)
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                    If New_Entry = True Then
                        new_record()
                    Else
                        move_record(lbl_DcNo.Text)
                    End If
                Else
                    move_record(lbl_DcNo.Text)
                End If
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()

            cmd.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_Date, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Ledger, "", "", "", "")
    End Sub
    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER' or ( Ledger_Type = 'WEAVER' and Own_Loom_Status = 1 ) )  and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_Type, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER' or ( Ledger_Type = 'WEAVER' and Own_Loom_Status = 1 ) )  and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If txt_JobNo.Enabled And txt_JobNo.Visible Then
                txt_JobNo.Focus()
            Else
                cbo_ClothName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER' or ( Ledger_Type = 'WEAVER' and Own_Loom_Status = 1 ) )  and Close_Status = 0 )", "(Ledger_IdNo = 0)")

        With cbo_Ledger

            If Asc(e.KeyChar) = 13 Then
                If Trim(cbo_Type.Text <> "ORDER") Then

                    If txt_JobNo.Enabled And txt_JobNo.Visible Then
                        txt_JobNo.Focus()
                    Else
                        cbo_ClothName.Focus()
                    End If


                Else
                    If MessageBox.Show("Do you want to select Order :", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        btn_Order_Selection_Click(sender, e)

                    Else
                        If txt_JobNo.Enabled And txt_JobNo.Visible Then
                            txt_JobNo.Focus()
                        Else
                            cbo_ClothName.Focus()
                        End If

                    End If
                End If
            End If
        End With

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
        Get_vehicle_from_Transport()

    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_DeliveryTo, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()

    End Sub

    Private Sub cbo_vehicleno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "JobWork_Piece_Delivery_Head", "Vehicle_No", "", "Vehicle_No")

    End Sub
    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, Nothing, "JobWork_Piece_Delivery_Head", "Vehicle_No", "", "Vehicle_No")

        If (e.KeyCode = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            cbo_Transport.Focus()

        End If

        If (e.KeyCode = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If txt_rate.Enabled = True Then
                txt_rate.Focus()
            Else
                txt_Remarks.Focus()


            End If
        End If

    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, Nothing, "JobWork_Piece_Delivery_Head", "Vehicle_No", "", "", False)
        If Asc(e.KeyChar) = 13 Then
            If txt_rate.Enabled = True Then
                txt_rate.Focus()
            Else
                txt_Remarks.Focus()
            End If
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
        Dim n As Integer
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobWork_Piece_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.JobWork_Piece_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobWork_Piece_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If



            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.JobWork_Piece_Delivery_Code IN (select z1.JobWork_Piece_Delivery_Code from JobWork_Production_Head z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Trim(txt_FilterRollNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.JobWork_Piece_Delivery_Code IN (select z.JobWork_Piece_Delivery_Code from JobWork_Piece_Delivery_Details z where z.Lot_No = '" & Trim(txt_FilterRollNo.Text) & "') "
            End If

            If Trim(txt_FilterPcsNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.JobWork_Piece_Delivery_Code IN (select z.JobWork_Piece_Delivery_Code from JobWork_Piece_Delivery_Details z where z.Pcs_No = '" & Trim(txt_FilterPcsNo.Text) & "') "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , C.* , d.Cloth_Name from JobWork_Piece_Delivery_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN JobWork_Piece_Delivery_Details C ON a.JobWork_Piece_Delivery_Code = c.JobWork_Piece_Delivery_Code LEFT OUTER JOIN Cloth_Head D ON a.Cloth_IdNo = d.Cloth_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Piece_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.JobWork_Piece_Delivery_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("JobWork_Piece_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = (dt2.Rows(i).Item("Cloth_Name").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = (dt2.Rows(i).Item("Lot_No").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = (dt2.Rows(i).Item("Pcs_No").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or  Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, txt_FilterRollNo, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, txt_FilterRollNo, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

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


    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        With dgv_Details
            If .CurrentCell.RowIndex = .RowCount - 1 Then
                cbo_Transport.Focus()
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next


        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Then
                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        On Error Resume Next

        With dgv_Details

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True
                    txt_DespatchTo.Focus()
                End If
            End If

            If e.KeyCode = Keys.Down Then
                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True
                    cbo_Transport.Focus()
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 Then
                    cbo_Transport.Focus()

                Else
                    SendKeys.Send("{DOWN}")

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            If btn_Selection.Enabled = True Then

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
                        .Rows(i).Cells(0).Value = i + 1
                    Next

                End With

                Total_Calculation()

            End If

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyCode = 40 Then
            If chk_Transfer.Visible = True And chk_Transfer.Enabled = True Then
                chk_Transfer.Focus()
            Else
                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                    '  btn_save.Focus() ' SendKeys.Send("{TAB}")
                End If
            End If
        End If

        If e.KeyCode = 38 Then txt_EWayBillNo.Focus() 'SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If chk_Transfer.Visible = True And chk_Transfer.Enabled = True Then
                chk_Transfer.Focus()

            Else
                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If

        End If
    End Sub

    Private Sub chk_Transfer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_Transfer.KeyDown
        If e.KeyCode = 40 Then btn_save.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub chk_Transfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_Transfer.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub chk_Warp_Yarn_Stock_Posting_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_Warp_Yarn_Stock_Posting.KeyDown
        If e.KeyCode = 40 Then btn_save.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub chk_Warp_Yarn_Stock_Posting_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_Warp_Yarn_Stock_Posting.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotRls As Single, TotMtrs As Single

        Sno = 0
        TotRls = 0
        TotMtrs = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotRls = TotRls + 1
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotRls)
            .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
        End With
        Amount()
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim n, i As Integer
        Dim sno As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim s As Integer = 0

        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(5).Value) = 1 Then

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(4).Value = Format(Val(dgv_Selection.Rows(i).Cells(4).Value), "#########0.00")
                dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(12).Value
                If Val(dgv_Details.Rows(n).Cells(7).Value) = 0 Then dgv_Details.Rows(n).Cells(7).Value = 100
                If dgv_Details.Columns(8).Visible = True Then
                    dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(9).Value
                End If
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(11).Value

            End If

        Next

        Total_Calculation()
        If dgv_Details.Columns(8).Visible = False Then
            Grid_Cell_DeSelect()
        End If

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_DeliveryTo.Enabled And cbo_DeliveryTo.Visible Then cbo_DeliveryTo.Focus()

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer, d As Integer
        Dim LedIdNo As Integer, CloIdNo As Integer
        Dim NewCode As String
        Dim ClothType As String
        Dim CompIDCondt As String = ""
        Dim TrnsfrSTS As Integer = 0
        Dim TrnTo_CmpGrpIdNo As Integer = 0
        Dim DbName As String = ""


        ClothType = Common_Procedures.ClothType_IdNoToName(con, 1)

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        CloIdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        If CloIdNo = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled And cbo_ClothName.Visible Then cbo_ClothName.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 1 Then
            CompIDCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompIDCondt = "(Company_Type <> 'UNACCOUNT')"
            End If
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        TrnsfrSTS = 0

        Da = New SqlClient.SqlDataAdapter("select * from JobWork_Piece_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Transfer_To_Status").ToString) = False Then
                TrnsfrSTS = Val(Dt1.Rows(0).Item("Transfer_To_Status").ToString)
            End If
        End If
        Dt1.Clear()

        TrnTo_CmpGrpIdNo = 0
        DbName = ""
        If TrnsfrSTS = 1 Then
            TrnTo_CmpGrpIdNo = Val(Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..CompanyGroup_Head", "Transfer_To_CompanyGroupIdNo", "(CompanyGroup_IdNo = " & Str(Val(Common_Procedures.CompGroupIdNo)) & ")"))
            DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(TrnTo_CmpGrpIdNo)))
        End If

        With dgv_Selection

            .Rows.Clear()

            chk_SelectAll.Checked = False

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*,d.po_No as Order_No,cl.clothtype_name,b.Weight from JobWork_Piece_Delivery_Details a INNER JOIN ClothType_Head cl ON a.ClothType_IdNo = cl.ClothType_IdNo LEFT OUTER JOIN Weaver_ClothReceipt_Piece_Details b ON a.Lot_Code = b.Lot_Code and a.Pcs_no = b.Piece_no  LEFT OUTER JOIN Weaver_Cloth_receipt_Head c ON b.Weaver_ClothReceipt_Code  = c.Weaver_ClothReceipt_Code LEFT OUTER JOIN JobWork_Pavu_Receipt_Details d ON c.Set_Code1  = d.Set_Code and  c.Beam_no1  = d.Beam_No where a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            Dt2 = New DataTable
            Da.Fill(Dt2)

            If Dt2.Rows.Count > 0 Then

                For i = 0 To Dt2.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt2.Rows(i).Item("Lot_No").ToString
                    .Rows(n).Cells(2).Value = Dt2.Rows(i).Item("Pcs_NO").ToString
                    .Rows(n).Cells(3).Value = Dt2.Rows(i).Item("clothtype_name").ToString
                    .Rows(n).Cells(4).Value = Format(Val(Dt2.Rows(i).Item("Meters").ToString), "########0.00")
                    .Rows(n).Cells(5).Value = "1"
                    .Rows(n).Cells(6).Value = Dt2.Rows(i).Item("lot_code").ToString
                    .Rows(n).Cells(7).Value = Dt2.Rows(i).Item("Entry_PkCondition").ToString
                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = Dt2.Rows(i).Item("Order_No").ToString
                    .Rows(n).Cells(10).Value = Format(Val(Dt2.Rows(i).Item("Weight").ToString), "########0.00")
                    .Rows(n).Cells(11).Value = Dt2.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                    .Rows(n).Cells(12).Value = Dt2.Rows(i).Item("folding").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                    Next

                    If TrnsfrSTS = 1 Then   'chk_Transfer.Visible = True 

                        Da = New SqlClient.SqlDataAdapter("Select * from " & Trim(DbName) & "..Weaver_ClothReceipt_Piece_Details Where Transfer_From_EntryCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(.Rows(n).Cells(6).Value) & "' and lot_code = '" & Trim(.Rows(n).Cells(6).Value) & "' and Piece_No = '" & Trim(.Rows(n).Cells(2).Value) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '')", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            If Val(Dt2.Rows(i).Item("clothtype_idno").ToString) = 5 Then
                                .Rows(n).Cells(8).Value = Dt1.Rows(0).Item("PackingSlip_Code_Type5").ToString
                            ElseIf Val(Dt2.Rows(i).Item("clothtype_idno").ToString) = 4 Then
                                .Rows(n).Cells(8).Value = Dt1.Rows(0).Item("PackingSlip_Code_Type4").ToString
                            ElseIf Val(Dt2.Rows(i).Item("clothtype_idno").ToString) = 3 Then
                                .Rows(n).Cells(8).Value = Dt1.Rows(0).Item("PackingSlip_Code_Type3").ToString
                            ElseIf Val(Dt2.Rows(i).Item("clothtype_idno").ToString) = 2 Then
                                .Rows(n).Cells(8).Value = Dt1.Rows(0).Item("PackingSlip_Code_Type2").ToString
                            Else
                                .Rows(n).Cells(8).Value = Dt1.Rows(0).Item("PackingSlip_Code_Type1").ToString
                            End If
                        End If
                        Dt1.Clear()

                    End If

                Next i

            End If

            Da = New SqlClient.SqlDataAdapter("select a.* from JobWork_Production_Head a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.JobWork_Delivery_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.cloth_Idno = " & Str(Val(CloIdNo)) & " order by a.JobWork_Production_Date, a.for_orderby, a.JobWork_Production_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_Production_No").ToString
                    If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "A,B,C" Then
                        .Rows(n).Cells(2).Value = "A"
                    Else
                        .Rows(n).Cells(2).Value = "1"
                    End If
                    .Rows(n).Cells(3).Value = Trim(UCase(ClothType))
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("JobWork_Production_Code").ToString
                    .Rows(n).Cells(7).Value = "JPROD-"
                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = ""
                    .Rows(n).Cells(10).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                    .Rows(n).Cells(11).Value = ""
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("folding").ToString

                Next

            End If
            Dt1.Clear()

            Dim vSTKOF_ID As Integer
            Dim vLed_type As String
            Dim vSTKOFID_CONDT As String

            vLed_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(LedIdNo)) & ")")

            If Trim(UCase(vLed_type)) = "JOBWORKER" Then
                vSTKOF_ID = LedIdNo
                vSTKOFID_CONDT = "(a.StockOff_IdNo = " & Str(Val(vSTKOF_ID)) & ")"
            Else
                vSTKOF_ID = 5
                vSTKOFID_CONDT = "( (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5) and a.Ledger_IdNo = " & Str(Val(LedIdNo)) & ")"
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then '---- SAMANTH TEXTILES (SOMANUR)
                vSTKOFID_CONDT = "(a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5 or a.StockOff_IdNo = " & Str(Val(vSTKOF_ID)) & ")"
            End If

            Da = New SqlClient.SqlDataAdapter("select a.Lot_Code, a.Lot_No, a.Piece_No, a.folding, a.Type1_Meters, a.Type2_Meters, a.Type3_Meters, a.Type4_Meters, a.Type5_Meters, a.Weight, a.PackingSlip_Code_Type1, a.PackingSlip_Code_Type2, a.PackingSlip_Code_Type3, a.PackingSlip_Code_Type4, a.PackingSlip_Code_Type5, a.Weaving_JobCode_forSelection, c.po_no as lot_pono, d.Po_No as Order_No from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Weaver_Cloth_receipt_Head c ON c.Weaver_ClothReceipt_Code = a.Lot_Code  LEFT OUTER JOIN JobWork_Pavu_Receipt_Details d ON (c.Set_Code1  = d.Set_Code and c.Beam_no1  = d.Beam_No ) where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & vSTKOFID_CONDT & " and a.cloth_Idno = " & Str(Val(CloIdNo)) & "  and ( (a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 = '') or (a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 = '')  or (a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 = '')  or (a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 = '')  or (a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 = '') ) order by a.for_orderby, a.Weaver_ClothReceipt_Date, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
            'Da = New SqlClient.SqlDataAdapter("select a.*, d.Po_No as Order_No from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Weaver_Cloth_receipt_Head c ON a.Weaver_ClothReceipt_Code  = c.Weaver_ClothReceipt_Code LEFT OUTER JOIN JobWork_Pavu_Receipt_Details d ON (c.Set_Code1  = d.Set_Code and  c.Beam_no1  = d.Beam_No ) where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & vSTKOFID_CONDT & " and a.cloth_Idno = " & Str(Val(CloIdNo)) & "  and ( (a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 = '') or (a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 = '')  or (a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 = '')  or (a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 = '')  or (a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 = '') ) order by a.for_orderby, a.Weaver_ClothReceipt_Date, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
            'Da = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.StockOff_IdNo = " & Str(Val(LedIdNo)) & " and a.cloth_Idno = " & Str(Val(CloIdNo)) & "  and ( (a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 = '') or (a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 = '')  or (a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 = '')  or (a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 = '')  or (a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 = '') ) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1


                    If Val(Dt1.Rows(i).Item("Type1_Meters").ToString) <> 0 And Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString) = "" Then

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type1
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type1_Meters").ToString
                        .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(7).Value = "PCDOF-"
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Order_No").ToString
                        If Trim(.Rows(n).Cells(9).Value) = "" Then
                            .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("lot_pono").ToString
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Weight").ToString
                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("folding").ToString

                    End If

                    If Val(Dt1.Rows(i).Item("Type2_Meters").ToString) <> 0 And Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString) = "" Then
                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type2
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type2_Meters").ToString
                        .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(7).Value = "PCDOF-"
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Order_No").ToString
                        If Trim(.Rows(n).Cells(9).Value) = "" Then
                            .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("lot_pono").ToString
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Weight").ToString
                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("folding").ToString
                    End If

                    If Val(Dt1.Rows(i).Item("Type3_Meters").ToString) <> 0 And Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString) = "" Then
                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type3
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type3_Meters").ToString
                        .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(7).Value = "PCDOF-"
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Order_No").ToString
                        If Trim(.Rows(n).Cells(9).Value) = "" Then
                            .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("lot_pono").ToString
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Weight").ToString
                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("folding").ToString

                    End If

                    If Val(Dt1.Rows(i).Item("Type4_Meters").ToString) <> 0 And Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString) = "" Then
                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type4
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type4_Meters").ToString
                        .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(7).Value = "PCDOF-"
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Order_No").ToString
                        If Trim(.Rows(n).Cells(9).Value) = "" Then
                            .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("lot_pono").ToString
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Weight").ToString
                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("folding").ToString

                    End If

                    If Val(Dt1.Rows(i).Item("Type5_Meters").ToString) <> 0 And Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString) = "" Then
                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type5
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type5_Meters").ToString
                        .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(7).Value = "PCDOF-"
                        .Rows(n).Cells(8).Value = ""
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Order_No").ToString
                        If Trim(.Rows(n).Cells(9).Value) = "" Then
                            .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("lot_pono").ToString
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Weight").ToString
                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("folding").ToString

                    End If

                Next

            End If
            Dt1.Clear()

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

        Try
            With dgv_Selection

                If .RowCount > 0 And RwIndx >= 0 Then

                    If .Rows(RwIndx).Cells(8).Value <> "" Then
                        .Rows(RwIndx).Cells(5).Value = 1
                        MessageBox.Show("already Delivered, Cannot De-Select this Piece", "INVALID PCS DE-SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If

                    .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

                    If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then .Rows(RwIndx).Cells(5).Value = ""

                    If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                        Next
                        .DefaultCellStyle.SelectionForeColor = Color.Black

                    Else
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                        Next
                        .DefaultCellStyle.SelectionForeColor = Color.Red

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID PCS SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Try
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                With dgv_Selection
                    If .Rows.Count > 0 Then
                        If .CurrentCell.RowIndex >= 0 Then
                            Select_Piece(.CurrentCell.RowIndex)
                            e.Handled = True
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID PCS SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub
    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub
    Private Sub cbo_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, cbo_Ledger, cbo_DeliveryTo, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If txt_JobNo.Enabled And txt_JobNo.Visible Then
                txt_JobNo.Focus()
            Else
                cbo_Ledger.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName.KeyPress

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Clo_idno As Integer = 0
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        Clo_idno = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Rolls :", "FOR PIECE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)


            Else
                cbo_DeliveryTo.Focus()

            End If



            Da = New SqlClient.SqlDataAdapter("select a.Wages_For_Type1 , Sound_Rate from Cloth_Head a Where a.cloth_Idno = " & Str(Val(Clo_idno)), con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                '    txt_rate.Text = Str(Val(Dt1.Rows(0).Item("Wages_For_Type1").ToString))
                txt_rate.Text = Dt1.Rows(0).Item("Sound_Rate").ToString
            End If


        End If

    End Sub

    Private Sub cbo_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub btn_OrderSelection_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_OrderSelection_Close.Click
        Close_OrderSelection()
    End Sub

    Private Sub btn_Order_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Order_Selection.Click
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_JbOrdCD As String = ""
        Dim Ent_CloID As Integer = 0
        Dim CompIDCondt As String = ""

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 1 Then
            CompIDCondt = ""
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Cmd.Connection = con

        Cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Name1          ,   Int1      ,   Meters1 ) " &
                            "             Select  a.JobWork_Order_Code, a.Cloth_IdNo, a.Meters  from JobWork_Order_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head b ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = c.Cloth_IdNo Where a.Ledger_IdNo = " & Str(Val(LedIdNo))
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Name1 ,   Int1      ,      meters1       ) " &
                            "    Select  a.JobWork_Order_Code, a.Cloth_IdNo, -1*a.Total_Delivery_Meters from JobWork_Piece_Delivery_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head b ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = c.Cloth_IdNo Where a.Ledger_IdNo = " & Str(Val(LedIdNo))
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " ( Name1, Int1,     Meters1) " &
                           " Select                Name1, Int1, sum(Meters1) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by Name1, Int1"
        Cmd.ExecuteNonQuery()

        With dgv_order_selection

            .Rows.Clear()

            SNo = 0
            Ent_JbOrdCD = ""
            Ent_CloID = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.Meters1 as balancemeters from JobWork_Order_Details a INNER JOIN JobWork_Piece_Delivery_Head b ON a.JobWork_Order_Code = b.JobWork_Order_Code and a.cloth_idno = b.cloth_idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = c.Cloth_IdNo INNER JOIN " & Trim(Common_Procedures.EntryTempTable) & " d ON a.JobWork_Order_Code = d.Name1 and a.cloth_idno = d.Int1 where b.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.JobWork_Order_Date, a.for_orderby, a.JobWork_Order_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    Ent_JbOrdCD = Dt1.Rows(i).Item("JobWork_Order_Code").ToString
                    Ent_CloID = Val(Dt1.Rows(i).Item("Cloth_IdNo").ToString)

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_Order_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("JobWork_Order_Date").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("balancemeters").ToString) + Val(Dt1.Rows(i).Item("Total_Delivery_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = "1"
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("JobWork_Order_Code").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.Meters1 as balancemeters from JobWork_Order_Details a INNER JOIN Cloth_Head c ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = c.Cloth_IdNo INNER JOIN " & Trim(Common_Procedures.EntryTempTable) & " d ON a.JobWork_Order_Code = d.Name1 and a.cloth_idno = d.Int1 where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and d.Meters1 > 0 and (a.JobWork_Order_Code <> '" & Trim(Ent_JbOrdCD) & "' OR (a.JobWork_Order_Code = '" & Trim(Ent_JbOrdCD) & "' and a.Cloth_IdNo <> " & Str(Val(Ent_CloID)) & " ) ) order by a.JobWork_Order_Date, a.for_orderby, a.JobWork_Order_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_Order_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("JobWork_Order_Date").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("balancemeters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = ""
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("JobWork_Order_Code").ToString

                Next

            End If
            Dt1.Clear()

        End With

        If dgv_Details.Columns(8).Visible = False Then
            Grid_Cell_DeSelect()
        End If


        pnl_Order_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_order_selection.Focus()
        pnl_Order_Selection.BringToFront()

    End Sub

    Private Sub dgv_Order_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_order_selection.CellClick
        Select_Order(e.RowIndex)
    End Sub

    Private Sub Select_Order(ByVal RwIndx As Integer)
        Dim i As Integer

        Try
            With dgv_order_selection

                If .RowCount > 0 And RwIndx >= 0 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(6).Value = ""
                    Next

                    .Rows(RwIndx).Cells(6).Value = (Val(.Rows(RwIndx).Cells(6).Value) + 1) Mod 2

                    If Val(.Rows(RwIndx).Cells(6).Value) = 0 Then

                        .Rows(RwIndx).Cells(6).Value = ""

                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                        Next

                    Else
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                        Next

                    End If

                    Close_OrderSelection()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID ORDER SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub dgv_Order_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_order_selection.KeyDown
        Try
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                With dgv_order_selection
                    If .Rows.Count > 0 Then
                        If .CurrentCell.RowIndex >= 0 Then
                            Select_Order(.CurrentCell.RowIndex)
                            e.Handled = True
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID ORDER SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Close_OrderSelection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        For i = 0 To dgv_order_selection.RowCount - 1

            If Val(dgv_order_selection.Rows(i).Cells(6).Value) = 1 Then

                lbl_JobCode.Text = dgv_order_selection.Rows(i).Cells(7).Value
                txt_JobNo.Text = dgv_order_selection.Rows(i).Cells(1).Value
                lbl_JobDate.Text = dgv_order_selection.Rows(i).Cells(2).Value
                cbo_ClothName.Text = dgv_order_selection.Rows(i).Cells(3).Value

            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Order_Selection.Visible = False
        If cbo_ClothName.Enabled And cbo_ClothName.Visible Then cbo_ClothName.Focus()

    End Sub

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                If Trim(.Rows(i).Cells(8).Value) <> "" Then
                    .Rows(i).Cells(5).Value = "1"

                Else
                    .Rows(i).Cells(5).Value = ""
                    For J = 0 To .ColumnCount - 1
                        .Rows(i).Cells(J).Style.ForeColor = Color.Black
                    Next J

                End If
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    If Val(.Rows(i).Cells(5).Value) = 0 Then
                        Select_Piece(i)
                    End If
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
            pnl_Print2.Visible = True
            pnl_Back.Enabled = False
            If btn_Print_Delivery.Enabled And btn_Print_Delivery.Visible Then
                btn_Print_Delivery.Focus()
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            print_Delivery()

        Else
            pnl_Print.Visible = True
            pnl_Back.Enabled = False
            If btn_Print_PcsWise_Delivery_Without_Weight.Enabled And btn_Print_PcsWise_Delivery_Without_Weight.Visible Then
                btn_Print_PcsWise_Delivery_Without_Weight.Focus()
            End If

        End If


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1021" Then '---- Asia Sizing (Palladam)
        '    pnl_Print.Visible = True
        '    pnl_Back.Enabled = False
        '    If btn_Print_PcsWise_Delivery_With_Weight.Enabled And btn_Print_PcsWise_Delivery_With_Weight.Visible Then
        '        btn_Print_PcsWise_Delivery_With_Weight.Focus()
        '    End If

        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
        '    pnl_Print2.Visible = True
        '    pnl_Back.Enabled = False
        '    If btn_Print_Delivery.Enabled And btn_Print_Delivery.Visible Then
        '        btn_Print_Delivery.Focus()
        '    End If

        'Else
        '    print_Delivery()

        'End If
    End Sub

    Public Sub print_Delivery()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Jobwork_Piece_Delivery_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from JobWork_Piece_Delivery_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then ' -----------SRINIVASA TEXTILE
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "123")
            prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")
        End If

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next


        If Print_PDF_Status = True Then
            '--This is actual & correct 
            'MessageBox.Show("Printing_Invoice - 11")
            PrintDocument1.DocumentName = "Invoice"
            'MessageBox.Show("Printing_Invoice - 12")
            PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
            'MessageBox.Show("Printing_Invoice - 13")
            PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
            'MessageBox.Show("Printing_Invoice - 14")
            PrintDocument1.Print()
            Exit Sub
        End If
        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        If prn_Status = 1 Then


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

                        Else

                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    Exit For
                                End If
                            Next

                        End If

                        PrintDocument1.Print()
                    End If

                Else
                    PrintDocument1.Print()

                End If
            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else

            'Try

            Dim ppd As New PrintPreviewDialog

            ppd.Document = PrintDocument1

            ppd.WindowState = FormWindowState.Normal
            ppd.StartPosition = FormStartPosition.CenterScreen
            ppd.ClientSize = New Size(600, 600)


            ppd.ShowDialog()
            'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
            '    ppd.ShowDialog()
            'End If

            'Catch ex As Exception
            '    MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            'End Try


        End If
        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        'If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
        '    Try
        '        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
        '        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        '            PrintDocument1.Print()
        '        End If

        '    Catch ex As Exception
        '        MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '    End Try


        'Else
        '    'Try

        '    Dim ppd As New PrintPreviewDialog

        '    ppd.Document = PrintDocument1

        '    ppd.WindowState = FormWindowState.Normal
        '    ppd.StartPosition = FormStartPosition.CenterScreen
        '    ppd.ClientSize = New Size(600, 600)

        '    ppd.ShowDialog()
        '    'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '    '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
        '    '    ppd.ShowDialog()
        '    'End If

        '    'Catch ex As Exception
        '    '    MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

        '    'End Try

        'End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String = ""
        Dim LmIdNo As Integer = 0
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim sno As Integer = 0

        Dim dt3 As New DataTable
        Dim da4 As New SqlClient.SqlDataAdapter

        Dim dt4 As New DataTable
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_PageNo = 0
        prn_NoofBmDets = 0
        prn_DetMxIndx = 0
        Total_Weight = 0
        Total_Mtr = 0
        Print_sno = 0
        prn_Count = 0

        Erase prn_DetAr

        prn_DetAr = New String(500, 30) {}

        Try
            da3 = New SqlClient.SqlDataAdapter("select j.*,L.Loom_name from  JobWork_Production_Head j left outer join Loom_Head L On L.Loom_Idno=j.Loom_idno Left Outer join JobWork_Piece_Delivery_Head JPD On j.JobWork_Delivery_Code = JPD.JobWork_Piece_Delivery_Code  where j.company_idno=  " & Str(Val(lbl_Company.Tag)) & "  and j.JobWork_Delivery_Code = '" & Trim(NewCode) & "'order by j.for_orderby", con)
            dt3 = New DataTable
            da3.Fill(dt3)

            da4 = New SqlClient.SqlDataAdapter("select c.cloth_name,c.WEAVE,a.po_no,sum(a.meters)as mtrs,j.rate,(sum(a.meters) *j.rate) as total  from JobWork_Piece_Delivery_Details a Left Outer Join JobWork_Piece_Delivery_Head j on j.JobWork_Piece_Delivery_Code=a.JobWork_Piece_Delivery_Code  LEFT OUTER JOIN Cloth_Head c ON j.Cloth_IdNo = c.Cloth_IdNo where a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' Group by c.cloth_name,C.wEAVE,a.po_no,j.rate,j.total_amount", con)
            prn_HdDt_1 = New DataTable
            da4.Fill(prn_HdDt_1)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.*, d.Ledger_mainName as Transport_Name, e.* , n.Count_Name As WarpName , G.Count_Name As WEftName ,lsh.state_name ,lsh.state_Code, f.Ledger_mainName as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.Pan_No as DeliveryTo_PanNo, Dsh.State_Name as DeliveryTo_State_Name, Dsh.State_Code as DeliveryTo_State_Code from JobWork_Piece_Delivery_Head a INNER JOIN company_Head c ON a.Company_IdNo = c.company_Idno INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_IdNo = d.ledger_IdNo LEFT OUTER JOIN Ledger_Head f ON a.Delivery_IdNo = f.ledger_IdNo Left outer join state_head lsh on b.ledger_state_idno=lsh.state_idno LEFT OUTER JOIN State_Head Dsh ON f.Ledger_State_IdNo = Dsh.State_IdNo   LEFT OUTER JOIN Cloth_Head e ON a.Cloth_IdNo = e.Cloth_IdNo LEFT OUTER JOIN Count_Head n ON n.Count_IdNo = e.Cloth_WarpCount_IdNo LEFT OUTER JOIN Count_Head g ON g.Count_IdNo = e.Cloth_WeftCount_IdNo  where a.company_idno= " & Str(Val(lbl_Company.Tag)) & "  and a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
                    da2 = New SqlClient.SqlDataAdapter("select a.*, c.loom_name,w.weight as wgt , w.Lot_NUmber ,b.Set_No1 As SetNo,b.Set_No2 , b.warp_LotNo, b.weft_lotno , b.po_no as lot_pono from JobWork_Piece_Delivery_Details a LEFT OUTER JOIN Weaver_Cloth_Receipt_Head b ON a.Lot_Code = b.Weaver_ClothReceipt_Code LEFT OUTER JOIN Weaver_ClothReceipt_Piece_Details w ON b.Weaver_ClothReceipt_Code = w.Lot_Code  and a.pcs_no=w.piece_no LEFT OUTER JOIN Loom_Head c ON b.Loom_IdNo  = c.Loom_IdNo  where a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by a.po_no, a.Sl_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                Else
                    da2 = New SqlClient.SqlDataAdapter("select a.*, c.loom_name,w.weight as wgt  from JobWork_Piece_Delivery_Details a LEFT OUTER JOIN Weaver_Cloth_Receipt_Head b ON a.Lot_Code = b.Weaver_ClothReceipt_Code LEFT OUTER JOIN Weaver_ClothReceipt_Piece_Details w ON b.Weaver_ClothReceipt_Code = w.Lot_Code  and a.pcs_no=w.piece_no LEFT OUTER JOIN Loom_Head c ON b.Loom_IdNo  = c.Loom_IdNo where a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                End If

                If prn_DetDt.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(i).Item("Meters").ToString) <> 0 Then

                            prn_DetMxIndx = prn_DetMxIndx + 1
                            sno = sno + 1
                            prn_DetAr(prn_DetMxIndx, 1) = sno 'Trim(prn_DetDt.Rows(i).Item("Sl_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 2) = ""
                            If Trim(UCase(prn_DetDt.Rows(i).Item("Entry_PkCondition").ToString)) = "JPROD-" Then
                                LmIdNo = Common_Procedures.get_FieldValue(con, "JobWork_Production_Head", "Loom_IdNo", "(JobWork_Production_Code = '" & Trim(prn_DetDt.Rows(i).Item("lot_code").ToString) & "')")
                                prn_DetAr(prn_DetMxIndx, 2) = Common_Procedures.Loom_IdNoToName(con, LmIdNo)

                            Else
                                If IsDBNull(prn_DetDt.Rows(i).Item("loom_name").ToString) = False Then
                                    prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("loom_name").ToString)
                                End If

                            End If
                            If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
                                prn_DetAr(prn_DetMxIndx, 3) = prn_DetDt.Rows(i).Item("Pcs_No").ToString

                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
                                    prn_DetAr(prn_DetMxIndx, 9) = Trim(prn_DetDt.Rows(i).Item("lot_pono").ToString)
                                Else
                                    prn_DetAr(prn_DetMxIndx, 9) = Trim(prn_DetDt.Rows(i).Item("Po_No").ToString)
                                End If

                                prn_DetAr(prn_DetMxIndx, 10) = Trim(prn_DetDt.Rows(i).Item("Lot_NUmber").ToString)
                                prn_DetAr(prn_DetMxIndx, 11) = Trim(prn_DetDt.Rows(i).Item("SetNo").ToString)
                                prn_DetAr(prn_DetMxIndx, 12) = Trim(prn_DetDt.Rows(i).Item("Warp_LotNo").ToString)
                                prn_DetAr(prn_DetMxIndx, 13) = Trim(prn_DetDt.Rows(i).Item("Weft_LotNo").ToString)

                            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1420" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
                                prn_DetAr(prn_DetMxIndx, 3) = Trim(prn_DetDt.Rows(i).Item("Pcs_No").ToString)

                            Else
                                prn_DetAr(prn_DetMxIndx, 3) = Trim(prn_DetDt.Rows(i).Item("lot_no").ToString) & "-" & Trim(prn_DetDt.Rows(i).Item("Pcs_No").ToString)

                            End If

                            prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
                            prn_DetAr(prn_DetMxIndx, 5) = Trim(prn_DetDt.Rows(i).Item("lot_no").ToString)

                            prn_DetAr(prn_DetMxIndx, 6) = Format(Val(prn_DetDt.Rows(i).Item("Weight").ToString), "#########0.00")
                            prn_DetAr(prn_DetMxIndx, 7) = Format((Val(prn_DetDt.Rows(i).Item("Weight").ToString)) / (Val(prn_DetDt.Rows(i).Item("Meters").ToString)), "#########0.000")
                            'prn_DetAr(prn_DetMxIndx, 9) = Val(dt3.Rows(0).Item("Folding_Percentage").ToString)
                            'vPRN_FOLDINGPERC = Val(dt3.Rows(0).Item("Folding_Percentage").ToString)
                            If dt3.Rows.Count > 0 Then
                                '    prn_DetAr(prn_DetMxIndx, 6) = Format(Val(dt3.Rows(i).Item("Weight").ToString), "#########0.00")
                                '    prn_DetAr(prn_DetMxIndx, 7) = Format((Val(dt3.Rows(i).Item("Weight").ToString)) / (Val(prn_DetDt.Rows(i).Item("Meters").ToString)), "#########0.000")
                                prn_DetAr(prn_DetMxIndx, 9) = Val(dt3.Rows(0).Item("Folding_Percentage").ToString)
                                vPRN_FOLDINGPERC = Val(dt3.Rows(0).Item("Folding_Percentage").ToString)
                            End If

                            prn_DetAr(prn_DetMxIndx, 8) = Format(Val(prn_DetDt.Rows(i).Item("wgt").ToString), "########0.00")
                            If Val(prn_DetDt.Rows(i).Item("wgt").ToString) > 0 Then
                                prn_DetAr(prn_DetMxIndx, 7) = Format((Val(prn_DetDt.Rows(i).Item("wgt").ToString)) / (Val(prn_DetDt.Rows(i).Item("Meters").ToString)), "#########0.000")
                            End If

                        End If
                    Next i



                End If

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()
            da4.Dispose()

            dt3.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "--1021--" Then
            If prn_Status = 1 Then
                Printing_Format1(e)
            Else
                Printing_Format4_PrePrint_1021(e)
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1052" Then '---- Shri Vedha Tex (Karumanthapatti) - Nithya Sizing
            If prn_Status = 1 Then
                Printing_Format_1233(e)
            Else
                Printing_Format3(e)
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1233" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1420" Then '----Vipin textile Somanur
            If prn_Status = 1 Then
                Printing_Format_1233(e)
            Else
                Printing_Format1(e)
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '----united weaves
            If prn_Status = 1 Then
                Printing_GST_Format_1186(e)
            Else
                Printing_Format_packingSlip_1186(e)
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            Printing_GST_Format_1544(e)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1547" Then
            If prn_Status = 1 Then
                Printing_Format_1547(e)
            Else
                Printing_Format1(e)
            End If

            'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1420" Then '----Cyber Tex
            '    Printing_Format_1420(e)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then '---- SAMANTH TEXTILES (SOMANUR)
            If prn_Status = 1 Then
                Printing_Format6_With_Weight_1608(e)
            Else
                Printing_Format6_1608(e)
            End If

        Else
            If prn_Status = 1 Then
                Printing_Format_1233(e)
            Else
                Printing_Format1(e)
            End If
            'Printing_Format5(e)  '-------Printing_Format1(e)

        End If
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
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        'PpSzSTS = False

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    'Debug.Print(ps.PaperName)
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
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                e.PageSettings.PaperSize = ps
        '                Exit For
        '            End If
        '        Next
        '    End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 30
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

        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            NoofItems_PerPage = 28
        Else
            NoofItems_PerPage = 34
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 65 : ClArr(2) = 80 : ClArr(3) = 110 : ClArr(4) = 110
        ClArr(5) = 65 : ClArr(6) = 80 : ClArr(7) = 110
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' Try

        If prn_HdDt.Rows.Count > 0 Then

            Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

            NoofDets = 0

            CurY = CurY - 10

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_NoofBmDets < prn_DetMxIndx

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                        e.HasMorePages = True

                        Return

                    End If

                    prn_DetIndx = prn_DetIndx + 1

                    CurY = CurY + TxtHgt

                    If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)

                        prn_NoofBmDets = prn_NoofBmDets + 1

                    End If

                    If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 12, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                        prn_NoofBmDets = prn_NoofBmDets + 1

                    End If

                    NoofDets = NoofDets + 1

                Loop

            End If

            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

        End If

        ' Catch ex As Exception

        ' MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String

        PageNo = PageNo + 1

        CurY = TMargin


        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GstinNo").ToString) <> "" Then
            Cmp_TinNo = "GST NO: " & prn_HdDt.Rows(0).Item("Company_GstinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            Common_Procedures.Print_To_PrintDocument(e, "PACKING SLIP", LMargin, CurY, 2, PrintWidth, p1Font)
        ElseIf Common_Procedures.settings.CustomerCode = "1420" Then
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY CHALLAN", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "PIECE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("SET NO     :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("Despatched_To").ToString) <> "" Then
            If Common_Procedures.settings.CustomerCode = "1420" Then
                Common_Procedures.Print_To_PrintDocument(e, "DESPATCH THROUGH", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "DESPATCH TO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatched_To").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(2))

        CurY = CurY + 10

        If Common_Procedures.settings.CustomerCode = "1420" Then
            Common_Procedures.Print_To_PrintDocument(e, "Quatlity :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString & "               Total Rolls :  " & prn_HdDt.Rows(0).Item("Total_Rolls").ToString & "              Total Meters :  " & prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + 10, CurY, 0, 0, pFont)

        Else
            Common_Procedures.Print_To_PrintDocument(e, "Quatlity :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("JobWork_Order_No").ToString) <> "" Then
                If Common_Procedures.settings.CustomerCode = "1087" Then
                    Common_Procedures.Print_To_PrintDocument(e, "PO No. :  " & prn_HdDt.Rows(0).Item("JobWork_Order_No").ToString, PageWidth - 20, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "JOB No. :  " & prn_HdDt.Rows(0).Item("JobWork_Order_No").ToString, PageWidth - 20, CurY, 1, 0, pFont)
                End If
            End If

        End If
        CurY = CurY + TxtHgt + 10
        If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "EWAY BILL NO :  " & prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + 10, CurY, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Approximate Value :  " & prn_HdDt.Rows(0).Item("Total_Amount").ToString, PageWidth - 40, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "SL.NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS.NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SL.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim W1 As Single

        Try
            W1 = e.Graphics.MeasureString("Vehicle No    :  ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

            If Common_Procedures.settings.CustomerCode <> "1420" Then

                CurY = CurY + TxtHgt

                Common_Procedures.Print_To_PrintDocument(e, "Total Rolls ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + W1 + 25, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Total Meters ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            End If

            If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            End If
            If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
                CurY = CurY + TxtHgt
                Dim len1 As Integer = 0

                p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
                len1 = e.Graphics.MeasureString(" Date     : ", pFont).Width
                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "INWARD / OUTWARD  : ", LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt + 5
                pFont = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Ref No   ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  " & prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + len1 + 10, CurY, 0, 0, pFont)
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, "Entry  ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  __________________", LMargin + len1 + 10, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 5

                Common_Procedures.Print_To_PrintDocument(e, "Date ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  __________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 5

                Common_Procedures.Print_To_PrintDocument(e, "Time   ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  __________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 10
            ElseIf Common_Procedures.settings.CustomerCode = "1420" Or Common_Procedures.settings.CustomerCode = "1441" Then

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

                CurY = CurY + TxtHgt + 5

                CurY = CurY + TxtHgt + 5
                CurY = CurY + TxtHgt + 5

            End If


            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If


            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 0, 0, pFont)


            If Common_Procedures.settings.CustomerCode = "1186" Or Common_Procedures.settings.CustomerCode = "1420" Or Common_Procedures.settings.CustomerCode = "1441" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then

                Common_Procedures.Print_To_PrintDocument(e, "Authorised Signature", PageWidth - 5, CurY, 1, 0, pFont)

            Else
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            End If
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim SNo As Integer
        Dim C1 As Single
        Dim PpSzSTS As Boolean = False

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
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
            .Left = 40 ' 65
            .Right = 40
            .Top = 30
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

        TxtHgt = 18 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(60) : ClArr(2) = 425 : ClArr(3) = 110
        ClArr(4) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3))

        CurY = TMargin
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

        CurY = CurY + strHeight - 2
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PIECE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = ClArr(1) + ClArr(2)

        W1 = e.Graphics.MeasureString("DC.NO    : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString)), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + 8

        Common_Procedures.Print_To_PrintDocument(e, "SL.NO", LMargin, CurY, 2, ClArr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClArr(1), CurY, 2, ClArr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ROLLS", LMargin + ClArr(1) + ClArr(2), CurY, 2, ClArr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        CurY = CurY + 13
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(3))

        SNo = SNo + 1
        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 40
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2), CurY, LMargin + ClArr(1) + ClArr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(3))

        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "Roll No.s  : ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Prn_PcNos1), LMargin + ClArr(1) + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, Trim(Prn_PcNos2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, Trim(Prn_PcNos3), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, Trim(Prn_PcNos4), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "Transport Name  :  " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Vehicle No  :  " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + ClArr(1) + ClArr(2) - 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        CurY = CurY + 10
        'CurY = CurY + TxtHgt
        'If Val(Common_Procedures.User.IdNo) <> 1 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        'End If
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Signature of the receiver", LMargin + 20, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 400, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(8) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        'MessageBox.Show("CurY = " & CurY)

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next
        PpSzSTS = False

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
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
        PrintDocument1.DefaultPageSettings.Landscape = False

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 30
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

        'If PrintDocument1.DefaultPageSettings.Landscape = True Then
        '    With PrintDocument1.DefaultPageSettings.PaperSize
        '        PrintWidth = .Height - TMargin - BMargin
        '        PrintHeight = .Width - RMargin - LMargin
        '        PageWidth = .Height - TMargin
        '        PageHeight = .Width - RMargin
        '    End With
        'End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 34 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 60 : ClArr(2) = 80 : ClArr(3) = 95 : ClArr(4) = 60 : ClArr(5) = 80 : ClArr(6) = 95 : ClArr(7) = 60 : ClArr(8) = 80

        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_NoofBmDets < prn_DetMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            prn_DetIndx = prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetIndx = prn_DetIndx + 1

                        CurY = CurY + TxtHgt

                        If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 12, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            ' Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 12, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            ' Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage+ NoofItems_PerPage, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage, 4)), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage+ NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        NoofDets = NoofDets + 1

                    Loop

                End If

                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        CurY = TMargin
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

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PIECE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(11) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(2))

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "Quatlity :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOOM NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOOM.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOOM.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim W1 As Single

        Try
            W1 = e.Graphics.MeasureString("Vehicle No    :  ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "Total Rolls ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Total Meters ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 70, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format4_PrePrint_1021(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, pFont1 As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim CurX As Single = 0
        Dim CurTime As Date
        Dim TotalMtr_Row1 As Single = 0
        Dim TotalMtr_Row2 As Single = 0
        Dim TotalMtr_Row3 As Single = 0
        Dim TotalMtr_Row4 As Single = 0
        '   Dim CurY As Single, TxtHgt As Single
        Dim PcsInWrds As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next
        PrintDocument1.DefaultPageSettings.Landscape = False

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0
            .Right = 0
            .Top = 10
            .Bottom = 0
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

        'If PrintDocument1.DefaultPageSettings.Landscape = True Then
        '    With PrintDocument1.DefaultPageSettings.PaperSize
        '        PrintWidth = .Height - TMargin - BMargin
        '        PrintHeight = .Width - RMargin - LMargin
        '        PageWidth = .Height - TMargin
        '        PageHeight = .Width - RMargin
        '    End With
        'End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        pFont1 = New Font("Calibri", 8, FontStyle.Regular)


        NoofItems_PerPage = 15 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 60 : ClArr(2) = 80 : ClArr(3) = 95 : ClArr(4) = 60 : ClArr(5) = 80 : ClArr(6) = 95 : ClArr(7) = 60 : ClArr(8) = 80

        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try


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

            If prn_HdDt.Rows.Count > 0 Then

                CurX = LMargin + 80
                CurY = TMargin + 215
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, CurX, CurY, 0, 0, p1Font)


                CurX = LMargin + 660
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, p1Font)

                CurTime = TimeOfDay.ToString

                CurX = LMargin + 320
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(CurTime), "h:mm tt"), CurX, CurY, 0, 0, p1Font)

                CurX = LMargin + 100
                CurY = TMargin + 255
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Name").ToString & "  , " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, CurX, CurY, 0, 0, p1Font)

                CurX = LMargin + 100
                CurY = TMargin + 290
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, CurX, CurY, 0, 0, p1Font)

                CurX = LMargin + 160
                CurY = TMargin + 375
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("WarpName").ToString, CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 280
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("WeftName").ToString, CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 400
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Reed").ToString, CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 520
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Pick").ToString, CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 645
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Width").ToString, CurX, CurY, 0, 0, pFont)


                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_NoofBmDets < prn_DetMxIndx

                        prn_DetIndx = prn_DetIndx + 1

                        If Val(prn_DetIndx) <= 15 Then

                            If Val(prn_DetIndx) = 1 Then
                                CurY = TMargin + 435
                            Else
                                CurY = CurY + TxtHgt + 15
                            End If

                            CurX = LMargin + 70
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), CurX, CurY, 0, 0, pFont)
                            CurX = LMargin + 210
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), CurX, CurY, 1, 0, pFont)

                            TotalMtr_Row1 = TotalMtr_Row1 + Val(prn_DetAr(prn_DetIndx, 4))

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        If Val(prn_DetIndx) > 15 And Val(prn_DetIndx) <= 30 Then

                            If Val(prn_DetIndx) = 16 Then
                                CurY = TMargin + 435
                            Else
                                CurY = CurY + TxtHgt + 15
                            End If

                            CurX = LMargin + 260
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), CurX, CurY, 0, 0, pFont)
                            CurX = LMargin + 390
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), CurX, CurY, 1, 0, pFont)

                            TotalMtr_Row2 = TotalMtr_Row2 + Val(prn_DetAr(prn_DetIndx, 4))

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        If Val(prn_DetIndx) > 30 And Val(prn_DetIndx) <= 45 Then

                            If Val(prn_DetIndx) = 31 Then
                                CurY = TMargin + 435
                            Else
                                CurY = CurY + TxtHgt + 15
                            End If

                            CurX = LMargin + 440
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), CurX, CurY, 0, 0, pFont)
                            CurX = LMargin + 580
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), CurX, CurY, 1, 0, pFont)

                            TotalMtr_Row3 = TotalMtr_Row3 + Val(prn_DetAr(prn_DetIndx, 4))

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        If Val(prn_DetIndx) > 45 And Val(prn_DetIndx) <= 60 Then

                            If Val(prn_DetIndx) = 46 Then
                                CurY = TMargin + 435
                            Else
                                CurY = CurY + TxtHgt + 15
                            End If

                            CurX = LMargin + 625
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), CurX, CurY, 0, 0, pFont)
                            CurX = LMargin + 760
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), CurX, CurY, 1, 0, pFont)

                            TotalMtr_Row4 = TotalMtr_Row4 + Val(prn_DetAr(prn_DetIndx, 4))

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        NoofDets = NoofDets + 1

                    Loop

                End If

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                CurX = LMargin + 210
                CurY = TMargin + 930

                Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotalMtr_Row1), "########0.00"), CurX, CurY, 1, 0, p1Font)

                CurX = LMargin + 390
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotalMtr_Row2), "########0.00"), CurX, CurY, 1, 0, p1Font)

                CurX = LMargin + 580
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotalMtr_Row3), "########0.00"), CurX, CurY, 1, 0, p1Font)

                CurX = LMargin + 760
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotalMtr_Row4), "########0.00"), CurX, CurY, 1, 0, p1Font)

                CurX = LMargin + 150 - 10
                CurY = TMargin + 960
                '  Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), CurX, CurY, 0, 0, p1Font)

                PcsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString))
                PcsInWrds = Replace(Trim(PcsInWrds), "", "")
                PcsInWrds = Replace(PcsInWrds, "Only", "")

                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0") & " ( " & PcsInWrds & " )", CurX, CurY, 0, 0, p1Font)

                CurX = LMargin + 430
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString), "########0.00"), CurX, CurY, 0, 0, p1Font)

                CurX = LMargin + 150
                CurY = TMargin + 1000 - 5
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, CurX, CurY, 0, 0, pFont)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format5(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next
        PpSzSTS = False

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
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
        PrintDocument1.DefaultPageSettings.Landscape = False

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 30
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

        'If PrintDocument1.DefaultPageSettings.Landscape = True Then
        '    With PrintDocument1.DefaultPageSettings.PaperSize
        '        PrintWidth = .Height - TMargin - BMargin
        '        PrintHeight = .Width - RMargin - LMargin
        '        PageWidth = .Height - TMargin
        '        PageHeight = .Width - RMargin
        '    End With
        'End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 34 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 60 : ClArr(2) = 80 : ClArr(3) = 95 : ClArr(4) = 60 : ClArr(5) = 80 : ClArr(6) = 95 : ClArr(7) = 60 : ClArr(8) = 80

        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format5_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_NoofBmDets < prn_DetMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            prn_DetIndx = prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetIndx = prn_DetIndx + 1

                        CurY = CurY + TxtHgt

                        If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 12, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            ' Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 12, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            ' Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage+ NoofItems_PerPage, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage, 4)), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage+ NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        NoofDets = NoofDets + 1

                    Loop

                End If

                Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format5_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        CurY = TMargin
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
        If Trim(prn_HdDt.Rows(0).Item("Company_GstinNo").ToString) <> "" Then
            Cmp_TinNo = "GST NO: " & prn_HdDt.Rows(0).Item("Company_GstinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PIECE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "GST NO:" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(11) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(2))

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "Quatlity :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOOM NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOOM.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOOM.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format5_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim W1 As Single

        Try
            W1 = e.Graphics.MeasureString("Vehicle No    :  ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "Total Rolls ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Total Meters ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 70, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub txt_FilterPcsNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_FilterPcsNo.KeyDown
        If e.KeyCode = 40 Then btn_Filter_Show_Click(sender, e) ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_FilterPcsNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FilterPcsNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Print_PDF_Status = False
        print_record()
    End Sub
    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
        pnl_Print2.Visible = False
    End Sub

    Private Sub btn_Print_PcsWise_Delivery_With_Weight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_PcsWise_Delivery_With_Weight.Click
        prn_Status = 1
        print_Delivery()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_PcsWise_Delivery_Without_Weight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_PcsWise_Delivery_Without_Weight.Click
        prn_Status = 2
        print_Delivery()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub Transfer_Entry_To_Another_Company(ByVal sqltr As SqlClient.SqlTransaction)
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Sno As Integer = 0
        Dim i As Integer = 0
        Dim Nr As Long = 0
        Dim stkof_idno As Integer = 0
        Dim led_id As Integer = 0
        Dim Clo_id As Integer = 0
        Dim DbName As String = ""
        Dim TrnTo_CmpGrpIdNo As Integer = 0
        Dim TrnTo_CmpIdNo As Integer = 0
        Dim TrnTo_LedIdNo As Integer = 0
        Dim TrnTo_CloIdNo As Integer = 0
        Dim NewCode As String = ""
        Dim T1_Mtrs As Double = 0, T2_Mtrs As Double = 0, T3_Mtrs As Double = 0, T4_Mtrs As Double = 0, T5_Mtrs As Double = 0
        Dim Tot_Clo_Mtrs As Double = 0
        Dim dCloTyp_ID As Integer = 0
        Dim pc_wt_mtr As Double = 0
        Dim pc_wgt As Double = 0
        Dim pc_FldPerc As Double = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim TrnTo_RecIdNo As Integer = 0
        Dim ConsYarn As Double = 0
        Dim Delv_ID As Integer = 0, Rec_ID As Integer = 0
        Dim Led_type As String = ""
        Dim WftCnt_ID As Integer = 0
        Dim vLm_IdNo As Integer = 0
        Dim vWdth_Type As String = ""
        Dim PvuConsMtrs As Double = 0
        Dim TotPavuConsMtrs As Double = 0
        Dim vCrmp_Perc As Double = 0
        Dim vEndsCnt_IdNo As Integer = 0
        Dim vTrsferPartyIdno_IdNo As Integer = 0
        Dim vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption As Boolean = False



        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text, sqltr)
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Clo_id = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text, sqltr)
        If Clo_id = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled And cbo_ClothName.Visible Then cbo_ClothName.Focus()
            Exit Sub
        End If

        vTrsferPartyIdno_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_StockTransferParty.Text, sqltr, TrnTo_DbName)

        TrnTo_CmpGrpIdNo = Val(Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..CompanyGroup_Head", "Transfer_To_CompanyGroupIdNo", "(CompanyGroup_IdNo = " & Str(Val(Common_Procedures.CompGroupIdNo)) & ")", , sqltr))

        DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(TrnTo_CmpGrpIdNo)))

        TrnTo_CmpIdNo = Val(Common_Procedures.get_FieldValue(con, "Company_Head", "Transfer_To_CompanyIdNo", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & ")", , sqltr))
        TrnTo_LedIdNo = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Transfer_To_LedgerIdNo", "(Ledger_IdNo = " & Str(Val(led_id)) & ")", , sqltr))
        TrnTo_CloIdNo = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Transfer_To_ClothIdno", "(Cloth_IdNo = " & Str(Val(Clo_id)) & ")", , sqltr))
        TrnTo_RecIdNo = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Transfer_To_LedgerIdNo", "(Ledger_IdNo = " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ")", , sqltr))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1075" Then

            stkof_idno = vTrsferPartyIdno_IdNo

        Else
            stkof_idno = TrnTo_LedIdNo

        End If

        Cmd.Connection = con
        Cmd.Transaction = sqltr

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)

        Cmd.CommandText = "Delete from  " & Trim(DbName) & "..Weaver_ClothReceipt_Piece_Details Where Transfer_From_EntryCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = ''"
        Cmd.ExecuteNonQuery()

        ''---'---remove after save all - by thanges
        ''---Cmd.CommandText = "Delete from " & Trim(DbName) & "..Stock_Cloth_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        ''---Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Delete from " & Trim(DbName) & "..Stock_Cloth_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "' and Transfer_From_EntryCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        Cmd.ExecuteNonQuery()

        ''---'---remove after save all - by thanges
        ''---Cmd.CommandText = "Delete from " & Trim(DbName) & "..Stock_Pavu_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        ''---Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Delete from " & Trim(DbName) & "..Stock_Pavu_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "' and Transfer_From_EntryCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        Cmd.ExecuteNonQuery()

        ''---'---remove after save all - by thanges
        ''---Cmd.CommandText = "Delete from " & Trim(DbName) & "..Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        ''---Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Delete from " & Trim(DbName) & "..Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "' and Transfer_From_EntryCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        Cmd.ExecuteNonQuery()

        If chk_Transfer.Checked = True Then

            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempSubTable) & ""
            Cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 And Trim(dgv_Details.Rows(i).Cells(2).Value) <> "" And Trim(.Rows(i).Cells(5).Value) <> "" Then

                        dCloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(3).Value, sqltr)

                        T5_Mtrs = 0 : T4_Mtrs = 0 : T3_Mtrs = 0 : T2_Mtrs = 0 : T1_Mtrs = 0
                        If Val(dCloTyp_ID) = 5 Then
                            T5_Mtrs = T5_Mtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                        ElseIf Val(dCloTyp_ID) = 4 Then
                            T4_Mtrs = T4_Mtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                        ElseIf Val(dCloTyp_ID) = 3 Then
                            T3_Mtrs = T3_Mtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                        ElseIf Val(dCloTyp_ID) = 2 Then
                            T2_Mtrs = T2_Mtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                        Else
                            T1_Mtrs = T1_Mtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                        End If

                        Sno = Sno + 1

                        pc_FldPerc = 0
                        pc_wgt = 0
                        pc_wt_mtr = 0

                        Da1 = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details where Lot_Code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'", con)
                        Da1.SelectCommand.Transaction = sqltr
                        Dt1 = New DataTable
                        Da1.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            pc_FldPerc = Val(Dt1.Rows(0).Item("Folding").ToString)
                            pc_wgt = Val(Dt1.Rows(0).Item("Weight").ToString)
                            pc_wt_mtr = Val(Dt1.Rows(0).Item("Weight_Meter").ToString)
                        End If
                        Dt1.Clear()

                        If pc_FldPerc = 0 Then pc_FldPerc = 100

                        Nr = 0
                        Cmd.CommandText = "Update " & Trim(DbName) & "..Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Date = @EntryDate, Weaver_ClothReceipt_Date = @EntryDate, StockOff_IdNo = " & Str(Val(stkof_idno)) & ", Ledger_IdNo =  " & Val(TrnTo_LedIdNo) & ", Folding_Receipt = " & Str(Val(pc_FldPerc)) & ", Folding_Checking = " & Str(Val(pc_FldPerc)) & ", Folding = " & Str(Val(pc_FldPerc)) & ", Sl_No = " & Str(Val(Sno)) & ", ReceiptMeters_Receipt = " & Str(Val(T1_Mtrs + T2_Mtrs + T3_Mtrs + T4_Mtrs + T5_Mtrs)) & ", ReceiptMeters_Checking = " & Str(Val(T1_Mtrs + T2_Mtrs + T3_Mtrs + T4_Mtrs + T5_Mtrs)) & ", Receipt_Meters = " & Str(Val(T1_Mtrs + T2_Mtrs + T3_Mtrs + T4_Mtrs + T5_Mtrs)) & ", Type1_Meters = " & Str(Val(T1_Mtrs)) & ", Type2_Meters = " & Str(Val(T2_Mtrs)) & ", Type3_Meters = " & Str(Val(T3_Mtrs)) & ", Type4_Meters = " & Str(Val(T4_Mtrs)) & ", Type5_Meters = " & Str(Val(T5_Mtrs)) & ", Total_Checking_Meters = " & Str(Val(T1_Mtrs + T2_Mtrs + T3_Mtrs + T4_Mtrs + T5_Mtrs)) & ", Weight = " & Str(Val(pc_wgt)) & ", Weight_Meter = " & Str(Val(pc_wt_mtr)) & " Where Transfer_From_EntryCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(.Rows(i).Cells(5).Value) & "' and Lot_Code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                        Nr = Cmd.ExecuteNonQuery()

                        If Nr = 0 And (Val(T1_Mtrs) + Val(T2_Mtrs) + Val(T3_Mtrs) + Val(T4_Mtrs) + Val(T5_Mtrs)) <> 0 Then
                            Cmd.CommandText = "Insert into " & Trim(DbName) & "..Weaver_ClothReceipt_Piece_Details (   Transfer_From_EntryCode                    ,                  Weaver_Piece_Checking_Code     ,             Company_IdNo       ,     Weaver_Piece_Checking_No  ,  Weaver_Piece_Checking_Date,                  Weaver_ClothReceipt_Code                   ,              Weaver_ClothReceipt_No      ,                               for_orderby                                       ,  Weaver_ClothReceipt_Date,                    Lot_Code            ,                    Lot_No              ,         StockOff_IdNo   ,          Ledger_IdNo       ,               Cloth_IdNo       ,         Folding_Receipt     ,          Folding_Checking    ,                Folding       ,           Sl_No       ,                                 PieceNo_OrderBy                                         ,                       Main_PieceNo        ,                    Piece_No            ,                     ReceiptMeters_Receipt                         ,                 ReceiptMeters_Checking                           ,                              Receipt_Meters                      ,         Type1_Meters     ,         Type2_Meters     ,         Type3_Meters     ,          Type4_Meters    ,          Type5_Meters    ,                        Total_Checking_Meters                     ,            Weight       ,        Weight_Meter          ) " &
                                                "                            Values                                 ( '" & Trim(Pk_Condition) & Trim(NewCode) & "' ,    '" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(TrnTo_CmpIdNo)) & ",  '" & Trim(lbl_DcNo.Text) & "',         @EntryDate        , '" & Trim(Pk_Condition) & Trim(.Rows(i).Cells(5).Value) & "',   '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(1).Value))) & ",          @EntryDate      , '" & Trim(.Rows(i).Cells(5).Value) & "', '" & Trim(.Rows(i).Cells(1).Value) & "',  " & Val(stkof_idno) & ", " & Val(TrnTo_LedIdNo) & " , " & Str(Val(TrnTo_CloIdNo)) & ", " & Str(Val(pc_FldPerc)) & ",  " & Str(Val(pc_FldPerc)) & ",  " & Str(Val(pc_FldPerc)) & ",  " & Str(Val(Sno)) & ",   " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Trim(.Rows(i).Cells(2).Value)))) & ",  " & Str(Val(.Rows(i).Cells(2).Value)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "',  " & Str(Val(T1_Mtrs + T2_Mtrs + T3_Mtrs + T4_Mtrs + T5_Mtrs)) & ", " & Str(Val(T1_Mtrs + T2_Mtrs + T3_Mtrs + T4_Mtrs + T5_Mtrs)) & ", " & Str(Val(T1_Mtrs + T2_Mtrs + T3_Mtrs + T4_Mtrs + T5_Mtrs)) & ", " & Str(Val(T1_Mtrs)) & ", " & Str(Val(T2_Mtrs)) & ", " & Str(Val(T3_Mtrs)) & ", " & Str(Val(T4_Mtrs)) & ", " & Str(Val(T5_Mtrs)) & ", " & Str(Val(T1_Mtrs + T2_Mtrs + T3_Mtrs + T4_Mtrs + T5_Mtrs)) & ", " & Str(Val(pc_wgt)) & ",  " & Str(Val(pc_wt_mtr)) & " ) "
                            Cmd.ExecuteNonQuery()
                        End If

                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(currency1, meters1, meters2, meters3, meters4, meters5) Values ( " & Str(Val(pc_FldPerc)) & ", " & Str(Val(T1_Mtrs)) & ", " & Str(Val(T2_Mtrs)) & ", " & Str(Val(T3_Mtrs)) & ", " & Str(Val(T4_Mtrs)) & ", " & Str(Val(T5_Mtrs)) & ")"
                        Cmd.ExecuteNonQuery()


                        vLm_IdNo = 0
                        vWdth_Type = ""
                        vCrmp_Perc = 0
                        vEndsCnt_IdNo = 0
                        Da1 = New SqlClient.SqlDataAdapter("Select * from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(.Rows(i).Cells(5).Value) & "'", con)
                        Da1.SelectCommand.Transaction = sqltr
                        Dt1 = New DataTable
                        Da1.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            vLm_IdNo = Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)
                            vWdth_Type = Dt1.Rows(0).Item("Width_Type").ToString
                            vCrmp_Perc = Val(Dt1.Rows(0).Item("Crimp_Percentage").ToString)
                            vEndsCnt_IdNo = Val(Dt1.Rows(0).Item("EndsCount_IdNo").ToString)
                        End If
                        Dt1.Clear()

                        If vEndsCnt_IdNo <> 0 Then

                            vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption = True
                            If Common_Procedures.settings.JobWorker_PavuWidthWiseConsumption_IN_Delivery = 1 Then
                                vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption = False
                            End If

                            PvuConsMtrs = Common_Procedures.get_Pavu_Consumption(con, TrnTo_CloIdNo, vLm_IdNo, (Val(T1_Mtrs) + Val(T2_Mtrs) + Val(T3_Mtrs) + Val(T4_Mtrs) + Val(T5_Mtrs)), vWdth_Type, sqltr, Val(vCrmp_Perc), , vCalc_AutoLoom_JbWrk_PavuWidthWiseConsumption)

                            Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, meters1) Values ( " & Str(Val(vEndsCnt_IdNo)) & ", " & Str(Val(PvuConsMtrs)) & ")"
                            Cmd.ExecuteNonQuery()

                        End If

                    End If

                Next

            End With

            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)
            Partcls = "Trans : JbWrk Pcs Delv : Dc.No. " & Trim(lbl_DcNo.Text)

            Sno = 0
            Tot_Clo_Mtrs = 0
            Da1 = New SqlClient.SqlDataAdapter("select currency1 as Folding, sum(meters1) as Type1Mtrs, sum(meters2) as Type2Mtrs, sum(meters3) as Type3Mtrs, sum(meters4) as Type4Mtrs, sum(meters5) as Type5Mtrs from " & Trim(Common_Procedures.EntryTempTable) & " group by currency1 having sum(meters1) <> 0 or sum(meters2) <> 0 or sum(meters3) <> 0 or sum(meters4) <> 0 or sum(meters5) <> 0", con)
            Da1.SelectCommand.Transaction = sqltr
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1
                    Sno = Sno + 1
                    Cmd.CommandText = "Insert into  " & Trim(DbName) & "..Stock_Cloth_Processing_Details (          Transfer_From_EntryCode           ,                Transfer_From_CompanyGroupIdNo    ,                Reference_Code                ,             Company_IdNo       ,              Reference_No    ,                               for_OrderBy                             , Reference_Date,       StockOff_IdNo    ,                               DeliveryTo_Idno             ,        ReceivedFrom_Idno         ,          Entry_ID     ,       Party_Bill_No   ,      Particulars         ,            Sl_No     ,             Cloth_Idno         ,                          Folding                       , UnChecked_Meters,                         Meters_Type1                    ,                          Meters_Type2                   ,                          Meters_Type3                   ,                         Meters_Type4                    ,                           Meters_Type5                   ) " &
                                                "                          Values                        ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.CompGroupIdNo)) & ", '" & Trim(Pk_Condition2) & Trim(NewCode) & "', " & Str(Val(TrnTo_CmpIdNo)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",     @EntryDate, " & Val(stkof_idno) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ",   " & Str(Val(TrnTo_RecIdNo)) & ",  '" & Trim(EntID) & "',  '" & Trim(PBlNo) & "',  '" & Trim(Partcls) & "' , " & Str(Val(Sno)) & ", " & Str(Val(TrnTo_CloIdNo)) & ",  " & Str(Val(Dt1.Rows(i).Item("Folding").ToString)) & ",        0        , " & Str(Val(Dt1.Rows(i).Item("Type1Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type2Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type3Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type4Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type5Mtrs").ToString)) & " ) "
                    Cmd.ExecuteNonQuery()

                    Tot_Clo_Mtrs = Tot_Clo_Mtrs + Val(Dt1.Rows(i).Item("Type1Mtrs").ToString) + Val(Dt1.Rows(i).Item("Type2Mtrs").ToString) + Val(Dt1.Rows(i).Item("Type3Mtrs").ToString) + Val(Dt1.Rows(i).Item("Type4Mtrs").ToString) + Val(Dt1.Rows(i).Item("Type5Mtrs").ToString)

                Next
            End If
            Dt1.Clear()

            Led_type = Common_Procedures.get_FieldValue(con, Trim(DbName) & "..Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(TrnTo_RecIdNo)) & ")", , sqltr)

            Delv_ID = 0 : Rec_ID = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                Delv_ID = TrnTo_LedIdNo
                Rec_ID = 0
            Else
                Delv_ID = 0
                Rec_ID = TrnTo_LedIdNo
            End If

            WftCnt_ID = Common_Procedures.get_FieldValue(con, Trim(DbName) & "..Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_IdNo = " & Str(Val(TrnTo_CloIdNo)) & ")", , sqltr)
            ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, TrnTo_CloIdNo, Val(Tot_Clo_Mtrs), sqltr)

            If Val(ConsYarn) <> 0 Then
                Cmd.CommandText = "Insert into " & Trim(DbName) & "..Stock_Yarn_Processing_Details (          Transfer_From_EntryCode           ,                Transfer_From_CompanyGroupIdNo    ,                Reference_Code                ,                Company_IdNo    ,             Reference_No     ,                               for_OrderBy                             , Reference_Date, DeliveryTo_Idno,           ReceivedFrom_Idno    ,           Entry_ID   ,           Particulars  ,      Party_Bill_No   , Sl_No,          Count_IdNo        , Yarn_Type, Mill_IdNo, Bags, Cones,                    Weight                            ) " &
                                  "                                  Values                        ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.CompGroupIdNo)) & ", '" & Trim(Pk_Condition2) & Trim(NewCode) & "', " & Str(Val(TrnTo_CmpIdNo)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",    @EntryDate ,           0    , " & Str(Val(TrnTo_RecIdNo)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "',   1  , " & Str(Val(WftCnt_ID)) & ",    'MILL',     0    ,   0 ,    0 , " & Str(Format(Val(ConsYarn), "#########0.000")) & " ) "
                Cmd.ExecuteNonQuery()
            End If

            Sno = 0
            Tot_Clo_Mtrs = 0
            Da1 = New SqlClient.SqlDataAdapter("select Int1 as Endscount_IdNo, sum(meters1) as PavuMtrs from " & Trim(Common_Procedures.EntryTempSubTable) & " group by Int1 having sum(meters1) <> 0", con)
            Da1.SelectCommand.Transaction = sqltr
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    Sno = Sno + 1

                    Cmd.CommandText = "Insert into " & Trim(DbName) & "..Stock_Pavu_Processing_Details (          Transfer_From_EntryCode           ,                Transfer_From_CompanyGroupIdNo    ,           Reference_Code                     ,                 Company_IdNo   ,            Reference_No      ,                               for_OrderBy                             , Reference_Date,  DeliveryTo_Idno,       ReceivedFrom_Idno        ,        Cloth_Idno              ,          Entry_ID    ,      Party_Bill_No   ,          Particulars   ,           Sl_No      ,                         EndsCount_IdNo                       , Sized_Beam,                         Meters                          ) " &
                                      "                                  Values                        ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.CompGroupIdNo)) & ", '" & Trim(Pk_Condition2) & Trim(NewCode) & "', " & Str(Val(TrnTo_CmpIdNo)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",    @EntryDate ,          0      , " & Str(Val(TrnTo_RecIdNo)) & ", " & Str(Val(TrnTo_CloIdNo)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("Endscount_IdNo").ToString)) & ",      0    , " & Str(Val(Dt1.Rows(i).Item("PavuMtrs").ToString)) & " ) "
                    Cmd.ExecuteNonQuery()

                Next
            End If
            Dt1.Clear()

        End If

    End Sub

    Private Sub cbo_StockTransferParty_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_StockTransferParty.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, TrnTo_DbName & "..Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or  Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_StockTransferParty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_StockTransferParty.KeyDown
        ' vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_StockTransferParty, txt_Remarks, Nothing, TrnTo_DbName & "..Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or  Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_StockTransferParty.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_StockTransferParty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_StockTransferParty.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_StockTransferParty, Nothing, TrnTo_DbName & "..Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub
    Private Sub txt_PcsSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PcsSelction.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
        If (e.KeyValue = 38) Then txt_LotSelction.Focus()

    End Sub

    Private Sub txt_PcsSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsSelction.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_PcsSelction.Text) <> "" Or Trim(txt_PcsSelction.Text) <> "" Then
                btn_lot_Pcs_selection_Click(sender, e)

            Else
                If dgv_Selection.Rows.Count > 0 Then
                    dgv_Selection.Focus()
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                    dgv_Selection.CurrentCell.Selected = True
                End If

            End If

        End If
    End Sub

    Private Sub txt_LotSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LotSelction.KeyDown
        If (e.KeyValue = 40) Then
            txt_PcsSelction.Focus()
        End If
    End Sub

    Private Sub txt_LotSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LotSelction.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_PcsSelction.Focus()
        End If
    End Sub

    Private Sub btn_lot_Pcs_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_lot_Pcs_selection.Click
        Dim LtNo As String
        Dim PcsNo As String
        Dim i As Integer

        If Trim(txt_LotSelction.Text) <> "" And Trim(txt_PcsSelction.Text) <> "" Then

            LtNo = Trim(txt_LotSelction.Text)
            PcsNo = Trim(txt_PcsSelction.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) And Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                    Call Select_Piece(i)
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 9 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 8
                    Exit For
                End If
            Next

            txt_LotSelction.Text = ""
            txt_PcsSelction.Text = ""
            If txt_LotSelction.Enabled = True Then txt_LotSelction.Focus()

        ElseIf Trim(txt_PcsSelction.Text) <> "" Then

            PcsNo = Trim(txt_PcsSelction.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                    Call Select_Piece(i)
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 9 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 8
                    Exit For
                End If
            Next

            txt_LotSelction.Text = ""
            txt_PcsSelction.Text = ""
            If txt_PcsSelction.Enabled = True Then txt_PcsSelction.Focus()

        End If


    End Sub

    Private Sub Grid_Selection(ByVal RwIndx As Integer)
        'Dim i As Integer

        'With dgv_Selection

        '    If .RowCount > 0 And RwIndx >= 0 Then

        '        .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

        '        If Val(.Rows(RwIndx).Cells(8).Value) = 0 Then

        '            .Rows(RwIndx).Cells(8).Value = ""

        '            For i = 0 To .ColumnCount - 1
        '                .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
        '            Next

        '        Else
        '            For i = 0 To .ColumnCount - 1
        '                .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
        '            Next

        '        End If

        '    End If
        '    If txt_LotSelction.Enabled = True Then txt_LotSelction.Focus()

        'End With

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
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
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

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub Printing_Format_1233(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim new_code As String = ""

        Dim da1 As New SqlClient.SqlDataAdapter

        Dim dt1 As New DataTable
        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If




        PpSzSTS = False

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    'Debug.Print(ps.PaperName)
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
        '    End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 20
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

        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 30 ' 32 '34 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 88 : ClArr(2) = 60 : ClArr(3) = 70 : ClArr(4) = 70 : ClArr(5) = 62
        ClArr(6) = 88 : ClArr(7) = 60 : ClArr(8) = 70 : ClArr(9) = 70
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        'ClArr(1) = 63 : ClArr(2) = 75 : ClArr(3) = 75 : ClArr(4) = 75
        'ClArr(5) = 62 : ClArr(6) = 75 : ClArr(7) = 75 : ClArr(8) = 75 : ClArr(9) = 75
        'ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))
        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' Try
        new_code = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'Next  select j.* from  JobWork_Production_Head j where j.company_idno=  1  and j.JobWork_Delivery_Code = '1-1/20-21'
        da1 = New SqlClient.SqlDataAdapter("select j.*,L.Loom_name from  JobWork_Production_Head j left outer join Loom_Head L On L.Loom_Idno=j.Loom_idno where j.company_idno=  " & Str(Val(lbl_Company.Tag)) & "  and j.JobWork_Delivery_Code = '" & Trim(new_code) & "'order by j.for_orderby", con)
        'da2 = New SqlClient.SqlDataAdapter("select a.*  from JobWork_Piece_Delivery_Details a  where JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
        dt1 = New DataTable
        da1.Fill(dt1)
        If prn_HdDt.Rows.Count > 0 Then

            Printing_Format_1233_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

            NoofDets = 0

            CurY = CurY - 10

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_NoofBmDets < prn_DetMxIndx

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_Format_1233_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                        e.HasMorePages = True

                        Return

                    End If

                    prn_DetIndx = prn_DetIndx + 1
                    CurY = CurY + TxtHgt

                    If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then


                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1233" Then '----Vipin textile Somanur
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + 10, CurY, 0, 0, pFont) 'pcs_no
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + 10, CurY, 0, 0, pFont) 'pcs_no
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1233" Then '----Vipin textile Somanur
                            If Val(prn_DetAr(prn_DetIndx, 8)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 8)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx, 8)), "#########0.000")
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx, 6)), "#########0.000")
                            End If

                            'If dt1.Rows.Count > 0 Then
                            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                            'End If

                        Else
                            If Val(prn_DetAr(prn_DetIndx, 8)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 8)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx, 8)), "#########0.000")
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx, 6)), "#########0.000")
                            End If

                        End If

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1420" Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 7)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        End If

                        prn_NoofBmDets = prn_NoofBmDets + 1


                    End If

                    If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then


                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1233" Then '----Vipin textile Somanur
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont) 'pcs_no
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont) 'pcs_no
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1233" Then '----Vipin textile Somanur
                            If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)), "#########0.000")
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.000")
                            End If

                        Else
                            If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)), "#########0.000")
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.000")
                            End If
                        End If

                        'If dt1.Rows.Count > 0 Then
                        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                        'End If

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1420" Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 10, CurY, 1, 0, pFont)
                        End If
                        prn_NoofBmDets = prn_NoofBmDets + 1
                    End If

                    NoofDets = NoofDets + 1
                Loop

            End If

            dt1.Clear()

            Printing_Format_1233_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

        End If


        ' Catch ex As Exception

        ' MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_1233_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTIN_No As String

        PageNo = PageNo + 1

        CurY = TMargin


        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        CurY = TMargin
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

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt - 18
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 60, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + 70, CurY, 0, 0, p1Font)

        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PIECE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 60, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + 70, CurY, 0, 0, pFont)

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        'p1Font = New Font("Calibri", 14, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO :M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + M1 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(2))

        CurY = CurY + 10

        If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Remarks").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
        End If

        Common_Procedures.Print_To_PrintDocument(e, "Quatlity :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin, CurY, 2, ClAr(1), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        CurY = CurY + 15
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "/MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "/MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format_1233_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim W1 As Single

        Try
            W1 = e.Graphics.MeasureString("Vehicle No    :  ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Total Rolls : " & Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total Meters : " & prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            If Val(Total_Weight) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Total Weight : " & Format(Val(Total_Weight), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_Weight), "#########0.000"), LMargin + W1 + 25, CurY, 0, 0, pFont)
            End If

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Total Meters ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If


            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Common_Procedures.settings.CustomerCode <> "1420" Then
                Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
            End If
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            If Common_Procedures.settings.CustomerCode = "1420" Then
                CurY = CurY + TxtHgt + 25
                Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Authorised Signature", PageWidth - 5, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Amount()
        With dgv_Details_Total

            If .RowCount <> 0 Then


                lbl_total_amount.Text = Format(Val(txt_rate.Text) * Val(.Rows(0).Cells(4).Value), "########0.00")
            End If
        End With
    End Sub
    Private Sub txt_rate_TextChanged(sender As Object, e As EventArgs) Handles txt_rate.TextChanged
        If FrmLdSTS = True Then Exit Sub

        'With dgv_Details_Total

        '    If .RowCount <> 0 Then


        '        lbl_total_amount.Text = Format(Val(txt_rate.Text) * Val(.Rows(0).Cells(4).Value), "########0.00")
        '    End If
        'End With

        Amount()

    End Sub


    Private Sub Printing_GST_Format_1186(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim ItmNm1 As String, ItmNm2 As String


        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        PpSzSTS = False

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
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
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 30
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

        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 10 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        'ClArr(1) = 50 : ClArr(2) = 350 : ClArr(3) = 95 : ClArr(4) = 95

        'ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))



        ClArr(1) = 50 : ClArr(2) = 280 : ClArr(3) = 70 : ClArr(4) = 95 : ClArr(5) = 95

        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' Try

        If prn_HdDt.Rows.Count > 0 Then

            Printing_GST_Format_1186_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

            NoofDets = 0

            CurY = CurY - 10

            If prn_HdDt.Rows.Count > 0 Then

                Do While prn_NoofBmDets <= prn_HdDt_1.Rows.Count - 1

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_GST_Format_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                        'prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                        e.HasMorePages = True

                        Return

                    End If
                    Print_sno = Print_sno + 1

                    CurY = CurY + TxtHgt
                    If Trim(prn_HdDt_1.Rows(prn_DetIndx).Item("po_no").ToString) <> "" Then

                        ItmNm1 = Trim(prn_HdDt_1.Rows(prn_DetIndx).Item("Cloth_Name").ToString) & Trim(prn_HdDt_1.Rows(prn_DetIndx).Item("wEAVE").ToString) & " - " & Trim(prn_HdDt_1.Rows(prn_DetIndx).Item("po_no").ToString)
                    Else
                        ItmNm1 = Trim(prn_HdDt_1.Rows(prn_DetIndx).Item("Cloth_Name").ToString) & Trim(prn_HdDt_1.Rows(prn_DetIndx).Item("wEAVE").ToString)
                    End If

                    ItmNm2 = ""
                    If Len(ItmNm1) > 35 Then
                        For I = 35 To 1 Step -1
                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 35
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                    End If

                    Common_Procedures.Print_To_PrintDocument(e, Print_sno, LMargin + 10, CurY, 0, 0, pFont)
                    'Total_Delivery_Meters

                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_1.Rows(prn_DetIndx).Item("Mtrs").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_1.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_1.Rows(prn_DetIndx).Item("total").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Total_Mtr = Total_Mtr + prn_HdDt_1.Rows(prn_DetIndx).Item("Mtrs").ToString

                    If Trim(ItmNm2) <> "" Then
                        CurY = CurY + TxtHgt - 5
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        NoofDets = NoofDets + 1
                    End If

                    prn_DetIndx = prn_DetIndx + 1

                    prn_NoofBmDets = prn_NoofBmDets + 1


                    NoofDets = NoofDets + 1

                Loop

            End If

            Printing_GST_Format_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

        End If

        ' Catch ex As Exception

        ' MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format_1186_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, city As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
        Dim Cmp_State As String, Cmp_StateCode As String, Cmp_GSTIN_No As String, Cmp_EMail As String

        PageNo = PageNo + 1
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY CHALLAN / PACKING LIST", LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : city = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = ""
        Cmp_State = "" : Cmp_StateCode = "" : Cmp_GSTIN_No = "" : Cmp_EMail = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "EMAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_City").ToString) <> "" Then
            City = "" & prn_HdDt.Rows(0).Item("Company_City").ToString
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)


        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & "," & City, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "/" & Cmp_CstNo, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "/ " & Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(12) = CurY

        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DC No. : " & prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + ClAr(1) + 90, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "No.Of Pcs : " & Format(Val(prn_HdDt.Rows(0).Item("total_Rolls").ToString), "######0"), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Vehicle No. : " & prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + 80, CurY, LMargin + ClAr(1) + 80, LnAr(12))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(12))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(12))
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("TRANSPORT DETAILS :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3)
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Italic Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "BILLED TO  :", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO : ", LMargin + M1 + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "  M/s. " & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + M1 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + M1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + M1 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + M1 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + M1 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(0).Item("State_name").ToString & "   CODE : " & prn_HdDt.Rows(0).Item("State_Code").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " STATE :   " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "     CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + M1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "GSTIN :  " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + M1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))

        CurY = CurY + 5
        Common_Procedures.Print_To_PrintDocument(e, "SL.NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, " QTY.METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE / MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT (Rs.)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_GST_Format_1186_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim CurY1 As Single = 0
        Dim D1 As Single = 0
        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width
        D1 = ClAr(1) + ClAr(2) + ClAr(3)
        Try

            For I = NoofDets + 1 To NoofItems_PerPage - 1

                CurY = CurY + TxtHgt

                'prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            pFont = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt - 10
            ' If is_LastPage = True Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Total_Mtr, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont,, True)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                'prn_TotMtrs = 0
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            'p1Font = New Font("Calibri", 13, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)

            Erase BnkDetAr
            If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                BInc = -1

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm1 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm2 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm3 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm4 = Trim(BnkDetAr(BInc))
                End If

            End If

            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            CurY1 = CurY
            'If is_LastPage = True Then
            '    'Left Side
            '    p1Font = New Font("Calibri", 11, FontStyle.Bold Or FontStyle.Underline)
            '    Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + 10, CurY1, 0, 0, p1Font)
            '    CurY1 = CurY1 + TxtHgt
            '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p1Font)
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p1Font)
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p1Font)
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p1Font)

            'End If


            'Right Side
            'CurY = CurY + TxtHgt - 10
            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Frieght", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If


            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Then '---- KALAIMAGAL TEX
            '            Common_Procedures.Print_To_PrintDocument(e, "Trade Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Else
            '            Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        End If
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If

            '    If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            '        CurY = CurY + TxtHgt + 5
            '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            '    End If
            'End If


            'CurY = CurY - 10

            'CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If
            'CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "CGST " & Val(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If
            'CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "SGST " & Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If
            'CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "IGST " & Val(prn_HdDt.Rows(0).Item("IGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If





            'CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            '    p1Font = New Font("Calibri", 13, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, "(Textile manufactring service (Weaving) )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 20, CurY, 1, 0, p1Font)
            'End If

            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "RoundOff", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If

            'End If


            'CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt + 5


            If is_LastPage = True Then
                pFont = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Total Assessable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("total_amount").ToString)), PageWidth - 10, CurY, 1, 0, p1Font)
            End If

            pFont = New Font("Calibri", 10, FontStyle.Regular)
            Dim len1 As Single = 0
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(7))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(9))

            CurY = CurY + TxtHgt - 5

            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("total_amount").ToString))

                p1Font = New Font("Calibri", 9, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(12) = CurY
            p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
            len1 = e.Graphics.MeasureString("Due Days / Date  : ", pFont).Width
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "INWARD / OUTWARD  : ", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + D1, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 5
            pFont = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Ref No   ", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, ":  " & prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + len1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME      :  " & BankNm1, LMargin + D1, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "Entry  ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":  _________________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT NO    :  " & BankNm2, LMargin + D1, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5


            Common_Procedures.Print_To_PrintDocument(e, "Date ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":  _________________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BRANCH NAME :  " & BankNm3, LMargin + D1, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt + 5

            Common_Procedures.Print_To_PrintDocument(e, "Time   ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":  _________________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE          :  " & BankNm4, LMargin + D1, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(13) = CurY

            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Goods Description : ", LMargin + D1, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "1. Overdue Interest will be charged at 24% from The invoice date. ", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "1. Uncalendered Grey Fabrics ", LMargin + D1, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "2. Our risk & responsibility ceases on goods leaving our factory.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "2. Goods Not for Sale.Goods Sent After Job Work Completion", LMargin + D1, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "3. Goods are supplied under our firm conditions.", LMargin + 10, CurY, 0, 0, pFont)
            e.Graphics.DrawLine(Pens.Black, LMargin + D1 - 10, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "4. Subject to Tirupur jurisdiction. ", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Tax Is Payable On Reverse Charge : YES / NO", LMargin + D1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "5. E.O & E .", LMargin + 10, CurY, 0, 0, pFont)



            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + D1 - 10, CurY, LMargin + D1 - 10, LnAr(12))


            CurY = CurY + 5
            Common_Procedures.Print_To_PrintDocument(e, "Certified that particulars given above are true and correct and the amount indicated represents the price actually charged and that ", LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "there is no flow of additional consideration directly or indirectly from the buyer.", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then
                p1Font = New Font("Elephant", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            Else
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Then
                CurY = CurY + TxtHgt
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.vijay_tex_Sign2, Drawing.Image), PageWidth - 110, CurY, 90, 55)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            '        CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 350, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_Print_Delivery_Click(sender As Object, e As EventArgs) Handles btn_Print_Delivery.Click
        prn_Status = 1
        print_Delivery()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Bale_Click(sender As Object, e As EventArgs) Handles btn_Print_Bale.Click
        prn_Status = 2
        print_Delivery()
        btn_print_Close_Click(sender, e)
    End Sub



    Private Sub Printing_Format_packingSlip_1186(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim da4 As New SqlClient.SqlDataAdapter

        Dim dt4 As New DataTable

        Total_Weight = 0

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        PpSzSTS = False

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
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
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 30
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

        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            NoofItems_PerPage = 24
        Else
            NoofItems_PerPage = 34
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 65 : ClArr(2) = 60 : ClArr(3) = 130 : ClArr(4) = 70
        ClArr(5) = 70 : ClArr(6) = 170 : ClArr(7) = 90
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 17 '18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' Try

        If prn_HdDt.Rows.Count > 0 Then

            Printing_Format_packingSlip_1186_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

            NoofDets = 0

            CurY = CurY - 10

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_NoofBmDets < prn_DetMxIndx

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_Format_packingSlip_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                        'prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                        e.HasMorePages = True

                        Return

                    End If

                    prn_DetIndx = prn_DetIndx + 1

                    CurY = CurY + TxtHgt

                    'With dgv_Details
                    '    da4 = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Piece_Details a  where a.Lot_code = '" & Trim(.Rows(0).Cells(5).Value) & "' Order by a.Sl_No", con)
                    '    dt4 = New DataTable
                    '    da4.Fill(dt4)
                    '    If dt4.Rows.Count > 0 Then
                    '        For I = 0 To dt4.Rows.Count - 1
                    If Trim(prn_DetAr(prn_DetIndx, 9)) <> Trim(prn_DetAr(prn_DetIndx - 1, 9)) Then
                        CurY = CurY + 5
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)

                    ' prn_NoofBmDets = prn_NoofBmDets + 1




                    Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 12, CurY, 0, 0, pFont)


                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 10)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 11)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                    '            ''Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)
                    '        Next I
                    '    End If
                    '    dt4.Clear()
                    Total_Weight = Total_Weight + Format(Val(prn_DetAr(prn_DetIndx, 8)), "#########0.00")

                    'End With
                    prn_NoofBmDets = prn_NoofBmDets + 1



                    NoofDets = NoofDets + 1

                Loop

            End If

            Printing_Format_packingSlip_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

        End If

        ' Catch ex As Exception

        ' MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_packingSlip_1186_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)

        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, City As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
        Dim Cmp_State As String, Cmp_StateCode As String, Cmp_GSTIN_No As String, Cmp_EMail As String

        PageNo = PageNo + 1

        CurY = TMargin


        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : City = ""

        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = ""
        Cmp_State = "" : Cmp_StateCode = "" : Cmp_GSTIN_No = "" : Cmp_EMail = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "EMAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_City").ToString) <> "" Then
            City = "" & prn_HdDt.Rows(0).Item("Company_City").ToString
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)


        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & "," & City, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "/" & Cmp_CstNo, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "/ " & Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        CurY = CurY + TxtHgt - 12
        If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            Common_Procedures.Print_To_PrintDocument(e, "PACKING LIST / DELIVERY CHALLAN", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "PIECE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("DESPATCH TO :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 20
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "DESPATCH TO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatched_To").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
            ' CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(2))

        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "Quality :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Weave :  " & prn_HdDt.Rows(0).Item("weave").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 40, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "SL.NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM ", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS/ROLL.NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PO NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

        CurY = CurY + 15
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format_packingSlip_1186_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim W1 As Single

        Try
            W1 = e.Graphics.MeasureString("Vehicle No    :  ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(Total_Weight), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "Total Rolls ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + W1 + 25, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Total Meters ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            'If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            'End If
            If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
                CurY = CurY + TxtHgt - 10
                Dim len1 As Integer = 0

                p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
                len1 = e.Graphics.MeasureString(" Date     : ", pFont).Width
                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "INWARD / OUTWARD  : ", LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt + 5
                pFont = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Ref No   ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  " & prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + len1 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, "Entry  ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  __________________", LMargin + len1 + 10, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 5

                Common_Procedures.Print_To_PrintDocument(e, "Date ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  __________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 5

                Common_Procedures.Print_To_PrintDocument(e, "Time   ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  __________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 10
            End If


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)



            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            CurY = CurY + TxtHgt + 5
            CurY = CurY + TxtHgt + 5

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
            End If




            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 5, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_pdf_Click(sender As Object, e As EventArgs) Handles btn_pdf.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
    End Sub


    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_ClothName, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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

    Private Sub dgtxt_Details_Enter(sender As Object, e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()

    End Sub
    Private Sub dgtxt_Details_KeyDown(sender As Object, e As KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            If .Visible Then
                If .Rows.Count > 0 Then
                    If Trim(.CurrentRow.Cells(5).Value) <> "" Then
                        e.Handled = True
                        e.SuppressKeyPress = True

                    End If
                End If
            End If
        End With
    End Sub



    Private Sub dgtxt_Details_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .Rows.Count > 0 Then
                    If Trim(.CurrentRow.Cells(5).Value) <> "" Then
                        e.Handled = True

                    Else
                        If .CurrentCell.ColumnIndex = 4 Then
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

    '---------------------------------------------------


    Private Sub Printing_Format_1420(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        PpSzSTS = False

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
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
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 30
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

        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            NoofItems_PerPage = 28
        Else
            NoofItems_PerPage = 33 ' 34
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 60 : ClArr(2) = 70 : ClArr(3) = 100 : ClArr(4) = 110
        ClArr(5) = 65 : ClArr(6) = 80 : ClArr(7) = 110
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' Try

        If prn_HdDt.Rows.Count > 0 Then

            Printing_Format_1420_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

            NoofDets = 0

            CurY = CurY - 10

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_NoofBmDets < prn_DetMxIndx

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_Format_1420_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                        e.HasMorePages = True

                        Return

                    End If

                    prn_DetIndx = prn_DetIndx + 1

                    CurY = CurY + TxtHgt

                    If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)

                        prn_NoofBmDets = prn_NoofBmDets + 1

                    End If

                    If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 12, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                        prn_NoofBmDets = prn_NoofBmDets + 1

                    End If

                    NoofDets = NoofDets + 1

                Loop

            End If

            Printing_Format_1420_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

        End If

        ' Catch ex As Exception

        ' MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_1420_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin


        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        CurY = TMargin
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
        If Trim(prn_HdDt.Rows(0).Item("Company_GstinNo").ToString) <> "" Then
            Cmp_TinNo = "GST NO: " & prn_HdDt.Rows(0).Item("Company_GstinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            Common_Procedures.Print_To_PrintDocument(e, "PACKING SLIP", LMargin, CurY, 2, PrintWidth, p1Font)
        ElseIf Common_Procedures.settings.CustomerCode = "1420" Then
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY CHALLAN", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "PIECE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        ' e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("SET NO     :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        If Common_Procedures.settings.CustomerCode = "1420" Then
            Common_Procedures.Print_To_PrintDocument(e, "DESPATCH THROUGH", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "DESPATCH TO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatched_To").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, LnAr(2))

        CurY = CurY + 10

        If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Remarks").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
        End If


        If Common_Procedures.settings.CustomerCode = "1420" Then

            Common_Procedures.Print_To_PrintDocument(e, "Quatlity :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString & "               Total Rolls :  " & prn_HdDt.Rows(0).Item("Total_Rolls").ToString & "              Total Meters :  " & prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + 10, CurY, 0, 0, pFont)
            '
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Quatlity :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "SL.NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS.NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4) + ClAr(5), pFont)


        Common_Procedures.Print_To_PrintDocument(e, "SL.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format_1420_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim W1 As Single

        Try
            W1 = e.Graphics.MeasureString("Vehicle No    :  ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

            If Common_Procedures.settings.CustomerCode <> "1420" Then

                CurY = CurY + TxtHgt

                Common_Procedures.Print_To_PrintDocument(e, "Total Rolls ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + W1 + 25, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Total Meters ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            End If

            If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            End If
            If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
                CurY = CurY + TxtHgt
                Dim len1 As Integer = 0

                p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
                len1 = e.Graphics.MeasureString(" Date     : ", pFont).Width
                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "INWARD / OUTWARD  : ", LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt + 5
                pFont = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Ref No   ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  " & prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + len1 + 10, CurY, 0, 0, pFont)
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, "Entry  ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  __________________", LMargin + len1 + 10, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 5

                Common_Procedures.Print_To_PrintDocument(e, "Date ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  __________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 5

                Common_Procedures.Print_To_PrintDocument(e, "Time   ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  __________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 10
            ElseIf Common_Procedures.settings.CustomerCode = "1420" Then

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

                CurY = CurY + TxtHgt + 5

                CurY = CurY + TxtHgt + 5
                CurY = CurY + TxtHgt + 5

            End If


            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If


            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 70, CurY, 0, 0, pFont)

            If Common_Procedures.settings.CustomerCode <> "1186" And Common_Procedures.settings.CustomerCode <> "1420" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1544" Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)
            End If
            If Common_Procedures.settings.CustomerCode = "1186" Or Common_Procedures.settings.CustomerCode = "1420" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
                Common_Procedures.Print_To_PrintDocument(e, "Authorised Signature", PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click


        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        'Grp_EWB.Location = New Point(62, 184)   '(65, 156)                '(59, 145)

        'Grp_EWB.Left = (Me.Width - pnl_Back.Width) / 2 + 250
        'Grp_EWB.Top = (Me.Height - pnl_Back.Height) / 2 + 200


        'btn_GENERATEEWB.Enabled = True
        'Grp_EWB.Visible = True
        'Grp_EWB.BringToFront()
        Grp_EWB.Left = (Me.Width - pnl_Back.Width) / 2 + 250
        Grp_EWB.Top = (Me.Height - pnl_Back.Height) / 2 + 200


    End Sub

    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        '   Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select Eway_BillNo from JobWork_Piece_Delivery_Head where JobWork_Piece_Delivery_Code = '" & NewCode & "'", con)
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
                         "  SELECT               'O'              , '4'             ,   'JOB WORK'        ,    'CHL'    , a.JobWork_Piece_Delivery_No ,a.JobWork_Piece_Delivery_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
                         " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo, L.Ledger_MainName, (case when a.Delivery_Idno <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.Delivery_Idno <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.Delivery_Idno <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.Delivery_Idno <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee, TS.State_Code, (case when a.Delivery_Idno <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode," &
                         " 1                     , 0 , a.Total_Amount     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
                         " a.Vehicle_No, 'R', '" & NewCode & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from JobWork_Piece_Delivery_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L ON a.Ledger_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.Delivery_Idno <> 0 and a.Delivery_Idno = tDELV.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo " &
                         " where a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
        CMD.ExecuteNonQuery()

        'vSgst = 

        CMD.CommandText = " Update EWB_Head Set CGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , SGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        CMD.ExecuteNonQuery()


        CMD.CommandText = " Update EWB_Head Set TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        CMD.ExecuteNonQuery()


        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        Dim dt1 As New DataTable

        da = New SqlClient.SqlDataAdapter(" Select  I.Cloth_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.Item_GST_Percentage, sum(SD.Total_Amount) As TaxableAmt,sum(SD.Total_Delivery_Meters) as Qty, 1 , 'MTR' AS Units " &
                                          " from JobWork_Piece_Delivery_Head SD Inner Join Cloth_Head I On SD.Cloth_IdNo = I.Cloth_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
                                          " Where SD.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
                                          " I.Cloth_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage", con)
        dt1 = New DataTable
        da.Fill(dt1)

        'da = New SqlClient.SqlDataAdapter(" Select  I.Cloth_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.Item_GST_Percentage,sum(SD.Taxable_Value) As TaxableAmt,sum(SD.Meters) as Qty,Min(Sl_No), 'MTR' AS Units " &
        '                                  " from ClothSales_Invoice_Details SD Inner Join Cloth_Head I On SD.Cloth_IdNo = I.Cloth_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
        '                                  " Where SD.ClothSales_Invoice_Code = '" & Trim(NewCode) & "' Group By " &
        '                                  " I.Cloth_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage", con)
        'dt1 = New DataTable
        'da.Fill(dt1)

        For I = 0 To dt1.Rows.Count - 1

            CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                                ,[QuantityUnit] ,  Tax_Perc                         ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               ,InvCode) " &
                              " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",'MTR'          ," & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "')"

            CMD.ExecuteNonQuery()

        Next

        btn_GENERATEEWB.Enabled = False

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "JobWork_Piece_Delivery_Head", "Eway_BillNo", "JobWork_Piece_Delivery_Code", Pk_Condition)

    End Sub
    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        'Dim einv As New eInvoice(Val(lbl_Company.Tag))
        'einv.GetAuthToken(rtbEWBResponse)

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GetAuthToken(rtbEWBResponse)
    End Sub

    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        txt_EWayBillNo.Text = txt_EWBNo.Text
    End Sub
    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWayBillNo.Text, rtbEWBResponse, 0)
    End Sub
    Private Sub btn_CancelEWB_1_Click(sender As Object, e As EventArgs) Handles btn_CancelEWB_1.Click

        'Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_EWayBillNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "JobWork_Piece_Delivery_Head", "Eway_BillNo", "JobWork_Piece_Delivery_Code")

    End Sub

    Private Sub btn_Close_EWB_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub

    Private Sub txt_rate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_rate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_EWayBillNo.Focus()

        End If
    End Sub

    Private Sub txt_EWayBillNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_EWayBillNo.KeyDown
        If e.KeyValue = 38 Then
            txt_rate.Focus()

        End If
        If e.KeyValue = 40 Then
            txt_Remarks.Focus()

        End If
    End Sub

    Private Sub txt_EWayBillNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_EWayBillNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Remarks.Focus()

        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btn_Cancel_PrintPanel2.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
        pnl_Print2.Visible = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btn_Close_PrintPanel2.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
        pnl_Print2.Visible = False
    End Sub

    Private Sub btn_Close_PieceSelection_Click(sender As Object, e As EventArgs) Handles btn_Close_PieceSelection.Click
        btn_Close_Selection_Click(sender, e)
    End Sub

    Private Sub btn_PRINTEWB_Detailed_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB_Detailed.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWayBillNo.Text, rtbEWBResponse, 1)
    End Sub

    Private Sub Printing_Format_packingSlip_1544(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim da4 As New SqlClient.SqlDataAdapter

        Dim dt4 As New DataTable

        Total_Weight = 0

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        PpSzSTS = False

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
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
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 30
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

        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            NoofItems_PerPage = 24
        Else
            NoofItems_PerPage = 34
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 65 : ClArr(2) = 60 : ClArr(3) = 130 : ClArr(4) = 70
        ClArr(5) = 70 : ClArr(6) = 170 : ClArr(7) = 90
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 17 '18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' Try

        If prn_HdDt.Rows.Count > 0 Then

            Printing_Format_packingSlip_1544_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

            NoofDets = 0

            CurY = CurY - 10

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_NoofBmDets < prn_DetMxIndx

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_Format_packingSlip_1544_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                        'prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                        e.HasMorePages = True

                        Return

                    End If

                    prn_DetIndx = prn_DetIndx + 1

                    CurY = CurY + TxtHgt

                    'With dgv_Details
                    '    da4 = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Piece_Details a  where a.Lot_code = '" & Trim(.Rows(0).Cells(5).Value) & "' Order by a.Sl_No", con)
                    '    dt4 = New DataTable
                    '    da4.Fill(dt4)
                    '    If dt4.Rows.Count > 0 Then
                    '        For I = 0 To dt4.Rows.Count - 1
                    If Trim(prn_DetAr(prn_DetIndx, 8)) <> Trim(prn_DetAr(prn_DetIndx - 1, 8)) Then
                        CurY = CurY + 5
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)

                    ' prn_NoofBmDets = prn_NoofBmDets + 1




                    Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 12, CurY, 0, 0, pFont)


                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 10)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 11)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                    '            ''Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)
                    '        Next I
                    '    End If
                    '    dt4.Clear()
                    Total_Weight = Total_Weight + Format(Val(prn_DetAr(prn_DetIndx, 8)), "#########0.00")

                    'End With
                    prn_NoofBmDets = prn_NoofBmDets + 1



                    NoofDets = NoofDets + 1

                Loop

            End If

            Printing_Format_packingSlip_1544_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

        End If

        ' Catch ex As Exception

        ' MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_packingSlip_1544_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)

        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, City As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
        Dim Cmp_State As String, Cmp_StateCode As String, Cmp_GSTIN_No As String, Cmp_EMail As String

        PageNo = PageNo + 1

        CurY = TMargin


        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : City = ""

        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = ""
        Cmp_State = "" : Cmp_StateCode = "" : Cmp_GSTIN_No = "" : Cmp_EMail = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "EMAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_City").ToString) <> "" Then
            City = "" & prn_HdDt.Rows(0).Item("Company_City").ToString
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)


        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & "," & City, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "/" & Cmp_CstNo, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "/ " & Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        CurY = CurY + TxtHgt - 12
        If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            Common_Procedures.Print_To_PrintDocument(e, "PACKING LIST / DELIVERY CHALLAN", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "PIECE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("DESPATCH TO :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 20
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "DESPATCH TO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatched_To").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
            ' CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(2))

        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "Quality :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Weave :  " & prn_HdDt.Rows(0).Item("weave").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 40, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "SL.NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM ", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS/ROLL.NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PO NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

        CurY = CurY + 15
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format_packingSlip_1544_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim W1 As Single

        Try
            W1 = e.Graphics.MeasureString("Vehicle No    :  ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(Total_Weight), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "Total Rolls ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + W1 + 25, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Total Meters ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            'If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            'End If
            If Common_Procedures.settings.CustomerCode = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
                CurY = CurY + TxtHgt - 10
                Dim len1 As Integer = 0

                p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
                len1 = e.Graphics.MeasureString(" Date     : ", pFont).Width
                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "INWARD / OUTWARD  : ", LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt + 5
                pFont = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Ref No   ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  " & prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + len1 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, "Entry  ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  __________________", LMargin + len1 + 10, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 5

                Common_Procedures.Print_To_PrintDocument(e, "Date ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  __________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 5

                Common_Procedures.Print_To_PrintDocument(e, "Time   ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  __________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 10
            End If


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)



            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            CurY = CurY + TxtHgt + 5
            CurY = CurY + TxtHgt + 5

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
            End If




            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 5, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub Printing_GST_Format_1544(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer, vFrst_Page_NoofItems_PerPage As Integer = 0
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim ItmNm1 As String = "", ItmNm2 As String = ""


        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        PpSzSTS = False

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    'Debug.Print(ps.PaperName)
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

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 30
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

        NoofItems_PerPage = 30 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        'ClArr(1) = 50 : ClArr(2) = 350 : ClArr(3) = 95 : ClArr(4) = 95

        'ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))


        ClArr(1) = 65 : ClArr(2) = 60 : ClArr(3) = 65 : ClArr(4) = 70
        ClArr(5) = 80 : ClArr(6) = 100 : ClArr(7) = 80 : ClArr(8) = 100
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 18 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If prn_HdDt.Rows.Count > 0 Then

            Printing_GST_Format_1544_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

            NoofDets = 0

            CurY = CurY - 10

            If prn_HdDt.Rows.Count > 0 Then

                Do While prn_NoofBmDets < prn_DetMxIndx

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_GST_Format_1544_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)


                        e.HasMorePages = True

                        Return

                    End If

                    prn_DetIndx = prn_DetIndx + 1

                    CurY = CurY + TxtHgt


                    If Trim(prn_DetAr(prn_DetIndx, 9)) <> Trim(prn_DetAr(prn_DetIndx - 1, 9)) Then
                        CurY = CurY + 5
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont) '-----PCS NO
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont) '---LOOM NO
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont) '-----PCS METERS
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)  '------PCS WEIGHT
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 11)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont) '------SET NO
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, ClArr(7), pFont,, True) '-------- PO NO
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 12)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 15, CurY, 0, 0, pFont) '------WARP_LOT_NO
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 13)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 15, CurY, 0, 0, pFont) '------WEFT_LOT_NO

                    Total_Weight = Total_Weight + Format(Val(prn_DetAr(prn_DetIndx, 8)), "#########0.00")
                    Total_Mtr = Total_Mtr + Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00").ToString

                    prn_NoofBmDets = prn_NoofBmDets + 1

                    NoofDets = NoofDets + 1

                Loop

            End If

            Printing_GST_Format_1544_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            If Trim(prn_InpOpts) <> "" Then

                If prn_Count < Len(Trim(prn_InpOpts)) Then

                    If Val(prn_InpOpts) <> "0" Then
                        prn_DetIndx = 0
                        prn_DetSNo = 0
                        prn_PageNo = 0
                        prn_NoofBmDets = 0
                        Total_Mtr = 0
                        Total_Weight = 0
                        e.HasMorePages = True
                        Return
                    End If

                End If

            End If

        End If

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format_1544_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single, M2 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, city As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
        Dim Cmp_State As String, Cmp_StateCode As String, Cmp_GSTIN_No As String, Cmp_EMail As String
        Dim CurY1 As Single
        Dim S As String


        PageNo = PageNo + 1

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


        CurY = TMargin

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 80, CurY - TxtHgt, 1, 0, pFont)
        End If
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PACKING LIST / DELIVERY CHALLAN", LMargin, CurY, 2, PrintWidth, p1Font)
        'CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : city = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = ""
        Cmp_State = "" : Cmp_StateCode = "" : Cmp_GSTIN_No = "" : Cmp_EMail = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "EMAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_City").ToString) <> "" Then
            city = "" & prn_HdDt.Rows(0).Item("Company_City").ToString
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)


        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & "," & city, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "/" & Cmp_CstNo, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "/ " & Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(12) = CurY

        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DC No. : " & prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "No.Of Pcs : " & Format(Val(prn_HdDt.Rows(0).Item("total_Rolls").ToString), "######0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Vehicle No. : " & prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EwayBill No. : ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)
        End If






        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, LnAr(12))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, LnAr(12))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 20, LnAr(12))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, LnAr(12))


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("TRANSPORT DETAILS :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 50
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Italic Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "BILLED TO  :", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO : ", LMargin + M1 + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        CurY1 = CurY
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "  M/s. " & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("Ledger_Address1")) Then
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If
        If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("Ledger_Address2")) Then

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If

        If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("Ledger_Address3")) Then
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If

        If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("Ledger_Address4")) Then
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If

        If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("State_name")) Then
            CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(0).Item("State_name").ToString & "   CODE : " & prn_HdDt.Rows(0).Item("State_Code").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If

        If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("Ledger_GSTinNo")) Then
            CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "GSTIN :  " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + M1 + 10, CurY1, 0, 0, p1Font)

        If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1")) Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + M1 + 30, CurY1, 0, 0, pFont)
        End If
        If IsDBNull(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2")) = False Then
            If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2")) Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + M1 + 30, CurY1, 0, 0, pFont)
            End If
        End If
        If IsDBNull(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3")) = False Then

            If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3")) Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + M1 + 30, CurY1, 0, 0, pFont)
            End If
        End If
        If IsDBNull(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4")) = False Then

            If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4")) Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + M1 + 30, CurY1, 0, 0, pFont)
            End If
        End If

        If IsDBNull(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name")) = False Then
            If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name")) Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " STATE :   " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "     CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + M1 + 30, CurY, 0, 0, pFont)
            End If
        End If
        If IsDBNull(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo")) = False Then
            If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo")) Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + M1 + 30, CurY1, 0, 0, pFont)
            End If
        End If

        CurY = IIf(CurY1 > CurY, CurY1, CurY)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, CurY, LMargin + M1, LnAr(2))

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        CurY = CurY + 5
        Common_Procedures.Print_To_PrintDocument(e, "Quality :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        If Not String.IsNullOrEmpty(prn_HdDt.Rows(0).Item("weave").ToString) Then
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Weave   :  " & prn_HdDt.Rows(0).Item("weave").ToString, PageWidth - ClAr(1), CurY, 1, 0, p1Font)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY



        CurY = CurY + 5

        Common_Procedures.Print_To_PrintDocument(e, "SL.NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ROLL NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1) + ClAr(2) + 5, CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 2, ClAr(4), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "GROSS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 2, ClAr(6), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PO NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 2, ClAr(7), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "WARP ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 2, ClAr(8), pFont)


        Common_Procedures.Print_To_PrintDocument(e, "WEFT ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, 2, ClAr(9), pFont)

        CurY = CurY + 15
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + 5, CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 2, ClAr(4), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOT NO ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, 2, ClAr(9), pFont)


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_GST_Format_1544_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim CurY1 As Single = 0
        Dim D1 As Single = 0
        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width
        D1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 20
        Try

            For I = NoofDets + 1 To NoofItems_PerPage - 1

                CurY = CurY + TxtHgt

                'prn_DetIndx = prn_DetIndx + 1

            Next

            'If is_LastPage = True Then
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            pFont = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt - 10
            ' If is_LastPage = True Then

            If is_LastPage = True Then
                'Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin, CurY, 2, ClAr(1), pFont)
                'Common_Procedures.Print_To_PrintDocument(e, Total_Mtr, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont,, True)

                'prn_TotMtrs = 0

                Common_Procedures.Print_To_PrintDocument(e, "TOTAL ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)


                Common_Procedures.Print_To_PrintDocument(e, Val(Total_Mtr), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(Total_Weight), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 1, 0, pFont)



            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY




            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            Erase BnkDetAr
            If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                BInc = -1

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm1 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm2 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm3 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm4 = Trim(BnkDetAr(BInc))
                End If

            End If

            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY1 = CurY
            'If is_LastPage = True Then
            '    'Left Side
            '    p1Font = New Font("Calibri", 11, FontStyle.Bold Or FontStyle.Underline)
            '    Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + 10, CurY1, 0, 0, p1Font)
            '    CurY1 = CurY1 + TxtHgt
            '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p1Font)
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p1Font)
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p1Font)
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p1Font)

            'End If


            'Right Side
            'CurY = CurY + TxtHgt - 10
            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Frieght", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If


            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Then '---- KALAIMAGAL TEX
            '            Common_Procedures.Print_To_PrintDocument(e, "Trade Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Else
            '            Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        End If
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If

            '    If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            '        CurY = CurY + TxtHgt + 5
            '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            '    End If
            'End If


            'CurY = CurY - 10

            'CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If
            'CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "CGST " & Val(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If
            'CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "SGST " & Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If
            'CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "IGST " & Val(prn_HdDt.Rows(0).Item("IGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If
            'End If





            'CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            '    p1Font = New Font("Calibri", 13, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, "(Textile manufactring service (Weaving) )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 20, CurY, 1, 0, p1Font)
            'End If

            'If is_LastPage = True Then
            '    If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "RoundOff", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            '    End If

            'End If


            'CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, PageWidth, CurY)
            'LnAr(8) = CurY
            'CurY = CurY + TxtHgt + 5


            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("total_amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt - 10
                pFont = New Font("Calibri", 12, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Value of Goods", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY - 5, 1, 0, pFont)
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("total_amount").ToString)), PageWidth - 10, CurY - 5, 1, 0, p1Font)

                    CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, LnAr(7))

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
                End If

            End If

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(9))

            'CurY = CurY + TxtHgt - 5

            'If is_LastPage = True Then
            '    BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("total_amount").ToString))

            '    p1Font = New Font("Calibri", 9, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            'End If

            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(12) = CurY
            'p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
            'len1 = e.Graphics.MeasureString("Due Days / Date  : ", pFont).Width
            'CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "INWARD / OUTWARD  : ", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + D1, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt + 5
            'pFont = New Font("Calibri", 9, FontStyle.Regular)
            'Common_Procedures.Print_To_PrintDocument(e, "Ref No   ", LMargin + 10, CurY, 0, 0, pFont)
            'p1Font = New Font("Calibri", 9, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, ":  " & prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + len1 + 10, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "BANK NAME      :  " & BankNm1, LMargin + D1, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt + 5
            'Common_Procedures.Print_To_PrintDocument(e, "Entry  ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":  _________________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT NO    :  " & BankNm2, LMargin + D1, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt + 5


            'Common_Procedures.Print_To_PrintDocument(e, "Date ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":  _________________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "BRANCH NAME :  " & BankNm3, LMargin + D1, CurY, 0, 0, p1Font)


            'CurY = CurY + TxtHgt + 5

            'Common_Procedures.Print_To_PrintDocument(e, "Time   ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":  _________________________ ", LMargin + len1 + 10, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE          :  " & BankNm4, LMargin + D1, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt + 10

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(13) = CurY

            'CurY = CurY + 5
            '    p1Font = New Font("Calibri", 12, FontStyle.Underline)
            '  Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)
            '    Common_Procedures.Print_To_PrintDocument(e, "Goods Description : ", LMargin + D1 - 20, CurY, 0, 0, p1Font)


            '  p1Font = New Font("Calibri", 9, FontStyle.Bold)
            ' CurY = CurY + TxtHgt + 10
            '    Common_Procedures.Print_To_PrintDocument(e, "1. Overdue Interest will be charged at 24% from The invoice date. ", LMargin + 10, CurY, 0, 0, pFont)

            '  Common_Procedures.Print_To_PrintDocument(e, "1. Uncalendered Grey Fabrics ", LMargin + D1 - 20, CurY, 0, 0, p1Font)


            '   CurY = CurY + TxtHgt
            '  Common_Procedures.Print_To_PrintDocument(e, "2. Our risk & responsibility ceases on goods leaving our factory.", LMargin + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "2. Goods Not for Sale.Goods Sent After Job Work Completion", LMargin + D1 - 20, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "3. Goods are supplied under our firm conditions.", LMargin + 10, CurY, 0, 0, pFont)
            'e.Graphics.DrawLine(Pens.Black, LMargin + D1 - 30, CurY, PageWidth, CurY)

            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "4. Subject to Tirupur jurisdiction. ", LMargin + 10, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Tax Is Payable On Reverse Charge : YES / NO", LMargin + D1 + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "5. E.O & E .", LMargin + 10, CurY, 0, 0, pFont)



            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin + D1 - 30, CurY, LMargin + D1 - 30, LnAr(12))

            If is_LastPage = True Then

            CurY = CurY + 5
                p1Font = New Font("Calibri", 11, FontStyle.Underline)
                Common_Procedures.Print_To_PrintDocument(e, "Goods Description : ", LMargin + 10, CurY, 0, 0, p1Font)

                pFont = New Font("Calibri", 10, FontStyle.Regular)
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "1. Uncalendered Grey Fabrics ", LMargin + 10, CurY, 0, 0, pFont)
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "2. Goods Not for Sale.Goods Sent After Job Work Completion ", LMargin + D1 - 20, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            End If


            'Common_Procedures.Print_To_PrintDocument(e, "Certified that particulars given above are true and correct and the amount indicated represents the price actually charged and that ", LMargin + 10, CurY, 0, 0, pFont)


            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "there is no flow of additional consideration directly or indirectly from the buyer.", LMargin + 10, CurY, 0, 0, pFont)

            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then
                p1Font = New Font("Elephant", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            Else
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Then
                CurY = CurY + TxtHgt
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.vijay_tex_Sign2, Drawing.Image), PageWidth - 110, CurY, 90, 55)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 350, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Printing_Format_1547(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim new_code As String = ""
        Dim flperc As String = 0
        Dim flmtr As String = 0
        Dim fmtr As String = 0
        Dim vBal_flperc As String = 0
        Dim vBalmtr As String = 0

        Dim da1 As New SqlClient.SqlDataAdapter

        Dim dt1 As New DataTable
        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If




        PpSzSTS = False

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    'Debug.Print(ps.PaperName)
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
        '    End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 20
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

        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 30 ' 32 '34 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 88 : ClArr(2) = 60 : ClArr(3) = 70 : ClArr(4) = 70 : ClArr(5) = 62
        ClArr(6) = 88 : ClArr(7) = 60 : ClArr(8) = 70 : ClArr(9) = 70
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        'ClArr(1) = 63 : ClArr(2) = 75 : ClArr(3) = 75 : ClArr(4) = 75
        'ClArr(5) = 62 : ClArr(6) = 75 : ClArr(7) = 75 : ClArr(8) = 75 : ClArr(9) = 75
        'ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))
        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' Try
        new_code = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'Next  select j.* from  JobWork_Production_Head j where j.company_idno=  1  and j.JobWork_Delivery_Code = '1-1/20-21'
        da1 = New SqlClient.SqlDataAdapter("select j.*,L.Loom_name from  JobWork_Production_Head j left outer join Loom_Head L On L.Loom_Idno=j.Loom_idno where j.company_idno=  " & Str(Val(lbl_Company.Tag)) & "  and j.JobWork_Delivery_Code = '" & Trim(new_code) & "'order by j.for_orderby", con)
        'da2 = New SqlClient.SqlDataAdapter("select a.*  from JobWork_Piece_Delivery_Details a  where JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
        dt1 = New DataTable
        da1.Fill(dt1)
        If prn_HdDt.Rows.Count > 0 Then

            Printing_Format_1547_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, flperc)

            NoofDets = 0

            CurY = CurY - 10

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_NoofBmDets < prn_DetMxIndx

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_Format_1547_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False, flmtr, flperc, vBal_flperc, vBalmtr)

                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                        e.HasMorePages = True

                        Return

                    End If

                    prn_DetIndx = prn_DetIndx + 1
                    CurY = CurY + TxtHgt

                    If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then


                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1233" Then '----Vipin textile Somanur
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + 10, CurY, 0, 0, pFont) 'pcs_no
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + 10, CurY, 0, 0, pFont) 'pcs_no
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1233" Then '----Vipin textile Somanur
                            If Val(prn_DetAr(prn_DetIndx, 8)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 8)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx, 8)), "#########0.000")
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx, 6)), "#########0.000")
                            End If

                            'If dt1.Rows.Count > 0 Then
                            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                            'End If

                        Else
                            If Val(prn_DetAr(prn_DetIndx, 8)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 8)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx, 8)), "#########0.000")
                            Else
                                If Val(prn_DetAr(prn_DetIndx, 6)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx, 6)), "#########0.000")
                                End If
                            End If

                        End If

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1420" Then
                            If Val(prn_DetAr(prn_DetIndx, 7)) > 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 7)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        End If

                        End If


                        flperc = vPRN_FOLDINGPERC

                        If Val(flperc) <> "100" Then

                            flmtr = Format(Val(prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString) * Val(flperc) / 100, "#########0.00")

                            flmtr = Format(Common_Procedures.Meter_RoundOff(flmtr), "#########0.00")

                            vBal_flperc = Format(100 - Val(flperc), "##########0.00")
                            vBalmtr = Format(Val(prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString) * Val(vBal_flperc) / 100, "#########0.00")

                            vBalmtr = Format(Common_Procedures.Meter_RoundOff(vBalmtr), "#########0.00")

                        End If
                        prn_NoofBmDets = prn_NoofBmDets + 1


                    End If

                    If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then


                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1233" Then '----Vipin textile Somanur
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont) 'pcs_no
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont) 'pcs_no
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1233" Then '----Vipin textile Somanur
                            If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)), "#########0.000")
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.000")
                            End If

                        Else
                            If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)), "#########0.000")
                            Else
                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.000")
                            End If
                            End If
                        End If

                        'If dt1.Rows.Count > 0 Then
                        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                        'End If

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1420" Then
                            If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7)) > 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 10, CurY, 1, 0, pFont)
                        End If

                        End If

                        prn_NoofBmDets = prn_NoofBmDets + 1
                    End If


                    NoofDets = NoofDets + 1
                Loop




            End If

            dt1.Clear()

            Printing_Format_1547_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True, flmtr, flperc, vBal_flperc, vBalmtr)

        End If


        ' Catch ex As Exception

        ' MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_1547_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal flperc As String)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTIN_No As String

        PageNo = PageNo + 1

        CurY = TMargin


        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        CurY = TMargin
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

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt - 18
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 60, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + 70, CurY, 0, 0, p1Font)

        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PIECE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 60, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + 70, CurY, 0, 0, pFont)

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        'p1Font = New Font("Calibri", 14, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        pFont = New Font("Calibri", 9, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO :M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + M1 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(2))

        CurY = CurY + 10

        If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Remarks").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
        End If

        Common_Procedures.Print_To_PrintDocument(e, "Quality :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "Folding  :  " & Val(vPRN_FOLDINGPERC) & " %", PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin, CurY, 2, ClAr(1), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        CurY = CurY + 15
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "/MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "/MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format_1547_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal flmtr As String, ByVal flperc As String, ByVal vBal_flperc As String, ByVal vBalmtr As String)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim W1 As Single

        Try
            W1 = e.Graphics.MeasureString("Vehicle No    :  ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt

            If is_LastPage = True Then



                Common_Procedures.Print_To_PrintDocument(e, "Total Pcs : " & Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "Total Rolls : " & Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + W1 + 25, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Total Meters : " & prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

                If Val(Total_Weight) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Total Weight : " & Format(Val(Total_Weight), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_Weight), "#########0.000"), LMargin + W1 + 25, CurY, 0, 0, pFont)
                End If

                If Val(flperc) <> "100" Then

                    Common_Procedures.Print_To_PrintDocument(e, "Total Meters 100 % : " & Format(Val(flmtr), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, pFont)
            End If


            If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            End If

                If Val(flperc) <> "100" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Folding Less " & Val(vBal_flperc) & " % : " & Format(Val(vBalmtr), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, pFont)
                End If




            End If

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Total Meters ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If


            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Common_Procedures.settings.CustomerCode <> "1420" Then
                Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            End If
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            If Common_Procedures.settings.CustomerCode = "1420" Then
                CurY = CurY + TxtHgt + 25
                Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Authorised Signature", PageWidth - 5, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Selection_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Selection.CellEnter
        With sender
            If Val(.Rows(e.RowIndex).Cells(5).Value) = 0 Then
                .DefaultCellStyle.SelectionForeColor = Color.Black
            Else
                .DefaultCellStyle.SelectionForeColor = Color.Red
            End If
        End With
    End Sub

    Private Sub btn_get_Weft_CountName_from_Master_Click(sender As Object, e As EventArgs) Handles btn_get_Weft_CountName_from_Master.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim Clo_IdNo As Integer
        Dim wftcnt_idno As Integer
        Dim Nr As Integer
        Dim NewCode As String

        If Trim(cbo_ClothName.Text) <> "" Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

            wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))

            cmd.Connection = con

            cmd.CommandText = "Update Stock_Yarn_Processing_Details set Count_IdNo = " & Str(Val(wftcnt_idno)) & " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        End If

    End Sub

    Private Sub lbl_Print_PcsWise_Delivery_Without_Weight_Click(sender As Object, e As EventArgs) Handles lbl_Print_PcsWise_Delivery_Without_Weight.Click
        btn_Print_PcsWise_Delivery_Without_Weight_Click(sender, e)
    End Sub

    Private Sub lbl_Print_PcsWise_Delivery_With_Weight_Click(sender As Object, e As EventArgs) Handles lbl_Print_PcsWise_Delivery_With_Weight.Click
        btn_Print_PcsWise_Delivery_With_Weight_Click(sender, e)
    End Sub

    Private Sub Printing_Format6_1608(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        PpSzSTS = False
        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
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
        PrintDocument1.DefaultPageSettings.Landscape = False

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 30
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

        'If PrintDocument1.DefaultPageSettings.Landscape = True Then
        '    With PrintDocument1.DefaultPageSettings.PaperSize
        '        PrintWidth = .Height - TMargin - BMargin
        '        PrintHeight = .Width - RMargin - LMargin
        '        PageWidth = .Height - TMargin
        '        PageHeight = .Width - RMargin
        '    End With
        'End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 31 ' 34 

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 60 : ClArr(2) = 80 : ClArr(3) = 95 : ClArr(4) = 60 : ClArr(5) = 80 : ClArr(6) = 95 : ClArr(7) = 60 : ClArr(8) = 80

        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format6_1608_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_NoofBmDets < prn_DetMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format6_1608_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            prn_DetIndx = prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetIndx = prn_DetIndx + 1

                        CurY = CurY + TxtHgt

                        If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 12, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 12, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage + NoofItems_PerPage, 4)), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        NoofDets = NoofDets + 1

                    Loop

                End If

                Printing_Format6_1608_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format6_1608_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_GSTNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_GSTNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PIECE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(11) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(11), LMargin + M1, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + M1 + 4, LnAr(11), LMargin + M1 + 4, LnAr(2))

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(2))

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "Quatlity :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "EWAY BILL NO :  " & prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + 10, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Value of Goods :  " & prn_HdDt.Rows(0).Item("Total_Amount").ToString, PageWidth - 40, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ROLL.NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ROLL.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ROLL.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format6_1608_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim W1 As Single

        Try
            W1 = e.Graphics.MeasureString("Vehicle No    :  ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "Total Rolls ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Total Meters ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 70, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format6_With_Weight_1608(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim new_code As String = ""

        Dim da1 As New SqlClient.SqlDataAdapter

        Dim dt1 As New DataTable
        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If




        PpSzSTS = False

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    'Debug.Print(ps.PaperName)
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
        '    End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 20
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

        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 28 ' 30 ' 32 '34

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 50 : ClArr(2) = 98 : ClArr(3) = 70 : ClArr(4) = 70 : ClArr(5) = 62
        ClArr(6) = 50 : ClArr(7) = 98 : ClArr(8) = 70 : ClArr(9) = 70
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        new_code = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        da1 = New SqlClient.SqlDataAdapter("select j.*,L.Loom_name from  JobWork_Production_Head j left outer join Loom_Head L On L.Loom_Idno=j.Loom_idno where j.company_idno=  " & Str(Val(lbl_Company.Tag)) & "  and j.JobWork_Delivery_Code = '" & Trim(new_code) & "'order by j.for_orderby", con)
        dt1 = New DataTable
        da1.Fill(dt1)
        If prn_HdDt.Rows.Count > 0 Then

            Printing_Format6_With_Weight_1608_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

            NoofDets = 0

            CurY = CurY - 10

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_NoofBmDets < prn_DetMxIndx

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_Format6_With_Weight_1608_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                        e.HasMorePages = True

                        Return

                    End If

                    prn_DetIndx = prn_DetIndx + 1
                    CurY = CurY + TxtHgt

                    If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont) 'pcs_no
                        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1233" Then '----Vipin textile Somanur
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + 10, CurY, 0, 0, pFont) 'pcs_no
                        'Else
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + 10, CurY, 0, 0, pFont) 'pcs_no
                        'End If
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1233" Then '----Vipin textile Somanur
                            If Val(prn_DetAr(prn_DetIndx, 8)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 8)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx, 8)), "#########0.000")
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx, 6)), "#########0.000")
                            End If

                        Else

                            If Val(prn_DetAr(prn_DetIndx, 8)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 8)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx, 8)), "#########0.000")
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx, 6)), "#########0.000")
                            End If

                        End If

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1420" Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 7)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        End If

                        prn_NoofBmDets = prn_NoofBmDets + 1


                    End If

                    If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then


                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont) 'pcs_no
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1233" Then '----Vipin textile Somanur
                            If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)), "#########0.000")
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.000")
                            End If

                        Else
                            If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8)), "#########0.000")
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Total_Weight = Format(Val(Total_Weight) + Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.000")
                            End If
                        End If

                        'If dt1.Rows.Count > 0 Then
                        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                        'End If

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1420" Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 10, CurY, 1, 0, pFont)
                        End If
                        prn_NoofBmDets = prn_NoofBmDets + 1
                    End If

                    NoofDets = NoofDets + 1
                Loop

            End If

            dt1.Clear()

            Printing_Format6_With_Weight_1608_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

        End If


        ' Catch ex As Exception

        ' MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format6_With_Weight_1608_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_GSTIN_No As String

        PageNo = PageNo + 1

        CurY = TMargin


        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt + 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PIECE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

        CurY = CurY + 5

        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO  :  " & prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DATE  :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)



        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO :M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + M1 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString & " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(2))

        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "Quatlity :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "EWAY BILL NO :  " & prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + 10, CurY, 0, 0, pFont)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Value of Goods :  " & prn_HdDt.Rows(0).Item("Total_Amount").ToString, PageWidth - 40, CurY, 1, 0, pFont)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "SL.", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ROLL", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SL.", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ROLL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

        CurY = CurY + 15
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "/MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "/MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format6_With_Weight_1608_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim W1 As Single

        Try
            W1 = e.Graphics.MeasureString("Vehicle No    :  ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))


            CurY = CurY + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                CurY = CurY - 5
                Common_Procedures.Print_To_PrintDocument(e, "Remarks : " & prn_HdDt.Rows(0).Item("Remarks").ToString, LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                CurY = CurY + TxtHgt - 5
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Total Rolls : " & Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0"), LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total Meters : " & prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 0, 0, pFont)

            If Val(Total_Weight) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Total Weight : " & Format(Val(Total_Weight), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
            End If


            If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If


            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            'If Common_Procedures.settings.CustomerCode = "1420" Then
            '    CurY = CurY + TxtHgt + 25
            '    Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "Authorised Signature", PageWidth - 5, CurY, 1, 0, pFont)
            'End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Print_PcsWise_Delivery_Without_Weight_GotFocus(sender As Object, e As EventArgs) Handles btn_Print_PcsWise_Delivery_Without_Weight.GotFocus
        btn_Print_PcsWise_Delivery_Without_Weight.BackColor = Color.Lime
        btn_Print_PcsWise_Delivery_Without_Weight.ForeColor = Color.Blue
    End Sub

    Private Sub btn_Print_PcsWise_Delivery_Without_Weight_LostFocus(sender As Object, e As EventArgs) Handles btn_Print_PcsWise_Delivery_Without_Weight.LostFocus
        btn_Print_PcsWise_Delivery_Without_Weight.BackColor = Color.FromArgb(41, 57, 85)
        btn_Print_PcsWise_Delivery_Without_Weight.ForeColor = Color.White
    End Sub

    Private Sub btn_Print_PcsWise_Delivery_With_Weight_GotFocus(sender As Object, e As EventArgs) Handles btn_Print_PcsWise_Delivery_With_Weight.GotFocus
        btn_Print_PcsWise_Delivery_With_Weight.BackColor = Color.Lime
        btn_Print_PcsWise_Delivery_With_Weight.ForeColor = Color.Blue
    End Sub

    Private Sub btn_Print_PcsWise_Delivery_With_Weight_LostFocus(sender As Object, e As EventArgs) Handles btn_Print_PcsWise_Delivery_With_Weight.LostFocus
        btn_Print_PcsWise_Delivery_With_Weight.BackColor = Color.FromArgb(41, 57, 85)
        btn_Print_PcsWise_Delivery_With_Weight.ForeColor = Color.White
    End Sub

    Private Sub btn_Print_Cancel_GotFocus(sender As Object, e As EventArgs) Handles btn_Print_Cancel.GotFocus
        btn_Print_Cancel.BackColor = Color.Lime
        btn_Print_Cancel.ForeColor = Color.Blue
    End Sub

    Private Sub btn_Print_Cancel_LostFocus(sender As Object, e As EventArgs) Handles btn_Print_Cancel.LostFocus
        btn_Print_Cancel.BackColor = Color.FromArgb(255, 90, 90)
        btn_Print_Cancel.ForeColor = Color.White
    End Sub

    Private Sub dgv_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub
End Class
