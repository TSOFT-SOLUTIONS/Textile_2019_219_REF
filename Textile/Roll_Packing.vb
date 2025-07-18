Imports System.Drawing.Printing
Imports System.IO

Public Class Roll_Packing
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "RLPCK-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_HdDt2 As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_HdAr(100, 10) As String
    Private prn_HdAr2(100, 10) As String
    Private prn_DetAr(100, 50, 10) As String
    Private prn_DetAr1(1000, 10) As String
    Private prn_DetMxIndx1 As Integer

    Private prn_DetMxIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_HdIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Count As Integer = 0
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Dim prn_PL_Tot_Rls As Integer = 0
    Dim prn_PL_Tot_Pcs As Integer = 0

    Dim prn_PL_Tot_Mtr As String = 0
    Dim prn_PL_Tot_GrsWgt As String = 0
    Dim prn_PL_Tot_NetWgt As String = 0
    Private prn_TotalPcs As String = ""
    Private Total_mtrs As Single = 0
    Private Total_Wgt As Single = 0


    Public vmskRollPckngPoText As String = ""
    Public vmskRollPckngPoStrt As Integer = -1

    Private prn_TotaGrslWgt As String = ""
    Private prn_PACKINGLISTNO As String = ""
    Private vPRN_FOLDINGPERC As Single

    Private prn_TotalMtrs As String = ""
    Private prn_TotalWgt As String = ""
    Private prn_TotalGrams As String = 0

    Dim vPrn_Roll_Packing_No As String = ""
    Dim vPrn_Roll_Packing_Date As String = ""
    Dim vPrn_Roll_Packing_Po_No As String = ""
    Dim vPrn_Roll_Packing_Po_Date As String = ""
    Dim vSort_No As String = ""
    Dim vCLONAME As String = ""

    Private Prn_BarcodeSticker As Boolean = False
    Private fs As FileStream
    Private sw As StreamWriter
    Private prn_HeadIndx As Integer




    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_Print.Visible = False
        Prn_BarcodeSticker = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        vmskOldText = ""
        vmskSelStrt = -1
        msk_date.Text = ""
        dtp_Date.Text = ""
        ' cbo_Cloth.Text = ""
        cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, 1)
        ' cbo_Cloth.Text = ""
        cbo_Bale_Bundle.Text = "ROLL"
        cbo_Type.Text = "PCS"
        txt_LotSelction.Text = ""
        txt_PcsSelction.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Roll_Packing_PoNo.Text = ""
        msk_Roll_Packing_Po_Date.Text = ""


        txt_Folding.Text = 100
        txt_Note.Text = ""
        chk_SelectAll.Checked = False

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Party.Text = ""
            cbo_Filter_Cloth.Text = ""
            txt_Filter_BuyerRefNo.Text = ""
            txt_Filter_LotNo.Text = ""
            txt_Filter_PcsNo.Text = ""
            txt_Filter_RollNo.Text = ""
            cbo_Filter_Party.SelectedIndex = -1
            cbo_Filter_Cloth.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_Cloth.Enabled = True
        cbo_Cloth.BackColor = Color.White

        cbo_ClothType.Enabled = True
        cbo_ClothType.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White

        btn_Selection.Enabled = True

        Grid_Cell_DeSelect()
        NoCalc_Status = False


        vmskRollPckngPoText = ""
        vmskRollPckngPoStrt = -1

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

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
            Msktxbx.SelectAll()
        End If

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

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

    Private Sub Roll_Packing_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Cloth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub Roll_Packing_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Roll_Packing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
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

    Private Sub Roll_Packing_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        FrmLdSTS = True

        dgv_Details.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))
        dgv_Selection.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))

        Me.Text = ""

        con.Open()

        dtp_Date.Text = ""
        msk_date.Text = ""

        lbl_Roll_Packing_PoNo.Visible = False
        lbl_Roll_Packing_Po_Date.Visible = False
        txt_Roll_Packing_PoNo.Visible = False
        msk_Roll_Packing_Po_Date.Visible = False
        dtp_Roll_Packing_Po_Date.Visible = False


        btn_BarcodePrint.Visible = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
            dgv_Details.Columns(1).HeaderText = "Doff No"
            dgv_Details.Columns(2).HeaderText = "FER No."
            dgv_Details.Columns(6).Visible = True
            dgv_Details.Columns(6).HeaderText = "GLM"
            dgv_Details.Columns(9).Visible = False
            dgv_Details.Columns(6).Width = dgv_Details.Columns(9).Width - 10
            dgv_Details.Columns(8).Width = dgv_Details.Columns(8).Width - 5
            dgv_Details.Columns(14).Visible = False
            dgv_Details.Columns(15).Visible = False

            dgv_Selection.Columns(1).HeaderText = "Doff No"
            dgv_Selection.Columns(2).HeaderText = "FER No."
            dgv_Selection.Columns(6).Visible = True
            dgv_Selection.Columns(6).HeaderText = "GLM"
            dgv_Selection.Columns(9).Visible = False
            dgv_Selection.Columns(6).Width = dgv_Details.Columns(6).Width
            dgv_Selection.Columns(8).Width = dgv_Details.Columns(8).Width

            dgv_Details.Columns(24).ReadOnly = True
            dgv_Details.Columns(25).ReadOnly = True

            'dgv_Selection.Columns(25).Visible = True
            'dgv_Selection.Columns(26).Visible = True

            lbl_Roll_Packing_PoNo.Visible = True
            lbl_Roll_Packing_Po_Date.Visible = True
            txt_Roll_Packing_PoNo.Visible = True
            msk_Roll_Packing_Po_Date.Visible = True
            dtp_Roll_Packing_Po_Date.Visible = True

            Label12.Text = "FER No"
            Label13.Text = "Doff No"

            Label16.Text = "Doff No"
            Label18.Text = "FER No"

            btn_BarcodePrint.Visible = True

        End If

        cbo_Bale_Bundle.Items.Clear()
        cbo_Bale_Bundle.Items.Add("BALE")
        cbo_Bale_Bundle.Items.Add("BUNDLE")
        cbo_Bale_Bundle.Items.Add("ROLL")

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add("PCS")
        cbo_Type.Items.Add("BUYER-OFFER")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If





        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Bale_Bundle.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Party.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_BuyerRefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_PcsNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_RollNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Ok.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LotSelction.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsSelction.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Roll_Packing_PoNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Roll_Packing_Po_Date.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Bale_Bundle.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Party.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_BuyerRefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_PcsNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_RollNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Roll_Packing_PoNo.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Ok.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LotSelction.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsSelction.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Roll_Packing_Po_Date.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_LotNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_PcsNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_BuyerRefNo.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PrintFrom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_LotNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_PcsNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_BuyerRefNo.KeyPress, AddressOf TextBoxControlKeyPress

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

            Else
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= 16 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Note.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(16)

                            End If

                        ElseIf .CurrentCell.ColumnIndex <= 15 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(16)

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True


                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 16 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If txt_Folding.Enabled Then
                                    txt_Folding.Focus()
                                Else
                                    cbo_Type.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(16)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(16)
                            '.CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

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
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "Company_Type = 'ACCOUNT'"
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
            dt1 = New DataTable
            da.Fill(dt1)

            NoofComps = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NoofComps = Val(dt1.Rows(0)(0).ToString)
                End If
            End If
            dt1.Clear()

            If Val(NoofComps) > 1 Then

                Dim f As New Company_Selection
                f.ShowDialog()

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
                            lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()
                    dt1.Dispose()
                    da.Dispose()

                    new_record()

                Else
                    Me.Close()

                End If

            Else

                Me.Close()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        Dim vBaleDelvCd As String = ""

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from Roll_Packing_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Roll_Packing_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Roll_Packing_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Roll_Packing_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))
                cbo_Bale_Bundle.Text = dt1.Rows(0).Item("Bale_Bundle").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Pcs_BufferOffer_Type").ToString
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_Folding.Text = Val(dt1.Rows(0).Item("Folding").ToString)
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                txt_Roll_Packing_PoNo.Text = dt1.Rows(0).Item("Roll_Packing_Po_No").ToString
                msk_Roll_Packing_Po_Date.Text = dt1.Rows(0).Item("Roll_Packing_Po_Date").ToString

                LockSTS = False

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.clothtype_name as Pcs_ClothTypeName, c.Ledger_Name as Pcs_PartyName, d.cloth_name as Pcs_ClothName from Roll_Packing_Details a INNER JOIN ClothType_Head b ON a.ClothType_IdNo <> 0 and a.ClothType_IdNo = b.ClothType_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Pcs_PartyIdNo <> 0 and a.Pcs_PartyIdNo = c.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON a.Pcs_Cloth_IdNo <> 0 and a.Pcs_Cloth_IdNo = d.Cloth_Idno where a.Roll_Packing_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Lot_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Pcs_NO").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Pcs_ClothTypeName").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        If Val(dgv_Details.Rows(n).Cells(5).Value) = 0 Then dgv_Details.Rows(n).Cells(5).Value = ""

                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")
                        If Val(dgv_Details.Rows(n).Cells(6).Value) = 0 Then dgv_Details.Rows(n).Cells(6).Value = ""

                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Buyer_Offer_No").ToString
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Buyer_RefNo").ToString

                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Party_PieceNo").ToString
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Pass_Meters").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(10).Value) = 0 Then dgv_Details.Rows(n).Cells(10).Value = ""
                        dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Less_Meters").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(11).Value) = 0 Then dgv_Details.Rows(n).Cells(11).Value = ""
                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Reject_Meters").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(12).Value) = 0 Then dgv_Details.Rows(n).Cells(12).Value = ""

                        dgv_Details.Rows(n).Cells(13).Value = Val(dt2.Rows(i).Item("Points").ToString)
                        If Val(dgv_Details.Rows(n).Cells(13).Value) = 0 Then dgv_Details.Rows(n).Cells(13).Value = ""
                        dgv_Details.Rows(n).Cells(14).Value = Val(dt2.Rows(i).Item("Point_Per_PassMeter").ToString)
                        If Val(dgv_Details.Rows(n).Cells(14).Value) = 0 Then dgv_Details.Rows(n).Cells(14).Value = ""

                        dgv_Details.Rows(n).Cells(15).Value = dt2.Rows(i).Item("Grade").ToString
                        dgv_Details.Rows(n).Cells(16).Value = dt2.Rows(i).Item("Roll_No").ToString

                        dgv_Details.Rows(n).Cells(17).Value = dt2.Rows(i).Item("lot_code").ToString
                        dgv_Details.Rows(n).Cells(18).Value = dt2.Rows(i).Item("Pcs_PartyName").ToString
                        dgv_Details.Rows(n).Cells(19).Value = dt2.Rows(i).Item("Pcs_ClothName").ToString

                        dgv_Details.Rows(n).Cells(20).Value = dt2.Rows(i).Item("Buyer_Offer_Code").ToString
                        dgv_Details.Rows(n).Cells(21).Value = dt2.Rows(i).Item("Roll_Code").ToString

                        vBaleDelvCd = ""
                        da3 = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Packing_Slip_Code = '" & Trim(dt2.Rows(i).Item("Roll_Code").ToString) & "'", con)
                        dt3 = New DataTable
                        da3.Fill(dt3)
                        If dt3.Rows.Count > 0 Then
                            If IsDBNull(dt3.Rows(0).Item("Delivery_Code").ToString) = False Then
                                vBaleDelvCd = dt3.Rows(0).Item("Delivery_Code").ToString
                            End If
                        End If
                        dt3.Clear()

                        dgv_Details.Rows(n).Cells(22).Value = vBaleDelvCd

                        If Val(dt2.Rows(i).Item("Loom_IdNo").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(23).Value = Common_Procedures.Loom_IdNoToName(con, Val(dt2.Rows(i).Item("Loom_IdNo").ToString))
                        Else
                            dgv_Details.Rows(n).Cells(23).Value = dt2.Rows(i).Item("Loom_No").ToString
                        End If

                        dgv_Details.Rows(n).Cells(24).Value = Format(Val(dt2.Rows(i).Item("Tare_Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(25).Value = Format(Val(dt2.Rows(i).Item("Gross_Weight").ToString), "########0.000")

                        If Trim(vBaleDelvCd) <> "" Then
                            For J = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                                dgv_Details.Rows(n).Cells(J).Style.ForeColor = Color.Red
                                dgv_Details.Rows(n).Cells(J).ReadOnly = True
                            Next
                            LockSTS = True
                        End If

                    Next i

                End If

                With dgv_Details_Total

                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")

                    .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_Passed_Meters").ToString), "########0.00")
                    .Rows(0).Cells(11).Value = Format(Val(dt1.Rows(0).Item("Total_Less_Meters").ToString), "########0.00")
                    .Rows(0).Cells(12).Value = Format(Val(dt1.Rows(0).Item("Total_Reject_Meters").ToString), "########0.00")
                    .Rows(0).Cells(13).Value = Val(dt1.Rows(0).Item("Total_Points").ToString)
                    .Rows(0).Cells(24).Value = Format(Val(dt1.Rows(0).Item("Total_Gross_weight").ToString), "########0.00")


                End With

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            If LockSTS = True Then

                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_Cloth.Enabled = False
                cbo_Cloth.BackColor = Color.LightGray

                cbo_ClothType.Enabled = False
                cbo_ClothType.BackColor = Color.LightGray

                txt_Folding.Enabled = False
                txt_Folding.BackColor = Color.LightGray

                'btn_Selection.Enabled = False

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
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ClothSales_Roll_Packing_Entry, New_Entry, Me, con, "Roll_Packing_Head", "Roll_Packing_Code", NewCode, "Roll_Packing_Date", "(Roll_Packing_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Packing_Slip_Head Where Roll_Packing_Code = '" & Trim(NewCode) & "' and Packing_Slip_Code LIKE '" & Trim(Pk_Condition) & "%' and Delivery_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some rolls delivered/invoiced", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Roll_packing_head", "Roll_packing_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Roll_packing_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Roll_packing_Details", "Roll_packing_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Lot_No,Pcs_No,Pcs_ClothType_IdNo,Meters ,Weight,Weight_Meter,Buyer_Offer_No,Buyer_RefNo,Party_PieceNo,Pass_Meters,Less_Meters,Reject_Meters,Points,Point_Per_PassMeter,Grade,Roll_No,Lot_Code,Pcs_PartyIdNo,Pcs_Cloth_IdNo,Buyer_Offer_Code,Roll_Code,Loom_IdNo,Loom_No", "Sl_No", "Roll_packing_Code, For_OrderBy, Company_IdNo, Roll_packing_No, Roll_packing_Date, Ledger_Idno", trans)

            Da = New SqlClient.SqlDataAdapter("select * from Roll_Packing_Details Where Roll_Packing_Code = '" & Trim(NewCode) & "' Order by sl_no ", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1, Roll_No_Type1 = '' Where PackingSlip_Code_Type1 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1, Roll_No_Type2 = '' Where PackingSlip_Code_Type2 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1, Roll_No_Type3 = '' Where PackingSlip_Code_Type3 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1, Roll_No_Type4 = '' Where PackingSlip_Code_Type4 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1, Roll_No_Type5 = '' Where PackingSlip_Code_Type5 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "delete from Packing_Slip_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "' and Roll_Packing_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "delete from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "' and Roll_Packing_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                Next i
            End If
            Dt1.Clear()

            cmd.CommandText = "delete from Roll_Packing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Roll_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'"
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
            cbo_Filter_Party.Text = ""
            cbo_Filter_Cloth.Text = ""
            txt_Filter_BuyerRefNo.Text = ""
            txt_Filter_LotNo.Text = ""
            txt_Filter_PcsNo.Text = ""
            txt_Filter_RollNo.Text = ""

            cbo_Filter_Party.SelectedIndex = -1
            cbo_Filter_Cloth.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Roll_Packing_No from Roll_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Roll_Packing_Code like '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Roll_Packing_No", con)
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

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Roll_Packing_No from Roll_Packing_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Roll_Packing_Code like '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Roll_Packing_No", con)
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

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Roll_Packing_No from Roll_Packing_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Roll_Packing_Code like '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Roll_Packing_No desc", con)
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

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Roll_Packing_No from Roll_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Roll_Packing_Code like '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Roll_Packing_No desc", con)
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

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Roll_Packing_Head", "Roll_Packing_Code", "For_OrderBy", "(Roll_Packing_Code like '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString

            'msk_Roll_Packing_Po_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Roll_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Roll_Packing_Code like '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Roll_Packing_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Roll_Packing_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Roll_Packing_Date").ToString
                End If

                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))
                If dt1.Rows(0).Item("Bale_Bundle").ToString <> "" Then
                    cbo_Bale_Bundle.Text = dt1.Rows(0).Item("Bale_Bundle").ToString
                End If
                If dt1.Rows(0).Item("Pcs_BufferOffer_Type").ToString <> "" Then
                    cbo_Type.Text = dt1.Rows(0).Item("Pcs_BufferOffer_Type").ToString
                End If

                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "########0.00")

            End If
            dt1.Clear()


            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

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

            RecCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Roll_Packing_No from Roll_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(RecCode) & "'", con)
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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.ClothSales_Roll_Packing_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Roll_Packing_No from Roll_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Roll No", "DOES NOT INSERT NEW REF NO...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF No...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vNewRollCode As String = ""
        Dim vRollNo As String = ""
        Dim Clth_ID As Integer = 0
        Dim Clthty_ID As Integer = 0
        Dim dCloTyp_ID As Integer = 0
        Dim dClo_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim led_id As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotMtrs As Single, vTotPcs As Single, vTotWgt As Single, vTotGrsWgt As Single
        Dim EntID As String = ""
        Dim dparty_ID As Integer = 0
        Dim vTotPassMtrs As Single = 0
        Dim vTotLessMtrs As Single = 0
        Dim vTotRejMtrs As Single = 0
        Dim vTotPts As Single = 0
        Dim Nr As Long = 0, Nr1 As Long = 0, Nr2 As Long = 0, Nr3 As Long = 0
        Dim vLmIdNo As Integer = 0
        Dim vLmNo As String = ""
        Dim vOrdByNo As String = ""
        Dim vPCKSLP_SlNo As String = 0
        Dim vROLL_PREFIXNO As String
        Dim vROLL_REFNO As String
        Dim vROLL_SUFFIXNO As String
        Dim Dup_SetNoBmNo As String
        Dim vRoll_Packing_Po_Date As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(lbl_RefNo.Text) = "" Then
            MessageBox.Show("Invalid Ref.No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ClothSales_Roll_Packing_Entry, New_Entry, Me, con, "Roll_Packing_Head", "Roll_Packing_Code", NewCode, "Roll_Packing_Date", "(Roll_Packing_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Roll_Packing_No desc", dtp_Date.Value.Date) = False Then Exit Sub
        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.PackinSlip_Entry, New_Entry) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If Clth_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        Clthty_ID = Common_Procedures.ClothType_NameToIdNo(con, cbo_ClothType.Text)
        If Clthty_ID = 0 Then
            MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothType.Enabled And cbo_ClothType.Visible Then cbo_ClothType.Focus()
            Exit Sub
        End If


        With dgv_Details

            Dup_SetNoBmNo = ""
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then

                    'If Trim(.Rows(i).Cells(7).Value) = "" Then
                    '    MessageBox.Show("Invalid Party Pcs.No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(7)
                    '    End If
                    '    Exit Sub
                    'End If

                    If Trim(UCase(cbo_Type.Text)) = Trim(UCase("BUYER-OFFER")) Then
                        If Val(.Rows(i).Cells(8).Value) = 0 And Val(.Rows(i).Cells(9).Value) = 0 And Val(.Rows(i).Cells(10).Value) = 0 Then
                            MessageBox.Show("Invalid Pass/Less/Rejection Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(8)
                            End If
                            Exit Sub
                        End If
                    End If

                    If Trim(.Rows(i).Cells(16).Value) = "" Then
                        MessageBox.Show("Invalid Roll No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(16)
                        End If
                        Exit Sub
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                        If Val(.Rows(i).Cells(16).Value) = 0 Then
                            MessageBox.Show("Invalid Roll No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(16)
                            End If
                            Exit Sub
                        End If
                    End If


                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then

                        If InStr(1, Trim(UCase(Dup_SetNoBmNo)), "~" & Trim(UCase(.Rows(i).Cells(16).Value)) & "~") > 0 Then
                            MessageBox.Show("Duplicate Roll No. ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(16)
                                .CurrentCell.Selected = True
                            End If
                            Exit Sub
                        End If

                        Dup_SetNoBmNo = Trim(Dup_SetNoBmNo) & "~" & Trim(UCase(.Rows(i).Cells(16).Value)) & "~"

                    End If

                End If

            Next

        End With

        vTotMtrs = 0 : vTotPcs = 0 : vTotWgt = 0
        vTotPassMtrs = 0 : vTotLessMtrs = 0 : vTotRejMtrs = 0 : vTotPts = 0 : vTotGrsWgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
            vTotWgt = Val(dgv_Details_Total.Rows(0).Cells(5).Value())

            vTotPassMtrs = Val(dgv_Details_Total.Rows(0).Cells(10).Value())
            vTotLessMtrs = Val(dgv_Details_Total.Rows(0).Cells(11).Value())
            vTotRejMtrs = Val(dgv_Details_Total.Rows(0).Cells(12).Value())
            vTotPts = Val(dgv_Details_Total.Rows(0).Cells(13).Value())
            vTotGrsWgt = Val(dgv_Details_Total.Rows(0).Cells(25).Value())

        End If

        Dim vOrdDt = ""
        If Trim(msk_Roll_Packing_Po_Date.Text) <> "" Then
            If IsDate(msk_Roll_Packing_Po_Date.Text) = True Then
                vOrdDt = Trim(msk_Roll_Packing_Po_Date.Text)
            End If
        End If
        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Roll_Packing_Head", "Roll_Packing_Code", "For_OrderBy", "(Roll_Packing_Code like '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))
            If New_Entry = True Then

                cmd.CommandText = "Insert into Roll_Packing_Head (   Roll_Packing_Code   ,                 Company_IdNo     ,           Roll_Packing_No     ,                               for_OrderBy                              , Roll_Packing_Date,    Pcs_BufferOffer_Type      ,        Ledger_IdNo      ,             Cloth_IdNo   ,          ClothType_IdNo    ,                 Folding           ,               Bale_Bundle           ,  Total_Pcs     ,         Total_Meters       ,         Total_Weight      ,        Total_Passed_Meters     ,          Total_Less_Meters     ,       Total_Reject_Meters     ,          Total_Points     ,               Note           ,                           User_IdNo                ,              Roll_Packing_Po_No           ,        Total_Gross_Weight    ,     Roll_Packing_Po_Date                       ) " &
                                    "          Values            ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @EntryDate    , '" & Trim(cbo_Type.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(Clth_ID)) & ", " & Str(Val(Clthty_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", '" & Trim(cbo_Bale_Bundle.Text) & "', " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & " , " & Str(Val(vTotWgt)) & " , " & Str(Val(vTotPassMtrs)) & " , " & Str(Val(vTotLessMtrs)) & " , " & Str(Val(vTotRejMtrs)) & " , " & Str(Val(vTotPts)) & " , '" & Trim(txt_Note.Text) & "', " & Val(Common_Procedures.User.IdNo) & " , '" & Trim(txt_Roll_Packing_PoNo.Text) & "',  " & Str(Val(vTotGrsWgt)) & ",   '" & Trim(vOrdDt) & "') "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Roll_packing_head", "Roll_packing_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Roll_packing_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Roll_packing_Details", "Roll_packing_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No,Pcs_No,Pcs_ClothType_IdNo,Meters ,Weight,Weight_Meter,Buyer_Offer_No,Buyer_RefNo,Party_PieceNo,Pass_Meters,Less_Meters,Reject_Meters,Points,Point_Per_PassMeter,Grade,Roll_No,Lot_Code,Pcs_PartyIdNo,Pcs_Cloth_IdNo,Buyer_Offer_Code,Roll_Code,Loom_IdNo,Loom_No", "Sl_No", "Roll_packing_Code, For_OrderBy, Company_IdNo, Roll_packing_No, Roll_packing_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Roll_Packing_Head Set Roll_Packing_Date = @EntryDate, Ledger_IdNo = " & Str(Val(led_id)) & " , Pcs_BufferOffer_Type = '" & Trim(cbo_Type.Text) & "' , Cloth_IdNo = " & Str(Val(Clth_ID)) & " , ClothType_IdNo = " & Str(Val(Clthty_ID)) & " , Folding = " & Str(Val(txt_Folding.Text)) & ", Bale_Bundle = '" & Trim(cbo_Bale_Bundle.Text) & "', Total_Pcs = " & Str(Val(vTotPcs)) & ", Total_Meters = " & Str(Val(vTotMtrs)) & " , Total_Weight = " & Str(Val(vTotWgt)) & " , Total_Passed_Meters = " & Str(Val(vTotPassMtrs)) & " , Total_Less_Meters = " & Str(Val(vTotLessMtrs)) & " , Total_Reject_Meters = " & Str(Val(vTotRejMtrs)) & " , Total_Points = " & Str(Val(vTotPts)) & " , Note = '" & Trim(txt_Note.Text) & "' , User_IdNo = " & Val(Common_Procedures.User.IdNo) & ", Total_Gross_Weight = " & Str(Val(vTotGrsWgt)) & ", Roll_Packing_Po_Date = '" & Trim(vOrdDt) & "'  ,  Roll_Packing_Po_No = '" & Trim(txt_Roll_Packing_PoNo.Text) & "'   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("select * from Roll_Packing_Details Where Roll_Packing_Code = '" & Trim(NewCode) & "' Order by sl_no ", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1, Roll_No_Type1 = '' Where PackingSlip_Code_Type1 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1, Roll_No_Type2 = '' Where PackingSlip_Code_Type2 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1, Roll_No_Type3 = '' Where PackingSlip_Code_Type3 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1, Roll_No_Type4 = '' Where PackingSlip_Code_Type4 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1, Roll_No_Type5 = '' Where PackingSlip_Code_Type5 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                        cmd.ExecuteNonQuery()

                    Next i

                End If
                Dt1.Clear()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            If Trim(lbl_RefNo.Text) <> "" Then
                Partcls = "Delv : Dc.No. " & Trim(lbl_RefNo.Text)
            End If
            PBlNo = Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Roll_Packing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Packing_Slip_Details From Packing_Slip_Details a, Packing_Slip_Head b Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Roll_Packing_Code = '" & Trim(NewCode) & "' and b.Delivery_Code = '' and a.Packing_Slip_Code = b.Packing_Slip_Code and a.Roll_Packing_Code = b.Roll_Packing_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Packing_Slip_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "' and Delivery_Code = ''"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        dCloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        dparty_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(18).Value, tr)
                        dClo_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(19).Value, tr)

                        vLmNo = .Rows(i).Cells(23).Value
                        vLmIdNo = Common_Procedures.Loom_NameToIdNo(con, .Rows(i).Cells(23).Value, tr)

                        vNewRollCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "\" & Trim(.Rows(i).Cells(16).Value) & "/" & Trim(Common_Procedures.FnYearCode)
                        'vNewRollCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(.Rows(i).Cells(16).Value) & "/" & Trim(Common_Procedures.FnYearCode)
                        .Rows(i).Cells(21).Value = vNewRollCode
                        vRollNo = Trim(.Rows(i).Cells(16).Value)

                        'If Trim(.Rows(i).Cells(16).Value) = "3144" Then
                        '    Debug.Print(Trim(.Rows(i).Cells(16).Value))
                        'End If

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Roll_Packing_Details (     Roll_Packing_Code   ,                 Company_IdNo     ,          Roll_Packing_No     ,                               for_OrderBy                              , Roll_Packing_Date ,        Ledger_IdNo      ,           Cloth_IdNo      ,          ClothType_IdNo     ,                  Folding           ,              Sl_No    ,                     Lot_No              ,                    Pcs_No              ,        Pcs_ClothType_IdNo   ,                      Meters              ,                      Weight              ,                      Weight_Meter        ,                    Buyer_Offer_No       ,                    Buyer_RefNo          ,                    Party_PieceNo        ,                      Pass_Meters          ,                      Less_Meters          ,                      Reject_Meters        ,                      Points               ,                    Point_Per_PassMeter    ,                    Grade                 ,                    Roll_No               ,                    Lot_Code              ,          Pcs_PartyIdNo      ,       Pcs_Cloth_IdNo     ,                Buyer_Offer_Code          ,                    Roll_Code             ,             Loom_IdNo     ,          Loom_No       ,                Tare_Weight                 ,                   Gross_Weight       ) " &
                                            "          Values               ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @EntryDate    , " & Str(Val(led_id)) & ",  " & Str(Val(Clth_ID)) & ",  " & Str(Val(Clthty_ID)) & ",  " & Str(Val(txt_Folding.Text)) & ",  " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(dCloTyp_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", '" & Trim(.Rows(i).Cells(7).Value) & "' , '" & Trim(.Rows(i).Cells(8).Value) & "' , '" & Trim(.Rows(i).Cells(9).Value) & "' , " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & ", " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & ", " & Str(Val(.Rows(i).Cells(14).Value)) & ", '" & Trim(.Rows(i).Cells(15).Value) & "' , '" & Trim(.Rows(i).Cells(16).Value) & "' , '" & Trim(.Rows(i).Cells(17).Value) & "' , " & Str(Val(dparty_ID)) & " , " & Str(Val(dClo_ID)) & ", '" & Trim(.Rows(i).Cells(20).Value) & "' , '" & Trim(.Rows(i).Cells(21).Value) & "' , " & Str(Val(vLmIdNo)) & " , '" & Trim(vLmNo) & "' , " & Str(Val(.Rows(i).Cells(24).Value)) & " ,  " & Str(Val(.Rows(i).Cells(25).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" Then

                            Da = New SqlClient.SqlDataAdapter("select * from Packing_Slip_Head Where Roll_Packing_Code <> '" & Trim(NewCode) & "' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_No = '" & Trim(.Rows(i).Cells(16).Value) & "' and Packing_Slip_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "'", con)
                            Da.SelectCommand.Transaction = tr
                            Dt1 = New DataTable
                            Da.Fill(Dt1)
                            If Dt1.Rows.Count > 0 Then
                                If IsDBNull(Dt1.Rows(0).Item("Packing_Slip_Code").ToString) = False Then
                                    If Trim(Dt1.Rows(0).Item("Packing_Slip_Code").ToString) <> "" Then
                                        Throw New ApplicationException("Duplicate Roll No. : " & Trim(.Rows(i).Cells(16).Value) & Chr(13) & "This RollNo is already in " & Trim(Dt1.Rows(0).Item("Roll_Packing_Code").ToString))
                                        Exit Sub
                                    End If
                                End If
                            End If
                            Dt1.Clear()

                        End If

                        vROLL_PREFIXNO = Common_Procedures.get_Prefix_from_OrderByCode(.Rows(i).Cells(16).Value)
                        vROLL_REFNO = Replace(Trim(UCase(.Rows(i).Cells(16).Value)), Trim(UCase(vROLL_PREFIXNO)), "")
                        vROLL_SUFFIXNO = ""

                        Nr = 0
                        cmd.CommandText = "Update Packing_Slip_Head set Packing_Slip_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & " , Roll_Packing_Party_IdNo = " & Str(Val(led_id)) & " , Note = '" & Trim(txt_Note.Text) & "' , User_IdNo = " & Val(Common_Procedures.User.IdNo) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'  and Packing_Slip_Code = '" & Trim(vNewRollCode) & "' and Delivery_Code <> ''"
                        Nr = cmd.ExecuteNonQuery()
                        If Nr = 0 Then

                            Nr1 = 0
                            Nr2 = 0
                            cmd.CommandText = "Update Packing_Slip_Head set Packing_Slip_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & " , Roll_Packing_Party_IdNo = " & Str(Val(led_id)) & " , Total_Pcs = Total_Pcs + 1, Total_Meters = Total_Meters + " & Str(Val(.Rows(i).Cells(10).Value)) & " , Total_Weight = Total_Weight + " & Str(Val(.Rows(i).Cells(5).Value)) & " , Net_Weight = Net_Weight + " & Str(Val(.Rows(i).Cells(5).Value)) & ", Tare_Weight = Tare_Weight + " & Str(Val(.Rows(i).Cells(24).Value)) & ", Gross_Weight = Gross_Weight + " & Str(Val(.Rows(i).Cells(25).Value)) & " , Note = '" & Trim(txt_Note.Text) & "' , User_IdNo = " & Val(Common_Procedures.User.IdNo) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'  and Packing_Slip_Code = '" & Trim(vNewRollCode) & "'"
                            Nr1 = cmd.ExecuteNonQuery()
                            If Nr1 = 0 Then

                                cmd.CommandText = "Insert into Packing_Slip_Head ( Roll_Packing_Code ,       Packing_Slip_Code     ,               Company_IdNo       ,        Packing_Slip_PrefixNo  ,        Packing_Slip_RefNo  ,        Packing_Slip_SuffixNo  ,            Packing_Slip_No              ,                               for_OrderBy                                        , Packing_Slip_Date,                                             Ledger_IdNo    ,    Roll_Packing_Party_IdNo,     Pcs_BufferOffer_Type     ,             Cloth_IdNo   ,            ClothType_IdNo  ,              Bale_Bundle            ,                  Folding           , Total_Pcs ,                      Total_Meters          ,                      Total_Weight          ,                      Net_Weight          ,                      Tare_Weight          ,                      Gross_Weight          ,               Note           ,                        Year_Code            ,             User_IdNo                    ) " &
                                                "          Values            ('" & Trim(NewCode) & "', '" & Trim(vNewRollCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vROLL_PREFIXNO) & "', '" & Trim(vROLL_REFNO) & "', '" & Trim(vROLL_SUFFIXNO) & "', '" & Trim(.Rows(i).Cells(16).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(16).Value))) & ",      @EntryDate  ,  " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(led_id)) & "  , '" & Trim(cbo_Type.Text) & "', " & Str(Val(Clth_ID)) & ", " & Str(Val(Clthty_ID)) & ", '" & Trim(cbo_Bale_Bundle.Text) & "',  " & Str(Val(txt_Folding.Text)) & ",     1     , " & Str(Val(.Rows(i).Cells(10).Value)) & " , " & Str(Val(.Rows(i).Cells(5).Value)) & "  , " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(24).Value)) & ", " & Str(Val(.Rows(i).Cells(25).Value)) & " , '" & Trim(txt_Note.Text) & "', '" & Trim(Common_Procedures.FnYearCode) & "', " & Val(Common_Procedures.User.IdNo) & " ) "
                                Nr2 = cmd.ExecuteNonQuery()

                                'cmd.CommandText = "Insert into Packing_Slip_Head ( Roll_Packing_Code     ,       Packing_Slip_Code     ,               Company_IdNo       ,           Packing_Slip_No               ,                               for_OrderBy                                        , Packing_Slip_Date,                                             Ledger_IdNo    ,    Roll_Packing_Party_IdNo,     Pcs_BufferOffer_Type     ,             Cloth_IdNo   ,            ClothType_IdNo  ,              Bale_Bundle            ,                  Folding           , Total_Pcs ,                      Total_Meters          ,                      Total_Weight          ,                      Net_Weight          ,                      Tare_Weight          ,                      Gross_Weight          ,               Note           ,             User_IdNo                    ) " &
                                '                "          Values            ('" & Trim(NewCode) & "', '" & Trim(vNewRollCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(.Rows(i).Cells(16).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(16).Value))) & ",      @EntryDate  ,  " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(led_id)) & "  , '" & Trim(cbo_Type.Text) & "', " & Str(Val(Clth_ID)) & ", " & Str(Val(Clthty_ID)) & ", '" & Trim(cbo_Bale_Bundle.Text) & "',  " & Str(Val(txt_Folding.Text)) & ",     1     , " & Str(Val(.Rows(i).Cells(10).Value)) & " , " & Str(Val(.Rows(i).Cells(5).Value)) & "  , " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(24).Value)) & ", " & Str(Val(.Rows(i).Cells(25).Value)) & " , '" & Trim(txt_Note.Text) & "', " & Val(Common_Procedures.User.IdNo) & " ) "
                                'Nr2 = cmd.ExecuteNonQuery()

                            End If

                            If Nr1 > 0 Or Nr2 > 0 Then

                                Nr3 = 0
                                vPCKSLP_SlNo = 0
                                cmd.CommandText = "Update Packing_Slip_Details set Packing_Slip_Date = @EntryDate Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'  and Packing_Slip_Code = '" & Trim(vNewRollCode) & "' and Lot_Code = '" & Trim(.Rows(i).Cells(17).Value) & "' and Pcs_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                                Nr3 = cmd.ExecuteNonQuery()
                                If Nr3 = 0 Then

                                    vPCKSLP_SlNo = Common_Procedures.get_MaxIdNo(con, "Packing_Slip_Details", "Sl_No", "(Packing_Slip_Code = '" & Trim(vNewRollCode) & "')", tr)

                                    cmd.CommandText = "Insert into Packing_Slip_Details (     Roll_Packing_Code  ,       Packing_Slip_Code       ,           Company_IdNo           ,        Packing_Slip_No                  ,                               for_OrderBy                                        , Packing_Slip_Date,          Cloth_IdNo      ,                  Folding           ,               Sl_No            ,                     Lot_No              ,                    Pcs_No              ,           ClothType_IdNo    ,                      Meters               ,                      Weight              ,                      Weight_Meter        ,            Party_IdNo       ,                    Lot_Code              ,             Loom_IdNo     ,          Loom_No      ) " &
                                                        "           Values              ( '" & Trim(NewCode) & "', '" & Trim(vNewRollCode) & "'  , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(.Rows(i).Cells(16).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(16).Value))) & ",     @EntryDate   , " & Str(Val(dClo_ID)) & ",  " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(vPCKSLP_SlNo)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(dCloTyp_ID)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(dparty_ID)) & " , '" & Trim(.Rows(i).Cells(17).Value) & "' , " & Str(Val(vLmIdNo)) & " , '" & Trim(vLmNo) & "' ) "
                                    cmd.ExecuteNonQuery()

                                End If

                            End If


                        End If



                        If dCloTyp_ID = 1 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '" & Trim(vNewRollCode) & "', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 + 1, Roll_No_Type1 = '" & Trim(.Rows(i).Cells(16).Value) & "', Roll_Tare_Weight = " & Str(Val(.Rows(i).Cells(24).Value)) & ", Roll_Gross_Weight = " & Str(Val(.Rows(i).Cells(25).Value)) & " Where lot_code = '" & Trim(.Rows(i).Cells(17).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and PackingSlip_Code_Type1 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                        ElseIf dCloTyp_ID = 2 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '" & Trim(vNewRollCode) & "', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 + 1, Roll_No_Type2 = '" & Trim(.Rows(i).Cells(16).Value) & "', Roll_Tare_Weight = " & Str(Val(.Rows(i).Cells(24).Value)) & ", Roll_Gross_Weight = " & Str(Val(.Rows(i).Cells(25).Value)) & " Where lot_code = '" & Trim(.Rows(i).Cells(17).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and PackingSlip_Code_Type2 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                        ElseIf dCloTyp_ID = 3 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '" & Trim(vNewRollCode) & "', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 + 1, Roll_No_Type3 = '" & Trim(.Rows(i).Cells(16).Value) & "', Roll_Tare_Weight = " & Str(Val(.Rows(i).Cells(24).Value)) & ", Roll_Gross_Weight = " & Str(Val(.Rows(i).Cells(25).Value)) & " Where lot_code = '" & Trim(.Rows(i).Cells(17).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and PackingSlip_Code_Type3 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                        ElseIf dCloTyp_ID = 4 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '" & Trim(vNewRollCode) & "', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 + 1, Roll_No_Type4 = '" & Trim(.Rows(i).Cells(16).Value) & "', Roll_Tare_Weight = " & Str(Val(.Rows(i).Cells(24).Value)) & ", Roll_Gross_Weight = " & Str(Val(.Rows(i).Cells(25).Value)) & " Where lot_code = '" & Trim(.Rows(i).Cells(17).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and PackingSlip_Code_Type4 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                        ElseIf dCloTyp_ID = 5 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '" & Trim(vNewRollCode) & "', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 + 1, Roll_No_Type5 = '" & Trim(.Rows(i).Cells(16).Value) & "', Roll_Tare_Weight = " & Str(Val(.Rows(i).Cells(24).Value)) & ", Roll_Gross_Weight = " & Str(Val(.Rows(i).Cells(25).Value)) & " Where lot_code = '" & Trim(.Rows(i).Cells(17).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and PackingSlip_Code_Type5 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                        End If

                    End If

                Next

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Roll_packing_Details", "Roll_packing_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No,Pcs_No,Pcs_ClothType_IdNo,Meters ,Weight,Weight_Meter,Buyer_Offer_No,Buyer_RefNo,Party_PieceNo,Pass_Meters,Less_Meters,Reject_Meters,Points,Point_Per_PassMeter,Grade,Roll_No,Lot_Code,Pcs_PartyIdNo,Pcs_Cloth_IdNo,Buyer_Offer_Code,Roll_Code,Loom_IdNo,Loom_No", "Sl_No", "Roll_packing_Code, For_OrderBy, Company_IdNo, Roll_packing_No, Roll_packing_Date, Ledger_Idno", tr)

            End With

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)


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
            If InStr(1, Trim(UCase(ex.Message)), Trim(UCase("PK_Packing_Slip_Head"))) > 0 Then
                MessageBox.Show("Duplicate Roll No in this Entry - " & Trim(vRollNo), "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(UCase(ex.Message)), Trim(UCase("PK_Packing_Slip_Details"))) > 0 Then
                MessageBox.Show("Duplicate Roll No in this Entry - " & Trim(vRollNo), "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If


        Finally
            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()
            cmd.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotMtrs As String, TotPcs As Integer, TotWgt As String
        Dim TotPassMtrs As String, TotRejMtrs As String, TotLessMtrs As String, TotPts As Single

        Dim Totgrswgt As String, TotTarewgt As String

        Try
            If FrmLdSTS = True Then Exit Sub

            Sno = 0
            TotPcs = 0
            TotMtrs = 0
            TotWgt = 0
            TotPassMtrs = 0
            TotRejMtrs = 0
            TotLessMtrs = 0
            TotPts = 0
            Totgrswgt = 0


            Totgrswgt = 0 : TotTarewgt = 0

            With dgv_Details
                For i = 0 To .RowCount - 1
                    Sno = Sno + 1
                    .Rows(i).Cells(0).Value = Sno
                    If Val(.Rows(i).Cells(4).Value) <> 0 Then
                        'TotRolls = TotRolls + 1
                        TotPcs = TotPcs + 1
                        TotMtrs = Format(Val(TotMtrs) + Val(.Rows(i).Cells(4).Value), "###########0.00")
                        TotWgt = Format(Val(TotWgt) + Val(.Rows(i).Cells(5).Value), "###########0.000")

                        TotPassMtrs = Format(Val(TotPassMtrs) + Val(.Rows(i).Cells(10).Value), "###########0.00")
                        TotRejMtrs = Format(Val(TotRejMtrs) + Val(.Rows(i).Cells(11).Value), "###########0.00")
                        TotLessMtrs = Format(Val(TotLessMtrs) + Val(.Rows(i).Cells(12).Value), "###########0.00")
                        TotPts = TotPts + Val(.Rows(i).Cells(13).Value)

                        Totgrswgt = Format(Val(Totgrswgt) + Val(.Rows(i).Cells(25).Value), "###########0.000")
                        'TotTarewgt = Format(Val(TotTarewgt) + Val(.Rows(i).Cells(25).Value), "###########0.000")

                    End If
                Next
            End With

            With dgv_Details_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(2).Value = Val(TotPcs)
                .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
                .Rows(0).Cells(5).Value = Format(Val(TotWgt), "########0.000")

                .Rows(0).Cells(10).Value = Format(Val(TotPassMtrs), "########0.00")
                .Rows(0).Cells(11).Value = Format(Val(TotRejMtrs), "########0.00")
                .Rows(0).Cells(12).Value = Format(Val(TotLessMtrs), "########0.00")
                .Rows(0).Cells(13).Value = Val(TotPts)

                .Rows(0).Cells(25).Value = Format(Val(Totgrswgt), "########0.000")
                '.Rows(0).Cells(25).Value = Format(Val(TotTarewgt), "########0.000")
            End With

        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub
    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, cbo_Type, cbo_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, cbo_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ClothType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub
    Private Sub cbo_ClothType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothType, cbo_Cloth, cbo_Bale_Bundle, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub

    Private Sub cbo_ClothType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothType, cbo_Bale_Bundle, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")
    End Sub

    Private Sub cbo_Bale_Bundle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Bale_Bundle.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Bale_Bundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Bale_Bundle.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Bale_Bundle, cbo_ClothType, txt_Folding, "", "", "", "")

    End Sub

    Private Sub cbo_Bale_Bundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Bale_Bundle.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Bale_Bundle, txt_Folding, "", "", "", "")
    End Sub

    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(16)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_Note.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Piece", "FOR PIECE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(16)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_Note.Focus()
                End If

            End If

        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If (e.KeyValue = 40) Then

            If txt_Roll_Packing_PoNo.Enabled And txt_Roll_Packing_PoNo.Visible = True Then
                txt_Roll_Packing_PoNo.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_date.Focus()
                End If
            End If
        End If

        If (e.KeyValue = 38) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(16)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_Folding.Focus()
            End If
        End If

    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If txt_Roll_Packing_PoNo.Enabled And txt_Roll_Packing_PoNo.Visible = True Then
                txt_Roll_Packing_PoNo.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_date.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 16 Then
                            If Trim(.Rows(e.RowIndex).Cells(22).Value) <> "" Then
                                .Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly = True
                            Else
                                .Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly = False
                            End If
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        Try

            If FrmLdSTS = True Then Exit Sub

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 2 Or e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 10 Or e.ColumnIndex = 11 Or e.ColumnIndex = 12 Or e.ColumnIndex = 13 Or e.ColumnIndex = 25 Then
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

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer = 0
        Dim n As Integer = 0
        Dim S As String = ""

        Try

            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details

                    n = .CurrentRow.Index

                    If Trim(.Rows(n).Cells(22).Value) = "" Then

                        If .Rows.Count <= 1 Then
                            For i = 0 To .ColumnCount - 1
                                .Rows(n).Cells(i).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(n)

                        End If

                        For i = 0 To .Rows.Count - 1
                            .Rows(i).Cells(0).Value = i + 1
                        Next

                        Total_Calculation()

                    Else

                        S = ""
                        If Trim(.Rows(n).Cells(22).Value) <> "" Then
                            S = " Already Rolls Delivered/Invoiced = " & Trim(.Rows(n).Cells(22).Value)
                        End If
                        MessageBox.Show(S, "DOES NOT REMOVE THIS ROLL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub

                    End If

                End With

            End If


            '----------------------

            With dgv_Details
                If .Visible Then

                    For i = 0 To .RowCount - 1

                        If Val(.Rows(i).Cells(25).Value) <> 0 And Val(.Rows(i).Cells(5).Value) <> 0 Then
                            .Rows(i).Cells(24).Value = Format(Val(.Rows(i).Cells(25).Value) - Val(.Rows(i).Cells(5).Value), "###########0.000")
                            .Rows(i).Cells(24).ReadOnly = True 'Tarewgt
                        End If

                        If Val(.Rows(i).Cells(24).Value) <> 0 And Val(.Rows(i).Cells(5).Value) <> 0 Then
                            .Rows(i).Cells(25).Value = Format(Val(.Rows(i).Cells(24).Value) + Val(.Rows(i).Cells(5).Value), "###########0.000")
                            .Rows(i).Cells(25).ReadOnly = True 'Grswgt
                        End If

                    Next

                End If

            End With

            '----------------------

        Catch ex As Exception
            '------------

        End Try

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

        Try

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 16 Then
                    If Common_Procedures.Accept_AlphaNumeric_WithOutSpecialCharacters_Only(Asc(e.KeyChar)) = 0 Then
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
        Dim Led_IdNo As Integer = 0
        Dim Clo_IdNo As Integer = 0
        Dim ClTyp_IdNo As Integer = 0
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clo_IdNo = 0
            ClTyp_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Roll_Packing_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Roll_Packing_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Roll_Packing_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Party.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_Party.Text)
            End If
            If Trim(cbo_Filter_Cloth.Text) <> "" Then
                Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Cloth.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If
            If Val(Clo_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Cloth_IdNo = " & Str(Val(Clo_IdNo))
            End If

            If Trim(txt_Filter_LotNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Roll_Packing_Code IN (select z1.Roll_Packing_Code from Roll_Packing_Details z1 where z1.Lot_No = '" & Trim(txt_Filter_LotNo.Text) & "') "
            End If
            If Trim(txt_Filter_PcsNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Roll_Packing_Code IN (select z2.Roll_Packing_Code from Roll_Packing_Details z2 where z2.Pcs_No = '" & Trim(txt_Filter_PcsNo.Text) & "') "
            End If
            If Trim(txt_Filter_RollNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Roll_Packing_Code IN (select z3.Roll_Packing_Code from Roll_Packing_Details z3 where z3.Roll_No = '" & Trim(txt_Filter_RollNo.Text) & "') "
            End If
            If Trim(txt_Filter_BuyerRefNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Roll_Packing_Code IN (select z4.Roll_Packing_Code from Roll_Packing_Details z4 where z4.Buyer_RefNo = '" & Trim(txt_Filter_BuyerRefNo.Text) & "') "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, c.Ledger_name from Roll_Packing_Head a INNER JOIN Cloth_Head b on a.cloth_idno <> 0 and a.cloth_idno = b.cloth_idno INNER JOIN Ledger_Head c on a.ledger_idno <> 0 and a.ledger_idno = c.ledger_idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Roll_Packing_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and a.Roll_Packing_Code LIKE '" & Trim(Pk_Condition) & "%' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Roll_Packing_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Roll_Packing_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Roll_Packing_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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

    Private Sub dtp_Filter_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_Fromdate.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True ': SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub


    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            dtp_Filter_ToDate.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_ToDate.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_Filter_Party.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, cbo_Filter_Party, txt_Filter_BuyerRefNo, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, txt_Filter_BuyerRefNo, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

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

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(11).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_Piece(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim CloIdNo As Integer, CloTypIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim StkOf_IdNo As Integer = 0
        Dim led_id As Integer = 0
        Dim Cnt_GrpIdNos As String = ""
        Dim Cnt_IdNo As Integer, Cnt_UndIdNo As Integer
        Dim Cnt_Cond As String = ""
        Dim BuyrOffrCondt As String = ""
        Dim PcsMtrs As Single = 0
        Dim vBaleDelvCd As String = ""
        Dim vLmIdNo As Long = 0
        Dim vLmNo As String = ""
        Dim Cmp_Name As String

        StkOf_IdNo = Common_Procedures.CommonLedger.Godown_Ac

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        CloIdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If CloIdNo = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        CloTypIdNo = Common_Procedures.ClothType_NameToIdNo(con, cbo_ClothType.Text)
        If CloTypIdNo = 0 Then
            MessageBox.Show("Invalid Cloth Type", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothType.Enabled And cbo_ClothType.Visible Then cbo_ClothType.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then
            MessageBox.Show("Invalid Folding", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Folding.Enabled And txt_Folding.Visible Then txt_Folding.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
            CompIDCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompIDCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
            End If
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Cnt_IdNo = CloIdNo

        Cnt_UndIdNo = Val(Cnt_IdNo)

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_head where Cloth_idno = " & Str(Val(Cnt_UndIdNo)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString) = False Then
                If Val(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString) <> 0 Then Cnt_UndIdNo = Val(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString)
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_head where Cloth_StockUnder_IdNo = " & Str(Val(Cnt_UndIdNo)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        Cnt_GrpIdNos = ""
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1
                Cnt_GrpIdNos = Trim(Cnt_GrpIdNos) & IIf(Trim(Cnt_GrpIdNos) <> "", ", ", "") & Trim(Val(Dt1.Rows(i).Item("Cloth_IdNo")))
            Next
        End If
        If Trim(Cnt_GrpIdNos) <> "" Then
            Cnt_GrpIdNos = "(" & Cnt_GrpIdNos & ")"
        Else
            Cnt_GrpIdNos = "(" & Trim(Val(Cnt_IdNo)) & ")"
        End If

        Cnt_Cond = "(a.Cloth_idno = " & Str(Cnt_IdNo) & " or a.Cloth_idno IN " & Trim(Cnt_GrpIdNos) & ")"


        With dgv_Selection
            chk_SelectAll.Checked = False
            .Rows.Clear()
            SNo = 0

            If CloTypIdNo = 1 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 IN (select z1.Roll_Code from Roll_Packing_Details z1 where z1.Roll_Packing_Code = '" & Trim(NewCode) & "') and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & " a.Folding = " & Str(Val(txt_Folding.Text)) & " and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type1, a.BuyerOffer_No_Type1, a.BuyerOffer_Party_PieceNo_Type1, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type1

                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type1_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type1_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type1_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type1").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)

                        .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type1").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type1").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type1").ToString
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            If Trim(.Rows(n).Cells(9).Value) = "" Then
                                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Piece_No").ToString
                            End If
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Roll_No_Type1").ToString

                        .Rows(n).Cells(11).Value = "1"  '--STS

                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type1").ToString)
                        Else
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("Type1_Meters").ToString)
                        End If
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type1").ToString)
                        .Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)
                        .Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type1").ToString)
                        .Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type1").ToString)
                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type1").ToString

                        .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type1").ToString
                        .Rows(n).Cells(22).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString


                        vBaleDelvCd = ""
                        If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString) = False Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString) <> "" Then
                                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString) & "'", con)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)
                                If Dt2.Rows.Count > 0 Then
                                    If IsDBNull(Dt2.Rows(0).Item("Delivery_Code").ToString) = False Then
                                        vBaleDelvCd = Dt2.Rows(0).Item("Delivery_Code").ToString
                                    End If
                                End If
                                Dt2.Clear()
                            End If
                        End If
                        .Rows(n).Cells(23).Value = vBaleDelvCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo
                        .Rows(n).Cells(25).Value = Dt1.Rows(i).Item("Roll_Tare_Weight").ToString
                        .Rows(n).Cells(26).Value = Dt1.Rows(i).Item("Roll_Gross_Weight").ToString


                        If Trim(vBaleDelvCd) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next
                        Else
                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    Next

                End If
                Dt1.Clear()

                BuyrOffrCondt = "(a.BuyerOffer_Code_Type1 = '')"
                If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                    BuyrOffrCondt = "(a.BuyerOffer_Code_Type1 <> '' and a.BuyerOffer_Code_Type1 IN  (SELECT tz1.Buyer_Offer_Code FROM Buyer_Offer_Head tz1 where tz1.Ledger_IdNo = " & Str(Val(led_id)) & " ) )"
                End If

                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.Cloth_Name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 = '' and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & BuyrOffrCondt & IIf(BuyrOffrCondt <> "", " and ", " ") & " a.Folding = " & Str(Val(txt_Folding.Text)) & " and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type1, a.BuyerOffer_No_Type1, a.BuyerOffer_Party_PieceNo_Type1, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type1

                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type1_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type1_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type1_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type1").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)




                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1007" Then

                            Da = New SqlClient.SqlDataAdapter("Select * from Company_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)), con)
                            Da.Fill(Dt3)


                            If Dt3.Rows.Count > 0 Then
                                If IsDBNull(Dt3.Rows(0).Item("Company_name").ToString) = False Then
                                    Cmp_Name = Trim(Dt3.Rows(0).Item("Company_name").ToString)
                                End If
                            End If

                            If (InStr(1, Trim(UCase(Cmp_Name)), "BHAGAVAN") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "TEXTILES") > 0) Or (InStr(1, Trim(UCase(Cmp_Name)), "RAJA") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "TEX") > 0) Then
                                .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                            Else
                                .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                            End If



                        End If


                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type1").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type1").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type1").ToString
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            If Trim(.Rows(n).Cells(9).Value) = "" Then
                                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Piece_No").ToString
                            End If
                        End If

                        .Rows(n).Cells(10).Value = ""  'ROLL NO

                        .Rows(n).Cells(11).Value = ""  'STS

                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type1").ToString)
                        Else
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("Type1_Meters").ToString)
                        End If

                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type1").ToString)
                        .Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)
                        .Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type1").ToString)
                        .Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type1").ToString)
                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type1").ToString

                        .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type1").ToString
                        .Rows(n).Cells(22).Value = ""
                        .Rows(n).Cells(23).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo
                        .Rows(n).Cells(25).Value = Dt1.Rows(i).Item("Roll_Tare_Weight").ToString
                        .Rows(n).Cells(26).Value = Dt1.Rows(i).Item("Roll_Gross_Weight").ToString

                    Next

                End If
                Dt1.Clear()

            End If

            If CloTypIdNo = 1 Or CloTypIdNo = 2 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2  IN (select z1.Roll_Code from Roll_Packing_Details z1 where z1.Roll_Packing_Code = '" & Trim(NewCode) & "') and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "   and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type2, a.BuyerOffer_No_Type2, a.BuyerOffer_Party_PieceNo_Type2, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type2

                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type2_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type2_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type2_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type2").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)

                        .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type2").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type2").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type2").ToString
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            If Trim(.Rows(n).Cells(9).Value) = "" Then
                                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Piece_No").ToString
                            End If
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Roll_No_Type2").ToString

                        .Rows(n).Cells(11).Value = "1"  '--STS

                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type2").ToString)
                        Else
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("Type2_Meters").ToString)
                        End If
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type2").ToString)
                        .Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)
                        .Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type2").ToString)
                        .Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type2").ToString)
                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type2").ToString

                        .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type2").ToString
                        .Rows(n).Cells(22).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString


                        vBaleDelvCd = ""
                        If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString) = False Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString) <> "" Then
                                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString) & "'", con)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)
                                If Dt2.Rows.Count > 0 Then
                                    If IsDBNull(Dt2.Rows(0).Item("Delivery_Code").ToString) = False Then
                                        vBaleDelvCd = Dt2.Rows(0).Item("Delivery_Code").ToString
                                    End If
                                End If
                                Dt2.Clear()
                            End If
                        End If
                        .Rows(n).Cells(23).Value = vBaleDelvCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo
                        .Rows(n).Cells(25).Value = Dt1.Rows(i).Item("Roll_Tare_Weight").ToString
                        .Rows(n).Cells(26).Value = Dt1.Rows(i).Item("Roll_Gross_Weight").ToString

                        If Trim(vBaleDelvCd) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next
                        Else
                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    Next

                End If
                Dt1.Clear()

                BuyrOffrCondt = "(a.BuyerOffer_Code_Type2 = '')"
                If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                    BuyrOffrCondt = "(a.BuyerOffer_Code_Type2 <> '' and a.BuyerOffer_Code_Type2 IN  (SELECT tz1.Buyer_Offer_Code FROM Buyer_Offer_Head tz1 where tz1.Ledger_IdNo = " & Str(Val(led_id)) & " ) )"
                End If

                Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 = ''  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & BuyrOffrCondt & IIf(BuyrOffrCondt <> "", " and ", " ") & " a.Folding = " & Str(Val(txt_Folding.Text)) & "  and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type2, a.BuyerOffer_No_Type2, a.BuyerOffer_Party_PieceNo_Type2, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type2

                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type2_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type2_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type2_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type2").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)

                        .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type2").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type2").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type2").ToString
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            If Trim(.Rows(n).Cells(9).Value) = "" Then
                                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Piece_No").ToString
                            End If
                        End If
                        .Rows(n).Cells(10).Value = ""  'ROLL NO

                        .Rows(n).Cells(11).Value = ""  'STS

                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type2").ToString)
                        Else
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("Type2_Meters").ToString)
                        End If
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type2").ToString)
                        .Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)
                        .Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type2").ToString)
                        .Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type2").ToString)
                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type2").ToString

                        .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type2").ToString
                        .Rows(n).Cells(22).Value = ""
                        .Rows(n).Cells(23).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo
                        .Rows(n).Cells(25).Value = Dt1.Rows(i).Item("Roll_Tare_Weight").ToString
                        .Rows(n).Cells(26).Value = Dt1.Rows(i).Item("Roll_Gross_Weight").ToString

                    Next

                End If
                Dt1.Clear()
            End If

            If CloTypIdNo = 3 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno   LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3  IN (select z1.Roll_Code from Roll_Packing_Details z1 where z1.Roll_Packing_Code = '" & Trim(NewCode) & "') and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type3, a.BuyerOffer_No_Type3, a.BuyerOffer_Party_PieceNo_Type3, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type3

                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type3_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type3_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type3_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type3").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)

                        .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type3").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type3").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type3").ToString
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            If Trim(.Rows(n).Cells(9).Value) = "" Then
                                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Piece_No").ToString
                            End If
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Roll_No_Type3").ToString

                        .Rows(n).Cells(11).Value = "1"  '--STS

                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type3").ToString)
                        Else
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("Type3_Meters").ToString)
                        End If
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type3").ToString)
                        .Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)
                        .Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type3").ToString)
                        .Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type3").ToString)
                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type3").ToString

                        .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type3").ToString
                        .Rows(n).Cells(22).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString


                        vBaleDelvCd = ""
                        If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString) = False Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString) <> "" Then
                                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString) & "'", con)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)
                                If Dt2.Rows.Count > 0 Then
                                    If IsDBNull(Dt2.Rows(0).Item("Delivery_Code").ToString) = False Then
                                        vBaleDelvCd = Dt2.Rows(0).Item("Delivery_Code").ToString
                                    End If
                                End If
                                Dt2.Clear()
                            End If
                        End If
                        .Rows(n).Cells(23).Value = vBaleDelvCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo
                        .Rows(n).Cells(25).Value = Dt1.Rows(i).Item("Roll_Tare_Weight").ToString
                        .Rows(n).Cells(26).Value = Dt1.Rows(i).Item("Roll_Gross_Weight").ToString

                        If Trim(vBaleDelvCd) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next
                        Else
                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    Next

                End If
                Dt1.Clear()

                BuyrOffrCondt = "(a.BuyerOffer_Code_Type3 = '')"
                If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                    BuyrOffrCondt = "(a.BuyerOffer_Code_Type3 <> '' and a.BuyerOffer_Code_Type3 IN  (SELECT tz1.Buyer_Offer_Code FROM Buyer_Offer_Head tz1 where tz1.Ledger_IdNo = " & Str(Val(led_id)) & " ) )"
                End If

                Da = New SqlClient.SqlDataAdapter("select a.* ,C.Ledger_Name, d.Cloth_Name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno   LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Cloth_Head d ON a.cloth_IdNo <> 0 and a.cloth_IdNo = d.cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 = ''  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & BuyrOffrCondt & IIf(BuyrOffrCondt <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type3, a.BuyerOffer_No_Type3, a.BuyerOffer_Party_PieceNo_Type3, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type3

                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type3_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type3_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type3_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type3").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)

                        .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type3").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type3").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type3").ToString
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            If Trim(.Rows(n).Cells(9).Value) = "" Then
                                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Piece_No").ToString
                            End If
                        End If
                        .Rows(n).Cells(10).Value = ""  'ROLL NO

                        .Rows(n).Cells(11).Value = ""  'STS

                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type3").ToString)
                        Else
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("Type3_Meters").ToString)
                        End If
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type3").ToString)
                        .Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)
                        .Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type3").ToString)
                        .Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type3").ToString)
                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type3").ToString

                        .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type3").ToString
                        .Rows(n).Cells(22).Value = ""
                        .Rows(n).Cells(23).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo
                        .Rows(n).Cells(25).Value = Dt1.Rows(i).Item("Roll_Tare_Weight").ToString
                        .Rows(n).Cells(26).Value = Dt1.Rows(i).Item("Roll_Gross_Weight").ToString

                    Next

                End If
                Dt1.Clear()
            End If

            If CloTypIdNo = 4 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4  IN (select z1.Roll_Code from Roll_Packing_Details z1 where z1.Roll_Packing_Code = '" & Trim(NewCode) & "') and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type4, a.BuyerOffer_No_Type4, a.BuyerOffer_Party_PieceNo_Type4, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type4

                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type4_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type4_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type4_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type4").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)

                        .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type4").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type3").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type4").ToString
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            If Trim(.Rows(n).Cells(9).Value) = "" Then
                                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Piece_No").ToString
                            End If
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Roll_No_Type4").ToString

                        .Rows(n).Cells(11).Value = "1"  '--STS

                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type4").ToString)
                        Else
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("Type4_Meters").ToString)
                        End If
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type4").ToString)
                        .Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)
                        .Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type4").ToString)
                        .Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type4").ToString)
                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type4").ToString

                        .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type4").ToString
                        .Rows(n).Cells(22).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString


                        vBaleDelvCd = ""
                        If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString) = False Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString) <> "" Then
                                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString) & "'", con)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)
                                If Dt2.Rows.Count > 0 Then
                                    If IsDBNull(Dt2.Rows(0).Item("Delivery_Code").ToString) = False Then
                                        vBaleDelvCd = Dt2.Rows(0).Item("Delivery_Code").ToString
                                    End If
                                End If
                                Dt2.Clear()
                            End If
                        End If
                        .Rows(n).Cells(23).Value = vBaleDelvCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo
                        .Rows(n).Cells(25).Value = Dt1.Rows(i).Item("Roll_Tare_Weight").ToString
                        .Rows(n).Cells(26).Value = Dt1.Rows(i).Item("Roll_Gross_Weight").ToString

                        If Trim(vBaleDelvCd) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next
                        Else
                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    Next

                End If
                Dt1.Clear()

                BuyrOffrCondt = "(a.BuyerOffer_Code_Type4 = '')"
                If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                    BuyrOffrCondt = "(a.BuyerOffer_Code_Type4 <> '' and a.BuyerOffer_Code_Type4 IN  (SELECT tz1.Buyer_Offer_Code FROM Buyer_Offer_Head tz1 where tz1.Ledger_IdNo = " & Str(Val(led_id)) & " ) )"
                End If

                Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno   LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 = ''  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & BuyrOffrCondt & IIf(BuyrOffrCondt <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type4, a.BuyerOffer_No_Type4, a.BuyerOffer_Party_PieceNo_Type4, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type4

                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type4_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type4_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type4_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type4").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)

                        .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type4").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type4").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type4").ToString
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            If Trim(.Rows(n).Cells(9).Value) = "" Then
                                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Piece_No").ToString
                            End If
                        End If
                        .Rows(n).Cells(10).Value = ""  'ROLL NO

                        .Rows(n).Cells(11).Value = ""  'STS

                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type4").ToString)
                        Else
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("Type4_Meters").ToString)
                        End If
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type4").ToString)
                        .Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)
                        .Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type4").ToString)
                        .Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type4").ToString)
                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type4").ToString

                        .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type4").ToString
                        .Rows(n).Cells(22).Value = ""
                        .Rows(n).Cells(23).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo
                        .Rows(n).Cells(25).Value = Dt1.Rows(i).Item("Roll_Tare_Weight").ToString
                        .Rows(n).Cells(26).Value = Dt1.Rows(i).Item("Roll_Gross_Weight").ToString

                    Next

                End If
                Dt1.Clear()
            End If
            If CloTypIdNo = 5 Then

                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5  IN (select z1.Roll_Code from Roll_Packing_Details z1 where z1.Roll_Packing_Code = '" & Trim(NewCode) & "') and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type5, a.BuyerOffer_No_Type5, a.BuyerOffer_Party_PieceNo_Type5, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type5

                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type5_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type5_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type5_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type5").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)

                        .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type5").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type5").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type5").ToString
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            If Trim(.Rows(n).Cells(9).Value) = "" Then
                                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Piece_No").ToString
                            End If
                        End If
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Roll_No_Type5").ToString

                        .Rows(n).Cells(11).Value = "1"  '--STS

                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type5").ToString)
                        Else
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("Type5_Meters").ToString)
                        End If
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type5").ToString)
                        .Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)
                        .Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type5").ToString)
                        .Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type5").ToString)
                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type5").ToString

                        .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type5").ToString
                        .Rows(n).Cells(22).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString


                        vBaleDelvCd = ""
                        If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString) = False Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString) <> "" Then
                                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString) & "'", con)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)
                                If Dt2.Rows.Count > 0 Then
                                    If IsDBNull(Dt2.Rows(0).Item("Delivery_Code").ToString) = False Then
                                        vBaleDelvCd = Dt2.Rows(0).Item("Delivery_Code").ToString
                                    End If
                                End If
                                Dt2.Clear()
                            End If
                        End If
                        .Rows(n).Cells(23).Value = vBaleDelvCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo
                        .Rows(n).Cells(25).Value = Dt1.Rows(i).Item("Roll_Tare_Weight").ToString
                        .Rows(n).Cells(26).Value = Dt1.Rows(i).Item("Roll_Gross_Weight").ToString

                        If Trim(vBaleDelvCd) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next
                        Else
                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    Next

                End If
                Dt1.Clear()

                BuyrOffrCondt = "(a.BuyerOffer_Code_Type5 = '')"
                If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                    BuyrOffrCondt = "(a.BuyerOffer_Code_Type5 <> '' and a.BuyerOffer_Code_Type5 IN  (SELECT tz1.Buyer_Offer_Code FROM Buyer_Offer_Head tz1 where tz1.Ledger_IdNo = " & Str(Val(led_id)) & " ) )"
                End If

                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 = '' and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & BuyrOffrCondt & IIf(BuyrOffrCondt <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type5, a.BuyerOffer_No_Type5, a.BuyerOffer_Party_PieceNo_Type5, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type5

                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Type5_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type5_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type5_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type5").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)

                        .Rows(n).Cells(5).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type5").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type5").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type5").ToString
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                            If Trim(.Rows(n).Cells(9).Value) = "" Then
                                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Piece_No").ToString
                            End If
                        End If
                        .Rows(n).Cells(10).Value = ""  'ROLL NO

                        .Rows(n).Cells(11).Value = ""  'STS

                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type5").ToString)
                        Else
                            .Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("Type5_Meters").ToString)
                        End If
                        .Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type4").ToString)
                        .Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)
                        .Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type5").ToString)
                        .Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type5").ToString)
                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type5").ToString

                        .Rows(n).Cells(21).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type5").ToString
                        .Rows(n).Cells(22).Value = ""
                        .Rows(n).Cells(23).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(24).Value = vLmNo
                        .Rows(n).Cells(25).Value = Dt1.Rows(i).Item("Roll_Tare_Weight").ToString
                        .Rows(n).Cells(26).Value = Dt1.Rows(i).Item("Roll_Gross_Weight").ToString

                    Next

                End If
                Dt1.Clear()

            End If
        End With

        pnl_Selection.Visible = True
        Pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
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

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer = 0
        Dim S As String = ""

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                If Trim(dgv_Selection.Rows(RwIndx).Cells(23).Value) = "" Then

                    .Rows(RwIndx).Cells(11).Value = (Val(.Rows(RwIndx).Cells(11).Value) + 1) Mod 2

                    If Val(.Rows(RwIndx).Cells(11).Value) = 1 Then

                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                        Next

                    Else

                        .Rows(RwIndx).Cells(11).Value = ""

                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                        Next

                    End If

                Else
                    S = ""
                    If Trim(dgv_Selection.Rows(RwIndx).Cells(23).Value) <> "" Then
                        S = "Already this rolls was baled/delivered = " & Trim(dgv_Selection.Rows(RwIndx).Cells(23).Value)
                    End If

                    MessageBox.Show(S, "DOES NOT DE-SELECT THIS ROLL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    Exit Sub

                End If

            End If

        End With

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Piece_Selection()
    End Sub

    Private Sub Piece_Selection()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim vMAX_RLNO As String = ""
        Dim vLEDID As Integer = 0
        Dim vLEDCONDT As String = ""

        Pnl_Back.Enabled = True
        dgv_Details.Rows.Clear()


        vMAX_RLNO = ""
        vLEDID = 0
        vLEDCONDT = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then

            'cmd.CommandText = "Insert into Packing_Slip_Head ( Roll_Packing_Code     ,       Packing_Slip_Code     ,               Company_IdNo       ,           Packing_Slip_No               ,                               for_OrderBy                                        , Packing_Slip_Date,                                             Ledger_IdNo    ,    Roll_Packing_Party_IdNo,     Pcs_BufferOffer_Type     ,             Cloth_IdNo   ,            ClothType_IdNo  ,              Bale_Bundle            ,                  Folding           , Total_Pcs ,                      Total_Meters          ,                      Total_Weight          ,                      Net_Weight          ,                      Tare_Weight          ,                      Gross_Weight          ,               Note           ,             User_IdNo                    ) " &
            '                            "          Values            ('" & Trim(NewCode) & "', '" & Trim(vNewRollCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(.Rows(i).Cells(16).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(16).Value))) & ",      @EntryDate  ,  " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(led_id)) & "  , '" & Trim(cbo_Type.Text) & "', " & Str(Val(Clth_ID)) & ", " & Str(Val(Clthty_ID)) & ", '" & Trim(cbo_Bale_Bundle.Text) & "',  " & Str(Val(txt_Folding.Text)) & ",     1     , " & Str(Val(.Rows(i).Cells(10).Value)) & " , " & Str(Val(.Rows(i).Cells(5).Value)) & "  , " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(24).Value)) & ", " & Str(Val(.Rows(i).Cells(25).Value)) & " , '" & Trim(txt_Note.Text) & "', " & Val(Common_Procedures.User.IdNo) & " ) "
            'Nr2 = cmd.ExecuteNonQuery()


            vLEDID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

            If vLEDID = 178 Or vLEDID = 257 Then
                vLEDCONDT = " and (Roll_Packing_Party_IdNo = 178 or Roll_Packing_Party_IdNo = 257)"
            Else
                vLEDCONDT = " and (Roll_Packing_Party_IdNo <> 178 and Roll_Packing_Party_IdNo <> 257)"
            End If


            vMAX_RLNO = ""
            Da = New SqlClient.SqlDataAdapter("select top 1 a.Packing_Slip_RefNo from Packing_Slip_Head a where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "'  " & vLEDCONDT & " Order by a.for_Orderby desc, a.Packing_Slip_RefNo desc, a.Packing_Slip_No desc", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    vMAX_RLNO = Dt1.Rows(0)(0).ToString
                End If
            End If
            Dt1.Clear()

            vMAX_RLNO = Replace(vMAX_RLNO, "MR ", "")
            vMAX_RLNO = Replace(vMAX_RLNO, "MR-", "")
            vMAX_RLNO = Replace(vMAX_RLNO, "MR", "")
            vMAX_RLNO = Replace(vMAX_RLNO, "MF ", "")
            vMAX_RLNO = Replace(vMAX_RLNO, "MF-", "")
            vMAX_RLNO = Replace(vMAX_RLNO, "MF", "")

            'vMAX_RLNO = Val(vMAX_RLNO) + 1
            'vMAX_RLNO = "MR" & vMAX_RLNO

        End If

        sno = 0
        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(11).Value) = 1 Then

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = sno
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(4).Value
                If Val(dgv_Selection.Rows(i).Cells(5).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(5).Value
                End If
                If Val(dgv_Selection.Rows(i).Cells(6).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(8).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(9).Value
                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(15).Value
                dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(16).Value
                dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(17).Value
                dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(18).Value
                If Val(dgv_Details.Rows(n).Cells(13).Value) = 0 Then dgv_Details.Rows(n).Cells(13).Value = ""
                dgv_Details.Rows(n).Cells(14).Value = dgv_Selection.Rows(i).Cells(19).Value
                If Val(dgv_Details.Rows(n).Cells(14).Value) = 0 Then dgv_Details.Rows(n).Cells(14).Value = ""
                dgv_Details.Rows(n).Cells(15).Value = dgv_Selection.Rows(i).Cells(20).Value

                dgv_Details.Rows(n).Cells(16).Value = dgv_Selection.Rows(i).Cells(10).Value

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then ' --mof
                    If New_Entry = True Then
                        If Trim(dgv_Details.Rows(n).Cells(16).Value) = "" Then
                            vMAX_RLNO = Val(vMAX_RLNO) + 1
                            If vLEDID = 178 Or vLEDID = 257 Then
                                dgv_Details.Rows(n).Cells(16).Value = "MF" & vMAX_RLNO
                            Else
                                dgv_Details.Rows(n).Cells(16).Value = "MR" & vMAX_RLNO
                            End If
                        End If
                    End If
                End If

                dgv_Details.Rows(n).Cells(17).Value = dgv_Selection.Rows(i).Cells(12).Value
                dgv_Details.Rows(n).Cells(18).Value = dgv_Selection.Rows(i).Cells(13).Value
                dgv_Details.Rows(n).Cells(19).Value = dgv_Selection.Rows(i).Cells(14).Value
                dgv_Details.Rows(n).Cells(20).Value = dgv_Selection.Rows(i).Cells(21).Value
                dgv_Details.Rows(n).Cells(21).Value = dgv_Selection.Rows(i).Cells(22).Value
                dgv_Details.Rows(n).Cells(22).Value = dgv_Selection.Rows(i).Cells(23).Value
                dgv_Details.Rows(n).Cells(23).Value = dgv_Selection.Rows(i).Cells(24).Value
                dgv_Details.Rows(n).Cells(24).Value = dgv_Selection.Rows(i).Cells(25).Value
                dgv_Details.Rows(n).Cells(25).Value = dgv_Selection.Rows(i).Cells(26).Value

                If Trim(dgv_Details.Rows(n).Cells(22).Value) <> "" Then
                    For j = 0 To dgv_Details.ColumnCount - 1
                        dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                        dgv_Details.Rows(n).Cells(j).Style.ForeColor = Color.Red
                    Next
                End If

            End If

        Next i

        Total_Calculation()

        Pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(16)
            dgv_Details.CurrentCell.Selected = True
        Else
            If txt_Note.Enabled And txt_Note.Visible Then txt_Note.Focus() Else msk_date.Focus()
        End If

    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        Pnl_Back.Enabled = True
        pnl_Print.Visible = False
        Prn_BarcodeSticker = False
    End Sub
    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        'Dim ps As Printing.PaperSize
        Dim NewCode As String

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.ClothSales_Roll_Packing_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Roll_Packing_Head a Where a.Roll_Packing_Code = '" & Trim(NewCode) & "'", con)
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
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then ' --- MANI OMEGA FABRICES

                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument3.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument3.PrinterSettings = PrintDialog1.PrinterSettings
                            PrintDocument3.Print()
                        End If

                    Else
                        PrintDocument3.Print()

                    End If

                Else

                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                            PrintDocument1.Print()
                        End If

                    Else
                        PrintDocument1.Print()

                    End If
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then ' --- MANI OMEGA FABRICES
                    ppd.Document = PrintDocument3
                Else
                    ppd.Document = PrintDocument1
                End If

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
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
    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim vSNO As Integer = 0
        Dim Mtrs_Flding As Integer


        prn_HdDt.Clear()

        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 0
        prn_DetIndx = 1
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_DetSNo = 0
        prn_Count = 1

        Erase prn_DetAr
        Erase prn_HdAr

        Erase prn_DetAr1

        prn_HdAr = New String(100, 10) {}

        prn_DetAr = New String(100, 50, 10) {}

        prn_DetAr1 = New String(1000, 10) {}

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            vSNO = 0
            da1 = New SqlClient.SqlDataAdapter("select a.*, tZ.*, c.Cloth_Name ,  E.* from Roll_Packing_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN LEDGER_Head E ON A.Ledger_IdNo = E.Ledger_IdNo Where a.Roll_Packing_Code = '" & Trim(NewCode) & "' Order by a.Roll_Packing_Date, a.for_OrderBy, a.Roll_Packing_No, a.Roll_Packing_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Roll_Packing_Details a where a.Roll_Packing_Code = '" & Trim(NewCode) & "'  order by a.Roll_No, a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)


                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Then  ' ----------------Rajeswari Weaving

                    For i As Integer = 0 To prn_DetDt.Rows.Count - 1

                        If Val(prn_DetDt.Rows(i).Item("Meters").ToString) <> 0 Then


                            prn_DetMxIndx = prn_DetMxIndx + 1
                            vSNO = vSNO + 1

                            prn_DetAr1(prn_DetMxIndx, 1) = vSNO
                            prn_DetAr1(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Roll_No").ToString)
                            prn_DetAr1(prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), " #######0.00")

                            Mtrs_Flding = Format(Val(prn_HdDt.Rows(0).Item("Folding").ToString) * prn_DetAr1(prn_DetMxIndx, 3) / 100, " #######0.00")
                            'Mtrs_Flding = Common_Procedures.Meter_RoundOff(Val(Mtrs_Flding))

                            prn_DetAr1(prn_DetMxIndx, 4) = Val(Mtrs_Flding)



                        End If

                    Next

                End If


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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Then ' ------------Rajeswari Weaving Somanur
            Printing_Format2_1033(e)
        Else
            Printing_Format1(e)
        End If


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
        Dim npCS As Integer = "1"

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

        NoofItems_PerPage = 27

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 75 : ClAr(3) = 70 : ClAr(4) = 70 : ClAr(5) = 45 : ClAr(6) = 90 : ClAr(7) = 95 : ClAr(8) = 90 : ClAr(9) = 90
        ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 1

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Roll_No").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(prn_DetIndx).Item("Party_PieceNo").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(prn_DetIndx).Item("Buyer_RefNo").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(npCS), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Points").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Point_Per_PassMeter").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(prn_DetIndx).Item("Grade").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), " #######0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Grade").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, pFont)

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
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim Cmp_Add As String = ""
        Dim C1 As Single, W1, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String

        PageNo = PageNo + 1

        CurY = TMargin + 30

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from ClothSales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        prn_Count = prn_Count + 1

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "ROLL PACKING", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)
        '    'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK_2, Drawing.Image), LMargin + 20, CurY, 115, 80)
        '    'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 75, 75)
        'End If

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
        W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
        W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
        S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "BUYER  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Roll_Packing_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Roll_Packing_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
        'End If

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        Try

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("cloth_nAME").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ROLL NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTY", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS.NO", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BUFER", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "REF.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO OF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, " POINTS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "POINTS/10", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0 LIN MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GRADE ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim p1Font As Font
        Dim W1, W2 As Single
        Dim C1 As Single

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString("Empty Gunnies  :", pFont).Width
        W2 = e.Graphics.MeasureString("Empty Cones  :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 5, CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Points").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Passed_Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), " #######0.000"), PageWidth - 10, CurY, 1, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Points").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(2))


        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Remarks : " & prn_HdDt.Rows(0).Item("Note").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString), PageWidth - 15, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_date, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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

    Private Sub txt_PcsSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PcsSelction.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
        If (e.KeyValue = 38) Then txt_LotSelction.Focus()

    End Sub

    Private Sub txt_PcsSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsSelction.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_PcsSelction.Text) <> "" Or Trim(txt_PcsSelction.Text) <> "" Then
                btn_lot_Pcs_selection_Click(sender, e)

            Else
                If dgv_Selection.Rows.Count > 0 Then
                    dgv_Selection.Focus()
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                    dgv_Selection.CurrentCell.Selected = True
                End If

            End If

        End If
    End Sub

    Private Sub txt_LotSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LotSelction.KeyDown
        If (e.KeyValue = 40) Then
            txt_PcsSelction.Focus()
        End If
    End Sub

    Private Sub txt_LotSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LotSelction.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_PcsSelction.Focus()
        End If
    End Sub

    Private Sub btn_lot_Pcs_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_lot_Pcs_selection.Click
        Dim LtNo As String
        Dim PcsNo As String
        Dim i As Integer

        If Trim(txt_LotSelction.Text) <> "" Or Trim(txt_PcsSelction.Text) <> "" Then

            LtNo = Trim(txt_LotSelction.Text)
            PcsNo = Trim(txt_PcsSelction.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) And Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                    Call Select_Piece(i)
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 11 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 10
                    Exit For
                ElseIf Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                    Call Select_Piece(i)
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 11 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 10
                    Exit For
                ElseIf Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) Then
                    Call Select_Piece(i)
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 11 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 10
                    Exit For

                End If
            Next

            txt_LotSelction.Text = ""
            txt_PcsSelction.Text = ""
            If txt_LotSelction.Enabled = True Then txt_LotSelction.Focus()

        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_date.Text = Date.Today
        End If
        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
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

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_PartyName, cbo_Cloth, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Cloth, "", "", "", "")
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .CurrentCell.RowIndex >= 0 And .CurrentCell.ColumnIndex >= 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_Party_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Party.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Party_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Party.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Party, dtp_Filter_ToDate, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Party_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Party.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Party, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub txt_Filter_RollNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Filter_RollNo.KeyDown
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Filter_RollNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_RollNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            btn_Filter_Show.Focus()
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub btn_Filter_Close_KeyPress(sender As Object, e As KeyPressEventArgs) Handles btn_Filter_Close.KeyPress

    End Sub

    Private Sub dgv_Details_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellDoubleClick

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        With dgv_Details

            If .Visible Then

                If IsNothing(.CurrentCell) Then Exit Sub

                If .CurrentCell.ColumnIndex = 16 Then

                    If Val(.Rows(.CurrentCell.RowIndex).Cells(10).Value) > 0 Then
                        MessageBox.Show("INV No. / DC No.  : " & Trim(.Rows(.CurrentCell.RowIndex).Cells(22).Value), "ROLL DELIVERED STATUS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                    End If

                End If

            End If

        End With

    End Sub

    Private Sub Printing_Format2_1033(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim npCS As Integer = "1"

        Dim Mtrs_Flding As Integer
        Dim Tot_Mtrs_Flding As Integer


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
            .Right = 40 '30
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

        NoofItems_PerPage = 25 '27

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClAr(1) = Val(45) : ClAr(2) = 90 : ClAr(3) = 115 : ClAr(4) = 105 : ClAr(5) = 95 : ClAr(6) = 95 : ClAr(7) = 115
        ClAr(8) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7))



        If Val(txt_Folding.Text) = 0 Or Val(txt_Folding.Text) = 100 Then

            ClAr(1) = ClAr(1) + ClAr(4) / 3
            ClAr(2) = ClAr(2) + ClAr(4) / 3
            ClAr(3) = ClAr(3) + ClAr(4) / 3
            ClAr(4) = 0

            ClAr(5) = ClAr(6) + ClAr(8) / 3
            ClAr(6) = ClAr(6) + ClAr(8) / 3
            ClAr(7) = ClAr(7) + ClAr(8) / 3
            ClAr(8) = 0

        End If





        'vS1 = ClAr(1)
        'vS2 = ClAr(1) + ClAr(2)
        'vS3 = ClAr(1) + ClAr(2) + ClAr(3)
        'vS4 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        'vS5 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        'vS6 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
        'vS7 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
        'vS8 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8)





        'ClAr(1) = Val(90) : ClAr(2) = 130 : ClAr(3) = 170 : ClAr(4) = 190
        'ClAr(5) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4))






        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader_1033(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetMxIndx > 0 Then

                    Do While prn_DetIndx <= prn_DetMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                            If prn_DetIndx < prn_DetMxIndx Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format2_PageFooter_1033(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False, Tot_Mtrs_Flding)

                                e.HasMorePages = True

                            Else


                                Printing_Format2_PageFooter_1033(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True, Tot_Mtrs_Flding)

                                e.HasMorePages = False

                            End If

                            Return

                        End If

                        If Trim(prn_DetAr1(prn_DetIndx, 2)) <> "" Or Trim(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 2)) <> "" Then

                            CurY = CurY + TxtHgt

                            If Val(prn_DetAr1(prn_DetIndx, 3)) <> 0 Then

                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_DetIndx, 2)), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr1(prn_DetIndx, 3)), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)

                                If ClAr(4) > 0 Then

                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr1(prn_DetIndx, 4)), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

                                End If

                                Tot_Mtrs_Flding = Val(Tot_Mtrs_Flding) + Val(prn_DetAr1(prn_DetIndx, 4))

                                End If

                                If Val(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 3)) <> 0 Then

                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 3)), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                                If ClAr(4) > 0 Then

                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 4)), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                                End If

                                Tot_Mtrs_Flding = Val(Tot_Mtrs_Flding) + Val(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 4))

                                End If



                            End If


                        NoofDets = NoofDets + 1
                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format2_PageFooter_1033(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True, Tot_Mtrs_Flding)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader_1033(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim Cmp_Add As String = ""
        Dim C1 As Single, W1, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String

        PageNo = PageNo + 1

        CurY = TMargin + 30

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from ClothSales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        prn_Count = prn_Count + 1

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "ROLL PACKING", LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt + 15

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        'p1Font = New Font("Calibri", 15, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "ROLL PACKING", LMargin, CurY, 2, PrintWidth, p1Font)

        'CurY = CurY + TxtHgt
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
        W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
        S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "BUYER  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Roll_Packing_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Roll_Packing_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
        'End If

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        Try

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "QUALITY : ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("cloth_nAME").ToString, LMargin + W1, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "FOLDING % : " & prn_HdDt.Rows(0).Item("Folding").ToString, PageWidth - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "FOLDING % : " & prn_HdDt.Rows(0).Item("Folding").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ROLL NO", LMargin + ClAr(1) + 10, CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + 10, CurY - 10, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(" & Format(Val(prn_HdDt.Rows(0).Item("Folding").ToString), " #######0") & "CM )", LMargin + ClAr(1) + ClAr(2) + 10, CurY + 5, 2, ClAr(3), pFont)

            If ClAr(4) > 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY - 10, 2, ClAr(4), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(100CM)", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 5, 2, ClAr(4), pFont)

            End If
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ROLL NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY - 10, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(" & Format(Val(prn_HdDt.Rows(0).Item("Folding").ToString), " #######0") & "CM )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 5, 2, ClAr(7), pFont)

            If ClAr(8) > 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY - 10, 2, ClAr(8), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(100CM)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + 5, 2, ClAr(8), pFont)

            End If

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            LnAr(3) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter_1033(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal Tot_Mtrs_Flding As Integer)
        Dim i As Integer
        Dim p1Font As Font
        Dim W1, W2 As Single
        Dim C1 As Single

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString("Empty Gunnies  :", pFont).Width
        W2 = e.Graphics.MeasureString("Empty Cones  :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(2))

        CurY = CurY + TxtHgt - 10

        If is_LastPage = True Then

            If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) Then
                Common_Procedures.Print_To_PrintDocument(e, "Total Rolls : " & Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Total Meters (" & Format(Val(prn_HdDt.Rows(0).Item("Folding").ToString), " #######0") & "CM ) : " & Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)


            End If

            If ClAr(8) > 0 Then
                If Val(Tot_Mtrs_Flding) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Total Meters (100Cm) : " & Format(Val(Tot_Mtrs_Flding), " #######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 1, 0, pFont)
                End If

            End If
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Remarks : " & prn_HdDt.Rows(0).Item("Note").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString), PageWidth - 15, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    'Private Sub dtp_PoNo_Date_ValueChanged(sender As Object, e As EventArgs) Handles dtp_Roll_Packing_Po_Date.ValueChanged
    '    msk_Roll_Packing_Po_Date.Text = dtp_Roll_Packing_Po_Date.Text
    'End Sub

    Private Sub dtp_Roll_Packing_Po_Date_TextChanged(sender As Object, e As EventArgs) Handles dtp_Roll_Packing_Po_Date.TextChanged
        If IsDate(dtp_Roll_Packing_Po_Date.Text) = True Then

            msk_Roll_Packing_Po_Date.Text = dtp_Roll_Packing_Po_Date.Text

        End If


    End Sub

    'Private Sub dtp_Roll_Packing_Po_Date_KeyUp(sender As Object, e As KeyEventArgs) Handles dtp_Roll_Packing_Po_Date.KeyUp
    '    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
    '        dtp_Roll_Packing_Po_Date.Text = Date.Today
    '    End If
    'End Sub

    'Private Sub dtp_Roll_Packing_Po_Date_Enter(sender As Object, e As EventArgs) Handles dtp_Roll_Packing_Po_Date.Enter
    '    msk_Roll_Packing_Po_Date.Focus()
    '    msk_Roll_Packing_Po_Date.SelectionStart = 0
    'End Sub

    Private Sub msk_Roll_Packing_Po_Date_KeyUp(sender As Object, e As KeyEventArgs) Handles msk_Roll_Packing_Po_Date.KeyUp


        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_Roll_Packing_Po_Date.Text = Date.Today
        End If
        If IsDate(msk_Roll_Packing_Po_Date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_Roll_Packing_Po_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_Roll_Packing_Po_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
        End If
        'If e.KeyCode = 46 Or e.KeyCode = 8 Then

        '    Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskRollPckngPoText, vmskRollPckngPoStrt)
        'End If


    End Sub

    Private Sub msk_Roll_Packing_Po_Date_KeyDown(sender As Object, e As KeyEventArgs) Handles msk_Roll_Packing_Po_Date.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        vmskRollPckngPoText = ""
        vmskRollPckngPoStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskRollPckngPoText = msk_Roll_Packing_Po_Date.Text
            vmskRollPckngPoStrt = msk_Roll_Packing_Po_Date.SelectionStart
        End If

        If e.KeyCode = 38 Then

            txt_Roll_Packing_PoNo.Focus()

        ElseIf e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If

    End Sub

    Private Sub PrintDocument3_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument3.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim sno As Integer = 0
        Dim vENTSELCTYPE As String = ""
        Dim vwgtmtr As String = 0
        Dim vLMNO As String = ""
        Dim vDUP_PACKSLIPNO As String = ""
        Dim vCLONAME As String
        Dim vDUP_ORDNO As String = ""

        sno = 0

        prn_HdDt.Clear()
        prn_HdDt2.Clear()
        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 0
        prn_DetIndx = 0
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_DetMxIndx1 = 0
        prn_Count = 1
        'prn_TotalBales = 0
        prn_TotalPcs = 0
        prn_TotalMtrs = 0
        prn_TotalWgt = 0
        prn_TotaGrslWgt = 0

        prn_PACKINGLISTNO = ""
        'vPRN_ClothSales_Invoice_No = ""
        'vPRN_ClothSales_Invoice_Date = ""
        'vPRN_PartyName = ""
        'vPRN_PartyCityName = ""
        'vPRN_DeliveryTo_PartyName = ""
        'vPRN_Vechile_No = ""
        'vPRN_Party_OrderNo = ""
        'vPRN_Party_OrderDate = ""
        'vPRN_Cloth_name = ""
        'vPRN_Pack_Type_Name = ""
        'vPRN_Packing_SlipNo = ""
        'vPRN_Weight_Column_Status = False
        vPRN_FOLDINGPERC = 100

        prn_PL_Tot_Rls = 0
        prn_PL_Tot_Pcs = 0
        prn_PL_Tot_Mtr = 0
        prn_PL_Tot_GrsWgt = 0
        prn_PL_Tot_NetWgt = 0
        prn_TotalGrams = 0

        Erase prn_HdAr
        prn_HdAr = New String(1000, 20) {}

        Erase prn_HdAr2
        prn_HdAr2 = New String(1000, 20) {}

        Erase prn_DetAr
        prn_DetAr = New String(1000, 1000, 10) {}

        Erase prn_DetAr1
        prn_DetAr1 = New String(1000, 10) {}

        vDUP_PACKSLIPNO = ""
        vCLONAME = ""

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try


            da2 = New SqlClient.SqlDataAdapter("select a.*, tZ.* from Roll_Packing_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno LEFT OUTER JOIN LEDGER_Head E ON A.Ledger_IdNo = E.Ledger_IdNo Where a.Roll_Packing_Code = '" & Trim(NewCode) & "' Order by a.Roll_Packing_Date, a.for_OrderBy, a.Roll_Packing_No, a.Roll_Packing_Code", con)
            prn_HdDt2 = New DataTable
            da2.Fill(prn_HdDt2)

            If prn_HdDt2.Rows.Count > 0 Then

                da1 = New SqlClient.SqlDataAdapter("select a.Packing_Slip_No,a.Gross_Weight,a.Net_Weight,a.cloth_idno, a.Total_Pcs as Pak_Pcs, a.Packing_Slip_Code, a.Total_Weight, a.Total_Meters as Pak_Mtrs,a.folding, tZ.*, tc.*, tL.*, 'L1' as loom_number " &
                                                   " from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno  " &
                                                   " INNER JOIN Cloth_Head tC ON tC.Cloth_IdNo = a.Cloth_IdNo   " &
                                                   " INNER JOIN Ledger_Head tL ON a.Roll_Packing_Party_IdNo = tL.Ledger_IdNo " &
                                                   " Where a.Total_Meters <> 0 and a.Roll_Packing_Code=  '" & Trim(NewCode) & "'  Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)

                prn_HdDt = New DataTable
                da1.Fill(prn_HdDt)

                If prn_HdDt.Rows.Count > 0 Then

                    For i = 0 To prn_HdDt.Rows.Count - 1

                        If Trim(UCase(vCLONAME)) <> Trim(UCase(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)) Then
                            sno = 0
                        End If

                        sno = sno + 1
                        prn_HdMxIndx = prn_HdMxIndx + 1

                        prn_HdAr(prn_HdMxIndx, 0) = sno

                        prn_HdAr(prn_HdMxIndx, 1) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                        If Trim(prn_HdDt.Rows(i).Item("Cloth_Description").ToString) <> "" Then
                            prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_HdDt.Rows(i).Item("Cloth_Description").ToString)
                        Else

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                                prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_HdDt.Rows(i).Item("ClothMain_Name").ToString)
                            Else
                                prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)
                            End If

                        End If
                        prn_HdAr(prn_HdMxIndx, 3) = Val(prn_HdDt.Rows(i).Item("Pak_Pcs").ToString)
                        prn_HdAr(prn_HdMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Pak_Mtrs").ToString), "#########0.00")
                        prn_HdAr(prn_HdMxIndx, 5) = Format(Val(prn_HdDt.Rows(i).Item("Gross_Weight").ToString), "#########0.00")

                        If Val(prn_HdAr(prn_HdMxIndx, 1)) = 8116 Then
                            Debug.Print(prn_HdAr(prn_HdMxIndx, 1))
                        End If

                        If Val(prn_HdDt.Rows(i).Item("Net_Weight").ToString) <> 0 Then
                            prn_HdAr(prn_HdMxIndx, 6) = Format(Val(prn_HdDt.Rows(i).Item("Net_Weight").ToString), "#########0.00")

                        Else
                            prn_HdAr(prn_HdMxIndx, 6) = Format(Val(prn_HdDt.Rows(i).Item("Total_Weight").ToString), "#########0.00")
                        End If
                        prn_HdAr(prn_HdMxIndx, 7) = Val(sno)
                        prn_HdAr(prn_HdMxIndx, 8) = Trim(prn_HdDt.Rows(i).Item("folding").ToString)
                        prn_HdAr(prn_HdMxIndx, 9) = Trim(prn_HdDt.Rows(i).Item("loom_number").ToString)

                        vwgtmtr = 0
                        vwgtmtr = Format(Val(prn_HdAr(prn_HdMxIndx, 6)) / Val(prn_HdAr(prn_HdMxIndx, 4)), "#########0.00")

                        prn_HdAr(prn_HdMxIndx, 10) = ""
                        If Val(vwgtmtr) <> 0 Then
                            prn_HdAr(prn_HdMxIndx, 10) = Format(Val(vwgtmtr), "#########0.00")
                        End If

                        prn_HdAr(prn_HdMxIndx, 11) = Trim(prn_HdDt.Rows(i).Item("sort_no").ToString)



                        prn_HdAr(prn_HdMxIndx, 14) = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)

                        vCLONAME = prn_HdDt.Rows(i).Item("Cloth_Name").ToString


                        If InStr(1, Trim(UCase(vDUP_PACKSLIPNO)), "~" & Trim(UCase(prn_HdAr(prn_HdMxIndx, 1))) & "~") = 0 Then
                            prn_PACKINGLISTNO = Trim(prn_PACKINGLISTNO) & IIf(Trim(prn_PACKINGLISTNO) <> "", ", ", "") & Trim(prn_HdAr(prn_HdMxIndx, 1))
                            vDUP_PACKSLIPNO = Trim(vDUP_PACKSLIPNO) & "~" & Trim(prn_HdAr(prn_HdMxIndx, 1)) & "~"
                        End If

                        vPRN_FOLDINGPERC = prn_HdAr(prn_HdMxIndx, 8)
                        If Val(vPRN_FOLDINGPERC) = 0 Then vPRN_FOLDINGPERC = 100


                        'If InStr(1, Trim(UCase(vDUP_ORDNO)), "~" & Trim(UCase(prn_HdAr(prn_HdMxIndx, 12))) & "~") = 0 Then
                        '    vPRN_Party_OrderNo = Trim(vPRN_Party_OrderNo) & IIf(Trim(vPRN_Party_OrderNo) <> "", ", ", "") & Trim(prn_HdAr(prn_HdMxIndx, 12))
                        '    vPRN_Party_OrderDate = Trim(vPRN_Party_OrderDate) & IIf(Trim(vPRN_Party_OrderDate) <> "", ", ", "") & Trim(prn_HdAr(prn_HdMxIndx, 13))
                        '    vDUP_ORDNO = Trim(vDUP_ORDNO) & "~" & Trim(prn_HdAr(prn_HdMxIndx, 12)) & "~"
                        'End If

                        'prn_TotalBales = prn_TotalBales + 1

                        prn_TotaGrslWgt = Format(Val(prn_TotaGrslWgt) + Format(Val(prn_HdDt.Rows(i).Item("Gross_Weight").ToString), "#########0.000"), "##########0.000")

                        prn_DetMxIndx = 0

                        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Loom_Name from Packing_Slip_Details a LEFT OUTER JOIN Loom_Head b ON a.Loom_IdNo = b.Loom_IdNo where a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString) & "' and a.Roll_Packing_Code=  '" & Trim(NewCode) & "'  order by a.Sl_No", con)
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
                                    prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 6) = Trim(prn_HdDt.Rows(i).Item("Folding").ToString)

                                    vLMNO = ""
                                    If IsDBNull(prn_DetDt.Rows(j).Item("Loom_Name").ToString) = False Then
                                        If Trim(prn_DetDt.Rows(j).Item("Loom_Name").ToString) <> "" Then
                                            vLMNO = Trim(prn_DetDt.Rows(j).Item("Loom_Name").ToString)
                                        End If
                                    End If
                                    If Trim(vLMNO) = "" Then
                                        vLMNO = Trim(prn_DetDt.Rows(j).Item("Loom_No").ToString)
                                    End If
                                    prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 7) = Trim(vLMNO)

                                    'If InStr(1, Trim(UCase(vDUP_PACKSLIPNO)), Trim(UCase(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString))) = 0 Then
                                    '    prn_DetAr1(prn_DetMxIndx1, 5) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                                    'End If

                                    prn_DetMxIndx1 = prn_DetMxIndx1 + 1
                                    prn_DetAr1(prn_DetMxIndx1, 5) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                                    prn_DetAr1(prn_DetMxIndx1, 0) = prn_DetMxIndx1
                                    prn_DetAr1(prn_DetMxIndx1, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                    prn_DetAr1(prn_DetMxIndx1, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                    prn_DetAr1(prn_DetMxIndx1, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
                                    prn_DetAr1(prn_DetMxIndx1, 4) = Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.000")
                                    prn_DetAr1(prn_DetMxIndx1, 6) = Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString) / Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.000")

                                    vLMNO = ""
                                    If IsDBNull(prn_DetDt.Rows(j).Item("Loom_Name").ToString) = False Then
                                        If Trim(prn_DetDt.Rows(j).Item("Loom_Name").ToString) <> "" Then
                                            vLMNO = Trim(prn_DetDt.Rows(j).Item("Loom_Name").ToString)
                                        End If
                                    End If
                                    If Trim(vLMNO) = "" Then
                                        vLMNO = Trim(prn_DetDt.Rows(j).Item("Loom_No").ToString)
                                    End If
                                    prn_DetAr1(prn_DetMxIndx1, 7) = Trim(vLMNO)

                                    prn_TotalPcs = Val(prn_TotalPcs) + 1
                                    prn_TotalMtrs = Format(Val(prn_TotalMtrs) + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00"), "##########0.00")
                                    prn_TotalWgt = Format(Val(prn_TotalWgt) + Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.000"), "##########0.000")

                                End If
                            Next j


                        Else


                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 5) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 0) = 1
                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = ""
                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = ""
                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_HdDt.Rows(i).Item("Pak_Mtrs").ToString), "#########0.00")

                            If Val(prn_HdDt.Rows(i).Item("Net_Weight").ToString) <> 0 Then
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Net_Weight").ToString), "#########0.000")

                            Else
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Total_Weight").ToString), "#########0.000")

                            End If

                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 6) = Trim(prn_HdDt.Rows(i).Item("Folding").ToString)
                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 7) = ""


                            prn_DetMxIndx1 = prn_DetMxIndx1 + 1
                            prn_DetAr1(prn_DetMxIndx1, 5) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                            prn_DetAr1(prn_DetMxIndx1, 0) = prn_DetMxIndx1
                            prn_DetAr1(prn_DetMxIndx1, 1) = ""
                            prn_DetAr1(prn_DetMxIndx1, 2) = ""
                            prn_DetAr1(prn_DetMxIndx1, 3) = Format(Val(prn_HdDt.Rows(i).Item("Pak_Mtrs").ToString), "#########0.00")
                            If Val(prn_HdDt.Rows(i).Item("Net_Weight").ToString) <> 0 Then
                                prn_DetAr1(prn_DetMxIndx1, 4) = Format(Val(prn_HdDt.Rows(i).Item("Net_Weight").ToString), "#########0.000")

                            Else
                                prn_DetAr1(prn_DetMxIndx1, 4) = Format(Val(prn_HdDt.Rows(i).Item("Total_Weight").ToString), "#########0.000")

                            End If

                            prn_DetAr1(prn_DetMxIndx1, 6) = Format(Val(prn_DetAr1(prn_DetMxIndx1, 4)) / Val(prn_DetAr1(prn_DetMxIndx1, 3)), "#########0.000")

                            prn_DetAr1(prn_DetMxIndx1, 7) = ""

                            prn_TotalPcs = Val(prn_TotalPcs) + Val(prn_HdDt.Rows(i).Item("Pak_Pcs").ToString)
                            prn_TotalMtrs = Format(Val(prn_TotalMtrs) + Format(Val(prn_DetAr1(prn_DetMxIndx1, 3)), "#########0.00"), "##########0.00")
                            prn_TotalWgt = Format(Val(prn_TotalWgt) + Format(Val(prn_DetAr1(prn_DetMxIndx1, 4)), "#########0.000"), "##########0.000")

                        End If

                    Next i

                Else
                    MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End If
            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub


    Private Sub PrintDocument3_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument3.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        vPrn_Roll_Packing_No = prn_HdDt2.Rows(0).Item("Roll_Packing_No").ToString
        vPrn_Roll_Packing_Date = prn_HdDt2.Rows(0).Item("Roll_Packing_Date").ToString
        vPrn_Roll_Packing_Po_No = prn_HdDt2.Rows(0).Item("Roll_Packing_Po_No").ToString
        vPrn_Roll_Packing_Po_Date = prn_HdDt2.Rows(0).Item("Roll_Packing_Po_Date").ToString

        vSort_No = Trim(prn_HdDt.Rows(0).Item("sort_no").ToString)
        vCLONAME = prn_HdDt.Rows(0).Item("Cloth_Name").ToString


        Common_Procedures.Printing_Format_PackingList_1464(PrintDocument3, e, prn_DetDt, prn_HdMxIndx, prn_HdDt, prn_HdAr, prn_PageNo, prn_HdIndx, prn_Count, prn_DetAr, prn_PL_Tot_Rls, prn_PL_Tot_Pcs, prn_PL_Tot_Mtr, prn_DetIndx, prn_DetMxIndx1, prn_PL_Tot_GrsWgt, prn_PL_Tot_NetWgt, prn_HdDt2, prn_TotalPcs, vPrn_Roll_Packing_No, vPrn_Roll_Packing_Date, vPrn_Roll_Packing_Po_No, vPrn_Roll_Packing_Po_Date, vSort_No, vCLONAME, vPrn_Invoice_Dc_No:="", vPrn_Invoice_Dc_Date:="", vPrn_ClothSales_Invoice_No:="", vPrn_ClothSales_Invoice_Date:="")

        'Printing_Format_PackingList_1464(PrintDocument3, e)

    End Sub
    'Private Sub Printing_Format_PackingList_1464(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs)
    '    Dim NoofDets As Integer, NoofItems_PerPage As Integer
    '    Dim pFont As Font, P1fONT As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim CurY As Single, TxtHgt As Single
    '    Dim LnAr(15) As Single, ClArr(15) As Single
    '    Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False
    '    Dim LM As Single = 0, TM As Single = 0
    '    Dim PgWt As Single = 0, PrWt As Single = 0
    '    Dim PgHt As Single = 0, PrHt As Single = 0
    '    Dim vPcsNos As String = ""
    '    Dim vLmNos As String = ""
    '    Dim vTot_Pcs As String = ""


    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
    '            e.PageSettings.PaperSize = ps
    '            Exit For
    '        End If
    '    Next

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 15
    '        .Right = 40
    '        .Top = 35
    '        .Bottom = 40
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

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

    '    e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

    '    pFont = New Font("Calibri", 10, FontStyle.Regular)

    '    NoofItems_PerPage = 36 ' 32

    '    Erase ClArr
    '    Erase LnAr
    '    ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClArr(1) = 35 : ClArr(2) = 70 : ClArr(3) = 80 : ClArr(4) = 110 : ClArr(5) = 50 : ClArr(6) = 50 : ClArr(7) = 50 : ClArr(8) = 40 : ClArr(9) = 75 : ClArr(10) = 75 : ClArr(11) = 75
    '    ClArr(12) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11))

    '    TxtHgt = 18.75 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  '20

    '    Try

    '        If prn_DetDt.Rows.Count > 0 Then

    '            If prn_HdMxIndx > 0 Then

    '                Erase LnAr
    '                LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '                Printing_Format_PackingList_1464_PageHeader(PrintDocument1, e, prn_HdDt, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, prn_HdIndx)
    '                CurY = CurY - 10

    '                NoofDets = 0
    '                Do While prn_HdIndx <= prn_HdMxIndx


    '                    If NoofDets >= NoofItems_PerPage Then

    '                        CurY = CurY + TxtHgt
    '                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) - 10, CurY, 1, 0, pFont)
    '                        NoofDets = NoofDets + 1

    '                        Printing_Format_PackingList_1464_PageFooter(e, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, False)

    '                        e.HasMorePages = True

    '                        NoofDets = 0
    '                        prn_Count = prn_Count + 1

    '                        Return

    '                    End If

    '                    prn_HdIndx = prn_HdIndx + 1

    '                    'If NoofDets > 0 And prn_HdIndx > 0 And Trim(prn_HdAr(prn_HdIndx, 14)) <> "" And Trim(UCase(prn_HdAr(prn_HdIndx, 14))) <> Trim(UCase(prn_HdAr(prn_HdIndx - 1, 14))) Then

    '                    '    Printing_Format_PackingList_1464_PageFooter(e, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, True)

    '                    '    prn_HdIndx = prn_HdIndx - 1

    '                    '    prn_PL_Tot_Rls = 0
    '                    '    prn_PL_Tot_Pcs = 0
    '                    '    prn_PL_Tot_Mtr = 0
    '                    '    prn_PL_Tot_GrsWgt = 0
    '                    '    prn_PL_Tot_NetWgt = 0

    '                    '    e.HasMorePages = True

    '                    '    Return

    '                    'End If


    '                    vPcsNos = ""
    '                    vLmNos = ""
    '                    vTot_Pcs = 0

    '                    'If Val(prn_HdAr(prn_HdIndx, 4)) <> 0 Then
    '                    '                        If Val(prn_DetAr1(prn_HdIndx, 3)) <> 0 Then


    '                    If Val(prn_HdAr(prn_HdIndx, 4)) <> 0 Then


    '                            'vPcsNos = Trim(prn_DetAr1(prn_HdIndx, 2))
    '                            'If Trim(prn_DetAr1(prn_HdIndx, 2)) <> "" Then
    '                            '    vPcsNos = Trim(vPcsNos) & "," & Trim(prn_DetAr1(prn_HdIndx, 2))
    '                            'End If
    '                            'If Trim(prn_DetAr1(prn_HdIndx, 2)) <> "" Then
    '                            '    vPcsNos = Trim(vPcsNos) & "," & Trim(prn_DetAr1(prn_HdIndx, 2))
    '                            'End If

    '                            'vLmNos = Trim(prn_DetAr1(prn_HdIndx, 7))
    '                            'If Trim(prn_DetAr1(prn_HdIndx, 7)) <> "" Then
    '                            '    vLmNos = Trim(vLmNos) & "," & Trim(prn_DetAr1(prn_HdIndx, 7))
    '                            'End If
    '                            'If Trim(prn_DetAr1(prn_HdIndx, 7)) <> "" Then
    '                            '    vLmNos = Trim(vLmNos) & "," & Trim(prn_DetAr1(prn_HdIndx, 7))
    '                            'End If


    '                            vPcsNos = Trim(prn_DetAr(prn_HdIndx, 1, 2))
    '                        If Trim(prn_DetAr(prn_HdIndx, 2, 2)) <> "" Then
    '                            vPcsNos = Trim(vPcsNos) & "," & Trim(prn_DetAr(prn_HdIndx, 2, 2))
    '                        End If
    '                        If Trim(prn_DetAr(prn_HdIndx, 3, 2)) <> "" Then
    '                            vPcsNos = Trim(vPcsNos) & "," & Trim(prn_DetAr(prn_HdIndx, 3, 2))
    '                        End If

    '                        vLmNos = Trim(prn_DetAr(prn_HdIndx, 1, 7))
    '                        If Trim(prn_DetAr(prn_HdIndx, 2, 7)) <> "" Then
    '                            vLmNos = Trim(vLmNos) & "," & Trim(prn_DetAr(prn_HdIndx, 2, 7))
    '                        End If
    '                        If Trim(prn_DetAr(prn_HdIndx, 3, 7)) <> "" Then
    '                            vLmNos = Trim(vLmNos) & "," & Trim(prn_DetAr(prn_HdIndx, 3, 7))
    '                        End If

    '                        CurY = CurY + TxtHgt

    '                        P1fONT = New Font("Calibri", 10, FontStyle.Regular)





    '                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdAr(prn_HdIndx, 7)), LMargin + 10, CurY, 0, 0, P1fONT)

    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdAr(prn_HdIndx, 1)), LMargin + ClArr(1) + 5, CurY, 0, ClArr(2), P1fONT,, True)

    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(vLmNos), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, ClArr(3), P1fONT,, True)

    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(vPcsNos), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, ClArr(4), P1fONT,, True)
    '                        If Val(prn_DetAr(prn_HdIndx, 1, 3)) <> 0 Then
    '                            vTot_Pcs = vTot_Pcs + 1
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetAr(prn_HdIndx, 1, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 1, 0, P1fONT)
    '                        End If
    '                        If Val(prn_DetAr(prn_HdIndx, 2, 3)) <> 0 Then
    '                            vTot_Pcs = vTot_Pcs + 1

    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetAr(prn_HdIndx, 2, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, 1, 0, P1fONT)
    '                        End If
    '                        If Val(prn_DetAr(prn_HdIndx, 3, 3)) <> 0 Then
    '                            vTot_Pcs = vTot_Pcs + 1

    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetAr(prn_HdIndx, 3, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, 1, 0, P1fONT)
    '                        End If

    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(vTot_Pcs), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, P1fONT)

    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdAr(prn_HdIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9), CurY, 1, 0, P1fONT)
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdAr(prn_HdIndx, 5)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10), CurY, 1, 0, P1fONT)
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdAr(prn_HdIndx, 6)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11), CurY, 1, 0, P1fONT)

    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(Format(Val(prn_HdAr(prn_HdIndx, 6)), "#########0.000") / Format(Val(prn_HdAr(prn_HdIndx, 4)), "#########0.00")), "############0.000"), PageWidth - 10, CurY, 1, 0, P1fONT)



    '                        'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetAr1(prn_HdIndx, 0)), LMargin + 10, CurY, 0, 0, P1fONT)

    '                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_HdIndx, 5)), LMargin + ClArr(1) + 5, CurY, 0, ClArr(2), P1fONT,, True)

    '                        'Common_Procedures.Print_To_PrintDocument(e, Trim(vLmNos), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, ClArr(3), P1fONT, Shrink_To_Fit:=True)

    '                        'Common_Procedures.Print_To_PrintDocument(e, Trim(vPcsNos), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, ClArr(4), P1fONT,, True)
    '                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 1, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 30, CurY, 1, 0, P1fONT)
    '                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 2, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 25, CurY, 1, 0, P1fONT)
    '                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 3, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 25, CurY, 1, 0, P1fONT)

    '                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_TotalPcs), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, 1, 0, P1fONT)

    '                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr1(prn_HdIndx, 3)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY, 1, 0, P1fONT)
    '                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr1(prn_HdIndx, 8)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 1, 0, P1fONT)
    '                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr1(prn_HdIndx, 4)), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 10, CurY, 1, 0, P1fONT)



    '                        prn_PL_Tot_Rls = prn_PL_Tot_Rls + 1

    '                        If Trim(prn_DetAr(prn_HdIndx, 1, 2)) <> "" And Val(prn_DetAr(prn_DetMxIndx1, 1, 3)) > 0 Then
    '                            prn_PL_Tot_Pcs = prn_PL_Tot_Pcs + 1
    '                        End If
    '                        If Trim(prn_DetAr(prn_HdIndx, 2, 2)) <> "" And Val(prn_DetAr(prn_HdIndx, 2, 3)) > 0 Then
    '                            prn_PL_Tot_Pcs = prn_PL_Tot_Pcs + 1
    '                        End If
    '                        If Trim(prn_DetAr(prn_HdIndx, 3, 2)) <> "" And Val(prn_DetAr(prn_HdIndx, 3, 3)) > 0 Then
    '                            prn_PL_Tot_Pcs = prn_PL_Tot_Pcs + 1
    '                        End If

    '                        prn_PL_Tot_Mtr = Format(Val(prn_PL_Tot_Mtr) + Format(Val(prn_HdAr(prn_HdIndx, 4)), "#########0.00"), "#########0.00")
    '                        prn_PL_Tot_GrsWgt = Format(Val(prn_PL_Tot_GrsWgt) + Format(Val(prn_HdAr(prn_HdIndx, 5)), "#########0.000"), "#########0.000")
    '                        prn_PL_Tot_NetWgt = Format(Val(prn_PL_Tot_NetWgt) + Format(Val(prn_HdAr(prn_HdIndx, 6)), "#########0.000"), "#########0.000")

    '                        '    prn_TotalGrams = Format(Val(prn_TotalGrams) + Format(Val(prn_HdAr(prn_HdIndx, 6)), "#########0.000") / Format(Val(prn_HdAr(prn_HdIndx, 4)), "#########0.00"), "##########0.000")


    '                        NoofDets = NoofDets + 1
    '                            prn_DetIndx = prn_DetIndx + 1

    '                        End If

    '                Loop

    '                Printing_Format_PackingList_1464_PageFooter(e, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, True)

    '            End If

    '        End If

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    '    e.HasMorePages = False

    'End Sub


    'Private Sub Printing_Format_PackingList_1464_PageHeader(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdDt As DataTable, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal prn_HdIndx As Integer)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim dt3 As New DataTable
    '    Dim dt4 As New DataTable
    '    Dim p1Font As Font
    '    Dim strHeight As Single
    '    Dim Cmp_Add As String = ""
    '    Dim C1, C2 As Single, W1, W2 As Single, S1, S2 As Single
    '    Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String

    '    PageNo = PageNo + 1

    '    CurY = TMargin + 30

    '    'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from ClothSales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
    '    'da2.Fill(dt2)
    '    'If dt2.Rows.Count > NoofItems_PerPage Then
    '    '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    'End If
    '    'dt2.Clear()

    '    prn_Count = prn_Count + 1

    '    p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "PACKING LIST", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)

    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY
    '    Desc = ""
    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

    '    Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
    '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
    '        Cmp_TinNo = "GST NO.: " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '        Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '    End If

    '    CurY = CurY + TxtHgt - 5

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1249-" Then
    '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Vaipav, Drawing.Image), LMargin + 20, CurY, 100, 90)
    '    End If

    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

    '    p1Font = New Font("Calibri", 9, FontStyle.Regular)
    '    CurY = CurY + strHeight - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font,, True)
    '    'CurY = CurY + TxtHgt - 1
    '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "     " & Cmp_TinNo & "     " & Cmp_EMail, LMargin, CurY, 2, PrintWidth, p1Font,, True)
    '    'CurY = CurY + TxtHgt - 1
    '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, PageWidth - 10, CurY, 1, 0, p1Font)

    '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
    '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

    '    CurY = CurY + TxtHgt + 10
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
    '    C2 = ClAr(1) + ClAr(2) + ClAr(3)
    '    W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
    '    S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
    '    W2 = e.Graphics.MeasureString("Sort No.    : ", pFont).Width
    '    S2 = e.Graphics.MeasureString("P.O Date   : ", pFont).Width


    '    CurY = CurY + 10
    '    p1Font = New Font("Calibri", 9, FontStyle.Bold)
    '    'p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font,, True)

    '    Common_Procedures.Print_To_PrintDocument(e, "Packing List No", LMargin + C1 + ClAr(4) + ClAr(1) + 15, CurY - 5, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + ClAr(4) + ClAr(1) + 15, CurY - 5, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt2.Rows(0).Item("Roll_Packing_No").ToString, PageWidth - ClAr(1) - ClAr(2) - 5, CurY - 5, 0, 0, p1Font)
    '    Dim vLEDADD As String = ""
    '    'Dim vDELADD As String = ""

    '    vLEDADD = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
    '    If Trim(vLEDADD) = "" Then
    '        vLEDADD = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
    '    End If
    '    If Trim(vLEDADD) = "" Then
    '        vLEDADD = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
    '    End If
    '    If Trim(vLEDADD) = "" Then
    '        vLEDADD = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
    '    End If

    '    'vDELADD = prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString

    '    'If Trim(vDELADD) = "" Then
    '    '    vDELADD = prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString
    '    'End If
    '    'If Trim(vDELADD) = "" Then
    '    '    vDELADD = prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString
    '    'End If
    '    'If Trim(vDELADD) = "" Then
    '    '    vDELADD = prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString
    '    'End If
    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(vLEDADD), LMargin + S1 - 10, CurY, 0, 0, pFont)
    '    ' Common_Procedures.Print_To_PrintDocument(e, " " & Trim(vDELADD), LMargin + S1 + ClAr(4) + ClAr(7) + ClAr(3) + ClAr(5), CurY, 0, 0, pFont)
    '    p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "Packing List Date", LMargin + C1 + ClAr(1) + ClAr(4) + 15, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + ClAr(1) + ClAr(4) + 20, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt2.Rows(0).Item("Roll_Packing_Date").ToString), "dd-MM-yyyy").ToString, PageWidth - ClAr(1) - ClAr(2) - 5, CurY, 0, 0, pFont)

    '    CurY = CurY + TxtHgt
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    'e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(1) + ClAr(2) + 10, CurY, LMargin + C2 + ClAr(1) + ClAr(2) + 10, LnAr(2))
    '    e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(1) + ClAr(2) + ClAr(4) + ClAr(8) + ClAr(2) + 10, CurY, LMargin + C2 + ClAr(1) + ClAr(2) + ClAr(4) + ClAr(8) + ClAr(2) + 10, LnAr(2))

    '    Try

    '        CurY = CurY + TxtHgt - 10
    '        Common_Procedures.Print_To_PrintDocument(e, "Sort No.", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdAr(prn_HdIndx + 1, 11), LMargin + W2 + 25, CurY, 0, 0, pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "Con ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 15, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdAr(prn_HdIndx + 1, 14), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 20, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt + 5
    '        Common_Procedures.Print_To_PrintDocument(e, "P.O No. ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt2.Rows(0).Item("Roll_Packing_Po_No").ToString, LMargin + W2 + 25, CurY, 0, 0, pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "P.O Date  :  " & Trim(prn_HdDt2.Rows(0).Item("Roll_Packing_Po_Date").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt2.Rows(0).Item("Roll_Packing_Po_Date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 20, CurY, 0, 0, pFont)




    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(2) = CurY

    '        CurY = CurY + TxtHgt - 15
    '        Common_Procedures.Print_To_PrintDocument(e, "INDIVIDUAL PCS METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 5, 2, ClAr(8), pFont)
    '        CurY = CurY + TxtHgt + 10
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY)
    '        LnAr(4) = CurY

    '        CurY = CurY - 15 ' 10

    '        Common_Procedures.Print_To_PrintDocument(e, "S.No", LMargin, CurY, 2, ClAr(1), pFont)


    '        Common_Procedures.Print_To_PrintDocument(e, "ROLL NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "LOOM NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "FER.NO.", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "1", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 18, 2, ClAr(5), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "2", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 18, 2, ClAr(6), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "3", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 18, 2, ClAr(7), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "GROSS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "NET", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "GRAMS", PageWidth - 10, CurY, 1, 0, pFont)

    '        CurY = CurY + 15
    '        '  Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
    '        ' Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) - 8, CurY, 2, ClAr(3), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)


    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(3) = CurY

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub



    'Private Sub Printing_Format_PackingList_1464_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal prn_HdIndx As Integer, ByVal is_LastPage As Boolean)
    '    Dim I As Integer
    '    Dim p1Font As Font

    '    p1Font = New Font("Calibri", 7, FontStyle.Bold)

    '    Try

    '        For I = NoofDets + 1 To NoofItems_PerPage
    '            CurY = CurY + TxtHgt
    '        Next


    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(2))
    '        ''e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(2))

    '        'e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(2))

    '        If is_LastPage = True Then


    '            CurY = CurY + TxtHgt + 5
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(5) = CurY

    '            CurY = CurY + 5

    '            Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + 5, CurY, 0, 0, pFont, , True)
    '            Common_Procedures.Print_To_PrintDocument(e, Val(prn_PL_Tot_Rls), LMargin + ClAr(1) + ClAr(2) - 15, CurY, 1, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Val(prn_TotalPcs), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

    '            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_PL_Tot_Mtr), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 1, 0, pFont)

    '            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_PL_Tot_GrsWgt), "########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)

    '            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_PL_Tot_NetWgt), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 1, 0, pFont)

    '            '   Common_Procedures.Print_To_PrintDocument(e, prn_TotalGrams, PageWidth - 10, CurY, 1, 0, pFont)
    '            '  'Common_Procedures.Print_To_PrintDocument(e, Format(Val(Format(Val(prn_PL_Tot_NetWgt), "#########0.000") / Format(Val(prn_PL_Tot_Mtr), "########0.00")), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)


    '        End If

    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY + 24, LMargin + ClAr(1), LnAr(5))

    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY + 24, LMargin + ClAr(1) + ClAr(2), LnAr(5))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 24, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(2) + ClAr(3) + ClAr(4), CurY + 24, LMargin + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 24, LMargin + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 24, LMargin + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5))

    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + 24, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5))

    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + 24, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + 24, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(5))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY + 24, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(5))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY + 24, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(5))
    '        ''e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY + 24, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(5))


    '        CurY = CurY + TxtHgt + 5
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(6) = CurY




    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(2))



    '            CurY = CurY + TxtHgt - 10

    '            p1Font = New Font("Calibri", 12, FontStyle.Bold)

    '        Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString), PageWidth - 15, CurY, 1, 0, p1Font)
    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt

    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)

    '        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 5, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY", PageWidth - 5, CurY, 1, 0, pFont)
    '        CurY = CurY + TxtHgt + 10

    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    Private Sub txt_Roll_Packing_PoNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Roll_Packing_PoNo.KeyDown
        If e.KeyCode = 38 Then
            txt_Note.Focus()
        ElseIf e.KeyCode = 40 Then
            msk_Roll_Packing_Po_Date.Focus()

        End If
    End Sub

    Private Sub txt_Roll_Packing_PoNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Roll_Packing_PoNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            msk_Roll_Packing_Po_Date.Focus()

        End If
    End Sub

    Private Sub msk_Roll_Packing_Po_Date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_Roll_Packing_Po_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub
    Private Sub btn_BarcodePrint_Click(sender As Object, e As EventArgs) Handles btn_BarcodePrint.Click

        Common_Procedures.Print_OR_Preview_Status = 0
        Prn_BarcodeSticker = True
        Printing_BarCode_Sticker_Format1_1464_DosPrint()

    End Sub
    Private Sub Printing_BarCode_Sticker_Format1_1464_DosPrint()
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
        Dim index As Integer = 0
        Dim indexchr As String = ""
        Dim character() As Char

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetSNo = 0
        prn_PageNo = 0




        'If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        'If Val(txt_PrintTo.Text) = 0 Then Exit Sub

        'prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        'prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        'Condt = ""
        'If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
        '    Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        'ElseIf Val(txt_PrintFrom.Text) <> 0 Then
        '    Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        'Else
        '    Exit Sub

        'End If


        Try
            da1 = New SqlClient.SqlDataAdapter("select a.*,b.*, tZ.*,c.* from Roll_Packing_Head a INNER JOIN ROll_Packing_Details b on a.Roll_Packing_Code=b.Roll_Packing_Code INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno LEFT OUTER JOIN LEDGER_Head E ON A.Ledger_IdNo = E.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.Roll_Packing_Code = '" & Trim(NewCode) & "' Order by a.Roll_Packing_Date, a.for_OrderBy, a.Roll_Packing_No, a.Roll_Packing_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_DetDt)
            If prn_DetDt.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            'prn_HdDt.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End Try

        NoofItems_PerPage = 1

        LnCnt = 0

        fs = New FileStream(Common_Procedures.Dos_Printing_FileName_Path, FileMode.Create)
        sw = New StreamWriter(fs, System.Text.Encoding.Default)


        Try

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                    'vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Meters").ToString), "##########0.00")
                    'vPcs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Pcs").ToString), "##########0.00")

                    'vYrCode = Microsoft.VisualBasic.Right(prn_DetDt.Rows(prn_DetIndx).Item("Roll_Packing_Code").ToString, 5)
                    'vBarCode = Microsoft.VisualBasic.Left(vYrCode, 2) & Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Company_IdNo").ToString)) & Trim(UCase(prn_DetDt.Rows(prn_DetIndx).Item("Packing_Slip_No").ToString))
                    'vBarCode = Trim(UCase(vBarCode))
                    ''vBarCode = "*" & Trim(UCase(vBarCode)) & "*"


                    If (prn_DetIndx = 0 Or prn_DetIndx Mod 2 = 0) Then
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) <> 0 Then

                            PrnTxt = "I8,A"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "q819"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "O"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "JF"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "ZT"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "Q240,25"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "N"
                            sw.WriteLine(PrnTxt)


                            PrnTxt = "A786,144,2,4,1,1,N,""FER No """
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A786,178,2,4,1,1,N,""Meters"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A786,44,2,4,1,1,N,""Sort No"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A786,212,2,4,1,1,N,""Roll No"""
                            sw.WriteLine(PrnTxt)

                            PrnTxt = "B512,59,1,1C,2,4,27,N,""" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Roll_No").ToString) & """" ' ---- BARCODE
                            sw.WriteLine(PrnTxt)




                            indexchr = ""

                            indexchr = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Roll_No").ToString)
                            'Dim character() As String = indexchr.Split(New Char() {})
                            character = indexchr.ToCharArray()


                            For index = 0 To character.Length - 1


                                If index = 0 Then
                                    PrnTxt = "A485,59,1,4,1,1,N,""" & character(0) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 1 Then
                                    PrnTxt = "A485,79,1,4,1,1,N,""" & character(1) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 2 Then
                                    PrnTxt = "A485,100,1,4,1,1,N,""" & character(2) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 3 Then
                                    PrnTxt = "A485,120,1,4,1,1,N,""" & character(3) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 4 Then
                                    PrnTxt = "A485,140,1,4,1,1,N,""" & character(4) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 5 Then
                                    PrnTxt = "A485,161,1,4,1,1,N,""" & character(5) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 6 Then
                                    PrnTxt = "A485,181,1,4,1,1,N,""" & character(6) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 7 Then
                                    PrnTxt = "A485,201,1,4,1,1,N,""" & character(7) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                            Next

                            PrnTxt = "A785,110,2,4,1,1,N,""Gross.Wt"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A785,77,2,4,1,1,N,""Net.Wt """
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A657,212,2,4,1,1,N,"":"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A657,178,2,4,1,1,N,"":"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A657,144,2,4,1,1,N,"":"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A658,44,2,4,1,1,N,"":"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A657,110,2,4,1,1,N,"":"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A658,80,2,4,1,1,N,"":"""
                            sw.WriteLine(PrnTxt)

                            PrnTxt = "A639,210,2,4,1,1,N,""" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Roll_No").ToString) & """"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A640,178,2,4,1,1,N,""" & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "##########0.00") & """"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A640,142,2,4,1,1,N,""" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Pcs_NO").ToString) & """"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A640,110,2,4,1,1,N,""" & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Gross_Weight").ToString), "##########0.000") & """"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A640,77,2,4,1,1,N,""" & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "##########0.000") & """"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A639,44,2,4,1,1,N,""" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sort_No").ToString) & """"
                            sw.WriteLine(PrnTxt)

                        End If

                    ElseIf prn_DetIndx Mod 2 = 1 Then
                        '--- 2

                        ';prn_DetIndx = prn_DetIndx + 1

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) <> 0 Then

                            PrnTxt = "A386,144,2,4,1,1,N,""FER No """
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A386,178,2,4,1,1,N,""Meters"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A386,44,2,4,1,1,N,""Sort No"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A386,212,2,4,1,1,N,""Roll No"""
                            sw.WriteLine(PrnTxt)

                            PrnTxt = "B112,59,1,1C,2,4,27,N,""" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Roll_No").ToString) & """" ' ---- BARCODE
                            sw.WriteLine(PrnTxt)


                            indexchr = ""

                            indexchr = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Roll_No").ToString)
                            character = indexchr.ToCharArray()

                            For index = 0 To character.Length - 1


                                If index = 0 Then
                                    PrnTxt = "A85,59,1,4,1,1,N,""" & character(0) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 1 Then
                                    PrnTxt = "A85,79,1,4,1,1,N,""" & character(1) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 2 Then
                                    PrnTxt = "A85,100,1,4,1,1,N,""" & character(2) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 3 Then
                                    PrnTxt = "A85,120,1,4,1,1,N,""" & character(3) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 4 Then
                                    PrnTxt = "A85,140,1,4,1,1,N,""" & character(4) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 5 Then
                                    PrnTxt = "A85,161,1,4,1,1,N,""" & character(5) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                                If index = 6 Then
                                    PrnTxt = "A85,181,1,4,1,1,N,""" & character(6) & """"
                                    sw.WriteLine(PrnTxt)

                                End If

                                If index = 7 Then
                                    PrnTxt = "A85,201,1,4,1,1,N,""" & character(7) & """"
                                    sw.WriteLine(PrnTxt)
                                End If

                            Next


                            PrnTxt = "A385,110,2,4,1,1,N,""Gross.Wt"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A385,77,2,4,1,1,N,""Net.Wt """
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A257,212,2,4,1,1,N,"":"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A257,178,2,4,1,1,N,"":"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A257,144,2,4,1,1,N,"":"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A258,44,2,4,1,1,N,"":"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A257,110,2,4,1,1,N,"":"""
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A258,80,2,4,1,1,N,"":"""
                            sw.WriteLine(PrnTxt)

                            PrnTxt = "A239,210,2,4,1,1,N,""" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Roll_No").ToString) & """"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A240,178,2,4,1,1,N,""" & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "##########0.00") & """"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A240,142,2,4,1,1,N,""" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Pcs_NO").ToString) & """"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A240,110,2,4,1,1,N,""" & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Gross_Weight").ToString), "##########0.000") & """"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A240,77,2,4,1,1,N,""" & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "##########0.000") & """"
                            sw.WriteLine(PrnTxt)
                            PrnTxt = "A239,44,2,4,1,1,N,""" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sort_No").ToString) & """"
                            sw.WriteLine(PrnTxt)

                            PrnTxt = "P1"
                            sw.WriteLine(PrnTxt)

                        End If
                    End If

                    prn_DetIndx = prn_DetIndx + 1

                Loop

                If prn_DetIndx Mod 2 = 1 Then
                    PrnTxt = "P1"
                    sw.WriteLine(PrnTxt)
                End If

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
End Class