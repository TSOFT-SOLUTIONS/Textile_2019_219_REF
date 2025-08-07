Public Class Stores_Item_PO_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "ITMPO-"
    Private cbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private vdgv_DrawNo As String = ""
    Private vCbo_ItmNm As String = ""
    Private vCloPic_STS As Boolean = False
    Private NoCalc_Status As Boolean = False

    Private vcbo_KeyDwnVal As Double
    Private dgv_ActCtrlName As String = ""

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_TaxDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private MOVESTS As Boolean = True
    Private prn_OriDupTri As String = ""
    Private prn_Count As Integer
    Private prn_InpOpts As String = ""
    Private PrintFrmt_Letter As Integer = 0
    Private prn_Status As Integer = 0
    Private PrintFrmt As String = ""
    Private Print_PDF_Status As Boolean = False


    Private Sub clear()


        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Terms.Visible = False

        Print_PDF_Status = False
        New_Entry = False

        lbl_PoNo.Text = ""
        lbl_PoNo.ForeColor = Color.Black

        dtp_date.Text = ""
        dtp_RefDate.Text = ""
        cbo_Ledger.Text = ""
        txt_Despatch.Text = ""
        '  cbo_Despatch.Text = ""

        txt_DeliveryTerms.Text = ""
        txt_PaymentTerms.Text = ""
        cbo_PaymentTerms.Text = ""

        txt_RefNo.Text = ""
        cbo_TaxType.Text = "GST"
        lbl_GrossAmount.Text = ""
        txt_DiscPercentage.Text = 0
        lbl_DiscAmount_Total.Text = ""
        txt_PackingCharges.Text = ""
        txt_Forwarding.Text = ""
        lbl_RoundOff.Text = ""
        txt_Remarks.Text = ""
        txt_AssessableValue.Text = ""
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""

        txt_DelvAdd1.Text = ""
        txt_DelvAdd2.Text = ""
        txt_DelvAdd3.Text = ""

        txt_Insurance.Text = ""
        txt_Warranty.Text = ""
        txt_installationCharges.Text = ""
        txt_Erection_Charges.Text = ""
        txt_Transportation.Text = ""
        txt_FrightAndUnloadingText.Text = ""
        txt_Documents.Text = ""
        'txt_Commissioning.Text = ""


        txt_DeliveryAddress1.Text = ""
        txt_DeliveryAddress2.Text = ""
        txt_DeliveryAddress3.Text = ""
        lbl_NetAmount.Text = "0.00"

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()


        dgv_Tax_Details.Rows.Clear()
        dgv_Tax_Details.Rows.Add()
        dgv_Tax_Details.Enabled = True

        cbo_Grid_Department.Text = False
        cbo_Grid_Item.Visible = False
        cbo_Grid_Brand.Text = False
        cbo_Grid_Unit.Visible = False

        PictureBox1.Image = Nothing
        vCloPic_STS = False

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White



        vdgv_DrawNo = ""
        vCbo_ItmNm = ""

        dgv_ActCtrlName = ""
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

        If Me.ActiveControl.Name <> cbo_Grid_Department.Name Then
            cbo_Grid_Department.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Item.Name Then
            cbo_Grid_Item.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Brand.Name Then
            cbo_Grid_Brand.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Unit.Name Then
            cbo_Grid_Unit.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
            pnl_Picture.Visible = False
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

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub Item_PO_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer = 0
        Dim CompCondt As String = ""

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Department.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DEPARTMENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Department.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Item.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Item.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Brand.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BRAND" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Brand.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Item_PO_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Department_Name from Department_Head order by Department_Name", con)
        da.Fill(dt1)
        cbo_Grid_Department.DataSource = dt1
        cbo_Grid_Department.DisplayMember = "Department_Name"

        da = New SqlClient.SqlDataAdapter("select Item_DisplayName from Stores_Item_AlaisHead order by Item_DisplayName", con)
        da.Fill(dt2)
        cbo_Grid_Item.DataSource = dt2
        cbo_Grid_Item.DisplayMember = "Item_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Brand_Name from Brand_Head order by Brand_Name", con)
        da.Fill(dt3)
        cbo_Grid_Brand.DataSource = dt3
        cbo_Grid_Brand.DisplayMember = "Brand_Name"

        da = New SqlClient.SqlDataAdapter("select Unit_Name from Unit_Head order by Unit_Name", con)
        da.Fill(dt4)
        cbo_Grid_Unit.DataSource = dt4
        cbo_Grid_Unit.DisplayMember = "Unit_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or (Ledger_Type = '' and AccountsGroup_IdNo = 14 ) ) order by Ledger_DisplayName", con)
        da.Fill(dt5)
        cbo_Ledger.DataSource = dt5
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Picture.Visible = False
        pnl_Picture.Left = (Me.Width - pnl_Picture.Width) - 25
        pnl_Picture.Top = (Me.Height - pnl_Picture.Height) - 50
        pnl_Picture.BringToFront()


        pnl_Terms.Visible = False
        pnl_Terms.Left = (Me.Width - pnl_Terms.Width) \ 2
        pnl_Terms.Top = (Me.Height - pnl_Terms.Height) \ 2
        pnl_Terms.BringToFront()

        pnl_Tax.Visible = False
        pnl_Tax.Left = (Me.Width - pnl_Tax.Width) \ 2
        pnl_Tax.Top = (Me.Height - pnl_Tax.Height) \ 2
        pnl_Tax.BringToFront()

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        dgv_Details.Columns(16).Visible = False

        If Trim(Common_Procedures.settings.CustomerCode) = "1464" Or Trim(Common_Procedures.settings.CustomerCode) = "1438" Then
            dgv_Details.Columns(16).Visible = True


            dgv_Details.Columns(1).Width = 80
            dgv_Details.Columns(2).Width = 55
            dgv_Details.Columns(3).Width = 150
            dgv_Details.Columns(4).Width = 70
            dgv_Details.Columns(5).Width = 55
            dgv_Details.Columns(5).HeaderText = "Qty"
            dgv_Details.Columns(6).Width = 40
            dgv_Details.Columns(7).Width = 60
            dgv_Details.Columns(8).Width = 80
            dgv_Details.Columns(9).Width = 40
            dgv_Details.Columns(10).Width = 40
            dgv_Details.Columns(11).Width = 55




        End If

        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Department.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Item.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Brand.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Item.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DeliveryTerms.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PaymentTerms.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentTerms.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Packinglabel.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ForwardingLabel.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_labelCaption1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_labelCaption2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LabelCaption3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LabelCaption4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LabelCaption5.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LabelCaption6.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LabelCaption7.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_LabelCaption9.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_RefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPercentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PackingCharges.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FrightAndUnloadingText.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Forwarding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Despatch.GotFocus, AddressOf ControlGotFocus
        '  AddHandler cbo_Despatch.GotFocus, AddressOf ControlGotFocus


        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Insurance.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Warranty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Erection_Charges.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_installationCharges.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Transportation.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FrightAndUnloadingText.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Documents.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DeliveryAddress1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DeliveryAddress2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DeliveryAddress3.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_Commissioning.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd3.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Department.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Item.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Brand.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Item.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DeliveryTerms.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PaymentTerms.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentTerms.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_RefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Insurance.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPercentage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PackingCharges.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Forwarding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Despatch.LostFocus, AddressOf ControlLostFocus
        ' AddHandler cbo_Despatch.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Warranty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Erection_Charges.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_installationCharges.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Transportation.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FrightAndUnloadingText.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FrightAndUnloadingText.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Documents.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DeliveryAddress1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DeliveryAddress2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DeliveryAddress3.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_Commissioning.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd3.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_RefDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_RefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PaymentTerms.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_CSTPercentage.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PackingCharges.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Forwarding.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Despatch.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Insurance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Warranty.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Erection_Charges.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_installationCharges.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FrightAndUnloadingText.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Transportation.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Commissioning.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Documents.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DeliveryAddress1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DeliveryAddress2.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_DeliveryAddress3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DelvAdd1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DelvAdd2.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_DelvAdd3.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_RefDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DeliveryTerms.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PaymentTerms.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RefNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PackingCharges.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Forwarding.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Despatch.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Insurance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Warranty.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Erection_Charges.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_installationCharges.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Transportation.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Commissioning.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DelvAdd1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DelvAdd2.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_DelvAdd3.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_FrightAndUnloadingText.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Documents.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DeliveryAddress1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DeliveryAddress2.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_DeliveryAddress3.KeyPress, AddressOf TextBoxControlKeyPress




        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Item_PO_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Item_PO_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Picture.Visible = True Then
                    btn_ClosePicture_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Terms.Visible = True Then
                    btn_CloseTerms_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

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
        Dim vclm_indx = 0
        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing


            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf dgv_ActCtrlName = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details
            End If


            If IsNothing(dgv1) = False Then

                With dgv1

                    vclm_indx = 9
                    If dgv_Details.Columns(16).Visible Then
                        vclm_indx = 16
                    ElseIf dgv_Details.Columns(11).Visible Then
                        vclm_indx = 11
                    End If


                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= vclm_indx Then '.ColumnCount - 11 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If txt_DiscPercentage.Visible And txt_DiscPercentage.Enabled Then
                                    txt_DiscPercentage.Focus()
                                    'Else
                                    'txt_DiscPercentage.Focus()
                                End If


                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 5 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(7)
                        ElseIf .CurrentCell.ColumnIndex = 7 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(9)
                        ElseIf .CurrentCell.ColumnIndex = 9 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(11)
                        ElseIf .CurrentCell.ColumnIndex = 11 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(16)
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then
                                    cbo_Ledger.Focus()

                                Else
                                    dtp_date.Focus()

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 7 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(5)
                        ElseIf .CurrentCell.ColumnIndex = 9 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(7)
                        ElseIf .CurrentCell.ColumnIndex = 11 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(9)
                        ElseIf .CurrentCell.ColumnIndex = 16 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(11)

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
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean
        Dim i As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Stores_Item_PO_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  Where a.PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_PoNo.Text = dt1.Rows(0).Item("PO_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("PO_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString

                '  cbo_Despatch.Text = dt1.Rows(0).Item("Despatch").ToString

                txt_DeliveryTerms.Text = dt1.Rows(0).Item("Delivery_Terms").ToString
                cbo_PaymentTerms.Text = dt1.Rows(0).Item("Payment_Terms").ToString
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                txt_RefNo.Text = dt1.Rows(0).Item("Ref_No").ToString
                dtp_RefDate.Text = dt1.Rows(0).Item("Ref_Date").ToString
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString

                txt_Documents.Text = dt1.Rows(0).Item("Documents").ToString
                txt_Insurance.Text = dt1.Rows(0).Item("Insurance").ToString
                txt_Warranty.Text = dt1.Rows(0).Item("Warranty").ToString
                txt_installationCharges.Text = dt1.Rows(0).Item("Installation_Charges").ToString
                txt_Transportation.Text = dt1.Rows(0).Item("Transportation").ToString
                txt_FrightAndUnloadingText.Text = dt1.Rows(0).Item("FrightAndUnloading_Text").ToString
                txt_Erection_Charges.Text = dt1.Rows(0).Item("Erection_Charges").ToString
                txt_DeliveryAddress1.Text = dt1.Rows(0).Item("Delivery_Address1").ToString
                txt_DeliveryAddress2.Text = dt1.Rows(0).Item("Delivery_Address2").ToString
                txt_DeliveryAddress3.Text = dt1.Rows(0).Item("Delivery_Address3").ToString


                lbl_GrossAmount.Text = dt1.Rows(0).Item("Total_Amount").ToString

                txt_DiscPercentage.Text = dt1.Rows(0).Item("Disc_Percentage").ToString
                lbl_DiscAmount_Total.Text = dt1.Rows(0).Item("Discount_Amount_Total").ToString
                txt_PackingCharges.Text = dt1.Rows(0).Item("Packing_Charges").ToString
                txt_Forwarding.Text = dt1.Rows(0).Item("Forwarding_Charges").ToString
                lbl_RoundOff.Text = dt1.Rows(0).Item("Round_Off").ToString
                txt_Packinglabel.Text = dt1.Rows(0).Item("Label_Caption1").ToString
                txt_ForwardingLabel.Text = dt1.Rows(0).Item("Label_Caption2").ToString

                txt_labelCaption1.Text = dt1.Rows(0).Item("Terms_Label1").ToString
                txt_labelCaption2.Text = dt1.Rows(0).Item("Terms_Label2").ToString
                txt_LabelCaption3.Text = dt1.Rows(0).Item("Terms_Label3").ToString
                txt_LabelCaption4.Text = dt1.Rows(0).Item("Terms_Label4").ToString
                txt_LabelCaption5.Text = dt1.Rows(0).Item("Terms_Label5").ToString
                txt_LabelCaption6.Text = dt1.Rows(0).Item("Terms_Label6").ToString
                txt_LabelCaption7.Text = dt1.Rows(0).Item("Terms_Label7").ToString
                txt_LabelCaption8.Text = dt1.Rows(0).Item("Terms_Label8").ToString
                'txt_LabelCaption9.Text = dt1.Rows(0).Item("Terms_Label9").ToString
                txt_AssessableValue.Text = dt1.Rows(0).Item("Taxable_Value").ToString

                txt_DelvAdd1.Text = dt1.Rows(0).Item("Desp_DelAdd1").ToString
                txt_DelvAdd2.Text = dt1.Rows(0).Item("Desp_DelAdd2").ToString
                txt_DelvAdd3.Text = dt1.Rows(0).Item("Desp_DelAdd3").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, b.Drawing_No, c.Department_name, d.Unit_name, e.Brand_Name from Stores_Item_PO_Details a INNER JOIN Stores_item_head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Department_Head c ON b.Department_idno = c.Department_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Brand_Head e ON a.Brand_idno = e.Brand_idno where a.PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0
                LockSTS = False

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1

                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)

                        If IsDBNull(dt2.Rows(i).Item("Department_name").ToString) = False Then
                            If Trim(dt2.Rows(i).Item("Department_name").ToString) <> "" Then
                                dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Department_name").ToString
                            Else
                                dgv_Details.Rows(n).Cells(1).Value = Common_Procedures.Department_IdNoToName(con, 1)
                            End If
                        End If

                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Drawing_No").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Item_name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Brand_name").ToString
                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("PO_Quantity").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Unit_name").ToString
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(9).Value = Val(dt2.Rows(i).Item("Disc_Percentage").ToString)
                        ' dgv_Details.Rows(n).Cells(10).Value = Val(dt2.Rows(i).Item("Vat_Percentage").ToString)
                        dgv_Details.Rows(n).Cells(11).Value = Val(dt2.Rows(i).Item("Cancel_Quantiy").ToString)

                        dgv_Details.Rows(n).Cells(12).Value = Val(dt2.Rows(i).Item("PO_Details_SlNo").ToString)
                        dgv_Details.Rows(n).Cells(13).Value = Val(dt2.Rows(i).Item("Purchased_Quantity").ToString) + Val(dt2.Rows(i).Item("PurchaseReturn_Quantity").ToString)

                        dgv_Details.Rows(n).Cells(14).Value = Val(dt2.Rows(i).Item("Disc_Amount").ToString)
                        ' dgv_Details.Rows(n).Cells(15).Value = Val(dt2.Rows(i).Item("Vat_Amount").ToString)
                        dgv_Details.Rows(n).Cells(16).Value = Trim(dt2.Rows(i).Item("Item_Description").ToString)

                        If Val(dgv_Details.Rows(n).Cells(13).Value) <> 0 Then
                            LockSTS = True

                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(i).Cells(j).Style.BackColor = Color.LightGray
                            Next

                        End If

                    Next i

                End If

                da4 = New SqlClient.SqlDataAdapter("Select a.* from Item_PO_GST_Tax_Details a Where a.PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)
                dt4 = New DataTable
                da4.Fill(dt4)

                With dgv_Tax_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt4.Rows.Count > 0 Then

                        For i = 0 To dt4.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = SNo
                            .Rows(n).Cells(1).Value = Trim(dt4.Rows(i).Item("HSN_Code").ToString)
                            .Rows(n).Cells(2).Value = IIf(Val(dt4.Rows(i).Item("Taxable_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("Taxable_Amount").ToString), "############0.00"), "")
                            .Rows(n).Cells(3).Value = IIf(Val(dt4.Rows(i).Item("CGST_Percentage").ToString) <> 0, Val(dt4.Rows(i).Item("CGST_Percentage").ToString), "")
                            .Rows(n).Cells(4).Value = IIf(Val(dt4.Rows(i).Item("CGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("CGST_Amount").ToString), "##########0.00"), "")
                            .Rows(n).Cells(5).Value = IIf(Val(dt4.Rows(i).Item("SGST_Percentage").ToString) <> 0, Val(dt4.Rows(i).Item("SGST_Percentage").ToString), "")
                            .Rows(n).Cells(6).Value = IIf(Val(dt4.Rows(i).Item("SGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("SGST_Amount").ToString), "###########0.00"), "")
                            .Rows(n).Cells(7).Value = IIf(Val(dt4.Rows(i).Item("IGST_Percentage").ToString) <> 0, Val(dt4.Rows(i).Item("IGST_Percentage").ToString), "")
                            .Rows(n).Cells(8).Value = IIf(Val(dt4.Rows(i).Item("IGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("IGST_Amount").ToString), "###########0.00"), "")
                        Next i

                    End If

                End With

                'Calculation_Grid_Total()

                ' With dgv_Details_Total
                '.Rows.Clear()
                ' .Rows.Add()
                '  .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                ' .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")
                '  .Rows(0).Cells(12).Value = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "########0.00")
                '  .Rows(0).Cells(13).Value = Format(Val(dt1.Rows(0).Item("Total_VatAmount").ToString), "########0.00")
                '  Calculation_Grid_Total()
                'End With

                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                If LockSTS = True Then
                    cbo_Ledger.Enabled = False
                    cbo_Ledger.BackColor = Color.LightGray
                    cbo_TaxType.Enabled = False
                    cbo_TaxType.BackColor = Color.LightGray
                End If

            Else
                new_record()


            End If

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()
            dt2.Dispose()
            da2.Dispose()

            If dtp_date.Visible And dtp_date.Enabled Then dtp_date.Focus()

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_PoNo.Text)

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Stores_Purchase_Order, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Stores_Purchase_Order, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Store_purchase_Order_entry, New_Entry, Me, con, "Stores_Item_PO_Head", "Po_Code", NewCode, "Po_Date", "(Po_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select sum(Purchased_Quantity+PurchaseReturn_Quantity) from Stores_Item_PO_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Alreay some quantity purchased/retuned against this PO", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Stores_Item_PO_Head", "PO_Code", Val(lbl_Company.Tag), NewCode, lbl_PoNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "PO_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Stores_Item_PO_Details", "PO_Code", Val(lbl_Company.Tag), NewCode, lbl_PoNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Item_IdNo , Brand_IdNo, PO_Quantity, Unit_idNo, Rate, Amount, Cancel_Quantiy, Purchased_Quantity, PurchaseReturn_Quantity,                 Disc_Percentage         ,Disc_Amount,Item_Description,Taxable_Value ,HSN_Code ,      GST_Percentage", "Sl_No", "PO_Code, For_OrderBy, Company_IdNo, PO_No, PO_Date, Ledger_Idno", trans)

            cmd.CommandText = "Delete from Stores_Item_PO_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Stores_Item_PO_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
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


            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Item_DisplayName from Stores_Item_AlaisHead order by Item_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_Item.DataSource = dt1
            cbo_Filter_Item.DisplayMember = "Item_DisplayName"

            cbo_Filter_Item.Text = ""
            cbo_Filter_Item.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Stores_Purchase_Order, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Stores_Purchase_Order, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
            If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Store_purchase_Order_entry, New_Entry, Me) = False Then Exit Sub

            inpno = InputBox("Enter New P.O No.", "FOR NEW NO INSERTION...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select PO_No from Stores_Item_PO_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PO_Code = '" & Trim(Pk_Condition) & Trim(RefCode) & "' and PO_Code LIKE '" & Trim(Pk_Condition) & "%'", con)
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
                    MessageBox.Show("Invalid P.O No", "DOES NOT INSERT PO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_PoNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW PO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 PO_No from Stores_Item_PO_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PO_Code like '" & Trim(Pk_Condition) & "%" & Common_Procedures.FnYearCode & "' Order by for_Orderby, PO_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_PoNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 PO_No from Stores_Item_PO_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and PO_Code like '" & Trim(Pk_Condition) & "%" & Common_Procedures.FnYearCode & "' Order by for_Orderby, PO_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_PoNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 PO_No from Stores_Item_PO_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and PO_Code like '" & Trim(Pk_Condition) & "%" & Common_Procedures.FnYearCode & "' Order by for_Orderby desc, PO_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 PO_No from Stores_Item_PO_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PO_Code like '" & Trim(Pk_Condition) & "%" & Common_Procedures.FnYearCode & "' Order by for_Orderby desc, PO_No desc", con)
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
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_PoNo.Text = Common_Procedures.get_MaxCode(con, "Stores_Item_PO_Head", "PO_Code", "For_OrderBy", "PO_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_PoNo.ForeColor = Color.Red

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try



    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter P.O No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select PO_No from Stores_Item_PO_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PO_Code = '" & Trim(Pk_Condition) & Trim(RefCode) & "'", con)
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
                MessageBox.Show("P.O No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim Led_IdNo As Integer = 0
        Dim Dep_ID As Integer = 0
        Dim Item_ID As Integer = 0
        Dim Unit_ID As Integer = 0
        Dim Brand_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim vTotQty As Single = 0
        Dim vTotAmt As Single = 0
        Dim vTotDiscAmt As Single = 0
        Dim Nr As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_PoNo.Text)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Stores_Purchase_Order, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Store_purchase_Order_entry, New_Entry, Me, con, "Stores_Item_PO_Head", "Po_Code", NewCode, "Po_Date", "(Po_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Po_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Po_No desc", dtp_date.Value.Date) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If

        If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If


        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Val(Led_IdNo) = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                Item_ID = Common_Procedures.itemalais_NameToIdNo(con, dgv_Details.Rows(i).Cells(3).Value)
                If Item_ID = 0 Then
                    MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                    End If
                    Exit Sub
                End If

                Brand_ID = Common_Procedures.Brand_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value)
                'If Brand_ID = 0 Then
                '    MessageBox.Show("Invalid Brand Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)
                '    End If
                '    Exit Sub
                'End If

                Unit_ID = Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(6).Value)
                If Unit_ID = 0 Then
                    MessageBox.Show("Invalid Unit Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(6)
                    End If
                    Exit Sub
                End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1391" Then
                    If Val(dgv_Details.Rows(i).Cells(7).Value) = 0 Then
                        MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(7)
                        End If
                        Exit Sub
                    End If
                End If



            End If

        Next

        vTotQty = 0 : vTotAmt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotQty = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            vTotDiscAmt = Val(dgv_Details_Total.Rows(0).Cells(12).Value())
        End If

        If vTotQty = 0 Then
            MessageBox.Show("Invalid P.O Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dgv_Details.Enabled And dgv_Details.Visible Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            End If
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_PoNo.Text = Common_Procedures.get_MaxCode(con, "Stores_Item_PO_Head", "PO_Code", "For_OrderBy", "PO_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PoDate", dtp_date.Value.Date)
            cmd.Parameters.AddWithValue("@RefDate", dtp_RefDate.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Stores_Item_PO_Head(PO_Code                                   , Company_IdNo                     , PO_No                        , for_OrderBy                                                           , PO_Date, Ledger_IdNo               , Delivery_Terms                        , Payment_Terms                        ,        Remarks                         , Total_Quantity           , Total_Amount             , Discount_Amount             ,        Net_Amount                         , Ref_No                       , Ref_Date, Documents                        , Tax_Type                       , Disc_Percentage                          , Discount_Amount_Total                      , Packing_Charges                         ,  Forwarding_Charges                                  , Round_Off                         ,Label_Caption1                        ,Label_Caption2                          ,Insurance                         ,Warranty                         ,Erection_Charges                         ,Transportation                         ,Installation_Charges                        ,FrightAndUnloading_Text                        ,Delivery_Address1                         , Delivery_Address2                         , Delivery_Address3       ,  Terms_Label1                        , Terms_Label2                         ,Terms_Label3                          , Terms_Label4                         , Terms_Label5                         , Terms_Label6                         , Terms_Label7                         , Terms_Label8                         , Total_CGST_Amount                        ,Total_SGST_Amount                  , Total_IGST_Amount                , Taxable_Value                         ,           Desp_DelAdd1 ,                           Desp_DelAdd2,                         Desp_DelAdd3 ) " & _
                                                        "Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PoNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PoNo.Text))) & ", @PoDate, " & Str(Val(Led_IdNo)) & ", '" & Trim(txt_DeliveryTerms.Text) & "', '" & Trim(cbo_PaymentTerms.Text) & "',  '" & Trim(txt_Remarks.Text) & "', " & Str(Val(vTotQty)) & ", " & Str(Val(vTotAmt)) & "," & Str(Val(vTotDiscAmt)) & "," & Str(Val(CSng(lbl_NetAmount.Text))) & ",'" & Trim(txt_RefNo.Text) & "',@RefDate   ,'" & Trim(txt_Documents.Text) & "','" & Trim(cbo_TaxType.Text) & "'," & Str(Val(txt_DiscPercentage.Text)) & " ," & Str(Val(lbl_DiscAmount_Total.Text)) & " ," & Str(Val(txt_PackingCharges.Text)) & "," & Str(Val(txt_Forwarding.Text)) & "," & Str(Val(lbl_RoundOff.Text)) & ", '" & Trim(txt_Packinglabel.Text) & "','" & Trim(txt_ForwardingLabel.Text) & "','" & Trim(txt_Insurance.Text) & "','" & Trim(txt_Warranty.Text) & "','" & Trim(txt_Erection_Charges.Text) & "','" & Trim(txt_Transportation.Text) & "','" & Trim(txt_installationCharges.Text) & "','" & Trim(txt_FrightAndUnloadingText.Text) & "','" & Trim(txt_DeliveryAddress1.Text) & "' , '" & Trim(txt_DeliveryAddress2.Text) & "' , '" & Trim(txt_DeliveryAddress3.Text) & "','" & Trim(txt_labelCaption1.Text) & "','" & Trim(txt_labelCaption2.Text) & "','" & Trim(txt_LabelCaption3.Text) & "','" & Trim(txt_LabelCaption4.Text) & "','" & Trim(txt_LabelCaption5.Text) & "','" & Trim(txt_LabelCaption6.Text) & "','" & Trim(txt_LabelCaption7.Text) & "','" & Trim(txt_LabelCaption8.Text) & "',   " & Val(lbl_CGST_Amount.Text) & "      , " & Val(lbl_SGST_Amount.Text) & " , " & Val(lbl_IGST_Amount.Text) & " ,   " & Val(txt_AssessableValue.Text) & " ,'" & Trim(txt_DelvAdd1.Text) & "','" & Trim(txt_DelvAdd2.Text) & "', '" & Trim(txt_DelvAdd3.Text) & "')"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Stores_Item_PO_Head", "PO_Code", Val(lbl_Company.Tag), NewCode, lbl_PoNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "PO_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Stores_Item_PO_Details", "PO_Code", Val(lbl_Company.Tag), NewCode, lbl_PoNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Item_IdNo , Brand_IdNo, PO_Quantity, Unit_idNo, Rate, Amount, Cancel_Quantiy, Purchased_Quantity, PurchaseReturn_Quantity,                 Disc_Percentage         ,Disc_Amount,Item_Description,Taxable_Value ,HSN_Code ,      GST_Percentage", "Sl_No", "PO_Code, For_OrderBy, Company_IdNo, PO_No, PO_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Stores_Item_PO_Head set PO_Date= @PoDate,Ref_Date = @RefDate, Ledger_IdNo = " & Str(Val(Led_IdNo)) & ", Delivery_Terms = '" & Trim(txt_DeliveryTerms.Text) & "', Payment_Terms = '" & Trim(cbo_PaymentTerms.Text) & "', Remarks = '" & Trim(txt_Remarks.Text) & "', Total_Quantity = " & Str(Val(vTotQty)) & ", Total_Amount = " & Str(Val(vTotAmt)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Discount_Amount = " & Str(Val(vTotDiscAmt)) & ",Ref_No ='" & Trim(txt_RefNo.Text) & "',Documents ='" & Trim(txt_Documents.Text) & "',  Tax_Type  ='" & Trim(cbo_TaxType.Text) & "', Disc_Percentage =" & Str(Val(txt_DiscPercentage.Text)) & " ,Discount_Amount_Total =" & Str(Val(lbl_DiscAmount_Total.Text)) & " ,Packing_Charges =" & Str(Val(txt_PackingCharges.Text)) & ",Forwarding_Charges =" & Str(Val(txt_Forwarding.Text)) & ",Round_Off =" & Str(Val(lbl_RoundOff.Text)) & " ,Label_Caption1 = '" & Trim(txt_Packinglabel.Text) & "',Label_Caption2 ='" & Trim(txt_ForwardingLabel.Text) & "',Insurance ='" & Trim(txt_Insurance.Text) & "',Warranty ='" & Trim(txt_Warranty.Text) & "',Erection_Charges='" & Trim(txt_Erection_Charges.Text) & "',Transportation='" & Trim(txt_Transportation.Text) & "',Installation_Charges ='" & Trim(txt_installationCharges.Text) & "',FrightAndUnloading_Text = '" & Trim(txt_FrightAndUnloadingText.Text) & "',Delivery_Address1 ='" & Trim(txt_DeliveryAddress1.Text) & "' ,Delivery_Address2 = '" & Trim(txt_DeliveryAddress2.Text) & "' ,Delivery_Address3 = '" & Trim(txt_DeliveryAddress3.Text) & "', Terms_Label1 ='" & Trim(txt_labelCaption1.Text) & "',Terms_Label2 = '" & Trim(txt_labelCaption2.Text) & "',Terms_Label3 = '" & Trim(txt_LabelCaption3.Text) & "',Terms_Label4 = '" & Trim(txt_LabelCaption4.Text) & "',Terms_Label5 = '" & Trim(txt_LabelCaption5.Text) & "',Terms_Label6 = '" & Trim(txt_LabelCaption6.Text) & "',Terms_Label7 = '" & Trim(txt_LabelCaption7.Text) & "',Terms_Label8 = '" & Trim(txt_LabelCaption8.Text) & "' , Taxable_Value = " & Val(txt_AssessableValue.Text) & " ,Total_CGST_Amount = " & Val(lbl_CGST_Amount.Text) & " , Total_SGST_Amount = " & Val(lbl_SGST_Amount.Text) & ",   Total_IGST_Amount =" & Val(lbl_IGST_Amount.Text) & ", Desp_DelAdd1='" & Trim(txt_DelvAdd1.Text) & "', Desp_DelAdd2 ='" & Trim(txt_DelvAdd2.Text) & "' , Desp_DelAdd3 ='" & Trim(txt_DelvAdd3.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Stores_Item_PO_Head", "PO_Code", Val(lbl_Company.Tag), NewCode, lbl_PoNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "PO_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Delete from Stores_Item_PO_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Purchased_Quantity = 0 and PurchaseReturn_Quantity = 0 "
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        Item_ID = Common_Procedures.itemalais_NameToIdNo(con, dgv_Details.Rows(i).Cells(3).Value, tr)

                        Brand_ID = Common_Procedures.Brand_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value, tr)

                        Unit_ID = Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(6).Value, tr)

                        cmd.CommandText = "Update Stores_Item_PO_Details set PO_Date = @PoDate, Ledger_IdNo = " & Str(Val(Led_IdNo)) & ", Sl_No = " & Str(Val(Sno)) & ", Item_Idno = " & Str(Val(Item_ID)) & ", Brand_IdNo = " & Str(Val(Brand_ID)) & ", PO_Quantity = " & Str(Val(.Rows(i).Cells(5).Value)) & ", Unit_Idno = " & Val(Unit_ID) & ", Rate = " & Str(Val(.Rows(i).Cells(7).Value)) & ", Amount = " & Str(Val(.Rows(i).Cells(8).Value)) & ",Disc_Percentage =" & Str(Val(.Rows(i).Cells(9).Value)) & ", Cancel_Quantiy = " & Str(Val(.Rows(i).Cells(11).Value)) & ",Disc_Amount= " & Str(Val(.Rows(i).Cells(14).Value)) & ",Item_Description= '" & Trim(.Rows(i).Cells(16).Value) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and PO_Details_SlNo = " & Str(Val(.Rows(i).Cells(12).Value))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 And Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then
                            cmd.CommandText = "Insert into Stores_Item_PO_Details ( PO_Code                              , Company_IdNo                     , PO_No                        , for_OrderBy                                                            , PO_Date, Ledger_IdNo               , Sl_No                , Item_IdNo                , Brand_IdNo                , PO_Quantity                              , Unit_idNo           , Rate                                     , Amount                                   , Cancel_Quantiy                            , Purchased_Quantity, PurchaseReturn_Quantity,                 Disc_Percentage         ,Disc_Amount                               ,Item_Description                        ,             Taxable_Value             ,           HSN_Code                   ,      GST_Percentage                               ) " & _
                                          "   Values                        ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PoNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PoNo.Text))) & ", @PoDate , " & Str(Val(Led_IdNo)) & ", " & Str(Val(Sno)) & ", " & Str(Val(Item_ID)) & ", " & Str(Val(Brand_ID)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Val(Unit_ID) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & ", 0                 , 0                      ," & Str(Val(.Rows(i).Cells(9).Value)) & "," & Str(Val(.Rows(i).Cells(14).Value)) & ",'" & Trim(.Rows(i).Cells(16).Value) & "'," & Trim(.Rows(i).Cells(17).Value) & " ,'" & Trim(.Rows(i).Cells(18).Value) & "', " & Trim(.Rows(i).Cells(19).Value) & " )"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Stores_Item_PO_Details", "PO_Code", Val(lbl_Company.Tag), NewCode, lbl_PoNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Item_IdNo , Brand_IdNo, PO_Quantity, Unit_idNo, Rate, Amount, Cancel_Quantiy, Purchased_Quantity, PurchaseReturn_Quantity,                 Disc_Percentage         ,Disc_Amount,Item_Description,Taxable_Value ,HSN_Code ,      GST_Percentage", "Sl_No", "PO_Code, For_OrderBy, Company_IdNo, PO_No, PO_Date, Ledger_Idno", tr)

            End With

            cmd.CommandText = "Delete from Item_PO_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Tax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Item_PO_GST_Tax_Details   ( PO_Code                                 ,    Company_IdNo                  ,    PO_No                     ,                               for_OrderBy                              , PO_Date      ,         Ledger_IdNo       ,            Sl_No     , HSN_Code                               ,      Taxable_Amount                      ,      CGST_Percentage                    ,       CGST_Amount                         ,      SGST_Percentage                     ,                    SGST_Amount          ,                    IGST_Percentage       ,                   IGST_Amount ) " & _
                                                "     Values               (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PoNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PoNo.Text))) & ",    @PoDate    , " & Str(Val(Led_IdNo)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & "  ," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & ") "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            tr.Commit()


            move_record(lbl_PoNo.Text)


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()

            If InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_1") > 0 Then
                MessageBox.Show("Invalid Purchase quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_2") > 0 Then
                MessageBox.Show("Invalid Cancel quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_3") > 0 Or InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_4") > 0 Then
                MessageBox.Show("Invalid Purchase Return quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_5") > 0 Then
                MessageBox.Show("Invalid P.O quantity, Lesser than Purchase/Cancel Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If



        Finally

            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        End Try

    End Sub

    Private Sub cbo_Grid_Department_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Department.KeyDown

        cbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Department, Nothing, Nothing, "Department_HEAD", "Department_name", "", "(Department_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Department.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                e.Handled = True
                If .CurrentRow.Index <= 0 Then
                    ' cbo_TaxType.Focus()
                    txt_DelvAdd3.Focus()
                Else
                    .Focus()
                    .CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index - 1).Cells(11)

                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_Department.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                e.Handled = True
                'If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then

                '    If txt_CSTPercentage.Visible And txt_CSTPercentage.Enabled Then
                '        txt_CSTPercentage.Focus()
                '    Else
                '        txt_DiscPercentage.Focus()

                '    End If

                'Else
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                'End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_Department_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Department.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Department, Nothing, "Department_Head", "Department_name", "", "(Department_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                'If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                '    If txt_CSTPercentage.Visible And txt_CSTPercentage.Enabled Then
                '        txt_CSTPercentage.Focus()
                '    Else
                '        txt_DiscPercentage.Focus()
                '    End If


                'Else
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                'End If
            End With



        End If
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_date, txt_RefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and AccountsGroup_IdNo = 14 ) ", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_RefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and AccountsGroup_IdNo = 14 ) ", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim item_nm As String
        Dim Unt_nm As String
        Dim dno As String
        Dim dep_idno As Integer = 0

        If e.ColumnIndex = 2 Then

            If Trim(dgv_Details.Rows(e.RowIndex).Cells(3).Value) = "" Or Trim(UCase(vdgv_DrawNo)) <> Trim(UCase(dgv_Details.Rows(e.RowIndex).Cells(2).Value)) Then

                dep_idno = Common_Procedures.Department_NameToIdNo(con, dgv_Details.Rows(e.RowIndex).Cells(1).Value)
                dno = dgv_Details.Rows(e.RowIndex).Cells(2).Value

                Da = New SqlClient.SqlDataAdapter("select a.Item_name, b.unit_name from Stores_item_head a left outer join unit_head b on a.unit_idno = b.unit_idno where a.department_idno = " & Str(Val(dep_idno)) & " and a.drawing_no = '" & Trim(dno) & "'", con)
                Da.Fill(Dt)

                item_nm = ""
                Unt_nm = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        item_nm = Trim(Dt.Rows(0).Item("Item_name").ToString)
                        Unt_nm = Trim(Dt.Rows(0).Item("unit_name").ToString)
                    End If
                End If

                Dt.Dispose()
                Da.Dispose()

                dgv_Details.Rows(e.RowIndex).Cells(3).Value = Trim(item_nm)
                dgv_Details.Rows(e.RowIndex).Cells(6).Value = Trim(Unt_nm)

            End If

        End If

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim rect As Rectangle
        Dim dep_idno As Integer = 0
        Dim Condt As String

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            vdgv_DrawNo = dgv_Details.Rows(e.RowIndex).Cells(2).Value

            If e.ColumnIndex = 1 And Val(.Rows(e.RowIndex).Cells(13).Value) = 0 Then

                If cbo_Grid_Department.Visible = False Or Val(cbo_Grid_Department.Tag) <> e.RowIndex Then

                    cbo_Grid_Department.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Department_Name from Department_Head order by Department_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Department.DataSource = Dt1
                    cbo_Grid_Department.DisplayMember = "Department_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Department.Left = .Left + rect.Left
                    cbo_Grid_Department.Top = .Top + rect.Top

                    cbo_Grid_Department.Width = rect.Width
                    cbo_Grid_Department.Height = rect.Height
                    cbo_Grid_Department.Text = .CurrentCell.Value

                    cbo_Grid_Department.Tag = Val(e.RowIndex)
                    cbo_Grid_Department.Visible = True

                    cbo_Grid_Department.BringToFront()
                    cbo_Grid_Department.Focus()

                End If

            Else
                cbo_Grid_Department.Visible = False

            End If

            If e.ColumnIndex = 2 And vCloPic_STS = False Then
                btn_ShowPicture_Click(sender, e)
            Else
                pnl_Picture.Visible = False
            End If

            If e.ColumnIndex = 3 And Val(.Rows(e.RowIndex).Cells(13).Value) = 0 Then

                If cbo_Grid_Item.Visible = False Or Val(cbo_Grid_Item.Tag) <> e.RowIndex Then

                    dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Grid_Department.Text))

                    Condt = ""
                    If dep_idno <> 0 And dep_idno <> 1 Then Condt = " Where (Item_idno = 0 or Department_idno = " & Str(Val(dep_idno)) & ")"

                    cbo_Grid_Item.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Item_DisplayName from Stores_Item_AlaisHead " & Condt & " order by Item_DisplayName", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_Item.DataSource = Dt2
                    cbo_Grid_Item.DisplayMember = "Item_DisplayName"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Item.Left = .Left + rect.Left
                    cbo_Grid_Item.Top = .Top + rect.Top

                    cbo_Grid_Item.Width = rect.Width
                    cbo_Grid_Item.Height = rect.Height
                    cbo_Grid_Item.Text = .CurrentCell.Value

                    cbo_Grid_Item.Tag = Val(e.RowIndex)
                    cbo_Grid_Item.Visible = True

                    cbo_Grid_Item.BringToFront()
                    cbo_Grid_Item.Focus()

                End If

            Else
                cbo_Grid_Item.Visible = False

            End If


            If e.ColumnIndex = 4 And Val(.Rows(e.RowIndex).Cells(13).Value) = 0 Then

                If cbo_Grid_Brand.Visible = False Or Val(cbo_Grid_Brand.Tag) <> e.RowIndex Then

                    cbo_Grid_Brand.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Brand_Name from Brand_Head order by Brand_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_Brand.DataSource = Dt3
                    cbo_Grid_Brand.DisplayMember = "Brand_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Brand.Left = .Left + rect.Left
                    cbo_Grid_Brand.Top = .Top + rect.Top

                    cbo_Grid_Brand.Width = rect.Width
                    cbo_Grid_Brand.Height = rect.Height
                    cbo_Grid_Brand.Text = .CurrentCell.Value

                    cbo_Grid_Brand.Tag = Val(e.RowIndex)
                    cbo_Grid_Brand.Visible = True

                    cbo_Grid_Brand.BringToFront()
                    cbo_Grid_Brand.Focus()


                End If


            Else
                cbo_Grid_Brand.Visible = False

            End If


            'If e.ColumnIndex = 6 Then

            '    If cbo_Grid_Unit.Visible = False Or Val(cbo_Grid_Unit.Tag) <> e.RowIndex Then

            '        cbo_Grid_Unit.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Unit_Name from Unit_Head order by Unit_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt4)
            '        cbo_Grid_Unit.DataSource = Dt4
            '        cbo_Grid_Unit.DisplayMember = "Unit_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Grid_Unit.Left = .Left + rect.Left
            '        cbo_Grid_Unit.Top = .Top + rect.Top

            '        cbo_Grid_Unit.Width = rect.Width
            '        cbo_Grid_Unit.Height = rect.Height
            '        cbo_Grid_Unit.Text = .CurrentCell.Value

            '        cbo_Grid_Unit.Tag = Val(e.RowIndex)
            '        cbo_Grid_Unit.Visible = True

            '        cbo_Grid_Unit.BringToFront()
            '        cbo_Grid_Unit.Focus()



            '    End If


            'Else
            '    cbo_Grid_Unit.Visible = False

            'End If

        End With

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        Try

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If e.ColumnIndex = 5 Or e.ColumnIndex = 7 Or e.ColumnIndex = 9 Or e.ColumnIndex = 10 Then

                        .CurrentRow.Cells(8).Value = Format(Val(.CurrentRow.Cells(5).Value) * Val(.CurrentRow.Cells(7).Value), "#########0.00")

                        Calculation_Grid_Total()

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        'dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)

        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> 0 Then
                        e.Handled = True
                    End If

                ElseIf .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 11 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                ElseIf .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Or Val(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            dgv_ActCtrlName = dgtxt_Details.Name
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown

        Try
            vcbo_KeyDwnVal = e.KeyValue

            'With dgv_Details

            '    If e.KeyCode = Keys.Left Then
            '        If .CurrentCell.ColumnIndex <= 1 Then
            '            If .CurrentCell.RowIndex = 0 Then
            '                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then
            '                    cbo_Ledger.Focus()
            '                Else
            '                    dtp_date.Focus()
            '                End If

            '            Else
            '                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)
            '            End If
            '        End If
            '    End If

            'End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgv_Details.KeyPress
        Try

            With dgv_Details

                If .Visible Then
                    If .CurrentCell.ColumnIndex = 2 Then
                        If Val(.Rows(.CurrentCell.RowIndex).Cells(12).Value) <> 0 Then
                            e.Handled = True
                        End If

                    ElseIf .CurrentCell.ColumnIndex = 5 Then
                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If



                    End If
                End If

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If Val(.Rows(n).Cells(13).Value) = 0 Then

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = i + 1
                    Next

                End If


            End With
            Calculation_Grid_Total()
        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_Grid_Item_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Item.GotFocus
        Dim dep_idno As Integer = 0
        Dim Itm_idno As Integer = 0
        Dim Condt As String

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value))

        Condt = ""
        If dep_idno <> 0 And dep_idno <> -1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        vCbo_ItmNm = Trim(cbo_Grid_Item.Text)

    End Sub

    Private Sub cbo_Grid_Item_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Item.KeyDown
        Dim dep_idno As Integer = 0
        Dim Condt As String

        cbo_KeyDwnVal = e.KeyValue

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Grid_Department.Text))

        Condt = ""
        If dep_idno <> 0 And dep_idno <> -1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Item, Nothing, Nothing, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Item.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Item.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If Trim(.Rows(.CurrentRow.Index).Cells(3).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then

                    'If txt_CSTPercentage.Visible And txt_CSTPercentage.Enabled Then
                    '    txt_CSTPercentage.Focus()
                    'Else
                    '    txt_DiscPercentage.Focus()

                    'End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End If

        End With

    End Sub

    Private Sub cbo_Grid_Item_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Item.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dno_nm As String
        Dim Unt_nm As String
        Dim Dep_nm As String
        Dim Brand_nm As String
        Dim dep_idno As Integer = 0
        Dim Itm_idno As Integer = 0
        Dim Condt As String
        Dim Rate As Single = 0
        Dim tax As Single = 0

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value))

        Condt = ""
        If dep_idno <> 0 And dep_idno <> -1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Item, Nothing, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            '  If Asc(e.KeyChar) = 13 Then

            'End If
            If Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6).Value) = "" Or Trim(UCase(vCbo_ItmNm)) <> Trim(UCase(cbo_Grid_Item.Text)) Then

                Itm_idno = Common_Procedures.itemalais_NameToIdNo(con, Trim(cbo_Grid_Item.Text))

                da = New SqlClient.SqlDataAdapter("select a.Drawing_No,a.Tax_Percentage, b.unit_name, c.department_name , Sd.* , bh.Brand_Name from Stores_item_head a left outer join Stores_Item_Details Sd on a.Item_Idno = Sd.Item_Idno left outer join Brand_Head bh on sd.Brand_Idno = bh.Brand_Idno left outer join unit_head b on a.unit_idno = b.unit_idno left outer join Department_Head c ON a.Department_IdNo = c.Department_IdNo Where a.item_IdNo = " & Str(Val(Itm_idno)), con)
                'da = New SqlClient.SqlDataAdapter("select a.Drawing_No,a.Rate,a.Tax_Percentage, b.unit_name, c.department_name from Stores_item_head a left outer join unit_head b on a.unit_idno = b.unit_idno left outer join Department_Head c ON a.Department_IdNo = c.Department_IdNo Where a.item_IdNo = " & Str(Val(Itm_idno)), con)
                da.Fill(dt)

                Dep_nm = ""
                dno_nm = ""
                Unt_nm = ""
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        Dep_nm = Trim(dt.Rows(0).Item("department_name").ToString)
                        dno_nm = Trim(dt.Rows(0).Item("Drawing_No").ToString)
                        Unt_nm = Trim(dt.Rows(0).Item("unit_name").ToString)
                        Rate = Val(dt.Rows(0).Item("Rate").ToString)
                        tax = Val(dt.Rows(0).Item("Tax_Percentage").ToString)


                        If dt.Rows.Count = 1 Then
                            Brand_nm = Trim(dt.Rows(0).Item("Brand_Name").ToString)
                        End If




                    End If
                End If

                dt.Dispose()
                da.Dispose()

                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value = Trim(Dep_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value = Trim(dno_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(4).Value = Trim(Brand_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6).Value = Trim(Unt_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7).Value = Val(Rate)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(10).Value = Val(tax)



            End If

            With dgv_Details

                ' .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_Item.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    txt_DiscPercentage.Focus()


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With
        End If

        '  If Asc(e.KeyChar) = 13 Then
        'If Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(3).Value) = "" And dgv_Details.CurrentRow.Index = dgv_Details.Rows.Count - 1 Then
        'If txt_CSTPercentage.Visible And txt_CSTPercentage.Enabled Then
        'txt_CSTPercentage.Focus()
        ' Else
        ' txt_DiscPercentage.Focus()
        'End If
        ' Else
        ' dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(3).Value = Trim(cbo_Grid_Item.Text)
        '  dgv_Details.Focus()
        ' dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(4)

        'End If

        '  End If

    End Sub

    Private Sub cbo_Grid_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Unit.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Unit, Nothing, Nothing, "Unit_Head", "Unit_name", "", "(Unit_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(5)

            End If

            If (e.KeyValue = 40 And cbo_Grid_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If Trim(.Rows(.CurrentRow.Index).Cells(6).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then

                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Item_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Item.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Item.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Item_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Item.TextChanged
        Try
            If cbo_Grid_Item.Visible Then


                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Item.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(3).Value = Trim(cbo_Grid_Item.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Unit, Nothing, "Unit_Head", "Unit_name", "", "(Unit_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(6).Value = Trim(cbo_Grid_Unit.Text)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7)

        End If
    End Sub

    Private Sub cbo_Grid_Unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Unit.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Unit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Unit.TextChanged
        Try
            If cbo_Grid_Unit.Visible Then


                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Unit.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 6 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Unit.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
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
        Dim Item_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Item_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.PO_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.PO_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.PO_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Item.Text) <> "" Then
                Item_IdNo = Common_Procedures.itemalais_NameToIdNo(con, cbo_Filter_Item.Text)
            End If

            If Val(Item_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Item_IdNo))
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.PO_Quantity,b.Amount ,c.item_name, d.unit_name from Stores_Item_PO_Head a left outer join Stores_Item_PO_Details b on a.PO_Code = b.PO_Code left outer join Stores_item_head c on b.item_idno = c.item_idno left outer join unit_head d on b.unit_idno = d.unit_idno  where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.PO_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by PO_Date, for_orderby, PO_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("PO_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("PO_Date").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Item_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("PO_Quantity").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Unit_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

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

    Private Sub cbo_Filter_Item_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Item.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Item, dtp_Filter_ToDate, btn_Filter_Show, "Stores_Item_AlaisHead", "Item_DisplayName", "", "(Item_idno = 0)")
    End Sub


    Private Sub cbo_Filter_Item_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Item.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Item, Nothing, "Stores_Item_AlaisHead", "Item_DisplayName", "", "(Item_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub dtp_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyDown
        'If e.KeyValue = 40 Then
        '    e.Handled = True
        '    SendKeys.Send("{TAB}")
        'End If
        'If e.KeyValue = 38 Then
        '    e.Handled = True
        '    btn_Cancel.Focus()
        'End If
    End Sub

    Private Sub dtp_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_date.KeyPress
        'If Asc(e.KeyChar) = 13 Then
        '    SendKeys.Send("{TAB}")
        'End If
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

    Private Sub cbo_Department_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Department.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Department_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Department.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Brand_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Brand.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Brand, Nothing, Nothing, "Brand_Head", "Brandname", "", "(Brand_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Brand.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)

            End If

            If (e.KeyValue = 40 And cbo_Grid_Brand.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Brand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Brand.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Brand, Nothing, "Brand_HEAD", "Brand_name", "", "(Brand_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(4).Value = Trim(cbo_Grid_Brand.Text)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(5)

        End If
    End Sub

    Private Sub cbo_Grid_Brand_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Brand.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Brand_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Brand.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Department_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Department.TextChanged
        Try
            If cbo_Grid_Department.Visible Then


                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Department.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Department.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dtp_Filter_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_Fromdate.KeyDown
        If e.KeyValue = 40 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            btn_Filter_Show.Focus()
        End If
    End Sub

    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_ToDate.KeyDown
        If e.KeyValue = 40 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            e.Handled = True
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub Calculation_Grid_Total()
        Dim Sno As Integer
        Dim TotQty As Single
        Dim TotAmt As Single
        Dim vatAmt1 As Single = 0
        Dim vatAmt2 As Single = 0
        Dim DiscAmt As Single = 0
        Dim VatAmt As Single = 0
        Dim Ttl_Taxable_Amount As Double

        'Dim VatAss1 As Single = 0, VatPerc1 As Single = 0
        'Dim VatAss2 As Single = 0, VatPerc2 As Single = 0


        Sno = 0
        TotQty = 0
        TotAmt = 0
        'VatAss1 = 0 : VatPerc1 = 0 : vatAmt1 = 0
        'VatAss2 = 0 : VatPerc2 = 0 : vatAmt2 = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(5).Value) <> 0 Then
                    TotQty = TotQty + Val(.Rows(i).Cells(5).Value)
                    TotAmt = TotAmt + Val(.Rows(i).Cells(8).Value)

                    .Rows(i).Cells(14).Value = Val(.Rows(i).Cells(8).Value) * (Val(.Rows(i).Cells(9).Value) / 100)
                    .Rows(i).Cells(17).Value = Val(.Rows(i).Cells(8).Value) - Val(.Rows(i).Cells(14).Value)

                    ' .Rows(i).Cells(15).Value = (Val(.Rows(i).Cells(8).Value) - Val(.Rows(i).Cells(14).Value)) * (Val(.Rows(i).Cells(10).Value) / 100)

                    DiscAmt = DiscAmt + .Rows(i).Cells(14).Value
                    'VatAmt = VatAmt + .Rows(i).Cells(15).Value
                    Ttl_Taxable_Amount = Ttl_Taxable_Amount + Val(.Rows(i).Cells(17).Value())




                End If

            Next
        End With



        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(TotQty)
            .Rows(0).Cells(8).Value = Format(Val(TotAmt), "########0.00")
            .Rows(0).Cells(12).Value = Format(DiscAmt, "########0.00")
            .Rows(0).Cells(13).Value = Format(VatAmt, "########0.00")

            .Rows(0).Cells(14).Value = Format(Ttl_Taxable_Amount, "########0.00")
        End With


        lbl_GrossAmount.Text = Format(Val(TotAmt), "#########0.00")

        GST_Calculation()
        NetAmount_Calculation()

    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub


    Private Sub btn_EnLargePicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EnLargePicture.Click

        If IsNothing(PictureBox1.Image) = False Then

            EnlargePicture.Text = "IMAGE   -   " & dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value
            EnlargePicture.PictureBox2.ClientSize = PictureBox1.Image.Size
            EnlargePicture.PictureBox2.Image = CType(PictureBox1.Image.Clone, Image)
            EnlargePicture.ShowDialog()

            dgv_Details.Focus()
            dgv_Details.CurrentCell.Selected = True

        End If

    End Sub

    Private Sub btn_ShowPicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ShowPicture.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dep_IdNo As Integer

        Dep_IdNo = Common_Procedures.Department_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value)

        PictureBox1.Image = Nothing
        pnl_Picture.Visible = False

        If Val(Dep_IdNo) <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Department_head a where Department_idno <> 1 and Department_idno = " & Str(Val(Dep_IdNo)), con)
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                If IsDBNull(Dt1.Rows(0).Item("Department_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(Dt1.Rows(0).Item("Department_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New System.IO.MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then

                                PictureBox1.Image = Image.FromStream(ms)

                                pnl_Picture.Visible = True
                                pnl_Picture.BringToFront()

                            End If
                        End Using
                    End If
                End If

            End If

        End If

        Dt1.Dispose()
        Da.Dispose()

    End Sub

    Private Sub btn_ClosePicture_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ClosePicture.Click
        vCloPic_STS = True
        pnl_Picture.Visible = False
        dgv_Details.Focus()
        dgv_Details.CurrentCell.Selected = True
        vCloPic_STS = False
    End Sub





    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PrinterName_Dflt As String = ""
        Dim ps As Printing.PaperSize


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Store_purchase_Order_entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Stores_Item_PO_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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
                PrintDocument1.DefaultPageSettings.Landscape = False
                Exit For
            End If
        Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                PrinterName_Dflt = PrintDocument1.PrinterSettings.PrinterName
                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "PurchaseOrder"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\PurchaseOrder.pdf"
                    PrintDocument1.Print()

                Else
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument1.Print()
                    End If

                End If
                PrintDocument1.PrinterSettings.PrinterName = PrinterName_Dflt

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try
                'If PrintFrmt_Letter <> 1 Then
                'If prn_Status <> 1 Then
                prn_InpOpts = ""

                prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "123")
                prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")

                ' End If
                'End If

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If
        Print_PDF_Status = False
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Stores_Item_PO_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.brand_name, d.Unit_Name from Stores_Item_PO_Details a INNER JOIN Stores_item_head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Brand_Head c on a.brand_idno = c.brand_idno LEFT OUTER JOIN Unit_Head d on a.unit_idno = d.unit_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                da2.Dispose()

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        prn_OriDupTri = ""

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "" Then
        '    Printing_Format2(e)
        'End If

        If Common_Procedures.settings.CustomerCode = "1234" Or Common_Procedures.settings.CustomerCode = "1037" Then
            Printing_Format2(e)
        ElseIf Common_Procedures.settings.CustomerCode = "1061" Or Common_Procedures.settings.CustomerCode = "1558" Then
            Printing_Format_3(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
            Printing_Format_1391(e)
        Else
            Printing_Format2(e)
            'Printing_Format1(e)
        End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 65
            .Right = 50
            .Top = 65
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 12, FontStyle.Regular)

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

        TxtHgt = 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 12

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(55)
        ClArr(2) = 290 : ClArr(3) = 100 : ClArr(4) = 70 : ClArr(5) = 95
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        CurY = TMargin

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            prn_DetSNo = prn_DetSNo + 1


                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString) & IIf(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Brand_Name").ToString) <> "", " - ", "") & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Brand_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 30 Then
                                For I = 30 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 30
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 25, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("PO_Quantity").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0
        Dim S1 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.brand_name, d.Unit_Name from Stores_Item_PO_Details a INNER JOIN Stores_item_head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Brand_Head c on a.brand_idno = c.brand_idno LEFT OUTER JOIN Unit_Head d on a.unit_idno = d.unit_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PO_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

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

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString("PURCHASE ORDER", p1Font).Height

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3)
            W1 = e.Graphics.MeasureString("P.O DATE  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO  : ", pFont).Width

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "P.O NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PO_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("PO_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY ADDRESS :  ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "        " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "        " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "        " & prn_HdDt.Rows(0).Item("Delivery_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "DEAR SIRS,     ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We do hereby confirm the following goods subject to our general terms and conditions ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim CenPs As Single = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))

            CenPs = ClAr(1) + ClAr(2) - 50  ' CInt(PageWidth \ 2)


            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TERMS ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Terms").ToString, CenPs + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "PAYMENT TERMS ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Payment_Terms").ToString, CenPs + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "EXCISE TERMS ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Excise_Terms").ToString, CenPs + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "REMARKS", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Remarks").ToString, CenPs + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, CenPs, LnAr(5), CenPs, CurY)


            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        '  Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim ItmName(10) As String
        Dim J As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 45
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

        TxtHgt = 18  '19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 6

        If Trim(prn_HdDt.Rows(0).Item("Delivery_Terms").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Documents").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Insurance").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Warranty").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Erection_Charges").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Transportation").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Installation_Charges").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("FrightAndUnloading_Text").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If




        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(50)
        ClArr(2) = 410 : ClArr(3) = 60 : ClArr(4) = 50 : ClArr(5) = 80
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        'ClArr(2) = 290 : ClArr(3) = 100 : ClArr(4) = 70 : ClArr(5) = 95
        'ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))
        CurY = TMargin

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            prn_DetSNo = prn_DetSNo + 1
                            J = 0
                            For k = 0 To 10
                                ItmName(k) = ""
                            Next


                            ItmName(0) = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString) & IIf(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Description").ToString) <> "", " (" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Description").ToString) & ")", "")

                            If Len(ItmName(0)) > 55 Then
