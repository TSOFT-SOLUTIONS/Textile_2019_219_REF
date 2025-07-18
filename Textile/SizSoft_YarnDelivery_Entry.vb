Imports System.IO
Public Class SizSoft_YarnDelivery_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "YNDLV-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private TrnTo_DbName As String = ""
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0

    Private fs As FileStream
    Private sw As StreamWriter

    Private Hz1 As Integer, Hz2 As Integer, Vz1 As Integer, Vz2 As Integer
    Private Corn1 As Integer, Corn2 As Integer, Corn3 As Integer, Corn4 As Integer
    Private LfCon As Integer, RgtCon As Integer

    Private Sub clear()

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        txt_BookNo.Text = ""
        cbo_Ledger.Text = ""
        cbo_VendorName.Text = ""
        txt_EmptyBags.Text = ""
        txt_EmptyCones.Text = ""
        txt_TexDcNo.Text = ""
        cbo_bagType.Text = ""
        cbo_coneType.Text = ""
        cbo_DeliveryTo.Text = ""
        txt_ElectronicRefNo.Text = ""
        txt_DateAndTimeOFSupply.Text = ""
        txt_Approx_Value.Text = ""
        txt_EmptyBeam.Text = ""
        cbo_BeamWidth.Text = ""
        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        txt_DeliveryAt.Text = ""
        txt_Remarks.Text = ""
        dtp_Time.Text = ""
        lbl_AvailableStock.Tag = 0
        lbl_AvailableStock.Text = ""
        cbo_Delivered.Text = ""

        txt_SlNo.Text = ""
        cbo_CountName.Text = ""
        cbo_YarnType.Text = "MILL"
        cbo_MillName.Text = ""
        txt_Bags.Text = ""
        cbo_SetNo.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""
        cbo_godown.Text = ""

        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Ledger.Tag = ""
        cbo_YarnType.Tag = ""
        cbo_CountName.Tag = ""
        cbo_MillName.Tag = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        Grid_Cell_DeSelect()

        txt_SlNo.Text = "1"
        cbo_YarnType.Text = "MILL"
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dtp_Date.Enabled = True
        dtp_Date.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_VehicleNo.Enabled = True
        cbo_VehicleNo.BackColor = Color.White
        txt_EmptyBeam.Enabled = True
        txt_EmptyBeam.BackColor = Color.White
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

        'If Me.ActiveControl.Name <> cbo_ItemName.Name Then
        '    cbo_ItemName.Visible = False
        'End If
        'If Me.ActiveControl.Name <> cbo_PackingType.Name Then
        '    cbo_PackingType.Visible = False
        'End If




        Grid_Cell_DeSelect()

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
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Company_Name as PartyName, c.Transport_Name, d.Beam_Width_Name,  glh.Ledger_Name as Godown_Name from SizSoft_Yarn_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Transport_Head c ON a.Transport_IdNo = c.Transport_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_IdNo = d.Beam_Width_IdNo LEFT OUTER JOIN ledger_Head glh ON a.Godown_IdNo = glh.Ledger_IdNo Where a.Yarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_DcNo.Text = dt1.Rows(0).Item("Yarn_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Yarn_Delivery_Date").ToString

                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                txt_BookNo.Text = dt1.Rows(0).Item("Book_No").ToString
                txt_TexDcNo.Text = dt1.Rows(0).Item("Textile_Dc_No").ToString

                txt_EmptyBeam.Text = Val(dt1.Rows(0).Item("Empty_Beam").ToString)
                cbo_BeamWidth.Text = dt1.Rows(0).Item("Beam_Width_Name").ToString
                txt_EmptyBags.Text = dt1.Rows(0).Item("Empty_Bags").ToString
                txt_EmptyCones.Text = dt1.Rows(0).Item("Empty_Cones").ToString

                cbo_Transport.Text = dt1.Rows(0).Item("Transport_Name").ToString

                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_Delivered.Text = dt1.Rows(0).Item("Delivered_By").ToString

                txt_DeliveryAt.Text = dt1.Rows(0).Item("Delivery_At").ToString
                cbo_godown.Text = dt1.Rows(0).Item("Godown_Name").ToString

                dtp_Time.Text = (dt1.Rows(0).Item("Entry_Time_Text").ToString)
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                'cbo_bagType.Text = Common_Procedures.Bag_Type_IdNoToName(con, dt1.Rows(0).Item("Bag_Type_Idno").ToString)
                'cbo_coneType.Text = Common_Procedures.Cone_Type_IdNoToName(con, dt1.Rows(0).Item("Cone_Type_Idno").ToString)
                txt_ElectronicRefNo.Text = dt1.Rows(0).Item("Electronic_Reference_No").ToString
                txt_DateAndTimeOFSupply.Text = dt1.Rows(0).Item("Date_And_Time_Of_Supply").ToString
                txt_Approx_Value.Text = Format(Val(dt1.Rows(0).Item("approx_Value").ToString), "############0.00")
                'cbo_DeliveryTo.Text = Common_Procedures.Delivery_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                If Trim(cbo_DeliveryTo.Text) = "" Then
                    cbo_DeliveryTo.Text = txt_DeliveryAt.Text
                End If
                cbo_VendorName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Vendor_IdNo").ToString))
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                If Val(dt1.Rows(0).Item("Loaded_By_Our_Employee").ToString) = 1 Then chk_Loaded.Checked = True
                '**********
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
                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from SizSoft_Yarn_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Yarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Yarn_Type").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("setcode_forSelection").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Mill_Name").ToString
                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Weaver_Yarn_Requirement_No").ToString
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Weaver_Yarn_Requirement_Code").ToString
                        dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Weaver_Yarn_Requirement_Details_Slno").ToString
                        If IsDBNull(dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                            If Trim(dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                                For j = 0 To dgv_Details.ColumnCount - 1
                                    dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If
                        End If
                        dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Lot_No").ToString
                    Next i

                End If

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()
            If LockSTS = True Then

                dtp_Date.Enabled = False
                dtp_Date.BackColor = Color.LightGray

                cbo_Transport.Enabled = False
                cbo_Transport.BackColor = Color.LightGray

                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray

                cbo_VehicleNo.Enabled = False
                cbo_VehicleNo.BackColor = Color.LightGray

                txt_EmptyBeam.Enabled = False
                txt_EmptyBeam.BackColor = Color.LightGray



            End If
            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub YarnDelivery_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DELIVERY" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BeamWidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BeamWidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VendorName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VendorName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_godown.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "GODOWN" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_godown.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub YarnDelivery_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim dt9 As New DataTable
        Dim dt10 As New DataTable
        Dim dt11 As New DataTable
        Dim LedIdNo As String
        Me.Text = ""

        con.Open()

        Dim vDbName As String = ""

        If Trim(TrnTo_DbName) <> "" Then
            vDbName = Trim(TrnTo_DbName) & ".."
        End If

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text, , TrnTo_DbName)

        btn_Selection.Visible = False
        Panel2.Enabled = True
        dgv_Details.EditMode = DataGridViewEditMode.EditProgrammatically
        dgv_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        'If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
        '    TrnTo_DbName = Common_Procedures.get_Company_TextileDataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        '    btn_Selection.Visible = True
        '    'Panel2.Enabled = False
        '    'dgv_Details.EditMode = DataGridViewEditMode.EditOnEnter
        '    'dgv_Details.SelectionMode = DataGridViewSelectionMode.CellSelect
        '    cbo_Ledger.Width = cbo_Ledger.Width - btn_Selection.Width - 20
        'Else
        '    TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        '    btn_Selection.Visible = False
        'End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1267" Then
            lbl_Del_Vendor.Text = "Vendor"
            cbo_DeliveryTo.Visible = False
            txt_DeliveryAt.Visible = False
            cbo_VendorName.Visible = True
            cbo_VendorName.BringToFront()

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '---- Kalaimagal Sizing (Avinashi)
            lbl_Del_Vendor.Text = "Delivery To"
            cbo_DeliveryTo.Visible = False
            cbo_VendorName.Visible = False
            txt_DeliveryAt.Visible = True
            txt_DeliveryAt.BringToFront()
        Else
            lbl_Del_Vendor.Text = "Delivery To"
            cbo_DeliveryTo.Visible = True
            cbo_VendorName.Visible = False
            txt_DeliveryAt.Visible = False
            cbo_DeliveryTo.BringToFront()
        End If
        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 and a.Close_Status = 0 ) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
        da.Fill(dt2)
        cbo_CountName.DataSource = dt2
        cbo_CountName.DisplayMember = "count_name"

        da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", con)
        da.Fill(dt3)
        cbo_MillName.DataSource = dt3
        cbo_MillName.DisplayMember = "mill_name"

        da = New SqlClient.SqlDataAdapter("select Transport_Name from Transport_Head order by Transport_Name", con)
        da.Fill(dt4)
        cbo_Transport.DataSource = dt4
        cbo_Transport.DisplayMember = "Transport_Name"

        da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head order by Yarn_Type", con)
        da.Fill(dt5)
        cbo_YarnType.DataSource = dt5
        cbo_YarnType.DisplayMember = "Yarn_Type"

        da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from Beam_Width_Head order by Beam_Width_Name", con)
        da.Fill(dt6)
        cbo_BeamWidth.DataSource = dt6
        cbo_BeamWidth.DisplayMember = "Beam_Width_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Vehicle_No) from SizSoft_Yarn_Delivery_Head order by Vehicle_No", con)
        da.Fill(dt7)
        cbo_VehicleNo.DataSource = dt7
        cbo_VehicleNo.DisplayMember = "Vehicle_No"

        da = New SqlClient.SqlDataAdapter("select distinct(Delivered_By) from SizSoft_Yarn_Delivery_Head order by Delivered_By", con)
        da.Fill(dt11)
        cbo_Delivered.DataSource = dt11
        cbo_Delivered.DisplayMember = "Delivered_By"

        da = New SqlClient.SqlDataAdapter("select distinct(setcode_forSelection) from Stock_BabyCone_Processing_Details order by setcode_forSelection", con)
        da.Fill(dt8)
        cbo_SetNo.DataSource = dt8
        cbo_SetNo.DisplayMember = "setcode_forSelection"

        da = New SqlClient.SqlDataAdapter("select Bag_Type_Name from Bag_Type_Head order by Bag_Type_Name", con)
        da.Fill(dt9)
        cbo_bagType.DataSource = dt9
        cbo_bagType.DisplayMember = "Bag_Type_Name"

        da = New SqlClient.SqlDataAdapter("select Cone_Type_Name from Cone_Type_Head order by Cone_Type_Name", con)
        da.Fill(dt10)
        cbo_coneType.DataSource = dt10
        cbo_coneType.DisplayMember = "Cone_Type_Name"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.BringToFront()
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2

        Pnl_DosPrint.Visible = False
        Pnl_DosPrint.BringToFront()
        Pnl_DosPrint.Left = (Me.Width - Pnl_DosPrint.Width) \ 2
        Pnl_DosPrint.Top = (Me.Height - Pnl_DosPrint.Height) \ 2

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        ' btn_UserModification.Visible = False
        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            'btn_UserModification.Visible = True
            chk_Printed.Enabled = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            dgv_Details.Columns(8).Visible = False
            dgv_Details.Columns(11).Visible = True
        End If


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VendorName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BookNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TexDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_bagType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_coneType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ElectronicRefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateAndTimeOFSupply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Approx_Value.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Delivered.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_godown.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Cones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DeliveryAt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BeamWidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyCones.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ElectronicRefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateAndTimeOFSupply.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Approx_Value.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_godown.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Add.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TexDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VendorName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BookNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cones.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DeliveryAt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BeamWidth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyCones.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_bagType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_coneType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Delivered.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BookNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TexDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ElectronicRefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Approx_Value.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Bags.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Cones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyBags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyCones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SlNo.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DeliveryAt.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BookNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Bags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TexDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ElectronicRefNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cones.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Approx_Value.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyCones.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DeliveryAt.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then
        dtp_Time.Visible = True
        'End If

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub YarnDelivery_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub YarnDelivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf Pnl_DosPrint.Visible = True Then
                    btn_Close_DosPrint_Click(sender, e)
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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '--------
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Count_name from Count_Head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select Mill_name from Mill_Head order by Mill_name", con)
            da.Fill(dt3)
            cbo_Filter_MillName.DataSource = dt3
            cbo_Filter_MillName.DisplayMember = "Mill_name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate   '.Date.ToShortDateString
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate   '.Date.ToShortDateString
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
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Delivery_No from SizSoft_Yarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Delivery_No from SizSoft_Yarn_Delivery_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Delivery_No from SizSoft_Yarn_Delivery_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Delivery_No from SizSoft_Yarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Delivery_No desc", con)
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

            lbl_DcNo.Text = "NEW" ' Common_Procedures.get_MaxCode(con, "SizSoft_Yarn_Delivery_Head", "Yarn_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red

            dtp_Time.Text = Format(Now, "hh:mm tt").ToString

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Receipt No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Delivery_No from SizSoft_Yarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Receipt No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '    Dim Da As New SqlClient.SqlDataAdapter
        '    Dim Dt As New DataTable
        '    Dim movno As String, inpno As String
        '    Dim RecCode As String

        '    If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        '    Try

        '        inpno = InputBox("Enter New Receipt No.", "FOR NEW RECEIPT INSERTION...")

        '        RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

        '        Da = New SqlClient.SqlDataAdapter("select Yarn_Delivery_No from SizSoft_Yarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(RecCode) & "'", con)
        '        Da.Fill(Dt)

        '        movno = ""
        '        If Dt.Rows.Count > 0 Then
        '            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
        '                movno = Trim(Dt.Rows(0)(0).ToString)
        '            End If
        '        End If

        '        Dt.Clear()
        '        Dt.Dispose()
        '        Da.Dispose()

        '        If Val(movno) <> 0 Then
        '            move_record(movno)

        '        Else
        '            If Val(inpno) = 0 Then
        '                MessageBox.Show("Invalid Receipt No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '            Else
        '                new_record()
        '                Insert_Entry = True
        '                lbl_DcNo.Text = Trim(UCase(inpno))

        '            End If

        '        End If

        '    Catch ex As Exception
        '        MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '    End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '----
    End Sub

    Private Sub txt_Bags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Bags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_Cones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub


    Private Sub txt_EmptyBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_SlNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SlNo.GotFocus
        '---Show_Yarn_CurrentStock()
    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then

            If Val(txt_SlNo.Text) = 0 Then
                txt_EmptyBeam.Focus()

            Else

                With dgv_Details

                    For i = 0 To .Rows.Count - 1
                        If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                            cbo_CountName.Text = .Rows(i).Cells(1).Value
                            cbo_YarnType.Text = .Rows(i).Cells(2).Value
                            cbo_SetNo.Text = .Rows(i).Cells(3).Value
                            cbo_MillName.Text = .Rows(i).Cells(4).Value
                            txt_Bags.Text = Val(.Rows(i).Cells(5).Value)
                            txt_Cones.Text = Val(.Rows(i).Cells(6).Value)
                            txt_Weight.Text = Format(Val(.Rows(i).Cells(7).Value), "########0.000")

                            Exit For

                        End If

                    Next

                End With

                SendKeys.Send("{TAB}")

            End If

        End If
    End Sub

    Private Sub cbo_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_YarnType.GotFocus
        With cbo_YarnType

            If Trim(cbo_YarnType.Text) = "" Then cbo_YarnType.Text = "MILL"
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "(Yarn_type <> '')")

        End With
        cbo_YarnType.Tag = cbo_YarnType.Text
        '---Show_Yarn_CurrentStock()
    End Sub

    Private Sub cbo_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_YarnType.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_YarnType, cbo_CountName, Nothing, "YarnType_Head", "Yarn_type", "", "(Yarn_type <> '')")
            With cbo_YarnType
                If e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If Trim(UCase(cbo_YarnType.Text)) = "BABY" Then
                        cbo_SetNo.Focus()

                    Else
                        cbo_MillName.Focus()

                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_YarnType.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_YarnType, cbo_MillName, "YarnType_Head", "Yarn_Type", "", "(Yarn_type <> '')")

            If Asc(e.KeyChar) = 13 Then
                If Trim(UCase(cbo_YarnType.Text)) = "BABY" Then
                    cbo_SetNo.Focus()
                Else
                    cbo_MillName.Focus()
                End If
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim n As Integer
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Yarn_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Yarn_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Yarn_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Yarn_Delivery_Code IN (select z1.Yarn_Delivery_Code from SizSoft_Yarn_Delivery_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ") "
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Yarn_Delivery_Code IN (select z2.Yarn_Delivery_Code from SizSoft_Yarn_Delivery_Details z2 where z2.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from SizSoft_Yarn_Delivery_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Yarn_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Yarn_Delivery_Date").ToString), "dd-MM-yyyy")
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
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

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

    Private Sub cbo_BeamWidth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BeamWidth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub




    Private Sub cbo_BeamWidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BeamWidth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BeamWidth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
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




    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

            txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
            cbo_CountName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
            cbo_YarnType.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
            cbo_SetNo.Text = dgv_Details.CurrentRow.Cells(3).Value
            cbo_MillName.Text = dgv_Details.CurrentRow.Cells(4).Value
            txt_Bags.Text = Val(dgv_Details.CurrentRow.Cells(5).Value)
            txt_Cones.Text = Val(dgv_Details.CurrentRow.Cells(6).Value)
            txt_Weight.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.000")

            If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

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
                .Rows.RemoveAt(n)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            Total_Calculation()

            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            cbo_CountName.Text = ""
            cbo_YarnType.Text = "MILL"
            cbo_SetNo.Text = ""
            cbo_MillName.Text = ""
            txt_Bags.Text = ""
            txt_Cones.Text = ""
            txt_Weight.Text = ""

            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()

        End If

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        '---Show_Yarn_CurrentStock()
    End Sub

    Private Sub cbo_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Try
            With cbo_MillName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    If Trim(UCase(cbo_YarnType.Text)) = "BABY" Then
                        cbo_SetNo.Focus()

                    Else
                        cbo_YarnType.Focus()

                    End If

                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    txt_Bags.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
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

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        'save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Close_Form()
    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        If Trim(cbo_CountName.Text) = "" Then
            MessageBox.Show("Invalid Count Name", "DOES NOT ADD...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
            Exit Sub
        End If

        If Trim(cbo_YarnType.Text) = "" Then
            MessageBox.Show("Invalid Yarn Type", "DOES NOT ADD...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_YarnType.Enabled And cbo_YarnType.Visible Then cbo_YarnType.Focus()
            Exit Sub
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1163" Then
            If Trim(UCase(cbo_YarnType.Text)) = "BABY" And Trim(cbo_SetNo.Text) = "" Then
                MessageBox.Show("Invalid Set No", "DOES NOT ADD...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_SetNo.Enabled And cbo_SetNo.Visible Then cbo_SetNo.Focus()
                Exit Sub
            End If
        End If

        If Trim(cbo_MillName.Text) = "" Then
            MessageBox.Show("Invalid MIll Name", "DOES NOT ADD...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_MillName.Enabled And cbo_MillName.Visible Then cbo_MillName.Focus()
            Exit Sub
        End If

        If Val(txt_Weight.Text) = 0 Then
            MessageBox.Show("Invalid Weight", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Weight.Enabled And txt_Weight.Visible Then txt_Weight.Focus()
            Exit Sub
        End If

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = cbo_CountName.Text
                    .Rows(i).Cells(2).Value = cbo_YarnType.Text
                    .Rows(i).Cells(3).Value = cbo_SetNo.Text
                    .Rows(i).Cells(4).Value = cbo_MillName.Text
                    .Rows(i).Cells(5).Value = Val(txt_Bags.Text)
                    .Rows(i).Cells(6).Value = Val(txt_Cones.Text)
                    .Rows(i).Cells(7).Value = Format(Val(txt_Weight.Text), "########0.000")

                    .Rows(i).Selected = True

                    MtchSTS = True

                    If i >= 8 Then .FirstDisplayedScrollingRowIndex = i - 7

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_CountName.Text
                .Rows(n).Cells(2).Value = cbo_YarnType.Text
                .Rows(n).Cells(3).Value = cbo_SetNo.Text
                .Rows(n).Cells(4).Value = cbo_MillName.Text
                .Rows(n).Cells(5).Value = Val(txt_Bags.Text)
                .Rows(n).Cells(6).Value = Val(txt_Cones.Text)
                .Rows(n).Cells(7).Value = Format(Val(txt_Weight.Text), "########0.000")

                .Rows(n).Selected = True

                If n >= 8 Then .FirstDisplayedScrollingRowIndex = n - 7

            End If

        End With

        Total_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_CountName.Text = ""
        cbo_YarnType.Text = "MILL"
        cbo_SetNo.Text = ""
        cbo_MillName.Text = ""
        txt_Bags.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""

        If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()

    End Sub

    Private Sub btn_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows.RemoveAt(i)

                    MtchSTS = True

                    Exit For

                End If

            Next

            If MtchSTS = True Then
                For i = 0 To .Rows.Count - 1
                    .Rows(n).Cells(0).Value = i + 1
                Next
            End If

        End With

        Total_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_CountName.Text = ""
        cbo_YarnType.Text = "MILL"
        cbo_SetNo.Text = ""
        cbo_MillName.Text = ""
        txt_Bags.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""

        If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()

    End Sub



    Private Sub txt_Bags_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Bags.TextChanged
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CntID As Integer
        Dim MilID As Integer
        Dim Cns_Bg As Single, Wt_Cn As Single

        CntID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
        MilID = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName.Text)

        If CntID <> 0 And MilID <> 0 And Trim(UCase(cbo_YarnType.Text)) = "MILL" Then

            Cns_Bg = 0 : Wt_Cn = 0
            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                Cns_Bg = Val(Dt.Rows(0).Item("Cones_Bag").ToString)
                Wt_Cn = Val(Dt.Rows(0).Item("Weight_Cone").ToString)
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(Cns_Bg) <> 0 Then
                txt_Cones.Text = Val(txt_Bags.Text) * Val(Cns_Bg)
            End If
            If Val(Wt_Cn) <> 0 Then
                txt_Weight.Text = Format(Val(txt_Cones.Text) * Val(Wt_Cn), "#########0.000")
            End If

        End If
    End Sub

    Private Sub txt_Cones_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Cones.TextChanged
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CntID As Integer
        Dim MilID As Integer
        Dim Cns_Bg As Single, Wt_Cn As Single

        CntID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
        MilID = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName.Text)

        If CntID <> 0 And MilID <> 0 And Trim(UCase(cbo_YarnType.Text)) = "MILL" Then

            Cns_Bg = 0 : Wt_Cn = 0
            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                Cns_Bg = Val(Dt.Rows(0).Item("Cones_Bag").ToString)
                Wt_Cn = Val(Dt.Rows(0).Item("Weight_Cone").ToString)
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(Wt_Cn) <> 0 Then
                txt_Weight.Text = Format(Val(txt_Cones.Text) * Val(Wt_Cn), "#########0.000")
            End If

        End If
    End Sub

    Private Sub cbo_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_CountName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CountName.LostFocus
        'Show_Yarn_CurrentStock()
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_Delivered, cbo_VehicleNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_SetNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SetNo.GotFocus
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)

        Cmp_Cond = ""
        'If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
        '    Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        'End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from SizSoft_Yarn_Delivery_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Yarn_Delivery_Code = '" & Trim(NewCode) & "') ) ) )"
        'Condt = "( a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Yarn_Delivery_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Yarn_Delivery_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_BabyCone_Processing_Details a", "setcode_forSelection", Condt, "(Reference_Code = '')")

        cbo_SetNo.Tag = cbo_SetNo.Text

    End Sub

    Private Sub cbo_SetNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SetNo.LostFocus
        If Trim(UCase(cbo_SetNo.Text)) <> Trim(UCase(cbo_SetNo.Tag)) Then
            get_BabyCone_Details()
        End If
    End Sub

    Private Sub cbo_setno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SetNo.KeyDown
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)

        Cmp_Cond = ""
        'If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
        '    Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        'End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from SizSoft_Yarn_Delivery_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Yarn_Delivery_Code = '" & Trim(NewCode) & "') ) ) )"
        'Condt = "( a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Yarn_Delivery_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Yarn_Delivery_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SetNo, cbo_YarnType, cbo_MillName, "Stock_BabyCone_Processing_Details", "setcode_forSelection", Condt, "(Reference_Code = '')")

    End Sub

    Private Sub cbo_setno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SetNo.KeyPress
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)

        Cmp_Cond = ""
        'If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
        '    Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        'End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Yarn_Delivery_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Yarn_Delivery_Code = '" & Trim(NewCode) & "') ) ) )"
        'Condt = "( a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from Yarn_Delivery_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Yarn_Delivery_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SetNo, cbo_MillName, "Stock_BabyCone_Processing_Details a", "setcode_forSelection", Condt, "(Reference_Code = '')")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, txt_BookNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim TexStk_iD As Integer = 0
        Dim LedIdNo As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            'If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then

            'LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text, , TrnTo_DbName)
            'TexStk_iD = Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(LedIdNo)) & ")")
            '  If TexStk_iD <> 0 Then

            If MessageBox.Show("Do you want to select Requirement:", "FOR REQUIREMENT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                'btn_Selection_Click(sender, e)

            Else
                txt_BookNo.Focus()

            End If

        Else
            txt_BookNo.Focus()

        End If

        'Else

        ' txt_BookNo.Focus()

        'End If

        'End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CountName, txt_SlNo, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(cbo_CountName.Text) <> "" Then
                cbo_YarnType.Focus()
            Else
                txt_EmptyBeam.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    'Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim dt1 As New DataTable
    '    Dim trans_id As Integer = 0

    '    If Trim(cbo_VehicleNo.Text) = "" And Trim(cbo_Transport.Text) <> "" Then

    '        trans_id = Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)

    '        Try

    '            If trans_id <> 0 Then
    '                da1 = New SqlClient.SqlDataAdapter("select top 1 * from SizSoft_Yarn_Delivery_Head where Transport_IdNo = " & Str(Val(trans_id)) & " Order by Yarn_Delivery_Date desc, for_Orderby desc, Yarn_Delivery_No desc", con)
    '                dt1 = New DataTable
    '                da1.Fill(dt1)

    '                If dt1.Rows.Count > 0 Then
    '                    cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
    '                End If

    '                dt1.Clear()
    '                dt1.Dispose()
    '                da1.Dispose()
    '            End If

    '        Catch ex As Exception
    '            MessageBox.Show(ex.Message, "INVALID VEHICLE DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End Try

    '    End If
    'End Sub

    Private Sub cbo_VehicleNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, Nothing, "", "", "", "")

        If (e.KeyValue = 40 And cbo_VehicleNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1267" Then
                cbo_VendorName.Focus()
            Else
                cbo_DeliveryTo.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, Nothing, "", "", "", "", False)
        If Asc(e.KeyChar) = 13 Then
            If cbo_VendorName.Visible = True Then
                cbo_VendorName.Focus()
            ElseIf txt_DeliveryAt.Visible = True Then
                txt_DeliveryAt.Focus()
            Else
                cbo_DeliveryTo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_CountName.Text) <> "" Then
                cbo_YarnType.Focus()
            Else
                txt_EmptyBeam.Focus()
            End If
        End If
    End Sub
    Private Sub cbo_Beamwidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BeamWidth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BeamWidth, txt_EmptyBeam, txt_EmptyBags, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub

    Private Sub cbo_BeamWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BeamWidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BeamWidth, txt_EmptyBags, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub


    Private Sub cbo_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName, txt_Bags, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub txt_EmptyCones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyCones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyBags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Weight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weight.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
            'SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_EmptyBeam_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_EmptyBeam.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then cbo_CountName.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If (e.KeyValue = 38) Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1267" Then
                cbo_VendorName.Focus()
            Else
                cbo_DeliveryTo.Focus()
            End If

        End If
        If (e.KeyValue = 40) Then
            btn_save.Focus()
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    'Private Sub Show_Yarn_CurrentStock()
    '    Dim vCntID As Integer
    '    Dim vLedID As Integer
    '    Dim CurStk As Decimal
    '    Dim Vdate As Date


    '    If Trim(cbo_Ledger.Text) <> "" And Trim(cbo_CountName.Text) <> "" Then
    '        vLedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
    '        vCntID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
    '        Vdate = dtp_Date.Value
    '        ' CurStk = 0
    '        If Val(cbo_Ledger.Tag) <> Val(vLedID) Or Val(lbl_AvailableStock.Tag) <> Val(vCntID) Then
    '            lbl_AvailableStock.Tag = 0
    '            lbl_AvailableStock.Text = ""
    '            CurStk = 0
    '            If Val(vLedID) <> 0 And Val(vCntID) <> 0 Then
    '                CurStk = Common_Procedures.get_Yarn_CurrentStock(con, Val(lbl_Company.Tag), vLedID, vCntID)
    '                cbo_Ledger.Tag = Val(vLedID)
    '                lbl_AvailableStock.Tag = Val(vCntID)
    '                lbl_AvailableStock.Text = Format(Val(CurStk), "#########0.000")
    '            End If
    '        End If

    '    Else
    '        cbo_Ledger.Tag = 0
    '        lbl_AvailableStock.Tag = 0
    '        lbl_AvailableStock.Text = ""

    '    End If
    'End Sub

    Private Sub get_BabyCone_Details()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CntID As Integer
        Dim NewCode As String
        Dim Ent_Bgs As Integer, Ent_Cns As Integer
        Dim Ent_Wgt As Single


        CntID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)

        If CntID <> 0 And Trim(cbo_SetNo.Text) <> "" And Trim(UCase(cbo_YarnType.Text)) = "BABY" Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select a.*, b.mill_name, c.bags as DelvEnt_Bags, c.cones as DelvEnt_cones, c.Weight as DelvEnt_Weight from Stock_BabyCone_Processing_Details a INNER JOIN mill_head b ON  a.mill_idno = b.mill_idno LEFT OUTER JOIN SizSoft_Yarn_Delivery_Details c ON c.Yarn_Delivery_Code = '" & Trim(NewCode) & "' and c.yarn_type = 'BABY' and a.SetCode_ForSelection = c.SetCode_ForSelection where a.setcode_forSelection = '" & Trim(cbo_SetNo.Text) & "' and a.count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                Ent_Bgs = 0 : Ent_Cns = 0 : Ent_Wgt = 0

                If IsDBNull(Dt.Rows(0).Item("DelvEnt_Bags").ToString) = False Then Ent_Bgs = Val(Dt.Rows(0).Item("DelvEnt_Bags").ToString)
                If IsDBNull(Dt.Rows(0).Item("DelvEnt_cones").ToString) = False Then Ent_Cns = Val(Dt.Rows(0).Item("DelvEnt_cones").ToString)
                If IsDBNull(Dt.Rows(0).Item("DelvEnt_Weight").ToString) = False Then Ent_Wgt = Val(Dt.Rows(0).Item("DelvEnt_Weight").ToString)

                cbo_MillName.Text = Dt.Rows(0).Item("Mill_Name").ToString
                txt_Bags.Text = (Val(Dt.Rows(0).Item("Baby_Bags").ToString) - Val(Dt.Rows(0).Item("Delivered_Bags").ToString) + Ent_Bgs)
                txt_Cones.Text = (Val(Dt.Rows(0).Item("Baby_Cones").ToString) - Val(Dt.Rows(0).Item("Delivered_Cones").ToString) + Ent_Cns)
                txt_Weight.Text = (Val(Dt.Rows(0).Item("Baby_Weight").ToString) - Val(Dt.Rows(0).Item("Delivered_Weight").ToString) + Ent_Wgt)

            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

        End If

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(5).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(6).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(7).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(TotBags)
            .Rows(0).Cells(6).Value = Val(TotCones)
            .Rows(0).Cells(7).Value = Format(Val(TotWeight), "########0.000")
        End With

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Invoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Invoice.Click
        prn_Status = 1
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint.Click
        prn_Status = 2
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub cbo_bagType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_bagType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_bagType, txt_TexDcNo, cbo_coneType, "Bag_Type_Head", "Bag_Type_Name", "", "(Bag_Type_Idno = 0)")

    End Sub

    Private Sub cbo_bagType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_bagType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_bagType, cbo_coneType, "Bag_Type_Head", "Bag_Type_Name", "", "(Bag_Type_Idno = 0)")

    End Sub

    Private Sub cbo_coneType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_coneType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_coneType, cbo_bagType, cbo_godown, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")

    End Sub

    Private Sub cbo_coneType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_coneType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_coneType, cbo_godown, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")

    End Sub



    Private Sub btn_Close_DosPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_DosPrint.Click
        pnl_Back.Enabled = True
        Pnl_DosPrint.Visible = False
    End Sub

    Private Sub Btn_DosCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_DosCancel.Click
        btn_Close_DosPrint_Click(sender, e)
    End Sub

    Private Sub Btn_LaserPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_LaserPrint.Click
        prn_Status = 2
        btn_Close_DosPrint_Click(sender, e)
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub



    Private Sub Cbo_DelTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Delivery_Party_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_VehicleNo, txt_Remarks, "Delivery_Party_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, txt_Remarks, "Delivery_Party_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub txt_DateAndTimeOFSupply_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DateAndTimeOFSupply.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then txt_ElectronicRefNo.Focus()
    End Sub

    Private Sub txt_DateAndTimeOFSupply_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DateAndTimeOFSupply.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_CountName.Focus()
        End If
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

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            'If Led_IdNo  = 0 Then Exit Sub

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            smstxt = " YARN DELIVERY" & vbCrLf

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                smstxt = smstxt & vbCrLf
            End If

            smstxt = smstxt & "DC.NO-" & Trim(lbl_DcNo.Text) & vbCrLf & "DATE-" & Trim(dtp_Date.Text)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Yarn_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Yarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                        smstxt = smstxt & vbCrLf
                    End If
                    '  smstxt = smstxt & vbCrLf
                    smstxt = smstxt & vbCrLf & "Count : " & Trim(dt2.Rows(i).Item("Count_Name").ToString)
                    smstxt = smstxt & vbCrLf & "Mill : " & Trim(dt2.Rows(i).Item("Mill_Name").ToString)

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                        smstxt = smstxt & vbCrLf & "Type : " & Trim(dt2.Rows(i).Item("Yarn_Type").ToString)
                    End If

                    If Val(dt2.Rows(i).Item("Bags").ToString) <> 0 Then
                        smstxt = smstxt & vbCrLf & "Bags : " & Trim(Val(dt2.Rows(i).Item("Bags").ToString))
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                        If Val(dt2.Rows(i).Item("Cones").ToString) <> 0 Then
                            smstxt = smstxt & vbCrLf & "Cones : " & Trim(dt2.Rows(i).Item("Cones").ToString)
                        End If
                    End If


                    smstxt = smstxt & vbCrLf & "Weight : " & Trim(Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000"))

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                        If Cmp_Typ = "UNACCOUNT" Then
                            smstxt = ""
                            smstxt = "RECEIPT " & vbCrLf
                            smstxt = smstxt & vbCrLf & "Bags : " & Trim(Val(dt2.Rows(i).Item("Bags").ToString))
                        End If
                    End If

                Next i

            End If
            dt2.Clear()

            smstxt = smstxt & vbCrLf & " Thanks! " & vbCrLf
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
                smstxt = smstxt & "GKT SIZING "
            Else '
                smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))
            End If

            'smstxt = smstxt & " " & vbCrLf & vbCrLf
            'smstxt = smstxt & " Thanks! " & vbCrLf
            'smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

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

    Private Sub cbo_VendorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VendorName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VendorName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VendorName, cbo_VehicleNo, txt_Remarks, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VendorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VendorName, txt_Remarks, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VendorName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Vendor_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_VendorName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    'Private Sub btn_UserModification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_UserModification.Click
    '    If Val(Common_Procedures.User.IdNo) = 1 Then
    '        Dim f1 As New User_Modifications
    '        f1.Entry_Name = Me.Name
    '        f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
    '        f1.ShowDialog()
    '    End If
    'End Sub



    Private Sub cbo_Delivered_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Delivered.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delivered, txt_Approx_Value, cbo_Transport, "", "", "", "")
    End Sub

    Private Sub cbo_Delivered_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Delivered.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delivered, cbo_Transport, "", "", "", "")
    End Sub


    Private Sub Update_PrintOut_Status(Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing)
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim vPrnSTS As Integer = 0


        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            If IsNothing(sqltr) = False Then
                cmd.Transaction = sqltr
            End If

            vPrnSTS = 0
            If chk_Printed.Checked = True Then
                vPrnSTS = 1
            End If

            cmd.CommandText = "Update SizSoft_Yarn_Delivery_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(NewCode) & "'"
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

    Private Sub cbo_godown_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_godown.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_godown_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_godown.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_godown, cbo_coneType, txt_ElectronicRefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_godown_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_godown.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_godown, txt_ElectronicRefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_godown_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_godown.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_bagType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    'Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim i As Integer, j As Integer, n As Integer, SNo As Integer
    '    Dim LedIdNo As Integer, CmpIdNo As String
    '    Dim NewCode As String
    '    Dim CompIDCondt As String
    '    Dim Ent_Bag As Single = 0
    '    Dim Ent_Wgt As Single = 0
    '    Dim Ent_Cone As Single = 0
    '    Dim Ent_Exc As Single = 0
    '    Dim TexStk_iD As Integer = 0

    '    Dim nr As Single = 0

    '    Dim vDbName As String = ""

    '    If Trim(TrnTo_DbName) <> "" Then
    '        vDbName = Trim(TrnTo_DbName) & ".."
    '    End If

    '    LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text, , TrnTo_DbName)
    '    If LedIdNo = 0 Then
    '        MessageBox.Show("Invalid Party Name", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
    '        Exit Sub
    '    End If

    '    TexStk_iD = Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(LedIdNo)) & ")")
    '    If TexStk_iD = 0 Then Exit Sub


    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


    '    CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
    '    If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
    '        CompIDCondt = ""
    '    End If


    '    With dgv_Selection

    '        .Rows.Clear()
    '        SNo = 0

    '        Da = New SqlClient.SqlDataAdapter("select  a.*, h.Bags As Ent_Bag,  h.Weight As Ent_Wgt,h.Cones As Ent_COne   from " & Trim(vDbName) & "Weaver_Yarn_Requirement_Details a   LEFT OUTER JOIN Yarn_Delivery_Details h ON h.Yarn_Delivery_Code = '" & Trim(NewCode) & "' and a.Weaver_Yarn_Requirement_Code = h.Weaver_Yarn_Requirement_Code and a.Weaver_Yarn_Requirement_Details_SlNo = h.Weaver_Yarn_Requirement_Details_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.DeliveryTo_IdNo = " & Str(Val(LedIdNo)) & " and ((a.Weight - a.Delivery_Weight ) > 0 or h.Weight > 0 ) order by a.Weaver_Yarn_Requirement_Date, a.for_orderby, a.Weaver_Yarn_Requirement_No", con)
    '        Dt1 = New DataTable
    '        nr = Da.Fill(Dt1)

    '        If Dt1.Rows.Count > 0 Then

    '            For i = 0 To Dt1.Rows.Count - 1

    '                n = .Rows.Add()


    '                Ent_Bag = 0
    '                Ent_Wgt = 0
    '                Ent_Cone = 0

    '                If IsDBNull(Dt1.Rows(i).Item("Ent_Bag").ToString) = False Then
    '                    Ent_Bag = Val(Dt1.Rows(i).Item("Ent_Bag").ToString)
    '                End If
    '                If IsDBNull(Dt1.Rows(i).Item("Ent_Cone").ToString) = False Then
    '                    Ent_Cone = Val(Dt1.Rows(i).Item("Ent_Cone").ToString)
    '                End If

    '                If IsDBNull(Dt1.Rows(i).Item("Ent_Wgt").ToString) = False Then
    '                    Ent_Wgt = Val(Dt1.Rows(i).Item("Ent_Wgt").ToString)
    '                End If



    '                SNo = SNo + 1
    '                .Rows(n).Cells(0).Value = Val(SNo)
    '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Weaver_Yarn_Requirement_No").ToString
    '                .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_Yarn_Requirement_Date").ToString), "dd-MM-yyyy")
    '                .Rows(n).Cells(3).Value = Common_Procedures.Count_IdNoToName(con, Val(Dt1.Rows(i).Item("Count_IdNo").ToString), , TrnTo_DbName)
    '                .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Yarn_Type").ToString
    '                .Rows(n).Cells(5).Value = Common_Procedures.Mill_IdNoToName(con, Val(Dt1.Rows(i).Item("Mill_IdNo").ToString), , TrnTo_DbName)
    '                .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Bags").ToString) - Val(Dt1.Rows(i).Item("Delivery_Bag").ToString) + Val(Ent_Bag), "#########0.00")
    '                .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Cones").ToString) - Val(Dt1.Rows(i).Item("Delivery_Cone").ToString) + Val(Ent_Cone), "#########0.00")
    '                .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString) - Val(Dt1.Rows(i).Item("Delivery_Weight").ToString) + Val(Ent_Wgt), "#########0.000")

    '                If Ent_Wgt > 0 Then
    '                    .Rows(n).Cells(9).Value = "1"
    '                    For j = 0 To .ColumnCount - 1
    '                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
    '                    Next

    '                Else
    '                    .Rows(n).Cells(9).Value = ""

    '                End If

    '                .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Weaver_Yarn_Requirement_Code").ToString
    '                .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Weaver_Yarn_Requirement_Details_SlNo").ToString

    '                .Rows(n).Cells(12).Value = Ent_Bag
    '                .Rows(n).Cells(13).Value = Ent_Cone
    '                .Rows(n).Cells(14).Value = Ent_Wgt
    '                ' .Rows(n).Cells(16).Value = Ent_Exc

    '            Next

    '        End If
    '        Dt1.Clear()

    '    End With

    '    pnl_Selection.Visible = True
    '    pnl_Back.Enabled = False
    '    '  pnl_Back.Visible = False
    '    dgv_Selection.Focus()

    'End Sub

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
        YarnDelivery_Selection()
    End Sub

    Private Sub YarnDelivery_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(9).Value) = 1 Then


                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(4).Value
                ' dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value

                ' cbo_TransportName.Text = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(11).Value
                If Val(dgv_Selection.Rows(i).Cells(12).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(12).Value
                Else
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(13).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(13).Value
                Else
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(14).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(14).Value
                Else
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If


            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        '  pnl_Back.Visible = True
        pnl_Selection.Visible = False
        If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()

    End Sub


End Class