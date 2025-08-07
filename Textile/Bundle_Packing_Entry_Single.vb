Imports System.Runtime.Remoting

Public Class Bundle_Packing_Entry_Single
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private SaveAll_STS As Boolean = False
    Private dgvDet_CboBx_ColNos_Arr As Integer() = {1}
    Private vEntryType As String = ""
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private Pk_Condition As String = "BPAES-"

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private vEntFnYrCode As String = ""

    Private LastNo As String = ""

    Private vMain_ClothName As String = ""

    Public Sub New(ByVal EntryType As String)
        vEntryType = Trim(UCase(EntryType))

        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        txt_note.Text = ""

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        'txt_bundleno.Enabled = False
        txt_bundleno.Text = ""
        txt_bundleno.ForeColor = Color.Black

        msk_date.Text = ""
        msk_date.SelectionStart = 0
        dtp_Date.Text = ""

        cbo_Filter_EmployeeName.Text = ""
        ' ' ' cbo_Filter_MillName.Text .Text   = ""
        cbo_Filter_ClothName.Text = ""
        Cbo_Employee_Name.Text = ""
        cbo_Partyname.Text = ""
        lbl_InvoiceNo.Text = ""

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        ' lbl_fabricname.Text = vMain_ClothName

        cbo_FabName.Text = vMain_ClothName

        ' lbl_fabricname.Text = ""
        txt_bundleno.Text = ""
        txt_mark.Text = ""
        txt_Pcs.Text = ""
        lbl_yards.Text = ""

        cbo_FabName.Text = ""

        lbl_AvailableStock.Tag = 0
        lbl_AvailableStock.Text = ""

        lbl_RefNo.BackColor = Color.White

        Cbo_Employee_Name.Enabled = True
        Cbo_Employee_Name.BackColor = Color.White

        cbo_Partyname.Enabled = True
        cbo_Partyname.BackColor = Color.White

        msk_date.Enabled = True
        msk_date.BackColor = Color.White

        dtp_Date.Enabled = True
        dtp_Date.BackColor = Color.White

        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False

        cbo_FabName.Tag = ""

        cbo_WareHouse.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_EmployeeName.Text = ""
            cbo_Filter_ClothName.SelectedIndex = -1
            cbo_Filter_EmployeeName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        'cbo_Grid_FabricName.Visible = False
        'cbo_Grid_FabricName.Top = 2000

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

        'If Me.ActiveControl.Name <> cbo_Grid_FabricName.Name Then
        '    cbo_Grid_FabricName.Visible = False
        '    cbo_Grid_FabricName.Top = 2000
        'End If

        'If Me.ActiveControl.Name <> dgv_Details.Name Then
        '    Grid_Cell_DeSelect()
        'End If

        Show_Pcs_CurrentStock()

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
        'On Error Resume Next
        'dgv_Details.CurrentCell.Selected = False
        'dgv_Details_Total.CurrentCell.Selected = False
        'dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub bundle_packing_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_FabricName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Grid_FabricName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                'lbl_Company.Tag = 1
                ' lbl_Company.Text = Common_Procedures.Company_IdNoToName(con, 1)
                Me.Text = lbl_Company.Text
                new_record()
                ' Pnl_Fabric_selection.Visible = True
                ' Pnl_Back.Enabled = False

            End If



        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub bundle_packing_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub bundle_packing_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        ' Dim CompCondt As String


        Dim OpYrCode As String = ""

        Me.Text = ""

        txt_bundleno.ReadOnly = True
        If Trim(Common_Procedures.settings.CustomerCode) = "1414" Then
            txt_bundleno.ReadOnly = False

            lbl_WareHouse.Visible = True
            cbo_WareHouse.Visible = True
            txt_note.Width = Cbo_Employee_Name.Width

        Else

            lbl_WareHouse.Visible = False
            cbo_WareHouse.Visible = False

        End If
        If Trim(UCase(vEntryType)) = "OPENING" Then
            Pk_Condition = "BPSOP-"
            Label1.Text = "OPENING BUNDLES"

            'OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            'OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            vEntFnYrCode = "OPENI" 'Trim(OpYrCode)

        Else

            Pk_Condition = "BPAES-"
            Label1.Text = "BUNDLE PACKING ENTRY SINGLE"
            vEntFnYrCode = Trim(Common_Procedures.FnYearCode)

        End If

        con.Open()

        dtp_Date.Text = ""
        msk_date.Text = ""
        msk_date.SelectionStart = 0

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2


        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            chk_Printed.Enabled = True
        End If


        'Pnl_Fabric_selection.BackColor = Me.BackColor
        'Pnl_Fabric_selection.Visible = False
        'Pnl_Fabric_selection.Enabled = True
        'Pnl_Fabric_selection.BringToFront()

        'Pnl_Fabric_selection.Left = (Me.Width - Pnl_Fabric_selection.Width) \ 2
        'Pnl_Fabric_selection.Top = (Me.Height - Pnl_Fabric_selection.Height) \ 2
        'Pnl_Back.Enabled = False


        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_bundleno.GotFocus, AddressOf ControlGotFocus
        '        AddHandler cbo_Grid_FabricName.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_EmployeeName.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Employee_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Partyname.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_mark.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_FabName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_filter_bundle_no.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WareHouse.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_note.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_bundleno.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WareHouse.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_FabName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Partyname.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        ' AddHandler cbo_Grid_FabricName.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Employee_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_mark.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_filter_bundle_no.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_EmployeeName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler txt_note.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_mark.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_filter_bundle_no.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler txt_note.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_mark.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_filter_bundle_no.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 1    '0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

        lbl_fabricname.Text = ""

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head  order by Cloth_Name", con)
        da.Fill(dt1)
        cbo_Fabriname.DataSource = dt1
        cbo_Fabriname.DisplayMember = "Cloth_Name"



    End Sub

    Private Sub bundle_packing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf Pnl_Fabric_selection.Visible = True Then
                    Me.Close()
                    Exit Sub

                Else
                    Close_Form()
                    Exit Sub

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    ''Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
    ''    Dim I As Integer = 0
    ''    Dim dgv1 As New DataGridView

    ''    If ActiveControl.Name = dgv_Details.Name Or ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

    ''        'On Error Resume Next

    ''        dgv1 = Nothing

    ''        If ActiveControl.Name = dgv_Details.Name Then
    ''            dgv1 = dgv_Details


    ''        ElseIf dgv_Details.IsCurrentRowDirty = True Then
    ''            dgv1 = dgv_Details



    ''        ElseIf Pnl_Back.Enabled = True Then
    ''            dgv1 = dgv_Details

    ''        End If

    ''        If IsNothing(dgv1) = False Then

    ''            With dgv1

    ''                If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

    ''                    Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, msk_date, txt_note, dgvDet_CboBx_ColNos_Arr, dgtxt_Details, msk_date)

    ''                    Return True

    ''                Else
    ''                    Return MyBase.ProcessCmdKey(msg, keyData)

    ''                End If


    ''            End With

    ''        Else

    ''            Return MyBase.ProcessCmdKey(msg, keyData)

    ''        End If

    ''    Else

    ''        Return MyBase.ProcessCmdKey(msg, keyData)

    ''    End If

    ''End Function
    Private Sub Close_Form()

        Try

            lbl_fabricname.Tag = 0
            lbl_fabricname.Text = ""

            Pnl_Fabric_selection.Visible = True
            Pnl_Back.Enabled = False


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(vEntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from bundle_packing_Head a  Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.bundle_packing_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("bundle_packing_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("bundle_packing_Date").ToString
                txt_note.Text = Trim(dt1.Rows(0).Item("note").ToString)
                Cbo_Employee_Name.Text = Common_Procedures.Employee_Simple_IdNoToName(con, dt1.Rows(0).Item("Employee_IdNo").ToString)
                cbo_Partyname.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("Party_IdNo").ToString)
                msk_date.Text = dtp_Date.Text
                msk_date.SelectionStart = 0

                lbl_InvoiceNo.Text = Trim(dt1.Rows(0).Item("ClothSales_Invoice_Code").ToString)
                If Trim(lbl_InvoiceNo.Text) = "" Then
                    lbl_InvoiceNo.Text = Trim(dt1.Rows(0).Item("ClothSales_Return_Code").ToString)
                End If


                txt_bundleno.Text = dt1.Rows(0).Item("bundle_No").ToString
                txt_mark.Text = dt1.Rows(0).Item("Mark").ToString
                txt_Pcs.Text = dt1.Rows(0).Item("Total_Pcs").ToString
                lbl_yards.Text = Format(Val(dt1.Rows(0).Item("Total_Yards").ToString), "########0.00")
                ' lbl_fabricname.Text = Common_Procedures.Cloth_IdNoToName(con, dt1.Rows(0).Item("First_ClothIdNo").ToString)
                cbo_FabName.Text = Common_Procedures.Cloth_IdNoToName(con, dt1.Rows(0).Item("First_ClothIdNo").ToString)
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                cbo_FabName.Tag = cbo_FabName.Text

                chk_Printed.Checked = False
                chk_Printed.Enabled = False
                chk_Printed.Visible = False
                If Val(dt1.Rows(0).Item("PrintOut_Status").ToString) = 1 Then
                    chk_Printed.Checked = True
                    chk_Printed.Visible = True
                    If Val(Common_Procedures.User.IdNo) = 1 Then
                        chk_Printed.Enabled = True
                    End If
                End If

                cbo_WareHouse.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))

                'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from bundle_packing_Details a INNER JOIN Cloth_Head b on a.Cloth_IdNo = b.Cloth_IdNo where a.bundle_packing_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                'dgv_Details.Rows.Clear()
                'SNo = 0

                'If dt2.Rows.Count > 0 Then

                '    For i = 0 To dt2.Rows.Count - 1

                '        n = dgv_Details.Rows.Add()

                '        SNo = SNo + 1
                '        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                '        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                '        dgv_Details.Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Mark").ToString)
                '        dgv_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Pcs").ToString), "########0.00")

                '        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Yards").ToString), "########0.00")

                '    Next i

                'End If

                'With dgv_Details_Total
                '    If .RowCount = 0 Then .Rows.Add()

                '    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Pcs").ToString), "########0.000")
                '    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Yards").ToString), "########0.000")
                'End With

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()

            Else
                new_record()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()


            If Trim(lbl_InvoiceNo.Text) <> "" Then

                lbl_RefNo.BackColor = Color.LightGray

                Cbo_Employee_Name.Enabled = False
                Cbo_Employee_Name.BackColor = Color.LightGray

                cbo_Partyname.Enabled = False
                cbo_Partyname.BackColor = Color.LightGray

                msk_date.Enabled = False
                msk_date.BackColor = Color.LightGray

                dtp_Date.Enabled = False
                dtp_Date.BackColor = Color.LightGray

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Bundle_entry, "~L~") = 0 And InStr(Common_Procedures.UR.Bundle_entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(vEntFnYrCode)
        'If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Bundle_Packing_Entry, New_Entry, Me, con, "bundle_packing_Head", "bundle_packing_Code", NewCode, "bundle_packing_Date", "(bundle_packing_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

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


        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(vEntFnYrCode)
        'NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from bundle_packing_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and bundle_packing_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            If IsDBNull(Dt1.Rows(0).Item("ClothSales_Invoice_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("ClothSales_Invoice_Code").ToString) <> "" Then
                    MessageBox.Show("Already Invoiced", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

        End If
        Dt1.Clear()
        Dt1.Dispose()
        Da.Dispose()

        trans = con.BeginTransaction

        Try



            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete From Stock_Piece_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  Reference_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete From Stock_Bundle_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  Reference_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from bundle_packing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and bundle_packing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from bundle_packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and bundle_packing_Code = '" & Trim(NewCode) & "'"
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

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'SIZING') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_ClothName.DataSource = dt1
            cbo_Filter_ClothName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select count_name from count_head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_EmployeeName.DataSource = dt2
            cbo_Filter_EmployeeName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select Mill_name from Mill_head order by Mill_name", con)
            da.Fill(dt2)
            ' cbo_Filter_MillName.Text .DataSource = dt2
            ' cbo_Filter_MillName.Text .DisplayMember = "Mill_name"


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_EmployeeName.Text = ""
            ' ' ' cbo_Filter_MillName.Text .Text   = ""

            cbo_Filter_ClothName.SelectedIndex = -1
            cbo_Filter_EmployeeName.SelectedIndex = -1
            ' cbo_Filter_MillName.Text .SelectedIndex = -1
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
        Dim vClo_IdNo As Integer

        Try

            '   vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, lbl_fabricname.Text)
            vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_FabName.Text)

            'and (First_ClothIdNo = " & Str(Val(vClo_IdNo)) & ")
            da = New SqlClient.SqlDataAdapter("select top 1 bundle_packing_No from bundle_packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and bundle_packing_Code like '" & Trim(Pk_Condition) & "%/" & Trim(vEntFnYrCode) & "'  Order by for_Orderby, bundle_packing_No", con)
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
        Dim OrdByNo As String = 0
        Dim vClo_IdNo As Integer
        Try

            'vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, lbl_fabricname.Text)

            vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_FabName.Text)

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            'and (First_ClothIdNo = " & Str(Val(vClo_IdNo)) & ")
            da = New SqlClient.SqlDataAdapter("select top 1 bundle_packing_No from bundle_packing_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and bundle_packing_Code like '%/" & Trim(vEntFnYrCode) & "'    Order by for_Orderby, bundle_packing_No", con)
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
        Dim OrdByNo As String = 0
        Dim vClo_IdNo As Integer
        Try

            'vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, lbl_fabricname.Text)

            vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_FabName.Text)

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            'and (First_ClothIdNo = " & Str(Val(vClo_IdNo)) & ")
            da = New SqlClient.SqlDataAdapter("select top 1 bundle_packing_No from bundle_packing_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & "  and bundle_packing_Code like '%/" & Trim(vEntFnYrCode) & "'   Order by for_Orderby desc, bundle_packing_No desc", con)
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
        Dim vClo_IdNo As Integer
        Try
            '  vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, lbl_fabricname.Text)
            vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_FabName.Text)

            'and (First_ClothIdNo = " & Str(Val(vClo_IdNo)) & ")
            da = New SqlClient.SqlDataAdapter("select top 1 bundle_packing_No from bundle_packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & "  and bundle_packing_Code like '%/" & Trim(vEntFnYrCode) & "'   Order by for_Orderby desc, bundle_packing_No desc", con)
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

        Dim vClo_IdNo As Integer



        Try
            clear()
            ' vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, lbl_fabricname.Text)
            vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_FabName.Text)
            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "bundle_packing_Head", "bundle_packing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Trim(vEntFnYrCode))
            lbl_RefNo.ForeColor = Color.Red

            If Trim(Common_Procedures.settings.CustomerCode) = "1414" Then

                txt_bundleno.Text = Common_Procedures.get_MaxCode(con, "bundle_packing_Head", "Bundle_Code", "Bundle_no", "", Val(lbl_Company.Tag), Trim(vEntFnYrCode))
                txt_bundleno.ForeColor = Color.Red
            Else

                txt_bundleno.Text = Common_Procedures.get_MaxCode(con, "bundle_packing_Head", "Bundle_Code", "Bundle_no", "(First_ClothIdNo = " & Str(Val(vClo_IdNo)) & ")", Val(lbl_Company.Tag), Trim(vEntFnYrCode))
                txt_bundleno.ForeColor = Color.Red
            End If


            msk_date.Text = Date.Today.ToShortDateString
            msk_date.SelectionStart = 0
            da = New SqlClient.SqlDataAdapter("select top 1 * from bundle_packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and bundle_packing_Code like '%/" & Trim(vEntFnYrCode) & "' Order by for_Orderby desc, bundle_packing_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("bundle_packing_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("bundle_packing_Date").ToString
                End If

                If Trim(Common_Procedures.settings.CustomerCode) <> "1414" Then

                    If Val(dt1.Rows(0).Item("First_ClothIdNo").ToString) <> 0 Then
                        cbo_FabName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("First_ClothIdNo").ToString))
                        cbo_FabName.Tag = cbo_FabName.Text
                        txt_bundleno.Text = Common_Procedures.get_MaxCode(con, "bundle_packing_Head", "Bundle_Code", "Bundle_no", "(First_ClothIdNo = " & Str(Val(Val(dt1.Rows(0).Item("First_ClothIdNo").ToString))) & ")", Val(lbl_Company.Tag), Trim(vEntFnYrCode))
                    End If
                End If

            End If
                dt1.Clear()

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

            RecCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(vEntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select bundle_packing_No from bundle_packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and bundle_packing_Code = '" & Trim(RecCode) & "'", con)
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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sizing_Yarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sizing_Yarn_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        '  If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Bundle_Packing_Entry, New_Entry, Me) = False Then Exit Sub
        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(vEntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select bundle_packing_No from bundle_packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and bundle_packing_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW ENTRY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vClo_IdNo As Integer = 0
        Dim EntID As String = ""
        Dim clthStock_In As String = ""
        Dim clthStk_Pcs_Mtr As String = ""
        Dim vEmpIdNo As String = ""
        Dim vPartyIdNo As String = ""
        Dim vItmCount As Integer = 0
        Dim Selc_PackingCode As String = ""
        Dim vBundle_Code As String = ""
        Dim Cloth_Cond = ""
        Dim vSELC_BUN_NO As String = ""
        Dim vWareHouseIdNo As String = ""

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(vEntFnYrCode)
        '' If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Bundle_Packing_Entry, New_Entry, Me, con, "bundle_packing_Head", "bundle_packing_Code", NewCode, "bundle_packing_Date", "(bundle_packing_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and bundle_packing_Code LIKE '%/" & Trim(vEntFnYrCode) & "')", "for_Orderby desc, bundle_packing_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        vEmpIdNo = Common_Procedures.Employee_Simple_NameToIdNo(con, Cbo_Employee_Name.Text)
        'If Val(vEmpIdNo) = 0 Then
        '    MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If Cbo_Employee_Name.Enabled And Cbo_Employee_Name.Visible Then Cbo_Employee_Name.Focus()
        '    Exit Sub
        'End If


        vPartyIdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_Partyname.Text)

        If Trim(lbl_InvoiceNo.Text) <> "" Then
            MessageBox.Show("Already Invoiced", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, lbl_fabricname.Text)
        vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_FabName.Text)
        If Val(vClo_IdNo) = 0 Then
            MessageBox.Show("Invalid Fabric Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_FabName.Enabled And cbo_FabName.Visible Then cbo_FabName.Focus()
            Exit Sub
        End If

        '''''' ----  COMMAND DATE 2024-10-18
        ''''
        ''Show_Pcs_CurrentStock()
        '''If (Val(lbl_AvailableStock.Text) - Val(txt_Pcs.Text)) > 0 Then
        ''If (Val(lbl_AvailableStock.Text) - Val(txt_Pcs.Text)) < 0 Then
        ''    MessageBox.Show("Negative stock", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        ''    If txt_Pcs.Enabled And txt_Pcs.Visible Then txt_Pcs.Focus()
        ''    Exit Sub
        ''End If

        If Trim(UCase(vEntryType)) <> "OPENING" Then
            If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
                Exit Sub
            End If
        End If


        vWareHouseIdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_WareHouse.Text)

        tr = con.BeginTransaction

        EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
        PBlNo = Trim(lbl_RefNo.Text)

        If Trim(UCase(vEntryType)) = "OPENING" Then
            Partcls = "Opening BundlePack : Ref.No. " & Trim(lbl_RefNo.Text)
        Else
            Partcls = "BundlePack : Ref.No. " & Trim(lbl_RefNo.Text)
        End If

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(vEntFnYrCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "bundle_packing_Head", "bundle_packing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Trim(vEntFnYrCode), tr)

                If Trim(Common_Procedures.settings.CustomerCode) = "1414" Then
                    txt_bundleno.Text = Common_Procedures.get_MaxCode(con, "bundle_packing_Head", "Bundle_Code", "Bundle_no", "", Val(lbl_Company.Tag), Trim(vEntFnYrCode), tr)
                Else
                    txt_bundleno.Text = Common_Procedures.get_MaxCode(con, "bundle_packing_Head", "Bundle_Code", "Bundle_no", "(First_ClothIdNo = " & Str(Val(vClo_IdNo)) & ")", Val(lbl_Company.Tag), Trim(vEntFnYrCode), tr)
                End If

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(vEntFnYrCode)

                End If

                Selc_PackingCode = Trim(lbl_RefNo.Text) & "/" & Trim(vEntFnYrCode) & "/" & Trim(Val(lbl_Company.Tag))

            vBundle_Code = Trim(Val(lbl_Company.Tag) & "/" & Trim(txt_bundleno.Text) & "/" & Trim(vEntFnYrCode))

            vSELC_BUN_NO = Trim(Trim(txt_bundleno.Text) & "/" & Trim(vEntFnYrCode)) & "/" & Trim(Val(lbl_Company.Tag))

            Cloth_Cond = "  and First_ClothIdNo = " & Str(Val(vClo_IdNo))
            If Trim(Common_Procedures.settings.CustomerCode) = "1414" Then
                Cloth_Cond = ""
            End If


            Da = New SqlClient.SqlDataAdapter("Select Bundle_Code,Bundle_Packing_No from bundle_packing_Head where company_idno = " & Val(lbl_Company.Tag) & " and Bundle_Code = '" & Trim(vBundle_Code) & "' " & Cloth_Cond & "  and Bundle_Packing_Code <> '" & Trim(NewCode) & "' ", con)
            If IsNothing(tr) = False Then
                Da.SelectCommand.Transaction = tr
            End If
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate BundleNo in this Cloth " & Chr(13) & "Already this BundleNo Enterd Ref No : " & Dt1.Rows(0)(1).ToString, "DOES Not SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_bundleno.Enabled And txt_bundleno.Visible Then txt_bundleno.Focus()
                Exit Sub
            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then

                cmd.CommandText = "Insert into bundle_packing_Head(bundle_packing_Code,         Company_IdNo,          bundle_packing_No,                              for_OrderBy,                                      bundle_packing_Date,             Party_idNo,             Employee_IdNo,                          Mark,                  Total_Pcs,            Total_Yards ,                  Note,                        Bundle_Packing_Selection_Code,           First_ClothIdNo           ,          bundle_No              ,           Bundle_code        ,           User_idno                   ,        Bundle_No_For_Selection , WareHouse_Idno  ) " &
                                                 "Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ",  " & Val(lbl_RefNo.Text) & " ,  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate,        " & Str(Val(vPartyIdNo)) & ", " & Str(Val(vEmpIdNo)) & ",    " & Val(txt_mark.Text) & ", " & Val(txt_Pcs.Text) & " ,  " & Val(lbl_yards.Text) & " , '" & Trim(txt_note.Text) & "','" & Trim(Selc_PackingCode) & "', " & Str(Val(vClo_IdNo)) & "  , " & Str(Val(txt_bundleno.Text)) & ", '" & Trim(vBundle_Code) & "'," & Val(Common_Procedures.User.IdNo) & " , '" & Trim(vSELC_BUN_NO) & "'  , " & Str(Val(vWareHouseIdNo)) & " )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update bundle_packing_Head set bundle_packing_Date = @EntryDate,  Party_idNo = " & Str(Val(vPartyIdNo)) & ",bundle_No= " & Val(txt_bundleno.Text) & ",Mark=" & Val(txt_mark.Text) & ", Total_Pcs = " & Str(Val(txt_Pcs.Text)) & ", Total_Yards = " & Str(Val(lbl_yards.Text)) & " ,Employee_IdNo=" & Val(vEmpIdNo) & " , Note ='" & Trim(txt_note.Text) & "', Bundle_Packing_Selection_Code = '" & Trim(Selc_PackingCode) & "' , First_ClothIdNo = " & Str(Val(vClo_IdNo)) & ",Bundle_code='" & Trim(vBundle_Code) & "' ,User_idNo = " & Val(Common_Procedures.User.IdNo) & ",Bundle_No_For_Selection = '" & Trim(vSELC_BUN_NO) & "' , WareHouse_Idno = " & Str(Val(vWareHouseIdNo)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and bundle_packing_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If


            cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Reference_Code, Reference_Date, Company_Idno, Cloth_IdNo ) " &
                      " Select                               Reference_Code, Reference_Date, Company_IdNo, Cloth_IdNo from Stock_Bundle_Processing_Details where Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            PBlNo = Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from bundle_packing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and bundle_packing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete From Stock_Bundle_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  Reference_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete From Stock_Piece_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  Reference_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()




            cmd.CommandText = "Insert into Stock_Bundle_Processing_Details (       Reference_Code    ,        Company_IdNo              ,               Reference_no     ,                                             for_OrderBy                 , Reference_Date, DeliveryTo_Idno,   ReceivedFrom_IdNo ,       Entry_ID          ,       Party_Bill_No    ,      Particulars        , Sl_No ,                 Cloth_IdNo   ,  Bundle ,              Mark              ,              Pcs               ,                Yards            )" &
                                "          Values                          (  '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,    @EntryDate ,       4        ,       0             ,    '" & Trim(EntID) & "',   '" & Trim(PBlNo) & "',  '" & Trim(Partcls) & "',   1   ,   " & Str(Val(vClo_IdNo)) & ",     1   ,  " & Str(Val(txt_mark.Text)) & ", " & Str(Val(txt_Pcs.Text)) & ", " & Str(Val(lbl_yards.Text)) & ")"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Insert into Stock_Piece_Processing_Details (          Reference_Code ,                  Company_IdNo    ,                     Reference_no,                                             for_OrderBy  ,                    Reference_Date ,       DeliveryTo_Idno,     ReceivedFrom_IdNo    ,       Entry_ID     ,                 Party_Bill_No,              Particulars  ,            Sl_No         ,                 Cloth_IdNo        ,                   Mark  ,                            Pcs          ,               Yards        )" &
                                    " Values                                    (  '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " , @EntryDate    ,             0        ,        4                ,    '" & Trim(EntID) & "',   '" & Trim(PBlNo) & "',     '" & Trim(Partcls) & "',     " & Str(Val(Sno)) & ",   " & Str(Val(vClo_IdNo)) & ",         " & Str(Val(txt_mark.Text)) & ",                " & Str(Val(txt_Pcs.Text)) & ",  " & Str(Val(lbl_yards.Text)) & ")"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Stock_Piece_Processing_Details (    Reference_Code    ,             Company_IdNo         ,            Reference_no           ,                        for_OrderBy       ,                                        Reference_Date,     Entry_ID,          Party_Bill_No       , Particulars,                    SL_No     ,          Cloth_IdNo      ,            Mark      ,                           Pcs,                                       Yards                      ) " &
            '                                        " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @EntryDate ,  '" & Trim(EntID) & "', '" & Trim(PBlNo) & "','" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(vClo_IdNo)) & "," & Str(Val(txt_mark.Text)) & ", " & Str(-1 * Val(txt_Pcs.Text)) & " , " & Str(Val(lbl_yards.Text)) & " )"
            'cmd.ExecuteNonQuery()

            If Val(Common_Procedures.User.IdNo) = 1 Then
                If chk_Printed.Visible = True Then
                    If chk_Printed.Enabled = True Then
                        Update_PrintOut_Status(tr)
                    End If
                End If
            End If

            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Reference_Code, Reference_Date, Company_Idno, Cloth_IdNo ) " &
                                      " Select                               Reference_Code, Reference_Date, Company_IdNo, Cloth_IdNo from Stock_Bundle_Processing_Details where Reference_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                'f Common_Procedures.Check_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If
            Else
                ' move_record(lbl_RefNo.Text)
                new_record()
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub


    Private Sub Total_Calculation()



        Dim Pcs As Single = 0
        Dim Mark As Single = 0
        Dim Yrds As Single

        Pcs = Val(txt_Pcs.Text)
        Mark = Val(txt_mark.Text)
        Yrds = Format(Val(lbl_yards.Text), "#########0.00")


        Yrds = (Pcs * Mark)
        lbl_yards.Text = Format(Val(Yrds), "#########0.00")


    End Sub


    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
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
        Dim EMP_IdNo As Integer, party_IdNo As Integer, cloth_idno As Integer
        Dim Condt As String = ""
        Dim Bundle_no As String

        Try

            Condt = ""

            party_IdNo = 0
            EMP_IdNo = 0
            cloth_idno = 0
            Bundle_no = 0


            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.bundle_packing_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.bundle_packing_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.bundle_packing_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            'If Trim(cbo_Filter_PartyName.Text) <> "" Then
            '    EMP_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            'End If
            If Trim(cbo_Filter_EmployeeName.Text) <> "" Then
                party_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_EmployeeName.Text)
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                cloth_idno = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If


            'If Val(EMP_IdNo) <> 0 Then
            '    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Employee_IdNo = " & Str(Val(EMP_IdNo)) & ")"
            'End If
            If Val(party_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.bundle_packing_Code IN ( select z1.bundle_packing_Code from bundle_packing_Details z1 where z1.party_IdNo = " & Str(Val(party_IdNo)) & " )"
            End If

            If Val(cloth_idno) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.First_clothidno = " & Str(Val(cloth_idno)) & ")"
            End If


            Bundle_no = Trim(txt_filter_bundle_no.Text)
            If Val(txt_filter_bundle_no.Text) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Bundle_No = " & Str(Val(Bundle_no)) & ")"
                '   Condt = "a.Bundle_no = " & Str(Val(bundle_no)) & ""
                '     Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (select Bundle_no from bundle_packing_Head)"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*,c.cloth_name as Fabric_name from bundle_packing_Head a inner join CLoth_head c on a.First_clothidno=c.cloth_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.bundle_packing_Code like '%/" & Trim(vEntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.bundle_packing_Date, a.for_orderby, a.bundle_packing_No", con)

            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    '  dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("bundle_packing_No").ToString
                    '  dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("bundle_packing_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("bundle_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("bundle_packing_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Fabric_name").ToString

                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("mark").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Yards").ToString), "########0.000")

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
            txt_filter_bundle_no.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_EmployeeName, btn_Filter_Show, "Cloth_head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, btn_Filter_Show, "Cloth_head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_EmployeeName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EmployeeName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub


    Private Sub cbo_Filter_EmployeeName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EmployeeName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EmployeeName, txt_filter_bundle_no, cbo_Filter_ClothName, "Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_EmployeeName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EmployeeName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EmployeeName, cbo_Filter_ClothName, "Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

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



    Public Sub print_record() Implements Interface_MDIActions.print_record
        Exit Sub
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 38 Then
            txt_note.Focus()

        End If
        If e.KeyCode = 40 Then
            'dgv_Details.Focus()
            'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            '   Cbo_Employee_Name.Focus()
            'txt_mark.Focus()
            cbo_FabName.Focus()

            e.Handled = True
            e.SuppressKeyPress = True
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
            'Cbo_Employee_Name.Focus()
            cbo_FabName.Focus()

            e.Handled = True
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

    Private Sub msk_date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.TextChanged
        msk_Date_LostFocus(sender, e)
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

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub


    Private Sub Cbo_Employee_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Employee_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_Head", "Employee_Name", "", "(Employee_IdNo=0)")

    End Sub

    Private Sub Cbo_Employee_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Employee_Name.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Employee_Name, txt_Pcs, cbo_Partyname, "Employee_Head", "Employee_Name", "", "(Employee_IdNo=0)")

    End Sub

    Private Sub Cbo_Employee_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Employee_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Employee_Name, cbo_Partyname, "Employee_Head", "Employee_Name", "", "(Employee_IdNo=0)")

    End Sub

    Private Sub Cbo_Employee_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Employee_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EmployeeCreation_Simple
            f.Show()

        End If
    End Sub




    Private Sub cbo_partyname_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Partyname.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Ledger_Name", "", "(Ledger_IdNo=0)")
    End Sub

    Private Sub cbo_partyname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Partyname.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Partyname, Cbo_Employee_Name, txt_note, "Ledger_Head", "Ledger_Name", "", "(Ledger_IdNo=0)")

        'If (e.KeyValue = 40 And cbo_Partyname.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        '    If dgv_Details.Rows.Count > 0 Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

        '    End If

        'End If

    End Sub

    Private Sub cbo_partyname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Partyname.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Partyname, txt_note, "Ledger_Head", "Ledger_Name", "", "(Ledger_IdNo=0)")
        If Asc(e.KeyChar) = 13 Then

            ' cbo_Grid_FabricName.Focus()
            txt_note.Focus()

            ''If dgv_Details.Rows.Count > 0 Then
            ''    dgv_Details.Focus()
            ''    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            ''End If

        End If

    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Partyname.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Ledger_Creation
            f.Show()

        End If
    End Sub

    Private Sub txt_note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_note.KeyDown

        If (e.KeyValue = 38) Or (e.Control = True And e.KeyValue = 38) Then

            ' dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(3)
            cbo_Partyname.Focus()
        End If

        If (e.KeyValue = 40) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_WareHouse.Visible And cbo_WareHouse.Enabled = True Then
                cbo_WareHouse.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()

                Else
                    msk_date.Focus()
                End If
            End If

        End If



    End Sub


    Private Sub txt_note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_note.KeyPress
        If Asc(e.KeyChar) = 13 Then


            If cbo_WareHouse.Visible And cbo_WareHouse.Enabled = True Then
                cbo_WareHouse.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()

                Else
                    msk_date.Focus()
                End If
            End If


        End If
    End Sub

    Private Sub PrintPreview_Toolstrip_ItemClicked(ByVal sender As Object, ByVal e As ToolStripItemClickedEventArgs)
        'If it is the print button that was clicked: run the printdialog
        If LCase(e.ClickedItem.Name) = LCase("printToolStripButton") Then

            Try
                chk_Printed.Checked = True
                chk_Printed.Visible = True
                Update_PrintOut_Status()

            Catch ex As Exception
                MsgBox("Print Error: " & ex.Message)

            End Try
        End If
    End Sub

    Private Sub PrintPreview_Shown(ByVal sender As Object, ByVal e As System.EventArgs)
        'Capture the click events for the toolstrip in the dialog box when the dialog is shown
        Dim ts As ToolStrip = CType(sender.Controls(1), ToolStrip)
        AddHandler ts.ItemClicked, AddressOf PrintPreview_Toolstrip_ItemClicked
    End Sub

    Private Sub Update_PrintOut_Status(Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing)
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim vPrnSTS As Integer = 0


        Try

            cmd.Connection = con
            If IsNothing(sqltr) = False Then
                cmd.Transaction = sqltr
            End If

            vPrnSTS = 0
            If chk_Printed.Checked = True Then
                vPrnSTS = 1
            End If


            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(vEntFnYrCode)
            cmd.CommandText = "Update bundle_packing_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and bundle_packing_Code  = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If chk_Printed.Checked = True Then
                chk_Printed.Visible = True
                If Val(Common_Procedures.User.IdNo) = 1 Then
                    chk_Printed.Enabled = True
                End If
            End If

            cmd.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try

    End Sub


    Private Sub btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OK.Click
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim CloID As Integer
        Dim cloth_idNo As Integer = 0

        da = New SqlClient.SqlDataAdapter("select Cloth_IdNo from Cloth_Head where Cloth_name = '" & Trim(cbo_Fabriname.Text) & "'", con)
        dt1 = New DataTable
        da.Fill(dt1)

        CloID = 0
        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                CloID = Val(dt1.Rows(0)(0).ToString)
            End If
        End If

        If CloID = 0 Then
            MessageBox.Show("Invalid Cloth Selection", "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Fabriname.Enabled Then cbo_Fabriname.Focus()
            Exit Sub
        End If

        vMain_ClothName = cbo_Fabriname.Text

        new_record()

        Pnl_Fabric_selection.Visible = False
        Pnl_Back.Enabled = True
        msk_date.Focus()

    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        '        Common_Procedures.CompIdNo = 0
        MessageBox.Show("Invalid Cloth Selection", "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Me.Close()
    End Sub


    Private Sub cbo_Fabriname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Fabriname.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim indx As Integer

        Try

            With cbo_Fabriname

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then

                            If .DroppedDown = True Then

                                FindStr = LTrim(.Text)

                                indx = .FindString(FindStr)

                                If indx <> -1 Then
                                    .SelectedText = ""
                                    .SelectedIndex = indx
                                    .SelectionStart = FindStr.Length
                                    .SelectionLength = .Text.Length
                                End If

                            End If

                        End If

                        btn_OK_Click(sender, e)

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        indx = .FindString(FindStr)

                        If indx <> -1 Then
                            .SelectedText = ""
                            .SelectedIndex = indx
                            .SelectionStart = FindStr.Length
                            .SelectionLength = .Text.Length
                            e.Handled = True

                        Else
                            e.Handled = True

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Fabriname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Fabriname.KeyUp
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim indx As Integer

        Try

            With cbo_Fabriname

                If e.KeyCode <> 27 Then

                    If e.KeyCode = 46 Then

                        Condt = ""
                        FindStr = LTrim(.Text)

                        indx = .FindString(FindStr)

                        If indx <> -1 Then
                            .SelectedText = ""
                            .SelectedIndex = indx
                            .SelectionStart = FindStr.Length
                            .SelectionLength = .Text.Length
                            e.Handled = True

                        Else
                            e.Handled = True

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub lbl_yards_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_yards.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_mark_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_mark.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_Pcs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Pcs.TextChanged
        Total_Calculation()
    End Sub

    Private Sub cbo_FabName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FabName.GotFocus
        cbo_FabName.Tag = cbo_FabName.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "cloth_head", "cloth_Name", "", "(cloth_IdNo=0)")
        Show_Pcs_CurrentStock()
    End Sub

    Private Sub cbo_FabName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FabName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FabName, msk_date, txt_mark, "cloth_head", "cloth_Name", "", "(cloth_IdNo=0)")
    End Sub

    Private Sub cbo_FabName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FabName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FabName, txt_mark, "cloth_head", "cloth_Name", "", "(cloth_IdNo=0)")
        Dim vClo_IdNo As Integer
        If Asc(e.KeyChar) = 13 Then
            If Trim(Common_Procedures.settings.CustomerCode) <> "1414" Then

                If Trim(UCase(cbo_FabName.Tag)) <> Trim(UCase(cbo_FabName.Text)) Then
                    vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_FabName.Text)
                    txt_bundleno.Text = Common_Procedures.get_MaxCode(con, "bundle_packing_Head", "Bundle_Code", "Bundle_no", "(First_ClothIdNo = " & Str(Val(vClo_IdNo)) & ")", Val(lbl_Company.Tag), Trim(vEntFnYrCode))
                End If
            End If
            Show_Pcs_CurrentStock()
            End If
    End Sub

    Private Sub cbo_FabName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_FabName.SelectedIndexChanged

    End Sub


    Private Sub Label14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label14.Click
        pnl_Filter.Visible = False
        Pnl_Back.Visible = True
        Pnl_Back.Enabled = True
    End Sub


    Private Sub Show_Pcs_CurrentStock()
        Dim vFabID As Integer
        Dim CurStk As Decimal

        If Trim(cbo_FabName.Text) <> "" Then

            vFabID = Common_Procedures.Cloth_NameToIdNo(con, cbo_FabName.Text)

            If Val(lbl_AvailableStock.Tag) <> Val(vFabID) Or Val(lbl_AvailableStock_Caption.Tag) <> Val(txt_mark.Text) Then

                lbl_AvailableStock_Caption.Tag = 0
                lbl_AvailableStock.Tag = 0
                lbl_AvailableStock.Text = ""

                If Val(vFabID) <> 0 And Val(txt_mark.Text) <> 0 Then



                    CurStk = Common_Procedures.get_Pcs_CurrentStock(con, Val(lbl_Company.Tag), vFabID, Val(txt_mark.Text))

                    lbl_AvailableStock_Caption.Tag = Val(txt_mark.Text)
                    lbl_AvailableStock.Tag = vFabID
                    lbl_AvailableStock.Text = Format(Val(CurStk), "#########0.000")

                End If

            End If

        Else

            lbl_AvailableStock_Caption.Tag = 0
            lbl_AvailableStock.Tag = 0
            lbl_AvailableStock.Text = ""

        End If

    End Sub

    Private Sub txt_mark_GotFocus(sender As Object, e As EventArgs) Handles txt_mark.GotFocus
        Show_Pcs_CurrentStock()
    End Sub

    Private Sub cbo_FabName_LostFocus(sender As Object, e As EventArgs) Handles cbo_FabName.LostFocus
        Show_Pcs_CurrentStock()
    End Sub



    Private Sub txt_Pcs_GotFocus(sender As Object, e As EventArgs) Handles txt_Pcs.GotFocus
        Show_Pcs_CurrentStock()
    End Sub

    Private Sub txt_mark_LostFocus(sender As Object, e As EventArgs) Handles txt_mark.LostFocus
        Show_Pcs_CurrentStock()
    End Sub

    Private Sub txt_bundleno_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_bundleno.KeyDown
        If e.KeyCode = 38 Then
            cbo_FabName.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_mark.Focus()
        End If
    End Sub

    Private Sub txt_bundleno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_bundleno.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_mark.Focus()
        End If
    End Sub

    Private Sub txt_mark_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_mark.KeyDown
        If e.KeyCode = 38 Then
            cbo_FabName.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_Pcs.Focus()
        End If

    End Sub
    Private Sub txt_mark_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_mark.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Pcs.Focus()
        End If
    End Sub

    Private Sub cbo_FabName_Leave(sender As Object, e As EventArgs) Handles cbo_FabName.Leave
        Dim vClo_IdNo As Integer
        If Trim(Common_Procedures.settings.CustomerCode) <> "1414" Then

            If Trim(UCase(cbo_FabName.Tag)) <> Trim(UCase(cbo_FabName.Text)) Then
                vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_FabName.Text)
                txt_bundleno.Text = Common_Procedures.get_MaxCode(con, "bundle_packing_Head", "Bundle_Code", "Bundle_no", "(First_ClothIdNo = " & Str(Val(vClo_IdNo)) & ")", Val(lbl_Company.Tag), Trim(vEntFnYrCode))
            End If
        End If
    End Sub

    Private Sub lbl_InvoiceNo_Click(sender As Object, e As EventArgs) Handles lbl_InvoiceNo.Click

    End Sub

    Private Sub cbo_WareHouse_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_WareHouse.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_WareHouse.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_WareHouse_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_WareHouse.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WareHouse, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_IdNo = 0 or Ledger_Type = 'GODOWN')  and Close_status = 0 )", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            ' txt_Amount.Focus()
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If

    End Sub

    Private Sub cbo_WareHouse_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_WareHouse.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WareHouse, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_IdNo = 0 or Ledger_Type = 'GODOWN')  and Close_status = 0 )", "(Ledger_idno = 0)")


        If (e.KeyValue = 40 And cbo_WareHouse.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If

        End If

        If (e.KeyValue = 38 And cbo_WareHouse.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            txt_note.Focus()

        End If

    End Sub

    Private Sub cbo_WareHouse_GotFocus(sender As Object, e As EventArgs) Handles cbo_WareHouse.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_IdNo = 0 or Ledger_Type = 'GODOWN')  and Close_status = 0 )", "(Ledger_idno = 0)")

    End Sub

    Private Sub txt_note_TextChanged(sender As Object, e As EventArgs) Handles txt_note.TextChanged

    End Sub

    Private Sub cbo_WareHouse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_WareHouse.SelectedIndexChanged

    End Sub
End Class
