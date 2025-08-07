Public Class Weaver_Piece_Checking_Simple
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Other_Condition As String = ""
    Private Pk_Condition As String = "PCCHK-"
    Private Pk_Condition2 As String = "WCLRC-"
    Private Pk_Condition3 As String = "CPREC-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

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
    Dim Type1, Type2, Type3, Type4, Type5 As String
    Dim Type11, Type22, Type33, Type44, Type55 As String
    Dim vType1, vType2, vType3, vType4, vType5 As Single
    Dim vTotType1, vTotType2, vTotType3, vTotType4, vTotType5 As Single


    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        lbl_ChkNo.Text = ""
        lbl_ChkNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_Weaver.Text = ""
        cbo_Quality.Text = ""

        txt_Excess_Short.Text = ""
        txt_Folding.Text = "96.50"
        txt_PDcNo.Text = ""
        txt_No_Pcs.Text = ""
        txt_Rec_Meter.Text = ""
        txt_RecNo.Text = ""
        lbl_lot.Text = Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Seconds_Meters.Text = ""
        txt_Seconds_Pieces.Text = ""
        txt_Sound_Meters.Text = ""
        txt_Sound_Piece.Text = ""
        txt_Other_Meters.Text = ""
        txt_Other_Pieces.Text = ""
        txt_Bits_Meter.Text = ""
        txt_Bits_Pieces.Text = ""
        txt_Reject_Meters.Text = ""
        txt_Reject_Pieces.Text = ""
        txt_Total_Meters.Text = ""
        txt_Total_Pieces.Text = ""

        cbo_Weaver.Enabled = True
        cbo_Weaver.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White

        btn_Selection.Enabled = True

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

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
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

    'Private Sub Grid_DeSelect()
    '    On Error Resume Next
    '    dgv_Details.CurrentCell.Selected = False
    '    dgv_Details_Total1.CurrentCell.Selected = False
    '    dgv_Details_Total2.CurrentCell.Selected = False
    'End Sub
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
        'dgv_Details.CurrentCell.Selected = False
        'dgv_Details_Total1.CurrentCell.Selected = False
        'dgv_Details_Total2.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
        'dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Weaver_Piece_Checking_Simple_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Weaver.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Weaver.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Quality.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Quality.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Weaver_Piece_Checking_Simple_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Weaver_Piece_Checking_Simple_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Weaver_Piece_Checking_Simple_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable

        lbl_lot.Text = StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase)
        'dgv_Details.Columns(5).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1))
        'dgv_Details.Columns(6).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2))
        'dgv_Details.Columns(7).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3))
        'dgv_Details.Columns(8).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4))
        'dgv_Details.Columns(9).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5))

        Me.Text = ""

        con.Open()

        Common_Procedures.get_CashPartyName_From_All_Entries(con)

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN') and Close_status = 0  order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Weaver.DataSource = dt1
        cbo_Weaver.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt2)
        cbo_Quality.DataSource = dt2
        cbo_Quality.DisplayMember = "Cloth_Name"


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaver.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Quality.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Excess_Short.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RecNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rec_Meter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_No_Pcs.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Reject_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Reject_Pieces.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Seconds_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Seconds_Pieces.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Sound_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Sound_Piece.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Other_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Other_Pieces.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bits_Meter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bits_Pieces.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weaver.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Quality.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Excess_Short.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RecNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rec_Meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_No_Pcs.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Reject_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Reject_Pieces.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Seconds_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Seconds_Pieces.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Sound_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Sound_Piece.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Other_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Other_Pieces.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bits_Meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bits_Pieces.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RecNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rec_Meter.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_No_Pcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Folding.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Reject_Meters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Reject_Pieces.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Seconds_Meters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Seconds_Pieces.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Sound_Meters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Sound_Piece.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Bits_Meter.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Bits_Pieces.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Other_Pieces.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Other_Meters.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RecNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rec_Meter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_No_Pcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Folding.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Bits_Meter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Bits_Pieces.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rec_Meter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Reject_Meters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Reject_Pieces.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Seconds_Meters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Seconds_Pieces.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Sound_Meters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Sound_Piece.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Other_Pieces.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Other_Meters.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0
        Other_Condition = "(Receipt_Type = '' or Receipt_Type = 'W')"

        Filter_Status = False
        FrmLdSTS = True
        new_record()
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False

        If Trim(no) = "" Then Exit Sub
        'If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Piece_Checking_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and " & Other_Condition, con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_ChkNo.Text = dt1.Rows(0).Item("Weaver_Piece_Checking_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_Piece_Checking_Date").ToString

                cbo_Weaver.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Quality.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))

                dtp_Rec_Date.Text = dt1.Rows(0).Item("Piece_Receipt_Date").ToString
                txt_Excess_Short.Text = Format(Val(dt1.Rows(0).Item("Excess_Short_Meter").ToString), "#######0.00")
                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "#######0.00")
                txt_PDcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                txt_Rec_Meter.Text = Format(Val(dt1.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00")
                lbl_RecPkCondition.Text = dt1.Rows(0).Item("Receipt_PkCondition").ToString
                lbl_RecCode.Text = dt1.Rows(0).Item("Piece_Receipt_Code").ToString
                txt_RecNo.Text = dt1.Rows(0).Item("Piece_Receipt_No").ToString
                txt_No_Pcs.Text = Format(Val(dt1.Rows(0).Item("noof_pcs").ToString), "#######0")

                txt_Reject_Meters.Text = Format(Val(dt1.Rows(0).Item("Reject_Meters").ToString), "#########0.00")
                txt_Reject_Pieces.Text = Format(Val(dt1.Rows(0).Item("Reject_Piece").ToString), "#########0")
                txt_Seconds_Pieces.Text = Format(Val(dt1.Rows(0).Item("Seconds_Piece").ToString), "#########0")
                txt_Seconds_Meters.Text = Format(Val(dt1.Rows(0).Item("Seconds_Meters").ToString), "#########0.00")
                txt_Sound_Piece.Text = Format(Val(dt1.Rows(0).Item("Sound_Piece").ToString), "#########0")
                txt_Sound_Meters.Text = Format(Val(dt1.Rows(0).Item("Sound_Meters").ToString), "###########0.00")
                txt_Bits_Pieces.Text = Format(Val(dt1.Rows(0).Item("Bits_Piece").ToString), "#########0")
                txt_Bits_Meter.Text = Format(Val(dt1.Rows(0).Item("Bits_Meters").ToString), "##########0.00")
                txt_Other_Pieces.Text = Format(Val(dt1.Rows(0).Item("Others_Piece").ToString), "#########0")
                txt_Other_Meters.Text = Format(Val(dt1.Rows(0).Item("Others_Meters").ToString), "#############0.00")
                txt_Total_Meters.Text = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "###########0.00")
                txt_Total_Pieces.Text = Format(Val(dt1.Rows(0).Item("Total_Piece").ToString), "#########0")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                'da2 = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a Where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' Order by PieceNo_OrderBy, Sl_No, Piece_No", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                'With dgv_Details

                '    .Rows.Clear()
                '    SNo = 0

                '    If dt2.Rows.Count > 0 Then

                '        For i = 0 To dt2.Rows.Count - 1

                '            n = .Rows.Add()

                '            .Rows(n).Cells(0).Value = dt2.Rows(i).Item("Piece_No").ToString
                '            If Val(dt2.Rows(i).Item("ReceiptMeters_Checking").ToString) <> 0 Then
                '                .Rows(n).Cells(1).Value = Format(Val(dt2.Rows(i).Item("ReceiptMeters_Checking").ToString), "########0.00")
                '            End If
                '            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Loom_No").ToString
                '            If Val(dt2.Rows(i).Item("Pick").ToString) <> 0 Then
                '                .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Pick").ToString)
                '            End If
                '            If Val(dt2.Rows(i).Item("Width").ToString) <> 0 Then
                '                .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Width").ToString)
                '            End If
                '            If Val(dt2.Rows(i).Item("Type1_Meters").ToString) <> 0 Then
                '                .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                '            End If
                '            If Val(dt2.Rows(i).Item("Type2_Meters").ToString) <> 0 Then
                '                .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Type2_Meters").ToString), "########0.00")
                '            End If
                '            If Val(dt2.Rows(i).Item("Type3_Meters").ToString) <> 0 Then
                '                .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Type3_Meters").ToString), "########0.00")
                '            End If
                '            If Val(dt2.Rows(i).Item("Type4_Meters").ToString) <> 0 Then
                '                .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Type4_Meters").ToString), "########0.00")
                '            End If
                '            If Val(dt2.Rows(i).Item("Type5_Meters").ToString) <> 0 Then
                '                .Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Type5_Meters").ToString), "########0.00")
                '            End If

                '            .Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString), "########0.00")
                '            If Val(dt2.Rows(i).Item("Weight").ToString) <> 0 Then
                '                .Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                '            End If
                '            If Val(dt2.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                '                .Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")
                '            End If

                '            .Rows(n).Cells(13).Value = dt2.Rows(i).Item("PackingSlip_Code_Type1").ToString
                '            .Rows(n).Cells(14).Value = dt2.Rows(i).Item("PackingSlip_Code_Type2").ToString
                '            .Rows(n).Cells(15).Value = dt2.Rows(i).Item("PackingSlip_Code_Type3").ToString
                '            .Rows(n).Cells(16).Value = dt2.Rows(i).Item("PackingSlip_Code_Type4").ToString
                '            .Rows(n).Cells(17).Value = dt2.Rows(i).Item("PackingSlip_Code_Type5").ToString



                '            If dt2.Rows(i).Item("PackingSlip_Code_Type1").ToString <> "" Then
                '                .Rows(n).Cells(5).Style.ForeColor = Color.Red
                '                LockSTS = True
                '            End If

                '            If dt2.Rows(i).Item("PackingSlip_Code_Type2").ToString <> "" Then
                '                .Rows(n).Cells(6).Style.ForeColor = Color.Red
                '                LockSTS = True
                '            End If


                '            If dt2.Rows(i).Item("PackingSlip_Code_Type3").ToString <> "" Then
                '                .Rows(n).Cells(7).Style.ForeColor = Color.Red
                '                LockSTS = True
                '            End If


                '            If dt2.Rows(i).Item("PackingSlip_Code_Type4").ToString <> "" Then
                '                .Rows(n).Cells(8).Style.ForeColor = Color.Red
                '                LockSTS = True
                '            End If

                '            If dt2.Rows(i).Item("PackingSlip_Code_Type5").ToString <> "" Then
                '                .Rows(n).Cells(9).Style.ForeColor = Color.Red
                '                LockSTS = True
                '            End If


                '        Next i

                '    End If

                '    If .RowCount = 0 Then .Rows.Add()

                'End With

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


                da2 = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code from Weaver_Cloth_Receipt_Head Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
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

                    btn_Selection.Enabled = False

                End If

            Else
                new_record()

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ChkNo.Text)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Piece_Checking_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Piece_Checking_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_Piece_Checking_Entry, New_Entry, Me, con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", NewCode, "Weaver_Piece_Checking_Date", "(Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        Da = New SqlClient.SqlDataAdapter("select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> ''or PackingSlip_Code_Type3 <> ''or PackingSlip_Code_Type4 <> ''or PackingSlip_Code_Type5 <> '')", con)
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

        Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code from Weaver_Cloth_Receipt_Head Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
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
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Weaver_Piece_Checking_head", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Weaver_Piece_Checking_Code, Company_IdNo, for_OrderBy", trans)

            cmd.CommandText = "Update ClothSales_Return_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Return_Meters = ReturnMeters_Return, Folding = Folding_Return, Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update ClothSales_Delivery_Return_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Return_Meters = ReturnMeters_Return, Folding = Folding_Return, Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = b.ConsumedYarn_Receipt from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = b.ConsumedPavu_Receipt from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = (case when b.Weaver_Wages_Code <> '' then b.Weaver_Wages_Date else b.Weaver_ClothReceipt_Date end), Folding = b.Folding_Receipt, UnChecked_Meters = (case when b.Weaver_Wages_Code = '' then b.ReceiptMeters_Receipt else 0 end), Meters_Type1 = (case when b.Weaver_Wages_Code <> '' then b.Type1_Wages_Meters else 0 end), Meters_Type2 = (case when b.Weaver_Wages_Code <> '' then b.Type2_Wages_Meters else 0 end), Meters_Type3 = (case when b.Weaver_Wages_Code <> '' then b.Type3_Wages_Meters else 0 end), Meters_Type4 = (case when b.Weaver_Wages_Code <> '' then b.Type4_Wages_Meters else 0 end), Meters_Type5 = (case when b.Weaver_Wages_Code <> '' then b.Type5_Wages_Meters else 0 end) from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Create_Status = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '',  Weaver_Piece_Checking_No = '', Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, ReceiptMeters_Checking = 0, Loom_No = '', Pick = 0, Width = 0, Type1_Meters = 0, Type2_Meters = 0, Type3_Meters = 0, Type4_Meters  = 0, Type5_Meters = 0, Total_Checking_Meters = 0, Weight = 0, Weight_Meter = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Consumed_Yarn = ConsumedYarn_Receipt, Consumed_Pavu = ConsumedPavu_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0, ConsumedYarn_Checking = 0, ConsumedPavu_Checking = 0, Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
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

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby, Weaver_Piece_Checking_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If
            dt.Clear()

            If Trim(movno) <> "" Then move_record(movno)
            'If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ChkNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby, Weaver_Piece_Checking_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)
            'If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ChkNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Weaver_Piece_Checking_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)
            'If Val(movno) <> 0 Then move_record(movno)

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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Weaver_Piece_Checking_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)
            'If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_ChkNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_ChkNo.ForeColor = Color.Red

            dtp_Date.Text = Date.Today.ToShortDateString
            Da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Piece_Checking_No desc", con)
            Dt1 = New DataTable
            da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("Weaver_Piece_Checking_Date").ToString <> "" Then dtp_Date.Text = Dt1.Rows(0).Item("Weaver_Piece_Checking_Date").ToString
                End If
            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Chk No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(InvCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Trim(movno) <> "" Then
                move_record(movno)

            Else
                MessageBox.Show("Chk No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Piece_Checking_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Piece_Checking_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Piece_Checking_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Chk No.", "FOR NEW CHK NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(InvCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Trim(movno) <> "" Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Chk No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_ChkNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT ...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim LotCd As String = ""
        Dim LotNo As String = ""

        Dim clth_ID As Integer = 0
        Dim Wev_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim Wdth_Typ As String = ""

        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""

        Dim vTot_RecMtrs As Single

        Dim vTot_Typ1Mtrs As Single
        Dim vTot_Typ2Mtrs As Single
        Dim vTot_Typ3Mtrs As Single
        Dim vTot_Typ5Mtrs As Single
        Dim vTot_Typ4Mtrs As Single
        Dim vTot_ChkMtrs As Single
        Dim vTot_Wgt As Single

        Dim Nr As Integer = 0

        Dim WagesCode As String = ""

        Dim ConsYarn As Single = 0
        Dim ConsPavu As Single = 0

        Dim stkof_idno As Integer = 0
        Dim Led_type As String = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ChkNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Piece_Checking_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Piece_Checking_Entry, New_Entry, Me, con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", NewCode, "Weaver_Piece_Checking_Date", "(Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Piece_Checking_No desc", dtp_Date.Value.Date) = False Then Exit Sub

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

        If Val(txt_Folding.Text) = 0 Then
            txt_Folding.Text = 100
        End If

        Wev_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        If Wev_ID = 0 Then
            MessageBox.Show("Invalid Weaver Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
            Exit Sub
        End If

        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Quality.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        NoCalc_Status = False
        ' Total_Calculation1()

        vTot_RecMtrs = 0 : vTot_Typ1Mtrs = 0 : vTot_Typ2Mtrs = 0 : vTot_Typ3Mtrs = 0 : vTot_Typ5Mtrs = 0 : vTot_Typ4Mtrs = 0 : vTot_ChkMtrs = 0 : vTot_Wgt = 0

        vTot_RecMtrs = Val(txt_Rec_Meter.Text)
        vTot_Typ1Mtrs = Val(txt_Sound_Meters.Text)
        vTot_Typ2Mtrs = Val(txt_Seconds_Meters.Text)
        vTot_Typ3Mtrs = Val(txt_Bits_Meter.Text)
        vTot_Typ4Mtrs = Val(txt_Reject_Meters.Text)
        vTot_Typ5Mtrs = Val(txt_Other_Meters.Text)
        vTot_ChkMtrs = vTot_Typ1Mtrs + vTot_Typ2Mtrs + vTot_Typ3Mtrs + vTot_Typ5Mtrs + vTot_Typ4Mtrs


        Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code, Loom_IdNo, Width_Type from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        WagesCode = ""
        Lm_ID = 0
        Wdth_Typ = ""
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
            End If
            Lm_ID = Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)
            Wdth_Typ = Dt1.Rows(0).Item("Width_Type").ToString
        End If
        Dt1.Clear()

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                'lbl_ChkNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@CheckingDate", dtp_Date.Value.Date)
            cmd.Parameters.AddWithValue("@RecDate", dtp_Rec_Date.Value.Date)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Weaver_Piece_Checking_Head ( Receipt_Type, Weaver_Piece_Checking_Code,               Company_IdNo       ,     Weaver_Piece_Checking_No  ,                               for_OrderBy                              , Weaver_Piece_Checking_Date,       Ledger_IdNo       ,           Receipt_PkCondition          ,         Piece_Receipt_Code     ,         Piece_Receipt_No      , Piece_Receipt_Date ,         Cloth_IdNo       ,             Party_DcNo        ,             noof_pcs             ,             ReceiptMeters_Receipt   ,               Folding              ,           Excess_Short_Meter           ,Sound_Meters                            , Sound_Piece                     ,   Seconds_Meters                        ,  Seconds_Piece                          ,     Bits_Meters                           , Bits_Piece                            ,  Reject_Meters                           , Reject_Piece                       ,             Others_Meters                  ,  Others_Piece                          ,  Total_Meters                       ,                            Total_Piece                  ,  User_idNo           ) " & _
                "Values                                                   ( 'W' , '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ChkNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ChkNo.Text))) & ",        @CheckingDate      , " & Str(Val(Wev_ID)) & ", '" & Trim(lbl_RecPkCondition.Text) & "', '" & Trim(lbl_RecCode.Text) & "', '" & Trim(txt_RecNo.Text) & "',      @RecDate     , " & Str(Val(clth_ID)) & ", '" & Trim(txt_PDcNo.Text) & "', " & Str(Val(txt_No_Pcs.Text)) & ", " & Str(Val(txt_Rec_Meter.Text)) & ",  " & Str(Val(txt_Folding.Text)) & ",   " & Str(Val(txt_Excess_Short.Text)) & " , " & Str(Val(txt_Sound_Meters.Text)) & ",  " & Str(Val(txt_Sound_Piece.Text)) & ", " & Str(Val(txt_Seconds_Meters.Text)) & ",  " & Str(Val(txt_Seconds_Pieces.Text)) & " , " & Str(Val(txt_Bits_Meter.Text)) & " ,  " & Str(Val(txt_Bits_Pieces.Text)) & ", " & Str(Val(txt_Reject_Meters.Text)) & " , " & Str(Val(txt_Reject_Pieces.Text)) & ", " & Str(Val(txt_Other_Meters.Text)) & ",  " & Str(Val(txt_Other_Pieces.Text)) & ", " & Str(Val(txt_Total_Meters.Text)) & ", " & Str(Val(txt_Total_Pieces.Text)) & ", " & Val(lbl_UserName.Text) & " ) "
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Piece_Checking_head", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Piece_Checking_Code, Company_IdNo, for_OrderBy", tr)
                '   Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Weaver_Piece_Checking_Details", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No,Beam_No,EndsCount_IdNo,Pcs,Meters_Pc,Meters,Rcpt_Pcs,Rcpt_Meters,Beam_Width_Idno  ,Noof_Used,Set_Code,New_Receipt_BeamNo", "Sl_No", "Weaver_Piece_Checking_Code, For_OrderBy, Company_IdNo, Weaver_Piece_Checking_No, Weaver_Piece_Checking_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Weaver_Piece_Checking_Head set Receipt_Type = 'W', Weaver_Piece_Checking_Date = @CheckingDate, Ledger_IdNo = " & Str(Val(Wev_ID)) & ", Receipt_PkCondition = '" & Trim(lbl_RecPkCondition.Text) & "', Piece_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "', Piece_Receipt_No = '" & Trim(txt_RecNo.Text) & "', Piece_Receipt_Date = @RecDate, Cloth_IdNo = " & Str(Val(clth_ID)) & ", Party_DcNo = '" & Trim(txt_PDcNo.Text) & "', noof_pcs = " & Str(Val(txt_No_Pcs.Text)) & ", ReceiptMeters_Receipt = " & Str(Val(txt_Rec_Meter.Text)) & ", Folding =  " & Str(Val(txt_Folding.Text)) & ", Excess_Short_Meter = " & Str(Val(txt_Excess_Short.Text)) & ",Sound_Meters  =  " & Str(Val(txt_Sound_Meters.Text)) & " , Sound_Piece = " & Str(Val(txt_Sound_Piece.Text)) & ",   Seconds_Meters = " & Str(Val(txt_Seconds_Meters.Text)) & ",  Seconds_Piece =  " & Str(Val(txt_Seconds_Pieces.Text)) & ", Bits_Meters =  " & Str(Val(txt_Bits_Meter.Text)) & " , Bits_Piece = " & Str(Val(txt_Bits_Pieces.Text)) & ",  Reject_Meters =  " & Str(Val(txt_Reject_Meters.Text)) & " , Reject_Piece = " & Str(Val(txt_Reject_Pieces.Text)) & " , Others_Meters = " & Str(Val(txt_Other_Meters.Text)) & " ,  Others_Piece =  " & Str(Val(txt_Other_Pieces.Text)) & " ,  Total_Meters = " & Str(Val(txt_Total_Meters.Text)) & "   ,  Total_Piece =  " & Str(Val(txt_Total_Pieces.Text)) & " , User_idNo =  " & Val(lbl_UserName.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = b.ConsumedYarn_Receipt from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = b.ConsumedPavu_Receipt from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = (case when b.Weaver_Wages_Code <> '' then b.Weaver_Wages_Date else b.Weaver_ClothReceipt_Date end), Folding = b.Folding_Receipt, UnChecked_Meters = (case when b.Weaver_Wages_Code = '' then b.ReceiptMeters_Receipt else 0 end), Meters_Type1 = (case when b.Weaver_Wages_Code <> '' then b.Type1_Wages_Meters else 0 end), Meters_Type2 = (case when b.Weaver_Wages_Code <> '' then b.Type2_Wages_Meters else 0 end), Meters_Type3 = (case when b.Weaver_Wages_Code <> '' then b.Type3_Wages_Meters else 0 end), Meters_Type4 = (case when b.Weaver_Wages_Code <> '' then b.Type4_Wages_Meters else 0 end), Meters_Type5 = (case when b.Weaver_Wages_Code <> '' then b.Type5_Wages_Meters else 0 end) from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Create_Status = 0 and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = ''"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '',  Weaver_Piece_Checking_No = '', Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, ReceiptMeters_Checking = 0, Loom_No = '', Pick = 0, Width = 0, Type1_Meters = 0, Type2_Meters = 0, Type3_Meters = 0, Type4_Meters  = 0, Type5_Meters = 0, Total_Checking_Meters = 0, Weight = 0, Weight_Meter = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Consumed_Yarn = ConsumedYarn_Receipt, Consumed_Pavu = ConsumedPavu_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0, ConsumedYarn_Checking = 0, ConsumedPavu_Checking = 0, Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, ReceiptMeters_Checking = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update ClothSales_Return_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Return_Meters = ReturnMeters_Return, Folding = Folding_Return, Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update ClothSales_Delivery_Return_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Return_Meters = ReturnMeters_Return, Folding = Folding_Return, Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Weaver_Piece_Checking_head", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Piece_Checking_Code, Company_IdNo, for_OrderBy", tr)
           
            ConsYarn = Val(Common_Procedures.get_Weft_ConsumedYarn(con, clth_ID, Val(vTot_RecMtrs), tr))

            ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, clth_ID, Lm_ID, Val(vTot_RecMtrs), Trim(Wdth_Typ), tr))

            If Trim(UCase(lbl_RecPkCondition.Text)) = "CLPUR-" Then
                LotCd = lbl_RecCode.Text & "/P"
                LotNo = txt_RecNo.Text & "/P"

            ElseIf Trim(UCase(lbl_RecPkCondition.Text)) = "CLDRT-" Then
                LotCd = lbl_RecCode.Text & "/D"
                LotNo = txt_RecNo.Text & "/D"

                cmd.CommandText = "Update ClothSales_Delivery_Return_Head set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Weaver_Piece_Checking_Date = @CheckingDate, Folding_Return = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", ReturnMeters_Return = " & Str(Val(vTot_RecMtrs)) & ", Return_Meters = " & Str(Val(vTot_RecMtrs)) & " , Type1_Checking_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & " Where ClothSales_Delivery_Return_Code = '" & Trim(lbl_RecCode.Text) & "'"
                Nr = cmd.ExecuteNonQuery()

            ElseIf Trim(UCase(lbl_RecPkCondition.Text)) = "CLSRT-" Then
                LotCd = lbl_RecCode.Text & "/S"
                LotNo = txt_RecNo.Text & "/S"

                cmd.CommandText = "Update ClothSales_Return_Head set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Weaver_Piece_Checking_Date = @CheckingDate, Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", ReceiptMeters_Checking = " & Str(Val(vTot_RecMtrs)) & ", Return_Meters = " & Str(Val(vTot_RecMtrs)) & " , Type1_Checking_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & " Where ClothSales_Return_Code = '" & Trim(lbl_RecCode.Text) & "'"
                Nr = cmd.ExecuteNonQuery()

            Else
                LotCd = lbl_RecCode.Text
                LotNo = txt_RecNo.Text

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Weaver_Piece_Checking_Date = @CheckingDate, Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", ReceiptMeters_Checking = " & Str(Val(vTot_RecMtrs)) & ", ConsumedYarn_Checking = " & Str(Val(ConsYarn)) & ", Consumed_Yarn = " & Str(Val(ConsYarn)) & ", ConsumedPavu_Checking = " & Str(Val(ConsPavu)) & ", Consumed_Pavu = " & Str(Val(ConsPavu)) & ", Type1_Checking_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & " Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'"
                Nr = cmd.ExecuteNonQuery()
                cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Weaver_Piece_Checking_Date = @CheckingDate, Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", ReceiptMeters_Checking = " & Str(Val(vTot_RecMtrs)) & " Where Cloth_Purchase_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "'"
                Nr = cmd.ExecuteNonQuery()

            End If

            Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Wev_ID)) & ")", , tr)

            stkof_idno = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                stkof_idno = Wev_ID
            Else
                stkof_idno = Val(Common_Procedures.CommonLedger.Godown_Ac)
            End If

            'With dgv_Details

            '    Sno = 0
            '    For i = 0 To .RowCount - 1

            '        If Val(.Rows(i).Cells(0).Value) <> 0 Then

            '            Sno = Sno + 1

            '            Nr = 0
            '            cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set  Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "',  Weaver_Piece_Checking_No = '" & Trim(lbl_ChkNo.Text) & "', Weaver_Piece_Checking_Date = @CheckingDate, Ledger_Idno = " & Str(Val(Wev_ID)) & ", StockOff_IdNo = " & Str(Val(stkof_idno)) & ", Cloth_IdNo = " & Str(Val(clth_ID)) & ", Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(Sno)) & ", PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Val(.Rows(i).Cells(0).Value)))) & ", ReceiptMeters_Checking = " & Str(Val(.Rows(i).Cells(1).Value)) & ", Receipt_Meters = " & Str(Val(.Rows(i).Cells(1).Value)) & ", Loom_No = '" & Trim(.Rows(i).Cells(2).Value) & "', Pick = " & Str(Val(.Rows(i).Cells(3).Value)) & ", Width = " & Str(Val(.Rows(i).Cells(4).Value)) & ", Type1_Meters = " & Str(Val(.Rows(i).Cells(5).Value)) & ", Type2_Meters = " & Str(Val(.Rows(i).Cells(6).Value)) & ", Type3_Meters = " & Str(Val(.Rows(i).Cells(7).Value)) & ", Type4_Meters  = " & Str(Val(.Rows(i).Cells(8).Value)) & ", Type5_Meters = " & Str(Val(.Rows(i).Cells(9).Value)) & ", Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(10).Value)) & ", Weight = " & Str(Val(.Rows(i).Cells(11).Value)) & ", Weight_Meter = " & Str(Val(.Rows(i).Cells(12).Value)) & "  where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "' and Lot_Code = '" & Trim(LotCd) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
            '            Nr = cmd.ExecuteNonQuery()

            '            If Nr = 0 Then
            '                cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code ,             Company_IdNo         ,     Weaver_Piece_Checking_No   ,  Weaver_Piece_Checking_Date,                         Weaver_ClothReceipt_Code                ,      Weaver_ClothReceipt_No     ,                               for_orderby                              , Weaver_ClothReceipt_Date,        Lot_Code      ,       Lot_No         ,           Ledger_Idno   ,            StockOff_IdNo    ,           Cloth_IdNo     ,            Folding_Checking       ,             Folding               ,           Sl_No      ,                 Piece_No               ,                                PieceNo_OrderBy                                         ,            ReceiptMeters_Checking         ,                Receipt_Meters             ,               Loom_No                  ,                Pick                       ,                     Width                 ,            Type1_Meters                  ,                   Type2_Meters           ,        Type3_Meters                       ,           Type4_Meters                   ,        Type5_Meters                      ,                  Total_Checking_Meters    ,                     Weight                ,                   Weight_Meter             ) " & _
            '                                    "     Values                                 (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_ChkNo.Text) & "',        @CheckingDate       , '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "',   '" & Trim(txt_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_RecNo.Text))) & ",      @RecDate           , '" & Trim(LotCd) & "', '" & Trim(LotNo) & "', " & Str(Val(Wev_ID)) & ", " & Str(Val(stkof_idno)) & ", " & Str(Val(clth_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(0).Value) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Trim(.Rows(i).Cells(0).Value)))) & ",  " & Str(Val(.Rows(i).Cells(1).Value)) & ",  " & Str(Val(.Rows(i).Cells(1).Value)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(.Rows(i).Cells(3).Value)) & " ,  " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " , " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & ", " & Str(Val(.Rows(i).Cells(12).Value)) & " ) "
            '                cmd.ExecuteNonQuery()
            '            End If

            '        End If

            '    Next

            'End With

            If Trim(WagesCode) = "" Then

                cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = " & Str(Val(ConsYarn)) & " Where Reference_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = " & Str(Val(ConsPavu)) & " Where Reference_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "'"
                cmd.ExecuteNonQuery()

            End If

            If Val(txt_Sound_Meters.Text) <> 0 Or Val(txt_Seconds_Meters.Text) <> 0 Or Val(txt_Bits_Meter.Text) <> 0 Or Val(txt_Reject_Meters.Text) <> 0 Or Val(txt_Other_Meters.Text) <> 0 Then
                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = @CheckingDate, Folding = " & Str(Val(txt_Folding.Text)) & ", UnChecked_Meters = 0, Meters_Type1 = " & Str(Val(txt_Sound_Meters.Text)) & ", Meters_Type2 = " & Str(Val(txt_Seconds_Meters.Text)) & ", Meters_Type3 = " & Str(Val(txt_Bits_Meter.Text)) & ", Meters_Type4 = " & Str(Val(txt_Reject_Meters.Text)) & ", Meters_Type5 = " & Str(Val(txt_Other_Meters.Text)) & " Where Reference_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "'"
                cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            move_record(lbl_ChkNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    'Private Sub Total_Calculation1()
    '    Dim TotRec As Single
    '    Dim Totsnd As Single
    '    Dim Totsec As Single
    '    Dim Totbit As Single
    '    Dim Totrej As Single
    '    Dim Tototr As Single
    '    Dim Tottlmr As Single
    '    Dim Totwgt As Single


    '    If NoCalc_Status = True Then Exit Sub

    '    TotRec = 0 : Totsnd = 0 : Totsec = 0 : Totbit = 0 : Totrej = 0 : Tototr = 0 : Tottlmr = 0 : Totwgt = 0

    '    With dgv_Details
    '        For i = 0 To .RowCount - 1
    '            If Val(.Rows(i).Cells(1).Value) <> 0 Or Val(.Rows(i).Cells(10).Value) <> 0 Then

    '                TotRec = TotRec + Val(.Rows(i).Cells(1).Value())
    '                Totsnd = Totsnd + Val(.Rows(i).Cells(5).Value())
    '                Totsec = Totsec + Val(.Rows(i).Cells(6).Value())
    '                Totbit = Totbit + Val(.Rows(i).Cells(7).Value())
    '                Totrej = Totrej + Val(.Rows(i).Cells(8).Value())
    '                Tototr = Tototr + Val(.Rows(i).Cells(9).Value())
    '                Tottlmr = Tottlmr + Val(.Rows(i).Cells(10).Value())
    '                Totwgt = Totwgt + Val(.Rows(i).Cells(11).Value())

    '            End If

    '        Next i

    '    End With


    '    With dgv_Details_Total1
    '        If .RowCount = 0 Then .Rows.Add()
    '        .Rows(0).Cells(1).Value = Format(Val(TotRec), "########0.00")
    '        .Rows(0).Cells(5).Value = Format(Val(Totsnd), "########0.00")
    '        .Rows(0).Cells(6).Value = Format(Val(Totsec), "########0.00")
    '        .Rows(0).Cells(7).Value = Format(Val(Totbit), "########0.00")
    '        .Rows(0).Cells(8).Value = Format(Val(Totrej), "########0.00")
    '        .Rows(0).Cells(9).Value = Format(Val(Tototr), "########0.00")
    '        .Rows(0).Cells(10).Value = Format(Val(Tottlmr), "########0.00")
    '        .Rows(0).Cells(11).Value = Format(Val(Totwgt), "########0.000")

    '    End With

    '    With dgv_Details_Total2
    '        If .RowCount = 0 Then .Rows.Add()
    '        .Rows(0).Cells(0).Value = "100%"
    '        .Rows(0).Cells(1).Value = "FOLDING"

    '        .Rows(0).Cells(5).Value = Format(Val(Totsnd) * Val(txt_Folding.Text) / 100, "########0.00")
    '        .Rows(0).Cells(6).Value = Format(Val(Totsec) * Val(txt_Folding.Text) / 100, "########0.00")
    '        .Rows(0).Cells(7).Value = Format(Val(Totbit) * Val(txt_Folding.Text) / 100, "########0.00")
    '        .Rows(0).Cells(8).Value = Format(Val(Totrej) * Val(txt_Folding.Text) / 100, "########0.00")
    '        .Rows(0).Cells(9).Value = Format(Val(Tototr) * Val(txt_Folding.Text) / 100, "########0.00")
    '        .Rows(0).Cells(10).Value = Format(Val(Tottlmr) * Val(txt_Folding.Text) / 100, "########0.00")

    '    End With

    '    Excess_Short_Meter_Calculation()
    'End Sub

    Private Sub Excess_Short_Meter_Calculation()
        Dim Tot_Mtr As Double = 0

        Tot_Mtr = Format(Val(txt_Total_Meters.Text) * Val(txt_Folding.Text) / 100, "######0.00")

        txt_Excess_Short.Text = Format(Val(Tot_Mtr) - Val(txt_Rec_Meter.Text), "######0.00")

    End Sub

    Private Sub Total_Meter_Calculation()

        txt_Total_Meters.Text = Format(Val(txt_Sound_Meters.Text) + Val(txt_Seconds_Meters.Text) + Val(txt_Bits_Meter.Text) + Val(txt_Reject_Meters.Text) + Val(txt_Other_Meters.Text), "#########0.00")

    End Sub

    Private Sub Total_Piece_Calculation()

        txt_Total_Pieces.Text = Format(Val(txt_Sound_Piece.Text) + Val(txt_Seconds_Pieces.Text) + Val(txt_Bits_Pieces.Text) + Val(txt_Reject_Pieces.Text) + Val(txt_Other_Pieces.Text), "#########0.00")

    End Sub

    'Private Sub TotalMeter_Calculation()
    '    Dim fldmtr As Integer = 0
    '    Dim Tot_Pc_Mtrs As Single = 0, Tot_Pc_Wt As Single = 0
    '    Dim fldperc As Single = 0
    '    Dim Wgt_Mtr As Single = 0
    '    Dim k As Integer = 0

    '    On Error Resume Next

    '    With dgv_Details
    '        If .Visible Then
    '            If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 11 Then

    '                .CurrentRow.Cells(10).Value = Format(Val(.CurrentRow.Cells(5).Value) + Val(.CurrentRow.Cells(6).Value) + Val(.CurrentRow.Cells(7).Value) + Val(.CurrentRow.Cells(8).Value) + Val(.CurrentRow.Cells(9).Value), "#########0.00")

    '                Tot_Pc_Mtrs = 0 : Tot_Pc_Wt = 0
    '                For k = 0 To .Rows.Count - 1

    '                    If Val(.CurrentRow.Cells(0).Value) = Val(.Rows(k).Cells(0).Value) Then
    '                        Tot_Pc_Mtrs = Tot_Pc_Mtrs + Val(.Rows(k).Cells(5).Value) + Val(.Rows(k).Cells(6).Value) + Val(.Rows(k).Cells(7).Value) + Val(.Rows(k).Cells(8).Value) + Val(.Rows(k).Cells(9).Value)
    '                        Tot_Pc_Wt = Tot_Pc_Wt + +Val(.Rows(k).Cells(11).Value)
    '                    End If

    '                Next

    '                fldperc = Val(txt_Folding.Text)
    '                If fldperc = 0 Then fldperc = 100

    '                Wgt_Mtr = 0
    '                If Tot_Pc_Mtrs <> 0 Then Wgt_Mtr = Format(Val(Tot_Pc_Wt) / (Tot_Pc_Mtrs * Val(fldperc) / 100), "#########0.000")

    '                For k = 0 To .Rows.Count - 1
    '                    If Val(.CurrentRow.Cells(0).Value) = Val(.Rows(k).Cells(0).Value) Then
    '                        .Rows(k).Cells(12).Value = Format(Val(Wgt_Mtr), "#########0.000")
    '                    End If
    '                Next


    '                ' ''.CurrentRow.Cells(12).Value = 0
    '                ''If Val(.CurrentRow.Cells(10).Value) <> 0 Then
    '                ''    .CurrentRow.Cells(12).Value = Format(Val(.CurrentRow.Cells(11).Value) / Val(.CurrentRow.Cells(10).Value), "#########0.000")
    '                ''End If

    '                ' Total_Calculation1()

    '            End If

    '        End If
    '    End With
    'End Sub

    Private Sub cbo_Weaver_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver.GotFocus
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN')  and  Close_status = 0 ", "(Ledger_idno = 0)")
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' ) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Weaver_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver, dtp_Date, txt_Folding, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaver, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' ) ", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Cloth Receipt :", "FOR CLOTH RECEIPT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                txt_Folding.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_Weaver_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Weaver.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Quality_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Quality.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_Quality_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Quality.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Quality, dtp_Rec_Date, txt_PDcNo, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Quality_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Quality.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Quality, txt_PDcNo, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Quality_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Quality.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Quality.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    'Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
    '    dgv_Details_CellLeave(sender, e)
    'End Sub

    'Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
    '    With dgv_Details
    '        If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Then
    '            If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
    '                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
    '            Else
    '                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
    '            End If
    '        End If
    '    End With
    'End Sub

    'Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    '    On Error Resume Next

    '    With dgv_Details
    '        If .Visible Then
    '            If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 11 Then
    '                TotalMeter_Calculation()
    '            End If
    '        End If
    '    End With
    'End Sub

    'Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs)
    '    dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    'End Sub

    'Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
    '    dgv_Details.EditingControl.BackColor = Color.Lime
    '    dgv_Details.EditingControl.ForeColor = Color.Blue
    '    dgtxt_Details.SelectAll()
    'End Sub

    'Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
    '    With dgv_Details

    '        If e.KeyValue = Keys.Delete Then

    '            If .CurrentCell.ColumnIndex = 5 Then
    '                If Trim(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> "" Then
    '                    e.Handled = True
    '                End If
    '            End If

    '            If .CurrentCell.ColumnIndex = 6 Then
    '                If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
    '                    e.Handled = True
    '                End If
    '            End If

    '            If .CurrentCell.ColumnIndex = 7 Then
    '                If Trim(.Rows(.CurrentCell.RowIndex).Cells(15).Value) <> "" Then
    '                    e.Handled = True
    '                End If
    '            End If
    '            If .CurrentCell.ColumnIndex = 8 Then
    '                If Trim(.Rows(.CurrentCell.RowIndex).Cells(16).Value) <> "" Then
    '                    e.Handled = True
    '                End If
    '            End If
    '            If .CurrentCell.ColumnIndex = 9 Then
    '                If Trim(.Rows(.CurrentCell.RowIndex).Cells(17).Value) <> "" Then
    '                    e.Handled = True
    '                End If
    '            End If

    '        End If

    '    End With

    'End Sub

    'Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
    '    On Error Resume Next
    '    With dgv_Details
    '        If .Visible Then
    '            If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 12 Then

    '                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
    '                    e.Handled = True
    '                End If

    '                If .CurrentCell.ColumnIndex = 5 Then
    '                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> "" Then
    '                        e.Handled = True
    '                    End If
    '                End If

    '                If .CurrentCell.ColumnIndex = 6 Then
    '                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
    '                        e.Handled = True
    '                    End If
    '                End If
    '                If .CurrentCell.ColumnIndex = 7 Then
    '                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(15).Value) <> "" Then
    '                        e.Handled = True
    '                    End If
    '                End If
    '                If .CurrentCell.ColumnIndex = 8 Then
    '                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(16).Value) <> "" Then
    '                        e.Handled = True
    '                    End If
    '                End If
    '                If .CurrentCell.ColumnIndex = 9 Then
    '                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(17).Value) <> "" Then
    '                        e.Handled = True
    '                    End If
    '                End If
    '            End If
    '        End If
    '    End With

    'End Sub

    'Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    With dgv_Details

    '        If e.KeyCode = Keys.Left Then
    '            If .CurrentCell.ColumnIndex <= 0 Then
    '                If .CurrentCell.RowIndex = 0 Then
    '                    txt_Folding.Focus()
    '                Else
    '                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
    '                End If
    '            End If
    '        End If

    '        If e.KeyCode = Keys.Right Then
    '            If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
    '                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
    '                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
    '                        save_record()
    '                    Else
    '                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(2)
    '                    End If
    '                End If
    '            End If
    '        End If


    '    End With

    'End Sub

    'Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Dim i As Integer
    '    Dim n As Integer
    '    Dim nrw As Integer
    '    Dim PNO As String
    '    Dim S As String

    '    If e.Control = True And (UCase(Chr(e.KeyCode)) = "A" Or UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then
    '        With dgv_Details

    '            n = .CurrentRow.Index

    '            PNO = Trim(UCase(.Rows(n).Cells(0).Value))

    '            If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Then

    '                S = Replace(Trim(PNO), Val(PNO), "")
    '                PNO = Val(PNO)

    '                If Trim(UCase(S)) <> "Z" Then
    '                    S = Trim(UCase(S))
    '                    If Trim(S) = "" Then S = "A" Else S = Trim(Chr(Asc(S) + 1))
    '                End If

    '            Else


    '                If Len(PNO) = 1 Then
    '                    S = "1"

    '                Else

    '                    S = Microsoft.VisualBasic.Right(PNO, Len(PNO) - 1)
    '                    S = Val(S) + 1

    '                    PNO = Microsoft.VisualBasic.Left(PNO, 1)

    '                End If

    '            End If

    '            If n <> .Rows.Count - 1 Then
    '                If Trim(UCase(PNO)) & Trim(UCase(S)) = Trim(UCase(.Rows(n + 1).Cells(0).Value)) Then
    '                    MessageBox.Show("Already Piece Inserted", "DES NOT INSERT NEW PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
    '                    Exit Sub
    '                End If
    '            End If

    '            nrw = n + 1

    '            dgv_Details.Rows.Insert(nrw, 1)

    '            dgv_Details.Rows(nrw).Cells(0).Value = Trim(UCase(PNO)) & S

    '            dgv_Details.Rows(nrw).Cells(2).Value = .Rows(n).Cells(2).Value
    '            If Val(.Rows(n).Cells(3).Value) <> 0 Then
    '                dgv_Details.Rows(nrw).Cells(3).Value = Val(.Rows(n).Cells(3).Value)
    '            End If
    '            If Val(.Rows(n).Cells(4).Value) <> 0 Then
    '                dgv_Details.Rows(nrw).Cells(4).Value = Val(.Rows(n).Cells(4).Value)
    '            End If

    '        End With

    '    End If

    '    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

    '        With dgv_Details

    '            n = .CurrentRow.Index

    '            If Val(.Rows(n).Cells(0).Value) = Trim(.Rows(n).Cells(0).Value) Then
    '                MessageBox.Show("cannot remove this piece", "DOES NOT REMOVE DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
    '                Exit Sub
    '            End If

    '            If .CurrentCell.RowIndex = .Rows.Count - 1 Then
    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(n).Cells(i).Value = ""
    '                Next

    '            Else
    '                .Rows.RemoveAt(n)

    '            End If

    '        End With

    '    End If
    'End Sub

    'Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    On Error Resume Next
    '    dgv_Details.CurrentCell.Selected = False
    'End Sub


    'Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
    '    save_record()
    'End Sub

    'Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
    '    Me.Close()
    'End Sub

    'Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
    '    If e.KeyValue = 38 Then cbo_Weaver.Focus() ' SendKeys.Send("+{TAB}")
    '    If (e.KeyValue = 40) Then
    '        If dgv_Details.Rows.Count > 0 Then
    '            dgv_Details.Focus()
    '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
    '            dgv_Details.CurrentCell.Selected = True
    '        End If
    '    End If
    'End Sub
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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN') and Close_status = 0 ", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN') and Close_status = 0 ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' ) and  Close_status = 0 ", "(Ledger_idno = 0)")
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

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clth_IdNo As Integer
        Dim Condt As String = ""

        Try

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

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Clth_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Val(Clth_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Cloth_IdNo = " & Str(Val(Clth_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, e.Ledger_Name from Weaver_Piece_Checking_Head a left outer join Weaver_ClothReceipt_Piece_Details b on a.Weaver_Piece_Checking_Code = b.Weaver_Piece_Checking_Code left outer join Cloth_head c on a.Cloth_idno = c.Cloth_idno left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Weaver_Piece_Checking_Date, for_orderby, Weaver_Piece_Checking_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaver_Piece_Checking_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Piece_Checking_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Piece_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Piece_Receipt_Date").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub
    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub
    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        'If Asc(e.KeyChar) = 13 Then
        '    If dgv_Details.Rows.Count > 0 Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        '        dgv_Details.CurrentCell.Selected = True
        '    End If
        'End If
    End Sub
    Private Sub txt_Rec_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rec_Meter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        Excess_Short_Meter_Calculation()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Piece_Checking_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Piece_Checking_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
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

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
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


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* ,c.* , d.Cloth_Name from Weaver_Piece_Checking_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)



            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Piece_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
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

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_Format1(e)
    End Sub
    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets, sno As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1 As Single, C2 As Single

        p1Font = New Font("Calibri", 10, FontStyle.Bold)

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
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
            .Left = 30
            .Right = 50
            .Top = 30
            .Bottom = 30
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
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If



        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(30) : ClAr(2) = 60 : ClAr(3) = 75 : ClAr(4) = 75 : ClAr(5) = 75 : ClAr(6) = 75 : ClAr(7) = 80 : ClAr(8) = 80 : ClAr(9) = 70 : ClAr(10) = 70
        ClAr(11) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
        C2 = C1 + ClAr(8)

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        sno = 0

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1


                        NoofDets = NoofDets + 1

                        sno = sno + 1
                        vType1 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString())
                        vType2 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString())
                        vType3 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString())
                        vType4 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString())
                        vType5 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString())


                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Val(sno), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType1), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType2), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType3), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType4), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType5), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Checking_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt + 10
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop
                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim strHeight As Single
        Dim C1, w1, w2 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = ""

        Type1 = "" : Type2 = "" : Type3 = "" : Type4 = "" : Type5 = ""
        Type11 = "" : Type22 = "" : Type33 = "" : Type44 = "" : Type55 = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        Type1 = "SOUND" : Type2 = "SECONDS" : Type3 = "BITS" : Type4 = "REJECT" : Type5 = "OTHERS"
        Type11 = "MTRS" : Type22 = "MTRS" : Type33 = "MTRS" : Type44 = "MTRS" : Type55 = "MTRS"

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FOLDING REPORT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 20

        w1 = e.Graphics.MeasureString("CHECKING DATE : ", pFont).Width
        w2 = e.Graphics.MeasureString("RECEIVED MTRS ", pFont).Width


        CurY = CurY + TxtHgt - 10
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "NAME   :  " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING DATE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Piece_Checking_Date").ToString), "dd-MM-yyyy").ToString, LMargin + w1 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RECEIVED NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Piece_Receipt_Date").ToString), "dd-MM-yyyy").ToString, PageWidth - 10, CurY, 1, 0, pFont)



        '"select b.weaver_wages_no, b.weaver_wages_date from weaver_cloth_receipt_head a INNER JOIN weaver_wages_head b ON A.WEAVER_WAGES_CODE = B.WEAVER_WAGes_code where a.weaver_clothreceipt_code = '" & trim(reccode) & "'

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING noof_pcs", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString), "#######0"), LMargin + w1 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RECEIVED MTRS", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00"), LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "FOLDING :  " & Format(Val(prn_HdDt.Rows(0).Item("Folding").ToString), "#######0"), PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "SL.", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PIECE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RECD", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type1), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "NO.", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type11), LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type22), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type33), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type44), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type55), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY




    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Bilno As String = "", BilDt As String = ""
        Dim reccode As String


        Dim w1 As Single

        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        reccode = Trim(Val(lbl_Company.Tag)) & "-" & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString & "/" & Trim(Common_Procedures.FnYearCode)

        da3 = New SqlClient.SqlDataAdapter("select b.weaver_wages_no, b.weaver_wages_date from weaver_cloth_receipt_head a INNER JOIN weaver_wages_head b ON A.WEAVER_WAGES_CODE = B.WEAVER_WAGes_code where a.weaver_clothreceipt_code = '" & Trim(reccode) & "'", con)
        prn_DetDt = New DataTable
        da3.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            BilDt = Format(Convert.ToDateTime(Dt1.Rows(0).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString
            Bilno = Dt1.Rows(0).Item("Weaver_Wages_No").ToString
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)


        vTotType1 = Val(prn_HdDt.Rows(0).Item("Total_Type1_Meters").ToString)
        vTotType2 = Val(prn_HdDt.Rows(0).Item("Total_Type2_Meters").ToString)
        vTotType3 = Val(prn_HdDt.Rows(0).Item("Total_Type3_Meters").ToString)
        vTotType4 = Val(prn_HdDt.Rows(0).Item("Total_Type4_Meters").ToString)
        vTotType5 = Val(prn_HdDt.Rows(0).Item("Total_Type5_Meters").ToString)

        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType1), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType2), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType3), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType4), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType5), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

        CurY = CurY + 10

        w1 = e.Graphics.MeasureString("RECEIVED ", pFont).Width
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Checking_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BILL NO   :   " & Bilno, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "RECEIVED ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE   :    " & BilDt, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)


        CurY = CurY + 10


        Common_Procedures.Print_To_PrintDocument(e, "EXCESS METERS ", LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Excess_Short_Meter").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_Print_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Dt2 As New DataTable

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection


            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.EndsCount_Name from Weaver_Cloth_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Date").ToString
                    '.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Party_DcNo").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString

                    .Rows(n).Cells(10).Value = Val(Dt1.Rows(i).Item("folding").ToString)

                    .Rows(n).Cells(11).Value = "WCLRC-"
                    .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(13).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, c.*, d.EndsCount_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Date").ToString
                    '.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Party_DcNo").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then '--------Ganesh Karthick Textiles (Somanur)
                        .Rows(n).Cells(10).Value = 96.5

                    Else
                        .Rows(n).Cells(10).Value = Val(Dt1.Rows(i).Item("folding").ToString)

                    End If



                    .Rows(n).Cells(11).Value = "WCLRC-"
                    .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(13).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)

                Next

            End If

            Dt1.Clear()


            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from Cloth_Purchase_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  where a.Weaver_Piece_Checking_Code= '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Cloth_Purchase_Receipt_Date, a.for_orderby, a.Cloth_Purchase_Receipt_No", con)
            Dt2 = New DataTable
            Da.Fill(Dt2)

            If Dt2.Rows.Count > 0 Then

                For i = 0 To Dt2.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt2.Rows(i).Item("Cloth_Purchase_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt2.Rows(i).Item("Cloth_Purchase_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt2.Rows(i).Item("Bill_No").ToString
                    .Rows(n).Cells(4).Value = Dt2.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Val(Dt2.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(7).Value = Format(Val(Dt2.Rows(i).Item("ReceiptMeters_Receipt").ToString), "#########0.00")
                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt2.Rows(i).Item("Cloth_Purchase_Receipt_Code").ToString
                    .Rows(n).Cells(10).Value = Val(Dt2.Rows(i).Item("folding").ToString)
                    .Rows(n).Cells(11).Value = "CPREC-"
                    .Rows(n).Cells(12).Value = Val(Dt2.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(13).Value = Val(Dt2.Rows(i).Item("pcs_tono").ToString)

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt2.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, c.* from Cloth_Purchase_Receipt_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Cloth_Purchase_Receipt_Date, a.for_orderby, a.Cloth_Purchase_Receipt_No", con)
            Dt2 = New DataTable
            Da.Fill(Dt2)

            If Dt2.Rows.Count > 0 Then

                For i = 0 To Dt2.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt2.Rows(i).Item("Cloth_Purchase_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt2.Rows(i).Item("Cloth_Purchase_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt2.Rows(i).Item("Bill_No").ToString
                    .Rows(n).Cells(4).Value = Dt2.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Val(Dt2.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(7).Value = Format(Val(Dt2.Rows(i).Item("ReceiptMeters_Receipt").ToString), "#########0.00")
                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = Dt2.Rows(i).Item("Cloth_Purchase_Receipt_Code").ToString
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then '--------Ganesh Karthick Textiles (Somanur)
                        .Rows(n).Cells(10).Value = 96.5

                    Else
                        .Rows(n).Cells(10).Value = Val(Dt1.Rows(i).Item("folding").ToString)

                    End If
                    ' .Rows(n).Cells(10).Value = Val(Dt2.Rows(i).Item("folding").ToString)
                    .Rows(n).Cells(11).Value = "CPREC-"
                    .Rows(n).Cells(12).Value = Val(Dt2.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(13).Value = Val(Dt2.Rows(i).Item("pcs_tono").ToString)

                Next

            End If

            Dt2.Clear()

            '---------------------------
            'CLOTH DELIVERY RETURN
            '---------------------------

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from ClothSales_Delivery_Return_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.ClothSales_Delivery_Return_Date, a.for_orderby, a.ClothSales_Delivery_Return_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Delivery_Return_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Delivery_Return_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Dc_No").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Return_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("ClothSales_Delivery_Return_Code").ToString
                    .Rows(n).Cells(10).Value = Val(Dt1.Rows(i).Item("Folding").ToString)
                    .Rows(n).Cells(11).Value = "CLDRT-"
                    .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(13).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from ClothSales_Delivery_Return_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Weaver_Piece_Checking_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.ClothSales_Delivery_Return_Date, a.for_orderby, a.ClothSales_Delivery_Return_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Delivery_Return_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Delivery_Return_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Dc_No").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Return_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("ClothSales_Delivery_Return_Code").ToString
                    .Rows(n).Cells(10).Value = Val(Dt1.Rows(i).Item("Folding").ToString)
                    .Rows(n).Cells(11).Value = "CLDRT-"
                    .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(13).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)


                Next

            End If
            Dt1.Clear()

            '---------------------------
            'CLOTH SALES RETURN
            '---------------------------

           Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from ClothSales_Return_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.ClothSales_Return_Date, a.for_orderby, a.ClothSales_Return_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Return_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Return_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Invoice_No").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Return_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("ClothSales_Return_Code").ToString
                    .Rows(n).Cells(10).Value = 0
                    .Rows(n).Cells(11).Value = "CLSRT-"
                    .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(13).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()


            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from ClothSales_Return_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Weaver_Piece_Checking_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.ClothSales_Return_Date, a.for_orderby, a.ClothSales_Return_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Return_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Return_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Invoice_No").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Return_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("ClothSales_Return_Code").ToString
                    .Rows(n).Cells(10).Value = 0
                    .Rows(n).Cells(11).Value = "CLSRT-"
                    .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(13).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                For i = 0 To dgv_Selection.Rows.Count - 1
                    dgv_Selection.Rows(i).Cells(8).Value = ""
                Next

                .Rows(RwIndx).Cells(8).Value = 1

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

                Close_Receipt_Selection()

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_Piece(n)

                e.Handled = True

            End If
        End If
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Receipt_Selection()
    End Sub

    Private Sub Close_Receipt_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim K As Integer = 0
        Dim M As Integer = 0
        Dim Clo_Pck As Single = 0
        Dim Clo_Wdth As Single = 0


        ' dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                'If Trim(dgv_Selection.Rows(i).Cells(11).Value) <> "CLSRT-" And Trim(dgv_Selection.Rows(i).Cells(11).Value) <> "CLDRT-" Then
                '    lbl_ChkNo.Text = dgv_Selection.Rows(i).Cells(1).Value
                'End If

                lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(9).Value
                txt_RecNo.Text = dgv_Selection.Rows(i).Cells(1).Value
                If Trim(dgv_Selection.Rows(i).Cells(2).Value) <> "" Then
                    If IsDate(dgv_Selection.Rows(i).Cells(2).Value) = True Then
                        dtp_Rec_Date.Text = Convert.ToDateTime(Trim(dgv_Selection.Rows(i).Cells(2).Value))
                    End If
                End If

                txt_PDcNo.Text = dgv_Selection.Rows(i).Cells(3).Value
                cbo_Quality.Text = dgv_Selection.Rows(i).Cells(4).Value
                txt_No_Pcs.Text = dgv_Selection.Rows(i).Cells(6).Value
                txt_Rec_Meter.Text = dgv_Selection.Rows(i).Cells(7).Value
                txt_Folding.Text = dgv_Selection.Rows(i).Cells(10).Value
                lbl_RecPkCondition.Text = dgv_Selection.Rows(i).Cells(11).Value

                'Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.* from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "' Order by a.sl_no", con)
                'Dt1 = New DataTable
                'Da1.Fill(Dt1)

                'If Dt1.Rows.Count > 0 Then

                '    Clo_Pck = Val(Dt1.Rows(0).Item("Cloth_Pick").ToString)
                '    Clo_Wdth = Val(Dt1.Rows(0).Item("Cloth_Width").ToString)

                '  For j = 0 To Dt1.Rows.Count - 1

                '                        n = dgv_Details.Rows.Add()

                '                        dgv_Details.Rows(n).Cells(0).Value = Dt1.Rows(j).Item("Piece_No").ToString
                '                        If Val(Dt1.Rows(j).Item("Receipt_Meters").ToString) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(1).Value = Dt1.Rows(j).Item("Receipt_Meters").ToString
                '                        End If
                '                        dgv_Details.Rows(n).Cells(2).Value = Dt1.Rows(j).Item("Loom_No").ToString
                '                        If Val(Dt1.Rows(j).Item("Pick").ToString) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(3).Value = Val(Dt1.Rows(j).Item("Pick").ToString)
                '                        Else
                '                            If Val(Dt1.Rows(j).Item("Cloth_Pick").ToString) <> 0 Then
                '                                dgv_Details.Rows(n).Cells(3).Value = Val(Dt1.Rows(j).Item("Cloth_Pick").ToString)
                '                            End If
                '                        End If
                '                        If Val(Dt1.Rows(j).Item("Width").ToString) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(4).Value = Val(Dt1.Rows(j).Item("Width").ToString)
                '                        Else
                '                            If Val(Dt1.Rows(j).Item("Cloth_Width").ToString) <> 0 Then
                '                                dgv_Details.Rows(n).Cells(4).Value = Val(Dt1.Rows(j).Item("Cloth_Width").ToString)
                '                            End If
                '                        End If
                '                        If Val(Dt1.Rows(j).Item("Type1_Meters").ToString) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(j).Item("Type1_Meters").ToString), "#########0.00")
                '                        End If
                '                        If Val(Dt1.Rows(j).Item("Type2_Meters").ToString) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(j).Item("Type2_Meters").ToString), "#########0.00")
                '                        End If
                '                        If Val(Dt1.Rows(j).Item("Type3_Meters").ToString) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(j).Item("Type3_Meters").ToString), "#########0.00")
                '                        End If
                '                        If Val(Dt1.Rows(j).Item("Type4_Meters").ToString) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(j).Item("Type4_Meters").ToString), "#########0.00")
                '                        End If
                '                        If Val(Dt1.Rows(j).Item("Type5_Meters").ToString) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(j).Item("Type5_Meters").ToString), "#########0.00")
                '                        End If
                '                        If (Val(Dt1.Rows(j).Item("Type1_Meters").ToString) + Val(Dt1.Rows(j).Item("Type2_Meters").ToString) + Val(Dt1.Rows(j).Item("Type3_Meters").ToString) + Val(Dt1.Rows(j).Item("Type4_Meters").ToString) + Val(Dt1.Rows(j).Item("Type5_Meters").ToString)) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(10).Value = Format(Val(Dt1.Rows(j).Item("Type1_Meters").ToString) + Val(Dt1.Rows(j).Item("Type2_Meters").ToString) + Val(Dt1.Rows(j).Item("Type3_Meters").ToString) + Val(Dt1.Rows(j).Item("Type4_Meters").ToString) + Val(Dt1.Rows(j).Item("Type5_Meters").ToString), "#########0.00")
                '                        End If
                '                        If Val(Dt1.Rows(j).Item("Weight").ToString) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(11).Value = Format(Val(Dt1.Rows(j).Item("Weight").ToString), "#########0.000")
                '                        End If
                '                        If Val(Dt1.Rows(j).Item("Weight_Meter").ToString) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(12).Value = Format(Val(Dt1.Rows(j).Item("Weight_Meter").ToString), "#########0.000")
                '                        End If

                '                    Next

                '                    For K = Val(dgv_Selection.Rows(i).Cells(12).Value) To (Val(dgv_Selection.Rows(i).Cells(12).Value) + Val(txt_No_Pcs.Text) - 1)

                '                        For M = 0 To dgv_Details.Rows.Count - 1
                '                            If K = Val(dgv_Details.Rows(M).Cells(0).Value) Then
                '                                GoTo LOOOP1
                '                            End If
                '                        Next

                '                        For j = 0 To dgv_Details.Rows.Count - 1
                '                            If K < Val(dgv_Details.Rows(j).Cells(0).Value) Then
                '                                dgv_Details.Rows.Insert(j, 1)
                '                                dgv_Details.Rows(j).Cells(0).Value = K
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
                '                        dgv_Details.Rows(n).Cells(0).Value = K
                '                        If Val(Clo_Pck) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(3).Value = Val(Clo_Pck)
                '                        End If
                '                        If Val(Clo_Wdth) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(4).Value = Val(Clo_Wdth)
                '                        End If

                'LOOOP1:

                '                    Next

                '                Else

                '                    For K = Val(dgv_Selection.Rows(i).Cells(12).Value) To (Val(dgv_Selection.Rows(i).Cells(12).Value) + Val(txt_No_Pcs.Text) - 1)

                '                        n = dgv_Details.Rows.Add()

                '                        dgv_Details.Rows(n).Cells(0).Value = K
                '                        If Val(Clo_Pck) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(3).Value = Val(Clo_Pck)
                '                        End If
                '                        If Val(Clo_Wdth) <> 0 Then
                '                            dgv_Details.Rows(n).Cells(4).Value = Val(Clo_Wdth)
                '                        End If

                '                    Next

                '                End If
                Dt1.Clear()

                ' Total_Calculation1()

                Exit For

            End If

        Next i

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_Folding.Enabled And txt_Folding.Visible Then txt_Folding.Focus()

    End Sub

    Private Sub txt_Other_Meters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Other_Meters.KeyDown
        If e.KeyValue = 38 Then
            txt_Other_Pieces.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    'Private Sub txt_Folding_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Folding.TextChanged
    '    Total_Calculation1()
    'End Sub

    'Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
    '    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
    '        dgv_Details_KeyUp(sender, e)
    '    End If
    '    If e.Control = True And (UCase(Chr(e.KeyCode)) = "A" Or UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then
    '        dgv_Details_KeyUp(sender, e)
    '    End If
    'End Sub



    Private Sub txt_Other_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Other_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
        Total_Meter_Calculation()
    End Sub

    Private Sub txt_Total_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Total_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub txt_Seconds_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Seconds_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        Total_Meter_Calculation()
    End Sub

    Private Sub txt_Total_Pieces_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Total_Pieces.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        Total_Piece_Calculation()
    End Sub

    Private Sub txt_Sound_Piece_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Sound_Piece.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        Total_Piece_Calculation()
    End Sub

    Private Sub txt_Sound_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Sound_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        Total_Meter_Calculation()
    End Sub

    Private Sub txt_Seconds_Pieces_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Seconds_Pieces.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        Total_Piece_Calculation()
    End Sub

    Private Sub txt_Reject_Pieces_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Reject_Pieces.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        Total_Piece_Calculation()
    End Sub

    Private Sub txt_Reject_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Reject_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        Total_Meter_Calculation()
    End Sub

    Private Sub txt_Other_Pieces_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Other_Pieces.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        Total_Piece_Calculation()
    End Sub

    Private Sub txt_Total_Meters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Total_Meters.TextChanged
        Excess_Short_Meter_Calculation()
    End Sub

    Private Sub dgv_Filter_Details_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellContentClick

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub
End Class