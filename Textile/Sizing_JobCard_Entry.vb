Public Class Sizing_JobCard_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "SZJOB-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private dgv_ActiveCtrl_Name As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Count As Integer = 0


    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_Prev_HeadIndx As Integer
    Private prn_HeadIndx As Integer
    Private vPrnt_2Copy_In_SinglePage As Integer = 0
    Private prn_TotCopies As Integer = 0
    Private Print_PDF_Status As Boolean = False
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Dim vDGV_ENTRCEL_COUNTNAME As String = ""
    Dim vDGV_ENTRCEL_MILLNAME As String = ""
    Dim vDGV_ENTRCEL_YARNTYPE As String = ""

    Private Sub clear()

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_YarnStock_Display.Visible = False


        lbl_JobNo.Text = ""
        lbl_JobNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        msk_Beam_Requirement_Date.Text = ""
        txt_DBF.Text = ""
        txt_WarpLegnth.Text = ""

        cbo_SizingName.Text = ""

        cbo_Loom_Type.Text = ""
        txt_Ends_1.Text = ""
        lbl_JobNo.Text = ""

        txt_Remarks.Text = ""
        txt_fabric_Weave.Text = ""
        txt_Fabric_Width.Text = ""
        txt_Beam_Width.Text = ""
        txt_sizing_length.Text = ""
        txt_InvoiceCode.Text = ""
        txt_BabyCone_DeliveryWeight.Text = ""
        txt_no_of_creel.Text = ""
        txt_beam_length.Text = ""
        txt_no_of_beams.Text = ""
        txt_elongation.Text = ""
        txt_Pickup_1.Text = ""
        txt_pickup_2.Text = ""
        dtp_beam_date.Text = ""

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
        cbo_Grid_Yarn_LotNo.Visible = False


        cbo_Grid_CountName.Tag = -1
        cbo_Grid_MillName.Tag = -1
        cbo_Grid_YarnType.Tag = -1
        cbo_Grid_Yarn_LotNo.Tag = -1



        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""
        cbo_Grid_YarnType.Text = ""
        cbo_Grid_Yarn_LotNo.Text = ""
        txt_No_of_set.Text = ""


        'cbo_Loom_Type.Enabled = True
        'cbo_Loom_Type.BackColor = Color.White

        'cbo_SizingName.Enabled = True
        'cbo_SizingName.BackColor = Color.White


        txt_InvoicePrefixNo.Text = ""
        cbo_InvoiceSufixNo.Text = ""
        cbo_Kind_Attn.Text = ""
        txt_no_of_weaver_beam.Text = ""
        cbo_Fabric_Name.Text = ""

        cbo_Loom_Type.Text = ""
        txt_Ends_2.Text = ""

        dgv_YarnStock.Rows.Clear()

        dgv_YarnDetails.Rows.Clear()
        dgv_YarnDetails.Rows(0).Cells(2).Value = "MILL"

        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False

        cbo_Loom_Type.Tag = ""
        cbo_SizingName.Tag = ""

        Grid_Cell_DeSelect()
        dgv_ActiveCtrl_Name = ""


        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_MillName.Name Then
            cbo_Grid_MillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_YarnType.Name Then
            cbo_Grid_YarnType.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Yarn_LotNo.Name Then
            cbo_Grid_Yarn_LotNo.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name And Me.ActiveControl.Name <> cbo_Grid_MillName.Name And Me.ActiveControl.Name <> cbo_Grid_YarnType.Name And Me.ActiveControl.Name <> cbo_Grid_Yarn_LotNo.Name And Me.ActiveControl.Name <> dgv_YarnDetails.Name And Me.ActiveControl.Name <> dgtxt_Details.Name Then
            pnl_YarnStock_Display.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
        'If Not IsNothing(dgv_YarnDetails_Total.CurrentCell) Then dgv_YarnDetails_Total.CurrentCell.Selected = False
        'If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as SizingName from Sizing_JobCard_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo   Where a.Sizing_JobCard_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)
            'lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

            If dt1.Rows.Count > 0 Then
                txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Sizing_JobCard_PrefixNo").ToString
                cbo_InvoiceSufixNo.Text = dt1.Rows(0).Item("Sizing_JobCard_SuffixNo").ToString
                lbl_JobNo.Text = dt1.Rows(0).Item("Sizing_JobCard_RefNo").ToString
                'lbl_JobNo.Text = dt1.Rows(0).Item("Sizing_JobCard_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sizing_JobCard_Date").ToString
                cbo_SizingName.Text = dt1.Rows(0).Item("SizingName").ToString
                cbo_Kind_Attn.Text = dt1.Rows(0).Item("Kind_Attention_Person_Name").ToString

                cbo_Loom_Type.Text = Common_Procedures.LoomType_IdNoToName(con, Val(dt1.Rows(0).Item("Loom_Type_idno").ToString))
                txt_fabric_Weave.Text = dt1.Rows(0).Item("Fabric_Weave").ToString
                txt_Ends_1.Text = dt1.Rows(0).Item("ends_name").ToString
                txt_no_of_creel.Text = dt1.Rows(0).Item("creel").ToString
                txt_Fabric_Width.Text = dt1.Rows(0).Item("Fabric_Width").ToString
                txt_DBF.Text = dt1.Rows(0).Item("DBF").ToString
                txt_WarpLegnth.Text = dt1.Rows(0).Item("Wrap_Length").ToString
                txt_beam_length.Text = dt1.Rows(0).Item("Beam_Length").ToString
                txt_sizing_length.Text = Format(Val(dt1.Rows(0).Item("Sizing_Length").ToString), "########0.00")
                txt_no_of_beams.Text = Format(Val(dt1.Rows(0).Item("No_Of_Beams").ToString), "########0.00")
                txt_Pickup_1.Text = Val(dt1.Rows(0).Item("Pick_Up").ToString)
                txt_elongation.Text = Format(Val(dt1.Rows(0).Item("Elongation").ToString), "#########0.00")
                dtp_beam_date.Text = dt1.Rows(0).Item("Beam_Requirement_Date").ToString
                msk_Beam_Requirement_Date.Text = dtp_beam_date.Text
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                txt_pickup_2.Text = Val(dt1.Rows(0).Item("Pick_Up_2").ToString)
                txt_No_of_set.Text = Val(dt1.Rows(0).Item("no_of_set").ToString)

                cbo_Fabric_Name.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_idno").ToString))
                txt_no_of_weaver_beam.Text = dt1.Rows(0).Item("No_Of_Weaver_beam").ToString

                lbl_no_of_cones_required.Text = Format(Val(dt1.Rows(0).Item("no_of_cones_required").ToString), "#########0.00")

                txt_Beam_Width.Text = dt1.Rows(0).Item("Beam_Width").ToString
                txt_Ends_2.Text = dt1.Rows(0).Item("Ends_Name_2").ToString



                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Sizing_JobCard_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Sizing_JobCard_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_YarnDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        n = dgv_YarnDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_YarnDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_YarnDetails.Rows(n).Cells(1).Value = dt2.Rows(I).Item("Count_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(2).Value = dt2.Rows(I).Item("Yarn_Type").ToString
                        dgv_YarnDetails.Rows(n).Cells(3).Value = dt2.Rows(I).Item("Mill_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(4).Value = Common_Procedures.YarnLotEntryReferenceCode_to_LotCodeSelection(con, dt2.Rows(I).Item("Lot_Entry_ReferenceCode").ToString)
                        dgv_YarnDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(I).Item("Weight_Bag").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(I).Item("Cones_Bag").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(I).Item("Weight_Per_Cone").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(I).Item("No_Of_Bags").ToString), "########0.0")
                        dgv_YarnDetails.Rows(n).Cells(9).Value = Val(dt2.Rows(I).Item("No_Of_Cones").ToString)
                        dgv_YarnDetails.Rows(n).Cells(10).Value = Val(dt2.Rows(I).Item("Loose_Cones").ToString)
                        dgv_YarnDetails.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(I).Item("Weight").ToString), "########0.000")

                    Next I

                End If

                dt2.Clear()

                Total_Calculation()

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_SizingName.Visible And cbo_SizingName.Enabled Then cbo_SizingName.Focus()

    End Sub

    Private Sub Job_Card_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SizingName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SizingName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Loom_Type.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMTYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Loom_Type.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Job_Card_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
            cbo_Kind_Attn.Visible = True
            lbl_Kind_Attn.Visible = True

        Else

            lbl_Kind_Attn.Visible = False
            cbo_Kind_Attn.Visible = False

        End If

        txt_Ends_2.Visible = False
        lbl_Ends_2.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1266" Then
            txt_Ends_2.Visible = True
            lbl_Ends_2.Visible = True
        Else
            txt_Ends_1.Left = lbl_Ends_2.Left
            txt_Ends_1.Width = txt_Fabric_Width.Width
        End If


        pnl_YarnStock_Display.Visible = False
        pnl_YarnStock_Display.Left = lbl_LoomType_Caption.Left + 10
        pnl_YarnStock_Display.Top = dgv_YarnDetails_Total.Top + dgv_YarnDetails_Total.Height + 15
        pnl_YarnStock_Display.BringToFront()


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        btn_UserModification.Visible = False
        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            btn_UserModification.Visible = True
            chk_Printed.Enabled = True
        End If

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
        cbo_Grid_Yarn_LotNo.Visible = False


        cbo_InvoiceSufixNo.Items.Clear()
        cbo_InvoiceSufixNo.Items.Add("")
        cbo_InvoiceSufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_InvoiceSufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Loom_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SizingName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_fabric_Weave.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DBF.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ends_1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WarpLegnth.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_beam_length.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_sizing_length.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_no_of_beams.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_no_of_creel.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_elongation.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Beam_Requirement_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pickup_1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Fabric_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_pickup_2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_No_of_set.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Beam_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Yarn_LotNo.GotFocus, AddressOf ControlGotFocus


        AddHandler btn_EMail.Enter, AddressOf ControlGotFocus

        AddHandler txt_InvoicePrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_InvoiceSufixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Kind_Attn.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_no_of_weaver_beam.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Fabric_Name.GotFocus, AddressOf ControlGotFocus



        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Loom_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SizingName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Ends_1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Ends_1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_no_of_creel.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WarpLegnth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_sizing_length.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_beam_length.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DBF.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_fabric_Weave.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_no_of_beams.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_elongation.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Beam_Requirement_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pickup_1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Fabric_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_pickup_2.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_EMail.Leave, AddressOf ControlLostFocus
        AddHandler txt_No_of_set.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Beam_Width.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_InvoicePrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_InvoiceSufixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Kind_Attn.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_no_of_weaver_beam.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Fabric_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Yarn_LotNo.LostFocus, AddressOf ControlLostFocus



        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_WarpLegnth.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_DBF.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_no_of_creel.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_fabric_Weave.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Fabric_Width.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Ends.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_beam_length.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_sizing_length.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_no_of_beams.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler lbl_Elogation.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler Beam_Requirement_Date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Pickup.KeyDown, AddressOf TextBoxControlKeyDown




        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_WarpLegnth.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_fabric_Weave.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_DBF.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Fabric_Width.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_no_of_creel.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_beam_length.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Ends.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_sizing_length.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_no_of_beams.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler lbl_Elogation.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler Beam_Requirement_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Pickup.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Ends_2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ends_2.LostFocus, AddressOf ControlLostFocus


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Job_Card_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Job_Card_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim UID As Single = 0
        Dim vUsrNm As String = "", vAcPwd As String = "", vUnAcPwd As String = ""


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '----- KALAIMAGAL TEXTILES (AVINASHI)
            Common_Procedures.Password_Input = ""
            Dim g As New Admin_Password
            g.ShowDialog()

            UID = 1
            Common_Procedures.get_Admin_Name_PassWord_From_DB(vUsrNm, vAcPwd, vUnAcPwd)

            vAcPwd = Common_Procedures.Decrypt(Trim(vAcPwd), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))
            vUnAcPwd = Common_Procedures.Decrypt(Trim(vUnAcPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))

            If Trim(Common_Procedures.Password_Input) <> Trim(vAcPwd) And Trim(Common_Procedures.Password_Input) <> Trim(vUnAcPwd) Then
                MessageBox.Show("Invalid Admin Password", "ADMIN PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.entry_jobcard_entry, New_Entry, Me, con, "Sizing_JobCard_Head", "Sizing_JobCard_Code", NewCode, "Sizing_JobCard_Date", "(Sizing_JobCard_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Da = New SqlClient.SqlDataAdapter("Select * from Sizing_JobCard_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code = '" & Trim(NewCode) & "' and yarn_type = 'BABY'", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    ' cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = Delivered_Bags - " & Str(Val(Dt1.Rows(i).Item("Bags").ToString)) & ", Delivered_Cones = Delivered_Cones - " & Str(Val(Dt1.Rows(i).Item("cones").ToString)) & ", Delivered_Weight = Delivered_Weight - " & Str(Val(Dt1.Rows(i).Item("Weight").ToString)) & " Where setcode_forSelection = '" & Trim(Dt1.Rows(i).Item("setcode_forSelection").ToString) & "'"
                    '''cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = Delivered_Bags - " 
                    '''& Str(Val(Dt1.Rows(i).Item("Bags").ToString)) & ", Delivered_Cones = Delivered_Cones - " & Str(Val(Dt1.Rows(i).Item("cones").ToString)) & ", Delivered_Weight = Delivered_Weight - " & Str(Val(Dt1.Rows(i).Item("Weight").ToString)) & " Where setcode_forSelection = '" & Trim(Dt1.Rows(i).Item("setcode_forSelection").ToString) & "' and Company_IdNo = " & Str(Val(Dt1.Rows(i).Item("Company_IdNo").ToString))
                    'cmd.ExecuteNonQuery()

                Next i

            End If
            Dt1.Clear()

            'cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sizing_JobCard_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sizing_JobCard_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_SizingName.Enabled = True And cbo_SizingName.Visible = True Then cbo_SizingName.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 ) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Count_name from Count_Head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select Mill_name from Mill_Head order by Mill_name", con)
            da.Fill(dt3)
            cbo_Filter_MillName.DataSource = dt3
            cbo_Filter_MillName.DisplayMember = "Mill_name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_MillName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
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

            'da = New SqlClient.SqlDataAdapter("select top 1 Sizing_JobCard_No from Sizing_JobCard_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sizing_JobCard_No", con)
            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_JobCard_RefNo from Sizing_JobCard_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sizing_JobCard_No", con)

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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

            'da = New SqlClient.SqlDataAdapter("select top 1 Sizing_JobCard_No from Sizing_JobCard_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sizing_JobCard_No", con)
            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_JobCard_RefNo from Sizing_JobCard_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sizing_JobCard_No", con)

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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

            'da = New SqlClient.SqlDataAdapter("select top 1 Sizing_JobCard_No from Sizing_JobCard_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sizing_JobCard_No desc", con)
            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_JobCard_RefNo from Sizing_JobCard_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sizing_JobCard_No desc", con)

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
            'da = New SqlClient.SqlDataAdapter("select top 1 Sizing_JobCard_No from Sizing_JobCard_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sizing_JobCard_No desc", con)
            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_JobCard_RefNo from Sizing_JobCard_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sizing_JobCard_No desc", con)

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

        Try
            clear()

            New_Entry = True

            lbl_JobNo.Text = Common_Procedures.get_MaxCode(con, "Sizing_JobCard_Head", "Sizing_JobCard_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_JobNo.ForeColor = Color.Red

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Job No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sizing_JobCard_No from Sizing_JobCard_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Job No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.yarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.yarn_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Job No.", "FOR NEW JOB INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sizing_JobCard_No from Sizing_JobCard_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Job No", "DOES NOT INSERT NEW JOB...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_JobNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW JOB...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Selc_SetCode As String
        Dim led_id As Integer = 0
        Dim Bw_id As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mil_ID As Integer = 0
        Dim Cnt_Grid_ID As Integer = 0
        Dim Mil_Grid_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = "", YrnPartcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotBags As Single, vTotCones As Single, vTotWeight As Single, vTotLooseCones As Single
        Dim vTotNoOfCones As Single, vTotYrnWeight As Single
        Dim vSELC_JOBCODE As String
        Dim vSetCd As String, vSetNo As String
        Dim Nr As Long
        Dim vLOT_ENT_REFCODE As String
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim loomtype_Id As Integer = 0
        Dim ByCnCnt_ID As Integer = 0
        Dim ByCnMil_ID As Integer = 0
        Dim vORDNO As String = ""
        Dim vINVoNo As String = ""
        Dim Ref_No As Integer = 0
        Dim Cloth_id As Integer = 0



        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.entry_jobcard_entry, New_Entry, Me, con, "Sizing_JobCard_Head", "Sizing_JobCard_Code", NewCode, "Sizing_JobCard_Date", "(Sizing_JobCard_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Yarn_Delivery_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If


        Cloth_id = Common_Procedures.Cloth_NameToIdNo(con, cbo_Fabric_Name.Text)
        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SizingName.Text)

        If led_id = 0 Then
            MessageBox.Show("Invalid Sizing Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_SizingName.Enabled And cbo_SizingName.Visible Then cbo_SizingName.Focus()
            Exit Sub
        End If


        'If Trim(txt_Ends.Text) = "" Then
        '    MessageBox.Show("Invalid Ends Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_Ends.Enabled Then txt_Ends.Focus()
        '    Exit Sub
        'End If

        'If Val(txt_WarpLegnth.Text) = 0 Then
        '    MessageBox.Show("Invalid Warp Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_WarpLegnth.Enabled Then txt_WarpLegnth.Focus()
        '    Exit Sub
        'End If


        For i = 0 To dgv_YarnDetails.RowCount - 1

            If Val(dgv_YarnDetails.Rows(i).Cells(6).Value) <> 0 Then

                YCnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(1).Value)
                If Val(YCnt_ID) = 0 Then
                    MessageBox.Show("Invalid CountName", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                If Trim(dgv_YarnDetails.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(2)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                YMil_ID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(3).Value)
                If Val(YMil_ID) = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(3)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

            End If

        Next
        loomtype_Id = Common_Procedures.LoomType_NameToIdNo(con, cbo_Loom_Type.Text)

        vTotBags = 0 : vTotCones = 0 : vTotWeight = 0 : vTotLooseCones = 0
        If dgv_YarnDetails_Total.RowCount > 0 Then

            vTotBags = Val(dgv_YarnDetails_Total.Rows(0).Cells(8).Value())
            vTotNoOfCones = Val(dgv_YarnDetails_Total.Rows(0).Cells(9).Value())
            vTotLooseCones = Val(dgv_YarnDetails_Total.Rows(0).Cells(10).Value())
            vTotYrnWeight = Val(dgv_YarnDetails_Total.Rows(0).Cells(11).Value())
        End If

        If Val(lbl_no_of_cones_required.Text) = 0 Then lbl_no_of_cones_required.Text = 0

        Selc_SetCode = Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))
        vINVoNo = Trim(txt_InvoicePrefixNo.Text) & Trim(lbl_JobNo.Text) & Trim(cbo_InvoiceSufixNo.Text)
        Ref_No = Val(lbl_JobNo.Text)

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_JobNo.Text = Common_Procedures.get_MaxCode(con, "Sizing_JobCard_Head", "Sizing_JobCard_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@JobDate", dtp_Date.Value.Date)

            'cmd.Parameters.Clear()
            'cmd.Parameters.AddWithValue("@BeamJobDate", Beam_Requirement_Date.Value.Date)
            If IsDate(msk_Beam_Requirement_Date.Text) = True Then
                cmd.Parameters.AddWithValue("@BeamJobDate", Convert.ToDateTime(msk_Beam_Requirement_Date.Text))
            End If

            vORDNO = Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text))

            vSELC_JOBCODE = Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

            If New_Entry = True Then

                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then

                cmd.CommandText = "Insert into Sizing_JobCard_Head (Sizing_JobCard_Code,  Sizing_JobCode_forSelection  ,            Company_IdNo             ,     Sizing_JobCard_RefNo     ,        Sizing_JobCard_No  ,            for_OrderBy    ,    Sizing_JobCard_Date  ,     ledger_idno,          Loom_Type_idno        , ends_name                  ,    Beam_Length                ,               Wrap_Length             ,                 Sizing_Length         ,              Remarks             ,         Fabric_Weave         ,                         creel        ,                 Fabric_Width ,                                 DBF,                                No_Of_Beams,                         Pick_Up,                              Elongation,                                  Beam_Requirement_Date,                                                           Pick_Up_2,                              no_of_set,                          no_of_cones_required        ,                Total_Bags ,                     Total_Cones,              Total_Weight ,                        Beam_Width                 ,           Ends_Name_2          ,                    Sizing_JobCard_PrefixNo              ,   Sizing_JobCard_SuffixNo               , Kind_Attention_Person_Name         ,     cloth_idno      ,         No_Of_Weaver_beam                  ,  Total_Loose_Cones) " &
                                                      " Values ('" & Trim(NewCode) & "',  '" & Trim(vSELC_JOBCODE) & "',  " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "'  ,    '" & Trim(vINVoNo) & "',  " & Str(Val(vORDNO)) & ", @JobDate, " & Str(Val(led_id)) & "," & Val(loomtype_Id) & "," & Str(Val(txt_Ends_1.Text)) & ",  '" & Trim(txt_beam_length.Text) & "' , " & Str(Val(txt_WarpLegnth.Text)) & " ,  " & Str(Val(txt_sizing_length.Text)) & " ,  '" & Trim(txt_Remarks.Text) & "',  '" & Trim(txt_fabric_Weave.Text) & "', '" & Trim(txt_no_of_creel.Text) & "', '" & Trim(txt_Fabric_Width.Text) & "' , '" & Trim(txt_DBF.Text) & "'," & Str(Val(txt_no_of_beams.Text)) & ",  " & Str(Val(txt_Pickup_1.Text)) & " ," & Str(Val(txt_elongation.Text)) & ", " & IIf(IsDate(msk_Beam_Requirement_Date.Text) = True, "@BeamJobDate", "Null") & ", " & Str(Val(txt_pickup_2.Text)) & ", '" & Trim(txt_No_of_set.Text) & "', " & Str(Val(CDbl(lbl_no_of_cones_required.Text))) & "," & Str(Val(vTotBags)) & ", " & Str(Val(vTotNoOfCones)) & "," & Str(Val(vTotYrnWeight)) & " , '" & Trim(txt_Beam_Width.Text) & "'," & Str(Val(txt_Ends_2.Text)) & ",   '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "'       , '" & Trim(cbo_InvoiceSufixNo.Text) & "' , '" & Trim(cbo_Kind_Attn.Text) & "' , " & Val(Cloth_id) & ", '" & Trim(txt_no_of_weaver_beam.Text) & "', " & Str(Val(vTotLooseCones)) & ")"
                cmd.ExecuteNonQuery()

                'Else

                '    cmd.CommandText = "Insert into Sizing_JobCard_Head (Sizing_JobCard_Code,  Sizing_JobCode_forSelection  ,          Company_IdNo             ,     Sizing_JobCard_RefNo      ,           for_OrderBy    , Sizing_JobCard_Date ,                                       ledger_idno,          Loom_Type_idno        , ends_name                  ,    Beam_Length                ,        Wrap_Length              ,                 Sizing_Length         ,              Remarks             ,         Fabric_Weave         ,                         creel        ,                 Fabric_Width ,                                 DBF,                                No_Of_Beams,                         Pick_Up,                              Elongation,                                  Beam_Requirement_Date,                                                           Pick_Up_2,                              no_of_set,                          no_of_cones_required        ,                Total_Bags ,                     Total_Cones,              Total_Weight ,                        Beam_Width                 ,           Ends_Name_2                 ,     cloth_idno          ,             No_Of_Weaver_beam                         ) " &
                '                                      " Values ('" & Trim(NewCode) & "',  '" & Trim(vSELC_JOBCODE) & "'    ,  " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "',  " & Str(Val(vORDNO)) & ",        @JobDate     , " & Str(Val(led_id)) & ", " & Val(loomtype_Id) & "," & Str(Val(txt_Ends_1.Text)) & ",  " & Str(Val(txt_beam_length.Text)) & " , '" & Trim(txt_WarpLegnth.Text) & "' ,  " & Str(Val(txt_sizing_length.Text)) & " ,  '" & Trim(txt_Remarks.Text) & "',  '" & Trim(txt_fabric_Weave.Text) & "', '" & Trim(txt_no_of_creel.Text) & "', '" & Trim(txt_Fabric_Width.Text) & "' , '" & Trim(txt_DBF.Text) & "'," & Str(Val(txt_no_of_beams.Text)) & ",  " & Str(Val(txt_Pickup_1.Text)) & " ," & Str(Val(txt_elongation.Text)) & ", " & IIf(IsDate(msk_Beam_Requirement_Date.Text) = True, "@BeamJobDate", "Null") & ", " & Str(Val(txt_pickup_2.Text)) & ", '" & Trim(txt_No_of_set.Text) & "', " & Str(Val(CDbl(lbl_no_of_cones_required.Text))) & "," & Str(Val(vTotBags)) & ", " & Str(Val(vTotNoOfCones)) & "," & Str(Val(vTotYrnWeight)) & " , '" & Trim(txt_Beam_Width.Text) & "'," & Str(Val(txt_Ends_2.Text)) & ",    " & Val(Cloth_id) & " , '" & Trim(txt_no_of_weaver_beam.Text) & "'            )"
                '    cmd.ExecuteNonQuery()

                'End If

            Else

                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then

                cmd.CommandText = "Update Sizing_JobCard_Head set Sizing_JobCode_forSelection = '" & Trim(vSELC_JOBCODE) & "',  Sizing_JobCard_Date = @JobDate, ledger_idno = " & Str(Val(led_id)) & ", Loom_Type_idno = " & Val(loomtype_Id) & ", ends_name = '" & Trim(txt_Ends_1.Text) & "', Beam_Length = '" & Trim(txt_beam_length.Text) & "', Wrap_Length = " & Str(Val(txt_WarpLegnth.Text)) & ", Sizing_Length = '" & Trim(txt_sizing_length.Text) & "', remarks = '" & Trim(txt_Remarks.Text) & "', Fabric_Weave = '" & Trim(txt_fabric_Weave.Text) & "', creel = '" & Trim(txt_no_of_creel.Text) & "', Fabric_Width = '" & Trim(txt_Fabric_Width.Text) & "', DBF = '" & Trim(txt_DBF.Text) & "', No_Of_Beams ='" & Trim(txt_no_of_beams.Text) & "', Pick_Up=" & Str(Val(txt_Pickup_1.Text)) & "  , Elongation = " & Str(Val(txt_elongation.Text)) & ",Beam_Requirement_Date=" & IIf(IsDate(msk_Beam_Requirement_Date.Text) = True, "@BeamJobDate", "Null") & " , Pick_Up_2 =  " & Str(Val(txt_pickup_2.Text)) & ",  no_of_set =  '" & Trim(txt_No_of_set.Text) & "', no_of_cones_required =  " & Str(Val(CDbl(lbl_no_of_cones_required.Text))) & " , Total_Bags = " & Str(Val(vTotBags)) & ", Total_Cones= " & Str(Val(vTotNoOfCones)) & ", Total_Weight =" & Str(Val(vTotYrnWeight)) & " , Beam_Width = '" & Trim(txt_Beam_Width.Text) & "' ,Ends_Name_2 = " & Str(Val(txt_Ends_2.Text)) & " , Sizing_JobCard_No = '" & Trim(vINVoNo) & "', Sizing_JobCard_PrefixNo = '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' ,  Sizing_JobCard_SuffixNo = '" & Trim(cbo_InvoiceSufixNo.Text) & "', Kind_Attention_Person_Name = '" & Trim(cbo_Kind_Attn.Text) & "',cloth_idno = " & Val(Cloth_id) & ", No_Of_Weaver_beam = '" & Trim(txt_no_of_weaver_beam.Text) & "' , Total_Loose_Cones = " & Str(Val(vTotLooseCones)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                'Else

                '    cmd.CommandText = "Update Sizing_JobCard_Head set Sizing_JobCode_forSelection = '" & Trim(vSELC_JOBCODE) & "',  Sizing_JobCard_Date = @JobDate, ledger_idno = " & Str(Val(led_id)) & ", Loom_Type_idno = " & Val(loomtype_Id) & ", ends_name = '" & Trim(txt_Ends_1.Text) & "', Beam_Length = '" & Trim(txt_beam_length.Text) & "', Wrap_Length = '" & Trim(txt_WarpLegnth.Text) & "', Sizing_Length = '" & Trim(txt_sizing_length.Text) & "', remarks = '" & Trim(txt_Remarks.Text) & "', Fabric_Weave = '" & Trim(txt_fabric_Weave.Text) & "', creel = '" & Trim(txt_no_of_creel.Text) & "', Fabric_Width = '" & Trim(txt_Fabric_Width.Text) & "', DBF = '" & Trim(txt_DBF.Text) & "', No_Of_Beams ='" & Trim(txt_no_of_beams.Text) & "', Pick_Up=" & Str(Val(txt_Pickup_1.Text)) & "  , Elongation = " & Str(Val(txt_elongation.Text)) & ",Beam_Requirement_Date=" & IIf(IsDate(msk_Beam_Requirement_Date.Text) = True, "@BeamJobDate", "Null") & " , Pick_Up_2 =  " & Str(Val(txt_pickup_2.Text)) & ",  no_of_set =  '" & Trim(txt_No_of_set.Text) & "', no_of_cones_required =  " & Str(Val(CDbl(lbl_no_of_cones_required.Text))) & " , Total_Bags = " & Str(Val(vTotBags)) & ", Total_Cones= " & Str(Val(vTotNoOfCones)) & ", Total_Weight =" & Str(Val(vTotYrnWeight)) & " , Beam_Width = '" & Trim(txt_Beam_Width.Text) & "' ,Ends_Name_2 = " & Str(Val(txt_Ends_2.Text)) & " ,cloth_idno = " & Val(Cloth_id) & ", No_Of_Weaver_beam = '" & Trim(txt_no_of_weaver_beam.Text) & "'   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code = '" & Trim(NewCode) & "'"
                '    cmd.ExecuteNonQuery()

                'End If

                'Da = New SqlClient.SqlDataAdapter("Select * from Sizing_JobCard_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code = '" & Trim(NewCode) & "' and yarn_type = 'BABY'", con)
                'Da.SelectCommand.Transaction = tr
                'Dt1 = New DataTable
                'Da.Fill(Dt1)

                'If Dt1.Rows.Count > 0 Then

                '    For i = 0 To Dt1.Rows.Count - 1

                '        Nr = 0
                '        ' cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = Delivered_Bags - " & Str(Val(Dt1.Rows(i).Item("Bags").ToString)) & ", Delivered_Cones = Delivered_Cones - " & Str(Val(Dt1.Rows(i).Item("cones").ToString)) & ", Delivered_Weight = Delivered_Weight - " & Str(Val(Dt1.Rows(i).Item("Weight").ToString)) & " Where setcode_forSelection = '" & Trim(Dt1.Rows(i).Item("setcode_forSelection").ToString) & "'"
                '        ''''cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = Delivered_Bags - " & Str(Val(Dt1.Rows(i).Item("Bags").ToString)) & ", Delivered_Cones = Delivered_Cones - " & Str(Val(Dt1.Rows(i).Item("cones").ToString)) & ", Delivered_Weight = Delivered_Weight - " & Str(Val(Dt1.Rows(i).Item("Weight").ToString)) & " Where setcode_forSelection = '" & Trim(Dt1.Rows(i).Item("setcode_forSelection").ToString) & "' and Company_IdNo = " & Str(Val(Dt1.Rows(i).Item("Company_IdNo").ToString))
                '        'Nr = cmd.ExecuteNonQuery()

                '    Next i

                'End If
                'Dt1.Clear()

            End If

            'If Val(Common_Procedures.settings.StatementPrint_BookNo_IN_Stock_Particulars_Status) = 1 Then
            '    Partcls = "JobCard : Job.No. " & Trim(lbl_JobNo.Text)
            '    ' PBlNo = Trim(txt_BookNo.Text)
            'Else
            '    Partcls = "JobCard : Job.No. " & Trim(lbl_JobNo.Text)
            '    PBlNo = Trim(lbl_JobNo.Text)
            'End If

            cmd.CommandText = "Delete from Sizing_JobCard_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_JobCard_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()


            With dgv_YarnDetails

                ' YrnPartcls = Partcls & Trim(.Rows(0).Cells(3).Value) & ", EndsCount : " & Trim(cbo_EndsCount.Text) & ", Beams : " & Trim(Val(vTotPvuBms)) & ", Meters : " & Trim(Val(vTotPvuMtrs))

                Sno = 0
                ByCnCnt_ID = 0
                ByCnMil_ID = 0

                Sno = 0
                For i = 0 To dgv_YarnDetails.RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(dgv_YarnDetails.Rows(i).Cells(7).Value) <> 0 Then


                        Cnt_Grid_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(1).Value), tr)
                        Mil_Grid_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(3).Value), tr)

                        vLOT_ENT_REFCODE = ""
                        If Trim(dgv_YarnDetails.Rows(i).Cells(4).Value) <> "" Then
                            vLOT_ENT_REFCODE = Common_Procedures.YarnLotCodeSelection_To_LotEntryReferenceCode(con, dgv_YarnDetails.Rows(i).Cells(4).Value, tr)
                        End If

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Sizing_JobCard_Details(Sizing_JobCard_Code,                   Company_IdNo,                    Sizing_JobCard_No,                for_OrderBy   , Sizing_JobCard_Date,       Ledger_IdNo,                  Sl_No,                   Count_IdNo,                        Yarn_Type,                                           Mill_IdNo,                               LOT_NO   ,                                                   Weight_Bag  ,                                                    Cones_Bag  ,                                                Weight_Per_Cone   ,                                            No_Of_Bags ,                                 No_Of_Cones   ,                                                    Loose_Cones  ,                                                 Weight                        ,        Lot_Entry_ReferenceCode   ) " &
                                                                       "  Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "', " & Str(Val(vORDNO)) & ",     @JobDate       ,     " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(Cnt_Grid_ID)) & ", '" & Trim(dgv_YarnDetails.Rows(i).Cells(2).Value) & "',  " & Str(Val(Mil_Grid_ID)) & ", " & Str(Val(dgv_YarnDetails.Rows(i).Cells(4).Value)) & "," & Str(Val(dgv_YarnDetails.Rows(i).Cells(5).Value)) & "  , " & Str(Val(dgv_YarnDetails.Rows(i).Cells(6).Value)) & "  ,  " & Str(Val(dgv_YarnDetails.Rows(i).Cells(7).Value)) & " , " & Str(Val(dgv_YarnDetails.Rows(i).Cells(8).Value)) & ",  " & Str(Val(dgv_YarnDetails.Rows(i).Cells(9).Value)) & ",   " & Str(Val(dgv_YarnDetails.Rows(i).Cells(10).Value)) & "," & Str(Val(dgv_YarnDetails.Rows(i).Cells(11).Value)) & " , '" & Trim(vLOT_ENT_REFCODE) & "' )"
                        cmd.ExecuteNonQuery()

                    End If




                Next

            End With


            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()



            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_JobNo.Text)
                End If
            Else
                move_record(lbl_JobNo.Text)
            End If

        Catch ex As Exception

            tr.Rollback()

            'If InStr(1, Trim(LCase(ex.Message)), "ck_stock_babycone_processing_details") > 0 Then
            '    MessageBox.Show("Invalid Baby cone Details - Delivery Qty greater than production Qty", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'Else
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'End If

        Finally
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub txt_SlNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then txt_Ends_1.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    'Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Dim i As Integer

    '    If Asc(e.KeyChar) = 13 Then

    '        If Val(txt_SlNo.Text) = 0 Then
    '            txt_Remarks.Focus()

    '        Else

    'With dgv_YarnDetails

    '    For i = 0 To .Rows.Count - 1
    '        If Val(dgv_YarnDetails.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

    '            cbo_Grid_Countname.Text = .Rows(i).Cells(1).Value
    '            cbo_YarnType.Text = .Rows(i).Cells(2).Value
    '            cbo_SetNo.Text = .Rows(i).Cells(3).Value
    '            cbo_GridMillName.Text = .Rows(i).Cells(4).Value
    '            txt_Bags.Text = Val(.Rows(i).Cells(5).Value)
    '            txt_Cones.Text = Val(.Rows(i).Cells(6).Value)
    '            txt_Weight.Text = Format(Val(.Rows(i).Cells(7).Value), "########0.000")

    '            Exit For

    '        End If

    '    Next

    'End With

    '            SendKeys.Send("{TAB}")

    '        End If

    '    End If
    'End Sub

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
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sizing_JobCard_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Sizing_JobCard_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sizing_JobCard_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Trim(cbo_Filter_MillName.Text) <> "" Then
                Mil_IdNo = Common_Procedures.Mill_NameToIdNo(con, cbo_Filter_MillName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sizing_JobCard_Code IN (select z1.Sizing_JobCard_Code from Sizing_JobCard_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ") "
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sizing_JobCard_Code IN (select z2.Sizing_JobCard_Code from Sizing_JobCard_Details z2 where z2.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Sizing_JobCard_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_JobCard_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sizing_JobCard_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Sizing_JobCard_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sizing_JobCard_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Fabric_Weave").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("creel").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Fabric_Width").ToString), "########0.000")

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

    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub




    'Private Sub cbo_BeamWidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
    '        Dim f As New Beam_Width_Creation

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = cbo_BeamWidth.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()
    '    End If
    'End Sub



    'Private Sub cbo_Grid_Count_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
    '        Dim f As New Count_Creation

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = cbo_Grid_Countname.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()

    '    End If
    'End Sub




    'Private Sub dgv_YarnDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
    '    On Error Resume Next


    '    If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub


    '    If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

    '    With dgv_YarnDetails
    '        If .Visible Then
    '            If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
    '                Total_Calculation()
    '            End If
    '        End If
    '    End With
    'End Sub

    'Private Sub dgv_YarnDetails_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)

    '    If Trim(dgv_YarnDetails.CurrentRow.Cells(1).Value) <> "" Then

    '        txt_SlNo.Text = Val(dgv_YarnDetails.CurrentRow.Cells(0).Value)
    '        cbo_Grid_Countname.Text = Trim(dgv_YarnDetails.CurrentRow.Cells(1).Value)
    '        cbo_YarnType.Text = Trim(dgv_YarnDetails.CurrentRow.Cells(2).Value)
    '        cbo_SetNo.Text = dgv_YarnDetails.CurrentRow.Cells(3).Value
    '        cbo_GridMillName.Text = dgv_YarnDetails.CurrentRow.Cells(4).Value
    '        txt_Bags.Text = Val(dgv_YarnDetails.CurrentRow.Cells(5).Value)
    '        txt_Cones.Text = Val(dgv_YarnDetails.CurrentRow.Cells(6).Value)
    '        txt_Weight.Text = Format(Val(dgv_YarnDetails.CurrentRow.Cells(7).Value), "########0.000")

    '        If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

    '    End If

    'End Sub

    'Private Sub dgv_YarnDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    vcbo_KeyDwnVal = e.KeyValue

    'End Sub

    'Private Sub dgv_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Dim n As Integer

    '    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

    '        With dgv_YarnDetails

    '            n = .CurrentRow.Index
    '            .Rows.RemoveAt(n)

    '            For i = 0 To .Rows.Count - 1
    '                .Rows(i).Cells(0).Value = i + 1
    '            Next

    '        End With

    '        Total_Calculation()

    '        'txt_SlNo.Text = dgv_YarnDetails.Rows.Count + 1
    '        cbo_Loom_Type.Text = ""
    '        'cbo_YarnType.Text = "MILL"
    '        'cbo_SetNo.Text = ""
    '        cbo_SizingName.Text = ""
    '        ' txt_Bags.Text = ""
    '        'txt_Cones.Text = ""
    '        'txt_Weight.Text = ""

    '        'If cbo_Grid_Countname.Enabled And cbo_Grid_Countname.Visible Then cbo_Grid_Countname.Focus()

    '    End If

    'End Sub

    'Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
    '        Common_Procedures.MDI_LedType = ""
    '        Dim f As New Ledger_Creation

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()
    '    End If
    'End Sub

    Private Sub cbo_SizingName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SizingName.GotFocus
        cbo_SizingName.Tag = cbo_SizingName.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub



    Private Sub cbo_SizingName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SizingName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SizingName, txt_Remarks, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            txt_Remarks.Focus()
        End If
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
                cbo_Kind_Attn.Focus()
            Else

                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            End If
        End If
    End Sub

    Private Sub cbo_SizingName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SizingName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SizingName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
                cbo_Kind_Attn.Focus()

            Else
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            End If
        End If
    End Sub

    Private Sub cbo_SizingName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SizingName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation


            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SizingName.Name
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
        Close_Form()
    End Sub

    Private Sub cbo_Loom_TypeName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Loom_Type.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New LoomType_Creation

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_Grid_CountName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Loom_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Loom_Type.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Loom_Type, Nothing, txt_fabric_Weave, "LoomType_Head", "LoomType_name", "", "(LoomType_IdNo=0)")
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Loom_Type, Nothing, txt_DBF, "LoomType_Head", "LoomType_name", "", "(LoomType_IdNo=0)")
        If (e.KeyValue = 38 And cbo_Loom_Type.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            End If
        End If
    End Sub

    Private Sub cbo_Loom_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Loom_Type.KeyPress
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Loom_Type, txt_fabric_Weave, "LoomType_Head", "LoomType_name", "", "(LoomType_IdNo=0)")
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Loom_Type, txt_DBF, "LoomType_Head", "LoomType_name", "", "(LoomType_IdNo=0)")
    End Sub

    Private Sub cbo_SetNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        'Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Grid_Countname.Text)

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
            Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Sizing_JobCard_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Sizing_JobCard_Code = '" & Trim(NewCode) & "') ) ) )"
        'Condt = "( a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Sizing_JobCard_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Sizing_JobCard_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_BabyCone_Processing_Details a", "setcode_forSelection", Condt, "(Reference_Code = '')")

        'cbo_SetNo.Tag = cbo_SetNo.Text

    End Sub

    'Private Sub cbo_SetNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If Trim(UCase(cbo_SetNo.Text)) <> Trim(UCase(cbo_SetNo.Tag)) Then
    '        get_BabyCone_Details()
    '    End If
    'End Sub

    'Private Sub cbo_setno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Dim NewCode As String
    '    Dim Led_ID As Integer, Cnt_ID As Integer
    '    Dim Condt As String
    '    Dim Cmp_Cond As String


    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

    '    Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Grid_Countname.Text)

    '    Cmp_Cond = ""
    '    If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
    '        Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
    '    End If

    '    Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Sizing_JobCard_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Sizing_JobCard_Code = '" & Trim(NewCode) & "') ) ) )"
    '    'Condt = "( a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Sizing_JobCard_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Sizing_JobCard_Code = '" & Trim(NewCode) & "') ) ) )"

    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SetNo, cbo_YarnType, cbo_GridMillName, "Stock_BabyCone_Processing_Details", "setcode_forSelection", Condt, "(Reference_Code = '')")

    'End Sub

    Private Sub cbo_setno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        'Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Grid_Countname.Text)

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
            Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Sizing_JobCard_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Sizing_JobCard_Code = '" & Trim(NewCode) & "') ) ) )"
        'Condt = "( a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Sizing_JobCard_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Sizing_JobCard_Code = '" & Trim(NewCode) & "') ) ) )"

        ' Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SetNo, cbo_GridMillName, "Stock_BabyCone_Processing_Details a", "setcode_forSelection", Condt, "(Reference_Code = '')")

    End Sub

    'Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, dtp_Date, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    'End Sub

    'Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, dtp_Date, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    'End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    'Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Countname, txt_SlNo, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    '    If (e.KeyValue = 40 And cbo_Grid_Countname.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
    '        If Trim(cbo_Grid_Countname.Text) <> "" Then
    '            cbo_YarnType.Focus()
    '        Else
    '            txt_Remarks.Focus()
    '        End If
    '    End If
    'End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    'Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Countname, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    '    If Asc(e.KeyChar) = 13 Then
    '        If Trim(cbo_Grid_Countname.Text) <> "" Then
    '            cbo_YarnType.Focus()
    '        Else
    '            txt_Remarks.Focus()
    '        End If
    '    End If
    'End Sub
    'Private Sub cbo_Beamwidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BeamWidth, cbo_Loom_Type, txt_WarpMeters, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    'End Sub

    'Private Sub cbo_BeamWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BeamWidth, txt_WarpMeters, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    'End Sub


    'Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GridMillName, txt_Bags, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    'End Sub

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub txt_WarpMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub





    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBags As Integer, TotCones As Long, TotWeight As String, TotLooseCones As Long

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotLooseCones = 0
        TotWeight = 0
        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(8).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(8).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(9).Value)
                    TotLooseCones = TotLooseCones + Val(.Rows(i).Cells(10).Value)
                    TotWeight = Val(TotWeight) + Val(.Rows(i).Cells(11).Value)
                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(8).Value = Val(TotBags)
            .Rows(0).Cells(9).Value = Val(TotCones)
            .Rows(0).Cells(10).Value = Val(TotLooseCones)
            .Rows(0).Cells(11).Value = Format(Val(TotWeight), "########0.000")
        End With

    End Sub
    Private Sub txt_EmptyBeam_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        'If e.KeyCode = 38 Then cbo_Grid_Countname.Focus() ' SendKeys.Send("+{TAB}")
    End Sub
    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub
    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize

        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Sizing_JobCard_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Sizing_JobCard_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER A4", 827, 1169)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'MessageBox.Show("Printing_Invoice - 7")
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        'MessageBox.Show("Printing_Invoice - 8")
        PrintDocument1.DefaultPageSettings.Landscape = False
        'MessageBox.Show("Printing_Invoice - 9")
        PpSzSTS = True

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

                Else

                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings


                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    Exit For
                                End If
                            Next




                            PrintDocument1.Print()
                        End If

                    Else

                        PrintDocument1.Print()

                    End If
                End If
            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                'MessageBox.Show("Printing_Invoice - 25")

                ppd.Document = PrintDocument1

                'MessageBox.Show("Printing_Invoice - 26")

                ppd.WindowState = FormWindowState.Maximized
                'MessageBox.Show("Printing_Invoice - 27")
                ppd.StartPosition = FormStartPosition.CenterScreen
                'MessageBox.Show("Printing_Invoice - 28")

                ppd.PrintPreviewControl.AutoZoom = True
                'MessageBox.Show("Printing_Invoice - 29")
                ppd.PrintPreviewControl.Zoom = 1.0
                'MessageBox.Show("Printing_Invoice - 30")


                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    'Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim NewCode As String

    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    prn_HdDt.Clear()
    '    prn_DetDt.Clear()
    '    prn_DetIndx = 0
    '    prn_DetSNo = 0
    '    prn_PageNo = 0

    '    Try

    '        da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*  from Sizing_JobCard_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_JobCard_Code = '" & Trim(NewCode) & "'", con)
    '        da1.Fill(prn_HdDt)

    '        If prn_HdDt.Rows.Count > 0 Then

    '            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name, d.Sizing_JobCard_No from Sizing_JobCard_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Sizing_JobCard_Head d ON a.SetCode_ForSelection <> '' and a.SetCode_ForSelection = d.setcode_forSelection where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_JobCard_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
    '            da2.Fill(prn_DetDt)

    '            da2.Dispose()

    '        Else
    '            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End If

    '        da1.Dispose()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
    '    If prn_HdDt.Rows.Count <= 0 Then Exit Sub
    '    'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
    '    Printing_Format1(e)
    '    'End If
    'End Sub

    'Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
    '    Dim EntryCode As String
    '    Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
    '    Dim pFont As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim CurY As Single, TxtHgt As Single
    '    Dim LnAr(15) As Single, ClArr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    'Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False

    '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
    '    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
    '    PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

    '    ''PrintDocument pd = new PrintDocument();
    '    ''pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
    '    ''pd.Print();

    '    'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '    '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '    '    Debug.Print(ps.PaperName)
    '    '    If ps.Width = 800 And ps.Height = 600 Then
    '    '        PrintDocument1.DefaultPageSettings.PaperSize = ps
    '    '        e.PageSettings.PaperSize = ps
    '    '        PpSzSTS = True
    '    '        Exit For
    '    '    End If
    '    'Next

    '    'If PpSzSTS = False Then
    '    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
    '    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
    '    '            e.PageSettings.PaperSize = ps
    '    '            PpSzSTS = True
    '    '            Exit For
    '    '        End If
    '    '    Next

    '    '    If PpSzSTS = False Then
    '    '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '    '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
    '    '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '    '                PrintDocument1.DefaultPageSettings.PaperSize = ps
    '    '                e.PageSettings.PaperSize = ps
    '    '                Exit For
    '    '            End If
    '    '        Next
    '    '    End If

    '    'End If

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 30 ' 50
    '        .Right = 30  '50
    '        .Top = 25
    '        .Bottom = 30 ' 50
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    pFont = New Font("Calibri", 11, FontStyle.Regular)

    '    e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

    '    With PrintDocument1.DefaultPageSettings.PaperSize
    '        PrintWidth = .Width - RMargin - LMargin
    '        PrintHeight = .Height - TMargin - BMargin
    '        PageWidth = .Width - RMargin
    '        PageHeight = .Height - BMargin
    '    End With
    '    If PrintDocument1.DefaultPageSettings.Landscape = True Then
    '        With PrintDocument1.DefaultPageSettings.PaperSize
    '            PrintWidth = .Height - TMargin - BMargin
    '            PrintHeight = .Width - RMargin - LMargin
    '            PageWidth = .Height - TMargin
    '            PageHeight = .Width - RMargin
    '        End With
    '    End If

    '    NoofItems_PerPage = 5 ' 6 ' 5

    '    Erase LnAr
    '    Erase ClArr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClArr(1) = 40
    '    ClArr(2) = 60 : ClArr(3) = 80 : ClArr(4) = 210 : ClArr(5) = 100 : ClArr(6) = 70 : ClArr(7) = 70
    '    ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

    '    'ClArr(1) = Val(40)
    '    'ClArr(2) = 60 : ClArr(3) = 80 : ClArr(4) = 150 : ClArr(5) = 100 : ClArr(6) = 70 : ClArr(7) = 70
    '    'ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))


    '    TxtHgt = 18.8  ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

    '    EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try

    '        If prn_HdDt.Rows.Count > 0 Then

    '            Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

    '            Try

    '                NoofDets = 0

    '                CurY = CurY - 10

    '                If prn_DetDt.Rows.Count > 0 Then

    '                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                        If NoofDets >= NoofItems_PerPage Then
    '                            CurY = CurY + TxtHgt

    '                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

    '                            NoofDets = NoofDets + 1

    '                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

    '                            e.HasMorePages = True
    '                            Return

    '                        End If

    '                        prn_DetSNo = prn_DetSNo + 1

    '                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
    '                        ItmNm2 = ""
    '                        If Len(ItmNm1) > 18 Then
    '                            For I = 18 To 1 Step -1
    '                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                            Next I
    '                            If I = 0 Then I = 18
    '                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
    '                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                        End If


    '                        CurY = CurY + TxtHgt

    '                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 25, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
    '                        If IsDBNull(prn_DetDt.Rows(prn_DetIndx).Item("Sizing_JobCard_No").ToString) = False Then
    '                            If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sizing_JobCard_No").ToString) <> "" Then
    '                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sizing_JobCard_No").ToString), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
    '                            End If
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

    '                        NoofDets = NoofDets + 1

    '                        If Trim(ItmNm2) <> "" Then
    '                            CurY = CurY + TxtHgt - 5
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
    '                            NoofDets = NoofDets + 1
    '                        End If

    '                        prn_DetIndx = prn_DetIndx + 1

    '                    Loop

    '                End If

    '                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

    '            Catch ex As Exception

    '                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '            End Try

    '        End If

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    '    e.HasMorePages = False

    'End Sub

    'Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
    '    Dim p1Font As Font
    '    Dim strHeight As Single
    '    Dim W1 As Single, C1 As Single, S1 As Single

    '    PageNo = PageNo + 1

    '    CurY = TMargin

    '    da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from Sizing_JobCard_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Sizing_JobCard_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
    '    da2.Fill(dt2)
    '    If dt2.Rows.Count > NoofItems_PerPage Then
    '        Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    End If
    '    dt2.Clear()


    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY

    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then
    '        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
    '        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '    Else

    '        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
    '            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
    '            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
    '            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '        Else
    '            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '        End If

    '    End If
    '    'Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    'Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    'Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
    '    End If

    '    CurY = CurY + TxtHgt - 10
    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

    '    If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '    Else
    '        p1Font = New Font("Calibri", 9, FontStyle.Regular)
    '    End If
    '    CurY = CurY + strHeight
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)

    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
    '    CurY = CurY + strHeight
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

    '    'CurY = CurY + strHeight
    '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

    '    CurY = CurY + TxtHgt - 13  ' 10
    '    p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

    '    'CurY = CurY + TxtHgt

    '    CurY = CurY + strHeight + 5 ' + 150
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    Try

    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
    '        W1 = e.Graphics.MeasureString("BOOK NO  : ", pFont).Width
    '        S1 = e.Graphics.MeasureString("TO    :", pFont).Width

    '        CurY = CurY + TxtHgt - 5
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)

    '        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_JobCard_No").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        p1Font = New Font("Calibri", 14, FontStyle.Bold)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sizing_JobCard_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(3) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

    '        CurY = CurY + TxtHgt - 10
    '        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(4) = CurY

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    '    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '        Dim p1Font As Font
    '        Dim I As Integer
    '        Dim Cmp_Name As String

    '        Try

    '            For I = NoofDets + 1 To NoofItems_PerPage
    '                CurY = CurY + TxtHgt
    '            Next

    '            CurY = CurY + TxtHgt
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(5) = CurY

    '            CurY = CurY + TxtHgt - 10
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

    '                If Val(prn_HdDt.Rows(0).Item("Fabric_Weave").ToString) <> 0 Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Fabric_Weave").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '                End If
    '                If Val(prn_HdDt.Rows(0).Item("creel").ToString) <> 0 Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("creel").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '                End If
    '                If Val(prn_HdDt.Rows(0).Item("Fabric_Width").ToString) <> 0 Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Fabric_Width").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
    '                End If
    '            End If

    '            CurY = CurY + TxtHgt - 15

    '            CurY = CurY + TxtHgt
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(6) = CurY

    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

    '            CurY = CurY + TxtHgt - 5

    '            Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
    '            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
    '                Common_Procedures.Print_To_PrintDocument(e, " Empty Beam : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 430, CurY, 0, 0, pFont)
    '            End If
    '            If Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString) <> 0 Then
    '                Common_Procedures.Print_To_PrintDocument(e, " Empty Bags : " & Trim(prn_HdDt.Rows(0).Item("Empty_Bags").ToString), PageWidth - 280, CurY, 0, 0, pFont)
    '            End If
    '            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
    '                Common_Procedures.Print_To_PrintDocument(e, " Empty Cones : " & Trim(prn_HdDt.Rows(0).Item("Empty_Cones").ToString), PageWidth - 150, CurY, 0, 0, pFont)
    '            End If

    '            CurY = CurY + TxtHgt + 10
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(7) = CurY

    '            CurY = CurY + TxtHgt
    '            If Val(Common_Procedures.User.IdNo) <> 1 Then
    '                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
    '            End If
    '            CurY = CurY + TxtHgt

    '            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
    '                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

    '            Else
    '                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

    '            End If

    '            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
    '            p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + PageWidth - 75, CurY, 1, 0, p1Font)

    '            'e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

    '            CurY = CurY + TxtHgt + 10

    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    '        Catch ex As Exception

    '            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End Try

    '    End Sub

    'Private Sub dgv_YarnDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    On Error Resume Next
    '    If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
    'End Sub



    Private Sub txt_EmptyBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyCones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyBags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Ends_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Ends_1.KeyDown
        If e.KeyCode = 40 Then
            ' txt_No_of_set.Focus()
            If txt_Ends_2.Visible Then
                txt_Ends_2.Focus()
            Else
                txt_beam_length.Focus()
            End If
        End If
        If e.KeyCode = 38 Then
            'txt_Fabric_Width.Focus() ' SendKeys.Send("+{TAB}")
            txt_elongation.Focus()
        End If
    End Sub

    Private Sub txt_Ends_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Ends_1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            ' txt_No_of_set.Focus()
            If txt_Ends_2.Visible Then
                    txt_Ends_2.Focus()
                Else
                    txt_beam_length.Focus()
                End If
            End If
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        'Dim dgv1 As New DataGridView
        'Dim i As Integer

        'If ActiveControl.Name = dgv_YarnDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then
        '    On Error Resume Next

        '    dgv1 = Nothing

        '    If ActiveControl.Name = dgv_YarnDetails.Name Then
        '        dgv1 = dgv_YarnDetails

        '    ElseIf dgv_YarnDetails.IsCurrentRowDirty = True Then
        '        dgv1 = dgv_YarnDetails

        '    ElseIf dgv_ActiveCtrl_Name = dgv_YarnDetails.Name Then
        '        dgv1 = dgv_YarnDetails

        '    End If

        '    If IsNothing(dgv1) = True Then


        '        With dgv1

        '            If dgv1.Name = dgv_YarnDetails.Name Then

        '                If keyData = Keys.Enter Or keyData = Keys.Down Then
        '                    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
        '                        If .CurrentCell.RowIndex = .RowCount - 1 Then
        '                            cbo_Loom_Type.Focus()

        '                        Else
        '                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

        '                        End If

        '                    Else
        '                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

        '                    End If

        '                    Return True

        '                ElseIf keyData = Keys.Up Then

        '                    If .CurrentCell.ColumnIndex <= 1 Then
        '                        If .CurrentCell.RowIndex = 0 Then

        '                            cbo_SizingName.Focus()

        '                        Else
        '                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

        '                        End If

        '                    Else
        '                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

        '                    End If

        '                    'Else
        '                    '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

        '                    'End If

        '                    'Else
        '                    '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

        '                End If

        '                Return True

        '                Else
        '                    Return MyBase.ProcessCmdKey(msg, keyData)

        '                End If



        'End With

        '    Else

        '        Return MyBase.ProcessCmdKey(msg, keyData)

        '    End If

        'Else

        '    Return MyBase.ProcessCmdKey(msg, keyData)

        'End If

        '*****************************************


        Dim dgv1 As New DataGridView
        Dim i As Integer

        If ActiveControl.Name = dgv_YarnDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then
            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_YarnDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails


            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1

                If dgv1.Name = dgv_YarnDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                'txt_TapeLength.Focus()
                                cbo_Loom_Type.Focus()

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
                                'If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Or Trim(UCase(cbo_Type.Text)) = "SIZING-UNIT PAVU DELIVERY" Then
                                cbo_SizingName.Focus()

                                'Else
                                '    If dgv_PavuDetails.Rows.Count = 0 Then dgv_PavuDetails.Rows.Add()
                                '    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.Beam_No)
                                '    dgv_PavuDetails.Focus()

                                'End If

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


                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If





    End Function

    Private Sub get_MillCount_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As Long
        Dim Wgt_Bag As Single
        Dim Wgt_Cn As Single
        Dim CntID As Integer
        Dim MilID As Integer

        If Trim(dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value) = "" Then Exit Sub
        If Trim(UCase(dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(2).Value)) <> "MILL" Then Exit Sub
        If Trim(dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(3).Value) = "" Then Exit Sub

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(3).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_YarnDetails

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        Wgt_Bag = Dt.Rows(0).Item("Weight_Bag").ToString
                        Wgt_Cn = Dt.Rows(0).Item("Weight_Cone").ToString
                        Cn_bag = Dt.Rows(0).Item("Cones_Bag").ToString
                    End If
                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

                .Rows(.CurrentRow.Index).Cells(5).Value = Format(Val(Wgt_Bag), "#########0.000")
                .Rows(.CurrentRow.Index).Cells(6).Value = Cn_bag
                .Rows(.CurrentRow.Index).Cells(7).Value = Format(Val(Wgt_Cn), "#########0.000")

            End With

        End If

    End Sub

    Private Sub cbo_Loom_Type_GotFocus(sender As Object, e As EventArgs) Handles cbo_Loom_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "LoomType_Head", "LoomType_name", "", "(LoomType_IdNo=0)")
    End Sub

    Private Sub txt_beam_length_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_beam_length.KeyDown
        If e.KeyCode = 38 Then
            'txt_WarpLegnth.Focus()
            If txt_Ends_2.Visible Then
                txt_Ends_2.Focus()
            Else
                txt_Ends_1.Focus()
            End If

        End If

        If e.KeyCode = 40 Then
            'txt_sizing_length.Focus()
            msk_Beam_Requirement_Date.Focus()
        End If
    End Sub

    Private Sub txt_beam_length_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_beam_length.KeyPress
        'If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            'txt_sizing_length.Focus()
            msk_Beam_Requirement_Date.Focus()
        End If
    End Sub

    Private Sub txt_fabric_Weave_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_fabric_Weave.KeyDown
        If e.KeyCode = 38 Then
            'cbo_Loom_Type.Focus()
            cbo_Fabric_Name.Focus()
        End If
        If e.KeyCode = 40 Then
            txt_Fabric_Width.Focus()
        End If
    End Sub

    Private Sub txt_fabric_Weave_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_fabric_Weave.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Fabric_Width.Focus()
        End If
    End Sub


    Private Sub txt_Fabric_Width_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Fabric_Width.KeyDown
        If e.KeyCode = 38 Then
            txt_fabric_Weave.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_Ends_1.Focus()
            txt_WarpLegnth.Focus()
        End If
    End Sub
    Private Sub txt_Fabric_Width_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Fabric_Width.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'txt_Ends_1.Focus()
            txt_WarpLegnth.Focus()
        End If
    End Sub

    Private Sub txt_no_of_creel_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_no_of_creel.KeyDown
        If e.KeyCode = 38 Then
            'txt_DBF.Focus()
            txt_no_of_weaver_beam.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_WarpLegnth.Focus()
            txt_Pickup_1.Focus()
        End If
    End Sub

    Private Sub txt_no_of_creel_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_no_of_creel.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'txt_WarpLegnth.Focus()
            txt_Pickup_1.Focus()
        End If
    End Sub

    Private Sub txt_WarpLegnth_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_WarpLegnth.KeyDown
        If e.KeyCode = 38 Then
            'txt_no_of_creel.Focus()
            txt_Fabric_Width.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_beam_length.Focus()
            txt_elongation.Focus()
        End If
    End Sub

    Private Sub txt_WarpLegnth_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_WarpLegnth.KeyPress
        'If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            'txt_beam_length.Focus()
            txt_elongation.Focus()
        End If
    End Sub

    Private Sub txt_sizing_length_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_sizing_length.KeyDown
        If e.KeyCode = 38 Then
            'txt_beam_length.Focus()
            txt_No_of_set.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_no_of_beams.Focus()
            txt_Beam_Width.Focus()
        End If
    End Sub

    Private Sub txt_sizing_length_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_sizing_length.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            'txt_no_of_beams.Focus()
            txt_Beam_Width.Focus()
        End If
    End Sub

    Private Sub txt_no_of_beams_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_no_of_beams.KeyDown
        If e.KeyCode = 38 Then
            'txt_sizing_length.Focus()
            txt_DBF.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_Pickup_1.Focus()
            txt_no_of_weaver_beam.Focus()
        End If
    End Sub

    Private Sub txt_no_of_beams_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_no_of_beams.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'txt_Pickup_1.Focus()
            txt_no_of_weaver_beam.Focus()
        End If
    End Sub

    Private Sub txt_Pickup_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Pickup_1.KeyDown
        If e.KeyCode = 38 Then
            'txt_no_of_beams.Focus()
            txt_no_of_creel.Focus()
        End If
        If e.KeyCode = 40 Then
            txt_pickup_2.Focus()
        End If
    End Sub

    Private Sub txt_Pickup_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Pickup_1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_pickup_2.Focus()
        End If
    End Sub

    Private Sub txt_DBF_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_DBF.KeyDown
        If e.KeyCode = 38 Then
            'txt_No_of_set.Focus()
            cbo_Loom_Type.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_no_of_creel.Focus()
            txt_no_of_beams.Focus()
        End If
    End Sub

    Private Sub txt_DBF_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_DBF.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'txt_no_of_creel.Focus()
            txt_no_of_beams.Focus()
        End If
    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        Yarn_Stock_Display(1)
    End Sub

    Private Sub cbo_Grid_CountName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If Val(.CurrentCell.RowIndex) <= 0 Then

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
                        cbo_Kind_Attn.Focus()
                    Else
                        cbo_SizingName.Focus()
                    End If


                    'Else
                    '    .Focus()
                    '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                    '    .CurrentCell.Selected = True


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                    .CurrentCell.Selected = True

                End If

            End If

            'End If
            'End If
            If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    cbo_Loom_Type.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_CountName.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    cbo_Loom_Type.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End With

        End If

    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_CountName_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then
                With dgv_YarnDetails
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_CountName.Text)
                        Yarn_Stock_Display(1)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        Yarn_Stock_Display(1)
    End Sub

    Private Sub cbo_Grid_MillName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True

            End If

        End With

    End Sub

    Private Sub cbo_Grid_MillName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails
                .Focus()
                .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_MillName.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With

        End If
    End Sub

    Private Sub cbo_Grid_MillName_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_MillName_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_MillName.TextChanged
        Try
            If cbo_Grid_MillName.Visible Then


                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
                With dgv_YarnDetails
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                        Yarn_Stock_Display(1)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_YarnType_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_YarnType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type='')")
        Yarn_Stock_Display(1)
    End Sub

    Private Sub cbo_Grid_YarnType_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_YarnType.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_YarnType, Nothing, Nothing, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type = '')")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End If

        End With
    End Sub

    Private Sub cbo_Grid_YarnType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_YarnType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_YarnType, Nothing, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type='')")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Focus()
                .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_YarnType.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End With

        End If
    End Sub

    Private Sub cbo_Grid_YarnType_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_YarnType.TextChanged
        Try
            If cbo_Grid_YarnType.Visible Then


                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
                With dgv_YarnDetails
                    If Val(cbo_Grid_YarnType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_YarnType.Text)
                        Yarn_Stock_Display(1)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_YarnDetails_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim rect As Rectangle


        With dgv_YarnDetails
            dgv_YarnDetails.Tag = .CurrentCell.Value

            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(.CurrentRow.Cells(2).Value) = "" Then
                .CurrentRow.Cells(2).Value = "MILL"
            End If

            vDGV_ENTRCEL_COUNTNAME = .CurrentRow.Cells(1).Value
            vDGV_ENTRCEL_YARNTYPE = .CurrentRow.Cells(2).Value
            vDGV_ENTRCEL_MILLNAME = .CurrentRow.Cells(3).Value

            If e.ColumnIndex = 1 Then

                If cbo_Grid_CountName.Visible = False Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                    cbo_Grid_CountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_CountName.DataSource = Dt1
                    cbo_Grid_CountName.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_CountName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_CountName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Grid_CountName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_CountName.Height = rect.Height  ' rect.Height
                    cbo_Grid_CountName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_CountName.Tag = Val(e.RowIndex)
                    cbo_Grid_CountName.Visible = True

                    cbo_Grid_CountName.BringToFront()
                    cbo_Grid_CountName.Focus()

                    'cbo_Grid_MillName.Visible = False
                    'cbo_Grid_YarnType.Visible = False

                End If


            Else

                cbo_Grid_CountName.Visible = False
                'cbo_Grid_CountName.Tag = -1
                'cbo_Grid_CountName.Text = ""

            End If

            If e.ColumnIndex = 2 Then

                If cbo_Grid_YarnType.Visible = False Or Val(cbo_Grid_YarnType.Tag) <> e.RowIndex Then

                    cbo_Grid_YarnType.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head order by Yarn_Type", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_YarnType.DataSource = Dt2
                    cbo_Grid_YarnType.DisplayMember = "Yarn_Type"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_YarnType.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_YarnType.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_YarnType.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_YarnType.Height = rect.Height  ' rect.Height

                    cbo_Grid_YarnType.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_YarnType.Tag = Val(e.RowIndex)
                    cbo_Grid_YarnType.Visible = True

                    cbo_Grid_YarnType.BringToFront()
                    cbo_Grid_YarnType.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_YarnType.Visible = False

                End If

            Else

                cbo_Grid_YarnType.Visible = False
                'cbo_Grid_YarnType.Tag = -1
                'cbo_Grid_YarnType.Text = ""

            End If

            If e.ColumnIndex = 3 Then

                If cbo_Grid_MillName.Visible = False Or Val(cbo_Grid_MillName.Tag) <> e.RowIndex Then

                    cbo_Grid_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_MillName.DataSource = Dt3
                    cbo_Grid_MillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_MillName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_MillName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_MillName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_MillName.Height = rect.Height  ' rect.Height

                    cbo_Grid_MillName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_MillName.Tag = Val(e.RowIndex)
                    cbo_Grid_MillName.Visible = True

                    cbo_Grid_MillName.BringToFront()
                    cbo_Grid_MillName.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_Grid_MillName.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If

            If e.ColumnIndex = 4 Then

                If cbo_Grid_Yarn_LotNo.Visible = False Or Val(cbo_Grid_Yarn_LotNo.Tag) <> e.RowIndex Then

                    cbo_Grid_Yarn_LotNo.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select LotCode_forSelection from Yarn_Lot_Head " &
                                                      "where Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & Trim(dgv_YarnDetails.CurrentRow.Cells(1).Value) & "') " &
                                                      " and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & Trim(dgv_YarnDetails.CurrentRow.Cells(3).Value) & "') order by LotCode_forSelection", con)
                    Dt4 = New DataTable
                    Da.Fill(Dt4)
                    cbo_Grid_Yarn_LotNo.DataSource = Dt4
                    cbo_Grid_Yarn_LotNo.DisplayMember = "LotCode_forSelection"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Yarn_LotNo.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_Yarn_LotNo.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_Yarn_LotNo.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_Yarn_LotNo.Height = rect.Height  ' rect.Height

                    cbo_Grid_Yarn_LotNo.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_Yarn_LotNo.Tag = Val(e.RowIndex)
                    cbo_Grid_Yarn_LotNo.Visible = True

                    cbo_Grid_Yarn_LotNo.BringToFront()
                    cbo_Grid_Yarn_LotNo.Focus()

                End If

            Else

                cbo_Grid_Yarn_LotNo.Visible = False

            End If

            If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(5).Value) = 0 And Val(.CurrentRow.Cells(6).Value) = 0 And Val(.CurrentRow.Cells(7).Value) = 0 Then
                    get_MillCount_Details()
                End If
            End If

        End With


        If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 8 Or e.ColumnIndex = 9 Or e.ColumnIndex = 10 Then
            Yarn_Stock_Display(1)
        End If

    End Sub

    Private Sub dgv_YarnDetails_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellLeave
        With dgv_YarnDetails

            If e.ColumnIndex = 1 Or e.ColumnIndex = 2 Or e.ColumnIndex = 3 Then
                If Trim(UCase(vDGV_ENTRCEL_COUNTNAME)) <> Trim(UCase(.CurrentRow.Cells(1).Value)) Or Trim(UCase(vDGV_ENTRCEL_YARNTYPE)) <> Trim(UCase(.CurrentRow.Cells(2).Value)) Or Trim(UCase(vDGV_ENTRCEL_MILLNAME)) <> Trim(UCase(.CurrentRow.Cells(3).Value)) Then
                    get_MillCount_Details()
                End If
            End If

        End With
    End Sub

    Private Sub dgv_YarnDetails_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellValueChanged
        Try

            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

            With dgv_YarnDetails
                If .Visible Then

                    If e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 8 Then

                        If e.ColumnIndex = 5 Or e.ColumnIndex = 6 Then
                            If Val(.Rows(e.RowIndex).Cells(6).Value) <> 0 Then
                                .Rows(e.RowIndex).Cells(7).Value = Format(Val(.Rows(e.RowIndex).Cells(5).Value) / Val(.Rows(e.RowIndex).Cells(6).Value), "#########0.000")
                            End If
                        End If

                        If e.ColumnIndex = 6 Or e.ColumnIndex = 8 Then
                            .Rows(e.RowIndex).Cells(9).Value = Val(Val(.Rows(e.RowIndex).Cells(8).Value) * Val(.Rows(e.RowIndex).Cells(6).Value))
                        End If

                        If e.ColumnIndex = 5 Or e.ColumnIndex = 8 Then
                            .Rows(e.RowIndex).Cells(11).Value = Format(Val(.Rows(e.RowIndex).Cells(8).Value) * Val(.Rows(e.RowIndex).Cells(5).Value), "#########0.000")
                        End If

                    End If

                    If e.ColumnIndex = 7 Or e.ColumnIndex = 9 Or e.ColumnIndex = 10 Then
                        .Rows(e.RowIndex).Cells(11).Value = Format((Val(.Rows(e.RowIndex).Cells(9).Value) + Val(.Rows(e.RowIndex).Cells(10).Value)) * Val(.Rows(e.RowIndex).Cells(7).Value), "#########0.000")
                    End If

                    Total_Calculation()

                End If

            End With

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub dgv_YarnDetails_KeyDown(sender As Object, e As KeyEventArgs) Handles dgv_YarnDetails.KeyDown
        On Error Resume Next
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_YarnDetails_KeyUp(sender As Object, e As KeyEventArgs) Handles dgv_YarnDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_YarnDetails

                n = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(n)

                End If

                Total_Calculation()

            End With

        End If
    End Sub

    Private Sub dgv_YarnDetails_LostFocus(sender As Object, e As EventArgs) Handles dgv_YarnDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
        If cbo_Grid_CountName.Visible = False And cbo_Grid_MillName.Visible = False And cbo_Grid_YarnType.Visible = False Then
            pnl_YarnStock_Display.Visible = False
        End If
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgv_YarnDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

        With dgv_YarnDetails

            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
            .Rows(n - 1).Cells(2).Value = "MILL"

        End With
    End Sub



    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnDetails.EditingControlShowing
        dgtxt_YarnDetails = CType(dgv_YarnDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_YarnDetails_Enter(sender As Object, e As EventArgs) Handles dgtxt_YarnDetails.Enter
        If dgv_YarnDetails.CurrentCell.ColumnIndex = 4 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 5 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 6 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 7 Then
            Yarn_Stock_Display(1)
        End If

        dgtxt_YarnDetails.Tag = dgtxt_YarnDetails.Text
        dgv_ActiveCtrl_Name = dgv_YarnDetails.Name
        dgv_YarnDetails.EditingControl.BackColor = Color.Lime
        dgv_YarnDetails.EditingControl.ForeColor = Color.Blue
        dgv_YarnDetails.SelectAll()
    End Sub

    Private Sub dgtxt_YarnDetails_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dgtxt_YarnDetails.KeyPress
        With dgv_YarnDetails
            If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Then
                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_YarnDetails_KeyUp(sender As Object, e As KeyEventArgs) Handles dgtxt_YarnDetails.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_YarnDetails_KeyUp(sender, e)
        End If
    End Sub

    Private Sub txt_elongation_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_elongation.KeyDown
        If e.KeyCode = 38 Then
            'txt_pickup_2.Focus()
            txt_WarpLegnth.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_Beam_Width.Focus()
            txt_Ends_1.Focus()
        End If
    End Sub

    Private Sub txt_elongation_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_elongation.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            'txt_Beam_Width.Focus()
            txt_Ends_1.Focus()
        End If
    End Sub

    Private Sub txt_Remarks_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyCode = 38 Then
            txt_Beam_Width.Focus()
        End If
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub txt_pick_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_pickup_2.KeyDown
        If e.KeyCode = 38 Then
            txt_Pickup_1.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_elongation.Focus()
            cbo_Fabric_Name.Focus()
        End If
    End Sub

    Private Sub txt_pick_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_pickup_2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            'txt_elongation.Focus()
            cbo_Fabric_Name.Focus()
        End If
    End Sub
    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0
        PrntCnt2ndPageSTS = False
        prn_Prev_HeadIndx = -100
        prn_HeadIndx = 0
        prn_Count = 0
        'prn_Count1 = 0
        'cnt = 0
        prn_DetIndx = 0
        'prn_DetIndx1 = 0
        prn_DetSNo = 0
        'prn_PageCount = 0


        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, d.*,d.Ledger_MainName ,Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code , Lsh.State_Name as Ledger_State_Name from Sizing_JobCard_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo   Left Outer JOIN Ledger_Head d ON a.Ledger_IdNo = d.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON d.Ledger_State_IdNo = Lsh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_JobCard_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.*   from Sizing_JobCard_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_JobCard_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim ps As Printing.PaperSize
        Dim vConeStk As Long = 0
        Dim vWGTStk As String = 0


        'set_PaperSize_For_PrintDocument1()

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        PrntCnt = 1
        If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 40
            .Right = 45
            .Top = 40 '50 ' 60
            .Bottom = 40


            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9, FontStyle.Regular)

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
        ' If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) = "" Then
        NoofItems_PerPage = NoofItems_PerPage + 1
        'End If

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 35 : ClAr(2) = 130 : ClAr(3) = 170 : ClAr(4) = 85 : ClAr(5) = 85 : ClAr(6) = 90
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
            TxtHgt = 16 ' 18.25
        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Then
            TxtHgt = 17.5
        Else
            TxtHgt = 17
        End If

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 Then
                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    'prn_Tot_EBeam_Stk = 0
                    'prn_Tot_Pavu_Stk = 0
                    'prn_Tot_Yarn_Stk = 0
                    'prn_Tot_Amt_Bal = 0

                    TpMargin = TMargin

                Else

                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    'prn_Tot_EBeam_Stk = 0
                    'prn_Tot_Pavu_Stk = 0
                    'prn_Tot_Yarn_Stk = 0
                    'prn_Tot_Amt_Bal = 0

                    TpMargin = 580 + TMargin  ' 600 + TMargin

                End If

            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                    NoofItems_PerPage = 4
                    If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) = "" Then
                        NoofItems_PerPage = NoofItems_PerPage + 1
                    End If

                    If Val(Common_Procedures.settings.WeaverWagesYarnReceipt_Print_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > NoofItems_PerPage Then
                            NoofItems_PerPage = 35
                        End If
                    End If

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 35 Then
                                For I = 15 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            vConeStk = 0 ' Str(Val(prn_DetDt.Rows(prn_DetIndx).Item("")).ToString)
                            vWGTStk = 0

                            dtp_Date.Text = prn_HdDt.Rows(0).Item("Sizing_JobCard_Date").ToString
                            Yarn_Stock_Display(2, Val(prn_HdDt.Rows(0).Item("ledger_idno").ToString), Val(prn_DetDt.Rows(prn_DetIndx).Item("count_idno").ToString), Trim(prn_DetDt.Rows(prn_DetIndx).Item("yarn_type").ToString), Val(prn_DetDt.Rows(prn_DetIndx).Item("mill_idno").ToString), vConeStk, vWGTStk)

                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Per_Cone").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Weight_Per_Cone").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("No_Of_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, vConeStk, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

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
                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 6 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt

LOOP2:



        prn_Count = prn_Count + 1

        If Val(prn_TotCopies) > 1 Then

            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_DetSNo = 0
                prn_PageNo = 0

                e.HasMorePages = True
                Return

            Else
                e.HasMorePages = False
            End If

        Else

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


        End If

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim M1 As Single
        Dim S1 As Single
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Count_Typ As String = ""
        Dim Mill_Typ As String = ""
        Dim strWidth As Single = 0
        Dim CurX As Single = 0
        Dim Loom_Typ As String = ""
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Sizing_JobCard_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_JobCard_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GSTNo = ""

        Dim vADD_BOLD_STS As Boolean = False


        vADD_BOLD_STS = False
        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)

            Dim vADDR1 As String = ""

            vADDR1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
            vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) & ",", "")
            vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Cmp_Add2 = vADDR1 & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            vADD_BOLD_STS = True

        Else

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        End If


        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 3
        If vADD_BOLD_STS = True Then
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            strHeight = TxtHgt
        End If


        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value


        CurY = CurY + TxtHgt - 1
        If Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("company_State_Idno").ToString))) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " STATE : " & Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("company_State_Idno").ToString))) & "   " & " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("company_GSTinNo").ToString), LMargin + 10, CurY, 2, PrintWidth, pFont)
        End If
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo & "  /  " & Cmp_CstNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo, LMargin + 10, CurY, 0, 0, pFont)



        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "JOB ORDER FORM", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + 5
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then '---- SANTHA EXPORTS (SOMANUR)
        '    CurY = CurY + TxtHgt
        '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821 ( Textile manufactring service )", LMargin, CurY, 2, PrintWidth, p1Font)
        '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        'Else

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 10, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin + 10, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821 ( Textile manufactring service )", PageWidth - 10, CurY, 1, 0, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        'Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin, CurY, 2, PrintWidth, p1Font)

        'End If

        CurY = CurY + strHeight  ' + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        M1 = ClAr(1) + ClAr(2)


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont,, True)
        Common_Procedures.Print_To_PrintDocument(e, "JobCard.No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_JobCard_No").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont,, True)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sizing_JobCard_Date")), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont,, True)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1395" Then '---- SANTHA EXPORTS (SOMANUR)
            CurY = CurY + TxtHgt
            If Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Ledger_State_Idno").ToString))) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " STATE : " & Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Ledger_State_Idno").ToString))), LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
        End If

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
            strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, p1Font).Width
            CurX = LMargin + S1 + 10 + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY, 0, PrintWidth, pFont)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10


        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO OF BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO OF CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CONE STOCK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "CONE STOCK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 15, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "RECEIVED UPTO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY





        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        'If Val(prn_HdDt.Rows(0).Item("Wrap_Length").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Wrap Length", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Wrap_Length").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If
        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'If Val(prn_HdDt.Rows(0).Item("Sizing_Length").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Sizing Length", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Sizing_Length").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)

        'End If

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'If Val(prn_HdDt.Rows(0).Item("Beam_Length").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Beam Length", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Beam_Length").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If
        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'If Val(prn_HdDt.Rows(0).Item("Pick_Up").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Pick Up", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Pick_Up").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'If Val(prn_HdDt.Rows(0).Item("Elongation").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Elongation", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Elongation").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'If Val(prn_HdDt.Rows(0).Item("Beam_Requirement_Date").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Beam Requirement Date", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Beam_Requirement_Date")), "dd-MM-yyyy".ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If



        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'Common_Procedures.Print_To_PrintDocument(e, "Remarks :", LMargin + 10, CurY, 0, 0, p1Font)

        'CurY = CurY + TxtHgt

        'CurY = CurY + TxtHgt


        'Common_Procedures.Print_To_PrintDocument(e, "1.Check Mill, Count, Lot & Warp Ends", LMargin + 10, CurY, 0, 0, pFont)
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "2.Try to take max warp length; cut cone should be at 30 grms or less", LMargin + 10, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "3.If any cone found damage and yarn quality issues, please inform before", LMargin + 10, CurY, 0, 0, pFont)


        'CurY = CurY + TxtHgt + 5
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)






        'p1Font = New Font("Calibri", 12, FontStyle.Bold)

        'Common_Procedures.Print_To_PrintDocument(e, "Note :", LMargin + 20, CurY, 0, 0, pFont)
        'CurY = CurY + TxtHgt
        'CurY = CurY + TxtHgt
        'CurY = CurY + TxtHgt
        'CurY = CurY + TxtHgt



    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single
        Dim M1 As Single
        Dim Area_Nm As String = ""
        Dim LedAdd1 As String = ""
        Dim LedAdd2 As String = ""
        Dim LedAdd3 As String = ""
        Dim LedAdd4 As String = ""
        Dim Loom_Typ As String = ""
        Dim Count_Typ As String = ""
        Dim Mill_Typ As String = ""


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString(" Vehicle No :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY



        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 15, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

        End If

        CurY = CurY + TxtHgt
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





        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
        M1 = ClAr(1) + ClAr(2)

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SIZING PROGRAM :", LMargin + 10, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(4) = CurY
        Loom_Typ = Common_Procedures.LoomType_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Loom_Type_idno").ToString))
        If Val(prn_HdDt.Rows(0).Item("Loom_Type_Idno").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Loom Type", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Loom_Typ, LMargin + M1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        If Trim(prn_HdDt.Rows(0).Item("Fabric_Weave").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Fabric Width / Weave", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Fabric_Width").ToString) & prn_HdDt.Rows(0).Item("Fabric_Weave").ToString, LMargin + M1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        Common_Procedures.Print_To_PrintDocument(e, "Beam Length", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Beam_Length").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        Common_Procedures.Print_To_PrintDocument(e, "Beam Width", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Beam_Width").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        If Trim(prn_HdDt.Rows(0).Item("ends_name").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Ends", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("ends_name").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1266" Then

            If Trim(prn_HdDt.Rows(0).Item("Ends_Name_2").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Ends for single", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ends_Name_2").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
            End If

        Else

            If Val(prn_HdDt.Rows(0).Item("creel").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "No Of Creel  ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("creel").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
            End If
        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        Common_Procedures.Print_To_PrintDocument(e, "No Of Set", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("no_of_set").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        Common_Procedures.Print_To_PrintDocument(e, "No Of Cones Required", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("no_of_cones_required").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        If Val(prn_HdDt.Rows(0).Item("No_Of_Beams").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "No Of Beams", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("No_Of_Beams").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        If Val(prn_HdDt.Rows(0).Item("Beam_Requirement_Date").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Beam Requirement Date", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Beam_Requirement_Date")), "dd-MM-yyyy".ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        End If

        'p1Font = New Font("Calibri", 16, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "YARN :", LMargin + 10, CurY, 0, 0, p1Font)

        'CurY = CurY + TxtHgt + 10
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        ''LnAr(4) = CurY

        'Count_Typ = Common_Procedures.Count_IdNoToName(con, Val(prn_DetDt.Rows(0).Item("Count_IdNo").ToString))
        'If Val(prn_DetDt.Rows(0).Item("Count_IdNo").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Count", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Count_Typ, LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        ''LnAr(4) = CurY
        'If Trim(prn_DetDt.Rows(0).Item("Yarn_Type").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Type", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(0).Item("Yarn_Type").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If
        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        '' LnAr(4) = CurY

        'Mill_Typ = Common_Procedures.Mill_IdNoToName(con, Val(prn_DetDt.Rows(0).Item("Mill_IdNo").ToString))
        'If Val(prn_DetDt.Rows(0).Item("Mill_IdNo").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Mill", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Mill_Typ, LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        ''LnAr(4) = CurY
        'If Trim(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "No Of Cones", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        ''LnAr(4) = CurY
        'If Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "No Of Bags", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
    End Sub

    Private Sub btn_EMail_Click(sender As Object, e As EventArgs) Handles btn_EMail.Click
        Dim Led_IdNo As Integer
        Dim MailTxt As String

        Try
            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SizingName.Text)

            MailTxt = "INVOICE " & vbCrLf & vbCrLf
            MailTxt = MailTxt & "Invoice No.-" & Trim(lbl_JobNo.Text) & vbCrLf & "Date-" & Trim(dtp_Date.Text)
            ' MailTxt = MailTxt & vbCrLf & "Lr No.-" & Trim(txt_LrNo.Text) & IIf(Trim(msk_Lr_Date.Text) <> "", " Dt.", "") & Trim(msk_Lr_Date.Text)
            'MailTxt = MailTxt & vbCrLf & "Value-" & Trim(lbl_NetAmount.Text)

            EMAIL_Entry.vMailID = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            EMAIL_Entry.vSubJect = "Invocie : " & Trim(lbl_JobNo.Text)
            EMAIL_Entry.vMessage = Trim(MailTxt)

            Dim f1 As New EMAIL_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dtp_beam_date_ValueChanged(sender As Object, e As EventArgs) Handles dtp_beam_date.ValueChanged
        msk_Beam_Requirement_Date.Text = dtp_beam_date.Text
    End Sub

    Private Sub dtp_beam_date_Enter(sender As Object, e As EventArgs) Handles dtp_beam_date.Enter
        msk_Beam_Requirement_Date.Focus()
        msk_Beam_Requirement_Date.SelectionStart = 0
    End Sub

    Private Sub msk_Beam_Requirement_Date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_Beam_Requirement_Date.KeyPress
        If UCase(Chr(Asc(e.KeyChar))) = "D" Then
            msk_Beam_Requirement_Date.Text = Date.Today
            msk_Beam_Requirement_Date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            txt_No_of_set.Focus()
        End If
    End Sub

    Private Sub msk_Beam_Requirement_Date_KeyUp(sender As Object, e As KeyEventArgs) Handles msk_Beam_Requirement_Date.KeyUp
        Dim vmsRetTxt As String = ""
        Dim vmsRetvl As Integer = -1
        If IsDate(msk_Beam_Requirement_Date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_Beam_Requirement_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Beam_Requirement_Date.Text))
                msk_Beam_Requirement_Date.SelectionStart = 0
            ElseIf e.KeyCode = 109 Then
                msk_Beam_Requirement_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Beam_Requirement_Date.Text))
                msk_Beam_Requirement_Date.SelectionStart = 0
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub msk_Beam_Requirement_Date_LostFocus(sender As Object, e As EventArgs) Handles msk_Beam_Requirement_Date.LostFocus
        If IsDate(msk_Beam_Requirement_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Beam_Requirement_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Beam_Requirement_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Beam_Requirement_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Beam_Requirement_Date.Text)) >= 2000 Then
                    dtp_beam_date.Value = Convert.ToDateTime(msk_Beam_Requirement_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub msk_Beam_Requirement_Date_KeyDown(sender As Object, e As KeyEventArgs) Handles msk_Beam_Requirement_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Beam_Requirement_Date.Text
            vmskSelStrt = msk_Beam_Requirement_Date.SelectionStart
        End If
        If e.KeyCode = 40 Then
            txt_No_of_set.Focus()
        End If
        If e.KeyCode = 38 Then
            txt_beam_length.Focus()
        End If
    End Sub

    Private Sub dtp_beam_date_TextChanged(sender As Object, e As EventArgs) Handles dtp_beam_date.TextChanged
        If IsDate(dtp_beam_date.Text) = True Then
            msk_Beam_Requirement_Date.Text = dtp_beam_date.Text
            msk_Beam_Requirement_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub dtp_beam_date_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_beam_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dtp_beam_date_KeyUp(sender As Object, e As KeyEventArgs) Handles dtp_beam_date.KeyUp
        If e.KeyCode = 17 And e.Control = False And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_beam_date.Text = Date.Today
        End If
    End Sub

    Private Sub txt_No_of_set_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_No_of_set.KeyDown
        If e.KeyCode = 38 Then
            'txt_Ends_2.Focus()
            msk_Beam_Requirement_Date.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_DBF.Focus()
            txt_sizing_length.Focus()
        End If
    End Sub

    Private Sub txt_No_of_set_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_No_of_set.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'txt_DBF.Focus()
            txt_sizing_length.Focus()
        End If
    End Sub

    Private Sub No_OF_Cones_Required()
        Dim vnoOfConeValue As String = 0
        Dim vconeCalc As String = 0
        Dim vEnds As String = ""
        Dim vnoOfCreel As String = 0
        Dim vNoOfSet As String = 0

        vNoOfSet = txt_No_of_set.Text
        vEnds = txt_Ends_1.Text
        vnoOfCreel = txt_no_of_creel.Text

        vconeCalc = Val(vEnds) / Val(vnoOfCreel)

        If Trim(Common_Procedures.settings.CustomerCode) = "1464" Then ' --- MANI OMEGA
            vnoOfConeValue = Val(vconeCalc) / Val(vNoOfSet)
        Else
            vnoOfConeValue = Val(vconeCalc) * Val(vNoOfSet)
        End If
        lbl_no_of_cones_required.Text = Format(Val(vnoOfConeValue), "###########0.00")
    End Sub

    Private Sub txt_no_of_beams_TextChanged(sender As Object, e As EventArgs) Handles txt_no_of_beams.TextChanged
        No_OF_Cones_Required()
    End Sub

    Private Sub txt_no_of_creel_TextChanged(sender As Object, e As EventArgs) Handles txt_no_of_creel.TextChanged
        No_OF_Cones_Required()
    End Sub

    Private Sub txt_No_of_set_TextChanged(sender As Object, e As EventArgs) Handles txt_No_of_set.TextChanged
        No_OF_Cones_Required()
    End Sub

    Private Sub Yarn_Stock_Display(ByVal vENTRY_PRINT_STS As Integer, Optional ByVal vPRINT_SIZING_IDNO As Integer = 0, Optional ByVal vPRINT_COUNT_IDNO As Integer = 0, Optional ByVal vPRINT_YARN_TYPE As String = "", Optional ByVal vPRINT_MILL_IDNO As Integer = 0, Optional ByRef vPRINT_STOCK_CONES As Long = 0, Optional ByRef vPRINT_STOCK_WEIGHT As String = "")

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1438" Then Exit Sub

        If pnl_YarnStock_Display.Visible = True Then Exit Sub

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Cnt_IdNo As Integer = 0
        Dim MILL_IdNo As Integer = 0
        Dim CONT As String = ""
        Dim n As Integer, Sno As Integer
        Dim vLED_IDNo As Integer
        Dim NewCode As String = ""
        Dim NewPKCode As String = ""
        Dim vYRNTYPE As String = ""
        Dim vTOT_STK_CNS As Long = 0
        Dim vTOT_STK_WGT As String = 0
        Dim vOrdBy_JBNo As String = 0


        vLED_IDNo = 0
        Cnt_IdNo = 0
        MILL_IdNo = 0
        vYRNTYPE = ""

        vPRINT_STOCK_CONES = 0
        vPRINT_STOCK_WEIGHT = 0

        If Val(vENTRY_PRINT_STS) = 2 Then
            vLED_IDNo = vPRINT_SIZING_IDNO
            Cnt_IdNo = vPRINT_COUNT_IDNO
            MILL_IdNo = vPRINT_MILL_IDNO
            vYRNTYPE = vPRINT_YARN_TYPE

        Else

            Try
                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
            Catch ex As Exception
                Exit Sub
            End Try
            vLED_IDNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SizingName.Text)
            Cnt_IdNo = Val(Common_Procedures.Count_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value)))
            MILL_IdNo = Val(Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(3).Value)))
            vYRNTYPE = Trim(dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(2).Value)

        End If

        vOrdBy_JBNo = Format(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text)), "########0.00")

        If Trim(vYRNTYPE) = "" Then
            vYRNTYPE = "MILL"
        End If

        If vLED_IDNo = 0 Or Cnt_IdNo = 0 Then
            dgv_YarnStock.Rows.Clear()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        NewPKCode = Trim(Pk_Condition) & Trim(NewCode)

        CONT = " a.Count_IdNo = " & Val(Cnt_IdNo)
        If Val(MILL_IdNo) <> 0 Then
            CONT = Trim(CONT) & IIf(Trim(CONT) <> "", " and ", "") & " a.MIll_IdNo = " & Val(MILL_IdNo)
        End If
        If Trim(vYRNTYPE) <> "" Then
            CONT = Trim(CONT) & IIf(Trim(CONT) <> "", " and ", "") & " a.Yarn_Type = '" & Trim(vYRNTYPE) & "'"
        End If

        If Val(vENTRY_PRINT_STS) = 1 Then
            pnl_YarnStock_Display.Visible = True
            pnl_YarnStock_Display.BringToFront()
        End If

        Try

            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
            cmd.ExecuteNonQuery()

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@invdate", dtp_Date.Value.Date)

            '----YARN STOCK
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Name1, int1, int2, weight1 ) Select d.Mill_Name, sum(a.Bags), sum(a.Cones), sum(a.Weight) from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo   LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNo = d.Mill_IdNo  Where a.Company_IdNo = " & Val(lbl_Company.Tag) & "  and  a.Reference_Date <= @invdate and a.Weight <> 0 and a.DeliveryTo_Idno = " & Str(Val(vLED_IDNo)) & IIf(Trim(CONT) <> "", " and ", "") & Trim(CONT) & IIf(New_Entry = False, " and a.Reference_Code <> '" & Trim(NewPKCode) & "'", "") & " group by d.Mill_Name "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Name1, int1, int2, weight1) Select d.Mill_Name, -1*sum(a.Bags), -1*sum(a.Cones), -1*sum(a.Weight)  from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNo <> 0 and a.Mill_IdNo = d.Mill_IdNo Where a.Company_IdNo = " & Val(lbl_Company.Tag) & "  and a.Reference_Date <= @invdate and a.Weight <> 0 and a.ReceivedFrom_Idno = " & Str(Val(vLED_IDNo)) & IIf(Trim(CONT) <> "", " and ", "") & Trim(CONT) & IIf(New_Entry = False, " and a.Reference_Code <> '" & Trim(NewPKCode) & "'", "") & " Group by d.Mill_Name "
            cmd.ExecuteNonQuery()

            '----SPECIFICATION RECEIPT PENDING
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Name1, int3, weight3 ) Select d.Mill_Name, sum(a.No_Of_Cones), sum(a.Weight) from Sizing_JobCard_Details a INNER JOIN Sizing_JobCard_Head b ON a.Sizing_JobCard_Code = b.Sizing_JobCard_Code INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNo = d.Mill_IdNo  Where a.Company_IdNo = " & Val(lbl_Company.Tag) & "  and (a.Sizing_JobCard_Date < @invdate or ( a.Sizing_JobCard_Date = @invdate and a.for_orderby < " & Str(Format(Val(vOrdBy_JBNo), "########0.00")) & ") ) and a.Weight <> 0 and a.Ledger_Idno = " & Str(Val(vLED_IDNo)) & IIf(Trim(CONT) <> "", " and ", "") & Trim(CONT) & IIf(New_Entry = False, " and a.Sizing_JobCard_Code <> '" & Trim(NewCode) & "'", "") & " and b.Sizing_JobCode_forSelection NOT IN (SELECT SQ1.Sizing_JobCode_forSelection FROM Sizing_Specification_Head SQ1) group by d.Mill_Name "
            cmd.ExecuteNonQuery()

            da = New SqlClient.SqlDataAdapter("Select Name1 as Mill_Name  , sum(Int1) as bagstock, Sum(int2) as conestock, Sum(weight1)  as weightstock, Sum(int3) as cone_spec_pending, Sum(weight3) as weight_spec_pending from " & Trim(Common_Procedures.ReportTempSubTable) & " group by name1 having Sum(weight1)  <> 0 or Sum(weight3)  <> 0 ", con)
            dt = New DataTable
            da.Fill(dt)

            With dgv_YarnStock

                .Rows.Clear()
                Sno = 0
                vTOT_STK_CNS = 0
                vTOT_STK_WGT = 0
                If dt.Rows.Count > 0 Then

                    For i = 0 To dt.Rows.Count - 1

                        n = .Rows.Add()

                        Sno = Sno + 1
                        .Rows(n).Cells(0).Value = Val(Sno)
                        .Rows(n).Cells(1).Value = dt.Rows(n).Item("Mill_Name").ToString

                        .Rows(n).Cells(2).Value = dt.Rows(n).Item("conestock").ToString
                        If Val(.Rows(n).Cells(2).Value) = 0 Then .Rows(n).Cells(2).Value = ""
                        .Rows(n).Cells(3).Value = Format(Val(dt.Rows(n).Item("weightstock").ToString), "###########0.000")
                        If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(3).Value = ""

                        .Rows(n).Cells(4).Value = dt.Rows(n).Item("cone_spec_pending").ToString
                        If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""
                        .Rows(n).Cells(5).Value = Format(Val(dt.Rows(n).Item("weight_spec_pending").ToString), "###########0.000")
                        If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""

                        .Rows(n).Cells(6).Value = Val(.Rows(n).Cells(2).Value) - Val(.Rows(n).Cells(4).Value)
                        .Rows(n).Cells(7).Value = Format(Val(.Rows(n).Cells(3).Value) - Val(.Rows(n).Cells(5).Value), "###########0.000")

                        vTOT_STK_CNS = Val(vTOT_STK_CNS) + Val(.Rows(n).Cells(6).Value)
                        vTOT_STK_WGT = Format(Val(vTOT_STK_WGT) + Val(.Rows(n).Cells(7).Value), "###########0.000")


                    Next i

                End If

            End With
            dt.Clear()


            vPRINT_STOCK_CONES = Val(vTOT_STK_CNS)
            vPRINT_STOCK_WEIGHT = Format(Val(vTOT_STK_WGT), "###########0.000")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR YARN STOCK DISPLAY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Private Sub btn_Close_YarnStock_Display_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_YarnStock_Display.Click
        pnl_YarnStock_Display.Visible = False
    End Sub

    Private Sub txt_Beam_Width_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Beam_Width.KeyDown
        If e.KeyCode = 38 Then
            'txt_elongation.Focus()
            txt_sizing_length.Focus()
        End If
        If e.KeyCode = 40 Then
            txt_Remarks.Focus()
        End If
    End Sub

    Private Sub txt_Beam_Width_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Beam_Width.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Remarks.Focus()
        End If
    End Sub

    Private Sub txt_Ends_2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Ends_2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            'txt_No_of_set.Focus()
            txt_beam_length.Focus()
        End If
    End Sub

    Private Sub txt_Ends_2_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Ends_2.KeyDown
        If e.KeyCode = 38 Then
            txt_Ends_1.Focus()
        ElseIf e.KeyCode = 40 Then
            'txt_No_of_set.Focus()
            txt_beam_length.Focus()
        End If
    End Sub

    Private Sub txt_InvoicePrefixNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_InvoicePrefixNo.KeyPress

        If Asc(e.KeyChar) = 13 Then
            cbo_InvoiceSufixNo.Focus()
        End If

    End Sub

    Private Sub txt_InvoicePrefixNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_InvoicePrefixNo.KeyDown
        If e.KeyCode = 40 Then
            cbo_InvoiceSufixNo.Focus()
        End If
    End Sub

    Private Sub lbl_JobNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles lbl_JobNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_InvoiceSufixNo.Focus()
        End If
    End Sub

    Private Sub lbl_JobNo_KeyDown(sender As Object, e As KeyEventArgs) Handles lbl_JobNo.KeyDown
        If e.KeyCode = 40 Then
            cbo_InvoiceSufixNo.Focus()
        End If
        If e.KeyCode = 38 Then
            txt_InvoicePrefixNo.Focus()
        End If
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_InvoiceSufixNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_InvoiceSufixNo, dtp_Date, "", "", "", "")
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_InvoiceSufixNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_InvoiceSufixNo, txt_InvoicePrefixNo, dtp_Date, "", "", "", "")
    End Sub

    Private Sub cbo_InvoiceSufixNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_InvoiceSufixNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            'Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_InvoiceSufixNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            'f.MdiParent = MDIParent1
            'f.Show()

        End If
    End Sub

    Private Sub cbo_Kind_Attn_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Kind_Attn.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Kind_Attn, Nothing, "Sizing_JobCard_Head", "Kind_Attention_Person_Name", "", "(Ledger_IdNo = 0)", False)

        If Asc(e.KeyChar) = 13 Then
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub cbo_Kind_Attn_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Kind_Attn.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Kind_Attn, cbo_SizingName, Nothing, "Sizing_JobCard_Head", "Kind_Attention_Person_Name", "", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub cbo_Kind_Attn_GotFocus(sender As Object, e As EventArgs) Handles cbo_Kind_Attn.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_JobCard_Head", "Kind_Attention_Person_Name", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Fabric_Name_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Fabric_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Fabric_Name, txt_fabric_Weave, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Fabric_Name_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Fabric_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Fabric_Name, txt_pickup_2, txt_fabric_Weave, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Fabric_Name_GotFocus(sender As Object, e As EventArgs) Handles cbo_Fabric_Name.GotFocus
        cbo_Fabric_Name.Tag = cbo_Fabric_Name.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Fabric_Name_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Fabric_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Fabric_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_no_of_weaver_beam_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_no_of_weaver_beam.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_no_of_creel.Focus()
        End If
    End Sub

    Private Sub txt_no_of_weaver_beam_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_no_of_weaver_beam.KeyDown
        If e.KeyCode = 40 Then
            txt_no_of_creel.Focus()
        End If
        If e.KeyCode = 38 Then
            txt_no_of_beams.Focus()
        End If
    End Sub

    Private Sub Printing_Format2_1464(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer, j As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim TpMargin As Single = 0
        Dim ps As Printing.PaperSize
        Dim vConeStk As Long = 0
        Dim vWGTStk As String = 0

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim SNO As Integer = 0
        Dim N As Integer = 0
        Dim p1font As Font

        Dim vOp_bags As String = ""
        Dim vop_cones As String = ""
        Dim vop_wt As String = ""
        Dim Vop_Stk As String = ""
        Dim vLOOSECONES As Long = 0

        Dim Delv_Mill_Nm As String = "NIL"
        Dim vCn_Wgt As String = ""

        Dim vSide_Line As Single = 0
        Dim vSide_Line_2 As Single = 0
        Dim vSide_Line_3 As Single = 0
        Dim vSide_Line_4 As Single = 0
        Dim vSide_Line_5 As Single = 0
        Dim vSide_Line_6 As Single = 0

        Dim vBeam_Utl As String = ""
        Dim NoofBags As String = ""
        Dim NoofCOnes As String = ""
        Dim NoofLooseCones As String = ""
        Dim TotWgt As String = ""
        Dim TotEnds As String = ""

        Dim vOP_BAGStk As Long, vOP_ConeStk As Long, vOP_WGTStk As String
        Dim vDELV_BAGStk As Long, vDELV_ConeStk As Long, vDELV_WGTStk As String

        Dim vLine_Pen As Pen, vOUTERBORDERLine_Pen As Pen

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
            .Left = 30 ' 40
            .Right = 45
            .Top = 30 '40 '50 ' 60
            .Bottom = 40


            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        'pFont = New Font("Calibri", 9, FontStyle.Regular)

        vOUTERBORDERLine_Pen = New Pen(Color.Black, 2)
        vLine_Pen = New Pen(Color.Black, 2)

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
        ' If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) = "" Then
        NoofItems_PerPage = NoofItems_PerPage + 1
        'End If

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        TxtHgt = 15.75 ' 17 ' 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        TpMargin = TMargin


        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_1464_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofItems_PerPage = 4
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

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If





                        vOP_BAGStk = 0 : vOP_ConeStk = 0 : vOP_WGTStk = 0
                        vDELV_BAGStk = 0 : vDELV_ConeStk = 0 : vDELV_WGTStk = 0

                        get_YarnStock_Details(prn_HdDt.Rows(0).Item("Sizing_JobCard_Date"), Val(prn_HdDt.Rows(0).Item("ledger_idno").ToString), Val(prn_DetDt.Rows(prn_DetIndx).Item("count_idno").ToString), Trim(prn_DetDt.Rows(prn_DetIndx).Item("yarn_type").ToString), Val(prn_DetDt.Rows(prn_DetIndx).Item("mill_idno").ToString), vOP_BAGStk, vOP_ConeStk, vOP_WGTStk, vDELV_BAGStk, vDELV_ConeStk, vDELV_WGTStk)

                        CurY = CurY + TxtHgt


                        vOp_bags = ""
                        vop_cones = ""
                        vop_wt = ""
                        Vop_Stk = ""
                        vLOOSECONES = 0
                        If Val(vOP_WGTStk) <> 0 Then

                            vLOOSECONES = 0
                            vOP_BAGStk = 0
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString) <> 0 Then
                                If Val(vOP_ConeStk) <> 0 Then

                                    vOP_BAGStk = Val(vOP_ConeStk) \ Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString)

                                    vLOOSECONES = Val(vOP_ConeStk) - (Val(vOP_BAGStk) * Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString))
                                    If vLOOSECONES < 0 Then vLOOSECONES = 0
                                End If

                                'If Val(vOP_BAGStk) <> 0 And Val(vOP_ConeStk) <> 0 Then
                                '    vLOOSECONES = Val(vOP_ConeStk) - (Val(vOP_BAGStk) * Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString))
                                '    If vLOOSECONES < 0 Then vLOOSECONES = 0
                                'End If
                            End If

                            If Val(vOP_BAGStk) <> 0 Then
                                vOp_bags = Val(vOP_BAGStk) & " Bags"
                                If vLOOSECONES > 0 Then
                                    vOp_bags = vOp_bags & " + " & vLOOSECONES & " LooseCones"
                                End If
                            End If

                            If Val(vOP_ConeStk) <> 0 Then
                                vop_cones = "(" & Val(vOP_ConeStk) & " Cones)"
                            End If

                            vop_wt = " - " + Format(Val(vOP_WGTStk), "#########0.000") + " Kgs"

                            Vop_Stk = vOp_bags & " " & vop_cones & " " & vop_wt

                        Else

                            Vop_Stk = " Nil "

                        End If

                        Delv_Mill_Nm = ""
                        If Val(vDELV_WGTStk) <> 0 Then

                            vLOOSECONES = 0
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString) <> 0 Then
                                If Val(vDELV_BAGStk) <> 0 And Val(vDELV_ConeStk) <> 0 Then
                                    vLOOSECONES = Val(vDELV_ConeStk) - (Val(vDELV_BAGStk) * Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString))
                                    If vLOOSECONES < 0 Then vLOOSECONES = 0
                                End If
                            End If

                            If Val(vDELV_BAGStk) <> 0 Then
                                Delv_Mill_Nm = Val(vDELV_BAGStk) & " Bags"
                                If vLOOSECONES > 0 Then
                                    Delv_Mill_Nm = Delv_Mill_Nm & " + " & vLOOSECONES & " LooseCones"
                                End If
                            End If

                            If Val(vDELV_ConeStk) <> 0 Then
                                Delv_Mill_Nm = Delv_Mill_Nm & " (" & Val(vDELV_ConeStk) & " Cones)"
                            End If

                            Delv_Mill_Nm = Delv_Mill_Nm & " - " + Format(Val(vDELV_WGTStk), "#########0.000") + " Kgs"

                        Else

                            Delv_Mill_Nm = " Nil "

                        End If


                        Dim vCLOSING_STOCK As String = ""
                        Dim vCLOSING_BAGS As Long = 0
                        Dim vCLOSING_CONES As Long = 0
                        Dim vCLOSING_WEIGHT As String = 0

                        vCLOSING_STOCK = ""
                        vCLOSING_BAGS = vOP_BAGStk + vDELV_BAGStk
                        vCLOSING_CONES = vOP_ConeStk + vDELV_ConeStk
                        vCLOSING_WEIGHT = vOP_WGTStk + vDELV_WGTStk
                        If Val(vCLOSING_WEIGHT) <> 0 Then

                            vCLOSING_BAGS = 0
                            vLOOSECONES = 0
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString) <> 0 Then

                                vCLOSING_BAGS = Val(vCLOSING_CONES) \ Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString)

                                If Val(vCLOSING_BAGS) <> 0 And Val(vCLOSING_CONES) <> 0 Then
                                    vLOOSECONES = Val(vCLOSING_CONES) - (Val(vCLOSING_BAGS) * Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString))
                                    If vLOOSECONES < 0 Then vLOOSECONES = 0
                                End If

                            End If

                            If Val(vCLOSING_BAGS) <> 0 Then
                                vCLOSING_STOCK = Val(vCLOSING_BAGS) & " Bags"
                                If vLOOSECONES > 0 Then
                                    vCLOSING_STOCK = vCLOSING_STOCK & " + " & vLOOSECONES & " LooseCones"
                                End If
                            End If

                            If Val(vCLOSING_CONES) <> 0 Then
                                vCLOSING_STOCK = vCLOSING_STOCK & "(" & Val(vCLOSING_CONES) & " Cones)"
                            End If

                            vCLOSING_STOCK = vCLOSING_STOCK & " - " + Format(Val(vCLOSING_WEIGHT), "#########0.000") + " Kgs"

                        Else

                            vCLOSING_STOCK = " Nil "

                        End If


                        vCn_Wgt = ""
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("weight_per_cone").ToString) <> 0 Then
                            vCn_Wgt = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("weight_per_cone").ToString), "##########0.000") + " Kgs"
                        End If

                        Dim vDESP_DETAILS As String = ""

                        vDESP_DETAILS = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString) & "     " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("LOT_NO").ToString) <> "" Then
                            vDESP_DETAILS = Trim(vDESP_DETAILS) & "     (LOT NO - " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("LOT_NO").ToString) & ")"
                        End If

                        CurY = CurY + TxtHgt + 5
                        p1font = New Font("Calibri", 12, FontStyle.Bold)
                        Common_Procedures.Print_To_PrintDocument(e, "MATERIAL DISPATCH  :  " & Trim(vDESP_DETAILS), LMargin + 10, CurY, 0, 0, p1font)


                        vSide_Line = LMargin + 60
                        vSide_Line_2 = PageWidth - 35

                        ClAr(1) = 40 : ClAr(2) = 150
                        ClAr(3) = vSide_Line_2 - (vSide_Line + ClAr(1) + ClAr(2))

                        CurY = CurY + TxtHgt + 5
                        e.Graphics.DrawLine(vLine_Pen, vSide_Line, CurY, vSide_Line_2, CurY)
                        LnAr(2) = CurY

                        CurY = CurY + TxtHgt - 5
                        Common_Procedures.Print_To_PrintDocument(e, " 1 ", vSide_Line + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " Opening Stock ", vSide_Line + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Vop_Stk, vSide_Line + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, p1font)

                        CurY = CurY + TxtHgt + 5
                        e.Graphics.DrawLine(vLine_Pen, vSide_Line, CurY, vSide_Line_2, CurY)

                        CurY = CurY + TxtHgt - 5
                        Common_Procedures.Print_To_PrintDocument(e, " 2 ", vSide_Line + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " Delivery From Mill ", vSide_Line + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Delv_Mill_Nm, vSide_Line + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, p1font)

                        CurY = CurY + TxtHgt + 5
                        e.Graphics.DrawLine(vLine_Pen, vSide_Line, CurY, vSide_Line_2, CurY)

                        CurY = CurY + TxtHgt - 5
                        Common_Procedures.Print_To_PrintDocument(e, " 3 ", vSide_Line + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " Cone Weight ", vSide_Line + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, vCn_Wgt, vSide_Line + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, p1font)

                        CurY = CurY + TxtHgt + 5
                        e.Graphics.DrawLine(vLine_Pen, vSide_Line, CurY, vSide_Line_2, CurY)

                        CurY = CurY + TxtHgt - 5
                        Common_Procedures.Print_To_PrintDocument(e, " 4 ", vSide_Line + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " Total Weight ", vSide_Line + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, vCLOSING_STOCK, vSide_Line + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, p1font)

                        CurY = CurY + TxtHgt + 5
                        e.Graphics.DrawLine(vLine_Pen, vSide_Line, CurY, vSide_Line_2, CurY)
                        LnAr(3) = CurY

                        e.Graphics.DrawLine(vLine_Pen, vSide_Line, LnAr(2), vSide_Line, CurY)
                        e.Graphics.DrawLine(vLine_Pen, vSide_Line + ClAr(1), LnAr(2), vSide_Line + ClAr(1), CurY)
                        e.Graphics.DrawLine(vLine_Pen, vSide_Line + ClAr(1) + ClAr(2), LnAr(2), vSide_Line + ClAr(1) + ClAr(2), CurY)
                        e.Graphics.DrawLine(vLine_Pen, vSide_Line_2, LnAr(2), vSide_Line_2, CurY)

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                    CurY = CurY + TxtHgt - 5
                    p1font = New Font("Calibri", 12, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, " Empty Beam Dispatch : ", LMargin + 10, CurY, 0, 0, p1font)


                    vSide_Line_3 = LMargin + 60
                    vSide_Line_4 = PageWidth - 35

                    ClAr(1) = 45 : ClAr(2) = 150 : ClAr(3) = 150 : ClAr(4) = 150
                    ClAr(5) = vSide_Line_4 - (vSide_Line_3 + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4))

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_4, CurY)
                    LnAr(4) = CurY

                    CurY = CurY + TxtHgt - 5
                    Common_Procedures.Print_To_PrintDocument(e, " SNo ", vSide_Line_3, CurY, 2, ClAr(1), pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " Particulars ", vSide_Line_3 + ClAr(1), CurY, 2, ClAr(2), pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " Opening Stock ", vSide_Line_3 + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " Dispatch ", vSide_Line_3 + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " Total Stock ", vSide_Line_3 + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_4, CurY)
                    CurY = CurY - 10

                    Dim vBM_OPSTK As Long, vBM_DELVSTK As Long, vBM_BALSTK As Long

                    SNO = 0

                    vBM_OPSTK = 0 : vBM_DELVSTK = 0 : vBM_BALSTK = 0

                    da1 = New SqlClient.SqlDataAdapter("Select * from LoomType_Head Where LoomType_IdNo <> 0 and LoomType_Name <> '' and EmptyBeam_StockMaintenance_Status = 1 order by LoomType_Name", con)
                    'da1 = New SqlClient.SqlDataAdapter("Select * from Beam_Width_Head order by Beam_Width_Name", con)
                    dt1 = New DataTable
                    da1.Fill(dt1)
                    For j = 0 To dt1.Rows.Count - 1

                        SNO = SNO + 1

                        If j <> 0 Then
                            CurY = CurY + TxtHgt + 5
                            e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_4, CurY)
                            CurY = CurY - 10
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Val(SNO), vSide_Line_3 + 15, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(dt1.Rows(j).Item("LoomType_Name").ToString), vSide_Line_3 + ClAr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(dt1.Rows(j).Item("Beam_Width_Name").ToString), vSide_Line_3 + ClAr(1) + 10, CurY, 0, 0, pFont)

                        If vBM_OPSTK <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, vBM_OPSTK & " Nos", vSide_Line_3 + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                        End If

                        If vBM_DELVSTK <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, vBM_DELVSTK & " Nos", vSide_Line_3 + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        End If

                        vBM_BALSTK = vBM_OPSTK + vBM_DELVSTK
                        If vBM_BALSTK <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, vBM_BALSTK & " Nos", vSide_Line_3 + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        End If

                    Next j

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_4, CurY)
                    LnAr(5) = CurY

                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, LnAr(4), vSide_Line_3, CurY)
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3 + ClAr(1), LnAr(4), vSide_Line_3 + ClAr(1), CurY)
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3 + ClAr(1) + ClAr(2), LnAr(4), vSide_Line_3 + ClAr(1) + ClAr(2), CurY)
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3 + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4), vSide_Line_3 + ClAr(1) + ClAr(2) + ClAr(3), CurY)
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3 + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4), vSide_Line_3 + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY)
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_4, LnAr(4), vSide_Line_4, CurY)

                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " PROGRAM-1 :", LMargin + 10, CurY, 0, 0, p1font)

                    vSide_Line_5 = PageWidth - 35

                    ClAr(1) = 200
                    ClAr(2) = vSide_Line_5 - (vSide_Line_3 + ClAr(1))

                    vBeam_Utl = ""

                    If Val(prn_HdDt.Rows(0).Item("Total_weight").ToString) <> 0 Then

                        If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                            vBeam_Utl = Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) & " Bags"
                        End If

                        If Val(prn_HdDt.Rows(0).Item("Total_Loose_Cones").ToString) <> 0 Then
                            vBeam_Utl = Trim(vBeam_Utl) & " + " & Val(prn_HdDt.Rows(0).Item("Total_Loose_Cones").ToString) & " LooseCones "
                        End If


                        If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                            vBeam_Utl = Trim(vBeam_Utl) & "  (" & Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Loose_Cones").ToString) & " Cones) "
                        End If
                        'If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                        '    vBeam_Utl = Trim(vBeam_Utl) & "  (" & Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) & " Cones) "
                        'End If

                        vBeam_Utl = Trim(vBeam_Utl) & " - " & Format(Val(prn_HdDt.Rows(0).Item("Total_weight").ToString), "##########0.000") & " Kgs"

                        End If


                        TotEnds = ""
                    If Val(prn_HdDt.Rows(0).Item("ends_name").ToString) <> 0 Then
                        TotEnds = Val(prn_HdDt.Rows(0).Item("ends_name").ToString) & " Ends "
                    End If
                    If Val(prn_HdDt.Rows(0).Item("No_of_set").ToString) <> 0 Then
                        TotEnds = TotEnds & "  (" & Val(prn_HdDt.Rows(0).Item("No_of_set").ToString) & " Beams Set)  "
                    End If
                    If Val(prn_HdDt.Rows(0).Item("creel").ToString) <> 0 Then
                        TotEnds = TotEnds & "  " & Val(prn_HdDt.Rows(0).Item("creel").ToString) & " Creel"
                    End If


                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_5, CurY)
                    LnAr(6) = CurY

                    CurY = CurY + TxtHgt - 5
                    Common_Procedures.Print_To_PrintDocument(e, " Bags to be utilized ", vSide_Line_3 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, vBeam_Utl, vSide_Line_3 + ClAr(1) + 10, CurY, 0, 0, p1font)

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_5, CurY)

                    CurY = CurY + TxtHgt - 5
                    Common_Procedures.Print_To_PrintDocument(e, " Total Ends/Beam ", vSide_Line_3 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, TotEnds, vSide_Line_3 + ClAr(1) + 10, CurY, 0, 0, p1font)

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_5, CurY)

                    CurY = CurY + TxtHgt - 5
                    Common_Procedures.Print_To_PrintDocument(e, " Set Length ", vSide_Line_3 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Wrap_Length").ToString, vSide_Line_3 + ClAr(1) + 10, CurY, 0, 0, p1font)

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_5, CurY)

                    CurY = CurY + TxtHgt - 5
                    Common_Procedures.Print_To_PrintDocument(e, " Beams to be used ", vSide_Line_3 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.LoomType_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Loom_Type_Idno").ToString))), vSide_Line_3 + ClAr(1) + 10, CurY, 0, 0, p1font)

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_5, CurY)

                    CurY = CurY + TxtHgt - 5
                    Common_Procedures.Print_To_PrintDocument(e, " No of Weaver's Beam ", vSide_Line_3 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("No_of_Weaver_Beam").ToString, vSide_Line_3 + ClAr(1) + 10, CurY, 0, 0, p1font)

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_5, CurY)

                    CurY = CurY + TxtHgt - 5
                    Common_Procedures.Print_To_PrintDocument(e, " Mtrs/Beam ", vSide_Line_3 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Beam_Length").ToString, vSide_Line_3 + ClAr(1) + 10, CurY, 0, 0, p1font)

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_5, CurY)

                    CurY = CurY + TxtHgt - 5
                    Common_Procedures.Print_To_PrintDocument(e, " DBF in Inches ", vSide_Line_3 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DBF").ToString, vSide_Line_3 + ClAr(1) + 10, CurY, 0, 0, p1font)

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_5, CurY)

                    CurY = CurY + TxtHgt - 5
                    Common_Procedures.Print_To_PrintDocument(e, " Due Date ", vSide_Line_3 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Beam_Requirement_Date").ToString), "dd-MM-yyyy").ToString, vSide_Line_3 + ClAr(1) + 10, CurY, 0, 0, p1font)

                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, CurY, vSide_Line_5, CurY)
                    LnAr(7) = CurY

                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3, LnAr(6), vSide_Line_3, CurY)
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_3 + ClAr(1), LnAr(6), vSide_Line_3 + ClAr(1), CurY)
                    e.Graphics.DrawLine(vLine_Pen, vSide_Line_5, LnAr(6), vSide_Line_5, CurY)

                End If

                Printing_Format2_1464_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



LOOP2:

        prn_Count = prn_Count + 1

        If Val(prn_TotCopies) > 1 Then


            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_DetSNo = 0
                prn_PageNo = 0

                e.HasMorePages = True
                Return

            Else
                e.HasMorePages = False
            End If

        Else

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


        End If

    End Sub


    Private Sub Printing_Format2_1464_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTNo As String
        Dim strHeight As Single
        'Dim C1 As Single
        'Dim W1 As Single
        'Dim M1 As Single
        'Dim S1 As Single
        'Dim Gst_dt As Date
        'Dim Entry_dt As Date
        Dim Count_Typ As String = ""
        Dim Mill_Typ As String = ""
        Dim strWidth As Single = 0
        Dim CurX As Single = 0
        Dim Loom_Typ As String = ""
        PageNo = PageNo + 1



        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Sizing_JobCard_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_JobCard_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GSTNo = ""

        Dim vADD_BOLD_STS As Boolean = False


        vADD_BOLD_STS = False

        p1Font = New Font("Calibri", 10, FontStyle.Regular)

        p1Font = New Font("Calibri", 10, FontStyle.Bold)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Ref.No : " & prn_HdDt.Rows(0).Item("Sizing_JobCard_No").ToString, LMargin + 10, CurY, 0, 0, p1Font)


        Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sizing_JobCard_Date")), "dd-MM-yyyy").ToString, PageWidth - 20, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 15
        Common_Procedures.Print_To_PrintDocument(e, "To : ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 40, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 40, CurY, 0, 0, p1Font)
        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 40, CurY, 0, 0, p1Font)
        End If
        If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString), LMargin + 40, CurY, 0, 0, p1Font)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "PAN NO : " & Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString), LMargin + 40, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "KIND ATTN : " & Trim(prn_HdDt.Rows(0).Item("Kind_Attention_Person_Name").ToString), LMargin, CurY, 2, PageWidth, p1Font)

        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "Dear Sir, ", LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Please arrange to do sizing job work according to the specification mentioned below.  ", LMargin + 10, CurY, 0, 0, pFont)

    End Sub

    Private Sub Printing_Format2_1464_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim Area_Nm As String = ""
        Dim LedAdd1 As String = ""
        Dim LedAdd2 As String = ""
        Dim LedAdd3 As String = ""
        Dim LedAdd4 As String = ""
        Dim Loom_Typ As String = ""
        Dim Count_Typ As String = ""
        Dim Mill_Typ As String = ""


        CurY = CurY + TxtHgt + 10

        p1Font = New Font("Calibri", 14, FontStyle.Bold Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, " Construction : ", LMargin + 10, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.Cloth_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("cloth_idno").ToString))), LMargin + 150, CurY + 2, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "We are sending the above material which has to be warped & Sized And the beams should be suitable to the above", LMargin + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "construction in ", LMargin + 25, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.LoomType_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Loom_Type_Idno").ToString))), LMargin + 130, CurY + 1, 0, 0, p1Font)


        CurY = CurY + TxtHgt + 10
        p1Font = New Font("Calibri", 14, FontStyle.Bold Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "Note : ", LMargin + 10, CurY, 0, 0, p1Font)

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "1. Please match the sized Beam pair as per our Beam No painted in our Beams. We are giving", LMargin + 25, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "    Beam No for each pair. Like 1A+1B, 2A+2B, 3A+3B .....", LMargin + 25, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "2. Kindly Return the cutcones every set along with sized beam delivery.", LMargin + 25, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "3. Sizing Material should be easily removable during desizing process.Use of PV PVA A,based", LMargin + 25, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     sizing material,any whitening agent or acrylic based sizing is strictly prohibited.", LMargin + 25, CurY, 0, 0, p1Font)


        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        CurY = CurY + TxtHgt + 15
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 20, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt + 15
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 20, CurY, 1, 0, p1Font)

    End Sub

    Private Sub get_YarnStock_Details(ByVal vPRINT_OPDATE As Date, ByVal vPRINT_SIZING_IDNO As Integer, ByVal vPRINT_COUNT_IDNO As Integer, ByVal vPRINT_YARN_TYPE As String, ByVal vPRINT_MILL_IDNO As Integer, ByRef vPRINT_STOCK_BAGS As Long, ByRef vPRINT_STOCK_CONES As Long, ByRef vPRINT_STOCK_WEIGHT As String, ByRef vPRINT_DELV_BAGS As Long, ByRef vPRINT_DELV_CONES As Long, ByRef vPRINT_DELV_WEIGHT As String)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Cnt_IdNo As Integer = 0
        Dim MILL_IdNo As Integer = 0
        Dim CONT As String = "", vDELVCONDT As String = ""
        Dim vSELC_JOBCODE As String
        Dim n As Integer
        Dim vLED_IDNo As Integer
        Dim NewCode As String = ""
        Dim NewPKCode As String = ""
        Dim vYRNTYPE As String = ""
        Dim vTOT_STK_BGS As Long = 0
        Dim vTOT_STK_CNS As Long = 0
        Dim vTOT_STK_WGT As String = 0
        Dim vOrdBy_JBNo As String = 0


        vPRINT_STOCK_BAGS = 0
        vPRINT_STOCK_CONES = 0
        vPRINT_STOCK_WEIGHT = 0


        vPRINT_DELV_BAGS = 0
        vPRINT_DELV_CONES = 0
        vPRINT_DELV_WEIGHT = 0

        vLED_IDNo = vPRINT_SIZING_IDNO
        Cnt_IdNo = vPRINT_COUNT_IDNO
        MILL_IdNo = vPRINT_MILL_IDNO
        vYRNTYPE = vPRINT_YARN_TYPE


        vOrdBy_JBNo = Format(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text)), "########0.00")

        If Trim(vYRNTYPE) = "" Then
            vYRNTYPE = "MILL"
        End If

        If vLED_IDNo = 0 Or Cnt_IdNo = 0 Then
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        NewPKCode = Trim(Pk_Condition) & Trim(NewCode)

        CONT = " a.Count_IdNo = " & Val(Cnt_IdNo)
        If Val(MILL_IdNo) <> 0 Then
            CONT = Trim(CONT) & IIf(Trim(CONT) <> "", " and ", "") & " a.MIll_IdNo = " & Val(MILL_IdNo)
        End If
        If Trim(vYRNTYPE) <> "" Then
            CONT = Trim(CONT) & IIf(Trim(CONT) <> "", " and ", "") & " a.Yarn_Type = '" & Trim(vYRNTYPE) & "'"
        End If

        vSELC_JOBCODE = Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

        vDELVCONDT = Trim(CONT) & IIf(Trim(CONT) <> "", " and ", "") & " a.Sizing_JobCode_forSelection = '" & Trim(vSELC_JOBCODE) & "'"

        CONT = Trim(CONT) & IIf(Trim(CONT) <> "", " and ", "") & " a.Sizing_JobCode_forSelection <> '" & Trim(vSELC_JOBCODE) & "'"


        Try

            cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@invdate", vPRINT_OPDATE)

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
            cmd.ExecuteNonQuery()

            '----YARN STOCK
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1, int2, weight1) Select sum(a.Bags), sum(a.Cones), sum(a.Weight)           from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  Where a.Company_IdNo = " & Val(lbl_Company.Tag) & "  and  a.Reference_Date <= @invdate and a.Weight <> 0 and a.DeliveryTo_Idno = " & Str(Val(vLED_IDNo)) & IIf(Trim(CONT) <> "", " and ", "") & Trim(CONT)
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1, int2, weight1) Select -1*sum(a.Bags), -1*sum(a.Cones), -1*sum(a.Weight)  from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  Where a.Company_IdNo = " & Val(lbl_Company.Tag) & "  and a.Reference_Date <= @invdate and a.Weight <> 0 and a.ReceivedFrom_Idno = " & Str(Val(vLED_IDNo)) & IIf(Trim(CONT) <> "", " and ", "") & Trim(CONT)
            cmd.ExecuteNonQuery()

            da = New SqlClient.SqlDataAdapter("Select sum(Int1) as bagstock, Sum(int2) as conestock, Sum(weight1) as weightstock, Sum(int3) as cone_spec_pending, Sum(weight3) as weight_spec_pending from " & Trim(Common_Procedures.ReportTempSubTable) & " having Sum(weight1)  <> 0 ", con)
            dt = New DataTable
            da.Fill(dt)

            vTOT_STK_BGS = 0
            vTOT_STK_CNS = 0
            vTOT_STK_WGT = 0
            If dt.Rows.Count > 0 Then
                vTOT_STK_BGS = Val(dt.Rows(n).Item("bagstock").ToString)
                vTOT_STK_CNS = Val(dt.Rows(n).Item("conestock").ToString)
                vTOT_STK_WGT = Format(Val(dt.Rows(n).Item("conestock").ToString), "###########0.000")
            End If
            dt.Clear()

            vPRINT_STOCK_BAGS = Val(vTOT_STK_BGS)
            vPRINT_STOCK_CONES = Val(vTOT_STK_CNS)
            vPRINT_STOCK_WEIGHT = Format(Val(vTOT_STK_WGT), "###########0.000")


            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
            cmd.ExecuteNonQuery()

            '----YARN DELIVERY to this job
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1, int2, weight1) Select sum(a.Bags), sum(a.Cones), sum(a.Weight)           from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  Where a.Company_IdNo = " & Val(lbl_Company.Tag) & "  and  a.Reference_Date <= @invdate and a.Weight <> 0 and a.DeliveryTo_Idno = " & Str(Val(vLED_IDNo)) & IIf(Trim(vDELVCONDT) <> "", " and ", "") & Trim(vDELVCONDT)
            cmd.ExecuteNonQuery()

            da = New SqlClient.SqlDataAdapter("Select sum(Int1) as bagstock, Sum(int2) as conestock, Sum(weight1) as weightstock, Sum(int3) as cone_spec_pending, Sum(weight3) as weight_spec_pending from " & Trim(Common_Procedures.ReportTempSubTable) & " having Sum(weight1)  <> 0 ", con)
            dt = New DataTable
            da.Fill(dt)
            vTOT_STK_BGS = 0
            vTOT_STK_CNS = 0
            vTOT_STK_WGT = 0
            If dt.Rows.Count > 0 Then
                vTOT_STK_BGS = Val(dt.Rows(n).Item("bagstock").ToString)
                vTOT_STK_CNS = Val(dt.Rows(n).Item("conestock").ToString)
                vTOT_STK_WGT = Format(Val(dt.Rows(n).Item("conestock").ToString), "###########0.000")
            End If
            dt.Clear()

            vPRINT_DELV_BAGS = Val(vTOT_STK_BGS)
            vPRINT_DELV_CONES = Val(vTOT_STK_CNS)
            vPRINT_DELV_WEIGHT = Format(Val(vTOT_STK_WGT), "###########0.000")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR YARN STOCK...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Private Sub dgtxt_YarnDetails_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_YarnDetails.TextChanged
        Try
            With dgv_YarnDetails

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_YarnDetails.Text)
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

    Private Sub get_Fabric_Details()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vCLOWIDTH As String
        Dim vCLOWeave As String
        Dim vCLOID As Integer

        If Trim(cbo_Fabric_Name.Text) = "" Then Exit Sub

        vCLOID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Fabric_Name.Text)

        vCLOWIDTH = "" : vCLOWeave = ""

        If vCLOID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select Cloth_Width, Weave from cloth_head where cloth_idno = " & Str(Val(vCLOID)), con)
            Dt = New DataTable
            Da.Fill(Dt)
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    vCLOWIDTH = Dt.Rows(0).Item("Cloth_Width").ToString
                    vCLOWeave = Dt.Rows(0).Item("Weave").ToString
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            txt_fabric_Weave.Text = vCLOWeave
            txt_Fabric_Width.Text = vCLOWIDTH

        End If

    End Sub

    Private Sub cbo_Fabric_Name_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Fabric_Name.SelectedIndexChanged

    End Sub

    Private Sub cbo_Fabric_Name_LostFocus(sender As Object, e As EventArgs) Handles cbo_Fabric_Name.LostFocus
        If Trim(cbo_Fabric_Name.Text) <> "" Then
            If Trim(UCase(cbo_Fabric_Name.Tag)) <> Trim(UCase(cbo_Fabric_Name.Text)) Then
                get_Fabric_Details()
            End If
        End If
    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_Enter(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_LotNo.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Lot_Head", "LotCode_forSelection", "(Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & Trim(dgv_YarnDetails.CurrentRow.Cells(1).Value) & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & Trim(dgv_YarnDetails.CurrentRow.Cells(3).Value) & "'))", "(Lot_No = '')")
    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_LotNo.TextChanged
        Try
            If cbo_Grid_Yarn_LotNo.Visible Then

                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

                With dgv_YarnDetails
                    If Val(cbo_Grid_Yarn_LotNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(4).Value = Trim(cbo_Grid_Yarn_LotNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Yarn_LotNo, Nothing, Nothing, "Yarn_Lot_Head", "LotCode_forSelection", "(Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & Trim(dgv_YarnDetails.CurrentRow.Cells(1).Value) & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & Trim(dgv_YarnDetails.CurrentRow.Cells(3).Value) & "'))", "(Lot_No = '')")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_Yarn_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Yarn_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Yarn_LotNo, Nothing, "Yarn_Lot_Head", "LotCode_forSelection", "(Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & Trim(dgv_YarnDetails.CurrentRow.Cells(1).Value) & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & Trim(dgv_YarnDetails.CurrentRow.Cells(3).Value) & "'))", "(Lot_No = '')")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End With

        End If

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyUp

        If e.Control = False And e.KeyValue = 17 Then

            Dim f As New Yarn_Lot_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Yarn_LotNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

End Class