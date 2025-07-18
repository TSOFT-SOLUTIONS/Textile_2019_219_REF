Public Class Beam_Knotting_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "BKNOT-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
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

        pnl_back.Enabled = True
        pnl_filter.Visible = False
        pnl_Selection.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        dtp_Date.Text = ""
        msk_Date.Text = ""
        cbo_Shift.Text = Common_Procedures.Shift_IdNoToName(con, 1)
        cbo_PartyName.Text = ""
        cbo_PartyName.Tag = ""
        cbo_ClothName1.Text = ""
        cbo_ClothName1.Tag = ""
        cbo_ClothName2.Text = ""
        cbo_ClothName2.Tag = ""
        cbo_ClothName3.Text = ""
        cbo_ClothName3.Tag = ""
        cbo_ClothName4.Text = ""
        cbo_ClothName4.Tag = ""

        lbl_Cloth_Name1.Text = ""
        lbl_Cloth_Name2.Text = ""
        lbl_Cloth_Name3.Text = ""
        lbl_Cloth_Name4.Text = ""

        lbl_WidthType.Text = ""

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        cbo_KnotterName.Text = ""
        txt_WagesAmount.Text = ""
        lbl_MillName.Text = ""
        lbl_WarpLotNo.Text = ""


        cbo_EndsCount.Text = ""

        cbo_KnotterName.Text = ""
        cbo_WidthType.Text = "SINGLE"
        cbo_ClothSales_OrderNo.Text = ""

        lbl_SetNo1.Text = ""
        lbl_Meters1.Text = ""
        lbl_Meters2.Text = ""
        lbl_SetNo2.Text = ""
        cbo_LoomNo.Text = ""
        cbo_LoomNo.Tag = ""
        lbl_BeamNo1.Text = ""
        lbl_BeamNo2.Text = ""
        lbl_EndsCount_Beam1.Text = ""
        lbl_EndsCount_Beam2.Text = ""
        txt_Shiftmetrs.Text = ""

        cbo_ClothSales_OrderNo_Quality_2.Text = ""
        cbo_ClothSales_OrderNo_Quality_3.Text = ""
        cbo_ClothSales_OrderNo_Quality_4.Text = ""


        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_ClothName1.Enabled = True
        cbo_ClothName1.BackColor = Color.White
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            cbo_WidthType.Enabled = False
            cbo_ClothName1.Enabled = False
            cbo_ClothName2.Enabled = False
            cbo_ClothName3.Enabled = False
            cbo_ClothName4.Enabled = False
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRIC

            lbl_Panel_1.Text = "A"
            lbl_Panel_2.Text = "B"
            lbl_Panel_3.Text = "C"
            lbl_Panel_4.Text = "D"

        Else

            lbl_Panel_1.Text = "1"
            lbl_Panel_2.Text = "2"
            lbl_Panel_3.Text = "3"
            lbl_Panel_4.Text = "4"

        End If

        lbl_Weaver_Job_No.Text = ""

        cbo_EndsCount.Enabled = True
        cbo_EndsCount.BackColor = Color.White

        cbo_LoomNo.Enabled = True
        cbo_LoomNo.BackColor = Color.White

        btn_Selection.Enabled = True

        btn_Save_ShiftMeters.Visible = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
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
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
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
        If Not IsNothing(dgv_filter.CurrentCell) Then dgv_filter.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_Name, e.Cloth_Name as Cloth_Name2, f.Cloth_Name as Cloth_Name3, g.Cloth_Name as Cloth_Name4, d.Loom_Name from Beam_Knotting_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Display_Cloth_Idno1 = c.Cloth_IdNo LEFT OUTER JOIN Cloth_Head e ON a.Display_Cloth_Idno2 = e.Cloth_IdNo LEFT OUTER JOIN Cloth_Head f ON a.Display_Cloth_Idno3 = f.Cloth_IdNo LEFT OUTER JOIN Cloth_Head g ON a.Display_Cloth_Idno4 = g.Cloth_IdNo LEFT OUTER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Beam_Knotting_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Beam_Knotting_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Beam_Knotting_Date").ToString
                msk_Date.Text = dtp_Date.Text

                cbo_Shift.Text = Common_Procedures.Shift_IdNoToName(con, Val(dt1.Rows(0).Item("Shift_IdNo").ToString))
                If Trim(cbo_Shift.Text) = "" Then
                    cbo_Shift.Text = dt1.Rows(0).Item("Shift").ToString
                End If

                cbo_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_ClothName1.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                If dt1.Rows(0).Item("Cloth_Name2").ToString <> "" Then
                    cbo_ClothName2.Text = dt1.Rows(0).Item("Cloth_Name2").ToString
                End If
                If dt1.Rows(0).Item("Cloth_Name3").ToString <> "" Then
                    cbo_ClothName3.Text = dt1.Rows(0).Item("Cloth_Name3").ToString
                End If
                If dt1.Rows(0).Item("Cloth_Name4").ToString <> "" Then
                    cbo_ClothName4.Text = dt1.Rows(0).Item("Cloth_Name4").ToString
                End If

                lbl_Cloth_Name1.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_Idno1").ToString))
                lbl_Cloth_Name2.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_Idno2").ToString))
                lbl_Cloth_Name3.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_Idno3").ToString))
                lbl_Cloth_Name4.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_Idno4").ToString))

                cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, dt1.Rows(0).Item("EndsCount_IdNo").ToString)
                cbo_LoomNo.Text = dt1.Rows(0).Item("Loom_Name").ToString

                cbo_WidthType.Text = dt1.Rows(0).Item("Display_Width_Type").ToString
                cbo_ClothSales_OrderNo.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString
                lbl_Weaver_Job_No.Text = dt1.Rows(0).Item("Weaving_JobCode_forSelection").ToString

                lbl_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString

                lbl_SetCode1.Text = dt1.Rows(0).Item("Set_Code1").ToString
                lbl_SetCode2.Text = dt1.Rows(0).Item("Set_Code2").ToString
                lbl_SetNo1.Text = dt1.Rows(0).Item("Set_No1").ToString
                lbl_SetNo2.Text = dt1.Rows(0).Item("Set_No2").ToString
                lbl_BeamNo1.Text = dt1.Rows(0).Item("Beam_No1").ToString
                lbl_BeamNo2.Text = dt1.Rows(0).Item("Beam_No2").ToString
                lbl_EndsCount_Beam1.Text = Common_Procedures.EndsCount_IdNoToName(con, Val(dt1.Rows(0).Item("EndsCount1_IdNo").ToString))
                lbl_EndsCount_Beam2.Text = Common_Procedures.EndsCount_IdNoToName(con, Val(dt1.Rows(0).Item("EndsCount2_IdNo").ToString))
                lbl_Meters1.Text = dt1.Rows(0).Item("Beam_Meters1").ToString
                cbo_KnotterName.Text = Common_Procedures.Employee_IdNoToName(con, Val(dt1.Rows(0).Item("Employee_IdNo").ToString))
                txt_WagesAmount.Text = dt1.Rows(0).Item("Wages_Amount").ToString

                lbl_MillName.Text = Common_Procedures.Mill_IdNoToName(con, Val(dt1.Rows(0).Item("Mill_IdNo").ToString))
                lbl_WarpLotNo.Text = dt1.Rows(0).Item("Warp_LotNo").ToString

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                If Val(lbl_Meters1.Text) = 0 Then
                    lbl_Meters1.Text = ""
                End If
                lbl_Meters2.Text = dt1.Rows(0).Item("Beam_Meters2").ToString
                If Val(lbl_Meters2.Text) = 0 Then
                    lbl_Meters2.Text = ""
                End If


                txt_Shiftmetrs.Text = dt1.Rows(0).Item("Shift_Meters").ToString
                cbo_ClothSales_OrderNo_Quality_2.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_2").ToString
                cbo_ClothSales_OrderNo_Quality_3.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_3").ToString
                cbo_ClothSales_OrderNo_Quality_4.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_4").ToString

                lbl_Panel_1.Text = dt1.Rows(0).Item("Panel_1").ToString
                lbl_Panel_2.Text = dt1.Rows(0).Item("Panel_2").ToString
                lbl_Panel_3.Text = dt1.Rows(0).Item("Panel_3").ToString
                lbl_Panel_4.Text = dt1.Rows(0).Item("Panel_4").ToString

                LockSTS = False
                If IsDBNull(dt1.Rows(0).Item("Production_Meters").ToString) = False Then
                    If Val(dt1.Rows(0).Item("Production_Meters").ToString) <> 0 Then
                        LockSTS = True
                    End If
                End If

                If IsDBNull(dt1.Rows(0).Item("Beam_RunOut_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Beam_RunOut_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                If IsDBNull(dt1.Rows(0).Item("Sort_Change_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Sort_Change_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If



                If LockSTS = True Then
                    cbo_PartyName.Enabled = False
                    cbo_PartyName.BackColor = Color.LightGray

                    'cbo_WidthType.Enabled = False
                    'cbo_WidthType.BackColor = Color.LightGray

                    'cbo_ClothName1.Enabled = False
                    'cbo_ClothName1.BackColor = Color.LightGray

                    'cbo_ClothName2.Enabled = False
                    'cbo_ClothName2.BackColor = Color.LightGray

                    'cbo_ClothName3.Enabled = False
                    'cbo_ClothName3.BackColor = Color.LightGray

                    cbo_EndsCount.Enabled = False
                    cbo_EndsCount.BackColor = Color.LightGray

                    cbo_LoomNo.Enabled = False
                    cbo_LoomNo.BackColor = Color.LightGray

                    btn_Selection.Enabled = False

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                        btn_Save_ShiftMeters.Visible = True
                    End If

                End If

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            da1.Dispose()
            dt1.Dispose()
            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()
            ' If dtp_date.Visible And dtp_date.Enabled Then dtp_date.Focus()

        End Try

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LoomNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_LoomNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Beam_Knotting_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("SINGLE")
        cbo_WidthType.Items.Add("DOUBLE")
        cbo_WidthType.Items.Add("TRIPLE")
        cbo_WidthType.Items.Add("FOURTH")
        cbo_WidthType.Items.Add("FIVE")
        cbo_WidthType.Items.Add("SIX")
        cbo_WidthType.Items.Add("SEVEN")
        cbo_WidthType.Items.Add("EIGHT")
        cbo_WidthType.Items.Add("NINE")
        cbo_WidthType.Items.Add("TEN")

        lbl_ClothSales_OrderNo_Caption.Visible = False
        lbl_ClothSales_OrderNo_Caption_Star.Visible = False
        cbo_ClothSales_OrderNo.Visible = False
        cbo_EndsCount.Width = cbo_PartyName.Width

        lbl_ClothSales_OrderNo_Caption_2.Visible = False
        cbo_ClothSales_OrderNo_Quality_2.Visible = False

        lbl_ClothSales_OrderNo_Caption_3.Visible = False
        cbo_ClothSales_OrderNo_Quality_3.Visible = False

        lbl_ClothSales_OrderNo_Caption_4.Visible = False
        cbo_ClothSales_OrderNo_Quality_4.Visible = False


        lbl_Panel_1.Visible = False
        lbl_Panel_2.Visible = False
        lbl_Panel_3.Visible = False
        lbl_Panel_4.Visible = False

        LABEL50.Visible = False
        LABEL51.Visible = False
        LABEL53.Visible = False
        LABEL54.Visible = False

        lbl_Weaver_Job_No.Visible = False
        lbl_Weaver_Job_No_Caption.Visible = False



        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
            lbl_ClothSales_OrderNo_Caption.Visible = True
            lbl_ClothSales_OrderNo_Caption_Star.Visible = True
            cbo_ClothSales_OrderNo.Visible = True
            cbo_EndsCount.Width = cbo_ClothName1.Width

            cbo_KnotterName.Visible = False
            lbl_KnotterName_Caption.Visible = True
            lbl_MillName.Visible = True
            lbl_MillName.BackColor = Color.FromArgb(255, 255, 192) ' Color.White
            lbl_MillName.Left = cbo_KnotterName.Left
            lbl_MillName.Top = cbo_KnotterName.Top
            lbl_MillName.Width = cbo_KnotterName.Width
            lbl_KnotterName_Caption.Text = "Mill Name"

            txt_WagesAmount.Visible = False
            lbl_WagesAmount_Caption.Visible = True
            lbl_WarpLotNo.Visible = True
            lbl_WarpLotNo.BackColor = Color.FromArgb(255, 255, 192) '  Color.White
            lbl_WarpLotNo.Left = txt_WagesAmount.Left
            lbl_WarpLotNo.Top = txt_WagesAmount.Top
            lbl_WarpLotNo.Width = txt_WagesAmount.Width
            lbl_WagesAmount_Caption.Text = "Warp Lot No."

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                btn_Save_ShiftMeters.Visible = True
                lbl_shft_metrs_Caption.Visible = True
                txt_Shiftmetrs.Visible = True
                cbo_Shift.Width = lbl_RefNo.Width
            End If


            cbo_EndsCount.Width = cbo_WidthType.Width

            lbl_ClothSales_OrderNo_Caption_2.Visible = True
            cbo_ClothSales_OrderNo_Quality_2.Visible = True

            lbl_ClothSales_OrderNo_Caption_3.Visible = True
            cbo_ClothSales_OrderNo_Quality_3.Visible = True

            lbl_ClothSales_OrderNo_Caption_4.Visible = True
            cbo_ClothSales_OrderNo_Quality_4.Visible = True


            lbl_Panel_1.Visible = True
            lbl_Panel_2.Visible = True
            lbl_Panel_3.Visible = True
            lbl_Panel_4.Visible = True

            LABEL50.Visible = True
            LABEL51.Visible = True
            LABEL53.Visible = True
            LABEL54.Visible = True


        Else
            '  cbo_Shift.Width = cbo_PartyName.Width
            cbo_Shift.Size = New Size(556, 23)
            cbo_PartyName.Width = cbo_ClothName1.Width
            cbo_EndsCount.Width = cbo_WidthType.Width

            cbo_ClothName1.Width = cbo_Shift.Width
            cbo_ClothName2.Width = cbo_Shift.Width
            cbo_ClothName3.Width = cbo_Shift.Width
            cbo_ClothName4.Width = cbo_Shift.Width

        End If

        If Common_Procedures.settings.Cloth_WarpConsumption_Multiple_EndsCount_Status = 1 Then '---- SOTEXPA QUALIDIS TEXTILE (SULUR)
            cbo_EndsCount.Visible = False
            lbl_EndsCount_Caption.Visible = False
            lbl_EndsCount_star.Visible = False

            cbo_PartyName.Width = cbo_ClothName1.Width


        Else

            dgv_Selection.Columns(8).Visible = False
            dgv_Selection.Columns(1).Width = dgv_Selection.Columns(1).Width + (dgv_Selection.Columns(8).Width / 4)
            dgv_Selection.Columns(2).Width = dgv_Selection.Columns(2).Width + (dgv_Selection.Columns(8).Width / 4)
            dgv_Selection.Columns(3).Width = dgv_Selection.Columns(3).Width + (dgv_Selection.Columns(8).Width / 4)
            dgv_Selection.Columns(4).Width = dgv_Selection.Columns(4).Width + (dgv_Selection.Columns(8).Width / 4)

        End If

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        If Common_Procedures.settings.Show_Weaver_JobCard_Entry_STATUS = 1 Then

            lbl_Weaver_Job_No.Visible = True
            lbl_Weaver_Job_No_Caption.Visible = True

            lbl_Weaver_Job_No_Caption.Left = lbl_shft_metrs_Caption.Left
            lbl_Weaver_Job_No.Left = txt_Shiftmetrs.Left
            lbl_Weaver_Job_No.Width = cbo_EndsCount.Width

            If lbl_Weaver_Job_No.Visible And lbl_Weaver_Job_No_Caption.Visible Then
                cbo_Shift.Width = cbo_PartyName.Width
            End If

        End If


        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName1.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_ClothName2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName3.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName4.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_KnotterName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SetNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_KnotterName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WagesAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_BeamNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_BeamNo.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName3.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SetNoSelection.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_LoomNo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_KnotterName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WagesAmount.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler dtp_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler dtp_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_Shiftmetrs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Shiftmetrs.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_ClothSales_OrderNo_Quality_2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderNo_Quality_2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderNo_Quality_3.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderNo_Quality_3.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderNo_Quality_4.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderNo_Quality_4.LostFocus, AddressOf ControlLostFocus



        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub


    Private Sub Beam_Knotting_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
        con.Dispose()
    End Sub

    Private Sub Beam_Knotting_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim NewCode As String = ""
        Dim DelvSts As Integer = 0
        Dim Nr As Integer = 0
        Dim NoofKnotBmsInCD As Integer = 0
        Dim NoofKnotBmsInLom As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Inhouse_Beam_knotting_Entry, New_Entry, Me, con, "Beam_Knotting_Head", "Beam_Knotting_Code", NewCode, "Beam_Knotting_Date", "(Beam_Knotting_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub








        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Beam_Knotting_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Beam_Knotting_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            Dim vBEAMKnot_ProdMeters As String = ""

            vBEAMKnot_ProdMeters = Common_Procedures.get_BeamKnotting_TotalProductionMeters_from_Doffing(con, Trim(NewCode))
            If Val(vBEAMKnot_ProdMeters) <> 0 Then
                MessageBox.Show("Invalid : Already Production entered after this knotting", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
            vBEAMKnot_ProdMeters = Common_Procedures.get_BeamKnotting_TotalProductionMeters_from_PieceChecking(con, Trim(NewCode))
            If Val(vBEAMKnot_ProdMeters) <> 0 Then
                MessageBox.Show("Invalid : Already Production entered after this knotting", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            'If IsDBNull(Dt1.Rows(0).Item("Production_Meters").ToString) = False Then
            '    If Val(Dt1.Rows(0).Item("Production_Meters").ToString) <> 0 Then
            '        MessageBox.Show("Invalid : Already Production entered after this knotting", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            '        Exit Sub
            '    End If
            'End If

            If IsDBNull(Dt1.Rows(0).Item("Beam_RunOut_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Beam_RunOut_Code").ToString) <> "" Then
                    MessageBox.Show("Invalid : Already this knotting, was runout", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            If IsDBNull(Dt1.Rows(0).Item("Sort_Change_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Sort_Change_Code").ToString) <> "" Then
                    MessageBox.Show("Invalid : Already this knotting, was Sort Changed", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            'If Trim(Dt1.Rows(0).Item("Set_Code1").ToString) <> "" And Trim(Dt1.Rows(0).Item("Beam_No1").ToString) <> "" Then
            '    Da = New SqlClient.SqlDataAdapter("Select Close_Status from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No1").ToString) & "'", con)
            '    Dt2 = New DataTable
            '    Da.Fill(Dt2)
            '    If Dt2.Rows.Count > 0 Then
            '        If IsDBNull(Dt2.Rows(0).Item("Close_Status").ToString) = False Then
            '            If Val(Dt2.Rows(0).Item("Close_Status").ToString) <> 0 Then
            '                Throw New ApplicationException("Invalid Editing : Already this Beams was Closed")
            '                Exit Sub
            '            End If
            '        End If
            '    End If
            '    Dt2.Clear()
            'End If

            'If Trim(Dt1.Rows(0).Item("Set_Code2").ToString) <> "" And Trim(Dt1.Rows(0).Item("Beam_No2").ToString) <> "" Then
            '    Da = New SqlClient.SqlDataAdapter("Select Close_Status from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No2").ToString) & "'", con)
            '    Dt2 = New DataTable
            '    Da.Fill(Dt2)
            '    If Dt2.Rows.Count > 0 Then
            '        If IsDBNull(Dt2.Rows(0).Item("Close_Status").ToString) = False Then
            '            If Val(Dt2.Rows(0).Item("Close_Status").ToString) <> 0 Then
            '                Throw New ApplicationException("Invalid Editing : Already this Beams was Closed")
            '                Exit Sub
            '            End If
            '        End If
            '    End If
            '    Dt2.Clear()
            'End If

        End If
        Dt1.Clear()

        tr = con.BeginTransaction

        Try
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Beam_Knotting_Head", "Beam_Knotting_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Beam_Knotting_Code, Company_IdNo, for_OrderBy", tr)

            cmd.Connection = con
            cmd.Transaction = tr

            Lm_ID = 0
            Da = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                Lm_ID = Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)

                Nr = 0
                cmd.CommandText = "Update Loom_Head Set Beam_Knotting_Code = '' Where Loom_IdNo = " & Str(Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
                Nr = cmd.ExecuteNonQuery
                'If Nr = 0 Then
                '    Throw New ApplicationException("Invalid Editing : Already this loom was knotted again")
                '    Exit Sub
                'End If

                If Trim(Dt1.Rows(0).Item("Set_Code1").ToString) <> "" And Trim(Dt1.Rows(0).Item("Beam_No1").ToString) <> "" Then
                    Nr = 0
                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '', Loom_Idno = 0 ,Weaving_JobCode_forSelection = '' From Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No1").ToString) & "' and Loom_Idno = " & Str(Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
                    'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '', Loom_Idno = 0 From Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No1").ToString) & "' and Loom_Idno = " & Str(Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "' and Close_Status = 0"
                    Nr = cmd.ExecuteNonQuery
                    'If Nr = 0 Then
                    '    Throw New ApplicationException("Invalid Editing : Already this Beams is running in another loom (or) Closed")
                    '    Exit Sub
                    'End If

                End If

                If Trim(Dt1.Rows(0).Item("Set_Code2").ToString) <> "" And Trim(Dt1.Rows(0).Item("Beam_No2").ToString) <> "" Then
                    Nr = 0
                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '', Loom_Idno = 0 ,Weaving_JobCode_forSelection = '' From Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No2").ToString) & "' and Loom_Idno = " & Str(Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
                    'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '', Loom_Idno = 0 From Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No2").ToString) & "' and Loom_Idno = " & Str(Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "' and Close_Status = 0"
                    Nr = cmd.ExecuteNonQuery
                    'If Nr = 0 Then
                    '    Throw New ApplicationException("Invalid Editing : Already this Beams is running in another loom (or) Closed")
                    '    Exit Sub
                    'End If
                End If

            End If
            Dt1.Clear()

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)

            cmd.CommandText = "delete from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'NoofKnotBmsInCD = 0
            'Da = New SqlClient.SqlDataAdapter("Select count(*) from Stock_SizedPavu_Processing_Details where Beam_Knotting_Code = '" & Trim(NewCode) & "'", con)
            'Da.SelectCommand.Transaction = tr
            'Dt2 = New DataTable
            'Da.Fill(Dt2)
            'If Dt2.Rows.Count > 0 Then
            '    If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
            '        NoofKnotBmsInCD = Val(Dt2.Rows(0)(0).ToString)
            '    End If
            'End If
            'Dt2.Clear()

            'If Val(NoofKnotBmsInCD) <> 0 Then
            '    Throw New ApplicationException("Invalid Knotting for this Code")
            '    Exit Sub
            'End If

            'NoofKnotBmsInLom = 0
            'Da = New SqlClient.SqlDataAdapter("Select count(*) from Stock_SizedPavu_Processing_Details where Loom_IdNo = " & Str(Val(Lm_ID)), con)
            'Da.SelectCommand.Transaction = tr
            'Dt2 = New DataTable
            'Da.Fill(Dt2)
            'If Dt2.Rows.Count > 0 Then
            '    If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
            '        NoofKnotBmsInLom = Val(Dt2.Rows(0)(0).ToString)
            '    End If
            'End If
            'Dt2.Clear()

            'If Val(NoofKnotBmsInLom) <> 0 Then
            '    Throw New ApplicationException("Invalid Knotting for this Loom")
            '    Exit Sub
            'End If

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()
        ' If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()

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

            da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
            da.Fill(dt2)
            cbo_Filter_LoomNo.DataSource = dt2
            cbo_Filter_LoomNo.DisplayMember = "Loom_Name"

            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt3)
            cbo_Filter_ClothName.DataSource = dt3
            cbo_Filter_ClothName.DisplayMember = "Cloth_Name"

            dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
            dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
            pnl_filter.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.SelectedIndex = -1
            cbo_Filter_LoomNo.SelectedIndex = -1
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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Beam_Knotting_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Beam_Knotting_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Inhouse_Beam_knotting_Entry, New_Entry, Me) = False Then Exit Sub





        Try

            inpno = InputBox("Enter New Ref.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Beam_Knotting_No from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
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
                    MessageBox.Show("Invalid Ref.No", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

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
            cmd.CommandText = "select top 1 Beam_Knotting_No from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Beam_Knotting_No"
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
            cmd.CommandText = "select top 1 Beam_Knotting_No from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Beam_Knotting_No desc"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Beam_Knotting_No from Beam_Knotting_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Beam_Knotting_No"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Beam_Knotting_No from Beam_Knotting_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Beam_Knotting_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Beam_Knotting_No desc"
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

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_RefNo.Text = NewID
            lbl_RefNo.ForeColor = Color.Red
            msk_Date.Text = Date.Today.ToShortDateString


            ' dtp_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Beam_Knotting_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                    If dt1.Rows(0).Item("Beam_Knotting_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Beam_Knotting_Date").ToString
                End If
            End If
            dt1.Clear()
            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
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

        Try

            inpno = InputBox("Enter Ref.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Beam_Knotting_No from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Ref.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim NewCode As String = ""
        Dim nr As Long = 0
        Dim led_id As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Clo_ID2 As Integer = 0
        Dim Clo_ID3 As Integer = 0
        Dim Clo_ID4 As Integer = 0
        Dim vMULTI_ENDSCNT_SELC_STS As String = 0
        Dim Disp_Clo_ID As Integer = 0
        Dim Disp_Clo_ID2 As Integer = 0
        Dim Disp_Clo_ID3 As Integer = 0
        Dim Disp_Clo_ID4 As Integer = 0
        Dim vSHFTIDNO As Integer
        Dim vMILL_id As Integer
        Dim EdsCnt_ID As Integer = 0, vEdsCnt1_ID As Integer = 0, vEdsCnt2_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim NoofInpBmsInLom As Integer = 0
        Dim NoofKnotBmsInCD As Integer = 0
        Dim NoofKnotBmsInLom As Integer = 0
        Dim Emp_id As Integer = 0
        Dim CR_id As Integer = 0
        Dim DR_id As Integer = 0
        Dim vOrdByNo As String = ""
        Dim vWeav_JobCode_Forselec As String = ""
        Dim lckdt As Date = Now
        Dim dat As Date = Now
        Dim vSELC_KNOTCODE As String = ""



        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1075-" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1377-" Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1377-" Then '---- KURINJHI WEAVING MILLS (PALLADAM) 
                lckdt = #10/10/2022#
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1075-" Then '---- JR Textiles (Somanur) Stantly
                lckdt = #1/30/2025#
            End If

            If IsDate(Common_Procedures.settings.Sdd) = True Then
                dat = Common_Procedures.settings.Sdd
            End If

            If DateDiff("d", lckdt.ToShortDateString, dat.ToShortDateString) > 0 Then
                MessageBox.Show("Run-time error '463': " & Chr(13) & Chr(13) & "Class not registered on local machine", "DOES Not SAVE", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                Me.Close()
                Application.Exit()
            End If

        End If

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Inhouse_Beam_knotting_Entry, New_Entry, Me, con, "Beam_Knotting_Head", "Beam_Knotting_Code", NewCode, "Beam_Knotting_Date", "(Beam_Knotting_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Beam_Knotting_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Inhouse_Beam_Knotting_Entry, New_Entry) = False Then Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        da = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'", con)
        dt1 = New DataTable
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0).Item("Sort_Change_Code").ToString) = False Then
                If Trim(dt1.Rows(0).Item("Sort_Change_Code").ToString) <> "" Then
                    MessageBox.Show("Invalid : Already this knotting, was Sort Changed", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        dt1.Clear()

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

        vSHFTIDNO = Common_Procedures.Shift_NameToIdNo(con, cbo_Shift.Text)

        If txt_Shiftmetrs.Visible = True Then
            If vSHFTIDNO = 0 Then
                MessageBox.Show("Invalid Shift Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Shift.Enabled Then cbo_Shift.Focus()
                Exit Sub
            End If
            If Val(txt_Shiftmetrs.Text) <= 0 Then
                MessageBox.Show("Invalid Shift-Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Shiftmetrs.Enabled Then txt_Shiftmetrs.Focus()
                Exit Sub
            End If
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            If Trim(cbo_WidthType.Text) = "" Then
                MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_WidthType.Enabled Then cbo_WidthType.Focus()
                Exit Sub
            End If
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
            If cbo_ClothSales_OrderNo.Visible Then
                If Trim(cbo_ClothSales_OrderNo.Text) = "" Then
                    MessageBox.Show("Invalid Sales Order No.", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_ClothSales_OrderNo.Enabled Then cbo_ClothSales_OrderNo.Focus()
                    Exit Sub
                End If
            End If
        End If

        Dim vSortCngCode As String = ""

        vSortCngCode = Common_Procedures.get_FieldValue(con, "Beam_Knotting_Head", "Sort_Change_Code", "(Beam_Knotting_Code = '" & Trim(NewCode) & "')")

        If Trim(vSortCngCode) <> "" Then
            If Trim(lbl_WidthType.Text) = "" Then
                lbl_WidthType.Text = Trim(cbo_WidthType.Text)
            End If
        Else
            lbl_WidthType.Text = Trim(cbo_WidthType.Text)
        End If


        Disp_Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName1.Text)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            If Disp_Clo_ID = 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName1.Enabled Then cbo_ClothName1.Focus()
                Exit Sub
            End If
        End If


        Disp_Clo_ID2 = 0
        If Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Or Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Or Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Or Trim(UCase(cbo_WidthType.Text)) = "FIVE" Or Trim(UCase(cbo_WidthType.Text)) = "SIX" Then
            Disp_Clo_ID2 = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName2.Text)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
                If Disp_Clo_ID2 = 0 Then
                    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_ClothName2.Enabled Then cbo_ClothName2.Focus()
                    Exit Sub
                End If
            End If
        End If

        Disp_Clo_ID3 = 0
        If Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Or Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Or Trim(UCase(cbo_WidthType.Text)) = "FIVE" Or Trim(UCase(cbo_WidthType.Text)) = "SIX" Then
            Disp_Clo_ID3 = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName3.Text)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
                If Disp_Clo_ID3 = 0 Then
                    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_ClothName3.Enabled Then cbo_ClothName3.Focus()
                    Exit Sub
                End If
            End If

        End If
        Disp_Clo_ID4 = 0
        If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Or Trim(UCase(cbo_WidthType.Text)) = "FIVE" Or Trim(UCase(cbo_WidthType.Text)) = "SIX" Then
            Disp_Clo_ID4 = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName4.Text)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
                If Disp_Clo_ID4 = 0 Then
                    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_ClothName4.Enabled Then cbo_ClothName4.Focus()
                    Exit Sub
                End If
            End If
        End If

        Clo_ID = Disp_Clo_ID
        'Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth_Name1.Text)
        'If Clo_ID = 0 Then
        '    Clo_ID = Disp_Clo_ID
        'End If

        Clo_ID2 = Disp_Clo_ID2
        'Clo_ID2 = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth_Name2.Text)
        'If Clo_ID2 = 0 Then
        '    Clo_ID2 = Disp_Clo_ID2
        'End If

        Clo_ID3 = Disp_Clo_ID3
        'Clo_ID3 = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth_Name3.Text)
        'If Clo_ID3 = 0 Then
        '    Clo_ID3 = Disp_Clo_ID3
        'End If

        Clo_ID4 = Disp_Clo_ID4
        'Clo_ID4 = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth_Name4.Text)
        'If Clo_ID4 = 0 Then
        '    Clo_ID4 = Disp_Clo_ID4
        'End If

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Lm_ID = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If


        EdsCnt_ID = 0
        If cbo_EndsCount.Visible = True Then
            EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
            If EdsCnt_ID = 0 Then
                MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_EndsCount.Enabled Then cbo_EndsCount.Focus()
                Exit Sub
            End If
        End If

        vEdsCnt1_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount_Beam1.Text)
        If vEdsCnt1_ID = 0 Then
            If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
                MessageBox.Show("Invalid Ends/Count for Beam1", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If
        End If
        If Val(vEdsCnt1_ID) <> 0 Then
            da = New SqlClient.SqlDataAdapter("select * from Cloth_EndsCount_Details Where Cloth_Idno = " & Str(Val(Disp_Clo_ID)) & " and EndsCount_IdNo = " & Str(Val(vEdsCnt1_ID)), con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count = 0 Then
                MessageBox.Show("Mismatch of EndsCount-1 with Cloth Master", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If
            dt1.Clear()
        End If

        If cbo_EndsCount.Visible = False Then
            EdsCnt_ID = vEdsCnt1_ID
        Else
            If EdsCnt_ID <> vEdsCnt1_ID Then
                MessageBox.Show("Mismatch of EndsCount with Beam1-EndsCount", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_EndsCount.Visible And cbo_EndsCount.Enabled Then cbo_EndsCount.Focus()
                Exit Sub
            End If
        End If

        vEdsCnt2_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount_Beam2.Text)
        If vEdsCnt2_ID = 0 Then
            If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                MessageBox.Show("Invalid Ends/Count for Beam2", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If
        End If

        If Val(vEdsCnt2_ID) <> 0 Then
            da = New SqlClient.SqlDataAdapter("select * from Cloth_EndsCount_Details Where Cloth_Idno = " & Str(Val(Disp_Clo_ID)) & " and EndsCount_IdNo = " & Str(Val(vEdsCnt2_ID)), con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count = 0 Then
                MessageBox.Show("Mismatch of EndsCount-2 with Cloth Master", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If
            dt1.Clear()
        End If

        Dim vCLTHID As Integer
        Dim vEDSCNTID As Integer
        For I = 1 To 8

            vCLTHID = Choose(I, Disp_Clo_ID, Disp_Clo_ID, Disp_Clo_ID2, Disp_Clo_ID2, Disp_Clo_ID3, Disp_Clo_ID3, Disp_Clo_ID4, Disp_Clo_ID4)
            vEDSCNTID = Choose(I, vEdsCnt1_ID, vEdsCnt2_ID, vEdsCnt1_ID, vEdsCnt2_ID, vEdsCnt1_ID, vEdsCnt2_ID, vEdsCnt1_ID, vEdsCnt2_ID)

            If vCLTHID <> 0 And vEDSCNTID <> 0 Then

                da = New SqlClient.SqlDataAdapter("select * from Cloth_EndsCount_Details Where Cloth_Idno = " & Str(Val(vCLTHID)) & " and EndsCount_IdNo = " & Str(Val(vEDSCNTID)), con)
                dt1 = New DataTable
                da.Fill(dt1)
                If dt1.Rows.Count = 0 Then
                    MessageBox.Show("Mismatch of EndsCount with Cloth Master" & Chr(13) & Common_Procedures.Cloth_IdNoToName(con, vCLTHID), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                    Exit Sub
                End If
                dt1.Clear()

            End If

        Next


        NoofInpBmsInLom = 0
        vMULTI_ENDSCNT_SELC_STS = 0
        Da4 = New SqlClient.SqlDataAdapter("Select Noof_Input_Beams, Multiple_EndsCount_Selection_Status from Loom_Head Where Loom_IdNo = " & Str(Val(Lm_ID)), con)
        Dt4 = New DataTable
        Da4.Fill(Dt4)
        If Dt4.Rows.Count > 0 Then
            NoofInpBmsInLom = Val(Dt4.Rows(0).Item("Noof_Input_Beams").ToString)
            vMULTI_ENDSCNT_SELC_STS = Val(Dt4.Rows(0).Item("Multiple_EndsCount_Selection_Status").ToString)
        End If
        Dt4.Clear()
        If Val(NoofInpBmsInLom) = 0 Then NoofInpBmsInLom = 1


        If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" And Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
            If Trim(UCase(lbl_EndsCount_Beam1.Text)) <> Trim(UCase(lbl_EndsCount_Beam2.Text)) Then

                If Val(vMULTI_ENDSCNT_SELC_STS) = 0 Then
                    MessageBox.Show("Don't select beams with different Ends/Count" & Chr(13) & Chr(13) & "Change the settings in Loom creation to select two beams", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                    Exit Sub
                End If

            End If

        End If

        vMILL_id = Common_Procedures.Mill_NameToIdNo(con, lbl_MillName.Text)

        Emp_id = Common_Procedures.Employee_NameToIdNo(con, cbo_KnotterName.Text)

        'NoofInpBmsInLom = Common_Procedures.get_FieldValue(con, "Loom_Head", "Noof_Input_Beams", "(Loom_IdNo = " & Str(Val(Lm_ID)) & ")")
        'If Val(NoofInpBmsInLom) = 0 Then NoofInpBmsInLom = 1

        If NoofInpBmsInLom = 1 Then

            If Trim(lbl_BeamNo1.Text) = "" Then
                MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(lbl_Meters1.Text) = 0 Then
                MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Trim(lbl_BeamNo2.Text) <> "" Then
                MessageBox.Show("Invalid Beams, Select Only One Beam", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(lbl_Meters2.Text) <> 0 Then
                MessageBox.Show("Invalid Beam Meters, Select Only One Beam", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

        Else

            If Trim(lbl_BeamNo1.Text) = "" Or Trim(lbl_BeamNo2.Text) = "" Then
                MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(lbl_Meters1.Text) = 0 Or Val(lbl_Meters2.Text) = 0 Then
                MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

        End If

        Dim vFABMTRS_EXPECTED As String, vFABMTRS_EXPECTED_PERBEAM As String

        vFABMTRS_EXPECTED = Calculate_Fabric_Meters(Val(lbl_Meters1.Text) + Val(lbl_Meters2.Text))
        vFABMTRS_EXPECTED_PERBEAM = Format(Val(vFABMTRS_EXPECTED) / 2, "##########0.00")


        vWeav_JobCode_Forselec = ""

        If Trim(lbl_Weaver_Job_No.Text) <> "" Then
            vWeav_JobCode_Forselec = Trim(lbl_Weaver_Job_No.Text)
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Beam_Knotting_Head", "Beam_Knotting_Code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))

            vSELC_KNOTCODE = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Beam_Knotting_Head (     Beam_Knotting_Code,           Company_IdNo      ,        Beam_Knotting_No      ,                               for_OrderBy                          , Beam_Knotting_Date,               Shift           ,    Ledger_IdNo     ,       Cloth_Idno1  ,       Cloth_Idno2  ,      Cloth_Idno3    ,      Cloth_Idno4    ,      Display_Cloth_Idno1   ,   Display_Cloth_Idno2   ,      Display_Cloth_Idno3 ,   Display_Cloth_Idno4    ,       EndsCount_IdNo       ,      Loom_IdNo     ,              Width_Type           ,           Display_Width_Type      ,              Knotter_Name           ,            Set_Code1             ,             Set_No1            ,             Beam_No1            ,          Beam_Meters1        ,             Set_Code2            ,             Set_No2            ,           Beam_No2              ,           Beam_Meters2        ,    Employee_IdNo   ,     Wages_Amount               ,     ClothSales_OrderCode_forSelection        ,                           User_idNo     ,        Fabric_Meters_Expected      ,        Fabric_Meters_Expected_PerBeam            ,           Mill_IdNo       ,               Warp_LotNo           ,         Shift_IdNo    ,             Shift_Meters               , ClothSales_OrderCode_forSelection_Quality_2        ,        ClothSales_OrderCode_forSelection_Quality_3     ,  ClothSales_OrderCode_forSelection_Quality_4             ,             PANEL_1                 ,               PANEL_2             ,            PANEL_3                ,         PANEL_4                    ,       Weaving_JobCode_forSelection      ,          EndsCount1_IdNo     ,        EndsCount2_IdNo        , BeamKnotting_Code_forSelection ) " &
                                        "      Values             ('" & Trim(NewCode) & "', " & Val(lbl_Company.Tag) & ", '" & Trim(lbl_RefNo.Text) & "', " & Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)) & ",     @EntryDate    , '" & Trim(cbo_Shift.Text) & "', " & Val(led_id) & ", " & Val(Clo_ID) & "," & Val(Clo_ID2) & ", " & Val(Clo_ID3) & ", " & Val(Clo_ID4) & ",    " & Val(Disp_Clo_ID) & "," & Val(Disp_Clo_ID2) & ", " & Val(Disp_Clo_ID3) & ", " & Val(Disp_Clo_ID4) & ", " & Str(Val(EdsCnt_ID)) & ",  " & Val(Lm_ID) & ", '" & Trim(lbl_WidthType.Text) & "', '" & Trim(cbo_WidthType.Text) & "', '" & Trim(cbo_KnotterName.Text) & "', '" & Trim(lbl_SetCode1.Text) & "', '" & Trim(lbl_SetNo1.Text) & "', '" & Trim(lbl_BeamNo1.Text) & "', " & Val(lbl_Meters1.Text) & ", '" & Trim(lbl_SetCode2.Text) & "', '" & Trim(lbl_SetNo2.Text) & "', '" & Trim(lbl_BeamNo2.Text) & "',  " & Val(lbl_Meters2.Text) & ", " & Val(Emp_id) & ", " & Val(txt_WagesAmount.Text) & ", '" & Trim(cbo_ClothSales_OrderNo.Text) & "', " & Val(Common_Procedures.User.IdNo) & ", " & Str(Val(vFABMTRS_EXPECTED)) & ", " & Str(Val(vFABMTRS_EXPECTED_PERBEAM)) & ", " & Str(Val(vMILL_id)) & ", '" & Trim(lbl_WarpLotNo.Text) & "' , " & Val(vSHFTIDNO) & "     , " & Val(txt_Shiftmetrs.Text) & "        , '" & Trim(cbo_ClothSales_OrderNo_Quality_2.Text) & "' , '" & Trim(cbo_ClothSales_OrderNo_Quality_3.Text) & "', '" & Trim(cbo_ClothSales_OrderNo_Quality_4.Text) & "'  ,  '" & Trim(lbl_Panel_1.Text) & "'    , '" & Trim(lbl_Panel_2.Text) & "'  ,  '" & Trim(lbl_Panel_3.Text) & "' ,  '" & Trim(lbl_Panel_4.Text) & "'  , '" & Trim(vWeav_JobCode_Forselec) & "'  , " & Str(Val(vEdsCnt1_ID)) & ", " & Str(Val(vEdsCnt2_ID)) & " , '" & Trim(vSELC_KNOTCODE) & "' ) "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Beam_Knotting_Head", "Beam_Knotting_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Beam_Knotting_Code, Company_IdNo, for_OrderBy", tr)

                da = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'", con)
                da.SelectCommand.Transaction = tr
                dt1 = New DataTable
                da.Fill(dt1)

                If dt1.Rows.Count > 0 Then

                    'If IsDBNull(dt1.Rows(0).Item("Production_Meters").ToString) = False Then
                    '    If Val(dt1.Rows(0).Item("Production_Meters").ToString) <> 0 Then
                    '        Throw New ApplicationException("Invalid Editing : Already Production entered after this knotting")
                    '        Exit Sub
                    '    End If
                    'End If
                    'If IsDBNull(dt1.Rows(0).Item("Beam_RunOut_Code").ToString) = False Then
                    '    If Trim(dt1.Rows(0).Item("Beam_RunOut_Code").ToString) <> "" Then
                    '        Throw New ApplicationException("Invalid Editing : Already this knotting, was runout")
                    '        Exit Sub
                    '    End If
                    'End If

                    'da = New SqlClient.SqlDataAdapter("Select Close_Status from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No1").ToString) & "'", con)
                    'da.SelectCommand.Transaction = tr
                    'dt2 = New DataTable
                    'da.Fill(dt2)
                    'If dt2.Rows.Count > 0 Then
                    '    If IsDBNull(dt2.Rows(0).Item("Close_Status").ToString) = False Then
                    '        If Val(dt2.Rows(0).Item("Close_Status").ToString) <> 0 Then
                    '            Throw New ApplicationException("Invalid Editing : Already this Beams was Closed")
                    '            Exit Sub
                    '        End If
                    '    End If
                    'End If
                    'dt2.Clear()

                    'da = New SqlClient.SqlDataAdapter("Select Close_Status from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No2").ToString) & "'", con)
                    'da.SelectCommand.Transaction = tr
                    'dt2 = New DataTable
                    'da.Fill(dt2)
                    'If dt2.Rows.Count > 0 Then
                    '    If IsDBNull(dt2.Rows(0).Item("Close_Status").ToString) = False Then
                    '        If Val(dt2.Rows(0).Item("Close_Status").ToString) <> 0 Then
                    '            Throw New ApplicationException("Invalid Editing : Already this Beams was Closed")
                    '            Exit Sub
                    '        End If
                    '    End If
                    'End If
                    'dt2.Clear()

                    nr = 0
                    cmd.CommandText = "Update Loom_Head Set Beam_Knotting_Code = '' Where Loom_IdNo = " & Str(Val(dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
                    nr = cmd.ExecuteNonQuery
                    'If nr = 0 Then
                    '    Throw New ApplicationException("Invalid Editing : Already this loom was knotted again")
                    '    Exit Sub
                    'End If

                    If Trim(dt1.Rows(0).Item("Set_Code1").ToString) <> "" And Trim(dt1.Rows(0).Item("Beam_No1").ToString) <> "" Then
                        nr = 0
                        cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '', Loom_Idno = 0 ,Weaving_JobCode_forSelection = '' From Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No1").ToString) & "' and Loom_Idno = " & Str(Val(dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "' and Close_Status = 0"
                        nr = cmd.ExecuteNonQuery
                        'If nr = 0 Then
                        '    Throw New ApplicationException("Invalid Editing : Already this Beams is running in another loom (or) Closed")
                        '    Exit Sub
                        'End If
                    End If

                    If Trim(dt1.Rows(0).Item("Set_Code2").ToString) <> "" And Trim(dt1.Rows(0).Item("Beam_No2").ToString) <> "" Then
                        nr = 0
                        cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '', Loom_Idno = 0 ,Weaving_JobCode_forSelection = '' From Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No2").ToString) & "' and Loom_Idno = " & Str(Val(dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "' and Close_Status = 0"
                        nr = cmd.ExecuteNonQuery
                        'If nr = 0 Then
                        '    Throw New ApplicationException("Invalid Editing : Already this Beams is running in another loom (or) Closed")
                        '    Exit Sub
                        'End If
                    End If


                End If
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Beam_Knotting_Head", "Beam_Knotting_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Beam_Knotting_Code, Company_IdNo, for_OrderBy", tr)

                dt1.Clear()

                cmd.CommandText = "Update Beam_Knotting_Head set Beam_Knotting_Date = @EntryDate, Shift = '" & Trim(cbo_Shift.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", Cloth_Idno1 = " & Str(Val(Clo_ID)) & ",  Cloth_Idno2 = " & Str(Val(Clo_ID2)) & ",  Cloth_Idno3 = " & Str(Val(Clo_ID3)) & ", Cloth_Idno4 = " & Str(Val(Clo_ID4)) & ", Display_Cloth_Idno1 = " & Str(Val(Disp_Clo_ID)) & ",  Display_Cloth_Idno2 = " & Str(Val(Disp_Clo_ID2)) & ",  Display_Cloth_Idno3 = " & Str(Val(Disp_Clo_ID3)) & ", Display_Cloth_Idno4 = " & Str(Val(Disp_Clo_ID4)) & ",  EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Employee_IdNo = " & Str(Val(Emp_id)) & " , Wages_Amount = " & Str(Val(txt_WagesAmount.Text)) & " ,  Loom_IdNo = " & Str(Val(Lm_ID)) & ",Display_Width_Type = '" & Trim(cbo_WidthType.Text) & "' ,  Width_Type = '" & Trim(lbl_WidthType.Text) & "', Knotter_Name = '" & Trim(cbo_KnotterName.Text) & "',  set_Code1 = '" & Trim(lbl_SetCode1.Text) & "', set_no1 = '" & Trim(lbl_SetNo1.Text) & "', Beam_No1 = '" & Trim(lbl_BeamNo1.Text) & "', Beam_Meters1 = " & Str(Val(lbl_Meters1.Text)) & ", set_Code2 = '" & Trim(lbl_SetCode2.Text) & "', set_no2 = '" & Trim(lbl_SetNo2.Text) & "', Beam_No2 = '" & Trim(lbl_BeamNo2.Text) & "', Beam_Meters2 = " & Str(Val(lbl_Meters2.Text)) & ", ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderNo.Text) & "', Fabric_Meters_Expected = " & Str(Val(vFABMTRS_EXPECTED)) & ", Fabric_Meters_Expected_PerBeam = " & Str(Val(vFABMTRS_EXPECTED_PERBEAM)) & " , Mill_IdNo = " & Str(Val(vMILL_id)) & ", Warp_LotNo = '" & Trim(lbl_WarpLotNo.Text) & "' , User_idNo = " & Val(Common_Procedures.User.IdNo) & ", Shift_IdNo = " & Val(vSHFTIDNO) & " , Shift_Meters = " & Val(txt_Shiftmetrs.Text) & "  ,ClothSales_OrderCode_forSelection_Quality_2='" & Trim(cbo_ClothSales_OrderNo_Quality_2.Text) & "' ,  ClothSales_OrderCode_forSelection_Quality_3='" & Trim(cbo_ClothSales_OrderNo_Quality_3.Text) & "' ,  ClothSales_OrderCode_forSelection_Quality_4='" & Trim(cbo_ClothSales_OrderNo_Quality_4.Text) & "' , Panel_1= '" & Trim(lbl_Panel_1.Text) & "' ,Panel_2= '" & Trim(lbl_Panel_2.Text) & "',Panel_3= '" & Trim(lbl_Panel_3.Text) & "',Panel_4= '" & Trim(lbl_Panel_4.Text) & "' , Weaving_JobCode_forSelection ='" & Trim(vWeav_JobCode_Forselec) & "' , EndsCount1_IdNo = " & Str(Val(vEdsCnt1_ID)) & ", EndsCount2_IdNo = " & Str(Val(vEdsCnt2_ID)) & " , BeamKnotting_Code_forSelection = '" & Trim(vSELC_KNOTCODE) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And Beam_Knotting_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If


            nr = 0
            cmd.CommandText = "Update Loom_Head Set Beam_Knotting_Code = '" & Trim(NewCode) & "' Where Loom_Idno = " & Str(Val(Lm_ID)) & " and Beam_Knotting_Code = ''"
            nr = cmd.ExecuteNonQuery
            'If nr = 0 Then
            '    Throw New ApplicationException("Already this Loom was knotted")
            '    Exit Sub
            'End If

            If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
                nr = 0
                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '" & Trim(NewCode) & "', Loom_Idno = " & Str(Val(Lm_ID)) & " ,Weaving_JobCode_forSelection = '" & Trim(vWeav_JobCode_Forselec) & "' Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "' and Beam_Knotting_Code = '' and Loom_Idno = 0 and Close_Status = 0"
                nr = cmd.ExecuteNonQuery
                'If nr = 0 Then
                '    Throw New ApplicationException("Already this Beam '" & Trim(lbl_BeamNo1.Text) & "' is running in another loom (or) Closed")
                '    Exit Sub
                'End If

            End If

            If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                nr = 0
                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '" & Trim(NewCode) & "', Loom_Idno = " & Str(Val(Lm_ID)) & ",Weaving_JobCode_forSelection = '" & Trim(vWeav_JobCode_Forselec) & "' Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "' and Beam_Knotting_Code = '' and Loom_Idno = 0 and Close_Status = 0"
                nr = cmd.ExecuteNonQuery
                'If nr = 0 Then
                '    Throw New ApplicationException("Already this Beam '" & Trim(lbl_BeamNo2.Text) & "' is running in another loom (or) Closed")
                '    Exit Sub
                'End If

            End If

            NoofKnotBmsInCD = 0
            da = New SqlClient.SqlDataAdapter("Select count(*) from Stock_SizedPavu_Processing_Details where Beam_Knotting_Code = '" & Trim(NewCode) & "'", con)
            da.SelectCommand.Transaction = tr
            dt2 = New DataTable
            da.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                    NoofKnotBmsInCD = Val(dt2.Rows(0)(0).ToString)
                End If
            End If
            dt2.Clear()

            'If Val(NoofKnotBmsInCD) <> Val(NoofInpBmsInLom) Then
            '    Throw New ApplicationException("Invalid Knotting for this Code")
            '    Exit Sub
            'End If

            NoofKnotBmsInLom = 0
            da = New SqlClient.SqlDataAdapter("Select count(*) from Stock_SizedPavu_Processing_Details where Loom_IdNo = " & Str(Val(Lm_ID)), con)
            da.SelectCommand.Transaction = tr
            dt2 = New DataTable
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                    NoofKnotBmsInLom = Val(dt2.Rows(0)(0).ToString)
                End If
            End If
            dt2.Clear()

            'If Val(NoofKnotBmsInLom) <> Val(NoofInpBmsInLom) Then
            '    Throw New ApplicationException("Invalid Knotting for this Loom")
            '    Exit Sub
            'End If


            CR_id = Emp_id
            DR_id = 23

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            vLed_IdNos = Val(DR_id) & "|" & CR_id
            vVou_Amts = -1 * (Val(txt_WagesAmount.Text)) & "|" & Val((txt_WagesAmount.Text))

            If Common_Procedures.Voucher_Updation(con, "knott.Wages", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "knotting No : " & Trim(lbl_RefNo.Text) & IIf(Trim(lbl_RefNo.Text) <> "", " , Loom.No : " & Trim(cbo_LoomNo.Text), ""), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            tr.Commit()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

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
            dt1.Dispose()
            da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------ No Print
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_Shiftmetrs, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If txt_Shiftmetrs.Visible Then
                txt_Shiftmetrs.Focus()
            Else
                cbo_Shift.Focus()
            End If

        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_EndsCount.Visible And cbo_EndsCount.Enabled Then
                cbo_EndsCount.Focus()
            Else
                cbo_LoomNo.Focus()
            End If

        End If



    End Sub


    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_EndsCount.Visible And cbo_EndsCount.Enabled Then
                cbo_EndsCount.Focus()
            Else
                cbo_LoomNo.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1

            f.Show()

        End If
    End Sub

    Private Sub cbo_ClothName1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName1.GotFocus
        Dim vCLO_CONDT As String
        Dim vFIRST_CLONAME As String

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""
        get_ClothName_Condition(vCLO_CONDT, vFIRST_CLONAME, sender)

        If Trim(sender.Text) = "" Then
            sender.Text = vFIRST_CLONAME
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Close_Status = 0 " & IIf(Trim(vCLO_CONDT) <> "", " and ", "") & vCLO_CONDT & " )", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_ClothName1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName1.KeyDown
        Dim vCLO_CONDT As String
        Dim vFIRST_CLONAME As String

        vcbo_KeyDwnVal = e.KeyValue

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""
        get_ClothName_Condition(vCLO_CONDT, vFIRST_CLONAME, Nothing)

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "(Close_Status = 0 " & IIf(Trim(vCLO_CONDT) <> "", " and ", "") & vCLO_CONDT & " )", "(Cloth_IdNo = 0)")

        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_WidthType.Enabled And cbo_WidthType.Visible Then
                cbo_WidthType.Focus()

            ElseIf cbo_LoomNo.Visible And cbo_LoomNo.Enabled Then
                cbo_LoomNo.Focus()

            ElseIf cbo_EndsCount.Visible And cbo_EndsCount.Enabled Then
                cbo_EndsCount.Focus()

            ElseIf cbo_PartyName.Visible And cbo_PartyName.Enabled Then
                cbo_PartyName.Focus()

            Else
                msk_Date.Focus()

            End If

        ElseIf (e.KeyValue = 40 And cbo_ClothName1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_ClothSales_OrderNo.Enabled And cbo_ClothSales_OrderNo.Visible Then
                cbo_ClothSales_OrderNo.Focus()

            ElseIf cbo_ClothName2.Visible And cbo_ClothName2.Enabled Then
                cbo_ClothName2.Focus()

            ElseIf cbo_KnotterName.Enabled = True Then
                cbo_KnotterName.Focus()

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If

            End If

        End If

    End Sub

    Private Sub cbo_ClothName1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName1.KeyPress
        Dim vCLO_CONDT As String
        Dim vFIRST_CLONAME As String

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""
        get_ClothName_Condition(vCLO_CONDT, vFIRST_CLONAME, Nothing)

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Cloth_Head", "Cloth_Name", "(Close_Status = 0 " & IIf(Trim(vCLO_CONDT) <> "", " and ", "") & vCLO_CONDT & " )", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If cbo_ClothSales_OrderNo.Visible And cbo_ClothSales_OrderNo.Enabled Then
                cbo_ClothSales_OrderNo.Focus()

            ElseIf cbo_ClothName2.Visible And cbo_ClothName2.Enabled Then
                cbo_ClothName2.Focus()

            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_ClothName2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName2.GotFocus
        Dim vCLO_CONDT As String
        Dim vFIRST_CLONAME As String

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""
        get_ClothName_Condition(vCLO_CONDT, vFIRST_CLONAME, sender)

        If Trim(sender.Text) = "" Then
            sender.Text = vFIRST_CLONAME
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName2.KeyDown
        Dim vCLO_CONDT As String
        Dim vFIRST_CLONAME As String

        vcbo_KeyDwnVal = e.KeyValue

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""
        get_ClothName_Condition(vCLO_CONDT, vFIRST_CLONAME, Nothing)

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName2, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_idno = 0)")

        If (e.KeyValue = 38 And cbo_ClothName2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_ClothSales_OrderNo.Enabled And cbo_ClothSales_OrderNo.Visible Then
                cbo_ClothSales_OrderNo.Focus()
            Else
                cbo_ClothName1.Focus()

            End If


        ElseIf (e.KeyValue = 40 And cbo_ClothName2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            'If cbo_ClothName3.Enabled = True Then
            '    cbo_ClothName3.Focus()
            'Else
            '    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '        save_record()
            '    Else
            '        dtp_Date.Focus()
            '    End If
            'End If

            If cbo_ClothSales_OrderNo_Quality_2.Enabled And cbo_ClothSales_OrderNo_Quality_2.Visible Then

                cbo_ClothSales_OrderNo_Quality_2.Focus()

            ElseIf cbo_ClothName3.Enabled = True Then
                cbo_ClothName3.Focus()

            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()

            Else

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If


            End If

        End If
    End Sub

    Private Sub cbo_ClothName2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName2.KeyPress
        Dim vCLO_CONDT As String
        Dim vFIRST_CLONAME As String

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""
        get_ClothName_Condition(vCLO_CONDT, vFIRST_CLONAME, Nothing)

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName2, Nothing, "Cloth_Head", "Cloth_Name", "(Close_Status = 0 " & IIf(Trim(vCLO_CONDT) <> "", " and ", "") & vCLO_CONDT & " )", "(Cloth_idno = 0)")

        'If Asc(e.KeyChar) = 13 Then
        '    If cbo_ClothName3.Enabled = True Then
        '        cbo_ClothName3.Focus()

        '    Else
        '        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '            save_record()
        '        Else
        '            dtp_Date.Focus()
        '        End If

        '    End If         ElseIf cbo_ClothName2.Visible And cbo_ClothName2.Enabled Then

        'End If

        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothSales_OrderNo_Quality_2.Visible And cbo_ClothSales_OrderNo_Quality_2.Enabled Then
                cbo_ClothSales_OrderNo_Quality_2.Focus()
            ElseIf cbo_ClothName3.Enabled = True Then
                cbo_ClothName3.Focus()
            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()
                'Else
                '    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '        save_record()
                '    Else
                '        msk_Date.Focus()
                '    End If
            End If
        End If

    End Sub


    Private Sub cbo_ClothName3_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName3.GotFocus
        Dim vCLO_CONDT As String
        Dim vFIRST_CLONAME As String

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""
        get_ClothName_Condition(vCLO_CONDT, vFIRST_CLONAME, sender)

        If Trim(sender.Text) = "" Then
            sender.Text = vFIRST_CLONAME
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName3.KeyDown
        Dim vCLO_CONDT As String
        Dim vFIRST_CLONAME As String

        vcbo_KeyDwnVal = e.KeyValue

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""
        get_ClothName_Condition(vCLO_CONDT, vFIRST_CLONAME, Nothing)

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName3, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_idno = 0)")

        If (e.KeyValue = 38 And cbo_ClothName3.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_ClothSales_OrderNo_Quality_2.Visible And cbo_ClothSales_OrderNo_Quality_2.Enabled Then
                cbo_ClothSales_OrderNo_Quality_2.Focus()
            Else

                cbo_ClothName2.Focus()

            End If
        ElseIf (e.KeyValue = 40 And cbo_ClothName3.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_ClothSales_OrderNo_Quality_3.Enabled And cbo_ClothSales_OrderNo_Quality_3.Visible Then

                cbo_ClothSales_OrderNo_Quality_3.Focus()


            ElseIf cbo_ClothName4.Enabled = True Then

                cbo_ClothName4.Focus()

            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()


            Else

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub cbo_ClothName3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName3.KeyPress
        Dim vCLO_CONDT As String
        Dim vFIRST_CLONAME As String

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""
        get_ClothName_Condition(vCLO_CONDT, vFIRST_CLONAME, Nothing)

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName3, Nothing, "Cloth_Head", "Cloth_Name", "(Close_Status = 0 " & IIf(Trim(vCLO_CONDT) <> "", " and ", "") & vCLO_CONDT & " )", "(Cloth_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            'If cbo_ClothName4.Enabled = True Then
            '    cbo_ClothName4.Focus()

            If cbo_ClothSales_OrderNo_Quality_3.Enabled And cbo_ClothSales_OrderNo_Quality_3.Visible Then
                cbo_ClothSales_OrderNo_Quality_3.Focus()

            ElseIf cbo_ClothName4.Visible And cbo_ClothName4.Enabled Then
                cbo_ClothName4.Focus()

            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If

            End If
        End If

    End Sub

    Private Sub cbo_ClothName4_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName4.GotFocus
        Dim vCLO_CONDT As String
        Dim vFIRST_CLONAME As String

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""
        get_ClothName_Condition(vCLO_CONDT, vFIRST_CLONAME, sender)

        If Trim(sender.Text) = "" Then
            sender.Text = vFIRST_CLONAME
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName4.KeyDown
        Dim vCLO_CONDT As String
        Dim vFIRST_CLONAME As String

        vcbo_KeyDwnVal = e.KeyValue

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""
        get_ClothName_Condition(vCLO_CONDT, vFIRST_CLONAME, Nothing)

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName4, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_idno = 0)")

        If (e.KeyValue = 38 And cbo_ClothName4.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_ClothSales_OrderNo_Quality_3.Visible And cbo_ClothSales_OrderNo_Quality_3.Enabled Then
                cbo_ClothSales_OrderNo_Quality_3.Focus()

            Else
                cbo_ClothName3.Focus()


            End If

        ElseIf (e.KeyValue = 40 And cbo_ClothName4.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_ClothSales_OrderNo_Quality_4.Visible And cbo_ClothSales_OrderNo_Quality_4.Enabled Then
                cbo_ClothSales_OrderNo_Quality_4.Focus()
            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()
            Else
                btn_save.Focus()
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If
            End If
        End If

    End Sub

    Private Sub cbo_ClothName4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName4.KeyPress
        Dim vCLO_CONDT As String
        Dim vFIRST_CLONAME As String

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""
        get_ClothName_Condition(vCLO_CONDT, vFIRST_CLONAME, Nothing)

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName4, Nothing, "Cloth_Head", "Cloth_Name", "(Close_Status = 0 " & IIf(Trim(vCLO_CONDT) <> "", " and ", "") & vCLO_CONDT & " )", "(Cloth_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If cbo_ClothSales_OrderNo_Quality_4.Visible And cbo_ClothSales_OrderNo_Quality_4.Enabled Then
                cbo_ClothSales_OrderNo_Quality_4.Focus()
            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()
            Else

                btn_save.Focus()
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, cbo_LoomNo, cbo_ClothName1, "", "", "", "")

        If (e.KeyValue = 38 And cbo_WidthType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then


            If cbo_LoomNo.Enabled And cbo_LoomNo.Visible Then

                cbo_LoomNo.Focus()

            ElseIf txt_Shiftmetrs.Visible And txt_Shiftmetrs.Enabled Then

                txt_Shiftmetrs.Focus()

            Else

                cbo_Shift.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_widthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, cbo_ClothName1, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            cbo_WidthType_TextChanged(sender, e)
        End If
    End Sub

    Private Sub cbo_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_IdNo)")
    End Sub

    Private Sub cbo_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Shift, msk_Date, txt_Shiftmetrs, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If txt_Shiftmetrs.Visible And txt_Shiftmetrs.Enabled Then
                txt_Shiftmetrs.Focus()
            Else
                cbo_PartyName.Focus()

            End If

        End If



    End Sub

    Private Sub cbo_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Shift, txt_Shiftmetrs, "Shift_Head", "Shift_Name", "", "(Shift_IdNo)")

        If (Asc(e.KeyChar) = 13 And cbo_Shift.DroppedDown = False) Then

            If txt_Shiftmetrs.Visible And txt_Shiftmetrs.Enabled Then
                txt_Shiftmetrs.Focus()
            Else
                cbo_PartyName.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_KnotterName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KnotterName.GotFocus

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_KnotterName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KnotterName.KeyDown
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_KnotterName, Nothing, txt_WagesAmount, "Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_KnotterName, Nothing, txt_WagesAmount, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        End If

        If (e.KeyValue = 38 And cbo_KnotterName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_ClothName4.Enabled = True Then
                cbo_ClothName4.Focus()
            ElseIf cbo_ClothName3.Enabled = True Then
                cbo_ClothName3.Focus()
            ElseIf cbo_ClothName2.Enabled = True Then
                cbo_ClothName2.Focus()
            ElseIf cbo_ClothName1.Enabled = True Then
                cbo_ClothName1.Focus()
            ElseIf cbo_WidthType.Enabled = True Then
                cbo_WidthType.Focus()
            Else
                cbo_LoomNo.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_KnotterName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_KnotterName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim New_Rate As Double = 0
        Dim Emp_idno As String

        Emp_idno = Common_Procedures.Employee_NameToIdNo(con, Trim(cbo_KnotterName.Text))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_KnotterName, txt_WagesAmount, "Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_KnotterName, txt_WagesAmount, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        End If

        If Asc(e.KeyChar) = 13 Then
            da = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Employee_Head a Where a.Employee_IdNo = " & Str(Val(Emp_idno)), con)
            da.Fill(dt)

            New_Rate = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    New_Rate = Val(dt.Rows(0).Item("Wages_Amount").ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            txt_WagesAmount.Text = Val(New_Rate)

        End If
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, cbo_Filter_LoomNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_LoomNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub


    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clt_IdNo As Integer, Lom_IdNo As Integer
        Dim Condt As String = ""
        Dim StCode As String = "", BmNo As String = ""
        Try

            Condt = ""
            Led_IdNo = 0
            Clt_IdNo = 0
            Lom_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Beam_Knotting_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Beam_Knotting_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Beam_Knotting_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Clt_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If
            If Val(Clt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Cloth_Idno1 = " & Str(Val(Clt_IdNo)) & " or a.Cloth_Idno2 = " & Str(Val(Clt_IdNo)) & " or a.Cloth_Idno3 = " & Str(Val(Clt_IdNo)) & " or a.Cloth_Idno4 = " & Str(Val(Clt_IdNo)) & ")"
            End If

            If Trim(cbo_Filter_LoomNo.Text) <> "" Then
                Lom_IdNo = Common_Procedures.Loom_NameToIdNo(con, cbo_Filter_LoomNo.Text)
            End If
            If Val(Lom_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Loom_Idno = " & Str(Val(Lom_IdNo)) & ")"
            End If


            StCode = "" : BmNo = ""
            If Trim(cbo_Filter_BeamNo.Text) <> "" Then
                da = New SqlClient.SqlDataAdapter("select * from Stock_SizedPavu_Processing_Details where BeamNo_SetCode_forSelection = '" & Trim(cbo_Filter_BeamNo.Text) & "'", con)
                dt2 = New DataTable
                da.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    StCode = dt2.Rows(0).Item("set_code").ToString
                    BmNo = dt2.Rows(0).Item("beam_no").ToString
                End If

                If Trim(StCode) <> "" And Trim(BmNo) <> "" Then
                    Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & "  ( (a.Set_Code1 = '" & Trim(StCode) & "' and a.Beam_No1 = '" & Trim(BmNo) & "') or (a.Set_Code2 = '" & Trim(StCode) & "' and a.Beam_No2 = '" & Trim(BmNo) & "') ) "
                End If

                'Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " tSPP.BeamNo_SetCode_forSelection = '" & Trim(cbo_Filter_BeamNo.Text) & "'"
                'Join1 = " LEFT OUTER JOIN Stock_SizedPavu_Processing_Details tSPP ON tSPP.BeamNo_SetCode_forSelection = '" & Trim(cbo_Filter_BeamNo.Text) & "' and ( (tSPP.Set_Code = a.Set_Code1 and tSPP.Beam_No = a.Beam_No1) or (tSPP.Set_Code = a.Set_Code2 and tSPP.Beam_No = a.Beam_No2) ) "

            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Cloth_Name,  d.Loom_Name from Beam_Knotting_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 = c.Cloth_IdNo LEFT OUTER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Beam_Knotting_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Beam_Knotting_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Beam_Knotting_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Beam_Knotting_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Beam_No1").ToString
                    dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Beam_No2").ToString



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
    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, dtp_FilterTo_date, cbo_Filter_PartyName, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub
    Private Sub cbo_Filter_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_LoomNo.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_LoomNo, cbo_Filter_PartyName, cbo_Filter_BeamNo, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_LoomNo, cbo_Filter_BeamNo, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(cbo_LoomNo.Text)) = "" Then
        '        If MessageBox.Show("Do you want to select  :", "FOR  SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '            btn_Selection_Click(sender, e)
        '        Else
        '            cbo_WidthType.Focus()
        '        End If

        '    Else
        '        cbo_WidthType.Focus()

        '    End If

        'End If

    End Sub



    Private Sub cbo_Filter_BeamNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_BeamNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    End Sub

    Private Sub cbo_Filter_BeamNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_BeamNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_BeamNo, cbo_Filter_LoomNo, btn_filtershow, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    End Sub

    Private Sub cbo_Filter_BeamNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_BeamNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_BeamNo, btn_filtershow, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    End Sub




    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub


    Private Sub btn_filtershow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles btn_filtershow.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub


    Private Sub btn_closefilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

    Private Sub cbo_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName1.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub


    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, cbo_PartyName, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        'If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
        '    If cbo_ClothSales_OrderNo.Enabled And cbo_ClothSales_OrderNo.Visible Then
        '        cbo_ClothSales_OrderNo.Focus()
        '    Else
        '        cbo_PartyName.Focus()
        '    End If
        'End if
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            'If cbo_ClothSales_OrderNo.Enabled And cbo_ClothSales_OrderNo.Visible Then
            '    cbo_ClothSales_OrderNo.Focus()
            'Else
            '    cbo_LoomNo.Focus()
            'End If
            If cbo_LoomNo.Visible And Enabled Then
                cbo_LoomNo.Focus()
            Else

                cbo_WidthType.Focus()
            End If


        End If
    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0 )")
        If Asc(e.KeyChar) = 13 Then
            If cbo_LoomNo.Visible And Enabled Then
                cbo_LoomNo.Focus()
            Else

                cbo_WidthType.Focus()
            End If
        End If
    End Sub


    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code = '' and Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " )", "(Loom_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code = '')", "(Loom_IdNo = 0)")
        End If
    End Sub

    Private Sub cbo_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code = '' and  Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " )", "(Loom_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code = '')", "(Loom_IdNo = 0)")
        End If
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_EndsCount.Visible And cbo_EndsCount.Enabled Then
                cbo_EndsCount.Focus()
            Else
                cbo_PartyName.Focus()
            End If

        End If
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If MessageBox.Show("Do you want to select Pavu :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)
            Else
                cbo_WidthType.Focus()
            End If

        End If

    End Sub

    Private Sub cbo_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, Nothing, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code = '' and  Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " )", "(Loom_IdNo = 0 )")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, Nothing, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code = '')", "(Loom_IdNo = 0 )")
        End If
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Pavu :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)
            Else
                cbo_WidthType.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_LoomNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LoomNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LoomNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_LoomNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.LostFocus
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Lm_ID As Integer

        With cbo_LoomNo

            If Trim(UCase(.Tag)) <> Trim(UCase(.Text)) Then

                Lm_ID = Common_Procedures.Loom_NameToIdNo(con, .Text)

                Da = New SqlClient.SqlDataAdapter("select top 1 Width_Type from Beam_Knotting_Head where loom_idno = " & Str(Val(Lm_ID)) & " Order by Beam_Knotting_Date desc, For_OrderBy desc, Beam_Knotting_No desc", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0).Item("Width_Type").ToString) = False Then
                        If Dt1.Rows(0).Item("Width_Type").ToString <> "" Then
                            cbo_WidthType.Text = Dt1.Rows(0).Item("Width_Type").ToString
                        End If
                    End If
                End If

            End If

        End With
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Led_ID As Integer
        Dim EdsCnt_ID As Integer, Lm_ID As Integer
        Dim NewCode As String = ""
        Dim CompIDCondt As String = ""
        Dim vENDCNTIDCondt As String = ""

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        EdsCnt_ID = 0
        If cbo_EndsCount.Visible = True Then
            EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
            If EdsCnt_ID = 0 Then
                MessageBox.Show("Invalid Ends/Count", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_EndsCount.Enabled Then cbo_EndsCount.Focus()
                Exit Sub
            End If
        End If

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Lm_ID = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            CompIDCondt = ""
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1520" Then  ' --- RAINBOW COTTON FABRIC 
            CompIDCondt = ""
        Else
            If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
                CompIDCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompIDCondt = "(Company_Type <> 'UNACCOUNT')"
                End If
            End If
        End If

        vENDCNTIDCondt = ""
        If cbo_EndsCount.Visible = True Then
            vENDCNTIDCondt = " a.EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & " and "
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Cmd.Connection = con

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
        Cmd.ExecuteNonQuery()

        If New_Entry = False Then
            Cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Name1, Name2, Date1, Weight1, Name3, Weight2, Meters1, Meters2, Name4 , Name5, Name6) select 1, a.Set_Code, a.Set_No, a.Reference_Date, a.For_OrderBy, a.beam_no, a.ForOrderBy_BeamNo, a.meters, (a.Meters - a.Production_Meters), '1' ,a.Weaving_JobCode_forSelection , tE.EndsCount_Name from Stock_SizedPavu_Processing_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno INNER JOIN EndsCount_Head tE ON a.EndsCount_idno = tE.EndsCount_idno, Beam_Knotting_Head b Where  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Beam_Knotting_Code = '" & Trim(NewCode) & "' and  " & vENDCNTIDCondt & " a.Set_Code = b.Set_Code1 and a.Beam_No = b.Beam_No1"
            Cmd.ExecuteNonQuery()
            Cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Name1, Name2, Date1, Weight1, Name3, Weight2, Meters1, Meters2, Name4 , Name5, Name6) select 2, a.Set_Code, a.Set_No, a.Reference_Date, a.For_OrderBy, a.beam_no, a.ForOrderBy_BeamNo, a.meters, (a.Meters - a.Production_Meters), '1' ,a.Weaving_JobCode_forSelection , tE.EndsCount_Name from Stock_SizedPavu_Processing_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno INNER JOIN EndsCount_Head tE ON a.EndsCount_idno = tE.EndsCount_idno, Beam_Knotting_Head b Where  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Beam_Knotting_Code = '" & Trim(NewCode) & "' and  " & vENDCNTIDCondt & " a.Set_Code = b.Set_Code2 and a.Beam_No = b.Beam_No2"
            Cmd.ExecuteNonQuery()
        End If

        Cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Name1, Name2, Date1, Weight1, Name3, Weight2, Meters1, Meters2, Name4, Name5, Name6) select 3, a.set_code, a.set_no, a.Reference_Date, a.For_OrderBy, a.beam_no, a.ForOrderBy_BeamNo, a.meters, (a.Meters - a.Production_Meters), '' ,a.Weaving_JobCode_forSelection , tE.EndsCount_Name from Stock_SizedPavu_Processing_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  INNER JOIN EndsCount_Head tE ON a.EndsCount_idno = tE.EndsCount_idno Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.StockAt_IdNo = " & Str(Val(Led_ID)) & " and " & vENDCNTIDCondt & " a.Pavu_Delivery_Code = '' and a.Beam_Knotting_Code = '' and a.Loom_IdNo = 0 and a.Close_Status = 0 and a.beam_no NOT IN (select z1.Name3 from " & Trim(Common_Procedures.EntryTempTable) & " z1 where z1.Name1 = a.set_code)"
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Name1, Name2, Date1, Weight1, Name3, Weight2, Meters1, Meters2, Name4,Name5) select 3, a.set_code, a.set_no, a.Reference_Date, a.For_OrderBy, a.beam_no, a.ForOrderBy_BeamNo, a.meters, (a.Meters - a.Production_Meters), '' ,a.Weaving_JobCode_forSelection from Stock_SizedPavu_Processing_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno Where  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.StockAt_IdNo = " & Str(Val(Led_ID)) & " and a.EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & " and a.Pavu_Delivery_Code = '' and a.Beam_Knotting_Code = '' and a.Loom_IdNo = 0 and a.Close_Status = 0 and a.beam_no NOT IN (select z1.Name3 from " & Trim(Common_Procedures.EntryTempTable) & " z1 where z1.Name1 = a.set_code)"
        Cmd.ExecuteNonQuery()

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select Name1 as Set_Code, Name2 as Set_No, Name3 as Beam_No, Meters1 as Total_Meters, Meters2 as Balance_Meters, Name4 as SelectionSTS, Name5 as Weaving_JobCode_forSelection , Name6 as EndsCount_Name from " & Trim(Common_Procedures.EntryTempTable) & " where Meters1 > 0 order by Int1, Name6, Date1, Weight1, name2, Weight2, Name3", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Balance_Meters").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(4).Value) = 1 Then
                        .Rows(n).Cells(4).Value = ""
                    End If
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("SelectionSTS").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Weaving_JobCode_forSelection").ToString
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString

                    If Val(.Rows(n).Cells(5).Value) = 1 Then
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next
                    End If

                Next

            End If
            Dt1.Clear()

        End With

        Dt1.Dispose()
        Da.Dispose()
        Cmd.Dispose()

        pnl_Selection.Visible = True
        pnl_back.Enabled = False
        dgv_Selection.Focus()
        If dgv_Selection.Rows.Count > 0 Then
            dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        End If

    End Sub
    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Pavu(e.RowIndex)
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

    Private Sub Select_Pavu(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(5).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next
                    .DefaultCellStyle.SelectionForeColor = Color.Red

                Else
                    .Rows(RwIndx).Cells(5).Value = ""
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next
                    .DefaultCellStyle.SelectionForeColor = Color.Black

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Try
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                    Select_Pavu(dgv_Selection.CurrentCell.RowIndex)
                    e.Handled = True
                End If
            End If
        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Selection()
    End Sub

    Private Sub Close_Selection()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim i As Integer
        Dim BmCnt As Integer
        Dim Lm_ID As Integer = 0
        Dim NoofInpBmsInLom As Integer = 0
        Dim NoofKnotBmsInCD As Integer = 0
        Dim NoofKnotBmsInLom As Integer = 0
        Dim vMULTI_ENDSCNT_SELC_STS As String = 0

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)


        NoofInpBmsInLom = 0
        vMULTI_ENDSCNT_SELC_STS = 0
        Da4 = New SqlClient.SqlDataAdapter("Select Noof_Input_Beams, Multiple_EndsCount_Selection_Status from Loom_Head Where Loom_IdNo = " & Str(Val(Lm_ID)), con)
        Dt4 = New DataTable
        Da4.Fill(Dt4)
        If Dt4.Rows.Count > 0 Then
            NoofInpBmsInLom = Val(Dt4.Rows(0).Item("Noof_Input_Beams").ToString)
            vMULTI_ENDSCNT_SELC_STS = Val(Dt4.Rows(0).Item("Multiple_EndsCount_Selection_Status").ToString)
        End If
        Dt4.Clear()
        'NoofInpBmsInLom = Common_Procedures.get_FieldValue(con, "Loom_Head", "Noof_Input_Beams", "(Loom_IdNo = " & Str(Val(Lm_ID)) & ")")
        If Val(NoofInpBmsInLom) = 0 Then NoofInpBmsInLom = 1

        BmCnt = 0
        For i = 0 To dgv_Selection.Rows.Count - 1
            If Val(dgv_Selection.Rows(i).Cells(5).Value) = 1 Then
                BmCnt = BmCnt + 1
            End If
        Next
        If BmCnt <> 0 And BmCnt <> NoofInpBmsInLom Then
            MessageBox.Show("Select " & NoofInpBmsInLom & " Beams", "DOES NOT SELECT BEAM...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        lbl_SetCode1.Text = ""
        lbl_SetNo1.Text = ""
        lbl_BeamNo1.Text = ""
        lbl_EndsCount_Beam1.Text = ""
        lbl_Meters1.Text = ""
        lbl_SetCode2.Text = ""
        lbl_SetNo2.Text = ""
        lbl_BeamNo2.Text = ""
        lbl_EndsCount_Beam2.Text = ""
        lbl_Meters2.Text = ""

        With dgv_Selection
            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(5).Value) = 1 Then

                    If Trim(lbl_SetCode1.Text) = "" And Trim(lbl_BeamNo1.Text) = "" Then
                        lbl_SetCode1.Text = .Rows(i).Cells(6).Value
                        lbl_SetNo1.Text = .Rows(i).Cells(1).Value
                        lbl_BeamNo1.Text = .Rows(i).Cells(2).Value
                        lbl_EndsCount_Beam1.Text = .Rows(i).Cells(8).Value
                        lbl_Meters1.Text = Format(Val(.Rows(i).Cells(4).Value), "#########0.00")  'Format(Val(.Rows(i).Cells(3).Value), "#########0.00")

                        lbl_Weaver_Job_No.Text = .Rows(i).Cells(7).Value

                        Da = New SqlClient.SqlDataAdapter("select a.*, tM.Mill_Name from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Mill_Head tM ON a.mill_idno = tM.mill_idno Where a.Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and a.Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            lbl_MillName.Text = Dt1.Rows(0).Item("Mill_Name").ToString
                            lbl_WarpLotNo.Text = Dt1.Rows(0).Item("Warp_LotNo").ToString
                        Else
                            lbl_MillName.Text = ""
                            lbl_WarpLotNo.Text = ""
                        End If

                        Dt1.Clear()

                    Else


                        lbl_SetCode2.Text = .Rows(i).Cells(6).Value
                        lbl_SetNo2.Text = .Rows(i).Cells(1).Value
                        lbl_BeamNo2.Text = .Rows(i).Cells(2).Value
                        lbl_EndsCount_Beam2.Text = .Rows(i).Cells(8).Value
                        lbl_Meters2.Text = Format(Val(.Rows(i).Cells(4).Value), "#########0.00")   'Format(Val(.Rows(i).Cells(3).Value), "#########0.00")

                        Exit For

                    End If
                End If
            Next
        End With


        If BmCnt = 2 And Trim(UCase(lbl_EndsCount_Beam1.Text)) <> Trim(UCase(lbl_EndsCount_Beam2.Text)) Then
            If Val(vMULTI_ENDSCNT_SELC_STS) = 0 Then

                lbl_SetCode1.Text = ""
                lbl_SetNo1.Text = ""
                lbl_BeamNo1.Text = ""
                lbl_EndsCount_Beam1.Text = ""
                lbl_Meters1.Text = ""
                lbl_SetCode2.Text = ""
                lbl_SetNo2.Text = ""
                lbl_BeamNo2.Text = ""
                lbl_EndsCount_Beam2.Text = ""
                lbl_Meters2.Text = ""

                MessageBox.Show("Don't select beams with different Ends/Count" & Chr(13) & Chr(13) & "Change the settings in Loom creation to select two beams", "DOES NOT SELECT BEAM...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If
        End If

        pnl_back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_WidthType.Enabled And cbo_WidthType.Visible Then
            cbo_WidthType.Focus()
        Else
            cbo_KnotterName.Focus()
        End If

    End Sub

    Private Sub cbo_WidthType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.TextChanged

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            cbo_WidthType.Text = ""
            cbo_ClothName1.Text = ""
            cbo_ClothName2.Text = ""
            cbo_ClothName3.Text = ""
            cbo_ClothName4.Text = ""
            cbo_WidthType.Enabled = False
            cbo_ClothName1.Enabled = False
            cbo_ClothName2.Enabled = False
            cbo_ClothName3.Enabled = False
            cbo_ClothName4.Enabled = False

        Else

            If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Or Trim(UCase(cbo_WidthType.Text)) = "FIVE" Or Trim(UCase(cbo_WidthType.Text)) = "SIX" Then
                cbo_ClothName1.Enabled = True
                cbo_ClothName2.Enabled = True
                cbo_ClothName3.Enabled = True
                cbo_ClothName4.Enabled = True


                cbo_ClothSales_OrderNo.Enabled = True
                cbo_ClothSales_OrderNo_Quality_2.Enabled = True
                cbo_ClothSales_OrderNo_Quality_3.Enabled = True
                cbo_ClothSales_OrderNo_Quality_4.Enabled = True

            ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Then
                cbo_ClothName1.Enabled = True
                cbo_ClothName2.Enabled = True
                cbo_ClothName3.Enabled = True

                cbo_ClothName4.Text = ""
                cbo_ClothName4.Enabled = False

                cbo_ClothSales_OrderNo.Enabled = True
                cbo_ClothSales_OrderNo_Quality_2.Enabled = True
                cbo_ClothSales_OrderNo_Quality_3.Enabled = True

                cbo_ClothSales_OrderNo_Quality_4.Text = ""
                cbo_ClothSales_OrderNo_Quality_4.Enabled = False

            ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Then
                cbo_ClothName3.Text = ""
                cbo_ClothName4.Text = ""
                cbo_ClothName2.Enabled = True
                cbo_ClothName3.Enabled = False
                cbo_ClothName4.Enabled = False


                cbo_ClothSales_OrderNo_Quality_3.Text = ""
                cbo_ClothSales_OrderNo_Quality_4.Text = ""

                cbo_ClothSales_OrderNo_Quality_2.Enabled = True
                cbo_ClothSales_OrderNo_Quality_3.Enabled = False
                cbo_ClothSales_OrderNo_Quality_4.Enabled = False

            Else

                cbo_ClothName2.Text = ""
                cbo_ClothName3.Text = ""
                cbo_ClothName4.Text = ""
                cbo_ClothName2.Enabled = False
                cbo_ClothName3.Enabled = False
                cbo_ClothName4.Enabled = False

                cbo_ClothSales_OrderNo_Quality_2.Text = ""
                cbo_ClothSales_OrderNo_Quality_3.Text = ""
                cbo_ClothSales_OrderNo_Quality_4.Text = ""
                cbo_ClothSales_OrderNo_Quality_2.Enabled = False
                cbo_ClothSales_OrderNo_Quality_3.Enabled = False
                cbo_ClothSales_OrderNo_Quality_4.Enabled = False

            End If

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
                    If i >= 10 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 9

                    Exit For

                End If
            Next

            txt_SetNoSelection.Text = ""
            txt_BeamNoSelection.Text = ""
            If txt_SetNoSelection.Enabled = True Then txt_SetNoSelection.Focus()

        End If
    End Sub

    Private Sub Grid_Selection(ByVal RwIndx As Integer)
        'Dim i As Integer

        'With dgv_Selection

        '    If .RowCount > 0 And RwIndx >= 0 Then

        '        .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

        '        If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then

        '            .Rows(RwIndx).Cells(5).Value = ""
        '            .CurrentCell = .Rows(RwIndx).Cells(0)
        '            If RwIndx >= 10 Then .FirstDisplayedScrollingRowIndex = RwIndx - 9

        '            For i = 0 To .ColumnCount - 1
        '                .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
        '            Next

        '        Else
        '            For i = 0 To .ColumnCount - 1
        '                .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
        '            Next

        '        End If

        '    End If
        '    If txt_SetNoSelection.Enabled = True Then txt_SetNoSelection.Focus()

        'End With

    End Sub

    Private Sub cbo_KnotterName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KnotterName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then
                Dim f As New EmployeeCreation_Simple

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_KnotterName.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            Else
                Dim f As New Payroll_Employee_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_KnotterName.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If


        End If

    End Sub

    Private Sub txt_Wages_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_WagesAmount.KeyDown
        If e.KeyValue = 40 Then
            btn_save.Focus()
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If

        End If

        If (e.KeyValue = 38) Then cbo_KnotterName.Focus()
    End Sub

    Private Sub txt_Wages_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WagesAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_save.Focus()
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_Shift.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            If txt_WagesAmount.Visible And txt_WagesAmount.Enabled Then
                txt_WagesAmount.Focus()
            Else
                cbo_ClothName1.Focus()
            End If
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_Shift.Focus() ' cbo_PartyName.Focus()
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If


    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
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
    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_ClothSales_OrderNo_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderNo.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")
    End Sub

    Private Sub cbo_ClothSales_OrderNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothSales_OrderNo.KeyDown

        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_EndsCount, cbo_LoomNo, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_PartyName, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")

        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            cbo_ClothName1.Focus()

        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_ClothName2.Visible And cbo_ClothName2.Enabled Then
                cbo_ClothName2.Focus()
            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If


        End If

    End Sub

    Private Sub cbo_ClothSales_OrderNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothSales_OrderNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")
        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothName2.Visible And cbo_ClothName2.Enabled Then
                cbo_ClothName2.Focus()
            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                    End If
                End If
            End If

    End Sub

    Private Sub cbo_EndsCount_Enter(sender As Object, e As EventArgs) Handles cbo_EndsCount.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0 )")
    End Sub

    Private Function Calculate_Fabric_Meters(vTotPavuMtrs As String) As String
        Dim vWidthVal As Integer = 0
        Dim vTotPvuMtrs As Single = 0
        Dim vTotPvuStk As Single = 0
        Dim vNoofBeams As Integer = 0
        Dim vDEFBMS As Integer = 0
        Dim vWdTyp As Integer = 0
        Dim vClo_IdNo As Integer
        Dim vTotPvuStkAlLoomMtr As String = 0
        Dim vFABMTRS As String = 0
        Dim Crmp_Perc As String, Crmp_Mtrs As String = 0

        vFABMTRS = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            vDEFBMS = 1
        Else
            vDEFBMS = 2
        End If

        vNoofBeams = vDEFBMS
        vWdTyp = vDEFBMS
        If Trim(cbo_WidthType.Text) <> "" Then
            If InStr(1, Trim(UCase(cbo_WidthType.Text)), "1 BEAM") > 0 Then
                vNoofBeams = 1
            ElseIf InStr(1, Trim(UCase(cbo_WidthType.Text)), "2 BEAM") > 0 Then
                vNoofBeams = 2
            End If

            If Trim(UCase(cbo_WidthType.Text)) = "SIX" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "SIX") > 0 Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "SIX") > 0 Then
                vWdTyp = 6
            ElseIf Trim(UCase(cbo_WidthType.Text)) = "FIVE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "FIVE") > 0 Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "FIVE") > 0 Then
                vWdTyp = 5
            ElseIf Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "FOURTH") > 0 Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "FOUR") > 0 Then
                vWdTyp = 4
            ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "TRIPLE") > 0 Then
                vWdTyp = 3
            ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "DOUBLE") > 0 Then
                vWdTyp = 2
            ElseIf Trim(UCase(cbo_WidthType.Text)) = "SINGLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "SINGLE") > 0 Then
                vWdTyp = 1
            End If


        End If

        vTotPvuStkAlLoomMtr = Format(vTotPavuMtrs / vNoofBeams * vWdTyp, "###########0.00")

        vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName1.Text)
        Crmp_Perc = 0
        If vClo_IdNo <> 0 Then
            Crmp_Perc = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Crimp_Percentage", "(Cloth_IdNo = " & Str(Val(vClo_IdNo)) & ")"))
            Crmp_Perc = Format(Val(Crmp_Perc), "##########0.00")
        End If

        Crmp_Mtrs = Format(Val(vTotPvuStkAlLoomMtr) * Val(Crmp_Perc) / 100, "##########0.00")

        vFABMTRS = Format(Val(vTotPvuStkAlLoomMtr) - Val(Crmp_Mtrs), "##########0.00")

        Calculate_Fabric_Meters = vFABMTRS

    End Function

    Private Sub txt_Shiftmetrs_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Shiftmetrs.KeyDown
        If e.KeyValue = 38 Then
            cbo_Shift.Focus()

        End If


        If e.KeyValue = 40 Then


            If cbo_PartyName.Enabled And Visible Then

                cbo_PartyName.Focus()

            Else

                cbo_WidthType.Focus()

            End If

        End If
    End Sub

    Private Sub txt_Shiftmetrs_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Shiftmetrs.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True




        If Asc(e.KeyChar) = 13 Then


            If cbo_PartyName.Enabled And Visible Then

                cbo_PartyName.Focus()

            Else

                cbo_WidthType.Focus()

            End If

        End If


    End Sub

    Private Sub btn_Save_ShiftMeters_Click(sender As Object, e As EventArgs) Handles btn_Save_ShiftMeters.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim vSHFTIDNO As Integer

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        vSHFTIDNO = Common_Procedures.Shift_NameToIdNo(con, cbo_Shift.Text)

        cmd.Connection = con

        cmd.CommandText = "Update Beam_Knotting_Head set Shift = '" & Trim(cbo_Shift.Text) & "' , Shift_IdNo = " & Val(vSHFTIDNO) & ", Shift_Meters = " & Val(txt_Shiftmetrs.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        MessageBox.Show("Shift Meters Updated Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        move_record(lbl_RefNo.Text)

        If msk_Date.Enabled And msk_Date.Visible Then
            msk_Date.Focus()
        Else
            txt_Shiftmetrs.Focus()
        End If

    End Sub

    Private Sub cbo_ClothSales_OrderNo_Quality_2_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderNo_Quality_2.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")

        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            cbo_ClothName2.Focus()

        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_ClothName3.Visible And cbo_ClothName3.Enabled Then
                cbo_ClothName3.Focus()
            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If


        End If
    End Sub

    Private Sub cbo_ClothSales_OrderNo_Quality_2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderNo_Quality_2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")


        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothName3.Visible And cbo_ClothName3.Enabled Then
                cbo_ClothName3.Focus()
            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_ClothSales_OrderNo_Quality_2_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderNo_Quality_2.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")
    End Sub

    Private Sub cbo_ClothSales_OrderNo_Quality_3_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderNo_Quality_3.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")

        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            cbo_ClothName3.Focus()

        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_ClothName4.Visible And cbo_ClothName4.Enabled Then
                cbo_ClothName4.Focus()

            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If


        End If

    End Sub

    Private Sub cbo_ClothSales_OrderNo_Quality_3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderNo_Quality_3.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")


        If Asc(e.KeyChar) = 13 Then

            If cbo_ClothName4.Visible And cbo_ClothName4.Enabled Then

                cbo_ClothName4.Focus()

            ElseIf cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then

                cbo_KnotterName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_ClothSales_OrderNo_Quality_3_GotFocus(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderNo_Quality_3.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")
    End Sub

    Private Sub cbo_ClothSales_OrderNo_Quality_4_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderNo_Quality_4.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")

        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            cbo_ClothName4.Focus()

        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If

        End If




    End Sub

    Private Sub cbo_ClothSales_OrderNo_Quality_4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderNo_Quality_4.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")


        If Asc(e.KeyChar) = 13 Then

            If cbo_KnotterName.Visible And cbo_KnotterName.Enabled Then
                cbo_KnotterName.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_ClothSales_OrderNo_Quality_4_GotFocus(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderNo_Quality_4.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")
    End Sub

    Private Sub btn_Close_Selection2_Click(sender As Object, e As EventArgs) Handles btn_Close_Selection2.Click
        Close_Selection()
    End Sub

    Private Sub get_ClothName_Condition(ByRef vCLO_CONDT As String, ByRef vFIRST_CLONAME As String, Optional cbobx As ComboBox = Nothing)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim EdsCnt_ID As Integer
        Dim Nr As Integer

        vCLO_CONDT = ""
        vFIRST_CLONAME = ""

        EdsCnt_ID = 0
        If cbo_EndsCount.Visible Then
            EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        ElseIf lbl_EndsCount_Beam1.Visible Then
            EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount_Beam1.Text)
        End If

        If EdsCnt_ID <> 0 Then

            vCLO_CONDT = "( cloth_idno IN ( select sq1.cloth_idno from Cloth_EndsCount_Details sq1 Where sq1.EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ") )"

            If IsNothing(cbobx) = False Then

                If Trim(cbobx.Text) = "" Then

                    Nr = 0
                    da1 = New SqlClient.SqlDataAdapter("select COUNT(*) from Cloth_EndsCount_Details Where EndsCount_IdNo = " & Str(Val(EdsCnt_ID)), con)
                    dt1 = New DataTable
                    da1.Fill(dt1)
                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            Nr = Val(dt1.Rows(0)(0).ToString)
                        End If
                    End If
                    dt1.Clear()

                    If Nr = 1 Then
                        da1 = New SqlClient.SqlDataAdapter("select b.cloth_name from Cloth_EndsCount_Details a, cloth_head b Where a.EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & " and a.cloth_idno = b.cloth_idno Order by b.cloth_name", con)
                        dt1 = New DataTable
                        da1.Fill(dt1)
                        If dt1.Rows.Count > 0 Then
                            vFIRST_CLONAME = dt1.Rows(0).Item("cloth_name").ToString
                        End If
                        dt1.Clear()
                    End If

                End If

            End If

        End If

    End Sub

    Private Sub cbo_EndsCount_GotFocus(sender As Object, e As EventArgs) Handles cbo_EndsCount.GotFocus
        cbo_EndsCount.Tag = cbo_EndsCount.Text
    End Sub

    Private Sub cbo_EndsCount_LostFocus(sender As Object, e As EventArgs) Handles cbo_EndsCount.LostFocus
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim vSTS As Boolean

        If Trim(UCase(cbo_EndsCount.Text)) <> "" And Trim(UCase(cbo_EndsCount.Tag)) <> Trim(UCase(cbo_EndsCount.Text)) Then
            If Trim(cbo_ClothName1.Text) <> "" Then
                vSTS = Check_Endscount_in_ClothName_Status(cbo_ClothName1.Text, cbo_EndsCount.Text)
                If vSTS = False Then
                    cbo_ClothName1.Text = ""
                End If
            End If
            If Trim(cbo_ClothName2.Text) <> "" Then
                vSTS = Check_Endscount_in_ClothName_Status(cbo_ClothName2.Text, cbo_EndsCount.Text)
                If vSTS = False Then
                    cbo_ClothName2.Text = ""
                End If
            End If
            If Trim(cbo_ClothName3.Text) <> "" Then
                vSTS = Check_Endscount_in_ClothName_Status(cbo_ClothName3.Text, cbo_EndsCount.Text)
                If vSTS = False Then
                    cbo_ClothName3.Text = ""
                End If
            End If
            If Trim(cbo_ClothName4.Text) <> "" Then
                vSTS = Check_Endscount_in_ClothName_Status(cbo_ClothName4.Text, cbo_EndsCount.Text)
                If vSTS = False Then
                    cbo_ClothName4.Text = ""
                End If
            End If
        End If

    End Sub

    Private Function Check_Endscount_in_ClothName_Status(ByVal vCLONAME As String, ByVal vENDSCNTNAME As String) As Boolean
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim vClo_ID As Integer
        Dim EdsCnt_ID As Integer
        Dim STS As Boolean = False

        STS = False
        If Trim(UCase(vCLONAME)) <> "" And Trim(UCase(vENDSCNTNAME)) <> "" Then

            vClo_ID = Common_Procedures.Cloth_NameToIdNo(con, vCLONAME)
            EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
            da1 = New SqlClient.SqlDataAdapter("select * from Cloth_EndsCount_Details a Where a.cloth_idno = " & Str(Val(vClo_ID)) & " and a.EndsCount_IdNo = " & Str(Val(EdsCnt_ID)), con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                STS = True
            End If
            dt1.Clear()

        End If

        Return STS

    End Function

    Private Sub cbo_ClothName1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothName1.SelectedIndexChanged

    End Sub
End Class
