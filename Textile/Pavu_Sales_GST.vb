Imports System.IO
Imports System.Drawing.Printing
Public Class Pavu_Sales_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GPVSA-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private Print_PDF_Status As Boolean = False
    Private EMAIL_Status As Boolean = False
    Private vEMAIL_Attachment_FileName As String

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 20) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_TotCopies As Integer = 0
    Private prn_total_mtr As Integer = 0
    Private prn_Count As Integer = 0
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Public CHk_Details_Cnt As Integer = 0
    Private prn_InpOpts As String = ""
    Private prn_HeadIndx As Integer
    Private prn_OriDupTri As String = ""

    Public Sub New()

        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()

    End Sub

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        chk_SelectAll.Checked = False
        Print_PDF_Status = False
        EMAIL_Status = False
        vEMAIL_Attachment_FileName = ""

        chk_Verified_Status.Checked = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_InvNo.Text = ""
        lbl_InvNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        txt_KuraiPavuBeam.Text = ""
        txt_KuraiPavuMeter.Text = ""
        txt_Freight.Text = ""
        txt_RateMeters.Text = ""
        txt_AddLess.Text = ""
        lbl_GrossAmount.Text = ""
        cbo_BillTo.Text = ""
        txt_DateAndTimeOFSupply.Text = ""
        cbo_EndsCount.Text = ""
        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        cbo_RecForm.Text = Common_Procedures.Ledger_IdNoToName(con, Common_Procedures.CommonLedger.Godown_Ac)
        cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Common_Procedures.CommonLedger.Sales_Ac)
        txt_SGST_Perc.Text = ""
        txt_IGST_Perc.Text = ""
        txt_CGST_Perc.Text = ""
        lbl_CGstAmount.Text = ""
        lbl_SGstAmount.Text = ""
        txt_Pavu_PrefixNo.Text = ""
        lbl_IGstAmount.Text = ""
        lbl_NetAmount.Text = ""
        cbo_description.Text = ""

        txt_Roundoff.Text = ""

        cbo_EntryType.Text = "DIRECT"
        cbo_Grid_EndsCount.Text = ""
        cbo_ShippedTo.Text = ""

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_PavuDetails.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        txt_DcNo.Text = ""
        txt_DcDate.Text = ""

        '------------------------


        pic_IRN_QRCode_Image.BackgroundImage = Nothing
        txt_eInvoiceNo.Text = ""
        txt_eInvoiceAckNo.Text = ""
        txt_eInvoiceAckDate.Text = ""
        txt_eInvoice_CancelStatus.Text = ""

        txt_EInvoiceCancellationReson.Text = ""
        txt_eInvoiceAckNo.Text = ""
        txt_eInvoiceAckDate.Text = ""
        txt_EInvoiceCancellationReson.Text = ""
        txt_eInvoice_CancelStatus.Text = ""

        Grp_EWB.Visible = False
        txt_eWayBill_No.Text = ""
        txt_EWB_Date.Text = ""
        txt_EWB_ValidUpto.Text = ""
        txt_EWB_Cancel_Status.Text = ""
        txt_EWB_Canellation_Reason.Text = ""

        txt_eWayBill_No.Enabled = True
        rtbeInvoiceResponse.Text = ""

        grp_EInvoice.Visible = False
        '--------------------

        txt_CGST_Perc.Enabled = False
        txt_SGST_Perc.Enabled = False
        txt_IGST_Perc.Enabled = False

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            'dtp_Filter_Fromdate.Text = ""
            'dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_EndsCountName.Text = ""

            dgv_Filter_Details.Rows.Clear()
        End If

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


        If Me.ActiveControl.Name <> dgv_PavuDetails_Total.Name Then
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
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_PavuDetails_Total.CurrentCell) Then dgv_PavuDetails_Total.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_PavuDetails_Total.CurrentCell) Then dgv_PavuDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Weaver_Pavu_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BillTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BillTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RecForm.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_RecForm.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ShippedTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ShippedTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)

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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Weaver_Pavu_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Weaver_Pavu_Delivery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim I As Integer


        Me.Text = ""

        'If Common_Procedures.settings.CustomerCode = "1005" Then
        '    txt_DateAndTimeOFSupply.Visible = True
        '    lbl_DateAndTimeOFSupply_Cap.Visible = True
        '    txt_DateAndTimeOFSupply.BackColor = Color.White
        '    lbl_ShippedTo_Caption.Visible = False
        '    cbo_ShippedTo.Visible = False

        'Else
        '    txt_DateAndTimeOFSupply.Visible = False
        '    lbl_DateAndTimeOFSupply_Cap.Visible = False
        '    cbo_BillTo.Width = cbo_EndsCount.Width

        'End If
        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type <> '') order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_BillTo.DataSource = dt1
        cbo_BillTo.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'TRANSPORT' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Transport.DataSource = dt2
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
        da.Fill(dt3)
        cbo_EndsCount.DataSource = dt3
        cbo_EndsCount.DisplayMember = "EndsCount_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where  Ledger_Type = 'GODOWN'  order by Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_RecForm.DataSource = dt4
        cbo_RecForm.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Vehicle_No from Pavu_Sales_Head order by Vehicle_No", con)
        da.Fill(dt5)
        cbo_VehicleNo.DataSource = dt5
        cbo_VehicleNo.DisplayMember = "Vehicle_No"

        da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
        da.Fill(dt6)
        cbo_Grid_EndsCount.DataSource = dt6
        cbo_Grid_EndsCount.DisplayMember = "EndsCount_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type <> '') order by Ledger_DisplayName", con)
        da.Fill(dt7)
        cbo_ShippedTo.DataSource = dt7
        cbo_ShippedTo.DisplayMember = "Ledger_DisplayName"
        cbo_ShippedTo.SelectedIndex = -1

        'lbl_ShippedTo_Caption.Location = New Point(411, 34)
        'cbo_ShippedTo.Location = New Point(495, 30)
        dtp_Date.Text = ""
        msk_date.Text = ""
        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2


        cbo_EntryType.Items.Clear()
        cbo_EntryType.Items.Add("")
        cbo_EntryType.Items.Add("DIRECT")
        cbo_EntryType.Items.Add("FROM STOCK")

        dgv_PavuDetails.Columns(14).Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1123" Then
            dgv_PavuDetails.Columns(14).Visible = True

            dgv_PavuDetails.Columns(0).Width = dgv_PavuDetails.Columns(0).Width - 10
            dgv_PavuDetails.Columns(3).Width = dgv_PavuDetails.Columns(3).Width - 10
            dgv_PavuDetails.Columns(5).Width = dgv_PavuDetails.Columns(5).Width - 15
            dgv_PavuDetails.Columns(6).Width = dgv_PavuDetails.Columns(6).Width - 15
            dgv_PavuDetails.Columns(12).Width = dgv_PavuDetails.Columns(12).Width - 15

            For I = 0 To dgv_PavuDetails.Columns.Count - 1
                dgv_PavuDetails_Total.Columns(I).Width = dgv_PavuDetails.Columns(I).Width
            Next

        End If


        chk_Verified_Status.Visible = False
        If Trim(Common_Procedures.settings.CustomerCode) = "1249" Or Trim(Common_Procedures.settings.CustomerCode) = "1116" Then
            If Val(Common_Procedures.User.IdNo) <> 1 And Common_Procedures.UR.Ledger_Verifition = "" Then chk_Verified_Status.Visible = True
        End If

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BillTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RecForm.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pavu_PrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_KuraiPavuBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_KuraiPavuMeter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SetNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RateMeters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SGST_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CGST_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_IGST_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EntryType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateAndTimeOFSupply.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsCountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_EndsCount.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_description.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EWay_billNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_EWay_billNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_description.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_DateAndTimeOFSupply.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BillTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RecForm.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pavu_PrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuMeter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SGST_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CGST_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_IGST_Perc.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_BeamNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SetNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RateMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EntryType.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsCountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_KuraiPavuBeam.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler txt_KuraiPavuMeter.KeyDown, AddressOf TextBoxControlKeyDown

        '  AddHandler txt_RateMeters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EWay_billNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DcDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SGST_Perc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CGST_Perc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pavu_PrefixNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_DateAndTimeOFSupply.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_KuraiPavuBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_KuraiPavuMeter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DateAndTimeOFSupply.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RateMeters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EWay_billNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DcDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CGST_Perc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SGST_Perc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pavu_PrefixNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler cbo_ShippedTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ShippedTo.LostFocus, AddressOf ControlLostFocus


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Weaver_Pavu_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub



                ElseIf Grp_EWB.Visible = True Then
                    btn_Close_EWB_Click(sender, e)
                    Exit Sub

                ElseIf grp_EInvoice.Visible = True Then
                    btn_Close_eInvoice_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES Not CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES Not CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim tot_Amt As Single = 0

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("Select a.*, b.Ledger_Name As DelvName, c.Ledger_Name As TransportName, d.EndsCount_Name, e.Ledger_Name As RecFromName, a.PavuSale_PrefixNo from Pavu_Sales_Head a INNER JOIN Ledger_Head b On a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c On a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d On a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e On a.ReceivedFrom_IdNo = e.Ledger_IdNO Where a.Pavu_Sales_Code = '" & Trim(NewCode) & "'  and Entry_VAT_GST_Type = 'GST'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_InvNo.Text = dt1.Rows(0).Item("Pavu_Sales_No").ToString
                txt_Pavu_PrefixNo.Text = dt1.Rows(0).Item("PavuSale_PrefixNo").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Pavu_Sales_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_BillTo.Text = dt1.Rows(0).Item("DelvName").ToString
                txt_KuraiPavuBeam.Text = Val(dt1.Rows(0).Item("Empty_Beam").ToString)
                txt_KuraiPavuMeter.Text = Val(dt1.Rows(0).Item("Pavu_Meters").ToString)
                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_Transport.Text = dt1.Rows(0).Item("TransportName").ToString
                cbo_RecForm.Text = dt1.Rows(0).Item("RecFromName").ToString
                txt_Freight.Text = Val(dt1.Rows(0).Item("Freight").ToString)
                cbo_description.Text = Trim(dt1.Rows(0).Item("Description").ToString)
                txt_EWay_billNo.Text = Trim(dt1.Rows(0).Item("eway_bill_no").ToString)
                cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))

                txt_DcNo.Text = Trim(dt1.Rows(0).Item("Dc_No").ToString)
                txt_DcDate.Text = Trim(dt1.Rows(0).Item("Dc_Date").ToString)

                txt_RateMeters.Text = Format(Val(dt1.Rows(0).Item("Rate_Meters").ToString), "########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("Add_Less").ToString), "########0.00")
                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")
                txt_Roundoff.Text = Format(Val(dt1.Rows(0).Item("Roundoff").ToString), "########0.00")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))


                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                txt_DateAndTimeOFSupply.Text = dt1.Rows(0).Item("Date_And_Time_Of_Supply").ToString
                cbo_ShippedTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("ShippedTo_IdNo").ToString))

                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True


                txt_eInvoiceNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
                txt_eInvoiceNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_No")) Then txt_eInvoiceAckNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_ACK_No").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_Date")) Then txt_eInvoiceAckDate.Text = Trim(dt1.Rows(0).Item("E_Invoice_ACK_Date").ToString)
                If Trim(txt_eInvoiceNo.Text) <> "" Then
                    If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancelled_Status")) Then
                        txt_eInvoice_CancelStatus.Text = IIf(dt1.Rows(0).Item("E_Invoice_Cancelled_Status") = True, "Cancelled", "Active")
                    End If
                End If

                If IsDBNull(dt1.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(dt1.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then

                                pic_IRN_QRCode_Image.BackgroundImage = Image.FromStream(ms)

                            End If
                        End Using
                    End If
                End If

                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason")) Then txt_EInvoiceCancellationReson.Text = Trim(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason").ToString)

                If Not IsDBNull(dt1.Rows(0).Item("EWB_No")) Then txt_eWayBill_No.Text = Trim(dt1.Rows(0).Item("EWB_No").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Date")) Then txt_EWB_Date.Text = Trim(dt1.Rows(0).Item("EWB_Date").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Valid_Upto")) Then txt_EWB_ValidUpto.Text = Trim(dt1.Rows(0).Item("EWB_Valid_Upto").ToString)
                If Trim(txt_eWayBill_No.Text) <> "" Then
                    If Not IsDBNull(dt1.Rows(0).Item("EWB_Cancelled")) Then
                        If dt1.Rows(0).Item("EWB_Cancelled") = True Then
                            txt_EWB_Cancel_Status.Text = "Cancelled"
                        Else
                            txt_EWB_Cancel_Status.Text = "Active"
                        End If
                    End If
                End If


                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason")) Then txt_EWB_Canellation_Reason.Text = Trim(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason").ToString)


                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Pavu_Delivery_Increment, c.EndsCount_Name, d.Beam_Width_Name from Pavu_Sales_Details a LEFT JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No LEFT JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_Idno = d.Beam_Width_Idno where a.Pavu_Sales_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_PavuDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then
                    tot_Amt = 0
                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_PavuDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_PavuDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_PavuDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Set_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Beam_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Pcs").ToString
                        If Val(dt2.Rows(i).Item("Meters_Pc").ToString) <> 0 Then
                            dgv_PavuDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Meters_Pc").ToString)
                        End If
                        dgv_PavuDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        dgv_PavuDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                        dgv_PavuDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Beam_Width_Name").ToString

                        dgv_PavuDetails.Rows(n).Cells(8).Value = ""
                        dgv_PavuDetails.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Noof_Used").ToString
                        dgv_PavuDetails.Rows(n).Cells(10).Value = dt2.Rows(i).Item("set_code").ToString
                        dgv_PavuDetails.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Pavu_Delivery_Increment").ToString


                        'If Val(dt2.Rows(i).Item("Rate").ToString) <> 0 Then
                        dgv_PavuDetails.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_PavuDetails.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

                        'Else
                        '    dgv_PavuDetails.Rows(n).Cells(12).Value = Format(Val(dt1.Rows(0).Item("Rate_Meters").ToString), "########0.00")
                        '    dgv_PavuDetails.Rows(n).Cells(13).Value = Format(Val(dgv_PavuDetails.Rows(n).Cells(5).Value) * Val(dgv_PavuDetails.Rows(n).Cells(12).Value), "########0.00")
                        'End If

                        dgv_PavuDetails.Rows(n).Cells(14).Value = dt2.Rows(i).Item("Noof_Beams").ToString

                        If Val(dgv_PavuDetails.Rows(n).Cells(9).Value) > 0 And Val(dgv_PavuDetails.Rows(n).Cells(9).Value) <> Val(dgv_PavuDetails.Rows(n).Cells(11).Value) Then
                            dgv_PavuDetails.Rows(n).Cells(8).Value = "1"
                        End If

                    Next i

                End If

                With dgv_PavuDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Beam").ToString)
                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(13).Value = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                End With

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CGST_Perc.Text = dt1.Rows(0).Item("CGST_Percentage").ToString
                lbl_CGstAmount.Text = dt1.Rows(0).Item("CGST_Amount").ToString
                txt_SGST_Perc.Text = dt1.Rows(0).Item("SGST_Percentage").ToString
                lbl_SGstAmount.Text = dt1.Rows(0).Item("SGST_Amount").ToString
                txt_IGST_Perc.Text = dt1.Rows(0).Item("IGST_Percentage").ToString
                lbl_IGstAmount.Text = dt1.Rows(0).Item("IGST_Amount").ToString
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                dt2.Clear()

            Else
                new_record()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()


            TotalPavu_Calculation()
            NetAmount_Calculation()


            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Nr As Long = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text)



        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Pavu_Sales_Entry, New_Entry, Me, con, "Pavu_Sales_Head", "Pavu_Sales_Code", NewCode, "Pavu_Sales_Date", "(Pavu_Sales_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "Pavu_sales_Head", "Verified_Status", "(Pavu_sales_Code = '" & Trim(NewCode) & "')")) = 1 Then
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

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Pavu_Sales_Head", "Pavu_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Pavu_Sales_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Pavu_Sales_Details", "Pavu_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Set_No,Beam_No,Pcs,Meters_Pc,Meters,EndsCount_IdNo,Beam_Width_IdNo,Noof_Used,Set_Code,Rate,Amount", "Sl_No", "Pavu_Sales_Code, For_OrderBy, Company_IdNo, Pavu_Sales_No, Pavu_Sales_Date, Ledger_Idno", trans)

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If
            If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(NewCode), trans)

            Da = New SqlClient.SqlDataAdapter("Select * from Pavu_Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    Nr = 0
                    cmd.CommandText = "update Stock_SizedPavu_Processing_Details set " _
                              & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("ReceivedFrom_IdNo").ToString)) & ", " _
                              & " Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1 " _
                              & " Where " _
                              & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("DeliveryTo_IdNo").ToString)) & " and " _
                              & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                              & " beam_no = '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "' and " _
                              & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString))
                    Nr = cmd.ExecuteNonQuery

                    If Nr = 0 Then
                        Throw New ApplicationException("Some Beams Delivered to Others - Beam No : " & Trim(Dt1.Rows(i).Item("Beam_No").ToString))
                        Exit Sub
                    End If

                Next
            End If
            Dt1.Clear()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Pavu_Sales_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Pavu_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

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
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
            da.Fill(dt3)
            cbo_Filter_EndsCountName.DataSource = dt3
            cbo_Filter_EndsCountName.DisplayMember = "EndsCount_Name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate

            cbo_Filter_PartyName.Text = ""

            cbo_Filter_EndsCountName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1

            cbo_Filter_EndsCountName.SelectedIndex = -1

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

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Sales_No from Pavu_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Entry_VAT_GST_Type = 'GST' Order by for_Orderby, Pavu_Sales_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Sales_No from Pavu_Sales_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Entry_VAT_GST_Type = 'GST' Order by for_Orderby, Pavu_Sales_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Sales_No from Pavu_Sales_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, Pavu_Sales_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Sales_No from Pavu_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, Pavu_Sales_No desc", con)
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
        Dim dt1 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_InvNo.Text = Common_Procedures.get_MaxCode(con, "Pavu_Sales_Head", "Pavu_Sales_Code", "For_OrderBy", "Entry_VAT_GST_Type = 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_InvNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Pavu_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, Pavu_Sales_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Pavu_Sales_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Pavu_Sales_Date").ToString
                End If
                If dt1.Rows(0).Item("PavuSale_PrefixNo").ToString <> "" Then txt_Pavu_PrefixNo.Text = dt1.Rows(0).Item("PavuSale_PrefixNo").ToString
                If dt1.Rows(0).Item("SalesAc_IdNo").ToString <> "" Then cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))
                If dt1.Rows(0).Item("CGST_Percentage").ToString <> "" Then txt_CGST_Perc.Text = dt1.Rows(0).Item("CGST_Percentage").ToString
                If dt1.Rows(0).Item("SGST_Percentage").ToString <> "" Then txt_SGST_Perc.Text = dt1.Rows(0).Item("SGST_Percentage").ToString
                If dt1.Rows(0).Item("IGST_Percentage").ToString <> "" Then txt_IGST_Perc.Text = dt1.Rows(0).Item("IGST_Percentage").ToString
            End If
            dt1.Clear()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus() : msk_date.SelectionStart = 0

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Pavu_Sales_No from Pavu_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Pavu_Sales_Entry, New_Entry, Me) = False Then Exit Sub


        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Pavu_Sales_No from Pavu_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvNo.Text = Trim(UCase(inpno))

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
        Dim Delv_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim KuPvu_EdsCnt_ID As Integer = 0
        Dim SzPvu_EdsCnt_ID As Integer = 0
        Dim vEdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotPvuBms As Single, vTotPvuMtrs As Single, vTotPvuPcs As Single
        Dim YCnt_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim EntID As String = ""
        Dim Bw_IdNo As Integer = 0
        Dim Pavu_DelvInc As Integer = 0
        Dim Ent_NoofUsed As Integer = 0
        Dim Stock_In As String
        Dim mtrspcs As Integer
        Dim dt2 As New DataTable
        Dim vTotPvuStk As Single = 0
        Dim SalesAc_ID As Integer = 0
        Dim VouBil As String = ""
        Dim Partc_AC As String = ""
        Dim vTotEBBms As Integer = 0
        Dim Delv_Ledtype As String = ""
        Dim GdEdCt_ID As Integer = 0
        Dim vNOOFBMS As Integer = 0

        Dim Verified_STS As String = ""

        Dim vOrdByNo As String = ""

        Dim Shipd_ID As Integer = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub


        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Pavu_Sales_Entry, New_Entry, Me, con, "Pavu_Sales_Head", "Pavu_Sales_Code", NewCode, "Pavu_Sales_Date", "(Pavu_Sales_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Pavu_Sales_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "Pavu_sales_Head", "Verified_Status", "(Pavu_sales_Code = '" & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        KuPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
            If KuPvu_EdsCnt_ID = 0 Then
                MessageBox.Show("Invalid EndsCount Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
                Exit Sub
            End If
        End If

        Delv_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_BillTo.Text)
        If Delv_ID = 0 Then
            MessageBox.Show("Invalid Delivery Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_BillTo.Enabled And cbo_BillTo.Visible Then cbo_BillTo.Focus()
            Exit Sub
        End If
        lbl_UserName.Text = Common_Procedures.User.IdNo
        Rec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)
        If Rec_ID = 0 Then Rec_ID = 4

        If Delv_ID = Rec_ID Then
            MessageBox.Show("Invalid Party Name" & Chr(13) & "Does not accept same party", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_BillTo.Enabled And cbo_BillTo.Visible Then cbo_BillTo.Focus()
            Exit Sub
        End If
        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_BillTo.Text)
        SalesAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAc.Text)
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        Shipd_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_ShippedTo.Text)

        With dgv_PavuDetails

            For i = 0 To .RowCount - 1

                If Val(dgv_PavuDetails.Rows(i).Cells(5).Value) <> 0 Then

                    If Trim(UCase(cbo_EntryType.Text)) = Trim(UCase("FROM STOCK")) Then

                        If Trim(dgv_PavuDetails.Rows(i).Cells(1).Value) = "" Then
                            MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                                dgv_PavuDetails.Focus()
                                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                            End If
                            Exit Sub
                        End If

                        If Trim(dgv_PavuDetails.Rows(i).Cells(2).Value) = "" Then
                            MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                                dgv_PavuDetails.Focus()
                                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(2)
                            End If
                            Exit Sub
                        End If


                        If Trim(dgv_PavuDetails.Rows(i).Cells(10).Value) = "" Then
                            MessageBox.Show("Invalid Set Code", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                                dgv_PavuDetails.Focus()
                                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                            End If
                            Exit Sub
                        End If


                    End If


                    vEdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(6).Value)
                    If Val(vEdsCnt_ID) = 0 Then
                        MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.Focus()
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(6)
                        End If
                        Exit Sub
                    End If

                    If .Columns(14).Visible = True Then
                        If Val(dgv_PavuDetails.Rows(i).Cells(14).Value) = 0 Then
                            MessageBox.Show("Invalid No.of Beams", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                                dgv_PavuDetails.Focus()
                                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(14)
                            End If
                            Exit Sub
                        End If
                    End If

                End If

            Next
        End With

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1

        vTotPvuBms = 0 : vTotPvuMtrs = 0 : vTotPvuPcs = 0
        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTotPvuBms = Val(dgv_PavuDetails_Total.Rows(0).Cells(2).Value())
            vTotPvuPcs = Val(dgv_PavuDetails_Total.Rows(0).Cells(3).Value())
            vTotPvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(5).Value())
        End If

        If Val(lbl_NetAmount.Text) = 0 Then lbl_NetAmount.Text = 0

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

        Dim ms As New MemoryStream()
        If IsNothing(pic_IRN_QRCode_Image.BackgroundImage) = False Then
            Dim bitmp As New Bitmap(pic_IRN_QRCode_Image.BackgroundImage)
            bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
        End If
        Dim data As Byte() = ms.GetBuffer()
        Dim p As New SqlClient.SqlParameter("@QrCode", SqlDbType.Image)
        p.Value = data
        cmd.Parameters.Add(p)
        ms.Dispose()

        Dim vEInvAckDate As String = ""
        vEInvAckDate = ""
        If Trim(txt_eInvoiceAckDate.Text) <> "" Then
            If IsDate(txt_eInvoiceAckDate.Text) = True Then
                If Year(CDate(txt_eInvoiceAckDate.Text)) <> 1900 Then
                    vEInvAckDate = Trim(txt_eInvoiceAckDate.Text)
                End If

            End If
        End If
        If Trim(vEInvAckDate) <> "" Then
            cmd.Parameters.AddWithValue("@EInvoiceAckDate", Convert.ToDateTime(vEInvAckDate))
        End If

        Dim eiCancel As String = "0"
        If txt_eInvoice_CancelStatus.Text = "Cancelled" Then
            eiCancel = "1"
        End If
        Dim EWBCancel As String = "0"
        If txt_EWB_Cancel_Status.Text = "Cancelled" Then
            eiCancel = "1"
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvNo.Text = Common_Procedures.get_MaxCode(con, "Pavu_Sales_Head", "Pavu_Sales_Code", "For_OrderBy", "Entry_VAT_GST_Type = 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr




            If New_Entry = True Then
                If Trim(txt_DateAndTimeOFSupply.Text) = "" Then txt_DateAndTimeOFSupply.Text = Format(Now, "dd-MM-yyyy hh:mm tt")
                cmd.CommandText = "Insert into Pavu_Sales_Head (Entry_VAT_GST_Type ,            Pavu_Sales_Code       ,                   Company_IdNo                     ,          Pavu_Sales_No                ,                               for_OrderBy                                             , Pavu_Sales_Date   ,          DeliveryTo_IdNo          ,     ReceivedFrom_IdNo    ,                SalesAc_IdNo                ,       EndsCount_IdNo                   ,              Pavu_Meters                          ,       Empty_Beam                          , Vehicle_No                         , Transport_Idno        , Total_Beam                   ,         Total_Pcs                    , Total_Meters                  , Freight                       , Rate_Meters                     , Gross_Amount                     , Add_Less                    , Net_Amount                                ,  User_IdNo                   , CGST_Percentage                    , CGST_Amount                         , SGST_Percentage                    , SGST_Amount                          ,     IGST_Percentage                 , IGST_Amount                          , PavuSale_PrefixNo                     ,                  Date_And_Time_Of_Supply        ,                 Roundoff ,                                 Verified_Status          ,   Description,                               eway_bill_no                ,      ShippedTo_IdNo        ,               Dc_No          ,               Dc_Date            ,                Remarks           ,EWB_No ) " &
                                    "           Values         (    'GST'          ,           '" & Trim(NewCode) & "',         " & Str(Val(lbl_Company.Tag)) & "         ,   '" & Trim(lbl_InvNo.Text) & "'       ,       " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & "             , @EntryDate      ,       " & Str(Val(Delv_ID)) & "   ,     " & Val(Rec_ID) & " ,          " & Str(Val(SalesAc_ID)) & "      ,    " & Str(Val(KuPvu_EdsCnt_ID)) & "   ,    " & Val(txt_KuraiPavuMeter.Text) & "       ,       " & Val(txt_KuraiPavuBeam.Text) & " , '" & Trim(cbo_VehicleNo.Text) & "' , " & Val(Trans_ID) & "     , " & Str(Val(vTotPvuBms)) & " ,    " & Str(Val(vTotPvuPcs)) & " , " & Str(Val(vTotPvuMtrs)) & " , " & Val(txt_Freight.Text) & " , " & Val(txt_RateMeters.Text) & ", " & Val(lbl_GrossAmount.Text) & "," & Val(txt_AddLess.Text) & "," & Str(Val(CSng(lbl_NetAmount.Text))) & " ," & Val(lbl_UserName.Text) & "," & Str(Val(txt_CGST_Perc.Text)) & "," & Str(Val(lbl_CGstAmount.Text)) & "," & Str(Val(txt_SGST_Perc.Text)) & ", " & Str(Val(lbl_SGstAmount.Text)) & ", " & Str(Val(txt_IGST_Perc.Text)) & ", " & Str(Val(lbl_IGstAmount.Text)) & ", '" & Trim(txt_Pavu_PrefixNo.Text) & "',          '" & Trim(txt_DateAndTimeOFSupply.Text) & "',           " & Str(Val(txt_Roundoff.Text)) & "   ,   " & Val(Verified_STS) & "    ,'" & Trim(cbo_description.Text) & "' ,'" & Trim(txt_EWay_billNo.Text) & "'    , " & Str(Val(Shipd_ID)) & " , '" & Trim(txt_DcNo.Text) & "', '" & Trim(txt_DcDate.Text) & "', '" & Trim(txt_Remarks.Text) & "' ,'" & Trim(txt_EWay_billNo.Text) & "') "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Pavu_Sales_Head", "Pavu_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Pavu_Sales_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Pavu_Sales_Details", "Pavu_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No,Beam_No,Pcs,Meters_Pc,Meters,EndsCount_IdNo,Beam_Width_IdNo,Noof_Used,Set_Code,Rate,Amount", "Sl_No", "Pavu_Sales_Code, For_OrderBy, Company_IdNo, Pavu_Sales_No, Pavu_Sales_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Pavu_Sales_Head set Entry_VAT_GST_Type = 'GST', Pavu_Sales_Date = @EntryDate, DeliveryTo_IdNo = " & Str(Val(Delv_ID)) & ", ReceivedFrom_IdNo = " & Str(Val(Rec_ID)) & ", Empty_Beam = " & Str(Val(txt_KuraiPavuBeam.Text)) & ", Pavu_Meters = " & Str(Val(txt_KuraiPavuMeter.Text)) & ", SalesAc_IdNo =" & Str(Val(SalesAc_ID)) & ",EndsCount_IdNo = " & Str(Val(KuPvu_EdsCnt_ID)) & ", Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' , Transport_Idno = " & Str(Val(Trans_ID)) & ", Freight = " & Str(Val(txt_Freight.Text)) & ", Total_Beam = " & Str(Val(vTotPvuBms)) & ", Total_Pcs = " & Str(Val(vTotPvuPcs)) & ", Total_Meters = " & Str(Val(vTotPvuMtrs)) & " ,Rate_Meters =" & Val(txt_RateMeters.Text) & ",Gross_Amount = " & Val(lbl_GrossAmount.Text) & ",Add_Less = " & Val(txt_AddLess.Text) & ",Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ",User_idNo = " & Val(lbl_UserName.Text) & ",CGST_Percentage =" & Str(Val(txt_CGST_Perc.Text)) & " ,CGST_Amount =" & Str(Val(lbl_CGstAmount.Text)) & " ,SGST_Percentage =" & Str(Val(txt_SGST_Perc.Text)) & ",SGST_Amount =" & Str(Val(lbl_SGstAmount.Text)) & ",IGST_Percentage =" & Str(Val(txt_IGST_Perc.Text)) & ",IGST_Amount =" & Str(Val(lbl_IGstAmount.Text)) & " , PavuSale_PrefixNo = '" & Trim(txt_Pavu_PrefixNo.Text) & "',Date_And_Time_Of_Supply = '" & Trim(txt_DateAndTimeOFSupply.Text) & "',Roundoff= " & txt_Roundoff.Text & " ,Verified_Status= " & Val(Verified_STS) & ",Description='" & Trim(cbo_description.Text) & "',eway_bill_no='" & Trim(txt_EWay_billNo.Text) & "',EWB_No='" & Trim(txt_EWay_billNo.Text) & "' , ShippedTo_IdNo=" & Str(Val(Shipd_ID)) & " , Dc_No = '" & Trim(txt_DcNo.Text) & "', Dc_Date = '" & Trim(txt_DcDate.Text) & "',E_Invoice_IRNO = '" & Trim(txt_eInvoiceNo.Text) & "' , E_Invoice_QR_Image =  @QrCode  , E_Invoice_ACK_No = '" & Trim(txt_eInvoiceAckNo.Text) & "' , E_Invoice_ACK_Date = " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & "  ,  E_Invoice_Cancelled_Status = " & eiCancel.ToString & " ,  E_Invoice_Cancellation_Reason = '" & Trim(txt_EInvoiceCancellationReson.Text) & "'  ,EWB_Date = '" & Trim(txt_EWB_Date.Text) & "',EWB_Valid_Upto = '" & Trim(txt_EWB_ValidUpto.Text) & "',EWB_Cancelled = " & EWBCancel.ToString & " ,  EWBCancellation_Reason = '" & Trim(txt_EWB_Canellation_Reason.Text) & "' , Remarks = '" & Trim(txt_Remarks.Text) & "'   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("Select * from Pavu_Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    For i = 0 To Dt1.Rows.Count - 1

                        cmd.CommandText = "update Stock_SizedPavu_Processing_Details set " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("ReceivedFrom_IdNo").ToString)) & ", " _
                                  & " Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1 " _
                                  & " Where " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("DeliveryTo_IdNo").ToString)) & " and " _
                                  & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                                  & " beam_no = '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "' and " _
                                  & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString))

                        cmd.ExecuteNonQuery()

                    Next
                End If
                Dt1.Clear()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Pavu_Sales_Head", "Pavu_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Pavu_Sales_Code, Company_IdNo, for_OrderBy", tr)

            EntID = Trim(Pk_Condition) & Trim(lbl_InvNo.Text)
            Partcls = "Pavu Sales : Inv.No. " & Trim(lbl_InvNo.Text)
            PBlNo = Trim(lbl_InvNo.Text)
            If Trim(txt_Pavu_PrefixNo.Text) <> "" Then
                Partcls = "Pavu Sales : Inv.No. " & Trim(txt_Pavu_PrefixNo.Text) & "-" & Trim(lbl_InvNo.Text)
                PBlNo = Trim(txt_Pavu_PrefixNo.Text) & "-" & Trim(lbl_InvNo.Text)
            End If

            cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Pavu_Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(txt_KuraiPavuMeter.Text) <> 0 And Val(KuPvu_EdsCnt_ID) <> 0 Then

                cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(KuPvu_EdsCnt_ID)) & ", " & Str(Val(txt_KuraiPavuBeam.Text)) & ", " & Str(Val(txt_KuraiPavuMeter.Text)) & ")"
                cmd.ExecuteNonQuery()

            End If

            With dgv_PavuDetails
                Sno = 0
                Partc_AC = ""
                For i = 0 To dgv_PavuDetails.RowCount - 1

                    If Val(dgv_PavuDetails.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        'If Trim(cbo_EntryType.Text) = "FROM STOCK" Then
                        '    SzPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(6).Value, tr)
                        'Else
                        '    SzPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, Trim(cbo_Grid_EndsCount.Text), tr)
                        'End If
                        SzPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(6).Value, tr)

                        Bw_IdNo = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(i).Cells(7).Value, tr)

                        Partc_AC = Partc_AC & "-" & "Set : " & Trim(.Rows(i).Cells(1).Value) & " ,Beam : " & Trim(.Rows(i).Cells(2).Value) & ", Meter : " & Val(dgv_PavuDetails.Rows(i).Cells(5).Value)

                        If Trim(UCase(cbo_EntryType.Text)) = Trim(UCase("FROM STOCK")) Then

                            Ent_NoofUsed = 0
                            If Val(.Rows(i).Cells(9).Value) = 0 Or (Val(.Rows(i).Cells(9).Value) > 0 And Val(.Rows(i).Cells(9).Value) = Val(.Rows(i).Cells(11).Value)) Then

                                Nr = 0
                                cmd.CommandText = "update Stock_SizedPavu_Processing_Details set StockAt_IdNo = " & Str(Val(Delv_ID)) & ", Pavu_Delivery_Increment = Pavu_Delivery_Increment + 1 " &
                                "Where      Set_Code = '" & Trim(.Rows(i).Cells(10).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and StockAt_IdNo = " & Str(Val(Rec_ID))
                                Nr = cmd.ExecuteNonQuery()

                                If Nr = 0 Then
                                    Throw New ApplicationException("Mismath Received From Name and Beam Details")
                                    Exit Sub
                                End If

                                Ent_NoofUsed = Val(Common_Procedures.get_FieldValue(con, "Stock_SizedPavu_Processing_Details", "Pavu_Delivery_Increment", "(Set_Code = '" & Trim(.Rows(i).Cells(10).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' )", , tr))

                            Else
                                Ent_NoofUsed = Val(.Rows(i).Cells(9).Value)

                            End If
                        End If


                        vNOOFBMS = Val(.Rows(i).Cells(14).Value)
                        If Val(vNOOFBMS) = 0 Then vNOOFBMS = 1

                        cmd.CommandText = "Insert into Pavu_Sales_Details ( Pavu_Sales_Code,              Company_IdNo        ,     Pavu_Sales_No            ,                               for_OrderBy                             , Pavu_Sales_Date , DeliveryTo_IdNo          ,    ReceivedFrom_IdNo     ,          Sl_No        ,                    Set_No              ,                    Beam_No             ,                      Pcs                 ,                      Meters_Pc           ,                      Meters              ,             EndsCount_IdNo       ,      Beam_Width_IdNo     ,              Noof_Used        ,                  Set_Code                 ,                      Rate                 ,                      Amount               ,            Noof_Beams      ) " &
                        " Values                                ('" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ", @EntryDate      , " & Str(Val(Delv_ID)) & ",  " & Str(Val(Rec_ID)) & ",  " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(SzPvu_EdsCnt_ID)) & ", " & Str(Val(Bw_IdNo)) & ", " & Str(Val(Ent_NoofUsed)) & ", '" & Trim(.Rows(i).Cells(10).Value) & "', " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & ", " & Str(Val(vNOOFBMS)) & " ) "
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(SzPvu_EdsCnt_ID)) & ", " & Str(Val(vNOOFBMS)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ")"

                        cmd.ExecuteNonQuery()

                    End If

                Next

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Pavu_Sales_Details", "Pavu_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No,Beam_No,Pcs,Meters_Pc,Meters,EndsCount_IdNo,Beam_Width_IdNo,Noof_Used,Set_Code,Rate,Amount", "Sl_No", "Pavu_Sales_Code, For_OrderBy, Company_IdNo, Pavu_Sales_No, Pavu_Sales_Date, Ledger_Idno", tr)

            End With

            Da = New SqlClient.SqlDataAdapter("select Int1 as PavuEndsCount_IdNo, sum(Int2) as PavuBeam, sum(Meters1) as PavuMeters from " & Trim(Common_Procedures.EntryTempTable) & " group by Int1 having sum(Int2) <> 0 or sum(Meters1) <> 0", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            Sno = 0
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1
                    vTotPvuMtrs = 0
                    vTotPvuMtrs = Str(Val(Dt1.Rows(i).Item("PavuMeters").ToString))

                    Stock_In = ""
                    mtrspcs = 0

                    Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  EndsCount_Head Where EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)), con)
                    Da.SelectCommand.Transaction = tr
                    dt2 = New DataTable
                    Da.Fill(dt2)

                    If dt2.Rows.Count > 0 Then
                        Stock_In = dt2.Rows(0)("Stock_In").ToString
                        mtrspcs = Val(dt2.Rows(0)("Meters_Pcs").ToString)
                    End If
                    dt2.Clear()

                    If Trim(UCase(Stock_In)) = "PCS" Then
                        If Val(mtrspcs) = 0 Then mtrspcs = 1
                        vTotPvuStk = vTotPvuMtrs / mtrspcs

                    Else
                        vTotPvuStk = vTotPvuMtrs

                    End If

                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo                     , Reference_No                 , for_OrderBy                                                           , Reference_Date, DeliveryTo_Idno          , ReceivedFrom_Idno       , Cloth_Idno, Entry_ID             , Party_Bill_No        , Particulars            , Sl_No                , EndsCount_IdNo                                                   , Sized_Beam                                             , Meters                       ) " &
                    "Values                       ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ", @EntryDate    , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", 0         , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(vTotPvuStk)) & " )"
                    cmd.ExecuteNonQuery()

                Next
            End If
            Dt1.Clear()

            If Val(txt_KuraiPavuBeam.Text) <> 0 Or Val(vTotPvuBms) <> 0 Then


                Delv_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Delv_ID)) & ")", , tr)

                vTotEBBms = 0
                If Trim(UCase(Delv_Ledtype)) <> "WEAVER" Then
                    vTotEBBms = Val(txt_KuraiPavuBeam.Text) + Val(vTotPvuBms)
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Entry_ID, Particulars, Sl_No, Beam_Width_IdNo, Empty_Beam, Pavu_Beam) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(PBlNo) & "', '" & Trim(EntID) & "', '" & Trim(Partcls) & "', 1, 0, " & Str(Val(vTotEBBms)) & ", " & Str(Val(txt_KuraiPavuBeam.Text) + Val(vTotPvuBms)) & ")"
                cmd.ExecuteNonQuery()
            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim vCGSTAmt As String = Format(Val((lbl_CGstAmount.Text)), "#############0.00")
            Dim vSGSTAmt As String = Format(Val((lbl_SGstAmount.Text)), "#############0.00")
            Dim vIGSTAmt As String = Format(Val((lbl_IGstAmount.Text)), "#############0.00")


            'If Val(lbl_NetAmount.Text) <> 0 Then
            vLed_IdNos = Led_ID & "|" & SalesAc_ID & ""
            vVou_Amts = -1 * Val(CSng(lbl_NetAmount.Text)) & "|" & (Val(CSng(lbl_NetAmount.Text)))




            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(NewCode), tr)

            If Common_Procedures.Voucher_Updation(con, "Pavu.Sales", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(PBlNo), Convert.ToDateTime(dtp_Date.Text), "Bill No. : " & Trim(PBlNo), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If
            'End If


            'Bill Posting
            If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, Led_ID, Trim(PBlNo), 0, Str(Val(CSng(lbl_NetAmount.Text))), "DR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_InvNo.Text)
                End If
            Else
                move_record(lbl_InvNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub cbo_DelvAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BillTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_DelvAt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BillTo, cbo_EntryType, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If txt_DateAndTimeOFSupply.Visible Then
                txt_DateAndTimeOFSupply.Focus()

            ElseIf cbo_ShippedTo.Visible Then
                cbo_ShippedTo.Focus()
            Else
                cbo_RecForm.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DelvAt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BillTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BillTo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If txt_DateAndTimeOFSupply.Visible Then
                txt_DateAndTimeOFSupply.Focus()

            ElseIf cbo_ShippedTo.Visible Then
                cbo_ShippedTo.Focus()

            Else
                cbo_RecForm.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DelvAt_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BillTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_RecForm_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RecForm.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_Type = 'GODOWN'", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_RecForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecForm, cbo_ShippedTo, cbo_SalesAc, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_Type = 'GODOWN'", "(Ledger_idno = 0)")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_ShippedTo.Visible And cbo_ShippedTo.Enabled Then
                cbo_ShippedTo.Focus()
            ElseIf txt_DateAndTimeOFSupply.Visible And txt_DateAndTimeOFSupply.Enabled Then
                txt_DateAndTimeOFSupply.Focus()
            Else
                cbo_BillTo.Focus()
            End If

        End If
    End Sub

    Private Sub cbo_RecForm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RecForm.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecForm, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_Type = 'GODOWN'", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(cbo_EntryType.Text) = "FROM STOCK" Then
                If MessageBox.Show("Do you want to select Pavu :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    cbo_SalesAc.Focus()

                End If

            Else
                cbo_SalesAc.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_RecForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RecForm.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAc, cbo_RecForm, txt_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAc, txt_dcno, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            'Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")

    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, txt_EWay_billNo, txt_KuraiPavuBeam, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_KuraiPavuBeam, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
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

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_RateMeters, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
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

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Pavu_Sales_Head", "Vehicle_No", "", "")

    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, cbo_description, "Pavu_Sales_Head", "Vehicle_No", "", "")

    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, cbo_description, "Pavu_Sales_Head", "Vehicle_No", "", "", False)

    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If (e.KeyValue = 38) Then
            If Trim(cbo_EntryType.Text) = "DIRECT" Then
                If dgv_PavuDetails.Rows.Count > 0 Then
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                    dgv_PavuDetails.Focus()
                Else
                    cbo_VehicleNo.Focus()

                End If

            Else
                cbo_VehicleNo.Focus()

            End If
        End If

        If e.KeyValue = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            txt_AddLess.Focus()
        End If

    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True

            txt_AddLess.Focus()

            'If Trim(cbo_EntryType.Text) = "DIRECT" Then
            '    If dgv_PavuDetails.Rows.Count > 0 Then
            '        dgv_PavuDetails.Focus()
            '        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)

            '    Else
            '        txt_RateMeters.Focus()

            '    End If

            'Else
            '    txt_RateMeters.Focus()

            'End If

        End If

    End Sub

    Private Sub txt_KuraiPavuMeter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_KuraiPavuMeter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_KuraiPavuBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_KuraiPavuBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
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

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, EdsCnt_IdNo As Integer
        Dim Condt As String = ""
        Dim EdsEdsCnt_IdNo As Integer, Mil_IdNo As Integer

        Try

            Condt = ""
            Led_IdNo = 0
            EdsCnt_IdNo = 0
            Mil_IdNo = 0
            EdsEdsCnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Pavu_Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Pavu_Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Pavu_Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_EndsCountName.Text) <> "" Then
                EdsCnt_IdNo = Common_Procedures.EndsCount_NameToIdNo(con, cbo_Filter_EndsCountName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.DeliveryTo_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If

            If Val(EdsCnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo)) & " or d.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo)) & ") "
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo)) & " "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.EndsCount_Name from Pavu_Sales_Head a INNER JOIN Pavu_Sales_Details d on a.Pavu_Sales_Code = d.Pavu_Sales_Code INNER JOIN Ledger_Head b on a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head c on d.EndsCount_IdNo = c.EndsCount_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Pavu_Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Pavu_Sales_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Pavu_Sales_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Pavu_Sales_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("EndsCount_Name").ToString

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

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_EndsCountName, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_EndsCountName, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_EndsCountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsCountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub


    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsCountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsCountName, cbo_Filter_PartyName, btn_Filter_Show, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsCountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsCountName, btn_Filter_Show, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_Details = Nothing
        dgtxt_Details = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try

            If FrmLdSTS = True Then Exit Sub

            dgv_PavuDetails.EditingControl.BackColor = Color.Lime
            dgv_PavuDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_Details.SelectAll()
        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim rect As Rectangle

        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_PavuDetails

                If .Visible = True Then

                    If .Rows.Count > 0 Then

                        'dgv_PavuDetails.Tag = .CurrentCell.Value

                        If Val(.CurrentRow.Cells(0).Value) = 0 Then
                            .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                        End If

                        If e.ColumnIndex = 6 Then

                            If cbo_Grid_EndsCount.Visible = False Or Trim(UCase(cbo_EntryType.Text)) = "DIRECT" Or Val(cbo_Grid_EndsCount.Tag) <> e.RowIndex Then

                                cbo_Grid_EndsCount.Tag = -1
                                da = New SqlClient.SqlDataAdapter("SELECT EndsCount_Name FROM EndsCount_Head ORDER BY EndsCount_Name", con)
                                dt1 = New DataTable
                                da.Fill(dt1)
                                cbo_Grid_EndsCount.DataSource = dt1
                                cbo_Grid_EndsCount.DisplayMember = "EndsCount_Name"

                                rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                                cbo_Grid_EndsCount.Left = .Left + rect.Left
                                cbo_Grid_EndsCount.Top = .Top + rect.Top

                                cbo_Grid_EndsCount.Width = rect.Width
                                cbo_Grid_EndsCount.Height = rect.Height
                                cbo_Grid_EndsCount.Text = .CurrentCell.Value

                                cbo_Grid_EndsCount.Tag = Val(e.RowIndex)
                                cbo_Grid_EndsCount.Visible = True

                                cbo_Grid_EndsCount.BringToFront()
                                cbo_Grid_EndsCount.Focus()

                            End If

                        Else

                            cbo_Grid_EndsCount.Visible = False

                        End If

                    End If
                End If

            End With


        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        If FrmLdSTS = True Then Exit Sub

        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
        With dgv_PavuDetails
            If .Visible Then
                If e.ColumnIndex = 3 Or e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 12 Then

                    If e.ColumnIndex = 3 Or e.ColumnIndex = 4 Then
                        .Rows(e.RowIndex).Cells(5).Value = Format(Val(.Rows(e.RowIndex).Cells(3).Value) * Val(.Rows(e.RowIndex).Cells(4).Value), "#########0.00")
                    End If

                    If e.ColumnIndex = 5 Or e.ColumnIndex = 12 Then
                        .Rows(e.RowIndex).Cells(13).Value = Format(Val(.Rows(e.RowIndex).Cells(5).Value) * Val(.Rows(e.RowIndex).Cells(12).Value), "#########0.00")
                    End If

                    TotalPavu_Calculation()

                End If
            End If
        End With

    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

                n = .CurrentRow.Index

                If Val(.Rows(n).Cells(8).Value) > 0 And Val(.Rows(n).Cells(8).Value) <> Val(.Rows(n).Cells(10).Value) Then
                    MessageBox.Show("Cannot Delete" & Chr(13) & "Already this pavu delivered to others")
                    Exit Sub
                End If

                If n = .Rows.Count - 1 Then

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

            TotalPavu_Calculation()
            NetAmount_Calculation()


        End If

    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer = 0
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then
            With dgv_PavuDetails
                If Trim(cbo_EntryType.Text) = "DIRECT" Then
                    .Rows(e.RowIndex).Cells(0).Value = Val(e.RowIndex) + 1
                Else
                    n = .RowCount
                    .Rows(n - 1).Cells(0).Value = Val(n)
                End If
            End With
        End If
    End Sub

    Private Sub TotalPavu_Calculation()
        Dim Sno As Integer
        Dim TotBms As Integer, TotMtrs As Single, TotPcs As Single, TotAmt As Single

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotBms = 0
        TotPcs = 0
        TotMtrs = 0
        With dgv_PavuDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(5).Value) <> 0 Then
                    If .Columns(14).Visible = True Then
                        TotBms = Val(TotBms) + Val(.Rows(i).Cells(14).Value)
                    Else
                        TotBms = TotBms + 1
                    End If

                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(5).Value)
                    TotPcs = TotPcs + Val(.Rows(i).Cells(3).Value)
                    TotAmt = TotAmt + Val(.Rows(i).Cells(13).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotBms)
            .Rows(0).Cells(3).Value = Val(TotPcs)
            .Rows(0).Cells(5).Value = Format(Val(TotMtrs), "########0.000")
            .Rows(0).Cells(13).Value = Format(Val(TotAmt), "########0.00")
        End With


        'lbl_GrossAmount.Text = Format(Val(txt_RateMeters.Text) * (Val(dgv_PavuDetails_Total.Rows(0).Cells(5).Value) + Val(txt_KuraiPavuMeter.Text)), "##########0.00")
        lbl_GrossAmount.Text = Format(Val(TotAmt) + (Val(txt_RateMeters.Text) * Val(txt_KuraiPavuMeter.Text)), "##########0.00")
        'lbl_NetAmount.Text = Format(Val(lbl_GrossAmount.Text) + Val(txt_AddLess.Text), "########0.00")

        NetAmount_Calculation()

    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        EMAIL_Status = False
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize
        Dim Def_PrntrNm As String = ""
        Dim vFILNm As String = ""
        Dim vFLPATH As String = ""
        Dim vPDFFLPATH_and_NAME As String = ""
        Dim pkInstalledPrinters As String
        Dim vPRNTRNAME As String


        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Pavu_Sales_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.* , d.EndsCount_Name , e.Ledger_Name as Trasport_Name from Pavu_Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.ReceivedFrom_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo  where a.Pavu_Sales_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                If EMAIL_Status = False Then
                    MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                End If

                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        prn_TotCopies = 1
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Sizing (Somanur)
            prn_TotCopies = Val(InputBox("Enter No.of Copies", "FOR SALES PRINTING...", "2"))
            If Val(prn_TotCopies) <= 0 Then
                Exit Sub
            End If
        End If


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

                    vFLPATH = Common_Procedures.AppPath & "\PDF"
                    If System.IO.Directory.Exists(vFLPATH) = False Then
                        System.IO.Directory.CreateDirectory(vFLPATH)
                    End If



                    '----Find all printers installed
                    vPRNTRNAME = ""
                    If EMAIL_Status = True Then
                        For Each pkInstalledPrinters In PrinterSettings.InstalledPrinters
                            If InStr(1, Trim(UCase(pkInstalledPrinters)), Trim(UCase("Microsoft"))) > 0 And InStr(1, Trim(UCase(pkInstalledPrinters)), Trim(UCase("Print"))) > 0 And InStr(1, Trim(UCase(pkInstalledPrinters)), Trim(UCase("PDF"))) > 0 Then
                                vPRNTRNAME = Trim(pkInstalledPrinters)
                                Exit For
                            End If
                            'Debug.Print(pkInstalledPrinters)
                        Next pkInstalledPrinters
                    End If

                    If Trim(vPRNTRNAME) = "" Then
                        For Each pkInstalledPrinters In PrinterSettings.InstalledPrinters
                            If InStr(1, Trim(UCase(pkInstalledPrinters)), Trim(UCase("doPDF"))) > 0 Then
                                vPRNTRNAME = Trim(pkInstalledPrinters)
                                Exit For
                            End If
                            'Debug.Print(pkInstalledPrinters)
                        Next pkInstalledPrinters
                    End If

                    If Trim(vPRNTRNAME) = "" Then
                        MessageBox.Show("PDF printing driver not installed", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If

                    Def_PrntrNm = PrintDocument1.PrinterSettings.PrinterName

                    vFILNm = Trim("PavuSales_" & Trim(lbl_InvNo.Text) & ".pdf")
                    vFILNm = StrConv(vFILNm, vbProperCase)
                    vPDFFLPATH_and_NAME = Trim(vFLPATH) & "\" & Trim(vFILNm)
                    vEMAIL_Attachment_FileName = Trim(vPDFFLPATH_and_NAME)

                    PrintDocument1.DocumentName = Trim(vFILNm)
                    PrintDocument1.PrinterSettings.PrinterName = Trim(vPRNTRNAME)    ' "Microsoft Print to PDF"
                    'PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintToFile = True
                    PrintDocument1.PrinterSettings.PrintFileName = Trim(vPDFFLPATH_and_NAME)
                    'PrintDocument1.PrinterSettings.PrintFileName = "c:\Statement.pdf"
                    PrintDocument1.Print()

                    'Debug.Print(PrintDocument1.PrinterSettings.PrintFileName)

                    PrintDocument1.PrinterSettings.PrinterName = Trim(Def_PrntrNm)


                    ''--This is actual & correct 
                    'Def_PrntrNm = PrintDocument1.PrinterSettings.PrinterName
                    'PrintDocument1.DocumentName = "Statement"
                    'PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    'PrintDocument1.PrinterSettings.PrintFileName = "c:\Statement.pdf"
                    'PrintDocument1.Print()


                Else

                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then

                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    'e.PageSettings.PaperSize = ps
                                    Exit For
                                End If
                            Next

                            PrintDocument1.Print()

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

                        PrintDocument1.Print()

                    End If

                End If


            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1.0

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub
    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Dim NewCode As String


        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        Dt1.Clear()
        prn_total_mtr = 0
        prn_DetIndx = 0
        prn_PageNo = 0
        prn_NoofBmDets = 0
        prn_DetMxIndx = 0
        prn_Count = 0
        Erase prn_DetAr

        prn_DetAr = New String(50, 20) {}

        Try

            '    da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , f.* ,c.Ledger_Name as Receiver_Name , d.EndsCount_Name , d.Ends_Name, ch.Count_Name, igh.Item_HSN_Code as HSN_CODE , e.Ledger_Name  as Trasport_Name  , f.*,CSH.State_Name as Company_State_Name  ,CSH.State_Code as Company_State_Code ,LSH.State_Name as Ledger_State_Name ,LSH.State_Code as Ledger_State_Code  from Pavu_Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN State_HEad LSH on B.Ledger_State_IdNo = LSH.State_IdNo LEFT OUTER JOIN Ledger_Head c ON a.ReceivedFrom_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo  LEFT OUTER JOIN Count_Head ch ON d.count_IdNo = ch.count_IdNo  LEFT OUTER JOIN ItemGroup_Head igh ON ch.ItemGroup_IdNo = IGH.ItemGroup_IdNo  LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo LEFT OUTER JOIN State_HEad CSH on F.Company_State_IdNo = CSH.State_IdNo  where a.Pavu_Sales_Code = '" & Trim(NewCode) & "'", con)
            ' da1 = New SqlClient.SqlDataAdapter("Select  a.*, b.* , f.* , g.* ,c.Ledger_Name as Receiver_Name , d.EndsCount_Name , d.Ends_Name, ch.Count_Name, igh.Item_HSN_Code as HSN_CODE , e.Ledger_Name  as Trasport_Name  , f.*,CSH.State_Name as Company_State_Name  ,CSH.State_Code as Company_State_Code ,LSH.State_Name as Ledger_State_Name ,LSH.State_Code as Ledger_State_Code ,g.Ledger_Name as ShippedTo_Name, g.Ledger_Address1 as Shipped_address1,g.Ledger_Address2 as Shipped_address2,g.Ledger_Address3 as Shipped_address3,g.Ledger_Address4 as Shipped_address4 ,g.Ledger_PhoneNo as Shipped_phoneNo,g.Ledger_GSTinNo as Shipped_GSTinNo from Pavu_Sales_Head a LEFT OUTER JOIN Ledger_Head b  ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN State_HEad LSH  on B.Ledger_State_IdNo = LSH.State_IdNo LEFT OUTER JOIN Ledger_Head c   ON a.ReceivedFrom_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo  LEFT OUTER JOIN Count_Head ch  ON d.count_IdNo = ch.count_IdNo  LEFT OUTER JOIN ItemGroup_Head igh ON ch.ItemGroup_IdNo = IGH.ItemGroup_IdNo  LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo INNER JOIN Company_Head f   ON a.Company_IdNo = f.Company_IdNo LEFT OUTER JOIN State_HEad CSH  on F.Company_State_IdNo = CSH.State_IdNo LEFT OUTER JOIN Ledger_Head g ON a.ShippedTo_IdNo = g.Ledger_IdNo where a.Pavu_Sales_Code = '" & Trim(NewCode) & "'", con)


            da1 = New SqlClient.SqlDataAdapter("Select  a.*, b.* , f.* , g.* ,c.Ledger_Name as Receiver_Name , d.EndsCount_Name , d.Ends_Name, ch.Count_Name, igh.Item_HSN_Code as HSN_CODE , e.Ledger_Name  as Trasport_Name  , f.*,CSH.State_Name as Company_State_Name  ,CSH.State_Code as Company_State_Code ,LSH.State_Name as Ledger_State_Name ,LSH.State_Code as Ledger_State_Code ,g.Ledger_Name as ShippedTo_Name, g.Ledger_Address1 as Shipped_address1,g.Ledger_Address2 as Shipped_address2,g.Ledger_Address3 as Shipped_address3,g.Ledger_Address4 as Shipped_address4 ,g.Ledger_PhoneNo as Shipped_phoneNo,g.Ledger_GSTinNo as Shipped_GSTinNo , SS.State_Name as Shipped_State_Name ,SS.State_Code as Shipped_State_Code  from Pavu_Sales_Head a LEFT OUTER JOIN Ledger_Head b   ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN State_HEad LSH    on B.Ledger_State_IdNo = LSH.State_IdNo LEFT OUTER JOIN Ledger_Head c      ON a.ReceivedFrom_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d 	ON a.EndsCount_IdNo = d.EndsCount_IdNo  LEFT OUTER JOIN Count_Head ch  	ON d.count_IdNo = ch.count_IdNo  LEFT OUTER JOIN ItemGroup_Head igh 	ON ch.ItemGroup_IdNo = IGH.ItemGroup_IdNo  LEFT OUTER JOIN Ledger_Head e 	ON a.Transport_IdNo = e.Ledger_IdNo INNER JOIN Company_Head f   	ON a.Company_IdNo = f.Company_IdNo LEFT OUTER JOIN State_HEad CSH 	 on F.Company_State_IdNo = CSH.State_IdNo LEFT OUTER JOIN Ledger_Head g ON a.ShippedTo_IdNo = g.Ledger_IdNo LEFT OUTER JOIN State_HEad SS    on G.Ledger_State_IdNo = SS.State_IdNo  where a.Pavu_Sales_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* , d.EndsCount_Name, d.Ends_Name, ch.Count_Name, igh.Item_HSN_Code as HSN_CODE from Pavu_Sales_Details a LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_idno = d.EndsCount_idno LEFT OUTER JOIN Count_Head ch ON d.count_IdNo = ch.count_IdNo  LEFT OUTER JOIN ItemGroup_Head igh ON ch.ItemGroup_IdNo = IGH.ItemGroup_IdNo where Pavu_Sales_Code ='" & Trim(NewCode) & "'   Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count >= 0 Then

                    'da3 = New SqlClient.SqlDataAdapter("select  a.Rate, d.EndsCount_Name, d.Ends_Name, ch.Count_Name, igh.Item_HSN_Code as HSN_CODE,sum (a.meters) as meters from Pavu_Sales_Details a LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_idno = d.EndsCount_idno LEFT OUTER JOIN Count_Head ch ON d.count_IdNo = ch.count_IdNo  LEFT OUTER JOIN ItemGroup_Head igh ON ch.ItemGroup_IdNo = IGH.ItemGroup_IdNo where Pavu_Sales_Code = '" & Trim(NewCode) & "'   group by d.EndsCount_Name, d.Ends_Name, ch.Count_Name, igh.Item_HSN_Code,a.Rate  Order by d.EndsCount_Name, d.Ends_Name, ch.Count_Name, igh.Item_HSN_Code,a.RAte", con)
                    'Dt1 = New DataTable
                    'da3.Fill(Dt1)

                    ' If Dt1.Rows.Count > 0 Then

                    For i = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(i).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(i).Item("EndsCount_Name").ToString, 15))
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Set_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = Trim(prn_DetDt.Rows(i).Item("Beam_No").ToString)
                            ' prn_DetAr(prn_DetMxIndx, 4) = Trim(prn_HdDt.Rows(i).Item("Description").ToString)

                            prn_DetAr(prn_DetMxIndx, 4) = Trim(prn_DetDt.Rows(i).Item("Pcs").ToString)

                            prn_total_mtr = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
                            prn_DetAr(prn_DetMxIndx, 5) = Format(Val(prn_total_mtr), "#########0.00")

                            'prn_DetAr(prn_DetMxIndx, 6) = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString), "#########0.00")
                            'prn_DetAr(prn_DetMxIndx, 7) = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString) * (Val(prn_DetDt.Rows(i).Item("Meters").ToString)), "#########0.00")
                            prn_DetAr(prn_DetMxIndx, 6) = Format(Val(prn_DetDt.Rows(i).Item("Rate").ToString), "#########0.00")
                            prn_DetAr(prn_DetMxIndx, 7) = Format(Val(prn_DetDt.Rows(i).Item("AMOUNT").ToString), "#########0.00")  ' Format(Val(prn_DetDt.Rows(i).Item("Rate").ToString) * (Val(prn_DetDt.Rows(i).Item("Meters").ToString)), "#########0.00")

                            prn_DetAr(prn_DetMxIndx, 8) = Trim(prn_DetDt.Rows(i).Item("Sl_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 9) = Trim(prn_DetDt.Rows(i).Item("HSN_CODE").ToString)

                            prn_DetAr(prn_DetMxIndx, 10) = Trim(prn_DetDt.Rows(i).Item("Ends_Name").ToString)
                            If Trim(prn_HdDt.Rows(0).Item("Description").ToString) <> "" Then
                                prn_DetAr(prn_DetMxIndx, 10) = Trim(prn_DetAr(prn_DetMxIndx, 10)) & "-" & Trim(prn_HdDt.Rows(0).Item("Description").ToString)
                            End If

                            prn_DetAr(prn_DetMxIndx, 11) = Trim(prn_DetDt.Rows(i).Item("Count_Name").ToString)

                            prn_DetAr(prn_DetMxIndx, 12) = prn_DetDt.Rows(i).Item("Noof_Beams").ToString

                        End If



                    Next i

                    'If Dt1.Rows.Count > 0 Then
                    '    For i = 0 To Dt1.Rows.Count - 1
                    '        If Val(Dt1.Rows(i).Item("Meters").ToString) <> 0 Then
                    '            prn_DetMxIndx = prn_DetMxIndx + 1
                    '            'prn_DetAr(prn_DetMxIndx, 1) = Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(i).Item("EndsCount_Name").ToString, 15))
                    '            ' prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Set_No").ToString)
                    '            ' prn_DetAr(prn_DetMxIndx, 3) = Trim(prn_DetDt.Rows(i).Item("Beam_No").ToString)
                    '            ' prn_DetAr(prn_DetMxIndx, 4) = Val(prn_DetDt.Rows(i).Item("Pcs").ToString)
                    '            prn_DetAr(prn_DetMxIndx, 5) = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    '            'prn_DetAr(prn_DetMxIndx, 6) = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString), "#########0.00")
                    '            'prn_DetAr(prn_DetMxIndx, 7) = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString) * (Val(prn_DetDt.Rows(i).Item("Meters").ToString)), "#########0.00")
                    '            prn_DetAr(prn_DetMxIndx, 6) = Format(Val(Dt1.Rows(i).Item("Rate").ToString), "#########0.00")
                    '            prn_DetAr(prn_DetMxIndx, 7) = Format(Val(Dt1.Rows(i).Item("Rate").ToString) * (Val(Dt1.Rows(i).Item("Meters").ToString)), "#########0.00")

                    '            'prn_DetAr(prn_DetMxIndx, 8) = Trim(prn_DetDt.Rows(i).Item("Sl_No").ToString)
                    '            'prn_DetAr(prn_DetMxIndx, 9) = Trim(prn_DetDt.Rows(i).Item("HSN_CODE").ToString)
                    '            'prn_DetAr(prn_DetMxIndx, 10) = Trim(prn_DetDt.Rows(i).Item("Ends_Name").ToString)
                    '            'prn_DetAr(prn_DetMxIndx, 11) = Trim(prn_DetDt.Rows(i).Item("Count_Name").ToString)

                    '        End If

                    '    Next i
                    'End If

                    If Trim(prn_HdDt.Rows(0).Item("EndsCount_Name").ToString) <> "" And Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString) <> 0 Then

                        prn_DetMxIndx = prn_DetMxIndx + 1
                        prn_DetAr(prn_DetMxIndx, 1) = Trim(Microsoft.VisualBasic.Left(prn_HdDt.Rows(0).Item("EndsCount_Name").ToString, 15))
                        prn_DetAr(prn_DetMxIndx, 2) = ""
                        prn_DetAr(prn_DetMxIndx, 3) = ""
                        prn_DetAr(prn_DetMxIndx, 4) = ""
                        prn_DetAr(prn_DetMxIndx, 5) = Format(Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString), "#########0.00")
                        prn_DetAr(prn_DetMxIndx, 6) = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString), "#########0.00")
                        prn_DetAr(prn_DetMxIndx, 7) = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString) * (Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString)), "#########0.00")
                        prn_DetAr(prn_DetMxIndx, 8) = prn_DetMxIndx
                        prn_DetAr(prn_DetMxIndx, 9) = Trim(prn_HdDt.Rows(0).Item("HSN_CODE").ToString)
                        prn_DetAr(prn_DetMxIndx, 10) = Trim(prn_HdDt.Rows(0).Item("Ends_Name").ToString & "-" & Trim(prn_HdDt.Rows(0).Item("Description").ToString))
                        prn_DetAr(prn_DetMxIndx, 11) = Trim(prn_HdDt.Rows(0).Item("Count_Name").ToString)
                        prn_DetAr(prn_DetMxIndx, 12) = prn_HdDt.Rows(0).Item("empty_beam").ToString

                    End If
                    'End If

                Else

                    MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End If
            End If

            da1.Dispose()


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    'Private Sub PrintDocument1_BeginPrint_1111(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim da3 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable

    '    Dim NewCode As String


    '    NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    prn_HdDt.Clear()
    '    prn_DetDt.Clear()
    '    Dt1.Clear()
    '    prn_total_mtr = 0
    '    prn_DetIndx = 0
    '    prn_PageNo = 0
    '    prn_NoofBmDets = 0
    '    prn_DetMxIndx = 0
    '    prn_Count = 0
    '    Erase prn_DetAr

    '    prn_DetAr = New String(50, 20) {}

    '    Try

    '        da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.Ledger_Name as Receiver_Name , d.EndsCount_Name , d.Ends_Name, ch.Count_Name, igh.Item_HSN_Code as HSN_CODE , e.Ledger_Name  as Trasport_Name  , f.*,CSH.State_Name as Company_State_Name  ,CSH.State_Code as Company_State_Code ,LSH.State_Name as Ledger_State_Name ,LSH.State_Code as Ledger_State_Code from Pavu_Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN State_HEad LSH on B.Ledger_State_IdNo = LSH.State_IdNo LEFT OUTER JOIN Ledger_Head c ON a.ReceivedFrom_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo  LEFT OUTER JOIN Count_Head ch ON d.count_IdNo = ch.count_IdNo  LEFT OUTER JOIN ItemGroup_Head igh ON ch.ItemGroup_IdNo = IGH.ItemGroup_IdNo  LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo LEFT OUTER JOIN State_HEad CSH on F.Company_State_IdNo = CSH.State_IdNo  where a.Pavu_Sales_Code = '" & Trim(NewCode) & "'", con)
    '        prn_HdDt = New DataTable
    '        da1.Fill(prn_HdDt)

    '        If prn_HdDt.Rows.Count > 0 Then

    '            da2 = New SqlClient.SqlDataAdapter("select a.* , d.EndsCount_Name, d.Ends_Name, ch.Count_Name, igh.Item_HSN_Code as HSN_CODE from Pavu_Sales_Details a LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_idno = d.EndsCount_idno LEFT OUTER JOIN Count_Head ch ON d.count_IdNo = ch.count_IdNo  LEFT OUTER JOIN ItemGroup_Head igh ON ch.ItemGroup_IdNo = IGH.ItemGroup_IdNo where Pavu_Sales_Code ='" & Trim(NewCode) & "'   Order by a.Sl_No", con)
    '            prn_DetDt = New DataTable
    '            da2.Fill(prn_DetDt)

    '            If prn_DetDt.Rows.Count >= 0 Then

    '                da3 = New SqlClient.SqlDataAdapter("select a.Rate, d.EndsCount_Name, d.Ends_Name, ch.Count_Name, igh.Item_HSN_Code as HSN_CODE,sum (a.meters) as meters from Pavu_Sales_Details a LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_idno = d.EndsCount_idno LEFT OUTER JOIN Count_Head ch ON d.count_IdNo = ch.count_IdNo  LEFT OUTER JOIN ItemGroup_Head igh ON ch.ItemGroup_IdNo = IGH.ItemGroup_IdNo where Pavu_Sales_Code = '" & Trim(NewCode) & "'   group by d.EndsCount_Name, d.Ends_Name, ch.Count_Name, igh.Item_HSN_Code,a.Rate  Order by d.EndsCount_Name, d.Ends_Name, ch.Count_Name, igh.Item_HSN_Code,a.RAte", con)
    '                Dt1 = New DataTable
    '                da3.Fill(Dt1)


    '                ' If Dt1.Rows.Count > 0 Then

    '                For i = 0 To Dt1.Rows.Count - 1
    '                    If Val(Dt1.Rows(i).Item("Meters").ToString) <> 0 Then
    '                        prn_DetMxIndx = prn_DetMxIndx + 1
    '                        prn_DetAr(prn_DetMxIndx, 1) = Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(i).Item("EndsCount_Name").ToString, 15))
    '                        prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Set_No").ToString)
    '                        prn_DetAr(prn_DetMxIndx, 3) = Trim(prn_DetDt.Rows(i).Item("Beam_No").ToString)
    '                        ' prn_DetAr(prn_DetMxIndx, 4) = Trim(prn_HdDt.Rows(i).Item("Description").ToString)
    '                        prn_total_mtr = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
    '                        prn_DetAr(prn_DetMxIndx, 5) = Format(Val(prn_total_mtr), "#########0.00")

    '                        'prn_DetAr(prn_DetMxIndx, 6) = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString), "#########0.00")
    '                        'prn_DetAr(prn_DetMxIndx, 7) = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString) * (Val(prn_DetDt.Rows(i).Item("Meters").ToString)), "#########0.00")
    '                        prn_DetAr(prn_DetMxIndx, 6) = Format(Val(prn_DetDt.Rows(i).Item("Rate").ToString), "#########0.00")
    '                        prn_DetAr(prn_DetMxIndx, 7) = Format(Val(prn_DetDt.Rows(i).Item("Rate").ToString) * (Val(prn_DetDt.Rows(i).Item("Meters").ToString)), "#########0.00")

    '                        prn_DetAr(prn_DetMxIndx, 8) = Trim(prn_DetDt.Rows(i).Item("Sl_No").ToString)
    '                        prn_DetAr(prn_DetMxIndx, 9) = Trim(prn_DetDt.Rows(i).Item("HSN_CODE").ToString)
    '                        prn_DetAr(prn_DetMxIndx, 10) = Trim(prn_DetDt.Rows(i).Item("Ends_Name").ToString & "-" & Trim(prn_HdDt.Rows(0).Item("Description").ToString))
    '                        prn_DetAr(prn_DetMxIndx, 11) = Trim(prn_DetDt.Rows(i).Item("Count_Name").ToString)

    '                    End If

    '                Next i
    '                'If Dt1.Rows.Count > 0 Then
    '                '    For i = 0 To Dt1.Rows.Count - 1
    '                '        If Val(Dt1.Rows(i).Item("Meters").ToString) <> 0 Then
    '                '            prn_DetMxIndx = prn_DetMxIndx + 1
    '                '            'prn_DetAr(prn_DetMxIndx, 1) = Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(i).Item("EndsCount_Name").ToString, 15))
    '                '            ' prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Set_No").ToString)
    '                '            ' prn_DetAr(prn_DetMxIndx, 3) = Trim(prn_DetDt.Rows(i).Item("Beam_No").ToString)
    '                '            ' prn_DetAr(prn_DetMxIndx, 4) = Val(prn_DetDt.Rows(i).Item("Pcs").ToString)
    '                '            prn_DetAr(prn_DetMxIndx, 5) = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
    '                '            'prn_DetAr(prn_DetMxIndx, 6) = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString), "#########0.00")
    '                '            'prn_DetAr(prn_DetMxIndx, 7) = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString) * (Val(prn_DetDt.Rows(i).Item("Meters").ToString)), "#########0.00")
    '                '            prn_DetAr(prn_DetMxIndx, 6) = Format(Val(Dt1.Rows(i).Item("Rate").ToString), "#########0.00")
    '                '            prn_DetAr(prn_DetMxIndx, 7) = Format(Val(Dt1.Rows(i).Item("Rate").ToString) * (Val(Dt1.Rows(i).Item("Meters").ToString)), "#########0.00")

    '                '            'prn_DetAr(prn_DetMxIndx, 8) = Trim(prn_DetDt.Rows(i).Item("Sl_No").ToString)
    '                '            'prn_DetAr(prn_DetMxIndx, 9) = Trim(prn_DetDt.Rows(i).Item("HSN_CODE").ToString)
    '                '            'prn_DetAr(prn_DetMxIndx, 10) = Trim(prn_DetDt.Rows(i).Item("Ends_Name").ToString)
    '                '            'prn_DetAr(prn_DetMxIndx, 11) = Trim(prn_DetDt.Rows(i).Item("Count_Name").ToString)

    '                '        End If

    '                '    Next i
    '                'End If

    '                If Trim(prn_HdDt.Rows(0).Item("EndsCount_Name").ToString) <> "" And Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString) <> 0 Then

    '                    prn_DetMxIndx = prn_DetMxIndx + 1
    '                    prn_DetAr(prn_DetMxIndx, 1) = Trim(Microsoft.VisualBasic.Left(prn_HdDt.Rows(0).Item("EndsCount_Name").ToString, 15))
    '                    prn_DetAr(prn_DetMxIndx, 2) = ""
    '                    prn_DetAr(prn_DetMxIndx, 3) = ""
    '                    prn_DetAr(prn_DetMxIndx, 4) = ""
    '                    prn_DetAr(prn_DetMxIndx, 5) = Format(Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString), "#########0.00")
    '                    prn_DetAr(prn_DetMxIndx, 6) = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString), "#########0.00")
    '                    prn_DetAr(prn_DetMxIndx, 7) = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString) * (Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString)), "#########0.00")
    '                    prn_DetAr(prn_DetMxIndx, 8) = prn_DetMxIndx
    '                    prn_DetAr(prn_DetMxIndx, 9) = Trim(prn_HdDt.Rows(0).Item("HSN_CODE").ToString)
    '                    prn_DetAr(prn_DetMxIndx, 10) = Trim(prn_HdDt.Rows(0).Item("Ends_Name").ToString & "-" & Trim(prn_HdDt.Rows(0).Item("Description").ToString))
    '                    prn_DetAr(prn_DetMxIndx, 11) = Trim(prn_HdDt.Rows(0).Item("Count_Name").ToString)

    '                End If
    '                'End If

    '            Else

    '                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '            End If
    '        End If

    '        da1.Dispose()


    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1123" Then
            'Printing_Format3_GST(e)
            Printing_GST_Format_1123(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then
            Printing_GST_Format11(e)
        Else
            Printing_Format2_GST(e)
            'Printing_Format1GST(e)
        End If
    End Sub

    Private Sub Printing_Format1GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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

        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    'Debug.Print(ps.PaperName)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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

        NoofItems_PerPage = 15

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 30 : ClArr(2) = 150 : ClArr(3) = 90 : ClArr(4) = 90 : ClArr(5) = 65 : ClArr(6) = 90 : ClArr(7) = 80
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_NoofBmDets < prn_DetMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetIndx = prn_DetIndx + 1

                        CurY = CurY + TxtHgt

                        If Val(prn_DetAr(prn_DetIndx, 5)) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 8)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 4))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 5)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 7)), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        'If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                        '    prn_NoofBmDets = prn_NoofBmDets + 1

                        'End If

                        NoofDets = NoofDets + 1

                    Loop

                End If




                Printing_Format1GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format1GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font

        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurY1 As Single = 0, CurX As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin


        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = "HO : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = "BO : " & prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else
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
            If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
                Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
            End If


        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1313" Then '--------- sri guru
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.sri_Guru_Fabrics_logo, Drawing.Image), LMargin + 20, CurY + 5, 115, 102)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1315" Then '--------- KR Tex
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KR_Logo, Drawing.Image), LMargin + 20, CurY + 5, 115, 102)
        End If



        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)


        If Common_Procedures.settings.CustomerCode <> "1313" And Common_Procedures.settings.CustomerCode <> "1315" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        If Common_Procedures.settings.CustomerCode = "1313" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Sri_Guru_Fabrics_Company, Drawing.Image), LMargin + 220, CurY, 280, 100)
            CurY = CurY + TxtHgt + 53
        ElseIf Common_Procedures.settings.CustomerCode = "1315" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KR_Company_Logo, Drawing.Image), LMargin + 280, CurY, 180, 97)
            CurY = CurY + TxtHgt + 53
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strHeight - 7
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        'End If

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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt + 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("INVOICE DATE  :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Invoice.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then
        If prn_HdDt.Rows(0).Item("PavuSale_PrefixNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PavuSale_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)
        End If
        'End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Sales_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No ", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString), LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If

        'CurY = CurY + TxtHgt + 10
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(6) = CurY
        'e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(6), LMargin + M1, LnAr(2))

        'CurY = CurY + TxtHgt - 10

        'Common_Procedures.Print_To_PrintDocument(e, "Ends Count ", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.EndsCount_IdNoToName(con, (prn_HdDt.Rows(0).Item("EndsCount_IdNo").ToString)), LMargin + W1 + 25, CurY, 0, 0, pFont)


        'If Trim(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Pavu Meters", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Pavu_Meters").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        'End If

        'If Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> "" Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "Pavu Beams ", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + W1 + 25, CurY, 0, 0, pFont)
        'End If
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))


        'e.Graphics.DrawLine(Pens.Black, LMargin + M1 + 4, LnAr(3), LMargin + M1 + 4, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format1GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p3Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        '  Dim m1 As Integer
        Dim C1 As Single, Amt As Single = 0
        Dim BmsInWrds As String
        Dim TotMtrs As Single = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            With dgv_PavuDetails
                For I = 0 To .RowCount - 1
                    TotMtrs = TotMtrs + Val(.Rows(I).Cells(5).Value())
                Next
            End With
            Amt = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString) * Val(TotMtrs), "##########0.00")

            CurY = CurY + TxtHgt - 10


            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then


                    'If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    'End If

                    'If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    'End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(Amt) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    End If
                    'Else

                    '    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                    '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    '    End If
                    '    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                    '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    '    End If
                    '    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                    '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    '    End If

                End If

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
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            ' CurY = CurY + TxtHgt
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10
            'CurY = CurY + TxtHgt
            'If Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Gross Amount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    'If is_LastPage = True Then
            '    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '    ' End If
            'End If

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







            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL BEAMS : " & Val((prn_HdDt.Rows(0).Item("Empty_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString)), LMargin + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Add_Less").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                'If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(0).Item("Add_Less").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                ' End If
            End If

            CurY = CurY + TxtHgt
            p3Font = New Font("Calibri", 10, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then
                CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 10, CurY, 0, 0, p1Font)
            End If
            If BankNm1 <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p3Font)
            End If
            If BankNm2 <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p3Font)
            End If
            If BankNm3 <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p3Font)
            End If
            If BankNm4 <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p3Font)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(prn_HdDt.Rows(0).Item("IGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If
            'CurY = CurY + TxtHgt



            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + C1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, PageWidth, CurY)
            'LnAr(8) = CurY

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

            'Common_Procedures.Print_To_PrintDocument(e, "Rate/Meters ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)


            'If Val(prn_HdDt.Rows(0).Item("Add_Less").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt + 5
            '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)
            '    CurY = CurY + TxtHgt + 5
            '    Common_Procedures.Print_To_PrintDocument(e, "Gross Amount ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 20, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            '    CurY = CurY + TxtHgt + 10
            '    Common_Procedures.Print_To_PrintDocument(e, "Add/Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 20, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Add_Less").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            'Else
            '    CurY = CurY + TxtHgt + 25
            'End If

            'CurY = CurY + TxtHgt + 5
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)

            'CurY = CurY + TxtHgt + 5
            'Common_Procedures.Print_To_PrintDocument(e, "Net Amount ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)



            'CurY = CurY + TxtHgt + 5
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(StrConv(BmsInWrds, vbProperCase)), "", "")
            Common_Procedures.Print_To_PrintDocument(e, "Rupees   : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'LnAr(7) = CurY
            'Common_Procedures.Print_To_PrintDocument(e, "Received The Beams As Per Above Details.", LMargin + 10, CurY + 5, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)


            CurY = CurY + TxtHgt + 50
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Led_IdNo As Integer
        Dim NewCode As String

        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)
        If Led_IdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            chk_SelectAll.Checked = False

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.noof_used as Ent_NoofUsed, b.*, c.EndsCount_Name, d.Beam_Width_Name from Pavu_Sales_Details a INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON b.Beam_Width_Idno = d.Beam_Width_Idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Pavu_Sales_Code = '" & Trim(NewCode) & "' and a.ReceivedFrom_IdNo = " & Str(Val(Led_IdNo)) & " order by a.for_orderby, a.Set_Code, b.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Meters_Pc").ToString)
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Ent_NoofUsed").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Pavu_Delivery_Increment").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        If Val(.Rows(n).Cells(9).Value) <> Val(.Rows(n).Cells(11).Value) Then
                            .Rows(i).Cells(j).Style.BackColor = Color.LightGray
                        End If
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name, c.Beam_Width_Name from Stock_SizedPavu_Processing_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head c ON a.Beam_Width_Idno = c.Beam_Width_Idno where a.Pavu_Delivery_Code = '' and a.Beam_Knotting_Code = '' and a.Close_Status = 0 and a.StockAt_IdNo = " & Str(Val(Led_IdNo)) & " order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Meters_Pc").ToString)
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = "-9999"
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Pavu_Delivery_Increment").ToString

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        If dgv_Selection.Rows.Count > 0 Then
            dgv_Selection.Focus()
            dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        End If

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Pavu(e.RowIndex)
    End Sub

    Private Sub Select_Pavu(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                If Val(.Rows(RwIndx).Cells(9).Value) > 0 Then
                    If Val(.Rows(RwIndx).Cells(9).Value) <> Val(.Rows(RwIndx).Cells(11).Value) Then
                        MessageBox.Show("Cannot deselect" & Chr(13) & "Already this pavu delivered to others")
                        Exit Sub
                    End If
                End If

                .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(8).Value) = 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(8).Value = ""
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next

        With dgv_Selection

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If .CurrentCell.RowIndex >= 0 Then
                    Select_Pavu(.CurrentCell.RowIndex)
                    e.Handled = True
                End If
            End If

            If e.KeyCode = Keys.Back Or e.KeyCode = Keys.Delete Then
                If .CurrentCell.RowIndex >= 0 Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(8).Value) = 1 Then
                        Select_Pavu(.CurrentCell.RowIndex)
                        e.Handled = True
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim n As Integer
        Dim sno As Integer

        With dgv_PavuDetails

            .Rows.Clear()

            For i = 0 To dgv_Selection.RowCount - 1

                If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(0).Value = Val(sno)
                    .Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                    .Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                    .Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                    .Rows(n).Cells(4).Value = Format(Val(dgv_Selection.Rows(i).Cells(4).Value), "#########0.00")
                    .Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(5).Value
                    .Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
                    .Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(7).Value

                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = ""

                    If Val(dgv_Selection.Rows(i).Cells(9).Value) > 0 Then

                        If Val(dgv_Selection.Rows(i).Cells(9).Value) <> Val(dgv_Selection.Rows(i).Cells(11).Value) Then
                            .Rows(n).Cells(8).Value = "1"
                        Else
                            .Rows(n).Cells(8).Value = ""
                        End If

                        .Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(9).Value

                    End If

                    .Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(10).Value
                    .Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(11).Value

                End If

            Next

        End With

        TotalPavu_Calculation()
        NetAmount_Calculation()

        Grid_Cell_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_SalesAc.Enabled And cbo_SalesAc.Visible Then cbo_SalesAc.Focus()

    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        'With dgv_PavuDetails
        '    vcbo_KeyDwnVal = e.KeyValue
        '    If .Visible Then
        '        If e.KeyValue = Keys.Delete Then
        '            If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
        '                e.Handled = True
        '                e.SuppressKeyPress = True
        '            End If

        '            If .CurrentCell.ColumnIndex >= 1 Then
        '                If Trim(UCase(cbo_EntryType.Text)) = "FROM STOCK" Then
        '                    e.Handled = True
        '                    e.SuppressKeyPress = True
        '                End If
        '            End If
        '        End If
        '    End If
        'End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_PavuDetails
                If .Visible Then

                    If .Rows.Count > 0 Then

                        If .CurrentCell.ColumnIndex <= 7 And Trim(.Rows(.CurrentCell.RowIndex).Cells(16).Value) <> "" Then
                            e.Handled = True
                            Add_NewRow_ToGrid()
                        End If

                        If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                            If Trim(UCase(cbo_EntryType.Text)) = "FROM STOCK" Then
                                e.Handled = True
                            End If
                        End If

                        If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If
                        End If

                    End If

                End If
            End With

        Catch ex As Exception
            '---

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_PavuDetails_KeyUp(sender, e)
        End If
    End Sub

    Private Sub txt_SetNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SetNoSelection.KeyDown
        If (e.KeyValue = 40) Then
            txt_BeamNoSelection.Focus()
        End If
    End Sub

    Private Sub txt_SetNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SetNoSelection.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_BeamNoSelection.Focus()
        End If
    End Sub

    Private Sub txt_BeamNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BeamNoSelection.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
        If (e.KeyValue = 38) Then txt_SetNoSelection.Focus()
    End Sub

    Private Sub txt_BeamNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BeamNoSelection.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_BeamNoSelection.Text) <> "" Or Trim(txt_SetNoSelection.Text) <> "" Then
                btn_Set_Bm_selection_Click(sender, e)

            Else
                If dgv_Selection.Rows.Count > 0 Then
                    dgv_Selection.Focus()
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                    dgv_Selection.CurrentCell.Selected = True
                End If

            End If

        End If
    End Sub

    Private Sub btn_Set_Bm_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Set_Bm_selection.Click
        Dim LtNo As String
        Dim PcsNo As String
        Dim i As Integer

        If Trim(txt_SetNoSelection.Text) <> "" Or Trim(txt_BeamNoSelection.Text) <> "" Then

            LtNo = Trim(txt_SetNoSelection.Text)
            PcsNo = Trim(txt_BeamNoSelection.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) And Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                    Call Select_Pavu(i)

                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 11 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 10

                    Exit For

                End If
            Next

            txt_SetNoSelection.Text = ""
            txt_BeamNoSelection.Text = ""
            If txt_SetNoSelection.Enabled = True Then txt_SetNoSelection.Focus()

        End If
    End Sub

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(8).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_Pavu(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With

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

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
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

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged

        If IsDate(dtp_Date.Text) = True Then

            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
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

    Private Sub txt_RateMeters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RateMeters.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            txt_KuraiPavuMeter.Focus()
        End If

        If e.KeyCode = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            cbo_Transport.Focus()
        End If
    End Sub

    Private Sub txt_RateMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RateMeters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_RateMeters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_RateMeters.TextChanged
        TotalPavu_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub txt_AddLess_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLess.KeyDown
        If e.KeyValue = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            txt_Freight.Focus()
        End If

        If e.KeyValue = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            txt_Remarks.Focus()
            'If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_date.Focus()
            'End If
        End If

    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress

        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            txt_Remarks.Focus()

            'If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_date.Focus()
            'End If
        End If

    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub


    Private Sub txt_KuraiPavuMeter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_KuraiPavuMeter.TextChanged
        TotalPavu_Calculation()
    End Sub

    Private Sub txt_IGST_Perc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_IGST_Perc.KeyDown
        If e.KeyValue = 38 Then
            txt_SGST_Perc.Focus()
        End If

        If e.KeyValue = 40 Then
            txt_Remarks.Focus()
            'If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_date.Focus()
            'End If
        End If
    End Sub

    Private Sub txt_IGST_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_IGST_Perc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_Remarks.Focus()

            'If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_date.Focus()
            'End If
        End If
    End Sub

    Private Sub txt_CGST_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CGST_Perc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_SGST_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SGST_Perc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub GET_Tax_Percentage()
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As DataTable
        Dim vEndsCntNm As String = ""
        Dim vEndCnt_IdNo As Integer = 0
        Dim vLed_IdNo As Integer = 0
        Dim vGSTperc As String = 0
        Dim vInterStateStatus As Boolean = False

        vEndsCntNm = ""
        If Trim(cbo_EndsCount.Text) <> "" Then
            vEndsCntNm = Trim(cbo_EndsCount.Text)

        Else
            If dgv_PavuDetails.Rows.Count > 0 Then
                vEndsCntNm = Trim(dgv_PavuDetails.Rows(0).Cells(6).Value)
            End If

        End If

        vEndCnt_IdNo = Common_Procedures.EndsCount_NameToIdNo(con, vEndsCntNm)

        vGSTperc = 0
        Da1 = New SqlClient.SqlDataAdapter("select igh.* from EndsCount_Head d INNER JOIN Count_Head ch ON d.count_IdNo = ch.count_IdNo INNER JOIN ItemGroup_Head igh ON ch.ItemGroup_IdNo = IGH.ItemGroup_IdNo  where d.EndsCount_IdNo = " & Str(Val(vEndCnt_IdNo)), con)
        Dt1 = New DataTable
        da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Item_GST_Percentage").ToString) = False Then
                vGSTperc = Dt1.Rows(0).Item("Item_GST_Percentage").ToString
            End If
        End If
        Dt1.Clear()

        vLed_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_BillTo.Text)
        vInterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), vLed_IdNo)

        txt_CGST_Perc.Enabled = False
        txt_SGST_Perc.Enabled = False
        txt_IGST_Perc.Enabled = False

        If vInterStateStatus = True Then
            txt_CGST_Perc.Text = ""
            txt_SGST_Perc.Text = ""
            txt_IGST_Perc.Text = vGSTperc

        Else

            txt_CGST_Perc.Text = Format(Val(vGSTperc) / 2, "######0.00")
            txt_SGST_Perc.Text = Format(Val(vGSTperc) / 2, "######0.00")
            txt_IGST_Perc.Text = ""

        End If

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As String = ""
        Dim vStrNetAmt As String = ""
        Dim RndOff As String = ""
        Dim vTaxableAmt As String = 0

        If FrmLdSTS = True Then Exit Sub

        vTaxableAmt = Val(lbl_GrossAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)

        GET_Tax_Percentage()

        lbl_CGstAmount.Text = Format(Val(vTaxableAmt) * Val(txt_CGST_Perc.Text) / 100, "##########0.00")
        lbl_SGstAmount.Text = Format(Val(vTaxableAmt) * Val(txt_SGST_Perc.Text) / 100, "###########0.00")
        lbl_IGstAmount.Text = Format(Val(vTaxableAmt) * Val(txt_IGST_Perc.Text) / 100, "##########0.00")

        NtAmt = Val(vTaxableAmt) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "############0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        vStrNetAmt = Format(Val(NtAmt), "##########0.00")

        RndOff = Format(Val(CSng(lbl_NetAmount.Text)) - Val(vStrNetAmt), "#########0.00")

        txt_Roundoff.Text = RndOff

    End Sub

    Private Sub txt_CGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CGST_Perc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_IGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_IGST_Perc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_SGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SGST_Perc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub Printing_Format2_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

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

        NoofItems_PerPage = 12 '15

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 30 : ClArr(2) = 140 : ClArr(3) = 70 : ClArr(4) = 80 : ClArr(5) = 80 : ClArr(6) = 55 : ClArr(7) = 80 : ClArr(8) = 70
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 17.5 '18 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Or Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString) <> 0 Then

                    Do While prn_NoofBmDets < prn_DetMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format2_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetIndx = prn_DetIndx + 1

                        CurY = CurY + TxtHgt

                        If Val(prn_DetAr(prn_DetIndx, 5)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 8)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 9))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                            If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 4))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 5)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 7)), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        NoofDets = NoofDets + 1

                    Loop

                End If

                Printing_Format2_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


                If Trim(prn_InpOpts) <> "" Then
                    If prn_Count < Len(Trim(prn_InpOpts)) Then


                        If Val(prn_InpOpts) <> "0" Then
                            prn_DetIndx = 0
                            prn_DetSNo = 0
                            prn_PageNo = 0
                            prn_NoofBmDets = 0

                            e.HasMorePages = True
                            Return
                        End If

                    End If
                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format2_GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurY1 As Single = 0, CurX As Single = 0
        Dim vLine_Pen As Pen
        Dim I As Integer = 0
        Dim br2 As SolidBrush

        PageNo = PageNo + 1

        CurY = TMargin


        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = "HO : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = "BO : " & prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else
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
            If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
                Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
            End If


        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
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


                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY - 5, 100, 100)
                            '--    e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY + 5, 100, 100)

                            'e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 20, CurY + 5, 110, 100)
                            'e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)

                        End If

                    End Using

                End If

            End If
        End If


        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 95, CurY, 90, 90)

                        End If

                    End Using

                End If

            End If

        End If


        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        'End If

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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt + 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        vLine_Pen = New Pen(Color.Black, 2)

        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)

            ItmNm2 = ""
            If Len(ItmNm1) > 35 Then
                For I = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If


            CurY = CurY + TxtHgt + 10


            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + 10, CurY + 5, 0, 0, p1Font, br2)
            Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(prn_HeadIndx).Item("E_Invoice_ACK_No").ToString, PrintWidth - 10, CurY + 5, 1, 0, p1Font, br2)

            If Trim(ItmNm2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "          " & Trim(ItmNm2), LMargin + 10, CurY + 5, 0, 0, p1Font, br2)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(prn_HeadIndx).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 10, CurY + 5, 1, 0, p1Font, br2)
            End If

        End If





        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("INVOICE DATE  :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Invoice.No", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1207" Then
            If prn_HdDt.Rows(0).Item("PavuSale_PrefixNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PavuSale_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)
            End If
        Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PavuSale_PrefixNo").ToString & prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Sales_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No ", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString), LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If

        'CurY = CurY + TxtHgt + 10
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(6) = CurY
        'e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(6), LMargin + M1, LnAr(2))

        'CurY = CurY + TxtHgt - 10

        'Common_Procedures.Print_To_PrintDocument(e, "Ends Count ", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.EndsCount_IdNoToName(con, (prn_HdDt.Rows(0).Item("EndsCount_IdNo").ToString)), LMargin + W1 + 25, CurY, 0, 0, pFont)


        'If Trim(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Pavu Meters", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Pavu_Meters").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        'End If

        'If Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> "" Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "Pavu Beams ", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + W1 + 25, CurY, 0, 0, pFont)
        'End If
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))


        'e.Graphics.DrawLine(Pens.Black, LMargin + M1 + 4, LnAr(3), LMargin + M1 + 4, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format2_GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p3Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        '  Dim m1 As Integer
        Dim C1 As Single, Amt As Single = 0
        Dim BmsInWrds As String
        Dim TotMtrs As Single = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            With dgv_PavuDetails
                For I = 0 To .RowCount - 1
                    TotMtrs = TotMtrs + Val(.Rows(I).Cells(5).Value())
                Next
            End With
            Amt = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString) * Val(TotMtrs), "##########0.00")

            CurY = CurY + TxtHgt - 10


            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then


                    'If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    'End If

                    'If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    'End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(Amt) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    End If
                    'Else

                    '    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                    '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    '    End If
                    '    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                    '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    '    End If
                    '    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                    '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    '    End If

                End If

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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            ' CurY = CurY + TxtHgt
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 6
            'CurY = CurY + TxtHgt
            'If Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Gross Amount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    'If is_LastPage = True Then
            '    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '    ' End If
            'End If

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





            CurY1 = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL BEAMS : " & Val((prn_HdDt.Rows(0).Item("Empty_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString)), LMargin + 10, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            p3Font = New Font("Calibri", 10, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then
                CurY1 = CurY1 + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 10, CurY, 0, 0, p1Font)
            End If
            If BankNm1 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm2 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm3 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm4 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
            End If


            If Val(prn_HdDt.Rows(0).Item("Add_Less").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                'If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(0).Item("Add_Less").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                ' End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Freight ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                'If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(0).Item("Freight").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                ' End If
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(prn_HdDt.Rows(0).Item("IGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If



            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "RoundOff  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Roundoff").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt


            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + C1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            'LnAr(9) = CurY

            'CurY = CurY + TxtHgt


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY



            CurY = CurY + TxtHgt - 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(StrConv(BmsInWrds, vbProperCase)), "", "")
            Common_Procedures.Print_To_PrintDocument(e, "Rupees   : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(11) = CurY

            '=============GST SUMMARY============

            Printing_GST_HSN_Details_Format1(e, CurY, LMargin, PageWidth, PrintWidth, LnAr(11))

            '====================================


            'LnAr(7) = CurY
            'Common_Procedures.Print_To_PrintDocument(e, "Received The Beams As Per Above Details.", LMargin + 10, CurY + 5, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)


            CurY = CurY + TxtHgt + 50
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format11(ByRef e As System.Drawing.Printing.PrintPageEventArgs)


        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim vFontName As String = ""
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim flperc As Single = 0
        Dim flmtr As Single = 0
        Dim fmtr As Single = 0
        Dim VechDesc1 As String = "", VechDesc2 As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim vLine_Pen As Pen
        Dim vActual_Rate As String = 0


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
            .Left = 20 ' 40
            .Right = 50
            .Top = 15 ' 20 '40 '50 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1229" Then '----Star Fabric Mills (Thekkalur)
            vFontName = "Cambria"
        Else
            vFontName = "Calibri"
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then
            pFont = New Font(vFontName, 11, FontStyle.Bold)
        Else
            pFont = New Font(vFontName, 9, FontStyle.Bold)
        End If

        pFont = New Font(vFontName, 9, FontStyle.Bold)
        'pFont = New Font(vFontName, 9, FontStyle.Bold)

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

        NoofItems_PerPage = 12

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then
            ClAr(1) = 30 : ClAr(2) = 250 : ClAr(3) = 80 : ClAr(4) = 45 : ClAr(5) = 0 : ClAr(6) = 50 : ClAr(7) = 105 : ClAr(8) = 80
            ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))
        Else
            ClAr(1) = 30 : ClAr(2) = 180 : ClAr(3) = 80 : ClAr(4) = 60 : ClAr(5) = 70 : ClAr(6) = 60 : ClAr(7) = 70 : ClAr(8) = 80
            ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))
        End If

        'ClAr(1) = 30 : ClAr(2) = 210 : ClAr(3) = 80 : ClAr(4) = 50 : ClAr(5) = 50 : ClAr(6) = 50 : ClAr(7) = 80 : ClAr(8) = 80
        'ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 16.5 ' 16.65 ' 17.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20


        vLine_Pen = New Pen(Color.Black, 2)

        'Try
        'prn_Prev_HeadIndx = prn_HeadIndx
        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Printing_GST_Format11_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, vLine_Pen, vFontName)

        If prn_HdDt.Rows.Count > 0 Then

            If prn_DetDt.Rows.Count > 0 Or Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString) <> 0 Then

                Do While prn_NoofBmDets < prn_DetMxIndx

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        'Printing_GST_Format11_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                        Printing_GST_Format11_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False, vLine_Pen, vFontName)

                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                        e.HasMorePages = True

                        Return

                    End If

                    prn_DetIndx = prn_DetIndx + 1

                    CurY = CurY + TxtHgt

                    If Val(prn_DetAr(prn_DetIndx, 5)) <> 0 Then

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 8)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 9))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

                        If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 4))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 5)), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 7)), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                        prn_NoofBmDets = prn_NoofBmDets + 1

                    End If

                    NoofDets = NoofDets + 1

                Loop

            End If

            Printing_GST_Format11_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True, vLine_Pen, vFontName)

            If Trim(prn_InpOpts) <> "" Then
                If prn_Count < Len(Trim(prn_InpOpts)) Then


                    If Val(prn_InpOpts) <> "0" Then
                        prn_DetIndx = 0
                        prn_DetSNo = 0
                        prn_PageNo = 0
                        prn_NoofBmDets = 0

                        e.HasMorePages = True
                        Return
                    End If

                End If
            End If

        End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try


        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If


    End Sub

    Private Sub Printing_GST_Format11_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByRef NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen, ByVal vFontName As String)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""
        Dim Rate_PCMETER As String = ""
        Dim Cmp_CIN_No As String = "", Cmp_CIN_Cap As String = ""


        PageNo = PageNo + 1

        CurY = TMargin


        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from Pavu_Sales_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.pavu_sales_code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
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

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
                    If Val(S) = 1 Then
                        prn_OriDupTri = "ORIGINAL"
                    ElseIf Val(S) = 2 Then
                        prn_OriDupTri = "TRANSPORT COPY"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "TRIPLICATE"
                    ElseIf Val(S) = 4 Then
                        prn_OriDupTri = "EXTRA COPY"
                    Else
                        If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                            prn_OriDupTri = Trim(prn_InpOpts)
                        End If
                    End If

                Else
                    If Val(S) = 1 Then
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1169" Then
                            prn_OriDupTri = "ORIGINAL FOR BUYER"
                        Else
                            prn_OriDupTri = "ORIGINAL FOR RECEIPIENT"
                        End If

                    ElseIf Val(S) = 2 Then
                        prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                    ElseIf Val(S) = 4 Then
                        prn_OriDupTri = "EXTRA COPY"
                    Else
                        If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                            prn_OriDupTri = Trim(prn_InpOpts)
                        End If
                    End If

                End If

            End If
        End If

        CurY = CurY + TxtHgt
        'CurY = CurY + TxtHgt

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
        p1Font = New Font(vFontName, 14, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
            Common_Procedures.Print_To_PrintDocument(e, "GST TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then '---- BHARATHI TEXTILE (TIRUPUR)
            Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE CLOTH", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- Selvanayaki Textiles (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font, Brushes.Red)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
        End If
        '  End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        'e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1229" Then '----Star Fabric Mills (Thekkalur)
            p1Font = New Font(vFontName, 8, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "*** shree ***", LMargin, CurY + 5, 2, PrintWidth, p1Font)
            CurY = CurY + 5
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Then '---- Sri Sathis Textile (Mangalam-VelayuthamPalayam)
            p1Font = New Font(vFontName, 8, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Sri Selvanayaki Amman Thunai", LMargin, CurY + 5, 2, PrintWidth, p1Font)
            CurY = CurY + 5
        End If

        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
        Cmp_CIN_No = "" : Cmp_CIN_Cap = ""

        prn_HeadIndx = 0

        Desc = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Description").ToString


        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString

        End If

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "Phone : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(prn_HeadIndx).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(prn_HeadIndx).Item("Company_State_Name").ToString
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateNm = Cmp_StateNm & "   CODE : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_State_Code").ToString
            End If
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString
        End If

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CinNo").ToString) <> "" Then
            Cmp_CIN_Cap = "CIN No. : "
            Cmp_CIN_No = prn_HdDt.Rows(prn_HeadIndx).Item("Company_CinNo").ToString
        End If


        CurY = CurY + TxtHgt - 5
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
            If Val(lbl_Company.Tag) = 1 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
            If InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHY") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHI") > 0 Then                                    '---- Ganapathy Spinning textile
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.GSM_LOGO, Drawing.Image), LMargin + 20, CurY, 112, 80)
            ElseIf InStr(1, Trim(UCase(Cmp_Name)), "LOGU") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "LOGA") > 0 Then                                          '---- Logu textile
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_LogaTex, Drawing.Image), LMargin + 20, CurY, 112, 80)
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Selvanayaki_Kpati, Drawing.Image), LMargin + 20, CurY - 10, 120, 90)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then '---- Bannari amman textiles
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.REVISED_LOGO_7___2_, Drawing.Image), LMargin + 20, CurY - 10, 130, 110)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- m.s textiles
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.ms_logo_2, Drawing.Image), LMargin + 20, CurY - 10, 130, 110)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then                   '---- Arul Kumaran Textiles (Somanur)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_ArulKumaran, Drawing.Image), LMargin + 20, CurY - 5, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 10, CurY - 5, 120, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1045" Then '---- Kesavalogu textiles
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KeasavLogu, Drawing.Image), LMargin + 10, CurY - 5, 120, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then '---- j.p.r TEXTILES 
            If InStr(1, Trim(UCase(Cmp_Name)), "JPR") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "J.P.R") > 0 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.JPR_LOGO2, Drawing.Image), LMargin + 10, CurY - 10, 150, 110)
            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1169" Then '---- GANESHA TEXTILES
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.GANESH_TEX, Drawing.Image), LMargin + 5, CurY - 15, 160, 115)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1173" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then '---- S.P TEXTILES & ANATHARAJA
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.SPT, Drawing.Image), LMargin + 5, CurY - 10, 140, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Then '----  SAKTHI VINAYAGA TEXTILES
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Shakthi_Vinayaka, Drawing.Image), LMargin + 5, CurY - 5, 115, 115)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1136" Then '---- ps TEXTILES
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.PS_LOGO, Drawing.Image), LMargin + 10, CurY - 15, 115, 115)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Then '----  senthil murugan TEXTILES
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Shakthi_Vinayaka, Drawing.Image), LMargin + 5, CurY - 5, 115, 115)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.MURUGAN1, Drawing.Image), PageWidth - 125, CurY - 5, 115, 115)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "3000" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.MURUGAN1, Drawing.Image), LMargin + 10, CurY - 5, 115, 115)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then '----
            If Val(lbl_Company.Tag) = 1 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.ammanswami, Drawing.Image), LMargin + 10, CurY, 112, 110)
            Else
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.MURUGAN1, Drawing.Image), LMargin + 10, CurY, 112, 110)
            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1214" Then '----
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Vinayakar_cholatx, Drawing.Image), LMargin + 10, CurY + 10, 112, 110)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1229" Then '----Star Fabric Mills (Thekkalur)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Star_1229, Drawing.Image), LMargin + 10, CurY, 112, 110)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Then                   '---- Amman Tex (Velayuthampalayam)    and Sri Sathis Textiles(velayuthampalayam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_AmmanTex, Drawing.Image), LMargin + 20, CurY - 5, 100, 90)
        End If



        'If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("E_Invoice_IRNO").ToString) <> "" Then

        '    If IsDBNull(prn_HdDt.Rows(prn_HeadIndx).Item("E_Invoice_QR_Image")) = False Then
        '        Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(prn_HeadIndx).Item("E_Invoice_QR_Image"), Byte())
        '        If Not imageData Is Nothing Then
        '            Using ms As New MemoryStream(imageData, 0, imageData.Length)
        '                ms.Write(imageData, 0, imageData.Length)
        '                If imageData.Length > 0 Then

        '                    pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

        '                    e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 2, 90, 90)

        '                End If
        '            End Using
        '        End If
        '    End If

        'End If


        Dim br = New SolidBrush(Color.FromArgb(255, 0, 0))

        p1Font = New Font(vFontName, 18, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- Selvanayaki Textiles (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
            p1Font = New Font(vFontName, 24, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, br)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1214" Then
            br = New SolidBrush(Color.FromArgb(249, 99, 40))
            p1Font = New Font(vFontName, 22, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, br)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "3000" Then
            p1Font = New Font(vFontName, 25, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
            CurY = CurY + strHeight - 15
        Else
            CurY = CurY + strHeight - 7
        End If

        If Desc <> "" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Gray)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)
            End If
        End If

        br = New SolidBrush(Color.FromArgb(0, 150, 0))

        strWidth = e.Graphics.MeasureString(Trim(Cmp_Add1 & " " & Cmp_Add2), p1Font).Width
        p1Font = New Font(vFontName, 11, FontStyle.Bold)

        If PrintWidth > strWidth And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1258" Then
            If Trim(Cmp_Add1 & " " & Cmp_Add2) <> "" Then
                CurY = CurY + TxtHgt - 1
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY - 5, 2, PrintWidth, p1Font, br)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, pFont)
                End If
            End If

            NoofItems_PerPage = NoofItems_PerPage - 1

        Else

            If Cmp_Add1 <> "" Then
                CurY = CurY + TxtHgt - 1
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY - 10, 2, PrintWidth, p1Font, br)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
                End If

            End If
            If Cmp_Add2 <> "" Then
                CurY = CurY + TxtHgt - 1
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY - 5, 2, PrintWidth, p1Font, br)

                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
                End If
            End If

        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_StateCap & " : " & Cmp_StateNm), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt

        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_CIN_Cap & Cmp_CIN_No & "     " & Cmp_PanCap & Cmp_PanNo), p1Font).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + ((PrintWidth - strWidth) / 2) - 20
        Else
            CurX = LMargin
        End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap, CurX, CurY - 3, 0, PrintWidth, p1Font)

        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth

        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY - 3, 0, 0, p1Font)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, p1Font).Width
        CurX = CurX + strWidth

        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_CIN_Cap, CurX, CurY - 3, 0, PrintWidth, p1Font)

        strWidth = e.Graphics.MeasureString("     " & Cmp_CIN_Cap, p1Font).Width
        CurX = CurX + strWidth

        Common_Procedures.Print_To_PrintDocument(e, Cmp_CIN_No, CurX, CurY - 3, 0, 0, p1Font)

        strWidth = e.Graphics.MeasureString(Cmp_CIN_No, p1Font).Width
        'p1Font = New Font(vFontName, 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)

        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, p1Font)


        'If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("E_Invoice_IRNO").ToString) <> "" Then
        '    CurY = CurY + TxtHgt
        '    p1Font = New Font(vFontName, 12, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "IRN : " & prn_HdDt.Rows(prn_HeadIndx).Item("E_Invoice_IRNO").ToString, LMargin, CurY, 2, PrintWidth, p1Font)
        'End If


        CurY = CurY + TxtHgt - 10
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        'Try

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30
        W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", pFont).Width
        S1 = e.Graphics.MeasureString("TO", pFont).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :", pFont).Width

        W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", pFont).Width
        S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", pFont).Width

        W3 = e.Graphics.MeasureString("INVOICE   DATE", pFont).Width
        S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", pFont).Width

        CurY = CurY + 10
        p1Font = New Font(vFontName, 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("PavuSale_PrefixNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PavuSale_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
        Else
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_Sales_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

        End If
        Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("eway_bill_no").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("eway_bill_no").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        'LnAr(2) = CurY

        CurY1 = CurY
        CurY2 = CurY

        '---left side

        CurY1 = CurY1 + 10
        p1Font = New Font(vFontName, 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER (BILLED TO) :", LMargin + 10, CurY1, 0, 0, p1Font)

        strHeight = e.Graphics.MeasureString("A", p1Font).Height
        CurY1 = CurY1 + strHeight
        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_mainName").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)

        CurY1 = CurY1 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
        End If

        'CurY1 = CurY1 + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, PageWidth, CurY1)

        'CurY1 = CurY1 + TxtHgt - 15
        'If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
        'End If
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
        '    If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
        '        strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
        '        CurX = LMargin + S1 + 10 + strWidth
        '        Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, pFont)
        '    End If
        'End If

        'If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10, CurY1, 0, 0, pFont)
        'End If
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
        '    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
        '        strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
        '        CurX = LMargin + C1 + S1 + 10 + strWidth
        '        Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY1, 0, PrintWidth, pFont)
        '    End If
        'End If

        'CurY1 = CurY1 + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, PageWidth, CurY1)
        'LnAr(10) = CurY1
        'If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
        '    CurY1 = CurY1 + TxtHgt - 15
        '    Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, "CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + C1 - 100, CurY1, 0, 0, pFont)
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + C1 + S1 + 10, CurY1, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, " CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, PageWidth - 100, CurY1, 0, 0, pFont)
        'End If


        '--Right Side

        CurY2 = CurY2 + 10
        p1Font = New Font(vFontName, 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE (SHIPPED TO) :", LMargin + C1 + 10, CurY2, 0, 0, p1Font)

        strHeight = e.Graphics.MeasureString("A", p1Font).Height
        CurY2 = CurY2 + strHeight
        p1Font = New Font(vFontName, 11, FontStyle.Bold)



        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_mainName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)

        CurY2 = CurY2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        End If


        CurY1 = IIf(CurY1 > CurY2, CurY1, CurY2)
        CurY1 = CurY1 + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, PageWidth, CurY1)



        CurY1 = CurY1 + TxtHgt - 15
        p1Font = New Font(vFontName, 10, FontStyle.Bold)
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString, p1Font).Width
                CurX = LMargin + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, p1Font)
            End If
        End If



        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString, LMargin + C1 + S1 + 10, CurY1, 0, 0, p1Font)
        End If



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString, p1Font).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, p1Font)
            End If
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1140" Then
            CurY1 = CurY1 + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, PageWidth, CurY1)
        End If
        LnAr(10) = CurY1

        CurY1 = CurY1 + TxtHgt - 15
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Name").ToString) <> "" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1140" Then
                Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "CODE : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Code").ToString, LMargin + C1 - 100, CurY1, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "STATE CODE : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Code").ToString, LMargin + C1 - 100, CurY1, 0, 0, pFont)
            End If
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Name").ToString) <> "" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1140" Then
                Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Name").ToString, LMargin + C1 + S1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " CODE : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Code").ToString, PageWidth - 100, CurY1, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, " STATE CODE : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Code").ToString, PageWidth - 100, CurY1, 0, 0, pFont)
            End If

        End If


        ''If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
        ''    CurY2 = CurY2 + TxtHgt
        ''    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + C1 + S1 + 10, CurY1, 0, 0, pFont)
        ''End If

        ''CurY2 = CurY2 + TxtHgt
        ''If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
        ''    Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
        ''End If
        ''If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
        ''    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
        ''        strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
        ''        CurX = LMargin + C1 + S1 + 10 + strWidth
        ''        Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, pFont)
        ''    End If
        ''End If


        ''e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
        ''e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 35, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 35, LnAr(3))
        ''e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 15, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 15, LnAr(3))
        ''e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 30, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 30, LnAr(3))
        ''e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 80, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 80, LnAr(3))


        CurY = IIf(CurY1 > CurY2, CurY1, CurY2)


        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 110, LnAr(10), LMargin + C1 - 110, LnAr(3))
        e.Graphics.DrawLine(Pens.Black, PageWidth - 110, LnAr(10), PageWidth - 110, LnAr(3))


        W2 = e.Graphics.MeasureString("DOCUMENT THROUGH   : ", pFont).Width
        S2 = e.Graphics.MeasureString("DATE & TIME OF SUPPLY  :", pFont).Width

        '--Right Side
        CurY = CurY + 10


        'Common_Procedures.Print_To_PrintDocument(e, "AGENT NAME ", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Agent_Name").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Transport_Mode").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


        ' CurY = CurY + TxtHgt


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "NO OF BALES", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Bales").ToString), "########0"), LMargin + W2 + 30, CurY, 0, 0, pFont)

        'Else

        '    Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Party_OrderNo").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        '    If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Party_OrderDate").ToString) <> "" Then
        '        strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(prn_HeadIndx).Item("Party_OrderNo").ToString, pFont).Width
        '        Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(prn_HeadIndx).Item("Party_OrderDate").ToString, LMargin + W2 + 30 + strWidth + 15, CurY, 0, 0, pFont)
        '    End If
        'End If


        Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Vehicle_No").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)




        Dim vprn_BlNos As String = ""
        'CurY = CurY + TxtHgt
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then

        '    vprn_BlNos = ""
        '    For I = 0 To prn_DetDt.Rows.Count - 1
        '        If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
        '            vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
        '        End If
        '    Next
        '    Common_Procedures.Print_To_PrintDocument(e, "BALE NOS", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, vprn_BlNos, LMargin + W2 + 30, CurY, 0, 0, pFont)

        'Else
        '    Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        '    If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Dc_Date").ToString) <> "" Then
        '        strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(prn_HeadIndx).Item("Dc_No").ToString, pFont).Width
        '        Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(prn_HeadIndx).Item("Dc_Date").ToString, LMargin + W2 + 30 + strWidth + 15, CurY, 0, 0, pFont)
        '    End If
        'End If


        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT NAME", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Trasport_Name").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

        ' CurY = CurY + TxtHgt

        'Common_Procedures.Print_To_PrintDocument(e, "LR NO.", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Lr_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Lr_Date").ToString) <> "" Then
        '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(prn_HeadIndx).Item("Lr_No").ToString, pFont).Width
        '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(prn_HeadIndx).Item("Lr_Date").ToString, LMargin + W2 + 30 + strWidth + 15, CurY, 0, 0, pFont)
        'End If

        'Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Despatch_To").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Despatch_To").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
        'Else
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("DeliveryTo_State_Name").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
        'End If


        'Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "BALE/BUNDLE WEIGHT", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Bale_Weight").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        'Else
        '    Common_Procedures.Print_To_PrintDocument(e, "DOCUMENT THROUGH", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Through_Name").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        'End If


        'Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)

        'If Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
        'Else
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
        'End If


        'If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Lc_No").ToString) <> "" Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "LC NO", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Lc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
        '    If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Lc_Date").ToString) <> "" Then
        '        strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(prn_HeadIndx).Item("Lc_No").ToString, pFont).Width
        '        Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(prn_HeadIndx).Item("Lc_Date").ToString, LMargin + W2 + 150, CurY, 0, 0, pFont)
        '    End If
        'End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(3))
        LnAr(4) = CurY


        CurY = CurY + 10



        'Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY + (TxtHgt \ 2), 2, ClAr(1), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "PRODUCT DESCRIPTION", LMargin + ClAr(1), CurY + (TxtHgt \ 2), 2, ClAr(2), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "HSN", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "MTRS /", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then
        '    'If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Type").ToString) = "ROLL" Then
        '    '    Common_Procedures.Print_To_PrintDocument(e, "ROLLS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + (TxtHgt \ 2), 2, ClAr(5), pFont)
        '    'ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Type").ToString) = "BUNDLE" Then
        '    '    Common_Procedures.Print_To_PrintDocument(e, "BUNDLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        '    'Else
        '    Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + (TxtHgt \ 2), 2, ClAr(5), pFont)
        '    'End If
        'Else
        '    Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + (TxtHgt \ 2), 2, ClAr(5), pFont)
        'End If


        'Common_Procedures.Print_To_PrintDocument(e, "PCS/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "BUNDLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)


        'Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + (TxtHgt \ 2), 2, ClAr(9), pFont)


        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)


        CurY = CurY + TxtHgt + 20
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            CurY = CurY + 10
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1229" Then '----Star Fabric Mills (Thekkalur)
                p1Font = New Font(vFontName, 7, FontStyle.Bold)
            Else
                p1Font = New Font(vFontName, 8, FontStyle.Bold)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
        End If

        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub Printing_GST_Format11_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal vLine_Pen As Pen, ByVal vFontName As String)
        Dim p1Font As Font, p2Font As Font, p3Font As Font
        Dim rndoff As Double, TtAmt As Double
        Dim I As Integer
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim SubClAr(15) As Single
        Dim vNoofHsnCodes As Integer = 0
        Dim vTaxPerc As Single = 0

        ' Try

        For I = NoofDets + 1 To NoofItems_PerPage

            CurY = CurY + TxtHgt

            prn_DetIndx = prn_DetIndx + 1

        Next

        CurY = CurY + TxtHgt + 7
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
        e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
        e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
        e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
        e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
        e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))



        CurY1 = CurY


        Erase BnkDetAr
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Bank_Ac_Details").ToString) <> "" Then
            BnkDetAr = Split(Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Bank_Ac_Details").ToString), ",")

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

        'Total_Bales      ,         Total_Pcs        

        pFont = New Font(vFontName, 9, FontStyle.Bold)
        If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString) <> 0 Then
            CurY1 = CurY1 + 10
            '    Common_Procedures.Print_To_PrintDocument(e, "Total Bundles : " & Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Bales").ToString), LMargin + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "Total Pcs : " & Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Pcs").ToString), LMargin + 200, CurY1, 0, 0, pFont)

            '    CurY1 = CurY1 + TxtHgt + 5
            '    Common_Procedures.Print_To_PrintDocument(e, "Total Yards : " & prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, LMargin + 10, CurY1, 0, 0, pFont)


            CurY1 = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL BEAMS : " & Val((prn_HdDt.Rows(0).Item("Empty_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString)), LMargin + 10, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total Meters : " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString) * 5.5 / 6, "###########0.00"), LMargin + 200, CurY1, 0, 0, pFont)

        End If


        CurY1 = CurY1 + 10

        p3Font = New Font(vFontName, 10, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then '---- BHARATHI TEXTILE (TIRUPUR)

            'CurY1 = CurY1 + TxtHgt
            'CurY1 = CurY1 + TxtHgt
            'CurY1 = CurY1 + TxtHgt
            'CurY1 = CurY1 + TxtHgt


            '' CurY1 = CurY1 + TxtHgt
            'e.Graphics.DrawLine(vLine_Pen, LMargin, CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1)

            'CurY1 = CurY1 + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, BankNm1 & " , " & BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)

            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, BankNm3 & " , " & BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
            'CurY1 = CurY1 + TxtHgt + 10
            'e.Graphics.DrawLine(vLine_Pen, LMargin, CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "3000" Then
            p3Font = New Font(vFontName, 15, FontStyle.Bold)
            ' CurY1 = CurY1 + TxtHgt
            ' CurY1 = CurY1 + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1)
            If BankNm1 <> "" Then
                CurY1 = CurY1 + TxtHgt + 3
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm2 <> "" Then
                CurY1 = CurY1 + TxtHgt + 3
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm3 <> "" Then
                CurY1 = CurY1 + TxtHgt + 3
                Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm4 <> "" Then
                CurY1 = CurY1 + TxtHgt + 3
                Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
            End If

        Else

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then
                p1Font = New Font(vFontName, 10, FontStyle.Underline Or FontStyle.Bold)
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 10, CurY1, 0, 0, p1Font)
            End If

            If BankNm1 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm2 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm3 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm4 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
        End If

        CurY1 = CurY1 + TxtHgt + 10
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1105" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Ganga Weaving (Dindugal)

        '    p1Font = New Font(vFontName, 10, FontStyle.Bold)
        '    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Gr_Time").ToString) <> 0 Then
        '        Common_Procedures.Print_To_PrintDocument(e, "DUE DATE : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + 10, CurY1, 0, 0, p1Font)
        '    Else
        '        Common_Procedures.Print_To_PrintDocument(e, "DUE DATE : IMMEDIATE", LMargin + 10, CurY1, 0, 0, p1Font)
        '    End If

        'Else

        '    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Gr_Time").ToString) <> 0 Then
        '        p1Font = New Font(vFontName, 10, FontStyle.Bold)
        '        Common_Procedures.Print_To_PrintDocument(e, "DUE DATE : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + 10, CurY1, 0, 0, p1Font)
        '    End If

        'End If

        '---Right Side
        CurY = CurY - 5

        'If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Trade_Discount_Perc").ToString) <> 0 Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("TradeDisc_Name").ToString) & " @ " & Trim(Val(prn_HdDt.Rows(0).Item("Trade_Discount").ToString)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
        '    'Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Trade_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
        'End If


        'CurY = CurY + TxtHgt
        'If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Cash_Discount_Perc").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("CashDisc_Name").ToString) & " @ " & Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
        '    'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Cash_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
        'End If


        'If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Amount").ToString) <> 0 Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
        'End If

        If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight").ToString) <> 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Freight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
        End If

        'If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Insurance").ToString) <> 0 Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Insurance_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Insurance").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
        'End If


        'If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Trade_Discount_Perc").ToString) <> 0 Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("Cash_Discount_Perc").ToString) <> 0 Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight").ToString) <> 0 Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("Insurance").ToString) <> 0 Then
        '    CurY = CurY + TxtHgt
        '    e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
        '    CurY = CurY - 15
        'End If

        vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

        CurY = CurY + TxtHgt
        If Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("IGST_Amount").ToString) <> 0 Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then '---- BHARATHI TEXTILE (TIRUPUR)
                p1Font = New Font(vFontName, 11, FontStyle.Bold)
            Else
                p1Font = New Font(vFontName, 10, FontStyle.Bold)
            End If

            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Gross_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "TAXABLE VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Gross_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)
            End If
        End If


        '----Gst
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CGST @ ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
        If Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Val(vTaxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "SGST @ ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
        If Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Val(vTaxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "IGST @ ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
        If Val(prn_HdDt.Rows(prn_HeadIndx).Item("IGST_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Val(vTaxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("IGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

        'If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tcs_Amount").ToString) <> 0 Then
        '        CurY = CurY + TxtHgt + 1
        '        e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
        '        CurY = CurY - 15 + 1
        '        CurY = CurY + TxtHgt

        '        p1Font = New Font(vFontName, 11, FontStyle.Bold)

        '        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, p1Font)
        '        'Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("TCS_Taxable_Value").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

        '        CurY = CurY + TxtHgt
        '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("TCs_name_caption").ToString) & "  @ " & Val(prn_HdDt.Rows(0).Item("Tcs_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Tcs_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
        '    End If

        TtAmt = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Gross_Amount").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("IGST_Amount").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("Add_Less").ToString), "#########0.00")

        rndoff = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Roundoff").ToString)
        'rndoff = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString) - Val(TtAmt)

        CurY = CurY + TxtHgt
        If Val(rndoff) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
            If Val(rndoff) >= 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
        End If


        If CurY1 > CurY Then CurY = CurY1

        If CurY < 690 Then CurY = 690 ' 731


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then '---- BHARATHI TEXTILE (TIRUPUR)
            ' CurY1 = CurY1 + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, BankNm1 & " , " & BankNm2, LMargin + 10, CurY, 0, 0, p3Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3 & " , " & BankNm4, LMargin + 10, CurY, 0, 0, p3Font)
            CurY = CurY + TxtHgt
            ' e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY)

        End If


        CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        'LnAr(8) = CurY


        p1Font = New Font(vFontName, 11, FontStyle.Bold)
        CurY = CurY + TxtHgt - 10

        Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, p1Font)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "3000" Then
            p1Font = New Font(vFontName, 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 3, CurY, 1, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)

        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY
        e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
        e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

        CurY = CurY + 5
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString))
        BmsInWrds = Replace(Trim(BmsInWrds), "", "")

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
            BmsInWrds = Trim(UCase(BmsInWrds))
        End If

        p1Font = New Font(vFontName, 10, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Then '---- Selvanayaki Textiles (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "Amount In Words : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then
            p1Font = New Font(vFontName, 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount In Words : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        '=============GST SUMMARY============
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1235" Then '---- Kalpana
        '    vNoofHsnCodes = Printing_GST_HSN_Details_Format1(EntryCode)
        If vNoofHsnCodes <> 0 Then
            ' Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), vLine_Pen)
            Printing_GST_HSN_Details_Format1(e, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
        End If

        'End If

        '==========================

        CurY = CurY + TxtHgt - 15

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1140" Then

            p1Font = New Font(vFontName, 9, FontStyle.Underline Or FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- Selvanayaki Textiles (Karumanthapatti)
                p2Font = New Font("Webdings", 8, FontStyle.Bold)
                p1Font = New Font(vFontName, 8, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, "Goods supplied under our firm condition", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt

            p2Font = New Font("Webdings", 8, FontStyle.Bold)
            p1Font = New Font(vFontName, 8, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile

                Common_Procedures.Print_To_PrintDocument(e, "Interest will be Charged at 24% P.A for the overdue payments from the Date of Invoice", LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any delay , Loss Or Damage During the Transport", LMargin + 10, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Quality Complaint Will be accepted only in Grey Stage for Fabrics and Cotton Yarn Stage for Yarns", LMargin + 10, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Subject to Palladam jurisdiction Only", LMargin + 10, CurY, 0, 0, p1Font)

            Else

                '1
                'If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
                '    Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
                '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- Selvanayaki Textiles (Karumanthapatti)
                '        Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 22% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 25, CurY, 0, 0, p1Font)
                '    Else
                '        Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 25, CurY, 0, 0, p1Font)
                '    End If

                'Else
                Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 22% from the invoice date ", LMargin + 25, CurY, 0, 0, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from the invoice date ", LMargin + 25, CurY, 0, 0, p1Font)
                End If

                'End If
                '3
                Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)

                '2
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 25, CurY, 0, 0, p1Font)
                '4
                Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " jurisdiction ", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)

            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- Selvanayaki Textiles (Karumanthapatti)

                CurY = CurY + TxtHgt
                p1Font = New Font(vFontName, 9, FontStyle.Underline Or FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Declaration :", LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt
                p1Font = New Font(vFontName, 8, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, "We Declare that this invoice shows the actual price of the goods described and that all particulars are true and correct.", LMargin + 25, CurY, 0, 0, p1Font)

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

        End If


        CurY = CurY + 5
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font(vFontName, 7, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- Selvanayaki Textiles (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct and the amount indicated represents the price actually charged and that there is no flow additional consideration", PageWidth - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "directly or indirectly from the buyer", LMargin + 20, CurY + 10, 0, 0, p1Font)
        Else
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1169" Then '---- Sri Ganesha Textiles (Somanur)
                Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)
            Else
                CurY = CurY - TxtHgt
            End If


        End If

        Dim br = New SolidBrush(Color.FromArgb(255, 0, 0))

        CurY = CurY + TxtHgt - 5
        p1Font = New Font(vFontName, 12, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- Selvanayaki Textiles (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then '---- Selvanayaki Textiles (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, br)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1214" Then
            br = New SolidBrush(Color.FromArgb(249, 99, 40))
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, br)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "3000" Then
            p1Font = New Font(vFontName, 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            p1Font = New Font(vFontName, 12, FontStyle.Bold)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        End If

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then
            CurY = CurY + TxtHgt
        End If

        If Val(prn_HdDt.Rows(prn_HeadIndx).Item("User_IdNo").ToString) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User_IdNoToName(con, Val(prn_HdDt.Rows(prn_HeadIndx).Item("User_IdNo").ToString))) & ")", LMargin + 25, CurY, 0, 0, pFont)
        End If
        ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1116" Then
        'If Val(Common_Procedures.User.IdNo) <> 1 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 25, CurY, 0, 0, pFont)
        'End If
        ' End If

        br = New SolidBrush(Color.FromArgb(0, 150, 0))
        CurY = CurY + TxtHgt
        '   CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then '---- Selvanayaki Textiles (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont, br)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont, br)
            p1Font = New Font(vFontName, 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, pFont, br)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, pFont)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then '---- kalpana cotton
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 200, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font(vFontName, 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, pFont)
        End If


        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(vLine_Pen, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(vLine_Pen, PageWidth, LnAr(1), PageWidth, CurY)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)
            CurY = CurY + TxtHgt - 10
            p1Font = New Font(vFontName, 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Please send payment details of this bill to asiatextilestirupur@yahoo.in", LMargin + 10, CurY, 0, 0, p1Font)
        End If

        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub
    Private Sub Printing_GST_HSN_Details_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef CurY As Single, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByVal LnAr As Single)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim TxtHgt As Single
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer = 0
        Dim NoofItems_Increment As Integer
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String
        Try
            pFont = New Font("Calibri", 10, FontStyle.Regular)


            NoofItems_PerPage = 3 ' 5

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 140 : SubClAr(2) = 130 : SubClAr(3) = 60 : SubClAr(4) = 95 : SubClAr(5) = 60 : SubClAr(6) = 90 : SubClAr(7) = 60
            SubClAr(8) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7))

            TxtHgt = 18.75 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20


            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin, CurY + 5, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 5, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, PageWidth, CurY)
            LnAr2 = CurY
            CurY = CurY + 5
            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            Da = New SqlClient.SqlDataAdapter("select * from Pavu_Sales_Head where Pavu_Sales_Code = '" & Trim(EntryCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                prn_DetIndx = 0
                NoofDets = 0
                NoofItems_Increment = 0
                CurY = CurY - 20

                Do While prn_DetIndx <= Dt.Rows.Count - 1

                    'ItmNm1 = Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString)
                    ItmNm1 = prn_DetAr(prn_DetMxIndx, 9) ' = Trim(prn_DetDt.Rows(I).Item("HSN_CODE").ToString)


                    ItmNm2 = ""
                    If Len(ItmNm1) > 40 Then
                        For I = 35 To 1 Step -1
                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 40
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                    End If



                    CurY = CurY + TxtHgt + 3

                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(0).Item("Gross_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(0).Item("Gross_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(0).Item("CGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(0).Item("CGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(0).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(0).Item("CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(0).Item("SGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(0).Item("SGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(0).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(0).Item("IGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(0).Item("IGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(0).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(0).Item("IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)

                    NoofItems_Increment = NoofItems_Increment + 1

                    NoofDets = NoofDets + 1



                    Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(0).Item("Gross_Amount").ToString)
                    Ttl_CGst = Ttl_CGst + Val(Dt.Rows(0).Item("CGST_Amount").ToString)
                    Ttl_Sgst = Ttl_Sgst + Val(Dt.Rows(0).Item("SGST_Amount").ToString)
                    Ttl_igst = Ttl_igst + Val(Dt.Rows(0).Item("IGST_Amount").ToString)
                    prn_DetIndx = prn_DetIndx + 1
                Loop

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)


            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            BmsInWrds = ""
            If (Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
                BmsInWrds = Replace(Trim(BmsInWrds), "", "")
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_EntryType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EntryType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EntryType, msk_date, cbo_BillTo, "", "", "", "")
    End Sub

    Private Sub cbo_EntryType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EntryType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntryType, cbo_BillTo, "", "", "", "")
    End Sub

    Private Sub dgv_PavuDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_EntryType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EntryType.TextChanged
        If cbo_EntryType.Text = "DIRECT" Then
            dgv_PavuDetails.AllowUserToAddRows = True
            dgv_PavuDetails.EditMode = DataGridViewEditMode.EditOnEnter
            dgv_PavuDetails.SelectionMode = DataGridViewSelectionMode.CellSelect
            dgv_PavuDetails.Columns(0).ReadOnly = True
            dgv_PavuDetails.Columns(8).ReadOnly = True
            dgv_PavuDetails.ReadOnly = False
        ElseIf cbo_EntryType.Text = "FROM STOCK" Then
            dgv_PavuDetails.AllowUserToAddRows = False
            dgv_PavuDetails.EditMode = DataGridViewEditMode.EditProgrammatically
            dgv_PavuDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgv_PavuDetails.ReadOnly = True

        End If
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim vLASTCOL As Integer = -1

        If ActiveControl.Name = dgv_PavuDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_PavuDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_PavuDetails

            Else
                dgv1 = dgv_PavuDetails

            End If

            With dgv1

                vLASTCOL = 12
                If .Columns(14).Visible = True Then
                    vLASTCOL = 14
                End If
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= vLASTCOL Then

                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            txt_Freight.Focus()

                        Else

                            If .CurrentCell.ColumnIndex >= vLASTCOL Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                            ElseIf .CurrentCell.ColumnIndex = 12 Then
                                If .Columns(14).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(14)
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                                End If
                            ElseIf .CurrentCell.ColumnIndex >= 6 And .CurrentCell.ColumnIndex <= 11 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(12)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            End If

                        End If

                    Else

                        If .CurrentCell.ColumnIndex = 12 Then
                            If .Columns(14).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(14)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then

                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            cbo_description.Focus()
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(vLASTCOL)
                        End If

                    Else
                        If .CurrentCell.ColumnIndex >= 14 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(12)
                        ElseIf .CurrentCell.ColumnIndex >= 12 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(6)
                        ElseIf .CurrentCell.ColumnIndex >= 6 And .CurrentCell.ColumnIndex <= 11 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        End If

                    End If

                    Return True

                Else
                    Return MyBase.ProcessCmdKey(msg, keyData)

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub cbo_Grid_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_EndsCount.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_EndsCount, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        With dgv_PavuDetails
            If e.KeyCode = 38 And cbo_Grid_EndsCount.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
                If .Visible Then
                    e.Handled = True
                    e.SuppressKeyPress = True
                    'If .Rows.Count <= 1 Then
                    '    txt_Freight.Focus()
                    'ElseIf .CurrentCell.ColumnIndex >= 6 And .CurrentCell.ColumnIndex <= 12 Then
                    '    .Focus()
                    '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)
                    'Else
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    .Focus()

                    'End If
                End If
            End If

            If e.KeyCode = 40 And cbo_Grid_EndsCount.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
                If .Visible Then
                    e.Handled = True
                    e.SuppressKeyPress = True
                    'If .CurrentCell.ColumnIndex <= 1 And .CurrentRow.Cells(1).Value = "" Then
                    '    txt_Freight.Focus()
                    'ElseIf .CurrentCell.ColumnIndex >= 6 And .CurrentCell.ColumnIndex <= 11 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(12)
                    .Focus()

                    '    'ElseIf .CurrentCell.ColumnIndex = 6 Then
                    '    '    .Focus()
                    '    '    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    'Else
                    '    .Focus()
                    '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    'End If

                End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_EndsCount, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        With dgv_PavuDetails

            If .Visible Then
                If Asc(e.KeyChar) = 13 Then

                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(12)
                    .Focus()



                End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_EndsCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_EndsCount.TextChanged
        Try

            With dgv_PavuDetails
                If cbo_Grid_EndsCount.Visible = True Then

                    If Val(cbo_Grid_EndsCount.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 6 Then
                        .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_EndsCount.Text)
                    End If

                End If

            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Add_NewRow_ToGrid()
        On Error Resume Next

        Dim i As Integer
        Dim n As Integer = -1

        With dgv_PavuDetails
            If .Visible Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    If Trim(UCase(cbo_EntryType.Text)) <> "FROM STOCK" Then
                        n = .Rows.Add()

                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = .Rows(.CurrentCell.RowIndex).Cells(i).Value
                            .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                        Next

                        For i = 0 To .Rows.Count - 1
                            .Rows(i).Cells(0).Value = i + 1
                        Next

                        .CurrentCell = .Rows(n).Cells(.CurrentCell.ColumnIndex)
                        .CurrentCell.Selected = True

                    End If
                End If

            End If

        End With

    End Sub

    Private Sub dgtxt_Details_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Leave
        Try

            dgv_PavuDetails.EditingControl.BackColor = Color.White
            dgv_PavuDetails.EditingControl.ForeColor = Color.Black
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_PavuDetails
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With
        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Function lbl_DateAndTimeOFSupply_Caption() As Object
        Throw New NotImplementedException
    End Function

    Private Sub Printing_Format3_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

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

        NoofItems_PerPage = 15

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 30 : ClArr(2) = 70 : ClArr(3) = 70 : ClArr(4) = 70 : ClArr(5) = 80 : ClArr(6) = 80 : ClArr(7) = 55 : ClArr(8) = 80 : ClArr(9) = 70
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 18 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Or Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString) <> 0 Then

                    Do While prn_NoofBmDets < prn_DetMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format3_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetIndx = prn_DetIndx + 1

                        CurY = CurY + TxtHgt

                        If Val(prn_DetAr(prn_DetIndx, 5)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 8)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 10)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 11)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 9))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 4))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 5)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 7)), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        NoofDets = NoofDets + 1

                    Loop

                End If

                Printing_Format3_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format3_GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font

        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurY1 As Single = 0, CurX As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin


        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = "HO : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = "BO : " & prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else
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
            If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
                Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
            End If


        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        'End If

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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt + 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("INVOICE DATE  :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Invoice.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then
            If prn_HdDt.Rows(0).Item("PavuSale_PrefixNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PavuSale_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)
            End If
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Sales_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No ", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString), LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))



        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format3_GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p3Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        '  Dim m1 As Integer
        Dim C1 As Single, Amt As Single = 0
        Dim BmsInWrds As String
        Dim TotMtrs As Single = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            With dgv_PavuDetails
                For I = 0 To .RowCount - 1
                    TotMtrs = TotMtrs + Val(.Rows(I).Cells(5).Value())
                Next
            End With
            Amt = Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString) * Val(TotMtrs), "##########0.00")

            CurY = CurY + TxtHgt - 10


            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then


                    'If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    'End If

                    'If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    'End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(Amt) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    End If
                    'Else

                    '    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                    '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    '    End If
                    '    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                    '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    '    End If
                    '    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                    '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    '    End If

                End If

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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            ' CurY = CurY + TxtHgt
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 5
            'CurY = CurY + TxtHgt
            'If Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Gross Amount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    'If is_LastPage = True Then
            '    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '    ' End If
            'End If

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





            CurY1 = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL BEAMS : " & Val((prn_HdDt.Rows(0).Item("Empty_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString)), LMargin + 10, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            p3Font = New Font("Calibri", 10, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then
                CurY1 = CurY1 + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : ", LMargin + 10, CurY, 0, 0, p1Font)
            End If
            If BankNm1 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm2 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm3 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm4 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
            End If


            If Val(prn_HdDt.Rows(0).Item("Add_Less").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                'If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(0).Item("Add_Less").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                ' End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Freight ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                'If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(0).Item("Freight").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                ' End If
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(prn_HdDt.Rows(0).Item("IGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If



            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "RoundOff  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Roundoff").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt


            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + C1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            'LnAr(9) = CurY

            'CurY = CurY + TxtHgt


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY



            CurY = CurY + TxtHgt - 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(StrConv(BmsInWrds, vbProperCase)), "", "")
            Common_Procedures.Print_To_PrintDocument(e, "Rupees   : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(11) = CurY

            '=============GST SUMMARY============

            Printing_GST_HSN_Details_Format1(e, CurY, LMargin, PageWidth, PrintWidth, LnAr(11))

            '====================================


            'LnAr(7) = CurY
            'Common_Procedures.Print_To_PrintDocument(e, "Received The Beams As Per Above Details.", LMargin + 10, CurY + 5, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)


            CurY = CurY + TxtHgt + 50
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub Printing_GST_Format_1123(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim CNTNm1 As String = "", CNTNm2 As String = ""
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Cmp_Name As String = ""
        Dim Wgt_Bag As String = ""
        Dim BagNo1 As String = "", BagNo2 As String = ""
        Dim YarnDesc1 As String = "", YarnDesc2 As String = ""


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
            .Right = 50 '45
            .Top = 20
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Bold)
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

        NoofItems_PerPage = 10 ' 11 '7


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 30 : ClArr(2) = 90 : ClArr(3) = 200 : ClArr(4) = 80 : ClArr(5) = 65 : ClArr(6) = 80 : ClArr(7) = 70
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        'ClArr(1) = 30 : ClArr(2) = 90 : ClArr(3) = 240 : ClArr(4) = 85 : ClArr(5) = 80 : ClArr(6) = 70
        'ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))




        TxtHgt = e.Graphics.MeasureString("A", pFont).Height

        TxtHgt = 18.5 ' 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20


        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format_1123_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Or Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString) <> 0 Then

                    Do While prn_DetIndx < prn_DetMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format3_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetIndx = prn_DetIndx + 1

                        CurY = CurY + TxtHgt

                        If Val(prn_DetAr(prn_DetIndx, 5)) <> 0 Then

                            CNTNm1 = Trim(prn_DetAr(prn_DetIndx, 11))
                            CNTNm2 = ""
                            If Len(CNTNm1) > 10 Then
                                For I = 10 To 1 Step -1
                                    If Mid$(Trim(CNTNm1), I, 1) = " " Or Mid$(Trim(CNTNm1), I, 1) = "," Or Mid$(Trim(CNTNm1), I, 1) = "." Or Mid$(Trim(CNTNm1), I, 1) = "-" Or Mid$(Trim(CNTNm1), I, 1) = "/" Or Mid$(Trim(CNTNm1), I, 1) = "_" Or Mid$(Trim(CNTNm1), I, 1) = "(" Or Mid$(Trim(CNTNm1), I, 1) = ")" Or Mid$(Trim(CNTNm1), I, 1) = "\" Or Mid$(Trim(CNTNm1), I, 1) = "[" Or Mid$(Trim(CNTNm1), I, 1) = "]" Or Mid$(Trim(CNTNm1), I, 1) = "{" Or Mid$(Trim(CNTNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 10

                                CNTNm2 = Microsoft.VisualBasic.Right(Trim(CNTNm1), Len(CNTNm1) - I)
                                CNTNm1 = Microsoft.VisualBasic.Left(Trim(CNTNm1), I - 1)
                            End If

                            ItmNm1 = Trim(prn_DetAr(prn_DetIndx, 10))
                            ItmNm2 = ""
                            If Len(ItmNm1) > 25 Then
                                For I = 25 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 25

                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 8)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(CNTNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 9))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetAr(prn_DetIndx, 12)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 5)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 6)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 7)), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                            If Trim(CNTNm2) <> "" Or Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CNTNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                        End If

                        NoofDets = NoofDets + 1

                    Loop

                End If

                Printing_GST_Format_1123_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                'If Trim(prn_InpOpts) <> "" Then
                '    If prn_Count < Len(Trim(prn_InpOpts)) Then


                '        If Val(prn_InpOpts) <> "0" Then
                '            prn_DetIndx = 0
                '            prn_DetSNo = 0
                '            prn_PageNo = 0

                '            e.HasMorePages = True
                '            Return
                '        End If

                '    End If
                'End If

                If Val(prn_TotCopies) > 1 Then
                    If prn_Count < Val(prn_TotCopies) Then

                        prn_DetIndx = 0
                        prn_DetSNo = 0
                        prn_PageNo = 0
                        prn_DetIndx = 0
                        prn_PageNo = 0
                        prn_NoofBmDets = 0


                        e.HasMorePages = True
                        Return

                    End If

                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format_1123_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""
        Dim vLrDt As String = ""
        Dim vPackType As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If

        End If

        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else

            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
            If Trim(Cmp_Add1) <> "" Then
                If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
                    Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
                Else
                    Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
                End If
            Else
                Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            End If

            Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)
            If Trim(Cmp_Add2) <> "" Then
                If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
                    Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
                Else
                    Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
                End If
            Else
                Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            End If

        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If


        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)

                        End If

                    End Using
                End If
            End If

        End If

        CurY = CurY + TxtHgt - 15
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1111" Then '-----Velan Spinning mills
            p1Font = New Font("Calibri", 30, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 22, FontStyle.Bold)
        End If

        Dim br = New SolidBrush(Color.FromArgb(255, 0, 0))
        br = New SolidBrush(Color.FromArgb(249, 99, 40))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1214" Then
            p1Font = New Font("Calibri", 22, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, br)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1313" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Sri_Guru_Fabrics_Company, Drawing.Image), LMargin + 220, CurY, 280, 100)
            CurY = CurY + TxtHgt + 40
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1315" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KR_Company_Logo, Drawing.Image), LMargin + 280, CurY, 180, 97)
            CurY = CurY + TxtHgt + 40
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1123" Then
            If Trim(Cmp_Name) = "SRI NIKITHA SIZING MILLS" Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Nkitha_logo_Logo, Drawing.Image), LMargin + 10, CurY, 90, 80)
            ElseIf Trim(Cmp_Name) = "SRI SANTHI SIZING MILLS" Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Shanthi_Logo, Drawing.Image), LMargin + 10, CurY, 90, 80)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If



        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        ItmNm1 = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1214" Then
            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        End If
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 5
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 3
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then


            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)

            ItmNm2 = ""
            If Len(ItmNm1) > 35 Then
                For i = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If

            CurY = CurY + TxtHgt + 2

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)

            If Trim(ItmNm2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "             " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)
            End If


        End If
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try


            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)


            W1 = e.Graphics.MeasureString("Invoice Date      :", pFont).Width
            W2 = e.Graphics.MeasureString("Transport Mode    :", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY + 10

            'Left Side
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
            'If prn_HdDt.Rows(0).Item("PavuSale_PrefixNo").ToString <> "" Then
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PavuSale_PrefixNo").ToString & "0" & prn_HdDt.Rows(0).Item("Pavu_Sales_no").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
            'Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PavuSale_PrefixNo").ToString & prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
            'End If


            Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt + 2
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("pavu_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
            End If


            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1, 0, 0, pFont)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("eway_bill_no").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("eway_bill_no").ToString), LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
            End If



            ' CurY1 = CurY1 + TxtHgt
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then
            'Common_Procedures.Print_To_PrintDocument(e, "Lr No", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            ''If Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
            '    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
            '    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            'End If

            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            '    If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
            '        strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
            '        Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            '    End If
            'End If

            'Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)


            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W1 + 30, CurY1, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
            'Else
            ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Agent_Name").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" Then
            '    CurY1 = CurY1 + TxtHgt
            '    If Trim(prn_HdDt.Rows(0).Item("Agent_Name").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Agent ", LMargin + 10, CurY1, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)
            '    End If
            '    'If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" Then

            '    '    vLrDt = ""
            '    '    If Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
            '    '        If IsDate(prn_HdDt.Rows(0).Item("Lr_Date").ToString) = True Then
            '    '            vLrDt = "  Date : " & Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString)
            '    '        End If
            '    '    End If

            '    '    Common_Procedures.Print_To_PrintDocument(e, "L.R No. ", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            '    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            '    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString & vLrDt, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)

            '    'End If
            'End If

            CurY = CurY1 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


            CurY1 = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  (BILLED TO)", LMargin, CurY1, 2, C2, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C2, CurY1, 2, PageWidth - C2, pFont)
            CurY = CurY1 + TxtHgt + 5


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + 10

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)


            If Val(prn_HdDt.Rows(0).Item("ShippedTo_IdNo").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("ShippedTo_Name").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)
            End If



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("ShippedTo_IdNo").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Shipped_address1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("ShippedTo_IdNo").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Shipped_address2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)


            If Val(prn_HdDt.Rows(0).Item("ShippedTo_IdNo").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Shipped_address3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            End If



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("ShippedTo_IdNo").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Shipped_address4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 12


            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 60 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

            If Val(prn_HdDt.Rows(0).Item("ShippedTo_IdNo").ToString) <> 0 Then
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("ShippedTo_IdNo").ToString)) & ")")
            Else
                If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
                    vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
                Else
                    vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("ShippedTo_IdNo").ToString)) & ")")
                End If
            End If


            If Val(prn_HdDt.Rows(0).Item("ShippedTo_IdNo").ToString) <> 0 Then

                If Trim(prn_HdDt.Rows(0).Item("Shipped_GSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
                    If Trim(prn_HdDt.Rows(0).Item("Shipped_GSTinNo").ToString) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Shipped_GSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                    End If
                    If Trim(vDelvPanNo) <> "" Then
                        strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Shipped_GSTinNo").ToString, pFont).Width
                        Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                    End If
                End If

            Else

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
                    If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                    End If
                    If Trim(vDelvPanNo) <> "" Then
                        strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                        Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                    End If
                End If

            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 70, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("ShippedTo_IdNo").ToString) <> 0 Then

                If Trim(prn_HdDt.Rows(0).Item("Shipped_State_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Shipped_State_Name").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Code :  " & prn_HdDt.Rows(0).Item("Shipped_State_Code").ToString, LMargin + C2 + ClAr(5) + ClAr(6) + 140, CurY, 0, 0, pFont)
                End If

            Else

                If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Code : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + C2 + ClAr(5) + ClAr(6) + 140, CurY, 0, 0, pFont)
                End If

            End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 100, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 100, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + 130, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + 130, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + 130, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + 130, LnAr(3))

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 5 + TxtHgt + 15
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)

            CurY = CurY + TxtHgt - 5

            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAMS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)


            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format_1123_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim Rup1 As String = "", Rup2 As String = ""
        Dim M As Integer = 0

        Dim Jurisdctn As String = ""

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, (Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GRoss_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            Erase BnkDetAr
            If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                BInc = -1

LOOP2:
                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm1 = Trim(BnkDetAr(BInc))
                    If InStr(1, Trim(UCase(BankNm1)), "BANK") > 0 And (InStr(1, Trim(UCase(BankNm1)), "DETAIL") > 0 Or InStr(1, Trim(UCase(BankNm1)), "DETAILS") > 0) Then
                        BankNm1 = ""
                        GoTo LOOP2
                    End If
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


            Y1 = CurY + 0.75
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)


            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin, CurY, 2, C1, pFont)
            'If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '    End If
            'End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME :   " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BRANCH         :    " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Add_Less").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("Add_Less").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "A/C NO          :     " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Taxable Value  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Add_Less").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE     :    " & BankNm4, LMargin + 10, CurY, 0, 0, pFont)


            If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(prn_HdDt.Rows(0).Item("IGST_Percentage").ToString), "##########0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            Else

                If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

                End If

            End If



            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY

            p1Font = New Font("Calibri", 9, FontStyle.Bold)

            Y1 = CurY + 0.75
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                '---
            Else
                If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

                End If

            End If



            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "1. Overdue interest will be charged at 24% from the invoice date.", LMargin + 10, CurY, 0, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)


            Common_Procedures.Print_To_PrintDocument(e, "Total  TAX Amount", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "2. We will not accept any claim after processing of goods.", LMargin + 10, CurY, 0, 0, pFont)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Roundoff").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Roundoff").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "3. We are not responsible for any loss or damage in transit.", LMargin + 10, CurY, 0, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin + C1, Y1, PageWidth, Y2)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 + 10, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 10, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "4. Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " Jurisdiction Only.", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY)
            LnAr(10) = CurY

            Y1 = CurY + 0.55
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + 40, Y2, Brushes.DarkGray)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + C1 - 10, CurY, 1, 0, pFont)

            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, p1Font)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt + 10, PageWidth, CurY + TxtHgt + 10)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt + 10, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))


            CurY = CurY + TxtHgt

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 40, CurY, LMargin + ClAr(1) + ClAr(2) + 40, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            CurY = CurY + 5



            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))

                BmsInWrds = Trim(StrConv(BmsInWrds, VbStrConv.ProperCase))

                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & BmsInWrds, LMargin + 10, CurY, 0, 0, p1Font)

                'Rup2 = ""
                'Rup1 = BmsInWrds
                'If Len(Rup1) > 60 Then
                '    For M = 60 To 1 Step -1
                '        If Mid$(Trim(Rup1), M, 1) = " " Then Exit For
                '    Next M
                '    If M = 0 Then M = 60
                '    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - M)
                '    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), M - 1)
                'End If

                'p1Font = New Font("Calibri", 11, FontStyle.Bold)
                'Common_Procedures.Print_To_PrintDocument(e, " " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
                'If Trim(Rup2) <> "" Then
                '    CurY = CurY + TxtHgt - 2
                '    Common_Procedures.Print_To_PrintDocument(e, " " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
                '    CurY = CurY - 10
                'End If

            End If



            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            LnAr(14) = CurY

            Dim br = New SolidBrush(Color.FromArgb(255, 0, 0))

            p1Font = New Font("Calibri", 7.5, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt - 5

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Received by", LMargin, CurY, 2, LMargin + ClAr(1) + ClAr(2) + 70, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1) + ClAr(2) + 100, CurY, 2, (LMargin + ClAr(1) + ClAr(2) + ClAr(3)) - (LMargin + ClAr(1) + ClAr(2) + 70), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 100, CurY, LMargin + ClAr(1) + ClAr(2) + 100, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(14))

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format_1123_PageFooter111(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim Rup1 As String = "", Rup2 As String = ""
        Dim M As Integer = 0

        Dim Jurisdctn As String = ""

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), "##########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)

                End If

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GRoss_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)

                'If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                'Else
                '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                'End If
                'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            Erase BnkDetAr
            If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                BInc = -1

