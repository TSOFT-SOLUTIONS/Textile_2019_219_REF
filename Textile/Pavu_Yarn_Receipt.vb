Imports System.IO

Public Class Pavu_Yarn_Receipt
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PYREC-"
    Private Prec_ActCtrl As New Control
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private Prnt_HalfSheet_STS As Boolean = False
    Private vPrnt_2Copy_In_SinglePage As Integer = 0
    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuNPcs As Integer
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo1 As Integer
    Private prn_DetIndx1 As Integer
    Private prn_NoofBmDets1 As Integer
    Private prn_DetSNo1 As Integer
    Private prn_NoofBmDets As Integer
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        chk_Verified_Status.Checked = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black

        vmskOldText = ""
        vmskSelStrt = -1

        msk_date.Text = ""
        dtp_Date.Text = ""
        txt_KuraiPavuBeam.Text = ""
        txt_KuraiPavuMeters.Text = ""
        txt_PartyDcNo.Text = ""
        cbo_RecForm.Text = ""
        cbo_RecForm.Tag = ""
        cbo_EndsCount.Text = ""
        txt_NoOfBobin.Text = ""

        cbo_EndsCount.Text = ""
        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        cbo_DelvAt.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
        cbo_DelvAt.Tag = ""
        cbo_Cloth.Text = ""

        txt_Freight.Text = ""
        txt_Note.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_PavuDetails.Rows.Clear()

        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        dgv_YarnDetails.Rows.Clear()
        dgv_YarnDetails.Rows(0).Cells(2).Value = "MILL"

        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_DeliveryName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_MillName.Text = ""

            cbo_Filter_DeliveryName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False

        cbo_Grid_CountName.Tag = -1
        cbo_Grid_MillName.Tag = -1
        cbo_Grid_YarnType.Tag = -1

        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""
        cbo_Grid_YarnType.Text = ""

        cbo_ClothSales_OrderCode_forSelection.Text = ""

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
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


        If Me.ActiveControl.Name <> dgv_YarnDetails_Total.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_YarnDetails.Name Then
            Common_Procedures.Hide_CurrentStock_Display()
        End If
        Grid_DeSelect()
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as DelvName, c.Ledger_Name as TransportName, d.EndsCount_Name,e.Ledger_Name as RecFromName , Ch.Cloth_Name from PavuYarn_Receipt_Head a INNER JOIN Ledger_Head b ON a.DeliveryTo_Idno = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.ReceivedFrom_Idno = e.Ledger_IdNO LEFT JOIN Cloth_Head Ch ON a.Cloth_IdNo = Ch.Cloth_IdNo Where a.PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RecNo.Text = dt1.Rows(0).Item("PavuYarn_Receipt_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("PavuYarn_Receipt_Date").ToString
                msk_date.Text = dtp_Date.Text

                cbo_DelvAt.Text = dt1.Rows(0).Item("DelvName").ToString
                txt_KuraiPavuBeam.Text = dt1.Rows(0).Item("Empty_Beam").ToString
                txt_KuraiPavuMeters.Text = Val(dt1.Rows(0).Item("Meters").ToString)
                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                txt_PartyDcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_Transport.Text = dt1.Rows(0).Item("TransportName").ToString
                cbo_RecForm.Text = dt1.Rows(0).Item("RecFromName").ToString
                txt_Freight.Text = Val(dt1.Rows(0).Item("Freight_Charge").ToString)
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_NoOfBobin.Text = dt1.Rows(0).Item("Empty_Bobin").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                cbo_Cloth.Text = dt1.Rows(0).Item("Cloth_Name").ToString

                cbo_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True


                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Pavu_Delivery_Increment, c.EndsCount_Name, d.Beam_Width_Name from Pavu_Receipt_Details a INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_Idno = d.Beam_Width_Idno where a.PavuYarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                'da2 = New SqlClient.SqlDataAdapter("Select a.*, b.EndsCount_Name, c.Beam_Width_Name from Pavu_Receipt_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head c ON a.Beam_Width_Idno = c.Beam_Width_Idno where a.PavuYarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_PavuDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_PavuDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_PavuDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_PavuDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Set_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Beam_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Pcs").ToString
                        dgv_PavuDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        dgv_PavuDetails.Rows(n).Cells(5).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                        dgv_PavuDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Beam_Width_Name").ToString
                        dgv_PavuDetails.Rows(n).Cells(7).Value = ""
                        dgv_PavuDetails.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Noof_Used").ToString
                        dgv_PavuDetails.Rows(n).Cells(9).Value = dt2.Rows(i).Item("set_code").ToString
                        dgv_PavuDetails.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Pavu_Delivery_Increment").ToString

                        If Val(dgv_PavuDetails.Rows(n).Cells(8).Value) > 0 And Val(dgv_PavuDetails.Rows(n).Cells(8).Value) <> Val(dgv_PavuDetails.Rows(n).Cells(10).Value) Then
                            dgv_PavuDetails.Rows(n).Cells(7).Value = "1"
                        End If

                    Next i

                End If

                With dgv_PavuDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Beam").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                End With

                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Yarn_Receipt_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.PavuYarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_YarnDetails.Rows.Clear()
                SNo = 0

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
                        dgv_YarnDetails.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Thiri").ToString), "########0.00")

                    Next i

                End If

                With dgv_YarnDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Thiri").ToString), "########0.00")
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub Pavu_Yarn_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DelvAt.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DelvAt.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VehicleNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_VehicleNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RecForm.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_RecForm.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Pavu_Yarn_Receipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_DelvAt.DataSource = dt1
        cbo_DelvAt.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Distinct(Vehicle_No) from PavuYarn_Receipt_Head order by Vehicle_No", con)
        da.Fill(dt7)
        cbo_VehicleNo.DataSource = dt7
        cbo_VehicleNo.DisplayMember = "Vehicle_No"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'TRANSPORT' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Transport.DataSource = dt2
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
        da.Fill(dt3)
        cbo_EndsCount.DataSource = dt3
        cbo_EndsCount.DisplayMember = "EndsCount_Name"

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

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
        da.Fill(dt8)
        cbo_RecForm.DataSource = dt8
        cbo_RecForm.DisplayMember = "Ledger_DisplayName"

        lbl_Bobin.Visible = False
        txt_NoOfBobin.Visible = False
        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

            dgv_PavuDetails.Columns(4).HeaderText = "MTR Or WGT"
            lbl_Bobin.Visible = True
            txt_NoOfBobin.Visible = True
        End If
        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

            dgv_Selection.Columns(4).HeaderText = "MTR Or WGT"
        End If
        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If
        lbl_Cloth.Visible = False
        cbo_Cloth.Visible = False
        dgv_YarnDetails.Columns(7).Visible = False
        dgv_YarnDetails_Total.Columns(7).Visible = False
        If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status) = 1 Or Val(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status) = 1 Then
            lbl_Cloth.Visible = True
            cbo_Cloth.Visible = True
            dgv_YarnDetails.Columns(7).Visible = True
            dgv_YarnDetails_Total.Columns(7).Visible = True

        Else

            dgv_YarnDetails.Columns(1).Width = dgv_YarnDetails.Columns(1).Width + 20
            dgv_YarnDetails.Columns(3).Width = dgv_YarnDetails.Columns(3).Width + 70
            dgv_YarnDetails.Columns(6).Width = dgv_YarnDetails.Columns(6).Width + 20

            dgv_YarnDetails_Total.Columns(1).Width = dgv_YarnDetails_Total.Columns(1).Width + 20
            dgv_YarnDetails_Total.Columns(3).Width = dgv_YarnDetails_Total.Columns(3).Width + 70
            dgv_YarnDetails_Total.Columns(6).Width = dgv_YarnDetails_Total.Columns(6).Width + 20

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


        cbo_Cloth.Visible = False
        lbl_Cloth.Visible = False
        If Val(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status) = 1 Then
            dgv_YarnDetails.Columns(7).HeaderText = "METERS"
            lbl_Cloth.Visible = True
            cbo_Cloth.Visible = True
        End If

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False

        dtp_Date.Text = ""
        msk_date.Text = ""
        txt_KuraiPavuBeam.Text = ""
        cbo_DelvAt.Text = ""
        cbo_DelvAt.Tag = ""
        cbo_EndsCount.Text = ""

        cbo_EndsCount.Text = ""
        cbo_VehicleNo.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        cbo_Verified_Sts.Items.Clear()
        cbo_Verified_Sts.Items.Add("")
        cbo_Verified_Sts.Items.Add("YES")
        cbo_Verified_Sts.Items.Add("NO")

        chk_Verified_Status.Visible = False
        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then
                chk_Verified_Status.Visible = True
                lbl_verfied_sts.Visible = True
                cbo_Verified_Sts.Visible = True


            End If
        Else
            chk_Verified_Status.Visible = False
            lbl_verfied_sts.Visible = False
            cbo_Verified_Sts.Visible = False
        End If




        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DelvAt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RecForm.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_KuraiPavuBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_KuraiPavuMeters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DelvAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RecForm.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfBobin.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_KuraiPavuBeam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_KuraiPavuMeters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfBobin.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_KuraiPavuBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_KuraiPavuMeters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoOfBobin.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Pavu_Yarn_Receipt_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Pavu_Yarn_Receipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then



                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_YarnDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_YarnDetails.IsCurrentRowDirty = True Then

                dgv1 = dgv_YarnDetails

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_YarnDetails

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Note.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 6 Then
                            If .CurrentRow.Cells(7).Visible = True Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(7)
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
                                txt_Freight.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 1 Then
                            If .Visible = False Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(7)
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

    End Sub



    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult
        Dim Nr As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PavuYarn_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PavuYarn_Receipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.PavuYarn_Receipt_Entry, New_Entry, Me, con, "PavuYarn_Receipt_Head", "PavuYarn_Receipt_Code", NewCode, "PavuYarn_Receipt_Date", "(PavuYarn_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "PavuYarn_Receipt_Head", "Verified_Status", "(PavuYarn_Receipt_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  ( Pavu_Delivery_Code <> '' or Pavu_Delivery_Increment <> 0)", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some Pavu Delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "PavuYarn_Receipt_head", "PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "PavuYarn_Receipt_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Pavu_Receipt_Details", "PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Set_No,Beam_No,Pcs,Meters,EndsCount_IdNo,Beam_Width_IdNo,Noof_Used,Set_Code", "Sl_No", "PavuYarn_Receipt_Code, For_OrderBy, Company_IdNo, PavuYarn_Receipt_No, PavuYarn_Receipt_Date, Ledger_Idno", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Yarn_Receipt_Details", "PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " count_idno, Yarn_Type, Mill_IdNo,  Bags, Cones, Weight , Thiri,Rate_For,Rate,Amount", "Sl_No", "PavuYarn_Receipt_Code, For_OrderBy, Company_IdNo, PavuYarn_Receipt_No, PavuYarn_Receipt_Date, Ledger_Idno", trans)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()


            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                      " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Da = New SqlClient.SqlDataAdapter("Select * from Pavu_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    Nr = 0
                    cmd.CommandText = "update Stock_SizedPavu_Processing_Details set " _
                              & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("ReceivedFrom_IdNo").ToString)) & ", " _
                              & " Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1 " _
                              & " Where " _
                              & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("DeliveryTo_IdNo").ToString)) & " and " _
                              & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                              & " beam_no = '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "' and " _
                              & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString))
                    Nr = cmd.ExecuteNonQuery

                    If Nr = 0 Then
                        Throw New ApplicationException("Some Pavu Delivered to Others")
                        Exit Sub
                    End If

                Next
            End If
            Dt1.Clear()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Pavu_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Yarn_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then
                If Common_Procedures.Check_is_Negative_Stock_Status(con, trans) = True Then Exit Sub
            End If

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_DeliveryName.DataSource = dt1
            cbo_Filter_DeliveryName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select count_name from count_head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
            da.Fill(dt3)
            cbo_Filter_EndsCount.DataSource = dt3
            cbo_Filter_EndsCount.DisplayMember = "EndsCount_Name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_DeliveryName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_EndsCount.Text = ""
            cbo_Filter_MillName.Text = ""
            cbo_Filter_DeliveryName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_EndsCount.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False
        If Filter_Status = True Then
            If dgv_Filter_Details.Rows.Count > 0 And Filter_RowNo >= 0 Then
                dgv_Filter_Details.Focus()
                dgv_Filter_Details.CurrentCell = dgv_Filter_Details.Rows(Filter_RowNo).Cells(0)
                dgv_Filter_Details.CurrentCell.Selected = True
            Else
                If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

            End If

        Else
            If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

        End If
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 PavuYarn_Receipt_No from PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, PavuYarn_Receipt_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 PavuYarn_Receipt_No from PavuYarn_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, PavuYarn_Receipt_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 PavuYarn_Receipt_No from PavuYarn_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, PavuYarn_Receipt_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 PavuYarn_Receipt_No from PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, PavuYarn_Receipt_No desc", con)
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

            lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "PavuYarn_Receipt_Head", "PavuYarn_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RecNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, PavuYarn_Receipt_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("PavuYarn_Receipt_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("PavuYarn_Receipt_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus() : msk_date.SelectionStart = 0

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

            inpno = InputBox("Enter Rec.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select PavuYarn_Receipt_No from PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code = '" & Trim(RecCode) & "'", con)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PavuYarn_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PavuYarn_Receipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.PavuYarn_Receipt_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Rec No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select PavuYarn_Receipt_No from PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Del_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim KuPvu_EdsCnt_ID As Integer = 0
        Dim SzPvu_EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotPvuBms As Single, vTotPvuMtrs As Single, vThiriMeters As Single
        Dim YCnt_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single
        Dim EntID As String = ""
        Dim Bw_IdNo As Integer = 0
        Dim Pavu_DelvInc As Integer = 0
        Dim Ent_NoofUsed As Integer = 0
        Dim Stock_In As String
        Dim mtrspcs As Integer
        Dim dt2 As New DataTable
        Dim vTotPvuStk As Single = 0
        Dim Prtcls_DelvIdNo As Integer = 0, Prtcls_RecIdNo As Integer
        Dim ThiriMeters As Single = 0
        Dim Stk_DelvIdNo As Integer, Stk_RecIdNo As Integer
        Dim Stock_Weight As Single = 0
        Dim Delv_Ledtype As String = ""
        Dim Rec_Ledtype As String = ""
        Dim YSno As Integer = 0
        Dim Cloth_ID As Integer = 0
        Dim vENTDB_DelvToIDno As String = 0
        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.PavuYarn_Receipt_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.PavuYarn_Receipt_Entry, New_Entry, Me, con, "PavuYarn_Receipt_Head", "PavuYarn_Receipt_Code", NewCode, "PavuYarn_Receipt_Date", "(PavuYarn_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, PavuYarn_Receipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "PavuYarn_Receipt_Head", "Verified_Status", "(PavuYarn_Receipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        If pnl_Back.Enabled = False Then
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

        Rec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)
        If Rec_ID = 0 Then
            MessageBox.Show("Invalid Received Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus()
            Exit Sub
        End If

        Del_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        If Del_ID = 0 Then Del_ID = 4

        If Val(Del_ID) = Val(Rec_ID) Then
            MessageBox.Show("Same Delivery And Received Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_DelvAt.Enabled And cbo_DelvAt.Visible Then cbo_DelvAt.Focus()
            Exit Sub
        End If

        KuPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        If KuPvu_EdsCnt_ID = 0 And Val(txt_KuraiPavuMeters.Text) <> 0 Then
            MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
            Exit Sub
        End If

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo

        If Trim(txt_PartyDcNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Da = New SqlClient.SqlDataAdapter("select * from PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and DeliveryTo_IdNo = " & Str(Val(Del_ID)) & " and Party_dcno = '" & Trim(txt_PartyDcNo.Text) & "' and PavuYarn_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and PavuYarn_Receipt_Code <> '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Party Dc No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_PartyDcNo.Enabled And txt_PartyDcNo.Visible Then txt_PartyDcNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If
        With dgv_PavuDetails

            For i = 0 To .RowCount - 1

                If Val(dgv_PavuDetails.Rows(i).Cells(4).Value) <> 0 Then

                    If Trim(dgv_PavuDetails.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_PavuDetails.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(2)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_PavuDetails.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(5)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                End If

            Next
        End With

        vTotPvuBms = 0 : vTotPvuMtrs = 0 : vThiriMeters = 0
        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTotPvuBms = Val(dgv_PavuDetails_Total.Rows(0).Cells(2).Value())
            vTotPvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(4).Value())
            If dgv_PavuDetails_Total.Columns(7).Visible = True Then
                vThiriMeters = Val(dgv_YarnDetails_Total.Rows(0).Cells(7).Value())
            End If
        End If

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1


        For i = 0 To dgv_YarnDetails.RowCount - 1

            If Val(dgv_YarnDetails.Rows(i).Cells(6).Value) <> 0 Then

                YCnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(1).Value)
                If Val(YCnt_ID) = 0 Then
                    MessageBox.Show("Invalid CountName", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If


                If Trim(dgv_YarnDetails.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(2)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                YMil_ID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(3).Value)
                If Val(YMil_ID) = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(3)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

            End If

        Next

        vTotYrnBags = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0
        If dgv_YarnDetails_Total.RowCount > 0 Then
            vTotYrnBags = Val(dgv_YarnDetails_Total.Rows(0).Cells(4).Value())
            vTotYrnCones = Val(dgv_YarnDetails_Total.Rows(0).Cells(5).Value())
            vTotYrnWeight = Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value())
        End If

        Cloth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "PavuYarn_Receipt_Head", "PavuYarn_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))


            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then
                cmd.CommandText = "Insert into PavuYarn_Receipt_Head (  PavuYarn_Receipt_Code,                 Company_IdNo     ,           PavuYarn_Receipt_No ,                               for_OrderBy                              , PavuYarn_Receipt_Date,       DeliveryTo_Idno   ,                 Empty_Beam         ,                 Meters               ,               Party_DcNo           ,            EndsCount_IdNo         ,               Vehicle_No           ,    Transport_Idno     ,   ReceivedFrom_Idno  ,            Freight_Charge     ,               Note            ,              Total_Beam      ,              Total_Meters    ,              Total_Bags      ,              Total_Cones      ,               Total_Weight      ,                 Empty_Bobin         ,             user_idNo         ,           Total_Thiri        ,        Cloth_IdNo  ,Verified_Status , ClothSales_OrderCode_forSelection) " &
                                  "Values                            ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",      @EntryDate      , " & Str(Val(Del_ID)) & ", " & Val(txt_KuraiPavuBeam.Text) & ", " & Val(txt_KuraiPavuMeters.Text) & ", '" & Trim(txt_PartyDcNo.Text) & "' , " & Str(Val(KuPvu_EdsCnt_ID)) & " , '" & Trim(cbo_VehicleNo.Text) & "' , " & Val(Trans_ID) & " , " & Val(Rec_ID) & "  , " & Val(txt_Freight.Text) & " , '" & Trim(txt_Note.Text) & "' , " & Str(Val(vTotPvuBms)) & " , " & Str(Val(vTotPvuMtrs)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & " , " & Str(Val(txt_NoOfBobin.Text)) & ", " & Val(lbl_UserName.Text) & "," & Str(Val(vThiriMeters)) & "," & Val(Cloth_ID) & "," & Val(Verified_STS) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' )"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "PavuYarn_Receipt_head", "PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "PavuYarn_Receipt_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Pavu_Receipt_Details", "PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No,Beam_No,Pcs,Meters,EndsCount_IdNo,Beam_Width_IdNo,Noof_Used,Set_Code", "Sl_No", "PavuYarn_Receipt_Code, For_OrderBy, Company_IdNo, PavuYarn_Receipt_No, PavuYarn_Receipt_Date, Ledger_Idno", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Yarn_Receipt_Details", "PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " count_idno, Yarn_Type, Mill_IdNo,  Bags, Cones, Weight , Thiri,Rate_For,Rate,Amount", "Sl_No", "PavuYarn_Receipt_Code, For_OrderBy, Company_IdNo, PavuYarn_Receipt_No, PavuYarn_Receipt_Date, Ledger_Idno", tr)

                If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                    vENTDB_DelvToIDno = Val(Common_Procedures.get_FieldValue(con, "PavuYarn_Receipt_Head", "DeliveryTo_Idno", "(PavuYarn_Receipt_Code = '" & Trim(NewCode) & "')", , tr))

                    If Val(vENTDB_DelvToIDno) <> Val(Del_ID) Then

                        cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                            " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                        cmd.ExecuteNonQuery()

                    End If

                End If

                cmd.CommandText = "Update PavuYarn_Receipt_Head set PavuYarn_Receipt_Date = @EntryDate, DeliveryTo_Idno = " & Str(Val(Del_ID)) & ", Empty_Beam = " & Val(txt_KuraiPavuBeam.Text) & ", Meters = " & Val(txt_KuraiPavuMeters.Text) & " ,Party_DcNo = '" & Trim(txt_PartyDcNo.Text) & "' ,  EndsCount_IdNo = " & Str(Val(KuPvu_EdsCnt_ID)) & ",Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' , Transport_Idno = " & Val(Trans_ID) & " ,ReceivedFrom_Idno = " & Val(Rec_ID) & " , Freight_Charge = " & Val(txt_Freight.Text) & " , Note = '" & Trim(txt_Note.Text) & "' , Total_Beam = " & Str(Val(vTotPvuBms)) & ", Total_Meters = " & Str(Val(vTotPvuMtrs)) & ", Total_Bags = " & Str(Val(vTotYrnBags)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & " , Empty_Bobin = " & Str(Val(txt_NoOfBobin.Text)) & ", user_IdNo  =  " & Val(lbl_UserName.Text) & " , Total_Thiri = " & Str(Val(vThiriMeters)) & " , Cloth_IdNo = " & Val(Cloth_ID) & ",Verified_Status=" & Val(Verified_STS) & " , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("Select * from Pavu_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    For i = 0 To Dt1.Rows.Count - 1

                        cmd.CommandText = "update Stock_SizedPavu_Processing_Details set " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("ReceivedFrom_IdNo").ToString)) & ", " _
                                  & " Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1 " _
                                  & " Where " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("DeliveryTo_IdNo").ToString)) & " and " _
                                  & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                                  & " beam_no = '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "' and " _
                                  & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString))
                        cmd.ExecuteNonQuery()

                    Next
                End If
                Dt1.Clear()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "PavuYarn_Receipt_head", "PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "PavuYarn_Receipt_Code, Company_IdNo, for_OrderBy", tr)

            EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)

            If Trim(txt_PartyDcNo.Text) <> "" Then
                PBlNo = Trim(txt_PartyDcNo.Text)
                Partcls = "Rcpt : P.DcNo " & Trim(txt_PartyDcNo.Text)
            Else
                PBlNo = Trim(lbl_RecNo.Text)
                Partcls = "Rcpt : Rec.No " & Trim(lbl_RecNo.Text)
            End If

            cmd.CommandText = "Delete from Pavu_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Yarn_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            If Val(txt_KuraiPavuMeters.Text) <> 0 And Val(KuPvu_EdsCnt_ID) <> 0 Then
                Prtcls_DelvIdNo = Del_ID
                Prtcls_RecIdNo = Rec_ID
                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars , ClothSales_OrderCode_forSelection  ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', -100, " & Str(Val(KuPvu_EdsCnt_ID)) & ", " & Str(Val(txt_KuraiPavuBeam.Text)) & ", " & Str(Val(txt_KuraiPavuMeters.Text)) & ", " & Str(Val(Prtcls_DelvIdNo)) & ", " & Str(Val(Prtcls_RecIdNo)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "')"
                cmd.ExecuteNonQuery()
            End If

            cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            With dgv_PavuDetails
                Sno = 0
                For i = 0 To dgv_PavuDetails.RowCount - 1

                    If Val(dgv_PavuDetails.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1

                        SzPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(5).Value, tr)

                        Bw_IdNo = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(i).Cells(6).Value, tr)

                        'Pavu_DelvInc = Val(Common_Procedures.get_FieldValue(con, "Stock_SizedPavu_Processing_Details", "Pavu_Delivery_Increment", "(Set_Code = '" & Trim(.Rows(i).Cells(9).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' )", , tr))

                        If Val(.Rows(i).Cells(8).Value) = 0 Or (Val(.Rows(i).Cells(8).Value) > 0 And Val(.Rows(i).Cells(8).Value) = Val(.Rows(i).Cells(10).Value)) Then

                            Nr = 0
                            cmd.CommandText = "update Stock_SizedPavu_Processing_Details set StockAt_IdNo = " & Str(Val(Del_ID)) & ", Pavu_Delivery_Increment = Pavu_Delivery_Increment + 1 " &
                                                        " Where  Set_Code = '" & Trim(.Rows(i).Cells(9).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and StockAt_IdNo = " & Str(Val(Rec_ID))
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                Throw New ApplicationException("Invalid Received From Name")
                                Exit Sub
                            End If

                            Ent_NoofUsed = Val(Common_Procedures.get_FieldValue(con, "Stock_SizedPavu_Processing_Details", "Pavu_Delivery_Increment", "(Set_Code = '" & Trim(.Rows(i).Cells(9).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' )", , tr))

                        Else
                            Ent_NoofUsed = Val(.Rows(i).Cells(8).Value)

                        End If

                        cmd.CommandText = "Insert into Pavu_Receipt_Details(PavuYarn_Receipt_Code,              Company_IdNo        ,     PavuYarn_Receipt_No     ,                               for_OrderBy                             , PavuYarn_Receipt_Date,       DeliveryTo_IdNo    ,      ReceivedFrom_IdNo  ,          Sl_No       ,               Set_No                   ,                 Beam_No                ,                    Pcs                    ,                    Meters                ,             EndsCount_IdNo       ,           Beam_Width_IdNo,              Noof_Used        ,                  Set_Code              ) " &
                                                    " Values  (  '" & Trim(NewCode) & "'           , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",        @EntryDate       ,  " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(.Rows(i).Cells(3).Value)) & " , " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(SzPvu_EdsCnt_ID)) & ", " & Str(Val(Bw_IdNo)) & ", " & Str(Val(Ent_NoofUsed)) & ", '" & Trim(.Rows(i).Cells(9).Value) & "')"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(SzPvu_EdsCnt_ID)) & ", 1, " & Str(Val(.Rows(i).Cells(4).Value)) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Pavu_Receipt_Details", "PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No,Beam_No,Pcs,Meters,EndsCount_IdNo,Beam_Width_IdNo,Noof_Used,Set_Code", "Sl_No", "PavuYarn_Receipt_Code, For_OrderBy, Company_IdNo, PavuYarn_Receipt_No, PavuYarn_Receipt_Date, Ledger_Idno", tr)

            End With

            Da = New SqlClient.SqlDataAdapter("select Int1 as PavuEndsCount_IdNo, sum(Int2) as PavuBeam, sum(Meters1) as PavuMeters from " & Trim(Common_Procedures.EntryTempTable) & " group by Int1 having sum(Int2) <> 0 or sum(Meters1) <> 0", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            Sno = 0
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1
                    vTotPvuMtrs = 0
                    vTotPvuMtrs = Str(Val(Dt1.Rows(i).Item("PavuMeters").ToString))

                    Stock_In = ""
                    mtrspcs = 0

                    Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  EndsCount_Head Where EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)), con)
                    Da.SelectCommand.Transaction = tr
                    dt2 = New DataTable
                    Da.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        Stock_In = dt2.Rows(0)("Stock_In").ToString
                        mtrspcs = Val(dt2.Rows(0)("Meters_Pcs").ToString)
                    End If
                    dt2.Clear()

                    If Trim(UCase(Stock_In)) = "PCS" Then
                        If Val(mtrspcs) = 0 Then mtrspcs = 1
                        vTotPvuStk = vTotPvuMtrs / mtrspcs

                    Else
                        vTotPvuStk = vTotPvuMtrs

                    End If

                    Prtcls_DelvIdNo = Del_ID
                    Prtcls_RecIdNo = Rec_ID

                    Sno = Sno + 1

                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars , ClothSales_OrderCode_forSelection ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(vTotPvuStk)) & ", " & Str(Val(Prtcls_DelvIdNo)) & ", " & Str(Val(Prtcls_RecIdNo)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'  )"
                    cmd.ExecuteNonQuery()

                Next
            End If
            Dt1.Clear()

            Delv_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Del_ID)) & ")", , tr)
            Rec_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Rec_ID)) & ")", , tr)


            With dgv_YarnDetails
                Sno = 0
                YSno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(6).Value) <> 0 Then

                        Sno = Sno + 1

                        YCnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        YMil_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)


                        ThiriMeters = 0
                        If .Columns(7).Visible = True Then
                            ThiriMeters = Val(.Rows(i).Cells(7).Value)
                        End If

                        cmd.CommandText = "Insert into Yarn_Receipt_Details (  PavuYarn_Receipt_Code,                 Company_IdNo     ,         PavuYarn_Receipt_No    ,                           for_OrderBy                                   , PavuYarn_Receipt_Date,            Sl_No        ,            count_idno    ,                     Yarn_Type         ,              Mill_IdNo   ,                          Bags            ,               Cones                      ,                        Weight            ,            Thiri            ) " &
                                          " Values                         ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_RecNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & " ,         @EntryDate   ,  " & Str(Val(Sno)) & " , " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(ThiriMeters)) & ")"
                        cmd.ExecuteNonQuery()


                        Stock_Weight = Val(.Rows(i).Cells(6).Value)
                        If Trim(UCase(Delv_Ledtype)) = "WEAVER" Then
                            If .Columns(7).Visible = True Then
                                Stock_Weight = 0
                                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                                    Stock_Weight = Val(.Rows(i).Cells(7).Value)
                                End If
                            End If
                        End If

                        If Val(Stock_Weight) <> 0 Then

                            Stk_DelvIdNo = 0 : Stk_RecIdNo = 0
                            Prtcls_DelvIdNo = 0 : Prtcls_RecIdNo = 0
                            If Trim(UCase(Delv_Ledtype)) = "JOBWORKER" Then
                                Stk_RecIdNo = Del_ID
                                Prtcls_DelvIdNo = Rec_ID

                            Else
                                Stk_DelvIdNo = Del_ID
                                Prtcls_RecIdNo = Rec_ID

                            End If

                            YSno = YSno + 1
                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (           Reference_Code                    , Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars , ClothSales_OrderCode_forSelection ) " &
                                                 " Values                                ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(YSno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(Stock_Weight)) & ", " & Str(Val(Prtcls_DelvIdNo)) & ", " & Str(Val(Prtcls_RecIdNo)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "')"
                            'cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(                   Reference_Code           ,                 Company_IdNo     ,               Reference_No    ,                               for_OrderBy                              , Reference_Date,       DeliveryTo_Idno    ,       ReceivedFrom_Idno  ,         Entry_ID      ,         Particulars    ,       Party_Bill_No   ,            Sl_No      ,            Count_IdNo     ,                       Yarn_Type        ,             Mill_IdNo    ,                       Bags                ,                Cones                      ,                        Weight            , DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars ) " & _
                            '                  "Values                                   ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",    @EntryDate , " & Str(Val(Del_ID)) & " , " & Str(Val(Rec_ID)) & " , '" & Trim(EntID) & "' , '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "' , " & Str(Val(Sno)) & " , " & Str(Val(YCnt_ID)) & " , '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & " , " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(.Rows(i).Cells(6).Value)) & ",    " & Str(Val(Del_ID)) & "  ,      " & Str(Val(Rec_ID)) & "   )"

                            cmd.ExecuteNonQuery()

                        End If

                        Stock_Weight = Val(.Rows(i).Cells(6).Value)
                        If Trim(UCase(Rec_Ledtype)) = "WEAVER" Then
                            If .Columns(7).Visible = True Then
                                Stock_Weight = 0
                                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                                    Stock_Weight = Val(.Rows(i).Cells(7).Value)
                                End If
                            End If
                        End If

                        If Val(Stock_Weight) <> 0 Then

                            Stk_DelvIdNo = 0 : Stk_RecIdNo = 0
                            Prtcls_DelvIdNo = 0 : Prtcls_RecIdNo = 0
                            If Trim(UCase(Rec_Ledtype)) = "JOBWORKER" Then
                                Stk_DelvIdNo = Rec_ID
                                Prtcls_RecIdNo = Del_ID
                            Else
                                Stk_RecIdNo = Rec_ID
                                Prtcls_DelvIdNo = Del_ID
                            End If

                            YSno = YSno + 1
                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars , ClothSales_OrderCode_forSelection) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @EntryDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(YSno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(Stock_Weight)) & ", " & Str(Val(Prtcls_DelvIdNo)) & ", " & Str(Val(Prtcls_RecIdNo)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "')"
                            cmd.ExecuteNonQuery()

                        End If


                        'cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(                   Reference_Code           ,                 Company_IdNo     ,               Reference_No    ,                               for_OrderBy                              , Reference_Date,       DeliveryTo_Idno    ,       ReceivedFrom_Idno  ,         Entry_ID      ,         Particulars    ,       Party_Bill_No   ,            Sl_No      ,            Count_IdNo     ,                       Yarn_Type        ,             Mill_IdNo    ,                       Bags                ,                Cones                      ,                        Weight            , DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars ) " & _
                        '                  "Values                                   ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",    @EntryDate , " & Str(Val(Del_ID)) & " , " & Str(Val(Rec_ID)) & " , '" & Trim(EntID) & "' , '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "' , " & Str(Val(Sno)) & " , " & Str(Val(YCnt_ID)) & " , '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & " , " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(.Rows(i).Cells(6).Value)) & ",    " & Str(Val(Del_ID)) & "  ,      " & Str(Val(Rec_ID)) & "   )"
                        'cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Yarn_Receipt_Details", "PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " count_idno, Yarn_Type, Mill_IdNo,  Bags, Cones, Weight , Thiri,Rate_For,Rate,Amount", "Sl_No", "PavuYarn_Receipt_Code, For_OrderBy, Company_IdNo, PavuYarn_Receipt_No, PavuYarn_Receipt_Date, Ledger_Idno", tr)

            End With

            Dim Empty_Bms As Integer
            Empty_Bms = 0
            If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                Empty_Bms = Val(txt_KuraiPavuBeam.Text)
            Else
                Empty_Bms = Val(txt_KuraiPavuBeam.Text) + Val(vTotPvuBms)
            End If

            If Val(txt_KuraiPavuBeam.Text) <> 0 Or Val(vTotPvuBms) <> 0 Or Val(vTotYrnBags) <> 0 Or Val(vTotYrnCones) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam,   Pavu_Beam , Empty_Bobin, Yarn_Bags, Yarn_Cones, Particulars, Entry_ID) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(PBlNo) & "', 1, 0, 0, " & Str(Val(Empty_Bms)) & "," & Str(Val(txt_NoOfBobin.Text)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", '" & Trim(Partcls) & "', '" & Trim(EntID) & "')"
                cmd.ExecuteNonQuery()
            End If

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                      " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()


            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)


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

    Private Sub cbo_DelvAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DelvAt.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DelvAt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvAt.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelvAt, cbo_RecForm, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DelvAt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DelvAt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DelvAt, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DelvAt_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvAt.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DelvAt.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PavuYarn_Receipt_Head", "Vehicle_No", "", "(Vehicle_No = '')")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, txt_Freight, "PavuYarn_Receipt_Head", "Vehicle_No", "", "(Vehicle_No = '')")
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_Freight, "PavuYarn_Receipt_Head", "Vehicle_No", "", "(Vehicle_No = '')", False)
    End Sub

    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, txt_PartyDcNo, txt_KuraiPavuMeters, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_KuraiPavuMeters, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
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
        Dim Del_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""
        Dim EdsCnt_IdNo As Integer, Mil_IdNo As Integer

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0
            EdsCnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.PavuYarn_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.PavuYarn_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.PavuYarn_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_DeliveryName.Text) <> "" Then
                Del_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_DeliveryName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Trim(cbo_Filter_MillName.Text) <> "" Then
                Mil_IdNo = Common_Procedures.Mill_NameToIdNo(con, cbo_Filter_MillName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                EdsCnt_IdNo = Common_Procedures.EndsCount_NameToIdNo(con, cbo_Filter_EndsCount.Text)
            End If


            If Val(Del_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.ReceivedFrom_Idno = " & Str(Val(Del_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.PavuYarn_Receipt_Code IN (select z1.PavuYarn_Receipt_Code from Yarn_Receipt_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.PavuYarn_Receipt_Code IN (select z1.PavuYarn_Receipt_Code from Yarn_Receipt_Details z1 where z1.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ")"
            End If

            If Val(EdsCnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo))
            End If

            If cbo_Verified_Sts.Visible = True And Trim(cbo_Verified_Sts.Text) <> "" Then

                If Trim(cbo_Verified_Sts.Text) = "YES" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 1 "
                ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 0 "
                End If

            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as Deliv_Name from PavuYarn_Receipt_Head a INNER JOIN Ledger_Head b on a.ReceivedFrom_Idno = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.PavuYarn_Receipt_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("PavuYarn_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("PavuYarn_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Deliv_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Beam").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Total_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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
            cbo_Filter_DeliveryName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_DeliveryName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_DeliveryName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_DeliveryName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_DeliveryName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_DeliveryName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_DeliveryName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_DeliveryName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            Filter_RowNo = dgv_Filter_Details.CurrentRow.Index
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

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_YarnDetails.EditingControl.BackColor = Color.Lime
        dgv_YarnDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()

    End Sub

    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        TotalPavu_Calculation()
        'SendKeys.Send("{up}")
        'SendKeys.Send("{Tab}")
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter
        With dgv_PavuDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave
        With dgv_PavuDetails
            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        On Error Resume Next

        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
        With dgv_PavuDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Then
                    TotalPavu_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_Details = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyDown

        On Error Resume Next

        With dgv_PavuDetails

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True
                    cbo_RecForm.Focus()
                End If
            End If

            If e.KeyCode = Keys.Down Then
                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True

                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.Focus()

                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                Else
                    SendKeys.Send("{Tab}")

                End If

            End If

        End With

    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

                n = .CurrentRow.Index

                If Val(.Rows(n).Cells(8).Value) > 0 And Val(.Rows(n).Cells(8).Value) <> Val(.Rows(n).Cells(10).Value) Then
                    MessageBox.Show("Cannot Delete" & Chr(13) & "Already this pavu delivered to others")
                    Exit Sub
                End If

                If n = .Rows.Count - 1 Then

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

            TotalPavu_Calculation()

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer

        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
        With dgv_PavuDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub TotalPavu_Calculation()
        Dim Sno As Integer
        Dim TotBms As Single, TotMtrs As Single

        Sno = 0
        TotBms = 0
        TotMtrs = 0
        With dgv_PavuDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotBms)
            .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
        End With

    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEndEdit
        If dgv_YarnDetails.CurrentRow.Cells(2).Value = "MILL" Then
            If dgv_YarnDetails.CurrentCell.ColumnIndex = 4 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 5 Then
                get_MillCount_Details()
            End If
        End If


        'TotalYarnTaken_Calculation()
        'SendKeys.Send("{up}")
        'SendKeys.Send("{Tab}")
    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_YarnDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(.CurrentRow.Cells(2).Value) = "" Then
                .CurrentRow.Cells(2).Value = "MILL"
            End If

            If e.ColumnIndex = 1 Then

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

                    'cbo_Grid_MillName.Visible = False
                    'cbo_Grid_YarnType.Visible = False

                End If


            Else

                cbo_Grid_CountName.Visible = False
                'cbo_Grid_CountName.Tag = -1
                'cbo_Grid_CountName.Text = ""

            End If

            If e.ColumnIndex = 2 Then

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

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_YarnType.Visible = False

                End If

            Else

                cbo_Grid_YarnType.Visible = False
                'cbo_Grid_YarnType.Tag = -1
                'cbo_Grid_YarnType.Text = ""

            End If

            If e.ColumnIndex = 3 Then

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

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_Grid_MillName.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If



        End With
    End Sub

    Private Sub dgv_YarnDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellLeave
        With dgv_YarnDetails

            'If e.ColumnIndex = 1 Then
            '    cbo_Grid_CountName.Visible = False
            '    'cbo_Grid_CountName.Tag = -1
            '    'cbo_Grid_CountName.Text = ""
            'End If

            'If e.ColumnIndex = 2 Then
            '    cbo_Grid_YarnType.Visible = False
            '    'cbo_Grid_YarnType.Tag = -1
            '    'cbo_Grid_YarnType.Text = ""
            'End If

            'If e.ColumnIndex = 3 Then
            '    cbo_Grid_MillName.Visible = False
            '    'cbo_Grid_MillName.Tag = -1
            '    'cbo_Grid_MillName.Text = ""
            'End If

            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellValueChanged
        On Error Resume Next
        With dgv_YarnDetails
            If .Visible = True Then
                If IsNothing(.CurrentCell) Then Exit Sub
                If e.ColumnIndex = 5 Or e.ColumnIndex = 6 Then
                    If .Columns(7).Visible = True Then

                        If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1) Then
                            Thiri_Calculation()
                        ElseIf Val(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status = 1) Then
                            Meters_Calculation()
                        End If

                    End If
                End If
                TotalYarnTaken_Calculation()
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyDown

        'On Error Resume Next

        On Error Resume Next

        With dgv_YarnDetails

            'MsgBox("dgv_YarnDetails_KeyDown : " & .CurrentCell.ColumnIndex())

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell.Selected = True
                    'SendKeys.Send("{RIGHT}")
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 0 Then
                    .CurrentCell.Selected = False
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell.Selected = True
                    'SendKeys.Send("{RIGHT}")
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    txt_Note.Focus()
                    'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    '    save_record()
                    'End If
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
        If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
        dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_YarnDetails.RowsAdded
        Dim n As Integer

        With dgv_YarnDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
            .Rows(n - 1).Cells(2).Value = "MILL"
        End With
    End Sub

    Private Sub TotalYarnTaken_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single, TotMeters As Single

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        TotMeters = 0
        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(6).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(4).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(5).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(6).Value)
                    If .Columns(7).Visible = True Then
                        TotMeters = TotMeters + Val(.Rows(i).Cells(7).Value)
                    End If
                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBags)
            .Rows(0).Cells(5).Value = Val(TotCones)
            .Rows(0).Cells(6).Value = Format(Val(TotWeight), "########0.000")
            If .Columns(7).Visible = True Then
                .Rows(0).Cells(7).Value = Format(Val(TotMeters), "########0.00")
            End If
        End With

    End Sub
    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown

        Dim dep_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, cbo_Grid_YarnType, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    If cbo_Cloth.Visible = True Then
                        cbo_Cloth.Focus()

                    ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                        cbo_ClothSales_OrderCode_forSelection.Focus()


                    Else
                        txt_Freight.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)
                    .CurrentCell.Selected = True


                End If
            End If

            If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then


                    txt_Note.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_CountName.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then


                    txt_Note.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End With

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
    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub
    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Dim dep_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True

            End If

        End With
    End Sub

    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails
                .Focus()
                .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_MillName.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
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

    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")
    End Sub

    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_YarnType.KeyDown
        Dim dep_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_YarnType, Nothing, Nothing, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End If

        End With
    End Sub

    Private Sub cbo_Grid_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_YarnType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_YarnType, Nothing, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Focus()
                .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_YarnType.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End With

        End If
    End Sub
    Private Sub cbo_Grid_YarnType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.TextChanged
        Try
            If cbo_Grid_YarnType.Visible Then

                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
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

    Private Sub txt_KuraiPavuBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_KuraiPavuBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgv_YarnDetails_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_YarnDetails.GotFocus
        'dgv_YarnDetails.Focus()
        'dgv_YarnDetails.CurrentCell.Selected = True
    End Sub

    Private Sub cbo_Filter_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, cbo_Filter_EndsCount, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, cbo_Filter_EndsCount, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsCount.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsCount, cbo_Filter_MillName, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Filter_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsCount, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_Verified_Sts_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Verified_Sts.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Verified_Sts_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Verified_Sts.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Verified_Sts, cbo_Filter_EndsCount, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub cbo_Verified_Sts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Verified_Sts.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Verified_Sts, btn_Filter_Show, "", "", "", "")
    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.PavuYarn_Receipt_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from PavuYarn_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
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





        prn_TotCopies = 1
        Prnt_HalfSheet_STS = False

        vPrnt_2Copy_In_SinglePage = Common_Procedures.settings.PavuYarnReceipt_Print_2Copy_In_SinglePage

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Sizing (Somanur)

            Dim mymsgbox As New Tsoft_MessageBox("Select Paper Size to Print", "A4,HALF-SHEET,CANCEL", "FOR PRINTING...", "IF A4 is selected, 2 copies of dc will be printed in single A4 sheet," & Chr(13) & "If HALF-SHEET is selected 1 copy of dc will be printed in 8x6 paper size", MesssageBoxIcons.Questions, 2)
            mymsgbox.ShowDialog()

            If mymsgbox.MessageBoxResult = 1 Then
                vPrnt_2Copy_In_SinglePage = 1

            ElseIf mymsgbox.MessageBoxResult = 2 Then
                Prnt_HalfSheet_STS = True

                vPrnt_2Copy_In_SinglePage = 0

            Else

                Exit Sub

            End If

            'prn_TotCopies = Val(InputBox("Enter No.of Copies", "FOR DELIVERY PRINTING...", "1"))
            'If Val(prn_TotCopies) <= 0 Then
            '    Exit Sub
            'End If

        End If


        set_PaperSize_For_PrintDocument1()

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    'Debug.Print(ps.PaperName)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '    If PpSzSTS = False Then
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                Exit For
        '            End If
        '        Next
        '    End If

        'End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        '    'Debug.Print(ps.PaperName)
                        '    If ps.Width = 800 And ps.Height = 600 Then
                        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        '        PpSzSTS = True
                        '        Exit For
                        '    End If
                        'Next

                        'If PpSzSTS = False Then
                        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
                        '            PpSzSTS = True
                        '            Exit For
                        '        End If
                        '    Next

                        '    If PpSzSTS = False Then
                        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
                        '                Exit For
                        '            End If
                        '        Next
                        '    End If

                        'End If
                        set_PaperSize_For_PrintDocument1()

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
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim i As Integer
        Dim vDup_SetNo As String
        Dim vPvu_BmNo As String, vDup_BmNo As String
        Dim W1 As Single = 0
        Dim FsNo As Single, LsNo As Single
        Dim FsBeamNo As String, LsBeamNo As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Recfrom_Name, c.Ledger_Address1, c.Ledger_Address2 ,c.Ledger_Address3,c.Ledger_Address4 from PavuYarn_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = c.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name from Yarn_Receipt_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                vPrn_PvuEdsCnt = ""
                vPrn_PvuTotBms = 0
                vPrn_PvuTotMtrs = 0 : vPrn_PvuNPcs = 0
                vPrn_PvuSetNo = "" : vDup_SetNo = ""
                vDup_BmNo = "" : vPvu_BmNo = ""
                vPrn_PvuBmNos1 = "" : vPrn_PvuBmNos2 = "" : vPrn_PvuBmNos3 = "" : vPrn_PvuBmNos4 = ""

                cmd.Connection = con

                cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                cmd.ExecuteNonQuery()

                da1 = New SqlClient.SqlDataAdapter("select a.*,b.* from Pavu_Receipt_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                Dt1 = New DataTable
                da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    vPrn_PvuEdsCnt = Dt1.Rows(0).Item("EndsCount_Name").ToString

                    For i = 0 To Dt1.Rows.Count - 1

                        vPrn_PvuTotBms = Val(vPrn_PvuTotBms) + 1
                        vPrn_PvuTotMtrs = vPrn_PvuTotMtrs + Val(Dt1.Rows(i).Item("Meters").ToString)
                        vPrn_PvuNPcs = vPrn_PvuNPcs + Val(Dt1.Rows(i).Item("Noof_Used").ToString)

                        If InStr(1, "~" & Trim(UCase(vDup_SetNo)) & "~", "~" & Trim(UCase(Dt1.Rows(i).Item("Set_No").ToString)) & "~") = 0 Then
                            vDup_SetNo = Trim(vDup_SetNo) & "~" & Trim(Dt1.Rows(i).Item("Set_No").ToString) & "~"
                            vPrn_PvuSetNo = vPrn_PvuSetNo & IIf(Trim(vPrn_PvuSetNo) <> "", ", ", "") & Dt1.Rows(i).Item("Set_No").ToString
                        End If

                        If InStr(1, "~" & Trim(UCase(vDup_BmNo)) & "~", "~" & Trim(UCase(Dt1.Rows(i).Item("Set_No").ToString)) & "^" & Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString)) & "~") = 0 Then
                            vDup_BmNo = Trim(vDup_BmNo) & "~" & Trim(Dt1.Rows(i).Item("Set_No").ToString) & "^" & Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString)) & "~"

                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Meters1) values ('" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "', " & Common_Procedures.OrderBy_CodeToValue(Trim(Dt1.Rows(i).Item("Beam_No").ToString)) & " )"
                            cmd.ExecuteNonQuery()

                        End If

                    Next i

                End If

                Dt1.Clear()

                vPvu_BmNo = ""
                FsNo = 0 : LsNo = 0
                FsBeamNo = "" : LsBeamNo = ""

                da1 = New SqlClient.SqlDataAdapter("Select Name1 as Beam_No, Meters1 as fororderby_beamno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name1", con)
                Dt1 = New DataTable
                da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    FsNo = Dt1.Rows(0).Item("fororderby_beamno").ToString
                    LsNo = Dt1.Rows(0).Item("fororderby_beamno").ToString

                    FsBeamNo = Trim(UCase(Dt1.Rows(0).Item("Beam_No").ToString))
                    LsBeamNo = Trim(UCase(Dt1.Rows(0).Item("Beam_No").ToString))

                    For i = 1 To Dt1.Rows.Count - 1
                        If LsNo + 1 = Val(Dt1.Rows(i).Item("fororderby_beamno").ToString) Then
                            LsNo = Val(Dt1.Rows(i).Item("fororderby_beamno").ToString)
                            LsBeamNo = Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString))

                        Else
                            If FsNo = LsNo Then
                                vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & ","
                            Else
                                vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & "-" & Trim(LsBeamNo) & ","
                            End If
                            FsNo = Dt1.Rows(i).Item("fororderby_beamno").ToString
                            LsNo = Dt1.Rows(i).Item("fororderby_beamno").ToString

                            FsBeamNo = Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString))
                            LsBeamNo = Trim(UCase(Dt1.Rows(i).Item("Beam_No").ToString))

                        End If

                    Next

                    If FsNo = LsNo Then vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) Else vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & "-" & Trim(LsBeamNo)

                End If
                Dt1.Clear()


                vPrn_PvuBmNos1 = Trim(vPvu_BmNo)
                vPrn_PvuBmNos2 = ""
                vPrn_PvuBmNos3 = ""
                vPrn_PvuBmNos4 = ""
                If Len(vPrn_PvuBmNos1) > 18 Then
                    For i = 18 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos1), i, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos1), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 18
                    vPrn_PvuBmNos2 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos1), Len(vPrn_PvuBmNos1) - i)
                    vPrn_PvuBmNos1 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos1), i - 1)
                End If

                If Len(vPrn_PvuBmNos2) > 23 Then
                    For i = 23 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos2), i, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos2), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 23
                    vPrn_PvuBmNos3 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos2), Len(vPrn_PvuBmNos2) - i)
                    vPrn_PvuBmNos2 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos2), i - 1)
                End If

                If Len(vPrn_PvuBmNos3) > 23 Then
                    For i = 23 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos3), i, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos3), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 23
                    vPrn_PvuBmNos4 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos3), Len(vPrn_PvuBmNos3) - i)
                    vPrn_PvuBmNos3 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos3), i - 1)
                End If

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
        Printing_Format1(e)
        'End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
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
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0

        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        PrntCnt = 1

        If vPrnt_2Copy_In_SinglePage = 1 Then

            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '    Debug.Print(ps.PaperName)
            '    If ps.Width = 800 And ps.Height = 600 Then
            '        PrintDocument1.DefaultPageSettings.PaperSize = ps
            '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '        e.PageSettings.PaperSize = ps
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next
            set_PaperSize_For_PrintDocument1()

            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If

            'Else

            '    If PpSzSTS = False Then
            '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '                PrintDocument1.DefaultPageSettings.PaperSize = ps
            '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '                e.PageSettings.PaperSize = ps
            '                PpSzSTS = True
            '                Exit For
            '            End If
            '        Next

            '        If PpSzSTS = False Then
            '            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
            '                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '                    PrintDocument1.DefaultPageSettings.PaperSize = ps
            '                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '                    e.PageSettings.PaperSize = ps
            '                    Exit For
            '                End If
            '            Next
            '        End If

            '    End If

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

        NoofItems_PerPage = 8 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 50 : ClArr(3) = 130 : ClArr(4) = 65 : ClArr(5) = 65 : ClArr(6) = 70 : ClArr(7) = 85
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin


                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If

            End If
            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                    W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                    NoofDets = 0


                    NoofItems_PerPage = 5
                    If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > NoofItems_PerPage Then
                            NoofItems_PerPage = 35
                        End If
                    End If

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            prn_DetSNo = prn_DetSNo + 1

                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 18 Then
                                For I = 18 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 18
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            End If
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                            If prn_DetIndx = 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, "Ends Count", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuEdsCnt), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 1 Then
                                Common_Procedures.Print_To_PrintDocument(e, "No.of Beams", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuTotBms)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 2 Then
                                Common_Procedures.Print_To_PrintDocument(e, "Meters", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(vPrn_PvuTotMtrs), "#########0.00")), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 3 Then
                                Common_Procedures.Print_To_PrintDocument(e, "Noof_Used", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuNPcs)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)


                            ElseIf prn_DetIndx = 4 Then
                                Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 5 Then
                                Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 6 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 7 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                            ElseIf prn_DetIndx = 8 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos4), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                            End If

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If


                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then


                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count = cnt + 6 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 6
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

                prn_DetIndx = 0
                'prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from Yarn_Receipt_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.PavuYarn_Receipt_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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

        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            '.BackgroundImage = Image.FromStream(ms)

                            ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 20, CurY + 5, 110, 100)

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
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("RE.C.NO    : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Recfrom_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PavuYarn_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("PavuYarn_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "PARTY D.C.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            'End If
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PAVU DETAILS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0

        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt



                If prn_DetIndx = 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Ends Count", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuEdsCnt), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 1 Then
                    Common_Procedures.Print_To_PrintDocument(e, "No.of Beams", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuTotBms)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 2 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Meters", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(vPrn_PvuTotMtrs), "#########0.00")), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 3 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Noof_Used", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(vPrn_PvuNPcs)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)


                ElseIf prn_DetIndx = 4 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 5 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 6 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 7 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 8 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                End If

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 2, ClAr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            If Common_Procedures.settings.CustomerCode = "1370" Then



                ' CurY = CurY + TxtHgt - 5

                ' Common_Procedures.Print_To_PrintDocument(e, "Remarks : " & Trim(prn_HdDt.Rows(0).Item("Note").ToString), LMargin + 10, CurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " Remarks : " & Trim(prn_HdDt.Rows(0).Item("Note").ToString), LMargin + 10, CurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(7) = CurY

                ' CurY = CurY + TxtHgt
            End If
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Public Sub Get_vehicle_from_Transport()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim transport_id As Integer
        transport_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_Transport.Text)
        Da = New SqlClient.SqlDataAdapter("select vehicle_no from ledger_head where ledger_idno=" & Str(Val(transport_id)) & "", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count <> 0 Then
            cbo_VehicleNo.Text = Dt.Rows(0).Item("vehicle_no").ToString


        End If
        Dt.Clear()
    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()

    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, Nothing, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_Transport.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            SendKeys.Send("+{TAB}")

        End If
        Get_vehicle_from_Transport()
    End Sub


    Private Sub txt_Frieght_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyCode = 38 Then cbo_VehicleNo.Focus()
        If e.KeyCode = 40 Then
            If cbo_Cloth.Visible = True Then
                cbo_Cloth.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                ' dgv_YarnDetails.CurrentCell.Selected = True
            End If
        End If

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub txt_Frieght_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If cbo_Cloth.Visible = True Then
                cbo_Cloth.Focus()

            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()

            Else
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                dgv_YarnDetails.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyCode = 40 Then msk_date.Focus()
        If e.KeyCode = 38 Then
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            ' dgv_YarnDetails.CurrentCell.Selected = True
        End If
    End Sub



    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_KuraiPavuMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_KuraiPavuMeters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub cbo_RecForm_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RecForm.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- M.S Textiles (Tirupur)
        '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = '' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or Show_In_All_Entry = 1 ) and Close_status = 0 ) ", "(Ledger_IdNo = 0)")
        'Else
        '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        'End If
    End Sub

    Private Sub cbo_RecForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecForm, msk_date, cbo_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- M.S Textiles (Tirupur)
        '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecForm, msk_date, cbo_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = '' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or Show_In_All_Entry = 1 ) and Close_status = 0 ) ", "(Ledger_IdNo = 0)")
        'Else
        '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecForm, msk_date, cbo_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        'End If
    End Sub

    Private Sub cbo_RecForm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RecForm.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecForm, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- M.S Textiles (Tirupur)
        '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecForm, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = '' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or Show_In_All_Entry = 1 ) and Close_status = 0 ) ", "(Ledger_IdNo = 0)")
        'Else
        '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecForm, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        'End If

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Pavu  :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                cbo_DelvAt.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_RecForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RecForm.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String

        LedNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)

        If LedNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then
            If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
                CompIDCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompIDCondt = "(Company_Type <> 'UNACCOUNT')"
                End If
            End If
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.noof_used as Ent_NoofUsed, b.*, c.EndsCount_Name, d.Beam_Width_Name from Pavu_Receipt_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON b.Beam_Width_Idno = d.Beam_Width_Idno where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & "  a.PavuYarn_Receipt_Code = '" & Trim(NewCode) & "' and a.ReceivedFrom_IdNo = " & Str(Val(LedNo)) & " order by a.for_orderby, a.Set_Code, b.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                    .Rows(n).Cells(7).Value = "1"
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Ent_NoofUsed").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Pavu_Delivery_Increment").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        If Val(.Rows(n).Cells(8).Value) <> Val(.Rows(n).Cells(8).Value) Then
                            .Rows(i).Cells(j).Style.BackColor = Color.Gray
                        End If
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name, c.Beam_Width_Name from Stock_SizedPavu_Processing_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head c ON a.Beam_Width_Idno = c.Beam_Width_Idno where  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & "  a.StockAt_IdNo = " & Str(Val(LedNo)) & " and a.Pavu_Delivery_Code = '' and a.Beam_Knotting_Code = '' and a.Close_Status = 0 order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString) - Val(Dt1.Rows(i).Item("Production_Meters").ToString), "#########0.00")
                    '.Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                    .Rows(n).Cells(7).Value = ""
                    .Rows(n).Cells(8).Value = "-9999"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Pavu_Delivery_Increment").ToString

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        If dgv_Selection.Rows.Count > 0 Then
            dgv_Selection.Focus()
            dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        End If

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Pavu(e.RowIndex)
    End Sub

    Private Sub Select_Pavu(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                If Val(.Rows(RwIndx).Cells(8).Value) > 0 And Val(.Rows(RwIndx).Cells(8).Value) <> Val(.Rows(RwIndx).Cells(10).Value) Then
                    MessageBox.Show("Cannot deselect" & Chr(13) & "Already this pavu delivered to others")
                    Exit Sub
                End If

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

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                Select_Pavu(dgv_Selection.CurrentCell.RowIndex)
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim n As Integer
        Dim sno As Integer

        With dgv_PavuDetails

            .Rows.Clear()

            For i = 0 To dgv_Selection.RowCount - 1

                If Val(dgv_Selection.Rows(i).Cells(7).Value) = 1 Then

                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(0).Value = Val(sno)
                    .Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                    .Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                    .Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                    .Rows(n).Cells(4).Value = Format(Val(dgv_Selection.Rows(i).Cells(4).Value), "#########0.00")
                    .Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(5).Value
                    .Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value

                    .Rows(n).Cells(8).Value = ""

                    If Val(dgv_Selection.Rows(i).Cells(8).Value) > 0 Then

                        If Val(dgv_Selection.Rows(i).Cells(8).Value) <> Val(dgv_Selection.Rows(i).Cells(10).Value) Then
                            .Rows(n).Cells(7).Value = "1"
                        Else
                            .Rows(n).Cells(7).Value = ""
                        End If

                        .Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(8).Value

                    End If

                    .Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(9).Value

                    .Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(10).Value

                End If

            Next

        End With

        TotalPavu_Calculation()

        Grid_Cell_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_DelvAt.Enabled And cbo_DelvAt.Visible Then cbo_DelvAt.Focus()

    End Sub

    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnDetails.EditingControlShowing
        dgtxt_Details = CType(dgv_YarnDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_YarnDetails_KeyUp(sender, e)
        End If
    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
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
    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then

            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
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

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, txt_Freight, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        With dgv_YarnDetails
            If e.KeyCode = 40 And cbo_Cloth.DroppedDown = False Or (e.KeyCode = 40 And e.Control = True) Then

                If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                    cbo_ClothSales_OrderCode_forSelection.Focus()

                Else

                    If .Visible = True Then
                        .Focus()
                        .CurrentCell = .CurrentRow.Cells(1)
                        .CurrentCell.Selected = True
                    Else
                        txt_Note.Focus()
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        With dgv_YarnDetails
            If Asc(e.KeyChar) = 13 Then

                If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                    cbo_ClothSales_OrderCode_forSelection.Focus()
                Else



                    If .Visible = True Then
                        .Focus()
                        .CurrentCell = .CurrentRow.Cells(1)
                        .CurrentCell.Selected = True
                    Else
                        txt_Note.Focus()
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub Meters_Calculation()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Cloth_ID As Integer
        Dim WeftCon As Single
        Dim Mtrs As Integer = 0


        Cloth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

        WeftCon = 0

        da = New SqlClient.SqlDataAdapter("SELECT * FROM Cloth_Head where Cloth_IdNo = " & Val(Cloth_ID) & "", con)
        dt = New DataTable
        da.Fill(dt)

        With dgv_YarnDetails
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    WeftCon = Val(dt.Rows(0).Item("Weight_Meter_Weft").ToString)
                End If
            End If

            If Val(.Rows(.CurrentRow.Index).Cells(6).Value) <> 0 Then
                .Rows(.CurrentRow.Index).Cells(7).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(6).Value) / Val(WeftCon), "########0.00")
            Else
                .Rows(.CurrentRow.Index).Cells(7).Value = Val(.Rows(.CurrentRow.Index).Cells(6).Value)
            End If
        End With

    End Sub

    Private Sub cbo_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyUp
        If e.Control = False And e.KeyCode = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub Thiri_Calculation()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim count_val As Single
        Dim CntID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value)

        count_val = 0

        Da = New SqlClient.SqlDataAdapter("select (Resultant_Count) from Count_Head where count_idno = " & Str(Val(CntID)), con)
        Da.Fill(Dt)
        With dgv_YarnDetails

            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    count_val = Dt.Rows(0).Item("Resultant_Count").ToString
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()


            If Val(.Rows(.CurrentRow.Index).Cells(6).Value) <> 0 Then
                .Rows(.CurrentRow.Index).Cells(7).Value = Format(count_val * 11 / 50 * .Rows(.CurrentRow.Index).Cells(6).Value, "##########0.000")
            Else
                .Rows(.CurrentRow.Index).Cells(7).Value = ""
            End If

        End With

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

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub
    Private Sub cbo_Grid_CountName_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
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

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If Asc(e.KeyChar) = 13 Then

            If dgv_YarnDetails.Rows.Count > 0 Then

                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)


            Else


                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    msk_date.Focus()
                End If

            End If

        End If
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")


        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)


            Else


                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    msk_date.Focus()
                End If
            End If

        End If

        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then


            If cbo_Cloth.Visible = True Then
                cbo_Cloth.Focus()
            Else
                txt_Freight.Focus()
            End If
        End If






    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter

    End Sub

    Private Sub txt_Freight_TextChanged(sender As Object, e As EventArgs) Handles txt_Freight.TextChanged

    End Sub

    Private Sub cbo_Cloth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Cloth.SelectedIndexChanged

    End Sub

    Private Sub cbo_Grid_CountName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Grid_CountName.SelectedIndexChanged

    End Sub
End Class

