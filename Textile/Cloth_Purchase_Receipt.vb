Imports System.Drawing.Printing

Public Class Cloth_Purchase_Receipt

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CPREC-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private dgv_ActCtrlName As String = ""

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private dgvDet_CboBx_ColNos_Arr As Integer() = {-1}

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
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub clear()

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1


        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_BillNo.Text = ""
        cbo_BillNo.Tag = cbo_BillNo.Text
        cbo_PartyName.Text = ""
        cbo_PartyName.Tag = cbo_PartyName.Text
        cbo_PartyName.Tag = ""
        cbo_Cloth.Text = ""

        cbo_BillNo.Text = ""
        cbo_BillNo.Tag = cbo_BillNo.Text

        txt_Folding.Text = "100"
        txt_NoOfPcs.Text = ""
        txt_NoOfPcs.Tag = txt_NoOfPcs.Text
        txt_PcsNoFrom.Text = "1"

        lbl_PcsNoTo.Text = ""
        txt_Meters.Text = ""
        txt_Note.Text = ""

        cbo_DeliveryAt.Text = ""
        cbo_Delivery_Purpose.Text = ""
        cbo_Type.Text = "DIRECT"

        lbl_Cloth_Purc_Order_Code.Text = ""
        lbl_Cloth_Purc_Order_Slno.Text = ""

        lbl_Cloth_Purc_Order_No.Text = ""
        txt_Pur_Order_Date.Text = ""



        cbo_Delivery_Purpose.Enabled = True

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        dgv_Details.Rows.Clear()
        dgv_Details.AllowUserToAddRows = False

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_Cloth.Enabled = True
        cbo_Cloth.BackColor = Color.White

        txt_NoOfPcs.Enabled = True
        txt_NoOfPcs.BackColor = Color.White

        txt_PcsNoFrom.Enabled = True
        txt_PcsNoFrom.BackColor = Color.White

        txt_Meters.Enabled = True
        txt_Meters.BackColor = Color.White

        cbo_PackType.Text = "PCS"
        chk_Checked_Pcs_Status.Checked = False
        cbo_Cloth.Enabled = True
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
            cbo_PackType.Text = "ROLL"
            chk_Checked_Pcs_Status.Checked = True
            'cbo_Cloth.Enabled = False
        End If


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Cloth.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_Cloth.SelectedIndex = -1


            dgv_Filter_Details.Rows.Clear()
        End If

        dgv_ActCtrlName = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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

        If Me.ActiveControl.Name <> dgv_Details.Name Then
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
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
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

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim LockSTS As Boolean = False
        Dim LtCd As String = ""

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_Name, D.Ledger_Name as Delivery_At , P.Process_Name as Delivery_Purpose,PC.Cloth_Name as Pro_Cloth_Name from Cloth_Purchase_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo left outer join Ledger_Head d on a.Deliver_At_IdNo = d.Ledger_IdNo left outer join Process_Head p on a.Delivery_Purpose_IdNo = p.Process_IdNo left outer join Cloth_Head PC on a.Processed_Cloth_IdNo = PC.Cloth_IdNo Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RecNo.Text = dt1.Rows(0).Item("Cloth_Purchase_Receipt_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Cloth_Purchase_Receipt_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_PartyName.Tag = cbo_PartyName.Text
                cbo_Cloth.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                cbo_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                cbo_BillNo.Tag = cbo_BillNo.Text
                txt_Folding.Text = dt1.Rows(0).Item("Folding").ToString
                txt_NoOfPcs.Text = Val(dt1.Rows(0).Item("noof_pcs").ToString)
                txt_NoOfPcs.Tag = txt_NoOfPcs.Text
                txt_PcsNoFrom.Text = dt1.Rows(0).Item("pcs_fromno").ToString
                lbl_PcsNoTo.Text = dt1.Rows(0).Item("pcs_tono").ToString
                txt_Meters.Text = dt1.Rows(0).Item("ReceiptMeters_Receipt").ToString
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                cbo_PackType.Text = dt1.Rows(0).Item("Pack_Type").ToString

                If Not IsDBNull(dt1.Rows(0).Item("Delivery_At")) Then
                    cbo_DeliveryAt.Text = dt1.Rows(0).Item("Delivery_At")
                End If

                If Not IsDBNull(dt1.Rows(0).Item("Delivery_Purpose")) Then
                    cbo_Delivery_Purpose.Text = dt1.Rows(0).Item("Delivery_Purpose")
                End If

                If Not IsDBNull(dt1.Rows(0).Item("Folding_Receipt")) Then
                    txt_Folding.Text = dt1.Rows(0).Item("Folding_Receipt")
                End If

                If Not IsDBNull(dt1.Rows(0).Item("Pro_Cloth_Name")) Then
                    cbo_Processed_Cloth.Text = dt1.Rows(0).Item("Pro_Cloth_Name").ToString()
                End If

                Enable_Disable_Delivery_Purpose()

                LockSTS = False
                If IsDBNull(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" Then
                        If Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString).ToUpper <> (Trim(Pk_Condition) & Trim(NewCode)).ToUpper Then
                            LockSTS = True
                        End If

                    End If
                End If
                If IsDBNull(dt1.Rows(0).Item("Cloth_Purchase_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Cloth_Purchase_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                If Val(dt1.Rows(0).Item("Checked_Piece_Receipt_Status").ToString) = 1 Then chk_Checked_Pcs_Status.Checked = True
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                cbo_Type.Text = dt1.Rows(0).Item("Receipt_Selection_Type").ToString

                lbl_Cloth_Purc_Order_Code.Text = Trim(dt1.Rows(0).Item("ClothPurchase_Order_Code").ToString)
                lbl_Cloth_Purc_Order_Slno.Text = Trim(dt1.Rows(0).Item("ClothPurchase_Order_Slno").ToString)

                lbl_Cloth_Purc_Order_No.Text = Trim(dt1.Rows(0).Item("ClothPurchase_Order_No").ToString)
                txt_Pur_Order_Date.Text = dt1.Rows(0).Item("ClothPurchase_Order_Date").ToString


                LtCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.LotCode.Purchase_Cloth_Receipt) & "/" & Trim(Common_Procedures.FnYearCode)


                da2 = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(LtCd) & "' and Create_Status = 1 Order by Sl_No, Piece_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        dgv_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Piece_No").ToString
                        dgv_Details.Rows(n).Cells(1).Value = Format(Val(dt2.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("PackingSlip_Code_Type1").ToString
                        If Trim(dgv_Details.Rows(n).Cells(3).Value) = "" Then
                            dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type1").ToString
                        End If

                        If Trim(dgv_Details.Rows(n).Cells(3).Value) <> "" Then
                            dgv_Details.Rows(n).Cells(1).Style.ForeColor = Color.Red
                            dgv_Details.Rows(n).Cells(2).Style.ForeColor = Color.Red

                            dgv_Details.Rows(n).Cells(1).ReadOnly = True
                            dgv_Details.Rows(n).Cells(2).ReadOnly = True

                            LockSTS = True
                        End If

                    Next i

                End If
                dt2.Clear()

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(0).Value = Val(dt1.Rows(0).Item("Total_Receipt_Pcs").ToString)
                    .Rows(0).Cells(1).Value = Format(Val(dt1.Rows(0).Item("Total_Receipt_Meters").ToString), "########0.00")
                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_Receipt_Weight").ToString), "########0.000")
                End With

            End If

            dt1.Clear()

            Grid_Cell_DeSelect()
            dgv_ActCtrlName = ""

            If LockSTS = True Then

                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_Cloth.Enabled = False
                cbo_Cloth.BackColor = Color.LightGray


                txt_NoOfPcs.Enabled = False
                txt_NoOfPcs.BackColor = Color.LightGray

                txt_PcsNoFrom.Enabled = False
                txt_PcsNoFrom.BackColor = Color.LightGray

                txt_Meters.Enabled = False
                txt_Meters.BackColor = Color.LightGray

                dgv_Details.AllowUserToAddRows = False

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

        End Try

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False

    End Sub

    Private Sub Cloth_Purchase_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Cloth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Cloth_Purchase_Receipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable

        Me.Text = ""

        con.Open()

        cbo_PackType.Items.Clear()
        cbo_PackType.Items.Add(" ")
        cbo_PackType.Items.Add("PCS")
        cbo_PackType.Items.Add("ROLL")
        cbo_PackType.Items.Add("BUNDLE")
        cbo_PackType.Items.Add("BALE")

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("ORDER")

        lbl_Checked_Pcs_Status_Caption.Visible = False
        chk_Checked_Pcs_Status.Visible = False
        cbo_BillNo.DropDownStyle = ComboBoxStyle.Simple

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
            cbo_BillNo.DropDownStyle = ComboBoxStyle.DropDown
        End If
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1520" Then '---- SRI RAINBOW COTTON FABRIC
        lbl_Checked_Pcs_Status_Caption.Visible = True
        chk_Checked_Pcs_Status.Visible = True
        'End If

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsNoFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfPcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryAt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Delivery_Purpose.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Processed_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pur_Order_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Cloth_Purc_Order_No.GotFocus, AddressOf ControlGotFocus





        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsNoFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfPcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Delivery_Purpose.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Processed_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pur_Order_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Cloth_Purc_Order_No.LostFocus, AddressOf ControlLostFocus



        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Folding.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfPcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Folding.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        dtp_Date.Text = ""
        msk_date.Text = ""
        cbo_BillNo.Text = ""
        cbo_PartyName.Text = ""
        cbo_PartyName.Tag = ""


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Order_Selection.Visible = False
        pnl_Order_Selection.Left = (Me.Width - pnl_Order_Selection.Width) \ 2
        pnl_Order_Selection.Top = (Me.Height - pnl_Order_Selection.Height) \ 2
        pnl_Order_Selection.BringToFront()




        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True

        If Val(Common_Procedures.settings.FabricProcessing_Entries_Status) = 1 Then
            cbo_Delivery_Purpose.Visible = True
            lbl_Delivery_Purpose.Visible = True

        Else
            cbo_Delivery_Purpose.Visible = False
            lbl_Delivery_Purpose.Visible = False
        End If

        If Val(Common_Procedures.settings.Multi_Godown_Status) = 1 Then
            cbo_DeliveryAt.Visible = True
            lbl_DeliveryAt.Visible = True
            cbo_Delivery_Purpose.Visible = True
            lbl_Delivery_Purpose.Visible = True
        Else
            cbo_DeliveryAt.Visible = False
            lbl_DeliveryAt.Visible = False
            cbo_Delivery_Purpose.Visible = False
            lbl_Delivery_Purpose.Visible = False
        End If


        If Val(Common_Procedures.settings.Sewing_Entries_Status) = 1 Then
            cbo_Processed_Cloth.Visible = True
            lbl_Product.Visible = True

        Else
            cbo_Processed_Cloth.Visible = False
            lbl_Product.Visible = False

        End If


        new_record()

    End Sub

    Private Sub Cloth_Purchase_Receipt_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Cloth_Purchase_Receipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Order_Selection.Visible = True Then
                    btn_Close_order_Selection_Click(sender, e)
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

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf dgv_ActCtrlName = dgv_Details.Name Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False And IsNothing(dgv1.CurrentCell) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If txt_Meters.Enabled And txt_Meters.Visible Then
                                    txt_Meters.Focus()

                                Else
                                    txt_Note.Focus()

                                End If

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

                                txt_PcsNoFrom.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 2)

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
        Dim Qa As Windows.Forms.DialogResult
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Cloth_Purchase_Receipt_Entry, New_Entry, Me, con, "Cloth_Purchase_Receipt_Head", "Cloth_Purchase_Receipt_Code", NewCode, "Cloth_Purchase_Receipt_Date", "(Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cloth_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cloth_Receipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.settings.CustomerCode <> "1186" Then


            Da = New SqlClient.SqlDataAdapter("select count(*) from Cloth_Purchase_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "' and  Weaver_Piece_Checking_Code <> '' and Checked_Piece_Receipt_Status = 0 ", con)
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
        End If
        Da = New SqlClient.SqlDataAdapter("select count(*) from Cloth_Purchase_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "' and Cloth_Purchase_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Purchase Bill Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '' or Bale_UnPacking_Code_Type1 <> '' or Bale_UnPacking_Code_Type2 <> '' or Bale_UnPacking_Code_Type3 <> '' or Bale_UnPacking_Code_Type4 <> '' or Bale_UnPacking_Code_Type5 <> '')", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Packing Slip prepared / Pcs Delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Cloth_Purchase_Receipt_Head", "Cloth_Purchase_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Cloth_Purchase_Receipt_Code, Company_IdNo, for_OrderBy", trans)

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Update ClothPurchase_order_Details set Purchase_Meters = a.Purchase_Meters - b.ReceiptMeters_Receipt from ClothPurchase_order_Details a, Cloth_Purchase_Receipt_Head b Where b.Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "' and b.Receipt_Selection_Type = 'ORDER' and a.ClothPurchase_Order_code = b.ClothPurchase_Order_code and a.ClothPurchase_Order_SlNo = b.ClothPurchase_Order_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cloth_Purchase_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Textile_Processing_Delivery_Head where ClothProcess_Delivery_Code = '" & Pk_Condition & NewCode & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Textile_Processing_Delivery_Details where Cloth_Processing_Delivery_Code = '" & Pk_Condition & NewCode & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from [FabricPurchase_Weaver_Lot_Head] where [Creating_DOC_Ref_Code] = '" & Pk_Condition & NewCode & "'"
            cmd.ExecuteNonQuery()

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

        If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

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


            da = New SqlClient.SqlDataAdapter("select cloth_name from cloth_head order by cloth_name", con)
            da.Fill(dt2)
            cbo_Filter_Cloth.DataSource = dt2
            cbo_Filter_Cloth.DisplayMember = "cloth_name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Cloth.Text = ""

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

            da = New SqlClient.SqlDataAdapter("select top 1 Cloth_Purchase_Receipt_No from Cloth_Purchase_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cloth_Purchase_Receipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Cloth_Purchase_Receipt_No from Cloth_Purchase_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cloth_Purchase_Receipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Cloth_Purchase_Receipt_No from Cloth_Purchase_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cloth_Purchase_Receipt_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Cloth_Purchase_Receipt_No from Cloth_Purchase_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cloth_Purchase_Receipt_No desc", con)
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
                lbl_RecNo.Text = GetNewNo()
            Else
                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Cloth_Purchase_Receipt_Head", "Cloth_Purchase_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            End If

            lbl_RecNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Cloth_Purchase_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cloth_Purchase_Receipt_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Cloth_Purchase_Receipt_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Cloth_Purchase_Receipt_Date").ToString
                End If
            End If
            dt1.Clear()

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

            inpno = InputBox("Enter Rec.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cloth_Purchase_Receipt_No from Cloth_Purchase_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code = '" & Trim(RecCode) & "'", con)
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

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Cloth_Purchase_Receipt_Entry, New_Entry, Me) = False Then Exit Sub

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cloth_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cloth_Receipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Rec No.", "FOR NEW REC INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cloth_Purchase_Receipt_No from Cloth_Purchase_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Rec No", "DOES NOT INSERT NEW REC...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RecNo.Text = Trim(UCase(inpno))

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
        Dim vTotRcptPcs As Integer, vTotRcptMtrs As String, vTotRcptWgt As String
        Dim WftCnt_ID As Integer = 0
        Dim EntID As String = 0
        Dim Dup_PcNo As String = ""
        Dim PurcCode As String = ""
        Dim PcsChkCode As String = ""
        Dim Nr As Integer = 0
        Dim LtCd As String = ""
        Dim LtNo As String = ""
        Dim clthStock_In As String = ""
        Dim clthmtrspcs As Single = 0
        Dim vCloStk_QTY As String = 0
        Dim vOrdByNo As String = ""
        Dim vSQL1 As String = ""
        Dim vLotCd As String = ""
        Dim vLotNo As String = ""
        Dim vStkOf_Pos_IdNo As Integer = 0
        Dim vWgt_per_Mtr As String = 0
        Dim vBrCode_Typ1 As String = ""
        Dim vPcNo As String = ""
        Dim vPcSubNo As String = ""
        Dim vOrdByPcNo As String = ""
        Dim Checked_Pcs_Sts As String = 0
        Dim Del_At_IdNo As Integer = 0
        Dim Del_Purpose_IdNo As Integer = 0
        Dim Lot_IdNo As Integer

        Dim vPUR_ORDER_CODE As String = ""
        Dim vPUR_ORDER_SLNO As Integer = 0
        Dim vPUR_ORDER_NO As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Cloth_Purchase_Receipt_Entry, New_Entry, Me, con, "Cloth_Purchase_Receipt_Head", "Cloth_Purchase_Receipt_Code", NewCode, "Cloth_Purchase_Receipt_Date", "(Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Cloth_Purchase_Receipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Cloth_Receipt_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If


        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If Clo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        If Len(Trim(cbo_DeliveryAt.Text)) > 0 And cbo_DeliveryAt.Visible Then
            Del_At_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryAt.Text)
        End If

        If Len(Trim(cbo_Delivery_Purpose.Text)) > 0 And cbo_Delivery_Purpose.Visible Then
            Del_Purpose_IdNo = Common_Procedures.Process_NameToIdNo(con, cbo_Delivery_Purpose.Text)
        End If

        If Len(Trim(cbo_Processed_Cloth.Text)) > 0 And cbo_Processed_Cloth.Visible Then
            Pro_Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Processed_Cloth.Text)
        End If

        Dim Del_Type As String
        If Del_At_IdNo <> 0 Then
            Del_Type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "Ledger_IdNo =" & Del_At_IdNo.ToString)
        End If

        If Del_At_IdNo <> 0 And Del_Type <> "GODOWN" And Del_Purpose_IdNo = 0 Then
            MessageBox.Show("Delivery Purpose (Process Name) is Manadatory when Delivery Location is not Own Godown.", "Process Name", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Del_Purpose_IdNo <> 0 And Pro_Clo_ID = 0 Then
            MessageBox.Show("Procesed Cloth Name is Manadatory when Delivery Location is not Own Godown.", "Process Name", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Del_At_IdNo = 0 Then
            Del_At_IdNo = 4
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1186" Then '---- UNITED WEAVES (PALLADAM)
            If Trim(cbo_BillNo.Text) <> "" Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                Da = New SqlClient.SqlDataAdapter("select * from Cloth_Purchase_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(Led_ID)) & " and Bill_no = '" & Trim(cbo_BillNo.Text) & "' and Cloth_Purchase_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Cloth_Purchase_Receipt_Code <> '" & Trim(NewCode) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    MessageBox.Show("Duplicate Party Dc No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_BillNo.Enabled And cbo_BillNo.Visible Then cbo_BillNo.Focus()
                    Exit Sub
                End If
                Dt1.Clear()
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

        vTotRcptPcs = 0 : vTotRcptMtrs = 0 : vTotRcptWgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotRcptPcs = Val(dgv_Details_Total.Rows(0).Cells(0).Value)
            vTotRcptMtrs = Val(dgv_Details_Total.Rows(0).Cells(1).Value)
            vTotRcptWgt = Val(dgv_Details_Total.Rows(0).Cells(2).Value)
        End If

        If Val(vTotRcptMtrs) <> 0 Then
            If Format(Val(vTotRcptMtrs), "#########0.00") <> Format(Val(txt_Meters.Text), "#########0.00") Then
                MessageBox.Show("Mismatch of Receipt Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Meters.Enabled And txt_Meters.Visible Then txt_Meters.Focus()
                Exit Sub
            End If
        End If

        Checked_Pcs_Sts = 0
        If chk_Checked_Pcs_Status.Checked = True Then Checked_Pcs_Sts = 1

        ' --- CODE BY  GOPI 2024-01-23

        If Trim(UCase(cbo_Type.Text)) = "" Or Trim(UCase(cbo_Type.Text)) <> "ORDER" Then
            cbo_Type.Text = "DIRECT"
        End If

        If Trim(UCase(cbo_Type.Text)) = Trim(UCase("ORDER")) Then
            vPUR_ORDER_CODE = Trim(lbl_Cloth_Purc_Order_Code.Text)
            vPUR_ORDER_SLNO = Val(lbl_Cloth_Purc_Order_Slno.Text)
            vPUR_ORDER_NO = Trim(lbl_Cloth_Purc_Order_No.Text)
        Else
            vPUR_ORDER_CODE = ""
            vPUR_ORDER_SLNO = 0
            vPUR_ORDER_NO = ""
        End If


        ' ---



        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then
                    lbl_RecNo.Text = GetNewNo(tr)
                Else
                    lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Cloth_Purchase_Receipt_Head", "Cloth_Purchase_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                End If

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@CloPurRecDate", Convert.ToDateTime(msk_date.Text))

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_Code, Cloth_Purchase_Code from Cloth_Purchase_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            PcsChkCode = ""
            PurcCode = ""

            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                End If
                If IsDBNull(Dt1.Rows(0).Item("Cloth_Purchase_Code").ToString) = False Then
                    PurcCode = Dt1.Rows(0).Item("Cloth_Purchase_Code").ToString
                End If
            End If

            Dt1.Clear()

            Dim Lot_No As String = Val(lbl_RecNo.Text)
            Dim Lot_Code As String = lbl_Company.Tag.ToString + "/" + Lot_No.ToString + "/" + Common_Procedures.FnYearCode.ToString
            Dim Lot_Code_forSelection As String = Lot_No.ToString + "/" + Common_Procedures.FnYearCode.ToString + "/" + lbl_Company.Tag.ToString

            If New_Entry = True Then

                If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then

                    If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then

                        cmd.CommandText = " If not exists (Select * from Lot_Head WHERE Lot_No = '" & Lot_Code_forSelection & "') begin " &
                                         " insert into Lot_Head ([Lot_IdNo]                                      ,	[Lot_No]                      ,	[Sur_Name]                                                             ,	[Lot_Description] ,	[Lot_Main_Name]               ,	            [Lot_Fn_Yr_Code]                  ,	[Auto_Created] ,         Ledger_IdNo     ,            Party_BillNo         ) " &
                                         "                Values( (select isnull(max(Lot_IdNo),0)+1 from Lot_Head),'" & Lot_Code_forSelection & "','" & Common_Procedures.Remove_NonCharacters(Lot_Code_forSelection) & "' ,            ''        ,'" & Lot_Code_forSelection & "','" & Common_Procedures.FnYearCode.ToString & "',       1        , " & Str(Val(Led_ID)) & ", '" & Trim(cbo_BillNo.Text) & "' ) end "
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = " If exists (Select * from Lot_Head WHERE Lot_No = '" & Lot_Code_forSelection & "') begin " &
                                     " Update Lot_Head set Ledger_IdNo = " & Str(Val(Led_ID)) & ", Party_BillNo = '" & Trim(cbo_BillNo.Text) & "'   WHERE Lot_No = '" & Lot_Code_forSelection & "' end "
                        cmd.ExecuteNonQuery()


                        cmd.CommandText = "INSERT INTO [FabricPurchase_Weaver_Lot_Head] (	[FabricPurchase_Weaver_Lot_IdNo]                                                                 ,	[FabricPurchase_Weaver_Lot_No] ,	[FabricPurchase_Weaver_Lot_Code] ,	[FabricPurchase_Weaver_Lot_Code_forSelection] ,	[For_OrderBy]                                                          ,	[Sur_Name]                                            ,	[FabricPurchase_Weaver_Lot_Date] ,	[Ledger_IdNo]        ,	[Creating_DOC_Ref_Code], Cloth_IdNo                  ,Lot_IdNo)" &
                                                                  "VALUES           ((Select isnull(max([FabricPurchase_Weaver_Lot_IdNo] ),0)+1   from [FabricPurchase_Weaver_Lot_Head]) ,'" & lbl_RecNo.Text & "'         ,'" & Lot_Code & "'                   ,'" & Lot_Code_forSelection & "'                  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",'" & Common_Procedures.Remove_NonCharacters(Lot_Code) & "',   @CloPurRecDate                   ," & Led_ID.ToString & ",'" & Pk_Condition & NewCode & "'," & Clo_ID.ToString & "," & Lot_IdNo.ToString & ")"
                        cmd.ExecuteNonQuery()

                    End If

                End If


            Else

                If Common_Procedures.settings.Continuous_Fabric_Lot_No_for_Purchase_Weaver = 1 Then

                    cmd.CommandText = " If not exists (Select * from Lot_Head WHERE Lot_No = '" & Lot_Code_forSelection & "') begin " &
                                         " insert into Lot_Head ([Lot_IdNo]                                      ,	[Lot_No]                      ,	[Sur_Name]                                                             ,	[Lot_Description] ,	[Lot_Main_Name]               ,	[Lot_Fn_Yr_Code]                  ,	[Auto_Created]           ,Ledger_IdNo,            Party_BillNo         ) " &
                                         "                Values( (select isnull(max(Lot_IdNo),0)+1 from Lot_Head),'" & Lot_Code_forSelection & "','" & Common_Procedures.Remove_NonCharacters(Lot_Code_forSelection) & "' ,            ''        ,'" & Lot_Code_forSelection & "','" & Common_Procedures.FnYearCode.ToString & "', 1            ," & Led_ID.ToString & ", '" & Trim(cbo_BillNo.Text) & "' ) end "
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = " If exists (Select * from Lot_Head WHERE Lot_No = '" & Lot_Code_forSelection & "') begin " &
                                      " Update Lot_Head set Ledger_IdNo = " & Led_ID.ToString & " , Party_BillNo = '" & Trim(cbo_BillNo.Text) & "' WHERE Lot_No = '" & Lot_Code_forSelection & "' end "
                    cmd.ExecuteNonQuery()

                    'Lot_IdNo = Common_Procedures.Lot_NoToIdNo(con, Lot_Code_forSelection, tr)

                    cmd.CommandText = "UPDATE [FabricPurchase_Weaver_Lot_Head] SET 	[FabricPurchase_Weaver_Lot_Date] = @CloPurRecDate ,	[Ledger_IdNo]  = " & Led_ID.ToString & ", Cloth_IdNo = " & Clo_ID.ToString & ",Lot_IdNo = " & Lot_IdNo.ToString & " where [Creating_DOC_Ref_Code] = '" & Pk_Condition & NewCode & "'"
                    cmd.ExecuteNonQuery()

                End If

            End If


            Lot_IdNo = Common_Procedures.Lot_NoToIdNo(con, Lot_Code_forSelection, tr)


            If New_Entry = True Then

                cmd.CommandText = "Insert into Cloth_Purchase_Receipt_Head ( Cloth_Purchase_Receipt_Code,             Company_IdNo         ,       Cloth_Purchase_Receipt_No  ,                               for_OrderBy                              , Cloth_Purchase_Receipt_Date,           Ledger_IdNo   ,      Cloth_IdNo    ,            Bill_No                   ,              Folding_Receipt        ,              Folding        ,           noof_pcs            ,             pcs_fromno         ,              pcs_tono        ,           ReceiptMeters_Receipt        ,            Receipt_Meters            ,                  Note       ,  Total_Receipt_Pcs      ,           Total_Receipt_Meters    , Total_Receipt_Weight , Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment, Cloth_Purchase_Code, Cloth_Purchase_Increment         , User_IdNo                                 , Checked_Piece_Receipt_Status, Deliver_At_IdNo                 , Delivery_Purpose_IdNo               ,Lot_IdNo       ,Processed_Cloth_IdNo       ,            Receipt_Selection_Type     ,   ClothPurchase_Order_No      ,   ClothPurchase_Order_Code      ,   ClothPurchase_Order_Slno   ,       ClothPurchase_Order_Date ,                 Pack_Type) " &
                                                " Values  ( '" & Trim(NewCode) & "'                     , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "'   , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",      @CloPurRecDate        , " & Str(Val(Led_ID)) & "," & Val(Clo_ID) & " ,       '" & Trim(cbo_BillNo.Text) & "',   " & Str(Val(txt_Folding.Text)) & ",   " & Str(Val(txt_Folding.Text)) & ", " & Val(txt_NoOfPcs.Text) & " , " & Val(txt_PcsNoFrom.Text) & ", " & Val(lbl_PcsNoTo.Text) & ", " & Str(Val(txt_Meters.Text)) & ", " & Str(Val(vTotRcptMtrs)) & ",    '" & Trim(txt_Note.Text) & "', " & Str(Val(vTotRcptPcs)) & ", " & Str(Val(vTotRcptMtrs)) & ", " & Str(Val(vTotRcptWgt)) & ",              ''          ,             0                  ,         ''       ,           0        ," & Val(Common_Procedures.User.IdNo) & "    , " & Val(Checked_Pcs_Sts) & "," & Del_At_IdNo.ToString & "," & Del_Purpose_IdNo.ToString & "," & Lot_IdNo.ToString & "," & Pro_Clo_ID.ToString & ",'" & Trim(UCase(cbo_Type.Text)) & "'   , '" & Trim(vPUR_ORDER_NO) & "' , '" & Trim(vPUR_ORDER_CODE) & "' ,  " & Val(vPUR_ORDER_SLNO) & ",'" & Trim(txt_Pur_Order_Date.Text) & "','" & Trim(cbo_PackType.Text) & "' )"
                cmd.ExecuteNonQuery()


                'If Trim(UCase(cbo_Type.Text)) = "ORDER" And Trim(lbl_Cloth_Purc_Order_Code.Text) <> "" Then
                '    Nr = 0
                '    cmd.CommandText = "Update ClothPurchase_Order_Details set Purchase_Meters = Purchase_Meters + " & Str(Val(txt_Meters.Text)) & " Where ClothPurchase_Order_Code = '" & Trim(lbl_Cloth_Purc_Order_Code.Text) & "' and ClothPurchase_Order_Slno = " & Str(Val(lbl_Cloth_Purc_Order_Slno.Text)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                '    Nr = cmd.ExecuteNonQuery()
                '    If Nr = 0 Then
                '        Throw New ApplicationException("Mismatch of Order and Party Details")
                '        Exit Sub
                '    End If
                'End If

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Cloth_Purchase_Receipt_Head", "Cloth_Purchase_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Cloth_Purchase_Receipt_Code, Company_IdNo, for_OrderBy", tr)

                cmd.CommandText = "Update ClothPurchase_order_Details set Purchase_Meters = a.Purchase_Meters - b.ReceiptMeters_Receipt from ClothPurchase_order_Details a, Cloth_Purchase_Receipt_Head b Where b.Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "' and b.Receipt_Selection_Type = 'ORDER' and a.ClothPurchase_Order_code = b.ClothPurchase_Order_code and a.ClothPurchase_Order_SlNo = b.ClothPurchase_Order_SlNo"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Cloth_Purchase_Receipt_Head Set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0, Deliver_At_IdNo = " & Del_At_IdNo.ToString & " , Delivery_Purpose_IdNo = " & Del_Purpose_IdNo.ToString & ",Lot_IdNo = " & Lot_IdNo.ToString & " Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Cloth_Purchase_Receipt_Date = @CloPurRecDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Cloth_IdNo = " & Val(Clo_ID) & " , Bill_No  = '" & Trim(cbo_BillNo.Text) & "', Folding_Receipt = " & Val(txt_Folding.Text) & " , Folding = " & Val(txt_Folding.Text) & " , noof_pcs = " & Val(txt_NoOfPcs.Text) & " , pcs_fromno = " & Val(txt_PcsNoFrom.Text) & " , pcs_tono = " & Val(lbl_PcsNoTo.Text) & " , ReceiptMeters_Receipt = " & Val(txt_Meters.Text) & ",  Note = '" & Trim(txt_Note.Text) & "', Total_Receipt_Pcs = " & Str(Val(vTotRcptPcs)) & ", Total_Receipt_Meters = " & Str(Val(vTotRcptMtrs)) & ", Total_Receipt_Weight = " & Str(Val(vTotRcptWgt)) & ", User_idNo = " & Val(Common_Procedures.User.IdNo) & " ,Checked_Piece_Receipt_Status = " & Val(Checked_Pcs_Sts) & ",Deliver_At_IdNo = " & Del_At_IdNo.ToString & ", Delivery_Purpose_IdNo = " & Del_Purpose_IdNo.ToString & ",Lot_IdNo = " & Lot_IdNo.ToString & ",Processed_Cloth_IdNo = " & Pro_Clo_ID.ToString & " ,Receipt_Selection_Type ='" & Trim(UCase(cbo_Type.Text)) & "' ,ClothPurchase_Order_No  ='" & Trim(lbl_Cloth_Purc_Order_No.Text) & "' ,  ClothPurchase_Order_Code   ='" & Trim(lbl_Cloth_Purc_Order_Code.Text) & "'  ,  ClothPurchase_Order_Slno = " & Val(lbl_Cloth_Purc_Order_Slno.Text) & " ,ClothPurchase_Order_Date = '" & Trim(txt_Pur_Order_Date.Text) & "' , Pack_Type = '" & Trim(cbo_PackType.Text) & "'   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Receipt_Meters = " & Val(vTotRcptMtrs) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = '' and Cloth_Purchase_Code = ''"
                cmd.ExecuteNonQuery()
                'cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Receipt_Meters = " & Val(txt_Meters.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = '' and Cloth_Purchase_Code = ''"
                'cmd.ExecuteNonQuery()




            End If


            ' --- GOPI 2024-01-23

            If Val(txt_Meters.Text) <> 0 Then

                If Trim(UCase(cbo_Type.Text)) = "ORDER" And Trim(lbl_Cloth_Purc_Order_Code.Text) <> "" Then
                    Nr = 0
                    cmd.CommandText = "Update ClothPurchase_Order_Details set Purchase_Meters = Purchase_Meters + " & Str(Val(txt_Meters.Text)) & " Where ClothPurchase_Order_Code = '" & Trim(lbl_Cloth_Purc_Order_Code.Text) & "' and ClothPurchase_Order_Slno = " & Str(Val(lbl_Cloth_Purc_Order_Slno.Text)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                    Nr = cmd.ExecuteNonQuery()
                    If Nr = 0 Then
                        Throw New ApplicationException("Mismatch of Order and Party Details")
                        Exit Sub
                    End If
                End If
            End If

            ' ---

            '--------Deva 

            cmd.CommandText = "Delete from Textile_Processing_Delivery_Head where ClothProcess_Delivery_Code = '" & Pk_Condition & NewCode & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Textile_Processing_Delivery_Details where Cloth_Processing_Delivery_Code = '" & Pk_Condition & NewCode & "'"
            cmd.ExecuteNonQuery()

            If Del_At_IdNo <> 0 And Del_Purpose_IdNo <> 0 Then

                cmd.CommandText = "Insert into Textile_Processing_Delivery_Head(ClothProcess_Delivery_Code, Company_IdNo                             , ClothProcess_Delivery_No  , for_OrderBy                     , ClothProcess_Delivery_Date ,           Ledger_IdNo        , Purchase_OrderNo, Transport_IdNo, Freight_Charges , Note                          ,                Total_Pcs          ,Total_Qty                 , Total_Meters                     , Total_Weight                  , Processing_Idno          , JobOrder_No                             ,  User_idNo  , Vehicle_No ,FabricPurchase_Weaver_Lot_IdNo   ,Lot_IdNo                 ,Folding)
                Values ('" & Pk_Condition & Trim(NewCode) & "'         , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",        @CloPurRecDate      , " & Str(Val(Del_At_IdNo)) & ", ''              ,   0           ,       0         ,  '" & Trim(txt_Note.Text) & "', " & Str(Val(txt_NoOfPcs.Text)) & ", 0                         , " & Str(Val(txt_Meters.Text)) & ", " & Str(Val(vTotRcptWgt)) & " ,  " & Str(Val(Del_Purpose_IdNo)) & ",'" & lbl_RecNo.Text & "' ," & Val(Common_Procedures.User.IdNo) & " ,''                        ," & Lot_IdNo.ToString & "        ," & Lot_IdNo.ToString & "," & Val(txt_Folding.Text).ToString & ")"
                cmd.ExecuteNonQuery()



                cmd.CommandText = "Insert into Textile_Processing_Delivery_Details(Cloth_Processing_Delivery_Code               , Company_IdNo                     , Cloth_Processing_Delivery_No      , for_OrderBy                                                             , Cloth_Processing_Delivery_Date,Sl_No, Ledger_IdNo              ,  Item_Idno             ,Item_To_Idno                 , Colour_Idno ,Processing_Idno                 ,   Bales   ,  Bales_Nos  ,  Delivery_Pcs                ,Delivery_Qty ,Meter_Qty                         ,Delivery_Meters                    ,Delivery_Weight                , PackingSlip_Codes , ClothProcessing_Delivery_PackingSlno ,FabricPurchase_Weaver_Lot_IdNo        ,Folding             ,Lot_IdNo) " &
                                                                     "Values ('" & Pk_Condition & Trim(NewCode) & "'       , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "'     , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @CloPurRecDate                , 1   , " & Str(Val(Del_At_IdNo)) & " ," & Str(Val(Clo_ID)) & ", " & Str(Val(Pro_Clo_ID)) & ", 0           , " & Val(Del_Purpose_IdNo) & " ,0           ,''           , " & Val(txt_NoOfPcs.Text) & ", 0           ,      0                            , " & Str(Val(txt_Meters.Text)) & ", " & Str(Val(vTotRcptWgt)) & " ,''                 ,''                               ," & Lot_IdNo.ToString & "," & Val(txt_Folding.Text).ToString & "," & Lot_IdNo.ToString & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Delete Textile_Processing_Delivery_Head where ClothProcess_Delivery_Code = '" & Pk_Condition & NewCode & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete Textile_Processing_Delivery_Details where Cloth_Processing_Delivery_Code = '" & Pk_Condition & NewCode & "'"
                cmd.ExecuteNonQuery()

            End If

            '/----------Deva


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Cloth_Purchase_Receipt_Head", "Cloth_Purchase_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Cloth_Purchase_Receipt_Code, Company_IdNo, for_OrderBy", tr)

            If chk_Checked_Pcs_Status.Checked = True Then

                cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Weaver_Piece_Checking_Date = @CloPurRecDate, Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", ReceiptMeters_Checking = " & Str(Val(txt_Meters.Text)) & ", Receipt_Meters = " & Str(Val(txt_Meters.Text)) & " , Type1_Checking_Meters = " & Str(Val(txt_Meters.Text)) & ", Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = " & Str(Val(txt_Meters.Text)) & ",Lot_IdNo = " & Lot_IdNo.ToString & " Where Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "'"
                ' cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Weaver_Piece_Checking_Date = @CloPurRecDate, Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", ReceiptMeters_Checking = " & Str(Val(vTotRcptMtrs)) & ", Receipt_Meters = " & Str(Val(vTotRcptMtrs)) & " , Type1_Checking_Meters = " & Str(Val(vTotRcptMtrs)) & ", Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = " & Str(Val(vTotRcptMtrs)) & ",Lot_IdNo = " & Lot_IdNo.ToString & " Where Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "'"
                Nr = cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)
            Partcls = "CloRcpt : LotNo. " & Trim(lbl_RecNo.Text)
            PBlNo = Trim(lbl_RecNo.Text)

            LtNo = Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.LotCode.Purchase_Cloth_Receipt)
            LtCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.LotCode.Purchase_Cloth_Receipt) & "/" & Trim(Common_Procedures.FnYearCode)


            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(LtCd) & "' and Create_Status = 1 and (Weaver_Piece_Checking_Code = '' or Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')  and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = '' "
            cmd.ExecuteNonQuery()

            'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_ClothReceipt_Piece_Details", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Beam_No,Pcs,Meters_Pc,Meters", "Sl_No", "Weaver_ClothReceipt_Code, For_OrderBy, Company_IdNo, Cloth_Purchase_Receipt_No, Cloth_Purchase_Receipt_Date, Ledger_Idno", tr)

            vStkOf_Pos_IdNo = Val(Common_Procedures.CommonLedger.OwnSort_Ac)    '--- Val(Common_Procedures.CommonLedger.Godown_Ac)


            With dgv_Details

                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Or chk_Checked_Pcs_Status.Checked = True Then

                        Sno = Sno + 1

                        vPcNo = Trim(.Rows(i).Cells(0).Value)

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
                            vPcSubNo = get_RollNo_from_PieceNo(vPcNo)
                            vOrdByPcNo = Common_Procedures.OrderBy_CodeToValue(vPcSubNo)

                        Else
                            vOrdByPcNo = Common_Procedures.OrderBy_CodeToValue(vPcNo)

                        End If

                        Nr = 0

                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Weaver_ClothReceipt_Date = @CloPurRecDate, Sl_No = " & Str(Val(Sno)) & ", PieceNo_OrderBy = " & Str(Val(vOrdByPcNo)) & ", ReceiptMeters_Receipt = " & Val(.Rows(i).Cells(1).Value) & ", Create_Status = 1 where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(LtCd) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then

                            cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_ClothReceipt_Code ,            Company_IdNo          ,      Weaver_ClothReceipt_No   ,                               for_OrderBy                              , Weaver_ClothReceipt_Date,        Lot_Code     ,          Lot_No     ,         Ledger_Idno     ,              StockOff_IdNo       ,                                       WareHouse_IdNo      ,     Cloth_IdNo          ,           Folding_Receipt         ,              Folding              ,         Sl_No        ,                     Piece_No           ,        PieceNo_OrderBy       ,     ReceiptMeters_Receipt           ,                Receipt_Meters       ,                   Weight            , Create_Status ) " &
                                                                "  Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",       @CloPurRecDate    , '" & Trim(LtCd) & "', '" & Trim(LtNo) & "', " & Str(Val(Led_ID)) & ", " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Clo_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(0).Value) & "', " & Str(Val(vOrdByPcNo)) & ", " & Val(.Rows(i).Cells(1).Value) & ", " & Val(.Rows(i).Cells(1).Value) & ", " & Val(.Rows(i).Cells(2).Value) & ",      1        )"
                            cmd.ExecuteNonQuery()

                        End If

                        If chk_Checked_Pcs_Status.Checked = True And Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Then

                            vWgt_per_Mtr = 0
                            If Val(.Rows(i).Cells(1).Value) <> 0 Then
                                vWgt_per_Mtr = Format(Val(.Rows(i).Cells(2).Value) / Val(.Rows(i).Cells(1).Value), "##########0.000")
                            End If

                            vBrCode_Typ1 = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(LtNo)) & Trim(UCase((.Rows(i).Cells(0).Value))) & "1"

                            vSQL1 = "Update Weaver_ClothReceipt_Piece_Details set  Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "',  Weaver_Piece_Checking_No = '" & Trim(lbl_RecNo.Text) & "', Weaver_Piece_Checking_Date = '" & Trim(Format(dtp_Date.Value, "MM/dd/yyyy")) & "', Lot_Code = '" & Trim(LtCd) & "' , Lot_No = '" & Trim(LtNo) & "' , Ledger_Idno = " & Str(Val(Led_ID)) & ", StockOff_IdNo = " & Str(Val(vStkOf_Pos_IdNo)) & ", Cloth_IdNo = " & Str(Val(Clo_ID)) & ", Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(Sno)) & ", PieceNo_OrderBy = " & Str(Val(vOrdByPcNo)) & ", ReceiptMeters_Checking = " & Str(Val(.Rows(i).Cells(1).Value)) & ", Receipt_Meters = " & Str(Val(.Rows(i).Cells(1).Value)) & ", Loom_No = '', Loom_IdNo = 0, Pick = 0, Width = 0, Type1_Meters = " & Str(Val(.Rows(i).Cells(1).Value)) & ", Type2_Meters = 0, Type3_Meters = 0, Type4_Meters  = 0, Type5_Meters = 0, Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(1).Value)) & ", Weight = " & Str(Val(.Rows(i).Cells(2).Value)) & ", Weight_Meter = " & Str(Val(vWgt_per_Mtr)) & ", Remarks = '', WareHouse_IdNo = " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", Checked_Pcs_Barcode_Type1 = '" & Trim(vBrCode_Typ1) & "', Checked_Pcs_Barcode_Type2 = '', Checked_Pcs_Barcode_Type3 = '', Checked_Pcs_Barcode_Type4 = '', Checked_Pcs_Barcode_Type5 = '' , Checker_Idno = 0 , Folder_idno = 0, Checker_Wgs_per_Mtr = 0, Folder_Wgs_per_Mtr = 0 Where Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(LtCd) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
                            cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(vSQL1), "'", "''") & "'"
                            Nr = cmd.ExecuteNonQuery()

                        End If

                    End If

                Next

            End With


            If (Trim(PcsChkCode) = "" Or Trim(PcsChkCode).ToUpper = (Trim(Pk_Condition) & Trim(NewCode)).ToUpper) And Trim(PurcCode) = "" Then

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

                If Trim(UCase(clthStock_In)) = "PCS" Then
                    vCloStk_QTY = Val(txt_NoOfPcs.Text)
                Else
                    vCloStk_QTY = Val(txt_Meters.Text)
                End If

                Dim vTYP1MTRS As String = 0

                vTYP1MTRS = 0
                If chk_Checked_Pcs_Status.Checked = True Then
                    vTYP1MTRS = vCloStk_QTY
                    vCloStk_QTY = 0
                End If

                cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Val(txt_Meters.Text) <> 0 And (Del_Purpose_IdNo = 0 Or Del_At_IdNo = 0) Then

                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,                                   StockOff_IdNo           ,       DeliveryTo_Idno                  ,       ReceivedFrom_Idno ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     ,               Folding             ,             UnChecked_Meters ,          Meters_Type1       , Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ,Lot_IdNo) " &
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @CloPurRecDate, " & Str(Val(Common_Procedures.CommonLedger.OwnSort_Ac)) & ", " & Str(Val(Del_At_IdNo.ToString)) & ", " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(vCloStk_QTY)) & ", " & Str(Val(vTYP1MTRS)) & " ,       0     ,       0     ,       0     ,       0      ," & Lot_IdNo.ToString & ") "
                    cmd.ExecuteNonQuery()

                Else

                    cmd.CommandText = "Delete from  Stock_Cloth_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                End If

            End If




            tr.Commit()


            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RecNo.Text)
                End If

            Else
                move_record(lbl_RecNo.Text)

            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        cbo_PartyName.Tag = cbo_PartyName.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )) or Show_In_All_Entry = 1)   ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_date, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )) or Show_In_All_Entry = 1)  ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )) or Show_In_All_Entry = 1)  ", "(Ledger_idno = 0)")

        'If Asc(e.KeyChar) = 13 Then
        '    e.Handled = True

        '    'If cbo_Cloth.Enabled = False Then
        '    '    If cbo_PartyName.Tag.ToString.ToUpper <> cbo_PartyName.Text.ToString.ToUpper Then
        '    '        cbo_PartyName.Tag = cbo_PartyName.Text
        '    '        Get_Fabric_Name_from_Purchase_Bill_Entry()
        '    '    End If
        '    'End If

        '    cbo_BillNo.Focus()

        'End If

    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_PartyName_LostFocus(sender As Object, e As EventArgs) Handles cbo_PartyName.LostFocus
        'If cbo_Cloth.Enabled = False Then
        '    If cbo_PartyName.Tag.ToString.ToUpper <> cbo_PartyName.Text.ToString.ToUpper Then
        '        cbo_PartyName.Tag = cbo_PartyName.Text
        '        Get_Fabric_Name_from_Purchase_Bill_Entry()
        '    End If
        'End If
    End Sub

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus

        Dim vCLO_CONDT As String = ""
        Dim Led_ID As String = 0

        vCLO_CONDT = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
            vCLO_CONDT = "Cloth_IdNo IN (select sq2.Cloth_IdNo from Cloth_Purchase_Head sq1, Cloth_Purchase_Details sq2 where sq1.Ledger_IdNo = " & Str(Val(Led_ID)) & " and sq1.Bill_No = '" & Trim(cbo_BillNo.Text) & "' and sq1.Cloth_Purchase_Code = sq2.Cloth_Purchase_Code )"
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", vCLO_CONDT, "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Dim vCLO_CONDT As String = ""
        Dim Led_ID As String = 0

        vCLO_CONDT = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
            vCLO_CONDT = "Cloth_IdNo IN (select sq2.Cloth_IdNo from Cloth_Purchase_Head sq1, Cloth_Purchase_Details sq2 where sq1.Ledger_IdNo = " & Str(Val(Led_ID)) & " and sq1.Bill_No = '" & Trim(cbo_BillNo.Text) & "' and sq1.Cloth_Purchase_Code = sq2.Cloth_Purchase_Code )"
        End If

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, cbo_PackType, txt_Folding, "Cloth_Head", "Cloth_Name", vCLO_CONDT, "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Dim vCLO_CONDT As String = ""
        Dim Led_ID As String = 0

        vCLO_CONDT = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
            vCLO_CONDT = "Cloth_IdNo IN (select sq2.Cloth_IdNo from Cloth_Purchase_Head sq1, Cloth_Purchase_Details sq2 where sq1.Ledger_IdNo = " & Str(Val(Led_ID)) & " and sq1.Bill_No = '" & Trim(cbo_BillNo.Text) & "' and sq1.Cloth_Purchase_Code = sq2.Cloth_Purchase_Code )"
        End If

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, txt_Folding, "Cloth_Head", "Cloth_Name", vCLO_CONDT, "(Cloth_IdNo = 0 )")

    End Sub

    Private Sub cbo_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth.Name
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

        Try

            Condt = ""
            Led_IdNo = 0
            Clo_IdNo = 0


            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cloth_Purchase_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Cloth_Purchase_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cloth_Purchase_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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



            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Cloth_Name  from Cloth_Purchase_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Purchase_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Cloth_Purchase_Receipt_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Cloth_Purchase_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Cloth_Purchase_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Folding").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("noof_pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Receipt_Meters").ToString), "########0.00")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)  ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Cloth, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)  ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, dtp_Filter_ToDate, cbo_Filter_PartyName, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, cbo_Filter_PartyName, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

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
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        Dim TotMtrs As Single = 0

        With dgv_Details
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
            If .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With

        Total_Calculation()

        With dgv_Details_Total
            If .RowCount > 0 Then
                TotMtrs = Val(.Rows(0).Cells(1).Value)
            End If
        End With
        txt_Meters.Text = Format(Val(TotMtrs), "#########0.00")

        dgv_Details_CellLeave(sender, e)

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle
        Dim n As Integer = 0
        With dgv_Details

            dgv_ActCtrlName = .Name



            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
                Dim vCloID As Integer = 0
                Dim vSrtNo As String = ""
                Dim vRolNo As String = ""
                Dim vPcNo As String = ""

                vCloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
                vSrtNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Sort_No", "(Cloth_IdNo = " & Str(Val(vCloID)) & ")")

                If .CurrentCell.ColumnIndex <> 0 Then

                    If e.RowIndex = 0 Then

                        vRolNo = ""
                        Da = New SqlClient.SqlDataAdapter("select  max(PieceNo_OrderBy) as pcs_no from Weaver_ClothReceipt_Piece_Details Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' and Cloth_Idno = " & Str(Val(vCloID)) & " ", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            'vPcNo = Dt1.Rows(0).Item("Piece_No").ToString
                            vRolNo = Dt1.Rows(0).Item("pcs_no").ToString 'get_RollNo_from_PieceNo(vPcNo)
                        End If

                        Dt1.Clear()
                        vRolNo = Trim(Val(vRolNo)) + 1

                        .Rows(e.RowIndex).Cells(0).Value = Trim(Common_Procedures.FnYearCode) & ":" & Trim(vSrtNo) & ":" & Trim(vRolNo)

                    Else

                        vPcNo = .Rows(e.RowIndex - 1).Cells(0).Value
                        ' vRolNo = Dt1.Rows(0).Item("pcs_no").ToString
                        vRolNo = get_RollNo_from_PieceNo(vPcNo)

                        vRolNo = Val(vRolNo) + 1

                        .Rows(e.RowIndex).Cells(0).Value = Trim(Common_Procedures.FnYearCode) & ":" & Trim(vSrtNo) & ":" & Trim(vRolNo)

                    End If
                End If


            ElseIf e.RowIndex = 0 Then
                    .CurrentRow.Cells(0).Value = Val(txt_PcsNoFrom.Text)

                Else
                    If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = Val(.Rows(e.RowIndex - 1).Cells(0).Value) + 1
                End If

            End If

            'If e.RowIndex > 0 Then
            '    If e.RowIndex = .Rows.Count - 1 Then
            '        If Val(.CurrentRow.Cells(1).Value) = 0 Then
            '            .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value)
            '            .Rows.Add()
            '        End If
            '    End If
            'End If

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
            If .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
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
                    txt_Meters.Text = Format(Val(TotMtrs), "#########0.00")

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
        Dim PurcCode As String = ""

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_Code, Cloth_Purchase_Code from Cloth_Purchase_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            PcsChkCode = ""
            PurcCode = ""
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                End If
                If IsDBNull(Dt1.Rows(0).Item("Cloth_Purchase_Code").ToString) = False Then
                    PurcCode = Dt1.Rows(0).Item("Cloth_Purchase_Code").ToString
                End If
            End If
            Dt1.Clear()

            If Trim(PcsChkCode) <> "" And Trim(PcsChkCode).ToUpper <> (Trim(Pk_Condition) & Trim(NewCode)).ToUpper Then
                MessageBox.Show("Piece Checking prepared", "DOES NOT DELETE PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
            If Trim(PurcCode) <> "" Then
                MessageBox.Show("Purchase Bill prepared", "DOES NOT DELETE PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            With dgv_Details

                n = .CurrentRow.Index


                If Trim(.Rows(n).Cells(3).Value) = "" Then

                    'If n = .Rows.Count - 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                    'Else
                    '    .Rows.RemoveAt(n)

                    'End If

                    PcsFrmNo = Val(txt_PcsNoFrom.Text)
                    If Val(PcsFrmNo) = 0 Then PcsFrmNo = 1

                    For i = 0 To .Rows.Count - 1
                        If i = 0 Then
                            .Rows(i).Cells(0).Value = Val(PcsFrmNo)
                        Else
                            .Rows(i).Cells(0).Value = Val(.Rows(i - 1).Cells(0).Value) + 1
                        End If
                    Next

                End If

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

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        With dgv_Details

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
                Dim vCloID As Integer = 0
                Dim vSrtNo As String = ""
                Dim vRolNo As String = ""
                Dim vPcNo As String = ""

                vCloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
                vSrtNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Sort_No", "(Cloth_IdNo = " & Str(Val(vCloID)) & ")")

                If e.RowIndex = 0 Then

                    vRolNo = ""
                    Da = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' and Cloth_Idno = " & Str(Val(vCloID)) & "  Order by Weaver_ClothReceipt_Date, PieceNo_OrderBy, Piece_No", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        vPcNo = Dt1.Rows(0).Item("Piece_No").ToString
                        vRolNo = get_RollNo_from_PieceNo(vPcNo)
                    End If
                    Dt1.Clear()

                    vRolNo = Trim(Val(vRolNo)) + 1

                    .Rows(e.RowIndex).Cells(0).Value = Trim(Common_Procedures.FnYearCode) & ":" & Trim(vSrtNo) & ":" & Trim(vRolNo)

                Else

                    vPcNo = .Rows(e.RowIndex - 1).Cells(0).Value
                    vRolNo = get_RollNo_from_PieceNo(vPcNo)

                    vRolNo = Trim(Val(vRolNo)) + 1

                    .Rows(e.RowIndex).Cells(0).Value = Trim(Common_Procedures.FnYearCode) & ":" & Trim(vSrtNo) & ":" & Trim(vRolNo)

                End If



            ElseIf e.RowIndex = 0 Then
                .CurrentRow.Cells(0).Value = Val(txt_PcsNoFrom.Text)

            Else
                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = Val(.Rows(e.RowIndex - 1).Cells(0).Value) + 1
                End If

            End If

        End With

    End Sub

    Private Sub Total_Calculation()
        Dim TotPcs As Integer
        Dim TotMtrs As String
        Dim TotWgt As String

        TotPcs = 0
        TotMtrs = 0
        TotWgt = 0
        With dgv_Details

            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(1).Value) <> 0 Then
                    TotPcs = TotPcs + 1
                    TotMtrs = Format(Val(TotMtrs) + Val(.Rows(i).Cells(1).Value), "########0.00")
                    TotWgt = Format(Val(TotWgt) + Val(.Rows(i).Cells(2).Value), "########0.000")
                End If

            Next

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(0).Value = TotPcs
            .Rows(0).Cells(1).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(2).Value = Format(Val(TotWgt), "########0.000")
        End With

        If Val(TotMtrs) <> 0 Then txt_Meters.Text = Format(Val(TotMtrs), "#########0.00")

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_PcsNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PcsNoFrom.KeyDown
        If e.KeyCode = 40 Then

            If Common_Procedures.settings.CustomerCode = 1516 Then

                txt_Meters.Focus()

            Else

                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.Focus()
                dgv_Details.CurrentCell.Selected = True

            End If

        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")

    End Sub



    Private Sub txt_ReceiptMeters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Meters.KeyDown

        Dim TotMtrs As Single = 0

        If e.KeyCode = 40 Then
            SendKeys.Send("{TAB}")

        ElseIf e.KeyCode = 38 Then
            SendKeys.Send("+{TAB}")

        ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 Then
            TotMtrs = 0
            With dgv_Details_Total
                If .RowCount > 0 Then
                    TotMtrs = Val(.Rows(0).Cells(1).Value)
                End If
            End With
            If Val(TotMtrs) <> 0 Then e.Handled = True : e.SuppressKeyPress = True

        End If
    End Sub

    Private Sub txt_ReceiptMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters.KeyPress
        Dim TotMtrs As Single = 0

        If Common_Procedures.Accept_NumericPositiveOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        With dgv_Details_Total
            TotMtrs = 0
            If .RowCount > 0 Then
                TotMtrs = Val(.Rows(0).Cells(1).Value)
            End If
        End With
        If Val(TotMtrs) <> 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub





    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_PcsNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsNoFrom.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = 1516 Then

                txt_Meters.Focus()

            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_Meters.Focus()
                End If
            End If

        End If

    End Sub

    Private Sub txt_NoOfPcs_GotFocus(sender As Object, e As EventArgs) Handles txt_NoOfPcs.GotFocus
        txt_NoOfPcs.Tag = txt_NoOfPcs.Text
    End Sub

    Private Sub txt_NoofPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfPcs.KeyPress
        If Common_Procedures.Accept_NumericPositiveOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            If Val(txt_NoOfPcs.Tag) <> Val(txt_NoOfPcs.Text) Then

                txt_NoOfPcs.Tag = txt_NoOfPcs.Text

                Add_Rows_in_Piece_Details_Grid()

            End If

            txt_PcsNoFrom.Focus()

        End If


    End Sub

    Private Sub txt_NoOfPcs_LostFocus(sender As Object, e As EventArgs) Handles txt_NoOfPcs.LostFocus
        If Val(txt_NoOfPcs.Tag) <> Val(txt_NoOfPcs.Text) Then

            txt_NoOfPcs.Tag = txt_NoOfPcs.Text

            Add_Rows_in_Piece_Details_Grid()

            txt_PcsNoFrom.Focus()

        End If
    End Sub

    Private Sub Add_Rows_in_Piece_Details_Grid()
        Dim sts As Boolean = False

        With dgv_Details

            If Val(txt_NoOfPcs.Text) = .Rows.Count Then
                Exit Sub

            ElseIf Val(txt_NoOfPcs.Text) > .Rows.Count Then
                For i = 1 To (Val(txt_NoOfPcs.Text) - .Rows.Count)
                    dgv_Details.Rows.Add()
                Next

            ElseIf Val(txt_NoOfPcs.Text) < .Rows.Count Then

                sts = False
                For i = .Rows.Count - 1 To IIf(Val(txt_NoOfPcs.Text) < 0, 0, Val(txt_NoOfPcs.Text)) Step -1
                    If Trim(.Rows(i).Cells(3).Value) <> "" Then
                        sts = True
                        Exit For
                    End If

                Next i

                If sts = False Then

                    For i = .Rows.Count - 1 To IIf(Val(txt_NoOfPcs.Text) < 0, 0, Val(txt_NoOfPcs.Text)) Step -1
                        .Rows.RemoveAt(i)
                        'If i = .Rows.Count - 1 Then
                        '    For J = 0 To .ColumnCount - 1
                        '        .Rows(i).Cells(J).Value = ""
                        '    Next

                        'Else
                        '    .Rows.RemoveAt(i)

                        'End If

                    Next

                Else

                    MessageBox.Show("Invalid No.of Pcs " & vbCr & "Some Pieces Delivered/Baled", "DOES NOT CHANGE NO.OF PCS", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End If

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(0).Value = (i + 1)
            Next

            Total_Calculation()

        End With

    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyCode = 40 Then msk_date.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub


    Private Sub txt_meters_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Meters.LostFocus
        With txt_Meters

            .Text = Format(Val(.Text), "#########0.00")
        End With
    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record
        '
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Cloth_Purchase_Receipt_Entry, New_Entry) = False Then Exit Sub

    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        print_record()
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

    Private Sub txt_NoOfPcs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_NoOfPcs.TextChanged
        PieceNo_To_Calculation()
    End Sub

    Private Sub txt_PcsNoFrom_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_PcsNoFrom.TextChanged
        Dim i As Integer = 0
        Dim PcFrmNo As Integer

        On Error Resume Next

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


    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_ActCtrlName = dgv_Details.Name
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown

        With dgv_Details

            If e.KeyValue = Keys.Delete Then

                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(3).Value) <> "" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
                End If

            End If

        End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Then

                    If Common_Procedures.Accept_NumericPositiveOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(3).Value) <> "" Then
                        e.Handled = True
                    End If

                End If

            End If

        End With

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        dgv_Details_KeyUp(sender, e)
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.Control = True And e.KeyValue = 13 Then
            If txt_Meters.Enabled And txt_Meters.Visible Then
                txt_Meters.Focus()

                ' Else
                'cbo_Transport.Focus()

            End If

        ElseIf e.KeyValue = 46 Then
            With dgv_Details
                If .CurrentCell.ColumnIndex = 1 Then
                    .Rows(.CurrentCell.RowIndex).Cells(1).Value = ""

                End If

            End With

        End If

    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then

            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
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
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_PackType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PackType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PackType, cbo_BillNo, cbo_Cloth, "", "", "", "")
    End Sub

    Private Sub cbo_PackType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PackType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PackType, cbo_Cloth, "", "", "", "")
    End Sub

    Private Sub cbo_BillNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BillNo.GotFocus
        cbo_BillNo.Tag = cbo_BillNo.Text
        If cbo_BillNo.DropDownStyle = ComboBoxStyle.DropDown Then

            Dim Led_ID As Integer
            Dim vCondt As String

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
            If Led_ID = 0 Then
                Exit Sub
            End If

            vCondt = "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")"

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Purchase_Head", "Bill_No", vCondt, "(Cloth_Purchase_Code = '')")

        End If
    End Sub

    Private Sub cbo_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillNo.KeyDown
        If cbo_BillNo.DropDownStyle = ComboBoxStyle.DropDown Then

            Dim Led_ID As Integer
            Dim vCondt As String

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
            If Led_ID = 0 Then
                Exit Sub
            End If

            vCondt = "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")"

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_PartyName, cbo_PackType, "Cloth_Purchase_Head", "Bill_No", vCondt, "(Cloth_Purchase_Code = '')")

        Else

            If e.KeyValue = 38 Or (e.Control = True And e.KeyValue = 38) Then
                e.Handled = True
                cbo_Type.Focus()
                'cbo_PartyName.Focus()

            ElseIf e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
                e.Handled = True
                cbo_PackType.Focus()

            End If

        End If


    End Sub

    Private Sub cbo_BillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BillNo.KeyPress
        If cbo_BillNo.DropDownStyle = ComboBoxStyle.DropDown Then
            Dim Led_ID As Integer
            Dim vCondt As String

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
            If Led_ID = 0 Then
                Exit Sub
            End If

            vCondt = "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")"

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Cloth_Purchase_Head", "Bill_No", vCondt, "(Cloth_Purchase_Code = '' )")

        End If

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True

            'If cbo_Cloth.Enabled = False Then
            '    If cbo_BillNo.Tag.ToString.ToUpper <> cbo_BillNo.Text.ToString.ToUpper Then
            '        cbo_BillNo.Tag = cbo_BillNo.Text
            '        Get_Fabric_Name_from_Purchase_Bill_Entry()
            '    End If
            'End If

            If cbo_PackType.Visible And cbo_PackType.Enabled Then
                cbo_PackType.Focus()
            Else
                cbo_Cloth.Focus()
            End If

        End If

    End Sub

    Private Sub cbo_BillNo_LostFocus(sender As Object, e As EventArgs) Handles cbo_BillNo.LostFocus
        'If cbo_Cloth.Enabled = False Then
        '    If cbo_BillNo.Tag.ToString.ToUpper <> cbo_BillNo.Text.ToString.ToUpper Then
        '        cbo_BillNo.Tag = cbo_BillNo.Text
        '        Get_Fabric_Name_from_Purchase_Bill_Entry()
        '    End If
        'End If
    End Sub

    Private Sub Get_Fabric_Name_from_Purchase_Bill_Entry_111()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Led_ID As Integer

        If Trim(cbo_BillNo.Text) = "" Then
            cbo_Cloth.Text = ""
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Led_ID = 0 Then
            cbo_Cloth.Text = ""
            Exit Sub
        End If

        Da = New SqlClient.SqlDataAdapter("select top 1 tC.cloth_name from Cloth_Purchase_Head a, Cloth_Purchase_Details b, cloth_Head tC where a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Bill_No = '" & Trim(cbo_BillNo.Text) & "' and a.Cloth_Purchase_Code = b.Cloth_Purchase_Code and b.cloth_idno = tC.cloth_idno Order by a.Cloth_Purchase_Date desc, a.for_orderby desc, a.Cloth_Purchase_No Desc, a.Cloth_Purchase_Code Desc, b.sl_no", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then

            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                cbo_Cloth.Text = Dt.Rows(0)(0).ToString

            Else
                cbo_Cloth.Text = ""

            End If

        Else
            cbo_Cloth.Text = ""

        End If

        Dt.Dispose()
        Da.Dispose()

    End Sub

    Private Sub btn_Rows_add_Click(sender As Object, e As EventArgs) Handles btn_Rows_add.Click
        Add_Rows_in_Piece_Details_Grid()
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


    Private Function get_RollNo_from_PieceNo(PcsNo As String) As String

        Dim i, k, n As Integer
        Dim vRolNo As String = ""


        'dgv_Details.Rows(0).Cells(0).Value = Val(txt_PcsNoFrom.Text)
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

    Private Sub cbo_DeliveryAt_Enter(sender As Object, e As EventArgs) Handles cbo_DeliveryAt.Enter

        cbo_DeliveryAt.Tag = cbo_DeliveryAt.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' OR  AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1)   ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_DeliveryAt_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_DeliveryAt.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryAt, txt_Meters, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' OR  AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub


    Private Sub cbo_DeliveryAt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_DeliveryAt.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' OR  AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            SendKeys.Send("{Tab}")
        End If

    End Sub


    Private Sub cbo_Delivery_Purpose_Enter(sender As Object, e As EventArgs) Handles cbo_Delivery_Purpose.Enter

        cbo_Delivery_Purpose.Tag = cbo_Delivery_Purpose.Text

        If Common_Procedures.settings.CustomerCode = "1516" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "Cloth_Delivered=1", "(Process_Idno=0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        End If


    End Sub

    Private Sub cbo_Delivery_Purpose_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Delivery_Purpose.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        If Common_Procedures.settings.CustomerCode = "1516" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delivery_Purpose, cbo_DeliveryAt, Nothing, "Process_Head", "Process_Name", "Cloth_Delivered=1", "(Process_Idno=0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delivery_Purpose, cbo_DeliveryAt, Nothing, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        End If

    End Sub


    Private Sub cbo_Delivery_Purpose_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Delivery_Purpose.KeyPress

        If Common_Procedures.settings.CustomerCode = "1516" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delivery_Purpose, Nothing, "Process_Head", "Process_Name", "Cloth_Delivered=1", "(Process_Idno=0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delivery_Purpose, Nothing, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        End If

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            SendKeys.Send("{Tab}")
        End If

    End Sub

    Private Sub cbo_DeliveryAt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_DeliveryAt.SelectedIndexChanged

        'If cbo_DeliveryAt.Tag <> cbo_DeliveryAt.Text Then
        '    Enable_Disable_Delivery_Purpose()
        '    cbo_DeliveryAt.Tag = cbo_DeliveryAt.Text
        'End If

    End Sub


    Private Sub Enable_Disable_Delivery_Purpose()

        If Len(Trim(cbo_DeliveryAt.Text)) = 0 Then

            cbo_Delivery_Purpose.Text = ""
            cbo_Delivery_Purpose.Enabled = False

        Else

            Dim Led_Type As String = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryAt.Text))

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

    Private Sub cbo_DeliveryAt_Leave(sender As Object, e As EventArgs) Handles cbo_DeliveryAt.Leave
        If cbo_DeliveryAt.Tag <> cbo_DeliveryAt.Text Then
            Enable_Disable_Delivery_Purpose()
            cbo_DeliveryAt.Tag = cbo_DeliveryAt.Text
        End If
    End Sub

    Private Function GetNewNo(Optional trans As SqlClient.SqlTransaction = Nothing) As String

        Dim New_No1 As Integer = Common_Procedures.get_MaxCode(con, "Cloth_Purchase_Receipt_Head", "Cloth_Purchase_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, trans)
        Dim New_No2 As Integer = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, trans)

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

    Private Sub cbo_Delivery_Purpose_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Delivery_Purpose.SelectedIndexChanged

    End Sub

    Private Sub cbo_PartyName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_PartyName.SelectedIndexChanged

    End Sub

    Private Sub cbo_DeliveryAt_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_DeliveryAt.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryAt.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

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

    Private Sub cbo_Cloth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Cloth.SelectedIndexChanged

    End Sub

    Private Sub cbo_Processed_Cloth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Processed_Cloth.SelectedIndexChanged

    End Sub

    Private Sub cbo_Processed_Cloth_Enter(sender As Object, e As EventArgs) Handles cbo_Processed_Cloth.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "Cloth_Type = 'PROCESSED FABRIC'", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Processed_Cloth_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Processed_Cloth.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Processed_Cloth, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "Cloth_Type = 'PROCESSED FABRIC'", "(Cloth_IdNo = 0)")

        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")

    End Sub

    Private Sub cbo_Processed_Cloth_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Processed_Cloth.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Processed_Cloth, Nothing, "Cloth_Head", "Cloth_Name", "Cloth_Type = 'PROCESSED FABRIC'", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            SendKeys.Send("{Tab}")
        End If

    End Sub

    Private Sub cbo_Processed_Cloth_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Processed_Cloth.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Processed_Cloth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub btn_Selection_Click(sender As Object, e As EventArgs) Handles btn_Selection.Click

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bls As Single = 0
        Dim Ent_BlNos As String = ""
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim Ent_Rate As Single = 0
        Dim vSELC_PKCODE As String = ""


        If Trim(UCase(cbo_Type.Text)) <> "ORDER" Then Exit Sub

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"


        If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
            With dgv_Order_Selection

                .Rows.Clear()

                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Cloth_Name, d.Ledger_Name as agentname, e.Ledger_Name as Transportname,e.vehicle_no,  g.ClothType_name,  h.noof_pcs as Ent_Pcs, h.ReceiptMeters_Receipt as Ent_Meters  from ClothPurchase_Order_Head a INNER JOIN ClothPurchase_Order_details b ON a.ClothPurchase_Order_Code = b.ClothPurchase_Order_Code INNER JOIN Cloth_Head c ON b.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head g ON b.ClothType_IdNo = g.ClothType_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Cloth_Purchase_Receipt_Head h ON h.Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "' and b.ClothPurchase_Order_Code = h.ClothPurchase_Order_Code and b.ClothPurchase_Order_SlNo = h.ClothPurchase_Order_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Order_Meters - b.Order_Cancel_Meters - b.Purchase_Meters ) > 0 or h.ReceiptMeters_Receipt > 0 ) order by a.ClothPurchase_Order_Date, a.for_orderby, a.ClothPurchase_Order_No", con)
                'Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Cloth_Name, d.Ledger_Name as agentname, e.Ledger_Name as Transportname,e.vehicle_no,  g.ClothType_name,  h.Pcs as Ent_Pcs, h.Meters as Ent_Meters , h.Rate as Ent_Rate from ClothPurchase_Order_Head a INNER JOIN ClothPurchase_Order_details b ON a.ClothPurchase_Order_Code = b.ClothPurchase_Order_Code INNER JOIN Cloth_Head c ON b.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head g ON b.ClothType_IdNo = g.ClothType_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Cloth_Purchase_Details h ON h.Cloth_Purchase_Code = '" & Trim(NewCode) & "' and b.ClothPurchase_Order_Code = h.ClothPurchase_Order_Code and b.ClothPurchase_Order_SlNo = h.ClothPurchase_Order_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Order_Meters - b.Order_Cancel_Meters - b.Purchase_Meters ) > 0 or h.Meters > 0 ) order by a.ClothPurchase_Order_Date, a.for_orderby, a.ClothPurchase_Order_No", con)
                Dt1 = New DataTable
                    Da.Fill(Dt1)


                    If Dt1.Rows.Count > 0 Then

                        For i = 0 To Dt1.Rows.Count - 1

                            n = .Rows.Add()

                            Ent_Bls = 0
                            Ent_BlNos = ""
                            Ent_Pcs = 0
                            Ent_Mtrs = 0
                            Ent_Rate = 0


                            If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
                                Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
                            End If
                            If IsDBNull(Dt1.Rows(i).Item("Ent_Meters").ToString) = False Then
                                Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_Meters").ToString)
                            End If

                            'If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then
                            '    Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
                            'End If

                            SNo = SNo + 1
                            .Rows(n).Cells(0).Value = Val(SNo)

                            .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothPurchase_Order_No").ToString

                            .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothPurchase_Order_Date").ToString), "dd-MM-yyyy")
                            .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                            .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("ClothType_Name").ToString
                            .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Fold_Perc").ToString)
                            .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Order_Pcs").ToString)
                            .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString) - Val(Dt1.Rows(i).Item("Order_Cancel_Meters").ToString) - Val(Dt1.Rows(i).Item("Purchase_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")
                            .Rows(n).Cells(8).Value = (Dt1.Rows(i).Item("Rate").ToString)
                            If Ent_Mtrs > 0 Then
                                .Rows(n).Cells(9).Value = "1"
                                For j = 0 To .ColumnCount - 1
                                    .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                Next

                            Else
                                .Rows(n).Cells(9).Value = ""

                            End If

                            .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("agentname").ToString
                            .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Transportname").ToString

                            .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Agent_Comm_Perc").ToString
                            .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Agent_Comm_Type").ToString
                            .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("ClothPurchase_Order_Code").ToString
                            .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("ClothPurchase_Order_SlNo").ToString

                            .Rows(n).Cells(16).Value = Ent_Pcs
                            .Rows(n).Cells(17).Value = Ent_Mtrs

                            ' .Rows(n).Cells(18).Value = Ent_Rate
                            'txt_Cash_Disc.Text = Dt1.Rows(i).Item("Discount_Percentage").ToString
                            'txt_Vechile.Text = Dt1.Rows(i).Item("vehicle_no").ToString
                            'txt_Freight.Text = Dt1.Rows(i).Item("Freight_Amount").ToString
                        Next
                    End If

            End With

        End If
        Dt1.Clear()

        pnl_Order_Selection.Visible = True
        pnl_Back.Enabled = False


    End Sub
    Private Sub btn_Close_order_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_order_Selection.Click
        Cloth_Invoice_Selection()
        pnl_Back.Enabled = True
        pnl_Order_Selection.Visible = False
        If cbo_BillNo.Enabled And cbo_BillNo.Visible Then cbo_BillNo.Focus()
    End Sub

    Private Sub Cloth_Invoice_Selection()

        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim VCurRow As Integer = 0
        Dim vprn_BlNos As String = ""
        vprn_BlNos = ""

        If dgv_Order_Selection.RowCount = 0 Then Exit Sub

        With dgv_Order_Selection

            VCurRow = .CurrentRow.Index

            ' For i = 0 To dgv_Order_Selection.RowCount - 1

            If Val(dgv_Order_Selection.Rows(.CurrentRow.Index).Cells(9).Value) = 1 Then


                lbl_Cloth_Purc_Order_No.Text = dgv_Order_Selection.Rows(VCurRow).Cells(1).Value
                txt_Pur_Order_Date.Text = dgv_Order_Selection.Rows(VCurRow).Cells(2).Value
                cbo_Cloth.Text = dgv_Order_Selection.Rows(VCurRow).Cells(3).Value
                txt_Folding.Text = dgv_Order_Selection.Rows(VCurRow).Cells(5).Value


                lbl_Cloth_Purc_Order_Code.Text = dgv_Order_Selection.Rows(VCurRow).Cells(14).Value
                lbl_Cloth_Purc_Order_Slno.Text = dgv_Order_Selection.Rows(VCurRow).Cells(15).Value

                If Val(dgv_Order_Selection.Rows(VCurRow).Cells(16).Value) <> 0 Then
                    txt_NoOfPcs.Text = dgv_Order_Selection.Rows(VCurRow).Cells(16).Value
                Else
                    txt_NoOfPcs.Text = dgv_Order_Selection.Rows(VCurRow).Cells(6).Value
                End If


                If Val(dgv_Order_Selection.Rows(VCurRow).Cells(17).Value) <> 0 Then
                    txt_Meters.Text = dgv_Order_Selection.Rows(VCurRow).Cells(17).Value
                Else
                    txt_Meters.Text = dgv_Order_Selection.Rows(VCurRow).Cells(7).Value
                End If



            End If

          '  Next
        End With
        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Order_Selection.Visible = False
        If cbo_BillNo.Enabled And cbo_BillNo.Visible Then cbo_BillNo.Focus()

    End Sub
    Private Sub dgv_Order_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Order_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Order_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(9).Value = (Val(.Rows(RwIndx).Cells(9).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(9).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(9).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Order_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Order_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Order_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Order_Selection.CurrentCell.RowIndex

                Select_Piece(n)

                e.Handled = True

            End If
        End If
    End Sub
    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_PartyName, cbo_BillNo, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
                If MessageBox.Show("Do you want to select Cloth Receipt :", "FOR CLOTH RECEIPT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)
                Else
                    cbo_BillNo.Focus()
                End If
            Else
                cbo_BillNo.Focus()

            End If

        End If

    End Sub

End Class