Lp:
                                For I = 55 To 1 Step -1
                                    If Mid$(Trim(ItmName(0)), I, 1) = " " Or Mid$(Trim(ItmName(0)), I, 1) = "," Or Mid$(Trim(ItmName(0)), I, 1) = "." Or Mid$(Trim(ItmName(0)), I, 1) = "-" Or Mid$(Trim(ItmName(0)), I, 1) = "/" Or Mid$(Trim(ItmName(0)), I, 1) = "_" Or Mid$(Trim(ItmName(0)), I, 1) = "(" Or Mid$(Trim(ItmName(0)), I, 1) = ")" Or Mid$(Trim(ItmName(0)), I, 1) = "\" Or Mid$(Trim(ItmName(0)), I, 1) = "[" Or Mid$(Trim(ItmName(0)), I, 1) = "]" Or Mid$(Trim(ItmName(0)), I, 1) = "{" Or Mid$(Trim(ItmName(0)), I, 1) = "}" Then Exit For
                                Next I
                                J = J + 1
                                If I = 0 Then I = 55
                                ItmName(J) = Microsoft.VisualBasic.Left(Trim(ItmName(0)), I - 1)
                                ItmName(0) = Microsoft.VisualBasic.Right(Trim(ItmName(0)), Len(ItmName(0)) - I)

                                If Len(ItmName(0)) > 55 Then
                                    GoTo Lp
                                End If
                            Else
                                ItmName(1) = ItmName(0)
                                ItmName(0) = ""
                            End If




                            CurY = CurY + TxtHgt
                            p1Font = New Font("Calibri", 10, FontStyle.Regular)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 25, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(1)), LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("PO_Quantity").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, p1Font)

                            NoofDets = NoofDets + 1

                            If Trim(ItmName(2)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(2)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmName(3)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(3)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmName(4)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(4)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmName(5)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(5)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmName(6)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(6)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmName(7)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(7)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmName(0)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(0)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                        Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)
                        If Trim(prn_InpOpts) <> "" Then

                            If prn_Count < Len(Trim(prn_InpOpts)) Then


                                If Val(prn_InpOpts) <> "0" Then
                                    prn_DetIndx = 0
                                    prn_DetSNo = 0
                                    prn_PageNo = 0

                                    e.HasMorePages = True
                                    Return
                                End If

                            End If
                        End If

                    End If




                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EmlNo As String
        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0
        Dim S1 As Single = 0
        Dim S As String
        Dim Rfno1 As String = ""
        Dim Rfno2 As String = ""
        Dim I As Integer = 0


        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.brand_name, d.Unit_Name from Stores_Item_PO_Details a INNER JOIN Stores_item_head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Brand_Head c on a.brand_idno = c.brand_idno LEFT OUTER JOIN Unit_Head d on a.unit_idno = d.unit_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PO_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Trim(prn_InpOpts)
                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)


                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If

        End If
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        If Val(dt2.Rows.Count) = 1 Then
            If Trim(prn_OriDupTri) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            End If
        Else
            If Trim(prn_OriDupTri) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 70, CurY - TxtHgt, 1, 0, pFont)
            End If
        End If

        'If Trim(prn_OriDupTri) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 70, CurY - TxtHgt, 1, 0, pFont)
        'End If
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO :" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_CstNo = "GST NO : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EmlNo = "EMAIL : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- PRAKASH Textiles (SOMANUR)

            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Prakash_logo, Drawing.Image), LMargin + 20, CurY, 100, 80)

        End If
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(Cmp_Add1) <> "" Then
            CurY = CurY + strHeight
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        If Trim(Cmp_Add2) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        If Trim(Cmp_Add2) = "" Then
            CurY = CurY + TxtHgt
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EmlNo, LMargin, CurY, 2, PrintWidth, pFont)
        If Trim(Cmp_EmlNo) = "" Then
            'CurY = CurY + TxtHgt
        End If

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString("PURCHASE ORDER", p1Font).Height

        p1Font = New Font("Calibri", 9, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2)
            W1 = e.Graphics.MeasureString("Your Ref NO: ", pFont).Width
            S1 = e.Graphics.MeasureString("TO  : ", pFont).Width

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "P.O NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PO_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("PO_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Your Ref NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ref_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            Rfno1 = Trim(prn_HdDt.Rows(0).Item("Ref_No").ToString)
            Rfno2 = ""
            If Len(Rfno1) > 20 Then
                For I = 20 To 1 Step -1
                    If Mid$(Trim(Rfno1), I, 1) = " " Or Mid$(Trim(Rfno1), I, 1) = "," Or Mid$(Trim(Rfno1), I, 1) = "." Or Mid$(Trim(Rfno1), I, 1) = "-" Or Mid$(Trim(Rfno1), I, 1) = "/" Or Mid$(Trim(Rfno1), I, 1) = "_" Or Mid$(Trim(Rfno1), I, 1) = "(" Or Mid$(Trim(Rfno1), I, 1) = ")" Or Mid$(Trim(Rfno1), I, 1) = "\" Or Mid$(Trim(Rfno1), I, 1) = "[" Or Mid$(Trim(Rfno1), I, 1) = "]" Or Mid$(Trim(Rfno1), I, 1) = "{" Or Mid$(Trim(Rfno1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 20
                Rfno2 = Microsoft.VisualBasic.Right(Trim(Rfno1), Len(Rfno1) - I)
                Rfno1 = Microsoft.VisualBasic.Left(Trim(Rfno1), I)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Trim(Rfno1), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)


            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "GST NO :" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            End If

            If Trim(Rfno2) <> "" Then
                '   CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, Trim(Rfno2), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            'CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Ref_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurY = CurY + 20
            If Trim(prn_HdDt.Rows(0).Item("Owner_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Kind Attention : " & prn_HdDt.Rows(0).Item("Owner_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            End If


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))

            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "We do hereby confirm the following goods subject to our general terms and conditions ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "We are pleased to place our order for the following items:", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT PRICE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT (in Rs)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim CenPs As Single = 0
        Dim W1 As Single = 0
        Dim W2(7) As Single
        Dim S1 As Single = 0
        Dim CurY1 As Single = 0
        Dim rmrk As String = ""
        Dim rmrk1 As String = ""

        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        da1 = New SqlClient.SqlDataAdapter("select a.* from Item_PO_GST_Tax_Details a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        prn_TaxDt = New DataTable
        da1.Fill(prn_TaxDt)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))

            ' CenPs = ClAr(1) + ClAr(2) - 50  ' CInt(PageWidth \ 2)

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            S1 = e.Graphics.MeasureString("Packing Charges & Forwarding charges :", pFont).Width

            CurY1 = CurY - 10

            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            End If

            'If Val(prn_HdDt.Rows(0).Item("Vat_Amount1").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Vat " & Val(prn_HdDt.Rows(0).Item("Vat_Percentage1").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Vat_Amount1").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Vat_Amount2").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Vat " & Val(prn_HdDt.Rows(0).Item("Vat_percentage2").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Vat_Amount2").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If

            'If Val(prn_HdDt.Rows(0).Item("Cst_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "CST " & Val(prn_HdDt.Rows(0).Item("Cst_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Cst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If

            If Val(prn_HdDt.Rows(0).Item("Discount_Amount_Total").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Discount " & IIf(Val(prn_HdDt.Rows(0).Item("Disc_Percentage").ToString) <> 0, Val(prn_HdDt.Rows(0).Item("Disc_Percentage").ToString) & "%", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount_Total").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            End If
            'If Val(prn_HdDt.Rows(0).Item("Excise_Duties").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Excise Duty " & Val(prn_HdDt.Rows(0).Item("Excise_Duties_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Excise_Duties").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Surcharges_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "SurCharges " & Val(prn_HdDt.Rows(0).Item("SurCharges_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Surcharges_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Cess_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Cess " & Val(prn_HdDt.Rows(0).Item("Cess_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Cess_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("EdCess_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Ed.Cess " & Val(prn_HdDt.Rows(0).Item("EdCess_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("EdCess_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            If Val(prn_HdDt.Rows(0).Item("Packing_Charges").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Label_Caption1").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Packing_Charges").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Forwarding_Charges").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Label_Caption2").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Forwarding_Charges").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Taxable_Value").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Taxable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "CGST " & IIf(Val(prn_TaxDt.Rows(0).Item("CGST_Percentage").ToString) <> 0, Val(prn_TaxDt.Rows(0).Item("CGST_Percentage").ToString) & "%", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "SGST " & IIf(Val(prn_TaxDt.Rows(0).Item("SGST_Percentage").ToString) <> 0, Val(prn_TaxDt.Rows(0).Item("SGST_Percentage").ToString) & "%", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "IGST " & IIf(Val(prn_TaxDt.Rows(0).Item("IGST_Percentage").ToString) <> 0, Val(prn_TaxDt.Rows(0).Item("IGST_Percentage").ToString) & "%", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            End If

            CurY1 = CurY1 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 50, CurY1, PageWidth, CurY1)
            LnAr(6) = CurY1
            CurY1 = CurY1 + TxtHgt - 10
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "Net Amount ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, p1Font)
            CurY1 = CurY1 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 50, CurY1, PageWidth, CurY1)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 50, CurY1, LMargin + ClAr(1) + ClAr(2) + 50, LnAr(6))



            pFont = New Font("Calibri", 9, FontStyle.Bold)
            p1Font = New Font("Calibri", 9, FontStyle.Regular)


            W1 = e.Graphics.MeasureString("Billing & Delivery Address :", pFont).Width
            For I = 0 To 7
                W2(I) = e.Graphics.MeasureString(Trim(prn_HdDt.Rows(0).Item("Terms_Label" & I + 1).ToString), pFont).Width
            Next
            If W1 > W2.Max Then
                W1 = W2.Max
            End If

            If Trim(prn_HdDt.Rows(0).Item("Delivery_Terms").ToString) <> "" Then
                CurY = CurY + TxtHgt - 10

                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Delivery_Terms").ToString)
                If Len(rmrk) > 40 Then
                    For I = 40 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 40
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "Delivery Terms ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If


            'If Trim(prn_HdDt.Rows(0).Item("Despatch").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    rmrk1 = ""
            '    rmrk = ""
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("Despatch").ToString)
            '    If Len(rmrk) > 35 Then
            '        For I = 35 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 35
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, " Delivery & Billing ", LMargin + 10, CurY, 0, 0, pFont)
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, " Address ", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, pFont)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, pFont)
            '    End If
            'End If




            If Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString)
                If Len(rmrk) > 40 Then
                    For I = 40 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 40
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "Payments ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Insurance").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Insurance").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label1").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label1").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Warranty").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Warranty").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label2").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label2").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Transportation").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Transportation").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label3").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label3").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Erection_Charges").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Erection_Charges").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label4").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label4").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Installation_Charges").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Installation_Charges").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label5").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label5").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Documents").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Documents").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label6").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label6").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("FrightAndUnloading_Text").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("FrightAndUnloading_Text").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label7").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label7").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If


            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString) <> "" Then


                ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString)
                ItmNm2 = ""
                If Len(ItmNm1) > 50 Then
                    For I = 50 To 1 Step -1
                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 50

                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)

                End If



                If Trim(prn_HdDt.Rows(0).Item("Terms_Label8").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Terms_Label8").ToString), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If ItmNm2 <> "" Then

                    CurY = CurY + TxtHgt

                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + W1 + 15, CurY, 0, 0, p1Font)

                End If
            End If
            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + W1 + 15, CurY, 0, 0, p1Font)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + W1 + 15, CurY, 0, 0, p1Font)
            End If


            If CurY < CurY1 Then
                CurY = CurY1
            End If
            If CurY < 850 Then
                CurY = 850
            End If
            'If CurY < 900 Then
            '    CurY = 900
            'End If

            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                CurY = CurY + 5
                rmrk = Trim(prn_HdDt.Rows(0).Item("Remarks").ToString)
                rmrk1 = ""
                If Len(rmrk) > 100 Then
                    For I = 100 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 100
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "Remarks ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If




            CurY = CurY + TxtHgt '+ 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, CenPs, LnAr(5), CenPs, CurY)

            CurY1 = CurY
            CurY = CurY + TxtHgt - 10
            pFont = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
            p1Font = New Font("Calibri", 8, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "TERMS & CONDITIONS :", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "1. Delivery Should be made strictly in accourdance with our order details.", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "2. Please quote our purchase order No. & Date in your Delivery Notes and Bills.", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "3. If the goods are not delivered as per our order, we will reject the materials.", LMargin + 10, CurY, 0, 0, p1Font)



            CurY = CurY + 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            pFont = New Font("Calibri", 9, FontStyle.Regular)

            If Trim(Common_Procedures.settings.CustomerCode) = "1464" Then

                Common_Procedures.Print_To_PrintDocument(e, "Store Incharge ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Department Incharge", LMargin + ClAr(2) - 180, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "GM", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Managing Director", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)


            Else

                Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + ClAr(2) - 120, CurY, 0, 0, pFont)

            End If


            CurY = CurY + TxtHgt + 10
            If CurY < CurY1 Then
                CurY = CurY1
            End If
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format_3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        '  Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim ItmName(10) As String
        Dim J As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 45
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

        TxtHgt = 18  '19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 6

        If Trim(prn_HdDt.Rows(0).Item("Delivery_Terms").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Documents").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Insurance").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Warranty").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Erection_Charges").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Transportation").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("Installation_Charges").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If
        If Trim(prn_HdDt.Rows(0).Item("FrightAndUnloading_Text").ToString) = "" Then
            NoofItems_PerPage = NoofItems_PerPage + 1
        End If




        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(50)
        ClArr(2) = 280 : ClArr(3) = 60 : ClArr(4) = 120 : ClArr(5) = 50 : ClArr(6) = 80
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        'ClArr(2) = 290 : ClArr(3) = 100 : ClArr(4) = 70 : ClArr(5) = 95
        'ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))
        CurY = TMargin

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format_3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            prn_DetSNo = prn_DetSNo + 1
                            J = 0
                            For k = 0 To 10
                                ItmName(k) = ""
                            Next


                            ItmName(0) = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString) & IIf(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Description").ToString) <> "", " (" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Description").ToString) & ")", "")

                            If Len(ItmName(0)) > 50 Then
