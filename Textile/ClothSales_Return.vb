Public Class ClothSales_Return

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CLSRT-"
    Private Pk_Condition_GST As String = "GCLSR-"

    Private Prec_ActCtrl As New Control
    Private NoCalc_Status As Boolean = False
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

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
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False
        pnl_Selection.Visible = False
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        txt_InvNo.Text = ""
        cbo_PartyName.Text = ""
        cbo_PartyName.Tag = ""
        cbo_Cloth.Text = ""
        txt_InvNo.Text = ""
        ' txt_Folding.Text = ""
        cbo_SalesAc.Text = ""
        txt_Amount.Text = ""

        txt_NoOfPcs.Text = ""
        txt_PcsNoFrom.Text = "1"

        lbl_PcsNoTo.Text = ""
        txt_Meters.Text = ""
        cbo_Transport.Text = ""
        txt_Freight.Text = ""

        txt_Note.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        lbl_ClothSales_Return_Code.Text = ""
        lbl_ClothSales_Return_SlNo.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details.AllowUserToAddRows = True

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        txt_InvNo.Enabled = True
        txt_InvNo.BackColor = Color.White

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_Cloth.Enabled = True
        cbo_Cloth.BackColor = Color.White

        txt_NoOfPcs.Enabled = True
        txt_NoOfPcs.BackColor = Color.White

        txt_PcsNoFrom.Enabled = True
        txt_PcsNoFrom.BackColor = Color.White

        txt_Meters.Enabled = True
        txt_Meters.BackColor = Color.White

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Cloth.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_Cloth.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()
        End If

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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

        If Me.ActiveControl.Name <> dgv_Details.Name Then
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
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
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
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim LockSTS As Boolean = False
        Dim LtCd As String = ""

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from ClothSales_Return_Head a Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Return_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("ClothSales_Return_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("ClothSales_Return_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("PurchaseAc_IdNo").ToString))
                txt_Amount.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "########0.00")
                txt_InvNo.Text = dt1.Rows(0).Item("Invoice_No").ToString
                txt_NoOfPcs.Text = Val(dt1.Rows(0).Item("noof_pcs").ToString)
                txt_PcsNoFrom.Text = dt1.Rows(0).Item("pcs_fromno").ToString
                lbl_PcsNoTo.Text = dt1.Rows(0).Item("pcs_tono").ToString
                txt_Meters.Text = Format(Val(dt1.Rows(0).Item("Return_Meters").ToString), "########0.00")
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight").ToString), "########0.00")
                If Val(txt_Freight.Text) = 0 Then
                    txt_Freight.Text = ""
                End If
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                lbl_ClothSales_Return_Code.Text = dt1.Rows(0).Item("ClothSales_Invoice_Code").ToString
                lbl_ClothSales_Return_SlNo.Text = dt1.Rows(0).Item("ClothSales_Invoice_SlNo").ToString

                LockSTS = False
                If IsDBNull(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                LtCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.LotCode.Sales_Return) & "/" & Trim(Common_Procedures.FnYearCode)

                da2 = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(LtCd) & "' and Create_Status = 1 Order by Sl_No, Piece_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        dgv_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Piece_No").ToString
                        dgv_Details.Rows(n).Cells(1).Value = Format(Val(dt2.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.00")

                    Next i

                End If
                dt2.Clear()

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(0).Value = Val(dt1.Rows(0).Item("Total_Return_Pcs").ToString)
                    .Rows(0).Cells(1).Value = Format(Val(dt1.Rows(0).Item("Total_Return_Meters").ToString), "########0.00")
                End With

            End If

            dt1.Clear()

            Grid_Cell_DeSelect()

            If LockSTS = True Then

                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_Cloth.Enabled = False
                cbo_Cloth.BackColor = Color.LightGray


                txt_NoOfPcs.Enabled = False
                txt_NoOfPcs.BackColor = Color.LightGray

                txt_PcsNoFrom.Enabled = False
                txt_PcsNoFrom.BackColor = Color.LightGray

                txt_Meters.Enabled = False
                txt_Meters.BackColor = Color.LightGray

                dgv_Details.AllowUserToAddRows = False

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub ClothSales_Return_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Cloth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub ClothSales_Return_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_PartyName.DataSource = dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt2)
        cbo_Cloth.DataSource = dt2
        cbo_Cloth.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_Transport.DataSource = dt3
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 28 ) order by Ledger_DisplayName", con)
        da.Fill(dt5)
        cbo_SalesAc.DataSource = dt5
        cbo_SalesAc.DisplayMember = "Ledger_DisplayName"


        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_InvNo.GotFocus, AddressOf ControlGotFocus
        ' AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsNoFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfPcs.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvNo.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsNoFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfPcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_InvNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfPcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoOfPcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        dtp_Date.Text = ""
        txt_InvNo.Text = ""
        cbo_PartyName.Text = ""
        cbo_PartyName.Tag = ""

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub ClothSales_Return_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub ClothSales_Return_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(1).Value) = 0 Then
                                If txt_Meters.Enabled And txt_Meters.Visible Then
                                    txt_Meters.Focus()
                                Else
                                    txt_Note.Focus()
                                End If


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

                                txt_PcsNoFrom.Focus()

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""


        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Sales_Return_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Sales_Return_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from ClothSales_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(NewCode) & "' and  Weaver_Piece_Checking_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Piece checking prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "Update ClothSales_Invoice_Details set Return_Meters = a.Return_Meters - b.Return_Meters from ClothSales_Invoice_Details a, ClothSales_Return_Head b Where b.ClothSales_Return_Code = '" & Trim(NewCode) & "'  and a.ClothSales_Invoice_Code = b.ClothSales_Invoice_Code and a.ClothSales_Invoice_SlNo = b.ClothSales_Invoice_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where Ledger_IdNo = 0 or ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) order by Ledger_DisplayName", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Return_No from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code NOT LIKE '" & Trim(Pk_Condition_GST) & "%'  AND ClothSales_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ClothSales_Return_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Return_No from ClothSales_Return_Head where for_orderby > " & Str(Val(OrdByNo)) & " and  ClothSales_Return_Code NOT LIKE '" & Trim(Pk_Condition_GST) & "%'   AND  company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ClothSales_Return_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Return_No from ClothSales_Return_Head where for_orderby < " & Str(Val(OrdByNo)) & " and   ClothSales_Return_Code NOT LIKE '" & Trim(Pk_Condition_GST) & "%'   AND company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothSales_Return_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Return_No from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code NOT LIKE '" & Trim(Pk_Condition_GST) & "%'  AND  ClothSales_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothSales_Return_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "ClothSales_Return_Head", "ClothSales_Return_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothSales_Return_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("ClothSales_Return_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("ClothSales_Return_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

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

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select ClothSales_Return_No from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & "  ClothSales_Return_Code NOT LIKE '" & Trim(Pk_Condition_GST) & "%'   AND  and ClothSales_Return_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Sales_Return_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Sales_Return_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select ClothSales_Return_No from ClothSales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " AND  ClothSales_Return_Code NOT LIKE '" & Trim(Pk_Condition_GST) & "%'   and ClothSales_Return_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim SalAc_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim TtRetMtrs As Single = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotRetPcs As Single, vTotRetMtrs As Single
        Dim WftCnt_ID As Integer = 0
        Dim EntID As String = 0
        Dim Dup_PcNo As String = ""
        Dim PcsChkCode As String = ""
        Dim Nr As Integer = 0
        Dim LtNo As String = ""
        Dim LtCd As String = ""
        Dim Usr_ID As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.ClothSales_Sales_Return_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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



        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If Clo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        SalAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAc.Text)
        If SalAc_ID = 0 And Val(txt_Amount.Text) <> 0 Then
            MessageBox.Show("Invalid Sales A/c Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalesAc.Enabled And cbo_SalesAc.Visible Then cbo_SalesAc.Focus()
            Exit Sub
        End If

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(1).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(0).Value) = "" Then
                        MessageBox.Show("Invalid Pcs No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .CurrentCell = .Rows(i).Cells(0)
                            .Focus()
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(UCase(Dup_PcNo)), "~" & Trim(UCase(.Rows(i).Cells(0).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate Pcs No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_PcNo = Trim(Dup_PcNo) & "~" & Trim(UCase(.Rows(i).Cells(0).Value)) & "~"

                End If

            Next

        End With

        NoCalc_Status = False
        Calculation_Details_Total()

        vTotRetPcs = 0 : vTotRetMtrs = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotRetPcs = Val(dgv_Details_Total.Rows(0).Cells(0).Value())
            vTotRetMtrs = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
        End If

        If Val(vTotRetMtrs) <> 0 Then
            If Val(vTotRetMtrs) <> Val(txt_Meters.Text) Then
                MessageBox.Show("Mismatch of Return Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Meters.Enabled And txt_Meters.Visible Then txt_Meters.Focus()
                Exit Sub
            End If
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "ClothSales_Return_Head", "ClothSales_Return_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)

            Da = New SqlClient.SqlDataAdapter("select * from ClothSales_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            PcsChkCode = ""
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                End If
            End If
            Dt1.Clear()

            If New_Entry = True Then
                cmd.CommandText = "Insert into ClothSales_Return_Head ( ClothSales_Return_Code ,             Company_IdNo         ,        ClothSales_Return_No   ,                               for_OrderBy                              ,  ClothSales_Return_Date,         Ledger_IdNo     ,             Invoice_No        ,      Cloth_IdNo    ,   PurchaseAc_IdNo  ,             Amount          ,              noof_pcs         ,             pcs_fromno         ,             pcs_tono         ,           ReturnMeters_Return    ,            Return_Meters         ,    Transport_IdNo    ,             Freight          ,               Note           ,        Total_Return_Pcs     ,       Total_Return_Meters    ,               ClothSales_Invoice_Code          ,             ClothSales_Invoice_SlNo         , Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment ,  user_idno  ) " & _
                                  "                   Values          ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @EntryDate       , " & Str(Val(Led_ID)) & ", '" & Trim(txt_InvNo.Text) & "', " & Val(Clo_ID) & ", " & Val(SalAc_ID) & ", " & Val(txt_Amount.Text) & ",  " & Val(txt_NoOfPcs.Text) & ", " & Val(txt_PcsNoFrom.Text) & ", " & Val(lbl_PcsNoTo.Text) & ", " & Str(Val(txt_Meters.Text)) & ", " & Str(Val(txt_Meters.Text)) & ", " & Val(Trans_ID) & ", " & Val(txt_Freight.Text) & ", '" & Trim(txt_Note.Text) & "', " & Str(Val(vTotRetPcs)) & ", " & Str(Val(vTotRetMtrs)) & ", '" & Trim(lbl_ClothSales_Return_Code.Text) & "', " & Val(lbl_ClothSales_Return_SlNo.Text) & ",               ''          ,             0                ," & Val(lbl_UserName.Text) & "   ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update ClothSales_Return_Head set ClothSales_Return_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Cloth_IdNo = " & Val(Clo_ID) & ", PurchaseAc_IdNo = " & Val(SalAc_ID) & ", Amount = " & Val(txt_Amount.Text) & ", Invoice_No = '" & Trim(txt_InvNo.Text) & "',  noof_pcs = " & Val(txt_NoOfPcs.Text) & " , pcs_fromno = " & Val(txt_PcsNoFrom.Text) & " , pcs_tono = " & Val(lbl_PcsNoTo.Text) & ", ReturnMeters_Return = " & Val(txt_Meters.Text) & ",Transport_IdNo = " & Val(Trans_ID) & " , Freight = " & Val(txt_Freight.Text) & " , Note = '" & Trim(txt_Note.Text) & "', Total_Return_Pcs = " & Str(Val(vTotRetPcs)) & ", Total_Return_Meters = " & Str(Val(vTotRetMtrs)) & ", ClothSales_Invoice_Code = '" & Trim(lbl_ClothSales_Return_Code.Text) & "', ClothSales_Invoice_SlNo = " & Val(lbl_ClothSales_Return_SlNo.Text) & " , User_IdNo = " & Val(lbl_UserName.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update ClothSales_Return_Head set  Return_Meters = " & Str(Val(txt_Meters.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = ''"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update ClothSales_Invoice_Details set Return_Meters = a.Return_Meters - b.Return_Meters from ClothSales_Invoice_Details a, ClothSales_Return_Head b Where b.ClothSales_Return_Code = '" & Trim(NewCode) & "'  and a.ClothSales_Invoice_Code = b.ClothSales_Invoice_Code and a.ClothSales_Invoice_SlNo = b.ClothSales_Invoice_SlNo"
                cmd.ExecuteNonQuery()

            End If

            If Trim(lbl_ClothSales_Return_Code.Text) <> "" Then
                Nr = 0
                cmd.CommandText = "Update ClothSales_Invoice_Details set Return_Meters = Return_Meters + " & Str(Val(txt_Meters.Text)) & " Where ClothSales_Invoice_Code = '" & Trim(lbl_ClothSales_Return_Code.Text) & "' and ClothSales_Invoice_SlNo = " & Str(Val(lbl_ClothSales_Return_SlNo.Text))
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    Throw New ApplicationException("Mismatch of PartyName & ClothSales Invoice Details")
                    Exit Sub
                End If
            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            Partcls = "SalesReturn : RefNo. " & Trim(lbl_RefNo.Text)
            If Trim(txt_InvNo.Text) <> "" Then
                PBlNo = Trim(txt_InvNo.Text)
            Else
                PBlNo = Trim(lbl_RefNo.Text)
            End If


            LtNo = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.LotCode.Sales_Return)
            LtCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.LotCode.Sales_Return) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(LtCd) & "' and Create_Status = 1 and Weaver_Piece_Checking_Code = ''"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Then

                        Sno = Sno + 1

                        Nr = 0
                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Weaver_ClothReceipt_Date = @EntryDate, Sl_No = " & Str(Val(Sno)) & ", PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(0).Value))) & ", ReceiptMeters_Receipt = " & Val(.Rows(i).Cells(1).Value) & ", Create_Status = 1 where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(LtCd) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_ClothReceipt_Code ,            Company_IdNo          ,      Weaver_ClothReceipt_No   ,                               for_OrderBy                              , Weaver_ClothReceipt_Date,        Lot_Code     ,          Lot_No     ,           Cloth_IdNo    ,             Sl_No     ,                    Piece_No            ,                               PieceNo_OrderBy                                   ,       ReceiptMeters_Receipt         ,                  Receipt_Meters     , Create_Status ) " & _
                                                                "  Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",          @EntryDate     , '" & Trim(LtCd) & "', '" & Trim(LtNo) & "', " & Str(Val(Clo_ID)) & ",  " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(0).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(0).Value))) & ", " & Val(.Rows(i).Cells(1).Value) & ", " & Val(.Rows(i).Cells(1).Value) & ",       1       )"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next

            End With

            If Trim(PcsChkCode) = "" Then

                cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Val(txt_Meters.Text) <> 0 Then
                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,                               StockOff_IdNo               ,                               DeliveryTo_Idno             ,       ReceivedFrom_Idno ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     ,               UnChecked_Meters    ,  Meters_Type1, Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ) " & _
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @EntryDate , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ",  " & Str(Val(txt_Meters.Text)) & ",       0      ,       0     ,       0     ,       0     ,       0      ) "
                    cmd.ExecuteNonQuery()
                End If

            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0

            AcPos_ID = Led_ID

            vLed_IdNos = AcPos_ID & "|" & SalAc_ID
            vVou_Amts = Val(txt_Amount.Text) & "|" & -1 * Val(txt_Amount.Text)
            If Common_Procedures.Voucher_Updation(con, "CloSale.Ret", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(txt_InvNo.Text), Convert.ToDateTime(dtp_Date.Text), "Inv No : " & Trim(txt_InvNo.Text) & ", Mtrs : " & Trim(Format(Val(txt_Meters.Text), "#########0.00")), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            Dim VouBil As String = ""
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Convert.ToDateTime(dtp_Date.Text), AcPos_ID, Trim(txt_InvNo.Text), 0, Val(txt_Amount.Text), "CR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

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
            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_Date, txt_Amount, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) ", "(Ledger_idno = 0)")
        If (e.KeyValue = 40 And cbo_PartyName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to select Cloth Sales Invoice Details ", "FOR CLOTH SALES INVOICE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                If txt_InvNo.Enabled Then txt_InvNo.Focus() Else cbo_SalesAc.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Cloth Sales Invoice:", "FOR CLOTH SALES INVOICE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                If txt_InvNo.Enabled Then txt_InvNo.Focus() Else cbo_SalesAc.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, cbo_SalesAc, txt_NoOfPcs, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, txt_NoOfPcs, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0 )")
    End Sub

    Private Sub cbo_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then


            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAc, cbo_PartyName, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAc, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_Amount, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Transport_Creation
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

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Clo_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clo_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothSales_Return_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.ClothSales_Return_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothSales_Return_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_Cloth.Text) <> "" Then
                Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Cloth.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Clo_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Cloth_IdNo = " & Str(Val(Clo_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Cloth_Name  from ClothSales_Return_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and A.ClothSales_Return_Code NOT LIKE '" & Trim(Pk_Condition_GST) & "%'   AND a.ClothSales_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.ClothSales_Return_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("ClothSales_Return_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ClothSales_Return_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Invoice_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("noof_pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Return_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

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

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_TYPE = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_TYPE = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_TYPE = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, cbo_Filter_PartyName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        Dim TotMtrs As Single = 0

        Try
            Calculation_Details_Total()

            With dgv_Details_Total
                If .RowCount > 0 Then
                    TotMtrs = Val(.Rows(0).Cells(1).Value)
                End If
            End With
            txt_Meters.Text = Format(Val(TotMtrs), "#########0.00")

            dgv_Details_CellLeave(sender, e)

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL END EDIT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Try
            With dgv_Details

                If e.RowIndex = 0 Then
                    .CurrentRow.Cells(0).Value = Val(txt_PcsNoFrom.Text)

                Else
                    If Val(.CurrentRow.Cells(0).Value) = 0 Then
                        .CurrentRow.Cells(0).Value = Val(.Rows(e.RowIndex - 1).Cells(0).Value) + 1
                    End If

                End If

            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details
                If .CurrentCell.ColumnIndex = 1 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                End If
            End With
        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS DCELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim TotMtrs As Single = 0

        Try

            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 1 Then
                        Calculation_Details_Total()

                        With dgv_Details_Total
                            If .RowCount > 0 Then
                                TotMtrs = Val(.Rows(0).Cells(1).Value)
                            End If
                        End With
                        txt_Meters.Text = Format(Val(TotMtrs), "#########0.00")

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL VALUE CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim PcsFrmNo As Integer = 0
        Dim NewCode As String = ""
        Dim PcsChkCode As String = ""

        Try

            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_Code, Cloth_Purchase_Code from ClothSales_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Return_Code = '" & Trim(NewCode) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                PcsChkCode = ""
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                        PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                    End If
                End If
                Dt1.Clear()

                If Trim(PcsChkCode) <> "" Then
                    MessageBox.Show("Piece Checking prepared", "DOES NOT DELETE PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If

                With dgv_Details

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
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

                Calculation_Details_Total()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub


    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded

        Try
            With dgv_Details

                If e.RowIndex = 0 Then
                    .CurrentRow.Cells(0).Value = Val(txt_PcsNoFrom.Text)

                Else
                    If Val(.CurrentRow.Cells(0).Value) = 0 Then
                        .CurrentRow.Cells(0).Value = Val(.Rows(e.RowIndex - 1).Cells(0).Value) + 1
                    End If

                End If

            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS ROWS ADD....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Calculation_Details_Total()
        Dim TotPcs As Double = 0
        Dim TotMtrs As Double = 0

        Try

            If NoCalc_Status = True Then Exit Sub

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

            If Val(TotMtrs) <> 0 Then txt_Meters.Text = Format(Val(TotMtrs), "#########0.00")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TOTAL CALCULATION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_PcsNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PcsNoFrom.KeyDown
        If e.KeyCode = 40 Then
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.Focus()
            dgv_Details.CurrentCell.Selected = True

        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_ReceiptMeters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Meters.KeyDown
        Dim TotMtrs As Double = 0

        Try
            If e.KeyCode = 40 Then
                SendKeys.Send("{TAB}")

            ElseIf e.KeyCode = 38 Then
                SendKeys.Send("+{TAB}")

            ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 Then
                TotMtrs = 0
                With dgv_Details_Total
                    If .RowCount > 0 Then
                        TotMtrs = Val(.Rows(0).Cells(1).Value)
                    End If
                End With
                If Val(TotMtrs) <> 0 Then e.Handled = True : e.SuppressKeyPress = True

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE RECEIPT METERS KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub txt_ReceiptMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters.KeyPress
        Dim TotMtrs As Double = 0

        Try

            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

            With dgv_Details_Total
                TotMtrs = 0
                If .RowCount > 0 Then
                    TotMtrs = Val(.Rows(0).Cells(1).Value)
                End If
            End With
            If Val(TotMtrs) <> 0 Then e.Handled = True

            If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE RECEIPT METERS KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Try
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE FOLDING KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_PcsNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsNoFrom.KeyPress
        Try
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

            If Asc(e.KeyChar) = 13 Then
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.Focus()
                dgv_Details.CurrentCell.Selected = True
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE PCSNO KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_NoofPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfPcs.KeyPress
        Try
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE NOOFPCS KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyCode = 40 Then dtp_Date.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_ShortMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_meters_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Meters.LostFocus
        With txt_Meters
            .Text = Format(Val(.Text), "#########0.00")
        End With
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub Calculation_TO_PieceNo()

        lbl_PcsNoTo.Text = ""

        If Val(txt_NoOfPcs.Text) > 0 Then

            If Val(txt_PcsNoFrom.Text) = 0 Then txt_PcsNoFrom.Text = "1"

            lbl_PcsNoTo.Text = Val(txt_PcsNoFrom.Text) + Val(txt_NoOfPcs.Text) - 1

        End If

    End Sub

    Private Sub txt_NoOfPcs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_NoOfPcs.TextChanged
        Calculation_TO_PieceNo()
    End Sub

    Private Sub txt_PcsNoFrom_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_PcsNoFrom.TextChanged
        Dim i As Integer = 0
        Dim PcFrmNo As Integer = 0

        Try
            Calculation_TO_PieceNo()

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
            MessageBox.Show(ex.Message, "ERROR WHILE PCSNOFROM CHANGED....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        Try
            dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS EDITING SHOWING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_Details.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE TXT_DETAILS ENTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_Details
                If .Visible Then

                    If .CurrentCell.ColumnIndex = 1 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE TXT_DETAILS KEYPRESS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        Try
            dgv_Details_KeyUp(sender, e)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE TXT_DETAILS KEYUP...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            If e.Control = True And e.KeyValue = 13 Then
                If txt_Meters.Enabled And txt_Meters.Visible Then
                    txt_Meters.Focus()

                End If

            ElseIf e.KeyValue = 46 Then
                With dgv_Details
                    If .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells(1).Value = ""

                    End If

                End With

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYDOWN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0, Ent_Amt As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT CLOTH SALES DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 1 Then
            CompIDCondt = ""
        End If

        With dgv_Selection

            .Rows.Clear()

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name,  b.Noof_Pcs as Ent_Pcs, b.Return_Meters as Ent_Meters,b.Amount as Ent_Amount from ClothSales_Invoice_Details a INNER JOIN ClothSales_Invoice_Head d ON d.ClothSales_Invoice_Code = a.ClothSales_Invoice_Code LEFT OUTER JOIN ClothSales_Return_Head b ON b.ClothSales_Return_Code = '" & Trim(NewCode) & "' and a.ClothSales_Invoice_Code = b.ClothSales_Invoice_Code and a.ClothSales_Invoice_SlNo = b.ClothSales_Invoice_SlNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo   Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " d.ledger_Idno = " & Str(Val(LedIdNo)) & " and ( (a.Meters - a.Return_Meters) > 0 or (b.Return_Meters ) > 0 ) order by a.ClothSales_Invoice_Date, a.for_orderby, a.ClothSales_Invoice_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    Ent_Pcs = 0
                    Ent_Mtrs = 0
                    Ent_Amt = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
                        Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Meters").ToString) = False Then
                        Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_Meters").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Amount").ToString) = False Then
                        Ent_Amt = Val(Dt1.Rows(i).Item("Ent_Amount").ToString)
                    End If

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Invoice_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Invoice_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    '.Rows(n).Cells(4).Value = Dt1.Rows(i).Item("SalesAcName").ToString
                    .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Pcs").ToString)
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString) - Val(Dt1.Rows(i).Item("Return_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Amount").ToString)
                    .Rows(n).Cells(8).Value = ""
                    If (Ent_Mtrs) > 0 Then
                        .Rows(n).Cells(8).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next
                    End If

                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("ClothSales_Invoice_Code").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("ClothSales_Invoice_SlNo").ToString

                    .Rows(n).Cells(11).Value = Ent_Pcs
                    .Rows(n).Cells(12).Value = Ent_Mtrs

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        If dgv_Selection.Enabled And dgv_Selection.Visible Then
            dgv_Selection.Focus()
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Invoice(e.RowIndex)
    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown

        Try
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                    Select_Invoice(dgv_Selection.CurrentCell.RowIndex)

                    e.Handled = True

                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE SELECTION KEYDOWN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub Select_Invoice(ByVal RwIndx As Integer)
        Dim i As Integer = 0
        Dim j As Integer = 0

        Try

            With dgv_Selection

                If .RowCount > 0 And RwIndx >= 0 Then

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(8).Value = ""
                        For j = 0 To .Columns.Count - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Black
                        Next
                    Next

                    .Rows(RwIndx).Cells(8).Value = 1

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                    Close_ClothInvoice_Selection()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE SELECT INVOICE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Try
            Close_ClothInvoice_Selection()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE CLOSE SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub Close_ClothInvoice_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        Try
            dgv_Details.Rows.Clear()

            For i = 0 To dgv_Selection.RowCount - 1

                If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                    txt_InvNo.Text = dgv_Selection.Rows(i).Cells(1).Value
                    cbo_Cloth.Text = dgv_Selection.Rows(i).Cells(3).Value
                    ' txt_Folding.Text = Val(dgv_Selection.Rows(i).Cells(5).Value)
                    cbo_SalesAc.Text = dgv_Selection.Rows(i).Cells(4).Value

                    If Val(dgv_Selection.Rows(i).Cells(11).Value) <> 0 Then
                        txt_NoOfPcs.Text = Val(dgv_Selection.Rows(i).Cells(11).Value)
                    Else
                        txt_NoOfPcs.Text = Val(dgv_Selection.Rows(i).Cells(6).Value)
                    End If
                    If Val(txt_NoOfPcs.Text) = 0 Then
                        txt_NoOfPcs.Text = ""
                    End If

                    If Val(dgv_Selection.Rows(i).Cells(12).Value) <> 0 Then
                        txt_Meters.Text = Format(Val(dgv_Selection.Rows(i).Cells(12).Value), "#########0.00")
                    Else
                        txt_Meters.Text = Format(Val(dgv_Selection.Rows(i).Cells(7).Value), "#########0.00")
                    End If
                    If Val(txt_Meters.Text) = 0 Then
                        txt_Meters.Text = ""
                    End If

                    lbl_ClothSales_Return_Code.Text = dgv_Selection.Rows(i).Cells(9).Value
                    lbl_ClothSales_Return_SlNo.Text = dgv_Selection.Rows(i).Cells(10).Value

                End If

            Next

            Calculation_Details_Total()

            pnl_Back.Enabled = True
            pnl_Selection.Visible = False
            If txt_InvNo.Enabled And txt_InvNo.Visible Then txt_InvNo.Focus() Else cbo_SalesAc.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE CLOSE INVOICE SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
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
End Class