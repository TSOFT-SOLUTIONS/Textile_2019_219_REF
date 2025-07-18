Public Class Doffing_Entry_Format3
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Other_Condition As String = ""
    Private Pk_Condition As String = "PDOFF-"
    Private PkCondition2_WCLRC As String = "WCLRC-"
    Private PkCondition3_PCDOF As String = "PCDOF-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private Save_Status As Boolean = False
    Private vNewly_Added_PcsNo As String = ""

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private Type1, Type2, Type3, Type4, Type5 As String
    Private Type11, Type22, Type33, Type44, Type55 As String
    Private vType1, vType2, vType3, vType4, vType5 As Single
    Private vTotType1, vTotType2, vTotType3, vTotType4, vTotType5 As Single

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False
        Save_Status = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        pnl_KnottingSelection.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        msk_date.Text = ""
        dtp_Date.Text = ""
        lbl_RecDate.Text = ""
        cbo_Weaver.Text = ""
        cbo_Pcs_ClothName.Text = ""

        lbl_ExcessShort.Text = ""
        txt_Folding.Text = "100"
        lbl_PDcNo.Text = ""
        lbl_Noofpcs.Text = ""
        lbl_RecMeter.Text = ""
        lbl_RecNo.Text = ""

        txt_Pcs_No.Text = ""
        txt_Pcs_No.Tag = ""
        txt_Pcs_RecMtrs.Text = ""
        txt_Pcs_Pick.Text = ""
        txt_Pcs_Width.Text = ""
        txt_Pcs_Type1Mtrs.Text = ""
        txt_Pcs_Type2Mtrs.Text = ""
        txt_Pcs_Type3Mtrs.Text = ""
        txt_Pcs_Type4Mtrs.Text = ""
        txt_Pcs_Type5Mtrs.Text = ""
        lbl_Pcs_TotalMtrs.Text = ""
        txt_Pcs_Weight.Text = ""
        lbl_Pcs_Wgt_Mtr.Text = ""
        cbo_Pcs_LoomNo.Text = ""
        cbo_Pcs_LoomNo.Tag = ""
        cbo_Pcs_LastPiece_Status.Text = "NO"
        cbo_Pcs_LastPiece_Status.Tag = ""
        lbl_Pcs_WidthType.Text = ""
        lbl_Pcs_KnotCode.Text = ""
        lbl_Pcs_KnotNo.Text = ""
        lbl_Pcs_SetNo1.Text = ""
        lbl_Pcs_SetNo2.Text = ""
        lbl_Pcs_BeamNo1.Text = ""
        lbl_Pcs_BeamNo2.Text = ""
        lbl_Pcs_Beam_TotMtrs1.Text = ""
        lbl_Pcs_Beam_BalMtrs1.Text = ""
        lbl_Pcs_PackSlipNo1.Text = ""
        lbl_Pcs_PackSlipNo2.Text = ""
        lbl_Pcs_PackSlipNo3.Text = ""
        lbl_Pcs_PackSlipNo4.Text = ""
        lbl_Pcs_PackSlipNo5.Text = ""
        lbl_Pcs_EndsCount.Text = ""
        lbl_Pcs_WeftCount.Text = ""

        cbo_Pcs_LoomNo.Enabled = True
        cbo_Pcs_ClothName.Enabled = True
        txt_Pcs_RecMtrs.Enabled = True

        dgv_Details.Rows.Clear()
        dgv_Details_Total1.Rows.Clear()
        dgv_Details_Total1.Rows.Add()

        dgv_Details.Rows.Clear()
        dgv_Details_Total2.Rows.Clear()
        dgv_Details_Total2.Rows.Add()
        dgv_Details_Total2.Rows(0).Cells(0).Value = "100%"
        dgv_Details_Total2.Rows(0).Cells(1).Value = "FOLDING"

        dgv_KnottingSelection.Rows.Clear()

        cbo_Weaver.Enabled = True
        cbo_Weaver.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White

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
        NoCalc_Status = False
    End Sub

    Private Sub Clear_PcsDetails()
        txt_Pcs_No.Text = ""
        txt_Pcs_No.Tag = ""
        txt_Pcs_RecMtrs.Text = ""
        txt_Pcs_Pick.Text = ""
        txt_Pcs_Width.Text = ""
        txt_Pcs_Type1Mtrs.Text = ""
        txt_Pcs_Type2Mtrs.Text = ""
        txt_Pcs_Type3Mtrs.Text = ""
        txt_Pcs_Type4Mtrs.Text = ""
        txt_Pcs_Type5Mtrs.Text = ""
        lbl_Pcs_TotalMtrs.Text = ""
        txt_Pcs_Weight.Text = ""
        lbl_Pcs_Wgt_Mtr.Text = ""
        cbo_Pcs_LoomNo.Text = ""
        cbo_Pcs_LoomNo.Tag = ""
        cbo_Pcs_LastPiece_Status.Text = "NO"
        cbo_Pcs_LastPiece_Status.Tag = ""
        lbl_Pcs_WidthType.Text = ""
        lbl_Pcs_KnotCode.Text = ""
        lbl_Pcs_KnotNo.Text = ""
        lbl_Pcs_SetNo1.Text = ""
        lbl_Pcs_SetNo2.Text = ""
        lbl_Pcs_BeamNo1.Text = ""
        lbl_Pcs_BeamNo2.Text = ""
        lbl_Pcs_Beam_TotMtrs1.Text = ""
        lbl_Pcs_Beam_BalMtrs1.Text = ""
        lbl_Pcs_PackSlipNo1.Text = ""
        lbl_Pcs_PackSlipNo2.Text = ""
        lbl_Pcs_PackSlipNo3.Text = ""
        lbl_Pcs_PackSlipNo4.Text = ""
        lbl_Pcs_PackSlipNo5.Text = ""

        cbo_Pcs_ClothName.Text = ""
        lbl_Pcs_WeftCount.Text = ""
        lbl_Pcs_EndsCount.Text = ""

        cbo_Pcs_LoomNo.Enabled = True
        cbo_Pcs_ClothName.Enabled = True
        txt_Pcs_RecMtrs.Enabled = True

        txt_Pcs_Type1Mtrs.Enabled = True
        txt_Pcs_Type2Mtrs.Enabled = True
        txt_Pcs_Type3Mtrs.Enabled = True
        txt_Pcs_Type4Mtrs.Enabled = True
        txt_Pcs_Type5Mtrs.Enabled = True

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_Cell_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(2, 57, 111)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If

    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Blue
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
        If Not IsNothing(dgv_Details_Total1.CurrentCell) Then dgv_Details_Total1.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total2.CurrentCell) Then dgv_Details_Total2.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Doffing_Entry_Format3_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Weaver.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Weaver.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Pcs_LoomNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Pcs_LoomNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Doffing_Entry_Format3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = ""

        lbl_LotNoHeading.Text = StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase)


        lbl_Type1_Heading.Text = StrConv(Common_Procedures.ClothType.Type1, vbProperCase)
        lbl_Type2_Heading.Text = StrConv(Common_Procedures.ClothType.Type2, vbProperCase)
        lbl_Type3_Heading.Text = StrConv(Common_Procedures.ClothType.Type3, vbProperCase)
        lbl_Type4_Heading.Text = StrConv(Common_Procedures.ClothType.Type4, vbProperCase)
        lbl_Type5_Heading.Text = StrConv(Common_Procedures.ClothType.Type5, vbProperCase)

        dgv_Details.Columns(5).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1))
        dgv_Details.Columns(6).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2))
        dgv_Details.Columns(7).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3))
        dgv_Details.Columns(8).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4))
        dgv_Details.Columns(9).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5))


        cbo_Pcs_LastPiece_Status.Items.Clear()
        cbo_Pcs_LastPiece_Status.Items.Add("YES")
        cbo_Pcs_LastPiece_Status.Items.Add("NO")

        con.Open()

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_Weaver, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()


        pnl_KnottingSelection.Visible = False
        pnl_KnottingSelection.Left = (Me.Width - pnl_KnottingSelection.Width) \ 2
        pnl_KnottingSelection.Top = (Me.Height - pnl_KnottingSelection.Height) \ 2
        pnl_KnottingSelection.BringToFront()




        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaver.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs_RecMtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs_Pick.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Pcs_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs_Type1Mtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs_Type2Mtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs_Type3Mtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs_Type4Mtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs_Type5Mtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Pcs_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Pcs_LastPiece_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Add.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Delete.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Clear.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weaver.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs_RecMtrs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs_Pick.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Pcs_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs_Type1Mtrs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs_Type2Mtrs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs_Type3Mtrs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs_Type4Mtrs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs_Type5Mtrs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Pcs_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Pcs_LastPiece_Status.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Delete.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Clear.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Folding.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs_No.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Pcs_RecMtrs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs_Pick.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs_Width.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs_Type1Mtrs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs_Type2Mtrs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs_Type3Mtrs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs_Type4Mtrs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs_Type5Mtrs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Folding.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Pcs_No.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Pcs_RecMtrs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs_Pick.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs_Width.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs_Type1Mtrs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs_Type2Mtrs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs_Type3Mtrs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs_Type4Mtrs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs_Type5Mtrs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0
        Other_Condition = "(Receipt_Type = 'AL')"

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Doffing_Entry_Format3_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Doffing_Entry_Format3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub



                ElseIf pnl_KnottingSelection.Visible = True Then
                    btn_Close_KnottingSelection_Click(sender, e)
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

    Private Sub move_record(ByVal no As String)
        Dim cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim i As Integer = 0, j As Integer = 0, k As Integer = 0
        Dim m As Integer = 0, n As Integer = 0
        Dim SNo As Integer = 0
        Dim LockSTS As Boolean = False
        Dim Clo_Pck As Single = 0, Clo_Wdth As Single = 0
        Dim PcsFrmNo As Integer = 0, PcsToNo As Integer = 0
        Dim SQL1 As String


        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("Select a.*, b.Ledger_Name, c.Cloth_Name, d.EndsCount_Name, e.Count_Name, f.Loom_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Count_Head e ON a.Count_IdNo = e.Count_IdNo LEFT OUTER JOIN Loom_Head f ON a.Loom_IdNo = f.Loom_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_No").ToString

                dtp_Date.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_Weaver.Text = dt1.Rows(0).Item("Ledger_Name").ToString

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                cmd.Connection = con
                'cmd.CommandType = CommandType.StoredProcedure
                'cmd.CommandText = "sp_get_weaverclothreceiptpiecedetails_for_moving2"
                'cmd.Parameters.Clear()
                'cmd.Parameters.Add("@weaver_clothreceipt_code", SqlDbType.VarChar)
                'cmd.Parameters("@weaver_clothreceipt_code").Value = Trim(NewCode)
                'da2 = New SqlClient.SqlDataAdapter(cmd)

                'Da3 = New SqlClient.SqlDataAdapter("select top 1 a.*, b.cloth_name, d.endscount_Name, e.count_name as weft_countname from Beam_Knotting_Head a  INNER JOIN Cloth_Head b ON a.Cloth_Idno1 = b.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN count_Head e ON b.Cloth_WeftCount_IdNo = e.Count_IdNo Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = '' Order by a.Beam_Knotting_Date desc, a.for_OrderBy desc, a.Beam_Knotting_Code desc", con)


                SQL1 = "Select a.*, b.Loom_Name, c.cloth_name, d.endscount_Name, e.count_name as weft_countname from Weaver_ClothReceipt_Piece_Details a INNER JOIN Loom_Head b ON a.Loom_IdNo = b.Loom_IdNo INNER JOIN Beam_Knotting_Head tBH ON tBH.Beam_Knotting_Code = a.Beam_Knotting_Code INNER JOIN Cloth_Head c ON a.Cloth_Idno = c.Cloth_IdNo INNER JOIN EndsCount_Head d ON d.EndsCount_IdNo = tBH.EndsCount_IdNo INNER JOIN count_Head e ON c.Cloth_WeftCount_IdNo = e.Count_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Lot_Code = '" & Trim(NewCode) & "' Order by a.PieceNo_OrderBy, a.Sl_No, a.Piece_No"
                cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                da2 = New SqlClient.SqlDataAdapter(cmd)
                'da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Loom_Name from Weaver_ClothReceipt_Piece_Details a LEFT OUTER JOIN Loom_Head b ON a.Loom_IdNo = b.Loom_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "' Order by a.PieceNo_OrderBy, a.Sl_No, a.Piece_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            .Rows(n).Cells(0).Value = dt2.Rows(i).Item("Piece_No").ToString
                            If Val(dt2.Rows(i).Item("Receipt_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(1).Value = Format(Val(dt2.Rows(i).Item("Receipt_Meters").ToString), "########0.00")
                            End If
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Loom_Name").ToString
                            If Val(dt2.Rows(i).Item("Pick").ToString) <> 0 Then
                                .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Pick").ToString)
                            End If
                            If Val(dt2.Rows(i).Item("Width").ToString) <> 0 Then
                                .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Width").ToString)
                            End If
                            If Val(dt2.Rows(i).Item("Type1_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Type2_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Type2_Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Type3_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Type3_Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Type4_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Type4_Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Type5_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Type5_Meters").ToString), "########0.00")
                            End If

                            .Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString), "########0.00")
                            If Val(dt2.Rows(i).Item("Weight").ToString) <> 0 Then
                                .Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            End If
                            If Val(dt2.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                                .Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")
                            End If

                            .Rows(n).Cells(13).Value = dt2.Rows(i).Item("PackingSlip_Code_Type1").ToString
                            If Trim(.Rows(n).Cells(13).Value) = "" Then .Rows(n).Cells(13).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type1").ToString
                            .Rows(n).Cells(14).Value = dt2.Rows(i).Item("PackingSlip_Code_Type2").ToString
                            If Trim(.Rows(n).Cells(14).Value) = "" Then .Rows(n).Cells(14).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type2").ToString
                            .Rows(n).Cells(15).Value = dt2.Rows(i).Item("PackingSlip_Code_Type3").ToString
                            If Trim(.Rows(n).Cells(15).Value) = "" Then .Rows(n).Cells(15).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type3").ToString
                            .Rows(n).Cells(16).Value = dt2.Rows(i).Item("PackingSlip_Code_Type4").ToString
                            If Trim(.Rows(n).Cells(16).Value) = "" Then .Rows(n).Cells(16).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type4").ToString
                            .Rows(n).Cells(17).Value = dt2.Rows(i).Item("PackingSlip_Code_Type5").ToString
                            If Trim(.Rows(n).Cells(17).Value) = "" Then .Rows(n).Cells(17).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type5").ToString

                            .Rows(n).Cells(18).Value = dt2.Rows(i).Item("Beam_Knotting_Code").ToString
                            .Rows(n).Cells(19).Value = dt2.Rows(i).Item("Beam_Knotting_No").ToString
                            .Rows(n).Cells(20).Value = dt2.Rows(i).Item("Set_Code1").ToString
                            .Rows(n).Cells(21).Value = dt2.Rows(i).Item("Set_No1").ToString
                            .Rows(n).Cells(22).Value = dt2.Rows(i).Item("Beam_No1").ToString
                            .Rows(n).Cells(23).Value = ""
                            If Trim(.Rows(n).Cells(20).Value) <> "" And Trim(.Rows(n).Cells(22).Value) <> "" Then
                                da1 = New SqlClient.SqlDataAdapter("Select Meters from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(.Rows(n).Cells(20).Value) & "' and Beam_No = '" & Trim(.Rows(n).Cells(22).Value) & "'", con)
                                dt3 = New DataTable
                                da1.Fill(dt3)
                                If dt3.Rows.Count > 0 Then
                                    .Rows(n).Cells(23).Value = dt3.Rows(0).Item("Meters").ToString
                                End If
                                dt3.Clear()
                            End If

                            .Rows(n).Cells(24).Value = dt2.Rows(i).Item("Set_Code2").ToString
                            .Rows(n).Cells(25).Value = dt2.Rows(i).Item("Set_No2").ToString
                            .Rows(n).Cells(26).Value = dt2.Rows(i).Item("Beam_No2").ToString
                            .Rows(n).Cells(27).Value = ""
                            If Val(dt2.Rows(i).Item("Balance_Meters1").ToString) <> 0 Then
                                .Rows(n).Cells(27).Value = Format(Val(dt2.Rows(i).Item("Balance_Meters1").ToString), "#########0.00")
                            End If

                            .Rows(n).Cells(28).Value = dt2.Rows(i).Item("Width_Type").ToString
                            .Rows(n).Cells(29).Value = Format(Val(dt2.Rows(i).Item("Crimp_Percentage").ToString), "#########0.00")
                            .Rows(n).Cells(30).Value = Format(Val(dt2.Rows(i).Item("BeamConsumption_Meters").ToString), "#########0.00")
                            .Rows(n).Cells(31).Value = dt2.Rows(i).Item("Is_LastPiece").ToString

                            .Rows(n).Cells(32).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                            .Rows(n).Cells(33).Value = dt2.Rows(i).Item("endscount_Name").ToString
                            .Rows(n).Cells(34).Value = dt2.Rows(i).Item("weft_countname").ToString

                            If Val(.Rows(n).Cells(5).Value) <> 0 Or Trim(.Rows(n).Cells(13).Value) <> "" Then
                                .Rows(n).Cells(5).Style.ForeColor = Color.Red
                                For j = 0 To .Columns.Count - 1
                                    .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If

                            If Val(.Rows(n).Cells(6).Value) <> 0 Or Trim(.Rows(n).Cells(14).Value) <> "" Then
                                .Rows(n).Cells(6).Style.ForeColor = Color.Red
                                For j = 0 To .Columns.Count - 1
                                    .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If

                            If Val(.Rows(n).Cells(7).Value) <> 0 Or Trim(.Rows(n).Cells(15).Value) <> "" Then
                                .Rows(n).Cells(7).Style.ForeColor = Color.Red
                                For j = 0 To .Columns.Count - 1
                                    .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If

                            If Val(.Rows(n).Cells(8).Value) <> 0 Or Trim(.Rows(n).Cells(16).Value) <> "" Then
                                .Rows(n).Cells(8).Style.ForeColor = Color.Red
                                For j = 0 To .Columns.Count - 1
                                    .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If

                            If Val(.Rows(n).Cells(9).Value) <> 0 Or Trim(.Rows(n).Cells(17).Value) <> "" Then
                                .Rows(n).Cells(9).Style.ForeColor = Color.Red
                                For j = 0 To .Columns.Count - 1
                                    .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If

                        Next i

                    End If


                    '                    PcsFrmNo = 0
                    '                    PcsToNo = 0
                    '                    da2 = New SqlClient.SqlDataAdapter("Select * from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                    '                    dt2 = New DataTable
                    '                    da2.Fill(dt2)
                    '                    If dt2.Rows.Count > 0 Then
                    '                        PcsFrmNo = Val(dt2.Rows(0).Item("pcs_fromno").ToString)
                    '                        PcsToNo = Val(dt2.Rows(0).Item("pcs_tono").ToString)
                    '                    End If
                    '                    dt2.Clear()


                    '                    Clo_Pck = 0
                    '                    Clo_Wdth = 0
                    '                    da2 = New SqlClient.SqlDataAdapter("Select * from Cloth_Head Where Cloth_IdNo = " & Str(Val(dt1.Rows(0).Item("Cloth_IdNo").ToString)), con)
                    '                    dt2 = New DataTable
                    '                    da2.Fill(dt2)
                    '                    If dt2.Rows.Count > 0 Then
                    '                        Clo_Pck = Val(dt2.Rows(0).Item("Cloth_Pick").ToString)
                    '                        Clo_Wdth = Val(dt2.Rows(0).Item("Cloth_Width").ToString)
                    '                    End If
                    '                    dt2.Clear()


                    '                    For k = Val(PcsFrmNo) To (Val(PcsFrmNo) + Val(lbl_Noofpcs.Text) - 1)

                    '                        For m = 0 To dgv_Details.Rows.Count - 1
                    '                            If k = Val(dgv_Details.Rows(m).Cells(0).Value) Then
                    '                                GoTo LOOOP1
                    '                            End If
                    '                        Next

                    '                        For j = 0 To dgv_Details.Rows.Count - 1
                    '                            If k < Val(dgv_Details.Rows(j).Cells(0).Value) Then
                    '                                dgv_Details.Rows.Insert(j, 1)
                    '                                dgv_Details.Rows(j).Cells(0).Value = k
                    '                                If Val(Clo_Pck) <> 0 Then
                    '                                    dgv_Details.Rows(j).Cells(3).Value = Val(Clo_Pck)
                    '                                End If
                    '                                If Val(Clo_Wdth) <> 0 Then
                    '                                    dgv_Details.Rows(j).Cells(4).Value = Val(Clo_Wdth)
                    '                                End If
                    '                                GoTo LOOOP1
                    '                            End If
                    '                        Next

                    '                        n = dgv_Details.Rows.Add()
                    '                        dgv_Details.Rows(n).Cells(0).Value = k
                    '                        If Val(Clo_Pck) <> 0 Then
                    '                            dgv_Details.Rows(n).Cells(3).Value = Val(Clo_Pck)
                    '                        End If
                    '                        If Val(Clo_Wdth) <> 0 Then
                    '                            dgv_Details.Rows(n).Cells(4).Value = Val(Clo_Wdth)
                    '                        End If
                    'LOOOP1:
                    '                    Next k

                    If .RowCount = 0 Then .Rows.Add()

                    For k = 0 To .Rows.Count - 1
                        If Trim(UCase(.Rows(k).Cells(0).Value)) = Trim(UCase(vNewly_Added_PcsNo)) Then
                            If k >= 8 Then
                                .FirstDisplayedScrollingRowIndex = k
                            End If
                            Exit For
                        End If
                    Next

                End With

                NoCalc_Status = False
                Calculation_Totals()
                NoCalc_Status = True
                'With dgv_Details_Total1
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(1).Value = Format(Val(dt1.Rows(0).Item("Total_Checking_Receipt_Meters").ToString), "########0.00")
                '    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Type1_Meters").ToString), "########0.00")
                '    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Type2_Meters").ToString), "########0.00")
                '    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Type3_Meters").ToString), "########0.00")
                '    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Type4_Meters").ToString), "########0.00")
                '    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_Type5_Meters").ToString), "########0.00")
                '    .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_Checking_Meters").ToString), "########0.00")
                '    .Rows(0).Cells(11).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                'End With

                'With dgv_Details_Total2
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(0).Value = "100%"
                '    .Rows(0).Cells(1).Value = "FOLDING"
                '    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Type1Meters_100Folding").ToString), "########0.00")
                '    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Type2Meters_100Folding").ToString), "########0.00")
                '    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Type3Meters_100Folding").ToString), "########0.00")
                '    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Type4Meters_100Folding").ToString), "########0.00")
                '    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_Type5Meters_100Folding").ToString), "########0.00")
                '    .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_Meters_100Folding").ToString), "########0.00")

                'End With

                da2 = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                        If Trim(dt2.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Then
                            LockSTS = True
                        End If
                    End If
                End If
                dt1.Clear()

                If LockSTS = True Then

                    cbo_Weaver.Enabled = False
                    cbo_Weaver.BackColor = Color.LightGray

                    txt_Folding.Enabled = False
                    txt_Folding.BackColor = Color.LightGray

                End If

            Else

                new_record()

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            dt3.Dispose()

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

        End Try

        vNewly_Added_PcsNo = ""
        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim I As Integer = 0
        Dim vOrdByNo As String = ""
        Dim SQL1 As String

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Inhouse_Doffing_Entry, New_Entry, Me, con, "Weaver_Cloth_Receipt_Head", "Weaver_Cloth_Receipt_Code", NewCode, "Weaver_Cloth_Receipt_Date", "(Weaver_Cloth_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

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

        cmd.Connection = con

        SQL1 = "select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and (Type1_Meters <> 0 or Type2_Meters <> 0 or Type3_Meters <> 0 or Type4_Meters <> 0 or Type5_Meters <> 0 )"
        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Already Piece checking prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        SQL1 = "select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '')"
        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Packing Slip prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Then
                    MessageBox.Show("Weaver Wages prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If
            End If
        End If
        Dt1.Clear()


        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Weaver_Piece_Checking_Code, Company_IdNo, for_OrderBy", trans)

            'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Weaver_ClothReceipt_Piece_Details", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Piece_No,Main_PieceNo,PieceNo_OrderBy,ReceiptMeters_Checking,Receipt_Meters,Loom_No,Is_LastPiece,Pick,Width,Type1_Meters,Type2_Meters,Type3_Meters,Type4_Meters,Type5_Meters,Total_Checking_Meters,Weight,Weight_Meter,Beam_Knotting_Code,Beam_Knotting_No,Loom_IdNo, Width_Type,Crimp_Percentage,Set_Code1,Set_No1,Beam_No1,Balance_Meters1,Set_Code2,Set_No2,Beam_No2,Balance_Meters2,BeamConsumption_Meters", "Sl_No", "Weaver_Piece_Checking_Code, For_OrderBy, Company_IdNo, Weaver_Piece_Checking_No, Weaver_Piece_Checking_Date, Ledger_Idno", trans)

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            SQL1 = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "'"
            cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            'cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Create_Status = 0"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
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

        Exit Sub

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_ClothReceipt_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            vNewly_Added_PcsNo = ""
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_ClothReceipt_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            vNewly_Added_PcsNo = ""
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Weaver_ClothReceipt_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_ClothReceipt_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            vNewly_Added_PcsNo = ""
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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_ClothReceipt_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            vNewly_Added_PcsNo = ""
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
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head ", "Weaver_ClothReceipt_Code", "for_orderby", "(Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red

            vNewly_Added_PcsNo = ""

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            vNewly_Added_PcsNo = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim NewCode As String = ""

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Inhouse_Doffing_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW CHK NO. INSERTION...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
            Dt = New DataTable
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
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim cmd2 As New SqlClient.SqlCommand
        Dim cmd3 As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim LotCd As String = ""
        Dim LotNo As String = ""
        Dim OrdByNo As String = 0
        Dim vOrdBy_PCSNo As String = 0
        Dim clth_ID As Integer = 0
        Dim EdsCnt_ID As Integer
        Dim WftCnt_ID As Integer
        Dim MasWftCnt_IDNo As Integer

        Dim Wev_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim NoofInpBmsInLom As Integer = 0

        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""

        Dim vTot_PCS As Integer
        Dim vTot_RecMtrs As Single

        Dim vTot_Typ1Mtrs As Single
        Dim vTot_Typ2Mtrs As Single
        Dim vTot_Typ3Mtrs As Single
        Dim vTot_Typ5Mtrs As Single
        Dim vTot_Typ4Mtrs As Single
        Dim vTot_ChkMtrs As Single
        Dim vTot_Wgt As Single

        Dim vTot_100Fld_Typ1Mtrs As Single
        Dim vTot_100Fld_Typ2Mtrs As Single
        Dim vTot_100Fld_Typ3Mtrs As Single
        Dim vTot_100Fld_Typ4Mtrs As Single
        Dim vTot_100Fld_Typ5Mtrs As Single
        Dim vTot_100Fld_ChkMtr As Single

        Dim I As Integer = 0, J As Integer = 0, K As Integer = 0, n As Integer = 0
        Dim Nr As Integer = 0

        Dim WagesCode As String = ""
        Dim vPCSCODE_FORSELECTION As String
        Dim vMAINPCSNO As String

        Dim ConsYarn As Single = 0
        Dim ConsPavu As Single = 0
        Dim Grd_UpSts As Boolean = False

        Dim Old_Loom_Idno As Integer
        Dim Old_SetCd1 As String, Old_Beam1 As String
        Dim Old_SetCd2 As String, Old_Beam2 As String
        Dim stkof_idno As Integer = 0
        Dim vGod_ID As Integer = 4
        Dim Led_type As String = 0

        'Dim New_Edit_Status As Boolean = False
        Dim vOrdBy_CHKNO As String = ""
        Dim vOrdBy_RECNO As String = ""
        Dim SQL1 As String = ""
        Dim vErrMsg As String = ""
        Dim vSELC_LOTCODE As String

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        vOrdBy_CHKNO = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Inhouse_Doffing_Entry, New_Entry, Me, con, "Weaver_Cloth_Receipt_Head", "Weaver_Cloth_Receipt_Code", NewCode, "Weaver_Cloth_Receipt_Date", "(Weaver_Cloth_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Cloth_Receipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Cloth_Receipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        Save_Status = False

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
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

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then
            txt_Folding.Text = 100
        End If

        Wev_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        If Wev_ID = 0 Then
            MessageBox.Show("Invalid Weaver Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
            Exit Sub
        End If

        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Pcs_ClothName.Text)
        If clth_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
            Exit Sub
        End If



        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_Pcs_EndsCount.Text)
        If Val(EdsCnt_ID) = 0 Then
            MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Pcs_ClothName.Enabled Then cbo_Pcs_ClothName.Focus()
            Exit Sub
        End If

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_EndsCount_Details Where Cloth_Idno = " & Str(Val(clth_ID)) & " and EndsCount_IdNo = " & Str(Val(EdsCnt_ID)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count = 0 Then
            MessageBox.Show("Mismatch of EndsCount with Cloth Master", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Pcs_ClothName.Enabled Then cbo_Pcs_ClothName.Focus()
            Exit Sub
        End If
        Dt1.Clear()


        WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_Pcs_WeftCount.Text)
        If Val(WftCnt_ID) = 0 Then
            MessageBox.Show("Invalid Weft Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Pcs_ClothName.Enabled Then cbo_Pcs_ClothName.Focus()
            Exit Sub
        End If

        MasWftCnt_IDNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_IdNo = " & Str(Val(clth_ID)) & ")")
        If Val(WftCnt_ID) <> Val(MasWftCnt_IDNo) Then
            MessageBox.Show("Mismatch of Weft Count with Cloth Master", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Pcs_ClothName.Enabled Then cbo_Pcs_ClothName.Focus()
            Exit Sub
        End If

        'If Val(lbl_RecCode.Text) = 0 Then
        '    MessageBox.Show("Invalid Lot No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
        '    Exit Sub
        'End If

        If Val(txt_Pcs_No.Text) = 0 And Trim(txt_Pcs_No.Text) = "0" Then
            MessageBox.Show("Invalid Piece No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Pcs_No.Enabled Then txt_Pcs_No.Focus()
            Exit Sub
        End If

        If Trim(UCase(txt_Pcs_No.Text)) <> Trim(UCase(txt_Pcs_No.Tag)) Then
            MessageBox.Show("Invalid Piece No and its Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Pcs_No.Enabled Then txt_Pcs_No.Focus()
            Exit Sub
        End If


        If Val(txt_Pcs_RecMtrs.Text) = 0 Then
            MessageBox.Show("Invalid Piece Receipt Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Pcs_RecMtrs.Enabled Then txt_Pcs_RecMtrs.Focus()
            Exit Sub
        End If


        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_Pcs_LoomNo.Text)

        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
            Exit Sub
        End If
        'If Trim(lbl_Pcs_WidthType.Text) = "" Then
        '    MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
        '    Exit Sub
        'End If


        If Trim(cbo_Pcs_LastPiece_Status.Text) = "" Then
            cbo_Pcs_LastPiece_Status.Text = "NO"
        ElseIf Trim(cbo_Pcs_LastPiece_Status.Text) <> "YES" And Trim(cbo_Pcs_LastPiece_Status.Text) <> "NO" Then
            cbo_Pcs_LastPiece_Status.Text = "NO"
        End If


        'If Val(txt_Pcs_RecMtrs.Text) = 0 Then
        '    If Val(Lm_ID) <> 0 Then
        '        MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
        '        Exit Sub
        '    End If
        'End If

        If Val(Lm_ID) <> 0 Then

            NoofInpBmsInLom = Common_Procedures.get_FieldValue(con, "Loom_Head", "Noof_Input_Beams", "(Loom_IdNo = " & Str(Val(Lm_ID)) & ")")
            If Val(NoofInpBmsInLom) = 0 Then NoofInpBmsInLom = 1

            If NoofInpBmsInLom = 1 Then
                If Trim(lbl_Pcs_BeamNo1.Text) = "" And Trim(lbl_Pcs_BeamNo2.Text) = "" Then
                    MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
                    Exit Sub
                End If

                If Trim(lbl_Pcs_BeamNo1.Text) <> "" And Trim(lbl_Pcs_BeamNo2.Text) <> "" Then
                    MessageBox.Show("Invalid Beams, Select Only One Beam for this Loom", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
                    Exit Sub
                End If

                'If Val(lbl_Pcs_Beam_BalMtrs1.Text) <= 0 Then
                '    MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                '    If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
                '    Exit Sub
                'End If

                'If Val(lbl_Pcs_Beam_TotMtrs1.Text) <> 0 Then
                '    If Val(lbl_Pcs_Beam_BalMtrs1.Text) < Val(txt_Pcs_RecMtrs.Text) Then
                '        MessageBox.Show("Invalid Beam Meters, Lesser than Receipt Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                '        If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
                '        Exit Sub
                '    End If
                'End If

            Else

                If Trim(lbl_Pcs_BeamNo1.Text) = "" Or Trim(lbl_Pcs_BeamNo2.Text) = "" Then
                    MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
                    Exit Sub
                End If

                'If Val(lbl_Pcs_Beam_BalMtrs1.Text) <= 0 Then
                '    MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                '    If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
                '    Exit Sub
                'End If

                'If Val(lbl_Pcs_Beam_TotMtrs1.Text) <> 0 Then
                '    If Val(lbl_Pcs_Beam_BalMtrs1.Text) < Val(txt_Pcs_RecMtrs.Text) Then
                '        MessageBox.Show("Invalid Beam Meters, Lesser than Receipt Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                '        If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
                '        Exit Sub
                '    End If
                'End If

            End If

        End If


        If Val(Lm_ID) = 0 Then

            lbl_Pcs_KnotCode.Text = ""
            lbl_Pcs_KnotNo.Text = ""

            lbl_Pcs_WidthType.Text = ""
            lbl_Pcs_CrimpPerc.Text = ""

            lbl_Pcs_SetCode1.Text = ""
            lbl_Pcs_SetNo1.Text = ""
            lbl_Pcs_BeamNo1.Text = ""

            lbl_Pcs_SetCode2.Text = ""
            lbl_Pcs_SetNo2.Text = ""
            lbl_Pcs_BeamNo2.Text = ""

            lbl_Pcs_Beam_TotMtrs1.Text = ""
            lbl_Pcs_Beam_BalMtrs1.Text = ""

            lbl_Pcs_BeamConsMtrs.Text = ""

        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        cmd.Connection = con

        'New_Edit_Status = True

        'cmd3.Connection = con

        'cmd3.CommandType = CommandType.StoredProcedure
        'cmd3.CommandText = "sp_get_Weaver_ClothReceipt_Piece_Details_by_LotCode_ClothReceiptCode_PieceNo"
        'cmd3.Parameters.Clear()
        'cmd3.Parameters.Add("@Lot_Code", SqlDbType.VarChar)
        'cmd3.Parameters("@Lot_Code").Value = Trim(NewCode)
        'cmd3.Parameters.Add("@Weaver_ClothReceipt_Code", SqlDbType.VarChar)
        'cmd3.Parameters("@Weaver_ClothReceipt_Code").Value = Trim(NewCode)
        'cmd3.Parameters.Add("@Piece_No", SqlDbType.VarChar)
        'cmd3.Parameters("@Piece_No").Value = Trim(txt_Pcs_No.Text)
        'Da = New SqlClient.SqlDataAdapter(cmd3)
        ''SQL1 = "Select Type1_Meters, Type2_Meters, Type3_Meters, Type4_Meters, Type5_Meters from Weaver_ClothReceipt_Piece_Details a Where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "'  and a.Piece_No = '" & Trim(txt_Pcs_No.Text) & "'"
        ''cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        ''Da = New SqlClient.SqlDataAdapter(cmd)
        ''Da = New SqlClient.SqlDataAdapter("Select * from Weaver_ClothReceipt_Piece_Details a Where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "'  and a.Piece_No = '" & Trim(txt_Pcs_No.Text) & "' Order by a.sl_no", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)
        'If Dt1.Rows.Count > 0 Then
        '    If Val(Dt1.Rows(0).Item("Type1_Meters").ToString) <> 0 Or Val(Dt1.Rows(0).Item("Type2_Meters").ToString) <> 0 Or Val(Dt1.Rows(0).Item("Type3_Meters").ToString) <> 0 Or Val(Dt1.Rows(0).Item("Type4_Meters").ToString) <> 0 Or Val(Dt1.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
        '        New_Edit_Status = False
        '    End If
        'End If
        'Dt1.Clear()

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Piece_Checking_Entry, New_Edit_Status) = False Then Exit Sub

        NoCalc_Status = False
        Calculation_Totals()
        Calculation_Beam_ConsumptionPavu()

        vTot_PCS = 0
        vTot_RecMtrs = 0 : vTot_Typ1Mtrs = 0 : vTot_Typ2Mtrs = 0 : vTot_Typ3Mtrs = 0 : vTot_Typ5Mtrs = 0 : vTot_Typ4Mtrs = 0 : vTot_ChkMtrs = 0 : vTot_Wgt = 0

        With dgv_Details_Total1
            If .RowCount > 0 Then
                vTot_PCS = Val(.Rows(0).Cells(0).Value())
                vTot_RecMtrs = Val(.Rows(0).Cells(1).Value())
                vTot_Typ1Mtrs = Val(.Rows(0).Cells(5).Value())
                vTot_Typ2Mtrs = Val(.Rows(0).Cells(6).Value())
                vTot_Typ3Mtrs = Val(.Rows(0).Cells(7).Value())
                vTot_Typ4Mtrs = Val(.Rows(0).Cells(8).Value())
                vTot_Typ5Mtrs = Val(.Rows(0).Cells(9).Value())
                vTot_ChkMtrs = Val(.Rows(0).Cells(10).Value())
                vTot_Wgt = Val(.Rows(0).Cells(11).Value())
            End If

        End With

        vTot_100Fld_Typ1Mtrs = 0 : vTot_100Fld_Typ2Mtrs = 0 : vTot_100Fld_Typ3Mtrs = 0 : vTot_100Fld_Typ4Mtrs = 0 : vTot_100Fld_Typ5Mtrs = 0 : vTot_100Fld_ChkMtr = 0
        'With dgv_Details_Total2
        '    If .RowCount > 0 Then

        '        vTot_100Fld_Typ1Mtrs = Val(.Rows(0).Cells(5).Value())
        '        vTot_100Fld_Typ2Mtrs = Val(.Rows(0).Cells(6).Value())
        '        vTot_100Fld_Typ3Mtrs = Val(.Rows(0).Cells(7).Value())
        '        vTot_100Fld_Typ4Mtrs = Val(.Rows(0).Cells(8).Value())
        '        vTot_100Fld_Typ5Mtrs = Val(.Rows(0).Cells(9).Value())
        '        vTot_100Fld_ChkMtr = Val(.Rows(0).Cells(10).Value())

        '    End If
        'End With

        WagesCode = ""
        Da = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
            End If
        End If
        Dt1.Clear()

        Old_Loom_Idno = 0
        Old_SetCd1 = ""
        Old_Beam1 = ""
        Old_SetCd2 = ""
        Old_Beam2 = ""

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head ", "Weaver_ClothReceipt_Code", "for_orderby", "(Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@entrydate", Convert.ToDateTime(msk_date.Text))

            OrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))
            vOrdBy_RECNO = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)

            vSELC_LOTCODE = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag)) & "/" & Trim(Pk_Condition)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Cloth_Receipt_Head ( Receipt_Type, Weaver_ClothReceipt_Code,           Company_IdNo      ,        Weaver_ClothReceipt_No ,     for_OrderBy     , Weaver_ClothReceipt_Date,    Ledger_IdNo     , Loom_IdNo,  Width_Type ,  Beam_Knotting_Code ,  Beam_Knotting_No ,     Cloth_Idno      ,       EndsCount_Idno  ,     Count_IdNo        ,  Beam_No1,  Set_Code1,  Set_No1, Balance_Meters1 ,  Beam_No2 ,  Set_Code2 ,  Set_No2 , Balance_Meters2,         Folding_Receipt           ,           Folding                 ,      Total_Receipt_Pcs    ,           noof_pcs        ,       ReceiptMeters_Receipt      ,           Receipt_Meters         ,       Total_Receipt_Meters       , ConsumedYarn_Receipt, Consumed_Yarn, ConsumedPavu_Receipt , Consumed_Pavu , BeamConsumption_Receipt, BeamConsumption_Meters, Crimp_Percentage , Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment   ,             user_idNo                   ,  Bar_Code,    lotcode_forSelection       ) " &
                                    "            Values                   (      'L'   ,  '" & Trim(NewCode) & "', " & Val(lbl_Company.Tag) & ", '" & Trim(lbl_RefNo.Text) & "', " & Val(OrdByNo) & ",         @entrydate      , " & Val(Wev_ID) & ",    0     ,     ''      ,         ''          ,      ''           , " & Val(clth_ID) & ", " & Val(EdsCnt_ID) & ", " & Val(WftCnt_ID) & ",     ''   ,     ''    ,     ''  ,     0           ,     ''    ,     ''     ,     ''   ,        0       , " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(vTot_PCS)) & ", " & Str(Val(vTot_PCS)) & ", " & Val(txt_Pcs_RecMtrs.Text) & ", " & Val(txt_Pcs_RecMtrs.Text) & ", " & Val(txt_Pcs_RecMtrs.Text) & ",       0             ,      0       ,      0               ,      0        ,           0            ,          0            ,           0      ,               ''          ,             0                     , " & Val(Common_Procedures.User.IdNo) & ",      ''  , '" & Trim(vSELC_LOTCODE) & "' ) "
                cmd.ExecuteNonQuery()


            Else

                'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdBy_CHKNO), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Piece_Checking_Code, Company_IdNo, for_OrderBy", tr)

                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Weaver_ClothReceipt_Piece_Details", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdBy_CHKNO), Pk_Condition, "", "", New_Entry, False, "Piece_No,Main_PieceNo,PieceNo_OrderBy,ReceiptMeters_Checking,Receipt_Meters,Loom_No,Is_LastPiece,Pick,Width,Type1_Meters,Type2_Meters,Type3_Meters,Type4_Meters,Type5_Meters,Total_Checking_Meters,Weight,Weight_Meter,Beam_Knotting_Code,Beam_Knotting_No,Loom_IdNo, Width_Type,Crimp_Percentage,Set_Code1,Set_No1,Beam_No1,Balance_Meters1,Set_Code2,Set_No2,Beam_No2,Balance_Meters2,BeamConsumption_Meters", "Sl_No", "Weaver_Piece_Checking_Code, For_OrderBy, Company_IdNo, Weaver_Piece_Checking_No, Weaver_Piece_Checking_Date, Ledger_Idno", tr)

                'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdBy_CHKNO), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Piece_Checking_Code, Company_IdNo, for_OrderBy", tr)

                '------ HEAD Updation
                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Receipt_Type = 'L', Weaver_ClothReceipt_Date = @entrydate, Ledger_IdNo = " & Str(Val(Wev_ID)) & ", Total_Receipt_Pcs = " & Str(Val(vTot_PCS)) & ", noof_pcs = " & Str(Val(vTot_PCS)) & ", ReceiptMeters_Receipt = " & Str(Val(vTot_RecMtrs)) & ", Total_Receipt_Meters = " & Str(Val(vTot_RecMtrs)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If


            ''--------
            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            SQL1 = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "'  and Piece_No = '" & Trim(txt_Pcs_No.Text) & "' and Weaver_Piece_Checking_Code = '' and (Type1_Meters+Type2_Meters+Type3_Meters+Type4_Meters+Type5_Meters) = 0  and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = ''"
            cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            'cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "'  and Piece_No = '" & Trim(txt_Pcs_No.Text) & "' and Weaver_Piece_Checking_Code = '' and (Type1_Meters+Type2_Meters+Type3_Meters+Type4_Meters+Type5_Meters) = 0  and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = ''"
            cmd.ExecuteNonQuery()

            Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Wev_ID)) & ")", , tr)

            stkof_idno = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                stkof_idno = Wev_ID
            Else
                stkof_idno = Val(Common_Procedures.CommonLedger.Godown_Ac)
            End If

            vMAINPCSNO = Trim(txt_Pcs_No.Text)

            vOrdBy_PCSNo = Val(Common_Procedures.OrderBy_CodeToValue(vMAINPCSNO))

            vPCSCODE_FORSELECTION = Trim(UCase(txt_Pcs_No.Text)) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag)) & "/" & Trim(Pk_Condition)

            Nr = 0
            SQL1 = "Update Weaver_ClothReceipt_Piece_Details set Weaver_ClothReceipt_date = '" & Trim(Format(dtp_Date.Value, "MM/dd/yyyy")) & "', Ledger_IdNo = " & Str(Val(Wev_ID)) & ", Cloth_IdNo = " & Str(Val(clth_ID)) & ", PieceCode_for_Selection = '" & Trim(vPCSCODE_FORSELECTION) & "' , Main_PieceNo = '" & Trim(Val(vMAINPCSNO)) & "' , PieceNo_OrderBy = " & Str(Val(vOrdBy_PCSNo)) & ", ReceiptMeters_Receipt = " & Val(txt_Pcs_RecMtrs.Text) & ", Loom_No = '" & Trim(cbo_Pcs_LoomNo.Text) & "', Create_Status = 1, StockOff_IdNo = " & Str(Val(stkof_idno)) & ", WareHouse_IdNo = " & Str(Val(vGod_ID)) & " , Beam_Knotting_Code = '" & Trim(lbl_Pcs_KnotCode.Text) & "', Beam_Knotting_No = '" & Trim(lbl_Pcs_KnotNo.Text) & "', Loom_IdNo = " & Str(Val(Lm_ID)) & ", Width_Type = '" & Trim(lbl_Pcs_WidthType.Text) & "', Crimp_Percentage = " & Str(Val(lbl_Pcs_CrimpPerc.Text)) & ", Set_Code1 = '" & Trim(lbl_Pcs_SetCode1.Text) & "', Set_No1 = '" & Trim(lbl_Pcs_SetNo1.Text) & "', Beam_No1 = '" & Trim(lbl_Pcs_BeamNo1.Text) & "', Balance_Meters1 = " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", Set_Code2 = '" & Trim(lbl_Pcs_SetCode2.Text) & "', Set_No2 = '" & Trim(lbl_Pcs_SetNo2.Text) & "', Beam_No2 = '" & Trim(lbl_Pcs_BeamNo2.Text) & "', Balance_Meters2 = " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", BeamConsumption_Meters = " & Str(Val(lbl_Pcs_BeamConsMtrs.Text)) & ", EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Count_IdNo = " & Str(Val(WftCnt_ID)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and Piece_No = '" & Trim(txt_Pcs_No.Text) & "'"
            cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            'cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Weaver_ClothReceipt_date = @EntryDate, Ledger_IdNo = " & Str(Val(Wev_ID)) & ", Cloth_IdNo = " & Str(Val(clth_ID)) & ", PieceCode_for_Selection = '" & Trim(vPCSCODE_FORSELECTION) & "' , Main_PieceNo = '" & Trim(Val(vMAINPCSNO)) & "' , PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vMAINPCSNO))) & ", ReceiptMeters_Receipt = " & Val(txt_Pcs_RecMtrs.Text) & ", Loom_No = '" & Trim(cbo_Pcs_LoomNo.Text) & "', Create_Status = 1, StockOff_IdNo = " & Str(Val(stkof_idno)) & ", WareHouse_IdNo = " & Str(Val(vGod_ID)) & " , Beam_Knotting_Code = '" & Trim(lbl_Pcs_KnotCode.Text) & "', Beam_Knotting_No = '" & Trim(lbl_Pcs_KnotNo.Text) & "', Loom_IdNo = " & Str(Val(Lm_ID)) & ", Width_Type = '" & Trim(lbl_Pcs_WidthType.Text) & "', Crimp_Percentage = " & Str(Val(lbl_Pcs_CrimpPerc.Text)) & ", Set_Code1 = '" & Trim(lbl_Pcs_SetCode1.Text) & "', Set_No1 = '" & Trim(lbl_Pcs_SetNo1.Text) & "', Beam_No1 = '" & Trim(lbl_Pcs_BeamNo1.Text) & "', Balance_Meters1 = " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", Set_Code2 = '" & Trim(lbl_Pcs_SetCode2.Text) & "', Set_No2 = '" & Trim(lbl_Pcs_SetNo2.Text) & "', Beam_No2 = '" & Trim(lbl_Pcs_BeamNo2.Text) & "', Balance_Meters2 = " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", BeamConsumption_Meters = " & Str(Val(lbl_Pcs_BeamConsMtrs.Text)) & ", EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Count_IdNo = " & Str(Val(WftCnt_ID)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and Piece_No = '" & Trim(txt_Pcs_No.Text) & "'"
            Nr = cmd.ExecuteNonQuery()

            If Nr = 0 Then

                SQL1 = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_ClothReceipt_Code ,            Company_IdNo          ,      Weaver_ClothReceipt_No   ,           for_OrderBy     ,            Weaver_ClothReceipt_date                 ,           Lot_Code      ,             Lot_No            ,     Ledger_IdNo         ,            Cloth_IdNo    ,            Folding_Receipt    ,             Folding          ,         Sl_No        ,                     Piece_No          ,                  Main_PieceNo  ,         PieceCode_for_Selection       ,         PieceNo_OrderBy       ,     ReceiptMeters_Receipt        ,                Receipt_Meters    ,                    Loom_No         , Create_Status ,              StockOff_IdNo  ,          WareHouse_IdNo    ,                Beam_Knotting_Code    ,           Beam_Knotting_No         ,         Loom_IdNo      ,           Width_Type                  ,             Crimp_Percentage            ,              Set_Code1               ,                  Set_No1           ,                 Beam_No1            ,                   Balance_Meters1           ,                  Set_Code2           ,                   Set_No2          ,                   Beam_No2          ,                    Balance_Meters2          ,                    BeamConsumption_Meters   ,         EndsCount_IdNo     ,             Count_IdNo      , Weaver_Piece_Checking_Code, Weaver_Piece_Checking_No, Weaver_Piece_Checking_Date, Type1_Meters, Type2_Meters, Type3_Meters, Type4_Meters, Type5_Meters ,  PackingSlip_Code_Type1, PackingSlip_Code_Type2, PackingSlip_Code_Type3, PackingSlip_Code_Type4, PackingSlip_Code_Type5, BuyerOffer_Code_Type1, BuyerOffer_Code_Type2, BuyerOffer_Code_Type3, BuyerOffer_Code_Type4, BuyerOffer_Code_Type5 ) "
                SQL1 = SQL1 & "            Values                     ('" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(OrdByNo)) & " , '" & Trim(Format(dtp_Date.Value, "MM/dd/yyyy")) & "',  '" & Trim(NewCode) & "', '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Wev_ID)) & ", " & Str(Val(clth_ID)) & ", " & Val(txt_Folding.Text) & " , " & Val(txt_Folding.Text) & ", " & Str(Val(Sno)) & ", '" & Trim(UCase(txt_Pcs_No.Text)) & "', '" & Trim(Val(vMAINPCSNO)) & "', '" & Trim(vPCSCODE_FORSELECTION) & "' , " & Str(Val(vOrdBy_PCSNo)) & ", " & Val(txt_Pcs_RecMtrs.Text) & ", " & Val(txt_Pcs_RecMtrs.Text) & ", '" & Trim(cbo_Pcs_LoomNo.Text) & "',       1       , " & Str(Val(stkof_idno)) & ", " & Str(Val(vGod_ID)) & "  , '" & Trim(lbl_Pcs_KnotCode.Text) & "', '" & Trim(lbl_Pcs_KnotNo.Text) & "', " & Str(Val(Lm_ID)) & ", '" & Trim(lbl_Pcs_WidthType.Text) & "', " & Str(Val(lbl_Pcs_CrimpPerc.Text)) & ", '" & Trim(lbl_Pcs_SetCode1.Text) & "', '" & Trim(lbl_Pcs_SetNo1.Text) & "', '" & Trim(lbl_Pcs_BeamNo1.Text) & "', " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", '" & Trim(lbl_Pcs_SetCode2.Text) & "', '" & Trim(lbl_Pcs_SetNo2.Text) & "', '" & Trim(lbl_Pcs_BeamNo2.Text) & "', " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", " & Str(Val(lbl_Pcs_BeamConsMtrs.Text)) & " , " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(WftCnt_ID)) & " ,          ''               ,            ''           ,        NUll               ,        0    ,       0     ,        0    ,       0     ,       0      ,            ''          ,            ''         ,            ''         ,            ''         ,            ''         ,            ''         ,            ''       ,            ''        ,            ''        ,            ''         )"
                cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                'cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_ClothReceipt_Code ,            Company_IdNo          ,      Weaver_ClothReceipt_No   ,           for_OrderBy     , Weaver_ClothReceipt_date,           Lot_Code      ,             Lot_No            ,     Ledger_IdNo         ,            Cloth_IdNo    ,            Folding_Receipt    ,             Folding          ,         Sl_No        ,                     Piece_No          ,                  Main_PieceNo  ,         PieceCode_for_Selection       ,                               PieceNo_OrderBy                      ,     ReceiptMeters_Receipt        ,                Receipt_Meters    ,                    Loom_No         , Create_Status ,              StockOff_IdNo  ,          WareHouse_IdNo    ,                Beam_Knotting_Code    ,           Beam_Knotting_No         ,         Loom_IdNo      ,           Width_Type                  ,             Crimp_Percentage            ,              Set_Code1               ,                  Set_No1           ,                 Beam_No1            ,                   Balance_Meters1           ,                  Set_Code2           ,                   Set_No2          ,                   Beam_No2          ,                    Balance_Meters2          ,                    BeamConsumption_Meters   ,         EndsCount_IdNo     ,             Count_IdNo      , Weaver_Piece_Checking_Code, Weaver_Piece_Checking_No, Weaver_Piece_Checking_Date, Type1_Meters, Type2_Meters, Type3_Meters, Type4_Meters, Type5_Meters ,  PackingSlip_Code_Type1, PackingSlip_Code_Type2, PackingSlip_Code_Type3, PackingSlip_Code_Type4, PackingSlip_Code_Type5, BuyerOffer_Code_Type1, BuyerOffer_Code_Type2, BuyerOffer_Code_Type3, BuyerOffer_Code_Type4, BuyerOffer_Code_Type5 ) " &
                '                    "           Values                           ('" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(OrdByNo)) & " ,          @entrydate     ,  '" & Trim(NewCode) & "', '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Wev_ID)) & ", " & Str(Val(clth_ID)) & ", " & Val(txt_Folding.Text) & " , " & Val(txt_Folding.Text) & ", " & Str(Val(Sno)) & ", '" & Trim(UCase(txt_Pcs_No.Text)) & "', '" & Trim(Val(vMAINPCSNO)) & "', '" & Trim(vPCSCODE_FORSELECTION) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vMAINPCSNO))) & ", " & Val(txt_Pcs_RecMtrs.Text) & ", " & Val(txt_Pcs_RecMtrs.Text) & ", '" & Trim(cbo_Pcs_LoomNo.Text) & "',       1       , " & Str(Val(stkof_idno)) & ", " & Str(Val(vGod_ID)) & "  , '" & Trim(lbl_Pcs_KnotCode.Text) & "', '" & Trim(lbl_Pcs_KnotNo.Text) & "', " & Str(Val(Lm_ID)) & ", '" & Trim(lbl_Pcs_WidthType.Text) & "', " & Str(Val(lbl_Pcs_CrimpPerc.Text)) & ", '" & Trim(lbl_Pcs_SetCode1.Text) & "', '" & Trim(lbl_Pcs_SetNo1.Text) & "', '" & Trim(lbl_Pcs_BeamNo1.Text) & "', " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", '" & Trim(lbl_Pcs_SetCode2.Text) & "', '" & Trim(lbl_Pcs_SetNo2.Text) & "', '" & Trim(lbl_Pcs_BeamNo2.Text) & "', " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", " & Str(Val(lbl_Pcs_BeamConsMtrs.Text)) & " , " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(WftCnt_ID)) & " ,          ''               ,            ''           ,        NUll               ,        0    ,       0     ,        0    ,       0     ,       0      ,            ''          ,            ''         ,            ''         ,            ''         ,            ''         ,            ''         ,            ''       ,            ''        ,            ''        ,            ''         ) "
                cmd.ExecuteNonQuery()

            End If

            'cmd2.Connection = con
            'cmd2.Transaction = tr
            'cmd2.CommandType = CommandType.StoredProcedure
            'cmd2.CommandText = "sp_save_Weaver_ClothReceipt_Piece_Details_update_and_insert"
            'cmd2.Parameters.Clear()
            'cmd2.Parameters.Add("@Weaver_Piece_Checking_Code", SqlDbType.VarChar)
            'cmd2.Parameters("@Weaver_Piece_Checking_Code").Value = Trim(NewCode)
            'cmd2.Parameters.Add("@Company_IdNo", SqlDbType.Int)
            'cmd2.Parameters("@Company_IdNo").Value = Val(lbl_Company.Tag)
            'cmd2.Parameters.Add("@Weaver_Piece_Checking_No", SqlDbType.VarChar)
            'cmd2.Parameters("@Weaver_Piece_Checking_No").Value = Trim(lbl_RefNo.Text)
            'cmd2.Parameters.Add("@Weaver_Piece_Checking_Date", SqlDbType.DateTime)
            'cmd2.Parameters("@Weaver_Piece_Checking_Date").Value = Convert.ToDateTime(msk_date.Text)
            'cmd2.Parameters.Add("@Ledger_Idno", SqlDbType.Int)
            'cmd2.Parameters("@Ledger_Idno").Value = Wev_ID
            'cmd2.Parameters.Add("@StockOff_IdNo", SqlDbType.Int)
            'cmd2.Parameters("@StockOff_IdNo").Value = stkof_idno
            'cmd2.Parameters.Add("@Weaver_ClothReceipt_Code", SqlDbType.VarChar)
            'cmd2.Parameters("@Weaver_ClothReceipt_Code").Value = Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text)
            'cmd2.Parameters.Add("@Weaver_ClothReceipt_No", SqlDbType.VarChar)
            'cmd2.Parameters("@Weaver_ClothReceipt_No").Value = Trim(lbl_RecNo.Text)
            'cmd2.Parameters.Add("@for_orderby", SqlDbType.Decimal)
            'cmd2.Parameters("@for_orderby").Value = Val(vOrdBy_RECNO)
            'cmd2.Parameters.Add("@Weaver_ClothReceipt_Date", SqlDbType.DateTime)
            'cmd2.Parameters("@Weaver_ClothReceipt_Date").Value = CDate(lbl_RecDate.Text)
            'cmd2.Parameters.Add("@Lot_Code", SqlDbType.VarChar)
            'cmd2.Parameters("@Lot_Code").Value = Trim(LotCd)
            'cmd2.Parameters.Add("@Lot_No", SqlDbType.VarChar)
            'cmd2.Parameters("@Lot_No").Value = Trim(LotNo)
            'cmd2.Parameters.Add("@Cloth_IdNo", SqlDbType.Int)
            'cmd2.Parameters("@Cloth_IdNo").Value = clth_ID
            'cmd2.Parameters.Add("@Folding_Checking", SqlDbType.Decimal)
            'cmd2.Parameters("@Folding_Checking").Value = Val(txt_Folding.Text)
            'cmd2.Parameters.Add("@Folding", SqlDbType.Decimal)
            'cmd2.Parameters("@Folding").Value = Val(txt_Folding.Text)
            'cmd2.Parameters.Add("@Sl_No", SqlDbType.Int)
            'cmd2.Parameters("@Sl_No").Value = Val(txt_Pcs_No.Text)
            'cmd2.Parameters.Add("@Piece_No", SqlDbType.VarChar)
            'cmd2.Parameters("@Piece_No").Value = Trim(txt_Pcs_No.Text)
            'cmd2.Parameters.Add("@Main_PieceNo", SqlDbType.VarChar)
            'cmd2.Parameters("@Main_PieceNo").Value = Trim(Val(txt_Pcs_No.Text))
            'cmd2.Parameters.Add("@PieceNo_OrderBy", SqlDbType.Decimal)
            'cmd2.Parameters("@PieceNo_OrderBy").Value = Val(Common_Procedures.OrderBy_CodeToValue(Trim(txt_Pcs_No.Text)))
            'cmd2.Parameters.Add("@ReceiptMeters_Checking", SqlDbType.Decimal)
            'cmd2.Parameters("@ReceiptMeters_Checking").Value = Val(txt_Pcs_RecMtrs.Text)
            'cmd2.Parameters.Add("@Receipt_Meters", SqlDbType.Decimal)
            'cmd2.Parameters("@Receipt_Meters").Value = Val(txt_Pcs_RecMtrs.Text)
            'cmd2.Parameters.Add("@Loom_No", SqlDbType.VarChar)
            'cmd2.Parameters("@Loom_No").Value = Trim(cbo_Pcs_LoomNo.Text)
            'cmd2.Parameters.Add("@Is_LastPiece", SqlDbType.VarChar)
            'cmd2.Parameters("@Is_LastPiece").Value = Trim(UCase(cbo_Pcs_LastPiece_Status.Text))
            'cmd2.Parameters.Add("@Pick", SqlDbType.Decimal)
            'cmd2.Parameters("@Pick").Value = Val(txt_Pcs_Pick.Text)
            'cmd2.Parameters.Add("@Width", SqlDbType.Decimal)
            'cmd2.Parameters("@Width").Value = Val(txt_Pcs_Width.Text)
            'cmd2.Parameters.Add("@Type1_Meters", SqlDbType.Decimal)
            'cmd2.Parameters("@Type1_Meters").Value = Val(txt_Pcs_Type1Mtrs.Text)
            'cmd2.Parameters.Add("@Type2_Meters", SqlDbType.Decimal)
            'cmd2.Parameters("@Type2_Meters").Value = Val(txt_Pcs_Type2Mtrs.Text)
            'cmd2.Parameters.Add("@Type3_Meters", SqlDbType.Decimal)
            'cmd2.Parameters("@Type3_Meters").Value = Val(txt_Pcs_Type3Mtrs.Text)
            'cmd2.Parameters.Add("@Type4_Meters", SqlDbType.Decimal)
            'cmd2.Parameters("@Type4_Meters").Value = Val(txt_Pcs_Type4Mtrs.Text)
            'cmd2.Parameters.Add("@Type5_Meters", SqlDbType.Decimal)
            'cmd2.Parameters("@Type5_Meters").Value = Val(txt_Pcs_Type5Mtrs.Text)
            'cmd2.Parameters.Add("@Total_Checking_Meters", SqlDbType.Decimal)
            'cmd2.Parameters("@Total_Checking_Meters").Value = Val(lbl_Pcs_TotalMtrs.Text)
            'cmd2.Parameters.Add("@Weight", SqlDbType.Decimal)
            'cmd2.Parameters("@Weight").Value = Val(txt_Pcs_Weight.Text)
            'cmd2.Parameters.Add("@Weight_Meter", SqlDbType.Decimal)
            'cmd2.Parameters("@Weight_Meter").Value = Val(lbl_Pcs_Wgt_Mtr.Text)
            'cmd2.Parameters.Add("@Beam_Knotting_Code", SqlDbType.VarChar)
            'cmd2.Parameters("@Beam_Knotting_Code").Value = Trim(lbl_Pcs_KnotCode.Text)
            'cmd2.Parameters.Add("@Beam_Knotting_No", SqlDbType.VarChar)
            'cmd2.Parameters("@Beam_Knotting_No").Value = Trim(lbl_Pcs_KnotNo.Text)
            'cmd2.Parameters.Add("@Loom_IdNo", SqlDbType.Int)
            'cmd2.Parameters("@Loom_IdNo").Value = Lm_ID
            'cmd2.Parameters.Add("@Width_Type", SqlDbType.VarChar)
            'cmd2.Parameters("@Width_Type").Value = Trim(lbl_Pcs_WidthType.Text)
            'cmd2.Parameters.Add("@Crimp_Percentage", SqlDbType.Decimal)
            'cmd2.Parameters("@Crimp_Percentage").Value = Val(lbl_Pcs_CrimpPerc.Text)
            'cmd2.Parameters.Add("@Set_Code1", SqlDbType.VarChar)
            'cmd2.Parameters("@Set_Code1").Value = Trim(lbl_Pcs_SetCode1.Text)
            'cmd2.Parameters.Add("@Set_No1", SqlDbType.VarChar)
            'cmd2.Parameters("@Set_No1").Value = Trim(lbl_Pcs_SetNo1.Text)
            'cmd2.Parameters.Add("@Beam_No1", SqlDbType.VarChar)
            'cmd2.Parameters("@Beam_No1").Value = Trim(lbl_Pcs_BeamNo1.Text)
            'cmd2.Parameters.Add("@Balance_Meters1", SqlDbType.Decimal)
            'cmd2.Parameters("@Balance_Meters1").Value = Val(lbl_Pcs_Beam_BalMtrs1.Text)
            'cmd2.Parameters.Add("@Set_Code2", SqlDbType.VarChar)
            'cmd2.Parameters("@Set_Code2").Value = Trim(lbl_Pcs_SetCode2.Text)
            'cmd2.Parameters.Add("@Set_No2", SqlDbType.VarChar)
            'cmd2.Parameters("@Set_No2").Value = Trim(lbl_Pcs_SetNo2.Text)
            'cmd2.Parameters.Add("@Beam_No2", SqlDbType.VarChar)
            'cmd2.Parameters("@Beam_No2").Value = Trim(lbl_Pcs_BeamNo2.Text)
            'cmd2.Parameters.Add("@Balance_Meters2", SqlDbType.Decimal)
            'cmd2.Parameters("@Balance_Meters2").Value = Val(lbl_Pcs_Beam_BalMtrs1.Text)
            'cmd2.Parameters.Add("@BeamConsumption_Meters", SqlDbType.Decimal)
            'cmd2.Parameters("@BeamConsumption_Meters").Value = Val(lbl_Pcs_BeamConsMtrs.Text)
            'cmd2.ExecuteNonQuery()



            'Nr = 0
            'SQL1 = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_No = '" & Trim(lbl_ChkNo.Text) & "', Weaver_Piece_Checking_Date = '" & Trim(Format(dtp_Date.Value, "MM/dd/yyyy")) & "', Ledger_Idno = " & Str(Val(Wev_ID)) & ", StockOff_IdNo = " & Str(Val(stkof_idno)) & ", Cloth_IdNo = " & Str(Val(clth_ID)) & ", Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(txt_Pcs_No.Text)) & ", Main_PieceNo = '" & Trim(Val(txt_Pcs_No.Text)) & "', PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_Pcs_No.Text))) & ", ReceiptMeters_Checking = " & Str(Val(txt_Pcs_RecMtrs.Text)) & ", Receipt_Meters = " & Str(Val(txt_Pcs_RecMtrs.Text)) & ", Loom_No = '" & Trim(cbo_Pcs_LoomNo.Text) & "', Is_LastPiece = '" & Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) & "', Pick = " & Str(Val(txt_Pcs_Pick.Text)) & ", Width = " & Str(Val(txt_Pcs_Width.Text)) & ", Type1_Meters = " & Str(Val(txt_Pcs_Type1Mtrs.Text)) & ", Type2_Meters = " & Str(Val(txt_Pcs_Type2Mtrs.Text)) & ", Type3_Meters = " & Str(Val(txt_Pcs_Type3Mtrs.Text)) & ", Type4_Meters = " & Str(Val(txt_Pcs_Type4Mtrs.Text)) & ", Type5_Meters = " & Str(Val(txt_Pcs_Type5Mtrs.Text)) & ", Total_Checking_Meters = " & Str(Val(lbl_Pcs_TotalMtrs.Text)) & ", Weight = " & Str(Val(txt_Pcs_Weight.Text)) & ", Weight_Meter = " & Str(Val(lbl_Pcs_Wgt_Mtr.Text)) & ", "
            'SQL1 = SQL1 & " Beam_Knotting_Code = '" & Trim(lbl_Pcs_KnotCode.Text) & "', Beam_Knotting_No = '" & Trim(lbl_Pcs_KnotNo.Text) & "', Loom_IdNo = " & Str(Val(Lm_ID)) & ", Width_Type = '" & Trim(lbl_Pcs_WidthType.Text) & "', Crimp_Percentage = " & Str(Val(lbl_Pcs_CrimpPerc.Text)) & ", Set_Code1 = '" & Trim(lbl_Pcs_SetCode1.Text) & "', Set_No1 = '" & Trim(lbl_Pcs_SetNo1.Text) & "', Beam_No1 = '" & Trim(lbl_Pcs_BeamNo1.Text) & "', Balance_Meters1 = " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", Set_Code2 = '" & Trim(lbl_Pcs_SetCode2.Text) & "', Set_No2 = '" & Trim(lbl_Pcs_SetNo2.Text) & "', Beam_No2 = '" & Trim(lbl_Pcs_BeamNo2.Text) & "', Balance_Meters2 = " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", BeamConsumption_Meters = " & Str(Val(lbl_Pcs_BeamConsMtrs.Text)) & " "
            'SQL1 = SQL1 & " Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "' and Lot_Code = '" & Trim(LotCd) & "' and Piece_No = '" & Trim(txt_Pcs_No.Text) & "'"

            'cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            'Nr = cmd.ExecuteNonQuery()

            ''cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_No = '" & Trim(lbl_ChkNo.Text) & "', Weaver_Piece_Checking_Date = @CheckingDate, Ledger_Idno = " & Str(Val(Wev_ID)) & ", StockOff_IdNo = " & Str(Val(stkof_idno)) & ", Cloth_IdNo = " & Str(Val(clth_ID)) & ", Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(txt_Pcs_No.Text)) & ", Main_PieceNo = '" & Trim(Val(txt_Pcs_No.Text)) & "', PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_Pcs_No.Text))) & ", ReceiptMeters_Checking = " & Str(Val(txt_Pcs_RecMtrs.Text)) & ", Receipt_Meters = " & Str(Val(txt_Pcs_RecMtrs.Text)) & ", Loom_No = '" & Trim(cbo_Pcs_LoomNo.Text) & "', Is_LastPiece = '" & Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) & "', Pick = " & Str(Val(txt_Pcs_Pick.Text)) & ", Width = " & Str(Val(txt_Pcs_Width.Text)) & ", Type1_Meters = " & Str(Val(txt_Pcs_Type1Mtrs.Text)) & ", Type2_Meters = " & Str(Val(txt_Pcs_Type2Mtrs.Text)) & ", Type3_Meters = " & Str(Val(txt_Pcs_Type3Mtrs.Text)) & ", Type4_Meters  = " & Str(Val(txt_Pcs_Type4Mtrs.Text)) & ", Type5_Meters = " & Str(Val(txt_Pcs_Type5Mtrs.Text)) & ", Total_Checking_Meters = " & Str(Val(lbl_Pcs_TotalMtrs.Text)) & ", Weight = " & Str(Val(txt_Pcs_Weight.Text)) & ", Weight_Meter = " & Str(Val(lbl_Pcs_Wgt_Mtr.Text)) & ", " & _
            ''                    " Beam_Knotting_Code = '" & Trim(lbl_Pcs_KnotCode.Text) & "', Beam_Knotting_No = '" & Trim(lbl_Pcs_KnotNo.Text) & "', Loom_IdNo = " & Str(Val(Lm_ID)) & ", Width_Type = '" & Trim(lbl_Pcs_WidthType.Text) & "', Crimp_Percentage = " & Str(Val(lbl_Pcs_CrimpPerc.Text)) & ", Set_Code1 = '" & Trim(lbl_Pcs_SetCode1.Text) & "', Set_No1 = '" & Trim(lbl_Pcs_SetNo1.Text) & "', Beam_No1 = '" & Trim(lbl_Pcs_BeamNo1.Text) & "', Balance_Meters1 = " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", Set_Code2 = '" & Trim(lbl_Pcs_SetCode2.Text) & "', Set_No2 = '" & Trim(lbl_Pcs_SetNo2.Text) & "', Beam_No2 = '" & Trim(lbl_Pcs_BeamNo2.Text) & "', Balance_Meters2 = " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", BeamConsumption_Meters = " & Str(Val(lbl_Pcs_BeamConsMtrs.Text)) & " " & _
            ''                    " Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "' and Lot_Code = '" & Trim(LotCd) & "' and Piece_No = '" & Trim(txt_Pcs_No.Text) & "'"
            ''Nr = cmd.ExecuteNonQuery()

            'If Nr = 0 Then

            '    SQL1 = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code ,             Company_IdNo         ,     Weaver_Piece_Checking_No   ,                   Weaver_Piece_Checking_Date          ,         Ledger_Idno     ,         StockOff_IdNo       ,                         Weaver_ClothReceipt_Code                ,      Weaver_ClothReceipt_No     ,             for_orderby       ,                Weaver_ClothReceipt_Date                      ,        Lot_Code      ,       Lot_No         ,           Cloth_IdNo     ,            Folding_Checking       ,             Folding               ,                 Sl_No            ,                 Piece_No       ,           Main_PieceNo              ,                        PieceNo_OrderBy                                        ,            ReceiptMeters_Checking     ,                Receipt_Meters          ,               Loom_No              ,                         Is_LastPiece                    ,                 Pick               ,                     Width            ,            Type1_Meters                 ,                   Type2_Meters          ,        Type3_Meters                     ,           Type4_Meters                  ,        Type5_Meters                     ,                  Total_Checking_Meters  ,                     Weight           ,                   Weight_Meter        ,                Beam_Knotting_Code        ,           Beam_Knotting_No         ,         Loom_IdNo      ,           Width_Type                  ,             Crimp_Percentage            ,              Set_Code1               ,                  Set_No1           ,                 Beam_No1            ,                   Balance_Meters1      ,                  Set_Code2           ,                   Set_No2          ,                   Beam_No2          ,                    Balance_Meters2     ,                    BeamConsumption_Meters    ) "
            '    SQL1 = SQL1 & "     Values                            (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_ChkNo.Text) & "',  '" & Trim(Format(dtp_Date.Value, "MM/dd/yyyy")) & "' , " & Str(Val(Wev_ID)) & ", " & Str(Val(stkof_idno)) & ", '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "',   '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vOrdBy_RECNO)) & ", '" & Trim(Format(CDate(lbl_RecDate.Text), "MM/dd/yyyy")) & "', '" & Trim(LotCd) & "', '" & Trim(LotNo) & "', " & Str(Val(clth_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Pcs_No.Text)) & ", '" & Trim(txt_Pcs_No.Text) & "', '" & Trim(Val(txt_Pcs_No.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Trim(txt_Pcs_No.Text)))) & ", " & Str(Val(txt_Pcs_RecMtrs.Text)) & ",  " & Str(Val(txt_Pcs_RecMtrs.Text)) & ", '" & Trim(cbo_Pcs_LoomNo.Text) & "', '" & Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) & "', " & Str(Val(txt_Pcs_Pick.Text)) & ",  " & Str(Val(txt_Pcs_Width.Text)) & ", " & Str(Val(txt_Pcs_Type1Mtrs.Text)) & ", " & Str(Val(txt_Pcs_Type2Mtrs.Text)) & ", " & Str(Val(txt_Pcs_Type3Mtrs.Text)) & ", " & Str(Val(txt_Pcs_Type4Mtrs.Text)) & ", " & Str(Val(txt_Pcs_Type5Mtrs.Text)) & ", " & Str(Val(lbl_Pcs_TotalMtrs.Text)) & ", " & Str(Val(txt_Pcs_Weight.Text)) & ", " & Str(Val(lbl_Pcs_Wgt_Mtr.Text)) & ", '" & Trim(lbl_Pcs_KnotCode.Text) & "', '" & Trim(lbl_Pcs_KnotNo.Text) & "', " & Str(Val(Lm_ID)) & ", '" & Trim(lbl_Pcs_WidthType.Text) & "', " & Str(Val(lbl_Pcs_CrimpPerc.Text)) & ", '" & Trim(lbl_Pcs_SetCode1.Text) & "', '" & Trim(lbl_Pcs_SetNo1.Text) & "', '" & Trim(lbl_Pcs_BeamNo1.Text) & "', " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", '" & Trim(lbl_Pcs_SetCode2.Text) & "', '" & Trim(lbl_Pcs_SetNo2.Text) & "', '" & Trim(lbl_Pcs_BeamNo2.Text) & "', " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", " & Str(Val(lbl_Pcs_BeamConsMtrs.Text)) & "  ) "

            '    cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            '    cmd.ExecuteNonQuery()

            '    'cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code ,             Company_IdNo         ,     Weaver_Piece_Checking_No   ,  Weaver_Piece_Checking_Date,         Ledger_Idno     ,         StockOff_IdNo       ,                         Weaver_ClothReceipt_Code                ,      Weaver_ClothReceipt_No     ,              for_orderby      , Weaver_ClothReceipt_Date,        Lot_Code      ,       Lot_No         ,           Cloth_IdNo     ,            Folding_Checking       ,             Folding               ,                 Sl_No            ,                 Piece_No       ,           Main_PieceNo              ,                        PieceNo_OrderBy                                        ,            ReceiptMeters_Checking     ,                Receipt_Meters          ,               Loom_No              ,                         Is_LastPiece                    ,                 Pick               ,                     Width            ,            Type1_Meters                 ,                   Type2_Meters          ,        Type3_Meters                     ,           Type4_Meters                  ,        Type5_Meters                     ,                  Total_Checking_Meters  ,                     Weight           ,                   Weight_Meter        ,                Beam_Knotting_Code        ,           Beam_Knotting_No         ,         Loom_IdNo      ,           Width_Type                  ,             Crimp_Percentage            ,              Set_Code1               ,                  Set_No1           ,                 Beam_No1            ,                   Balance_Meters1      ,                  Set_Code2           ,                   Set_No2          ,                   Beam_No2          ,                    Balance_Meters2     ,                    BeamConsumption_Meters    ) " &
            '    '                    "     Values                                 (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_ChkNo.Text) & "',        @CheckingDate       , " & Str(Val(Wev_ID)) & ", " & Str(Val(stkof_idno)) & ", '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "',   '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vOrdBy_RECNO)) & ",      @ReceiptDate           , '" & Trim(LotCd) & "', '" & Trim(LotNo) & "', " & Str(Val(clth_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Pcs_No.Text)) & ", '" & Trim(txt_Pcs_No.Text) & "', '" & Trim(Val(txt_Pcs_No.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Trim(txt_Pcs_No.Text)))) & ", " & Str(Val(txt_Pcs_RecMtrs.Text)) & ",  " & Str(Val(txt_Pcs_RecMtrs.Text)) & ", '" & Trim(cbo_Pcs_LoomNo.Text) & "', '" & Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) & "', " & Str(Val(txt_Pcs_Pick.Text)) & ",  " & Str(Val(txt_Pcs_Width.Text)) & ", " & Str(Val(txt_Pcs_Type1Mtrs.Text)) & ", " & Str(Val(txt_Pcs_Type2Mtrs.Text)) & ", " & Str(Val(txt_Pcs_Type3Mtrs.Text)) & ", " & Str(Val(txt_Pcs_Type4Mtrs.Text)) & ", " & Str(Val(txt_Pcs_Type5Mtrs.Text)) & ", " & Str(Val(lbl_Pcs_TotalMtrs.Text)) & ", " & Str(Val(txt_Pcs_Weight.Text)) & ", " & Str(Val(lbl_Pcs_Wgt_Mtr.Text)) & ", '" & Trim(lbl_Pcs_KnotCode.Text) & "', '" & Trim(lbl_Pcs_KnotNo.Text) & "', " & Str(Val(Lm_ID)) & ", '" & Trim(lbl_Pcs_WidthType.Text) & "', " & Str(Val(lbl_Pcs_CrimpPerc.Text)) & ", '" & Trim(lbl_Pcs_SetCode1.Text) & "', '" & Trim(lbl_Pcs_SetNo1.Text) & "', '" & Trim(lbl_Pcs_BeamNo1.Text) & "', " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", '" & Trim(lbl_Pcs_SetCode2.Text) & "', '" & Trim(lbl_Pcs_SetNo2.Text) & "', '" & Trim(lbl_Pcs_BeamNo2.Text) & "', " & Str(Val(lbl_Pcs_Beam_BalMtrs1.Text)) & ", " & Str(Val(lbl_Pcs_BeamConsMtrs.Text)) & "  ) "
            '    'cmd.ExecuteNonQuery()

            'End If

            'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_ClothReceipt_Piece_Details", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdBy_CHKNO), Pk_Condition, "", "", New_Entry, False, "Piece_No,Main_PieceNo,PieceNo_OrderBy,ReceiptMeters_Checking,Receipt_Meters,Loom_No,Is_LastPiece,Pick,Width,Type1_Meters,Type2_Meters,Type3_Meters,Type4_Meters,Type5_Meters,Total_Checking_Meters,Weight,Weight_Meter,Beam_Knotting_Code,Beam_Knotting_No,Loom_IdNo, Width_Type,Crimp_Percentage,Set_Code1,Set_No1,Beam_No1,Balance_Meters1,Set_Code2,Set_No2,Beam_No2,Balance_Meters2,BeamConsumption_Meters", "Sl_No", "Weaver_Piece_Checking_Code, For_OrderBy, Company_IdNo, Weaver_Piece_Checking_No, Weaver_Piece_Checking_Date, Ledger_Idno", tr)

            'If Val(txt_Pcs_RecMtrs.Text) <> 0 Then

            '    If Val(Lm_ID) <> 0 Then

            '        If Trim(lbl_Pcs_KnotCode.Text) <> "" Then

            '            Nr = 0
            '            cmd.CommandText = "Update Beam_Knotting_Head set Production_Meters = Production_Meters + " & Str(Val(txt_Pcs_RecMtrs.Text)) & " where Loom_IdNo = " & Str(Val(Lm_ID)) & " and Beam_Knotting_Code = '" & Trim(lbl_Pcs_KnotCode.Text) & "' and Ledger_IdNo = " & Str(Val(Wev_ID))
            '            Nr = cmd.ExecuteNonQuery
            '            If Nr = 0 Then
            '                Throw New ApplicationException("Mismatch of Loom Knotting && Party")
            '                Exit Sub
            '            End If

            '        End If

            '        'If Trim(lbl_Pcs_SetCode1.Text) <> "" And Trim(lbl_Pcs_BeamNo1.Text) <> "" Then
            '        '    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = Production_Meters + " & Str(Val(lbl_Pcs_BeamConsMtrs.Text)) & " where set_code = '" & Trim(lbl_Pcs_SetCode1.Text) & "' and beam_no = '" & Trim(lbl_Pcs_BeamNo1.Text) & "'"
            '        '    cmd.ExecuteNonQuery()
            '        'End If

            '        'If Trim(lbl_Pcs_SetCode2.Text) <> "" And Trim(lbl_Pcs_BeamNo2.Text) <> "" Then
            '        '    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = Production_Meters + " & Str(Val(lbl_Pcs_BeamConsMtrs.Text)) & " Where set_code = '" & Trim(lbl_Pcs_SetCode2.Text) & "' and beam_no = '" & Trim(lbl_Pcs_BeamNo2.Text) & "'"
            '        '    cmd.ExecuteNonQuery()
            '        'End If

            '    End If

            'End If

            ''---- stock Posting
            'Call Stock_Posting(NewCode, Wev_ID, clth_ID, LotCd, Lm_ID, WagesCode, tr)

            ''----- Checking for negative beam meters
            'If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, clth_ID, lbl_Pcs_SetCode1.Text, lbl_Pcs_BeamNo1.Text, vErrMsg, tr) = True Then
            '    Throw New ApplicationException(vErrMsg)
            '    Exit Sub
            'End If

            ''----- Saving Verification-1
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then '----KRG TEXTILE MILLS (PALLADAM)

            '    cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            '    cmd.ExecuteNonQuery()

            '    If New_Entry = False Then
            '        '----- Editing

            '        If Trim(Old_SetCd1) <> "" And Trim(Old_Beam1) <> "" Then
            '            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(Old_SetCd1) & "' and Beam_No = '" & Trim(Old_Beam1) & "'"
            '            cmd.ExecuteNonQuery()
            '            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select '" & Trim(Old_SetCd1) & "', '" & Trim(Old_Beam1) & "', -1*BeamConsumption_Meters from Weaver_ClothReceipt_Piece_Details where (Set_Code1 = '" & Trim(Old_SetCd1) & "' and Beam_No1 = '" & Trim(Old_Beam1) & "') OR (Set_Code2 = '" & Trim(Old_SetCd1) & "' and Beam_No2 = '" & Trim(Old_Beam1) & "')"
            '            cmd.ExecuteNonQuery()
            '        End If

            '        If Trim(Old_SetCd2) <> "" And Trim(Old_Beam2) <> "" Then
            '            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(Old_SetCd2) & "' and Beam_No = '" & Trim(Old_Beam2) & "'"
            '            cmd.ExecuteNonQuery()
            '            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select '" & Trim(Old_SetCd2) & "', '" & Trim(Old_Beam2) & "', -1*BeamConsumption_Meters from Weaver_ClothReceipt_Piece_Details where (Set_Code1 = '" & Trim(Old_SetCd2) & "' and Beam_No1 = '" & Trim(Old_Beam2) & "') OR (Set_Code2 = '" & Trim(Old_SetCd2) & "' and Beam_No2 = '" & Trim(Old_Beam2) & "')"
            '            cmd.ExecuteNonQuery()
            '        End If

            '    End If

            '    ''----- Saving
            '    If Trim(lbl_Pcs_SetCode1.Text) <> "" And Trim(lbl_Pcs_BeamNo1.Text) <> "" Then
            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(lbl_Pcs_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_Pcs_BeamNo1.Text) & "'"
            '        cmd.ExecuteNonQuery()
            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select '" & Trim(lbl_Pcs_SetCode1.Text) & "', '" & Trim(lbl_Pcs_BeamNo1.Text) & "', -1*BeamConsumption_Meters from Weaver_ClothReceipt_Piece_Details where (Set_Code1 = '" & Trim(lbl_Pcs_SetCode1.Text) & "' and Beam_No1 = '" & Trim(lbl_Pcs_BeamNo1.Text) & "') OR (Set_Code2 = '" & Trim(lbl_Pcs_SetCode1.Text) & "' and Beam_No2 = '" & Trim(lbl_Pcs_BeamNo1.Text) & "')"
            '        cmd.ExecuteNonQuery()
            '    End If
            '    If Trim(lbl_Pcs_SetCode2.Text) <> "" And Trim(lbl_Pcs_BeamNo2.Text) <> "" Then
            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(lbl_Pcs_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_Pcs_BeamNo2.Text) & "'"
            '        cmd.ExecuteNonQuery()
            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) select '" & Trim(lbl_Pcs_SetCode2.Text) & "', '" & Trim(lbl_Pcs_BeamNo2.Text) & "', -1*BeamConsumption_Meters from Weaver_ClothReceipt_Piece_Details where (Set_Code1 = '" & Trim(lbl_Pcs_SetCode2.Text) & "' and Beam_No1 = '" & Trim(lbl_Pcs_BeamNo2.Text) & "') OR (Set_Code2 = '" & Trim(lbl_Pcs_SetCode2.Text) & "' and Beam_No2 = '" & Trim(lbl_Pcs_BeamNo2.Text) & "')"
            '        cmd.ExecuteNonQuery()
            '    End If

            '    Da = New SqlClient.SqlDataAdapter("select Name1, Name2, sum(Meters1) as ProdMtrs from " & Trim(Common_Procedures.EntryTempTable) & " Group by Name1, Name2 having sum(Meters1) <> 0 Order by Name1, Name2", con)
            '    Da.SelectCommand.Transaction = tr
            '    Dt2 = New DataTable
            '    Da.Fill(Dt2)
            '    If Dt2.Rows.Count > 0 Then
            '        If IsDBNull(Dt2.Rows(0).Item("ProdMtrs").ToString) = False Then
            '            If Val(Dt2.Rows(0).Item("ProdMtrs").ToString) <> 0 Then
            '                Throw New ApplicationException("Invalid Editing : Mismatch of Production Meters in Beams")
            '                Exit Sub
            '            End If
            '        End If
            '    End If
            '    Dt2.Clear()

            '    '----- Saving Verification-2
            '    cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            '    cmd.ExecuteNonQuery()

            '    If Trim(lbl_Pcs_KnotCode.Text) <> "" Then
            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select Production_Meters from Beam_Knotting_Head where Beam_Knotting_Code = '" & Trim(lbl_Pcs_KnotCode.Text) & "'"
            '        cmd.ExecuteNonQuery()
            '        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select -1*ReceiptMeters_Checking from Weaver_ClothReceipt_Piece_Details where Beam_Knotting_Code = '" & Trim(lbl_Pcs_KnotCode.Text) & "'"
            '        cmd.ExecuteNonQuery()
            '    End If

            '    Da = New SqlClient.SqlDataAdapter("select sum(Meters1) as ProdMtrs from " & Trim(Common_Procedures.EntryTempTable) & " having sum(Meters1) <> 0", con)
            '    Da.SelectCommand.Transaction = tr
            '    Dt2 = New DataTable
            '    Da.Fill(Dt2)
            '    If Dt2.Rows.Count > 0 Then
            '        If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
            '            If Val(Dt2.Rows(0)(0).ToString) <> 0 Then
            '                Throw New ApplicationException("Invalid Editing : Mismatch of Production Meters in Knotting")
            '                Exit Sub
            '            End If
            '        End If
            '    End If
            '    Dt2.Clear()

            'End If

            'If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

            '    cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
            '                              " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "'"
            '    cmd.ExecuteNonQuery()

            '    If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

            'End If

            tr.Commit()

            vNewly_Added_PcsNo = Trim(txt_Pcs_No.Text)
            move_record(lbl_RefNo.Text)
            vNewly_Added_PcsNo = ""

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            Save_Status = True

        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Trim(UCase(ex.Message)), Trim(UCase("Duplicate_WeaverClothReceiptPieceDetails_PieceCodeforSelection"))) > 0 Then
                MessageBox.Show("Duplicate Piece No", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Pcs_No.Visible And txt_Pcs_No.Enabled Then txt_Pcs_No.Focus() Else msk_date.Focus()
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

    Private Sub Calculation_Totals()
        Dim TotPcs As Integer
        Dim TotRec As String
        Dim Totsnd As String
        Dim Totsec As String
        Dim Totbit As String
        Dim Totrej As String
        Dim Tototr As String
        Dim Tottlmr As String
        Dim Totwgt As String

        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub

        TotPcs = 0
        TotRec = 0 : Totsnd = 0 : Totsec = 0 : Totbit = 0 : Totrej = 0 : Tototr = 0 : Tottlmr = 0 : Totwgt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(0).Value) <> 0 Then

                    TotPcs = TotPcs + 1
                    TotRec = Format(Val(TotRec) + Val(.Rows(i).Cells(1).Value()), "########0.00")
                    Totsnd = Format(Val(Totsnd) + Val(.Rows(i).Cells(5).Value()), "########0.00")
                    Totsec = Format(Val(Totsec) + Val(.Rows(i).Cells(6).Value()), "########0.00")
                    Totbit = Format(Val(Totbit) + Val(.Rows(i).Cells(7).Value()), "########0.00")
                    Totrej = Format(Val(Totrej) + Val(.Rows(i).Cells(8).Value()), "########0.00")
                    Tototr = Format(Val(Tototr) + Val(.Rows(i).Cells(9).Value()), "########0.00")
                    Tottlmr = Format(Val(Tottlmr) + Val(.Rows(i).Cells(10).Value()), "########0.00")
                    Totwgt = Format(Val(Totwgt) + Val(.Rows(i).Cells(11).Value()), "########0.000")

                End If

            Next i

        End With


        With dgv_Details_Total1
            If .RowCount = 0 Then .Rows.Add()

            .Rows(0).Cells(0).Value = TotPcs
            .Rows(0).Cells(1).Value = Format(Val(TotRec), "########0.00")
            .Rows(0).Cells(5).Value = Format(Val(Totsnd), "########0.00")
            .Rows(0).Cells(6).Value = Format(Val(Totsec), "########0.00")
            .Rows(0).Cells(7).Value = Format(Val(Totbit), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(Totrej), "########0.00")
            .Rows(0).Cells(9).Value = Format(Val(Tototr), "########0.00")
            .Rows(0).Cells(10).Value = Format(Val(Tottlmr), "########0.00")
            .Rows(0).Cells(11).Value = Format(Val(Totwgt), "########0.000")

        End With

        'With dgv_Details_Total2
        '    If .RowCount = 0 Then .Rows.Add()
        '    .Rows(0).Cells(0).Value = "100%"
        '    .Rows(0).Cells(1).Value = "FOLDING"

        '    .Rows(0).Cells(5).Value = Format(Val(Totsnd) * Val(txt_Folding.Text) / 100, "########0.00")
        '    .Rows(0).Cells(6).Value = Format(Val(Totsec) * Val(txt_Folding.Text) / 100, "########0.00")
        '    .Rows(0).Cells(7).Value = Format(Val(Totbit) * Val(txt_Folding.Text) / 100, "########0.00")
        '    .Rows(0).Cells(8).Value = Format(Val(Totrej) * Val(txt_Folding.Text) / 100, "########0.00")
        '    .Rows(0).Cells(9).Value = Format(Val(Tototr) * Val(txt_Folding.Text) / 100, "########0.00")
        '    .Rows(0).Cells(10).Value = Format(Val(Tottlmr) * Val(txt_Folding.Text) / 100, "########0.00")

        'End With

        'Calculation_Excess_Short_Meter()

    End Sub

    Private Sub Calculation_Excess_Short_Meter()
        If FrmLdSTS = True Then Exit Sub
        'With dgv_Details_Total1
        '    If .Visible = True Then
        '        If .Rows.Count > 0 Then
        '            lbl_ExcessShort.Text = Val(.Rows(0).Cells(10).Value) - Val(lbl_RecMeter.Text)
        '        End If
        '    End If
        'End With
    End Sub

    Private Sub Calculation_Pcs_TotalMeter()
        Dim fldmtr As Integer = 0
        Dim Tot_Pc_Mtrs As Single = 0, Tot_Pc_Wt As Single = 0
        Dim fldperc As Single = 0
        Dim Wgt_Mtr As Single = 0
        Dim k As Integer = 0

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        With dgv_Details

            lbl_Pcs_TotalMtrs.Text = Format(Val(txt_Pcs_Type1Mtrs.Text) + Val(txt_Pcs_Type2Mtrs.Text) + Val(txt_Pcs_Type3Mtrs.Text) + Val(txt_Pcs_Type4Mtrs.Text) + Val(txt_Pcs_Type5Mtrs.Text), "###########0.00")

            'Tot_Pc_Mtrs = Val(lbl_Pcs_TotalMtrs.Text)
            'Tot_Pc_Wt = Val(txt_Pcs_Weight.Text)

            'For k = 0 To .Rows.Count - 1
            '    If Val(txt_Pcs_No.Text) = Val(.Rows(k).Cells(0).Value) Then
            '        If Trim(UCase(txt_Pcs_No.Text)) <> Trim(UCase(.Rows(k).Cells(0).Value)) Then
            '            Tot_Pc_Mtrs = Tot_Pc_Mtrs + Val(.Rows(k).Cells(5).Value) + Val(.Rows(k).Cells(6).Value) + Val(.Rows(k).Cells(7).Value) + Val(.Rows(k).Cells(8).Value) + Val(.Rows(k).Cells(9).Value)
            '            Tot_Pc_Wt = Tot_Pc_Wt + +Val(.Rows(k).Cells(11).Value)
            '        End If
            '    End If
            'Next

            'fldperc = Val(txt_Folding.Text)
            'If fldperc = 0 Then fldperc = 100

            'Wgt_Mtr = 0
            'If Tot_Pc_Mtrs <> 0 Then Wgt_Mtr = Format(Val(Tot_Pc_Wt) / (Tot_Pc_Mtrs * Val(fldperc) / 100), "#########0.000")

            'lbl_Pcs_Wgt_Mtr.Text = Format(Val(Wgt_Mtr), "#########0.000")
            'For k = 0 To .Rows.Count - 1
            '    If Val(txt_Pcs_No.Text) = Val(.Rows(k).Cells(0).Value) Then
            '        If Trim(UCase(txt_Pcs_No.Text)) <> Trim(UCase(.Rows(k).Cells(0).Value)) Then
            '            .Rows(k).Cells(12).Value = ""
            '            If Val(Wgt_Mtr) <> 0 Then
            '                .Rows(k).Cells(12).Value = Format(Val(Wgt_Mtr), "#########0.000")
            '            End If
            '        End If
            '    End If
            'Next

        End With

    End Sub

    Private Sub cbo_Weaver_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver, msk_date, txt_Folding, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaver, txt_Pcs_No, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        'If Asc(e.KeyChar) = 13 Then

        '    If btn_Selection.Enabled = True Then
        '        If MessageBox.Show("Do you want to select Cloth Receipt", "FOR CLOTH RECEIPT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '            btn_Selection_Click(sender, e)

        '        Else
        '            If txt_Folding.Enabled Then txt_Folding.Focus() Else txt_Pcs_No.Focus()

        '        End If

        '    Else
        '        If txt_Folding.Enabled Then txt_Folding.Focus() Else txt_Pcs_No.Focus()

        '    End If

        'End If

    End Sub

    Private Sub cbo_Weaver_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Weaver.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Pcs_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Pcs_LoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "(Beam_Knotting_Code <> '' )", "(Loom_IdNo = 0 )")
    End Sub

    Private Sub cbo_Pcs_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Pcs_LoomNo.KeyDown
        Dim clo_ID As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Pcs_LoomNo, txt_Pcs_No, cbo_Pcs_ClothName, "Loom_Head", "Loom_Name", "(Beam_Knotting_Code <> '' )", "(Loom_IdNo = 0 )")

    End Sub

    Private Sub cbo_Pcs_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Pcs_LoomNo.KeyPress
        Dim clo_ID As Integer = 0
        Dim Lm_ID As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Pcs_LoomNo, cbo_Pcs_ClothName, "Loom_Head", "Loom_Name", "(Beam_Knotting_Code <> '')", "(Loom_IdNo = 0 )")



        If Asc(e.KeyChar) = 13 Then

            If Trim(cbo_Pcs_LoomNo.Text) <> "" And (Trim(UCase(cbo_Pcs_LoomNo.Text)) <> Trim(UCase(cbo_Pcs_LoomNo.Tag)) Or Trim(lbl_Pcs_KnotCode.Text) = "") Then

                Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_Pcs_LoomNo.Text)
                If Val(Lm_ID) <> 0 Then
                    btn_Select_KnottingDetails_Click(sender, e)

                Else

                    lbl_Pcs_KnotCode.Text = ""
                    lbl_Pcs_KnotNo.Text = ""

                    cbo_Pcs_ClothName.Text = ""
                    lbl_Pcs_EndsCount.Text = ""
                    lbl_Pcs_WeftCount.Text = ""

                    lbl_Pcs_WidthType.Text = ""
                    lbl_Pcs_CrimpPerc.Text = ""

                    lbl_Pcs_SetCode1.Text = ""
                    lbl_Pcs_SetNo1.Text = ""
                    lbl_Pcs_BeamNo1.Text = ""

                    lbl_Pcs_SetCode2.Text = ""
                    lbl_Pcs_SetNo2.Text = ""
                    lbl_Pcs_BeamNo2.Text = ""

                    lbl_Pcs_Beam_TotMtrs1.Text = ""
                    lbl_Pcs_Beam_BalMtrs1.Text = ""

                    lbl_Pcs_BeamConsMtrs.Text = ""

                End If

            End If

        End If

    End Sub

    Private Sub cbo_Pcs_LoomNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Pcs_LoomNo.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LoomNo_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Pcs_LoomNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Pcs_LastPiece_Status_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Pcs_LastPiece_Status.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Pcs_LastPiece_Status, cbo_Pcs_LoomNo, txt_Pcs_Type1Mtrs, "", "", "", "")
        'If (e.KeyValue = 40 And cbo_Pcs_LastPiece_Status.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        '    btn_Add.Focus()
        'End If

    End Sub

    Private Sub cbo_Pcs_LastPiece_Status_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Pcs_LastPiece_Status.KeyPress
        Dim clo_ID As Integer = 0
        Dim Lm_ID As Integer = 0

        clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Pcs_ClothName.Text)

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Pcs_LastPiece_Status, txt_Pcs_Type1Mtrs, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then

            If Trim(cbo_Pcs_LoomNo.Text) <> "" And (Trim(UCase(cbo_Pcs_LoomNo.Text)) <> Trim(UCase(cbo_Pcs_LoomNo.Tag)) Or Trim(UCase(cbo_Pcs_LastPiece_Status.Text)) <> Trim(UCase(cbo_Pcs_LastPiece_Status.Tag)) Or Trim(lbl_Pcs_KnotCode.Text) = "") Then

                Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_Pcs_LoomNo.Text)
                If Val(Lm_ID) <> 0 Then
                    btn_Select_KnottingDetails_Click(sender, e)

                Else
                    lbl_Pcs_KnotCode.Text = ""
                    lbl_Pcs_KnotNo.Text = ""

                    lbl_Pcs_WidthType.Text = ""
                    lbl_Pcs_CrimpPerc.Text = ""

                    lbl_Pcs_SetCode1.Text = ""
                    lbl_Pcs_SetNo1.Text = ""
                    lbl_Pcs_BeamNo1.Text = ""

                    lbl_Pcs_SetCode2.Text = ""
                    lbl_Pcs_SetNo2.Text = ""
                    lbl_Pcs_BeamNo2.Text = ""

                    lbl_Pcs_Beam_TotMtrs1.Text = ""
                    lbl_Pcs_Beam_BalMtrs1.Text = ""

                    lbl_Pcs_BeamConsMtrs.Text = ""

                End If

            End If

        End If

    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        dgv_Details_CellLeave(sender, e)

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        Try

            If FrmLdSTS = True Then Exit Sub
            If NoCalc_Status = True Then Exit Sub
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 1 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 8 Or e.ColumnIndex = 9 Or e.ColumnIndex = 11 Then
                            Calculation_Pcs_TotalMeter()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details

            If e.KeyValue = Keys.Delete Then

                If .CurrentCell.ColumnIndex = 5 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> "" Then
                        e.Handled = True
                    End If
                End If

                If .CurrentCell.ColumnIndex = 6 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
                        e.Handled = True
                    End If
                End If

                If .CurrentCell.ColumnIndex = 7 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(15).Value) <> "" Then
                        e.Handled = True
                    End If
                End If
                If .CurrentCell.ColumnIndex = 8 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(16).Value) <> "" Then
                        e.Handled = True
                    End If
                End If
                If .CurrentCell.ColumnIndex = 9 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(17).Value) <> "" Then
                        e.Handled = True
                    End If
                End If

            End If

        End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        On Error Resume Next
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 12 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                    If .CurrentCell.ColumnIndex = 5 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If

                    If .CurrentCell.ColumnIndex = 6 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = 7 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(15).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = 8 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(16).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = 9 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(17).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer = 0
        Dim J As Integer = 0
        Dim n As Integer = 0
        Dim nrw As Integer = 0
        Dim S As String = ""


        'If e.Control = True And (UCase(Chr(e.KeyCode)) = "A" Or UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then
        '    With dgv_Details

        '        n = .CurrentRow.Index

        '        S = Replace(Trim(.Rows(n).Cells(0).Value), Val(.Rows(n).Cells(0).Value), "")
        '        If Trim(UCase(S)) <> "Z" Then
        '            S = Trim(UCase(S))
        '            If Trim(S) = "" Then S = "A" Else S = Trim(Chr(Asc(S) + 1))
        '            If n <> .Rows.Count - 1 Then
        '                If Trim(Val(.Rows(n).Cells(0).Value)) & Trim(UCase(S)) = Trim(UCase(.Rows(n + 1).Cells(0).Value)) Then
        '                    MessageBox.Show("Already Piece Inserted", "DES NOT INSERT NEW PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '                    Exit Sub
        '                End If
        '            End If

        '            nrw = n + 1

        '            dgv_Details.Rows.Insert(nrw, 1)

        '            dgv_Details.Rows(nrw).Cells(0).Value = Trim(Val(.Rows(n).Cells(0).Value)) & S

        '            dgv_Details.Rows(nrw).Cells(2).Value = .Rows(n).Cells(2).Value
        '            If Val(.Rows(n).Cells(3).Value) <> 0 Then
        '                dgv_Details.Rows(nrw).Cells(3).Value = Val(.Rows(n).Cells(3).Value)
        '            End If
        '            If Val(.Rows(n).Cells(4).Value) <> 0 Then
        '                dgv_Details.Rows(nrw).Cells(4).Value = Val(.Rows(n).Cells(4).Value)
        '            End If

        '        End If

        '    End With

        'End If

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                If Trim(.Rows(.CurrentCell.RowIndex).Cells(13).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(15).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(16).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(17).Value) = "" Then

                    dgv_Details_DoubleClick(sender, e)
                    btn_Delete_Click(sender, e)

                    'n = .CurrentRow.Index

                    ''If trim(Val(.Rows(n).Cells(0).Value)) = Trim(.Rows(n).Cells(0).Value) Then
                    ''    MessageBox.Show("cannot remove this piece", "DOES NOT REMOVE DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    ''    Exit Sub
                    ''End If

                    'If Trim(Val(.Rows(n).Cells(0).Value)) = Trim(.Rows(n).Cells(0).Value) Then
                    '    For J = 1 To .ColumnCount - 1
                    '        .Rows(n).Cells(J).Value = ""
                    '    Next

                    'Else

                    '    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    '        For J = 1 To .ColumnCount - 1
                    '            .Rows(n).Cells(J).Value = ""
                    '        Next

                    '    Else
                    '        .Rows.RemoveAt(n)

                    '    End If

                    'End If

                    'Calculation_Totals()

                    'btn_Clear_Click(sender, e)

                Else
                    MessageBox.Show("Packing slip Prepared", "DOES NOT REMOVE DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End With

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        btn_Add_Click(sender, e)
        'save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
        'If e.KeyValue = 38 Then cbo_Weaver.Focus() ' SendKeys.Send("+{TAB}")
        'If (e.KeyValue = 40) Then
        '    txt_Pcs_No.Focus()
        '    'If dgv_Details.Rows.Count > 0 Then
        '    '    dgv_Details.Focus()
        '    '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        '    '    dgv_Details.CurrentCell.Selected = True
        '    'End If
        'End If
    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        Dim MtchSTS As Boolean = False
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 And Trim(txt_Pcs_No.Text) = "" Then
                MtchSTS = False
                For i = dgv_Details.Rows.Count - 1 To 0 Step -1
                    If (Val(dgv_Details.Rows(i).Cells(5).Value) + Val(dgv_Details.Rows(i).Cells(6).Value) + Val(dgv_Details.Rows(i).Cells(7).Value) + Val(dgv_Details.Rows(i).Cells(8).Value) + Val(dgv_Details.Rows(i).Cells(9).Value)) <> 0 Then
                        If (i + 1) <= (dgv_Details.Rows.Count - 1) Then
                            txt_Pcs_No.Text = dgv_Details.Rows(i + 1).Cells(0).Value
                        End If
                        MtchSTS = True
                        Exit For
                    End If
                Next i
                If MtchSTS = False Then
                    If dgv_Details.Rows.Count > 0 Then
                        txt_Pcs_No.Text = dgv_Details.Rows(0).Cells(0).Value
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clth_IdNo As Integer
        Dim Condt As String = ""
        Dim SQL1 As String

        Try

            cmd.Connection = con

            Condt = ""
            Led_IdNo = 0
            Clth_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Piece_Checking_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_Piece_Checking_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Piece_Checking_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            'If Trim(cbo_Filter_ClothName.Text) <> "" Then
            '    Clth_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_ClothName.Text)
            'End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            'If Val(Clth_IdNo) <> 0 Then
            '    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Cloth_IdNo = " & Str(Val(Clth_IdNo))
            'End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Weaver_ClothReceipt_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1

                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_ClothReceipt_date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("WeaverName").ToString
                    'dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Party_DcNo").ToString
                    'dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    'dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("EndsCount_Name").ToString


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
            vNewly_Added_PcsNo = ""
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


    Private Sub txt_Folding_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Folding.TextChanged
        Calculation_Totals()
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim Grd_UpSts As Boolean = False
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = 0
        Dim Wev_ID As Integer = 0
        Dim clth_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim PcsNoFrom As Integer = 0
        Dim PcsNoTo As Integer = 0
        Dim vPcsNo As String = ""
        Dim vExc_DofMtrs_Perc As String = 0
        Dim vALLOWED_EXC_DofMtrs As String = 0

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        If Val(txt_Pcs_No.Text) = 0 And Trim(txt_Pcs_No.Text) = "0" Then
            MessageBox.Show("Invalid Piece No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Pcs_No.Enabled Then txt_Pcs_No.Focus()
            Exit Sub
        End If

        'If Val(lbl_RecCode.Text) = 0 Then
        '    MessageBox.Show("Invalid Lot No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
        '    Exit Sub
        'End If

        Wev_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        If Wev_ID = 0 Then
            MessageBox.Show("Invalid Weaver Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
            Exit Sub
        End If

        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Pcs_ClothName.Text)
        If clth_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
            Exit Sub
        End If

        If Trim(UCase(txt_Pcs_No.Text)) <> Trim(UCase(txt_Pcs_No.Tag)) Then
            MessageBox.Show("Invalid Piece No and its Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Pcs_No.Enabled Then txt_Pcs_No.Focus()
            Exit Sub
        End If



        'If Val(txt_Pcs_No.Text) > Val(PcsNoTo) Then
        '    MessageBox.Show("Invalid Piece No, Greater than Last Piece No ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If txt_Pcs_No.Enabled Then txt_Pcs_No.Focus()
        '    Exit Sub
        'End If

        If Val(txt_Pcs_RecMtrs.Text) = 0 Then
            MessageBox.Show("Invalid Piece Receipt Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Pcs_RecMtrs.Enabled Then txt_Pcs_RecMtrs.Focus()
            Exit Sub
        End If

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_Pcs_LoomNo.Text)

        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
            Exit Sub
        End If
        'If Trim(lbl_Pcs_WidthType.Text) = "" Then
        '    MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
        '    Exit Sub
        'End If



        'If Trim(lbl_Pcs_SetCode1.Text) <> "" And Trim(lbl_Pcs_BeamNo1.Text) <> "" Then
        '    If Val(lbl_Pcs_TotalMtrs.Text) <> 0 Then

        '        vExc_DofMtrs_Perc = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Excess_Doffing_Meters_Percentage_Allowed", "(Cloth_IdNo = " & Str(Val(clth_ID)) & ")")

        '        vALLOWED_EXC_DofMtrs = Format(Val(lbl_Pcs_Beam_TotMtrs1.Text) * Val(vExc_DofMtrs_Perc) / 100, "##########0.00")

        '        If Val(lbl_Pcs_BeamConsMtrs.Text) > (Val(lbl_Pcs_Beam_BalMtrs1.Text) + Val(vALLOWED_EXC_DofMtrs)) Then
        '            MessageBox.Show("Invalid Piece Meters" & Chr(13) & "Greater than Balance Meters in Beam", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '            If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
        '            Exit Sub
        '        End If
        '    End If
        'End If


        Grd_UpSts = False

        With dgv_Details
            For i = 0 To .Rows.Count - 1
                If Trim(UCase(.Rows(i).Cells(0).Value)) = Trim(UCase(txt_Pcs_No.Text)) Then

                    Add_To_Grid(i)

                    If Val(lbl_Pcs_TotalMtrs.Text) > 0 Then
                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.BackColor = Color.White
                        Next j

                    Else
                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.BackColor = Color.FromArgb(251, 255, 246)
                        Next j

                    End If

                    If Trim(lbl_Pcs_PackSlipNo1.Text) <> "" Then .Rows(i).Cells(5).Style.ForeColor = Color.Red
                    If Trim(lbl_Pcs_PackSlipNo2.Text) <> "" Then .Rows(i).Cells(6).Style.ForeColor = Color.Red
                    If Trim(lbl_Pcs_PackSlipNo3.Text) <> "" Then .Rows(i).Cells(7).Style.ForeColor = Color.Red
                    If Trim(lbl_Pcs_PackSlipNo4.Text) <> "" Then .Rows(i).Cells(8).Style.ForeColor = Color.Red
                    If Trim(lbl_Pcs_PackSlipNo5.Text) <> "" Then .Rows(i).Cells(9).Style.ForeColor = Color.Red

                    Grd_UpSts = True

                    If i >= 9 Then
                        .FirstDisplayedScrollingRowIndex = i - 9
                    End If

                    Exit For

                End If
            Next i

            'PcsNoFrom = 0
            'PcsNoTo = 0
            'If dgv_Details.Rows.Count > 0 Then
            '    PcsNoFrom = Val(dgv_Details.Rows(0).Cells(0).Value)
            '    PcsNoTo = Val(dgv_Details.Rows(dgv_Details.Rows.Count - 1).Cells(0).Value)
            'End If

            'For i = 0 To .Rows.Count - 1
            '    If Val(.Rows(i).Cells(0).Value) = Val(txt_Pcs_No.Text) Then
            '        .Rows(i).Cells(12).Value = IIf(Val(lbl_Pcs_Wgt_Mtr.Text) <> 0, Format(Val(lbl_Pcs_Wgt_Mtr.Text), "#######0.000"), "")
            '    End If
            'Next i

            If Grd_UpSts = False Then
                For i = 0 To .Rows.Count - 1
                    If Val(.Rows(i).Cells(0).Value) = Val(txt_Pcs_No.Text) Then
                        n = i + 1
                        .Rows.Insert(n, 1)
                        Add_To_Grid(n)
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.BackColor = Color.White
                        Next j
                        'If i >= 8 Then
                        '    .FirstDisplayedScrollingRowIndex = i - 8
                        'End If

                        Exit For
                    End If
                Next
            End If

        End With

        Calculation_Totals()

        vPcsNo = txt_Pcs_No.Text

        Save_Status = False
        save_record()

        If Save_Status = True Then
            If txt_Pcs_No.Enabled And txt_Pcs_No.Visible Then txt_Pcs_No.Focus()

            For i = 0 To dgv_Details.Rows.Count - 1
                If Trim(UCase(dgv_Details.Rows(i).Cells(0).Value)) = Trim(UCase(vPcsNo)) Then
                    If (i + 1) <= (dgv_Details.Rows.Count - 1) Then
                        txt_Pcs_No.Text = dgv_Details.Rows(i + 1).Cells(0).Value
                    End If
                End If
            Next i

        End If

    End Sub


    Private Sub Add_To_Grid(ByVal Rw As Integer)
        With dgv_Details
            .Rows(Rw).Cells(0).Value = Trim(UCase(txt_Pcs_No.Text))
            .Rows(Rw).Cells(1).Value = IIf(Val(txt_Pcs_RecMtrs.Text) <> 0, Format(Val(txt_Pcs_RecMtrs.Text), "#######0.00"), "")
            .Rows(Rw).Cells(2).Value = Trim(cbo_Pcs_LoomNo.Text)
            .Rows(Rw).Cells(3).Value = IIf(Val(txt_Pcs_Pick.Text) <> 0, Val(txt_Pcs_Pick.Text), "")
            .Rows(Rw).Cells(4).Value = IIf(Val(txt_Pcs_Width.Text) <> 0, Val(txt_Pcs_Width.Text), "")
            .Rows(Rw).Cells(5).Value = IIf(Val(txt_Pcs_Type1Mtrs.Text) <> 0, Format(Val(txt_Pcs_Type1Mtrs.Text), "#######0.00"), "")
            .Rows(Rw).Cells(6).Value = IIf(Val(txt_Pcs_Type2Mtrs.Text) <> 0, Format(Val(txt_Pcs_Type2Mtrs.Text), "#######0.00"), "")
            .Rows(Rw).Cells(7).Value = IIf(Val(txt_Pcs_Type3Mtrs.Text) <> 0, Format(Val(txt_Pcs_Type3Mtrs.Text), "#######0.00"), "")
            .Rows(Rw).Cells(8).Value = IIf(Val(txt_Pcs_Type4Mtrs.Text) <> 0, Format(Val(txt_Pcs_Type4Mtrs.Text), "#######0.00"), "")
            .Rows(Rw).Cells(9).Value = IIf(Val(txt_Pcs_Type5Mtrs.Text) <> 0, Format(Val(txt_Pcs_Type5Mtrs.Text), "#######0.00"), "")
            .Rows(Rw).Cells(10).Value = IIf(Val(lbl_Pcs_TotalMtrs.Text) <> 0, Format(Val(lbl_Pcs_TotalMtrs.Text), "#######0.00"), "")
            .Rows(Rw).Cells(11).Value = IIf(Val(txt_Pcs_Weight.Text) <> 0, Format(Val(txt_Pcs_Weight.Text), "#######0.000"), "")
            .Rows(Rw).Cells(12).Value = IIf(Val(lbl_Pcs_Wgt_Mtr.Text) <> 0, Format(Val(lbl_Pcs_Wgt_Mtr.Text), "#######0.000"), "")
            .Rows(Rw).Cells(13).Value = Trim(lbl_Pcs_PackSlipNo1.Text)
            .Rows(Rw).Cells(14).Value = Trim(lbl_Pcs_PackSlipNo2.Text)
            .Rows(Rw).Cells(15).Value = Trim(lbl_Pcs_PackSlipNo3.Text)
            .Rows(Rw).Cells(16).Value = Trim(lbl_Pcs_PackSlipNo4.Text)
            .Rows(Rw).Cells(17).Value = Trim(lbl_Pcs_PackSlipNo5.Text)
            .Rows(Rw).Cells(18).Value = Trim(lbl_Pcs_KnotCode.Text)
            .Rows(Rw).Cells(19).Value = Trim(lbl_Pcs_KnotNo.Text)
            .Rows(Rw).Cells(20).Value = Trim(lbl_Pcs_SetCode1.Text)
            .Rows(Rw).Cells(21).Value = Trim(lbl_Pcs_SetNo1.Text)
            .Rows(Rw).Cells(22).Value = Trim(lbl_Pcs_BeamNo1.Text)
            .Rows(Rw).Cells(23).Value = Format(Val(lbl_Pcs_Beam_TotMtrs1.Text), "#########0.00")
            .Rows(Rw).Cells(24).Value = Trim(lbl_Pcs_SetCode2.Text)
            .Rows(Rw).Cells(25).Value = Trim(lbl_Pcs_SetNo2.Text)
            .Rows(Rw).Cells(26).Value = Trim(lbl_Pcs_BeamNo2.Text)
            .Rows(Rw).Cells(27).Value = Format(Val(lbl_Pcs_Beam_BalMtrs1.Text), "#########0.00")
            .Rows(Rw).Cells(28).Value = Trim(lbl_Pcs_WidthType.Text)
            .Rows(Rw).Cells(29).Value = Val(lbl_Pcs_CrimpPerc.Text)
            .Rows(Rw).Cells(30).Value = Format(Val(lbl_Pcs_BeamConsMtrs.Text), "#########0.00")
            .Rows(Rw).Cells(31).Value = Trim(cbo_Pcs_LastPiece_Status.Text)
            .Rows(Rw).Cells(32).Value = Trim(cbo_Pcs_ClothName.Text)
            .Rows(Rw).Cells(33).Value = Trim(lbl_Pcs_EndsCount.Text)
            .Rows(Rw).Cells(34).Value = Trim(lbl_Pcs_WeftCount.Text)
        End With
    End Sub

    Private Sub btn_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim I As Integer = 0
        Dim J As Integer = 0
        Dim n As Integer = 0
        Dim NewCode As String = ""
        Dim Wev_ID As Integer = 0
        Dim clth_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim LotCd As String = ""
        Dim LotNo As String = ""
        Dim WagesCode As String = ""
        Dim SQL1 As String
        Dim vTOTPCS As String = 0
        Dim vTOTRECMTRS As String = 0



        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Val(txt_Pcs_No.Text) = 0 And Trim(txt_Pcs_No.Text) = "0" Then
            MessageBox.Show("Invalid Piece No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Pcs_No.Enabled Then txt_Pcs_No.Focus()
            Exit Sub
        End If


        'If Val(lbl_RecCode.Text) = 0 Then
        '    MessageBox.Show("Invalid Lot No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
        '    Exit Sub
        'End If

        If Trim(lbl_Pcs_PackSlipNo1.Text) <> "" Or Trim(lbl_Pcs_PackSlipNo2.Text) <> "" Or Trim(lbl_Pcs_PackSlipNo3.Text) <> "" Or Trim(lbl_Pcs_PackSlipNo4.Text) <> "" Or Trim(lbl_Pcs_PackSlipNo5.Text) <> "" Then
            MessageBox.Show("Packing Slip Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Pcs_No.Enabled And txt_Pcs_No.Visible Then txt_Pcs_No.Focus()
            Exit Sub
        End If

        'If Trim(UCase(txt_Pcs_No.Text)) = Trim(Val(txt_Pcs_No.Text)) Then
        '    MessageBox.Show("Can not Delete this Piece, Delete it from Cloth Receipt", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If txt_Pcs_No.Enabled And txt_Pcs_No.Visible Then txt_Pcs_No.Focus()
        '    Exit Sub
        'End If

        'Wev_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        'If Wev_ID = 0 Then
        '    MessageBox.Show("Invalid Weaver Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
        '    Exit Sub
        'End If

        'clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        'If clth_ID = 0 Then
        '    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
        '    Exit Sub
        'End If


        If Trim(UCase(txt_Pcs_No.Text)) <> Trim(UCase(txt_Pcs_No.Tag)) Then
            MessageBox.Show("Invalid Piece No and its Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Pcs_No.Enabled Then txt_Pcs_No.Focus()
            Exit Sub
        End If

        'If Trim(Val(txt_Pcs_No.Text)) = Trim(txt_Pcs_No.Text) Then
        '    If Val(txt_Pcs_RecMtrs.Text) = 0 Then
        '        MessageBox.Show("Invalid Piece Receipt Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If txt_Pcs_RecMtrs.Enabled Then txt_Pcs_RecMtrs.Focus()
        '        Exit Sub
        '    End If
        'End If

        'Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_Pcs_LoomNo.Text)
        'If Val(txt_Pcs_RecMtrs.Text) <> 0 Then
        '    If Val(Lm_ID) = 0 Then
        '        MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
        '        Exit Sub
        '    End If
        '    If Trim(lbl_Pcs_WidthType.Text) = "" Then
        '        MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
        '        Exit Sub
        '    End If
        'End If
        'If Val(txt_Pcs_RecMtrs.Text) = 0 Then
        '    If Val(Lm_ID) <> 0 Then
        '        MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
        '        Exit Sub
        '    End If
        'End If

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        cmd.Connection = con


        WagesCode = ""
        LotCd = ""
        LotNo = ""

        Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
            End If
        End If
        Dt1.Clear()


        SQL1 = "select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and Piece_No = '" & Trim(txt_Pcs_No.Text) & "' and (Type1_Meters <> 0 or Type2_Meters <> 0 or Type3_Meters <> 0 or Type4_Meters <> 0 or Type5_Meters <> 0 )"
        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Piece Checking prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        SQL1 = "select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and Piece_No = '" & Trim(txt_Pcs_No.Text) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '')"
        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Packing Slip prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        cmd.Connection = con

        tr = con.BeginTransaction

        Try

            cmd.Transaction = tr

            SQL1 = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "'  and Piece_No = '" & Trim(txt_Pcs_No.Text) & "' and Weaver_Piece_Checking_Code = '' and (Type1_Meters+Type2_Meters+Type3_Meters+Type4_Meters+Type5_Meters) = 0  and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = ''"
            cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            cmd.ExecuteNonQuery()



            vTOTPCS = 0
            vTOTRECMTRS = 0

            SQL1 = "select COUNT(Piece_No) as TotPcs, sum(Receipt_Meters) as TotRecMeters from Weaver_ClothReceipt_Piece_Details Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and Piece_No = '" & Trim(txt_Pcs_No.Text) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '')"
            cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    vTOTPCS = Val(Dt1.Rows(0)(0).ToString)
                End If
                If IsDBNull(Dt1.Rows(0)(1).ToString) = False Then
                    vTOTRECMTRS = Val(Dt1.Rows(0)(1).ToString)
                End If
            End If
            Dt1.Clear()

            '------ HEAD Updation
            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Total_Receipt_Pcs = " & Str(Val(vTOTPCS)) & ", noof_pcs = " & Str(Val(vTOTPCS)) & ", ReceiptMeters_Receipt = " & Str(Val(vTOTRECMTRS)) & ", Total_Receipt_Meters = " & Str(Val(vTOTRECMTRS)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            move_record(lbl_RefNo.Text)

            MessageBox.Show("Piece Deleted Sucessfully!!!", "FOR DELETING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            Call Clear_PcsDetails()

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If txt_Pcs_No.Enabled And txt_Pcs_No.Visible Then txt_Pcs_No.Focus()

        End Try

    End Sub

    Private Sub btn_Clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Clear.Click
        Clear_PcsDetails()
        If txt_Pcs_No.Enabled And txt_Pcs_No.Visible Then txt_Pcs_No.Focus()
    End Sub

    Private Sub btn_Select_KnottingDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Select_KnottingDetails.Click
        Dim Lm_ID As Integer

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_Pcs_LoomNo.Text)
        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
            Exit Sub
        End If

        get_Beam_KnottingDetails()

        txt_Pcs_No.Tag = txt_Pcs_No.Text
        cbo_Pcs_LoomNo.Tag = cbo_Pcs_LoomNo.Text
        cbo_Pcs_LastPiece_Status.Tag = cbo_Pcs_LastPiece_Status.Text


    End Sub

    Private Sub get_Beam_KnottingDetails()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Lm_ID As Integer
        Dim Clo_ID As Integer
        Dim NewCode As String = ""
        Dim SQL1 As String
        Dim vCRIMPPERC As String = 0
        Dim vPRODMTRS As String = 0



        cmd.Connection = con

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_Pcs_LoomNo.Text)
        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
            Exit Sub
        End If

        'Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_Quality.Text)
        'If Val(Clo_ID) = 0 Then
        '    MessageBox.Show("Invalid Quality", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
        '    Exit Sub
        'End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        lbl_Pcs_KnotCode.Text = ""
        lbl_Pcs_KnotNo.Text = ""

        cbo_Pcs_ClothName.Text = ""
        lbl_Pcs_EndsCount.Text = ""
        lbl_Pcs_WeftCount.Text = ""

        lbl_Pcs_WidthType.Text = ""
        lbl_Pcs_CrimpPerc.Text = ""

        lbl_Pcs_SetCode1.Text = ""
        lbl_Pcs_SetNo1.Text = ""
        lbl_Pcs_BeamNo1.Text = ""

        lbl_Pcs_SetCode2.Text = ""
        lbl_Pcs_SetNo2.Text = ""
        lbl_Pcs_BeamNo2.Text = ""

        lbl_Pcs_Beam_TotMtrs1.Text = ""
        lbl_Pcs_Beam_BalMtrs1.Text = ""

        lbl_Pcs_BeamConsMtrs.Text = ""

        get_Doffing_PcsDetails()

        If Trim(lbl_Pcs_KnotCode.Text) = "" Then

            Da3 = New SqlClient.SqlDataAdapter("select top 1 a.*, b.cloth_name, d.endscount_Name, e.count_name as weft_countname from Beam_Knotting_Head a  INNER JOIN Cloth_Head b ON a.Cloth_Idno1 = b.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN count_Head e ON b.Cloth_WeftCount_IdNo = e.Count_IdNo Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = '' Order by a.Beam_Knotting_Date desc, a.for_OrderBy desc, a.Beam_Knotting_Code desc", con)
            Dt3 = New DataTable
            Da3.Fill(Dt3)
            If Dt3.Rows.Count > 0 Then

                lbl_Pcs_KnotCode.Text = Dt3.Rows(0).Item("Beam_Knotting_Code").ToString
                lbl_Pcs_KnotNo.Text = Dt3.Rows(0).Item("Beam_Knotting_No").ToString

                cbo_Pcs_ClothName.Text = Dt3.Rows(0).Item("cloth_name").ToString
                lbl_Pcs_EndsCount.Text = Dt3.Rows(0).Item("endscount_Name").ToString
                lbl_Pcs_WeftCount.Text = Dt3.Rows(0).Item("weft_countname").ToString

                lbl_Pcs_WidthType.Text = Dt3.Rows(0).Item("Width_Type").ToString

                lbl_Pcs_SetCode1.Text = Dt3.Rows(0).Item("Set_Code1").ToString
                lbl_Pcs_SetNo1.Text = Dt3.Rows(0).Item("Set_No1").ToString
                lbl_Pcs_BeamNo1.Text = Dt3.Rows(0).Item("Beam_No1").ToString

                lbl_Pcs_SetCode2.Text = Dt3.Rows(0).Item("Set_Code2").ToString
                lbl_Pcs_SetNo2.Text = Dt3.Rows(0).Item("Set_No2").ToString
                lbl_Pcs_BeamNo2.Text = Dt3.Rows(0).Item("Beam_No2").ToString

                lbl_Pcs_Beam_TotMtrs1.Text = ""
                lbl_Pcs_Beam_BalMtrs1.Text = ""

                'If Trim(lbl_Pcs_SetCode1.Text) <> "" And Trim(lbl_Pcs_BeamNo1.Text) <> "" Then

                '    cmd.Connection = con
                '    cmd.CommandType = CommandType.StoredProcedure
                '    cmd.CommandText = "SP_get_Beam_Details_from_SizedPavu_Processing_Details"
                '    cmd.Parameters.Add("@setcode", SqlDbType.VarChar)
                '    cmd.Parameters("@setcode").Value = Trim(lbl_Pcs_SetCode1.Text)
                '    cmd.Parameters.Add("@beamno", SqlDbType.VarChar)
                '    cmd.Parameters("@beamno").Value = Trim(lbl_Pcs_BeamNo1.Text)

                '    Da4 = New SqlClient.SqlDataAdapter(cmd)
                '    'Da4 = New SqlClient.SqlDataAdapter("Select Meters, Production_Meters from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_Pcs_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_Pcs_BeamNo1.Text) & "'", con)
                '    Dt4 = New DataTable
                '    Da4.Fill(Dt4)
                '    If Dt4.Rows.Count > 0 Then
                '        lbl_Pcs_Beam_TotMtrs1.Text = Dt4.Rows(0).Item("Meters").ToString

                '        'lbl_Pcs_Beam_BalMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                '        vCRIMPPERC = Common_Procedures.Calculation_CrimpPercentage_On_BEAMRUNOUT(con, lbl_Pcs_SetCode1.Text, lbl_Pcs_BeamNo1.Text, Val(lbl_Pcs_Beam_TotMtrs1.Text), vPRODMTRS)
                '        lbl_Pcs_Beam_BalMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(vPRODMTRS), "#########0.00")


                '    End If
                '    Dt4.Clear()
                'End If

                lbl_Pcs_CrimpPerc.Text = ""
                Da4 = New SqlClient.SqlDataAdapter("Select * from Cloth_Head Where Cloth_IdNo = " & Str(Val(Clo_ID)), con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    lbl_Pcs_CrimpPerc.Text = Dt4.Rows(0).Item("Crimp_Percentage").ToString
                End If
                Dt4.Clear()

                'Calculation_Beam_ConsumptionPavu()

            End If
            Dt3.Clear()

        End If

        txt_Pcs_No.Tag = txt_Pcs_No.Text
        cbo_Pcs_LoomNo.Tag = cbo_Pcs_LoomNo.Text
        cbo_Pcs_LastPiece_Status.Tag = cbo_Pcs_LastPiece_Status.Text

        Dt1.Dispose()
        Da1.Dispose()

        Dt2.Dispose()
        Da2.Dispose()

        Dt3.Dispose()
        Da3.Dispose()

        Dt4.Dispose()
        Da4.Dispose()

    End Sub

    Private Sub txt_Pcs_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Pcs_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(txt_Pcs_No.Text) <> "" And (Trim(UCase(txt_Pcs_No.Text)) <> Trim(UCase(txt_Pcs_No.Tag)) Or (Val(txt_Pcs_RecMtrs.Text) = 0 And Val(txt_Pcs_No.Tag) = 0)) Then
                btn_Select_PcsDetails_Click(sender, e)
                cbo_Pcs_LoomNo.Focus()
            Else
                cbo_Pcs_LoomNo.Focus()
            End If
        End If
    End Sub

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        With dgv_Details

            If Trim(.CurrentRow.Cells(0).Value) <> "" Then

                txt_Pcs_No.Text = Trim(UCase(.CurrentRow.Cells(0).Value))
                txt_Pcs_No.Tag = txt_Pcs_No.Text


                btn_Select_PcsDetails_Click(sender, e)

            End If

        End With

        If txt_Pcs_No.Enabled And txt_Pcs_No.Visible Then txt_Pcs_No.Focus()

    End Sub

    Private Sub txt_Pcs_RecMtrs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Pcs_RecMtrs.TextChanged
        Calculation_Pcs_TotalMeter()
        Calculation_Beam_ConsumptionPavu()
    End Sub

    Private Sub txt_Pcs_Type1Mtrs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Pcs_Type1Mtrs.TextChanged
        Calculation_Pcs_TotalMeter()
    End Sub

    Private Sub txt_Pcs_Type2Mtrs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Pcs_Type2Mtrs.TextChanged
        Calculation_Pcs_TotalMeter()
    End Sub

    Private Sub txt_Pcs_Type3Mtrs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Pcs_Type3Mtrs.TextChanged
        Calculation_Pcs_TotalMeter()
    End Sub

    Private Sub txt_Pcs_Type4Mtrs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Pcs_Type4Mtrs.TextChanged
        Calculation_Pcs_TotalMeter()
    End Sub

    Private Sub txt_Pcs_Type5Mtrs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Pcs_Type5Mtrs.TextChanged
        Calculation_Pcs_TotalMeter()
    End Sub

    Private Sub txt_Pcs_Weight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Pcs_Weight.KeyDown
        If e.KeyValue = 38 Then
            txt_Pcs_Type4Mtrs.Focus()
            e.Handled = True
            e.SuppressKeyPress = True
        End If
        If e.KeyValue = 40 Then
            btn_Add.Focus()
            e.Handled = True
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub txt_Pcs_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Pcs_Weight.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Pcs_Weight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Pcs_Weight.TextChanged
        Calculation_Pcs_TotalMeter()
    End Sub

    Private Sub Calculation_Beam_ConsumptionPavu()
        'Dim CloID As Integer
        'Dim ConsPavu As Single
        'Dim LmID As Integer
        'Dim NoofBeams As Integer = 0

        'CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        'LmID = Common_Procedures.Loom_NameToIdNo(con, cbo_Pcs_LoomNo.Text)

        'ConsPavu = Common_Procedures.get_Pavu_Consumption(con, CloID, LmID, Val(txt_Pcs_RecMtrs.Text), Trim(lbl_Pcs_WidthType.Text), , Val(lbl_Pcs_CrimpPerc.Text))

        'NoofBeams = 0
        'If Trim(lbl_Pcs_BeamNo1.Text) <> "" And Trim(lbl_Pcs_BeamNo2.Text) <> "" Then
        '    NoofBeams = 2
        'Else
        '    NoofBeams = 1
        'End If
        'If Val(NoofBeams) = 0 Then NoofBeams = 1

        'lbl_Pcs_BeamConsMtrs.Text = Format(Val(ConsPavu) / NoofBeams, "#########0.00")
    End Sub

    Private Sub btn_Select_PcsDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Select_PcsDetails.Click
        Dim PcNo As String

        If Val(txt_Pcs_No.Text) = 0 And Trim(txt_Pcs_No.Text) = "0" Then
            MessageBox.Show("Invalid Piece No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Pcs_No.Enabled Then txt_Pcs_No.Focus()
            Exit Sub
        End If

        PcNo = txt_Pcs_No.Text
        Call Clear_PcsDetails()

        txt_Pcs_No.Text = PcNo
        txt_Pcs_No.Tag = Trim(txt_Pcs_No.Text)

        get_Doffing_PcsDetails()

        If txt_Pcs_No.Enabled And txt_Pcs_No.Visible Then txt_Pcs_No.Focus()

    End Sub

    Private Sub get_Doffing_PcsDetails()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Clo_Pck As Single, Clo_Wdth As Single
        Dim PcNo As String = ""
        Dim LockSTS As Boolean = False
        Dim SQL1 As String = ""
        Dim vCRIMPPERC As String = 0
        Dim vPRODMTRS As String = 0
        Dim NewCode As String = ""


        If Val(txt_Pcs_No.Text) = 0 And Trim(txt_Pcs_No.Text) = "0" Then
            MessageBox.Show("Invalid Piece No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Pcs_No.Enabled Then txt_Pcs_No.Focus()
            Exit Sub
        End If

        'If Trim(lbl_RecCode.Text) = "" Then
        '    MessageBox.Show("Invalid LotNo", "INVALID LOT SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus()
        '    Exit Sub
        'End If


        LockSTS = False


        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        cmd.Connection = con

        SQL1 = "Select a.*, b.*, c.Loom_Name, d.endscount_Name, e.count_name as Weft_Countname from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo INNER JOIN Loom_Head c ON a.Loom_IdNo = c.Loom_IdNo  INNER JOIN EndsCount_Head d ON b.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN count_Head e ON b.Cloth_WeftCount_IdNo = e.Count_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'  and a.Piece_No = '" & Trim(txt_Pcs_No.Text) & "' Order by a.sl_no"
        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        Da1 = New SqlClient.SqlDataAdapter(cmd)
        'Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.*, c.Loom_Name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo LEFT OUTER JOIN Loom_Head c ON a.Loom_IdNo = c.Loom_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "'  and a.Piece_No = '" & Trim(txt_Pcs_No.Text) & "' Order by a.sl_no", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            Clo_Pck = Val(Dt1.Rows(0).Item("Cloth_Pick").ToString)
            Clo_Wdth = Val(Dt1.Rows(0).Item("Cloth_Width").ToString)

            cbo_Pcs_ClothName.Text = Dt1.Rows(0).Item("cloth_name").ToString
            lbl_Pcs_EndsCount.Text = Dt1.Rows(0).Item("endscount_Name").ToString
            lbl_Pcs_WeftCount.Text = Dt1.Rows(0).Item("Weft_Countname").ToString

            If Val(Dt1.Rows(0).Item("Receipt_Meters").ToString) <> 0 Then
                txt_Pcs_RecMtrs.Text = Dt1.Rows(0).Item("Receipt_Meters").ToString
            Else
                txt_Pcs_RecMtrs.Text = ""
            End If
            If Val(Dt1.Rows(0).Item("Pick").ToString) <> 0 Then
                txt_Pcs_Pick.Text = Val(Dt1.Rows(0).Item("Pick").ToString)
            Else
                If Val(Dt1.Rows(0).Item("Cloth_Pick").ToString) <> 0 Then
                    txt_Pcs_Pick.Text = Val(Dt1.Rows(0).Item("Cloth_Pick").ToString)
                End If
            End If
            If Val(Dt1.Rows(0).Item("Width").ToString) <> 0 Then
                txt_Pcs_Width.Text = Val(Dt1.Rows(0).Item("Width").ToString)
            Else
                If Val(Dt1.Rows(0).Item("Cloth_Width").ToString) <> 0 Then
                    txt_Pcs_Width.Text = Val(Dt1.Rows(0).Item("Cloth_Width").ToString)
                End If
            End If
            If Val(Dt1.Rows(0).Item("Type1_Meters").ToString) <> 0 Then
                txt_Pcs_Type1Mtrs.Text = Format(Val(Dt1.Rows(0).Item("Type1_Meters").ToString), "#########0.00")
            End If
            If Val(Dt1.Rows(0).Item("Type2_Meters").ToString) <> 0 Then
                txt_Pcs_Type2Mtrs.Text = Format(Val(Dt1.Rows(0).Item("Type2_Meters").ToString), "#########0.00")
            End If
            If Val(Dt1.Rows(0).Item("Type3_Meters").ToString) <> 0 Then
                txt_Pcs_Type3Mtrs.Text = Format(Val(Dt1.Rows(0).Item("Type3_Meters").ToString), "#########0.00")
            End If
            If Val(Dt1.Rows(0).Item("Type4_Meters").ToString) <> 0 Then
                txt_Pcs_Type4Mtrs.Text = Format(Val(Dt1.Rows(0).Item("Type4_Meters").ToString), "#########0.00")
            End If
            If Val(Dt1.Rows(0).Item("Type5_Meters").ToString) <> 0 Then
                txt_Pcs_Type5Mtrs.Text = Format(Val(Dt1.Rows(0).Item("Type5_Meters").ToString), "#########0.00")
            End If
            If (Val(Dt1.Rows(0).Item("Type1_Meters").ToString) + Val(Dt1.Rows(0).Item("Type2_Meters").ToString) + Val(Dt1.Rows(0).Item("Type3_Meters").ToString) + Val(Dt1.Rows(0).Item("Type4_Meters").ToString) + Val(Dt1.Rows(0).Item("Type5_Meters").ToString)) <> 0 Then
                lbl_Pcs_TotalMtrs.Text = Format(Val(Dt1.Rows(0).Item("Type1_Meters").ToString) + Val(Dt1.Rows(0).Item("Type2_Meters").ToString) + Val(Dt1.Rows(0).Item("Type3_Meters").ToString) + Val(Dt1.Rows(0).Item("Type4_Meters").ToString) + Val(Dt1.Rows(0).Item("Type5_Meters").ToString), "#########0.00")
            End If
            If Val(Dt1.Rows(0).Item("Weight").ToString) <> 0 Then
                txt_Pcs_Weight.Text = Format(Val(Dt1.Rows(0).Item("Weight").ToString), "#########0.000")
            End If
            If Val(Dt1.Rows(0).Item("Weight_Meter").ToString) <> 0 Then
                lbl_Pcs_Wgt_Mtr.Text = Format(Val(Dt1.Rows(0).Item("Weight_Meter").ToString), "#########0.000")
            End If

            cbo_Pcs_LoomNo.Text = Dt1.Rows(0).Item("Loom_Name").ToString
            lbl_Pcs_WidthType.Text = Dt1.Rows(0).Item("Width_Type").ToString
            lbl_Pcs_KnotCode.Text = Dt1.Rows(0).Item("Beam_Knotting_Code").ToString
            lbl_Pcs_KnotNo.Text = Dt1.Rows(0).Item("Beam_Knotting_No").ToString
            lbl_Pcs_SetCode1.Text = Dt1.Rows(0).Item("Set_Code1").ToString
            lbl_Pcs_SetNo1.Text = Dt1.Rows(0).Item("Set_No1").ToString
            lbl_Pcs_BeamNo1.Text = Dt1.Rows(0).Item("Beam_No1").ToString

            lbl_Pcs_SetCode2.Text = Dt1.Rows(0).Item("Set_Code2").ToString
            lbl_Pcs_SetNo2.Text = Dt1.Rows(0).Item("Set_No2").ToString
            lbl_Pcs_BeamNo2.Text = Dt1.Rows(0).Item("Beam_No2").ToString



            lbl_Pcs_Beam_TotMtrs1.Text = ""
            lbl_Pcs_Beam_BalMtrs1.Text = ""

            'If Val(Dt1.Rows(0).Item("Balance_Meters1").ToString) <> 0 Then
            '    lbl_Pcs_Beam_BalMtrs1.Text = Format(Val(Dt1.Rows(0).Item("Balance_Meters1").ToString), "#########0.00")
            'End If
            'If Trim(lbl_Pcs_SetCode1.Text) <> "" And Trim(lbl_Pcs_BeamNo1.Text) <> "" Then

            '    cmd.Connection = con
            '    cmd.CommandType = CommandType.StoredProcedure
            '    cmd.CommandText = "SP_get_Beam_Details_from_SizedPavu_Processing_Details"
            '    cmd.Parameters.Add("@setcode", SqlDbType.VarChar)
            '    cmd.Parameters("@setcode").Value = Trim(lbl_Pcs_SetCode1.Text)
            '    cmd.Parameters.Add("@beamno", SqlDbType.VarChar)
            '    cmd.Parameters("@beamno").Value = Trim(lbl_Pcs_BeamNo1.Text)

            '    Da1 = New SqlClient.SqlDataAdapter(cmd)
            '    'Da1 = New SqlClient.SqlDataAdapter("Select Meters, Production_Meters from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_Pcs_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_Pcs_BeamNo1.Text) & "'", con)
            '    Dt2 = New DataTable
            '    Da1.Fill(Dt2)
            '    If Dt2.Rows.Count > 0 Then
            '        lbl_Pcs_Beam_TotMtrs1.Text = Dt2.Rows(0).Item("Meters").ToString
            '        'lbl_Pcs_Beam_BalMtrs1.Text = Format(Val(Dt2.Rows(0).Item("Meters").ToString) - Val(Dt2.Rows(0).Item("Production_Meters").ToString), "#########0.00")
            '        vPRODMTRS = 0
            '        vCRIMPPERC = Common_Procedures.Calculation_CrimpPercentage_On_BEAMRUNOUT(con, lbl_Pcs_SetCode1.Text, lbl_Pcs_BeamNo1.Text, Val(lbl_Pcs_Beam_TotMtrs1.Text), vPRODMTRS)
            '        lbl_Pcs_Beam_BalMtrs1.Text = Format(Val(lbl_Pcs_Beam_TotMtrs1.Text) - Val(vPRODMTRS), "#########0.00")

            '    End If
            '    Dt2.Clear()
            'End If

            lbl_Pcs_CrimpPerc.Text = 0 ' Format(Val(Dt1.Rows(0).Item("Crimp_Percentage").ToString), "#########0.00")
            lbl_Pcs_BeamConsMtrs.Text = 0 ' Format(Val(Dt1.Rows(0).Item("BeamConsumption_Meters").ToString), "#########0.00")

            lbl_Pcs_PackSlipNo1.Text = Dt1.Rows(0).Item("PackingSlip_Code_Type1").ToString
            If Trim(lbl_Pcs_PackSlipNo1.Text) = "" Then lbl_Pcs_PackSlipNo1.Text = Dt1.Rows(0).Item("BuyerOffer_Code_Type1").ToString
            lbl_Pcs_PackSlipNo2.Text = Dt1.Rows(0).Item("PackingSlip_Code_Type2").ToString
            If Trim(lbl_Pcs_PackSlipNo2.Text) = "" Then lbl_Pcs_PackSlipNo2.Text = Dt1.Rows(0).Item("BuyerOffer_Code_Type2").ToString
            lbl_Pcs_PackSlipNo3.Text = Dt1.Rows(0).Item("PackingSlip_Code_Type3").ToString
            If Trim(lbl_Pcs_PackSlipNo3.Text) = "" Then lbl_Pcs_PackSlipNo3.Text = Dt1.Rows(0).Item("BuyerOffer_Code_Type3").ToString
            lbl_Pcs_PackSlipNo4.Text = Dt1.Rows(0).Item("PackingSlip_Code_Type4").ToString
            If Trim(lbl_Pcs_PackSlipNo4.Text) = "" Then lbl_Pcs_PackSlipNo4.Text = Dt1.Rows(0).Item("BuyerOffer_Code_Type4").ToString
            lbl_Pcs_PackSlipNo5.Text = Dt1.Rows(0).Item("PackingSlip_Code_Type5").ToString
            If Trim(lbl_Pcs_PackSlipNo5.Text) = "" Then lbl_Pcs_PackSlipNo5.Text = Dt1.Rows(0).Item("BuyerOffer_Code_Type5").ToString

            cbo_Pcs_LastPiece_Status.Text = Dt1.Rows(0).Item("Is_LastPiece").ToString

            If Val(lbl_Pcs_TotalMtrs.Text) <> 0 Then
                cbo_Pcs_LoomNo.Enabled = False
                cbo_Pcs_ClothName.Enabled = False
                txt_Pcs_RecMtrs.Enabled = False
                LockSTS = True
            End If

            If Trim(lbl_Pcs_PackSlipNo1.Text) <> "" Then
                txt_Pcs_Type1Mtrs.Enabled = False
                LockSTS = True
            End If
            If Trim(lbl_Pcs_PackSlipNo2.Text) <> "" Then
                txt_Pcs_Type2Mtrs.Enabled = False
                LockSTS = True
            End If
            If Trim(lbl_Pcs_PackSlipNo3.Text) <> "" Then
                txt_Pcs_Type3Mtrs.Enabled = False
                LockSTS = True
            End If
            If Trim(lbl_Pcs_PackSlipNo4.Text) <> "" Then
                txt_Pcs_Type4Mtrs.Enabled = False
                LockSTS = True
            End If
            If Trim(lbl_Pcs_PackSlipNo5.Text) <> "" Then
                txt_Pcs_Type5Mtrs.Enabled = False
                LockSTS = True
            End If

            txt_Folding.Enabled = True
            If LockSTS = True Then
                txt_Folding.Enabled = False
            End If

            txt_Pcs_RecMtrs.Enabled = True
            Da1 = New SqlClient.SqlDataAdapter("Select a.Weaver_Wages_Code from Weaver_Cloth_Receipt_Head a Where a.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
            Dt2 = New DataTable
            Da1.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                If IsDBNull(Dt2.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                    If Trim(Dt2.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Then
                        txt_Pcs_RecMtrs.Enabled = False
                    End If
                End If
            End If
            Dt2.Clear()

        End If
        Dt1.Clear()


        'If Val(txt_Pcs_Pick.Text) = 0 Then
        '    Da1 = New SqlClient.SqlDataAdapter("Select cloth_pick, cloth_width from cloth_head where cloth_name = '" & Trim(lbl_Quality.Text) & "'", con)
        '    Dt2 = New DataTable
        '    Da1.Fill(Dt2)
        '    If Dt2.Rows.Count > 0 Then
        '        txt_Pcs_Pick.Text = Val(Dt2.Rows(0).Item("Cloth_Pick").ToString)
        '        txt_Pcs_Width.Text = Val(Dt2.Rows(0).Item("Cloth_Width").ToString)
        '        If Val(txt_Pcs_Pick.Text) = 0 Then txt_Pcs_Pick.Text = ""
        '        If Val(txt_Pcs_Width.Text) = 0 Then txt_Pcs_Width.Text = ""
        '    End If
        '    Dt2.Clear()
        'End If


    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Exit Sub
    End Sub


    Private Sub cbo_Pcs_LastPiece_Status_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Pcs_LastPiece_Status.LostFocus
        If Trim(cbo_Pcs_LastPiece_Status.Text) = "" Then
            cbo_Pcs_LastPiece_Status.Text = "NO"
        ElseIf Trim(cbo_Pcs_LastPiece_Status.Text) <> "YES" And Trim(cbo_Pcs_LastPiece_Status.Text) <> "NO" Then
            cbo_Pcs_LastPiece_Status.Text = "NO"
        End If
    End Sub

    Private Sub btn_BeamSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BeamSelection.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim SQL1 As String = ""
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim Led_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim NewCode As String = ""
        Dim EntKnotCode As String = ""
        Dim n As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim SNo As Integer = 0

        cmd.Connection = con


        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
            Exit Sub
        End If

        'Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        'If Val(Clo_ID) = 0 Then
        '    MessageBox.Show("Invalid Quality", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
        '    Exit Sub
        'End If

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_Pcs_LoomNo.Text)
        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Pcs_LoomNo.Enabled Then cbo_Pcs_LoomNo.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        EntKnotCode = ""

        SNo = 0
        dgv_KnottingSelection.Rows.Clear()


        SQL1 = "Select a.*, b.Loom_Name, d.EndsCount_Name from Weaver_ClothReceipt_Piece_Details tW INNER JOIN Beam_Knotting_Head a ON tW.Beam_Knotting_Code <> '' and tW.Beam_Knotting_Code = a.Beam_Knotting_Code INNER JOIN Loom_Head b ON a.Loom_IdNo <> 0 and a.Loom_IdNo = b.Loom_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = d.EndsCount_IdNo Where tW.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tW.Ledger_IdNo = " & Str(Val(Led_ID)) & " and tW.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and tW.Lot_Code = '" & Trim(NewCode) & "' and tW.Loom_IdNo = " & Str(Val(Lm_ID)) & " and tW.Piece_No = '" & Trim(txt_Pcs_No.Text) & "' Order by a.Beam_Knotting_Date Desc, a.for_OrderBy Desc, a.Beam_Knotting_Code Desc"
        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        Da1 = New SqlClient.SqlDataAdapter(cmd)
        'Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.Loom_Name, d.EndsCount_Name from Weaver_ClothReceipt_Piece_Details tW INNER JOIN Beam_Knotting_Head a ON tW.Beam_Knotting_Code <> '' and tW.Beam_Knotting_Code = a.Beam_Knotting_Code INNER JOIN Loom_Head b ON a.Loom_IdNo <> 0 and a.Loom_IdNo = b.Loom_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = d.EndsCount_IdNo Where tW.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tW.Ledger_IdNo = " & Str(Val(Led_ID)) & " and tW.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and tW.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "' and tW.cloth_IdNo = " & Str(Val(Clo_ID)) & " and tW.Loom_IdNo = " & Str(Val(Lm_ID)) & " and tW.Piece_No = '" & Trim(txt_Pcs_No.Text) & "' Order by a.Beam_Knotting_Date Desc, a.for_OrderBy Desc, a.Beam_Knotting_Code Desc", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then

            n = dgv_KnottingSelection.Rows.Add()

            SNo = SNo + 1
            dgv_KnottingSelection.Rows(n).Cells(0).Value = Val(SNo)
            dgv_KnottingSelection.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Beam_Knotting_Date").ToString), "dd-MM-yyyy")
            dgv_KnottingSelection.Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_Knotting_No").ToString
            dgv_KnottingSelection.Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Loom_Name").ToString
            dgv_KnottingSelection.Rows(n).Cells(4).Value = cbo_Pcs_ClothName.Text
            dgv_KnottingSelection.Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
            dgv_KnottingSelection.Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Set_No1").ToString
            dgv_KnottingSelection.Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_No1").ToString
            dgv_KnottingSelection.Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Beam_No2").ToString


            If Trim(Dt1.Rows(i).Item("Set_Code1").ToString) <> "" And Trim(Dt1.Rows(i).Item("Beam_No1").ToString) <> "" Then
                Da4 = New SqlClient.SqlDataAdapter("Select Meters, Production_Meters from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(i).Item("Beam_No1").ToString) & "'", con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    dgv_KnottingSelection.Rows(n).Cells(9).Value = Dt4.Rows(0).Item("Meters").ToString
                    dgv_KnottingSelection.Rows(n).Cells(10).Value = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")

                End If
                Dt4.Clear()
            End If

            dgv_KnottingSelection.Rows(n).Cells(11).Value = "1"
            dgv_KnottingSelection.Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString

            For j = 0 To dgv_KnottingSelection.ColumnCount - 1
                dgv_KnottingSelection.Rows(n).Cells(j).Style.ForeColor = Color.Red
            Next

            EntKnotCode = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString

        End If
        Dt1.Clear()


        Da1 = New SqlClient.SqlDataAdapter("select a.*, b.Loom_Name, d.EndsCount_Name from Beam_Knotting_Head a INNER JOIN Loom_Head b ON a.Loom_IdNo <> 0 and a.Loom_IdNo = b.Loom_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = d.EndsCount_IdNo Where a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and (a.Cloth_IdNo1 = " & Str(Val(Clo_ID)) & " or a.Cloth_IdNo2 = " & Str(Val(Clo_ID)) & " or a.Cloth_IdNo3 = " & Str(Val(Clo_ID)) & ") and a.Beam_Knotting_Code <> '" & Trim(EntKnotCode) & "' Order by a.Beam_Knotting_Date Desc, a.for_OrderBy Desc, a.Beam_Knotting_Code Desc", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then

            For i = 0 To Dt1.Rows.Count - 1

                n = dgv_KnottingSelection.Rows.Add()

                SNo = SNo + 1
                dgv_KnottingSelection.Rows(n).Cells(0).Value = Val(SNo)
                dgv_KnottingSelection.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Beam_Knotting_Date").ToString), "dd-MM-yyyy")
                dgv_KnottingSelection.Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_Knotting_No").ToString
                dgv_KnottingSelection.Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Loom_Name").ToString
                dgv_KnottingSelection.Rows(n).Cells(4).Value = cbo_Pcs_ClothName.Text
                dgv_KnottingSelection.Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                dgv_KnottingSelection.Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Set_No1").ToString
                dgv_KnottingSelection.Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_No1").ToString
                dgv_KnottingSelection.Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Beam_No2").ToString

                If Trim(Dt1.Rows(i).Item("Set_Code1").ToString) <> "" And Trim(Dt1.Rows(i).Item("Beam_No1").ToString) <> "" Then
                    Da4 = New SqlClient.SqlDataAdapter("Select Meters, Production_Meters from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(i).Item("Beam_No1").ToString) & "'", con)
                    Dt4 = New DataTable
                    Da4.Fill(Dt4)
                    If Dt4.Rows.Count > 0 Then
                        dgv_KnottingSelection.Rows(n).Cells(9).Value = Dt4.Rows(0).Item("Meters").ToString
                        dgv_KnottingSelection.Rows(n).Cells(10).Value = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                    End If
                    Dt4.Clear()
                End If

                dgv_KnottingSelection.Rows(n).Cells(11).Value = ""
                dgv_KnottingSelection.Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Beam_Knotting_Code").ToString

                For j = 0 To dgv_KnottingSelection.ColumnCount - 1
                    dgv_KnottingSelection.Rows(n).Cells(j).Style.ForeColor = Color.Black
                Next

            Next

        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da1.Dispose()

        pnl_KnottingSelection.Visible = True
        pnl_Back.Enabled = False

        If dgv_KnottingSelection.Rows.Count = 0 Then
            dgv_KnottingSelection.Rows.Add()
        End If
        If dgv_KnottingSelection.Rows.Count > 0 Then
            If dgv_KnottingSelection.Enabled And dgv_KnottingSelection.Visible Then
                dgv_KnottingSelection.Focus()
                dgv_KnottingSelection.CurrentCell = dgv_KnottingSelection.Rows(0).Cells(0)
            End If
        End If

    End Sub


    Private Sub Select_Knotting(ByVal RwIndx As Integer)
        Dim i As Integer
        Dim j As Integer

        With dgv_KnottingSelection

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

    Private Sub dgv_KnottingSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_KnottingSelection.KeyDown
        Dim n As Integer

        Try

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then

                If dgv_KnottingSelection.Rows.Count > 0 Then

                    If dgv_KnottingSelection.CurrentCell.RowIndex >= 0 Then

                        n = dgv_KnottingSelection.CurrentCell.RowIndex

                        Select_Knotting(n)

                        e.Handled = True

                    End If

                End If

            End If

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub dgv_KnottingSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KnottingSelection.CellClick
        Select_Knotting(e.RowIndex)
    End Sub

    Private Sub dgv_KnottingSelection_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KnottingSelection.CellDoubleClick
        Select_Knotting(e.RowIndex)
        Close_Beam_Selection()
    End Sub

    Private Sub btn_Close_KnottingSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_KnottingSelection.Click
        Close_Beam_Selection()
    End Sub

    Private Sub Close_Beam_Selection()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim KnotCode As String = ""
        Dim vCRIMPPERC As String = 0
        Dim vPRODMTRS As String = 0

        Cmd.Connection = con


        KnotCode = ""
        For i = 0 To dgv_KnottingSelection.RowCount - 1
            If Val(dgv_KnottingSelection.Rows(i).Cells(11).Value) = 1 Then
                KnotCode = dgv_KnottingSelection.Rows(i).Cells(12).Value
                Exit For
            End If
        Next

        Da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_Name, c.Crimp_Percentage, d.EndsCount_Name, e.Count_Name, f.Loom_name from Beam_Knotting_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 <> 0 and a.Cloth_IdNo1 = c.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Count_Head e ON c.Cloth_WeftCount_IdNo <> 0 and c.Cloth_WeftCount_IdNo = e.Count_IdNo INNER JOIN Loom_Head f ON a.Loom_IdNo <> 0 and a.Loom_IdNo = f.Loom_IdNo Where a.Beam_Knotting_Code = '" & Trim(KnotCode) & "'", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then

            lbl_Pcs_KnotCode.Text = Dt1.Rows(0).Item("Beam_Knotting_Code").ToString
            lbl_Pcs_KnotNo.Text = Dt1.Rows(0).Item("Beam_Knotting_No").ToString
            lbl_Pcs_WidthType.Text = Dt1.Rows(0).Item("Width_Type").ToString

            lbl_Pcs_SetCode1.Text = Dt1.Rows(0).Item("Set_Code1").ToString
            lbl_Pcs_SetNo1.Text = Dt1.Rows(0).Item("Set_No1").ToString
            lbl_Pcs_BeamNo1.Text = Dt1.Rows(0).Item("Beam_No1").ToString

            lbl_Pcs_SetCode2.Text = Dt1.Rows(0).Item("Set_Code2").ToString
            lbl_Pcs_SetNo2.Text = Dt1.Rows(0).Item("Set_No2").ToString
            lbl_Pcs_BeamNo2.Text = Dt1.Rows(0).Item("Beam_No2").ToString

            lbl_Pcs_Beam_TotMtrs1.Text = ""
            lbl_Pcs_Beam_BalMtrs1.Text = ""

            If Trim(lbl_Pcs_SetCode1.Text) <> "" And Trim(lbl_Pcs_BeamNo1.Text) <> "" Then

                Cmd.Connection = con
                Cmd.CommandType = CommandType.StoredProcedure
                Cmd.CommandText = "SP_get_Beam_Details_from_SizedPavu_Processing_Details"
                Cmd.Parameters.Add("@setcode", SqlDbType.VarChar)
                Cmd.Parameters("@setcode").Value = Trim(lbl_Pcs_SetCode1.Text)
                Cmd.Parameters.Add("@beamno", SqlDbType.VarChar)
                Cmd.Parameters("@beamno").Value = Trim(lbl_Pcs_BeamNo1.Text)

                Da4 = New SqlClient.SqlDataAdapter(Cmd)
                'Da4 = New SqlClient.SqlDataAdapter("Select Meters, Production_Meters from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_Pcs_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_Pcs_BeamNo1.Text) & "'", con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    lbl_Pcs_Beam_TotMtrs1.Text = Dt4.Rows(0).Item("Meters").ToString


                    'lbl_Pcs_Beam_BalMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt4.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                    vPRODMTRS = 0
                    vCRIMPPERC = Common_Procedures.Calculation_CrimpPercentage_On_BEAMRUNOUT(con, lbl_Pcs_SetCode1.Text, lbl_Pcs_BeamNo1.Text, Val(lbl_Pcs_Beam_TotMtrs1.Text), vPRODMTRS)
                    lbl_Pcs_Beam_BalMtrs1.Text = Format(Val(lbl_Pcs_Beam_TotMtrs1.Text) - Val(vPRODMTRS), "#########0.00")


                End If
                Dt4.Clear()
            End If

            lbl_Pcs_CrimpPerc.Text = Dt1.Rows(0).Item("Crimp_Percentage").ToString

        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da1.Dispose()

        Calculation_Beam_ConsumptionPavu()

        pnl_Back.Enabled = True
        pnl_KnottingSelection.Visible = False
        If txt_Pcs_Type1Mtrs.Enabled And txt_Pcs_Type1Mtrs.Visible Then txt_Pcs_Type1Mtrs.Focus()

    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_Weaver.Focus()
        End If
    End Sub
    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
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

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_date.Focus()
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
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_Weaver.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
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

    Private Sub txt_Pcs_RecMtrs_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Pcs_RecMtrs.KeyDown
        If e.KeyValue = 38 Then
            cbo_Pcs_ClothName.Focus()
            e.Handled = True
            e.SuppressKeyPress = True
        End If
        If e.KeyValue = 40 Then
            btn_Add.Focus()
            e.Handled = True
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub txt_Pcs_RecMtrs_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Pcs_RecMtrs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        Calculation_Pcs_TotalMeter()
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub cbo_Pcs_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Pcs_ClothName.GotFocus


        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_idno = 0)")

        'Dim Da4 As New SqlClient.SqlDataAdapter
        'Dim Dt4 As New DataTable
        'Dim Lm_ID As Integer = 0
        'Dim Clo_ID1 As Integer = 0
        'Dim Clo_ID2 As Integer = 0
        'Dim Clo_ID3 As Integer = 0
        'Dim Clo_ID4 As Integer = 0
        'Dim Clo_Wrp_ID1 As Integer = 0, Clo_Wrp_ID2 As Integer = 0, Clo_Wrp_ID3 As Integer = 0, Clo_Wrp_ID4 As Integer = 0
        'Dim Clo_Wft_ID1 As Integer = 0, Clo_Wft_ID2 As Integer = 0, Clo_Wft_ID3 As Integer = 0, Clo_Wft_ID4 As Integer = 0
        'Dim Clo_Reed1 As Integer = 0, Clo_Reed2 As Integer = 0, Clo_Reed3 As Integer = 0, Clo_Reed4 As Integer = 0
        'Dim Clo_Width1 As Integer = 0, Clo_Width2 As Integer = 0, Clo_Width3 As Integer = 0, Clo_Width4 As Integer = 0

        'Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_Pcs_LoomNo.Text)

        'Clo_ID1 = 0 : Clo_ID2 = 0 : Clo_ID3 = 0 : Clo_ID4 = 0
        'Clo_Wrp_ID1 = 0 : Clo_Wrp_ID2 = 0 : Clo_Wrp_ID3 = 0 : Clo_Wrp_ID4 = 0
        'Clo_Wft_ID1 = 0 : Clo_Wft_ID2 = 0 : Clo_Wft_ID3 = 0 : Clo_Wft_ID4 = 0
        'Clo_Reed1 = 0 : Clo_Reed2 = 0 : Clo_Reed3 = 0 : Clo_Reed4 = 0
        'Clo_Width1 = 0 : Clo_Width2 = 0 : Clo_Width3 = 0 : Clo_Width4 = 0

        'Da4 = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code = '" & Trim(lbl_Pcs_KnotCode.Text) & "'", con)
        'Dt4 = New DataTable
        'Da4.Fill(Dt4)
        'If Dt4.Rows.Count > 0 Then
        '    Clo_ID1 = Val(Dt4.Rows(0).Item("Cloth_Idno1").ToString)
        '    Clo_ID2 = Val(Dt4.Rows(0).Item("Cloth_Idno2").ToString)
        '    Clo_ID3 = Val(Dt4.Rows(0).Item("Cloth_Idno3").ToString)
        '    Clo_ID4 = Val(Dt4.Rows(0).Item("Cloth_Idno4").ToString)
        'End If

        'Dt4.Clear()
        'Dt4.Dispose()
        'Da4.Dispose()

        'Clo_Wrp_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
        'Clo_Wrp_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
        'Clo_Wrp_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
        'Clo_Wrp_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

        'Clo_Wft_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
        'Clo_Wft_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
        'Clo_Wft_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
        'Clo_Wft_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

        'Clo_Reed1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
        'Clo_Reed2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
        'Clo_Reed3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
        'Clo_Reed4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

        'Clo_Width1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
        'Clo_Width2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
        'Clo_Width3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
        'Clo_Width4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & "  or Cloth_idno = " & Str(Val(Clo_ID4)) & "  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID1)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID1)) & " and Cloth_Reed = " & Str(Val(Clo_Reed1)) & " and Cloth_Width = " & Str(Val(Clo_Width1)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID2)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID2)) & " and Cloth_Reed = " & Str(Val(Clo_Reed2)) & " and Cloth_Width = " & Str(Val(Clo_Width2)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID3)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID3)) & " and Cloth_Reed = " & Str(Val(Clo_Reed3)) & " and Cloth_Width = " & Str(Val(Clo_Width3)) & " ) or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID4)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID4)) & " and Cloth_Reed = " & Str(Val(Clo_Reed4)) & " and Cloth_Width = " & Str(Val(Clo_Width4)) & " ) )", "(Cloth_idno = 0)")

        cbo_Pcs_ClothName.Tag = cbo_Pcs_ClothName.Text




    End Sub

    Private Sub cbo_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Pcs_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_Pcs_LoomNo, txt_Pcs_RecMtrs, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_idno = 0)")

        'Dim Da4 As New SqlClient.SqlDataAdapter
        'Dim Dt4 As New DataTable
        'Dim Lm_ID As Integer = 0
        'Dim Clo_ID1 As Integer = 0
        'Dim Clo_ID2 As Integer = 0
        'Dim Clo_ID3 As Integer = 0
        'Dim Clo_ID4 As Integer = 0
        'Dim Clo_Wrp_ID1 As Integer = 0, Clo_Wrp_ID2 As Integer = 0, Clo_Wrp_ID3 As Integer = 0, Clo_Wrp_ID4 As Integer = 0
        'Dim Clo_Wft_ID1 As Integer = 0, Clo_Wft_ID2 As Integer = 0, Clo_Wft_ID3 As Integer = 0, Clo_Wft_ID4 As Integer = 0
        'Dim Clo_Reed1 As Integer = 0, Clo_Reed2 As Integer = 0, Clo_Reed3 As Integer = 0, Clo_Reed4 As Integer = 0
        'Dim Clo_Width1 As Integer = 0, Clo_Width2 As Integer = 0, Clo_Width3 As Integer = 0, Clo_Width4 As Integer = 0


        'Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_Pcs_LoomNo.Text)

        'Clo_ID1 = 0 : Clo_ID2 = 0 : Clo_ID3 = 0 : Clo_ID4 = 0
        'Clo_Wrp_ID1 = 0 : Clo_Wrp_ID2 = 0 : Clo_Wrp_ID3 = 0 : Clo_Wrp_ID4 = 0
        'Clo_Wft_ID1 = 0 : Clo_Wft_ID2 = 0 : Clo_Wft_ID3 = 0 : Clo_Wft_ID4 = 0
        'Clo_Reed1 = 0 : Clo_Reed2 = 0 : Clo_Reed3 = 0 : Clo_Reed4 = 0
        'Clo_Width1 = 0 : Clo_Width2 = 0 : Clo_Width3 = 0 : Clo_Width4 = 0

        'Da4 = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code = '" & Trim(lbl_Pcs_KnotCode.Text) & "'", con)
        'Dt4 = New DataTable
        'Da4.Fill(Dt4)
        'If Dt4.Rows.Count > 0 Then
        '    Clo_ID1 = Val(Dt4.Rows(0).Item("Cloth_Idno1").ToString)
        '    Clo_ID2 = Val(Dt4.Rows(0).Item("Cloth_Idno2").ToString)
        '    Clo_ID3 = Val(Dt4.Rows(0).Item("Cloth_Idno3").ToString)
        '    Clo_ID4 = Val(Dt4.Rows(0).Item("Cloth_Idno4").ToString)
        'End If

        'Dt4.Clear()
        'Dt4.Dispose()

        'Da4.Dispose()


        'Clo_Wrp_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
        'Clo_Wrp_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
        'Clo_Wrp_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
        'Clo_Wrp_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

        'Clo_Wft_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
        'Clo_Wft_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
        'Clo_Wft_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
        'Clo_Wft_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

        'Clo_Reed1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
        'Clo_Reed2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
        'Clo_Reed3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
        'Clo_Reed4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

        'Clo_Width1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
        'Clo_Width2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
        'Clo_Width3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
        'Clo_Width4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Pcs_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & "  or Cloth_idno = " & Str(Val(Clo_ID4)) & "  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID1)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID1)) & " and Cloth_Reed = " & Str(Val(Clo_Reed1)) & " and Cloth_Width = " & Str(Val(Clo_Width1)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID2)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID2)) & " and Cloth_Reed = " & Str(Val(Clo_Reed2)) & " and Cloth_Width = " & Str(Val(Clo_Width2)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID3)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID3)) & " and Cloth_Reed = " & Str(Val(Clo_Reed3)) & " and Cloth_Width = " & Str(Val(Clo_Width3)) & " ) or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID4)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID4)) & " and Cloth_Reed = " & Str(Val(Clo_Reed4)) & " and Cloth_Width = " & Str(Val(Clo_Width4)) & " ) )", "(Cloth_idno = 0)")

        'If (e.KeyCode = 38 And cbo_Pcs_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyCode = 38) Then
        '    cbo_Pcs_LoomNo.Focus()

        'ElseIf (e.KeyValue = 40 And cbo_Pcs_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        '    txt_Pcs_RecMtrs.Focus()

        'End If

    End Sub

    Private Sub cbo_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Pcs_ClothName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Pcs_ClothName.Tag = "----------------------"
        End If

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_Pcs_RecMtrs, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_idno = 0)")

        'Dim Da4 As New SqlClient.SqlDataAdapter
        'Dim Dt4 As New DataTable
        'Dim Lm_ID As Integer = 0
        'Dim Clo_ID1 As Integer = 0
        'Dim Clo_ID2 As Integer = 0
        'Dim Clo_ID3 As Integer = 0
        'Dim Clo_ID4 As Integer = 0
        'Dim Clo_Wrp_ID1 As Integer = 0, Clo_Wrp_ID2 As Integer = 0, Clo_Wrp_ID3 As Integer = 0, Clo_Wrp_ID4 As Integer = 0
        'Dim Clo_Wft_ID1 As Integer = 0, Clo_Wft_ID2 As Integer = 0, Clo_Wft_ID3 As Integer = 0, Clo_Wft_ID4 As Integer = 0
        'Dim Clo_Reed1 As Integer = 0, Clo_Reed2 As Integer = 0, Clo_Reed3 As Integer = 0, Clo_Reed4 As Integer = 0
        'Dim Clo_Width1 As Integer = 0, Clo_Width2 As Integer = 0, Clo_Width3 As Integer = 0, Clo_Width4 As Integer = 0

        'Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_Pcs_LoomNo.Text)

        'Clo_ID1 = 0 : Clo_ID2 = 0 : Clo_ID3 = 0 : Clo_ID4 = 0
        'Clo_Wrp_ID1 = 0 : Clo_Wrp_ID2 = 0 : Clo_Wrp_ID3 = 0 : Clo_Wrp_ID4 = 0
        'Clo_Wft_ID1 = 0 : Clo_Wft_ID2 = 0 : Clo_Wft_ID3 = 0 : Clo_Wft_ID4 = 0
        'Clo_Reed1 = 0 : Clo_Reed2 = 0 : Clo_Reed3 = 0 : Clo_Reed4 = 0
        'Clo_Width1 = 0 : Clo_Width2 = 0 : Clo_Width3 = 0 : Clo_Width4 = 0

        'Da4 = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_Knotting_Code = '" & Trim(lbl_Pcs_KnotCode.Text) & "'", con)
        'Dt4 = New DataTable
        'Da4.Fill(Dt4)
        'If Dt4.Rows.Count > 0 Then
        '    Clo_ID1 = Val(Dt4.Rows(0).Item("Cloth_Idno1").ToString)
        '    Clo_ID2 = Val(Dt4.Rows(0).Item("Cloth_Idno2").ToString)
        '    Clo_ID3 = Val(Dt4.Rows(0).Item("Cloth_Idno3").ToString)
        '    Clo_ID4 = Val(Dt4.Rows(0).Item("Cloth_Idno4").ToString)
        'End If
        'Dt4.Clear()
        'Dt4.Dispose()
        'Da4.Dispose()

        'Clo_Wrp_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
        'Clo_Wrp_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
        'Clo_Wrp_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
        'Clo_Wrp_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WarpCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

        'Clo_Wft_ID1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
        'Clo_Wft_ID2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
        'Clo_Wft_ID3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
        'Clo_Wft_ID4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

        'Clo_Reed1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
        'Clo_Reed2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
        'Clo_Reed3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
        'Clo_Reed4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Reed", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

        'Clo_Width1 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID1)) & ")"))
        'Clo_Width2 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID2)) & ")"))
        'Clo_Width3 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID3)) & ")"))
        'Clo_Width4 = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_Width", "(cloth_idno = " & Str(Val(Clo_ID4)) & ")"))

        'If Asc(e.KeyChar) = 13 Then
        '    cbo_Pcs_ClothName.Tag = "----------------------"
        'End If

        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_Pcs_RecMtrs, "Cloth_Head", "Cloth_Name", "(Cloth_idno = 0 or Cloth_idno = " & Str(Val(Clo_ID1)) & " or Cloth_idno = " & Str(Val(Clo_ID2)) & " or Cloth_idno = " & Str(Val(Clo_ID3)) & "  or Cloth_idno = " & Str(Val(Clo_ID4)) & "  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID1)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID1)) & " and Cloth_Reed = " & Str(Val(Clo_Reed1)) & " and Cloth_Width = " & Str(Val(Clo_Width1)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID2)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID2)) & " and Cloth_Reed = " & Str(Val(Clo_Reed2)) & " and Cloth_Width = " & Str(Val(Clo_Width2)) & " )  or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID3)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID3)) & " and Cloth_Reed = " & Str(Val(Clo_Reed3)) & " and Cloth_Width = " & Str(Val(Clo_Width3)) & " )   or ( Cloth_WarpCount_IdNo = " & Str(Val(Clo_Wrp_ID4)) & " and Cloth_WeftCount_IdNo = " & Str(Val(Clo_Wft_ID4)) & " and Cloth_Reed = " & Str(Val(Clo_Reed4)) & " and Cloth_Width = " & Str(Val(Clo_Width4)) & " ) )", "(Cloth_idno = 0)")


    End Sub



    Private Sub cbo_ClothName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Pcs_ClothName.LostFocus
        Dim Clo_IdNo As Integer = 0
        Dim wftcnt_idno As Integer = 0

        If Trim(UCase(cbo_Pcs_ClothName.Tag)) <> Trim(UCase(cbo_Pcs_ClothName.Text)) Then
            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Pcs_ClothName.Text)

            wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
            lbl_Pcs_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, wftcnt_idno)

        End If
    End Sub

End Class