Lp:
                                For I = 50 To 1 Step -1
                                    If Mid$(Trim(ItmName(0)), I, 1) = " " Or Mid$(Trim(ItmName(0)), I, 1) = "," Or Mid$(Trim(ItmName(0)), I, 1) = "." Or Mid$(Trim(ItmName(0)), I, 1) = "-" Or Mid$(Trim(ItmName(0)), I, 1) = "/" Or Mid$(Trim(ItmName(0)), I, 1) = "_" Or Mid$(Trim(ItmName(0)), I, 1) = "(" Or Mid$(Trim(ItmName(0)), I, 1) = ")" Or Mid$(Trim(ItmName(0)), I, 1) = "\" Or Mid$(Trim(ItmName(0)), I, 1) = "[" Or Mid$(Trim(ItmName(0)), I, 1) = "]" Or Mid$(Trim(ItmName(0)), I, 1) = "{" Or Mid$(Trim(ItmName(0)), I, 1) = "}" Then Exit For
                                Next I
                                J = J + 1
                                If I = 0 Then I = 50
                                ItmName(J) = Microsoft.VisualBasic.Left(Trim(ItmName(0)), I - 1)
                                ItmName(0) = Microsoft.VisualBasic.Right(Trim(ItmName(0)), Len(ItmName(0)) - I)

                                If Len(ItmName(0)) > 50 Then
                                    GoTo Lp
                                End If
                            Else
                                ItmName(1) = ItmName(0)
                                ItmName(0) = ""
                            End If




                            CurY = CurY + TxtHgt
                            p1Font = New Font("Calibri", 10, FontStyle.Regular)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 25, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(1)), LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("PO_Quantity").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Brand_name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, p1Font)

                            NoofDets = NoofDets + 1

                            If Trim(ItmName(2)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(2)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmName(3)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(3)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmName(4)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(4)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmName(5)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(5)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmName(6)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(6)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmName(7)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(7)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmName(0)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(0)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                        Printing_Format_3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)
                        If Trim(prn_InpOpts) <> "" Then

                            If prn_Count < Len(Trim(prn_InpOpts)) Then


                                If Val(prn_InpOpts) <> "0" Then
                                    prn_DetIndx = 0
                                    prn_DetSNo = 0
                                    prn_PageNo = 0

                                    e.HasMorePages = True
                                    Return
                                End If

                            End If
                        End If

                    End If




                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EmlNo As String
        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0
        Dim S1 As Single = 0
        Dim S As String
        Dim Rfno1 As String = ""
        Dim Rfno2 As String = ""
        Dim I As Integer = 0


        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.brand_name, d.Unit_Name from Stores_Item_PO_Details a INNER JOIN Stores_item_head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Brand_Head c on a.brand_idno = c.brand_idno LEFT OUTER JOIN Unit_Head d on a.unit_idno = d.unit_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PO_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Trim(prn_InpOpts)
                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)


                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If

        End If
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        If Val(dt2.Rows.Count) = 1 Then
            If Trim(prn_OriDupTri) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            End If
        Else
            If Trim(prn_OriDupTri) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 70, CurY - TxtHgt, 1, 0, pFont)
            End If
        End If

        'If Trim(prn_OriDupTri) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 70, CurY - TxtHgt, 1, 0, pFont)
        'End If
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO :" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_CstNo = "GST NO : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EmlNo = "EMAIL : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- PRAKASH Textiles (SOMANUR)

            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Prakash_logo, Drawing.Image), LMargin + 20, CurY, 100, 80)

        End If
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(Cmp_Add1) <> "" Then
            CurY = CurY + strHeight
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        If Trim(Cmp_Add2) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        If Trim(Cmp_Add2) = "" Then
            CurY = CurY + TxtHgt
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EmlNo, LMargin, CurY, 2, PrintWidth, pFont)
        If Trim(Cmp_EmlNo) = "" Then
            'CurY = CurY + TxtHgt
        End If


        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString("PURCHASE ORDER", p1Font).Height

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" OR Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then '---Prakash cottex

            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.prakash_cottex_Logo, Drawing.Image), LMargin + 20, CurY - 90, 112, 80)

        End If

        p1Font = New Font("Calibri", 9, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2)
            W1 = e.Graphics.MeasureString("Your Ref NO: ", pFont).Width
            S1 = e.Graphics.MeasureString("TO  : ", pFont).Width

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Our P.O NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PO_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("PO_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Our Ref NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ref_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            Rfno1 = Trim(prn_HdDt.Rows(0).Item("Ref_No").ToString)
            Rfno2 = ""
            If Len(Rfno1) > 20 Then
                For I = 20 To 1 Step -1
                    If Mid$(Trim(Rfno1), I, 1) = " " Or Mid$(Trim(Rfno1), I, 1) = "," Or Mid$(Trim(Rfno1), I, 1) = "." Or Mid$(Trim(Rfno1), I, 1) = "-" Or Mid$(Trim(Rfno1), I, 1) = "/" Or Mid$(Trim(Rfno1), I, 1) = "_" Or Mid$(Trim(Rfno1), I, 1) = "(" Or Mid$(Trim(Rfno1), I, 1) = ")" Or Mid$(Trim(Rfno1), I, 1) = "\" Or Mid$(Trim(Rfno1), I, 1) = "[" Or Mid$(Trim(Rfno1), I, 1) = "]" Or Mid$(Trim(Rfno1), I, 1) = "{" Or Mid$(Trim(Rfno1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 20
                Rfno2 = Microsoft.VisualBasic.Right(Trim(Rfno1), Len(Rfno1) - I)
                Rfno1 = Microsoft.VisualBasic.Left(Trim(Rfno1), I)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Trim(Rfno1), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)


            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "GST NO :" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            End If

            If Trim(Rfno2) <> "" Then
                '   CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, Trim(Rfno2), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            'CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Ref_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurY = CurY + 20
            If Trim(prn_HdDt.Rows(0).Item("Owner_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Kind Attention : " & prn_HdDt.Rows(0).Item("Owner_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            End If


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))

            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "We do hereby confirm the following goods subject to our general terms and conditions ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "We are pleased to place our order for the following items:", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT PRICE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT (in Rs)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format_3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim CenPs As Single = 0
        Dim W1 As Single = 0
        Dim W2(7) As Single
        Dim S1 As Single = 0
        Dim CurY1 As Single = 0
        Dim rmrk As String = ""
        Dim rmrk1 As String = ""

        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        da1 = New SqlClient.SqlDataAdapter("select a.* from Item_PO_GST_Tax_Details a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        prn_TaxDt = New DataTable
        da1.Fill(prn_TaxDt)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

            ' CenPs = ClAr(1) + ClAr(2) - 50  ' CInt(PageWidth \ 2)

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            S1 = e.Graphics.MeasureString("Packing Charges & Forwarding charges :", pFont).Width

            CurY1 = CurY - 10

            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If

            'If Val(prn_HdDt.Rows(0).Item("Vat_Amount1").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Vat " & Val(prn_HdDt.Rows(0).Item("Vat_Percentage1").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Vat_Amount1").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Vat_Amount2").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Vat " & Val(prn_HdDt.Rows(0).Item("Vat_percentage2").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Vat_Amount2").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If

            'If Val(prn_HdDt.Rows(0).Item("Cst_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "CST " & Val(prn_HdDt.Rows(0).Item("Cst_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Cst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If

            If Val(prn_HdDt.Rows(0).Item("Discount_Amount_Total").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Discount " & IIf(Val(prn_HdDt.Rows(0).Item("Disc_Percentage").ToString) <> 0, Val(prn_HdDt.Rows(0).Item("Disc_Percentage").ToString) & "%", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount_Total").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If
            'If Val(prn_HdDt.Rows(0).Item("Excise_Duties").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Excise Duty " & Val(prn_HdDt.Rows(0).Item("Excise_Duties_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Excise_Duties").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Surcharges_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "SurCharges " & Val(prn_HdDt.Rows(0).Item("SurCharges_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Surcharges_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Cess_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Cess " & Val(prn_HdDt.Rows(0).Item("Cess_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Cess_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("EdCess_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Ed.Cess " & Val(prn_HdDt.Rows(0).Item("EdCess_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("EdCess_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            If Val(prn_HdDt.Rows(0).Item("Packing_Charges").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Label_Caption1").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Packing_Charges").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Forwarding_Charges").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Label_Caption2").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Forwarding_Charges").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Taxable_Value").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Taxable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "CGST " & IIf(Val(prn_TaxDt.Rows(0).Item("CGST_Percentage").ToString) <> 0, Val(prn_TaxDt.Rows(0).Item("CGST_Percentage").ToString) & "%", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "SGST " & IIf(Val(prn_TaxDt.Rows(0).Item("SGST_Percentage").ToString) <> 0, Val(prn_TaxDt.Rows(0).Item("SGST_Percentage").ToString) & "%", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "IGST " & IIf(Val(prn_TaxDt.Rows(0).Item("IGST_Percentage").ToString) <> 0, Val(prn_TaxDt.Rows(0).Item("IGST_Percentage").ToString) & "%", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - S1, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If

            CurY1 = CurY1 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY1, PageWidth, CurY1)
            LnAr(6) = CurY1
            CurY1 = CurY1 + TxtHgt - 10
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)




            Common_Procedures.Print_To_PrintDocument(e, "Net Amount ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - S1, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, p1Font)

            'CurY1 = CurY1 + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 50, CurY1, PageWidth, CurY1)
            'LnAr(7) = CurY1

            CurY1 = CurY1 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY1, PageWidth, CurY1)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(6))


            pFont = New Font("Calibri", 9, FontStyle.Bold)
            p1Font = New Font("Calibri", 9, FontStyle.Regular)


            W1 = e.Graphics.MeasureString("Billing & Delivery Address :", pFont).Width
            For I = 0 To 7
                W2(I) = e.Graphics.MeasureString(Trim(prn_HdDt.Rows(0).Item("Terms_Label" & I + 1).ToString), pFont).Width
            Next
            If W1 > W2.Max Then
                W1 = W2.Max
            End If

            If Trim(prn_HdDt.Rows(0).Item("Delivery_Terms").ToString) <> "" Then
                CurY = CurY + TxtHgt - 10

                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Delivery_Terms").ToString)
                If Len(rmrk) > 40 Then
                    For I = 40 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 40
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "Expected Delivery Date", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 30, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 35, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If


            'If Trim(prn_HdDt.Rows(0).Item("Despatch").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    rmrk1 = ""
            '    rmrk = ""
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("Despatch").ToString)
            '    If Len(rmrk) > 35 Then
            '        For I = 35 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 35
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, " Delivery & Billing ", LMargin + 10, CurY, 0, 0, pFont)
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, " Address ", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, pFont)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, pFont)
            '    End If
            'End If




            If Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString)
                If Len(rmrk) > 40 Then
                    For I = 40 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 40
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "Payments ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 30, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 35, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            'CurY = CurY + TxtHgt
            'If Trim(prn_HdDt.Rows(0).Item("Delivery_Terms").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Expected Delivery Date : " & prn_HdDt.Rows(0).Item("Delivery_Terms").ToString, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            'End If


            If Trim(prn_HdDt.Rows(0).Item("Insurance").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Insurance").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label1").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label1").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Warranty").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Warranty").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label2").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label2").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Transportation").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Transportation").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label3").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label3").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Erection_Charges").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Erection_Charges").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label4").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label4").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Installation_Charges").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Installation_Charges").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label5").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label5").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Documents").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("Documents").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label6").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label6").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("FrightAndUnloading_Text").ToString) <> "" Then
                CurY = CurY + TxtHgt
                rmrk1 = ""
                rmrk = ""
                rmrk = Trim(prn_HdDt.Rows(0).Item("FrightAndUnloading_Text").ToString)
                If Len(rmrk) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label7").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label7").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If


            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString) <> "" Then


                ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString)
                ItmNm2 = ""
                If Len(ItmNm1) > 50 Then
                    For I = 50 To 1 Step -1
                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 50

                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)

                End If



                If Trim(prn_HdDt.Rows(0).Item("Terms_Label8").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Terms_Label8").ToString), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                End If
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If ItmNm2 <> "" Then

                    CurY = CurY + TxtHgt

                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + W1 + 15, CurY, 0, 0, p1Font)

                End If
            End If
            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + W1 + 15, CurY, 0, 0, p1Font)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + W1 + 15, CurY, 0, 0, p1Font)
            End If


            If CurY < CurY1 Then
                CurY = CurY1
            End If
            If CurY < 900 Then
                CurY = 900
            End If

            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                CurY = CurY + 5
                rmrk = Trim(prn_HdDt.Rows(0).Item("Remarks").ToString)
                rmrk1 = ""
                If Len(rmrk) > 100 Then
                    For I = 100 To 1 Step -1
                        If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 100
                    rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
                    rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "Remarks ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                If rmrk1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
                End If
            End If




            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, CenPs, LnAr(5), CenPs, CurY)

            CurY1 = CurY
            CurY = CurY + TxtHgt + 10
            pFont = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
            p1Font = New Font("Calibri", 8, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "TERMS & CONDITIONS :", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "1. Delivery Should be made strictly in accordance with our order details.", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "2. Please quote our purchase order No. & Date in your Delivery Notes and Bills.", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "3. If the goods are not delivered as per our order, we will reject the materials.", LMargin + 10, CurY, 0, 0, p1Font)



            CurY = CurY + 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            pFont = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Approved By ", LMargin + ClAr(2) - 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + ClAr(2) + ClAr(3), CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            If CurY < CurY1 Then
                CurY = CurY1
            End If
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_DeliveryTerms_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DeliveryTerms.KeyDown
        If e.KeyValue = 38 Then
            ' cbo_Despatch.Focus()
            txt_RefNo.Focus()
        End If

        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, txt_DeliveryTerms, cbo_PaymentTerms, "", "", "", "")
        'If (e.KeyValue = 40 And cbo_TaxType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        '    dgv_Details.Focus()
        '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        'End If
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, cbo_PaymentTerms, "", "", "", "")
        'If Asc(e.KeyChar) = 13 Then
        '    btn_Terms.Focus()
        '    'dgv_Details.Focus()
        '    'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        'End If
    End Sub



    Private Sub txt_DiscPercentage_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscPercentage.KeyDown
        If (e.KeyValue = 38) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            End If
        Else
            txt_PackingCharges.Focus()
        End If
        'If (e.KeyValue = 40) Or (e.Control = True And e.KeyValue = 40) Then
        '    If txt_DiscPercentage.Text = 0 Then
        '        ' lbl_DiscAmount_Total.Focus()
        '        txt_PackingCharges.Focus()
        '    Else
        '        ' txt_DutiesPercentage.Focus()
        '    End If
        'End If
    End Sub

    Private Sub txt_DiscPercentage_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPercentage.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_PackingCharges.Focus()
        End If


    End Sub

    Private Sub txt_DiscPercentage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_DiscPercentage.TextChanged
        Tax_Calculation()
    End Sub

    Private Sub NetAmount_Calculation()
        Dim netAmt As String = 0

        netAmt = Format(Val(txt_AssessableValue.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text), "##########0.00")

        lbl_RoundOff.Text = Format(Format(Val(netAmt), "#########0") - Val(netAmt), "#########0.00")

        lbl_NetAmount.Text = Common_Procedures.Currency_Format(netAmt - -Val(lbl_RoundOff.Text))


    End Sub

    Private Sub Tax_Calculation()
        Dim cst As Single = 0
        Dim dsc As Single = 0
        Dim dty As Single = 0
        Dim sur As Single = 0
        Dim Cess As Single = 0
        Dim edCess As Single = 0


        ' lbl_CstAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_CSTPercentage.Text) / 100, "########0.00")

        If Val(txt_DiscPercentage.Text) <> 0 Then
            lbl_DiscAmount_Total.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_DiscPercentage.Text) / 100, "#######0.00")
        Else
            lbl_DiscAmount_Total.Text = 0
           
        End If


        GST_Calculation()
        NetAmount_Calculation()

    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 38 Then
            e.Handled = True
            txt_Forwarding.Focus()
        End If
        If e.KeyValue = 40 Then
            e.Handled = True
            btn_Save.Focus()
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_date.Focus()
            End If
        End If
    End Sub

    'Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
    '    If Trim(UCase(cbo_TaxType.Text)) = "NO TAX" Then
    '        lbl_vatPercentage1.Text = ""
    '        lbl_VatPercentage2.Text = ""
    '        lbl_VatAmount2.Text = ""
    '        lbl_VatAmount1.Text = ""
    '        txt_CSTPercentage.Text = ""
    '        txt_CSTPercentage.Enabled = False
    '        lbl_CstAmount.Text = ""
    '        Calculation_Grid_Total()
    '    ElseIf Trim(UCase(cbo_TaxType.Text)) = "VAT" Then
    '        txt_CSTPercentage.Text = ""
    '        txt_CSTPercentage.Enabled = False
    '        lbl_CstAmount.Text = ""
    '        Calculation_Grid_Total()
    '    ElseIf Trim(UCase(cbo_TaxType.Text)) = "CST" Then
    '        lbl_vatPercentage1.Text = ""
    '        lbl_VatPercentage2.Text = ""
    '        lbl_VatAmount2.Text = ""
    '        lbl_VatAmount1.Text = ""
    '        txt_CSTPercentage.Enabled = True
    '        Calculation_Grid_Total()
    '    End If
    'End Sub

    Private Sub txt_PackingCharges_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_PackingCharges.TextChanged
        GST_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_DiscAmount_Total_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_DiscAmount_Total.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Documents_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Insurance.KeyDown

    End Sub

    Private Sub txt_Documents_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Insurance.KeyPress

    End Sub

    Private Sub txt_Packinglabel_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Packinglabel.LostFocus
        txt_Packinglabel.BackColor = Color.LightSkyBlue
    End Sub

    Private Sub txt_ForwardingLabel_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ForwardingLabel.LostFocus
        txt_ForwardingLabel.BackColor = Color.LightSkyBlue
    End Sub

    Private Sub btn_Terms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Terms.Click
        pnl_Terms.Visible = True
        pnl_Terms.BringToFront()
        pnl_Back.Enabled = False
        txt_Insurance.Focus()
    End Sub

    Private Sub btn_CloseTerms_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseTerms.Click


        pnl_Back.Enabled = True
        pnl_Terms.Visible = False
        dgv_Details.Focus()
        dgv_Details.CurrentCell.Selected = True


    End Sub

    Private Sub txt_DeliveryAddress3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DeliveryAddress3.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            pnl_Back.Enabled = True
            pnl_Terms.Visible = False
            dgv_Details.Focus()
            dgv_Details.CurrentCell.Selected = True

        End If
    End Sub

    Private Sub txt_DeliveryAddress3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DeliveryAddress3.KeyPress

        If Asc(e.KeyChar) = 13 Then

            pnl_Back.Enabled = True
            pnl_Terms.Visible = False
            dgv_Details.Focus()
            dgv_Details.CurrentCell.Selected = True

        End If
    End Sub

  
    Private Sub txt_labelCaption1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_labelCaption1.LostFocus
        txt_labelCaption1.BackColor = Color.White
    End Sub

    Private Sub txt_labelCaption2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_labelCaption2.LostFocus
        txt_labelCaption2.BackColor = Color.White
    End Sub

    Private Sub txt_LabelCaption3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_LabelCaption3.LostFocus
        txt_LabelCaption3.BackColor = Color.White
    End Sub

    Private Sub txt_LabelCaption4_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_LabelCaption4.LostFocus
        txt_LabelCaption4.BackColor = Color.White
    End Sub

    Private Sub txt_LabelCaption5_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_LabelCaption5.LostFocus
        txt_LabelCaption5.BackColor = Color.White
    End Sub

    Private Sub txt_LabelCaption6_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_LabelCaption6.LostFocus
        txt_LabelCaption6.BackColor = Color.White
    End Sub

    Private Sub txt_LabelCaption7_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_LabelCaption7.LostFocus
        txt_LabelCaption7.BackColor = Color.White
    End Sub

    Private Sub btn_PDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
        'Print_PDF_Status = False
    End Sub

    Private Sub btn_Tax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Tax.Click
        pnl_Back.Enabled = False
        pnl_Tax.Visible = True
        pnl_Tax.Focus()
    End Sub

    Private Sub btn_Tax_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Tax_Close.Click
        pnl_Tax.Visible = False
        pnl_Back.Enabled = True
    End Sub




    Private Sub Get_GST_Percentage_From_ItemGroup(ByVal ItemName As String, ByRef HSN_Code As String, ByRef GST_PerCent As Single)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            HSN_Code = ""
            GST_PerCent = 0

            da = New SqlClient.SqlDataAdapter("select a.* from ItemGroup_Head a INNER JOIN Stores_Item_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where b.Item_Name ='" & Trim(ItemName) & "'", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Item_HSN_Code").ToString) = False Then
                    HSN_Code = Trim(dt.Rows(0).Item("Item_HSN_Code").ToString)
                End If
                If IsDBNull(dt.Rows(0).Item("Item_GST_Percentage").ToString) = False Then
                    GST_PerCent = Val(dt.Rows(0).Item("Item_GST_Percentage").ToString)
                End If

            End If

            dt.Clear()


        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub Get_HSN_CodeWise_Tax_Details()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        Dim Led_IdNo As Integer = 0
        Dim AssVal_Pack_Frwt_Dic_Amt As String = ""
        Dim InterStateStatus As Boolean = False

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            If cbo_TaxType.Text = "GST" Then

                AssVal_Pack_Frwt_Dic_Amt = Format(Val(txt_PackingCharges.Text) + Val(txt_Forwarding.Text) - Val(lbl_DiscAmount_Total.Text), "#########0.00")

                With dgv_Details

                    If .Rows.Count > 0 Then

                        For i = 0 To .Rows.Count - 1

                            If Trim(.Rows(i).Cells(1).Value) <> "" Then

                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1                ,                  Currency1            ,                       Currency2            ) " & _
                                                    "          Values     ( '" & Trim(.Rows(i).Cells(18).Value) & "', " & Val(.Rows(i).Cells(19).Value) & " ,  " & Str(Val(.Rows(i).Cells(17).Value)) + Val(AssVal_Pack_Frwt_Dic_Amt) & " ) "
                                cmd.ExecuteNonQuery()

                                AssVal_Pack_Frwt_Dic_Amt = 0

                            End If
                        Next

                    End If

                End With

            End If


            With dgv_Tax_Details

                .Rows.Clear()
                Sno = 0

                da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as TaxableAmount from " & Trim(Common_Procedures.EntryTempTable) & " group by name1, Currency1 Having sum(Currency2) <> 0 order by Name1, Currency1", con)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then

                    Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                    InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), Led_IdNo)

                    For i = 0 To dt.Rows.Count - 1

                        n = .Rows.Add()

                        Sno = Sno + 1

                        .Rows(n).Cells(0).Value = Sno
                        .Rows(n).Cells(1).Value = dt.Rows(i).Item("HSN_Code").ToString

                        .Rows(n).Cells(2).Value = Format(Val(dt.Rows(i).Item("TaxableAmount").ToString), "##########0.00")
                        If Val(.Rows(n).Cells(2).Value) = 0 Then .Rows(n).Cells(2).Value = ""

                        If InterStateStatus = True Then

                            .Rows(n).Cells(7).Value = Format(Val(dt.Rows(i).Item("GST_Percentage").ToString), "#############0.00")
                            If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""

                        Else

                            .Rows(n).Cells(3).Value = Format(Val(dt.Rows(i).Item("GST_Percentage").ToString) / 2, "############0.00")
                            If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(3).Value = ""

                            .Rows(n).Cells(5).Value = Format(Val(dt.Rows(i).Item("GST_Percentage").ToString) / 2, "############0.00")
                            If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""

                        End If

                        .Rows(n).Cells(4).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(3).Value) / 100, "#############0.00")
                        If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                        .Rows(n).Cells(6).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(5).Value) / 100, "#############0.00")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                        .Rows(n).Cells(8).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(7).Value) / 100, "#############0.00")
                        If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                    Next

                End If
                dt.Clear()

                dt.Dispose()
                da.Dispose()

            End With

            Total_Tax_Calculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub Total_Tax_Calculation()
        Dim Sno As Integer
        Dim TotAss_Val As Single
        Dim TotCGST_amt As Double
        Dim TotSGST_amt As Double
        Dim TotIGST_amt As Double

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotAss_Val = 0 : TotCGST_amt = 0 : TotSGST_amt = 0 : TotIGST_amt = 0

        With dgv_Tax_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    TotAss_Val = TotAss_Val + Val(.Rows(i).Cells(2).Value())
                    TotCGST_amt = TotCGST_amt + Val(.Rows(i).Cells(4).Value())
                    TotSGST_amt = TotSGST_amt + Val(.Rows(i).Cells(6).Value())
                    TotIGST_amt = TotIGST_amt + Val(.Rows(i).Cells(8).Value())


                End If

            Next i

        End With



        With dgv_Tax_Total_Details
            If .RowCount = 0 Then .Rows.Add()

            .Rows(0).Cells(2).Value = Format(Val(TotAss_Val), "##########0.00")
            .Rows(0).Cells(4).Value = Format(Val(TotCGST_amt), "##########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotSGST_amt), "##########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotIGST_amt), "##########0.00")

        End With



        If cbo_TaxType.Text = "GST" Then
            txt_AssessableValue.Text = Format(Val(TotAss_Val), "##########0.00")
        Else
            txt_AssessableValue.Text = Format(Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount_Total.Text) + Val(txt_PackingCharges.Text) + Val(txt_Forwarding.Text), "##########0.00")
        End If
        'txt_AssessableValue.Text = Format(Val(TotAss_Val) - Val(lbl_DiscAmount_Total.Text) + Val(txt_PackingCharges.Text) + Val(txt_Forwarding.Text), "##########0.00")
        lbl_CGST_Amount.Text = IIf(Val(TotCGST_amt) <> 0, Format(Val(TotCGST_amt), "##########0.00"), "")
        lbl_SGST_Amount.Text = IIf(Val(TotSGST_amt) <> 0, Format(Val(TotSGST_amt), "##########0.00"), "")
        lbl_IGST_Amount.Text = IIf(Val(TotIGST_amt) <> 0, Format(Val(TotIGST_amt), "##########0.00"), "")

    End Sub

    Private Sub GST_Calculation()
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim CGST_Per As Single = 0, SGST_Per As Single = 0, IGST_Per As Single = 0, GST_Per As Single = 0
        Dim HSN_Code As String = ""
        Dim Taxable_Amount As Double = 0
        Dim Led_IdNo As Integer = 0

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            With dgv_Details

                If dgv_Details.Rows.Count > 0 Then

                    For RowIndx = 0 To dgv_Details.Rows.Count - 1


                        '.Rows(RowIndx).Cells(17).Value = ""   'TAXABLE VALUE
                        .Rows(RowIndx).Cells(18).Value = ""   'HSN CODE
                        .Rows(RowIndx).Cells(19).Value = ""   'GST %

                        If Trim(.Rows(RowIndx).Cells(3).Value) <> "" Or Val(.Rows(RowIndx).Cells(8).Value) <> 0 Then

                            HSN_Code = ""
                            GST_Per = 0
                            Get_GST_Percentage_From_ItemGroup(Trim(.Rows(RowIndx).Cells(3).Value), HSN_Code, GST_Per)


                            '-- Taxable value 
                            'Taxable_Amount = 0
                            'Taxable_Amount = Taxable_Amount + Val(.Rows(RowIndx).Cells(8).Value)


                            '.Rows(RowIndx).Cells(17).Value = Format(Val(Taxable_Amount), "##########0.00")
                            .Rows(RowIndx).Cells(18).Value = Trim(HSN_Code)
                            .Rows(RowIndx).Cells(19).Value = Format(Val(GST_Per), "########0.00")

                        End If

                    Next

                    Get_HSN_CodeWise_Tax_Details()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DO GST CALCULATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Forwarding_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Forwarding.TextChanged
        GST_Calculation()
        NetAmount_Calculation()
    End Sub
    Private Sub cbo_TaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.SelectedIndexChanged
        GST_Calculation()

        NetAmount_Calculation()
    End Sub

    'Private Sub cbo_Despatch_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Despatch.GotFocus
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stores_Item_PO_Head", "Despatch", "", "")
    'End Sub

    'Private Sub cbo_Despatch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Despatch.KeyDown

    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Despatch, dtp_RefDate, txt_DeliveryTerms, "Stores_Item_PO_Head", "Despatch", "", "")

    'End Sub

    'Private Sub cbo_Despatch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Despatch.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Despatch, txt_DeliveryTerms, "Stores_Item_PO_Head", "Despatch", "", "", False)
    'End Sub

    Private Sub cbo_PaymentTerms_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentTerms.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stores_Item_PO_Head", "Payment_Terms", "", "")
    End Sub

    Private Sub cbo_PaymentTerms_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentTerms.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentTerms, cbo_TaxType, txt_DelvAdd1, "Stores_Item_PO_Head", "Payment_terms", "", "")
    End Sub


    Private Sub cbo_PaymentTerms_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentTerms.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentTerms, txt_DelvAdd1, "Stores_Item_PO_Head", "Payment_terms", "", "", False)
    End Sub

    Private Sub txt_DelvAdd3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DelvAdd3.KeyDown
        If (e.KeyValue = 38) Then
            txt_DelvAdd2.Focus()
        End If

        If (e.KeyValue = 40) Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If

    End Sub

    Private Sub txt_DelvAdd3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DelvAdd3.KeyPress
        If Asc(e.KeyChar) = 13 Then
            ' btn_Terms.Focus()
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub btn_terms_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_terms_Close.Click
        pnl_Terms.Visible = False
        pnl_Back.Enabled = True
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_Grid_Item_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Item.SelectedIndexChanged

    End Sub


    Private Sub Printing_Format_1391(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        '  Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim ItmName(10) As String
        Dim J As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 45
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

        TxtHgt = 18  '19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 28

        'If Trim(prn_HdDt.Rows(0).Item("Delivery_Terms").ToString) = "" Then
        '    NoofItems_PerPage = NoofItems_PerPage + 1
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) = "" Then
        '    NoofItems_PerPage = NoofItems_PerPage + 1
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Documents").ToString) = "" Then
        '    NoofItems_PerPage = NoofItems_PerPage + 1
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Insurance").ToString) = "" Then
        '    NoofItems_PerPage = NoofItems_PerPage + 1
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Warranty").ToString) = "" Then
        '    NoofItems_PerPage = NoofItems_PerPage + 1
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Erection_Charges").ToString) = "" Then
        '    NoofItems_PerPage = NoofItems_PerPage + 1
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Transportation").ToString) = "" Then
        '    NoofItems_PerPage = NoofItems_PerPage + 1
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Installation_Charges").ToString) = "" Then
        '    NoofItems_PerPage = NoofItems_PerPage + 1
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("FrightAndUnloading_Text").ToString) = "" Then
        '    NoofItems_PerPage = NoofItems_PerPage + 1
        'End If




        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        'ClArr(0) = 0
        'ClArr(1) = Val(50)
        'ClArr(2) = 410 : ClArr(3) = 60 : ClArr(4) = 50 : ClArr(5) = 80
        'ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))


        ClArr(0) = 0
        ClArr(1) = Val(50)
        ClArr(2) = 430 : ClArr(3) = 100
        ClArr(4) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3))



        'ClArr(2) = 290 : ClArr(3) = 100 : ClArr(4) = 70 : ClArr(5) = 95
        'ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))
        CurY = TMargin

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_1391_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format_1391_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            prn_DetSNo = prn_DetSNo + 1
                            J = 0
                            For k = 0 To 10
                                ItmName(k) = ""
                            Next


                            ItmName(0) = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString) & IIf(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Description").ToString) <> "", " (" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Description").ToString) & ")", "")

                            If Len(ItmName(0)) > 50 Then
