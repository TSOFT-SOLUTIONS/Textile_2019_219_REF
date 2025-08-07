Public Class Own_Order
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "OWNOR-"
    Private dgv_ActiveCtrl_Name As String
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_SizingDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_WeavingDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_ProcessingDetails As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_HdAr(100, 10) As String
    Private prn_DetAr(100, 50, 10) As String
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private Print_PDF_Status As Boolean = False
    Private Filter_RowNo As Integer = -1
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False
        Print_PDF_Status = False
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1

        lbl_Po_No.Text = ""
        lbl_Po_No.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_Type.Text = "DIRECT"
        cbo_Quality.Text = ""
        txt_OrderNo.Text = ""
        lbl_FabricWftGrms.Text = ""
        lbl_WarpWt.Text = ""
        txt_Processing_ChargeMtr.Text = ""
        txt_WeavinChargeMtr.Text = ""
        txt_SizingCharge.Text = ""
        txt_OrderMtrs.Text = ""
        lbl_TotalWarpWgt.Text = ""
        lbl_Total_WeftWgt.Text = ""
        lbl_TotalYarnWgt.Text = ""
        lbl_WeftWgt.Text = ""
        txt_WarpRate.Text = ""
        txt_WeftRate.Text = ""
        lbl_ActualCost.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_Details.Rows.Clear()
        dgv_WeavingDetails.Rows.Clear()


        dgv_SizingDetails.Rows.Clear()
        dgv_processingdetails.Rows.Clear()


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_BuyerName.Text = ""
            cbo_Filter_BuyerName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()
        dgv_ActiveCtrl_Name = ""

        cbo_Count.Visible = False
        cbo_Count.Tag = -100
        cbo_Type.Visible = False
        cbo_Type.Tag = -100
        cbo_MillName.Visible = False
        cbo_MillName.Tag = -100
        cbo_PartyName.Visible = False
        cbo_PartyName.Tag = -100

        Cbo_WeaverName.Visible = False
        Cbo_WeaverName.Tag = -100

        cbo_Sizing_Name.Visible = False
        cbo_Sizing_Name.Tag = -100

        cbo_Process_Name.Visible = False
        cbo_Process_Name.Tag = -100

        cbo_Processor_Name.Visible = False
        cbo_Processor_Name.Tag = -100

        cbo_Quality.Enabled = True
        cbo_Quality.BackColor = Color.White


        Cbo_WeaverName.Enabled = True
        Cbo_WeaverName.BackColor = Color.White

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_Type.Enabled = True
        cbo_Type.BackColor = Color.White

        cbo_Count.Enabled = True
        cbo_Count.BackColor = Color.White

        cbo_MillName.Enabled = True
        cbo_MillName.BackColor = Color.White

       

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
        If Me.ActiveControl.Name <> cbo_Count.Name Then
            cbo_Count.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Type.Name Then
            cbo_Type.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_MillName.Name Then
            cbo_MillName.Visible = False
        End If

        If Me.ActiveControl.Name <> Cbo_WeaverName.Name Then
            Cbo_WeaverName.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_PartyName.Name Then
            cbo_PartyName.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Sizing_Name.Name Then
            cbo_Sizing_Name.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Process_Name.Name Then
            cbo_Process_Name.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Processor_Name.Name Then
            cbo_Processor_Name.Visible = False
        End If
        Grid_DeSelect()

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

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.SuppressKeyPress = True : e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.SuppressKeyPress = True : e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        dgv_WeavingDetails.CurrentCell.Selected = False
        dgv_SizingDetails.CurrentCell.Selected = False
        dgv_processingdetails.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Yarn_Purchase_Order_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Quality.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Quality.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Sizing_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Sizing_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_WeaverName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_WeaverName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Processor_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Processor_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Process_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PROCESS" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Process_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Own_Order_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = ""

        con.Open()

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("WARP")
        cbo_Type.Items.Add("WEFT")
        cbo_Type.Items.Add("BOTH")



        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WeavinChargeMtr.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Quality.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_WeaverName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Process_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Processor_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_WeaverName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus



        AddHandler txt_Processing_ChargeMtr.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SizingCharge.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WeftRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WarpRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderMtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_BuyerName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_lotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WeavinChargeMtr.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Quality.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_WeaverName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Process_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Processor_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_WeaverName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Processing_ChargeMtr.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SizingCharge.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WarpRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WeftRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderMtrs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_BuyerName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_lotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WarpRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WeavinChargeMtr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WeftRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SizingCharge.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderMtrs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown



        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WeavinChargeMtr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WarpRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SizingCharge.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WeftRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderMtrs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress


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

    Private Sub Own_Order_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Own_Order_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub



                Else
                    If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub
                    Else
                        Me.Close()
                    End If

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer

        If ActiveControl.Name = dgv_Details.Name Or ActiveControl.Name = dgv_SizingDetails.Name Or ActiveControl.Name = dgv_WeavingDetails.Name Or ActiveControl.Name = dgv_processingdetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Details.Name Then
                dgv1 = dgv_Details


            ElseIf ActiveControl.Name = dgv_SizingDetails.Name Then
                dgv1 = dgv_SizingDetails

            ElseIf dgv_SizingDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_SizingDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_SizingDetails.Name Then
                dgv1 = dgv_SizingDetails

            ElseIf ActiveControl.Name = dgv_WeavingDetails.Name Then
                dgv1 = dgv_WeavingDetails

            ElseIf dgv_WeavingDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_WeavingDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_WeavingDetails.Name Then
                dgv1 = dgv_WeavingDetails
            ElseIf ActiveControl.Name = dgv_processingdetails.Name Then
                dgv1 = dgv_processingdetails

            ElseIf dgv_processingdetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_processingdetails

            ElseIf dgv_ActiveCtrl_Name = dgv_processingdetails.Name Then
                dgv1 = dgv_processingdetails
            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1

                If dgv1.Name = dgv_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                'If dgv_SizingDetails.RowCount > 0 Then
                                '    dgv_SizingDetails.Focus()
                                '    dgv_SizingDetails.CurrentCell = dgv_SizingDetails.Rows(0).Cells(1)
                                'Else
                                '    If dgv_WeavingDetails.RowCount > 0 Then
                                '        dgv_WeavingDetails.Focus()
                                '        dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                                '    Else
                                '        If dgv_processingdetails.RowCount > 0 Then
                                '            dgv_processingdetails.Focus()
                                '            dgv_processingdetails.CurrentCell = dgv_processingdetails.Rows(0).Cells(1)
                                '        Else

                                '        End If
                                '    End If
                                'End If
                                If dgv_SizingDetails.Rows.Count = 0 Then dgv_SizingDetails.Rows.Add()
                                dgv_SizingDetails.Focus()
                                dgv_SizingDetails.CurrentCell = dgv_SizingDetails.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If
                       

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_Details.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next
                                If dgv_SizingDetails.Rows.Count = 0 Then dgv_SizingDetails.Rows.Add()
                                dgv_SizingDetails.Focus()
                                dgv_SizingDetails.CurrentCell = dgv_SizingDetails.Rows(0).Cells(1)

                                'If dgv_SizingDetails.RowCount > 0 Then
                                '    dgv_SizingDetails.Focus()
                                '    dgv_SizingDetails.CurrentCell = dgv_SizingDetails.Rows(0).Cells(1)
                                'Else
                                '    If dgv_WeavingDetails.RowCount > 0 Then
                                '        dgv_WeavingDetails.Focus()
                                '        dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                                '    Else
                                '        If dgv_processingdetails.RowCount > 0 Then
                                '            dgv_processingdetails.Focus()
                                '            dgv_processingdetails.CurrentCell = dgv_processingdetails.Rows(0).Cells(1)
                                '        Else
                                '            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                '                save_record()
                                '            Else
                                '                dtp_Date.Focus()
                                '            End If
                                '        End If
                                '    End If
                                'End If


                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If
                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                txt_Processing_ChargeMtr.Focus()

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



                ElseIf dgv1.Name = dgv_SizingDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If dgv_WeavingDetails.Rows.Count = 0 Then dgv_WeavingDetails.Rows.Add()
                                dgv_WeavingDetails.Focus()
                                dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                                'If dgv_WeavingDetails.RowCount > 0 Then
                                '    dgv_WeavingDetails.Focus()
                                '    dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                                'Else
                                '    If dgv_processingdetails.RowCount > 0 Then
                                '        dgv_processingdetails.Focus()
                                '        dgv_processingdetails.CurrentCell = dgv_processingdetails.Rows(0).Cells(1)
                                '    Else
                                '    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                '        save_record()
                                '    Else
                                '        dtp_Date.Focus()
                                '    End If
                                '    End If
                                'End If


                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


                            End If


                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)




                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If dgv_Details.RowCount > 0 Then
                                    dgv_Details.Focus()
                                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                                Else
                                    txt_Processing_ChargeMtr.Focus()

                                End If


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
                ElseIf dgv1.Name = dgv_WeavingDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then


                                
                                    If dgv_processingdetails.RowCount > 0 Then
                                        dgv_processingdetails.Focus()
                                        dgv_processingdetails.CurrentCell = dgv_processingdetails.Rows(0).Cells(1)
                                    Else
                                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                            save_record()
                                        Else
                                            dtp_Date.Focus()
                                        End If
                                    End If



                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


                            End If


                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)




                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If dgv_SizingDetails.RowCount > 0 Then
                                    dgv_SizingDetails.Focus()
                                    dgv_SizingDetails.CurrentCell = dgv_SizingDetails.Rows(0).Cells(1)
                                Else
                                    If dgv_Details.RowCount > 0 Then
                                        dgv_Details.Focus()
                                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                                    Else
                                        txt_Processing_ChargeMtr.Focus()

                                    End If
                                End If

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
                ElseIf dgv1.Name = dgv_processingdetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then



                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    dtp_Date.Focus()
                                End If
                            


                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


                        End If


                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)




                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If dgv_WeavingDetails.RowCount > 0 Then
                                    dgv_WeavingDetails.Focus()
                                    dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                                Else
                                    If dgv_SizingDetails.RowCount > 0 Then
                                        dgv_SizingDetails.Focus()
                                        dgv_SizingDetails.CurrentCell = dgv_SizingDetails.Rows(0).Cells(1)
                                    Else
                                        If dgv_Details.RowCount > 0 Then
                                            dgv_Details.Focus()
                                            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                                        Else
                                            txt_Processing_ChargeMtr.Focus()

                                        End If
                                    End If
                                End If
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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Own_Order_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Own_Order_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_Po_No.Text = dt1.Rows(0).Item("Own_Order_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Own_Order_Date").ToString
                msk_date.Text = dtp_Date.Text
                txt_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                cbo_Quality.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                lbl_FabricWftGrms.Text = Format(Val(dt1.Rows(0).Item("Fabric_Weight_Grams").ToString), "############0.000")
                lbl_WarpWt.Text = Format(Val(dt1.Rows(0).Item("Warp_Weight").ToString), "############0.000")
                lbl_WeftWgt.Text = Format(Val(dt1.Rows(0).Item("Weft_Weight").ToString), "############0.000")
                txt_OrderMtrs.Text = Format(Val(dt1.Rows(0).Item("Order_Meters").ToString), "############0.00")
                lbl_TotalWarpWgt.Text = Format(Val(dt1.Rows(0).Item("Total_Warp_Weight").ToString), "############0.000")
                lbl_Total_WeftWgt.Text = Format(Val(dt1.Rows(0).Item("Total_Weft_Weight").ToString), "############0.000")
                lbl_TotalYarnWgt.Text = Format(Val(dt1.Rows(0).Item("Total_Yarn_Weight").ToString), "############0.000")
                txt_WarpRate.Text = Format(Val(dt1.Rows(0).Item("Warp_Rate").ToString), "############0.00")
                txt_WeftRate.Text = Format(Val(dt1.Rows(0).Item("Weft_Rate").ToString), "############0.00")
                txt_SizingCharge.Text = Format(Val(dt1.Rows(0).Item("Sizing_Charge_Kg").ToString), "############0.000")
                txt_WeavinChargeMtr.Text = Format(Val(dt1.Rows(0).Item("Weaving_Charge_Kg").ToString), "############0.000")
                txt_Processing_ChargeMtr.Text = Format(Val(dt1.Rows(0).Item("Processing_Charge_Meter").ToString), "############0.00")
                lbl_ActualCost.Text = Format(Val(dt1.Rows(0).Item("Actual_Cost").ToString), "############0.00")

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Count_Name, c.Mill_Name  from Own_Order_Yarn_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNo = c.Mill_IdNo    Where a.Own_Order_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Warp_Weft_Type").ToString
                            .Rows(n).Cells(2).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(i).Item("Ledger_IdNo").ToString))
                            .Rows(n).Cells(3).Value = dt2.Rows(i).Item("Count_Name").ToString
                            .Rows(n).Cells(4).Value = (dt2.Rows(i).Item("Mill_NAme").ToString)
                            
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            
                            'If Val(dgv_Details.Rows(n).Cells(14).Value) <> 0 Or Val(dgv_Details.Rows(n).Cells(15).Value) <> 0 Or Val(dgv_Details.Rows(n).Cells(16).Value) <> 0 Then
                            '    For j = 0 To dgv_Details.ColumnCount - 1
                            '        dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            '    Next j
                            '    LockSTS = True
                            'End If

                        Next i

                    End If

                End With

               
                da2 = New SqlClient.SqlDataAdapter("Select a.*  from Own_Order_Sizing_Details a  Where a.Own_Order_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt3 = New DataTable
                da2.Fill(dt3)

                With dgv_SizingDetails

                    .Rows.Clear()
                    SNo = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt3.Rows(i).Item("Ledger_IdNo").ToString))
                            

                        Next i

                    End If

                End With

                da2 = New SqlClient.SqlDataAdapter("Select a.*  from Own_Order_Weaving_Details a  Where a.Own_Order_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt4 = New DataTable
                da2.Fill(dt4)

                With dgv_WeavingDetails

                    .Rows.Clear()
                    SNo = 0

                    If dt4.Rows.Count > 0 Then

                        For i = 0 To dt4.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt4.Rows(i).Item("Ledger_IdNo").ToString))


                        Next i

                    End If

                End With

                da2 = New SqlClient.SqlDataAdapter("Select a.*  from Own_Order_Processing_Details a  Where a.Own_Order_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt5 = New DataTable
                da2.Fill(dt5)

                With dgv_processingdetails

                    .Rows.Clear()
                    SNo = 0

                    If dt5.Rows.Count > 0 Then

                        For i = 0 To dt5.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt5.Rows(i).Item("Ledger_IdNo").ToString))
                            .Rows(n).Cells(2).Value = Common_Procedures.Process_IdNoToName(con, Val(dt5.Rows(i).Item("Process_IdNo").ToString))
                            .Rows(n).Cells(3).Value = Format(Val(dt5.Rows(i).Item("Rate").ToString), "########0.00")
                        Next i

                    End If

                End With




            End If

            If LockSTS = True Then


                cbo_Quality.Enabled = False
                cbo_Quality.BackColor = Color.LightGray

                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                Cbo_WeaverName.Enabled = False
                Cbo_WeaverName.BackColor = Color.LightGray

                cbo_Type.Enabled = False
                cbo_Type.BackColor = Color.LightGray

                cbo_Count.Enabled = False
                cbo_Count.BackColor = Color.LightGray

                cbo_MillName.Enabled = False
                cbo_MillName.BackColor = Color.LightGray

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dgv_ActiveCtrl_Name = ""
            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()
            dt3.Dispose()
            dt4.Dispose()
            dt5.Dispose()
            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Po_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        'Da = New SqlClient.SqlDataAdapter("select * from Own_Order_Yarn_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "' and  Receipt_Weight <> 0", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)
        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
        '        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("Already Purchased Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'Dt1.Clear()

        'Da = New SqlClient.SqlDataAdapter("select * from Own_Order_Yarn_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "' and  Delivery_Weight <> 0", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)
        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
        '        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("Already Purchased Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'Dt1.Clear()

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Po_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "delete from Own_Order_Yarn_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Own_Order_Sizing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Own_Order_Weaving_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Own_Order_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Own_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "'"
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



            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_BuyerName.Text = ""
            cbo_Filter_BuyerName.SelectedIndex = -1


            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False
        If Filter_Status = True Then
            If dgv_Filter_Details.Rows.Count > 0 And Filter_RowNo >= 0 Then
                dgv_Filter_Details.Focus()
                dgv_Filter_Details.CurrentCell = dgv_Filter_Details.Rows(Filter_RowNo).Cells(0)
                dgv_Filter_Details.CurrentCell.Selected = True
            Else
                If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

            End If

        Else
            If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

        End If

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Own_Order_No from Own_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Own_Order_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_Po_No.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Own_Order_No from Own_Order_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Own_Order_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_Po_No.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Own_Order_No from Own_Order_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Own_Order_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Own_Order_No from Own_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Own_Order_No desc", con)
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
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_Po_No.Text = Common_Procedures.get_MaxCode(con, "Own_Order_Head", "Own_Order_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_Po_No.ForeColor = Color.Red






        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally



            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter  Ref No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Own_Order_No from Own_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show(" Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New  Ref No.", "FOR NEW REF NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Own_Order_No from Own_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid  Ref No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_Po_No.Text = Trim(UCase(inpno))

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
        Dim Mill_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Clth_ID As Integer = 0
        Dim Siz_ID As Integer = 0
        Dim Wea_ID As Integer = 0
        Dim Procer_ID As Integer = 0
        Dim Proc_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Slno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim YnTy_ID As Integer = 0
        Dim Prc_ID As Integer = 0
        Dim YrnClthNm As String = ""
        Dim Nr As Integer = 0
        Dim PurCd As String = ""
        Dim PurSlNo As Long = 0
        Dim DcCd As String = ""
        Dim DcSlNo As Long = 0
        Dim PcsChkCode As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
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

        lbl_UserName.Text = Common_Procedures.User.IdNo


        Clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Quality.Text)

        If Clth_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Quality.Enabled Then cbo_Quality.Focus()
            Exit Sub
        End If

        


        For i = 0 To dgv_Details.RowCount - 1

            If Trim(dgv_Details.Rows(i).Cells(1).Value) <> "" Or Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, dgv_Details.Rows(i).Cells(2).Value)
                If Led_ID = 0 Then
                    MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                    End If
                    Exit Sub
                End If

                Cnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(i).Cells(3).Value)

                If Cnt_ID = 0 Then
                    MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                    End If
                    Exit Sub
                End If

                Mill_ID = Common_Procedures.Mill_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value)

                If Mill_ID = 0 Then
                    MessageBox.Show("Invalid Yarn Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)
                    End If
                    Exit Sub
                End If

               

                If Val(dgv_Details.Rows(i).Cells(5).Value) = 0 Then
                    MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(5)
                    End If
                    Exit Sub
                End If

               

            End If

        Next

       
      

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Po_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_Po_No.Text = Common_Procedures.get_MaxCode(con, "Own_Order_Head", "Own_Order_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Po_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If


            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(msk_date.Text))


            If New_Entry = True Then
                cmd.CommandText = "Insert into Own_Order_Head (   Own_Order_Code  ,               Company_IdNo       ,                        Own_Order_No    ,                       for_OrderBy                                         ,                 Own_Order_Date           ,Order_No                           ,      Cloth_IdNo       , Fabric_Weight_Grams                       ,  Warp_Weight                      ,   Weft_Weight           ,           Order_Meters             ,     Total_Warp_Weight           ,               Total_Weft_Weight          ,            Total_Yarn_Weight        , Warp_Rate                        ,  Weft_Rate                       , Sizing_Charge_Kg                       ,        Weaving_Charge_Kg             ,           Processing_Charge_Meter                       ,       Actual_Cost                   , User_IdNo ) " & _
                                    "     Values                   ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_Po_No.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Po_No.Text))) & ",                  @OrderDate                ,'" & Trim(txt_OrderNo.Text) & "' , " & Str(Val(Clth_ID)) & " ," & Str(Val(lbl_FabricWftGrms.Text)) & " , " & Val(lbl_WarpWt.Text) & " ," & Str(Val(lbl_WeftWgt.Text)) & " ,  " & Val(txt_OrderMtrs.Text) & ", " & Val(lbl_TotalWarpWgt.Text) & ", " & Val(lbl_Total_WeftWgt.Text) & "," & Val(lbl_TotalYarnWgt.Text) & ",  " & Str(Val(txt_WarpRate.Text)) & " ,  " & Val(txt_WeftRate.Text) & " ,  " & Str(Val(txt_SizingCharge.Text)) & "  ,  " & Str(Val(txt_WeavinChargeMtr.Text)) & "  , " & Str(Val(txt_Processing_ChargeMtr.Text)) & ",  " & Val(lbl_ActualCost.Text) & ", " & Val(lbl_UserName.Text) & " ) "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Own_Order_Head set Own_Order_Date = @OrderDate, Order_No = '" & Trim(txt_OrderNo.Text) & "'  , Cloth_IdNo =  " & Str(Val(Clth_ID)) & " ,Fabric_Weight_Grams = " & Str(Val(lbl_FabricWftGrms.Text)) & " , Warp_Weight = " & Val(lbl_WarpWt.Text) & " , Weft_Weight = " & Val(lbl_WeftWgt.Text) & " ,  Order_Meters =  " & Val(txt_OrderMtrs.Text) & ",   Total_WArp_Weight = " & Val(lbl_TotalWarpWgt.Text) & " ,Total_Weft_Weight = " & Val(lbl_Total_WeftWgt.Text) & " ,  Total_Yarn_Weight = " & Val(lbl_TotalYarnWgt.Text) & ",Warp_Rate = " & Str(Val(txt_WarpRate.Text)) & "   ,Weft_Rate = " & Val(txt_WeftRate.Text) & " , Sizing_Charge_Kg = " & Str(Val(txt_SizingCharge.Text)) & "   , Weaving_Charge_Kg = " & Str(Val(txt_WeavinChargeMtr.Text)) & " ,Processing_Charge_Meter =  " & Str(Val(txt_Processing_ChargeMtr.Text)) & " ,Actual_Cost = " & Val(lbl_ActualCost.Text) & ",User_IdNo =  " & Val(lbl_UserName.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_Po_No.Text)
            PBlNo = Trim(lbl_Po_No.Text)
            Partcls = "Order: Ref.No. " & Trim(lbl_Po_No.Text)

            cmd.CommandText = "Delete from Own_Order_Yarn_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Own_Order_Sizing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Own_Order_Weaving_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Own_Order_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0


                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)
                        'Clr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)
                        'Nr = 0
                        'cmd.CommandText = "Update Own_Order_Yarn_Details set Own_Order_Date = @OrderDate , Sl_No = " & Str(Val(Sno)) & " ,  Ledger_Idno = " & Val(Bye_ID) & ", YarnCount_IdNo = " & Str(Val(Cnt_ID)) & ", YarnType_IdNo = " & Val(YnTy_ID) & ", YarnMill_IdNo =  " & Val(Mill_ID) & ",  Colour_idNo = " & Val(Clr_ID) & ",  Order_Bags =  " & Val(.Rows(i).Cells(5).Value) & ",  Order_Cone = " & Val(.Rows(i).Cells(6).Value) & " ,order_Weight_Bags =" & Str(Val(.Rows(i).Cells(7).Value)) & " , Order_weight = " & Str(Val(.Rows(i).Cells(8).Value)) & ",Rate =" & Str(Val(.Rows(i).Cells(9).Value)) & "  , Amount =" & Str(Val(.Rows(i).Cells(10).Value)) & "  , Io_Style_Code = '" & Trim(.Rows(i).Cells(11).Value) & "' ,PO_Planning_Yarn_SlNo =  " & Str(Val(.Rows(i).Cells(13).Value)) & "   where Company_IdNo =  " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "' and Yarn_Purchase_Order_Slno = " & Str(Val(.Rows(i).Cells(12).Value)) & ""
                        'Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then

                            cmd.CommandText = "Insert into Own_Order_Yarn_Details (    Own_Order_Code ,               Company_IdNo       ,      Own_Order_No        ,                               for_OrderBy                                         ,     Own_Order_Date          ,     Cloth_Idno  ,      Sl_No           ,    Warp_Weft_Type                           ,     Ledger_IdNo      ,        Count_IdNo       ,     Mill_IdNo          ,  Rate                                   ) " & _
                                                    "     Values                  ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_Po_No.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Po_No.Text))) & ",    @OrderDate              ,  " & Val(Clth_ID) & " , " & Str(Val(Sno)) & ",'" & Trim(.Rows(i).Cells(1).Value) & "',   " & Str(Val(Led_ID)) & ",  " & Val(Cnt_ID) & ", " & Str(Val(Mill_ID)) & ",  " & Str(Val(.Rows(i).Cells(5).Value)) & "  ) "
                            cmd.ExecuteNonQuery()
                        End If


                    End If

                Next

            End With


            With dgv_SizingDetails

                Slno = 0


                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Slno = Slno + 1

                        Siz_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(1).Value, tr)


                        cmd.CommandText = "Insert into Own_Order_Sizing_Details (    Own_Order_Code ,               Company_IdNo       ,      Own_Order_No        ,                               for_OrderBy                                    ,     Own_Order_Date          ,     Cloth_IdNo          ,      Sl_No        ,     Ledger_IdNo     ) " & _
                                                "     Values                  ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_Po_No.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Po_No.Text))) & ",    @OrderDate              ,  " & Val(Clth_ID) & " , " & Str(Val(Slno)) & ",   " & Val(Siz_ID) & " ) "
                        cmd.ExecuteNonQuery()
                    End If




                Next

            End With


            With dgv_WeavingDetails

                Slno = 0


                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Slno = Slno + 1

                        Wea_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(1).Value, tr)


                        cmd.CommandText = "Insert into Own_Order_Weaving_Details (    Own_Order_Code ,               Company_IdNo       ,      Own_Order_No        ,                               for_OrderBy                                    ,     Own_Order_Date          ,     Cloth_IdNo          ,      Sl_No        ,     Ledger_IdNo     ) " & _
                                                "     Values                  ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_Po_No.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Po_No.Text))) & ",    @OrderDate              ,  " & Val(Clth_ID) & " , " & Str(Val(Slno)) & ",   " & Val(Wea_ID) & " ) "
                        cmd.ExecuteNonQuery()
                    End If




                Next

            End With

            With dgv_processingdetails

                Slno = 0


                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Slno = Slno + 1

                        Procer_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Proc_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        cmd.CommandText = "Insert into Own_Order_Processing_Details (    Own_Order_Code ,               Company_IdNo       ,      Own_Order_No        ,                               for_OrderBy                                    ,     Own_Order_Date          ,     Cloth_IdNo          ,      Sl_No        ,     Ledger_IdNo                ,     Process_Idno       ,        Rate                          ) " & _
                                                "     Values                  ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_Po_No.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Po_No.Text))) & ",    @OrderDate              ,  " & Val(Clth_ID) & " , " & Str(Val(Slno)) & ",   " & Val(Procer_ID) & "     ,   " & Val(Proc_ID) & "," & Val(.Rows(i).Cells(3).Value) & ") "
                        cmd.ExecuteNonQuery()
                    End If




                Next

            End With

            tr.Commit()

            move_record(lbl_Po_No.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)


        Catch ex As Exception
            tr.Rollback()
            'If InStr(1, Err.Description, "CK_Own_Order_Yarn_Details_1") > 0 Then
            '    MessageBox.Show("Invalid Receipt Quantity, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'ElseIf InStr(1, Err.Description, "CK_Own_Order_Yarn_Details_2") > 0 Then
            '    MessageBox.Show("Invalid Delivery Quantity, Delivery Quantity must greater than Receipt Quantity", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'ElseIf InStr(1, Err.Description, "CK_Own_Order_Yarn_Details_3") > 0 Then
            '    MessageBox.Show("Invalid Receipt Bags, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'ElseIf InStr(1, Err.Description, "CK_Own_Order_Yarn_Details_4") > 0 Then
            '    MessageBox.Show("Invalid Receipt Bags, Receipt Bags must be lesser than Order Bags", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


            'Else
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ' End If

        Finally
            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub



   
    Private Sub cbo_Quality_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Quality.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", " ", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Quality_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Quality.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Quality, msk_date, txt_OrderMtrs, "Cloth_Head", "Cloth_Name", " ", "(Cloth_idno = 0)")


    End Sub

    Private Sub cbo_Quality_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Quality.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Quality, txt_OrderMtrs, "Cloth_Head", "Cloth_Name", " ", "(Cloth_idno = 0)")
        

    End Sub

    Private Sub cbo_Quality_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Quality.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Quality.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    

   
    Private Sub cbo_Filter_lotNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_lotNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Lot_Head", "Lot_No", "", "(Lot_IdNo = 0)")
    End Sub


    Private Sub cbo_Filter_lotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_lotNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_lotNo, dtp_Filter_ToDate, cbo_Filter_BuyerName, "Lot_Head", "Lot_No", "", "(Lot_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_lotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_lotNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_lotNo, cbo_Filter_BuyerName, "Lot_Head", "Lot_No", "", "(Lot_IdNo = 0)")

    End Sub
    'Private Sub cbo_OrderNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_WeaverName.GotFocus
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "BuyerOrder_Head", "Io_Style_Code", "(Order_Close =  0)", "")
    'End Sub

    'Private Sub cbo_OrderNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_WeaverName.KeyDown
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_WeaverName, Nothing, Nothing, "BuyerOrder_Head", "Io_Style_Code", "(Order_Close =  0)", "")
    '    With dgv_Details


    '        If (e.KeyValue = 38 And Cbo_WeaverName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
    '            .Focus()
    '            .CurrentCell = .Rows(.CurrentRow.Index).Cells(9)

    '        End If
    '        If (e.KeyValue = 40 And Cbo_WeaverName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

    '            .Focus()
    '            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

    '        End If


    '    End With
    'End Sub

    'Private Sub Cbo_WeaverName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_WeaverName.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_WeaverName, Nothing, "BuyerOrder_Head", "Io_Style_Code", "(Order_Close =  0)", "")
    '    If Asc(e.KeyChar) = 13 Then
    '        e.Handled = True

    '        With dgv_WeavingDetails
    '            .Focus()
    '            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)



    '        End With


    '    End If
    'End Sub








    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
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
        Dim Bye_IdNo As Integer, Lt_idNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Bye_IdNo = 0
            Lt_idNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Own_Order_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Own_Order_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Own_Order_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_BuyerName.Text) <> "" Then
                Bye_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_BuyerName.Text)
            End If


            If Val(Bye_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_IdNo = " & Str(Val(Bye_IdNo)) & ")"
            End If

            If Trim(cbo_Filter_lotNo.Text) <> "" Then
                Lt_idNo = Common_Procedures.Lot_NoToIdNo(con, cbo_Filter_lotNo.Text)
            End If


            If Val(Lt_idNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Lot_IdNo = " & Str(Val(Lt_idNo)) & ")"
            End If



            da = New SqlClient.SqlDataAdapter("select a.*,  c.Ledger_Name from Own_Order_Head a   INNER JOIN Ledger_head c on a.Ledger_IdNo = c.Ledger_idno    where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Own_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Own_Order_Date, for_orderby, Own_Order_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Own_Order_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Common_Procedures.Lot_IdNoToNo(con, Val(dt2.Rows(i).Item("Lot_IdNo").ToString))
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(i).Item("Agent_IdNo").ToString))
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Total_Order_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Order_Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")


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
            Filter_RowNo = dgv_Filter_Details.CurrentRow.Index
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


    Private Sub cbo_Filter_BuyerName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_BuyerName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 14 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_BuyerName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_BuyerName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_BuyerName, cbo_Filter_lotNo, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 14 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_BuyerName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_BuyerName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_BuyerName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 14 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub


    

    Private Sub txt_Processing_ChargeMtr_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Processing_ChargeMtr.KeyDown
        If (e.KeyValue) = 40 Then
            'If Trim(cbo_Type.Text) = "ORDER" Then
            '   


            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else

                If dgv_SizingDetails.Rows.Count > 0 Then
                    dgv_SizingDetails.Focus()
                    dgv_SizingDetails.CurrentCell = dgv_SizingDetails.Rows(0).Cells(1)
                Else
                    If dgv_WeavingDetails.Rows.Count > 0 Then
                        dgv_WeavingDetails.Focus()
                        dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                    Else
                        If dgv_processingdetails.Rows.Count > 0 Then
                            dgv_processingdetails.Focus()
                            dgv_processingdetails.CurrentCell = dgv_processingdetails.Rows(0).Cells(1)
                        Else
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                msk_date.Focus()
                            End If
                        End If

                    End If
                End If



            End If
        End If
        If (e.KeyValue) = 38 Then
            txt_WeavinChargeMtr.Focus()
        End If
    End Sub








    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim Rect As Rectangle

        With dgv_Details
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


            If e.ColumnIndex = 1 Then

                If cbo_Type.Visible = False Or Val(cbo_Type.Tag) <> e.RowIndex Then

                    cbo_Type.Tag = -1

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Type.Left = .Left + Rect.Left
                    cbo_Type.Top = .Top + Rect.Top

                    cbo_Type.Width = Rect.Width
                    cbo_Type.Height = Rect.Height
                    cbo_Type.Text = .CurrentCell.Value

                    cbo_Type.Tag = Val(e.RowIndex)
                    cbo_Type.Visible = True

                    cbo_Type.BringToFront()
                    cbo_Type.Focus()



                End If

            Else
                cbo_Type.Visible = False

            End If


            If e.ColumnIndex = 2 Then

                If cbo_PartyName.Visible = False Or Val(cbo_PartyName.Tag) <> e.RowIndex Then

                    cbo_PartyName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead WHERE (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)) order by Ledger_DisplayName", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_PartyName.DataSource = Dt1
                    cbo_PartyName.DisplayMember = "Ledger_DisplayName"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_PartyName.Left = .Left + Rect.Left
                    cbo_PartyName.Top = .Top + Rect.Top

                    cbo_PartyName.Width = Rect.Width
                    cbo_PartyName.Height = Rect.Height
                    cbo_PartyName.Text = .CurrentCell.Value

                    cbo_PartyName.Tag = Val(e.RowIndex)
                    cbo_PartyName.Visible = True

                    cbo_PartyName.BringToFront()
                    cbo_PartyName.Focus()



                End If

            Else
                cbo_PartyName.Visible = False

            End If



            If e.ColumnIndex = 3 Then

                If cbo_Count.Visible = False Or Val(cbo_Count.Tag) <> e.RowIndex Then

                    cbo_Count.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Count.DataSource = Dt1
                    cbo_Count.DisplayMember = "Count_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Count.Left = .Left + Rect.Left
                    cbo_Count.Top = .Top + Rect.Top

                    cbo_Count.Width = Rect.Width
                    cbo_Count.Height = Rect.Height
                    cbo_Count.Text = .CurrentCell.Value

                    cbo_Count.Tag = Val(e.RowIndex)
                    cbo_Count.Visible = True

                    cbo_Count.BringToFront()
                    cbo_Count.Focus()



                End If

            Else
                cbo_Count.Visible = False

            End If

           

            If e.ColumnIndex = 4 Then

                If cbo_MillName.Visible = False Or Val(cbo_MillName.Tag) <> e.RowIndex Then

                    cbo_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_MillName.DataSource = Dt3
                    cbo_MillName.DisplayMember = "Mill_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_MillName.Left = .Left + Rect.Left
                    cbo_MillName.Top = .Top + Rect.Top
                    cbo_MillName.Width = Rect.Width
                    cbo_MillName.Height = Rect.Height

                    cbo_MillName.Text = .CurrentCell.Value

                    cbo_MillName.Tag = Val(e.RowIndex)
                    cbo_MillName.Visible = True

                    cbo_MillName.BringToFront()
                    cbo_MillName.Focus()

                End If

            Else

                cbo_MillName.Visible = False

            End If

           
        End With

    End Sub



    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
            
        End With
    End Sub
    
    

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = Nothing

        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)

    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_ActiveCtrl_Name = dgv_Details.Name
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgv_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then

                    'If Val(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> 0 Or Val(.Rows(.CurrentCell.RowIndex).Cells(15).Value) <> 0 Or Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(16).Value) <> 0 Then
                    '    e.Handled = True
                    '    e.SuppressKeyPress = True
                    'End If

                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex <> 5 Then


                    'If Val(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> 0 Or Val(.Rows(.CurrentCell.RowIndex).Cells(15).Value) <> 0 Or Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(16).Value) <> 0 Then
                    '    e.Handled = True

                    'End If
                End If
                If .CurrentCell.ColumnIndex = 5 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If

            End If
        End With

    End Sub
    Private Sub dgtxt_details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
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


    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_PartyName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_PartyName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(3)
            End If

        End With



    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)

            End With

        End If
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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


    Private Sub cbo_PartyName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.TextChanged
        Try
            If cbo_PartyName.Visible Then
                With dgv_Details
                    If Val(cbo_PartyName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_PartyName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub


    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, Nothing, Nothing, "", "", "", "")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Type.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    txt_Processing_ChargeMtr.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                End If
            End If
            If (e.KeyValue = 40 And cbo_Type.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
                    dgv_SizingDetails.Focus()
                    dgv_SizingDetails.CurrentCell = dgv_SizingDetails.Rows(0).Cells(1)
                    'Else
                    '    If dgv_WeavingDetails.Rows.Count > 0 Then
                    '        dgv_WeavingDetails.Focus()
                    '        dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                    '    Else
                    '        If dgv_processingdetails.Rows.Count > 0 Then
                    '            dgv_processingdetails.Focus()
                    '            dgv_processingdetails.CurrentCell = dgv_processingdetails.Rows(0).Cells(1)
                    '        Else
                    '            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    '                save_record()
                    '            Else
                    '                msk_date.Focus()
                    '            End If
                    '        End If

                    '    End If
                    'End If


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End If

        End With


    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If dgv_SizingDetails.Rows.Count = 0 Then dgv_SizingDetails.Rows.Add()
                    dgv_SizingDetails.Focus()
                    dgv_SizingDetails.CurrentCell = dgv_SizingDetails.Rows(0).Cells(1)
                    'Else
                    '    If dgv_WeavingDetails.Rows.Count > 0 Then
                    '        dgv_WeavingDetails.Focus()
                    '        dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                    '    Else
                    '        If dgv_processingdetails.Rows.Count > 0 Then
                    '            dgv_processingdetails.Focus()
                    '            dgv_processingdetails.CurrentCell = dgv_processingdetails.Rows(0).Cells(1)
                    '        Else
                    '            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    '                save_record()
                    '            Else
                    '                msk_date.Focus()
                    '            End If
                    '        End If

                    '    End If
                    'End If


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With
        End If

    End Sub

   
    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        Try
            If cbo_Type.Visible Then
                With dgv_Details
                    If Val(cbo_Type.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Type.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_Details
            If (e.KeyValue = 38 And cbo_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
               
                    .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                
            End If

            If (e.KeyValue = 40 And cbo_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                
                    .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(4)
                   

            End If



        End With
    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_Details
            If Asc(e.KeyChar) = 13 Then

                  .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(4)


            End If
        End With
    End Sub

    Private Sub cbo_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then


            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Count_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.TextChanged
        Try
            If cbo_Count.Visible Then
                With dgv_Details
                    If Val(cbo_Count.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Count.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With



    End Sub

    Private Sub cbo_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.TextChanged
        Try
            If cbo_MillName.Visible Then
                With dgv_Details
                    If Val(cbo_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Sizing_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Sizing_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_Name, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        With dgv_SizingDetails

            If (e.KeyValue = 38 And cbo_Sizing_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    If dgv_Details.RowCount > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    Else
                        txt_Processing_ChargeMtr.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(1)
                    .CurrentCell.Selected = True
                End If
            End If

            If (e.KeyValue = 40 And cbo_Sizing_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentRow.Index >= .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    If dgv_WeavingDetails.Rows.Count = 0 Then dgv_WeavingDetails.Rows.Add()
                    dgv_WeavingDetails.Focus()
                    dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                    'If dgv_WeavingDetails.Rows.Count > 0 Then
                    '    dgv_WeavingDetails.Focus()
                    '    dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                    'Else
                    '    If dgv_processingdetails.Rows.Count > 0 Then
                    '        dgv_processingdetails.Focus()
                    '        dgv_processingdetails.CurrentCell = dgv_processingdetails.Rows(0).Cells(1)
                    '    Else
                    '        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    '            save_record()
                    '        Else
                    '            msk_date.Focus()
                    '        End If
                    '    End If

                    'End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    .CurrentCell.Selected = True

                End If

            End If



        End With



    End Sub

    Private Sub cbo_Sizing_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_Name, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_SizingDetails
                If .CurrentRow.Index = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    If dgv_WeavingDetails.Rows.Count = 0 Then dgv_WeavingDetails.Rows.Add()
                    dgv_WeavingDetails.Focus()
                    dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                    'If dgv_WeavingDetails.Rows.Count > 0 Then
                    '    dgv_WeavingDetails.Focus()
                    '    dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                    'Else
                    '    If dgv_processingdetails.Rows.Count > 0 Then
                    '        dgv_processingdetails.Focus()
                    '        dgv_processingdetails.CurrentCell = dgv_processingdetails.Rows(0).Cells(1)
                    '    Else
                    '        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    '            save_record()
                    '        Else
                    '            msk_date.Focus()
                    '        End If
                    '    End If

                    'End If
                Else


                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(.CurrentCell.ColumnIndex)

                End If
            End With


        End If
    End Sub

    Private Sub cbo_Sizing_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Sizing_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_Sizing_Name_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing_Name.TextChanged
        Try
            If cbo_Sizing_Name.Visible Then
                With dgv_SizingDetails
                    If Val(cbo_Sizing_Name.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Sizing_Name.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Cbo_WeaverName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_WeaverName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub Cbo_WeaverName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_WeaverName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_WeaverName, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        With dgv_WeavingDetails

            If (e.KeyValue = 38 And Cbo_WeaverName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    If dgv_SizingDetails.Rows.Count > 0 Then
                        dgv_SizingDetails.Focus()
                        dgv_SizingDetails.CurrentCell = dgv_SizingDetails.Rows(0).Cells(1)
                    Else
                        If dgv_Details.RowCount > 0 Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                        Else
                            txt_Processing_ChargeMtr.Focus()
                        End If
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(1)
                    .CurrentCell.Selected = True
                End If
            End If

            If (e.KeyValue = 40 And Cbo_WeaverName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then


                If .CurrentRow.Index = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    If dgv_processingdetails.Rows.Count > 0 Then
                        dgv_processingdetails.Focus()
                        dgv_processingdetails.CurrentCell = dgv_processingdetails.Rows(0).Cells(1)
                    Else
                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            save_record()
                        Else
                            msk_date.Focus()
                        End If
                    End If

                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    .CurrentCell.Selected = True

                End If

            End If

           

        End With



    End Sub

    Private Sub Cbo_WeaverName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_WeaverName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_WeaverName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_WeavingDetails
                If .CurrentRow.Index = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    If dgv_processingdetails.Rows.Count > 0 Then
                        dgv_processingdetails.Focus()
                        dgv_processingdetails.CurrentCell = dgv_processingdetails.Rows(0).Cells(1)
                    Else
                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            save_record()
                        Else
                            msk_date.Focus()
                        End If
                    End If

                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                End If
            End With


        End If
    End Sub

    Private Sub Cbo_WeaverName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_WeaverName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_WeaverName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub Cbo_WeaverName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_WeaverName.TextChanged
        Try
            If Cbo_WeaverName.Visible Then
                With dgv_WeavingDetails
                    If Val(Cbo_WeaverName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_WeaverName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_Processor_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Processor_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Processor_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Processor_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Processor_Name, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

        With dgv_processingdetails

            If (e.KeyValue = 38 And cbo_Processor_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    If dgv_WeavingDetails.Rows.Count > 0 Then
                        dgv_WeavingDetails.Focus()
                        dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                    Else
                        If dgv_SizingDetails.Rows.Count > 0 Then
                            dgv_SizingDetails.Focus()
                            dgv_SizingDetails.CurrentCell = dgv_SizingDetails.Rows(0).Cells(1)
                        Else
                            If dgv_Details.RowCount > 0 Then
                                dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                            Else
                                txt_Processing_ChargeMtr.Focus()
                            End If
                        End If
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(3)
                    .CurrentCell.Selected = True
                End If
                End If

            If (e.KeyValue = 40 And cbo_Processor_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then


                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        msk_date.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(2)
                    .CurrentCell.Selected = True

                End If

            End If



        End With



    End Sub

    Private Sub cbo_Processor_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Processor_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Processor_Name, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_processingdetails
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then


                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        msk_date.Focus()
                    End If


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(2)

                End If
            End With


        End If
    End Sub

    Private Sub cbo_Processor_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Processor_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Processor_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Processor_Name_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Processor_Name.TextChanged
        Try
            If cbo_Processor_Name.Visible Then
                With dgv_processingdetails
                    If Val(cbo_Processor_Name.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Processor_Name.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Process_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Process_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")
    End Sub
    Private Sub cbo_Process_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Process_Name.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Process_Name, Nothing, Nothing, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")

        With dgv_processingdetails
            If (e.KeyValue = 38 And cbo_Process_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)

            End If

            If (e.KeyValue = 40 And cbo_Process_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)


            End If



        End With
    End Sub

    Private Sub cbo_Process_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Process_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Process_Name, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_processingdetails
            If Asc(e.KeyChar) = 13 Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)


            End If
        End With
    End Sub

    Private Sub cbo_Process_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Process_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then


            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Process_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Process_Name_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Process_Name.TextChanged
        Try
            If cbo_Process_Name.Visible Then
                With dgv_processingdetails
                    If Val(cbo_Process_Name.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Process_Name.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Po_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Own_Order_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Own_Order_Code = '" & Trim(NewCode) & "' ", con)
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
        Dim ps As Printing.PaperSize
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

                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "Yarn Purchase Order"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Yarn Purchase Order.pdf"
                    PrintDocument1.Print()

                Else
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument1.Print()
                    End If
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
        Print_PDF_Status = False
    End Sub


    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Po_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetDt1.Clear()
        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, CSH.State_Name as Company_State_Name, CSH.State_Code as Company_State_Code, LSH.State_Name as Ledger_State_Name, LSH.State_Code as Ledger_State_Code  from Own_Order_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo    LEFT OUTER JOIN State_HEad CSH on b.Company_State_IdNo = CSH.State_IdNo LEFT OUTER JOIN State_HEad LSH on c.Ledger_State_IdNo = LSH.State_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Own_Order_Code = '" & Trim(NewCode) & "' ", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.* from Own_Order_Yarn_Details a INNER JOIN YarnCount_Head b ON a.YarnCount_IdNo = b.YarnCount_IdNo where a.Own_Order_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Own_Order_No", con)
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
        'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
        Printing_Format2(e)
        'End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim ItmNm3 As String = "", ItmNm4 As String = ""
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer



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

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 40
            .Top = 40
            .Bottom = 40
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

        NoofItems_PerPage = 9

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 80 : ClArr(3) = 100 : ClArr(4) = 120 : ClArr(5) = 55 : ClArr(6) = 75 : ClArr(7) = 85 : ClArr(8) = 80
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Po_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ' ItmNm3 = Common_Procedures.YarnType_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("YarNtYPE_IdNO").ToString))
                        ItmNm4 = ""
                        If Len(ItmNm3) > 8 Then
                            For I = 8 To 1 Step -1
                                If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 8
                            ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                            ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I - 1)
                        End If


                        '  ItmNm1 = Common_Procedures.YarnMill_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("YarnMill_IdNO").ToString))
                        ItmNm2 = ""
                        If Len(ItmNm1) > 15 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 15
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If


                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("YarnCount_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)


                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Order_bags").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Order_Weight_Bags").ToString), "###########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Order_Weight").ToString), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm4) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm4), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If
                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If






                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single, S2 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String, Cmp_CstNo1 As String
        Dim CInc As Integer
        Dim CstDetAr() As String
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Own_Order_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Own_Order_Code = '" & Trim(EntryCode) & "'  Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_CstNo1 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        Erase CstDetAr
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            CstDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString), ",")

            CInc = -1

            CInc = CInc + 1
            If UBound(CstDetAr) >= CInc Then
                Cmp_CstNo = Trim(CstDetAr(CInc))
            End If

            CInc = CInc + 1
            If UBound(CstDetAr) >= CInc Then
                Cmp_CstNo1 = Trim(CstDetAr(CInc))
            End If



        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        ' e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.CompanyLOGO_RD, Drawing.Image), LMargin + 20, CurY, 112, 80)
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, PageWidth - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        ' CurY = CurY + TxtHgt - 1

        ' Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, "CST NO :" & Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo1, PageWidth - 10, CurY, 1, 0, pFont)
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("DELIVERY DATE  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            S2 = e.Graphics.MeasureString("ORDER.NO & DATE               :    ", pFont).Width

            CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "LOT.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Lot_IdNoToNo(con, Val(prn_HdDt.Rows(0).Item("lOT_IdNO").ToString)), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PO.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Own_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PO.DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Own_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt - 5
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "YARN", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "YARN", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WGT PER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AGREED", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            p1Font = New Font("Rupee Foradian", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT (`)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            p1Font = New Font("Rupee Foradian", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "RATE (`)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), p1Font)


            Common_Procedures.Print_To_PrintDocument(e, "(APPROX)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "(APPROX)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim p1Font As Font
        Dim I As Integer
        Dim BmsInWrds As String
        Dim W1 As Single = 0
        Dim Cmp_Name As String

        Dim vprn_PckNos As String = ""
        Dim Tot_Wgt As Single = 0, Tot_Amt As Single = 0, Tot_Bgs As Single = 0, Tot_Wgt_Bag As Single = 0
        W1 = e.Graphics.MeasureString("Payment Terms : ", pFont).Width

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Po_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(0).Item("Total_Order_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(0).Item("Total_Order_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & (prn_HdDt.Rows(0).Item("Total_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + +ClAr(3) + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))



            CurY = CurY + TxtHgt - 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            StrConv(BmsInWrds, vbProperCase)

            Common_Procedures.Print_To_PrintDocument(e, "In Words     : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Taxes").ToString)
            'ItmNm2 = ""
            'If Len(ItmNm1) > 70 Then
            '    For I = 70 To 1 Step -1
            '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
            '    Next I
            '    If I = 0 Then I = 70
            '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
            '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            'End If

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "Taxes", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Taxes").ToString), LMargin + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Payment Terms", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Payments_Terms").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Delivery Terms", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Terms").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Agent_IdNO").ToString)), LMargin + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            da2 = New SqlClient.SqlDataAdapter("select a.*,b.* from Own_Order_Sizing_Details a   INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.DeliveryAt_IdNO  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Own_Order_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Own_Order_No", con)
            dt2 = New DataTable
            da2.Fill(dt2)
            If dt2.Rows.Count > 0 Then



                If Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(0).Item("DeliveryAt_IdNO").ToString)) <> "" Then

                    Common_Procedures.Print_To_PrintDocument(e, "Delivery At", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(0).Item("DeliveryAt_IdNO").ToString)), LMargin + W1 + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & dt2.Rows(0).Item("Ledger_Address1").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & dt2.Rows(0).Item("Ledger_Address2").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & dt2.Rows(0).Item("Ledger_Address3").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & dt2.Rows(0).Item("Ledger_Address4").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

                End If
            End If
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY


            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("Calibri", 24, FontStyle.Bold)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, ".", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, ".", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Enclose duplicate purchase order along with your invoice ", LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, ".", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Yarn supplied by you should be as per our order", LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, ".", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "We reserve the right to accept/reject on delay deliveries and sub-standard quality ", LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, ".", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "All dispute are subject to Tirupur jurisdiction", LMargin + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt - 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt


            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
            Common_Procedures.Print_To_PrintDocument(e, "This is a computer generated", LMargin + 20, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_PDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
        'Print_PDF_Status = False
    End Sub

    Private Sub btn_EMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EMail.Click
        Dim Led_IdNo As Integer
        Dim MailTxt As String
        Dim amt As Single
        Try


            amt = 0

           

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Quality.Text)

            MailTxt = "YARN PURCHASE ORDER " & vbCrLf & vbCrLf
            MailTxt = MailTxt & "Po.No.-" & Trim(lbl_Po_No.Text) & vbCrLf & "Date-" & Trim(dtp_Date.Text)
            ' MailTxt = MailTxt & vbCrLf & "Agent Name.-" & Trim(cbo_Agent.Text)
            MailTxt = MailTxt & vbCrLf & "Value-" & Trim(amt)

            EMAIL_Entry.vMailID = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            EMAIL_Entry.vSubJect = "Po.No : " & Trim(lbl_Po_No.Text)
            EMAIL_Entry.vMessage = Trim(MailTxt)

            Dim f1 As New EMAIL_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_SizingDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_SizingDetails.CellEndEdit
        dgv_SizingDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_SizingDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_SizingDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter

        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Dt3 As New DataTable

        Dim Rect As Rectangle

        With dgv_SizingDetails
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If




            If e.ColumnIndex = 1 Then

                If cbo_Sizing_Name.Visible = False Or Val(cbo_Sizing_Name.Tag) <> e.RowIndex Then

                    cbo_Sizing_Name.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_Type = 'SIZING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 order by Ledger_DisplayName", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Sizing_Name.DataSource = Dt3
                    cbo_Sizing_Name.DisplayMember = "Ledger_DisplayName"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Sizing_Name.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Sizing_Name.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Sizing_Name.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_Sizing_Name.Height = Rect.Height  ' rect.Height

                    cbo_Sizing_Name.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Sizing_Name.Tag = Val(e.RowIndex)
                    cbo_Sizing_Name.Visible = True

                    cbo_Sizing_Name.BringToFront()
                    cbo_Sizing_Name.Focus()

                End If

            Else

                cbo_Sizing_Name.Visible = False

            End If


        End With

    End Sub

    Private Sub dgv_SizingDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_SizingDetails.CellLeave
        With dgv_SizingDetails
           

        End With
    End Sub

    Private Sub dgv_SizingDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_SizingDetails.CellValueChanged
        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_SizingDetails
                If .Visible Then

                    If (.CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2) And Trim(.CurrentCell.Value.ToString) <> "" Then
                        If .CurrentRow.Index = .Rows.Count - 1 Then
                            .Rows.Add()
                        End If
                    End If

                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub
    Private Sub dgv_SizingDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_SizingDetails.EditingControlShowing
        dgtxt_SizingDetails = Nothing

        dgtxt_SizingDetails = CType(dgv_SizingDetails.EditingControl, DataGridViewTextBoxEditingControl)

    End Sub

    Private Sub dgtxt_SizingDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_SizingDetails.Enter
        dgv_ActiveCtrl_Name = dgv_SizingDetails.Name
        dgv_SizingDetails.EditingControl.BackColor = Color.Lime
        dgv_SizingDetails.EditingControl.ForeColor = Color.Blue
        dgv_SizingDetails.SelectAll()
    End Sub

    Private Sub dgtxt_SizingDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_SizingDetails.KeyDown
        With dgv_SizingDetails
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then

                    'If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                    '    If Trim(UCase(cbo_Type.Text)) = "DIRECT" Or Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                    '        e.Handled = True
                    '        e.SuppressKeyPress = True
                    '    End If
                    'End If

                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_SizingDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_SizingDetails.KeyPress

        With dgv_SizingDetails
            If .Visible Then

                ' If .CurrentCell.ColumnIndex = 3 Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

                ' If

            End If
        End With

    End Sub
    Private Sub dgtxt_SizingDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_SizingDetails.TextChanged
        Try
            With dgv_SizingDetails

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_SizingDetails.Text)

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


    Private Sub dgtxt_SizingDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_SizingDetails.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Delivery_Details_KeyUp(sender, e)
        End If

    End Sub

    Private Sub dgv_Delivery_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_SizingDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Delivery_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_SizingDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_SizingDetails

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If



            End With

        End If

    End Sub

    Private Sub dgv_SizingDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_SizingDetails.LostFocus
        On Error Resume Next
        dgv_SizingDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_SizingDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_SizingDetails.RowsAdded
        Dim n As Integer

        With dgv_SizingDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    


    Private Sub dgv_WeavingDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WeavingDetails.CellEndEdit
        dgv_WeavingDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_WeavingDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WeavingDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter

        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Dt3 As New DataTable

        Dim Rect As Rectangle

        With dgv_WeavingDetails
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If




            If e.ColumnIndex = 1 Then

                If Cbo_WeaverName.Visible = False Or Val(Cbo_WeaverName.Tag) <> e.RowIndex Then

                    Cbo_WeaverName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 order by Ledger_DisplayName", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    Cbo_WeaverName.DataSource = Dt3
                    Cbo_WeaverName.DisplayMember = "Ledger_DisplayName"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_WeaverName.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    Cbo_WeaverName.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    Cbo_WeaverName.Width = Rect.Width  ' .CurrentCell.Size.Width
                    Cbo_WeaverName.Height = Rect.Height  ' rect.Height

                    Cbo_WeaverName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    Cbo_WeaverName.Tag = Val(e.RowIndex)
                    Cbo_WeaverName.Visible = True

                    Cbo_WeaverName.BringToFront()
                    Cbo_WeaverName.Focus()

                End If

            Else

                Cbo_WeaverName.Visible = False

            End If


        End With

    End Sub

    Private Sub dgv_WeavingDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WeavingDetails.CellLeave
        With dgv_WeavingDetails


        End With
    End Sub

    Private Sub dgv_WeavingDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_WeavingDetails.CellValueChanged
        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_WeavingDetails
                If .Visible Then

                    If (.CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2) And Trim(.CurrentCell.Value.ToString) <> "" Then
                        If .CurrentRow.Index = .Rows.Count - 1 Then
                            .Rows.Add()
                        End If
                    End If

                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub
    Private Sub dgv_WeavingDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_WeavingDetails.EditingControlShowing
        dgtxt_WeavingDetails = Nothing

        dgtxt_WeavingDetails = CType(dgv_WeavingDetails.EditingControl, DataGridViewTextBoxEditingControl)

    End Sub

    Private Sub dgtxt_WeavingDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WeavingDetails.Enter
        dgv_ActiveCtrl_Name = dgv_WeavingDetails.Name
        dgv_WeavingDetails.EditingControl.BackColor = Color.Lime
        dgv_WeavingDetails.EditingControl.ForeColor = Color.Blue
        dgv_WeavingDetails.SelectAll()
    End Sub

    Private Sub dgtxt_WeavingDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WeavingDetails.KeyDown
        With dgv_WeavingDetails
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then

                    'If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                    '    If Trim(UCase(cbo_Type.Text)) = "DIRECT" Or Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                    '        e.Handled = True
                    '        e.SuppressKeyPress = True
                    '    End If
                    'End If

                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_WeavingDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_WeavingDetails.KeyPress

        With dgv_WeavingDetails
            If .Visible Then

                '' If .CurrentCell.ColumnIndex = 3 Then

                'If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                '    e.Handled = True
                'End If

                ' If

            End If
        End With

    End Sub
    Private Sub dgtxt_WeavingDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WeavingDetails.TextChanged
        Try
            With dgv_WeavingDetails

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_WeavingDetails.Text)

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


    Private Sub dgTXT_WeavingDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WeavingDetails.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_WeavingDetails_KeyUp(sender, e)
        End If

    End Sub

    Private Sub dgv_WeavingDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_SizingDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_WeavingDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_SizingDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_SizingDetails

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If



            End With

        End If

    End Sub

    Private Sub dgv_WeavingDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_WeavingDetails.LostFocus
        On Error Resume Next
        dgv_WeavingDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_WeavingDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_WeavingDetails.RowsAdded
        Dim n As Integer

        With dgv_WeavingDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim ItmNm3 As String = "", ItmNm4 As String = ""
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer



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

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 40
            .Top = 40
            .Bottom = 40
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

        NoofItems_PerPage = 9

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 110 : ClArr(3) = 150 : ClArr(4) = 60 : ClArr(5) = 80 : ClArr(6) = 100 : ClArr(7) = 90
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Po_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ' ItmNm3 = Common_Procedures.YarnType_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("YarNtYPE_IdNO").ToString))
                        ItmNm4 = ""
                        If Len(ItmNm3) > 15 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 15
                            ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                            ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I - 1)
                        End If


                        ' ItmNm1 = Common_Procedures.YarnMill_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("YarnMill_IdNO").ToString))
                        ItmNm2 = ""
                        If Len(ItmNm1) > 18 Then
                            For I = 18 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 18
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If


                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("YarnCount_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        '  Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)


                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Order_bags").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Order_Weight_Bags").ToString), "###########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Order_Weight").ToString), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm4) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm4), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If
                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If






                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single, S2 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String, Cmp_CstNo1 As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CInc As Integer
        Dim CstDetAr() As String
        Dim CurX As String = ""
        Dim strWidth As Single = 0



        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Own_Order_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Own_Order_Code = '" & Trim(EntryCode) & "'  Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_CstNo1 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = "" : Cmp_PanNo = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
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
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        Erase CstDetAr
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            CstDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString), ",")

            CInc = -1

            CInc = CInc + 1
            If UBound(CstDetAr) >= CInc Then
                Cmp_CstNo = Trim(CstDetAr(CInc))
            End If

            CInc = CInc + 1
            If UBound(CstDetAr) >= CInc Then
                Cmp_CstNo1 = Trim(CstDetAr(CInc))
            End If

        End If

        ' e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.CompanyLOGO_RD, Drawing.Image), LMargin + 20, CurY + 20, 112, 80)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        ' e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.CompanyLOGO_RD, Drawing.Image), LMargin + 20, CurY, 112, 80)
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, pFont).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, pFont).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, PageWidth - 10, CurY, 1, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, "CST NO :" & Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo1, PageWidth - 10, CurY, 1, 0, pFont)
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("DELIVERY DATE  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            S2 = e.Graphics.MeasureString("ORDER.NO & DATE               :    ", pFont).Width

            CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "LOT.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Lot_IdNoToNo(con, Val(prn_HdDt.Rows(0).Item("lOT_IdNO").ToString)), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PO.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Own_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PO.DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Own_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt - 5
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "YARN", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "YARN", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WGT PER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AGREED", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            p1Font = New Font("Rupee Foradian", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT (`)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            p1Font = New Font("Rupee Foradian", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "RATE (`)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), p1Font)


            Common_Procedures.Print_To_PrintDocument(e, "(APPROX)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "(APPROX)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim p1Font As Font
        Dim I As Integer
        Dim BmsInWrds As String
        Dim W1 As Single = 0
        Dim Cmp_Name As String

        Dim vprn_PckNos As String = ""
        Dim Tot_Wgt As Single = 0, Tot_Amt As Single = 0, Tot_Bgs As Single = 0, Tot_Wgt_Bag As Single = 0
        W1 = e.Graphics.MeasureString("Payment Terms : ", pFont).Width

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Po_No.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(0).Item("Total_Order_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(0).Item("Total_Order_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & (prn_HdDt.Rows(0).Item("Total_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))



            CurY = CurY + TxtHgt - 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            StrConv(BmsInWrds, vbProperCase)

            Common_Procedures.Print_To_PrintDocument(e, "In Words     : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Taxes").ToString)
            'ItmNm2 = ""
            'If Len(ItmNm1) > 70 Then
            '    For I = 70 To 1 Step -1
            '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
            '    Next I
            '    If I = 0 Then I = 70
            '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
            '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            'End If

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "Taxes", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Taxes").ToString), LMargin + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Payment Terms", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Payments_Terms").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Delivery Terms", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Terms").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Agent_IdNO").ToString)), LMargin + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            da2 = New SqlClient.SqlDataAdapter("select a.*,b.* from Own_Order_Sizing_Details a   INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.DeliveryAt_IdNO  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Own_Order_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Own_Order_No", con)
            dt2 = New DataTable
            da2.Fill(dt2)
            If dt2.Rows.Count > 0 Then



                If Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(0).Item("DeliveryAt_IdNO").ToString)) <> "" Then

                    Common_Procedures.Print_To_PrintDocument(e, "Delivery At", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(0).Item("DeliveryAt_IdNO").ToString)), LMargin + W1 + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & dt2.Rows(0).Item("Ledger_Address1").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & dt2.Rows(0).Item("Ledger_Address2").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & dt2.Rows(0).Item("Ledger_Address3").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & dt2.Rows(0).Item("Ledger_Address4").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

                End If
            End If
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY


            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("Calibri", 24, FontStyle.Bold)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, ".", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, ".", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Enclose duplicate purchase order along with your invoice ", LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, ".", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Yarn supplied by you should be as per our order", LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, ".", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "We reserve the right to accept/reject on delay deliveries and sub-standard quality ", LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, ".", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "All dispute are subject to Tirupur jurisdiction", LMargin + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt - 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt


            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
            Common_Procedures.Print_To_PrintDocument(e, "This is a computer generated", LMargin + 20, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    
  
    Private Sub txt_Processing_ChargeMtr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Processing_ChargeMtr.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else

                If dgv_SizingDetails.Rows.Count > 0 Then
                    dgv_SizingDetails.Focus()
                    dgv_SizingDetails.CurrentCell = dgv_SizingDetails.Rows(0).Cells(1)
                Else
                    If dgv_WeavingDetails.Rows.Count > 0 Then
                        dgv_WeavingDetails.Focus()
                        dgv_WeavingDetails.CurrentCell = dgv_WeavingDetails.Rows(0).Cells(1)
                    Else
                        If dgv_processingdetails.Rows.Count > 0 Then
                            dgv_processingdetails.Focus()
                            dgv_processingdetails.CurrentCell = dgv_processingdetails.Rows(0).Cells(1)
                        Else
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                msk_date.Focus()
                            End If
                        End If

                    End If
                End If



            End If
        End If
    End Sub
    Private Sub dgv_processingdetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_processingdetails.CellEndEdit
        dgv_processingdetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_processingdetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_processingdetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Dt3 As New DataTable

        Dim Rect As Rectangle

        With dgv_processingdetails
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If




            If e.ColumnIndex = 1 Then

                If cbo_Processor_Name.Visible = False Or Val(cbo_Processor_Name.Tag) <> e.RowIndex Then

                    cbo_Processor_Name.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) order by Ledger_DisplayName", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Processor_Name.DataSource = Dt3
                    cbo_Processor_Name.DisplayMember = "Ledger_DisplayName"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Processor_Name.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Processor_Name.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Processor_Name.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_Processor_Name.Height = Rect.Height  ' rect.Height

                    cbo_Processor_Name.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Processor_Name.Tag = Val(e.RowIndex)
                    cbo_Processor_Name.Visible = True

                    cbo_Processor_Name.BringToFront()
                    cbo_Processor_Name.Focus()

                End If

            Else

                cbo_Processor_Name.Visible = False

            End If

            If e.ColumnIndex = 2 Then

                If cbo_Process_Name.Visible = False Or Val(cbo_Process_Name.Tag) <> e.RowIndex Then

                    cbo_Process_Name.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Process_Name from Process_Head order by Process_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Process_Name.DataSource = Dt2
                    cbo_Process_Name.DisplayMember = "Process_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Process_Name.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Process_Name.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Process_Name.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_Process_Name.Height = Rect.Height  ' rect.Height

                    cbo_Process_Name.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Process_Name.Tag = Val(e.RowIndex)
                    cbo_Process_Name.Visible = True

                    cbo_Process_Name.BringToFront()
                    cbo_Process_Name.Focus()

                End If

            Else

                cbo_Process_Name.Visible = False

            End If
        End With

    End Sub

    Private Sub dgv_processingdetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_processingdetails.CellLeave
        With dgv_processingdetails


        End With
    End Sub

    Private Sub dgv_processingdetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_processingdetails.CellValueChanged
        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_processingdetails
                If .Visible Then



                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub
    Private Sub dgv_ProcessingDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_processingdetails.EditingControlShowing
        dgtxt_ProcessingDetails = Nothing

        dgtxt_ProcessingDetails = CType(dgv_processingdetails.EditingControl, DataGridViewTextBoxEditingControl)

    End Sub

    Private Sub dgtxt_ProcessingDetailss_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_ProcessingDetails.Enter
        dgv_ActiveCtrl_Name = dgv_processingdetails.Name
        dgv_processingdetails.EditingControl.BackColor = Color.Lime
        dgv_processingdetails.EditingControl.ForeColor = Color.Blue
        dgv_processingdetails.SelectAll()
    End Sub

    Private Sub dgtxt_ProcessingDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_ProcessingDetails.KeyDown
        With dgv_processingdetails
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then

                    'If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                    '    If Trim(UCase(cbo_Type.Text)) = "DIRECT" Or Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                    '        e.Handled = True
                    '        e.SuppressKeyPress = True
                    '    End If
                    'End If

                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_ProcessingDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_ProcessingDetails.KeyPress

        With dgv_processingdetails
            If .Visible Then

                If .CurrentCell.ColumnIndex = 3 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If

            End If
        End With

    End Sub
    Private Sub dgtxt_ProcessingDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_ProcessingDetails.TextChanged
        Try
            With dgv_processingdetails

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_ProcessingDetails.Text)

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


    Private Sub dgTXT_ProcessingDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_ProcessingDetails.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_processingdetails_KeyUp(sender, e)
        End If

    End Sub

    Private Sub dgv_processingdetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_processingdetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_processingdetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_processingdetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_processingdetails

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If



            End With

        End If

    End Sub

    Private Sub dgv_processingdetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_processingdetails.LostFocus
        On Error Resume Next
        dgv_processingdetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_processingdetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_processingdetails.RowsAdded
        Dim n As Integer

        With dgv_processingdetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub
    Private Sub Costing_Calculation()
        Dim WarpCnt As String = ""
        Dim TapeLgth As Single = 0
        Dim Width As Single = 0
        Dim EndsInch As Single = 0
        Dim WeftCnt As String = ""
        Dim ReedSpace As Single = 0
        Dim PickInch As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Clth_Id As Integer = 0
        Dim Warp_CSt As Single = 0
        Dim Weft_Cst As Single = 0

        Clth_Id = Common_Procedures.Cloth_NameToIdNo(con, cbo_Quality.Text)
        If Clth_Id <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Cloth_Head where   Cloth_idno = " & Str(Val(Clth_Id)), con)
            Dt = New DataTable
            Da.Fill(Dt)
            If Dt.Rows.Count > 0 Then
                WarpCnt = Common_Procedures.Count_IdNoToName(con, Val(Dt.Rows(0).Item("Cloth_WarpCount_IdNo").ToString))
                WeftCnt = Common_Procedures.Count_IdNoToName(con, Val(Dt.Rows(0).Item("Cloth_WeftCount_IdNo").ToString))
                ReedSpace = Dt.Rows(0).Item("Cloth_ReedSpace").ToString
                Width = Dt.Rows(0).Item("Cloth_Width").ToString
                TapeLgth = Dt.Rows(0).Item("Tape_Length").ToString
                EndsInch = Dt.Rows(0).Item("Cloth_Reed").ToString
                PickInch = Dt.Rows(0).Item("Cloth_Pick").ToString
            End If
        End If

        

        ''Processing
        'lbl_Processing.Text = Format(Val(txt_ProcessingChargeMtr.Text), "########0.00")
        ''Weaving
        'lbl_Weaving.Text = Format(Val(txt_WeavingChargeMtr.Text), "########0.00")

        ''profit % of warpcost+weftcost
        'lbl_Profit.Text = Format((Val(lbl_WeftYarnCostMtr.Text) + Val(lbl_WarpyarnCostMtr.Text)) * Val(txt_Profit.Text) / 100, "#########0.00")

        'WEIGHT METER FOR FABRIC IN GRAMS
        If Val(WarpCnt) <> 0 And Val(WeftCnt) <> 0 And Val(Width) <> 0 Then
            lbl_FabricWftGrms.Text = Format(Val((Val(EndsInch) / Val(WarpCnt)) + (Val(PickInch) / Val(WeftCnt))) * 0.68 * Val(Width), "#########0")
            lbl_WarpWt.Text = Format(Val((Val(EndsInch) / Val(WarpCnt))) * 0.68 * Val(Width), "#########0")
            lbl_WeftWgt.Text = Format(Val((Val(PickInch) / Val(WeftCnt))) * 0.68 * Val(Width), "#########0")

        Else
            lbl_FabricWftGrms.Text = ""
            lbl_WarpWt.Text = ""
            lbl_WeftWgt.Text = ""

        End If
        If Val(txt_OrderMtrs.Text) <> 0 Then
            lbl_Total_WeftWgt.Text = Format(Val(txt_OrderMtrs.Text) * Val(lbl_WeftWgt.Text), "###########0.000")
            lbl_TotalWarpWgt.Text = Format(Val(txt_OrderMtrs.Text) * Val(lbl_WarpWt.Text), "###########0.000")
            lbl_TotalYarnWgt.Text = Format(Val(txt_OrderMtrs.Text) * Val(lbl_FabricWftGrms.Text), "###########0.000")
        End If
        'COST/METER FOR WARP YARN
        If Val(WarpCnt) <> 0 Then
            Warp_CSt = Format(Val(((Val(txt_WarpRate.Text) + Val(txt_SizingCharge.Text)) * Val(TapeLgth) * (Val(Width) * Val(EndsInch))) / 66600) / Val(WarpCnt), " ########0.00")
        Else
            Warp_CSt = "0.00"
        End If

        'COST/METER FOR WEFT YARN
        If Val(ReedSpace) <> 0 And Val(PickInch) <> 0 And Val(WeftCnt) <> 0 Then
            Weft_Cst = Format(Val(1 / (1848 * Val(WeftCnt) / Val(ReedSpace) / Val(PickInch) / 1.09367)) * Val(txt_WeftRate.Text), " ########0.00")
        Else
            Weft_Cst = "0.00"
        End If
        'TOTAL COST METER
        lbl_ActualCost.Text = Format(Val(Warp_CSt) + Val(Weft_Cst) + Val(txt_WeavinChargeMtr.Text) + Val(txt_Processing_ChargeMtr.Text), " ########0.00")

    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
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

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

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

    Private Sub cbo_Quality_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Quality.LostFocus
        Costing_Calculation()
    End Sub

    Private Sub txt_WeftRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeftRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_WeftRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WeftRate.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_WarpRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WarpRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_WarpRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WarpRate.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_WeavinChargeMtr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeavinChargeMtr.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_WeavinChargeMtr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WeavinChargeMtr.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_Processing_ChargeMtr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Processing_ChargeMtr.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_OrderMtrs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OrderMtrs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_OrderMtrs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_OrderMtrs.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_SizingCharge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SizingCharge.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_SizingCharge_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SizingCharge.TextChanged
        Costing_Calculation()
    End Sub
End Class