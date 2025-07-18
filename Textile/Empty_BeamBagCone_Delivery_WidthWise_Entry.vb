Imports System.Drawing.Printing
Imports System.IO
Public Class Empty_BeamBagCone_Delivery_WidthWise_Entry


    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "EBDLV-"
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private Prec_ActCtrl As New Control
    Private prn_PageNo As Integer
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private dgv_ActCtrlName As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private vPrnt_2Copy_In_SinglePage As Integer = 0
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_Count As Integer = 0
    Private prn_TotCopies As Integer = 0
    Private prn_PageNo1 As Integer
    Private prn_DetIndx1 As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo1 As Integer
    Private prn_DetSNo As Integer
    Private prn_NoofBmDets As Integer
    Private prn_NoofBmDets1 As Integer
    Private Prnt_HalfSheet_STS As Boolean = False
    Private prn_HeadIndx As Integer
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private Print_PDF_Status As Boolean = False

    Private EMAIL_Status As Boolean = False
    Private WHATSAPP_Status As Boolean = False

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_back.Enabled = True
        pnl_filter.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1
        chk_Verified_Status.Checked = False

        lbl_dcno.Text = ""
        lbl_dcno.ForeColor = Color.Black
        msk_date.Text = ""
        dtp_date.Text = ""
        cbo_partyname.Text = ""
        cbo_RecForm.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
        cbo_vehicleno.Text = ""
        txt_emptycones.Text = ""
        txt_Purpose_Of_Delivery.Text = ""
        txt_remarks.Text = ""
        txt_emptybags.Text = ""
        Print_PDF_Status = False

        txt_JumpoEmpty.Text = ""
        txt_EmptyBobin_Party.Text = ""
        txt_emptyBobin.Text = ""
        txt_Party_DcNo.Text = ""
        cbo_beamwidth.Text = ""
        cbo_Vendor.Text = ""
        cbo_LoomType_Creation.Text = ""
        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()

        Grid_Cell_DeSelect()
        cbo_beamwidth.Visible = False
        cbo_beamwidth.Tag = -1
        cbo_Vendor.Visible = False
        cbo_Vendor.Tag = -1
        cbo_LoomType_Creation.Visible = False
        cbo_LoomType_Creation.Tag = -1

        chk_GSTTax_Invocie.Checked = True

        txt_Purpose_Of_Delivery.Text = "Jobwork WarpFilling Purpose Return,Not For Sale "

        txt_EmptyBeam_PrefixNo.Text = ""
        cbo_EmptyBeam_SufixNo.Text = ""
        txt_Empty_Beam_Hsn.Text = "730890"
        txt_Gst_Tax.Text = "5"

        chk_Ewb_No_Sts.Checked = False
        txt_EWBNo.Text = ""


        cbo_DeliveryAt.Text = ""


        vPrnt_2Copy_In_SinglePage = 0

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name ,c.Ledger_Name as RecName from Empty_BeamBagCone_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.ReceivedFrom_IdNo = c.Ledger_IdNo where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                txt_EmptyBeam_PrefixNo.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_PrefixNo").ToString
                cbo_EmptyBeam_SufixNo.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_SuffixNo").ToString
                lbl_dcno.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_RefNo").ToString

                'lbl_dcno.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString
                msk_date.Text = dtp_date.Text
                cbo_partyname.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_RecForm.Text = dt1.Rows(0).Item("RecName").ToString
                txt_emptybags.Text = dt1.Rows(0).Item("Empty_Bags").ToString
                'cbo_beamwidth.Text = Common_Procedures.BeamWidth_IdNoToName(con, Val(dt1.Rows(0).Item("Beam_Width_IdNo").ToString))
                txt_emptycones.Text = dt1.Rows(0).Item("Empty_Cones").ToString
                cbo_vehicleno.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_Purpose_Of_Delivery.Text = dt1.Rows(0).Item("Remarks").ToString
                txt_emptyBobin.Text = dt1.Rows(0).Item("Empty_Bobin").ToString
                txt_JumpoEmpty.Text = dt1.Rows(0).Item("Empty_Jumbo").ToString
                txt_EmptyBobin_Party.Text = dt1.Rows(0).Item("EmptyBobin_Party").ToString
                txt_Party_DcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True
                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False
                txt_Purpose_Of_Delivery.Text = dt1.Rows(0).Item("Purpose_Of_Delv").ToString
                txt_Empty_Beam_Hsn.Text = dt1.Rows(0).Item("Empty_Beam_HSN_Code").ToString
                txt_Gst_Tax.Text = dt1.Rows(0).Item("GST_Percentage").ToString
                cbo_DeliveryAt.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Deliveryto_idno").ToString))
                txt_EWBNo.Text = dt1.Rows(0).Item("EwayBill_No").ToString
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                If Trim(txt_EWBNo.Text) <> "" Then
                    chk_Ewb_No_Sts.Checked = True
                Else
                    chk_Ewb_No_Sts.Checked = False
                End If

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Beam_Width_Name from Empty_BeamBagCone_Delivery_Details a LEFT OUTER JOIN Beam_Width_Head b ON a.Beam_Width_IdNo = b.Beam_Width_IdNo   Where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        Sno = Sno + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_Details.Rows(n).Cells(1).Value = Val(dt2.Rows(i).Item("Empty_Beam").ToString)
                        dgv_Details.Rows(n).Cells(2).Value = Common_Procedures.Vendor_IdNoToName(con, Val(dt2.Rows(i).Item("Vendor_IdNo").ToString))
                        dgv_Details.Rows(n).Cells(3).Value = (dt2.Rows(i).Item("Beam_Width_Name").ToString)
                        dgv_Details.Rows(n).Cells(4).Value = Common_Procedures.LoomType_IdNoToName(con, Val(dt2.Rows(i).Item("LoomType_Idno").ToString))
                        dgv_Details.Rows(n).Cells(5).Value = (dt2.Rows(i).Item("Beam_Nos").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = (dt2.Rows(i).Item("Beam_Width_Rate").ToString)
                        dgv_Details.Rows(n).Cells(7).Value = (dt2.Rows(i).Item("Amount").ToString)

                    Next i

                End If
                dt2.Clear()

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Empty_beam").ToString)
                    '.Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Consumption").ToString), "########0.000")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.000")
                End With
            End If

            dt1.Dispose()
            da1.Dispose()
            Grid_Cell_DeSelect()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_partyname.Visible And cbo_partyname.Enabled Then cbo_partyname.Focus()

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_WidthWise_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_partyname.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_partyname.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RecForm.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_RecForm.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_beamwidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_beamwidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Vendor.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Vendor.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LoomType_Creation.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMTYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_LoomType_Creation.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If


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

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim mskdtxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            mskdtxbx = Me.ActiveControl
            mskdtxbx.SelectionStart = 0

        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_beamwidth.Name Then
            cbo_beamwidth.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Vendor.Name Then
            cbo_Vendor.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_LoomType_Creation.Name Then
            cbo_LoomType_Creation.Visible = False
        End If


        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black

            ElseIf Prec_ActCtrl.Name = txt_Empty_Beam_Hsn.Name Or Prec_ActCtrl.Name = txt_Gst_Tax.Name Then
                Prec_ActCtrl.BackColor = Color.AntiqueWhite
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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        'dgv_Filter_Details.CurrentCell.Selected = False
    End Sub


    Private Sub Empty_BeamBagCone_Delivery_WidthWise_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable

        Me.Text = ""

        con.Open()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)
            Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
        Else
            Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
            Label11.Text = "EMPTY BEAM DELIVERY"
        End If
        Da.Fill(Dt1)
        cbo_partyname.DataSource = Dt1
        cbo_partyname.DisplayMember = "Ledger_DisplayName"


        Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
        Da.Fill(Dt3)
        cbo_RecForm.DataSource = Dt3
        cbo_RecForm.DisplayMember = "Ledger_DisplayName"


        Da = New SqlClient.SqlDataAdapter("select Distinct(vehicle_No) from Empty_BeamBagCone_Delivery_Head order by Vehicle_No", con)
        Da.Fill(Dt2)
        cbo_vehicleno.DataSource = Dt2
        cbo_vehicleno.DisplayMember = "Vehicle_No"

        txt_EmptyBeam_PrefixNo.Visible = True
        cbo_EmptyBeam_SufixNo.Visible = True

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20



        chk_Verified_Status.Visible = False
        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then chk_Verified_Status.Visible = True
        End If

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        If Common_Procedures.settings.Beam_WidthWise_Delivery_Status = 1 Then
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            dgv_Details.Columns(2).Visible = False
            dgv_Details_Total.Columns(2).Visible = False

            dgv_Details.Columns(5).Width = 350
            dgv_Details_Total.Columns(5).Width = 350

            txt_EmptyBeam_PrefixNo.Visible = True
            cbo_EmptyBeam_SufixNo.Visible = True



        End If

        cbo_EmptyBeam_SufixNo.Items.Clear()
        cbo_EmptyBeam_SufixNo.Items.Add("")
        cbo_EmptyBeam_SufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_EmptyBeam_SufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))
        cbo_EmptyBeam_SufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate))
        cbo_EmptyBeam_SufixNo.Items.Add("/" & Trim(Year(Common_Procedures.Company_FromDate)) & "-" & Trim(Microsoft.VisualBasic.Right(Year(Common_Procedures.Company_ToDate), 2)))



        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_partyname.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RecForm.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_beamwidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vendor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vehicleno.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptybags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptycones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptyBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_JumpoEmpty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBobin_Party.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Party_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Purpose_Of_Delivery.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBeam_PrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EmptyBeam_SufixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomType_Creation.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryAt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_beamwidth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vendor.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_partyname.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RecForm.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vehicleno.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptybags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptyBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptycones.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_JumpoEmpty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBobin_Party.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Party_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Purpose_Of_Delivery.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBeam_PrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EmptyBeam_SufixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomType_Creation.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_emptybags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_emptycones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_JumpoEmpty.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyBobin_Party.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Party_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_emptyBobin.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_emptybags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_emptycones.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_JumpoEmpty.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBobin_Party.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Party_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_emptyBobin.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_WidthWise_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_WidthWise_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
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


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_dcno.Text)



        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, New_Entry, Me, con, "Empty_BeamBagCone_Delivery_Head", "Empty_BeamBagCone_Delivery_Code", NewCode, "Empty_BeamBagCone_Delivery_Date", "(Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub



        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Empty_BeamBagCone_Delivery_head", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Empty_BeamBagCone_Delivery_Code, Company_IdNo, for_OrderBy", tr)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Empty_BeamBagCone_Delivery_Details", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "  Empty_Beam,Vendor_IdNo,Beam_Width_IdNo", "Sl_No", "Empty_BeamBagCone_Delivery_Code, For_OrderBy, Company_IdNo, Empty_BeamBagCone_Delivery_No, Empty_BeamBagCone_Delivery_Date, Ledger_Idno", tr)


            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Empty_BeamBagCone_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()
        'If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            dtp_FilterFrom_date.Text = ""
            dtp_FilterTo_date.Text = ""
            pnl_filter.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_filter.Rows.Clear()

            da.Dispose()

        End If

        pnl_filter.Visible = True
        pnl_filter.Enabled = True
        pnl_filter.BringToFront()
        pnl_back.Enabled = False
        If dtp_FilterFrom_date.Enabled And dtp_FilterFrom_date.Visible Then dtp_FilterFrom_date.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, New_Entry, Me) = False Then Exit Sub


        Try

            inpno = InputBox("Enter New Dc.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Empty_BeamBagCone_Delivery_RefNo from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Dc.No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_dcno.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try

            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Delivery_RefNo from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Empty_BeamBagCone_Delivery_RefNo"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Delivery_RefNo from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Empty_BeamBagCone_Delivery_RefNo desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_dcno.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Delivery_RefNo from Empty_BeamBagCone_Delivery_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Empty_BeamBagCone_Delivery_RefNo"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_dcno.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Delivery_RefNo from Empty_BeamBagCone_Delivery_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Empty_BeamBagCone_Delivery_RefNo desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_dcno.Text = NewID
            lbl_dcno.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString




            ' dtp_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Empty_BeamBagCone_Delivery_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False
                txt_Empty_Beam_Hsn.Text = Trim(dt1.Rows(0).Item("Empty_Beam_HSN_Code").ToString)
                txt_Gst_Tax.Text = Val(dt1.Rows(0).Item("GST_Percentage").ToString)
                txt_Purpose_Of_Delivery.Text = Trim(dt1.Rows(0).Item("Purpose_of_Delv").ToString)

                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                    If dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString
                End If

                If Trim(dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_PrefixNo").ToString) <> "" Then txt_EmptyBeam_PrefixNo.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_PrefixNo").ToString
                If Trim(dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_suffixNo").ToString) <> "" Then cbo_EmptyBeam_SufixNo.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_suffixNo").ToString


            End If
            dt1.Clear()
            If msk_date.Enabled And msk_date.Visible Then
                msk_date.Focus()
                msk_date.SelectionStart = 0
            End If
            'If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()




        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Dc.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Empty_BeamBagCone_Delivery_RefNo from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Dc.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim nr As Long = 0
        Dim Sno As Integer = 0
        Dim Bw_ID As Integer = 0
        Dim led_id As Integer = 0
        Dim Rec_id As Integer = 0
        Dim Partcls As String, PBlNo As String, EntID As String
        Dim vTotetybm As Single
        Dim Vndr_Id As Integer = 0
        Dim vLoomType_Idno As Integer = 0

        Dim vTotamt As Single
        Dim DelAt_id As Integer = 0
        Dim Trans_id As Integer = 0



        Dim Verified_STS As String = ""

        Dim vOrdByNo As String = ""

        Dim vGST_Tax_Inv_Sts As Integer = 0

        Dim vInvNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_dcno.Text)
        DelAt_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryAt.Text)
        Trans_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)



        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, New_Entry, Me, con, "Empty_BeamBagCone_Delivery_Head", "Empty_BeamBagCone_Delivery_Code", NewCode, "Empty_BeamBagCone_Delivery_Date", "(Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Empty_BeamBagCone_Delivery_No desc", dtp_date.Value.Date) = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "Empty_BeamBagCone_Delivery_Head", "Verified_Status", "(Empty_BeamBagCone_Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If



        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled Then msk_date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)

        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_partyname.Enabled Then cbo_partyname.Focus()
            Exit Sub
        End If

        Rec_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)
        If Rec_id = 0 Then Rec_id = 4
        If Trim(txt_Party_DcNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            da = New SqlClient.SqlDataAdapter("select * from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(led_id)) & " and Party_dcno = '" & Trim(txt_Party_DcNo.Text) & "' and Empty_BeamBagCone_Delivery_code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Empty_BeamBagCone_Delivery_code <> '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Party Dc No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Party_DcNo.Enabled And txt_Party_DcNo.Visible Then txt_Party_DcNo.Focus()
                Exit Sub
            End If
            dt1.Clear()
        End If

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1

        vGST_Tax_Inv_Sts = 0
        If chk_GSTTax_Invocie.Checked = True Then vGST_Tax_Inv_Sts = 1

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(1).Value) <> 0 Then

                    If Val(.Rows(i).Cells(1).Value) = 0 Then
                        MessageBox.Show("Invalid Empty Beam", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If


                End If

            Next
        End With

        Total_Calculation()

        vTotetybm = 0
        vTotamt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotetybm = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
            ' vTotconMtrs = Val(dgv_BobinDetails_Total.Rows(0).Cells(4).Value())
            vTotamt = Val(dgv_Details_Total.Rows(0).Cells(7).Value())

        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
                da.SelectCommand.Transaction = tr
                da.Fill(dt4)


                NewNo = 0
                If dt4.Rows.Count > 0 Then
                    If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                        NewNo = Int(Val(dt4.Rows(0)(0).ToString))
                        NewNo = Val(NewNo) + 1
                    End If
                End If
                dt4.Clear()
                If Trim(NewNo) = "" Then NewNo = Trim(lbl_dcno.Text)

                lbl_dcno.Text = Trim(NewNo)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If



            vInvNo = Trim(txt_EmptyBeam_PrefixNo.Text) & Trim(lbl_dcno.Text) & Trim(cbo_EmptyBeam_SufixNo.Text)

            'vInvNo = Trim(txt_EmptyBeam_PrefixNo.Text) & " " & Trim(lbl_dcno.Text) & " " & Trim(cbo_EmptyBeam_SufixNo.Text)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Empty_BeamBagCone_Delivery_Head ( Empty_BeamBagCone_Delivery_Code, Company_IdNo, Empty_BeamBagCone_Delivery_No, Empty_BeamBagCone_Delivery_RefNo, for_OrderBy,Empty_BeamBagCone_Delivery_Date, Ledger_IdNo, Empty_Bags,  Empty_Cones, Empty_Bobin ,EmptyBobin_Party,Empty_Jumbo,Vehicle_No,Remarks ,ReceivedFrom_IdNo ,Party_DcNo,Empty_Beam,Verified_Status,GST_Tax_Invoice_Status, Purpose_Of_Delv , Empty_BeamBagCone_Delivery_PrefixNo , Empty_BeamBagCone_Delivery_SuffixNo , Empty_Beam_HSN_Code , GST_Percentage , Total_Amount , EwayBill_No , DeliveryTo_IdNo , Transport_idno) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ",'" & Trim(vInvNo) & "', '" & Trim(lbl_dcno.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcno.Text))) & ", @DeliveryDate," & Val(led_id) & ", " & Val(txt_emptybags.Text) & ",  " & Val(txt_emptycones.Text) & ", " & Val(txt_emptyBobin.Text) & " ," & Val(txt_EmptyBobin_Party.Text) & " ," & Val(txt_JumpoEmpty.Text) & " ,'" & Trim(cbo_vehicleno.Text) & "','" & Trim(txt_remarks.Text) & "' , " & Val(Rec_id) & ",'" & Trim(txt_Party_DcNo.Text) & "'," & Val(vTotetybm) & ", " & Val(Verified_STS) & "," & Str(Val(vGST_Tax_Inv_Sts)) & ",'" & Trim(txt_Purpose_Of_Delivery.Text) & "' , '" & Trim(txt_EmptyBeam_PrefixNo.Text) & "' , '" & Trim(cbo_EmptyBeam_SufixNo.Text) & "', '" & Trim(txt_Empty_Beam_Hsn.Text) & "' , " & Val(txt_Gst_Tax.Text) & " , " & Str(Val(vTotamt)) & ",'" & Trim(txt_EWBNo.Text) & "'," & Str(Val(DelAt_id)) & ", " & Str(Val(Trans_id)) & " )"

                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Empty_BeamBagCone_Delivery_head", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Empty_BeamBagCone_Delivery_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Empty_BeamBagCone_Delivery_Details", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "  Empty_Beam,Vendor_IdNo,Beam_Width_IdNo", "Sl_No", "Empty_BeamBagCone_Delivery_Code, For_OrderBy, Company_IdNo, Empty_BeamBagCone_Delivery_No, Empty_BeamBagCone_Delivery_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Empty_BeamBagCone_Delivery_Head set Empty_BeamBagCone_Delivery_Date = @DeliveryDate, Ledger_IdNo = " & Val(led_id) & ", Empty_Bags = " & Val(txt_emptybags.Text) & ", Beam_Width_IdNo = " & Val(Bw_ID) & ", Empty_Cones=" & Val(txt_emptycones.Text) & " , Empty_Bobin =" & Val(txt_emptyBobin.Text) & " ,EmptyBobin_Party = " & Val(txt_EmptyBobin_Party.Text) & "  ,Empty_Jumbo = " & Val(txt_JumpoEmpty.Text) & "  ,Vehicle_No='" & Trim(cbo_vehicleno.Text) & "',Remarks='" & Trim(txt_remarks.Text) & "' , ReceivedFrom_IdNo = " & Val(Rec_id) & " , Party_DcNo='" & Trim(txt_Party_DcNo.Text) & "',Empty_Beam =" & Val(vTotetybm) & " ,Verified_Status= " & Val(Verified_STS) & ", GST_Tax_Invoice_Status = " & Str(Val(vGST_Tax_Inv_Sts)) & ", Purpose_Of_Delv = '" & Trim(txt_Purpose_Of_Delivery.Text) & "', Empty_BeamBagCone_Delivery_No = '" & Trim(vInvNo) & "' , Empty_BeamBagCone_Delivery_PrefixNo = '" & Trim(txt_EmptyBeam_PrefixNo.Text) & "' , Empty_BeamBagCone_Delivery_SuffixNo = '" & Trim(cbo_EmptyBeam_SufixNo.Text) & "', Empty_Beam_HSN_Code = '" & Trim(txt_Empty_Beam_Hsn.Text) & "' , GST_Percentage = " & Val(txt_Gst_Tax.Text) & ", Total_Amount = " & Str(Val(vTotamt)) & " , EwayBill_No = '" & Trim(txt_EWBNo.Text) & "' , DeliveryTo_IdNo = " & Val(DelAt_id) & "    Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Empty_BeamBagCone_Delivery_head", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Empty_BeamBagCone_Delivery_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Empty_BeamBagCone_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            EntID = Trim(Pk_Condition) & Trim(lbl_dcno.Text)
            Partcls = "Delv : Dc.No. " & Trim(lbl_dcno.Text)
            PBlNo = Trim(lbl_dcno.Text)



            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(1).Value) <> 0 Then

                        Sno = Sno + 1
                        Vndr_Id = Common_Procedures.Vendor_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Bw_ID = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        vLoomType_Idno = Common_Procedures.LoomType_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)

                        cmd.CommandText = "Insert into Empty_BeamBagCone_Delivery_Details (  Empty_BeamBagCone_Delivery_Code,  Company_IdNo           ,        Empty_BeamBagCone_Delivery_No      ,           for_OrderBy                    , Empty_BeamBagCone_Delivery_Date,     Sl_No             ,               Empty_Beam               ,         Vendor_IdNo    ,      Beam_Width_IdNo    ,                   LoomType_Idno              ,                       Beam_nos         ,               Beam_Width_Rate              ,                  Amount                   ) " &
                                                                        " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "'," & Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcno.Text)) & " , @DeliveryDate                    ,  " & Str(Val(Sno)) & " ,  " & Val(.Rows(i).Cells(1).Value) & ", " & Val(Vndr_Id) & " ," & Str(Val(Bw_ID)) & "  ,       " & Str(Val(vLoomType_Idno)) & "   ,    '" & Trim(.Rows(i).Cells(5).Value) & "' ,  " & Str(Val(.Rows(i).Cells(6).Value)) & " ,  " & Str(Val(.Rows(i).Cells(7).Value)) & ") "
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No,Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Empty_Bobin, EmptyBobin_Party, Empty_Jumbo,Vendor_Idno , LoomType_Idno) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcno.Text))) & ", @DeliveryDate, " & Str(Val(led_id)) & ", " & Str(Val(Rec_id)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   " & Str(Val(Sno)) & ",  " & Str(Val(Bw_ID)) & " ," & Val(.Rows(i).Cells(1).Value) & " ,0 ,0, 0, 0,0 ," & Val(Vndr_Id) & " ," & Str(Val(vLoomType_Idno)) & "   )"
                        cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Empty_BeamBagCone_Delivery_Details", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "  Empty_Beam,Vendor_IdNo,Beam_Width_IdNo", "Sl_No", "Empty_BeamBagCone_Delivery_Code, For_OrderBy, Company_IdNo, Empty_BeamBagCone_Delivery_No, Empty_BeamBagCone_Delivery_Date, Ledger_Idno", tr)

            End With


            If Val(txt_emptybags.Text) <> 0 Or Val(txt_emptycones.Text) <> 0 Or Val(txt_emptyBobin.Text) <> 0 Or Val(txt_EmptyBobin_Party.Text) <> 0 Or Val(txt_JumpoEmpty.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Beam, Empty_Bags, Empty_Cones, Empty_Bobin, EmptyBobin_Party, Empty_Jumbo,Vendor_IdNo) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcno.Text))) & ", @DeliveryDate, " & Str(Val(led_id)) & ", " & Str(Val(Rec_id)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 101, 0, " & Str(Val(txt_emptybags.Text)) & ", " & Str(Val(txt_emptycones.Text)) & ", " & Str(Val(txt_emptyBobin.Text)) & ", " & Str(Val(txt_EmptyBobin_Party.Text)) & ", " & Str(Val(txt_JumpoEmpty.Text)) & " ," & Val(Vndr_Id) & " )"
                cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_dcno.Text)
                End If
            Else
                move_record(lbl_dcno.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
        'If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
    End Sub




    Private Sub cbo_partyname_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_partyname.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_partyname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_partyname.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_partyname, msk_date, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_partyname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_partyname.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_partyname, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_partyname.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_partyname.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_RecForm_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RecForm.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_RecForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecForm, cbo_partyname, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_RecForm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RecForm.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecForm, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_RecForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RecForm.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub



    Private Sub cbo_vehicleno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_vehicleno.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Empty_BeamBagCone_Delivery_Head", "Vehicle_No", "", "Vehicle_No")

    End Sub

    Private Sub cbo_vehicleno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vehicleno.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vehicleno, cbo_Transport, txt_Purpose_Of_Delivery, "Empty_BeamBagCone_Delivery_Head", "Vehicle_No", "", "Vehicle_No")
    End Sub
    Private Sub cbo_vehicleno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vehicleno.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vehicleno, txt_Purpose_Of_Delivery, "Empty_BeamBagCone_Delivery_Head", "Vehicle_No", "", "", False)

    End Sub



    Private Sub txt_emptybeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_emptybags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptybags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_emptycones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptycones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_FilterTo_date, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub dtp_FilterTo_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_FilterTo_date.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub


    Private Sub dtp_FilterTo_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FilterTo_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Itm_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Itm_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Empty_BeamBagCone_Delivery_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Empty_BeamBagCone_Delivery_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Empty_BeamBagCone_Delivery_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Empty_BeamBagCone_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Empty_BeamBagCone_Delivery_No", con)
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Empty_BeamBagCone_Delivery_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Empty_beam").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Empty_Bags").ToString
                    dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Empty_Cones").ToString

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

    End Sub

    Private Sub dtp_FilterFrom_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_FilterFrom_date.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub


    Private Sub dtp_FilterFrom_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FilterFrom_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub btn_filtershow_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btn_filtershow.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub btn_filtershow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles btn_filtershow.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub


    Private Sub btn_closefilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_closefilter.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub dgv_filter_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_filter.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_back.Enabled = True
            pnl_filter.Visible = False
        End If

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Empty_BeamBagCone_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try


        prn_InpOpts = ""

        If Val(Common_Procedures.settings.All_Delivery_Print_Ori_Dup_Trip_Sts) = 1 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then

            If EMAIL_Status = True Or WHATSAPP_Status = True Then
                prn_InpOpts = "1"  ' "123"
            Else
                prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "123")
                prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")

            End If
        End If


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

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


        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt = New DataTable
        prn_DetDt = New DataTable
        prn_PageNo = 0
        prn_DetIndx = 0
        prn_Count = 0

        Try







            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName, Ebh.State_Name as Company_State_Name, Ebh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code ,f.Ledger_mainName as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_Mail as DeliveryTo_LedgerMailNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.MobileNo_Frsms as DeliveryTo_LedgerMobileNo_Frsms, f.Pan_No as DeliveryTo_PanNo , Dsh.State_Name as DeliveryTo_State_Name, Dsh.State_Code as DeliveryTo_State_Code  from Empty_BeamBagCone_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo LEFT OUTER JOIN State_Head Ebh ON b.Company_State_IdNo = Ebh.State_IdNo LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Ledger_Head f ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = f.Ledger_IdNo LEFT OUTER JOIN State_Head Dsh ON f.Ledger_State_IdNo = Dsh.State_IdNo  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)


            prn_HdDt = New DataTable

            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.LoomType_Name,c.Beam_Width_Name from  Empty_BeamBagCone_Delivery_details a LEFT JOIN LoomType_Head b ON a.LoomType_Idno = b.Loomtype_Idno LEFT JOIN Beam_Width_Head c ON a.Beam_width_idno = c.Beam_Width_Idno where company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
                da2.Fill(prn_DetDt)

            Else


                MessageBox.Show("This Is New Entry", "DOES Not PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES Not PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
            Printing_Format2_1464(e)
        Else
            Printing_Format1(e)
        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim I As Integer, NoofItems_PerPage As Integer
        Dim vGST_BILL As Integer = 0
        Dim Sno As Integer
        Dim BMNos1 As String, BMNos2 As String, BMNos3 As String, BMNos4 As String
        Dim EntryCode As String
        Dim NoofDets As Integer
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0





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

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 65
            .Right = 40 '30
            .Top = 40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        pFont = New Font("Calibri", 11, FontStyle.Regular)
        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 14 '15 '16.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(18) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(18) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ClArr(1) = 35 : ClArr(2) = 110 : ClArr(3) = 65 : ClArr(4) = 45 : ClArr(5) = 45 : ClArr(6) = 40 : ClArr(7) = 70 : ClArr(8) = 95 : ClArr(9) = 115 : ClArr(10) = 70
        If Not (prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1) Then
            ClArr(9) = ClArr(9) + ClArr(4)
            ClArr(4) = 0
        End If
        ClArr(11) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10))



        'Printing_Format1_PageHeader(e, EntryCode, TxtHgt, strHeight, pFont, p1Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PageHeight, PrintWidth, PrintHeight, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)



        Dim vS1 As Single = 0
        Dim vS2 As Single = 0
        Dim vS3 As Single = 0
        Dim vS4 As Single = 0
        Dim vS5 As Single = 0
        Dim vS6 As Single = 0
        Dim vS7 As Single = 0
        Dim vS8 As Single = 0


        vS1 = ClArr(1) + ClArr(2)
        vS2 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4)
        vS3 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6)
        vS4 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7)
        vS5 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8)
        vS6 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9)
        vS7 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10)
        vS8 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11)


        '   
        Try

            If prn_HdDt.Rows.Count > 0 Then
                vGST_BILL = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)
                'Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, strHeight, pFont, p1Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PageHeight, PrintWidth, PrintHeight, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                NoofItems_PerPage = 2

                If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) = "" Then
                    NoofItems_PerPage = NoofItems_PerPage + 1
                End If

                'If Val(Common_Procedures.settings.WeaverWagesYarnReceipt_Print_2Copy_In_SinglePage) = 1 Then
                '    If prn_DetDt.Rows.Count > NoofItems_PerPage Then
                '        NoofItems_PerPage = 35
                '    End If
                'End If

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + vS5, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1


                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, strHeight, pFont, p1Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PageHeight, PrintWidth, PrintHeight, prn_PageNo, NoofItems_PerPage, NoofDets, False, CurY, LnAr, ClArr)



                            e.HasMorePages = True
                            Return

                        End If

                        BMNos1 = ""
                        BMNos2 = ""
                        BMNos3 = ""
                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Beam_Nos").ToString) <> "" Then
                            BMNos1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Beam_Nos").ToString)
                            'BMNos1 = "BEAM No.s  : " & Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString)
                        End If

                        If Len(BMNos1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(BMNos1), I, 1) = " " Or Mid$(Trim(BMNos1), I, 1) = "," Or Mid$(Trim(BMNos1), I, 1) = "." Or Mid$(Trim(BMNos1), I, 1) = "-" Or Mid$(Trim(BMNos1), I, 1) = "/" Or Mid$(Trim(BMNos1), I, 1) = "_" Or Mid$(Trim(BMNos1), I, 1) = "(" Or Mid$(Trim(BMNos1), I, 1) = ")" Or Mid$(Trim(BMNos1), I, 1) = "\" Or Mid$(Trim(BMNos1), I, 1) = "[" Or Mid$(Trim(BMNos1), I, 1) = "]" Or Mid$(Trim(BMNos1), I, 1) = "{" Or Mid$(Trim(BMNos1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            BMNos2 = Microsoft.VisualBasic.Right(Trim(BMNos1), Len(BMNos1) - I)
                            BMNos1 = Microsoft.VisualBasic.Left(Trim(BMNos1), I)
                        End If

                        If Len(BMNos2) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(BMNos2), I, 1) = " " Or Mid$(Trim(BMNos2), I, 1) = "," Or Mid$(Trim(BMNos2), I, 1) = "." Or Mid$(Trim(BMNos2), I, 1) = "-" Or Mid$(Trim(BMNos2), I, 1) = "/" Or Mid$(Trim(BMNos2), I, 1) = "_" Or Mid$(Trim(BMNos2), I, 1) = "(" Or Mid$(Trim(BMNos2), I, 1) = ")" Or Mid$(Trim(BMNos2), I, 1) = "\" Or Mid$(Trim(BMNos2), I, 1) = "[" Or Mid$(Trim(BMNos2), I, 1) = "]" Or Mid$(Trim(BMNos2), I, 1) = "{" Or Mid$(Trim(BMNos2), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            BMNos3 = Microsoft.VisualBasic.Right(Trim(BMNos2), Len(BMNos2) - I)
                            BMNos2 = Microsoft.VisualBasic.Left(Trim(BMNos2), I)
                        End If

                        If Len(BMNos3) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(BMNos3), I, 1) = " " Or Mid$(Trim(BMNos3), I, 1) = "," Or Mid$(Trim(BMNos3), I, 1) = "." Or Mid$(Trim(BMNos3), I, 1) = "-" Or Mid$(Trim(BMNos3), I, 1) = "/" Or Mid$(Trim(BMNos3), I, 1) = "_" Or Mid$(Trim(BMNos3), I, 1) = "(" Or Mid$(Trim(BMNos3), I, 1) = ")" Or Mid$(Trim(BMNos3), I, 1) = "\" Or Mid$(Trim(BMNos3), I, 1) = "[" Or Mid$(Trim(BMNos3), I, 1) = "]" Or Mid$(Trim(BMNos3), I, 1) = "{" Or Mid$(Trim(BMNos3), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            BMNos4 = Microsoft.VisualBasic.Right(Trim(BMNos3), Len(BMNos3) - I)
                            BMNos3 = Microsoft.VisualBasic.Left(Trim(BMNos3), I)
                        End If




                        If Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Empty_Beam").ToString)) <> 0 Then


                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("sl_no").ToString, LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, "Empty Beam", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Beam_HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                            If Val(ClArr(4)) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pFont)
                            End If
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Empty_Beam").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, "Nos", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Beam_Width_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)

                            p1Font = New Font("Calibri", 9, FontStyle.Regular)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("LoomType_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, p1Font)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 5, CurY, 0, 0, p1Font)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Beam_Width_Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11), CurY, 1, 0, pFont)


                            If Trim(BMNos2) <> "" Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 5, CurY, 0, 0, p1Font)
                            End If

                            If Trim(BMNos3) <> "" Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 5, CurY, 0, 0, p1Font)
                            End If

                            If Trim(BMNos4) <> "" Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos4), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 5, CurY, 0, 0, p1Font)
                            End If

                        End If

                        prn_DetIndx = prn_DetIndx + 1

                        NoofDets = NoofDets + 1
                    Loop
                End If



                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Or Trim(BMNos2) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Then
                        Sno = Sno + 1

                        Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "Empty Bags", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bag_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)

                        If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3), CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bags").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bags").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bags").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "Nos", LMargin + vS2 + ClArr(5), CurY, 0, 0, pFont)



                    End If

                    'If Trim(BMNos2) <> "" Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos2), LMargin + vS5 + 5, CurY, 0, 0, pFont)
                    'End If
                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) <> 0 Or Trim(BMNos3) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) <> 0 Then
                        Sno = Sno + 1

                        Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "Empty Cones", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cone_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)

                        If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3), CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cones").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cones").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "Nos", LMargin + vS2 + ClArr(5), CurY, 0, 0, pFont)



                    End If
                    'If Trim(BMNos3) <> "" Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos3), LMargin + vS5 + 5, CurY, 0, 0, pFont)
                    'End If
                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bobin").ToString)) <> 0 Or Trim(BMNos4) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bobin").ToString)) <> 0 Then
                        Sno = Sno + 1

                        Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, "Empty Bobin", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cone_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)
                        If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3), CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bobin").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bobin").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "Nos", LMargin + vS2 + ClArr(5), CurY, 0, 0, pFont)


                    End If
                    'If Trim(BMNos4) <> "" Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos4), LMargin + vS5 + 5, CurY, 0, 0, pFont)
                    'End If
                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Jumbo").ToString)) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    Sno = Sno + 1

                    Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "Empty Jumbo", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cone_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)
                    If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3), CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Jumbo").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Jumbo").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, "Nos", LMargin + vS2 + ClArr(5), CurY, 0, 0, pFont)

                End If



                'NoofDets = NoofDets + 1

                'If Trim(ItmNm2) <> "" Then
                '    CurY = CurY + TxtHgt - 5
                '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                '    NoofDets = NoofDets + 1
                'End If

                'prn_DetIndx = prn_DetIndx + 1





                'Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, strHeight, pFont, p1Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PageHeight, PrintWidth, PrintHeight, prn_PageNo, NoofItems_PerPage, NoofDets, True, CurY, LnAr, ClArr)

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
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


        PrntCnt2ndPageSTS = False