LOOP2:
                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm1 = Trim(BnkDetAr(BInc))
                    If InStr(1, Trim(UCase(BankNm1)), "BANK") > 0 And (InStr(1, Trim(UCase(BankNm1)), "DETAIL") > 0 Or InStr(1, Trim(UCase(BankNm1)), "DETAILS") > 0) Then
                        BankNm1 = ""
                        GoTo LOOP2
                    End If
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


            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)


            Y1 = CurY + 0.75
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            'If IsDBNull(dt1.Rows(0).Item("Freight_Name").ToString) = False Then
            '    If Trim(dt1.Rows(0).Item("Freight_Name").ToString) <> "" Then
            '        txt_Freight_Name.Text = dt1.Rows(0).Item("Freight_Name").ToString
            '    End If
            'End If
            'txt_FreightAmount.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")
            'If IsDBNull(dt1.Rows(0).Item("Packing_Name").ToString) = False Then
            '    If Trim(dt1.Rows(0).Item("Packing_Name").ToString) <> "" Then
            '        txt_Packing_Name.Text = dt1.Rows(0).Item("Packing_Name").ToString
            '    End If
            'End If
            'txt_Packing.Text = Format(Val(dt1.Rows(0).Item("Packing_Amount").ToString), "#########0.00")
            'If IsDBNull(dt1.Rows(0).Item("AddLess_Name").ToString) = False Then
            '    If Trim(dt1.Rows(0).Item("AddLess_Name").ToString) <> "" Then
            '        txt_AddLess_Name.Text = dt1.Rows(0).Item("AddLess_Name").ToString
            '    End If
            'End If
            'txt_AddLessAmount.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")


            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin, CurY, 2, C1, pFont)
            'If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '    End If
            'End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "" & BankNm1, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "" & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            'If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            '    End If
            'End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "" & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & BankNm4, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 80 + 15, CurY, 0, 0, pFont)


            If Val(prn_HdDt.Rows(0).Item("Add_Less").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("Add_Less").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 80, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 80, LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)



            Y1 = CurY + 0.75
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Taxable Value  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Add_Less").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt

            CurY1 = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15

            p1Font = New Font("Calibri", 9, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "1. Overdue interest will be charged at 24% from the invoice date.", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "1. Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " Jurisdiction.", LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1261" Then '---- KPG SOMANUR (KARUMATHAMPATTI)
                    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.00") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1261" Then '---- KPG SOMANUR (KARUMATHAMPATTI)
                    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.00") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                End If

                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "2. We will not accept any claim after processing of goods.", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "3. We are not responsible for any loss or damage in transit.", LMargin + 10, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1261" Then '---- KPG SOMANUR (KARUMATHAMPATTI)
                    Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                End If

                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If


            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "4. Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " Jurisdiction Only.", LMargin + 10, CurY, 0, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)

            'If Val(prn_HdDt.Rows(0).Item("Cess_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : " & Trim(prn_HdDt.Rows(0).Item("Cess_Name").ToString) & "  @ " & Format(Val(prn_HdDt.Rows(0).Item("Cess_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Cess_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            '    CurY = CurY + TxtHgt

            '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            'End If


            Common_Procedures.Print_To_PrintDocument(e, "Total  TAX Amount", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Bold)

            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "DECLARATION :", LMargin + 10, CurY, 0, 0, p1Font)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Roundoff").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Roundoff").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "We declare that this invoice shows the actual price of the goods described", LMargin + 10, CurY, 0, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin + C1, Y1, PageWidth, Y2)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 + 10, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 10, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "and that all particulars are true and correct.", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY)
            LnAr(10) = CurY

            Y1 = CurY + 0.55
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + 40, Y2, Brushes.DarkGray)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + C1 - 10, CurY, 1, 0, pFont)

            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, p1Font)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt + 10, PageWidth, CurY + TxtHgt + 10)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt + 10, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))


            CurY = CurY + TxtHgt

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt + 10, PageWidth, CurY + TxtHgt + 10)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

            ''e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 40, CurY, LMargin + ClAr(1) + ClAr(2) + 40, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            CurY = CurY + 5

            'p1Font = New Font("Calibri", 9, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY + 5, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY + 5, 1, 0, p1Font)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt + 10, PageWidth, CurY + TxtHgt + 10)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt + 10, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))


            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
                    BmsInWrds = Trim(UCase(BmsInWrds))
                Else
                    BmsInWrds = Trim(StrConv(BmsInWrds, VbStrConv.ProperCase))
                End If

                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & BmsInWrds, LMargin + 10, CurY, 0, 0, p1Font)

                'Rup2 = ""
                'Rup1 = BmsInWrds
                'If Len(Rup1) > 60 Then
                '    For M = 60 To 1 Step -1
                '        If Mid$(Trim(Rup1), M, 1) = " " Then Exit For
                '    Next M
                '    If M = 0 Then M = 60
                '    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - M)
                '    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), M - 1)
                'End If

                'p1Font = New Font("Calibri", 11, FontStyle.Bold)
                'Common_Procedures.Print_To_PrintDocument(e, " " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
                'If Trim(Rup2) <> "" Then
                '    CurY = CurY + TxtHgt - 2
                '    Common_Procedures.Print_To_PrintDocument(e, " " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
                '    CurY = CurY - 10
                'End If

            End If



            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)

            LnAr(14) = CurY

            Dim br = New SolidBrush(Color.FromArgb(255, 0, 0))

            p1Font = New Font("Calibri", 7.5, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1169" Then '---- Sri Ganesha Textiles (Somanur)
                Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)
            End If
            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1214" Then
                br = New SolidBrush(Color.FromArgb(249, 99, 40))
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font, br)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt - 5

            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
            Common_Procedures.Print_To_PrintDocument(e, "Received by", LMargin, CurY, 2, LMargin + ClAr(1) + ClAr(2) + 70, pFont)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 35, CurY, 0, 0, pFont)
            'End If

            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1) + ClAr(2) + 70, CurY, 2, (LMargin + ClAr(1) + ClAr(2) + ClAr(3)) - (LMargin + ClAr(1) + ClAr(2) + 70), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 70, CurY, LMargin + ClAr(1) + ClAr(2) + 70, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(14))

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Function get_GST_Tax_Percentage_For_Printing(ByVal EntryCode As String) As Single
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0

        TaxPerc = 0

        Cmd.Connection = con

        Cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSubTable) & " "
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " (Meters1, Currency1) select (CGST_Percentage+SGST_Percentage), (CGST_Amount+SGST_Amount) from Pavu_Sales_Head  Where Pavu_Sales_Code = '" & Trim(EntryCode) & "' and (CGST_Amount+SGST_Amount) <> 0"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " (Meters1, Currency1) select IGST_Percentage, IGST_Amount from Pavu_Sales_Head  Where Pavu_Sales_Code = '" & Trim(EntryCode) & "' and IGST_Amount <> 0"
        Cmd.ExecuteNonQuery()

        TaxPerc = 0

        Da = New SqlClient.SqlDataAdapter("Select Meters1, sum(Currency1) from " & Trim(Common_Procedures.EntryTempSubTable) & " Group by Meters1 Having sum(Currency1) <> 0", con)
        'Da = New SqlClient.SqlDataAdapter("Select * from Yarn_Sales_GST_Tax_Details Where Yarn_Sales_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Pavu_Sales_Head Where Pavu_Sales_Code = '" & Trim(EntryCode) & "'", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    If Val(Dt2.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                        TaxPerc = Val(Dt2.Rows(0).Item("IGST_Percentage").ToString)
                    Else
                        TaxPerc = Val(Dt2.Rows(0).Item("CGST_Percentage").ToString)
                    End If
                End If
                Dt2.Clear()

            End If
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Dt2.Dispose()
        Da.Dispose()

        get_GST_Tax_Percentage_For_Printing = Format(Val(TaxPerc), "#########0.00")

    End Function


    Private Sub cbo_description_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_description.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Pavu_Sales_Head", "Description", "", "")

    End Sub

    Private Sub cbo_description_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_description.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_description, cbo_VehicleNo, Nothing, "Pavu_Sales_Head", "Description", "", "")
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            e.Handled = True
            e.SuppressKeyPress = True
            If Trim(cbo_EntryType.Text) = "DIRECT" Then
                If dgv_PavuDetails.Rows.Count > 0 Then

                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)

                Else
                    txt_Freight.Focus()

                End If

            Else
                txt_Freight.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_description_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_description.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_description, Nothing, "Pavu_Sales_Head", "Description", "", "", False)
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True

            If Trim(UCase(cbo_EntryType.Text)) = Trim(UCase("DIRECT")) Then
                If dgv_PavuDetails.Rows.Count > 0 Then
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                    dgv_PavuDetails.CurrentCell.Selected = True

                Else
                    txt_Freight.Focus()

                End If

            Else
                txt_Freight.Focus()

            End If

        End If
    End Sub


    Private Sub cbo_EndsCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_DelvTo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BillTo.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub btn_EMail_Click(sender As Object, e As EventArgs) Handles btn_EMail.Click
        Dim Led_IdNo As Integer
        Dim MailTxt As String

        Try

            Common_Procedures.Print_OR_Preview_Status = 1
            Print_PDF_Status = True
            EMAIL_Status = True
            print_record()

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_BillTo.Text)
            'If Led_IdNo  = 0 Then Exit Sub


            MailTxt = "PAVU SALES " & vbCrLf & vbCrLf

            MailTxt = MailTxt & "INVOICE.NO:" & Trim(lbl_InvNo.Text) & vbCrLf & "INVOICE.DATE:" & Trim(dtp_Date.Text) & vbCrLf

            MailTxt = MailTxt & vbCrLf
            MailTxt = MailTxt & vbCrLf

            If System.IO.File.Exists(vEMAIL_Attachment_FileName) = True Then
                MailTxt = MailTxt & "Please find the following attachment(s):"

                MailTxt = MailTxt & "        PavuSales_" & Trim(lbl_InvNo.Text) & ".pdf"
            End If



            EMAIL_Entry.vMailID = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            EMAIL_Entry.vSubJect = "Pavu Sales Invoice : " & Trim(lbl_InvNo.Text)
            EMAIL_Entry.vMessage = Trim(MailTxt)
            EMAIL_Entry.vAttchFilepath = ""
            If System.IO.File.Exists(vEMAIL_Attachment_FileName) = True Then
                EMAIL_Entry.vAttchFilepath = Trim(vEMAIL_Attachment_FileName)
            End If


            Print_PDF_Status = False
            EMAIL_Status = False

            Dim f1 As New EMAIL_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        EMAIL_Status = False
        print_record()
        Print_PDF_Status = False
    End Sub

    Private Sub cbo_ShippedTo_GotFocus(sender As Object, e As EventArgs) Handles cbo_ShippedTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_ShippedTo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ShippedTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ShippedTo, cbo_BillTo, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_ShippedTo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ShippedTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ShippedTo, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_ShippedTo_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_ShippedTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ShippedTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub txt_KuraiPavuMeter_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_KuraiPavuMeter.KeyDown
        If e.KeyValue = 38 Then
            txt_KuraiPavuBeam.Focus()

        End If
        If e.KeyValue = 40 Then
            txt_RateMeters.Focus()

        End If

    End Sub
    Private Sub btn_EInvoice_Generation_Click(sender As Object, e As EventArgs) Handles btn_EInvoice_Generation.Click
        btn_Generate_eInvoice.Enabled = True
        btn_Generate_EWB_IRN.Enabled = True

        grp_EInvoice.Visible = True
        grp_EInvoice.BringToFront()
        grp_EInvoice.Left = (Me.Width - grp_EInvoice.Width) / 2
        grp_EInvoice.Top = (Me.Height - grp_EInvoice.Height) / 2
        btn_CheckConnectivity1.Enabled = False
        btn_CheckConnectivity1.Visible = False
    End Sub
    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click

        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        Grp_EWB.Left = (Me.Width - grp_EInvoice.Width) / 2
        Grp_EWB.Top = (Me.Height - grp_EInvoice.Height) / 2 + 200

    End Sub
    Private Sub btn_Generate_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Generate_eInvoice.Click
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim vPAVU_NAME As String = ""
        Dim vIS_SERVC_STS As Integer = 0

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()


        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Pavu_Sales_Head Where pavu_sales_code = '" & Trim(NewCode) & "'"

        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Pavu_Sales_Head Where pavu_sales_code = '" & Trim(NewCode) & "' and Len(E_Invoice_IRNO) > 0"
        c = Cmd.ExecuteScalar

        If c > 0 Then
            Dim k As Integer = MsgBox("An IRN Has been Generated already for this Invoice. Do you want to Delete the Previous IRN ?", vbYesNo, "IRN Generated")
            If k = vbNo Then
                MsgBox("Cannot Create a New IRN When there is an IRN generated already !", vbOKOnly, "Duplicate IRN ")
                Exit Sub
            Else

            End If
        End If

        If Val(txt_KuraiPavuMeter.Text) <> 0 Or Val(txt_KuraiPavuBeam.Text) <> 0 Then
            If Val(txt_RateMeters.Text) = 0 Then
                MessageBox.Show("Invalid Rate ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_RateMeters.Enabled And txt_RateMeters.Visible Then txt_RateMeters.Focus()
                Exit Sub
            End If

        Else

            For i = 0 To dgv_PavuDetails.RowCount - 1

                If Val(dgv_PavuDetails.Rows(i).Cells(1).Value) <> 0 Or Val(dgv_PavuDetails.Rows(i).Cells(5).Value) <> 0 Then

                    If Val(dgv_PavuDetails.Rows(i).Cells(12).Value) = 0 Or Val(dgv_PavuDetails.Rows(i).Cells(13).Value) = 0 Then
                        MessageBox.Show("Invalid Rate / Amount", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.Focus()
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(12)
                        End If
                        Exit Sub
                    End If
                End If

            Next
        End If

        If Val(lbl_NetAmount.Text) = 0 Then
            MessageBox.Show("Invalid Amount", "DOES NOT GENERATE E-INVOICE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_AddLess.Enabled And txt_AddLess.Visible Then txt_AddLess.Focus()
            Exit Sub
        End If

        Dim tr As SqlClient.SqlTransaction

        tr = con.BeginTransaction
        Cmd.Transaction = tr

        Try


            Cmd.CommandText = "Delete from e_Invoice_Head  where Ref_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Delete from e_Invoice_Details  where Ref_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Head (                      e_Invoice_No            ,  e_Invoice_date , Buyer_IdNo     , Consignee_IdNo,    Assessable_Value                   ,    CGST    ,     SGST    ,    IGST     , Cess , State_Cess , Round_Off, Nett_Invoice_Value,      Ref_Sales_Code    ,  Other_Charges , Dispatcher_idno )" &
                              "Select                             PavuSale_PrefixNo + Pavu_Sales_No  ,  Pavu_Sales_Date, DeliveryTo_IdNo, ShippedTo_IdNo,  ( Gross_Amount + Freight + Add_Less) , CGST_Amount,  SGST_Amount, IGST_Amount ,   0  ,    0       ,     0    ,    Net_Amount     , '" & Trim(NewCode) & "',      0         ,    0              from Pavu_Sales_head where pavu_sales_code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSimpleTable) & ""
            Cmd.ExecuteNonQuery()

            '---from Pavu_Sales_Details
            Cmd.CommandText = "Insert into  " & Trim(Common_Procedures.EntryTempSimpleTable) & " (       INT1  ,                                                          NAME1                                                            ,       NAME2         ,      METERS1 ,      NAME3  ,       METERS2 ,                                                   METERS3                                                         ,            METERS4    ,                                                      CURRENCY1                                                        ,         METERS5         ,      METERS6   ,       METERS7   ,       METERS8 ,    METERS9  ,   METERS10  ,     CURRENCY2        ,	   CURRENCY3  , 	CURRENCY4     ,	      CURRENCY5         ,	CURRENCY6  ,     CURRENCY7	 ,	          NAME4         ) " &
                                                                                    " Select        tPSD.Sl_No , tECH.EndsCount_Name + (case when tPSH.Description<> '' then '  -  ' + tPSH.Description else '' end)  as producDescription ,  tIGH.Item_HSN_Code , tPSD.Meters  , 'MTR' as UOM,  tPSD.Rate    ,  ( (tPSD.Amount + (CASE WHEN tPSD.sl_no = 1 then ( tPSH.Freight + tPSH.Add_Less) else 0 end ) ) ) as Total_Amount ,  0  as DiscountAmount , ( (tPSD.Amount + (CASE WHEN tPSD.sl_no = 1 then ( tPSH.Freight + tPSH.Add_Less) else 0 end ) ) ) as Assessable_Amount , tIGH.Item_GST_Percentage,   0 AS SgstAmt ,    0 AS CgstAmt ,  0 AS igstAmt ,  0 AS CesRt , 0 AS CesAmt ,	0 AS CesNonAdvlAmt ,  0 AS StateCesRt ,	 0 AS StateCesAmt ,	0 as StateCesNonAdvlAmt , 0 as OthChrg , 0 as TotItemVal , '" & Trim(NewCode) & "'    " &
                                                                                      " from Pavu_Sales_Details tPSD  INNER JOIN Pavu_Sales_Head tPSH  ON tPSH.pavu_sales_code = tPSD.pavu_sales_code INNER JOIN ENDSCOUNT_HEAD tECH ON tPSD.EndsCount_IdNo = tECH.EndsCount_IdNo INNER JOIN  Count_Head tCH ON tCH.Count_IdNo = tECH.Count_IdNo  INNER JOIN ItemGroup_Head tIGH ON tCH.ItemGroup_IdNo = tIGH.ItemGroup_IdNo " &
                                                                                      " Where tPSD.pavu_sales_code = '" & Trim(NewCode) & "' and tPSD.Meters > 0 Order by tPSD.Sl_No "
            Cmd.ExecuteNonQuery()


            '----from Pavu_Sales_Head
            Cmd.CommandText = "Insert into  " & Trim(Common_Procedures.EntryTempSimpleTable) & " (  INT1,      NAME1                                                                                                                 ,       NAME2         ,      METERS1     ,      NAME3  ,      METERS2     ,                                                   METERS3                                                                                       ,            METERS4    ,                                                      CURRENCY1                                                                                           ,         METERS5         ,       METERS6  ,       METERS7  ,       METERS8  ,    METERS9  ,   METERS10  ,     CURRENCY2       ,	   CURRENCY3  , 	CURRENCY4     ,	      CURRENCY5         ,	CURRENCY6  ,     CURRENCY7	 ,	NAME4                   ) " &
                                                                                    " Select        201 , tECH.EndsCount_Name  + (case when tPSH.Description<> '' then '  -  ' + tPSH.Description else '' end)  as producDescription ,  tIGH.Item_HSN_Code , tPSH.pavu_meters , 'MTR' as UOM, tPSH.Rate_Meters ,  (  (tPSH.pavu_meters*tPSH.Rate_Meters ) + (CASE WHEN tPSH.Total_Meters = 0 then ( tPSH.Freight + tPSH.Add_Less) else 0 end ) ) as Total_Amount ,  0  as DiscountAmount ,       ( (tPSH.pavu_meters*tPSH.Rate_Meters ) + (CASE WHEN tPSH.Total_Meters = 0 then ( tPSH.Freight + tPSH.Add_Less) else 0 end ) ) as Assessable_Amount , tIGH.Item_GST_Percentage,   0 AS SgstAmt ,    0 AS CgstAmt ,  0 AS igstAmt ,  0 AS CesRt , 0 AS CesAmt ,	0 AS CesNonAdvlAmt ,  0 AS StateCesRt ,	 0 AS StateCesAmt ,	0 as StateCesNonAdvlAmt , 0 as OthChrg , 0 as TotItemVal , '" & Trim(NewCode) & "'    " &
                                                                                      " from Pavu_Sales_Head tPSH INNER JOIN ENDSCOUNT_HEAD tECH ON tPSH.EndsCount_IdNo = tECH.EndsCount_IdNo INNER JOIN Count_Head tCH ON tCH.Count_IdNo = tECH.Count_IdNo  INNER JOIN ItemGroup_Head tIGH ON tIGH.ItemGroup_IdNo = tCH.ItemGroup_IdNo " &
                                                                                      " Where tPSH.pavu_sales_code = '" & Trim(NewCode) & "' and tPSH.pavu_meters > 0"
            Cmd.ExecuteNonQuery()


            Cmd.CommandText = "Insert into e_Invoice_Details (      Sl_No , IsService    ,     Product_Description       ,             HSN_Code ,    Batch_Details    ,          Quantity      ,         Unit  ,        Unit_Price      ,           Total_Amount  ,     Discount        ,         Assessable_Amount       ,          GST_Rate    ,    SGST_Amount           ,  	          IGST_Amount ,	       CGST_Amount     ,	        Cess_rate,         Cess_Amount    , 	    CessNonAdvlAmount	  ,      State_Cess_Rate        ,	   State_Cess_Amount            ,	    	StateCessNonAdvlAmount    ,	        	 Other_Charge ,      	Total_Item_Value	 ,		AttributesDetails   ,      Ref_Sales_Code   )    " &
                                                " SELECT    INT1 AS Sl_No , 0 as IsServc ,  NAME1 AS Product_Description ,    NAME2 AS HSN_Code , '' as batchdetails  ,  METERS1 AS   Quantity , NAME3 AS Unit ,  METERS2 AS Unit_Price , METERS3 as Total_Amount ,  METERS4 as Discount,  CURRENCY1 as Assessable_Amount , METERS5 AS  GST_Rate ,   METERS6 AS SGST_Amount ,  METERS7 as IGST_Amount , METERS8 as CGST_Amount , METERS9 AS Cess_rate, METERS10 AS Cess_Amount, CURRENCY2 AS CessNonAdvlAmount, CURRENCY3 as State_Cess_Rate,	CURRENCY3 AS   State_Cess_Amount, CURRENCY4 AS 	StateCessNonAdvlAmount,	CURRENCY5 AS  Other_Charge,	CURRENCY6 as Total_Item_Value,	'' as AttributesDetails , NAME4 AS  Ref_Sales_Code   " &
                                                " FROM " & Trim(Common_Procedures.EntryTempSimpleTable) & " Where METERS1 > 0"
            Cmd.ExecuteNonQuery()


            tr.Commit()


        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message & Chr(13) & "Cannot Generate IRN.", "DOES NOT GENERATE IRN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Exit Sub

        End Try


        btn_Generate_eInvoice.Enabled = False

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateIRN(Val(lbl_Company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Pavu_Sales_Head", "pavu_sales_code", Pk_Condition)

    End Sub
    Private Sub btn_Delete_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Delete_eInvoice.Click

        If Len(Trim(txt_EInvoiceCancellationReson.Text)) = 0 Then
            MsgBox("Please provide the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.CancelIRNByIRN(txt_eInvoiceNo.Text, rtbeInvoiceResponse, "Pavu_Sales_Head", "pavu_sales_code", con, txt_eInvoice_CancelStatus, NewCode, txt_EInvoiceCancellationReson.Text)

    End Sub
    Private Sub btn_Close_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Close_eInvoice.Click
        grp_EInvoice.Visible = False
    End Sub
    Private Sub Btn_Qr_Code_Add_Click(sender As Object, e As EventArgs) Handles Btn_Qr_Code_Add.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pic_IRN_QRCode_Image.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub Btn_Qr_Code_Close_Click(sender As Object, e As EventArgs) Handles Btn_Qr_Code_Close.Click
        pic_IRN_QRCode_Image.BackgroundImage = Nothing
    End Sub

    Private Sub btn_Generate_EWB_Click(sender As Object, e As EventArgs) Handles btn_Generate_EWB_IRN.Click
        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Pavu_Sales_DETAILS Where pavu_sales_code = '" & NewCode & "'"
        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Pavu_Sales_Head Where pavu_sales_code = '" & NewCode & "' and (Len(eway_bill_no) >0 or Len(E_Invoice_IRNO) = 0 OR E_Invoice_IRNO IS NULL )"
        c = Cmd.ExecuteScalar

        If c > 0 Then
            'Dim k As Integer = MsgBox("EWB Has been Generated already for this Invoice. Do you want to Delete the Previous IRN ?", vbYesNo, "IRN Generated")
            'If k = vbNo Then
            MsgBox("Cannot Create a New EWB When there is an EWB generated already and/or an IRN has not been generated!", vbOKOnly, "Duplicate EWB ")
            Exit Sub
            'Else
            'End If 
        End If

        Dim tr As SqlClient.SqlTransaction

        tr = con.BeginTransaction
        Cmd.Transaction = tr

        Try

            Cmd.CommandText = "Delete from EWB_By_IRN  where InvCode = '" & NewCode & "'"
            Cmd.ExecuteNonQuery()


            Cmd.CommandText = "Insert into EWB_By_IRN  (	[IRN]         ,     	[TransID]        ,	            [TransMode]  ,	[TransDocNo]    , [TransDocDate] ,	[VehicleNo]        ,                [Distance]                                              ,	[VehType] ,	[TransName]         ,    [InvCode]           ,  Company_Idno ,     Company_Pincode,                                           Shipped_To_Idno                        ,                                       Shipped_To_Pincode               ) " &
                                " Select                A.E_Invoice_IRNO  ,  ISNULL(t.Ledger_GSTINNo, '' ) ,        '1'    ,       0   ,   Null         ,       '' , (CASE WHEN a.ShippedTo_IdNo <> 0 THEN  D.Distance ELSE L.Distance END),      'R'    ,  t.Ledger_Mainname  , '" & Trim(NewCode) & "' , tZ.Company_IdNo, tZ.Company_PinCode, (CASE WHEN a.ShippedTo_IdNo <> 0 THEN  a.ShippedTo_IdNo ELSE a.DeliveryTo_IdNo END), (CASE WHEN a.ShippedTo_IdNo <> 0 THEN  D.Pincode ELSE L.Pincode END)    " &
                                    " from Pavu_Sales_Head a INNER JOIN Company_Head tZ on a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head L on a.DeliveryTo_IdNo = L.Ledger_IdNo LEFT OUTER JOIN Ledger_Head D on a.ShippedTo_IdNo = D.Ledger_IdNo LEFT OUTER JOIN Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo Where a.pavu_sales_code = '" & Trim(NewCode) & "'"

            Cmd.ExecuteNonQuery()

            tr.Commit()

            'Exit Sub

            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()

            MessageBox.Show(ex.Message + " Cannot Generate IRN.", "ERROR WHILE GENERATING E-WAY BILL BY IRN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Exit Sub

        End Try

        btn_Generate_EWB_IRN.Enabled = False

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateEWBByIRN(NewCode, rtbeInvoiceResponse, txt_eWayBill_No, txt_EWB_Date, txt_EWB_ValidUpto, con, "Pavu_Sales_Head", "pavu_sales_code", txt_EWB_Canellation_Reason, txt_EWB_Cancel_Status, Pk_Condition)

        Cmd.CommandText = "DELETE FROM EWB_By_IRN WHERE INVCODE = '" & NewCode & "'"
        Cmd.ExecuteNonQuery()

    End Sub
    Private Sub btn_Print_EWB_Click(sender As Object, e As EventArgs) Handles btn_Print_EWB.Click

        'Dim ewb As New EWB(Val(lbl_Company.Tag))
        'EWB.PrintEWB(txt_EWayBillNo.Text, rtbeInvoiceResponse)

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_eWayBill_No.Text, rtbeInvoiceResponse, 0)

    End Sub
    Private Sub btn_Detail_PRINT_EWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINT_EWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.PrintEWB(txt_eWayBill_No.Text, rtbEWBResponse, 1)
    End Sub
    Private Sub btn_Cancel_EWB_Click(sender As Object, e As EventArgs) Handles btn_Cancel_EWB.Click
        If Len(Trim(txt_EWB_Canellation_Reason.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim Ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_eWayBill_No.Text, NewCode, con, rtbeInvoiceResponse, txt_eWayBill_No, "Pavu_Sales_Head", "eway_bill_no", "pavu_sales_code")

        'Dim einv As New eInvoice(Val(lbl_Company.Tag))

        'einv.Cancel_EWB_IRN(NewCode, txt_eWayBill_No.Text, rtbeInvoiceResponse, txt_EWB_Cancel_Status, Con, "Pavu_Sales_Head", "pavu_sales_code", txt_EWB_Canellation_Reason.Text)
    End Sub
    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()


        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select EWB_No from Pavu_Sales_Head where Pavu_Sales_Code = '" & NewCode & "'", con)
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

        CMD.CommandText = "Delete from EWB_Head Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()


        If Len(Trim(cbo_ShippedTo.Text)) = 0 Then

            CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
                         "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
                         "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
                         "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
                         "[VehicleNo]      ,[VehicleType]   , [InvCode]) " &
                         " " &
                         " " &
                         "  SELECT               'O'              , '1'             ,   ''              ,    'INV'    ,  A.PavuSale_PrefixNo + a.Pavu_Sales_No ,a.Pavu_Sales_date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
                         " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo  ,L.Ledger_Name,L.Ledger_Address1+L.Ledger_Address2,L.Ledger_Address3+L.Ledger_Address4,L.City_Town,L.Pincode, TS.State_Code,TS.State_Code," &
                         " 1                     ,a.Add_Less + a.Roundoff, A.Gross_Amount    , A.CGST_Amount  ,  A.SGST_Amount , A.IGST_Amount   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " '' AS LR_No        ,         '' AS Lr_Date            , a.Net_Amount         ,   '1' AS TrMode ," &
                         " a.Vehicle_No,'R','" & NewCode & "' from Pavu_Sales_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L on a.DeliveryTo_IdNo = L.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo Left Outer Join State_Head FS On " &
                         " C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  where a.Pavu_Sales_Code = '" & NewCode & "'"

        Else

            CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
                         "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
                         "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
                         "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
                         "[VehicleNo]      ,[VehicleType]   , [InvCode]) " &
                         " " &
                         " " &
                         "  SELECT               'O'              , '1'             ,   ''              ,    'INV'    ,     A.PavuSale_PrefixNo + a.Pavu_Sales_No  , a.Pavu_Sales_date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
                         " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,LH.Ledger_GSTINNo  ,LH.Ledger_Name,L.Ledger_Address1+L.Ledger_Address2,L.Ledger_Address3+L.Ledger_Address4,L.City_Town,L.Pincode, TS.State_Code,TS.State_Code," &
                         " 1                     ,a.Add_Less + a.Roundoff, A.Gross_Amount    , A.CGST_Amount  ,  A.SGST_Amount , A.IGST_Amount   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " '' AS LR_No        ,         '' AS Lr_Date      , a.Net_Amount         ,     '1'  AS TrMode ," &
                         " a.Vehicle_No,'R','" & NewCode & "' from Pavu_Sales_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head LH on a.DeliveryTo_IdNo = LH.Ledger_IdNo  Inner Join Ledger_Head L on a.ShippedTo_IdNo = L.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo Left Outer Join State_Head FS On " &
                         " C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  where a.Pavu_Sales_Code = '" & NewCode & "'"

        End If

        CMD.ExecuteNonQuery()

        Dim vCgst_Amt As String = 0
        Dim vSgst_Amt As String = 0
        Dim vIgst_AMt As String = 0
        Dim vTax_Perc As String = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable


        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        ''------------------


        da = New SqlClient.SqlDataAdapter(" Select  I.EndsCount_Name, (I.EndsCount_Name + ' - WARP'  ) , IG.Item_HSN_Code , IG.Item_GST_Percentage , sum(PD.Meters * PD.Rate) As TaxableAmt,sum(PD.Meters) as Qty, 1 , 'MTR' AS Units  , tz.Company_State_IdNo , Lh.Ledger_State_Idno   " &
                                          " from Pavu_Sales_Head SD Inner Join Pavu_Sales_DETAILS Pd On Pd.Pavu_Sales_Code = Sd.Pavu_Sales_Code  Inner Join EndsCount_Head I On PD.EndsCount_IdNo = I.EndsCount_IdNo INNER JOIN Count_Head Ch On Ch.Count_Idno = I.Count_Idno " &
                                          " Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo  INNER Join Ledger_Head Lh ON Lh.Ledger_Idno =  SD.DeliveryTo_IdNo   INNER JOIN Company_Head tz On tz.Company_Idno = SD.Company_Idno   Where SD.Pavu_Sales_Code = '" & Trim(NewCode) & "' Group By " &
                                          " I.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage , tz.Company_State_IdNo , Lh.Ledger_State_Idno    ", con)

        Dim DT1 As New DataTable
        DT1 = New DataTable
        da.Fill(DT1)


        If DT1.Rows.Count > 0 Then
            For I = 0 To DT1.Rows.Count - 1

                vTax_Perc = 0

                ' If Val(DT1.Rows(I).Item("GST_Tax_Invoice_Status")) = 1 Then
                If DT1.Rows(I).Item("Company_State_IdNo") = DT1.Rows(I).Item("Ledger_State_Idno") Then

                        If Val(DT1.Rows(I).Item(3).ToString) <> 0 Then
                            vCgst_Amt = ((DT1.Rows(I).Item(4) * Val(DT1.Rows(I).Item(3).ToString) / 100) / 2)
                            vSgst_Amt = vCgst_Amt
                        vIgst_AMt = 0

                        vTax_Perc = DT1.Rows(I).Item(3).ToString

                    Else
                            vCgst_Amt = 0
                            vSgst_Amt = 0
                        vIgst_AMt = 0

                        vTax_Perc = 0
                    End If
                    Else

                        If Val(DT1.Rows(I).Item(3).ToString) <> 0 Then
                            vIgst_AMt = (DT1.Rows(I).Item(4) * Val(DT1.Rows(I).Item(3).ToString) / 100)
                            vCgst_Amt = 0
                        vSgst_Amt = 0

                        vTax_Perc = DT1.Rows(I).Item(3).ToString
                    Else
                            vIgst_AMt = 0
                            vCgst_Amt = 0
                        vSgst_Amt = 0
                        vTax_Perc = 0

                    End If

                    End If




                'Else

                '    vIgst_AMt = 0
                '    vCgst_Amt = 0
                '    vSgst_Amt = 0

                'End If


                CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                    ,     [QuantityUnit] ,             Tax_Perc      ,	[CessRate]       ,	[CessNonAdvol]  ,	[TaxableAmount]               , InvCode      ,                   Cgst_Value  ,                       Sgst_Value ,                         Igst_Value) " &
                      " values                 (" & DT1.Rows(I).Item(6).ToString & ",'" & DT1.Rows(I).Item(0) & "', '" & DT1.Rows(I).Item(1) & "', '" & DT1.Rows(I).Item(2) & "', " & DT1.Rows(I).Item(5).ToString & ",         'KGS'          ," & Val(vTax_Perc) & " ,          0          , 0                   ," & DT1.Rows(I).Item(4) & ",'" & NewCode & "',   '" & Str(Val(vCgst_Amt)) & "'        ,   '" & Str(Val(vSgst_Amt)) & "'     , '" & Str(Val(vIgst_AMt)) & "')"

                CMD.ExecuteNonQuery()

            Next
        End If

        DT1.Clear()
        da.Dispose()


        da = New SqlClient.SqlDataAdapter(" Select I.EndsCount_Name, (I.EndsCount_Name + ' - WARP'  ) , IG.Item_HSN_Code , IG.Item_GST_Percentage , (sum(SD.Pavu_Meters)*SD.Rate_Meters) As TaxableAmt, sum(SD.Pavu_Meters) as Qty, 201 as SlNo, 'MTR' AS Units  , tz.Company_State_IdNo , Lh.Ledger_State_Idno   " &
                                          " from Pavu_Sales_Head SD Inner Join EndsCount_Head I On SD.EndsCount_IdNo = I.EndsCount_IdNo INNER JOIN Count_Head Ch On Ch.Count_Idno = I.Count_Idno " &
                                          " Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo  INNER Join Ledger_Head Lh ON Lh.Ledger_Idno =  SD.DeliveryTo_IdNo   INNER JOIN Company_Head tz On tz.Company_Idno = SD.Company_Idno  Where SD.Pavu_Sales_Code = '" & Trim(NewCode) & "' and SD.Pavu_Meters > 0 Group By " &
                                          " I.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage,SD.Rate_Meters  , tz.Company_State_IdNo , Lh.Ledger_State_Idno   ", con)
        DT1 = New DataTable
        da.Fill(DT1)
        If DT1.Rows.Count > 0 Then
            For I = 0 To DT1.Rows.Count - 1

                vTax_Perc = 0

                ' If Val(DT1.Rows(I).Item("GST_Tax_Invoice_Status")) = 1 Then
                If DT1.Rows(I).Item("Company_State_IdNo") = DT1.Rows(I).Item("Ledger_State_Idno") Then

                    If Val(DT1.Rows(I).Item(3).ToString) <> 0 Then
                        vCgst_Amt = ((DT1.Rows(I).Item(4) * Val(DT1.Rows(I).Item(3).ToString) / 100) / 2)
                        vSgst_Amt = vCgst_Amt
                        vIgst_AMt = 0

                        vTax_Perc = DT1.Rows(I).Item(3).ToString
                    Else
                        vCgst_Amt = 0
                        vSgst_Amt = 0
                        vIgst_AMt = 0
                        vTax_Perc = 0
                    End If


                Else

                    If Val(DT1.Rows(I).Item(3).ToString) <> 0 Then
                        vIgst_AMt = (DT1.Rows(I).Item(4) * Val(DT1.Rows(I).Item(3).ToString) / 100)
                        vCgst_Amt = 0
                        vSgst_Amt = 0
                        vTax_Perc = DT1.Rows(I).Item(3).ToString
                    Else
                        vIgst_AMt = 0
                        vCgst_Amt = 0
                        vSgst_Amt = 0
                        vTax_Perc = 0
                    End If

                    End If




                'Else

                '    vIgst_AMt = 0
                '    vCgst_Amt = 0
                '    vSgst_Amt = 0

                'End If

                CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                    ,     [QuantityUnit] ,             Tax_Perc      ,	[CessRate]       ,	[CessNonAdvol]  ,	[TaxableAmount]               , InvCode      ,                   Cgst_Value  ,                       Sgst_Value ,                         Igst_Value) " &
                      " values                 (" & DT1.Rows(I).Item(6).ToString & ",'" & DT1.Rows(I).Item(0) & "', '" & DT1.Rows(I).Item(1) & "', '" & DT1.Rows(I).Item(2) & "', " & DT1.Rows(I).Item(5).ToString & ",         'KGS'          ," & Val(vTax_Perc) & " ,          0          , 0                   ," & DT1.Rows(I).Item(4) & ",'" & NewCode & "',   '" & Str(Val(vCgst_Amt)) & "'        ,   '" & Str(Val(vSgst_Amt)) & "'     , '" & Str(Val(vIgst_AMt)) & "')"
                CMD.ExecuteNonQuery()

            Next
        End If




        ''-------------


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

        ' ------

        btn_GENERATEEWB.Enabled = False

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Pavu_Sales_Head", "eway_bill_no", "Pavu_Sales_Code", Pk_Condition)


    End Sub

    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        'Dim einv As New eInvoice(Val(lbl_Company.Tag))
        'einv.GetAuthToken(rtbEWBResponse)

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GetAuthToken(rtbEWBResponse)
    End Sub
    Private Sub btn_CancelEWB_1_Click(sender As Object, e As EventArgs) Handles btn_CancelEWB_1.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_eWayBill_No.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Pavu_Sales_Head", "eway_bill_no", "Pavu_Sales_Code")

    End Sub
    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 0)
    End Sub
    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 1)
    End Sub
    Private Sub txt_eWayBill_No_TextChanged(sender As Object, e As EventArgs) Handles txt_eWayBill_No.TextChanged
        txt_EWBNo.Text = txt_eWayBill_No.Text
    End Sub

    Private Sub txt_eInvoiceNo_Enter(sender As Object, e As EventArgs) Handles txt_eInvoiceNo.Enter
        btn_Generate_eInvoice.Enabled = True
    End Sub
    Private Sub txt_EInvoiceCancellationReson_Enter(sender As Object, e As EventArgs) Handles txt_EInvoiceCancellationReson.Enter
        btn_Generate_eInvoice.Enabled = True
    End Sub

    Private Sub txt_EWB_Canellation_Reason_Enter(sender As Object, e As EventArgs) Handles txt_EWB_Canellation_Reason.Enter
        btn_Generate_eInvoice.Enabled = True
    End Sub

    Private Sub txt_eInvoiceAckNo_Enter(sender As Object, e As EventArgs) Handles txt_eInvoiceAckNo.Enter
        btn_Generate_eInvoice.Enabled = True
    End Sub

    Private Sub txt_eInvoiceAckDate_Enter(sender As Object, e As EventArgs) Handles txt_eInvoiceAckDate.Enter
        btn_Generate_eInvoice.Enabled = True
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 38 Then
            txt_AddLess.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Close_EWB_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub

    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        txt_EWay_billNo.Text = txt_EWBNo.Text
    End Sub
End Class