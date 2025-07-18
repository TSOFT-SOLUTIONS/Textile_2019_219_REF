Imports System.Drawing.Printing

Public Class Rewinding_Receipt
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "RWREC-"
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

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Enum Dgv_ColDetails As Integer

        'SLNO '= 0
        'COUNT_NAME '= 2
        'MILL_NAME '= 3
        'SET_NO '= 4
        'BAGS '= 5
        'CONES '= 6
        'NET_WEIGHT '= 7
        'EXCESS_SHORT '= 8
        'REWINDING_DELIVERY_CODE '= 8
        'REWINDING_DELIVERY_SLNO '= 9
        'GROSS_WEIGHT '= 10
        'TARE_WEIGHT '= 11
        'LOT_NO '= 12


        SLNO                            '0
        COUNT_NAME                      '1
        MILL_NAME                       '2
        SET_NO                          '3
        BAGS                            '4
        BAG_GRAMS                       '5
        CONES                           '6
        CONE_WEIGHT                     '7
        GROSS_WEIGHT                    '8
        TARE_WEIGHT                     '9
        NET_WEIGHT                      '10
        RATE                            '11    
        AMOUNT                          '12    
        WASTE_TOLERANCE                 '13    
        EXCESS_SHORT                    '14
        LOT_NO                          '15
        REWINDING_DELIVERY_CODE         '16
        REWINDING_DELIVERY_SLNO         '17
        REWINDING_DELIVERY_WEIGHT       '18



    End Enum


    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Visible = True
        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_DelvTo.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
        cbo_RecFrom.Text = ""
        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""
        cbo_EntryType.Text = "SELECTION"
        cbo_TransportName.Text = ""

        cbo_Filter_CountName.Text = ""
        cbo_Filter_MillName.Text = ""
        cbo_Filter_PartyName.Text = ""
        txt_Party_DcNo.Text = ""
        txt_BillAmt.Text = ""
        txt_BillNo.Text = ""
        txt_Empty_Gunnies.Text = ""
        txt_Empty_Cones.Text = ""
        txt_Freight.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Waste_Tolerance.Text = ""
        txt_Waste_Tolerance_Caption.Text = "Waste Tolerance"

        dgv_YarnDetails.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

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

    Private Sub Rewinding_Receipt_Activated1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DelvTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DelvTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RecFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_RecFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub Rewinding_Receipt_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Rewinding_Receipt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

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

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0  or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_DelvTo.DataSource = dt1
        cbo_DelvTo.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'REWINDING' ) order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_RecFrom.DataSource = dt2
        cbo_RecFrom.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_TransportName.DataSource = dt3
        cbo_TransportName.DisplayMember = "Ledger_DisplayName"


        da = New SqlClient.SqlDataAdapter("select distinct(Vechile_No) from Rewinding_Receipt_Head order by Vechile_No", con)
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

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False

        cbo_EntryType.Items.Clear()
        cbo_EntryType.Items.Add(" ")
        cbo_EntryType.Items.Add("DIRECT")
        cbo_EntryType.Items.Add("SELECTION")

        dtp_Date.Text = ""
        msk_date.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        dgv_YarnDetails.Columns(Dgv_ColDetails.CONE_WEIGHT).Visible = False
        dgv_YarnDetails.Columns(Dgv_ColDetails.BAG_GRAMS).Visible = False
        dgv_YarnDetails.Columns(Dgv_ColDetails.WASTE_TOLERANCE).Visible = False
        dgv_YarnDetails.Columns(Dgv_ColDetails.REWINDING_DELIVERY_WEIGHT).Visible = False
        'dgv_YarnDetails.Columns(Dgv_ColDetails.RATE).Visible = False
        'dgv_YarnDetails.Columns(Dgv_ColDetails.AMOUNT).Visible = False

        dgv_YarnDetails_Total.Columns(Dgv_ColDetails.CONE_WEIGHT).Visible = False
        dgv_YarnDetails_Total.Columns(Dgv_ColDetails.BAG_GRAMS).Visible = False
        dgv_YarnDetails_Total.Columns(Dgv_ColDetails.WASTE_TOLERANCE).Visible = False
        'dgv_YarnDetails_Total.Columns(Dgv_ColDetails.RATE).Visible = False
        'dgv_YarnDetails_Total.Columns(Dgv_ColDetails.AMOUNT).Visible = False



        txt_Waste_Tolerance_Caption.Visible = False
        txt_Waste_Tolerance.Visible = False

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
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

        If dgv_YarnDetails.Columns(Dgv_ColDetails.BAG_GRAMS).Visible And dgv_YarnDetails.Columns(Dgv_ColDetails.CONE_WEIGHT).Visible Then

            dgv_YarnDetails.Columns(Dgv_ColDetails.TARE_WEIGHT).ReadOnly = True

        Else

            dgv_YarnDetails.Columns(Dgv_ColDetails.TARE_WEIGHT).ReadOnly = False

        End If


        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DelvTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RecFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EntryType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Party_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillAmt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Gunnies.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Cones.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cone_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Bag_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Cone_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Yarn_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Waste_Tolerance.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DelvTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RecFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EntryType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Gunnies.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Cones.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Party_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillAmt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cone_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Bag_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Cone_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Yarn_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Waste_Tolerance.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Empty_Gunnies.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Empty_Cones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Party_DcNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Empty_Gunnies.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Empty_Cones.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Party_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus



        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True

        If Common_Procedures.settings.Show_Yarn_LotNo_Status = 1 Then
            dgv_YarnDetails.Columns(Dgv_ColDetails.LOT_NO).Visible = True
        Else
            dgv_YarnDetails.Columns(Dgv_ColDetails.LOT_NO).Visible = False
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1464" Then ' -- MOF

            dgv_YarnDetails.Columns(Dgv_ColDetails.NET_WEIGHT).HeaderText = "NET WEIGHT"

            dgv_YarnDetails.Columns(Dgv_ColDetails.CONE_WEIGHT).Visible = True
            dgv_YarnDetails.Columns(Dgv_ColDetails.BAG_GRAMS).Visible = True
            dgv_YarnDetails.Columns(Dgv_ColDetails.WASTE_TOLERANCE).Visible = True
            'dgv_YarnDetails.Columns(Dgv_ColDetails.RATE).Visible = True
            'dgv_YarnDetails.Columns(Dgv_ColDetails.AMOUNT).Visible = True


            dgv_YarnDetails_Total.Columns(Dgv_ColDetails.CONE_WEIGHT).Visible = True
            dgv_YarnDetails_Total.Columns(Dgv_ColDetails.BAG_GRAMS).Visible = True
            dgv_YarnDetails_Total.Columns(Dgv_ColDetails.WASTE_TOLERANCE).Visible = True
            'dgv_YarnDetails_Total.Columns(Dgv_ColDetails.RATE).Visible = True
            'dgv_YarnDetails_Total.Columns(Dgv_ColDetails.AMOUNT).Visible = True


            txt_Waste_Tolerance_Caption.Visible = True
            txt_Waste_Tolerance.Visible = True

        End If


        If Trim(Common_Procedures.settings.CustomerCode) <> "1464" Then  ' -- MOF

            dgv_YarnDetails.Columns(Dgv_ColDetails.CONE_WEIGHT).Visible = False
            dgv_YarnDetails.Columns(Dgv_ColDetails.BAG_GRAMS).Visible = False
            dgv_YarnDetails.Columns(Dgv_ColDetails.WASTE_TOLERANCE).Visible = False
            'dgv_YarnDetails.Columns(Dgv_ColDetails.RATE).Visible = False
            'dgv_YarnDetails.Columns(Dgv_ColDetails.AMOUNT).Visible = False


            dgv_YarnDetails_Total.Columns(Dgv_ColDetails.CONE_WEIGHT).Visible = False
            dgv_YarnDetails_Total.Columns(Dgv_ColDetails.BAG_GRAMS).Visible = False
            dgv_YarnDetails_Total.Columns(Dgv_ColDetails.WASTE_TOLERANCE).Visible = False
            'dgv_YarnDetails_Total.Columns(Dgv_ColDetails.RATE).Visible = False
            'dgv_YarnDetails_Total.Columns(Dgv_ColDetails.AMOUNT).Visible = False


            txt_Waste_Tolerance_Caption.Visible = False
            txt_Waste_Tolerance.Visible = False

            If dgv_YarnDetails.Columns(Dgv_ColDetails.LOT_NO).Visible = True Then

                dgv_YarnDetails.Columns(Dgv_ColDetails.COUNT_NAME).Width = 140
                dgv_YarnDetails.Columns(Dgv_ColDetails.MILL_NAME).Width = 250

                dgv_YarnDetails_Total.Columns(Dgv_ColDetails.COUNT_NAME).Width = 140
                dgv_YarnDetails_Total.Columns(Dgv_ColDetails.MILL_NAME).Width = 250
            Else
                dgv_YarnDetails.Columns(Dgv_ColDetails.COUNT_NAME).Width = 180
                dgv_YarnDetails.Columns(Dgv_ColDetails.MILL_NAME).Width = 340

                dgv_YarnDetails_Total.Columns(Dgv_ColDetails.COUNT_NAME).Width = 180
                dgv_YarnDetails_Total.Columns(Dgv_ColDetails.MILL_NAME).Width = 340

            End If

            dgv_YarnDetails.Columns(Dgv_ColDetails.SET_NO).Width = 100
            dgv_YarnDetails.Columns(Dgv_ColDetails.BAGS).Width = 70
            dgv_YarnDetails.Columns(Dgv_ColDetails.CONES).Width = 70
            dgv_YarnDetails.Columns(Dgv_ColDetails.LOT_NO).Width = 120

            dgv_YarnDetails_Total.Columns(Dgv_ColDetails.SET_NO).Width = 100
            dgv_YarnDetails_Total.Columns(Dgv_ColDetails.BAGS).Width = 70
            dgv_YarnDetails_Total.Columns(Dgv_ColDetails.CONES).Width = 70
            dgv_YarnDetails_Total.Columns(Dgv_ColDetails.LOT_NO).Width = 120


        End If


        new_record()

    End Sub
    Private Sub Rewinding_Receipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean

        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_YarnDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            dgv1 = dgv_YarnDetails

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                btn_save.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(Dgv_ColDetails.COUNT_NAME)

                            End If

                            'ElseIf .CurrentCell.ColumnIndex = Dgv_ColDetails.EXCESS_SHORT Then

                            '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.GROSS_WEIGHT)
                        ElseIf .CurrentCell.ColumnIndex = Dgv_ColDetails.BAGS Then
                            If dgv_YarnDetails.Columns(Dgv_ColDetails.BAG_GRAMS).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.BAG_GRAMS)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.CONES)
                            End If
                        ElseIf .CurrentCell.ColumnIndex = Dgv_ColDetails.CONES Then
                            If dgv_YarnDetails.Columns(Dgv_ColDetails.CONE_WEIGHT).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.CONE_WEIGHT)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.GROSS_WEIGHT)
                            End If
                        ElseIf .CurrentCell.ColumnIndex = Dgv_ColDetails.AMOUNT Then
                            If dgv_YarnDetails.Columns(Dgv_ColDetails.WASTE_TOLERANCE).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.WASTE_TOLERANCE)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.EXCESS_SHORT)
                            End If
                        ElseIf .CurrentCell.ColumnIndex = Dgv_ColDetails.EXCESS_SHORT Then

                            If dgv_YarnDetails.Columns(Dgv_ColDetails.LOT_NO).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.LOT_NO)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(Dgv_ColDetails.COUNT_NAME)

                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_Freight.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(Dgv_ColDetails.TARE_WEIGHT)
                                '.CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)

                            End If

                            'ElseIf .CurrentCell.ColumnIndex = Dgv_ColDetails.GROSS_WEIGHT Then

                            '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(Dgv_ColDetails.EXCESS_SHORT)

                        ElseIf .CurrentCell.ColumnIndex = Dgv_ColDetails.GROSS_WEIGHT Then

                            If dgv_YarnDetails.Columns(Dgv_ColDetails.CONE_WEIGHT).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.CONE_WEIGHT)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.CONES)
                            End If
                        ElseIf .CurrentCell.ColumnIndex = Dgv_ColDetails.CONES Then
                            If dgv_YarnDetails.Columns(Dgv_ColDetails.BAG_GRAMS).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.BAG_GRAMS)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.BAGS)
                            End If
                        ElseIf .CurrentCell.ColumnIndex = Dgv_ColDetails.EXCESS_SHORT Then

                            If dgv_YarnDetails.Columns(Dgv_ColDetails.WASTE_TOLERANCE).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(Dgv_ColDetails.WASTE_TOLERANCE)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(Dgv_ColDetails.AMOUNT)

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
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from Rewinding_Receipt_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Rewinding_Receipt_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RecNo.Text = dt1.Rows(0).Item("Rewinding_Receipt_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Rewinding_Receipt_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_DelvTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                cbo_RecFrom.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("ReceivedFrom_IdNo").ToString))
                cbo_EntryType.Text = dt1.Rows(0).Item("Receipt_Type").ToString

                cbo_TransportName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))

                cbo_Vechile.Text = dt1.Rows(0).Item("Vechile_No").ToString

                txt_Party_DcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                txt_BillAmt.Text = Format(Val(dt1.Rows(0).Item("Bill_Amount").ToString), "########0.00")

                If Val(dt1.Rows(0).Item("Empty_Gunnies").ToString) <> 0 Then
                    txt_Empty_Gunnies.Text = Val(dt1.Rows(0).Item("Empty_Gunnies").ToString)
                End If

                If Val(dt1.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                    txt_Empty_Cones.Text = Val(dt1.Rows(0).Item("Empty_Cones").ToString)
                End If

                If Val(dt1.Rows(0).Item("Freight").ToString) <> 0 Then
                    txt_Freight.Text = Val(dt1.Rows(0).Item("Freight").ToString)
                End If
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                cbo_Cone_Type.Text = Common_Procedures.Conetype_IdNoToName(con, Val(dt1.Rows(0).Item("Cone_type_idno").ToString))
                txt_Empty_Bag_Weight.Text = dt1.Rows(0).Item("Empty_Bag_Weight").ToString
                txt_Empty_Cone_Weight.Text = dt1.Rows(0).Item("Empty_Cone_Weight").ToString


                txt_Waste_Tolerance_Caption.Text = dt1.Rows(0).Item("Waste_Tolerance_Caption").ToString
                txt_Waste_Tolerance.Text = dt1.Rows(0).Item("Waste_Tolerance_Percentage").ToString

                cbo_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Rewinding_Receipt_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Rewinding_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_YarnDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_YarnDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.SLNO).Value = Val(SNo)
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.COUNT_NAME).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.MILL_NAME).Value = dt2.Rows(i).Item("Mill_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.SET_NO).Value = dt2.Rows(i).Item("Set_No").ToString
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.BAGS).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.CONES).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.NET_WEIGHT).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.EXCESS_SHORT).Value = Format(Val(dt2.Rows(i).Item("Excess_Short").ToString), "########0.000")

                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.RATE).Value = Val(dt2.Rows(i).Item("Rewinding_Rate").ToString)
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.AMOUNT).Value = Format(Val(dt2.Rows(i).Item("Rewinding_Amount").ToString), "########0.00")

                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.REWINDING_DELIVERY_CODE).Value = dt2.Rows(i).Item("Rewinding_Delivery_Code").ToString
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.REWINDING_DELIVERY_SLNO).Value = dt2.Rows(i).Item("Rewinding_Delivery_SlNo").ToString
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.GROSS_WEIGHT).Value = dt2.Rows(i).Item("Gross_Weight").ToString
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.TARE_WEIGHT).Value = dt2.Rows(i).Item("Tare_Weight").ToString
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.LOT_NO).Value = Common_Procedures.YarnLotEntryReferenceCode_to_LotCodeSelection(con, dt2.Rows(i).Item("Lot_Entry_ReferenceCode").ToString)

                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.CONE_WEIGHT).Value = Format(Val(dt2.Rows(i).Item("Empty_Cone_Weight").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.BAG_GRAMS).Value = Format(Val(dt2.Rows(i).Item("Empty_Bag_Weight").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.WASTE_TOLERANCE).Value = Format(Val(dt2.Rows(i).Item("Waste_Tolerance_Weight").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.REWINDING_DELIVERY_WEIGHT).Value = Format(Val(dt2.Rows(i).Item("Rewinding_Delivery_Weight").ToString), "########0.000")




                    Next i



                End If

                With dgv_YarnDetails_Total

                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(Dgv_ColDetails.BAGS).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(Dgv_ColDetails.CONES).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(Dgv_ColDetails.NET_WEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    .Rows(0).Cells(Dgv_ColDetails.TARE_WEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Tare_Weight").ToString), "########0.000")
                    .Rows(0).Cells(Dgv_ColDetails.GROSS_WEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Gross_Weight").ToString), "########0.000")
                    .Rows(0).Cells(Dgv_ColDetails.EXCESS_SHORT).Value = Format(Val(dt1.Rows(0).Item("Total_Ex_St").ToString), "########0.000")

                    .Rows(0).Cells(Dgv_ColDetails.CONE_WEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Empty_Cone_Weight").ToString), "########0.000")
                    .Rows(0).Cells(Dgv_ColDetails.BAG_GRAMS).Value = Format(Val(dt1.Rows(0).Item("Total_Empty_Bag_Weight").ToString), "########0.000")
                    .Rows(0).Cells(Dgv_ColDetails.WASTE_TOLERANCE).Value = Format(Val(dt1.Rows(0).Item("Total_Waste_Tolerance_Weight").ToString), "########0.000")

                    .Rows(0).Cells(Dgv_ColDetails.REWINDING_DELIVERY_WEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Rewinding_Delivery_Weight").ToString), "########0.000")
                    .Rows(0).Cells(Dgv_ColDetails.AMOUNT).Value = Format(Val(dt1.Rows(0).Item("Total_Rewinding_Amount").ToString), "########0.00")


                End With

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

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

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Rewinding_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Rewinding_Receipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Rewinding_Receipt_Entry, New_Entry, Me, con, "Rewinding_Receipt_Head", "Rewinding_Receipt_Code", NewCode, "Rewinding_Receipt_Date", "(Rewinding_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

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

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", " Rewinding_Receipt_head", " Rewinding_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", " Rewinding_Receipt_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", " Rewinding_Receipt_Details", " Rewinding_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Mill_IdNo,count_idno, Bags, Cones, Weight, Set_No , Excess_Short , Rewinding_Delivery_Code , Rewinding_Delivery_SlNo", "Sl_No", " Rewinding_Receipt_Code, For_OrderBy, Company_IdNo,  Rewinding_Receipt_No,  Rewinding_Receipt_Date, Ledger_Idno", trans)


            cmd.CommandText = "Update Rewinding_Delivery_Details set Delivery_Weight = a.Delivery_Weight - (b.Weight-b.Excess_Short + Waste_Tolerance_Weight ) from Rewinding_Delivery_Details a, Rewinding_Receipt_Details b Where b.Rewinding_Receipt_Code = '" & Trim(NewCode) & "' and a.Rewinding_Delivery_code = b.Rewinding_Delivery_code and a.Rewinding_Delivery_SlNo = b.Rewinding_Delivery_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Rewinding_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Rewinding_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code = '" & Trim(NewCode) & "'"
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

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'REWINDING') order by Ledger_DisplayName", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Rewinding_Receipt_No from Rewinding_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Rewinding_Receipt_No", con)
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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Rewinding_Receipt_No from Rewinding_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Rewinding_Receipt_No", con)
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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Rewinding_Receipt_No from Rewinding_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Rewinding_Receipt_No desc", con)
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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da = New SqlClient.SqlDataAdapter("select top 1 Rewinding_Receipt_No from Rewinding_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Rewinding_Receipt_No desc", con)
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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            clear()

            New_Entry = True

            lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Rewinding_Receipt_Head", "Rewinding_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RecNo.ForeColor = Color.Red
            msk_date.Text = Date.Today.ToShortDateString

            da = New SqlClient.SqlDataAdapter("select top 1 * from Rewinding_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Rewinding_Receipt_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Rewinding_Receipt_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Rewinding_Receipt_Date").ToString
                    If dt1.Rows(0).Item("Waste_Tolerance_Caption").ToString <> "" Then txt_Waste_Tolerance_Caption.Text = dt1.Rows(0).Item("Waste_Tolerance_Caption").ToString

                End If
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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            inpno = InputBox("Enter Rec.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Rewinding_Receipt_No from Rewinding_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Rec No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Rewinding_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Rewinding_Receipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Rewinding_Receipt_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Rec No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Rewinding_Receipt_No from Rewinding_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Rec No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RecNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single, vTotYrExSt As Single
        Dim EntID As String = ""
        Dim Nr As Integer = 0
        Dim RwDcCd As String = ""
        Dim RwDcDetSlNo As Long = 0
        Dim Usr_ID As Integer = 0
        Dim vOrdByNo As String = ""
        Dim cone_Type_id As Integer = 0
        Dim vLOT_ENT_REFCODE As String = ""
        Dim vTot_Empty_Bag_Wgt = ""
        Dim vTot_Empty_Cone_Wgt = ""
        Dim vTot_Waste_Tole_Wgt = ""
        Dim vTot_Rewind_Del_Wgt = ""
        Dim vTot_Tare_Wgt = ""
        Dim vTot_Gross_Wgt = ""
        Dim vTot_Amount = ""



        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Rewinding_Receipt_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Rewinding_Receipt_Entry, New_Entry, Me, con, "Rewinding_Receipt_Head", "Rewinding_Receipt_Code", NewCode, "Rewinding_Receipt_Date", "(Rewinding_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Rewinding_Receipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub


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

        Delv_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvTo.Text)
        If Delv_ID = 0 Then
            Delv_ID = 4
            'MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'If cbo_DelvTo.Enabled And cbo_DelvTo.Visible Then cbo_DelvTo.Focus()
            'Exit Sub
        End If

        Rec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecFrom.Text)
        If Rec_ID = 0 Then
            MessageBox.Show("Invalid Receiver Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_RecFrom.Enabled And cbo_RecFrom.Visible Then cbo_RecFrom.Focus()
            Exit Sub
        End If
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)

        cone_Type_id = Common_Procedures.ConeType_NameToIdNo(con, cbo_Cone_Type.Text)

        lbl_UserName.Text = Common_Procedures.User.IdNo
        For i = 0 To dgv_YarnDetails.RowCount - 1

            If Val(dgv_YarnDetails.Rows(i).Cells(Dgv_ColDetails.NET_WEIGHT).Value) <> 0 Then

                YCnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(Dgv_ColDetails.COUNT_NAME).Value)
                If Val(YCnt_ID) = 0 Then
                    MessageBox.Show("Invalid CountName", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(Dgv_ColDetails.COUNT_NAME)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If


                YMil_ID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(Dgv_ColDetails.MILL_NAME).Value)
                If Val(YMil_ID) = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(Dgv_ColDetails.MILL_NAME)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

            End If

        Next

        If Trim(txt_Party_DcNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Da = New SqlClient.SqlDataAdapter("select * from Rewinding_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ReceivedFrom_IdNo = " & Str(Val(Rec_ID)) & " and  Party_DcNo = '" & Trim(txt_Party_DcNo.Text) & "' and Rewinding_Receipt_code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Rewinding_Receipt_code <> '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Party dc No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Party_DcNo.Enabled And txt_Party_DcNo.Visible Then txt_Party_DcNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If
        vTotYrnBags = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0 : vTotYrExSt = 0
        vTot_Waste_Tole_Wgt = 0 : vTot_Empty_Cone_Wgt = 0 : vTot_Empty_Bag_Wgt = 0 : vTot_Rewind_Del_Wgt = 0 : vTot_Tare_Wgt = 0 : vTot_Gross_Wgt = 0 : vTot_Amount = 0
        If dgv_YarnDetails_Total.RowCount > 0 Then

            vTotYrnBags = Val(dgv_YarnDetails_Total.Rows(0).Cells(Dgv_ColDetails.BAGS).Value())
            vTotYrnCones = Val(dgv_YarnDetails_Total.Rows(0).Cells(Dgv_ColDetails.CONES).Value())
            vTotYrnWeight = Val(dgv_YarnDetails_Total.Rows(0).Cells(Dgv_ColDetails.NET_WEIGHT).Value())
            vTot_Tare_Wgt = Val(dgv_YarnDetails_Total.Rows(0).Cells(Dgv_ColDetails.TARE_WEIGHT).Value())
            vTot_Gross_Wgt = Val(dgv_YarnDetails_Total.Rows(0).Cells(Dgv_ColDetails.GROSS_WEIGHT).Value())
            vTotYrExSt = Val(dgv_YarnDetails_Total.Rows(0).Cells(Dgv_ColDetails.EXCESS_SHORT).Value())

            vTot_Empty_Bag_Wgt = Val(dgv_YarnDetails_Total.Rows(0).Cells(Dgv_ColDetails.BAG_GRAMS).Value())
            vTot_Empty_Cone_Wgt = Val(dgv_YarnDetails_Total.Rows(0).Cells(Dgv_ColDetails.CONE_WEIGHT).Value())
            vTot_Waste_Tole_Wgt = Val(dgv_YarnDetails_Total.Rows(0).Cells(Dgv_ColDetails.WASTE_TOLERANCE).Value())

            vTot_Rewind_Del_Wgt = Val(dgv_YarnDetails_Total.Rows(0).Cells(Dgv_ColDetails.REWINDING_DELIVERY_WEIGHT).Value())
            vTot_Amount = Val(dgv_YarnDetails_Total.Rows(0).Cells(Dgv_ColDetails.AMOUNT).Value())

        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Rewinding_Receipt_Head", "Rewinding_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@RecDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Rewinding_Receipt_Head(Rewinding_Receipt_Code, Company_IdNo  ,                    Rewinding_Receipt_No,                          for_OrderBy,                                 Rewinding_Receipt_Date,         Receipt_Type,                    DeliveryTo_IdNo   ,       ReceivedFrom_IdNo ,                      Party_DcNo,                 Total_Bags,                         Total_Cones,                Total_Weight ,                     Total_Ex_St ,                    Bill_No ,                      Bill_Amount ,                           Empty_Gunnies,                               Empty_Cones ,                           Vechile_No,                Transport_IdNo ,                     Freight  ,                         User_idNo ,                  Cone_type_idno ,                        Empty_Bag_Weight ,                     Empty_Cone_Weight       ,           Waste_Tolerance_Caption             ,       Waste_Tolerance_Percentage          ,       Total_Waste_Tolerance_Weight    ,       Total_Empty_Bag_Weight          ,           Total_Empty_Cone_Weight   ,     Total_Rewinding_Delivery_Weight   ,      Total_Tare_Weight            ,          Total_Gross_Weight      ,      Total_Rewinding_Amount   , ClothSales_OrderCode_forSelection )  " &
                                                        " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate,  '" & Trim(cbo_EntryType.Text) & "' ,  " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(txt_Party_DcNo.Text) & "',  " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & " , " & Str(Val(vTotYrExSt)) & " , '" & Trim(txt_BillNo.Text) & "' ,  " & Str(Val(txt_BillAmt.Text)) & " , " & Str(Val(txt_Empty_Gunnies.Text)) & " ,  " & Str(Val(txt_Empty_Cones.Text)) & " , '" & Trim(cbo_Vechile.Text) & "', " & Str(Val(Trans_ID)) & " , " & Str(Val(txt_Freight.Text)) & "," & Val(lbl_UserName.Text) & ", " & Str(Val(cone_Type_id)) & " , " & Val(txt_Empty_Bag_Weight.Text) & " , " & Val(txt_Empty_Cone_Weight.Text) & " ,'" & Trim(txt_Waste_Tolerance_Caption.Text) & "'," & Val(txt_Waste_Tolerance.Text) & "  , " & Str(Val(vTot_Waste_Tole_Wgt)) & " , " & Str(Val(vTot_Empty_Bag_Wgt)) & " , " & Str(Val(vTot_Empty_Cone_Wgt)) & "  , " & Str(Val(vTot_Rewind_Del_Wgt)) & ", " & Str(Val(vTot_Tare_Wgt)) & "     , " & Str(Val(vTot_Gross_Wgt)) & " , " & Str(Val(vTot_Amount)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'  )"
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", " Rewinding_Receipt_head", " Rewinding_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", " Rewinding_Receipt_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", " Rewinding_Receipt_Details", " Rewinding_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Mill_IdNo,count_idno, Bags, Cones, Weight, Set_No , Excess_Short , Rewinding_Delivery_Code , Rewinding_Delivery_SlNo", "Sl_No", " Rewinding_Receipt_Code, For_OrderBy, Company_IdNo,  Rewinding_Receipt_No,  Rewinding_Receipt_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Rewinding_Receipt_Head set Rewinding_Receipt_Date = @RecDate, DeliveryTo_IdNo = " & Str(Val(Delv_ID)) & " , ReceivedFrom_IdNo = " & Str(Val(Rec_ID)) & " , Party_DcNo = '" & Trim(txt_Party_DcNo.Text) & "'   , Receipt_Type = '" & Trim(cbo_EntryType.Text) & "' ,  Total_Bags = " & Str(Val(vTotYrnBags)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & " , Total_Ex_St = " & Str(Val(vTotYrExSt)) & " , Bill_No = '" & Trim(txt_BillNo.Text) & "'  , Bill_Amount = " & Str(Val(txt_BillAmt.Text)) & "  ,  Freight = " & Str(Val(txt_Freight.Text)) & ",  Empty_Gunnies = " & Str(Val(txt_Empty_Gunnies.Text)) & " , Empty_Cones = " & Str(Val(txt_Empty_Cones.Text)) & " , Vechile_No = '" & Trim(cbo_Vechile.Text) & "' , Transport_IdNo = " & Str(Val(Trans_ID)) & ",user_idNo = " & Val(lbl_UserName.Text) & " , Cone_type_idno = " & Str(Val(cone_Type_id)) & ", Empty_Bag_Weight = " & Val(txt_Empty_Bag_Weight.Text) & " , Empty_Cone_Weight = " & Val(txt_Empty_Cone_Weight.Text) & "  ,  Waste_Tolerance_Caption  ='" & Trim(txt_Waste_Tolerance_Caption.Text) & "' ,Total_Tare_Weight = " & Str(Val(vTot_Tare_Wgt)) & " , Total_Gross_Weight = " & Str(Val(vTot_Gross_Wgt)) & ", Total_Rewinding_Amount =" & Str(Val(vTot_Amount)) & " , Waste_Tolerance_Percentage  =" & Val(txt_Waste_Tolerance.Text) & " , Total_Rewinding_Delivery_Weight =  " & Str(Val(vTot_Rewind_Del_Wgt)) & "  , Total_Waste_Tolerance_Weight  = " & Str(Val(vTot_Waste_Tole_Wgt)) & "  , Total_Empty_Bag_Weight =" & Str(Val(vTot_Empty_Bag_Wgt)) & " , Total_Empty_Cone_Weight =" & Str(Val(vTot_Empty_Cone_Wgt)) & " , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Rewinding_Delivery_Details set Delivery_Weight = a.Delivery_Weight - (b.Weight-b.Excess_Short + Waste_Tolerance_Weight ) from Rewinding_Delivery_Details a, Rewinding_Receipt_Details b Where b.Rewinding_Receipt_Code = '" & Trim(NewCode) & "' and a.Rewinding_Delivery_code = b.Rewinding_Delivery_code and a.Rewinding_Delivery_SlNo = b.Rewinding_Delivery_SlNo"
                cmd.ExecuteNonQuery()


            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", " Rewinding_Receipt_head", " Rewinding_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", " Rewinding_Receipt_Code, Company_IdNo, for_OrderBy", tr)

            EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)
            Partcls = "Rcpt : Rec.No. " & Trim(lbl_RecNo.Text)

            PBlNo = Trim(lbl_RecNo.Text)

            cmd.CommandText = "Delete from Rewinding_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_YarnDetails
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(Dgv_ColDetails.NET_WEIGHT).Value) <> 0 Then

                        Sno = Sno + 1

                        YCnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(Dgv_ColDetails.COUNT_NAME).Value, tr)

                        YMil_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(Dgv_ColDetails.MILL_NAME).Value, tr)


                        RwDcCd = ""
                        RwDcDetSlNo = 0
                        If Trim(UCase(cbo_EntryType.Text)) = "SELECTION" Or Trim(UCase(cbo_EntryType.Text)) = "ORDER" Then
                            RwDcCd = Trim(.Rows(i).Cells(Dgv_ColDetails.REWINDING_DELIVERY_CODE).Value)
                            RwDcDetSlNo = Val(.Rows(i).Cells(Dgv_ColDetails.REWINDING_DELIVERY_SLNO).Value)
                        End If

                        vLOT_ENT_REFCODE = ""
                        If Trim(.Rows(i).Cells(Dgv_ColDetails.LOT_NO).Value) <> "" Then
                            vLOT_ENT_REFCODE = Common_Procedures.YarnLotCodeSelection_To_LotEntryReferenceCode(con, .Rows(i).Cells(Dgv_ColDetails.LOT_NO).Value, tr)
                        End If

                        cmd.CommandText = "Insert into Rewinding_Receipt_Details(Rewinding_Receipt_Code         ,       Company_IdNo            ,    Rewinding_Receipt_No       ,                               for_OrderBy,                         Rewinding_Receipt_Date     ,   Sl_No           ,            Mill_IdNo      ,       count_idno      ,                                    Bags                       ,                                        Cones              ,                                        Weight                     ,                                    Set_No                      ,                                       Excess_Short                   , Rewinding_Delivery_Code   ,           Rewinding_Delivery_SlNo     ,                                    Gross_Weight                    ,                   Tare_Weight                                    ,                   LotCode_forSelection                        ,        Lot_Entry_ReferenceCode  ,                         Waste_Tolerance_Weight                           ,                      Empty_Bag_Weight                            ,                               Empty_Cone_Weight                ,                          Rewinding_Delivery_Weight                               ,                        Rewinding_Rate                      ,                              Rewinding_Amount                  )  " &
                                                                        " Values ('" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate          , " & Str(Val(Sno)) & ",  " & Str(Val(YMil_ID)) & ", " & Str(Val(YCnt_ID)) & ", " & Str(Val(.Rows(i).Cells(Dgv_ColDetails.BAGS).Value)) & ", " & Str(Val(.Rows(i).Cells(Dgv_ColDetails.CONES).Value)) & ", " & Str(Val(.Rows(i).Cells(Dgv_ColDetails.NET_WEIGHT).Value)) & " ,  '" & Trim(.Rows(i).Cells(Dgv_ColDetails.SET_NO).Value) & "',  " & Str(Val(.Rows(i).Cells(Dgv_ColDetails.EXCESS_SHORT).Value)) & " ,     '" & Trim(RwDcCd) & "'  ,       " & Str(Val(RwDcDetSlNo)) & "   , " & Str(Val(.Rows(i).Cells(Dgv_ColDetails.GROSS_WEIGHT).Value)) & ", " & Str(Val(.Rows(i).Cells(Dgv_ColDetails.TARE_WEIGHT).Value)) & ",  '" & Trim(.Rows(i).Cells(Dgv_ColDetails.LOT_NO).Value) & "', '" & Trim(vLOT_ENT_REFCODE) & "' ," & Str(Val(.Rows(i).Cells(Dgv_ColDetails.WASTE_TOLERANCE).Value)) & "   ," & Str(Val(.Rows(i).Cells(Dgv_ColDetails.BAG_GRAMS).Value)) & "," & Str(Val(.Rows(i).Cells(Dgv_ColDetails.CONE_WEIGHT).Value)) & " , " & Str(Val(.Rows(i).Cells(Dgv_ColDetails.REWINDING_DELIVERY_WEIGHT).Value)) & " , " & Str(Val(.Rows(i).Cells(Dgv_ColDetails.RATE).Value)) & " ," & Str(Val(.Rows(i).Cells(Dgv_ColDetails.AMOUNT).Value)) & "  )"
                        cmd.ExecuteNonQuery()

                        If Trim(RwDcCd) <> "" And Val(RwDcDetSlNo) <> 0 Then
                            Nr = 0
                            cmd.CommandText = "Update Rewinding_Delivery_Details set Delivery_Weight = Delivery_Weight + (" & Str(Val(.Rows(i).Cells(Dgv_ColDetails.NET_WEIGHT).Value) - Val(.Rows(i).Cells(Dgv_ColDetails.EXCESS_SHORT).Value)) + Val(.Rows(i).Cells(Dgv_ColDetails.WASTE_TOLERANCE).Value) & ") Where Rewinding_Delivery_code = '" & Trim(RwDcCd) & "' and Rewinding_Delivery_SlNo = " & Str(Val(RwDcDetSlNo)) & " and Ledger_IdNo = " & Str(Val(Rec_ID))
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Order and Party Details")
                            End If
                        End If


                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo,  Mill_IdNo, yarn_Type, Bags, Cones, Weight ,LotCode_forSelection, Lot_Entry_ReferenceCode , ClothSales_OrderCode_forSelection ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ",  " & Str(Val(YMil_ID)) & ",'R/W' , " & Str(Val(.Rows(i).Cells(Dgv_ColDetails.BAGS).Value)) & ", " & Str(Val(.Rows(i).Cells(Dgv_ColDetails.CONES).Value)) & ", " & Str(Val(.Rows(i).Cells(Dgv_ColDetails.NET_WEIGHT).Value)) & ", '" & Str(Val(.Rows(i).Cells(Dgv_ColDetails.LOT_NO).Value)) & "' , '" & Trim(vLOT_ENT_REFCODE) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "')"
                        cmd.ExecuteNonQuery()


                    End If

                Next


                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", " Rewinding_Receipt_Details", " Rewinding_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Mill_IdNo,count_idno, Bags, Cones, Weight, Set_No , Excess_Short , Rewinding_Delivery_Code , Rewinding_Delivery_SlNo", "Sl_No", " Rewinding_Receipt_Code, For_OrderBy, Company_IdNo,  Rewinding_Receipt_No,  Rewinding_Receipt_Date, Ledger_Idno", tr)

            End With


            If Val(txt_Empty_Cones.Text) <> 0 Or Val(txt_Empty_Gunnies.Text) <> 0 Or Val(vTotYrnBags) <> 0 Or Val(vTotYrnCones) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Bags, Empty_Cones, Yarn_Bags, Yarn_Cones ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",@RecDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(txt_Empty_Cones.Text)) & ", " & Str(Val(txt_Empty_Gunnies.Text)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ")"
                cmd.ExecuteNonQuery()
            End If

            Dim vVou_LedIdNos As String = "", vVou_Amts As String = "", vVou_ErrMsg As String = ""

            vVou_LedIdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "Rwn.YRcpt", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RecNo.Text), Convert.ToDateTime(msk_date.Text), Partcls, vVou_LedIdNos, vVou_Amts, vVou_ErrMsg, tr) = False Then
                Throw New ApplicationException(vVou_ErrMsg)
            End If


            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RecNo.Text)
                End If
            Else
                move_record(lbl_RecNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub cbo_DelvTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DelvTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1)  or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_DelvTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelvTo, cbo_RecFrom, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1)  or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DelvTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DelvTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DelvTo, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1)  or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DelvTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Sizing_Creation
            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DelvTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_RecFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RecFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Rec_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecFrom.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecFrom, cbo_EntryType, cbo_DelvTo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Rec_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RecFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecFrom, cbo_DelvTo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 And (Trim(UCase(cbo_EntryType.Text)) = "SELECTION" Or Trim(UCase(cbo_EntryType.Text)) = "ORDER") Then
            If MessageBox.Show("Do you want to select from Delivery:", "FOR R/W DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                cbo_DelvTo.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Rec_Ledgerr_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RecFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEndEdit
        TotalYarnTaken_Calculation()
    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEnter

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_YarnDetails
            If Val(.CurrentRow.Cells(Dgv_ColDetails.SLNO).Value) = 0 Then
                .CurrentRow.Cells(Dgv_ColDetails.SLNO).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = Dgv_ColDetails.COUNT_NAME Then

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



            If e.ColumnIndex = Dgv_ColDetails.MILL_NAME Then

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


            If e.ColumnIndex = Dgv_ColDetails.LOT_NO Then

                If cbo_Grid_Yarn_LotNo.Visible = False Or Val(cbo_Grid_Yarn_LotNo.Tag) <> e.RowIndex Then

                    cbo_Grid_Yarn_LotNo.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select LotCode_forSelection from Yarn_Lot_Head " &
                                                      "where Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_YarnDetails.CurrentRow.Cells(Dgv_ColDetails.COUNT_NAME).Value & "') " &
                                                      " and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_YarnDetails.CurrentRow.Cells(Dgv_ColDetails.MILL_NAME).Value & "') order by LotCode_forSelection", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)

                    cbo_Grid_Yarn_LotNo.DataSource = Dt2
                    cbo_Grid_Yarn_LotNo.DisplayMember = "LotCode_forSelection"

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
            If .CurrentCell.ColumnIndex = Dgv_ColDetails.CONES Or .CurrentCell.ColumnIndex = Dgv_ColDetails.EXCESS_SHORT Or .CurrentCell.ColumnIndex = Dgv_ColDetails.CONE_WEIGHT Or .CurrentCell.ColumnIndex = Dgv_ColDetails.BAG_GRAMS Or .CurrentCell.ColumnIndex = Dgv_ColDetails.WASTE_TOLERANCE _
                Or .CurrentCell.ColumnIndex = Dgv_ColDetails.TARE_WEIGHT Or .CurrentCell.ColumnIndex = Dgv_ColDetails.NET_WEIGHT Or .CurrentCell.ColumnIndex = Dgv_ColDetails.GROSS_WEIGHT Then
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
                If .CurrentCell.ColumnIndex = Dgv_ColDetails.BAGS Or .CurrentCell.ColumnIndex = Dgv_ColDetails.CONES Or .CurrentCell.ColumnIndex = Dgv_ColDetails.GROSS_WEIGHT Or .CurrentCell.ColumnIndex = Dgv_ColDetails.NET_WEIGHT Or .CurrentCell.ColumnIndex = Dgv_ColDetails.EXCESS_SHORT Or .CurrentCell.ColumnIndex = Dgv_ColDetails.BAG_GRAMS _
                     Or .CurrentCell.ColumnIndex = Dgv_ColDetails.CONE_WEIGHT Or .CurrentCell.ColumnIndex = Dgv_ColDetails.TARE_WEIGHT Or .CurrentCell.ColumnIndex = Dgv_ColDetails.WASTE_TOLERANCE Or .CurrentCell.ColumnIndex = Dgv_ColDetails.RATE Or .CurrentCell.ColumnIndex = Dgv_ColDetails.EXCESS_SHORT Then

                    Net_Weight_Calculation(e.RowIndex)

                    If Val(dgv_YarnDetails.Rows(e.RowIndex).Cells(Dgv_ColDetails.NET_WEIGHT).Value) <> 0 And Val(dgv_YarnDetails.Rows(e.RowIndex).Cells(Dgv_ColDetails.RATE).Value) <> 0 Then
                        dgv_YarnDetails.Rows(e.RowIndex).Cells(Dgv_ColDetails.AMOUNT).Value = Format(Val(.Rows(e.RowIndex).Cells(Dgv_ColDetails.NET_WEIGHT).Value) * Val(.Rows(e.RowIndex).Cells(Dgv_ColDetails.RATE).Value), "##########0.00")
                    Else
                        dgv_YarnDetails.Rows(e.RowIndex).Cells(Dgv_ColDetails.AMOUNT).Value = ""
                    End If

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
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    txt_Party_DcNo.Focus()
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 0 Then
                    .CurrentCell.Selected = False
                    txt_Party_DcNo.Focus()
                    'SendKeys.Send("{RIGHT}")
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(Dgv_ColDetails.COUNT_NAME).Value) = "" Then

                    txt_BillNo.Focus()

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
                    .Rows(i).Cells(Dgv_ColDetails.SLNO).Value = i + 1
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
            .Rows(n - 1).Cells(Dgv_ColDetails.SLNO).Value = Val(n)

        End With
    End Sub
    'Private Sub get_MillCount_Details()
    '    Dim q As Single = 0
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt As New DataTable
    '    Dim Cn_bag As Single
    '    Dim Wgt_Bag As Single
    '    Dim Wgt_Cn As Single
    '    Dim CntID As Integer
    '    Dim MilID As Integer

    '    CntID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value)
    '    MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(2).Value)

    '    Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

    '    If CntID <> 0 And MilID <> 0 Then

    '        Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
    '        Da.Fill(Dt)
    '        With dgv_YarnDetails

    '            If Dt.Rows.Count > 0 Then
    '                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
    '                    Wgt_Bag = Dt.Rows(0).Item("Weight_Bag").ToString
    '                    Wgt_Cn = Dt.Rows(0).Item("Weight_Cone").ToString
    '                    Cn_bag = Dt.Rows(0).Item("Cones_Bag").ToString
    '                End If
    '            End If

    '            Dt.Clear()
    '            Dt.Dispose()
    '            Da.Dispose()

    '            If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
    '                If .CurrentCell.ColumnIndex = 4 Then
    '                    If Val(Cn_bag) <> 0 Then
    '                        .Rows(.CurrentRow.Index).Cells(5).Value = .Rows(.CurrentRow.Index).Cells(4).Value * Val(Cn_bag)
    '                    End If

    '                    If Val(Wgt_Bag) <> 0 Then
    '                        .Rows(.CurrentRow.Index).Cells(6).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(4).Value) * Val(Wgt_Bag), "#########0.000")
    '                    End If

    '                End If

    '                If .CurrentCell.ColumnIndex = 5 Then
    '                    If Val(Wgt_Cn) <> 0 Then
    '                        .Rows(.CurrentRow.Index).Cells(6).Value = Format(.Rows(.CurrentRow.Index).Cells(5).Value * Val(Wgt_Cn), "##########0.000")
    '                    End If

    '                End If

    '            End If

    '        End With

    '    End If

    'End Sub

    Private Sub TotalYarnTaken_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single, TotExSt As Single, Tot_Emp_Bag_Wgt = 0F, Tot_Emp_Cone_Wgt = 0F, Tot_Tole_Wgt = 0F, Tot_Rewind_Del_Wgt = 0F, Tot_Gross_Wgt = 0F, Tot_Tare_Wgt = 0F, Tot_Amount = 0F

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        TotExSt = 0
        Tot_Emp_Bag_Wgt = 0
        Tot_Emp_Cone_Wgt = 0
        Tot_Tole_Wgt = 0
        Tot_Rewind_Del_Wgt = 0
        Tot_Tare_Wgt = 0
        Tot_Gross_Wgt = 0

        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(Dgv_ColDetails.SLNO).Value = Sno
                If Val(.Rows(i).Cells(Dgv_ColDetails.NET_WEIGHT).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(Dgv_ColDetails.BAGS).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(Dgv_ColDetails.CONES).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(Dgv_ColDetails.NET_WEIGHT).Value)
                    Tot_Gross_Wgt = Tot_Gross_Wgt + Val(.Rows(i).Cells(Dgv_ColDetails.GROSS_WEIGHT).Value)
                    Tot_Tare_Wgt = Tot_Tare_Wgt + Val(.Rows(i).Cells(Dgv_ColDetails.TARE_WEIGHT).Value)
                    TotExSt = TotExSt + Val(.Rows(i).Cells(Dgv_ColDetails.EXCESS_SHORT).Value)

                    Tot_Emp_Bag_Wgt = Tot_Emp_Bag_Wgt + Val(.Rows(i).Cells(Dgv_ColDetails.BAG_GRAMS).Value)
                    Tot_Emp_Cone_Wgt = Tot_Emp_Cone_Wgt + Val(.Rows(i).Cells(Dgv_ColDetails.CONE_WEIGHT).Value)
                    Tot_Tole_Wgt = Tot_Tole_Wgt + Val(.Rows(i).Cells(Dgv_ColDetails.WASTE_TOLERANCE).Value)
                    Tot_Rewind_Del_Wgt = Tot_Rewind_Del_Wgt + Val(.Rows(i).Cells(Dgv_ColDetails.REWINDING_DELIVERY_WEIGHT).Value)

                    Tot_Amount = Tot_Amount + Val(.Rows(i).Cells(Dgv_ColDetails.AMOUNT).Value)
                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(Dgv_ColDetails.BAGS).Value = Val(TotBags)
            .Rows(0).Cells(Dgv_ColDetails.CONES).Value = Val(TotCones)
            .Rows(0).Cells(Dgv_ColDetails.NET_WEIGHT).Value = Format(Val(TotWeight), "########0.000")
            .Rows(0).Cells(Dgv_ColDetails.GROSS_WEIGHT).Value = Format(Val(Tot_Gross_Wgt), "########0.000")
            .Rows(0).Cells(Dgv_ColDetails.TARE_WEIGHT).Value = Format(Val(Tot_Tare_Wgt), "########0.000")
            .Rows(0).Cells(Dgv_ColDetails.EXCESS_SHORT).Value = Format(Val(TotExSt), "########0.000")

            .Rows(0).Cells(Dgv_ColDetails.BAG_GRAMS).Value = Format(Val(Tot_Emp_Bag_Wgt), "########0.000")
            .Rows(0).Cells(Dgv_ColDetails.CONE_WEIGHT).Value = Format(Val(Tot_Emp_Cone_Wgt), "########0.000")
            .Rows(0).Cells(Dgv_ColDetails.WASTE_TOLERANCE).Value = Format(Val(Tot_Tole_Wgt), "########0.000")
            .Rows(0).Cells(Dgv_ColDetails.REWINDING_DELIVERY_WEIGHT).Value = Format(Val(Tot_Rewind_Del_Wgt), "########0.000")
            .Rows(0).Cells(Dgv_ColDetails.AMOUNT).Value = Format(Val(Tot_Amount), "########0.00")

        End With

    End Sub

    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, cbo_Grid_CountName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

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
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = Dgv_ColDetails.MILL_NAME Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, cbo_Grid_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        With dgv_YarnDetails
            With dgv_YarnDetails
                If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    If .CurrentCell.RowIndex = 0 Then

                        If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                            cbo_ClothSales_OrderCode_forSelection.Focus()
                        ElseIf txt_Waste_Tolerance.Visible And txt_Waste_Tolerance.Enabled Then
                            txt_Waste_Tolerance.Focus()
                        ElseIf txt_Empty_Cone_Weight.Enabled And txt_Empty_Cone_Weight.Visible = True Then
                            txt_Empty_Cone_Weight.Focus()
                        Else
                            txt_Party_DcNo.Focus()
                        End If

                    Else

                        .Focus()
                        If dgv_YarnDetails.Columns(Dgv_ColDetails.LOT_NO).Visible = True Then
                            .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(Dgv_ColDetails.LOT_NO)
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(Dgv_ColDetails.EXCESS_SHORT)
                        End If

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
                    txt_BillNo.Focus()
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
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = Dgv_ColDetails.COUNT_NAME Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_YarnDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = Dgv_ColDetails.BAGS Or .CurrentCell.ColumnIndex = Dgv_ColDetails.CONES Or .CurrentCell.ColumnIndex = Dgv_ColDetails.NET_WEIGHT Or .CurrentCell.ColumnIndex = Dgv_ColDetails.RATE Then

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
        Dim n As Integer, i As Integer
        Dim Delv_IDNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""
        Dim EdsCnt_IdNo As Integer, Mil_IdNo As Integer

        Try

            Condt = ""
            Delv_IDNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0
            EdsCnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Rewinding_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Rewinding_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Rewinding_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Delv_IDNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Trim(cbo_Filter_MillName.Text) <> "" Then
                Mil_IdNo = Common_Procedures.Mill_NameToIdNo(con, cbo_Filter_MillName.Text)
            End If

            If Val(Delv_IDNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.ReceivedFrom_IdNo = " & Str(Val(Delv_IDNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Rewinding_Receipt_Code IN (select z1.Rewinding_Receipt_Code from Rewinding_Receipt_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Rewinding_Receipt_Code IN (select z2.Rewinding_Receipt_Code from Rewinding_Receipt_Details z2 where z2.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ")"
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Rewinding_Receipt_Head a INNER JOIN Ledger_Head b on a.ReceivedFrom_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Rewinding_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Rewinding_Receipt_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Rewinding_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Rewinding_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
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
        Dim Ps As Printing.PaperSize

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Rewinding_Receipt_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Rewinding_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Rewinding_Receipt_Code = '" & Trim(NewCode) & "'", con)
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

        If Trim(Common_Procedures.settings.CustomerCode) = "1464" Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    Ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = Ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = Ps
                    PrintDocument1.DefaultPageSettings.Landscape = True
                    Exit For
                End If
            Next
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
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, e.Transport_Name  from Rewinding_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_IdNo = c.Ledger_IdNo  Left Outer JOIN Transport_Head e ON a.Transport_IdNo = e.Transport_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Rewinding_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Rewinding_Receipt_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Rewinding_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        If Trim(Common_Procedures.settings.CustomerCode) = "1464" Then
            Printing_Format2_1464(e)
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

        NoofItems_PerPage = 6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(45) : ClAr(2) = 270 : ClAr(3) = 120 : ClAr(4) = 100 : ClAr(5) = 110
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

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
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Rewinding_Receipt_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Rewinding_Receipt_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "REWINDING RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Rewinding_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT ", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CONE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
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
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

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
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

        CurY = CurY + 10
        If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Empty Cones ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString), LMargin + W2 + 30, CurY, 0, 0, pFont)
        End If


        If Val(prn_HdDt.Rows(0).Item("Empty_Gunnies").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Empty Gunnies  ", LMargin + C1 + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Empty_Gunnies").ToString), LMargin + W1 + C1 + 30, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Vehicle No  ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + W2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

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



    Private Sub txt_BillAmt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BillAmt.KeyDown

        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub txt_BillAmt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BillAmt.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If

    End Sub

    Private Sub txt_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BillNo.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 38) Then
            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(Dgv_ColDetails.COUNT_NAME)

            Else
                txt_Party_DcNo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EntryType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EntryType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EntryType, msk_date, cbo_RecFrom, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EntryType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntryType, cbo_RecFrom, "", "", "", "")
    End Sub

    Private Sub cbo_TransportName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Transportname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, txt_Empty_Cones, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

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

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Yarn_Receipt_Head", "Vechile_No", "", "")

    End Sub
    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, txt_Freight, cbo_Cone_Type, "Weaver_Yarn_Receipt_Head", "Vechile_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vechile, cbo_Cone_Type, "Weaver_Yarn_Receipt_Head", "Vechile_No", "", "", False)
    End Sub

    Private Sub txt_Empty_Cones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Empty_Cones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Empty_Gunnies_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Empty_Gunnies.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If cbo_Vechile.Enabled And cbo_Vechile.Visible = True Then
                cbo_Vechile.Focus()
            ElseIf dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(Dgv_ColDetails.COUNT_NAME)

            Else
                txt_BillNo.Focus()

            End If

            cbo_Vechile.Focus()
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If cbo_Vechile.Enabled And cbo_Vechile.Visible = True Then
                cbo_Vechile.Focus()
            Else
                If dgv_YarnDetails.Rows.Count > 0 Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(Dgv_ColDetails.COUNT_NAME)

                Else
                    txt_BillNo.Focus()

                End If
            End If
        End If
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bag As Single = 0
        Dim Ent_Cones As Single = 0
        Dim Ent_wgt As Single = 0
        Dim Ent_Exc As Single = 0
        Dim Ent_Rate As Single = 0
        Dim Ent_Amt As Single = 0

        If Trim(cbo_EntryType.Text) <> "SELECTION" Then
            MessageBox.Show("Invalid Entry Type", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EntryType.Enabled And cbo_EntryType.Visible Then cbo_EntryType.Focus()
            Exit Sub
        End If

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecFrom.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_RecFrom.Enabled And cbo_RecFrom.Visible Then cbo_RecFrom.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Count_Name, d.Ledger_Name as Delvname, e.Ledger_Name as Transportname,  g.Mill_name , h.Bags as Ent_Bags , h.Cones as Ent_Cones , h.Weight As Ent_Wgt , h.Excess_Short As Ent_Exs_Sht , h.Gross_Weight as Ent_Grss_Wgt,h.Empty_Bag_Weight as Ent_Empty_Bag_Wgt ,h.Empty_Cone_Weight as Ent_Empty_Cone_Wgt,h.Rewinding_Rate as Ent_RAte ,h.Rewinding_Amount as Ent_Amount from Rewinding_Delivery_Head a INNER JOIN Rewinding_Delivery_details b ON a.Rewinding_Delivery_Code = b.Rewinding_Delivery_Code INNER JOIN Count_Head c ON b.Count_IdNo = c.Count_IdNo INNER JOIN Mill_Head g ON b.Mill_IdNo = g.Mill_IdNo LEFT OUTER JOIN Ledger_Head d ON a.ReceivedFrom_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Rewinding_Receipt_Details h ON h.Rewinding_Receipt_Code = '" & Trim(NewCode) & "' and b.Rewinding_Delivery_Code = h.Rewinding_Delivery_Code and b.Rewinding_Delivery_SlNo = h.Rewinding_Delivery_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.DeliveryTo_IdNo = " & Str(Val(LedIdNo)) & " and ((b.Weight - b.Delivery_Weight ) > 0 or h.Weight > 0 ) order by a.Rewinding_Delivery_Date, a.for_orderby, a.Rewinding_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()
                    Ent_Bag = 0
                    Ent_Cones = 0
                    Ent_wgt = 0
                    Ent_Exc = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Bags").ToString) = False And Val(Dt1.Rows(i).Item("Ent_Bags").ToString) <> 0 Then
                        Ent_Bag = Val(Dt1.Rows(i).Item("Ent_Bags").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Cones").ToString) = False And Val(Dt1.Rows(i).Item("Ent_Cones").ToString) <> 0 Then
                        Ent_Cones = Val(Dt1.Rows(i).Item("Ent_Cones").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Wgt").ToString) = False And Val(Dt1.Rows(i).Item("Ent_Wgt").ToString) <> 0 Then
                        Ent_wgt = Val(Dt1.Rows(i).Item("Ent_Wgt").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Exs_Sht").ToString) = False And Val(Dt1.Rows(i).Item("Ent_Exs_Sht").ToString) <> 0 Then
                        Ent_Exc = Val(Dt1.Rows(i).Item("Ent_Exs_Sht").ToString)
                    End If

                    SNo = SNo + 1

                    If SNo = 115 Then
                        Debug.Print(SNo)
                    End If


                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Rewinding_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Rewinding_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Mill_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString) - Val(Dt1.Rows(i).Item("Delivery_Weight").ToString) + (Val(Ent_wgt) - Val(Ent_Exc)), "#########0.00")


                    If Ent_wgt > 0 Then
                        .Rows(n).Cells(9).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(9).Value = ""

                    End If

                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Delvname").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Party_DcNo").ToString
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Vechile_No").ToString
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Transportname").ToString
                    .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Freight").ToString
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Rewinding_Delivery_Code").ToString
                    .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Rewinding_Delivery_SlNo").ToString

                    .Rows(n).Cells(17).Value = Ent_Bag
                    .Rows(n).Cells(18).Value = Ent_Cones
                    .Rows(n).Cells(19).Value = Ent_wgt
                    .Rows(n).Cells(20).Value = Ent_Exc



                    .Rows(n).Cells(21).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                    .Rows(n).Cells(22).Value = Format(Val(Dt1.Rows(i).Item("Ent_Grss_Wgt").ToString), "#########0.000")
                    .Rows(n).Cells(23).Value = Format(Val(Dt1.Rows(i).Item("Ent_Empty_Bag_Wgt").ToString), "#########0.000")
                    .Rows(n).Cells(24).Value = Format(Val(Dt1.Rows(i).Item("Ent_Empty_Cone_Wgt").ToString), "#########0.000")

                    .Rows(n).Cells(25).Value = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
                    .Rows(n).Cells(26).Value = Format(Val(Dt1.Rows(i).Item("Ent_Amount").ToString), "#########0.00")


                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        Pnl_Back.Enabled = False
        Pnl_Back.Visible = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(9).Value = (Val(.Rows(RwIndx).Cells(9).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(9).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(9).Value = ""

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

        Pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        Pnl_Back.Visible = True

        dgv_YarnDetails.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(9).Value) = 1 Then


                cbo_DelvTo.Text = dgv_Selection.Rows(i).Cells(10).Value
                cbo_Vechile.Text = dgv_Selection.Rows(i).Cells(12).Value
                txt_Party_DcNo.Text = dgv_Selection.Rows(i).Cells(11).Value
                cbo_TransportName.Text = dgv_Selection.Rows(i).Cells(13).Value

                If txt_Freight.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(14).Value) <> "" Then
                        txt_Freight.Text = dgv_Selection.Rows(i).Cells(14).Value
                    End If
                End If

                n = dgv_YarnDetails.Rows.Add()
                sno = sno + 1
                dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.SLNO).Value = Val(sno)
                dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.COUNT_NAME).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.MILL_NAME).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.SET_NO).Value = dgv_Selection.Rows(i).Cells(5).Value

                If Val(dgv_Selection.Rows(i).Cells(17).Value) <> 0 Then
                    dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.BAGS).Value = dgv_Selection.Rows(i).Cells(17).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(18).Value) <> 0 Then
                    dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.CONES).Value = dgv_Selection.Rows(i).Cells(18).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(19).Value) <> 0 Then
                    dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.NET_WEIGHT).Value = dgv_Selection.Rows(i).Cells(19).Value
                Else
                    dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.NET_WEIGHT).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(20).Value) <> 0 Then
                    dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.EXCESS_SHORT).Value = dgv_Selection.Rows(i).Cells(20).Value
                End If

                dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.REWINDING_DELIVERY_CODE).Value = dgv_Selection.Rows(i).Cells(15).Value
                dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.REWINDING_DELIVERY_SLNO).Value = dgv_Selection.Rows(i).Cells(16).Value

                dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.REWINDING_DELIVERY_WEIGHT).Value = dgv_Selection.Rows(i).Cells(21).Value

                If Val(dgv_Selection.Rows(i).Cells(22).Value) <> 0 Then
                    dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.GROSS_WEIGHT).Value = dgv_Selection.Rows(i).Cells(22).Value
                Else
                    dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.GROSS_WEIGHT).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If

                dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.BAG_GRAMS).Value = dgv_Selection.Rows(i).Cells(23).Value
                    dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.CONE_WEIGHT).Value = dgv_Selection.Rows(i).Cells(24).Value

                    dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.RATE).Value = dgv_Selection.Rows(i).Cells(25).Value
                    dgv_YarnDetails.Rows(n).Cells(Dgv_ColDetails.AMOUNT).Value = dgv_Selection.Rows(i).Cells(26).Value


                End If

                Net_Weight_Calculation(i)

        Next

        '  TotalYarnTaken_Calculation()

        Pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        Pnl_Back.Visible = True
        If txt_Empty_Gunnies.Enabled And txt_Empty_Gunnies.Visible Then txt_Empty_Gunnies.Focus()

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_YarnDetails_KeyUp(sender, e)
        End If
    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
        End If
    End Sub



    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Date.Text = Date.Today
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

    Private Sub cbo_RecFrom_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_RecFrom.SelectedIndexChanged

    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub


    Private Sub cbo_coneType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cone_Type.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cone_Type, cbo_Vechile, txt_Empty_Bag_Weight, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")

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

            ElseIf txt_Waste_Tolerance.Visible And txt_Waste_Tolerance.Enabled Then
                txt_Waste_Tolerance.Focus()
            ElseIf dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(Dgv_ColDetails.COUNT_NAME)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Empty_Cone_Weight_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Empty_Cone_Weight.KeyDown
        If (e.KeyValue = 40) Then
            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()

            ElseIf txt_Waste_Tolerance.Visible And txt_Waste_Tolerance.Enabled Then
                txt_Waste_Tolerance.Focus()
            ElseIf dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(Dgv_ColDetails.COUNT_NAME)

            Else
                btn_save.Focus()

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Lot_Head", "LotCode_forSelection", "Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_YarnDetails.CurrentRow.Cells(Dgv_ColDetails.COUNT_NAME).Value & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_YarnDetails.CurrentRow.Cells(Dgv_ColDetails.MILL_NAME).Value & "')", "(Lot_No = '')")
    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_LotNo.TextChanged

        Try
            If cbo_Grid_Yarn_LotNo.Visible Then

                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

                With dgv_YarnDetails
                    If Val(cbo_Grid_Yarn_LotNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = Dgv_ColDetails.LOT_NO Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(Dgv_ColDetails.LOT_NO).Value = Trim(cbo_Grid_Yarn_LotNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception

            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Yarn_LotNo, Nothing, Nothing, "Yarn_Lot_Head", "LotCode_forSelection", "Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_YarnDetails.CurrentRow.Cells(Dgv_ColDetails.COUNT_NAME).Value & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_YarnDetails.CurrentRow.Cells(Dgv_ColDetails.MILL_NAME).Value & "')", "(Lot_No = '')")


        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_Yarn_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Yarn_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                'If .CurrentCell.ColumnIndex < .ColumnCount - 1 Then
                '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                'Else
                If .CurrentRow.Index < .RowCount - 1 Then
                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(Dgv_ColDetails.COUNT_NAME)
                End If
                ' End If
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Yarn_LotNo, Nothing, "Yarn_Lot_Head", "LotCode_forSelection", "Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_YarnDetails.CurrentRow.Cells(Dgv_ColDetails.COUNT_NAME).Value & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_YarnDetails.CurrentRow.Cells(Dgv_ColDetails.MILL_NAME).Value & "')", "(Lot_No = '')")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Focus()
                'If .CurrentCell.ColumnIndex < .ColumnCount - 1 Then
                '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                'Else
                If .CurrentRow.Index < .RowCount - 1 Then
                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(Dgv_ColDetails.COUNT_NAME)
                End If
                'End If

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
    Private Sub cbo_EntryType_TextChanged(sender As Object, e As EventArgs) Handles cbo_EntryType.TextChanged

    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_YarnDetails
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub txt_Waste_Tolerance_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Waste_Tolerance.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                If dgv_YarnDetails.Rows.Count > 0 Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(Dgv_ColDetails.COUNT_NAME)

                Else
                    btn_save.Focus()

                End If
            End If



        End If
    End Sub

    Private Sub txt_Waste_Tolerance_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Waste_Tolerance.KeyDown
        If (e.KeyValue = 40) Then

            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                If dgv_YarnDetails.Rows.Count > 0 Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(Dgv_ColDetails.COUNT_NAME)

                Else
                    btn_save.Focus()

                End If
            End If
        End If


        If e.KeyValue = 38 Then
            txt_Empty_Cone_Weight.Focus()
        End If
    End Sub

    Private Sub txt_Waste_Tolerance_TextChanged(sender As Object, e As EventArgs) Handles txt_Waste_Tolerance.TextChanged

        For i = 0 To dgv_YarnDetails.RowCount - 1
            Net_Weight_Calculation(i)
        Next i
    End Sub

    Private Sub Net_Weight_Calculation(Optional CurRow As Integer = Nothing, Optional CurCol As Integer = Nothing)
        On Error Resume Next

        Dim vTareWgt = ""
        Dim vNetWgt = ""
        Dim vExc_Shrt_Wgt = ""

        vTareWgt = 0 : vNetWgt = 0 : vExc_Shrt_Wgt = 0

        With dgv_YarnDetails

            If .Rows(CurRow).Cells(Dgv_ColDetails.COUNT_NAME).Value <> "" Then

                ' --- TARE WEIGHT --- ' 

                If dgv_YarnDetails.Columns(Dgv_ColDetails.BAG_GRAMS).Visible And dgv_YarnDetails.Columns(Dgv_ColDetails.CONE_WEIGHT).Visible Then

                    vTareWgt = (Val(.Rows(CurRow).Cells(Dgv_ColDetails.BAG_GRAMS).Value) * Val(.Rows(CurRow).Cells(Dgv_ColDetails.BAGS).Value)) + (Val(.Rows(CurRow).Cells(Dgv_ColDetails.CONE_WEIGHT).Value) * Val(.Rows(CurRow).Cells(Dgv_ColDetails.CONES).Value))

                    dgv_YarnDetails.Rows(CurRow).Cells(Dgv_ColDetails.TARE_WEIGHT).Value = Format(Val(vTareWgt), "##########0.000")

                End If

                ' --- WASTE TOLERANCE WEIGHT --- '

                If Val(txt_Waste_Tolerance.Text) <> 0 And dgv_YarnDetails.Columns(Dgv_ColDetails.WASTE_TOLERANCE).Visible Then
                    dgv_YarnDetails.Rows(CurRow).Cells(Dgv_ColDetails.WASTE_TOLERANCE).Value = Format(Val(Val(dgv_YarnDetails.Rows(CurRow).Cells(Dgv_ColDetails.REWINDING_DELIVERY_WEIGHT).Value) * Val(txt_Waste_Tolerance.Text) / 100), "##########0.000")
                Else
                    dgv_YarnDetails.Rows(CurRow).Cells(Dgv_ColDetails.WASTE_TOLERANCE).Value = 0
                    End If


                ' --- NET WEIGHT --- '

                If Val(.Rows(CurRow).Cells(Dgv_ColDetails.GROSS_WEIGHT).Value) <> 0 Then
                    vNetWgt = Val(.Rows(CurRow).Cells(Dgv_ColDetails.GROSS_WEIGHT).Value) - Val(.Rows(CurRow).Cells(Dgv_ColDetails.TARE_WEIGHT).Value) '+ Val(dgv_YarnDetails.Rows(CurRow).Cells(Dgv_ColDetails.EXCESS_SHORT).Value)
                    dgv_YarnDetails.Rows(CurRow).Cells(Dgv_ColDetails.NET_WEIGHT).Value = Format(Val(vNetWgt), "##########0.000")

                End If



                ' --- EXCESS SHORT WEIGHT --- '

                'EXSHRT = NETWGT + TOLERNCE - DELVERYWGT
                If Common_Procedures.settings.CustomerCode = "1464" Then ' -- mof autocalcu

                        If Val(vNetWgt) <> 0 Then
                            vExc_Shrt_Wgt = (Val(vNetWgt) + Val(dgv_YarnDetails.Rows(CurRow).Cells(Dgv_ColDetails.WASTE_TOLERANCE).Value)) - Val(dgv_YarnDetails.Rows(CurRow).Cells(Dgv_ColDetails.REWINDING_DELIVERY_WEIGHT).Value)
                        End If

                        dgv_YarnDetails.Rows(CurRow).Cells(Dgv_ColDetails.EXCESS_SHORT).Value = Format(Val(vExc_Shrt_Wgt), "########0.000") ' Format(Val(vNetWgt) - Val(dgv_YarnDetails.Rows(CurRow).Cells(Dgv_ColDetails.REWINDING_DELIVERY_WEIGHT).Value), "########0.000")

                        ' TotalYarnTaken_Calculation()

                    End If
                End If
        End With

    End Sub
    Private Sub Printing_Format2_1464(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim CntItmNm1 As String, CntItmNm2 As String

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

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

        '    If PpSzSTS = False Then
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                e.PageSettings.PaperSize = ps
        '                Exit For
        '            End If
        '        Next
        '    End If

        'End If


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.Landscape = True
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 30
            .Top = 60 '30
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

        NoofItems_PerPage = 16 '5 '6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(30) : ClAr(2) = 230 : ClAr(3) = 130 : ClAr(4) = 60 : ClAr(5) = 60 : ClAr(6) = 60 : ClAr(7) = 70 : ClAr(8) = 70 : ClAr(9) = 70 : ClAr(10) = 70 : ClAr(11) = 70 : ClAr(12) = 70
        ClAr(13) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12))

        TxtHgt = 18 '18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_1464_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format2_1464_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If


                        CntItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString)
                        CntItmNm2 = ""

                        If Len(CntItmNm1) > 10 Then
                            For I = 10 To 1 Step -1
                                If Mid$(Trim(CntItmNm1), I, 1) = " " Or Mid$(Trim(CntItmNm1), I, 1) = "," Or Mid$(Trim(CntItmNm1), I, 1) = "." Or Mid$(Trim(CntItmNm1), I, 1) = "-" Or Mid$(Trim(CntItmNm1), I, 1) = "/" Or Mid$(Trim(CntItmNm1), I, 1) = "_" Or Mid$(Trim(CntItmNm1), I, 1) = "(" Or Mid$(Trim(CntItmNm1), I, 1) = ")" Or Mid$(Trim(CntItmNm1), I, 1) = "\" Or Mid$(Trim(CntItmNm1), I, 1) = "[" Or Mid$(Trim(CntItmNm1), I, 1) = "]" Or Mid$(Trim(CntItmNm1), I, 1) = "{" Or Mid$(Trim(CntItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 10
                            CntItmNm2 = Microsoft.VisualBasic.Right(Trim(CntItmNm1), Len(CntItmNm1) - I)
                            CntItmNm1 = Microsoft.VisualBasic.Left(Trim(CntItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(CntItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Set_No").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Empty_Bag_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Empty_Cone_Weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Gross_Weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Tare_Weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Waste_Tolerance_Weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Excess_Short").ToString), "#######0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        '                      Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Empty_Bag_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)

                        '                      Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        '                      Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Empty_Cone_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)

                        '                      Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Gross_Weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                        '                      Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Tare_Weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
                        '                      Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "#######0.000"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Or Trim(CntItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(CntItmNm2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format2_1464_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_1464_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Rewinding_Receipt_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Rewinding_Receipt_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "REWINDING RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
        W1 = e.Graphics.MeasureString("DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        Dim L1 = 0F

        L1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10)



        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rewinding_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)


        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + L1, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + L1 + 40, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Rewinding_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + L1 + 50, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Bill_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Bill No", LMargin + C1 + 10, CurY + 5, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 20, CurY + 5, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + C1 + W1 + 30, CurY + 5, 0, 0, pFont)
        End If

        If Val(prn_HdDt.Rows(0).Item("Bill_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Bill Amount : " & Val(prn_HdDt.Rows(0).Item("Bill_Amount").ToString), LMargin + L1, CurY + 5, 0, 0, pFont)

        End If


        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Rewinding_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        If Val(prn_HdDt.Rows(0).Item("Waste_Tolerance_Percentage").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Waste Tolerance  :   " & Val(prn_HdDt.Rows(0).Item("Waste_Tolerance_Percentage").ToString) & " %", LMargin + C1 + 10, CurY + 5, 0, 0, pFont)

        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT ", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BAG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "KG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 15, 2, ClAr(6), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "CONE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "CONE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "KG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + 15, 2, ClAr(8), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "GROSS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + 15, 2, ClAr(9), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TARE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + 15, 2, ClAr(10), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "NET", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY + 15, 2, ClAr(11), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "WASTE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOLER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY + 15, 2, ClAr(12), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "EXCESS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SHORT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY + 15, 2, ClAr(13), pFont)


        CurY = CurY + TxtHgt + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format2_1464_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
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
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Empty_Bag_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Empty_cONE_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Gross_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Tare_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Waste_Tolerance_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Ex_St").ToString), "#######0.000"), PageWidth - 10, CurY, 1, 0, pFont)

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
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "Received From  ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, prn_HdDt.Rows(0).Item("ReceivedFrom_IdNo").ToString), LMargin + W2 + 30, CurY, 0, 0, pFont)

        If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Empty_Gunnies").ToString) <> 0 Then
            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Empty Cones ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString), LMargin + W2 + 30, CurY, 0, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(0).Item("Empty_Gunnies").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Empty Gunnies  ", LMargin + C1 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Empty_Gunnies").ToString), LMargin + W1 + C1 + 30, CurY, 0, 0, pFont)
            End If
        End If

        If Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString) <> "" Then
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No  ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

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

    Private Sub cbo_ClothSales_OrderCode_forSelection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.SelectedIndexChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If Asc(e.KeyChar) = 13 Then

            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(Dgv_ColDetails.COUNT_NAME)

            Else
                btn_save.Focus()

            End If

        End If


    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If (e.KeyCode = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(Dgv_ColDetails.COUNT_NAME)

            Else
                btn_save.Focus()

            End If
        End If

        If (e.KeyCode = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If txt_Waste_Tolerance.Visible = True Then
                txt_Waste_Tolerance.Focus()
            Else
                txt_Empty_Cone_Weight.Focus()
            End If

        End If


    End Sub

End Class