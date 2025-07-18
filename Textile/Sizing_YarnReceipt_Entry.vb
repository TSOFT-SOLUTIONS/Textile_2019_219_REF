Imports System.IO
Public Class Sizing_YarnReceipt_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Pk_Condition As String = "YNREC-"
    Private Pk_Condition_Tex As String = "SSYRC-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_PageNo1 As Integer
    Private prn_DetSNo1 As Integer
    Private prn_NoofBmDets1 As Integer
    Private prn_DetIndx1 As Integer
    Private prn_NoofBmDets As Integer

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private TrnTo_DbName As String = ""

    Private fs As FileStream
    Private sw As StreamWriter

    Private Hz1 As Integer, Hz2 As Integer, Vz1 As Integer, Vz2 As Integer
    Private Corn1 As Integer, Corn2 As Integer, Corn3 As Integer, Corn4 As Integer
    Private LfCon As Integer, RgtCon As Integer

    Private Prnt_HalfSheet_STS As Boolean = False
    Private vPrnt_2Copy_In_SinglePage As Integer = 0


    Private Print_PDF_Status As Boolean = False
    Private EMAIL_Status As Boolean = False
    Private WHATSAPP_Status As Boolean = False


    Private Sub clear()

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_ReceiptNo.Text = ""
        lbl_ReceiptNo.ForeColor = Color.Black

        Print_PDF_Status = False

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
        pnl_Delivery_Selection.Visible = False
        lbl_Delivery_Code.Text = ""
        txt_SlNo.Text = ""
        cbo_CountName.Text = ""
        cbo_YarnType.Text = "MILL"
        cbo_MillName.Text = ""
        cbo_Det_Location.Text = ""
        txt_DetLotNo.Text = ""
        txt_Bags.Text = ""

        txt_Weight_Bag.Text = ""
        txt_Cones_Bag.Text = ""
        txt_Weight_Cone.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""

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

        da = New SqlClient.SqlDataAdapter("select distinct(Vehicle_No) from Yarn_Receipt_Head order by Vehicle_No", con)
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

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        If IsNothing(dgv_Details_Total.CurrentCell) Then Exit Sub
        If IsNothing(dgv_Filter_Details.CurrentCell) Then Exit Sub

        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName, c.Transport_Name, d.Beam_Width_Name, glh.Ledger_Name as Godown_Name from Yarn_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Transport_Head c ON a.Transport_IdNo = c.Transport_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_IdNo = d.Beam_Width_IdNo LEFT OUTER JOIN ledger_Head glh ON a.WareHouse_IdNo = glh.Ledger_IdNo Where a.Yarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
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

                cbo_bagType.Text = Common_Procedures.Bag_Type_IdNoToName(con, dt1.Rows(0).Item("Bag_Type_Idno").ToString)
                cbo_coneType.Text = Common_Procedures.Conetype_IdNoToName(con, dt1.Rows(0).Item("Cone_Type_Idno").ToString)

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
                lbl_Delivery_Code.Text = dt1.Rows(0).Item("Delivery_Code").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Selection_type").ToString
                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from SizingSoft_Yarn_Receipt_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Yarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                        dgv_Details.Rows(n).Cells(10).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(i).Item("Location_IdNo").ToString))

                        dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Lot_No").ToString


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

    Private Sub YarnReceipt_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

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

    Private Sub YarnReceipt_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt9 As New DataTable
        Dim dt10 As New DataTable

        Me.Text = ""

        con.Open()

        If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
            TrnTo_DbName = Common_Procedures.get_Company_TextileDataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        Else
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        End If



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then '---- Kalaimagal Sizing (Palladam)
            txt_BookNo.Visible = False
            lbl_BookNo_Caption.Text = "Lot No"
            txt_LotNo.Visible = True
            txt_LotNo.Width = txt_BookNo.Width
            txt_LotNo.BackColor = txt_BookNo.BackColor
        Else
            txt_BookNo.Visible = True
            lbl_BookNo_Caption.Text = "Book No"
            txt_LotNo.Visible = False
        End If

        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            txt_BookNo.Visible = False
            lbl_BookNo_Caption.Visible = False

            cbo_Type.Visible = True
            lbl_Type_Caption.Visible = True
            lbl_Type_Caption.BackColor = Color.LightSkyBlue
            cbo_Type.Width = txt_BookNo.Width
            cbo_Type.BackColor = txt_BookNo.BackColor

            btn_Delivery_Selection.Visible = True

        End If

        lbl_Godown_Caption.Visible = False
        cbo_godown.Visible = False
        txt_PartyDcNo.Width = cbo_Ledger.Width

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then
            lbl_Godown_Caption.Text = "Location"
        End If

        If Val(Common_Procedures.settings.Multi_Godown_Status) = 1 Then
            lbl_Godown_Caption.Visible = True
            cbo_godown.Visible = True
            txt_PartyDcNo.Width = 99
        End If

        da = New SqlClient.SqlDataAdapter("select Bag_Type_Name from Bag_Type_Head order by Bag_Type_Name", con)
        da.Fill(dt9)
        cbo_bagType.DataSource = dt9
        cbo_bagType.DisplayMember = "Bag_Type_Name"

        da = New SqlClient.SqlDataAdapter("select Conetype_Name from ConeType_Head order by Conetype_Name", con)
        da.Fill(dt10)
        cbo_coneType.DataSource = dt10
        cbo_coneType.DisplayMember = "Conetype_Name"

        'da = New SqlClient.SqlDataAdapter("select Cone_type_Name from Cone_Type_Head order by Cone_type_Name", con)
        'da.Fill(dt10)
        'cbo_coneType.DataSource = dt10
        'cbo_coneType.DisplayMember = "Cone_type_Name"

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

        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            chk_Printed.Enabled = True
        End If

        btn_UserModification.Visible = False
        If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
            btn_UserModification.Visible = True
        End If

        pnl_Delivery_Selection.Visible = False
        pnl_Delivery_Selection.Left = (Me.Width - pnl_Delivery_Selection.Width) \ 2
        pnl_Delivery_Selection.Top = (Me.Height - pnl_Delivery_Selection.Height) \ 2
        pnl_Delivery_Selection.BringToFront()

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


        'AddHandler dtp_Time.KeyPress, AddressOf TextBoxControlKeyPress
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
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Det_Location.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Det_Location.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DetLotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DetLotNo.LostFocus, AddressOf ControlLostFocus

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then
        dtp_Time.Visible = True
        'End If

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0
        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("DELIVERY")
        Filter_Status = False
        FrmLdSTS = True

        ' Detail Grid Showing  for KKP only
        If Common_Procedures.settings.Yarn_Stock_LotNo_wise_Status Then
            lbl_DetLocation.Visible = True
            cbo_Det_Location.Visible = True
            lbl_DetLotNo.Visible = True
            txt_DetLotNo.Visible = True
        Else
            lbl_DetLocation.Visible = False
            cbo_Det_Location.Visible = False
            lbl_DetLotNo.Visible = False
            txt_DetLotNo.Visible = False
        End If

        new_record()

    End Sub

    Private Sub YarnReceipt_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub YarnReceipt_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Delivery_Selection.Visible = True Then
                    btn_Close_Delivery_Selection_Click(sender, e)
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
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vDbName As String = ""
        Dim LedIdNo As Integer = 0
        Dim TexComp_ID As String = 0
        Dim UID As Single = 0
        Dim vUsrNm As String = "", vAcPwd As String = "", vUnAcPwd As String = ""


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


        If Trim(TrnTo_DbName) <> "" Then
            vDbName = Trim(TrnTo_DbName) & ".."
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_YARN_RECEIPT, New_Entry, Me, con, "Yarn_Receipt_Head", "Yarn_Receipt_Code", NewCode, "Yarn_Receipt_Date", "(Yarn_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from Yarn_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
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

        tr = con.BeginTransaction

        Try

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ReceiptNo.Text)

            cmd.Connection = con
            cmd.Transaction = tr

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Yarn_Receipt_Head", "Yarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Yarn_Receipt_Code, Company_IdNo, for_OrderBy", tr)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "SizingSoft_Yarn_Receipt_Details", "Yarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Count_IdNo, Yarn_Type, Mill_IdNo, Weight_Bag, Cones_Bag, Weight_Cone, Bags, Cones, Weight", "Sl_No", "Yarn_Receipt_Code, For_OrderBy, Company_IdNo, Yarn_Receipt_No, Yarn_Receipt_Date, Ledger_Idno", tr)

            If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
                LedIdNo = Val(Common_Procedures.get_FieldValue(con, "Yarn_Delivery_Head", "ledger_idno", "(Yarn_Delivery_Code = '" & Trim(NewCode) & "')", , tr))
                TexComp_ID = Val(Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(LedIdNo)) & ")", , tr))
                If Val(TexComp_ID) <> 0 Then

                    cmd.CommandText = "delete from " & Trim(vDbName) & "SizSoft_Yarn_Receipt_Head where Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "delete from " & Trim(vDbName) & "SizSoft_SizingSoft_Yarn_Receipt_Details where Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Delete from " & Trim(vDbName) & "Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                End If
            End If

            cmd.CommandText = "Delete from Stock_WasteMaterial_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from SizingSoft_Yarn_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Yarn_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Receipt_No from Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Receipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Receipt_No from Yarn_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Receipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Receipt_No from Yarn_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Receipt_No desc", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Receipt_No from Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Receipt_No desc", con)
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
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_ReceiptNo.Text = Common_Procedures.get_MaxCode(con, "Yarn_Receipt_Head", "Yarn_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_ReceiptNo.ForeColor = Color.Red


            dtp_Time.Text = Format(Now, "hh:mm tt").ToString

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
        da.Dispose()

        'If Trim(Common_Procedures.settings.CustomerCode) = "1112" Then '----VENUS SIZING
        '    If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
        'Else
        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        'End If

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Receipt No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Receipt_No from Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_YARN_RECEIPT, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Receipt No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Receipt_No from Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Receipt No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_ReceiptNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim led_id As Integer = 0
        Dim trans_id As Integer = 0
        Dim Bw_id As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mil_ID As Integer = 0
        Dim LocDet_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single
        Dim Bg_Id As Integer
        Dim Gd_Id As Integer
        Dim Con_Id As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt As New DataTable
        Dim WstBg_Id As Integer
        Dim WstCn_Id As Integer
        Dim vOrdByNo As String = ""
        Dim vYrnPartcls As String = ""
        Dim TexComp_ID As String = 0
        Dim vEntLedIdNo As String = 0
        Dim vDbName As String = ""
        Dim TexCnt_iD As String = 0
        Dim TexMil_iD As String = 0
        Dim Nr As Long = 0
        Dim TexLed_ID As String = 0
        Dim vNewFrmTYpe As String = ""
        Dim vSELC_DCCODE As String = ""

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        If Trim(TrnTo_DbName) <> "" Then
            vDbName = Trim(TrnTo_DbName) & ".."
        End If

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_YARN_RECEIPT, New_Entry, Me, con, "Yarn_Receipt_Head", "Yarn_Receipt_Code", NewCode, "Yarn_Receipt_Date", "(Yarn_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Yarn_Receipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
            If Trim(cbo_VehicleNo.Text) = "" Then
                MessageBox.Show("Invalid Vehicle No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_VehicleNo.Enabled And cbo_VehicleNo.Visible Then cbo_VehicleNo.Focus()
                Exit Sub
            End If
        End If

        If Trim(cbo_VehicleNo.Text) <> "" Then
            cbo_VehicleNo.Text = Common_Procedures.Vehicle_Number_Remove_Unwanted_Spaces(Trim(cbo_VehicleNo.Text))
        End If

        If dtp_Time.Visible Then

            If New_Entry = True Then
                If Trim(dtp_Time.Text) = "" Or IsDate(dtp_Time.Text) = False Then
                    dtp_Time.Text = Format(Now, "Short Time").ToString
                End If
            End If
            If Trim(dtp_Time.Text) = "" Then
                MessageBox.Show("Invalid Time", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                Exit Sub
            End If
            If IsDate(dtp_Time.Text) = False Then
                MessageBox.Show("Invalid Time", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                Exit Sub
            End If

        Else

            If New_Entry = True Or Trim(dtp_Time.Text) = "" Or IsDate(dtp_Time.Text) = False Then
                dtp_Time.Value = Now
            End If

        End If

        trans_id = Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)

        Bg_Id = Common_Procedures.BagType_NameToIdNo(con, cbo_bagType.Text)
        Gd_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_godown.Text)
        Con_Id = Common_Procedures.ConeType_NameToIdNo(con, cbo_coneType.Text)

        Bw_id = Common_Procedures.BeamWidth_NameToIdNo(con, cbo_BeamWidth.Text)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
            If Val(txt_EmptyBeam.Text) <> 0 Then
                If Bw_id = 0 Then
                    MessageBox.Show("Invalid BeamWidth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_BeamWidth.Enabled And cbo_BeamWidth.Visible Then cbo_BeamWidth.Focus()
                    Exit Sub
                End If
            End If
        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(9).Value) <> 0 Then
                Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(1).Value))
                If Cnt_ID = 0 Then
                    MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
                    Exit Sub
                End If

                If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_YarnType.Enabled And cbo_YarnType.Visible Then cbo_YarnType.Focus()
                    Exit Sub
                End If

                Mil_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(3).Value))
                If Mil_ID = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_MillName.Enabled And cbo_MillName.Visible Then cbo_MillName.Focus()
                    Exit Sub
                End If

                '10 LOCATION 11 LOT
                'Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(10).Value), tr)

                If Common_Procedures.settings.CustomerCode = "1288" Then
                    Dim l As Integer = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(10).Value))
                    If l = 0 Then
                        MessageBox.Show("Invalid Location Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If cbo_Det_Location.Enabled And cbo_Det_Location.Visible Then cbo_Det_Location.Focus()
                        Exit Sub
                    End If

                End If


                If Common_Procedures.settings.CustomerCode = "1282" And Trim(dgv_Details.Rows(i).Cells(2).Value.ToString.ToUpper) = "MILL" Then
                    Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(Mil_ID)) & " and count_idno = " & Str(Val(Cnt_ID)), con)
                    Da.Fill(Dt)

                    If Val(Dt.Rows.Count) = 0 Then
                        MessageBox.Show("Invalid Mill Name Or Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If cbo_MillName.Enabled And cbo_MillName.Visible Then cbo_MillName.Focus()
                        Exit Sub
                    End If

                    Dt.Clear()
                    Dt.Dispose()
                    Da.Dispose()
                End If

            End If

        Next

        vSELC_DCCODE = ""
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(cbo_Type.Text) = "DELIVERY" Then
            vSELC_DCCODE = Trim(lbl_Delivery_Code.Text)
        End If

        vTotYrnBags = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotYrnBags = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotYrnCones = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            vTotYrnWeight = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from Yarn_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
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
        tr = con.BeginTransaction

        'Try

        If Insert_Entry = True Or New_Entry = False Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Else

            lbl_ReceiptNo.Text = Common_Procedures.get_MaxCode(con, "Yarn_Receipt_Head", "Yarn_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        End If

        cmd.Connection = con
        cmd.Transaction = tr

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@ReceiptDate", dtp_Date.Value.Date)

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ReceiptNo.Text)

        If New_Entry = True Then

            cmd.CommandText = "Insert into Yarn_Receipt_Head(User_IdNo                               , Yarn_Receipt_Code     ,  Company_IdNo                    , Yarn_Receipt_No                   , for_OrderBy               , Yarn_Receipt_Date, Book_No                        , Ledger_IdNo             , Party_DcNo                        , Beam_Width_IdNo        , Empty_Beam                          , Transport_IdNo            , Vehicle_No                        , Received_By                        , Remarks                         , Total_Bags                   ,  Total_Cones                  , Total_Weight                    ,  Bag_Type_Idno           , Cone_Type_Idno          ,   Entry_Time_Text           ,            WareHouse_IdNo   ,Lot_No    ,Selection_type,Delivery_Code  ) " &
                                              "     Values (" & Str(Val(Common_Procedures.User.IdNo)) & ",'" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @ReceiptDate     , '" & Trim(txt_BookNo.Text) & "', " & Str(Val(led_id)) & ", '" & Trim(txt_PartyDcNo.Text) & "', " & Str(Val(Bw_id)) & ", " & Str(Val(txt_EmptyBeam.Text)) & ", " & Str(Val(trans_id)) & ", '" & Trim(cbo_VehicleNo.Text) & "', '" & Trim(txt_ReceivedBy.Text) & "', '" & Trim(txt_Remarks.Text) & "', " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & " , " & Str(Val(Bg_Id)) & "  ," & Str(Val(Con_Id)) & " ,'" & Trim(dtp_Time.Text) & "', " & Val(Gd_Id) & "       , '" & Trim(txt_LotNo.Text) & "','" & Trim(cbo_Type.Text) & "','" & Trim(vSELC_DCCODE) & "')"
            cmd.ExecuteNonQuery()

        Else

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Yarn_Receipt_Head", "Yarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Receipt_Code, Company_IdNo, for_OrderBy", tr)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "SizingSoft_Yarn_Receipt_Details", "Yarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo, Yarn_Type, Mill_IdNo, Weight_Bag, Cones_Bag, Weight_Cone, Bags, Cones, Weight", "Sl_No", "Yarn_Receipt_Code, For_OrderBy, Company_IdNo, Yarn_Receipt_No, Yarn_Receipt_Date, Ledger_Idno", tr)

            If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
                vEntLedIdNo = Val(Common_Procedures.get_FieldValue(con, "Yarn_Receipt_Head", "ledger_idno", "(Yarn_Receipt_Code = '" & Trim(NewCode) & "')", , tr))
                TexComp_ID = Val(Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(vEntLedIdNo)) & ")", , tr))
                If Val(TexComp_ID) <> 0 Then
                    cmd.CommandText = "Delete from " & Trim(vDbName) & "SizSoft_Yarn_Receipt_Head Where Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Delete from " & Trim(vDbName) & "SizSoft_Yarn_Receipt_Details Where Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Delete from " & Trim(vDbName) & "Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()
                End If
            End If

            cmd.CommandText = "Update Yarn_Receipt_Head set User_IdNo = " & Str(Val(Common_Procedures.User.IdNo)) & ",Yarn_Receipt_Date = @ReceiptDate, Book_No = '" & Trim(txt_BookNo.Text) & "'  , Bag_Type_Idno = " & Str(Val(Bg_Id)) & "  , Cone_Type_Idno = " & Str(Val(Con_Id)) & " , Ledger_IdNo = " & Str(Val(led_id)) & ", Party_DcNo = '" & Trim(txt_PartyDcNo.Text) & "', Beam_Width_IdNo = " & Str(Val(Bw_id)) & ", Empty_Beam = " & Str(Val(txt_EmptyBeam.Text)) & ", Transport_IdNo = " & Str(Val(trans_id)) & ", Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "', Received_By = '" & Trim(txt_ReceivedBy.Text) & "', Remarks = '" & Trim(txt_Remarks.Text) & "', Total_Bags = " & Str(Val(vTotYrnBags)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & ", Entry_Time_Text = '" & Trim(dtp_Time.Text) & "', WareHouse_IdNo =  " & Val(Gd_Id) & " ,Lot_No ='" & Trim(txt_LotNo.Text) & "' ,Selection_type='" & Trim(cbo_Type.Text) & "',Delivery_Code='" & Trim(vSELC_DCCODE) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_WasteMaterial_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

        End If

        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Yarn_Receipt_Head", "Yarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, vOrdByNo, Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Receipt_Code, Company_IdNo, for_OrderBy", tr)

        If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then

            TexComp_ID = Val(Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(led_id)) & ")", , tr))

            If Val(TexComp_ID) <> 0 Then

                TexLed_ID = Common_Procedures.get_FieldValue(con, "company_head", "Textile_To_SizingIdNo", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")", , tr)
                If Val(TexLed_ID) = 0 Then
                    Throw New ApplicationException("Invalid Textile Sizing Name" & Chr(13) & "Select ``Textile_Sizing_Name``  in  Company_Creation  for  " & lbl_Company.Text)
                    Exit Sub
                End If

                cmd.CommandText = "Insert into " & Trim(vDbName) & "SizSoft_Yarn_Receipt_Head (           User_IdNo                         , Yarn_Receipt_Code     ,  Company_IdNo               , Yarn_Receipt_No                   , for_OrderBy               , Yarn_Receipt_Date, Book_No                        , Ledger_IdNo                , Party_DcNo                        , Beam_Width_IdNo        , Empty_Beam                          , Transport_IdNo            , Vehicle_No                        , Received_By                        , Remarks                         , Total_Bags                   ,  Total_Cones                  , Total_Weight                    ,  Bag_Type_Idno           , Cone_Type_Idno          ,   Entry_Time_Text           ,   WareHouse_IdNo   ,               Lot_No           ) " &
                                      "Values                                                     (" & Str(Val(Common_Procedures.User.IdNo)) & ",'" & Trim(NewCode) & "', " & Str(Val(TexComp_ID)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @ReceiptDate     , '" & Trim(txt_BookNo.Text) & "', " & Str(Val(TexLed_ID)) & ", '" & Trim(txt_PartyDcNo.Text) & "', " & Str(Val(Bw_id)) & ", " & Str(Val(txt_EmptyBeam.Text)) & ", " & Str(Val(trans_id)) & ", '" & Trim(cbo_VehicleNo.Text) & "', '" & Trim(txt_ReceivedBy.Text) & "', '" & Trim(txt_Remarks.Text) & "', " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & " , " & Str(Val(Bg_Id)) & "  ," & Str(Val(Con_Id)) & " ,'" & Trim(dtp_Time.Text) & "', " & Val(Gd_Id) & " , '" & Trim(txt_LotNo.Text) & "' ) "
                cmd.ExecuteNonQuery()

            End If

        End If


        If Val(Common_Procedures.settings.StatementPrint_BookNo_IN_Stock_Particulars_Status) = 1 Then
            Partcls = "Yarn : Rec.No. " & Trim(lbl_ReceiptNo.Text)
            If Trim(txt_BookNo.Text) <> "" Then PBlNo = Trim(txt_BookNo.Text) Else PBlNo = Trim(lbl_ReceiptNo.Text)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            Partcls = "Yarn : Rec.No. " & Trim(txt_LotNo.Text)
            PBlNo = Trim(txt_LotNo.Text)
        Else
            Partcls = "Yarn : Rec.No. " & Trim(lbl_ReceiptNo.Text)
            PBlNo = Trim(lbl_ReceiptNo.Text)

        End If

        'If Trim(txt_PartyDcNo.Text) <> "" Then
        '    Partcls = "Rcpt : P.Dc.No. " & Trim(txt_PartyDcNo.Text)
        '    PBlNo = Trim(txt_PartyDcNo.Text)
        'Else
        '    Partcls = "Rcpt : Rec.No. " & Trim(lbl_ReceiptNo.Text)
        '    PBlNo = Trim(lbl_ReceiptNo.Text)
        'End If

        If Val(Bg_Id) <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select a.* from Waste_Head a Where a.Bag_Type_Idno = " & Str(Val(Bg_Id)), con)
            Da.SelectCommand.Transaction = tr
            Dt = New DataTable
            Da.Fill(Dt)

            WstBg_Id = 0
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    WstBg_Id = Val(Dt.Rows(0).Item("Packing_Idno").ToString)
                End If
            End If

            Dt.Dispose()
            Da.Dispose()

            cmd.CommandText = "Insert into Stock_WasteMaterial_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No        ,                   For_OrderBy      ,   Reference_Date  ,        Ledger_IdNo      ,      Party_Bill_No   ,           Sl_No      ,          Waste_IdNo  ,      Quantity     ,     Rate ,  Amount ) " &
                                             "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @ReceiptDate   , " & Str(Val(led_id)) & ", '" & Trim(PBlNo) & "', 1, " & Str(Val(WstBg_Id)) & ", " & Str(Val(vTotYrnBags)) & ",  0   , 0 )"
            cmd.ExecuteNonQuery()

        End If

        If Val(Con_Id) <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select a.* from Waste_Head a Where a.Cone_Type_Idno = " & Str(Val(Con_Id)), con)
            Da.SelectCommand.Transaction = tr
            Dt = New DataTable
            Da.Fill(Dt)

            WstCn_Id = 0
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    WstCn_Id = Val(Dt.Rows(0).Item("Packing_Idno").ToString)
                End If
            End If

            Dt.Dispose()
            Da.Dispose()

            cmd.CommandText = "Insert into Stock_WasteMaterial_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No        ,                    For_OrderBy     , Reference_Date,        Ledger_IdNo      ,      Party_Bill_No   ,           Sl_No      ,          Waste_IdNo  ,      Quantity     ,     Rate ,  Amount ) " &
                                             "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @ReceiptDate   , " & Str(Val(led_id)) & ", '" & Trim(PBlNo) & "', 2 , " & Str(Val(WstCn_Id)) & ", " & Str(Val(vTotYrnCones)) & ",  0   , 0 )"
            cmd.ExecuteNonQuery()

        End If


        cmd.CommandText = "Delete from SizingSoft_Yarn_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Delete from Yarn_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        Sno = 0
        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(9).Value) <> 0 Then

                Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(1).Value), tr)
                Mil_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(3).Value), tr)
                LocDet_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(10).Value), tr)

                Sno = Sno + 1

                cmd.CommandText = "Insert into SizingSoft_Yarn_Receipt_Details(Yarn_Receipt_Code, Company_IdNo, Yarn_Receipt_No, for_OrderBy, Yarn_Receipt_Date, Ledger_IdNo, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Weight_Bag, Cones_Bag, Weight_Cone, Bags, Cones, Weight,Location_IdNo,Lot_no) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @ReceiptDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "', " & Str(Val(Mil_ID)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(8).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(9).Value)) & "," & Str(Val(LocDet_ID)) & ",'" & Trim(dgv_Details.Rows(i).Cells(11).Value) & "')"
                cmd.ExecuteNonQuery()



                vYrnPartcls = Partcls
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Somanur)
                    vYrnPartcls = vYrnPartcls & ",  Mill :  " & Trim(dgv_Details.Rows(i).Cells(3).Value)
                End If

                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (                   SoftwareType_IdNo                              ,                    Reference_Code            ,                 Company_IdNo      ,               Reference_No         ,            for_OrderBy     , Reference_Date , DeliveryTo_Idno,     ReceivedFrom_Idno    ,      Party_Bill_No    ,               Sl_No   ,             Count_IdNo   ,                           Yarn_Type                ,            Mill_IdNo     ,                           Weight_Bag                 ,                             Cones_Bag                ,                         Weight_Cone                  ,                           Bags                       ,                            Cones                     ,                             Weight                   ,          Particulars        , Posting_For, Set_Code, Set_No,     WareHouse_IdNo   ,Lot_No  ) " &
                                      " Values                                (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_ReceiptNo.Text) & "' , " & Str(Val(vOrdByNo)) & " ,  @ReceiptDate  ,          0     , " & Str(Val(led_id)) & " , '" & Trim(PBlNo) & "' , " & Str(Val(Sno)) & " , " & Str(Val(Cnt_ID)) & " , '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "' , " & Str(Val(Mil_ID)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(8).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(9).Value)) & " , '" & Trim(vYrnPartcls) & "' ,   'RECEIPT',      '' ,    '' , " & Str(Val(LocDet_ID)) & ",'" & Trim(dgv_Details.Rows(i).Cells(11).Value) & "')"
                cmd.ExecuteNonQuery()

                If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then

                    TexComp_ID = Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(led_id)) & ")", , tr)

                    If Val(TexComp_ID) <> 0 Then

                        TexLed_ID = Common_Procedures.get_FieldValue(con, "company_head", "Textile_To_SizingIdNo", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")", , tr)
                        If Val(TexLed_ID) = 0 Then
                            Throw New ApplicationException("Invalid Textile Sizing Name" & Chr(13) & "Select ``Textile_Sizing_Name``  in  Company_Creation  for  " & lbl_Company.Text)
                            Exit Sub
                        End If

                        TexCnt_iD = Common_Procedures.get_FieldValue(con, "count_head", "Textile_To_CountIdNo", "(count_idno = " & Str(Val(Cnt_ID)) & ")", , tr)
                        If Val(TexCnt_iD) = 0 Then
                            vNewFrmTYpe = "COUNT"
                            Throw New ApplicationException("Invalid Textile Count Name" & Chr(13) & "Select ``Textile_Count_Name``  in  Count_Creation  for  " & dgv_Details.Rows(i).Cells(1).Value)
                            Exit Sub
                        End If

                        TexMil_iD = Common_Procedures.get_FieldValue(con, "Mill_head", "Textile_To_MillIdNo", "(Mill_idno = " & Str(Val(Mil_ID)) & ")", , tr)
                        If Val(TexMil_iD) = 0 Then
                            vNewFrmTYpe = "MILL"
                            Throw New ApplicationException("Invalid Textile Mill Name" & Chr(13) & "Select ``Textile_Mill_Name``  in  Mill_Creation  for  " & dgv_Details.Rows(i).Cells(3).Value)
                            Exit Sub
                        End If


                        cmd.CommandText = "Insert into " & Trim(vDbName) & "SizSoft_Yarn_Receipt_Details (   Yarn_Receipt_Code    ,             Company_IdNo    ,          Yarn_Receipt_No          ,          for_OrderBy      , Yarn_Receipt_Date,         Ledger_IdNo        ,         Sl_No        ,           Count_IdNo       ,                               Yarn_Type           ,               Mill_IdNo    ,                                 Weight_Bag          ,                                 Cones_Bag           ,                                 Weight_Cone         ,                                 Bags                ,                                 Cones               ,                                 Weight               ) " &
                                                "                               Values                       ( '" & Trim(NewCode) & "', " & Str(Val(TexComp_ID)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & ",   @ReceiptDate   , " & Str(Val(TexLed_ID)) & ", " & Str(Val(Sno)) & ", " & Str(Val(TexCnt_iD)) & ", '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "', " & Str(Val(TexMil_iD)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(8).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(9).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into  " & Trim(vDbName) & "Stock_Yarn_Processing_Details (               Reference_Code                    ,            Company_IdNo     ,             Reference_No          ,         for_OrderBy       , Reference_Date,      DeliveryTo_Idno       ,  ReceivedFrom_Idno ,                              Entry_ID                            ,        Particulars     ,     Party_Bill_No    ,            Sl_No      ,          Count_IdNo       ,                               Yarn_Type           ,           Mill_IdNo        ,                                 Bags                ,                                 Cones               ,                                 Weight              , DeliveryToIdno_ForParticulars,  ReceivedFromIdno_ForParticulars  ) " &
                                                "          Values                                              ( '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "', " & Str(Val(TexComp_ID)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & ",   @ReceiptDate, " & Str(Val(TexLed_ID)) & ",          0         , '" & Trim(Trim(Pk_Condition_Tex) & Trim(lbl_ReceiptNo.Text)) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(TexCnt_iD)) & ", '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "', " & Str(Val(TexMil_iD)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(8).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(9).Value)) & ", " & Str(Val(TexLed_ID)) & "  ,               0                   ) "
                        cmd.ExecuteNonQuery()

                    End If

                End If

            End If

        Next

        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(UCase(cbo_Type.Text)) = "DELIVERY" And Trim(vSELC_DCCODE) <> "" Then
            If Val(vTotYrnWeight) <> 0 Then

                cmd.CommandText = "Insert into Yarn_Delivery_Selections_Processing_Details (                   Reference_Code           ,               Company_IdNo       ,             Reference_No          ,         for_OrderBy       , Reference_Date,         Delivery_Code       ,               Delivery_No         ,      DeliveryTo_Idno    , ReceivedFrom_Idno       ,             Party_Dc_No           ,              Total_Bags           ,                total_cones         ,                   Total_Weight       ) " &
                                    "  Values                                              ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @ReceiptDate  , '" & Trim(vSELC_DCCODE) & "', '" & Trim(txt_PartyDcNo.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(led_id)) & ", '" & Trim(txt_PartyDcNo.Text) & "', " & Str(-1 * Val(vTotYrnBags)) & ", " & Str(-1 * Val(vTotYrnCones)) & ", " & Str(-1 * Val(vTotYrnWeight)) & " )"
                cmd.ExecuteNonQuery()

            End If
        End If

        Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "SizingSoft_Yarn_Receipt_Details", "Yarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_ReceiptNo.Text, vOrdByNo, Pk_Condition, "", "", New_Entry, False, "Count_IdNo, Yarn_Type, Mill_IdNo, Weight_Bag, Cones_Bag, Weight_Cone, Bags, Cones, Weight", "Sl_No", "Yarn_Receipt_Code, For_OrderBy, Company_IdNo, Yarn_Receipt_No, Yarn_Receipt_Date, Ledger_Idno", tr)

        If Val(txt_EmptyBeam.Text) <> 0 Or Val(vTotYrnBags) <> 0 Or Val(vTotYrnCones) <> 0 Then
            cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( SoftwareType_IdNo  , Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @ReceiptDate, 0, " & Str(Val(led_id)) & ", '" & Trim(PBlNo) & "', 1, " & Str(Val(Bw_id)) & ", " & Str(Val(txt_EmptyBeam.Text)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", '" & Trim(Partcls) & "' )"
            cmd.ExecuteNonQuery()
        End If

        If Val(Common_Procedures.User.IdNo) = 1 Then
            If chk_Printed.Visible = True Then
                If chk_Printed.Enabled = True Then
                    Update_PrintOut_Status(tr)
                End If
            End If
        End If


        tr.Commit()


        MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)


        If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
            If New_Entry = True Then
                new_record()
            Else
                move_record(lbl_ReceiptNo.Text)
            End If
        Else
            move_record(lbl_ReceiptNo.Text)
        End If

        'Catch ex As Exception
        '    tr.Rollback()
        '    MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'Finally
        '    If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        '    If Trim(UCase(vNewFrmTYpe)) = "COUNT" Then
        '        Dim f1 As New Count_Creation '(Cnt_ID)

        '        Common_Procedures.Master_Return.Form_Name = ""
        '        Common_Procedures.Master_Return.Control_Name = ""
        '        Common_Procedures.Master_Return.Return_Value = ""
        '        Common_Procedures.Master_Return.Master_Type = ""

        '        f1.MdiParent = MDIParent1
        '        f1.Show()

        '    ElseIf Trim(UCase(vNewFrmTYpe)) = "MILL" Then
        '        Dim f2 As New Mill_Creation '(Mil_ID)

        '        Common_Procedures.Master_Return.Form_Name = ""
        '        Common_Procedures.Master_Return.Control_Name = ""
        '        Common_Procedures.Master_Return.Return_Value = ""
        '        Common_Procedures.Master_Return.Master_Type = ""

        '        f2.MdiParent = MDIParent1
        '        f2.Show()

        '    End If

        'End Try

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
                    'LOCATION 10 LOT 11
                    .Rows(i).Cells(10).Value = cbo_Det_Location.Text
                    .Rows(i).Cells(11).Value = txt_DetLotNo.Text

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
                .Rows(n).Cells(10).Value = cbo_Det_Location.Text
                .Rows(n).Cells(11).Value = txt_DetLotNo.Text

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

        cbo_Det_Location.Text = ""
        txt_DetLotNo.Text = ""

        If cbo_CountName.Enabled And cbo_CountName.Visible Then
            cbo_CountName.Focus()
        End If


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
            Dim f As New Sizing_Count_Creation

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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Yarn_Receipt_Code IN (select z1.Yarn_Receipt_Code from SizingSoft_Yarn_Receipt_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ") "
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Yarn_Receipt_Code IN (select z2.Yarn_Receipt_Code from SizingSoft_Yarn_Receipt_Details z2 where z2.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Yarn_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Receipt_No", con)
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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

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

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim trans_id As Integer = 0

        If Trim(cbo_VehicleNo.Text) = "" And Trim(cbo_Transport.Text) <> "" Then

            trans_id = Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)

            Try

                If trans_id <> 0 Then
                    da1 = New SqlClient.SqlDataAdapter("select top 1 * from Yarn_Receipt_Head where Transport_IdNo = " & Str(Val(trans_id)) & " Order by Yarn_Receipt_Date desc, for_Orderby desc, Yarn_Receipt_No desc", con)
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

            cbo_Det_Location.Text = dgv_Details.CurrentRow.Cells(10).Value
            txt_DetLotNo.Text = dgv_Details.CurrentRow.Cells(11).Value

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
        cbo_Det_Location.Text = ""
        txt_DetLotNo.Text = ""

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
                    CurStk = Common_Procedures.get_Yarn_CurrentStock(con, Val(lbl_Company.Tag), vLedID, vCntID)
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

        Try
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
        Catch ex As Exception
            '---
        Finally
            Dt.Dispose()
            Da.Dispose()
        End Try




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
            If cbo_Det_Location.Visible Then
                cbo_Det_Location.Focus()
            Else
                btn_Add_Click(sender, e)
            End If

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, Nothing, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        If e.KeyValue = 38 Then
            If cbo_BeamWidth.Enabled And cbo_BeamWidth.Visible Then
                cbo_BeamWidth.Focus()
            ElseIf txt_EmptyBeam.Enabled And txt_EmptyBeam.Visible Then
                txt_EmptyBeam.Focus()
            Else
                cbo_coneType.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        End If

        If (e.KeyValue = 38 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_Type.Visible = True Then
                cbo_Type.Focus()
            ElseIf txt_LotNo.Visible = True Then
                txt_LotNo.Focus()
            Else
                txt_BookNo.Focus()
            End If
        End If

        'If e.KeyCode = 40 And cbo_Ledger.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
        '    If Trim(Common_Procedures.settings.CustomerCode) = "1112" Then '----VENUS SIZING
        '        If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
        '    Else
        '        txt_PartyDcNo.Focus()
        '    End If
        'End If

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        End If

        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(cbo_Type.Text) = "DELIVERY" Then
                Delivery_Selection()
                Panel2.Enabled = False
                Exit Sub

            Else
                Panel2.Enabled = True

            End If
        End If


        'If Asc(e.KeyChar) = 13 Then
        '    e.Handled = True
        '    If Trim(Common_Procedures.settings.CustomerCode) = "1112" Then '----VENUS SIZING
        '        If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
        '    Else
        '        txt_PartyDcNo.Focus()
        '    End If
        'End If
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CountName, dtp_Time, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
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
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_ReceivedBy, "", "", "", "", False)
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
        Print_PDF_Status = False
        print_record()



    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_YARN_RECEIPT, New_Entry) = False Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (Palladam)
            pnl_Print.Visible = True
            pnl_Back.Enabled = False
            If btn_Print_Preprint.Enabled And btn_Print_Preprint.Visible Then
                btn_Print_Preprint.Focus()
            End If

        ElseIf Val(Common_Procedures.settings.Dos_Printing) = 1 Then
            Pnl_DosPrint.Visible = True
            pnl_Back.Enabled = False
            If Btn_DosPrint.Enabled And Btn_DosPrint.Visible Then
                Btn_DosPrint.Focus()
            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Then '---- Prakash textile & sizing

            Dim mymsgbox As New Tsoft_MessageBox("Select Paper Size to Print", "A4,HALF-SHEET,CANCEL", "FOR DELIVERY PRINTING...", "IF A4 is selected, 2 copies of dc will be printed in single A4 sheet," & Chr(13) & "If HALF-SHEET is selected 1 copy of dc will be printed in 8x6 paper size", MesssageBoxIcons.Questions, 2)
            mymsgbox.ShowDialog()

            If mymsgbox.MessageBoxResult = 1 Then
                vPrnt_2Copy_In_SinglePage = 1
                Prnt_HalfSheet_STS = False

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

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

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



            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

            'PpSzSTS = False

            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
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

        End If


    End Sub
    Private Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize
        Dim inpno As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Yarn_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Somanur)
            inpno = InputBox("Enter No.of Copies", "FOR PRINTING...", 3)
            prn_TotCopies = Val(inpno)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (palladam)
            prn_TotCopies = 2
        End If
        If Val(prn_TotCopies) <= 0 Then
            Exit Sub
        End If

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1


        If Common_Procedures.settings.CustomerCode = "1288" Then
            PrintDocument1.DefaultPageSettings.Landscape = True
        Else
            PrintDocument1.DefaultPageSettings.Landscape = False
        End If




            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1034" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1033" Then


        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        'Debug.Print(ps.PaperName)
        '        If ps.Width = 800 And ps.Height = 600 Then
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        'Else


        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            Exit For
        '        End If
        '    Next


        'End If


        'If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
        '    Try
        '        If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
        '            PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
        '            If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '                PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        '                PrintDocument1.Print()
        '            End If
        '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1034" Then


        '                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                    'Debug.Print(ps.PaperName)
        '                    If ps.Width = 800 And ps.Height = 600 Then
        '                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                        PpSzSTS = True
        '                        Exit For
        '                    End If
        '                Next

        '                If PpSzSTS = False Then
        '                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                            PpSzSTS = True
        '                            Exit For
        '                        End If
        '                    Next

        '                    If PpSzSTS = False Then
        '                        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                                Exit For
        '                            End If
        '                        Next
        '                    End If

        '                End If
        '            Else
        '                If PpSzSTS = False Then
        '                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                            Exit For
        '                        End If
        '                    Next
        '                End If
        '            End If

        '        Else
        '            PrintDocument1.Print()

        '        End If


        '    Catch ex As Exception
        '        MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '    End Try


        'Else

        '    Try

        '        Dim ppd As New PrintPreviewDialog

        '        ppd.Document = PrintDocument1

        '        ppd.WindowState = FormWindowState.Maximized
        '        ppd.StartPosition = FormStartPosition.CenterScreen
        '        'ppd.ClientSize = New Size(600, 600)

        '        AddHandler ppd.Shown, AddressOf PrintPreview_Shown
        '        ppd.ShowDialog()
        '        'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '        '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
        '        '    ppd.ShowDialog()
        '        'End If

        '    Catch ex As Exception
        '        MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

        '    End Try

        'End If


        '-------------------------------------------


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Print_PDF_Status = True Then
                    '--This is actual & correct 

                    PrintDocument1.DocumentName = "Document"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Document.pdf"
                    PrintDocument1.Print()

                Else
                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    Exit For
                                End If
                            Next

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

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                'ppd.ClientSize = New Size(600, 600)
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0
        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Beam_Width_Name, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, Gh.Ledger_MainName as Godown_Name from Yarn_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = csh.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Ledger_Head Gh ON a.WareHouse_IdNo = Gh.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON c.ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_IdNo = d.Beam_Width_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Receipt_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                Printing_Format1(e)
            Else
                Printing_Format2(e)
            End If
        ElseIf Val(Common_Procedures.settings.YarnReceipt_Print_2Copy_In_SinglePage) = 1 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then
                Printing_Format1037(e)
            Else
                Printing_Format6(e)
            End If
        ElseIf Val(Common_Procedures.settings.Dos_Printing) = 1 Then
            If prn_Status = 1 Then
                Printing_Format3_DosPrint()
            Else
                Printing_Format1(e)
            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            Printing_Format4(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then
            Printing_Format_1288(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then
            Printing_Format1037(e)
            Else
            Printing_Format1(e)

        End If

    End Sub

    Private Sub Printing_Format_1288(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String
        Dim CtNm1 As String
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt As Integer = 0

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'PrntCnt = 1


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = PS
                e.PageSettings.PaperSize = PS
                Exit For
            End If
        Next


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            PS = PrintDocument1.PrinterSettings.PaperSizes(I)
            Debug.Print(PS.PaperName)
            If PS.Width = 800 And PS.Height = 600 Then
                PrintDocument1.DefaultPageSettings.PaperSize = PS
                e.PageSettings.PaperSize = PS
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        Exit For
                    End If
                Next
            End If



            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    Exit For
                End If
            Next

        End If

        'If PrntCnt2ndPageSTS = False Then
        '    PrntCnt = 2
        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10  ' 50 
            .Right = 50
            .Top = 10 '30
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

        If Common_Procedures.settings.CustomerCode = "1288" Then
            PrintDocument1.DefaultPageSettings.Landscape = True
        End If


        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        ' ''NoofItems_PerPage = 8 '4  '5 
        ' '' update for full page
        If Common_Procedures.settings.CustomerCode = "1288" Then  ' KKP 
            'NoofItems_PerPage = 45 '4  '5 
            NoofItems_PerPage = 18
        ElseIf Common_Procedures.settings.CustomerCode = "1044" Then '-------GANESH KARTHI TEXTILE PRIVATE LIMITED
            NoofItems_PerPage = 8 '4  '5 
        Else
            NoofItems_PerPage = 10 '4  '5 
        End If


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 45 : ClArr(3) = 250 : ClArr(4) = 170 : ClArr(5) = 65 : ClArr(6) = 70 : ClArr(7) = 65 : ClArr(8) = 50 : ClArr(9) = 62
        ClArr(10) = 100

        ClArr(11) = 95
        ClArr(12) = 95
        'ClArr(12) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height  ' 20
        If Common_Procedures.settings.CustomerCode = "1044" Then '-------GANESH KARTHI TEXTILE PRIVATE LIMITED
            TxtHgt = 18 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        Else
            TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader_1288(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth - 50, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format1_PageFooter_1288(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth - 50, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)
                                e.HasMorePages = True
                                Return
                            End If

                            prn_DetSNo = prn_DetSNo + 1

                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                            'ItmNm2 = ""
                            'If Len(ItmNm1) > 16 Then
                            '    For I = 16 To 1 Step -1
                            '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            '    Next I
                            '    If I = 0 Then I = 16
                            '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            'End If

                            CtNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString)
                            'CtmNm2 = ""
                            'If Len(CtNm1) > 8 Then
                            '    For I = 8 To 1 Step -1
                            '        If Mid$(Trim(CtNm1), I, 1) = " " Or Mid$(Trim(CtNm1), I, 1) = "," Or Mid$(Trim(CtNm1), I, 1) = "." Or Mid$(Trim(CtNm1), I, 1) = "-" Or Mid$(Trim(CtNm1), I, 1) = "/" Or Mid$(Trim(CtNm1), I, 1) = "_" Or Mid$(Trim(CtNm1), I, 1) = "(" Or Mid$(Trim(CtNm1), I, 1) = ")" Or Mid$(Trim(CtNm1), I, 1) = "\" Or Mid$(Trim(CtNm1), I, 1) = "[" Or Mid$(Trim(CtNm1), I, 1) = "]" Or Mid$(Trim(CtNm1), I, 1) = "{" Or Mid$(Trim(CtNm1), I, 1) = "}" Then Exit For
                            '    Next
                            '    If I = 0 Then I = 8
                            '    CtmNm2 = Microsoft.VisualBasic.Right(Trim(CtNm1), Len(CtNm1) - I)
                            '    CtNm1 = Microsoft.VisualBasic.Left(Trim(CtNm1), I - 1)
                            'End If


                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(CtNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("weight_Bag").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("weight_Bag").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Cone").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Cone").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                            End If


                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Lot_No").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Location_IdNo").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            'If Trim(ItmNm2) <> "" Or Trim(CtmNm2) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(CtmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If
                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                    Printing_Format1_PageFooter_1288(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth - 50, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                ' prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format1_PageHeader_1288(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurX As Single = 0
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Ledname1 As String
        Dim Ledname2 As String
        Dim i As Integer


        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Receipt_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Receipt_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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

        CurY = CurY + TxtHgt - 20
        p1Font = New Font("Calibri", 17, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
        End If
        CurY = CurY + strHeight - 3
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + strHeight - 3
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)


        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        Else
            CurY = CurY + TxtHgt - 3
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
            CurX = CurX + strWidth - 3
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX + 3, CurY, 0, 0, pFont)

            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurX = CurX + strWidth - 3
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth - 3
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 3
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt - 1

        End If

        CurY = CurY + TxtHgt - 15  '10
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN RECEIPT NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt
        CurY = CurY + strHeight - 5 ' + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("PARTY D.C.NO  : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

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


            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & Ledname1, LMargin + 10, CurY, 0, 0, p1Font)

            If Trim(Ledname2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(Ledname2), LMargin + S1 + 10, CurY, 0, 0, p1Font)
                'NoofDets = NoofDets + 1
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
                If Trim(prn_HdDt.Rows(0).Item("Lot_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lot_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            Else
                If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Through", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PARTY D.C.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 5
            If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
                If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " TIN NO.: " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
            Else
                If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                    pFont = New Font("Calibri", 11, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
            End If
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then '--KKP SPINNING MILLS PVT. LTD
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                If Trim(prn_HdDt.Rows(0).Item("Godown_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Location", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Godown_Name").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            pFont = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WT/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CNS/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WT/CN", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOTNO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(10), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOCATION", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(10), pFont)

            pFont = New Font("calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter_1288(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt + 2
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'CurY = CurY + TxtHgt
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 70, CurY, 2, ClAr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                End If
            End If




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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))

            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Remarks :" & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 350, CurY, 0, 0, pFont)
                If IsDBNull(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) = False Then
                    If Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, " Beam Width : " & Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString), PageWidth - 200, CurY, 0, 0, pFont)
                    End If
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

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
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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
        Dim ItmNm1 As String, ItmNm2 As String
        Dim I As Integer, NoofDets As Integer
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
            Common_Procedures.Print_To_PrintDocument(e, "YARN RECEIPT NOTE", CurX, CurY, 0, 0, p1Font)


            CurX = LMargin + 80
            CurY = TMargin + 140
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO : " & prn_HdDt.Rows(0).Item("Yarn_Receipt_No").ToString, CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 340
            Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Receipt_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, p1Font)

            time = TimeOfDay.ToString("h:mm:ss tt")

            CurX = LMargin + 580
            Common_Procedures.Print_To_PrintDocument(e, "TIME : " & (time), CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            CurX = LMargin + 60
            e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 730, CurY)

            CurX = LMargin + 65 ' 40  '150
            CurY = TMargin + 180 ' 122 ' 100
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "From M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, CurX + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, CurX + 20, CurY, 0, 0, pFont)
            End If

            CurX = LMargin + 300 ' 40  '150
            CurY = TMargin + 240 ' 122 ' 100
            Common_Procedures.Print_To_PrintDocument(e, "We have Received the following", CurX, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            CurX = LMargin + 60
            e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 730, CurY)

            CurX = LMargin + 65 ' 40  '150
            CurY = TMargin + 265 ' 122 ' 100
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Count", CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 180 ' 40  '150
            Common_Procedures.Print_To_PrintDocument(e, "Name of the Mill ", CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 440 ' 40  '150
            Common_Procedures.Print_To_PrintDocument(e, "Bags", CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 565 ' 40  '150
            Common_Procedures.Print_To_PrintDocument(e, "Cones", CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 675 ' 40  '150
            Common_Procedures.Print_To_PrintDocument(e, "Weight", CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            CurX = LMargin + 60
            e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 730, CurY)

            Try

                NoofDets = 0

                CurY = 275 ' 370

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 30 Then
                            For I = 6 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 30
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If


                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString), 65, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 185, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + 550, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), "########0"), LMargin + 660, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + 780, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Or Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), 115, CurY, 0, 0, pFont)


                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next


            CurY = TMargin + 390
            e.Graphics.DrawLine(Pens.Black, LMargin + 60, CurY, LMargin + 790, CurY)

            CurX = LMargin + 200
            CurY = TMargin + 400
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", CurX, CurY, 0, 0, pFont)

            CurX = LMargin + 550
            CurY = TMargin + 400

            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), "########0"), CurX, CurY, 1, 0, pFont)

            CurX = LMargin + 660

            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), "########0"), CurX, CurY, 1, 0, pFont)
            CurX = LMargin + 780
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "########0.000"), CurX, CurY, 1, 0, pFont)

            'CurY = TMargin + 440
            'e.Graphics.DrawLine(Pens.Black, LMargin + 60, CurY, LMargin + 790, CurY)

            CurY = TMargin + 440
            e.Graphics.DrawLine(Pens.Black, LMargin + 170, CurY, LMargin + 170, TMargin + 260)
            e.Graphics.DrawLine(Pens.Black, LMargin + 430, CurY, LMargin + 430, TMargin + 260)
            e.Graphics.DrawLine(Pens.Black, LMargin + 560, CurY, LMargin + 560, TMargin + 260)
            e.Graphics.DrawLine(Pens.Black, LMargin + 670, CurY, LMargin + 670, TMargin + 260)

            CurX = LMargin + 200
            CurY = TMargin + 450
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) & "    Duplicate for Book No . B1", CurX, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Public Sub Initialize_DosPrint()
        Dim PrnTxt As String = ""

        PrnTxt = ""
        PrnTxt = Chr(27) & "@" & ""      '---> initialise printer
        sw.WriteLine(PrnTxt)
        PrnTxt = Chr(18) & ""               '---> condensed Off
        sw.WriteLine(PrnTxt)
        PrnTxt = Chr(27) & "P" & ""          '---> 10 CPI
        sw.WriteLine(PrnTxt)
        'Print #1, Chr(27); "x0";        '---> Draft   (or)  'Print #1, Chr(27); "%0"        '---> Draft - not confirmed (have to check)
        PrnTxt = Chr(27) & "t1" & ""         '---> Character set "Graphics"
        sw.WriteLine(PrnTxt)
        PrnTxt = Chr(27) & "2" & ""          '---> Line Spacing 1/6 (6 lines per inch)
        sw.WriteLine(PrnTxt)
        PrnTxt = Chr(27) & "x0" & ""        '---> Draft   (or)  'Print #1, Chr(27); "%0"        '---> Draft - not confirmed (have to check)
        sw.WriteLine(PrnTxt)

        ' PrnTxt = Chr(27) & "@" & Chr(18) & Chr(27) & "P" & Chr(27) & "t1" & Chr(27) & "2" & Chr(27) & "x0"
    End Sub

    Private Sub Get_DosLineDetails()
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

    Private Sub Printing_Format3_DosPrint()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Beam_Width_Name from Yarn_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_IdNo = d.Beam_Width_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Receipt_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


        Get_DosLineDetails()

        LnCnt = 0

        fs = New FileStream(Common_Procedures.Dos_Printing_FileName_Path, FileMode.Create)
        sw = New StreamWriter(fs, System.Text.Encoding.Default)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_DosPrint_PageHeader(LnCnt)

                prn_DetIndx = 0
                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        PrnTxt = Chr(Vz1) & Space(1) & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString) & Space(5 - Len(Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString)))) & Chr(Vz2) & Space(1) & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString) & Space(7 - Len(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString))) & Chr(Vz2) & Space(1) & Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString, 22)) & Space(22 - Len(Microsoft.VisualBasic.Left(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString), 22))) & Chr(Vz2) & Space(1) & Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, 6)) & Space(6 - Len(Microsoft.VisualBasic.Left(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString), 6))) & Chr(Vz2) & Space(7 - Len(Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString)))) & Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) & Space(1) & Chr(Vz2) & Space(8 - Len(Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString)))) & Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) & Space(1) & Chr(Vz2) & Space(10 - Len(Trim(Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000")))) & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000") & Space(1) & Chr(Vz1)
                        sw.WriteLine(PrnTxt)

                        LnCnt = LnCnt + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format3_DosPrint_PageFooter(LnCnt)

                'w.Close()
                'w.Dispose()

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

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            sw.Close()
            fs.Close()
            sw.Dispose()
            fs.Dispose()


        End Try

    End Sub

    Public Sub Printing_Format3_DosPrint_PageHeader(ByRef LnCnt As Integer)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim PrnTxt As String = ""

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

            PrnTxt = Chr(27) & "@" & Chr(18) & Chr(27) & "P" & Chr(27) & "t1" & Chr(27) & "2" & Chr(27) & "x0"
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Trim(ChrW(Corn1)) & StrDup(78, Chr(Hz1)) & ChrW(Corn2)
            sw.WriteLine(PrnTxt)
            'PrnTxt = Chr(Corn1) & StrDup(78, Chr(Hz1)) & Chr(Corn2)
            'sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(39 - Len(Trim(Cmp_Name))) & Chr(14) & Chr(27) & "E" & Trim(Cmp_Name) & Chr(27) & "F" & Chr(20) & Space(39 - Len(Trim(Cmp_Name))) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            If Len(Trim(Cmp_Add1 & " " & Cmp_Add2)) > 78 Then
                PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1) & Chr(13) & Space(2) & Chr(15) & Space(65 - ((Len((Cmp_Add1) & " " & (Cmp_Add2)) / 2) + 0.1)) & Trim(Cmp_Add1 & " " & Cmp_Add2) & Chr(18)
                sw.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
                PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1) & Chr(13) & Space(2) & Chr(15) & Space(65 - ((Len((Cmp_Add3) & " " & (Cmp_Add4)) / 2) + 0.1)) & Trim(Cmp_Add3 & " " & Cmp_Add4) & Chr(18)
                sw.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1

            Else
                PrnTxt = Chr(Vz1) & Space(39 - ((Len((Cmp_Add1) & " " & (Cmp_Add2)) / 2) + 0.1)) & Trim(Cmp_Add1 & " " & Cmp_Add2) & Space(39 - ((Len(Cmp_Add1 & " " & Cmp_Add2) / 2) + 0.1)) & Space(Len(Cmp_Add1 & " " & Cmp_Add2) Mod 2) & Chr(Vz1)
                sw.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
                PrnTxt = Chr(Vz1) & Space(39 - ((Len((Cmp_Add3) & " " & (Cmp_Add4)) / 2) + 0.1)) & Trim(Cmp_Add3 & " " & Cmp_Add4) & Space(39 - ((Len(Cmp_Add3 & " " & Cmp_Add4) / 2) + 0.1)) & Space(Len(Cmp_Add3 & " " & Cmp_Add4) Mod 2) & Chr(Vz1)
                sw.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1

            End If

            PrnTxt = Chr(Vz1) & Space(35 - Math.Round((Len(Cmp_PhNo) / 2) + 0.1)) & "Phone : " & Trim(Cmp_PhNo) & Space(35 - Math.Round((Len(Cmp_PhNo) / 2) + 0.1)) & Space(Len(Cmp_PhNo) Mod 2) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(22) & Chr(14) & Chr(27) & "E" & "YARN RECEIPT NOTE" & Chr(27) & "F" & Chr(20) & Space(22) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(LfCon) & StrDup(39, Chr(Hz2)) & Chr(194) & StrDup(38, Chr(Hz2)) & Chr(RgtCon)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(1) & "From : " & Space(31) & Chr(Vz2) & Space(38) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & "M/s." & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString) & Space(31 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString))) & Chr(Vz2) & Space(1) & "REC NO : " & Trim(prn_HdDt.Rows(0).Item("Yarn_Receipt_No").ToString) & Space(28 - Len(Trim(prn_HdDt.Rows(0).Item("Yarn_Receipt_No").ToString))) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString))) & Chr(Vz2) & Space(38) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString))) & Chr(Vz2) & Space(1) & "DATE   : " & Trim(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Receipt_Date").ToString), "dd-MM-yyyy").ToString) & Space(28 - Len(Trim(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Receipt_Date").ToString), "dd-MM-yyyy").ToString))) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString))) & Chr(Vz2) & Space(38) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString))) & Chr(Vz2) & Space(1) & "PARTY DC.NO : " & Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) & Space(23 - Len(Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString))) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            'SUB HEADING
            PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(194) & StrDup(8, Chr(Hz2)) & Chr(194) & StrDup(23, Chr(Hz2)) & Chr(197) & StrDup(7, Chr(Hz2)) & Chr(194) & StrDup(8, Chr(Hz2)) & Chr(194) & StrDup(9, Chr(Hz2)) & Chr(194) & StrDup(11, Chr(Hz2)) & Chr(RgtCon)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & " S.No " & Chr(Vz2) & "  TYPE  " & Chr(Vz2) & "       MILL NAME       " & Chr(Vz2) & " COUNT " & Chr(Vz2) & "  BAGS  " & Chr(Vz2) & "  CONES  " & Chr(Vz2) & "   WEIGHT  " & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(197) & StrDup(8, Chr(Hz2)) & Chr(197) & StrDup(23, Chr(Hz2)) & Chr(197) & StrDup(7, Chr(Hz2)) & Chr(197) & StrDup(8, Chr(Hz2)) & Chr(197) & StrDup(9, Chr(Hz2)) & Chr(197) & StrDup(11, Chr(Hz2)) & Chr(RgtCon)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

        Catch ex As Exception
            sw.Close()
            sw.Dispose()
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_DosPrint_PageFooter(ByRef LnCnt As Integer)
        Dim EBm_Txt As String = ""
        Dim EBm_Wdth As String = ""
        Dim Cmp_Name As String = ""
        Dim PrnTxt As String = ""

        Try

            For J = prn_DetIndx + 1 To 5
                PrnTxt = Chr(Vz1) & Space(6) & Chr(Vz2) & Space(8) & Chr(Vz2) & Space(23) & Chr(Vz2) & Space(7) & Chr(Vz2) & Space(8) & Chr(Vz2) & Space(9) & Chr(Vz2) & Space(11) & Chr(Vz1)
                sw.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            Next J


            PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(197) & StrDup(8, Chr(Hz2)) & Chr(197) & StrDup(23, Chr(Hz2)) & Chr(197) & StrDup(7, Chr(Hz2)) & Chr(197) & StrDup(8, Chr(Hz2)) & Chr(197) & StrDup(9, Chr(Hz2)) & Chr(197) & StrDup(11, Chr(Hz2)) & Chr(RgtCon)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(6) & Chr(Vz2) & Space(8) & Chr(Vz2) & " TOTAL" & Space(17) & Chr(Vz2) & Space(7) & Chr(Vz2) & Space(7 - Len(Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString))) & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) & Space(1) & Chr(Vz2) & Space(8 - Len(Trim(prn_HdDt.Rows(0).Item("Total_Cones").ToString))) & Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) & Space(1) & Chr(Vz2) & Space(10 - Len(Trim(Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000")))) & Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000") & Space(1) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(193) & StrDup(8, Chr(Hz2)) & Chr(193) & StrDup(23, Chr(Hz2)) & Chr(193) & StrDup(7, Chr(Hz2)) & Chr(193) & StrDup(8, Chr(Hz2)) & Chr(193) & StrDup(9, Chr(Hz2)) & Chr(193) & StrDup(11, Chr(Hz2)) & Chr(RgtCon)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            EBm_Txt = "" : EBm_Wdth = ""
            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then EBm_Txt = "EMPTY BEAM : " & Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) <> "" Then EBm_Wdth = "BEAM WIDTH  : " & Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString)

            PrnTxt = Chr(Vz1) & Space(1) & "Through     : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) & Space(36 - Len(Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString))) & Space(2) & Trim(EBm_Txt) & Space(25 - Len(Trim(EBm_Txt))) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(1) & Trim(EBm_Wdth) & Space(75 - Len(Trim(EBm_Wdth))) & Space(2) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(LfCon) & StrDup(78, Chr(Hz2)) & Chr(RgtCon)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            PrnTxt = Chr(Vz1) & " Receiver Signature   " & "Prepared By " & Space(39 - Len(Microsoft.VisualBasic.Left(Trim(Cmp_Name), 39))) & "For " & Cmp_Name & Space(1) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Corn3) & StrDup(78, Chr(Hz1)) & Chr(Corn4)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            For I = LnCnt + 1 To 36
                PrnTxt = ""
                sw.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            Next

        Catch ex As Exception
            sw.Close()
            sw.Dispose()
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus

        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 or (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        End If


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

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from SizingSoft_Yarn_Receipt_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Yarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from SizingSoft_Yarn_Receipt_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Yarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

            If Common_Procedures.settings.CustomerCode = "1102" Then
                Sms_Entry.vSmsPhoneNo = Trim(PhNo) & "," & "9361188135"
            Else
                Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            End If


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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_bagType, Nothing, cbo_coneType, "Bag_Type_Head", "Bag_Type_Name", "", "(Bag_Type_Idno = 0)")
        If e.KeyCode = 38 And cbo_bagType.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
            If cbo_godown.Visible Then
                cbo_godown.Focus()
            Else
                txt_PartyDcNo.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_bagType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_bagType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_bagType, cbo_coneType, "Bag_Type_Head", "Bag_Type_Name", "", "(Bag_Type_Idno = 0)")
    End Sub

    Private Sub cbo_bagType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_bagType.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_coneType, cbo_bagType, dtp_Time, "ConeType_Head", "Conetype_Name", "", "(ConeType_Idno = 0)")
    End Sub

    Private Sub cbo_coneType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_coneType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_coneType, dtp_Time, "ConeType_Head", "Conetype_Name", "", "(ConeType_Idno = 0)")
    End Sub

    Private Sub cbo_coneType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_coneType.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New ConeType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_coneType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_Close_DosPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_DosPrint.Click
        pnl_Back.Enabled = True
        Pnl_DosPrint.Visible = False
    End Sub

    Private Sub Btn_DosCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_DosCancel.Click
        btn_Close_DosPrint_Click(sender, e)
    End Sub

    Private Sub Btn_DosPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_DosPrint.Click
        prn_Status = 1
        Printing_Format3_DosPrint()
        btn_Close_DosPrint_Click(sender, e)
    End Sub

    Private Sub Btn_LaserPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_LaserPrint.Click
        prn_Status = 2
        printing_invoice()
        btn_Close_DosPrint_Click(sender, e)
    End Sub

    Private Sub btn_UserModification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            If IsNothing(sqltr) = False Then
                cmd.Transaction = sqltr
            End If

            vPrnSTS = 0
            If chk_Printed.Checked = True Then
                vPrnSTS = 1
            End If

            cmd.CommandText = "Update Yarn_Receipt_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
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

    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim CntNm1 As String, CntNm2 As String
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            PS = PrintDocument1.PrinterSettings.PaperSizes(I)
            Debug.Print(PS.PaperName)
            If PS.Width = 800 And PS.Height = 600 Then
                PrintDocument1.DefaultPageSettings.PaperSize = PS
                e.PageSettings.PaperSize = PS
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = PS
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    PpSzSTS = True
                    Exit For
                End If
            Next
            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            '        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
            '        PrintDocument1.DefaultPageSettings.PaperSize = PS
            '        e.PageSettings.PaperSize = PS
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        Exit For
                    End If
                Next
            End If

        End If

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = PS
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

        pFont = New Font("Calibri", 7, FontStyle.Regular)

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

        NoofItems_PerPage = 16  '5 

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(25) : ClArr(2) = 25 : ClArr(3) = 105 : ClArr(4) = 95 : ClArr(5) = 45 : ClArr(6) = 45 : ClArr(7) = 44 : ClArr(8) = 30 : ClArr(9) = 40
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height  ' 20
        TxtHgt = 18.7 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format4_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            prn_DetSNo = prn_DetSNo + 1

                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 10 Then
                                For I = 18 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 10
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If


                            CntNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString)
                            CntNm2 = ""
                            If Len(CntNm1) > 10 Then
                                For I = 18 To 1 Step -1
                                    If Mid$(Trim(CntNm1), I, 1) = " " Or Mid$(Trim(CntNm1), I, 1) = "," Or Mid$(Trim(CntNm1), I, 1) = "." Or Mid$(Trim(CntNm1), I, 1) = "-" Or Mid$(Trim(CntNm1), I, 1) = "/" Or Mid$(Trim(CntNm1), I, 1) = "_" Or Mid$(Trim(CntNm1), I, 1) = "(" Or Mid$(Trim(CntNm1), I, 1) = ")" Or Mid$(Trim(CntNm1), I, 1) = "\" Or Mid$(Trim(CntNm1), I, 1) = "[" Or Mid$(Trim(CntNm1), I, 1) = "]" Or Mid$(Trim(CntNm1), I, 1) = "{" Or Mid$(Trim(CntNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 10
                                CntNm2 = Microsoft.VisualBasic.Right(Trim(CntNm1), Len(CntNm1) - I)
                                CntNm1 = Microsoft.VisualBasic.Left(Trim(CntNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(CntNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("weight_Bag").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("weight_Bag").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Cone").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Cone").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                            End If
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), PageWidth, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CntNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If


                    Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        'e.HasMorePages = False
        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                ' prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format4_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim pFontBold As Font = New Font("Calibri", 8, FontStyle.Bold)
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_Add3 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurX As Single = 0
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Ledname1 As String
        Dim Ledname2 As String
        Dim i As Integer


        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Receipt_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Receipt_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = ""
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
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
                Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
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

        CurY = CurY + TxtHgt - 10
        'p1Font = New Font("Calibri", 18, FontStyle.Bold)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            p1Font = New Font("Calibri", 8.5, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin, CurY, 2, PrintWidth, p1Font)

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, p1Font)

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

            CurY = CurY + TxtHgt - 1

        End If

        CurY = CurY + TxtHgt - 15  '10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN RECEIPT NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 5 ' + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            'C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("PARTY D.C.NO  : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

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


            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & Ledname1, LMargin + 10, CurY, 0, 0, p1Font)

            If Trim(Ledname2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(Ledname2), LMargin + S1 + 10, CurY, 0, 0, p1Font)
                'NoofDets = NoofDets + 1
            End If

            p1Font = New Font("calibri", 10, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 9, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME ", LMargin + C1 + 10, CurY, 0, 0, pFontBold)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFontBold)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFontBold)

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFontBold)




            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)



            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
                If Trim(prn_HdDt.Rows(0).Item("Lot_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lot_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            Else
                If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            End If


            If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PARTY D.C.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
                If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " TIN NO.: " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
            Else
                If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
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
            Common_Procedures.Print_To_PrintDocument(e, "WT/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CNS/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WT/CN", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format4_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim Cmp_UserName As String = "", Cmp_Divi As String = ""

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 8, FontStyle.Bold)

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 2, ClAr(4), p1Font)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth, CurY, 1, 0, p1Font)
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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 9

            'Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Remarks : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 350, CurY, 0, 0, pFont)
                If IsDBNull(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) = False Then
                    If Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, " Beam Width : " & Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString), PageWidth - 200, CurY, 0, 0, pFont)
                    End If
                End If
            End If

            CurY = CurY + TxtHgt + 8
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY


            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt - 5
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Cmp_UserName = Trim(Common_Procedures.User.Name)
                ' Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            Cmp_Divi = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Divi, PageWidth - 60, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt - 3
            Common_Procedures.Print_To_PrintDocument(e, Cmp_UserName, PageWidth - 60, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 15

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
                Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 180, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            End If



            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format6(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt As Integer = 0

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        PrntCnt = 1

        If Val(Common_Procedures.settings.YarnReceipt_Print_2Copy_In_SinglePage) = 1 Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    Exit For
                End If
            Next

        Else
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                Debug.Print(PS.PaperName)
                If PS.Width = 800 And PS.Height = 600 Then
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                            PrintDocument1.DefaultPageSettings.PaperSize = PS
                            e.PageSettings.PaperSize = PS
                            Exit For
                        End If
                    Next
                End If

            End If

            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
            '        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
            '        PrintDocument1.DefaultPageSettings.PaperSize = PS
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
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 4  '5 

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 55 : ClArr(3) = 158 : ClArr(4) = 70 : ClArr(5) = 65 : ClArr(6) = 70 : ClArr(7) = 65 : ClArr(8) = 60 : ClArr(9) = 62
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height  ' 20
        TxtHgt = 18.7 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.YarnReceipt_Print_2Copy_In_SinglePage) = 1 Then
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




            EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format6_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                    Try


                        NoofDets = 0

                        CurY = CurY - 10

                        If prn_DetDt.Rows.Count > 0 Then

                            Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                                If NoofDets >= NoofItems_PerPage Then

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format6_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)
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
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("weight_Bag").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("weight_Bag").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                End If
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                End If
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Cone").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Cone").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                End If
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                End If
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                prn_DetIndx = prn_DetIndx + 1

                            Loop

                        End If


                        Printing_Format6_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


                    Catch ex As Exception

                        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    End Try

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

            If Val(Common_Procedures.settings.YarnReceipt_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count = Cnt + 10 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Cnt = 10
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt

LOOP2:

        'e.HasMorePages = False
        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                ' prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format6_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurX As Single = 0
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Ledname1 As String
        Dim Ledname2 As String
        Dim i As Integer


        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Receipt_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Receipt_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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


        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

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

            CurY = CurY + TxtHgt - 1

        End If

        CurY = CurY + TxtHgt - 15  '10
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN RECEIPT NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 5 ' + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("PARTY D.C.NO  : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

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


            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & Ledname1, LMargin + 10, CurY, 0, 0, p1Font)

            If Trim(Ledname2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(Ledname2), LMargin + S1 + 10, CurY, 0, 0, p1Font)
                'NoofDets = NoofDets + 1
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)




            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)



            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
                If Trim(prn_HdDt.Rows(0).Item("Lot_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lot_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            Else
                If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Through", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If


            If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PARTY D.C.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
                If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " TIN NO.: " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
            Else
                If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
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
            Common_Procedures.Print_To_PrintDocument(e, "WT/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CNS/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WT/CN", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format6_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 2, ClAr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 9

            'Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Remarks : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 350, CurY, 0, 0, pFont)
                If IsDBNull(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) = False Then
                    If Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, " Beam Width : " & Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString), PageWidth - 200, CurY, 0, 0, pFont)
                    End If
                End If
            End If

            CurY = CurY + TxtHgt + 8
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

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
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Det_Location_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Det_Location.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Det_Location_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Det_Location.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Det_Location.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Det_Location_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Det_Location.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Det_Location, txt_DetLotNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Det_Location_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Det_Location.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Det_Location, txt_DetLotNo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub txt_DetLotNo_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DetLotNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub



    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim CtNm1 As String, CtmNm2 As String
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt As Integer = 0

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'PrntCnt = 1



        If Common_Procedures.settings.CustomerCode = "1038" Or Common_Procedures.settings.CustomerCode = "1037" Then
            PrntCnt = 1
            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                If PrntCnt2ndPageSTS = False Then
                    PrntCnt = 2
                End If
            End If

            set_PaperSize_For_PrintDocument1()
        Else
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    Exit For
                End If
            Next


            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                Debug.Print(PS.PaperName)
                If PS.Width = 800 And PS.Height = 600 Then
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                            PrintDocument1.DefaultPageSettings.PaperSize = PS
                            e.PageSettings.PaperSize = PS
                            Exit For
                        End If
                    Next
                End If



                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        Exit For
                    End If
                Next

            End If
        End If




        'If PrntCnt2ndPageSTS = False Then
        '    PrntCnt = 2
        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10  ' 50 
            .Right = 50
            .Top = 10 '30
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
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        ' ''NoofItems_PerPage = 8 '4  '5 
        ' '' update for full page
        If Common_Procedures.settings.CustomerCode = "1288" Then  ' KKP 
            'NoofItems_PerPage = 45 '4  '5 
            NoofItems_PerPage = 38
        ElseIf Common_Procedures.settings.CustomerCode = "1044" Then '-------GANESH KARTHI TEXTILE PRIVATE LIMITED
            NoofItems_PerPage = 8 '4  '5 
        Else
            NoofItems_PerPage = 10 '4  '5 
        End If


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 45 : ClArr(3) = 140 : ClArr(4) = 120 : ClArr(5) = 65 : ClArr(6) = 70 : ClArr(7) = 65 : ClArr(8) = 50 : ClArr(9) = 62
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height  ' 20
        If Common_Procedures.settings.CustomerCode = "1044" Then '-------GANESH KARTHI TEXTILE PRIVATE LIMITED
            TxtHgt = 18 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        Else
            TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

        'PrntCnt2ndPageSTS = False
        'TpMargin = TMargin

        'For PCnt = 1 To PrntCnt
        '    If Val(Common_Procedures.settings.YarnReceipt_Print_2Copy_In_SinglePage) = 1 Then
        '        If PCnt = 1 Then


        '            prn_PageNo1 = prn_PageNo

        '            prn_DetIndx1 = prn_DetIndx
        '            prn_DetSNo1 = prn_DetSNo
        '            prn_NoofBmDets1 = prn_NoofBmDets
        '            TpMargin = TMargin

        '        Else

        '            prn_PageNo = prn_PageNo1
        '            prn_NoofBmDets = prn_NoofBmDets1
        '            prn_DetIndx = prn_DetIndx1
        '            prn_DetSNo = prn_DetSNo1

        '            TpMargin = 560 + TMargin  ' 600 + TMargin

        '        End If


        '    End If




        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                Try


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
                            If Len(ItmNm1) > 16 Then
                                For I = 16 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 16
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            CtNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString)
                            CtmNm2 = ""
                            If Len(CtNm1) > 8 Then
                                For I = 8 To 1 Step -1
                                    If Mid$(Trim(CtNm1), I, 1) = " " Or Mid$(Trim(CtNm1), I, 1) = "," Or Mid$(Trim(CtNm1), I, 1) = "." Or Mid$(Trim(CtNm1), I, 1) = "-" Or Mid$(Trim(CtNm1), I, 1) = "/" Or Mid$(Trim(CtNm1), I, 1) = "_" Or Mid$(Trim(CtNm1), I, 1) = "(" Or Mid$(Trim(CtNm1), I, 1) = ")" Or Mid$(Trim(CtNm1), I, 1) = "\" Or Mid$(Trim(CtNm1), I, 1) = "[" Or Mid$(Trim(CtNm1), I, 1) = "]" Or Mid$(Trim(CtNm1), I, 1) = "{" Or Mid$(Trim(CtNm1), I, 1) = "}" Then Exit For
                                Next
                                If I = 0 Then I = 8
                                CtmNm2 = Microsoft.VisualBasic.Right(Trim(CtNm1), Len(CtNm1) - I)
                                CtNm1 = Microsoft.VisualBasic.Left(Trim(CtNm1), I - 1)
                            End If


                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(CtNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("weight_Bag").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("weight_Bag").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Cone").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Cone").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                            End If
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Or Trim(CtmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CtmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        '        If Val(Common_Procedures.settings.YarnReceipt_Print_2Copy_In_SinglePage) = 1 Then

        '            If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
        '                If prn_DetDt.Rows.Count = Cnt + 10 Then
        '                    PrntCnt2ndPageSTS = True
        '                    e.HasMorePages = True
        '                    Cnt = 10
        '                    Return
        '                End If
        '            End If
        '        End If

        '        PrntCnt2ndPageSTS = False

        '        Next PCnt

        'LOOP2:

        'e.HasMorePages = False
        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                ' prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurX As Single = 0
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Ledname1 As String
        Dim Ledname2 As String
        Dim i As Integer


        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Receipt_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Receipt_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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

        CurY = CurY + TxtHgt - 20
        p1Font = New Font("Calibri", 17, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
        End If
        CurY = CurY + strHeight - 3
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + strHeight - 3
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)


        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        Else
            CurY = CurY + TxtHgt - 3
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
            CurX = CurX + strWidth - 3
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX + 3, CurY, 0, 0, pFont)

            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurX = CurX + strWidth - 3
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth - 3
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 3
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt - 1

        End If

        CurY = CurY + TxtHgt - 15  '10
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN RECEIPT NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt
        CurY = CurY + strHeight - 5 ' + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("PARTY D.C.NO  : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

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


            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & Ledname1, LMargin + 10, CurY, 0, 0, p1Font)

            If Trim(Ledname2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(Ledname2), LMargin + S1 + 10, CurY, 0, 0, p1Font)
                'NoofDets = NoofDets + 1
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
                If Trim(prn_HdDt.Rows(0).Item("Lot_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lot_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            Else
                If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Through", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PARTY D.C.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 5
            If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
                If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " TIN NO.: " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
            Else
                If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                    pFont = New Font("Calibri", 11, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
            End If
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then '--KKP SPINNING MILLS PVT. LTD
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                If Trim(prn_HdDt.Rows(0).Item("Godown_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Location", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Godown_Name").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            pFont = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WT/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CNS/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WT/CN", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            pFont = New Font("calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
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

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt + 2
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            'CurY = CurY + TxtHgt
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 80, CurY, 2, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Remarks :" & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 350, CurY, 0, 0, pFont)
                If IsDBNull(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) = False Then
                    If Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, " Beam Width : " & Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString), PageWidth - 200, CurY, 0, 0, pFont)
                    End If
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

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
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Delivery_Selection()
        Dim CMD As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer, Ledger_Party_idno As Integer
        Dim Led_IdNo As Integer
        Dim NewCode As String = ""
        Dim CompIDCondt As String = ""
        Dim RcptBm_PavuInc As Integer
        Dim vjoinTYP As String


        If Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then Exit Sub

        CMD.Connection = con

        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_IdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.Selection_CompanyIdno = " & Str(Val(lbl_Company.Tag)) & ")"

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then


            With dgv_delivery_Selections

                .Rows.Clear()
                SNo = 0

                For i = 1 To 2

                    If i = 1 Then
                        '---editing
                        Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No, a.for_OrderBy, a.Reference_Date, a.Total_Bags as Bags , a.Total_Cones as Cones, a.Total_Weight as Weight from Yarn_Delivery_Selections_Processing_Details a where  a.Selection_CompanyIdno = " & Str(Val(lbl_Company.Tag)) & " and ( a.Selection_ReceivedFromIdNo = " & Str(Val(Led_IdNo)) & " OR a.Selection_ledgerIdno = " & Str(Val(Led_IdNo)) & " ) and a.Delivery_Code = a.reference_code and a.Total_Weight > 0 and a.Delivery_Code IN (Select sq1.Delivery_Code from Yarn_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' )  ", con)
                        'Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No,  a.Total_Bags as Bags , a.Total_Cones as Cones, a.Total_Weight as Weight from Yarn_Delivery_Selections_Processing_Details a where   a.Selection_Ledgeridno =" & Str(Val(Led_IdNo)) & " and " & CompIDCondt & " and a.Delivery_Code = a.reference_code and a.Total_bags > 0 and a.Total_Weight > 0 and a.Delivery_Code IN (Select sq1.Delivery_Code from Yarn_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' )  ", con)
                    Else
                        'new entry
                        CMD.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                        CMD.ExecuteNonQuery()

                        Common_Procedures.get_YarnDelivery_Selection_Processing_Pending(con)

                        'new entry
                        Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No, a.for_OrderBy, a.Reference_Date, tET.meters1 as Bags , tET.int2 as Cones, tET.weight3 as Weight from Yarn_Delivery_Selections_Processing_Details a INNER JOIN " & Trim(Common_Procedures.EntryTempTable) & " tET ON tET.Name1 <> '' and tET.weight3 <> 0 and a.Delivery_Code = tET.Name1 where a.Selection_CompanyIdno = " & Str(Val(lbl_Company.Tag)) & " and (a.Selection_ReceivedFromIdNo = " & Str(Val(Led_IdNo)) & " OR a.Selection_ledgerIdno = " & Str(Val(Led_IdNo)) & " ) and a.Total_Weight > 0 Order by a.Reference_Date DESC, a.for_OrderBy DESC, a.Delivery_Code DESC, a.Delivery_No DESC", con)
                        'Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No,  SUM(a.Total_Bags) as Bags , SUM(a.Total_Cones) as Cones, SUM(a.Total_Weight) as Weight from Yarn_Delivery_Selections_Processing_Details a where   a.Selection_Ledgeridno =" & Str(Val(Led_IdNo)) & " and " & CompIDCondt & " and a.Delivery_Code NOT IN (Select sq1.Delivery_Code from Yarn_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' ) Group by a.Delivery_Code, a.Delivery_No Having Sum(a.Total_bags) > 0  and  sum(a.Total_Weight) > 0 ", con)
                    End If


                    Dt2 = New DataTable


                    Da2.Fill(Dt2)

                    If Dt2.Rows.Count > 0 Then

                        For k = 0 To Dt2.Rows.Count - 1

                            If Val(Dt2.Rows(k).Item("Weight").ToString) > 0 Then

                                SNo = SNo + 1
                                n = .Rows.Add()

                                .Rows(n).Cells(0).Value = Val(SNo)
                                .Rows(n).Cells(1).Value = Dt2.Rows(k).Item("Delivery_No").ToString
                                .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt2.Rows(k).Item("Reference_Date").ToString), "dd-MM-yyyy")
                                .Rows(n).Cells(3).Value = Dt2.Rows(k).Item("Delivery_No").ToString
                                .Rows(n).Cells(4).Value = Dt2.Rows(k).Item("Bags").ToString
                                .Rows(n).Cells(5).Value = Dt2.Rows(k).Item("Cones").ToString
                                .Rows(n).Cells(6).Value = Dt2.Rows(k).Item("Weight").ToString
                                .Rows(n).Cells(8).Value = Trim(Dt2.Rows(k).Item("Delivery_Code").ToString)

                                If i = 1 Then

                                    .Rows(n).Cells(7).Value = 1
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(k).Cells(j).Style.ForeColor = Color.Red
                                    Next

                                Else
                                    .Rows(n).Cells(7).Value = ""
                                    'For j = 0 To .ColumnCount - 1
                                    '    .Rows(k).Cells(j).Style.ForeColor = Color.Black
                                    'Next

                                End If


                            End If
                        Next


                    End If
                    Dt2.Clear()

                Next

            End With


        End If
        pnl_Delivery_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_delivery_Selections.Focus()

    End Sub

    Private Sub Close_Delivery_Selection()
        Dim CMD As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, n As Integer
        Dim sno As Integer = 0
        Dim Clo_IdNo As Integer = 0

        CMD.Connection = con

        sno = 0

        Clo_IdNo = 0

        lbl_Delivery_Code.Text = ""
        txt_PartyDcNo.Text = ""

        dgv_Details.Rows.Clear()

        For k = 0 To dgv_delivery_Selections.RowCount - 1

            If Val(dgv_delivery_Selections.Rows(k).Cells(7).Value) = 1 Then

                lbl_Delivery_Code.Text = Trim(dgv_delivery_Selections.Rows(k).Cells(8).Value)
                txt_PartyDcNo.Text = Trim(dgv_delivery_Selections.Rows(k).Cells(1).Value)

                Da = New SqlClient.SqlDataAdapter("Select isnull(b.ledger_name,'') as deliveryatname from Yarn_Delivery_Selections_Processing_Details a LEFT OUTER JOIN ledger_head b ON b.ledger_idno <> 0 and b.ledger_idno = a.DeliveryAt_Idno Where a.Reference_Code = '" & Trim(dgv_delivery_Selections.Rows(k).Cells(8).Value) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If Trim(Dt1.Rows(0).Item("deliveryatname").ToString) <> "" Then
                        txt_Remarks.Text = Dt1.Rows(0).Item("deliveryatname").ToString
                    End If
                End If
                Dt1.Clear()


                CMD.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                CMD.ExecuteNonQuery()

                Common_Procedures.get_YarnDelivery_Selection_Processing_Details(con, Trim(dgv_delivery_Selections.Rows(k).Cells(8).Value))

                Da = New SqlClient.SqlDataAdapter("Select int1 as Sl_No, Name1 as countname, name2 as Yarn_type, name3 as millname, meters1 as Bags, int2 as Cones, weight3 as Weight, weight4 as Thiri from " & Trim(Common_Procedures.EntryTempTable) & " Order by int1", con)
                'Da = New SqlClient.SqlDataAdapter("Select a.* from Sizing_Yarn_Delivery_Details a   where 'SYNDC-'+  a.Sizing_Yarn_Delivery_code =  '" & Trim(dgv_delivery_Selections.Rows(k).Cells(8).Value) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                With dgv_Details

                    If Val(Dt1.Rows.Count <> 0) Then

                        For i = 0 To Dt1.Rows.Count - 1

                            n = dgv_Details.Rows.Add()

                            .Rows(n).Cells(0).Value = i + 1  ' Trim(Dt1.Rows(i).Item("Sl_No").ToString)
                            .Rows(n).Cells(1).Value = Trim(Dt1.Rows(i).Item("countname").ToString) ' Common_Procedures.Count_IdNoToName(con, Dt1.Rows(i).Item("count_idno").ToString)
                            .Rows(n).Cells(2).Value = Trim(Dt1.Rows(i).Item("Yarn_type").ToString)
                            .Rows(n).Cells(3).Value = Trim(Dt1.Rows(i).Item("millname").ToString) ' Common_Procedures.Mill_IdNoToName(con, Dt1.Rows(i).Item("mill_idno").ToString)
                            .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Bags").ToString)
                            .Rows(n).Cells(8).Value = Val(Dt1.Rows(i).Item("Cones").ToString)
                            .Rows(n).Cells(9).Value = Val(Dt1.Rows(i).Item("Weight").ToString)
                            'If Val(Dt1.Rows(i).Item("Thiri").ToString) <> 0 Then
                            '    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Thiri").ToString)
                            'End If

                        Next
                    End If
                End With
                Dt1.Clear()

                'Da = New SqlClient.SqlDataAdapter("Select a.* from SizingSoft_Yarn_Delivery_Details a   where  'SYDEL-'+  a.Yarn_Delivery_code =  '" & Trim(dgv_delivery_Selections.Rows(k).Cells(8).Value) & "' ", con)
                'Dt1 = New DataTable
                'Da.Fill(Dt1)

                'With dgv_Details

                '    If Val(Dt1.Rows.Count <> 0) Then

                '        For i = 0 To Dt1.Rows.Count - 1

                '            n = dgv_Details.Rows.Add()

                '            .Rows(n).Cells(0).Value = Trim(Dt1.Rows(i).Item("Sl_No").ToString)
                '            .Rows(n).Cells(1).Value = Common_Procedures.Count_IdNoToName(con, Dt1.Rows(i).Item("count_idno").ToString)
                '            .Rows(n).Cells(2).Value = Trim(Dt1.Rows(i).Item("Yarn_type").ToString)
                '            .Rows(n).Cells(3).Value = Common_Procedures.Mill_IdNoToName(con, Dt1.Rows(i).Item("mill_idno").ToString)
                '            .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Bags").ToString)
                '            .Rows(n).Cells(8).Value = Val(Dt1.Rows(i).Item("Cones").ToString)
                '            .Rows(n).Cells(9).Value = Val(Dt1.Rows(i).Item("Weight").ToString)

                '        Next
                '    End If

                'End With

                Exit For

            End If

            Dt1.Clear()

        Next


        pnl_Back.Enabled = True
        pnl_Delivery_Selection.Visible = False
        Panel2.Enabled = False

        If cbo_Transport.Visible And cbo_Transport.Enabled Then cbo_Transport.Focus()

        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
            txt_EmptyBeam.Enabled = False
            cbo_BeamWidth.Enabled = False
        End If


    End Sub
    Private Sub btn_Close_Delivery_Selection_Click(sender As Object, e As EventArgs) Handles btn_Close_Delivery_Selection.Click

        Close_Delivery_Selection()

    End Sub



    Private Sub Select_Dc(ByVal RwIndx As Integer)
        Dim i As Integer




        With dgv_delivery_Selections

            If .RowCount > 0 And RwIndx >= 0 Then

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(7).Value = ""
                Next

                .Rows(RwIndx).Cells(7).Value = 1

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


                Close_Delivery_Selection()
                Total_Calculation()
            End If

        End With



    End Sub

    Private Sub cbo_Ledger_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Ledger.SelectedIndexChanged

    End Sub

    Private Sub dgv_delivery_Selections_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgv_delivery_Selections.CellMouseClick
        Close_Delivery_Selection()

        Total_Calculation()
    End Sub

    Private Sub cbo_Type_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        EMAIL_Status = False
        WHATSAPP_Status = False
        print_record()
        'Print_PDF_Status = False
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, dtp_Date, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Ledger, "", "", "", "")

    End Sub
    Private Sub dgv_delivery_Selections_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_delivery_Selections.CellClick
        Select_Dc(e.RowIndex)
    End Sub


    Private Sub dgv_delivery_Selections_KeyDown(sender As Object, e As KeyEventArgs) Handles dgv_delivery_Selections.KeyDown
        On Error Resume Next

        With dgv_delivery_Selections

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If .CurrentCell.RowIndex >= 0 Then
                    Select_Dc(.CurrentCell.RowIndex)
                    e.Handled = True
                End If
            End If

            If e.KeyCode = Keys.Back Or e.KeyCode = Keys.Delete Then
                If .CurrentCell.RowIndex >= 0 Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(7).Value) = 1 Then
                        Select_Dc(.CurrentCell.RowIndex)
                        e.Handled = True
                    End If
                End If
            End If
            Total_Calculation()

        End With


    End Sub

    Private Sub btn_Delivery_Selection_Click(sender As Object, e As EventArgs) Handles btn_Delivery_Selection.Click
        Delivery_Selection()
        Total_Calculation()
    End Sub

    Private Sub dtp_Time_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtp_Time.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_CountName.Focus()
        End If
    End Sub

    Private Sub dtp_Time_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_Time.KeyDown
        If e.KeyCode = 38 Then
            cbo_coneType.Focus()
        End If

        If e.KeyCode = 40 Then
            cbo_CountName.Focus()
        End If
    End Sub


    Private Sub Printing_Format1037(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim CtNm1 As String, CtmNm2 As String
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt As Integer = 0

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        PrntCnt = 1

        If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If
        End If

        set_PaperSize_For_PrintDocument1()

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10  ' 50 
            .Right = 50
            .Top = 10 '30
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
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        ' ''NoofItems_PerPage = 8 '4  '5 
        ' '' update for full page
        If Common_Procedures.settings.CustomerCode = "1288" Then  ' KKP 
            'NoofItems_PerPage = 45 '4  '5 
            NoofItems_PerPage = 38
        ElseIf Common_Procedures.settings.CustomerCode = "1044" Then '-------GANESH KARTHI TEXTILE PRIVATE LIMITED
            NoofItems_PerPage = 8 '4  '5 
        Else
            NoofItems_PerPage = 10 '4  '5 
        End If


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 45 : ClArr(3) = 140 : ClArr(4) = 120 : ClArr(5) = 65 : ClArr(6) = 70 : ClArr(7) = 65 : ClArr(8) = 50 : ClArr(9) = 62
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height  ' 20
        If Common_Procedures.settings.CustomerCode = "1044" Then '-------GANESH KARTHI TEXTILE PRIVATE LIMITED
            TxtHgt = 18 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        Else
            TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If

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




            EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format1037_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                    Try


                        NoofDets = 0

                        CurY = CurY - 10

                        If prn_DetDt.Rows.Count > 0 Then

                            Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                                If NoofDets >= NoofItems_PerPage Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then


                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                        NoofDets = NoofDets + 1

                                        Printing_Format1037_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)
                                        e.HasMorePages = True
                                        Return

                                    End If

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format1037_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)
                                    e.HasMorePages = True
                                    Return
                                End If

                                prn_DetSNo = prn_DetSNo + 1

                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 16 Then
                                    For I = 16 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 16
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                CtNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString)
                                CtmNm2 = ""
                                If Len(CtNm1) > 8 Then
                                    For I = 8 To 1 Step -1
                                        If Mid$(Trim(CtNm1), I, 1) = " " Or Mid$(Trim(CtNm1), I, 1) = "," Or Mid$(Trim(CtNm1), I, 1) = "." Or Mid$(Trim(CtNm1), I, 1) = "-" Or Mid$(Trim(CtNm1), I, 1) = "/" Or Mid$(Trim(CtNm1), I, 1) = "_" Or Mid$(Trim(CtNm1), I, 1) = "(" Or Mid$(Trim(CtNm1), I, 1) = ")" Or Mid$(Trim(CtNm1), I, 1) = "\" Or Mid$(Trim(CtNm1), I, 1) = "[" Or Mid$(Trim(CtNm1), I, 1) = "]" Or Mid$(Trim(CtNm1), I, 1) = "{" Or Mid$(Trim(CtNm1), I, 1) = "}" Then Exit For
                                    Next
                                    If I = 0 Then I = 8
                                    CtmNm2 = Microsoft.VisualBasic.Right(Trim(CtNm1), Len(CtNm1) - I)
                                    CtNm1 = Microsoft.VisualBasic.Left(Trim(CtNm1), I - 1)
                                End If


                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CtNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("weight_Bag").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("weight_Bag").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                End If
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones_Bag").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                End If
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Cone").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Cone").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                End If
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                End If
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Or Trim(CtmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(CtmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If
                                prn_DetIndx = prn_DetIndx + 1

                            Loop

                        End If

                        Printing_Format1037_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


                    Catch ex As Exception

                        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    End Try

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count >= 6 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        ' Cnt = 10
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
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                ' prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format1037_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurX As Single = 0
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Ledname1 As String
        Dim Ledname2 As String
        Dim i As Integer


        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Receipt_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Receipt_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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

        CurY = CurY + TxtHgt - 20
        p1Font = New Font("Calibri", 17, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
        End If
        CurY = CurY + strHeight - 3
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + strHeight - 3
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)


        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        Else
            CurY = CurY + TxtHgt - 3
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
            CurX = CurX + strWidth - 3
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX + 3, CurY, 0, 0, pFont)

            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurX = CurX + strWidth - 3
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth - 3
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 3
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt - 1

        End If

        CurY = CurY + TxtHgt - 15  '10
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN RECEIPT NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt
        CurY = CurY + strHeight - 5 ' + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("PARTY D.C.NO  : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

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


            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & Ledname1, LMargin + 10, CurY, 0, 0, p1Font)

            If Trim(Ledname2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(Ledname2), LMargin + S1 + 10, CurY, 0, 0, p1Font)
                'NoofDets = NoofDets + 1
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
                If Trim(prn_HdDt.Rows(0).Item("Lot_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lot_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            Else
                If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Through", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PARTY D.C.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 5
            If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
                If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " TIN NO.: " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
            Else
                If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                    pFont = New Font("Calibri", 11, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
            End If
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then '--KKP SPINNING MILLS PVT. LTD
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                If Trim(prn_HdDt.Rows(0).Item("Godown_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Location", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Godown_Name").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            pFont = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WT/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CNS/BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WT/CN", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            pFont = New Font("calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1037_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt + 2
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            'CurY = CurY + TxtHgt
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 80, CurY, 2, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Remarks :" & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 350, CurY, 0, 0, pFont)
                If IsDBNull(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) = False Then
                    If Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, " Beam Width : " & Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString), PageWidth - 200, CurY, 0, 0, pFont)
                    End If
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

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
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

End Class

