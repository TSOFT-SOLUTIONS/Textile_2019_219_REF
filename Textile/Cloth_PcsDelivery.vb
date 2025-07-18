Public Class Cloth_PcsDelivery
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CPDLV-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private Prev_kyData As Keys
    Private dgv_ActiveCtrl_Name As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(100, 50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_HdAr(100, 10) As String
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_BaleSelection.Visible = False

        pnl_Print.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_Agent.Text = ""
        cbo_Through.Text = "DIRECT"
        cbo_Transport.Text = ""
        cbo_DespTo.Text = ""
        cbo_ClothName.Text = ""

        cbo_Type.Text = "DIRECT"
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        txt_Freight.Text = ""
        txt_OrderNo.Text = ""
        txt_DelvAdd1.Text = ""
        txt_DelvAdd2.Text = ""
        txt_LrNo.Text = ""
        txt_Lr_Date.Text = ""
        txt_Order_Date.Text = ""
        txt_Folding.Text = ""
        txt_BaleNos.Text = ""
        lbl_TotalMeters.Text = ""
        lbl_TotalPcs.Text = ""
        txt_Note.Text = ""
        txt_Noofbundle.Text = ""
        txt_freightPerBundle.Text = ""

        chk_Sample.Checked = False
        chk_Invoice_In_100_Folding.Checked = True
        txt_DateAndTimeOFSupply.Text = ""

        lbl_TaxableMeter.Text = 0
        txt_Rate.Text = 0
        txt_NetAmount.Text = 0

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_Agent.Enabled = True
        cbo_Agent.BackColor = Color.White

        cbo_DespTo.Enabled = True
        cbo_DespTo.BackColor = Color.White

        cbo_Through.Enabled = True
        cbo_Through.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        txt_OrderNo.Enabled = True
        txt_OrderNo.BackColor = Color.White

        txt_Order_Date.Enabled = True
        txt_Order_Date.BackColor = Color.White

        txt_DelvAdd1.Enabled = True
        txt_DelvAdd1.BackColor = Color.White

        txt_DelvAdd2.Enabled = True
        txt_DelvAdd2.BackColor = Color.White

        txt_Lr_Date.Enabled = True
        txt_Lr_Date.BackColor = Color.White

        txt_LrNo.Enabled = True
        txt_LrNo.BackColor = Color.White

        cbo_ClothName.Enabled = True
        cbo_ClothName.BackColor = Color.White

        txt_Freight.Enabled = True
        txt_Freight.BackColor = Color.White

        txt_Noofbundle.Enabled = True
        txt_Noofbundle.BackColor = Color.White

        txt_freightPerBundle.Enabled = True
        txt_freightPerBundle.BackColor = Color.White

        dgv_Details.ReadOnly = False

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

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
        dgv_ActiveCtrl_Name = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.SpringGreen
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
        'dgv_Details.CurrentCell.Selected = False
        'dgv_Details_Total.CurrentCell.Selected = False
        'dgv_Filter_Details.CurrentCell.Selected = False

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub ClothSales_Cloth_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Cloth_PcsDelivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Cloth_PcsDelivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                    'ElseIf pnl_BaleSelection.Visible = True Then
                    '    btn_Close_BaleSelection_Click(sender, e)
                    '    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Cloth_PcsDelivery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where ( Ledger_IdNo = 0 or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_PartyName.DataSource = dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Transport.DataSource = dt2
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_Agent.DataSource = dt3
        cbo_Agent.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt4)
        cbo_ClothName.DataSource = dt4
        cbo_ClothName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Despatch_To) from ClothSales_Delivery_Head order by Despatch_To", con)
        da.Fill(dt6)
        cbo_DespTo.DataSource = dt6
        cbo_DespTo.DisplayMember = "Despatch_To"


        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("ORDER")

        cbo_Through.Items.Clear()
        cbo_Through.Items.Add(" ")
        cbo_Through.Items.Add("DIRECT")
        cbo_Through.Items.Add("BANK")
        cbo_Through.Items.Add("AGENT")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        'pnl_BaleSelection.Visible = False
        'pnl_BaleSelection.Left = (Me.Width - pnl_BaleSelection.Width) \ 2
        'pnl_BaleSelection.Top = (Me.Height - pnl_BaleSelection.Height) \ 2
        'pnl_BaleSelection.BringToFront()

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DespTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Through.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Order_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Lr_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BaleNos.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NetAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateAndTimeOFSupply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Noofbundle.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_freightPerBundle.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DespTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Through.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Lr_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Order_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BaleNos.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateAndTimeOFSupply.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NetAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Noofbundle.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_freightPerBundle.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DelvAdd1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Folding.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BaleNos.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LrNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Lr_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Order_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NetAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DateAndTimeOFSupply.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Noofbundle.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_freightPerBundle.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DelvAdd1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Lr_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Folding.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BaleNos.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LrNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Order_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NetAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DateAndTimeOFSupply.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Noofbundle.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_freightPerBundle.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim Pr_kyData As Keys

        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details
            ElseIf dgv_ActiveCtrl_Name = dgv_Details.Name Then
                dgv1 = dgv_Details
            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            Pr_kyData = Prev_kyData
            Prev_kyData = keyData

            If IsNothing(dgv1) = False Then

                With dgv1

                    ' If (keyData = Keys.Enter Or keyData = Keys.Down Or keyData = 131085) Then

                    ' If keyData = Keys.Enter Or keyData = Keys.Down Then
                    If (keyData = Keys.Enter Or keyData = Keys.Down Or keyData = 131085) Then

                        If .CurrentCell.ColumnIndex >= 10 Or Pr_kyData = 131089 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                cbo_Transport.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                            End If
                        Else

                            If Trim(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> "" Then

                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                            Else

                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                            End If

                            'If .CurrentCell.RowIndex <> .RowCount - 1 And .CurrentCell.ColumnIndex = 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            '    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)
                            'ElseIf .CurrentCell.ColumnIndex = 2 And Trim(.CurrentRow.Cells(2).Value) = "" Then
                            '    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                            'Else
                            '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            'End If
                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 0 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_DelvAdd2.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(10)

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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False
        Dim n As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from ClothSales_Delivery_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_DcNo.Text = dt1.Rows(0).Item("ClothSales_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("ClothSales_Delivery_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_ClothName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_Type.Text = dt1.Rows(0).Item("Order_Type").ToString
                cbo_DespTo.Text = dt1.Rows(0).Item("Despatch_To").ToString
                cbo_Through.Text = dt1.Rows(0).Item("Through_Name").ToString

                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                txt_Lr_Date.Text = dt1.Rows(0).Item("Lr_Date").ToString
                txt_Order_Date.Text = dt1.Rows(0).Item("Party_OrderDate").ToString
                txt_OrderNo.Text = dt1.Rows(0).Item("Party_OrderNo").ToString
                txt_DelvAdd1.Text = dt1.Rows(0).Item("Delivery_Address1").ToString
                txt_DelvAdd2.Text = dt1.Rows(0).Item("Delivery_Address2").ToString
                txt_Freight.Text = dt1.Rows(0).Item("Freight_Amount").ToString
                txt_Noofbundle.Text = dt1.Rows(0).Item("No_Of_Bundle").ToString
                txt_freightPerBundle.Text = dt1.Rows(0).Item("Freight_per_Bundle").ToString

                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_Folding.Text = dt1.Rows(0).Item("Folding").ToString
                txt_BaleNos.Text = dt1.Rows(0).Item("Bale_Nos").ToString
                lbl_TotalMeters.Text = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "##########0.00")
                lbl_TotalPcs.Text = Format(Val(dt1.Rows(0).Item("Total_Pcs").ToString), "##########0.00")

                lbl_TaxableMeter.Text = Format(Val(dt1.Rows(0).Item("Taxable_Meter").ToString), "##########0.00")
                txt_Rate.Text = Format(Val(dt1.Rows(0).Item("Rate").ToString), "##########0.00")
                txt_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "##########0.00")
                txt_DateAndTimeOFSupply.Text = Trim(dt1.Rows(0).Item("Date_And_Time_Of_Supply").ToString)

                chk_Sample.Checked = False
                If Val(dt1.Rows(0).Item("Sample_Status").ToString) = 1 Then chk_Sample.Checked = True

                chk_Invoice_In_100_Folding.Checked = False
                If Val(dt1.Rows(0).Item("Invoice_In_100_Folding_Status").ToString) = 1 Then chk_Invoice_In_100_Folding.Checked = True

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                da2 = New SqlClient.SqlDataAdapter("Select a.* , B.Invoice_Meters AS InvMeters from ClothSales_PcsDelivery_Details a INNER JOIN ClothSales_Delivery_Details B ON  a.ClothSales_Delivery_Code =  B.ClothSales_Delivery_Code Where a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            .Rows(n).Cells(0).Value = dt2.Rows(i).Item("sERIAL_nO").ToString
                            .Rows(n).Cells(1).Value = Format(Val(dt2.Rows(i).Item("Pcs1").ToString), "##########0.00")
                            .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Pcs2").ToString), "#########0.00")
                            .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Pcs3").ToString), "#########0.00")
                            .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Pcs4").ToString), "##########0.00")
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Pcs5").ToString), "##########0.00")
                            .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Pcs6").ToString), "###########0.00")
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Pcs7").ToString), "###########0.00")
                            .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Pcs8").ToString), "##########0.00")
                            .Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Pcs9").ToString), "#########0.00")
                            .Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Pcs10").ToString), "#########0.00")
                            .Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            .Rows(n).Cells(12).Value = dt2.Rows(i).Item("ClothSales_Order_Code").ToString
                            .Rows(n).Cells(13).Value = dt2.Rows(i).Item("ClothSales_Order_SlNo").ToString
                            .Rows(n).Cells(14).Value = dt2.Rows(i).Item("ClothSales_Delivery_SlNo").ToString

                            .Rows(n).Cells(15).Value = dt2.Rows(i).Item("InvMeters").ToString
                            '.Rows(n).Cells(16).Value = dt2.Rows(i).Item("PackingSlip_Codes").ToString


                            If Val(.Rows(n).Cells(15).Value) <> 0 Then
                                For j = 0 To .ColumnCount - 1
                                    .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If


                        Next i

                    End If

                    If .Rows.Count = 0 Then
                        .Rows.Add()

                    Else

                        n = .Rows.Count - 1
                        If Trim(.Rows(n).Cells(1).Value) = "" And Val(.Rows(n).Cells(11).Value) = 0 Then
                            .Rows(n).Cells(14).Value = ""
                            If Val(.Rows(n).Cells(14).Value) = 0 Then
                                If n = 0 Then
                                    .Rows(n).Cells(14).Value = 1
                                Else
                                    .Rows(n).Cells(14).Value = Val(.Rows(n - 1).Cells(14).Value) + 1
                                End If
                            End If
                        End If

                    End If

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(11).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                End With


                'da2 = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Delivery_DetailsSlNo, a.Delivery_No, a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                'With dgv_BaleDetails

                '    .Rows.Clear()
                '    SNo = 0

                '    If dt2.Rows.Count > 0 Then

                '        For i = 0 To dt2.Rows.Count - 1

                '            n = .Rows.Add()

                '            SNo = SNo + 1

                '            .Rows(n).Cells(0).Value = Val(dt2.Rows(i).Item("Delivery_DetailsSlNo").ToString)
                '            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Packing_Slip_No").ToString
                '            .Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                '            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Total_Meters").ToString)
                '            .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Weight").ToString)
                '            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Packing_Slip_Code").ToString
                '            .Rows(n).Cells(6).Value = dt2.Rows(i).Item("Bale_Bundle").ToString

                '        Next i

                '    End If

                'End With


            End If

            Grid_Cell_DeSelect()

            If LockSTS = True Then
                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_Agent.Enabled = False
                cbo_Agent.BackColor = Color.LightGray

                cbo_DespTo.Enabled = False
                cbo_DespTo.BackColor = Color.LightGray

                cbo_Through.Enabled = False
                cbo_Through.BackColor = Color.LightGray

                cbo_Transport.Enabled = False
                cbo_Transport.BackColor = Color.LightGray

                txt_OrderNo.Enabled = False
                txt_OrderNo.BackColor = Color.LightGray

                txt_Order_Date.Enabled = False
                txt_Order_Date.BackColor = Color.LightGray

                txt_DelvAdd1.Enabled = False
                txt_DelvAdd1.BackColor = Color.LightGray

                txt_DelvAdd2.Enabled = False
                txt_DelvAdd2.BackColor = Color.LightGray

                txt_Lr_Date.Enabled = False
                txt_Lr_Date.BackColor = Color.LightGray

                txt_LrNo.Enabled = False
                txt_LrNo.BackColor = Color.LightGray

                cbo_ClothName.Enabled = False
                cbo_ClothName.BackColor = Color.LightGray

                txt_Freight.Enabled = False
                txt_Freight.BackColor = Color.LightGray

                txt_Noofbundle.Enabled = False
                txt_Noofbundle.BackColor = Color.LightGray

                txt_freightPerBundle.Enabled = False
                txt_freightPerBundle.BackColor = Color.LightGray

                dgv_Details.ReadOnly = True

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, New_Entry, Me, con, "ClothSales_Delivery_Head", "ClothSales_Delivery_Code", NewCode, "ClothSales_Delivery_Date", "(ClothSales_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select sum(Invoice_Meters) from ClothSales_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some pieces invoiced for this order", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction
        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "ClothSales_Delivery_head", "ClothSales_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "ClothSales_Delivery_Code, Company_IdNo, for_OrderBy", trans)

        Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "ClothSales_PcsDelivery_Details", "ClothSales_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "   sERIAL_nO,Pcs1,Pcs2,Pcs3 ,Pcs4 ,Pcs5,Pcs6,Pcs7,Pcs8,Pcs9,Pcs10,Meters,ClothSales_Order_code ,ClothSales_Order_SlNo  ,ClothSales_Delivery_Slno", "Sl_No", "ClothSales_Delivery_Code, For_OrderBy, Company_IdNo, ClothSales_Delivery_No, ClothSales_Delivery_Date, Ledger_Idno", trans)

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            'cmd.CommandText = "Update ClothSales_order_Details set Delivery_Meters = a.Delivery_Meters - b.Meters from ClothSales_order_Details a, ClothSales_Delivery_Details b Where b.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' and a.ClothSales_Order_code = b.ClothSales_Order_code and a.ClothSales_Order_SlNo = b.ClothSales_Order_SlNo"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '', Delivery_No = '', Delivery_DetailsSlNo = 0, Delivery_Increment = Delivery_Increment - 1, Delivery_Date = Null Where Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Update ClothSales_order_Details set Delivery_Meters = a.Delivery_Meters - b.Meters from ClothSales_order_Details a, ClothSales_PcsDelivery_Details b Where b.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' and a.ClothSales_Order_code = b.ClothSales_Order_code and a.ClothSales_Order_SlNo = b.ClothSales_Order_SlNo"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "delete from ClothSales_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ClothSales_PcsDelivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'"
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

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Delivery_No from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ClothSales_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Delivery_No from ClothSales_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ClothSales_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Delivery_No from ClothSales_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothSales_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Delivery_No from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothSales_Delivery_No desc", con)
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

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "ClothSales_Delivery_Head", "ClothSales_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_DcNo.ForeColor = Color.Red
            msk_Date.Text = Date.Today.ToShortDateString
            Da = New SqlClient.SqlDataAdapter("select top 1 * from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothSales_Delivery_nO desc", con)
            Dt1 = New DataTable
            da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                    If Dt1.Rows(0).Item("ClothSales_Delivery_Date").ToString <> "" Then msk_Date.Text = Dt1.Rows(0).Item("ClothSales_Delivery_Date").ToString
                End If
            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Dc No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select ClothSales_Delivery_No from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to DELETE", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW Dc NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select ClothSales_Delivery_No from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(Pk_Condition) & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

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
        Dim clth_ID As Integer = 0
        Dim FP_ID As Integer = 0
        Dim clthtyp_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim Led_ID As Integer = 0
        Dim Ag_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vTotBals As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim Smp_STS As Integer = 0
        Dim Inv_In_100Folding_STS As Integer = 0
        Dim Nr As Integer = 0
        Dim OrdCd As String = ""
        Dim OrdSlNo As Long = 0
        Dim DcCd As String = ""
        Dim DcSlNo As Long = 0
        Dim OpYrCode As String = ""
        Dim Usr_ID As Integer = 0
        Dim vOrdByNo As String = ""
        Dim vSELC_DCCODE As String

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)



        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, New_Entry, Me, con, "ClothSales_Delivery_Head", "ClothSales_Delivery_Code", NewCode, "ClothSales_Delivery_Date", "(ClothSales_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, ClothSales_Delivery_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, New_Entry) = False Then Exit Sub


       
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
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        'lbl_UserName.Text = (Common_Procedures.User.IdNo)

        Ag_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        If clth_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then
            txt_Folding.Text = 100
        End If

        clthtyp_ID = 1

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then

                'clth_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                'If clth_ID = 0 Then
                '    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                '    End If
                '    Exit Sub
                'End If

                'clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(i).Cells(2).Value)
                'If clthtyp_ID = 0 Then
                '    MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                '    End If
                '    Exit Sub
                'End If

                'If Val(dgv_Details.Rows(i).Cells(3).Value) = 0 Then
                '    MessageBox.Show("Invalid Folding", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                '    End If
                '    Exit Sub
                'End If

                If Val(dgv_Details.Rows(i).Cells(11).Value) = 0 Then
                    MessageBox.Show("Invalid metres", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(11)
                    End If
                    Exit Sub
                End If

            End If

        Next

        NoCalc_Status = False
        Total_Calculation()

        vTotPcs = 0 : vTotMtrs = 0 : vTotBals = 0

        If dgv_Details_Total.RowCount > 0 Then
            'vTotBals = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            'vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If

        For i = 0 To dgv_Details.Rows.Count - 1
            If dgv_Details.Rows(i).Cells(11).Value <> 0 Then
                vTotBals = vTotBals + 1
            End If
        Next

        'If vTotMtrs = 0 Then
        '    MessageBox.Show("Invalid METERS", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If dgv_Details.Enabled And dgv_Details.Visible Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(11)
        '    End If
        '    Exit Sub
        'End If

        Smp_STS = 0
        If chk_Sample.Checked = True Then Smp_STS = 1

        Inv_In_100Folding_STS = 0
        If chk_Invoice_In_100_Folding.Checked = True Then Inv_In_100Folding_STS = 1


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "ClothSales_Delivery_Head", "ClothSales_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            vSELC_DCCODE = Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DeliveryDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                If Trim(txt_DateAndTimeOFSupply.Text) = "" Then txt_DateAndTimeOFSupply.Text = Format(Now, "dd-MM-yyyy")

                cmd.CommandText = "Insert into ClothSales_Delivery_Head ( ClothSales_Delivery_Code ,               Company_IdNo       ,     ClothSales_Delivery_No  ,                     for_OrderBy                                                           , ClothSales_Delivery_Date  ,              Ledger_IdNo,    Order_Type                  ,      Party_OrderNo                  ,    Party_OrderDate               ,        Through_Name            ,     Agent_IdNo          ,    Lr_No                         ,          Lr_Date                   ,   Despatch_To                   , Sample_Status                  ,  Delivery_Address1             , Delivery_Address2               ,       Transport_IdNo            ,No_Of_Bundle                             ,Freight_per_Bundle                        ,      Freight_Amount                   ,        Note                    ,               Total_Pcs,             Total_Meters  ,                        Cloth_IdNo                     ,       Total_Bales          ,        Folding       ,         Bale_Nos                              ,            Taxable_Meter          ,             Rate           ,             Net_Amount          ,               Date_And_Time_Of_Supply        ,    Invoice_In_100_Folding_Status  ,user_idno  , ClothSales_DeliveryCode_forSelection ) " &
                                    "     Values                     (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",       @DeliveryDate               , " & Str(Val(Led_ID)) & ",  '" & Trim(cbo_Type.Text) & "' ,  '" & Trim(txt_OrderNo.Text) & "'  , '" & Trim(txt_Order_Date.Text) & "', '" & Trim(cbo_Through.Text) & "', " & Str(Val(Ag_ID)) & ", '" & Trim(txt_LrNo.Text) & "', '" & Trim(txt_Lr_Date.Text) & "'    , '" & Trim(UCase(cbo_DespTo.Text)) & "',     " & Str(Val(Smp_STS)) & "  , '" & Trim(txt_DelvAdd1.Text) & "', '" & Trim(txt_DelvAdd2.Text) & "',        " & Str(Val(Trans_ID)) & ",   " & Str(Val(txt_Noofbundle.Text)) & "," & Str(Val(txt_freightPerBundle.Text)) & "," & Str(Val(txt_Freight.Text)) & "  , '" & Trim(txt_Note.Text) & "' , " & Str(Val(lbl_TotalPcs.Text)) & ", " & Str(Val(lbl_TotalMeters.Text)) & ",  " & Str(Val(clth_ID)) & " , " & Str(Val(vTotBals)) & " ,  " & Str(Val(txt_Folding.Text)) & ", '" & Trim(txt_BaleNos.Text) & "'," & Val(lbl_TaxableMeter.Text) & " , " & Val(txt_Rate.Text) & " , " & Val(txt_NetAmount.Text) & " , '" & Trim(txt_DateAndTimeOFSupply.Text) & "' , " & Val(Inv_In_100Folding_STS) & "  , " & Val(Common_Procedures.User.IdNo) & ",     '" & Trim(vSELC_DCCODE) & "'    ) "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "ClothSales_Delivery_head", "ClothSales_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "ClothSales_Delivery_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "ClothSales_PcsDelivery_Details", "ClothSales_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "   sERIAL_nO,Pcs1,Pcs2,Pcs3 ,Pcs4 ,Pcs5,Pcs6,Pcs7,Pcs8,Pcs9,Pcs10,Meters,ClothSales_Order_code ,ClothSales_Order_SlNo  ,ClothSales_Delivery_Slno", "Sl_No", "ClothSales_Delivery_Code, For_OrderBy, Company_IdNo, ClothSales_Delivery_No, ClothSales_Delivery_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update ClothSales_Delivery_Head set ClothSales_Delivery_Date = @DeliveryDate, Ledger_IdNo =  " & Str(Val(Led_ID)) & " ,    Order_Type = '" & Trim(cbo_Type.Text) & "' , Party_OrderNo =  '" & Trim(txt_OrderNo.Text) & "',     Party_OrderDate = '" & Trim(txt_Order_Date.Text) & "' ,          Through_Name = '" & Trim(cbo_Through.Text) & "'              ,     Agent_IdNo = " & Str(Val(Ag_ID)) & "    ,   Lr_No    = '" & Trim(txt_LrNo.Text) & "'       ,   Lr_Date    = '" & Trim(txt_Lr_Date.Text) & "'  ,   Despatch_To = '" & Trim(UCase(cbo_DespTo.Text)) & "', Sample_Status =   " & Str(Val(Smp_STS)) & " ,     Transport_IdNo = " & Str(Val(Trans_ID)) & "       ,  Delivery_Address1 = '" & Trim(txt_DelvAdd1.Text) & "', Delivery_Address2 = '" & Trim(txt_DelvAdd2.Text) & "'  ,No_Of_Bundle = " & Str(Val(txt_Noofbundle.Text)) & "   ,Freight_per_Bundle  = " & Str(Val(txt_freightPerBundle.Text)) & "   ,Freight_Amount =  " & Str(Val(txt_Freight.Text)) & "  ,  Note = '" & Trim(txt_Note.Text) & "' ,  Total_Bales  = " & Str(Val(vTotBals)) & " , Taxable_Meter = " & Val(lbl_TaxableMeter.Text) & "  , Rate = " & Val(txt_Rate.Text) & " , Net_Amount = " & Val(txt_NetAmount.Text) & " ,Date_And_Time_Of_Supply ='" & Trim(txt_DateAndTimeOFSupply.Text) & "' ,    Total_Pcs =   " & Str(Val(lbl_TotalPcs.Text)) & ", Total_Meters = " & Str(Val(lbl_TotalMeters.Text)) & " , Cloth_IdNo = " & Str(Val(clth_ID)) & " , Folding = " & Str(Val(txt_Folding.Text)) & "  , Bale_Nos = '" & Trim(txt_BaleNos.Text) & "',   Invoice_In_100_Folding_Status  = " & Val(Inv_In_100Folding_STS) & " ,user_idno  = " & Val(Common_Procedures.User.IdNo) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update ClothSales_order_Details set Delivery_Meters = a.Delivery_Meters - b.Meters from ClothSales_order_Details a, ClothSales_Delivery_Details b Where b.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' and a.ClothSales_Order_code = b.ClothSales_Order_code and a.ClothSales_Order_SlNo = b.ClothSales_Order_SlNo"
                'cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '', Delivery_No = '', Delivery_DetailsSlNo = 0, Delivery_Increment = Delivery_Increment - 1, Delivery_Date = Null Where Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                'cmd.ExecuteNonQuery()
                cmd.CommandText = "Update ClothSales_order_Details set Delivery_Meters = a.Delivery_Meters - b.Meters from ClothSales_order_Details a, ClothSales_PcsDelivery_Details b Where b.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' and a.ClothSales_Order_code = b.ClothSales_Order_code and a.ClothSales_Order_SlNo = b.ClothSales_Order_SlNo"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "ClothSales_Delivery_head", "ClothSales_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "ClothSales_Delivery_Code, Company_IdNo, for_OrderBy", tr)

            

            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)
            Partcls = "ClothSales : Dc.No. " & Trim(lbl_DcNo.Text)

            cmd.CommandText = "Delete from ClothSales_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'and Invoice_Meters = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from ClothSales_PcsDelivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(11).Value) <> 0 Then

                        Sno = Sno + 1

                        'clth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        'clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        'OrdCd = ""
                        'OrdSlNo = 0
                        'If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
                        '    OrdCd = Trim(.Rows(i).Cells(8).Value)
                        '    OrdSlNo = Val(.Rows(i).Cells(9).Value)
                        'End If


                        cmd.CommandText = "Insert into ClothSales_PcsDelivery_Details ( ClothSales_Delivery_Code ,               Company_IdNo       ,   ClothSales_Delivery_No    ,                     for_OrderBy                                        , ClothSales_Delivery_Date       ,            Ledger_IdNo        ,         Sl_No         ,  sERIAL_nO     ,                       Pcs1              ,                 Pcs2                    ,                   Pcs3           ,                       Pcs4                 ,                        Pcs5                  ,                   Pcs6               ,                       Pcs7                    ,          Pcs8         ,                      Pcs9                                     ,    Pcs10             ,                             Meters              ,  ClothSales_Order_code ,   ClothSales_Order_SlNo  ,              ClothSales_Delivery_Slno     ) " & _
                                            "     Values                        (   '" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",       @DeliveryDate            ,      " & Str(Val(Led_ID)) & " , " & Str(Val(Sno)) & "    , '" & Trim(.Rows(i).Cells(0).Value) & "', " & Str(Val(.Rows(i).Cells(1).Value)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ",  " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & "," & Str(Val(.Rows(i).Cells(6).Value)) & ",   " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ",  " & Str(Val(.Rows(i).Cells(9).Value)) & ",  " & Str(Val(.Rows(i).Cells(10).Value)) & ",  " & Str(Val(.Rows(i).Cells(11).Value)) & ",   '" & Trim(OrdCd) & "'  , " & Str(Val(OrdSlNo)) & ", " & Str(Val(.Rows(i).Cells(14).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                        If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
                            Nr = 0
                            cmd.CommandText = "Update ClothSales_Order_Details set Delivery_Meters = Delivery_Meters + " & Str(Val(.Rows(i).Cells(11).Value)) & " Where ClothSales_Order_code = '" & Trim(.Rows(i).Cells(12).Value) & "' and ClothSales_Order_SlNo = " & Str(Val(.Rows(i).Cells(13).Value)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Order and Party Details")
                            End If
                        End If


                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "ClothSales_PcsDelivery_Details", "ClothSales_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "   sERIAL_nO,Pcs1,Pcs2,Pcs3 ,Pcs4 ,Pcs5,Pcs6,Pcs7,Pcs8,Pcs9,Pcs10,Meters,ClothSales_Order_code ,ClothSales_Order_SlNo  ,ClothSales_Delivery_Slno", "Sl_No", "ClothSales_Delivery_Code, For_OrderBy, Company_IdNo, ClothSales_Delivery_No, ClothSales_Delivery_Date, Ledger_Idno", tr)

            End With

            Nr = 0
            cmd.CommandText = "Update  ClothSales_Delivery_Details set ClothSales_Delivery_Date = @DeliveryDate , Ledger_IdNo = " & Str(Val(Led_ID)) & ", Sl_No  = " & Str(Val(1)) & " , Cloth_IdNo = " & Str(Val(clth_ID)) & " , ClothType_IdNo = " & Str(Val(clthtyp_ID)) & " , Fold_Perc =  " & Str(Val(txt_Folding.Text)) & ", Bales = 0 ,   Bales_Nos = '',      Pcs   = " & Str(Val(lbl_TotalPcs.Text)) & ",  Meters = " & Str(Val(lbl_TotalMeters.Text)) & "  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "' and ClothSales_Delivery_SlNo = 1 "
            Nr = cmd.ExecuteNonQuery()

            If Nr = 0 Then

                cmd.CommandText = "Insert into ClothSales_Delivery_Details ( ClothSales_Delivery_Code ,               Company_IdNo       ,   ClothSales_Delivery_No    ,                     for_OrderBy                                        , ClothSales_Delivery_Date       ,            Ledger_IdNo        ,         Sl_No       ,          Cloth_IdNo          ,      ClothType_IdNo         ,                   Fold_Perc              ,                 Bales   ,   Bales_Nos   ,                       Pcs                 ,                      Meters             ,  ClothSales_Order_code ,   ClothSales_Order_SlNo  , ClothSales_Delivery_SlNo   ) " & _
                                    "     Values                        (   '" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",       @DeliveryDate            ,      " & Str(Val(Led_ID)) & " , " & Str(Val(1)) & ", " & Str(Val(clth_ID)) & "   , " & Str(Val(clthtyp_ID)) & "," & Str(Val(txt_Folding.Text)) & "          ,            0           ,      ''        ,  " & Str(Val(lbl_TotalPcs.Text)) & "      , " & Str(Val(lbl_TotalMeters.Text)) & "  , ''  , 0      , 1                          ) "
                cmd.ExecuteNonQuery()
            End If

            cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                             , Reference_Date,                                    StockOff_IdNo          ,     DeliveryTo_Idno     ,                            ReceivedFrom_Idno              ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      , Sl_No,           Cloth_Idno     ,                      Folding      ,   Meters_Type1                         ) " & _
                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",  @DeliveryDate, " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(clth_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(lbl_TotalMeters.Text)) & " ) "
            cmd.ExecuteNonQuery()

            'With dgv_BaleDetails

            '    Sno = 0
            '    For i = 0 To .RowCount - 1

            '        If Val(.Rows(i).Cells(3).Value) <> 0 And Trim(.Rows(i).Cells(5).Value) <> "" Then

            '            cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "', Delivery_No = '" & Trim(lbl_DcNo.Text) & "', Delivery_DetailsSlNo = " & Str(Val(.Rows(i).Cells(0).Value)) & ", Delivery_Increment = Delivery_Increment + 1, Delivery_Date = @DeliveryDate Where Packing_Slip_Code = '" & Trim(.Rows(i).Cells(5).Value) & "'"
            '            cmd.ExecuteNonQuery()

            '        End If

            '    Next i

            'End With

            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempSubTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Name1, Int1, Int2, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select a.ClothSales_Delivery_Code, a.ClothSales_Delivery_SlNo, a.Cloth_IdNo, a.Fold_Perc, (CASE WHEN a.ClothType_IdNo = 1 THEN a.Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 2 THEN a.Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 3 THEN a.Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 4 THEN a.Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 5 THEN a.Meters ELSE 0 END) from ClothSales_Delivery_Details a where a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Name1, Int1, Int2, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select tC.ClothSales_Delivery_Code, tC.ClothSales_Delivery_SlNo, tC.Cloth_IdNo, tC.Fold_Perc, -1*(CASE WHEN tC.ClothType_IdNo = 1 THEN a.Total_Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 2 THEN a.Total_Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 3 THEN a.Total_Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 4 THEN a.Total_Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 5 THEN a.Total_Meters ELSE 0 END) from Packing_Slip_Head a, ClothSales_Delivery_Details tC where tC.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' and a.Packing_Slip_Code LIKE '%/" & Trim(OpYrCode) & "' and a.Delivery_Code = '" & Trim(Pk_Condition) & "' + tC.ClothSales_Delivery_Code and a.Delivery_DetailsSlNo = tC.ClothSales_Delivery_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Name1, Int1, Int2, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select tC.ClothSales_Delivery_Code, tC.ClothSales_Delivery_SlNo, tC.Cloth_IdNo, tC.Fold_Perc, -1*(CASE WHEN tC.ClothType_IdNo = 1 THEN b.Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 2 THEN b.Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 3 THEN b.Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 4 THEN b.Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 5 THEN b.Meters ELSE 0 END) from Packing_Slip_Head a, Packing_Slip_Details b, ClothSales_Delivery_Details tC where tC.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' and a.Packing_Slip_Code NOT LIKE '%/" & Trim(OpYrCode) & "' and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code and a.Delivery_Code = '" & Trim(Pk_Condition) & "' + tC.ClothSales_Delivery_Code and a.Delivery_DetailsSlNo = tC.ClothSales_Delivery_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select Int2, Weight1, sum(Meters1), sum(Meters2), sum(Meters3), sum(Meters4), sum(Meters5) from " & Trim(Common_Procedures.EntryTempSubTable) & " group by Int2, Weight1 "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select a.Cloth_IdNo, a.Folding, (CASE WHEN a.ClothType_IdNo = 1 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 2 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 3 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 4 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 5 THEN a.Total_Meters ELSE 0 END) from Packing_Slip_Head a where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Packing_Slip_Code LIKE '%/" & Trim(OpYrCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select b.Cloth_IdNo, b.Folding, (CASE WHEN b.ClothType_IdNo = 1 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 2 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 3 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 4 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 5 THEN b.Meters ELSE 0 END) from Packing_Slip_Head a, Packing_Slip_Details b where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Packing_Slip_Code NOT LIKE '%/" & Trim(OpYrCode) & "' and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code"
            cmd.ExecuteNonQuery()

            Da = New SqlClient.SqlDataAdapter("select Int1 as Clo_IdNo, Weight1 as FoldPerc, sum(Meters1 ) as Type1_Mtrs, sum(Meters2) as Type2_Mtrs, sum(Meters3) as Type3_Mtrs, sum(Meters4) as Type4_Mtrs, sum(Meters5) as Type5_Mtrs from " & Trim(Common_Procedures.EntryTempTable) & " group by Int1, Weight1 having sum(Meters1) <> 0 or sum(Meters2) <> 0 or sum(Meters3) <> 0 or sum(Meters4) <> 0 or sum(Meters5) <> 0 ", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            'Sno = 0
            'If Dt1.Rows.Count > 0 Then
            '    For i = 0 To Dt1.Rows.Count - 1
            '        Sno = Sno + 1

            '        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                             , Reference_Date ,                                            StockOff_IdNo  ,      DeliveryTo_Idno    ,                              ReceivedFrom_Idno            ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No      ,                         Cloth_Idno                     ,                          Folding                       ,                         Meters_Type1                     ,                         Meters_Type2                     ,                         Meters_Type3                     ,                         Meters_Type4                     ,                         Meters_Type5                          ) " & _
            '                                        " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",  @DeliveryDate, " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("Clo_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("FoldPerc").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type1_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type2_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type3_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type4_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type5_Mtrs").ToString)) & "     ) "
            '        cmd.ExecuteNonQuery()

            '    Next
            'End If

    
            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            vLed_IdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)

            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)

            If Common_Procedures.Voucher_Updation(con, "Clo.PcsDlv", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_DcNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_DcNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If
            tr.Commit()

            move_record(lbl_DcNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

        End Try

    End Sub


    Private Sub Total_Calculation()
        Dim TotPcs As Single
        Dim TotMtrs As Single
        Dim fldmtr As Double = 0
        Dim fmt As Double = 0
        Dim CloID As Integer = 0
        Dim total_mtr As Integer = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim StkIn_For As String = ""
        Dim mtr_pcs As Single = 0
        Dim flperc As String = 0

        If NoCalc_Status = True Then Exit Sub


        TotMtrs = 0

        With dgv_Details
            For i = 0 To .RowCount - 1

                '.Rows(i).Cells(0).Value = Sno

                If Val(.Rows(i).Cells(1).Value) <> 0 Or Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Or Val(.Rows(i).Cells(9).Value) <> 0 Or Val(.Rows(i).Cells(10).Value) <> 0 Then

                    If Val(.Rows(i).Cells(1).Value) <> 0 Then
                        TotPcs = TotPcs + 1
                    End If

                    If Val(.Rows(i).Cells(2).Value) <> 0 Then
                        TotPcs = TotPcs + 1
                    End If

                    If Val(.Rows(i).Cells(3).Value) <> 0 Then
                        TotPcs = TotPcs + 1
                    End If
                    If Val(.Rows(i).Cells(4).Value) <> 0 Then
                        TotPcs = TotPcs + 1
                    End If
                    If Val(.Rows(i).Cells(5).Value) <> 0 Then
                        TotPcs = TotPcs + 1
                    End If
                    If Val(.Rows(i).Cells(6).Value) <> 0 Then
                        TotPcs = TotPcs + 1
                    End If
                    If Val(.Rows(i).Cells(7).Value) <> 0 Then
                        TotPcs = TotPcs + 1
                    End If
                    If Val(.Rows(i).Cells(8).Value) <> 0 Then
                        TotPcs = TotPcs + 1
                    End If
                    If Val(.Rows(i).Cells(9).Value) <> 0 Then
                        TotPcs = TotPcs + 1
                    End If
                    If Val(.Rows(i).Cells(10).Value) <> 0 Then
                        TotPcs = TotPcs + 1
                    End If

                    .Rows(i).Cells(11).Value = Format(Val(.Rows(i).Cells(1).Value) + Val(.Rows(i).Cells(2).Value) + Val(.Rows(i).Cells(3).Value) + Val(.Rows(i).Cells(4).Value) + Val(.Rows(i).Cells(5).Value) + Val(.Rows(i).Cells(6).Value) + Val(.Rows(i).Cells(7).Value) + Val(.Rows(i).Cells(8).Value) + Val(.Rows(i).Cells(9).Value) + Val(.Rows(i).Cells(10).Value), "########0.00")

                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(11).Value())

                    ' TotPcs = Val(TotPcs1) + Val(TotPcs2) + Val(TotPcs3) + Val(TotPcs4) + Val(TotPcs5) + Val(TotPcs6) + Val(TotPcs7) + Val(TotPcs8) + Val(TotPcs9) + Val(TotPcs10)

                End If

            Next i

        End With

        lbl_TotalPcs.Text = Val(TotPcs)
        lbl_TotalMeters.Text = Format(Val(TotMtrs), "########0.00")

        If Val(txt_Folding.Text) = 0 Or Val(txt_Folding.Text) = 100 Then


            lbl_TaxableMeter.Text = lbl_TotalMeters.Text
            txt_NetAmount.Text = Format(Val(lbl_TaxableMeter.Text) * Val(txt_Rate.Text), "#########0.00")

        Else

            'fmt = ((100 - Val(txt_Folding.Text)) / 100) * Val(lbl_TotalMeters.Text)

            ''Dim VTotMtrs As Double 'Val(lbl_TotalMeters.Text)

            ''VTotMtrs = (Val(lbl_TotalMeters.Text) * Val(txt_Folding.Text)) / 100

            ''fmt = (Val(VTotMtrs) - Val(lbl_TotalMeters.Text))

            total_mtr = 0

            flperc = Format(100 - Val(txt_Folding.Text), "##########0.00")


            fmt = Val(lbl_TotalMeters.Text) * Val(flperc) / 100

            fmt = Format(Math.Abs(Val(fmt)), "######0.00")



            If (100 - Val(txt_Folding.Text)) > 0 Then

                fldmtr = Format(Val(lbl_TotalMeters.Text) - Val(fmt), "#########0.000")
                lbl_TaxableMeter.Text = Val(fldmtr)
            Else
                fldmtr = Format(Val(lbl_TotalMeters.Text) + Val(fmt), "#########0.000")
                lbl_TaxableMeter.Text = Val(fldmtr)
            End If




            'If (100 - Val(txt_Folding.Text)) > 0 Then

            '    '  total_mtr = Format(Val(lbl_TotalMeters.Text) - Val(fmt), "#########0.000")
            '    lbl_TaxableMeter.Text = Val(total_mtr)
            'Else
            '    total_mtr = Int(Val(lbl_TotalMeters.Text) + Val(fmt))
            '    ' total_mtr = Format(Val(lbl_TotalMeters.Text) + Val(fmt), "#########0.00")
            '    lbl_TaxableMeter.Text = Val(total_mtr)
            'End If


            txt_NetAmount.Text = Format(Val(lbl_TaxableMeter.Text) * Val(txt_Rate.Text), "#########0.00")

        End If


        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()

            .Rows(0).Cells(11).Value = Format(Val(TotMtrs), "########0.00")
        End With

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_Date, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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

    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, cbo_Through, txt_LrNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_LrNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_PartyName, txt_OrderNo, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, txt_OrderNo, "", "", "", "")
        If Asc(e.KeyChar) = 13 And cbo_Type.Text = "ORDER" Then
            If MessageBox.Show("Do you want to select order:", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                txt_OrderNo.Focus()

            End If

        End If


    End Sub

    Private Sub cbo_DespTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DespTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Delivery_Head", "Despatch_To", "", "")

    End Sub

    Private Sub cbo_DespTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DespTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DespTo, cbo_ClothName, txt_Folding, "ClothSales_Delivery_Head", "Despatch_To", "", "")

    End Sub

    Private Sub cbo_DespTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DespTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DespTo, txt_Folding, "ClothSales_Delivery_Head", "Despatch_To", "", "", False)
    End Sub

    Private Sub cbo_Through_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Through.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Through_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Through.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Through, txt_Order_Date, cbo_Agent, "", "", "", "")

    End Sub

    Private Sub cbo_Through_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Through.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Through, cbo_Agent, "", "", "", "", False)

    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, Nothing, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        If e.KeyCode = 38 And cbo_Transport.DroppedDown = False Or (e.Control = True And e.KeyValue = 38) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_DelvAdd2.Focus()
            End If
        End If

        If e.KeyValue = 40 And cbo_Transport.DroppedDown = False And (e.Control = True And e.KeyValue = 40) Then
            If txt_Noofbundle.Visible And txt_Noofbundle.Enabled Then
                txt_Noofbundle.Focus()
            Else
                txt_Freight.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If txt_Noofbundle.Visible And txt_Noofbundle.Enabled Then
                txt_Noofbundle.Focus()
            ElseIf txt_Freight.Visible And txt_Freight.Enabled Then
                txt_Freight.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
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

    Private Sub cbo_Grid_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.GotFocus
        vCbo_ItmNm = Trim(cbo_ClothName.Text)

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyDown

        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, txt_Lr_Date, cbo_DespTo, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")


    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, cbo_DespTo, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim N As Integer
        With dgv_Details

            N = e.RowIndex
            If N > 0 Then
                If .Rows(N).Cells(0).Value = 0 Then
                    .Rows(N).Cells(0).Value = Val(.Rows(N - 1).Cells(0).Value) + 1
                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Then
                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_ActiveCtrl_Name = dgv_Details.Name
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            vcbo_KeyDwnVal = e.KeyValue

            'If e.KeyCode = Keys.Left Then
            '    If .CurrentCell.ColumnIndex <= 1 Then
            '        If .CurrentCell.RowIndex = 0 Then
            '            txt_DelvAdd2.Focus()
            '        Else
            '            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
            '        End If
            '    End If
            'End If

            'If e.KeyCode = Keys.Right Then
            '    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
            '        If .CurrentCell.RowIndex >= .Rows.Count - 1 Then
            '            cbo_Transport.Focus()
            '        Else
            '            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
            '        End If
            '    End If
            'End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details

                If Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) = 0 Then

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    Total_Calculation()

                End If

            End With

        End If

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    btn_BaleSelection_Click(sender, e)
        'End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 1

        With dgv_Details

            '  n = .RowCount

            'If n > 1 Then
            '    .Rows(n - 1).Cells(0).Value = Val(.Rows(n - 2).Cells(0).Value) + 1
            'End If


            If Val(.Rows(e.RowIndex).Cells(14).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(14).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(14).Value = Val(.Rows(e.RowIndex - 1).Cells(14).Value) + 1
                End If
            End If

        End With

    End Sub

    Private Sub txt_DelvAdd2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DelvAdd2.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)

            Else
                cbo_Transport.Focus()

            End If
        End If
    End Sub

    Private Sub txt_DelvAdd2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DelvAdd2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)

            Else
                cbo_Transport.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then
            txt_Freight.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
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
                Condt = "a.ClothSales_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.ClothSales_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothSales_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Clth_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Val(Clth_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Cloth_IdNo = " & Str(Val(Clth_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name,  e.Ledger_Name from ClothSales_Delivery_Head a  left outer join Cloth_head c on a.Cloth_idno = c.Cloth_idno  left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.ClothSales_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by ClothSales_Delivery_Date, for_orderby, ClothSales_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("ClothSales_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ClothSales_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Party_OrderNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Total_Pcs").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")

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

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from ClothSales_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName ,e.Ledger_Name as Agent_Name , f.* , Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code , g.* from ClothSales_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo Left outer JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo INNER JOIN Cloth_Head f ON a.Cloth_idno = f.Cloth_idno left outer JOIN ItemGroup_Head G ON F.ItemGroup_IdNo = G.ItemGroup_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from ClothSales_PcsDelivery_Details a  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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

        ' Printing_Delivery_Format1(e)
        Printing_Bundle_Format2(e)
    End Sub

    'Public Sub Printing_Bundle()
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim dt1 As New DataTable
    '    Dim NewCode As String
    '    Dim ps As Printing.PaperSize
    '    Dim PpSzSTS As Boolean = False

    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try

    '        da1 = New SqlClient.SqlDataAdapter("select * from ClothSales_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
    '        dt1 = New DataTable
    '        da1.Fill(dt1)

    '        If dt1.Rows.Count <= 0 Then

    '            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Exit Sub

    '        End If

    '        dt1.Dispose()
    '        da1.Dispose()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try



    '    If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

    '        Try
    '            PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
    '            If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '                PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
    '                PrintDocument1.Print()
    '            End If

    '        Catch ex As Exception
    '            MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End Try


    '    Else
    '        Try

    '            Dim ppd As New PrintPreviewDialog

    '            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '                If ps.Width = 800 And ps.Height = 600 Then
    '                    PrintDocument1.DefaultPageSettings.PaperSize = ps
    '                    PpSzSTS = True
    '                    Exit For
    '                End If
    '            Next

    '            If PpSzSTS = False Then
    '                Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
    '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
    '                PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
    '            End If

    '            ppd.Document = PrintDocument1

    '            ppd.WindowState = FormWindowState.Normal
    '            ppd.StartPosition = FormStartPosition.CenterScreen
    '            ppd.ClientSize = New Size(600, 600)

    '            ppd.ShowDialog()

    '        Catch ex As Exception
    '            MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

    '        End Try

    '    End If

    'End Sub

    Private Sub Printing_Bundle_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        ' Dim ItmNm1 As String, ItmNm2 As String

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 900 And ps.Height = 1200 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

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
            .Left = 30
            .Right = 65
            .Top = 15 ' 30
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

        NoofItems_PerPage = 30 '35

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(60) : ClAr(2) = 60 : ClAr(3) = 60 : ClAr(4) = 60 : ClAr(5) = 60 : ClAr(6) = 60 : ClAr(7) = 60 : ClAr(8) = 60 : ClAr(9) = 60 : ClAr(10) = 60 : ClAr(11) = 60
        ClAr(12) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13))

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Bundle_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            'Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Bundle_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        'ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        'ItmNm2 = ""
                        'If Len(ItmNm1) > 25 Then
                        '    For I = 15 To 1 Step -1
                        '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        '    Next I
                        '    If I = 0 Then I = 25
                        '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        'End If

                        CurY = CurY + TxtHgt + 2
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Bale_").ToString), LMargin + 15, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("SERIAL_NO").ToString), LMargin + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs1").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs1").ToString), LMargin + ClAr(1) + ClAr(2) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs2").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs2").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs3").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs3").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs4").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs4").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs5").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs5").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs6").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs6").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs7").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs7").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs8").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs8").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs9").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs9").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs10").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs10").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 5, CurY, 1, 0, pFont)

                        End If


                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        'CurY = CurY + TxtHgt - 5
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY + TxtHgt, PageWidth, CurY + TxtHgt)

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Bundle_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Bundle_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1, w2 As Single
        Dim S1, s2 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name from ClothSales_PcsDelivery_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH DELIVERY SLIP", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothSales_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "AGENT", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "FOLDING % ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Folding").ToString & " - ( " & prn_HdDt.Rows(0).Item("Note").ToString & " )", LMargin + s2 + 30, CurY, 0, 0, pFont)


        'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "MARKING", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bale_Nos").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "RNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS1", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS2", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS3", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS4", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS5", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS6", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS7", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS8", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS9", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS10", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Bundle_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim vprn_BlNos As String = ""
        Dim C1 As Single
        Dim W1, w2 As Single
        Dim S1, s2 As Single

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
        W1 = e.Graphics.MeasureString("ORDER NO      : ", pFont).Width
        w2 = e.Graphics.MeasureString("DESP.TO       : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO            :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT     :  ", pFont).Width

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(5) = CurY


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
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))



        CurY = CurY + TxtHgt - 10

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS  :  " & Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL PCS : " & Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + 10, CurY, 0, 0, pFont)


        'CurY = CurY + TxtHgt - 10
        'If is_LastPage = True Then
        '    ' Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        '    ' Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)

        'End If

        'vprn_BlNos = ""
        'For i = 0 To prn_DetDt.Rows.Count - 1
        '    If Trim(prn_DetDt.Rows(i).Item("Bales_Nos").ToString) <> "" Then
        '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(i).Item("Bales_Nos").ToString
        '    End If
        'Next
        'Common_Procedures.Print_To_PrintDocument(e, "BALES NOS : " & vprn_BlNos, LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt + 10

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Printing_Bundle_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        ' Dim ItmNm1 As String, ItmNm2 As String

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 900 And ps.Height = 1200 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

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
            .Left = 30
            .Right = 65
            .Top = 15 ' 30
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

        NoofItems_PerPage = 33 '35

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(60) : ClAr(2) = 60 : ClAr(3) = 60 : ClAr(4) = 60 : ClAr(5) = 60 : ClAr(6) = 60 : ClAr(7) = 60 : ClAr(8) = 60 : ClAr(9) = 60 : ClAr(10) = 60 : ClAr(11) = 60
        ClAr(12) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13))

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Bundle_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            'Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Bundle_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        'ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        'ItmNm2 = ""
                        'If Len(ItmNm1) > 25 Then
                        '    For I = 15 To 1 Step -1
                        '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        '    Next I
                        '    If I = 0 Then I = 25
                        '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        'End If

                        CurY = CurY + TxtHgt + 2
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Bale_").ToString), LMargin + 15, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("SERIAL_NO").ToString), LMargin + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs1").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs1").ToString), LMargin + ClAr(1) + ClAr(2) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs2").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs2").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs3").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs3").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs4").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs4").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs5").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs5").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs6").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs6").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs7").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs7").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs8").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs8").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs9").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs9").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)

                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs10").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs10").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 5, CurY, 1, 0, pFont)

                        End If


                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        'CurY = CurY + TxtHgt - 5
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY + TxtHgt, PageWidth, CurY + TxtHgt)

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Bundle_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Bundle_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1, w2 As Single
        Dim S1, s2 As Single
        Dim strWidth As Single = 0
        Dim Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name from ClothSales_PcsDelivery_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        '  Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "Phone : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO : " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO : " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateNm = Cmp_StateNm & "   CODE : " & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        'p1Font = New Font("Calibri", 14, FontStyle.Bold)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
        '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        'End If

        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        CurY = CurY + TxtHgt
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight - 20
        'If Desc <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)
        'End If

        strWidth = e.Graphics.MeasureString(Trim(Cmp_Add1 & " " & Cmp_Add2), p1Font).Width
        If PrintWidth > strWidth Then
            If Trim(Cmp_Add1 & " " & Cmp_Add2) <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, pFont)
            End If

            NoofItems_PerPage = NoofItems_PerPage - 1

        Else

            If Cmp_Add1 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            End If
            If Cmp_Add2 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
            End If

        End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
            If PrintWidth > strWidth Then
                CurX = LMargin + (PrintWidth - strWidth) / 2
            Else
                CurX = LMargin
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
            strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        ' strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
        ' CurX = CurX + strWidth
        'Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        'strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        'CurX = CurX + strWidth
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "CLOTH DELIVERY SLIP", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
        W1 = e.Graphics.MeasureString("ORDER NO    : ", pFont).Width
            w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
            s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothSales_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Item_HSN_Code").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "AGENT", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "FOLDING % ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Folding").ToString & " - ( " & prn_HdDt.Rows(0).Item("Note").ToString & " )", LMargin + s2 + 30, CurY, 0, 0, pFont)


        'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "MARKING", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bale_Nos").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "RNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS1", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS2", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS3", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS4", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS5", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS6", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS7", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS8", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS9", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS10", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Bundle_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim vprn_BlNos As String = ""
        Dim C1 As Single
        Dim W1, w2 As Single
        Dim S1, s2 As Single

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
        W1 = e.Graphics.MeasureString("ORDER NO      : ", pFont).Width
        w2 = e.Graphics.MeasureString("DESP.TO       : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO            :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT     :  ", pFont).Width

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(5) = CurY


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
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))

        CurY = CurY + TxtHgt - 10

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS  :  " & Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "########0.00") & " ( In " & prn_HdDt.Rows(0).Item("Folding").ToString & " Cm))", PageWidth - 10, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL PCS : " & Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "RATE  :  " & Format(Val(prn_HdDt.Rows(0).Item("Rate").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(4) + ClAr(4), CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TAXABLE METERS : " & Format(Val(prn_HdDt.Rows(0).Item("Taxable_Meter").ToString), "########0.00") & " (In 100 Cm)", LMargin + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Dim NETAMT As Double = 0
        NETAMT = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "########0")
        Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT  :  " & Format(Val(NETAMT), "########0.00"), PageWidth - 10, CurY, 1, 0, p1Font)

        'CurY = CurY + TxtHgt - 10
        'If is_LastPage = True Then
        '    ' Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        '    ' Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)

        'End If

        'vprn_BlNos = ""
        'For i = 0 To prn_DetDt.Rows.Count - 1
        '    If Trim(prn_DetDt.Rows(i).Item("Bales_Nos").ToString) <> "" Then
        '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(i).Item("Bales_Nos").ToString
        '    End If
        'Next
        'Common_Procedures.Print_To_PrintDocument(e, "BALES NOS : " & vprn_BlNos, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt + 10

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bls As Single = 0
        Dim Ent_BlNos As String = ""
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Cloth_Name, d.Ledger_Name as agentname, e.Ledger_Name as Transportname,  g.ClothType_name, h.Bales as Ent_Bales, h.Bales_Nos as Ent_Bales_Nos, h.Pcs as Ent_Pcs, h.Meters as Ent_DcMeters from ClothSales_Order_Head a INNER JOIN Clothsales_Order_details b ON a.ClothSales_Order_Code = b.ClothSales_Order_Code INNER JOIN Cloth_Head c ON b.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head g ON b.ClothType_IdNo = g.ClothType_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN ClothSales_Delivery_Details h ON h.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' and b.ClothSales_Order_Code = h.ClothSales_Order_Code and b.ClothSales_Order_SlNo = h.ClothSales_Order_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Order_Meters - b.Order_Cancel_Meters - b.Delivery_Meters) > 0 or h.Meters > 0 ) order by a.ClothSales_Order_Date, a.for_orderby, a.ClothSales_Order_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()



                    Ent_Bls = 0
                    Ent_BlNos = ""
                    Ent_Pcs = 0
                    Ent_Mtrs = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Bales").ToString) = False Then
                        Ent_Bls = Val(Dt1.Rows(i).Item("Ent_Bales").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Bales_Nos").ToString) = False Then
                        Ent_BlNos = Dt1.Rows(i).Item("Ent_Bales_Nos").ToString
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
                        Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_DcMeters").ToString) = False Then
                        Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_DcMeters").ToString)
                    End If

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Order_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Order_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(4).Value = "SOUND"
                    .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Fold_Perc").ToString)
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Order_Pcs").ToString)
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString) - Val(Dt1.Rows(i).Item("Order_Cancel_Meters").ToString) - Val(Dt1.Rows(i).Item("Delivery_Meters").ToString) - Val(Dt1.Rows(i).Item("Invoice_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")

                    If Ent_Mtrs > 0 Then
                        .Rows(n).Cells(8).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(8).Value = ""

                    End If

                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("agentname").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Transportname").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Through_Name").ToString
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Despatch_To").ToString
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Delivery_Address1").ToString
                    .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Delivery_Address2").ToString
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Clothsales_Order_Code").ToString
                    .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Clothsales_Order_SlNo").ToString

                    .Rows(n).Cells(17).Value = Val(Ent_Bls)
                    .Rows(n).Cells(18).Value = Ent_BlNos
                    .Rows(n).Cells(19).Value = Ent_Pcs
                    .Rows(n).Cells(20).Value = Ent_Mtrs

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

                .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

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
        Cloth_Invoice_Selection()
    End Sub

    Private Sub Cloth_Invoice_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then


                txt_OrderNo.Text = dgv_Selection.Rows(i).Cells(1).Value
                txt_Order_Date.Text = dgv_Selection.Rows(i).Cells(2).Value
                cbo_Agent.Text = dgv_Selection.Rows(i).Cells(9).Value
                cbo_Through.Text = dgv_Selection.Rows(i).Cells(11).Value
                cbo_DespTo.Text = dgv_Selection.Rows(i).Cells(12).Value
                cbo_Transport.Text = dgv_Selection.Rows(i).Cells(10).Value
                cbo_ClothName.Text = dgv_Selection.Rows(i).Cells(3).Value
                txt_Folding.Text = dgv_Selection.Rows(i).Cells(5).Value
                If txt_DelvAdd1.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(13).Value) <> "" Then
                        txt_DelvAdd1.Text = dgv_Selection.Rows(i).Cells(13).Value
                    End If
                End If

                If txt_DelvAdd2.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(14).Value) <> "" Then
                        txt_DelvAdd2.Text = dgv_Selection.Rows(i).Cells(14).Value
                    End If
                End If

                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                'dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(3).Value
                ' dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(4).Value
                'dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(5).Value

                'If Val(dgv_Selection.Rows(i).Cells(17).Value) <> 0 Then
                '    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(17).Value
                'End If
                'dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(18).Value

                'If Val(dgv_Selection.Rows(i).Cells(19).Value) <> 0 Then
                '    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(19).Value
                'Else
                '    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
                'End If

                If Val(dgv_Selection.Rows(i).Cells(20).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(20).Value
                Else
                    dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(11).Value
                End If

                dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(15).Value
                dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(16).Value


            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_LrNo.Enabled And txt_LrNo.Visible Then txt_LrNo.Focus()

    End Sub

    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
            dgv_Details.AllowUserToAddRows = False
        Else
            dgv_Details.AllowUserToAddRows = True
        End If
    End Sub

    'Private Sub btn_BaleSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim i As Integer, j As Integer, n As Integer, SNo As Integer
    '    Dim Clo_ID As Integer, CloType_ID As Integer
    '    Dim NewCode As String
    '    Dim Fd_Perc As Integer
    '    Dim CompIDCondt As String
    '    Dim dgvDet_CurRow As Integer
    '    Dim dgv_DetSlNo As Long

    '    Try

    '        If dgv_Details.CurrentCell.RowIndex < 0 Then
    '            MessageBox.Show("Invalid Cloth Name & Type Selection", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            If dgv_Details.Enabled And dgv_Details.Visible Then
    '                If dgv_Details.Rows.Count > 0 Then
    '                    dgv_Details.Focus()
    '                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
    '                    dgv_Details.CurrentCell.Selected = True
    '                End If
    '            End If
    '            Exit Sub
    '        End If

    '        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value)
    '        If Clo_ID = 0 Then
    '            MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            If dgv_Details.Enabled And dgv_Details.Visible Then
    '                If dgv_Details.Rows.Count > 0 Then
    '                    dgv_Details.Focus()
    '                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
    '                    If cbo_ClothName.Visible And cbo_ClothName.Enabled Then cbo_ClothName.Focus()
    '                    'dgv_Details.CurrentCell.Selected = True
    '                    Exit Sub
    '                End If
    '            End If
    '            Exit Sub
    '        End If

    '        CloType_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(2).Value)
    '        If CloType_ID = 0 Then
    '            MessageBox.Show("Invalid Cloth Type ", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            If dgv_Details.Enabled And dgv_Details.Visible Then
    '                If dgv_Details.Rows.Count > 0 Then
    '                    dgv_Details.Focus()
    '                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
    '                    If cbo_Grid_Clothtype.Visible And cbo_Grid_Clothtype.Enabled Then cbo_Grid_Clothtype.Focus()
    '                    Exit Sub
    '                End If
    '            End If
    '            Exit Sub
    '        End If

    '        Fd_Perc = Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(3).Value)
    '        If Val(Fd_Perc) = 0 Then
    '            MessageBox.Show("Invalid Folding", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            If dgv_Details.Enabled And dgv_Details.Visible Then
    '                If dgv_Details.Rows.Count > 0 Then
    '                    dgv_Details.Focus()
    '                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
    '                    dgv_Details.CurrentCell.Selected = True
    '                End If
    '            End If
    '            Exit Sub
    '        End If

    '        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
    '        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
    '            CompIDCondt = ""
    '        End If

    '        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '        dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
    '        dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(10).Value)

    '        With dgv_BaleSelection

    '            .Rows.Clear()
    '            SNo = 0

    '            Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_DetailsSlNo = " & Str(Val(dgv_DetSlNo)) & " and a.Cloth_IdNo = " & Str(Val(Clo_ID)) & "  and a.ClothType_IdNo = " & Str(Val(CloType_ID)) & "  and a.Folding = " & Str(Val(Fd_Perc)) & " order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No, a.Packing_Slip_Code", con)
    '            Dt1 = New DataTable
    '            Da.Fill(Dt1)

    '            If Dt1.Rows.Count > 0 Then

    '                For i = 0 To Dt1.Rows.Count - 1

    '                    n = .Rows.Add()

    '                    SNo = SNo + 1
    '                    .Rows(n).Cells(0).Value = Val(SNo)
    '                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Packing_Slip_No").ToString
    '                    If Val(Dt1.Rows(i).Item("Total_Pcs").ToString) <> 0 Then
    '                        .Rows(n).Cells(2).Value = Val(Dt1.Rows(i).Item("Total_Pcs").ToString)
    '                    End If
    '                    If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
    '                        .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
    '                    End If
    '                    If Val(Dt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
    '                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
    '                    End If
    '                    .Rows(n).Cells(5).Value = "1"
    '                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Packing_Slip_Code").ToString
    '                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Bale_Bundle").ToString

    '                    For j = 0 To .ColumnCount - 1
    '                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
    '                    Next

    '                Next

    '            End If
    '            Dt1.Clear()

    '            Da = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Head a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Delivery_Code = '' and a.Cloth_IdNo = " & Str(Val(Clo_ID)) & "  and a.ClothType_IdNo = " & Str(Val(CloType_ID)) & "  and a.Folding = " & Str(Val(Fd_Perc)) & " order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No, a.Packing_Slip_Code", con)
    '            Dt1 = New DataTable
    '            Da.Fill(Dt1)

    '            If Dt1.Rows.Count > 0 Then

    '                For i = 0 To Dt1.Rows.Count - 1

    '                    n = .Rows.Add()

    '                    SNo = SNo + 1
    '                    .Rows(n).Cells(0).Value = Val(SNo)
    '                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Packing_Slip_No").ToString
    '                    If Val(Dt1.Rows(i).Item("Total_Pcs").ToString) <> 0 Then
    '                        .Rows(n).Cells(2).Value = Val(Dt1.Rows(i).Item("Total_Pcs").ToString)
    '                    End If
    '                    If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
    '                        .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
    '                    End If
    '                    If Val(Dt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
    '                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
    '                    End If
    '                    .Rows(n).Cells(5).Value = ""
    '                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Packing_Slip_Code").ToString
    '                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Bale_Bundle").ToString

    '                Next

    '            End If
    '            Dt1.Clear()


    '        End With

    '        pnl_BaleSelection.Visible = True
    '        pnl_Back.Enabled = False
    '        dgv_BaleSelection.Focus()
    '        If dgv_BaleSelection.Rows.Count > 0 Then
    '            dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(0).Cells(0)
    '            dgv_BaleSelection.CurrentCell.Selected = True
    '        End If

    '    Catch ex As NullReferenceException
    '        MessageBox.Show("Select the ClothName for Bale Selection", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try



    'End Sub

    '    Private Sub dgv_BaleSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BaleSelection.CellClick
    '        Select_Bale(e.RowIndex)
    '    End Sub

    '    Private Sub Select_Bale(ByVal RwIndx As Integer)
    '        Dim i As Integer

    '        With dgv_BaleSelection

    '            If .RowCount > 0 And RwIndx >= 0 Then

    '                .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

    '                If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then .Rows(RwIndx).Cells(5).Value = ""

    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
    '                Next

    '            End If

    '        End With

    '    End Sub

    '    Private Sub dgv_BaleSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BaleSelection.KeyDown
    '        On Error Resume Next

    '        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
    '            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
    '                Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
    '                e.Handled = True
    '            End If
    '        End If

    '        If e.KeyCode = Keys.Delete Or e.KeyCode = Keys.Back Then
    '            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
    '                If Val(dgv_BaleSelection.Rows(dgv_BaleSelection.CurrentCell.RowIndex).Cells(5).Value) = 1 Then
    '                    e.Handled = True
    '                    Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
    '                End If
    '            End If
    '        End If

    '    End Sub

    '    Private Sub btn_Close_BaleSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_BaleSelection.Click
    '        Dim Cmd As New SqlClient.SqlCommand
    '        Dim Da1 As New SqlClient.SqlDataAdapter
    '        Dim Dt1 As New DataTable
    '        Dim I As Integer, J As Integer
    '        Dim n As Integer
    '        Dim sno As Integer
    '        Dim dgvDet_CurRow As Integer = 0
    '        Dim dgv_DetSlNo As Integer = 0
    '        Dim NoofBls As Integer
    '        Dim FsNo As Single, LsNo As Single
    '        Dim FsBaleNo As String, LsBaleNo As String
    '        Dim BlNo As String, PackSlpCodes As String
    '        Dim Tot_Pcs As Single, Tot_Mtrs As Single


    '        Cmd.Connection = con

    '        dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
    '        dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(10).Value)

    '        With dgv_BaleDetails

    'LOOP1:
    '            For I = 0 To .RowCount - 1

    '                If Val(.Rows(I).Cells(0).Value) = Val(dgv_DetSlNo) Then

    '                    If I = .Rows.Count - 1 Then
    '                        For J = 0 To .ColumnCount - 1
    '                            .Rows(I).Cells(J).Value = ""
    '                        Next

    '                    Else
    '                        .Rows.RemoveAt(I)

    '                    End If

    '                    GoTo LOOP1

    '                End If

    '            Next I

    '            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
    '            Cmd.ExecuteNonQuery()

    '            NoofBls = 0 : Tot_Pcs = 0 : Tot_Mtrs = 0 : BlNo = "" : PackSlpCodes = ""

    '            For I = 0 To dgv_BaleSelection.RowCount - 1

    '                If Val(dgv_BaleSelection.Rows(I).Cells(5).Value) = 1 Then

    '                    n = .Rows.Add()

    '                    sno = sno + 1
    '                    .Rows(n).Cells(0).Value = Val(dgv_DetSlNo)
    '                    .Rows(n).Cells(1).Value = dgv_BaleSelection.Rows(I).Cells(1).Value
    '                    .Rows(n).Cells(2).Value = Val(dgv_BaleSelection.Rows(I).Cells(2).Value)
    '                    .Rows(n).Cells(3).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(3).Value), "#########0.00")
    '                    .Rows(n).Cells(4).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(4).Value), "#########0.000")
    '                    .Rows(n).Cells(5).Value = dgv_BaleSelection.Rows(I).Cells(6).Value
    '                    .Rows(n).Cells(6).Value = dgv_BaleSelection.Rows(I).Cells(7).Value

    '                    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) values ('" & Trim(dgv_BaleSelection.Rows(I).Cells(6).Value) & "', '" & Trim(dgv_BaleSelection.Rows(I).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_BaleSelection.Rows(I).Cells(1).Value))) & " ) "
    '                    Cmd.ExecuteNonQuery()

    '                    NoofBls = NoofBls + 1
    '                    Tot_Pcs = Val(Tot_Pcs) + Val(dgv_BaleSelection.Rows(I).Cells(2).Value)
    '                    Tot_Mtrs = Val(Tot_Mtrs) + Val(dgv_BaleSelection.Rows(I).Cells(3).Value)
    '                    PackSlpCodes = Trim(PackSlpCodes) & IIf(Trim(PackSlpCodes) = "", "~", "") & Trim(dgv_BaleSelection.Rows(I).Cells(6).Value) & "~"

    '                End If

    '            Next

    '            BlNo = ""
    '            FsNo = 0 : LsNo = 0
    '            FsBaleNo = "" : LsBaleNo = ""

    '            Da1 = New SqlClient.SqlDataAdapter("Select Name1 as Bale_Code, Name2 as Bale_No, Meters1 as fororderby_baleno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name2, Name1", con)
    '            Dt1 = New DataTable
    '            Da1.Fill(Dt1)

    '            If Dt1.Rows.Count > 0 Then

    '                FsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)
    '                LsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)

    '                FsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))
    '                LsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))

    '                For I = 1 To Dt1.Rows.Count - 1
    '                    If LsNo + 1 = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString) Then
    '                        LsNo = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString)
    '                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

    '                    Else
    '                        If FsNo = LsNo Then
    '                            BlNo = BlNo & Trim(FsBaleNo) & ","
    '                        Else
    '                            BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo) & ","
    '                        End If
    '                        FsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString
    '                        LsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString

    '                        FsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))
    '                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

    '                    End If

    '                Next

    '                If FsNo = LsNo Then BlNo = BlNo & Trim(FsBaleNo) Else BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo)

    '            End If
    '            Dt1.Clear()

    '            If Trim(dgv_Details.Rows(dgvDet_CurRow).Cells(12).Value) <> "" Then
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = ""
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = ""
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = ""
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = ""
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(12).Value = ""
    '            End If
    '            If Val(NoofBls) <> 0 And Val(Tot_Mtrs) <> 0 Then
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = NoofBls
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = BlNo
    '                If Val(Tot_Pcs) <> 0 Then
    '                    dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = Val(Tot_Pcs)
    '                End If
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = Format(Val(Tot_Mtrs), "#########0.00")
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(12).Value = PackSlpCodes
    '            End If

    '            Add_NewRow_ToGrid()

    '            Total_Calculation()

    '        End With

    '        pnl_Back.Enabled = True
    '        pnl_BaleSelection.Visible = False
    '        If dgv_Details.Enabled And dgv_Details.Visible Then
    '            If dgv_Details.Rows.Count > 0 Then
    '                dgv_Details.Focus()
    '                If dgv_Details.CurrentCell.RowIndex >= 0 Then
    '                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(6)
    '                    dgv_Details.CurrentCell.Selected = True
    '                End If
    '            End If
    '        End If

    '    End Sub

    Private Sub Add_NewRow_ToGrid()
        On Error Resume Next

        Dim i As Integer
        Dim n As Integer = -1

        With dgv_Details
            If .Visible Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    If Trim(UCase(cbo_Type.Text)) <> "ORDER" Then
                        n = .Rows.Add()
                        'MessageBox.Show("New Added Row = " & n & "  -  Current Row = " & .CurrentCell.RowIndex)

                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = .Rows(.CurrentCell.RowIndex).Cells(i).Value
                            .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                        Next

                        For i = 0 To .Rows.Count - 1
                            .Rows(i).Cells(0).Value = i + 1
                        Next

                        .CurrentCell = .Rows(n).Cells(.CurrentCell.ColumnIndex)
                        .CurrentCell.Selected = True

                    End If
                End If

            End If

        End With

    End Sub

    Public Sub Printing_Bale()
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Packing_Slip_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("No Entry Found", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                        PrintDocument2.DefaultPageSettings.PaperSize = ps
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                            PrintDocument2.DefaultPageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

                PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument2.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                        PrintDocument2.DefaultPageSettings.PaperSize = ps
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                            PrintDocument2.DefaultPageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

                ppd.Document = PrintDocument2

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument2.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        pnl_Back.Enabled = True
        pnl_Print.Visible = False

    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""

        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 1
        prn_DetIndx = 0
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_Count = 1
        Erase prn_DetAr
        Erase prn_HdAr

        prn_HdAr = New String(100, 10) {}

        prn_DetAr = New String(100, 50, 10) {}

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, tZ.*, c.Cloth_Name from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                For i = 0 To prn_HdDt.Rows.Count - 1

                    prn_HdMxIndx = prn_HdMxIndx + 1

                    prn_HdAr(prn_HdMxIndx, 1) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                    prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)
                    prn_HdAr(prn_HdMxIndx, 3) = Val(prn_HdDt.Rows(i).Item("Total_Pcs").ToString)
                    prn_HdAr(prn_HdMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Total_Meters").ToString), "#########0.00")

                    prn_DetMxIndx = 0

                    da2 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                    If prn_DetDt.Rows.Count > 0 Then
                        For j = 0 To prn_DetDt.Rows.Count - 1
                            If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
                                prn_DetMxIndx = prn_DetMxIndx + 1

                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

                            End If
                        Next j
                    End If

                Next i

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Common_Procedures.Printing_PackingSlip_Format1(PrintDocument2, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            '---

        End Try
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_PartyName.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Note.Focus()
        End If

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

    Private Sub cbo_ClothName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.LostFocus
        Dim fldmtr As Double = 0
        Dim fmt As Double = 0
        Dim CloID As Integer = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim StkIn_For As String = ""
        Dim mtr_pcs As Single = 0

        CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        txt_Rate.Text = 0

        Da = New SqlClient.SqlDataAdapter("Select Sound_Rate from Cloth_Head Where Cloth_IdNo = " & Str(Val(CloID)), con)
        Dt2 = New DataTable
        Da.Fill(Dt2)
        If Dt2.Rows.Count > 0 Then
            txt_Rate.Text = Val(Dt2.Rows(0)("Sound_Rate").ToString)
        End If
        Dt2.Clear()

    End Sub

    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = "", AgPNo As String = ""
        Dim Led_IdNo As Integer = 0, Agnt_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim BlNos As String = ""
        Dim Ag_Name As String = ""

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            Agnt_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)

            AgPNo = ""
            If Val(Agnt_IdNo) <> 0 Then
                AgPNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_PhoneNo", "(Ledger_IdNo = " & Str(Val(Agnt_IdNo)) & ")")
                Ag_Name = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mainname", "(Ledger_IdNo = " & Str(Val(Agnt_IdNo)) & ")")
            End If

            If Trim(AgPNo) <> "" Then
                PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", ",", "") & Trim(AgPNo)
            End If

            smstxt = Trim(cbo_PartyName.Text) & Chr(13)
            smstxt = smstxt & " DC No : " & Trim(lbl_DcNo.Text) & Chr(13)
            smstxt = smstxt & " Date : " & Trim(msk_Date.Text) & Chr(13)
          
            If Trim((Ag_Name)) <> "" Then smstxt = smstxt & " No.Of Bundles : " & Trim((Ag_Name)) & Chr(13)
            smstxt = smstxt & " No.Of Bundles : " & Val((lbl_TotalPcs.Text)) & Chr(13)
            smstxt = smstxt & " Quality : " & Trim(cbo_ClothName.Text) & Chr(13)
            smstxt = smstxt & " Meters : " & Val(lbl_TotalMeters.Text) & Chr(13)
            smstxt = smstxt & " Billable Meter : " & Val(lbl_TaxableMeter.Text) & "(In 100 Cm)"
            '  End If
            'If dgv_Details.RowCount > 0 Then
            '    smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details.Rows(0).Cells(4).Value())) & Chr(13)
            '    smstxt = smstxt & " Meters : " & Val((dgv_Details.Rows(0).Cells(7).Value())) & Chr(13)
            'End If
            '  smstxt = smstxt & " Bill Amount : " & Trim(lbl_Net_Amt.Text) & Chr(13)
            smstxt = smstxt & " " & Chr(13)
            smstxt = smstxt & " Thanks! " & Chr(13)
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

    Private Sub txt_Noofbundle_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Noofbundle.TextChanged
        If NoCalc_Status = True Then Exit Sub
        txt_Freight.Text = Format(Val(txt_Noofbundle.Text) * Val(txt_freightPerBundle.Text), "###########0.00")
    End Sub

    Private Sub txt_freightPerBundle_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_freightPerBundle.TextChanged
        If NoCalc_Status = True Then Exit Sub
        txt_Freight.Text = Format(Val(txt_Noofbundle.Text) * Val(txt_freightPerBundle.Text), "###########0.00")
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub txt_Folding_TextChanged(sender As Object, e As EventArgs) Handles txt_Folding.TextChanged
        Total_Calculation()
    End Sub
End Class