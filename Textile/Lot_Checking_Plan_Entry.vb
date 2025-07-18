Imports System.Drawing.Printing
Imports System.IO
Imports System.IO.Ports
Imports System.Threading
Imports Excel = Microsoft.Office.Interop.Excel
Public Class Lot_Checking_Plan_ENtry
    Implements Interface_MDIActions

    Public vEntry_BaleGroupIdNo As Integer = 0
    Private vEntry_BaleGroupName As String = ""

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Other_Condition As String = ""
    Private PkCondition_Entry As String = "LCHKP-"
    'Private PkCondition_Sample As String = "SPKSL-"
    'Private PkCondition_Direct As String = "PASLD-"
    'Private PkCondition_BaleGroupWiseEntry As String = "PBG"
    'Private PkCondition_RollPacking As String = "RLPCK-"
    'Private PkCondition_BaleDirectEntry As String = "BALES-"
    'Private PkCondition_BaleOnly As String = "PSBAL-"
    'Private PkCondition_BundleOnly As String = "PSROL-"
    'Private PkCondition_RollOnly As String = "PSBUN-"
    'Private PkCondition_TABLET As String = "PSLPT-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private vEntryType As String = ""
    Private dgv_ActCtrlName As String = ""
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private dgvDet_CboBx_ColNos_Arr As Integer() = {-100}
    Private Prn_BarcodeSticker As Boolean = False

    Private Print_PDF_Status As Boolean = False
    Private EMAIL_Status As Boolean = False
    Private WHATSAPP_Status As Boolean = False
    Private vEMAIL_Attachment_FileName As String


    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_HdAr(1000, 10) As String
    Private prn_DetAr(1000, 1000, 10) As String

    Private prn_DetAr1(1000, 10) As String

    Private prn_DetMxIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_HdIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Count As Integer = 0
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private prn_DetBarCdStkr As Integer

    Private prn_TotalBales As Integer = 0
    Private prn_TotalPcs As String = ""
    Private prn_TotalMtrs As String = ""
    Private prn_TotalWgt As String = ""
    Private Total_mtrs As Single = 0
    Private Format_2_Status As Integer = 0

    Private prn_meters As String = ""
    Private prn_Pcs As String = ""

    Private prn_HeadIndx As Integer
    Private vtot_pcs As Integer = 0
    Private vtot_wgt As Integer = 0
    Private lst_prnt As Boolean = False

    Private prn_Clothname As String = ""
    Private vTot_Mtrs As String = ""
    Private vPacking_SlipNo As String = ""
    Private vPartyName As String = ""
    Private Total_pcs As Integer = 0
    Private Pack_Type_Name As String = ""
    Private vPRN_Weight_Column_Status As Boolean

    Private fs As FileStream
    Private sw As StreamWriter
    Public Sub New()
        'vEntryType = Trim(UCase(EntryType))
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False
        Prn_BarcodeSticker = False
        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Pnl_ClothReceipt_Selection.Visible = False
        pnl_Print.Visible = False
        vPRN_Weight_Column_Status = False

        Print_PDF_Status = False
        EMAIL_Status = False
        WHATSAPP_Status = False
        vEMAIL_Attachment_FileName = ""


        vmskOldText = ""
        vmskSelStrt = -1
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black



        msk_Plan_date.Text = ""
        dtp_Plan_Date.Text = ""

        cbo_Checking_Section.Text = ""







        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))


        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()
        Dgv_ClothReceipt_Selection.Rows.Clear()



        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            'cbo_Filter_Cloth.Text = ""
            cbo_Filter_CheckingSection.Text = ""
            'cbo_Filter_Cloth.SelectedIndex = -1
            cbo_Filter_CheckingSection.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If


        cbo_Checking_Section.Enabled = True
        cbo_Checking_Section.BackColor = Color.White
        btn_ClothReceipt_Selection.Enabled = True

        dgv_ActCtrlName = ""

        Grid_Cell_DeSelect()
        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.SpringGreen
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

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        'If Me.ActiveControl.Name <> cbo_Cloth.Name And Me.ActiveControl.Name <> cbo_Checking_Section.Name And Me.ActiveControl.Name <> cbo_Bale_Bundle.Name And Me.ActiveControl.Name <> txt_Folding.Name And Me.ActiveControl.Name <> dgv_Details.Name And Me.ActiveControl.Name <> dgtxt_Details.Name Then
        '    pnl_StockDisplay.Visible = False
        'End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(2, 57, 111)
                Prec_ActCtrl.ForeColor = Color.White
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

    Private Sub Packing_Slip_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer = 0
        Dim CompCondt As String = ""

        Try

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName_StockOF.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_PartyName_StockOF.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Cloth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Checking_Section.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Checking_Section.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Sales_Party_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Sales_Party_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

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

                Me.Text = lbl_Heading.Text & "  -  " & lbl_Company.Text

                new_record()

            End If

            'If FrmLdSTS = True Then

            '    lbl_Company.Text = ""
            '    lbl_Company.Tag = 0
            '    Common_Procedures.CompIdNo = 0

            '    Me.Text = ""

            '    CompCondt = ""
            '    If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
            '        CompCondt = "Company_Type = 'ACCOUNT'"
            '    End If

            '    da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
            '    dt1 = New DataTable
            '    da.Fill(dt1)

            '    NoofComps = 0
            '    If dt1.Rows.Count > 0 Then
            '        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
            '            NoofComps = Val(dt1.Rows(0)(0).ToString)
            '        End If
            '    End If
            '    dt1.Clear()

            '    If Val(NoofComps) = 1 Then

            '        da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo", con)
            '        dt1 = New DataTable
            '        da.Fill(dt1)

            '        If dt1.Rows.Count > 0 Then
            '            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
            '                Common_Procedures.CompIdNo = Val(dt1.Rows(0)(0).ToString)
            '            End If

            '        End If
            '        dt1.Clear()

            '    Else

            '        Dim f As New Company_Selection
            '        f.ShowDialog()

            '    End If

            '    If Val(Common_Procedures.CompIdNo) <> 0 Then

            '        da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
            '        dt1 = New DataTable
            '        da.Fill(dt1)

            '        If dt1.Rows.Count > 0 Then
            '            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
            '                lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
            '                lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
            '                Me.Text = Trim(dt1.Rows(0)(1).ToString)
            '            End If
            '        End If
            '        dt1.Clear()

            '        new_record()

            '    Else
            '        MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            '        'Me.Close()
            '        Exit Sub

            '    End If

            'End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub Packing_Slip_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Packing_Slip_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_ClothReceipt_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Packing_Slip_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim I As Integer
        Dim x1, y1 As Single

        con.Open()

        PkCondition_Entry = ""

        'txt_BalePrefixNo.Visible = False
        'cbo_BaleSuffixNo.Visible = False
        vEntry_BaleGroupName = ""
        'If Trim(UCase(vEntryType)) = "SAMPLE" Then
        '    lbl_Heading.Text = "PACKING SLIP (SAMPLE)"
        '    PkCondition_Entry = PkCondition_Sample
        '    Other_Condition = "(Packing_Slip_Code LIKE '" & Trim(PkCondition_Sample) & "%')"
        '    Me.BackColor = Color.LightGray

        '    'lbl_BaleRefNo.Left = cbo_PartyName_StockOF.Left
        '    'lbl_BaleRefNo.Width = cbo_PartyName_StockOF.Width

        'ElseIf Trim(UCase(vEntryType)) = "BALEGROUPWISE" Then
        '    lbl_Heading.Text = "PACKING SLIP (BALEGROUPWISE)"
        '    PkCondition_Entry = Trim(PkCondition_BaleGroupWiseEntry) & Trim(Format(Val(vEntry_BaleGroupIdNo), "00")) & "-"
        '    Other_Condition = "(Packing_Slip_Code LIKE '" & Trim(PkCondition_Entry) & "%' and BaleGroup_IdNo = " & Str(Val(vEntry_BaleGroupIdNo)) & " )"
        '    Me.BackColor = Color.WhiteSmoke
        '    'txt_BalePrefixNo.Enabled = False
        '    vEntry_BaleGroupName = Common_Procedures.ClothSet_IdNoToName(con, vEntry_BaleGroupIdNo)
        '    'txt_BalePrefixNo.Visible = True
        '    'cbo_BaleSuffixNo.Visible = True

        'ElseIf Trim(UCase(vEntryType)) = "BALE" Then

        '    lbl_Heading.Text = "PACKING SLIP (BALE)"
        '    PkCondition_Entry = PkCondition_BaleOnly
        '    Other_Condition = "(Packing_Slip_Code LIKE '" & Trim(PkCondition_BaleOnly) & "%')"
        '    Me.BackColor = Color.LightSteelBlue

        '    'txt_BalePrefixNo.Visible = True
        '    'cbo_BaleSuffixNo.Visible = True

        '    lbl_RefNo_Caption.Text = "Bale No"

        'ElseIf Trim(UCase(vEntryType)) = "ROLL" Then

        '    lbl_Heading.Text = "PACKING SLIP (ROLL)"
        '    PkCondition_Entry = PkCondition_RollOnly
        '    Other_Condition = "(Packing_Slip_Code LIKE '" & Trim(PkCondition_RollOnly) & "%')"
        '    Me.BackColor = Color.LightGray ' Color.LightSalmon  ' Color.LightSeaGreen

        '    'txt_BalePrefixNo.Visible = True
        '    'cbo_BaleSuffixNo.Visible = True

        '    lbl_RefNo_Caption.Text = "Roll No"

        'ElseIf Trim(UCase(vEntryType)) = "BUNDLE" Then

        '    lbl_Heading.Text = "PACKING SLIP (BUNDLE)"
        '    PkCondition_Entry = PkCondition_BundleOnly
        '    Other_Condition = "(Packing_Slip_Code LIKE '" & Trim(PkCondition_BundleOnly) & "%')"
        '    Me.BackColor = Color.LightGoldenrodYellow

        '    'txt_BalePrefixNo.Visible = True
        '    'cbo_BaleSuffixNo.Visible = True

        '    lbl_RefNo_Caption.Text = "Bundle No"

        'ElseIf Trim(UCase(vEntryType)) = "TABLET" Then

        '    lbl_Heading.Text = "PACKING SLIP (TABLET)"
        '    PkCondition_Entry = PkCondition_TABLET
        '    Other_Condition = "(Packing_Slip_Code LIKE '" & Trim(PkCondition_Entry) & "%')"
        '    Me.BackColor = Color.Khaki ' Color.LightSteelBlue

        '    'txt_BalePrefixNo.Visible = True
        '    'cbo_BaleSuffixNo.Visible = True

        '    x1 = lbl_BaleRefNo.Left
        '    y1 = lbl_BaleRefNo.Top

        '    'lbl_BaleRefNo.Left = txt_BalePrefixNo.Left
        '    'lbl_BaleRefNo.Top = txt_BalePrefixNo.Top

        '    'txt_BalePrefixNo.Left = lbl_BaleRefNo.Left + lbl_BaleRefNo.Width + 10 ' x1
        '    'txt_BalePrefixNo.Top = y1

        'Else

        '    lbl_Heading.Text = "PACKING SLIP"
        '    PkCondition_Entry = ""
        '    Other_Condition = "(Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_Sample) & "%' and Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_Direct) & "%' and Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%' and Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' and Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_BaleGroupWiseEntry) & "%' and Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_BaleOnly) & "%' and Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_RollOnly) & "%' and Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_BundleOnly) & "%')"

        '    'txt_BalePrefixNo.Visible = True
        '    'cbo_BaleSuffixNo.Visible = True

        'End If

        'dgv_Details.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))
        'dgv_Selection.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))

        btn_BarcodePrint.Visible = False
        If Common_Procedures.settings.CustomerCode = "1267" Then
            dtp_Plan_Date.Enabled = True
            msk_Plan_date.Enabled = True
            btn_BarcodePrint.Visible = True

        ElseIf Common_Procedures.settings.CustomerCode = "1155" Then
            btn_BarcodePrint.Visible = True
            btn_BarcodePrint.Text = "PACKING SLIP STICKER"
            btn_BarcodePrint.BackColor = Color.DarkOrange

        Else
            'cbo_BaleSuffixNo.DropDownStyle = ComboBoxStyle.Simple

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1381" Then
            btn_excel.Visible = True
            'chk_Bale_Close_Sts.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            lbl_RefNo_Caption.Text = "Bale No"

            dgv_Details.Columns(2).HeaderText = "RollNo/PieceNo"
            'dgv_Selection.Columns(2).HeaderText = "RollNo/PieceNo"

            dgv_Details.Columns(1).Visible = False
            dgv_Details.Columns(2).Width = dgv_Details.Columns(2).Width + dgv_Details.Columns(1).Width

            'dgv_Selection.Columns(1).Visible = False
            'dgv_Selection.Columns(2).Width = dgv_Selection.Columns(2).Width + dgv_Selection.Columns(1).Width
            'dgv_Selection.Columns(9).Visible = False
            'dgv_Selection.Columns(7).Width = dgv_Selection.Columns(7).Width + dgv_Selection.Columns(9).Width - 10

        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1438" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1487" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1489" Then
            '-----SATHY TEXTILES (SATHYAMANGALAM)
            lbl_RefNo_Caption.Text = "Packing Slip No."
            dgv_Details.Columns(2).HeaderText = "Roll No."
            'dgv_Selection.Columns(2).HeaderText = "Roll No."

            'cbo_Sales_Party_Name.Visible = True
            'lbl_Sales_Party_Name.Visible = True
            'txt_Note.Width = Lbl_DelvCode.Width

            btn_Print_Ok.Text = "PACKING LIST" & Chr(13) & "(Without Weight)"

            btn_BarcodePrint.Visible = True
            btn_BarcodePrint.Text = "PACKING LIST" & Chr(13) & "(With Weight)"

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1520" Then

            'cbo_Sales_Party_Name.Visible = True
            'lbl_Sales_Party_Name.Visible = True
            'txt_Note.Width = Lbl_DelvCode.Width

            btn_Print_Ok.Text = "PACKING LIST" & Chr(13) & "(Without Weight)"

            btn_BarcodePrint.Visible = True
            btn_BarcodePrint.Text = "PACKING LIST" & Chr(13) & "(With Weight)"

        End If

        'dgv_Details.ReadOnly = True
        'dgv_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1490" Then '---- LAKSHMI  SARASWATHI EXPORTS (THIRUCHENCODE)

            dgv_Details.ReadOnly = False
            dgv_Details.SelectionMode = DataGridViewSelectionMode.CellSelect
            For I = 0 To dgv_Details.ColumnCount - 1
                dgv_Details.Columns(I).ReadOnly = True
            Next

            dgv_Details.Columns(7).Visible = False
            dgv_Details.Columns(9).Visible = False
            dgv_Details.Columns(10).Visible = False

            dgv_Details_Total.Columns(7).Visible = False
            dgv_Details_Total.Columns(9).Visible = False
            dgv_Details_Total.Columns(10).Visible = False

            dgv_Details.Columns(11).Visible = True
            dgv_Details.Columns(12).Visible = True
            dgv_Details.Columns(13).Visible = True
            dgv_Details.Columns(14).Visible = True
            dgv_Details.Columns(15).Visible = True

            dgv_Details.Columns(11).ReadOnly = False
            dgv_Details.Columns(12).ReadOnly = False
            dgv_Details.Columns(13).ReadOnly = True
            dgv_Details.Columns(14).ReadOnly = False
            dgv_Details.Columns(15).ReadOnly = True


            dgv_Details.Columns(11).Width = (dgv_Details.Columns(7).Width + dgv_Details.Columns(9).Width + dgv_Details.Columns(10).Width) \ 5
            dgv_Details.Columns(12).Width = dgv_Details.Columns(11).Width
            dgv_Details.Columns(13).Width = dgv_Details.Columns(11).Width
            dgv_Details.Columns(14).Width = dgv_Details.Columns(11).Width
            dgv_Details.Columns(15).Width = dgv_Details.Columns(11).Width - 5

            dgv_Details_Total.Columns(11).Visible = True
            dgv_Details_Total.Columns(12).Visible = True
            dgv_Details_Total.Columns(13).Visible = True
            dgv_Details_Total.Columns(14).Visible = True
            dgv_Details_Total.Columns(15).Visible = True

            For I = 0 To dgv_Details.ColumnCount - 1
                dgv_Details_Total.Columns(I).Visible = dgv_Details.Columns(I).Visible
                dgv_Details_Total.Columns(I).Width = dgv_Details.Columns(I).Width
            Next

        End If

        pnl_StockDisplay.Visible = False
        pnl_StockDisplay.Left = dgv_Details.Left
        'pnl_StockDisplay.Top = txt_net_weight.Top - 5
        pnl_StockDisplay.BringToFront()

        Me.Text = ""

        'cbo_PartyName_StockOF.Visible = False
        'lbl_PartyName_StockOF_Caption.Visible = False
        'If Common_Procedures.settings.JOBWORKENTRY_Status = 1 Then
        '    cbo_PartyName_StockOF.Visible = True
        '    lbl_PartyName_StockOF_Caption.Visible = True
        'End If

        'cbo_Godown_StockIN.Visible = False
        'lbl_Godown_StockIN_Caption.Visible = False
        'If Common_Procedures.settings.Multi_Godown_Status = 1 Then
        '    cbo_Godown_StockIN.Visible = True
        '    lbl_Godown_StockIN_Caption.Visible = True

        '    If Common_Procedures.settings.JOBWORKENTRY_Status = 0 Then
        '        lbl_Godown_StockIN_Caption.Left = lbl_PartyName_StockOF_Caption.Left
        '        cbo_Godown_StockIN.Left = cbo_PartyName_StockOF.Left
        '        cbo_Godown_StockIN.Width = cbo_PartyName_StockOF.Width
        '    End If

        'End If


        If Common_Procedures.settings.JOBWORKENTRY_Status = 0 And Common_Procedures.settings.Multi_Godown_Status = 0 Then
            Dim X As Single = 0
            'X = (cbo_Cloth.Top - cbo_PartyName_StockOF.Top) \ 2
            'lbl_ClothCaption.Top = lbl_ClothCaption.Top - X
            'lbl_ClothStarCaption.Top = lbl_ClothStarCaption.Top - X
            'cbo_Cloth.Top = cbo_Cloth.Top - X
            lbl_Checking_Section.Top = lbl_Checking_Section.Top - X
            cbo_Checking_Section.Top = cbo_Checking_Section.Top - X
        End If



        dtp_Plan_Date.Text = ""
        msk_Plan_date.Text = ""

        'cbo_Bale_Bundle.Items.Clear()
        'cbo_Bale_Bundle.Items.Add("BALE")
        'cbo_Bale_Bundle.Items.Add("BUNDLE")
        'cbo_Bale_Bundle.Items.Add("ROLL")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        Pnl_ClothReceipt_Selection.Visible = False
        Pnl_ClothReceipt_Selection.Left = (Me.Width - Pnl_ClothReceipt_Selection.Width) \ 2
        Pnl_ClothReceipt_Selection.Top = (Me.Height - Pnl_ClothReceipt_Selection.Height) \ 2
        Pnl_ClothReceipt_Selection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()


        'chk_Verified_Status.Visible = False
        'If Common_Procedures.settings.Vefified_Status = 1 Then
        '    If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then chk_Verified_Status.Visible = True
        'End If


        'btn_UserModification.Visible = False
        'If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
        '    If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
        '        btn_UserModification.Visible = True
        '    End If
        'End If

        'AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus



        'AddHandler txt_BalePrefixNo.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_BaleSuffixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Plan_date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Allotment_Date.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Checking_Section.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Bale_Bundle.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CheckingSection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintTo.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_PartyName_StockOF.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Ok.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_PcsSelection_LotNo.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_PcsSelection_PcsNo.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_PcsSelection_MetersEqualTo.GotFocus, AddressOf ControlGotFocus
        ''AddHandler cbo_Godown_StockIN.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_PcsSelection_BarCode.GotFocus, AddressOf ControlGotFocus

        'AddHandler txt_net_weight.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_gross_weight.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_Tare_weight.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Sales_Party_Name.Enter, AddressOf ControlGotFocus

        'AddHandler txt_BalePrefixNo.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_BaleSuffixNo.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_PartyName_StockOF.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Plan_date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Allotment_Date.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Checking_Section.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Bale_Bundle.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CheckingSection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintTo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Ok.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_PcsSelection_LotNo.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_PcsSelection_PcsNo.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_PcsSelection_MetersEqualTo.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Godown_StockIN.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_PcsSelection_BarCode.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Sales_Party_Name.Leave, AddressOf ControlLostFocus


        'AddHandler txt_net_weight.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_Tare_weight.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_gross_weight.LostFocus, AddressOf ControlLostFocus

        'AddHandler txt_net_weight.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Tare_weight.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_gross_weight.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_Plan_date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_BalePrefixNo.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_BaleSuffixNo.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler txt_net_weight.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Tare_weight.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_gross_weight.KeyPress, AddressOf TextBoxControlKeyPress

        'AddHandler msk_Plan_date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_BalePrefixNo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler cbo_BaleSuffixNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PrintFrom.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf dgv_ActCtrlName = dgv_Details.Name Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False And Pnl_Back.Enabled = True Then

                If IsNothing(dgv1.CurrentCell) Then Exit Function

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then


                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            btn_save.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(7)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.RowIndex = 0 Then
                            cbo_Checking_Section.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        'Dim da As SqlClient.SqlDataAdapter
        'Dim dt1 As New DataTable
        'Dim NoofComps As Integer
        'Dim CompCondt As String

        'Try

        '    lbl_Company.Tag = 0
        '    lbl_Company.Text = ""
        '    Me.Text = ""
        '    Common_Procedures.CompIdNo = 0

        '    CompCondt = ""
        '    If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
        '        CompCondt = "Company_Type = 'ACCOUNT'"
        '    End If

        '    da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
        '    dt1 = New DataTable
        '    da.Fill(dt1)

        '    NoofComps = 0
        '    If dt1.Rows.Count > 0 Then
        '        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
        '            NoofComps = Val(dt1.Rows(0)(0).ToString)
        '        End If
        '    End If
        '    dt1.Clear()

        '    If Val(NoofComps) > 1 Then

        '        Dim f As New Company_Selection
        '        f.ShowDialog()

        '        If Val(Common_Procedures.CompIdNo) <> 0 Then

        '            da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
        '            dt1 = New DataTable
        '            da.Fill(dt1)

        '            If dt1.Rows.Count > 0 Then
        '                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
        '                    lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
        '                    lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
        '                    Me.Text = Trim(dt1.Rows(0)(1).ToString)
        '                End If
        '            End If
        '            dt1.Clear()
        '            dt1.Dispose()
        '            da.Dispose()

        '            new_record()

        '        Else
        '            Me.Close()

        '        End If

        '    Else

        '        Me.Close()

        '    End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        Dim cDelvCheck_STS As Boolean = True
        Dim Other_Condtn2 As String = ""

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try


            da1 = New SqlClient.SqlDataAdapter("select a.*, c.Checking_Section_Name from Lot_Checking_Plan_Head a INNER JOIN Checking_Section_Head c ON a.Checking_Section_IdNo = c.Checking_Section_IdNo where a.Lot_Checking_Plan_Code ='" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Lot_Checking_Plan_No").ToString
                dtp_Plan_Date.Text = dt1.Rows(0).Item("Lot_Checking_Plan_Date").ToString
                msk_Plan_date.Text = dtp_Plan_Date.Text
                Dtp_Allotment_Date.Text = dt1.Rows(0).Item("Lot_Checking_Allotment_Date").ToString
                msk_Allotment_Date.Text = Dtp_Allotment_Date.Text
                cbo_Checking_Section.Text = dt1.Rows(0).Item("Checking_Section_Name").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.cloth_name , c.Ledger_Name from Lot_Checking_Plan_Details a INNER JOIN Cloth_Head b ON  a.Cloth_IdNo = b.Cloth_IdNo LEFT OUTER JOIN Ledger_Head c ON  a.Party_Idno = c.Ledger_Idno  where a.Lot_Checking_Plan_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Receipt_Date").ToString).Date, "dd-MM-yyyy")
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Lot_No").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("cloth_name").ToString
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("pcs").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Priority").ToString
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Cloth_Receipt_Code").ToString
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("lotcode_forSelection").ToString

                    Next i

                End If

                With dgv_Details_Total

                    Total_Calculation()
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Pcs").ToString), "########0.00")
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")

                End With

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()


            Else

                new_record()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()


            'cbo_Checking_Section.Enabled = False
            'cbo_Checking_Section.BackColor = Color.LightGray


            Grid_Cell_DeSelect()
            dgv_ActCtrlName = ""

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        dgv_ActCtrlName = ""
        If msk_Plan_date.Visible And msk_Plan_date.Enabled Then msk_Plan_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_Lot_Checking_Planing_Entry, New_Entry, Me, con, "Packing_Slip_Head", "Packing_Slip_Code", NewCode, "Packing_Slip_Date", "(Packing_Slip_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        'If Common_Procedures.settings.Vefified_Status = 1 Then
        '    NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '    If Val(Common_Procedures.get_FieldValue(con, "Packing_Slip_Head", "Verified_Status", "(Packing_Slip_Code = '" & Trim(NewCode) & "')")) = 1 Then
        '        MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        Exit Sub
        '    End If
        'End If


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

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        NewCode = Replace(NewCode, "'", "''")


        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "delete from Lot_Checking_Plan_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lot_Checking_Plan_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Lot_Checking_Plan_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lot_Checking_Plan_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_Plan_date.Enabled = True And msk_Plan_date.Visible = True Then msk_Plan_date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            'da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            'da.Fill(dt1)
            'cbo_Filter_Cloth.DataSource = dt1
            'cbo_Filter_Cloth.DisplayMember = "Cloth_Name"

            da = New SqlClient.SqlDataAdapter("select Checking_Section_Name from Checking_Section_head order by Checking_Section_Name", con)
            da.Fill(dt2)
            cbo_Filter_CheckingSection.DataSource = dt2
            cbo_Filter_CheckingSection.DisplayMember = "Checking_Section_Name"


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            'cbo_Filter_Cloth.Text = ""
            cbo_Filter_CheckingSection.Text = ""


            'cbo_Filter_Cloth.SelectedIndex = -1
            cbo_Filter_CheckingSection.SelectedIndex = -1
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
        Dim Other_Condtn2 As String

        Try

            'Other_Condtn2 = "(select count(tsq1.lot_code) from Packing_Slip_Details tsq1 where tsq1.Packing_Slip_Code = a.Packing_Slip_Code and tsq1.lot_code <> '') > 0"

            da = New SqlClient.SqlDataAdapter("select top 1 Lot_Checking_Plan_No from Lot_Checking_Plan_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lot_Checking_Plan_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Lot_Checking_Plan_No", con)


            'da = New SqlClient.SqlDataAdapter("select top 1 a.Packing_Slip_RefNo from Packing_Slip_Head a where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " and " & Other_Condtn2 & " Order by a.for_Orderby, a.Packing_Slip_RefNo, a.Packing_Slip_No", con)
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
        Dim NewCode As String = ""
        Dim MtchSTS As Boolean = False
        Dim BalNo As String = ""
        Dim L As Integer = -1
        Dim Other_Condtn2 As String = ""

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Lot_Checking_Plan_No from Lot_Checking_Plan_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lot_Checking_Plan_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Lot_Checking_Plan_No", con)
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
        Dim NewCode As String = ""
        Dim MtchSTS As Boolean = False
        Dim BalNo As String = ""
        Dim L As Integer = -1
        Dim Other_Condtn2 As String

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Lot_Checking_Plan_No from Lot_Checking_Plan_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lot_Checking_Plan_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby desc, Lot_Checking_Plan_No desc", con)
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
        Dim Other_Condtn2 As String

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Lot_Checking_Plan_No from Lot_Checking_Plan_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lot_Checking_Plan_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby desc, Lot_Checking_Plan_No desc", con)
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

            dtp_Plan_Date.Enabled = True ' False
            msk_Plan_date.Enabled = True ' False

            New_Entry = True

            msk_Plan_date.Text = Date.Today.ToShortDateString
            msk_Allotment_Date.Text = Date.Today.ToShortDateString

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Lot_Checking_Plan_Head", "Lot_Checking_Plan_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            If msk_Plan_date.Enabled And msk_Plan_date.Visible Then msk_Plan_date.Focus() : msk_Plan_date.SelectionStart = 0


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

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")



            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Lot_Checking_Plan_No from Lot_Checking_Plan_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lot_Checking_Plan_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Lot_Checking_Planing_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Lot No.", "FOR NEW REC INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Lot_Checking_Plan_No from Lot_Checking_Plan_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Lot_Checking_Plan_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Dt2 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Clth_ID As Integer = 0
        Dim Clthty_ID As Integer = 0
        Dim dCloTyp_ID As Integer = 0
        Dim dClo_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim vLed_IdNo As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim vTotMtrs As String, vTotPcs As Integer
        Dim party_ID As Integer = 0
        Dim vLmIdNo As Integer = 0
        Dim vLmNo As String = ""
        Dim vChkSec_IdNo As Integer = 0
        Dim Nr As Long = 0
        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""
        Dim cDelvCheck_STS As Boolean = True
        Dim vBARCODE As String = ""
        Dim Led_ID As Integer = 0
        Dim vCloTypMtrs_FldNameVal As String = ""
        Dim vNEWPCNo_OrdBy As String = 0
        Dim vTOT_BEAM_CONS_MTRS As String = 0
        Dim vOLD_PC_BEAM_CONS_MTRS As String = 0
        Dim vNEW_PC_BEAM_CONS_MTRS As String = 0
        Dim vTOT_CHK_MTRS As String = 0
        Dim vNEW_PCS_TYP1MTRS As String = 0
        Dim vNEW_PCS_TYP2MTRS As String = 0
        Dim vNEW_PCS_TYP3MTRS As String = 0
        Dim vNEW_PCS_TYP4MTRS As String = 0
        Dim vNEW_PCS_TYP5MTRS As String = 0
        Dim vBALE_PCS_MTRS As String = 0
        Dim vBALE_PCS_WGT As String = 0
        Dim vPCS_WidthType As String = ""
        Dim vPCS_CrimpPerc As String = 0
        Dim vPCS_BeamNo1 As String = ""
        Dim vPCS_BeamNo2 As String = ""
        Dim vPCS_EXCSHT_Mtr As String = 0
        Dim vPCS_ACTMtr As String = 0
        Dim vERRMSG As String = ""
        Dim vFAB_LOTCODE As String = ""

        cmd.Connection = con

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Lot_Checking_Planing_Entry, New_Entry, Me, con, "Packing_Slip_Head", "Packing_Slip_Code", NewCode, "Packing_Slip_Date", "(Packing_Slip_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Packing_Slip_No desc", dtp_Plan_Date.Value.Date) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Plan_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Plan_date.Enabled And msk_Plan_date.Visible Then msk_Plan_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Plan_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Plan_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Plan_date.Enabled And msk_Plan_date.Visible Then msk_Plan_date.Focus()
            Exit Sub
        End If


        vChkSec_IdNo = Common_Procedures.CheckingSection_NameToIdNo(con, cbo_Checking_Section.Text)

        Total_Calculation()

        vTotMtrs = 0 : vTotPcs = 0
        If dgv_Details_Total.RowCount > 0 Then

            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(5).Value)
            vTotMtrs = Format(Val(dgv_Details_Total.Rows(0).Cells(6).Value), "#########0.00")

        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then

                NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PlanDate", Convert.ToDateTime(msk_Plan_date.Text))
            cmd.Parameters.AddWithValue("@AllotmentDate", Convert.ToDateTime(msk_Allotment_Date.Text))

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)



            If New_Entry = True Then

                cmd.CommandText = "Insert into Lot_Checking_Plan_Head( Lot_Checking_Plan_Code ,              Company_IdNo         ,       Lot_Checking_Plan_No      ,             for_OrderBy       , Lot_Checking_Plan_Date , Lot_Checking_Allotment_Date ,       Checking_Section_IdNo      ,          Total_Meters       ,            Total_Pcs      )" &
                                  "Values                            ('" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_RefNo.Text) & "'  ,    " & Str(Val(vOrdByNo)) & " ,        @PlanDate       ,         @AllotmentDate      ,  " & Str(Val(vChkSec_IdNo)) & "  , " & Str(Val(vTotMtrs)) & "  , " & Str(Val(vTotPcs)) & " )   "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Lot_Checking_Plan_Head set Lot_Checking_Plan_Date = @PlanDate , Lot_Checking_Allotment_Date = @AllotmentDate , Checking_Section_IdNo =  " & Str(Val(vChkSec_IdNo)) & " , Total_Pcs =  " & Str(Val(vTotPcs)) & ", Total_Meters = " & Str(Val(vTotMtrs)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And Lot_Checking_Plan_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Lot_Checking_Plan_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Lot_Checking_Plan_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0


                For i = 0 To .RowCount - 1



                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        party_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        dClo_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)

                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@PlanDate", Convert.ToDateTime(msk_Plan_date.Text))
                        cmd.Parameters.AddWithValue("@RecDate", Convert.ToDateTime(.Rows(i).Cells(1).Value))
                        cmd.Parameters.AddWithValue("@AllotmentDate", Convert.ToDateTime(msk_Allotment_Date.Text))

                        Sno = Sno + 1

                        'cmd.CommandText = "Insert into Packing_Slip_Details (   Packing_Slip_Code   ,              Company_IdNo        ,            Packing_Slip_No     ,            for_OrderBy    , Packing_Slip_Date,          Cloth_IdNo      ,                  Folding           ,           Sl_No      ,                     Lot_No              ,                    Pcs_No              ,           ClothType_IdNo    ,                Meters           ,              Weight            ,                      Weight_Meter        ,             Party_IdNo     ,                    Lot_Code             ,             Loom_IdNo     ,          Loom_No     ,        ExcSht_Meters             ,     Piece_Actual_Meters       ) " &
                        '                        "          Values               ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BaleNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @EntryDate    , " & Str(Val(dClo_ID)) & ",  " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(dCloTyp_ID)) & ", " & Str(Val(vBALE_PCS_MTRS)) & ", " & Str(Val(vBALE_PCS_WGT)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(party_ID)) & " , '" & Trim(.Rows(i).Cells(8).Value) & "' , " & Str(Val(vLmIdNo)) & " , '" & Trim(vLmNo) & "', " & Str(Val(vPCS_EXCSHT_Mtr)) & ", " & Str(Val(vPCS_ACTMtr)) & " ) "
                        'cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Lot_Checking_Plan_Details (  Lot_Checking_Plan_Code    ,            Company_IdNo           ,        Lot_Checking_Plan_No     ,          for_OrderBy        , Lot_Checking_Plan_Date ,       Sl_No           ,  Receipt_Date ,                 Lot_No                  ,          Party_IdNo        ,           Cloth_IdNo       ,              Pcs                       ,             Meters                   ,            Priority                  ,             Cloth_Receipt_Code          ,         lotcode_forSelection           ,  Checking_Section_Idno             ,  Allotment_Date )" &
                                                "          Values                    ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & " ,  '" & Trim(lbl_RefNo.Text) & "' , " & Str(Val(vOrdByNo)) & "  ,          @PlanDate     , " & Str(Val(Sno)) & " ,    @RecDate   , '" & Trim(.Rows(i).Cells(2).Value) & "' , " & Str(Val(party_ID)) & " ,  " & Str(Val(dClo_ID)) & " ,   " & Val(.Rows(i).Cells(5).Value) & " , " & Val(.Rows(i).Cells(6).Value) & " , " & Val(.Rows(i).Cells(7).Value) & " , '" & Trim(.Rows(i).Cells(8).Value) & "' , '" & Trim(.Rows(i).Cells(9).Value) & "',    " & Str(Val(vChkSec_IdNo)) & "  ,   @AllotmentDate  ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Packing_Slip_Details", "Packing_Slip_Code", Val(lbl_Company.Tag), NewCode, lbl_BaleNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, False, "Lot_No,Pcs_No,ClothType_IdNo,Meters,Weight,Weight_Meter,Party_IdNo,Lot_Code,Loom_IdNo,Loom_No", "Sl_No", "Packing_Slip_Code, For_OrderBy, Company_IdNo, Packing_Slip_No, Packing_Slip_Date, Ledger_Idno", tr)

            End With

            tr.Commit()

            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()
            cmd.Dispose()

            If msk_Plan_date.Enabled And msk_Plan_date.Visible Then msk_Plan_date.Focus()

        End Try

    End Sub


    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotMtrs As String = 0, TotPcs As Integer = 0, TotWgt As String = 0, TotLot As Integer = 0
        Dim vNEW_PCSMTR As String
        Dim vTOT_PACKMTR As String
        Dim vTOT_NEWPCSMTR As String
        Dim vTOT_EXCSHTMTR As String
        Dim vTOT_PCSMTR As String
        Dim vPCS_NETMTR As String


        If FrmLdSTS = True Then Exit Sub

        NoCalc_Status = True

        Sno = 0
        TotPcs = 0
        TotMtrs = 0
        TotWgt = 0
        TotLot = 0
        vNEW_PCSMTR = 0
        vTOT_PACKMTR = 0
        vTOT_NEWPCSMTR = 0
        vTOT_EXCSHTMTR = 0
        vTOT_PCSMTR = 0
        vPCS_NETMTR = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotLot = TotLot + 1
                    TotPcs = Format(Val(TotPcs) + Val(.Rows(i).Cells(5).Value), "########0.00")
                    TotMtrs = Format(Val(TotMtrs) + Val(.Rows(i).Cells(6).Value), "########0.00")

                End If

            Next



        End With

        With dgv_Details_Total

            If .RowCount = 0 Then .Rows.Add()

            .Rows(0).Cells(2).Value = Val(TotLot)
            .Rows(0).Cells(5).Value = Format(Val(TotPcs), "########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotMtrs), "########0.00")

        End With



        NoCalc_Status = False

    End Sub

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim vCondt As String = ""
        If Trim(UCase(vEntryType)) = "BALEGROUPWISE" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" Then
                vCondt = "(ClothSet_IdNo = " & Str(Val(vEntry_BaleGroupIdNo)) & ")"
            End If
        End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", vCondt, "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim vCondt As String = ""
        If Trim(UCase(vEntryType)) = "BALEGROUPWISE" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" Then
                vCondt = "(ClothSet_IdNo = " & Str(Val(vEntry_BaleGroupIdNo)) & ")"
            End If
        End If
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, cbo_Godown_StockIN, cbo_Checking_Section, "Cloth_Head", "Cloth_Name", vCondt, "(Cloth_IdNo = 0)")
        'If e.KeyCode = 38 And cbo_Cloth.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
        '    If cbo_Godown_StockIN.Visible = True Then
        '        cbo_Godown_StockIN.Focus()
        '    ElseIf cbo_PartyName_StockOF.Visible = True Then
        '        cbo_PartyName_StockOF.Focus()
        '    Else
        '        msk_Plan_date.Focus()
        '    End If
        'End If

    End Sub

    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Clo_IdNo As Integer
        Dim vCondt As String = ""
        Dim vRollTube_Wgt As String = ""

        If Trim(UCase(vEntryType)) = "BALEGROUPWISE" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" Then
                vCondt = "(ClothSet_IdNo = " & Str(Val(vEntry_BaleGroupIdNo)) & ")"
            End If
        End If

        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, Nothing, "Cloth_Head", "Cloth_Name", vCondt, "(Cloth_IdNo = 0)")

        'If Asc(e.KeyChar) = 13 Then
        '    cbo_Checking_Section.Focus()
        '    Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        '    vRollTube_Wgt = 0

        '    vRollTube_Wgt = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "RollTube_Wgt", "(cloth_idno = " & Str(Val(Clo_IdNo)) & " )"))
        '    txt_Tare_weight.Text = vRollTube_Wgt

        'End If

    End Sub

    Private Sub cbo_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.Control = False And e.KeyValue = 17 Then
        '    Dim f As New Cloth_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_Cloth.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""
        '    f.MdiParent = MDIParent1
        '    f.Show()

        'End If
    End Sub

    Private Sub cbo_ClothType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Checking_Section.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Checking_Section_Head", "Checking_Section_Name", "", "Checking_Section_Name")

    End Sub
    Private Sub cbo_ClothType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Checking_Section.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Checking_Section, msk_Allotment_Date, btn_ClothReceipt_Selection, "Checking_Section_Head", "Checking_Section_Name", "", "Checking_Section_Name")

    End Sub

    Private Sub cbo_Checking_Section_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Checking_Section.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Checking_Section, btn_ClothReceipt_Selection, "Checking_Section_Head", "Checking_Section_Name", "", "Checking_Section_Name")

    End Sub

    Private Sub cbo_Bale_Bundle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Bale_Bundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Bale_Bundle, cbo_Checking_Section, txt_Folding, "", "", "", "")

    End Sub

    Private Sub cbo_Bale_Bundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Bale_Bundle, txt_Folding, "", "", "", "")
    End Sub

    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            e.Handled = True
            If MessageBox.Show("Do you want to select Piece", "FOR PIECE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)
            Else
                If dgv_Details.Rows.Count > 0 And dgv_Details.Columns(11).Visible = True And dgv_Details.Columns(11).ReadOnly = False Then
                    '    dgv_Details.Focus()
                    '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(11)
                    '    dgv_Details.CurrentCell.Selected = True
                    'ElseIf txt_net_weight.Enabled = True And txt_net_weight.Visible = True Then
                    '    txt_net_weight.Focus()
                    'ElseIf txt_Tare_weight.Enabled = True And txt_Tare_weight.Visible = True Then
                    '    txt_Tare_weight.Focus()
                    'ElseIf txt_Note.Enabled And txt_Note.Visible Then
                    '    txt_Note.Focus()
                    'Else
                    '    msk_Plan_date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Piece", "FOR PIECE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)
            Else
                If dgv_Details.Rows.Count > 0 And dgv_Details.Columns(11).Visible = True And dgv_Details.Columns(11).ReadOnly = False Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(11)
                    dgv_Details.CurrentCell.Selected = True
                    'ElseIf txt_net_weight.Enabled = True And txt_net_weight.Visible = True Then
                    '    txt_net_weight.Focus()
                    'ElseIf txt_Tare_weight.Enabled = True And txt_Tare_weight.Visible = True Then
                    '    txt_Tare_weight.Focus()
                    'ElseIf txt_Note.Enabled And txt_Note.Visible Then
                    '    txt_Note.Focus()
                Else
                    msk_Plan_date.Focus()
                End If
            End If
            'If dgv_Details.Rows.Count > 0 Then
            '    dgv_Details.Focus()
            '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            'Else
            '    txt_Note.Focus()

            'End If
        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If (e.KeyValue = 40) Then
            'If cbo_Sales_Party_Name.Visible = True Then
            '    cbo_Sales_Party_Name.Focus()
            'ElseIf MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_Plan_date.Focus()
            'End If
        End If
        If (e.KeyValue = 38) Then
            'txt_Folding.Focus()
        End If

    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            'If cbo_Sales_Party_Name.Visible = True Then
            '    cbo_Sales_Party_Name.Focus()
            'ElseIf MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_Plan_date.Focus()
            'End If
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit

        If dgv_Details.CurrentCell.ColumnIndex = 2 Or dgv_Details.CurrentCell.ColumnIndex = 4 Or dgv_Details.CurrentCell.ColumnIndex = 5 Then
            Total_Calculation()
        End If

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 6 And .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim vNEW_PCSMTR As String

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 14 Then



                            '------ ROW CALCULATION in  TOTAL_CALCULATION()

                            'If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 14 Then

                            '    If .Columns(11).Visible = True And .Columns(13).Visible = True Then

                            '        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 11 Then
                            '            vNEW_PCSMTR = 0
                            '            If Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) > 0 Then
                            '                If Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) < Val(.Rows(.CurrentCell.RowIndex).Cells(4).Value) Then
                            '                    vNEW_PCSMTR = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(4).Value) - Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value), "########0.00")
                            '                End If
                            '            End If
                            '            .Rows(.CurrentCell.RowIndex).Cells(13).Value = Format(Val(vNEW_PCSMTR), "########0.00")
                            '        End If

                            '        If .Columns(11).Visible = True And Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) > 0 Then
                            '            .Rows(.CurrentCell.RowIndex).Cells(15).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(14).Value), "########0.00")
                            '        Else
                            '            .Rows(.CurrentCell.RowIndex).Cells(15).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(4).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(14).Value), "########0.00")
                            '        End If

                            '    End If

                            'End If

                            Total_Calculation()

                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '---

        End Try
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    'txt_Folding.Focus()
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 0 Then
                    .CurrentCell.Selected = False
                    'txt_Folding.Focus()
                    'SendKeys.Send("{RIGHT}")
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    'txt_Note.Focus()

                Else
                    SendKeys.Send("{Tab}")

                End If


            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                .Rows.RemoveAt(.CurrentRow.Index)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

                Total_Calculation()

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
    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_ActCtrlName = dgv_Details.Name
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 11 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                ElseIf .CurrentCell.ColumnIndex = 14 Then

                    If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If

            End If
        End With
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
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""
        Dim EdsCnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Other_Condtn2 As String

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0
            EdsCnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Lot_Checking_Plan_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Lot_Checking_Plan_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Lot_Checking_Plan_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            'If Trim(cbo_Filter_Cloth.Text) <> "" Then
            '    Led_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Cloth.Text)
            'End If

            If Trim(cbo_Filter_CheckingSection.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.CheckingSection_NameToIdNo(con, cbo_Filter_CheckingSection.Text)
            End If



            'If Val(Led_IdNo) <> 0 Then
            '    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Cloth_IdNo = " & Str(Val(Led_IdNo))
            'End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Checking_Section_IdNo = " & Str(Val(Cnt_IdNo))
            End If

            'Other_Condtn2 = "(select count(tsq1.lot_code) from Packing_Slip_Details tsq1 where tsq1.Packing_Slip_Code = a.Packing_Slip_Code and tsq1.lot_code <> '') > 0"
            'Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & Other_Condtn2


            da = New SqlClient.SqlDataAdapter("select a.* from Lot_Checking_Plan_Head a  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Lot_Checking_Plan_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & "  Order by a.for_orderby, a.Lot_Checking_Plan_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.* , b.Cloth_name from Packing_Slip_Head a Inner join Cloth_Head b on a.cloth_idno = b.cloth_idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and  " & Other_Condition & IIf(Trim(Condt) <> "", " and ", "") & Condt & "  Order by a.for_orderby, a.Packing_Slip_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Lot_Checking_Plan_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Lot_Checking_Plan_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Lot_Checking_Allotment_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Common_Procedures.CheckingSection_IdNoToName(con, dt2.Rows(i).Item("Checking_Section_idno").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Total_Pcs").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")


                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub


    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress

        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_CheckingSection.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, dtp_Filter_ToDate, cbo_Filter_CheckingSection, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, cbo_Filter_CheckingSection, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    'Private Sub cbo_Filter_ClothType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CheckingSection.GotFocus
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Checking_Section_Head", "Checking_Section_Head", "", "Checking_Section_Head")

    'End Sub

    Private Sub cbo_Filter_ClothType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CheckingSection.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CheckingSection, dtp_Filter_ToDate, btn_Filter_Show, "Checking_Section_Head", "Checking_Section_Name", "", "Checking_Section_Name")

    End Sub

    Private Sub cbo_Filter_ClothType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CheckingSection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CheckingSection, btn_Filter_Show, "Checking_Section_Head", "Checking_Section_Name", "", "Checking_Section_Name")

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

    'Private Sub chk_PcsSelection_SelectAllPcs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim i As Integer
    '    Dim J As Integer
    '    Dim v1stVisiRow As Integer = 0

    '    With dgv_Selection

    '        For i = 0 To .Rows.Count - 1
    '            If .Rows(i).Visible = True Then
    '                .Rows(i).Cells(8).Value = ""
    '                For J = 0 To .ColumnCount - 1
    '                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
    '                Next J
    '            End If
    '        Next i

    '        v1stVisiRow = -1
    '        If chk_PcsSelection_SelectAllPcs.Checked = True Then
    '            For i = 0 To .Rows.Count - 1
    '                If .Rows(i).Visible = True Then
    '                    Select_Piece(i)
    '                    If v1stVisiRow = -1 Then v1stVisiRow = i
    '                End If
    '            Next i
    '        End If

    '        Total_PieceSelection_Calculation()

    '        If .Rows.Count > 0 Then

    '            If v1stVisiRow >= 0 Then
    '                .Focus()
    '                .CurrentCell = .Rows(v1stVisiRow).Cells(0)
    '                .CurrentCell.Selected = True
    '            Else
    '                txt_PcsSelection_LotNo.Focus()
    '            End If

    '        End If

    '    End With

    'End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ClothReceipt_Selection.Click
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String = ""
        Dim PcsChk_Condt As String = ""
        Dim Fldng As Single = 0
        Dim vLomTypeCondt As String = ""
        Dim vPCSCHK_APPSTS_JOIN As String = ""
        Dim SQL1 As String = ""

        'LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        'If LedIdNo = 0 Then
        '    MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus()
        '    Exit Sub
        'End If

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With Dgv_ClothReceipt_Selection

            .Rows.Clear()
            SNo = 0

            Cmd.Connection = con
            Cmd.CommandTimeout = 1000

            SQL1 = "Select a.Weaver_ClothReceipt_Date,a.Lot_No,a.Ledger_Idno,a.Total_Receipt_Pcs,a.noof_pcs,a.ReceiptMeters_Wages,a.ReceiptMeters_Receipt,a.Weaver_ClothReceipt_Code,a.lotCode_ForSelection, c.Cloth_Name from Weaver_Cloth_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo INNER JOIN Lot_Checking_Plan_Details d ON d.Lot_Checking_Plan_Code = '" & Trim(NewCode) & "' and A.lotcode_forSelection = d.lotcode_forSelection where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " order by a.Weaver_ClothReceipt_Date desc, a.for_orderby desc, a.Weaver_ClothReceipt_No desc"
            Cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            Da = New SqlClient.SqlDataAdapter(Cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_ClothReceipt_Date").ToString).Date, "dd-MM-yyyy")
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Lot_No").ToString
                    .Rows(n).Cells(3).Value = Common_Procedures.Ledger_IdNoToName(con, Dt1.Rows(i).Item("Ledger_Idno").ToString)
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    If Val(Dt1.Rows(i).Item("Total_Receipt_Pcs").ToString) <> 0 Then
                        .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Total_Receipt_Pcs").ToString)
                    Else
                        .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    End If

                    If Val(Dt1.Rows(i).Item("ReceiptMeters_Wages").ToString) <> 0 Then
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("ReceiptMeters_Wages").ToString), "########0.000")
                    Else
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.000")
                    End If

                    .Rows(n).Cells(7).Value = "1"

                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("lotCode_ForSelection").ToString


                    For j = 0 To .ColumnCount - 1
                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()


            Cmd.Connection = con
            Cmd.CommandTimeout = 1000
            Cmd.CommandType = CommandType.StoredProcedure
            Cmd.CommandText = "sp_get_lotcheckingplandetails_for_selection"
            Cmd.Parameters.Clear()
            Cmd.Parameters.Add("@compidno", SqlDbType.Int)
            Cmd.Parameters("@compidno").Value = Val(lbl_Company.Tag)
            Cmd.Parameters.Add("@LotChecking_PlanCode", SqlDbType.VarChar)
            Cmd.Parameters("@LotChecking_PlanCode").Value = Trim(NewCode)

            'SQL1 = "Select a.Weaver_ClothReceipt_Date,a.Lot_No,a.Ledger_Idno,a.Total_Receipt_Pcs,a.noof_pcs,a.ReceiptMeters_Wages,a.ReceiptMeters_Receipt,a.Weaver_ClothReceipt_Code,a.lotCode_ForSelection, c.Cloth_Name from Weaver_Cloth_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '' and a.Lot_Approved_Status = 1 and A.lotcode_forSelection NOT IN (SELECT SQ1.lotcode_forSelection FROM Lot_Checking_Plan_Details SQ1 where sq1.Lot_Checking_Plan_Code <> '" & Trim(NewCode) & "') and A.lotcode_forSelection NOT IN (SELECT SQ3.lotcode_forSelection FROM LotAllotment_Head SQ3)  order by a.Weaver_ClothReceipt_Date desc, a.for_orderby desc, a.Weaver_ClothReceipt_No desc"
            'SQL1 = "Select a.Weaver_ClothReceipt_Date,a.Lot_No,a.Ledger_Idno,a.Total_Receipt_Pcs,a.noof_pcs,a.ReceiptMeters_Wages,a.ReceiptMeters_Receipt,a.Weaver_ClothReceipt_Code,a.lotCode_ForSelection, c.Cloth_Name from Weaver_Cloth_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '' and a.Lot_Approved_Status = 1 and A.lotcode_forSelection NOT IN (SELECT SQ1.lotcode_forSelection FROM Lot_Checking_Plan_Details SQ1 where sq1.Lot_Checking_Plan_Code <> '" & Trim(NewCode) & "') and A.lotcode_forSelection NOT IN (SELECT SQ3.lotcode_forSelection FROM LotAllotment_Head SQ3) "
            'SQL1 = "Select a.Weaver_ClothReceipt_Date,a.Lot_No,a.Ledger_Idno,a.Total_Receipt_Pcs,a.noof_pcs,a.ReceiptMeters_Wages,a.ReceiptMeters_Receipt,a.Weaver_ClothReceipt_Code,a.lotCode_ForSelection, c.Cloth_Name from Weaver_Cloth_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '' and (A.lotcode_forSelection NOT IN (SELECT SQ1.lotcode_forSelection FROM Lot_Checking_Plan_Details SQ1 where sq1.Lot_Checking_Plan_Code <> '" & Trim(NewCode) & "')) and  (A.lotcode_forSelection  IN (SELECT SQ2.lotcode_forSelection FROM Lot_Approved_Head SQ2)) and (A.lotcode_forSelection NOT IN (SELECT SQ3.lotcode_forSelection FROM LotAllotment_Head SQ3)) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No"
            'SQL1 = "Select a.Weaver_ClothReceipt_Date,a.Lot_No,a.Ledger_Idno,a.Total_Receipt_Pcs,a.noof_pcs,a.ReceiptMeters_Wages,a.ReceiptMeters_Receipt,a.Weaver_ClothReceipt_Code,a.lotCode_ForSelection, c.Cloth_Name from Weaver_Cloth_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " AND A.lotcode_forSelection NOT IN (SELECT SQ1.lotcode_forSelection FROM Lot_Checking_Plan_Details SQ1 where sq1.Lot_Checking_Plan_Code <> '" & Trim(NewCode) & "') and  A.lotcode_forSelection  IN (SELECT SQ2.lotcode_forSelection FROM Lot_Approved_Head SQ2)  order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No"

            'SQL1 = "Select a.Weaver_ClothReceipt_Date,a.Lot_No,a.Ledger_Idno,a.Total_Receipt_Pcs,a.noof_pcs,a.ReceiptMeters_Wages,a.ReceiptMeters_Receipt,a.Weaver_ClothReceipt_Code,a.lotCode_ForSelection, c.Cloth_Name from Weaver_Cloth_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " AND A.lotcode_forSelection NOT IN (SELECT SQ1.lotcode_forSelection FROM Lot_Checking_Plan_Details SQ1 where sq1.Lot_Checking_Plan_Code <> '" & Trim(NewCode) & "') and  A.lotcode_forSelection  IN (SELECT SQ2.lotcode_forSelection FROM Lot_Approved_Head SQ2) and A.lotcode_forSelection NOT IN (SELECT SQ3.lotcode_forSelection FROM LotAllotment_Head SQ3) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No"
            'Cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            Da = New SqlClient.SqlDataAdapter(Cmd)
            'Da = New SqlClient.SqlDataAdapter("Select a.Weaver_ClothReceipt_Date, c.Cloth_Name from Weaver_Cloth_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " AND A.lotcode_forSelection NOT IN (SELECT SQ1.lotcode_forSelection FROM Lot_Checking_Plan_Details SQ1 where sq1.Lot_Checking_Plan_Code <> '" & Trim(NewCode) & "') and  A.lotcode_forSelection  IN (SELECT SQ2.lotcode_forSelection FROM Lot_Approved_Head SQ2) and A.lotcode_forSelection NOT IN (SELECT SQ3.lotcode_forSelection FROM LotAllotment_Head SQ3) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_ClothReceipt_Date").ToString).Date, "dd-MM-yyyy")
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Lot_No").ToString
                    .Rows(n).Cells(3).Value = Common_Procedures.Ledger_IdNoToName(con, Dt1.Rows(i).Item("Ledger_Idno").ToString)
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    If Val(Dt1.Rows(i).Item("Total_Receipt_Pcs").ToString) <> 0 Then
                        .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Total_Receipt_Pcs").ToString)
                    Else
                        .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    End If

                    If Val(Dt1.Rows(i).Item("ReceiptMeters_Wages").ToString) <> 0 Then
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("ReceiptMeters_Wages").ToString), "########0.000")
                    Else
                        .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.000")
                    End If

                    .Rows(n).Cells(7).Value = ""

                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("lotCode_ForSelection").ToString


                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Blue
                    Next

                Next

            End If
            Dt1.Clear()

        End With

        Pnl_ClothReceipt_Selection.Visible = True
        Pnl_Back.Enabled = False
        Dgv_ClothReceipt_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        'Select_Piece(e.RowIndex)
    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Dim n As Integer

        'On Error Resume Next

        'If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
        '    If dgv_Selection.CurrentCell.RowIndex >= 0 Then
        '        n = dgv_Selection.CurrentCell.RowIndex
        '        Select_Piece(n)
        '        e.Handled = True
        '    End If
        'End If

    End Sub

    'Private Sub Select_Piece(ByVal RwIndx As Integer)
    '    Dim i As Integer

    '    With dgv_Selection

    '        If .RowCount > 0 And RwIndx >= 0 Then

    '            .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

    '            If Val(.Rows(RwIndx).Cells(8).Value) = 1 Then

    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
    '                Next
    '                dgv_Selection.DefaultCellStyle.SelectionForeColor = Color.Red

    '            Else

    '                .Rows(RwIndx).Cells(8).Value = ""

    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
    '                Next
    '                dgv_Selection.DefaultCellStyle.SelectionForeColor = Color.Blue

    '            End If

    '        End If

    '        Total_PieceSelection_Calculation()

    '    End With

    'End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Piece_Selection()
    End Sub

    'Private Sub Piece_Selection()
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim n As Integer = 0
    '    Dim sno As Integer = 0
    '    Dim i As Integer = 0
    '    Dim SQL1 As String
    '    Dim NewCode As String
    '    Dim vCLOTHTYP_ID As String = 0

    '    NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    cmd.Connection = con
    '    cmd.CommandTimeout = 600

    '    dgv_Details.Rows.Clear()

    '    sno = 0
    '    For i = 0 To dgv_Selection.RowCount - 1

    '        If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

    '            n = dgv_Details.Rows.Add()

    '            sno = sno + 1
    '            dgv_Details.Rows(n).Cells(0).Value = sno
    '            dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
    '            dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
    '            dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
    '            dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(4).Value
    '            If Val(dgv_Selection.Rows(i).Cells(5).Value) <> 0 Then
    '                dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(5).Value
    '            End If
    '            If Val(dgv_Selection.Rows(i).Cells(6).Value) <> 0 Then
    '                dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
    '            End If
    '            dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(7).Value
    '            dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(9).Value
    '            dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(10).Value
    '            dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(11).Value

    '            If dgv_Details.Columns(14).Visible = True Then

    '                vCLOTHTYP_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(n).Cells(3).Value)

    '                SQL1 = "Select ExcSht_Meters from Packing_Slip_Details Where Packing_Slip_Code = '" & Trim(NewCode) & "' and lot_code = '" & Trim(dgv_Details.Rows(n).Cells(8).Value) & "' and Pcs_No = '" & Trim(dgv_Details.Rows(n).Cells(2).Value) & "' and ClothType_IdNo = " & Str(Val(vCLOTHTYP_ID))
    '                cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
    '                da1 = New SqlClient.SqlDataAdapter(cmd)
    '                dt2 = New DataTable
    '                da1.Fill(dt2)
    '                If dt2.Rows.Count > 0 Then
    '                    If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
    '                        If Val(dt2.Rows(0)(0).ToString) <> 0 Then
    '                            dgv_Details.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(0)(0).ToString), "##########0.00")
    '                        End If
    '                    End If
    '                End If
    '                dt2.Clear()

    '            End If

    '        End If

    '    Next i

    '    Total_Calculation()

    '    Pnl_Back.Enabled = True
    '    pnl_Selection.Visible = False

    '    If dgv_Details.Rows.Count > 0 And dgv_Details.Columns(11).Visible = True And dgv_Details.Columns(11).ReadOnly = False Then
    '        dgv_Details.Focus()
    '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(11)
    '        dgv_Details.CurrentCell.Selected = True
    '        'ElseIf txt_net_weight.Enabled = True And txt_net_weight.Visible = True Then
    '        '    txt_net_weight.Focus()
    '        'ElseIf txt_Tare_weight.Enabled = True And txt_Tare_weight.Visible = True Then
    '        '    txt_Tare_weight.Focus()
    '        'ElseIf txt_Note.Enabled And txt_Note.Visible Then
    '        '    txt_Note.Focus()
    '    Else
    '        msk_Plan_date.Focus()
    '    End If


    'End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        EMAIL_Status = False
        WHATSAPP_Status = False
        print_record()
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        Pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Lot_Checking_Planing_Entry, New_Entry) = False Then Exit Sub

        Prn_BarcodeSticker = False

        pnl_Print.Visible = True
        Pnl_Back.Enabled = False
        txt_PrintFrom.Text = lbl_RefNo.Text
        txt_PrintTo.Text = lbl_RefNo.Text
        If txt_PrintFrom.Enabled And txt_PrintFrom.Visible Then
            txt_PrintFrom.Focus()
            txt_PrintFrom.SelectAll()
        End If
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Ok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Ok.Click
        vPRN_Weight_Column_Status = False
        Printing_Bale()
    End Sub

    Private Sub txt_PrintFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintFrom.KeyDown
        If e.KeyCode = Keys.Down Then
            txt_PrintTo.Focus()
        End If
    End Sub

    Private Sub txt_PrintTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintTo.KeyDown
        If e.KeyCode = Keys.Down Then
            btn_Print_Ok.Focus()
        End If
        If e.KeyCode = Keys.Up Then
            txt_PrintFrom.Focus()
        End If
    End Sub

    Private Sub txt_PrintTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PrintTo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Printing_Bale()
        End If
    End Sub

    Public Sub Printing_Bale()
        Dim prtFrm As String = 0
        Dim prtTo As String = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize
        Dim Def_PrntrNm As String = ""
        Dim vFILNm As String = ""
        Dim vFLPATH As String = ""
        Dim vPDFFLPATH_and_NAME As String = ""
        Dim vPRNTRNAME As String
        Dim vPARTYNM As String = ""
        Dim v1st_CLONM As String = ""

        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub

        prtFrm = Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text)
        prtTo = Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text)

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Str(Val(prtTo))

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If

        Try

            vPARTYNM = ""
            v1st_CLONM = ""
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Packing_Slip_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & IIf(Trim(Condt) <> "", " and ", "") & Condt, con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("No Entry Found", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            Else

                vPARTYNM = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Sales_Party_idno").ToString))
                If IsDBNull(dt1.Rows(0).Item("Cloth_Name").ToString) = False Then
                    v1st_CLONM = dt1.Rows(0).Item("Cloth_Name").ToString
                End If

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If Prn_BarcodeSticker = True Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 2X4", 400, 200)
            'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 2X4", 200, 400)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            'e.PageSettings.PaperSize = pkCustomSize1

        Else

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    'e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        End If



        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            If Print_PDF_Status = True Then

                vFLPATH = ""
                vPRNTRNAME = Common_Procedures.get_PDF_PrinterName(EMAIL_Status, WHATSAPP_Status, vFLPATH)

                If Trim(vPRNTRNAME) = "" Then
                    Exit Sub
                End If

                Def_PrntrNm = PrintDocument1.PrinterSettings.PrinterName

                vPARTYNM = Common_Procedures.Replace_SpecialCharacters_With_UnderScore(vPARTYNM)

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION)
                    vFILNm = Trim("PackingList_" & Trim(lbl_RefNo.Text) & "_" & Trim(vPARTYNM) & ".pdf")
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1489" Then '-----K V P WEAVES (ANNUR)
                    v1st_CLONM = Common_Procedures.Replace_SpecialCharacters_With_UnderScore(v1st_CLONM)
                    vFILNm = Trim("PackingList_" & Trim(lbl_RefNo.Text) & "_" & Trim(vPARTYNM) & "_" & Trim(v1st_CLONM) & ".pdf")
                Else
                    vFILNm = Trim("PackingList_" & Trim(lbl_RefNo.Text) & ".pdf")
                End If
                'vFILNm = Trim("PackingList_" & Trim(lbl_BaleRefNo.Text) & ".pdf")

                vFILNm = StrConv(vFILNm, vbProperCase)
                vPDFFLPATH_and_NAME = Trim(vFLPATH) & "\" & Trim(vFILNm)
                vEMAIL_Attachment_FileName = Trim(vPDFFLPATH_and_NAME)

                PrintDocument1.DocumentName = Trim(vFILNm)
                PrintDocument1.PrinterSettings.PrinterName = Trim(vPRNTRNAME)    ' "Microsoft Print to PDF"
                'PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                PrintDocument1.PrinterSettings.PrintToFile = True
                PrintDocument1.PrinterSettings.PrintFileName = Trim(vPDFFLPATH_and_NAME)
                PrintDocument1.Print()

                PrintDocument1.PrinterSettings.PrinterName = Trim(Def_PrntrNm)
                Print_PDF_Status = False


            Else

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
                    MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If


        Else


            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1.0

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        Pnl_Back.Enabled = True
        pnl_Print.Visible = False
        Print_PDF_Status = False

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim prtFrm As String = 0
        Dim prtTo As String = 0
        Dim Condt As String = ""
        Dim Clthname As String = ""
        Dim vSNO As Integer = 0

        vSNO = 0
        prn_HeadIndx = 0


        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub

        prtFrm = Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text)
        prtTo = Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text)

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Str(Val(prtTo))

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If

        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 1
        prn_DetIndx = 0
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1438" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1487" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1489" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1520" Then
            If vPRN_Weight_Column_Status = True Then
                prn_DetIndx = 0
            Else
                prn_DetIndx = 1
            End If

        End If
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_Count = 1
        prn_DetBarCdStkr = 1

        Erase prn_DetAr
        Erase prn_HdAr

        prn_HdAr = New String(1000, 10) {}

        prn_DetAr = New String(1000, 1000, 10) {}

        Erase prn_DetAr1
        prn_DetAr1 = New String(1000, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, tZ.*, c.Cloth_Name, c.Cloth_Description, tL.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code  from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno  LEFT OUTER JOIN State_Head Csh ON tZ.Company_State_IdNo = Csh.State_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  LEFT OUTER JOIN Ledger_Head tL ON a.Sales_Party_idno = tL.Ledger_IdNo Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and " & Trim(Other_Condition) & IIf(Trim(Condt) <> "", " and ", "") & Condt & " order by a.for_orderby, a.Packing_Slip_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                For i = 0 To prn_HdDt.Rows.Count - 1

                    prn_HdMxIndx = prn_HdMxIndx + 1

                    prn_HdAr(prn_HdMxIndx, 1) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)

                    If Trim(prn_HdDt.Rows(i).Item("Cloth_Description").ToString) <> "" Then
                        Clthname = Trim(prn_HdDt.Rows(i).Item("Cloth_Description").ToString)

                    Else
                        Clthname = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)
                    End If

                    prn_HdAr(prn_HdMxIndx, 2) = Trim(Clthname)
                    prn_HdAr(prn_HdMxIndx, 3) = Val(prn_HdDt.Rows(i).Item("Total_Pcs").ToString)
                    prn_HdAr(prn_HdMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                    prn_HdAr(prn_HdMxIndx, 5) = Trim(prn_HdDt.Rows(i).Item("Bale_Bundle").ToString)
                    prn_HdAr(prn_HdMxIndx, 6) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString)
                    prn_HdAr(prn_HdMxIndx, 7) = Trim(prn_HdDt.Rows(i).Item("company_idno").ToString)

                    prn_DetMxIndx = 0

                    da2 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                    If prn_DetDt.Rows.Count > 0 Then
                        For j = 0 To prn_DetDt.Rows.Count - 1
                            If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
                                prn_DetMxIndx = prn_DetMxIndx + 1
                                If Common_Procedures.settings.CustomerCode = "1234" Then
                                    prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Replace(Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString), "T", "")
                                Else
                                    prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                End If

                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

                            End If
                        Next j
                    End If


                    '------------------------Sathy Tex 1438 
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1438" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1487" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1489" Then

                        prn_DetMxIndx = 0
                        da3 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", con)
                        prn_DetDt = New DataTable
                        da3.Fill(prn_DetDt)

                        If prn_DetDt.Rows.Count > 0 Then
                            For j = 0 To prn_DetDt.Rows.Count - 1
                                If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
                                    prn_DetMxIndx = prn_DetMxIndx + 1

                                    vSNO = vSNO + 1


                                    If vPRN_Weight_Column_Status = True Then

                                        prn_DetAr1(prn_DetMxIndx, 5) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                                        prn_DetAr1(prn_DetMxIndx, 0) = prn_DetMxIndx
                                        prn_DetAr1(prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                        prn_DetAr1(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                        prn_DetAr1(prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
                                        prn_DetAr1(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.000")
                                        prn_DetAr1(prn_DetMxIndx, 6) = Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString) / Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.000")


                                    Else

                                        prn_DetAr1(prn_DetMxIndx, 1) = vSNO

                                        If Common_Procedures.settings.CustomerCode = "1234" Then
                                            prn_DetAr1(prn_DetMxIndx, 2) = Replace(Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString), "T", "")
                                        Else
                                            prn_DetAr1(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                        End If

                                        prn_DetAr1(prn_DetMxIndx, 3) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                        prn_DetAr1(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")


                                    End If


                                End If
                            Next j
                        End If

                        prn_Clothname = Trim(Clthname) ' prn_HdDt.Rows(0).Item("Cloth_name").ToString

                        Total_pcs = Val(prn_HdDt.Rows(0).Item("total_pcs").ToString)
                        vTot_Mtrs = Format(Val(prn_HdDt.Rows(0).Item("total_Meters").ToString), "########0.00")
                        vtot_wgt = Format(Val(prn_HdDt.Rows(0).Item("total_Weight").ToString), "########0.000")

                        vPacking_SlipNo = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                        Pack_Type_Name = Trim(prn_HdDt.Rows(i).Item("Bale_Bundle").ToString)

                    End If

                    '-----------------------------

                Next i

            Else

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim EntryCode As String
        Dim prn_NoofBmDets As String
        Dim vPartyCityName As String
        Dim vPACKINGSLIPDATE As String

        vtot_pcs = 0
        'vtot_wgt = 0
        lst_prnt = False

        prn_NoofBmDets = 0

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Prn_BarcodeSticker = True Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                Printing_BarCode_Sticker_Format1(e)
            Else
                Printing_PackingSlip_Sticker_Format_1155(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx, vtot_wgt, vtot_pcs, lst_prnt)
            End If

        Else

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1249" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1516" Then '---- VAIPAV TEXTILES PVT LTD (SOMANUR) AND ---- VIPIN TEXTILES (SOMANUR) 
                Common_Procedures.Printing_PackingSlip_Format2(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
                Common_Procedures.Printing_PackingSlip_Format_1155(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx, vtot_wgt, vtot_pcs, lst_prnt)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1438" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1487" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1489" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1520" Then

                ' PartyName = prn_HdDt.Rows(0).Item("Sales_Party_idno").ToString
                'vPartyName = Common_Procedures.Ledger_IdNoToName(con, (prn_HdDt.Rows(0).Item("Sales_Party_idno").ToString))

                vPartyName = prn_HdDt.Rows(0).Item("Ledger_mainName").ToString
                vPartyCityName = ""
                If Trim(prn_HdDt.Rows(0).Item("City_Town").ToString) <> "" Then
                    vPartyCityName = prn_HdDt.Rows(0).Item("City_Town").ToString

                ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) = "" Then
                    vPartyCityName = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

                ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) = "" Then
                    vPartyCityName = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString

                ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) = "" Then
                    vPartyCityName = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString

                Else
                    vPartyCityName = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString

                End If

                Dim vFOLDING As String

                vFOLDING = ""
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1489" Then
                    vFOLDING = Val(prn_HdDt.Rows(0).Item("Folding").ToString)
                    If Val(vFOLDING) = 0 Then vFOLDING = 100
                End If

                vPACKINGSLIPDATE = Trim(Format(prn_HdDt.Rows(0).Item("Packing_Slip_Date"), "dd-MM-yyyy"))

                If vPRN_Weight_Column_Status = True Then

                    Common_Procedures.Printing_Format_PackingList_1438(PrintDocument1, e, prn_HdDt, prn_DetDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr1, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx, prn_NoofBmDets, vPRN_Weight_Column_Status, vPacking_SlipNo, "", "", vPartyName, vPartyCityName, "", prn_Clothname, Pack_Type_Name, Total_pcs, vTot_Mtrs, vtot_wgt, "", vFOLDING, "", "", vPACKINGSLIPDATE)

                Else

                    Common_Procedures.Printing_PackingSlip_Format_1391(PrintDocument1, e, prn_HdDt, prn_DetDt, prn_DetMxIndx, prn_DetAr1, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx, prn_NoofBmDets, vPRN_Weight_Column_Status, vPacking_SlipNo, "", "", vPartyName, vPartyCityName, "", prn_Clothname, Pack_Type_Name, Total_pcs, vTot_Mtrs, vtot_wgt, "", vFOLDING, vPACKINGSLIPDATE)

                End If

            Else

                Common_Procedures.Printing_PackingSlip_Format1(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)

            End If

        End If

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                            dgv_Details_KeyUp(sender, e)
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName_StockOF, msk_Plan_date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
        'If e.KeyCode = 40 And cbo_PartyName_StockOF.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
        '    If cbo_Godown_StockIN.Visible = True Then
        '        cbo_Godown_StockIN.Focus()
        '    Else
        '        cbo_Cloth.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName_StockOF, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    If cbo_Godown_StockIN.Visible = True Then
        '        cbo_Godown_StockIN.Focus()
        '    Else
        '        cbo_Cloth.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.Control = False And e.KeyValue = 17 Then
        '    Common_Procedures.MDI_LedType = "JOBWORKER"
        '    Dim f As New Ledger_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_PartyName_StockOF.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()
        'End If
    End Sub

    Private Sub txt_PcsSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.KeyValue = 40 Then
        '    If dgv_Selection.Rows.Count > 0 Then
        '        dgv_Selection.Focus()
        '        dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        '        dgv_Selection.CurrentCell.Selected = True
        '    End If
        'End If
        'If (e.KeyValue = 38) Then txt_PcsSelection_LotNo.Focus()

    End Sub

    Private Sub txt_PcsSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'If Asc(e.KeyChar) = 13 Then

        '    If Trim(txt_PcsSelection_PcsNo.Text) <> "" Or Trim(txt_PcsSelection_PcsNo.Text) <> "" Then
        '        btn_PcsSelection_Select_Click(sender, e)

        '    Else
        '        If dgv_Selection.Rows.Count > 0 Then
        '            dgv_Selection.Focus()
        '            dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        '            dgv_Selection.CurrentCell.Selected = True
        '        End If

        '    End If

        'End If
    End Sub

    Private Sub txt_PcsSelection_LotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If (e.KeyValue = 40) Then
        '    txt_PcsSelection_PcsNo.Focus()
        'End If
    End Sub

    Private Sub txt_PcsSelection_LotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'If Asc(e.KeyChar) = 13 Then
        '    txt_PcsSelection_PcsNo.Focus()
        'End If
    End Sub

    'Private Sub btn_PcsSelection_Select_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim LtNo As String
    '    Dim PcsNo As String
    '    Dim i As Integer
    '    Dim vBarCode As String = ""
    '    Dim vYrCode As String = ""
    '    Dim vPcsTyp As String = ""
    '    Dim vSELECTN_BY As String = ""
    '    Dim vMATCHSTS As Boolean = False


    '    vMATCHSTS = False
    '    If Trim(txt_PcsSelection_BarCode.Text) <> "" Then

    '        vSELECTN_BY = "Bar Code"

    '        For i = 0 To dgv_Selection.Rows.Count - 1

    '            If dgv_Selection.Rows(i).Visible = True Then

    '                vBarCode = dgv_Selection.Rows(i).Cells(12).Value

    '                If Trim(vBarCode) = "" Then

    '                    Da1 = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Piece_Details a, Weaver_Piece_Checking_Head b where a.Lot_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Piece_No = '" & Trim(dgv_Selection.Rows(i).Cells(2).Value) & "' and a.Weaver_Piece_Checking_Code = b.Weaver_Piece_Checking_Code ORDER BY Lot_No, PieceNo_OrderBy ASC", con)
    '                    Dt1 = New DataTable
    '                    Da1.Fill(Dt1)
    '                    If Dt1.Rows.Count > 0 Then
    '                        vYrCode = Microsoft.VisualBasic.Right(Trim(Dt1.Rows(0).Item("Lot_Code").ToString), 5)
    '                        vPcsTyp = Common_Procedures.ClothType_NameToIdNo(con, dgv_Selection.Rows(i).Cells(3).Value)
    '                        vBarCode = Microsoft.VisualBasic.Left(vYrCode, 2) & Trim(Val(Dt1.Rows(0).Item("Company_IdNo").ToString)) & Trim(UCase(Dt1.Rows(0).Item("Lot_No").ToString)) & Trim(UCase(Dt1.Rows(prn_DetIndx).Item("Piece_No").ToString)) & Trim(Val(vPcsTyp))
    '                    End If
    '                    Dt1.Clear()

    '                End If

    '                If Trim(UCase(vBarCode)) = Trim(UCase(txt_PcsSelection_BarCode.Text)) And Val(dgv_Selection.Rows(i).Cells(8).Value) = 0 Then
    '                    Call Select_Piece(i)
    '                    vMATCHSTS = True
    '                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
    '                    If i >= 9 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 8
    '                    Exit For
    '                End If

    '            End If

    '        Next

    '        txt_PcsSelection_LotNo.Text = ""
    '        txt_PcsSelection_PcsNo.Text = ""
    '        txt_PcsSelection_BarCode.Text = ""
    '        If txt_PcsSelection_BarCode.Enabled = True Then txt_PcsSelection_BarCode.Focus()

    '    ElseIf Trim(txt_PcsSelection_LotNo.Text) <> "" And Trim(txt_PcsSelection_PcsNo.Text) <> "" Then

    '        vSELECTN_BY = "Lot NO / Piece No"

    '        LtNo = Trim(txt_PcsSelection_LotNo.Text)
    '        PcsNo = Trim(txt_PcsSelection_PcsNo.Text)

    '        For i = 0 To dgv_Selection.Rows.Count - 1
    '            If dgv_Selection.Rows(i).Visible = True Then
    '                If Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) And Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
    '                    Call Select_Piece(i)
    '                    vMATCHSTS = True
    '                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
    '                    If i >= 9 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 8

    '                    Exit For
    '                End If
    '            End If
    '        Next

    '        txt_PcsSelection_LotNo.Text = ""
    '        txt_PcsSelection_PcsNo.Text = ""
    '        txt_PcsSelection_BarCode.Text = ""
    '        If txt_PcsSelection_LotNo.Enabled = True Then txt_PcsSelection_LotNo.Focus()

    '    ElseIf Trim(txt_PcsSelection_PcsNo.Text) <> "" Then

    '        PcsNo = Trim(txt_PcsSelection_PcsNo.Text)

    '        vSELECTN_BY = "Piece No"

    '        For i = 0 To dgv_Selection.Rows.Count - 1
    '            If dgv_Selection.Rows(i).Visible = True Then
    '                If Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
    '                    Call Select_Piece(i)
    '                    vMATCHSTS = True
    '                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
    '                    If i >= 9 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 8
    '                    Exit For
    '                End If
    '            End If
    '        Next

    '        txt_PcsSelection_LotNo.Text = ""
    '        txt_PcsSelection_PcsNo.Text = ""
    '        txt_PcsSelection_BarCode.Text = ""
    '        If txt_PcsSelection_PcsNo.Enabled = True Then txt_PcsSelection_PcsNo.Focus()

    '    End If

    '    Total_PieceSelection_Calculation()

    '    If vMATCHSTS = False And Trim(vSELECTN_BY) <> "" Then
    '        MessageBox.Show("Invalid " & vSELECTN_BY, "DOES NOT SELECT PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
    '    End If

    'End Sub


    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Plan_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Plan_date.Text
            vmskSelStrt = msk_Plan_date.SelectionStart
        End If


        If e.KeyCode = 40 Then
            msk_Allotment_Date.Focus()
        End If



    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Plan_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Plan_date.Text = Date.Today
            msk_Plan_date.SelectionStart = 0
        End If


        If Asc(e.KeyChar) = 13 Then
            msk_Allotment_Date.Focus()
        End If

    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Plan_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_Plan_date.Text = Date.Today
        End If
        If e.KeyCode = 107 Then
            msk_Plan_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Plan_date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Plan_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Plan_date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If

    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Plan_Date.TextChanged
        If FrmLdSTS = True Then Exit Sub

        If IsDate(dtp_Plan_Date.Text) = True Then

            msk_Plan_date.Text = dtp_Plan_Date.Text
            msk_Plan_date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Plan_date.LostFocus

        If IsDate(msk_Plan_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Plan_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Plan_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Plan_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Plan_date.Text)) >= 2000 Then
                    dtp_Plan_Date.Value = Convert.ToDateTime(msk_Plan_date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Plan_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Plan_Date.Text = Date.Today
        End If
    End Sub

    'Private Sub btn_PcsSelection_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim vFirstRowNo As Integer = -1
    '    Dim i As Integer

    '    If Val(txt_PcsSelection_MetersEqualTo.Text) <> 0 Then

    '        For i = 0 To dgv_Selection.Rows.Count - 1
    '            dgv_Selection.Rows(i).Visible = False
    '        Next

    '        vFirstRowNo = -1
    '        For i = 0 To dgv_Selection.Rows.Count - 1
    '            If Val(dgv_Selection.Rows(i).Cells(4).Value) = Val(txt_PcsSelection_MetersEqualTo.Text) Then
    '                dgv_Selection.Rows(i).Visible = True
    '                If vFirstRowNo = -1 Then vFirstRowNo = i
    '            End If
    '        Next

    '        If vFirstRowNo >= 0 Then
    '            dgv_Selection.Focus()
    '            dgv_Selection.CurrentCell = dgv_Selection.Rows(vFirstRowNo).Cells(0)
    '            dgv_Selection.CurrentCell.Selected = True

    '        End If

    '        txt_PcsSelection_MetersEqualTo.SelectAll()
    '        If txt_PcsSelection_MetersEqualTo.Enabled = True Then txt_PcsSelection_MetersEqualTo.Focus()
    '        txt_PcsSelection_MetersEqualTo.SelectAll()

    '    ElseIf Val(txt_PcsSelection_MetersFrom.Text) <> 0 And Val(txt_PcsSelection_MetersTo.Text) <> 0 Then

    '        For i = 0 To dgv_Selection.Rows.Count - 1
    '            dgv_Selection.Rows(i).Visible = False
    '        Next

    '        vFirstRowNo = -1
    '        For i = 0 To dgv_Selection.Rows.Count - 1
    '            If Val(dgv_Selection.Rows(i).Cells(4).Value) >= Val(txt_PcsSelection_MetersFrom.Text) And Val(dgv_Selection.Rows(i).Cells(4).Value) <= Val(txt_PcsSelection_MetersTo.Text) Then
    '                dgv_Selection.Rows(i).Visible = True
    '                If vFirstRowNo = -1 Then vFirstRowNo = i
    '            End If
    '        Next

    '        If vFirstRowNo >= 0 Then
    '            dgv_Selection.Focus()
    '            dgv_Selection.CurrentCell = dgv_Selection.Rows(vFirstRowNo).Cells(0)
    '            dgv_Selection.CurrentCell.Selected = True

    '        End If

    '        txt_PcsSelection_MetersFrom.SelectAll()
    '        If txt_PcsSelection_MetersFrom.Enabled = True Then txt_PcsSelection_MetersFrom.Focus()


    '    ElseIf Trim(cbo_PcsSelection_VendorName.Text) <> "" Then

    '        For i = 0 To dgv_Selection.Rows.Count - 1
    '            dgv_Selection.Rows(i).Visible = False
    '        Next

    '        vFirstRowNo = -1
    '        For i = 0 To dgv_Selection.Rows.Count - 1
    '            If Trim(UCase(dgv_Selection.Rows(i).Cells(7).Value)) = Trim(UCase(cbo_PcsSelection_VendorName.Text)) Then
    '                dgv_Selection.Rows(i).Visible = True
    '                If vFirstRowNo = -1 Then vFirstRowNo = i
    '            End If
    '        Next

    '        If vFirstRowNo >= 0 Then
    '            dgv_Selection.Focus()
    '            dgv_Selection.CurrentCell = dgv_Selection.Rows(vFirstRowNo).Cells(0)
    '            dgv_Selection.CurrentCell.Selected = True

    '        End If

    '        cbo_PcsSelection_VendorName.SelectAll()
    '        If cbo_PcsSelection_VendorName.Enabled = True Then cbo_PcsSelection_VendorName.Focus()
    '        cbo_PcsSelection_VendorName.SelectAll()

    '    Else

    '        btn_PcsSelection_ShowAll_Pieces_Click(sender, e)

    '    End If

    '    Total_PieceSelection_Calculation()

    'End Sub

    Private Sub btn_PcsSelection_ShowAll_Pieces_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim i As Integer = 0
        'Dim CurRow As Integer = 0

        'Try
        '    For i = 0 To dgv_Selection.Rows.Count - 1
        '        dgv_Selection.Rows(i).Visible = True
        '    Next
        '    txt_PcsSelection_MetersEqualTo.Text = ""
        '    txt_PcsSelection_MetersFrom.Text = ""
        '    txt_PcsSelection_MetersTo.Text = ""
        '    cbo_PcsSelection_VendorName.Text = ""

        'Catch ex As Exception
        '    '---
        'End Try
    End Sub

    Private Sub txt_PcsSelection_MetersEqualTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.KeyValue = 40 Then
        '    txt_PcsSelection_MetersFrom.Focus()
        '    'If dgv_Selection.Rows.Count > 0 Then
        '    '    dgv_Selection.Focus()
        '    '    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        '    '    dgv_Selection.CurrentCell.Selected = True
        '    'End If
        'End If
        'If (e.KeyValue = 38) Then txt_PcsSelection_PcsNo.Focus()
    End Sub

    Private Sub txt_PcsSelection_MetersEqualTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'If Asc(e.KeyChar) = 13 Then
        '    btn_PcsSelection_Show_Click(sender, e)
        'End If
    End Sub

    Private Sub txt_PcsSelection_MetersFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.KeyValue = 40 Then
        '    txt_PcsSelection_MetersTo.Focus()
        'End If
        'If e.KeyValue = 38 Then txt_PcsSelection_MetersEqualTo.Focus()
    End Sub

    Private Sub txt_PcsSelection_MetersFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'If Asc(e.KeyChar) = 13 Then
        '    txt_PcsSelection_MetersTo.Focus()
        'End If
    End Sub

    Private Sub txt_PcsSelection_MetersTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.KeyValue = 40 Then
        '    cbo_PcsSelection_VendorName.Focus()
        'End If
        'If e.KeyValue = 38 Then txt_PcsSelection_MetersFrom.Focus()
    End Sub

    Private Sub txt_PcsSelection_MetersTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'If Asc(e.KeyChar) = 13 Then
        '    btn_PcsSelection_Show_Click(sender, e)
        'End If
    End Sub

    Private Sub cbo_Godown_StockIN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' and Close_status = 0)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_StockIN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Godown_StockIN, Nothing, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' and Close_status = 0)  ", "(Ledger_IdNo = 0)")
        'If (e.KeyValue = 38 And cbo_Godown_StockIN.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
        '    If cbo_PartyName_StockOF.Visible And cbo_PartyName_StockOF.Enabled Then
        '        cbo_PartyName_StockOF.Focus()
        '    ElseIf msk_Plan_date.Visible And msk_Plan_date.Enabled Then
        '        msk_Plan_date.Focus()
        '        'ElseIf txt_BaleSuffixNo.Visible And txt_BaleSuffixNo.Enabled Then
        '        '    txt_BaleSuffixNo.Focus()
        '        'ElseIf txt_BalePrefixNo.Visible And txt_BalePrefixNo.Enabled Then
        '        '    txt_BalePrefixNo.Focus()
        '    ElseIf txt_Note.Visible And txt_Note.Enabled Then
        '        txt_Note.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_Godown_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Godown_StockIN, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' and Close_status = 0)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub txt_PcsSelection_BarCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.KeyValue = 40 Then
        '    If dgv_Selection.Rows.Count > 0 Then
        '        dgv_Selection.Focus()
        '        dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        '        dgv_Selection.CurrentCell.Selected = True
        '    End If
        'End If
        'If (e.KeyValue = 38) Then txt_PcsSelection_MetersEqualTo.Focus()
    End Sub

    Private Sub txt_PcsSelection_BarCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(txt_PcsSelection_BarCode.Text) <> "" Then
        '        btn_PcsSelection_Select_Click(sender, e)
        '    End If
        'End If
    End Sub

    Private Sub txt_BalePrefixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_BaleSuffixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub btn_BarcodePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_BarcodePrint.Click

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then

            Common_Procedures.Print_OR_Preview_Status = 0
            Prn_BarcodeSticker = True
            Printing_BarCode_Sticker_Format4_DosPrint()
            'Printing_BarCode_Sticker()

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            Prn_BarcodeSticker = True
            vPRN_Weight_Column_Status = False
            Printing_Bale()

        Else

            vPRN_Weight_Column_Status = True
            Printing_Bale()

        End If


    End Sub


    Private Sub Printing_BarCode_Sticker()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim prtFrm As String, prtTo As String
        Dim Condt As String

        If Prn_BarcodeSticker = False Then Exit Sub

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If

        Try

            da1 = New SqlClient.SqlDataAdapter("Select a.*, c.Cloth_Name from Packing_Slip_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code = '" & Trim(NewCode) & "'", con)
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
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

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
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1.0

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub Printing_BarCode_Sticker_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, BarFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim CurY As Single
        Dim CurX As Single
        Dim BrCdX As Single = 20
        Dim BrCdY As Single = 100
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = "", vPcs As String = ""
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

        Try

            If prn_HdDt.Rows.Count > 0 Then

                'NoofDets = 1
                For noofitems = 1 To NoofItems_PerPage


                    'prn_HeadIndx
                    vFldMtrs = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString), "##########0.00")
                    vPcs = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Pcs").ToString), "##########0.00")
                    'vBarCode = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Bar_Code").ToString)

                    If Val(vFldMtrs) <> 0 Then

                        'If NoofDets >= NoofItems_PerPage Then
                        '    e.HasMorePages = True
                        '    Return
                        'End If

                        CurY = TMargin

                        CurX = LMargin - 1
                        If noofitems Mod 2 = 0 Then
                            CurX = CurX + ((PageWidth + RMargin) \ 2)
                        End If

                        ItmNm1 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString)

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
                        Common_Procedures.Print_To_PrintDocument(e, "Sort : " & ItmNm1, CurX, CurY, 0, PrintWidth, pFont, , True)
                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 2
                            Common_Procedures.Print_To_PrintDocument(e, ItmNm2, CurX + 100, CurY, 1, PrintWidth, pFont, , True)
                        End If

                        pFont = New Font("Calibri", 9, FontStyle.Bold)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Bale No  : " & prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Pcs          : " & vPcs, CurX, CurY, 0, PrintWidth, pFont, , True)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Meters   : " & vFldMtrs, CurX, CurY, 0, PrintWidth, pFont, , True)

                        Dim vYrCode As String = ""

                        vYrCode = Microsoft.VisualBasic.Right(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_Code").ToString, 5)
                        vBarCode = Microsoft.VisualBasic.Left(vYrCode, 2) & Trim(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Company_IdNo").ToString)) & Trim(UCase(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_No").ToString))
                        vBarCode = "*" & Trim(UCase(vBarCode)) & "*"
                        BarFont = New Font("Free 3 of 9", 18, FontStyle.Regular)

                        CurY = CurY + TxtHgt + 5
                        e.Graphics.DrawString(Trim(vBarCode), BarFont, Brushes.Black, CurX, CurY)

                        pFont = New Font("Calibri", 14, FontStyle.Bold)
                        CurY = CurY + TxtHgt + TxtHgt - 6
                        Common_Procedures.Print_To_PrintDocument(e, Trim(vBarCode), CurX + 5, CurY, 0, PrintWidth, pFont, , True)

                        'NoofDets = NoofDets + 1

                    End If

                    'prn_DetBarCdStkr = prn_DetBarCdStkr + 1
                    'prn_DetBarCdStkr = 1
                    'prn_DetIndx = prn_DetIndx + 1


                    prn_HeadIndx = prn_HeadIndx + 1

                    If prn_HeadIndx > prn_HdDt.Rows.Count - 1 Then
                        Exit For
                    End If



                Next


            End If '' end of  If prn_HdDt.Rows.Count > 0 Then


            'prn_HeadIndx = prn_HeadIndx + 1

            If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
                e.HasMorePages = True
            Else
                'e.HasMorePages = False
                e.HasMorePages = False

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub





    'Private Sub Printing_BarCode_Sticker_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
    '    Dim pFont As Font, BarFont As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim I As Integer
    '    Dim NoofItems_PerPage As Integer, NoofDets As Integer
    '    Dim TxtHgt As Single
    '    Dim PpSzSTS As Boolean = False
    '    Dim LnAr(15) As Single, ClAr(15) As Single
    '    Dim CurY As Single
    '    Dim CurX As Single
    '    Dim BrCdX As Single = 20
    '    Dim BrCdY As Single = 100
    '    Dim vBarCode As String = ""
    '    Dim vFldMtrs As String = "", vPcs As String = ""
    '    Dim ItmNm1 As String, ItmNm2 As String


    '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.18", 325, 118)
    '    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
    '    PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
    '    e.PageSettings.PaperSize = pkCustomSize1

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 5
    '        .Right = 2
    '        .Top = 5 ' 40
    '        .Bottom = 2
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

    '    With PrintDocument1.DefaultPageSettings.PaperSize
    '        PrintWidth = .Width - RMargin - LMargin
    '        PrintHeight = .Height - TMargin - BMargin
    '        PageWidth = .Width - RMargin
    '        PageHeight = .Height - BMargin
    '    End With
    '    If PrintDocument1.DefaultPageSettings.Landscape = True Then
    '        With PrintDocument1.DefaultPageSettings.PaperSize
    '            PrintWidth = .Height - TMargin - BMargin
    '            PrintHeight = .Width - RMargin - LMargin
    '            PageWidth = .Height - TMargin
    '            PageHeight = .Width - RMargin
    '        End With
    '    End If

    '    NoofItems_PerPage = 2

    '    TxtHgt = 13.5

    '    Try

    '        If prn_HdDt.Rows.Count > 0 Then

    '            NoofDets = 0

    '            vFldMtrs = Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "##########0.00")
    '            vPcs = Format(Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), "##########0.00")
    '            'vBarCode = Trim(prn_HdDt.Rows(0).Item("Bar_Code").ToString)

    '            If Val(vFldMtrs) <> 0 Then

    '                If NoofDets >= NoofItems_PerPage Then
    '                    e.HasMorePages = True
    '                    Return
    '                End If

    '                CurY = TMargin

    '                CurX = LMargin - 1
    '                If NoofDets = 1 Then
    '                    CurX = CurX + ((PageWidth + RMargin) \ 2)
    '                End If

    '                'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
    '                '    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
    '                'Else
    '                ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Name").ToString)
    '                'End If

    '                ItmNm2 = ""
    '                If Len(ItmNm1) > 21 Then
    '                    For I = 21 To 1 Step -1
    '                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                    Next I
    '                    If I = 0 Then I = 21

    '                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I + 1)
    '                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                End If

    '                pFont = New Font("Calibri", 9, FontStyle.Bold)
    '                Common_Procedures.Print_To_PrintDocument(e, "Sort : " & ItmNm1, CurX, CurY, 0, PrintWidth, pFont, , True)
    '                If Trim(ItmNm2) <> "" Then
    '                    CurY = CurY + TxtHgt - 2
    '                    Common_Procedures.Print_To_PrintDocument(e, ItmNm2, CurX + 100, CurY, 1, PrintWidth, pFont, , True)
    '                End If

    '                pFont = New Font("Calibri", 9, FontStyle.Bold)

    '                CurY = CurY + TxtHgt
    '                Common_Procedures.Print_To_PrintDocument(e, "Bale No  : " & prn_HdDt.Rows(0).Item("Packing_Slip_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)

    '                CurY = CurY + TxtHgt
    '                Common_Procedures.Print_To_PrintDocument(e, "Pcs          : " & vPcs, CurX, CurY, 0, PrintWidth, pFont, , True)

    '                CurY = CurY + TxtHgt
    '                Common_Procedures.Print_To_PrintDocument(e, "Meters   : " & vFldMtrs, CurX, CurY, 0, PrintWidth, pFont, , True)

    '                vBarCode = "*" & Trim(UCase(vBarCode)) & "*"
    '                BarFont = New Font("Free 3 of 9", 18, FontStyle.Regular)

    '                CurY = CurY + TxtHgt + 5
    '                e.Graphics.DrawString(Trim(vBarCode), BarFont, Brushes.Black, CurX, CurY)

    '                pFont = New Font("Calibri", 14, FontStyle.Bold)
    '                CurY = CurY + TxtHgt + TxtHgt - 6
    '                Common_Procedures.Print_To_PrintDocument(e, Trim(vBarCode), CurX + 5, CurY, 0, PrintWidth, pFont, , True)

    '                NoofDets = NoofDets + 1

    '            End If

    '            prn_DetBarCdStkr = prn_DetBarCdStkr + 1
    '            prn_DetBarCdStkr = 1
    '            prn_DetIndx = prn_DetIndx + 1


    '        End If


    '        prn_HeadIndx = prn_HeadIndx + 1

    '        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
    '            e.HasMorePages = True
    '        Else
    '            'e.HasMorePages = False
    '            e.HasMorePages = False
    '            Prn_BarcodeSticker = False
    '        End If

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    '    'e.HasMorePages = False
    '    'Prn_BarcodeSticker = False

    'End Sub


    Private Sub cbo_Bale_Bundle_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' If FrmLdSTS = True Then Exit Sub
        If Common_Procedures.settings.CustomerCode = "1357" Then
            'If cbo_Bale_Bundle.Text = "BALE" Then
            '    txt_Tare_weight.Text = "0.400"
            'ElseIf cbo_Bale_Bundle.Text = "BUNDLE" Then
            '    txt_Tare_weight.Text = "1.400"
            'Else

            '    txt_Tare_weight.Text = ""
            'End If
        End If
    End Sub

    Private Sub txt_Tare_weight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If FrmLdSTS = True Then Exit Sub
        Calculation_GrossWeight()

    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs)
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub txt_net_weight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Calculation_GrossWeight()
    End Sub

    Private Sub Calculation_GrossWeight()
        Dim vTOTMTR As String = 0

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        vTOTMTR = 0
        If dgv_Details_Total.Rows.Count > 0 Then

            vTOTMTR = Val(dgv_Details_Total.Rows(0).Cells(4).Value)

            'If dgv_Details_Total.Rows(0).Cells(5).Value <> 0 Then
            '    txt_net_weight.Text = Format(Val(dgv_Details_Total.Rows(0).Cells(5).Value), "######00.000")
            '    txt_gross_weight.Text = Format(Val(dgv_Details_Total.Rows(0).Cells(5).Value) + Val(txt_Tare_weight.Text), "######00.000")

            'Else

            '    txt_gross_weight.Text = Format(Val(txt_net_weight.Text) + Val(txt_Tare_weight.Text), "######00.000")

            'End If

        Else

            'txt_gross_weight.Text = Format(Val(txt_net_weight.Text) + Val(txt_Tare_weight.Text), "######00.000")

        End If

        'lbl_Weight_per_Mtr.Text = ""
        'If Val(vTOTMTR) <> 0 Then
        '    lbl_Weight_per_Mtr.Text = Format(Val(txt_net_weight.Text) / Val(vTOTMTR), "######0.000")
        'End If


    End Sub

    Private Sub cbo_Godown_StockIN_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub cbo_PartyName_StockOF_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub txt_BaleSuffixNo_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub btn_excel_Click(sender As Object, e As EventArgs) Handles btn_excel.Click
        Dim EntryCode As String = ""
        Dim prn_HdMxIndx As Long

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

        prn_HdAr = New String(1000, 10) {}

        prn_DetAr = New String(1000, 1000, 10) {}

        Common_Procedures.Printing_PackingSlip_Format1155_Excel(con, EntryCode, Val(lbl_Company.Tag), Other_Condition, "", "", txt_PrintFrom.Text, txt_PrintTo.Text, prn_HdDt, prn_DetDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)
        'Printing_Format1_Excel()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub Printing_Format1_Excel()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim Indx As Integer = 0
        Dim RndOffAmt As String = ""
        Dim NtAmt As String = ""
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim FlName1 As String = "", FlName2 As String = ""
        Dim NewCode As String = ""
        Dim n As Integer = 0
        Dim DetRwsCnt As Integer = 0
        Dim xlCurRow As Integer = 0
        Dim xlApp As Excel.Application = New Microsoft.Office.Interop.Excel.Application()

        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim Condt As String = ""
        Dim Clthname As String = ""

        prn_HeadIndx = 0


        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub

        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If















        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_meters = 0
        prn_Pcs = 0
        prn_PageNo = 0
        prn_HdIndx = 1
        prn_DetIndx = 0
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_Count = 1
        prn_TotalBales = 0
        prn_TotalPcs = 0
        prn_TotalMtrs = 0
        prn_TotalWgt = 0
        Erase prn_DetAr

        Erase prn_HdAr

        prn_HdAr = New String(1000, 1000) {}

        prn_DetAr = New String(1000, 1000, 10) {}

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*,a.Packing_Slip_No, a.cloth_idno, a.Total_Pcs as Pak_Pcs, a.Packing_Slip_Code, a.Total_Weight, a.Total_Meters as Pak_Mtrs, tZ.*, c.Cloth_Name,c.Cloth_Description from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and " & Trim(Other_Condition) & IIf(Trim(Condt) <> "", " and ", "") & Condt & " order by a.for_orderby, a.Packing_Slip_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                For i = 0 To prn_HdDt.Rows.Count - 1

                    prn_HdMxIndx = prn_HdMxIndx + 1

                    prn_HdAr(prn_HdMxIndx, 1) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                    prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)
                    prn_HdAr(prn_HdMxIndx, 3) = Val(prn_HdDt.Rows(i).Item("Pak_Pcs").ToString)
                    prn_HdAr(prn_HdMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Pak_Mtrs").ToString), "#########0.00")

                    prn_TotalBales = prn_TotalBales + 1

                    prn_meters = prn_meters + Val(prn_HdDt.Rows(i).Item("Pak_Mtrs").ToString)

                    prn_Pcs = prn_Pcs + Val(prn_HdDt.Rows(i).Item("Pak_Pcs").ToString)

                    prn_DetMxIndx = 0

                    da2 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                    If prn_DetDt.Rows.Count > 0 Then
                        For j = 0 To prn_DetDt.Rows.Count - 1
                            If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
                                prn_DetMxIndx = prn_DetMxIndx + 1
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 5) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 0) = Trim(prn_DetDt.Rows(j).Item("Sl_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.00")

                                prn_TotalPcs = Val(prn_TotalPcs) + 1
                                prn_TotalMtrs = Format(Val(prn_TotalMtrs) + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00"), "##########0.00")
                                prn_TotalWgt = Format(Val(prn_TotalWgt) + Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.000"), "##########0.000")

                            End If
                        Next j
                    End If

                Next i

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try











        'da1 = New SqlClient.SqlDataAdapter("select a.*, tZ.*, c.Cloth_Name,c.Cloth_Description from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and " & Trim(Other_Condition) & " order by a.for_orderby, a.Packing_Slip_Code", Con)
        'prn_HdDt = New DataTable
        'da1.Fill(prn_HdDt)
        If prn_HdDt.Rows.Count > 0 Then

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

            'FlName1 = Microsoft.VisualBasic.Left(Trim(Common_Procedures.AppPath), 2) & "\Packing_excel.xlsx"


            FlName1 = Trim(Common_Procedures.AppPath) & "\Packing_List_excel_Copy.xlsx"


            Dim xlWorkBook As Excel.Workbook
            Dim xlWorkSheet As Excel.Worksheet

            Dim misValue As Object = System.Reflection.Missing.Value


            FlName2 = Trim(FlName2)

            If File.Exists(FlName1) = False Then
                MessageBox.Show("Invalid  " & Chr(13) & FlName1, "DOES NOT SHOW REPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            SaveFileDialog1.ShowDialog()
            FlName2 = SaveFileDialog1.FileName


            FlName2 = Trim(FlName2) & ".xlsx"

            For Each workbook In xlApp.Workbooks
                If workbook.Name.ToString.ToLower = FlName2.ToString.ToLower Then
                    MsgBox("Close the Excell Workbook Named " & FlName2 & ". It needs to be closed to proceed further")
                    Exit Sub
                End If
            Next

            If File.Exists(FlName2) = True Then
                File.Delete(FlName2)
            End If

            File.Copy(FlName1, FlName2, True)



            xlWorkBook = xlApp.Workbooks.Open(FlName2)
            'xlWorkBook = xlApp.Workbooks.Add(misValue)
            xlWorkSheet = xlWorkBook.Sheets(1)

            Try


                xlCurRow = 2
                xlWorkSheet.Cells(xlCurRow, 1) = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)
                xlWorkSheet.Cells((xlCurRow + 1), 1) = "PACKING LIST"
                'xlWorkSheet.Cells(xlCurRow + 3, 1) = "PARTY NAME : " & Trim(prn_HdDt.Rows(0).Item("Ledger_name").ToString)

                ' xlWorkSheet.Cells(xlCurRow + 3, 7) = "INVOICE No. : " & Trim(prn_HdDt.Rows(0).Item("ClothSales_Invoice_RefNo").ToString)
                'xlWorkSheet.Cells(xlCurRow + 4, 7) = "INVOICE DATE. : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Invoice_Date").ToString), "dd-MM-yyyy").ToString
                'xlWorkSheet.Cells(xlCurRow + 5, 7) = "FOLDING : " & Trim(prn_HdDt.Rows(0).Item("Folding").ToString)
                xlWorkSheet.Cells(xlCurRow + 3, 1) = "CLOTHNAME : " & Trim(prn_HdDt.Rows(0).Item("Cloth_name").ToString)


                xlWorkSheet.Cells(xlCurRow + 4, 1) = "TOTAL BALES : " & prn_TotalBales
                xlWorkSheet.Cells(xlCurRow + 5, 1) = "TOTAL METERS : " & prn_meters

                xlWorkSheet.Cells(xlCurRow + 6, 1) = "TOTAL PIECES : " & prn_Pcs




                xlCurRow = 11

                'da1 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(0).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", Con)
                'prn_DetDt = New DataTable
                'da1.Fill(prn_DetDt)

                'If prn_DetDt.Rows.Count > 0 Then






                Do While prn_HdIndx <= prn_HdMxIndx

                    prn_DetIndx = prn_DetIndx + 1

                    xlCurRow = xlCurRow + 1
                    xlWorkSheet.Cells(xlCurRow, 1) = Val(prn_HdIndx).ToString


                    xlWorkSheet.Cells(xlCurRow, 2) = Trim(prn_HdAr(prn_HdIndx, 1))
                    xlWorkSheet.Cells(xlCurRow, 3) = Trim(prn_HdAr(prn_HdIndx, 3))
                    xlWorkSheet.Cells(xlCurRow, 4) = Trim(prn_HdAr(prn_HdIndx, 4))

                    xlWorkSheet.Cells(xlCurRow, 5) = Val(prn_DetAr(prn_HdIndx, 1, 3))

                    xlWorkSheet.Cells(xlCurRow, 6) = Val(prn_DetAr(prn_HdIndx, 2, 3))

                    xlWorkSheet.Cells(xlCurRow, 7) = Val(prn_DetAr(prn_HdIndx, 3, 3))

                    xlWorkSheet.Cells(xlCurRow, 8) = Val(prn_DetAr(prn_HdIndx, 4, 3))

                    xlWorkSheet.Cells(xlCurRow, 9) = Val(prn_DetAr(prn_HdIndx, 5, 3))

                    xlWorkSheet.Cells(xlCurRow, 10) = Val(prn_DetAr(prn_HdIndx, 6, 3))

                    xlWorkSheet.Cells(xlCurRow, 11) = Val(prn_DetAr(prn_HdIndx, 7, 3))

                    prn_HdIndx = prn_HdIndx + 1
                    prn_Count = prn_Count + 1
                    prn_DetIndx = 0
                Loop

                'End If

                DetRwsCnt = 0



                xlWorkBook.Save()



            Catch ex As Exception
                MessageBox.Show(ex.Message, "DOES NOT PRINT INVOICE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Finally

            End Try

        End If


        da1.Dispose()

    End Sub

    'Private Sub Total_PieceSelection_Calculation()
    '    Dim vTOTPCS As Integer
    '    Dim vTOTMTRS As String
    '    Dim vTOTSELCPCS As Integer
    '    Dim vTOTSELCMTRS As String

    '    If FrmLdSTS = True Then Exit Sub

    '    vTOTPCS = 0
    '    vTOTMTRS = 0
    '    vTOTSELCPCS = 0
    '    vTOTSELCMTRS = 0

    '    For i = 0 To dgv_Selection.Rows.Count - 1

    '        If Trim(dgv_Selection.Rows(i).Cells(2).Value) <> "" And Val(dgv_Selection.Rows(i).Cells(4).Value) <> 0 Then
    '            If dgv_Selection.Rows(i).Visible = True Then
    '                vTOTPCS = vTOTPCS + 1
    '                vTOTMTRS = Format(Val(vTOTMTRS) + Val(dgv_Selection.Rows(i).Cells(4).Value), "##########0.00")
    '            End If


    '            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then
    '                vTOTSELCPCS = vTOTSELCPCS + 1
    '                vTOTSELCMTRS = Format(Val(vTOTSELCMTRS) + Val(dgv_Selection.Rows(i).Cells(4).Value), "##########0.00")
    '            End If

    '        End If

    '    Next

    '    lbl_PcsSelection_TotalPcs.Text = vTOTPCS
    '    lbl_PcsSelection_TotalMeters.Text = Format(Val(vTOTMTRS), "##########0.00")
    '    lbl_PcsSelection_TotalSelectedPcs.Text = vTOTSELCPCS
    '    lbl_PcsSelection_TotalSelectedMeters.Text = Format(Val(vTOTSELCMTRS), "##########0.00")

    'End Sub

    Private Sub cbo_PcsSelection_VendorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' OR Ledger_Type = 'GODOWN' OR (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PcsSelection_VendorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_PcsSelection_MetersTo, btn_PcsSelection_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' OR Ledger_Type = 'GODOWN' OR (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PcsSelection_VendorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' OR Ledger_Type = 'GODOWN' OR (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) )", "(Ledger_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    btn_PcsSelection_Show_Click(sender, e)
        'End If
    End Sub

    Private Sub Printing_BarCode_Sticker_Format4_DosPrint()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = "", vPcs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim vYrCode As String = ""
        Dim prtFrm As String = ""
        Dim prtTo As String = ""
        Dim Condt As String = ""

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_HeadIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        prn_HeadIndx = 0


        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub

        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If


        Try

            da1 = New SqlClient.SqlDataAdapter("Select a.*, tZ.*, c.Cloth_Name, c.Cloth_Description from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and " & Trim(Other_Condition) & IIf(Trim(Condt) <> "", " and ", "") & Condt & " order by a.for_orderby, a.Packing_Slip_Code", con)
            'da1 = New SqlClient.SqlDataAdapter("Select a.*, c.Cloth_Name from Packing_Slip_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)
            If prn_HdDt.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            'prn_HdDt.Dispose()
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

                Do While prn_HeadIndx <= prn_HdDt.Rows.Count - 1

                    vFldMtrs = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString), "##########0.00")
                    vPcs = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Pcs").ToString), "##########0.00")

                    vYrCode = Microsoft.VisualBasic.Right(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_Code").ToString, 5)
                    vBarCode = Microsoft.VisualBasic.Left(vYrCode, 2) & Trim(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Company_IdNo").ToString)) & Trim(UCase(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_No").ToString))
                    vBarCode = Trim(UCase(vBarCode))
                    'vBarCode = "*" & Trim(UCase(vBarCode)) & "*"

                    If Val(vFldMtrs) <> 0 Then


                        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Description").ToString) <> "" Then
                            ItmNm1 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Description").ToString)
                        Else
                            ItmNm1 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString)
                        End If

                        ItmNm2 = ""
                        'If Len(ItmNm1) > 21 Then
                        '    For I = 21 To 1 Step -1
                        '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        '    Next I
                        '    If I = 0 Then I = 21

                        '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I + 1)
                        '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        'End If

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


                        'PrnTxt = "A556,227,2,2,2,2,N,""Sort : """
                        'sw.WriteLine(PrnTxt)
                        PrnTxt = "A544,224,2,2,2,2,N,""" & Trim(ItmNm1) & """"
                        sw.WriteLine(PrnTxt)
                        'PrnTxt = "A553,181,2,2,2,2,N,""" & Trim(ItmNm2) & """"
                        'sw.WriteLine(PrnTxt)
                        PrnTxt = "A553,53,2,2,2,2,N,""Bale:"""
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A555,158,2,2,2,2,N,""Pcs:"""
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A329,59,2,3,2,2,N,""" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_No").ToString) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A463,157,2,2,2,2,N,""" & Trim(Val(vPcs)) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A553,96,2,2,2,2,N,""MTRS:"""
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A433,97,2,2,2,2,N,""" & Trim(Val(vFldMtrs)) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "B391,189,2,1,2,4,66,N,""" & Trim(UCase(vBarCode)) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A295,117,2,1,1,1,N,""" & Trim(UCase(vBarCode)) & """"
                        sw.WriteLine(PrnTxt)

                        PrnTxt = "W1"
                        sw.WriteLine(PrnTxt)

                    End If

                    prn_HeadIndx = prn_HeadIndx + 1

                Loop

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

    Private Sub Printing_BarCode_Sticker_Format4_DosPrint_222()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = "", vPcs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim vYrCode As String = ""
        Dim prtFrm As String = ""
        Dim prtTo As String = ""
        Dim Condt As String = ""

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_HeadIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        prn_HeadIndx = 0


        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub



        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If


        Try

            da1 = New SqlClient.SqlDataAdapter("Select a.*, tZ.*, c.Cloth_Name, c.Cloth_Description from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and " & Trim(Other_Condition) & IIf(Trim(Condt) <> "", " and ", "") & Condt & " order by a.for_orderby, a.Packing_Slip_Code", con)
            'da1 = New SqlClient.SqlDataAdapter("Select a.*, c.Cloth_Name from Packing_Slip_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)
            If prn_HdDt.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            'prn_HdDt.Dispose()
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

                Do While prn_HeadIndx <= prn_HdDt.Rows.Count - 1

                    vFldMtrs = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString), "##########0.00")
                    vPcs = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Pcs").ToString), "##########0.00")

                    vYrCode = Microsoft.VisualBasic.Right(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_Code").ToString, 5)
                    vBarCode = Microsoft.VisualBasic.Left(vYrCode, 2) & Trim(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Company_IdNo").ToString)) & Trim(UCase(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_No").ToString))
                    vBarCode = Trim(UCase(vBarCode))
                    'vBarCode = "*" & Trim(UCase(vBarCode)) & "*"

                    If Val(vFldMtrs) <> 0 Then


                        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Description").ToString) <> "" Then
                            ItmNm1 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Description").ToString)
                        Else
                            ItmNm1 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString)
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


                        PrnTxt = "A556,227,2,2,2,2,N,""Sort : """
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A438,225,2,2,2,2,N,""" & Trim(ItmNm1) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A553,181,2,2,2,2,N,""" & Trim(ItmNm2) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A555,42,2,2,2,2,N,""Bale : """
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A556,134,2,2,2,2,N,""Pcs : """
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A438,42,2,2,2,2,N,""" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_No").ToString) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A460,127,2,2,2,2,N,""" & Trim(vPcs) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A553,87,2,2,2,2,N,""MTRS : """
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A433,83,2,2,2,2,N,""" & Trim(Val(vFldMtrs)) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "B394,190,2,1,2,4,81,N,""" & Trim(UCase(vBarCode)) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A298,103,2,1,1,1,N,""" & Trim(UCase(vBarCode)) & """"
                        sw.WriteLine(PrnTxt)

                        PrnTxt = "W1"
                        sw.WriteLine(PrnTxt)

                    End If

                    prn_HeadIndx = prn_HeadIndx + 1

                Loop


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

    Private Sub Printing_BarCode_Sticker_Format4_DosPrint_111()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = "", vPcs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim vYrCode As String = ""
        Dim prtFrm As String = ""
        Dim prtTo As String = ""
        Dim Condt As String = ""

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_HeadIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        prn_HeadIndx = 0


        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub




        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If


        Try

            da1 = New SqlClient.SqlDataAdapter("Select a.*, tZ.*, c.Cloth_Name, c.Cloth_Description from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and " & Trim(Other_Condition) & IIf(Trim(Condt) <> "", " and ", "") & Condt & " order by a.for_orderby, a.Packing_Slip_Code", con)
            'da1 = New SqlClient.SqlDataAdapter("Select a.*, c.Cloth_Name from Packing_Slip_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)
            If prn_HdDt.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            prn_HdDt.Dispose()
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

                Do While prn_HeadIndx <= prn_HdDt.Rows.Count - 1

                    vFldMtrs = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString), "##########0.00")
                    vPcs = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Pcs").ToString), "##########0.00")

                    vYrCode = Microsoft.VisualBasic.Right(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_Code").ToString, 5)
                    vBarCode = Microsoft.VisualBasic.Left(vYrCode, 2) & Trim(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Company_IdNo").ToString)) & Trim(UCase(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_No").ToString))
                    vBarCode = Trim(UCase(vBarCode))
                    'vBarCode = "*" & Trim(UCase(vBarCode)) & "*"

                    If Val(vFldMtrs) <> 0 Then


                        'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                        '    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        'Else
                        ItmNm1 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString)
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


                        PrnTxt = "A556,227,2,2,2,2,N,""Sort : " & Trim(ItmNm1) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A556,185,2,2,2,2,N,""" & Trim(ItmNm2) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A567,145,2,2,2,2,N,""Bale No  : """
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A567,99,2,2,2,2,N,""Pcs : """
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A453,139,2,2,2,2,N,""" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_No").ToString) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A447,99,2,2,2,2,N,""" & Trim(vPcs) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A564,46,2,2,2,2,N,""MTRS : """
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A453,46,2,2,2,2,N,""" & Trim(Val(vFldMtrs)) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "B329,150,2,1,2,4,73,N,""" & Trim(UCase(vBarCode)) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "A326,71,2,1,2,2,N,""" & Trim(UCase(vBarCode)) & """"
                        sw.WriteLine(PrnTxt)

                        PrnTxt = "W1"
                        sw.WriteLine(PrnTxt)

                    End If

                    prn_HeadIndx = prn_HeadIndx + 1

                Loop


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

    Private Sub cbo_Cloth_TextChanged(sender As Object, e As EventArgs)
        Dim Clo_IdNo As Integer
        'Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

        'txt_Tare_weight.Text = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "RollTube_Wgt", "(cloth_idno = " & Str(Val(Clo_IdNo)) & " )"))

    End Sub

    Private Sub txt_net_weight_KeyDown(sender As Object, e As KeyEventArgs)
        Dim vTOTWGT As String = 0

        If FrmLdSTS = True Then Exit Sub

        vTOTWGT = 0
        If dgv_Details_Total.Rows.Count > 0 Then

            vTOTWGT = Val(dgv_Details_Total.Rows(0).Cells(5).Value)

        End If

        If Val(vTOTWGT) <> 0 Then
            'txt_net_weight.Text = Format(Val(vTOTWGT), "#########0.000")
            'e.Handled = True
            'e.SuppressKeyPress = True
        End If

    End Sub



    Private Sub cbo_Sales_Party_Name_GotFocus(sender As Object, e As EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Sales_Party_Name_KeyDown(sender As Object, e As KeyEventArgs)
        vcbo_KeyDwnVal = e.KeyValue
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sales_Party_Name, txt_Note, Lbl_DelvCode, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Sales_Party_Name_KeyPress(sender As Object, e As KeyPressEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sales_Party_Name, Lbl_DelvCode, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Sales_Party_Name_KeyUp(sender As Object, e As KeyEventArgs)
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    Common_Procedures.MDI_LedType = ""
        '    Dim f As New Ledger_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_Sales_Party_Name.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()

        'End If
    End Sub

    Private Sub cbo_BaleSuffixNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_BitsGroup_Head", "Cloth_BitsGroup_Name", "", "(Cloth_BitsGroup_Name='')")
    End Sub
    Private Sub cbo_BaleSuffixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_Note, msk_Plan_date, "Cloth_BitsGroup_Head", "Cloth_BitsGroup_Name", "", "(Cloth_BitsGroup_Name='')")
    End Sub

    Private Sub cbo_BaleSuffixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, msk_Plan_date, "Cloth_BitsGroup_Head", "Cloth_BitsGroup_Name", "", "(Cloth_BitsGroup_Name='')")
    End Sub

    Private Sub cbo_BaleSuffixNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.Control = False And e.KeyValue = 17 Then
        '    Dim f As New Cloth_BitsGroup_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_BaleSuffixNo.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""
        '    f.MdiParent = MDIParent1
        '    f.Show()

        'End If
    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        EMAIL_Status = False
        WHATSAPP_Status = False
        print_record()
        'Print_PDF_Status = False
    End Sub

    Private Sub dgv_Selection_CellEnter(sender As Object, e As DataGridViewCellEventArgs)
        'dgv_ActCtrlName = dgv_Selection.Name
        'If Val(dgv_Selection.Rows(e.RowIndex).Cells(8).Value) = 1 Then
        '    dgv_Selection.DefaultCellStyle.SelectionForeColor = Color.Red
        'Else
        '    dgv_Selection.DefaultCellStyle.SelectionForeColor = Color.Blue
        'End If
    End Sub

    Private Sub dgv_Details_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellEnter


        Try

            dgv_ActCtrlName = dgv_Details.Name

            If FrmLdSTS = True Then Exit Sub

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details
                If .Visible Then
                    '    If .Rows.Count > 0 Then
                    '        If e.ColumnIndex = 11 Or e.ColumnIndex = 12 Or e.ColumnIndex = 13 Then
                    '            If Trim(.Rows(e.RowIndex).Cells(12).Value) = "" Then
                    '                If Val(.Rows(e.RowIndex).Cells(13).Value) > 0 Then
                    '                    .Rows(e.RowIndex).Cells(12).Value = get_Max_PieceNumber_of_LotCode(.Rows(e.RowIndex).Cells(8).Value, .Rows(e.RowIndex).Cells(2).Value)
                    '                End If

                    '            Else

                    '                If Val(.Rows(e.RowIndex).Cells(13).Value) <= 0 Then
                    '                    .Rows(e.RowIndex).Cells(12).Value = ""
                    '                End If

                    '            End If
                    '        End If
                    '    End If


                End If
            End With

        Catch ex As Exception
            '---

        End Try




    End Sub

    Private Sub dgv_Filter_Details_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellEnter
        dgv_ActCtrlName = dgv_Filter_Details.Name
    End Sub

    Private Sub dgv_Details_GotFocus(sender As Object, e As EventArgs) Handles dgv_Details.GotFocus
        dgv_ActCtrlName = dgv_Details.Name
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

    Private Function get_Max_PieceNumber_of_LotCode(ByVal vLOTCODE As String, ByVal vPCSNO As String) As String
        Dim cn1 As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim SQL1 As String = ""
        Dim vPCSSUBNO As String = ""
        Dim vLASTPCSNO As String = ""
        Dim vMAXPCSNO As String = ""
        Dim vMAXPCSPREFIXNO As String = ""
        Dim vMAXPCSSUFFIXNO As String = ""
        Dim I, K As Integer
        Dim vPCNo_OrdBy As String


        vLASTPCSNO = ""
        vPCSSUBNO = ""


        vPCSNO = Trim(UCase(vPCSNO))

        vPCSSUBNO = vPCSNO
        If InStr(1, vPCSNO, "A") > 0 Or InStr(1, vPCSNO, "B") > 0 Or InStr(1, vPCSNO, "C") > 0 Or InStr(1, vPCSNO, "D") > 0 Or InStr(1, vPCSNO, "E") > 0 Then
            K = InStr(1, vPCSNO, "A")
            If K <= 0 Then K = InStr(1, vPCSNO, "B")
            If K <= 0 Then K = InStr(1, vPCSNO, "C")
            If K <= 0 Then K = InStr(1, vPCSNO, "D")
            If K <= 0 Then K = InStr(1, vPCSNO, "E")

            If K > 0 Then
                vPCSSUBNO = Microsoft.VisualBasic.Left(vPCSNO, K)
            End If

        End If

        cn1.Open()

        cmd.Connection = cn1

        vMAXPCSNO = ""
        If Trim(vLOTCODE) <> "" And Trim(vPCSSUBNO) <> "" Then

            cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempSimpleTable)
            cmd.ExecuteNonQuery()

            SQL1 = "Insert into " & Trim(Common_Procedures.EntryTempSimpleTable) & " ( meters1, name1 ) Select a.PieceNo_OrderBy, a.Piece_No from Weaver_ClothReceipt_Piece_Details a Where a.lot_code = '" & Trim(vLOTCODE) & "'"
            cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            cmd.ExecuteNonQuery()

            For I = 0 To dgv_Details.Rows.Count - 1

                If Val(dgv_Details.Rows(I).Cells(4).Value) <> 0 And Trim(dgv_Details.Rows(I).Cells(12).Value) <> "" Then

                    If Trim(UCase(dgv_Details.Rows(I).Cells(8).Value)) = Trim(UCase(vLOTCODE)) Then

                        vPCNo_OrdBy = Val(Common_Procedures.OrderBy_CodeToValue(Trim(UCase(dgv_Details.Rows(I).Cells(12).Value))))

                        SQL1 = "Insert into " & Trim(Common_Procedures.EntryTempSimpleTable) & " ( meters1, name1 ) Values (" & Str(Val(vPCNo_OrdBy)) & ", '" & Trim(UCase(dgv_Details.Rows(I).Cells(12).Value)) & "')"
                        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                        cmd.ExecuteNonQuery()

                    End If

                End If

            Next

            SQL1 = "Select TOP 1 a.Name1 as Piece_No from " & Trim(Common_Procedures.EntryTempSimpleTable) & " a Where a.name1 LIKE '" & Trim(vPCSSUBNO) & "%' order by meters1 DESC, name1 DESC"
            'SQL1 = "Select TOP 1 a.Piece_No from Weaver_ClothReceipt_Piece_Details a Where a.lot_code = '" & Trim(vLOTCODE) & "' and a.Piece_No LIKE '" & Trim(vPCSSUBNO) & "%' order by a.Weaver_ClothReceipt_Date DESC, a.for_orderby DESC, a.Weaver_ClothReceipt_No DESC, a.PieceNo_OrderBy DESC, a.Piece_No DESC"
            cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                vMAXPCSNO = Dt1.Rows(0).Item("Piece_No").ToString
            End If
            Dt1.Clear()

        End If


        Dt1.Dispose()
        Da.Dispose()

        cn1.Close()
        cn1.Dispose()


        vLASTPCSNO = ""
        If Trim(vMAXPCSNO) <> "" Then

            vMAXPCSNO = Trim(UCase(vMAXPCSNO))
            vPCSSUBNO = ""
            vMAXPCSPREFIXNO = ""
            vMAXPCSSUFFIXNO = ""
            If InStr(1, vMAXPCSNO, "A") > 0 Or InStr(1, vMAXPCSNO, "B") > 0 Or InStr(1, vMAXPCSNO, "C") > 0 Or InStr(1, vMAXPCSNO, "D") > 0 Or InStr(1, vMAXPCSNO, "E") > 0 Then
                K = InStr(1, vMAXPCSNO, "A")
                If K <= 0 Then K = InStr(1, vMAXPCSNO, "B")
                If K <= 0 Then K = InStr(1, vMAXPCSNO, "C")
                If K <= 0 Then K = InStr(1, vMAXPCSNO, "D")
                If K <= 0 Then K = InStr(1, vMAXPCSNO, "E")

                If K > 0 Then
                    vMAXPCSPREFIXNO = Microsoft.VisualBasic.Left(vMAXPCSNO, K)
                    vMAXPCSSUFFIXNO = Microsoft.VisualBasic.Right(vMAXPCSNO, Len(vMAXPCSNO) - K)
                    vLASTPCSNO = Trim(vMAXPCSPREFIXNO) & Trim(Val(vMAXPCSSUFFIXNO) + 1)
                Else
                    GoTo GO_TO_LOOP1
                End If

            Else

GO_TO_LOOP1:
                vLASTPCSNO = Trim(UCase(vMAXPCSNO)) & "A"

            End If

        End If

        Return vLASTPCSNO

    End Function

    Private Sub Fabric_Stock_Display()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1490" Then '---- LAKSHMI SARASWATHI EXPORTS (THIRUCHENCODE)
            Exit Sub
        End If

        Dim cn1 As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vSTKOFF_IdNo As Integer
        Dim vGdwn_IdNo As Integer
        Dim Clth_ID As Integer
        Dim vFLDNGPERC As String
        Dim vCURRSTOCK_TYPE1 As String = 0
        Dim vCURRSTOCK_TYPE2 As String = 0
        Dim vCURRSTOCK_TYPE3 As String = 0
        Dim vCURRSTOCK_TYPE4 As String = 0
        Dim vCURRSTOCK_TYPE5 As String = 0
        Dim n As Integer

        cn1.Open()

        'Clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If Clth_ID = 0 Then
            Exit Sub
        End If


        'vSTKOFF_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName_StockOF.Text)
        If vSTKOFF_IdNo = 0 Then vSTKOFF_IdNo = Common_Procedures.CommonLedger.OwnSort_Ac

        'vGdwn_IdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_Godown_StockIN.Text)
        If vGdwn_IdNo = 0 Then vGdwn_IdNo = Common_Procedures.CommonLedger.Godown_Ac


        'vFLDNGPERC = Val(txt_Folding.Text)
        If Val(vFLDNGPERC) = 0 Then
            vFLDNGPERC = 100
        End If

        Dim vFAB_UPTO_DateSTOCK_TYPE1 As String, vFAB_UPTO_DateSTOCK_TYPE2 As String, vFAB_UPTO_DateSTOCK_TYPE3 As String, vFAB_UPTO_DateSTOCK_TYPE4 As String, vFAB_UPTO_DateSTOCK_TYPE5 As String


        Common_Procedures.get_Fabric_CurrentStock(cn1, Val(lbl_Company.Tag), vSTKOFF_IdNo, vGdwn_IdNo, Now.Date, Clth_ID, 0, Val(vFLDNGPERC), vCURRSTOCK_TYPE1, vCURRSTOCK_TYPE2, vCURRSTOCK_TYPE3, vCURRSTOCK_TYPE4, vCURRSTOCK_TYPE5, vFAB_UPTO_DateSTOCK_TYPE1, vFAB_UPTO_DateSTOCK_TYPE2, vFAB_UPTO_DateSTOCK_TYPE3, vFAB_UPTO_DateSTOCK_TYPE4, vFAB_UPTO_DateSTOCK_TYPE5, Nothing)
        'Common_Procedures.get_Fabric_CurrentStock(cn1, Val(lbl_Company.Tag), vSTKOFF_IdNo, vGdwn_IdNo, Clth_ID, Val(vFLDNGPERC), vCURRSTOCK_TYPE1, vCURRSTOCK_TYPE2, vCURRSTOCK_TYPE3, vCURRSTOCK_TYPE4, vCURRSTOCK_TYPE5)

        dgv_StockDisplay.Rows.Clear()
        n = dgv_StockDisplay.Rows.Add
        If Val(vCURRSTOCK_TYPE1) <> 0 Then
            dgv_StockDisplay.Rows(n).Cells(0).Value = Format(Val(vCURRSTOCK_TYPE1), "##########0.00")
        End If
        If Val(vCURRSTOCK_TYPE2) <> 0 Then
            dgv_StockDisplay.Rows(n).Cells(1).Value = Format(Val(vCURRSTOCK_TYPE2), "##########0.00")
        End If
        If Val(vCURRSTOCK_TYPE3) <> 0 Then
            dgv_StockDisplay.Rows(n).Cells(2).Value = Format(Val(vCURRSTOCK_TYPE3), "##########0.00")
        End If
        If Val(vCURRSTOCK_TYPE4) <> 0 Then
            dgv_StockDisplay.Rows(n).Cells(3).Value = Format(Val(vCURRSTOCK_TYPE4), "##########0.00")
        End If
        If Val(vCURRSTOCK_TYPE5) <> 0 Then
            dgv_StockDisplay.Rows(n).Cells(4).Value = Format(Val(vCURRSTOCK_TYPE5), "##########0.00")
        End If
        dgv_StockDisplay.CurrentCell.Selected = False
        pnl_StockDisplay.Visible = True
        pnl_StockDisplay.BringToFront()

    End Sub

    Private Sub cbo_Cloth_Enter(sender As Object, e As EventArgs)
        Fabric_Stock_Display()
    End Sub

    Private Sub cbo_ClothType_Enter(sender As Object, e As EventArgs) Handles cbo_Checking_Section.Enter
        'Fabric_Stock_Display()
    End Sub

    Private Sub cbo_Bale_Bundle_Enter(sender As Object, e As EventArgs)
        Fabric_Stock_Display()
    End Sub

    Private Sub txt_Folding_Enter(sender As Object, e As EventArgs)
        Fabric_Stock_Display()
    End Sub

    Private Function Calculation_Beam_ConsumptionPavu(ByVal vCloth_ID As Integer, ByVal vLOOM_ID As Integer, ByVal vTOT_PCSMTRS As String, ByVal vPCS_WidthType As String, ByVal vPCS_CrimpPerc As String, ByVal vPCS_BeamNo1 As String, ByVal vPCS_BeamNo2 As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As String
        Dim vConsPavu As String = 0
        Dim vPCS_BeamConsMtrs As String = 0
        Dim NoofBeams As Integer = 0

        vConsPavu = Common_Procedures.get_Pavu_Consumption(con, vCloth_ID, vLOOM_ID, Val(vTOT_PCSMTRS), Trim(vPCS_WidthType), sqltr, Val(vPCS_CrimpPerc))

        NoofBeams = 0
        If Trim(vPCS_BeamNo1) <> "" And Trim(vPCS_BeamNo2) <> "" Then
            NoofBeams = 2
        Else
            NoofBeams = 1
        End If
        If Val(NoofBeams) = 0 Then NoofBeams = 1

        vPCS_BeamConsMtrs = Format(Val(vConsPavu) / NoofBeams, "#########0.00")

        Return vPCS_BeamConsMtrs

    End Function

    Private Sub Printing_PackingSlip_Sticker_Format_1155(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdDt As DataTable, ByVal prn_HdMxIndx As Integer, ByVal prn_DetMxIndx As Integer, ByRef prn_HdAr(,) As String, ByRef prn_DetAr(,,) As String, ByRef prn_PageNo As Integer, ByRef prn_Count As Integer, ByRef prn_HdIndx As Integer, ByRef prn_DetIndx As Integer, ByRef vtot_wgt As Integer, ByRef vtot_pcs As Integer, ByVal lst_print As Boolean)
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim LM As Single = 0, TM As Single = 0
        Dim PgWt As Single = 0, PrWt As Single = 0
        Dim PgHt As Single = 0, PrHt As Single = 0


        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 2X4", 400, 200)
        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 2X4", 200, 400)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        e.PageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 20
            .Top = 5 ' 10
            .Bottom = 5 ' 10
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 10, FontStyle.Regular)

        NoofItems_PerPage = 5 '4 '  7 ' 8 

        Erase ClArr
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 180 ' 100
        ClArr(2) = PageWidth - (LMargin + ClArr(1))

        TxtHgt = 13.75 ' 13.9 ' 14 ' 15 ' 17 ' 17.6 ' 17.75 '18.75 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Try

            If prn_HdDt.Rows.Count > 0 Then

                If prn_HdMxIndx > 0 Then

                    Do While prn_HdIndx <= prn_HdMxIndx

                        LM = LMargin
                        TM = TMargin
                        PgWt = PageWidth
                        PgHt = PageHeight
                        PrWt = PrintWidth
                        PrHt = PrintHeight

                        Erase LnAr
                        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

                        Printing_PackingSlip_Sticker_Format_1155_PageHeader(PrintDocument1, e, prn_HdDt, prn_HdAr, TxtHgt, pFont, LM, RMargin, TM, BMargin, PgWt, PrWt, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, prn_HdIndx)
                        CurY = CurY - 10

                        NoofDets = 0
                        Do While prn_DetIndx < Val(prn_HdAr(prn_HdIndx, 3))

                            If NoofDets >= NoofItems_PerPage Then


                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LM + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1

                                Printing_PackingSlip_Sticker_Format_1155_PageFooter(PrintDocument1, e, prn_HdAr, TxtHgt, pFont, LM, RMargin, TM, BMargin, PgWt, PrWt, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, vtot_wgt, vtot_pcs, False, prn_Count, lst_print)


                                e.HasMorePages = True

                                prn_Count = prn_Count + 1

                                vtot_pcs = 0
                                vtot_wgt = 0
                                Return

                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If Val(prn_DetAr(prn_HdIndx, prn_DetIndx, 3)) <> 0 Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, prn_DetIndx, 2)), LM + 20, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_HdIndx, prn_DetIndx, 3)), "#########0.00"), LM + ClArr(1) + ClArr(2) - 20, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                            End If

                        Loop

                        Printing_PackingSlip_Sticker_Format_1155_PageFooter(PrintDocument1, e, prn_HdAr, TxtHgt, pFont, LM, RMargin, TM, BMargin, PgWt, PrWt, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, vtot_wgt, vtot_pcs, True, prn_Count, lst_print)

                        prn_HdIndx = prn_HdIndx + 1
                        prn_Count = prn_Count + 1
                        prn_DetIndx = 0

                        If prn_HdIndx <= prn_HdMxIndx Then

                            e.HasMorePages = True
                            Return

                        End If

                    Loop

                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_PackingSlip_Sticker_Format_1155_PageHeader(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdDt As DataTable, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal prn_HdIndx As Integer)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim i As Integer
        Dim W1 As Single, LFT1 As Single, TP1 As Single
        Dim PrntWt1 As Single = 0
        Dim PrntWt2 As Single = 0
        Dim Cmp_Name As String = ""
        Dim Cmp_Add As String = ""
        Dim Cmp_Phone As String = ""
        Dim vHEADING As String
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim strWidth As Single
        Dim br1 As SolidBrush, br2 As SolidBrush


        PageNo = PageNo + 1

        CurY = TMargin


        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_City").ToString) <> "" Then
            Cmp_Add = Trim(prn_HdDt.Rows(0).Item("Company_City").ToString) & IIf(Trim(prn_HdDt.Rows(0).Item("Company_PinCode").ToString) <> "", "-", "") & Trim(prn_HdDt.Rows(0).Item("Company_PinCode").ToString)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString) <> "" Then
            Cmp_Add = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) <> "" Then
            Cmp_Add = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString) <> "" Then
            Cmp_Add = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString) <> "" Then
            Cmp_Add = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
        End If

        Cmp_Phone = ""
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_Phone = "Phone : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
        End If


        PrntWt1 = ((PrintDocument1.DefaultPageSettings.PaperSize.Width / 3) * 2) - PrintDocument1.DefaultPageSettings.Margins.Right - PrintDocument1.DefaultPageSettings.Margins.Left

        LFT1 = PrntWt1 + 45
        PrntWt2 = (PrintDocument1.DefaultPageSettings.PaperSize.Width / 3) - PrintDocument1.DefaultPageSettings.Margins.Right - PrintDocument1.DefaultPageSettings.Margins.Left
        'PrntWt = PrintDocument1.DefaultPageSettings.PaperSize.Width - PrintDocument1.DefaultPageSettings.Margins.Right - PrintDocument1.DefaultPageSettings.Margins.Left

        CurY = TMargin
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrntWt1, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 5
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add & "    " & Cmp_Phone, LMargin, CurY, 2, PrntWt1, pFont)
        strHeight = e.Graphics.MeasureString(Cmp_Add, p1Font).Height


        'CurY = CurY + TxtHgt
        'p1Font = New Font("Calibri", 10, FontStyle.Bold)

        TP1 = TMargin

        vHEADING = "PACKING SLIP"
        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, vHEADING, LFT1, TP1, 2, PrntWt2, p1Font)

        'Y1 = TP1 - 4 + 0.5
        'Y2 = TP1 + TxtHgt + 15
        'strWidth = e.Graphics.MeasureString(Trim(vHEADING), p1Font).Width
        'br1 = New SolidBrush(Color.Black)
        'Common_Procedures.FillRegionRectangle(e, LFT1, Y1, LFT1 + strWidth + 10, Y2, br1)
        'br2 = New SolidBrush(Color.White)
        'Common_Procedures.Print_To_PrintDocument(e, vHEADING, LFT1 + 5, TP1 + 2, 2, PrntWt2, p1Font, br2)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt

        Try

            W1 = e.Graphics.MeasureString("QUALITY   :  ", pFont).Width

            p1Font = New Font("Calibri", 8, FontStyle.Regular)

            Dim vPACKTYPE As String = ""
            vPACKTYPE = prn_HdAr(prn_HdMxIndx, 5)
            If Trim(vPACKTYPE) = "" Then vPACKTYPE = "ROLL"

            CurY = CurY + 6
            Common_Procedures.Print_To_PrintDocument(e, Trim(UCase(vPACKTYPE)) & " NO", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1, CurY, 0, 0, p1Font)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdAr(prn_HdIndx, 1), LMargin + W1 + 10, CurY - 3, 0, 0, p1Font)

            Dim ItmNm1 As String, ItmNm2 As String
            ItmNm1 = prn_HdAr(prn_HdIndx, 2)
            ItmNm2 = ""
            If Len(ItmNm1) > 50 Then
                For i = 50 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 50
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If

            CurY = CurY + TxtHgt + 2
            p1Font = New Font("Calibri", 8, FontStyle.Regular)

            Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + W1 + 10, CurY, 0, 0, p1Font)

            If Trim(ItmNm2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm2, LMargin + W1 + 25, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 3
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            CurY = CurY + TxtHgt - 12
            Common_Procedures.Print_To_PrintDocument(e, "PCS NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

            CurY = CurY + TxtHgt + 3
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_PackingSlip_Sticker_Format_1155_PageFooter(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal prn_HdIndx As Integer, ByRef vtot_wgt As Integer, ByRef vtot_pcs As Integer, ByVal is_LastPage As Boolean, ByRef prn_Count As Integer, ByVal lst_print As Boolean)
        Dim I As Integer
        Dim PrntWt As Single = 0

        Try

            PrntWt = PrintDocument1.DefaultPageSettings.PaperSize.Width - PrintDocument1.DefaultPageSettings.Margins.Right - PrintDocument1.DefaultPageSettings.Margins.Left

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt + 1
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            CurY = CurY + TxtHgt - 14

            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdAr(prn_HdIndx, 3))), LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdAr(prn_HdIndx, 4)), "#########0.00"), LMargin + ClAr(1) + ClAr(2) - 20, CurY, 1, 0, pFont)

            vtot_pcs = vtot_pcs + Val(prn_HdAr(prn_HdIndx, 3))
            vtot_wgt = vtot_wgt + Val(prn_HdAr(prn_HdIndx, 4))
            lst_print = True

            CurY = CurY + TxtHgt + 3
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(1))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


        End Try

    End Sub

    Private Sub msk_Allotment_Date_LostFocus(sender As Object, e As EventArgs) Handles msk_Allotment_Date.LostFocus
        If IsDate(msk_Allotment_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Allotment_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Allotment_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Allotment_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Allotment_Date.Text)) >= 2000 Then
                    Dtp_Allotment_Date.Value = Convert.ToDateTime(msk_Allotment_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub msk_Allotment_Date_KeyUp(sender As Object, e As KeyEventArgs) Handles msk_Allotment_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_Allotment_Date.Text = Date.Today
        End If
        If e.KeyCode = 107 Then
            msk_Allotment_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Allotment_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Allotment_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Allotment_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If
    End Sub

    Private Sub msk_Allotment_Date_KeyDown(sender As Object, e As KeyEventArgs) Handles msk_Allotment_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Allotment_Date.Text
            vmskSelStrt = msk_Allotment_Date.SelectionStart
        End If
    End Sub



    Private Sub Dtp_Allotment_Date_TextChanged(sender As Object, e As EventArgs) Handles Dtp_Allotment_Date.TextChanged
        If FrmLdSTS = True Then Exit Sub

        If IsDate(Dtp_Allotment_Date.Text) = True Then

            msk_Allotment_Date.Text = Dtp_Allotment_Date.Text
            msk_Allotment_Date.SelectionStart = 0

        End If

    End Sub

    Private Sub Dtp_Allotment_Date_KeyUp(sender As Object, e As KeyEventArgs) Handles Dtp_Allotment_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dtp_Allotment_Date.Text = Date.Today
        End If
    End Sub

    Private Sub cbo_Checking_Section_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Checking_Section.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Checking_Section_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Checking_Section.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub ClothReceipt_Selection(ByVal RwIndx As Integer)
        Dim i As Integer

        With Dgv_ClothReceipt_Selection



            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(7).Value = (Val(.Rows(RwIndx).Cells(7).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(7).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(7).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If




        End With

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Btn_ClothReceipt_Close.Click

        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim vRec_Nos As String = ""
        Dim Rec_No As String = ""
        dgv_Details.Rows.Clear()

        For i = 0 To Dgv_ClothReceipt_Selection.RowCount - 1

            If Val(Dgv_ClothReceipt_Selection.Rows(i).Cells(7).Value) = 1 Then

                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)

                dgv_Details.Rows(n).Cells(1).Value = Dgv_ClothReceipt_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(2).Value = Dgv_ClothReceipt_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(3).Value = Dgv_ClothReceipt_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(4).Value = Dgv_ClothReceipt_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(5).Value = Dgv_ClothReceipt_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(6).Value = Dgv_ClothReceipt_Selection.Rows(i).Cells(6).Value
                dgv_Details.Rows(n).Cells(8).Value = Dgv_ClothReceipt_Selection.Rows(i).Cells(8).Value
                dgv_Details.Rows(n).Cells(9).Value = Dgv_ClothReceipt_Selection.Rows(i).Cells(9).Value



            End If




        Next


        Pnl_Back.Enabled = True
        Pnl_ClothReceipt_Selection.Visible = False

        If dgv_Details.Enabled And dgv_Details.Visible Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If dgv_Details.CurrentCell.RowIndex >= 0 Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(7)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
        End If
    End Sub

    Private Sub Dgv_ClothReceipt_Selection_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv_ClothReceipt_Selection.CellClick
        ClothReceipt_Selection(e.RowIndex)
    End Sub

    Private Sub Dgv_ClothReceipt_Selection_KeyDown(sender As Object, e As KeyEventArgs) Handles Dgv_ClothReceipt_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If Dgv_ClothReceipt_Selection.CurrentCell.RowIndex >= 0 Then

                n = Dgv_ClothReceipt_Selection.CurrentCell.RowIndex

                ClothReceipt_Selection(n)

                e.Handled = True

            End If
        End If
    End Sub



    Private Sub cbo_Filter_ClothType_GotFocus(sender As Object, e As EventArgs) Handles cbo_Filter_CheckingSection.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Checking_Section_Head", "Checking_Section_Name", "", "Checking_Section_Name")
    End Sub

    Private Sub msk_Allotment_Date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_Allotment_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Allotment_Date.Text = Date.Today
            msk_Allotment_Date.SelectionStart = 0
        End If


        If Asc(e.KeyChar) = 13 Then
            cbo_Checking_Section.Focus()
        End If
    End Sub


End Class