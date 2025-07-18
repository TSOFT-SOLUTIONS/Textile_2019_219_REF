Imports System.Drawing.Printing
Imports System.IO
Public Class Weaver_Piece_Approval_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "WPCAP-"

    Private PkCondition_Entry As String = ""
    Private prn_DetSNo As Integer
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    'Private prn_HdDt_New As New DataTable
    'Private prn_DetDt_New As New DataTable
    Private prn_DetBarCdStkr As Integer
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_HsnIndx As Integer
    Private prn_DetAr(200, 10) As String
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private Print_PDF_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxtDefect_Details As New DataGridViewTextBoxEditingControl

    Private NoCalc_Status As Boolean = False
    Private MOV_Status As Boolean = False

    Private vBARCDPRNT_PCSNO As String = ""
    Private vBARCDPRNT_COLNO As String = ""
    Private fs As FileStream
    Private sw As StreamWriter


    Private Enum dgvCol_LotDetails As Integer
        Slno                '0
        Checking_Date       '1
        YearCode            '2
        LotNo               '3
        TotChkPcs           '4
        TotChk_Mtrs         '5
        STS                 '6
        ClothReceipt_Code   '7
        LotCode             '8
    End Enum

    Private Enum dgvCol_PieceDetails As Integer
        Slno            '0
        YEARCODE        '1
        REC_PK          '2
        LOT_NO          '3
        CHK_Date        '4
        FOLDING         '5
        TABLENO         '6
        PIECENO         '7
        REC_MTRS        '8
        A_Sounds        '9
        B_Seconds       '10
        C_Bits          '11
        REJECT          '12
        Others          '13
        TOTAL_MTRS      '14
        WEIGHT          '15
        WGT_MTR         '16
        TOTAL_POINTS    '17
        APP_STS         '18
        CLOTH_REC_CODE  '19
        LOTCODE         '20
        PCS_MAINNO      '21
        PCS_SUBNO       '22
        DEFECT          '23
    End Enum

    Private Enum dgvCol_PieceVerificationPendingDetails As Integer
        Slno                '0
        USERNAME            '1
        TABLENO             '2
        YEARCODE            '3
        REC_PKCONDITION     '4
        LOT_NO              '5
        CHK_Date            '6
        CHK_Time            '7
        FOLDING             '8
        PIECENO             '9
        REC_MTRS            '10
        A_Sounds            '11
        B_Seconds           '12
        C_Bits              '13
        REJECT              '14
        Others              '15
        TOTAL_MTRS          '16
        WEIGHT              '17
        WGT_MTR             '18
        TOTAL_POINTS        '19
        DEFECTDETAILS       '20
        VIEWDEFECTDETAILS   '21
        CLOTH_REC_CODE      '22
        LOTCODE             '23
        PCS_MAINNO          '24
        PCS_SUBNO           '25
    End Enum

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        Dim obj As Object
        Dim ctrl1 As Object, ctrl2 As Object, ctrl3 As Object
        Dim pnl1 As Panel, pnl2 As Panel
        Dim grpbx As GroupBox

        New_Entry = False
        Insert_Entry = False
        Print_PDF_Status = False
        MOV_Status = False


        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_PIECE_VERIFICATION_PENDING_DETAILS.Visible = False

        lbl_invNo.Text = ""

        lbl_CheckingNo.Text = ""
        lbl_CheckingNo.ForeColor = Color.Black

        dtp_Date.Text = Now.Date

        cbo_Grid_Defect.Visible = False
        cbo_Grid_Defect.Tag = -100


        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Common_Procedures.User.IdNo)))

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If



        For Each obj In Me.Controls
            If TypeOf obj Is TextBox Then
                obj.text = ""

            ElseIf TypeOf obj Is ComboBox Then
                obj.text = ""

            ElseIf TypeOf obj Is DateTimePicker Then
                obj.text = ""

            ElseIf TypeOf obj Is GroupBox Then
                grpbx = obj
                For Each ctrl1 In grpbx.Controls
                    If TypeOf ctrl1 Is TextBox Then
                        ctrl1.text = ""
                    ElseIf TypeOf ctrl1 Is ComboBox Then
                        ctrl1.text = ""
                    ElseIf TypeOf ctrl1 Is DateTimePicker Then
                        ctrl1.text = ""
                    End If
                Next

            ElseIf TypeOf obj Is Panel Then
                pnl1 = obj
                If Trim(UCase(pnl1.Name)) <> Trim(UCase(pnl_Filter.Name)) Then
                    For Each ctrl2 In pnl1.Controls
                        If TypeOf ctrl2 Is TextBox Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is ComboBox Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is DataGridView Then
                            ctrl2.Rows.Clear()
                        ElseIf TypeOf ctrl2 Is DateTimePicker Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is Panel Then
                            pnl2 = ctrl2
                            If Trim(UCase(pnl2.Name)) <> Trim(UCase(pnl_Filter.Name)) Then
                                For Each ctrl3 In pnl2.Controls
                                    If TypeOf ctrl3 Is TextBox Then
                                        ctrl3.text = ""
                                    ElseIf TypeOf ctrl3 Is ComboBox Then
                                        ctrl3.text = ""
                                    ElseIf TypeOf ctrl3 Is DateTimePicker Then
                                        ctrl3.text = ""
                                    End If
                                Next
                            End If

                        End If

                    Next

                End If

            End If

        Next



        dtp_Date.Enabled = False
        If Common_Procedures.User.IdNo = 1 Or Trim(Common_Procedures.UR.Weaver_Piece_Approval_Entry_Edit_DateColumn) <> "" Then
            dtp_Date.Enabled = True
        End If

        dgv_PIECE_VERIFICATION_PENDING_DETAILS.Rows.Clear()
        dgv_PIECE_VERIFICATION_PENDING_DefectHidden_Details.Rows.Clear()

        dgv_PIECE_VERIFICATION_PENDING_DefectHidden_Details.Visible = False

        dgv_PieceDetails.Rows.Clear()
        dgv_PieceDetails_Total.Rows.Clear()
        dgv_PieceDetails_Total.Rows.Add()

        dgv_LotDetails.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_defect_Total.Rows.Clear()
        dgv_defect_Total.Rows.Add()

        dgv_DefectHidden_Details.Rows.Clear()

        pnl_defect_details.Visible = False

        '  txt_SlNo.Text = "1"

        'cbo_InwardType.Enabled = True
        'cbo_InwardType.BackColor = Color.White

        cbo_Grid_Defect.Enabled = True
        cbo_Grid_Defect.BackColor = Color.White


    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Or MOV_Status = True Then Exit Sub

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

        If Me.ActiveControl.Name <> dgv_PieceDetails.Name Then
            Grid_Cell_DeSelect()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Defect.Name Then
            cbo_Grid_Defect.Visible = False
            cbo_Grid_Defect.Tag = -100
        End If


        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Or MOV_Status = True Then Exit Sub

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
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
        If FrmLdSTS = True Then Exit Sub
        If Not IsNothing(dgv_PieceDetails.CurrentCell) Then dgv_PieceDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_LotDetails.CurrentCell) Then dgv_LotDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_PieceDetails_Total.CurrentCell) Then dgv_PieceDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim Chk_Lab As Integer = 0
        Dim Unt_Id As Integer = 0
        Dim All_STS As Boolean
        Dim Sl_No As Integer = 0

        If Val(no) = 0 Then Exit Sub

        clear()

        MOV_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try


            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Piece_Approval_Head a where a.Weaver_Piece_Approval_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count > 0 Then

                lbl_CheckingNo.Text = dt1.Rows(0).Item("Weaver_Piece_Approval_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_Piece_Approval_Date").ToString
                txt_Narration.Text = dt1.Rows(0).Item("Narration").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("User_IdNo").ToString))))


                get_Approval_Pending_PieceDetails(-1, NewCode)

                'da1 = New SqlClient.SqlDataAdapter("Select * from Weaver_ClothReceipt_App_PieceChecking_Details a Where Weaver_Piece_Approval_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' order by Checking_Date, for_orderby, Lot_Code, Lot_No, PieceNo_OrderBy, Piece_No", con)
                'dt1 = New DataTable
                'da1.Fill(dt1)

                'With dgv_PieceDetails

                '    .Rows.Clear()
                '    dgv_DefectHidden_Details.Rows.Clear()

                '    SNo = 0

                '    If dt1.Rows.Count > 0 Then

                '        For i = 0 To dt1.Rows.Count - 1

                '            All_STS = False

                '            n = dgv_PieceDetails.Rows.Add()
                '            SNo = SNo + 1
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.Slno).Value = Val(SNo)
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.YEARCODE).Value = dt1.Rows(i).Item("Year_Code").ToString
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.LOT_NO).Value = dt1.Rows(i).Item("Lot_No").ToString
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.CHK_Date).Value = dt1.Rows(i).Item("Checking_Date").ToString

                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.PIECENO).Value = dt1.Rows(i).Item("Piece_No").ToString

                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.REC_MTRS).Value = Format(Val(dt1.Rows(i).Item("Receipt_Meters").ToString), "###########0.00")
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.A_Sounds).Value = Format(Val(dt1.Rows(i).Item("Type1_Meters").ToString), "###########0.00")
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.B_Seconds).Value = Format(Val(dt1.Rows(i).Item("Type2_Meters").ToString), "###########0.00")
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.C_Bits).Value = Format(Val(dt1.Rows(i).Item("Type3_Meters").ToString), "###########0.00")
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.REJECT).Value = Format(Val(dt1.Rows(i).Item("Type4_Meters").ToString), "###########0.00")
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.Others).Value = Format(Val(dt1.Rows(i).Item("Type5_Meters").ToString), "###########0.00")
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.TOTAL_MTRS).Value = Format(Val(dt1.Rows(i).Item("Total_Checking_Meters").ToString), "###########0.00")
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.TOTAL_POINTS).Value = Format(Val(dt1.Rows(i).Item("Piece_Checking_Defect_Points").ToString), "###########0.00")


                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.WEIGHT).Value = dt1.Rows(i).Item("Weight").ToString
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.WGT_MTR).Value = dt1.Rows(i).Item("Weight_Meter").ToString

                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value = dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.LOTCODE).Value = dt1.Rows(i).Item("Lot_Code").ToString
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.PCS_MAINNO).Value = dt1.Rows(i).Item("Piece_MainNo").ToString
                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.PCS_SUBNO).Value = dt1.Rows(i).Item("Piece_SubNo").ToString
                '            All_STS = False
                '            If Val(dt1.Rows(0).Item("Approved_Status").ToString) = 1 Then All_STS = True

                '            dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.APP_STS).Value = All_STS


                '            ''---------------
                '            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Piece_Checking_Defect_Name from Weaver_ClothReceipt_App_Piece_Defect_Details a, Piece_Checking_Defect_head b Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value) & "' and a.Piece_No = '" & Trim(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value) & "' and a.Piece_Checking_Defect_IdNo = b.Piece_Checking_Defect_IdNo ", con)
                '            dt2 = New DataTable
                '            da2.Fill(dt2)

                '            Sl_No = 0
                '            If dt2.Rows.Count > 0 Then

                '                For j = 0 To dt2.Rows.Count - 1

                '                    n = dgv_DefectHidden_Details.Rows.Add()

                '                    dgv_DefectHidden_Details.Rows(n).Cells(0).Value = dt2.Rows(j).Item("Weaver_ClothReceipt_Code").ToString
                '                    dgv_DefectHidden_Details.Rows(n).Cells(1).Value = dt2.Rows(j).Item("Piece_No").ToString
                '                    dgv_DefectHidden_Details.Rows(n).Cells(2).Value = dt2.Rows(j).Item("Piece_Checking_Defect_Name").ToString ' Common_Procedures.Defect_IdNoToName(con, Val(dt2.Rows(j).Item("Piece_Checking_Defect_IdNo").ToString))
                '                    dgv_DefectHidden_Details.Rows(n).Cells(3).Value = dt2.Rows(j).Item("Piece_Checking_Defect_Points").ToString
                '                    dgv_DefectHidden_Details.Rows(n).Cells(4).Value = dt2.Rows(j).Item("Noof_Times").ToString
                '                    dgv_DefectHidden_Details.Rows(n).Cells(5).Value = dt2.Rows(j).Item("Total_PieceChecking_Defect_Points").ToString

                '                Next j

                '            End If
                '            dt2.Dispose()
                '            da2.Dispose()
                '            ''------------------------



                '        Next i

                '    End If
                '    '----------------
                '    For i = 0 To .Rows.Count - 1
                '        dgv_PieceDetails.Rows(i).Cells(0).Value = i + 1
                '    Next

                'End With
                dt1.Clear()






            Else

                Me.new_record()

            End If
            dt1.Clear()

            Grid_Cell_DeSelect()

            MOV_Status = False
            TotalAmount_Calculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            MOV_Status = False

            dt1.Dispose()
            dt2.Dispose()

            da1.Dispose()
            da2.Dispose()

            If dgv_PieceDetails.Enabled And dgv_PieceDetails.Visible Then
                If dgv_PieceDetails.Rows.Count > 0 Then
                    dgv_PieceDetails.Focus()
                    dgv_PieceDetails.CurrentCell = dgv_PieceDetails.Rows(0).Cells(0)
                Else

                    txt_Narration.Focus()
                End If
            Else

                txt_Narration.Focus()
            End If

        End Try


    End Sub

    Private Sub Item_Inward_Checking_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If


            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Defect.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CHECKING MISTAKE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Defect.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Item_Inward_Checking_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load '
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Me.Text = ""

        con.Open()



        'cbo_InwardType.Items.Clear()
        'cbo_InwardType.Items.Add("DELIVERY")


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_defect_details.Visible = False
        pnl_defect_details.Left = (Me.Width - pnl_defect_details.Width) \ 2
        pnl_defect_details.Top = (Me.Height - pnl_defect_details.Height) \ 2
        pnl_defect_details.BringToFront()


        pnl_PIECE_VERIFICATION_PENDING_DETAILS.Visible = False
        pnl_PIECE_VERIFICATION_PENDING_DETAILS.Left = (Me.Width - pnl_PIECE_VERIFICATION_PENDING_DETAILS.Width) \ 2
        pnl_PIECE_VERIFICATION_PENDING_DETAILS.Top = (Me.Height - pnl_PIECE_VERIFICATION_PENDING_DETAILS.Height) \ 2
        pnl_PIECE_VERIFICATION_PENDING_DETAILS.BringToFront()

        btn_BarCodePrint.Visible = False
        btn_BarCodePrint_SinglePieces.Visible = False
        If Val(Common_Procedures.User.IdNo) = 1 Or Trim(Common_Procedures.UR.Weaver_Piece_Approval_Entry_BarCode_Print_Status) <> "" Then
            btn_BarCodePrint.Visible = True
            btn_BarCodePrint_SinglePieces.Visible = True
        End If
        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Narration.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter_Show.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Defect.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Narration.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter_Show.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Defect.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress



        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0



        Filter_Status = False
        FrmLdSTS = True
        new_record()



    End Sub

    Private Sub Item_Inward_Checking_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Item_Inward_Checking_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_PIECE_VERIFICATION_PENDING_DETAILS.Visible = True Then
                    btn_Close_PIECE_VERIFICATION_PENDING_DETAILS_Click(sender, e)
                    Exit Sub
                ElseIf pnl_defect_details.Visible = True Then
                    btn_Defect_Details_Close_Click(sender, e)
                    Exit Sub

                ElseIf MessageBox.Show("Do you want to Close?", "FOR CLOSE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        MessageBox.Show("editing not allowed", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_Piece_Approval_Entry, New_Entry, Me, con, "Weaver_Piece_Approval_Head", "Weaver_Piece_Approval_Code", NewCode, "Weaver_Piece_Approval_Date", "(Weaver_Piece_Approval_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        'User Modification
        'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Weaver_Piece_Approval_Head", "Weaver_Piece_Approval_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Weaver_Piece_Approval_Code, Company_IdNo, for_OrderBy", trans)


    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Exit Sub

        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.ledger_idno = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
            da.Fill(dt2)
            cbo_Filter_ItemName.DataSource = dt2
            cbo_Filter_ItemName.DisplayMember = "item_name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Approval_No from Weaver_Piece_Approval_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & "   Order by for_Orderby, Weaver_Piece_Approval_No", con)
            dt = New DataTable
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
        Dim OrdByNo As Double = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_CheckingNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Approval_No from Weaver_Piece_Approval_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & "  Order by for_Orderby, Weaver_Piece_Approval_No", con)
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

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub


    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Double = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_CheckingNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Approval_No from Weaver_Piece_Approval_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " Order by for_Orderby desc, Weaver_Piece_Approval_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Approval_No from Weaver_Piece_Approval_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & "  Order by for_Orderby desc, Weaver_Piece_Approval_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As exception
            messagebox.show(ex.message, "for  moving...", messageboxbuttons.okcancel, messageboxicon.error)

        Finally
            dt.dispose()
            da.dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Try

            clear()

            New_Entry = True

            lbl_CheckingNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Piece_Approval_Head", "Weaver_Piece_Approval_Code", "For_OrderBy", "Weaver_Piece_Approval_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_CheckingNo.ForeColor = Color.Red

            get_Approval_Pending_LotDetails()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            'If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            If dgv_LotDetails.Enabled And dgv_LotDetails.Visible Then
                If dgv_LotDetails.Rows.Count > 0 Then
                    dgv_LotDetails.Focus()
                    dgv_LotDetails.CurrentCell = dgv_LotDetails.Rows(0).Cells(dgvCol_LotDetails.Slno)
                Else

                    txt_Narration.Focus()
                End If
            Else

                txt_Narration.Focus()
            End If

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Approval No.", "FOR FINDING...")

            RefCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Approval_No from Weaver_Piece_Approval_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Approval_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Invocie No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_PieceDetails.Name Or ActiveControl.Name = dgv_defect_details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_PieceDetails.Name Then
                dgv1 = dgv_PieceDetails

            ElseIf dgv_PieceDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_PieceDetails

            ElseIf ActiveControl.Name = dgv_defect_details.Name Then
                dgv1 = dgv_defect_details

            ElseIf dgv_defect_details.IsCurrentRowDirty = True Then
                dgv1 = dgv_defect_details

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_PieceDetails

            ElseIf pnl_defect_details.Enabled = True Then
                dgv1 = dgv_defect_details
            Else


            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_PieceDetails.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then



                            If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(1).Value) = 0 Then

                                    If txt_Narration.Enabled And txt_Narration.Visible Then
                                        txt_Narration.Focus()
                                    Else
                                        save_record()
                                    End If

                                Else

                                    If .CurrentCell.RowIndex = .RowCount - 1 Then
                                        txt_Narration.Focus()
                                    Else
                                        '.Rows.Add()
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                    End If

                                End If
                            ElseIf .CurrentCell.ColumnIndex = dgvCol_PieceDetails.B_Seconds Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                            ElseIf .CurrentCell.ColumnIndex = dgvCol_PieceDetails.TOTAL_MTRS Then
                                txt_Narration.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then

                                    dtp_Date.Focus()

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


                    ElseIf dgv1.Name = dgv_defect_details.Name Then


                        If keyData = Keys.Enter Or keyData = Keys.Down Then


                            If .CurrentCell.RowIndex = .RowCount - 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                                DefectPoint_Close()

                            ElseIf .CurrentCell.ColumnIndex = 3 Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            Else
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    txt_Narration.Focus()
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                End If

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then
                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    dtp_Date.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(3)

                                End If

                            ElseIf .CurrentCell.ColumnIndex >= 2 Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(1)

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

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Receipt_Inward_Checking_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Receipt_Inward_Checking_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Piece_Approval_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Invocie No.", "FOR NEW INVOICE INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Approval_No from Weaver_Piece_Approval_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Inward_Check_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Invoice No.", "DOES NOT INSERT NEW INVOICE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_CheckingNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW INVOICE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vWEV_ID As Integer = 0
        Dim vFoldr_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim vChkr_ID As Single = 0
        Dim vNOOFPCS As Integer = 0
        Dim vforOrdby As Double = 0
        Dim Amt As Single = 0
        Dim L_ID As Integer = 0
        Dim chk_Lab As Integer = 0
        Dim VouBil As String = ""
        Dim vGOD_ID As Integer = 0
        Dim vSTFOFF_ID As Integer = 0
        Dim vCLTH_IDNO As Integer = 0
        Dim vSaTotQty As Single = 0
        Dim vTotExcSHt As Integer = 0
        Dim nr As Integer
        Dim vBrCode_Typ1 As String = "", vBrCode_Typ2 As String = "", vBrCode_Typ3 As String = "", vBrCode_Typ4 As String = "", vBrCode_Typ5 As String = ""
        Dim vOrdByPieceNo As String = 0
        Dim vOrdByRecNo As String = 0
        Dim SQL1 As String
        Dim vPCSCHKCODE As String = ""
        Dim vWdth_Typ As String = ""
        Dim vLOOMTYP As String = ""
        Dim vLM_ID As Integer = 0
        Dim vWAGESCODE As String = ""
        Dim vPCSCHKNO As String = ""
        Dim vPCSCHKDATE As Date
        Dim vRCPTTYPE As String = ""
        Dim vPDcNo As String = ""
        Dim vRECPCS As Integer = 0
        Dim vRECMTRS As String = 0
        Dim vRECDATE As Date
        Dim vLED_TYPE As String
        Dim vSTKOFF_POS_IDNO As Integer
        Dim vChecker_id As String = 0
        Dim vFolder_id As String = 0
        Dim vLotCodSel As String = ""
        Dim vPcs_ChkrId As String = 0
        Dim vPcs_FoldrId As String = 0
        Dim vRmks As String = ""
        Dim vLmNo As String = ""
        Dim vCHKDATE As Date = #1/1/2000#
        Dim vChkr_Wgs_per_Mtr As String = ""
        Dim vFldr_Wgs_per_Mtr As String = ""
        Dim vCREATED_DTTM_TXT As String = ""
        Dim vFAB_LOTCODE As String = ""
        Dim vERRMSG As String = ""




        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Piece_Approval_Entry, New_Entry, Me, con, "Weaver_Piece_Approval_Head", "Weaver_Piece_Approval_Code", NewCode, "Weaver_Piece_Approval_Date", "(Weaver_Piece_Approval_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Approval_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Piece_Approval_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

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



        If New_Entry = False Then
            MessageBox.Show("editing not allowed", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
            Exit Sub
        End If


        With dgv_PieceDetails

            Sno = 0
            vNOOFPCS = 0

            For i = 0 To .Rows.Count - 1

                If Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) <> "" And Trim(.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value) <> "" Then

                    If .Rows(i).Cells(dgvCol_PieceDetails.APP_STS).Value = True Then

                        vNOOFPCS = vNOOFPCS + 1

                        vCHKDATE = #1/1/2000#

                        If Trim(.Rows(i).Cells(dgvCol_PieceDetails.CHK_Date).Value) <> "" Then
                            If IsDate(.Rows(i).Cells(dgvCol_PieceDetails.CHK_Date).Value) = True Then
                                vCHKDATE = CDate(Trim(.Rows(i).Cells(dgvCol_PieceDetails.CHK_Date).Value))
                            End If
                        End If

                        If DateDiff(DateInterval.Day, dtp_Date.Value.Date, vCHKDATE.Date) <> 0 Then
                            MessageBox.Show("Actual Piece Checking Date not equal to Piece Approval Date fpr Piece No. : " & Trim(.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dtp_Date.Enabled Then
                                dtp_Date.Focus()
                            Else
                                If .Enabled And .Visible Then
                                    .Focus()
                                    .CurrentCell = .Rows(i).Cells(dgvCol_PieceDetails.CHK_Date)
                                End If
                            End If
                            Exit Sub
                        End If

                    End If

                End If

            Next i

        End With

        If vNOOFPCS = 0 Then
            MessageBox.Show("no pieces approved to save", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
            Exit Sub
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_CheckingNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Piece_Approval_Head", "Weaver_Piece_Approval_Code", "For_OrderBy", "Weaver_Piece_Approval_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PieceApproval", dtp_Date.Value.Date)
            'cmd.Parameters.AddWithValue("@SalescheckingDate", dtp_Date.Value.Date)

            vforOrdby = Val(Common_Procedures.OrderBy_CodeToValue(lbl_CheckingNo.Text))

            If New_Entry = True Then


                cmd.CommandText = "Insert into Weaver_Piece_Approval_Head (       Weaver_Piece_Approval_Code          ,              Company_IdNo        ,        Weaver_Piece_Approval_No     ,        for_OrderBy    ,         Weaver_Piece_Approval_Date ,           User_IdNo   ,                            Narration           )    " &
                                  " Values                                ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_CheckingNo.Text) & "', " & Str(Val(vforOrdby)) & ",       @PieceApproval  ,        " & Val(Common_Procedures.User.IdNo) & ", '" & Trim(txt_Narration.Text) & "' )"
                cmd.ExecuteNonQuery()

                With dgv_PieceDetails

                    Sno = 0
                    vFAB_LOTCODE = ""

                    For i = 0 To .Rows.Count - 1

                        If Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) <> "" Then

                            vFAB_LOTCODE = Trim(vFAB_LOTCODE) & "~" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) & "~"

                            Sno = Sno + 1

                            If dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.APP_STS).Value = True Then

                                vLotCodSel = Trim(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))


                                '-----------------Defect Details

                                cmd.CommandText = "Delete from Weaver_ClothReceipt_App_Piece_Defect_Details Where Weaver_ClothReceipt_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value) & "' and  Piece_No = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " "
                                cmd.ExecuteNonQuery()


                                With dgv_DefectHidden_Details

                                    Dim vPcsDeft_Id As Integer = 0


                                    Sno = 0
                                    For j = 0 To .RowCount - 1

                                        If Trim(.Rows(j).Cells(0).Value) = Trim(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value) And Trim(.Rows(j).Cells(1).Value) = Trim(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value) Then
                                            Sno = Sno + 1


                                            vPcsDeft_Id = Common_Procedures.Defect_NameToIdNo(con, .Rows(j).Cells(2).Value, tr)
                                            cmd.Parameters.Clear()
                                            cmd.Parameters.AddWithValue("@ChkDate", Today.Date)
                                            cmd.Parameters.AddWithValue("@ChkDateTime", Convert.ToDateTime(Today.Date))

                                            cmd.CommandText = "Insert Into Weaver_ClothReceipt_App_Piece_Defect_Details (                 Company_Idno ,               Receipt_PkCondition  ,                      Year_Code   ,                            Weaver_ClothReceipt_Code    ,                    for_orderby ,                                         Lot_Code  ,                                                      Lot_No     ,                                                               LotCode_Selection ,            Checking_Date   ,     CheckingDate_Text     ,   Checking_DateTime   ,   CheckingDateTime_Text   ,               Piece_No     ,                         PieceNo_OrderBy     ,                             Checking_Table_IdNo  ,                                                                   Piece_MainNo  ,                                                                                Piece_SubNo   ,                                                           Checker_Idno       ,    Piece_Checking_Defect_IdNo ,       Piece_Checking_Defect_Points        ,                User_Idno  ,                                   Noof_Times   ,                  Total_PieceChecking_Defect_Points   )" &
                                                                                                           "   Values (   " & Str(Val(lbl_Company.Tag)) & "       ,    'WCLRC-'    ,          '" & Trim(Common_Procedures.FnYearCode) & "' ,    '" & Trim(.Rows(j).Cells(0).Value) & "' ,             " & Str(Val(vOrdByRecNo)) & "  ,  '" & Trim(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) & "', '" & Trim(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value) & "'  ,  '" & Trim(vLotCodSel) & "' ,        @ChkDate        ,    @ChkDate          ,       @ChkDateTime         ,      @ChkDateTime  ,      '" & Trim(.Rows(j).Cells(1).Value) & "',    " & Str(Val(vOrdByPieceNo)) & " ,       " & Str(Val(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.TABLENO).Value)) & "       ,   '" & Trim(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.PCS_MAINNO).Value) & "' , '" & Trim(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.PCS_SUBNO).Value) & "' ,  " & (Val(vChecker_id)) & "   ,  " & (Val(vPcsDeft_Id)) & "   ,  " & Val((.Rows(j).Cells(3).Value)) & "  , " & Val(Common_Procedures.User.IdNo) & " ,  " & Val((.Rows(j).Cells(4).Value)) & "    ,  " & Val((.Rows(j).Cells(5).Value)) & "     ) "
                                            nr = cmd.ExecuteNonQuery()


                                        End If

                                    Next j

                                    Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_ClothReceipt_App_Piece_Defect_Details", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_CheckingNo.Text, Val(vforOrdby), Pk_Condition, "", "", New_Entry, False, " Piece_Checking_Defect_idno, Piece_Checking_Defect_Points , Noof_Times, Total_PieceChecking_Defect_Points , User_Idno ", "Piece_No", "Weaver_ClothReceipt_Code, Piece_No, Company_IdNo,  Checking_Date ", tr)

                                End With

                                '////////////////////////////////////////////////////////////////////////////////////


                                '------------------Remarks

                                cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempSubTable) & ""
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(name1, name2, name3 , Int1, Int2) Select a.LotCode_Selection , a.piece_no, (CASE WHEN b.Piece_Checking_Defect_shortname <> '' THEN b.Piece_Checking_Defect_shortname ELSE b.Piece_Checking_Defect_Name END) as defectname , a.Noof_Times, a.Total_PieceChecking_Defect_Points  from Weaver_ClothReceipt_App_Piece_Defect_Details a, Piece_Checking_Defect_head b where a.LotCode_Selection = '" & Trim(vLotCodSel) & "' and a.piece_no = '" & Trim(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value) & "' and a.Piece_Checking_Defect_IdNo = b.Piece_Checking_Defect_IdNo and a.Total_PieceChecking_Defect_Points <> 0  "
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(name1, name2, name3, Int1) select name1, name2, name3 + '(' + cast(sum(Int1) as varchar) + ')', sum(Int2) from " & Trim(Common_Procedures.EntryTempSubTable) & " group by name1, name2, name3 having sum(Int1) <> 0"
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempSubTable) & ""
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(name1, name2, name3, Int1) select name1, name2, STUFF((SELECT ', ' + convert(varchar(10), name3, 120) FROM " & Trim(Common_Procedures.EntryTempTable) & " b where a.Name1 = b.Name1 and a.Name2 = b.Name2 FOR XML PATH ('')) , 1, 1, '')  AS name333, sum(int1) from " & Trim(Common_Procedures.EntryTempTable) & " a group by name1, name2, name3"
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(name1, name2, name3, Int1) select name1, name2, name3 , sum(int1) from " & Trim(Common_Procedures.EntryTempSubTable) & " group by name1, name2, name3 having sum(Int1) <> 0"
                                cmd.ExecuteNonQuery()

                                vRmks = ""
                                da = New SqlClient.SqlDataAdapter("select Name3 from " & Trim(Common_Procedures.EntryTempTable) & " ", con)
                                da.SelectCommand.Transaction = tr
                                dt2 = New DataTable
                                da.Fill(dt2)
                                If dt2.Rows.Count > 0 Then
                                    vRmks = dt2.Rows(0).Item("name3").ToString
                                End If
                                dt2.Clear()

                                '------------------

                                nr = 0

                                'Old
                                'cmd.CommandText = "Update Weaver_ClothReceipt_App_PieceChecking_Details set Weaver_Piece_Approval_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ,  Approved_Status =  1  Where  Weaver_ClothReceipt_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value) & "' And  Lot_code  = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value) & "' and Weaver_Piece_Approval_Code = '' "
                                'nr = cmd.ExecuteNonQuery()

                                cmd.CommandText = "Update Weaver_ClothReceipt_App_PieceChecking_Details set Weaver_Piece_Approval_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ,  Approved_Status =  1 , Receipt_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.REC_MTRS).Value)) & "  , Type1_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.A_Sounds).Value)) & ", Type2_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.B_Seconds).Value)) & ", Type3_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.C_Bits).Value)) & " , Type4_Meters =" & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.REJECT).Value)) & " , Type5_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.Others).Value)) & ", Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.TOTAL_MTRS).Value)) & " ,Piece_Checking_Defect_Points = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.TOTAL_POINTS).Value)) & ", Defect_Remarks = '" & Trim(vRmks) & "'  Where  Weaver_ClothReceipt_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value) & "' And  Lot_code  = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value) & "' and Weaver_Piece_Approval_Code = '' "
                                nr = cmd.ExecuteNonQuery()

                                If nr = 0 Then
                                    Throw New ApplicationException("Error in Updation : Invalid Piece Details")
                                    Exit Sub
                                End If

                                '////////////////// Weaver_ClothReceipt_Piece_Details Updation

                                vBrCode_Typ1 = ""
                                vBrCode_Typ2 = ""
                                vBrCode_Typ3 = ""
                                vBrCode_Typ4 = ""
                                vBrCode_Typ5 = ""

                                If Val(.Rows(i).Cells(dgvCol_PieceDetails.A_Sounds).Value) <> 0 Then
                                    vBrCode_Typ1 = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value)) & Trim(UCase((.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value))) & "1"
                                End If
                                If Val(.Rows(i).Cells(dgvCol_PieceDetails.B_Seconds).Value) <> 0 Then
                                    vBrCode_Typ2 = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value)) & Trim(UCase((.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value))) & "2"
                                End If
                                If Val(.Rows(i).Cells(dgvCol_PieceDetails.C_Bits).Value) <> 0 Then
                                    vBrCode_Typ3 = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value)) & Trim(UCase((.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value))) & "3"
                                End If
                                If Val(.Rows(i).Cells(dgvCol_PieceDetails.REJECT).Value) <> 0 Then
                                    vBrCode_Typ4 = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value)) & Trim(UCase((.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value))) & "4"
                                End If
                                If Val(.Rows(i).Cells(dgvCol_PieceDetails.Others).Value) <> 0 Then
                                    vBrCode_Typ5 = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value)) & Trim(UCase((.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value))) & "5"
                                End If

                                vChkr_ID = 0
                                'If Trim(dgv_Details.Rows(i).Cells(dgvCol_PieceDetails.CHECKERNAME).Value) <> "" Then
                                '    vChkr_ID = Common_Procedures.Employee_Simple_NameToIdNo(con, dgv_Details.Rows(i).Cells(dgvCol_PieceDetails.CHECKERNAME).Value, tr)
                                'End If

                                vFoldr_ID = 0
                                'If Trim(dgv_Details.Rows(i).Cells(dgvCol_PieceDetails.FOLDERNAME).Value) <> "" Then
                                '    vFoldr_ID = Common_Procedures.Employee_Simple_NameToIdNo(con, dgv_Details.Rows(i).Cells(dgvCol_PieceDetails.FOLDERNAME).Value, tr)
                                'End If


                                vOrdByRecNo = Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value))
                                vOrdByPieceNo = Common_Procedures.OrderBy_CodeToValue(Val(.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value))

                                da = New SqlClient.SqlDataAdapter("select Weaver_ClothReceipt_Date, Weaver_Piece_Checking_Code, Weaver_IR_Wages_Code, Weaver_Wages_Code, Loom_IdNo, Width_Type, Loom_Type, Ledger_IdNo, Cloth_IdNo, Party_DcNo, noof_pcs, Receipt_Meters, StockOff_IdNo, WareHouse_IdNo, Dc_Receipt_Meters from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) & "'", con)
                                da.SelectCommand.Transaction = tr
                                dt1 = New DataTable
                                da.Fill(dt1)

                                vPCSCHKCODE = ""
                                vWdth_Typ = ""
                                vLOOMTYP = ""
                                vLM_ID = 0
                                vWAGESCODE = ""
                                vCLTH_IDNO = 0
                                vWEV_ID = 0

                                vPDcNo = ""
                                vRECPCS = 0
                                vRECMTRS = 0
                                vSTFOFF_ID = 0
                                vRECDATE = #1/1/1900#
                                vGOD_ID = 0
                                If dt1.Rows.Count > 0 Then
                                    If IsDBNull(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                                        vPCSCHKCODE = dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                                    End If
                                    If IsDBNull(dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                                        vWAGESCODE = dt1.Rows(0).Item("Weaver_Wages_Code").ToString
                                    End If
                                    If Trim(vWAGESCODE) = "" Then
                                        If IsDBNull(dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString) = False Then
                                            vWAGESCODE = dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString
                                        End If
                                    End If
                                    vRECDATE = dt1.Rows(0).Item("Weaver_ClothReceipt_Date")
                                    vLM_ID = Val(dt1.Rows(0).Item("Loom_IdNo").ToString)
                                    vWdth_Typ = dt1.Rows(0).Item("Width_Type").ToString
                                    vLOOMTYP = dt1.Rows(0).Item("Loom_Type").ToString
                                    vWEV_ID = Val(dt1.Rows(0).Item("Ledger_IdNo").ToString)
                                    vCLTH_IDNO = Val(dt1.Rows(0).Item("Cloth_IdNo").ToString)
                                    vPDcNo = dt1.Rows(0).Item("Party_DcNo").ToString
                                    vRECPCS = Val(dt1.Rows(0).Item("noof_pcs").ToString)
                                    If Val(dt1.Rows(0).Item("Dc_Receipt_Meters").ToString) > 0 Then
                                        vRECMTRS = Val(dt1.Rows(0).Item("Dc_Receipt_Meters").ToString)
                                    Else
                                        vRECMTRS = Val(dt1.Rows(0).Item("Receipt_Meters").ToString)
                                    End If

                                    vSTFOFF_ID = Val(dt1.Rows(0).Item("StockOff_IdNo").ToString)
                                    vGOD_ID = Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString)

                                End If
                                dt1.Clear()

                                If vGOD_ID = 0 Then vGOD_ID = Common_Procedures.CommonLedger.Godown_Ac

                                '-----------------





                                vChecker_id = 0
                                vFolder_id = 0
                                da = New SqlClient.SqlDataAdapter("select * from Lot_Allotment_Details Where Lotcode_ForSelection = '" & Trim(vLotCodSel) & "' ", con)
                                da.SelectCommand.Transaction = tr
                                dt2 = New DataTable
                                da.Fill(dt2)

                                If dt2.Rows.Count > 0 Then
                                    vChecker_id = Val(dt2.Rows(0).Item("Checker_Idno_IRwages").ToString)
                                    vFolder_id = Val(dt2.Rows(0).Item("Folder_Idno_IRwages").ToString)
                                End If
                                dt2.Clear()

                                '---------------

                                '---------------Pcs Chking Folder Checker

                                vPcs_ChkrId = 0
                                da = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_App_PieceChecking_Details Where Lotcode_Selection = '" & Trim(vLotCodSel) & "' ", con)
                                da.SelectCommand.Transaction = tr
                                dt3 = New DataTable
                                da.Fill(dt3)

                                If dt3.Rows.Count > 0 Then
                                    vPcs_ChkrId = Val(dt3.Rows(0).Item("Folder_Idno").ToString)
                                End If
                                dt3.Clear()

                                vPcs_FoldrId = 0
                                da = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_App_Piece_Defect_Details Where Lotcode_Selection = '" & Trim(vLotCodSel) & "' ", con)
                                da.SelectCommand.Transaction = tr
                                dt4 = New DataTable
                                da.Fill(dt4)

                                If dt4.Rows.Count > 0 Then
                                    vPcs_FoldrId = Val(dt4.Rows(0).Item("Checker_Idno").ToString)
                                End If
                                dt4.Clear()
                                '---------------



                                '----------------LoomNo
                                vLmNo = ""
                                da = New SqlClient.SqlDataAdapter("select Loom_No from Weaver_ClothReceipt_App_PieceChecking_Details Where Lotcode_Selection = '" & Trim(vLotCodSel) & "' and Piece_No = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value) & "'", con)
                                da.SelectCommand.Transaction = tr
                                dt3 = New DataTable
                                da.Fill(dt3)
                                If dt3.Rows.Count > 0 Then
                                    vLmNo = dt3.Rows(0).Item("Loom_No").ToString
                                End If
                                dt3.Clear()


                                '----------------

                                vPCSCHKNO = ""
                                vPCSCHKDATE = dtp_Date.Value.Date


                                If Trim(vPCSCHKCODE) = "" Then

                                    PkCondition_Entry = ""
                                    If Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.REC_PK).Value)) = "CLSRT-" Or Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.REC_PK).Value)) = "GCLSR-" Or Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.REC_PK).Value)) = "CLDRT-" Then
                                        'Label1.Text = "PIECE CHECKING (SALES)"
                                        PkCondition_Entry = "SPCCK-"
                                        'Other_Condition = "(Receipt_Type = 'S')"
                                        vRCPTTYPE = "S"


                                    ElseIf Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.REC_PK).Value)) = "CPREC-" Then
                                        'Label1.Text = "PIECE CHECKING (PURCHASE)"
                                        PkCondition_Entry = "PPCCK-"
                                        'Other_Condition = "(Receipt_Type = 'P')"
                                        vRCPTTYPE = "P"
                                        'ElseIf Trim(UCase(vEntryType)) = "WEAVER" Then
                                        '    Label1.Text = "PIECE CHECKING (WEAVER)"
                                        '    PkCondition_Entry = ""


                                    Else
                                        'Label1.Text = "PIECE CHECKING (WEAVER)"
                                        PkCondition_Entry = ""
                                        'Other_Condition = "(Receipt_Type = '' or Receipt_Type = 'W')"
                                        vRCPTTYPE = "W"

                                    End If

                                    vPCSCHKNO = Common_Procedures.get_MaxCode(con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", "For_OrderBy", "(Receipt_Type <> 'L')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                                    vPCSCHKCODE = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(vPCSCHKNO) & "/" & Trim(Common_Procedures.FnYearCode)

                                    cmd.Parameters.Clear()
                                    cmd.Parameters.AddWithValue("@CheckingDate", Convert.ToDateTime(dtp_Date.Text))
                                    cmd.Parameters.AddWithValue("@RecDate", vRECDATE)

                                    vCREATED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
                                    cmd.Parameters.AddWithValue("@createddatetime", Now)


                                    cmd.CommandText = "Insert into Weaver_Piece_Checking_Head (              Receipt_Type         ,    Weaver_Piece_Checking_Code,               Company_IdNo       ,   Weaver_Piece_Checking_No ,                               for_OrderBy                         , Weaver_Piece_Checking_Date,       Ledger_IdNo       ,                                Receipt_PkCondition                            ,                               Piece_Receipt_Code                        ,                               Piece_Receipt_No                               , Piece_Receipt_Date,                               Lot_No                                         ,          Cloth_IdNo       ,             Party_DcNo   ,               noof_pcs   ,    ReceiptMeters_Receipt  ,                          Folding                                    ,           StockOff_IdNo      ,                           user_idNo      , Verified_Status ,  Approved_Status ,             Checker_Idno     ,           Folder_Idno        , Cloth_TransferTo_Idno,                 Loom_Type      ,                    created_useridno           ,   created_DateTime,          created_DateTime_Text    , Last_modified_useridno, Last_modified_DateTime, Last_modified_DateTime_Text ) " &
                                                                "     Values                  ( '" & Trim(UCase(vRCPTTYPE)) & "'  , '" & Trim(vPCSCHKCODE) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vPCSCHKNO) & "'   , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vPCSCHKNO))) & ",        @CheckingDate      , " & Str(Val(vWEV_ID)) & ", '" & Trim(Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.REC_PK).Value))) & "', '" & Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value)) & "', '" & Trim(Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value))) & "',      @RecDate     , '" & Trim(Trim(UCase(.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value))) & "',  " & Str(Val(vCLTH_IDNO)) & ", '" & Trim(vPDcNo) & "', " & Str(Val(vRECPCS)) & ", " & Str(Val(vRECMTRS)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.FOLDING).Value)) & ", " & Str(Val(vSTFOFF_ID)) & " , " & Val(Common_Procedures.User.IdNo) & " ,         0       ,      0           , " & Str(Val(vChecker_id)) & ",  " & Str(Val(vFolder_id)) & ",            0         , '" & Trim(UCase(vLOOMTYP)) & "',  " & Str(Val(Common_Procedures.User.IdNo)) & ",  @createddatetime ,  '" & Trim(vCREATED_DTTM_TXT) & "',              0        ,     NUll              ,          ''                  ) "
                                    cmd.ExecuteNonQuery()

                                Else

                                    da = New SqlClient.SqlDataAdapter("Select Weaver_Piece_Checking_Code, Company_IdNo, Weaver_Piece_Checking_No, for_OrderBy, Weaver_Piece_Checking_Date from Weaver_Piece_Checking_Head Where Weaver_Piece_Checking_Code = '" & Trim(vPCSCHKCODE) & "'", con)
                                    da.SelectCommand.Transaction = tr
                                    dt1 = New DataTable
                                    da.Fill(dt1)
                                    If dt1.Rows.Count > 0 Then
                                        vPCSCHKNO = dt1.Rows(0).Item("Weaver_Piece_Checking_No").ToString
                                        vPCSCHKDATE = dt1.Rows(0).Item("Weaver_Piece_Checking_Date").ToString
                                    End If
                                    dt1.Clear()

                                    cmd.Parameters.Clear()
                                    cmd.Parameters.AddWithValue("@CheckingDate", vPCSCHKDATE)
                                    cmd.Parameters.AddWithValue("@RecDate", vRECDATE)

                                    cmd.CommandText = "Update Weaver_Piece_Checking_Head Set ReceiptMeters_Receipt = " & Str(Val(vRECMTRS)) & " Where Weaver_Piece_Checking_Code = '" & Trim(vPCSCHKCODE) & "'"
                                    cmd.ExecuteNonQuery()


                                End If

                                vLED_TYPE = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(vWEV_ID)) & ")", , tr)

                                vSTKOFF_POS_IDNO = 0

                                If Trim(UCase(vLED_TYPE)) = "JOBWORKER" Then
                                    vSTKOFF_POS_IDNO = vWEV_ID
                                Else
                                    vSTKOFF_POS_IDNO = Val(Common_Procedures.CommonLedger.OwnSort_Ac)    '--- Val(Common_Procedures.CommonLedger.Godown_Ac)
                                End If

                                vChkr_Wgs_per_Mtr = ""
                                vFldr_Wgs_per_Mtr = ""

                                da3 = New SqlClient.SqlDataAdapter("select a.Checking_Wages_Meter, a.Folding_Wages_Meter  from  LoomType_Head a INNER JOIN cloth_head b ON a.loomType_idno = b.loom_Type_idno where b.cloth_idno = " & vCLTH_IDNO & " ", con)
                                da3.SelectCommand.Transaction = tr
                                dt3 = New DataTable
                                da3.Fill(dt3)
                                If dt3.Rows.Count > 0 Then
                                    vChkr_Wgs_per_Mtr = Val(dt3.Rows(0).Item("Checking_Wages_Meter").ToString)
                                    vFldr_Wgs_per_Mtr = Val(dt3.Rows(0).Item("Folding_Wages_Meter").ToString)
                                End If
                                dt3.Clear()

                                nr = 0
                                SQL1 = "Update Weaver_ClothReceipt_Piece_Details set  Weaver_Piece_Checking_Code = '" & Trim(vPCSCHKCODE) & "',  Weaver_Piece_Checking_No = '" & Trim(vPCSCHKNO) & "', Weaver_Piece_Checking_Date = '" & Trim(Format(vPCSCHKDATE, "MM/dd/yyyy")) & "', for_orderby = " & Str(Val(vPCSCHKNO)) & ", Lot_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) & "' , Lot_No = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value) & "' , Ledger_Idno = " & Str(Val(vWEV_ID)) & ", StockOff_IdNo = " & Str(Val(vSTKOFF_POS_IDNO)) & ", Cloth_IdNo = " & Str(Val(vCLTH_IDNO)) & ", Folding_Checking = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.FOLDING).Value)) & ", Folding = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.FOLDING).Value)) & ", Sl_No = " & Str(Val(Sno)) & ", main_pieceno = '" & Trim(Val(.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value)) & "', PieceNo_OrderBy = " & Str(Val(vOrdByPieceNo)) & ", ReceiptMeters_Checking = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.REC_MTRS).Value)) & ", Receipt_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.REC_MTRS).Value)) & ", Loom_No =  '" & Trim(vLmNo) & "' , Pick = 0, Width = 0, Type1_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.A_Sounds).Value)) & ", Type2_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.B_Seconds).Value)) & ", Type3_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.C_Bits).Value)) & ", Type4_Meters  = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.REJECT).Value)) & ", Type5_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.Others).Value)) & ", Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.TOTAL_MTRS).Value)) & ", Weight = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.WEIGHT).Value)) & ", Weight_Meter = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.WGT_MTR).Value)) & ", Remarks = '" & Trim(vRmks) & "' , WareHouse_IdNo = " & Str(Val(vGOD_ID)) & ", Checked_Pcs_Barcode_Type1 = '" & Trim(vBrCode_Typ1) & "', Checked_Pcs_Barcode_Type2 = '" & Trim(vBrCode_Typ2) & "', Checked_Pcs_Barcode_Type3 = '" & Trim(vBrCode_Typ3) & "', Checked_Pcs_Barcode_Type4 = '" & Trim(vBrCode_Typ4) & "', Checked_Pcs_Barcode_Type5 = '" & Trim(vBrCode_Typ5) & "', Checker_Idno = " & (Val(vChecker_id)) & " , Folder_idno = " & (Val(vFolder_id)) & ", Checker_Wgs_per_Mtr = " & Val(vChkr_Wgs_per_Mtr) & ", Folder_Wgs_per_Mtr = 0, Total_CheckingMeters_100Folding = 0, ExcessShort_Status_YesNo = '', Excess_Short_Meter = 0, BeamNo_SetCode = '' , Pcs_Checker_Idno = " & (Val(vPcs_ChkrId)) & "   ,  Pcs_Folder_Idno = " & (Val(vPcs_FoldrId)) & "    Where Weaver_ClothReceipt_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value) & "' and Lot_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value) & "'"
                                cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
                                nr = cmd.ExecuteNonQuery()

                                If nr = 0 Then

                                    SQL1 = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code     ,             Company_IdNo         ,   Weaver_Piece_Checking_No ,              Weaver_Piece_Checking_Date            ,                    Weaver_ClothReceipt_Code                             ,                      Weaver_ClothReceipt_No                       ,          for_orderby         ,           Weaver_ClothReceipt_Date             ,                    Lot_Code                                      ,                    Lot_No                                       ,           Ledger_Idno    ,            StockOff_IdNo          ,              Cloth_IdNo     ,                      Folding_Checking                              ,                      Folding                                       ,           Sl_No      ,                         Piece_No                                 ,                       main_pieceno                                     ,          PieceNo_OrderBy        ,                       ReceiptMeters_Checking                        ,                       Receipt_Meters                                  ,            Loom_No,                 Pick ,  Width ,                      Type1_Meters                                   ,                      Type2_Meters                                    ,                      Type3_Meters                                  ,                      Type4_Meters                                 ,                      Type5_Meters                                 ,                      Total_Checking_Meters                            ,                      Weight                                     ,                      Weight_Meter                                     ,              Remarks ,        WareHouse_IdNo     ,   Checked_Pcs_Barcode_Type1 ,   Checked_Pcs_Barcode_Type2 ,   Checked_Pcs_Barcode_Type3 ,   Checked_Pcs_Barcode_Type4 ,   Checked_Pcs_Barcode_Type5  ,          CHecker_idno     ,          Folder_idno        ,        Checker_Wgs_per_Mtr    ,        Folder_Wgs_per_Mtr      , Total_CheckingMeters_100Folding ,  ExcessShort_Status_YesNo ,  Excess_Short_Meter ,  BeamNo_SetCode ,        Pcs_Checker_Idno    ,        Pcs_Folder_Idno ) "
                                    SQL1 = SQL1 & "     Values                            (    '" & Trim(vPCSCHKCODE) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(vPCSCHKNO) & "' ,  '" & Trim(Format(vPCSCHKDATE, "MM/dd/yyyy")) & "' , '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value) & "',   '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value) & "', " & Str(Val(vOrdByRecNo)) & ", '" & Trim(Format(vRECDATE, "MM/dd/yyyy")) & "' , '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) & "', '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value) & "', " & Str(Val(vWEV_ID)) & ", " & Str(Val(vSTKOFF_POS_IDNO)) & ", " & Str(Val(vCLTH_IDNO)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.FOLDING).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.FOLDING).Value)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value) & "',  '" & Trim(Val(.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value)) & "',  " & Str(Val(vOrdByPieceNo)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.REC_MTRS).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.REC_MTRS).Value)) & ",   '" & Trim(vLmNo) & "'    ,   0  ,     0  , " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.A_Sounds).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.B_Seconds).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.C_Bits).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.REJECT).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.Others).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.TOTAL_MTRS).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.WEIGHT).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.WGT_MTR).Value)) & " ,     '" & Trim(vRmks) & "'   , " & Str(Val(vGOD_ID)) & " , '" & Trim(vBrCode_Typ1) & "', '" & Trim(vBrCode_Typ2) & "', '" & Trim(vBrCode_Typ3) & "', '" & Trim(vBrCode_Typ4) & "', '" & Trim(vBrCode_Typ5) & "' ," & Str(Val(vChecker_id)) & " , " & Str(Val(vFolder_id)) & " , " & Val(vChkr_Wgs_per_Mtr) & ", " & Val(vFldr_Wgs_per_Mtr) & " ,            0                    ,         ''                ,         0           ,        ''    ,   " & (Val(vPcs_ChkrId)) & " , " & (Val(vPcs_FoldrId)) & " ) "
                                    cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
                                    nr = cmd.ExecuteNonQuery()

                                ElseIf nr > 1 Then
                                    Throw New ApplicationException("Invalid Piece Details Updation")
                                    Exit Sub

                                End If


                                Dim vTOT_RECMTRS As String = 0
                                Dim vTOT_TYP1MTRS As String = 0
                                Dim vTOT_TYP2MTRS As String = 0
                                Dim vTOT_TYP3MTRS As String = 0
                                Dim vTOT_TYP4MTRS As String = 0
                                Dim vTOT_TYP5MTRS As String = 0
                                Dim vTOT_CHKMTRS As String = 0
                                Dim vCONSYARN As String = 0
                                Dim vCONSPAVU As String = 0
                                Dim vTOT_WGT As String = 0


                                da3 = New SqlClient.SqlDataAdapter("select sum(a.Receipt_Meters) as RECMTRS, sum(a.Type1_Meters) as TY1MTRS, sum(a.Type2_Meters) as TY2MTRS, sum(a.Type3_Meters) as TY3MTRS, sum(a.Type4_Meters) as TY4MTRS, sum(a.Type5_Meters) as TY5MTRS, SUM(A.Total_Checking_Meters) as TOTCHKMTRS, SUM(A.weight) as TOTWGT  from  Weaver_ClothReceipt_Piece_Details a Where a.Weaver_ClothReceipt_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value) & "' and a.Lot_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) & "'", con)
                                da3.SelectCommand.Transaction = tr
                                dt3 = New DataTable
                                da3.Fill(dt3)
                                If dt3.Rows.Count > 0 Then
                                    vTOT_RECMTRS = Val(dt3.Rows(0).Item("RECMTRS").ToString)
                                    vTOT_TYP1MTRS = Val(dt3.Rows(0).Item("TY1MTRS").ToString)
                                    vTOT_TYP2MTRS = Val(dt3.Rows(0).Item("TY2MTRS").ToString)
                                    vTOT_TYP3MTRS = Val(dt3.Rows(0).Item("TY3MTRS").ToString)
                                    vTOT_TYP4MTRS = Val(dt3.Rows(0).Item("TY4MTRS").ToString)
                                    vTOT_TYP5MTRS = Val(dt3.Rows(0).Item("TY5MTRS").ToString)
                                    vTOT_CHKMTRS = Val(dt3.Rows(0).Item("TOTCHKMTRS").ToString)
                                    vTOT_WGT = Val(dt3.Rows(0).Item("TOTWGT").ToString)

                                End If
                                dt3.Clear()


                                If Trim(UCase(vLOOMTYP)) = "AUTO LOOM" Or Trim(UCase(vLOOMTYP)) = "AUTOLOOM" Then '---- Lakshmi Saraswathi Textiles (Thiruchengodu)
                                    vCONSYARN = Val(Common_Procedures.get_Weft_ConsumedYarn(con, vCLTH_IDNO, Val(vTOT_CHKMTRS), tr))
                                    vCONSPAVU = Val(Common_Procedures.get_Pavu_Consumption(con, vCLTH_IDNO, vLM_ID, Val(vTOT_CHKMTRS), Trim(vWdth_Typ), tr))

                                Else
                                    vCONSYARN = Val(Common_Procedures.get_Weft_ConsumedYarn(con, vCLTH_IDNO, Val(vTOT_CHKMTRS), tr))
                                    vCONSPAVU = Val(Common_Procedures.get_Pavu_Consumption(con, vCLTH_IDNO, vLM_ID, Val(vTOT_RECMTRS), Trim(vWdth_Typ), tr))

                                End If

                                Dim vFOLDPREC As String = 0

                                Dim vTot_100Fld_Typ1Mtrs As String = 0
                                Dim vTot_100Fld_Typ2Mtrs As String = 0
                                Dim vTot_100Fld_Typ3Mtrs As String = 0
                                Dim vTot_100Fld_Typ4Mtrs As String = 0
                                Dim vTot_100Fld_Typ5Mtrs As String = 0

                                Dim vTot_100Fld_ChkMtr As String = 0
                                Dim vTOTEXCSHTMTR As String = 0

                                vFOLDPREC = 100
                                If Val(.Rows(i).Cells(dgvCol_PieceDetails.FOLDING).Value) <> 0 Then vFOLDPREC = Val(.Rows(i).Cells(dgvCol_PieceDetails.FOLDING).Value)

                                vTot_100Fld_Typ1Mtrs = Format(Val(vTOT_TYP1MTRS) * Val(vFOLDPREC) / 100, "########0.00")
                                vTot_100Fld_Typ2Mtrs = Format(Val(vTOT_TYP2MTRS) * Val(vFOLDPREC) / 100, "########0.00")
                                vTot_100Fld_Typ3Mtrs = Format(Val(vTOT_TYP3MTRS) * Val(vFOLDPREC) / 100, "########0.00")
                                vTot_100Fld_Typ4Mtrs = Format(Val(vTOT_TYP4MTRS) * Val(vFOLDPREC) / 100, "########0.00")
                                vTot_100Fld_Typ5Mtrs = Format(Val(vTOT_TYP5MTRS) * Val(vFOLDPREC) / 100, "########0.00")
                                vTot_100Fld_ChkMtr = Format(Val(vTOT_CHKMTRS) * Val(vFOLDPREC) / 100, "########0.00")

                                vTOTEXCSHTMTR = Format(Val(vTOT_CHKMTRS) - Val(vTOT_RECMTRS), "#########0.00")

                                cmd.CommandText = "Update Weaver_Piece_Checking_Head set Total_Checking_Receipt_Meters =  " & Str(Val(vTOT_RECMTRS)) & ", Total_Type1_Meters = " & Str(Val(vTOT_TYP1MTRS)) & ",  Total_Type2_Meters = " & Str(Val(vTOT_TYP2MTRS)) & ", Total_Type3_Meters = " & Str(Val(vTOT_TYP3MTRS)) & ", Total_Type4_Meters = " & Str(Val(vTOT_TYP4MTRS)) & ", Total_Type5_Meters = " & Str(Val(vTOT_TYP5MTRS)) & ", Total_Checking_Meters = " & Str(Val(vTOT_CHKMTRS)) & ", Total_Weight = " & Str(Val(vTOT_WGT)) & ", Total_Type1Meters_100Folding = " & Str(Val(vTot_100Fld_Typ1Mtrs)) & ", Total_Type2Meters_100Folding = " & Str(Val(vTot_100Fld_Typ2Mtrs)) & ", Total_Type3Meters_100Folding = " & Str(Val(vTot_100Fld_Typ3Mtrs)) & ", Total_Type4Meters_100Folding = " & Str(Val(vTot_100Fld_Typ4Mtrs)) & ", Total_Type5Meters_100Folding = " & Str(Val(vTot_100Fld_Typ5Mtrs)) & ", Total_Meters_100Folding  =  " & Str(Val(vTot_100Fld_ChkMtr)) & ", Excess_Short_Meter = " & Str(Val(vTOTEXCSHTMTR)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And Weaver_Piece_Checking_Code = '" & Trim(vPCSCHKCODE) & "'"
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '" & Trim(vPCSCHKCODE) & "', Weaver_Piece_Checking_Increment = 1, Weaver_Piece_Checking_Date = @CheckingDate, Folding_Checking = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.FOLDING).Value)) & ", Folding = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.FOLDING).Value)) & ", ReceiptMeters_Checking = " & Str(Val(vTOT_RECMTRS)) & ", Receipt_Meters = " & Str(Val(vTOT_RECMTRS)) & ", ConsumedYarn_Checking = " & Str(Val(vCONSYARN)) & ", Consumed_Yarn = " & Str(Val(vCONSYARN)) & ", ConsumedPavu_Checking = " & Str(Val(vCONSPAVU)) & ", Consumed_Pavu = " & Str(Val(vCONSPAVU)) & ", Type1_Checking_Meters = " & Str(Val(vTOT_TYP1MTRS)) & ", Type2_Checking_Meters = " & Str(Val(vTOT_TYP2MTRS)) & ", Type3_Checking_Meters = " & Str(Val(vTOT_TYP3MTRS)) & ", Type4_Checking_Meters = " & Str(Val(vTOT_TYP4MTRS)) & ", Type5_Checking_Meters = " & Str(Val(vTOT_TYP5MTRS)) & ", Total_Checking_Meters = " & Str(Val(vTOT_CHKMTRS)) & " Where Weaver_ClothReceipt_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) & "'"
                                nr = cmd.ExecuteNonQuery()

                                cmd.CommandText = "Update Stock_Yarn_Processing_Details set reference_date = @CheckingDate,  Weight = " & Str(Val(vCONSYARN)) & " Where Reference_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value) & "'"
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "Update Stock_Pavu_Processing_Details set reference_date = @CheckingDate,  Meters = " & Str(Val(vCONSPAVU)) & " Where Reference_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value) & "'"
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = @CheckingDate,  Cloth_IdNo = " & Str(Val(vCLTH_IDNO)) & ", Folding = " & Str(Val(.Rows(i).Cells(dgvCol_PieceDetails.FOLDING).Value)) & ", UnChecked_Meters = 0, Meters_Type1 = " & Str(Val(vTOT_TYP1MTRS)) & ", Meters_Type2 = " & Str(Val(vTOT_TYP2MTRS)) & ", Meters_Type3 = " & Str(Val(vTOT_TYP3MTRS)) & ", Meters_Type4 = " & Str(Val(vTOT_TYP4MTRS)) & ", Meters_Type5 = " & Str(Val(vTOT_TYP5MTRS)) & " Where Reference_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value) & "'"
                                cmd.ExecuteNonQuery()

                                '////////////////////////////////////////////////////////////////////////////////////






                            End If

                        End If

                    Next i

                    Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_ClothReceipt_App_PieceChecking_Details", "Weaver_Piece_Approval_Code", Val(lbl_Company.Tag), NewCode, lbl_CheckingNo.Text, Val(vforOrdby), Pk_Condition, "", "", New_Entry, False, " Lot_No,  Checking_Date, Piece_No , Receipt_Meters , Type1_Meters , Type2_Meters , Type3_Meters ,  Type4_Meters , Type5_Meters , Total_Checking_Meters, Approved_Status , Total_Points ", "Piece_No", "Weaver_Piece_Approval_Code, For_OrderBy, Company_IdNo ", tr)

                End With


                'Else

                '    Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Piece_Approval_Head", "Weaver_Piece_Approval_Code", Val(lbl_Company.Tag), NewCode, lbl_CheckingNo.Text, Val(vforOrdby), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Piece_Approval_Code, Company_IdNo, for_OrderBy", tr)

                '    Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Weaver_ClothReceipt_App_PieceChecking_Details", "Weaver_Piece_Approval_Code", Val(lbl_Company.Tag), NewCode, lbl_CheckingNo.Text, Val(vforOrdby), Pk_Condition, "", "", New_Entry, False, " Lot_No,  Checking_Date, Piece_No , Receipt_Meters , Type1_Meters , Type2_Meters , Type3_Meters ,  Type4_Meters , Type5_Meters , Total_Checking_Meters, Approved_Status , Total_Points  ", "", "Weaver_Piece_Approval_Code, For_OrderBy, Company_IdNo", tr)

                '    Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Weaver_ClothReceipt_App_Piece_Defect_Details", "Weaver_ClothReceipt_Code", Val(lbl_Company.Tag), NewCode, lbl_CheckingNo.Text, Val(vforOrdby), Pk_Condition, "", "", New_Entry, False, " Piece_Checking_Defect_idno, Piece_Checking_Defect_Points , Noof_Times, Total_PieceChecking_Defect_Points , User_Idno ", "", "Weaver_ClothReceipt_Code, Piece_No, Company_IdNo,  Checking_Date ", tr)

            End If


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Weaver_Piece_Approval_Head", "Weaver_Piece_Approval_Code", Val(lbl_Company.Tag), NewCode, lbl_CheckingNo.Text, Val(vforOrdby), Pk_Condition, "", "", New_Entry, False, "", "Weaver_Piece_Approval_No", "Weaver_Piece_Approval_Code, Company_IdNo, for_OrderBy", tr)


            '----- Saving Cross Checking
            vERRMSG = ""
            '--***********************************************************COMMEMTED BY FOR-1267-BY-THANGES-TODAY-ONLY(12-09-2023)
            'If Trim(vFAB_LOTCODE) <> "" Then
            '    If Common_Procedures.Cross_Checking_PieceChecking_PackingSlip_Meters(con, vFAB_LOTCODE, vERRMSG, tr) = False Then
            '        Throw New ApplicationException(vERRMSG)
            '        Exit Sub
            '    End If
            'End If


            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "For SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            move_record(lbl_CheckingNo.Text)

        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Err.Description, "CK_Sales_Order_Details_1") > 0 Then
                MessageBox.Show("Invalid Invoice Quantity, Must be greater than zero", "For SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Err.Description, "CK_Sales_Order_Details_2") > 0 Then
                MessageBox.Show("Invalid Invoice Quantity, Invoice Quantity must be lesser than Order Quantity", "For SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "For SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally
            cmd.Dispose()
            tr.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub



    '***** GST END *****

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : e.SuppressKeyPress = True : txt_Narration.Focus() ' SendKeys.Send("+{TAB}")
    End Sub


    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
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

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Weaver_Piece_Approval_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = " a.Weaver_Piece_Approval_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Weaver_Piece_Approval_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ItemName.Text) <> "" Then
                Itm_IdNo = Common_Procedures.Item_NameToIdNo(con, cbo_Filter_ItemName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Itm_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Inward_Check_Code IN (select z.Inward_Check_Code from Receipt_Inward_Check_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.Weaver_Piece_Approval_No, a.Weaver_Piece_Approval_Date, a.Total_Qty, a.Net_Amount, b.Ledger_Name from Weaver_Piece_Approval_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Inward_Check_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Weaver_Piece_Approval_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaver_Piece_Approval_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Piece_Approval_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            dt2.Dispose()
            da.Dispose()

        End Try

        If dgv_Filter_Details.Rows.Count > 0 Then
            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()
        Else
            dtp_Filter_Fromdate.Focus()
        End If

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, btn_Filter_Show, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, Nothing, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        If dgv_Filter_Details.Rows.Count > 0 Then
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_Back.Enabled = True
                pnl_Filter.Visible = False
            End If

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



    Private Sub dgv_PieceDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PieceDetails.KeyUp
        On Error Resume Next
        Dim n As Integer


        'If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

        '    With dgv_PieceDetails

        '        n = .CurrentRow.Index
        '        .Rows.RemoveAt(n)

        '        For i = 0 To .Rows.Count - 1
        '            .Rows(n).Cells(0).Value = i + 1
        '        Next

        '    End With

        '    TotalAmount_Calculation()

        'End If

    End Sub


    Private Sub txt_Narration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Narration.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            dtp_Date.Focus()
        End If
        If e.KeyCode = 40 Then
            save_record()
        End If
    End Sub

    Private Sub txt_Narration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Narration.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub


    Private Sub TotalAmount_Calculation()
        Dim Sno As Integer = 0
        Dim vTOTPCS As Integer = 0
        Dim TotRecMtr As String = 0
        Dim TotSouMtr As String = 0
        Dim TotSecMtr As String = 0
        Dim TotBitsMtr As String = 0
        Dim TotRejMtr As String = 0
        Dim TotOtherMtr As String = 0
        Dim TotMtr As String = 0
        Dim Total_points As String = 0
        Dim TotChkMtrs As String = 0
        Dim TotDefect_Points As String = 0

        Dim vTotPts As String = 0

        Dim Slno As Integer = 0

        If FrmLdSTS = True Or NoCalc_Status = True Or MOV_Status = True Then Exit Sub

        Sno = 0 : Slno = 0
        vTOTPCS = 0
        TotRecMtr = 0 : TotSouMtr = 0 : TotSecMtr = 0 : TotBitsMtr = 0 : TotRejMtr = 0 : TotOtherMtr = 0 : TotMtr = 0
        TotChkMtrs = 0

        For i = 0 To dgv_PieceDetails.RowCount - 1

            Sno = Sno + 1

            dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.Slno).Value = Sno

            If Trim(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.LOT_NO).Value) <> "" Then

                vTOTPCS = vTOTPCS + 1
                TotRecMtr = TotRecMtr + Val(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.REC_MTRS).Value)

                TotSouMtr = TotSouMtr + Val(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.A_Sounds).Value)
                TotSecMtr = TotSecMtr + Val(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.B_Seconds).Value)
                TotBitsMtr = TotBitsMtr + Val(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.C_Bits).Value)
                TotRejMtr = TotRejMtr + Val(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.REJECT).Value)
                TotOtherMtr = TotOtherMtr + Val(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.Others).Value)
                TotMtr = TotMtr + Val(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.TOTAL_MTRS).Value)
                TotDefect_Points = TotDefect_Points + Val(dgv_PieceDetails.Rows(i).Cells(dgvCol_PieceDetails.TOTAL_POINTS).Value)
            End If

        Next


        With dgv_PieceDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(dgvCol_PieceDetails.PIECENO).Value = vTOTPCS
            .Rows(0).Cells(dgvCol_PieceDetails.REC_MTRS).Value = Format(Val(TotRecMtr), "###########0.00")
            .Rows(0).Cells(dgvCol_PieceDetails.A_Sounds).Value = Format(Val(TotSouMtr), "###########0.00")
            .Rows(0).Cells(dgvCol_PieceDetails.B_Seconds).Value = Format(Val(TotSecMtr), "###########0.00")
            .Rows(0).Cells(dgvCol_PieceDetails.C_Bits).Value = Format(Val(TotBitsMtr), "###########0.00")
            .Rows(0).Cells(dgvCol_PieceDetails.REJECT).Value = Format(Val(TotRejMtr), "###########0.00")
            .Rows(0).Cells(dgvCol_PieceDetails.Others).Value = Format(Val(TotOtherMtr), "###########0.00")
            .Rows(0).Cells(dgvCol_PieceDetails.TOTAL_MTRS).Value = Format(Val(TotMtr), "###########0.00")
            .Rows(0).Cells(dgvCol_PieceDetails.TOTAL_POINTS).Value = Format(Val(TotDefect_Points), "###########0")
        End With

        With dgv_defect_details
            For i = 0 To dgv_defect_details.RowCount - 1

                Slno = Slno + 1

                dgv_defect_details.Rows(i).Cells(0).Value = Slno

                Total_points = Total_points + Val(dgv_defect_details.Rows(i).Cells(2).Value)
                vTotPts = vTotPts + Val(dgv_defect_details.Rows(i).Cells(4).Value)

            Next
        End With


        With dgv_defect_Total
            If .RowCount = 0 Then .Rows.Add()
            '.Rows(0).Cells(2).Value = Format(Val(Total_points), "###########0.00")
            .Rows(0).Cells(4).Value = Format(Val(vTotPts), "###########0.00")

        End With



        Sno = 0
        vTOTPCS = 0
        TotChkMtrs = 0
        For i = 0 To dgv_LotDetails.RowCount - 1

            Sno = Sno + 1

            dgv_LotDetails.Rows(i).Cells(dgvCol_LotDetails.Slno).Value = Sno

            If Trim(dgv_LotDetails.Rows(i).Cells(dgvCol_LotDetails.LotNo).Value) <> "" Then
                vTOTPCS = vTOTPCS + Val(dgv_LotDetails.Rows(i).Cells(dgvCol_LotDetails.TotChkPcs).Value)
                TotChkMtrs = TotChkMtrs + Val(dgv_LotDetails.Rows(i).Cells(dgvCol_LotDetails.TotChk_Mtrs).Value)

            End If

        Next

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(dgvCol_LotDetails.TotChkPcs).Value = Val(vTOTPCS)
            .Rows(0).Cells(dgvCol_LotDetails.TotChk_Mtrs).Value = Format(Val(TotChkMtrs), "###########0.00")
        End With

    End Sub


    Private Sub dgv_PieceDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PieceDetails.EditingControlShowing
        dgtxt_Details = Nothing
        '  If dgv_Details.CurrentCell.ColumnIndex = 4 Then
        dgtxt_Details = CType(dgv_PieceDetails.EditingControl, DataGridViewTextBoxEditingControl)
        ' End If
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_PieceDetails.EditingControl.BackColor = Color.Lime
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_PieceDetails_KeyUp(sender, e)
        End If
    End Sub

    Private Sub dgtxt_details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged

        Try
            With dgv_PieceDetails

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




    Private Sub btn_LotDetails_Click(sender As Object, e As EventArgs) Handles btn_LotDetails.Click
        new_record()
    End Sub

    Private Sub get_Approval_Pending_LotDetails()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim n As Integer
        Dim SNo As Integer

        da1 = New SqlClient.SqlDataAdapter("Select a.Checking_Date, a.Year_Code, a.Weaver_ClothReceipt_Code, a.Lot_Code, a.Lot_No, Count(a.Piece_No) as Pcsno,  Sum(a.Total_Checking_Meters) as ChkMtrs  from Weaver_ClothReceipt_App_PieceChecking_Details a Where ISNULL(a.Approved_Status, 0) = 0 and ISNULL(a.Verified_Status, 0) <> 0 Group By a.Year_Code, a.Weaver_ClothReceipt_Code, a.Lot_Code, a.Lot_No, a.For_OrderBy, Checking_Date Having Sum(a.Total_Checking_Meters) <> 0 Order by a.Checking_Date Desc, a.For_OrderBy Desc, a.Lot_Code Desc, a.Lot_No Desc", con)
        dt1 = New DataTable
        da1.Fill(dt1)

        With dgv_LotDetails

            .Rows.Clear()

            SNo = 0

            If dt1.Rows.Count > 0 Then



                For i = 0 To dt1.Rows.Count - 1

                    n = dgv_LotDetails.Rows.Add()
                    SNo = SNo + 1
                    dgv_LotDetails.Rows(n).Cells(dgvCol_LotDetails.Slno).Value = Val(SNo)
                    dgv_LotDetails.Rows(n).Cells(dgvCol_LotDetails.Checking_Date).Value = Format(Convert.ToDateTime(dt1.Rows(i).Item("Checking_Date").ToString), "dd-MM-yyyy")
                    dgv_LotDetails.Rows(n).Cells(dgvCol_LotDetails.YearCode).Value = dt1.Rows(i).Item("Year_Code").ToString

                    dgv_LotDetails.Rows(n).Cells(dgvCol_LotDetails.LotNo).Value = dt1.Rows(i).Item("Lot_No").ToString
                    dgv_LotDetails.Rows(n).Cells(dgvCol_LotDetails.TotChkPcs).Value = dt1.Rows(i).Item("Pcsno").ToString
                    dgv_LotDetails.Rows(n).Cells(dgvCol_LotDetails.TotChk_Mtrs).Value = Format(Val(dt1.Rows(i).Item("ChkMtrs").ToString), "###########0.00")
                    dgv_LotDetails.Rows(n).Cells(dgvCol_LotDetails.STS).Value = ""
                    dgv_LotDetails.Rows(n).Cells(dgvCol_LotDetails.ClothReceipt_Code).Value = dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                    dgv_LotDetails.Rows(n).Cells(dgvCol_LotDetails.LotCode).Value = dt1.Rows(i).Item("Lot_Code").ToString

                Next

            End If
        End With

        dt1.Clear()

        Try

            If IsNothing(dgv_LotDetails.CurrentCell) Then Exit Sub
            dgv_LotDetails.CurrentCell.Selected = False

        Catch ex As Exception
            '-----

        End Try

    End Sub
    Private Sub dgv_LotDetails_DoubleClick(sender As Object, e As EventArgs) Handles dgv_LotDetails.DoubleClick

        If IsNothing(dgv_LotDetails.CurrentCell) Then Exit Sub
        Select_LotNo(dgv_LotDetails.CurrentRow.Index)
        get_Approval_Pending_PieceDetails(dgv_LotDetails.CurrentRow.Index, "")

    End Sub

    Private Sub btn_PieceDetails_Click(sender As Object, e As EventArgs) Handles btn_PieceDetails.Click
        Dim I As Integer
        For I = 0 To dgv_LotDetails.Rows.Count - 1
            If Val(dgv_LotDetails.Rows(I).Cells(dgvCol_LotDetails.STS).Value) = 1 Then
                get_Approval_Pending_PieceDetails(dgv_LotDetails.CurrentRow.Index, "")
                Exit For
            End If
        Next
    End Sub
    Private Sub get_Approval_Pending_PieceDetails(ByVal vCURRROW As Integer, ByVal NewCode As String)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim n As Integer
        Dim SNo As Integer
        Dim Slno As Integer
        Dim vLotCode As String = ""
        Dim vRecCode As String = ""
        Dim vCond As String = ""
        Dim All_STS As Boolean = False
        Dim vCLORECMTRS As String = 0

        vLotCode = ""
        vRecCode = ""



        If Trim(NewCode) = "" Then

            If Val(vCURRROW) >= 0 Then
                If Trim(dgv_LotDetails.Rows(vCURRROW).Cells(dgvCol_LotDetails.YearCode).Value) <> "" And Trim(dgv_LotDetails.Rows(vCURRROW).Cells(dgvCol_LotDetails.LotNo).Value) <> "" Then
                    vLotCode = Trim(dgv_LotDetails.Rows(vCURRROW).Cells(dgvCol_LotDetails.LotCode).Value)
                    vRecCode = Trim(dgv_LotDetails.Rows(vCURRROW).Cells(dgvCol_LotDetails.ClothReceipt_Code).Value)
                End If
            End If
            vCond = "a.Lot_Code = '" & Trim(vLotCode) & "' and a.Weaver_ClothReceipt_Code = '" & Trim(vRecCode) & "' and ISNULL(a.Approved_Status, 0) = 0 and a.Verified_Status <> 0 "

        Else
            vCond = "(a.Weaver_Piece_Approval_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')"

        End If


        da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Checking_Table_No, ISNULL(c.Receipt_Meters,0) as ClothReceipt_Meters from Weaver_ClothReceipt_App_PieceChecking_Details a LEFT OUTER JOIN Checking_TableNo_Head b ON a.Checking_Table_IdNo = b.Checking_Table_IdNo LEFT OUTER JOIN Weaver_ClothReceipt_Piece_Details c ON a.Lot_Code = c.Lot_Code and a.Weaver_ClothReceipt_Code = c.Weaver_ClothReceipt_Code and a.Piece_No = c.Piece_No Where " & vCond & " and a.Total_Checking_Meters <> 0 Order by a.PieceNo_OrderBy, a.Piece_No ", con)
        'da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Checking_Table_No, c.Receipt_Meters as ClothReceipt_Meters from Weaver_ClothReceipt_App_PieceChecking_Details a, Checking_TableNo_Head b, Weaver_ClothReceipt_Piece_Details c Where " & vCond & " and a.Checking_Table_IdNo = b.Checking_Table_IdNo and a.Lot_Code = c.Lot_Code and a.Weaver_ClothReceipt_Code = c.Weaver_ClothReceipt_Code and a.Piece_No = c.Piece_No Order by a.PieceNo_OrderBy, a.Piece_No ", con)
        ''da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Checking_Table_No from Weaver_ClothReceipt_App_PieceChecking_Details a, Checking_TableNo_Head b Where " & vCond & " and a.Checking_Table_IdNo = b.Checking_Table_IdNo  Order by a.PieceNo_OrderBy, a.Piece_No ", con)
        ''da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Checking_Table_No from Weaver_ClothReceipt_App_PieceChecking_Details a, Checking_TableNo_Head b Where a.Lot_Code = '" & Trim(vLotCode) & "' and a.Weaver_ClothReceipt_Code = '" & Trim(vRecCode) & "' and ISNULL(a.Approved_Status, 0) = 0 and a.Checking_Table_IdNo = b.Checking_Table_IdNo", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        With dgv_PieceDetails

            .Rows.Clear()
            dgv_DefectHidden_Details.Rows.Clear()

            SNo = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_PieceDetails.Rows.Add()
                    SNo = SNo + 1

                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.Slno).Value = Val(SNo)
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.YEARCODE).Value = dt2.Rows(i).Item("Year_Code").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.REC_PK).Value = dt2.Rows(i).Item("Receipt_PkCondition").ToString

                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.LOT_NO).Value = dt2.Rows(i).Item("Lot_No").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.CHK_Date).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Checking_Date").ToString), "dd-MM-yyyy").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.FOLDING).Value = dt2.Rows(i).Item("folding").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.TABLENO).Value = dt2.Rows(i).Item("Checking_Table_No").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.PIECENO).Value = dt2.Rows(i).Item("Piece_No").ToString



                    vCLORECMTRS = 0
                    If IsDBNull(dt2.Rows(i).Item("ClothReceipt_Meters").ToString) = False Then
                        If Val(dt2.Rows(i).Item("ClothReceipt_Meters").ToString) <> 0 Then
                            vCLORECMTRS = dt2.Rows(i).Item("ClothReceipt_Meters").ToString
                        End If
                    End If

                    If Val(vCLORECMTRS) <> 0 Then
                        dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.REC_MTRS).Value = Val(vCLORECMTRS)
                    Else
                        dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.REC_MTRS).Value = dt2.Rows(i).Item("Receipt_Meters").ToString
                    End If

                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.A_Sounds).Value = dt2.Rows(i).Item("Type1_Meters").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.B_Seconds).Value = dt2.Rows(i).Item("Type2_Meters").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.C_Bits).Value = dt2.Rows(i).Item("Type3_Meters").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.REJECT).Value = dt2.Rows(i).Item("Type4_Meters").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.Others).Value = dt2.Rows(i).Item("Type5_Meters").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.TOTAL_MTRS).Value = dt2.Rows(i).Item("Total_Checking_Meters").ToString

                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.WEIGHT).Value = dt2.Rows(i).Item("Weight").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.WGT_MTR).Value = dt2.Rows(i).Item("Weight_Meter").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.TOTAL_POINTS).Value = dt2.Rows(i).Item("Piece_Checking_Defect_Points").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value = dt2.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.LOTCODE).Value = dt2.Rows(i).Item("Lot_Code").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.PCS_MAINNO).Value = dt2.Rows(i).Item("Piece_MainNo").ToString
                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.PCS_SUBNO).Value = dt2.Rows(i).Item("Piece_SubNo").ToString

                    All_STS = False
                    If Val(dt2.Rows(i).Item("Approved_Status").ToString) = 1 Then All_STS = True

                    dgv_PieceDetails.Rows(n).Cells(dgvCol_PieceDetails.APP_STS).Value = All_STS

                    '------------------

                    da2 = New SqlClient.SqlDataAdapter("select  a.Piece_Checking_Defect_IdNo,a.Piece_Checking_Defect_Points, a.Total_PieceChecking_Defect_Points , a.Noof_Times , a.Weaver_ClothReceipt_Code , a.piece_no from Weaver_ClothReceipt_App_Piece_Defect_Details a INNER join Weaver_ClothReceipt_App_PieceChecking_Details b on b. Weaver_ClothReceipt_Code=a.Weaver_ClothReceipt_Code and b.piece_no=a.piece_no  where a.lot_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.LOTCODE).Value) & "' and  a.piece_no= '" & Trim(.Rows(i).Cells(dgvCol_PieceDetails.PIECENO).Value) & "' ", con)
                    dt3 = New DataTable
                    da2.Fill(dt3)

                    With dgv_DefectHidden_Details

                        Slno = 0

                        If dt3.Rows.Count > 0 Then

                            For j = 0 To dt3.Rows.Count - 1

                                n = dgv_DefectHidden_Details.Rows.Add()
                                Slno = Slno + 1

                                dgv_DefectHidden_Details.Rows(n).Cells(0).Value = dt3.Rows(j).Item("Weaver_ClothReceipt_Code").ToString
                                dgv_DefectHidden_Details.Rows(n).Cells(1).Value = dt3.Rows(j).Item("piece_no").ToString
                                dgv_DefectHidden_Details.Rows(n).Cells(2).Value = Common_Procedures.Defect_IdNoToName(con, dt3.Rows(j).Item("Piece_Checking_Defect_IdNo").ToString)
                                dgv_DefectHidden_Details.Rows(n).Cells(3).Value = dt3.Rows(j).Item("Piece_Checking_Defect_Points").ToString
                                dgv_DefectHidden_Details.Rows(n).Cells(4).Value = dt3.Rows(j).Item("Noof_Times").ToString
                                dgv_DefectHidden_Details.Rows(n).Cells(5).Value = dt3.Rows(j).Item("Total_PieceChecking_Defect_Points").ToString

                            Next

                        End If
                        dt3.Dispose()

                    End With

                    '---------------------

                Next


            End If

            For i = 0 To .Rows.Count - 1
                dgv_PieceDetails.Rows(i).Cells(0).Value = i + 1
            Next

            TotalAmount_Calculation()

        End With



        Try

            If IsNothing(dgv_PieceDetails.CurrentCell) Then Exit Sub
            dgv_PieceDetails.CurrentCell.Selected = False

            If dgv_PieceDetails.Rows.Count > 0 Then
                dgv_PieceDetails.Focus()
                dgv_PieceDetails.CurrentCell = dgv_PieceDetails.Rows(0).Cells(0)
            End If


        Catch ex As Exception
            '-----

        End Try

    End Sub
    Private Sub dgv_LotDetails_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_LotDetails.CellClick
        Select_LotNo(e.RowIndex)
    End Sub
    Private Sub Select_LotNo(ByVal RwIndx As Integer)
        Dim i As Integer
        Dim j As Integer

        With dgv_LotDetails

            If .RowCount > 0 And RwIndx >= 0 Then

                For i = 0 To .Rows.Count - 1
                    If Val(.Rows(i).Cells(dgvCol_LotDetails.STS).Value) = 1 Then
                        .Rows(i).Cells(dgvCol_LotDetails.STS).Value = ""
                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Black
                        Next
                    End If
                Next


                .Rows(RwIndx).Cells(dgvCol_LotDetails.STS).Value = 1

                For i = 0 To .ColumnCount - 1
                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                Next

            End If

        End With

    End Sub

    Private Sub dgv_LotDetails_KeyDown(sender As Object, e As KeyEventArgs) Handles dgv_LotDetails.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_LotDetails.CurrentCell.RowIndex >= 0 Then

                n = dgv_LotDetails.CurrentCell.RowIndex

                Select_LotNo(n)


                e.Handled = True

                If e.KeyCode = Keys.Enter Then
                    get_Approval_Pending_PieceDetails(n, "")
                End If

            End If
        End If
    End Sub

    Private Sub dgv_LotDetails_LostFocus(sender As Object, e As EventArgs) Handles dgv_LotDetails.LostFocus
        On Error Resume Next
        If IsNothing(dgv_LotDetails.CurrentCell) Then Exit Sub
        dgv_LotDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_PieceDetails_LostFocus(sender As Object, e As EventArgs) Handles dgv_PieceDetails.LostFocus
        On Error Resume Next
        If IsNothing(dgv_PieceDetails.CurrentCell) Then Exit Sub
        dgv_PieceDetails.CurrentCell.Selected = False
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Throw New NotImplementedException()
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Piece_Approval_Entry, New_Entry) = False Then Exit Sub
    End Sub



    Private Sub dgv_PieceDetails_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_PieceDetails.CellContentClick
        Dim n, sno As Integer
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim vClthRecCode As String = ""
        Dim vPcsNo As String = ""

        If FrmLdSTS = True Or NoCalc_Status = True Or MOV_Status = True Then Exit Sub

        If e.ColumnIndex = dgvCol_PieceDetails.DEFECT Then

            'n = dgv_defect_details.Rows.Add()
            'da2 = New SqlClient.SqlDataAdapter("select  a.Piece_Checking_Defect_IdNo,a.Piece_Checking_Defect_Points, a.Total_PieceChecking_Defect_Points , a.Noof_Times , a.Weaver_ClothReceipt_Code , a.piece_no from Weaver_ClothReceipt_App_Piece_Defect_Details a INNER join Weaver_ClothReceipt_App_PieceChecking_Details b on b. Weaver_ClothReceipt_Code=a.Weaver_ClothReceipt_Code and b.piece_no=a.piece_no  where a.lot_Code ='" & Trim(dgv_PieceDetails.Rows(e.RowIndex).Cells(dgvCol_PieceDetails.LOTCODE).Value) & "' and  a.piece_no='" & Trim(dgv_PieceDetails.Rows(e.RowIndex).Cells(dgvCol_PieceDetails.PIECENO).Value) & "'", con)
            'dt2 = New DataTable
            'da2.Fill(dt2)

            'With dgv_defect_details


            '    .Rows.Clear()

            '    sno = 0

            '    If dt2.Rows.Count > 0 Then

            '        For i = 0 To dt2.Rows.Count - 1

            '            n = dgv_defect_details.Rows.Add()
            '            sno = sno + 1
            '            dgv_defect_details.Rows(n).Cells(0).Value = Val(sno)
            '            dgv_defect_details.Rows(n).Cells(1).Value = Common_Procedures.Defect_IdNoToName(con, dt2.Rows(i).Item("Piece_Checking_Defect_IdNo").ToString)
            '            dgv_defect_details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Piece_Checking_Defect_Points").ToString
            '            dgv_defect_details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Noof_Times").ToString
            '            dgv_defect_details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Total_PieceChecking_Defect_Points").ToString

            '        Next

            '    Else



            vClthRecCode = Trim(dgv_PieceDetails.CurrentRow.Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value)
            vPcsNo = Trim(dgv_PieceDetails.CurrentRow.Cells(dgvCol_PieceDetails.PIECENO).Value)

            With dgv_defect_details

                sno = 0
                .Rows.Clear()

                For i = 0 To dgv_DefectHidden_Details.RowCount - 1
                    If vClthRecCode = Trim(dgv_DefectHidden_Details.Rows(i).Cells(0).Value) And vPcsNo = Trim(dgv_DefectHidden_Details.Rows(i).Cells(1).Value) Then

                        sno = sno + 1

                        n = .Rows.Add()
                        .Rows(n).Cells(0).Value = sno
                        .Rows(n).Cells(1).Value = Trim(dgv_DefectHidden_Details.Rows(i).Cells(2).Value)
                        .Rows(n).Cells(2).Value = Trim(dgv_DefectHidden_Details.Rows(i).Cells(3).Value)
                        .Rows(n).Cells(3).Value = Trim(dgv_DefectHidden_Details.Rows(i).Cells(4).Value)
                        .Rows(n).Cells(4).Value = Trim(dgv_DefectHidden_Details.Rows(i).Cells(5).Value)

                    End If
                Next

            End With


            'End If

            ' End With

            pnl_Back.Enabled = False
            pnl_defect_details.Visible = True

            pnl_defect_details.BringToFront()

            If dgv_defect_details.Rows.Count > 0 Then
                dgv_defect_details.Focus()
                dgv_defect_details.CurrentCell = dgv_defect_details.Rows(0).Cells(1)
                dgv_defect_details.CurrentCell.Selected = True
            End If


        End If

    End Sub

    Private Sub btn_BarCodePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BarCodePrint.Click
        vBARCDPRNT_PCSNO = ""
        vBARCDPRNT_COLNO = -1

        Common_Procedures.Print_OR_Preview_Status = 1
        Printing_BarCode_Sticker_Format4_DosPrint()

        'Common_Procedures.Print_OR_Preview_Status = 0
        'Printing_BarCode_Sticker()
    End Sub

    Private Sub Printing_BarCode_Sticker()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
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
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "CheckingReport"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\CheckingReport.pdf"
                    PrintDocument1.Print()

                Else
                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
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

        Print_PDF_Status = False

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
        Dim EntryCode As String
        Dim CurY As Single
        Dim CurX As Single
        Dim BrCdX As Single = 20
        Dim BrCdY As Single = 100
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String



        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.18", 325, 118)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        e.PageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
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

        NoofItems_PerPage = 2

        TxtHgt = 13.5

        EntryCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            'If prn_HdDt.Rows.Count > 0 Then

            NoofDets = 0

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                    Do While prn_DetBarCdStkr <= 5

                        vFldMtrs = 0
                        vBarCode = ""
                        If prn_DetBarCdStkr = 1 Then
                            vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString), "##########0.00")
                            vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type1").ToString)
                        ElseIf prn_DetBarCdStkr = 2 Then
                            vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString), "##########0.00")
                            vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type2").ToString)
                        ElseIf prn_DetBarCdStkr = 3 Then
                            vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString), "##########0.00")
                            vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type3").ToString)
                        ElseIf prn_DetBarCdStkr = 4 Then
                            vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString), "##########0.00")
                            vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type4").ToString)
                        ElseIf prn_DetBarCdStkr = 5 Then
                            vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString), "##########0.00")
                            vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type5").ToString)
                        End If

                        If Val(vFldMtrs) <> 0 Then

                            If NoofDets >= NoofItems_PerPage Then
                                e.HasMorePages = True
                                Return
                            End If

                            CurY = TMargin

                            CurX = LMargin - 1
                            If NoofDets = 1 Then
                                CurX = CurX + ((PageWidth + RMargin) \ 2)
                            End If

                            'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                            'ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                            'Else
                            ItmNm1 = Trim(Common_Procedures.Cloth_IdNoToName(con, prn_DetDt.Rows(0).Item("Cloth_idno").ToString))
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
                            Common_Procedures.Print_To_PrintDocument(e, "Lot.NO: " & prn_DetDt.Rows(0).Item("lot_no").ToString & "      P.NO: " & prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)

                            'Common_Procedures.Print_To_PrintDocument(e, "LOT NO : " & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)
                            'CurY = CurY + TxtHgt
                            'Common_Procedures.Print_To_PrintDocument(e, "PCS NO : " & prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "METERS : " & vFldMtrs, CurX, CurY, 0, PrintWidth, pFont, , True)


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

                            pFont = New Font("Calibri", 14, FontStyle.Bold)
                            'CurY = CurY + TxtHgt + TxtHgt + 5
                            CurY = CurY + TxtHgt + TxtHgt - 6
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vBarCode), CurX + 5, CurY, 0, PrintWidth, pFont, , True)

                            NoofDets = NoofDets + 1

                        End If

                        prn_DetBarCdStkr = prn_DetBarCdStkr + 1

                    Loop

                    prn_DetBarCdStkr = 1
                    prn_DetIndx = prn_DetIndx + 1

                Loop

            End If

            'End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub PrintDocument1_BeginPrint(sender As Object, e As PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode, vPCSCHKCODE As String
        NewCode = Pk_Condition & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'vPCSCHKCODE = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(vPCSCHKNO) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetBarCdStkr = 1

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Piece_Approval_Head a  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Approval_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)



            'If prn_HdDt.Rows.Count > 0 Then

            da2 = New SqlClient.SqlDataAdapter("select a.* ,c.* from Weaver_ClothReceipt_App_PieceChecking_Details a LEft Outer Join Weaver_Piece_Approval_Head b on a.Weaver_Piece_Approval_Code=b.Weaver_Piece_Approval_Code Left outer join Weaver_ClothReceipt_Piece_Details c On c.Lot_Code=a.Lot_Code and c.Piece_No = a.piece_no where a.Weaver_Piece_Approval_Code = '" & Trim(NewCode) & "' ORDER BY a.PieceNo_OrderBy ASC", con)
            prn_DetDt = New DataTable
            da2.Fill(prn_DetDt)

            'Else
            'MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxBut.tons.OKCancel, MessageBoxIcon.Error)

            'End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_BarCode_Sticker_Format1(e)

    End Sub

    Private Sub dgv_defect_details_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_defect_details.CellValueChanged

        On Error Resume Next

        With dgv_defect_details
            If .Visible Then
                If IsNothing(dgv_defect_details.CurrentCell) Then Exit Sub

                If e.ColumnIndex = 1 Or e.ColumnIndex = 2 Or e.ColumnIndex = 3 Then

                    .Rows(e.RowIndex).Cells(4).Value = Format(Val(.Rows(e.RowIndex).Cells(2).Value) * Val(.Rows(e.RowIndex).Cells(3).Value), "###########0.00")

                End If

            End If

            TotalAmount_Calculation()
        End With




    End Sub


    Private Sub dgv_PieceDetails_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_PieceDetails.CellValueChanged
        Dim vREQPTS_SOUND As String = 0
        Dim vREQPTS_REJECT As String = 0
        Dim vTotMtr As String = 0
        Dim vTotPts As String = 0

        Dim vSound As String = 0
        Dim vSec As String = 0
        Dim vRej As String = 0
        Dim vOthr As String = 0

        On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Or MOV_Status = True Then Exit Sub

        With dgv_PieceDetails
            If .Visible Then
                If IsNothing(dgv_PieceDetails.CurrentCell) Then Exit Sub

                If e.ColumnIndex = dgvCol_PieceDetails.A_Sounds Or e.ColumnIndex = dgvCol_PieceDetails.B_Seconds Or e.ColumnIndex = dgvCol_PieceDetails.C_Bits Or e.ColumnIndex = dgvCol_PieceDetails.REJECT Or e.ColumnIndex = dgvCol_PieceDetails.Others Or e.ColumnIndex = dgvCol_PieceDetails.TOTAL_MTRS Or e.ColumnIndex = dgvCol_PieceDetails.TOTAL_POINTS Then

                    '---- if points <=24 for 100mtrs then it is SOUND , so for 1 mtr 0.24 points
                    '---- if points <=48 for 100mtrs then it is SECONDS , so for 1 mtr 0.48 points
                    '---- if points >48 for 100mtrs then it is REJECT

                    vTotMtr = .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.TOTAL_MTRS).Value
                    vTotPTS = .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.TOTAL_POINTS).Value

                    vREQPTS_SOUND = Format(Val(vTotMtr) * 0.24, "##########0.00")
                    vREQPTS_REJECT = Format(Val(vTotMtr) * 0.48, "##########0.00")

                    If Val(vTotPts) <> 0 Or Val(vTotMtr) <> 0 Then

                        If Val(vTotMtr) < 30 And Val(vTotMtr) > 0 Then
                            .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.Others).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_PieceDetails.TOTAL_MTRS).Value), "##########0.00")
                            .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.A_Sounds).Value = 0
                            .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.B_Seconds).Value = 0
                            .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.REJECT).Value = 0

                        Else

                            If Val(vTotPts) > Val(vREQPTS_REJECT) Then
                                .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.REJECT).Value = Val(.Rows(e.RowIndex).Cells(dgvCol_PieceDetails.TOTAL_MTRS).Value)
                                .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.A_Sounds).Value = 0
                                .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.B_Seconds).Value = 0
                                .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.Others).Value = 0

                            Else

                                If Val(vTotPts) > Val(vREQPTS_SOUND) Then
                                    .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.B_Seconds).Value = Val(.Rows(e.RowIndex).Cells(dgvCol_PieceDetails.TOTAL_MTRS).Value)
                                    .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.A_Sounds).Value = 0
                                    .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.REJECT).Value = 0
                                    .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.Others).Value = 0

                                Else
                                    .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.A_Sounds).Value = Val(.Rows(e.RowIndex).Cells(dgvCol_PieceDetails.TOTAL_MTRS).Value)
                                    .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.B_Seconds).Value = 0
                                    .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.REJECT).Value = 0
                                    .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.Others).Value = 0

                                End If

                            End If

                        End If

                    Else
                        .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.A_Sounds).Value = 0
                        .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.B_Seconds).Value = 0
                        .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.REJECT).Value = 0
                        .Rows(e.RowIndex).Cells(dgvCol_PieceDetails.Others).Value = 0

                    End If
                End If

                TotalAmount_Calculation()

            End If

        End With
    End Sub

    Private Sub btn_Defect_Details_Close_Click(sender As Object, e As EventArgs) Handles btn_Defect_Details_Close.Click
        'pnl_Back.Enabled = True
        'pnl_defect_details.Visible = False

        'If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        'Put_PieceDefect_DetailsToHidden()

        DefectPoint_Close()

    End Sub

    Private Sub DefectPoint_Close()
        pnl_Back.Enabled = True
        pnl_defect_details.Visible = False

        If txt_Narration.Enabled = True And txt_Narration.Visible = True Then txt_Narration.Focus()

        Put_PieceDefect_DetailsToHidden()
        TotalAmount_Calculation()

    End Sub

    Private Sub Put_PieceDefect_DetailsToHidden()

        Dim I As Integer

        Dim vClthRecCode As String = ""
        Dim vPcsNo As String = ""

        Dim n As Integer
        Dim dgvDet_CurRow As Integer = 0
        Dim dgv_DetSlNo As Integer = 0

        vClthRecCode = Trim(dgv_PieceDetails.CurrentRow.Cells(dgvCol_PieceDetails.CLOTH_REC_CODE).Value)
        vPcsNo = Trim(dgv_PieceDetails.CurrentRow.Cells(dgvCol_PieceDetails.PIECENO).Value)

        With dgv_DefectHidden_Details