LOOP2:



        prn_HeadIndx = prn_HeadIndx + 1
        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True

        Else
            e.HasMorePages = False

        End If

        prn_DetDt.Clear()
        prn_PageNo = 0

        prn_DetIndx = 0
        prn_DetSNo = 0


        e.HasMorePages = False

    End Sub



    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal strheight As Single, ByVal pFont As Font, ByVal p1Font As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal Pageheight As Single, ByVal PrintWidth As Single, ByVal Printheight As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single)

        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim I As Integer
        Dim vGST_BILL As Integer = 0
        Dim Sno As Integer
        Dim BMNos1 As String, BMNos2 As String, BMNos3 As String, BMNos4 As String
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""
        Dim Cmp_Gstin_No As String
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
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt - 3, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Gstin_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.: " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_Gstin_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strheight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height





        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 5, CurY, 80, 80)

                        End If

                    End Using

                End If

            End If

        End If

        CurY = CurY + strheight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 15, FontStyle.Bold)

        vGST_BILL = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1139" Then '---- SIVAKUMAR Textiles (THEKKALUR)
            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM DELIVERY CHALLAN", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        End If


        Common_Procedures.Print_To_PrintDocument(e, Cmp_Gstin_No, LMargin + 10, CurY + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "( Not For Sale )", LMargin + 30, CurY + 15, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SAC : 99882 ( Textile Manufacture )", LMargin, CurY + 15, 2, PrintWidth, pFont)
        strheight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strheight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width

        Common_Procedures.Print_To_PrintDocument(e, "DC.NO  :", LMargin + 20, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString, LMargin + ClArr(1) + ClArr(3) - 20, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "DATE  : ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("EwayBill_No").ToString) <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 55, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EwayBill_No").ToString, LMargin + C1 + W1 + 65, CurY, 0, 0, pFont)

        End If




        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY AT : ", LMargin + C1 + 10, CurY, 0, 0, pFont)




        'W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width






        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Deliveryto_LedgerName").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

        'Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString)), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)



        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : ", LMargin + 15, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + ClArr(3), CurY, 0, 0, pFont)

        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + ClArr(3), CurY, 0, 0, pFont)
        End If



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClArr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClArr(1), CurY, 2, ClArr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN", LMargin + ClArr(1) + ClArr(2), CurY - 10, 2, ClArr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + ClArr(1) + ClArr(2), CurY + 5, 2, ClArr(3), pFont)
        If Val(ClArr(4)) > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "GST", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY - 10, 2, ClArr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY + 5, 2, ClArr(4), pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "UOM", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 2, ClArr(6), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY - 10, 2, ClArr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WIDTH", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY + 5, 2, ClArr(7), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY - 10, 2, ClArr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY + 5, 2, ClArr(8), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BEAM NOS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), CurY, 2, ClArr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9), CurY, 2, ClArr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), CurY, 2, ClArr(11), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal strheight As Single, ByVal pFont As Font, ByVal p1Font As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal Pageheight As Single, ByVal PrintWidth As Single, ByVal Printheight As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single)

        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim I As Integer
        Dim vGST_BILL As Integer = 0
        Dim Sno As Integer
        Dim BMNos1 As String, BMNos2 As String, BMNos3 As String, BMNos4 As String

        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0
        Dim vigst_amt As String = 0
        Dim vTxPerc As String = 0





        'For I = NoofDets + 1 To NoofItems_PerPage
        CurY = CurY + TxtHgt + 10
        'Next


        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        vGST_BILL = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)

        If is_LastPage = True Then

            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClArr(1), CurY, 2, ClArr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Beam").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11), CurY, 1, 0, pFont)

        End If



        CurY = CurY + TxtHgt + 5



        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)




        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), LnAr(3), LMargin + ClArr(1), CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2), LnAr(3), LMargin + ClArr(1) + ClArr(2), CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY)
        If Val(ClArr(4)) > 0 Then
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY)
        End If
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9), CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11), CurY)


        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        If Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) <> 0 And Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

            '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1158" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Value            :  ", LMargin + C1 + 50, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + C1 + 160, CurY, 0, 0, pFont)
            '    Else
            '        Common_Procedures.Print_To_PrintDocument(e, "Taxable Value    :  ", LMargin + C1 + 50, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + C1 + 160, CurY, 0, 0, pFont)
            '    End If
            'Else
            'Common_Procedures.Print_To_PrintDocument(e, "Total Amount :  ", LMargin + C1 + 50, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "############0.00"), LMargin + C1 + 165, CurY, 0, 0, p1Font)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "Transport Name : ", LMargin + 10, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TransportName").ToString), LMargin + ClArr(1) + ClArr(3) + 15, CurY, 0, 0, pFont)
        End If



        'If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO : ", LMargin + 10, CurY, 0, 0, p1Font)
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_name").ToString), LMargin + ClArr(1) + 72, CurY, 0, 0, p1Font)
        'End If
        '-------------------------*********************

        'CurY = CurY + TxtHgt + 5
        vTxPerc = 0
        vCgst_amt = 0
        vSgst_amt = 0
        vigst_amt = 0
        If Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

            If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then
                'vTxPerc = Format(Val(prn_DetDt1.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")
                vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) * 2.5 / 100, "############0.00")
                vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) * 2.5 / 100, "############0.00")

                Common_Procedures.Print_To_PrintDocument(e, " CGST 2.5 % : " & vCgst_amt, LMargin + C1 + 47, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " SGST 2.5 % : " & vSgst_amt, LMargin + C1 + 190, CurY, 0, 0, pFont)

            Else

                'vTxPerc = prn_HdDt.Rows(0).Item("item_gst_percentage").ToString
                vigst_amt = Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) * 5 / 100, "############0.00")

                Common_Procedures.Print_To_PrintDocument(e, " IGST % : " & vigst_amt, LMargin + C1 + 47, CurY, 0, 0, pFont)

            End If
        End If

        'If Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) & "  " & Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString), LMargin + 10, CurY - 3, 0, 0, pFont)
        'End If

        CurY = CurY + TxtHgt
        '-------------------------*********************
        If Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) <> 0 And Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

            vTxamt = Val(vCgst_amt) + Val(vSgst_amt) + Val(vigst_amt)
            vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) + vTxamt, "###########0.00")

            Common_Procedures.Print_To_PrintDocument(e, "Value of Goods : " & vNtAMt, LMargin + C1 + 50, CurY + 5, 0, 0, p1Font)

        End If


        If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            'CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "Through Vehicle No. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
        End If
        '-------------------------*********************

        'If Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Del_Address4").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) & "  " & Trim(prn_HdDt.Rows(0).Item("Del_Address4").ToString), LMargin + 10, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt + 5
        'If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN  : " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString), LMargin + 10, CurY, 0, 0, pFont)
        'End If


        'If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
        '    CurY = CurY + TxtHgt + 5
        '    Common_Procedures.Print_To_PrintDocument(e, "Through Vehicle No. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)
        'End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "REMARKS  : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        End If
        CurY = CurY + TxtHgt - 5

        If Trim(prn_HdDt.Rows(0).Item("Purpose_of_delv").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Purpose : " & Trim(prn_HdDt.Rows(0).Item("Purpose_of_delv").ToString), LMargin + 10, CurY, 0, 0, pFont)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1544" Then
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
        End If


        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 325, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 20, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        If Val(prn_PageNo) > 1 Or is_LastPage = False Then
            CurY = CurY + 5
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Page No. : " & prn_PageNo, LMargin, CurY, 2, PageWidth, p1Font)
        End If

    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_JumpoEmpty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_JumpoEmpty.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_emptyBobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptyBobin.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyBobin_Party_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBobin_Party.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub cbo_beamwidth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_beamwidth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")

    End Sub


    Private Sub cbo_beamwidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_beamwidth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_beamwidth, Nothing, Nothing, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_beamwidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If .CurrentCell.ColumnIndex = 3 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(1)
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                End If
            End If


            If (e.KeyValue = 40 And cbo_beamwidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        msk_date.Focus()
                        'dtp_Date.Focus()
                    End If

                ElseIf .CurrentCell.ColumnIndex = 3 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(4)

                Else
                    .Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

                End If


            End If

        End With
    End Sub

    Private Sub cbo_beamwidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_beamwidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_beamwidth, Nothing, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        msk_date.Focus()
                        'dtp_Date.Focus()
                    End If

                ElseIf .CurrentCell.ColumnIndex = 3 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(4)

                Else
                    .Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_beamwidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_beamwidth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_beamwidth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub msk_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
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

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_date.Text = Date.Today
        End If
        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If
    End Sub

    Private Sub dtp_date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_date.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_Date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_date.ValueChanged
        msk_date.Text = dtp_date.Text
    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.Enter
        msk_date.Focus()
        msk_date.SelectionStart = 0
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.TextChanged

        If IsDate(dtp_date.Text) = True Then

            msk_date.Text = dtp_date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub
    Private Sub dgv_details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit

        dgv_Details_CellLeave(sender, e)

    End Sub

    Private Sub dgv_details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Rect As Rectangle

        With dgv_Details



            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


            If e.ColumnIndex = 2 Then

                If cbo_Vendor.Visible = False Or Val(cbo_Vendor.Tag) <> e.RowIndex Then



                    cbo_Vendor.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Vendor_Name from Vendor_Head Order by Vendor_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Vendor.DataSource = Dt1
                    cbo_Vendor.DisplayMember = "Vendor_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Vendor.Left = .Left + Rect.Left
                    cbo_Vendor.Top = .Top + Rect.Top
                    cbo_Vendor.Width = Rect.Width
                    cbo_Vendor.Height = Rect.Height

                    cbo_Vendor.Text = .CurrentCell.Value

                    cbo_Vendor.Tag = Val(e.RowIndex)
                    cbo_Vendor.Visible = True

                    cbo_Vendor.BringToFront()
                    cbo_Vendor.Focus()



                End If

            Else

                cbo_Vendor.Visible = False

            End If


            If e.ColumnIndex = 3 Then

                If cbo_beamwidth.Visible = False Or Val(cbo_beamwidth.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_Details.Name

                    cbo_beamwidth.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from Beam_Width_Head Order by Beam_Width_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_beamwidth.DataSource = Dt2
                    cbo_beamwidth.DisplayMember = "Beam_Width_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_beamwidth.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_beamwidth.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_beamwidth.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_beamwidth.Height = Rect.Height  ' rect.Height

                    cbo_beamwidth.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_beamwidth.Tag = Val(e.RowIndex)
                    cbo_beamwidth.Visible = True

                    cbo_beamwidth.BringToFront()
                    cbo_beamwidth.Focus()

                End If

            Else

                cbo_beamwidth.Visible = False

            End If

            If e.ColumnIndex = 4 Then

                If cbo_LoomType_Creation.Visible = False Or Val(cbo_LoomType_Creation.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_Details.Name

                    cbo_LoomType_Creation.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select LoomTYpe_Name from LoomTYpe_Head Order by LoomTYpe_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_beamwidth.DataSource = Dt3
                    cbo_beamwidth.DisplayMember = "LoomTYpe_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_LoomType_Creation.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_LoomType_Creation.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_LoomType_Creation.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_LoomType_Creation.Height = Rect.Height  ' rect.Height

                    cbo_LoomType_Creation.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_LoomType_Creation.Tag = Val(e.RowIndex)
                    cbo_LoomType_Creation.Visible = True

                    cbo_LoomType_Creation.BringToFront()
                    cbo_LoomType_Creation.Focus()

                End If

            Else

                cbo_LoomType_Creation.Visible = False

            End If


        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 1 And .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0")

                End If

            ElseIf .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If

        End With
        Total_Calculation()
    End Sub

    Private Sub dgv_details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                    If .Rows(e.RowIndex).Cells(6).Value <> 0 Then
                        .Rows(e.RowIndex).Cells(7).Value = Val(.Rows(e.RowIndex).Cells(1).Value) * Val(.Rows(e.RowIndex).Cells(6).Value)

                    End If
                End If
                Total_Calculation()

            End If
        End With

    End Sub

    Private Sub dgv_details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        'dgv_ActCtrlName = dgv_Details.Name
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details

            If Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 1 Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

            End If

        End With

    End Sub

    Private Sub dgv_details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

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
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            Total_Calculation()

        End If

    End Sub

    Private Sub dgv_details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With

    End Sub

    Private Sub dgv_details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub
    Private Sub Total_Calculation()
        Dim vTotetybm As Single
        Dim vTotrate As Single
        Dim vTotamt As Single
        Dim i As Integer
        Dim sno As Integer

        vTotetybm = 0
        vTotrate = 0
        vTotamt = 0
        With dgv_Details
            For i = 0 To .Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Then

                    vTotetybm = vTotetybm + Val(.Rows(i).Cells(1).Value)
                    vTotamt = vTotamt + Val(.Rows(i).Cells(7).Value)

                End If
            Next
        End With

        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()

        dgv_Details_Total.Rows(0).Cells(1).Value = Val(vTotetybm)
        ' dgv_etails_Total.Rows(0).Cells(4).Value = Format(Val(vTotMtrs), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(7).Value = Format(Val(vTotamt), "########0")








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
            ElseIf pnl_back.Enabled = True Then
                dgv1 = dgv_Details
            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    msk_date.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 1 Then

                            .Focus()
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(3)


                        Else

                            .Focus()
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                'txt_Purpose_Of_Delivery.Focus()
                                txt_remarks.Focus()

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

    Private Sub cbo_beamwidth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_beamwidth.TextChanged
        Try
            If cbo_beamwidth.Visible Then
                With dgv_Details
                    If Val(cbo_beamwidth.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_beamwidth.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Vendor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vendor.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vendor_Head", "Vendor_Name", "", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_Vendor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vendor.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vendor, Nothing, Nothing, "Vendor_Head", "Vendor_Name", "", "(Vendor_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Vendor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Vendor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(3)




            End If

        End With
    End Sub



    Private Sub cbo_Vendor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vendor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vendor, Nothing, "Vendor_Head", "Vendor_Name", "", "(Vendor_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(3)


            End With

        End If

    End Sub

    Private Sub cbo_Vendor_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vendor.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Vendor_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Vendor.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Vendor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vendor.TextChanged
        Try
            If cbo_Vendor.Visible Then
                With dgv_Details
                    If Val(cbo_Vendor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Vendor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub msk_date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus
        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2000 Then
                    dtp_date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
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

    Private Sub cbo_Vendor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Vendor.SelectedIndexChanged

    End Sub

    Private Sub cbo_LoomType_Creation_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_LoomType_Creation.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LoomType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LoomType_Creation.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If


    End Sub

    Private Sub cbo_LoomType_Creation_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_LoomType_Creation.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomType_Creation, "", "LoomType_Head", "LoomType_Name", "", "(LoomType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        msk_date.Focus()
                        'dtp_Date.Focus()
                    End If

                ElseIf .CurrentCell.ColumnIndex = 4 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(5)

                Else
                    .Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

                End If
            End With

        End If


    End Sub

    Private Sub cbo_LoomType_Creation_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_LoomType_Creation.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomType_Creation, cbo_beamwidth, "", "LoomTYpe_Head", "LoomTYpe_Name", "", "(LoomTYpe_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_LoomType_Creation.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If .CurrentCell.ColumnIndex = 5 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(4)
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                End If
            End If


            If (e.KeyValue = 40 And cbo_LoomType_Creation.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        msk_date.Focus()
                        'dtp_Date.Focus()
                    End If

                ElseIf .CurrentCell.ColumnIndex = 4 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(5)

                Else
                    .Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

                End If


            End If

        End With


    End Sub

    Private Sub cbo_LoomType_Creation_GotFocus(sender As Object, e As EventArgs) Handles cbo_LoomType_Creation.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "LoomTYpe_Head", "LoomType_Name", "", "(LoomType_IdNo = 0)")
    End Sub

    Private Sub txt_remarks_TextChanged(sender As Object, e As EventArgs) Handles txt_remarks.TextChanged

    End Sub

    Private Sub txt_remarks_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True

        End If
    End Sub

    Private Sub txt_remarks_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_remarks.KeyDown
        If e.KeyValue = 38 Then
            txt_Gst_Tax.Focus()

            'SendKeys.Send("+{TAB}")
        End If
        If (e.KeyValue = 40) Then

            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True

        End If
    End Sub



    Private Sub txt_Purpose_Of_Delivery_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Purpose_Of_Delivery.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Empty_Beam_Hsn.Focus()
        End If
    End Sub


    Private Sub txt_Purpose_Of_Delivery_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Purpose_Of_Delivery.KeyDown
        If e.KeyCode = 40 Then
            txt_Empty_Beam_Hsn.Focus()
        End If

        If e.KeyCode = 38 Then
            cbo_vehicleno.Focus()
        End If

    End Sub

    Private Sub txt_Gst_Tax_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Gst_Tax.KeyDown
        If e.KeyCode = 40 Then
            txt_remarks.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_Empty_Beam_Hsn.Focus()
        End If
    End Sub

    Private Sub txt_Gst_Tax_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Gst_Tax.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_remarks.Focus()
        End If
    End Sub

    Private Sub txt_Empty_Beam_Hsn_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Empty_Beam_Hsn.KeyPress


        If Asc(e.KeyChar) = 13 Then
            txt_Gst_Tax.Focus()
        End If

    End Sub

    Private Sub txt_Empty_Beam_Hsn_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Empty_Beam_Hsn.KeyDown
        If e.KeyCode = 40 Then
            txt_Gst_Tax.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_Purpose_Of_Delivery.Focus()
        End If

    End Sub



    Private Sub cbo_LoomType_Creation_TextChanged(sender As Object, e As EventArgs) Handles cbo_LoomType_Creation.TextChanged
        Try
            If cbo_LoomType_Creation.Visible Then
                With dgv_Details
                    If Val(cbo_LoomType_Creation.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_LoomType_Creation.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub dgtxt_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_Details.TextChanged
        With dgv_Details
            If .Rows.Count <> 0 Then
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
            End If
        End With
    End Sub


    Private Sub txt_Party_DcNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Party_DcNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_DeliveryAt.Focus()
        End If
    End Sub

    Private Sub txt_Party_DcNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Party_DcNo.KeyDown
        If e.KeyCode = 40 Then
            cbo_DeliveryAt.Focus()
        End If

        If e.KeyCode = 38 Then
            cbo_RecForm.Focus()
        End If
    End Sub

    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click
        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        Grp_EWB.Left = (Me.Width - pnl_back.Width) / 2 + 100
        Grp_EWB.Top = (Me.Height - pnl_back.Height) / 2 + 160
    End Sub

    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        '   Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Details

            If .CurrentCell.ColumnIndex = 6 Then
                If .CurrentCell.ColumnIndex = 0 Then
                    MessageBox.Show("Invalid Beam Rate", "DOES NOT GENERATE EWB...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    'If txt_Beam_Rate.Enabled And txt_Beam_Rate.Visible Then txt_Beam_Rate.Focus()
                    Exit Sub
                End If
            End If
        End With

        Dim da As New SqlClient.SqlDataAdapter("Select EwayBill_No from Empty_BeamBagCone_Delivery_Head where Empty_BeamBagCone_Delivery_Code = '" & NewCode & "'", con)
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


        CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
                         "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
                         "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
                         "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
                         "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
                         " " &
                         " " &
                         "  SELECT               'O'              , '4'             ,   'JOB WORK'              ,    'CHL'    , a.Empty_BeamBagCone_Delivery_No ,a.Empty_BeamBagCone_Delivery_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
                         " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo, L.Ledger_MainName, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee, TS.State_Code, (case when a.DeliveryTo_IdNo <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode," &
                         " 1                     , 0 , a.Amount     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
                         " a.Vehicle_No, 'R', '" & NewCode & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from Empty_BeamBagCone_Delivery_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L ON a.Ledger_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on ( CASE WHEN a.DeliveryTo_IdNo <> 0 THEN a.DeliveryTo_IdNo ELSE a.Ledger_IdNo  END ) = tDELV.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo " &
                          " where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
        CMD.ExecuteNonQuery()

        ' --------------

        Dim vCgst_Amt As String = 0
        Dim vSgst_Amt As String = 0
        Dim vIgst_AMt As String = 0
        Dim vTax_Perc As String = 0

        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter


        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        'da = New SqlClient.SqlDataAdapter(" Select  1, a.Empty_Beam_HSN_Code  ,a.Empty_Beam as Qty , a.GST_Percentage  , sum(b.Empty_Beam * b.Beam_width_Rate) As TaxableAmt , tz.Company_State_IdNo , Lh.Ledger_State_Idno  ,a.GST_Tax_Invoice_Status , b.LoomType_idno, b.beam_width_idno, b.Amount " &
        '                                  " from  Empty_BeamBagCone_Delivery_DETAILS a  INNER join Empty_BeamBagCone_Delivery_Details b ON b.Empty_BeamBagCone_Delivery_No = a.Empty_BeamBagCone_Delivery_No INNER Join Ledger_Head Lh ON Lh.Ledger_Idno =  a.Ledger_Idno  INNER JOIN Company_Head tz On tz.Company_Idno = a.Company_Idno  Where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
        '                                  " a.Empty_Beam_HSN_Code , a.Empty_Beam , a.GST_Percentage , tz.Company_State_IdNo , Lh.Ledger_State_Idno  ,a.GST_Tax_Invoice_Status,  ", con)

        da = New SqlClient.SqlDataAdapter("select a.Sl_No , b.Empty_Beam_HSN_Code  ,a.Empty_Beam as qty ,   B.GST_Percentage   , sum(a.Empty_Beam * a.Beam_Width_Rate) As TaxableAmt , tz.Company_State_IdNo , Lh.Ledger_State_Idno  , b.GST_Tax_Invoice_Status ,C.loomType_Name,d.Beam_Width_Name  " &
                                          " from Empty_BeamBagCone_Delivery_Details a inner join Empty_BeamBagCone_Delivery_Head b on a.empty_beambagcone_delivery_Code = b.empty_beambagcone_delivery_Code   " &
                                          " LEFT OUTER JOIN  LoomType_Head  C ON  A.LoomType_idno=C.LoomType_idno  " &
                                          " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = b.Ledger_Idno  INNER JOIN Company_Head tz On tz.Company_Idno = b.Company_Idno " &
                                          " LEFT OUTER JOIN  Beam_Width_Head  D ON  a.Beam_Width_IdNo=D.Beam_Width_IdNo   " &
                                          " Where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "' group by a.Sl_No , b.Empty_Beam_HSN_Code,a.Empty_Beam,B.GST_Percentage, tz.Company_State_IdNo , Lh.Ledger_State_Idno  , b.GST_Tax_Invoice_Status ,C.loomType_Name,d.Beam_Width_Name   ", con)
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

            CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]     ,	                                 [Product_Description]                       ,        	[HSNCode]           ,          	[Quantity]                ,     [QuantityUnit] ,      Tax_Perc           ,	     [CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]          ,    InvCode      ,             Cgst_Value          ,             Sgst_Value          ,           Igst_Value) " &
                              " values                 (" & dt1.Rows(I).Item(0).ToString & ",    'EMPTY BEAM'     ,     '" & dt1.Rows(I).Item(8) & "' + '' +'" & dt1.Rows(I).Item(9) & "' , '" & dt1.Rows(I).Item(1) & "', " & dt1.Rows(I).Item(2).ToString & ",         'NOS'      , " & Val(vTax_Perc) & "  ,          0              ,           0       ," & dt1.Rows(I).Item(4) & " ,'" & NewCode & "',   '" & Str(Val(vCgst_Amt)) & "' ,   '" & Str(Val(vSgst_Amt)) & "' , '" & Str(Val(vIgst_AMt)) & "')"

            CMD.ExecuteNonQuery()

        Next

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
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Empty_BeamBagCone_Delivery_Head", "EwayBill_No", "Empty_BeamBagCone_Delivery_Code", Pk_Condition)

    End Sub

    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GetAuthToken(rtbEWBResponse)
    End Sub

    Private Sub btn_CancelEWB_1_Click(sender As Object, e As EventArgs) Handles btn_CancelEWB_1.Click

        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_EWBNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Empty_BeamBagCone_Delivery_Head", "EwayBill_No", "Empty_BeamBagCone_Delivery_Code")
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

    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 0)
    End Sub

    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 1)
    End Sub

    Private Sub btn_Close_EWB_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub

    Private Sub cbo_DeliveryAt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_DeliveryAt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryAt, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_DeliveryAt_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_DeliveryAt.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryAt.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_DeliveryAt_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_DeliveryAt.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryAt, txt_Party_DcNo, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DeliveryAt_GotFocus(sender As Object, e As EventArgs) Handles cbo_DeliveryAt.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Transport.KeyUp
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

    Private Sub cbo_Transport_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Transport.KeyPress
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_vehicleno, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        'Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_Transport_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_DeliveryAt, cbo_vehicleno, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        'Get_vehicle_from_Transport()
    End Sub


    Private Sub Printing_Format2_1464(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim I As Integer, NoofItems_PerPage As Integer
        Dim vGST_BILL As Integer = 0
        Dim Sno As Integer
        Dim BMNos1 As String, BMNos2 As String, BMNos3 As String, BMNos4 As String
        Dim EntryCode As String
        Dim NoofDets As Integer
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0





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

        ' End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 65
            .Right = 50 '40 '30
            .Top = 326 '40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        pFont = New Font("Calibri", 11, FontStyle.Regular)
        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 14 '15 '16.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(18) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(18) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ClArr(1) = Val(45) : ClArr(2) = 120 : ClArr(3) = 75 : ClArr(4) = 45 : ClArr(5) = 55 : ClArr(6) = 110 : ClArr(7) = 180 : ClArr(8) = 50
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))






        'Printing_Format1_PageHeader(e, EntryCode, TxtHgt, strHeight, pFont, p1Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PageHeight, PrintWidth, PrintHeight, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)



        Dim vS1 As Single = 0
        Dim vS2 As Single = 0
        Dim vS3 As Single = 0
        Dim vS4 As Single = 0
        Dim vS5 As Single = 0
        Dim vS6 As Single = 0
        Dim vS7 As Single = 0
        Dim vS8 As Single = 0
        Dim vS9 As Single = 0
        Dim vS10 As Single = 0

        vS1 = ClArr(1)
        vS2 = ClArr(1) + ClArr(2)
        vS3 = ClArr(1) + ClArr(2) + ClArr(3)
        vS4 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4)
        vS5 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5)
        vS6 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6)
        vS7 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7)
        vS8 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8)
        vS9 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9)
        vS10 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10)

        'vPrnt_2Copy_In_SinglePage = Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage  '---------not completed

        'For PCnt = 1 To PrntCnt

        '    If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

        '        If PCnt = 1 Then
        '            prn_PageNo1 = prn_PageNo

        '            prn_DetIndx1 = prn_DetIndx
        '            prn_DetSNo1 = prn_DetSNo
        '            prn_NoofBmDets1 = prn_NoofBmDets
        '            TMargin = TMargin


        '        Else


        '            prn_PageNo = prn_PageNo1
        '            prn_NoofBmDets = prn_NoofBmDets1
        '            prn_DetIndx = prn_DetIndx1
        '            prn_DetSNo = prn_DetSNo1

        '            TMargin = 560 + TMargin  ' 600 + TMargin

        '        End If

        '    End If



        Try

            If prn_HdDt.Rows.Count > 0 Then
                vGST_BILL = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)
                'Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                Printing_Format2_PageHeader_1464(e, EntryCode, TxtHgt, strHeight, pFont, p1Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PageHeight, PrintWidth, PrintHeight, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                NoofItems_PerPage = 2

                If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) = "" Then
                    NoofItems_PerPage = NoofItems_PerPage + 1
                End If

                'If Val(Common_Procedures.settings.WeaverWagesYarnReceipt_Print_2Copy_In_SinglePage) = 1 Then
                '    If prn_DetDt.Rows.Count > NoofItems_PerPage Then
                '        NoofItems_PerPage = 35
                '    End If
                'End If

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + vS5, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1


                            Printing_Format2_PageFooter_1464(e, EntryCode, TxtHgt, strHeight, pFont, p1Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PageHeight, PrintWidth, PrintHeight, prn_PageNo, NoofItems_PerPage, NoofDets, False, CurY, LnAr, ClArr)



                            e.HasMorePages = True
                            Return

                        End If


                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Beam_Nos").ToString) <> "" Then
                            BMNos1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Beam_Nos").ToString)
                            'BMNos1 = "BEAM No.s  : " & Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString)
                        End If

                        If Len(BMNos1) > 30 Then
                            For I = 30 To 1 Step -1
                                If Mid$(Trim(BMNos1), I, 1) = " " Or Mid$(Trim(BMNos1), I, 1) = "," Or Mid$(Trim(BMNos1), I, 1) = "." Or Mid$(Trim(BMNos1), I, 1) = "-" Or Mid$(Trim(BMNos1), I, 1) = "/" Or Mid$(Trim(BMNos1), I, 1) = "_" Or Mid$(Trim(BMNos1), I, 1) = "(" Or Mid$(Trim(BMNos1), I, 1) = ")" Or Mid$(Trim(BMNos1), I, 1) = "\" Or Mid$(Trim(BMNos1), I, 1) = "[" Or Mid$(Trim(BMNos1), I, 1) = "]" Or Mid$(Trim(BMNos1), I, 1) = "{" Or Mid$(Trim(BMNos1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 30
                            BMNos2 = Microsoft.VisualBasic.Right(Trim(BMNos1), Len(BMNos1) - I)
                            BMNos1 = Microsoft.VisualBasic.Left(Trim(BMNos1), I)
                        End If

                        If Len(BMNos2) > 30 Then
                            For I = 30 To 1 Step -1
                                If Mid$(Trim(BMNos2), I, 1) = " " Or Mid$(Trim(BMNos2), I, 1) = "," Or Mid$(Trim(BMNos2), I, 1) = "." Or Mid$(Trim(BMNos2), I, 1) = "-" Or Mid$(Trim(BMNos2), I, 1) = "/" Or Mid$(Trim(BMNos2), I, 1) = "_" Or Mid$(Trim(BMNos2), I, 1) = "(" Or Mid$(Trim(BMNos2), I, 1) = ")" Or Mid$(Trim(BMNos2), I, 1) = "\" Or Mid$(Trim(BMNos2), I, 1) = "[" Or Mid$(Trim(BMNos2), I, 1) = "]" Or Mid$(Trim(BMNos2), I, 1) = "{" Or Mid$(Trim(BMNos2), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 30
                            BMNos3 = Microsoft.VisualBasic.Right(Trim(BMNos2), Len(BMNos2) - I)
                            BMNos2 = Microsoft.VisualBasic.Left(Trim(BMNos2), I)
                        End If

                        If Len(BMNos3) > 30 Then
                            For I = 30 To 1 Step -1
                                If Mid$(Trim(BMNos3), I, 1) = " " Or Mid$(Trim(BMNos3), I, 1) = "," Or Mid$(Trim(BMNos3), I, 1) = "." Or Mid$(Trim(BMNos3), I, 1) = "-" Or Mid$(Trim(BMNos3), I, 1) = "/" Or Mid$(Trim(BMNos3), I, 1) = "_" Or Mid$(Trim(BMNos3), I, 1) = "(" Or Mid$(Trim(BMNos3), I, 1) = ")" Or Mid$(Trim(BMNos3), I, 1) = "\" Or Mid$(Trim(BMNos3), I, 1) = "[" Or Mid$(Trim(BMNos3), I, 1) = "]" Or Mid$(Trim(BMNos3), I, 1) = "{" Or Mid$(Trim(BMNos3), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 30
                            BMNos4 = Microsoft.VisualBasic.Right(Trim(BMNos3), Len(BMNos3) - I)
                            BMNos3 = Microsoft.VisualBasic.Left(Trim(BMNos3), I)
                        End If




                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Empty_Beam").ToString) <> 0 Then


                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("sl_no").ToString, LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, "Empty Beam", LMargin + vS1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Beam_HSN_Code").ToString, LMargin + vS2, CurY, 0, 0, pFont)

                            'If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS3, CurY, 0, 0, pFont)
                            '    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Empty_Beam").ToString, LMargin + vS4 - 10, CurY, 2, 0, pFont)


                            'Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Empty_Beam").ToString, LMargin + vS3 - 10, CurY, 2, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Empty_Beam").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                            'End If
                            Common_Procedures.Print_To_PrintDocument(e, "Nos", LMargin + vS4, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Beam_type").ToString, LMargin + vS6, CurY, 0, 0, pFont)

                            ''Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Beam_Width_Name").ToString, LMargin + vS3 - 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("LoomType_Name").ToString, LMargin + vS5, CurY, 0, 0, pFont)

                            p1Font = New Font("Calibri", 9, FontStyle.Regular)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos1), LMargin + vS6, CurY, 0, 0, p1Font)

                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Beam_Width_Rate").ToString), LMargin + vS7, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), LMargin + vS9 - 10, CurY, 1, 0, pFont)


                            If Trim(BMNos2) <> "" Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos2), LMargin + vS6, CurY, 0, 0, p1Font)
                            End If

                            If Trim(BMNos3) <> "" Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos3), LMargin + vS6, CurY, 0, 0, p1Font)
                            End If

                            If Trim(BMNos4) <> "" Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos4), LMargin + vS6, CurY, 0, 0, p1Font)
                            End If

                        End If

                        prn_DetIndx = prn_DetIndx + 1

                        NoofDets = NoofDets + 1
                    Loop
                End If



                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Or Trim(BMNos2) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Then
                        Sno = Sno + 1

                        Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "Empty Bags", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bag_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)

                        If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3), CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bags").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bags").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bags").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "Nos", LMargin + vS2 + ClArr(5), CurY, 0, 0, pFont)



                    End If

                    'If Trim(BMNos2) <> "" Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos2), LMargin + vS5 + 5, CurY, 0, 0, pFont)
                    'End If
                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) <> 0 Or Trim(BMNos3) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) <> 0 Then
                        Sno = Sno + 1

                        Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "Empty Cones", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cone_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)

                        If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3), CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cones").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cones").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "Nos", LMargin + vS2 + ClArr(5), CurY, 0, 0, pFont)



                    End If
                    'If Trim(BMNos3) <> "" Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos3), LMargin + vS5 + 5, CurY, 0, 0, pFont)
                    'End If
                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bobin").ToString)) <> 0 Or Trim(BMNos4) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bobin").ToString)) <> 0 Then
                        Sno = Sno + 1

                        Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, "Empty Bobin", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cone_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)
                        If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3), CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bobin").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bobin").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "Nos", LMargin + vS2 + ClArr(5), CurY, 0, 0, pFont)


                    End If
                    'If Trim(BMNos4) <> "" Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos4), LMargin + vS5 + 5, CurY, 0, 0, pFont)
                    'End If
                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Jumbo").ToString)) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    Sno = Sno + 1

                    Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "Empty Jumbo", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cone_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)
                    If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3), CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Jumbo").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Jumbo").ToString, LMargin + vS2, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, "Nos", LMargin + vS2 + ClArr(5), CurY, 0, 0, pFont)

                End If



                'NoofDets = NoofDets + 1

                'If Trim(ItmNm2) <> "" Then
                '    CurY = CurY + TxtHgt - 5
                '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                '    NoofDets = NoofDets + 1
                'End If

                'prn_DetIndx = prn_DetIndx + 1





                'Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

                Printing_Format2_PageFooter_1464(e, EntryCode, TxtHgt, strHeight, pFont, p1Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PageHeight, PrintWidth, PrintHeight, prn_PageNo, NoofItems_PerPage, NoofDets, True, CurY, LnAr, ClArr)

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
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
        '    If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
        '        If prn_DetDt.Rows.Count > 6 Then
        '            PrntCnt2ndPageSTS = True
        '            e.HasMorePages = True
        '            Return
        '        End If
        '    End If
        'End If

        'PrntCnt2ndPageSTS = False

        'Next PCnt

