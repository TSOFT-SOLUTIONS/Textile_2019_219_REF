Imports System.IO
Public Class Empty_BeamBagCone_Delivery_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "EBDLV-"
    Private Pk_Condition1 As String = "EBFRT"
    Private prn_HdDt As New DataTable
    Private Prec_ActCtrl As New Control
    Private prn_PageNo As Integer
    Private vcbo_KeyDwnVal As Double
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo1 As Integer
    Private prn_DetIndx1 As Integer
    Private prn_NoofBmDets1 As Integer
    Private prn_DetSNo1 As Integer
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private prn_DetIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private Prnt_HalfSheet_STS As Boolean = False
    Private vPrnt_2Copy_In_SinglePage As Integer = 0
    Private Print_PDF_Status As Boolean = False
    Private Mov_Status As Boolean = False
    Private NoCalc_Status As Boolean = False
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
        Print_PDF_Status = False
        Mov_Status = False
        NoCalc_Status = True
        Grp_EWB.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1
        lbl_dcno.Text = ""
        lbl_dcno.ForeColor = Color.Black
        msk_date.Text = ""
        dtp_date.Text = ""
        cbo_Partyname_DelvTo.Text = ""
        cbo_RecForm.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
        cbo_vehicleno.Text = ""
        cbo_DeliveryAt.Text = ""
        txt_emptycones.Text = ""
        txt_remarks.Text = ""
        txt_BeamNos.Text = ""
        txt_emptybags.Text = ""
        txt_emptybeam.Text = ""
        txt_JumpoEmpty.Text = ""
        txt_EmptyBobin_Party.Text = ""
        txt_emptyBobin.Text = ""
        txt_Party_DcNo.Text = ""
        cbo_beamwidth.Text = ""
        txt_Book_No.Text = ""
        txt_Transport_Freight.Text = ""
        cbo_EndsCount.Text = ""
        cbo_Transport.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        txt_Amount.Text = ""
        txt_Beam_Rate.Text = ""
        txt_EWBNo.Text = ""
        lbl_dcno.Enabled = True
        dtp_date.Enabled = True
        msk_date.Enabled = True
        cbo_Partyname_DelvTo.Enabled = True
        cbo_RecForm.Enabled = True
        cbo_DeliveryAt.Enabled = True
        txt_Book_No.Enabled = True
        txt_emptybags.Enabled = True
        cbo_beamwidth.Enabled = True
        txt_emptycones.Enabled = True
        cbo_vehicleno.Enabled = True
        txt_remarks.Enabled = True
        txt_BeamNos.Enabled = True
        txt_emptybeam.Enabled = True
        txt_emptyBobin.Enabled = True
        txt_JumpoEmpty.Enabled = True
        txt_EmptyBobin_Party.Enabled = True
        txt_Party_DcNo.Enabled = True
        cbo_Transport.Enabled = True
        txt_Transport_Freight.Enabled = True
        cbo_EndsCount.Enabled = True
        lbl_UserName.Enabled = True
        lbl_Receipt_Code.Enabled = True

        cbo_LoomType_Creation.Text = ""

        txt_DcPrefixNo.Text = ""
        cbo_DcSufixNo.Text = "" ' "/" & Common_Procedures.FnYearCode

        If cbo_jobcardno.Visible Then cbo_jobcardno.Text = ""


        chk_GSTTax_Invocie.Checked = True
        txt_Empty_Beam_Hsn.Text = "730890"
        txt_Empty_Bag_Hsn.Text = ""
        txt_Empty_Cone_Hsn.Text = ""
        txt_Gst_Tax.Text = ""
        chk_Ewb_No_Sts.Checked = False


        Mov_Status = False
        NoCalc_Status = False

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String

        If Val(no) = 0 Then Exit Sub

        clear()

        Mov_Status = True


        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name ,c.Ledger_Name as RecName,d.Ledger_Name as DeliveryName from Empty_BeamBagCone_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.ReceivedFrom_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.DeliveryTo_IdNo = d.Ledger_IdNo where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                cbo_DcSufixNo.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_SuffixNo").ToString
                txt_DcPrefixNo.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_PrefixNo").ToString
                lbl_dcno.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_RefNo").ToString
                'lbl_dcno.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString

                dtp_date.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString
                msk_date.Text = dtp_date.Text
                cbo_Partyname_DelvTo.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_RecForm.Text = dt1.Rows(0).Item("RecName").ToString
                cbo_DeliveryAt.Text = dt1.Rows(0).Item("DeliveryName").ToString
                txt_Book_No.Text = dt1.Rows(0).Item("Book_No").ToString
                txt_emptybags.Text = dt1.Rows(0).Item("Empty_Bags").ToString
                cbo_beamwidth.Text = Common_Procedures.BeamWidth_IdNoToName(con, Val(dt1.Rows(0).Item("Beam_Width_IdNo").ToString))
                txt_emptycones.Text = dt1.Rows(0).Item("Empty_Cones").ToString
                cbo_vehicleno.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                txt_BeamNos.Text = dt1.Rows(0).Item("Beam_Nos").ToString
                txt_emptybeam.Text = dt1.Rows(0).Item("Empty_Beam").ToString
                txt_emptyBobin.Text = dt1.Rows(0).Item("Empty_Bobin").ToString
                txt_JumpoEmpty.Text = dt1.Rows(0).Item("Empty_Jumbo").ToString
                txt_EmptyBobin_Party.Text = dt1.Rows(0).Item("EmptyBobin_Party").ToString
                txt_Party_DcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_Transport_Freight.Text = Format(Val(dt1.Rows(0).Item("Transport_Freight").ToString), "#########0.00")
                cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, Val(dt1.Rows(0).Item("EndsCount_IdNo").ToString))
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                lbl_Receipt_Code.Text = dt1.Rows(0).Item("Receipt_Code").ToString
                txt_Amount.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "#########0.00")
                cbo_LoomType_Creation.Text = Common_Procedures.LoomType_IdNoToName(con, Val(dt1.Rows(0).Item("LoomType_Idno").ToString))
                cbo_jobcardno.Text = dt1.Rows(0).Item("Sizing_JobCode_forSelection").ToString

                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False
                txt_Empty_Beam_Hsn.Text = dt1.Rows(0).Item("Empty_Beam_HSN_Code").ToString
                txt_Empty_Bag_Hsn.Text = dt1.Rows(0).Item("Empty_Bag_HSN_Code").ToString
                txt_Empty_Cone_Hsn.Text = dt1.Rows(0).Item("Empty_Cone_HSN_Code").ToString
                txt_Gst_Tax.Text = dt1.Rows(0).Item("GST_Percentage").ToString
                txt_Beam_Rate.Text = dt1.Rows(0).Item("Beam_Rate").ToString
                txt_EWBNo.Text = dt1.Rows(0).Item("EwayBill_No").ToString
                If Trim(txt_EWBNo.Text) <> "" Then
                    chk_Ewb_No_Sts.Checked = True
                Else
                    chk_Ewb_No_Sts.Checked = False
                End If

            End If


            da2 = New SqlClient.SqlDataAdapter("Select a.Empty_Beam from Empty_Beam_Selection_Processing_Details a where  a.Reference_Code<>'" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_Code='" & Trim(Pk_Condition) & Trim(NewCode) & "' AND a.Empty_Beam < 0", con)
            dt2 = New DataTable
            da2.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                lbl_dcno.Enabled = False

                dtp_date.Enabled = False
                msk_date.Enabled = False
                cbo_Partyname_DelvTo.Enabled = False
                cbo_RecForm.Enabled = False
                cbo_DeliveryAt.Enabled = False
                txt_Book_No.Enabled = False
                txt_emptybags.Enabled = False
                cbo_beamwidth.Enabled = False
                txt_emptycones.Enabled = False
                cbo_vehicleno.Enabled = False
                txt_remarks.Enabled = False
                txt_BeamNos.Enabled = False
                txt_emptybeam.Enabled = False
                txt_emptyBobin.Enabled = False
                txt_JumpoEmpty.Enabled = False
                txt_EmptyBobin_Party.Enabled = False
                txt_Party_DcNo.Enabled = False
                cbo_Transport.Enabled = False
                txt_Transport_Freight.Enabled = False
                cbo_EndsCount.Enabled = False
                lbl_UserName.Enabled = False
                lbl_Receipt_Code.Enabled = False




            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        Mov_Status = False
        NoCalc_Status = False

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Partyname_DelvTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Partyname_DelvTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RecForm.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_RecForm.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryAt.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryAt.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_beamwidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_beamwidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
                    'MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Me.Close()
                    Exit Sub


                End If

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

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                If Prec_ActCtrl.Name = txt_Empty_Beam_Hsn.Name Or Prec_ActCtrl.Name = txt_Empty_Bag_Hsn.Name Or Prec_ActCtrl.Name = txt_Empty_Cone_Hsn.Name Or Prec_ActCtrl.Name = txt_Gst_Tax.Name Then
                    Prec_ActCtrl.BackColor = Color.AntiqueWhite
                    Prec_ActCtrl.ForeColor = Color.Black
                Else
                    Prec_ActCtrl.BackColor = Color.White
                    Prec_ActCtrl.ForeColor = Color.Black
                End If
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

    Private Sub Empty_BeamBagCone_Delivery_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable

        Me.Text = ""

        con.Open()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)
            Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER') and Close_status = 0 order by Ledger_DisplayName", con)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
        Else
            Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER')  and Close_status = 0 order by Ledger_DisplayName", con)
        End If
        Da.Fill(Dt1)
        cbo_Partyname_DelvTo.DataSource = Dt1
        cbo_Partyname_DelvTo.DisplayMember = "Ledger_DisplayName"

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
        Else
            Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER') and Close_status = 0 order by Ledger_DisplayName", con)
        End If
        Da.Fill(Dt3)
        cbo_RecForm.DataSource = Dt3
        cbo_RecForm.DisplayMember = "Ledger_DisplayName"

        Da = New SqlClient.SqlDataAdapter("select Distinct(vehicle_No) from Empty_BeamBagCone_Delivery_Head order by Vehicle_No", con)
        Da.Fill(Dt2)
        cbo_vehicleno.DataSource = Dt2
        cbo_vehicleno.DisplayMember = "Vehicle_No"

        lbl_EmptyBobinOwn.Visible = False
        lbl_EmptyBobinParty.Visible = False
        lbl_EmptyJumpo.Visible = False
        txt_emptyBobin.Visible = False
        txt_EmptyBobin_Party.Visible = False
        txt_JumpoEmpty.Visible = False
        cbo_Transport.Visible = True
        lbl_Transport.Visible = True
        txt_Transport_Freight.Visible = True
        lbl_Freight.Visible = True

        lbl_jobcardno.Visible = False
        cbo_jobcardno.Visible = False
        'cbo_LoomType_Creation.Visible = False

        If Common_Procedures.settings.Bobin_Zari_Kuri_Entries_Status = 1 Then
            lbl_EmptyBobinOwn.Visible = True
            lbl_EmptyBobinParty.Visible = True
            lbl_EmptyJumpo.Visible = True
            txt_emptyBobin.Visible = True
            txt_EmptyBobin_Party.Visible = True
            txt_JumpoEmpty.Visible = True
            cbo_Transport.Visible = False
            lbl_Transport.Visible = False
            txt_Transport_Freight.Visible = False
            lbl_Freight.Visible = False
        End If

        cbo_EndsCount.Visible = False
        lbl_Ends.Visible = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.k Textiles 
            cbo_EndsCount.Visible = True
            lbl_Ends.Visible = True
        End If

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        If Common_Procedures.settings.Show_Sizing_JobCard_Entry_Status = 1 Then

            lbl_jobcardno.Visible = True
            cbo_jobcardno.Visible = True
            cbo_jobcardno.BackColor = Color.White
            txt_BeamNos.Width = txt_emptycones.Width

            lbl_beam_type.Visible = True
            cbo_LoomType_Creation.Visible = True
            cbo_LoomType_Creation.BackColor = Color.White
            cbo_beamwidth.Width = txt_emptycones.Width

        Else

            lbl_jobcardno.Visible = False
            cbo_jobcardno.Visible = False
            'lbl_beam_type.Visible = False
            'cbo_LoomType_Creation.Visible = False

        End If



        cbo_DcSufixNo.Items.Clear()
        cbo_DcSufixNo.Items.Add("")
        cbo_DcSufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_DcSufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))
        cbo_DcSufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate))
        cbo_DcSufixNo.Items.Add("/" & Trim(Year(Common_Procedures.Company_FromDate)) & "-" & Trim(Microsoft.VisualBasic.Right(Year(Common_Procedures.Company_ToDate), 2)))

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        AddHandler cbo_DeliveryAt.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Partyname_DelvTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RecForm.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_beamwidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vehicleno.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptybags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptybeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptycones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptyBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_JumpoEmpty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBobin_Party.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Party_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNos.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Book_No.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Transport_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomType_Creation.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_jobcardno.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Beam_Hsn.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Bag_Hsn.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Cone_Hsn.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Gst_Tax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Beam_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcPrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DcSufixNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_beamwidth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryAt.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Partyname_DelvTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RecForm.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vehicleno.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptybags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptyBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptybeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptycones.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_JumpoEmpty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBobin_Party.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Party_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamNos.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Book_No.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Transport_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomType_Creation.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_jobcardno.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Beam_Hsn.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Bag_Hsn.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Cone_Hsn.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Gst_Tax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Beam_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcPrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DcSufixNo.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_emptybags.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_emptybeam.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_emptycones.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_JumpoEmpty.KeyDown, AddressOf TextBoxControlKeyDown
        '   AddHandler txt_EmptyBobin_Party.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Party_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_emptyBobin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Book_No.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_BeamNos.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Transport_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_LoomType_Creation.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_emptybags.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_emptybeam.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_emptycones.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_JumpoEmpty.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_EmptyBobin_Party.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Party_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_emptyBobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Book_No.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_BeamNos.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Transport_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler cbo_LoomType_Creation.KeyPress, AddressOf TextBoxControlKeyPress


        If cbo_EndsCount.Visible = False Then

            txt_Amount.Width = txt_remarks.Width
        End If


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
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
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim da2 As SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_dcno.Text)


        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, New_Entry, Me, con, "Empty_BeamBagCone_Delivery_Head", "Empty_BeamBagCone_Delivery_Code", NewCode, "Empty_BeamBagCone_Delivery_Date", "(Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If



        da2 = New SqlClient.SqlDataAdapter("Select a.Empty_Beam from Empty_Beam_Selection_Processing_Details a where  a.Reference_Code<>'" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_Code='" & Trim(Pk_Condition) & Trim(NewCode) & "' AND a.Empty_Beam < 0", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            MessageBox.Show("Already Receipt Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Empty_BeamBagCone_Delivery_head", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Empty_BeamBagCone_Delivery_Code, Company_IdNo, for_OrderBy", tr)

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), tr)



            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Empty_Beam_Selection_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()
        'If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()
    End Sub
    Public Sub Get_vehicle_from_Transport()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1186" Then '---- UNITED WEAVES (PALLADAM)
            Exit Sub
        End If


        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim transport_id As Integer
        transport_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_Transport.Text)
        Da = New SqlClient.SqlDataAdapter("select vehicle_no from ledger_head where ledger_idno=" & Str(Val(transport_id)) & "", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count <> 0 Then
            cbo_vehicleno.Text = Dt.Rows(0).Item("vehicle_no").ToString


        End If
        Dt.Clear()
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
        Dim vCSInvCode As String = ""
        Dim vYSInvCode As String = ""
        Dim vYSMovNo As String = ""
        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_EmptyBeam_Return, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_EmptyBeam_Return, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
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

            vYSMovNo = ""
            If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then
                vYSInvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                da = New SqlClient.SqlDataAdapter("select JObwork_Empty_BeamBagCone_Delivery_RefNo from JObwork_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JObwork_Empty_BeamBagCone_Delivery_Code = '" & Trim(vYSInvCode) & "'", con)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        vYSMovNo = Trim(dt.Rows(0)(0).ToString)
                    End If
                End If

                dt.Clear()

            End If



            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(vYSMovNo) <> 0 Then


                MessageBox.Show("This DC No. is in Jobwork Empty Beam DC", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Dc.No", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_dcno.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try

            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Delivery_RefNo from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby, Empty_BeamBagCone_Delivery_RefNo"
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

            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Delivery_RefNo from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby desc, Empty_BeamBagCone_Delivery_RefNo desc"
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Delivery_RefNo from Empty_BeamBagCone_Delivery_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby, Empty_BeamBagCone_Delivery_RefNo"
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Delivery_RefNo from Empty_BeamBagCone_Delivery_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby desc,Empty_BeamBagCone_Delivery_RefNo desc"
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then
                lbl_dcno.Text = Common_Procedures.get_Beam_Delivery_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            Else

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
            End If

            lbl_dcno.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Empty_BeamBagCone_Delivery_RefNo desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then

                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False

                If IsDBNull(dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_PrefixNo").ToString) = False Then
                    If dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_PrefixNo").ToString <> "" Then txt_DcPrefixNo.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_PrefixNo").ToString
                End If
                If IsDBNull(dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_SuffixNo").ToString) = False Then
                    If dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_SuffixNo").ToString <> "" Then cbo_DcSufixNo.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_SuffixNo").ToString
                End If

                If Trim(dt1.Rows(0).Item("Empty_Beam_HSN_Code").ToString) <> "" Then txt_Empty_Beam_Hsn.Text = Trim(dt1.Rows(0).Item("Empty_Beam_HSN_Code").ToString)

                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString
                End If

            End If
            dt1.Clear()


            If msk_date.Enabled And msk_date.Visible Then
                msk_date.Focus()
                msk_date.SelectionStart = 0
            End If
            'If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        dt1.Dispose()
        da.Dispose()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String
        Dim InvCode As String = ""
        Dim vYSInvCode As String = ""
        Dim vYSMovNo As String = ""
        Dim vOSmovCode As String = ""
        Dim vOSmovNo As String = ""
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
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

            vYSMovNo = ""
            If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then
                vYSInvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                da = New SqlClient.SqlDataAdapter("select JObwork_Empty_BeamBagCone_Delivery_RefNo from JObwork_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JObwork_Empty_BeamBagCone_Delivery_Code = '" & Trim(vYSInvCode) & "'", con)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        vYSMovNo = Trim(dt.Rows(0)(0).ToString)
                    End If
                End If

                dt.Clear()

            End If



            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(vYSMovNo) <> 0 Then


                MessageBox.Show("This DC No. is in Jobwork Empty Beam DC", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


            Else
                MessageBox.Show("Dc.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim Bw_ID As Integer = 0
        Dim led_id As Integer = 0
        Dim Rec_id As Integer = 0
        Dim Partcls As String, PBlNo As String, EntID As String
        Dim Trans_ID As Integer
        Dim ECnt_ID As Integer = 0
        Dim vOrdByNo As String = ""
        Dim DelAt_id As Integer = 0
        Dim vCOMP_LEDIDNO As Integer = 0
        Dim vDELVLED_COMPIDNO As Integer = 0
        Dim vSELC_RCVDIDNO As Integer
        Dim vREC_Ledtype As String = ""

        Dim vLoomType_Idno As Integer = 0
        Dim vGST_Tax_Inv_Sts As Integer = 0
        Dim vDCNo = ""


        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, New_Entry, Me, con, "Empty_BeamBagCone_Delivery_Head", "Empty_BeamBagCone_Delivery_Code", NewCode, "Empty_BeamBagCone_Delivery_Date", "(Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Empty_BeamBagCone_Delivery_RefNo desc", dtp_date.Value.Date) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled Then msk_date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Partyname_DelvTo.Text)
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        EntID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)

        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Partyname_DelvTo.Enabled Then cbo_Partyname_DelvTo.Focus()
            Exit Sub
        End If
        Bw_ID = Common_Procedures.BeamWidth_NameToIdNo(con, cbo_beamwidth.Text)
        Rec_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)
        DelAt_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryAt.Text)


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
        If Rec_id = 0 Then Rec_id = 4

        If led_id = Rec_id Then
            MessageBox.Show("Invalid DeliveryTo Name" & Chr(13) & "Both DeliveryTo and ReceivedFrom should not be same", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Partyname_DelvTo.Enabled Then cbo_Partyname_DelvTo.Focus()
            Exit Sub
        End If



        If Trim(cbo_vehicleno.Text) <> "" Then
            cbo_vehicleno.Text = Common_Procedures.Vehicle_Number_Remove_Unwanted_Spaces(Trim(cbo_vehicleno.Text))
        End If

        vGST_Tax_Inv_Sts = 0
        If chk_GSTTax_Invocie.Checked = True Then vGST_Tax_Inv_Sts = 1

        If Val(txt_Transport_Freight.Text) <> 0 Then
            If Trim(cbo_Transport.Text) = "" Then
                MessageBox.Show("Invalid Transport Name ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Transport.Enabled Then cbo_Transport.Focus()
                Exit Sub
            End If
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else
                If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then
                    lbl_dcno.Text = Common_Procedures.get_Beam_Delivery_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
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
                End If
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            vDCNo = Trim(txt_DcPrefixNo.Text) & Trim(lbl_dcno.Text) & Trim(cbo_DcSufixNo.Text)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(msk_date.Text))

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_dcno.Text)

            dt1.Clear()

            vLoomType_Idno = Common_Procedures.LoomType_NameToIdNo(con, Trim(cbo_LoomType_Creation.Text), tr)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Empty_BeamBagCone_Delivery_Head ( Empty_BeamBagCone_Delivery_Code, Company_IdNo                     ,     Empty_BeamBagCone_Delivery_No  ,        Empty_BeamBagCone_Delivery_RefNo     ,           Empty_BeamBagCone_Delivery_PrefixNo                ,              Empty_BeamBagCone_Delivery_SuffixNo    ,          for_OrderBy       ,     Empty_BeamBagCone_Delivery_Date, Ledger_IdNo        , Book_No                         , Empty_Beam                     , Empty_Bags                     , Beam_Width_IdNo   , Empty_Cones                     , Empty_Bobin                      , EmptyBobin_Party                       , Empty_Jumbo                      , Vehicle_No                        , Remarks                          , ReceivedFrom_IdNo  ,DeliveryTo_IdNo   , Party_DcNo                          , Transport_IdNo       , Transport_Freight                       , EndsCount_IdNo    ,             User_idNo                   ,                         Beam_Nos      ,           Amount             ,               LoomType_idno      , Sizing_JobCode_forSelection        ,             Empty_Beam_HSN_Code             ,                Empty_Bag_HSN_Code           ,                Empty_Cone_HSN_Code             ,         GST_Tax_Invoice_Status       ,          GST_Percentage        ,                 Beam_Rate   , EwayBill_No  ) " &
                    "Values                                                        ('" & Trim(NewCode) & "'         , " & Str(Val(lbl_Company.Tag)) & ",     '" & Trim(vDCNo) & "'      ,           '" & Trim(lbl_dcno.Text) & "'     , '" & Trim(UCase(txt_DcPrefixNo.Text)) & "' ,'" & Trim(cbo_DcSufixNo.Text) & "', " & Str(Val(vOrdByNo)) & " ,     @DeliveryDate                  , " & Val(led_id) & ", '" & Trim(txt_Book_No.Text) & "', " & Val(txt_emptybeam.Text) & ", " & Val(txt_emptybags.Text) & ", " & Val(Bw_ID) & ", " & Val(txt_emptycones.Text) & ", " & Val(txt_emptyBobin.Text) & " , " & Val(txt_EmptyBobin_Party.Text) & " , " & Val(txt_JumpoEmpty.Text) & " , '" & Trim(cbo_vehicleno.Text) & "', '" & Trim(txt_remarks.Text) & "' , " & Val(Rec_id) & ", " & Val(DelAt_id) & " , '" & Trim(txt_Party_DcNo.Text) & "' , " & Val(Trans_ID) & ", " & Val(txt_Transport_Freight.Text) & " , " & Val(EntID) & ", " & Val(Common_Procedures.User.IdNo) & ", '" & Trim(txt_BeamNos.Text) & "' , " & Val(txt_Amount.Text) & " ,       " & Val(vLoomType_Idno) & ", '" & Trim(cbo_jobcardno.Text) & "' , '" & Trim(txt_Empty_Beam_Hsn.Text) & "', '" & Trim(txt_Empty_Bag_Hsn.Text) & "' ,   '" & Trim(txt_Empty_Cone_Hsn.Text) & "' ,   " & Str(Val(vGST_Tax_Inv_Sts)) & " ,  " & Val(txt_Gst_Tax.Text) & " ,  " & Val(txt_Beam_Rate.Text) & "  , '" & Trim(txt_EWBNo.Text) & "') "

                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Empty_BeamBagCone_Delivery_head", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Empty_BeamBagCone_Delivery_Code, Company_IdNo, for_OrderBy", tr)

                cmd.CommandText = "Update Empty_BeamBagCone_Delivery_Head set Empty_BeamBagCone_Delivery_Date = @DeliveryDate,Empty_BeamBagCone_Delivery_No =  '" & Trim(vDCNo) & "' ,Empty_BeamBagCone_Delivery_RefNo = '" & Trim(lbl_dcno.Text) & "' , Empty_BeamBagCone_Delivery_PrefixNo = '" & Trim(UCase(txt_DcPrefixNo.Text)) & "' , Empty_BeamBagCone_Delivery_SuffixNo = '" & Trim(cbo_DcSufixNo.Text) & "' ,Ledger_IdNo = " & Val(led_id) & ",Book_No ='" & Trim(txt_Book_No.Text) & "',  Empty_Beam = " & Val(txt_emptybeam.Text) & ", Empty_Bags = " & Val(txt_emptybags.Text) & ", Beam_Width_IdNo = " & Val(Bw_ID) & ", Empty_Cones=" & Val(txt_emptycones.Text) & " , Empty_Bobin =" & Val(txt_emptyBobin.Text) & " ,EndsCount_IdNo =" & Val(EntID) & " ,EmptyBobin_Party = " & Val(txt_EmptyBobin_Party.Text) & "  ,Empty_Jumbo = " & Val(txt_JumpoEmpty.Text) & "  ,Vehicle_No='" & Trim(cbo_vehicleno.Text) & "',Remarks='" & Trim(txt_remarks.Text) & "' , ReceivedFrom_IdNo = " & Val(Rec_id) & " , Party_DcNo='" & Trim(txt_Party_DcNo.Text) & "' ,Transport_Freight = " & Val(txt_Transport_Freight.Text) & " , Transport_IdNo = " & Val(Trans_ID) & " , User_idno = " & Val(Common_Procedures.User.IdNo) & " , Beam_Nos = '" & Trim(txt_BeamNos.Text) & "' , DeliveryTo_Idno=" & Val(DelAt_id) & " ,  Amount = " & Val(txt_Amount.Text) & " , LoomType_idno = " & Val(vLoomType_Idno) & " , Sizing_JobCode_forSelection = '" & Trim(cbo_jobcardno.Text) & "',Beam_Rate = " & Val(txt_Beam_Rate.Text) & ",EwayBill_No= '" & Trim(txt_EWBNo.Text) & "', Empty_Beam_HSN_Code = '" & Trim(txt_Empty_Beam_Hsn.Text) & "' , Empty_Bag_HSN_Code = '" & Trim(txt_Empty_Bag_Hsn.Text) & "', Empty_Cone_HSN_Code = '" & Trim(txt_Empty_Cone_Hsn.Text) & "', GST_Percentage = " & Val(txt_Gst_Tax.Text) & " , GST_Tax_Invoice_Status = " & Str(Val(vGST_Tax_Inv_Sts)) & "     Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Empty_BeamBagCone_Delivery_head", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Empty_BeamBagCone_Delivery_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Empty_Beam_Selection_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            EntID = Trim(Pk_Condition) & Trim(lbl_dcno.Text)
            Partcls = "Delv : Dc.No. " & Trim(lbl_dcno.Text)
            If Trim(txt_Book_No.Text) <> "" Then
                PBlNo = Trim(txt_Book_No.Text)
            Else
                PBlNo = Trim(lbl_dcno.Text)
            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            vLed_IdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Transport_Freight.Text) & "|" & -1 * Val(txt_Transport_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "EmBm:Frgt", Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), Trim(lbl_dcno.Text), Convert.ToDateTime(msk_date.Text), Partcls, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            If Val(txt_emptybeam.Text) <> 0 Or Val(txt_emptybags.Text) <> 0 Or Val(txt_emptycones.Text) <> 0 Or Val(txt_emptyBobin.Text) <> 0 Or Val(txt_EmptyBobin_Party.Text) <> 0 Or Val(txt_JumpoEmpty.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo                     , Reference_No                 , for_OrderBy               , Reference_Date, DeliveryTo_Idno         , ReceivedFrom_Idno       , Entry_ID             , Party_Bill_No        , Particulars            , Sl_No, Empty_Beam                          , Empty_Bags                          , Empty_Cones                          , Empty_Bobin                          , EmptyBobin_Party                           , Empty_Jumbo                           , Beam_Width_IdNo     ,             LoomType_idno      ) " &
                "Values                                      ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "', " & Str(Val(vOrdByNo)) & ", @DeliveryDate , " & Str(Val(led_id)) & ", " & Str(Val(Rec_id)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1    , " & Str(Val(txt_emptybeam.Text)) & ", " & Str(Val(txt_emptybags.Text)) & ", " & Str(Val(txt_emptycones.Text)) & ", " & Str(Val(txt_emptyBobin.Text)) & ", " & Str(Val(txt_EmptyBobin_Party.Text)) & ", " & Str(Val(txt_JumpoEmpty.Text)) & " , " & Val(Bw_ID) & "  ,  " & Val(vLoomType_Idno) & "  )"
                cmd.ExecuteNonQuery()
            End If




            vDELVLED_COMPIDNO = 0
            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
                vDELVLED_COMPIDNO = Common_Procedures.Ledger_IdNoToCompanyIdNo(con, Str(Val(led_id)), tr)
            End If

            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And vDELVLED_COMPIDNO <> 0 Then

                vCOMP_LEDIDNO = Common_Procedures.Company_IdnoToTextileLedgerIdNo(con, Str(Val(lbl_Company.Tag)), tr)

                vSELC_RCVDIDNO = 0
                vREC_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Rec_id)) & ")", , tr)
                If Trim(UCase(vREC_Ledtype)) = "GODOWN" Then
                    vSELC_RCVDIDNO = Rec_id
                Else
                    vSELC_RCVDIDNO = vCOMP_LEDIDNO
                End If

                cmd.CommandText = "Insert into Empty_Beam_Selection_Processing_Details (                   Reference_Code           ,                 Company_IdNo     ,            Reference_No      ,           for_OrderBy     , Reference_Date ,                Delivery_Code                ,           Delivery_No        ,       DeliveryTo_Idno   ,     ReceivedFrom_Idno   ,         DeliveryAt_Idno   ,         Party_Dc_No          , Beam_Width_IdNo    ,                Empty_Beam           ,                 Empty_Bags          ,                 Empty_Cones          ,     Selection_CompanyIdno          ,       Selection_Ledgeridno     ,      Selection_ReceivedFromIdNo  ) " &
                                    " Values                                           ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "', " & Str(Val(vOrdByNo)) & ",  @DeliveryDate , '" & Trim(Pk_Condition) & Trim(NewCode) & "', '" & Trim(lbl_dcno.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(Rec_id)) & ", " & Str(Val(DelAt_id)) & ", '" & Trim(lbl_dcno.Text) & "',  " & Val(Bw_ID) & ", " & Str(Val(txt_emptybeam.Text)) & ", " & Str(Val(txt_emptybags.Text)) & ", " & Str(Val(txt_emptycones.Text)) & ", " & Str(Val(vDELVLED_COMPIDNO)) & ", " & Str(Val(vCOMP_LEDIDNO)) & ", " & Str(Val(vSELC_RCVDIDNO)) & " ) "
                cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Empty_BeamBagCone_Receipt_Head set Delivery_code = '" & Trim(NewCode) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_code = '" & Trim(lbl_Receipt_Code.Text) & "'"
                'cmd.ExecuteNonQuery()

            End If

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
        'If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
    End Sub

    Private Sub cbo_partyname_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Partyname_DelvTo.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  ", "(Ledger_idno = 0)")
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(  ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        ElseIf Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'GODOWN'  or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End If

    End Sub

    Private Sub cbo_partyname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Partyname_DelvTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Partyname_DelvTo, msk_date, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) ", "(Ledger_idno = 0)")
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Partyname_DelvTo, msk_date, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        ElseIf Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Partyname_DelvTo, msk_date, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Partyname_DelvTo, msk_date, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End If

    End Sub

    Private Sub cbo_partyname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Partyname_DelvTo.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Partyname_DelvTo, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  ", "(Ledger_idno = 0)")
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Partyname_DelvTo, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        ElseIf Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Partyname_DelvTo, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 ) ", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Partyname_DelvTo, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 ) ", "(Ledger_idno = 0)")
        End If

    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Partyname_DelvTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Partyname_DelvTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_RecForm_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RecForm.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
            'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING' or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) and  Close_status = 0 ", "(Ledger_idno = 0)")
        End If
    End Sub

    Private Sub cbo_RecForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecForm, cbo_Partyname_DelvTo, cbo_DeliveryAt, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecForm, cbo_Partyname_DelvTo, cbo_DeliveryAt, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        End If

    End Sub

    Private Sub cbo_RecForm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RecForm.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecForm, cbo_DeliveryAt, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecForm, cbo_DeliveryAt, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        End If

    End Sub

    Private Sub cbo_RecForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RecForm.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub
    Private Sub cbo_delivery_to_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryAt.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
            'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING' or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) and  Close_status = 0 ", "(Ledger_idno = 0)")
        End If
    End Sub

    Private Sub cbo_delivery_to_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryAt.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryAt, cbo_RecForm, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryAt, cbo_RecForm, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        End If

    End Sub

    Private Sub cbo_delivery_to_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryAt.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryAt, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryAt, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        End If

    End Sub

    Private Sub cbo_delivery_to_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryAt.KeyUp
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
    Private Sub txt_remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_remarks.KeyDown
        If e.KeyValue = 38 Then ' SendKeys.Send("+{TAB}")
            txt_Amount.Focus()

        End If

        If (e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_vehicleno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_vehicleno.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Empty_BeamBagCone_Delivery_Head", "Vehicle_No", "", "Vehicle_No")

    End Sub

    Private Sub cbo_vehicleno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vehicleno.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vehicleno, Nothing, txt_Gst_Tax, "Empty_BeamBagCone_Delivery_Head", "Vehicle_No", "", "Vehicle_No")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If txt_JumpoEmpty.Visible And txt_JumpoEmpty.Enabled Then
                txt_JumpoEmpty.Focus()
            Else
                txt_Transport_Freight.Focus()
            End If
        End If
    End Sub
    Private Sub cbo_vehicleno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vehicleno.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vehicleno, txt_Gst_Tax, "Empty_BeamBagCone_Delivery_Head", "Vehicle_No", "", "", False)

    End Sub

    Private Sub txt_remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub



    Private Sub txt_emptybeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptybeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            txt_Empty_Beam_Hsn.Focus()
        End If

    End Sub

    Private Sub txt_emptybags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptybags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            txt_Empty_Bag_Hsn.Focus()
        End If

    End Sub

    Private Sub txt_emptycones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptycones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            txt_Empty_Cone_Hsn.Focus()
        End If

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

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Empty_BeamBagCone_Delivery_RefNo").ToString
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
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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





    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Empty_BeamBagCone_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try


        prn_TotCopies = 1
        Prnt_HalfSheet_STS = False

        vPrnt_2Copy_In_SinglePage = Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Sizing (Somanur)

            Dim mymsgbox As New Tsoft_MessageBox("Select Paper Size to Print", "A4,HALF-SHEET,CANCEL", "FOR PRINTING...", "IF A4 is selected, 2 copies of dc will be printed in single A4 sheet," & Chr(13) & "If HALF-SHEET is selected 1 copy of dc will be printed in 8x6 paper size", MesssageBoxIcons.Questions, 2)
            mymsgbox.ShowDialog()

            If mymsgbox.MessageBoxResult = 1 Then
                vPrnt_2Copy_In_SinglePage = 1

            ElseIf mymsgbox.MessageBoxResult = 2 Then
                Prnt_HalfSheet_STS = True

                vPrnt_2Copy_In_SinglePage = 0

            Else

                Exit Sub

            End If

            'prn_TotCopies = Val(InputBox("Enter No.of Copies", "FOR DELIVERY PRINTING...", "1"))
            'If Val(prn_TotCopies) <= 0 Then
            '    Exit Sub
            'End If

        End If


        set_PaperSize_For_PrintDocument1()

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    'Debug.Print(ps.PaperName)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '    If PpSzSTS = False Then
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                Exit For
        '            End If
        '        Next
        '    End If

        'End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Print_PDF_Status = True Then
                    PrintDocument1.DocumentName = "Delivery"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Delivery.pdf"
                    PrintDocument1.Print()

                Else
                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                            '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                            '    'Debug.Print(ps.PaperName)
                            '    If ps.Width = 800 And ps.Height = 600 Then
                            '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                            '        PrintDocument1.DefaultPageSettings.PaperSize = ps
                            '        PpSzSTS = True
                            '        Exit For
                            '    End If
                            'Next

                            'If PpSzSTS = False Then
                            '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                            '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                            '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                            '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                            '            PrintDocument1.DefaultPageSettings.PaperSize = ps
                            '            PpSzSTS = True
                            '            Exit For
                            '        End If
                            '    Next

                            '    If PpSzSTS = False Then
                            '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                            '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                            '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                            '                PrintDocument1.DefaultPageSettings.PaperSize = ps
                            '                Exit For
                            '            End If
                            '        Next
                            '    End If

                            'End If

                            set_PaperSize_For_PrintDocument1()
                            PrintDocument1.Print()
                        End If

                    Else
                        PrintDocument1.Print()

                    End If
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
                ppd.PrintPreviewControl.Zoom = 1.0


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
        prn_PageNo = 0

        'Try

        ' da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* ,D.* ,g.Ledger_name as Del_name,g.Ledger_Address1 as del_address1 ,g.Ledger_Address2 as del_address2 ,g.Ledger_Address3 as del_address3 ,g.Ledger_Address4 as del_address4 from Empty_BeamBagCone_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head g ON a.DeliveryTO_IdNo = g.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
        da1 = New SqlClient.SqlDataAdapter("select a.*, b.*,d.EndsCount_Name,T.Ledger_mAINname as transport,bw.Beam_Width_Name as Beam_type, c.*,g.Ledger_mAINname as Del_name,g.Ledger_Address1 as del_address1 ,g.Ledger_Address2 as del_address2 ,g.Ledger_Address3 as del_address3 ,g.Ledger_Address4 as del_address4 ,g.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, Dsh.State_Name as DeliveryTo_State_Name,Lsh.State_Name as Ledger_State_Name from Empty_BeamBagCone_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Ledger_Head g ON a.DeliveryTO_IdNo = g.Ledger_IdNo LEFT OUTER JOIN State_Head Dsh ON g.Ledger_State_IdNo = Dsh.State_IdNo  LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Beam_Width_Head bw ON a.Beam_Width_IdNo = bw.Beam_Width_IdNo LEFT OUTER JOIN Ledger_Head T ON T.Ledger_IdNo = a.Transport_IdNo  LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
        prn_HdDt = New DataTable
        da1.Fill(prn_HdDt)

        If prn_HdDt.Rows.Count <= 0 Then

            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End If



        da1.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then
            Printing_Format2(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
            Printing_Format_1186(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1408" Then
            Printing_Format_1408(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
            Printing_Format3(e)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Then
            Printing_Format_1474(e)

        Else

            Printing_Format3(e)
            'Printing_Format1(e)

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
        Dim I As Integer
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0
        Dim BMNos1 As String, BMNos2 As String, BMNos3 As String, BMNos4 As String

        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0

        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        PrntCnt = 1

        If vPrnt_2Copy_In_SinglePage = 1 Then
            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '    Debug.Print(ps.PaperName)
            '    If ps.Width = 800 And ps.Height = 600 Then
            '        PrintDocument1.DefaultPageSettings.PaperSize = ps
            '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '        e.PageSettings.PaperSize = ps
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next

            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If

        Else

            'If PpSzSTS = False Then
            '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '            PrintDocument1.DefaultPageSettings.PaperSize = ps
            '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
            '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '                e.PageSettings.PaperSize = ps
            '                Exit For
            '            End If
            '        Next
            '    End If

            ' End If
            set_PaperSize_For_PrintDocument1()

        End If



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 65
            .Right = 30
            .Top = 40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 16.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt

            If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then

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

            'NoofItems_PerPage = 5
            'If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then
            '    If prn_DetDt.Rows.Count > NoofItems_PerPage Then
            '        NoofItems_PerPage = 35
            '    End If
            'End If

            CurY = TpMargin
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

            CurY = CurY + strHeight
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

            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM/BAG/CONE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)

            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            CurY = CurY + strHeight + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

            C1 = 450
            C2 = PageWidth - (LMargin + C1)

            W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            End If


            If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


            ClArr(1) = Val(200) : ClArr(2) = 200
            ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))
            BMNos1 = ""
            If Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString) <> "" Then
                BMNos1 = "BEAM No.s  : " & Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString)
            End If
            BMNos2 = ""
            BMNos3 = ""
            BMNos4 = ""
            If Len(BMNos1) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos1), I, 1) = " " Or Mid$(Trim(BMNos1), I, 1) = "," Or Mid$(Trim(BMNos1), I, 1) = "." Or Mid$(Trim(BMNos1), I, 1) = "-" Or Mid$(Trim(BMNos1), I, 1) = "/" Or Mid$(Trim(BMNos1), I, 1) = "_" Or Mid$(Trim(BMNos1), I, 1) = "(" Or Mid$(Trim(BMNos1), I, 1) = ")" Or Mid$(Trim(BMNos1), I, 1) = "\" Or Mid$(Trim(BMNos1), I, 1) = "[" Or Mid$(Trim(BMNos1), I, 1) = "]" Or Mid$(Trim(BMNos1), I, 1) = "{" Or Mid$(Trim(BMNos1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos2 = Microsoft.VisualBasic.Right(Trim(BMNos1), Len(BMNos1) - I)
                BMNos1 = Microsoft.VisualBasic.Left(Trim(BMNos1), I)
            End If

            If Len(BMNos2) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos2), I, 1) = " " Or Mid$(Trim(BMNos2), I, 1) = "," Or Mid$(Trim(BMNos2), I, 1) = "." Or Mid$(Trim(BMNos2), I, 1) = "-" Or Mid$(Trim(BMNos2), I, 1) = "/" Or Mid$(Trim(BMNos2), I, 1) = "_" Or Mid$(Trim(BMNos2), I, 1) = "(" Or Mid$(Trim(BMNos2), I, 1) = ")" Or Mid$(Trim(BMNos2), I, 1) = "\" Or Mid$(Trim(BMNos2), I, 1) = "[" Or Mid$(Trim(BMNos2), I, 1) = "]" Or Mid$(Trim(BMNos2), I, 1) = "{" Or Mid$(Trim(BMNos2), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos3 = Microsoft.VisualBasic.Right(Trim(BMNos2), Len(BMNos2) - I)
                BMNos2 = Microsoft.VisualBasic.Left(Trim(BMNos2), I)
            End If

            If Len(BMNos3) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos3), I, 1) = " " Or Mid$(Trim(BMNos3), I, 1) = "," Or Mid$(Trim(BMNos3), I, 1) = "." Or Mid$(Trim(BMNos3), I, 1) = "-" Or Mid$(Trim(BMNos3), I, 1) = "/" Or Mid$(Trim(BMNos3), I, 1) = "_" Or Mid$(Trim(BMNos3), I, 1) = "(" Or Mid$(Trim(BMNos3), I, 1) = ")" Or Mid$(Trim(BMNos3), I, 1) = "\" Or Mid$(Trim(BMNos3), I, 1) = "[" Or Mid$(Trim(BMNos3), I, 1) = "]" Or Mid$(Trim(BMNos3), I, 1) = "{" Or Mid$(Trim(BMNos3), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos4 = Microsoft.VisualBasic.Right(Trim(BMNos3), Len(BMNos3) - I)
                BMNos3 = Microsoft.VisualBasic.Left(Trim(BMNos3), I)
            End If

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "We sent you", LMargin + 20, CurY, 0, 0, pFont)



            CurY = CurY + TxtHgt
            If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
                BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & " (" & BmsInWrds & ") empty beams "

                Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)

            End If

            If Trim(BMNos1) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos1), LMargin + C1, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Beam_type").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "BEAM WIDTH :", LMargin + C1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Beam_type").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString))
                BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) & " (" & BmsInWrds & ") empty bags "

                Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
            End If
            If Trim(BMNos2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos2), LMargin + C1, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Approximate Value", LMargin + C1 + 70, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + 195, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + C1 + 200, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_cones").ToString))
                BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) & " (" & BmsInWrds & ") empty cones "


                Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
            End If
            If Trim(BMNos3) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos3), LMargin + C1, CurY, 0, 0, pFont)
            End If

            '-------------------------*********************

            If Val(prn_HdDt.Rows(0).Item("Amount").ToString) <> 0 Then

                vCgst_amt = Format((Val(prn_HdDt.Rows(0).Item("Amount").ToString) * 2.5 / 100), "############0")
                vSgst_amt = Format((Val(prn_HdDt.Rows(0).Item("Amount").ToString) * 2.5 / 100), "############0")

                Common_Procedures.Print_To_PrintDocument(e, " CGST 2.5 % : " & vCgst_amt, LMargin + C1 + 65, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " SGST 2.5 % : " & vSgst_amt, LMargin + C1 + 180, CurY, 0, 0, pFont)
            End If
            '-------------------------*********************

            CurY = CurY + TxtHgt + 5

            If Val(prn_HdDt.Rows(0).Item("Amount").ToString) <> 0 Then
                vTxamt = Val(vCgst_amt) + Val(vSgst_amt) 'Format((Val(prn_HdDt.Rows(0).Item("Amount").ToString) * 5 / 100), "############0.00")

                Common_Procedures.Print_To_PrintDocument(e, " Tax Amount : " & vTxamt, LMargin + C1 + 65, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Delivery To. : " & Trim(prn_HdDt.Rows(0).Item("Del_name").ToString), LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt

                '-------------------------*********************
                If Val(vTxamt) <> 0 Then
                    vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) + vTxamt, "###########0.00")
                    Common_Procedures.Print_To_PrintDocument(e, " Net Amount : " & vNtAMt, LMargin + C1 + 65, CurY, 0, 0, pFont)
                End If
                '-------------------------*********************

                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) & Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString), LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) & Trim(prn_HdDt.Rows(0).Item("Del_Address4").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If




            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, "Through Vehicle No. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)
            End If
            If Trim(BMNos4) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos4), LMargin + C1, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "REMARKS  : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            End If

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 325, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))



            If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_HdDt.Rows.Count = cnt + 6 Then
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


                e.HasMorePages = True
                Return

            End If

        End If
    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim CurY1 As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim I As Integer
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0
        Dim BMNos1 As String, BMNos2 As String, BMNos3 As String, BMNos4 As String

        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0

        Dim vTxPerc As String = 0
        Dim vIgst_amt As String = 0
        Dim Cmp_Gstin_No As String

        Dim Sno As Integer = 0
        Dim vGST_BILL As Integer = 0

        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        PrntCnt = 1

        If vPrnt_2Copy_In_SinglePage = 1 Then
            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '    Debug.Print(ps.PaperName)
            '    If ps.Width = 800 And ps.Height = 600 Then
            '        PrintDocument1.DefaultPageSettings.PaperSize = ps
            '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '        e.PageSettings.PaperSize = ps
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next

            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If

        Else

            'If PpSzSTS = False Then
            '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '            PrintDocument1.DefaultPageSettings.PaperSize = ps
            '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
            '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '                e.PageSettings.PaperSize = ps
            '                Exit For
            '            End If
            '        Next
            '    End If

            ' End If
            set_PaperSize_For_PrintDocument1()

        End If



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 65
            .Right = 40 '30
            .Top = 30 '40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With



        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClArr(1) = Val(35) : ClArr(2) = 120 : ClArr(3) = 75 : ClArr(4) = 45 : ClArr(5) = 45 : ClArr(6) = 45 : ClArr(7) = 85
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        If Trim(Common_Procedures.settings.CustomerCode) = "1027" Then
            TxtHgt = 15.5 '15.5 '16 '16.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        Else
            TxtHgt = 16 '16.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

        For PCnt = 1 To PrntCnt

            If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then

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

            'NoofItems_PerPage = 5
            'If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then
            '    If prn_DetDt.Rows.Count > NoofItems_PerPage Then
            '        NoofItems_PerPage = 35
            '    End If
            'End If

            CurY = TpMargin
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
            Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Gstin_No = ""

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
            End If
            'If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            '    Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
            'End If
            If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
                Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
            End If

            If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
                Cmp_Gstin_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
            End If

            vGST_BILL = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)



            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            'If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            '    If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

            '        Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
            '        If Not imageData Is Nothing Then
            '            Using ms As New MemoryStream(imageData, 0, imageData.Length)
            '                ms.Write(imageData, 0, imageData.Length)

            '                If imageData.Length > 0 Then

            '                    '.BackgroundImage = Image.FromStream(ms)

            '                    ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
            '                    e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY + 5, 90, 90)

            '                End If

            '            End Using

            '        End If

            '    End If

            'End If

            CurY = CurY + strHeight - 10
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Gstin_No, LMargin + 10, CurY + 10, 0, 0, pFont)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Gstin_No, LMargin + 10, CurY + 5, 0, 0, pFont)

            p1Font = New Font("Calibri", 15, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM/BAG/CONE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "( Not For Sale )", LMargin + 30, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SAC : 99882 ( Textile Manufacture )", LMargin, CurY + 15, 2, PrintWidth, pFont)


            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            CurY = CurY + strHeight + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            CurY = CurY + TxtHgt - 13
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, p1Font)

            C1 = 450
            C2 = PageWidth - (LMargin + C1)

            W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 55, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString), LMargin + C1 + W1 + 65, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY1 = CurY
            CurY1 = CurY1 + 5
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 55, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 65, CurY1, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY1 = CurY
            CurY1 = CurY1 + 10
            If Trim(prn_HdDt.Rows(0).Item("EwayBill_No").ToString) <> "" Then

                Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL NO", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 55, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EwayBill_No").ToString, LMargin + C1 + W1 + 65, CurY1, 0, 0, pFont)

            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                CurY1 = CurY
                CurY1 = CurY1 + 15
                Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 55, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + W1 + 65, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Or Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString) <> 0 Then
                    CurY1 = CurY
                    CurY1 = CurY1 + 20
                    Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 40, CurY1, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, prn_HdDt.Rows(0).Item("Transport_IdNo").ToString), LMargin + C1 + W1 + 50, CurY1, 0, 0, pFont)
                End If

            End If


            If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "     " & "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + 10, CurY, 0, 0, pFont)

            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


            BMNos1 = ""
            If Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString) <> "" Then
                BMNos1 = Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString)
                'BMNos1 = "BEAM No.s  : " & Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString)
            End If
            BMNos2 = ""
            BMNos3 = ""
            BMNos4 = ""
            If Len(BMNos1) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos1), I, 1) = " " Or Mid$(Trim(BMNos1), I, 1) = "," Or Mid$(Trim(BMNos1), I, 1) = "." Or Mid$(Trim(BMNos1), I, 1) = "-" Or Mid$(Trim(BMNos1), I, 1) = "/" Or Mid$(Trim(BMNos1), I, 1) = "_" Or Mid$(Trim(BMNos1), I, 1) = "(" Or Mid$(Trim(BMNos1), I, 1) = ")" Or Mid$(Trim(BMNos1), I, 1) = "\" Or Mid$(Trim(BMNos1), I, 1) = "[" Or Mid$(Trim(BMNos1), I, 1) = "]" Or Mid$(Trim(BMNos1), I, 1) = "{" Or Mid$(Trim(BMNos1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos2 = Microsoft.VisualBasic.Right(Trim(BMNos1), Len(BMNos1) - I)
                BMNos1 = Microsoft.VisualBasic.Left(Trim(BMNos1), I)
            End If

            If Len(BMNos2) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos2), I, 1) = " " Or Mid$(Trim(BMNos2), I, 1) = "," Or Mid$(Trim(BMNos2), I, 1) = "." Or Mid$(Trim(BMNos2), I, 1) = "-" Or Mid$(Trim(BMNos2), I, 1) = "/" Or Mid$(Trim(BMNos2), I, 1) = "_" Or Mid$(Trim(BMNos2), I, 1) = "(" Or Mid$(Trim(BMNos2), I, 1) = ")" Or Mid$(Trim(BMNos2), I, 1) = "\" Or Mid$(Trim(BMNos2), I, 1) = "[" Or Mid$(Trim(BMNos2), I, 1) = "]" Or Mid$(Trim(BMNos2), I, 1) = "{" Or Mid$(Trim(BMNos2), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos3 = Microsoft.VisualBasic.Right(Trim(BMNos2), Len(BMNos2) - I)
                BMNos2 = Microsoft.VisualBasic.Left(Trim(BMNos2), I)
            End If

            If Len(BMNos3) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos3), I, 1) = " " Or Mid$(Trim(BMNos3), I, 1) = "," Or Mid$(Trim(BMNos3), I, 1) = "." Or Mid$(Trim(BMNos3), I, 1) = "-" Or Mid$(Trim(BMNos3), I, 1) = "/" Or Mid$(Trim(BMNos3), I, 1) = "_" Or Mid$(Trim(BMNos3), I, 1) = "(" Or Mid$(Trim(BMNos3), I, 1) = ")" Or Mid$(Trim(BMNos3), I, 1) = "\" Or Mid$(Trim(BMNos3), I, 1) = "[" Or Mid$(Trim(BMNos3), I, 1) = "]" Or Mid$(Trim(BMNos3), I, 1) = "{" Or Mid$(Trim(BMNos3), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos4 = Microsoft.VisualBasic.Right(Trim(BMNos3), Len(BMNos3) - I)
                BMNos3 = Microsoft.VisualBasic.Left(Trim(BMNos3), I)
            End If

            'CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "We sent you", LMargin + 20, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "SL NO", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClArr(4) + ClArr(5) - 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "UOM", LMargin + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "BEAM WIDTH", LMargin + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "BEAM NOS", LMargin + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClArr(1), CurY, 2, ClArr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClArr(1) + ClArr(2), CurY, 2, ClArr(3), pFont)
            If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 40, CurY, 2, ClArr(4), pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "UOM", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 2, ClArr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY - 10, 2, ClArr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WIDTH", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY + 5, 2, ClArr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM NOS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, 2, ClArr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Dim vS1 As Single = 0
            Dim vS2 As Single = 0
            Dim vS3 As Single = 0
            Dim vS4 As Single = 0


            vS1 = ClArr(1) + ClArr(2)
            vS2 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4)
            vS3 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6)
            vS4 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7)

            Sno = 0

            For J As Integer = 0 To prn_HdDt.Rows.Count - 1



                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then

                    Sno = Sno + 1

                    Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Empty Beam", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Beam_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)

                    If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Beam").ToString, LMargin + vS2 + 10, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Beam").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, "NOS", LMargin + vS2 + ClArr(5) + 5, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Beam_type").ToString, LMargin + vS3 + 5, CurY, 0, 0, pFont)

                    If Trim(BMNos1) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos1), LMargin + vS4 + 5, CurY, 0, 0, pFont)
                    End If

                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Or Trim(BMNos2) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Then
                        Sno = Sno + 1

                        Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "Empty Bags", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bag_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)

                        If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bags").ToString, LMargin + vS2 + 10, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bags").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "NOS", LMargin + vS2 + ClArr(5) + 5, CurY, 0, 0, pFont)
                    End If

                    If Trim(BMNos2) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos2), LMargin + vS4 + 5, CurY, 0, 0, pFont)
                    End If
                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) <> 0 Or Trim(BMNos3) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) <> 0 Then
                        Sno = Sno + 1

                        Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "Empty Cones", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cone_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)

                        If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cones").ToString, LMargin + vS2 + 10, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cones").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "NOS", LMargin + vS2 + ClArr(5) + 5, CurY, 0, 0, pFont)
                    End If
                    If Trim(BMNos3) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos3), LMargin + vS4 + 5, CurY, 0, 0, pFont)
                    End If
                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bobin").ToString)) <> 0 Or Trim(BMNos4) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bobin").ToString)) <> 0 Then
                        Sno = Sno + 1

                        Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, "Empty Bobin", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cone_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)
                        If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bobin").ToString, LMargin + vS2 + 10, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bobin").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "NOS", LMargin + vS2 + ClArr(5) + 5, CurY, 0, 0, pFont)
                    End If
                    If Trim(BMNos4) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos4), LMargin + vS4 + 5, CurY, 0, 0, pFont)
                    End If
                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Jumbo").ToString)) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    Sno = Sno + 1

                    Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "Empty Jumbo", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cone_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)
                    If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Jumbo").ToString, LMargin + vS2 + 10, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Jumbo").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, "NOS", LMargin + vS2 + ClArr(5) + 5, CurY, 0, 0, pFont)
                End If


            Next J



            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + 5, LnAr(3), LMargin + ClArr(1) + 5, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2), LnAr(3), LMargin + ClArr(1) + ClArr(2), CurY)

            If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY)
                e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY)
            Else
                e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 30, LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 30, CurY)
            End If

            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY)

            '-------------------------*********************
            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            If Val(prn_HdDt.Rows(0).Item("Amount").ToString) <> 0 And Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1158" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Value            :  ", LMargin + C1 + 50, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + C1 + 160, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value    :  ", LMargin + C1 + 50, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + C1 + 160, CurY, 0, 0, pFont)
                End If
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods  :  ", LMargin + C1 + 50, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + C1 + 165, CurY, 0, 0, p1Font)
            End If



            If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO : ", LMargin + 10, CurY, 0, 0, p1Font)
            End If
                    If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_name").ToString), LMargin + ClArr(1) + 72, CurY, 0, 0, p1Font)
            End If
                    '-------------------------*********************

                    CurY = CurY + TxtHgt + 5
            vTxPerc = 0
            vCgst_amt = 0
            vSgst_amt = 0
            vIgst_amt = 0
            If Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

                If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then

                    vTxPerc = Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString) / 2, "############0.00")


                    vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(vTxPerc) / 100, "############0.00")
                    vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(vTxPerc) / 100, "############0.00")
                    '
                    'vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * 2.5 / 100, "############0.00")
                    'vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * 2.5 / 100, "############0.00")

                    Common_Procedures.Print_To_PrintDocument(e, " CGST " & Val(vTxPerc) & " % : " & vCgst_amt, LMargin + C1 + 47, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " SGST " & Val(vTxPerc) & " % : " & vSgst_amt, LMargin + C1 + 190, CurY, 0, 0, pFont)

                Else

                    vTxPerc = prn_HdDt.Rows(0).Item("GST_Percentage").ToString
                    vIgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                    Common_Procedures.Print_To_PrintDocument(e, " IGST " & Val(vTxPerc) & "% : " & vIgst_amt, LMargin + C1 + 47, CurY, 0, 0, pFont)

                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) & "  " & Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString), LMargin + 10, CurY - 3, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            '-------------------------*********************
            If Val(prn_HdDt.Rows(0).Item("Amount").ToString) <> 0 And Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

                vTxamt = Val(vCgst_amt) + Val(vSgst_amt) + Val(vIgst_amt)
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) + vTxamt, "###########0.00")

                Common_Procedures.Print_To_PrintDocument(e, "Value of Goods : " & vNtAMt, LMargin + C1 + 50, CurY + 5, 0, 0, p1Font)

            End If
            '-------------------------*********************

            If Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Del_Address4").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) & "  " & Trim(prn_HdDt.Rows(0).Item("Del_Address4").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN  : " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If


            'If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            '    CurY = CurY + TxtHgt + 5
            '    Common_Procedures.Print_To_PrintDocument(e, "Through Vehicle No. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)
            'End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "REMARKS  : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            End If

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 325, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 20, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))



            If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_HdDt.Rows.Count = cnt + 6 Then
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


                e.HasMorePages = True
                Return

            End If

        End If
    End Sub
    Private Sub Printing_Format_1186_old(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, city As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
        Dim Cmp_State As String, Cmp_StateCode As String, Cmp_GSTIN_No As String, Cmp_EMail As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim I As Integer
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0
        Dim BMNos1 As String, BMNos2 As String, BMNos3 As String, BMNos4 As String


        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        PrntCnt = 1

        If vPrnt_2Copy_In_SinglePage = 1 Then
            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '    Debug.Print(ps.PaperName)
            '    If ps.Width = 800 And ps.Height = 600 Then
            '        PrintDocument1.DefaultPageSettings.PaperSize = ps
            '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '        e.PageSettings.PaperSize = ps
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next

            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If

        Else

            'If PpSzSTS = False Then
            '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '            PrintDocument1.DefaultPageSettings.PaperSize = ps
            '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
            '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '                e.PageSettings.PaperSize = ps
            '                Exit For
            '            End If
            '        Next
            '    End If

            ' End If
            set_PaperSize_For_PrintDocument1()

        End If



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 65
            .Right = 30
            .Top = 40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9, FontStyle.Regular)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 16 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt

            If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then

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

            'NoofItems_PerPage = 5
            'If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then
            '    If prn_DetDt.Rows.Count > NoofItems_PerPage Then
            '        NoofItems_PerPage = 35
            '    End If
            'End If

            CurY = TpMargin
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
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)


            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            CurY = CurY + strHeight
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & ", " & city, LMargin + 10, CurY, 0, PrintWidth, pFont)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "  / " & Cmp_CstNo, LMargin + 10, CurY, 0, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "  /  " & Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
            CurY = CurY + TxtHgt - 12
            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(15) = CurY
            CurY = CurY + TxtHgt - 12
            p1Font = New Font("Calibri", 16, FontStyle.Bold)


            ' Common_Procedures.Print_To_PrintDocument(e, "D.C No . : " & Trim(prn_HdDt.Rows(0).Item("JobWork_Empty_BeamBagCone_Delivery_No").ToString), LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
            ' Common_Procedures.Print_To_PrintDocument(e, "D.C DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Empty_BeamBagCone_Delivery_Date").ToString)), LMargin + 610, CurY, 0, 0, pFont)


            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            CurY = CurY + strHeight + 5
            ' e.Graphics.DrawLine(Pens.Black, LMargin + 150, CurY, LMargin + 150, LnAr(15))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + 600, CurY, LMargin + 600, LnAr(15))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

            C1 = 450
            C2 = PageWidth - (LMargin + C1)

            W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


            ClArr(1) = Val(200) : ClArr(2) = 200
            ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))
            BMNos1 = ""
            If Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString) <> "" Then
                BMNos1 = "BEAM No.s  : " & Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString)
            End If
            BMNos2 = ""
            BMNos3 = ""
            BMNos4 = ""
            If Len(BMNos1) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos1), I, 1) = " " Or Mid$(Trim(BMNos1), I, 1) = "," Or Mid$(Trim(BMNos1), I, 1) = "." Or Mid$(Trim(BMNos1), I, 1) = "-" Or Mid$(Trim(BMNos1), I, 1) = "/" Or Mid$(Trim(BMNos1), I, 1) = "_" Or Mid$(Trim(BMNos1), I, 1) = "(" Or Mid$(Trim(BMNos1), I, 1) = ")" Or Mid$(Trim(BMNos1), I, 1) = "\" Or Mid$(Trim(BMNos1), I, 1) = "[" Or Mid$(Trim(BMNos1), I, 1) = "]" Or Mid$(Trim(BMNos1), I, 1) = "{" Or Mid$(Trim(BMNos1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos2 = Microsoft.VisualBasic.Right(Trim(BMNos1), Len(BMNos1) - I)
                BMNos1 = Microsoft.VisualBasic.Left(Trim(BMNos1), I)
            End If

            If Len(BMNos2) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos2), I, 1) = " " Or Mid$(Trim(BMNos2), I, 1) = "," Or Mid$(Trim(BMNos2), I, 1) = "." Or Mid$(Trim(BMNos2), I, 1) = "-" Or Mid$(Trim(BMNos2), I, 1) = "/" Or Mid$(Trim(BMNos2), I, 1) = "_" Or Mid$(Trim(BMNos2), I, 1) = "(" Or Mid$(Trim(BMNos2), I, 1) = ")" Or Mid$(Trim(BMNos2), I, 1) = "\" Or Mid$(Trim(BMNos2), I, 1) = "[" Or Mid$(Trim(BMNos2), I, 1) = "]" Or Mid$(Trim(BMNos2), I, 1) = "{" Or Mid$(Trim(BMNos2), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos3 = Microsoft.VisualBasic.Right(Trim(BMNos2), Len(BMNos2) - I)
                BMNos2 = Microsoft.VisualBasic.Left(Trim(BMNos2), I)
            End If

            If Len(BMNos3) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos3), I, 1) = " " Or Mid$(Trim(BMNos3), I, 1) = "," Or Mid$(Trim(BMNos3), I, 1) = "." Or Mid$(Trim(BMNos3), I, 1) = "-" Or Mid$(Trim(BMNos3), I, 1) = "/" Or Mid$(Trim(BMNos3), I, 1) = "_" Or Mid$(Trim(BMNos3), I, 1) = "(" Or Mid$(Trim(BMNos3), I, 1) = ")" Or Mid$(Trim(BMNos3), I, 1) = "\" Or Mid$(Trim(BMNos3), I, 1) = "[" Or Mid$(Trim(BMNos3), I, 1) = "]" Or Mid$(Trim(BMNos3), I, 1) = "{" Or Mid$(Trim(BMNos3), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos4 = Microsoft.VisualBasic.Right(Trim(BMNos3), Len(BMNos3) - I)
                BMNos3 = Microsoft.VisualBasic.Left(Trim(BMNos3), I)
            End If

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "We sent your", LMargin + 20, CurY, 0, 0, pFont)



            CurY = CurY + TxtHgt
            If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
                BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")



                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & " (" & BmsInWrds & ") empty beams "
                Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)

            End If

            If Trim(BMNos1) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos1), LMargin + C1, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString))
                BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) & " (" & BmsInWrds & ") empty bags "

                Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
            End If
            If Trim(BMNos2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos2), LMargin + C1, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_cones").ToString))
                BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) & " (" & BmsInWrds & ") empty cones "


                Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
            End If
            If Trim(BMNos3) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos3), LMargin + C1, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5
            If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Delivery To. : " & Trim(prn_HdDt.Rows(0).Item("Del_name").ToString), LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) & Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString), LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) & Trim(prn_HdDt.Rows(0).Item("Del_Address4").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If



            CurY = CurY + TxtHgt + 5
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Through Vehicle No. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)
            End If
            If Trim(BMNos4) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos4), LMargin + C1, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "A BEAM - PS : PICKING SIDE", LMargin + 500, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DIRECTION : REVERSE", LMargin + 500, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "A BEAM - PS : RECEIVING SIDE", LMargin + 500, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DIRECTION : FORWARD", LMargin + 500, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "REMARKS  : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            End If
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 325, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signature ", LMargin + PageWidth - 20, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))



            If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_HdDt.Rows.Count = cnt + 6 Then
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


                e.HasMorePages = True
                Return

            End If

        End If
    End Sub
    Private Sub Printing_Format_1186(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, city As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
        Dim Cmp_State As String, Cmp_StateCode As String, Cmp_GSTIN_No As String, Cmp_EMail As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim W2 As Single
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
            .Left = 20 ' 65
            .Right = 30
            .Top = 40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9, FontStyle.Regular)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 16 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

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
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)


        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & ", " & city, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "  / " & Cmp_CstNo, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "  /  " & Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(15) = CurY
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "D.C No . : " & Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString), LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "D.C DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString)), LMargin + 610, CurY, 0, 0, pFont)


        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin + 150, CurY, LMargin + 150, LnAr(15))
        e.Graphics.DrawLine(Pens.Black, LMargin + 600, CurY, LMargin + 600, LnAr(15))
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline Or FontStyle.Italic)
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, " TO : ", LMargin + 10, CurY, 0, 0, p1Font)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY AT : ", LMargin + C1 + 10, CurY, 0, 0, p1Font)


        W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_MAINName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("del_name").ToString), LMargin + C1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("del_address1").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Empty_BeamBagCone_Delivery_Date").ToString)), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("del_address2").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("del_address3").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("Ledger_Address4").ToString <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("del_address4").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)

        End If

        If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + 10, CurY, 0, 0, pFont)

        End If
        If prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)

        End If
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "STATE : " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)

        'Ledger_GSTinNo DeliveryTo_State_Name
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


        ClArr(1) = Val(200) : ClArr(2) = 200
        ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

        W2 = e.Graphics.MeasureString("A BEAM - PS  : ", pFont).Width

        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        'If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Delivery At. : " & Trim(prn_HdDt.Rows(0).Item("Del_name").ToString), LMargin + 10, CurY, 0, 0, p1Font)
        'End If


        'If Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) <> "" Then
        '    CurY = CurY + TxtHgt + 5
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString), LMargin + 10, CurY, 0, 0, p1Font)

        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString) <> "" Then
        '    CurY = CurY + TxtHgt + 5

        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString), LMargin + 10, CurY, 0, 0, p1Font)

        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) <> "" Then
        '    CurY = CurY + TxtHgt + 5

        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString), LMargin + 10, CurY, 0, 0, p1Font)

        'End If
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "No. Of Beam  ", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " :   " & Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)), LMargin + W2 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "NOTE : ", LMargin + 500, CurY, 0, 0, p1Font)
        End If
        CurY = CurY + TxtHgt + 5

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "A BEAM - PS : PICKING SIDE", LMargin + 500, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Beam Nos   ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " :   " & Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString), LMargin + W2 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Transport  ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " :   " & Trim(prn_HdDt.Rows(0).Item("Transport").ToString), LMargin + W2 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DIRECTION   : REVERSE", LMargin + 500, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Vehicle No ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " :   " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + W2 + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "B BEAM - RS : RECEIVING SIDE", LMargin + 500, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Beam Type  ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " :   " & Trim(prn_HdDt.Rows(0).Item("Beam_type").ToString), LMargin + W2 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DIRECTION   : FORWARD", LMargin + 500, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(11) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, " REMARKS :  " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        End If

        LnAr(11) = CurY
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        End If
        If Common_Procedures.settings.CustomerCode = "1186" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

        End If
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        If Common_Procedures.settings.CustomerCode <> "1186" Then
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

        End If

        If Common_Procedures.settings.CustomerCode = "1186" Then
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + 250, CurY, LMargin + 250, LnAr(11))
        e.Graphics.DrawLine(Pens.Black, LMargin + 450, CurY, LMargin + 450, LnAr(11))
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        e.HasMorePages = False

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
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim beamWidth As String = ""
        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            Debug.Print(ps.PaperName)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 65
            .Right = 30
            .Top = 40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

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

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 15, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM/BAG/CONE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


        ClArr(1) = Val(200) : ClArr(2) = 200
        ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "We sent your", LMargin + 20, CurY, 0, 0, pFont)



        CurY = CurY + TxtHgt
        If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & " (" & BmsInWrds & ") empty beams "

            Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)

        End If

        Common_Procedures.Print_To_PrintDocument(e, "EndsCount :  " & prn_HdDt.Rows(0).Item("EndsCount_Name").ToString, LMargin + C1, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5
        If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Then
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) & " (" & BmsInWrds & ") empty bags "

            Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
        End If
        beamWidth = Common_Procedures.BeamWidth_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Beam_Width_IdNo").ToString))

        Common_Procedures.Print_To_PrintDocument(e, "Beam Width :  " & beamWidth, LMargin + C1, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt + 5
        If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_cones").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) & " (" & BmsInWrds & ") empty cones "


            Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("REmarks").ToString, LMargin + C1, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5


        Common_Procedures.Print_To_PrintDocument(e, "Through vehicle no. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 325, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        e.HasMorePages = False

    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
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
        If Asc(e.KeyChar) = 13 Then
            cbo_vehicleno.Focus()
        End If
    End Sub

    Private Sub txt_emptyBobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptyBobin.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyBobin_Party_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBobin_Party.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_JumpoEmpty.Focus()
        End If
    End Sub
    Private Sub cbo_beamwidth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_beamwidth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")

    End Sub


    Private Sub cbo_beamwidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_beamwidth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_beamwidth, Nothing, Nothing, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")


        If (e.KeyValue = 40 And cbo_beamwidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_LoomType_Creation.Enabled And cbo_LoomType_Creation.Visible = True Then
                cbo_LoomType_Creation.Focus()
            Else

                txt_BeamNos.Focus()

            End If


        End If



        If e.KeyCode = 38 Then
            txt_Empty_Cone_Hsn.Focus()
        End If


    End Sub

    Private Sub cbo_beamwidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_beamwidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_beamwidth, Nothing, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If cbo_LoomType_Creation.Enabled And cbo_LoomType_Creation.Visible = True Then
                cbo_LoomType_Creation.Focus()
            Else
                txt_BeamNos.Focus()
            End If


        End If


    End Sub

    Private Sub cbo_beamwidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_beamwidth.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_beamwidth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

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
                    dtp_date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_date.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.TextChanged
        If IsDate(dtp_date.Text) = True Then

            msk_date.Text = dtp_date.Text
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

        If e.KeyCode = 38 Then
            cbo_DcSufixNo.Focus()
        ElseIf e.KeyCode = 40 Then
            cbo_Partyname_DelvTo.Focus()
        End If

    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.Enter
        msk_date.Focus()
        msk_date.SelectionStart = 0
    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()
    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, Nothing, txt_Transport_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_Transport.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_jobcardno.Enabled And cbo_jobcardno.Visible = True Then
                cbo_jobcardno.Focus()
            Else
                txt_BeamNos.Focus()
            End If
        End If




        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Transport_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()
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
    Private Sub txt_Transport_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Transport_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, txt_Amount, txt_remarks, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_remarks, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")



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

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Partyname_DelvTo.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")


            ' If Trim(AgPNo) <> "" Then
            PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", "", "")
            ' End If

            smstxt = smstxt & "DC No : " & Trim(lbl_dcno.Text) & vbCrLf
            smstxt = smstxt & " Date : " & Trim(msk_date.Text) & vbCrLf
            smstxt = smstxt & " Empty Beam: " & Val(txt_emptybeam.Text) & vbCrLf

            smstxt = smstxt & " Empty Bag : " & Trim(txt_emptybags.Text) & vbCrLf



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
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()

        'Print_PDF_Status = False
    End Sub

    Private Sub Printing_Format_1408(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim I As Integer
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0
        Dim BMNos1 As String, BMNos2 As String, BMNos3 As String, BMNos4 As String


        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        PrntCnt = 1

        If vPrnt_2Copy_In_SinglePage = 1 Then
            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '    Debug.Print(ps.PaperName)
            '    If ps.Width = 800 And ps.Height = 600 Then
            '        PrintDocument1.DefaultPageSettings.PaperSize = ps
            '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '        e.PageSettings.PaperSize = ps
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next

            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If

        Else

            'If PpSzSTS = False Then
            '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '            PrintDocument1.DefaultPageSettings.PaperSize = ps
            '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
            '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '                e.PageSettings.PaperSize = ps
            '                Exit For
            '            End If
            '        Next
            '    End If

            ' End If
            set_PaperSize_For_PrintDocument1()

        End If



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 65
            .Right = 30
            .Top = 40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        p1Font = New Font("TSCu_SaiIndira", 11, FontStyle.Regular)
        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 16.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt

            If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then

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

            'NoofItems_PerPage = 5
            'If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then
            '    If prn_DetDt.Rows.Count > NoofItems_PerPage Then
            '        NoofItems_PerPage = 35
            '    End If
            'End If

            CurY = TpMargin
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
            Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Tamil_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Tamil_Address1").ToString '& " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Tamil_Address2").ToString '& " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
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
            p1Font = New Font("SaiIndira", 16, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
            p1Font = New Font("SaiIndira", 11, FontStyle.Regular)
            CurY = CurY + strHeight
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 10

            p1Font = New Font("SaiIndira", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "À£õ ¦¼Ä¢Å¡¢ Ãº£Ð", LMargin, CurY, 2, PrintWidth, p1Font)

            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
            p1Font = New Font("Calibri", 15, FontStyle.Bold)

            CurY = CurY + strHeight + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

            C1 = 450
            C2 = PageWidth - (LMargin + C1)

            W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("SaiIndira", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "¦¿. ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("SaiIndira", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "§¾¾¢ ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

            pFont = New Font("SaiIndira", 11, FontStyle.Regular)
            ClArr(1) = Val(200) : ClArr(2) = 200
            ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))
            BMNos1 = ""
            If Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString) <> "" Then
                BMNos1 = "BEAM No.s  : " & Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString)
            End If
            BMNos2 = ""
            BMNos3 = ""
            BMNos4 = ""
            If Len(BMNos1) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos1), I, 1) = " " Or Mid$(Trim(BMNos1), I, 1) = "," Or Mid$(Trim(BMNos1), I, 1) = "." Or Mid$(Trim(BMNos1), I, 1) = "-" Or Mid$(Trim(BMNos1), I, 1) = "/" Or Mid$(Trim(BMNos1), I, 1) = "_" Or Mid$(Trim(BMNos1), I, 1) = "(" Or Mid$(Trim(BMNos1), I, 1) = ")" Or Mid$(Trim(BMNos1), I, 1) = "\" Or Mid$(Trim(BMNos1), I, 1) = "[" Or Mid$(Trim(BMNos1), I, 1) = "]" Or Mid$(Trim(BMNos1), I, 1) = "{" Or Mid$(Trim(BMNos1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos2 = Microsoft.VisualBasic.Right(Trim(BMNos1), Len(BMNos1) - I)
                BMNos1 = Microsoft.VisualBasic.Left(Trim(BMNos1), I)
            End If

            If Len(BMNos2) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos2), I, 1) = " " Or Mid$(Trim(BMNos2), I, 1) = "," Or Mid$(Trim(BMNos2), I, 1) = "." Or Mid$(Trim(BMNos2), I, 1) = "-" Or Mid$(Trim(BMNos2), I, 1) = "/" Or Mid$(Trim(BMNos2), I, 1) = "_" Or Mid$(Trim(BMNos2), I, 1) = "(" Or Mid$(Trim(BMNos2), I, 1) = ")" Or Mid$(Trim(BMNos2), I, 1) = "\" Or Mid$(Trim(BMNos2), I, 1) = "[" Or Mid$(Trim(BMNos2), I, 1) = "]" Or Mid$(Trim(BMNos2), I, 1) = "{" Or Mid$(Trim(BMNos2), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos3 = Microsoft.VisualBasic.Right(Trim(BMNos2), Len(BMNos2) - I)
                BMNos2 = Microsoft.VisualBasic.Left(Trim(BMNos2), I)
            End If

            If Len(BMNos3) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos3), I, 1) = " " Or Mid$(Trim(BMNos3), I, 1) = "," Or Mid$(Trim(BMNos3), I, 1) = "." Or Mid$(Trim(BMNos3), I, 1) = "-" Or Mid$(Trim(BMNos3), I, 1) = "/" Or Mid$(Trim(BMNos3), I, 1) = "_" Or Mid$(Trim(BMNos3), I, 1) = "(" Or Mid$(Trim(BMNos3), I, 1) = ")" Or Mid$(Trim(BMNos3), I, 1) = "\" Or Mid$(Trim(BMNos3), I, 1) = "[" Or Mid$(Trim(BMNos3), I, 1) = "]" Or Mid$(Trim(BMNos3), I, 1) = "{" Or Mid$(Trim(BMNos3), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos4 = Microsoft.VisualBasic.Right(Trim(BMNos3), Len(BMNos3) - I)
                BMNos3 = Microsoft.VisualBasic.Left(Trim(BMNos3), I)
            End If

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "«ýÒ¨¼Â£÷ , ", LMargin + 20, CurY, 0, 0, pFont)



            CurY = CurY + TxtHgt
            If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
                BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & " (" & BmsInWrds & ") empty beams "

                Common_Procedures.Print_To_PrintDocument(e, "­þôÀ×õ §Åý, Ä¡¡¢ ãÄõ " & Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & "(" & BmsInWrds & ")  ¸¡Ä¢ À£õ¸û அÛôÀ¢Ôû§Ç¡õ .", LMargin + 100, CurY, 0, 0, pFont)

            End If

            If Trim(BMNos1) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos1), LMargin + C1, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            'If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Then
            '    BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString))
            '    BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            '    SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) & " (" & BmsInWrds & ") empty bags "

            '    Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
            'End If
            'If Trim(BMNos2) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos2), LMargin + C1, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt + 5
            'If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
            '    BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_cones").ToString))
            '    BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            '    SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) & " (" & BmsInWrds & ") empty cones "


            '    Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
            'End If
            'If Trim(BMNos3) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos3), LMargin + C1, CurY, 0, 0, pFont)
            'End If
            CurY = CurY + TxtHgt + 5
            'If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Delivery To. : " & Trim(prn_HdDt.Rows(0).Item("Del_name").ToString), LMargin + 10, CurY, 0, 0, pFont)
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) & Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString), LMargin + 10, CurY, 0, 0, pFont)
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) & Trim(prn_HdDt.Rows(0).Item("Del_Address4").ToString), LMargin + 10, CurY, 0, 0, pFont)
            'End If



            CurY = CurY + TxtHgt + 5
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "ÅñÊ ±ñ  : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)
            End If
            If Trim(BMNos4) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos4), LMargin + C1, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "ÌÈ¢ôÒ  : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            End If

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "¦ÀüÚì¦¸¡ñ¼Å÷", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 325, CurY, 0, 0, pFont)
            p1Font = New Font("SaiIndira", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, LMargin + PageWidth - 30, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))



            If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_HdDt.Rows.Count = cnt + 6 Then
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


                e.HasMorePages = True
                Return

            End If

        End If
    End Sub

    Private Sub txt_Amount_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Amount.KeyDown
        If (e.KeyValue = 38) Then
            txt_Gst_Tax.Focus()
        End If

        If (e.KeyValue = 40) Then
            If cbo_EndsCount.Visible And cbo_EndsCount.Enabled Then
                cbo_EndsCount.Focus()
            Else
                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    save_record()
                'Else
                '    msk_date.Focus()
                'End If
                txt_remarks.Focus()
            End If
        End If

    End Sub

    Private Sub txt_Amount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Amount.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_EndsCount.Visible And cbo_EndsCount.Enabled Then
                cbo_EndsCount.Focus()
            Else
                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    save_record()
                'Else
                '    msk_date.Focus()
                'End If
                txt_remarks.Focus()

            End If
        End If
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
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomType_Creation, txt_BeamNos, "LoomType_Head", "LoomType_Name", "", "(LoomType_IdNo = 0)")

    End Sub


    Private Sub cbo_LoomType_Creation_GotFocus(sender As Object, e As EventArgs) Handles cbo_LoomType_Creation.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "LoomTYpe_Head", "LoomType_Name", "", "(LoomType_IdNo = 0)")
    End Sub

    Private Sub cbo_LoomType_Creation_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_LoomType_Creation.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomType_Creation, cbo_beamwidth, txt_BeamNos, "LoomTYpe_Head", "LoomTYpe_Name", "", "(LoomTYpe_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_LoomType_Creation.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            cbo_beamwidth.Focus()
        ElseIf (e.KeyValue = 40 And cbo_LoomType_Creation.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            txt_BeamNos.Focus()
        End If

    End Sub

    Private Sub txt_BeamNos_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_BeamNos.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If txt_emptyBobin.Enabled And txt_emptyBobin.Visible = True Then
                txt_emptyBobin.Focus()
            ElseIf cbo_jobcardno.Enabled And cbo_jobcardno.Visible = True Then
                cbo_jobcardno.Focus()
                Else
                    cbo_Transport.Focus()
            End If

        End If
    End Sub

    Private Sub txt_BeamNos_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_BeamNos.KeyDown
        If e.KeyCode = 38 Then

            If cbo_LoomType_Creation.Enabled And cbo_LoomType_Creation.Visible = True Then
                cbo_LoomType_Creation.Focus()
            Else
                cbo_beamwidth.Focus()
            End If

        ElseIf e.KeyCode = 40 Then
            If txt_emptyBobin.Enabled And txt_emptyBobin.Visible = True Then
                txt_emptyBobin.Focus()
            ElseIf cbo_jobcardno.Enabled And cbo_jobcardno.Visible = True Then
                cbo_jobcardno.Focus()
            Else
                cbo_Transport.Focus()
            End If
        End If
    End Sub



    Private Sub cbo_jobcardno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_jobcardno.KeyPress
        Dim Led_idno As Integer = 0

        If Trim(cbo_Partyname_DelvTo.Text) <> "" Then
            Led_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Partyname_DelvTo.Text)
        End If

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_jobcardno, cbo_Transport, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", "ledger_idno = " & Str(Val(Led_idno)) & "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_jobcardno_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_jobcardno.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_jobcardno, txt_BeamNos, cbo_Transport, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_jobcardno_GotFocus(sender As Object, e As EventArgs) Handles cbo_jobcardno.GotFocus
        Dim Led_idno As Integer = 0

        If Trim(cbo_Partyname_DelvTo.Text) <> "" Then
            Led_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Partyname_DelvTo.Text)
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", "ledger_idno = " & Str(Val(Led_idno)) & "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub txt_emptybags_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_emptybags.KeyDown

        If e.KeyCode = 38 Then
            'If cbo_jobcardno.Enabled And cbo_jobcardno.Visible = True Then
            '    cbo_jobcardno.Focus()
            'Else
            '    txt_BeamNos.Focus()
            'End If
            txt_Empty_Beam_Hsn.Focus()

        End If

        If e.KeyCode = 40 Then
            txt_Empty_Bag_Hsn.Focus()
        End If


    End Sub

    Private Sub txt_emptybeam_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_emptybeam.KeyDown
        If e.KeyCode = 40 Then
            txt_Empty_Beam_Hsn.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_Book_No.Focus()
        End If
    End Sub

    Private Sub txt_Empty_Beam_Hsn_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Empty_Beam_Hsn.KeyPress

        If Asc(e.KeyChar) = 13 Then
            txt_emptybags.Focus()
        End If


    End Sub

    Private Sub txt_Empty_Beam_Hsn_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Empty_Beam_Hsn.KeyDown
        If e.KeyCode = 40 Then
            txt_emptybags.Focus()
        End If


        If e.KeyCode = 38 Then
            txt_emptybeam.Focus()
        End If
    End Sub

    Private Sub txt_emptycones_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_emptycones.KeyDown
        If e.KeyCode = 40 Then
            txt_Empty_Cone_Hsn.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_Empty_Bag_Hsn.Focus()
        End If
    End Sub

    Private Sub txt_Empty_Cone_Hsn_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Empty_Cone_Hsn.KeyPress

        If Asc(e.KeyChar) = 13 Then
            cbo_beamwidth.Focus()
        End If

    End Sub

    Private Sub txt_Empty_Cone_Hsn_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Empty_Cone_Hsn.KeyDown
        If e.KeyCode = 40 Then
            cbo_beamwidth.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_emptycones.Focus()
        End If

    End Sub

    Private Sub txt_Empty_Bag_Hsn_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Empty_Bag_Hsn.KeyPress

        If Asc(e.KeyChar) = 13 Then
            txt_emptycones.Focus()
        End If

    End Sub

    Private Sub txt_Empty_Bag_Hsn_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Empty_Bag_Hsn.KeyDown
        If e.KeyCode = 40 Then
            txt_emptycones.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_emptybags.Focus()
        End If
    End Sub

    Private Sub txt_Gst_Tax_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Gst_Tax.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Beam_Rate.Focus()
            'txt_Amount.Focus()
        End If
    End Sub

    Private Sub txt_Gst_Tax_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Gst_Tax.KeyDown
        If e.KeyCode = 40 Then
            txt_Beam_Rate.Focus()
        End If

        If e.KeyCode = 38 Then
            cbo_vehicleno.Focus()
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


        If Val(txt_Beam_Rate.Text) = 0 Then
            MessageBox.Show("Invalid Beam Rate", "DOES NOT GENERATE EWB...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Beam_Rate.Enabled And txt_Beam_Rate.Visible Then txt_Beam_Rate.Focus()
            Exit Sub
        End If

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
                         " Inner Join Ledger_Head L ON a.Ledger_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on ( CASE WHEN a.DeliveryTo_IdNo <> 0 THEN a.DeliveryTo_IdNo ELSE a.Ledger_IdNo END ) = tDELV.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo " &
                          " where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
        CMD.ExecuteNonQuery()

        'vSgst = 

        'CMD.CommandText = " Update EWB_Head Set CGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , SGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        'CMD.ExecuteNonQuery()


        'CMD.CommandText = " Update EWB_Head Set TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        'CMD.ExecuteNonQuery()


        'CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        'CMD.ExecuteNonQuery()

        'Dim vPARTICULARS_FIELDNAME As String = ""
        'If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then
        '    vPARTICULARS_FIELDNAME = "(c.Count_Name )"
        'Else
        '    vPARTICULARS_FIELDNAME = "( I.Count_Name + ' - ' + IG.ItemGroup_Name )"
        'End If

        'Dim dt1 As New DataTable


        'da = New SqlClient.SqlDataAdapter(" Select  I.Count_Name, IG.ItemGroup_Name ,IG.Item_HSN_Code,( Case When Lh.Ledger_Type ='Weaver' and Lh.Ledger_GSTINNo <> '' Then (IG.Item_GST_Percentage ) else 0 end ) , sum(a.Amount) As TaxableAmt,sum(a.Total_Weight) as Qty, 1 , 'WGT' AS Units " &
        '                                  " from Weaver_Yarn_Delivery_Details SD Inner Join Empty_BeamBagCone_Delivery_Head a On a.Empty_BeamBagCone_Delivery_Code = sd.Empty_BeamBagCone_Delivery_Code Inner Join Count_Head I On SD.Count_IdNo = I.Count_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
        '                                  " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = a.DeliveryTo_IdNo INNER JOIN Company_Head tz On tz.Company_Idno = a.Company_Idno  Where SD.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
        '                                  " I.Count_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage,Lh.Ledger_Type, Lh.Ledger_GSTINNo", con)
        'dt1 = New DataTable
        'da.Fill(dt1)

        'For I = 0 To dt1.Rows.Count - 1

        '    CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                                ,[QuantityUnit] ,  Tax_Perc                         ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               ,InvCode) " &
        '                      " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",'KGS'          ," & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "')"

        '    CMD.ExecuteNonQuery()

        'Next

        'btn_GENERATEEWB.Enabled = False

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

        da = New SqlClient.SqlDataAdapter(" Select  1, a.Empty_Beam_HSN_Code  ,a.Empty_Beam as Qty , a.GST_Percentage  , sum(Empty_Beam * Beam_Rate) As TaxableAmt , tz.Company_State_IdNo , Lh.Ledger_State_Idno  ,a.GST_Tax_Invoice_Status  " &
                                          " from  Empty_BeamBagCone_Delivery_Head a  INNER Join Ledger_Head Lh ON Lh.Ledger_Idno =  a.Ledger_Idno  INNER JOIN Company_Head tz On tz.Company_Idno = a.Company_Idno  Where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
                                          " a.Empty_Beam_HSN_Code , a.Empty_Beam , a.GST_Percentage , tz.Company_State_IdNo , Lh.Ledger_State_Idno  ,a.GST_Tax_Invoice_Status  ", con)
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

            CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]     ,	[Product_Description]     ,         	[HSNCode]        ,          	[Quantity]             ,     [QuantityUnit] ,      Tax_Perc           ,	     [CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]          ,    InvCode      ,             Cgst_Value          ,             Sgst_Value          ,           Igst_Value) " &
                              " values                 (" & dt1.Rows(I).Item(0).ToString & ",    'EMPTY BEAM'     ,    'EMPTY BEAM'           , '" & dt1.Rows(I).Item(1) & "', " & dt1.Rows(I).Item(2).ToString & ",         'NOS'      , " & Val(vTax_Perc) & "  ,          0              ,           0       ," & dt1.Rows(I).Item(4) & " ,'" & NewCode & "',   '" & Str(Val(vCgst_Amt)) & "' ,   '" & Str(Val(vSgst_Amt)) & "' , '" & Str(Val(vIgst_AMt)) & "')"

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

    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 0)
    End Sub

    Private Sub btn_CancelEWB_1_Click(sender As Object, e As EventArgs) Handles btn_CancelEWB_1.Click
        'Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_EWBNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Empty_BeamBagCone_Delivery_Head", "EwayBill_No", "Empty_BeamBagCone_Delivery_Code")

    End Sub

    Private Sub btn_Close_EWB_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub
    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 1)
    End Sub

    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GetAuthToken(rtbEWBResponse)
    End Sub

    Private Sub txt_Beam_Rate_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Beam_Rate.KeyDown
        If e.KeyCode = 38 Then
            txt_Gst_Tax.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_Amount.Focus()

        End If
    End Sub

    Private Sub txt_Beam_Rate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Beam_Rate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Amount.Focus()
        End If
    End Sub
    Private Sub Amount_Calculation()
        Dim vEmpty_Bag_Rate As Integer = 0
        Dim vEmpty_Cone_Rate As Integer = 0
        Dim Tot_Amount As Integer = 0

        If Mov_Status = True Or NoCalc_Status = True Then Exit Sub

        If Val(txt_emptybeam.Text) <> 0 Then

            txt_Amount.Text = Format(Val(txt_emptybeam.Text) * Val(txt_Beam_Rate.Text), "############0.00")

        Else
            txt_Amount.Text = 0
        End If

    End Sub
    Private Sub txt_Beam_Rate_TextChanged(sender As Object, e As System.EventArgs) Handles txt_Beam_Rate.TextChanged
        Amount_Calculation()
    End Sub
    Private Sub txt_emptybeam_TextChanged(sender As Object, e As EventArgs) Handles txt_emptybeam.TextChanged
        Amount_Calculation()
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

    Private Sub txt_EmptyBobin_Party_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_EmptyBobin_Party.KeyDown
        If e.KeyCode = 38 Then
            txt_emptyBobin.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_JumpoEmpty.Focus()
        End If
    End Sub

    Private Sub txt_JumpoEmpty_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_JumpoEmpty.KeyDown
        If e.KeyCode = 38 Then
            txt_EmptyBobin_Party.Focus()
        ElseIf e.KeyCode = 40 Then
            cbo_vehicleno.Focus()
        End If
    End Sub
    Private Sub cbo_InvoiceSufixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DcSufixNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DcSufixNo, txt_DcPrefixNo, msk_date, "", "", "", "")
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DcSufixNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DcSufixNo, msk_date, "", "", "", "", False)
    End Sub

    Private Sub txt_InvoicePrefixNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_DcPrefixNo.KeyDown
        If e.KeyCode = 38 Then
            txt_remarks.Focus()
        ElseIf e.KeyCode = 40 Then
            cbo_DcSufixNo.Focus()
        End If
    End Sub

    Private Sub txt_InvoicePrefixNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_DcPrefixNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_DcSufixNo.Focus()
        End If
    End Sub

    Private Sub Printing_Format_1474(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim CurY1 As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim I As Integer
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0
        Dim BMNos1 As String, BMNos2 As String, BMNos3 As String, BMNos4 As String

        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0

        Dim vTxPerc As String = 0
        Dim vIgst_amt As String = 0
        Dim Cmp_Gstin_No As String

        Dim Sno As Integer = 0
        Dim vGST_BILL As Integer = 0

        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        PrntCnt = 1

        If vPrnt_2Copy_In_SinglePage = 1 Then
            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '    Debug.Print(ps.PaperName)
            '    If ps.Width = 800 And ps.Height = 600 Then
            '        PrintDocument1.DefaultPageSettings.PaperSize = ps
            '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '        e.PageSettings.PaperSize = ps
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next

            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If

        Else

            'If PpSzSTS = False Then
            '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '            PrintDocument1.DefaultPageSettings.PaperSize = ps
            '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
            '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '                e.PageSettings.PaperSize = ps
            '                Exit For
            '            End If
            '        Next
            '    End If

            ' End If
            set_PaperSize_For_PrintDocument1()

        End If



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 65
            .Right = 40 '30
            .Top = 30 '40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With



        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClArr(1) = Val(35) : ClArr(2) = 120 : ClArr(3) = 75 : ClArr(4) = 45 : ClArr(5) = 45 : ClArr(6) = 45 : ClArr(7) = 85
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        If Trim(Common_Procedures.settings.CustomerCode) = "1027" Then
            TxtHgt = 15.5 '15.5 '16 '16.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        Else
            TxtHgt = 16 '16.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

        For PCnt = 1 To PrntCnt

            If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then

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

            'NoofItems_PerPage = 5
            'If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then
            '    If prn_DetDt.Rows.Count > NoofItems_PerPage Then
            '        NoofItems_PerPage = 35
            '    End If
            'End If

            CurY = TpMargin
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
            Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Gstin_No = ""

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
            End If
            'If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            '    Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
            'End If
            If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
                Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
            End If

            If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
                Cmp_Gstin_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
            End If

            vGST_BILL = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)



            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            'If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            '    If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

            '        Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
            '        If Not imageData Is Nothing Then
            '            Using ms As New MemoryStream(imageData, 0, imageData.Length)
            '                ms.Write(imageData, 0, imageData.Length)

            '                If imageData.Length > 0 Then

            '                    '.BackgroundImage = Image.FromStream(ms)

            '                    ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
            '                    e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY + 5, 90, 90)

            '                End If

            '            End Using

            '        End If

            '    End If

            'End If

            CurY = CurY + strHeight - 10
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Gstin_No, LMargin + 10, CurY + 10, 0, 0, pFont)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Gstin_No, LMargin + 10, CurY + 5, 0, 0, pFont)

            p1Font = New Font("Calibri", 15, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM/BAG/CONE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "( Not For Sale )", LMargin + 30, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SAC : 99882 ( Textile Manufacture )", LMargin, CurY + 15, 2, PrintWidth, pFont)


            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            CurY = CurY + strHeight + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            CurY = CurY + TxtHgt - 13
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, p1Font)

            C1 = 450
            C2 = PageWidth - (LMargin + C1)

            W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 55, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString), LMargin + C1 + W1 + 65, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY1 = CurY
            CurY1 = CurY1 + 5
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 55, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 65, CurY1, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY1 = CurY
            CurY1 = CurY1 + 10
            If Trim(prn_HdDt.Rows(0).Item("EwayBill_No").ToString) <> "" Then

                Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL NO", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 55, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EwayBill_No").ToString, LMargin + C1 + W1 + 65, CurY1, 0, 0, pFont)

            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                CurY1 = CurY
                CurY1 = CurY1 + 15
                Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 55, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + W1 + 65, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Or Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString) <> 0 Then
                    CurY1 = CurY
                    CurY1 = CurY1 + 20
                    Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 40, CurY1, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, prn_HdDt.Rows(0).Item("Transport_IdNo").ToString), LMargin + C1 + W1 + 50, CurY1, 0, 0, pFont)
                End If

            End If


            If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "     " & "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + 10, CurY, 0, 0, pFont)

            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


            BMNos1 = ""
            If Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString) <> "" Then
                BMNos1 = Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString)
                'BMNos1 = "BEAM No.s  : " & Trim(prn_HdDt.Rows(0).Item("Beam_Nos").ToString)
            End If
            BMNos2 = ""
            BMNos3 = ""
            BMNos4 = ""
            If Len(BMNos1) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos1), I, 1) = " " Or Mid$(Trim(BMNos1), I, 1) = "," Or Mid$(Trim(BMNos1), I, 1) = "." Or Mid$(Trim(BMNos1), I, 1) = "-" Or Mid$(Trim(BMNos1), I, 1) = "/" Or Mid$(Trim(BMNos1), I, 1) = "_" Or Mid$(Trim(BMNos1), I, 1) = "(" Or Mid$(Trim(BMNos1), I, 1) = ")" Or Mid$(Trim(BMNos1), I, 1) = "\" Or Mid$(Trim(BMNos1), I, 1) = "[" Or Mid$(Trim(BMNos1), I, 1) = "]" Or Mid$(Trim(BMNos1), I, 1) = "{" Or Mid$(Trim(BMNos1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos2 = Microsoft.VisualBasic.Right(Trim(BMNos1), Len(BMNos1) - I)
                BMNos1 = Microsoft.VisualBasic.Left(Trim(BMNos1), I)
            End If

            If Len(BMNos2) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos2), I, 1) = " " Or Mid$(Trim(BMNos2), I, 1) = "," Or Mid$(Trim(BMNos2), I, 1) = "." Or Mid$(Trim(BMNos2), I, 1) = "-" Or Mid$(Trim(BMNos2), I, 1) = "/" Or Mid$(Trim(BMNos2), I, 1) = "_" Or Mid$(Trim(BMNos2), I, 1) = "(" Or Mid$(Trim(BMNos2), I, 1) = ")" Or Mid$(Trim(BMNos2), I, 1) = "\" Or Mid$(Trim(BMNos2), I, 1) = "[" Or Mid$(Trim(BMNos2), I, 1) = "]" Or Mid$(Trim(BMNos2), I, 1) = "{" Or Mid$(Trim(BMNos2), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos3 = Microsoft.VisualBasic.Right(Trim(BMNos2), Len(BMNos2) - I)
                BMNos2 = Microsoft.VisualBasic.Left(Trim(BMNos2), I)
            End If

            If Len(BMNos3) > 40 Then
                For I = 40 To 1 Step -1
                    If Mid$(Trim(BMNos3), I, 1) = " " Or Mid$(Trim(BMNos3), I, 1) = "," Or Mid$(Trim(BMNos3), I, 1) = "." Or Mid$(Trim(BMNos3), I, 1) = "-" Or Mid$(Trim(BMNos3), I, 1) = "/" Or Mid$(Trim(BMNos3), I, 1) = "_" Or Mid$(Trim(BMNos3), I, 1) = "(" Or Mid$(Trim(BMNos3), I, 1) = ")" Or Mid$(Trim(BMNos3), I, 1) = "\" Or Mid$(Trim(BMNos3), I, 1) = "[" Or Mid$(Trim(BMNos3), I, 1) = "]" Or Mid$(Trim(BMNos3), I, 1) = "{" Or Mid$(Trim(BMNos3), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 40
                BMNos4 = Microsoft.VisualBasic.Right(Trim(BMNos3), Len(BMNos3) - I)
                BMNos3 = Microsoft.VisualBasic.Left(Trim(BMNos3), I)
            End If

            'CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "We sent you", LMargin + 20, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "SL NO", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClArr(4) + ClArr(5) - 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "UOM", LMargin + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "BEAM WIDTH", LMargin + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "BEAM NOS", LMargin + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClArr(1), CurY, 2, ClArr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClArr(1) + ClArr(2), CurY, 2, ClArr(3), pFont)
            If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 40, CurY, 2, ClArr(4), pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "UOM", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 2, ClArr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY - 10, 2, ClArr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WIDTH", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY + 5, 2, ClArr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM NOS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, 2, ClArr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Dim vS1 As Single = 0
            Dim vS2 As Single = 0
            Dim vS3 As Single = 0
            Dim vS4 As Single = 0


            vS1 = ClArr(1) + ClArr(2)
            vS2 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4)
            vS3 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6)
            vS4 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7)

            Sno = 0

            For J As Integer = 0 To prn_HdDt.Rows.Count - 1



                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then

                    Sno = Sno + 1

                    Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Empty Beam", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Beam_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)

                    If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Beam").ToString, LMargin + vS2 + 10, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Beam").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, "NOS", LMargin + vS2 + ClArr(5) + 5, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Beam_type").ToString, LMargin + vS3 + 5, CurY, 0, 0, pFont)

                    If Trim(BMNos1) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos1), LMargin + vS4 + 5, CurY, 0, 0, pFont)
                    End If

                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Or Trim(BMNos2) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Then
                        Sno = Sno + 1

                        Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "Empty Bags", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bag_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)

                        If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bags").ToString, LMargin + vS2 + 10, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bags").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "NOS", LMargin + vS2 + ClArr(5) + 5, CurY, 0, 0, pFont)
                    End If

                    If Trim(BMNos2) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos2), LMargin + vS4 + 5, CurY, 0, 0, pFont)
                    End If
                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) <> 0 Or Trim(BMNos3) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) <> 0 Then
                        Sno = Sno + 1

                        Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "Empty Cones", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cone_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)

                        If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cones").ToString, LMargin + vS2 + 10, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cones").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "NOS", LMargin + vS2 + ClArr(5) + 5, CurY, 0, 0, pFont)
                    End If
                    If Trim(BMNos3) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos3), LMargin + vS4 + 5, CurY, 0, 0, pFont)
                    End If
                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bobin").ToString)) <> 0 Or Trim(BMNos4) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bobin").ToString)) <> 0 Then
                        Sno = Sno + 1

                        Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, "Empty Bobin", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cone_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)
                        If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bobin").ToString, LMargin + vS2 + 10, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Bobin").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "NOS", LMargin + vS2 + ClArr(5) + 5, CurY, 0, 0, pFont)
                    End If
                    If Trim(BMNos4) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BMNos4), LMargin + vS4 + 5, CurY, 0, 0, pFont)
                    End If
                End If

                If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Jumbo").ToString)) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    Sno = Sno + 1

                    Common_Procedures.Print_To_PrintDocument(e, Val(Sno), LMargin + 10, CurY, 0, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "Empty Jumbo", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Cone_HSN_Code").ToString, LMargin + vS1 + 5, CurY, 0, 0, pFont)
                    If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("GST_Percentage").ToString, LMargin + vS1 + ClArr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Jumbo").ToString, LMargin + vS2 + 10, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Jumbo").ToString, LMargin + vS2 + 5, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, "NOS", LMargin + vS2 + ClArr(5) + 5, CurY, 0, 0, pFont)
                End If


            Next J



            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + 5, LnAr(3), LMargin + ClArr(1) + 5, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2), LnAr(3), LMargin + ClArr(1) + ClArr(2), CurY)

            If prn_HdDt.Rows(0).Item("GST_Percentage").ToString <> 0 And Val(vGST_BILL) = 1 Then
                e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY)
                e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY)
            Else
                e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 30, LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 30, CurY)
            End If

            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY)

            '-------------------------*********************
            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            If Val(prn_HdDt.Rows(0).Item("Amount").ToString) <> 0 And Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1158" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Value            :  ", LMargin + C1 + 50, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + C1 + 160, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value    :  ", LMargin + C1 + 50, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + C1 + 160, CurY, 0, 0, pFont)
                End If
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods  :  ", LMargin + C1 + 50, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Amount").ToString), LMargin + C1 + 165, CurY, 0, 0, p1Font)
            End If



            If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO : ", LMargin + 10, CurY, 0, 0, p1Font)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_name").ToString), LMargin + ClArr(1) + 72, CurY, 0, 0, p1Font)
            End If
            '-------------------------*********************

            CurY = CurY + TxtHgt + 5
            vTxPerc = 0
            vCgst_amt = 0
            vSgst_amt = 0
            vIgst_amt = 0
            If Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

                If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then

                    vTxPerc = Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString) / 2, "############0.00")


                    vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(vTxPerc) / 100, "############0.00")
                    vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(vTxPerc) / 100, "############0.00")
                    '
                    'vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * 2.5 / 100, "############0.00")
                    'vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * 2.5 / 100, "############0.00")

                    Common_Procedures.Print_To_PrintDocument(e, " CGST " & Val(vTxPerc) & " % : " & vCgst_amt, LMargin + C1 + 47, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " SGST " & Val(vTxPerc) & " % : " & vSgst_amt, LMargin + C1 + 190, CurY, 0, 0, pFont)

                Else

                    vTxPerc = prn_HdDt.Rows(0).Item("GST_Percentage").ToString
                    vIgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                    Common_Procedures.Print_To_PrintDocument(e, " IGST " & Val(vTxPerc) & "% : " & vIgst_amt, LMargin + C1 + 47, CurY, 0, 0, pFont)

                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) & "  " & Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString), LMargin + 10, CurY - 3, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            '-------------------------*********************
            If Val(prn_HdDt.Rows(0).Item("Amount").ToString) <> 0 And Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then

                vTxamt = Val(vCgst_amt) + Val(vSgst_amt) + Val(vIgst_amt)
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) + vTxamt, "###########0.00")

                Common_Procedures.Print_To_PrintDocument(e, "Value of Goods : " & vNtAMt, LMargin + C1 + 50, CurY + 5, 0, 0, p1Font)

            End If
            '-------------------------*********************

            If Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Del_Address4").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) & "  " & Trim(prn_HdDt.Rows(0).Item("Del_Address4").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN  : " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If


            'If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            '    CurY = CurY + TxtHgt + 5
            '    Common_Procedures.Print_To_PrintDocument(e, "Through Vehicle No. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)
            'End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "REMARKS  : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            End If
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 20, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 325, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signature ", LMargin + PageWidth - 20, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))



            If Val(Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_HdDt.Rows.Count = cnt + 6 Then
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


                e.HasMorePages = True
                Return

            End If

        End If
    End Sub

End Class