LOOP1:
            For I = 0 To .RowCount - 1

                If Trim(.Rows(I).Cells(0).Value) = Trim(vClthRecCode) And Trim(.Rows(I).Cells(1).Value) = Trim(vPcsNo) Then

                    If I = .Rows.Count - 1 Then
                        For J = 0 To .ColumnCount - 1
                            .Rows(I).Cells(J).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(I)

                    End If

                    GoTo LOOP1

                End If
            Next I

            For I = 0 To dgv_defect_details.RowCount - 1

                If Trim(dgv_defect_details.Rows(I).Cells(1).Value) <> "" Then
                    n = .Rows.Add()
                    .Rows(n).Cells(0).Value = Trim(vClthRecCode)
                    .Rows(n).Cells(1).Value = Trim(vPcsNo)
                    .Rows(n).Cells(2).Value = dgv_defect_details.Rows(I).Cells(1).Value
                    .Rows(n).Cells(3).Value = dgv_defect_details.Rows(I).Cells(2).Value
                    .Rows(n).Cells(4).Value = dgv_defect_details.Rows(I).Cells(3).Value
                    .Rows(n).Cells(5).Value = dgv_defect_details.Rows(I).Cells(4).Value

                End If
                With dgv_PieceDetails
                    .CurrentRow.Cells(dgvCol_PieceDetails.TOTAL_POINTS).Value = Val(dgv_defect_Total.Rows(0).Cells(4).Value)
                End With
            Next I



        End With

    End Sub

    Private Sub cbo_Grid_Defect_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_Defect.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Piece_Checking_Defect_head", "Piece_Checking_Defect_Name", "", "(Piece_Checking_Defect_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_Defect_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Defect.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Defect, Nothing, Nothing, "Piece_Checking_Defect_head", "Piece_Checking_Defect_Name", "", "(Piece_Checking_Defect_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_defect_details

            If (e.KeyValue = 38 And cbo_Grid_Defect.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Defect.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Defect_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_Defect.KeyPress

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim vPsChkDeft_Id As Integer = 0
        Dim vPschkPoints As String = 0


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Defect, Nothing, "Piece_Checking_Defect_head", "Piece_Checking_Defect_Name", "", "(Piece_Checking_Defect_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then


            vPsChkDeft_Id = Common_Procedures.Defect_NameToIdNo(con, Trim(cbo_Grid_Defect.Text))

            da = New SqlClient.SqlDataAdapter("select a.* from Piece_Checking_Defect_head a where a.Piece_Checking_Defect_IdNo = " & Str(Val(vPsChkDeft_Id)) & "  ", con)
            dt = New DataTable
            da.Fill(dt)
            vPschkPoints = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Piece_Checking_Defect_Points").ToString) = False Then
                    vPschkPoints = Val(dt.Rows(0).Item("Piece_Checking_Defect_Points").ToString)
                End If
            End If

            With dgv_defect_details
                e.Handled = True
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                .Rows(.CurrentRow.Index).Cells(2).Value = ""

                If Val(vPschkPoints) <> 0 Then
                    .Rows(.CurrentRow.Index).Cells(2).Value = vPschkPoints
                End If

            End With


            With dgv_defect_details
                e.Handled = True
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_Grid_Defect_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Defect.TextChanged
        Try
            If cbo_Grid_Defect.Visible Then
                If IsNothing(dgv_defect_details.CurrentCell) Then Exit Sub
                With dgv_defect_details
                    If Val(cbo_Grid_Defect.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Defect.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_defect_details_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_defect_details.CellEnter

        If FrmLdSTS = True Then Exit Sub

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle

        With dgv_defect_details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If



            If e.ColumnIndex = 1 Then


                If cbo_Grid_Defect.Visible = False Or Val(cbo_Grid_Defect.Tag) <> e.RowIndex Then

                    cbo_Grid_Defect.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Piece_Checking_Defect_Name from Piece_Checking_Defect_head order by Piece_Checking_Defect_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Defect.DataSource = Dt1
                    cbo_Grid_Defect.DisplayMember = "Piece_Checking_Defect_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Defect.Left = .Left + Rect.Left
                    cbo_Grid_Defect.Top = .Top + Rect.Top

                    cbo_Grid_Defect.Width = Rect.Width
                    cbo_Grid_Defect.Height = Rect.Height
                    cbo_Grid_Defect.Text = .CurrentCell.Value

                    cbo_Grid_Defect.Tag = Val(e.RowIndex)
                    cbo_Grid_Defect.Visible = True

                    cbo_Grid_Defect.BringToFront()
                    cbo_Grid_Defect.Focus()

                Else
                    'If cbo_Grid_Clothtype.Visible = True Then
                    '    cbo_Grid_Clothtype.BringToFront()
                    '    cbo_Grid_Clothtype.Focus()
                    'End If

                End If


            Else
                cbo_Grid_Defect.Visible = False

            End If

        End With

    End Sub

    Private Sub dgtxtDefect_Details_Enter(sender As Object, e As EventArgs) Handles dgtxtDefect_Details.Enter

        dgv_defect_details.EditingControl.BackColor = Color.Lime
        dgv_defect_details.EditingControl.ForeColor = Color.Blue

    End Sub

    Private Sub dgtxtDefect_Details_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dgtxtDefect_Details.KeyPress
        With dgv_defect_details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If

            End If
        End With
    End Sub

    Private Sub dgtxtDefect_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxtDefect_Details.TextChanged
        Try
            With dgv_defect_details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxtDefect_Details.Text)
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

    Private Sub dgv_defect_details_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgv_defect_details.EditingControlShowing
        dgtxtDefect_Details = Nothing
        If dgv_defect_details.CurrentCell.ColumnIndex >= 1 Then
            dgtxtDefect_Details = CType(dgv_defect_details.EditingControl, DataGridViewTextBoxEditingControl)
        End If
    End Sub

    Private Sub dgv_defect_details_LostFocus(sender As Object, e As EventArgs) Handles dgv_defect_details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_defect_details.CurrentCell) Then Exit Sub
        dgv_defect_details.CurrentCell.Selected = False

    End Sub


    Private Sub cbo_Grid_HideDefect_KeyPress(sender As Object, e As KeyPressEventArgs)

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim vPsChkDeft_Id As Integer = 0
        Dim vPschkPoints As String = 0

        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_HideDefect, Nothing, "Piece_Checking_Defect_head", "Piece_Checking_Defect_Name", "", "(Piece_Checking_Defect_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then

        '    vPsChkDeft_Id = Common_Procedures.Defect_NameToIdNo(con, Trim(cbo_Grid_HideDefect.Text))

        '    da = New SqlClient.SqlDataAdapter("select a.* from Piece_Checking_Defect_head a where a.Piece_Checking_Defect_IdNo = " & Str(Val(vPsChkDeft_Id)) & "  ", con)
        '    dt = New DataTable
        '    da.Fill(dt)

        '    If dt.Rows.Count > 0 Then
        '        If IsDBNull(dt.Rows(0).Item("Piece_Checking_Defect").ToString) = False Then
        '            vPschkPoints = Val(dt.Rows(0).Item("Piece_Checking_Defect").ToString)
        '        End If
        '    End If

        '    With dgv_DefectHidden_Details
        '        e.Handled = True
        '        .Focus()
        '        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

        '        If Val(vPschkPoints) <> 0 Then
        '            .Rows(.CurrentRow.Index).Cells(3).Value = vPschkPoints
        '        End If

        '    End With

        'End If
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As EventArgs) Handles btn_UserModification.Click

        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If

    End Sub

    Private Sub cbo_Grid_Defect_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Defect.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Piece_Checking_Defect_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Defect.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgv_defect_details_KeyUp(sender As Object, e As KeyEventArgs) Handles dgv_defect_details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_defect_details

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



        End If
    End Sub

    Private Sub btn_BarCodePrint_SinglePieces_Click(sender As Object, e As EventArgs) Handles btn_BarCodePrint_SinglePieces.Click
        vBARCDPRNT_PCSNO = ""
        vBARCDPRNT_COLNO = -1

        Try

            If IsNothing(dgv_PieceDetails.CurrentCell) Then
                MessageBox.Show("Invalid Piece No Selection" & vbCrLf & "Select a pieceno to Print", "DOES NOT PRINT BARCODE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            vBARCDPRNT_PCSNO = Trim(dgv_PieceDetails.CurrentRow.Cells(7).Value)
            vBARCDPRNT_COLNO = dgv_PieceDetails.CurrentCell.ColumnIndex

        Catch ex As Exception
            '-----
        End Try

        Common_Procedures.Print_OR_Preview_Status = 1
        Printing_BarCode_Sticker_Format4_DosPrint()

    End Sub


    Private Sub Printing_BarCode_Sticker_Format4_DosPrint()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim vPCSNO_CONDT As String
        Dim vBARCDPRNT_STS As Boolean = True

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        vPCSNO_CONDT = ""
        If Trim(vBARCDPRNT_PCSNO) <> "" Then
            vPCSNO_CONDT = " and (a.Piece_No = '" & Trim(vBARCDPRNT_PCSNO) & "')"
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

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Piece_Approval_Head a where a.Weaver_Piece_Approval_Code = '" & Trim(NewCode) & "' ", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("Select a.*, c.Checked_Pcs_Barcode_Type1, c.Checked_Pcs_Barcode_Type2, c.Checked_Pcs_Barcode_Type3, c.Checked_Pcs_Barcode_Type4, c.Checked_Pcs_Barcode_Type5, tQ.Cloth_Name, tQ.Cloth_Description from Weaver_ClothReceipt_App_PieceChecking_Details a INNER JOIN Weaver_ClothReceipt_Piece_Details c ON a.Lot_Code = c.Lot_Code and a.Weaver_ClothReceipt_Code = c.Weaver_ClothReceipt_Code and a.Piece_No = c.Piece_No INNER JOIN Cloth_Head tQ ON c.cloth_idno = tQ.cloth_idno Where a.Weaver_Piece_Approval_Code = '" & Trim(NewCode) & "' " & vPCSNO_CONDT & " and a.Total_Checking_Meters <> 0 Order by a.PieceNo_OrderBy, a.Piece_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


        NoofItems_PerPage = 1

        LnCnt = 0

        fs = New FileStream(Common_Procedures.Dos_Printing_FileName_Path, FileMode.Create)
        sw = New StreamWriter(fs, System.Text.Encoding.Default)


        Try

            If prn_HdDt.Rows.Count > 0 Then

                NoofDets = 0

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        Do While prn_DetBarCdStkr <= 5


                            vFldMtrs = 0
                            vBarCode = ""
                            vBARCDPRNT_STS = True

                            If prn_DetBarCdStkr = 1 Then

                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 9 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 10 And Val(vBARCDPRNT_COLNO) <> 11 And Val(vBARCDPRNT_COLNO) <> 12 And Val(vBARCDPRNT_COLNO) <> 13) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type1").ToString)
                                End If


                            ElseIf prn_DetBarCdStkr = 2 Then
                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 10 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 9 And Val(vBARCDPRNT_COLNO) <> 11 And Val(vBARCDPRNT_COLNO) <> 12 And Val(vBARCDPRNT_COLNO) <> 13) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type2").ToString)
                                End If


                            ElseIf prn_DetBarCdStkr = 3 Then
                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 11 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 9 And Val(vBARCDPRNT_COLNO) <> 10 And Val(vBARCDPRNT_COLNO) <> 12 And Val(vBARCDPRNT_COLNO) <> 13) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type3").ToString)
                                End If


                            ElseIf prn_DetBarCdStkr = 4 Then
                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 12 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 9 And Val(vBARCDPRNT_COLNO) <> 10 And Val(vBARCDPRNT_COLNO) <> 11 And Val(vBARCDPRNT_COLNO) <> 13) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type4").ToString)
                                End If


                            ElseIf prn_DetBarCdStkr = 5 Then
                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 13 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 9 And Val(vBARCDPRNT_COLNO) <> 10 And Val(vBARCDPRNT_COLNO) <> 11 And Val(vBARCDPRNT_COLNO) <> 12) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type5").ToString)
                                End If


                            End If


                            If Val(vFldMtrs) <> 0 Then

                                If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                                    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                                Else
                                    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                                End If

                                ItmNm2 = ""
                                If Len(ItmNm1) > 21 Then
                                    For I = 21 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 21

                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I + 1)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                ItmNm1 = Replace(ItmNm1, """", """""")
                                ItmNm2 = Replace(ItmNm2, """", """""")

                                PrnTxt = "I8,1,001"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "ZN"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "q580"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "S30"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "O"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "*D5F"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "JF"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "H11"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "ZT"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "Q240,25"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "N"
                                sw.WriteLine(PrnTxt)


                                PrnTxt = "A556,227,2,2,2,2,N,""" & Trim(ItmNm1) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A556,185,2,2,2,2,N,""" & Trim(ItmNm2) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A567,145,2,2,2,2,N,""L.NO:"""
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A567,99,2,2,2,2,N,""P.NO:"""
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A453,139,2,2,2,2,N,""" & prn_DetDt.Rows(prn_DetIndx).Item("Lot_No").ToString & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A447,99,2,2,2,2,N,""" & prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A564,46,2,2,2,2,N,""MTRS:"""
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A453,46,2,2,2,2,N,""" & Trim(Val(vFldMtrs)) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "B329,150,2,1,2,4,73,N,""" & Trim(UCase(vBarCode)) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A326,71,2,1,2,2,N,""" & Trim(UCase(vBarCode)) & """"
                                sw.WriteLine(PrnTxt)

                                PrnTxt = "W1"
                                sw.WriteLine(PrnTxt)

                                NoofDets = NoofDets + 1

                            End If

                            prn_DetBarCdStkr = prn_DetBarCdStkr + 1

                        Loop

                        prn_DetBarCdStkr = 1
                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

            End If

            sw.Close()
            fs.Close()
            sw.Dispose()
            fs.Dispose()

            If Val(Common_Procedures.Print_OR_Preview_Status) = 2 Then
                Dim p1 As New System.Diagnostics.Process
                p1.EnableRaisingEvents = False
                p1.StartInfo.FileName = Common_Procedures.Dos_PrintPreView_BatchFileName_Path
                p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized
                p1.Start()

            Else
                Dim p2 As New System.Diagnostics.Process
                p2.EnableRaisingEvents = False
                p2.StartInfo.FileName = Common_Procedures.Dos_Print_BatchFileName_Path
                p2.StartInfo.CreateNoWindow = True
                p2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                p2.Start()

            End If

            MessageBox.Show("BarCode Sticker Printed", "FOR BARCODE STICKER PRINTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


        Finally

            Try
                sw.Close()
                fs.Close()
                sw.Dispose()
                fs.Dispose()
            Catch ex As Exception
                '-----

            End Try

        End Try

    End Sub

    Private Sub btn_PCS_VERIFICATION_PENDING_IN_APP_Click(sender As Object, e As EventArgs) Handles btn_PCS_VERIFICATION_PENDING_IN_APP.Click
        Dim CMD As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim m As Integer, n As Integer
        Dim SNo As Integer
        Dim SlNo As Integer
        Dim vCLORECMTRS As String = 0
        Dim vDEFDET As String = 0

        CMD.Connection = con

        With dgv_PIECE_VERIFICATION_PENDING_DETAILS

            dgv_PIECE_VERIFICATION_PENDING_DETAILS.Rows.Clear()
            dgv_PIECE_VERIFICATION_PENDING_DefectHidden_Details.Rows.Clear()

            SNo = 0

            'da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Checking_Table_No, ISNULL(c.Receipt_Meters,0) as ClothReceipt_Meters from Weaver_ClothReceipt_App_PieceChecking_Details a LEFT OUTER JOIN Checking_TableNo_Head b ON a.Checking_Table_IdNo = b.Checking_Table_IdNo LEFT OUTER JOIN Weaver_ClothReceipt_Piece_Details c ON a.Lot_Code = c.Lot_Code and a.Weaver_ClothReceipt_Code = c.Weaver_ClothReceipt_Code and a.Piece_No = c.Piece_No Where " & vCond & " and a.Total_Checking_Meters <> 0 Order by a.PieceNo_OrderBy, a.Piece_No ", con)

            da1 = New SqlClient.SqlDataAdapter("Select a.*, tU.user_name, tCTH.Checking_Table_No, ISNULL(c.Receipt_Meters,0) as ClothReceipt_Meters from Weaver_ClothReceipt_App_PieceChecking_Details a LEFT OUTER JOIN appuser_head tU ON a.user_idno = tU.user_idno LEFT OUTER JOIN Checking_TableNo_Head tCTH ON a.Checking_Table_IdNo = tCTH.Checking_Table_IdNo LEFT OUTER JOIN Weaver_ClothReceipt_Piece_Details c ON a.Lot_Code = c.Lot_Code and a.Weaver_ClothReceipt_Code = c.Weaver_ClothReceipt_Code and a.Piece_No = c.Piece_No Where ISNULL(a.Approved_Status, 0) = 0 and ISNULL(a.Verified_Status, 0) = 0 and a.Total_Checking_Meters <> 0 Order by a.Checking_Date Desc, a.Checking_DateTime Desc, a.For_OrderBy, a.Lot_Code, a.Lot_No, a.PieceNo_OrderBy, a.Piece_No", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                For i = 0 To dt1.Rows.Count - 1

                    SNo = SNo + 1

                    n = .Rows.Add()

                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.Slno).Value = Val(SNo)
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.USERNAME).Value = dt1.Rows(i).Item("user_name").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.TABLENO).Value = dt1.Rows(i).Item("Checking_Table_No").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.YEARCODE).Value = dt1.Rows(i).Item("Year_Code").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.REC_PKCONDITION).Value = dt1.Rows(i).Item("Receipt_PkCondition").ToString

                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.LOT_NO).Value = dt1.Rows(i).Item("Lot_No").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.CHK_Date).Value = Format(Convert.ToDateTime(dt1.Rows(i).Item("Checking_Date").ToString), "dd-MM-yyyy").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.CHK_Time).Value = Format(Convert.ToDateTime(dt1.Rows(i).Item("Checking_DateTime").ToString), "hh:mm am/pm").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.FOLDING).Value = dt1.Rows(i).Item("folding").ToString

                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.PIECENO).Value = dt1.Rows(i).Item("Piece_No").ToString

                    vCLORECMTRS = 0
                    If IsDBNull(dt1.Rows(i).Item("ClothReceipt_Meters").ToString) = False Then
                        If Val(dt1.Rows(i).Item("ClothReceipt_Meters").ToString) <> 0 Then
                            vCLORECMTRS = Format(Val(dt1.Rows(i).Item("ClothReceipt_Meters").ToString), "##########0.00")
                        End If
                    End If

                    If Val(vCLORECMTRS) <> 0 Then
                        .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.REC_MTRS).Value = Val(vCLORECMTRS)
                    Else
                        .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.REC_MTRS).Value = dt1.Rows(i).Item("Receipt_Meters").ToString
                    End If

                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.A_Sounds).Value = dt1.Rows(i).Item("Type1_Meters").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.B_Seconds).Value = dt1.Rows(i).Item("Type2_Meters").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.C_Bits).Value = dt1.Rows(i).Item("Type3_Meters").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.REJECT).Value = dt1.Rows(i).Item("Type4_Meters").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.Others).Value = dt1.Rows(i).Item("Type5_Meters").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.TOTAL_MTRS).Value = dt1.Rows(i).Item("Total_Checking_Meters").ToString

                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.WEIGHT).Value = dt1.Rows(i).Item("Weight").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.WGT_MTR).Value = dt1.Rows(i).Item("Weight_Meter").ToString

                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.TOTAL_POINTS).Value = dt1.Rows(i).Item("Piece_Checking_Defect_Points").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.DEFECTDETAILS).Value = ""
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.CLOTH_REC_CODE).Value = dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.LOTCODE).Value = dt1.Rows(i).Item("Lot_Code").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.PCS_MAINNO).Value = dt1.Rows(i).Item("Piece_MainNo").ToString
                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.PCS_SUBNO).Value = dt1.Rows(i).Item("Piece_SubNo").ToString

                    '------------------GETTING DEFECT DETAILS INTO A STRING VARIABLE

                    CMD.CommandText = "truncate table entrytempsub"
                    CMD.ExecuteNonQuery()

                    CMD.CommandText = "insert into entrytempsub(name1, name2, name3, Int1, Int2) select a.LotCode_Selection , a.piece_no, (CASE WHEN b.Piece_Checking_Defect_shortname <> '' THEN b.Piece_Checking_Defect_shortname ELSE b.Piece_Checking_Defect_Name END) as defectname, a.Noof_Times, a.Total_PieceChecking_Defect_Points  from Weaver_ClothReceipt_App_Piece_Defect_Details a, Piece_Checking_Defect_head b where a.Weaver_ClothReceipt_Code = '" & Trim(dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString) & "' and a.Piece_No = '" & Trim(dt1.Rows(i).Item("Piece_No").ToString) & "' and a.Piece_Checking_Defect_IdNo = b.Piece_Checking_Defect_IdNo and a.Total_PieceChecking_Defect_Points <> 0 and a.Noof_Times <> 0 "
                    CMD.ExecuteNonQuery()

                    CMD.CommandText = "truncate table entrytemp"
                    CMD.ExecuteNonQuery()

                    CMD.CommandText = "insert into entrytemp(name1, name2, name3, Int1) select name1, name2, name3 + '(' + cast(sum(Int1) as varchar) + ')', sum(Int2) from entrytempsub group by name1, name2, name3 having sum(Int1) <> 0"
                    CMD.ExecuteNonQuery()


                    CMD.CommandText = "truncate table entrytempsub"
                    CMD.ExecuteNonQuery()

                    CMD.CommandText = "insert into entrytempsub(name1, name2, name3, Int1) select name1, name2, STUFF((SELECT ', ' + convert(varchar(10), name3, 120) FROM entrytemp b where a.Name1 = b.Name1 and a.Name2 = b.Name2 FOR XML PATH ('')) , 1, 1, '')  AS name333, sum(int1) from entrytemp a group by name1, name2, name3"
                    CMD.ExecuteNonQuery()

                    CMD.CommandText = "truncate table entrytemp"
                    CMD.ExecuteNonQuery()

                    CMD.CommandText = "insert into entrytemp(name1, name2, name3, Int1) select name1, name2, name3 , sum(int1) from entrytempsub group by name1, name2, name3 having sum(Int1) <> 0"
                    CMD.ExecuteNonQuery()


                    vDEFDET = ""
                    da1 = New SqlClient.SqlDataAdapter("select  name1, name2, name3, Int1 from entrytemp order by name3", con)
                    dt3 = New DataTable
                    da1.Fill(dt3)

                    If dt3.Rows.Count > 0 Then
                        For j = 0 To dt3.Rows.Count - 1
                            vDEFDET = Trim(vDEFDET) & IIf(Trim(vDEFDET) <> "", ", ", "") & dt3.Rows(j).Item("name3").ToString
                        Next
                    End If

                    dt3.Dispose()

                    .Rows(n).Cells(dgvCol_PieceVerificationPendingDetails.DEFECTDETAILS).Value = Trim(vDEFDET)


                    ''------------------MOVING DEFECT DETAILS IN HIDDEN GRID

                    'vDEFDET = ""

                    'da1 = New SqlClient.SqlDataAdapter("select  a.Piece_Checking_Defect_IdNo, a.Piece_Checking_Defect_Points, a.Total_PieceChecking_Defect_Points, a.Noof_Times, a.Weaver_ClothReceipt_Code, a.piece_no, c.Piece_Checking_Defect_Name from Weaver_ClothReceipt_App_Piece_Defect_Details a INNER join Weaver_ClothReceipt_App_PieceChecking_Details b on b. Weaver_ClothReceipt_Code=a.Weaver_ClothReceipt_Code and b.piece_no=a.piece_no INNER JOIN Piece_Checking_Defect_head c on c.Piece_Checking_Defect_IdNo = a.Piece_Checking_Defect_IdNo where a.lot_Code = '" & Trim(.Rows(i).Cells(dgvCol_PieceVerificationPendingDetails.LOTCODE).Value) & "' and  a.piece_no= '" & Trim(.Rows(i).Cells(dgvCol_PieceVerificationPendingDetails.PIECENO).Value) & "' and a.Total_PieceChecking_Defect_Points <> 0 and a.Noof_Times <> 0", con)
                    'dt3 = New DataTable
                    'da1.Fill(dt3)

                    'With dgv_PIECE_VERIFICATION_PENDING_DefectHidden_Details
                    '    SlNo = 0
                    '    If dt3.Rows.Count > 0 Then
                    '        For j = 0 To dt3.Rows.Count - 1
                    '            m = dgv_PIECE_VERIFICATION_PENDING_DefectHidden_Details.Rows.Add()
                    '            SlNo = SlNo + 1
                    '            dgv_PIECE_VERIFICATION_PENDING_DefectHidden_Details.Rows(m).Cells(0).Value = dt3.Rows(j).Item("Weaver_ClothReceipt_Code").ToString
                    '            dgv_PIECE_VERIFICATION_PENDING_DefectHidden_Details.Rows(m).Cells(1).Value = dt3.Rows(j).Item("piece_no").ToString
                    '            dgv_PIECE_VERIFICATION_PENDING_DefectHidden_Details.Rows(m).Cells(2).Value = dt3.Rows(j).Item("Piece_Checking_Defect_Name").ToString ' Common_Procedures.Defect_IdNoToName(con, dt3.Rows(j).Item("Piece_Checking_Defect_IdNo").ToString)
                    '            dgv_PIECE_VERIFICATION_PENDING_DefectHidden_Details.Rows(m).Cells(3).Value = dt3.Rows(j).Item("Piece_Checking_Defect_Points").ToString
                    '            dgv_PIECE_VERIFICATION_PENDING_DefectHidden_Details.Rows(m).Cells(4).Value = dt3.Rows(j).Item("Noof_Times").ToString
                    '            dgv_PIECE_VERIFICATION_PENDING_DefectHidden_Details.Rows(m).Cells(5).Value = dt3.Rows(j).Item("Total_PieceChecking_Defect_Points").ToString
                    '            vDEFDET = Trim(vDEFDET) & ""
                    '        Next

                    '    End If
                    '    dt3.Dispose()
                    'End With

                Next i

            End If

        End With


        dt1.Clear()

        pnl_PIECE_VERIFICATION_PENDING_DETAILS.Visible = True
        pnl_Back.Enabled = False
        If dgv_PIECE_VERIFICATION_PENDING_DETAILS.Rows.Count > 0 Then
            dgv_PIECE_VERIFICATION_PENDING_DETAILS.CurrentCell = dgv_PIECE_VERIFICATION_PENDING_DETAILS.Rows(0).Cells(0)
            dgv_PIECE_VERIFICATION_PENDING_DETAILS.Focus()
        End If

        Try

            If IsNothing(dgv_PIECE_VERIFICATION_PENDING_DETAILS.CurrentCell) Then Exit Sub
            dgv_PIECE_VERIFICATION_PENDING_DETAILS.CurrentCell.Selected = False

        Catch ex As Exception
            '-----

        End Try
    End Sub

    Private Sub btn_Close_PIECE_VERIFICATION_PENDING_DETAILS_Click(sender As Object, e As EventArgs) Handles btn_Close_PIECE_VERIFICATION_PENDING_DETAILS.Click
        pnl_Back.Enabled = True
        pnl_PIECE_VERIFICATION_PENDING_DETAILS.Visible = False
    End Sub

    Private Sub btn_Close2_PIECE_VERIFICATION_PENDING_DETAILS_Click(sender As Object, e As EventArgs) Handles btn_Close2_PIECE_VERIFICATION_PENDING_DETAILS.Click
        btn_Close_PIECE_VERIFICATION_PENDING_DETAILS_Click(sender, e)
    End Sub

End Class
