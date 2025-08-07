Imports System.Drawing.Printing
Imports System.IO
Public Class ClothSales_Order_Indent_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CSORD-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Single
    Private vCbo_ItmNm As String
    Private dgv_ActCtrlName As String = ""
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_Delivery_Schedule_Input As New DataGridViewTextBoxEditingControl

    Private Print_PDF_Status As Boolean = False
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Public Shared EntFnYrCode As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private Amount As Integer = 0
    Private prn_Delv_DetDt As New DataTable

    Dim vDELVSCH_item1, vDELVSCH_item2, vDELVSCH_item3, vDELVSCH_item4, vDELVSCH_item5, vDELVSCH_item6, vDELVSCH_item7 As String

    Private Enum dgvCol_DelvDetails As Integer
        SNo '0.
        clothname '1
        type '2
        loom_type
        fold '3
        bales '4
        order_pcs '5
        order_mtrs '6
        rate '7
        Amount '9
        cancel_mtrs '10
        clothsales_order_slno '11
        clothsales_invoice_meters '12
        clothsales_delivery_meters '13
        enq_no '14
        clothsales_enquiry_code '15
        clothsales_enquiry_slno '16
        Delivery_Schedule_Selection    '17
        Delivery_Schedule_Details_SlNo '18
        Meters_per_Bill     '19
    End Enum

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_PartyPO_Image.Visible = False
        chk_Verified_Status.Checked = False
        chk_CloseStatus.Checked = False
        Chk_ReadyStockAvailable.Checked = False
        pnl_LoomSelection_ToolTip.Visible = False
        Pnl_Delv_Selection.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        vmskOldText = ""
        vmskSelStrt = -1
        cbo_DeliveryTo.Text = ""
        lbl_DiscAmount.Text = ""

        txt_GrTime.Text = ""
        msk_GrDate.Text = ""
        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_Agent.Text = ""
        cbo_Through.Text = "DIRECT"
        cbo_Transport.Text = ""
        cbo_DespTo.Text = ""
        cbo_Grid_ClothName.Text = ""
        cbo_Grid_Clothtype.Text = ""
        cbo_Grid_loomType.Text = ""
        cbo_Com_Type.Text = "%"
        cbo_Type.Text = "DIRECT"
        txt_com_per.Text = ""
        txt_OrderNo.Text = ""
        txt_DelvAdd1.Text = ""
        txt_DelvAdd2.Text = ""
        txt_Note.Text = ""
        txt_PaymentTerms.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        txt_vehicle_no.Text = ""
        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White
        txt_Attend.Text = ""
        cbo_taxType.Text = ""
        txt_delivery_due_days.Text = ""
        msk_delivery_date.Text = ""
        Msk_Final_Delv_Date.Text = ""

        txt_Disc.Text = ""
        cbo_fright_ac.Text = ""
        txt_gst_percentage.Text = ""
        txt_delivery_Schedule.Text = ""
        txt_bale_meters.Text = ""
        txt_piece_Meters.Text = ""


        lbl_UserName_CreatedBy.Text = ""
        lbl_UserName_ModifiedBy.Text = ""

        cbo_DeliveryTo.Enabled = True
        cbo_DeliveryTo.BackColor = Color.White

        cbo_Agent.Enabled = True
        cbo_Agent.BackColor = Color.White

        cbo_DespTo.Enabled = True
        cbo_DespTo.BackColor = Color.White

        cbo_Through.Enabled = True
        cbo_Through.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        txt_OrderNo.Enabled = True
        txt_OrderNo.BackColor = Color.White

        txt_DelvAdd1.Enabled = True
        txt_DelvAdd1.BackColor = Color.White

        txt_DelvAdd2.Enabled = True
        txt_DelvAdd2.BackColor = Color.White

        txt_com_per.Enabled = True
        txt_com_per.BackColor = Color.White

        cbo_Com_Type.Enabled = True
        cbo_Com_Type.BackColor = Color.White

        pic_PartyPO_Image.BackgroundImage = Nothing

        cbo_Grid_ClothName.Enabled = True
        cbo_Grid_ClothName.BackColor = Color.White

        cbo_Grid_Clothtype.Enabled = True
        cbo_Grid_Clothtype.BackColor = Color.White


        cbo_Grid_loomType.Enabled = True
        cbo_Grid_loomType.BackColor = Color.White

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        Dgv_Delivery_Schedule_Details_Hidden.Rows.Clear()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_ClothName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Grid_ClothName.Visible = False
        cbo_Grid_Clothtype.Visible = False
        cbo_Grid_loomType.Visible = False
        dgv_ActCtrlName = ""
        NoCalc_Status = False
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
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
            Msktxbx = Me.ActiveControl
            Msktxbx.SelectionStart = 0
        End If

        If Me.ActiveControl.Name <> cbo_Grid_ClothName.Name Then
            cbo_Grid_ClothName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Clothtype.Name Then
            cbo_Grid_Clothtype.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_loomType.Name Then
            cbo_Grid_loomType.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_Details_Total.Name And Not (TypeOf ActiveControl Is DataGridViewTextBoxEditingControl) Then
            pnl_LoomSelection_ToolTip.Visible = False

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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
        If Not IsNothing(Dgv_Delivery_Schedule_Details_Input.CurrentCell) Then Dgv_Delivery_Schedule_Details_Input.CurrentCell.Selected = False
    End Sub

    Private Sub ClothSales_Order_Indent_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Clothtype.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Clothtype.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_loomType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOM TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_loomType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                Me.Text = lbl_Heading.Text & "  -  " & lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub ClothSales_Order_Indent_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub ClothSales_Order_Indent_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub
                ElseIf Pnl_Delv_Selection.Visible = True Then
                    btn_Close_Delv_Selection_Click(sender, e)
                    Exit Sub
                ElseIf pnl_PartyPO_Image.Visible = True Then
                    btn_Close_PartyPO_Image_Click(sender, e)
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

    Private Sub ClothSales_Order_Indent_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim OpYrCode As String = ""
        Me.Text = ""

        con.Open()

        If Trim(UCase(Common_Procedures.ClothOrder_Opening_OR_Entry)) = "OPENING" Then
            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            EntFnYrCode = OpYrCode

        Else
            EntFnYrCode = Common_Procedures.FnYearCode

        End If

        Pnl_Delv_Selection.Visible = False
        Pnl_Delv_Selection.Left = (Me.Width - Pnl_Delv_Selection.Width) \ 2
        Pnl_Delv_Selection.Top = (Me.Height - Pnl_Delv_Selection.Height) \ 2


        lbl_Piece_Meters.Visible = False
        txt_piece_Meters.Visible = False
        lbl_bale_Meters.Visible = False
        txt_bale_meters.Visible = False
        Msk_Final_Delv_Date.Visible = False
        Dtp_Final_Delv_Date.Visible = False
        lbl_Final_Delv_Date.Visible = False
        Chk_ReadyStockAvailable.Visible = False

        lbl_caption_vehicleno.Visible = False
        txt_vehicle_no.Visible = False
        txt_caption_attend.Visible = False
        txt_Attend.Visible = False
        If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then
            Label14.Visible = True

            cbo_Type.Visible = True
            cbo_Type.Enabled = False
        End If

        If Common_Procedures.settings.CustomerCode = "1186" Or Common_Procedures.settings.CustomerCode = "1380" Or Common_Procedures.settings.CustomerCode = "1446" Then

            lbl_Heading.Text = "FABRIC SALES ORDER"


            txt_caption_attend.Visible = True
            txt_Attend.Visible = True
            txt_Attend.Width = cbo_DespTo.Width
            txt_Attend.BackColor = Color.White

            lbl_DespTo_Caption.Visible = False
            cbo_DespTo.Visible = False

            lbl_caption_vehicleno.Visible = False
            txt_vehicle_no.Visible = False
            dgv_Details.Columns(dgvCol_DelvDetails.loom_type).Visible = True
            cbo_taxType.Visible = True
            Label36.Visible = True 'taxtype

            Label14.Visible = False
            cbo_Type.Visible = False

            Label35.Visible = True
            Label33.Visible = True
            msk_delivery_date.Visible = True
            dtp_delivery_date.Visible = True
            txt_delivery_due_days.Visible = True
            Label41.Visible = False
            cbo_Transport.Visible = False
            Label14.Visible = False
            cbo_Type.Visible = False
            txt_PaymentTerms.Visible = False
            Label24.Visible = False



            dgv_Details.Columns(dgvCol_DelvDetails.type).Visible = False
            dgv_Details.Columns(dgvCol_DelvDetails.fold).Visible = False
            dgv_Details.Columns(dgvCol_DelvDetails.bales).Visible = False
            dgv_Details.Columns(dgvCol_DelvDetails.enq_no).Visible = False

            dgv_Details.Columns(dgvCol_DelvDetails.clothname).Width = dgv_Details.Columns(dgvCol_DelvDetails.clothname).Width + dgv_Details.Columns(dgvCol_DelvDetails.bales).Width + dgv_Details.Columns(dgvCol_DelvDetails.enq_no).Width

        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1155" Then
            dtp_delivery_date.Visible = True
            txt_delivery_due_days.Visible = True
            Label33.Visible = True
            Label35.Visible = True
            msk_delivery_date.Visible = True
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then


            dgv_Details.Columns(dgvCol_DelvDetails.order_pcs).Visible = False
            dgv_Details_Total.Columns(dgvCol_DelvDetails.order_pcs).Visible = False
            dgv_Details.Columns(dgvCol_DelvDetails.Delivery_Schedule_Selection).Visible = True
            dgv_Details.Columns(dgvCol_DelvDetails.Meters_per_Bill).Visible = True

            Msk_Final_Delv_Date.Visible = True
            Dtp_Final_Delv_Date.Visible = True
            lbl_Final_Delv_Date.Visible = True
            lbl_Piece_Meters.Visible = True
            txt_piece_Meters.Visible = True
            lbl_bale_Meters.Visible = True
            txt_bale_meters.Visible = True
            Chk_ReadyStockAvailable.Visible = True


        ElseIf Common_Procedures.settings.CustomerCode = "1186" Or Common_Procedures.settings.CustomerCode = "1380" Or Common_Procedures.settings.CustomerCode = "1446" Then

            dgv_Details.Columns(dgvCol_DelvDetails.Delivery_Schedule_Selection).Visible = False


            dgv_Details.Columns(dgvCol_DelvDetails.clothname).Width = 200
            dgv_Details.Columns(dgvCol_DelvDetails.loom_type).Width = 115
            dgv_Details.Columns(dgvCol_DelvDetails.order_mtrs).Width = 110
            dgv_Details.Columns(dgvCol_DelvDetails.order_pcs).Width = 110
            dgv_Details.Columns(dgvCol_DelvDetails.rate).Width = 110
            dgv_Details.Columns(dgvCol_DelvDetails.Amount).Width = 115
            dgv_Details.Columns(dgvCol_DelvDetails.cancel_mtrs).Width = 115

            'dgv_Details.Columns(dgvCol_DelvDetails.type).Visible = False
            'dgv_Details.Columns(dgvCol_DelvDetails.fold).Visible = False
            'dgv_Details.Columns(dgvCol_DelvDetails.bales).Visible = False
            'dgv_Details.Columns(dgvCol_DelvDetails.enq_no).Visible = False



        Else

            dgv_Details.Columns(dgvCol_DelvDetails.Delivery_Schedule_Selection).Visible = False
            dgv_Details.Columns(dgvCol_DelvDetails.clothname).Width = 180
            dgv_Details.Columns(dgvCol_DelvDetails.type).Width = 100
            dgv_Details.Columns(dgvCol_DelvDetails.loom_type).Width = 110
            dgv_Details.Columns(dgvCol_DelvDetails.fold).Width = 55
            dgv_Details.Columns(dgvCol_DelvDetails.bales).Width = 70
            dgv_Details.Columns(dgvCol_DelvDetails.order_mtrs).Width = 75
            dgv_Details.Columns(dgvCol_DelvDetails.order_pcs).Width = 90
            dgv_Details.Columns(dgvCol_DelvDetails.rate).Width = 85
            dgv_Details.Columns(dgvCol_DelvDetails.Amount).Width = 100
            dgv_Details.Columns(dgvCol_DelvDetails.cancel_mtrs).Width = 100


        End If

        Common_Procedures.get_CashPartyName_From_All_Entries(con)

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where ( Ledger_IdNo = 0 or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_PartyName.DataSource = dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Transport.DataSource = dt2
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_Agent.DataSource = dt3
        cbo_Agent.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt4)
        cbo_Grid_ClothName.DataSource = dt4
        cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
        da.Fill(dt5)
        cbo_Grid_Clothtype.DataSource = dt5
        cbo_Grid_Clothtype.DisplayMember = "ClothType_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Despatch_To) from ClothSales_Order_Head order by Despatch_To", con)
        da.Fill(dt6)
        cbo_DespTo.DataSource = dt6
        cbo_DespTo.DisplayMember = "Despatch_To"

        chk_Verified_Status.Visible = False
        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then chk_Verified_Status.Visible = True
        End If

        chk_CloseStatus.Visible = False
        If Trim(Common_Procedures.UR.ClothSales_OrderIndent_Entry_Close_Option) <> "" Then
            chk_CloseStatus.Visible = True
        End If

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        cbo_Com_Type.Items.Clear()
        cbo_Com_Type.Items.Add("%")
        cbo_Com_Type.Items.Add("MTR")

        cbo_Through.Items.Clear()
        cbo_Through.Items.Add(" ")
        cbo_Through.Items.Add("DIRECT")
        cbo_Through.Items.Add("BANK")
        cbo_Through.Items.Add("AGENT")

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("ENQUIRY")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_PartyPO_Image.Visible = False
        pnl_PartyPO_Image.Top = (Me.Height - pnl_PartyPO_Image.Height) \ 2
        pnl_PartyPO_Image.Left = (Me.Width - pnl_PartyPO_Image.Width) \ 2
        pnl_PartyPO_Image.BringToFront()


        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Com_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DespTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Clothtype.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Through.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_com_per.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PaymentTerms.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_GrDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GrTime.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_loomType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_piece_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_bale_meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_vehicle_no.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Attend.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_taxType.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_delivery_due_days.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_delivery_date.GotFocus, AddressOf ControlGotFocus
        AddHandler Msk_Final_Delv_Date.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_taxType.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Disc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_delivery_Schedule.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_fright_ac.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_gst_percentage.GotFocus, AddressOf ControlGotFocus




        AddHandler txt_Disc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_delivery_Schedule.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_fright_ac.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_gst_percentage.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_piece_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_bale_meters.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_delivery_due_days.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_delivery_date.LostFocus, AddressOf ControlLostFocus
        AddHandler Msk_Final_Delv_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Attend.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_vehicle_no.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus

        AddHandler chk_CloseStatus.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_CloseStatus.LostFocus, AddressOf ControlLostFocus

        AddHandler Chk_ReadyStockAvailable.GotFocus, AddressOf ControlGotFocus
        AddHandler Chk_ReadyStockAvailable.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Com_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DespTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Clothtype.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Through.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_com_per.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PaymentTerms.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_GrDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GrTime.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_loomType.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_com_per.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DelvAdd1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_GrDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GrTime.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Attend.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_delivery_due_days.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_delivery_date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Attend.KeyPress, AddressOf TextBoxControlKeyPress


        'AddHandler txt_Disc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_delivery_Schedule.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_gst_percentage.KeyPress, AddressOf TextBoxControlKeyPress


        'AddHandler txt_Disc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_delivery_Schedule.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_gst_percentage.KeyDown, AddressOf TextBoxControlKeyDown





        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_com_per.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DelvAdd1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_GrDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GrTime.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_delivery_due_days.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler msk_delivery_date.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
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

        If ActiveControl.Name = dgv_Details.Name Or ActiveControl.Name = Dgv_Delivery_Schedule_Details_Input.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            'On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details
            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details
            ElseIf dgv_ActCtrlName = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf ActiveControl.Name = Dgv_Delivery_Schedule_Details_Input.Name Then
                dgv1 = Dgv_Delivery_Schedule_Details_Input
            ElseIf Dgv_Delivery_Schedule_Details_Input.IsCurrentRowDirty = True Then
                dgv1 = Dgv_Delivery_Schedule_Details_Input
            ElseIf dgv_ActCtrlName = Dgv_Delivery_Schedule_Details_Input.Name Then
                dgv1 = Dgv_Delivery_Schedule_Details_Input

            ElseIf Pnl_Delv_Selection.Visible = True Then
                dgv1 = Dgv_Delivery_Schedule_Details_Input

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_Details.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            'GOTO_LOOP1:
                            If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
