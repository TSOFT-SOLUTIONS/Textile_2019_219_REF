Public Class Weaver_Cloth_Inward
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private Pk_Condition As String = "WVCIN-"
    Private PkCondition_WADVP As String = "WPADP-"
    Private PkCondition_WPYMT As String = "WPYMT-"
    Private PkCondition_WCLRC As String = "WCLRC-"
    Private PkCondition_WFRGT As String = "WFRGT-"
    Private PkCondition_WPTDS As String = "GWTDS-"
    Private Pk_Condition2 As String = "GWWAL-"

    Private Cooli_Count As Integer = 1
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(100, 1000, 50) As String
    Private prn_DetAr_Ledger(100, 1000, 50) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private print_Format As String = ""
    Private prn_HdAr(100, 10) As String
    Private prn_DetAr1(100, 50, 10) As String
    Private prn_pos As Integer = 0
    Private prn_cnt As Integer = 0
    Private Prn_Sql_Conditon_Multi As String = ""

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Public Structure Report_ComboDetails
        Dim PKey As String
        Dim TableName As String
        Dim Selection_FieldName As String
        Dim Return_FieldName As String
        Dim Condition As String
        Dim Display_Name As String
        Dim BlankFieldCondition As String
        Dim CtrlType_Cbo_OR_Txt As String
        Dim MultiSelection_Status As Boolean
        Dim MultiSelectedIdNos_AsString As String
        Dim MultiSelectedNames_AsString As String
        Dim MultiSelectedIdNos_ForInQuery As String
        Dim MultiSelectedNames_ForInQuery As String
    End Structure

    Private RptCboDet(10) As Report_ComboDetails
    Private Wev_LedgerIdNo(500) As String
    Private glob_i As Integer = 0
    Private Loop_sts As Boolean = False

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_Grid_WeaverName.Text = ""

        cbo_TransportName.Text = ""
        cbo_Vechile.Text = ""

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        cbo_Filter_CountName.Text = ""
        cbo_Filter_MillName.Text = ""
        cbo_Filter_PartyName.Text = ""

        lbl_Frieght.Text = ""
        txt_NofoKattu.Text = ""
        txt_RatePerKattu.Text = ""
        txt_Party_DcNo.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        msk_LedgerFromDate.Text = ""
        msk_LedgerToDate.Text = ""
        dtp_LedgerFromDate.Text = ""
        dtp_LedgerToDate.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Grid_WeaverName.Visible = False
        cbo_Grid_ClothName.Visible = False
        cbo_Multi_WeaverName.Text = ""

        NoCalc_Status = False
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

        If Me.ActiveControl.Name <> cbo_Grid_WeaverName.Name Then
            cbo_Grid_WeaverName.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Grid_ClothName.Name Then
            cbo_Grid_ClothName.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Weaver_Yarn_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_WeaverName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_WeaverName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
           
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_WeaverName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "WEANER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_WeaverName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Multi_WeaverName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "WEANER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Multi_WeaverName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Weaver_Yarn_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Weaver_Yarn_Delivery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable

        Me.Text = ""

        con.Open()

        cbo_Grid_WeaverName.Visible = False
        cbo_Grid_ClothName.Visible = False

        dtp_Date.Text = ""
        msk_date.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        Pnl_PrintSelection.Visible = False
        Pnl_PrintSelection.Left = (Me.Width - Pnl_PrintSelection.Width) \ 2
        Pnl_PrintSelection.Top = (Me.Height - Pnl_PrintSelection.Height) \ 2 - 100

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_LedgerFromDate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_LedgerToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_WeaverName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_WeaverName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Frieght.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Party_DcNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Multi_WeaverName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NofoKattu.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RatePerKattu.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_LedgerFromDate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_LedgerToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_WeaverName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_WeaverName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Frieght.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Party_DcNo.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Multi_WeaverName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NofoKattu.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RatePerKattu.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_LedgerFromDate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_LedgerToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NofoKattu.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RatePerKattu.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Party_DcNo.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NofoKattu.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RatePerKattu.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Party_DcNo.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

        'Dgv_Details Columns- 35,100,85,270,70,80,105
        'Dgv_Details Columns- 35,90,80,240,65,70,85,75 After Add Thiri

    End Sub

    Private Sub Weaver_Yarn_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            ElseIf Pnl_Back.Enabled = True Then
                dgv1 = dgv_Details



            End If

            If IsNothing(dgv1) = False Then

                With dgv1



                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                cbo_TransportName.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 6 Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(9)

                        ElseIf .CurrentCell.ColumnIndex = 9 Then

                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_NofoKattu.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 9 Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(6)

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
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Wages_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Weaver_Wages_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_Wages_Date").ToString
                msk_date.Text = dtp_Date.Text


                txt_Party_DcNo.Text = dt1.Rows(0).Item("P_Dc_No").ToString

                cbo_TransportName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_Vechile.Text = dt1.Rows(0).Item("Vechile_No").ToString

                txt_NofoKattu.Text = Format(Val(dt1.Rows(0).Item("Pcs").ToString), "##########0.00")
                txt_RatePerKattu.Text = Format(Val(dt1.Rows(0).Item("FrieghtPer_Item").ToString), "############0.00")
                lbl_Frieght.Text = Format(Val(dt1.Rows(0).Item("Freight_Charge").ToString), "##########0.00")

                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Wages_Details a  where  a.Pcs <> 0 and  a.Receipt_Meters <> 0 and a.Weaver_Wages_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then
                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(i).Item("Ledger_IdNo").ToString))
                        dgv_Details.Rows(n).Cells(2).Value = Common_Procedures.Cloth_IdNoToName(con, Val(dt2.Rows(i).Item("Cloth_IdNo").ToString))
                        dgv_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Pcs").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Mark_Length").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Receipt_Meters").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Less_Meters").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Net_Meters").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Gross_Amount").ToString), "########0")

                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("CGST_Amount").ToString), "########0")
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("SGST_Amount").ToString), "########0")

                        dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Tds_Perc_Calc").ToString), "########0")
                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Assesable_Value").ToString), "########0")

                        dgv_Details.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Advance_Less").ToString), "########0")
                        dgv_Details.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0")

                        dgv_Details.Rows(n).Cells(15).Value = dt2.Rows(i).Item("Bill_No").ToString

                    Next i
                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Pcs").ToString)
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.000")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Receipt_Meters").ToString), "########0.000")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Cooly").ToString), "########0")

                    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("TOTAL_CGST_Amount").ToString), "########0")
                    .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("TOTAL_SGST_Amount").ToString), "########0")

                    .Rows(0).Cells(11).Value = Format(Val(dt1.Rows(0).Item("TOTAL_TDS_Amount").ToString), "########0")
                    .Rows(0).Cells(12).Value = Format(Val(dt1.Rows(0).Item("TOTAL_Assesable_Value").ToString), "########0")

                    .Rows(0).Cells(13).Value = Format(Val(dt1.Rows(0).Item("Less_Amount").ToString), "########0")
                    .Rows(0).Cells(14).Value = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0")

                End With

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

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Yarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Yarn_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Pnl_Back.Enabled = False Then
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

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
          
            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_Wages_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_Wages_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(PkCondition_WADVP) & "%'  and Voucher_Code LIKE  '%" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(PkCondition_WADVP) & "%' and Entry_Identification LIKE  '%" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(PkCondition_WADVP) & "%'  and Voucher_Code LIKE  '%" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(PkCondition_WADVP) & "%' and Entry_Identification LIKE  '%" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%'  and Voucher_Code LIKE  '%-" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%' and Entry_Identification LIKE  '%-" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%'  and Voucher_Code LIKE  '%-" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%' and Entry_Identification LIKE  '%-" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(PkCondition_WFRGT) & "%'  and Voucher_Code LIKE  '%-" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(PkCondition_WFRGT) & "%' and Entry_Identification LIKE  '%-" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(PkCondition_WFRGT) & "%'  and Voucher_Code LIKE  '%-" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(PkCondition_WFRGT) & "%' and Entry_Identification LIKE  '%-" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(PkCondition_WPTDS) & "%'  and Voucher_Code LIKE  '%-" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(PkCondition_WPTDS) & "%' and Entry_Identification LIKE  '%-" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(PkCondition_WPTDS) & "%'  and Voucher_Code LIKE  '%-" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(PkCondition_WPTDS) & "%' and Entry_Identification LIKE  '%-" & Trim(NewCode) & "'"
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

        If Filter_Status = False Then




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
        pnl_Filter.BringToFront()
        Pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Wages_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Wages_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Wages_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Wages_No desc", con)
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
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Wages_Head", "Weaver_Wages_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString
            'da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Wages_No desc", con)
            'dt1 = New DataTable
            'da.Fill(dt1)
            'If dt1.Rows.Count > 0 Then
            '    If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
            '        If dt1.Rows(0).Item("Weaver_Yarn_Delivery_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Weaver_Yarn_Delivery_Date").ToString
            '    End If
            'End If
            'dt1.Clear()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Yarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Yarn_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Delv_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim Wev_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0, CSno As Integer = 0, pSno As Integer = 0, ySno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim vTotPcs As Single, vTotMeters As Single, vTotLessMtr As Single, vTotNetMtr As Single, vTotAmt As Single, vTotAdvlsAmt As Single, vTotNetAmt As Single
        Dim vTotCGST As Single, vTotSGST As Single, vTotTDS As Single, vTotASSEBLE As Single
        Dim EntID As String = ""
        Dim Nr As Integer = 0
        Dim Cnt_Wft_Id As Integer = 0
        Dim Cnt_Wft_wgt As Single = 0
        Dim vVou_Amt As String = ""
        Dim EndsCnt_Idno As Integer = 0
        Dim Chck_sts As Boolean = False
        '  Dim EndsCnt_Idno As Integer = 0
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim TdsAc_ID As Integer = 0
        Dim PcsChkCode As String = ""
        Dim Lm_ID As Integer = 0
        Dim Wdth_Typ As String = ""
        Dim Rep_Partcls_Wages As String = ""
        Dim RCM_Sts As String = ""
        Dim vWgAmt As String = ""
        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '   If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Yarn_Delivery_Entry, New_Entry) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        Trans_ID = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_TransportName.Text)))

        'If Trans_ID <> 0 And Val(lbl_Frieght.Text) = 0 Then
        '    MessageBox.Show("Invalid Fright Amount", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_NofoKattu.Enabled And txt_NofoKattu.Visible Then txt_NofoKattu.Focus()
        '    Exit Sub
        'End If


        lbl_UserName.Text = Common_Procedures.User.IdNo
        If Rec_ID = 0 Then Rec_ID = 4


        Total_Calculation()

        vTotPcs = 0 : vTotMeters = 0 : vTotLessMtr = 0 : vTotNetMtr = 0 : vTotAmt = 0 : vTotAdvlsAmt = 0 : vTotNetAmt = 0 : vTotCGST = 0 : vTotSGST = 0 : vTotTDS = 0 : vTotASSEBLE = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotMeters = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotLessMtr = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotNetMtr = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(8).Value())

            vTotCGST = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
            vTotSGST = Val(dgv_Details_Total.Rows(0).Cells(10).Value())

            vTotTDS = Val(dgv_Details_Total.Rows(0).Cells(11).Value())
            vTotASSEBLE = Val(dgv_Details_Total.Rows(0).Cells(12).Value())

            vTotAdvlsAmt = Val(dgv_Details_Total.Rows(0).Cells(13).Value())
            vTotNetAmt = Val(dgv_Details_Total.Rows(0).Cells(14).Value())
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Wages_Head", "Weaver_Wages_Code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@WagesDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Wages_Head (    Weaver_Wages_Code   ,               Company_IdNo       ,     Weaver_Wages_No  ,                     for_OrderBy                                                ,  Weaver_Wages_Date    ,   Weaver_Cloth_Receipt_Code     ,Transport_IdNo        ,Vechile_No                       ,   P_Dc_No                         ,Rec_No , Rec_Date             ,Cloth_IdNo,      Freight_Charge               ,Total_Cooly           ,  Less_Amount              ,  Net_Amount            , Total_Meters          ,    Receipt_Meters ,       Pcs                        ,FrieghtPer_Item                   ,  TOTAL_CGST_Amount , TOTAL_SGST_Amount , TOTAL_TDS_Amount  , TOTAL_Assesable_Value ) " & _
                                   "     Values                 ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @WagesDate        ,          ''                     ," & Val(Trans_ID) & " ,'" & Trim(cbo_Vechile.Text) & "' ,'" & Trim(txt_Party_DcNo.Text) & "',''     ,  @WagesDate          ,0         ," & Str(Val(lbl_Frieght.Text)) & "," & Val(vTotAmt) & "  ," & Val(vTotAdvlsAmt) & "  ," & Val(vTotNetAmt) & " ," & Val(vTotMeters) & "," & Val(vTotNetMtr) & "," & Val(txt_NofoKattu.Text) & "," & Val(txt_RatePerKattu.Text) & " , " & Val(vTotCGST) & "  ," & Val(vTotSGST) & " ," & Val(vTotTDS) & "," & Val(vTotASSEBLE) & ") "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Weaver_Wages_Head set Weaver_Wages_Date =  @WagesDate    ,Transport_IdNo =" & Val(Trans_ID) & " ,Vechile_No ='" & Trim(cbo_Vechile.Text) & "', P_Dc_No  ='" & Trim(txt_Party_DcNo.Text) & "', Freight_Charge  = " & Str(Val(lbl_Frieght.Text)) & " ,Total_Cooly  =" & Val(vTotAmt) & " ,  Less_Amount =" & Val(vTotAdvlsAmt) & " ,  Net_Amount =" & Val(vTotNetAmt) & " , Total_Meters  =" & Val(vTotMeters) & "  , Receipt_Meters =" & Val(vTotNetMtr) & ",  Pcs =" & Val(txt_NofoKattu.Text) & " ,FrieghtPer_Item =" & Val(txt_RatePerKattu.Text) & " ,  TOTAL_CGST_Amount =" & Val(vTotCGST) & "  , TOTAL_SGST_Amount =" & Val(vTotSGST) & "   , TOTAL_TDS_Amount =" & Val(vTotTDS) & "   , TOTAL_Assesable_Value =" & Val(vTotASSEBLE) & "    Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            Partcls = "Wages : Bill.No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_Wages_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(PkCondition_WADVP) & "-%'  and Voucher_Code LIKE  '%-" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(PkCondition_WADVP) & "-%' and Entry_Identification LIKE  '%-" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(PkCondition_WADVP) & "-%'  and Voucher_Code LIKE  '%-" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(PkCondition_WADVP) & "-%' and Entry_Identification LIKE  '%-" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%'  and Voucher_Code LIKE  '%-" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%' and Entry_Identification LIKE  '%-" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%'  and Voucher_Code LIKE  '%-" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%' and Entry_Identification LIKE  '%-" & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()


            With dgv_Details

                Sno = 0
                CSno = 0
                Cnt_Wft_Id = 0
                EndsCnt_Idno = 0
                pSno = 1000
                CSno = 1000
                ySno = 1000

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1

                        Wev_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(.Rows(i).Cells(1).Value), tr)
                        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, Trim(.Rows(i).Cells(2).Value), tr)
                       
                        Cnt_Wft_Id = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_idno = " & Str(Val(Clo_ID)) & ")", , tr))

                        EndsCnt_Idno = Val(Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "EndsCount_IdNo", "(cloth_idno = " & Str(Val(Clo_ID)) & " and Sl_No = 1 )", , tr))

                        Da1 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Wages_Details a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bill_No = " & Str(Val(.Rows(i).Cells(15).Value)) & " and Ledger_IdNo = " & Str(Val(Wev_ID)) & " ", con)
                        Da1.SelectCommand.Transaction = tr
                        Dt1 = New DataTable
                        Da1.Fill(Dt1)

                        If Dt1.Rows.Count > 0 Then
                            MessageBox.Show(Trim(.Rows(i).Cells(1).Value) & " - Duplicate Bill No - " & (.Rows(i).Cells(15).Value), "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            tr.Commit()

                            Dt1.Dispose()
                            Da.Dispose()
                            Exit Sub
                        End If

                        Nr = 0
                        cmd.CommandText = "Insert into Weaver_Wages_Details(Weaver_Wages_Code  , Company_IdNo                     , Weaver_Wages_No               , for_OrderBy                                                            , Weaver_Wages_Date ,  Sl_No               ,        Ledger_IdNo        ,  Cloth_IdNo             ,       Pcs                               ,   Mark_Length                            ,  Receipt_Meters                          , Less_Meters                             , Net_Meters                              , Gross_Amount                             ,CGST_Amount                               , SGST_Amount                              ,  Tds_Perc_Calc                          , Assesable_Value                          ,  Advance_Less                              , Net_Amount                               ,                   Bill_No  ) " & _
                                                               "Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @WagesDate        , " & Str(Val(Sno)) & ",   " & Str(Val(Wev_ID)) & ", " & Str(Val(Clo_ID)) & "," & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & "," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & " ," & Str(Val(.Rows(i).Cells(9).Value)) & " ," & Str(Val(.Rows(i).Cells(10).Value)) & "," & Str(Val(.Rows(i).Cells(11).Value)) & "," & Str(Val(.Rows(i).Cells(12).Value)) & "," & Str(Val(.Rows(i).Cells(13).Value)) & "," & Str(Val(.Rows(i).Cells(14).Value)) & " ," & Str(Val(.Rows(i).Cells(15).Value)) & " )"
                        Nr = cmd.ExecuteNonQuery()

                        'Nr = 0
                        'cmd.CommandText = "Update Stock_Pavu_Processing_Details set Reference_Date = b.Weaver_Wages_Date, Meters =  (b.Pcs  * b.Mark_Length)  from Stock_Pavu_Processing_Details a, Weaver_Wages_Details b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition) & "' + b.Weaver_Wages_Code"
                        'Nr = cmd.ExecuteNonQuery()

                        'If Nr = 0 Then

                        pSno = pSno + 1
                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code      ,  Company_IdNo                    , Reference_No                  , for_OrderBy                                                            , Reference_Date , DeliveryTo_Idno, ReceivedFrom_Idno        ,DeliveryToIdno_ForParticulars , ReceivedFromIdno_ForParticulars , Cloth_Idno              , Entry_ID             , Party_Bill_No        , Particulars            , Sl_No                , EndsCount_IdNo                , Sized_Beam , Meters)  " & _
                                                      "Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @WagesDate     , 0              , " & Str(Val(Wev_ID)) & " ,        4                     , " & Str(Val(Wev_ID)) & "       , " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(pSno)) & ", " & Str(Val(EndsCnt_Idno)) & ", 0          , " & Str(Val(.Rows(i).Cells(3).Value) * Val(.Rows(i).Cells(4).Value)) & " )"
                        Nr = cmd.ExecuteNonQuery()
                        'End If

                        'Nr = 0
                        'cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = b.Weaver_Wages_Date, UnChecked_Meters = 0 , Meters_Type1 = b.Net_Meters, Meters_Type2 = 0, Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, Weaver_Wages_Details b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "'  and a.Reference_Code = '" & Trim(Pk_Condition) & "' + b.Weaver_Wages_Code"
                        'Nr = cmd.ExecuteNonQuery()

                        'If Nr = 0 Then
                        CSno = CSno + 1
                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,       StockOff_IdNo         ,        DeliveryTo_Idno        ,       ReceivedFrom_Idno   ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     , Folding,             UnChecked_Meters               ,  Meters_Type1, Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ) " & _
                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @WagesDate  ,          4                 ,               4               , " & Str(Val(Wev_ID)) & " , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   " & Val(CSno) & "     , " & Str(Val(Clo_ID)) & ",   100    ,  0                        ,    " & Str(Val(.Rows(i).Cells(7).Value)) & "    ,       0     ,       0     ,       0     ,       0      ) "
                        Nr = cmd.ExecuteNonQuery()
                        ' End If

                        ySno = ySno + 1
                        Nr = 0
                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code           , Company_IdNo                     , Reference_No                 , for_OrderBy                                                             , Reference_Date     , DeliveryTo_Idno, ReceivedFrom_Idno        , Entry_ID              , Particulars           , Party_Bill_No         , Sl_No                 , Count_IdNo                   , Yarn_Type, Mill_IdNo, Bags, Cones , Weight)  " & _
                                                           "Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @WagesDate         ,       0         , " & Str(Val(Wev_ID)) & ", '" & Trim(EntID) & "' , '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Val(ySno) & "     , " & Str(Val(Cnt_Wft_Id)) & ", 'MILL'   ,    0     ,  0  ,    0  ,   " & Val(.Rows(i).Cells(7).Value) & "  )"
                        Nr = cmd.ExecuteNonQuery()

                        'If Val(.Rows(i).Cells(9).Value) <> 0 Then
                        '    Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), "WADVD-" & Trim(NewCode), tr)
                        'End If

                        vLed_IdNos = ""
                        vVou_Amt = ""

                        vLed_IdNos = Val(Common_Procedures.CommonLedger.Cash_Ac) & "|" & Wev_ID
                        vVou_Amt = Val((.Rows(i).Cells(13).Value)) & "|" & -1 * Val((.Rows(i).Cells(13).Value))
                        If Common_Procedures.Voucher_Updation(con, "Wea.AdvDed", Val(lbl_Company.Tag), Trim(PkCondition_WADVP) & Trim(Val(Wev_ID)) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_RefNo.Text) & IIf(Trim(txt_Party_DcNo.Text) <> "", " , P.Dc.No : " & Trim(txt_Party_DcNo.Text), ""), vLed_IdNos, vVou_Amt, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                            Throw New ApplicationException(ErrMsg)
                        End If

                        'vLed_IdNos = ""
                        'vVou_Amt = ""

                        'vLed_IdNos = Wev_ID & "|" & Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac)
                        'vVou_Amt = Val((.Rows(i).Cells(8).Value)) & "|" & -1 * Val((.Rows(i).Cells(8).Value))

                        'If Common_Procedures.Voucher_Updation(con, "Wea.Wages", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_RefNo.Text) & IIf(Trim(txt_Party_DcNo.Text) <> "", " , P.Dc.No : " & Trim(txt_Party_DcNo.Text), ""), vLed_IdNos, vVou_Amt, ErrMsg, tr,Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                        '    Throw New ApplicationException(ErrMsg)
                        'End If

                        Cr_ID = Wev_ID
                        Dr_ID = Common_Procedures.CommonLedger.Weaving_Wages_Ac
                        TdsAc_ID = Common_Procedures.CommonLedger.TDS_Payable_Ac

                        RCM_Sts = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_GSTinNo", "(Ledger_IdNo = " & Wev_ID & ")", 0, tr)
                        vVou_Amts = 0
                        If Trim(RCM_Sts) <> "" Then

                            vLed_IdNos = Wev_ID & "|" & Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac) & "|24|25"
                            vVou_Amts = Format(Val(CSng((.Rows(i).Cells(8).Value))), "#########0.00") & "|" & -1 * Format(Val(CSng((.Rows(i).Cells(8).Value))) - Val(CSng((.Rows(i).Cells(9).Value))) - Val(CSng((.Rows(i).Cells(10).Value))), "#########0.00") & "|" & -1 * Format(Val(CSng((.Rows(i).Cells(9).Value))), "#########0.00") & "|" & -1 * Format(Val(CSng((.Rows(i).Cells(10).Value))), "#########0.00")
                            If Common_Procedures.Voucher_Updation(con, "WeaWg.Wages", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(Val(Wev_ID)) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_RefNo.Text) & IIf(Trim(txt_Party_DcNo.Text) <> "", " , P.Dc.No : " & Trim(txt_Party_DcNo.Text), ""), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                                Throw New ApplicationException(ErrMsg)
                            End If

                        Else   'With Out Registeration
                            '27 - RCM CGST
                            '28 - RCM SGST

                            vLed_IdNos = Wev_ID & "|27|28|" & Common_Procedures.CommonLedger.Weaving_Wages_Ac & "|24|25"
                            vVou_Amts = Format(Val(CSng((.Rows(i).Cells(8).Value)) - Val((.Rows(i).Cells(9).Value)) - Val((.Rows(i).Cells(10).Value))), "#########0.00") & "|" & Format(Val(CSng((.Rows(i).Cells(9).Value))), "##########0.00") & "|" & Format(Val(CSng((.Rows(i).Cells(10).Value))), "###########0.00") & "|" & -1 * Format(Val(CSng((.Rows(i).Cells(8).Value)) - Val((.Rows(i).Cells(9).Value)) - Val((.Rows(i).Cells(10).Value))), "#########0.00") & "|" & -1 * Format(Val(CSng((.Rows(i).Cells(9).Value))), "#########0.00") & "|" & -1 * Format(Val(CSng((.Rows(i).Cells(10).Value))), "#########0.00")

                            If Common_Procedures.Voucher_Updation(con, "WeaWg.Wages", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(Val(Wev_ID)) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_RefNo.Text) & IIf(Trim(txt_Party_DcNo.Text) <> "", " , P.Dc.No : " & Trim(txt_Party_DcNo.Text), ""), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                                Throw New ApplicationException(ErrMsg)
                            End If

                        End If

                        '--Tds A/c Posting
                        Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPTDS) & Trim(NewCode), tr)
                        vLed_IdNos = ""
                        vVou_Amts = ""
                        ErrMsg = ""

                        vLed_IdNos = Val(Common_Procedures.CommonLedger.TDS_Payable_Ac) & "|" & Wev_ID
                        vVou_Amts = Val(CSng((.Rows(i).Cells(11).Value))) & "|" & -1 * Val(CSng((.Rows(i).Cells(11).Value)))

                        If Common_Procedures.Voucher_Updation(con, "WeaWg.Tds", Val(lbl_Company.Tag), Trim(PkCondition_WPTDS) & Trim(Val(Wev_ID)) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_RefNo.Text) & IIf(Trim(txt_Party_DcNo.Text) <> "", " , P.Dc.No : " & Trim(txt_Party_DcNo.Text), ""), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                            Throw New ApplicationException(ErrMsg)
                        End If

                    End If

                Next

            End With

            Dim vVou_LedIdNos As String = "", vVou_ErrMsg As String = ""
            vVou_Amts = ""

            If Val(lbl_Frieght.Text) = 0 Then lbl_Frieght.Text = 0.0

            vVou_LedIdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(lbl_Frieght.Text) & "|" & -1 * Val(lbl_Frieght.Text)
            If Common_Procedures.Voucher_Updation(con, "Wea.Wags", Val(lbl_Company.Tag), Trim(PkCondition_WFRGT) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_date.Text), Partcls, vVou_LedIdNos, vVou_Amts, vVou_ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(vVou_ErrMsg)
            End If

            Dim Cloth_Id As Integer
            Da = New SqlClient.SqlDataAdapter("select a.Ledger_IdNo  from  Ledger_Head a where a.Ledger_Type = 'WEAVER' and a.Ledger_Idno NOT IN (select b.Ledger_idno from Weaver_wages_details b where b.Weaver_Wages_Code= '" & Trim(NewCode) & "' )", con)
            If IsNothing(tr) = False Then
                Da.SelectCommand.Transaction = tr
            End If
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                For j = 0 To Dt.Rows.Count - 1
                    If IsDBNull(Dt.Rows(j).Item("Ledger_IdNo").ToString) = False Then

                        Da1 = New SqlClient.SqlDataAdapter("select top 1 Cloth_IdNo  from  Weaver_Wages_Details  where Weaver_Wages_Code <> '" & Trim(NewCode) & "' and  Ledger_IdNo = " & Val(Dt.Rows(j).Item("Ledger_IdNo").ToString) & " order by  Weaver_Wages_No desc ", con)
                        If IsNothing(tr) = False Then
                            Da1.SelectCommand.Transaction = tr
                        End If
                        Dt1 = New DataTable
                        Da1.Fill(Dt1)

                        Cloth_Id = 0
                        If Dt1.Rows.Count > 0 Then
                            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                                Cloth_Id = Dt1.Rows(0)(0).ToString
                            End If
                        End If
                        ' Cloth_Id = Dt1.Rows(0).Item("Cloth_IdNo").ToString()

                        If Dt1.Rows.Count > 0 Then
                           
                            Nr = 0
                            Sno = Sno + 1
                            cmd.CommandText = "Insert into Weaver_Wages_Details(Weaver_Wages_Code  , Company_IdNo                     , Weaver_Wages_No               , for_OrderBy                                                            , Weaver_Wages_Date ,  Sl_No               ,        Ledger_IdNo                                            ,  Cloth_IdNo             ,       Pcs   ,   Mark_Length      ,  Receipt_Meters  , Less_Meters  , Net_Meters   , Gross_Amount  ,Advance_Less  , Net_Amount) " & _
                                                                   "Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @WagesDate        , " & Str(Val(Sno)) & ",   " & Str(Val(Dt.Rows(j).Item("Ledger_IdNo").ToString)) & " ,     " & Str(Val(Cloth_Id)) & "                 ,          0  ,         0          ,        0         ,      0       ,       0      ,     0         ,     0        ,         0    )"
                            Nr = cmd.ExecuteNonQuery()

                        End If
                            Dt1.Dispose()
                            Da1.Dispose()

                    End If
                Next

            End If

            Dt.Dispose()
            Da.Dispose()


            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)


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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub cbo_Grid_WeaverName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_WeaverName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Grid_WeaverName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_WeaverName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_WeaverName, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_WeaverName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    txt_Party_DcNo.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                End If
            End If
            If (e.KeyValue = 40 And cbo_Grid_WeaverName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_Grid_WeaverName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_WeaverName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_WeaverName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save the details", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.OK Then
                        save_record()
                    Else
                        msk_date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With
        End If
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_WeaverName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_WeaverName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_RecFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_TransportName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transportname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, Nothing, cbo_Vechile, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_TransportName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub cbo_Transportname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportName, cbo_Vechile, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_TransportName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Yarn_Delivery_Head", "Vechile_No", "", "")

    End Sub
    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, cbo_TransportName, txt_NofoKattu, "Weaver_Yarn_Delivery_Head", "Vechile_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vechile, txt_NofoKattu, "Weaver_Yarn_Delivery_Head", "Vechile_No", "", "", False)
    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        Total_Calculation()

    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_Details
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
            'If Val(.CurrentRow.Cells(15).Value) = 0 Then
            '.CurrentRow.Cells(15).Value = .CurrentRow.Index + 1
            'End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_WeaverName.Visible = False Or Val(cbo_Grid_WeaverName.Tag) <> e.RowIndex Then

                    cbo_Grid_WeaverName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead WHERE (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER') and Close_status = 0  order by Ledger_DisplayName", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_WeaverName.DataSource = Dt1
                    cbo_Grid_WeaverName.DisplayMember = "Ledger_DisplayName"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_WeaverName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_WeaverName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Grid_WeaverName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_WeaverName.Height = rect.Height  ' rect.Height
                    cbo_Grid_WeaverName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_WeaverName.Tag = Val(e.RowIndex)
                    cbo_Grid_WeaverName.Visible = True

                    cbo_Grid_WeaverName.BringToFront()
                    cbo_Grid_WeaverName.Focus()


                Else

                    cbo_Grid_WeaverName.Visible = False


                End If

            End If
            If e.ColumnIndex = 2 Then

                If cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head  order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_ClothName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Grid_ClothName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_ClothName.Height = rect.Height  ' rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()

                Else

                    cbo_Grid_ClothName.Visible = False
                End If


            End If


        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0")
                End If
            End If

            If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If

            If .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 12 Or .CurrentCell.ColumnIndex = 13 Or .CurrentCell.ColumnIndex = 10 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim Wev_ID As Integer = 0
        Dim Adv_Amt As Single = 0
        Dim Wages_For_Type1 As Single = 0
        Dim SoundRate As Double = 0
        Dim ClothID As Integer = 0
        Dim RCM_Sts As String = ""
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details

            Total_Calculation()

            If .Visible Then

                If e.ColumnIndex = 1 Then
                    Wev_ID = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(.Rows(.CurrentRow.Index).Cells(1).Value)))
                    Adv_Amt = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Advance_deduction_amount", "Ledger_IdNo =" & Val(Wev_ID)))
                    If Adv_Amt <> 0 Then
                        .Rows(.CurrentRow.Index).Cells(9).Value = Format(Adv_Amt, "########0")
                    End If
                End If

                If e.ColumnIndex = 5 Or e.ColumnIndex = 6 Then

                    .Rows(.CurrentRow.Index).Cells(7).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(5).Value) - Val(.Rows(.CurrentRow.Index).Cells(6).Value), "##########0.000")

                End If

                If e.ColumnIndex = 7 Then
                    Wages_For_Type1 = 0
                    ClothID = Val(Common_Procedures.Cloth_NameToIdNo(con, Trim(.Rows(.CurrentRow.Index).Cells(2).Value)))
                    Wages_For_Type1 = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Wages_For_Type1", "Cloth_Idno =" & Val(ClothID)))

                    .Rows(.CurrentRow.Index).Cells(8).Value = Format(Wages_For_Type1 * Val(.Rows(.CurrentRow.Index).Cells(7).Value), "###########0")
                End If

                If e.ColumnIndex = 8 Then
                    .Rows(e.RowIndex).Cells(9).Value = Format(Val(.Rows(e.RowIndex).Cells(8).Value) * (2.5 / 100), "##########0")
                    .Rows(e.RowIndex).Cells(10).Value = Format(Val(.Rows(e.RowIndex).Cells(8).Value) * (2.5 / 100), "##########0")
                    .Rows(e.RowIndex).Cells(11).Value = Format(Val(.Rows(e.RowIndex).Cells(8).Value) * (1 / 100), "##########0")
                    RCM_Sts = ""

                    RCM_Sts = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_GSTinNo", "(Ledger_Name = '" & (.Rows(e.RowIndex).Cells(1).Value) & "')", 0)

                    If Trim(RCM_Sts) <> "" Then
                        .Rows(e.RowIndex).Cells(12).Value = Format(Val(.Rows(e.RowIndex).Cells(8).Value) + Val(.Rows(e.RowIndex).Cells(9).Value) + Val(.Rows(.CurrentRow.Index).Cells(10).Value) - Val(.Rows(.CurrentRow.Index).Cells(11).Value), "##########0.00")
                    Else
                        .Rows(e.RowIndex).Cells(12).Value = Format(Val(.Rows(e.RowIndex).Cells(8).Value) - Val(.Rows(e.RowIndex).Cells(11).Value), "##########0.00")
                    End If
                End If

                If e.ColumnIndex = 9 Or e.ColumnIndex = 10 Or e.ColumnIndex = 11 Then
                    .Rows(e.RowIndex).Cells(12).Value = Format(Val(.Rows(e.RowIndex).Cells(8).Value) + Val(.Rows(e.RowIndex).Cells(9).Value) + Val(.Rows(.CurrentRow.Index).Cells(10).Value) - Val(.Rows(.CurrentRow.Index).Cells(11).Value), "##########0.00")
                End If

                If e.ColumnIndex = 12 OR e.ColumnIndex = 13 Then
                 
                    If Val(.Rows(.CurrentRow.Index).Cells(12).Value) <> 0 Then
                        .Rows(.CurrentRow.Index).Cells(14).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(12).Value) - Val(.Rows(.CurrentRow.Index).Cells(13).Value), "##########0")
                    End If
                  
                End If

            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                .Rows.RemoveAt(.CurrentRow.Index)
                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next


            End With
        End If

        Total_Calculation()
    End Sub

    Private Sub dgv_YarnDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        Dim bl_no As Integer = 0
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        bl_no = Common_Procedures.get_MaxCode(con, "Weaver_Wages_Details", "Weaver_Wages_Code", "Bill_No", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)

            .Rows(0).Cells(15).Value = Val(bl_no)
            If n > 1 Then
                .Rows(n - 1).Cells(15).Value = Val(.Rows(n - 2).Cells(15).Value) + 1
            End If
          
        End With
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        On Error Resume Next
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

   

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With
    End Sub

    Private Sub txt_Empty_Beam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub
    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Yarn_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_Yarn_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Yarn_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.DeliveryTo_IdNo = " & Str(Val(Led_IdNo)) & ")"
            End If
            If Val(Cnt_IdNo) <> 0 Then

                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " A.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ""
            End If

            If Val(Mil_IdNo) <> 0 Then

                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " A.Mill_IdNo = " & Str(Val(Mil_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Weaver_Yarn_Delivery_Head a inner join Ledger_head e on a.DeliveryTo_IdNo = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Weaver_Yarn_Delivery_Date, a.for_orderby, a.Weaver_Yarn_Delivery_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Weaver_Yarn_Delivery_Head a left outer join Weaver_Wages_Details b on a.Weaver_Wages_Code = b.Weaver_Wages_Code left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Weaver_Yarn_Delivery_Date, a.for_orderby, a.Weaver_Yarn_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaver_Yarn_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Yarn_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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


    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            Pnl_Back.Enabled = True
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
    Private Sub cbo_Filter_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub
    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        Pnl_PrintSelection.Visible = True
        Pnl_Back.Enabled = False
        btn_Choolie_Chit_Print.Focus()

        Get_PreviousWagesDate()

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String
        Dim cont As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* , e.Transport_Name  from Weaver_Wages_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Left Outer JOIN Transport_Head e ON a.Transport_IdNo = e.Transport_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                If Prn_Sql_Conditon_Multi <> "" Then

                    If Not InStr(Prn_Sql_Conditon_Multi, "and") > 0 Then
                        cont = "and a.Ledger_idno IN ( " & Prn_Sql_Conditon_Multi & ")"
                    Else
                        cont = Prn_Sql_Conditon_Multi
                    End If

                    da2 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.Ledger_Name as Weaver_Name, C.Pan_No , C.Ledger_GSTinNo ,  c.Tamil_Name ,d.Tamil_Name as ClothTamilName ,d.Wages_For_Type1  from Weaver_Wages_Details a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_IdNo where a.Receipt_Meters <> 0  and  a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " " & cont & "  Order by a.Sl_No", con) '  and a.Weaver_Wages_Code = '" & Trim(NewCode) & "'" & cont & "  Order by a.Sl_No", con)
                Else
                    da2 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.Ledger_Name as Weaver_Name, C.Pan_No , C.Ledger_GSTinNo ,c.Tamil_Name , d.Tamil_Name as ClothTamilName ,d.Wages_For_Type1  from Weaver_Wages_Details a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_IdNo where Z  a.Receipt_Meters <> 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  Order by a.Sl_No", con)
                End If
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then

                End If

                glob_i = 0
            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If



            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        'If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Dim c As Integer = 0
        Dim cnt As Integer = 0
        Dim NewCode As String = ""


        If Trim(UCase(print_Format)) = "FORMAT-1" Then

            Printing_Get_Details()

            Printing_Format1(e)

        ElseIf Trim(UCase(print_Format)) = "FORMAT-2" Then

            Printing_Get_Details_Ledger()

            If Prn_Sql_Conditon_Multi = "" Then

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                da1 = New SqlClient.SqlDataAdapter("select Ledger_IdNo , Weaver_Wages_Date  from Weaver_Wages_Details where Weaver_Wages_Code ='" & Trim(NewCode) & "'", con)
                da1.Fill(dt)

                If dt.Rows.Count > 0 Then

                    For i = 0 To dt.Rows.Count - 1

                        i = i + glob_i

                        Weaver_AllStock_Ledger(dt.Rows(i).Item("Ledger_IdNo").ToString, dt.Rows(i).Item("Weaver_Wages_Date").ToString)

                        da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy , meters1 as for_OrderBy, name2 as Ref_No, name6 as Particulars ,name4 as Led_Name ,  name1 as Ref_Code, name3 as Dc_Rec_No,sum(Meters10) as noofitems, (sum(Int6)) as EmptyBeamDel,(sum(Int7)) as EmptyBeamRec, sum(Meters6) as PavuDelvMtrs ,sum(Meters7) as PavuRecMtrs , sum(weight1) as YarnDelvWgt , sum(weight2) as YarnRecWgt ,Name10 as Tamil_Name ,Int4 as Mill_Idno ,Int5 as Count_IdNo from " & Trim(Common_Procedures.ReportTempTable) & " group by name6 , Date1, Int3, meters1, name2, name1, name3 , Name4 ,Name10,Int4 ,Int5 having sum(Int6)  <> 0 or sum(Int7)  <> 0 or sum(Meters6) <> 0 or sum(Meters7) <> 0 or sum(weight1) <> 0 or sum(weight2) <> 0 Order by Date1, Int3, meters1, name2, name1", con)

                        ' da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy, meters1 as for_OrderBy, name2 as Ref_No, name6 as Particulars ,name1 as Ref_Code, name3 as Dc_Rec_No, sum(Meters10) as noofitems, sum(Int6) as EmptyBeam, Meters6 as PavuDelvMtrs, Meters7 as PavuRecMtrs, weight1 as YarnDelvWgt, weight2 as YarnRecWgt from " & Trim(Common_Procedures.ReportTempTable) & " group by Date1, Int3, meters1, name2, name1, name3 ,name6 , meters6 ,meters7 ,weight1,weight2 having sum(Int6)  <> 0 Order by Date1, Int3, meters1, name2, name1 ", con)

                        prn_DetDt1 = New DataTable
                        da2.Fill(prn_DetDt1)

                        prn_pos = i Mod 2

                        Print_Begin(dt.Rows(i).Item("Ledger_IdNo").ToString)

                        Printing_Format2(e, dt.Rows(i).Item("Ledger_IdNo").ToString, prn_pos)


                        glob_i = i + 1


                        If glob_i = dt.Rows.Count Then
                            Exit Sub
                        Else
                            e.HasMorePages = True
                            Return
                        End If

                    Next
                End If

            Else
                Erase Wev_LedgerIdNo

                Wev_LedgerIdNo = New String(100) {}

                Wev_LedgerIdNo = Split(Prn_Sql_Conditon_Multi, ",")

                For Each indx In Wev_LedgerIdNo
                    cnt = cnt + 1
                Next
                '  For Each indx In Wev_LedgerIdNo

                Weaver_AllStock_Ledger(Val(Wev_LedgerIdNo(glob_i)), Convert.ToDateTime((msk_date.Text)))

                da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy , meters1 as for_OrderBy, name2 as Ref_No, name6 as Particulars ,name4 as Led_Name ,  name1 as Ref_Code, name3 as Dc_Rec_No,sum(Meters10) as noofitems, (sum(Int6) )as EmptyBeamDel ,(sum(Int7) )as EmptyBeamRec , sum(Meters6) as PavuDelvMtrs ,sum(Meters7) as PavuRecMtrs , sum(weight1) as YarnDelvWgt , sum(weight2) as YarnRecWgt ,Name10 as Tamil_Name ,Int4 as Mill_Idno ,Int5 as Count_IdNo from " & Trim(Common_Procedures.ReportTempTable) & " group by name6 , Date1, Int3, meters1, name2, name1, name3 , Name4 ,Name10,Int4 ,Int5 having sum(Int7)  <> 0 or sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(Meters7) <> 0 or sum(weight1) <> 0 or sum(weight2) <> 0 Order by Date1, Int3, meters1, name2, name1", con)

                'da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy, meters1 as for_OrderBy, name2 as Ref_No, name6 as Particulars, name4 as Led_Name ,name1 as Ref_Code, name3 as Dc_Rec_No,sum(Meters10) as noofitems, sum(Int6) as EmptyBeam, sum(Meters6) as PavuDelvMtrs ,sum(Meters7) as PavuRecMtrs , sum(weight1) as YarnDelvWgt , sum(weight2) as YarnRecWgt from " & Trim(Common_Procedures.ReportTempTable) & " group by name6 , Date1, Int3, meters1, name2, name1, name3  ,Name4 having sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(Meters7) <> 0 or sum(weight1) <> 0 or sum(weight2) <> 0 Order by Date1, Int3, meters1, name2, name1", con)
                prn_DetDt1 = New DataTable
                da2.Fill(prn_DetDt1)


                Print_Begin(Wev_LedgerIdNo(glob_i))

                Printing_Format2(e, Wev_LedgerIdNo(glob_i), c)

                c = c + 1

                glob_i = glob_i + 1


                If glob_i = cnt Then
                    Exit Sub
                Else
                    e.HasMorePages = True
                    Return
                End If
                '  Next


            End If


        ElseIf Trim(UCase(print_Format)) = "FORMAT-3" Then

            Printing_Get_Details_Ledger()

            If Prn_Sql_Conditon_Multi = "" Then

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                da1 = New SqlClient.SqlDataAdapter("select Ledger_IdNo , Weaver_Wages_Date  from Weaver_Wages_Details where Weaver_Wages_Code ='" & Trim(NewCode) & "'", con)
                da1.Fill(dt)

                If dt.Rows.Count > 0 Then

                    For i = 0 To dt.Rows.Count - 1

                        i = i + glob_i

                        Weaver_AllStock_Ledger(dt.Rows(i).Item("Ledger_IdNo").ToString, dt.Rows(i).Item("Weaver_Wages_Date").ToString)

                        da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy , meters1 as for_OrderBy, name2 as Ref_No, name6 as Particulars ,name4 as Led_Name ,  name1 as Ref_Code, name3 as Dc_Rec_No,sum(Meters10) as noofitems, (sum(Int6)) as EmptyBeamDel,(sum(Int7)) as EmptyBeamRec, sum(Meters6) as PavuDelvMtrs ,sum(Meters7) as PavuRecMtrs , sum(weight1) as YarnDelvWgt , sum(weight2) as YarnRecWgt ,Name10 as Tamil_Name ,Int4 as Mill_Idno ,Int5 as Count_IdNo from " & Trim(Common_Procedures.ReportTempTable) & " group by name6 , Date1, Int3, meters1, name2, name1, name3 , Name4 ,Name10,Int4 ,Int5 having sum(Int6)  <> 0 or sum(Int7)  <> 0 or sum(Meters6) <> 0 or sum(Meters7) <> 0 or sum(weight1) <> 0 or sum(weight2) <> 0 Order by Date1, Int3, meters1, name2, name1", con)

                        ' da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy, meters1 as for_OrderBy, name2 as Ref_No, name6 as Particulars ,name1 as Ref_Code, name3 as Dc_Rec_No, sum(Meters10) as noofitems, sum(Int6) as EmptyBeam, Meters6 as PavuDelvMtrs, Meters7 as PavuRecMtrs, weight1 as YarnDelvWgt, weight2 as YarnRecWgt from " & Trim(Common_Procedures.ReportTempTable) & " group by Date1, Int3, meters1, name2, name1, name3 ,name6 , meters6 ,meters7 ,weight1,weight2 having sum(Int6)  <> 0 Order by Date1, Int3, meters1, name2, name1 ", con)

                        prn_DetDt1 = New DataTable
                        da2.Fill(prn_DetDt1)

                        prn_pos = i Mod 2

                        Print_Begin(dt.Rows(i).Item("Ledger_IdNo").ToString)

                        Printing_Format3(e, dt.Rows(i).Item("Ledger_IdNo").ToString, prn_pos)


                        glob_i = i + 1


                        If glob_i = dt.Rows.Count Then
                            Exit Sub
                        Else
                            e.HasMorePages = True
                            Return
                        End If

                    Next
                End If

            Else
                Erase Wev_LedgerIdNo

                Wev_LedgerIdNo = New String(100) {}

                Wev_LedgerIdNo = Split(Prn_Sql_Conditon_Multi, ",")

                For Each indx In Wev_LedgerIdNo
                    cnt = cnt + 1
                Next
                '  For Each indx In Wev_LedgerIdNo

                Weaver_AllStock_Ledger(Val(Wev_LedgerIdNo(glob_i)), Convert.ToDateTime((msk_date.Text)))

                da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy , meters1 as for_OrderBy, name2 as Ref_No, name6 as Particulars ,name4 as Led_Name ,  name1 as Ref_Code, name3 as Dc_Rec_No,sum(Meters10) as noofitems, (sum(Int6) )as EmptyBeamDel ,(sum(Int7) )as EmptyBeamRec , sum(Meters6) as PavuDelvMtrs ,sum(Meters7) as PavuRecMtrs , sum(weight1) as YarnDelvWgt , sum(weight2) as YarnRecWgt ,Name10 as Tamil_Name ,Int4 as Mill_Idno ,Int5 as Count_IdNo from " & Trim(Common_Procedures.ReportTempTable) & " group by name6 , Date1, Int3, meters1, name2, name1, name3 , Name4 ,Name10,Int4 ,Int5 having sum(Int7)  <> 0 or sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(Meters7) <> 0 or sum(weight1) <> 0 or sum(weight2) <> 0 Order by Date1, Int3, meters1, name2, name1", con)

                'da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy, meters1 as for_OrderBy, name2 as Ref_No, name6 as Particulars, name4 as Led_Name ,name1 as Ref_Code, name3 as Dc_Rec_No,sum(Meters10) as noofitems, sum(Int6) as EmptyBeam, sum(Meters6) as PavuDelvMtrs ,sum(Meters7) as PavuRecMtrs , sum(weight1) as YarnDelvWgt , sum(weight2) as YarnRecWgt from " & Trim(Common_Procedures.ReportTempTable) & " group by name6 , Date1, Int3, meters1, name2, name1, name3  ,Name4 having sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(Meters7) <> 0 or sum(weight1) <> 0 or sum(weight2) <> 0 Order by Date1, Int3, meters1, name2, name1", con)
                prn_DetDt1 = New DataTable
                da2.Fill(prn_DetDt1)


                Print_Begin(Wev_LedgerIdNo(glob_i))

                Printing_Format3(e, Wev_LedgerIdNo(glob_i), c)

                c = c + 1

                glob_i = glob_i + 1


                If glob_i = cnt Then
                    Exit Sub
                Else
                    e.HasMorePages = True
                    Return
                End If
                '  Next


            End If


looop1:

        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font, TFont As Font, T1Font As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim CurY As Single
        Dim Cen As Single = 0, Hgt1 As Single = 0
        Dim L1 As Single = 0, L2 As Single = 0, L3 As Single = 0, L4 As Single = 0
        Dim R1 As Single = 0, R2 As Single = 0, R3 As Single = 0, R4 As Single = 0
        Dim T1 As Single = 0, T2 As Single = 0, T3 As Single = 0, T4 As Single = 0
        Dim Pos As Integer = 0
        Dim CurY1 As Single = 0
        Dim Cnt As Integer = 0


        If PpSzSTS = False Then

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 5
            .Right = 40
            .Top = 10
            .Bottom = 10
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
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        T1 = LMargin + 10 : T2 = LMargin + 100 : T3 = LMargin + 200 : T4 = LMargin + 350

        Cen = PageWidth / 2

        R1 = Cen + 10 : R2 = Cen + 100 : R3 = Cen + 230 : R4 = Cen + 350

        Hgt1 = 10

        Dim dashValues As Single() = {5, 2, 15, 4}
        Dim blackPen As New Pen(Color.Black, 1)
        blackPen.DashPattern = dashValues
        ' e.Graphics.DrawLine(blackPen, New Point(5, 5), New Point(405, 5))


        p1Font = New Font("Calibri", 11, FontStyle.Regular)
        TFont = New Font("Baamini", 11, FontStyle.Bold)
        T1Font = New Font("Baamini", 11, FontStyle.Regular)

        TxtHgt = 18.5

        CurY = TMargin

        Try

            If Val(Cooli_Count) = 0 Then
                Cooli_Count = 1
            End If

            '   If prn_DetDt.Rows.Count > 0 Then
            If Cooli_Count > 0 Then
                CurY1 = CurY

                ' For I = 0 To prn_DetDt.Rows.Count - 1
                For I = 0 To Cooli_Count - 1

                    I = prn_cnt + I


                    Cnt = Cnt + 1
                    Pos = I Mod 2

                    If Pos = 0 Then
                        L1 = T1 : L2 = T2 : L3 = T3 : L4 = T4
                        Cen = PageWidth / 2

                    Else
                        L1 = R1 : L2 = R2 : L3 = R3 : L4 = R4
                        Cen = PageWidth
                        CurY = CurY1
                    End If

                    CurY = CurY + TxtHgt

                    Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(Pos, I, 1), L1, CurY, 0, 0, p1Font)

                    Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(Pos, I, 2), Cen - 10, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt
                    CurY = CurY + TxtHgt
                    CurY = CurY + Hgt1

                    Common_Procedures.Print_To_PrintDocument(e, "$yp", L1, CurY, 0, 0, T1Font)                    '------Coolie
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(Pos, I, 3), L3 - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "KdÊ ghfÊfp", L3, CurY, 0, 0, T1Font)                        '------ Opening advance
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(Pos, I, 6), Cen - 5, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt
                    CurY = CurY + Hgt1
                    Common_Procedures.Print_To_PrintDocument(e, "ghfÊfp gpbjÊjkÊ", L1, CurY, 0, 0, T1Font)
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(Pos, I, 4), L3 - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ghfÊfp gpbjÊjkÊ", L3, CurY, 0, 0, T1Font)
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(Pos, I, 4), Cen - 5, CurY, 1, 0, p1Font)

                    CurY = CurY + TxtHgt
                    CurY = CurY + Hgt1
                    e.Graphics.DrawLine(Pens.Black, L2, CurY, L3, CurY)
                    e.Graphics.DrawLine(Pens.Black, L3 + 80, CurY, Cen - 5, CurY)

                    CurY = CurY + TxtHgt
                    CurY = CurY + Hgt1
                    Common_Procedures.Print_To_PrintDocument(e, "epfu $yp", L1, CurY, 0, 0, T1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(prn_DetAr(Pos, I, 3) - prn_DetAr(Pos, I, 4), "##########0.00"), L3 - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "kPjp ghfÊfp", L3, CurY, 0, 0, T1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(prn_DetAr(Pos, I, 6) - prn_DetAr(Pos, I, 4), "##########0.00"), Cen - 5, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt
                    CurY = CurY + Hgt1
                    e.Graphics.DrawLine(Pens.Black, L2, CurY, L3, CurY)
                    e.Graphics.DrawLine(Pens.Black, L3 + 80, CurY, Cen - 5, CurY)

                    CurY = CurY + TxtHgt
                    CurY = CurY + TxtHgt
                    CurY = CurY + Hgt1

                    If Pos = 1 Then
                        CurY1 = CurY
                    Else
                        e.Graphics.DrawLine(blackPen, Cen, CurY, Cen, CurY1)
                    End If

                    If Cnt = 10 Then
                        prn_cnt = I + 1

                        e.HasMorePages = True
                        Return
                    Else

                        e.Graphics.DrawLine(blackPen, L1, CurY, PageWidth, CurY)

                        prn_cnt = 0
                    End If


                Next

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_DetSNo = 0
                prn_PageNo = 0

                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Get_Details()
        Dim cmd As New SqlClient.SqlCommand
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String = ""

        'prn_HdDt.Clear()
        'prn_DetDt.Clear()

        prn_PageNo = 0
        prn_DetIndx = 0

        prn_DetMxIndx = 0
        prn_Count = 1
        Erase prn_DetAr
        Erase prn_HdAr

        prn_HdAr = New String(500, 50) {}

        prn_DetAr = New String(500, 500, 500) {}

        prn_pos = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Prn_Sql_Conditon_Multi <> "" Then
            da2 = New SqlClient.SqlDataAdapter("select a.*, B.Ledger_Name ,c.Cloth_Name from Weaver_Wages_Details a LEFT OUTER JOIN Ledger_Head B ON a.Ledger_IdNo =B.Ledger_IdNo LEFT OUTER JOIN Cloth_HEAD c ON a.Cloth_IdNo = c.Cloth_IdNo where  a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  " & Prn_Sql_Conditon_Multi & " order by a.Sl_No", con)

            '  da2 = New SqlClient.SqlDataAdapter("select a.*, B.Ledger_Name ,c.Cloth_Name from Weaver_Wages_Details a LEFT OUTER JOIN Ledger_Head B ON a.Ledger_IdNo =B.Ledger_IdNo LEFT OUTER JOIN Cloth_HEAD c ON a.Cloth_IdNo = c.Cloth_IdNo where  a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "' " & Prn_Sql_Conditon_Multi & " order by a.Sl_No", con)
        Else
            da2 = New SqlClient.SqlDataAdapter("select a.*, B.Ledger_Name ,c.Cloth_Name from Weaver_Wages_Details a LEFT OUTER JOIN Ledger_Head B ON a.Ledger_IdNo =B.Ledger_IdNo LEFT OUTER JOIN Cloth_HEAD c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Receipt_Meters <> 0  and  a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " order by a.Sl_No", con)

            ' da2 = New SqlClient.SqlDataAdapter("select a.*, B.Ledger_Name ,c.Cloth_Name from Weaver_Wages_Details a LEFT OUTER JOIN Ledger_Head B ON a.Ledger_IdNo =B.Ledger_IdNo LEFT OUTER JOIN Cloth_HEAD c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Receipt_Meters <> 0  and  a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "' order by a.Sl_No", con)
        End If

        dt2 = New DataTable
        da2.Fill(dt2)

        If dt2.Rows.Count > 0 Then
            For j = 0 To dt2.Rows.Count - 1

                '  If Val(dt2.Rows(j).Item("Receipt_Meters").ToString) <> 0 Then

                prn_DetAr(prn_pos, prn_DetMxIndx, 1) = Trim(dt2.Rows(j).Item("Ledger_Name").ToString)
                prn_DetAr(prn_pos, prn_DetMxIndx, 2) = Format((dt2.Rows(j).Item("Weaver_Wages_Date")), "dd/MM/yyyy")
                prn_DetAr(prn_pos, prn_DetMxIndx, 3) = Format(Val(dt2.Rows(j).Item("Gross_Amount").ToString), "#########0.00")
                prn_DetAr(prn_pos, prn_DetMxIndx, 4) = Format(Val(dt2.Rows(j).Item("Advance_Less").ToString), "#########0.00")
                prn_DetAr(prn_pos, prn_DetMxIndx, 5) = Format(Val(dt2.Rows(j).Item("Net_Amount").ToString), "#########0.00")
                prn_DetAr(prn_pos, prn_DetMxIndx, 6) = Format(Val(Get_AdvanceBalanceDetails_COOLY(Val(dt2.Rows(j).Item("Ledger_IdNo").ToString), dt2.Rows(j).Item("Weaver_Wages_Date"))), "#########0.00")
                prn_DetAr(prn_pos, prn_DetMxIndx, 7) = Format(Val(dt2.Rows(j).Item("Assesable_Value").ToString), "#########0.00")

                prn_DetMxIndx = prn_DetMxIndx + 1
                ' End If

                If prn_pos = 0 Then prn_pos = 1 Else prn_pos = 0

            Next j
        End If
    End Sub

    Private Sub Printing_Get_Details_Ledger()
        Dim cmd As New SqlClient.SqlCommand
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String = ""

        dt2.Clear()

        prn_PageNo = 0
        prn_DetIndx = 0

        prn_DetMxIndx = 0
        prn_Count = 1
        Erase prn_DetAr_Ledger
        Erase prn_HdAr

        prn_HdAr = New String(500, 50) {}

        prn_DetAr_Ledger = New String(500, 1000, 100) {}

        prn_pos = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        cmd.Connection = con
        cmd.Parameters.Clear()
        If IsDate(Convert.ToDateTime(msk_LedgerFromDate.Text)) = True Then
            cmd.Parameters.AddWithValue("@fromdate", Convert.ToDateTime(msk_LedgerFromDate.Text))
        Else
            cmd.Parameters.AddWithValue("@fromdate", Convert.ToDateTime(Common_Procedures.Company_FromDate))
        End If

        If IsDate(Convert.ToDateTime(msk_LedgerToDate.Text)) = True Then
            cmd.Parameters.AddWithValue("@todate", Convert.ToDateTime(msk_LedgerToDate.Text))
        Else
            cmd.Parameters.AddWithValue("@todate", Convert.ToDateTime(Now))
        End If


        If Prn_Sql_Conditon_Multi <> "" Then

            cmd.CommandText = "select SUM(A.Gross_Amount) as Gross_Amount ,SUM(A.Advance_Less) as Advance_Less ,SUM(A.Net_Amount) as Net_Amount , SUM(A.Assesable_Value) as Assesable_Value , SUM(A.Tds_Perc_Calc) as TDS_Value ,a.Ledger_IdNo , Sum(a.Less_Meters) as LessMeters  from Weaver_Wages_Details a LEFT OUTER JOIN Ledger_Head B ON a.Ledger_IdNo =B.Ledger_IdNo LEFT OUTER JOIN Cloth_HEAD c ON a.Cloth_IdNo = c.Cloth_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  Weaver_Wages_Date between @fromdate and @todate and a.ledger_idno in ( " & Prn_Sql_Conditon_Multi & ") GROUP BY a.Ledger_IdNo " ''"order by a.Sl_No"
            da2 = New SqlClient.SqlDataAdapter(cmd)
            dt2 = New DataTable
            da2.Fill(dt2)

        Else
            cmd.CommandText = "select SUM(A.Gross_Amount) as Gross_Amount ,SUM(A.Advance_Less) as Advance_Less ,SUM(A.Net_Amount) as Net_Amount, SUM(A.Assesable_Value) as Assesable_Value , SUM(A.Tds_Perc_Calc) as TDS_Value  , a.Ledger_IdNo , sum(a.Less_Meters) as LessMeters from Weaver_Wages_Details a LEFT OUTER JOIN Ledger_Head B ON a.Ledger_IdNo =B.Ledger_IdNo LEFT OUTER JOIN Cloth_HEAD c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Receipt_Meters <> 0 and  a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  Weaver_Wages_Date between @fromdate and @todate GROUP BY a.Ledger_IdNo  " '"order by a.Sl_No"
            da2 = New SqlClient.SqlDataAdapter(cmd)
            dt2 = New DataTable
            da2.Fill(dt2)

        End If

        dt2 = New DataTable
        da2.Fill(dt2)

        If dt2.Rows.Count > 0 Then
            For j = 0 To dt2.Rows.Count - 1


                prn_DetAr_Ledger(prn_pos, Val(dt2.Rows(j).Item("Ledger_IdNo").ToString), 1) = Format(Val(dt2.Rows(j).Item("Gross_Amount").ToString), "#########0.00")
                prn_DetAr_Ledger(prn_pos, Val(dt2.Rows(j).Item("Ledger_IdNo").ToString), 2) = Format(Val(dt2.Rows(j).Item("Advance_Less").ToString), "#########0.00")
                prn_DetAr_Ledger(prn_pos, Val(dt2.Rows(j).Item("Ledger_IdNo").ToString), 3) = Format(Val(dt2.Rows(j).Item("Net_Amount").ToString), "#########0.00")
                prn_DetAr_Ledger(prn_pos, Val(dt2.Rows(j).Item("Ledger_IdNo").ToString), 6) = Format(Val(dt2.Rows(j).Item("Assesable_Value").ToString), "#########0.00")
                prn_DetAr_Ledger(prn_pos, Val(dt2.Rows(j).Item("Ledger_IdNo").ToString), 7) = Format(Val(dt2.Rows(j).Item("TDS_Value").ToString), "#########0.00")



                ' prn_DetAr_Ledger(prn_pos, Val(dt2.Rows(j).Item("Ledger_IdNo").ToString), 4) = Format(Val(Get_AdvanceBalanceDetails(Val(dt2.Rows(j).Item("Ledger_IdNo").ToString), dt2.Rows(j).Item("Weaver_Wages_Date"))), "#########0.00")

                If IsDate(Convert.ToDateTime(msk_LedgerToDate.Text)) = True Then
                    prn_DetAr_Ledger(prn_pos, Val(dt2.Rows(j).Item("Ledger_IdNo").ToString), 4) = Format(Val(Get_AdvanceBalanceDetails(Val(dt2.Rows(j).Item("Ledger_IdNo").ToString), Convert.ToDateTime(msk_LedgerToDate.Text))), "#########0.00")
                End If

                prn_DetAr_Ledger(prn_pos, Val(dt2.Rows(j).Item("Ledger_IdNo").ToString), 5) = Format(Val(dt2.Rows(j).Item("LessMeters").ToString), "#########0.00")

                prn_DetMxIndx = prn_DetMxIndx + 1

                prn_pos = 0

            Next j
        End If

    End Sub

    Private Function Get_AdvanceBalanceDetails(ByVal Emp_id As Integer, ByVal wages_date As Date) As Double
        Dim cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim Ttl_advnace As Double = 0
        Dim vDate_To As Date, vDate_From As Date
        Dim CompIDCondt As String
        Dim SqlCondt As String = ""
        Dim NewCode As String


        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@WeaWageDate", wages_date.Date)

        If IsDate(msk_LedgerFromDate.Text) = True Then
            vDate_From = Convert.ToDateTime(msk_LedgerFromDate.Text)
        Else
            vDate_From = Common_Procedures.Company_FromDate
        End If
        If IsDate(msk_LedgerToDate.Text) = True Then
            vDate_To = Convert.ToDateTime(msk_LedgerToDate.Text)
        Else
            vDate_To = wages_date
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'cmd.CommandText = "select max(a.weaver_wages_date) from Weaver_Wages_Head a LEFT OUTER JOIN Weaver_Wages_Details B ON A.Weaver_Wages_Code = b.Weaver_Wages_Code Where " & Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & " b.Ledger_IdNo = " & Emp_id & "  and  a.Weaver_Wages_Code <> '" & Trim(NewCode) & "' and a.Weaver_Wages_Date < @WeaWageDate"
        'da1 = New SqlClient.SqlDataAdapter(cmd)
        'dt1 = New DataTable
        'da1.Fill(dt1)

        'If dt1.Rows.Count > 0 Then

        '    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then

        '        If IsDate(dt1.Rows(0)(0).ToString) = True Then
        '            vDate_From = dt1.Rows(0)(0).ToString
        '            vDate_From = DateAdd("d", 1, vDate_From.Date)
        '        End If

        '    End If

        'End If

        'dt1.Clear()
        'da1.Dispose()

        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@WeaWageDate", wages_date)
        cmd.Parameters.AddWithValue("@fromdate", vDate_From.Date)
        cmd.Parameters.AddWithValue("@todate", vDate_To.Date)

        cmd.CommandText = " Truncate Table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "( Currency1) Select  sum(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where a.Ledger_IdNo =" & Emp_id & " and  a.Voucher_Date < @fromdate and a.Voucher_Amount <> 0 and a.Voucher_Code NOT LIKE 'WVCIN-%'"
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "( Currency1) Select  -1*abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where  a.Ledger_IdNo =" & Emp_id & " and a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount < 0  and  a.Voucher_Code = 'WVCIN-" & Trim(NewCode) & "' "
        'cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "( Currency1) Select  -1*abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where  a.Ledger_IdNo =" & Emp_id & " and a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount < 0  and  a.Voucher_Code = 'WVCIN-" & Trim(NewCode) & "' "
        cmd.ExecuteNonQuery()


        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "( Currency1) Select abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where  a.Ledger_IdNo =" & Emp_id & " and a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount > 0 and  a.Voucher_Code NOT LIKE 'WVCIN-%'  "
        'cmd.ExecuteNonQuery()



        da2 = New SqlClient.SqlDataAdapter("select sum(Currency1) as current_balance from " & Trim(Common_Procedures.ReportTempTable) & " ", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > 0 Then

            Ttl_advnace = Val(dt2.Rows(0).Item("current_balance").ToString())

        End If
        dt2.Clear()
        da2.Dispose()

        Get_AdvanceBalanceDetails = Ttl_advnace

    End Function

    Private Function Get_AdvanceBalanceDetails_COOLY(ByVal Emp_id As Integer, ByVal wages_date As Date) As Double
        Dim cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim Ttl_advnace As Double = 0
        Dim vDate_To As Date, vDate_From As Date
        Dim CompIDCondt As String
        Dim SqlCondt As String = ""
        Dim NewCode As String


        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@WeaWageDate", wages_date.Date)

        If IsDate(msk_LedgerFromDate.Text) = True Then
            vDate_From = Convert.ToDateTime(msk_LedgerFromDate.Text)
        Else
            vDate_From = Common_Procedures.Company_FromDate
        End If

        If IsDate(msk_LedgerToDate.Text) = True Then
            vDate_To = Convert.ToDateTime(msk_LedgerToDate.Text)
            vDate_To = vDate_To.AddDays(-1)
        Else
            vDate_To = wages_date
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'cmd.CommandText = "select max(a.weaver_wages_date) from Weaver_Wages_Head a LEFT OUTER JOIN Weaver_Wages_Details B ON A.Weaver_Wages_Code = b.Weaver_Wages_Code Where " & Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & " b.Ledger_IdNo = " & Emp_id & "  and  a.Weaver_Wages_Code <> '" & Trim(NewCode) & "' and a.Weaver_Wages_Date < @WeaWageDate"
        'da1 = New SqlClient.SqlDataAdapter(cmd)
        'dt1 = New DataTable
        'da1.Fill(dt1)

        'If dt1.Rows.Count > 0 Then

        '    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then

        '        If IsDate(dt1.Rows(0)(0).ToString) = True Then
        '            vDate_From = dt1.Rows(0)(0).ToString
        '            vDate_From = DateAdd("d", 1, vDate_From.Date)
        '        End If

        '    End If

        'End If

        'dt1.Clear()
        'da1.Dispose()

        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@WeaWageDate", wages_date)
        cmd.Parameters.AddWithValue("@fromdate", vDate_From.Date)
        cmd.Parameters.AddWithValue("@todate", vDate_To.Date)

        cmd.CommandText = " Truncate Table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "( Currency1) Select  sum(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where a.Ledger_IdNo =" & Emp_id & " and  a.Voucher_Date < @fromdate and a.Voucher_Amount <> 0 and a.Voucher_Code NOT LIKE 'WVCIN-%'"
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "( Currency1) Select  -1*abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where  a.Ledger_IdNo =" & Emp_id & " and a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount < 0  and  a.Voucher_Code = 'WVCIN-" & Trim(NewCode) & "' "
        'cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "( Currency1) Select  -1*abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where  a.Ledger_IdNo =" & Emp_id & " and a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount < 0  " 'and  a.Voucher_Code = 'WVCIN-" & Trim(NewCode) & "' "
        cmd.ExecuteNonQuery()


        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "( Currency1) Select abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where  a.Ledger_IdNo =" & Emp_id & " and a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount > 0 and  a.Voucher_Code NOT LIKE 'WVCIN-%'  "
        'cmd.ExecuteNonQuery()



        da2 = New SqlClient.SqlDataAdapter("select sum(Currency1) as current_balance from " & Trim(Common_Procedures.ReportTempTable) & " ", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > 0 Then

            Ttl_advnace = Val(dt2.Rows(0).Item("current_balance").ToString())

        End If
        dt2.Clear()
        da2.Dispose()

        Get_AdvanceBalanceDetails_COOLY = Ttl_advnace

    End Function
    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString(" Vehicle No :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), " #######0.000"), PageWidth - 10, CurY, 1, 0, pFont)

        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, " Vehicle No : ", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
        'If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Empty Beam : " & Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 20, CurY, 1, 0, pFont)
        'End If
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal Led_Id As Integer, ByVal pos As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt3 As New DataTable
        Dim pFont As Font, TFont As Font
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
        Dim Pavu_Dr As Double = 0
        Dim Pavu_Cr As Double = 0
        Dim Yarn_Dr As Double = 0
        Dim Yarn_Cr As Double = 0

        Dim Beam_Dr As Integer = 0
        Dim Beam_Cr As Integer = 0

        Dim Beam_Bal As Integer = 0
        Dim Reed As Double = 0
        Dim pick As Double = 0
        Dim width As Double = 0
        Dim Led_Idno As Integer = 0
        Dim Cloth_TamilName As String = ""
        Dim YARN_TamilName As String = ""

        If PpSzSTS = False Then
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
            .Left = 25
            .Right = 60
            .Top = 30
            .Bottom = 30
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
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(75) : ClAr(2) = 200 : ClAr(3) = 50 : ClAr(4) = 80 : ClAr(5) = 80 : ClAr(6) = 90 : ClAr(7) = 90 : ClAr(8) = 50
        ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, Led_Id)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt1.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt1.Rows.Count - 1

                        CurY = CurY + TxtHgt
                        pFont = New Font("Calibri", 10, FontStyle.Regular)
                        TFont = New Font("Baamini", 10, FontStyle.Regular)

                        If IsDate(prn_DetDt1.Rows(prn_DetIndx).Item("Date1")) = True Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(prn_DetDt1.Rows(prn_DetIndx).Item("Date1"), "dd/MM/yyyy"), LMargin, CurY, 2, ClAr(1), pFont)
                        Else
                            If prn_DetDt1.Rows.Count > 1 Then
                                If IsDate(prn_DetDt1.Rows(prn_DetIndx + 1).Item("Date1")) = True Then
                                    'If Trim(UCase(prn_DetDt1.Rows(prn_DetIndx).Item("Particulars").ToString)) = "PAVU" Then
                                    Common_Procedures.Print_To_PrintDocument(e, Format(prn_DetDt1.Rows(prn_DetIndx + 1).Item("Date1"), "dd/MM/yyyy"), LMargin, CurY, 2, ClAr(1), pFont)
                                End If
                            End If
                         
                        End If

                        Led_Idno = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Idno", "Ledger_Name = '" & Trim(UCase(prn_DetDt1.Rows(prn_DetIndx).Item("Led_Name").ToString)) & "'"))

                        da = New SqlClient.SqlDataAdapter("select b.* from Weaver_Wages_Details a INNER JOIN Cloth_Head B ON a.Cloth_Idno = b.Cloth_Idno  where a.Weaver_Wages_Code = '" & Trim(EntryCode) & "' and a.Ledger_Idno = " & Str(Val(Led_Idno)), con)
                        da.Fill(dt3)
                        Reed = 0 : pick = 0 : width = 0 : Cloth_TamilName = ""
                        If dt3.Rows.Count > 0 Then
                            Reed = dt3.Rows(0).Item("Cloth_Reed").ToString
                            pick = dt3.Rows(0).Item("Cloth_Pick").ToString
                            width = dt3.Rows(0).Item("Cloth_Width").ToString
                            Cloth_TamilName = dt3.Rows(0).Item("Cloth_Name").ToString
                            'Cloth_TamilName = dt3.Rows(0).Item("Tamil_name").ToString
                        End If
                        dt3.Clear()
                        dt3.Dispose()


                        da = New SqlClient.SqlDataAdapter("select a.* ,b.*, c.* from Mill_Head a LEFT OUTER JOIN Mill_Count_Details B ON  a.Mill_Idno = b.Mill_Idno  LEFT OUTER JOIN Count_Head C on b.Count_Idno = C.Count_Idno  where a.Mill_Idno = " & Val(prn_DetDt1.Rows(prn_DetIndx).Item("Mill_Idno").ToString) & " and  b.Count_Idno =  " & Val(prn_DetDt1.Rows(prn_DetIndx).Item("Count_Idno").ToString), con)
                        da.Fill(dt3)
                        YARN_TamilName = ""
                        If dt3.Rows.Count > 0 Then
                            YARN_TamilName = Replace(UCase(dt3.Rows(0).Item("Count_Name").ToString), "S", "")
                            YARN_TamilName = YARN_TamilName & " " & Trim(dt3.Rows(0).Item("Mill_Name").ToString) & " " & Format(Val(dt3.Rows(0).Item("Weight_Bag").ToString), "#######0") & "KG"
                            'YARN_TamilName = YARN_TamilName & " " & Trim(dt3.Rows(0).Item("Tamil_name").ToString) & " " & Format(Val(dt3.Rows(0).Item("Weight_Bag").ToString), "#######0") & " fpNyh ig"
                        End If
                        dt3.Clear()
                        dt3.Dispose()


                        pFont = New Font("Calibri", 12, FontStyle.Regular)
                        pFont = New Font("Calibri", 10, FontStyle.Regular)

                        If Trim(UCase(prn_DetDt1.Rows(prn_DetIndx).Item("Particulars").ToString)) = "CLOTH" And Trim(UCase(prn_DetDt1.Rows(prn_DetIndx).Item("Tamil_Name").ToString)) = "CLOTHRECEIPT" Then

                            Common_Procedures.Print_To_PrintDocument(e, Reed & "-Reed " & pick & "-Pick " & width & "-Width", LMargin + ClAr(1) + 10, CurY, 0, ClAr(2), pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Reed & "h_L " & pick & "gpfÊF " & width & "mfykÊ", LMargin + ClAr(1) + 10, CurY, 0, ClAr(2), TFont)
                        ElseIf Trim(UCase(prn_DetDt1.Rows(prn_DetIndx).Item("Particulars").ToString)) = "YARN" And Trim(UCase(prn_DetDt1.Rows(prn_DetIndx).Item("Tamil_Name").ToString)) = "YARNRECEIPT" Then
                            Common_Procedures.Print_To_PrintDocument(e, YARN_TamilName, LMargin + ClAr(1) + 2, CurY, 0, ClAr(2), pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt1.Rows(prn_DetIndx).Item("Tamil_Name").ToString, LMargin + ClAr(1) + 2, CurY, 0, ClAr(2), pFont)
                        End If
                        pFont = New Font("Calibri", 11, FontStyle.Regular)

                        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_DetDt1.Rows(prn_DetIndx).Item("noofitems").ToString) <> 0, Format(Val(prn_DetDt1.Rows(prn_DetIndx).Item("noofitems").ToString), "#########0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 2, CurY, 1, ClAr(3), pFont)

                        pFont = New Font("Calibri", 12, FontStyle.Regular)
                        If Val(prn_DetDt1.Rows(prn_DetIndx).Item("PavuDelvMtrs").ToString) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx).Item("PavuDelvMtrs").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, ClAr(4), pFont)
                        If Val(prn_DetDt1.Rows(prn_DetIndx).Item("PavuRecMtrs").ToString) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx).Item("PavuRecMtrs").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, ClAr(4), pFont)
                        If Val(prn_DetDt1.Rows(prn_DetIndx).Item("YarnDelvWgt").ToString) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx).Item("YarnDelvWgt").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, ClAr(4), pFont)
                        If Val(prn_DetDt1.Rows(prn_DetIndx).Item("YarnRecWgt").ToString) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_DetDt1.Rows(prn_DetIndx).Item("YarnRecWgt").ToString)), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, ClAr(4), pFont)

                        '  Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeam").ToString) <> 0, Format(Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeam").ToString), "##########0"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, ClAr(4), pFont)
                        '  Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeam").ToString) <> 0, Format(Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeam").ToString), "##########0"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, ClAr(4), pFont)
                        If Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeamDel").ToString) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeamDel").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, ClAr(4), pFont)
                        If Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeamRec").ToString) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Math.Abs(Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeamRec").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 5, CurY, 1, ClAr(4), pFont)

                        CurY = CurY + 1

                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY + TxtHgt, PageWidth + 15, CurY + TxtHgt)

                        CurY = CurY + 5

                        NoofDets = NoofDets + 1

                        '-----Pavu Total

                        Pavu_Cr = Pavu_Cr + Val(prn_DetDt1.Rows(prn_DetIndx).Item("PavuDelvMtrs").ToString)

                        Pavu_Dr = Pavu_Dr + Math.Abs(Val(prn_DetDt1.Rows(prn_DetIndx).Item("PavuRecMtrs").ToString))

                        '-----Yarn Total

                        Yarn_Cr = Yarn_Cr + Val(prn_DetDt1.Rows(prn_DetIndx).Item("YarnDelvWgt").ToString)

                        Yarn_Dr = Yarn_Dr + Math.Abs(Val(prn_DetDt1.Rows(prn_DetIndx).Item("YarnRecWgt").ToString))

                        '------Beam Total 

                        Beam_Cr = Beam_Cr + Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeamDel").ToString)

                        Beam_Dr = Beam_Dr + Math.Abs(Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeamRec").ToString))

                        '  Beam_Bal = Beam_Bal + Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeam").ToString)

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True, Pavu_Cr, Pavu_Dr, Yarn_Cr, Yarn_Dr, Beam_Cr, Beam_Dr, Led_Id)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        '  e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Led_Id As Integer)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font, TFont As Font, T1Font As Font


        PageNo = PageNo + 1

        CurY = TMargin

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        '  Common_Procedures.Print_To_PrintDocument(e, "Bill No   : " & prn_DetDt.Rows(0).Item("Bill_No").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Cloth_Name").ToString, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 10

        TFont = New Font("Baamini", 13, FontStyle.Bold)
        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "$yp & -", PageWidth - 50, CurY, 1, 0, TFont)

        ' Common_Procedures.Print_To_PrintDocument(e, "Bill Date : " & Format(Convert.ToDateTime(prn_DetDt.Rows(0).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(0).Item("Wages_For_Type1").ToString), "########0.00"), PageWidth - 10, CurY - 3, 1, 0, p1Font)

        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("ClothTamilName").ToString, LMargin, CurY, 2, PrintWidth, TFont)

        CurY = CurY + TxtHgt + 10
        TFont = New Font("Baamini", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "ýngahÊ", LMargin + 10, CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 50, CurY, 0, 0, TFont)
        TFont = New Font("Baamini", 12, FontStyle.Bold)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Weaver_Name").ToString, LMargin + 80, CurY - 2, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Tamil_Name").ToString, LMargin + 80, CurY, 0, 0, TFont)

     
     
        TFont = New Font("Baamini", 12, FontStyle.Regular)
        p1Font = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "Njjp", PageWidth - 205, CurY, 1, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", PageWidth - 195, CurY, 1, 0, TFont)

        If IsDate(Convert.ToDateTime(msk_LedgerFromDate.Text)) = True Then
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(msk_LedgerFromDate.Text), "dd/MM/yyyy"), PageWidth - 110, CurY - 1, 1, 0, p1Font)
        End If
        'Common_Procedures.Print_To_PrintDocument(e, Format(Get_Ledger_Previousdate(prn_DetDt.Rows(0).Item("Ledger_IdNo"), prn_DetDt.Rows(0).Item("Weaver_Wages_Date")), "dd/MM/yyyy"), PageWidth - 110, CurY - 1, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "to", PageWidth - 90, CurY - 3, 1, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Format(prn_DetDt.Rows(0).Item("Weaver_Wages_Date"), "dd/MM/yyyy"), PageWidth - 5, CurY - 1, 1, 0, p1Font)
        If IsDate(Convert.ToDateTime(msk_LedgerToDate.Text)) = True Then
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(msk_LedgerToDate.Text), "dd/MM/yyyy"), PageWidth - 5, CurY - 1, 1, 0, p1Font)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth + 15, CurY)
        LnAr(1) = CurY
        T1Font = New Font("Baamini", 11, FontStyle.Bold)
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Njjp", LMargin, CurY, 2, ClAr(1), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "tpgukÊ", LMargin + ClAr(1), CurY, 2, ClAr(2), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "msT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "ghT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4) + ClAr(5), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Cil", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6) + ClAr(7), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "fhyp gPkÊ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 2, ClAr(9), T1Font)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth + 15, CurY)
        LnAr(2) = CurY

        TFont = New Font("Baamini", 10, FontStyle.Regular)

        CurY = CurY + TxtHgt - 10

        Common_Procedures.Print_To_PrintDocument(e, "tuT kP", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "gwÊW kP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "tuT kP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "gwÊW kP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "tuT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "gwÊW", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 5, CurY, 2, ClAr(9), T1Font)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth + 15, CurY)
        LnAr(3) = CurY


    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal Pavu_Cr As Double, ByVal Pavu_Dr As Double, ByVal yarn_cr As Double, ByVal Yarn_Dr As Double, ByVal Beam_Cr As Double, ByVal Beam_Dr As Double, ByVal Loop_id As Integer)
        Dim p1Font As Font, TFont As Font
        Dim W1 As Single

        'For i = NoofDets + 1 To NoofItems_PerPage
        '    CurY = CurY + TxtHgt
        'Next

        CurY = CurY + TxtHgt - 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY + 1, PageWidth + 15, CurY + 1)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(1))

        CurY = CurY + TxtHgt - 10

        TFont = New Font("Baamini", 10, FontStyle.Regular)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "nkhjÊjkÊ", LMargin + ClAr(1), CurY, 0, ClAr(4), TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 100, CurY, 1, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Pavu_Cr) <> 0, Format(Val(Pavu_Cr), "#########0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Pavu_Dr) <> 0, Format(Val(Pavu_Dr), "#########0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(yarn_cr) <> 0, Format(Val(yarn_cr), "#########0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Yarn_Dr) <> 0, Format(Val(Yarn_Dr), "#########0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, p1Font)

        ' Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Beam_Count) <> 0, Format(Val(Beam_Count), "#########0"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Beam_Cr) <> 0, Val(Beam_Cr), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Beam_Dr) <> 0, Val(Beam_Dr), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 5, CurY, 1, 0, p1Font)

        Pavu_Cr = Pavu_Dr - Pavu_Cr
        yarn_cr = Yarn_Dr - yarn_cr

        Beam_Cr = Beam_Dr - Beam_Cr

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth + 15, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "kPjp ifapUgÊG", LMargin + ClAr(1), CurY, 0, ClAr(4), TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 100, CurY, 1, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, "epfughT kP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Pavu_Cr, "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "epfuCil kP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(yarn_cr, "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "kPjp", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, Format(Beam_Cr, "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 5, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth + 15, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(1))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(1))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(1))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(1))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(1))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 17, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 17, LnAr(1))

        ' CurY = CurY + 10

        '----GETTING  AMOUNT DETIALS


        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        W1 = 150
        CurY = CurY + TxtHgt
        prn_pos = 0

        Common_Procedures.Print_To_PrintDocument(e, "$yp", LMargin + 10, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 - 20, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 1), LMargin + W1 + 80, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "nkhjÊj ghfÊfp", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 - 20, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 80, CurY, 1, 0, pFont)

        If Val(prn_DetAr_Ledger(prn_pos, Loop_id, 5)) <> 0 Then
            p1Font = New Font("Calibri", 11, FontStyle.Regular)
            e.Graphics.DrawLine(Pens.Black, LMargin + 290, CurY - 8, 500, CurY - 8)
            e.Graphics.DrawLine(Pens.Black, LMargin + 290, CurY - 5, 500, CurY - 5)

            Common_Procedures.Print_To_PrintDocument(e, "Less Meter :  " & prn_DetAr_Ledger(prn_pos, Loop_id, 5), LMargin + 300, CurY, 0, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + 290, CurY + TxtHgt, 500, CurY + TxtHgt)
            e.Graphics.DrawLine(Pens.Black, LMargin + 290, CurY + TxtHgt + 3, 500, CurY + TxtHgt + 3)

            e.Graphics.DrawLine(Pens.Black, LMargin + 290, CurY + TxtHgt + 3, LMargin + 290, CurY - 8)
            e.Graphics.DrawLine(Pens.Black, 500, CurY + TxtHgt + 3, 500, CurY - 8)

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
        End If

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "ghfÊfp gpbjÊjkÊ ", LMargin + 10, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, ": (-) ", LMargin + W1 - 20, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 2), LMargin + W1 + 80, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "ghfÊfp gpbjÊjkÊ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 - 20, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 80, CurY, 1, 0, pFont)

        'CurY = CurY + TxtHgt

        'Common_Procedures.Print_To_PrintDocument(e, "tup gpbjÊjkÊ ", LMargin + 10, CurY, 0, 0, TFont)
        'Common_Procedures.Print_To_PrintDocument(e, ": (-)", LMargin + W1 - 20, CurY, 0, 0, TFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 7), LMargin + W1 + 80, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + W1 + 10, CurY, LMargin + W1 + 80, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 80, CurY)


        CurY = CurY + TxtHgt - 10
        ' TFont = New Font("Baamini", 10, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "epfu $yp", LMargin + 10, CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 - 20, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 3), LMargin + W1 + 80, CurY, 1, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "kPjp ghfÊfpnjif", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 - 20, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr_Ledger(prn_pos, Loop_id, 4)) - Val(prn_DetAr_Ledger(prn_pos, Loop_id, 2)), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 80, CurY, 1, 0, p1Font)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + W1 + 10, CurY, LMargin + W1 + 80, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 80, CurY)

        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "epfu $yp &", LMargin + 10, CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 3), LMargin + 95, CurY - 2, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "ngwÊWfÊnfhzÊNldÊ", LMargin + 160, CurY, 0, 0, TFont)

    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal Led_Id As Integer, ByVal pos As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt3 As New DataTable
        Dim pFont As Font, TFont As Font
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
        Dim Pavu_Dr As Double = 0
        Dim Pavu_Cr As Double = 0
        Dim Yarn_Dr As Double = 0
        Dim Yarn_Cr As Double = 0

        Dim Beam_Dr As Integer = 0
        Dim Beam_Cr As Integer = 0

        Dim Beam_Bal As Integer = 0
        Dim Reed As Double = 0
        Dim pick As Double = 0
        Dim width As Double = 0
        Dim Led_Idno As Integer = 0
        Dim Cloth_TamilName As String = ""
        Dim YARN_TamilName As String = ""

        If PpSzSTS = False Then
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
            .Left = 25
            .Right = 60
            .Top = 30
            .Bottom = 30
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
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(75) : ClAr(2) = 200 : ClAr(3) = 50 : ClAr(4) = 80 : ClAr(5) = 80 : ClAr(6) = 90 : ClAr(7) = 90 : ClAr(8) = 50
        ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, Led_Id)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt1.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt1.Rows.Count - 1

                        CurY = CurY + TxtHgt
                        pFont = New Font("Calibri", 10, FontStyle.Regular)
                        TFont = New Font("Baamini", 10, FontStyle.Regular)

                        If IsDate(prn_DetDt1.Rows(prn_DetIndx).Item("Date1")) = True Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(prn_DetDt1.Rows(prn_DetIndx).Item("Date1"), "dd/MM/yyyy"), LMargin, CurY, 2, ClAr(1), pFont)
                        Else
                            If prn_DetDt1.Rows.Count > 1 Then
                                If IsDate(prn_DetDt1.Rows(prn_DetIndx + 1).Item("Date1")) = True Then
                                    'If Trim(UCase(prn_DetDt1.Rows(prn_DetIndx).Item("Particulars").ToString)) = "PAVU" Then
                                    Common_Procedures.Print_To_PrintDocument(e, Format(prn_DetDt1.Rows(prn_DetIndx + 1).Item("Date1"), "dd/MM/yyyy"), LMargin, CurY, 2, ClAr(1), pFont)
                                End If
                            End If

                        End If

                        Led_Idno = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Idno", "Ledger_Name = '" & Trim(UCase(prn_DetDt1.Rows(prn_DetIndx).Item("Led_Name").ToString)) & "'"))

                        da = New SqlClient.SqlDataAdapter("select b.* from Weaver_Wages_Details a INNER JOIN Cloth_Head B ON a.Cloth_Idno = b.Cloth_Idno  where a.Weaver_Wages_Code = '" & Trim(EntryCode) & "' and a.Ledger_Idno = " & Str(Val(Led_Idno)), con)
                        da.Fill(dt3)
                        Reed = 0 : pick = 0 : width = 0 : Cloth_TamilName = ""
                        If dt3.Rows.Count > 0 Then
                            Reed = dt3.Rows(0).Item("Cloth_Reed").ToString
                            pick = dt3.Rows(0).Item("Cloth_Pick").ToString
                            width = dt3.Rows(0).Item("Cloth_Width").ToString
                            Cloth_TamilName = dt3.Rows(0).Item("Cloth_Name").ToString
                            'Cloth_TamilName = dt3.Rows(0).Item("Tamil_name").ToString
                        End If
                        dt3.Clear()
                        dt3.Dispose()


                        da = New SqlClient.SqlDataAdapter("select a.* ,b.*, c.* from Mill_Head a LEFT OUTER JOIN Mill_Count_Details B ON  a.Mill_Idno = b.Mill_Idno  LEFT OUTER JOIN Count_Head C on b.Count_Idno = C.Count_Idno  where a.Mill_Idno = " & Val(prn_DetDt1.Rows(prn_DetIndx).Item("Mill_Idno").ToString) & " and  b.Count_Idno =  " & Val(prn_DetDt1.Rows(prn_DetIndx).Item("Count_Idno").ToString), con)
                        da.Fill(dt3)
                        YARN_TamilName = ""
                        If dt3.Rows.Count > 0 Then
                            YARN_TamilName = Replace(UCase(dt3.Rows(0).Item("Count_Name").ToString), "S", "")
                            YARN_TamilName = YARN_TamilName & " " & Trim(dt3.Rows(0).Item("Mill_Name").ToString) & " " & Format(Val(dt3.Rows(0).Item("Weight_Bag").ToString), "#######0") & "KG"
                            'YARN_TamilName = YARN_TamilName & " " & Trim(dt3.Rows(0).Item("Tamil_name").ToString) & " " & Format(Val(dt3.Rows(0).Item("Weight_Bag").ToString), "#######0") & " fpNyh ig"
                        End If
                        dt3.Clear()
                        dt3.Dispose()


                        pFont = New Font("Calibri", 12, FontStyle.Regular)
                        pFont = New Font("Calibri", 10, FontStyle.Regular)

                        If Trim(UCase(prn_DetDt1.Rows(prn_DetIndx).Item("Particulars").ToString)) = "CLOTH" And Trim(UCase(prn_DetDt1.Rows(prn_DetIndx).Item("Tamil_Name").ToString)) = "CLOTHRECEIPT" Then

                            Common_Procedures.Print_To_PrintDocument(e, Reed & "-Reed " & pick & "-Pick " & width & "-Width", LMargin + ClAr(1) + 10, CurY, 0, ClAr(2), pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Reed & "h_L " & pick & "gpfÊF " & width & "mfykÊ", LMargin + ClAr(1) + 10, CurY, 0, ClAr(2), TFont)
                        ElseIf Trim(UCase(prn_DetDt1.Rows(prn_DetIndx).Item("Particulars").ToString)) = "YARN" And Trim(UCase(prn_DetDt1.Rows(prn_DetIndx).Item("Tamil_Name").ToString)) = "YARNRECEIPT" Then
                            Common_Procedures.Print_To_PrintDocument(e, YARN_TamilName, LMargin + ClAr(1) + 2, CurY, 0, ClAr(2), pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt1.Rows(prn_DetIndx).Item("Tamil_Name").ToString, LMargin + ClAr(1) + 2, CurY, 0, ClAr(2), pFont)
                        End If
                        pFont = New Font("Calibri", 11, FontStyle.Regular)

                        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_DetDt1.Rows(prn_DetIndx).Item("noofitems").ToString) <> 0, Format(Val(prn_DetDt1.Rows(prn_DetIndx).Item("noofitems").ToString), "#########0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 2, CurY, 1, ClAr(3), pFont)

                        pFont = New Font("Calibri", 12, FontStyle.Regular)
                        If Val(prn_DetDt1.Rows(prn_DetIndx).Item("PavuDelvMtrs").ToString) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx).Item("PavuDelvMtrs").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, ClAr(4), pFont)
                        If Val(prn_DetDt1.Rows(prn_DetIndx).Item("PavuRecMtrs").ToString) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx).Item("PavuRecMtrs").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, ClAr(4), pFont)
                        If Val(prn_DetDt1.Rows(prn_DetIndx).Item("YarnDelvWgt").ToString) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx).Item("YarnDelvWgt").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, ClAr(4), pFont)
                        If Val(prn_DetDt1.Rows(prn_DetIndx).Item("YarnRecWgt").ToString) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_DetDt1.Rows(prn_DetIndx).Item("YarnRecWgt").ToString)), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, ClAr(4), pFont)

                        '  Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeam").ToString) <> 0, Format(Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeam").ToString), "##########0"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, ClAr(4), pFont)
                        '  Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeam").ToString) <> 0, Format(Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeam").ToString), "##########0"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, ClAr(4), pFont)
                        If Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeamDel").ToString) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeamDel").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, ClAr(4), pFont)
                        If Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeamRec").ToString) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Math.Abs(Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeamRec").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 5, CurY, 1, ClAr(4), pFont)

                        CurY = CurY + 1

                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY + TxtHgt, PageWidth + 15, CurY + TxtHgt)

                        CurY = CurY + 5

                        NoofDets = NoofDets + 1

                        '-----Pavu Total

                        Pavu_Cr = Pavu_Cr + Val(prn_DetDt1.Rows(prn_DetIndx).Item("PavuDelvMtrs").ToString)

                        Pavu_Dr = Pavu_Dr + Math.Abs(Val(prn_DetDt1.Rows(prn_DetIndx).Item("PavuRecMtrs").ToString))

                        '-----Yarn Total

                        Yarn_Cr = Yarn_Cr + Val(prn_DetDt1.Rows(prn_DetIndx).Item("YarnDelvWgt").ToString)

                        Yarn_Dr = Yarn_Dr + Math.Abs(Val(prn_DetDt1.Rows(prn_DetIndx).Item("YarnRecWgt").ToString))

                        '------Beam Total 

                        Beam_Cr = Beam_Cr + Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeamDel").ToString)

                        Beam_Dr = Beam_Dr + Math.Abs(Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeamRec").ToString))

                        '  Beam_Bal = Beam_Bal + Val(prn_DetDt1.Rows(prn_DetIndx).Item("EmptyBeam").ToString)

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True, Pavu_Cr, Pavu_Dr, Yarn_Cr, Yarn_Dr, Beam_Cr, Beam_Dr, Led_Id)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        '  e.HasMorePages = False

    End Sub

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Led_Id As Integer)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font, TFont As Font, T1Font As Font


        PageNo = PageNo + 1

        CurY = TMargin

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Bill No   : " & prn_DetDt.Rows(0).Item("Bill_No").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        '  Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Cloth_Name").ToString, LMargin, CurY, 2, PrintWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Weaver_Name").ToString, LMargin, CurY, 2, PrintWidth, p1Font)

        Dim gstno As String = ""

        If Trim(prn_DetDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            gstno = "GSTIN : " & Trim(prn_DetDt.Rows(0).Item("Ledger_GSTinNo").ToString)
        ElseIf Trim(prn_DetDt.Rows(0).Item("Pan_No").ToString) <> "" Then
            gstno = "PAN NO : " & Trim(prn_DetDt.Rows(0).Item("Pan_No").ToString)
        End If
        Common_Procedures.Print_To_PrintDocument(e, gstno, PageWidth - 5, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        TFont = New Font("Baamini", 13, FontStyle.Bold)
        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "$yp & -", PageWidth - 50, CurY, 1, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, "Bill Date : " & Format(Convert.ToDateTime(prn_DetDt.Rows(0).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(0).Item("Wages_For_Type1").ToString), "########0.00"), PageWidth - 10, CurY - 3, 1, 0, p1Font)

        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("ClothTamilName").ToString, LMargin, CurY, 2, PrintWidth, TFont)

        CurY = CurY + TxtHgt + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth + 15, CurY)

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "To : " & prn_DetDt.Rows(0).Item("Company_name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "GSTIN " & prn_DetDt.Rows(0).Item("Company_GSTinNo").ToString, PageWidth - 5, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth + 15, CurY)

        CurY = CurY + TxtHgt - 10
        TFont = New Font("Baamini", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "Cloth", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 50, CurY, 0, 0, p1Font)
        TFont = New Font("Baamini", 12, FontStyle.Bold)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        '  Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Weaver_Name").ToString, LMargin + 80, CurY - 2, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Cloth_Name").ToString, LMargin + 80, CurY, 0, 0, p1Font)


        TFont = New Font("Baamini", 12, FontStyle.Regular)
        p1Font = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "Njjp", PageWidth - 205, CurY, 1, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", PageWidth - 195, CurY, 1, 0, TFont)

        If IsDate(Convert.ToDateTime(msk_LedgerFromDate.Text)) = True Then
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(msk_LedgerFromDate.Text), "dd/MM/yyyy"), PageWidth - 110, CurY - 1, 1, 0, p1Font)
        End If
        'Common_Procedures.Print_To_PrintDocument(e, Format(Get_Ledger_Previousdate(prn_DetDt.Rows(0).Item("Ledger_IdNo"), prn_DetDt.Rows(0).Item("Weaver_Wages_Date")), "dd/MM/yyyy"), PageWidth - 110, CurY - 1, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "to", PageWidth - 90, CurY - 3, 1, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Format(prn_DetDt.Rows(0).Item("Weaver_Wages_Date"), "dd/MM/yyyy"), PageWidth - 5, CurY - 1, 1, 0, p1Font)
        If IsDate(Convert.ToDateTime(msk_LedgerToDate.Text)) = True Then
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(msk_LedgerToDate.Text), "dd/MM/yyyy"), PageWidth - 5, CurY - 1, 1, 0, p1Font)
        End If

        CurY = CurY + TxtHgt + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth + 15, CurY)
        LnAr(1) = CurY
        T1Font = New Font("Baamini", 11, FontStyle.Bold)
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Njjp", LMargin, CurY, 2, ClAr(1), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "tpgukÊ", LMargin + ClAr(1), CurY, 2, ClAr(2), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "msT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "ghT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4) + ClAr(5), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Cil", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6) + ClAr(7), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "fhyp gPkÊ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 2, ClAr(9), T1Font)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth + 15, CurY)
        LnAr(2) = CurY

        TFont = New Font("Baamini", 10, FontStyle.Regular)

        CurY = CurY + TxtHgt - 10

        Common_Procedures.Print_To_PrintDocument(e, "tuT kP", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "gwÊW kP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "tuT kP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "gwÊW kP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "tuT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), T1Font)
        Common_Procedures.Print_To_PrintDocument(e, "gwÊW", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 5, CurY, 2, ClAr(9), T1Font)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth + 15, CurY)
        LnAr(3) = CurY


    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal Pavu_Cr As Double, ByVal Pavu_Dr As Double, ByVal yarn_cr As Double, ByVal Yarn_Dr As Double, ByVal Beam_Cr As Double, ByVal Beam_Dr As Double, ByVal Loop_id As Integer)
        Dim p1Font As Font, TFont As Font
        Dim W1 As Single

        'For i = NoofDets + 1 To NoofItems_PerPage
        '    CurY = CurY + TxtHgt
        'Next

        CurY = CurY + TxtHgt - 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY + 1, PageWidth + 15, CurY + 1)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(1))

        CurY = CurY + TxtHgt - 10

        TFont = New Font("Baamini", 10, FontStyle.Regular)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "nkhjÊjkÊ", LMargin + ClAr(1), CurY, 0, ClAr(4), TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 100, CurY, 1, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Pavu_Cr) <> 0, Format(Val(Pavu_Cr), "#########0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Pavu_Dr) <> 0, Format(Val(Pavu_Dr), "#########0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(yarn_cr) <> 0, Format(Val(yarn_cr), "#########0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Yarn_Dr) <> 0, Format(Val(Yarn_Dr), "#########0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, p1Font)

        ' Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Beam_Count) <> 0, Format(Val(Beam_Count), "#########0"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Beam_Cr) <> 0, Val(Beam_Cr), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Beam_Dr) <> 0, Val(Beam_Dr), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 5, CurY, 1, 0, p1Font)

        Pavu_Cr = Pavu_Dr - Pavu_Cr
        yarn_cr = Yarn_Dr - yarn_cr

        Beam_Cr = Beam_Dr - Beam_Cr

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth + 15, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "kPjp ifapUgÊG", LMargin + ClAr(1), CurY, 0, ClAr(4), TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 100, CurY, 1, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, "epfughT kP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Pavu_Cr, "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "epfuCil kP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(yarn_cr, "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "kPjp", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, Format(Beam_Cr, "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 5, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth + 15, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(1))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(1))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(1))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(1))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(1))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 17, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 17, LnAr(1))

        ' CurY = CurY + 10

        '----GETTING  AMOUNT DETIALS


        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        W1 = 150
        CurY = CurY + TxtHgt
        prn_pos = 0

        Common_Procedures.Print_To_PrintDocument(e, "$yp", LMargin + 10, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 - 20, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 1), LMargin + W1 + 80, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "nkhjÊj ghfÊfp", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 - 20, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 80, CurY, 1, 0, pFont)

        If Val(prn_DetAr_Ledger(prn_pos, Loop_id, 5)) <> 0 Then
            p1Font = New Font("Calibri", 11, FontStyle.Regular)
            e.Graphics.DrawLine(Pens.Black, LMargin + 290, CurY - 8, 500, CurY - 8)
            e.Graphics.DrawLine(Pens.Black, LMargin + 290, CurY - 5, 500, CurY - 5)

            Common_Procedures.Print_To_PrintDocument(e, "Less Meter :  " & prn_DetAr_Ledger(prn_pos, Loop_id, 5), LMargin + 300, CurY, 0, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + 290, CurY + TxtHgt, 500, CurY + TxtHgt)
            e.Graphics.DrawLine(Pens.Black, LMargin + 290, CurY + TxtHgt + 3, 500, CurY + TxtHgt + 3)

            e.Graphics.DrawLine(Pens.Black, LMargin + 290, CurY + TxtHgt + 3, LMargin + 290, CurY - 8)
            e.Graphics.DrawLine(Pens.Black, 500, CurY + TxtHgt + 3, 500, CurY - 8)

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
        End If

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "ghfÊfp gpbjÊjkÊ ", LMargin + 10, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, ": (-) ", LMargin + W1 - 20, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 2), LMargin + W1 + 80, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "ghfÊfp gpbjÊjkÊ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 - 20, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 80, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "tup gpbjÊjkÊ ", LMargin + 10, CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, ": (-)", LMargin + W1 - 20, CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 7), LMargin + W1 + 80, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + W1 + 10, CurY, LMargin + W1 + 80, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 80, CurY)


        CurY = CurY + TxtHgt - 10
        ' TFont = New Font("Baamini", 10, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "epfu $yp", LMargin + 10, CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 - 20, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 3), LMargin + W1 + 80, CurY, 1, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "kPjp ghfÊfpnjif", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 - 20, CurY, 0, 0, TFont)

        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr_Ledger(prn_pos, Loop_id, 4)) - Val(prn_DetAr_Ledger(prn_pos, Loop_id, 2)), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 80, CurY, 1, 0, p1Font)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + W1 + 10, CurY, LMargin + W1 + 80, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + W1 + 80, CurY)

        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "epfu $yp &", LMargin + 10, CurY, 0, 0, TFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr_Ledger(prn_pos, Loop_id, 3), LMargin + 95, CurY - 2, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "ngwÊWfÊnfhzÊNldÊ", LMargin + 160, CurY, 0, 0, TFont)

    End Sub
    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_TransportName.Focus()
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

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_TransportName.Focus()
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

    Private Sub cbo_Grid_WeaverName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_WeaverName.LostFocus
        Try
            If Trim(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value) <> "" Then

                dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(4).Value = 40
            End If
        Catch ex As Exception

        End Try
     
    End Sub

    Private Sub cbo_DelvTo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_WeaverName.TextChanged
        Try
            If cbo_Grid_WeaverName.Visible Then


                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_WeaverName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_WeaverName.Text)

                    End If
                End With
            End If

        Catch ex As Exception


        End Try
    End Sub

    Private Sub txt_Party_DcNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Party_DcNo.KeyDown
        If (e.KeyValue = 40) Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
        If (e.KeyValue = 38) Then
            lbl_Frieght.Focus()
        End If
    End Sub


    Private Sub txt_Party_DcNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Party_DcNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub cbo_Grid_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")

    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    txt_Party_DcNo.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                End If
            End If
            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save the details", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.OK Then
                        save_record()
                    Else
                        msk_date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With
        End If
    End Sub

    Private Sub cbo_Grid_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try
            If cbo_Grid_ClothName.Visible Then


                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception


        End Try
    End Sub

    Private Sub Total_Calculation()
        Dim TotPcs As Single, TotRecMtrs As Single, TotAmt As Single
        Dim TotNetMtrs As Single, TotLessMtrs As Single, TotAdvAmt As Single, TotNetAmt As Single
        Dim vTotCGST As Single, vTotSGST As Single, vTotTDS As Single, vTotASSEBLE As Single

        TotPcs = 0 : TotRecMtrs = 0 : TotLessMtrs = 0 : TotNetMtrs = 0
        TotAmt = 0 : TotAdvAmt = 0 : TotNetAmt = 0
        vTotCGST = 0 : vTotSGST = 0 : vTotTDS = 0 : vTotASSEBLE = 0

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(3).Value) <> 0 Then
                    TotPcs = TotPcs + Val(.Rows(i).Cells(3).Value)
                    TotRecMtrs = TotRecMtrs + Val(.Rows(i).Cells(5).Value)
                    TotLessMtrs = TotLessMtrs + Val(.Rows(i).Cells(6).Value)
                    TotNetMtrs = TotNetMtrs + Val(.Rows(i).Cells(7).Value)
                    TotAmt = TotAmt + Val(.Rows(i).Cells(8).Value)

                    vTotCGST = vTotCGST + Val(.Rows(i).Cells(9).Value)
                    vTotSGST = vTotSGST + Val(.Rows(i).Cells(10).Value)
                    vTotTDS = vTotTDS + Val(.Rows(i).Cells(11).Value)
                    vTotASSEBLE = vTotASSEBLE + Val(.Rows(i).Cells(12).Value)

                    TotAdvAmt = TotAdvAmt + Val(.Rows(i).Cells(13).Value)
                    TotNetAmt = TotNetAmt + Val(.Rows(i).Cells(14).Value)
                End If
            Next

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Val(TotPcs)
            .Rows(0).Cells(5).Value = Format(Val(TotRecMtrs), "########0.000")
            .Rows(0).Cells(6).Value = Format(Val(TotLessMtrs), "########0.000")
            .Rows(0).Cells(7).Value = Format(Val(TotNetMtrs), "########0.000")
            .Rows(0).Cells(8).Value = Format(Val(TotAmt), "########0.00")

            .Rows(0).Cells(9).Value = Format(Val(vTotCGST), "########0.00")
            .Rows(0).Cells(10).Value = Format(Val(vTotSGST), "########0.00")

            .Rows(0).Cells(11).Value = Format(Val(vTotTDS), "########0.00")
            .Rows(0).Cells(12).Value = Format(Val(vTotASSEBLE), "########0.00")

            .Rows(0).Cells(13).Value = Format(Val(TotAdvAmt), "########0.00")
            .Rows(0).Cells(14).Value = Format(Val(TotNetAmt), "########0.00")

        End With

        lbl_Frieght.Text = Format(Val(txt_NofoKattu.Text) * Val(txt_RatePerKattu.Text), "#######0.00")
    End Sub

    Private Sub btn_Choolie_Chit_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Choolie_Chit_Print.Click
        Dim Wev_Id As Integer = 0
        Prn_Sql_Conditon_Multi = ""


        If Trim(cbo_Multi_WeaverName.Text) <> "" Then

            If InStr(Trim(UCase(cbo_Multi_WeaverName.Text)), "SELECTED") > 0 Then

                Prn_Sql_Conditon_Multi = "and a.Ledger_IdNo IN (" & RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedIdNos_AsString & ")"

            Else
                Wev_Id = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Multi_WeaverName.Text)))
                Prn_Sql_Conditon_Multi = "and a.Ledger_IdNo = " & Wev_Id

            End If
        End If

        print_Format = "FORMAT-1"
        Printing_Invoice()

        btn_Close_PrintSelection_Click(sender, e)
    End Sub

    Private Sub btn_LedgerPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_LedgerPrint.Click
        Dim Wev_Id As Integer = 0

        Prn_Sql_Conditon_Multi = ""

        If Trim(cbo_Multi_WeaverName.Text) <> "" Then

            If InStr(Trim(UCase(cbo_Multi_WeaverName.Text)), "SELECTED") > 0 Then

                Prn_Sql_Conditon_Multi = RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedIdNos_AsString

            Else

                Wev_Id = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Multi_WeaverName.Text)))

                Prn_Sql_Conditon_Multi = Wev_Id
            End If
        End If

        print_Format = "FORMAT-2"
        Printing_Invoice()
        btn_Close_PrintSelection_Click(sender, e)

    End Sub

    Private Sub btn_Coolie_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CoolierRegister_Print.Click
        Dim NewCode As String
        Dim f As New Report_Details

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Weaver Cloth Inward Coolie Register"
        Common_Procedures.RptInputDet.ReportHeading = "Weaver Cloth Inward Coolie Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT"
        Common_Procedures.RptInputDet.Name1 = NewCode
        Common_Procedures.RptInputDet.Date1 = dtp_Date.Value
        f.MdiParent = MDIParent1
        f.Show()
        f.dtp_FromDate.Text = dtp_Date.Text
        Pnl_PrintSelection.Visible = False
        Pnl_Back.Enabled = True

        'print_Format = "FORMAT-3"
        'Printing_Invoice()
        'btn_Close_PrintSelection_Click(sender, e)
    End Sub

    Private Sub btn_Close_PrintSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_PrintSelection.Click
        Pnl_PrintSelection.Visible = False
        Pnl_Back.Enabled = True
        cbo_Multi_WeaverName.Text = ""

    End Sub

    Public Sub Printing_Invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize



        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Wages_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'", con)


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

        prn_TotCopies = 1


        If PpSzSTS = False Then

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


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        If PpSzSTS = False Then

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

    Private Sub Weaver_AllStock_Ledger(ByVal Led_IdNo As String, ByVal Wages_Date As Date)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vDate_To As Date, vDate_From As Date
        Dim CompIDCondt As String
        Dim SqlCondt As String = ""
        Dim NewCode As String
        Dim Nr As Integer = 0

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@WeaWageDate", Wages_Date.Date)

        If IsDate(msk_LedgerFromDate.Text) = True Then
            vDate_From = Convert.ToDateTime(msk_LedgerFromDate.Text)
        Else
            vDate_From = Common_Procedures.Company_FromDate
        End If

        If IsDate(Convert.ToDateTime(msk_LedgerToDate.Text)) = True Then
            vDate_To = msk_LedgerToDate.Text
        Else
            vDate_To = Wages_Date
        End If

        ' vDate_To = DateAdd("d", 10, vDate_To.Date)


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'cmd.CommandText = "select max(a.weaver_wages_date) AS WagesDate from Weaver_Wages_Head a LEFT OUTER JOIN Weaver_Wages_Details B ON A.Weaver_Wages_Code = b.Weaver_Wages_Code Where " & Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & " b.Ledger_IdNo = " & Led_IdNo & " and  a.Weaver_Wages_Code <> '" & Trim(NewCode) & "'  and a.Weaver_Wages_Date < @WeaWageDate"
        'Da1 = New SqlClient.SqlDataAdapter(cmd)
        'Dt1 = New DataTable
        'Da1.Fill(Dt1)

        'If Dt1.Rows.Count > 0 Then

        '    If IsDBNull(Dt1.Rows(0).Item("WagesDate").ToString) = False Then

        '        If IsDate(Dt1.Rows(0).Item("WagesDate").ToString) = True Then
        '            vDate_From = Dt1.Rows(0).Item("WagesDate").ToString
        '            vDate_From = DateAdd("d", 1, vDate_From.Date)
        '        End If

        '    End If

        'End If

        'Dt1.Clear()

        cmd.Connection = con

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@WeaWageDate", Wages_Date.Date)
        cmd.Parameters.AddWithValue("@fromdate", vDate_From.Date)
        cmd.Parameters.AddWithValue("@todate", vDate_To.Date)

        SqlCondt = Trim(CompIDCondt)
        If Trim(Led_IdNo) <> "" Then

            SqlCondt = Trim(SqlCondt) & IIf(Trim(SqlCondt) <> "", " and ", "") & "tP.Ledger_IdNo = " & Led_IdNo

        End If

        '-------- Empty Beam,  Empty Bag,  Empty Cone
        '-------------------
        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()


        ''cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1          ,int2              ) " & _
        ''                  "Select -1*(a.Empty_Beam+a.Pavu_Beam)  ,a.DeliveryTo_Idno from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0 )"
        ''cmd.ExecuteNonQuery()
        ''cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1              ,int2) " & _
        ''                          "Select (a.Empty_Beam+Pavu_Beam) ,a.ReceivedFrom_Idno  from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0)"
        ''cmd.ExecuteNonQuery()

        ''cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "( int3, name5    , name6 , Int6) " & _
        ''                                 " Select  0   , 'Opening', 'BEAM', sum(Int1) from " & Trim(Common_Procedures.EntryTempSubTable) & " group by  int2 having sum(Int1) <> 0"
        ''cmd.ExecuteNonQuery()


        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1 ) Select (a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0 )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1) Select -1*(a.Empty_Beam+Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, name5,  Int7 , Int6) Select 0, 'Opening',  (case when sum(Int1) > 0 then sum(Int1) else 0 end), (case when sum(Int1) < 0 then abs(sum(Int1)) else 0 end) from " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()


        ''cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, name5,  Int6) Select 0, 'Opening', sum(Int1) from " & Trim(Common_Procedures.ReportTempSubTable) & " having sum(Int1) <> 0"
        ''Nr = cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1           , name1           , name2         , meters1      , name3          , name4         , name5        , name6       ,     Int6      , Int7       ,Name10               ) " & _
                                       " Select   1  , a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'PAVU'  ,      0            ,abs((a.Empty_Beam+a.Pavu_Beam)) , tR.Ledger_Name   from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 AND a.DeliveryTo_Idno = tP.Ledger_IdNo LEFT OUTER JOIN Ledger_Head tR ON a.ReceivedFrom_Idno = tR.Ledger_IdNo  Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and (a.Empty_Beam+a.Pavu_Beam) <> 0 "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1           , name1           , name2         , meters1      , name3          , name4         , name5        , name6            , Int6           , Int7   , Name10             ) " & _
                                                 "Select 2 , a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'EMPTYBEAM',   abs((a.Empty_Beam+a.Pavu_Beam))   ,   0         , 'BEAMRECEIPT'    from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 AND a.ReceivedFrom_Idno = tP.Ledger_IdNo  Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate  and (a.Empty_Beam+a.Pavu_Beam) <> 0 "
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int3,  Int6) Select 1,  (a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0 )"
        'cmd.ExecuteNonQuery()
        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int3,  Int6) Select 2,  -1*abs(a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0)"
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Int6 ) Select 0, sum(Int6) from " & Trim(Common_Procedures.ReportTempSubTable) & " "
        'cmd.ExecuteNonQuery()


        '-------- Pavu 

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(name1         , Meters10         , meters1) " & _
                                        "  Select c.endscount_name ,a.Sized_Beam   , -1*a.Meters from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = c.EndsCount_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and a.Meters <> 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(name1   ,Meters10           , meters1) " & _
                                   "Select c.endscount_name  ,-1 *a.Sized_Beam, a.Meters from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = c.EndsCount_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and a.Meters <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, name5      , meters6                                                   , meters7) " & _
                                               "Select 0, 'Opening',    (case when sum(meters1) > 0 then sum(meters1) else 0 end), (case when sum(meters1) < 0 then abs(sum(meters1)) else 0 end) from " & Trim(Common_Procedures.ReportTempSubTable) & " " ' group by name1 having sum(meters1) <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1           , name1           , name2         , meters1      , name3          , name4         , name5        , name6 , name7           , Meters10            , meters6      , meters7          ,Name10) " & _
                                            " Select 1, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'PAVU', c.endscount_name, abs(a.Sized_Beam),       0      ,   abs(a.Meters)  ,tPP.Ledger_Name  from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 AND a.DeliveryTo_Idno = tP.Ledger_IdNo  LEFT OUTER JOIN Ledger_Head tPP ON a.ReceivedFromIdno_ForParticulars = tPP.Ledger_IdNo  INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Meters <> 0 "
        Nr = cmd.ExecuteNonQuery()

        'pavu receipt
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1           , name1           , name2         , meters1      , name3          , name4         , name5       , name6   , name7            , Meters10    , meters6         ,  meters7    , Name10) " & _
                                            "Select 2 , a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'PAVU' , c.endscount_name , wr.Rcpt_Pcs       ,  abs(a.Meters)  ,      0      , 'PAVU RETURN'  from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 AND a.ReceivedFrom_Idno = tP.Ledger_IdNo LEFT OUTER JOIN Ledger_Head tPP ON a.DeliveryToIdno_ForParticulars  = tPP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo INNER JOIN Weaver_KuraiPavu_Receipt_Details wr ON 'KPVRC-' + wr.Weaver_KuraiPavu_Receipt_Code = a.Reference_Code Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Meters <> 0 AND  a.Reference_Code NOT LIKE '" & Trim(Pk_Condition) & "%' "
        cmd.ExecuteNonQuery()

        'cloth receipt
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1           , name1           , name2         , meters1      , name3          , name4         , name5       , name6   , name7           , Meters10              , meters6       , meters7 ,  Name10     ) " & _
                                            "Select 2 , a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'CLOTH', c.endscount_name,abs(a.Sized_Beam),abs(a.Meters)  ,  0       , 'CLOTHRECEIPT' from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 AND a.ReceivedFrom_Idno = tP.Ledger_IdNo LEFT OUTER JOIN Ledger_Head tPP ON a.DeliveryToIdno_ForParticulars  = tPP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Meters <> 0 AND  a.Reference_Code LIKE '" & Trim(Pk_Condition) & "%' "
        cmd.ExecuteNonQuery()

        '-------- Yarn

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(name1      , Meters10     , weight1) " & _
                                         " Select c.count_name  , a.Bags    , -1*a.Weight from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo <> 0 and a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and a.Weight <> 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(name1      , Meters10     , weight1) " & _
                                           "Select c.count_name ,-1*a.Bags , a.Weight from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo <> 0 and a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and a.Weight <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, name5, name7, weight1, weight2) Select 0, 'Opening', name1,  (case when sum(Weight1) > 0 then sum(Weight1) else 0 end), (case when sum(Weight1) < 0 then abs(sum(Weight1)) else 0 end) from " & Trim(Common_Procedures.ReportTempSubTable) & "  group by name1 having sum(Weight1) <> 0"
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, name5, name7, Meters10 , weight1, weight2) Select 0, 'Opening', name1, sum(Meters10), (case when sum(Weight1) > 0 then sum(Weight1) else 0 end), (case when sum(Weight1) < 0 then abs(sum(Weight1)) else 0 end) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by name1 having sum(Weight1) <> 0"
        'cmd.ExecuteNonQuery()


        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1           , name1           , name2         , meters1      , name3          , name4         , name5        , name6 ,name7       ,Meters10                , weight1      , weight2       ,Name10         , iNT4      , Int5) " & _
                                        " Select   1  , a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'YARN',c.count_name,    abs(a.Bags)      , 0            ,abs(a.Weight) , 'YARNRECEIPT'  , a.Mill_IdNo  , a.Count_Idno from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 AND a.DeliveryTo_Idno = tP.Ledger_IdNo  LEFT OUTER JOIN Ledger_Head tPP ON a.ReceivedFromIdno_ForParticulars <> 0 and a.ReceivedFromIdno_ForParticulars = tPP.Ledger_IdNo   INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and (a.Reference_Code NOT LIKE 'SZSPC-%' and a.Reference_Code NOT LIKE 'SZPRC-%') and a.Weight <> 0 "
        cmd.ExecuteNonQuery()

        '--YARN RECEIPT
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1           , name1           , name2         , meters1      , name3          , name4         , name5        , name6  ,name7            ,Meters10                , weight1           , weight2     ,  Name10       , iNT4      , Int5       ) " & _
                                                 "Select 2 , a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'YARN',c.count_name, abs(a.Bags)      ,   abs(a.Weight)         ,   0         , 'YARNRECEIPT'  , a.Mill_IdNo  , a.Count_Idno  from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 AND a.ReceivedFrom_Idno = tP.Ledger_IdNo    INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and (a.Reference_Code NOT LIKE 'SZSPC-%' and a.Reference_Code NOT LIKE 'SZPRC-%') and a.Weight <> 0 AND a.Reference_Code NOT LIKE '" & Trim(Pk_Condition) & "%'  AND a.DeliveryToIdno_ForParticulars <> 0"
        cmd.ExecuteNonQuery()

        '--CLOTH RECEIPT
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1           , name1           , name2         , meters1      , name3          , name4         , name5        , name6  ,name7       ,Meters10                , weight1          , weight2   , Name10          ) " & _
                                            "Select 2 , a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'CLOTH',c.count_name, abs(a.Bags)      ,    abs(a.Weight)    ,    0      ,  'CLOTHRECEIPT' from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 AND a.ReceivedFrom_Idno = tP.Ledger_IdNo   INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and (a.Reference_Code NOT LIKE 'SZSPC-%' and a.Reference_Code NOT LIKE 'SZPRC-%') and a.Weight <> 0 AND a.Reference_Code LIKE '" & Trim(Pk_Condition) & "%' "
        cmd.ExecuteNonQuery()

        '-------- Amount

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, name5, name6, Currency1) Select 0, 'Opening', 'AMOUNT', sum(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Voucher_Date < @fromdate and a.Voucher_Amount <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, Currency1) Select 12, a.Voucher_Date, a.Voucher_Code, a.Voucher_No, a.For_OrderBy, a.Voucher_No, tP.Ledger_Name, replace(left(a.Entry_Identification, len(a.Entry_Identification)-6),'-' + cast(a.Company_Idno as varchar) + '-','-') as Particularss, 'AMOUNT', -1*abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount < 0 "
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, Currency1) Select 11, a.Voucher_Date, a.Voucher_Code, a.Voucher_No, a.For_OrderBy, a.Voucher_No, tP.Ledger_Name, replace(left(a.Entry_Identification, len(a.Entry_Identification)-6),'-' + cast(a.Company_Idno as varchar) + '-','-') as Particularss, 'AMOUNT', abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount > 0 "
        cmd.ExecuteNonQuery()

        '------------pCS
        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1              , name1                         , name2            , meters1      , name3            , name4         , name5, name6  ,name7,Meters10  ,Name10 ) " & _
        '                            " Select       2  , a.Weaver_Wages_Date, 'WVCIN-' + a.Weaver_Wages_Code, a.Weaver_Wages_No, a.For_OrderBy, a.Weaver_Wages_No, tP.Ledger_Name, ''   , 'CLOTH', ''  , a.pcs , 'CLOTHRECEIPT' from Weaver_Wages_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Weaver_Wages_Code = '" & Trim(NewCode) & "'"
        'cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1              , name1                         , name2            , meters1      , name3            , name4         , name5, name6  ,name7,Meters10  ,Name10 ) " & _
                                  " Select       2  , a.Weaver_Wages_Date, 'WVCIN-' + a.Weaver_Wages_Code, a.Weaver_Wages_No, a.For_OrderBy, a.Weaver_Wages_No, tP.Ledger_Name, ''   , 'CLOTH', ''  , a.pcs , 'CLOTHRECEIPT' from Weaver_Wages_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.pcs <> 0 "
        cmd.ExecuteNonQuery()

        '----
        cmd.CommandText = "update " & Trim(Common_Procedures.ReportTempTable) & " set name10 = 'Opening' from " & Trim(Common_Procedures.ReportTempTable) & " a WHERE a.name5 = 'Opening'"
        cmd.ExecuteNonQuery()



    End Sub

    Private Sub cbo_Multi_WeaverName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Multi_WeaverName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Multi_WeaverName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Multi_WeaverName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Multi_WeaverName, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        If (e.KeyValue = 40 And cbo_Multi_WeaverName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            btn_Choolie_Chit_Print.Focus()

        End If

    End Sub

    Private Sub cbo_Multi_WeaverName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Multi_WeaverName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Multi_WeaverName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Choolie_Chit_Print.Focus()
        End If
    End Sub

    Private Sub cbo_Multi_WeaverName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Multi_WeaverName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Show_MultiInput_List()
        End If
    End Sub

    Private Sub Show_MultiInput_List()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim SqlCondt As String = ""
        Dim I As Integer = 0
        Dim n As Integer = 0
        Dim a() As String
        Dim SelcNms As String = ""

        chklst_MultiInput.Items.Clear()
        lst_MultiInput_IdNos.Items.Clear()

        SelcNms = ""
        If Trim(RptCboDet(1).MultiSelectedNames_AsString) <> "" Then
            a = Split(RptCboDet(1).MultiSelectedNames_AsString, ",")
            SelcNms = Join(a, "~")
            If Trim(SelcNms) <> "" Then
                SelcNms = "~" & SelcNms & "~"
            End If
        End If

        Da1 = New SqlClient.SqlDataAdapter("Select DISTINCT Ledger_Name, Ledger_idno from Ledger_Head where Ledger_Type = 'WEAVER' Order by Ledger_Name", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            For I = 0 To Dt1.Rows.Count - 1
                n = chklst_MultiInput.Items.Add(Dt1.Rows(I).Item("Ledger_Name").ToString)

                If Trim(Dt1.Rows(I).Item("Ledger_Name").ToString) <> "" Then

                    lst_MultiInput_IdNos.Items.Add(Dt1.Rows(I).Item("Ledger_idno").ToString)

                Else
                    lst_MultiInput_IdNos.Items.Add(0)
                End If

                If InStr(1, Trim(UCase(SelcNms)), "~" & Trim(UCase((Dt1.Rows(I)(0).ToString))) & "~") > 0 Then
                    chklst_MultiInput.SetItemChecked(n, True)
                End If

            Next
        End If
        Dt1.Clear()


        lbl_MultiInput_Heading.Text = "SELECT ITEMS"

        lbl_MultiInput_Heading.Text = "Select Weaver Name "

        pnl_MultiInput.Left = Val(Pnl_PrintSelection.Left) + 70
        pnl_MultiInput.Top = Pnl_PrintSelection.Top + cbo_Multi_WeaverName.Height + 40
        pnl_MultiInput.BringToFront()


        chklst_MultiInput.Tag = 1
        pnl_MultiInput.Visible = True
        Pnl_Back.Enabled = False
        chklst_MultiInput.Focus()
        SendKeys.Send("{DOWN}")

    End Sub

    Private Sub btn_MultiInput_SelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_MultiInput_SelectAll.Click
        Dim I As Integer = 0

        For I = 0 To chklst_MultiInput.Items.Count - 1
            chklst_MultiInput.SetItemChecked(I, True)
        Next I
        Set_CheckedList_SelectedItem_Text()

    End Sub

    Private Sub Set_CheckedList_SelectedItem_Text()
        Dim s As String = ""

        s = ""
        If chklst_MultiInput.CheckedItems.Count > 0 Then
            s = chklst_MultiInput.CheckedItems.Count & " Name Selected"
        End If

        Cooli_Count = 1
        Cooli_Count = Val(chklst_MultiInput.CheckedItems.Count)

        If Val(chklst_MultiInput.Tag) = 1 Then
            If cbo_Multi_WeaverName.Visible Then cbo_Multi_WeaverName.Text = s
        End If
    End Sub

    Private Sub btn_MultiInput_DeSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_MultiInput_DeSelectAll.Click
        Dim I As Integer = 0

        For I = 0 To chklst_MultiInput.Items.Count - 1
            chklst_MultiInput.SetItemChecked(I, False)
        Next I
        Set_CheckedList_SelectedItem_Text()

    End Sub

    Private Sub chklst_MultiInput_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chklst_MultiInput.Click
        Set_CheckedList_SelectedItem_Text()
    End Sub

    Private Sub chklst_MultiInput_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chklst_MultiInput.KeyPress
        If Asc(e.KeyChar) = 13 Then
            chklst_MultiInput.SetItemChecked(chklst_MultiInput.SelectedIndex, Not chklst_MultiInput.GetItemChecked(chklst_MultiInput.SelectedIndex))
        End If
        Set_CheckedList_SelectedItem_Text()
    End Sub

    Private Sub chklst_MultiInput_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chklst_MultiInput.SelectedIndexChanged
        Set_CheckedList_SelectedItem_Text()
    End Sub

    Private Sub btn_Close_MultiInput_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_MultiInput.Click
        Dim i As Integer = 0
        Dim indexChecked As Integer

        RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelection_Status = False
        RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedIdNos_AsString = ""
        RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedNames_AsString = ""
        RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedIdNos_ForInQuery = "-9999999999"
        RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedNames_ForInQuery = "'~~~~~~~~~!!!!@@@@~~~~~~'"


        For Each indexChecked In chklst_MultiInput.CheckedIndices
            If Trim(chklst_MultiInput.Items(indexChecked).ToString) <> "" Then
                RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedNames_AsString = Trim(RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedNames_AsString) & IIf(Trim(RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedNames_AsString) <> "", ",", "") & Trim(chklst_MultiInput.Items(indexChecked).ToString)
                RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedNames_ForInQuery = Trim(RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedNames_ForInQuery) & IIf(Trim(RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedNames_ForInQuery) <> "", ",", "") & "'" & Trim(chklst_MultiInput.Items(indexChecked).ToString) & "'"

                ' If Trim(RptCboDet(Val(chklst_MultiInput.Tag)).Return_FieldName) <> "" Then
                If indexChecked <= lst_MultiInput_IdNos.Items.Count - 1 Then
                    If Val(lst_MultiInput_IdNos.Items(indexChecked).ToString) <> 0 Then
                        RptCboDet(1).MultiSelectedIdNos_AsString = Trim(RptCboDet(1).MultiSelectedIdNos_AsString) & IIf(Trim(RptCboDet(1).MultiSelectedIdNos_AsString) <> "", ",", "") & Trim(lst_MultiInput_IdNos.Items(indexChecked).ToString)
                        RptCboDet(1).MultiSelectedIdNos_ForInQuery = Trim(RptCboDet(1).MultiSelectedIdNos_ForInQuery) & IIf(Trim(RptCboDet(1).MultiSelectedIdNos_ForInQuery) <> "", ",", "") & Trim(lst_MultiInput_IdNos.Items(indexChecked).ToString)
                    End If
                End If
                'End If

                RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelection_Status = True

            End If

        Next

        RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedIdNos_ForInQuery = "(" & Trim(RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedIdNos_ForInQuery) & ")"
        RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedNames_ForInQuery = "(" & Trim(RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedNames_ForInQuery) & ")"

        pnl_Back.Enabled = True
        pnl_MultiInput.Visible = False
        Set_CheckedList_SelectedItem_Text()

        If Val(chklst_MultiInput.Tag) = 1 Then
            If cbo_Multi_WeaverName.Visible And cbo_Multi_WeaverName.Enabled Then cbo_Multi_WeaverName.Focus()
        End If

    End Sub

    Private Function Get_Ledger_Previousdate(ByVal led_id As Integer, ByVal cur_date As Date) As Date
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vDate_To As Date, vDate_From As Date
        Dim SqlCondt As String = ""
        Dim NewCode As String


        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@WeaWageDate", cur_date)

        If IsDate(msk_LedgerFromDate.Text) = True Then
            vDate_From = Convert.ToDateTime(msk_LedgerFromDate.Text)
        Else
            vDate_From = Common_Procedures.Company_FromDate
        End If

        If IsDate(msk_LedgerToDate.Text) = True Then
            vDate_To = Convert.ToDateTime(msk_LedgerToDate.Text)
        Else
            vDate_To = cur_date
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'cmd.CommandText = "select max(a.weaver_wages_date) from Weaver_Wages_Head a Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Weaver_Wages_Date < @WeaWageDate"
        'Da1 = New SqlClient.SqlDataAdapter(cmd)
        'Dt1 = New DataTable
        'Da1.Fill(Dt1)

        'If Dt1.Rows.Count > 0 Then

        '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then

        '        If IsDate(Dt1.Rows(0)(0).ToString) = True Then
        '            vDate_From = Dt1.Rows(0)(0).ToString
        '            vDate_From = DateAdd("d", 1, vDate_From.Date)
        '        End If

        '    End If

        'End If

        'Dt1.Clear()

        Get_Ledger_Previousdate = vDate_From

    End Function

    Private Sub Print_Begin(ByVal Led_Id As Integer)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String
        Dim cont As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0


        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* , e.Transport_Name  from Weaver_Wages_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Left Outer JOIN Transport_Head e ON a.Transport_IdNo = e.Transport_IdNo where  a.Receipt_Meters <> 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.Ledger_Name as Weaver_Name , C.Pan_No , C.Ledger_GSTinNo ,  c.Tamil_Name ,d.Cloth_Name, d.Tamil_Name as clothTamilName ,d.Wages_For_Type1   from Weaver_Wages_Details a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "'and a.Ledger_idno  = " & Val(Led_Id) & "  Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_BalanceRegister_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BalanceRegister_Print.Click
        Dim NewCode As String
        Dim f As New Report_Details

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Weaver Cloth Inward Balance Register"
        Common_Procedures.RptInputDet.ReportHeading = "Weaver Cloth Inward Balance Register"
        Common_Procedures.RptInputDet.ReportInputs = "1DT"
        Common_Procedures.RptInputDet.Name1 = NewCode
        Common_Procedures.RptInputDet.Date1 = dtp_Date.Value
        f.MdiParent = MDIParent1
        f.Show()
        f.dtp_FromDate.Text = dtp_Date.Text
        Pnl_PrintSelection.Visible = False
        Pnl_Back.Enabled = True

    End Sub

    Private Sub btn_Close_PrintSelection_1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_PrintSelection_1.Click
        Pnl_PrintSelection.Visible = False
        Pnl_Back.Enabled = True
        cbo_Multi_WeaverName.Text = ""

    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
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

    Private Sub txt_NofoKattu_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NofoKattu.TextChanged
        lbl_Frieght.Text = Format(Val(txt_NofoKattu.Text) * Val(txt_RatePerKattu.Text), "#######0.00")
    End Sub

    Private Sub txt_RatePerKattu_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_RatePerKattu.TextChanged
        lbl_Frieght.Text = Format(Val(txt_NofoKattu.Text) * Val(txt_RatePerKattu.Text), "#######0.00")
    End Sub

    Private Sub msk_LedgerFromDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_LedgerFromDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub msk_LedgerFromDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_LedgerFromDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_LedgerFromDate.Text = Date.Today
        End If
        If IsDate(msk_LedgerFromDate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_LedgerFromDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_LedgerFromDate.Text))
            ElseIf e.KeyCode = 109 Then
                msk_LedgerFromDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_LedgerFromDate.Text))
            End If
        End If
    End Sub

    Private Sub msk_LedgerFromDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_LedgerFromDate.LostFocus

        If IsDate(msk_LedgerFromDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_LedgerFromDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_LedgerFromDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_LedgerFromDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_LedgerFromDate.Text)) >= 2000 Then
                    dtp_LedgerFromDate.Value = Convert.ToDateTime(msk_LedgerFromDate.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_LedgerFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_LedgerFromDate.TextChanged
        If IsDate(dtp_LedgerFromDate.Text) = True Then
            msk_LedgerFromDate.Text = dtp_LedgerFromDate.Text
        End If
    End Sub

    Private Sub msk_LedgerToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_LedgerToDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub msk_LedgerToDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_LedgerToDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_LedgerToDate.Text = Date.Today
        End If
        If IsDate(msk_LedgerToDate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_LedgerToDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_LedgerToDate.Text))
            ElseIf e.KeyCode = 109 Then
                msk_LedgerToDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_LedgerToDate.Text))
            End If
        End If
    End Sub

    Private Sub msk_LedgerToDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_LedgerToDate.LostFocus

        If IsDate(msk_LedgerToDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_LedgerToDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_LedgerToDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_LedgerToDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_LedgerToDate.Text)) >= 2000 Then
                    dtp_LedgerToDate.Value = Convert.ToDateTime(msk_LedgerToDate.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_LedgerToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_LedgerToDate.TextChanged
        If IsDate(dtp_LedgerToDate.Text) = True Then
            msk_LedgerToDate.Text = dtp_LedgerToDate.Text
        End If
    End Sub

    Private Sub Get_PreviousWagesDate()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vDate_From As Date
        Dim NewCode As String
        Dim Nr As Integer = 0

        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@WeaWageDate", dtp_Date.Value.Date)


        vDate_From = Common_Procedures.Company_FromDate
        dtp_LedgerFromDate.Value = vDate_From.Date
        dtp_LedgerToDate.Value = dtp_Date.Value.Date
        If IsDate(dtp_LedgerToDate.Text) = True Then
            msk_LedgerToDate.Text = dtp_LedgerToDate.Text
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        cmd.CommandText = "select max(a.weaver_wages_date) AS WagesDate from Weaver_Wages_Head a LEFT OUTER JOIN Weaver_Wages_Details B ON A.Weaver_Wages_Code = b.Weaver_Wages_Code Where   a.Weaver_Wages_Code <> '" & Trim(NewCode) & "' and a.company_idno = " & Str(Val(lbl_Company.Tag)) & "  and a.Weaver_Wages_Date < @WeaWageDate"
        Da1 = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("WagesDate").ToString) = False Then
                If IsDate(Dt1.Rows(0).Item("WagesDate").ToString) = True Then

                    vDate_From = Dt1.Rows(0).Item("WagesDate").ToString

                    dtp_LedgerFromDate.Value = DateAdd("d", 1, vDate_From.Date)
                    dtp_LedgerToDate.Value = dtp_Date.Value.Date

                End If

            End If

        End If

        Dt1.Clear()
        Da1.Dispose()

    End Sub

    Private Sub btn_GSTPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_GSTPrint.Click
        Dim Wev_Id As Integer = 0

        Prn_Sql_Conditon_Multi = ""

        If Trim(cbo_Multi_WeaverName.Text) <> "" Then

            If InStr(Trim(UCase(cbo_Multi_WeaverName.Text)), "SELECTED") > 0 Then

                Prn_Sql_Conditon_Multi = RptCboDet(Val(chklst_MultiInput.Tag)).MultiSelectedIdNos_AsString

            Else

                Wev_Id = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Multi_WeaverName.Text)))

                Prn_Sql_Conditon_Multi = Wev_Id
            End If
        End If

        print_Format = "FORMAT-3"
        Printing_Invoice()
        btn_Close_PrintSelection_Click(sender, e)

    End Sub
End Class