LOOP2:



        'prn_Count = prn_Count + 1

        'If Val(prn_TotCopies) > 1 Then

        '    If prn_Count < Val(prn_TotCopies) Then

        '        prn_DetIndx = 0
        '        prn_DetSNo = 0
        '        prn_PageNo = 0

        '        e.HasMorePages = True
        '        Return

        '    Else
        '        e.HasMorePages = False
        '    End If

        'Else

        '    prn_HeadIndx = prn_HeadIndx + 1
        '    If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
        '        e.HasMorePages = True

        '    Else
        '        e.HasMorePages = False

        '    End If


        'prn_Count = prn_Count + 1


        '    e.HasMorePages = False

        '    If Val(prn_TotCopies) > 1 Then
        '        If prn_Count < Val(prn_TotCopies) Then

        '            prn_DetIndx = 0
        '            'prn_DetSNo = 0
        '            prn_PageNo = 0
        '            prn_DetIndx = 0
        '            prn_NoofBmDets = 0


        '            e.HasMorePages = True
        '            Return

        '        End If

        '    End If

        'prn_DetDt.Clear()
        'prn_PageNo = 0

        'prn_DetIndx = 0
        'prn_DetSNo = 0

        'End If

        '   e.HasMorePages = False
    End Sub

    Private Sub Printing_Format2_PageHeader_1464(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal strheight As Single, ByVal pFont As Font, ByVal p1Font As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal Pageheight As Single, ByVal PrintWidth As Single, ByVal Printheight As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single)

        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim I As Integer
        Dim vGST_BILL As Integer = 0
        Dim Sno As Integer
        Dim BMNos1 As String, BMNos2 As String, BMNos3 As String, BMNos4 As String
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""
        Dim Cmp_Gstin_No As String
        Dim S = 0

        ''PrintDocument pd = new PrintDocument();
        ''pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        ''pd.Print();

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        'ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        'Debug.Print(ps.PaperName)
        'If ps.Width = 800 And ps.Height = 600 Then
        'PrintDocument1.DefaultPageSettings.PaperSize = ps
        'e.PageSettings.PaperSize = ps
        'PpSzSTS = True
        'Exit For
        'End If
        'Next

        'If PpSzSTS = False Then
        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        'If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        'ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        'PrintDocument1.DefaultPageSettings.PaperSize = ps
        'e.PageSettings.PaperSize = ps
        'PpSzSTS = True
        'Exit For
        'End If
        'Next

        'If PpSzSTS = False Then
        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        'If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        'ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        'PrintDocument1.DefaultPageSettings.PaperSize = ps
        'e.PageSettings.PaperSize = ps
        'Exit For
        'End If
        'Next
        'End If

        'End If

        'With PrintDocument1.DefaultPageSettings.Margins
        '.Left = 20 ' 65
        '.Right = 30
        '.Top = 40
        '.Bottom = 50
        'LMargin = .Left
        'RMargin = .Right
        'TMargin = .Top
        'BMargin = .Bottom
        'End With

        'pFont = New Font("Calibri", 11, FontStyle.Regular)

        ''e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        'With PrintDocument1.DefaultPageSettings.PaperSize
        'PrintWidth = .Width - RMargin - LMargin
        'Printheight = .Height - TMargin - BMargin
        'PageWidth = .Width - RMargin
        'PageHeight = .Height - BMargin
        'End With

        'TxtHgt = 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        'Erase LnAr
        'Erase ClArr

        'LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        'ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        PageNo = PageNo + 1
        CurY = TMargin

        If PageNo <= 1 Then
            prn_Count = prn_Count + 1
        End If

        prn_OriDupTri = ""

        'If String.IsNullOrEmpty(prn_InpOpts) Then prn_InpOpts = "1"

        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)


                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If


        End If

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt - 3, 1, 0, pFont)
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Gstin_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.: " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_Gstin_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strheight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height





        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 5, CurY, 80, 80)

                        End If

                    End Using

                End If

            End If

        End If

        CurY = CurY + strheight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 15, FontStyle.Bold)

        vGST_BILL = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)
        Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Gstin_No, LMargin + 10, CurY + 5, 0, 0, pFont)

        CurY = CurY + TxtHgt - 5
        'Common_Procedures.Print_To_PrintDocument(e, "( Not For Sale )", LMargin + 30, CurY + 15, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SAC : 99882 ( Textile Manufacture )", LMargin, CurY + 15, 2, PrintWidth, pFont)
        strheight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strheight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = 450
        C2 = PageWidth - (LMargin + C1)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "DELIVERY AT : ", LMargin + C1 + 10, CurY, 0, 0, pFont)




        W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width

        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Deliveryto_LedgerName").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)



        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString)), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            'CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No. :" & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)



        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : ", LMargin + 15, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + ClArr(3), CurY, 0, 0, pFont)

        End If

        If Trim(prn_HdDt.Rows(0).Item("EwayBill_No").ToString) <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL NO", LMargin + C1 + 10, CurY + 5, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 55, CurY + 5, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EwayBill_No").ToString, LMargin + C1 + W1 + 65, CurY + 5, 0, 0, pFont)

        End If

        'If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
        '    'Common_Procedures.Print_To_PrintDocument(e, "GSTIN : ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        '    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + 10, CurY, 0, 0, pFont)
        '    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + ClArr(3), CurY, 0, 0, pFont)
        'End If



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, LnAr(2))

        CurY = CurY + TxtHgt - 5

        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClArr(1) + 10, CurY, 0, ClArr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN", LMargin + ClArr(1) + ClArr(2) + 10, CurY - 10, 0, ClArr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + ClArr(1) + ClArr(2) + 10, CurY + 5, 0, ClArr(3), pFont)
        'If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "GST", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY - 10, 0, ClArr(4), pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY + 5, 0, ClArr(4), pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 0, ClArr(5), pFont)
        'Else
        Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 0, ClArr(4), pFont)
        'End If

        Common_Procedures.Print_To_PrintDocument(e, "UOM", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 0, ClArr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM TYPE", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, ClArr(6), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY + 5, 0, ClArr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NOS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, ClArr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, 0, ClArr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), CurY, 0, ClArr(9), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    End Sub

    Private Sub Printing_Format2_PageFooter_1464(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal strheight As Single, ByVal pFont As Font, ByVal p1Font As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal Pageheight As Single, ByVal PrintWidth As Single, ByVal Printheight As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single)

        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim I As Integer
        Dim vGST_BILL As Integer = 0
        Dim Sno As Integer
        Dim BMNos1 As String, BMNos2 As String, BMNos3 As String, BMNos4 As String

        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0
        Dim vigst_amt As String = 0
        Dim vTxPerc As String = 0

        Dim vS1 As Single = 0
        Dim vS2 As Single = 0
        Dim vS3 As Single = 0
        Dim vS4 As Single = 0
        Dim vS5 As Single = 0
        Dim vS6 As Single = 0
        Dim vS7 As Single = 0
        Dim vS8 As Single = 0
        Dim vS9 As Single = 0
        Dim vS10 As Single = 0

        vS1 = ClArr(1)
        vS2 = ClArr(1) + ClArr(2)
        vS3 = ClArr(1) + ClArr(2) + ClArr(3)
        vS4 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4)
        vS5 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5)
        vS6 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6)
        vS7 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7)
        vS8 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8)
        vS9 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9)



        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        vGST_BILL = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)

        If is_LastPage = True Then

            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + vS1, CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Beam").ToString, LMargin + vS3 - 10, CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "############0.00"), LMargin + vS9 - 10, CurY, 1, 0, pFont)

        End If



        CurY = CurY + TxtHgt + 5



        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)




        e.Graphics.DrawLine(Pens.Black, LMargin + vS1, LnAr(3), LMargin + vS1, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + vS2 - 10, LnAr(3), LMargin + vS2 - 10, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + vS3 - 10, LnAr(3), LMargin + vS3 - 10, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + vS4 - 10, LnAr(3), LMargin + vS4 - 10, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + vS5 - 10, LnAr(3), LMargin + vS5 - 10, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + vS6 - 10, LnAr(3), LMargin + vS6 - 10, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + vS7 - 10, LnAr(3), LMargin + vS7 - 10, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + vS8 - 10, LnAr(3), LMargin + vS8 - 10, CurY)

        'e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 100, LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 100, CurY)


        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        If Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) <> 0 And Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

            '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1158" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Value            :  ", LMargin + C1 + 50, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + C1 + 160, CurY, 0, 0, pFont)
            '    Else
            '        Common_Procedures.Print_To_PrintDocument(e, "Taxable Value    :  ", LMargin + C1 + 50, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + C1 + 160, CurY, 0, 0, pFont)
            '    End If
            'Else
            'Common_Procedures.Print_To_PrintDocument(e, "Total Amount :  ", LMargin + C1 + 50, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "############0.00"), LMargin + C1 + 165, CurY, 0, 0, p1Font)
        End If


        'Common_Procedures.Print_To_PrintDocument(e, "Transport Name : ", LMargin + 10, CurY, 0, 0, pFont)

        'If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TransportName").ToString), LMargin + ClArr(1) + ClArr(3) + 15, CurY, 0, 0, pFont)
        'End If



        'If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO : ", LMargin + 10, CurY, 0, 0, p1Font)
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_name").ToString), LMargin + ClArr(1) + 72, CurY, 0, 0, p1Font)
        'End If
        '-------------------------*********************

        'CurY = CurY + TxtHgt + 5
        vTxPerc = 0
        vCgst_amt = 0
        vSgst_amt = 0
        vigst_amt = 0
        If Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

            If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then
                'vTxPerc = Format(Val(prn_DetDt1.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")
                vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) * 2.5 / 100, "############0.00")
                vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) * 2.5 / 100, "############0.00")

                Common_Procedures.Print_To_PrintDocument(e, " CGST 2.5 % : " & vCgst_amt, LMargin + C1 + 47, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " SGST 2.5 % : " & vSgst_amt, LMargin + C1 + 190, CurY, 0, 0, pFont)

            Else

                'vTxPerc = prn_HdDt.Rows(0).Item("item_gst_percentage").ToString
                vigst_amt = Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) * 5 / 100, "############0.00")

                Common_Procedures.Print_To_PrintDocument(e, " IGST % : " & vigst_amt, LMargin + C1 + 47, CurY, 0, 0, pFont)

            End If
        End If

        'If Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) & "  " & Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString), LMargin + 10, CurY - 3, 0, 0, pFont)
        'End If

        CurY = CurY + TxtHgt
        '-------------------------*********************
        If Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) <> 0 And Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

            vTxamt = Val(vCgst_amt) + Val(vSgst_amt) + Val(vigst_amt)
            vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) + vTxamt, "###########0.00")

            Common_Procedures.Print_To_PrintDocument(e, "Value of Goods : " & vNtAMt, LMargin + C1 + ClArr(1) + ClArr(2) - 10, CurY + 5, 0, 0, p1Font)

        End If


        'If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
        '    'CurY = CurY + TxtHgt + 5
        '    Common_Procedures.Print_To_PrintDocument(e, "Through Vehicle No. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
        'End If
        '-------------------------*********************

        'If Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Del_Address4").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) & "  " & Trim(prn_HdDt.Rows(0).Item("Del_Address4").ToString), LMargin + 10, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt + 5
        'If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN  : " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString), LMargin + 10, CurY, 0, 0, pFont)
        'End If


        'If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
        '    CurY = CurY + TxtHgt + 5
        '    Common_Procedures.Print_To_PrintDocument(e, "Through Vehicle No. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)
        'End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "REMARKS  : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        End If
        CurY = CurY + TxtHgt - 5

        If Trim(prn_HdDt.Rows(0).Item("Purpose_of_delv").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "NOTE : " & Trim(prn_HdDt.Rows(0).Item("Purpose_of_delv").ToString), LMargin + 10, CurY, 0, 0, pFont)
        End If


        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        End If

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 325, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 20, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        If Val(prn_PageNo) > 1 Or is_LastPage = False Then
            CurY = CurY + 5
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Page No. : " & prn_PageNo, LMargin, CurY, 2, PageWidth, p1Font)
        End If

    End Sub
End Class