GOTO_LOOP1:

                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    If txt_PaymentTerms.Visible And txt_PaymentTerms.Enabled Then
                                        txt_PaymentTerms.Focus()
                                    Else
                                        txt_Disc.Focus()
                                    End If

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_DelvDetails.clothname)

                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.clothname Then
                                If .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.type).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.type)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.loom_type)

                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.bales Then
                                If .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.order_pcs).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.order_pcs)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.order_mtrs)
                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.rate Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.cancel_mtrs)

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.loom_type Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.order_pcs)

                                'ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.cancel_mtrs Then
                                '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.Delivery_Schedule_Selection)

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.order_mtrs Then
                                If Trim(UCase(cbo_Type.Text)) = Trim(UCase("ENQUIRY")) Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.cancel_mtrs)
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.rate)
                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.cancel_mtrs Then

                                If .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.loom_type).Visible = True Then

                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.loom_type)

                                ElseIf .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Delivery_Schedule_Selection).Visible = True Then

                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Delivery_Schedule_Selection)

                                ElseIf .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Meters_per_Bill).Visible = True Then

                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Meters_per_Bill)

                                Else

                                    GoTo GOTO_LOOP1

                                    '.CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Delivery_Schedule_Selection Then
                                If .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Meters_per_Bill).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Meters_per_Bill)
                                Else
                                    GoTo GOTO_LOOP1
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    cbo_DeliveryTo.Focus()

                                Else
                                    If .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Meters_per_Bill).Visible = True Then
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_DelvDetails.Meters_per_Bill)
                                    ElseIf .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Delivery_Schedule_Selection).Visible = True Then
                                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.Delivery_Schedule_Selection)
                                    ElseIf .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.cancel_mtrs).Visible = True And .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.cancel_mtrs).ReadOnly = False Then
                                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.cancel_mtrs)
                                    Else
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_DelvDetails.rate)
                                    End If
                                    '.CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 7)

                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Meters_per_Bill Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.Delivery_Schedule_Selection)

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Delivery_Schedule_Selection Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.cancel_mtrs)

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.cancel_mtrs Then
                                If Trim(UCase(cbo_Type.Text)) = "ENQUIRY" Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.order_mtrs)
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.rate)
                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.order_mtrs Then
                                If .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.order_pcs).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.order_pcs)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.bales)
                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.fold Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.type)
                            ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.order_pcs Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.bales)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                            End If

                            Return True

                        Else

                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If



                    ElseIf dgv1.Name = Dgv_Delivery_Schedule_Details_Input.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If .CurrentCell.ColumnIndex >= 2 Then
                                '.Rows.Add()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            Else

                                'If IsDate(Dgv_Delv_Selection.Rows(.CurrentCell.RowIndex).Cells(1).Value) = False Then

                                '    MessageBox.Show("Invaild Delivery Schedule Date", "DOES NOT ADD", MessageBoxButtons.OK, MessageBoxIcon.Error)

                                '    Exit Function

                                'Else

                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                                'End If

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then

                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)

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

                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from ClothSales_Order_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Order_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("ClothSales_Order_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("ClothSales_Order_Date").ToString
                msk_date.Text = dtp_Date.Text
                lbl_DiscAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString
                cbo_Com_Type.Text = dt1.Rows(0).Item("Agent_Comm_Type").ToString
                cbo_DespTo.Text = dt1.Rows(0).Item("Despatch_To").ToString
                cbo_Through.Text = dt1.Rows(0).Item("Through_Name").ToString
                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_idno").ToString))
                msk_GrDate.Text = dt1.Rows(0).Item("Gr_Date").ToString
                txt_GrTime.Text = dt1.Rows(0).Item("Gr_Time").ToString

                If Val(dt1.Rows(0).Item("created_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("created_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("created_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_CreatedBy.Text = "Created by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("created_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("created_DateTime_Text").ToString)
                    Else
                        lbl_UserName_CreatedBy.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("created_useridno").ToString))))
                    End If
                End If
                If Val(dt1.Rows(0).Item("Last_modified_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("Last_modified_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("Last_modified_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_ModifiedBy.Text = "Last Modified by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("Last_modified_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("Last_modified_DateTime_Text").ToString)
                    End If
                End If

                txt_com_per.Text = dt1.Rows(0).Item("Agent_Comm_Perc").ToString
                txt_OrderNo.Text = dt1.Rows(0).Item("Party_OrderNo").ToString
                txt_DelvAdd1.Text = dt1.Rows(0).Item("Delivery_Address1").ToString
                txt_DelvAdd2.Text = dt1.Rows(0).Item("Delivery_Address2").ToString
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_PaymentTerms.Text = dt1.Rows(0).Item("Payment_Terms").ToString
                txt_vehicle_no.Text = dt1.Rows(0).Item("vehicle_no").ToString
                txt_Attend.Text = dt1.Rows(0).Item("Attend").ToString
                cbo_taxType.Text = dt1.Rows(0).Item("Tax_Type").ToString

                msk_delivery_date.Text = dt1.Rows(0).Item("Delivery_due_date").ToString
                txt_delivery_due_days.Text = dt1.Rows(0).Item("Delivery_Due_days").ToString

                Msk_Final_Delv_Date.Text = dt1.Rows(0).Item("Final_Delivery_date").ToString

                txt_gst_percentage.Text = dt1.Rows(0).Item("GST_Perc").ToString
                txt_delivery_Schedule.Text = dt1.Rows(0).Item("Delivery_Schedule").ToString
                txt_Disc.Text = dt1.Rows(0).Item("Discount").ToString
                cbo_fright_ac.Text = dt1.Rows(0).Item("freight_Ac").ToString

                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True
                If Val(dt1.Rows(0).Item("Order_Close_Status").ToString) = 1 Then chk_CloseStatus.Checked = True
                If Val(dt1.Rows(0).Item("Ready_Stock_Available_Status").ToString) = 1 Then Chk_ReadyStockAvailable.Checked = True

                txt_piece_Meters.Text = dt1.Rows(0).Item("Piece_Meters").ToString
                txt_bale_meters.Text = dt1.Rows(0).Item("Bale_Meters").ToString

                If IsDBNull(dt1.Rows(0).Item("PartyPO_Document_Image")) = False Then
                    Dim imageData4 As Byte() = DirectCast(dt1.Rows(0).Item("PartyPO_Document_Image"), Byte())
                    If Not imageData4 Is Nothing Then
                        Using ms9 As New MemoryStream(imageData4, 0, imageData4.Length)
                            ms9.Write(imageData4, 0, imageData4.Length)
                            If imageData4.Length > 0 Then
                                pic_PartyPO_Image.BackgroundImage = Image.FromStream(ms9)
                            End If
                        End Using
                    End If
                End If

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Cloth_Name, c.ClothType_Name , L.LoomType_Name  from ClothSales_Order_Details a LEFT OUTER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_IdNo  LEFT OUTER JOIN LoomType_Head L ON a.loomType_IdNo = L.LoomType_IdNo Where a.ClothSales_Order_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(dgvCol_DelvDetails.SNo).Value = Val(SNo)
                            .Rows(n).Cells(dgvCol_DelvDetails.clothname).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.type).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                            If Val(dt2.Rows(i).Item("Fold_Perc").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_DelvDetails.fold).Value = Format(Val(dt2.Rows(i).Item("Fold_Perc").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Bales").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_DelvDetails.bales).Value = Val(dt2.Rows(i).Item("Bales").ToString)
                            End If
                            If Val(dt2.Rows(i).Item("Order_Pcs").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_DelvDetails.order_pcs).Value = Val(dt2.Rows(i).Item("Order_Pcs").ToString)
                            End If
                            .Rows(n).Cells(dgvCol_DelvDetails.order_mtrs).Value = Format(Val(dt2.Rows(i).Item("Order_Meters").ToString), "########0.00")
                            If Val(dt2.Rows(i).Item("Rate").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_DelvDetails.rate).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            End If
                            .Rows(n).Cells(dgvCol_DelvDetails.Amount).Value = Format(Val(dt2.Rows(i).Item("AMOunt").ToString), "########0.00")
                            If Val(dt2.Rows(i).Item("Order_Cancel_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_DelvDetails.cancel_mtrs).Value = Format(Val(dt2.Rows(i).Item("Order_Cancel_Meters").ToString), "########0.00")
                            End If
                            .Rows(n).Cells(dgvCol_DelvDetails.clothsales_order_slno).Value = dt2.Rows(i).Item("ClothSales_Order_SlNo").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.clothsales_invoice_meters).Value = dt2.Rows(i).Item("Invoice_Meters").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.clothsales_delivery_meters).Value = dt2.Rows(i).Item("Delivery_Meters").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.enq_no).Value = dt2.Rows(i).Item("ClothSales_Enquiry_No").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.clothsales_enquiry_code).Value = dt2.Rows(i).Item("ClothSales_Enquiry_Code").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.clothsales_enquiry_slno).Value = dt2.Rows(i).Item("ClothSales_Enquiry_Slno").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.loom_type).Value = dt2.Rows(i).Item("LoomType_name").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.Delivery_Schedule_Details_SlNo).Value = dt2.Rows(i).Item("Delivery_Schedule_Details_SlNo").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.Meters_per_Bill).Value = dt2.Rows(i).Item("Meters_per_Bill").ToString

                            If Trim(.Rows(n).Cells(dgvCol_DelvDetails.clothsales_invoice_meters).Value) <> 0 Or Val(.Rows(n).Cells(dgvCol_DelvDetails.clothsales_delivery_meters).Value) <> 0 Then
                                For j = 0 To .ColumnCount - 1
                                    If j <> dgvCol_DelvDetails.order_pcs And j <> dgvCol_DelvDetails.order_mtrs And j <> dgvCol_DelvDetails.cancel_mtrs Then
                                        .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                    End If
                                Next j
                                LockSTS = True
                            End If

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bales").ToString)
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Order_Meters").ToString), "########0.00")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_AMount").ToString), "########0.00")
                    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_Cancel_Meters").ToString), "########0.00")
                End With


            End If




            da3 = New SqlClient.SqlDataAdapter("Select * from ClothSales_Order_Delivery_Schedule_Details where ClothSales_Order_Code = '" & Trim(NewCode) & "' Order by sl_no", con)
            dt3 = New DataTable
            da3.Fill(dt3)

            If dt3.Rows.Count > 0 Then

                With Dgv_Delivery_Schedule_Details_Hidden

                    For i As Integer = 0 To dt3.Rows.Count - 1

                        n = .Rows.Add()

                        .Rows(n).Cells(0).Value = dt3.Rows(i).Item("SL_NO").ToString
                        .Rows(n).Cells(1).Value = dt3.Rows(i).Item("Delivery_Schedule_Details_SlNo").ToString
                        .Rows(n).Cells(2).Value = Trim(dt3.Rows(i).Item("Delivery_Date").ToString)
                        .Rows(n).Cells(3).Value = Val(dt3.Rows(i).Item("Meters").ToString)
                        .Rows(n).Cells(4).Value = ""

                    Next

                End With

            End If

            Grid_Cell_DeSelect()

            If LockSTS = True Then

                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_Agent.Enabled = False
                cbo_Agent.BackColor = Color.LightGray

                cbo_DespTo.Enabled = False
                cbo_DespTo.BackColor = Color.LightGray

                cbo_Through.Enabled = False
                cbo_Through.BackColor = Color.LightGray

                cbo_Transport.Enabled = False
                cbo_Transport.BackColor = Color.LightGray

                txt_OrderNo.Enabled = False
                txt_OrderNo.BackColor = Color.LightGray

                txt_DelvAdd1.Enabled = False
                txt_DelvAdd1.BackColor = Color.LightGray

                txt_DelvAdd2.Enabled = False
                txt_DelvAdd2.BackColor = Color.LightGray

                txt_com_per.Enabled = True
                txt_com_per.BackColor = Color.LightGray

                cbo_Com_Type.Enabled = True
                cbo_Com_Type.BackColor = Color.LightGray

                cbo_Grid_ClothName.Enabled = True
                cbo_Grid_ClothName.BackColor = Color.LightGray

                cbo_Grid_Clothtype.Enabled = True
                cbo_Grid_Clothtype.BackColor = Color.LightGray


                cbo_Grid_loomType.Enabled = True
                cbo_Grid_loomType.BackColor = Color.LightGray

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()
            dgv_ActCtrlName = ""
        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Order_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Order_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ClothSales_Order_Indent_Entry, New_Entry, Me, con, "ClothSales_Order_Head", "ClothSales_Order_Code", NewCode, "ClothSales_Order_Date", "(ClothSales_Order_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "ClothSales_Order_Head", "Verified_Status", "(ClothSales_Order_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If



        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Da = New SqlClient.SqlDataAdapter("select sum(Delivery_Meters) from ClothSales_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some pieces delivered for this order", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select sum(Invoice_Meters) from ClothSales_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some pieces invoiced for this order", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction
        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "ClothSales_Order_head", "ClothSales_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "ClothSales_Order_Code, Company_IdNo, for_OrderBy", trans)

        Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "ClothSales_Order_Details", "ClothSales_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "   Cloth_IdNo,ClothType_IdNo,Fold_Perc,Bales,Order_Pcs,Order_Meters,Rate,Order_Cancel_Meters,ClothSales_Enquiry_No,ClothSales_Enquiry_Code,ClothSales_Enquiry_Slno ,Selection_Type", "Sl_No", "ClothSales_Order_Code, For_OrderBy, Company_IdNo, ClothSales_Order_No, ClothSales_Order_Date, Ledger_Idno", trans)

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            cmd.Connection = con
            cmd.Transaction = trans
            cmd.CommandText = "Update ClothSales_Enquiry_Details set Order_Meters = a.Order_Meters - b.Order_Meters from ClothSales_Enquiry_Details a, ClothSales_Order_Details b Where b.ClothSales_Order_Code = '" & Trim(NewCode) & "' and b.Selection_Type = 'ENQUIRY' and a.ClothSales_Enquiry_code = b.ClothSales_Enquiry_code and a.ClothSales_Enquiry_SlNo = b.ClothSales_Enquiry_SlNo"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from ClothSales_Order_Delivery_Schedule_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from ClothSales_Order_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from ClothSales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead  Where ( Ledger_IdNo = 0 or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt2)
            cbo_Filter_ClothName.DataSource = dt2
            cbo_Filter_ClothName.DisplayMember = "Cloth_Name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.Text = ""
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Order_No from ClothSales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, ClothSales_Order_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Order_No from ClothSales_Order_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, ClothSales_Order_No asc", con)
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

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Order_No from ClothSales_Order_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, ClothSales_Order_No desc", con)
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

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Order_No from ClothSales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, ClothSales_Order_No desc", con)
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

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "ClothSales_Order_Head", "ClothSales_Order_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode)
            lbl_RefNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString

            Da = New SqlClient.SqlDataAdapter("select top 1 * from ClothSales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, ClothSales_Order_No desc", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("ClothSales_Order_Date").ToString <> "" Then msk_date.Text = Dt1.Rows(0).Item("ClothSales_Order_Date").ToString
                End If

                If Dt1.Rows(0).Item("GST_PERC").ToString <> "" Then txt_gst_percentage.Text = Dt1.Rows(0).Item("GST_PERC").ToString
                If Dt1.Rows(0).Item("freight_Ac").ToString <> "" Then cbo_fright_ac.Text = Dt1.Rows(0).Item("freight_Ac").ToString

            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()

            If msk_date.Enabled And msk_date.Visible Then
                msk_date.Focus()
                msk_date.SelectionStart = 0
            End If

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Order Ref No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select ClothSales_Order_No from ClothSales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(InvCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Order Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

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
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Order_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Order_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.ClothSales_Order_Indent_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Order Ref No.", "FOR NEW REF NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select ClothSales_Order_No from ClothSales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(InvCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Order Ref No.", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT ...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim clth_ID As Integer = 0
        Dim clthtyp_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim Led_ID As Integer = 0
        Dim Ag_ID As Integer = 0
        Dim Sno As Integer = 0, Sno2 As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotBls As Integer, vTotPcs As Single, vTotOrdMtrs As Single, vTotCnlMtrs As Single, vTotAMount As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim Usr_ID As Integer = 0
        Dim Nr As Integer = 0
        Dim vGrDt As String = ""
        Dim EnqCd As String = ""
        Dim EnqNo As String = ""
        Dim EnqSlno As Integer = 0
        Dim vOrdByNo As String = ""
        Dim vDel_ID As Integer = 0
        Dim vloomType_ID As Integer = 0
        Dim vCLOSE_STS As Integer = 0
        Dim vReadyStkAvailable_STS As Integer = 0
        Dim Verified_STS As String = ""
        Dim vSELC_DCCODE As String
        Dim vCloSal_OrdDate As Date = #6/13/2025#
        Dim vCREATED_DTTM_TXT As String = ""
        Dim vMODIFIED_DTTM_TXT As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Val(Common_Procedures.User.IdNo) = 0 Then
            MessageBox.Show("Invalid User Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.ClothSales_Order_Entry, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ClothSales_Order_Indent_Entry, New_Entry, Me, con, "ClothSales_Order_Head", "ClothSales_Order_Code", NewCode, "ClothSales_Order_Date", "(ClothSales_Order_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, ClothSales_Order_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "ClothSales_Order_Head", "Verified_Status", "(ClothSales_Order_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
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

        If Common_Procedures.settings.CustomerCode = "1267" Then
            If txt_Note.Text = "" Then
                MessageBox.Show("Invalid Note", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Note.Enabled And txt_Note.Visible Then txt_Note.Focus()
                Exit Sub
            End If
            If txt_PaymentTerms.Text = "" Then
                MessageBox.Show("Invalid Payment terms", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_PaymentTerms.Enabled And txt_PaymentTerms.Visible Then txt_PaymentTerms.Focus()
                Exit Sub
            End If
        End If


        If EntFnYrCode = Common_Procedures.FnYearCode Then
            If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
                Exit Sub
            End If
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If


        vDel_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)

        'If vDel_ID = 0 Then
        '    MessageBox.Show("Invalid Delivery At", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_DeliveryTo.Enabled Then cbo_DeliveryTo.Focus()
        '    Exit Sub
        'End If

        Ag_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)


        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo

        If Trim(UCase(cbo_Type.Text)) = "" Or (Trim(UCase(cbo_Type.Text)) <> "ENQUIRY") Then
            cbo_Type.Text = "DIRECT"
        End If
        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(dgvCol_DelvDetails.order_mtrs).Value) <> 0 Then

                clth_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(i).Cells(dgvCol_DelvDetails.clothname).Value)
                If clth_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_DelvDetails.clothname)
                    End If
                    Exit Sub
                End If

                clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(i).Cells(dgvCol_DelvDetails.type).Value)
                If clthtyp_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_DelvDetails.type)
                    End If
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(dgvCol_DelvDetails.fold).Value) = 0 Then
                    MessageBox.Show("Invalid Folding", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_DelvDetails.fold)
                    End If
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(dgvCol_DelvDetails.order_mtrs).Value) = 0 Then
                    MessageBox.Show("Invalid Order metres", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_DelvDetails.order_mtrs)
                    End If
                    Exit Sub
                End If

            End If

        Next

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1

        vCLOSE_STS = 0
        If chk_CloseStatus.Checked = True Then vCLOSE_STS = 1

        vReadyStkAvailable_STS = 0
        If Chk_ReadyStockAvailable.Checked = True Then vReadyStkAvailable_STS = 1

        vGrDt = ""
        If Trim(msk_GrDate.Text) <> "" Then
            If IsDate(msk_GrDate.Text) = True Then
                vGrDt = Trim(msk_GrDate.Text)
            End If
        End If

        NoCalc_Status = False
        Total_Calculation()

        vTotBls = 0 : vTotPcs = 0 : vTotOrdMtrs = 0 : vTotCnlMtrs = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBls = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotOrdMtrs = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotAMount = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            vTotCnlMtrs = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
        End If

        If vTotOrdMtrs = 0 Then
            MessageBox.Show("Invalid Order Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dgv_Details.Enabled And dgv_Details.Visible Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_DelvDetails.order_mtrs)
            End If
            Exit Sub
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "ClothSales_Order_Head", "ClothSales_Order_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode, tr)
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            End If

            If dtp_Date.Value.Date >= vCloSal_OrdDate.Date And Common_Procedures.settings.CustomerCode = "1267" Then
                vSELC_DCCODE = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))
            Else
                vSELC_DCCODE = Trim(txt_OrderNo.Text) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))
            End If


            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(msk_date.Text))

            Dim vDelv_duedate As String = ""
            vDelv_duedate = ""
            If msk_delivery_date.Visible Then
                If Trim(msk_delivery_date.Text) <> "" Then
                    If IsDate(msk_delivery_date.Text) = True Then
                        cmd.Parameters.AddWithValue("@DELIVERYDate", Convert.ToDateTime(msk_delivery_date.Text))
                        vDelv_duedate = Trim(msk_delivery_date.Text)
                    End If
                End If
            End If

            Dim vFinalDelv_date As String = ""
            vFinalDelv_date = ""
            If Msk_Final_Delv_Date.Visible Then
                If Trim(Msk_Final_Delv_Date.Text) <> "" Then
                    If IsDate(Msk_Final_Delv_Date.Text) = True Then
                        cmd.Parameters.AddWithValue("@FinalDELIVERYDate", Convert.ToDateTime(Msk_Final_Delv_Date.Text))
                        vFinalDelv_date = Trim(Msk_Final_Delv_Date.Text)
                    End If
                End If
            End If

            Dim ms9 As New MemoryStream()
            If IsNothing(pic_PartyPO_Image.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(pic_PartyPO_Image.BackgroundImage)
                bitmp.Save(ms9, Drawing.Imaging.ImageFormat.Jpeg)
            Else
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                    Throw New ApplicationException("Invalid Party Order image attachment")
                End If
            End If
            Dim data9 As Byte() = ms9.GetBuffer()
            Dim N9 As New SqlClient.SqlParameter("@partydc_image", SqlDbType.Image)
            N9.Value = data9
            cmd.Parameters.Add(N9)
            ms9.Dispose()

            vCREATED_DTTM_TXT = ""
            vMODIFIED_DTTM_TXT = ""

            vCREATED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@createddatetime", Now)


            vMODIFIED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@modifieddatetime", Now)

            If New_Entry = True Then
                cmd.CommandText = "Insert into ClothSales_Order_Head ( ClothSales_Order_Code     ,               Company_IdNo       ,     ClothSales_Order_No       ,                     for_OrderBy                                        , ClothSales_Order_Date  ,              Ledger_IdNo,          Party_OrderNo              ,            Through_Name         ,     Agent_IdNo         ,  Agent_Comm_Perc                  , Agent_Comm_Type                  ,   Despatch_To                  ,   Transport_IdNo          ,  Delivery_Address1               , Delivery_Address2                ,                      Note     ,            Total_Bales   ,               Total_Pcs  ,          Total_Order_Meters  ,           Total_Cancel_Meters,           Total_AMount  ,  User_idNo                      ,   Gr_Time                        ,         Gr_Date      ,      Selection_Type          ,       Payment_Terms                ,          Verified_Status ,       DeliveryTo_idno     ,                   Vehicle_no      ,                   Attend          ,                Delivery_Schedule                       ,Discount                   ,GST_PERC                    ,           freight_Ac             ,        Discount_Amount             ,           Tax_type          ,          delivery_Due_Days                  ,    delivery_Due_Date                                                 ,    Order_Close_Status    ,    ClothSales_OrderCode_forSelection ,            Final_Delivery_date                                            ,             Piece_Meters          ,           Bale_Meters           , PartyPO_Document_Image , Ready_Stock_Available_Status  ,                         created_useridno           ,   created_DateTime,          created_DateTime_Text    , Last_modified_useridno, Last_modified_DateTime, Last_modified_DateTime_Text  ) " &
                " Values                                             (   '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @OrderDate       , " & Str(Val(Led_ID)) & ",   '" & Trim(txt_OrderNo.Text) & "'  , '" & Trim(cbo_Through.Text) & "', " & Str(Val(Ag_ID)) & ", " & Str(Val(txt_com_per.Text)) & ", '" & Trim(cbo_Com_Type.Text) & "', '" & Trim(cbo_DespTo.Text) & "', " & Str(Val(Trans_ID)) & ", '" & Trim(txt_DelvAdd1.Text) & "', '" & Trim(txt_DelvAdd2.Text) & "', '" & Trim(txt_Note.Text) & "' , " & Str(Val(vTotBls)) & ", " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotOrdMtrs)) & ", " & Str(Val(vTotCnlMtrs)) & "  , " & Str(Val(vTotAMount)) & ",  " & Val(lbl_UserName.Text) & " , " & Str(Val(txt_GrTime.Text)) & ", '" & Trim(vGrDt) & "', '" & Trim(cbo_Type.Text) & "','" & Trim(txt_PaymentTerms.Text) & "', " & Val(Verified_STS) & ", " & Str(Val(vDel_ID)) & ",'" & Trim(txt_vehicle_no.Text) & "','" & Trim(txt_Attend.Text) & "','" & Trim(txt_delivery_Schedule.Text) & "'," & Val(txt_Disc.Text) & "," & Val(txt_gst_percentage.Text) & ",'" & Trim(cbo_fright_ac.Text) & "'," & Str(Val(lbl_DiscAmount.Text)) & ",'" & Trim(cbo_taxType.Text) & "' ,'" & Trim(txt_delivery_due_days.Text) & "'   , " & IIf(IsDate(vDelv_duedate) = True, "@DELIVERYDate", "Null") & "    , " & Val(vCLOSE_STS) & " ,     '" & Trim(vSELC_DCCODE) & "'      ,  " & IIf(IsDate(vFinalDelv_date) = True, "@FinalDELIVERYDate", "Null") & "  , " & Val(txt_piece_Meters.Text) & "," & Val(txt_bale_meters.Text) & ",   @partydc_image  , " & Val(vReadyStkAvailable_STS) & "  ,      " & Str(Val(Common_Procedures.User.IdNo)) & ",  @createddatetime ,  '" & Trim(vCREATED_DTTM_TXT) & "',              0        ,     NUll              ,          ''      ) "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "ClothSales_Order_head", "ClothSales_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "ClothSales_Order_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "ClothSales_Order_Details", "ClothSales_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "   Cloth_IdNo,ClothType_IdNo,Fold_Perc,Bales,Order_Pcs,Order_Meters,Rate,Order_Cancel_Meters,ClothSales_Enquiry_No,ClothSales_Enquiry_Code,ClothSales_Enquiry_Slno ,Selection_Type", "Sl_No", "ClothSales_Order_Code, For_OrderBy, Company_IdNo, ClothSales_Order_No, ClothSales_Order_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update ClothSales_Order_Head set ClothSales_Order_Date = @OrderDate, Ledger_IdNo =  " & Str(Val(Led_ID)) & " , Party_OrderNo =  '" & Trim(txt_OrderNo.Text) & "',            Through_Name = '" & Trim(cbo_Through.Text) & "'              ,     Agent_IdNo = " & Str(Val(Ag_ID)) & "    ,  Agent_Comm_Perc = " & Str(Val(txt_com_per.Text)) & "       , Agent_Comm_Type = '" & Trim(cbo_Com_Type.Text) & "' ,   Despatch_To = '" & Trim(cbo_DespTo.Text) & "',   Transport_IdNo = " & Str(Val(Trans_ID)) & "       ,  Delivery_Address1 = '" & Trim(txt_DelvAdd1.Text) & "', Delivery_Address2 = '" & Trim(txt_DelvAdd2.Text) & "'  ,  Note = '" & Trim(txt_Note.Text) & "' , Total_Bales = " & Str(Val(vTotBls)) & "  ,    Gr_Time = " & Str(Val(txt_GrTime.Text)) & ", Gr_Date = '" & Trim(vGrDt) & "', Total_Pcs = " & Str(Val(vTotPcs)) & "  ,  Total_Order_Meters = " & Str(Val(vTotOrdMtrs)) & ", Total_Cancel_Meters = " & Str(Val(vTotCnlMtrs)) & " ,Total_AMount= " & Str(Val(vTotAMount)) & ", User_IdNo =  " & Val(lbl_UserName.Text) & ", Selection_Type = '" & Trim(cbo_Type.Text) & "' , Payment_Terms = '" & Trim(txt_PaymentTerms.Text) & "',Verified_Status= " & Val(Verified_STS) & " ,DeliveryTo_idno= " & Str(Val(vDel_ID)) & ",Vehicle_No='" & Trim(txt_vehicle_no.Text) & "',Attend='" & Trim(txt_Attend.Text) & "',Delivery_Schedule='" & Trim(txt_delivery_Schedule.Text) & "',Discount=" & Val(txt_Disc.Text) & ",GST_PERC=" & Val(txt_gst_percentage.Text) & ",freight_Ac='" & Trim(cbo_fright_ac.Text) & "',Discount_Amount=" & Str(Val(lbl_DiscAmount.Text)) & " ,Tax_type='" & Trim(cbo_taxType.Text) & "' ,Delivery_due_days='" & Trim(txt_delivery_due_days.Text) & "', Delivery_due_date = " & IIf(IsDate(vDelv_duedate) = True, "@DELIVERYDate", "Null") & " , Order_Close_Status = " & Val(vCLOSE_STS) & " ,  ClothSales_OrderCode_forSelection = '" & Trim(vSELC_DCCODE) & "' , Final_Delivery_date =  " & IIf(IsDate(vFinalDelv_date) = True, "@FinalDELIVERYDate", "Null") & " , Piece_Meters = " & Val(txt_piece_Meters.Text) & " , Bale_Meters = " & Val(txt_bale_meters.Text) & " , PartyPO_Document_Image = @partydc_image , Ready_Stock_Available_Status = " & Val(vReadyStkAvailable_STS) & " ,Last_modified_useridno = " & Str(Val(Common_Procedures.User.IdNo)) & ", Last_modified_DateTime = @modifieddatetime, Last_modified_DateTime_Text = '" & Trim(vMODIFIED_DTTM_TXT) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Update ClothSales_Enquiry_Details set Order_Meters = a.Order_Meters - b.Order_Meters from ClothSales_Enquiry_Details a, ClothSales_Order_Details b Where b.ClothSales_Order_Code = '" & Trim(NewCode) & "' and b.Selection_Type = 'ENQUIRY' and a.ClothSales_Enquiry_code = b.ClothSales_Enquiry_code and a.ClothSales_Enquiry_SlNo = b.ClothSales_Enquiry_SlNo"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "ClothSales_Order_head", "ClothSales_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "ClothSales_Order_Code, Company_IdNo, for_OrderBy", tr)


            cmd.CommandText = "Delete from ClothSales_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(NewCode) & "' and Invoice_Meters = 0 and Delivery_Meters = 0"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from ClothSales_Order_Delivery_Schedule_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(dgvCol_DelvDetails.order_mtrs).Value) <> 0 Then

                        Sno = Sno + 1

                        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_DelvDetails.clothname).Value, tr)

                        clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(dgvCol_DelvDetails.type).Value, tr)

                        vloomType_ID = Common_Procedures.LoomType_NameToIdNo(con, .Rows(i).Cells(dgvCol_DelvDetails.loom_type).Value, tr)


                        EnqCd = ""
                        EnqSlno = 0
                        EnqNo = ""
                        If Trim(UCase(cbo_Type.Text)) = "ENQUIRY" Then
                            EnqNo = Trim(.Rows(i).Cells(dgvCol_DelvDetails.enq_no).Value)
                            EnqCd = Trim(.Rows(i).Cells(dgvCol_DelvDetails.clothsales_enquiry_code).Value)

                            EnqSlno = Val(.Rows(i).Cells(dgvCol_DelvDetails.clothsales_enquiry_slno).Value)
                        End If

                        Nr = 0
                        cmd.CommandText = "Update ClothSales_Order_Details set ClothSales_Order_Date = @OrderDate , Ledger_IdNo = " & Str(Val(Led_ID)) & ", Sl_No  = " & Str(Val(Sno)) & " , Cloth_IdNo = " & Str(Val(clth_ID)) & " , ClothType_IdNo = " & Str(Val(clthtyp_ID)) & " , Fold_Perc =  " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.fold).Value)) & ", Bales = " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.fold).Value)) & " , Order_Pcs = " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.order_pcs).Value)) & " ,       Order_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.order_mtrs).Value)) & " , Rate= " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.rate).Value)) & "  ,  Amount =  " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Amount).Value)) & "   , Order_Cancel_Meters =  " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.cancel_mtrs).Value)) & " ,   ClothSales_Enquiry_No  ='" & Trim(EnqNo) & "'          ,  ClothSales_Enquiry_Code  = '" & Trim(EnqCd) & "'         ,    ClothSales_Enquiry_Slno = " & Val(EnqSlno) & ", Selection_Type= '" & Trim(cbo_Type.Text) & "' , LoomType_idno = " & Str(Val(vloomType_ID)) & " , Delivery_Schedule_Details_SlNo = " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Delivery_Schedule_Details_SlNo).Value)) & ", Meters_per_Bill = " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Meters_per_Bill).Value)) & "  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(NewCode) & "'  and ClothSales_Order_SlNo = " & Val(.Rows(i).Cells(dgvCol_DelvDetails.clothsales_order_slno).Value)
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into ClothSales_Order_Details (     ClothSales_Order_Code ,               Company_IdNo       ,       ClothSales_Order_No     ,                     for_OrderBy                                        , ClothSales_Order_Date       ,      Ledger_IdNo        ,          Sl_No       ,        Cloth_IdNo          ,       ClothType_IdNo        ,                      Fold_Perc                                 ,                      Bales                                       ,                      Order_Pcs                                      ,                      Order_Meters                                    ,                       Rate                                      ,                      Order_Cancel_Meters                               ,     ClothSales_Enquiry_No ,  ClothSales_Enquiry_Code ,    ClothSales_Enquiry_Slno ,        Selection_Type        ,          LoomType_idno         ,                      AMount                                      ,                      Delivery_Schedule_Details_SlNo                                      ,                      Meters_per_Bill                                       ) " &
                                                "     Values                        (   '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @OrderDate            , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", " & Str(Val(clth_ID)) & "  , " & Str(Val(clthtyp_ID)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.fold).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.bales).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.order_pcs).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.order_mtrs).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.rate).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.cancel_mtrs).Value)) & " ,  '" & Trim(EnqNo) & "'    ,  '" & Trim(EnqCd) & "'   , " & Val(EnqSlno) & "       , '" & Trim(cbo_Type.Text) & "', " & Str(Val(vloomType_ID)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Amount).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Delivery_Schedule_Details_SlNo).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Meters_per_Bill).Value)) & " ) "
                            cmd.ExecuteNonQuery()
                        End If
                        If Trim(UCase(cbo_Type.Text)) = "ENQUIRY" And Trim(.Rows(i).Cells(dgvCol_DelvDetails.clothsales_enquiry_code).Value) <> "" Then
                            Nr = 0
                            cmd.CommandText = "Update ClothSales_Enquiry_Details set Order_Meters = Order_Meters + " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.order_mtrs).Value)) & " Where ClothSales_Enquiry_code = '" & Trim(.Rows(i).Cells(dgvCol_DelvDetails.clothsales_enquiry_code).Value) & "' and ClothSales_Enquiry_SlNo = " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.clothsales_enquiry_slno).Value)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Order and Party Details")
                                Exit Sub
                            End If
                        End If


                        Sno2 = 0
                        For J As Integer = 0 To Dgv_Delivery_Schedule_Details_Hidden.RowCount - 1
                            Sno2 = Sno2 + 1
                            If Val(Dgv_Delivery_Schedule_Details_Hidden.Rows(J).Cells(2).Value) <> 0 And Val(Dgv_Delivery_Schedule_Details_Hidden.Rows(J).Cells(1).Value) = Val(dgv_Details.Rows(i).Cells(18).Value) Then

                                cmd.CommandText = "Insert into ClothSales_Order_Delivery_Schedule_Details ( ClothSales_Order_Code    ,            Company_IdNo           ,       Sl_No      ,                               Delivery_Schedule_Details_SlNo    ,                                      Delivery_Date                ,                                    Meters                      ) " &
                                          "                 values                                        (  '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " ," & Val(Sno2) & " , " & Val(Dgv_Delivery_Schedule_Details_Hidden.Rows(J).Cells(1).Value) & "  , '" & Trim(Dgv_Delivery_Schedule_Details_Hidden.Rows(J).Cells(2).Value) & "' , " & Val(Dgv_Delivery_Schedule_Details_Hidden.Rows(J).Cells(3).Value) & " )  "

                                cmd.ExecuteNonQuery()

                            End If

                        Next


                    End If

                Next

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "ClothSales_Order_Details", "ClothSales_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "   Cloth_IdNo,ClothType_IdNo,Fold_Perc,Bales,Order_Pcs,Order_Meters,Rate,Order_Cancel_Meters,ClothSales_Enquiry_No,ClothSales_Enquiry_Code,ClothSales_Enquiry_Slno ,Selection_Type", "Sl_No", "ClothSales_Order_Code, For_OrderBy, Company_IdNo, ClothSales_Order_No, ClothSales_Order_Date, Ledger_Idno", tr)

            End With

            tr.Commit()

            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

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
            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("CK_Stores_Item_PO_Details"))) > 0 Then
                MessageBox.Show("Invalid Po Meters - Lesser than Delivery/Invoce Meters", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBls As Integer
        Dim TotPcs As Single
        Dim TotOrdMtrs As Double
        Dim TotCnlMtrs As Double
        Dim TotAmt As Double

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotBls = 0 : TotPcs = 0 : TotOrdMtrs = 0 : TotCnlMtrs = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(dgvCol_DelvDetails.SNo).Value = Sno
                If Val(.Rows(i).Cells(dgvCol_DelvDetails.order_mtrs).Value) <> 0 Then

                    TotBls = TotBls + Val(.Rows(i).Cells(dgvCol_DelvDetails.bales).Value())
                    TotPcs = TotPcs + Val(.Rows(i).Cells(dgvCol_DelvDetails.order_pcs).Value())
                    TotOrdMtrs = TotOrdMtrs + Val(.Rows(i).Cells(dgvCol_DelvDetails.order_mtrs).Value())
                    TotCnlMtrs = TotCnlMtrs + Val(.Rows(i).Cells(dgvCol_DelvDetails.cancel_mtrs).Value())
                    TotAmt = TotAmt + Val(.Rows(i).Cells(dgvCol_DelvDetails.Amount).Value())
                End If

            Next i

        End With


        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBls)
            .Rows(0).Cells(5).Value = Val(TotPcs)
            .Rows(0).Cells(6).Value = Format(Val(TotOrdMtrs), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotAmt), "########0.00")
            .Rows(0).Cells(9).Value = Format(Val(TotCnlMtrs), "########0.00")
        End With

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' or Ledger_Type = 'JOBWORKER'  and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' or Ledger_Type = 'JOBWORKER'  and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
        If (e.KeyValue = 40 And cbo_PartyName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(cbo_Type.Text)) = "ENQUIRY" Then
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_DelvDetails.order_mtrs)

                Else

                    If txt_PaymentTerms.Visible And txt_PaymentTerms.Enabled Then
                        txt_PaymentTerms.Focus()
                    Else
                        txt_Note.Focus()
                    End If

                End If
            Else
                txt_OrderNo.Focus()
            End If
        End If

        If (e.KeyValue = 38 And cbo_PartyName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_taxType.Visible And cbo_taxType.Enabled Then
                cbo_taxType.Focus()
            Else
                cbo_Type.Focus()
            End If

        End If

    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' or Ledger_Type = 'JOBWORKER'  and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "ENQUIRY" Then
                If MessageBox.Show("Do you want to select Cloth Enquiry :", "FOR CLOTH ENQUIRY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    txt_OrderNo.Focus()

                End If

            Else
                txt_OrderNo.Focus()

            End If

        End If
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
    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
        cbo_Agent.Tag = cbo_Agent.Text
    End Sub
    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, cbo_Through, txt_com_per, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_com_per, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Agent.Tag)) <> Trim(UCase(cbo_Agent.Text)) Then
                cbo_Agent.Tag = cbo_Agent.Text
                Get_AgentComm()
            End If
        End If
    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Com_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Com_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Com_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Com_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Com_Type, txt_com_per, txt_GrTime, "", "", "", "")
    End Sub

    Private Sub cbo_Com_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Com_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Com_Type, txt_GrTime, "", "", "", "")
    End Sub

    Private Sub cbo_DespTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DespTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Delivery_Head", "Despatch_To", "", "")

    End Sub
    Private Sub cbo_DespTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DespTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DespTo, msk_GrDate, Nothing, "ClothSales_Order_Head", "Despatch_To", "", "")

        If e.KeyCode = 40 Then
            If txt_delivery_due_days.Enabled And txt_delivery_due_days.Visible = True Then

                txt_delivery_due_days.Focus()

            Else

                cbo_DeliveryTo.Focus()

            End If
        End If

    End Sub

    Private Sub cbo_DespTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DespTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DespTo, Nothing, "ClothSales_Order_Head", "Despatch_To", "", "", False)


        If Asc(e.KeyChar) = 13 Then

            If txt_delivery_due_days.Enabled And txt_delivery_due_days.Visible = True Then
                txt_delivery_due_days.Focus()

            Else
                cbo_DeliveryTo.Focus()
            End If

        End If

    End Sub
    Private Sub cbo_Through_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Through.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Through_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Through.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Through, txt_OrderNo, cbo_Agent, "", "", "", "")
    End Sub

    Private Sub cbo_Through_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Through.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Through, cbo_Agent, "", "", "", "")
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
            txt_vehicle_no.Text = Dt.Rows(0).Item("vehicle_no").ToString
        End If
        Dt.Clear()
    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()
    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, Nothing, txt_Note, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_Transport.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_fright_ac.Visible = True Then

                cbo_fright_ac.Focus()

            Else

                If txt_Attend.Visible = True Then
                    txt_Attend.Focus()
                Else
                    cbo_DespTo.Focus()
                End If
            End If
        End If
        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Note, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.KeyValue = 17 Then

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

    Private Sub cbo_Grid_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_ClothName.Text)

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown

        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, cbo_DeliveryTo, cbo_Grid_Clothtype, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    If Trim(UCase(cbo_Type.Text)) = "ENQUIRY" Then
                        cbo_PartyName.Focus()
                    Else
                        If txt_PaymentTerms.Visible = True Then
                            txt_PaymentTerms.Focus()
                        Else
                            cbo_DeliveryTo.Focus()

                        End If

                    End If
                Else
                    .Focus()
                    If .Columns(dgvCol_DelvDetails.Meters_per_Bill).Visible = True Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_DelvDetails.Meters_per_Bill)
                    ElseIf .Columns(dgvCol_DelvDetails.Delivery_Schedule_Selection).Visible = True Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_DelvDetails.Delivery_Schedule_Selection)
                    ElseIf .Columns(dgvCol_DelvDetails.cancel_mtrs).Visible = True And .Columns(dgvCol_DelvDetails.cancel_mtrs).ReadOnly = False Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_DelvDetails.cancel_mtrs)
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_DelvDetails.rate)
                    End If
                    .Focus()
                    '.CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_DelvDetails.cancel_mtrs)
                    '.CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(dgvCol_DelvDetails.clothname).Value) = "" Then
                    txt_Disc.Focus()


                Else

                    If .Columns(dgvCol_DelvDetails.type).Visible = True And .Columns(dgvCol_DelvDetails.fold).Visible = True And .Columns(dgvCol_DelvDetails.bales).Visible = True Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    Else
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.loom_type)

                    End If
                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, cbo_Grid_Clothtype, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.clothname).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_Disc.Focus()

                Else
                    If .Columns(dgvCol_DelvDetails.type).Visible = True And .Columns(dgvCol_DelvDetails.fold).Visible = True And .Columns(dgvCol_DelvDetails.bales).Visible = True Then
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.type)

                    Else
                        .Focus()

                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.loom_type)

                    End If

                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Clothtype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_Clothtype.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_Clothtype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Clothtype.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Clothtype, Nothing, Nothing, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Clothtype.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Clothtype.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.fold)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Clothtype_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Clothtype.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Clothtype, Nothing, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.fold)

            End With

        End If

    End Sub


    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle
        Dim Delv_Schedule_Width As Integer

        With dgv_Details

            dgv_ActCtrlName = .Name

            If Val(.CurrentRow.Cells(dgvCol_DelvDetails.SNo).Value) = 0 Then
                .CurrentRow.Cells(dgvCol_DelvDetails.SNo).Value = .CurrentRow.Index + 1
            End If

            If Val(.Rows(e.RowIndex).Cells(18).Value) = 0 Then
                Set_Max_DetailsSlNo(e.RowIndex, 18)
            End If

            If Trim(.CurrentRow.Cells(dgvCol_DelvDetails.type).Value) = "" Then
                .CurrentRow.Cells(dgvCol_DelvDetails.type).Value = "SOUND"
            End If

            If Val(.CurrentRow.Cells(dgvCol_DelvDetails.fold).Value) = 0 Then
                .CurrentRow.Cells(dgvCol_DelvDetails.fold).Value = "100"
            End If

            If e.ColumnIndex = dgvCol_DelvDetails.clothname And (Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.clothsales_invoice_meters).Value) = 0 And Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.clothsales_delivery_meters).Value) = 0) Then

                If (cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex) Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + rect.Left
                    cbo_Grid_ClothName.Top = .Top + rect.Top

                    cbo_Grid_ClothName.Width = rect.Width
                    cbo_Grid_ClothName.Height = rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()

                End If

            Else
                cbo_Grid_ClothName.Visible = False

            End If

            If e.ColumnIndex = dgvCol_DelvDetails.type And (Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.clothsales_invoice_meters).Value) = 0 And Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.clothsales_delivery_meters).Value) = 0) Then

                If cbo_Grid_Clothtype.Visible = False Or Val(cbo_Grid_Clothtype.Tag) <> e.RowIndex Then

                    cbo_Grid_Clothtype.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Clothtype.DataSource = Dt1
                    cbo_Grid_Clothtype.DisplayMember = "ClothType_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Clothtype.Left = .Left + rect.Left
                    cbo_Grid_Clothtype.Top = .Top + rect.Top

                    cbo_Grid_Clothtype.Width = rect.Width
                    cbo_Grid_Clothtype.Height = rect.Height
                    cbo_Grid_Clothtype.Text = .CurrentCell.Value

                    cbo_Grid_Clothtype.Tag = Val(e.RowIndex)
                    cbo_Grid_Clothtype.Visible = True

                    cbo_Grid_Clothtype.BringToFront()
                    cbo_Grid_Clothtype.Focus()

                End If

            Else
                cbo_Grid_Clothtype.Visible = False

            End If




            If e.ColumnIndex = dgvCol_DelvDetails.loom_type Then

                If cbo_Grid_loomType.Visible = False Or Val(cbo_Grid_loomType.Tag) <> e.RowIndex Then

                    cbo_Grid_loomType.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select LoomType_Name from LoomType_Head order by LoomType_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_loomType.DataSource = Dt1
                    cbo_Grid_loomType.DisplayMember = "LoomType_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_loomType.Left = .Left + rect.Left
                    cbo_Grid_loomType.Top = .Top + rect.Top

                    cbo_Grid_loomType.Width = rect.Width
                    cbo_Grid_loomType.Height = rect.Height
                    cbo_Grid_loomType.Text = .CurrentCell.Value

                    cbo_Grid_loomType.Tag = Val(e.RowIndex)
                    cbo_Grid_loomType.Visible = True

                    cbo_Grid_loomType.BringToFront()
                    cbo_Grid_loomType.Focus()

                End If

            Else
                cbo_Grid_loomType.Visible = False

            End If






            If e.ColumnIndex = dgvCol_DelvDetails.Delivery_Schedule_Selection Then


                rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)



                btn_Delv_Selection.Left = .Left + .GetCellDisplayRectangle(17, e.RowIndex, False).Left

                btn_Delv_Selection.Top = .Top + rect.Top
                btn_Delv_Selection.Height = rect.Height
                btn_Delv_Selection.Visible = True

                btn_Delv_Selection.BringToFront()

                'pnl_LoomSelection_ToolTip.Left = .Left + rect.Left - 70
                'pnl_LoomSelection_ToolTip.Top = .Top + rect.Top + rect.Height + 3

                'pnl_LoomSelection_ToolTip.Visible = True

            Else

                pnl_LoomSelection_ToolTip.Visible = False
                btn_Delv_Selection.Visible = False



            End If



        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = dgvCol_DelvDetails.order_mtrs Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.cancel_mtrs Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.rate Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details

            If .Visible Then
                If .CurrentCell.ColumnIndex = dgvCol_DelvDetails.bales Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.order_pcs Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.order_mtrs Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.cancel_mtrs Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.rate Then
                    .CurrentRow.Cells(dgvCol_DelvDetails.Amount).Value = Format(Val(.CurrentRow.Cells(dgvCol_DelvDetails.order_mtrs).Value * .CurrentRow.Cells(dgvCol_DelvDetails.rate).Value), "#######0.00")

                    Total_Calculation()
                End If
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
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details
            'If e.KeyValue = Keys.Delete Then
            If .CurrentCell.ColumnIndex <> dgvCol_DelvDetails.order_pcs And .CurrentCell.ColumnIndex <> dgvCol_DelvDetails.order_mtrs And .CurrentCell.ColumnIndex <> dgvCol_DelvDetails.cancel_mtrs Then
                If Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.clothsales_invoice_meters).Value) <> 0 Or Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.clothsales_delivery_meters).Value) <> 0 Then
                    e.Handled = True
                    e.SuppressKeyPress = True
                End If
            End If
            'End If
        End With
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex <> dgvCol_DelvDetails.order_pcs And .CurrentCell.ColumnIndex <> dgvCol_DelvDetails.order_mtrs And .CurrentCell.ColumnIndex <> dgvCol_DelvDetails.cancel_mtrs Then
                        If Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.clothsales_invoice_meters).Value) <> 0 Or Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.clothsales_delivery_meters).Value) <> 0 Then
                            e.Handled = True
                            Exit Sub
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = dgvCol_DelvDetails.fold Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.bales Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.order_pcs Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.order_mtrs Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.rate Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.cancel_mtrs Then
                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        With dgv_Details

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.ColumnIndex <= 1 Then
                    If .CurrentCell.RowIndex = 0 Then
                        txt_DelvAdd2.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                    End If
                End If
            End If

            If e.KeyCode = Keys.Right Then
                If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                    If .CurrentCell.RowIndex >= .Rows.Count - 1 Then
                        If txt_PaymentTerms.Visible And txt_PaymentTerms.Enabled Then
                            txt_PaymentTerms.Focus()
                        Else
                            txt_Note.Focus()
                        End If
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_DelvDetails.clothname)
                    End If
                End If
            End If

        End With

    End Sub


    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                If Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.clothsales_invoice_meters).Value) = 0 And Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.clothsales_delivery_meters).Value) = 0 Then

                    n = .CurrentRow.Index

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(dgvCol_DelvDetails.SNo).Value = i + 1
                    Next

                End If

            End With

        End If

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            If dgv_Details.CurrentCell.ColumnIndex = dgvCol_DelvDetails.Delivery_Schedule_Selection Then
                btn_Delv_Selection_Click(sender, e)
            End If
        End If


    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(dgvCol_DelvDetails.SNo).Value = Val(n)

            If Val(.Rows(e.RowIndex).Cells(18).Value) = 0 Then
                Set_Max_DetailsSlNo(e.RowIndex, 18)
            End If

        End With
    End Sub

    Private Sub txt_DelvAdd2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DelvAdd2.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_DelvDetails.clothname)

            Else
                If txt_PaymentTerms.Visible And txt_PaymentTerms.Enabled Then
                    txt_PaymentTerms.Focus()
                Else
                    txt_Note.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub txt_DelvAdd2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DelvAdd2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_DelvDetails.clothname)

            Else
                If txt_PaymentTerms.Visible And txt_PaymentTerms.Enabled Then
                    txt_PaymentTerms.Focus()
                Else
                    txt_Note.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then
            'cbo_fright_ac.Focus()

            cbo_Transport.Focus()

        End If
        If e.KeyValue = 40 Then

            If Msk_Final_Delv_Date.Enabled And Msk_Final_Delv_Date.Visible = True Then
                Msk_Final_Delv_Date.Focus()
            Else

                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    save_record()
                Else
                    msk_date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_Grid_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try
            If cbo_Grid_ClothName.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Clothtype_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.TextChanged
        Try
            If cbo_Grid_Clothtype.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Clothtype.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_DelvDetails.type Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Clothtype.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub cbo_Grid_loomtype_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_loomType.TextChanged
        Try
            If cbo_Grid_loomType.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_loomType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_DelvDetails.loom_type Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_loomType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then


            If Msk_Final_Delv_Date.Enabled And Msk_Final_Delv_Date.Visible = True Then
                Msk_Final_Delv_Date.Focus()
            Else

                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    save_record()
                Else
                    msk_date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clth_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clth_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothSales_Order_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.ClothSales_Order_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothSales_Order_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Clth_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Val(Clth_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Cloth_IdNo = " & Str(Val(Clth_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.ClothType_name, e.Ledger_Name from ClothSales_Order_Head a left outer join ClothSales_Order_Details b on a.ClothSales_Order_Code = b.ClothSales_Order_Code left outer join Cloth_head c on b.Cloth_idno = c.Cloth_idno left outer join ClothType_head d on b.ClothType_idno = d.ClothType_idno left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.ClothSales_Order_Code like '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by ClothSales_Order_Date, for_orderby, ClothSales_Order_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("ClothSales_Order_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ClothSales_Order_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Party_OrderNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Order_Meters").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

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
    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub
    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' or Ledger_Type = 'JOBWORKER'  and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' or Ledger_Type = 'JOBWORKER'  and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' or Ledger_Type = 'JOBWORKER'  and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.ClothSales_Order_Indent_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from ClothSales_Order_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(NewCode) & "'", con)
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


        PpSzSTS = False

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

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




        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

                Else

                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument1.Print()
                    End If
                End If
            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
        End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        PpSzSTS = False




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
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim NewCode As String
        Dim I As Integer, J As Integer

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        vDELVSCH_item1 = ""
        vDELVSCH_item2 = ""
        vDELVSCH_item3 = ""
        vDELVSCH_item4 = ""
        vDELVSCH_item5 = ""
        vDELVSCH_item6 = ""
        vDELVSCH_item7 = ""

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,f.* ,d.Ledger_MAINName as TransportName, Ah.Ledger_MAINName as Agent_Name,f.Ledger_MAINName as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.Pan_No as DeliveryTo_PanNo from ClothSales_Order_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo  Left outer JOIN Ledger_Head Ah ON a.Agent_IdNo = Ah.Ledger_IdNo LEFT OUTER JOIN Ledger_Head f ON  a.DeliveryTo_IdNo = f.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Order_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Cloth_name,b.ClothMain_Name,b.Sort_No,b.weave,b.Weight_Meter_Fabric,b.Cloth_Description, b.Stock_In, d.ClothType_name, L.LoomType_name ,I.Item_GST_Percentage,i.Item_HSN_Code from ClothSales_Order_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno LEFT OUTER JOIN loomType_Head L ON L.LoomType_idno = a.LoomType_idno lEFT oUTER JOIN ItemGroup_Head i ON B.ItemGroup_IdNo = i.ItemGroup_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Order_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)




                '-------------Code By Lalith 2025_05_28

                da3 = New SqlClient.SqlDataAdapter("select a.* from ClothSales_Order_Delivery_Schedule_Details a where  a.ClothSales_Order_Code = '" & Trim(NewCode) & "' and a.Meters > 0 ORDER BY A.SL_NO  ", con)
                prn_Delv_DetDt = New DataTable
                da3.Fill(prn_Delv_DetDt)

                If prn_Delv_DetDt.Rows.Count > 0 Then

                    Dim VCont As String

                    VCont = ""
                    For J = 0 To prn_Delv_DetDt.Rows.Count - 1


                        VCont = Trim(VCont) & IIf(Trim(VCont) <> "", ",", "") & Trim(prn_Delv_DetDt.Rows(J).Item("Delivery_Date").ToString) & "-" & Trim(prn_Delv_DetDt.Rows(J).Item("Meters").ToString) & " Mtrs"

                        vDELVSCH_item1 = Trim(VCont)
                        vDELVSCH_item2 = ""
                        vDELVSCH_item3 = ""
                        vDELVSCH_item4 = ""
                        vDELVSCH_item5 = ""

                        If Len(vDELVSCH_item1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(vDELVSCH_item1), I, 1) = "," Then Exit For
                            Next I
                            If I = 0 Then I = 25

                            vDELVSCH_item2 = Microsoft.VisualBasic.Right(Trim(vDELVSCH_item1), Len(vDELVSCH_item1) - I)
                            vDELVSCH_item1 = Microsoft.VisualBasic.Left(Trim(vDELVSCH_item1), I)
                        End If

                        If Len(vDELVSCH_item2) > 30 Then
                            For I = 30 To 1 Step -1
                                If Mid$(Trim(vDELVSCH_item2), I, 1) = "," Then Exit For
                            Next I
                            If I = 0 Then I = 30

                            vDELVSCH_item3 = Microsoft.VisualBasic.Right(Trim(vDELVSCH_item2), Len(vDELVSCH_item2) - I)
                            vDELVSCH_item2 = Microsoft.VisualBasic.Left(Trim(vDELVSCH_item2), I)
                        End If

                        If Len(vDELVSCH_item3) > 35 Then
                            For I = 35 To 1 Step -1
                                If Mid$(Trim(vDELVSCH_item3), I, 1) = "," Then Exit For
                            Next I
                            If I = 0 Then I = 35

                            vDELVSCH_item4 = Microsoft.VisualBasic.Right(Trim(vDELVSCH_item3), Len(vDELVSCH_item3) - I)
                            vDELVSCH_item3 = Microsoft.VisualBasic.Left(Trim(vDELVSCH_item3), I)
                        End If

                        If Len(vDELVSCH_item4) > 40 Then
                            For I = 40 To 1 Step -1
                                If Mid$(Trim(vDELVSCH_item4), I, 1) = "," Then Exit For
                            Next I
                            If I = 0 Then I = 40

                            vDELVSCH_item5 = Microsoft.VisualBasic.Right(Trim(vDELVSCH_item4), Len(vDELVSCH_item4) - I)
                            vDELVSCH_item4 = Microsoft.VisualBasic.Left(Trim(vDELVSCH_item4), I)
                        End If

                        If Len(vDELVSCH_item5) > 40 Then
                            For I = 40 To 1 Step -1
                                If Mid$(Trim(vDELVSCH_item5), I, 1) = "," Then Exit For
                            Next I
                            If I = 0 Then I = 40

                            vDELVSCH_item6 = Microsoft.VisualBasic.Right(Trim(vDELVSCH_item5), Len(vDELVSCH_item5) - I)
                            vDELVSCH_item5 = Microsoft.VisualBasic.Left(Trim(vDELVSCH_item5), I)
                        End If

                        If Len(vDELVSCH_item6) > 55 Then
                            For I = 55 To 1 Step -1
                                If Mid$(Trim(vDELVSCH_item6), I, 1) = "," Then Exit For
                            Next I
                            If I = 0 Then I = 55

                            vDELVSCH_item6 = Microsoft.VisualBasic.Right(Trim(vDELVSCH_item6), Len(vDELVSCH_item6) - I)
                            vDELVSCH_item7 = Microsoft.VisualBasic.Left(Trim(vDELVSCH_item6), I)
                        End If


                    Next

                End If


                '-------------Code By Lalith


            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        If Common_Procedures.settings.CustomerCode = "1186" Or Common_Procedures.settings.CustomerCode = "1380" Or Common_Procedures.settings.CustomerCode = "1446" Or Common_Procedures.settings.CustomerCode = "1464" Then
            Printing_Format_2_1186(e)
        ElseIf Common_Procedures.settings.CustomerCode = "1155" Then
            Printing_Format_3(e)
        ElseIf Common_Procedures.settings.CustomerCode = "1267" Then
            Printing_Format1267(e)
        Else
            Printing_Format1(e)
        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 15
            .Bottom = 40
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then
            NoofItems_PerPage = 2
        Else
            NoofItems_PerPage = 4
        End If




        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}



        ClAr(1) = Val(35) : ClAr(2) = 275 : ClAr(3) = 120 : ClAr(4) = 80 : ClAr(5) = 120
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then
            TxtHgt = 17 '17.5 '18
        Else
            TxtHgt = 18
        End If

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If
                        If Common_Procedures.settings.CustomerCode = "1234" Then
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        Else
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        End If

                        ItmNm2 = ""
                        If Len(ItmNm1) > 45 Then
                            For I = 45 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 45
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("ClothType_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GstNo As String
        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0, w2 As Single = 0
        Dim S1 As Single = 0, s2 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from ClothSales_Order_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Order_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GstNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)


        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH SALES ORDER INDENT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("PAYMENT TERMS : ", pFont).Width
        w2 = e.Graphics.MeasureString("DELIVERY TO. : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER REF.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothSales_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString.ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "THROUGH     :  " & prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT  :  " & prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AGENT           :  " & prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "DESPATCH TO   :  " & prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + 10, CurY, 0, 0, pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1421" Then  ' ------GK TEX

            If Trim(prn_HdDt.Rows(0).Item("Agent_Comm_Perc").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "COMMISSION    :  " & prn_HdDt.Rows(0).Item("Agent_Comm_Perc").ToString & "%", LMargin + 10, CurY, 0, 0, pFont)
            End If

        End If


        If Trim(prn_HdDt.Rows(0).Item("vehicle_no").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO   :  " & prn_HdDt.Rows(0).Item("vehicle_no").ToString, LMargin + 10, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "FOLD%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim C1 As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        C1 = ClAr(1) + 50
        W1 = e.Graphics.MeasureString("PAYMENT TERMS : ", pFont).Width

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Order_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY


        If Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "PAYMENT TERMS", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1234" Then
            If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Remarks", LMargin + 5, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Note").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)
            End If
        End If

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub
    Private Sub Printing_Format_3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 15
            .Bottom = 40
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

        NoofItems_PerPage = 4

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 210 : ClAr(3) = 60 : ClAr(4) = 60 : ClAr(5) = 120 : ClAr(6) = 90 : ClAr(7) = 80
        ClAr(8) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7))

        'ClAr(1) = Val(35) : ClAr(2) = 275 : ClAr(3) = 120 : ClAr(4) = 80 : ClAr(5) = 120
        'ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If
                        If Common_Procedures.settings.CustomerCode = "1234" Then
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        Else
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        End If

                        ItmNm2 = ""
                        If Len(ItmNm1) > 45 Then
                            For I = 45 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 45
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("ClothType_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        If Val(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Payment_Terms").ToString & "Days", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "IMMEDIATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 1, 0, pFont)
                        End If
                        If Val(prn_HdDt.Rows(0).Item("Delivery_Due_Days").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("delivery_Due_Days").ToString & "Days", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + +ClAr(7) - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "READY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + +ClAr(7) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0, w2 As Single = 0
        Dim S1 As Single = 0, s2 As Single = 0
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim I As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from ClothSales_Order_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Order_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH SALES ORDER INDENT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("PAYMENT TERMS : ", pFont).Width
        w2 = e.Graphics.MeasureString("DELIVERY TO. : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        'Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString)

        ItmNm2 = ""
        If Len(ItmNm1) > 30 Then
            For I = 30 To 1 Step -1
                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
            Next I
            If I = 0 Then I = 30

            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
        If Trim(ItmNm2) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & Trim(ItmNm2), LMargin + 10, CurY, 0, 0, p1Font)
        End If
        'End If

        Common_Procedures.Print_To_PrintDocument(e, "ORDER REF.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothSales_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString.ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "THROUGH     :  " & prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT  :  " & prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AGENT           :  " & prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Agent_Comm_Perc").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "COMMISSION    :  " & prn_HdDt.Rows(0).Item("Agent_Comm_Perc").ToString & "%", LMargin + 10, CurY, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DESPATCH TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        End If


        If Trim(prn_HdDt.Rows(0).Item("vehicle_no").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO   :  " & prn_HdDt.Rows(0).Item("vehicle_no").ToString, LMargin + 10, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "FOLD%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PAYMENT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim C1 As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        C1 = ClAr(1) + 50
        W1 = e.Graphics.MeasureString("PAYMENT TERMS : ", pFont).Width

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Order_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY


        'If Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "PAYMENT TERMS", LMargin + 5, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)
        'End If



        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Remarks", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Note").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            If dgv_Details.CurrentCell.ColumnIndex = dgvCol_DelvDetails.Delivery_Schedule_Selection Then
                btn_Delv_Selection_Click(sender, e)
            End If
        End If

    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_date.Text = Date.Today
        'End If
        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
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

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click
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



            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            Agnt_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
            AgPNo = ""
            If Val(Agnt_IdNo) <> 0 Then
                AgPNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_PhoneNo", "(Ledger_IdNo = " & Str(Val(Agnt_IdNo)) & ")")
            End If

            If Trim(AgPNo) <> "" Then
                PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", ",", "") & Trim(AgPNo)
            End If

            smstxt = Trim(cbo_PartyName.Text) & Chr(13)
            smstxt = smstxt & " Order Ref.No : " & Trim(lbl_RefNo.Text) & Chr(13)
            smstxt = smstxt & " Order Date : " & Trim(msk_date.Text) & Chr(13)
            If Trim(txt_OrderNo.Text) <> "" Then
                smstxt = smstxt & " Party Order No : " & Trim(txt_OrderNo.Text) & Chr(13)
            End If
            If Trim(cbo_DespTo.Text) <> "" Then
                smstxt = smstxt & " Despatch To : " & Trim(cbo_DespTo.Text) & Chr(13)
            End If

            smstxt = smstxt & "  FABRIC DETAILS : " & Chr(13)

            If dgv_Details.RowCount > 0 Then
                For i = 0 To dgv_Details.RowCount - 1
                    If Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Then
                        smstxt = smstxt & "  Cloth Name : " & Trim((dgv_Details.Rows(i).Cells(dgvCol_DelvDetails.clothname).Value)) & Chr(13)
                        smstxt = smstxt & "  Meters : " & Val(dgv_Details.Rows(i).Cells(dgvCol_DelvDetails.order_mtrs).Value) & Chr(13)
                        smstxt = smstxt & "  Rate : " & Val(dgv_Details.Rows(i).Cells(dgvCol_DelvDetails.rate).Value) & Chr(13)

                    End If
                Next i
            End If
            smstxt = smstxt & "  Payment Terms :" & Trim(txt_PaymentTerms.Text) & Chr(13)
            'If dgv_Details.RowCount > 0 Then
            '    smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details.Rows(0).Cells(4).Value())) & Chr(13)
            '    smstxt = smstxt & " Meters : " & Val((dgv_Details.Rows(0).Cells(7).Value())) & Chr(13)
            'End If
            '   smstxt = smstxt & " Bill Amount : " & Trim(lbl_Net_Amt.Text) & Chr(13)

            smstxt = smstxt & " " & Chr(13)
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

    Private Sub btn_PDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
        'Print_PDF_Status = False
    End Sub

    Private Sub Get_AgentComm()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Agnt_ID As Integer = 0
        Dim Cloth_Comm_Percentage As Single = 0
        Dim Cloth_Comm_Mtr As Single = 0

        Agnt_ID = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Agent.Text)))

        If Agnt_ID = 0 Then Exit Sub

        Try

            da = New SqlClient.SqlDataAdapter("select Cloth_Comm_Percentage ,Cloth_Comm_Meter from ledger_head  where  Ledger_IdNo = " & Str(Val(Agnt_ID)) & "", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Cloth_Comm_Percentage").ToString) = False Then
                    Cloth_Comm_Percentage = Val(dt.Rows(0).Item("Cloth_Comm_Percentage").ToString)
                End If
                If IsDBNull(dt.Rows(0).Item("Cloth_Comm_Meter").ToString) = False Then
                    Cloth_Comm_Mtr = Val(dt.Rows(0).Item("Cloth_Comm_Meter").ToString)
                End If
            End If
            dt.Clear()


            If Trim(cbo_Com_Type.Text) = "%" Then
                txt_com_per.Text = Val(Cloth_Comm_Percentage)
            Else
                txt_com_per.Text = Val(Cloth_Comm_Mtr)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub cbo_Agent_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.TextChanged
        Get_AgentComm()
    End Sub

    Private Sub GraceTime_Calculation()

        msk_GrDate.Text = ""
        If IsDate(msk_date.Text) = True And Val(txt_GrTime.Text) >= 0 Then
            msk_GrDate.Text = DateAdd("d", Val(txt_GrTime.Text), Convert.ToDateTime(msk_date.Text))
        End If
        msk_delivery_date.Text = ""
        If IsDate(msk_date.Text) = True And Val(txt_delivery_due_days.Text) >= 0 Then
            msk_delivery_date.Text = DateAdd("d", Val(txt_delivery_due_days.Text), Convert.ToDateTime(msk_date.Text))
        End If
    End Sub

    Private Sub txt_GrTime_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GrTime.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_GrTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GrTime.TextChanged
        GraceTime_Calculation()
    End Sub

    Private Sub dtp_GrDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_GrDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_GrDate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_GrDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_GrDate.TextChanged
        If IsDate(dtp_GrDate.Text) = True Then

            msk_GrDate.Text = dtp_GrDate.Text
            msk_GrDate.SelectionStart = 0
        End If
    End Sub

    Private Sub dtp_GrDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_GrDate.ValueChanged
        msk_GrDate.Text = dtp_GrDate.Text
    End Sub

    Private Sub dtp_GrDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_GrDate.Enter
        msk_GrDate.Focus()
        msk_GrDate.SelectionStart = 0
    End Sub
    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_date, cbo_PartyName, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_PartyName, "", "", "", "")



    End Sub
    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bls As Single = 0
        Dim Ent_BlNos As String = ""
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim Ent_Rate As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Cloth_Name, d.Ledger_Name as agentname, e.Ledger_Name as Transportname,  g.ClothType_name, h.Bales as Ent_Bales,  h.Order_Pcs as Ent_Pcs, h.Order_Meters as Ent_Meters,h.Rate as Ent_Rate from ClothSales_Enquiry_Head a INNER JOIN Clothsales_Enquiry_details b ON a.ClothSales_Enquiry_Code = b.ClothSales_Enquiry_Code INNER JOIN Cloth_Head c ON b.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head g ON b.ClothType_IdNo = g.ClothType_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN ClothSales_Order_Details h ON h.ClothSales_Order_Code = '" & Trim(NewCode) & "' and b.ClothSales_Enquiry_Code = h.ClothSales_Enquiry_Code and b.ClothSales_Enquiry_SlNo = h.ClothSales_Enquiry_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Enquiry_Meters - b.Enquiry_Cancel_Meters - b.Order_Meters) > 0 or h.Order_Meters > 0 ) order by a.ClothSales_Enquiry_Date, a.for_orderby, a.ClothSales_Enquiry_No", con)
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
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Bales").ToString) = False Then
                        Ent_Bls = Val(Dt1.Rows(i).Item("Ent_Bales").ToString)
                    End If
                    'If IsDBNull(Dt1.Rows(i).Item("Ent_Bales_Nos").ToString) = False Then
                    '    Ent_BlNos = Dt1.Rows(i).Item("Ent_Bales_Nos").ToString
                    'End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
                        Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Meters").ToString) = False Then
                        Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_Meters").ToString)
                    End If

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then
                        Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
                    End If

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Enquiry_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Enquiry_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("ClothType_Name").ToString
                    .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Fold_Perc").ToString)
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Bales").ToString)
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Enquiry_Pcs").ToString)
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Enquiry_Meters").ToString) - Val(Dt1.Rows(i).Item("Order_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")
                    .Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(i).Item("Rate").ToString), "#########0.00")
                    .Rows(n).Cells(10).Value = Format(Val(Dt1.Rows(i).Item("Enquiry_Cancel_Meters").ToString), "#########0.00")
                    If Ent_Mtrs > 0 Then
                        .Rows(n).Cells(11).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(11).Value = ""

                    End If

                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("agentname").ToString
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Transportname").ToString
                    .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Through_Name").ToString
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Despatch_To").ToString
                    .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Delivery_Address1").ToString
                    .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Delivery_Address2").ToString
                    .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("Clothsales_Enquiry_Code").ToString
                    .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("Clothsales_Enquiry_SlNo").ToString

                    .Rows(n).Cells(20).Value = Val(Ent_Bls)
                    .Rows(n).Cells(21).Value = Ent_Pcs
                    .Rows(n).Cells(22).Value = Ent_Mtrs
                    .Rows(n).Cells(23).Value = Ent_Rate
                    .Rows(n).Cells(24).Value = (Dt1.Rows(i).Item("Party_OrderNo").ToString)
                    .Rows(n).Cells(25).Value = (Dt1.Rows(i).Item("Gr_Time").ToString)
                    .Rows(n).Cells(26).Value = (Dt1.Rows(i).Item("Gr_Date").ToString)
                    .Rows(n).Cells(27).Value = (Dt1.Rows(i).Item("Agent_Comm_Perc").ToString)
                    .Rows(n).Cells(28).Value = (Dt1.Rows(i).Item("Agent_Comm_Type").ToString)
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


                txt_OrderNo.Text = dgv_Selection.Rows(i).Cells(24).Value
                txt_GrTime.Text = dgv_Selection.Rows(i).Cells(25).Value
                msk_GrDate.Text = dgv_Selection.Rows(i).Cells(26).Value
                txt_com_per.Text = dgv_Selection.Rows(i).Cells(27).Value
                cbo_Com_Type.Text = dgv_Selection.Rows(i).Cells(28).Value
                cbo_Agent.Text = dgv_Selection.Rows(i).Cells(12).Value
                cbo_Through.Text = dgv_Selection.Rows(i).Cells(14).Value
                cbo_DespTo.Text = dgv_Selection.Rows(i).Cells(15).Value
                cbo_Transport.Text = dgv_Selection.Rows(i).Cells(13).Value

                If txt_DelvAdd1.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(16).Value) <> "" Then
                        txt_DelvAdd1.Text = dgv_Selection.Rows(i).Cells(16).Value
                    End If
                End If

                If txt_DelvAdd2.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(17).Value) <> "" Then
                        txt_DelvAdd2.Text = dgv_Selection.Rows(i).Cells(17).Value
                    End If
                End If

                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(5).Value

                If Val(dgv_Selection.Rows(i).Cells(20).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(20).Value
                Else
                    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If


                If Val(dgv_Selection.Rows(i).Cells(21).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(21).Value
                Else
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(22).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(22).Value
                Else
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(23).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(23).Value
                Else
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(9).Value
                End If

                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(18).Value
                dgv_Details.Rows(n).Cells(14).Value = dgv_Selection.Rows(i).Cells(19).Value

            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)

        Else
            If txt_PaymentTerms.Visible And txt_PaymentTerms.Enabled Then
                txt_PaymentTerms.Focus()
            Else
                txt_Note.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        If Trim(UCase(cbo_Type.Text)) = "ENQUIRY" Then
            dgv_Details.AllowUserToAddRows = False
            txt_OrderNo.Enabled = False
            cbo_Through.Enabled = False
            cbo_Agent.Enabled = False
            txt_com_per.Enabled = False
            cbo_Com_Type.Enabled = False
            txt_GrTime.Enabled = False
            msk_GrDate.Enabled = False
            cbo_DespTo.Enabled = False
            cbo_Transport.Enabled = False
            txt_DelvAdd1.Enabled = False
            txt_DelvAdd2.Enabled = False
        Else
            dgv_Details.AllowUserToAddRows = True
            txt_OrderNo.Enabled = True
            cbo_Through.Enabled = True
            cbo_Agent.Enabled = True
            txt_com_per.Enabled = True
            cbo_Com_Type.Enabled = True
            txt_GrTime.Enabled = True
            msk_GrDate.Enabled = True
            cbo_DespTo.Enabled = True
            cbo_Transport.Enabled = True
            txt_DelvAdd1.Enabled = True
            txt_DelvAdd2.Enabled = True
        End If
    End Sub

    Private Sub txt_PaymentTerms_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PaymentTerms.KeyDown
        If e.KeyCode = 38 Then
            cbo_DeliveryTo.Focus()

        End If
        If e.KeyCode = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_DelvDetails.clothname)
                dgv_Details.CurrentCell.Selected = True

            End If
        End If
    End Sub

    Private Sub txt_PaymentTerms_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PaymentTerms.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_DelvDetails.clothname)
                dgv_Details.CurrentCell.Selected = True

            End If
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_DeliveryTo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If txt_PaymentTerms.Visible And txt_PaymentTerms.Enabled Then
                txt_PaymentTerms.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_DelvDetails.clothname)



                End If
            End If


        End If

        If (e.KeyValue = 38 And cbo_DeliveryTo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_Transport.Visible And cbo_Transport.Enabled Then
                cbo_Transport.Focus()
            Else
                msk_delivery_date.Focus()

            End If


        End If
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If txt_PaymentTerms.Visible And txt_PaymentTerms.Enabled Then
                txt_PaymentTerms.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_DelvDetails.clothname)



                End If
            End If
        End If
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

    Private Sub cbo_DeliveryTo_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbo_DeliveryTo.SelectedIndexChanged

    End Sub

    Private Sub txt_DelvAdd2_AcceptsTabChanged(sender As Object, e As EventArgs) Handles txt_DelvAdd2.AcceptsTabChanged

    End Sub

    Private Sub cbo_Grid_loomType_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_loomType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "LoomType_Head", "Loomtype_Name", "  ", "(LoomType_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_loomType_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_loomType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_loomType, Nothing, Nothing, "LoomType_Head", "Loomtype_Name", "  ", "(LoomType_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_Grid_loomType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.CurrentRow.Cells(dgvCol_DelvDetails.order_pcs)
            End If

        ElseIf (e.KeyValue = 38 And cbo_Grid_loomType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.CurrentRow.Cells(dgvCol_DelvDetails.clothname)
            End If

        End If
    End Sub

    Private Sub cbo_Grid_loomType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_loomType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_loomType, Nothing, "LoomType_Head", "Loomtype_Name", "  ", "(LoomType_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.CurrentRow.Cells(dgvCol_DelvDetails.order_pcs)
            Else
                If txt_PaymentTerms.Visible And txt_PaymentTerms.Enabled Then
                    txt_PaymentTerms.Focus()
                Else
                    txt_Note.Focus()
                End If
            End If

        End If
    End Sub

    Private Sub cbo_Grid_loomType_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_loomType.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New LoomType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_loomType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Printing_Format_1186(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Amount = 0


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 15
            .Bottom = 40
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

        NoofItems_PerPage = 6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(50) : ClAr(2) = 250 : ClAr(3) = 100 : ClAr(4) = 0 : ClAr(5) = 130 : ClAr(6) = 100
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_1186_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If
                        If Common_Procedures.settings.CustomerCode = "1234" Then
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) & " " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Weave").ToString)
                        Else
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString) & " " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Weave").ToString)
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

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("LoomType_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        ' Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Amount = Format(Val((Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString) * Val(prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString))), "#######0.00")

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(Amount), "######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter_Fabric").ToString) <> 0 Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Weight : " & prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter_Fabric").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If
                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_1186_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, City As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
        Dim Cmp_State As String, Cmp_StateCode As String, Cmp_GSTIN_No As String, Cmp_EMail As String

        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0, w2 As Single = 0
        Dim S1 As Single = 0, s2 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from ClothSales_Order_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Order_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH SALES ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) - 60
        W1 = e.Graphics.MeasureString("PAYMENT TERMS : ", pFont).Width
        w2 = e.Graphics.MeasureString("PARTY P.O NO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MAINName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothSales_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PARTY P.O NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString.ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)



        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "THROUGH     :  " & prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "DESPATCH TO   :  " & prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + 10, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AGENT           :  " & prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt


        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("vehicle_no").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO   :  " & prn_HdDt.Rows(0).Item("vehicle_no").ToString, LMargin + 10, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)



        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOOM TYPE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "FOLD%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE/MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT (Rs)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format_1186_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim C1 As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
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
        C1 = ClAr(1) + 50
        W1 = e.Graphics.MeasureString("PAYMENT TERMS : ", pFont).Width

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Order_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(Amount), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Attend").ToString) <> "" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Kind Attend :  ", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Attend").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 0, 0, p1Font)

        End If
        '& prn_HdDt.Rows(0).Item("Attend").ToString

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Dear Sir, ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "We are hereby confirming the sales of cotton greige fabrics as follows :", LMargin + ClAr(1), CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 20
        If Trim(prn_HdDt.Rows(0).Item("Discount").ToString) <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, "Cash Discount", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Discount").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)

        End If

        If Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Payment Terms", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)

        End If
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        If Trim(prn_HdDt.Rows(0).Item("GST_PERC").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("GST_PERC").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)

        End If

        If Trim(prn_HdDt.Rows(0).Item("Delivery_Schedule").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Delivery Schedule", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Delivery_Schedule").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)

        End If






        If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Transportation", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TransportName").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)

        End If

        If Trim(prn_HdDt.Rows(0).Item("Despatch_to").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Despatch To", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Despatch_to").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)

        End If

        If Trim(prn_HdDt.Rows(0).Item("freight_Ac").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Frieght", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("freight_Ac").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)

        End If

        If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Remarks", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Note").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "Terms & Condition", LMargin + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "a). Fabric should be free from Weaving defects, Slubs & Stains & of top Dyeable Quality.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "b). Piece Length: 80 % : 80 mtrs and above, 20 % : 40-79 mtrs. No Short Length Will be accepted.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "c). No Variation in Count/Reed/Pick/Width is accepted.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "d). Delivery Date should be Stictly adhered to.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "e). Please mention the PO number in the Invoice.", LMargin + 12, CurY, 0, 0, pFont)

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, p1Font)


        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub cbo_fright_ac_GotFocus(sender As Object, e As EventArgs) Handles cbo_fright_ac.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "freight_Ac", "", "")
    End Sub

    Private Sub cbo_fright_ac_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_fright_ac.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_fright_ac, txt_Disc, cbo_Transport, "ClothSales_Order_Head", "freight_Ac", "", "")

    End Sub

    Private Sub cbo_fright_ac_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_fright_ac.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_fright_ac, cbo_Transport, "ClothSales_Order_Head", "freight_Ac", "", "", False)

    End Sub

    Private Sub txt_gst_percentage_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_gst_percentage.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Disc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Disc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            cbo_fright_ac.Focus()

        End If
    End Sub

    Private Sub PrintDocument1_QueryPageSettings(sender As Object, e As QueryPageSettingsEventArgs) Handles PrintDocument1.QueryPageSettings

    End Sub

    Private Sub txt_Disc_TextChanged(sender As Object, e As EventArgs) Handles txt_Disc.TextChanged
        If (dgv_Details_Total.Rows(0).Cells(8).Value) <> 0 Then
            lbl_DiscAmount.Text = Format(Val(dgv_Details_Total.Rows(0).Cells(8).Value) * Val(txt_Disc.Text) / 100, "########0.00")

        End If

    End Sub

    Private Sub cbo_taxType_GotFocus(sender As Object, e As EventArgs) Handles cbo_taxType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "tax_type", "", "(ClothSales_Order_no=0)")
    End Sub

    Private Sub cbo_taxType_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_taxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_taxType, msk_date, cbo_PartyName, "ClothSales_Order_Head", "tax_type", "", "(ClothSales_Order_no=0)")

    End Sub

    Private Sub cbo_taxType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_taxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_taxType, cbo_PartyName, "ClothSales_Order_Head", "tax_type", "", "(ClothSales_Order_no=0)", False)

    End Sub


    Private Sub txt_delivery_due_days_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_delivery_due_days.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_delivery_due_days_TextChanged(sender As Object, e As EventArgs) Handles txt_delivery_due_days.TextChanged
        GraceTime_Calculation()

    End Sub

    Private Sub dtp_delivery_date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_delivery_date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_delivery_date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_delivery_date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_delivery_date.TextChanged
        If IsDate(dtp_delivery_date.Text) = True Then

            msk_delivery_date.Text = dtp_delivery_date.Text
            msk_delivery_date.SelectionStart = 0
        End If
    End Sub

    Private Sub dtp_delivery_date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_delivery_date.ValueChanged
        msk_delivery_date.Text = dtp_delivery_date.Text
    End Sub

    Private Sub dtp_delivery_date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_delivery_date.Enter
        msk_delivery_date.Focus()
        msk_delivery_date.SelectionStart = 0
    End Sub

    Private Sub Printing_Format_2_1186(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim amount As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1

            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 15
            .Bottom = 40
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

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 320 : ClAr(3) = 80 : ClAr(4) = 0 : ClAr(5) = 100 : ClAr(6) = 80
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 17 '17.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_2_1186_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format_2_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = ""
                        ItmNm2 = ""
                        If Common_Procedures.settings.CustomerCode = "1186" Or Common_Procedures.settings.CustomerCode = "1464" Then
                            ItmNm1 = "Const.      " & ":  " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("ClothMain_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 50 Then
                                For I = 50 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 50
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If
                        End If


                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)

                        If Common_Procedures.settings.CustomerCode = "1186" Or Common_Procedures.settings.CustomerCode = "1464" Then
                            Common_Procedures.Print_To_PrintDocument(e, "Sort No    " & ":  " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sort_No").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("ClothMain_Name").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        If Trim(UCase(prn_DetDt.Rows(0)("Stock_In").ToString)) = Trim(UCase("PCS")) Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Order_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                        amount = Val(prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString) * Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate"))
                        Common_Procedures.Print_To_PrintDocument(e, Val(amount), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1


                        If Trim(ItmNm1) <> "" Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                        End If


                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Weave").ToString) <> "" Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Weave     " & ":  " & prn_DetDt.Rows(prn_DetIndx).Item("weave").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("loomType_name").ToString) <> "" Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Quality    " & ":  " & prn_DetDt.Rows(prn_DetIndx).Item("loomType_name").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        If Common_Procedures.settings.CustomerCode = "1186" Or Common_Procedures.settings.CustomerCode = "1464" Then
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter_Fabric").ToString) <> 0 Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Weight     " & ": " & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter_Fabric").ToString), "#####0.0000") & " Kgs", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format_2_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_2_1186_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_Add3 As String, city As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String

        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0, w2 As Single = 0
        Dim S1 As Single = 0, s2 As Single = 0
        Dim strHeight1 As Single
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from ClothPurchase_Order_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothPurchase_Order_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY


        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""


        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString


        'Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
        'Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString

        If InStr(1, Trim(UCase(Cmp_Name)), "UNITED") > 0 And InStr(1, Trim(UCase(Cmp_Name)), "WEAVES") > 0 Then
            Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Else
            Cmp_Add1 = "" & prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = "" & prn_HdDt.Rows(0).Item("Company_Address2").ToString

        End If

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
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
            If InStr(1, Trim(UCase(Cmp_Name)), "UNITED") > 0 And InStr(1, Trim(UCase(Cmp_Name)), "WEAVES") > 0 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)
            End If
        End If



        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & "," & city, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        ' Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "  / " & Cmp_CstNo, LMargin + 10, CurY, 0, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "/ " & Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "FABRIC SALES ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        C1 = ClAr(1) + ClAr(2) + ClAr(3) + 50
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(15) = CurY

        CurY = CurY + 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        If Common_Procedures.settings.CustomerCode = "1186" Or Common_Procedures.settings.CustomerCode = "1464" Then
            Common_Procedures.Print_To_PrintDocument(e, "SO No. :  GF/SO-" & prn_HdDt.Rows(0).Item("ClothSales_Order_No").ToString & "/" & Common_Procedures.FnYearCode, LMargin + 10, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "SO No. :  " & prn_HdDt.Rows(0).Item("ClothSales_Order_No").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        End If

        'strHeight1 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("ClothPurchase_Order_No").ToString, p1Font).Height

        Common_Procedures.Print_To_PrintDocument(e, "SO Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Order_date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1) + (ClAr(2) / 2) + 20, CurY, 2, 0, p1Font)
        'strHeight1 = e.Graphics.MeasureString(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothPurchase_Order_Date").ToString), "dd-MM-yyyy").ToString, p1Font).Height

        Common_Procedures.Print_To_PrintDocument(e, "Party PO No. : " & prn_HdDt.Rows(0).Item("Party_OrderNo").ToString.ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 50, LnAr(15))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + (ClAr(2) / 2) + 10, CurY, LMargin + ClAr(1) + (ClAr(2) / 2) + 10, LnAr(15))
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + (ClAr(3) / 2)
        W1 = e.Graphics.MeasureString("PARTY P.O NO: ", pFont).Width
        w2 = e.Graphics.MeasureString("Desp TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Italic Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY AT : ", LMargin + C1 + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 5

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_MAINName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        End If

        If prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString <> "" Then


            Common_Procedures.Print_To_PrintDocument(e, "GSTIN  : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)

        End If
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "THROUGH     :  " & prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, " AGENT NAME  : Mr ." & Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Agent_IdNo").ToString)), LMargin + C1 + 20, CurY, 0, 0, pFont)

        'Delivery_GSTinNo
        CurY = CurY + TxtHgt + 5

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        CurY = CurY + 10
        If Trim(prn_HdDt.Rows(0).Item("Attend").ToString) <> "" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Kind Attn . :  ", LMargin + ClAr(1) + ClAr(2) - 80, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Mr." & Trim(prn_HdDt.Rows(0).Item("Attend").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, p1Font)

        End If
        CurY = CurY + TxtHgt + 20

        Common_Procedures.Print_To_PrintDocument(e, "Dear Sir, ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "We are hereby confirming the sales of cotton greige fabrics as follows :", LMargin + ClAr(1), CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 20

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        If Trim(UCase(prn_DetDt.Rows(0)("Stock_In").ToString)) = Trim(UCase("PCS")) Then
            Common_Procedures.Print_To_PrintDocument(e, "ORDER PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/PC", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        Else
            Common_Procedures.Print_To_PrintDocument(e, "ORDER MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        End If

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL AMOUNT (Rs)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

        CurY = CurY + 15

        Common_Procedures.Print_To_PrintDocument(e, "Excl.Gst", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format_2_1186_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
        Dim BmsInWrds As String
        Dim ItmNm1 As String = "", ItmNm2 As String = "", ItmNm3 As String = "", ItmNm4 As String = "", ItmNm5 As String = "", ItmNm6 As String = "", ItmNm7 As String = "", ItmNm8 As String = "", ItmNm9 As String = "", ItmNm10 As String = ""
        Dim DelvAddAr() As String
        Dim DelInc As Integer = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            If Trim(UCase(prn_DetDt.Rows(0)("Stock_In").ToString)) = Trim(UCase("PCS")) Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Order_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10
        W1 = e.Graphics.MeasureString(" Delivery Schedule    : ", pFont).Width


        CurY = CurY + TxtHgt + 10

        '& prn_HdDt.Rows(0).Item("Attend").ToString


        p1Font = New Font("Calibri", 10, FontStyle.Bold)

        ' Common_Procedures.Print_To_PrintDocument(e, " Payment" & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + 10, CurY, 0, 0, p1Font)

        If Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " Cash Discount", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Trim(prn_HdDt.Rows(0).Item("Discount").ToString) & " %", LMargin + W1 + 10, CurY, 0, 0, pFont)

        End If
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, " Tax Type ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " : " & Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString), LMargin + W1 + 10, CurY, 0, 0, pFont)


        'AddLessAfterTax_Text
        'If Val(prn_HdDt.Rows(0).Item("Gr_time").ToString) <> 0 Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, " Payment Days", LMargin + 10, CurY, 0, 0, p1Font)
        '    Common_Procedures.Print_To_PrintDocument(e, " : " & Format(Val(prn_HdDt.Rows(0).Item("Gr_time").ToString), "#####0") & "  Days  " & " / " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + W1 + 10, CurY, 0, 0, pFont)
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, " Delivery Schedule", LMargin + 10, CurY, 0, 0, p1Font)
        '    Common_Procedures.Print_To_PrintDocument(e, " : Before " & Trim(prn_HdDt.Rows(0).Item("Delivery_Due_Days").ToString) & "   Days " & " / " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Delivery_Due_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)
        'End If


        If Val(prn_HdDt.Rows(0).Item("Gr_time").ToString) <> 0 Then

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Payment Days", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Format(Val(prn_HdDt.Rows(0).Item("Gr_time").ToString), "#####0") & "  Days  " & " / " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Gr_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)

        Else
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Payment Days", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " : " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Gr_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : AGAINST RTGS \ PROFORMA", LMargin + W1 + 10, CurY, 0, 0, pFont)
        End If



        If Val(prn_HdDt.Rows(0).Item("Delivery_Due_Days").ToString) <> 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Delivery Schedule", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " : Before  " & Trim(prn_HdDt.Rows(0).Item("Delivery_Due_Days").ToString) & "   Days " & " / " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Delivery_Due_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)
        Else
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Delivery Schedule", LMargin + 10, CurY, 0, 0, p1Font)
            ' Common_Procedures.Print_To_PrintDocument(e, " : " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Delivery_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :IMMEDIATE ", LMargin + W1 + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, " Transportation", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " : " & Trim(prn_HdDt.Rows(0).Item("freight_Ac").ToString), LMargin + W1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then



            Common_Procedures.Print_To_PrintDocument(e, " Remarks", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Trim(prn_HdDt.Rows(0).Item("Note").ToString), LMargin + W1 + 10, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt + 20
        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "a). Fabric should be free from Weaving defects, Slubs & Stains & of top Dyeable Quality.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "b). Piece Length: 80 % : 150 mtrs and above, 20 % : 40-79 mtrs. No Short Length Will be accepted.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "c). No Variation in Count/Reed/Pick/Width is accepted.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "d). Delivery Date should be Stictly adhered to.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "e). Kindly mention the PO number in the Invoice.", LMargin + 12, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 20
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt


        CurY = CurY + TxtHgt


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1186" Then
            CurY = CurY + TxtHgt + 10
        End If



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" And Print_PDF_Status = True Then

            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.UNITED_WEAVES_SIGN, Drawing.Image), LMargin + 10, CurY, 90, 55)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt

        End If

        'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 350, CurY, 0, 0, pFont)



        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)



    End Sub



    Private Sub txt_Disc_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Disc.KeyDown
        If e.KeyCode = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_DelvDetails.clothname)
                dgv_Details.CurrentCell.Selected = True

            End If
        End If
        If e.KeyCode = 40 Then
            cbo_fright_ac.Focus()

        End If
    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_Details.TextChanged
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



    Private Sub Dtp_Final_Delv_Date_ValueChanged(sender As Object, e As EventArgs) Handles Dtp_Final_Delv_Date.ValueChanged
        Msk_Final_Delv_Date.Text = Dtp_Final_Delv_Date.Text
    End Sub

    Private Sub Dtp_Final_Delv_Date_TextChanged(sender As Object, e As EventArgs) Handles Dtp_Final_Delv_Date.TextChanged
        If IsDate(Dtp_Final_Delv_Date.Text) = True Then

            Msk_Final_Delv_Date.Text = Dtp_Final_Delv_Date.Text
            Msk_Final_Delv_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub Dtp_Final_Delv_Date_KeyUp(sender As Object, e As KeyEventArgs) Handles Dtp_Final_Delv_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dtp_Final_Delv_Date.Text = Date.Today
        End If
    End Sub

    Private Sub Dtp_Final_Delv_Date_Enter(sender As Object, e As EventArgs) Handles Dtp_Final_Delv_Date.Enter
        Msk_Final_Delv_Date.Focus()
        Msk_Final_Delv_Date.SelectionStart = 0
    End Sub

    Private Sub Msk_Final_Delv_Date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Msk_Final_Delv_Date.KeyPress

        If Trim(UCase(e.KeyChar)) = "D" Then
            Msk_Final_Delv_Date.Text = Date.Today
            Msk_Final_Delv_Date.SelectionStart = 0
        End If

    End Sub

    Private Sub Msk_Final_Delv_Date_KeyUp(sender As Object, e As KeyEventArgs) Handles Msk_Final_Delv_Date.KeyUp

        If IsDate(Msk_Final_Delv_Date.Text) = True Then
            If e.KeyCode = 107 Then
                Msk_Final_Delv_Date.Text = DateAdd("D", 1, Convert.ToDateTime(Msk_Final_Delv_Date.Text))
            ElseIf e.KeyCode = 109 Then
                Msk_Final_Delv_Date.Text = DateAdd("D", -1, Convert.ToDateTime(Msk_Final_Delv_Date.Text))
            End If
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub Msk_Final_Delv_Date_KeyDown(sender As Object, e As KeyEventArgs) Handles Msk_Final_Delv_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = Msk_Final_Delv_Date.Text
            vmskSelStrt = Msk_Final_Delv_Date.SelectionStart
        End If
    End Sub

    Private Sub Msk_Final_Delv_Date_LostFocus(sender As Object, e As EventArgs) Handles Msk_Final_Delv_Date.LostFocus
        If IsDate(Msk_Final_Delv_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(Msk_Final_Delv_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(Msk_Final_Delv_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(Msk_Final_Delv_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(Msk_Final_Delv_Date.Text)) >= 2000 Then
                    Dtp_Final_Delv_Date.Value = Convert.ToDateTime(Msk_Final_Delv_Date.Text)
                End If
            End If

        End If
    End Sub



    Private Sub txt_piece_Meters_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_piece_Meters.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_bale_meters.Focus()
        End If
    End Sub

    Private Sub txt_piece_Meters_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_piece_Meters.KeyDown
        If e.KeyCode = 40 Then
            txt_bale_meters.Focus()
        End If

        If e.KeyCode = 38 Then
            Msk_Final_Delv_Date.Focus()
        End If
    End Sub

    Private Sub txt_bale_meters_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_bale_meters.KeyPress


        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If

    End Sub

    Private Sub txt_bale_meters_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_bale_meters.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If

        If e.KeyCode = 38 Then
            txt_piece_Meters.Focus()
        End If
    End Sub



    Private Sub btn_Delv_Selection_Click(sender As Object, e As EventArgs) Handles btn_Delv_Selection.Click
        Dim Sno As Integer = 0

        With Dgv_Delivery_Schedule_Details_Input

            .Rows.Clear()

            For i As Integer = 0 To Dgv_Delivery_Schedule_Details_Hidden.RowCount - 1

                If Val(Dgv_Delivery_Schedule_Details_Hidden.Rows(i).Cells(2).Value) <> 0 And Val(Dgv_Delivery_Schedule_Details_Hidden.Rows(i).Cells(1).Value) = Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(18).Value) Then

                    Dim n = .Rows.Add()

                    Sno = Sno + 1

                    .Rows(n).Cells(0).Value = Val(Sno)
                    .Rows(n).Cells(1).Value = Dgv_Delivery_Schedule_Details_Hidden.Rows(i).Cells(2).Value
                    .Rows(n).Cells(2).Value = Dgv_Delivery_Schedule_Details_Hidden.Rows(i).Cells(3).Value

                End If

            Next

        End With


        Pnl_Delv_Selection.Visible = True
        ActiveControl.Name = Dgv_Delivery_Schedule_Details_Input.Name
        Pnl_Delv_Selection.Focus()
        Pnl_Delv_Selection.BringToFront()
        pnl_Back.Enabled = False


        If Dgv_Delivery_Schedule_Details_Input.Rows.Count > 0 Then
            Dgv_Delivery_Schedule_Details_Input.CurrentCell = Dgv_Delivery_Schedule_Details_Input.Rows(0).Cells(1)
        Else
            Dgv_Delivery_Schedule_Details_Input.Rows.Add()
            Dgv_Delivery_Schedule_Details_Input.CurrentCell = Dgv_Delivery_Schedule_Details_Input.Rows(0).Cells(1)
        End If

        Dgv_Delivery_Schedule_Details_Input.Focus()
        Dgv_Delivery_Schedule_Details_Input.CurrentCell.Selected = True


    End Sub


    Private Sub Set_Max_DetailsSlNo(ByVal RowNo As Integer, ByVal DetSlNo_ColNo As Integer)
        Dim MaxSlNo As Integer = 0
        Dim i As Integer

        With dgv_Details
            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(18).Value) > Val(MaxSlNo) Then
                    MaxSlNo = Val(.Rows(i).Cells(18).Value)
                End If
            Next
            .Rows(RowNo).Cells(18).Value = Val(MaxSlNo) + 1
        End With

    End Sub

    Private Sub btn_Close_Delv_Selection_Click(sender As Object, e As EventArgs) Handles btn_Close_Delv_Selection.Click
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim dgvDet_CurRow As Integer = 0
        Dim dgv_DetSlNo As Integer = 0
        Dim dgvselect_DetSlNo As Integer = 0
        Dim NewCode As String = ""

        Dim sno = 0

        For i As Integer = 0 To Dgv_Delivery_Schedule_Details_Input.RowCount - 1

            If Trim(Dgv_Delivery_Schedule_Details_Input.Rows(i).Cells(1).Value) <> "" Then

                If IsDate(Dgv_Delivery_Schedule_Details_Input.Rows(i).Cells(1).Value) = False Then

                    MessageBox.Show("Invaild Delivery Schedule Date", "DOES NOT ADD", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    Dgv_Delivery_Schedule_Details_Input.Focus()
                    Dgv_Delivery_Schedule_Details_Input.CurrentCell = Dgv_Delivery_Schedule_Details_Input.Rows(i).Cells(1)

                    Exit Sub

                End If
            End If
        Next


        dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
        dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(18).Value)


LOOP1:

        With Dgv_Delivery_Schedule_Details_Hidden

            For i As Integer = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(1).Value) = Val(dgv_DetSlNo) Then

                    .Rows.RemoveAt(i)

                    GoTo LOOP1

                End If

            Next i



            For I As Integer = 0 To Dgv_Delivery_Schedule_Details_Input.RowCount - 1

                If Trim(Dgv_Delivery_Schedule_Details_Input.Rows(I).Cells(1).Value) <> "" Or Val(Dgv_Delivery_Schedule_Details_Input.Rows(I).Cells(2).Value) <> 0 Then

                    Dim n = .Rows.Add()




                    .Rows(n).Cells(0).Value = 0
                    .Rows(n).Cells(1).Value = dgv_DetSlNo
                    .Rows(n).Cells(2).Value = Trim(Dgv_Delivery_Schedule_Details_Input.Rows(I).Cells(1).Value)
                    .Rows(n).Cells(3).Value = Dgv_Delivery_Schedule_Details_Input.Rows(I).Cells(2).Value
                    .Rows(n).Cells(4).Value = ""

                End If

            Next
        End With


        Pnl_Delv_Selection.Visible = False
        pnl_Back.Enabled = True
        dgv_Details.Enabled = True


        With dgv_Details

            .Focus()
            If .CurrentCell.RowIndex + 1 <= .RowCount - 1 Then
                .CurrentCell = dgv_Details.Rows(.CurrentCell.RowIndex + 1).Cells(1)
            End If
            .CurrentCell.Selected = True

        End With


    End Sub

    Private Sub Dgv_Delivery_Schedule_Details_Input_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles Dgv_Delivery_Schedule_Details_Input.EditingControlShowing
        dgtxt_Delivery_Schedule_Input = CType(Dgv_Delivery_Schedule_Details_Input.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Delivery_Schedule_Input_Enter(sender As Object, e As EventArgs) Handles dgtxt_Delivery_Schedule_Input.Enter
        Dgv_Delivery_Schedule_Details_Input.EditingControl.BackColor = Color.Lime
        Dgv_Delivery_Schedule_Details_Input.EditingControl.ForeColor = Color.Blue
        dgtxt_Delivery_Schedule_Input.SelectAll()
    End Sub

    Private Sub Dgv_Delivery_Schedule_Details_Input_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv_Delivery_Schedule_Details_Input.CellEnter
        With Dgv_Delivery_Schedule_Details_Input
            dgv_ActCtrlName = .Name
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
        End With
    End Sub

    Private Sub dgtxt_Delivery_Schedule_Input_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_Delivery_Schedule_Input.TextChanged
        With Dgv_Delivery_Schedule_Details_Input
            If .Rows.Count <> 0 Then
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Delivery_Schedule_Input.Text)
            End If
        End With
    End Sub

    Private Sub dgv_Delv_Selection_Details_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles Dgv_Delivery_Schedule_Details_Hidden.RowsAdded

        Dim n As Integer
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With Dgv_Delivery_Schedule_Details_Hidden
            n = .RowCount
            .Rows(n - 1).Cells(dgvCol_DelvDetails.SNo).Value = Val(n)
        End With

    End Sub


    Private Sub dgv_Selection_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Selection.CellEnter
        dgv_ActCtrlName = dgv_Selection.Name
    End Sub


    Private Sub dgv_Filter_Details_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellEnter
        dgv_ActCtrlName = dgv_Filter_Details.Name
    End Sub

    Private Sub btn_Add_PartyPO_Image_Click(sender As Object, e As EventArgs) Handles btn_Add_PartyPO_Image.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pic_PartyPO_Image.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub btn_Delete_PartyPO_Image_Click(sender As Object, e As EventArgs) Handles btn_Delete_PartyPO_Image.Click
        pic_PartyPO_Image.BackgroundImage = Nothing
    End Sub

    Private Sub btn_Enlarge_PartyPO_Image_Click(sender As Object, e As EventArgs) Handles btn_Enlarge_PartyPO_Image.Click
        Dim f As New Enlarge_Image(pic_PartyPO_Image.BackgroundImage)
        f.MdiParent = MDIParent1
        f.Show()
    End Sub

    Private Sub btn_Show_PartyPO_Image_Click(sender As Object, e As EventArgs) Handles btn_Show_PartyPO_Image.Click
        pnl_PartyPO_Image.Visible = True
        pnl_Back.Enabled = False
        btn_Add_PartyPO_Image.Focus()
    End Sub

    Private Sub btn_Close_PartyPO_Image_Click(sender As Object, e As EventArgs) Handles btn_Close_PartyPO_Image.Click
        pnl_Back.Enabled = True
        pnl_PartyPO_Image.Visible = False
    End Sub

    Private Sub Msk_Final_Delv_Date_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles Msk_Final_Delv_Date.MaskInputRejected

    End Sub



    Private Sub cbo_fright_ac_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_fright_ac.SelectedIndexChanged

    End Sub

    Private Sub cbo_Transport_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Transport.SelectedIndexChanged

    End Sub

    Private Sub txt_Note_TextChanged(sender As Object, e As EventArgs) Handles txt_Note.TextChanged

    End Sub


    Private Sub Printing_Format1267(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 15
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
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then
            NoofItems_PerPage = 2
        Else
            NoofItems_PerPage = 26
        End If




        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}



        ClAr(1) = Val(35) : ClAr(2) = 275 : ClAr(3) = 120 : ClAr(4) = 80 : ClAr(5) = 120
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then
            TxtHgt = 17 '17.5 '18
        Else
            TxtHgt = 18
        End If

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1267_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1267_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If
                        If Common_Procedures.settings.CustomerCode = "1234" Then
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        Else
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        End If

                        ItmNm2 = ""
                        If Len(ItmNm1) > 45 Then
                            For I = 45 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 45
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("ClothType_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1267_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1267_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GstNo As String
        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0, w2 As Single = 0
        Dim S1 As Single = 0, s2 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from ClothSales_Order_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Order_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GstNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)


        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH SALES ORDER INDENT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("PAYMENT TERMS : ", pFont).Width
        w2 = e.Graphics.MeasureString("DELIVERY TO. : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER REF.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothSales_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString.ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "Final Delv Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Final_Delivery_date").ToString)), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        If Val(prn_HdDt.Rows(0).Item("Piece_Meters").ToString.ToString) <> 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Piece Meter", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Piece_Meters").ToString.ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If

        If Trim(vDELVSCH_item1) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Delivery Schedule", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(vDELVSCH_item1), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If

        If Trim(vDELVSCH_item2) <> "" Or Trim(vDELVSCH_item3) <> "" Or Trim(vDELVSCH_item4) <> "" Or Trim(vDELVSCH_item5) <> "" Or Trim(vDELVSCH_item6) <> "" Or Trim(vDELVSCH_item7) <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, Trim(vDELVSCH_item2) & Trim(vDELVSCH_item3), LMargin + C1 + 10, CurY + TxtHgt, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(vDELVSCH_item4) & Trim(vDELVSCH_item5), LMargin + C1 + 10, CurY + TxtHgt + TxtHgt, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(vDELVSCH_item6) & Trim(vDELVSCH_item7), LMargin + C1 + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, pFont)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "THROUGH     :  " & prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT  :  " & prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AGENT           :  " & prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "DESPATCH TO   :  " & prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + 10, CurY, 0, 0, pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1421" Then  ' ------GK TEX

            If Trim(prn_HdDt.Rows(0).Item("Agent_Comm_Perc").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "COMMISSION    :  " & prn_HdDt.Rows(0).Item("Agent_Comm_Perc").ToString & "%", LMargin + 10, CurY, 0, 0, pFont)
            End If

        End If


        If Trim(prn_HdDt.Rows(0).Item("vehicle_no").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO   :  " & prn_HdDt.Rows(0).Item("vehicle_no").ToString, LMargin + 10, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "FOLD%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format1267_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim C1 As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        C1 = ClAr(1) + 50
        W1 = e.Graphics.MeasureString("PAYMENT TERMS : ", pFont).Width

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Order_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY


        If Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "PAYMENT TERMS", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1234" Then
            If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Remarks", LMargin + 5, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Note").ToString), LMargin + W1 + 20, CurY, 0, 0, pFont)
            End If
        End If

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub


End Class