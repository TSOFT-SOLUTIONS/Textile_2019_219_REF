Imports System.IO
Public Class Sizing_Empty_BeamBagCone_Delivery_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "EBDLV-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private prn_DetDt As New DataTable
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private prn_HdDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_Status As Integer
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private prn_DetIndx As Integer
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_DetSNo As Integer

    Private pth As String
    Private pth2 As String
    Private PrnTxt As String = ""
    Private a() As String
    Private fs As FileStream
    Private r As StreamReader
    Private w As StreamWriter

    Private Hz1 As Integer, Hz2 As Integer, Vz1 As Integer, Vz2 As Integer
    Private Corn1 As Integer, Corn2 As Integer, Corn3 As Integer, Corn4 As Integer
    Private LfCon As Integer, RgtCon As Integer
    Private LnCnt As Integer = 0

    ' PRAKASH    SIZING 
    Private Prnt_HalfSheet_STS As Boolean = False
    Private vPrnt_2Copy_In_SinglePage As Integer = 0
    ' PRAKASH    SIZING 

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub


    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        lbl_dcno.Text = ""
        lbl_dcno.ForeColor = Color.Black
        dtp_date.Text = ""
        cbo_partyname.Text = ""
        cbo_beamwidth.Text = ""
        txt_BookNo.Text = ""
        cbo_vehicleno.Text = ""
        txt_emptycones.Text = ""
        txt_remarks.Text = ""
        txt_emptybags.Text = ""
        cbo_bagType.Text = ""
        cbo_coneType.Text = ""
        cbo_Vendor.Text = ""
        dtp_Time.Text = ""
        cbo_Delivered.Text = ""
        txt_SetNo.Text = ""
        dgv_Details.Rows.Clear()

        Grid_Cell_DeSelect()
        cbo_beamwidth.Visible = False
        cbo_beamwidth.Tag = -1

        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False

        cbo_Vendor.Visible = False
        cbo_Vendor.Tag = -1
        dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
        dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dtp_date.Enabled = True
        dtp_date.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        cbo_partyname.Enabled = True
        cbo_partyname.BackColor = Color.White

        cbo_vehicleno.Enabled = True
        cbo_vehicleno.BackColor = Color.White
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            'Me.ActiveControl.BackColor = Color.FromArgb(200, 150, 200)
            Me.ActiveControl.BackColor = Color.Lime  ' Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_beamwidth.Name Then
            cbo_beamwidth.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Vendor.Name Then
            cbo_Vendor.Visible = False
        End If


        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub
    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        'dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        If Val(no) = 0 Then Exit Sub
        Dim LockSTS As Boolean = False
        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Sizing_Empty_BeamBagCone_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_dcno.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString
                cbo_partyname.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                txt_BookNo.Text = dt1.Rows(0).Item("Book_No").ToString
                txt_emptybags.Text = dt1.Rows(0).Item("Empty_Bags").ToString
                txt_emptycones.Text = dt1.Rows(0).Item("Empty_Cones").ToString
                cbo_vehicleno.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                dtp_Time.Text = (dt1.Rows(0).Item("Entry_Time_Text").ToString)
                cbo_Delivered.Text = dt1.Rows(0).Item("Delivered_To").ToString
                cbo_Transport.Text = Common_Procedures.Transport_IdNoToName(con, dt1.Rows(0).Item("Transport_Idno").ToString)
                cbo_Location.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("Location_Idno").ToString)
                cbo_bagType.Text = Common_Procedures.Bag_Type_IdNoToName(con, dt1.Rows(0).Item("Bag_Type_Idno").ToString)
                cbo_coneType.Text = Common_Procedures.Conetype_IdNoToName(con, dt1.Rows(0).Item("Cone_Type_Idno").ToString)
                txt_SetNo.Text = dt1.Rows(0).Item("Set_No").ToString


                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

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
                If IsDBNull(dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Beam_Width_Name from Sizing_Empty_BeamBagCone_Delivery_Details a LEFT OUTER JOIN Beam_Width_Head b ON a.Beam_Width_IdNo = b.Beam_Width_IdNo   Where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        Sno = Sno + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_Details.Rows(n).Cells(1).Value = Val(dt2.Rows(i).Item("Empty_Beam").ToString)
                        dgv_Details.Rows(n).Cells(2).Value = Common_Procedures.Vendor_IdNoToName(con, dt2.Rows(i).Item("Vendor_Idno").ToString)
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Beam_Width_Name").ToString

                    Next i

                End If
                dt2.Clear()

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Empty_beam").ToString)
                    '.Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Consumption").ToString), "########0.000")
                End With
            End If


            da2 = New SqlClient.SqlDataAdapter("Select a.Empty_Beam from Empty_Beam_Selection_Processing_Details a where  a.Reference_Code<>'" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_Code='" & Trim(Pk_Condition) & Trim(NewCode) & "' AND a.Empty_Beam < 0", con)
            dt2 = New DataTable
            da2.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                LockSTS = True
            End If

            dt2.Clear()

            dt1.Dispose()
            da1.Dispose()
            If LockSTS = True Then

                dtp_date.Enabled = False
                dtp_date.BackColor = Color.LightGray

                cbo_Transport.Enabled = False
                cbo_Transport.BackColor = Color.LightGray

                cbo_partyname.Enabled = False
                cbo_partyname.BackColor = Color.LightGray

                cbo_vehicleno.Enabled = False
                cbo_vehicleno.BackColor = Color.LightGray




            End If
            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_partyname.Visible And cbo_partyname.Enabled Then cbo_partyname.Focus()

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_partyname.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_partyname.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_beamwidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_beamwidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Vendor.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Vendor.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_bagType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BAGTYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_bagType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_coneType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CONETYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_coneType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Empty_BeamBagCone_Delivery_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim Dt9 As New DataTable
        Dim dt10 As New DataTable

        Me.Text = ""

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        Da.Fill(Dt1)
        cbo_partyname.DataSource = Dt1
        cbo_partyname.DisplayMember = "Ledger_DisplayName"

        Da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from beam_Width_head order by Beam_Width_Name", con)
        Da.Fill(Dt2)
        cbo_beamwidth.DataSource = Dt2
        cbo_beamwidth.DisplayMember = "Beam_Width_Name"

        'Da = New SqlClient.SqlDataAdapter("select distinct(vehicle_No) from Sizing_Empty_BeamBagCone_Delivery_Head order by Vehicle_No", con)
        'Da.Fill(dt3)
        'cbo_vehicleno.DataSource = dt3
        'cbo_vehicleno.DisplayMember = "Vehicle_No"

        'Da = New SqlClient.SqlDataAdapter("select distinct(Delivered_To) from Sizing_Empty_BeamBagCone_Delivery_Head order by Delivered_To", con)
        'Da.Fill(dt4)
        'cbo_Delivered.DataSource = dt4
        'cbo_Delivered.DisplayMember = "Delivered_To"

        Da = New SqlClient.SqlDataAdapter("select Bag_Type_Name from Bag_Type_Head order by Bag_Type_Name", con)
        Da.Fill(Dt9)
        cbo_bagType.DataSource = Dt9
        cbo_bagType.DisplayMember = "Bag_Type_Name"

        Da = New SqlClient.SqlDataAdapter("select Cone_Type_Name from Cone_Type_Head order by Cone_Type_Name", con)
        Da.Fill(dt10)
        cbo_coneType.DataSource = dt10
        cbo_coneType.DisplayMember = "Cone_Type_Name"

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            lbl_SetNo_Caption.Visible = True
            txt_SetNo.Visible = True
        Else
            lbl_SetNo_Caption.Visible = False
            txt_SetNo.Visible = False
        End If

        Pnl_DosPrint.Visible = False
        Pnl_DosPrint.BringToFront()
        Pnl_DosPrint.Left = (Me.Width - Pnl_DosPrint.Width) \ 2
        Pnl_DosPrint.Top = (Me.Height - Pnl_DosPrint.Height) \ 2

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        pnl_Print.Visible = False
        pnl_Print.BringToFront()
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2

        btn_UserModification.Visible = False
        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            btn_UserModification.Visible = True
            chk_Printed.Enabled = True
        End If

        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_partyname.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BookNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_beamwidth.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptycones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptybags.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vehicleno.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_bagType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_coneType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vendor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Delivered.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_FilterFrom_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterTo_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Location.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Location.LostFocus, AddressOf ControlLostFocus


        AddHandler btn_filtershow.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_closefilter.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SetNo.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_partyname.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BookNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_beamwidth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptycones.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptybags.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vehicleno.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vendor.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Delivered.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_FilterFrom_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FilterTo_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_bagType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_coneType.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_filtershow.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_closefilter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SetNo.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BookNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_emptycones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_emptybags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_remarks.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SetNo.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BookNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_emptycones.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_emptybags.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_remarks.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SetNo.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then
            dtp_Time.Visible = True
        End If

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
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

    Private Sub Empty_BeamBagCone_Delivery_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim UID As Single = 0
        Dim vUsrNm As String = "", vAcPwd As String = "", vUnAcPwd As String = ""
        Dim vOrdByNo As String = ""
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '----- KALAIMAGAL TEXTILES (AVINASHI)
            Common_Procedures.Password_Input = ""
            Dim g As New Admin_Password
            g.ShowDialog()

            UID = 1
            Common_Procedures.get_Admin_Name_PassWord_From_DB(vUsrNm, vAcPwd, vUnAcPwd)

            vAcPwd = Common_Procedures.Decrypt(Trim(vAcPwd), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))
            vUnAcPwd = Common_Procedures.Decrypt(Trim(vUnAcPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))

            If Trim(Common_Procedures.Password_Input) <> Trim(vAcPwd) And Trim(Common_Procedures.Password_Input) <> Trim(vUnAcPwd) Then
                MessageBox.Show("Invalid Admin Password", "ADMIN PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_EMPTY_BEAM_DELIVERY, New_Entry, Me, con, "Sizing_Empty_BeamBagCone_Delivery_Head", "Empty_BeamBagCone_Delivery_Code", NewCode, "Empty_BeamBagCone_Delivery_Date", "(Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub



        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from Sizing_Empty_BeamBagCone_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            If IsDBNull(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                    MessageBox.Show("Already Gate Pass Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

        End If
        Dt1.Clear()
        Dt1.Dispose()
        Da.Dispose()


        da2 = New SqlClient.SqlDataAdapter("Select a.Empty_Beam from Empty_Beam_Selection_Processing_Details a where  a.Reference_Code<>'" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_Code='" & Trim(Pk_Condition) & Trim(NewCode) & "' AND a.Empty_Beam < 0", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            MessageBox.Show("Already Receipt Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        tr = con.BeginTransaction
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_dcno.Text)

        Try



            cmd.Connection = con
            cmd.Transaction = tr


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Sizing_Empty_BeamBagCone_Delivery_Head", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Empty_BeamBagCone_Delivery_Code, Company_IdNo, for_OrderBy", tr)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Sizing_Empty_BeamBagCone_Delivery_Details", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Empty_Beam,  Vendor_idNo, Beam_Width_IdNo  ", "Sl_No", "Empty_BeamBagCone_Delivery_Code, For_OrderBy, Company_IdNo, Empty_BeamBagCone_Receipt_No, Empty_BeamBagCone_Receipt_Date, Ledger_Idno", tr)

            cmd.CommandText = "Delete from Empty_Beam_Selection_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_WasteMaterial_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sizing_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
            dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
            pnl_filter.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_filter.Rows.Clear()

            da.Dispose()

        End If

        pnl_filter.Visible = True
        pnl_filter.Enabled = True
        pnl_filter.BringToFront()
        pnl_back.Enabled = False
        If dtp_FilterFrom_date.Enabled And dtp_FilterFrom_date.Visible Then dtp_FilterFrom_date.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_EMPTY_BEAM_DELIVERY, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Dc.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Empty_BeamBagCone_Delivery_No from Sizing_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Dc.No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_dcno.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try

            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Delivery_No from Sizing_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Empty_BeamBagCone_Delivery_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Delivery_No from Sizing_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Empty_BeamBagCone_Delivery_No desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_dcno.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Delivery_No from Sizing_Empty_BeamBagCone_Delivery_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Empty_BeamBagCone_Delivery_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_dcno.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Delivery_No from Sizing_Empty_BeamBagCone_Delivery_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Empty_BeamBagCone_Delivery_No desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

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

            lbl_dcno.Text = Common_Procedures.get_MaxCode(con, "Sizing_Empty_BeamBagCone_Delivery_Head", "Empty_BeamBagCone_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)


            dtp_Time.Text = Format(Now, "hh:mm tt").ToString

            lbl_dcno.ForeColor = Color.Red

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
        da.Dispose()


    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Dc.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Empty_BeamBagCone_Delivery_No from Sizing_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Dc.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim nr As Long = 0
        Dim led_id As Integer = 0
        Dim bw_id As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotetybm As Single
        Dim Sno As Integer = 0
        Dim Bg_Id As Integer
        Dim Con_Id As Integer
        Dim Trans_Id As Integer = 0
        Dim Dt As New DataTable
        Dim WstBg_Id As Integer
        Dim WstCn_Id As Integer
        Dim Vndr_Id As Integer = 0
        Dim UserIdNo As Integer = 0
        Dim Loc_id As Integer = 0
        Dim vOrdByNo As String = ""
        Dim vDELVLED_COMPIDNO As Integer = 0
        Dim vCOMP_LEDIDNO As Integer = 0
        Dim vSELC_RCVDIDNO As Integer = 0

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        UserIdNo = Common_Procedures.User.IdNo

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_EMPTY_BEAM_DELIVERY, New_Entry, Me, con, "Sizing_Empty_BeamBagCone_Delivery_Head", "Empty_BeamBagCone_Delivery_Code", NewCode, "Empty_BeamBagCone_Delivery_Date", "(Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Empty_BeamBagCone_Delivery_No desc", dtp_date.Value.Date) = False Then Exit Sub



        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text, tr)
        Loc_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Location.Text, tr)
        Bg_Id = Common_Procedures.BagType_NameToIdNo(con, cbo_bagType.Text, tr)
        Con_Id = Common_Procedures.ConeType_NameToIdNo(con, cbo_coneType.Text, tr)
        Trans_Id = Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_partyname.Enabled Then cbo_partyname.Focus()
            Exit Sub
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
            If Trim(cbo_vehicleno.Text) = "" Then
                MessageBox.Show("Invalid Vehicle No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_vehicleno.Enabled And cbo_vehicleno.Visible Then cbo_vehicleno.Focus()
                Exit Sub
            End If
        End If

        If Trim(cbo_vehicleno.Text) <> "" Then
            cbo_vehicleno.Text = Common_Procedures.Vehicle_Number_Remove_Unwanted_Spaces(Trim(cbo_vehicleno.Text))
        End If

        If dtp_Time.Visible Then
            If New_Entry = True Then
                If Trim(dtp_Time.Text) = "" Or IsDate(dtp_Time.Text) = False Then
                    dtp_Time.Text = Format(Now, "Short Time").ToString
                End If
            End If
            If Trim(dtp_Time.Text) = "" Then
                MessageBox.Show("Invalid Time", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
                Exit Sub
            End If

            If IsDate(dtp_Time.Text) = False Then
                MessageBox.Show("Invalid Time", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
                Exit Sub
            End If
        Else
            If New_Entry = True Or Trim(dtp_Time.Text) = "" Or IsDate(dtp_Time.Text) = False Then
                dtp_Time.Value = Now
            End If
        End If

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(1).Value) <> 0 Then

                    If Val(.Rows(i).Cells(1).Value) = 0 Then
                        MessageBox.Show("Invalid Empty Beam", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If


                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
                        If Trim(.Rows(i).Cells(3).Value) = "" Then
                            MessageBox.Show("Invalid Beam Width", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(3)
                            End If
                            Exit Sub
                        End If
                    End If

                End If

            Next
        End With

        Total_Calculation()

        vTotetybm = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotetybm = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
            ' vTotconMtrs = Val(dgv_BobinDetails_Total.Rows(0).Cells(4).Value())
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        da = New SqlClient.SqlDataAdapter("select * from Sizing_Empty_BeamBagCone_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            If IsDBNull(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                    MessageBox.Show("Already Gate Pass Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

        End If
        Dt1.Clear()
        Dt1.Dispose()
        da.Dispose()

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Sizing_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
                da.SelectCommand.Transaction = tr
                da.Fill(dt4)


                NewNo = 0
                If dt4.Rows.Count > 0 Then
                    If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                        NewNo = Int(Val(dt4.Rows(0)(0).ToString))
                        NewNo = Val(NewNo) + 1
                    End If
                End If
                dt4.Clear()
                If Trim(NewNo) = "" Then NewNo = Trim(lbl_dcno.Text)

                lbl_dcno.Text = Trim(NewNo)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DeliveryDate", dtp_date.Value.Date)

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_dcno.Text)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Sizing_Empty_BeamBagCone_Delivery_Head(User_IdNo , Empty_BeamBagCone_Delivery_Code, Company_IdNo, Empty_BeamBagCone_Delivery_No, for_OrderBy,Empty_BeamBagCone_Delivery_Date, Ledger_IdNo, Book_No, Empty_Beam,Empty_Bags,Empty_Cones,Vehicle_No,Remarks , Bag_Type_Idno , Cone_Type_Idno ,Entry_Time_Text,Delivered_To ,Transport_Idno, set_no,Location_IdNo) Values (" & Str(UserIdNo) & ",'" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "', " & Str(Val(vOrdByNo)) & ", @DeliveryDate," & Val(led_id) & ", '" & Trim(txt_BookNo.Text) & "', " & Val(vTotetybm) & ",  " & Val(txt_emptybags.Text) & ", " & Val(txt_emptycones.Text) & ",'" & Trim(cbo_vehicleno.Text) & "','" & Trim(txt_remarks.Text) & "' , " & Str(Val(Bg_Id)) & "  ," & Str(Val(Con_Id)) & ",'" & Trim(dtp_Time.Text) & "','" & Trim(cbo_Delivered.Text) & "'," & Val(Trans_Id) & ", '" & Trim(txt_SetNo.Text) & "'," & Loc_id & ")"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Sizing_Empty_BeamBagCone_Delivery_Head", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Empty_BeamBagCone_Delivery_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Sizing_Empty_BeamBagCone_Delivery_Details", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Empty_Beam,  Vendor_idNo, Beam_Width_IdNo  ", "Sl_No", "Empty_BeamBagCone_Delivery_Code, For_OrderBy, Company_IdNo, Empty_BeamBagCone_Receipt_No, Empty_BeamBagCone_Receipt_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Sizing_Empty_BeamBagCone_Delivery_Head set User_IdNo=" & Str(UserIdNo) & ", Empty_BeamBagCone_Delivery_Date = @DeliveryDate, Ledger_IdNo = " & Val(led_id) & ", Bag_Type_Idno = " & Str(Val(Bg_Id)) & "  , Cone_Type_Idno = " & Str(Val(Con_Id)) & " , Book_No = '" & Trim(txt_BookNo.Text) & "', Empty_Beam = " & Val(vTotetybm) & ", Entry_Time_Text = '" & Trim(dtp_Time.Text) & "' ,  Empty_Bags = " & Val(txt_emptybags.Text) & ",Empty_Cones=" & Val(txt_emptycones.Text) & ",Vehicle_No='" & Trim(cbo_vehicleno.Text) & "',Remarks='" & Trim(txt_remarks.Text) & "', Delivered_To ='" & Trim(cbo_Delivered.Text) & "',Transport_Idno = " & Val(Trans_Id) & ", set_no = '" & Trim(txt_SetNo.Text) & "',Location_IdNo=" & Loc_id & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Stock_WasteMaterial_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Sizing_Empty_BeamBagCone_Delivery_Head", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Empty_BeamBagCone_Delivery_Code, Company_IdNo, for_OrderBy", tr)



            If Val(Common_Procedures.settings.StatementPrint_BookNo_IN_Stock_Particulars_Status) = 1 Then
                Partcls = "Return : Dc.No. " & Trim(lbl_dcno.Text)
                If Trim(txt_BookNo.Text) <> "" Then PBlNo = Trim(txt_BookNo.Text) Else PBlNo = Trim(lbl_dcno.Text)
                'PBlNo = Trim(txt_BookNo.Text)
            Else
                Partcls = "Return : Dc.No. " & Trim(lbl_dcno.Text)
                PBlNo = Trim(lbl_dcno.Text)
            End If

            If Val(Bg_Id) <> 0 Then

                da = New SqlClient.SqlDataAdapter("select a.* from Waste_Head a Where a.Bag_Type_Idno = " & Str(Val(Bg_Id)), con)
                Dt = New DataTable
                da.SelectCommand.Transaction = tr
                da.Fill(Dt)

                WstBg_Id = 0
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        WstBg_Id = Val(Dt.Rows(0).Item("Packing_Idno").ToString)
                    End If
                End If

                Dt.Dispose()
                da.Dispose()

                cmd.CommandText = "Insert into Stock_WasteMaterial_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No        ,         For_OrderBy      , Reference_Date,        Ledger_IdNo      ,      Party_Bill_No   ,           Sl_No      ,          Waste_IdNo  ,      Quantity     ,     Rate ,  Amount ) " &
                                             "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "', " & Str(Val(vOrdByNo)) & ",    @DeliveryDate   , " & Str(Val(led_id)) & ", '" & Trim(PBlNo) & "', 1, " & Str(Val(WstBg_Id)) & ", " & Str(-1 * Val(txt_emptybags.Text)) & ",  0   , 0 )"
                cmd.ExecuteNonQuery()

            End If

            If Val(Con_Id) <> 0 Then

                da = New SqlClient.SqlDataAdapter("select a.* from Waste_Head a Where a.Cone_Type_Idno = " & Str(Val(Con_Id)), con)
                da.SelectCommand.Transaction = tr
                Dt = New DataTable
                da.Fill(Dt)

                WstCn_Id = 0
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        WstCn_Id = Val(Dt.Rows(0).Item("Packing_Idno").ToString)
                    End If
                End If

                Dt.Dispose()
                da.Dispose()

                cmd.CommandText = "Insert into Stock_WasteMaterial_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No        ,         For_OrderBy      ,   Reference_Date   ,        Ledger_IdNo      ,      Party_Bill_No   ,           Sl_No      ,          Waste_IdNo  ,      Quantity     ,     Rate ,  Amount ) " &
                                             "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "', " & Str(Val(vOrdByNo)) & ",    @DeliveryDate   , " & Str(Val(led_id)) & ", '" & Trim(PBlNo) & "', 2 , " & Str(Val(WstCn_Id)) & ", " & Str(-1 * Val(txt_emptycones.Text)) & ",      0   , 0       )"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Sizing_Empty_BeamBagCone_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Empty_Beam_Selection_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(1).Value) <> 0 Then

                        Sno = Sno + 1
                        Vndr_Id = Common_Procedures.Vendor_AlaisNameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        bw_id = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        cmd.CommandText = "Insert into Sizing_Empty_BeamBagCone_Delivery_Details ( Empty_BeamBagCone_Delivery_Code,           Company_IdNo           ,   Empty_BeamBagCone_Delivery_No ,  for_OrderBy          , Empty_BeamBagCone_Delivery_Date,          Sl_No        ,                   Empty_Beam         ,       Vendor_idNo    ,      Beam_Width_IdNo    ) " &
                                            "           Values                                   (      '" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "'   , " & Val(vOrdByNo) & " ,         @DeliveryDate          , " & Str(Val(Sno)) & " ,  " & Val(.Rows(i).Cells(1).Value) & ", " & Val(Vndr_Id) & " , " & Str(Val(bw_id)) & " ) "
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(SoftwareType_IdNo  ,Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Beam_Width_IdNo,Vendor_idno, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "', " & Str(Val(vOrdByNo)) & ", @DeliveryDate, " & Str(Val(led_id)) & ", 0, '" & Trim(PBlNo) & "',  " & Str(Val(Sno)) & " , " & Str(Val(bw_id)) & "," & Val(Vndr_Id) & ", " & Val(.Rows(i).Cells(1).Value) & ", 0,0, '" & Trim(Partcls) & "' )"
                        cmd.ExecuteNonQuery()


                    End If

                Next
            End With
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Sizing_Empty_BeamBagCone_Delivery_Details", "Empty_BeamBagCone_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_dcno.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Empty_Beam,  Vendor_idNo, Beam_Width_IdNo  ", "Sl_No", "Empty_BeamBagCone_Delivery_Code, For_OrderBy, Company_IdNo, Empty_BeamBagCone_Receipt_No, Empty_BeamBagCone_Receipt_Date, Ledger_Idno", tr)


            If Val(vTotetybm) <> 0 Or Val(txt_emptybags.Text) <> 0 Or Val(txt_emptycones.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(SoftwareType_IdNo  , Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars,Vendor_IdNo) Values (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "', " & Str(Val(vOrdByNo)) & ", @DeliveryDate, " & Str(Val(led_id)) & ", 0, '" & Trim(PBlNo) & "', 101 , 0 ,  0 , " & Str(Val(txt_emptybags.Text)) & ", " & Str(Val(txt_emptycones.Text)) & ", '" & Trim(Partcls) & "'," & Val(Vndr_Id) & " )"
                nr = cmd.ExecuteNonQuery()
            End If

            If Val(Common_Procedures.User.IdNo) = 1 Then
                If chk_Printed.Visible = True Then
                    If chk_Printed.Enabled = True Then
                        Update_PrintOut_Status(tr)
                    End If
                End If
            End If

            vDELVLED_COMPIDNO = 0
            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
                vDELVLED_COMPIDNO = Common_Procedures.Ledger_IdNoToCompanyIdNo(con, Str(Val(led_id)), tr)
            End If

            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And vDELVLED_COMPIDNO <> 0 Then

                vCOMP_LEDIDNO = Common_Procedures.Company_IdnoToSizingLedgerIdNo(con, Str(Val(lbl_Company.Tag)), tr)

                vSELC_RCVDIDNO = vCOMP_LEDIDNO
                'vREC_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Rec_id)) & ")", , tr)
                'If Trim(UCase(vREC_Ledtype)) = "GODOWN" Then
                '    vSELC_RCVDIDNO = Rec_id
                'Else
                '    vSELC_RCVDIDNO = vCOMP_LEDIDNO
                'End If

                cmd.CommandText = "Insert into Empty_Beam_Selection_Processing_Details (                Reference_Code              ,              Company_IdNo        ,            Reference_No      ,             for_OrderBy   , Reference_Date  ,            Delivery_Code                    ,     Delivery_No               ,      DeliveryTo_Idno    , ReceivedFrom_Idno, DeliveryAt_Idno ,         Party_Dc_No          , Beam_Width_IdNo    ,         Empty_Beam         ,                 Empty_Bags          ,                  Empty_Cones         ,    Selection_CompanyIdno           ,       Selection_Ledgeridno     ,      Selection_ReceivedFromIdNo  ) " &
                                    "           Values                                 ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "', " & Str(Val(vOrdByNo)) & ",   @DeliveryDate , '" & Trim(Pk_Condition) & Trim(NewCode) & "', '" & Trim(lbl_dcno.Text) & "' , " & Str(Val(led_id)) & ",        0         ,        0        , '" & Trim(lbl_dcno.Text) & "',  " & Val(bw_id) & ", " & Str(Val(vTotetybm)) & ", " & Str(Val(txt_emptybags.Text)) & ", " & Str(Val(txt_emptycones.Text)) & ", " & Str(Val(vDELVLED_COMPIDNO)) & ", " & Str(Val(vCOMP_LEDIDNO)) & ", " & Str(Val(vSELC_RCVDIDNO)) & " ) "
                cmd.ExecuteNonQuery()

            End If


            tr.Commit()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1163" Then '---- Guru Sizing (Somanur) Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_dcno.Text)
                End If
            Else
                move_record(lbl_dcno.Text)
            End If



        Catch ex As Exception
            tr.Rollback()
        MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

    End Sub

    Private Sub cbo_partyname_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_partyname.GotFocus
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status=0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status=0 )", "(Ledger_IdNo = 0)")
        End If


    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_partyname.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_partyname, dtp_date, cbo_Delivered, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status=0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_partyname, dtp_date, cbo_Delivered, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status=0 )", "(Ledger_IdNo = 0)")
        End If

        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, txt_RecNo, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub



    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_partyname.KeyPress

        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_partyname, cbo_Delivered, "Ledger_AlaisHead", "Ledger_DisplayName", " (AccountsGroup_IdNo = 10 and Close_Status=0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_partyname, cbo_Delivered, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status=0 )", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_partyname.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_partyname.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub



    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_beamwidth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_beamwidth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub





    Private Sub cbo_beamwidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_beamwidth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_beamwidth, Nothing, Nothing, "Beam_Width_head", "Beam_Width_name", "", "Beam_Width_name")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_beamwidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_beamwidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        ' msk_date.Focus()
                        dtp_date.Focus()
                    End If

                Else
                    .Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

                End If


            End If

        End With
    End Sub

    Private Sub cbo_beamwidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_beamwidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_beamwidth, Nothing, "Beam_Width_head", "Beam_Width_name", "", "Beam_Width_name")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        'msk_date.Focus()
                        dtp_date.Focus()
                    End If

                Else
                    .Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_beamwidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_beamwidth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_beamwidth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub



    Private Sub txt_remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_remarks.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True

        End If
    End Sub

    Private Sub txt_remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True

        End If
    End Sub

    Private Sub txt_emptybeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_emptybags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptybags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_emptycones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptycones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vehicleno.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vehicleno, cbo_bagType, "Sizing_Empty_BeamBagCone_Delivery_Head", "Vehicle_No", "", "(Empty_BeamBagCone_Delivery_No = 0)", False)
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vehicleno.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vehicleno, txt_emptybags, cbo_bagType, "Sizing_Empty_BeamBagCone_Delivery_Head", "Vehicle_No", "", "(Empty_BeamBagCone_Delivery_No = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_FilterTo_date, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, txt_RecNo, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub



    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_filtershow_Click(sender, e)
        End If
    End Sub

    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Itm_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Itm_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Empty_BeamBagCone_Delivery_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Empty_BeamBagCone_Delivery_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Empty_BeamBagCone_Delivery_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Sizing_Empty_BeamBagCone_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Empty_BeamBagCone_Delivery_No", con)
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Empty_BeamBagCone_Delivery_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Empty_beam").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Empty_Bags").ToString
                    dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Empty_Cones").ToString

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

    End Sub

    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub




    Private Sub btn_closefilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_closefilter.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub dgv_filter_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_filter.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String = ""

        Try
            movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_back.Enabled = True
                pnl_filter.Visible = False
            End If
        Catch ex As Exception
            '---
        End Try


    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_EMPTY_BEAM_DELIVERY, New_Entry) = False Then Exit Sub

        'pnl_Print.Visible = True
        'pnl_back.Enabled = False
        'If btn_Print_Invoice.Enabled And btn_Print_Invoice.Visible Then
        '    btn_Print_Invoice.Focus()
        'End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (Palladam)
            pnl_Print.Visible = True
            pnl_back.Enabled = False
            If btn_Print_Preprint.Enabled And btn_Print_Preprint.Visible Then
                btn_Print_Preprint.Focus()
            End If
        ElseIf Common_Procedures.settings.Dos_Printing = 1 Then
            Pnl_DosPrint.Visible = True
            pnl_back.Enabled = False
            If Btn_DosPrint.Enabled And Btn_DosPrint.Visible Then
                Btn_DosPrint.Focus()
            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Then '---- Prakash sizing

            Dim mymsgbox As New Tsoft_MessageBox("Select Paper Size to Print", "A4,HALF-SHEET,CANCEL", "FOR DELIVERY PRINTING...", "IF A4 is selected, 2 copies of dc will be printed in single A4 sheet," & Chr(13) & "If HALF-SHEET is selected 1 copy of dc will be printed in 8x6 paper size", MesssageBoxIcons.Questions, 2)
            mymsgbox.ShowDialog()

            If mymsgbox.MessageBoxResult = 1 Then
                vPrnt_2Copy_In_SinglePage = 1

            ElseIf mymsgbox.MessageBoxResult = 2 Then
                Prnt_HalfSheet_STS = True
                vPrnt_2Copy_In_SinglePage = 0

            Else

                Exit Sub

            End If

            'prn_TotCopies = Val(InputBox("Enter No.of Copies", "FOR DELIVERY PRINTING...", "2"))
            'If Val(prn_TotCopies) <= 0 Then
            '    Exit Sub
            'End If

            set_PaperSize_For_PrintDocument1()

            If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

                Try
                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                            set_PaperSize_For_PrintDocument1()



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

                    ppd.WindowState = FormWindowState.Maximized
                    ppd.StartPosition = FormStartPosition.CenterScreen
                    'ppd.ClientSize = New Size(600, 600)
                    ppd.PrintPreviewControl.AutoZoom = True
                    ppd.PrintPreviewControl.Zoom = 1.0

                    ppd.ShowDialog()

                Catch ex As Exception
                    MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

                End Try

            End If






        Else
            printing_invoice()

        End If

    End Sub


    Private Sub set_PaperSize_For_PrintDocument1()
        Dim I As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next


        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Or Prnt_HalfSheet_STS = True Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_A4_As_Default_PaperSize) = 1 Or Val(Common_Procedures.settings.Printing_For_FullSheet_Set_A4_As_Default_PaperSize) = 1 Or Val(vPrnt_2Copy_In_SinglePage) = 1 Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        Else

            PpSzSTS = False

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                If ps.Width = 800 And ps.Height = 600 Then
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then

                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                            PrintDocument1.DefaultPageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

            End If

        End If

    End Sub

    Private Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize

        prn_DetDt.Clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Sizing_Empty_BeamBagCone_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count >= 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.Empty_Beam AS BEAMS, b.Beam_Width_name from Sizing_Empty_BeamBagCone_Delivery_Details a LEFT OUTER JOIN Beam_Width_hEAD b ON a.Beam_Width_IdNo = b.Beam_Width_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try


        prn_TotCopies = 1
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (palladam)
            prn_TotCopies = 2
            If Val(prn_TotCopies) <= 0 Then
                Exit Sub
            End If
        End If

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            Debug.Print(ps.PaperName)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If Common_Procedures.settings.CustomerCode = "1282" Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

        Else

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
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
                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        'Debug.Print(ps.PaperName)
                        If ps.Width = 800 And ps.Height = 600 Then
                            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                            PrintDocument1.DefaultPageSettings.PaperSize = ps
                            PpSzSTS = True
                            Exit For
                        End If
                    Next

                    If PpSzSTS = False Then
                        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                PrintDocument1.DefaultPageSettings.PaperSize = ps
                                PpSzSTS = True
                                Exit For
                            End If
                        Next

                        If PpSzSTS = False Then
                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    Exit For
                                End If
                            Next
                        End If
                        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                PrintDocument1.DefaultPageSettings.PaperSize = ps
                                Exit For
                            End If
                        Next
                    End If
                Else
                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                            PrintDocument1.DefaultPageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                    PrintDocument1.Print()

                End If


            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            'Try

            Dim ppd As New PrintPreviewDialog

            ppd.Document = PrintDocument1

            ppd.WindowState = FormWindowState.Maximized
            ppd.StartPosition = FormStartPosition.CenterScreen
            'ppd.ClientSize = New Size(600, 600)

            AddHandler ppd.Shown, AddressOf PrintPreview_Shown
            ppd.ShowDialog()


            'Catch ex As Exception
            '    MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            'End Try

        End If
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_PageNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Beam_Width_Name, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code  from Sizing_Empty_BeamBagCone_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo <> 0 and a.Company_IdNo = b.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = csh.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON c.ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_IdNo = d.Beam_Width_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                da2 = New SqlClient.SqlDataAdapter("select EBMD.*,VH.* FROM Sizing_Empty_BeamBagCone_Delivery_Details EBMD INNER JOIN Vendor_Head VH ON EBMD.Vendor_IdNo = VH.Vendor_IdNo WHERE EBMD.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "' ORDER BY EBMD.Beam_Width_IdNo ", con)
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

    Private Sub PrintDocument1_EndPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.EndPrint
        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            chk_Printed.Checked = True
            Update_PrintOut_Status()
        End If
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (Palladam)
            If prn_Status = 1 Then
                'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
                Printing_Format1(e)
            Else
                Printing_Format2(e)
            End If 'End 
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            'Printing_Format1(e)
            Printing_Format4(e)
        ElseIf Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage = 1 Then
            Printing_Format5(e)
        Else
            Printing_Format1(e)
        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        'Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PS As Printing.PaperSize
        Dim CurX As Single = 0, strWidth As Single = 0
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        Dim Ledname1 As String, Vndr_Nm1 As String, Vndr_Nm2 As String
        Dim Ledname2 As String
        Dim i As Integer

        Dim PCnt As Integer = 0, PrntCnt As Integer = 0


        If Common_Procedures.settings.CustomerCode = "1038" Then
            PrntCnt = 1
            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                If PrntCnt2ndPageSTS = False Then
                    PrntCnt = 2
                End If
            End If

            set_PaperSize_For_PrintDocument1()
        Else
            For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                PS = PrintDocument1.PrinterSettings.PaperSizes(i)
                Debug.Print(PS.PaperName)
                If PS.Width = 800 And PS.Height = 600 Then
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(i).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(i)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument1.PrinterSettings.PaperSizes(i).Kind = Printing.PaperKind.A4 Then
                            PS = PrintDocument1.PrinterSettings.PaperSizes(i)
                            PrintDocument1.DefaultPageSettings.PaperSize = PS
                            e.PageSettings.PaperSize = PS
                            Exit For
                        End If
                    Next
                End If

            End If
            For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(i).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(i)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    Exit For
                End If
            Next
        End If



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20  ' 50 
            .Right = 55
            .Top = 35   '30
            .Bottom = 35 ' 30
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

        TxtHgt = 19.75 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        Else

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
                Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            End If

        End If

        'Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        'Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        'Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
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
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                Led_GstNo = "GSTIN" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Led_TinNo = " TIN NO  " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
            End If
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 13  ' 10

        Else

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
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 1

        End If
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM/BAG DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        W1 = e.Graphics.MeasureString("BOOK NO : ", pFont).Width

        Ledname1 = Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)
        '  End If

        Ledname2 = ""

        If Len(Ledname1) > 40 Then
            For i = 40 To 1 Step -1
                If Mid$(Trim(Ledname1), i, 1) = " " Or Mid$(Trim(Ledname1), i, 1) = "," Or Mid$(Trim(Ledname1), i, 1) = "." Or Mid$(Trim(Ledname1), i, 1) = "-" Or Mid$(Trim(Ledname1), i, 1) = "/" Or Mid$(Trim(Ledname1), i, 1) = "_" Or Mid$(Trim(Ledname1), i, 1) = "(" Or Mid$(Trim(Ledname1), i, 1) = ")" Or Mid$(Trim(Ledname1), i, 1) = "\" Or Mid$(Trim(Ledname1), i, 1) = "[" Or Mid$(Trim(Ledname1), i, 1) = "]" Or Mid$(Trim(Ledname1), i, 1) = "{" Or Mid$(Trim(Ledname1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 40
            Ledname2 = Microsoft.VisualBasic.Right(Trim(Ledname1), Len(Ledname1) - i)
            Ledname1 = Microsoft.VisualBasic.Left(Trim(Ledname1), i - 1)
        End If


        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & Ledname1, LMargin + 10, CurY, 0, 0, p1Font)


        If Trim(Ledname2) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & Trim(Ledname2), LMargin + 10, CurY, 0, 0, p1Font)
            'NoofDets = NoofDets + 1
        End If

        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add1, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)

        Vndr_Nm1 = ""
        Vndr_Nm2 = ""
        If prn_DetDt.Rows.Count > 0 Then
            Vndr_Nm1 = Trim(prn_DetDt.Rows(0).Item("Vendor_Name").ToString)
            Vndr_Nm2 = ""
            If Len(Vndr_Nm1) > 25 Then
                For i = 25 To 1 Step -1
                    If Mid$(Trim(Vndr_Nm1), i, 1) = " " Or Mid$(Trim(Vndr_Nm1), i, 1) = "," Or Mid$(Trim(Vndr_Nm1), i, 1) = "." Or Mid$(Trim(Vndr_Nm1), i, 1) = "-" Or Mid$(Trim(Vndr_Nm1), i, 1) = "/" Or Mid$(Trim(Vndr_Nm1), i, 1) = "_" Or Mid$(Trim(Vndr_Nm1), i, 1) = "(" Or Mid$(Trim(Vndr_Nm1), i, 1) = ")" Or Mid$(Trim(Vndr_Nm1), i, 1) = "\" Or Mid$(Trim(Vndr_Nm1), i, 1) = "[" Or Mid$(Trim(Vndr_Nm1), i, 1) = "]" Or Mid$(Trim(Vndr_Nm1), i, 1) = "{" Or Mid$(Trim(Vndr_Nm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 25
                Vndr_Nm2 = Microsoft.VisualBasic.Right(Trim(Vndr_Nm1), Len(Vndr_Nm1) - i)
                Vndr_Nm1 = Microsoft.VisualBasic.Left(Trim(Vndr_Nm1), i - 1)
            End If
        End If


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add2, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "VENDOR", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Vndr_Nm1), LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add2, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOCATION", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.Ledger_IdNoToName(con, prn_HdDt.Rows(0).Item("Location_IdNo"))), LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)

        If prn_HdDt.Rows(0).Item("Set_No").ToString <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Set_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add3, LMargin + 10, CurY, 0, 0, pFont)
        If Trim(Vndr_Nm2) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(Vndr_Nm2), LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Led_TinNo, LMargin + 25, CurY, 0, 0, pFont)
            End If

        Else
            If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + 25, CurY, 0, 0, pFont)
            End If

        End If
        'Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


        ClArr(1) = Val(200) : ClArr(2) = 200
        ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "As Per Your Request We Have Deliverd The Below Mentioned Items", LMargin + 20, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            W1 = e.Graphics.MeasureString(Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString), pFont).Width

            If Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) <> "" Then
                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & " (" & BmsInWrds & ") empty beams  (Width : " & Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) & " )"

            Else
                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & " (" & BmsInWrds & ") empty beams "

            End If
            Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)

        End If

        CurY = CurY + TxtHgt + 5
        If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Then
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) & " (" & BmsInWrds & ") empty bags "

            Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt + 5
        If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_cones").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) & " (" & BmsInWrds & ") empty cones "


            Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5

        Common_Procedures.Print_To_PrintDocument(e, "Through vehicle no. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        If prn_HdDt.Rows(0).Item("Remarks").ToString <> "" Then
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Remarks : " & prn_HdDt.Rows(0).Item("Remarks").ToString, LMargin + 20, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 260, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        End If
        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
        End If

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, LMargin + PageWidth - 35, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                ' prn_DetIndx = 0
                prn_PageNo = 0
                '  prn_DetIndx = 0
                prn_PageNo = 0
                '  prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, pFont1 As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Single = 0
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim NoofItems_PerPage As Integer
        Dim AmtInWrds As String = ""
        Dim PrnHeading As String = ""
        Dim I As Integer
        Dim time As String = ""

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                'PageSetupDialog1.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0 ' 65
            .Right = 0 ' 50
            .Top = 0 ' 65
            .Bottom = 0 ' 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        pFont1 = New Font("Calibri", 8, FontStyle.Regular)

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

        TxtHgt = 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        NoofItems_PerPage = 5

        Try

            'For I = 100 To 1100 Step 300

            '    CurY = I
            '    For J = 1 To 850 Step 40

            '        CurX = J
            '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
            '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

            '        CurX = J + 20
            '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

            '    Next

            'Next

            'For I = 200 To 800 Step 250

            '    CurX = I
            '    For J = 1 To 1200 Step 40

            '        CurY = J
            '        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

            '        CurY = J + 20
            '        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

            '    Next

            'Next

            'e.HasMorePages = False

            CurX = LMargin + 340
            CurY = TMargin + 100
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "BEAMS DELIVERY NOTE", CurX, CurY, 0, 0, p1Font)


            CurX = LMargin + 80
            CurY = TMargin + 140
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO : " & prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString, CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 340
            Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, p1Font)

            time = TimeOfDay.ToString("h:mm:ss tt")

            CurX = LMargin + 580
            Common_Procedures.Print_To_PrintDocument(e, "TIME : " & (time), CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            CurX = LMargin + 60
            e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 730, CurY)

            CurX = LMargin + 65 ' 40  '150
            CurY = TMargin + 180 ' 122 ' 100
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "To M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, CurX + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, CurX + 20, CurY, 0, 0, pFont)
            End If

            CurX = LMargin + 300 ' 40  '150
            CurY = TMargin + 240 ' 122 ' 100
            Common_Procedures.Print_To_PrintDocument(e, "We have Delivered the following", CurX, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            CurX = LMargin + 60
            e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 730, CurY)

            CurX = LMargin + 65 ' 40  '150
            CurY = TMargin + 265 ' 122 ' 100
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " Particulars     ", CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 350 ' 40  '150
            Common_Procedures.Print_To_PrintDocument(e, "Size in Inches ", CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 580 ' 40  '150
            Common_Procedures.Print_To_PrintDocument(e, " Quantity", CurX, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            CurX = LMargin + 60
            e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 730, CurY)


            CurY = 320 ' 370

            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAMS", 70, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString), LMargin + 350, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + 700, CurY, 1, 0, pFont)


            CurY = TMargin + 390
            e.Graphics.DrawLine(Pens.Black, LMargin + 60, CurY, LMargin + 790, CurY)

            CurX = LMargin + 200
            CurY = TMargin + 400
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", CurX, CurY, 0, 0, pFont)

            CurX = LMargin + 550
            CurY = TMargin + 400


            CurX = LMargin + 700
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), "########0"), CurX, CurY, 1, 0, pFont)

            'CurY = TMargin + 440
            'e.Graphics.DrawLine(Pens.Black, LMargin + 60, CurY, LMargin + 790, CurY)

            CurY = TMargin + 440
            e.Graphics.DrawLine(Pens.Black, LMargin + 330, CurY, LMargin + 330, TMargin + 260)
            e.Graphics.DrawLine(Pens.Black, LMargin + 550, CurY, LMargin + 550, TMargin + 260)

            CurX = LMargin + 200
            CurY = TMargin + 450
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) & "    Duplicate for Book No . B1", CurX, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Invoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Invoice.Click
        prn_Status = 1
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint.Click
        prn_Status = 2
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub
    Private Sub dgv_details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit

        dgv_Details_CellLeave(sender, e)

    End Sub

    Private Sub btn_Close_DosPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_DosPrint.Click
        pnl_back.Enabled = True
        Pnl_DosPrint.Visible = False
    End Sub

    Private Sub Btn_DosPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_DosPrint.Click
        prn_Status = 1
        Printing_Format3()
        btn_Close_DosPrint_Click(sender, e)
    End Sub

    Private Sub Btn_LaserPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_LaserPrint.Click
        prn_Status = 2
        printing_invoice()
        btn_Close_DosPrint_Click(sender, e)
    End Sub
    Private Sub Get_DosLoneDetails()
        Hz1 = Common_Procedures.Dos_DottedLines.Hz1
        Hz2 = Common_Procedures.Dos_DottedLines.Hz2
        Vz1 = Common_Procedures.Dos_DottedLines.Vz1
        Vz2 = Common_Procedures.Dos_DottedLines.Vz2
        Corn1 = Common_Procedures.Dos_DottedLines.Corn1
        Corn2 = Common_Procedures.Dos_DottedLines.Corn2
        Corn3 = Common_Procedures.Dos_DottedLines.Corn3
        Corn4 = Common_Procedures.Dos_DottedLines.Corn4
        LfCon = Common_Procedures.Dos_DottedLines.LfCon
        RgtCon = Common_Procedures.Dos_DottedLines.RgtCon
    End Sub

    Private Sub Printing_Format3()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Sizing_Empty_BeamBagCone_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.Empty_Beam AS BEAMS, b.Beam_Width_name from Sizing_Empty_BeamBagCone_Delivery_Details a LEFT OUTER JOIN Beam_Width_hEAD b ON a.Beam_Width_IdNo = b.Beam_Width_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        Get_DosLoneDetails()
        LnCnt = 0

        pth = Common_Procedures.Dos_Printing_FileName_Path

        If File.Exists(pth) = False Then
            fs = New FileStream(pth, FileMode.Create)
            w = New StreamWriter(fs)
            w.Close()
            fs.Close()
            w.Dispose()
            fs.Dispose()
        End If

        Try

            If prn_HdDt.Rows.Count > 0 Then

                If File.Exists(pth) = True Then Printing_Format3_PageHeader()
                prn_DetIndx = 0
                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        PrnTxt = Chr(Vz1) & Space(6) & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Beam_Width_Name").ToString) & Space(10 - Len(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Beam_Width_Name").ToString))) & Space(17 - Len(Trim(prn_DetDt.Rows(prn_DetIndx).Item("BEAMS").ToString))) & Trim(prn_DetDt.Rows(prn_DetIndx).Item("BEAMS").ToString) & Space(5) & Space(40) & Chr(Vz1)
                        w.WriteLine(PrnTxt)
                        LnCnt = LnCnt + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format3_PageFooter()

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

                'MessageBox.Show("Printed Sucessfully!!!", "PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            w.Close()
            'fs.Close()
            w.Dispose()
            'fs.Dispose()

        End Try

    End Sub

    Public Sub Printing_Format3_PageHeader()
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String


        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : Cmp_Add4 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Try
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString
            Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
                Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
                Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
            End If

            If File.Exists(pth) = True Then w = New StreamWriter(pth)

            PrnTxt = Chr(27) & "@" & Chr(18) & Chr(27) & "P" & Chr(27) & "t1" & Chr(27) & "2" & Chr(27) & "x0"
            LnCnt = LnCnt + 1

            PrnTxt = ""
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(39 - Len(Cmp_Name)) & Chr(14) & Chr(27) & "E" & Cmp_Name & Chr(27) & "F" & Chr(20) & Space(39 - Len(Cmp_Name)) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            If Len(Trim(Cmp_Add1 & " " & Cmp_Add2)) > 78 Then
                PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1) & Chr(13) & Space(2) & Chr(15) & Space(65 - ((Len((Cmp_Add1) & " " & (Cmp_Add2)) / 2) + 0.1)) & Trim(Cmp_Add1 & " " & Cmp_Add2) & Chr(18)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
                PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1) & Chr(13) & Space(2) & Chr(15) & Space(65 - ((Len((Cmp_Add3) & " " & (Cmp_Add4)) / 2) + 0.1)) & Trim(Cmp_Add3 & " " & Cmp_Add4) & Chr(18)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            Else
                PrnTxt = Chr(Vz1) & Space(39 - ((Len((Cmp_Add1) & " " & (Cmp_Add2)) / 2) + 0.1)) & Trim(Cmp_Add1 & " " & Cmp_Add2) & Space(39 - ((Len(Cmp_Add1 & " " & Cmp_Add2) / 2) + 0.1)) & Space(Len(Cmp_Add1 & " " & Cmp_Add2) Mod 2) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
                PrnTxt = Chr(Vz1) & Space(39 - ((Len((Cmp_Add3) & " " & (Cmp_Add4)) / 2) + 0.1)) & Trim(Cmp_Add3 & " " & Cmp_Add4) & Space(39 - ((Len(Cmp_Add3 & " " & Cmp_Add4) / 2) + 0.1)) & Space(Len(Cmp_Add3 & " " & Cmp_Add4) Mod 2) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            End If

            PrnTxt = Chr(Vz1) & Space(35 - Math.Round((Len(Cmp_PhNo) / 2) + 0.1)) & "Phone : " & Trim(Cmp_PhNo) & Space(35 - Math.Round((Len(Cmp_PhNo) / 2) + 0.1)) & Space(Len(Cmp_PhNo) Mod 2) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(21) & Chr(14) & Chr(27) & "E" & "EMPTY BEAM DELIVERY" & Chr(27) & "F" & Chr(18) & Space(21) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(LfCon) & StrDup(39, Chr(Hz2)) & Chr(194) & StrDup(38, Chr(Hz2)) & Chr(RgtCon)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(1) & "From & " & Space(31) & Chr(Vz2) & Space(38) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & "M/s." & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString) & Space(31 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString))) & Chr(Vz2) & Space(1) & "REC NO & " & Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString) & Space(28 - Len(Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString))) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString))) & Chr(Vz2) & Space(38) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString))) & Chr(Vz2) & Space(1) & "DATE   & " & Trim(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_dATE").ToString), "dd-MM-yyyy").ToString) & Space(28 - Len(Trim(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_DATE").ToString), "dd-MM-yyyy").ToString))) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString))) & Chr(Vz2) & Space(38) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString))) & Chr(Vz2) & Space(1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            'SUB HEADING

            PrnTxt = Chr(LfCon) & StrDup(39, Chr(Hz2)) & Chr(193) & StrDup(38, Chr(Hz2)) & Chr(RgtCon)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1


            PrnTxt = Chr(Vz1) & Space(5) & "We deliverd your " & Space(56) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(5) & "-----------------" & Space(56) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            If Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString) <> 0 Then
                PrnTxt = Chr(Vz1) & Space(10) & Space(17 - Len(Trim(prn_HdDt.Rows(0).Item("Empty_Bags").ToString))) & Trim(prn_HdDt.Rows(0).Item("Empty_Bags").ToString) & "  empty bags" & Space(38) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            Else
                PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            End If
            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                PrnTxt = Chr(Vz1) & Space(10) & Space(17 - Len(Trim(prn_HdDt.Rows(0).Item("Empty_Cones").ToString))) & Trim(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) & "  empty Cones" & Space(37) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            Else
                PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            End If
            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                PrnTxt = Chr(Vz1) & Space(10) & Space(17 - Len(Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))) & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) & "  empty beams" & Space(37) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            Else
                PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            End If

            PrnTxt = Chr(Vz1) & Space(5) & "-----------------" & Space(57) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1



        Catch ex As Exception
            w.Close()
            w.Dispose()
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_PageFooter()
        Dim EBm_Txt As String = ""
        Dim EBm_Wdth As String = ""
        Dim Cmp_Name As String = ""
        Dim BmsInWrds As String

        Try

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")


            PrnTxt = Chr(Vz1) & Space(5) & "Through vechile no. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) & Space(53 - Len(Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString))) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(LfCon) & StrDup(78, Chr(Hz2)) & Chr(RgtCon)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & " Signature of the Receiver    " & Space(43 - Len(Cmp_Name)) & "For " & Cmp_Name & Space(1) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Corn3) & StrDup(78, Chr(Hz1)) & Chr(Corn4)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            For I = LnCnt To 36
                PrnTxt = ""
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            Next

        Catch ex As Exception
            w.Close()
            w.Dispose()
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Rect As Rectangle

        With dgv_Details

            ' dgv_ActCtrlName = .Name.ToString

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
            If e.ColumnIndex = 2 Then

                If cbo_Vendor.Visible = False Or Val(cbo_Vendor.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_Details.Name

                    cbo_Vendor.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Vendor_Name from Vendor_Head Order by Vendor_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Vendor.DataSource = Dt1
                    cbo_Vendor.DisplayMember = "Vendor_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Vendor.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Vendor.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Vendor.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_Vendor.Height = Rect.Height  ' rect.Height

                    cbo_Vendor.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Vendor.Tag = Val(e.RowIndex)
                    cbo_Vendor.Visible = True

                    cbo_Vendor.BringToFront()
                    cbo_Vendor.Focus()



                End If

            Else

                cbo_Vendor.Visible = False

            End If


            If e.ColumnIndex = 3 Then

                If cbo_beamwidth.Visible = False Or Val(cbo_beamwidth.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_Details.Name

                    cbo_beamwidth.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from Beam_Width_Head Order by Beam_Width_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_beamwidth.DataSource = Dt2
                    cbo_beamwidth.DisplayMember = "Beam_Width_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_beamwidth.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_beamwidth.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_beamwidth.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_beamwidth.Height = Rect.Height  ' rect.Height

                    cbo_beamwidth.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_beamwidth.Tag = Val(e.RowIndex)
                    cbo_beamwidth.Visible = True

                    cbo_beamwidth.BringToFront()
                    cbo_beamwidth.Focus()



                End If

            Else

                cbo_beamwidth.Visible = False

            End If



        End With

    End Sub



    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0")
                End If
            End If
        End With
        Total_Calculation()
    End Sub

    Private Sub dgv_details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 1 Then

                    Total_Calculation()

                End If

            End If
        End With

    End Sub

    Private Sub dgv_details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        'dgv_ActCtrlName = dgv_Details.Name
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details

            If Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 1 Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

            End If

        End With

    End Sub

    Private Sub dgv_details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            Total_Calculation()

        End If

    End Sub

    Private Sub dgv_details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With

    End Sub

    Private Sub dgv_details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Total_Calculation()
        Dim vTotetybm As Single
        Dim i As Integer
        Dim sno As Integer
        If FrmLdSTS = True Then Exit Sub
        vTotetybm = 0
        With dgv_Details
            For i = 0 To .Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(.Rows(i).Cells(1).Value) <> 0 Then

                    vTotetybm = vTotetybm + Val(.Rows(i).Cells(1).Value)


                End If
            Next
        End With

        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()

        dgv_Details_Total.Rows(0).Cells(1).Value = Val(vTotetybm)
        ' dgv_etails_Total.Rows(0).Cells(4).Value = Format(Val(vTotMtrs), "#########0.000")

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
            ElseIf pnl_back.Enabled = True Then
                dgv1 = dgv_Details
            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    dtp_date.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_remarks.Focus()

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

                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub cbo_beamwidth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_beamwidth.TextChanged
        Try
            If FrmLdSTS = True Then Exit Sub
            If cbo_beamwidth.Visible Then
                With dgv_Details
                    If Val(cbo_beamwidth.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_beamwidth.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_bagType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_bagType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_bagType, cbo_vehicleno, cbo_coneType, "Bag_Type_Head", "Bag_Type_Name", "", "(Bag_Type_Idno = 0)")

    End Sub

    Private Sub cbo_bagType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_bagType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_bagType, cbo_coneType, "Bag_Type_Head", "Bag_Type_Name", "", "(Bag_Type_Idno = 0)")

    End Sub

    Private Sub cbo_bagType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_bagType.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Bag_Type_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_bagType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_coneType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_coneType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_coneType, cbo_bagType, cbo_Transport, "ConeType_Head", "ConeType_Name", "", "(ConeType_Idno = 0)")

    End Sub

    Private Sub cbo_coneType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_coneType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_coneType, cbo_Transport, "ConeType_Head", "ConeType_Name", "", "(ConeType_Idno = 0)")

    End Sub

    Private Sub cbo_coneType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_coneType.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New ConeType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_coneType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_Vendor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vendor.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vendor_AlaisHead", "Vendor_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_Vendor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vendor.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vendor, Nothing, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Vendor_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Vendor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Vendor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(3)




            End If

        End With
    End Sub



    Private Sub cbo_Vendor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vendor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vendor, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Vendor_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(3)


            End With

        End If

    End Sub

    Private Sub cbo_Vendor_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vendor.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Vendor_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Vendor.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Vendor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vendor.TextChanged
        Try
            If cbo_Vendor.Visible Then
                With dgv_Details
                    If Val(cbo_Vendor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Vendor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String

        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim Led_IdNo As Integer = 0

        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim Cmp_Typ As String = ""

        Try

            Cmp_Typ = Trim(Common_Procedures.get_FieldValue(con, "Company_Head", "Company_Type", ""))

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)
            'If Led_IdNo  = 0 Then Exit Sub

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                smstxt = "EMPTY BEAM & BAG DELIVERY" & vbCrLf
            Else
                smstxt = "EMPTY BEAM BAG CONE DELIVERY" & vbCrLf
                smstxt = smstxt & vbCrLf
            End If

            smstxt = smstxt & "DC.NO-" & Trim(lbl_dcno.Text) & vbCrLf & "DATE-" & Trim(dtp_date.Text)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            da2 = New SqlClient.SqlDataAdapter("select Empty_Beam from Sizing_Empty_BeamBagCone_Delivery_Head where Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If Val(dt2.Rows(i).Item("Empty_Beam").ToString) <> 0 Then
                    smstxt = smstxt & " " & vbCrLf
                    smstxt = smstxt & "TOTAL EMPTY BEAMS : " & Trim(dt2.Rows(i).Item("Empty_Beam").ToString)
                End If
            End If
            dt2.Clear()

            If Val(txt_emptybags.Text) <> 0 Then
                smstxt = smstxt & " " & vbCrLf
                smstxt = smstxt & "TOTAL EMPTY BAG : " & Trim(txt_emptybags.Text)
            End If

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then       '---- Ganesh karthik Sizing (Somanur)

            'End If

            smstxt = smstxt & vbCrLf & " Thanks! " & vbCrLf
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
                smstxt = smstxt & "GKT SIZING "
            Else '
                smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))
            End If

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

    Private Sub cbo_Delivered_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Delivered.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delivered, cbo_partyname, Nothing, "Sizing_Empty_BeamBagCone_Delivery_Head", "Delivered_To", "", "(Empty_BeamBagCone_Delivery_No = 0)")

        If (e.KeyCode = 40 And cbo_Delivered.DroppedDown = False) Or (e.KeyCode = 40 And e.Control = True) Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
                txt_SetNo.Focus()
            Else
                txt_BookNo.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_Delivered_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Delivered.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delivered, Nothing, "Sizing_Empty_BeamBagCone_Delivery_Head", "Delivered_To", "", "(Empty_BeamBagCone_Delivery_No = 0)", False)


        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
                txt_SetNo.Focus()
            Else
                txt_BookNo.Focus()
            End If

        End If


    End Sub


    'Private Sub txt_BookNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BookNo.KeyPress
    '    If Asc(e.KeyChar) = 38 Then
    '        cbo_Delivered.Focus()
    '    End If
    'End Sub

    Private Sub Update_PrintOut_Status(Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing)
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim vPrnSTS As Integer = 0


        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            If IsNothing(sqltr) = False Then
                cmd.Transaction = sqltr
            End If

            vPrnSTS = 0
            If chk_Printed.Checked = True Then
                vPrnSTS = 1
            End If

            cmd.CommandText = "Update Sizing_Empty_BeamBagCone_Delivery_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
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

    Private Sub PrintPreview_Shown(ByVal sender As Object, ByVal e As System.EventArgs)
        'Capture the click events for the toolstrip in the dialog box when the dialog is shown
        Dim ts As ToolStrip = CType(sender.Controls(1), ToolStrip)
        AddHandler ts.ItemClicked, AddressOf PrintPreview_Toolstrip_ItemClicked
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
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_coneType, txt_remarks, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_remarks, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub
    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Transport_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    'Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
    '    If prn_HdDt.Rows.Count <= 0 Then Exit Sub

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (Palladam)
    '        If prn_Status = 1 Then
    '            'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
    '            Printing_Format1(e)
    '        Else
    '            Printing_Format2(e)
    '        End If 'End 
    '    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
    '        Printing_Format3(e)
    '    Else
    '        Printing_Format1(e)
    '    End If

    'End Sub

    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim pFontBold As Font = New Font("Calibri", 8, FontStyle.Bold)
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        'Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PS As Printing.PaperSize
        Dim CurX As Single = 0, strWidth As Single = 0
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        Dim Ledname1 As String
        Dim Ledname2 As String
        Dim i As Integer
        Dim cmp_userName As String = "", Cmp_Divi As String = ""


        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            PS = PrintDocument1.PrinterSettings.PaperSizes(i)
            Debug.Print(PS.PaperName)
            If PS.Width = 800 And PS.Height = 600 Then
                PrintDocument1.DefaultPageSettings.PaperSize = PS
                e.PageSettings.PaperSize = PS
                PpSzSTS = True
                Exit For
            End If
        Next

        For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(i).Kind = Printing.PaperKind.A5 Then
                PS = PrintDocument1.PrinterSettings.PaperSizes(i)
                PrintDocument1.DefaultPageSettings.PaperSize = PS
                e.PageSettings.PaperSize = PS
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(i).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(i)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    Exit For
                End If
            Next
        End If


        'For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(i).Kind = Printing.PaperKind.A4 Then
        '        PS = PrintDocument1.PrinterSettings.PaperSizes(i)
        '        PrintDocument1.DefaultPageSettings.PaperSize = PS
        '        e.PageSettings.PaperSize = PS
        '        Exit For
        '    End If
        'Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20  ' 50 
            .Right = 55
            .Top = 35   '30
            .Bottom = 35 ' 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 8, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 21 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString
            Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

        Else

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
                Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString
                Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString
                Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

            End If

        End If

        'Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        'Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        'Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
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
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                Led_GstNo = "GSTIN" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Led_TinNo = " TIN NO  " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
            End If
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + strHeight
        p1Font = New Font("Calibri", 8, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin, CurY, 2, PrintWidth, p1Font)

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt - 13  ' 10

        Else

            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), p1Font).Width
            If PrintWidth > strWidth Then
                CurX = LMargin + (PrintWidth - strWidth) / 2
            Else
                CurX = LMargin
            End If

            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
            strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, p1Font)

            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, p1Font)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 1

        End If
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM/BAG DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 250
        C2 = PageWidth - (LMargin + C1)

        W1 = e.Graphics.MeasureString("BOOK NO : ", pFont).Width

        Ledname1 = Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)
        '  End If

        Ledname2 = ""

        If Len(Ledname1) > 40 Then
            For i = 40 To 1 Step -1
                If Mid$(Trim(Ledname1), i, 1) = " " Or Mid$(Trim(Ledname1), i, 1) = "," Or Mid$(Trim(Ledname1), i, 1) = "." Or Mid$(Trim(Ledname1), i, 1) = "-" Or Mid$(Trim(Ledname1), i, 1) = "/" Or Mid$(Trim(Ledname1), i, 1) = "_" Or Mid$(Trim(Ledname1), i, 1) = "(" Or Mid$(Trim(Ledname1), i, 1) = ")" Or Mid$(Trim(Ledname1), i, 1) = "\" Or Mid$(Trim(Ledname1), i, 1) = "[" Or Mid$(Trim(Ledname1), i, 1) = "]" Or Mid$(Trim(Ledname1), i, 1) = "{" Or Mid$(Trim(Ledname1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 40
            Ledname2 = Microsoft.VisualBasic.Right(Trim(Ledname1), Len(Ledname1) - i)
            Ledname1 = Microsoft.VisualBasic.Left(Trim(Ledname1), i - 1)
        End If


        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & Ledname1, LMargin + 10, CurY, 0, 0, p1Font)


        If Trim(Ledname2) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & Trim(Ledname2), LMargin + 10, CurY, 0, 0, p1Font)
            'NoofDets = NoofDets + 1
        End If

        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add1, LMargin + 10, CurY, 0, 0, pFont)


        If prn_DetDt.Rows.Count > 0 Then
            If Not IsNothing(prn_DetDt.Rows(0).Item("Vendor_Name")) Then
                If prn_DetDt.Rows(0).Item("Vendor_Name") <> "" Then
                    CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add2, LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "VENDOR", LMargin + C1 + 10, CurY, 0, 0, pFontBold)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFontBold)
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Vendor_Name").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFontBold)

                End If
            End If
        End If





        CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add2, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFontBold)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFontBold)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFontBold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFontBold)

        If prn_HdDt.Rows(0).Item("Set_No").ToString <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Set_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        If prn_HdDt.Rows(0).Item("Vehicle_No").ToString <> "" Then
            CurY = CurY + TxtHgt
            'p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add3, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Led_TinNo, LMargin + 25, CurY, 0, 0, pFont)
            End If

        Else
            If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + 25, CurY, 0, 0, pFont)
            End If

        End If
        'Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


        ClArr(1) = Val(200) : ClArr(2) = 200
        ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "As Per Your Request We Have Deliverd The Below Mentioned Items", LMargin + 20, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            W1 = e.Graphics.MeasureString(Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString), pFont).Width

            If Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) <> "" Then
                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & " (" & BmsInWrds & ") empty beams  (Width : " & Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) & " )"

            Else
                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & " (" & BmsInWrds & ") empty beams "

            End If
            Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)

        End If

        CurY = CurY + TxtHgt + 5
        If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Then
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) & " (" & BmsInWrds & ") empty bags "

            Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt + 5
        If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_cones").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) & " (" & BmsInWrds & ") empty cones "


            Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5

        'Common_Procedures.Print_To_PrintDocument(e, "Through vehicle no. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        If prn_HdDt.Rows(0).Item("Remarks").ToString <> "" Then
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Remarks : " & prn_HdDt.Rows(0).Item("Remarks").ToString, LMargin + 20, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        End If

        CurY = CurY + TxtHgt - 15
        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        End If


        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        If Val(Common_Procedures.User.IdNo) <> 1 Then
            cmp_userName = Trim(Common_Procedures.User.Name)
            'Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 260, CurY, 0, 0, pFont)
        End If

        Cmp_Divi = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Divi, PageWidth - 60, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, cmp_userName, PageWidth - 60, CurY, 1, 0, p1Font)
        'CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt


        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 180, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
        End If



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                ' prn_DetIndx = 0
                prn_PageNo = 0
                '  prn_DetIndx = 0
                prn_PageNo = 0
                '  prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format5(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        'Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PS As Printing.PaperSize
        Dim CurX As Single = 0, strWidth As Single = 0
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        Dim Ledname1 As String
        Dim Ledname2 As String
        Dim i As Integer
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0



        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1


        If Common_Procedures.settings.EmptyBeamBagConeDelivery_Print_2Copy_In_SinglePage = 1 Then
            For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                PS = PrintDocument1.PrinterSettings.PaperSizes(i)
                Debug.Print(PS.PaperName)
                If PS.Width = 800 And PS.Height = 600 Then
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    PpSzSTS = True
                    Exit For
                End If
            Next

        Else
            If PpSzSTS = False Then
                For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(i).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(i)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument1.PrinterSettings.PaperSizes(i).Kind = Printing.PaperKind.A4 Then
                            PS = PrintDocument1.PrinterSettings.PaperSizes(i)
                            PrintDocument1.DefaultPageSettings.PaperSize = PS
                            e.PageSettings.PaperSize = PS
                            Exit For
                        End If
                    Next
                End If

            End If

            'For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    If PrintDocument1.PrinterSettings.PaperSizes(i).Kind = Printing.PaperKind.A4 Then
            '        PS = PrintDocument1.PrinterSettings.PaperSizes(i)
            '        PrintDocument1.DefaultPageSettings.PaperSize = PS
            '        e.PageSettings.PaperSize = PS
            '        Exit For
            '    End If
            'Next

        End If

        If PrntCnt2ndPageSTS = False Then
            PrntCnt = 2
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20  ' 50 
            .Right = 55
            .Top = 35   '30
            .Bottom = 35 ' 30
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

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        TxtHgt = 19.75 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin



        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0
                    ' prn_NoofBmDets = 0
                    TpMargin = TMargin

                Else

                    prn_PageNo = 0
                    ' prn_NoofBmDets = 0
                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If
            End If

            'Erase LnAr
            'Erase ClArr


            CurY = TpMargin
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
            Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
            Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            Else

                If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                    Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
                    Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
                    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

                Else
                    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
                    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

                End If

            End If

            'Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            'Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            'Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
                Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
                Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
                Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
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
            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""

            If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
                Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
            Else
                Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
                Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
                Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
                Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                    Led_GstNo = "GSTIN" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
                End If
                If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    Led_TinNo = " TIN NO  " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
                End If
            End If

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Else
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
            End If
            CurY = CurY + strHeight
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
            CurY = CurY + strHeight
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

            Gst_dt = #7/1/2017#
            Entry_dt = dtp_date.Value

            If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt - 13  ' 10

            Else

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
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)
                'CurY = CurY + TxtHgt - 1
                'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, PageWidth - 10, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt - 1

            End If
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM/BAG DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)

            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            CurY = CurY + strHeight
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

            C1 = 450
            C2 = PageWidth - (LMargin + C1)

            W1 = e.Graphics.MeasureString("BOOK NO : ", pFont).Width

            Ledname1 = Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)
            '  End If

            Ledname2 = ""

            If Len(Ledname1) > 40 Then
                For i = 40 To 1 Step -1
                    If Mid$(Trim(Ledname1), i, 1) = " " Or Mid$(Trim(Ledname1), i, 1) = "," Or Mid$(Trim(Ledname1), i, 1) = "." Or Mid$(Trim(Ledname1), i, 1) = "-" Or Mid$(Trim(Ledname1), i, 1) = "/" Or Mid$(Trim(Ledname1), i, 1) = "_" Or Mid$(Trim(Ledname1), i, 1) = "(" Or Mid$(Trim(Ledname1), i, 1) = ")" Or Mid$(Trim(Ledname1), i, 1) = "\" Or Mid$(Trim(Ledname1), i, 1) = "[" Or Mid$(Trim(Ledname1), i, 1) = "]" Or Mid$(Trim(Ledname1), i, 1) = "{" Or Mid$(Trim(Ledname1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 40
                Ledname2 = Microsoft.VisualBasic.Right(Trim(Ledname1), Len(Ledname1) - i)
                Ledname1 = Microsoft.VisualBasic.Left(Trim(Ledname1), i - 1)
            End If


            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & Ledname1, LMargin + 10, CurY, 0, 0, p1Font)


            If Trim(Ledname2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "     " & Trim(Ledname2), LMargin + 10, CurY, 0, 0, p1Font)
                'NoofDets = NoofDets + 1
            End If

            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add1, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add2, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)

            If prn_HdDt.Rows(0).Item("Set_No").ToString <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Set_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add3, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
                If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Led_TinNo, LMargin + 25, CurY, 0, 0, pFont)
                End If

            Else
                If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + 25, CurY, 0, 0, pFont)
                End If

            End If
            'Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


            ClArr(1) = Val(200) : ClArr(2) = 200
            ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "As Per Your Request We Have Deliverd The Below Mentioned Items", LMargin + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
                BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

                W1 = e.Graphics.MeasureString(Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString), pFont).Width

                If Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) <> "" Then
                    SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & " (" & BmsInWrds & ") empty beams  (Width : " & Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) & " )"

                Else
                    SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & " (" & BmsInWrds & ") empty beams "

                End If
                Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)

            End If

            CurY = CurY + TxtHgt + 5
            If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString))
                BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) & " (" & BmsInWrds & ") empty bags "

                Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5
            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_cones").ToString))
                BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

                SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) & " (" & BmsInWrds & ") empty cones "


                Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5

            Common_Procedures.Print_To_PrintDocument(e, "Through vehicle no. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            If prn_HdDt.Rows(0).Item("Remarks").ToString <> "" Then
                CurY = CurY + TxtHgt - 15
                Common_Procedures.Print_To_PrintDocument(e, "Remarks : " & prn_HdDt.Rows(0).Item("Remarks").ToString, LMargin + 20, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 260, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            End If
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
                Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            End If

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, LMargin + PageWidth - 35, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))


            If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 5 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End If
            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP2:


        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                ' prn_DetIndx = 0
                prn_PageNo = 0
                '  prn_DetIndx = 0
                prn_PageNo = 0
                '  prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub cbo_coneType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_coneType.SelectedIndexChanged

    End Sub

    Private Sub cbo_Location_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Location.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_Location_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Location.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Location, cbo_vehicleno, txt_remarks, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Location_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Location.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Location, txt_remarks, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Location_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Location.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Transport_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Location.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_vehicleno_GotFocus(sender As Object, e As EventArgs) Handles cbo_vehicleno.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_Empty_BeamBagCone_Delivery_Head", "Vehicle_No", "", "(Empty_BeamBagCone_Delivery_No = 0)")
    End Sub

    Private Sub cbo_Delivered_GotFocus(sender As Object, e As EventArgs) Handles cbo_Delivered.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_Empty_BeamBagCone_Delivery_Head", "Delivered_To", "", "(Empty_BeamBagCone_Delivery_No = 0)")
    End Sub


End Class