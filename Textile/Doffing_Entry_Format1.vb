Imports Microsoft

Public Class Doffing_Entry_Format1
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Other_Condition As String = ""
    Private Pk_Condition As String = "PCDOF-"
    Private PkCondition2_INCHK As String = "INCHK-"
    Private PkCondition3_CRCHK As String = "CRCHK-"
    Private PkCondition4_GWEWA As String = "GWEWA-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private SaveAll_Sts As Boolean = False

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_DetBarCdStkr As Integer
    Private LastNo As String = ""
    Private prn_HeadIndx As Integer
    Private vSql_Cond = ""

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
        pnl_Weft_Consumption_Details.Visible = False
        btn_Show_WeftConsumption_Details.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RollNo.Text = ""
        lbl_RollNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        lbl_WeftCount.Text = ""
        lbl_PartyName.Text = ""
        lbl_PartyName.Tag = ""
        cbo_ClothName.Text = ""
        cbo_ClothName.Tag = ""
        lbl_WidthType.Text = ""
        lbl_EndsCount.Text = ""
        txt_BarCode.Text = ""
        txt_BarCode.Enabled = False

        lbl_KnotNo.Text = ""
        lbl_KnotCode.Text = ""
        cbo_Pcs_LastPiece_Status.Text = "NO"
        cbo_Pcs_LastPiece_Status.Tag = ""
        lbl_SetCode1.Text = ""
        lbl_SetNo1.Text = ""
        lbl_TotMtrs1.Text = ""
        lbl_TotMtrs2.Text = ""
        lbl_SetCode2.Text = ""
        lbl_SetNo2.Text = ""
        cbo_LoomNo.Text = ""
        cbo_LoomNo.Tag = ""
        lbl_BeamNo1.Text = ""
        lbl_BeamNo2.Text = ""
        txt_DoffMtrs.Text = ""
        txt_CrimpPerc.Text = ""
        txt_Doff_Shift_Meters.Text = ""
        cbo_DoffShift.Text = ""
        lbl_ConsPavu.Text = ""
        lbl_BeamConsPavu.Text = ""
        lbl_ConsWeftYarn.Text = ""
        lbl_BalMtrs1.Text = ""
        lbl_BalMtrs2.Text = ""
        cbo_SelectionLoomNo.Text = ""

        lbl_WarpMillName.Text = ""
        lbl_WarpLotNo.Text = ""
        cbo_Weft_MillName.Text = ""
        cbo_Weft_MillName.Tag = ""
        cbo_WeftLotNo.Text = ""
        cbo_WeftLotNo.Tag = ""
        lbl_FabricLotNo.Text = ""

        cbo_ClothSales_OrderNo.Items.Clear()
        cbo_ClothSales_OrderNo.Text = ""

        cbo_PanelQuality.Items.Clear()
        cbo_PanelQuality.Text = ""

        lbl_PoNo.Text = ""
        cbo_WeftLotNo.Text = ""
        cbo_WeftLotNo.Tag = ""

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRIC
        '    cbo_PanelQuality.Items.Clear()
        '    cbo_PanelQuality.Items.Add("")
        '    cbo_PanelQuality.Items.Add("A")
        '    cbo_PanelQuality.Items.Add("B")
        '    cbo_PanelQuality.Items.Add("C")
        '    cbo_PanelQuality.Items.Add("D")
        'Else
        '    cbo_PanelQuality.Items.Clear()
        '    cbo_PanelQuality.Items.Add("")
        '    cbo_PanelQuality.Items.Add("1")
        '    cbo_PanelQuality.Items.Add("2")
        '    cbo_PanelQuality.Items.Add("3")
        '    cbo_PanelQuality.Items.Add("4")
        'End If

        pnl_Weft_Consumption_Details.Visible = False
        dgv_Weft_Consumption_Details.Rows.Clear()

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        cbo_LoomNo.Enabled = True
        cbo_LoomNo.BackColor = Color.White

        cbo_ClothName.Enabled = True
        cbo_ClothName.BackColor = Color.White

        txt_DoffMtrs.Enabled = True
        txt_DoffMtrs.BackColor = Color.White

        txt_CrimpPerc.Enabled = True
        txt_CrimpPerc.BackColor = Color.White

        cbo_DoffShift.Enabled = True
        cbo_DoffShift.BackColor = Color.White

        txt_Doff_Shift_Meters.Enabled = True
        txt_Doff_Shift_Meters.BackColor = Color.White

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
                Prec_ActCtrl.BackColor = Color.FromArgb(2, 57, 111)
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
        dgv_filter.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False
        Dim BmRunOutCd As String = ""
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt4 As New DataTable
        Dim n As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("Select a.*, b.Ledger_Name, c.Cloth_Name, c.Multiple_WeftCount_Status, d.EndsCount_Name, e.Count_Name, f.Loom_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Count_Head e ON a.Count_IdNo = e.Count_IdNo LEFT OUTER JOIN Loom_Head f ON a.Loom_IdNo = f.Loom_IdNo Where a.Receipt_Type = 'L' and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and " & Other_Condition, con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RollNo.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
                msk_Date.Text = dtp_Date.Text
                lbl_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_LoomNo.Text = dt1.Rows(0).Item("Loom_Name").ToString
                cbo_LoomNo.Tag = cbo_LoomNo.Text
                cbo_Pcs_LastPiece_Status.Text = dt1.Rows(0).Item("Is_LastPiece").ToString
                cbo_Pcs_LastPiece_Status.Tag = cbo_Pcs_LastPiece_Status.Text
                txt_BarCode.Text = dt1.Rows(0).Item("Bar_Code").ToString

                lbl_KnotCode.Text = dt1.Rows(0).Item("Beam_Knotting_Code").ToString
                lbl_KnotNo.Text = dt1.Rows(0).Item("Beam_Knotting_No").ToString

                cbo_ClothName.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                lbl_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
                lbl_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString

                'lbl_WeftCount.Text = dt1.Rows(0).Item("Count_Name").ToString
                If Val(dt1.Rows(0).Item("Multiple_WeftCount_Status").ToString) = 1 Then
                    btn_Show_WeftConsumption_Details.Visible = True
                    btn_Show_WeftConsumption_Details.BringToFront()
                    lbl_WeftCount.Text = ""
                Else
                    lbl_WeftCount.Text = dt1.Rows(0).Item("Count_Name").ToString
                    btn_Show_WeftConsumption_Details.Visible = False
                End If

                lbl_SetCode1.Text = dt1.Rows(0).Item("Set_Code1").ToString
                lbl_SetNo1.Text = dt1.Rows(0).Item("Set_No1").ToString
                lbl_BeamNo1.Text = dt1.Rows(0).Item("Beam_No1").ToString
                lbl_BalMtrs1.Text = dt1.Rows(0).Item("Balance_Meters1").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                lbl_TotMtrs1.Text = ""
                lbl_WarpMillName.Text = ""
                lbl_WarpLotNo.Text = ""
                da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    lbl_TotMtrs1.Text = dt2.Rows(0).Item("Meters").ToString
                    lbl_WarpMillName.Text = Common_Procedures.Mill_IdNoToName(con, Val(dt2.Rows(0).Item("Mill_IdNo").ToString))
                    lbl_WarpLotNo.Text = dt2.Rows(0).Item("Warp_LotNo").ToString
                End If
                dt2.Clear()

                lbl_SetCode2.Text = dt1.Rows(0).Item("Set_Code2").ToString
                lbl_SetNo2.Text = dt1.Rows(0).Item("Set_No2").ToString
                lbl_BeamNo2.Text = dt1.Rows(0).Item("Beam_No2").ToString
                lbl_BalMtrs2.Text = dt1.Rows(0).Item("Balance_Meters2").ToString
                lbl_TotMtrs2.Text = ""
                da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    lbl_TotMtrs2.Text = dt2.Rows(0).Item("Meters").ToString
                End If
                dt2.Clear()

                cbo_DoffShift.Text = Common_Procedures.Shift_IdNoToName(con, Val(dt1.Rows(0).Item("Doff_Shift_IdNo").ToString))
                txt_Doff_Shift_Meters.Text = dt1.Rows(0).Item("Doff_Shift_Meters").ToString

                txt_DoffMtrs.Text = dt1.Rows(0).Item("ReceiptMeters_Receipt").ToString
                txt_CrimpPerc.Text = dt1.Rows(0).Item("Crimp_Percentage").ToString
                lbl_ConsPavu.Text = dt1.Rows(0).Item("ConsumedPavu_Receipt").ToString
                lbl_ConsWeftYarn.Text = dt1.Rows(0).Item("ConsumedYarn_Receipt").ToString
                lbl_BeamConsPavu.Text = dt1.Rows(0).Item("BeamConsumption_Receipt").ToString

                lbl_WarpMillName.Text = Common_Procedures.Mill_IdNoToName(con, Val(dt1.Rows(0).Item("Warp_Mill_IdNo").ToString))
                lbl_WarpLotNo.Text = dt1.Rows(0).Item("Warp_LotNo").ToString
                cbo_Weft_MillName.Text = Common_Procedures.Mill_IdNoToName(con, Val(dt1.Rows(0).Item("Weft_Mill_IdNo").ToString))
                cbo_Weft_MillName.Tag = cbo_Weft_MillName.Text
                cbo_WeftLotNo.Text = dt1.Rows(0).Item("Weft_LotNo").ToString
                cbo_WeftLotNo.Tag = cbo_WeftLotNo.Text
                lbl_FabricLotNo.Text = dt1.Rows(0).Item("Fabric_LotNo").ToString
                cbo_ClothSales_OrderNo.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString
                cbo_PanelQuality.Text = dt1.Rows(0).Item("Panel_Quality").ToString
                lbl_PoNo.Text = dt1.Rows(0).Item("Po_No").ToString


                dgv_Weft_Consumption_Details.Rows.Clear()
                da1 = New SqlClient.SqlDataAdapter("Select a.*, b.count_name from Weaver_ClothReceipt_Consumed_Yarn_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt4 = New DataTable
                da1.Fill(dt4)
                If dt4.Rows.Count > 0 Then
                    For i = 0 To dt4.Rows.Count - 1

                        n = dgv_Weft_Consumption_Details.Rows.Add()
                        dgv_Weft_Consumption_Details.Rows(n).Cells(0).Value = dt4.Rows(i).Item("count_name").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(1).Value = dt4.Rows(i).Item("Gram_Perc_Type").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(2).Value = Val(dt4.Rows(i).Item("Consumption_Gram_Perc").ToString)
                        dgv_Weft_Consumption_Details.Rows(n).Cells(3).Value = Format(Val(dt4.Rows(i).Item("Consumed_Yarn_Weight").ToString), "#########0.000")

                    Next

                End If
                dt4.Clear()


                'BmRunOutCd = Common_Procedures.get_FieldValue(con, "Beam_Knotting_Head", "Beam_RunOut_Code", "(Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "')")
                'If BmRunOutCd <> "" Then
                '    LockSTS = True
                'End If

                If IsDBNull(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                If LockSTS = True Then
                    cbo_LoomNo.Enabled = False
                    cbo_LoomNo.BackColor = Color.LightGray

                    'cbo_ClothName.Enabled = False
                    'cbo_ClothName.BackColor = Color.LightGray

                    'txt_DoffMtrs.Enabled = False
                    'txt_DoffMtrs.BackColor = Color.LightGray

                    'txt_CrimpPerc.Enabled = False
                    'txt_CrimpPerc.BackColor = Color.LightGray
                End If





            Else
                new_record()

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Doffing_Entry_Format1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(lbl_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                lbl_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(lbl_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                lbl_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LoomNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_LoomNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Weft_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Weft_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Doffing_Entry_Format1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable

        Me.Text = ""

        lbl_Heading.Text = "ROLL  DOFFING  ENTRY"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then '---- SAMANTH TEXTILES (SOMANUR)
            lbl_Heading.Text = "PIECE DOFFING  ENTRY"
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1158" Then '---- NIDHIE WEAVING (PALLADAM)
            Other_Condition = "(Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition2_INCHK) & "%' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition3_CRCHK) & "%' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition4_GWEWA) & "%')"
        Else
            Other_Condition = "(Receipt_Type = 'L' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition2_INCHK) & "%' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition3_CRCHK) & "%' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition4_GWEWA) & "%')"
        End If

        lbl_RollNo_Caption.Text = StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase)

        con.Open()


        lbl_PoNo_Caption.Visible = False
        lbl_PoNo.Visible = False

        If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then '---ARULJOTHI EXPORTS PVT LTD (SOMANUR)
            lbl_PartyName.Width = 327
            txt_BarCode.Visible = True
            lbl_BarCode.Visible = True
        End If

        chk_Show_All_Knottings.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            chk_Show_All_Knottings.Visible = True
        End If

        btn_Save_SalesOrderNo.Visible = False

        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)

            'lbl_WarpMillName_Caption.Visible = True
            'lbl_WarpMillName_Caption.Left = lbl_SetNo1_Caption.Left
            'lbl_WarpMillName_Caption.Top = lbl_SetNo1_Caption.Top
            'lbl_WarpMillName.Visible = True
            'lbl_WarpMillName.BackColor = lbl_SetNo1.BackColor
            'lbl_WarpMillName.Left = lbl_SetNo1.Left
            'lbl_WarpMillName.Top = lbl_SetNo1.Top
            'lbl_WarpMillName.Width = lbl_SetNo1.Width

            'lbl_WarpLotNo_Caption.Visible = True
            'lbl_WarpLotNo_Caption.Left = lbl_TotMtrs1_Caption.Left
            'lbl_WarpLotNo_Caption.Top = lbl_TotMtrs1_Caption.Top
            'lbl_WarpLotNo.Visible = True
            'lbl_WarpLotNo.BackColor = lbl_TotMtrs1.BackColor
            'lbl_WarpLotNo.Left = lbl_TotMtrs1.Left
            'lbl_WarpLotNo.Top = lbl_TotMtrs1.Top
            'lbl_WarpLotNo.Width = lbl_TotMtrs1.Width


            lbl_Weft_MillName_Caption.Visible = True
            lbl_Weft_MillName_Caption.Left = lbl_SetNo2_Caption.Left
            lbl_Weft_MillName_Caption.Top = lbl_SetNo2_Caption.Top
            cbo_Weft_MillName.Visible = True
            cbo_Weft_MillName.BackColor = Color.White
            cbo_Weft_MillName.Left = lbl_SetNo2.Left
            cbo_Weft_MillName.Top = lbl_SetNo2.Top
            cbo_Weft_MillName.Width = lbl_SetNo2.Width

            'lbl_WeftLotNo_Caption.Visible = True
            'lbl_WeftLotNo_Caption.Left = lbl_TotMtrs2_Caption.Left
            'lbl_WeftLotNo_Caption.Top = lbl_TotMtrs2_Caption.Top
            'txt_WeftLotNo.Visible = True
            'txt_WeftLotNo.BackColor = Color.White
            'txt_WeftLotNo.Left = lbl_TotMtrs2.Left
            'txt_WeftLotNo.Top = lbl_TotMtrs2.Top
            'txt_WeftLotNo.Width = lbl_TotMtrs2.Width

            lbl_FabricLotNo_Caption.Visible = True
            lbl_FabricLotNo_Caption.Left = lbl_ConsWeftYarn_Caption.Left
            lbl_FabricLotNo_Caption.Top = lbl_ConsPavu_Caption.Top ' lbl_ConsWeftYarn_Caption.Top
            lbl_FabricLotNo.Visible = True
            lbl_FabricLotNo.BackColor = lbl_ConsWeftYarn.BackColor
            lbl_FabricLotNo.Left = lbl_ConsWeftYarn.Left
            lbl_FabricLotNo.Top = lbl_ConsWeftYarn.Top
            lbl_FabricLotNo.Width = lbl_ConsWeftYarn.Width


            lbl_BeamNo1.Width = cbo_DoffShift.Width
            lbl_SetNo1_Caption.Left = lbl_Doff_Shift_Meters_Caption.Left
            lbl_SetNo1.Left = txt_Doff_Shift_Meters.Left
            lbl_SetNo1.Width = txt_Doff_Shift_Meters.Width
            lbl_SetNo1_Caption.Visible = True
            lbl_SetNo1.Visible = True

            lbl_BalMtrs1.Width = cbo_DoffShift.Width
            lbl_TotMtrs1_Caption.Left = lbl_Doff_Shift_Meters_Caption.Left
            lbl_TotMtrs1.Left = txt_Doff_Shift_Meters.Left
            lbl_TotMtrs1.Width = txt_Doff_Shift_Meters.Width
            lbl_TotMtrs1_Caption.Visible = True
            lbl_TotMtrs1.Visible = True

            lbl_BeamNo2.Width = cbo_DoffShift.Width
            lbl_SetNo2_Caption.Left = lbl_Doff_Shift_Meters_Caption.Left
            lbl_SetNo2.Left = txt_Doff_Shift_Meters.Left
            lbl_SetNo2.Width = txt_Doff_Shift_Meters.Width
            lbl_SetNo2_Caption.Visible = True
            lbl_SetNo2.Visible = True

            lbl_BalMtrs2.Width = cbo_DoffShift.Width
            lbl_TotMtrs2_Caption.Left = lbl_Doff_Shift_Meters_Caption.Left
            lbl_TotMtrs2.Left = txt_Doff_Shift_Meters.Left
            lbl_TotMtrs2.Top = lbl_BalMtrs2.Top
            lbl_TotMtrs2.Width = txt_Doff_Shift_Meters.Width
            lbl_TotMtrs2_Caption.Visible = True
            lbl_TotMtrs2.Visible = True

            lbl_ConsPavu.Width = cbo_DoffShift.Width
            lbl_ConsWeftYarn_Caption.Left = lbl_Doff_Shift_Meters_Caption.Left
            lbl_ConsWeftYarn.Left = txt_Doff_Shift_Meters.Left
            lbl_ConsWeftYarn.Width = txt_Doff_Shift_Meters.Width
            lbl_ConsWeftYarn_Caption.Visible = True
            lbl_ConsWeftYarn.Visible = True
            lbl_ConsWeftYarn_Caption.BringToFront()
            lbl_ConsWeftYarn.BringToFront()

            lbl_DoffShift_Caption.Visible = True
            cbo_DoffShift.Visible = True
            lbl_Doff_Shift_Meters_Caption.Visible = True
            txt_Doff_Shift_Meters.Visible = True

            lbl_DoffMtrs_Caption.Text = "Roll Meters"

            lbl_ClothSales_OrderNo_Caption.Visible = True
            cbo_ClothSales_OrderNo.Visible = True

            lbl_ClothSales_OrderNo_Caption.Location = New Point(463, 208)
            cbo_ClothSales_OrderNo.Location = New Point(566, 204)
            cbo_ClothSales_OrderNo.Size = New Size(361, 23)
            cbo_ClothSales_OrderNo.BackColor = Color.White


            lbl_WarpMillName_Caption.Visible = True
            lbl_WarpMillName.Visible = True

            lbl_WarpMillName_Caption.Location = New Point(463, 246)
            lbl_WarpMillName.Location = New Point(566, 243)
            lbl_WarpMillName.Size = New Size(361, 23)
            lbl_WarpMillName.BackColor = Color.White


            lbl_WarpLotNo_Caption.Visible = True
            lbl_WarpLotNo.Visible = True

            lbl_WarpLotNo_Caption.Location = New Point(463, 322)
            lbl_WarpLotNo.Location = New Point(567, 318)
            ' lbl_WarpLotNo.Size = New Size(361, 23)
            lbl_WarpLotNo.BackColor = Color.White


            lbl_WeftLotNo_Caption.Visible = True
            cbo_WeftLotNo.Visible = True
            cbo_WeftLotNo.BackColor = Color.White
            'lbl_WeftLotNo_Caption.Location = New Point(733, 321)
            ' txt_WeftLotNo.Location = New Point(744, 318)
            ' txt_WeftLotNo.Size = lbl_WarpLotNo.Size

            lbl_Panel.Visible = True
            cbo_PanelQuality.Visible = True
            cbo_PanelQuality.BackColor = Color.White

            lbl_Panel.Location = cbo_ClothName_Caption.Location
            cbo_PanelQuality.Location = cbo_ClothName.Location
            cbo_PanelQuality.Width = lbl_BeamNo1.Width


            cbo_ClothName_Caption.Left = lbl_SetNo1_Caption.Left
            cbo_ClothName_Caption.BringToFront()

            cbo_ClothName.Left = lbl_SetNo1.Left
            cbo_ClothName.Width = lbl_WidthType_Caption.Left - 110

            lbl_WidthType_Caption.Left = lbl_WeftLotNo_Caption.Left
            lbl_WidthType.Left = cbo_WeftLotNo.Left
            lbl_WidthType.Width = cbo_WeftLotNo.Width


            '   cbo_ClothName.DropDownWidth = StartPosition.CenterParent

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1464" Then
                cbo_WeftLotNo.DropDownStyle = cbo_WeftLotNo.DropDownStyle.Simple
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                btn_Save_SalesOrderNo.Visible = True
            End If

        Else

            lbl_DoffShift_Caption.Visible = False
            cbo_DoffShift.Visible = False
            lbl_Doff_Shift_Meters_Caption.Visible = False
            txt_Doff_Shift_Meters.Visible = False

            lbl_CrimpPerc_Caption.Left = lbl_DoffMtrs_Caption.Left
            txt_CrimpPerc.Left = lbl_TotMtrs2.Left
            txt_CrimpPerc.Width = lbl_TotMtrs2.Width

            lbl_DoffMtrs_Caption.Left = lbl_DoffShift_Caption.Left
            txt_DoffMtrs.Left = lbl_BalMtrs2.Left
            txt_DoffMtrs.Width = lbl_BalMtrs2.Width


            lbl_Panel.Visible = False
            cbo_PanelQuality.Visible = False


        End If

        cbo_Pcs_LastPiece_Status.Items.Clear()
        cbo_Pcs_LastPiece_Status.Items.Add("")
        cbo_Pcs_LastPiece_Status.Items.Add("YES")
        cbo_Pcs_LastPiece_Status.Items.Add("NO")

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_Weft_Consumption_Details.Visible = False
        pnl_Weft_Consumption_Details.Left = (Me.Width - pnl_Weft_Consumption_Details.Width) \ 2
        pnl_Weft_Consumption_Details.Top = (Me.Height - pnl_Weft_Consumption_Details.Height) \ 2


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then '---- SRI SRINIVASA TEXTILES (PALLADAM)

            lbl_WarpLotNo_Caption.Visible = True
            lbl_WarpLotNo.Visible = True

            lbl_WarpLotNo_Caption.Location = New Point(463, 322)
            lbl_WarpLotNo.Location = New Point(567, 318)

            lbl_WeftLotNo_Caption.Visible = True
            cbo_WeftLotNo.Visible = True
            cbo_WeftLotNo.BackColor = Color.White

            lbl_SetNo2.Width = lbl_WarpLotNo.Width
            lbl_PoNo_Caption.Visible = True
            lbl_PoNo.Visible = True

            lbl_TotMtrs2.Visible = False

        End If


        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DoffShift.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Doff_Shift_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DoffMtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CrimpPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BarCode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Pcs_LastPiece_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weft_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WeftLotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_ShowKnottingDetails.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_CloseKnottingDetails.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DoffMtrs.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DoffShift.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Doff_Shift_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CrimpPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BarCode.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintTo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_ShowKnottingDetails.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_CloseKnottingDetails.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Pcs_LastPiece_Status.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weft_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WeftLotNo.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DoffMtrs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BarCode.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PrintFrom.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BarCode.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PrintFrom.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_ClothSales_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PanelQuality.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PanelQuality.LostFocus, AddressOf ControlLostFocus



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


    Private Sub Doffing_Entry_Format1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Doffing_Entry_Format1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_Print_Cancel_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Weft_Consumption_Details.Visible Then
                    Call btn_Close_Weft_Consumption_Details_Click(sender, e)
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
        Dim BmRunOutCd As String = ""
        Dim vOrdByNo As String = ""
        Dim vBEAMKnot_ProdMeters As String = 0
        Dim SQL1 As String
        Dim Old_Loom_Idno As Integer
        Dim Old_SetCd1 As String, Old_Beam1 As String
        Dim Old_SetCd2 As String, Old_Beam2 As String
        Dim Old_BMKNOTCd As String
        Dim Old_CLTH_Idno As Integer
        Dim vBEAM_ProdMeters As String = 0
        Dim vErrMsg As String


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text)

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Doffing_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Doffing_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Inhouse_Doffing_Entry, New_Entry, Me, con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", NewCode, "Weaver_ClothReceipt_Date", "(Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Old_Loom_Idno = 0
        Old_SetCd1 = ""
        Old_Beam1 = ""
        Old_SetCd2 = ""
        Old_Beam2 = ""
        Old_BMKNOTCd = ""
        Old_CLTH_Idno = 0


        Da = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then


            If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" Then
                    MessageBox.Show("Already Piece Checking Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            Old_Loom_Idno = Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)
            Old_SetCd1 = Dt1.Rows(0).Item("set_code1").ToString
            Old_Beam1 = Dt1.Rows(0).Item("beam_no1").ToString
            Old_SetCd2 = Dt1.Rows(0).Item("set_code2").ToString
            Old_Beam2 = Dt1.Rows(0).Item("beam_no2").ToString
            Old_BMKNOTCd = Dt1.Rows(0).Item("Beam_Knotting_Code").ToString
            Old_CLTH_Idno = Val(Dt1.Rows(0).Item("Cloth_IdNo").ToString)


            'BmRunOutCd = Common_Procedures.get_FieldValue(con, "Beam_Knotting_Head", "Beam_RunOut_Code", "(Beam_Knotting_Code = '" & Trim(Dt1.Rows(0).Item("Beam_Knotting_Code").ToString) & "')")
            'If Trim(BmRunOutCd) <> "" Then
            '    MessageBox.Show("Already this knotting, was runout", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            '    Exit Sub
            'End If

            'Da = New SqlClient.SqlDataAdapter("Select Close_Status from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No1").ToString) & "'", con)
            'Dt2 = New DataTable
            'Da.Fill(Dt2)
            'If Dt2.Rows.Count > 0 Then
            '    If IsDBNull(Dt2.Rows(0).Item("Close_Status").ToString) = False Then
            '        If Val(Dt2.Rows(0).Item("Close_Status").ToString) <> 0 Then
            '            Throw New ApplicationException("Already this Beams was Closed")
            '            Exit Sub
            '        End If
            '    End If
            'End If
            'Dt2.Clear()

            'Da = New SqlClient.SqlDataAdapter("Select Close_Status from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No2").ToString) & "'", con)
            'Dt2 = New DataTable
            'Da.Fill(Dt2)
            'If Dt2.Rows.Count > 0 Then
            '    If IsDBNull(Dt2.Rows(0).Item("Close_Status").ToString) = False Then
            '        If Val(Dt2.Rows(0).Item("Close_Status").ToString) <> 0 Then
            '            Throw New ApplicationException("Already this Beams was Closed")
            '            Exit Sub
            '        End If
            '    End If
            'End If
            'Dt2.Clear()

        End If
        Dt1.Clear()

        tr = con.BeginTransaction
        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Weaver_Cloth_Receipt_head", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RollNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Weaver_ClothReceipt_Code, Company_IdNo, for_OrderBy", tr)

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "delete from Weaver_ClothReceipt_Consumed_Yarn_Details where Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Trim(Old_BMKNOTCd) <> "" Then
                vBEAMKnot_ProdMeters = Common_Procedures.get_BeamKnotting_TotalProductionMeters_from_Doffing(con, Trim(Old_BMKNOTCd), tr)

                SQL1 = "Update Beam_Knotting_Head set Production_Meters = " & Str(Val(vBEAMKnot_ProdMeters)) & " Where Beam_Knotting_Code = '" & Trim(Old_BMKNOTCd) & "'"
                cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                cmd.ExecuteNonQuery()
            End If
            'cmd.CommandText = "Update Beam_Knotting_Head set Production_Meters = a.Production_Meters - b.ReceiptMeters_Receipt from Beam_Knotting_Head a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Beam_Knotting_Code = b.Beam_Knotting_Code"
            'cmd.ExecuteNonQuery()

            If Trim(Old_SetCd1) <> "" And Trim(Old_Beam1) <> "" Then

                vBEAM_ProdMeters = 0
                vErrMsg = ""
                '----- Checking for negative beam meters
                If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Old_CLTH_Idno, Old_SetCd1, Old_Beam1, vBEAM_ProdMeters, vErrMsg, tr) = True Then
                    Throw New ApplicationException(vErrMsg)
                    Exit Sub

                Else

                    SQL1 = "Update Stock_SizedPavu_Processing_Details set Production_Meters = " & Str(Val(vBEAM_ProdMeters)) & " Where set_code = '" & Trim(Old_SetCd1) & "' and beam_no = '" & Trim(Old_Beam1) & "'"
                    cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                    cmd.ExecuteNonQuery()


                    'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and b.Set_code1 <> '' AND b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
                    'cmd.ExecuteNonQuery()



                End If

            End If


            If Trim(Old_SetCd2) <> "" And Trim(Old_Beam2) <> "" Then

                vBEAM_ProdMeters = 0
                vErrMsg = ""
                '----- Checking for negative beam meters
                If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Old_CLTH_Idno, Old_SetCd2, Old_Beam2, vBEAM_ProdMeters, vErrMsg, tr) = True Then
                    Throw New ApplicationException(vErrMsg)
                    Exit Sub

                Else
                    SQL1 = "Update Stock_SizedPavu_Processing_Details set Production_Meters = " & Str(Val(vBEAM_ProdMeters)) & " Where set_code = '" & Trim(Old_SetCd2) & "' and beam_no = '" & Trim(Old_Beam2) & "'"
                    cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                    cmd.ExecuteNonQuery()

                    'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' AND b.Set_code2 <> '' AND b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
                    'cmd.ExecuteNonQuery()


                End If

            End If


            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim Cmd As New SqlClient.SqlCommand
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable
            Dim dt4 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
            dt1 = New DataTable
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
            dt2 = New DataTable
            da.Fill(dt2)
            cbo_Filter_LoomNo.DataSource = dt2
            cbo_Filter_LoomNo.DisplayMember = "Loom_Name"

            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            dt3 = New DataTable
            da.Fill(dt3)
            cbo_Filter_ClothName.DataSource = dt3
            cbo_Filter_ClothName.DisplayMember = "Cloth_Name"

            da = New SqlClient.SqlDataAdapter("select BeamNo_SetCode_forSelection from Stock_SizedPavu_Processing_Details order by BeamNo_SetCode_forSelection", con)
            dt4 = New DataTable
            da.Fill(dt4)
            cbo_Filter_BeamNo.DataSource = dt4
            cbo_Filter_BeamNo.DisplayMember = "BeamNo_SetCode_forSelection"

            Common_Procedures.Update_SizedPavu_BeamNo_for_Selection(con)
            'Cmd.Connection = con
            'Cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set BeamNo_SetCode_forSelection = Beam_No + ' | ' + setcode_forSelection Where Beam_No <> ''"
            'Cmd.ExecuteNonQuery()

            dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
            dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
            pnl_filter.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.SelectedIndex = -1
            cbo_Filter_LoomNo.SelectedIndex = -1
            cbo_Filter_BeamNo.SelectedIndex = -1
            dgv_filter.Rows.Clear()

            da.Dispose()
            Cmd.Dispose()

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
        Dim movno2 As String
        Dim NewCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Doffing_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Doffing_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Inhouse_Doffing_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Roll.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where Receipt_Type = 'L'  and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition2_INCHK) & "%'"
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

            cmd.CommandText = "select Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where Receipt_Type <> 'L'  and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition2_INCHK) & "%'"
            dr = cmd.ExecuteReader

            movno2 = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno2 = dr(0).ToString
                    End If
                End If
            End If
            dr.Close()

            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(movno) <> 0 Then
                MessageBox.Show("Invalid Roll.No", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Roll.No", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RollNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try

            cmd.Connection = con
            cmd.CommandText = "select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where Receipt_Type = 'L' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby, Weaver_ClothReceipt_No"
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
            cmd.CommandText = "select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where Receipt_Type = 'L' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Weaver_ClothReceipt_No desc"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RollNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where Receipt_Type = 'L' and for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and  " & Other_Condition & " Order by for_Orderby, Weaver_ClothReceipt_No"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RollNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where Receipt_Type = 'L' and for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Weaver_ClothReceipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and  " & Other_Condition & " Order by for_Orderby desc, Weaver_ClothReceipt_No desc"
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
        Dim Barcode_Generate As String = ""

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and  " & Other_Condition, con)
            dt = New DataTable
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_RollNo.Text = NewID
            lbl_RollNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition2_INCHK) & "%' Order by for_Orderby desc, Weaver_ClothReceipt_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Weaver_ClothReceipt_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
                End If
            End If
            dt1.Clear()

            Generate_Barcode()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim movno2 As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Roll.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con



            cmd.CommandText = "select Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where Receipt_Type = 'L' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition2_INCHK) & "%'"
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

            cmd.CommandText = "select Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where Receipt_Type <> 'L' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Weaver_ClothReceipt_Code NOT LIKE '" & Trim(PkCondition2_INCHK) & "%'"
            dr = cmd.ExecuteReader
            movno2 = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno2 = dr(0).ToString
                    End If
                End If
            End If
            dr.Close()

            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(movno2) <> 0 Then
                MessageBox.Show("Roll.No. already entered in Weaver Cloth Receipt", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show("Roll.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim led_id As Integer = 0, Delv_ID As Integer = 0, Rec_ID As Integer = 0
        Dim vLedtype As String = ""
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim WftCnt_ID As Integer = 0, vWFTCNTIDno As Integer
        Dim Lm_ID As Integer = 0
        Dim Partcls As String, PBlNo As String, EntID As String
        Dim PcsChkCode As String = 0
        Dim PavuConsMtrs As Single = 0
        Dim NoofInpBmsInLom As Integer
        Dim Old_Loom_Idno As Integer
        Dim Old_SetCd1 As String, Old_Beam1 As String
        Dim Old_SetCd2 As String, Old_Beam2 As String
        Dim Old_BMKNOTCd As String
        Dim Old_CLTH_Idno As Integer
        Dim OrdByNo As Single = 0
        Dim YrnPartcls As String = ""
        Dim MasWftCnt_IDNo As Integer = 0
        Dim vErrMsg As String = ""
        Dim vStkOff_IDno As String
        Dim vSELC_LOTCODE As String = ""
        Dim vOrdByNo As String = ""
        Dim vDOFSHFTIDNO As Integer
        Dim WftMil_ID As Integer
        Dim WrpCnt_ID As Integer
        Dim WrpMil_ID As Integer
        Dim vBEAM_ProdMeters As String = 0
        Dim vBEAMKnot_ProdMeters As String = 0
        Dim SQL1 As String
        Dim sNO As Integer
        Dim vSTKPOSTING_STS As Boolean = False

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text)

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Inhouse_Doffing_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Inhouse_Doffing_Entry, New_Entry, Me, con, "Weaver_Cloth_Receipt_Head", "Weaver_Cloth_Receipt_Code", NewCode, "Weaver_Cloth_Receipt_Date", "(Weaver_Cloth_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Cloth_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Cloth_Receipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Lm_ID = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        If Trim(cbo_Pcs_LastPiece_Status.Text) = "" Then
            cbo_Pcs_LastPiece_Status.Text = "NO"
        ElseIf Trim(cbo_Pcs_LastPiece_Status.Text) <> "YES" And Trim(cbo_Pcs_LastPiece_Status.Text) <> "NO" Then
            cbo_Pcs_LastPiece_Status.Text = "NO"
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        vLedtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(led_id)) & ")")
        If Trim(UCase(vLedtype)) = "JOBWORKER" Then
            vStkOff_IDno = led_id
        Else
            vStkOff_IDno = Common_Procedures.CommonLedger.OwnSort_Ac
        End If

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        If Clo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If

        If Trim(lbl_WidthType.Text) = "" Then
            MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If

        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount.Text)
        If Val(EdsCnt_ID) = 0 Then
            MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If

        da = New SqlClient.SqlDataAdapter("select * from Cloth_EndsCount_Details Where Cloth_Idno = " & Str(Val(Clo_ID)) & " and EndsCount_IdNo = " & Str(Val(EdsCnt_ID)), con)
        dt1 = New DataTable
        da.Fill(dt1)
        If dt1.Rows.Count = 0 Then
            MessageBox.Show("Mismatch of EndsCount with Cloth Master", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If
        dt1.Clear()

        WftCnt_ID = 0
        If btn_Show_WeftConsumption_Details.Visible = False Then
            WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)
            If Val(WftCnt_ID) = 0 Then
                MessageBox.Show("Invalid Weft Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
                Exit Sub
            End If

            MasWftCnt_IDNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_IdNo = " & Str(Val(Clo_ID)) & ")")
            If Val(WftCnt_ID) <> Val(MasWftCnt_IDNo) Then
                MessageBox.Show("Mismatch of Weft Count with Cloth Master", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
                Exit Sub
            End If

        End If
        'WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)
        'If Val(WftCnt_ID) = 0 Then
        '    MessageBox.Show("Invalid Weft Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
        '    Exit Sub
        'End If

        WrpCnt_ID = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ")")
        WrpMil_ID = Common_Procedures.Mill_NameToIdNo(con, lbl_WarpMillName.Text)
        WftMil_ID = Common_Procedures.Mill_NameToIdNo(con, cbo_Weft_MillName.Text)

        'MasWftCnt_IDNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_IdNo = " & Str(Val(Clo_ID)) & ")")
        'If Val(WftCnt_ID) <> Val(MasWftCnt_IDNo) Then
        '    MessageBox.Show("Mismatch of Weft Count with Cloth Master", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
        '    Exit Sub
        'End If

        NoofInpBmsInLom = Common_Procedures.get_FieldValue(con, "Loom_Head", "Noof_Input_Beams", "(Loom_IdNo = " & Str(Val(Lm_ID)) & ")")
        If Val(NoofInpBmsInLom) = 0 Then NoofInpBmsInLom = 1

        If NoofInpBmsInLom = 1 Then
            If Trim(lbl_BeamNo1.Text) = "" And Trim(lbl_BeamNo2.Text) = "" Then
                MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(lbl_BalMtrs1.Text) = 0 And Val(lbl_BalMtrs2.Text) = 0 Then
                MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

        Else
            If Trim(lbl_BeamNo1.Text) = "" Or Trim(lbl_BeamNo2.Text) = "" Then
                MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(lbl_BalMtrs1.Text) = 0 Or Val(lbl_BalMtrs2.Text) = 0 Then
                MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

        End If

        vDOFSHFTIDNO = 0
        If cbo_DoffShift.Visible And txt_Doff_Shift_Meters.Visible Then
            vDOFSHFTIDNO = Common_Procedures.Shift_NameToIdNo(con, cbo_DoffShift.Text)
            If Val(txt_Doff_Shift_Meters.Text) > 0 Then
                If vDOFSHFTIDNO = 0 Then
                    MessageBox.Show("Invalid Doff ShiftName", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_DoffShift.Visible And cbo_DoffShift.Enabled Then cbo_DoffShift.Focus()
                    Exit Sub
                End If
            End If
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
            If Val(txt_DoffMtrs.Text) = 0 Then
                MessageBox.Show("Invalid Doff Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_DoffMtrs.Enabled Then txt_DoffMtrs.Focus()
                Exit Sub
            End If
        End If

        If cbo_PanelQuality.Visible = True Then
            If Trim(cbo_PanelQuality.Text) = "" Then
                MessageBox.Show("Invalid Panel Name, Select Panel", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PanelQuality.Enabled Then cbo_PanelQuality.Focus()
                Exit Sub
            End If
        End If

        If cbo_ClothSales_OrderNo.Visible = True Then
            If Trim(cbo_ClothSales_OrderNo.Text) = "" Then
                MessageBox.Show("Invalid Sales Order Indent No.", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothSales_OrderNo.Enabled Then cbo_ClothSales_OrderNo.Focus()
                Exit Sub
            End If
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "--1544--" Then '---- SRI SRINIVASA TEXTILES (PALLADAM)

            If lbl_PoNo.Visible = True And Trim(lbl_PoNo.Text) = "" Then
                MessageBox.Show("Invalid PO No.", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If lbl_WarpLotNo.Visible = True And Trim(lbl_WarpLotNo.Text) = "" Then
                MessageBox.Show("Invalid Warp LotNo.", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If cbo_WeftLotNo.Visible = True And Trim(cbo_WeftLotNo.Text) = "" Then
                MessageBox.Show("Invalid Weft LotNo.", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_WeftLotNo.Enabled Then cbo_WeftLotNo.Focus()
                Exit Sub
            End If

        End If

        Call ConsumedPavu_Calculation()
        Call ConsumedYarn_Calculation()
        Call Generate_Barcode()

        get_Fabric_LotNo()
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)

            If SaveAll_Sts <> True Then

                If Trim(cbo_Weft_MillName.Text) = "" Then
                    MessageBox.Show("Invalid Weft Mill Name,Cann't get Weft LotNo", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If Trim(cbo_WeftLotNo.Text) = "" Then
                    MessageBox.Show("Invalid Weft LotNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_WeftLotNo.Enabled And cbo_WeftLotNo.Visible Then cbo_WeftLotNo.Focus()
                    Exit Sub
                End If
                If Trim(lbl_WarpLotNo.Text) = "" Then
                    MessageBox.Show("Invalid Warp LotNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If lbl_WarpLotNo.Enabled And lbl_WarpLotNo.Visible Then lbl_WarpLotNo.Focus()
                    Exit Sub
                End If
            End If


            'If Trim(lbl_FabricLotNo.Text) = "" Then
            '    MessageBox.Show("Invalid Fabric LotNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            '    If cbo_Weft_MillName.Enabled And cbo_Weft_MillName.Visible Then cbo_Weft_MillName.Focus()
            '    Exit Sub
            'End If
        End If

        PcsChkCode = ""
        Old_Loom_Idno = 0
        Old_SetCd1 = ""
        Old_Beam1 = ""
        Old_SetCd2 = ""
        Old_Beam2 = ""
        Old_BMKNOTCd = ""
        Old_CLTH_Idno = 0

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RollNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", "for_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            OrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text))
            vSELC_LOTCODE = Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag)) & "/" & Trim(Pk_Condition)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Cloth_Receipt_Head ( Receipt_Type, Weaver_ClothReceipt_Code,           Company_IdNo      ,        Weaver_ClothReceipt_RefNo   , Weaver_ClothReceipt_SuffixNo,      Weaver_ClothReceipt_No  ,     for_OrderBy     , Weaver_ClothReceipt_Date,    StockOff_IdNo         ,    Ledger_IdNo     ,       Loom_IdNo   ,             Width_Type            ,           Beam_Knotting_Code     ,       Beam_Knotting_No         ,     Cloth_Idno     ,       EndsCount_Idno  ,     Count_IdNo        ,              Beam_No1           ,              Set_Code1           ,              Set_No1           ,          Balance_Meters1      ,               Beam_No2          ,               Set_Code2          ,             Set_No2            ,            Balance_Meters2    , Folding_Receipt, Folding, Total_Receipt_Pcs, noof_pcs,       ReceiptMeters_Receipt   ,           Receipt_Meters      ,       Total_Receipt_Meters    ,       ConsumedYarn_Receipt        ,              Consumed_Yarn        ,         ConsumedPavu_Receipt       ,              Consumed_Pavu         ,     BeamConsumption_Receipt            ,         BeamConsumption_Meters         ,             Crimp_Percentage        , Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment   ,            user_idNo                     ,              Bar_Code          ,       lotcode_forSelection    ,                 Is_LastPiece                        ,           Doff_Shift_IdNo     ,             Doff_Shift_Meters               ,         Warp_Count_IdNo    ,            Warp_Mill_IdNo  ,           Warp_LotNo              ,             Weft_Mill_IdNo  ,                Weft_LotNo          ,            Fabric_LotNo               ,      ClothSales_OrderCode_forSelection      ,             Panel_Quality             ,              Po_No            ) " &
                                  "            Values                    (      'L'    ,  '" & Trim(NewCode) & "', " & Val(lbl_Company.Tag) & ", '" & Trim(lbl_RollNo.Text) & "',           ''                ,  '" & Trim(lbl_RollNo.Text) & "', " & Val(OrdByNo) & ",         @EntryDate      , " & Val(vStkOff_IDno) & ", " & Val(led_id) & ", " & Val(Lm_ID) & ", '" & Trim(lbl_WidthType.Text) & "', '" & Trim(lbl_KnotCode.Text) & "', '" & Trim(lbl_KnotNo.Text) & "', " & Val(Clo_ID) & ", " & Val(EdsCnt_ID) & ", " & Val(WftCnt_ID) & ", '" & Trim(lbl_BeamNo1.Text) & "', '" & Trim(lbl_SetCode1.Text) & "', '" & Trim(lbl_SetNo1.Text) & "', " & Val(lbl_BalMtrs1.Text) & ", '" & Trim(lbl_BeamNo2.Text) & "', '" & Trim(lbl_SetCode2.Text) & "', '" & Trim(lbl_SetNo2.Text) & "', " & Val(lbl_BalMtrs2.Text) & ",      100       ,   100  ,       1          ,    1    , " & Val(txt_DoffMtrs.Text) & ", " & Val(txt_DoffMtrs.Text) & ", " & Val(txt_DoffMtrs.Text) & ", " & Val(lbl_ConsWeftYarn.Text) & ", " & Val(lbl_ConsWeftYarn.Text) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(lbl_ConsPavu.Text)) & ", " & Str(Val(lbl_BeamConsPavu.Text)) & ", " & Str(Val(lbl_BeamConsPavu.Text)) & ", " & Str(Val(txt_CrimpPerc.Text)) & ",               ''          ,             0                     ," & Val(Common_Procedures.User.IdNo) & "  ,'" & Trim(txt_BarCode.Text) & "', '" & Trim(vSELC_LOTCODE) & "' , '" & Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) & "', " & Str(Val(vDOFSHFTIDNO)) & ", " & Str(Val(txt_Doff_Shift_Meters.Text)) & ", " & Str(Val(WrpCnt_ID)) & ", " & Str(Val(WrpMil_ID)) & ", '" & Trim(lbl_WarpLotNo.Text) & "', " & Str(Val(WftMil_ID)) & " ,  '" & Trim(cbo_WeftLotNo.Text) & "',  '" & Trim(lbl_FabricLotNo.Text) & "' ,  '" & Trim(cbo_ClothSales_OrderNo.Text) & "' ,'" & Trim(cbo_PanelQuality.Text) & "' ,  '" & Trim(lbl_PoNo.Text) & "')"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Cloth_Receipt_head", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RollNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_ClothReceipt_Code, Company_IdNo, for_OrderBy", tr)

                da = New SqlClient.SqlDataAdapter("Select * from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
                da.SelectCommand.Transaction = tr
                dt1 = New DataTable
                da.Fill(dt1)
                If dt1.Rows.Count > 0 Then

                    If IsDBNull(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                        If Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" And SaveAll_Sts = False Then
                            PcsChkCode = Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString)
                            'Throw New ApplicationException("Already Piece Checking Prepared")
                            'Exit Sub
                        End If
                    End If

                    Old_Loom_Idno = Val(dt1.Rows(0).Item("Loom_IdNo").ToString)
                    Old_SetCd1 = dt1.Rows(0).Item("set_code1").ToString
                    Old_Beam1 = dt1.Rows(0).Item("beam_no1").ToString
                    Old_SetCd2 = dt1.Rows(0).Item("set_code2").ToString
                    Old_Beam2 = dt1.Rows(0).Item("beam_no2").ToString
                    Old_BMKNOTCd = dt1.Rows(0).Item("Beam_Knotting_Code").ToString
                    Old_CLTH_Idno = Val(dt1.Rows(0).Item("Cloth_IdNo").ToString)

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

                End If
                dt1.Clear()

                'cmd.CommandText = "Update Beam_Knotting_Head set Production_Meters = a.Production_Meters - b.ReceiptMeters_Receipt from Beam_Knotting_Head a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Beam_Knotting_Code = b.Beam_Knotting_Code"
                'cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters -  (CASE WHEN b.Weaver_Piece_Checking_Code <> '' THEN b.BeamConsumption_Checking ELSE b.BeamConsumption_Receipt END)  from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and b.Set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
                'cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters -  (CASE WHEN b.Weaver_Piece_Checking_Code <> '' THEN b.BeamConsumption_Checking ELSE b.BeamConsumption_Receipt END)  from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and b.Set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
                'cmd.ExecuteNonQuery()


                '------ HEAD Updation

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Receipt_Type = 'L', Weaver_ClothReceipt_Date = @EntryDate, StockOff_IdNo = " & Val(vStkOff_IDno) & ", Ledger_IdNo = " & Str(Val(led_id)) & ", Loom_IdNo = " & Str(Val(Lm_ID)) & ", Width_Type = '" & Trim(lbl_WidthType.Text) & "', Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "', Beam_Knotting_No = '" & Trim(lbl_KnotNo.Text) & "', Cloth_Idno = " & Str(Val(Clo_ID)) & ", EndsCount_IdNo = " & Val(EdsCnt_ID) & ", Count_IdNo = " & Val(WftCnt_ID) & ", set_Code1 = '" & Trim(lbl_SetCode1.Text) & "', set_No1 = '" & Trim(lbl_SetNo1.Text) & "', Beam_No1 = '" & Trim(lbl_BeamNo1.Text) & "', Balance_Meters1 = " & Str(Val(lbl_BalMtrs1.Text)) & ", set_Code2 = '" & Trim(lbl_SetCode2.Text) & "', set_no2 = '" & Trim(lbl_SetNo2.Text) & "', Beam_No2 = '" & Trim(lbl_BeamNo2.Text) & "', Balance_Meters2 = " & Str(Val(lbl_BalMtrs2.Text)) & ", ReceiptMeters_Receipt = " & Str(Val(txt_DoffMtrs.Text)) & ", Total_Receipt_Meters = " & Str(Val(txt_DoffMtrs.Text)) & ", ConsumedYarn_Receipt = " & Str(Val(lbl_ConsWeftYarn.Text)) & ", ConsumedPavu_Receipt = " & Str(Val(lbl_ConsPavu.Text)) & ", BeamConsumption_Receipt = " & Str(Val(lbl_BeamConsPavu.Text)) & ", Crimp_Percentage = " & Str(Val(txt_CrimpPerc.Text)) & " , Bar_Code = '" & Trim(txt_BarCode.Text) & "',lotcode_forSelection='" & Trim(vSELC_LOTCODE) & "' , Is_LastPiece = '" & Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) & "', Doff_Shift_IdNo = " & Str(Val(vDOFSHFTIDNO)) & ", Doff_Shift_Meters = " & Str(Val(txt_Doff_Shift_Meters.Text)) & " , Warp_Count_IdNo = " & Str(Val(WrpCnt_ID)) & ", Warp_Mill_IdNo = " & Str(Val(WrpMil_ID)) & ", Warp_LotNo = '" & Trim(lbl_WarpLotNo.Text) & "', Weft_Mill_IdNo = " & Str(Val(WftMil_ID)) & " , Weft_LotNo = '" & Trim(cbo_WeftLotNo.Text) & "', Fabric_LotNo = '" & Trim(lbl_FabricLotNo.Text) & "', ClothSales_OrderCode_forSelection  = '" & Trim(cbo_ClothSales_OrderNo.Text) & "' , Panel_Quality ='" & Trim(cbo_PanelQuality.Text) & "', Po_No = '" & Trim(lbl_PoNo.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Receipt_Meters = " & Str(Val(txt_DoffMtrs.Text)) & ", Consumed_Yarn = " & Str(Val(lbl_ConsWeftYarn.Text)) & ", Consumed_Pavu = " & Str(Val(lbl_ConsPavu.Text)) & ", BeamConsumption_Meters = " & Str(Val(lbl_BeamConsPavu.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = '' and Weaver_Wages_Code = ''"
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Weaver_Cloth_Receipt_head", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RollNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_ClothReceipt_Code, Company_IdNo, for_OrderBy", tr)

            EntID = Trim(Pk_Condition) & Trim(lbl_RollNo.Text)
            Partcls = "Doff : Roll.No. " & Trim(lbl_RollNo.Text)
            PBlNo = Trim(lbl_RollNo.Text)

            vBEAMKnot_ProdMeters = Common_Procedures.get_BeamKnotting_TotalProductionMeters_from_Doffing(con, lbl_KnotCode.Text, tr)
            nr = 0
            SQL1 = "Update Beam_Knotting_Head set Production_Meters = " & Str(Val(vBEAMKnot_ProdMeters)) & " where  Loom_IdNo = " & Str(Val(Lm_ID)) & " and Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "' and Ledger_IdNo = " & Str(Val(led_id))
            cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            nr = cmd.ExecuteNonQuery()
            'cmd.CommandText = "Update Beam_Knotting_Head set Production_Meters = Production_Meters + " & Str(Val(txt_DoffMtrs.Text)) & " where Loom_IdNo = " & Str(Val(Lm_ID)) & " and Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "' and Ledger_IdNo = " & Str(Val(led_id))
            'nr = cmd.ExecuteNonQuery
            If nr = 0 Then
                Throw New ApplicationException("Mismatch of Loom Knotting && Party")
                Exit Sub
            End If

            YrnPartcls = Partcls & ", Cloth : " & Trim(cbo_ClothName.Text) & ", Meters :" & Str(Val(txt_DoffMtrs.Text))




            vSTKPOSTING_STS = False
            If Trim(PcsChkCode) = "" Then

                cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


                cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


                Delv_ID = 0 : Rec_ID = 0
                If Trim(UCase(vLedtype)) = "JOBWORKER" Then
                    Delv_ID = led_id
                    Rec_ID = 0
                Else
                    Delv_ID = 0
                    Rec_ID = led_id
                End If

                'If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
                '    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = Production_Meters + " & Str(Val(lbl_BeamConsPavu.Text)) & " where set_code = '" & Trim(lbl_SetCode1.Text) & "' and beam_no = '" & Trim(lbl_BeamNo1.Text) & "'"
                '    cmd.ExecuteNonQuery()
                'End If

                'If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                '    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = Production_Meters + " & Str(Val(lbl_BeamConsPavu.Text)) & " where set_code = '" & Trim(lbl_SetCode2.Text) & "' and beam_no = '" & Trim(lbl_BeamNo2.Text) & "'"
                '    cmd.ExecuteNonQuery()
                'End If

                If Trim(UCase(vLedtype)) <> "JOBWORKER" Or (Common_Procedures.settings.JobWorker_Pavu_Yarn_Stock_Posting_IN_Production = 1 And Trim(UCase(vLedtype)) = "JOBWORKER") Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1192" Then

                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters , ClothSales_OrderCode_forSelection ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(OrdByNo)) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(EdsCnt_ID)) & ", 0, " & Str(Val(lbl_ConsPavu.Text)) & " , '" & Trim(cbo_ClothSales_OrderNo.Text) & "' )"
                    cmd.ExecuteNonQuery()

                    vSTKPOSTING_STS = True
                    If btn_Show_WeftConsumption_Details.Visible = False Then
                        If Val(WftCnt_ID) <> 0 And Val(lbl_ConsWeftYarn.Text) <> 0 Then
                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight , ClothSales_OrderCode_forSelection ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(OrdByNo)) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "', 1, " & Str(Val(WftCnt_ID)) & ", 'MILL', 0, 0, 0, " & Str(Val(lbl_ConsWeftYarn.Text)) & ", '" & Trim(cbo_ClothSales_OrderNo.Text) & "' )"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                End If


                '----Multi WeftCount Yarn consumption posting

                cmd.CommandText = "delete from Weaver_ClothReceipt_Consumed_Yarn_Details where Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If btn_Show_WeftConsumption_Details.Visible = True Then

                    With dgv_Weft_Consumption_Details

                        sNO = 0
                        For i = 0 To .RowCount - 1

                            If Trim(.Rows(i).Cells(0).Value) <> "" Then

                                vWFTCNTIDno = Common_Procedures.Count_NameToIdNo(con, Trim(.Rows(i).Cells(0).Value), tr)

                                If Val(vWFTCNTIDno) <> 0 Then

                                    sNO = sNO + 1

                                    cmd.CommandText = "Insert into Weaver_ClothReceipt_Consumed_Yarn_Details (               Weaver_ClothReceipt_Code       ,           Company_IdNo           ,           Sl_No      ,             Count_IdNo       ,                    Gram_Perc_Type       ,                    Consumption_Gram_Perc  ,                Consumed_Yarn_Weight        )  " &
                                                            " Values                                         (  '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", " & Str(Val(sNO)) & ", " & Str(Val(vWFTCNTIDno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "' , " & Str(Val(.Rows(i).Cells(2).Value)) & " ,  " & Str(Val(.Rows(i).Cells(3).Value)) & " ) "
                                    cmd.ExecuteNonQuery()

                                    If vSTKPOSTING_STS = True And Val(.Rows(i).Cells(3).Value) <> 0 Then

                                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (              Reference_Code                ,                Company_IdNo      ,                Reference_No        ,        for_OrderBy       , Reference_Date,      DeliveryTo_Idno     ,       ReceivedFrom_Idno ,         Entry_ID     ,         Particulars       ,      Party_Bill_No   ,              Sl_No           ,           Count_IdNo         , Yarn_Type, Mill_IdNo, Bags, Cones,                 Weight                 ,         ClothSales_OrderCode_forSelection   ) " &
                                                                "           Values                   ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(OrdByNo)) & ",   @EntryDate  , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "',  " & Str(Val(1000 + sNO)) & " , " & Str(Val(vWFTCNTIDno)) & ",   'MILL' ,     0    ,   0 ,    0 , " & Str(Val(.Rows(i).Cells(3).Value)) & " , '" & Trim(cbo_ClothSales_OrderNo.Text) & "' ) "
                                        cmd.ExecuteNonQuery()

                                    End If

                                End If

                            End If

                        Next

                    End With

                End If



                If Val(txt_DoffMtrs.Text) <> 0 Then

                    Delv_ID = 0 : Rec_ID = 0
                    If Val(led_id) = Val(Common_Procedures.CommonLedger.Godown_Ac) Then
                        Delv_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                        Rec_ID = 0

                    Else
                        Delv_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                        Rec_ID = Val(led_id)

                    End If

                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (   Reference_Code     ,             Company_IdNo         ,             Reference_No       ,     for_OrderBy          , Reference_Date,         StockOff_IdNo         ,  DeliveryTo_Idno         ,       ReceivedFrom_Idno ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     , Folding,             UnChecked_Meters       ,  Meters_Type1, Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ,                    ClothSales_OrderCode_forSelection ) " &
                                                "    Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(OrdByNo)) & ",    @EntryDate , " & Str(Val(vStkOff_IDno)) & ", " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ",   100  , " & Str(Val(txt_DoffMtrs.Text)) & ",       0      ,       0     ,       0     ,       0     ,       0      , '" & Trim(cbo_ClothSales_OrderNo.Text) & "'  ) "
                    cmd.ExecuteNonQuery()

                End If

            Else

                'If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
                '    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters +  (CASE WHEN b.Weaver_Piece_Checking_Code <> '' THEN b.BeamConsumption_Checking ELSE b.BeamConsumption_Receipt END)  from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and b.Set_code1 <> '' and b.Beam_No1 <> '' and b.Set_code1 = '" & Trim(lbl_SetCode1.Text) & "' and b.Beam_No1 = '" & Trim(lbl_BeamNo1.Text) & "' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
                '    cmd.ExecuteNonQuery()
                'End If

                'If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                '    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters +  (CASE WHEN b.Weaver_Piece_Checking_Code <> '' THEN b.BeamConsumption_Checking ELSE b.BeamConsumption_Receipt END)  from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and b.Set_code2 <> '' and b.Beam_No2 <> '' and b.Set_code2 = '" & Trim(lbl_SetCode2.Text) & "' and b.Beam_No2 = '" & Trim(lbl_BeamNo2.Text) & "' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
                '    cmd.ExecuteNonQuery()
                'End If


            End If


            cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()


            'If New_Entry = True Then

            vBEAM_ProdMeters = 0
            vErrMsg = ""

            If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Clo_ID, lbl_SetCode1.Text, lbl_BeamNo1.Text, vBEAM_ProdMeters, vErrMsg, tr) = True Then
                Throw New ApplicationException(vErrMsg)
                Exit Sub
            Else
                If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = " & Str(Val(vBEAM_ProdMeters)) & " where set_code = '" & Trim(lbl_SetCode1.Text) & "' and beam_no = '" & Trim(lbl_BeamNo1.Text) & "'"
                    cmd.ExecuteNonQuery()
                End If
            End If

            vBEAM_ProdMeters = 0
            vErrMsg = ""

            If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Clo_ID, lbl_SetCode2.Text, lbl_BeamNo2.Text, vBEAM_ProdMeters, vErrMsg, tr) = True Then
                Throw New ApplicationException(vErrMsg)
                Exit Sub

            Else
                If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = " & Str(Val(vBEAM_ProdMeters)) & " where set_code = '" & Trim(lbl_SetCode2.Text) & "' and beam_no = '" & Trim(lbl_BeamNo2.Text) & "'"
                    cmd.ExecuteNonQuery()
                End If

            End If

            'End If


            If Trim(Old_BMKNOTCd) <> "" Then

                If Trim(UCase(lbl_KnotCode.Text)) <> Trim(UCase(Old_BMKNOTCd)) Then

                    vBEAMKnot_ProdMeters = Common_Procedures.get_BeamKnotting_TotalProductionMeters_from_Doffing(con, Trim(Old_BMKNOTCd), tr)

                    SQL1 = "Update Beam_Knotting_Head set Production_Meters = " & Str(Val(vBEAMKnot_ProdMeters)) & " where Beam_Knotting_Code = '" & Trim(Old_BMKNOTCd) & "'"
                    cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                    cmd.ExecuteNonQuery()

                End If

            End If


            If Trim(Old_SetCd1) <> "" And Trim(Old_Beam1) <> "" Then

                If Not (Trim(UCase(Old_SetCd1)) = Trim(UCase(lbl_SetCode1.Text)) And Trim(UCase(Old_Beam1)) = Trim(UCase(lbl_BeamNo1.Text))) Then

                    vBEAM_ProdMeters = 0
                    vErrMsg = ""
                    '----- Checking for negative beam meters
                    If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Old_CLTH_Idno, Old_SetCd1, Old_Beam1, vBEAM_ProdMeters, vErrMsg, tr) = True Then
                        Throw New ApplicationException(vErrMsg)
                        Exit Sub

                    Else

                        SQL1 = "Update Stock_SizedPavu_Processing_Details set Production_Meters = " & Str(Val(vBEAM_ProdMeters)) & " Where set_code = '" & Trim(Old_SetCd1) & "' and beam_no = '" & Trim(Old_Beam1) & "'"
                        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                        cmd.ExecuteNonQuery()

                    End If

                End If

            End If

            If Trim(Old_SetCd2) <> "" And Trim(Old_Beam2) <> "" Then

                If Not (Trim(UCase(Old_SetCd2)) = Trim(UCase(lbl_SetCode2.Text)) And Trim(UCase(Old_Beam2)) = Trim(UCase(lbl_BeamNo2.Text))) Then

                    vBEAM_ProdMeters = 0
                    vErrMsg = ""
                    '----- Checking for negative beam meters
                    If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Old_CLTH_Idno, Old_SetCd2, Old_Beam2, vBEAM_ProdMeters, vErrMsg, tr) = True Then
                        Throw New ApplicationException(vErrMsg)
                        Exit Sub

                    Else
                        SQL1 = "Update Stock_SizedPavu_Processing_Details set Production_Meters = " & Str(Val(vBEAM_ProdMeters)) & " Where set_code = '" & Trim(Old_SetCd2) & "' and beam_no = '" & Trim(Old_Beam2) & "'"
                        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                        cmd.ExecuteNonQuery()

                    End If

                End If

            End If


            'cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            'cmd.ExecuteNonQuery()

            'If New_Entry = False Then
            '    '----- Editing
            '    If Trim(Old_SetCd1) <> "" And Trim(Old_Beam1) <> "" Then
            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(Old_SetCd1) & "' and Beam_No = '" & Trim(Old_Beam1) & "'"
            '        cmd.ExecuteNonQuery()
            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select '" & Trim(Old_SetCd1) & "', '" & Trim(Old_Beam1) & "', -1*BeamConsumption_Meters from Weaver_Cloth_Receipt_Head where (Set_Code1 = '" & Trim(Old_SetCd1) & "' and Beam_No1 = '" & Trim(Old_Beam1) & "') OR (Set_Code2 = '" & Trim(Old_SetCd1) & "' and Beam_No2 = '" & Trim(Old_Beam1) & "')"
            '        cmd.ExecuteNonQuery()
            '    End If
            '    If Trim(Old_SetCd2) <> "" And Trim(Old_Beam2) <> "" Then
            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(Old_SetCd2) & "' and Beam_No = '" & Trim(Old_Beam2) & "'"
            '        cmd.ExecuteNonQuery()

            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select '" & Trim(Old_SetCd2) & "', '" & Trim(Old_Beam2) & "', -1*BeamConsumption_Meters from Weaver_Cloth_Receipt_Head where (Set_Code1 = '" & Trim(Old_SetCd2) & "' and Beam_No1 = '" & Trim(Old_Beam2) & "') OR (Set_Code2 = '" & Trim(Old_SetCd2) & "' and Beam_No2 = '" & Trim(Old_Beam2) & "')"
            '        cmd.ExecuteNonQuery()

            '    End If
            'End If

            '''''----- Saving

            'If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
            '    cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'"
            '    cmd.ExecuteNonQuery()

            '    cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select '" & Trim(lbl_SetCode1.Text) & "', '" & Trim(lbl_BeamNo1.Text) & "', -1*BeamConsumption_Meters from Weaver_Cloth_Receipt_Head where (Set_Code1 = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No1 = '" & Trim(lbl_BeamNo1.Text) & "') OR (Set_Code2 = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No2 = '" & Trim(lbl_BeamNo1.Text) & "')"
            '    cmd.ExecuteNonQuery()
            'End If
            'If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
            '    cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'"
            '    cmd.ExecuteNonQuery()
            '    cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select '" & Trim(lbl_SetCode2.Text) & "', '" & Trim(lbl_BeamNo2.Text) & "', -1*BeamConsumption_Meters from Weaver_Cloth_Receipt_Head where (Set_Code1 = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No1 = '" & Trim(lbl_BeamNo2.Text) & "') OR (Set_Code2 = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No2 = '" & Trim(lbl_BeamNo2.Text) & "')"
            '    cmd.ExecuteNonQuery()
            'End If

            'da = New SqlClient.SqlDataAdapter("select Name1, Name2, sum(Meters1) as ProdMtrs from " & Trim(Common_Procedures.EntryTempTable) & " Group by Name1, Name2 having sum(Meters1) <> 0 Order by Name1, Name2", con)
            'da.SelectCommand.Transaction = tr
            'dt2 = New DataTable
            'da.Fill(dt2)
            'If dt2.Rows.Count > 0 Then
            '    If IsDBNull(dt2.Rows(0).Item("ProdMtrs").ToString) = False Then
            '        If Val(dt2.Rows(0).Item("ProdMtrs").ToString) <> 0 Then
            '            Throw (New ApplicationException("Invalid Editing : Mismatch of Production Meters"))
            '            Exit Sub
            '        End If
            '    End If
            'End If
            'dt2.Clear()


            ''----- Saving
            'cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select Production_Meters from Beam_Knotting_Head where Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select -1*ReceiptMeters_Receipt from Weaver_Cloth_Receipt_Head where  Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'"
            'cmd.ExecuteNonQuery()

            'da = New SqlClient.SqlDataAdapter("select sum(Meters1) as ProdMtrs from " & Trim(Common_Procedures.EntryTempTable) & " having sum(Meters1) <> 0", con)
            'da.SelectCommand.Transaction = tr
            'dt2 = New DataTable
            'da.Fill(dt2)
            'If dt2.Rows.Count > 0 Then
            '    If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
            '        If Val(dt2.Rows(0)(0).ToString) <> 0 Then
            '            Throw New ApplicationException("Invalid Editing : Mismatch of Production Meters")
            '            Exit Sub
            '        End If
            '    End If
            'End If
            'dt2.Clear()


            tr.Commit()

            If SaveAll_Sts <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If
            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RollNo.Text)
                End If

            Else
                move_record(lbl_RollNo.Text)

            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub




    Private Sub cbo_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.GotFocus
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim Lm_ID As Integer = 0
        Dim Clo_ID1 As Integer = 0
        Dim Clo_ID2 As Integer = 0
        Dim Clo_ID3 As Integer = 0
        Dim Clo_ID4 As Integer = 0
        Dim Clo_Wrp_ID1 As Integer = 0, Clo_Wrp_ID2 As Integer = 0, Clo_Wrp_ID3 As Integer = 0, Clo_Wrp_ID4 As Integer = 0
        Dim Clo_Wft_ID1 As Integer = 0, Clo_Wft_ID2 As Integer = 0, Clo_Wft_ID3 As Integer = 0, Clo_Wft_ID4 As Integer = 0
        Dim Clo_Reed1 As Integer = 0, Clo_Reed2 As Integer = 0, Clo_Reed3 As Integer = 0, Clo_Reed4 As Integer = 0
        Dim Clo_Width1 As Integer = 0, Clo_Width2 As Integer = 0, Clo_Width3 As Integer = 0, Clo_Width4 As Integer = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Textiles (Somanur)

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        Else

            Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

            Clo_ID1 = 0 : Clo_ID2 = 0 : Clo_ID3 = 0 : Clo_ID4 = 0
            Clo_Wrp_ID1 = 0 : Clo_Wrp_ID2 = 0 : Clo_Wrp_ID3 = 0 : Clo_Wrp_ID4 = 0
            Clo_Wft_ID1 = 0 : Clo_Wft_ID2 = 0 : Clo_Wft_ID3 = 0 : Clo_Wft_ID4 = 0
            Clo_Reed1 = 0 : Clo_Reed2 = 0 : Clo_Reed3 = 0 : Clo_Reed4 = 0
            Clo_Width1 = 0 : Clo_Width2 = 0 : Clo_Width3 = 0 : Clo_Width4 = 0

            Da4 = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'", con)
            Dt4 = New DataTable
            Da4.Fill(Dt4)
            If Dt4.Rows.Count > 0 Then
                Clo_ID1 = Val(Dt4.Rows(0).Item("Cloth_Idno1").ToString)
                Clo_ID2 = Val(Dt4.Rows(0).Item("Cloth_Idno2").ToString)
                Clo_ID3 = Val(Dt4.Rows(0).Item("Cloth_Idno3").ToString)
                Clo_ID4 = Val(Dt4.Rows(0).Item("Cloth_Idno4").ToString)
            End If
            Dt4.Clear()
            Dt4.Dispose()
            Da4.Dispose()

            Clo_Wrp_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Wrp_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Wrp_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Wrp_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Wft_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Wft_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Wft_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Wft_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Reed1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Reed2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Reed3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Reed4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Width1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Width2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Width3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Width4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & "  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID1)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID1)) & " and Cloth_Reed = " & Str(Val(Clo_Reed1)) & " and Cloth_Width = " & Str(Val(Clo_Width1)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID2)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID2)) & " and Cloth_Reed = " & Str(Val(Clo_Reed2)) & " and Cloth_Width = " & Str(Val(Clo_Width2)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID3)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID3)) & " and Cloth_Reed = " & Str(Val(Clo_Reed3)) & " and Cloth_Width = " & Str(Val(Clo_Width3)) & " ) or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID4)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID4)) & " and Cloth_Reed = " & Str(Val(Clo_Reed4)) & " and Cloth_Width = " & Str(Val(Clo_Width4)) & " ) )", "(Cloth_idno = 0)")

            'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & ")", "(Cloth_idno = 0)")
            cbo_ClothName.Tag = cbo_ClothName.Text
        End If


    End Sub

    Private Sub cbo_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyDown
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim Lm_ID As Integer = 0
        Dim Clo_ID1 As Integer = 0
        Dim Clo_ID2 As Integer = 0
        Dim Clo_ID3 As Integer = 0
        Dim Clo_ID4 As Integer = 0
        Dim Clo_Wrp_ID1 As Integer = 0, Clo_Wrp_ID2 As Integer = 0, Clo_Wrp_ID3 As Integer = 0, Clo_Wrp_ID4 As Integer = 0
        Dim Clo_Wft_ID1 As Integer = 0, Clo_Wft_ID2 As Integer = 0, Clo_Wft_ID3 As Integer = 0, Clo_Wft_ID4 As Integer = 0
        Dim Clo_Reed1 As Integer = 0, Clo_Reed2 As Integer = 0, Clo_Reed3 As Integer = 0, Clo_Reed4 As Integer = 0
        Dim Clo_Width1 As Integer = 0, Clo_Width2 As Integer = 0, Clo_Width3 As Integer = 0, Clo_Width4 As Integer = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Textiles (Somanur)
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, cbo_LoomNo, txt_DoffMtrs, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        Else
            Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

            Clo_ID1 = 0 : Clo_ID2 = 0 : Clo_ID3 = 0 : Clo_ID4 = 0
            Clo_Wrp_ID1 = 0 : Clo_Wrp_ID2 = 0 : Clo_Wrp_ID3 = 0 : Clo_Wrp_ID4 = 0
            Clo_Wft_ID1 = 0 : Clo_Wft_ID2 = 0 : Clo_Wft_ID3 = 0 : Clo_Wft_ID4 = 0
            Clo_Reed1 = 0 : Clo_Reed2 = 0 : Clo_Reed3 = 0 : Clo_Reed4 = 0
            Clo_Width1 = 0 : Clo_Width2 = 0 : Clo_Width3 = 0 : Clo_Width4 = 0

            Da4 = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'", con)
            Dt4 = New DataTable
            Da4.Fill(Dt4)
            If Dt4.Rows.Count > 0 Then
                Clo_ID1 = Val(Dt4.Rows(0).Item("Cloth_Idno1").ToString)
                Clo_ID2 = Val(Dt4.Rows(0).Item("Cloth_Idno2").ToString)
                Clo_ID3 = Val(Dt4.Rows(0).Item("Cloth_Idno3").ToString)
                Clo_ID4 = Val(Dt4.Rows(0).Item("Cloth_Idno4").ToString)
            End If
            Dt4.Clear()
            Dt4.Dispose()
            Da4.Dispose()

            Clo_Wrp_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Wrp_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Wrp_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Wrp_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Wft_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Wft_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Wft_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Wft_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Reed1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Reed2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Reed3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Reed4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Width1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Width2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Width3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Width4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, Nothing, txt_DoffMtrs, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & "  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID1)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID1)) & " and Cloth_Reed = " & Str(Val(Clo_Reed1)) & " and Cloth_Width = " & Str(Val(Clo_Width1)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID2)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID2)) & " and Cloth_Reed = " & Str(Val(Clo_Reed2)) & " and Cloth_Width = " & Str(Val(Clo_Width2)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID3)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID3)) & " and Cloth_Reed = " & Str(Val(Clo_Reed3)) & " and Cloth_Width = " & Str(Val(Clo_Width3)) & " ) or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID4)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID4)) & " and Cloth_Reed = " & Str(Val(Clo_Reed4)) & " and Cloth_Width = " & Str(Val(Clo_Width4)) & " ) )", "(Cloth_idno = 0)")
            If e.KeyCode = 38 And cbo_ClothName.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then

                If cbo_PanelQuality.Visible = True And cbo_PanelQuality.Enabled Then
                    cbo_PanelQuality.Focus()
                ElseIf cbo_Pcs_LastPiece_Status.Visible = True And cbo_Pcs_LastPiece_Status.Enabled = True Then
                    cbo_Pcs_LastPiece_Status.Focus()
                ElseIf txt_BarCode.Visible = True And txt_BarCode.Enabled = True Then
                    txt_BarCode.Focus()
                ElseIf cbo_LoomNo.Visible = True And cbo_LoomNo.Enabled = True Then
                    cbo_LoomNo.Focus()
                Else
                    msk_Date.Focus()
                End If

            ElseIf e.KeyCode = 40 And cbo_ClothName.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then

                'If cbo_Weft_MillName.Visible Then
                '    cbo_Weft_MillName.Focus()
                'ElseIf cbo_DoffShift.Visible Then
                '    cbo_DoffShift.Focus()
                'Else
                '    txt_DoffMtrs.Focus()
                'End If

                If cbo_ClothSales_OrderNo.Visible And cbo_ClothSales_OrderNo.Enabled = True Then
                    cbo_ClothSales_OrderNo.Focus()
                ElseIf cbo_DoffShift.Visible And cbo_DoffShift.Enabled = True Then
                    cbo_DoffShift.Focus()
                ElseIf cbo_WeftLotNo.Visible And cbo_WeftLotNo.Enabled = True Then
                    cbo_WeftLotNo.Focus()
                Else
                    txt_DoffMtrs.Focus()
                End If

            End If

        End If

        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, cbo_LoomNo, txt_DoffMtrs, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & ")", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName.KeyPress
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim Lm_ID As Integer = 0
        Dim Clo_ID1 As Integer = 0
        Dim Clo_ID2 As Integer = 0
        Dim Clo_ID3 As Integer = 0
        Dim Clo_ID4 As Integer = 0
        Dim Clo_Wrp_ID1 As Integer = 0, Clo_Wrp_ID2 As Integer = 0, Clo_Wrp_ID3 As Integer = 0, Clo_Wrp_ID4 As Integer = 0
        Dim Clo_Wft_ID1 As Integer = 0, Clo_Wft_ID2 As Integer = 0, Clo_Wft_ID3 As Integer = 0, Clo_Wft_ID4 As Integer = 0
        Dim Clo_Reed1 As Integer = 0, Clo_Reed2 As Integer = 0, Clo_Reed3 As Integer = 0, Clo_Reed4 As Integer = 0
        Dim Clo_Width1 As Integer = 0, Clo_Width2 As Integer = 0, Clo_Width3 As Integer = 0, Clo_Width4 As Integer = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Textiles (Somanur)
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, txt_DoffMtrs, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        Else

            Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

            Clo_ID1 = 0 : Clo_ID2 = 0 : Clo_ID3 = 0 : Clo_ID4 = 0
            Clo_Wrp_ID1 = 0 : Clo_Wrp_ID2 = 0 : Clo_Wrp_ID3 = 0 : Clo_Wrp_ID4 = 0
            Clo_Wft_ID1 = 0 : Clo_Wft_ID2 = 0 : Clo_Wft_ID3 = 0 : Clo_Wft_ID4 = 0
            Clo_Reed1 = 0 : Clo_Reed2 = 0 : Clo_Reed3 = 0 : Clo_Reed4 = 0
            Clo_Width1 = 0 : Clo_Width2 = 0 : Clo_Width3 = 0 : Clo_Width4 = 0

            Da4 = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'", con)
            Dt4 = New DataTable
            Da4.Fill(Dt4)
            If Dt4.Rows.Count > 0 Then
                Clo_ID1 = Val(Dt4.Rows(0).Item("Cloth_Idno1").ToString)
                Clo_ID2 = Val(Dt4.Rows(0).Item("Cloth_Idno2").ToString)
                Clo_ID3 = Val(Dt4.Rows(0).Item("Cloth_Idno3").ToString)
                Clo_ID4 = Val(Dt4.Rows(0).Item("Cloth_Idno4").ToString)
            End If
            Dt4.Clear()
            Dt4.Dispose()
            Da4.Dispose()

            Clo_Wrp_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Wrp_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Wrp_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Wrp_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Wft_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Wft_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Wft_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Wft_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Reed1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Reed2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Reed3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Reed4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            Clo_Width1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
            Clo_Width2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
            Clo_Width3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
            Clo_Width4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

            If Asc(e.KeyChar) = 13 Then
                cbo_ClothName.Tag = "----------------------"
            End If

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & "  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID1)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID1)) & " and Cloth_Reed = " & Str(Val(Clo_Reed1)) & " and Cloth_Width = " & Str(Val(Clo_Width1)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID2)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID2)) & " and Cloth_Reed = " & Str(Val(Clo_Reed2)) & " and Cloth_Width = " & Str(Val(Clo_Width2)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID3)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID3)) & " and Cloth_Reed = " & Str(Val(Clo_Reed3)) & " and Cloth_Width = " & Str(Val(Clo_Width3)) & " )   or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID4)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID4)) & " and Cloth_Reed = " & Str(Val(Clo_Reed4)) & " and Cloth_Width = " & Str(Val(Clo_Width4)) & " ) )", "(Cloth_idno = 0)")


            If Asc(e.KeyChar) = 13 Then
                'If cbo_Weft_MillName.Visible Then
                '    cbo_Weft_MillName.Focus()
                'ElseIf cbo_DoffShift.Visible Then
                '    cbo_DoffShift.Focus()
                'Else
                '    txt_DoffMtrs.Focus()
                'End If

                If cbo_ClothSales_OrderNo.Visible And cbo_ClothSales_OrderNo.Enabled = True Then
                    cbo_ClothSales_OrderNo.Focus()
                ElseIf cbo_WeftLotNo.Visible And cbo_WeftLotNo.Enabled = True Then
                    cbo_WeftLotNo.Focus()
                ElseIf cbo_DoffShift.Visible And cbo_DoffShift.Enabled = True Then
                    cbo_DoffShift.Focus()
                Else
                    txt_DoffMtrs.Focus()
                End If
            End If

        End If

        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, txt_DoffMtrs, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & ")", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_ClothName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.LostFocus
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim Clo_IdNo As Integer = 0
        Dim wftcnt_idno As Integer = 0
        Dim vMULTIWFT_STS As Integer = 0

        If Trim(UCase(cbo_ClothName.Tag)) <> Trim(UCase(cbo_ClothName.Text)) Then

            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

            wftcnt_idno = 0
            vMULTIWFT_STS = 0
            Da4 = New SqlClient.SqlDataAdapter("Select Cloth_WeftCount_IdNo, Multiple_WeftCount_Status from Cloth_Head Where Cloth_Idno = " & Str(Val(Clo_IdNo)), con)
            Dt4 = New DataTable
            Da4.Fill(Dt4)
            If Dt4.Rows.Count > 0 Then
                vMULTIWFT_STS = Val(Dt4.Rows(0).Item("Multiple_WeftCount_Status").ToString)
                wftcnt_idno = Val(Dt4.Rows(0).Item("Cloth_WeftCount_IdNo").ToString)
            End If
            Dt4.Clear()

            If vMULTIWFT_STS = 0 Then
                'wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
                lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, wftcnt_idno)
                btn_Show_WeftConsumption_Details.Visible = False

            Else
                lbl_WeftCount.Text = ""
                btn_Show_WeftConsumption_Details.Visible = True
                btn_Show_WeftConsumption_Details.BringToFront()

            End If

            ConsumedYarn_Calculation()

        End If

    End Sub

    Private Sub cbo_ClothName_LostFocus_1111(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Clo_IdNo As Integer = 0
        Dim wftcnt_idno As Integer = 0

        If Trim(UCase(cbo_ClothName.Tag)) <> Trim(UCase(cbo_ClothName.Text)) Then
            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

            wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
            lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, wftcnt_idno)

            ConsumedYarn_Calculation()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, cbo_Filter_LoomNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_LoomNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clt_IdNo As Integer, Lom_IdNo As Integer
        Dim Condt As String = ""
        Dim Doff_Mtr As Double = 0
        Dim Chk_Mtr As Double = 0
        Dim StCode As String = "", BmNo As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clt_IdNo = 0
            Lom_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Weaver_ClothReceipt_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Weaver_ClothReceipt_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Weaver_ClothReceipt_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Cloth_Idno = " & Str(Val(Clt_IdNo)) & ")"
            End If

            Lom_IdNo = 0
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

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Cloth_Name,  d.Loom_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  LEFT OUTER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo  Where a.Receipt_Type = 'L' and a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & "  and " & Other_Condition & " Order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            Chk_Mtr = 0
            Doff_Mtr = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Loom_Name").ToString
                    dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Beam_No1").ToString
                    dgv_filter.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Beam_No2").ToString
                    dgv_filter.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.00")
                    dgv_filter.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Type1_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type2_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type3_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type4_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type5_Checking_Meters").ToString), "########0.00")
                    If Val(dgv_filter.Rows(n).Cells(8).Value) = 0 Then
                        dgv_filter.Rows(n).Cells(8).Value = ""
                    End If

                    Chk_Mtr = Chk_Mtr + (Val(dt2.Rows(i).Item("Type1_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type2_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type3_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type4_Checking_Meters").ToString) + Val(dt2.Rows(i).Item("Type5_Checking_Meters").ToString))
                    Doff_Mtr = Doff_Mtr + Format(Val(dt2.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.00")

                Next i

            End If

            dgv_fILTER_Total.Rows.Add()
            dgv_fILTER_Total.Rows(0).Cells(2).Value = "TOTAL"
            dgv_fILTER_Total.Rows(0).Cells(7).Value = Format(Val(Doff_Mtr), "########0.00")
            dgv_fILTER_Total.Rows(0).Cells(8).Value = Format(Val(Chk_Mtr), "########0.00")

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

    End Sub

    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, dtp_FilterTo_date, cbo_Filter_PartyName, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_LoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
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
        Try
            Dim movno As String = ""

            movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_back.Enabled = True
                pnl_filter.Visible = False
            End If

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE OPEN FILTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub



    Private Sub cbo_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "( Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " ) ", "(Loom_IdNo = 0 )")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        End If

        cbo_LoomNo.Tag = cbo_LoomNo.Text
    End Sub

    Private Sub cbo_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, msk_Date, Nothing, "Loom_Head", "Loom_Name", "( Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " ) ", "(Loom_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, msk_Date, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
        End If

        If e.KeyCode = 40 And cbo_LoomNo.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
            If cbo_Pcs_LastPiece_Status.Visible = True And cbo_Pcs_LastPiece_Status.Enabled = True Then
                cbo_Pcs_LastPiece_Status.Focus()
            ElseIf txt_BarCode.Visible = True And txt_BarCode.Enabled = True Then
                txt_BarCode.Focus()
            Else
                cbo_ClothName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, Nothing, "Loom_Head", "Loom_Name", "( Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " ) ", "(Loom_IdNo = 0 )")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        End If

        If Asc(e.KeyChar) = 13 Then

            'If Trim(cbo_LoomNo.Text) <> "" And (Trim(UCase(cbo_LoomNo.Text)) <> Trim(UCase(cbo_LoomNo.Tag)) Or Trim(lbl_KnotCode.Text) = "") Then
            '    btn_Selection_Click(sender, e)
            'End If

            If cbo_Pcs_LastPiece_Status.Visible = True And cbo_Pcs_LastPiece_Status.Enabled = True Then
                cbo_Pcs_LastPiece_Status.Focus()
            ElseIf txt_BarCode.Visible = True And txt_BarCode.Enabled = True Then
                txt_BarCode.Focus()
            Else
                cbo_ClothName.Focus()
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

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_DoffMtrs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DoffMtrs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_CrimpPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CrimpPerc.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_CrimpPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CrimpPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub ConsumedPavu_Calculation()
        Dim CloID As Integer
        Dim ConsPavu As Single
        Dim LmID As Integer
        Dim NoofBeams As Integer = 0

        CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        LmID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        ConsPavu = Common_Procedures.get_Pavu_Consumption(con, CloID, LmID, Val(txt_DoffMtrs.Text), Trim(lbl_WidthType.Text), , Val(txt_CrimpPerc.Text))

        lbl_ConsPavu.Text = Format(ConsPavu, "#########0.00")

        If Trim(lbl_BeamNo1.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
            NoofBeams = 2
        Else
            NoofBeams = 1
        End If
        If Val(NoofBeams) = 0 Then NoofBeams = 1

        lbl_BeamConsPavu.Text = Format(Val(lbl_ConsPavu.Text) / NoofBeams, "#########0.00")

    End Sub

    Private Sub ConsumedYarn_Calculation()
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim CloID As Integer
        Dim ConsYarn As String = 0, vTOTConsYarn As String = 0
        Dim vTot_ChkMtrs As String = 0
        Dim vTot_ChkWGT As String = 0
        Dim n, I As Integer
        Dim vWEFT_CONSFOR_MTRS_OR_WGT As String = 0

        CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        vTot_ChkMtrs = Val(txt_DoffMtrs.Text)
        vTot_ChkWGT = 0

        dgv_Weft_Consumption_Details.Rows.Clear()

        If Common_Procedures.settings.Cloth_WeftConsumption_Multiple_WeftCount_Status = 1 Then '---- SOTEXPA QUALIDIS TEXTILE (SULUR)

            Dim vMULTIWEFTSTS As String = 0

            vMULTIWEFTSTS = Common_Procedures.get_FieldValue(con, "cloth_Head", "Multiple_WeftCount_Status", "(cloth_idno = " & Str(Val(CloID)) & ")")
            If Val(vMULTIWEFTSTS) = 1 Then

                vTOTConsYarn = 0
                Da4 = New SqlClient.SqlDataAdapter("Select a.*, b.count_name from Cloth_Additional_Weft_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo Where Cloth_Idno = " & Str(Val(CloID)), con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    For I = 0 To Dt4.Rows.Count - 1

                        If Trim(UCase(Dt4.Rows(I).Item("ConsumptionFor_Meters_Weight").ToString)) = "WEIGHT" Then
                            vWEFT_CONSFOR_MTRS_OR_WGT = Val(vTot_ChkWGT)
                        Else
                            vWEFT_CONSFOR_MTRS_OR_WGT = Val(vTot_ChkMtrs)
                        End If

                        If Trim(UCase(Dt4.Rows(I).Item("Gram_Perc_Type").ToString)) = "%" Then
                            ConsYarn = Format(Val(vWEFT_CONSFOR_MTRS_OR_WGT) * Val(Dt4.Rows(I).Item("Consumption_Gram_Perc").ToString) / 100, "##########0.000")
                        Else
                            ConsYarn = Format(Val(vWEFT_CONSFOR_MTRS_OR_WGT) * Val(Dt4.Rows(I).Item("Consumption_Gram_Perc").ToString), "##########0.000")
                        End If

                        n = dgv_Weft_Consumption_Details.Rows.Add()
                        dgv_Weft_Consumption_Details.Rows(n).Cells(0).Value = Dt4.Rows(I).Item("count_name").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(1).Value = Dt4.Rows(I).Item("Gram_Perc_Type").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(2).Value = Val(Dt4.Rows(I).Item("Consumption_Gram_Perc").ToString)
                        dgv_Weft_Consumption_Details.Rows(n).Cells(3).Value = Format(Val(ConsYarn), "#########0.000")

                        vTOTConsYarn = Val(vTOTConsYarn) + Val(ConsYarn)

                    Next

                End If
                Dt4.Clear()

                lbl_ConsWeftYarn.Text = Format(Val(vTOTConsYarn), "#########0.000")

            Else
                GoTo GOTOLOOP1

            End If

        Else

GOTOLOOP1:
            ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(vTot_ChkMtrs))
            lbl_ConsWeftYarn.Text = Format(Val(ConsYarn), "#########0.000")

            dgv_Weft_Consumption_Details.Rows.Clear()

            If btn_Show_WeftConsumption_Details.Visible = True Then

                If Val(lbl_ConsWeftYarn.Text) <> 0 Then

                    Dim vCLO_WGTPERMTR As String = 0
                    vCLO_WGTPERMTR = Common_Procedures.get_FieldValue(con, "cloth_Head", "Weight_Meter_Weft", "(cloth_idno = " & Str(Val(CloID)) & ")")

                    n = dgv_Weft_Consumption_Details.Rows.Add()

                    dgv_Weft_Consumption_Details.Rows(n).Cells(0).Value = lbl_WeftCount.Text
                    dgv_Weft_Consumption_Details.Rows(n).Cells(1).Value = "GRAM"
                    dgv_Weft_Consumption_Details.Rows(n).Cells(2).Value = vCLO_WGTPERMTR
                    dgv_Weft_Consumption_Details.Rows(n).Cells(3).Value = lbl_ConsWeftYarn.Text

                End If

            End If

        End If

    End Sub

    Private Sub ConsumedYarn_Calculation_111()
        Dim CloID As Integer
        Dim ConsYarn As Single

        CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(txt_DoffMtrs.Text))

        lbl_ConsWeftYarn.Text = Format(ConsYarn, "#########0.000")

    End Sub

    Private Sub txt_DoffMtrs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_DoffMtrs.TextChanged
        ConsumedPavu_Calculation()
        ConsumedYarn_Calculation()
    End Sub

    Private Sub txt_CrimpPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CrimpPerc.TextChanged
        ConsumedPavu_Calculation()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Da5 As New SqlClient.SqlDataAdapter
        Dim Da6 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim Dt6 As New DataTable
        Dim Lm_ID As Integer
        Dim NewCode As String = ""
        Dim led_id As Integer = 0
        Dim WftCnt_ID As Integer = 0, vWFTCNTIDno As Integer
        Dim vORDNO As String = ""
        Dim vSAME_ORDNO_STS As Boolean = True
        Dim vPANELNO As String = ""
        Dim vWFTCNT_NM As String = ""



        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom NO", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text)
        WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)

        If Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) = "YES" Then

            btn_KnottingSelection_Click(sender, e)

        Else

            Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.Ledger_Name, c.Cloth_IdNo, c.Cloth_Name, c.Multiple_WeftCount_Status, d.EndsCount_Name, e.Count_Name, f.Loom_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Count_Head e ON a.Count_IdNo = e.Count_IdNo LEFT OUTER JOIN Loom_Head f ON a.Loom_IdNo = f.Loom_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and " & Other_Condition, con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                lbl_PartyName.Text = Dt1.Rows(0).Item("Ledger_Name").ToString

                lbl_KnotCode.Text = Dt1.Rows(0).Item("Beam_Knotting_Code").ToString
                lbl_KnotNo.Text = Dt1.Rows(0).Item("Beam_Knotting_No").ToString

                cbo_ClothName.Text = Dt1.Rows(0).Item("Cloth_Name").ToString
                lbl_WidthType.Text = Dt1.Rows(0).Item("Width_Type").ToString
                lbl_EndsCount.Text = Dt1.Rows(0).Item("EndsCount_Name").ToString
                lbl_WeftCount.Text = ""

                If Val(Dt1.Rows(0).Item("Multiple_WeftCount_Status").ToString) = 1 Then
                    btn_Show_WeftConsumption_Details.Visible = True
                    btn_Show_WeftConsumption_Details.BringToFront()
                    lbl_WeftCount.Text = ""
                    get_Multiple_WeftYarn_Consumption_Count_Details(Val(Dt1.Rows(0).Item("Cloth_IdNo").ToString))

                Else
                    btn_Show_WeftConsumption_Details.Visible = False
                    lbl_WeftCount.Text = Dt1.Rows(0).Item("Count_Name").ToString
                    dgv_Weft_Consumption_Details.Rows.Clear()

                End If

                lbl_SetCode1.Text = Dt1.Rows(0).Item("Set_Code1").ToString
                lbl_SetNo1.Text = Dt1.Rows(0).Item("Set_No1").ToString
                lbl_BeamNo1.Text = Dt1.Rows(0).Item("Beam_No1").ToString
                lbl_BalMtrs1.Text = Dt1.Rows(0).Item("Balance_Meters1").ToString

                lbl_TotMtrs1.Text = ""
                lbl_WarpMillName.Text = ""
                lbl_WarpLotNo.Text = ""
                Da2 = New SqlClient.SqlDataAdapter("Select Meters, Mill_IdNo, Warp_LotNo from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                Dt2 = New DataTable
                Da2.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    lbl_TotMtrs1.Text = Dt2.Rows(0).Item("Meters").ToString
                    lbl_WarpMillName.Text = Common_Procedures.Mill_IdNoToName(con, Val(Dt2.Rows(0).Item("Mill_IdNo").ToString))
                    lbl_WarpLotNo.Text = Dt2.Rows(0).Item("Warp_LotNo").ToString
                End If
                Dt2.Clear()

                lbl_SetCode2.Text = Dt1.Rows(0).Item("Set_Code2").ToString
                lbl_SetNo2.Text = Dt1.Rows(0).Item("Set_No2").ToString
                lbl_BeamNo2.Text = Dt1.Rows(0).Item("Beam_No2").ToString
                lbl_BalMtrs2.Text = Dt1.Rows(0).Item("Balance_Meters2").ToString
                lbl_TotMtrs2.Text = ""


                Da2 = New SqlClient.SqlDataAdapter("Select Meters from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
                Dt2 = New DataTable
                Da2.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    lbl_TotMtrs2.Text = Dt2.Rows(0).Item("Meters").ToString
                End If
                Dt2.Clear()

                'txt_DoffMtrs.Text = Dt1.Rows(0).Item("Doff_Meters").ToString
                'txt_CrimpPerc.Text = Dt1.Rows(0).Item("Crimp_Percentage").ToString
                'lbl_ConsPavu.Text = Dt1.Rows(0).Item("ConsumedPavu_Receipt").ToString
                'lbl_ConsWeftYarn.Text = Dt1.Rows(0).Item("ConsumedYarn_Receipt").ToString

                ' --- code by gopi 2025-02-08

                cbo_Weft_MillName.Text = Common_Procedures.Mill_IdNoToName(con, Val(Dt1.Rows(0).Item("Weft_Mill_IdNo").ToString))
                cbo_WeftLotNo.Text = Dt1.Rows(0).Item("Weft_LotNo").ToString
                lbl_FabricLotNo.Text = Dt1.Rows(0).Item("Fabric_LotNo").ToString

                cbo_ClothSales_OrderNo.Text = Dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString
                cbo_PanelQuality.Text = Dt1.Rows(0).Item("Panel_Quality").ToString

                ' --- Command by gopi 2025-02-08

                'cbo_Weft_MillName.Text = Common_Procedures.Mill_IdNoToName(con, Val(Dt2.Rows(0).Item("Weft_Mill_IdNo").ToString))
                'cbo_WeftLotNo.Text = Dt2.Rows(0).Item("Weft_LotNo").ToString
                'lbl_FabricLotNo.Text = Dt2.Rows(0).Item("Fabric_LotNo").ToString

                'cbo_ClothSales_OrderNo.Text = Dt2.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString
                'cbo_PanelQuality.Text = Dt2.Rows(0).Item("Panel_Quality").ToString


            Else

                Da3 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_IdNo, c.Cloth_Name, c.Crimp_Percentage, c.Multiple_WeftCount_Status, d.EndsCount_Name, e.Count_Name from Beam_Knotting_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 = c.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Count_Head e ON c.Cloth_WeftCount_IdNo = e.Count_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = '' Order by a.Beam_Knotting_Date, a.for_OrderBy, a.Beam_Knotting_Code", con)
                'Da3 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_IdNo, c.Cloth_Name, c.Crimp_Percentage, c.Multiple_WeftCount_Status, d.EndsCount_Name, e.Count_Name from Beam_Knotting_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 = c.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Count_Head e ON c.Cloth_WeftCount_IdNo = e.Count_IdNo Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = '' Order by a.Beam_Knotting_Date, a.for_OrderBy, a.Beam_Knotting_Code", con)
                Dt3 = New DataTable
                Da3.Fill(Dt3)
                If Dt3.Rows.Count > 0 Then
                    lbl_PartyName.Text = Dt3.Rows(0).Item("Ledger_Name").ToString

                    lbl_KnotCode.Text = Dt3.Rows(0).Item("Beam_Knotting_Code").ToString
                    lbl_KnotNo.Text = Dt3.Rows(0).Item("Beam_Knotting_No").ToString
                    lbl_EndsCount.Text = Dt3.Rows(0).Item("EndsCount_Name").ToString
                    lbl_WidthType.Text = Dt3.Rows(0).Item("Width_Type").ToString

                    cbo_ClothName.Text = ""
                    lbl_WeftCount.Text = ""
                    vWFTCNT_NM = ""
                    If Val(Dt3.Rows(0).Item("Cloth_Idno2").ToString) = 0 And Val(Dt3.Rows(0).Item("Cloth_Idno3").ToString) = 0 And Val(Dt3.Rows(0).Item("Cloth_Idno4").ToString) = 0 Then
                        cbo_ClothName.Text = Dt3.Rows(0).Item("Cloth_Name").ToString
                        vWFTCNT_NM = Dt3.Rows(0).Item("Count_Name").ToString
                    ElseIf Val(Dt3.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt3.Rows(0).Item("Cloth_Idno2").ToString) And Val(Dt3.Rows(0).Item("Cloth_Idno3").ToString) = 0 And Val(Dt3.Rows(0).Item("Cloth_Idno4").ToString) = 0 Then
                        cbo_ClothName.Text = Dt3.Rows(0).Item("Cloth_Name").ToString
                        vWFTCNT_NM = Dt3.Rows(0).Item("Count_Name").ToString
                    ElseIf Val(Dt3.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt3.Rows(0).Item("Cloth_Idno2").ToString) And Val(Dt3.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt3.Rows(0).Item("Cloth_Idno3").ToString) And Val(Dt3.Rows(0).Item("Cloth_Idno4").ToString) = 0 Then
                        cbo_ClothName.Text = Dt3.Rows(0).Item("Cloth_Name").ToString
                        vWFTCNT_NM = Dt3.Rows(0).Item("Count_Name").ToString
                    ElseIf Val(Dt3.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt3.Rows(0).Item("Cloth_Idno2").ToString) And Val(Dt3.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt3.Rows(0).Item("Cloth_Idno3").ToString) And Val(Dt3.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt3.Rows(0).Item("Cloth_Idno4").ToString) Then
                        cbo_ClothName.Text = Dt3.Rows(0).Item("Cloth_Name").ToString
                        vWFTCNT_NM = Dt3.Rows(0).Item("Count_Name").ToString
                    End If

                    If Val(Dt3.Rows(0).Item("Multiple_WeftCount_Status").ToString) = 1 Then
                        btn_Show_WeftConsumption_Details.Visible = True
                        btn_Show_WeftConsumption_Details.BringToFront()
                        lbl_WeftCount.Text = ""
                        get_Multiple_WeftYarn_Consumption_Count_Details(Val(Dt3.Rows(0).Item("Cloth_IdNo").ToString))
                    Else
                        lbl_WeftCount.Text = vWFTCNT_NM
                        btn_Show_WeftConsumption_Details.Visible = False
                        dgv_Weft_Consumption_Details.Rows.Clear()
                    End If

                    'lbl_TotMtrs1.Text = ""
                    'lbl_BalMtrs1.Text = ""
                    'Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                    'Dt4 = New DataTable
                    'Da4.Fill(Dt4)
                    'If Dt4.Rows.Count > 0 Then
                    '    lbl_TotMtrs1.Text = Dt4.Rows(0).Item("Meters").ToString
                    '    lbl_BalMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt3.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                    'End If
                    'Dt4.Clear()

                    lbl_SetCode1.Text = Dt3.Rows(0).Item("Set_Code1").ToString
                    lbl_SetNo1.Text = Dt3.Rows(0).Item("Set_No1").ToString
                    lbl_BeamNo1.Text = Dt3.Rows(0).Item("Beam_No1").ToString

                    lbl_TotMtrs1.Text = ""
                    lbl_BalMtrs1.Text = ""
                    lbl_WarpMillName.Text = ""
                    lbl_WarpLotNo.Text = ""
                    Da4 = New SqlClient.SqlDataAdapter("Select Meters, Production_Meters, Mill_IdNo, Warp_LotNo from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                    Dt4 = New DataTable
                    Da4.Fill(Dt4)
                    If Dt4.Rows.Count > 0 Then
                        lbl_TotMtrs1.Text = Dt4.Rows(0).Item("Meters").ToString
                        lbl_BalMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                        lbl_WarpMillName.Text = Common_Procedures.Mill_IdNoToName(con, Val(Dt4.Rows(0).Item("Mill_IdNo").ToString))
                        lbl_WarpLotNo.Text = Dt4.Rows(0).Item("Warp_LotNo").ToString
                    End If
                    Dt4.Clear()

                    lbl_SetCode2.Text = Dt3.Rows(0).Item("Set_Code2").ToString
                    lbl_SetNo2.Text = Dt3.Rows(0).Item("Set_No2").ToString
                    lbl_BeamNo2.Text = Dt3.Rows(0).Item("Beam_No2").ToString
                    lbl_BalMtrs2.Text = ""
                    lbl_TotMtrs2.Text = ""
                    If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                        Da4 = New SqlClient.SqlDataAdapter("Select Meters, Production_Meters from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
                        Dt4 = New DataTable
                        Da4.Fill(Dt4)
                        If Dt4.Rows.Count > 0 Then
                            lbl_TotMtrs2.Text = Dt4.Rows(0).Item("Meters").ToString
                            lbl_BalMtrs2.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                        End If
                        Dt4.Clear()
                    End If

                    'txt_DoffMtrs.Text = dt3.Rows(0).Item("Doff_Meters").ToString
                    txt_CrimpPerc.Text = Dt3.Rows(0).Item("Crimp_Percentage").ToString
                    'lbl_ConsPavu.Text = dt3.Rows(0).Item("ConsumedPavu_Receipt").ToString
                    'lbl_ConsWeftYarn.Text = dt3.Rows(0).Item("ConsumedYarn_Receipt").ToString


                    vSAME_ORDNO_STS = True
                    vPANELNO = "A"
                    vORDNO = Dt3.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                    If Trim(Dt3.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_2").ToString) <> "" Then
                        If Trim(UCase(vORDNO)) <> Trim(UCase(Dt3.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_2").ToString)) Then
                            vSAME_ORDNO_STS = False
                            vPANELNO = ""
                        End If
                    End If

                    If vSAME_ORDNO_STS = True Then
                        If Trim(Dt3.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_3").ToString) <> "" Then
                            If Trim(UCase(vORDNO)) <> Trim(UCase(Dt3.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_3").ToString)) Then
                                vSAME_ORDNO_STS = False
                                vPANELNO = ""
                            End If
                        End If
                    End If

                    If vSAME_ORDNO_STS = True Then
                        If Trim(Dt3.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_4").ToString) <> "" Then
                            If Trim(UCase(vORDNO)) <> Trim(UCase(Dt3.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_4").ToString)) Then
                                vSAME_ORDNO_STS = False
                                vPANELNO = ""
                            End If
                        End If
                    End If

                    If vSAME_ORDNO_STS = True Then
                        cbo_ClothSales_OrderNo.Text = vORDNO
                        cbo_PanelQuality.Text = vPANELNO
                    Else
                        cbo_ClothSales_OrderNo.Text = ""
                        cbo_PanelQuality.Text = ""
                    End If



                End If
                Dt3.Clear()

                lbl_PoNo.Text = ""
                'lbl_WarpLotNo.Text = ""
                Da5 = New SqlClient.SqlDataAdapter("select po_no , warp_Lot_No from  JobWork_Pavu_Receipt_Details  Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                Dt5 = New DataTable
                Da5.Fill(Dt5)
                If Dt5.Rows.Count = 1 Then
                    lbl_PoNo.Text = Dt5.Rows(0).Item("Po_no").ToString
                    If Trim(lbl_WarpLotNo.Text) = "" Then
                        lbl_WarpLotNo.Text = Dt5.Rows(0).Item("warp_Lot_No").ToString
                    End If
                End If
                Dt5.Clear()

                cbo_WeftLotNo.Text = ""

                led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text)
                WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)

                Da6 = New SqlClient.SqlDataAdapter("select distinct(weft_Lot_No) as weftLotNo from JobWork_yarn_Receipt_Details Where Ledger_Idno = " & Str(Val(led_id)) & " and Count_idno = " & Str(Val(WftCnt_ID)) & " and Po_No = '" & Trim(lbl_PoNo.Text) & "'", con)
                Dt6 = New DataTable
                Da6.Fill(Dt6)
                If Dt6.Rows.Count > 0 Then
                    If Dt6.Rows.Count = 1 Then
                        cbo_WeftLotNo.Text = Dt6.Rows(0).Item("weftLotNo").ToString
                    End If
                End If
                Dt6.Clear()


            End If

        End If

        cbo_LoomNo.Tag = cbo_LoomNo.Text
        cbo_Pcs_LastPiece_Status.Tag = cbo_Pcs_LastPiece_Status.Text

        Dt1.Dispose()
        Da1.Dispose()

        Dt2.Dispose()
        Da2.Dispose()

        Dt3.Dispose()
        Da3.Dispose()

        Dt4.Dispose()
        Da4.Dispose()

        'If cbo_Weft_MillName.Enabled And cbo_Weft_MillName.Visible Then
        '    cbo_Weft_MillName.Focus()
        'ElseIf cbo_DoffShift.Enabled And cbo_DoffShift.Visible Then
        '    cbo_DoffShift.Focus()
        'ElseIf txt_DoffMtrs.Enabled And txt_DoffMtrs.Visible Then
        '    txt_DoffMtrs.Focus()
        'End If
        If cbo_PanelQuality.Visible And cbo_PanelQuality.Enabled Then
            cbo_PanelQuality.Focus()
            'ElseIf cbo_ClothName.Enabled And cbo_ClothName.Visible Then
            '    cbo_ClothName.Focus()
        ElseIf cbo_DoffShift.Enabled And cbo_DoffShift.Visible Then
            cbo_DoffShift.Focus()
        ElseIf txt_DoffMtrs.Enabled And txt_DoffMtrs.Visible Then
            txt_DoffMtrs.Focus()
        End If

    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_KnottingSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_KnottingSelection.Click
        pnl_Selection.Visible = True
        pnl_back.Enabled = False

        cbo_SelectionLoomNo.Text = ""
        dgv_Selection.Rows.Clear()
        If Trim(cbo_LoomNo.Text) <> "" Then
            cbo_SelectionLoomNo.Text = cbo_LoomNo.Text
            btn_ShowKnottingDetails_Click(sender, e)
        End If

        If cbo_SelectionLoomNo.Enabled And cbo_SelectionLoomNo.Visible Then cbo_SelectionLoomNo.Focus()

    End Sub

    Private Sub btn_ShowKnottingDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ShowKnottingDetails.Click
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Lm_ID As Integer
        Dim NewCode As String = ""
        Dim EntKnotCode As String = ""
        Dim n As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim SNo As Integer = 0
        Dim vNOOFITEMS As Integer = 0

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_SelectionLoomNo.Text)
        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES Not SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_SelectionLoomNo.Enabled Then cbo_SelectionLoomNo.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        EntKnotCode = ""
        vNOOFITEMS = 0
        SNo = 0
        dgv_Selection.Rows.Clear()

        Da1 = New SqlClient.SqlDataAdapter("Select a.*, tP.Ledger_Name, b.Loom_Name, c.Cloth_Name, c.Crimp_Percentage, d.EndsCount_Name, e.Count_Name from Weaver_Cloth_Receipt_Head tW INNER JOIN Beam_Knotting_Head a On Tw.Beam_Knotting_Code = a.Beam_Knotting_Code INNER JOIN Ledger_Head tP On a.Ledger_IdNo <> 0 And a.Ledger_IdNo = tP.Ledger_IdNo INNER JOIN Loom_Head b On a.Loom_IdNo <> 0 And a.Loom_IdNo = b.Loom_IdNo INNER JOIN Cloth_Head c On a.Cloth_IdNo1 <> 0 And a.Cloth_IdNo1 = c.Cloth_IdNo INNER JOIN EndsCount_Head d On a.EndsCount_IdNo <> 0 And a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Count_Head e On c.Cloth_WeftCount_IdNo <> 0 And c.Cloth_WeftCount_IdNo = e.Count_IdNo Where tW.Loom_IdNo = " & Str(Val(Lm_ID)) & " And tW.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And tW.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and " & Other_Condition & " Order by a.Beam_Knotting_Date Desc, a.for_OrderBy Desc, a.Beam_Knotting_Code Desc", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then

            n = dgv_Selection.Rows.Add()

            vNOOFITEMS = vNOOFITEMS + 1

            SNo = SNo + 1
            dgv_Selection.Rows(n).Cells(0).Value = Val(SNo)
            dgv_Selection.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Beam_Knotting_Date").ToString), "dd-MM-yyyy")
            dgv_Selection.Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_Knotting_No").ToString
            dgv_Selection.Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Loom_Name").ToString
            dgv_Selection.Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
            dgv_Selection.Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
            dgv_Selection.Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Set_No1").ToString
            dgv_Selection.Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_No1").ToString
            dgv_Selection.Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Beam_No2").ToString
            dgv_Selection.Rows(n).Cells(9).Value = ""
            dgv_Selection.Rows(n).Cells(10).Value = ""

            If Trim(Dt1.Rows(i).Item("Set_Code1").ToString) <> "" And Trim(Dt1.Rows(i).Item("Beam_No1").ToString) <> "" Then
                Da1 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(i).Item("Beam_No1").ToString) & "'", con)
                Dt2 = New DataTable
                Da1.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    dgv_Selection.Rows(n).Cells(9).Value = Dt2.Rows(0).Item("Meters").ToString
                    dgv_Selection.Rows(n).Cells(10).Value = Format(Val(Dt2.Rows(0).Item("Meters").ToString) - Val(Dt2.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                End If
                Dt2.Clear()
            End If

            dgv_Selection.Rows(n).Cells(11).Value = "1"
            dgv_Selection.Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString

            For j = 0 To dgv_Selection.ColumnCount - 1
                dgv_Selection.Rows(n).Cells(j).Style.ForeColor = Color.Red
            Next

            EntKnotCode = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString

        End If
        Dt1.Clear()

        Da1 = New SqlClient.SqlDataAdapter("select a.*, tP.Ledger_Name, b.Loom_Name, c.Cloth_Name, c.Crimp_Percentage, d.EndsCount_Name, e.Count_Name from Beam_Knotting_Head a INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo INNER JOIN Loom_Head b ON a.Loom_IdNo <> 0 and a.Loom_IdNo = b.Loom_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 <> 0 and a.Cloth_IdNo1 = c.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Count_Head e ON c.Cloth_WeftCount_IdNo <> 0 and c.Cloth_WeftCount_IdNo = e.Count_IdNo Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code <> '" & Trim(EntKnotCode) & "' Order by a.Beam_Knotting_Date Desc, a.for_OrderBy Desc, a.Beam_Knotting_Code Desc", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then

            For i = 0 To Dt1.Rows.Count - 1


                If chk_Show_All_Knottings.Visible = True And chk_Show_All_Knottings.Enabled = True Then
                    If chk_Show_All_Knottings.Checked = False Then
                        If vNOOFITEMS = 3 Then
                            Exit For
                        End If
                    End If
                End If

                n = dgv_Selection.Rows.Add()
                vNOOFITEMS = vNOOFITEMS + 1
                SNo = SNo + 1
                dgv_Selection.Rows(n).Cells(0).Value = Val(SNo)
                dgv_Selection.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Beam_Knotting_Date").ToString), "dd-MM-yyyy")
                dgv_Selection.Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_Knotting_No").ToString
                dgv_Selection.Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Loom_Name").ToString
                dgv_Selection.Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                dgv_Selection.Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                dgv_Selection.Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Set_No1").ToString
                dgv_Selection.Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_No1").ToString
                dgv_Selection.Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Beam_No2").ToString
                dgv_Selection.Rows(n).Cells(9).Value = ""
                dgv_Selection.Rows(n).Cells(10).Value = ""

                If Trim(Dt1.Rows(i).Item("Set_Code1").ToString) <> "" And Trim(Dt1.Rows(i).Item("Beam_No1").ToString) <> "" Then
                    Da1 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(i).Item("Beam_No1").ToString) & "'", con)
                    Dt2 = New DataTable
                    Da1.Fill(Dt2)
                    If Dt2.Rows.Count > 0 Then
                        dgv_Selection.Rows(n).Cells(9).Value = Dt2.Rows(0).Item("Meters").ToString
                        dgv_Selection.Rows(n).Cells(10).Value = Format(Val(Dt2.Rows(0).Item("Meters").ToString) - Val(Dt2.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                    End If
                    Dt2.Clear()
                End If

                dgv_Selection.Rows(n).Cells(11).Value = ""
                dgv_Selection.Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString

                For j = 0 To dgv_Selection.ColumnCount - 1
                    dgv_Selection.Rows(n).Cells(j).Style.ForeColor = Color.Black
                Next

            Next

        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da1.Dispose()

        Dt2.Dispose()

        If dgv_Selection.Rows.Count > 0 Then
            If dgv_Selection.Enabled And dgv_Selection.Visible Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
            End If

        Else
            If cbo_SelectionLoomNo.Enabled And cbo_SelectionLoomNo.Visible Then cbo_SelectionLoomNo.Focus()

        End If

    End Sub

    Private Sub Select_Knotting(ByVal RwIndx As Integer)
        Dim i As Integer
        Dim j As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(11).Value = ""
                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Black
                    Next
                Next

                .Rows(RwIndx).Cells(11).Value = 1
                For i = 0 To .ColumnCount - 1
                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                Next

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        Try

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then

                If dgv_Selection.Rows.Count > 0 Then

                    If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                        n = dgv_Selection.CurrentCell.RowIndex

                        Select_Knotting(n)

                        e.Handled = True

                    End If

                End If

            End If

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Knotting(e.RowIndex)
    End Sub

    Private Sub dgv_Selection_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellDoubleClick
        Select_Knotting(e.RowIndex)
        Close_Knotting_Selection()
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Knotting_Selection()
    End Sub

    Private Sub Close_Knotting_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim KnotCode As String = ""
        Dim vWFTCNT_NM As String = ""

        KnotCode = ""
        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(11).Value) = 1 Then
                KnotCode = dgv_Selection.Rows(i).Cells(12).Value
                Exit For

            End If

        Next

        Da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_IdNo, c.Cloth_Name, c.Crimp_Percentage, c.Multiple_WeftCount_Status, d.EndsCount_Name, e.Count_Name, f.Loom_name from Beam_Knotting_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 <> 0 and a.Cloth_IdNo1 = c.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Count_Head e ON c.Cloth_WeftCount_IdNo <> 0 and c.Cloth_WeftCount_IdNo = e.Count_IdNo INNER JOIN Loom_Head f ON a.Loom_IdNo <> 0 and a.Loom_IdNo = f.Loom_IdNo Where a.Beam_Knotting_Code = '" & Trim(KnotCode) & "'", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then

            lbl_PartyName.Text = Dt1.Rows(0).Item("Ledger_Name").ToString

            cbo_LoomNo.Text = Dt1.Rows(0).Item("Loom_Name").ToString

            lbl_KnotCode.Text = Dt1.Rows(0).Item("Beam_Knotting_Code").ToString
            lbl_KnotNo.Text = Dt1.Rows(0).Item("Beam_Knotting_No").ToString
            lbl_EndsCount.Text = Dt1.Rows(0).Item("EndsCount_Name").ToString
            lbl_WidthType.Text = Dt1.Rows(0).Item("Width_Type").ToString

            cbo_ClothName.Text = ""
            lbl_WeftCount.Text = ""
            vWFTCNT_NM = ""
            If Val(Dt1.Rows(0).Item("Cloth_Idno2").ToString) = 0 And Val(Dt1.Rows(0).Item("Cloth_Idno3").ToString) = 0 And Val(Dt1.Rows(0).Item("Cloth_Idno4").ToString) = 0 Then
                cbo_ClothName.Text = Dt1.Rows(0).Item("Cloth_Name").ToString
                vWFTCNT_NM = Dt1.Rows(0).Item("Count_Name").ToString
            ElseIf Val(Dt1.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt1.Rows(0).Item("Cloth_Idno2").ToString) And Val(Dt1.Rows(0).Item("Cloth_Idno3").ToString) = 0 And Val(Dt1.Rows(0).Item("Cloth_Idno4").ToString) = 0 Then
                cbo_ClothName.Text = Dt1.Rows(0).Item("Cloth_Name").ToString
                vWFTCNT_NM = Dt1.Rows(0).Item("Count_Name").ToString
            ElseIf Val(Dt1.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt1.Rows(0).Item("Cloth_Idno2").ToString) And Val(Dt1.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt1.Rows(0).Item("Cloth_Idno3").ToString) And Val(Dt1.Rows(0).Item("Cloth_Idno4").ToString) = 0 Then
                cbo_ClothName.Text = Dt1.Rows(0).Item("Cloth_Name").ToString
                vWFTCNT_NM = Dt1.Rows(0).Item("Count_Name").ToString
            ElseIf Val(Dt1.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt1.Rows(0).Item("Cloth_Idno2").ToString) And Val(Dt1.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt1.Rows(0).Item("Cloth_Idno3").ToString) And Val(Dt1.Rows(0).Item("Cloth_Idno1").ToString) = Val(Dt1.Rows(0).Item("Cloth_Idno4").ToString) Then
                cbo_ClothName.Text = Dt1.Rows(0).Item("Cloth_Name").ToString
                vWFTCNT_NM = Dt1.Rows(0).Item("Count_Name").ToString
            End If

            If Val(Dt1.Rows(0).Item("Multiple_WeftCount_Status").ToString) = 1 Then
                btn_Show_WeftConsumption_Details.Visible = True
                btn_Show_WeftConsumption_Details.BringToFront()
                lbl_WeftCount.Text = ""
                get_Multiple_WeftYarn_Consumption_Count_Details(Val(Dt1.Rows(0).Item("Cloth_IdNo").ToString))
            Else
                lbl_WeftCount.Text = vWFTCNT_NM
                btn_Show_WeftConsumption_Details.Visible = False
                dgv_Weft_Consumption_Details.Rows.Clear()
            End If

            lbl_SetCode1.Text = Dt1.Rows(0).Item("Set_Code1").ToString
            lbl_SetNo1.Text = Dt1.Rows(0).Item("Set_No1").ToString
            lbl_BeamNo1.Text = Dt1.Rows(0).Item("Beam_No1").ToString

            lbl_TotMtrs1.Text = ""
            lbl_BalMtrs1.Text = ""
            lbl_WarpMillName.Text = ""
            lbl_WarpLotNo.Text = ""
            Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
            Dt4 = New DataTable
            Da4.Fill(Dt4)
            If Dt4.Rows.Count > 0 Then
                lbl_TotMtrs1.Text = Dt4.Rows(0).Item("Meters").ToString
                lbl_BalMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                lbl_WarpMillName.Text = Common_Procedures.Mill_IdNoToName(con, Val(Dt4.Rows(0).Item("Mill_IdNo").ToString))
                lbl_WarpLotNo.Text = Dt4.Rows(0).Item("Warp_LotNo").ToString
            End If
            Dt4.Clear()

            lbl_SetCode2.Text = Dt1.Rows(0).Item("Set_Code2").ToString
            lbl_SetNo2.Text = Dt1.Rows(0).Item("Set_No2").ToString
            lbl_BeamNo2.Text = Dt1.Rows(0).Item("Beam_No2").ToString
            lbl_BalMtrs2.Text = ""
            lbl_TotMtrs2.Text = ""
            If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    lbl_TotMtrs2.Text = Dt4.Rows(0).Item("Meters").ToString
                    lbl_BalMtrs2.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                End If
                Dt4.Clear()
            End If

            'txt_DoffMtrs.Text = dt1.Rows(0).Item("Doff_Meters").ToString
            txt_CrimpPerc.Text = Dt1.Rows(0).Item("Crimp_Percentage").ToString
            'lbl_ConsPavu.Text = dt1.Rows(0).Item("ConsumedPavu_Receipt").ToString
            'lbl_ConsWeftYarn.Text = dt1.Rows(0).Item("ConsumedYarn_Receipt").ToString

        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da1.Dispose()

        cbo_LoomNo.Tag = cbo_LoomNo.Text
        cbo_Pcs_LastPiece_Status.Tag = cbo_Pcs_LastPiece_Status.Text

        ConsumedPavu_Calculation()
        ConsumedYarn_Calculation()

        pnl_back.Enabled = True
        pnl_Selection.Visible = False
        If txt_DoffMtrs.Enabled And txt_DoffMtrs.Visible Then txt_DoffMtrs.Focus()

    End Sub

    Private Sub btn_CloseKnottingDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseKnottingDetails.Click
        Close_Knotting_Selection()
    End Sub

    Private Sub cbo_SelectionLoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SelectionLoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_SelectionLoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SelectionLoomNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SelectionLoomNo, Nothing, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_SelectionLoomNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Selection.Rows.Count > 0 Then
                If dgv_Selection.Enabled And dgv_Selection.Visible Then
                    dgv_Selection.Focus()
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                End If

            Else
                If btn_ShowKnottingDetails.Enabled And btn_ShowKnottingDetails.Visible Then btn_ShowKnottingDetails.Focus()

            End If

        End If


    End Sub

    Private Sub cbo_SelectionLoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SelectionLoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SelectionLoomNo, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        If Asc(e.KeyChar) = 13 Then
            btn_ShowKnottingDetails_Click(sender, e)
            'If MessageBox.Show("Do you want to select Pavu :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
            '    btn_Selection_Click(sender, e)
            'Else
            '    cbo_WidthType.Focus()
            'End If
        End If
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

    Public Sub Generate_Barcode()

        txt_BarCode.Text = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(lbl_RollNo.Text)) & "U"

    End Sub

    Private Sub btn_BarCodePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BarCodePrint.Click
        Common_Procedures.Print_OR_Preview_Status = 0
        'Printing_BarCode_Sticker()
        print_record()
    End Sub

    'Public Sub print_record() Implements Interface_MDIActions.print_record
    '    Common_Procedures.Print_OR_Preview_Status = 0
    '    Printing_BarCode_Sticker()
    'End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        pnl_Print.Visible = True
        pnl_back.Enabled = False
        txt_PrintFrom.Text = lbl_RollNo.Text
        txt_PrintTo.Text = lbl_RollNo.Text
        If txt_PrintFrom.Enabled And txt_PrintFrom.Visible Then
            txt_PrintFrom.Focus()
            txt_PrintFrom.SelectAll()
        End If
    End Sub

    Private Sub Printing_BarCode_Sticker()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim sql As String = ""


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("Select a.*, c.Cloth_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and " & Other_Condition, con)
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

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.18", 325, 118)
        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.5", 325, 150)
        PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument2.DefaultPageSettings.PaperSize = pkCustomSize1

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try


                'Else
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument2.Print()
                    End If

                Else
                    PrintDocument2.Print()

                End If

                'End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument2

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

        'Print_PDF_Status = False

    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String
        Dim SQL As String
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim Condt As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        prn_HeadIndx = 0


        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub



        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetBarCdStkr = 1

        Try



            SQL = "Select a.*, c.Cloth_Name from Weaver_Cloth_Receipt_Head a " &
                        " INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo " &
                        " Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) &
                          " and a.Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'" &
                            IIf(Trim(Condt) <> "", " and ", "") & Condt &
                          " and  " & Other_Condition
            da1 = New SqlClient.SqlDataAdapter(SQL, con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                '-----

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument2_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_BarCode_Sticker_Format1(e)

    End Sub

    Private Sub Printing_BarCode_Sticker_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, BarFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim CurY As Single
        Dim CurX As Single
        Dim BrCdX As Single = 20
        Dim BrCdY As Single = 100
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String


        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.18", 325, 118)
        PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument2.DefaultPageSettings.PaperSize = pkCustomSize1
        e.PageSettings.PaperSize = pkCustomSize1

        With PrintDocument2.DefaultPageSettings.Margins
            .Left = 5
            .Right = 2
            .Top = 5 ' 40
            .Bottom = 2
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument2.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If PrintDocument2.DefaultPageSettings.Landscape = True Then
            With PrintDocument2.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 2

        TxtHgt = 13.5

        Try

            If prn_HdDt.Rows.Count > 0 Then




                For noofitems = 1 To NoofItems_PerPage
                    NoofDets = 0

                    vFldMtrs = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("ReceiptMeters_Receipt").ToString), "##########0.00")
                    vBarCode = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Bar_Code").ToString)

                    If Val(vFldMtrs) <> 0 Then

                        If NoofDets >= NoofItems_PerPage Then
                            e.HasMorePages = True
                            Return
                        End If

                        CurY = TMargin

                        CurX = LMargin - 1
                        If noofitems Mod 2 = 0 Then
                            CurX = CurX + ((PageWidth + RMargin) \ 2)
                        End If

                        'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                        '    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        'Else
                        ItmNm1 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString)
                        'End If

                        ItmNm2 = ""
                        If Len(ItmNm1) > 21 Then
                            For I = 21 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 21

                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I + 1)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        pFont = New Font("Calibri", 9, FontStyle.Bold)
                        Common_Procedures.Print_To_PrintDocument(e, ItmNm1, CurX, CurY, 0, PrintWidth, pFont, , True)
                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 2
                            Common_Procedures.Print_To_PrintDocument(e, ItmNm2, CurX + 120, CurY, 1, PrintWidth, pFont, , True)
                        End If

                        pFont = New Font("Calibri", 9, FontStyle.Bold)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Lot NO: " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_ClothReceipt_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "METERS : " & vFldMtrs, CurX, CurY, 0, PrintWidth, pFont, , True)

                        If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Loom_IdNoToName(con, prn_HdDt.Rows(prn_HeadIndx).Item("Loom_IdNo").ToString), CurX, CurY, 0, PrintWidth, pFont, , True)
                        End If


                        'vBarCode = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Val(lbl_Company.Tag) & Trim(prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString) & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString) & Trim(Val(prn_DetBarCdStkr))

                        'vBarCode = Chr(204) & Trim(UCase(vBarCode)) & "g" & Chr(206)
                        'BarFont = New Font("Code 128", 36, FontStyle.Regular)
                        'BarFont = New Font("Code 128", 24, FontStyle.Regular)

                        vBarCode = "*" & Trim(UCase(vBarCode)) & "*"
                        'BarFont = New Font("Free 3 of 9", 24, FontStyle.Regular)
                        BarFont = New Font("Free 3 of 9", 18, FontStyle.Regular)

                        CurY = CurY + TxtHgt + 5
                        'CurY = CurY + TxtHgt + 2
                        'CurY = CurY + TxtHgt - 2
                        e.Graphics.DrawString(Trim(vBarCode), BarFont, Brushes.Black, CurX, CurY)

                        pFont = New Font("Calibri", 12, FontStyle.Bold)
                        'CurY = CurY + TxtHgt + TxtHgt + 5
                        CurY = CurY + TxtHgt + TxtHgt - 6
                        Common_Procedures.Print_To_PrintDocument(e, Trim(vBarCode), CurX + 5, CurY, 0, PrintWidth, pFont, , True)

                        NoofDets = NoofDets + 1

                    End If

                    prn_DetBarCdStkr = prn_DetBarCdStkr + 1

                    'Loop

                    prn_DetBarCdStkr = 1
                    prn_DetIndx = prn_DetIndx + 1



                    prn_HeadIndx = prn_HeadIndx + 1

                    If prn_HeadIndx > prn_HdDt.Rows.Count - 1 Then
                        Exit For
                    End If
                Next

            End If '' end of  If prn_HdDt.Rows.Count > 0 Then


            'prn_HeadIndx = prn_HeadIndx + 1

            If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
                e.HasMorePages = True
            Else
                'e.HasMorePages = False
                e.HasMorePages = False

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        'e.HasMorePages = False

    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim Pwd As String = ""
        Dim g As New Password

        g.ShowDialog()

        Pwd = Common_Procedures.Password_Input

        If Trim(Pwd) <> "TSSA7417" Then
            MessageBox.Show("Incorrect Password!...", "DOESNOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_Sts = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_RollNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(lbl_RollNo.Text) Then
            Timer1.Enabled = False
            SaveAll_Sts = False
            MessageBox.Show("All Entries Saved Successfully", "FOR SAVING", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Exit Sub
        Else
            movenext_record()
        End If
    End Sub

    Private Sub btn_BarcodePrint_prnpnl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BarcodePrint_prnpnl.Click
        Common_Procedures.Print_OR_Preview_Status = 0
        Printing_BarCode_Sticker()

    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        pnl_back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Close_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        btn_Print_Cancel_Click(sender, e)
    End Sub

    Private Sub txt_PrintTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintTo.KeyDown
        If e.KeyCode = Keys.Down Then
            btn_Print_Ok.Focus()
        End If
        If e.KeyCode = Keys.Up Then
            txt_PrintFrom.Focus()
        End If
    End Sub

    Private Sub txt_PrintTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PrintTo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_BarcodePrint_prnpnl_Click(sender, e)
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_Pcs_LastPiece_Status_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Pcs_LastPiece_Status.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_LoomNo, Nothing, "", "", "", "")
        If (e.KeyValue = 38 And cbo_Pcs_LastPiece_Status.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            cbo_LoomNo.Focus()

        ElseIf (e.KeyValue = 40 And cbo_Pcs_LastPiece_Status.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_PanelQuality.Visible Then
                cbo_PanelQuality.Focus()
            Else
                cbo_ClothName.Focus()


            End If
        End If

    End Sub

    Private Sub cbo_Pcs_LastPiece_Status_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Pcs_LastPiece_Status.KeyPress
        Dim clo_ID As Integer = 0
        Dim Lm_ID As Integer = 0

        clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then



            If Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) = "YES" Then

                If Trim(cbo_LoomNo.Text) <> "" And (Trim(UCase(cbo_LoomNo.Text)) <> Trim(UCase(cbo_LoomNo.Tag)) Or Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) <> Trim(UCase(cbo_Pcs_LastPiece_Status.Tag)) Or Trim(lbl_KnotCode.Text) = "") Then

                    Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
                    If Val(Lm_ID) <> 0 Then
                        btn_KnottingSelection_Click(sender, e)

                    Else

                        lbl_KnotCode.Text = ""
                        lbl_KnotNo.Text = ""

                        lbl_WidthType.Text = ""
                        txt_CrimpPerc.Text = ""

                        lbl_SetCode1.Text = ""
                        lbl_SetNo1.Text = ""
                        lbl_BeamNo1.Text = ""

                        lbl_SetCode2.Text = ""
                        lbl_SetNo2.Text = ""
                        lbl_BeamNo2.Text = ""

                        lbl_TotMtrs1.Text = ""
                        lbl_TotMtrs1.Text = ""

                        lbl_BalMtrs1.Text = ""
                        lbl_BalMtrs2.Text = ""

                        lbl_BeamConsPavu.Text = ""

                        lbl_WarpMillName.Text = ""
                        lbl_WarpLotNo.Text = ""
                        cbo_Weft_MillName.Text = ""
                        cbo_WeftLotNo.Text = ""
                        lbl_FabricLotNo.Text = ""


                    End If

                End If

            Else


                btn_Selection_Click(sender, e)


            End If
            If cbo_PanelQuality.Visible Then

                cbo_PanelQuality.Focus()
            Else
                'txt_DoffMtrs.Focus()
                cbo_ClothName.Focus()


            End If
        End If

    End Sub

    Private Sub cbo_Pcs_LastPiece_Status_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Pcs_LastPiece_Status.LostFocus
        If Trim(cbo_Pcs_LastPiece_Status.Text) = "" Then
            cbo_Pcs_LastPiece_Status.Text = "NO"
        ElseIf Trim(cbo_Pcs_LastPiece_Status.Text) <> "YES" And Trim(cbo_Pcs_LastPiece_Status.Text) <> "NO" Then
            cbo_Pcs_LastPiece_Status.Text = "NO"
        End If
    End Sub

    Private Sub cbo_Pcs_LastPiece_Status_GotFocus(sender As Object, e As EventArgs) Handles cbo_Pcs_LastPiece_Status.GotFocus
        cbo_Pcs_LastPiece_Status.Tag = cbo_Pcs_LastPiece_Status.Text
    End Sub

    Private Sub cbo_DoffShift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DoffShift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_IdNo)")
    End Sub

    Private Sub cbo_DoffShift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DoffShift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, txt_Doff_Shift_Meters, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_WeftLotNo.Visible And cbo_WeftLotNo.Enabled Then
                cbo_WeftLotNo.Focus()
            Else
                cbo_ClothName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DoffShift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DoffShift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_Doff_Shift_Meters, "Shift_Head", "Shift_Name", "", "(Shift_IdNo)")
    End Sub


    Private Sub txt_DoffMtrs_Enter(sender As Object, e As EventArgs) Handles txt_DoffMtrs.Enter
        Set_DoffMeters_Enability()
    End Sub

    Private Sub txt_Doff_Shift_Meters_TextChanged(sender As Object, e As EventArgs) Handles txt_Doff_Shift_Meters.TextChanged
        Set_DoffMeters_Enability()
    End Sub

    Private Sub cbo_DoffShift_TextChanged(sender As Object, e As EventArgs) Handles cbo_DoffShift.TextChanged
        Set_DoffMeters_Enability()
    End Sub

    Private Sub txt_Doff_Shift_Meters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Doff_Shift_Meters.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If Val(txt_Doff_Shift_Meters.Text) <> 0 Then
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If

            Else

                txt_DoffMtrs.Focus()

            End If
            'SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Doff_Shift_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Doff_Shift_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If Val(txt_Doff_Shift_Meters.Text) <> 0 Then
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If

            Else

                txt_DoffMtrs.Focus()

            End If

        End If
    End Sub

    Private Sub Set_DoffMeters_Enability()

        If cbo_DoffShift.Visible = True And txt_Doff_Shift_Meters.Visible = True Then

            If Trim(cbo_DoffShift.Text) <> "" And Val(txt_Doff_Shift_Meters.Text) > 0 Then

                txt_DoffMtrs.Enabled = False

                Dim Lm_ID As Integer
                Dim vPRODMTRS_FABWISE As String = ""
                Dim a() As String
                Dim b() As String
                Dim vENT_CLOID As Integer
                Dim vDOFFMTRS As String
                Dim vPRODMTRS As String

                Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
                vENT_CLOID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

                vDOFFMTRS = Common_Procedures.get_DoffMeters_from_Daily_Production(con, dtp_Date.Value.Date, lbl_RollNo.Text, Lm_ID, cbo_DoffShift.Text, Val(txt_Doff_Shift_Meters.Text), Other_Condition, "", lbl_KnotCode.Text, vPRODMTRS_FABWISE)

                a = Split(vPRODMTRS_FABWISE, "|")

                vPRODMTRS = 0
                For i = 0 To UBound(a)

                    b = Split(a(i), "~")
                    If UBound(b) >= 1 Then

                        If Val(b(0)) = Val(vENT_CLOID) Then
                            vPRODMTRS = b(1)
                            Exit For
                        End If

                    End If


                Next

                txt_DoffMtrs.Text = Format(Val(vPRODMTRS), "##########0.00")


                'get_DoffMeters_from_Daily_Production()

            Else

                txt_DoffMtrs.Enabled = True

            End If

        Else

            txt_DoffMtrs.Enabled = True

        End If

    End Sub

    Private Sub get_DoffMeters_from_DailyProduction_111()
        Dim cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim vROLLMTRS As String = 0
        Dim Lm_ID As Integer = 0
        Dim vORDBYNO As String
        Dim vPREVDOFFCODE As String = ""
        Dim vPREVDOFFDATE As Date = #1/1/2000#
        Dim vPREVDOFSHFTIDNO As Integer = 0
        Dim vPREVDOFSHFTMTRS As String = 0
        Dim vENTDOFFDATE As Date = #1/1/2000#
        Dim vENTDOFSHFTIDNO As Integer = 0
        Dim vENTDOFSHFTMTRS As String = 0
        Dim vDAT1 As Date = #1/1/2000#
        Dim vDAT2 As Date = #1/1/2000#
        Dim vSHFTPRODMTRS As String = 0

        vSHFTPRODMTRS = 0
        If cbo_DoffShift.Visible = True And txt_Doff_Shift_Meters.Visible = True Then

            If Trim(cbo_DoffShift.Text) <> "" And Val(txt_Doff_Shift_Meters.Text) > 0 Then

                Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
                If Lm_ID <> 0 Then

                    vORDBYNO = Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text)

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)

                    cmd.Connection = con

                    vENTDOFFDATE = dtp_Date.Value.Date
                    vENTDOFSHFTIDNO = Common_Procedures.Shift_NameToIdNo(con, cbo_DoffShift.Text)
                    vENTDOFSHFTMTRS = Val(txt_Doff_Shift_Meters.Text)

                    vPREVDOFFCODE = ""
                    vPREVDOFFDATE = #1/1/2000#
                    vPREVDOFSHFTIDNO = 0
                    vPREVDOFSHFTMTRS = 0

                    cmd.CommandText = "Select top 1 a.* from Weaver_Cloth_Receipt_Head a Where a.Receipt_Type = 'L' and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and ( a.Weaver_ClothReceipt_Date < @entrydate or (a.Weaver_ClothReceipt_Date = @entrydate and a.for_orderby < " & Str(Val(vORDBYNO)) & " ) ) and " & Other_Condition & " Order by a.Weaver_ClothReceipt_Date desc, a.for_orderby desc, a.Weaver_ClothReceipt_Code desc"
                    da1 = New SqlClient.SqlDataAdapter(cmd)
                    'da1 = New SqlClient.SqlDataAdapter("Select top 1 a.* from Weaver_Cloth_Receipt_Head a Where a.Receipt_Type = 'L' and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ( a.Weaver_ClothReceipt_Date < @entrydate or (a.Weaver_ClothReceipt_Date = @entrydate and a.for_orderby < " & Str(Val(vORDBYNO)) & " ) ) and " & Other_Condition & " Order by a.Weaver_ClothReceipt_Date desc, a.for_orderby desc, a.Weaver_ClothReceipt_Code desc", con)
                    dt1 = New DataTable
                    da1.Fill(dt1)
                    If dt1.Rows.Count > 0 Then
                        vPREVDOFFCODE = dt1.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
                        vPREVDOFFDATE = dt1.Rows(0).Item("Weaver_ClothReceipt_Date")
                        vPREVDOFSHFTIDNO = Val(dt1.Rows(0).Item("Doff_Shift_IdNo").ToString)
                        vPREVDOFSHFTMTRS = Val(dt1.Rows(0).Item("Doff_Shift_Meters").ToString)
                    End If
                    dt1.Clear()

                    cmd.Parameters.AddWithValue("@previousdoffdate", vPREVDOFFDATE)

                    vSHFTPRODMTRS = 0
                    cmd.CommandText = "Select a.* from LoomNo_Production_Details a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.LoomNo_Production_Date between @previousdoffdate and @EntryDate Order by a.LoomNo_Production_Date, a.for_OrderBy, a.LoomNo_Production_Code"
                    da1 = New SqlClient.SqlDataAdapter(cmd)
                    dt1 = New DataTable
                    da1.Fill(dt1)
                    If dt1.Rows.Count > 0 Then

                        For i = 0 To dt1.Rows.Count - 1

                            If DateDiff(DateInterval.Day, vPREVDOFFDATE, dt1.Rows(i).Item("LoomNo_Production_Date")) = 0 Then

                                If DateDiff(DateInterval.Day, vENTDOFFDATE, dt1.Rows(i).Item("LoomNo_Production_Date")) = 0 Then

                                    If vPREVDOFSHFTIDNO = vENTDOFSHFTIDNO Then

                                        vSHFTPRODMTRS = Format(Val(vSHFTPRODMTRS) + Val(vENTDOFSHFTMTRS) - Val(vPREVDOFSHFTMTRS), "##########0.00")

                                    Else

                                        If vPREVDOFSHFTIDNO = 7 Then
                                            vSHFTPRODMTRS = Format(Val(vSHFTPRODMTRS) + Val(dt1.Rows(i).Item("Shift2_Mtrs").ToString) - Val(vPREVDOFSHFTMTRS) + Val(vENTDOFSHFTMTRS), "##########0.00")
                                        Else
                                            vSHFTPRODMTRS = Format(Val(vSHFTPRODMTRS) + Val(dt1.Rows(i).Item("Shift1_Mtrs").ToString) - Val(vPREVDOFSHFTMTRS) + Val(vENTDOFSHFTMTRS), "##########0.00")
                                        End If

                                    End If

                                Else

                                    If vPREVDOFSHFTIDNO = 7 Then
                                        vSHFTPRODMTRS = Format(Val(vSHFTPRODMTRS) + Val(dt1.Rows(i).Item("Shift2_Mtrs").ToString) - Val(vPREVDOFSHFTMTRS), "##########0.00")
                                    Else
                                        vSHFTPRODMTRS = Format(Val(vSHFTPRODMTRS) + Val(dt1.Rows(i).Item("Shift1_Mtrs").ToString) - Val(vPREVDOFSHFTMTRS) + Val(dt1.Rows(i).Item("Shift2_Mtrs").ToString), "##########0.00")
                                    End If

                                End If

                            ElseIf DateDiff(DateInterval.Day, vENTDOFFDATE, dt1.Rows(i).Item("LoomNo_Production_Date")) = 0 Then

                                If vENTDOFSHFTIDNO = 7 Then
                                    vSHFTPRODMTRS = Format(Val(vSHFTPRODMTRS) + Val(dt1.Rows(i).Item("Shift1_Mtrs").ToString) + Val(vENTDOFSHFTMTRS), "##########0.00")
                                Else
                                    vSHFTPRODMTRS = Format(Val(vSHFTPRODMTRS) + Val(vENTDOFSHFTMTRS), "##########0.00")
                                End If

                            Else

                                vSHFTPRODMTRS = Format(Val(vSHFTPRODMTRS) + Val(dt1.Rows(i).Item("Shift1_Mtrs").ToString) + Val(dt1.Rows(i).Item("Shift2_Mtrs").ToString), "##########0.00")

                            End If

                        Next i

                    End If

                    dt1.Clear()

                End If

            End If

        End If

        txt_DoffMtrs.Text = Format(Val(vSHFTPRODMTRS), "##########0.00")

    End Sub

    Private Sub cbo_Weft_MillName_Enter(sender As Object, e As EventArgs) Handles cbo_Weft_MillName.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        cbo_Weft_MillName.Tag = cbo_Weft_MillName.Text
    End Sub

    Private Sub cbo_Weft_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weft_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, cbo_WeftLotNo, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            'If cbo_ClothName.Visible And cbo_ClothName.Enabled Then
            '    cbo_ClothName.Focus()
            'ElseIf cbo_Pcs_LastPiece_Status.Visible And cbo_Pcs_LastPiece_Status.Enabled Then
            '    cbo_Pcs_LastPiece_Status.Focus()
            'ElseIf cbo_Pcs_LastPiece_Status.Visible And cbo_Pcs_LastPiece_Status.Enabled Then
            '    cbo_Pcs_LastPiece_Status.Focus()
            'Else
            '    msk_Date.Focus()
            'End If
            If cbo_ClothSales_OrderNo.Visible And cbo_ClothSales_OrderNo.Enabled Then
                cbo_ClothSales_OrderNo.Focus()
            ElseIf cbo_Pcs_LastPiece_Status.Visible And cbo_Pcs_LastPiece_Status.Enabled Then
                cbo_Pcs_LastPiece_Status.Focus()
            ElseIf cbo_Pcs_LastPiece_Status.Visible And cbo_Pcs_LastPiece_Status.Enabled Then
                cbo_Pcs_LastPiece_Status.Focus()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Weft_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weft_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, cbo_WeftLotNo, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Weft_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weft_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = sender.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Weft_MillName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weft_MillName.LostFocus
        If Trim(UCase(cbo_Weft_MillName.Tag)) <> Trim(UCase(cbo_Weft_MillName.Text)) Then
            get_Fabric_LotNo()

        End If
    End Sub
    Private Sub get_Fabric_LotNo()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim EdsCnt_ID As Integer
        Dim WrpCnt_ID As Integer
        Dim WftCnt_ID As Integer
        Dim WrpMil_ID As Integer
        Dim WftMil_ID As Integer
        Dim vFABLOTNO As String

        vFABLOTNO = ""

        If Trim(lbl_EndsCount.Text) = "" Then GoTo GOTOLOOP1
        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount.Text)
        If Val(EdsCnt_ID) = 0 Then GoTo GOTOLOOP1

        If Trim(lbl_WeftCount.Text) = "" Then GoTo GOTOLOOP1
        WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)
        If Val(WftCnt_ID) = 0 Then GoTo GOTOLOOP1

        WrpCnt_ID = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ")")
        If Val(WrpCnt_ID) = 0 Then GoTo GOTOLOOP1

        If Trim(lbl_WarpMillName.Text) = "" Then GoTo GOTOLOOP1
        WrpMil_ID = Common_Procedures.Mill_NameToIdNo(con, lbl_WarpMillName.Text)
        If Val(WrpMil_ID) = 0 Then GoTo GOTOLOOP1

        If Trim(cbo_Weft_MillName.Text) = "" Then GoTo GOTOLOOP1
        WftMil_ID = Common_Procedures.Mill_NameToIdNo(con, cbo_Weft_MillName.Text)
        If Val(WftMil_ID) = 0 Then GoTo GOTOLOOP1

        vFABLOTNO = ""
        da = New SqlClient.SqlDataAdapter("select * from Fabric_LotNo_Head Where Warp_Count_IdNo = " & Str(Val(WrpCnt_ID)) & " and Warp_Mill_IdNo = " & Str(Val(WrpMil_ID)) & " and Warp_LotNo = '" & Trim(lbl_WarpLotNo.Text) & "' and Weft_Count_IdNo = " & Str(Val(WftCnt_ID)) & " and Weft_Mill_IdNo = " & Str(Val(WftMil_ID)) & " and Weft_LotNo = '" & Trim(cbo_WeftLotNo.Text) & "'", con)
        dt1 = New DataTable
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            vFABLOTNO = dt1.Rows(0).Item("Fabric_LotNo").ToString
        End If
        dt1.Clear()

GOTOLOOP1:
        lbl_FabricLotNo.Text = Trim(UCase(vFABLOTNO))

    End Sub

    Private Sub cbo_ClothSales_OrderNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderNo.KeyDown

        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim Lm_ID As Integer = 0
        'Dim Lm_ID2 As Integer = 0
        'Dim VLm_ID2 As Integer = 0

        'vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothSales_OrderNo, cbo_ClothName, cbo_Weft_MillName, "", "", "", "")

        'Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        'da = New SqlClient.SqlDataAdapter("select  a.loom_idno ,a.ClothSales_OrderCode_forSelection ,a.ClothSales_OrderCode_forSelection_Quality_2 from Beam_Knotting_Head a inner join  Weaver_Cloth_Receipt_Head b on a.loom_idno=b.loom_idno  left outer join  Weaver_Cloth_Receipt_Head c   on a.ClothSales_OrderCode_forSelection=c.ClothSales_OrderCode_forSelection left outer join  Weaver_Cloth_Receipt_Head d On a.ClothSales_OrderCode_forSelection_Quality_2 =d.Beam_Knotting_Code where A.Beam_Knotting_Code in (select Beam_Knotting_Code  from Beam_Knotting_Head  where Loom_IdNo = " & Str(Val(Lm_ID)) & " )  group by a.loom_idno ,a.ClothSales_OrderCode_forSelection ,a. ClothSales_OrderCode_forSelection_Quality_2 ", con)
        'dt = New DataTable
        'da.Fill(dt)


        'If dt.Rows.Count > 0 Then



        '    If Trim(dt.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_2").ToString) <> "" Then
        '        cbo_ClothSales_OrderNo.Items.Clear()
        '        cbo_ClothSales_OrderNo.Items.Add(dt.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString)
        '        cbo_ClothSales_OrderNo.Items.Add(dt.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_2").ToString)

        '    Else
        '        'cbo_ClothSales_OrderNo.Items.Clear()
        '        'cbo_ClothSales_OrderNo.Items.Add(dt.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString)

        '        cbo_ClothSales_OrderNo.Text = dt.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

        '    End If

        'End If


        'dt.Clear()

    End Sub

    Private Sub cbo_ClothSales_OrderNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderNo.KeyPress


        'If Asc(e.KeyChar) = 13 Then
        '    cbo_ClothSales_OrderNo.Tag = "----------------------"
        'End If


        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim Lm_ID As Integer = 0
        'Dim Lm_ID2 As Integer = 0
        'Dim VLm_ID2 As Integer = 0


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothSales_OrderNo, cbo_Weft_MillName, "", "", "", "", False)

        'Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        'da = New SqlClient.SqlDataAdapter("select  a.loom_idno ,a.ClothSales_OrderCode_forSelection ,a.ClothSales_OrderCode_forSelection_Quality_2 from Beam_Knotting_Head a inner join  Weaver_Cloth_Receipt_Head b on a.loom_idno=b.loom_idno  left outer join  Weaver_Cloth_Receipt_Head c   on a.ClothSales_OrderCode_forSelection=c.ClothSales_OrderCode_forSelection left outer join  Weaver_Cloth_Receipt_Head d On a.ClothSales_OrderCode_forSelection_Quality_2 =d.Beam_Knotting_Code where A.Beam_Knotting_Code in (select Beam_Knotting_Code  from Beam_Knotting_Head  where Loom_IdNo = " & Str(Val(Lm_ID)) & " )  group by a.loom_idno ,a.ClothSales_OrderCode_forSelection ,a. ClothSales_OrderCode_forSelection_Quality_2 ", con)
        'dt = New DataTable
        'da.Fill(dt)


        'If dt.Rows.Count > 0 Then


        '    If Trim(dt.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_2").ToString) <> "" Then
        '        cbo_ClothSales_OrderNo.Items.Clear()
        '        cbo_ClothSales_OrderNo.Items.Add(dt.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString)
        '        cbo_ClothSales_OrderNo.Items.Add(dt.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_2").ToString)

        '    Else
        '        'cbo_ClothSales_OrderNo.Items.Clear()
        '        'cbo_ClothSales_OrderNo.Items.Add(dt.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString)

        '        cbo_ClothSales_OrderNo.Text = dt.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

        '    End If

        'End If



        'dt.Clear()

    End Sub

    Private Sub cbo_ClothSales_OrderNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderNo.GotFocus


        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim Lm_ID As Integer = 0
        'Dim Lm_ID2 As Integer = 0
        'Dim VLm_ID2 As Integer = 0


        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

        'Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)



        'If cbo_ClothName.Text <> "" Then



        '    da = New SqlClient.SqlDataAdapter("select  a.loom_idno ,a.ClothSales_OrderCode_forSelection ,a.ClothSales_OrderCode_forSelection_Quality_2 from Beam_Knotting_Head a inner join  Weaver_Cloth_Receipt_Head b on a.loom_idno=b.loom_idno  left outer join  Weaver_Cloth_Receipt_Head c   on a.ClothSales_OrderCode_forSelection=c.ClothSales_OrderCode_forSelection left outer join  Weaver_Cloth_Receipt_Head d On a.ClothSales_OrderCode_forSelection_Quality_2 =d.Beam_Knotting_Code where A.Beam_Knotting_Code in (select Beam_Knotting_Code  from Beam_Knotting_Head  where Loom_IdNo = " & Str(Val(Lm_ID)) & " )  group by a.loom_idno ,a.ClothSales_OrderCode_forSelection ,a. ClothSales_OrderCode_forSelection_Quality_2 ", con)
        '    dt = New DataTable
        '    da.Fill(dt)


        '    If dt.Rows.Count > 0 Then



        '        If Trim(dt.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_2").ToString) <> "" Then
        '            cbo_ClothSales_OrderNo.Items.Clear()
        '            cbo_ClothSales_OrderNo.Items.Add(dt.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString)
        '            cbo_ClothSales_OrderNo.Items.Add(dt.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_2").ToString)

        '        Else
        '            'cbo_ClothSales_OrderNo.Items.Clear()
        '            'cbo_ClothSales_OrderNo.Items.Add(dt.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString)

        '            cbo_ClothSales_OrderNo.Text = dt.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

        '        End If

        '    End If
        'End If

        'dt.Clear()

        ''   Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Knotting_Head", "ClothSales_OrderCode_forSelection ", "(Loom_Idno =" & Str(Val(Lm_ID)) & " ) ", "(Loom_Idno=0)")
        'cbo_ClothSales_OrderNo.Tag = cbo_ClothSales_OrderNo.Text

    End Sub
    Private Sub Get_Quality_Panel_Selection()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim vCond As String = ""
        Dim vLm_Id As Integer = 0
        Dim VClo_ID As Integer = 0


        '   Try
        If Trim(cbo_PanelQuality.Text) <> "" Then

            If Trim(cbo_PanelQuality.Text) = "A" Then
                vCond = " Panel_1 = '" & Trim(cbo_PanelQuality.Text) & "' "
            ElseIf Trim(cbo_PanelQuality.Text) = "B" Then
                vCond = " Panel_2 = '" & Trim(cbo_PanelQuality.Text) & "' "
            ElseIf Trim(cbo_PanelQuality.Text) = "C" Then
                vCond = " Panel_3 = '" & Trim(cbo_PanelQuality.Text) & "' "
            ElseIf Trim(cbo_PanelQuality.Text) = "D" Then
                vCond = " Panel_4 = '" & Trim(cbo_PanelQuality.Text) & "' "
            End If


            If Trim(vCond) <> "" Then


                vLm_Id = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
                'VClo_ID = Common_Procedures.Cloth_IdNoToName(con, cbo_ClothName.Text)

                da = New SqlClient.SqlDataAdapter("Select Cloth_Idno1 ,Cloth_Idno2,Cloth_Idno3,Cloth_Idno4, ClothSales_OrderCode_forSelection ,ClothSales_OrderCode_forSelection_Quality_2,ClothSales_OrderCode_forSelection_Quality_3,ClothSales_OrderCode_forSelection_Quality_4, * from Beam_Knotting_Head a Where  " & Trim(vCond) & " and Loom_idno = " & Val(vLm_Id) & "  and beam_knotting_code ='" & Trim(lbl_KnotCode.Text) & "'  order by for_OrderBy desc , Beam_Knotting_Date desc ", con)
                dt = New DataTable
                da.Fill(dt)


                If dt.Rows.Count > 0 Then
                    'cbo_ClothName.Text = dt.Rows(0).Item("Cloth_Idno1").ToString
                    If Trim(cbo_PanelQuality.Text) = "A" Then
                        cbo_ClothName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt.Rows(0).Item("Cloth_idno1").ToString))
                        cbo_ClothSales_OrderNo.Text = dt.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString
                    End If
                    If Trim(cbo_PanelQuality.Text) = "B" Then
                        cbo_ClothName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt.Rows(0).Item("Cloth_idno2").ToString))
                        cbo_ClothSales_OrderNo.Text = dt.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_2").ToString
                    End If
                    If Trim(cbo_PanelQuality.Text) = "C" Then
                        cbo_ClothName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt.Rows(0).Item("Cloth_idno3").ToString))
                        cbo_ClothSales_OrderNo.Text = dt.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_3").ToString
                    End If
                    If Trim(cbo_PanelQuality.Text) = "D" Then
                        cbo_ClothName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt.Rows(0).Item("Cloth_idno4").ToString))
                        cbo_ClothSales_OrderNo.Text = dt.Rows(0).Item("ClothSales_OrderCode_forSelection_Quality_4").ToString
                    End If


                End If

            End If
        Else
            Exit Sub
        End If




        'Catch ex As Exception

        'End Try

    End Sub


    Private Sub cbo_PanelQuality_TextChanged(sender As Object, e As EventArgs) Handles cbo_PanelQuality.TextChanged

        'Dim Da As New SqlClient.SqlDataAdapter
        'Dim Dt1 As New DataTable
        'Dim vlm_id As String
        'vlm_id = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        'Da = New SqlClient.SqlDataAdapter("SELECT PANEL_1,PANEL_2,PANEL_3,PANEL_4 FROM BEAM_KNOTTING_HEAD where loom_idno='" & Val(vlm_id) & "' ", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)

        'If Dt1.Rows.Count > 0 Then

        '    cbo_PanelQuality.Items.Clear()
        '    cbo_PanelQuality.Items.Add("")

        '    cbo_PanelQuality.Items.Add(Dt1.Rows(0).Item("panel_1").ToString)
        '    cbo_PanelQuality.Items.Add(Dt1.Rows(0).Item("panel_2").ToString)
        '    cbo_PanelQuality.Items.Add(Dt1.Rows(0).Item("panel_3").ToString)
        '    cbo_PanelQuality.Items.Add(Dt1.Rows(0).Item("panel_4").ToString)

        'End If
        'Dt1.Clear()
        Get_Quality_Panel_Selection()

    End Sub
    Private Sub cbo_PanelQuality_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_PanelQuality.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PanelQuality, cbo_Pcs_LastPiece_Status, cbo_ClothName, "", "", "", "")

        Get_Quality_Panel_Selection()

    End Sub

    Private Sub cbo_PanelQuality_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_PanelQuality.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PanelQuality, cbo_ClothName, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then
            Get_Quality_Panel_Selection()
        End If

    End Sub

    Private Sub cbo_PanelQuality_GotFocus(sender As Object, e As EventArgs) Handles cbo_PanelQuality.GotFocus
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vlm_id As String

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

        vlm_id = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        Da = New SqlClient.SqlDataAdapter("SELECT panel_1,panel_2,panel_3,panel_4 FROM BEAM_KNOTTING_HEAD where loom_idno='" & Str(Val(vlm_id)) & "' AND beam_knotting_code ='" & Trim(lbl_KnotCode.Text) & "' ", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            cbo_PanelQuality.Items.Clear()
            cbo_PanelQuality.Items.Add("")
            cbo_PanelQuality.Items.Add(Dt1.Rows(0).Item("panel_1").ToString)
            cbo_PanelQuality.Items.Add(Dt1.Rows(0).Item("panel_2").ToString)
            cbo_PanelQuality.Items.Add(Dt1.Rows(0).Item("panel_3").ToString)
            cbo_PanelQuality.Items.Add(Dt1.Rows(0).Item("panel_4").ToString)

        End If
        Dt1.Clear()
        Get_Quality_Panel_Selection()

    End Sub

    Private Sub cbo_WeftLotNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_WeftLotNo.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
            Dim WftCnt_ID As Integer
            Dim WftMil_ID As Integer

            WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)
            If Val(WftCnt_ID) = 0 Then Exit Sub

            WftMil_ID = Common_Procedures.Mill_NameToIdNo(con, cbo_Weft_MillName.Text)
            If Val(WftMil_ID) = 0 Then Exit Sub

            vSql_Cond = "(count_idno = " & Str(Val(WftCnt_ID)) & " and Mill_Idno = " & Str(Val(WftMil_ID)) & ")"

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Lot_Head", "Lot_No", vSql_Cond, "(Entry_ReferenceCode = '')")

        Else
            Dim led_id As Integer
            Dim WftCnt_ID As Integer
            led_id = 0
            WftCnt_ID = 0
            If Trim(lbl_PartyName.Text) <> "" Then led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text)
            If Trim(lbl_WeftCount.Text) <> "" Then WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "JobWork_yarn_Receipt_Details", "Weft_Lot_No", "(Ledger_idno = " & Str(Val(led_id)) & " and count_idno = " & Str(Val(WftCnt_ID)) & "   and Po_No = '" & Trim(lbl_PoNo.Text) & "')", "(JobWork_PavuYarn_Receipt_Code = '')")
        End If
    End Sub

    Private Sub cbo_WeftLotNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_WeftLotNo.KeyDown
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_ClothName, txt_DoffMtrs, "Yarn_Lot_Head", "Lot_No", vSql_Cond, "(Entry_ReferenceCode = '')")
        Else
            Dim led_id As Integer
            Dim WftCnt_ID As Integer
            led_id = 0
            WftCnt_ID = 0
            If Trim(lbl_PartyName.Text) <> "" Then led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text)
            If Trim(lbl_WeftCount.Text) <> "" Then WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_ClothName, txt_DoffMtrs, "JobWork_yarn_Receipt_Details", "Weft_Lot_No", "(Ledger_idno = " & Str(Val(led_id)) & " and count_idno = " & Str(Val(WftCnt_ID)) & "  and Po_No = '" & Trim(lbl_PoNo.Text) & "')", "(JobWork_PavuYarn_Receipt_Code = '')")
        End If
    End Sub

    Private Sub cbo_WeftLotNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_WeftLotNo.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_DoffMtrs, "Yarn_Lot_Head", "Lot_No", vSql_Cond, "(Entry_ReferenceCode = '')")
        Else
            Dim led_id As Integer
            Dim WftCnt_ID As Integer
            led_id = 0
            WftCnt_ID = 0
            If Trim(lbl_PartyName.Text) <> "" Then led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text)
            If Trim(lbl_WeftCount.Text) <> "" Then WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_DoffMtrs, "JobWork_yarn_Receipt_Details", "Weft_Lot_No", "(Ledger_idno = " & Str(Val(led_id)) & " and count_idno = " & Str(Val(WftCnt_ID)) & "   and Po_No = '" & Trim(lbl_PoNo.Text) & "')", "(JobWork_PavuYarn_Receipt_Code = '')")
        End If
    End Sub
    Private Sub btn_Save_SalesOrderNo_Click(sender As Object, e As EventArgs) Handles btn_Save_SalesOrderNo.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim vSHFTIDNO As Integer
        Dim Pwd As String = ""
        Dim g As New Password

        g.ShowDialog()

        Pwd = Common_Procedures.Password_Input

        If Trim(Pwd) <> "GOLDMOF123" Then
            MessageBox.Show("Incorrect Password!...", "DOESNOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        cmd.Connection = con

        ' --- cloth Stock

        cmd.CommandText = " Update a set a.ClothSales_OrderCode_forSelection = b.ClothSales_OrderCode_forSelection from Stock_Cloth_Processing_Details a  " &
                          " INNER JOIN Weaver_Cloth_Receipt_Head b on 'PCDOF-'+ b.Weaver_ClothReceipt_Code = a.Reference_Code where b.ClothSales_OrderCode_forSelection <> '' "
        cmd.ExecuteNonQuery()

        ' --- Yarn Stock

        cmd.CommandText = " Update a set a.ClothSales_OrderCode_forSelection = b.ClothSales_OrderCode_forSelection from Stock_Yarn_Processing_Details a  " &
                          " INNER Join Weaver_Cloth_Receipt_Head b On 'PCDOF-'+ b.Weaver_ClothReceipt_Code = a.Reference_Code where b.ClothSales_OrderCode_forSelection <> '' "
        cmd.ExecuteNonQuery()

        ' --- Pavu Stock

        cmd.CommandText = " Update a set a.ClothSales_OrderCode_forSelection = b.ClothSales_OrderCode_forSelection from Stock_Pavu_Processing_Details a  " &
                          " INNER Join Weaver_Cloth_Receipt_Head b On 'PCDOF-'+ b.Weaver_ClothReceipt_Code = a.Reference_Code where b.ClothSales_OrderCode_forSelection <> '' "
        cmd.ExecuteNonQuery()


        MessageBox.Show("Sales Order Nos are Updated Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        move_record(lbl_RollNo.Text)

    End Sub

    Private Sub btn_Show_WeftConsumption_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Show_WeftConsumption_Details.Click
        pnl_back.Enabled = False
        pnl_Weft_Consumption_Details.Visible = True
        If dgv_Weft_Consumption_Details.Rows.Count > 0 Then
            dgv_Weft_Consumption_Details.Focus()
            dgv_Weft_Consumption_Details.CurrentCell = dgv_Weft_Consumption_Details.Rows(0).Cells(0)
        End If
    End Sub

    Private Sub btn_Close_Weft_Consumption_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Weft_Consumption_Details.Click
        pnl_back.Enabled = True
        pnl_Weft_Consumption_Details.Visible = False
    End Sub

    Private Sub get_Multiple_WeftYarn_Consumption_Count_Details(ByVal CloID As Integer)
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim n, I As Integer
        Dim vMULTIWEFTSTS As String = 0

        dgv_Weft_Consumption_Details.Rows.Clear()

        If Common_Procedures.settings.Cloth_WeftConsumption_Multiple_WeftCount_Status = 1 Then '---- SOTEXPA QUALIDIS TEXTILE (SULUR)

            If CloID = 0 Then
                If Trim(cbo_ClothName.Text) <> "" Then
                    CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
                End If
            End If

            vMULTIWEFTSTS = Common_Procedures.get_FieldValue(con, "cloth_Head", "Multiple_WeftCount_Status", "(cloth_idno = " & Str(Val(CloID)) & ")")
            If Val(vMULTIWEFTSTS) = 1 Then


                Da4 = New SqlClient.SqlDataAdapter("Select a.*, b.count_name from Cloth_Additional_Weft_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo Where Cloth_Idno = " & Str(Val(CloID)), con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    For I = 0 To Dt4.Rows.Count - 1

                        n = dgv_Weft_Consumption_Details.Rows.Add()
                        dgv_Weft_Consumption_Details.Rows(n).Cells(0).Value = Dt4.Rows(I).Item("count_name").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(1).Value = Dt4.Rows(I).Item("Gram_Perc_Type").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(2).Value = Val(Dt4.Rows(I).Item("Consumption_Gram_Perc").ToString)
                        dgv_Weft_Consumption_Details.Rows(n).Cells(3).Value = ""

                    Next

                End If
                Dt4.Clear()

            End If

        End If

    End Sub

    Private Sub chk_Show_All_Knottings_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Show_All_Knottings.CheckedChanged
        btn_ShowKnottingDetails_Click(sender, e)
    End Sub
End Class