Imports System.Drawing.Printing
Imports System.IO
Public Class Lot_ApprovAL_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "LOTAP-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_BobinDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_KuriDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String

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

    Private vMOVREC_STS As Boolean = False
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private prn_Status As Integer = 0

    Private Print_PDF_Status As Boolean = False
    Private EMAIL_Status As Boolean = False
    Private WHATSAPP_Status As Boolean = False
    Private vEMAIL_Attachment_FileName As String


    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        Dim dttm As Date

        chk_approved_sts.Checked = True
        txt_remarks.Text = ""

        New_Entry = False
        Insert_Entry = False
        vMOVREC_STS = False

        pnl_Back.Enabled = True

        cbo_LotCodeSelection.Text = ""
        cbo_LotCodeSelection.Tag = cbo_LotCodeSelection.Text

        cbo_LotCodeSelection.Enabled = False
        btn_List_PieceDetails.Enabled = False

        lbl_dc_receipt_pcs.Text = ""
        lbl_dc_receipt_meters.Text = ""
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        msk_date.Text = ""
        dtp_Date.Text = ""
        lbl_Yarn.Text = ""
        lbl_Pavu.Text = ""

        dtp_Date.Enabled = True ' False
        msk_date.Enabled = True ' False

        'If dtp_Date.Enabled = False Then
        '    dttm = New DateTime(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4), 4, 1)
        '    dttm = DateAdd(DateInterval.Day, -1, dttm)
        '    dtp_Date.Text = dttm
        'End If

        lbl_WeaverName.Text = ""
        lbl_ClothName.Text = ""
        lbl_EndsCount.Text = ""

        txt_NoOfPcs.Text = ""

        txt_PcsNoFrom.Text = "1"

        lbl_PcsNoTo.Text = ""
        txt_ReceiptMeters.Text = ""


        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))


        dgv_Details.Rows.Clear()
        dgv_Details.AllowUserToAddRows = False

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()


        dgv_Details.ReadOnly = False
        dgv_Details.Columns(0).ReadOnly = True
        dgv_Details.Columns(1).ReadOnly = False
        dgv_Details.Columns(2).ReadOnly = True
        'dgv_Details.Columns(3).ReadOnly = True
        dgv_Details.Columns(4).ReadOnly = True
        dgv_Details.Columns(5).ReadOnly = True
        chk_approved_sts.Enabled = True

        cbo_Grid_BeamNo1.Visible = False
        cbo_Grid_BeamNo2.Visible = False
        cbo_Grid_BeamNo1.Text = ""
        cbo_Grid_BeamNo2.Text = ""

        txt_NoOfPcs.Enabled = True
        btn_Design_PieceDetails_Grid.Enabled = True

        dgv_ActiveCtrl_Name = ""


    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        Dim chk As CheckBox

        On Error Resume Next

        If FrmLdSTS = True Or vMOVREC_STS = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Or TypeOf Me.ActiveControl Is CheckBox Then
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
        ElseIf TypeOf Me.ActiveControl Is CheckBox Then
            chk = Me.ActiveControl
            chk.Focus()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_BeamNo1.Name Then
            cbo_Grid_BeamNo1.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_BeamNo2.Name Then
            cbo_Grid_BeamNo2.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If FrmLdSTS = True Or vMOVREC_STS = True Then Exit Sub

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String = ""
        Dim vLOTCODE1 As String = ""
        Dim n, I, J As Integer
        Dim LockSTS As Boolean = False
        Dim vPCSCHK_APPSTS As String = 0
        Dim SQL1 As String = ""
        Dim cmd As New SqlClient.SqlCommand
        Dim vCLORECCODE As String = ""
        Dim vLOTCODE As String = ""
        Dim vLOTNO As String = ""
        Dim vRCPT_PKCONDT As String = ""
        Dim vPCSCHKCODE As String = ""
        Dim vWAGESCODE As String = ""
        Dim vLOTALOT_STS As Boolean = False
        Dim vAPP_PCSENT_STS As Boolean = False
        Dim vAPP_DEFECTENT_STS As Boolean = False


        If Val(no) = 0 Then Exit Sub

        clear()

        vMOVREC_STS = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da1 = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from lot_Approved_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Lot_approved_Code ='" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("lot_Approved_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("lot_Approved_date")
                msk_date.Text = dtp_Date.Text
                cbo_LotCodeSelection.Text = dt1.Rows(0).Item("lotcode_forSelection").ToString
                cbo_LotCodeSelection.Tag = cbo_LotCodeSelection.Text

                lbl_ClothName.Text = dt1.Rows(0).Item("Cloth_Name").ToString


                txt_NoOfPcs.Text = Val(dt1.Rows(0).Item("noof_pcs").ToString)
                txt_PcsNoFrom.Text = dt1.Rows(0).Item("pcs_fromno").ToString
                lbl_PcsNoTo.Text = dt1.Rows(0).Item("pcs_tono").ToString
                txt_ReceiptMeters.Text = dt1.Rows(0).Item("Receipt_Meters").ToString

                lbl_dc_receipt_pcs.Text = dt1.Rows(0).Item("Receipt_DC_Pcs").ToString
                lbl_dc_receipt_meters.Text = dt1.Rows(0).Item("Receipt_DC_Meters").ToString
                txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                If Val(dt1.Rows(0).Item("Approved_sts").ToString) = 1 Then chk_approved_sts.Checked = True

                dgv_Details.Rows.Clear()
                For I = Val(txt_PcsNoFrom.Text) To Val(lbl_PcsNoTo.Text)
                    n = dgv_Details.Rows.Add()
                    dgv_Details.Rows(n).Cells(0).Value = I
                Next I

                lbl_WeaverName.Text = ""
                lbl_EndsCount.Text = ""
                vLOTCODE1 = ""
                da2 = New SqlClient.SqlDataAdapter("select a.Weaver_ClothReceipt_Code, c.ledger_name as weavername, d.EndsCount_Name from Weaver_Cloth_Receipt_head a INNER JOIN ledger_Head c ON a.ledger_idno = c.ledger_idno  INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where a.lotcode_forSelection = '" & Trim(cbo_LotCodeSelection.Text) & "'", con)
                'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, c.ledger_name as weavername from Weaver_Cloth_Receipt_head a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno INNER JOIN ledger_Head c ON a.ledger_idno = c.ledger_idno  where a.lotcode_forSelection = '" & Trim(cbo_LotCodeSelection.Text) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    lbl_WeaverName.Text = dt2.Rows(0).Item("weavername").ToString
                    lbl_EndsCount.Text = dt2.Rows(0).Item("EndsCount_Name").ToString
                    vLOTCODE1 = dt2.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
                End If
                dt2.Clear()

                vLOTNO = ""
                vLOTCODE = ""
                vWAGESCODE = ""
                vPCSCHKCODE = ""
                vLOTALOT_STS = False
                vAPP_PCSENT_STS = False
                vAPP_DEFECTENT_STS = False

                da2 = New SqlClient.SqlDataAdapter("select a.Weaver_ClothReceipt_No, a.Weaver_ClothReceipt_Code, a.Weaver_Piece_Checking_Code, a.Weaver_Wages_Code, a.StockOff_IdNo, a.WareHouse_IdNo from Weaver_Cloth_Receipt_Head a   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_ForSelection = '" & Trim(cbo_LotCodeSelection.Text) & "' ", con)
                dt2.Clear()
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    vLOTNO = dt2.Rows(0).Item("Weaver_ClothReceipt_No").ToString
                    vLOTCODE = dt2.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
                    vPCSCHKCODE = dt2.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                    vWAGESCODE = dt2.Rows(0).Item("Weaver_Wages_Code").ToString
                Else
                    MessageBox.Show("Invalid Lot No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    cbo_LotCodeSelection.Focus()
                    Exit Sub
                End If
                dt2.Clear()

                LockSTS = False



                cmd.Connection = con
                cmd.CommandTimeout = 2000
                SQL1 = "Select a.Piece_No, a.Receipt_Meters, a.Weaver_Piece_Checking_Code, a.Type1_Meters, a.Type2_Meters, a.Type3_Meters, a.Type4_Meters, a.Type5_Meters, a.loom_No, a.BeamNo_SetCode_forSelection, a.BeamNo2_SetCode_forSelection from Weaver_ClothReceipt_Piece_Details a where a.Lot_Code = '" & Trim(vLOTCODE1) & "' Order by a.Sl_No, a.Piece_No"
                'SQL1 = "Select a.Piece_No, a.Receipt_Meters, a.Weaver_Piece_Checking_Code, a.Type1_Meters, a.Type2_Meters, a.Type3_Meters, a.Type4_Meters, a.Type5_Meters, a.loom_No, a.BeamNo_SetCode_forSelection, a.BeamNo2_SetCode_forSelection from Weaver_ClothReceipt_Piece_Details a, Weaver_Cloth_Receipt_Head b where b.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.lotcode_FORSelection = '" & Trim(cbo_LotCodeSelection.Text) & "' and  a.Weaver_ClothReceipt_Code = 'WCLRC-' + b.Weaver_ClothReceipt_Code and a.Lot_Code = b.Weaver_ClothReceipt_Code Order by a.Sl_No, a.Piece_No"
                cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                da2 = New SqlClient.SqlDataAdapter(cmd)
                'da2 = New SqlClient.SqlDataAdapter("Select a.Piece_No, a.Receipt_Meters, a.Weaver_Piece_Checking_Code, a.Type1_Meters, a.Type2_Meters, a.Type3_Meters, a.Type4_Meters, a.Type5_Meters, a.loom_No, a.BeamNo_SetCode_forSelection, a.BeamNo2_SetCode_forSelection from Weaver_ClothReceipt_Piece_Details a, Weaver_Cloth_Receipt_Head b where b.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.lotcode_FORSelection = '" & Trim(cbo_LotCodeSelection.Text) & "' and  a.Weaver_ClothReceipt_Code = 'WCLRC-' + b.Weaver_ClothReceipt_Code and a.Lot_Code = b.Weaver_ClothReceipt_Code Order by a.Sl_No, a.Piece_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        For J = 0 To dgv_Details.Rows.Count - 1

                            If Trim(UCase(dgv_Details.Rows(J).Cells(0).Value)) = Trim(UCase(dt2.Rows(I).Item("Piece_No").ToString)) Then

                                If Val(dgv_Details.Rows(J).Cells(1).Value) = 0 Then
                                    dgv_Details.Rows(J).Cells(1).Value = Format(Val(dt2.Rows(I).Item("Receipt_Meters").ToString), "########0.00")
                                End If
                                dgv_Details.Rows(J).Cells(2).Value = Val(dt2.Rows(I).Item("Type1_Meters").ToString) + Val(dt2.Rows(I).Item("Type2_Meters").ToString) + Val(dt2.Rows(I).Item("Type3_Meters").ToString) + Val(dt2.Rows(I).Item("Type4_Meters").ToString) + Val(dt2.Rows(I).Item("Type5_Meters").ToString)
                                If Val(dgv_Details.Rows(J).Cells(2).Value) = 0 Then
                                    dgv_Details.Rows(J).Cells(2).Value = ""
                                End If

                                dgv_Details.Rows(J).Cells(3).Value = dt2.Rows(I).Item("Loom_No").ToString
                                dgv_Details.Rows(J).Cells(4).Value = dt2.Rows(I).Item("BeamNo_SetCode_forSelection").ToString
                                dgv_Details.Rows(J).Cells(5).Value = dt2.Rows(I).Item("BeamNo2_SetCode_forSelection").ToString

                                If Val(dgv_Details.Rows(J).Cells(2).Value) <> 0 Then
                                    dgv_Details.Rows(J).Cells(0).ReadOnly = True
                                    dgv_Details.Rows(J).Cells(0).Style.BackColor = Color.LightGray
                                    dgv_Details.Rows(J).Cells(1).ReadOnly = True
                                    dgv_Details.Rows(J).Cells(1).Style.BackColor = Color.LightGray
                                    dgv_Details.Rows(J).Cells(2).ReadOnly = True
                                    dgv_Details.Rows(J).Cells(2).Style.BackColor = Color.LightGray
                                    'dgv_Details.Rows(J).Cells(3).ReadOnly = True
                                    'dgv_Details.Rows(J).Cells(3).Style.BackColor = Color.LightGray
                                    'dgv_Details.Rows(J).Cells(4).Style.BackColor = Color.LightGray
                                    'dgv_Details.Rows(J).Cells(5).Style.BackColor = Color.LightGray
                                    LockSTS = True

                                Else
                                    If Trim(vPCSCHKCODE) <> "" Then
                                        dgv_Details.Rows(J).Cells(2).ReadOnly = True
                                        dgv_Details.Rows(J).Cells(2).Style.BackColor = Color.Coral
                                    End If

                                End If
                                Exit For

                            End If

                        Next J

                    Next I

                End If
                dt2.Clear()

                vMOVREC_STS = False
                Total_Calculation()
                vMOVREC_STS = True





                da2 = New SqlClient.SqlDataAdapter("select count(a.Checking_Table_IdNo) from Lot_Allotment_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_ForSelection = '" & Trim(cbo_LotCodeSelection.Text) & "' and a.Checking_Table_IdNo <> 0 Having count(a.Checking_Table_IdNo) <> 0", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    vLOTALOT_STS = True
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select count(a.Piece_Checking_Defect_IdNo) from Weaver_ClothReceipt_App_Piece_Defect_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.LotCode_Selection = '" & Trim(cbo_LotCodeSelection.Text) & "' and a.Piece_Checking_Defect_IdNo <> 0  Having count(a.Piece_Checking_Defect_IdNo) <> 0", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    vAPP_DEFECTENT_STS = True
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select count(a.Piece_No) from Weaver_ClothReceipt_App_PieceChecking_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_Selection = '" & Trim(cbo_LotCodeSelection.Text) & "' and a.Piece_No <> '' and a.Total_Checking_Meters <> 0  Having count(a.Piece_No) <> 0", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    vAPP_PCSENT_STS = True
                End If
                dt2.Clear()

                If Trim(vWAGESCODE) <> "" Then
                    'dgv_Details.ReadOnly = True

                    LockSTS = True

                    For J = 0 To dgv_Details.Rows.Count - 1

                        dgv_Details.Rows(J).Cells(0).ReadOnly = True
                        dgv_Details.Rows(J).Cells(0).Style.BackColor = Color.LightGray
                        dgv_Details.Rows(J).Cells(1).ReadOnly = True
                        dgv_Details.Rows(J).Cells(1).Style.BackColor = Color.LightGray
                        'dgv_Details.Rows(J).Cells(3).ReadOnly = True
                        'dgv_Details.Rows(J).Cells(3).Style.BackColor = Color.LightGray
                        'dgv_Details.Rows(J).Cells(4).Style.BackColor = Color.LightGray
                        'dgv_Details.Rows(J).Cells(5).Style.BackColor = Color.LightGray

                    Next J

                End If

                If Trim(vPCSCHKCODE) <> "" Then

                    vPCSCHK_APPSTS = 0
                    da2 = New SqlClient.SqlDataAdapter("Select Approved_Status from Weaver_Piece_Checking_Head a where a.Weaver_Piece_Checking_Code = '" & Trim(vPCSCHKCODE) & "' ", con)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        vPCSCHK_APPSTS = dt2.Rows(0).Item("Approved_Status").ToString
                    End If
                    dt2.Clear()

                    If Val(vPCSCHK_APPSTS) = 1 Then

                        'dgv_Details.ReadOnly = True

                        LockSTS = True


                        For J = 0 To dgv_Details.Rows.Count - 1

                            dgv_Details.Rows(J).Cells(0).ReadOnly = True
                            dgv_Details.Rows(J).Cells(0).Style.BackColor = Color.LightGray
                            dgv_Details.Rows(J).Cells(1).ReadOnly = True
                            dgv_Details.Rows(J).Cells(1).Style.BackColor = Color.LightGray
                            'dgv_Details.Rows(J).Cells(3).ReadOnly = True
                            'dgv_Details.Rows(J).Cells(3).Style.BackColor = Color.LightGray
                            'dgv_Details.Rows(J).Cells(4).Style.BackColor = Color.LightGray
                            'dgv_Details.Rows(J).Cells(5).Style.BackColor = Color.LightGray

                        Next J

                    End If

                End If

                If vAPP_PCSENT_STS = True Then
                    chk_approved_sts.Enabled = True
                End If

                If vAPP_DEFECTENT_STS = True Then
                    chk_approved_sts.Enabled = True
                End If

            Else

                new_record()

            End If

            dt1.Clear()

            If LockSTS = True Then
                chk_approved_sts.Enabled = False
                txt_NoOfPcs.Enabled = False
                btn_Design_PieceDetails_Grid.Enabled = False
            End If

            Grid_Cell_DeSelect()

            vMOVREC_STS = False

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            vMOVREC_STS = False

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If cbo_LotCodeSelection.Visible And cbo_LotCodeSelection.Enabled Then cbo_LotCodeSelection.Focus()

        End Try

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Lot_ApprovAL_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
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

    Private Sub Lot_ApprovAL_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FrmLdSTS = True
        Me.Text = lbl_Heading.Text

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Common_Procedures.Update_SizedPavu_BeamNo_for_Selection(con)

        msk_date.Enabled = True
        dtp_Date.Enabled = True

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1146" Then '------ASHMITHA TEXTILE
            lbl_LotNoCaption.Visible = True
            lbl_LotNoCaption.Text = "Folding %"
        End If

        dtp_Date.Text = ""
        msk_date.Text = ""

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If


        AddHandler chk_approved_sts.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_approved_sts.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_LotCodeSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LotCodeSelection.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_NoOfPcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsNoFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ReceiptMeters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_NoOfPcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsNoFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ReceiptMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfPcs.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_ReceiptMeters.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_NoOfPcs.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_ReceiptMeters.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Lot_ApprovAL_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Lot_ApprovAL_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                Else
                    Close_Form()
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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

            Else
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then



                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(1).Value) = 0 Then

                                If txt_remarks.Enabled And txt_remarks.Visible Then
                                    txt_remarks.Focus()
                                Else
                                    chk_approved_sts.Focus()
                                End If

                            Else

                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    If txt_remarks.Enabled And txt_remarks.Visible Then
                                        txt_remarks.Focus()
                                    Else
                                        chk_approved_sts.Focus()
                                    End If

                                Else

                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(3)
                                    '.CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 1 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(3)

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                cbo_LotCodeSelection.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 3 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(1)

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
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = lbl_Heading.Text
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
                            Me.Text = lbl_Heading.Text & "   -   " & Trim(dt1.Rows(0)(1).ToString)
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult
        Dim vOrdByNo As String = ""


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Lot_Approval_Entry, New_Entry, Me, con, "Lot_Approved_Head", "Lot_Approved_code", NewCode, "Lot_Approved_Date", "(Lot_Approved_code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        Dim vCLORECCODE As String = ""
        Dim vLOTCODE As String = ""
        Dim vLOTNO As String = ""
        Dim vRCPT_PKCONDT As String = ""
        Dim vPCSCHKCODE As String = ""
        Dim vWAGESCODE As String = ""
        Dim vLOTALOT_STS As Boolean = False
        Dim vAPP_PCSENT_STS As Boolean = False
        Dim vAPP_DEFECTENT_STS As Boolean = False

        vLOTNO = ""
        vLOTCODE = ""
        vWAGESCODE = ""
        vPCSCHKCODE = ""
        vLOTALOT_STS = False
        vAPP_PCSENT_STS = False
        vAPP_DEFECTENT_STS = False

        Da2 = New SqlClient.SqlDataAdapter("select a.Weaver_ClothReceipt_No, a.Weaver_ClothReceipt_Code, a.Weaver_Piece_Checking_Code, a.Weaver_Wages_Code, a.StockOff_IdNo, a.WareHouse_IdNo from Weaver_Cloth_Receipt_Head a   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_ForSelection = '" & Trim(cbo_LotCodeSelection.Text) & "' ", con)
        Dt2.Clear()
        Da2.Fill(Dt2)
        If Dt2.Rows.Count > 0 Then
            vLOTNO = Dt2.Rows(0).Item("Weaver_ClothReceipt_No").ToString
            vLOTCODE = Dt2.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
            vPCSCHKCODE = Dt2.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
            vWAGESCODE = Dt2.Rows(0).Item("Weaver_Wages_Code").ToString
        Else
            MessageBox.Show("Invalid Lot No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_LotCodeSelection.Focus()
            Exit Sub
        End If
        Dt2.Clear()


        Da2 = New SqlClient.SqlDataAdapter("select count(a.Checking_Table_IdNo) from Lot_Allotment_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_ForSelection = '" & Trim(cbo_LotCodeSelection.Text) & "' and a.Checking_Table_IdNo <> 0 Having count(a.Checking_Table_IdNo) <> 0", con)
        Dt2.Clear()
        Da2.Fill(Dt2)
        If Dt2.Rows.Count > 0 Then
            vLOTALOT_STS = True
        End If
        Dt2.Clear()

        Da2 = New SqlClient.SqlDataAdapter("select count(a.Piece_Checking_Defect_IdNo) from Weaver_ClothReceipt_App_Piece_Defect_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.LotCode_Selection = '" & Trim(cbo_LotCodeSelection.Text) & "' and a.Piece_Checking_Defect_IdNo <> 0  Having count(a.Piece_Checking_Defect_IdNo) <> 0", con)
        Dt2.Clear()
        Da2.Fill(Dt2)
        If Dt2.Rows.Count > 0 Then
            vAPP_DEFECTENT_STS = True
        End If
        Dt2.Clear()

        Da2 = New SqlClient.SqlDataAdapter("select count(a.Piece_No) from Weaver_ClothReceipt_App_PieceChecking_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_Selection = '" & Trim(cbo_LotCodeSelection.Text) & "' and a.Piece_No <> '' and a.Total_Checking_Meters <> 0  Having count(a.Piece_No) <> 0", con)
        Dt2.Clear()
        Da2.Fill(Dt2)
        If Dt2.Rows.Count > 0 Then
            vAPP_PCSENT_STS = True
        End If
        Dt2.Clear()

        If Trim(vWAGESCODE) <> "" Then
            MessageBox.Show("Already Wages prepared", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_LotCodeSelection.Focus()
            Exit Sub
        End If

        If Trim(vPCSCHKCODE) <> "" Then
            MessageBox.Show("Already Piece checking prepared", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_LotCodeSelection.Focus()
            Exit Sub
        End If

        If Common_Procedures.User.IdNo <> 1 Then
            If vAPP_PCSENT_STS = True Then
                MessageBox.Show("Already Piece details entered", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                cbo_LotCodeSelection.Focus()
                Exit Sub
            End If

            If vAPP_DEFECTENT_STS = True Then
                MessageBox.Show("Already Piece defects entered", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                cbo_LotCodeSelection.Focus()
                Exit Sub
            End If

        End If


        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Lot_Approved_Head", "Lot_Approved_code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Lot_Approved_code, Company_IdNo, for_OrderBy", trans)
            'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Weaver_ClothReceipt_Piece_Details", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Piece_No,Main_PieceNo,PieceNo_OrderBy,ReceiptMeters_Receipt,Receipt_Meters, Create_Status ,StockOff_IdNo,WareHouse_IdNo", "Sl_No", "Weaver_ClothReceipt_Code, For_OrderBy, Company_IdNo, Weaver_ClothReceipt_No, Weaver_ClothReceipt_Date, Ledger_Idno", trans)

            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Lot_Approved_Status = 0, noof_pcs = DC_Receipt_Pcs, ReceiptMeters_Receipt = 0, Receipt_Meters = 0 Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and lotcode_forSelection = '" & Trim(cbo_LotCodeSelection.Text) & "' and Weaver_ClothReceipt_Code = '" & Trim(vLOTCODE) & "'"
            cmd.ExecuteNonQuery()

            vRCPT_PKCONDT = "WCLRC-"
            vCLORECCODE = Trim(vRCPT_PKCONDT) & Trim(vLOTCODE)

            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(vCLORECCODE) & "' and Lot_Code = '" & Trim(vLOTCODE) & "' and Create_Status = 2 and Weaver_Piece_Checking_Code = ''"
            'cmd.CommandText = "delete from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(vCLORECCODE) & "' and Lot_Code = '" & Trim(vLOTCODE) & "' "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Lot_Approved_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lot_Approved_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If cbo_LotCodeSelection.Enabled = True And cbo_LotCodeSelection.Visible = True Then cbo_LotCodeSelection.Focus()

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

            da = New SqlClient.SqlDataAdapter("select cloth_name from cloth_head order by cloth_name", con)
            da.Fill(dt2)
            cbo_Filter_Cloth.DataSource = dt2
            cbo_Filter_Cloth.DisplayMember = "cloth_name"



            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Cloth.Text = ""
            txt_Filter_RecNo.Text = ""
            txt_Filter_RecNoTo.Text = ""
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

            da = New SqlClient.SqlDataAdapter("select top 1 lot_Approved_No from lot_Approved_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and lot_Approved_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, lot_Approved_No", con)
            dt = New DataTable
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 lot_Approved_No from lot_Approved_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and lot_Approved_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, lot_Approved_No", con)
            dt = New DataTable
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 lot_Approved_No from lot_Approved_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and lot_Approved_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby desc, lot_Approved_No desc", con)
            dt = New DataTable
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

            da = New SqlClient.SqlDataAdapter("select top 1 lot_Approved_No from lot_Approved_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and lot_Approved_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby desc, lot_Approved_No desc", con)
            dt = New DataTable
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

            dtp_Date.Enabled = True ' False
            msk_date.Enabled = True ' False

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "lot_Approved_Head", "lot_Approved_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red
            cbo_LotCodeSelection.Enabled = True
            btn_List_PieceDetails.Enabled = True

            If cbo_LotCodeSelection.Enabled = True And cbo_LotCodeSelection.Visible = True Then cbo_LotCodeSelection.Focus()

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

            inpno = InputBox("Enter Lot.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select lot_Approved_No from lot_Approved_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and lot_Approved_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
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
                MessageBox.Show("Lot No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Lot_Approval_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Lot No.", "FOR NEW REC INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select lot_Approved_No from lot_Approved_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and lot_Approved_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
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
                    MessageBox.Show("Invalid Lot No", "DOES NOT INSERT NEW REC...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

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
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotRcptPcs As Single, vTotRcptMtrs As Double
        Dim WftCnt_ID As Integer = 0
        Dim EntID As String = 0
        Dim Dup_PcNo As String = ""
        Dim WagesCode As String = ""
        Dim PcsChkCode As String = ""
        Dim ClthName As String = ""
        Dim Nr As Integer = 0
        Dim vENDSCNT_ID As Integer = 0
        Dim KuriCnt_ID As Integer = 0
        Dim StkDelvTo_ID As Integer = 0, StkRecFrm_ID As Integer = 0
        Dim Led_type As String = ""
        Dim vStkOf_Pos_IdNo As Integer = 0, StkOff_ID As Integer = 0
        Dim Stock_In As String = ""
        Dim clthStock_In As String = 0
        Dim YrnCons_For As String = ""
        Dim mtrspcs As Single = 0
        Dim clthmtrspcs As Single = 0
        Dim clthPcs_Mtr As Single = 0
        Dim Purc_STS As Integer = 0
        Dim NoStkPos_Sts As Integer = 0
        Dim OurOrd_No As String = ""
        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
        Dim vGod_ID As Integer = 0
        Dim vDelv_ID As Integer = 0, vRec_ID As Integer = 0
        Dim Vchk_UNLOADED As Integer = 0
        Dim NoWeaWages_Bill_Sts As Integer = 0
        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""
        Dim vDat1 As Date = #1/1/2000#
        Dim vDat2 As Date = #2/2/2000#
        Dim Approved_STS As Integer = 0
        Dim vPCSCHK_APPSTS As String = 0
        Dim vCHCK_LOOM_NO As String = ""


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


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


        If msk_date.Visible = True Then

            If Trim(msk_date.Text) <> "" Then
                If Trim(msk_date.Text) <> "-  -" Then
                    If IsDate(msk_date.Text) = True Then
                        vDat1 = Convert.ToDateTime(msk_date.Text)
                    End If
                End If
            End If

            If Trim(dtp_Date.Text) <> "" Then
                If IsDate(dtp_Date.Text) = True Then
                    vDat2 = dtp_Date.Value.Date
                End If
            End If

            If IsDate(vDat1) = True And IsDate(vDat2) = True Then

                If DateDiff(DateInterval.Day, vDat1, vDat2) <> 0 Then

                    msk_date.Focus()

                    MessageBox.Show("Invalid Cloth Receipt Date", "DOES NOT SHOW REPORT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End If

        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Lot_Approval_Entry, New_Entry, Me, con, "Lot_Approved_Head", "Lot_Approved_Code", NewCode, "Lot_Approved_Date", "(Lot_Approved_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Lot_Approved_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Lot_Approved_no desc", dtp_Date.Value.Date) = False Then Exit Sub


        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_WeaverName.Text)
        If Led_ID = 0 Then
            If cbo_LotCodeSelection.Enabled And cbo_LotCodeSelection.Visible Then cbo_LotCodeSelection.Focus()
            MessageBox.Show("Invalid Weaver Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)
        If Clo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_LotCodeSelection.Enabled And cbo_LotCodeSelection.Visible Then cbo_LotCodeSelection.Focus()
            Exit Sub
        End If

        vENDSCNT_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount.Text)
        If vENDSCNT_ID = 0 Then
            MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_LotCodeSelection.Enabled And cbo_LotCodeSelection.Visible Then cbo_LotCodeSelection.Focus()
            Exit Sub
        End If

        'Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")")

        vStkOf_Pos_IdNo = 0

        'If Val(txt_NoOfPcs.Text) = 0 Then txt_NoOfPcs.Text = 1

        With dgv_Details

            For i = 0 To .RowCount - 1

                If (Trim(.Rows(i).Cells(0).Value) <> "" And Val(.Rows(i).Cells(0).Value) <> 0) Or Val(.Rows(i).Cells(1).Value) <> 0 Then

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

                    If Val(.Rows(i).Cells(1).Value) = 0 Then
                        MessageBox.Show("Invalid Pcs Meter", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .CurrentCell = .Rows(i).Cells(1)
                            .Focus()
                        End If
                        Exit Sub
                    End If

                    'If Val(.Rows(i).Cells(3).Value) = 0 Then
                    '    MessageBox.Show("Invalid Loom NO", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .CurrentCell = .Rows(i).Cells(3)
                    '        .Focus()
                    '    End If
                    '    Exit Sub
                    'End If

                    'If Trim(.Rows(i).Cells(4).Value) = "" And Trim(.Rows(i).Cells(5).Value) = "" Then
                    '    MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .CurrentCell = .Rows(i).Cells(4)
                    '        .Focus()
                    '    End If
                    '    Exit Sub
                    'End If

                End If

            Next

        End With

        Total_Calculation()

        vTotRcptPcs = 0 : vTotRcptMtrs = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotRcptPcs = Val(dgv_Details_Total.Rows(0).Cells(0).Value())
            vTotRcptMtrs = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
        End If

        If Val(txt_NoOfPcs.Text) = 0 Then
            MessageBox.Show("Invalid Receipt Pcs", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_NoOfPcs.Enabled And txt_NoOfPcs.Visible Then txt_NoOfPcs.Focus()
            Exit Sub
        End If


        If Val(txt_ReceiptMeters.Text) = 0 Then
            MessageBox.Show("Invalid Receipt Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dgv_Details.Rows.Count > 0 Then
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.Focus()
                End If
                Exit Sub
            End If
            Exit Sub
        End If

        Approved_STS = 0
        If chk_approved_sts.Checked = True Then Approved_STS = 1

        If Approved_STS = 0 Then
            MessageBox.Show("Invalid Approval, Lot not Approved", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If chk_approved_sts.Enabled And chk_approved_sts.Visible Then chk_approved_sts.Focus()
            Exit Sub
        End If

        Dim vCLORECCODE As String = ""
        Dim vLOTCODE As String = ""
        Dim vLOTNO As String = ""
        Dim vRCPT_PKCONDT As String = ""
        Dim vPCSCHKCODE As String = ""
        Dim vWAGESCODE As String = ""
        Dim vLOTALOT_STS As Boolean = False
        Dim vAPP_PCSENT_STS As Boolean = False
        Dim vAPP_DEFECTENT_STS As Boolean = False

        vLOTNO = ""
        vLOTCODE = ""
        vWAGESCODE = ""
        vPCSCHKCODE = ""
        vLOTALOT_STS = False
        vAPP_PCSENT_STS = False
        vAPP_DEFECTENT_STS = False
        vStkOf_Pos_IdNo = 0
        vGod_ID = 0

        da2 = New SqlClient.SqlDataAdapter("select a.Weaver_ClothReceipt_No, a.Weaver_ClothReceipt_Code, a.Weaver_Piece_Checking_Code, a.Weaver_Wages_Code, a.StockOff_IdNo, a.WareHouse_IdNo from Weaver_Cloth_Receipt_Head a   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_ForSelection = '" & Trim(cbo_LotCodeSelection.Text) & "' ", con)
        dt2.Clear()
        da2.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            vLOTNO = dt2.Rows(0).Item("Weaver_ClothReceipt_No").ToString
            vLOTCODE = dt2.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
            vPCSCHKCODE = dt2.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
            vWAGESCODE = dt2.Rows(0).Item("Weaver_Wages_Code").ToString
            vStkOf_Pos_IdNo = Val(dt2.Rows(0).Item("StockOff_IdNo").ToString)
            vGod_ID = Val(dt2.Rows(0).Item("WareHouse_IdNo").ToString)
        Else
            MessageBox.Show("Invalid Lot No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_LotCodeSelection.Focus()
            Exit Sub
        End If
        dt2.Clear()

        If vStkOf_Pos_IdNo = 0 Then vStkOf_Pos_IdNo = Common_Procedures.CommonLedger.Godown_Ac
        If vGod_ID = 0 Then vGod_ID = Common_Procedures.CommonLedger.Godown_Ac



        'If Trim(vWAGESCODE) <> "" Then
        '    MessageBox.Show("Already Weaver Wages prepared", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    cbo_LotCodeSelection.Focus()
        '    Exit Sub
        'End If

        'If Trim(vPCSCHKCODE) <> "" Then

        '    vPCSCHK_APPSTS = 0
        '    da2 = New SqlClient.SqlDataAdapter("select Approved_Status  from Weaver_Piece_Checking_Head a  where a.Weaver_Piece_Checking_Code = '" & Trim(vPCSCHKCODE) & "' ", con)
        '    dt2.Clear()
        '    da2.Fill(dt2)
        '    If dt2.Rows.Count > 0 Then
        '        vPCSCHK_APPSTS = dt2.Rows(0).Item("Approved_Status").ToString
        '    End If
        '    dt2.Clear()

        '    If Val(vPCSCHK_APPSTS) = 1 Then
        '        MessageBox.Show("Already IR Entry prepared and approved", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        cbo_LotCodeSelection.Focus()
        '        Exit Sub
        '    End If

        'End If

        da2 = New SqlClient.SqlDataAdapter("select count(a.Checking_Table_IdNo) from Lot_Allotment_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_ForSelection = '" & Trim(cbo_LotCodeSelection.Text) & "' and a.Checking_Table_IdNo <> 0 Having count(a.Checking_Table_IdNo) <> 0", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            vLOTALOT_STS = True
        End If
        dt2.Clear()

        da2 = New SqlClient.SqlDataAdapter("select count(a.Piece_Checking_Defect_IdNo) from Weaver_ClothReceipt_App_Piece_Defect_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.LotCode_Selection = '" & Trim(cbo_LotCodeSelection.Text) & "' and a.Piece_Checking_Defect_IdNo <> 0  Having count(a.Piece_Checking_Defect_IdNo) <> 0", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            vAPP_DEFECTENT_STS = True
        End If
        dt2.Clear()

        da2 = New SqlClient.SqlDataAdapter("select count(a.Piece_No) from Weaver_ClothReceipt_App_PieceChecking_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lotcode_Selection = '" & Trim(cbo_LotCodeSelection.Text) & "' and a.Piece_No <> '' and a.Total_Checking_Meters <> 0  Having count(a.Piece_No) <> 0", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            vAPP_PCSENT_STS = True
        End If
        dt2.Clear()



        'If vAPP_PCSENT_STS = True Then
        '    MessageBox.Show("Already Piece details entered", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    cbo_LotCodeSelection.Focus()
        '    Exit Sub
        'End If

        'If vAPP_DEFECTENT_STS = True Then
        '    MessageBox.Show("Already Piece defects entered", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    cbo_LotCodeSelection.Focus()
        '    Exit Sub
        'End If



        '====================================================================================================================================
        '====================================================================================================================================
        '////////////////////////////////////          APPLY AFTER 1 WEEK
        '====================================================================================================================================
        '====================================================================================================================================

        'Dim vTEX_WEAV_ALL_LOOMSNOS As String

        'vTEX_WEAV_ALL_LOOMSNOS = ""

        'Da = New SqlClient.SqlDataAdapter("select Loom_No from  Weaver_Loom_Details Where ledger_idno = " & Str(Val(Led_ID)) & " and Cloth_Idno = " & Str(Val(Clo_ID)), con)
        'Da.SelectCommand.CommandTimeout = 600
        'Dt1 = New DataTable
        'Da.Fill(Dt1)

        'If Dt1.Rows.Count > 0 Then

        '    For I = 0 To Dt1.Rows.Count - 1

        '        If IsDBNull(Dt1.Rows(I).Item("Loom_No").ToString) = False Then
        '            If Trim(Dt1.Rows(I).Item("Loom_No").ToString) <> "" Then
        '                vTEX_WEAV_ALL_LOOMSNOS = Trim(vTEX_WEAV_ALL_LOOMSNOS) & "~" & Trim(Dt1.Rows(I).Item("Loom_No").ToString) & "~"
        '            End If
        '        End If

        '    Next I

        'Else

        '    MessageBox.Show("Invalid Cloth Name  {" & Trim(lbl_ClothName.Text) & "} " & Chr(13) & "This {Cloth_Name} does not belong to this Vendor", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    Exit Sub

        'End If
        'Dt1.Clear()


        'If dgv_Details.Columns(3).Visible = True Then

        '    For i = 0 To dgv_Details.RowCount - 1

        '        If Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Then

        '            If Trim(dgv_Details.Rows(i).Cells(3).Value) = "" Then
        '                'Throw New ApplicationException("Invalid {Loom No} For Pcs No : " & Trim(dgv_Details.Rows(i).Cells(0).Value))
        '                MessageBox.Show("Invalid {Loom No} For Pcs No : " & Trim(dgv_Details.Rows(i).Cells(0).Value), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '                Exit Sub
        '            End If

        '            If Trim(dgv_Details.Rows(i).Cells(3).Value) <> "" Then

        '                If InStr(1, Trim(UCase(vTEX_WEAV_ALL_LOOMSNOS)), "~" & Trim(dgv_Details.Rows(i).Cells(3).Value) & "~") <= 0 Then
        '                    MessageBox.Show("Invalid Loom No {" & Trim(dgv_Details.Rows(i).Cells(3).Value) & "} in Pcs No : " & Trim(dgv_Details.Rows(i).Cells(0).Value) & Chr(13) & "The [Loom No] for this [ClothName] does not belong to this vendor", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '                    Exit Sub
        '                    ' Throw New ApplicationException("Invalid Loom No {" & Trim(dgv_Details.Rows(i).Cells(3).Value) & "} in Pcs No : " & Trim(dgv_Details.Rows(i).Cells(0).Value) & Chr(13) & "The [Loom No] for this [ClothName] does not belong to this vendor")
        '                    ' Exit Sub
        '                End If

        '            End If

        '        End If


        '    Next

        'End If





        'If dgv_Details.Columns(4).Visible = True Or dgv_Details.Columns(5).Visible = True Then

        '    For i = 0 To dgv_Details.RowCount - 1

        '        If Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Then

        '            If Trim(dgv_Details.Rows(i).Cells(4).Value) <> "" Then

        '                Da = New SqlClient.SqlDataAdapter("Select Beam_No, Meters from Stock_SizedPavu_Processing_Details Where Vendor_IdNo = " & Str(Val(Led_ID)) & " and Cloth_Idno = " & Str(Val(Clo_ID)) & " and Vendor_LoomNo = '" & Trim(dgv_Details.Rows(i).Cells(3).Value) & "' and BeamNo_SetCode_forSelection = '" & Trim(dgv_Details.Rows(i).Cells(4).Value) & "'", con)
        '                Da.SelectCommand.CommandTimeout = 600
        '                dt2 = New DataTable
        '                Da.Fill(dt2)
        '                If dt2.Rows.Count <= 0 Then
        '                    MessageBox.Show("Invalid Beam No {" & Trim(dgv_Details.Rows(i).Cells(4).Value) & "} " & Chr(13) & "This {BeamNo} does not belong to this Vendor", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '                    Exit Sub
        '                End If
        '                dt2.Clear()

        '                Da = New SqlClient.SqlDataAdapter("Select Beam_No, Meters from Stock_SizedPavu_Processing_Details Where Vendor_IdNo = " & Str(Val(Led_ID)) & " and Cloth_Idno = " & Str(Val(Clo_ID)) & " and Vendor_LoomNo = '" & Trim(dgv_Details.Rows(i).Cells(3).Value) & "' and BeamNo_SetCode_forSelection = '" & Trim(dgv_Details.Rows(i).Cells(5).Value) & "'", con)
        '                Da.SelectCommand.CommandTimeout = 600
        '                dt2 = New DataTable
        '                Da.Fill(dt2)
        '                If dt2.Rows.Count <= 0 Then
        '                    MessageBox.Show("Invalid Beam No {" & Trim(dgv_Details.Rows(i).Cells(5).Value) & "} " & Chr(13) & "This {BeamNo} does not belong to this Vendor", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '                    Exit Sub
        '                End If
        '                dt2.Clear()

        '            End If

        '        End If

        '    Next

        'End If

        '====================================================================================================================================
        '====================================================================================================================================
        '////////////////////////////////////
        '====================================================================================================================================
        '====================================================================================================================================


        vRCPT_PKCONDT = "WCLRC-"
        vCLORECCODE = Trim(vRCPT_PKCONDT) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(vLOTNO) & "/" & Trim(Common_Procedures.FnYearCode)


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Lot_Approved_Head", "Lot_Approved_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value)

            If Trim(vPCSCHKCODE) = "" Then

                If New_Entry = True Then

                    cmd.CommandText = "Insert into Lot_Approved_Head (  Lot_Approved_Code      ,             Company_IdNo         ,       Lot_Approved_No         ,                               for_OrderBy                              , Lot_Approved_date        ,            Lotcode_ForSelection          ,            Lot_No     ,            Lot_Code     ,        Receipt_PkCondition   ,  Weaver_ClothReceipt_Code  ,       Cloth_IdNo    ,             noof_pcs          ,             pcs_fromno         ,              pcs_tono        ,           Receipt_Meters                ,                Receipt_Dc_Meters             ,       Total_Receipt_Pcs ,    Total_Receipt_Meters  ,          approved_sts    ,               remarks            ,     Receipt_Dc_Pcs  ) " &
                                            "           Values           ( '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @EntryDate          , '" & Trim(cbo_LotCodeSelection.Text) & "', '" & Trim(vLOTNO) & "', '" & Trim(vLOTCODE) & "', '" & Trim(vRCPT_PKCONDT) & "', '" & Trim(vCLORECCODE) & "', " & Val(Clo_ID) & " , " & Val(txt_NoOfPcs.Text) & " , " & Val(txt_PcsNoFrom.Text) & ", " & Val(lbl_PcsNoTo.Text) & ", " & Str(Val(txt_ReceiptMeters.Text)) & ", " & Str(Val(lbl_dc_receipt_meters.Text)) & " , " & Val(vTotRcptPcs) & ", " & Val(vTotRcptMtrs) & ", " & Val(Approved_STS) & ", '" & Trim(txt_remarks.Text) & "' , " & Str(Val(lbl_dc_receipt_pcs.Text)) & ") "
                    cmd.ExecuteNonQuery()

                Else

                    Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Lot_Approved_Head", "Lot_Approved_code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Lot_Approved_code, Company_IdNo, for_OrderBy", tr)

                    cmd.CommandText = "Update Lot_Approved_Head set Lot_Approved_date = @EntryDate, Lotcode_ForSelection = '" & Trim(cbo_LotCodeSelection.Text) & "', Lot_No = '" & Trim(vLOTNO) & "', Lot_Code = '" & Trim(vLOTCODE) & "', Receipt_PkCondition = '" & Trim(vRCPT_PKCONDT) & "', Weaver_ClothReceipt_Code = '" & Trim(vCLORECCODE) & "', Cloth_IdNo = " & Val(Clo_ID) & ", noof_pcs = " & Val(txt_NoOfPcs.Text) & " ,pcs_fromno= " & Val(txt_PcsNoFrom.Text) & ",pcs_tono= " & Val(lbl_PcsNoTo.Text) & ",Receipt_Meters= " & Str(Val(txt_ReceiptMeters.Text)) & " ,Total_Receipt_Pcs=" & Val(vTotRcptPcs) & "   ,Total_Receipt_Meters=" & Val(vTotRcptMtrs) & " ,approved_sts=" & Val(Approved_STS) & ",Remarks='" & Trim(txt_remarks.Text) & "',Receipt_Dc_Meters=" & Str(Val(lbl_dc_receipt_meters.Text)) & " , Receipt_Dc_Pcs = " & Str(Val(lbl_dc_receipt_pcs.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Lot_Approved_Code = '" & Trim(NewCode) & "' "
                    cmd.ExecuteNonQuery()

                End If
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Lot_Approved_Head", "Lot_Approved_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Lot_Approved_code, Company_IdNo, for_OrderBy", tr)

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Lot_Approved_Status = " & Val(Approved_STS) & ", noof_pcs = " & Val(txt_NoOfPcs.Text) & ", ReceiptMeters_Receipt = " & Val(txt_ReceiptMeters.Text) & ", Receipt_Meters = " & Val(txt_ReceiptMeters.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and lotcode_forSelection = '" & Trim(cbo_LotCodeSelection.Text) & "' and Weaver_ClothReceipt_Code = '" & Trim(vLOTCODE) & "'"
                cmd.ExecuteNonQuery()

                EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)

                cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(vCLORECCODE) & "' and Lot_Code = '" & Trim(vLOTCODE) & "' and Create_Status = 2 and Weaver_Piece_Checking_Code = ''"
                cmd.ExecuteNonQuery()

            End If

            With dgv_Details

                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Then

                        Sno = Sno + 1

                        Nr = 0
                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Weaver_ClothReceipt_date = @EntryDate, Ledger_Idno = " & Str(Val(Led_ID)) & ", Sl_No = " & Str(Val(Sno)) & ", Main_PieceNo = '" & Trim(Val(.Rows(i).Cells(0).Value)) & "' , PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(0).Value))) & ", ReceiptMeters_Receipt = " & Val(.Rows(i).Cells(1).Value) & ", Create_Status = 2, StockOff_IdNo = " & Str(Val(vStkOf_Pos_IdNo)) & ", WareHouse_IdNo = " & Str(Val(vGod_ID)) & ", BeamNo_SetCode_forSelection = '" & Trim(.Rows(i).Cells(4).Value) & "', BeamNo2_SetCode_forSelection = '" & Trim(.Rows(i).Cells(5).Value) & "',LOOM_NO='" & Trim(.Rows(i).Cells(3).Value) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(vCLORECCODE) & "' and Lot_Code = '" & Trim(vLOTCODE) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_ClothReceipt_Code  ,            Company_IdNo          ,  Weaver_ClothReceipt_No ,                               for_OrderBy                       , Weaver_ClothReceipt_date,           Lot_Code       ,             Lot_No    ,     Ledger_Idno         , Cloth_IdNo          , Folding_Receipt, Folding,         Sl_No        ,                     Piece_No           ,                  Main_PieceNo               ,                               PieceNo_OrderBy                                   ,     ReceiptMeters_Receipt           ,                Receipt_Meters       , Create_Status ,              StockOff_IdNo       ,          WareHouse_IdNo  ,        BeamNo_SetCode_forSelection     ,       BeamNo2_SetCode_forSelection      ,                 LOOM_NO             ) " &
                                                    "           Values                           ('" & Trim(vCLORECCODE) & "', " & Str(Val(lbl_Company.Tag)) & ",   '" & Trim(vLOTNO) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vLOTNO))) & " ,          @EntryDate     ,  '" & Trim(vLOTCODE) & "', '" & Trim(vLOTNO) & "', " & Str(Val(Led_ID)) & ", " & Str(Val(Clo_ID)) & ",     100        ,   100  , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(0).Value) & "', '" & Trim(Val(.Rows(i).Cells(0).Value)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(0).Value))) & ", " & Val(.Rows(i).Cells(1).Value) & ", " & Val(.Rows(i).Cells(1).Value) & ",       2       , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(vGod_ID)) & ", '" & Trim(.Rows(i).Cells(4).Value) & "', '" & Trim(.Rows(i).Cells(5).Value) & "' ,'" & Trim(.Rows(i).Cells(3).Value) & "') "
                            Nr = cmd.ExecuteNonQuery()

                        Else

                            Nr = 0
                            cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Ledger_Idno = " & Str(Val(Led_ID)) & ", Receipt_Meters = " & Val(.Rows(i).Cells(1).Value) & ", ReceiptMeters_Checking = " & Val(.Rows(i).Cells(1).Value) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(vCLORECCODE) & "' and Lot_Code = '" & Trim(vLOTCODE) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
                            Nr = cmd.ExecuteNonQuery()

                        End If

                    End If

                Next
                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_ClothReceipt_Piece_Details", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Piece_No,Main_PieceNo,PieceNo_OrderBy,ReceiptMeters_Receipt,Receipt_Meters, Create_Status ,StockOff_IdNo,WareHouse_IdNo", "Sl_No", "Weaver_ClothReceipt_Code, For_OrderBy, Company_IdNo, Weaver_ClothReceipt_No, Weaver_ClothReceipt_Date, Ledger_Idno", tr)

            End With

            tr.Commit()

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If
            Else
                move_record(lbl_RefNo.Text)
            End If

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            'If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
            If New_Entry = True Then
                new_record()
            Else
                move_record(lbl_RefNo.Text)
            End If
            'Else
            '    move_record(lbl_RefNo.Text)
            'End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()

            If cbo_LotCodeSelection.Enabled And cbo_LotCodeSelection.Visible Then cbo_LotCodeSelection.Focus()

        End Try


    End Sub


    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        Dim TotPcs As Single = 0
        Dim TotMtrs As Single = 0

        Total_Calculation()


        With dgv_Details_Total
            If .RowCount > 0 Then
                TotPcs = Val(.Rows(0).Cells(0).Value)
                TotMtrs = Val(.Rows(0).Cells(1).Value)
            End If
        End With
        If Val(txt_ReceiptMeters.Text) = 0 Then
            'txt_NoOfPcs.Text = Val(TotPcs)
            txt_ReceiptMeters.Text = Format(Val(TotMtrs), "#########0.00")
        End If

        dgv_Details_CellLeave(sender, e)

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim Nextvalue As Integer = 0
        Dim rect As Rectangle


        If FrmLdSTS = True Or vMOVREC_STS = True Then Exit Sub
        With dgv_Details

            If e.RowIndex = 0 Then
                .CurrentRow.Cells(0).Value = Val(txt_PcsNoFrom.Text)
            Else
                'If Val(.CurrentRow.Cells(0).Value) = 0 Then
                '    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                'End If

                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = Val(.Rows(e.RowIndex - 1).Cells(0).Value) + 1
                End If
            End If

            If e.ColumnIndex = 4 Then

                If cbo_Grid_BeamNo1.Visible = False Or Val(cbo_Grid_BeamNo1.Tag) <> e.RowIndex Then

                    cbo_Grid_BeamNo1.Tag = -1
                    'set_Grid_BeamNo1_and_BeamNo2_combo_datasource(1)
                    'Da = New SqlClient.SqlDataAdapter("select BeamNo_SetCode_forSelection from Stock_SizedPavu_Processing_Details order by BeamNo_SetCode_forSelection", con)
                    'Dt1 = New DataTable
                    'Da.Fill(Dt1)
                    'cbo_Grid_BeamNo1.DataSource = Dt1
                    'cbo_Grid_BeamNo1.DisplayMember = "BeamNo_SetCode_forSelection"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_BeamNo1.Left = .Left + rect.Left
                    cbo_Grid_BeamNo1.Top = .Top + rect.Top

                    cbo_Grid_BeamNo1.Width = rect.Width
                    cbo_Grid_BeamNo1.Height = rect.Height

                    Debug.Print(cbo_Grid_BeamNo1.Text)
                    cbo_Grid_BeamNo1.Text = .CurrentCell.Value
                    Debug.Print(cbo_Grid_BeamNo1.Text)
                    cbo_Grid_BeamNo1.Tag = Val(e.RowIndex)
                    Debug.Print(cbo_Grid_BeamNo1.Text)
                    cbo_Grid_BeamNo1.Visible = True
                    Debug.Print(cbo_Grid_BeamNo1.Text)
                    cbo_Grid_BeamNo1.BringToFront()
                    Debug.Print(cbo_Grid_BeamNo1.Text)
                    cbo_Grid_BeamNo1.Focus()
                    Debug.Print(cbo_Grid_BeamNo1.Text)
                End If

            Else

                cbo_Grid_BeamNo1.Visible = False

            End If


            If e.ColumnIndex = 5 Then

                If cbo_Grid_BeamNo2.Visible = False Or Val(cbo_Grid_BeamNo2.Tag) <> e.RowIndex Then

                    cbo_Grid_BeamNo2.Tag = -1
                    'set_Grid_BeamNo1_and_BeamNo2_combo_datasource(2)
                    'Da = New SqlClient.SqlDataAdapter("select BeamNo_SetCode_forSelection from Stock_SizedPavu_Processing_Details order by BeamNo_SetCode_forSelection", con)
                    'Dt1 = New DataTable
                    'Da.Fill(Dt1)
                    'cbo_Grid_BeamNo2.DataSource = Dt1
                    'cbo_Grid_BeamNo2.DisplayMember = "BeamNo_SetCode_forSelection"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_BeamNo2.Left = .Left + rect.Left
                    cbo_Grid_BeamNo2.Top = .Top + rect.Top

                    cbo_Grid_BeamNo2.Width = rect.Width
                    cbo_Grid_BeamNo2.Height = rect.Height
                    cbo_Grid_BeamNo2.Text = .CurrentCell.Value

                    cbo_Grid_BeamNo2.Tag = Val(e.RowIndex)
                    cbo_Grid_BeamNo2.Visible = True

                    cbo_Grid_BeamNo2.BringToFront()
                    cbo_Grid_BeamNo2.Focus()

                End If

            Else

                cbo_Grid_BeamNo2.Visible = False

            End If


        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        If FrmLdSTS = True Or vMOVREC_STS = True Then Exit Sub
        With dgv_Details
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim TotMtrs As Single = 0

        On Error Resume Next

        If FrmLdSTS = True Or vMOVREC_STS = True Then Exit Sub

        With dgv_Details

            If .Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                If .CurrentCell.ColumnIndex = 1 Then

                    Total_Calculation()

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
        Dim WagesCode As String = ""

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            If Val(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value) = 0 Then

                'Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_Code, Weaver_Wages_Code, Weaver_IR_Wages_Code from Weaver_Cloth_Receipt_Head Where lotcode_forSelection = '" & Trim(cbo_LotCodeSelection.Text) & "'", con)
                'Dt1 = New DataTable
                'Da.Fill(Dt1)

                'PcsChkCode = ""
                'WagesCode = ""
                'If Dt1.Rows.Count > 0 Then
                '    If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                '        PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                '    End If
                '    If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                '        WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
                '    End If
                '    If Trim(WagesCode) = "" Then
                '        If IsDBNull(Dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString) = False Then
                '            WagesCode = Dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString
                '        End If
                '    End If
                'End If
                'Dt1.Clear()


                'If Trim(PcsChkCode) <> "" Then
                '    MessageBox.Show("Piece Checking prepared", "DOES NOT DELETE PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                '    Exit Sub
                'End If
                'If Trim(WagesCode) <> "" Then
                '    MessageBox.Show("Weaver wages prepared", "DOES NOT DELETE PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                '    Exit Sub
                'End If

                With dgv_Details

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 And .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    PcsFrmNo = Val(txt_PcsNoFrom.Text)
                    If Val(PcsFrmNo) = 0 Then PcsFrmNo = 1

                    For i = 0 To .Rows.Count - 1
                        If i = 0 Then
                            .Rows(i).Cells(0).Value = Val(PcsFrmNo)
                        Else
                            .Rows(i).Cells(0).Value = Val(.Rows(i - 1).Cells(0).Value) + 1
                        End If
                    Next

                End With

                Total_Calculation()

            End If

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub


    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded

        On Error Resume Next
        If FrmLdSTS = True Or vMOVREC_STS = True Then Exit Sub
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details

            If e.RowIndex = 0 Then
                .CurrentRow.Cells(0).Value = Val(txt_PcsNoFrom.Text)

            Else
                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = Val(.Rows(e.RowIndex - 1).Cells(0).Value) + 1
                End If

            End If

        End With

    End Sub

    Private Sub Total_Calculation()
        Dim TotPcs As Single, TotMtrs As Single

        If FrmLdSTS = True Or vMOVREC_STS = True Then Exit Sub

        PieceNo_To_Calculation()

        TotPcs = 0
        TotMtrs = 0
        With dgv_Details

            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(1).Value) <> 0 Then
                    TotPcs = TotPcs + 1
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(1).Value)
                End If
            Next

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(0).Value = Val(TotPcs)
            .Rows(0).Cells(1).Value = Format(Val(TotMtrs), "########0.00")
        End With

        'txt_NoOfPcs.Text = Val(TotPcs)
        txt_ReceiptMeters.Text = Format(Val(TotMtrs), "#########0.00")

    End Sub




    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_ReceiptMeters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ReceiptMeters.KeyDown
        Dim TotMtrs As Single = 0

        If e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 Then
            TotMtrs = 0
            With dgv_Details_Total
                If .RowCount > 0 Then
                    TotMtrs = Val(.Rows(0).Cells(1).Value)
                End If
            End With
            If Val(TotMtrs) <> 0 Then e.Handled = True : e.SuppressKeyPress = True

        End If
    End Sub

    Private Sub txt_ReceiptMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ReceiptMeters.KeyPress
        Dim TotMtrs As Single = 0

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        With dgv_Details_Total
            TotMtrs = 0
            If .RowCount > 0 Then
                TotMtrs = Val(.Rows(0).Cells(1).Value)
            End If
        End With
        If Val(TotMtrs) <> 0 Then e.Handled = True

    End Sub


    Private Sub txt_PcsNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsNoFrom.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.Focus()
            dgv_Details.CurrentCell.Selected = True
        End If

    End Sub

    Private Sub txt_NoOfPcs_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NoOfPcs.GotFocus
        txt_NoOfPcs.Tag = txt_NoOfPcs.Text
    End Sub

    Private Sub txt_NoofPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfPcs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If Val(txt_NoOfPcs.Tag) <> Val(txt_NoOfPcs.Text) Then
                txt_NoOfPcs.Tag = txt_NoOfPcs.Text
                Design_PieceDetails_Grid()
            End If
        End If
    End Sub

    Private Sub txt_NoOfPcs_LostFocus(sender As Object, e As EventArgs) Handles txt_NoOfPcs.LostFocus
        If Val(txt_NoOfPcs.Tag) <> Val(txt_NoOfPcs.Text) Then
            txt_NoOfPcs.Tag = txt_NoOfPcs.Text
            Design_PieceDetails_Grid()
        End If
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


    Private Sub txt_PcsNoFrom_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_PcsNoFrom.TextChanged
        Try
            Grid_PieceNo_Generation()
        Catch ex As Exception
            '---
        End Try
    End Sub


    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details
            If .Visible Then

                'If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(0).Value) = 0 Then
                '    e.Handled = True
                'End If

                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 3 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If

            End If
        End With

    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        dgv_Details_KeyUp(sender, e)
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        If e.Control = True And e.KeyValue = 13 Then
            If txt_ReceiptMeters.Enabled And txt_ReceiptMeters.Visible Then
                txt_ReceiptMeters.Focus()

            Else
                chk_approved_sts.Focus()

            End If

        ElseIf e.KeyValue = 46 Then
            With dgv_Details
                If .CurrentCell.ColumnIndex = 1 Then
                    .Rows(.CurrentCell.RowIndex).Cells(1).Value = ""

                End If

            End With

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

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_LotCodeSelection.Focus()
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

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If FrmLdSTS = True Or vMOVREC_STS = True Then Exit Sub
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_LotCodeSelection.Focus()

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

    Private Sub cbo_StockOff_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Grid_PieceNo_Generation()
        Dim i As Integer = 0
        Dim PcFrmNo As Integer = 0

        Try

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


        Catch ex As Exception
            '-----

        End Try

    End Sub


    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_LotCodeSelection_GotFocus(sender As Object, e As EventArgs) Handles cbo_LotCodeSelection.GotFocus
        cbo_LotCodeSelection.Tag = cbo_LotCodeSelection.Text
        Dim vLOT_APRVL_STARTDATE As Date = #8/15/2021#

        Dim vCurYr As String = ""
        Dim vPreYr As String = ""
        vCurYr = Trim(Common_Procedures.FnYearCode)
        vPreYr = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnYearCode), 2)
        '  vPreYr = Trim(Format(Val(vPreYr) - 1, "00")) & "-" & Trim(Format(Val(vPreYr), "00"))

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Cloth_Receipt_head", "lotcode_forSelection", "(company_idno<>0  and Return_Status = 0 and Weaver_ClothReceipt_date > '" & Trim(Format(vLOT_APRVL_STARTDATE, "MM/dd/yyyy")) & "'  and (  lotcode_forSelection LIKE '%/" & Trim(vCurYr) & "%'  or lotcode_forSelection LIKE '%/" & Trim(vPreYr) & "%'  ) )", "(lotcode_forSelection = '')")
    End Sub

    Private Sub cbo_LotCodeSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_LotCodeSelection.KeyDown
        Dim vCurYr As String = ""
        Dim vPreYr As String = ""
        vCurYr = Trim(Common_Procedures.FnYearCode)
        vPreYr = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnYearCode), 2)
        '  vPreYr = Trim(Format(Val(vPreYr) - 1, "00")) & "-" & Trim(Format(Val(vPreYr), "00"))

        Dim vLOT_APRVL_STARTDATE As Date = #8/15/2021#
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LotCodeSelection, msk_date, txt_remarks, "Weaver_Cloth_Receipt_head", "lotcode_forSelection", "(company_idno<>0 and Return_Status = 0 and Weaver_ClothReceipt_date > '" & Trim(Format(vLOT_APRVL_STARTDATE, "MM/dd/yyyy")) & "'   and (  lotcode_forSelection LIKE '%/" & Trim(vCurYr) & "%'  or lotcode_forSelection LIKE '%/" & Trim(vPreYr) & "%'  ) )", "(lotcode_forSelection='')")
    End Sub

    Private Sub cbo_LotCodeSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_LotCodeSelection.KeyPress

        Dim vCurYr As String = ""
        Dim vPreYr As String = ""
        vCurYr = Trim(Common_Procedures.FnYearCode)
        vPreYr = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnYearCode), 2)
        '    vPreYr = Trim(Format(Val(vPreYr) - 1, "00")) & "-" & Trim(Format(Val(vPreYr), "00"))

        Dim vLOT_APRVL_STARTDATE As Date = #8/15/2021#
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LotCodeSelection, Nothing, "Weaver_Cloth_Receipt_head", "lotcode_forSelection", "(company_idno <> 0 and Return_Status = 0 and Weaver_ClothReceipt_date > '" & Trim(Format(vLOT_APRVL_STARTDATE, "MM/dd/yyyy")) & "'  and (  lotcode_forSelection LIKE '%/" & Trim(vCurYr) & "%'  or lotcode_forSelection LIKE '%/" & Trim(vPreYr) & "%'  ) )", "(lotcode_forSelection='')")

        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(cbo_LotCodeSelection.Text)) <> Trim(UCase(cbo_LotCodeSelection.Tag)) Then
                cbo_LotCodeSelection.Tag = cbo_LotCodeSelection.Text
                Check_and_Get_PieceDetails(sender)
            End If

            Dim vTotRcptMtrs As String = 0
            vTotRcptMtrs = 0
            If dgv_Details_Total.RowCount > 0 Then
                vTotRcptMtrs = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
            End If

            If Val(vTotRcptMtrs) = 0 Then
                If Val(txt_NoOfPcs.Text) = 0 Then
                    txt_NoOfPcs.Focus()

                ElseIf dgv_Details.Rows.Count > 0 Then
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                        dgv_Details.Focus()
                    End If
                    Exit Sub
                Else
                    txt_remarks.Focus()
                End If

            Else
                txt_remarks.Focus()
                'chk_approved_sts.Focus()

            End If

        End If

    End Sub
    Private Sub txt_remarks_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_remarks.KeyDown
        If e.KeyCode = 38 Then
            If txt_NoOfPcs.Visible And txt_NoOfPcs.Enabled Then
                txt_NoOfPcs.Focus()
            Else
                If cbo_LotCodeSelection.Visible And cbo_LotCodeSelection.Enabled Then cbo_LotCodeSelection.Focus() Else msk_date.Focus()
            End If
        End If




        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_LotCodeSelection.Focus()
            End If
        End If

    End Sub

    Private Sub txt_remarks_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                If msk_date.Visible And msk_date.Enabled Then msk_date.Focus() Else cbo_LotCodeSelection.Focus()
            End If
        End If

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

    Private Sub btn_List_PieceDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_List_PieceDetails.Click
        Check_and_Get_PieceDetails(sender)
    End Sub

    Private Sub Check_and_Get_PieceDetails(sender As System.Object)
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = ""
        Dim NewCode As String = ""
        Dim Cat_ID As Integer = 0

        Try

            cbo_LotCodeSelection.Tag = cbo_LotCodeSelection.Text

            Cmd.Connection = con

            Cmd.CommandText = "Select Lot_Approved_No from Lot_Approved_Head Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and LotCode_FORSelection = '" & Trim(cbo_LotCodeSelection.Text) & "'"
            Da = New SqlClient.SqlDataAdapter(Cmd)
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

                If Trim(UCase(movno)) <> Trim(UCase(lbl_RefNo.Text)) Then
                    move_record(movno)

                Else

                    If sender.name.ToString.ToLower = btn_List_PieceDetails.Name.ToString.ToLower Then
                        move_record(movno)
                    End If

                End If

            Else

                Dim vLOTCODESELC As String = ""

                vLOTCODESELC = cbo_LotCodeSelection.Text

                new_record()

                cbo_LotCodeSelection.Text = vLOTCODESELC
                cbo_LotCodeSelection.Tag = cbo_LotCodeSelection.Text

                get_PieceList()

            End If

            cbo_LotCodeSelection.Tag = cbo_LotCodeSelection.Text

        Catch ex As Exception
            '----

        End Try

    End Sub


    Private Sub get_PieceList()
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim I As Integer, J As Integer


        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, c.ledger_name as weavername, d.EndsCount_Name from Weaver_Cloth_Receipt_head a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno   INNER JOIN ledger_Head c ON a.ledger_idno = c.ledger_idno  INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where a.lotcode_forSelection = '" & Trim(cbo_LotCodeSelection.Text) & "'", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            lbl_WeaverName.Text = dt2.Rows(0).Item("weavername").ToString
            lbl_ClothName.Text = dt2.Rows(0).Item("Cloth_name").ToString
            lbl_EndsCount.Text = dt2.Rows(0).Item("EndsCount_name").ToString
            txt_NoOfPcs.Text = dt2.Rows(0).Item("noof_pcs").ToString
            txt_ReceiptMeters.Text = dt2.Rows(0).Item("Receipt_Meters").ToString
            txt_PcsNoFrom.Text = dt2.Rows(0).Item("pcs_fromno").ToString
            lbl_PcsNoTo.Text = dt2.Rows(0).Item("pcs_tono").ToString
            lbl_dc_receipt_pcs.Text = dt2.Rows(0).Item("DC_Receipt_Pcs").ToString
            lbl_dc_receipt_meters.Text = dt2.Rows(0).Item("Dc_Receipt_Meters").ToString
            If Val(txt_NoOfPcs.Text) = 0 Then
                txt_NoOfPcs.Text = lbl_dc_receipt_pcs.Text
            End If

        Else

            lbl_WeaverName.Text = ""
            lbl_ClothName.Text = ""
            lbl_EndsCount.Text = ""
            txt_NoOfPcs.Text = ""
            txt_PcsNoFrom.Text = ""
            lbl_PcsNoTo.Text = ""
            lbl_dc_receipt_pcs.Text = ""
            lbl_dc_receipt_meters.Text = ""

        End If
        dt2.Clear()


        vMOVREC_STS = True
        PieceNo_To_Calculation()
        dgv_Details.Rows.Clear()
        For I = Val(txt_PcsNoFrom.Text) To Val(lbl_PcsNoTo.Text)
            n = dgv_Details.Rows.Add()
            dgv_Details.Rows(n).Cells(0).Value = I
        Next I


        da2 = New SqlClient.SqlDataAdapter("Select a.Piece_No, a.Receipt_Meters from Weaver_ClothReceipt_App_PieceReceipt_Details a Where a.company_idno =  1 and a.LotCode_Selection = '" & Trim(cbo_LotCodeSelection.Text) & "'  Order by a.pieceno_OrderBy, a.Piece_No ", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        If dt2.Rows.Count > 0 Then

            For I = 0 To dt2.Rows.Count - 1

                For J = 0 To dgv_Details.Rows.Count - 1

                    If Val(dgv_Details.Rows(J).Cells(0).Value) = Val(dt2.Rows(I).Item("Piece_No").ToString) Then
                        If Val(dgv_Details.Rows(J).Cells(1).Value) = 0 Then
                            dgv_Details.Rows(J).Cells(1).Value = Format(Val(dt2.Rows(I).Item("Receipt_Meters").ToString), "########0.00")
                        End If
                        Exit For
                    End If

                Next J

            Next I

        End If
        dt2.Clear()

        vMOVREC_STS = False

        Total_Calculation()

        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False

        If Val(txt_NoOfPcs.Text) = 0 Then
            txt_NoOfPcs.Focus()
        Else
            chk_approved_sts.Focus()
        End If


    End Sub

    Private Sub cbo_LotCodeSelection_LostFocus(sender As Object, e As EventArgs) Handles cbo_LotCodeSelection.LostFocus
        If Trim(UCase(cbo_LotCodeSelection.Text)) <> Trim(UCase(cbo_LotCodeSelection.Tag)) Then
            cbo_LotCodeSelection.Tag = cbo_LotCodeSelection.Text
            Check_and_Get_PieceDetails(sender)
        End If
    End Sub

    Private Sub cbo_Grid_BeamNo1_Enter(sender As Object, e As EventArgs) Handles cbo_Grid_BeamNo1.Enter

        Debug.Print(cbo_Grid_BeamNo1.Text)
        set_Grid_BeamNo1_and_BeamNo2_combo_datasource(sender)
        Debug.Print(cbo_Grid_BeamNo1.Text)

        'Dim vSQL_CONDT As String = ""
        'vSQL_CONDT = get_sql_condition_for_BeamNos()
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", vSQL_CONDT, "(BeamNo_SetCode_forSelection = '')")
        cbo_Grid_BeamNo1.BackColor = Color.Lime
        cbo_Grid_BeamNo1.ForeColor = Color.Blue
    End Sub
    Private Sub cbo_Grid_BeamNo1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BeamNo1.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "", "", "", "")

        With dgv_Details

            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)
            End If


        End With

    End Sub

    Private Sub cbo_Grid_BeamNo1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_BeamNo1.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "", "", "", "", True)

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BeamNo1.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)


            End With

        End If

    End Sub


    Private Sub cbo_Grid_BeamNo1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BeamNo1.TextChanged
        Try
            If cbo_Grid_BeamNo1.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_BeamNo1.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BeamNo1.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_BeamNo1_Leave(sender As Object, e As EventArgs) Handles cbo_Grid_BeamNo1.Leave
        cbo_Grid_BeamNo1.BackColor = Color.White
        cbo_Grid_BeamNo1.ForeColor = Color.Black
    End Sub


    Private Sub cbo_Grid_BeamNo2_Enter(sender As Object, e As EventArgs) Handles cbo_Grid_BeamNo2.Enter
        set_Grid_BeamNo1_and_BeamNo2_combo_datasource(sender)

        'Dim vSQL_CONDT As String = ""
        'vSQL_CONDT = get_sql_condition_for_BeamNos()
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", vSQL_CONDT, "(BeamNo_SetCode_forSelection = '')")

        cbo_Grid_BeamNo2.BackColor = Color.Lime
        cbo_Grid_BeamNo2.ForeColor = Color.Blue

    End Sub

    Private Sub cbo_Grid_BeamNo2_Leave(sender As Object, e As EventArgs) Handles cbo_Grid_BeamNo2.Leave
        cbo_Grid_BeamNo2.BackColor = Color.White
        cbo_Grid_BeamNo2.ForeColor = Color.Black
    End Sub

    Private Sub cbo_Grid_BeamNo2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BeamNo2.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "", "", "", "")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")

        With dgv_Details

            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If

            If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    txt_remarks.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                End If

            End If


        End With

    End Sub

    Private Sub cbo_Grid_BeamNo2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_BeamNo2.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "", "", "", "", True)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(5).Value = Trim(cbo_Grid_BeamNo2.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    txt_remarks.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                End If

            End With

        End If

    End Sub


    Private Sub cbo_Grid_BeamNo2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BeamNo2.TextChanged
        Try
            If cbo_Grid_BeamNo2.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_BeamNo2.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 5 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BeamNo2.Text)
                    End If
                End With
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub set_Grid_BeamNo1_and_BeamNo2_combo_datasource(ByVal sender As Object)
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim vDAT As Date
        Dim Clo_ID As Integer
        Dim vENDSCNT_ID As Integer
        Dim Wea_ID As Integer
        Dim vLMNO As String = ""
        Dim vCboTxt As String

        vCboTxt = sender.Text

        Cmd.Connection = con
        Cmd.CommandTimeout = 1000

        Wea_ID = 0
        If Trim(lbl_WeaverName.Text) <> "" Then
            Wea_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_WeaverName.Text)
        End If
        Clo_ID = 0
        If Trim(lbl_ClothName.Text) <> "" Then
            Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)
        End If
        vENDSCNT_ID = 0
        If Trim(lbl_EndsCount.Text) <> "" Then
            vENDSCNT_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount.Text)
        End If

        vLMNO = ""
        If Not IsNothing(dgv_Details.CurrentCell) Then
            vLMNO = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(3).Value
        End If
        If Trim(vLMNO) = "" Then
            vLMNO = "--~~LOOMNO--~~"
        End If

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.CommandText = "sp_get_combolist_BeamNoSetCodeforSelection_from_SizedPavuProcessingDetails"
        Cmd.Parameters.Clear()

        vDAT = dtp_Date.Value.Date.AddMonths(-6)
        Cmd.Parameters.Add("@fromdate", SqlDbType.Date)
        Cmd.Parameters("@fromdate").Value = vDAT.Date
        Cmd.Parameters.Add("@todate", SqlDbType.Date)
        Cmd.Parameters("@todate").Value = dtp_Date.Value.Date
        Cmd.Parameters.Add("@vendoridno", SqlDbType.Int)
        Cmd.Parameters("@vendoridno").Value = Wea_ID
        Cmd.Parameters.Add("@clothidno", SqlDbType.Int)
        Cmd.Parameters("@clothidno").Value = Clo_ID
        Cmd.Parameters.Add("@endscountidno", SqlDbType.Int)
        Cmd.Parameters("@endscountidno").Value = vENDSCNT_ID
        Cmd.Parameters.Add("@loomno", SqlDbType.VarChar)
        Cmd.Parameters("@loomno").Value = vLMNO
        Da = New SqlClient.SqlDataAdapter(Cmd)

        sender.DataSource = Nothing
        sender.DisplayMember = ""
        sender.SelectedText = ""
        sender.SelectedIndex = -1

        Dim Dt1 As New DataTable
        Dt1 = New DataTable
        Da.Fill(Dt1)
        sender.DataSource = Dt1
        sender.DisplayMember = "BeamNo_SetCode_forSelection"

        sender.Text = Trim(vCboTxt)

        'If vBEAM1_vBEAM2_STS = 1 Then
        '    Dim Dt1 As New DataTable
        '    Dt1 = New DataTable
        '    Da.Fill(Dt1)
        '    cbo_Grid_BeamNo1.DataSource = Dt1
        '    cbo_Grid_BeamNo1.DisplayMember = "BeamNo_SetCode_forSelection"

        'Else

        '    Dim Dt2 As New DataTable
        '    Dt2 = New DataTable
        '    Da.Fill(Dt2)
        '    cbo_Grid_BeamNo2.DataSource = Dt2
        '    cbo_Grid_BeamNo2.DisplayMember = "BeamNo_SetCode_forSelection"
        'End If

    End Sub

    Private Sub btn_EMail_Click(sender As Object, e As EventArgs) Handles btn_EMail.Click
        Dim Led_IdNo As Integer
        Dim MailTxt As String

        Try


            Common_Procedures.Print_OR_Preview_Status = 1
            Print_PDF_Status = True
            EMAIL_Status = True
            WHATSAPP_Status = False
            'Printing_CheckingReport()

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_WeaverName.Text)

            MailTxt = "PIECE CHECKING " & vbCrLf & vbCrLf
            MailTxt = MailTxt & "Lot No. :" & Trim(cbo_LotCodeSelection.Text) & vbCrLf & "Date-" & Trim(msk_date.Text)

            MailTxt = MailTxt & vbCrLf & "Qualitiy : " & Trim(lbl_ClothName.Text)
            MailTxt = MailTxt & vbCrLf & "Rec-Meters :" & Trim(lbl_dc_receipt_meters.Text)

            If System.IO.File.Exists(vEMAIL_Attachment_FileName) = True Then
                MailTxt = MailTxt & vbCrLf
                MailTxt = MailTxt & vbCrLf
                MailTxt = MailTxt & "Please find the following attachment(s):"
                MailTxt = MailTxt & "        " & Trim(Path.GetFileName(vEMAIL_Attachment_FileName))
            End If

            EMAIL_Entry.vMailID = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            EMAIL_Entry.vSubJect = "Fabric Receipt Lot : " & Trim(cbo_LotCodeSelection.Text)

            EMAIL_Entry.vMessage = Trim(MailTxt)
            EMAIL_Entry.vAttchFilepath = ""
            If System.IO.File.Exists(vEMAIL_Attachment_FileName) = True Then
                EMAIL_Entry.vAttchFilepath = Trim(vEMAIL_Attachment_FileName)
            End If

            Dim f1 As New EMAIL_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Printing_Receipt_CheckingReport()
    End Sub

    Private Sub Printing_Receipt_CheckingReport()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Lot_Approval_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head Where lotcode_forSelection = '" & Trim(cbo_LotCodeSelection.Text) & "'", con)
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

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                'e.PageSettings.PaperSize = ps
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
                ppd.PrintPreviewControl.Zoom = 1
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

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* ,c.* ,d.Cloth_Name, d.Cloth_Description ,E.EndsCount_Name , ig.Item_Hsn_Code from Weaver_Cloth_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno LEFT OUTER JOIN EndsCount_Head E ON E.EndsCount_IdNo = a.EndsCount_Idno INNER JOIN ItemGroup_head ig On ig.ItemGroup_Idno = d.ItemGroup_Idno where a.lotcode_forSelection = '" & Trim(cbo_LotCodeSelection.Text) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                '---

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Format_Half(e)
    End Sub

    Private Sub Printing_Format_Half(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 30 
            .Right = 40
            .Top = 25 ' 30 ' 50 
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

        NoofItems_PerPage = 3 ' 4 ' 6

        Erase LnAr
        Erase ClAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 35 : ClAr(2) = 0 : ClAr(3) = 60 : ClAr(4) = 100 : ClAr(5) = 100 : ClAr(6) = 130
        ClAr(2) = PageWidth - (LMargin + ClAr(1) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 17

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_Half_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_HdDt.Rows.Count > 0 Then

                    ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Description").ToString)
                    If Trim(ItmNm1) = "" Then
                        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Name").ToString)
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
                    Common_Procedures.Print_To_PrintDocument(e, "1", LMargin + 15, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Dc_Receipt_pcs").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Dc_Receipt_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Receipt_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                    NoofDets = NoofDets + 1

                    If Trim(ItmNm2) <> "" Then
                        CurY = CurY + TxtHgt - 5
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        NoofDets = NoofDets + 1
                    End If

                End If

                Printing_Format_Half_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_Half_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1, w2 As Single
        Dim S1, s2 As Single
        Dim Cmp_GSTIN_No As String

        PageNo = PageNo + 1

        CurY = TMargin

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from Cloth_Purchase_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        'da2.Fill(dt2)

        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

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
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
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
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth - 70, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, PageWidth - 40, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "WEAVER FABRIC RECEIPT NOTE", LMargin, CurY, 2, PrintWidth, p1Font)


        CurY = CurY + strHeight - 3
        p1Font = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin + 10, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SAC CODE : 998821 ( Textile manufactring service )", PageWidth - 10, CurY, 1, 0, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("E-Way Bill No.   : ", pFont).Width
        w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "LOT.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Party DC NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No.", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, " PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Eway_BillNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No.", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Eway_BillNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DC.PCS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DC.METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub


    Private Sub Printing_Format_Half_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim vTxamt As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0

        For i = NoofDets + 1 To NoofItems_PerPage

            CurY = CurY + TxtHgt

            prn_DetIndx = prn_DetIndx + 1

        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10

        Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("Dc_Receipt_pcs").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Dc_Receipt_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Receipt_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)



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
        CurY = CurY + 10


        CurY = CurY + 10
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_Design_PieceDetails_Grid_Click(sender As Object, e As EventArgs) Handles btn_Design_PieceDetails_Grid.Click
        Design_PieceDetails_Grid()
    End Sub

    Private Sub Design_PieceDetails_Grid()
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer



        N = dgv_Details.Rows.Count

        If N = Val(txt_NoOfPcs.Text) Then
            '--do nothing

        ElseIf Val(txt_NoOfPcs.Text) = 0 Then
            dgv_Details.Rows.Clear()

        ElseIf N < Val(txt_NoOfPcs.Text) Then

            For I = N + 1 To Val(txt_NoOfPcs.Text)
                dgv_Details.Rows.Add()
            Next I

            For I = 0 To dgv_Details.Rows.Count - 1
                dgv_Details.Rows(I).Cells(0).Value = I + 1
            Next

        Else


LOOP1:

            For J = Val(txt_NoOfPcs.Text) To dgv_Details.Rows.Count - 1

                'If J = dgv_Details.Rows.Count - 1 Then
                '    For I = 0 To dgv_Details.Columns.Count - 1
                '        dgv_Details.Rows(J).Cells(I).Value = ""
                '    Next

                'Else
                dgv_Details.Rows.RemoveAt(J)
                GoTo LOOP1

                'End If

            Next


            For I = 0 To dgv_Details.Rows.Count - 1
                dgv_Details.Rows(I).Cells(0).Value = I + 1
            Next

        End If


        Total_Calculation()

    End Sub




End Class