Imports System.IO
Public Class SizSoft_YarnReceipt_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "YNREC-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
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

        lbl_ReceiptNo.Text = ""
        lbl_ReceiptNo.ForeColor = Color.Black

        Set_ComboBox_DataSource()

        dtp_Date.Text = ""
        txt_BookNo.Text = ""
        txt_LotNo.Text = ""
        cbo_Ledger.Text = ""
        txt_PartyDcNo.Text = ""

        dtp_Time.Text = ""
        txt_EmptyBeam.Text = ""
        cbo_BeamWidth.Text = ""
        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        txt_ReceivedBy.Text = ""
        txt_Remarks.Text = ""
        cbo_bagType.Text = ""
        cbo_godown.Text = ""
        cbo_coneType.Text = ""

        lbl_AvailableStock.Tag = 0
        lbl_AvailableStock.Text = ""

        txt_SlNo.Text = ""
        cbo_CountName.Text = ""
        cbo_YarnType.Text = "MILL"
        cbo_MillName.Text = ""
        txt_Bags.Text = ""

        txt_Weight_Bag.Text = ""
        txt_Cones_Bag.Text = ""
        txt_Weight_Cone.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""

        cbo_VendorName.Text = ""

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

    Private Sub Set_ComboBox_DataSource()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
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

        da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head where Yarn_Type <> 'BABY' order by Yarn_Type", con)
        da.Fill(dt5)
        cbo_YarnType.DataSource = dt5
        cbo_YarnType.DisplayMember = "Yarn_Type"

        da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from Beam_Width_Head order by Beam_Width_Name", con)
        da.Fill(dt6)
        cbo_BeamWidth.DataSource = dt6
        cbo_BeamWidth.DisplayMember = "Beam_Width_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Vehicle_No) from SizSoft_Yarn_Receipt_Head order by Vehicle_No", con)
        da.Fill(dt7)
        cbo_VehicleNo.DataSource = dt7
        cbo_VehicleNo.DisplayMember = "Vehicle_No"

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Then
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
            If TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
            Else
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If
    End Sub

    Private Sub DateTimePicker_OnEnter(ByVal sender As Object, ByVal e As EventArgs)

        If FrmLdSTS = True Then Exit Sub

        Try

            ''Dim dtpicker As DateTimePicker = DirectCast(sender, DateTimePicker)
            ' ''GET THE CURRENT FORMAT AND CUSTOMFORMAT STRING OF THE DTP
            ''Dim CurrentFormat As DateTimePickerFormat = dtpicker.Format
            ''Dim CurrentCustomFormat As String = dtpicker.CustomFormat

            ' ''IF THE FORMAT IS NOT CUSTOM, CHANGE IT TO CUSTOM
            ' ''OTHERWISE CHANGE IT TO SOMETHING OTHER THAN CUSTOM
            ''If dtpicker.Format <> DateTimePickerFormat.Custom Then
            ''    dtpicker.Format = DateTimePickerFormat.Custom
            ''    'SET THE CUSTOM FORMAT TO AN EMPTY STRING
            ''    dtpicker.CustomFormat = ""
            ''Else
            ''    dtpicker.Format = DateTimePickerFormat.Short
            ''End If

            ' ''SET BACK THE CACHED VALUES SO THE ACTUAL FORMAT NEVER REALLY CHANGES
            ''dtpicker.Format = CurrentFormat
            ''dtpicker.CustomFormat = CurrentCustomFormat
        Catch ex As Exception
            '
        End Try

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Company_Name as PartyName, c.Transport_Name, d.Beam_Width_Name, glh.Ledger_Name as Godown_Name from SizSoft_Yarn_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Transport_Head c ON a.Transport_IdNo = c.Transport_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_IdNo = d.Beam_Width_IdNo LEFT OUTER JOIN ledger_Head glh ON a.Godown_IdNo = glh.Ledger_IdNo Where a.Yarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_ReceiptNo.Text = dt1.Rows(0).Item("Yarn_Receipt_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Yarn_Receipt_Date").ToString

                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                txt_BookNo.Text = dt1.Rows(0).Item("Book_No").ToString
                txt_LotNo.Text = dt1.Rows(0).Item("Lot_No").ToString
                txt_PartyDcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString

                txt_EmptyBeam.Text = Val(dt1.Rows(0).Item("Empty_Beam").ToString)
                cbo_BeamWidth.Text = dt1.Rows(0).Item("Beam_Width_Name").ToString

                cbo_Transport.Text = dt1.Rows(0).Item("Transport_Name").ToString

                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString

                cbo_bagType.Text = "" ' Common_Procedures.Bag_Type_IdNoToName(con, dt1.Rows(0).Item("Bag_Type_Idno").ToString)
                cbo_coneType.Text = "" ' Common_Procedures.Cone_Type_IdNoToName(con, dt1.Rows(0).Item("Cone_Type_Idno").ToString)

                cbo_godown.Text = dt1.Rows(0).Item("Godown_Name").ToString

                dtp_Time.Text = (dt1.Rows(0).Item("Entry_Time_Text").ToString)

                txt_ReceivedBy.Text = dt1.Rows(0).Item("Received_By").ToString

                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

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

                If Val(dt1.Rows(0).Item("UnLoaded_by_Our_employee").ToString) = 1 Then chk_UNLOADEDBYOUREMPLOYEE.Checked = True

                cbo_VendorName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Vendor_IdNo").ToString))

                lbl_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                lbl_EntryType.Text = dt1.Rows(0).Item("Entry_Type").ToString

                'exCnt_iD = Common_Procedures.get_FieldValue(con, "count_head", "Textile_To_CountIdNo", "(count_idno = " & Str(Val(Cnt_ID)) & ")")
                'Cnt_ID = Common_Procedures.Count_NameToIdNo(con, dt2.Rows(0).Item("Count_IdNo").ToString)

                'exCnt_iD = Common_Procedures.get_FieldValue(con, "count_head", "Textile_To_CountIdNo", "(count_idno = " & Str(Val(Cnt_ID)) & ")")
                'If Val(TexCnt_iD) = 0 Then
                '    Throw New ApplicationException("Invalid Textile Count Name - Select ``Textile_Count_Name``  in  Count_Creation  for  " & dgv_Details.Rows(i).Cells(1).Value)
                '    Exit Sub
                'End If

                'TexMil_iD = Common_Procedures.get_FieldValue(con, "Mill_head", "Textile_To_MillIdNo", "(Mill_idno = " & Str(Val(Mil_ID)) & ")")
                'If Val(TexMil_iD) = 0 Then
                '    Throw New ApplicationException("Invalid Textile Mill Name - Select ``Textile_Mill_Name``  in  Mill_Creation  for  " & dgv_Details.Rows(i).Cells(4).Value)
                '    Exit Sub
                'End If


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from SizSoft_Yarn_Receipt_Details a inner JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Yarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Mill_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Weight_Bag").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Cones_Bag").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight_Cone").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                        dgv_Details.Rows(n).Cells(8).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        If IsDBNull(dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                            If Trim(dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                                For j = 0 To dgv_Details.ColumnCount - 1
                                    dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If
                        End If
                    Next i

                End If

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(7).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(8).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

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
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub SizSoft_YarnReceipt_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_godown.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "GODOWN" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_godown.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VendorName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VendorName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                'new_record()
                movelast_record()
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub SizSoft_YarnReceipt_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt9 As New DataTable
        Dim dt10 As New DataTable

        Me.Text = ""

        con.Open()
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '---- Kalaimagal Sizing (Palladam)
            txt_BookNo.Visible = False
            'lbl_BookNo.Text = "Lot No"
            txt_LotNo.Visible = True
        Else
            txt_BookNo.Visible = True
            lbl_BookNo.Text = "Book No"
            txt_LotNo.Visible = False
        End If
        da = New SqlClient.SqlDataAdapter("select Bag_Type_Name from Bag_Type_Head order by Bag_Type_Name", con)
        da.Fill(dt9)
        cbo_bagType.DataSource = dt9
        cbo_bagType.DisplayMember = "Bag_Type_Name"

        da = New SqlClient.SqlDataAdapter("select Cone_Type_Name from Cone_Type_Head order by Cone_Type_Name", con)
        da.Fill(dt10)
        cbo_coneType.DataSource = dt10
        cbo_coneType.DisplayMember = "Cone_Type_Name"

        'da = New SqlClient.SqlDataAdapter("select Cone_Type_Name from Cone_Type_Head order by Cone_Type_Name", con)
        'da.Fill(dt10)
        'cbo_coneType.DataSource = dt10
        'cbo_coneType.DisplayMember = "Cone_Type_Name"

        Pnl_DosPrint.Visible = False
        Pnl_DosPrint.BringToFront()
        Pnl_DosPrint.Left = (Me.Width - Pnl_DosPrint.Width) \ 2
        Pnl_DosPrint.Top = (Me.Height - Pnl_DosPrint.Height) \ 2

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.BringToFront()
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If
        'btn_UserModification.Visible = False
        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            '   btn_UserModification.Visible = True
            chk_Printed.Enabled = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            chk_UNLOADEDBYOUREMPLOYEE.Visible = True
        Else
            chk_UNLOADEDBYOUREMPLOYEE.Visible = False
        End If

        'AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BookNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bags.GotFocus, AddressOf ControlGotFocus
        AddHandler Btn_DosPrint.GotFocus, AddressOf ControlGotFocus
        AddHandler Btn_LaserPrint.GotFocus, AddressOf ControlGotFocus
        AddHandler Btn_DosCancel.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Cones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ReceivedBy.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BeamWidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight_Bag.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight_Cone.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Cones_Bag.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Add.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Delete.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_SMS.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_bagType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_coneType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_godown.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VendorName.GotFocus, AddressOf ControlGotFocus

        'AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BookNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cones.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_bagType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_coneType.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ReceivedBy.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BeamWidth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cones_Bag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight_Cone.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_godown.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Weight_Bag.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_SMS.LostFocus, AddressOf ControlLostFocus
        AddHandler Btn_DosPrint.LostFocus, AddressOf ControlLostFocus
        AddHandler Btn_LaserPrint.LostFocus, AddressOf ControlLostFocus
        AddHandler Btn_DosCancel.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VendorName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BookNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LotNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Bags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Cones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ReceivedBy.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyBeam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight_Cone.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Cones_Bag.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SlNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight_Bag.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BookNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LotNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Bags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cones.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight_Cone.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight_Bag.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ReceivedBy.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cones_Bag.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then
        dtp_Time.Visible = True
        'End If

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        'new_record()
        movelast_record()
    End Sub

    Private Sub SizSoft_YarnReceipt_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub SizSoft_YarnReceipt_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '---
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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Receipt_No from SizSoft_Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Receipt_No", con)
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
        Dim OrdByNo As String = ""

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ReceiptNo.Text))
            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Receipt_No from SizSoft_Yarn_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Receipt_No", con)
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
        Dim OrdByNo As String = "'"

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ReceiptNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Receipt_No from SizSoft_Yarn_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Receipt_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Receipt_No from SizSoft_Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Receipt_No desc", con)
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
        '----

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Receipt No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Receipt_No from SizSoft_Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Receipt No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '---
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '----
    End Sub


    Private Sub cbo_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        cbo_CountName.Tag = cbo_CountName.Text
        Show_Yarn_CurrentStock()
    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        If Trim(cbo_CountName.Text) = "" Then
            MessageBox.Show("Invalid Count Name", "DOES NOT ADD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
            Exit Sub
        End If

        If Trim(cbo_MillName.Text) = "" Then
            MessageBox.Show("Invalid MIll Name", "DOES NOT ADD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_MillName.Enabled And cbo_MillName.Visible Then cbo_MillName.Focus()
            Exit Sub
        End If

        If Trim(cbo_YarnType.Text) = "" Then
            MessageBox.Show("Invalid Yarn Type", "DOES NOT ADD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_YarnType.Enabled And cbo_YarnType.Visible Then cbo_YarnType.Focus()
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
                    .Rows(i).Cells(3).Value = cbo_MillName.Text
                    .Rows(i).Cells(4).Value = Format(Val(txt_Weight_Bag.Text), "########0.000")
                    .Rows(i).Cells(5).Value = Val(txt_Cones_Bag.Text)
                    .Rows(i).Cells(6).Value = Format(Val(txt_Weight_Cone.Text), "########0.000")
                    .Rows(i).Cells(7).Value = Val(txt_Bags.Text)
                    .Rows(i).Cells(8).Value = Val(txt_Cones.Text)
                    .Rows(i).Cells(9).Value = Format(Val(txt_Weight.Text), "########0.000")

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
                .Rows(n).Cells(3).Value = cbo_MillName.Text
                .Rows(n).Cells(4).Value = Format(Val(txt_Weight_Bag.Text), "########0.000")
                .Rows(n).Cells(5).Value = Val(txt_Cones_Bag.Text)
                .Rows(n).Cells(6).Value = Format(Val(txt_Weight_Cone.Text), "########0.000")
                .Rows(n).Cells(7).Value = Val(txt_Bags.Text)
                .Rows(n).Cells(8).Value = Val(txt_Cones.Text)
                .Rows(n).Cells(9).Value = Format(Val(txt_Weight.Text), "########0.000")

                .Rows(n).Selected = True

                If n >= 8 Then .FirstDisplayedScrollingRowIndex = n - 7

            End If

        End With

        Total_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_CountName.Text = ""
        cbo_YarnType.Text = "MILL"
        cbo_MillName.Text = ""
        txt_Weight_Bag.Text = ""
        txt_Cones_Bag.Text = ""
        txt_Weight_Cone.Text = ""
        txt_Bags.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""

        If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()

    End Sub

    Private Sub txt_Bags_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Bags.GotFocus
        Show_Yarn_CurrentStock()
    End Sub

    Private Sub txt_Bags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Bags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Bags_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Bags.TextChanged
        If Val(txt_Cones_Bag.Text) <> 0 Then
            txt_Cones.Text = Val(txt_Bags.Text) * Val(txt_Cones_Bag.Text)
        End If
        If Val(txt_Weight_Bag.Text) <> 0 Then
            txt_Weight.Text = Format(Val(txt_Bags.Text) * Val(txt_Weight_Bag.Text), "#########0.000")
        End If
    End Sub

    Private Sub txt_Cones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Cones_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Cones.TextChanged
        If Val(txt_Weight_Cone.Text) <> 0 Then
            txt_Weight.Text = Format(Val(txt_Cones.Text) * Val(txt_Weight_Cone.Text), "#########0.000")
        End If
    End Sub

    Private Sub txt_Weight_Cone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight_Cone.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Weight_Cone_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weight_Cone.KeyUp
        If (Not (e.KeyCode >= 65 And e.KeyCode <= 90)) And e.KeyCode <> 27 And e.KeyCode <> 13 And e.KeyCode <> 8 And e.KeyCode <> 9 And e.KeyCode <> 37 And e.KeyCode <> 38 And e.KeyCode <> 39 And e.KeyCode <> 40 And e.KeyCode <> 16 And e.KeyCode <> 17 Then
            txt_Weight_Bag.Text = Format(Val(txt_Cones_Bag.Text) * Val(txt_Weight_Cone.Text), "#########0.000")
            If Val(txt_Cones.Text) <> 0 Then
                txt_Weight.Text = Format(Val(txt_Cones.Text) * Val(txt_Weight_Cone.Text), "#########0.000")
            End If
        End If
    End Sub

    Private Sub txt_Weight_Cone_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Weight_Cone.LostFocus
        txt_Weight_Cone.Text = Format(Val(txt_Weight_Cone.Text), "#########0.000")
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
        If Trim(UCase(cbo_CountName.Tag)) <> Trim(UCase(cbo_CountName.Text)) Then
            get_MillCount_Details()
        End If
        Show_Yarn_CurrentStock()
    End Sub


    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        Show_Yarn_CurrentStock()
    End Sub

    Private Sub txt_EmptyBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_SlNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SlNo.GotFocus
        Show_Yarn_CurrentStock()
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
                            cbo_MillName.Text = .Rows(i).Cells(3).Value
                            txt_Weight_Bag.Text = Format(Val(.Rows(i).Cells(4).Value), "########0.000")
                            txt_Cones_Bag.Text = Val(.Rows(i).Cells(5).Value)
                            txt_Weight_Cone.Text = Format(Val(.Rows(i).Cells(6).Value), "########0.000")
                            txt_Bags.Text = Val(.Rows(i).Cells(7).Value)
                            txt_Cones.Text = Val(.Rows(i).Cells(8).Value)
                            txt_Weight.Text = Format(Val(.Rows(i).Cells(9).Value), "########0.000")

                            Exit For

                        End If

                    Next

                End With

                SendKeys.Send("{TAB}")

            End If

        End If
    End Sub

    Private Sub cbo_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_YarnType.GotFocus

        If Trim(cbo_YarnType.Text) = "" Then cbo_YarnType.Text = "MILL"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "(Yarn_type <> 'BABY')", "(Yarn_type <> '')")

        cbo_YarnType.Tag = cbo_YarnType.Text
        Show_Yarn_CurrentStock()
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
                Condt = "a.Yarn_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Yarn_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Yarn_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Yarn_Receipt_Code IN (select z1.Yarn_Receipt_Code from Yarn_Receipt_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ") "
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Yarn_Receipt_Code IN (select z2.Yarn_Receipt_Code from Yarn_Receipt_Details z2 where z2.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from SizSoft_Yarn_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Receipt_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Yarn_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Yarn_Receipt_Date").ToString), "dd-MM-yyyy")
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

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim trans_id As Integer = 0

        If Trim(cbo_VehicleNo.Text) = "" And Trim(cbo_Transport.Text) <> "" Then

            trans_id = 0 ' Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)

            Try

                If trans_id <> 0 Then
                    da1 = New SqlClient.SqlDataAdapter("select top 1 * from SizSoft_Yarn_Receipt_Head where Transport_IdNo = " & Str(Val(trans_id)) & " Order by Yarn_Receipt_Date desc, for_Orderby desc, Yarn_Receipt_No desc", con)
                    dt1 = New DataTable
                    da1.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                    End If

                    dt1.Clear()
                    dt1.Dispose()
                    da1.Dispose()
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "INVALID VEHICLE DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        End If

    End Sub

    Private Sub cbo_VehicleNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.LostFocus
        With cbo_VehicleNo
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_YarnType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_YarnType.LostFocus
        If Trim(UCase(cbo_YarnType.Tag)) <> Trim(UCase(cbo_YarnType.Text)) Then
            get_MillCount_Details()
        End If
        With cbo_YarnType
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Then
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
            cbo_MillName.Text = dgv_Details.CurrentRow.Cells(3).Value
            txt_Weight_Bag.Text = Format(Val(dgv_Details.CurrentRow.Cells(4).Value), "########0.000")
            txt_Cones_Bag.Text = Val(dgv_Details.CurrentRow.Cells(5).Value)
            txt_Weight_Cone.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.000")
            txt_Bags.Text = Val(dgv_Details.CurrentRow.Cells(7).Value)
            txt_Cones.Text = Val(dgv_Details.CurrentRow.Cells(8).Value)
            txt_Weight.Text = Format(Val(dgv_Details.CurrentRow.Cells(9).Value), "########0.000")

            If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

        End If

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
        cbo_MillName.Text = ""
        txt_Weight_Bag.Text = ""
        txt_Cones_Bag.Text = ""
        txt_Weight_Cone.Text = ""
        txt_Bags.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""

        If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()

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
            cbo_MillName.Text = ""
            txt_Weight_Bag.Text = ""
            txt_Cones_Bag.Text = ""
            txt_Weight_Cone.Text = ""
            txt_Bags.Text = ""
            txt_Cones.Text = ""
            txt_Weight.Text = ""

            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()

        End If

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
        Show_Yarn_CurrentStock()
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

    Private Sub cbo_MillName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.LostFocus
        If Trim(UCase(cbo_MillName.Tag)) <> Trim(UCase(cbo_MillName.Text)) Then
            get_MillCount_Details()
        End If
    End Sub

    Private Sub txt_Weight_Bag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight_Bag.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Weight_Bag_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weight_Bag.KeyUp
        If (Not (e.KeyCode >= 65 And e.KeyCode <= 90)) And e.KeyCode <> 27 And e.KeyCode <> 13 And e.KeyCode <> 8 And e.KeyCode <> 9 And e.KeyCode <> 37 And e.KeyCode <> 38 And e.KeyCode <> 39 And e.KeyCode <> 40 Then
            If Val(txt_Cones_Bag.Text) <> 0 Then
                txt_Weight_Cone.Text = Format(Val(txt_Weight_Bag.Text) / Val(txt_Cones_Bag.Text), "#########0.000")
            End If
        End If
    End Sub

    Private Sub txt_Weight_Bag_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Weight_Bag.TextChanged
        If Val(txt_Bags.Text) <> 0 Then
            txt_Weight.Text = Format(Val(txt_Bags.Text) * Val(txt_Weight_Bag.Text), "#########0.000")
        End If
    End Sub

    Private Sub txt_Weight_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Weight.LostFocus
        txt_Weight.Text = Format(Val(txt_Weight.Text), "#########0.000")
    End Sub

    Private Sub txt_Cones_Bag_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Cones_Bag.TextChanged
        If Val(txt_Cones_Bag.Text) <> 0 Then
            txt_Weight_Cone.Text = Format(Val(txt_Weight_Bag.Text) / Val(txt_Cones_Bag.Text), "#########0.000")
        End If
        If Val(txt_Bags.Text) <> 0 Then
            txt_Cones.Text = Val(txt_Bags.Text) * Val(txt_Cones_Bag.Text)
        End If
    End Sub



    Private Sub txt_Cones_Bag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cones_Bag.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub


    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub Show_Yarn_CurrentStock()
        Dim vCntID As Integer
        Dim vLedID As Integer
        Dim CurStk As Decimal
        Dim Vdate As Date

        If Trim(cbo_Ledger.Text) <> "" And Trim(cbo_CountName.Text) <> "" Then
            vLedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            vCntID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
            Vdate = dtp_Date.Value
            If Val(cbo_Ledger.Tag) <> Val(vLedID) Or Val(lbl_AvailableStock.Tag) <> Val(vCntID) Then
                lbl_AvailableStock.Tag = 0
                lbl_AvailableStock.Text = ""
                If Val(vLedID) <> 0 And Val(vCntID) <> 0 Then
                    CurStk = 0 'Common_Procedures.get_Yarn_CurrentStock(con, Val(lbl_Company.Tag), vLedID, vCntID)
                    ' CurStk = Common_Procedures.get_Yarn_CurrentStock(con, Val(lbl_Company.Tag), vLedID, vCntID)
                    cbo_Ledger.Tag = Val(vLedID)
                    lbl_AvailableStock.Tag = Val(vCntID)
                    lbl_AvailableStock.Text = Format(Val(CurStk), "#########0.000")
                End If
            End If

        Else
            cbo_Ledger.Tag = 0
            lbl_AvailableStock.Tag = 0
            lbl_AvailableStock.Text = ""

        End If
    End Sub

    Private Sub get_MillCount_Details()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CntID As Integer
        Dim MilID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
        MilID = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName.Text)

        If CntID <> 0 And MilID <> 0 And Trim(UCase(cbo_YarnType.Text)) = "MILL" Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                txt_Weight_Bag.Text = Format(Val(Dt.Rows(0).Item("Weight_Bag").ToString), "#########0.000")
                txt_Cones_Bag.Text = Val(Dt.Rows(0).Item("Cones_Bag").ToString)
                txt_Weight_Cone.Text = Format(Val(Dt.Rows(0).Item("Weight_Cone").ToString), "#########0.000")
            End If

            Dt.Clear()

        End If

        Dt.Dispose()
        Da.Dispose()

    End Sub

    Private Sub Weight_Calculation()
        If Val(txt_Cones_Bag.Text) <> 0 Then
            txt_Cones.Text = Val(txt_Bags.Text) * Val(txt_Cones_Bag.Text)
        End If
        If Val(txt_Weight_Cone.Text) <> 0 Then
            txt_Weight.Text = Format(Val(txt_Cones.Text) * Val(txt_Weight_Cone.Text), "#########0.000")
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
                If Val(.Rows(i).Cells(9).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(7).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(8).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(9).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(7).Value = Val(TotBags)
            .Rows(0).Cells(8).Value = Val(TotCones)
            .Rows(0).Cells(9).Value = Format(Val(TotWeight), "########0.000")
        End With

    End Sub

    Private Sub txt_Weight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weight.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Close_Form()
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_BeamWidth, cbo_VehicleNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If txt_BookNo.Visible = True Then
                txt_BookNo.Focus()
            Else
                txt_LotNo.Focus()
            End If
        End If
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, txt_RecNo, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_YarnType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_YarnType, cbo_CountName, cbo_MillName, "YarnType_Head", "Yarn_type", "(Yarn_type <> 'BABY')", "(Yarn_type <> '')")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, txt_RecNo, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub


    Private Sub cbo_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_YarnType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_YarnType, cbo_MillName, "YarnType_Head", "Yarn_Type", "(Yarn_type <> 'BABY')", "(Yarn_type <> '')")
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CountName, cbo_coneType, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
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

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_ReceivedBy, "", "", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, txt_Remarks, "", "", "", "")
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BeamWidth, txt_EmptyBeam, cbo_Transport, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub

    Private Sub cbo_BeamWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BeamWidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BeamWidth, cbo_Transport, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub

    Private Sub cbo_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName, txt_Weight_Bag, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub
    Private Sub cbo_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillName, cbo_YarnType, txt_Weight_Bag, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
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

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '---
    End Sub



    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_BeamWidth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BeamWidth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
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

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                smstxt = (cbo_Ledger.Text) & vbCrLf
                smstxt = smstxt & "YARN REC.NO-" & Trim(lbl_ReceiptNo.Text) & vbCrLf & "DATE-" & Trim(dtp_Date.Text)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Yarn_Receipt_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Yarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                            smstxt = smstxt & vbCrLf
                        End If
                        '  smstxt = smstxt & vbCrLf
                        smstxt = smstxt & vbCrLf & Trim(dt2.Rows(i).Item("Count_Name").ToString) & " - " & Trim(dt2.Rows(i).Item("Mill_Name").ToString)

                        If Val(dt2.Rows(i).Item("Bags").ToString) <> 0 Then
                            smstxt = smstxt & vbCrLf & "Bags : " & Trim(Val(dt2.Rows(i).Item("Bags").ToString))
                        End If
                        smstxt = smstxt & vbCrLf & "Weight : " & Trim(Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0"))
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
            Else
                smstxt = "YARN RECEIPT " & vbCrLf


                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                    smstxt = smstxt & vbCrLf
                End If

                smstxt = smstxt & "REC.NO-" & Trim(lbl_ReceiptNo.Text) & vbCrLf & "DATE-" & Trim(dtp_Date.Text)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Yarn_Receipt_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Yarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
            End If

            ' smstxt = smstxt & " " & vbCrLf & vbCrLf
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
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_bagType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_bagType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_bagType, txt_BookNo, cbo_coneType, "Bag_Type_Head", "Bag_Type_Name", "", "(Bag_Type_Idno = 0)")
    End Sub

    Private Sub cbo_bagType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_bagType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_bagType, cbo_coneType, "Bag_Type_Head", "Bag_Type_Name", "", "(Bag_Type_Idno = 0)")
    End Sub

    Private Sub cbo_bagType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_bagType.KeyUp
        'If e.Control = False And e.KeyValue = 17 Then
        '    Dim f As New Bag_Type_Creation
        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_bagType.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""
        '    f.MdiParent = MDIParent1
        '    f.Show()
        'End If
    End Sub
    Private Sub cbo_coneType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_coneType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_coneType, cbo_bagType, cbo_CountName, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")
    End Sub

    Private Sub cbo_coneType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_coneType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_coneType, cbo_CountName, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")
    End Sub

    Private Sub cbo_coneType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_coneType.KeyUp
        'If e.Control = False And e.KeyValue = 17 Then
        '    Dim f As New Cone_Type_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_coneType.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()
        'End If
    End Sub

    Private Sub btn_Close_DosPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_DosPrint.Click
        pnl_Back.Enabled = True
        Pnl_DosPrint.Visible = False
    End Sub

    Private Sub Btn_DosCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_DosCancel.Click
        btn_Close_DosPrint_Click(sender, e)
    End Sub

    
    Private Sub btn_UserModification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_UserModification.Click
        'If Val(Common_Procedures.User.IdNo) = 1 Then
        '    Dim f1 As New User_Modifications
        '    f1.Entry_Name = Me.Name
        '    f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        '    f1.ShowDialog()
        'End If
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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            If IsNothing(sqltr) = False Then
                cmd.Transaction = sqltr
            End If

            vPrnSTS = 0
            If chk_Printed.Checked = True Then
                vPrnSTS = 1
            End If

            cmd.CommandText = "Update SizSoft_Yarn_Receipt_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
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

    Private Sub cbo_godown_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_godown.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
    End Sub



    Private Sub cbo_godown_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_godown.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_godown, txt_PartyDcNo, cbo_bagType, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_godown_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_godown.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_godown, cbo_bagType, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_type ='GODOWN')", "(Ledger_IdNo = 0)")
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



    'Private Sub txt_BookNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BookNo.KeyDown
    '    If e.KeyValue = 38 Then
    '        cbo_godown.Focus()
    '    End If
    'End Sub

    Private Sub cbo_VendorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VendorName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VendorName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VendorName, txt_EmptyBeam, cbo_BeamWidth, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VendorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VendorName, cbo_BeamWidth, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VendorName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_VendorName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

End Class