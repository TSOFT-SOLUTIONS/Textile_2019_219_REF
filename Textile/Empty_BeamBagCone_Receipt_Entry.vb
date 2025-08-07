Imports System.Drawing.Printing
Imports System.IO
Public Class Empty_BeamBagCone_Receipt_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "EBREC-"
    Private Pk_Condition1 As String = "EBRFR-"
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
    Private Godown_Ledger_idno As Integer = 0
    Private Mov_Status As Boolean = False
    Private NoCalc_Status As Boolean = False


    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_filter.Visible = False
        pnl_back.Enabled = True
        chk_Verified_Status.Checked = False
        Mov_Status = False
        NoCalc_Status = True


        vmskOldText = ""
        vmskSelStrt = -1
        lbl_ReceiptNo.Text = ""
        lbl_ReceiptNo.ForeColor = Color.Black
        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_beamwidth.Text = ""
        cbo_vehicleno.Text = ""
        txt_remarks.Text = ""
        txt_emptybeam.Text = ""
        txt_emptybags.Text = ""
        txt_emptycones.Text = ""
        txt_PartyBobin.Text = ""
        txt_EmptyBobin.Text = ""
        txt_JumpoBobin.Text = ""
        txt_Freight_amount.Text = ""
        lbl_Delivery_Code.Text = ""
        cbo_Transport.Text = ""

        cbo_Type.Text = "DIRECT"

        Grp_EWB.Visible = False
        txt_remarks.Text = ""
        txt_Beam_Rate.Text = ""
        txt_EWBNo.Text = ""
        chk_Ewb_No_Sts.Checked = False
        chk_GSTTax_Invocie.Checked = True
        txt_Gst_Percentage.Text = ""
        txt_Hsn_Code.Text = ""
        txt_Amount.Text = ""
        rtbEWBResponse.Text = ""
        cbo_Beam_type.Text = ""


        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        chk_UNLOADEDBYOUREMPLOYEE.Checked = False
        cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
        txt_Book_No.Text = ""
        txt_Party_DcNo.Text = ""
        pnl_Selection.Visible = False

        Mov_Status = False
        NoCalc_Status = False

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
    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        If Val(no) = 0 Then Exit Sub

        clear()
        Mov_Status = True


        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Beam_Width_Name from Empty_BeamBagCone_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Beam_Width_Head c ON a.Beam_Width_IdNo = c.Beam_Width_IdNo where a.Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_ReceiptNo.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Receipt_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Receipt_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                txt_Party_DcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                txt_Book_No.Text = dt1.Rows(0).Item("Book_No").ToString
                cbo_beamwidth.Text = dt1.Rows(0).Item("Beam_Width_Name").ToString
                cbo_vehicleno.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                txt_emptybeam.Text = dt1.Rows(0).Item("Empty_Beam").ToString
                txt_emptybags.Text = dt1.Rows(0).Item("Empty_Bags").ToString
                txt_emptycones.Text = dt1.Rows(0).Item("Empty_Cones").ToString
                txt_EmptyBobin.Text = dt1.Rows(0).Item("Empty_Bobin").ToString
                txt_PartyBobin.Text = dt1.Rows(0).Item("EmptyBobin_Party").ToString
                txt_JumpoBobin.Text = dt1.Rows(0).Item("Empty_Jumbo").ToString
                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_Freight_amount.Text = dt1.Rows(0).Item("Freight_Amount").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Selection_type").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True
                lbl_Delivery_Code.Text = dt1.Rows(0).Item("Delivery_Code").ToString
                If Val(dt1.Rows(0)("Unloaded_By_Our_Employee").ToString) <> 0 Then
                    chk_UNLOADEDBYOUREMPLOYEE.Checked = True
                End If

                cbo_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString


                txt_Beam_Rate.Text = dt1.Rows(0).Item("Beam_Rate").ToString
                txt_Amount.Text = dt1.Rows(0).Item("Net_Amount").ToString
                txt_EWBNo.Text = dt1.Rows(0).Item("EwayBill_No").ToString
                If Trim(txt_EWBNo.Text) <> "" Then

                    chk_Ewb_No_Sts.Checked = True
                Else
                    chk_Ewb_No_Sts.Checked = False
                End If

                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False

                txt_Hsn_Code.Text = dt1.Rows(0).Item("HSN_Code").ToString
                txt_Gst_Percentage.Text = dt1.Rows(0).Item("GST_Percentage").ToString
                cbo_Beam_type.Text = Common_Procedures.LoomType_IdNoToName(con, Val(dt1.Rows(0).Item("LoomType_Idno").ToString))


            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        Mov_Status = False
        NoCalc_Status = False


        If cbo_PartyName.Visible And cbo_PartyName.Enabled Then cbo_PartyName.Focus()

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

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                If Prec_ActCtrl.Name = txt_Gst_Percentage.Name Or Prec_ActCtrl.Name = txt_Hsn_Code.Name Then
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


    Private Sub Empty_BeamBagCone_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_beamwidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_beamwidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Beam_type.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMTYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Beam_type.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
                    'MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.Close()
                    Exit Sub


                End If

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Empty_BeamBagCone_Receipt_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Empty_BeamBagCone_Receipt_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Me.Text = ""



        If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then '---- BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION) 


            chk_UNLOADEDBYOUREMPLOYEE.Visible = True



        End If
        con.Open()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)
            Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' ) order by Ledger_DisplayName", con)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
        Else
            Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Close_status = 0 order by Ledger_DisplayName", con)
        End If


        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            cbo_Type.Visible = True

            Lbl_type_caption.Visible = True
            btn_Selection.Visible = True

            Label2.Location = New Point(214, 27)

            Label23.Location = New Point(245, 27)
            msk_date.Location = New Point(270, 23)
            dtp_Date.Location = New Point(374, 23)
            lbl_ReceiptNo.Width = 94

        End If

        Da.Fill(Dt1)
        cbo_PartyName.DataSource = Dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        Da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from beam_Width_head order by Beam_Width_Name", con)
        Da.Fill(Dt2)
        cbo_beamwidth.DataSource = Dt2
        cbo_beamwidth.DisplayMember = "Beam_Width_Name"

        Da = New SqlClient.SqlDataAdapter("select vehicle_No from Empty_BeamBagCone_Receipt_Head order by Vehicle_No", con)
        Da.Fill(dt3)
        cbo_vehicleno.DataSource = dt3
        cbo_vehicleno.DisplayMember = "Vehicle_No"


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = (Me.Height - pnl_filter.Height) \ 2


        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()


        'Grp_EWB.Visible = False
        'Grp_EWB.Left = (Me.Width - Grp_EWB.Width) \ 2
        'Grp_EWB.Top = (Me.Height - Grp_EWB.Height) \ 2
        'Grp_EWB.BringToFront()

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

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("DELIVERY")
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_beamwidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vehicleno.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptybags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptybeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptycones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Party_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Book_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_JumpoBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_amount.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_UNLOADEDBYOUREMPLOYEE.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Beam_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Hsn_Code.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Gst_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Beam_type.GotFocus, AddressOf ControlGotFocus




        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_beamwidth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vehicleno.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptybags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptybeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptycones.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Party_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Book_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_JumpoBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_amount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Beam_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Hsn_Code.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Gst_Percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Beam_type.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_UNLOADEDBYOUREMPLOYEE.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_emptybags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_emptybeam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_emptycones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Party_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Book_No.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyBobin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyBobin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_JumpoBobin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight_amount.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_remarks.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_Beam_type.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Beam_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Hsn_Code.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Gst_Percentage.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Amount.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_emptybags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_emptybeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_emptycones.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Party_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Book_No.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyBobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_JumpoBobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight_amount.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler cbo_Beam_type.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_remarks.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Beam_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Hsn_Code.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Gst_Percentage.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Amount.KeyPress, AddressOf TextBoxControlKeyPress

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Empty_BeamBagCone_Receipt_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then
                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Delivery_Selection_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ReceiptNo.Text)

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Empty_BeamBagCone_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Empty_BeamBagCone_Receipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Empty_BeamBagCone_Receipt_Entry, New_Entry, Me, con, "Empty_BeamBagCone_Receipt_Head", "Empty_BeamBagCone_Receipt_Code", NewCode, "Empty_BeamBagCone_Receipt_Date", "(Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "Empty_BeamBagCone_Receipt_Head", "Verified_Status", "(Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Empty_BeamBagCone_Receipt_head", "Empty_BeamBagCone_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Empty_BeamBagCone_Receipt_Code, Company_IdNo, for_OrderBy", tr)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), tr)


            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            cmd.CommandText = "Delete from Empty_Beam_Selection_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

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


        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Empty_BeamBagCone_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Empty_BeamBagCone_Receipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Empty_BeamBagCone_Receipt_Entry, New_Entry, Me) = False Then Exit Sub




        Try

            inpno = InputBox("Enter New Receipt No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Empty_BeamBagCone_Receipt_No from Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'"
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
                    MessageBox.Show("Invalid Receipt No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_ReceiptNo.Text = Trim(UCase(inpno))

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
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Receipt_No from Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Empty_BeamBagCone_Receipt_No"
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
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Receipt_No from Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Empty_BeamBagCone_Receipt_No desc"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ReceiptNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Receipt_No from Empty_BeamBagCone_Receipt_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Empty_BeamBagCone_Receipt_No"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ReceiptNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Receipt_No from Empty_BeamBagCone_Receipt_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Empty_BeamBagCone_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Empty_BeamBagCone_Receipt_No desc"
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

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If


            dt.Dispose()
            'da.Dispose()

            NewID = NewID + 1

            lbl_ReceiptNo.Text = NewID
            lbl_ReceiptNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Empty_BeamBagCone_Receipt_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Empty_BeamBagCone_Receipt_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Receipt_Date").ToString
                End If
                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False

            End If
            dt1.Clear()



            If msk_date.Enabled And msk_date.Visible Then
                msk_date.Focus()
                msk_date.SelectionStart = 0
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt1.Dispose()
        da.Dispose()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Receipt No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Empty_BeamBagCone_Receipt_No from Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Receipt No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim led_id As Integer = 0
        Dim Bw_ID As Integer = 0
        Dim Partcls As String
        Dim PBlNo As String
        Dim EntID As String
        Dim Sub_Particulars As String = ""
        Dim Trans_ID As Integer = 0
        Dim Vchk_UNLOADED As Integer = 0

        Dim vSELC_DCCODE As String = ""
        Dim vOrdByNo As String = ""
        Dim Godown_Ledger_idno As Integer = 0
        Dim Ledger_Compidno As Integer = 0
        Dim Delv_ID As Integer = 0
        Dim vGST_Tax_Inv_Sts As Integer = 0

        Dim vLoomType_Idno As Integer = 0



        Dim Verified_STS As String = ""

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Empty_BeamBagCone_Receipt_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Empty_BeamBagCone_Receipt_Entry, New_Entry, Me, con, "Empty_BeamBagCone_Receipt_Head", "Empty_BeamBagCone_Receipt_Code", NewCode, "Empty_BeamBagCone_Receipt_Date", "(Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Empty_BeamBagCone_Receipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "Empty_BeamBagCone_Receipt_Head", "Verified_Status", "(Empty_BeamBagCone_Receipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled Then msk_date.Focus()
            Exit Sub
        End If


        Vchk_UNLOADED = 0
        If chk_UNLOADEDBYOUREMPLOYEE.Checked = True Then Vchk_UNLOADED = 1

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled Then msk_date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        Bw_ID = Common_Procedures.BeamWidth_NameToIdNo(con, cbo_beamwidth.Text)
        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1

        vLoomType_Idno = Common_Procedures.LoomType_NameToIdNo(con, cbo_Beam_type.Text)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1040" Then

            If Trim(txt_Party_DcNo.Text) <> "" Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                da = New SqlClient.SqlDataAdapter("select * from Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(led_id)) & " and Party_dcno = '" & Trim(txt_Party_DcNo.Text) & "' and Empty_BeamBagCone_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Empty_BeamBagCone_Receipt_Code <> '" & Trim(NewCode) & "'", con)
                dt1 = New DataTable
                da.Fill(dt1)
                If dt1.Rows.Count > 0 Then
                    MessageBox.Show("Duplicate Party Dc No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If txt_Party_DcNo.Enabled And txt_Party_DcNo.Visible Then txt_Party_DcNo.Focus()
                    Exit Sub
                End If
                dt1.Clear()
                dt1.Dispose()
            End If
        End If

        Delv_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        If Delv_ID = 0 Then Delv_ID = 4


        If Delv_ID = led_id Then
            MessageBox.Show("Invalid Party Name, Does not accept same name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_DeliveryTo.Enabled And cbo_DeliveryTo.Visible Then cbo_DeliveryTo.Focus()
            Exit Sub
        End If
        vSELC_DCCODE = ""
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(cbo_Type.Text) = "DELIVERY" Then
            vSELC_DCCODE = Trim(lbl_Delivery_Code.Text)
        End If
        vGST_Tax_Inv_Sts = 0
        If chk_GSTTax_Invocie.Checked = True Then vGST_Tax_Inv_Sts = 1

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
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
                da.Dispose()
                If Trim(NewNo) = "" Then NewNo = Trim(lbl_ReceiptNo.Text)

                lbl_ReceiptNo.Text = Trim(NewNo)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ReceiptDate", Convert.ToDateTime(msk_date.Text))

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ReceiptNo.Text)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Empty_BeamBagCone_Receipt_Head(Empty_BeamBagCone_Receipt_Code, Company_IdNo, Empty_BeamBagCone_Receipt_No, for_OrderBy,Empty_BeamBagCone_Receipt_Date, Ledger_IdNo,Party_DcNo,Book_No, Empty_Beam, Beam_Width_IdNo,Vehicle_No,Remarks , Empty_Bags , Empty_Cones , Empty_Bobin , EmptyBobin_Party , Empty_Jumbo  , user_idNo , Transport_IdNo , Freight_Amount ,Unloaded_By_Our_Employee ,Verified_Status,Delivery_Code,Selection_type,DeliveryTo_IdNo , Beam_Rate  , EwayBill_No , Net_Amount  ,GST_Tax_Invoice_Status , HSN_Code , GST_Percentage , LoomType_Idno) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @ReceiptDate, " & Val(led_id) & ",'" & Trim(txt_Party_DcNo.Text) & "','" & Trim(txt_Book_No.Text) & "', " & Val(txt_emptybeam.Text) & ", " & Str(Val(Bw_ID)) & ", '" & Trim(cbo_vehicleno.Text) & "','" & Trim(txt_remarks.Text) & "' , " & Val(txt_emptybags.Text) & ",  " & Val(txt_emptycones.Text) & " , " & Val(txt_EmptyBobin.Text) & " , " & Val(txt_PartyBobin.Text) & " , " & Val(txt_JumpoBobin.Text) & "," & Val(lbl_UserName.Text) & " ," & Val(Trans_ID) & "," & Val(txt_Freight_amount.Text) & "," & Val(Vchk_UNLOADED) & " , " & Val(Verified_STS) & ",'" & Trim(vSELC_DCCODE) & "','" & Trim(cbo_Type.Text) & "'," & Str(Val(Delv_ID)) & " ," & Val(txt_Beam_Rate.Text) & ",'" & Trim(txt_EWBNo.Text) & "'," & Val(txt_Amount.Text) & " , " & Str(Val(vGST_Tax_Inv_Sts)) & ",'" & Trim(txt_Hsn_Code.Text) & "' ," & Val(txt_Gst_Percentage.Text) & " , " & Str(Val(vLoomType_Idno)) & ")"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Empty_BeamBagCone_Receipt_head", "Empty_BeamBagCone_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Empty_BeamBagCone_Receipt_Code, Company_IdNo, for_OrderBy", tr)

                cmd.CommandText = "Update Empty_BeamBagCone_Receipt_Head set Empty_BeamBagCone_Receipt_Date = @ReceiptDate, Ledger_IdNo = " & Val(led_id) & ",Party_DcNo = '" & Trim(txt_Party_DcNo.Text) & "',Book_No = '" & Trim(txt_Book_No.Text) & "', Empty_Beam = " & Val(txt_emptybeam.Text) & ", Empty_Bags= " & Val(txt_emptybags.Text) & ",  Empty_Cones = " & Val(txt_emptycones.Text) & " , Empty_Bobin = " & Val(txt_EmptyBobin.Text) & " ,EmptyBobin_Party = " & Val(txt_PartyBobin.Text) & " ,Empty_Jumbo = " & Val(txt_JumpoBobin.Text) & " , Beam_Width_IdNo = " & Val(Bw_ID) & ", Vehicle_No = '" & Trim(cbo_vehicleno.Text) & "', Remarks = '" & Trim(txt_remarks.Text) & "' , User_idNo = " & Val(lbl_UserName.Text) & ",Transport_IdNo = " & Val(Trans_ID) & " ,Freight_amount = " & Val(txt_Freight_amount.Text) & ",Unloaded_By_Our_Employee=" & Val(Vchk_UNLOADED) & " ,Verified_Status= " & Val(Verified_STS) & " ,Delivery_Code='" & Trim(vSELC_DCCODE) & "',Selection_type='" & Trim(cbo_Type.Text) & "',DeliveryTo_IdNo=" & Str(Val(Delv_ID)) & " ,Beam_Rate =" & Val(txt_Beam_Rate.Text) & " , EwayBill_No = '" & Trim(txt_EWBNo.Text) & "'   , Net_Amount = " & Val(txt_Amount.Text) & " ,  GST_Tax_Invoice_Status = " & Str(Val(vGST_Tax_Inv_Sts)) & " ,Hsn_Code = '" & Trim(txt_Hsn_Code.Text) & "', GST_Percentage = " & Val(txt_Gst_Percentage.Text) & " , LoomType_Idno = " & Str(Val(vLoomType_Idno)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Empty_BeamBagCone_Receipt_head", "Empty_BeamBagCone_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Empty_BeamBagCone_Receipt_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Empty_Beam_Selection_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'Godown_Ledger_idno = Common_Procedures.get_FieldValue(con, "Company_Head", "Sizing_To_LedgerIdNo", "(COmpany_idno = " & Str(Val(lbl_Company.Tag)) & ")")
            EntID = Trim(Pk_Condition) & Trim(lbl_ReceiptNo.Text)
            Partcls = "Receipt : Rec.No. " & Trim(lbl_ReceiptNo.Text)
            PBlNo = Trim(lbl_ReceiptNo.Text)

            If Val(txt_emptybeam.Text) <> 0 Or Val(txt_emptybags.Text) <> 0 Or Val(txt_emptycones.Text) <> 0 Or Val(txt_EmptyBobin.Text) <> 0 Then


                Sub_Particulars = Sub_Particulars & " ("

                If Val(txt_emptybeam.Text) <> 0 Then
                    Sub_Particulars = Sub_Particulars & "Beam"
                End If
                If Val(txt_emptybags.Text) <> 0 Then
                    Sub_Particulars = Sub_Particulars & IIf(Sub_Particulars <> "", ",", "") & "Bag"
                End If
                If Val(txt_emptycones.Text) <> 0 Then
                    Sub_Particulars = Sub_Particulars & IIf(Sub_Particulars <> "", ",", "") & "Cone"
                End If
                If Val(txt_EmptyBobin.Text) <> 0 Then
                    Sub_Particulars = Sub_Particulars & IIf(Sub_Particulars <> "", ",", "") & "Bobin"
                End If
                Sub_Particulars = Sub_Particulars & ")"
            End If

            Partcls = Partcls & " " & Sub_Particulars
            If Val(txt_emptybeam.Text) <> 0 Or Val(txt_emptybags.Text) <> 0 Or Val(txt_emptycones.Text) <> 0 Or Val(txt_EmptyBobin.Text) <> 0 Or Val(txt_PartyBobin.Text) <> 0 Or Val(txt_JumpoBobin.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo                     , Reference_No                      ,            for_OrderBy    , Reference_Date, DeliveryTo_Idno                                           , ReceivedFrom_Idno       , Entry_ID             , Party_Bill_No        , Particulars            , Sl_No, Beam_Width_IdNo        , Empty_Beam                          , Empty_Bags                          , Empty_Cones                          , Empty_Bobin                          , EmptyBobin_Party                     , Empty_Jumbo       ,              LoomType_idno                     ) " &
                "Values                                    ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @ReceiptDate  , " & Str(Val(Delv_ID)) & ", " & Str(Val(led_id)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1    , " & Str(Val(Bw_ID)) & ", " & Str(Val(txt_emptybeam.Text)) & ", " & Str(Val(txt_emptybags.Text)) & ", " & Str(Val(txt_emptycones.Text)) & ", " & Str(Val(txt_EmptyBobin.Text)) & ", " & Str(Val(txt_PartyBobin.Text)) & ", " & Str(Val(txt_JumpoBobin.Text)) & " ,  " & Val(vLoomType_Idno) & " )"
                cmd.ExecuteNonQuery()
            End If

            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(cbo_Type.Text = "DELIVERY") And Trim(vSELC_DCCODE) <> "" Then
                If Val(txt_emptybeam.Text) <> 0 Or Val(txt_emptybags.Text) <> 0 Or Val(txt_emptycones.Text) <> 0 Or Val(txt_EmptyBobin.Text) <> 0 Or Val(txt_PartyBobin.Text) <> 0 Or Val(txt_JumpoBobin.Text) <> 0 Then
                    cmd.CommandText = "Insert into Empty_Beam_Selection_Processing_Details (                   Reference_Code           ,             Company_IdNo         ,               Reference_No        ,          for_OrderBy      , Reference_Date    ,    Delivery_Code                        ,     Delivery_No                        , DeliveryTo_Idno                                           , ReceivedFrom_Idno       , Party_Dc_No                           , Beam_Width_IdNo        , Empty_Beam                          , Empty_Bags                          , Empty_Cones                           ,    Selection_Ledgeridno       ,          Selection_CompanyIdno      ) " &
                                        "           Values                                 ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @ReceiptDate       ,   '" & Trim(vSELC_DCCODE) & "',     '" & Trim(txt_Party_DcNo.Text) & "', " & Str(Val(Delv_ID)) & ", " & Str(Val(led_id)) & ",  '" & Trim(txt_Party_DcNo.Text) & "'    , " & Str(Val(Bw_ID)) & ", " & Str(-1 * Val(txt_emptybeam.Text)) & ", " & Str(-1 * Val(txt_emptybags.Text)) & ", " & Str(-1 * Val(txt_emptycones.Text)) & " ," & Str(Val(led_id)) & "," & Str(Val(lbl_Company.Tag)) & ")"
                    cmd.ExecuteNonQuery()
                End If
            End If



            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), tr)

            vLed_IdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight_amount.Text) & "|" & -1 * Val(txt_Freight_amount.Text)
            If Common_Procedures.Voucher_Updation(con, "EmptyBm.Rcpt.Frgt", Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), Trim(lbl_ReceiptNo.Text), Convert.ToDateTime(dtp_Date.Text), Partcls, vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_ReceiptNo.Text)
                End If
            Else
                move_record(lbl_ReceiptNo.Text)
            End If


        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'Finally
            dt1.Dispose()
            '    'da.Dispose()
            cmd.Dispose()
            tr.Dispose()
        End Try

        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End If
    End Sub

    Private Sub cbo_partyname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_date, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)  or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        ElseIf Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, cbo_Type, cbo_DeliveryTo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_date, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        End If
    End Sub

    Private Sub cbo_partyname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_DeliveryTo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)  or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        Else

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN'  or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
            If Asc(e.KeyChar) = 13 Then
                If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(cbo_Type.Text = "DELIVERY") Then
                    btn_Selection_Click(sender, e)

                Else
                    cbo_DeliveryTo.Focus()

                End If
            End If
        End If
    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_beamwidth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_beamwidth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")

    End Sub

    Private Sub cbo_beamwidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_beamwidth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_beamwidth, txt_emptybeam, cbo_Beam_type, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")

    End Sub

    Private Sub cbo_beamwidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_beamwidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_beamwidth, cbo_Beam_type, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")
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



    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    'Private Sub txt_remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_remarks.KeyDown
    '    If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    '    If (e.KeyValue = 40) Then
    '        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
    '            save_record()
    '        Else
    '            msk_date.Focus()
    '        End If
    '    End If
    'End Sub

    Private Sub cbo_vehicleno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_vehicleno.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Empty_BeamBagCone_Receipt_Head", "Vehicle_No", "", "Vehicle_No")
    End Sub

    Private Sub cbo_vehicleno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vehicleno.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vehicleno, cbo_Transport, txt_Freight_amount, "Empty_BeamBagCone_Receipt_Head", "Vehicle_No", "", "Vehicle_No")
    End Sub

    'Private Sub txt_remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_remarks.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
    '            save_record()
    '        Else
    '            msk_date.Focus()
    '        End If
    '    End If
    'End Sub

    Private Sub txt_emptybeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptybeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub cbo_vehicleno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vehicleno.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vehicleno, txt_Freight_amount, "Empty_BeamBagCone_Receipt_Head", "Vehicle_No", "", "", False)

    End Sub
    Private Sub btn_print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub
    Private Sub btn_closefilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_closefilter.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False

    End Sub

    Private Sub btn_filtershow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
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
                Condt = "a.Empty_BeamBagCone_Receipt_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Empty_BeamBagCone_Receipt_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Empty_BeamBagCone_Receipt_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Empty_BeamBagCone_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Empty_BeamBagCone_Receipt_No", con)
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = " " & dt2.Rows(i).Item("Empty_BeamBagCone_Receipt_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Empty_BeamBagCone_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Empty_beam").ToString

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

    Private Sub dgv_filter_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellEndEdit
        SendKeys.Send("{UP}")
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub dgv_filter_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellEnter
        With dgv_filter

            If Val(.Rows(e.RowIndex).Cells(0).Value) = 0 Then
                .Rows(e.RowIndex).Cells(0).Value = e.RowIndex + 1
            End If
        End With
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Empty_BeamBagCone_Receipt_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Empty_BeamBagCone_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'", con)
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
        prn_TotCopies = 1
        Prnt_HalfSheet_STS = False

        vPrnt_2Copy_In_SinglePage = Common_Procedures.settings.EmptyBeamBagConeReceipt_Print_2Copy_In_SinglePage

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt = New DataTable
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Beam_Width_Name from Empty_BeamBagCone_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_IdNo = d.Beam_Width_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Format1(e)
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
        Dim Cmp_PhNo As String, Cmp_GSTNo As String, Cmp_UAMNO As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0

        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0

        Dim vTxPerc As String = 0
        Dim vIgst_amt As String = 0
        Dim Cmp_Gstin_No As String

        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        PrntCnt = 1

        '' If vPrnt_2Copy_In_SinglePage = 1 Then

        ''For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        ''    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        ''    Debug.Print(ps.PaperName)
        ''    If ps.Width = 800 And ps.Height = 600 Then
        ''        PrintDocument1.DefaultPageSettings.PaperSize = ps
        ''        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        ''        e.PageSettings.PaperSize = ps
        ''        PpSzSTS = True
        ''        Exit For
        ''    End If
        ''Next
        'set_PaperSize_For_PrintDocument1()

        If PrntCnt2ndPageSTS = False Then
            PrntCnt = 2
        End If
        'Else

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

        'End If
        set_PaperSize_For_PrintDocument1()


        ' End If



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 65
            .Right = 30
            .Top = 40
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

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        If Trim(Common_Procedures.settings.CustomerCode) = "1037" And vPrnt_2Copy_In_SinglePage = 1 Then
            PCnt = PCnt + 1
            PrntCnt = PrntCnt + 1
        End If

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

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

            TxtHgt = 18 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

            CurY = TpMargin
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
            Cmp_PhNo = "" : Cmp_GSTNo = "" : Cmp_UAMNO = ""

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
                Cmp_GSTNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_UAM_No").ToString) <> "" Then
                Cmp_UAMNO = "UDYAM No. : " & prn_HdDt.Rows(0).Item("Company_UAM_No").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

                If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                    Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)

                            If imageData.Length > 0 Then

                                '.BackgroundImage = Image.FromStream(ms)

                                ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                                e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 20, CurY + 5, 110, 100)

                            End If

                        End Using

                    End If

                End If

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
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo & "        " & Cmp_UAMNO, LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            CurY = CurY + strHeight + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "FROM : ", LMargin + 10, CurY, 0, 0, pFont)

            C1 = 450
            C2 = PageWidth - (LMargin + C1)

            W1 = e.Graphics.MeasureString("PARTY DC.NO : ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PARTY DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("EwayBill_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("EwayBill_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


            CurY = CurY + TxtHgt - 5

            ClArr(1) = Val(200) : ClArr(2) = 200
            ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))


            Common_Procedures.Print_To_PrintDocument(e, "BEAM WIDTH", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF BEAMS", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

            '---
            pFont = New Font("Calibri", 11, FontStyle.Regular)

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1158" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Value ", LMargin + C1 + 68, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ": ", LMargin + C1 + 115, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + C1 + 130, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + C1 + 70, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1 + 165, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, LMargin + C1 + 175, CurY, 0, 0, pFont)
                End If
            End If

            '--
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString, LMargin + 100 + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Beam").ToString, LMargin + 100 + ClArr(1) + 25, CurY, 0, 0, pFont)


            '----

            vTxPerc = 0
            vCgst_amt = 0
            vSgst_amt = 0
            vIgst_amt = 0
            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then
                If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then
                    vTxPerc = Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString) / 2, "############0.00")
                    vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")
                    vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Val(vTxPerc) & " %   :   " & vCgst_amt, LMargin + C1 + 70, CurY, 0, 0, pFont)


                Else

                    vTxPerc = Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "############0.00")
                    vIgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Val(vTxPerc) & " %    :   " & vIgst_amt, LMargin + C1 + 70, CurY + 5, 0, 0, pFont)

                End If
            End If

            '----
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

            '------

            If Val(vSgst_amt) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Val(vTxPerc) & " %   :   " & vSgst_amt, LMargin + C1 + 70, CurY, 0, 0, pFont)
            End If

            '-----

            CurY = CurY + TxtHgt

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            Common_Procedures.Print_To_PrintDocument(e, "We received your " & Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & "(" & BmsInWrds & ") empty beams", LMargin + 100, CurY, 0, 0, pFont)

            ' ---

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then
                vTxamt = Val(vCgst_amt) + Val(vSgst_amt) + Val(vIgst_amt)
                If Val(vIgst_amt) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Tax Amount   :   " & Format(Val(vTxamt), "########0.00"), LMargin + C1 + 70, CurY - 5, 0, 0, pFont)
                ElseIf Val(vCgst_amt) <> 0 And Val(vSgst_amt) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Tax Amount   :   " & Format(Val(vTxamt), "########0.00"), LMargin + C1 + 70, CurY, 0, 0, pFont)
                End If
            End If

            ' ---

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "through vehicle no. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)

            If Val(vTxamt) <> 0 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) + vTxamt, "###########0.00")
                Common_Procedures.Print_To_PrintDocument(e, "Net Amount   :   " & vNtAMt, LMargin + C1 + 70, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Signature of the receiver", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 350, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 20, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

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

    Private Sub txt_emptybags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptybags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_emptycones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptycones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_FilterTo_date, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            If cbo_Type.Visible = True Then
                cbo_Type.Focus()
            Else
                cbo_PartyName.Focus()



            End If
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

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_idno = 0)")
        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_JumpoBobin, cbo_vehicleno, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_vehicleno, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()
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

    Private Sub chk_UNLOADEDBYOUREMPLOYEE_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_UNLOADEDBYOUREMPLOYEE.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub chk_UNLOADEDBYOUREMPLOYEE_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_UNLOADEDBYOUREMPLOYEE.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
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

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")


            ' If Trim(AgPNo) <> "" Then
            PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", "", "")
            ' End If

            smstxt = smstxt & "Receipt No : " & Trim(lbl_ReceiptNo.Text) & vbCrLf
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
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_remarks_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_remarks.KeyDown
        If e.KeyValue = 38 Then

            txt_Freight_amount.Focus()


        End If
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_remarks_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
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
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0


        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        PrntCnt = 1

        '' If vPrnt_2Copy_In_SinglePage = 1 Then

        ''For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        ''    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        ''    Debug.Print(ps.PaperName)
        ''    If ps.Width = 800 And ps.Height = 600 Then
        ''        PrintDocument1.DefaultPageSettings.PaperSize = ps
        ''        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        ''        e.PageSettings.PaperSize = ps
        ''        PpSzSTS = True
        ''        Exit For
        ''    End If
        ''Next
        'set_PaperSize_For_PrintDocument1()

        If PrntCnt2ndPageSTS = False Then
            PrntCnt = 2
        End If
        'Else

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

        'End If
        set_PaperSize_For_PrintDocument1()


        ' End If



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 65
            .Right = 30
            .Top = 40
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

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        If Trim(Common_Procedures.settings.CustomerCode) = "1037" And vPrnt_2Copy_In_SinglePage = 1 Then
            PCnt = PCnt + 1
            PrntCnt = PrntCnt + 1
        End If

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

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

            TxtHgt = 18 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

            CurY = TpMargin
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
            Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Tamil_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Tamil_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_tamil_Address2").ToString 'prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
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

            CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            CurY = CurY + strHeight + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "FROM : ", LMargin + 10, CurY, 0, 0, pFont)

            C1 = 450
            C2 = PageWidth - (LMargin + C1)

            W1 = e.Graphics.MeasureString("PARTY DC.NO : ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PARTY DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


            CurY = CurY + TxtHgt - 5

            ClArr(1) = Val(200) : ClArr(2) = 200
            ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))


            Common_Procedures.Print_To_PrintDocument(e, "BEAM WIDTH", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF BEAMS", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString, LMargin + 100 + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Empty_Beam").ToString, LMargin + 100 + ClArr(1) + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            Common_Procedures.Print_To_PrintDocument(e, "­þôÀ×õ §Åý, ல¡¡¢ ãÄõ " & Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & "(" & BmsInWrds & ") empty beams", LMargin + 100, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "¸¡Ä¢ À£õ¸û அÛôÀ¢Ôû§Ç¡õ . " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Signature of the receiver", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 350, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 20, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

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
    Private Sub cbo_Type_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_date, cbo_PartyName, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_PartyName, "", "", "", "")

    End Sub

    Private Sub btn_Selection_Click(sender As Object, e As EventArgs) Handles btn_Selection.Click
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer, Ledger_Party_idno As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bls As Single = 0
        Dim Ent_BlNos As String = ""
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim Ent_ShtMtrs As Single = 0
        Dim Ent_Rate As Single = 0
        Dim vCOMP_IDNO As String
        Dim vCOMP_LEDIDNO1 As String, vCOMP_LEDIDNO2 As String
        Dim vCOMP_GODIDNO1 As String, vCOMP_GODIDNO2 As String, vCOMP_GODIDNO3 As String


        If Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then Exit Sub

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)


        'con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        'con.Open()

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.Selection_CompanyIdno = " & Str(Val(lbl_Company.Tag)) & ")"

        'vCOMP_IDNO = Common_Procedures.Ledger_IdNoToCompanyIdNo(con, Val(LedIdNo))

        'vCOMP_LEDIDNO1 = Common_Procedures.Company_IdnoToTextileLedgerIdNo(con, Val(vCOMP_IDNO))
        'vCOMP_LEDIDNO2 = Common_Procedures.Company_IdnoToSizingLedgerIdNo(con, Val(vCOMP_IDNO))

        'vCOMP_GODIDNO1 = Val(Common_Procedures.get_FieldValue(con, "Company_Head", "Company_WareHouse_idno_1", "(Company_IdNo = " & Str(Val(vCOMP_IDNO)) & ")"))
        'vCOMP_GODIDNO2 = Val(Common_Procedures.get_FieldValue(con, "Company_Head", "Company_WareHouse_idno_2", "(Company_IdNo = " & Str(Val(vCOMP_IDNO)) & ")"))
        'vCOMP_GODIDNO3 = Val(Common_Procedures.get_FieldValue(con, "Company_Head", "Company_WareHouse_idno_3", "(Company_IdNo = " & Str(Val(vCOMP_IDNO)) & ")"))


        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then


            With dgv_Selection


                lbl_Heading_Selection.Text = "DELIVERY SELECTION"

                .Rows.Clear()
                SNo = 0

                For i = 1 To 2

                    If i = 1 Then
                        '---editing
                        Da1 = New SqlClient.SqlDataAdapter("Select a.delivery_No,a.Delivery_Code, a.Empty_beam as Beams , a.Empty_bags as Bags, a.Empty_Cones as Cones from Empty_Beam_Selection_Processing_Details a Where ( a.Selection_ReceivedFromIdNo = " & Str(Val(LedIdNo)) & " OR a.Selection_ledgerIdno = " & Str(Val(LedIdNo)) & " ) and " & CompIDCondt & " and a.Delivery_Code = a.reference_code and a.Empty_Beam > 0 and a.Delivery_Code IN (Select sq1.Delivery_Code from Empty_Beam_Selection_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' ) ", con)
                    Else
                        'new entry
                        Da1 = New SqlClient.SqlDataAdapter("Select a.delivery_No,a.Delivery_Code , Sum(a.Empty_beam) as Beams , Sum(a.Empty_bags) as Bags, Sum(a.Empty_Cones) as Cones from Empty_Beam_Selection_Processing_Details a Where  ( a.Selection_ReceivedFromIdNo = " & Str(Val(LedIdNo)) & " OR a.Selection_ledgerIdno = " & Str(Val(LedIdNo)) & " ) and " & CompIDCondt & " and a.Delivery_Code NOT IN (Select sq1.Delivery_Code from Empty_Beam_Selection_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' ) Group by a.Delivery_Code, a.Delivery_No Having Sum(a.Empty_beam) > 0  ", con)
                        'Da1 = New SqlClient.SqlDataAdapter("Select a.delivery_No,a.Delivery_Code ,a.DeliveryTo_Idno, Sum(a.Empty_beam) as Beams , Sum(a.Empty_bags) as Bags, Sum(a.Empty_Cones) as Cones from Empty_Beam_Selection_Processing_Details a Where  ( ( a.Selection_ReceivedFromIdNo = " & Str(Val(vCOMP_LEDIDNO1)) & " or a.Selection_ReceivedFromIdNo = " & Str(Val(vCOMP_LEDIDNO2)) & "or a.Selection_ReceivedFromIdNo = " & Str(Val(vCOMP_GODIDNO1)) & " or a.Selection_ReceivedFromIdNo = " & Str(Val(vCOMP_GODIDNO2)) & "or a.Selection_ReceivedFromIdNo = " & Str(Val(vCOMP_GODIDNO3)) & " ) OR ( a.Selection_ledgerIdno = " & Str(Val(vCOMP_LEDIDNO1)) & " or a.Selection_ledgerIdno = " & Str(Val(vCOMP_LEDIDNO2)) & "or a.Selection_ledgerIdno = " & Str(Val(vCOMP_GODIDNO1)) & " or a.Selection_ledgerIdno = " & Str(Val(vCOMP_GODIDNO2)) & " or a.Selection_ledgerIdno =" & Str(Val(vCOMP_GODIDNO3)) & " ) ) and " & CompIDCondt & " and a.Delivery_Code NOT IN (Select sq1.Delivery_Code from Empty_Beam_Selection_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' ) Group by a.Delivery_Code, a.Delivery_No,a.DeliveryTo_Idno Having Sum(a.Empty_beam) > 0  ", con)
                        ''Da1 = New SqlClient.SqlDataAdapter("Select a.delivery_No,a.Delivery_Code ,a.DeliveryTo_Idno, Sum(a.Empty_beam) as Beams , Sum(a.Empty_bags) as Bags, Sum(a.Empty_Cones) as Cones from Empty_Beam_Selection_Processing_Details a left outer join company_head ch on ch.textile_unit_LedgerIDno =" & Str(Val(LedIdNo)) & " where   ( a.Selection_ledgerIdno =" & Str(Val(vCOMP_GODIDNO1)) & " or a.Selection_ledgerIdno =" & Str(Val(vCOMP_GODIDNO3)) & "or a.Selection_ledgerIdno =" & Str(Val(vCOMP_GODIDNO3)) & " )  and " & CompIDCondt & " and a.Delivery_Code NOT IN (Select sq1.Delivery_Code from Empty_Beam_Selection_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' ) Group by a.Delivery_Code, a.Delivery_No,a.DeliveryTo_Idno Having Sum(a.Empty_beam) > 0  ", con)
                    End If


                    Dt1 = New DataTable


                    Da1.Fill(Dt1)

                    If Dt1.Rows.Count > 0 Then

                        For k = 0 To Dt1.Rows.Count - 1

                            If Val(Dt1.Rows(k).Item("Beams").ToString) > 0 Then

                                SNo = SNo + 1
                                n = .Rows.Add()


                                .Rows(n).Cells(0).Value = Val(SNo)
                                .Rows(n).Cells(1).Value = Dt1.Rows(k).Item("Delivery_No").ToString
                                '.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy")
                                .Rows(n).Cells(3).Value = Dt1.Rows(k).Item("Delivery_No").ToString
                                .Rows(n).Cells(4).Value = Dt1.Rows(k).Item("Beams").ToString
                                .Rows(n).Cells(5).Value = Dt1.Rows(k).Item("Bags").ToString
                                .Rows(n).Cells(6).Value = Dt1.Rows(k).Item("Cones").ToString
                                .Rows(n).Cells(8).Value = Dt1.Rows(k).Item("Delivery_Code").ToString
                                .Rows(n).Cells(9).Value = 0 'Dt1.Rows(k).Item("DeliveryAt_Idno").ToString

                                If i = 1 Then

                                    .Rows(n).Cells(7).Value = 1
                                    'For j = 0 To .ColumnCount - 1
                                    '    .Rows(k).Cells(j).Style.ForeColor = Color.Red
                                    'Next

                                Else
                                    .Rows(n).Cells(7).Value = ""
                                    'For j = 0 To .ColumnCount - 1
                                    '    .Rows(k).Cells(j).Style.ForeColor = Color.Black
                                    'Next

                                End If

                            End If

                        Next

                    End If
                    Dt1.Clear()

                Next

            End With

        End If
        pnl_Selection.Visible = True
        pnl_back.Enabled = False
        dgv_Selection.Focus()
    End Sub

    Private Sub dgv_Selection_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Dc(e.RowIndex)
    End Sub



    Private Sub Select_Dc(ByVal RwIndx As Integer)
        Dim i As Integer


        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(7).Value = ""
                Next

                .Rows(RwIndx).Cells(7).Value = 1

                If Val(.Rows(RwIndx).Cells(7).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(7).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If



            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(sender As Object, e As KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next
        If IsNothing(dgv_Selection.CurrentCell) Then Exit Sub
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                e.Handled = True
                Select_Dc(dgv_Selection.CurrentCell.RowIndex)
                btn_Close_Delivery_Selection_Click(sender, e)
            End If
        End If
    End Sub

    Private Sub btn_Close_Delivery_Selection_Click(sender As Object, e As EventArgs) Handles btn_Close_Delivery_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, n As Integer
        Dim sno As Integer = 0
        Dim Clo_IdNo As Integer = 0


        sno = 0
        Clo_IdNo = 0

        For i = 0 To dgv_Selection.RowCount - 1


            If Val(dgv_Selection.Rows(i).Cells(7).Value) = 1 Then

                txt_Party_DcNo.Text = Trim(dgv_Selection.Rows(i).Cells(1).Value)
                txt_emptybeam.Text = Val(dgv_Selection.Rows(i).Cells(4).Value)
                txt_emptycones.Text = Val(dgv_Selection.Rows(i).Cells(5).Value)
                txt_emptybags.Text = Val(dgv_Selection.Rows(i).Cells(6).Value)

                lbl_Delivery_Code.Text = Trim(dgv_Selection.Rows(i).Cells(8).Value)

                'cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dgv_Selection.Rows(i).Cells(9).Value))

                Da = New SqlClient.SqlDataAdapter("Select isnull(b.ledger_name,'') as deliveryatname from Empty_Beam_Selection_Processing_Details a LEFT OUTER JOIN ledger_head b ON  b.ledger_idno = a.DeliveryAt_Idno Where a.Reference_Code = '" & Trim(Trim(dgv_Selection.Rows(i).Cells(8).Value)) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If Trim(Dt1.Rows(0).Item("deliveryatname").ToString) <> "" Then
                        cbo_DeliveryTo.Text = Dt1.Rows(0).Item("deliveryatname").ToString
                    End If
                End If
                Dt1.Clear()

                Exit For

            End If

        Next

        Dt1.Dispose()
        Da.Dispose()

        pnl_back.Enabled = True
        pnl_Selection.Visible = False
        txt_Party_DcNo.Focus()
    End Sub

    Private Sub dgv_Selection_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgv_Selection.CellMouseClick
        btn_Close_Delivery_Selection_Click(sender, e)

    End Sub

    Private Sub cbo_Type_TextChanged(sender As Object, e As EventArgs) Handles cbo_Type.TextChanged
        If Trim(cbo_Type.Text) = "DELIVERY" Then
            txt_emptybeam.Enabled = False
            cbo_beamwidth.Enabled = False
        Else
            txt_emptybeam.Enabled = True
            cbo_beamwidth.Enabled = True
        End If
    End Sub

    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_PartyName, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click
        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        'Grp_EWB.Left = (Me.Width - pnl_back.Width) / 2
        'Grp_EWB.Top = (Me.Height - pnl_back.Height) / 2

        Grp_EWB.Location = New Point(111, 234)

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
        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select EwayBill_No from Empty_BeamBagCone_Receipt_Head where Empty_BeamBagCone_Receipt_code = '" & NewCode & "'", con)
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
                         "  SELECT               'I'              , '6'             ,   'JOB WORK RETURNS'        ,    'CHL'    , a.Empty_BeamBagCone_Receipt_No , a.Empty_BeamBagCone_Receipt_DAte     , L.Ledger_GSTINNo, L.Ledger_MainName   , L.Ledger_Address1 +  L.Ledger_Address2 , L.Ledger_Address3 + L.Ledger_Address4 , L.City_Town ," &
                         " L.PinCode     , TS.State_Code  ,TS.State_Code    , C.Company_GSTINNo, C.Company_Name , (case when a.DeliveryTo_IdNo = 4 then (C.Company_Address1+C.Company_Address2) when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else C.Company_Address1+C.Company_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo = 4 then (c.Company_Address3+C.Company_Address4) when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  c.Company_Address3+C.Company_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo = 4 then (c.Company_City) when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else c.Company_City end) as city_town_name, (case when a.DeliveryTo_IdNo = 4 then (c.Company_PinCode) when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  c.Company_PinCode end) as pincodee,(case when a.DeliveryTo_IdNo = 4 then (FS.State_Code) when a.DeliveryTo_IdNo <> 0 then TDCS.State_Code ELSE  FS.State_Code END ),  (case when a.DeliveryTo_IdNo = 4 then (FS.State_Code) when a.DeliveryTo_IdNo <> 0 then TDCS.State_Code ELSE  FS.State_Code END   )  as actual_StateCode , " &
                         " 1                     , 0 , a.Net_Amount     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
                         " a.Vehicle_No, 'R', '" & NewCode & "', (case when a.DeliveryTo_IdNo = 4 or a.DeliveryTo_IdNo = 0 then  c.Company_GSTINNo else tDELV.Ledger_GSTINNo end ) as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName  from Empty_BeamBagCone_Receipt_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L ON a.Ledger_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo   left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head  TDCS ON tDELV.Ledger_State_IdNo = TDCS.State_IdNo  " &
                         " where a.Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'"
        CMD.ExecuteNonQuery()



        'vSgst = 

        'CMD.CommandText = " Update EWB_Head Set CGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , SGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        'CMD.ExecuteNonQuery()


        'CMD.CommandText = " Update EWB_Head Set TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        'CMD.ExecuteNonQuery()

        Dim vCgst_Amt As String = 0
        Dim vSgst_Amt As String = 0
        Dim vIgst_AMt As String = 0
        Dim vTax_Perc As String = 0

        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        Dim dt1 As New DataTable

        da = New SqlClient.SqlDataAdapter(" Select  1, a.HSN_Code,a.Empty_Beam as Qty , a.GST_Percentage  , sum(Empty_Beam * Beam_Rate) As TaxableAmt , tz.Company_State_IdNo , Lh.Ledger_State_Idno  ,a.GST_Tax_Invoice_Status  " &
                                          " from  Empty_BeamBagCone_Receipt_Head a  INNER Join Ledger_Head Lh ON Lh.Ledger_Idno =  a.Ledger_Idno  INNER JOIN Company_Head tz On tz.Company_Idno = a.Company_Idno  Where a.Empty_BeamBagCone_Receipt_code = '" & Trim(NewCode) & "' Group By " &
                                          " a.HSN_Code , a.Empty_Beam , a.GST_Percentage , tz.Company_State_IdNo , Lh.Ledger_State_Idno  ,a.GST_Tax_Invoice_Status  ", con)
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

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Empty_BeamBagCone_Receipt_Head", "EwayBill_No", "Empty_BeamBagCone_Receipt_code", Pk_Condition)


    End Sub
    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 0)
    End Sub

    Private Sub btn_CancelEWB_1_Click(sender As Object, e As EventArgs) Handles btn_CancelEWB_1.Click
        'Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_EWBNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Empty_BeamBagCone_Receipt_Head", "EwayBill_No", "Empty_BeamBagCone_Receipt_code")

    End Sub

    Private Sub btn_Close_EWB_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub
    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 1)
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

    'Private Sub txt_rate_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Beam_Rate.KeyDown
    '    If e.KeyCode = 38 Then
    '        txt_Freight_amount.Focus()
    '    ElseIf e.KeyCode = 40 Then

    '        txt_remarks.Focus()

    '    End If
    'End Sub

    'Private Sub txt_rate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Beam_Rate.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        txt_remarks.Focus()

    '    End If
    'End Sub
    Private Sub txt_rate_TextChanged(sender As Object, e As System.EventArgs) Handles txt_Beam_Rate.TextChanged
        Amount_Calculation()
    End Sub
    Private Sub txt_emptybeam_TextChanged(sender As Object, e As EventArgs) Handles txt_emptybeam.TextChanged
        Amount_Calculation()
    End Sub
    Private Sub Amount_Calculation()
        Dim vEmpty_Bag_Rate As Integer = 0
        Dim vEmpty_Cone_Rate As Integer = 0

        If Mov_Status = True Or NoCalc_Status = True Then Exit Sub

        If Val(txt_emptybeam.Text) <> 0 Then

            txt_Amount.Text = Format(Val(txt_emptybeam.Text) * Val(txt_Beam_Rate.Text), "############0.00")
        Else
            txt_Amount.Text = 0
        End If

    End Sub
    Private Sub cbo_Beam_type_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Beam_type.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LoomType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Beam_type.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If


    End Sub

    Private Sub cbo_Beam_type_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Beam_type.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Beam_type, txt_emptybags, "LoomType_Head", "LoomType_Name", "", "(LoomType_IdNo = 0)")

    End Sub

    Private Sub cbo_Beam_type_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Beam_type.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Beam_type, cbo_beamwidth, txt_emptybags, "LoomTYpe_Head", "LoomTYpe_Name", "", "(LoomTYpe_IdNo = 0)")

    End Sub

    Private Sub cbo_Beam_type_GotFocus(sender As Object, e As EventArgs) Handles cbo_Beam_type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "LoomTYpe_Head", "LoomType_Name", "", "(LoomType_IdNo = 0)")
    End Sub

End Class