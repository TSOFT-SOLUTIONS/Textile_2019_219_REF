Public Class JobWork_PavuYarn_Receipt
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "JPYRC-"
    Private Prec_ActCtrl As New Control
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private dgv_ActCtrlName As String = ""
    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_PavuDetails As New DataGridViewTextBoxEditingControl

    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String

    Private vcbo_KeyDwnVal As Double
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        txt_InvoicePrefixNo.Text = ""

        vmskOldText = ""
        vmskSelStrt = -1


        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        txt_PartyDcNo.Text = ""
        txt_remarks.Text = ""
        cbo_Ledger.Text = ""
        cbo_Ledger.Tag = ""
        cbo_EndsCount.Text = ""
        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""

        cbo_Cloth.Text = ""
        cbo_WidthType.Text = ""
        txt_CrimpPerc.Text = ""
        cbo_DeliveryTo.Text = ""

        dgv_PavuDetails.Rows.Clear()

        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        dgv_YarnDetails.Rows.Clear()
        dgv_YarnDetails.Rows(0).Cells(2).Value = "MILL"

        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_MillName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
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

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_EndsCount.Enabled = True
        cbo_EndsCount.BackColor = Color.White

        dgv_ActCtrlName = ""

    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(2, 57, 111)
                Prec_ActCtrl.ForeColor = Color.White
            ElseIf TypeOf Prec_ActCtrl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
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
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName, c.Cloth_Name, d.EndsCount_Name ,e.Ledger_Name as Transport_Name from JobWork_PavuYarn_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo Where a.JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString

                lbl_RecNo.Text = dt1.Rows(0).Item("JobWork_PavuYarn_Receipt_RefNo").ToString
                dtp_Date.Text = dt1.Rows(0).Item("JobWork_PavuYarn_Receipt_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                txt_PartyDcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                cbo_Cloth.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                cbo_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
                txt_CrimpPerc.Text = dt1.Rows(0).Item("Crimp_Percentage").ToString

                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                cbo_Transport.Text = dt1.Rows(0).Item("Transport_Name").ToString

                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))

                LockSTS = False

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Pavu_Delivery_Code, b.Pavu_Delivery_Increment, b.Beam_Knotting_Code, b.Loom_Idno, b.Production_Meters, b.Close_Status from JobWork_Pavu_Receipt_Details a, Stock_SizedPavu_Processing_Details b Where a.JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "' and b.Reference_Code ='" & Trim(Pk_Condition) & "' + a.JobWork_PavuYarn_Receipt_Code and a.Set_No = b.Set_No and a.Beam_No = b.Beam_No Order by a.Sl_No", con)
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
                        dgv_PavuDetails.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        If Trim(dt2.Rows(i).Item("Pavu_Delivery_Code").ToString) <> "" Or Val(dt2.Rows(i).Item("Pavu_Delivery_Increment").ToString) <> 0 Or Trim(dt2.Rows(i).Item("Beam_Knotting_Code").ToString) <> "" Or Val(dt2.Rows(i).Item("Loom_Idno").ToString) <> 0 Or Val(dt2.Rows(i).Item("Production_Meters").ToString) <> 0 Or Val(dt2.Rows(i).Item("Close_Status").ToString) <> 0 Then
                            dgv_PavuDetails.Rows(n).Cells(4).Value = "1"
                            LockSTS = True
                        Else
                            dgv_PavuDetails.Rows(n).Cells(4).Value = ""
                        End If

                        dgv_PavuDetails.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Po_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Warp_Lot_No").ToString



                    Next i

                End If

                With dgv_PavuDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Beam").ToString)
                    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                End With

                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from JobWork_Yarn_Receipt_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                        dgv_YarnDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Po_No").ToString
                        dgv_YarnDetails.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Weft_Lot_No").ToString

                    Next i

                End If

                With dgv_YarnDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With

                dt2.Clear()


            End If

            dt1.Clear()


            If LockSTS = True Then
                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray

                cbo_EndsCount.Enabled = False
                cbo_EndsCount.BackColor = Color.LightGray

            End If

            Grid_Cell_DeSelect()
            dgv_ActCtrlName = ""
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try


    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub JobWork_PavuYarn_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Cloth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub JobWork_PavuYarn_Receipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'JOBWORKER' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt2)
        cbo_Cloth.DataSource = dt2
        cbo_Cloth.DisplayMember = "Cloth_Name"

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

        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("")
        'cbo_WidthType.Items.Add("SINGLE")
        'cbo_WidthType.Items.Add("DOUBLE")
        'cbo_WidthType.Items.Add("TRIPLE")
        'cbo_WidthType.Items.Add("FOURTH")

        cbo_WidthType.Items.Add("SINGLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("SINGLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("DOUBLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("DOUBLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("TRIPLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("TRIPLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("FOUR FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("FOUR FABRIC FROM 2 BEAMS")


        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False

        btn_BarcodePrint.Visible = False

        lbl_DelvTo.Visible = False
        cbo_DeliveryTo.Visible = False

        dtp_Date.Text = ""
        txt_PartyDcNo.Text = ""
        cbo_Ledger.Text = ""
        cbo_Ledger.Tag = ""
        cbo_EndsCount.Text = ""

        cbo_EndsCount.Text = ""
        cbo_Cloth.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        cbo_WidthType.Visible = False
        lbl_Widthtype_Caption.Visible = False
        If Common_Procedures.settings.JobWorker_PavuWidthWiseConsumption_IN_Delivery = 1 Then
            cbo_WidthType.Visible = True
            lbl_Widthtype_Caption.Visible = True
        End If

        txt_CrimpPerc.Visible = False
        lbl_CrimpPerc_Caption.Visible = False
        If Common_Procedures.settings.AutoLoom_Pavu_CrimpMeters_Consumption_Stock_Posting_In_Delivery_Receipt_Entry = 1 Then
            txt_CrimpPerc.Visible = True
            lbl_CrimpPerc_Caption.Visible = True
        End If

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        dgv_PavuDetails.Columns(5).Visible = False
        dgv_PavuDetails.Columns(6).Visible = False
        dgv_YarnDetails.Columns(7).Visible = False
        dgv_YarnDetails.Columns(8).Visible = False

        If Common_Procedures.settings.CustomerCode = "1186" Then
            Label1.Text = "WEFT / WARP INWARD ENTRY"
            dgv_PavuDetails.Columns(5).Visible = True
            dgv_PavuDetails.Columns(4).Visible = False
            dgv_PavuDetails.Columns(1).Width = 70
            dgv_PavuDetails.Columns(2).Width = 70
            dgv_PavuDetails.Columns(3).Width = 70
            dgv_PavuDetails.Columns(5).Width = 100
        End If



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then '---- SRI SRINIVASA TEXTILES (PALLADAM)

            dgv_PavuDetails.Columns(5).Visible = True
            dgv_PavuDetails.Columns(6).Visible = True

            dgv_YarnDetails.Columns(7).Visible = True
            dgv_YarnDetails.Columns(8).Visible = True

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            btn_BarcodePrint.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1007" Then

            lbl_DelvTo.Visible = True
            cbo_DeliveryTo.Visible = True

        Else
            lbl_DelvTo.Visible = False
            cbo_DeliveryTo.Visible = False

            lbl_remarks.Left = lbl_VehicleNo.Left
            txt_remarks.Left = cbo_VehicleNo.Left
            txt_remarks.Width = 889
        End If


        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CrimpPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_InvoicePrefixNo.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CrimpPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_CrimpPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CrimpPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvoicePrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvoicePrefixNo.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub JobWork_PavuYarn_Receipt_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub JobWork_PavuYarn_Receipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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
        Dim i As Integer
        Dim vLASTCOLNO As Integer = 0

        If ActiveControl.Name = dgv_PavuDetails.Name Or ActiveControl.Name = dgv_YarnDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_PavuDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_ActCtrlName = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails


            ElseIf ActiveControl.Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_YarnDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_ActCtrlName = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails
            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_YarnDetails
            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1

                If dgv1.Name = dgv_PavuDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If dgv_YarnDetails.Rows.Count > 0 Then
                                    dgv_YarnDetails.Focus()
                                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                                    dgv_YarnDetails.CurrentCell.Selected = True
                                End If

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 3 Then

                            If .Columns(5).Visible = True Then
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)
                            End If



                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_PavuDetails.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                If dgv_YarnDetails.Rows.Count > 0 Then
                                    dgv_YarnDetails.Focus()
                                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                                    dgv_YarnDetails.CurrentCell.Selected = True
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If
                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                cbo_VehicleNo.Focus()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 5 Then
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)

                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If


                ElseIf dgv1.Name = dgv_YarnDetails.Name Then

                    If dgv1.Columns(8).Visible = True Then
                        vLASTCOLNO = 8
                    ElseIf dgv1.Columns(7).Visible = True Then
                        vLASTCOLNO = 7
                    Else
                        vLASTCOLNO = 6
                    End If

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= vLASTCOLNO Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                '    save_record()
                                'Else
                                '    msk_Date.Focus()
                                'End If

                                If cbo_DeliveryTo.Visible And cbo_DeliveryTo.Enabled = True Then
                                    cbo_DeliveryTo.Focus()
                                Else
                                    txt_remarks.Focus()
                                End If

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


                            End If
                        ElseIf .CurrentCell.ColumnIndex = 4 Then
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(6)

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_YarnDetails.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next
                                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                '    save_record()
                                'Else
                                '    dtp_Date.Focus()
                                'End If
                                If cbo_DeliveryTo.Visible And cbo_DeliveryTo.Enabled = True Then
                                    cbo_DeliveryTo.Focus()
                                Else
                                    txt_remarks.Focus()
                                End If


                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                dgv_PavuDetails.Focus()
                                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)


                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(vLASTCOLNO)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    End If

            End With

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)
        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_PavuYarn_Receipt, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_PavuYarn_Receipt, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Jobwork_Pavu_Yarn_Receipt_Entry, New_Entry, Me, con, "JobWork_PavuYarn_Receipt_Head", "JobWork_PavuYarn_Receipt_Code", NewCode, "JobWork_PavuYarn_Receipt_Date", "(JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub



        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and ( Pavu_Delivery_Code <> '' or Pavu_Delivery_Increment <> 0 or Beam_Knotting_Code <> '' or Loom_Idno <> 0 or Production_Meters <> 0 or Close_Status <> 0)", con)
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

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "JobWork_PavuYarn_Receipt_Head", "JobWork_PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "JobWork_PavuYarn_Receipt_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "JobWork_Yarn_Receipt_Details", "JobWork_PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "count_idno, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ", "Sl_No", "JobWork_PavuYarn_Receipt_Code, For_OrderBy, Company_IdNo, JobWork_PavuYarn_Receipt_No, JobWork_PavuYarn_Receipt_Date, Ledger_Idno", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "JobWork_Pavu_Receipt_Details", "JobWork_PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Set_No, Beam_No, Meters", "Sl_No", "JobWork_PavuYarn_Receipt_Code, For_OrderBy, Company_IdNo, JobWork_PavuYarn_Receipt_No, JobWork_PavuYarn_Receipt_Date, Ledger_Idno", trans)

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Loom_Idno = 0 and Production_Meters = 0 and Close_Status = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_Pavu_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_Yarn_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select cloth_name from cloth_head order by cloth_name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "cloth_name"

            da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
            da.Fill(dt3)
            cbo_Filter_EndsCount.DataSource = dt3
            cbo_Filter_EndsCount.DisplayMember = "EndsCount_Name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_EndsCount.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_EndsCount.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_PavuYarn_Receipt_RefNo from JobWork_PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_PavuYarn_Receipt_RefNo", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_PavuYarn_Receipt_RefNo from JobWork_PavuYarn_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_PavuYarn_Receipt_RefNo", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_PavuYarn_Receipt_RefNo from JobWork_PavuYarn_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_PavuYarn_Receipt_RefNo desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_PavuYarn_Receipt_RefNo from JobWork_PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_PavuYarn_Receipt_RefNo desc", con)
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

            lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_PavuYarn_Receipt_Head", "JobWork_PavuYarn_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RecNo.ForeColor = Color.Red


            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from JobWork_PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_PavuYarn_Receipt_RefNo desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("JobWork_PavuYarn_Receipt_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("JobWork_PavuYarn_Receipt_Date").ToString
                End If
                If IsDBNull(dt1.Rows(0).Item("Invoice_PrefixNo").ToString) = False Then
                    If dt1.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString
                End If

            End If
            dt1.Clear()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

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

            Da = New SqlClient.SqlDataAdapter("select JobWork_PavuYarn_Receipt_RefNo from JobWork_PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code = '" & Trim(RecCode) & "'", con)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_PavuYarn_Receipt, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_PavuYarn_Receipt, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Jobwork_Pavu_Yarn_Receipt_Entry, New_Entry, Me) = False Then Exit Sub




        Try

            inpno = InputBox("Enter New Rec No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select JobWork_PavuYarn_Receipt_RefNo from JobWork_PavuYarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Led_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotPvuBms As Single, vTotPvuMtrs As Single
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single
        Dim EntID As String = ""
        Dim Dup_SetNoBmNo As String = ""
        Dim pCnt_ID As Integer = 0
        Dim pEds_Nm As Integer = 0
        Dim vSetCd As String = ""
        Dim vSelc_SetCode As String = ""
        Dim Nr As Integer = 0
        Dim Trans_id As Integer = 0
        Dim vOrdByNo As String = ""
        Dim vCrmp_Mtrs As String = 0
        Dim vStkPvuMtrs As String = 0
        Dim vWdTyp As Single = 0
        Dim NoofBeams As Integer = 0
        Dim vInvoNo As String = ""
        Dim vDelvTo_IdNo As Integer = 0
        Dim vStkAt_IdNo As Integer = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Jobwork_Pavu_Yarn_Receipt_Entry, New_Entry, Me, con, "JobWork_PavuYarn_Receipt_Head", "JobWork_PavuYarn_Receipt_Code", NewCode, "JobWork_PavuYarn_Receipt_Date", "(JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, JobWork_PavuYarn_Receipt_RefNo desc", dtp_Date.Value.Date) = False Then Exit Sub


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)

        vDelvTo_IdNo = 0
        If cbo_DeliveryTo.Visible = True Then
            vDelvTo_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        End If

        If Val(vDelvTo_IdNo) <> 0 Then
            vStkAt_IdNo = Val(vDelvTo_IdNo)
        Else
            vStkAt_IdNo = Val(Led_ID)
        End If

        With dgv_PavuDetails

                For i = 0 To .RowCount - 1

                    If (Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(2).Value) <> "") Or Val(.Rows(i).Cells(3).Value) <> 0 Then

                        If Trim(.Rows(i).Cells(1).Value) = "" Then
                            MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .CurrentCell = .Rows(i).Cells(1)
                                .Focus()
                            End If
                            Exit Sub
                        End If

                        If Trim(.Rows(i).Cells(2).Value) = "" Then
                            MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .CurrentCell = .Rows(i).Cells(2)
                                .Focus()
                            End If
                            Exit Sub
                        End If

                        If Val(.Rows(i).Cells(3).Value) = 0 Then
                            MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                                .CurrentCell = .Rows(i).Cells(3)
                                .Focus()
                            End If
                            Exit Sub
                        End If

                        If InStr(1, Trim(UCase(Dup_SetNoBmNo)), "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~" & Trim(UCase(.Rows(i).Cells(2).Value)) & "~") > 0 Then
                            MessageBox.Show("Duplicate BeamNo ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(1)
                                .CurrentCell.Selected = True
                            End If
                            Exit Sub
                        End If
                        Dup_SetNoBmNo = Trim(Dup_SetNoBmNo) & "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~" & Trim(UCase(.Rows(i).Cells(2).Value)) & "~"

                    End If

                Next

            End With

            vTotPvuBms = 0 : vTotPvuMtrs = 0
        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTotPvuBms = Val(dgv_PavuDetails_Total.Rows(0).Cells(2).Value())
            vTotPvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(3).Value())
        End If

        If Common_Procedures.settings.JobWorker_PavuWidthWiseConsumption_IN_Delivery = 1 Then

            If vTotPvuMtrs <> 0 Then

                If Clo_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
                    Exit Sub
                End If

                If cbo_WidthType.Visible And cbo_WidthType.Text = "" Then
                    MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_WidthType.Enabled And cbo_WidthType.Visible Then cbo_WidthType.Focus()
                    Exit Sub
                End If

            End If

        End If


        If Val(Clo_ID) <> 0 And Val(EdsCnt_ID) <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Cloth_EndsCount_Details Where Cloth_Idno = " & Str(Val(Clo_ID)) & " and EndsCount_IdNo = " & Str(Val(EdsCnt_ID)), con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count = 0 Then
                MessageBox.Show("EndsCount mismatches with ClothMaster", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Cloth.Enabled Then cbo_Cloth.Focus()
                Exit Sub
            End If
            Dt1.Clear()

        End If


        vInvoNo = Trim(txt_InvoicePrefixNo.Text) & Trim(lbl_RecNo.Text)

        If EdsCnt_ID = 0 And vTotPvuMtrs <> 0 Then
            MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
            Exit Sub
        End If

        Trans_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)


        With dgv_YarnDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Then

                    YCnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Val(YCnt_ID) = 0 Then
                        MessageBox.Show("Invalid CountName", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .CurrentCell = .Rows(0).Cells(1)
                        .Focus()
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .CurrentCell = .Rows(0).Cells(2)
                        .Focus()
                        Exit Sub
                    End If

                    YMil_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(3).Value)
                    If Val(YMil_ID) = 0 Then
                        MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .CurrentCell = .Rows(0).Cells(3)
                        .Focus()
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(6).Value) = 0 Then
                        MessageBox.Show("Invalid Bags", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .CurrentCell = .Rows(0).Cells(6)
                        .Focus()
                        Exit Sub
                    End If

                End If

            Next

        End With

        vTotYrnBags = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0
        If dgv_YarnDetails_Total.RowCount > 0 Then
            vTotYrnBags = Val(dgv_YarnDetails_Total.Rows(0).Cells(4).Value())
            vTotYrnCones = Val(dgv_YarnDetails_Total.Rows(0).Cells(5).Value())
            vTotYrnWeight = Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_PavuYarn_Receipt_Head", "JobWork_PavuYarn_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into JobWork_PavuYarn_Receipt_Head(JobWork_PavuYarn_Receipt_Code, Company_IdNo, JobWork_PavuYarn_Receipt_No,JobWork_PavuYarn_Receipt_RefNo,Invoice_PrefixNo, for_OrderBy, JobWork_PavuYarn_Receipt_Date, Ledger_IdNo, Party_DcNo, Cloth_IdNo, EndsCount_IdNo, Total_Beam, Total_Meters, Total_Bags, Total_Cones, Total_Weight, Transport_IdNo  ,  Vehicle_No, Width_Type, Crimp_Percentage,Remarks , DeliveryTo_IdNo) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "',   '" & Trim(lbl_RecNo.Text) & "','" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @EntryDate, " & Str(Val(Led_ID)) & ", '" & Trim(txt_PartyDcNo.Text) & "', " & Str(Val(Clo_ID)) & ", " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(vTotPvuBms)) & ", " & Str(Val(vTotPvuMtrs)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & "," & Str(Val(Trans_id)) & ", '" & Trim(cbo_VehicleNo.Text) & "', '" & Trim(cbo_WidthType.Text) & "', " & Str(Val(txt_CrimpPerc.Text)) & " , '" & Trim(txt_remarks.Text) & "' , " & Str(Val(vDelvTo_IdNo)) & ")"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "JobWork_PavuYarn_Receipt_Head", "JobWork_PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "JobWork_PavuYarn_Receipt_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "JobWork_Yarn_Receipt_Details", "JobWork_PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "count_idno, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ", "Sl_No", "JobWork_PavuYarn_Receipt_Code, For_OrderBy, Company_IdNo, JobWork_PavuYarn_Receipt_No, JobWork_PavuYarn_Receipt_Date, Ledger_Idno", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "JobWork_Pavu_Receipt_Details", "JobWork_PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No, Beam_No, Meters", "Sl_No", "JobWork_PavuYarn_Receipt_Code, For_OrderBy, Company_IdNo, JobWork_PavuYarn_Receipt_No, JobWork_PavuYarn_Receipt_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update JobWork_PavuYarn_Receipt_Head set  Invoice_PrefixNo = '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' ,  JobWork_PavuYarn_Receipt_RefNo =   '" & Trim(lbl_RecNo.Text) & "',JobWork_PavuYarn_Receipt_No='" & Trim(vInvoNo) & "' ,JobWork_PavuYarn_Receipt_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Party_DcNo = '" & Trim(txt_PartyDcNo.Text) & "', Cloth_IdNo = " & Str(Val(Clo_ID)) & ", EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Total_Beam = " & Str(Val(vTotPvuBms)) & ", Total_Meters = " & Str(Val(vTotPvuMtrs)) & ", Total_Bags = " & Str(Val(vTotYrnBags)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & ",Transport_IdNo = " & Str(Val(Trans_id)) & ", Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' , Width_Type = '" & Trim(cbo_WidthType.Text) & "', Crimp_Percentage = " & Str(Val(txt_CrimpPerc.Text)) & ",Remarks= '" & Trim(txt_remarks.Text) & "' , DeliveryTo_IdNo = " & Str(Val(vDelvTo_IdNo)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "JobWork_PavuYarn_Receipt_Head", "JobWork_PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "JobWork_PavuYarn_Receipt_Code, Company_IdNo, for_OrderBy", tr)

            EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)
            If Trim(txt_PartyDcNo.Text) <> "" Then
                Partcls = "Rcpt : P.Dc.No. " & Trim(txt_PartyDcNo.Text)
                PBlNo = Trim(txt_PartyDcNo.Text)
            Else
                Partcls = "Rcpt : Rec.No. " & Trim(vInvoNo)
                PBlNo = Trim(vInvoNo)
            End If

            cmd.CommandText = "Delete from JobWork_Pavu_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from JobWork_Yarn_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Loom_Idno = 0 and Production_Meters = 0 and Close_Status = 0"
            cmd.ExecuteNonQuery()

            pCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ")", , tr))
            pEds_Nm = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Ends_Name", "(EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ")", , tr))

            With dgv_PavuDetails
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Then

                        Sno = Sno + 1

                        vSetCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(.Rows(i).Cells(1).Value) & "/" & Trim(Common_Procedures.FnYearCode)
                        vSelc_SetCode = Trim(.Rows(i).Cells(1).Value) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

                        cmd.CommandText = "Insert into JobWork_Pavu_Receipt_Details ( JobWork_PavuYarn_Receipt_Code, Company_IdNo,JobWork_PavuYarn_Receipt_REFNo, JobWork_PavuYarn_Receipt_No, for_OrderBy, JobWork_PavuYarn_Receipt_Date, Ledger_IdNo, EndsCount_IdNo, Sl_No, Set_Code, SetCode_ForSelection, Set_No, Beam_No, Meters,Po_no,Warp_Lot_No) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @EntryDate, " & Str(Val(Led_ID)) & ", " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(vSetCd) & "', '" & Trim(vSelc_SetCode) & "', '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(.Rows(i).Cells(3).Value)) & ", '" & Trim(.Rows(i).Cells(5).Value) & "','" & Trim(.Rows(i).Cells(6).Value) & "'  )"
                        cmd.ExecuteNonQuery()

                        Nr = 0
                        cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Reference_Date = @EntryDate, Sl_No = " & Str(Val(Sno)) & " " &
                                                    " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then

                            cmd.CommandText = "Insert into Stock_SizedPavu_Processing_Details ( Reference_Code,              Company_IdNo        ,          Reference_No         ,                               for_OrderBy                              , Reference_Date,           Ledger_IdNo   ,         StockAt_IdNo    ,            Set_Code   ,                         Set_No               ,      setcode_forSelection    ,         Ends_Name      ,           count_idno     ,           EndsCount_IdNo   , Mill_IdNo, Beam_Width_Idno, Sizing_SlNo,            Sl_No     ,                    Beam_No             ,                               ForOrderBy_BeamNo                                 , Gross_Weight, Tare_Weight, Net_Weight, Noof_Pcs, Meters_Pc,                      Meters              , Pavu_Delivery_Code, Pavu_Delivery_Increment, Beam_Knotting_Code, Loom_Idno ) " &
                                                    "   Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",   @EntryDate  , " & Str(Val(Led_ID)) & ", " & Str(Val(vStkAt_IdNo)) & ", '" & Trim(vSetCd) & "', '" & Trim(Trim(.Rows(i).Cells(1).Value)) & "', '" & Trim(vSelc_SetCode) & "', '" & Trim(pEds_Nm) & "', " & Str(Val(pCnt_ID)) & ", " & Str(Val(EdsCnt_ID)) & ",     0    ,       0        ,      0     , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(2).Value))) & ",       0     ,      0     ,      0    ,     0   ,     0    , " & Str(Val(.Rows(i).Cells(3).Value)) & ",      ''           ,           0            ,        ''         ,      0    ) "
                            cmd.ExecuteNonQuery()

                            '*******Cmd By Lalith 2025_06_21

                            'cmd.CommandText = "Insert into Stock_SizedPavu_Processing_Details ( Reference_Code,              Company_IdNo        ,          Reference_No         ,                               for_OrderBy                              , Reference_Date,           Ledger_IdNo   ,         StockAt_IdNo    ,            Set_Code   ,                         Set_No               ,      setcode_forSelection    ,         Ends_Name      ,           count_idno     ,           EndsCount_IdNo   , Mill_IdNo, Beam_Width_Idno, Sizing_SlNo,            Sl_No     ,                    Beam_No             ,                               ForOrderBy_BeamNo                                 , Gross_Weight, Tare_Weight, Net_Weight, Noof_Pcs, Meters_Pc,                      Meters              , Pavu_Delivery_Code, Pavu_Delivery_Increment, Beam_Knotting_Code, Loom_Idno ) " &
                            '                        "   Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",   @EntryDate  , " & Str(Val(Led_ID)) & ", " & Str(Val(Led_ID)) & ", '" & Trim(vSetCd) & "', '" & Trim(Trim(.Rows(i).Cells(1).Value)) & "', '" & Trim(vSelc_SetCode) & "', '" & Trim(pEds_Nm) & "', " & Str(Val(pCnt_ID)) & ", " & Str(Val(EdsCnt_ID)) & ",     0    ,       0        ,      0     , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(2).Value))) & ",       0     ,      0     ,      0    ,     0   ,     0    , " & Str(Val(.Rows(i).Cells(3).Value)) & ",      ''           ,           0            ,        ''         ,      0    ) "
                            'cmd.ExecuteNonQuery()

                            '*******Cmd By Lalith 2025_06_21

                        End If

                    End If

                Next

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "JobWork_Pavu_Receipt_Details", "JobWork_PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No, Beam_No, Meters", "Sl_No", "JobWork_PavuYarn_Receipt_Code, For_OrderBy, Company_IdNo, JobWork_PavuYarn_Receipt_No, JobWork_PavuYarn_Receipt_Date, Ledger_Idno", tr)

            End With


            If Val(vTotPvuMtrs) <> 0 Then

                If Common_Procedures.settings.JobWorker_PavuWidthWiseConsumption_IN_Delivery = 1 Then

                    NoofBeams = 2
                    If Trim(cbo_WidthType.Text) <> "" Then
                        If InStr(1, Trim(UCase(cbo_WidthType.Text)), "1 BEAM") > 0 Then
                            NoofBeams = 1
                        ElseIf InStr(1, Trim(UCase(cbo_WidthType.Text)), "2 BEAM") > 0 Then
                            NoofBeams = 2
                        End If
                    End If

                    vWdTyp = 0
                    If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "FOURTH") > 0 Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "FOUR") > 0 Then
                        vWdTyp = 4
                    ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "TRIPLE") > 0 Then
                        vWdTyp = 3
                    ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "DOUBLE") > 0 Then
                        vWdTyp = 2
                    ElseIf Trim(UCase(cbo_WidthType.Text)) = "SINGLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "SINGLE") > 0 Then
                        vWdTyp = 1
                    End If

                    vStkPvuMtrs = Format(vTotPvuMtrs / NoofBeams * vWdTyp, "###########0.00")

                    'If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
                    '    vWdTyp = 2
                    'ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Then
                    '    vWdTyp = 1.5
                    'ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Then
                    '    vWdTyp = 1
                    'Else
                    '    vWdTyp = 0.5
                    'End If
                    'vStkPvuMtrs = Format(vTotPvuMtrs * vWdTyp, "###########0.00")

                    vCrmp_Mtrs = 0
                    If Common_Procedures.settings.AutoLoom_Pavu_CrimpMeters_Consumption_Stock_Posting_In_Delivery_Receipt_Entry = 1 Then
                        vCrmp_Mtrs = Format(Val(vStkPvuMtrs) * Val(txt_CrimpPerc.Text) / 100, "###########0.00")
                    End If

                    vStkPvuMtrs = Format(Val(vStkPvuMtrs) - Val(vCrmp_Mtrs), "###########0.00")

                Else

                    vStkPvuMtrs = vTotPvuMtrs

                End If


                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date, DeliveryTo_Idno                  ,     ReceivedFrom_Idno   ,         Cloth_Idno      ,       Entry_ID       ,      Party_Bill_No   ,      Particulars       ,            Sl_No     ,         EndsCount_IdNo     ,            Sized_Beam       ,               Meters          ) " &
                                        " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",   @EntryDate  ,        " & Str(Val(vDelvTo_IdNo)) & "       , " & Str(Val(Led_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(vTotPvuBms)) & ", " & Str(Val(vStkPvuMtrs)) & " ) "
                cmd.ExecuteNonQuery()


            End If


            With dgv_YarnDetails
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(6).Value) <> 0 Then

                        Sno = Sno + 1

                        YCnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        YMil_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        cmd.CommandText = "Insert into JobWork_Yarn_Receipt_Details(JobWork_PavuYarn_Receipt_Code, Company_IdNo, JobWork_PavuYarn_Receipt_REFNo, JobWork_PavuYarn_Receipt_No, for_OrderBy, JobWork_PavuYarn_Receipt_Date, Ledger_IdNo, Sl_No, count_idno, Yarn_Type, Mill_IdNo, Bags, Cones, Weight , Po_No , Weft_Lot_No ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @EntryDate, " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " , '" & Trim(.Rows(i).Cells(7).Value) & "', '" & Trim(.Rows(i).Cells(8).Value) & "' )"
                        cmd.ExecuteNonQuery()



                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @EntryDate, " & Str(Val(vDelvTo_IdNo)) & ", " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " )"
                        cmd.ExecuteNonQuery()



                    End If

                Next

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "JobWork_Yarn_Receipt_Details", "JobWork_PavuYarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "count_idno, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ", "Sl_No", "JobWork_PavuYarn_Receipt_Code, For_OrderBy, Company_IdNo, JobWork_PavuYarn_Receipt_No, JobWork_PavuYarn_Receipt_Date, Ledger_Idno", tr)

            End With

            If Val(vTotPvuBms) <> 0 Or Val(vTotYrnBags) <> 0 Or Val(vTotYrnCones) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", '" & Trim(PBlNo) & "', 1, 0, " & Str(Val(vTotPvuBms)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RecNo.Text)
                End If

            Else
                move_record(lbl_RecNo.Text)

            End If

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("IX_Stock_SizedPavu_Processing_Details_2"))) > 0 Then
                MessageBox.Show("Duplicate SetNo/BeamNo", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("IX_Stock_SizedPavu_Processing_Details_1"))) > 0 Then
                MessageBox.Show("Duplicate SetNo/BeamNo", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("PK_SizedPavu_Processing_Details"))) > 0 Then
                MessageBox.Show("Duplicate SetNo/BeamNo", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            e.Handled = True
            txt_remarks.Focus()
            'dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(0)
            'dgv_YarnDetails.CurrentCell.Selected = True
            'dgv_YarnDetails.Focus()

            '' SendKeys.Send("+{TAB}")
        End If

    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cbo_Ledger_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'JOBWORKER' and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub


    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, msk_Date, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'JOBWORKER' and Close_status = 0 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'JOBWORKER' and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress_111(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Ledger

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText

                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If

                                End If
                            End If
                        End If

                        txt_PartyDcNo.Focus()

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

                        Condt = "where (ledger_idno = 0 or Ledger_Type = 'JOBWORKER')"
                        If Trim(FindStr) <> "" Then
                            Condt = " where (Ledger_Type = 'JOBWORKER') and (Ledger_DisplayName like '" & FindStr & "%' or Ledger_DisplayName like '% " & FindStr & "%')"
                        End If

                        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead  " & Condt & " Order by Ledger_DisplayName", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Ledger_DisplayName"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim vCONDT As String = ""
        Dim EdsCnt_ID As Integer
        Dim Nr As Integer

        vCONDT = ""
        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)

        If Trim(cbo_Cloth.Text) = "" Then

            Nr = 0
            da1 = New SqlClient.SqlDataAdapter("select COUNT(*) from Cloth_EndsCount_Details Where EndsCount_IdNo = " & Str(Val(EdsCnt_ID)), con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    Nr = Val(dt1.Rows(0)(0).ToString)
                End If
            End If
            dt1.Clear()

            If Nr = 1 Then
                da1 = New SqlClient.SqlDataAdapter("select b.cloth_name from Cloth_EndsCount_Details a, cloth_head b Where a.EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & " and a.cloth_idno = b.cloth_idno", con)
                dt1 = New DataTable
                da1.Fill(dt1)
                If dt1.Rows.Count > 0 Then
                    cbo_Cloth.Text = dt1.Rows(0).Item("cloth_name").ToString
                End If
                dt1.Clear()
            End If

        End If

        vCONDT = "( cloth_idno IN ( select sq1.cloth_idno from Cloth_EndsCount_Details sq1 Where sq1.EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ") )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "cloth_name", "(Close_Status=0 and " & vCONDT & ")", "(cloth_idno=0)")
        With cbo_Cloth
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Try

            Dim vCONDT As String = ""
            Dim EdsCnt_ID As Integer

            vCONDT = ""
            EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)

            vCONDT = "( cloth_idno IN ( select sq1.cloth_idno from Cloth_EndsCount_Details sq1 Where sq1.EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ") )"

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_EndsCount, Nothing, "Cloth_Head", "cloth_name", "(Close_Status=0 and " & vCONDT & ")", "(cloth_idno=0)")
            With cbo_Cloth
                If e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                        cbo_WidthType.Focus()
                    Else
                        cbo_Transport.Focus()
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES Not Select...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Dim vCONDT As String = ""
        Dim EdsCnt_ID As Integer

        vCONDT = ""
        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)

        vCONDT = "( cloth_idno IN ( select sq1.cloth_idno from Cloth_EndsCount_Details sq1 Where sq1.EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ") )"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Cloth_Head", "cloth_name", "(Close_Status=0 and " & vCONDT & ")", "(cloth_idno=0)")
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                cbo_WidthType.Focus()
            Else
                cbo_Transport.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Cloth_KeyPress_111(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Cloth

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        If cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                            cbo_WidthType.Focus()
                        Else
                            cbo_EndsCount.Focus()
                        End If

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

                        Condt = ""
                        If Trim(FindStr) <> "" Then
                            Condt = " Where (Cloth_Name Like '" & FindStr & "%' or Cloth_Name like '% " & FindStr & "%') "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head " & Condt & " order by Cloth_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Cloth_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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

    Private Sub cbo_Cloth_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.LostFocus
        With cbo_Cloth
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub


    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "(Close_Status=0)", "(EndsCount_idno = 0)")
        With cbo_EndsCount
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_PartyDcNo, Nothing, "EndsCount_Head", "EndsCount_Name", "(Close_Status=0)", "(EndsCount_idno = 0)")
            With cbo_EndsCount
                If e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    If cbo_Cloth.Visible And cbo_Cloth.Enabled Then
                        cbo_Cloth.Focus()
                    ElseIf cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                        cbo_WidthType.Focus()
                    ElseIf cbo_Transport.Visible And cbo_Transport.Enabled Then
                        cbo_Transport.Focus()
                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "EndsCount_Head", "EndsCount_Name", "(Close_Status=0)", "(EndsCount_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Cloth.Visible And cbo_Cloth.Enabled Then
                cbo_Cloth.Focus()
            ElseIf cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                cbo_WidthType.Focus()
            ElseIf cbo_Transport.Visible And cbo_Transport.Enabled Then
                cbo_Transport.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_EndsCount_KeyPress_111(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_EndsCount

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        cbo_Transport.Focus()

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

                        Condt = ""
                        If Trim(FindStr) <> "" Then
                            Condt = " Where (EndsCount_Name like '" & FindStr & "%' or EndsCount_Name like '% " & FindStr & "%') "
                        End If

                        da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head " & Condt & " order by EndsCount_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "EndsCount_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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

    Private Sub cbo_EndsCount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.LostFocus
        With cbo_EndsCount
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
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
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""
        Dim EdsCnt_IdNo As Integer, Mil_IdNo As Integer

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0
            EdsCnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobWork_PavuYarn_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.JobWork_PavuYarn_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobWork_PavuYarn_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                EdsCnt_IdNo = Common_Procedures.EndsCount_NameToIdNo(con, cbo_Filter_EndsCount.Text)
            End If



            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.JobWork_PavuYarn_Receipt_Code IN (select z1.JobWork_PavuYarn_Receipt_Code from JobWork_Yarn_Receipt_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.JobWork_PavuYarn_Receipt_Code IN (select z2.JobWork_PavuYarn_Receipt_Code from JobWork_Yarn_Receipt_Details z2 where z2.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ")"
            End If

            If Val(EdsCnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from JobWork_PavuYarn_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_PavuYarn_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.JobWork_PavuYarn_Receipt_RefNo", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("JobWork_PavuYarn_Receipt_RefNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("JobWork_PavuYarn_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
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
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        With cbo_Filter_PartyName
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            With cbo_Filter_PartyName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    dtp_Filter_ToDate.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Filter_CountName.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        With cbo_Filter_PartyName

            If Asc(e.KeyChar) = 13 Then

                If Trim(.Text) <> "" Then
                    If .DroppedDown = True Then
                        If Trim(.SelectedText) <> "" Then
                            .Text = .SelectedText
                        Else
                            If .Items.Count > 0 Then
                                .SelectedIndex = 0
                                .SelectedItem = .Items(0)
                                .Text = .GetItemText(.SelectedItem)
                            End If
                        End If
                    End If
                End If

                cbo_Filter_CountName.Focus()

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

                Condt = " Where (Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER')"
                If Trim(FindStr) <> "" Then
                    Condt = " Where (Ledger_Type = 'JOBWORKER') and (Ledger_DisplayName like '" & FindStr & "%' or Ledger_DisplayName like '% " & FindStr & "%') "
                End If

                da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead " & Condt & " order by Ledger_DisplayName", con)
                da.Fill(dt)

                .DataSource = dt
                .DisplayMember = "Ledger_DisplayName"

                .Text = Trim(FindStr)

                .SelectionStart = FindStr.Length

                e.Handled = True

            End If

        End With

    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        With cbo_Filter_CountName
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Filter_CountName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.LostFocus
        With cbo_Filter_CountName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Try
            With cbo_Filter_CountName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Filter_PartyName.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Filter_MillName.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        With cbo_Filter_CountName

            If Asc(e.KeyChar) = 13 Then

                If Trim(.Text) <> "" Then
                    If .DroppedDown = True Then
                        If Trim(.SelectedText) <> "" Then
                            .Text = .SelectedText
                        Else
                            If .Items.Count > 0 Then
                                .SelectedIndex = 0
                                .SelectedItem = .Items(0)
                                .Text = .GetItemText(.SelectedItem)
                            End If
                        End If
                    End If
                End If

                cbo_Filter_MillName.Focus()

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

                If Trim(FindStr) <> "" Then
                    Condt = " Where count_name like '" & Trim(FindStr) & "%' or count_name like '% " & Trim(FindStr) & "%' "
                End If

                da = New SqlClient.SqlDataAdapter("select count_name from Count_Head " & Condt & " order by count_name", con)
                da.Fill(dt)

                .DataSource = dt
                .DisplayMember = "count_name"

                .Text = Trim(FindStr)

                .SelectionStart = FindStr.Length

                e.Handled = True

            End If

        End With

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String = 0

        On Error Resume Next

        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)
        End If

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


    Private Sub cbo_Filter_PartyName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.LostFocus
        With cbo_Filter_PartyName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub


    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        TotalPavu_Calculation()
        'SendKeys.Send("{up}")
        'SendKeys.Send("{Tab}")
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter
        With dgv_PavuDetails
            dgv_ActCtrlName = .Name
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave
        With dgv_PavuDetails
            If .CurrentCell.ColumnIndex = 3 Then
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
                If .CurrentCell.ColumnIndex = 3 Then
                    TotalPavu_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_PavuDetails = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub


    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

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
                If Val(.Rows(i).Cells(3).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(3).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotBms)
            .Rows(0).Cells(3).Value = Format(Val(TotMtrs), "########0.00")
        End With

    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEndEdit
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
            dgv_ActCtrlName = .Name
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
        If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
        With dgv_YarnDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

                    If dgv_YarnDetails.CurrentRow.Cells(2).Value = "MILL" Then
                        If dgv_YarnDetails.CurrentCell.ColumnIndex = 4 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 5 Then
                            get_MillCount_Details()
                        End If
                    End If

                    TotalYarnTaken_Calculation()

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
        End With
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



    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown

        'Try
        With cbo_Grid_CountName
            If e.KeyValue = 38 And .DroppedDown = False Then
                e.Handled = True

                With dgv_YarnDetails
                    If Val(.CurrentCell.RowIndex) <= 0 Then
                        dgv_PavuDetails.Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                        dgv_PavuDetails.CurrentCell.Selected = True


                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)
                        .CurrentCell.Selected = True


                    End If
                End With
                .Visible = False
                .Tag = -1
                .Text = ""

                'SendKeys.Send("+{TAB}")
            ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                e.Handled = True
                With dgv_YarnDetails
                    If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                        'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        '    save_record()
                        'Else
                        '    msk_Date.Focus()
                        '    Exit Sub
                        'End If

                        If cbo_DeliveryTo.Visible And cbo_DeliveryTo.Enabled = True Then
                            cbo_DeliveryTo.Focus()
                        Else
                            txt_remarks.Focus()
                        End If


                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If
                End With
                .Visible = False
                .Tag = -1
                .Text = ""

                'SendKeys.Send("{TAB}")
            ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                .DroppedDown = True

            End If
        End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try
    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        'Try

        With cbo_Grid_CountName

            If Asc(e.KeyChar) <> 27 Then

                If Asc(e.KeyChar) = 13 Then

                    If Trim(.Text) <> "" Then
                        If .DroppedDown = True Then
                            If Trim(.SelectedText) <> "" Then
                                .Text = .SelectedText
                            Else
                                If .Items.Count > 0 Then
                                    .SelectedIndex = 0
                                    .SelectedItem = .Items(0)
                                    .Text = .GetItemText(.SelectedItem)
                                End If
                            End If
                        End If
                    End If

                    With dgv_YarnDetails

                        .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_CountName.Text)
                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            '    save_record()
                            'Else
                            '    msk_Date.Focus()
                            '    Exit Sub
                            'End If
                            If cbo_DeliveryTo.Visible And cbo_DeliveryTo.Enabled = True Then
                                cbo_DeliveryTo.Focus()
                            Else
                                txt_remarks.Focus()
                            End If


                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            '.CurrentCell.Selected = True

                        End If
                    End With
                    .Visible = False
                    .Tag = -1
                    .Text = ""

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

                    If Trim(FindStr) <> "" Then
                        Condt = " Where count_name like '" & Trim(FindStr) & "%' or count_name like '% " & Trim(FindStr) & "%' "
                    End If

                    da = New SqlClient.SqlDataAdapter("select count_name from Count_Head " & Condt & " order by count_name", con)
                    da.Fill(dt)

                    .DataSource = dt
                    .DisplayMember = "count_name"

                    .Text = FindStr

                    .SelectionStart = FindStr.Length

                    e.Handled = True

                End If

            End If

        End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

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

    Private Sub cbo_Grid_CountName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.LostFocus

        With cbo_Grid_CountName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With

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
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus

        With cbo_Grid_MillName
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_Grid_MillName.Text.Length

        End With

    End Sub

    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Try
            With cbo_Grid_MillName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    End With
                    .Visible = False
                    .Tag = -1
                    .Text = ""

                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With
                    .Visible = False
                    .Tag = -1
                    .Text = ""

                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Grid_MillName

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        With dgv_YarnDetails
                            .Focus()
                            .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_MillName.Text)
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True
                        End With
                        .Visible = False
                        .Tag = -1
                        .Text = ""

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

                        Condt = ""
                        If Trim(FindStr) <> "" Then
                            Condt = " Where mill_name like '" & Trim(FindStr) & "%' or mill_name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select mill_name from mill_Head " & Condt & " order by mill_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "mill_name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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

    Private Sub cbo_Grid_MillName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.LostFocus

        With cbo_Grid_MillName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With

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
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.GotFocus

        With cbo_Grid_YarnType
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_Grid_YarnType.Text.Length
        End With

    End Sub

    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_YarnType.KeyDown
        Try
            With cbo_Grid_YarnType
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    End With
                    .Visible = False
                    .Tag = -1
                    .Text = ""

                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End With
                    .Visible = False
                    .Tag = -1
                    .Text = ""

                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_YarnType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Grid_YarnType

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        With dgv_YarnDetails
                            .Focus()
                            .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_YarnType.Text)
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        End With
                        .Visible = False
                        .Tag = -1
                        .Text = ""

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

                        Condt = ""
                        If Trim(FindStr) <> "" Then
                            Condt = " Where (Yarn_Type like '" & Trim(FindStr) & "%' or Yarn_Type like '% " & Trim(FindStr) & "%') "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head " & Condt & " order by Yarn_Type", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Yarn_Type"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_YarnType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.LostFocus

        With cbo_Grid_YarnType
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With

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
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_PartyDcNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_PartyDcNo.GotFocus
        With txt_PartyDcNo
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub txt_PartyDcNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PartyDcNo.KeyDown
        If e.KeyCode = 40 Then cbo_EndsCount.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then cbo_Ledger.Focus() 'SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_PartyDcNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PartyDcNo.KeyPress
        If Asc(e.KeyChar) = 13 Then cbo_EndsCount.Focus()
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

        With cbo_Filter_MillName
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_Filter_MillName.Text.Length

        End With

    End Sub

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        Try
            With cbo_Filter_MillName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    cbo_Filter_CountName.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Filter_EndsCount.Focus()

                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Filter_MillName

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        cbo_Filter_EndsCount.Focus()

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

                        Condt = ""
                        If Trim(FindStr) <> "" Then
                            Condt = " Where mill_name like '" & Trim(FindStr) & "%' or mill_name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select mill_name from mill_Head " & Condt & " order by mill_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "mill_name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Filter_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Filter_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Filter_MillName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.LostFocus

        With cbo_Filter_MillName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With

    End Sub

    Private Sub cbo_Filter_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsCount.GotFocus

        With cbo_Filter_EndsCount
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_Filter_EndsCount.Text.Length

        End With

    End Sub

    Private Sub cbo_Filter_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsCount.KeyDown
        Try
            With cbo_Filter_EndsCount
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Filter_MillName.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    btn_Filter_Show.Focus()

                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsCount.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Filter_EndsCount

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        btn_Filter_Show_Click(sender, e)

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

                        Condt = ""
                        If Trim(FindStr) <> "" Then
                            Condt = " Where EndsCount_Name like '" & Trim(FindStr) & "%' or EndsCount_Name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head " & Condt & " order by EndsCount_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "EndsCount_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Filter_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Filter_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Filter_EndsCount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsCount.LostFocus
        With cbo_Filter_EndsCount
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Jobwork_Pavu_Yarn_Receipt_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from JobWork_PavuYarn_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
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

        End If

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1


        'If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
        '    Try
        '        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
        '        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        '            PrintDocument1.Print()
        '        End If

        '    Catch ex As Exception
        '        MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '    End Try


        'Else
        '    Try

        '        Dim ppd As New PrintPreviewDialog

        '        ppd.Document = PrintDocument1

        '        ppd.WindowState = FormWindowState.Normal
        '        ppd.StartPosition = FormStartPosition.CenterScreen
        '        ppd.ClientSize = New Size(600, 600)

        '        ppd.ShowDialog()

        '    Catch ex As Exception
        '        MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

        '    End Try

        'End If


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                            'Debug.Print(ps.PaperName)
                            If ps.Width = 800 And ps.Height = 600 Then
                                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                PrintDocument1.DefaultPageSettings.PaperSize = ps
                                'e.PageSettings.PaperSize = ps
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
                                    'e.PageSettings.PaperSize = ps
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
                                        'e.PageSettings.PaperSize = ps
                                        Exit For
                                    End If
                                Next
                            End If

                        End If

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from JobWork_PavuYarn_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name from JobWork_Yarn_Receipt_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                vPrn_PvuEdsCnt = ""
                vPrn_PvuTotBms = 0
                vPrn_PvuTotMtrs = 0
                vPrn_PvuSetNo = "" : vDup_SetNo = ""
                vDup_BmNo = "" : vPvu_BmNo = ""
                vPrn_PvuBmNos1 = "" : vPrn_PvuBmNos2 = "" : vPrn_PvuBmNos3 = "" : vPrn_PvuBmNos4 = ""

                cmd.Connection = con

                cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                cmd.ExecuteNonQuery()

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name from JobWork_Pavu_Receipt_Details a INNER JOIN EndsCount_Head b on a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                Dt1 = New DataTable
                da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    vPrn_PvuEdsCnt = Dt1.Rows(0).Item("EndsCount_Name").ToString

                    For I = 0 To Dt1.Rows.Count - 1

                        vPrn_PvuTotBms = Val(vPrn_PvuTotBms) + 1
                        vPrn_PvuTotMtrs = vPrn_PvuTotMtrs + Val(Dt1.Rows(I).Item("Meters").ToString)

                        If InStr(1, "~" & Trim(UCase(vDup_SetNo)) & "~", "~" & Trim(UCase(Dt1.Rows(I).Item("Set_No").ToString)) & "~") = 0 Then
                            vDup_SetNo = Trim(vDup_SetNo) & "~" & Trim(Dt1.Rows(I).Item("Set_No").ToString) & "~"
                            vPrn_PvuSetNo = vPrn_PvuSetNo & IIf(Trim(vPrn_PvuSetNo) <> "", ", ", "") & Dt1.Rows(I).Item("Set_No").ToString
                        End If

                        If InStr(1, "~" & Trim(UCase(vDup_BmNo)) & "~", "~" & Trim(UCase(Dt1.Rows(I).Item("Set_No").ToString)) & "^" & Trim(UCase(Dt1.Rows(I).Item("Beam_No").ToString)) & "~") = 0 Then
                            vDup_BmNo = Trim(vDup_BmNo) & "~" & Trim(Dt1.Rows(I).Item("Set_No").ToString) & "^" & Trim(UCase(Dt1.Rows(I).Item("Beam_No").ToString)) & "~"

                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Meters1) values ('" & Trim(Dt1.Rows(I).Item("Beam_No").ToString) & "', " & Common_Procedures.OrderBy_CodeToValue(Trim(Dt1.Rows(I).Item("Beam_No").ToString)) & " )"
                            cmd.ExecuteNonQuery()

                        End If

                    Next I

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

                    For I = 1 To Dt1.Rows.Count - 1
                        If LsNo + 1 = Val(Dt1.Rows(I).Item("fororderby_beamno").ToString) Then
                            LsNo = Val(Dt1.Rows(I).Item("fororderby_beamno").ToString)
                            LsBeamNo = Trim(UCase(Dt1.Rows(I).Item("Beam_No").ToString))

                        Else
                            If FsNo = LsNo Then
                                vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & ","
                            Else
                                vPvu_BmNo = vPvu_BmNo & Trim(FsBeamNo) & "-" & Trim(LsBeamNo) & ","
                            End If
                            FsNo = Dt1.Rows(I).Item("fororderby_beamno").ToString
                            LsNo = Dt1.Rows(I).Item("fororderby_beamno").ToString

                            FsBeamNo = Trim(UCase(Dt1.Rows(I).Item("Beam_No").ToString))
                            LsBeamNo = Trim(UCase(Dt1.Rows(I).Item("Beam_No").ToString))

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
                    For I = 18 To 1 Step -1
                        If Mid$(Trim(vPrn_PvuBmNos1), I, 1) = " " Or Mid$(Trim(vPrn_PvuBmNos1), I, 1) = "," Or Mid$(Trim(vPrn_PvuBmNos1), I, 1) = "." Or Mid$(Trim(vPrn_PvuBmNos1), I, 1) = "-" Or Mid$(Trim(vPrn_PvuBmNos1), I, 1) = "/" Or Mid$(Trim(vPrn_PvuBmNos1), I, 1) = "_" Or Mid$(Trim(vPrn_PvuBmNos1), I, 1) = "(" Or Mid$(Trim(vPrn_PvuBmNos1), I, 1) = ")" Or Mid$(Trim(vPrn_PvuBmNos1), I, 1) = "\" Or Mid$(Trim(vPrn_PvuBmNos1), I, 1) = "[" Or Mid$(Trim(vPrn_PvuBmNos1), I, 1) = "]" Or Mid$(Trim(vPrn_PvuBmNos1), I, 1) = "{" Or Mid$(Trim(vPrn_PvuBmNos1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 18
                    vPrn_PvuBmNos2 = Microsoft.VisualBasic.Right(Trim(vPrn_PvuBmNos1), Len(vPrn_PvuBmNos1) - I)
                    vPrn_PvuBmNos1 = Microsoft.VisualBasic.Left(Trim(vPrn_PvuBmNos1), I - 1)
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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
            Printing_Format_1186(e)
        Else
            Printing_Format1(e)
        End If



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

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            Debug.Print(ps.PaperName)
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

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        PrintDocument1.DefaultPageSettings.Landscape = False
        'If PrintDocument1.DefaultPageSettings.Landscape = True Then
        '    With PrintDocument1.DefaultPageSettings.PaperSize
        '        PrintWidth = .Height - TMargin - BMargin
        '        PrintHeight = .Width - RMargin - LMargin
        '        PageWidth = .Height - TMargin
        '        PageHeight = .Width - RMargin
        '    End With
        'End If

        NoofItems_PerPage = 8 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 50 : ClArr(3) = 130 : ClArr(4) = 65 : ClArr(5) = 65 : ClArr(6) = 70 : ClArr(7) = 85
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 17 '18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

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
                        If Common_Procedures.settings.CustomerCode = "1186" Then

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString) / Val(prn_DetDt.Rows(prn_DetIndx).Item("bags").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                        Else
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            End If
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
                            Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 4 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 5 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 6 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 7 Then
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

        e.HasMorePages = False

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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from JobWork_Yarn_Receipt_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_PavuYarn_Receipt_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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
        If Common_Procedures.settings.CustomerCode = "1186" Then

            Common_Procedures.Print_To_PrintDocument(e, "WEFT / WARP INWARD", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "PAVU & YARN RECEIPT NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("PARTY D.C.NO  : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_PavuYarn_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_PavuYarn_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PARTY D.C.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
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
            If Common_Procedures.settings.CustomerCode = "1186" Then
                Common_Procedures.Print_To_PrintDocument(e, "WGT /", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Else

                Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PAVU DETAILS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            If Common_Procedures.settings.CustomerCode = "1186" Then
                Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 15, 2, ClAr(6), pFont)

            End If



            CurY = CurY + TxtHgt + 20
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
                    Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 4 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 5 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 6 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 7 Then
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
            If Common_Procedures.settings.CustomerCode <> "1186" Then

                If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                    If is_LastPage = True Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    End If
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

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1052" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle_No : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If

            'If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 250, CurY, 0, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(7) = CurY

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
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

    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnDetails.EditingControlShowing
        dgtxt_YarnDetails = CType(dgv_YarnDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_PavuDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.Enter
        dgv_ActCtrlName = dgv_PavuDetails.Name
        dgv_PavuDetails.EditingControl.BackColor = Color.Lime
        dgv_PavuDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_PavuDetails.SelectAll()
    End Sub

    Private Sub dgtxt_YarnDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_YarnDetails.Enter
        dgv_ActCtrlName = dgv_YarnDetails.Name
        dgv_YarnDetails.EditingControl.BackColor = Color.Lime
        dgv_YarnDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_YarnDetails.SelectAll()
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

    Public Sub Get_vehicle_from_Transport()

        If Common_Procedures.settings.CustomerCode <> "1186" Then '------UNITED WEAVES
            Exit Sub
        End If

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
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            e.Handled = True
            If cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                cbo_WidthType.Focus()
            ElseIf cbo_Cloth.Visible And cbo_Cloth.Enabled Then
                cbo_Cloth.Focus()
            End If
        End If
        Get_vehicle_from_Transport()

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()
    End Sub
    Private Sub cbo_vehicleno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "JobWork_PavuYarn_Receipt_Head", "Vehicle_No", "", "Vehicle_No")

    End Sub
    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, Nothing, "JobWork_PavuYarn_Receipt_Head", "Vehicle_No", "", "Vehicle_No")
        If (e.KeyValue = 40 And cbo_VehicleNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            e.Handled = True
            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
            dgv_PavuDetails.Focus()
            dgv_PavuDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, Nothing, "JobWork_PavuYarn_Receipt_Head", "Vehicle_No", "", "Vehicle_No", False)
        If Asc(e.KeyChar) = 13 Then
            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
            dgv_PavuDetails.Focus()
            dgv_PavuDetails.CurrentCell.Selected = True
        End If
    End Sub
    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

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
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            e.Handled = True
            If dgv_YarnDetails.Rows.Count > 0 Then


                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            Else
                cbo_VehicleNo.Focus()
            End If

            ' SendKeys.Send("+{TAB}")
        End If
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If

    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged

        If IsDate(dtp_Date.Text) = True Then

            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus

        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_WidthType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, cbo_Cloth, cbo_Transport, "", "", "", "")
    End Sub

    Private Sub cbo_WidthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, cbo_Transport, "", "", "", "")
    End Sub

    Private Sub txt_CrimpPerc_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CrimpPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_Cloth_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cbo_Cloth.SelectedIndexChanged
        Dim vCloID As Integer = 0

        vCloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        txt_CrimpPerc.Text = Common_Procedures.get_FieldValue(con, "cloth_head", "Crimp_Percentage", "(Cloth_IdNo = " & Str(Val(vCloID)) & ")")

    End Sub

    Private Sub Printing_Format_1186(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            Debug.Print(ps.PaperName)
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

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        PrintDocument1.DefaultPageSettings.Landscape = False
        'If PrintDocument1.DefaultPageSettings.Landscape = True Then
        '    With PrintDocument1.DefaultPageSettings.PaperSize
        '        PrintWidth = .Height - TMargin - BMargin
        '        PrintHeight = .Width - RMargin - LMargin
        '        PageWidth = .Height - TMargin
        '        PageHeight = .Width - RMargin
        '    End With
        'End If

        NoofItems_PerPage = 20 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 50 : ClArr(3) = 130 : ClArr(4) = 65 : ClArr(5) = 65 : ClArr(6) = 70 : ClArr(7) = 85
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18
        ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_1186_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

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
                        If Common_Procedures.settings.CustomerCode = "1186" Then

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString) / Val(prn_DetDt.Rows(prn_DetIndx).Item("bags").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                        Else
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            End If
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
                            Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 4 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + W1 + 25, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 5 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 6 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 50, CurY, 0, 0, pFont)

                        ElseIf prn_DetIndx = 7 Then
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


                Printing_Format_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_1186_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_Add3 As String, city As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String

        Dim New_Code As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from JobWork_Yarn_Receipt_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_PavuYarn_Receipt_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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
        Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "EMAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_City").ToString) <> "" Then
            City = "" & prn_HdDt.Rows(0).Item("Company_City").ToString
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)


        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & "," & City, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "/" & Cmp_CstNo, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "/ " & Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        If Common_Procedures.settings.CustomerCode = "1186" Then

            Common_Procedures.Print_To_PrintDocument(e, "WEFT / WARP INWARD", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "PAVU & YARN RECEIPT NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("PARTY D.C.NO  : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_PavuYarn_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_PavuYarn_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PARTY D.C.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5



            New_Code = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            da1 = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name from JobWork_Pavu_Receipt_Details a INNER JOIN EndsCount_Head b on a.EndsCount_IdNo = b.EndsCount_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_PavuYarn_Receipt_Code = '" & Trim(New_Code) & "' Order by a.sl_no", con)
            dt1 = New DataTable
            da1.Fill(dt1)
            prn_PageNo=1
            If dt1.Rows.Count <> 0 Then


                If prn_PageNo = 1 Then

                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    LnAr(11) = CurY
                    e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(11), LMargin + C1, LnAr(2))
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "S.No", LMargin, CurY, 2, ClAr(1) - 125, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) - 30, CurY, 2, ClAr(3), pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30, CurY, 2, ClAr(5), pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 2, ClAr(6), pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "P.O NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 2, ClAr(8), pFont)


                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    LnAr(12) = CurY
                    If dt1.Rows.Count <> 0 Then


                        For I = 0 To dt1.Rows.Count - 1


                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Trim(dt1.Rows(I).Item("Sl_no").ToString), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(dt1.Rows(I).Item("set_no").ToString), LMargin + ClAr(1) + ClAr(2) + 5, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(dt1.Rows(I).Item("Beam_No").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)


                            Common_Procedures.Print_To_PrintDocument(e, Val(dt1.Rows(I).Item("Meters")).ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                            'End If
                            Common_Procedures.Print_To_PrintDocument(e, Trim(dt1.Rows(I).Item("Po_NO").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)

                        Next
                    Else
                        CurY = CurY + TxtHgt
                        CurY = CurY + TxtHgt
                        CurY = CurY + TxtHgt
                        CurY = CurY + TxtHgt
                    End If
                    dt1.Clear()

                    CurY = CurY + TxtHgt + 10
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    LnAr(13) = CurY





                    'If (vPrn_PvuTotMtrs) <> 0 Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(vPrn_PvuTotMtrs), "#########0.00")), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    'End If

                    'If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_weight").ToString) <> 0 Then
                    '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_weight").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    'End If

                    'CurY = CurY + TxtHgt
                    'From_name = ""
                    'If prn_HdDt.Rows(prn_HeadIndx).Item("Yarn_RecFrom_Name").ToString <> "" And Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Weight").ToString) <> 0 Then
                    '    If prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_RecFrom_Name").ToString <> "" And (Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString <> 0) Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("Meters").ToString <> 0)) Then
                    '        From_name = "Rec.From (Yarn) : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Yarn_RecFrom_Name").ToString)
                    '    Else
                    '        From_name = "Rec.From : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Yarn_RecFrom_Name").ToString)
                    '    End If
                    'End If

                    'If prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_RecFrom_Name").ToString <> "" And (Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString <> 0) Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("Meters").ToString <> 0)) Then
                    '    If prn_HdDt.Rows(prn_HeadIndx).Item("Yarn_RecFrom_Name").ToString <> "" And Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Weight").ToString) <> 0 Then
                    '        From_name = From_name & IIf(Trim(From_name) <> "", "         ", "") & "Rec.From (Pavu) : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_RecFrom_Name").ToString)
                    '    Else
                    '        From_name = From_name & IIf(Trim(From_name) <> "", "         ", "") & "Rec.From : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pavu_RecFrom_Name").ToString)
                    '    End If
                    'End If

                    CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, From_name, LMargin + 20, CurY, 0, 0, pFont)
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(11))
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(11))
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(11))
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(11))

                End If
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY


            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            If Common_Procedures.settings.CustomerCode = "1186" Then
                Common_Procedures.Print_To_PrintDocument(e, "WGT /", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Else

                Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PAVU DETAILS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            If Common_Procedures.settings.CustomerCode = "1186" Then
                Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 15, 2, ClAr(6), pFont)

            End If


            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format_1186_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
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
                    Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuSetNo), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 4 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + W1 + 25, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 5 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 6 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPrn_PvuBmNos3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, 0, 0, pFont)

                ElseIf prn_DetIndx = 7 Then
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
            If Common_Procedures.settings.CustomerCode <> "1186" Then

                If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                    If is_LastPage = True Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    End If
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

            'CurY = CurY + TxtHgt - 5

            'Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                CurY = CurY + 5
                Common_Procedures.Print_To_PrintDocument(e, " Remarks : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            End If


            'LnAr(7) = CurY
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If
            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory ", PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_remarks_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_remarks.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True

            If cbo_DeliveryTo.Enabled And cbo_DeliveryTo.Visible = True Then
                cbo_DeliveryTo.Focus()
            Else
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                dgv_YarnDetails.CurrentCell.Selected = True
                dgv_YarnDetails.Focus()
            End If

        End If

        If e.KeyCode = 40 Then
            e.Handled = True
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_remarks_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_pdf_Click(sender As Object, e As EventArgs) Handles btn_pdf.Click
        PrintDocument1.DocumentName = "Weft / Warp Receipt"
        PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
        PrintDocument1.PrinterSettings.PrintFileName = "c:\Receipt.pdf"
        PrintDocument1.Print()
    End Sub

    Private Sub dgtxt_PavuDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.TextChanged
        Try
            With dgv_PavuDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(sender.Text)
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

    Private Sub dgtxt_YarnDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_YarnDetails.TextChanged
        Try
            With dgv_YarnDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(sender.Text)
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btn_print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub
    Private Sub btn_BarcodePrint_Click(sender As Object, e As EventArgs) Handles btn_BarcodePrint.Click
        'Common_Procedures.Print_OR_Preview_Status = 0
        'Prn_BarcodeSticker = True
        Printing_BarCode_Sticker_Format1_1608_DosPrint()
    End Sub
    Private Sub Printing_BarCode_Sticker_Format1_1608_DosPrint()
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
        Dim prn_DetAr(,) As String

        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Erase prn_DetAr
        prn_DetAr = New String(100, 10) {}

        Try
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            da2 = New SqlClient.SqlDataAdapter("select a.*, Lh.Ledger_Name,d.Ends_Name from JobWork_Pavu_Receipt_Details a INNER JOIN JobWork_PavuYarn_Receipt_Head hd on hd.JobWork_PavuYarn_Receipt_Code = a.JobWork_PavuYarn_Receipt_Code inner Join Ledger_Head Lh ON hd.Ledger_IdNo = lh.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON hd.EndsCount_IdNo = d.EndsCount_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_PavuYarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)

            prn_DetDt = New DataTable
            da2.Fill(prn_DetDt)

            If prn_DetDt.Rows.Count > 0 Then

                For I = 0 To prn_DetDt.Rows.Count - 1

                    prn_DetIndx = prn_DetIndx + 1

                    prn_DetAr(prn_DetIndx, 0) = Trim(prn_DetDt.Rows(I).Item("Ledger_Name").ToString)
                    prn_DetAr(prn_DetIndx, 1) = Trim(prn_DetDt.Rows(I).Item("Set_No").ToString)
                    prn_DetAr(prn_DetIndx, 2) = Trim(prn_DetDt.Rows(I).Item("Beam_No").ToString)
                    prn_DetAr(prn_DetIndx, 3) = Trim(prn_DetDt.Rows(I).Item("Ends_Name").ToString)
                    prn_DetAr(prn_DetIndx, 4) = Format(Val(prn_DetDt.Rows(I).Item("Meters").ToString), "##########0.00")
                    prn_DetAr(prn_DetIndx, 5) = Trim(prn_DetDt.Rows(I).Item("Set_Code").ToString) & "/" & prn_DetDt.Rows(I).Item("Beam_No").ToString

                Next

                Common_Procedures.Printing_BarCode_Sticker_Format1_1608_DosPrint(prn_DetDt, prn_DetAr, prn_DetIndx)
            Else

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_EndsCount_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_EndsCount.SelectedIndexChanged

    End Sub

    Private Sub cbo_WidthType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_WidthType.SelectedIndexChanged

    End Sub

    Private Sub txt_PartyDcNo_TextChanged(sender As Object, e As EventArgs) Handles txt_PartyDcNo.TextChanged

    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, txt_remarks, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = '' or Ledger_Type = 'GODOWN')) or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " ( ((Ledger_Type = '' or Ledger_Type = 'GODOWN') ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_DeliveryTo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            e.Handled = True

            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            dgv_YarnDetails.CurrentCell.Selected = True
            dgv_YarnDetails.Focus()

        End If

        If (e.KeyValue = 40 And cbo_DeliveryTo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            txt_remarks.Focus()

        End If

    End Sub

    Private Sub cbo_DeliveryTo_Enter(sender As Object, e As EventArgs) Handles cbo_DeliveryTo.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( ((Ledger_Type = '' or Ledger_Type = 'GODOWN') ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub txt_remarks_TextChanged(sender As Object, e As EventArgs) Handles txt_remarks.TextChanged

    End Sub
End Class
