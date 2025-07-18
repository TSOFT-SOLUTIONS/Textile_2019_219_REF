Public Class InHouse_PieceDelivery_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private Pk_Condition As String = "GPCDC-"
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
    Private dgvDet_CboBx_ColNos_Arr As Integer() = {1}
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

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
        cbo_Weaving_JobNo.Text = ""
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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName, c.Ledger_Name as Transport_Name, d.Cloth_Name , del.Ledger_Name as Delivery_Name from InHouse_Piece_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head del ON a.Delivery_Idno = del.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_Idno = d.cloth_idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_DcNo.Text = dt1.Rows(0).Item("InHouse_Piece_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("InHouse_Piece_Delivery_Date").ToString
                msk_Date.Text = dtp_Date.Text

                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString

                cbo_ClothName.Text = dt1.Rows(0).Item("Cloth_Name").ToString

                cbo_Transport.Text = dt1.Rows(0).Item("Transport_Name").ToString

                cbo_DeliveryTo.Text = dt1.Rows(0).Item("delivery_name").ToString
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString

                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                txt_rate.Text = dt1.Rows(0).Item("Rate").ToString

                lbl_total_amount.Text = dt1.Rows(0).Item("Total_Amount").ToString

                txt_EWayBillNo.Text = dt1.Rows(0).Item("Eway_BillNo").ToString


                da3 = New SqlClient.SqlDataAdapter("select a.* , b.ClothType_Name from InHouse_Piece_Delivery_Details a LEFT OUTER JOIN ClothType_Head b ON a.ClothType_Idno = b.ClothType_Idno  Where InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by sl_no", con)
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
                        dgv_Details.Rows(n).Cells(7).Value = dt3.Rows(i).Item("folding").ToString
                        dgv_Details.Rows(n).Cells(8).Value = dt3.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt3.Rows(i).Item("Weight").ToString), "########0.00")

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Total_Rolls").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Delivery_Meters").ToString), "########0.00")
                End With

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
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1075" Then '---- JR TEX ( STANLEY ) ( MS FABRICS ) (SULUR)   (or)   J.R TEX ( STANLEY ) ( M.S FABRICS ) (SULUR)
            btn_SaveAll.Visible = True
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

        pnl_2.Visible = False
        pnl_2.BringToFront()
        pnl_2.Left = (Me.Width - pnl_2.Width) \ 2
        pnl_2.Top = (Me.Height - pnl_2.Height) \ 2



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

        cbo_Weaving_JobNo.Enabled = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            lbl_JobNo_Caption.Text = "PO No."
            cbo_Weaving_JobNo.Enabled = True
            cbo_Weaving_JobNo.Width = cbo_Type.Width

            lbl_JobDate.Visible = False
            lbl_JobDate_Caption.Visible = False

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1420" Then
            dgv_Details.Columns(9).Visible = True
            dgv_Details.Columns(9).Width = 70
            dgv_Details.Columns(9).ReadOnly = True

        End If

        lbl_rate.Visible = True
        lbl_TotalAmount_caption.Visible = True
        txt_rate.Visible = True
        lbl_total_amount.Visible = True

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
        AddHandler cbo_Weaving_JobNo.GotFocus, AddressOf ControlGotFocus


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
        AddHandler cbo_Weaving_JobNo.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_DespatchTo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FilterRollNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler cbo_Weaving_JobNo.KeyDown, AddressOf TextBoxControlKeyDown


        '  AddHandler txt_rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DespatchTo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FilterRollNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler cbo_Weaving_JobNo.KeyPress, AddressOf TextBoxControlKeyPress

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
                ElseIf pnl_2.Visible = True Then
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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Jobwork_Piece_Delivery_Entry, New_Entry, Me, con, "InHouse_Piece_Delivery_Head", "InHouse_Piece_Delivery_Code", NewCode, "InHouse_Piece_Delivery_Date", "(InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "InHouse_Piece_Delivery_Head", "InHouse_Piece_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "InHouse_Piece_Delivery_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "InHouse_Piece_Delivery_Details", "InHouse_Piece_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Lot_No, Pcs_No, ClothType_IdNo, Meters ,  Lot_Code , Entry_PkCondition ", "Sl_No", "InHouse_Piece_Delivery_Code, For_OrderBy, Company_IdNo, InHouse_Piece_Delivery_No, InHouse_Piece_Delivery_Date, Ledger_Idno, Weight", trans)



            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1 Where PackingSlip_Code_Type1 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1 Where PackingSlip_Code_Type2 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1 Where PackingSlip_Code_Type3 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1 Where PackingSlip_Code_Type4 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1 Where PackingSlip_Code_Type5 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from InHouse_Piece_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from InHouse_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select top 1 InHouse_Piece_Delivery_No from InHouse_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and InHouse_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, InHouse_Piece_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 InHouse_Piece_Delivery_No from InHouse_Piece_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and InHouse_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, InHouse_Piece_Delivery_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 InHouse_Piece_Delivery_No from InHouse_Piece_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and InHouse_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, InHouse_Piece_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 InHouse_Piece_Delivery_No from InHouse_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and InHouse_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, InHouse_Piece_Delivery_No desc", con)
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
                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "InHouse_Piece_Delivery_Head", "InHouse_Piece_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            End If

            lbl_DcNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from InHouse_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and InHouse_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, InHouse_Piece_Delivery_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("InHouse_Piece_Delivery_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("InHouse_Piece_Delivery_Date").ToString
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

            Da = New SqlClient.SqlDataAdapter("select InHouse_Piece_Delivery_No from InHouse_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and InHouse_Piece_Delivery_Code = '" & Trim(RecCode) & "'", con)
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

            Da = New SqlClient.SqlDataAdapter("select InHouse_Piece_Delivery_No from InHouse_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and InHouse_Piece_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Nr As Long = 0
        Dim dCloTyp_ID As Integer = 0
        Dim T1_Mtrs As String = 0
        Dim T2_Mtrs As String = 0
        Dim T3_Mtrs As String = 0
        Dim T4_Mtrs As String = 0
        Dim T5_Mtrs As String = 0
        Dim Transto_STS As Integer = 0
        Dim Transtkparty_Id As Integer = 0
        Dim vLm_IdNo As Integer = 0
        Dim vWdth_Type As String = ""
        Dim vCrmp_Perc As Single = 0
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
        Dim vFOLDPERC As String
        Dim vOrdByNo As String = ""
        Dim Delivery_ID As Integer = 0
        Dim vWARPYARN_STOCK_POSTING_STS As Integer
        Dim vWARPCOUNT_ID As Integer = 0
        Dim vCONSYARN_FORWARP As String = 0

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(lbl_DcNo.Text) = "" Then
            MessageBox.Show("Invalid Dc.No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Jobwork_Piece_Delivery_Entry, New_Entry, Me, con, "InHouse_Piece_Delivery_Head", "InHouse_Piece_Delivery_Code", NewCode, "InHouse_Piece_Delivery_Date", "(InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and InHouse_Piece_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, InHouse_Piece_Delivery_No desc", dtp_Date.Value.Date) = False Then Exit Sub

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

        Trans_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)


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



        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
        OpDate = DateAdd(DateInterval.Day, -1, OpDate)

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else
                If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then
                    lbl_DcNo.Text = Common_Procedures.get_Cloth_JobWork_Delivery_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                Else
                    lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "InHouse_Piece_Delivery_Head", "InHouse_Piece_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                End If

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

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

                cmd.CommandText = "Insert into InHouse_Piece_Delivery_Head ( InHouse_Piece_Delivery_Code,               Company_IdNo       ,     InHouse_Piece_Delivery_No,           for_OrderBy     , InHouse_Piece_Delivery_Date,          Ledger_IdNo    ,        Cloth_Idno       ,        Delivery_Idno         ,         Transport_IdNo    ,               Vehicle_No          ,               Remarks           ,            Total_Rolls   ,     Total_Delivery_Meters ,                 Rate           ,                  Total_Amount          ,               Eway_BillNo            ) " &
                                    " Values                               ( '" & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",       @DcDate              , " & Str(Val(Led_id)) & ", " & Str(Val(Clo_id)) & ", " & Str(Val(Delivery_ID)) & ", " & Str(Val(Trans_id)) & ", '" & Trim(cbo_VehicleNo.Text) & "', '" & Trim(txt_Remarks.Text) & "', " & Str(Val(vTotRls)) & ", " & Str(Val(vTotMtrs)) & ", " & Str(Val(txt_rate.Text)) & ", " & Str(Val(lbl_total_amount.Text)) & ", '" & Trim(txt_EWayBillNo.Text) & "'  ) "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "InHouse_Piece_Delivery_Head", "InHouse_Piece_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "InHouse_Piece_Delivery_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "InHouse_Piece_Delivery_Details", "InHouse_Piece_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No, Pcs_No, ClothType_IdNo, Meters ,  Lot_Code , Entry_PkCondition ", "Sl_No", "InHouse_Piece_Delivery_Code, For_OrderBy, Company_IdNo, InHouse_Piece_Delivery_No, InHouse_Piece_Delivery_Date, Ledger_Idno, Weight", tr)

                cmd.CommandText = "Update InHouse_Piece_Delivery_Head set InHouse_Piece_Delivery_Date = @DcDate, Ledger_IdNo = " & Str(Val(Led_id)) & ", Cloth_Idno = " & Str(Val(Clo_id)) & ", Delivery_Idno=" & Str(Val(Delivery_ID)) & " , Transport_idno = " & Str(Val(Trans_id)) & ", Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "', Remarks = '" & Trim(txt_Remarks.Text) & "', Total_Rolls = " & Str(Val(vTotRls)) & ", Total_Delivery_Meters = " & Str(Val(vTotMtrs)) & " , Rate = " & Str(Val(txt_rate.Text)) & ", Total_amount = " & Str(Val(lbl_total_amount.Text)) & ", Eway_BillNo =  '" & Trim(txt_EWayBillNo.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1 Where PackingSlip_Code_Type1 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1 Where PackingSlip_Code_Type2 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1 Where PackingSlip_Code_Type3 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1 Where PackingSlip_Code_Type4 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1 Where PackingSlip_Code_Type5 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "InHouse_Piece_Delivery_Head", "InHouse_Piece_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "InHouse_Piece_Delivery_Code, Company_IdNo, for_OrderBy", tr)


            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)
            Partcls = "Inhouse Pcs Delv : Dc.No. " & Trim(lbl_DcNo.Text) & ", Meters : " & Format(Val(vTotMtrs), "##########0.00")


            cmd.CommandText = "Delete from InHouse_Piece_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSimpleTable) & ""
            cmd.ExecuteNonQuery()


            T1_Mtrs = 0
            T2_Mtrs = 0
            T3_Mtrs = 0
            T4_Mtrs = 0
            T5_Mtrs = 0

            vTotOpMtrs = 0

            Sno = 0
            For i = 0 To dgv_Details.RowCount - 1

                With dgv_Details

                    If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 And Trim(dgv_Details.Rows(i).Cells(5).Value) <> "" Then

                        dCloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        T1_Mtrs = 0
                        T2_Mtrs = 0
                        T3_Mtrs = 0
                        T4_Mtrs = 0
                        T5_Mtrs = 0
                        If Val(dCloTyp_ID) = 5 Then
                            T5_Mtrs = Format(Val(dgv_Details.Rows(i).Cells(4).Value), "##########0.00")
                        ElseIf Val(dCloTyp_ID) = 4 Then
                            T4_Mtrs = Format(Val(dgv_Details.Rows(i).Cells(4).Value), "##########0.00")
                        ElseIf Val(dCloTyp_ID) = 3 Then
                            T3_Mtrs = Format(Val(dgv_Details.Rows(i).Cells(4).Value), "##########0.00")
                        ElseIf Val(dCloTyp_ID) = 2 Then
                            T2_Mtrs = Format(Val(dgv_Details.Rows(i).Cells(4).Value), "##########0.00")
                        Else
                            T1_Mtrs = Format(Val(dgv_Details.Rows(i).Cells(4).Value), "##########0.00")
                        End If

                        vFOLDPERC = Val(dgv_Details.Rows(i).Cells(7).Value)
                        If Val(vFOLDPERC) = 0 Then vFOLDPERC = 100

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into InHouse_Piece_Delivery_Details (  InHouse_Piece_Delivery_Code,               Company_IdNo       ,   InHouse_Piece_Delivery_No  ,          for_OrderBy      , InHouse_Piece_Delivery_Date,             Ledger_IdNo ,            Cloth_IdNo    ,            Sl_No      ,                     Lot_No              ,                    Pcs_No              ,           ClothType_IdNo    ,                      Meters              ,                    Lot_Code            ,                    Entry_PkCondition    ,             Folding         ,         Weaving_JobCode_forSelection   ,                      Weight               ) " &
                                            "           Values                        (     '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",        @DcDate             , " & Str(Val(Led_id)) & ",  " & Str(Val(Clo_id)) & ",  " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(dCloTyp_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", '" & Trim(.Rows(i).Cells(5).Value) & "', '" & Trim(.Rows(i).Cells(6).Value) & "' , " & Str(Val(vFOLDPERC)) & ", '" & Trim(.Rows(i).Cells(8).Value) & "', " & Str(Val(.Rows(i).Cells(9).Value)) & " ) "
                        cmd.ExecuteNonQuery()


                        If dCloTyp_ID = 1 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '" & Trim(NewCode) & "', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 2 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '" & Trim(NewCode) & "', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 3 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '" & Trim(NewCode) & "', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 4 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '" & Trim(NewCode) & "', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 5 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '" & Trim(NewCode) & "', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(5).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        End If


                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSimpleTable) & "(Currency1, Meters1, Meters2, Meters3, Meters4, Meters5) values (" & Str(Val(vFOLDPERC)) & ", " & Str(Val(T1_Mtrs)) & ", " & Str(Val(T2_Mtrs)) & ", " & Str(Val(T3_Mtrs)) & ", " & Str(Val(T4_Mtrs)) & ", " & Str(Val(T5_Mtrs)) & ")"
                        cmd.ExecuteNonQuery()


                    End If

                End With

            Next i
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "InHouse_Piece_Delivery_Details", "InHouse_Piece_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No, Pcs_No, ClothType_IdNo, Meters ,  Lot_Code , Entry_PkCondition ", "Sl_No", "InHouse_Piece_Delivery_Code, For_OrderBy, Company_IdNo, InHouse_Piece_Delivery_No, InHouse_Piece_Delivery_Date, Ledger_Idno, Weight", tr)


            Da1 = New SqlClient.SqlDataAdapter("select Currency1 as folding, sum(meters1) as type1mtrs, sum(meters2) as type2mtrs, sum(meters3) as type3mtrs, sum(meters4) as type4mtrs, sum(meters5) as type5mtrs from " & Trim(Common_Procedures.EntryTempSimpleTable) & " group by Currency1 having sum(meters1) <> 0 or sum(meters2) <> 0 or sum(meters3) <> 0 or sum(meters4) <> 0 or sum(meters5) <> 0", con)
            Da1.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    Sno = Sno + 1

                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (     Reference_Code     ,             Company_IdNo         ,           Reference_No       ,          for_OrderBy      , Reference_Date,                                            StockOff_IdNo   ,     DeliveryTo_Idno     ,                            ReceivedFrom_Idno              ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No      ,           Cloth_Idno    ,                         Folding                       ,                         Meters_Type1                    ,                         Meters_Type2                    ,                         Meters_Type3                    ,                         Meters_Type4                    ,                         Meters_Type5                     ) " &
                                        " Values                                  ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",     @DcDate   , " & Str(Val(Common_Procedures.CommonLedger.OwnSort_Ac)) & ", " & Str(Val(Led_id)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Clo_id)) & ", " & Str(Val(Dt1.Rows(i).Item("folding").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("type1mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("type2mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("type3mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("type4mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("type5mtrs").ToString)) & " ) "
                    cmd.ExecuteNonQuery()

                Next

            End If

            Dt1.Clear()

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)  and Close_status = 0 )", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_Type, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)  and Close_status = 0 )", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Weaving_JobNo.Enabled And cbo_Weaving_JobNo.Visible Then
                cbo_Weaving_JobNo.Focus()
            Else
                cbo_ClothName.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)  and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_DeliveryTo, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_vehicleno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "InHouse_Piece_Delivery_Head", "Vehicle_No", "", "Vehicle_No")

    End Sub
    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, Nothing, "InHouse_Piece_Delivery_Head", "Vehicle_No", "", "Vehicle_No")

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
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, Nothing, "InHouse_Piece_Delivery_Head", "Vehicle_No", "", "", False)
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
                Condt = "a.InHouse_Piece_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.InHouse_Piece_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.InHouse_Piece_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If



            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If



            If Trim(txt_FilterRollNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.InHouse_Piece_Delivery_Code IN (select z.InHouse_Piece_Delivery_Code from InHouse_Piece_Delivery_Details z where z.Lot_No = '" & Trim(txt_FilterRollNo.Text) & "') "
            End If

            If Trim(txt_FilterPcsNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.InHouse_Piece_Delivery_Code IN (select z.InHouse_Piece_Delivery_Code from InHouse_Piece_Delivery_Details z where z.Pcs_No = '" & Trim(txt_FilterPcsNo.Text) & "') "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , C.* , d.Cloth_Name from InHouse_Piece_Delivery_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN InHouse_Piece_Delivery_Details C ON a.InHouse_Piece_Delivery_Code = c.InHouse_Piece_Delivery_Code LEFT OUTER JOIN Cloth_Head D ON a.Cloth_IdNo = d.Cloth_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.InHouse_Piece_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.InHouse_Piece_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("InHouse_Piece_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("InHouse_Piece_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = (dt2.Rows(i).Item("Cloth_Name").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = (dt2.Rows(i).Item("Lot_No").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = (dt2.Rows(i).Item("Pcs_No").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

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
        Amount_Calculation()
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
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


        With dgv_Selection

            .Rows.Clear()

            chk_SelectAll.Checked = False

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*,d.po_No as Order_No,cl.clothtype_name,b.Weight from InHouse_Piece_Delivery_Details a INNER JOIN ClothType_Head cl ON a.ClothType_IdNo = cl.ClothType_IdNo LEFT OUTER JOIN Weaver_ClothReceipt_Piece_Details b ON a.Lot_Code = b.Lot_Code and a.Pcs_no = b.Piece_no  LEFT OUTER JOIN Weaver_Cloth_receipt_Head c ON b.Weaver_ClothReceipt_Code  = c.Weaver_ClothReceipt_Code LEFT OUTER JOIN JobWork_Pavu_Receipt_Details d ON c.Set_Code1  = d.Set_Code and  c.Beam_no1  = d.Beam_No where a.InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                    .Rows(n).Cells(8).Value = Dt2.Rows(i).Item("folding").ToString
                    .Rows(n).Cells(9).Value = Dt2.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                    .Rows(n).Cells(10).Value = Format(Val(Dt2.Rows(i).Item("Weight").ToString), "########0.00")

                    For j = 0 To .ColumnCount - 1
                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next i

            End If


            Da = New SqlClient.SqlDataAdapter("select a.Lot_Code, Lot_No, Piece_No, folding, Type1_Meters, Type2_Meters, Type3_Meters, Type4_Meters, Type5_Meters, Weight, PackingSlip_Code_Type1, PackingSlip_Code_Type2, PackingSlip_Code_Type3, PackingSlip_Code_Type4, PackingSlip_Code_Type5, Weaving_JobCode_forSelection from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5) and a.cloth_Idno = " & Str(Val(CloIdNo)) & "  and ( (a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 = '') or (a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 = '')  or (a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 = '')  or (a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 = '')  or (a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 = '') ) order by a.for_orderby, a.Weaver_ClothReceipt_Date, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
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
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("folding").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Weight").ToString


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
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("folding").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Weight").ToString

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
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("folding").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Weight").ToString

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
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("folding").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Weight").ToString

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
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("folding").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Weight").ToString

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

                    .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

                    If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then .Rows(RwIndx).Cells(5).Value = ""

                    If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                        Next

                    Else
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                        Next

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
                dgv_Details.Rows(n).Cells(7).Value = ""
                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(9).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(10).Value

            End If

        Next

        Total_Calculation()

        Grid_Cell_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_DeliveryTo.Enabled And cbo_DeliveryTo.Visible Then cbo_DeliveryTo.Focus()

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
            If cbo_Weaving_JobNo.Enabled And cbo_Weaving_JobNo.Visible Then
                cbo_Weaving_JobNo.Focus()
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
                            "    Select  a.JobWork_Order_Code, a.Cloth_IdNo, -1*a.Total_Delivery_Meters from InHouse_Piece_Delivery_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head b ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = c.Cloth_IdNo Where a.Ledger_IdNo = " & Str(Val(LedIdNo))
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

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.Meters1 as balancemeters from JobWork_Order_Details a INNER JOIN InHouse_Piece_Delivery_Head b ON a.JobWork_Order_Code = b.JobWork_Order_Code and a.cloth_idno = b.cloth_idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = c.Cloth_IdNo INNER JOIN " & Trim(Common_Procedures.EntryTempTable) & " d ON a.JobWork_Order_Code = d.Name1 and a.cloth_idno = d.Int1 where b.InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.JobWork_Order_Date, a.for_orderby, a.JobWork_Order_No", con)
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
                cbo_Weaving_JobNo.Text = dgv_order_selection.Rows(i).Cells(1).Value
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


        print_Delivery()

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1021" Then '---- Asia Sizing (Palladam)
        '    pnl_Print.Visible = True
        '    pnl_Back.Enabled = False
        '    If btn_Print_Invoice.Enabled And btn_Print_Invoice.Visible Then
        '        btn_Print_Invoice.Focus()
        '    End If
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
        '    pnl_2.Visible = True
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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Jobwork_Piece_Delivery_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from InHouse_Piece_Delivery_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        'If prn_Status = 1 Then

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.Landscape = False

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

        'Else

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        'End If

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

                            'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
                            'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                            'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
                            'PrintDocument1.DefaultPageSettings.Landscape = False

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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_PageNo = 0
        prn_NoofBmDets = 0
        prn_DetMxIndx = 0
        Total_Weight = 0
        Total_Mtr = 0
        Print_sno = 0

        Erase prn_DetAr

        prn_DetAr = New String(500, 30) {}

        Try

            'da4 = New SqlClient.SqlDataAdapter("select c.cloth_name,c.WEAVE,a.Weaving_JobCode_forSelection,sum(a.meters)as mtrs,j.rate,(sum(a.meters) *j.rate) as total  from InHouse_Piece_Delivery_Details a Left Outer Join InHouse_Piece_Delivery_Head j on j.InHouse_Piece_Delivery_Code=a.InHouse_Piece_Delivery_Code  LEFT OUTER JOIN Cloth_Head c ON j.Cloth_IdNo = c.Cloth_IdNo where a.InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "' Group by c.cloth_name,C.wEAVE,a.Weaving_JobCode_forSelection,j.rate,j.total_amount", con)
            'prn_HdDt_1 = New DataTable
            'da4.Fill(prn_HdDt_1)


            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.*, d.Ledger_mainName as Transport_Name, e.* , n.Count_Name As WarpName , G.Count_Name As WEftName ,lsh.state_name ,lsh.state_Code, f.Ledger_mainName as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.Pan_No as DeliveryTo_PanNo, Dsh.State_Name as DeliveryTo_State_Name, Dsh.State_Code as DeliveryTo_State_Code from InHouse_Piece_Delivery_Head a INNER JOIN company_Head c ON a.Company_IdNo = c.company_Idno INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_IdNo = d.ledger_IdNo LEFT OUTER JOIN Ledger_Head f ON a.Delivery_IdNo = f.ledger_IdNo Left outer join state_head lsh on b.ledger_state_idno=lsh.state_idno LEFT OUTER JOIN State_Head Dsh ON f.Ledger_State_IdNo = Dsh.State_IdNo   LEFT OUTER JOIN Cloth_Head e ON a.Cloth_IdNo = e.Cloth_IdNo LEFT OUTER JOIN Count_Head n ON n.Count_IdNo = e.Cloth_WarpCount_IdNo LEFT OUTER JOIN Count_Head g ON g.Count_IdNo = e.Cloth_WeftCount_IdNo  where a.company_idno= " & Str(Val(lbl_Company.Tag)) & "  and a.InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, c.loom_name,w.weight as wgt  from InHouse_Piece_Delivery_Details a LEFT OUTER JOIN Weaver_Cloth_Receipt_Head b ON a.Lot_Code = b.Weaver_ClothReceipt_Code LEFT OUTER JOIN Weaver_ClothReceipt_Piece_Details w ON b.Weaver_ClothReceipt_Code = w.Lot_Code  and a.pcs_no=w.piece_no LEFT OUTER JOIN Loom_Head c ON b.Loom_IdNo  = c.Loom_IdNo where a.InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then

                    For i = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(i).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            sno = sno + 1
                            prn_DetAr(prn_DetMxIndx, 1) = sno 'Trim(prn_DetDt.Rows(i).Item("Sl_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 2) = ""
                            If IsDBNull(prn_DetDt.Rows(i).Item("loom_name").ToString) = False Then
                                prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("loom_name").ToString)
                            End If

                            If Common_Procedures.settings.CustomerCode = "1186" Then
                                prn_DetAr(prn_DetMxIndx, 3) = prn_DetDt.Rows(i).Item("Pcs_No").ToString
                                prn_DetAr(prn_DetMxIndx, 9) = Trim(prn_DetDt.Rows(i).Item("Weaving_JobCode_forSelection").ToString)
                                prn_DetAr(prn_DetMxIndx, 10) = "" ' Trim(prn_DetDt.Rows(i).Item("Lot_NUmber").ToString)
                                prn_DetAr(prn_DetMxIndx, 11) = Trim(prn_DetDt.Rows(i).Item("SetNo").ToString)

                            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1420" Then
                                prn_DetAr(prn_DetMxIndx, 3) = Trim(prn_DetDt.Rows(i).Item("Pcs_No").ToString)
                            Else
                                prn_DetAr(prn_DetMxIndx, 3) = Trim(prn_DetDt.Rows(i).Item("lot_no").ToString) & "-" & Trim(prn_DetDt.Rows(i).Item("Pcs_No").ToString)
                            End If

                            prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
                            prn_DetAr(prn_DetMxIndx, 5) = Trim(prn_DetDt.Rows(i).Item("lot_no").ToString)

                            If dt3.Rows.Count > 0 Then
                                prn_DetAr(prn_DetMxIndx, 6) = Format(Val(dt3.Rows(i).Item("Weight").ToString), "#########0.00")
                                prn_DetAr(prn_DetMxIndx, 7) = Format((Val(dt3.Rows(i).Item("Weight").ToString)) / (Val(prn_DetDt.Rows(i).Item("Meters").ToString)), "#########0.000")
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1021" Then
            If prn_Status = 1 Then
                Printing_Format1(e)
            Else
                Printing_Format4(e)
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1052" Then '---- Shri Vedha Tex (Karumanthapatti) - Nithya Sizing
            Printing_Format3(e)


        Else
            Printing_Format1(e)
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
        If Common_Procedures.settings.CustomerCode = "1186" Then
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
        If Common_Procedures.settings.CustomerCode = "1186" Then
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
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "DESPATCH TO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 4, LnAr(2))

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "Quatlity :  " & prn_HdDt.Rows(0).Item("Cloth_Name").ToString & "               Total Rolls :  " & prn_HdDt.Rows(0).Item("Total_Rolls").ToString & "              Total Meters :  " & prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString, LMargin + 10, CurY, 0, 0, pFont)


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
            If Common_Procedures.settings.CustomerCode = "1186" Then
                CurY = CurY + TxtHgt
                Dim len1 As Integer = 0

                p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
                len1 = e.Graphics.MeasureString(" Date     : ", pFont).Width
                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "INWARD / OUTWARD  : ", LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt + 5
                pFont = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Ref No   ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  " & prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_No").ToString, LMargin + len1 + 10, CurY, 0, 0, pFont)
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


            If Common_Procedures.settings.CustomerCode = "1186" Or Common_Procedures.settings.CustomerCode = "1420" Or Common_Procedures.settings.CustomerCode = "1441" Then

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
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_Date").ToString)), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

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
    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_No").ToString, CurX, CurY, 0, 0, p1Font)


                CurX = LMargin + 660
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, p1Font)

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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

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
        pnl_2.Visible = False
    End Sub

    Private Sub btn_Print_Invoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Invoice.Click
        prn_Status = 1
        print_Delivery()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint.Click
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

        If Trim(txt_LotSelction.Text) <> "" Or Trim(txt_PcsSelction.Text) <> "" Then

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


    Private Sub Amount_Calculation()
        If FrmLdSTS = True Then Exit Sub
        With dgv_Details_Total
            lbl_total_amount.Text = ""
            If .RowCount <> 0 Then
                lbl_total_amount.Text = Format(Val(txt_rate.Text) * Val(.Rows(0).Cells(4).Value), "########0.00")
            End If
        End With
    End Sub

    Private Sub txt_rate_TextChanged(sender As Object, e As EventArgs) Handles txt_rate.TextChanged
        If FrmLdSTS = True Then Exit Sub
        Amount_Calculation()
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
        If Common_Procedures.settings.CustomerCode = "1186" Then
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
        If Common_Procedures.settings.CustomerCode = "1186" Then
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
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

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
            If Common_Procedures.settings.CustomerCode = "1186" Then
                CurY = CurY + TxtHgt
                Dim len1 As Integer = 0

                p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
                len1 = e.Graphics.MeasureString(" Date     : ", pFont).Width
                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "INWARD / OUTWARD  : ", LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt + 5
                pFont = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Ref No   ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":  " & prn_HdDt.Rows(0).Item("InHouse_Piece_Delivery_No").ToString, LMargin + len1 + 10, CurY, 0, 0, pFont)
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

            If Common_Procedures.settings.CustomerCode <> "1186" And Common_Procedures.settings.CustomerCode <> "1420" Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)
            End If
            If Common_Procedures.settings.CustomerCode = "1186" Or Common_Procedures.settings.CustomerCode = "1420" Then

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

        Dim da As New SqlClient.SqlDataAdapter("Select Eway_BillNo from InHouse_Piece_Delivery_Head where InHouse_Piece_Delivery_Code = '" & NewCode & "'", con)
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
                         "  SELECT               'O'              , '4'             ,   'JOB WORK'        ,    'CHL'    , a.InHouse_Piece_Delivery_No ,a.InHouse_Piece_Delivery_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
                         " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo, L.Ledger_MainName, (case when a.Delivery_Idno <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.Delivery_Idno <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.Delivery_Idno <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.Delivery_Idno <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee, TS.State_Code, (case when a.Delivery_Idno <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode," &
                         " 1                     , 0 , a.Total_Amount     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
                         " a.Vehicle_No, 'R', '" & NewCode & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from InHouse_Piece_Delivery_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L ON a.Ledger_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.Delivery_Idno <> 0 and a.Delivery_Idno = tDELV.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo " &
                         " where a.InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
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
                                          " from InHouse_Piece_Delivery_Head SD Inner Join Cloth_Head I On SD.Cloth_IdNo = I.Cloth_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
                                          " Where SD.InHouse_Piece_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
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
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "InHouse_Piece_Delivery_Head", "Electronic_Reference_No", "InHouse_Piece_Delivery_Code", Pk_Condition)



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

        EWB.CancelEWB(txt_EWayBillNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "InHouse_Piece_Delivery_Head", "Electronic_Reference_No", "InHouse_Piece_Delivery_Code")

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
End Class
