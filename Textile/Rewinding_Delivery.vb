Public Class Rewinding_Delivery
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "RWDLV-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer


    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private Print_PDF_Status As Boolean = False
    Private EMAIL_Status As Boolean = False
    Private WHATSAPP_Status As Boolean = False
    Private vEMAIL_Attachment_FileName As String
    Private prn_Count As Integer


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_Ledger.Text = ""
        cbo_Rec_Ledger.Text = ""
        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""
        cbo_Grid_YarnType.Text = ""

        cbo_Filter_CountName.Text = ""
        cbo_Filter_MillName.Text = ""
        cbo_Filter_PartyName.Text = ""

        cbo_Vechile.Text = ""
        txt_Party_DcNo.Text = ""
        txt_Freight.Text = ""
        cbo_TransportName.Text = ""
        If Trim(Common_Procedures.settings.CustomerCode) = "1464" Then ' --- MOF
            cbo_Type.Text = "DIRECT"
        Else
            cbo_Type.Text = "SELECTION"
        End If

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))


        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_Rec_Ledger.Enabled = True
        cbo_Rec_Ledger.BackColor = Color.White

        cbo_TransportName.Enabled = True
        cbo_TransportName.BackColor = Color.White

        cbo_Vechile.Enabled = True
        cbo_Vechile.BackColor = Color.White

        txt_Party_DcNo.Enabled = True
        txt_Party_DcNo.BackColor = Color.White

        txt_Freight.Enabled = True
        txt_Freight.BackColor = Color.White


        cbo_Grid_CountName.Enabled = True
        cbo_Grid_CountName.BackColor = Color.White

        cbo_Grid_MillName.Enabled = True
        cbo_Grid_MillName.BackColor = Color.White

        dgv_YarnDetails.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

        cbo_Cone_Type.Text = ""
        txt_Empty_Bag_Weight.Text = ""
        txt_Empty_Cone_Weight.Text = ""


        cbo_ClothSales_OrderCode_forSelection.Text = ""


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

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False

        NoCalc_Status = False

        cbo_Cone_Type.Text = ""
        txt_Empty_Bag_Weight.Text = ""
        txt_Empty_Cone_Weight.Text = ""

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
            Msktxbx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_MillName.Name Then
            cbo_Grid_MillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_YarnType.Name Then
            cbo_Grid_YarnType.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_YarnDetails_Total.Name Then
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
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_YarnDetails_Total.CurrentCell) Then dgv_YarnDetails_Total.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_YarnDetails_Total.CurrentCell) Then dgv_YarnDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Rewinding_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Rec_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Rec_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_YarnType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "YARN" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_YarnType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Rewinding_Delivery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'REWINDING' ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"



        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or  Ledger_Type = 'GODOWN' or  Ledger_Type = 'SIZING'  or  Ledger_Type = 'WEAVER' or  Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Rec_Ledger.DataSource = dt2
        cbo_Rec_Ledger.DisplayMember = "Ledger_DisplayName"


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_TransportName.DataSource = dt3
        cbo_TransportName.DisplayMember = "Ledger_DisplayName"


        da = New SqlClient.SqlDataAdapter("select distinct(Vechile_No) from Rewinding_Delivery_Head order by Vechile_No", con)
        da.Fill(dt7)
        cbo_Vechile.DataSource = dt7
        cbo_Vechile.DisplayMember = "Vechile_No"


        da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", con)
        da.Fill(dt4)
        cbo_Grid_MillName.DataSource = dt4
        cbo_Grid_MillName.DisplayMember = "mill_name"

        da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
        da.Fill(dt5)
        cbo_Grid_CountName.DataSource = dt5
        cbo_Grid_CountName.DisplayMember = "count_name"

        da = New SqlClient.SqlDataAdapter("select yarn_type from YarnType_Head order by yarn_type", con)
        da.Fill(dt6)
        cbo_Grid_YarnType.DataSource = dt6
        cbo_Grid_YarnType.DisplayMember = "yarn_type"

        cbo_Type.Items.Add("")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("SELECTION")

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        dtp_Date.Text = ""
        msk_date.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1464" Then
            cbo_Type.Enabled = False
        End If

        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            cbo_ClothSales_OrderCode_forSelection.Visible = True
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = True

            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)

        Else

            cbo_ClothSales_OrderCode_forSelection.Visible = False
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = False

        End If


        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Rec_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Party_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cone_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Bag_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Cone_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Yarn_LotNo.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Rec_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Party_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Yarn_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cone_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Bag_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Cone_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Party_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Party_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus



        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        If Common_Procedures.settings.Show_Yarn_LotNo_Status = 1 Then
            dgv_YarnDetails.Columns(13).Visible = True
        Else
            dgv_YarnDetails.Columns(13).Visible = False
        End If


        Filter_Status = False
        FrmLdSTS = True

        new_record()

    End Sub

    Private Sub Rewinding_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub
                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Rewinding_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean

        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_YarnDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            'If ActiveControl.Name = dgv_YarnDetails.Name Then
            dgv1 = dgv_YarnDetails
            'ElseIf dgv_YarnDetails.IsCurrentRowDirty = True Then
            '    dgv1 = dgv_YarnDetails
            'ElseIf Pnl_Back.Enabled = True Then
            '    dgv1 = dgv_YarnDetails
            'End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                    Return True
                                Else
                                    msk_date.Focus()
                                    Return True
                                End If

                            Else

                                For I = .CurrentCell.ColumnIndex + 1 To .Columns.Count - 1
                                    If .Columns(I).Visible And .Columns(I).ReadOnly = False Then
                                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                        Return True
                                    End If
                                Next

                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                Return True

                            End If

                        Else

                            If .CurrentCell.ColumnIndex = 7 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(11)
                            Else
                                '.CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                For I = .CurrentCell.ColumnIndex + 1 To .Columns.Count - 1
                                    If .Columns(I).Visible And .Columns(I).ReadOnly = False Then
                                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                        Return True
                                    End If
                                Next

                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                                Return True

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_Freight.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(12)
                                '.CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 4)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 11 Then

                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(7)

                        Else
                            .Focus()
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
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from Rewinding_Delivery_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Rewinding_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_DcNo.Text = dt1.Rows(0).Item("Rewinding_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Rewinding_Delivery_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                cbo_Rec_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("ReceivedFrom_IdNo").ToString))
                txt_Party_DcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                cbo_TransportName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))

                cbo_Vechile.Text = dt1.Rows(0).Item("Vechile_No").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString

                If Val(dt1.Rows(0).Item("Freight").ToString) <> 0 Then
                    txt_Freight.Text = Val(dt1.Rows(0).Item("Freight").ToString)
                End If

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                cbo_Cone_Type.Text = Common_Procedures.Conetype_IdNoToName(con, Val(dt1.Rows(0).Item("Cone_type_idno").ToString))
                txt_Empty_Bag_Weight.Text = dt1.Rows(0).Item("Empty_Bag_Weight").ToString
                txt_Empty_Cone_Weight.Text = dt1.Rows(0).Item("Empty_Cone_Weight").ToString

                cbo_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Rewinding_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Rewinding_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_YarnDetails.Rows.Clear()
                SNo = 0
                With dgv_YarnDetails

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = dgv_YarnDetails.Rows.Add()

                            SNo = SNo + 1
                            dgv_YarnDetails.Rows(n).Cells(0).Value = Val(SNo)
                            dgv_YarnDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                            dgv_YarnDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Yarn_Type").ToString
                            dgv_YarnDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Mill_Name").ToString
                            dgv_YarnDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                            dgv_YarnDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                            dgv_YarnDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            dgv_YarnDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Set_No").ToString
                            .Rows(n).Cells(8).Value = dt2.Rows(i).Item("Rewinding_Delivery_SlNo").ToString
                            .Rows(n).Cells(9).Value = dt2.Rows(i).Item("Delivery_Weight").ToString
                            .Rows(n).Cells(10).Value = dt2.Rows(i).Item("Reference_Code").ToString
                            .Rows(n).Cells(11).Value = dt2.Rows(i).Item("Gross_Weight").ToString
                            .Rows(n).Cells(12).Value = dt2.Rows(i).Item("Tare_Weight").ToString
                            .Rows(n).Cells(13).Value = Common_Procedures.YarnLotEntryReferenceCode_to_LotCodeSelection(con, dt2.Rows(i).Item("Lot_Entry_ReferenceCode").ToString)


                            If Trim(.Rows(n).Cells(9).Value) <> 0 Then
                                For j = 0 To .ColumnCount - 1
                                    .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If


                        Next i

                    End If

                    With dgv_YarnDetails_Total
                        If .RowCount = 0 Then .Rows.Add()
                        .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                        .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                        .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    End With

                    dt2.Clear()

                    dt2.Dispose()
                    da2.Dispose()
                End With
            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

            If LockSTS = True Then

                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray

                cbo_Rec_Ledger.Enabled = False
                cbo_Rec_Ledger.BackColor = Color.LightGray

                cbo_TransportName.Enabled = False
                cbo_TransportName.BackColor = Color.LightGray

                cbo_Vechile.Enabled = False
                cbo_Vechile.BackColor = Color.LightGray

                txt_Party_DcNo.Enabled = False
                txt_Party_DcNo.BackColor = Color.LightGray

                txt_Freight.Enabled = False
                txt_Freight.BackColor = Color.LightGray

                cbo_Grid_CountName.Enabled = False
                cbo_Grid_CountName.BackColor = Color.LightGray

                cbo_Grid_MillName.Enabled = False
                cbo_Grid_MillName.BackColor = Color.LightGray

            End If
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
        Dim vOrdByNo As String = ""

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Rewinding_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Rewinding_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Rewinding_Delivery_Entry, New_Entry, Me, con, "Rewinding_Delivery_Head", "Rewinding_Delivery_Code", NewCode, "Rewinding_Delivery_Date", "(Rewinding_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

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


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select sum(Delivery_Weight) from Rewinding_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Some Yarns Delivered for this order", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()



        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", " Rewinding_Delivery_head", " Rewinding_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", " Rewinding_Delivery_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", " Rewinding_Delivery_Details", " Rewinding_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Yarn_Type, Mill_IdNo,count_idno, Bags, Cones, Weight , Set_No,Reference_Code", "Sl_No", " Rewinding_Delivery_Code, For_OrderBy, Company_IdNo,  Rewinding_Delivery_No,  Rewinding_Delivery_Date, Ledger_Idno", trans)

            cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = a.Delivered_Bags - b.Bags, Delivered_Cones = a.Delivered_Cones - b.Cones, Delivered_Weight = a.Delivered_Weight - b.Weight from Stock_BabyCone_Processing_Details a, Rewinding_Delivery_Details b Where b.Rewinding_Delivery_Code = '" & Trim(NewCode) & "' and a.Reference_Code = b.Reference_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from Rewinding_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Rewinding_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code = '" & Trim(NewCode) & "'"
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
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select count_name from count_head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select Mill_name from Mill_head order by Mill_name", con)
            da.Fill(dt2)
            cbo_Filter_MillName.DataSource = dt2
            cbo_Filter_MillName.DisplayMember = "Mill_name"


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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da = New SqlClient.SqlDataAdapter("select top 1 Rewinding_Delivery_No from Rewinding_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Rewinding_Delivery_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As String = ""

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Rewinding_Delivery_No from Rewinding_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Rewinding_Delivery_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As String = ""

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Rewinding_Delivery_No from Rewinding_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Rewinding_Delivery_No desc", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da = New SqlClient.SqlDataAdapter("select top 1 Rewinding_Delivery_No from Rewinding_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Rewinding_Delivery_No desc", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            clear()

            New_Entry = True

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Rewinding_Delivery_Head", "Rewinding_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red
            msk_date.Text = Date.Today.ToShortDateString

            da = New SqlClient.SqlDataAdapter("select top 1 * from Rewinding_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Rewinding_Delivery_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Rewinding_Delivery_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Rewinding_Delivery_Date").ToString
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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Rewinding_Delivery_No from Rewinding_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code = '" & Trim(RecCode) & "'", con)
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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Rewinding_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Rewinding_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Rewinding_Delivery_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Rewinding_Delivery_No from Rewinding_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                    lbl_DcNo.Text = Trim(UCase(inpno))

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
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Delv_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single
        Dim EntID As String = ""
        Dim Nr As Integer = 0
        Dim Usr_ID As Integer = 0
        Dim ByCn_RefCd As String = ""
        Dim vOrdByNo As String = ""
        Dim cone_Type_Id As Integer = 0
        Dim vLOT_ENT_REFCODE As String = ""

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Rewinding_Delivery_Entry, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Rewinding_Delivery_Entry, New_Entry, Me, con, "Rewinding_Delivery_Head", "Rewinding_Delivery_Code", NewCode, "Rewinding_Delivery_Date", "(Rewinding_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Rewinding_Delivery_No desc", dtp_Date.Value.Date) = False Then Exit Sub


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

        Delv_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Delv_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If
        Rec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Rec_Ledger.Text)

        cone_Type_Id = Common_Procedures.ConeType_NameToIdNo(con, cbo_Cone_Type.Text)

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        For i = 0 To dgv_YarnDetails.RowCount - 1

            If Val(dgv_YarnDetails.Rows(i).Cells(6).Value) <> 0 Then

                YCnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(1).Value)
                If Val(YCnt_ID) = 0 Then
                    MessageBox.Show("Invalid CountName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                If Trim(dgv_YarnDetails.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(2)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                YMil_ID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(3).Value)
                If Val(YMil_ID) = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(3)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

            End If

        Next

        If Trim(txt_Party_DcNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Da = New SqlClient.SqlDataAdapter("select * from Rewinding_delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and DeliveryTo_IdNo = " & Str(Val(Delv_ID)) & " and  Party_DcNo = '" & Trim(txt_Party_DcNo.Text) & "' and Rewinding_Delivery_code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Rewinding_Delivery_code <> '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Party dc No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Party_DcNo.Enabled And txt_Party_DcNo.Visible Then txt_Party_DcNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If

        vTotYrnBags = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0
        If dgv_YarnDetails_Total.RowCount > 0 Then
            vTotYrnBags = Val(dgv_YarnDetails_Total.Rows(0).Cells(4).Value())
            vTotYrnCones = Val(dgv_YarnDetails_Total.Rows(0).Cells(5).Value())
            vTotYrnWeight = Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value())
        End If

        If Common_Procedures.settings.Sales_OrderNumber_compulsory_in_ALLEntry_Status = 1 Then
            If Trim(cbo_ClothSales_OrderCode_forSelection.Text) = "" Then
                MessageBox.Show("Invalid Sales Order No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothSales_OrderCode_forSelection.Enabled And cbo_ClothSales_OrderCode_forSelection.Visible Then cbo_ClothSales_OrderCode_forSelection.Focus()
                Exit Sub
            End If
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Rewinding_Delivery_Head", "Rewinding_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DcDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Rewinding_Delivery_Head(Rewinding_Delivery_Code, Company_IdNo, Rewinding_Delivery_No, for_OrderBy, Rewinding_Delivery_Date, DeliveryTo_IdNo,Selection_Type, ReceivedFrom_IdNo, Total_Bags, Total_Cones, Total_Weight ,  Party_DcNo , Transport_IdNo ,Vechile_No ,Freight,  user_Idno , Cone_type_idno , Empty_Bag_Weight , Empty_Cone_Weight , ClothSales_OrderCode_forSelection) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DcDate, " & Str(Val(Delv_ID)) & ", '" & Trim(cbo_Type.Text) & "' ," & Str(Val(Rec_ID)) & "  , " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & "  , '" & Trim(txt_Party_DcNo.Text) & "' , " & Str(Val(Trans_ID)) & ", '" & Trim(cbo_Vechile.Text) & "' ," & Str(Val(txt_Freight.Text)) & "," & Val(lbl_UserName.Text) & " , " & Str(Val(cone_Type_Id)) & ", " & Val(txt_Empty_Bag_Weight.Text) & " , " & Val(txt_Empty_Cone_Weight.Text) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' )"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", " Rewinding_Delivery_head", " Rewinding_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", " Rewinding_Delivery_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", " Rewinding_Delivery_Details", " Rewinding_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Yarn_Type, Mill_IdNo,count_idno, Bags, Cones, Weight , Set_No,Reference_Code", "Sl_No", " Rewinding_Delivery_Code, For_OrderBy, Company_IdNo,  Rewinding_Delivery_No,  Rewinding_Delivery_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Rewinding_Delivery_Head set Rewinding_Delivery_Date = @DcDate, DeliveryTo_IdNo = " & Str(Val(Delv_ID)) & ",Selection_Type ='" & Trim(cbo_Type.Text) & "' , ReceivedFrom_IdNo = " & Str(Val(Rec_ID)) & "  ,  Total_Bags = " & Str(Val(vTotYrnBags)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & "  , Party_DcNo =  '" & Trim(txt_Party_DcNo.Text) & "'  , Freight = " & Str(Val(txt_Freight.Text)) & " ,  Vechile_No = '" & Trim(cbo_Vechile.Text) & "' , Transport_IdNo = " & Str(Val(Trans_ID)) & ", User_IdNo = " & Val(lbl_UserName.Text) & " , Cone_type_idno = " & Str(Val(cone_Type_Id)) & ", Empty_Bag_Weight = " & Val(txt_Empty_Bag_Weight.Text) & ", Empty_Cone_Weight = " & Val(txt_Empty_Cone_Weight.Text) & "  , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = a.Delivered_Bags - b.Bags, Delivered_Cones = a.Delivered_Cones - b.Cones, Delivered_Weight = a.Delivered_Weight - b.Weight from Stock_BabyCone_Processing_Details a, Rewinding_Delivery_Details b Where b.Rewinding_Delivery_Code = '" & Trim(NewCode) & "' and a.Reference_Code = b.Reference_Code"
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", " Rewinding_Delivery_head", " Rewinding_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", " Rewinding_Delivery_Code, Company_IdNo, for_OrderBy", tr)

            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            If Trim(lbl_DcNo.Text) <> "" Then
                Partcls = "Delv : Dc.No. " & Trim(lbl_DcNo.Text)
            End If
            PBlNo = Trim(lbl_DcNo.Text)

            cmd.CommandText = "Delete from Rewinding_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code = '" & Trim(NewCode) & "' and Delivery_Weight = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            With dgv_YarnDetails
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(6).Value) <> 0 Then

                        Sno = Sno + 1

                        YCnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        YMil_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        ByCn_RefCd = ""
                        If Trim(cbo_Type.Text) = "SELECTION" Then
                            ByCn_RefCd = Trim(.Rows(i).Cells(10).Value)
                        End If

                        vLOT_ENT_REFCODE = ""
                        If Trim(.Rows(i).Cells(13).Value) <> "" Then
                            vLOT_ENT_REFCODE = Common_Procedures.YarnLotCodeSelection_To_LotEntryReferenceCode(con, .Rows(i).Cells(13).Value, tr)
                        End If

                        Nr = 0
                        cmd.CommandText = "Update Rewinding_Delivery_Details set Rewinding_Delivery_Date = @DcDate , Ledger_IdNo = " & Str(Val(Delv_ID)) & ", Sl_No  = " & Str(Val(Sno)) & " , count_idno = " & Str(Val(YCnt_ID)) & " , Mill_IdNo = " & Str(Val(YMil_ID)) & " , Bags =  " & Str(Val(.Rows(i).Cells(4).Value)) & ", Cones = " & Str(Val(.Rows(i).Cells(5).Value)) & " ,  Weight = " & Str(Val(.Rows(i).Cells(6).Value)) & " , Set_No= '" & Trim(.Rows(i).Cells(7).Value) & "', Reference_Code = '" & Trim(ByCn_RefCd) & "' , Gross_Weight = " & Str(Val(.Rows(i).Cells(11).Value)) & " , Tare_Weight = " & Str(Val(.Rows(i).Cells(12).Value)) & " , LotCode_forSelection = '" & Trim(.Rows(i).Cells(13).Value) & "'  , Lot_Entry_ReferenceCode = '" & Trim(vLOT_ENT_REFCODE) & "' where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code = '" & Trim(NewCode) & "'  and Rewinding_Delivery_SlNo = " & Val(dgv_YarnDetails.Rows(i).Cells(8).Value)
                        Nr = cmd.ExecuteNonQuery()
                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Rewinding_Delivery_Details ( Rewinding_Delivery_Code, Company_IdNo, Rewinding_Delivery_No, for_OrderBy, Rewinding_Delivery_Date, Ledger_IdNo, Sl_No,  Yarn_Type, Mill_IdNo,count_idno, Bags, Cones, Weight , Set_No,Reference_Code , Gross_Weight , Tare_Weight, LotCode_forSelection, Lot_Entry_ReferenceCode) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DcDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(YCnt_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", '" & Trim(.Rows(i).Cells(7).Value) & "','" & Trim(ByCn_RefCd) & "' , " & Str(Val(.Rows(i).Cells(11).Value)) & " , " & Str(Val(.Rows(i).Cells(12).Value)) & ", '" & Trim(.Rows(i).Cells(13).Value) & "' , '" & Trim(vLOT_ENT_REFCODE) & "' )"
                            cmd.ExecuteNonQuery()
                        End If

                        If Trim(ByCn_RefCd) <> "" Then
                            Nr = 0
                            cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = Delivered_Bags + " & Str(Val(.Rows(i).Cells(4).Value)) & ", Delivered_Cones = Delivered_Cones + " & Str(Val(.Rows(i).Cells(5).Value)) & " ,Delivered_Weight = Delivered_Weight + " & Str(Val(.Rows(i).Cells(6).Value)) & " Where Reference_code = '" & Trim(ByCn_RefCd) & "'  and DeliveryTo_IdNo = " & Str(Val(Rec_ID))
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Babycone andParty Details")
                                Exit Sub
                            End If
                        End If

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ,LotCode_forSelection, Lot_Entry_ReferenceCode , ClothSales_OrderCode_forSelection  ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DcDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " , '" & Trim(.Rows(i).Cells(13).Value) & "' , '" & Trim(vLOT_ENT_REFCODE) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "')"
                        cmd.ExecuteNonQuery()

                    End If

                Next

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", " Rewinding_Delivery_Details", " Rewinding_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Yarn_Type, Mill_IdNo,count_idno, Bags, Cones, Weight , Set_No,Reference_Code", "Sl_No", " Rewinding_Delivery_Code, For_OrderBy, Company_IdNo,  Rewinding_Delivery_No,  Rewinding_Delivery_Date, Ledger_Idno", tr)

            End With

            If Val(vTotYrnBags) <> 0 Or Val(vTotYrnCones) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Yarn_Bags, Yarn_Cones ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DcDate, 0, " & Str(Val(Delv_ID)) & ", '" & Trim(PBlNo) & "', 1,  " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ")"
                cmd.ExecuteNonQuery()
            End If

            Dim vVou_LedIdNos As String = "", vVou_Amts As String = "", vVou_ErrMsg As String = ""

            vVou_LedIdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "Rwn.YDelv", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(msk_date.Text), Partcls, vVou_LedIdNos, vVou_Amts, vVou_ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(vVou_ErrMsg)
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_DcNo.Text)
                End If
            Else
                move_record(lbl_DcNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_Type, cbo_Rec_Ledger, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_Rec_Ledger, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "REWINDING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Rec_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Rec_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1)  or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Rec_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Rec_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Rec_Ledger, cbo_Ledger, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1)  or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Rec_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Rec_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Rec_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "SELECTION" Then
                If MessageBox.Show("Do you want to select Baby Cone :", "FOR BABY CONE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)
                Else
                    txt_Party_DcNo.Focus()
                End If

            Else
                txt_Party_DcNo.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Rec_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Rec_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Sizing_Creation
            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Rec_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub txt_Party_DcNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Party_DcNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEndEdit
        TotalYarnTaken_Calculation()
        If dgv_YarnDetails.CurrentRow.Cells(2).Value = "MILL" Then
            If dgv_YarnDetails.CurrentCell.ColumnIndex = 4 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 5 Then
                get_MillCount_Details()
            End If
        End If
    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEnter

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_YarnDetails

            If Val(.Rows(e.RowIndex).Cells(8).Value) = 0 Then
                Set_Max_DetailsSlNo(e.RowIndex, 8)
                'If e.RowIndex = 0 Then
                '    .Rows(e.RowIndex).Cells(15).Value = 1
                'Else
                '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
                'End If
            End If
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(.CurrentRow.Cells(2).Value) = "" Then
                .CurrentRow.Cells(2).Value = "MILL"
            End If

            If e.ColumnIndex = 1 And (Val(.Rows(.CurrentCell.RowIndex).Cells(9).Value)) = 0 Then

                If cbo_Grid_CountName.Visible = False Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                    cbo_Grid_CountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_CountName.DataSource = Dt1
                    cbo_Grid_CountName.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_CountName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_CountName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Grid_CountName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_CountName.Height = rect.Height  ' rect.Height
                    cbo_Grid_CountName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_CountName.Tag = Val(e.RowIndex)
                    cbo_Grid_CountName.Visible = True

                    cbo_Grid_CountName.BringToFront()
                    cbo_Grid_CountName.Focus()


                End If


            Else

                cbo_Grid_CountName.Visible = False

            End If

            If e.ColumnIndex = 2 And (Val(.Rows(.CurrentCell.RowIndex).Cells(9).Value)) = 0 Then

                If cbo_Grid_YarnType.Visible = False Or Val(cbo_Grid_YarnType.Tag) <> e.RowIndex Then

                    cbo_Grid_YarnType.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head order by Yarn_Type", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_YarnType.DataSource = Dt2
                    cbo_Grid_YarnType.DisplayMember = "Yarn_Type"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_YarnType.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_YarnType.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_YarnType.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_YarnType.Height = rect.Height  ' rect.Height

                    cbo_Grid_YarnType.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_YarnType.Tag = Val(e.RowIndex)
                    cbo_Grid_YarnType.Visible = True

                    cbo_Grid_YarnType.BringToFront()
                    cbo_Grid_YarnType.Focus()

                End If

            Else

                cbo_Grid_YarnType.Visible = False

            End If

            If e.ColumnIndex = 3 And (Val(.Rows(.CurrentCell.RowIndex).Cells(9).Value)) = 0 Then

                If cbo_Grid_MillName.Visible = False Or Val(cbo_Grid_MillName.Tag) <> e.RowIndex Then

                    cbo_Grid_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_MillName.DataSource = Dt3
                    cbo_Grid_MillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_MillName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_MillName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_MillName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_MillName.Height = rect.Height  ' rect.Height

                    cbo_Grid_MillName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_MillName.Tag = Val(e.RowIndex)
                    cbo_Grid_MillName.Visible = True

                    cbo_Grid_MillName.BringToFront()
                    cbo_Grid_MillName.Focus()

                End If

            Else

                cbo_Grid_MillName.Visible = False

            End If

            If e.ColumnIndex = 13 Then

                If cbo_Grid_Yarn_LotNo.Visible = False Or Val(cbo_Grid_Yarn_LotNo.Tag) <> e.RowIndex Then

                    cbo_Grid_Yarn_LotNo.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select LotCode_forSelection from Yarn_Lot_Head " &
                                                      "where Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_YarnDetails.CurrentRow.Cells(1).Value & "') " &
                                                      " and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_YarnDetails.CurrentRow.Cells(3).Value & "') order by LotCode_forSelection", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_YarnType.DataSource = Dt2
                    cbo_Grid_YarnType.DisplayMember = "LotCode_forSelection"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Yarn_LotNo.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_Yarn_LotNo.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_Yarn_LotNo.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_Yarn_LotNo.Height = rect.Height  ' rect.Height

                    cbo_Grid_Yarn_LotNo.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_Yarn_LotNo.Tag = Val(e.RowIndex)
                    cbo_Grid_Yarn_LotNo.Visible = True

                    cbo_Grid_Yarn_LotNo.BringToFront()
                    cbo_Grid_Yarn_LotNo.Focus()

                End If

            Else

                cbo_Grid_Yarn_LotNo.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_YarnDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellLeave
        With dgv_YarnDetails



            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
        With dgv_YarnDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                    TotalYarnTaken_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnDetails.EditingControlShowing
        dgtxt_Details = CType(dgv_YarnDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_YarnDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyDown

        With dgv_YarnDetails

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 1 Then
                    .CurrentCell.Selected = False
                    txt_Party_DcNo.Focus()
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 1 Then
                    .CurrentCell.Selected = False
                    txt_Party_DcNo.Focus()
                    'SendKeys.Send("{RIGHT}")
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    End If

                Else
                    SendKeys.Send("{Tab}")

                End If


            End If

        End With


    End Sub

    Private Sub dgv_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyUp




        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_YarnDetails

                .Rows.RemoveAt(.CurrentRow.Index)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With
        End If

    End Sub

    Private Sub dgv_YarnDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_YarnDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_YarnDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
        With dgv_YarnDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
            .Rows(n - 1).Cells(2).Value = "MILL"
            If Val(.Rows(e.RowIndex).Cells(8).Value) = 0 Then
                Set_Max_DetailsSlNo(e.RowIndex, 8)
            End If
        End With
    End Sub

    Private Sub get_MillCount_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As Single
        Dim Wgt_Bag As Single
        Dim Wgt_Cn As Single
        Dim CntID As Integer
        Dim MilID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(3).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_YarnDetails

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        Wgt_Bag = Dt.Rows(0).Item("Weight_Bag").ToString
                        Wgt_Cn = Dt.Rows(0).Item("Weight_Cone").ToString
                        Cn_bag = Dt.Rows(0).Item("Cones_Bag").ToString
                    End If
                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    If .CurrentCell.ColumnIndex = 4 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = .Rows(.CurrentRow.Index).Cells(4).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(4).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 5 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(.Rows(.CurrentRow.Index).Cells(5).Value * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

    End Sub

    Private Sub TotalYarnTaken_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(6).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(4).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(5).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(6).Value)
                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBags)
            .Rows(0).Cells(5).Value = Val(TotCones)
            .Rows(0).Cells(6).Value = Format(Val(TotWeight), "########0.000")
        End With

    End Sub

    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

    End Sub
    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_YarnType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_YarnType, cbo_Grid_CountName, cbo_Grid_MillName, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With


    End Sub

    Private Sub cbo_Grid_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_YarnType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_YarnType, cbo_Grid_MillName, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")
        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub
    Private Sub cbo_Grid_YarnType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.TextChanged
        Try
            If cbo_Grid_YarnType.Visible Then
                With dgv_YarnDetails
                    If Val(cbo_Grid_YarnType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_YarnType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, cbo_Grid_YarnType, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With



    End Sub

    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_Grid_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Grid_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.TextChanged
        Try
            If cbo_Grid_MillName.Visible Then
                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
                With dgv_YarnDetails
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, cbo_Grid_YarnType, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        With dgv_YarnDetails
            With dgv_YarnDetails

                If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    If .CurrentCell.RowIndex = 0 Then

                        If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                            cbo_ClothSales_OrderCode_forSelection.Focus()
                        ElseIf txt_Empty_Cone_Weight.Enabled And txt_Empty_Cone_Weight.Visible = True Then
                            txt_Empty_Cone_Weight.Focus()
                        Else
                            txt_Freight.Focus()
                        End If
                    Else
                            .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 12)
                        '.CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 4)
                    End If
                End If
                If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End With
    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With
        End If

    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then
                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
                With dgv_YarnDetails
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_YarnDetails.EditingControl.BackColor = Color.Lime
        dgv_YarnDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_YarnDetails
            If e.KeyValue = Keys.Delete Then
                If Val(.Rows(.CurrentCell.RowIndex).Cells(9).Value) <> 0 Then
                    e.Handled = True
                End If
            End If
        End With
    End Sub


    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_YarnDetails
            If .Visible Then

                If Val(.Rows(.CurrentCell.RowIndex).Cells(9).Value) <> 0 Then
                    e.Handled = True
                End If

                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
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
        Dim n As Integer
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Rewinding_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Rewinding_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Rewinding_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Rewinding_Delivery_Code IN ( select z1.Rewinding_Delivery_Code from Rewinding_Delivery_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & " )"
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ""
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Rewinding_Delivery_Code IN ( select z2.Rewinding_Delivery_Code from Rewinding_Delivery_Details z2 where z2.Mill_IdNo = " & Str(Val(Mil_IdNo)) & " )"
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Mill_IdNo = " & Str(Val(Mil_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Rewinding_Delivery_Head a inner join Ledger_head e on a.DeliveryTo_IdNo = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Rewinding_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Rewinding_Delivery_Date, a.for_orderby, a.Rewinding_Delivery_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Rewinding_Delivery_Head a left outer join Rewinding_Delivery_Details b on a.Rewinding_Delivery_Code = b.Rewinding_Delivery_Code left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Rewinding_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Rewinding_Delivery_Date, a.for_orderby, a.Rewinding_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Rewinding_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Rewinding_Delivery_Date").ToString), "dd-MM-yyyy")
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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) )", "(Ledger_idno = 0)")
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
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Rewinding_Delivery_Entry, New_Entry) = False Then Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Rewinding_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        prn_InpOpts = ""
        If Val(Common_Procedures.settings.All_Delivery_Print_Ori_Dup_Trip_Sts) = 1 Then

            If EMAIL_Status = True Or WHATSAPP_Status = True Then
                prn_InpOpts = "1"  ' "123"
            Else
                prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "123")
                prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")

            End If
        End If


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
        prn_Count = 0


        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as Ledger1_Name , e.Transport_Name  from Rewinding_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_IdNo = c.Ledger_IdNo Left Outer JOIN Ledger_Head d ON a.ReceivedFrom_IdNo = d.Ledger_IdNo Left Outer JOIN Transport_Head e ON a.Transport_IdNo = e.Transport_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Rewinding_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Rewinding_Delivery_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Rewinding_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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

        'If PpSzSTS = False Then

        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        'If PpSzSTS = False Then
        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next
        'End If

        'End If

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

        pFont = New Font("Calibri", 10, FontStyle.Regular)

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


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
            NoofItems_PerPage = 30

        Else

            NoofItems_PerPage = 6

        End If



        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(45) : ClAr(2) = 70 : ClAr(3) = 220 : ClAr(4) = 150 : ClAr(5) = 65 : ClAr(6) = 65 : ClAr(7) = 70
        ClAr(8) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

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

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_No").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, 0, pFont)

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
        Dim S As String

        PageNo = PageNo + 1

        CurY = TMargin

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Rewinding_Delivery_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Rewinding_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        'da2.Fill(dt2)

        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        If PageNo <= 1 Then
            prn_Count = prn_Count + 1
        End If

        prn_OriDupTri = ""

        'If String.IsNullOrEmpty(prn_InpOpts) Then prn_InpOpts = "1"

        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

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

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
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
        Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Rewinding_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "YARN", LMargin + ClAr(1), CurY - 10, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY + TxtHgt / 2, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CONE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

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
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 1, 0, pFont)

        End If

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

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, " Vehicle No : ", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 5

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
        If Val(prn_PageNo) > 1 Or is_LastPage = False Then
            CurY = CurY + 5
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Page No. : " & prn_PageNo, LMargin, CurY, 2, PageWidth, p1Font)
        End If
    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Yarn_Delivery_Head", "Vechile_No", "", "")

    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, txt_Party_DcNo, cbo_TransportName, "Weaver_Yarn_Delivery_Head", "Vechile_No", "", "")

    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vechile, cbo_TransportName, "Weaver_Yarn_Delivery_Head", "Vechile_No", "", "", False)

    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then

            If cbo_Cone_Type.Enabled And cbo_Cone_Type.Visible = True Then
                cbo_Cone_Type.Focus()
            Else

                If dgv_YarnDetails.Rows.Count > 0 Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                Else
                    btn_save.Focus()

                End If
            End If
        End If

    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If cbo_Cone_Type.Enabled And cbo_Cone_Type.Visible = True Then
                cbo_Cone_Type.Focus()
            Else
                If dgv_YarnDetails.Rows.Count > 0 Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                Else
                    btn_save.Focus()

                End If
            End If
        End If
    End Sub

    Private Sub cbo_TransportName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transportname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, cbo_Vechile, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transportname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportName, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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
    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_YarnDetails_KeyUp(sender, e)
        End If
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_date, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Ledger, "", "", "", "")


    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Wgt As Single = 0
        Dim Ent_Bg As Integer = 0
        Dim Ent_cne As Integer = 0
        Dim Ent_DcDetSlNo As Long
        Dim RecFrm_Ledger_Cond = ""

        If Trim(cbo_Type.Text) <> "SELECTION" Then
            MessageBox.Show("Invalid Entry Type", "DOES NOT SELECT BABY CONE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
            Exit Sub
        End If

        LedNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Rec_Ledger.Text)

        If LedNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT BABY CONE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Rec_Ledger.Enabled And cbo_Rec_Ledger.Visible Then cbo_Rec_Ledger.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        'If Trim(Common_Procedures.settings.CustomerCode) <> "1464" Then ' -- MOF
        '    RecFrm_Ledger_Cond = "(a.DeliveryTo_IdNo  = " & Str(Val(LedNo)) & ")"
        '    End IF 

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()
            SNo = 0


            Da = New SqlClient.SqlDataAdapter("select a.*, b.Weight as Ent_DcWeight,  b.Cones as Ent_DcCones ,  b.Bags as Ent_DcBags , b.Rewinding_Delivery_SlNo as Ent_Rewinding_Delivery_SlNo , c.Count_Name, d.Mill_Name from Stock_BabyCone_Processing_Details a LEFT OUTER JOIN Rewinding_Delivery_Details b ON b.Rewinding_delivery_Code = '" & Trim(NewCode) & "' and a.Reference_Code = b.Reference_Code LEFT OUTER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head d ON a.Mill_Idno = d.Mill_Idno where  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & "  a.yarn_type = 'BABY' and a.DeliveryTo_IdNo  = " & Str(Val(LedNo)) & " and ((a.Baby_Weight - a.Delivered_Weight) > 0 or b.Weight > 0 )  order by a.for_orderby, a.Set_Code", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()
                    Ent_Wgt = 0
                    Ent_Bg = 0
                    Ent_cne = 0

                    Ent_DcDetSlNo = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_DcWeight").ToString) = False Then
                        Ent_Wgt = Val(Dt1.Rows(i).Item("Ent_DcWeight").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_DcCones").ToString) = False Then
                        Ent_cne = Val(Dt1.Rows(i).Item("Ent_DcCones").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_DcBags").ToString) = False Then
                        Ent_Bg = Val(Dt1.Rows(i).Item("Ent_DcBags").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Rewinding_Delivery_SlNo").ToString) = False Then
                        Ent_DcDetSlNo = Val(Dt1.Rows(i).Item("Ent_Rewinding_Delivery_SlNo").ToString)
                    End If

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Yarn_Type").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Mill_Name").ToString
                    .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Baby_Bags").ToString) - Val(Dt1.Rows(i).Item("Delivered_Bags").ToString) + Val(Ent_Bg)
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Baby_Cones").ToString) - Val(Dt1.Rows(i).Item("Delivered_Cones").ToString) + Val(Ent_cne)
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Baby_Weight").ToString) - Val(Dt1.Rows(i).Item("Delivered_Weight").ToString) + Val(Ent_Wgt), "#########0.00")

                    If Ent_Wgt > 0 Then
                        .Rows(n).Cells(8).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else

                        .Rows(n).Cells(8).Value = ""

                    End If

                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Reference_Code").ToString
                    .Rows(n).Cells(10).Value = Ent_DcDetSlNo
                    .Rows(n).Cells(11).Value = Ent_Wgt
                    .Rows(n).Cells(12).Value = Ent_cne
                    .Rows(n).Cells(13).Value = Ent_Bg

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        Pnl_Back.Enabled = False
        If dgv_Selection.Rows.Count > 0 Then
            dgv_Selection.Focus()
            dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        End If

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Baby(e.RowIndex)
    End Sub

    Private Sub Select_Baby(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                'If Val(.Rows(RwIndx).Cells(8).Value) > 0 And Val(.Rows(RwIndx).Cells(8).Value) <> Val(.Rows(RwIndx).Cells(10).Value) Then
                '    MessageBox.Show("Cannot deselect" & Chr(13) & "Already this pavu delivered to others")
                '    Exit Sub
                'End If

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
        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                Select_Baby(dgv_Selection.CurrentCell.RowIndex)
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        BabyCone_Selection()
    End Sub
    Private Sub BabyCone_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_YarnDetails.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                n = dgv_YarnDetails.Rows.Add()
                sno = sno + 1
                dgv_YarnDetails.Rows(n).Cells(0).Value = Val(sno)
                dgv_YarnDetails.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_YarnDetails.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_YarnDetails.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                'dgv_YarnDetails.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
                'dgv_YarnDetails.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                ''dgv_YarnDetails.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_YarnDetails.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_YarnDetails.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_YarnDetails.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(11).Value
                dgv_YarnDetails.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(9).Value



                If Val(dgv_Selection.Rows(i).Cells(11).Value) <> 0 Then
                    dgv_YarnDetails.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(11).Value
                Else
                    dgv_YarnDetails.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(12).Value) <> 0 Then
                    dgv_YarnDetails.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(12).Value
                Else
                    dgv_YarnDetails.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(13).Value) <> 0 Then
                    dgv_YarnDetails.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(13).Value
                Else
                    dgv_YarnDetails.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
                End If

            End If
        Next
        For i = 0 To dgv_YarnDetails.Rows.Count - 1
            If Val(dgv_YarnDetails.Rows(i).Cells(8).Value) = 0 Then
                Set_Max_DetailsSlNo(i, 8)
            End If
        Next
        TotalYarnTaken_Calculation()

        Pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_Party_DcNo.Enabled And txt_Party_DcNo.Visible Then txt_Party_DcNo.Focus()

    End Sub

    Private Sub Set_Max_DetailsSlNo(ByVal RowNo As Integer, ByVal DetSlNo_ColNo As Integer)
        Dim MaxSlNo As Integer = 0
        Dim i As Integer

        'With dgv_YarnDetails
        '    For i = 0 To .Rows.Count - 1
        '        If Val(.Rows(i).Cells(DetSlNo_ColNo).Value) > Val(MaxSlNo) Then
        '            MaxSlNo = Val(.Rows(i).Cells(DetSlNo_ColNo).Value)
        '        End If
        '    Next
        '    .Rows(RowNo).Cells(DetSlNo_ColNo).Value = Val(MaxSlNo) + 1
        'End With

    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_date.Text = Date.Today
        'End If
        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
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
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_coneType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cone_Type.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cone_Type, txt_Freight, txt_Empty_Bag_Weight, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")

    End Sub

    Private Sub cbo_coneType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cone_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cone_Type, txt_Empty_Bag_Weight, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")

    End Sub

    Private Sub cbo_coneType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cone_Type.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New ConeType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cone_Type.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_Empty_Bag_Weight_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Empty_Bag_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            txt_Empty_Cone_Weight.Focus()
        End If


    End Sub

    Private Sub txt_Empty_Bag_Weight_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Empty_Bag_Weight.KeyDown

        If e.KeyCode = 40 Then
            txt_Empty_Cone_Weight.Focus()
        End If

        If e.KeyCode = 38 Then
            cbo_Cone_Type.Focus()
        End If

    End Sub

    Private Sub txt_Empty_Cone_Weight_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Empty_Cone_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()

            Else

                If dgv_YarnDetails.Rows.Count > 0 Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                Else
                    btn_save.Focus()

                End If

            End If
        End If

    End Sub

    Private Sub txt_Empty_Cone_Weight_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Empty_Cone_Weight.KeyDown
        If (e.KeyValue = 40) Then

            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()

            Else

                If dgv_YarnDetails.Rows.Count > 0 Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                Else
                    btn_save.Focus()

                End If

            End If

        End If


        If e.KeyValue = 38 Then
            txt_Empty_Bag_Weight.Focus()
        End If
    End Sub


    Private Sub cbo_Cone_Type_GotFocus(sender As Object, e As EventArgs) Handles cbo_Cone_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")
    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_Enter(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_LotNo.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Lot_Head", "LotCode_forSelection", "Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_YarnDetails.CurrentRow.Cells(1).Value & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_YarnDetails.CurrentRow.Cells(3).Value & "')", "(Lot_No = '')")
    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_LotNo.TextChanged

        Try
            If cbo_Grid_Yarn_LotNo.Visible Then

                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

                With dgv_YarnDetails
                    If Val(cbo_Grid_Yarn_LotNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 13 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(13).Value = Trim(cbo_Grid_Yarn_LotNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Yarn_LotNo, Nothing, Nothing, "Yarn_Lot_Head", "LotCode_forSelection", "Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_YarnDetails.CurrentRow.Cells(1).Value & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_YarnDetails.CurrentRow.Cells(3).Value & "')", "(Lot_No = '')")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_Yarn_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Yarn_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                If .CurrentCell.ColumnIndex < .ColumnCount - 1 Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    If .CurrentRow.Index < .RowCount - 1 Then
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Yarn_LotNo, Nothing, "Yarn_Lot_Head", "LotCode_forSelection", "Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_YarnDetails.CurrentRow.Cells(1).Value & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_YarnDetails.CurrentRow.Cells(3).Value & "')", "(Lot_No = '')")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Focus()
                If .CurrentCell.ColumnIndex < .ColumnCount - 1 Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    If .CurrentRow.Index < .RowCount - 1 Then
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)
                    End If
                End If

            End With

        End If

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyUp

        If e.Control = False And e.KeyValue = 17 Then

            Dim f As New Yarn_Lot_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Yarn_LotNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If Asc(e.KeyChar) = 13 Then

            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If

        End If





    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If (e.KeyCode = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            txt_Empty_Cone_Weight.Focus()
        End If

        If (e.KeyCode = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub txt_Empty_Cone_Weight_TextChanged(sender As Object, e As EventArgs) Handles txt_Empty_Cone_Weight.TextChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.SelectedIndexChanged

    End Sub

    Private Sub cbo_Grid_CountName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Grid_CountName.SelectedIndexChanged

    End Sub

    Private Sub lbl_ClothSales_OrderCode_forSelection_Caption_Click(sender As Object, e As EventArgs) Handles lbl_ClothSales_OrderCode_forSelection_Caption.Click

    End Sub
End Class