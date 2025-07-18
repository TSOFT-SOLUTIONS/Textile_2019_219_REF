Imports System.IO
Imports System.Drawing.Printing

Public Class Stores_Item_Purchase_BillMaking_GST

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "IPUBM-"           '--------"GITPU-"
    '  Private Pk_Condition1 As String = "GITPA-"
    ' Private PkCondition2_TDSSP As String = "TDSSP-"
    Private cbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private vdgv_DrawNo As String = ""
    Private vCbo_ItmNm As String = ""
    Private vCloPic_STS As Boolean = False
    Private NoCalc_Status As Boolean = False

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

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

        '   cbo_EntType.Text = "DIRECT"

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
        ' cbo_PaymentMethod.Text = "CREDIT"


        txt_Tcs_Name.Text = "TCS"
        txt_TcsPerc.Text = ""
        lbl_TcsAmount.Text = ""
        pnl_TotalSales_Amount.Visible = True
        txt_TCS_TaxableValue.Text = ""
        txt_TcsPerc.Enabled = False
        txt_TCS_TaxableValue.Enabled = False
        lbl_TotalSales_Amount_Current_Year.Text = "0.00"
        lbl_TotalSales_Amount_Previous_Year.Text = "0.00"
        chk_TCSAmount_RoundOff_STS.Checked = True

        lbl_Invoice_Value_Before_TCS.Text = ""
        lbl_RoundOff_Invoice_Value_Before_TCS.Text = ""
        chk_TCS_Tax.Checked = False

        lbl_BillAmount.Text = ""
        txt_TdsPerc.Text = ""
        lbl_TdsAmount.Text = ""
        txt_TDS_TaxableValue.Text = ""
        txt_TdsPerc.Enabled = False
        txt_TDS_TaxableValue.Enabled = False
        chk_TDS_Tax.Checked = False


        txt_GrnNo.Text = ""
        lbl_PurchaseCode.Text = ""

        cbo_Grid_Department.Text = False
        Cbo_Grid_MotionType.Text = False
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
        If Me.ActiveControl.Name <> Cbo_Grid_MotionType.Name Then
            Cbo_Grid_MotionType.Visible = False
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
        If Not IsNothing(dgv_Selection.CurrentCell) Then dgv_Selection.CurrentCell.Selected = False
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

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Grid_MotionType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MOTION" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Grid_MotionType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
        Dim dt7 As New DataTable

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

        da = New SqlClient.SqlDataAdapter("select Motion_Type_Name from Motion_Type_Head order by Motion_Type_Name", con)
        da.Fill(dt7)
        cbo_Grid_Department.DataSource = dt7
        cbo_Grid_Department.DisplayMember = "Motion_Type_Name"

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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1158" Then
            btn_SaveAll.Visible = True
        End If


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

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

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
        AddHandler Cbo_Grid_MotionType.GotFocus, AddressOf ControlGotFocus

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
        AddHandler Cbo_Grid_MotionType.LostFocus, AddressOf ControlLostFocus

        '   AddHandler dtp_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        ' AddHandler dtp_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_GrnNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GrnNo.LostFocus, AddressOf ControlLostFocus



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

                            If .CurrentCell.ColumnIndex >= 7 Then

                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    txt_DiscPerc.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

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
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Stores_Item_Purchase_BillMaking_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  Where a.Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Item_Purchase_BillMaking_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("Item_Purchase_BillMaking_Date").ToString
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

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")
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

                If Val(dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False
                txt_TCS_TaxableValue.Text = dt1.Rows(0).Item("TCS_Taxable_Value").ToString
                If Val(dt1.Rows(0).Item("EDIT_TCS_TaxableValue").ToString) = 1 Then
                    txt_TcsPerc.Enabled = True
                    txt_TCS_TaxableValue.Enabled = True
                End If
                If IsDBNull(dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = 1 Then chk_TCSAmount_RoundOff_STS.Checked = True Else chk_TCSAmount_RoundOff_STS.Checked = False
                End If
                txt_TcsPerc.Text = Val(dt1.Rows(0).Item("Tcs_Percentage").ToString)
                lbl_TcsAmount.Text = dt1.Rows(0).Item("TCS_Amount").ToString
                lbl_Invoice_Value_Before_TCS.Text = dt1.Rows(0).Item("Invoice_Value_Before_TCS").ToString
                lbl_RoundOff_Invoice_Value_Before_TCS.Text = dt1.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString

                If Val(dt1.Rows(0).Item("TDS_Tax_Status").ToString) = 1 Then chk_TDS_Tax.Checked = True Else chk_TDS_Tax.Checked = False
                txt_TDS_TaxableValue.Text = dt1.Rows(0).Item("TDS_Taxable_Value").ToString
                If Val(dt1.Rows(0).Item("EDIT_TDS_TaxableValue").ToString) = 1 Then
                    txt_TdsPerc.Enabled = True
                    txt_TDS_TaxableValue.Enabled = True
                End If
                txt_TdsPerc.Text = Val(dt1.Rows(0).Item("TDS_Percentage").ToString)
                lbl_TdsAmount.Text = dt1.Rows(0).Item("TDS_Amount").ToString

                lbl_BillAmount.Text = dt1.Rows(0).Item("Bill_Amount").ToString

                txt_GrnNo.Text = dt1.Rows(0).Item("Item_Purchase_No").ToString
                lbl_PurchaseCode.Text = dt1.Rows(0).Item("Item_Purchase_CODE").ToString


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, b.Drawing_No, c.Department_name, d.Unit_name, e.Brand_Name, f.Motion_Type_Name from Stores_Item_Purchase_BillMaking_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Department_Head c ON b.Department_idno = c.Department_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Brand_Head e ON a.Brand_idno = e.Brand_idno LEFT OUTER JOIN Motion_Type_Head F ON a.Motion_Type_IdNo = f.Motion_Type_IdNo  where a.Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
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
                        dgv_Details.Rows(n).Cells(10).Value = Val(dt2.Rows(i).Item("Purchase_BillMaking_Details_SlNo").ToString)
                        dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Po_Code").ToString
                        dgv_Details.Rows(n).Cells(12).Value = Val(dt2.Rows(i).Item("Po_Details_SlNo").ToString)
                        dgv_Details.Rows(n).Cells(13).Value = Val(dt2.Rows(i).Item("PurchaseReturn_Quantity").ToString)

                        dgv_Details.Rows(n).Cells(16).Value = Val(dt2.Rows(i).Item("Taxable_Value").ToString)
                        dgv_Details.Rows(n).Cells(17).Value = Val(dt2.Rows(i).Item("GST_Percentage").ToString)
                        dgv_Details.Rows(n).Cells(18).Value = Val(dt2.Rows(i).Item("HSN_Code").ToString)
                        dgv_Details.Rows(n).Cells(19).Value = dt2.Rows(i).Item("Catelog_pageno").ToString
                        dgv_Details.Rows(n).Cells(20).Value = dt2.Rows(i).Item("Motion_Type_Name").ToString

                        If Val(dgv_Details.Rows(n).Cells(13).Value) <> 0 Then
                            LockSTS = True

                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(i).Cells(j).Style.BackColor = Color.LightGray
                            Next

                        End If

                    Next i

                End If

                With dgv_Details_Total
                    .Rows.Clear()
                    .Rows.Add()
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")
                End With
                da4 = New SqlClient.SqlDataAdapter("Select a.* from Stores_Item_Purchase_BillMaking_GST_Tax_Details a Where a.Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)
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
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Stores_Purchase_Inward, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Stores_Purchase_Inward, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Store_purchase_Inward_entry, New_Entry, Me, con, "Stores_Item_Purchase_BillMaking_Head", "Item_Purchase_BillMaking_Code", NewCode, "Item_Purchase_BillMaking_Date", "(Item_Purchase_BillMaking_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


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

        Da = New SqlClient.SqlDataAdapter("select sum(PurchaseReturn_Quantity) from Stores_Item_Purchase_BillMaking_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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
        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Stores_Item_Purchase_BillMaking_Head", "Item_Purchase_BillMaking_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Item_Purchase_BillMaking_Code, Company_IdNo, for_OrderBy", trans)

        Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Stores_Item_Purchase_BillMaking_Details", "Item_Purchase_BillMaking_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Item_IdNo,Brand_IdNo,Quantity,Unit_idNo,Rate,Amount,Po_No,Po_Code,PO_Details_SlNo, PurchaseReturn_Quantity,Taxable_Value,GST_Percentage,HSN_Code", "Sl_No", "Item_Purchase_BillMaking_Code, For_OrderBy, Company_IdNo, Item_Purchase_BillMaking_No, Item_Purchase_BillMaking_Date, Ledger_Idno", trans)

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans


            'If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
            '    Throw New ApplicationException("Error on Voucher Bill Deletion")
            'End If

            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), trans)
            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition2_TDSSP) & Trim(NewCode), trans)


            'cmd.CommandText = "Update Stores_Item_PO_Details Set Purchased_Quantity = a.Purchased_Quantity - b.Quantity from Stores_Item_PO_Details a, Stores_Item_Purchase_BillMaking_Details b where b.Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and b.Entry_Type = 'PO' and a.Po_Code = b.Po_Code and a.PO_Details_SlNo = b.PO_Details_SlNo"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stores_Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "UPDATE Stores_Item_Purchase_Head SET Item_Purchase_BillMaking_Code='' WHERE Item_Purchase_BillMaking_Code='" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stores_Item_Purchase_BillMaking_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Stores_Item_Purchase_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()

            'If InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_1") > 0 Then
            '    MessageBox.Show("Invalid Purchase quantity", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_2") > 0 Then
            '    MessageBox.Show("Invalid Cancel quantity", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_3") > 0 Or InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_4") > 0 Then
            '    MessageBox.Show("Invalid Purchase Return quantity", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_5") > 0 Then
            '    MessageBox.Show("Invalid Return quantity, Lesser than PO Quantity", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_Purchase_BillMaking_Details_1") > 0 Then
            '    MessageBox.Show("Invalid Purchase Return quantity", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_Purchase_BillMaking_Details_2") > 0 Then
            '    MessageBox.Show("Invalid Return quantity, Lesser than Purchase Quantity", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'Else
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ' End If

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
            If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Store_purchase_Inward_entry, New_Entry, Me) = False Then Exit Sub

            ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Stores_Purchase_Inward, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Stores_Purchase_Inward, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

            inpno = InputBox("Enter New Ref No.", "FOR NEW NO INSERTION...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Item_Purchase_BillMaking_No from Stores_Item_Purchase_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code LIKE '" & Trim(Pk_Condition) & "%' AND Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(RefCode) & "'", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Item_Purchase_BillMaking_No from Stores_Item_Purchase_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code LIKE '" & Trim(Pk_Condition) & "%' AND Item_Purchase_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Item_Purchase_BillMaking_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Item_Purchase_BillMaking_No from Stores_Item_Purchase_BillMaking_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code LIKE '" & Trim(Pk_Condition) & "%' AND Item_Purchase_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Item_Purchase_BillMaking_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Item_Purchase_BillMaking_No from Stores_Item_Purchase_BillMaking_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Item_Purchase_BillMaking_Code LIKE '" & Trim(Pk_Condition) & "%' AND  Item_Purchase_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Item_Purchase_BillMaking_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Item_Purchase_BillMaking_No from Stores_Item_Purchase_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and   Item_Purchase_BillMaking_Code LIKE '" & Trim(Pk_Condition) & "%' AND Item_Purchase_BillMaking_Code like  '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Item_Purchase_BillMaking_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Stores_Item_Purchase_BillMaking_Head", "Item_Purchase_BillMaking_Code", "For_OrderBy", "Item_Purchase_BillMaking_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            da1 = New SqlClient.SqlDataAdapter("select Top 1 a.*, b.ledger_name as SalesAcName, c.ledger_name as TaxAcName from Stores_Item_Purchase_BillMaking_Head a LEFT OUTER JOIN Ledger_Head b ON a.PurchaseAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Item_Purchase_BillMaking_No desc", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                If dt1.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_PurchaseAc.Text = dt1.Rows(0).Item("SalesAcName").ToString
                If dt1.Rows(0).Item("Entry_Type").ToString <> "" Then cbo_EntType.Text = dt1.Rows(0).Item("Entry_Type").ToString
                'If Dt1.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_SalesAc.Text = Dt1.Rows(0).Item("SalesAcName").ToString
                'If Dt1.Rows(0).Item("TaxAcName").ToString <> "" Then cbo_VatAc.Text = Dt1.Rows(0).Item("TaxAcName").ToString


                If IsDBNull(dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False
                End If

                If IsDBNull(dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = 1 Then chk_TCSAmount_RoundOff_STS.Checked = True Else chk_TCSAmount_RoundOff_STS.Checked = False
                End If

                If IsDBNull(dt1.Rows(0).Item("Tds_Tax_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("Tds_Tax_Status").ToString) = 1 Then chk_TDS_Tax.Checked = True Else chk_TDS_Tax.Checked = False
                End If

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

            Da = New SqlClient.SqlDataAdapter("select Item_Purchase_BillMaking_No from Stores_Item_Purchase_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code LIKE '" & Trim(Pk_Condition) & "%' AND Item_Purchase_BillMaking_Code = '" & Trim(RefCode) & "'", con)
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
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_IdNo As Integer = 0
        Dim Dep_ID As Integer = 0
        Dim Item_ID As Integer = 0
        Dim Unit_ID As Integer = 0
        Dim MotTyp_Id As Integer = 0
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
        Dim vOrdByNo As String = ""
        Dim vTCS_AssVal_EditSTS As Integer = 0
        Dim vTCS_Tax_Sts As Integer = 0
        Dim vTCSAmtRndOff_STS As Integer = 0
        Dim vTDS_AssVal_EditSTS As Integer = 0
        Dim vTDS_Tax_Sts As Integer = 0
        Dim VPur_No As String = ""


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Stores_Purchase_Inward, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Store_purchase_Inward_entry, New_Entry, Me, con, "Stores_Item_Purchase_BillMaking_Head", "Item_Purchase_BillMaking_Code", NewCode, "Item_Purchase_BillMaking_Date", "(Item_Purchase_BillMaking_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Item_Purchase_BillMaking_No desc", dtp_date.Value.Date) = False Then Exit Sub

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

        If Val(lbl_NetAmount.Text) = 0 Then lbl_NetAmount.Text = 0

        PurcAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAc.Text)
        If PurcAc_ID = 0 And Val(CDbl(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PurchaseAc.Enabled And cbo_PurchaseAc.Visible Then cbo_PurchaseAc.Focus()
            Exit Sub
        End If

        If Trim(txt_BillNo.Text) = "" Then
            MessageBox.Show("Invalid BillNo", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
            Exit Sub
        End If

        If Val(lbl_TcsAmount.Text) <> 0 And Val(lbl_TdsAmount.Text) <> 0 Then
            MessageBox.Show("Invalid TCS/TDS Amount" & Chr(13) & "Bothe TCS and TDS cannot done at same time", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If chk_TCS_Tax.Enabled And chk_TCS_Tax.Visible Then
                chk_TCS_Tax.Focus()
            ElseIf chk_TDS_Tax.Enabled And chk_TDS_Tax.Visible Then
                chk_TDS_Tax.Focus()
            End If
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from Stores_Item_Purchase_BillMaking_Head where Ledger_IdNo = " & Str(Val(Led_IdNo)) & " and Bill_No = '" & Trim(txt_BillNo.Text) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Item_Purchase_BillMaking_Code <> '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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

        vTCS_Tax_Sts = 0
        If chk_TCS_Tax.Checked = True Then vTCS_Tax_Sts = 1
        vTCS_AssVal_EditSTS = 0
        If txt_TCS_TaxableValue.Enabled = True Then vTCS_AssVal_EditSTS = 1
        vTCSAmtRndOff_STS = 0
        If chk_TCSAmount_RoundOff_STS.Checked = True Then vTCSAmtRndOff_STS = 1


        vTDS_Tax_Sts = 0
        If chk_TDS_Tax.Checked = True Then vTDS_Tax_Sts = 1
        vTDS_AssVal_EditSTS = 0
        If txt_TDS_TaxableValue.Enabled = True Then vTDS_AssVal_EditSTS = 1

        NoCalc_Status = False
        TotalQuantity_Calculation()

        vTotQty = 0 : vTotAmt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotQty = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
        End If

        If vTotQty = 0 Then
            MessageBox.Show("Invalid Purchase Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dgv_Details.Enabled And dgv_Details.Visible Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            End If
            Exit Sub
        End If


        If Trim(lbl_PurchaseCode.Text) <> "" Then

            If Val(Led_IdNo) <> 0 Then

                Da2 = New SqlClient.SqlDataAdapter("SELECT A.* FROM Stores_Item_Purchase_Head A WHERE Item_Purchase_Code='" & Trim(lbl_PurchaseCode.Text) & "' AND  a.Ledger_idno = " & Str(Val(Led_IdNo)) & " ", con)
                Dt2 = New DataTable
                Da2.Fill(Dt2)

                If Dt2.Rows.Count > 0 Then
                    VPur_No = Dt2.Rows(0).Item("Item_Purchase_No").ToString
                End If
            End If


            If Trim(VPur_No) <> Trim(txt_GrnNo.Text) Then
                MessageBox.Show("Mismatch Of Our Purchase No for this Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
                Exit Sub
            End If

        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Stores_Item_Purchase_BillMaking_Head", "Item_Purchase_BillMaking_Code", "For_OrderBy", "Item_Purchase_BillMaking_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PurcDate", dtp_date.Value.Date)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Stores_Item_Purchase_BillMaking_Head(               Item_Purchase_BillMaking_Code,             Company_IdNo         ,           Item_Purchase_BillMaking_No   ,                  for_OrderBy                             ,      Item_Purchase_BillMaking_Date   ,       Entry_Type              ,           Ledger_IdNo     ,  Cash_PartyName           ,             Bill_No          ,           PurchaseAc_IdNo  ,        Total_Quantity    ,       Total_Amount       ,      CashDiscount_Percentage       ,       CashDiscount_Amount            ,              TaxAc_IdNo    ,                   Freight_Amount       ,             AddLess_Amount       ,             RoundOff_Amount         ,                   Net_Amount             ,               Remarks       ,               Payment_Method              , Assessable_Value                       ,Total_CGST_Amount                 ,Total_SGST_Amount                ,Total_IGST_Amount                  ,     GST_Tax_Type            , Tcs_Name_caption ,             Tcs_percentage         ,                    Tcs_Amount           ,                     TCS_Taxable_Value,                            EDIT_TCS_TaxableValue ,             Tcs_Tax_Status,             TCSAmount_RoundOff_Status,                         Invoice_Value_Before_TCS ,                            RoundOff_Invoice_Value_Before_TCS          ,              TDS_Tax_Status   ,              EDIT_TDS_TaxableValue     ,                  TDS_Taxable_Value          ,              TDS_Percentage       ,                TDS_Amount           ,                Bill_Amount                  ,    Item_Purchase_No            ,           Item_Purchase_Code ) " &
                                    "                                         Values   ('" & Trim(Pk_Condition) & Trim(NewCode) & "',     " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'         , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @PurcDate     , '" & Trim(cbo_EntType.Text) & "', " & Str(Val(Led_IdNo)) & ", '" & Trim(CsParNm) & "',  '" & Trim(txt_BillNo.Text) & "', " & Str(Val(PurcAc_ID)) & ", " & Str(Val(vTotQty)) & ", " & Str(Val(vTotAmt)) & ", " & Str(Val(txt_DiscPerc.Text)) & ", " & Str(Val(lbl_DiscAmount.Text)) & ", " & Str(Val(TxAc_ID)) & ",   " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CDbl(lbl_NetAmount.Text))) & ", '" & Trim(txt_Remarks.Text) & "' , '" & Trim(cbo_PaymentMethod.Text) & "' ," & Val(txt_AssessableValue.Text) & " ," & Val(lbl_CGST_Amount.Text) & " ," & Val(lbl_SGST_Amount.Text) & "," & Val(lbl_IGST_Amount.Text) & " ,'" & Trim(cbo_TaxType.Text) & "' ,       'TCS'      ,  " & Str(Val(txt_TcsPerc.Text)) & ",    " & Str(Val(lbl_TcsAmount.Text)) & " ,  " & Str(Val(txt_TCS_TaxableValue.Text)) & ", " & Str(Val(vTCS_AssVal_EditSTS)) & ", " & Str(Val(vTCS_Tax_Sts)) & ", " & Str(Val(vTCSAmtRndOff_STS)) & ", " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & " ,             " & Str(Val(vTDS_Tax_Sts)) & ",  " & Str(Val(vTDS_AssVal_EditSTS)) & " ,  " & Str(Val(txt_TDS_TaxableValue.Text)) & ", " & Str(Val(txt_TdsPerc.Text)) & ", " & Str(Val(lbl_TdsAmount.Text)) & ", " & Str(Val(lbl_BillAmount.Text)) & "  , '" & Trim(txt_GrnNo.Text) & "' ,'" & Trim(lbl_PurchaseCode.Text) & "' )"
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Stores_Item_Purchase_BillMaking_Head", "Item_Purchase_BillMaking_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Item_Purchase_BillMaking_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Stores_Item_Purchase_BillMaking_Details", "Item_Purchase_BillMaking_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Item_IdNo,Brand_IdNo,Quantity,Unit_idNo,Rate,Amount,Po_No,Po_Code,PO_Details_SlNo, PurchaseReturn_Quantity,Taxable_Value,GST_Percentage,HSN_Code", "Sl_No", "Item_Purchase_BillMaking_Code, For_OrderBy, Company_IdNo, Item_Purchase_BillMaking_No, Item_Purchase_BillMaking_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Stores_Item_Purchase_BillMaking_Head set Item_Purchase_BillMaking_Date= @PurcDate, Entry_Type = '" & Trim(cbo_EntType.Text) & "', Ledger_IdNo = " & Str(Val(Led_IdNo)) & ", Bill_No = '" & Trim(txt_BillNo.Text) & "', PurchaseAc_IdNo = " & Str(Val(PurcAc_ID)) & ", Total_Quantity = " & Str(Val(vTotQty)) & ", Total_Amount = " & Str(Val(vTotAmt)) & ", CashDiscount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(lbl_DiscAmount.Text)) & ",  TaxAc_IdNo = " & Str(Val(TxAc_ID)) & ", GST_Tax_Type = 'GST', Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ",Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Cash_PartyName = '" & Trim(CsParNm) & "',  RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CDbl(lbl_NetAmount.Text))) & ", Remarks = '" & Trim(txt_Remarks.Text) & "',Assessable_Value = " & Val(txt_AssessableValue.Text) & " , Total_CGST_Amount = " & Val(lbl_CGST_Amount.Text) & " ,Total_SGST_Amount = " & Val(lbl_SGST_Amount.Text) & ",Total_IGST_Amount =" & Val(lbl_IGST_Amount.Text) & "  ,  Tcs_Name_caption = 'TCS', Tcs_percentage=" & Str(Val(txt_TcsPerc.Text)) & ",Tcs_Amount= " & Str(Val(lbl_TcsAmount.Text)) & " , TCS_Taxable_Value = " & Str(Val(txt_TCS_TaxableValue.Text)) & ", EDIT_TCS_TaxableValue = " & Str(Val(vTCS_AssVal_EditSTS)) & " , Tcs_Tax_Status = " & Str(Val(vTCS_Tax_Sts)) & " , TCSAmount_RoundOff_Status = " & Str(Val(vTCSAmtRndOff_STS)) & " , Invoice_Value_Before_TCS = " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", RoundOff_Invoice_Value_Before_TCS = " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & "  , TDS_Tax_Status = " & Str(Val(vTDS_Tax_Sts)) & ", EDIT_TDS_TaxableValue = " & Str(Val(vTDS_AssVal_EditSTS)) & " , TDS_Taxable_Value = " & Str(Val(txt_TDS_TaxableValue.Text)) & ", TDS_Percentage = " & Str(Val(txt_TdsPerc.Text)) & ",  TDS_Amount = " & Str(Val(lbl_TdsAmount.Text)) & " , Bill_Amount = " & Str(Val(lbl_BillAmount.Text)) & " ,  Item_Purchase_No  = '" & Trim(txt_GrnNo.Text) & "' , Item_Purchase_Code ='" & Trim(lbl_PurchaseCode.Text) & "'   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stores_Item_Purchase_Head set Item_Purchase_BillMaking_Code = '' Where Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


                'cmd.CommandText = "Update Stores_Item_PO_Details Set Purchased_Quantity = a.Purchased_Quantity - b.Quantity from Stores_Item_PO_Details a, Stores_Item_Purchase_BillMaking_Details b where b.Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and b.Entry_Type = 'PO' and a.Po_Code = b.Po_Code and a.PO_Details_SlNo = b.PO_Details_SlNo"
                'cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Stores_Item_Purchase_BillMaking_Head", "Item_Purchase_BillMaking_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Item_Purchase_BillMaking_Code, Company_IdNo, for_OrderBy", tr)


            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(txt_BillNo.Text)
            Partcls = "ITPurcBillMak : Ref.No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Stores_Item_Purchase_BillMaking_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stores_Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()




            If Trim(lbl_PurchaseCode.Text) <> "" Then
                Nr = 0
                cmd.CommandText = "UPDATE Stores_Item_Purchase_Head SET Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' WHERE Item_Purchase_Code='" & Trim(lbl_PurchaseCode.Text) & "'   AND  Ledger_IdNo =" & Str(Val(Led_IdNo)) & " "
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    Throw New ApplicationException("Mismatch of Purchase Bill and Party Details")
                End If
            End If


            With dgv_Details
                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        Item_ID = Common_Procedures.itemalais_NameToIdNo(con, dgv_Details.Rows(i).Cells(3).Value, tr)

                        Brand_ID = Common_Procedures.Brand_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value, tr)

                        Unit_ID = Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(6).Value, tr)

                        MotTyp_Id = Common_Procedures.Motion_Type_NameToIdNo(con, dgv_Details.Rows(i).Cells(20).Value, tr)


                        cmd.CommandText = "Update Stores_Item_Purchase_BillMaking_Details set Item_Purchase_BillMaking_Date= @PurcDate, Entry_Type = '" & Trim(cbo_EntType.Text) & "', Ledger_IdNo = " & Str(Val(Led_IdNo)) & ", Bill_No = '" & Trim(txt_BillNo.Text) & "', Sl_No = " & Str(Val(Sno)) & ", Item_Idno = " & Str(Val(Item_ID)) & ", Brand_IdNo = " & Str(Val(Brand_ID)) & ", Quantity = " & Str(Val(.Rows(i).Cells(5).Value)) & ", Unit_Idno = " & Val(Unit_ID) & ", Rate = " & Str(Val(.Rows(i).Cells(7).Value)) & ", Amount = " & Str(Val(.Rows(i).Cells(8).Value)) & ", Po_No = '" & Trim(.Rows(i).Cells(9).Value) & "', Po_Code = '" & Trim(.Rows(i).Cells(11).Value) & "', PO_Details_SlNo = " & Str(Val(.Rows(i).Cells(12).Value)) & ",Taxable_Value=" & Str(Val(.Rows(i).Cells(16).Value)) & ", GST_Percentage=" & Str(Val(.Rows(i).Cells(17).Value)) & ",HSN_Code=" & Str(Val(.Rows(i).Cells(18).Value)) & ",Catelog_pageno= '" & Trim(.Rows(i).Cells(19).Value) & "',  Motion_Type_IdNo = " & Val(MotTyp_Id) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Purchase_BillMaking_Details_SlNo = " & Str(Val(.Rows(i).Cells(10).Value))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Stores_Item_Purchase_BillMaking_Details (   Item_Purchase_BillMaking_Code      ,           Company_IdNo            ,       Item_Purchase_BillMaking_No        ,                       for_OrderBy                                     ,Item_Purchase_BillMaking_Date,        Entry_Type          ,           Ledger_IdNo         ,           Bill_No                 ,   Sl_No           ,           Item_IdNo       ,           Brand_IdNo          ,           Quantity                ,       Unit_idNo       ,                   Rate                    ,               Amount                  ,                   Po_No                   ,               Po_Code                     ,               PO_Details_SlNo         , PurchaseReturn_Quantity   ,               Taxable_Value               ,           GST_Percentage                  ,           HSN_Code    , Catelog_pageno  , Motion_Type_IdNo   ) " &
                                                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @PurcDate    , '" & Trim(cbo_EntType.Text) & "', " & Str(Val(Led_IdNo)) & ", '" & Trim(txt_BillNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(Item_ID)) & ", " & Str(Val(Brand_ID)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Val(Unit_ID) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", '" & Trim(.Rows(i).Cells(9).Value) & "', '" & Trim(.Rows(i).Cells(11).Value) & "', " & Str(Val(.Rows(i).Cells(12).Value)) & ",               0          ," & Str(Val(.Rows(i).Cells(16).Value)) & "," & Str(Val(.Rows(i).Cells(17).Value)) & ",'" & Trim(.Rows(i).Cells(18).Value) & "', '" & Trim(.Rows(i).Cells(19).Value) & "', " & Val(MotTyp_Id) & ")"
                            cmd.ExecuteNonQuery()
                        End If

                        'If Trim(UCase(cbo_EntType.Text)) = "PO" Then

                        '    cmd.CommandText = "Update Stores_Item_PO_Details Set Purchased_Quantity = Purchased_Quantity + " & Str(Val(.Rows(i).Cells(5).Value)) & " where Po_Code = '" & Trim(.Rows(i).Cells(11).Value) & "' and PO_Details_SlNo = " & Str(Val(.Rows(i).Cells(12).Value)) & " and Ledger_IdNo = " & Str(Val(Led_IdNo))
                        '    Nr = cmd.ExecuteNonQuery()

                        '    If Nr = 0 Then
                        '        tr.Rollback()
                        '        MessageBox.Show("Mismatch of PO and Party details", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        '        If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
                        '        Exit Sub
                        '    End If

                        'End If

                        'cmd.CommandText = "Insert into Stores_Stock_Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, Entry_ID, Party_Bill_No, Particulars, Sl_No, Item_IdNo, Unit_IdNo, Brand_IdNo, Quantity_New, Quantity_Old_Usable, Quantity_Old_Scrap) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PurcDate, " & Str(Val(Led_IdNo)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Item_ID)) & ", " & Str(Val(Unit_ID)) & ", " & Str(Val(Brand_ID)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", 0, 0 )"
                        'cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Stores_Item_Purchase_BillMaking_Details", "Item_Purchase_BillMaking_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Item_IdNo,Brand_IdNo,Quantity,Unit_idNo,Rate,Amount,Po_No,Po_Code,PO_Details_SlNo, PurchaseReturn_Quantity,Taxable_Value,GST_Percentage,HSN_Code", "Sl_No", "Item_Purchase_BillMaking_Code, For_OrderBy, Company_IdNo, Item_Purchase_BillMaking_No, Item_Purchase_BillMaking_Date, Ledger_Idno", tr)

            End With
            '---Tax Details
            cmd.CommandText = "Delete from Stores_Item_Purchase_BillMaking_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Tax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Stores_Item_Purchase_BillMaking_GST_Tax_Details   ( Item_Purchase_BillMaking_Code  ,               Company_IdNo       ,      Item_Purchase_BillMaking_No    ,                               for_OrderBy                              , Item_Purchase_BillMaking_Date  ,         Ledger_IdNo     ,            Sl_No     , HSN_Code                               ,Taxable_Amount                            ,CGST_Percentage                           ,CGST_Amount                               ,SGST_Percentage                            ,SGST_Amount                              ,IGST_Percentage                          ,IGST_Amount ) " &
                                                "     Values                                  (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @PurcDate     , " & Str(Val(Led_IdNo)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & "  ," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & ") "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With


            'cmd.CommandText = "truncate table Entry_GST_Tax_Details_Temp"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "insert into Entry_GST_Tax_Details_Temp(GST_Percentage, CGST_Amount, SGST_Amount, IGST_Amount) select (CGST_Percentage + SGST_Percentage + IGST_Percentage), sum(CGST_Amount), sum(SGST_Amount), sum(IGST_Amount) from Stores_Item_Purchase_BillMaking_GST_Tax_Details where Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (CGST_Percentage <> 0 or SGST_Percentage <> 0 or IGST_Percentage <> 0) and (CGST_Amount <> 0 or SGST_Amount <> 0 or IGST_Amount <> 0) Group by CGST_Percentage,  SGST_Percentage,  IGST_Percentage Having sum(CGST_Amount) <> 0 or sum(SGST_Amount) <> 0 or sum(IGST_Amount) <> 0"
            'cmd.ExecuteNonQuery()

            'Dim vVOUPOS_GSTAC_IDNOS As String = "", vVOUPOS_GST_AMTS As String = ""

            'Common_Procedures.get_GST_AC_IDNOS_for_AC_POSTING(con, "DR", vVOUPOS_GSTAC_IDNOS, vVOUPOS_GST_AMTS, tr)


            'Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            'Dim vAssVal As String = ""

            'vAssVal = Format(Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_Freight.Text), "##########0.00")
            'vLed_IdNos = Led_IdNo & "|" & PurcAc_ID & "|" & Trim(vVOUPOS_GSTAC_IDNOS) & "|" & Common_Procedures.CommonLedger.ADDLESS_AMOUNT_AC & "|" & Common_Procedures.CommonLedger.TCS_PAYABLE_AC & "|" & Common_Procedures.CommonLedger.ROUNDOFF_AC

            'vVou_Amts = Val(lbl_BillAmount.Text) & "|" & -1 * Val(vAssVal) & "|" & Trim(vVOUPOS_GST_AMTS) & "|" & -1 * Val(txt_AddLess.Text) & "|" & -1 * Val(lbl_TcsAmount.Text) & "|" & -1 * (Val(lbl_RoundOff.Text) + Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text))

            'If Common_Procedures.Voucher_Updation(con, "Gst.Store.ItemPurc", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(dtp_date.Text), "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
            '    Throw New ApplicationException(ErrMsg)
            'End If


            'Dim VouBil As String = ""
            'VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_date.Text, Led_IdNo, Trim(txt_BillNo.Text), 0, Val(CDbl(lbl_NetAmount.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr,, SaveAll_STS)
            'If Trim(UCase(VouBil)) = "ERROR" Then
            '    Throw New ApplicationException("Error on Voucher Bill Posting")
            'End If

            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), tr)
            'If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Led_IdNo <> 1 Then

            '    vLed_IdNos = "" : vVou_Amts = "" : ErrMsg = ""
            '    If Val(CDbl(lbl_NetAmount.Text)) <> 0 Then
            '        vLed_IdNos = 1 & "|" & Led_IdNo
            '        vVou_Amts = Val(CSng(lbl_NetAmount.Text)) & "|" & -1 * (Val(CSng(lbl_NetAmount.Text)))
            '        If Common_Procedures.Voucher_Updation(con, "Gst.Store.ItemPayment", Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(dtp_date.Text), "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
            '            Throw New ApplicationException(ErrMsg)
            '        End If
            '    End If
            'End If



            'vLed_IdNos = ""
            'vVou_Amts = ""
            'ErrMsg = ""
            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition2_TDSSP) & Trim(NewCode), tr)

            'vLed_IdNos = Val(Common_Procedures.CommonLedger.TDS_Payable_Ac) & "|" & Led_IdNo
            'vVou_Amts = Val(lbl_TdsAmount.Text) & "|" & -1 * Val(lbl_TdsAmount.Text)

            'If Common_Procedures.Voucher_Updation(con, "StorePurc.Tds", Val(lbl_Company.Tag), Trim(PkCondition2_TDSSP) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(dtp_date.Text), "Bill No : " & Trim(txt_BillNo.Text) & " , StorePurc.No : " & Trim(lbl_RefNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
            '    Throw New ApplicationException(ErrMsg)
            'End If


            tr.Commit()

            move_record(lbl_RefNo.Text)

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If



        Catch ex As Exception

            tr.Rollback()

            Timer1.Enabled = False
            SaveAll_STS = False

            'If InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_1") > 0 Then
            '    MessageBox.Show("Invalid Purchase quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_2") > 0 Then
            '    MessageBox.Show("Invalid Cancel quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_3") > 0 Or InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_4") > 0 Then
            '    MessageBox.Show("Invalid Purchase Return quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_PO_Details_5") > 0 Then
            '    MessageBox.Show("Invalid Purchase quantity, Greater than PO Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_Purchase_BillMaking_Details_1") > 0 Then
            '    MessageBox.Show("Invalid Purchase Return quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'ElseIf InStr(1, LCase(ex.Message), "ck_Stores_Item_Purchase_BillMaking_Details_2") > 0 Then
            '    MessageBox.Show("Invalid Purchase quantity, Lesser than Return Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'Else
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'End If

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
                    .CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index - 1).Cells(.ColumnCount - 5)

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6) ) ", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If MessageBox.Show("Do you want to select Purchase Bill:", "FOR PURCHASE BILL SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                dtp_date.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6) ) ", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            '  If Trim(UCase(cbo_EntType.Text)) = "PO" Then

            If MessageBox.Show("Do you want to select Purchase Bill", "FOR PURCHASE BILL SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then

                btn_Selection_Click(sender, e)

            Else
                dtp_date.Focus()

                End If

                'Else
                '    dtp_date.Focus()

                'End If

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

            If e.ColumnIndex = 20 And Trim(UCase(cbo_EntType.Text)) <> "PO" Then

                If Cbo_Grid_MotionType.Visible = False Or Val(Cbo_Grid_MotionType.Tag) <> e.RowIndex Then

                    Cbo_Grid_MotionType.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Motion_Type_Name from Motion_Type_Head order by Motion_Type_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Grid_MotionType.DataSource = Dt1
                    Cbo_Grid_MotionType.DisplayMember = "Motion_Type_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_MotionType.Left = .Left + rect.Left
                    Cbo_Grid_MotionType.Top = .Top + rect.Top

                    Cbo_Grid_MotionType.Width = rect.Width
                    Cbo_Grid_MotionType.Height = rect.Height
                    Cbo_Grid_MotionType.Text = .CurrentCell.Value

                    Cbo_Grid_MotionType.Tag = Val(e.RowIndex)
                    Cbo_Grid_MotionType.Visible = True

                    Cbo_Grid_MotionType.BringToFront()
                    Cbo_Grid_MotionType.Focus()

                End If

            Else
                Cbo_Grid_MotionType.Visible = False

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

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If e.ColumnIndex = 5 Or e.ColumnIndex = 7 Then

                    .CurrentRow.Cells(8).Value = Format(Val(.CurrentRow.Cells(5).Value) * Val(.CurrentRow.Cells(7).Value), "#########0.00")

                    TotalQuantity_Calculation()

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

                ElseIf .CurrentCell.ColumnIndex = 7 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Or Trim(UCase(cbo_EntType.Text)) = "PO" Or Val(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> 0 Then
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
        Dim MotTyp_nm As String
        Dim Dep_nm As String
        Dim New_Rate As Double = 0
        Dim dep_idno As Integer = 0
        Dim Itm_idno As Integer = 0
        Dim Brand_idno As Integer = 0
        Dim MotTyp_Id As Integer = 0
        Dim Condt As String
        Dim catelog_pgno As String

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Grid_Department.Text))
        MotTyp_Id = Common_Procedures.Motion_Type_NameToIdNo(con, Trim(Cbo_Grid_MotionType.Text))


        Condt = ""
        If dep_idno <> 0 And dep_idno <> 1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Item, Nothing, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(11).Value) = "" Or Trim(UCase(vCbo_ItmNm)) <> Trim(UCase(cbo_Grid_Item.Text)) Then

                Itm_idno = Common_Procedures.itemalais_NameToIdNo(con, Trim(cbo_Grid_Item.Text))

                da = New SqlClient.SqlDataAdapter("select a.Drawing_No, b.unit_name, c.department_name , a.rate, d.Motion_Type_Name, a.Catelog_pageno from Stores_Item_Head a left outer join unit_head b on a.unit_idno = b.unit_idno left outer join Department_Head c ON a.Department_IdNo = c.Department_IdNo  left outer join Motion_Type_Head d ON a.Motion_Type_IdNo = d.Motion_Type_IdNo Where a.item_IdNo = " & Str(Val(Itm_idno)), con)
                da.Fill(dt)

                Dep_nm = ""
                dno_nm = ""
                Unt_nm = ""
                MotTyp_nm = ""
                New_Rate = 0
                catelog_pgno = ""

                Brand_idno = Common_Procedures.Brand_NameToIdNo(con, Trim(cbo_Grid_Brand.Text))

                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        Dep_nm = Trim(dt.Rows(0).Item("department_name").ToString)
                        dno_nm = Trim(dt.Rows(0).Item("Drawing_No").ToString)
                        Unt_nm = Trim(dt.Rows(0).Item("unit_name").ToString)
                        MotTyp_nm = Trim(dt.Rows(0).Item("Motion_Type_Name").ToString)
                        catelog_pgno = Trim(dt.Rows(0).Item("Catelog_pageno").ToString)

                        If Trim(Common_Procedures.settings.CustomerCode) <> "1158" Then
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

                End If

                dt.Dispose()
                da.Dispose()


                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value = Trim(Dep_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value = Trim(dno_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6).Value = Trim(Unt_nm)

                If Trim(Common_Procedures.settings.CustomerCode) <> "1158" Then
                    dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7).Value = Format(Val(New_Rate), "#########0.00")
                End If
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(19).Value = Trim(catelog_pgno)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(20).Value = Trim(MotTyp_nm)

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
        Dim Led_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Item_Purchase_BillMaking_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Item_Purchase_BillMaking_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Item_Purchase_BillMaking_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Ledger.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_Ledger.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Stores_Item_Purchase_BillMaking_Head a left outer join ledger_head b on a.Ledger_IdNo = b.Ledger_IdNo Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_BillMaking_Code LIKE '" & Trim(Pk_Condition) & "%' and a.Item_Purchase_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Item_Purchase_BillMaking_Date, a.for_orderby, a.Item_Purchase_BillMaking_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Purchase_BillMaking_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Item_Purchase_BillMaking_Date").ToString), "dd-MM-yyyy")
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



        If e.KeyValue = 38 Then
            e.Handled = True
            btn_Cancel.Focus()
        End If

        If e.KeyValue = 40 Then
            cbo_Ledger.Focus()

        End If

    End Sub

    Private Sub dtp_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_date.KeyPress

        If Asc(e.KeyChar) = 13 Then
            cbo_Ledger.Focus()
        End If

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
        Dim TotQty As String
        Dim TotAmt As String
        Dim Ttl_Tax_Amount As String
        Dim Ttl_CashDisc As String
        Dim Ttl_Taxable_Amount As String

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        Sno = 0

        TotQty = 0 : TotAmt = 0 : Ttl_Tax_Amount = 0 : Ttl_CashDisc = 0 : Ttl_Taxable_Amount = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(5).Value) <> 0 Then
                    TotQty = Val(TotQty) + Val(.Rows(i).Cells(5).Value)
                    TotAmt = Val(TotAmt) + Val(.Rows(i).Cells(8).Value)
                    Ttl_CashDisc = Val(Ttl_CashDisc) + Val(.Rows(i).Cells(15).Value())
                    Ttl_Taxable_Amount = Val(Ttl_Taxable_Amount) + Val(.Rows(i).Cells(16).Value())

                End If
            Next
        End With

        lbl_GrossAmount.Text = Format(Val(TotAmt), "########0.00")
        txt_AssessableValue.Text = Format(Val(TotAmt), "########0.00")

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(TotQty)
            .Rows(0).Cells(8).Value = Format(Val(TotAmt), "########0.00")
            .Rows(0).Cells(15).Value = Format(Val(Ttl_CashDisc), "########0.00")
            .Rows(0).Cells(16).Value = Format(Val(Ttl_Taxable_Amount), "########0.00")
        End With
        lbl_DiscAmount.Text = Format(Val(Ttl_CashDisc), "########0.00")

        GST_Calculation()
        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim GrsAmt As Single
        Dim NtAmt As Single
        Dim GST_Amt As Single = 0

        Dim vTCS_AssVal As String = 0
        Dim vTOT_SalAmt As String = 0
        Dim vTCS_Amt As String = 0
        Dim vInvAmt_Bfr_TCS As String = 0
        Dim vBlAmt As String = 0
        Dim vTDS_AssVal As String = 0
        Dim vTDS_Amt As String = 0

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        GrsAmt = 0

        With dgv_Details_Total
            If .Rows.Count > 0 Then
                GrsAmt = Val(.Rows(0).Cells(8).Value)
            End If
        End With

        'If Val(txt_DiscPerc.Text) <> 0 Then
        lbl_DiscAmount.Text = Format(Val(GrsAmt) * Val(txt_DiscPerc.Text) / 100, "########0.00")
        'End If
        GST_Amt = Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)




        Dim vTCS_StartDate As Date = #9/30/2020#
        Dim vMIN_TCS_assval As String = "5000000"

        If chk_TCS_Tax.Checked = True Then

            If DateDiff("d", vTCS_StartDate.Date, dtp_date.Value.Date) > 0 Then

                If txt_TCS_TaxableValue.Enabled = False Then

                    vTOT_SalAmt = Format(Val(txt_AssessableValue.Text) + Val(GST_Amt), "###########0")

                    vTCS_AssVal = 0

                    If Val(CDbl(lbl_TotalSales_Amount_Previous_Year.Text)) > Val(vMIN_TCS_assval) Then

                        vTCS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                    ElseIf Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) > Val(vMIN_TCS_assval) Then
                        vTCS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                    ElseIf (Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt)) > Val(vMIN_TCS_assval) Then
                        vTCS_AssVal = Format(Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt) - Val(vMIN_TCS_assval), "############0")

                    End If
                    txt_TCS_TaxableValue.Text = Format(Val(vTCS_AssVal), "############0.00")

                    If Val(txt_TCS_TaxableValue.Text) > 0 Then
                        If Val(txt_TcsPerc.Text) = 0 Then
                            txt_TcsPerc.Text = "0.1"
                        End If
                    End If

                End If

                vTCS_Amt = Format(Val(txt_TCS_TaxableValue.Text) * Val(txt_TcsPerc.Text) / 100, "##########0.00")
                If chk_TCSAmount_RoundOff_STS.Checked = True Then
                    vTCS_Amt = Format(Val(vTCS_Amt), "##########0")
                End If
                lbl_TcsAmount.Text = Format(Val(vTCS_Amt), "##########0.00")

            Else

                txt_TCS_TaxableValue.Text = ""
                txt_TcsPerc.Text = ""
                lbl_TcsAmount.Text = ""

            End If

        Else

            txt_TCS_TaxableValue.Text = ""
            txt_TcsPerc.Text = ""
            lbl_TcsAmount.Text = ""

        End If


        vInvAmt_Bfr_TCS = Format(Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_Freight.Text) + Val(GST_Amt), "###########0.00")
        lbl_Invoice_Value_Before_TCS.Text = Format(Val(vInvAmt_Bfr_TCS), "###########0")
        lbl_RoundOff_Invoice_Value_Before_TCS.Text = Format(Val(lbl_Invoice_Value_Before_TCS.Text) - Val(vInvAmt_Bfr_TCS), "###########0.00")



        vBlAmt = Format(Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_Freight.Text) + Val(GST_Amt) + Val(lbl_TcsAmount.Text) + Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text), "###########0.00")

        lbl_BillAmount.Text = Format(Val(vBlAmt), "##########0")
        lbl_BillAmount.Text = Format(Val(lbl_BillAmount.Text), "##########0.00")

        lbl_RoundOff.Text = Format(Val(lbl_BillAmount.Text) - Val(vBlAmt), "#########0.00")
        If Val(lbl_RoundOff.Text) = 0 Then lbl_RoundOff.Text = ""


        Dim vTDS_StartDate As Date = #6/30/2021#

        If chk_TDS_Tax.Checked = True Then

            If DateDiff("d", vTDS_StartDate.Date, dtp_date.Value.Date) > 0 Then

                If txt_TDS_TaxableValue.Enabled = False Then

                    vTOT_SalAmt = Format(Val(txt_AssessableValue.Text), "###########0")

                    vTDS_AssVal = 0

                    If Val(CDbl(lbl_TotalSales_Amount_Previous_Year.Text)) > Val(vMIN_TCS_assval) Then
                        vTDS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                    ElseIf Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) > Val(vMIN_TCS_assval) Then
                        vTDS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                    ElseIf (Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt)) > Val(vMIN_TCS_assval) Then
                        vTDS_AssVal = Format(Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt) - Val(vMIN_TCS_assval), "############0")

                    End If

                    If Common_Procedures.settings.CustomerCode = "1087" Then ' Kalaimagal Palladam
                        txt_TDS_TaxableValue.Text = Format(Val(lbl_GrossAmount.Text), "############0.00")
                    Else
                        txt_TDS_TaxableValue.Text = Format(Val(vTDS_AssVal), "############0.00")
                    End If


                    If Val(txt_TDS_TaxableValue.Text) > 0 Then
                        If Val(txt_TdsPerc.Text) = 0 Then
                            txt_TdsPerc.Text = "0.1"
                        End If
                    End If

                End If

                If Common_Procedures.settings.CustomerCode = "1087" Then ' Kalaimagal Palladam
                    vTDS_Amt = Format(Val(lbl_GrossAmount.Text) * Val(txt_TdsPerc.Text) / 100, "##########0")
                Else
                    vTDS_Amt = Format(Val(txt_TDS_TaxableValue.Text) * Val(txt_TdsPerc.Text) / 100, "##########0")
                End If

                lbl_TdsAmount.Text = Format(Val(vTDS_Amt), "##########0.00")

            Else

                txt_TDS_TaxableValue.Text = ""
                txt_TdsPerc.Text = ""
                lbl_TdsAmount.Text = ""

            End If

        Else

            txt_TDS_TaxableValue.Text = ""
            txt_TdsPerc.Text = ""
            lbl_TdsAmount.Text = ""

        End If




        'Old
        'NtAmt = Val(GrsAmt) - Val(lbl_DiscAmount.Text) + Val(txt_Freight.Text) + Val(GST_Amt) + Val(txt_AddLess.Text)
        'lbl_NetAmount.Text = Format(Val(NtAmt), "###########0")

        'lbl_RoundOff.Text = Format(Val(lbl_NetAmount.Text) - Val(NtAmt), "#########0.00")
        'If Val(lbl_RoundOff.Text) = 0 Then lbl_RoundOff.Text = ""

        'lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_NetAmount.Text)))


        NtAmt = Format(Val(lbl_BillAmount.Text) - Val(lbl_TdsAmount.Text), "##########0.00")

        lbl_NetAmount.Text = Format(Val(NtAmt), "##########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))


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
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Store_purchase_Inward_entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Stores_Item_Purchase_BillMaking_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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

        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" OR Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '   Else


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            '      End If


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

            '  da1 = New SqlClient.SqlDataAdapter("select a.*, b.*,c.Ledger_Name from Stores_Item_Purchase_BillMaking_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, CSH.State_Name as Company_State_Name  ,CSH.State_Code as Company_State_Code ,LSH.State_Name as Ledger_State_Name ,LSH.State_Code as Ledger_State_Code  from Stores_Item_Purchase_BillMaking_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_HEad CSH on b.Company_State_IdNo = CSH.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN State_HEad LSH on c.Ledger_State_IdNo = LSH.State_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                '  da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name,  f.Brand_Name,g.PO_Date  ,dp.department_name  from Stores_Item_Purchase_BillMaking_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno  LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno LEFT OUTER JOIN Stores_Item_PO_Head g ON g.PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' inner join Department_Head  dp on a.department_idno =dp.Department_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, b.Drawing_No, c.Department_name, d.Unit_name, e.Brand_Name, f.Motion_Type_Name from Stores_Item_Purchase_BillMaking_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Department_Head c ON b.Department_idno = c.Department_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Brand_Head e ON a.Brand_idno = e.Brand_idno LEFT OUTER JOIN Motion_Type_Head F ON a.Motion_Type_IdNo = f.Motion_Type_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
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
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" OR Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
        '    Printing_Format2(e)
        'Else
        'Printing_Format1(e)
        'End If

        Printing_FormatGST(e)



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

                'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
                '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                '        PrintDocument1.DefaultPageSettings.PaperSize = ps
                '        PpSzSTS = True
                '        Exit For
                '    End If
                'Next

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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name, f.Brand_Name from Stores_Item_Purchase_BillMaking_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno  LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_BillMaking_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" OR Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
            Common_Procedures.Print_To_PrintDocument(e, "PURCHASE ORDER RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        End If
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" OR Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then '---Prakash cottex

            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.prakash_cottex_Logo, Drawing.Image), LMargin + 20, CurY - 90, 112, 80)

        End If

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
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Item_Purchase_BillMaking_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Received From", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "Received From :  " & "M/s." & prn_HdDt.Rows(0).Item("Received_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Item_Purchase_BillMaking_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "PO.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Po_No").ToString.ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "PO.DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_DetDt.Rows(0).Item("PO_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


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


    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'if PpSzSTS = False Then

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next


        'If PpSzSTS = False Then

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next


        'If PpSzSTS = False Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            Exit For
        '        End If
        '    Next
        'End If
        'End If
        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 60 '30
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        'pFont = New Font("Calibri", 11, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        'If PrintDocument1.DefaultPageSettings.Landscape = True Then
        '    With PrintDocument1.DefaultPageSettings.PaperSize
        '        PrintWidth = .Height - TMargin - BMargin
        '        PrintHeight = .Width - RMargin - LMargin
        '        PageWidth = .Height - TMargin
        '        PageHeight = .Width - RMargin
        '    End With
        'End If

        NoofItems_PerPage = 16 '8

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(30) : ClAr(2) = 220 : ClAr(3) = 110 : ClAr(4) = 70 : ClAr(5) = 70
        'ClAr(1) = Val(35) : ClAr(2) = 325 : ClAr(3) = 150 : ClAr(4) = 80 : ClAr(5) = 75
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        TxtHgt = 18 '19

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
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

                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1 As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim I As Integer

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name, f.Brand_Name from Stores_Item_Purchase_BillMaking_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno  LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_BillMaking_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 5, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 25, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY + 5, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 5, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" OR Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
            Common_Procedures.Print_To_PrintDocument(e, "PURCHASE ORDER RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        End If
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" OR Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then '---Prakash cottex

            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.prakash_cottex_Logo, Drawing.Image), LMargin + 10, CurY - 90, 80, 80)

        End If

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2)
        'C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("Received From :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        'Common_Procedures.Print_To_PrintDocument(e, "Issued To", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Item_Purchase_BillMaking_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)


        ItmNm1 = "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, "Received From", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + S1 + 30, CurY, 0, 0, pFont)
        If Trim(ItmNm2) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & Trim(ItmNm2), LMargin + 10, CurY, 0, 0, pFont)
        End If

        'Common_Procedures.Print_To_PrintDocument(e, "Received From :  " & "M/s." & prn_HdDt.Rows(0).Item("Received_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Item_Purchase_BillMaking_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "PO.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Po_No").ToString.ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "PO.DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_DetDt.Rows(0).Item("PO_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


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

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
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
        Dim VSELECCODE As String

        'If Trim(UCase(cbo_EntType.Text)) <> "PO" Then
        '    MessageBox.Show("Invalid Type", "DOES NOT SELECT PO...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
        '    Exit Sub
        'End If

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




            For K = 1 To 2

                VSELECCODE = ""

                If K = 1 Then

                    VSELECCODE = Trim(Pk_Condition) & Trim(NewCode)

                End If


                ' Da = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, b.Drawing_No, c.Department_name, d.Unit_name, e.Brand_Name, f.Quantity as Ent_Purchase_Quantity, f.Rate as Ent_Rate, f.Purchase_BillMaking_Details_SlNo as Ent_Purchase_SlNo, f.PurchaseReturn_Quantity as Ent_PurcReturn_Qty from Stores_Item_PO_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Department_Head c ON b.Department_idno = c.Department_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Brand_Head e ON a.Brand_idno = e.Brand_idno LEFT OUTER JOIN Stores_Item_Purchase_BillMaking_Details F ON f.Item_Purchase_BillMaking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and f.Entry_Type = '" & Trim(cbo_EntType.Text) & "' and a.Po_Code = f.Po_Code and a.Po_Details_SlNo = f.PO_Details_SlNo Where a.ledger_idno = " & Str(Val(LedIdNo)) & " and ( (a.PO_Quantity - a.Cancel_Quantiy - a.Purchased_Quantity + a.PurchaseReturn_Quantity) > 0 or f.Quantity > 0 ) Order by a.For_OrderBy, a.PO_No, a.PO_Details_SlNo", con)
                Da = New SqlClient.SqlDataAdapter("Select  A.*, B.LEDGER_NAME FROM Stores_Item_Purchase_Head  A  LEFT OUTER JOIN Ledger_Head B  On A.Ledger_IdNo =B.Ledger_IdNo WHERE A.Item_Purchase_BillMaking_Code ='" & Trim(VSELECCODE) & "' And A.LEDGER_IDNO = " & Str(Val(LedIdNo)) & " order by a.Item_Purchase_Date, a.for_orderby, a.Item_Purchase_No ", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add
                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Item_Purchase_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Item_Purchase_Date").ToString), "dd-MM-yyyy").ToString
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Bill_No").ToString
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Total_Quantity").ToString
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Net_Amount").ToString


                        .Rows(n).Cells(6).Value = ""

                        If Trim(VSELECCODE) <> "" Then
                            .Rows(n).Cells(6).Value = "1"
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next
                        End If


                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Item_Purchase_Code").ToString


                    Next

                End If

            Next K
            Da.Dispose()
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

                If Trim(.Rows(RwIndx).Cells(7).Value) = "" Then Exit Sub


                .Rows(RwIndx).Cells(6).Value = (Val(.Rows(RwIndx).Cells(6).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(6).Value) = 0 Then

                    .Rows(RwIndx).Cells(6).Value = ""

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



        Close_Purchase_Bill_Selection()

        'Dim i As Integer, n As Integer
        'Dim sno As Integer
        'Dim Ent_Qty As Single, Ent_Rate As Single

        'dgv_Details.Rows.Clear()

        'NoCalc_Status = True

        'sno = 0

        'For i = 0 To dgv_Selection.RowCount - 1

        '    If Val(dgv_Selection.Rows(i).Cells(6).Value) = 1 Then

        '        n = dgv_Details.Rows.Add()

        '        sno = sno + 1

        '        'dgv_Details.Rows(n).Cells(0).Value = Val(sno)
        '        'dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(2).Value
        '        'dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
        '        'dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
        '        'dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
        '        'dgv_Details.Rows(n).Cells(5).Value = Val(Ent_Qty)
        '        'dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
        '        'dgv_Details.Rows(n).Cells(7).Value = Val(Ent_Rate)
        '        'dgv_Details.Rows(n).Cells(8).Value = Format(Val(Ent_Qty) * Val(Ent_Rate), "##########0.00")
        '        'dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(1).Value
        '        'dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(12).Value
        '        'dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(10).Value
        '        'dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(11).Value

        '    End If

        'Next i

        'NoCalc_Status = False
        'TotalQuantity_Calculation()

        'Grid_DeSelect()

        'pnl_Back.Enabled = True
        'pnl_Selection.Visible = False

        'txt_BillNo.Focus()

        ''If dgv_Details.Rows.Count > 0 Then
        ''    dgv_Details.Focus()
        ''    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
        ''    dgv_Details.CurrentCell.Selected = True

        ''Else
        ''    txt_DiscPerc.Focus()

        ''End If

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
        '  vCondt = "(AccountsGroup_IdNo = 27 Or AccountsGroup_IdNo = 15 Or AccountsGroup_IdNo = 16 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27 Or AccountsGroup_IdNo = 15 Or AccountsGroup_IdNo = 16 )", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAc, cbo_Ledger, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27 Or AccountsGroup_IdNo = 15 Or AccountsGroup_IdNo = 16 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAc, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27 Or AccountsGroup_IdNo = 15 Or AccountsGroup_IdNo = 16 )", "(Ledger_IdNo = 0)")
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
            'MessageBox.Show(ex.Message, "DOES Not Select...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_PaymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMethod, cbo_Ledger, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES Not Select...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_PurchaseAc, txt_GrnNo, "", "", "", "")

    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_GrnNo, "", "", "", "")

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
                            Taxable_Amount = Val(.Rows(RowIndx).Cells(8).Value) - Val(.Rows(RowIndx).Cells(15).Value)


                            .Rows(RowIndx).Cells(16).Value = Format(Val(Taxable_Amount), "##########0.00")
                            .Rows(RowIndx).Cells(17).Value = Format(Val(GST_Per), "########0.00")
                            .Rows(RowIndx).Cells(18).Value = Trim(HSN_Code)

                        End If

                    Next RowIndx

                    Get_HSN_CodeWise_Tax_Details()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES Not Do GST CALCULATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub Get_GST_Percentage_From_ItemGroup(ByVal ItemName As String, ByRef HSN_Code As String, ByRef GST_PerCent As Single)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            HSN_Code = ""
            GST_PerCent = 0

            da = New SqlClient.SqlDataAdapter("Select a.* from ItemGroup_Head a INNER JOIN Stores_Item_Head b On a.ItemGroup_IdNo = b.ItemGroup_IdNo Where b.Item_Name ='" & Trim(ItemName) & "'", con)
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

                AssVal_Pack_Frgt_Ins_Amt = Format(Val(txt_Freight.Text), "#########0.00")
                'AssVal_Pack_Frgt_Ins_Amt = Format(Val(txt_AddLess.Text) + Val(txt_Freight.Text), "#########0.00")

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

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub txt_AddLess_TextChanged(sender As Object, e As EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim pwd As String = ""

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> Trim(UCase("TSSA7417")) Then
            MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_RefNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub


    Private Sub txt_TCS_TaxableValue_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_TCS_TaxableValue.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_TotalSales_Amount_Current_Year_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_TotalSales_Amount_Current_Year.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_TotalSales_Amount_Previous_Year_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_TotalSales_Amount_Previous_Year.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub btn_EDIT_TCS_TaxableValue_Click(sender As System.Object, e As System.EventArgs) Handles btn_EDIT_TCS_TaxableValue.Click
        txt_TCS_TaxableValue.Enabled = Not txt_TCS_TaxableValue.Enabled
        txt_TcsPerc.Enabled = Not txt_TcsPerc.Enabled

        If txt_TCS_TaxableValue.Enabled Then

            txt_TCS_TaxableValue.Text = lbl_Invoice_Value_Before_TCS.Text
            txt_TcsPerc.Text = "0.1"

            txt_TCS_TaxableValue.Focus()

        Else
            chk_TCS_Tax.Focus()
            'btn_save.Focus()

        End If
    End Sub

    Private Sub chk_TCS_Tax_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCS_Tax.CheckedChanged
        NetAmount_Calculation()
    End Sub

    Private Sub chk_TCSAmount_RoundOff_STS_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCSAmount_RoundOff_STS.CheckedChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_TcsPerc_TextChanged(sender As Object, e As System.EventArgs) Handles txt_TcsPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub get_Ledger_TotalSales()
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim TtSalAmt_CurrYr As String = 0
        Dim TtSalAmt_PrevYr As String = 0
        Dim GpCd As String = ""
        Dim Datcondt As String = ""
        Dim n As Integer = 0
        Dim I As Integer = 0
        Dim Led_ID As Integer = 0
        Dim vPrevYrCode As String = ""
        Dim NewCode As String = ""


        Try


            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            lbl_TotalSales_Amount_Current_Year.Text = "0.00"
            lbl_TotalSales_Amount_Previous_Year.Text = "0.00"
            '-----------TOTAL SALES

            cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@entrydate", dtp_date.Value.Date)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            If Led_ID <> 0 Then

                cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount > 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GCLPR-%' OR a.Voucher_Code LIKE 'GPAVP-%' OR a.Voucher_Code LIKE 'GCOPU-%'  OR a.Voucher_Code LIKE 'GYPUR-%'  OR a.Voucher_Code LIKE 'GSPUR-%' OR a.Voucher_Code LIKE 'EBPUR-%' OR a.Voucher_Code LIKE 'GITPU-%' OR a.Voucher_Code LIKE 'VGYPR-%') "
                'cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GSSAL-%') "
                da = New SqlClient.SqlDataAdapter(cmd)
                dt1 = New DataTable
                da.Fill(dt1)

                TtSalAmt_CurrYr = 0
                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        TtSalAmt_CurrYr = Val(dt1.Rows(0).Item("BalAmount").ToString)
                    End If
                End If
                dt1.Clear()


                vPrevYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnYearCode), 2)
                vPrevYrCode = Trim(Format(Val(vPrevYrCode) - 1, "00")) & "-" & Trim(Format(Val(vPrevYrCode), "00"))

                cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount > 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(vPrevYrCode) & "' and (a.Voucher_Code LIKE 'GCLPR-%' OR a.Voucher_Code LIKE 'GPAVP-%' OR a.Voucher_Code LIKE 'GCOPU-%'  OR a.Voucher_Code LIKE 'GYPUR-%'  OR a.Voucher_Code LIKE 'GSPUR-%' OR a.Voucher_Code LIKE 'EBPUR-%' OR a.Voucher_Code LIKE 'GITPU-%'OR a.Voucher_Code LIKE 'VGYPR-%') "
                da = New SqlClient.SqlDataAdapter(cmd)
                dt1 = New DataTable
                da.Fill(dt1)

                TtSalAmt_PrevYr = 0
                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        TtSalAmt_PrevYr = Val(dt1.Rows(0).Item("BalAmount").ToString)
                    End If
                End If
                dt1.Clear()

                dt1.Dispose()
                da.Dispose()
                cmd.Dispose()

                lbl_TotalSales_Amount_Current_Year.Text = Trim(Common_Procedures.Currency_Format(Math.Abs(Val(TtSalAmt_CurrYr))))
                lbl_TotalSales_Amount_Previous_Year.Text = Trim(Common_Procedures.Currency_Format(Math.Abs(Val(TtSalAmt_PrevYr))))


            End If


        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE GETTIG TOTAL SALES....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub btn_EDIT_TDS_TaxableValue_Click(sender As System.Object, e As System.EventArgs) Handles btn_EDIT_TDS_TaxableValue.Click
        txt_TDS_TaxableValue.Enabled = Not txt_TDS_TaxableValue.Enabled

        txt_TdsPerc.Enabled = Not txt_TdsPerc.Enabled

        If txt_TDS_TaxableValue.Enabled Then

            If Common_Procedures.settings.CustomerCode = "1087" Then
                txt_TDS_TaxableValue.Text = lbl_GrossAmount.Text
            Else
                txt_TDS_TaxableValue.Text = txt_AssessableValue.Text
            End If


            txt_TdsPerc.Text = "0.1"

            txt_TDS_TaxableValue.Focus()

        Else
            chk_TDS_Tax.Focus()

        End If


    End Sub

    Private Sub chk_TDS_Tax_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TDS_Tax.CheckedChanged
        NetAmount_Calculation()
    End Sub
    Private Sub txt_TdsPerc_TextChanged(sender As Object, e As System.EventArgs) Handles txt_TdsPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_TDS_TaxableValue_TextChanged(sender As Object, e As EventArgs) Handles txt_TDS_TaxableValue.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub Cbo_Grid_MotionType_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_MotionType.KeyDown
        cbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_MotionType, Nothing, Nothing, "Motion_Type_Head", "Motion_Type_Name", "", "(Motion_Type_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And Cbo_Grid_MotionType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                e.Handled = True
                If .CurrentRow.Index <= 0 Then
                    txt_BillNo.Focus()

                Else
                    .CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index - 1).Cells(.ColumnCount - 5)

                End If

            End If

            If (e.KeyValue = 40 And Cbo_Grid_MotionType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                e.Handled = True
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_DiscPerc.Focus()

                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End If

        End With
    End Sub

    Private Sub Cbo_Grid_MotionType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Grid_MotionType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_MotionType, Nothing, "Motion_Type_Head", "Motion_Type_Name", "", "(Motion_Type_IdNo = 0)")
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

    Private Sub Cbo_Grid_MotionType_KeyUp(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_MotionType.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Department_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Grid_MotionType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Cbo_Grid_MotionType_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_MotionType.TextChanged
        Try
            If Cbo_Grid_MotionType.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(Cbo_Grid_MotionType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_MotionType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Close_Purchase_Bill_Selection()

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim n As Integer
        Dim sno As Integer
        Dim VLed_Idno As Integer


        dgv_Details.Rows.Clear()

        txt_GrnNo.Text = ""
        lbl_PurchaseCode.Text = ""

        Try

            For i = 0 To dgv_Selection.RowCount - 1

                If Val(dgv_Selection.Rows(i).Cells(6).Value) = 1 Then


                    txt_GrnNo.Text = dgv_Selection.Rows(i).Cells(1).Value
                    '   dgv_Selection.Rows(i).Cells(2).Value ------date 
                    txt_BillNo.Text = dgv_Selection.Rows(i).Cells(3).Value

                    With dgv_Details_Total
                        n = .Rows.Add()
                        sno = sno + 1
                        .Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(4).Value
                    End With

                    lbl_NetAmount.Text = dgv_Selection.Rows(i).Cells(5).Value
                    lbl_PurchaseCode.Text = dgv_Selection.Rows(i).Cells(7).Value


                    If Trim(lbl_PurchaseCode.Text) <> "" Then

                        VLed_Idno = Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Ledger.Text))

                        Da = New SqlClient.SqlDataAdapter("Select  A.*, B.LEDGER_NAME FROM Stores_Item_Purchase_Head  A  LEFT OUTER JOIN Ledger_Head B  On A.Ledger_IdNo =B.Ledger_IdNo WHERE A.Item_Purchase_Code ='" & Trim(lbl_PurchaseCode.Text) & "' And  A.LEDGER_IDNO =" & Str(Val(VLed_Idno)) & "  order by a.Item_Purchase_Date, a.for_orderby, a.Item_Purchase_No ", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)

                        If Dt1.Rows.Count > 0 Then


                            dtp_date.Text = Dt1.Rows(0).Item("Item_Purchase_Date").ToString
                            cbo_EntType.Text = Dt1.Rows(0).Item("Entry_Type").ToString

                            'cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                            If IsDBNull(Dt1.Rows(0).Item("Ledger_Name").ToString) = False Then
                                If Trim(Dt1.Rows(0).Item("Ledger_Name").ToString) <> "" Then
                                    If Val(Dt1.Rows(0).Item("Ledger_IdNo").ToString) <> 1 Then
                                        cbo_Ledger.Text = Dt1.Rows(0).Item("Ledger_Name").ToString
                                    Else
                                        cbo_Ledger.Text = Dt1.Rows(0).Item("Cash_PartyName").ToString
                                    End If
                                Else
                                    cbo_Ledger.Text = Dt1.Rows(0).Item("Cash_PartyName").ToString
                                End If
                            Else
                                cbo_Ledger.Text = Dt1.Rows(0).Item("Cash_PartyName").ToString
                            End If

                            '  txt_BillNo.Text = Dt1.Rows(0).Item("Bill_No").ToString
                            cbo_PurchaseAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(0).Item("PurchaseAc_IdNo").ToString))

                            lbl_GrossAmount.Text = Format(Val(Dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")
                            txt_DiscPerc.Text = Val(Dt1.Rows(0).Item("CashDiscount_Percentage").ToString)
                            lbl_DiscAmount.Text = Format(Val(Dt1.Rows(0).Item("CashDiscount_Amount").ToString), "#########0.00")
                            txt_AssessableValue.Text = Format(Val(Dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")


                            txt_Freight.Text = Format(Val(Dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")
                            txt_AddLess.Text = Format(Val(Dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
                            lbl_RoundOff.Text = Format(Val(Dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")
                            ' lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(Dt1.Rows(0).Item("Net_Amount").ToString))
                            cbo_TaxType.Text = Dt1.Rows(0).Item("GST_Tax_Type").ToString

                            txt_AssessableValue.Text = Format(Val(Dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")
                            lbl_CGST_Amount.Text = Format(Val(Dt1.Rows(0).Item("Total_CGST_Amount").ToString), "#########0.00")
                            lbl_SGST_Amount.Text = Format(Val(Dt1.Rows(0).Item("Total_SGST_Amount").ToString), "#########0.00")
                            lbl_IGST_Amount.Text = Format(Val(Dt1.Rows(0).Item("Total_IGST_Amount").ToString), "#########0.00")

                            txt_Remarks.Text = Dt1.Rows(0).Item("Remarks").ToString
                            cbo_PaymentMethod.Text = Dt1.Rows(0).Item("Payment_Method").ToString

                            If Val(Dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False
                            txt_TCS_TaxableValue.Text = Dt1.Rows(0).Item("TCS_Taxable_Value").ToString
                            If Val(Dt1.Rows(0).Item("EDIT_TCS_TaxableValue").ToString) = 1 Then
                                txt_TcsPerc.Enabled = True
                                txt_TCS_TaxableValue.Enabled = True
                            End If
                            If IsDBNull(Dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = False Then
                                If Val(Dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = 1 Then chk_TCSAmount_RoundOff_STS.Checked = True Else chk_TCSAmount_RoundOff_STS.Checked = False
                            End If
                            txt_TcsPerc.Text = Val(Dt1.Rows(0).Item("Tcs_Percentage").ToString)
                            lbl_TcsAmount.Text = Dt1.Rows(0).Item("TCS_Amount").ToString
                            lbl_Invoice_Value_Before_TCS.Text = Dt1.Rows(0).Item("Invoice_Value_Before_TCS").ToString
                            lbl_RoundOff_Invoice_Value_Before_TCS.Text = Dt1.Rows(0).Item("RoundOff_Invoice_Value_Before_TCS").ToString

                            If Val(Dt1.Rows(0).Item("TDS_Tax_Status").ToString) = 1 Then chk_TDS_Tax.Checked = True Else chk_TDS_Tax.Checked = False
                            txt_TDS_TaxableValue.Text = Dt1.Rows(0).Item("TDS_Taxable_Value").ToString
                            If Val(Dt1.Rows(0).Item("EDIT_TDS_TaxableValue").ToString) = 1 Then
                                txt_TdsPerc.Enabled = True
                                txt_TDS_TaxableValue.Enabled = True
                            End If
                            txt_TdsPerc.Text = Val(Dt1.Rows(0).Item("TDS_Percentage").ToString)
                            lbl_TdsAmount.Text = Dt1.Rows(0).Item("TDS_Amount").ToString

                            lbl_BillAmount.Text = Dt1.Rows(0).Item("Bill_Amount").ToString



                        End If


                        Da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, b.Drawing_No, c.Department_name, d.Unit_name, e.Brand_Name, f.Motion_Type_Name from Stores_Item_Purchase_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Department_Head c ON b.Department_idno = c.Department_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Brand_Head e ON a.Brand_idno = e.Brand_idno LEFT OUTER JOIN Motion_Type_Head F ON a.Motion_Type_IdNo = f.Motion_Type_IdNo  where a.Item_Purchase_Code = '" & Trim(lbl_PurchaseCode.Text) & "'   Order by a.Sl_No", con)
                        Dt2 = New DataTable
                        Da2.Fill(Dt2)

                        With dgv_Details

                            .Rows.Clear()
                            sno = 0

                            If Dt2.Rows.Count > 0 Then

                                For J = 0 To Dt2.Rows.Count - 1

                                    n = .Rows.Add()

                                    sno = sno + 1

                                    dgv_Details.Rows(n).Cells(0).Value = Val(sno)

                                    If IsDBNull(Dt2.Rows(J).Item("Department_name").ToString) = False Then
                                        If Trim(Dt2.Rows(J).Item("Department_name").ToString) <> "" Then
                                            dgv_Details.Rows(n).Cells(1).Value = Dt2.Rows(J).Item("Department_name").ToString
                                        Else
                                            dgv_Details.Rows(n).Cells(1).Value = Common_Procedures.Department_IdNoToName(con, 1)
                                        End If
                                    End If

                                    dgv_Details.Rows(n).Cells(2).Value = Dt2.Rows(J).Item("Drawing_No").ToString
                                    dgv_Details.Rows(n).Cells(3).Value = Dt2.Rows(J).Item("Item_name").ToString
                                    dgv_Details.Rows(n).Cells(4).Value = Dt2.Rows(J).Item("Brand_name").ToString
                                    dgv_Details.Rows(n).Cells(5).Value = Val(Dt2.Rows(J).Item("Quantity").ToString)
                                    dgv_Details.Rows(n).Cells(6).Value = Dt2.Rows(J).Item("Unit_name").ToString
                                    dgv_Details.Rows(n).Cells(7).Value = Format(Val(Dt2.Rows(J).Item("Rate").ToString), "########0.00")
                                    dgv_Details.Rows(n).Cells(8).Value = Format(Val(Dt2.Rows(J).Item("Amount").ToString), "########0.00")
                                    dgv_Details.Rows(n).Cells(9).Value = Val(Dt2.Rows(J).Item("Po_No").ToString)
                                    dgv_Details.Rows(n).Cells(10).Value = Val(Dt2.Rows(J).Item("Purchase_Details_SlNo").ToString)
                                    dgv_Details.Rows(n).Cells(11).Value = Dt2.Rows(J).Item("Po_Code").ToString
                                    dgv_Details.Rows(n).Cells(12).Value = Val(Dt2.Rows(J).Item("Po_Details_SlNo").ToString)
                                    dgv_Details.Rows(n).Cells(13).Value = Val(Dt2.Rows(J).Item("PurchaseReturn_Quantity").ToString)

                                    dgv_Details.Rows(n).Cells(16).Value = Val(Dt2.Rows(J).Item("Taxable_Value").ToString)
                                    dgv_Details.Rows(n).Cells(17).Value = Val(Dt2.Rows(J).Item("GST_Percentage").ToString)
                                    dgv_Details.Rows(n).Cells(18).Value = Val(Dt2.Rows(J).Item("HSN_Code").ToString)
                                    dgv_Details.Rows(n).Cells(19).Value = Dt2.Rows(J).Item("Catelog_pageno").ToString
                                    dgv_Details.Rows(n).Cells(20).Value = Dt2.Rows(J).Item("Motion_Type_Name").ToString



                                Next J
                            End If
                        End With



                        Da3 = New SqlClient.SqlDataAdapter("Select a.* from Store_Item_Purchase_GST_Tax_Details a Where a.Store_Item_Purchase_Code = '" & Trim(lbl_PurchaseCode.Text) & "' ", con)
                        Dt3 = New DataTable
                        Da3.Fill(Dt3)

                        With dgv_Tax_Details

                            .Rows.Clear()
                            sno = 0

                            If Dt3.Rows.Count > 0 Then

                                For K = 0 To Dt3.Rows.Count - 1

                                    n = .Rows.Add()

                                    sno = sno + 1

                                    .Rows(n).Cells(0).Value = sno
                                    .Rows(n).Cells(1).Value = Trim(Dt3.Rows(K).Item("HSN_Code").ToString)
                                    .Rows(n).Cells(2).Value = IIf(Val(Dt3.Rows(K).Item("Taxable_Amount").ToString) <> 0, Format(Val(Dt3.Rows(K).Item("Taxable_Amount").ToString), "############0.00"), "")
                                    .Rows(n).Cells(3).Value = IIf(Val(Dt3.Rows(K).Item("CGST_Percentage").ToString) <> 0, Val(Dt3.Rows(K).Item("CGST_Percentage").ToString), "")
                                    .Rows(n).Cells(4).Value = IIf(Val(Dt3.Rows(K).Item("CGST_Amount").ToString) <> 0, Format(Val(Dt3.Rows(K).Item("CGST_Amount").ToString), "##########0.00"), "")
                                    .Rows(n).Cells(5).Value = IIf(Val(Dt3.Rows(K).Item("SGST_Percentage").ToString) <> 0, Val(Dt3.Rows(K).Item("SGST_Percentage").ToString), "")
                                    .Rows(n).Cells(6).Value = IIf(Val(Dt3.Rows(K).Item("SGST_Amount").ToString) <> 0, Format(Val(Dt3.Rows(K).Item("SGST_Amount").ToString), "###########0.00"), "")
                                    .Rows(n).Cells(7).Value = IIf(Val(Dt3.Rows(K).Item("IGST_Percentage").ToString) <> 0, Val(Dt3.Rows(K).Item("IGST_Percentage").ToString), "")
                                    .Rows(n).Cells(8).Value = IIf(Val(Dt3.Rows(K).Item("IGST_Amount").ToString) <> 0, Format(Val(Dt3.Rows(K).Item("IGST_Amount").ToString), "###########0.00"), "")

                                Next K

                            End If

                        End With


                    End If

                End If

            Next


            'Da.Dispose()
            'Dt1.Clear()
            'Da2.Dispose()
            'Dt2.Clear()
            'Da3.Dispose()
            'Dt3.Clear()

            NoCalc_Status = False
            TotalQuantity_Calculation()

            Grid_DeSelect()

            pnl_Back.Enabled = True
            pnl_Selection.Visible = False

            txt_DiscPerc.Focus()


        Catch ex As Exception

            MessageBox.Show(ex.Message, "ERROR ON SELECTION", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_GrnNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_GrnNo.KeyDown

        If e.KeyValue = 38 Then
            cbo_TaxType.Focus()

        End If
        If e.KeyValue = 40 Then
            txt_BillNo.Focus()
        End If
    End Sub

    Private Sub txt_GrnNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_GrnNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_BillNo.Focus()

        End If
    End Sub
    Private Sub Printing_FormatGST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer, j As Integer, k As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim VDepartNme1 As String, VDepartNme2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer
        Dim VItem1 As String
        Dim VItem2 As String
        Dim VItem3 As String
        Dim VItem4 As String




        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 45
            .Top = 45
            .Bottom = 45
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

        NoofItems_PerPage = 13 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(30) : ClArr(2) = 150 : ClArr(3) = 50 : ClArr(4) = 150 : ClArr(5) = 80 : ClArr(6) = 50 : ClArr(7) = 50 : ClArr(8) = 65
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_FormatGST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1


                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_FormatGST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        VDepartNme1 = prn_DetDt.Rows(prn_DetIndx).Item("Department_Name").ToString
                        VDepartNme2 = ""
                        If Len(VDepartNme1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(VDepartNme1), I, 1) = " " Or Mid$(Trim(VDepartNme1), I, 1) = "," Or Mid$(Trim(VDepartNme1), I, 1) = "." Or Mid$(Trim(VDepartNme1), I, 1) = "-" Or Mid$(Trim(VDepartNme1), I, 1) = "/" Or Mid$(Trim(VDepartNme1), I, 1) = "_" Or Mid$(Trim(VDepartNme1), I, 1) = "(" Or Mid$(Trim(VDepartNme1), I, 1) = ")" Or Mid$(Trim(VDepartNme1), I, 1) = "\" Or Mid$(Trim(VDepartNme1), I, 1) = "[" Or Mid$(Trim(VDepartNme1), I, 1) = "]" Or Mid$(Trim(VDepartNme1), I, 1) = "{" Or Mid$(Trim(VDepartNme1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            VDepartNme2 = Microsoft.VisualBasic.Right(Trim(VDepartNme1), Len(VDepartNme1) - I)
                            VDepartNme1 = Microsoft.VisualBasic.Left(Trim(VDepartNme1), I - 1)
                        End If



                        VItem1 = prn_DetDt.Rows(prn_DetIndx).Item("Item_name").ToString
                        VItem2 = ""

                        VItem3 = ""
                        VItem4 = ""

                        If Len(VItem1) > 20 Then
                            For j = 20 To 1 Step -1
                                If Mid$(Trim(VItem1), j, 1) = " " Or Mid$(Trim(VItem1), j, 1) = "," Or Mid$(Trim(VItem1), j, 1) = "." Or Mid$(Trim(VItem1), j, 1) = "-" Or Mid$(Trim(VItem1), j, 1) = "/" Or Mid$(Trim(VItem1), j, 1) = "_" Or Mid$(Trim(VItem1), j, 1) = "(" Or Mid$(Trim(VItem1), j, 1) = ")" Or Mid$(Trim(VItem1), j, 1) = "\" Or Mid$(Trim(VItem1), j, 1) = "[" Or Mid$(Trim(VItem1), j, 1) = "]" Or Mid$(Trim(VItem1), j, 1) = "{" Or Mid$(Trim(VItem1), j, 1) = "}" Then Exit For
                            Next j

                            If j = 0 Then j = 20

                            VItem2 = Microsoft.VisualBasic.Right(Trim(VItem1), Len(VItem1) - j)
                            VItem1 = Microsoft.VisualBasic.Left(Trim(VItem1), j - 1)

                        End If



                        If Len(VItem2) > 15 Then

                            For j = 15 To 1 Step -1
                                If Mid$(Trim(VItem2), j, 1) = " " Or Mid$(Trim(VItem2), j, 1) = "," Or Mid$(Trim(VItem1), j, 1) = "." Or Mid$(Trim(VItem2), j, 1) = "-" Or Mid$(Trim(VItem2), j, 1) = "/" Or Mid$(Trim(VItem2), j, 1) = "_" Or Mid$(Trim(VItem2), j, 1) = "(" Or Mid$(Trim(VItem2), j, 1) = ")" Or Mid$(Trim(VItem2), j, 1) = "\" Or Mid$(Trim(VItem2), j, 1) = "[" Or Mid$(Trim(VItem2), j, 1) = "]" Or Mid$(Trim(VItem2), j, 1) = "{" Or Mid$(Trim(VItem2), j, 1) = "}" Then Exit For

                            Next j

                            If j = 0 Then j = 15

                            VItem3 = Microsoft.VisualBasic.Right(Trim(VItem2), Len(VItem2) - j)
                            VItem2 = Microsoft.VisualBasic.Left(Trim(VItem2), j - 1)


                        End If

                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(VDepartNme1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Brand_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)


                        CurY = CurY + TxtHgt + 10

                        SNo = SNo + 1

                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(VDepartNme1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Drawing_No").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(VItem1), LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Brand_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(VItem2) <> "" Or Trim(VDepartNme2) <> "" Then

                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(VDepartNme2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(VItem2), LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                        End If

                        If Trim(VItem3) <> "" Or Trim(VItem4) <> "" Then

                            CurY = CurY + TxtHgt - 5

                            Common_Procedures.Print_To_PrintDocument(e, Trim(VItem3), LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(VItem4), LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY + 20, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                        End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_FormatGST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_FormatGST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim DelvToName As String
        Dim CurY1 As Single = 0, CurX As Single = 0
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Stores_Item_Purchase_BillMaking_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_BillMaking_code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
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
        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            '.BackgroundImage = Image.FromStream(ms)

                            ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 15, CurY + 10, 110, 110)

                        End If

                    End Using

                End If

            End If

        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        'End If

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "BILL PASS STATEMENT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("PURCHASE NO             : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Purchase", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Item_Purchase_BillMaking_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Purchase Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Item_Purchase_BillMaking_DATE").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)



            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


                Common_Procedures.Print_To_PrintDocument(e, "Grn No", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Item_Purchase_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

            '    Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Y/N)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "YES", LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(3) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(4), LMargin + C1, LnAr(2))
            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            ''Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

            '    CurY = CurY + TxtHgt + 10

            ' DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))

            ''Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Delivery At", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + W1 + 30, CurY, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, "Grn No", LMargin + C2 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Item_Purchase_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(4), LMargin + C1, LnAr(2))
            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DEPARTMENT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "PART", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2), CurY + 15, 2, ClAr(3), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BRAND", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY + 10, PageWidth, CurY + 10)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_FormatGST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
        Dim BmsInWrds As String

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)

            '   Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))


            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width
            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Trim(prn_HdDt.Rows(0).Item("CashDiscount_Percentage").ToString) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            ' CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'If is_LastPage = True Then
            '    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            'End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 10
            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @" & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @" & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @" & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "AddLess After Tax ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + C1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            'CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            'CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            CurY = CurY + TxtHgt - 10
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + 10
            'p1Font = New Font("Calibri", 12, FontStyle.Underline)
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   defect noted in yarn please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt


            CurY = CurY + TxtHgt



            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Function get_GST_Tax_Percentage_For_Printing(ByVal EntryCode As String) As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0

        TaxPerc = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Stores_Item_Purchase_BillMaking_GST_Tax_Details Where item_Purchase_BillMaking_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Stores_Item_Purchase_BillMaking_GST_Tax_Details Where item_Purchase_BillMaking_Code = '" & Trim(EntryCode) & "'", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    If Val(Dt2.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                        TaxPerc = Val(Dt2.Rows(0).Item("IGST_Percentage").ToString)
                    Else
                        TaxPerc = Val(Dt2.Rows(0).Item("CGST_Percentage").ToString)
                    End If
                End If
                Dt2.Clear()

            End If
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Dt2.Dispose()
        Da.Dispose()

        get_GST_Tax_Percentage_For_Printing = Format(Val(TaxPerc), "#########0.00")

    End Function
End Class