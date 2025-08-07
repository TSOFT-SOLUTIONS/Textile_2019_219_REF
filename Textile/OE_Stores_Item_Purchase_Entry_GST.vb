Public Class OE_Stores_Item_Purchase_Entry_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GITPU-"
    Private Pk_Condition1 As String = "GITPA-"
    Private cbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private vdgv_DrawNo As String = ""
    Private vCbo_ItmNm As String = ""
    Private vCloPic_STS As Boolean = False
    Private NoCalc_Status As Boolean = False

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_Picture.Visible = False
        pnl_Tax.Visible = False
        New_Entry = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_date.Text = ""

        cbo_EntType.Text = "DIRECT"

        cbo_Ledger.Text = ""
        txt_BillNo.Text = ""

        lbl_GrossAmount.Text = ""

        txt_DiscPerc.Text = ""
        lbl_DiscAmount.Text = ""

        txt_AssessableValue.Text = ""

        txt_Freight.Text = ""
        txt_AddLess.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = "0.00"
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""
        txt_Remarks.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_Selection.Rows.Clear()
        dgv_Tax_Details.Rows.Clear()
        dgv_Tax_Total_Details.Rows.Clear()

        lbl_AmountInWords.Text = "Rupees  :  "
        cbo_PaymentMethod.Text = "CREDIT"

        cbo_Grid_Department.Text = False
        cbo_Grid_Item.Visible = False
        cbo_Grid_Brand.Text = False
        cbo_Grid_Unit.Visible = False

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Ledger.Text = ""
            cbo_Filter_Ledger.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        PictureBox1.Image = Nothing
        vCloPic_STS = False

        vdgv_DrawNo = ""
        vCbo_ItmNm = ""

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.PaleGreen
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
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Selection.CurrentCell.Selected = False
    End Sub

    Private Sub Item_Purchase_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer = 0
        Dim CompCondt As String = ""

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PurchaseAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PurchaseAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Item_Purchase_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable

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

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or  (Ledger_Type = '' and (AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6) )  ) order by Ledger_DisplayName", con)
        da.Fill(dt5)
        cbo_Ledger.DataSource = dt5
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 27 ) order by Ledger_DisplayName", con)
        da.Fill(dt6)
        cbo_PurchaseAc.DataSource = dt6
        cbo_PurchaseAc.DisplayMember = "Ledger_DisplayName"

        cbo_EntType.Items.Clear()
        cbo_EntType.Items.Add("DIRECT")
        cbo_EntType.Items.Add("PO")

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("-NIL-")
        cbo_TaxType.Items.Add("GST")
        cbo_TaxType.Items.Add("NO TAX")


        cbo_PaymentMethod.Items.Clear()
        cbo_PaymentMethod.Items.Add("")
        cbo_PaymentMethod.Items.Add("CASH")
        cbo_PaymentMethod.Items.Add("CREDIT")
        cbo_PaymentMethod.Items.Add("ADVANCE")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Picture.Visible = False
        pnl_Picture.Left = (Me.Width - pnl_Picture.Width) - 25
        pnl_Picture.Top = (Me.Height - pnl_Picture.Height) - 50
        pnl_Picture.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Tax.Visible = False
        pnl_Tax.Left = (Me.Width - pnl_Tax.Width) \ 2
        pnl_Tax.Top = (Me.Height - pnl_Tax.Height) \ 2
        pnl_Tax.BringToFront()

        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EntType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Department.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Item.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Brand.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PurchaseAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentMethod.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EntType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Department.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Item.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Brand.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PurchaseAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentMethod.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
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

    Private Sub Item_Purchase_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Item_Purchase_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Picture.Visible = True Then
                    btn_ClosePicture_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Tax.Visible = True Then
                    btn_Tax_Close_Click(sender, e)
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

                        If Trim(UCase(cbo_EntType.Text)) = "PO" Then
                            If .CurrentCell.ColumnIndex >= 5 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    txt_DiscPerc.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(5)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(5)

                            End If

                            Return True



                        Else

                            If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then

                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    txt_DiscPerc.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If

                            Else
                                '.CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                                SendKeys.Send("{TAB}")
                            End If

                            Return True

                        End If

                    ElseIf keyData = Keys.Up Then

                        If Trim(UCase(cbo_EntType.Text)) = "PO" Then
                            If .CurrentCell.ColumnIndex <= 5 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    txt_BillNo.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(5)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)

                            End If

                            Return True

                        Else

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    txt_BillNo.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(21)

                                End If

                            Else
                                '.CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                                SendKeys.Send("+{TAB}")
                            End If

                            Return True

                        End If


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
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Stores_Item_Purchase_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  Where a.Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Item_Purchase_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("Item_Purchase_Date").ToString
                cbo_EntType.Text = dt1.Rows(0).Item("Entry_Type").ToString

                'cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                If IsDBNull(dt1.Rows(0).Item("Ledger_Name").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Ledger_Name").ToString) <> "" Then
                        If Val(dt1.Rows(0).Item("Ledger_IdNo").ToString) <> 1 Then
                            cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                        Else
                            cbo_Ledger.Text = dt1.Rows(0).Item("Cash_PartyName").ToString
                        End If
                    Else
                        cbo_Ledger.Text = dt1.Rows(0).Item("Cash_PartyName").ToString
                    End If
                Else
                    cbo_Ledger.Text = dt1.Rows(0).Item("Cash_PartyName").ToString
                End If

                txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                cbo_PurchaseAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("PurchaseAc_IdNo").ToString))

                '  lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")
                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Total_NetAmount").ToString), "#########0.00")

                txt_DiscPerc.Text = Val(dt1.Rows(0).Item("CashDiscount_Percentage").ToString)
                lbl_DiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "#########0.00")
                txt_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")


                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))
                cbo_TaxType.Text = dt1.Rows(0).Item("GST_Tax_Type").ToString

                txt_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")
                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_CGST_Amount").ToString), "#########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_SGST_Amount").ToString), "#########0.00")
                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_IGST_Amount").ToString), "#########0.00")

                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                cbo_PaymentMethod.Text = dt1.Rows(0).Item("Payment_Method").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, b.Drawing_No, c.Department_name, d.Unit_name, e.Brand_Name from Stores_Item_Purchase_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Department_Head c ON b.Department_idno = c.Department_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Brand_Head e ON a.Brand_idno = e.Brand_idno where a.Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                SNo = 0

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
                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Unit_name").ToString
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = Val(dt2.Rows(i).Item("Po_No").ToString)
                        dgv_Details.Rows(n).Cells(10).Value = Val(dt2.Rows(i).Item("Purchase_Details_SlNo").ToString)
                        dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Po_Code").ToString
                        dgv_Details.Rows(n).Cells(12).Value = Val(dt2.Rows(i).Item("Po_Details_SlNo").ToString)
                        dgv_Details.Rows(n).Cells(13).Value = Val(dt2.Rows(i).Item("PurchaseReturn_Quantity").ToString)

                        dgv_Details.Rows(n).Cells(12).Value = Val(dt2.Rows(i).Item("Taxable_Value").ToString)
                        dgv_Details.Rows(n).Cells(13).Value = Val(dt2.Rows(i).Item("GST_Percentage").ToString)
                        dgv_Details.Rows(n).Cells(14).Value = Val(dt2.Rows(i).Item("HSN_Code").ToString)

                        dgv_Details.Rows(n).Cells(20).Value = Val(dt2.Rows(i).Item("Disc_Perc").ToString)
                        dgv_Details.Rows(n).Cells(21).Value = Format(Val(dt2.Rows(i).Item("Disc_Amt").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(22).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")


                        'If Val(dgv_Details.Rows(n).Cells(13).Value) <> 0 Then
                        '    LockSTS = True

                        '    For j = 0 To dgv_Details.ColumnCount - 1
                        '        dgv_Details.Rows(i).Cells(j).Style.BackColor = Color.LightGray
                        '    Next

                        'End If

                    Next i

                End If

                With dgv_Details_Total
                    .Rows.Clear()
                    .Rows.Add()
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")
                    .Rows(0).Cells(22).Value = Format(Val(dt1.Rows(0).Item("Total_NetAmount").ToString), "########0.00")
                    .Rows(0).Cells(21).Value = Format(Val(dt1.Rows(0).Item("Total_DiscAmt").ToString), "########0.00")

                End With
                da4 = New SqlClient.SqlDataAdapter("Select a.* from Store_Item_Purchase_GST_Tax_Details a Where a.Store_Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)
                dt4 = New DataTable
                da4.Fill(dt4)

                With dgv_Tax_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt4.Rows.Count > 0 Then

                        For I = 0 To dt4.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = SNo
                            .Rows(n).Cells(1).Value = Trim(dt4.Rows(I).Item("HSN_Code").ToString)
                            .Rows(n).Cells(2).Value = IIf(Val(dt4.Rows(I).Item("Taxable_Amount").ToString) <> 0, Format(Val(dt4.Rows(I).Item("Taxable_Amount").ToString), "############0.00"), "")
                            .Rows(n).Cells(3).Value = IIf(Val(dt4.Rows(I).Item("CGST_Percentage").ToString) <> 0, Val(dt4.Rows(I).Item("CGST_Percentage").ToString), "")
                            .Rows(n).Cells(4).Value = IIf(Val(dt4.Rows(I).Item("CGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(I).Item("CGST_Amount").ToString), "##########0.00"), "")
                            .Rows(n).Cells(5).Value = IIf(Val(dt4.Rows(I).Item("SGST_Percentage").ToString) <> 0, Val(dt4.Rows(I).Item("SGST_Percentage").ToString), "")
                            .Rows(n).Cells(6).Value = IIf(Val(dt4.Rows(I).Item("SGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(I).Item("SGST_Amount").ToString), "###########0.00"), "")
                            .Rows(n).Cells(7).Value = IIf(Val(dt4.Rows(I).Item("IGST_Percentage").ToString) <> 0, Val(dt4.Rows(I).Item("IGST_Percentage").ToString), "")
                            .Rows(n).Cells(8).Value = IIf(Val(dt4.Rows(I).Item("IGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(I).Item("IGST_Amount").ToString), "###########0.00"), "")
                        Next I

                    End If

                End With

                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                'If LockSTS = True Then
                '    cbo_Ledger.Enabled = False
                '    cbo_Ledger.BackColor = Color.LightGray
                'End If

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

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Stores_Purchase_Inward, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Stores_Purchase_Inward, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

        Da = New SqlClient.SqlDataAdapter("select sum(PurchaseReturn_Quantity) from Stores_Item_Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Alreay some quantity retuned against this purchase", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans


            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), trans)


            cmd.CommandText = "Update Stores_Item_PO_Details Set Purchased_Quantity = a.Purchased_Quantity - b.Quantity from Stores_Item_PO_Details a, Stores_Item_Purchase_Details b where b.Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and b.Entry_Type = 'PO' and a.Po_Code = b.Po_Code and a.PO_Details_SlNo = b.PO_Details_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stores_Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stores_Item_Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Stores_Item_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()

            If InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_1") > 0 Then
                MessageBox.Show("Invalid Purchase quantity", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_2") > 0 Then
                MessageBox.Show("Invalid Cancel quantity", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_3") > 0 Or InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_4") > 0 Then
                MessageBox.Show("Invalid Purchase Return quantity", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_5") > 0 Then
                MessageBox.Show("Invalid Return quantity, Lesser than PO Quantity", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_Purchase_Details_1") > 0 Then
                MessageBox.Show("Invalid Purchase Return quantity", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_Purchase_Details_2") > 0 Then
                MessageBox.Show("Invalid Return quantity, Lesser than Purchase Quantity", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

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

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or (Ledger_Type = '' and AccountsGroup_IdNo = 14 ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_Ledger.DataSource = dt1
            cbo_Filter_Ledger.DisplayMember = "Ledger_DisplayName"

            cbo_Filter_Ledger.Text = ""
            cbo_Filter_Ledger.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Back.Enabled = False
        If Filter_Status = False Then
            If dgv_Filter_Details.Rows.Count > 0 Then
                dgv_Filter_Details.Focus()
                dgv_Filter_Details.CurrentCell = dgv_Filter_Details.Rows(0).Cells(0)
                dgv_Filter_Details.CurrentCell.Selected = True

            Else
                If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

            End If

        Else
            If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

        End If


    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Stores_Purchase_Inward, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Stores_Purchase_Inward, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

            inpno = InputBox("Enter New Ref No.", "FOR NEW NO INSERTION...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Item_Purchase_No from Stores_Item_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code LIKE '" & Trim(Pk_Condition) & "%' AND Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(RefCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT PO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW NO ...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            da = New SqlClient.SqlDataAdapter("select top 1 Item_Purchase_No from Stores_Item_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code LIKE '" & Trim(Pk_Condition) & "%' AND Item_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Item_Purchase_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Item_Purchase_No from Stores_Item_Purchase_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code LIKE '" & Trim(Pk_Condition) & "%' AND Item_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Item_Purchase_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Item_Purchase_No from Stores_Item_Purchase_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Item_Purchase_Code LIKE '" & Trim(Pk_Condition) & "%' AND  Item_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Item_Purchase_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Item_Purchase_No from Stores_Item_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and   Item_Purchase_Code LIKE '" & Trim(Pk_Condition) & "%' AND Item_Purchase_Code like  '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Item_Purchase_No desc", con)
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
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Stores_Item_Purchase_Head", "Item_Purchase_Code", "For_OrderBy", "Item_Purchase_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            da1 = New SqlClient.SqlDataAdapter("select Top 1 a.*, b.ledger_name as SalesAcName, c.ledger_name as TaxAcName from Stores_Item_Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.PurchaseAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Item_Purchase_No desc", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                If dt1.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_PurchaseAc.Text = dt1.Rows(0).Item("SalesAcName").ToString
                If dt1.Rows(0).Item("Entry_Type").ToString <> "" Then cbo_EntType.Text = dt1.Rows(0).Item("Entry_Type").ToString
                'If Dt1.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_SalesAc.Text = Dt1.Rows(0).Item("SalesAcName").ToString
                'If Dt1.Rows(0).Item("TaxAcName").ToString <> "" Then cbo_VatAc.Text = Dt1.Rows(0).Item("TaxAcName").ToString
            End If

            dt1.Clear()

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da1.Dispose()

        End Try



    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            RefCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Item_Purchase_No from Stores_Item_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code LIKE '" & Trim(Pk_Condition) & "%' AND Item_Purchase_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim PurcAc_ID As Integer = 0
        Dim TxAc_ID As Integer = 0
        Dim Nr As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim CsParNm As String
        Dim vdisAmt As Single = 0
        Dim vNetamt As Single = 0

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Stores_Purchase_Inward, New_Entry) = False Then Exit Sub

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

        If (Trim(UCase(cbo_EntType.Text)) <> "DIRECT" And Trim(UCase(cbo_EntType.Text)) <> "PO") Then
            MessageBox.Show("Invalid Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
            Exit Sub
        End If



        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        CsParNm = ""
        If Led_IdNo = 0 Then
            If Trim(UCase(cbo_PaymentMethod.Text)) = "CREDIT" Then
                MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
                Exit Sub

            Else
                Led_IdNo = 1
                CsParNm = Trim(cbo_Ledger.Text)

            End If
        End If

        If Led_IdNo = 1 And Trim(CsParNm) = "" Then
            CsParNm = "Cash"
        End If

        PurcAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAc.Text)
        If PurcAc_ID = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PurchaseAc.Enabled And cbo_PurchaseAc.Visible Then cbo_PurchaseAc.Focus()
            Exit Sub
        End If

        If Trim(txt_BillNo.Text) = "" Then
            MessageBox.Show("Invalid BillNo", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from Stores_Item_Purchase_Head where Ledger_IdNo = " & Str(Val(Led_IdNo)) & " and Bill_No = '" & Trim(txt_BillNo.Text) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Item_Purchase_Code <> '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            MessageBox.Show("Duplicate BillNo to this party", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
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
                If Brand_ID = 0 Then
                    MessageBox.Show("Invalid Brand Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)
                    End If
                    Exit Sub
                End If

                Unit_ID = Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(6).Value)
                If Unit_ID = 0 Then
                    MessageBox.Show("Invalid Unit Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(6)
                    End If
                    Exit Sub
                End If

                'If Val(dgv_Details.Rows(i).Cells(7).Value) = 0 Then
                '    MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(7)
                '    End If
                '    Exit Sub
                'End If

            End If

        Next

        NoCalc_Status = False
        TotalQuantity_Calculation()

        vTotQty = 0 : vTotAmt = 0 : vdisAmt = 0 : vNetamt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotQty = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            vdisAmt = Val(dgv_Details_Total.Rows(0).Cells(21).Value())
            vNetamt = Val(dgv_Details_Total.Rows(0).Cells(22).Value())
        End If

        If vTotQty = 0 Then
            MessageBox.Show("Invalid Purchase Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dgv_Details.Enabled And dgv_Details.Visible Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            End If
            Exit Sub
        End If



        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Stores_Item_Purchase_Head", "Item_Purchase_Code", "For_OrderBy", "Item_Purchase_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PurcDate", dtp_date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Stores_Item_Purchase_Head(                   Item_Purchase_Code,             Company_IdNo         ,           Item_Purchase_No   ,                               for_OrderBy                             ,      Item_Purchase_Date   ,       Entry_Type              ,           Ledger_IdNo     ,  Cash_PartyName           ,             Bill_No          ,           PurchaseAc_IdNo  ,        Total_Quantity    ,       Total_Amount       ,      CashDiscount_Percentage       ,       CashDiscount_Amount            ,              TaxAc_IdNo    ,                   Freight_Amount       ,             AddLess_Amount       ,             RoundOff_Amount         ,                   Net_Amount             ,               Remarks       ,               Payment_Method              , Assessable_Value                       ,Total_CGST_Amount                 ,Total_SGST_Amount                ,Total_IGST_Amount                  ,     GST_Tax_Type             , Total_DiscAmt ,     Total_NetAmount ) " &
                                    "                         Values   ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @PurcDate     , '" & Trim(cbo_EntType.Text) & "', " & Str(Val(Led_IdNo)) & ", '" & Trim(CsParNm) & "',  '" & Trim(txt_BillNo.Text) & "', " & Str(Val(PurcAc_ID)) & ", " & Str(Val(vTotQty)) & ", " & Str(Val(vTotAmt)) & ", " & Str(Val(txt_DiscPerc.Text)) & ", " & Str(Val(lbl_DiscAmount.Text)) & ", " & Str(Val(TxAc_ID)) & ",   " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", '" & Trim(txt_Remarks.Text) & "' , '" & Trim(cbo_PaymentMethod.Text) & "' ," & Val(txt_AssessableValue.Text) & " ," & Val(lbl_CGST_Amount.Text) & " ," & Val(lbl_SGST_Amount.Text) & "," & Val(lbl_IGST_Amount.Text) & " ,'" & Trim(cbo_TaxType.Text) & "',    " & Str(Val(vdisAmt)) & ", " & Str(Val(vNetamt)) & " )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Stores_Item_Purchase_Head set Item_Purchase_Date= @PurcDate, Entry_Type = '" & Trim(cbo_EntType.Text) & "', Ledger_IdNo = " & Str(Val(Led_IdNo)) & ", Bill_No = '" & Trim(txt_BillNo.Text) & "', PurchaseAc_IdNo = " & Str(Val(PurcAc_ID)) & ", Total_Quantity = " & Str(Val(vTotQty)) & ", Total_Amount = " & Str(Val(vTotAmt)) & ", CashDiscount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(lbl_DiscAmount.Text)) & ",  TaxAc_IdNo = " & Str(Val(TxAc_ID)) & ", GST_Tax_Type = 'GST', Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ",Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Cash_PartyName = '" & Trim(CsParNm) & "',  RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Remarks = '" & Trim(txt_Remarks.Text) & "',Assessable_Value = " & Val(txt_AssessableValue.Text) & " , Total_CGST_Amount = " & Val(lbl_CGST_Amount.Text) & " ,Total_SGST_Amount = " & Val(lbl_SGST_Amount.Text) & ",Total_IGST_Amount =" & Val(lbl_IGST_Amount.Text) & " ,Total_DiscAmt = " & Str(Val(vdisAmt)) & ", Total_NetAmount = " & Str(Val(vNetamt)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stores_Item_PO_Details Set Purchased_Quantity = a.Purchased_Quantity - b.Quantity from Stores_Item_PO_Details a, Stores_Item_Purchase_Details b where b.Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and b.Entry_Type = 'PO' and a.Po_Code = b.Po_Code and a.PO_Details_SlNo = b.PO_Details_SlNo"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(txt_BillNo.Text)
            Partcls = "Purc : Ref.No. " & Trim(lbl_RefNo.Text)

            'cmd.CommandText = "Delete from Stores_Item_Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(NewCode) & "' and PurchaseReturn_Quantity = 0"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stores_Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        Item_ID = Common_Procedures.itemalais_NameToIdNo(con, dgv_Details.Rows(i).Cells(3).Value, tr)

                        Brand_ID = Common_Procedures.Brand_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value, tr)

                        Unit_ID = Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(6).Value, tr)

                        cmd.CommandText = "Update Stores_Item_Purchase_Details set Item_Purchase_Date= @PurcDate, Entry_Type = '" & Trim(cbo_EntType.Text) & "', Ledger_IdNo = " & Str(Val(Led_IdNo)) & ", Bill_No = '" & Trim(txt_BillNo.Text) & "', Sl_No = " & Str(Val(Sno)) & ", Item_Idno = " & Str(Val(Item_ID)) & ", Brand_IdNo = " & Str(Val(Brand_ID)) & ", Quantity = " & Str(Val(.Rows(i).Cells(5).Value)) & ", Unit_Idno = " & Val(Unit_ID) & ", Rate = " & Str(Val(.Rows(i).Cells(7).Value)) & ", Amount = " & Str(Val(.Rows(i).Cells(8).Value)) & ", Po_No = '" & Trim(.Rows(i).Cells(9).Value) & "', Po_Code = '" & Trim(.Rows(i).Cells(11).Value) & "', PO_Details_SlNo = " & Str(Val(.Rows(i).Cells(12).Value)) & ",Taxable_Value=" & Str(Val(.Rows(i).Cells(16).Value)) & ", GST_Percentage=" & Str(Val(.Rows(i).Cells(17).Value)) & ",HSN_Code=" & Str(Val(.Rows(i).Cells(18).Value)) & ", Disc_Perc = " & Str(Val(.Rows(i).Cells(20).Value)) & "  , Disc_Amt = " & Str(Val(.Rows(i).Cells(21).Value)) & ", Total_Amount = " & Str(Val(.Rows(i).Cells(22).Value)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Purchase_Details_SlNo = " & Str(Val(.Rows(i).Cells(10).Value))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Stores_Item_Purchase_Details (   Item_Purchase_Code      ,           Company_IdNo            ,       Item_Purchase_No        ,                       for_OrderBy                                     ,Item_Purchase_Date,        Entry_Type          ,           Ledger_IdNo         ,           Bill_No                 ,   Sl_No           ,           Item_IdNo       ,           Brand_IdNo          ,           Quantity                ,       Unit_idNo       ,                   Rate                    ,               Amount                  ,                   Po_No                   ,               Po_Code                     ,               PO_Details_SlNo         , PurchaseReturn_Quantity   ,               Taxable_Value               ,           GST_Percentage                  ,                           HSN_Code                ,                 Disc_Perc,                                   Disc_Amt,                                               Total_Amount          ) " &
                                                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @PurcDate    , '" & Trim(cbo_EntType.Text) & "', " & Str(Val(Led_IdNo)) & ", '" & Trim(txt_BillNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(Item_ID)) & ", " & Str(Val(Brand_ID)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Val(Unit_ID) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", '" & Trim(.Rows(i).Cells(9).Value) & "', '" & Trim(.Rows(i).Cells(11).Value) & "', " & Str(Val(.Rows(i).Cells(12).Value)) & ",               0          ," & Str(Val(.Rows(i).Cells(16).Value)) & "," & Str(Val(.Rows(i).Cells(17).Value)) & ",'" & Trim(.Rows(i).Cells(18).Value) & "', " & Str(Val(.Rows(i).Cells(20).Value)) & "  , " & Str(Val(.Rows(i).Cells(21).Value)) & " , " & Str(Val(.Rows(i).Cells(22).Value)) & "  )"
                            cmd.ExecuteNonQuery()
                        End If

                        If Trim(UCase(cbo_EntType.Text)) = "PO" Then

                            cmd.CommandText = "Update Stores_Item_PO_Details Set Purchased_Quantity = Purchased_Quantity + " & Str(Val(.Rows(i).Cells(5).Value)) & " where Po_Code = '" & Trim(.Rows(i).Cells(11).Value) & "' and PO_Details_SlNo = " & Str(Val(.Rows(i).Cells(12).Value)) & " and Ledger_IdNo = " & Str(Val(Led_IdNo))
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                tr.Rollback()
                                MessageBox.Show("Mismatch of PO and Party details", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
                                Exit Sub
                            End If

                        End If

                        cmd.CommandText = "Insert into Stores_Stock_Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, Entry_ID, Party_Bill_No, Particulars, Sl_No, Item_IdNo, Unit_IdNo, Brand_IdNo, Quantity_New, Quantity_Old_Usable, Quantity_Old_Scrap) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PurcDate, " & Str(Val(Led_IdNo)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Item_ID)) & ", " & Str(Val(Unit_ID)) & ", " & Str(Val(Brand_ID)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", 0, 0 )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With
            '---Tax Details
            cmd.CommandText = "Delete from Store_Item_Purchase_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Store_Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Tax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Store_Item_Purchase_GST_Tax_Details   ( Store_Item_Purchase_Code  ,               Company_IdNo       ,      Store_Item_Purchase_No    ,                               for_OrderBy                              , Store_Item_Purchase_Date  ,         Ledger_IdNo     ,            Sl_No     , HSN_Code                               ,Taxable_Amount                            ,CGST_Percentage                           ,CGST_Amount                               ,SGST_Percentage                            ,SGST_Amount                              ,IGST_Percentage                          ,IGST_Amount ) " &
                                                "     Values                                  (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @PurcDate     , " & Str(Val(Led_IdNo)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & "  ," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & ") "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = "", vAc_IdNo As Integer = 0, vNarr As String = ""

            If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
                vAc_IdNo = 1
                vNarr = "Bill No : " & Trim(txt_BillNo.Text) & " ,  " & Trim(cbo_Ledger.Text)

            Else
                vAc_IdNo = Led_IdNo
                vNarr = "Bill No : " & Trim(txt_BillNo.Text)

            End If

            vLed_IdNos = vAc_IdNo & "|" & PurcAc_ID
            vVou_Amts = Val(CSng(lbl_NetAmount.Text)) & "|" & -1 * (Val(CSng(lbl_NetAmount.Text)))
            If Common_Procedures.Voucher_Updation(con, "Gst.Store.ItemPurc", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(dtp_date.Text), vNarr, vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If
            'End If


            'Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            ''If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            'vLed_IdNos = Led_IdNo & "|" & PurcAc_ID
            'vVou_Amts = Val(CSng(lbl_NetAmount.Text)) & "|" & -1 * (Val(CSng(lbl_NetAmount.Text)))
            'If Common_Procedures.Voucher_Updation(con, "Gst.Store.ItemPurc", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(dtp_date.Text), "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
            '    Throw New ApplicationException(ErrMsg)
            'End If
            ''End If



            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), tr)
            'If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Led_IdNo <> 1 Then

            '    vLed_IdNos = "" : vVou_Amts = "" : ErrMsg = ""
            '    If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            '        vLed_IdNos = 1 & "|" & Led_IdNo
            '        vVou_Amts = Val(CSng(lbl_NetAmount.Text)) & "|" & -1 * (Val(CSng(lbl_NetAmount.Text)))
            '        If Common_Procedures.Voucher_Updation(con, "Gst.Store.ItemPayment", Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(dtp_date.Text), "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
            '            Throw New ApplicationException(ErrMsg)
            '        End If
            '    End If
            'End If


            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If
            If Trim(UCase(cbo_PaymentMethod.Text)) <> "CASH" Then
                Dim VouBil As String = ""
                VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_date.Text, Led_IdNo, Trim(txt_BillNo.Text), 0, Val(CSng(lbl_NetAmount.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr)
                If Trim(UCase(VouBil)) = "ERROR" Then
                    Throw New ApplicationException("Error on Voucher Bill Posting")
                End If
            End If

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If New_Entry = True Then
                new_record()
            Else
                move_record(lbl_RefNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()

            If InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_1") > 0 Then
                MessageBox.Show("Invalid Purchase quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_2") > 0 Then
                MessageBox.Show("Invalid Cancel quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_3") > 0 Or InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_4") > 0 Then
                MessageBox.Show("Invalid Purchase Return quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_5") > 0 Then
                MessageBox.Show("Invalid Purchase quantity, Greater than PO Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_Purchase_Details_1") > 0 Then
                MessageBox.Show("Invalid Purchase Return quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_Purchase_Details_2") > 0 Then
                MessageBox.Show("Invalid Purchase quantity, Lesser than Return Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally

            cmd.Dispose()
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
                    txt_BillNo.Focus()

                Else
                    .CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index - 1).Cells(21)

                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_Department.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                e.Handled = True
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_DiscPerc.Focus()

                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_Department_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Department.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Department, Nothing, "Department_Head", "Department_name", "", "(Department_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_DiscPerc.Focus()

                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End With



        End If
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown

        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_PaymentMethod, cbo_PurchaseAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6) ) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6) ) ", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(cbo_EntType.Text)) = "PO" Then

                If MessageBox.Show("Do you want to select P.O?", "FOR P.O SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    cbo_PurchaseAc.Focus()

                End If

            Else
                cbo_PurchaseAc.Focus()

            End If

        End If

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

                Da = New SqlClient.SqlDataAdapter("select a.Item_name, b.unit_name from Stores_Item_Head a left outer join unit_head b on a.unit_idno = b.unit_idno where a.department_idno = " & Str(Val(dep_idno)) & " and a.drawing_no = '" & Trim(dno) & "'", con)
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

            If e.ColumnIndex = 1 And Trim(UCase(cbo_EntType.Text)) <> "PO" Then

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

            If e.ColumnIndex = 2 And vCloPic_STS = False And Trim(UCase(cbo_EntType.Text)) <> "PO" Then
                btn_ShowPicture_Click(sender, e)
            Else
                pnl_Picture.Visible = False
            End If

            If e.ColumnIndex = 3 And Trim(UCase(cbo_EntType.Text)) <> "PO" Then

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


            If e.ColumnIndex = 4 And Trim(UCase(cbo_EntType.Text)) <> "PO" Then

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


            'If e.ColumnIndex = 6  And Trim(UCase(cbo_EntType.Text)) <> "PO" Then

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
        If FrmLdSTS = True Then
            Exit Sub
        End If

        On Error Resume Next
        With dgv_Details
            If .Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                If e.ColumnIndex = 5 Or e.ColumnIndex = 7 Or e.ColumnIndex = 8 Or e.ColumnIndex = 20 Or e.ColumnIndex = 21 Or e.ColumnIndex = 22 Then
                    .CurrentRow.Cells(8).Value = Format(Val(.CurrentRow.Cells(5).Value) * Val(.CurrentRow.Cells(7).Value), "#########0.00")
                    '.CurrentRow.Cells(21).Value = Format(Val(.CurrentRow.Cells(8).Value) * Val(.CurrentRow.Cells(20).Value) / 100, "#########0.00")
                    '.CurrentRow.Cells(22).Value = Format(Val(.CurrentRow.Cells(8).Value) - Val(.CurrentRow.Cells(21).Value), "#########0.00")
                    TotalQuantity_Calculation()
                    GST_Calculation()
                End If

                If e.ColumnIndex = 8 Or e.ColumnIndex = 20 Or e.ColumnIndex = 21 Then
                    If .CurrentRow.Cells(20).Value = "" Then
                        .CurrentRow.Cells(22).Value = Format(Val(.CurrentRow.Cells(8).Value) - Val(.CurrentRow.Cells(21).Value), "#########0.00")
                    Else
                        .CurrentRow.Cells(21).Value = Format(Val(.CurrentRow.Cells(8).Value) * Val(.CurrentRow.Cells(20).Value) / 100, "#########0.00")
                        .CurrentRow.Cells(22).Value = Format(Val(.CurrentRow.Cells(8).Value) - Val(.CurrentRow.Cells(21).Value), "#########0.00")

                    End If
                End If

            End If
        End With
    End Sub


    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Then
                    If Trim(UCase(cbo_EntType.Text)) = "PO" Or Val(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> 0 Then
                        e.Handled = True
                    End If

                ElseIf .CurrentCell.ColumnIndex = 5 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                ElseIf .CurrentCell.ColumnIndex = 20 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                ElseIf .CurrentCell.ColumnIndex = 7 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Or Trim(UCase(cbo_EntType.Text)) = "PO" Then ' Or Val(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

        'With dgv_Details
        '    If .Visible Then
        '        If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then

        '            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
        '                e.Handled = True
        '            End If

        '        End If
        '    End If
        'End With

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.ColumnIndex <= 1 Then
                    If .CurrentCell.RowIndex = 0 Then
                        txt_BillNo.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 5)
                    End If
                End If
            End If

        End With

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

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_Grid_Item_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Item.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_Item.Text)
    End Sub

    Private Sub cbo_Grid_Item_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Item.KeyDown
        Dim dep_idno As Integer = 0
        Dim Condt As String

        cbo_KeyDwnVal = e.KeyValue

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Grid_Department.Text))

        Condt = ""
        If dep_idno <> 0 And dep_idno <> 1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Item, Nothing, Nothing, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Item.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Item.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Item_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Item.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dno_nm As String
        Dim Unt_nm As String
        Dim Dep_nm As String
        Dim New_Rate As Double = 0
        Dim dep_idno As Integer = 0
        Dim Itm_idno As Integer = 0
        Dim Brand_idno As Integer = 0
        Dim Condt As String

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Grid_Department.Text))
        Brand_idno = Common_Procedures.Brand_NameToIdNo(con, Trim(cbo_Grid_Brand.Text))

        Condt = ""
        If dep_idno <> 0 And dep_idno <> 1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Item, Nothing, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6).Value) = "" Or Trim(UCase(vCbo_ItmNm)) <> Trim(UCase(cbo_Grid_Item.Text)) Then

                Itm_idno = Common_Procedures.itemalais_NameToIdNo(con, Trim(cbo_Grid_Item.Text))

                da = New SqlClient.SqlDataAdapter("select a.Drawing_No, b.unit_name, c.department_name , a.rate from Stores_Item_Head a left outer join unit_head b on a.unit_idno = b.unit_idno left outer join Department_Head c ON a.Department_IdNo = c.Department_IdNo Where a.item_IdNo = " & Str(Val(Itm_idno)), con)
                da.Fill(dt)

                Dep_nm = ""
                dno_nm = ""
                Unt_nm = ""
                New_Rate = 0
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        Dep_nm = Trim(dt.Rows(0).Item("department_name").ToString)
                        dno_nm = Trim(dt.Rows(0).Item("Drawing_No").ToString)
                        Unt_nm = Trim(dt.Rows(0).Item("unit_name").ToString)

                        If Val(Brand_idno) <> 0 Then
                            da1 = New SqlClient.SqlDataAdapter("select a.rate from Stores_Item_details a Where a.item_IdNo = " & Str(Val(Itm_idno)) & " and a.Brand_IdNo = " & Str(Val(Brand_idno)), con)
                            da1.Fill(dt1)
                            If dt1.Rows.Count > 0 Then
                                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                                    New_Rate = Val(dt1.Rows(0).Item("Rate").ToString)
                                End If
                            End If

                            dt1.Dispose()
                            da1.Dispose()

                        End If

                    End If

                End If

                dt.Dispose()
                da.Dispose()


                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value = Trim(Dep_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value = Trim(dno_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6).Value = Trim(Unt_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7).Value = Format(Val(New_Rate), "#########0.00")

            End If

        End If

        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(3).Value = Trim(cbo_Grid_Item.Text)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(4)
        End If

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
                With dgv_Details
                    If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
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
            Dim f As New Stores_Unit_Creation

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
                With dgv_Details
                    If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
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
        Dim Led_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Item_Purchase_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Item_Purchase_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Item_Purchase_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Ledger.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_Ledger.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Stores_Item_Purchase_Head a left outer join ledger_head b on a.Ledger_IdNo = b.Ledger_IdNo Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_Code LIKE '" & Trim(Pk_Condition) & "%' and a.Item_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Item_Purchase_Date, a.for_orderby, a.Item_Purchase_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Purchase_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Item_Purchase_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("bill_no").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

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

    Private Sub cbo_Filter_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Ledger, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")
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
            Dim f As New Stores_Department_Creation

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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dno_nm As String
        Dim Unt_nm As String
        Dim Dep_nm As String
        Dim New_Rate As Double = 0
        Dim dep_idno As Integer = 0
        Dim Itm_idno As Integer = 0
        Dim Brand_idno As Integer = 0

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Grid_Department.Text))
        Brand_idno = Common_Procedures.Brand_NameToIdNo(con, Trim(cbo_Grid_Brand.Text))

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Brand, Nothing, "Brand_HEAD", "Brand_name", "", "(Brand_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6).Value) = "" Or Trim(UCase(vCbo_ItmNm)) <> Trim(UCase(cbo_Grid_Item.Text)) Then

                Itm_idno = Common_Procedures.itemalais_NameToIdNo(con, Trim(cbo_Grid_Item.Text))

                da = New SqlClient.SqlDataAdapter("select a.Drawing_No, b.unit_name, c.department_name , a.rate from Stores_Item_Head a left outer join unit_head b on a.unit_idno = b.unit_idno left outer join Department_Head c ON a.Department_IdNo = c.Department_IdNo Where a.item_IdNo = " & Str(Val(Itm_idno)), con)
                da.Fill(dt)

                Dep_nm = ""
                dno_nm = ""
                Unt_nm = ""
                New_Rate = 0
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        Dep_nm = Trim(dt.Rows(0).Item("department_name").ToString)
                        dno_nm = Trim(dt.Rows(0).Item("Drawing_No").ToString)
                        Unt_nm = Trim(dt.Rows(0).Item("unit_name").ToString)

                        If Val(Brand_idno) <> 0 Then
                            da1 = New SqlClient.SqlDataAdapter("select a.rate from Stores_Item_details a Where a.item_IdNo = " & Str(Val(Itm_idno)) & " and a.Brand_IdNo = " & Str(Val(Brand_idno)), con)
                            da1.Fill(dt1)
                            If dt1.Rows.Count > 0 Then
                                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                                    New_Rate = Val(dt1.Rows(0).Item("Rate").ToString)
                                End If
                            End If

                            dt1.Dispose()
                            da1.Dispose()

                        End If

                    End If

                End If

                dt.Dispose()
                da.Dispose()

                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7).Value = Format(Val(New_Rate), "#########0.00")

            End If

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
                With dgv_Details
                    If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
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
        'If e.KeyValue = 40 Then
        '    e.Handled = True
        '    SendKeys.Send("{TAB}")
        'End If
        'If e.KeyValue = 38 Then
        '    btn_Filter_Show.Focus()
        'End If
    End Sub

    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress

        'If Asc(e.KeyChar) = 13 Then
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_ToDate.KeyDown
        'If e.KeyValue = 40 Then
        '    e.Handled = True
        '    SendKeys.Send("{TAB}")
        'End If
        'If e.KeyValue = 38 Then
        '    e.Handled = True
        '    SendKeys.Send("+{TAB}")
        'End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        'If Asc(e.KeyChar) = 13 Then
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub txt_DiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscPerc.KeyDown
        If e.KeyValue = 38 Then
            e.Handled = True

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If Trim(UCase(cbo_EntType.Text)) = "PO" Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                End If
            Else
                txt_BillNo.Focus()

            End If

        End If

        If e.KeyValue = 40 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        TotalQuantity_Calculation()
        NetAmount_Calculation()
    End Sub



    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        TotalQuantity_Calculation()
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TotalQuantity_Calculation()
    End Sub

    Private Sub lbl_NetAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_NetAmount.TextChanged
        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 38 Then
            e.Handled = True
            txt_AddLess.Focus()
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


    Private Sub TotalQuantity_Calculation()
        Dim Sno As Integer
        Dim TotQty As Single
        Dim TotAmt As Single
        Dim Ttl_Tax_Amount As Double
        Dim Ttl_CashDisc As Double
        Dim Ttl_Taxable_Amount As Double
        Dim disamt As Double
        Dim totalamt As Double

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub


        Sno = 0

        TotQty = 0 : TotAmt = 0 : Ttl_Tax_Amount = 0 : Ttl_CashDisc = 0 : Ttl_Taxable_Amount = 0 : disamt = 0 : totalamt = 0


        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(5).Value) <> 0 Then
                    TotQty = TotQty + Val(.Rows(i).Cells(5).Value)
                    TotAmt = TotAmt + Val(.Rows(i).Cells(8).Value)
                    Ttl_CashDisc = Ttl_CashDisc + Val(.Rows(i).Cells(15).Value())
                    Ttl_Taxable_Amount = Ttl_Taxable_Amount + Val(.Rows(i).Cells(16).Value())
                    disamt = disamt + Val(.Rows(i).Cells(21).Value())
                    totalamt = totalamt + Val(.Rows(i).Cells(22).Value)
                End If
            Next
        End With

        lbl_GrossAmount.Text = Format(Val(totalamt), "########0.00")


        txt_AssessableValue.Text = Format(Val(totalamt), "########0.00")

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(TotQty)
            .Rows(0).Cells(8).Value = Format(Val(TotAmt), "########0.00")
            .Rows(0).Cells(15).Value = Format(Val(Ttl_CashDisc), "########0.00")
            .Rows(0).Cells(16).Value = Format(Val(Ttl_Taxable_Amount), "########0.00")
            .Rows(0).Cells(21).Value = Format(Val(disamt), "########0.00")
            .Rows(0).Cells(22).Value = Format(Val(totalamt), "########0.00")
        End With
        lbl_DiscAmount.Text = Format(Val(Ttl_CashDisc), "########0.00")

        GST_Calculation()
        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim GrsAmt As Single
        Dim NtAmt As Single
        Dim GST_Amt As Single = 0
        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        GrsAmt = 0

        With dgv_Details_Total
            If .Rows.Count > 0 Then
                GrsAmt = Val(.Rows(0).Cells(22).Value)
            End If
        End With

        'If Val(txt_DiscPerc.Text) <> 0 Then
        lbl_DiscAmount.Text = Format(Val(GrsAmt) * Val(txt_DiscPerc.Text) / 100, "########0.00")
        'End If
        GST_Amt = Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)
        NtAmt = Val(GrsAmt) + Val(GST_Amt) + Val(txt_Freight.Text) + Val(txt_AddLess.Text) - Val(lbl_DiscAmount.Text)
        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")

        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_NetAmount.Text)))


    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Stores_Item_Purchase_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*,c.Ledger_Name from Stores_Item_Purchase_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name,  f.Brand_Name from Stores_Item_Purchase_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno  LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
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
            .Right = 30
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

        NoofItems_PerPage = 8

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 325 : ClAr(3) = 150 : ClAr(4) = 80 : ClAr(5) = 75
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        TxtHgt = 19

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString)
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
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Brand_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)
                        ' Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Machine_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

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
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name, f.Brand_Name from Stores_Item_Purchase_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno  LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "PURCHASE RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("Received From :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        'Common_Procedures.Print_To_PrintDocument(e, "Issued To", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Item_Purchase_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Received From", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "Received From :  " & "M/s." & prn_HdDt.Rows(0).Item("Received_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Item_Purchase_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BRAND", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, "MACHINE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

        CurY = CurY + TxtHgt
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

    Private Sub cbo_EntType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EntType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EntType, dtp_date, cbo_TaxType, "", "", "", "")
    End Sub

    Private Sub cbo_EntType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EntType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntType, cbo_TaxType, "", "", "", "")
    End Sub

    Private Sub txt_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BillNo.KeyDown
        If e.KeyValue = 38 Then
            cbo_PurchaseAc.Focus()
        End If
        If e.KeyValue = 40 Then

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If Trim(UCase(cbo_EntType.Text)) = "PO" Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                End If
            Else
                txt_DiscPerc.Focus()

            End If

        End If
    End Sub

    Private Sub txt_BillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BillNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If Trim(UCase(cbo_EntType.Text)) = "PO" Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                End If
            Else
                txt_DiscPerc.Focus()

            End If
        End If
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Qty As Single, Ent_Rate As Single, Ent_PurcRet_Qty As Single
        Dim Ent_DetSlNo As Long

        If Trim(UCase(cbo_EntType.Text)) <> "PO" Then
            MessageBox.Show("Invalid Type", "DOES NOT SELECT PO...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
            Exit Sub
        End If

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PO...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, b.Drawing_No, c.Department_name, d.Unit_name, e.Brand_Name, f.Quantity as Ent_Purchase_Quantity, f.Rate as Ent_Rate, f.Purchase_Details_SlNo as Ent_Purchase_SlNo, f.PurchaseReturn_Quantity as Ent_PurcReturn_Qty from Stores_Item_PO_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Department_Head c ON b.Department_idno = c.Department_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Brand_Head e ON a.Brand_idno = e.Brand_idno LEFT OUTER JOIN Stores_Item_Purchase_Details F ON f.Item_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and f.Entry_Type = '" & Trim(cbo_EntType.Text) & "' and a.Po_Code = f.Po_Code and a.Po_Details_SlNo = f.PO_Details_SlNo Where a.ledger_idno = " & Str(Val(LedIdNo)) & " and ( (a.PO_Quantity - a.Cancel_Quantiy - a.Purchased_Quantity + a.PurchaseReturn_Quantity) > 0 or f.Quantity > 0 ) Order by a.For_OrderBy, a.PO_No, a.PO_Details_SlNo", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    Ent_Qty = 0 : Ent_Rate = 0 : Ent_DetSlNo = 0 : Ent_PurcRet_Qty = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Purchase_SlNo").ToString) = False Then Ent_DetSlNo = Val(Dt1.Rows(i).Item("Ent_Purchase_SlNo").ToString)
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Purchase_Quantity").ToString) = False Then Ent_Qty = Val(Dt1.Rows(i).Item("Ent_Purchase_Quantity").ToString)
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
                    If IsDBNull(Dt1.Rows(i).Item("Ent_PurcReturn_Qty").ToString) = False Then Ent_PurcRet_Qty = Val(Dt1.Rows(i).Item("Ent_PurcReturn_Qty").ToString)

                    If (Val(Dt1.Rows(i).Item("PO_Quantity").ToString) - Val(Dt1.Rows(i).Item("Cancel_Quantiy").ToString) - Val(Dt1.Rows(i).Item("Purchased_Quantity").ToString) + Val(Dt1.Rows(i).Item("PurchaseReturn_Quantity").ToString) + Ent_Qty) > 0 Then

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("PO_No").ToString

                        If IsDBNull(Dt1.Rows(i).Item("Department_name").ToString) = False Then
                            If Trim(Dt1.Rows(i).Item("Department_name").ToString) <> "" Then
                                .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Department_name").ToString
                            Else
                                .Rows(n).Cells(2).Value = Common_Procedures.Department_IdNoToName(con, 1)
                            End If
                        End If

                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Drawing_No").ToString
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Item_name").ToString
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Brand_name").ToString
                        .Rows(n).Cells(6).Value = (Val(Dt1.Rows(i).Item("PO_Quantity").ToString) - Val(Dt1.Rows(i).Item("Cancel_Quantiy").ToString) - Val(Dt1.Rows(i).Item("Purchased_Quantity").ToString) + Val(Dt1.Rows(i).Item("PurchaseReturn_Quantity").ToString) + Ent_Qty)
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Unit_name").ToString
                        .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Rate").ToString), "########0.00")
                        If Val(Ent_Qty) > 0 Then
                            .Rows(n).Cells(9).Value = "1"
                        Else
                            .Rows(n).Cells(9).Value = ""
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("PO_Code").ToString
                        .Rows(n).Cells(11).Value = Val(Dt1.Rows(i).Item("PO_Details_SlNo").ToString)
                        .Rows(n).Cells(12).Value = Val(Ent_DetSlNo)
                        .Rows(n).Cells(13).Value = Val(Ent_Qty)
                        .Rows(n).Cells(14).Value = Val(Ent_Rate)
                        .Rows(n).Cells(15).Value = Val(Ent_PurcRet_Qty)

                        If Val(Ent_Qty) > 0 Then

                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    End If

                Next

            End If
            Dt1.Clear()

            If .Rows.Count = 0 Then
                n = .Rows.Add()
                .Rows(n).Cells(0).Value = "1"
            End If

        End With

        pnl_Selection.Visible = True
        pnl_Selection.BringToFront()
        pnl_Back.Enabled = False

        dgv_Selection.Focus()
        dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        dgv_Selection.CurrentCell.Selected = True

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Grid_Selection(e.RowIndex)
    End Sub

    Private Sub Grid_Selection(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                If Val(.Rows(RwIndx).Cells(6).Value) = 0 And Trim(.Rows(RwIndx).Cells(10).Value) = "" Then Exit Sub

                If Val(.Rows(RwIndx).Cells(15).Value) <> 0 Then
                    MessageBox.Show("Already some items returned, cannot de-select.", "DOES NOT DE-SELECT PO...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                .Rows(RwIndx).Cells(9).Value = (Val(.Rows(RwIndx).Cells(9).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(9).Value) = 0 Then

                    .Rows(RwIndx).Cells(9).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                    Next

                Else
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next

        With dgv_Selection

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                    e.Handled = True
                    Grid_Selection(dgv_Selection.CurrentCell.RowIndex)
                End If
            End If

        End With

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim i As Integer, n As Integer
        Dim sno As Integer
        Dim Ent_Qty As Single, Ent_Rate As Single

        dgv_Details.Rows.Clear()

        NoCalc_Status = True

        sno = 0

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(9).Value) = 1 Then

                If Val(dgv_Selection.Rows(i).Cells(13).Value) <> 0 Then
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(13).Value)

                Else
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(6).Value)

                End If

                If Val(dgv_Selection.Rows(i).Cells(14).Value) <> 0 Then
                    Ent_Rate = Val(dgv_Selection.Rows(i).Cells(14).Value)

                Else
                    Ent_Rate = Val(dgv_Selection.Rows(i).Cells(8).Value)

                End If

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(5).Value = Val(Ent_Qty)
                dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Details.Rows(n).Cells(7).Value = Val(Ent_Rate)
                dgv_Details.Rows(n).Cells(8).Value = Format(Val(Ent_Qty) * Val(Ent_Rate), "##########0.00")
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(12).Value
                dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(11).Value

            End If

        Next i

        NoCalc_Status = False
        TotalQuantity_Calculation()

        Grid_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        txt_BillNo.Focus()

        'If dgv_Details.Rows.Count > 0 Then
        '    dgv_Details.Focus()
        '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
        '    dgv_Details.CurrentCell.Selected = True

        'Else
        '    txt_DiscPerc.Focus()

        'End If

    End Sub

    Private Sub dgv_Selection_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Selection.LostFocus
        On Error Resume Next
        dgv_Selection.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_EntType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EntType.TextChanged
        If Trim(UCase(cbo_EntType.Text)) = "PO" Then
            dgv_Details.AllowUserToAddRows = False
        Else
            dgv_Details.AllowUserToAddRows = True
        End If
    End Sub
    Private Sub cbo_PurchaseAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PurchaseAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAc, cbo_Ledger, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAc, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PurchaseAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_PaymentMethod_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentMethod.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentMethod, cbo_TaxType, cbo_Ledger, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_PaymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMethod, cbo_Ledger, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_EntType, cbo_PaymentMethod, "", "", "", "")

    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, cbo_PaymentMethod, "", "", "", "")

    End Sub

    Private Sub cbo_TaxType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.LostFocus
        If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
            cbo_TaxType.Tag = cbo_TaxType.Text
            GST_Calculation()
        End If
    End Sub

    Private Sub cbo_TaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.SelectedIndexChanged
        GST_Calculation()
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


                        .Rows(RowIndx).Cells(14).Value = ""  'Cash Dis%
                        .Rows(RowIndx).Cells(15).Value = ""   'Cash Dis Amt
                        .Rows(RowIndx).Cells(16).Value = ""  ' Taxable value
                        .Rows(RowIndx).Cells(17).Value = ""  ' GST %
                        .Rows(RowIndx).Cells(18).Value = ""  ' HSN code

                        If Trim(.Rows(RowIndx).Cells(1).Value) <> "" Or Val(.Rows(RowIndx).Cells(4).Value) = 0 Or Val(.Rows(RowIndx).Cells(5).Value) = 0 Or Val(.Rows(RowIndx).Cells(7).Value) = 0 Then

                            HSN_Code = ""
                            GST_Per = 0
                            Get_GST_Percentage_From_ItemGroup(Trim(.Rows(RowIndx).Cells(3).Value), HSN_Code, GST_Per)

                            '--Cash discount
                            .Rows(RowIndx).Cells(14).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                            .Rows(RowIndx).Cells(15).Value = Format(Val(.Rows(RowIndx).Cells(14).Value) * (Val(.Rows(RowIndx).Cells(8).Value) / 100), "########0.00")


                            '-- Taxable value = amount - (trade disc + cash disc)
                            'Taxable_Amount = Val(.Rows(RowIndx).Cells(8).Value) - Val(.Rows(RowIndx).Cells(15).Value)
                            Taxable_Amount = Val(.Rows(RowIndx).Cells(22).Value) - Val(.Rows(RowIndx).Cells(15).Value)

                            .Rows(RowIndx).Cells(16).Value = Format(Val(Taxable_Amount), "##########0.00")
                            .Rows(RowIndx).Cells(17).Value = Format(Val(GST_Per), "########0.00")
                            .Rows(RowIndx).Cells(18).Value = Trim(HSN_Code)

                        End If

                    Next RowIndx

                    Get_HSN_CodeWise_Tax_Details()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DO GST CALCULATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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
        Dim AssVal_Pack_Frgt_Ins_Amt As String = ""
        Dim InterStateStatus As Boolean = False

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            If cbo_TaxType.Text = "GST" Then

                AssVal_Pack_Frgt_Ins_Amt = Format(Val(txt_AddLess.Text) + Val(txt_Freight.Text), "#########0.00")

                With dgv_Details

                    If .Rows.Count > 0 Then
                        For i = 0 To .Rows.Count - 1
                            If Trim(.Rows(i).Cells(1).Value) <> "" Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1                ,                  Currency1            ,                       Currency2                                             ) " &
                                                    "          Values     ( '" & Trim(.Rows(i).Cells(18).Value) & "', " & Val(.Rows(i).Cells(17).Value) & " ,  " & Str(Val(.Rows(i).Cells(16).Value) + Val(AssVal_Pack_Frgt_Ins_Amt)) & " ) "
                                cmd.ExecuteNonQuery()

                                AssVal_Pack_Frgt_Ins_Amt = 0

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

                        .Rows(n).Cells(2).Value = Format(Val(dt.Rows(i).Item("TaxableAmount").ToString), "############0.00")
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
        Dim TotCGST_amt As Single
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

        txt_AssessableValue.Text = Format(Val(TotAss_Val), "##########0.00")
        lbl_CGST_Amount.Text = IIf(Val(TotCGST_amt) <> 0, Format(Val(TotCGST_amt), "##########0.00"), "")
        lbl_SGST_Amount.Text = IIf(Val(TotSGST_amt) <> 0, Format(Val(TotSGST_amt), "##########0.00"), "")
        lbl_IGST_Amount.Text = IIf(Val(TotIGST_amt) <> 0, Format(Val(TotIGST_amt), "##########0.00"), "")

    End Sub

    Private Sub btn_Tax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Tax.Click
        pnl_Back.Enabled = False
        pnl_Tax.Visible = True
        pnl_Tax.Focus()
    End Sub

    Private Sub btn_Tax_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Tax_Close.Click
        pnl_Tax.Visible = False
        pnl_Back.Enabled = True

    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_Total_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details_Total.CellContentClick

    End Sub

    Private Sub txt_AddLess_TextChanged1(sender As Object, e As System.EventArgs) Handles txt_AddLess.TextChanged
        TotalQuantity_Calculation()
    End Sub

    Private Sub dgv_Details_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgv_Details.CellValidating

    End Sub
End Class