Lp:
                                For I = 50 To 1 Step -1
                                    If Mid$(Trim(ItmName(0)), I, 1) = " " Or Mid$(Trim(ItmName(0)), I, 1) = "," Or Mid$(Trim(ItmName(0)), I, 1) = "." Or Mid$(Trim(ItmName(0)), I, 1) = "-" Or Mid$(Trim(ItmName(0)), I, 1) = "/" Or Mid$(Trim(ItmName(0)), I, 1) = "_" Or Mid$(Trim(ItmName(0)), I, 1) = "(" Or Mid$(Trim(ItmName(0)), I, 1) = ")" Or Mid$(Trim(ItmName(0)), I, 1) = "\" Or Mid$(Trim(ItmName(0)), I, 1) = "[" Or Mid$(Trim(ItmName(0)), I, 1) = "]" Or Mid$(Trim(ItmName(0)), I, 1) = "{" Or Mid$(Trim(ItmName(0)), I, 1) = "}" Then Exit For
                                Next I
                                J = J + 1
                                If I = 0 Then I = 50
                                ItmName(J) = Microsoft.VisualBasic.Left(Trim(ItmName(0)), I - 1)
                                ItmName(0) = Microsoft.VisualBasic.Right(Trim(ItmName(0)), Len(ItmName(0)) - I)

                                If Len(ItmName(0)) > 50 Then
                                    GoTo Lp
                                End If
                            Else
                                ItmName(1) = ItmName(0)
                                ItmName(0) = ""
                            End If




                            CurY = CurY + TxtHgt
                            p1Font = New Font("Calibri", 10, FontStyle.Regular)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 25, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(1)), LMargin + ClArr(1), CurY, 2, ClArr(2), p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("PO_Quantity").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), p1Font)
                            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, p1Font)
                            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, p1Font)

                            NoofDets = NoofDets + 1

                            If Trim(ItmName(2)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(2)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmName(3)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(3)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmName(4)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(4)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmName(5)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(5)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmName(6)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(6)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmName(7)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(7)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmName(0)) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmName(0)), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                        Printing_Format_1391_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)
                        If Trim(prn_InpOpts) <> "" Then

                            If prn_Count < Len(Trim(prn_InpOpts)) Then


                                If Val(prn_InpOpts) <> "0" Then
                                    prn_DetIndx = 0
                                    prn_DetSNo = 0
                                    prn_PageNo = 0

                                    e.HasMorePages = True
                                    Return
                                End If

                            End If
                        End If

                    End If




                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_1391_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EmlNo As String
        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0
        Dim S1 As Single = 0
        Dim S As String
        Dim Rfno1 As String = ""
        Dim Rfno2 As String = ""
        Dim I As Integer = 0


        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.brand_name, d.Unit_Name from Stores_Item_PO_Details a INNER JOIN Stores_item_head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Brand_Head c on a.brand_idno = c.brand_idno LEFT OUTER JOIN Unit_Head d on a.unit_idno = d.unit_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PO_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Trim(prn_InpOpts)
                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)


                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If

        End If
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        If Val(dt2.Rows.Count) = 1 Then
            If Trim(prn_OriDupTri) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            End If
        Else
            If Trim(prn_OriDupTri) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 70, CurY - TxtHgt, 1, 0, pFont)
            End If
        End If

        'If Trim(prn_OriDupTri) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 70, CurY - TxtHgt, 1, 0, pFont)
        'End If
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO :" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_CstNo = "GST NO : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EmlNo = "EMAIL : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- PRAKASH Textiles (SOMANUR)

            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Prakash_logo, Drawing.Image), LMargin + 20, CurY, 100, 80)

        End If
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(Cmp_Add1) <> "" Then
            CurY = CurY + strHeight
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        If Trim(Cmp_Add2) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        If Trim(Cmp_Add2) = "" Then
            CurY = CurY + TxtHgt
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EmlNo, LMargin, CurY, 2, PrintWidth, pFont)
        If Trim(Cmp_EmlNo) = "" Then
            'CurY = CurY + TxtHgt
        End If

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString("PURCHASE ORDER", p1Font).Height

        p1Font = New Font("Calibri", 9, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2)
            W1 = e.Graphics.MeasureString("Your Ref NO: ", pFont).Width
            S1 = e.Graphics.MeasureString("TO  : ", pFont).Width

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Our P.O NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PO_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("PO_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "Your Ref NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ref_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            Rfno1 = Trim(prn_HdDt.Rows(0).Item("Ref_No").ToString)
            Rfno2 = ""
            If Len(Rfno1) > 20 Then
                For I = 20 To 1 Step -1
                    If Mid$(Trim(Rfno1), I, 1) = " " Or Mid$(Trim(Rfno1), I, 1) = "," Or Mid$(Trim(Rfno1), I, 1) = "." Or Mid$(Trim(Rfno1), I, 1) = "-" Or Mid$(Trim(Rfno1), I, 1) = "/" Or Mid$(Trim(Rfno1), I, 1) = "_" Or Mid$(Trim(Rfno1), I, 1) = "(" Or Mid$(Trim(Rfno1), I, 1) = ")" Or Mid$(Trim(Rfno1), I, 1) = "\" Or Mid$(Trim(Rfno1), I, 1) = "[" Or Mid$(Trim(Rfno1), I, 1) = "]" Or Mid$(Trim(Rfno1), I, 1) = "{" Or Mid$(Trim(Rfno1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 20
                Rfno2 = Microsoft.VisualBasic.Right(Trim(Rfno1), Len(Rfno1) - I)
                Rfno1 = Microsoft.VisualBasic.Left(Trim(Rfno1), I)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Trim(Rfno1), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)


            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "GST NO :" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            End If

            If Trim(Rfno2) <> "" Then
                '   CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, Trim(Rfno2), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Ref_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurY = CurY + 20
            If Trim(prn_HdDt.Rows(0).Item("Owner_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Kind Attention : " & prn_HdDt.Rows(0).Item("Owner_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            End If


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))

            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "We do hereby confirm the following goods subject to our general terms and conditions ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "We are pleased to place our order for the following items:", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "UNIT PRICE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "AMOUNT (in Rs)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format_1391_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim CenPs As Single = 0
        Dim W1 As Single = 0
        Dim W2(7) As Single
        Dim S1 As Single = 0
        Dim CurY1 As Single = 0
        Dim rmrk As String = ""
        Dim rmrk1 As String = ""

        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PoNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        da1 = New SqlClient.SqlDataAdapter("select a.* from Item_PO_GST_Tax_Details a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        prn_TaxDt = New DataTable
        da1.Fill(prn_TaxDt)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))

            ' CenPs = ClAr(1) + ClAr(2) - 50  ' CInt(PageWidth \ 2)

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            S1 = e.Graphics.MeasureString("Packing Charges & Forwarding charges :", pFont).Width

            CurY1 = CurY - 10

            'If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If

            'If Val(prn_HdDt.Rows(0).Item("Vat_Amount1").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Vat " & Val(prn_HdDt.Rows(0).Item("Vat_Percentage1").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Vat_Amount1").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Vat_Amount2").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Vat " & Val(prn_HdDt.Rows(0).Item("Vat_percentage2").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Vat_Amount2").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If

            'If Val(prn_HdDt.Rows(0).Item("Cst_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "CST " & Val(prn_HdDt.Rows(0).Item("Cst_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Cst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If

            'If Val(prn_HdDt.Rows(0).Item("Discount_Amount_Total").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Discount " & IIf(Val(prn_HdDt.Rows(0).Item("Disc_Percentage").ToString) <> 0, Val(prn_HdDt.Rows(0).Item("Disc_Percentage").ToString) & "%", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount_Total").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Excise_Duties").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Excise Duty " & Val(prn_HdDt.Rows(0).Item("Excise_Duties_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Excise_Duties").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Surcharges_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "SurCharges " & Val(prn_HdDt.Rows(0).Item("SurCharges_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Surcharges_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Cess_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Cess " & Val(prn_HdDt.Rows(0).Item("Cess_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Cess_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("EdCess_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Ed.Cess " & Val(prn_HdDt.Rows(0).Item("EdCess_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("EdCess_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Packing_Charges").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Label_Caption1").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Packing_Charges").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If

            'If Val(prn_HdDt.Rows(0).Item("Forwarding_Charges").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Label_Caption2").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Forwarding_Charges").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Taxable_Value").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Taxable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "CGST " & IIf(Val(prn_TaxDt.Rows(0).Item("CGST_Percentage").ToString) <> 0, Val(prn_TaxDt.Rows(0).Item("CGST_Percentage").ToString) & "%", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "SGST " & IIf(Val(prn_TaxDt.Rows(0).Item("SGST_Percentage").ToString) <> 0, Val(prn_TaxDt.Rows(0).Item("SGST_Percentage").ToString) & "%", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "IGST " & IIf(Val(prn_TaxDt.Rows(0).Item("IGST_Percentage").ToString) <> 0, Val(prn_TaxDt.Rows(0).Item("IGST_Percentage").ToString) & "%", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If
            'If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, pFont)
            'End If

            'CurY1 = CurY1 + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 50, CurY1, PageWidth, CurY1)
            LnAr(6) = CurY1
            'CurY1 = CurY1 + TxtHgt - 10
            'pFont = New Font("Calibri", 11, FontStyle.Regular)
            'p1Font = New Font("Calibri", 12, FontStyle.Bold)

            'Common_Procedures.Print_To_PrintDocument(e, "Net Amount ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - S1, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY1, 1, 0, p1Font)
            'CurY1 = CurY1 + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 50, CurY1, PageWidth, CurY1)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 50, CurY1, LMargin + ClAr(1) + ClAr(2) + 50, LnAr(6))



            pFont = New Font("Calibri", 9, FontStyle.Bold)
            p1Font = New Font("Calibri", 9, FontStyle.Regular)


            'W1 = e.Graphics.MeasureString("Billing & Delivery Address :", pFont).Width
            'For I = 0 To 7
            '    W2(I) = e.Graphics.MeasureString(Trim(prn_HdDt.Rows(0).Item("Terms_Label" & I + 1).ToString), pFont).Width
            'Next
            'If W1 > W2.Max Then
            '    W1 = W2.Max
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Delivery_Terms").ToString) <> "" Then
            '    CurY = CurY + TxtHgt - 10

            '    rmrk1 = ""
            '    rmrk = ""
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("Delivery_Terms").ToString)
            '    If Len(rmrk) > 40 Then
            '        For I = 40 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 40
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, "Delivery Terms ", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    End If
            'End If


            'If Trim(prn_HdDt.Rows(0).Item("Despatch").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    rmrk1 = ""
            '    rmrk = ""
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("Despatch").ToString)
            '    If Len(rmrk) > 35 Then
            '        For I = 35 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 35
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, " Delivery & Billing ", LMargin + 10, CurY, 0, 0, pFont)
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, " Address ", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, pFont)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, pFont)
            '    End If
            'End If




            'If Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    rmrk1 = ""
            '    rmrk = ""
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString)
            '    If Len(rmrk) > 40 Then
            '        For I = 40 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 40
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, "Payments ", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    End If
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Insurance").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    rmrk1 = ""
            '    rmrk = ""
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("Insurance").ToString)
            '    If Len(rmrk) > 70 Then
            '        For I = 70 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 70
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    If Trim(prn_HdDt.Rows(0).Item("Terms_Label1").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    End If
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Warranty").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    rmrk1 = ""
            '    rmrk = ""
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("Warranty").ToString)
            '    If Len(rmrk) > 70 Then
            '        For I = 70 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 70
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    If Trim(prn_HdDt.Rows(0).Item("Terms_Label2").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    End If
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Transportation").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    rmrk1 = ""
            '    rmrk = ""
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("Transportation").ToString)
            '    If Len(rmrk) > 70 Then
            '        For I = 70 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 70
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    If Trim(prn_HdDt.Rows(0).Item("Terms_Label3").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    End If
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Erection_Charges").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    rmrk1 = ""
            '    rmrk = ""
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("Erection_Charges").ToString)
            '    If Len(rmrk) > 70 Then
            '        For I = 70 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 70
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    If Trim(prn_HdDt.Rows(0).Item("Terms_Label4").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    End If
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Installation_Charges").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    rmrk1 = ""
            '    rmrk = ""
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("Installation_Charges").ToString)
            '    If Len(rmrk) > 70 Then
            '        For I = 70 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 70
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    If Trim(prn_HdDt.Rows(0).Item("Terms_Label5").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label5").ToString, LMargin + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    End If
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Documents").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    rmrk1 = ""
            '    rmrk = ""
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("Documents").ToString)
            '    If Len(rmrk) > 70 Then
            '        For I = 70 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 70
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    If Trim(prn_HdDt.Rows(0).Item("Terms_Label6").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label6").ToString, LMargin + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    End If
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("FrightAndUnloading_Text").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    rmrk1 = ""
            '    rmrk = ""
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("FrightAndUnloading_Text").ToString)
            '    If Len(rmrk) > 70 Then
            '        For I = 70 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 70
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    If Trim(prn_HdDt.Rows(0).Item("Terms_Label7").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label7").ToString, LMargin + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    End If
            'End If


            'If Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString) <> "" Then


            '    ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString)
            '    ItmNm2 = ""
            '    If Len(ItmNm1) > 50 Then
            '        For I = 50 To 1 Step -1
            '            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 50

            '        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
            '        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)

            '    End If



            '    If Trim(prn_HdDt.Rows(0).Item("Terms_Label8").ToString) <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Terms_Label8").ToString), LMargin + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
            '    End If
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    If ItmNm2 <> "" Then

            '        CurY = CurY + TxtHgt

            '        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + W1 + 15, CurY, 0, 0, p1Font)

            '    End If
            'End If
            'If Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + W1 + 15, CurY, 0, 0, p1Font)
            'End If
            'If Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + W1 + 15, CurY, 0, 0, p1Font)
            'End If


            'If CurY < CurY1 Then
            '    CurY = CurY1
            'End If
            'If CurY < 900 Then
            '    CurY = 900
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
            '    CurY = CurY + 5
            '    rmrk = Trim(prn_HdDt.Rows(0).Item("Remarks").ToString)
            '    rmrk1 = ""
            '    If Len(rmrk) > 100 Then
            '        For I = 100 To 1 Step -1
            '            If Mid$(Trim(rmrk), I, 1) = " " Or Mid$(Trim(rmrk), I, 1) = "," Or Mid$(Trim(rmrk), I, 1) = "." Or Mid$(Trim(rmrk), I, 1) = "-" Or Mid$(Trim(rmrk), I, 1) = "/" Or Mid$(Trim(rmrk), I, 1) = "_" Or Mid$(Trim(rmrk), I, 1) = "(" Or Mid$(Trim(rmrk), I, 1) = ")" Or Mid$(Trim(rmrk), I, 1) = "\" Or Mid$(Trim(rmrk), I, 1) = "[" Or Mid$(Trim(rmrk), I, 1) = "]" Or Mid$(Trim(rmrk), I, 1) = "{" Or Mid$(Trim(rmrk), I, 1) = "}" Then Exit For
            '        Next I
            '        If I = 0 Then I = 100
            '        rmrk1 = Microsoft.VisualBasic.Right(Trim(rmrk), Len(rmrk) - I)
            '        rmrk = Microsoft.VisualBasic.Left(Trim(rmrk), I - 1)
            '    End If
            '    Common_Procedures.Print_To_PrintDocument(e, "Remarks ", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, p1Font)
            '    Common_Procedures.Print_To_PrintDocument(e, rmrk, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    If rmrk1 <> "" Then
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, rmrk1, LMargin + W1 + 15, CurY, 0, 0, p1Font)
            '    End If
            'End If




            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'e.Graphics.DrawLine(Pens.Black, CenPs, LnAr(5), CenPs, CurY)

            CurY1 = CurY
            CurY = CurY + TxtHgt + 10
            pFont = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
            p1Font = New Font("Calibri", 8, FontStyle.Regular)
            'Common_Procedures.Print_To_PrintDocument(e, "TERMS & CONDITIONS :", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "1. Delivery Should be made strictly in accourdance with our order details.", LMargin + 10, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt - 5
            'Common_Procedures.Print_To_PrintDocument(e, "2. Please quote our purchase order No. & Date in your Delivery Notes and Bills.", LMargin + 10, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt - 5
            'Common_Procedures.Print_To_PrintDocument(e, "3. If the goods are not delivered as per our order, we will reject the materials.", LMargin + 10, CurY, 0, 0, p1Font)



            'CurY = CurY + 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            pFont = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + ClAr(2) - 120, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            If CurY < CurY1 Then
                CurY = CurY1
            End If
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


End Class