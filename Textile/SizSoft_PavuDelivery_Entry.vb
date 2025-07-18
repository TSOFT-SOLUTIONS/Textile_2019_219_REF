Imports System.IO
Public Class SizSoft_PavuDelivery_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PVDLV-"
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private cnt As Integer = 0
    Private prn_DetIndx As Integer
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo1 As Integer
    Private prn_DetIndx1 As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_NoofBmDets1 As Integer
    Private prn_Status As Integer
    Private Prev_kyData As Keys
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private PrntCnt2ndPageSTS As Boolean = False

    Private pth As String
    Private pth2 As String
    Private PrnTxt As String = ""
    Private a() As String
    Private fs As FileStream
    Private r As StreamReader
    Private w As StreamWriter
    Private prn_DetSNo As Integer
    Private prn_DetSNo1 As Integer
    Private Hz1 As Integer, Hz2 As Integer, Vz1 As Integer, Vz2 As Integer
    Private Corn1 As Integer, Corn2 As Integer, Corn3 As Integer, Corn4 As Integer
    Private LfCon As Integer, RgtCon As Integer
    Private LnCnt As Integer = 0, CenCon As Integer
    Private CenDwn As Integer, CenUp As Integer

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        txt_BookNo.Text = ""
        txt_SetNo.Text = ""
        cbo_Ledger.Text = ""
        lbl_CountName.Text = ""
        cbo_DeliveryTo.Text = ""
        cbo_Grid_Vendor.Text = ""
        cbo_VendorName.Text = ""
        txt_Rate.Text = ""
        lbl_value.Text = ""
        lbl_Ends.Text = ""
        cbo_Transport.Text = ""

        cbo_Type.Text = "SPEC"
        If (Common_Procedures.settings.CustomerCode) = "1282" Then
            cbo_Type.Text = "DIRECT"
        End If

        cbo_VehicleNo.Text = ""
        cbo_Delivered.Text = ""
        txt_Remarks.Text = ""
        txt_ElectronicRefNo.Text = ""
        txt_DateAndTimeOFSupply.Text = ""
        dtp_Time.Text = ""
        cbo_Grid_Count.Text = ""

        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate   '.Date.ToShortDateString
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate   '.Date.ToShortDateString
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Ledger.Tag = ""

        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        dtp_Date.Enabled = True
        dtp_Date.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_VehicleNo.Enabled = True
        cbo_VehicleNo.BackColor = Color.White
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

        If Me.ActiveControl.Name <> cbo_Grid_Count.Name Then
            cbo_Grid_Count.Visible = False
        End If

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Company_Name as PartyName, c.Transport_Name from SizSoft_Pavu_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Transport_Head c ON a.Transport_IdNo = c.Transport_IdNo Where a.Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_DcNo.Text = dt1.Rows(0).Item("Pavu_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Pavu_Delivery_Date").ToString

                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                txt_BookNo.Text = dt1.Rows(0).Item("Book_No").ToString

                lbl_Ends.Text = dt1.Rows(0).Item("Ends_Name").ToString

                lbl_CountName.Text = dt1.Rows(0).Item("Count_Name").ToString

                cbo_Transport.Text = dt1.Rows(0).Item("Transport_Name").ToString
                'cbo_DeliveryTo.Text ="" ' Common_Procedures.Delivery_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                cbo_VendorName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Vendor_IdNo").ToString))
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_Delivered.Text = dt1.Rows(0).Item("Delivered_By").ToString

                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                Cbo_RateFor.Text = dt1.Rows(0).Item("Rate_For").ToString
                txt_ElectronicRefNo.Text = dt1.Rows(0).Item("Electronic_Reference_No").ToString
                txt_DateAndTimeOFSupply.Text = dt1.Rows(0).Item("Date_And_Time_Of_Supply").ToString
                txt_Rate.Text = dt1.Rows(0).Item("Rate").ToString
                lbl_value.Text = dt1.Rows(0).Item("Approx_Value").ToString
                If IsDBNull(dt1.Rows(0).Item("Invoice_Selection_Type").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Invoice_Selection_Type").ToString) <> "" Then
                        cbo_Type.Text = dt1.Rows(0).Item("Invoice_Selection_Type").ToString
                    End If
                End If

                dtp_Time.Text = (dt1.Rows(0).Item("Entry_Time_Text").ToString)
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                If IsDBNull(dt1.Rows(0).Item("Sizing_Specification_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Sizing_Specification_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                If IsDBNull(dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

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


                da2 = New SqlClient.SqlDataAdapter("select a.* , b.Count_Name from SizSoft_Pavu_Delivery_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Pavu_Delivery_Code = '" & Trim(NewCode) & "' Order by a.For_OrderBy, a.Set_No, a.ForOrderBy_BeamNo, a.Beam_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Set_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Beam_No").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Noof_Pcs").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then '---- Sri Meenakshi Sizing (Somanur)
                            dgv_Details.Rows(n).Cells(5).Value = Common_Procedures.Vendor_IdNoToName(con, dt2.Rows(i).Item("Vendor_Idno").ToString)
                        Else
                            dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("DeliveryTo_Name").ToString
                        End If

                        dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("set_code").ToString
                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Ends_Name").ToString
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Meters_pc").ToString
                        dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Net_Weight").ToString

                        If LockSTS = True Then
                            For J = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                                dgv_Details.Rows(n).Cells(J).Style.ForeColor = Color.Red
                            Next
                        End If

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Beam").ToString)
                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Sizing_Tot_Weight").ToString), "#########0.000")
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

            End If
            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub PavuDelivery_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

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

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Vendor.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Vendor.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VendorName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VendorName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub PavuDelivery_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        Me.Text = ""

        con.Open()



        lbl_Ends.Visible = True
        lbl_CountName.Visible = True
        Label12.Visible = True
        Label21.Visible = True

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            lbl_Ends.Visible = False
            lbl_CountName.Visible = False
            Label12.Visible = False
            Label21.Visible = False
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            lbl_Del_Vendor.Text = "Vendor"
            cbo_DeliveryTo.Visible = False
            cbo_VendorName.Visible = True
            cbo_VendorName.BringToFront()
            cbo_VendorName.Width = cbo_DeliveryTo.Width
            cbo_VendorName.BackColor = Color.White
            dgv_Details.Columns(5).ReadOnly = True
        Else
            lbl_Del_Vendor.Text = "Delivery To"
            cbo_DeliveryTo.Visible = True
            cbo_VendorName.Visible = False
            cbo_DeliveryTo.BringToFront()
            dgv_Details.Columns(5).ReadOnly = False
        End If


        lbl_typeCaption.Visible = False
        cbo_Type.Visible = False
        btn_Selection.Visible = True
        If (Common_Procedures.settings.CustomerCode) = "1282" Then
            'lbl_typeCaption.Visible = True
            'cbo_Type.Visible = True
            btn_Selection.Visible = False
            cbo_Ledger.Width = txt_BookNo.Width
        Else
            cbo_Ledger.Width = txt_BookNo.Width - btn_Selection.Width - 20
        End If

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("SPEC")


        Cbo_RateFor.Items.Clear()
        Cbo_RateFor.Items.Add("")
        Cbo_RateFor.Items.Add("METERS")
        Cbo_RateFor.Items.Add("WEIGHT")

        Pnl_DosPrint.Visible = False
        Pnl_DosPrint.BringToFront()
        Pnl_DosPrint.Left = (Me.Width - Pnl_DosPrint.Width) \ 2
        Pnl_DosPrint.Top = (Me.Height - Pnl_DosPrint.Height) \ 2

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.BringToFront()
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2

        ' btn_UserModification.Visible = False
        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            'btn_UserModification.Visible = True
            chk_Printed.Enabled = True
        End If


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VendorName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BookNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ElectronicRefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateAndTimeOFSupply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_value.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter_Show.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter_Close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Preprint.GotFocus, AddressOf ControlGotFocus
        AddHandler Btn_DosPrint.GotFocus, AddressOf ControlGotFocus
        AddHandler Btn_LaserPrint.GotFocus, AddressOf ControlGotFocus
        AddHandler Btn_DosCancel.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Vendor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Delivered.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_RateFor.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VendorName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BookNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ElectronicRefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateAndTimeOFSupply.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_value.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Vendor.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Delivered.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_RateFor.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Count.LostFocus, AddressOf ControlLostFocus


        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter_Show.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter_Close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Preprint.LostFocus, AddressOf ControlLostFocus
        AddHandler Btn_DosPrint.LostFocus, AddressOf ControlLostFocus
        AddHandler Btn_LaserPrint.LostFocus, AddressOf ControlLostFocus
        AddHandler Btn_DosCancel.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_DateAndTimeOFSupply.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BookNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ElectronicRefNo.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Approx_Value.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DateAndTimeOFSupply.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_value.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BookNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Remarks.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then
            dtp_Time.Visible = True
        End If

        Filter_Status = False
        FrmLdSTS = True
        movelast_record()

    End Sub

    Private Sub PavuDelivery_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub PavuDelivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim Pr_kyData As Keys

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

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            Pr_kyData = Prev_kyData
            Prev_kyData = keyData

            With dgv1

                If (keyData = Keys.Enter Or keyData = Keys.Down Or keyData = 131085) Then

                    If Pr_kyData = 131089 Then
                        cbo_Transport.Focus()

                    ElseIf .CurrentCell.ColumnIndex >= 9 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            cbo_Transport.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                        End If






                    ElseIf Trim(cbo_Type.Text) = "DIRECT" And .CurrentCell.ColumnIndex = 5 Then

                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(7)

                    ElseIf Trim(cbo_Type.Text) <> "DIRECT" And .CurrentCell.ColumnIndex >= 5 Then

                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            cbo_Transport.Focus()


                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(5)
                            ' .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                        End If



                    Else

                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                    Return True


                ElseIf keyData = Keys.Up Then

                    If .CurrentCell.ColumnIndex <= .ColumnCount Then
                        If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex = 1 Then
                            txt_ElectronicRefNo.Focus()

                        ElseIf Trim(cbo_Type.Text) = "DIRECT" And .CurrentCell.ColumnIndex = 7 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)

                        ElseIf Trim(cbo_Type.Text) <> "DIRECT" And .CurrentCell.ColumnIndex = 5 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(5)

                        ElseIf .CurrentCell.ColumnIndex = 1 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(9)
                        Else

                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                    End If

                    Return True

                Else
                    Return MyBase.ProcessCmdKey(msg, keyData)

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

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

            da = New SqlClient.SqlDataAdapter("select distinct(Ends_Name) from Stock_SizedPavu_Processing_Details order by Ends_Name", con)
            da.Fill(dt3)
            cbo_Filter_EndsName.DataSource = dt3
            cbo_Filter_EndsName.DisplayMember = "Ends_Name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate   '.Date.ToShortDateString
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate   '.Date.ToShortDateString
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_EndsName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_EndsName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Delivery_No from SizSoft_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Pavu_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Delivery_No from SizSoft_Pavu_Delivery_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Pavu_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Delivery_No from SizSoft_Pavu_Delivery_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Pavu_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Delivery_No from SizSoft_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Pavu_Delivery_No desc", con)
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

            lbl_DcNo.Text = "NEW" ' Common_Procedures.get_MaxCode(con, "SizSoft_Pavu_Delivery_Head", "Pavu_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            dtp_Time.Text = Format(Now, "hh:mm tt").ToString

            lbl_DcNo.ForeColor = Color.Red
            If Trim(txt_DateAndTimeOFSupply.Text) = "" Then txt_DateAndTimeOFSupply.Text = Format(Now, "dd-MM-yyyy hh:mm tt")
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Pavu_Delivery_No from SizSoft_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '-----
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_Remarks, "", "", "", "", False)
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_DeliveryTo, txt_Remarks, "", "", "", "")
        If (e.KeyValue = 40 And cbo_VehicleNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                txt_Remarks.Focus()
            End If
        End If

        If (e.KeyValue = 38 And cbo_VehicleNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                cbo_VendorName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_ElectronicRefNo, cbo_Delivered, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
        If (e.KeyValue = 38) Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '---- Kalaimagal Sizing (Avinashi)
                txt_BookNo.Focus()

            Else
                If dgv_Details.RowCount > 0 Then
                    dgv_Details.Focus()
                    If Trim(cbo_Type.Text) <> "DIRECT" Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.RowCount - 1).Cells(5)
                        dgv_Details.CurrentCell.Selected = True
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.RowCount - 1).Cells(9)
                        dgv_Details.CurrentCell.Selected = True
                    End If


                Else
                    txt_BookNo.Focus()

                End If

            End If

        End If
    End Sub


    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_Delivered, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
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
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Pavu_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Pavu_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Pavu_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Pavu_Delivery_Code IN (select z1.Pavu_Delivery_Code from Stock_SizedPavu_Processing_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Trim(cbo_Filter_EndsName.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Pavu_Delivery_Code IN (select z2.Pavu_Delivery_Code from Stock_SizedPavu_Processing_Details z2 where z2.Ends_Name = '" & Trim(cbo_Filter_EndsName.Text) & "')"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from SizSoft_Pavu_Delivery_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Pavu_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Pavu_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Pavu_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Beam").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")

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

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, txt_RecNo, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub



    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_EndsName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_EndsName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_EndsName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "Ends_Name", "", "")
    End Sub


    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsName, cbo_Filter_CountName, btn_Filter_Show, "Stock_SizedPavu_Processing_Details", "Ends_Name", "", "")

    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsName, Nothing, "Stock_SizedPavu_Processing_Details", "Ends_Name", "", "")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        'With dgv_Details
        '    If .CurrentCell.RowIndex = .RowCount - 1 Then
        '        cbo_Transport.Focus()
        '    End If
        'End With
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
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

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then '---- Sri Meenakshi Sizing (Somanur)

                If e.ColumnIndex = 5 Then

                    If cbo_Grid_Vendor.Visible = False Or Val(cbo_Grid_Vendor.Tag) <> e.RowIndex Then

                        'dgv_ActCtrlName = dgv_Details.Name

                        cbo_Grid_Vendor.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Vendor_Name from Vendor_Head Order by Vendor_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_Grid_Vendor.DataSource = Dt1
                        cbo_Grid_Vendor.DisplayMember = "Vendor_Name"

                        Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_Vendor.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Grid_Vendor.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_Grid_Vendor.Width = Rect.Width  ' .CurrentCell.Size.Width
                        cbo_Grid_Vendor.Height = Rect.Height  ' rect.Height

                        cbo_Grid_Vendor.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Grid_Vendor.Tag = Val(e.RowIndex)
                        cbo_Grid_Vendor.Visible = True

                        cbo_Grid_Vendor.BringToFront()
                        cbo_Grid_Vendor.Focus()
                    End If
                Else

                    cbo_Grid_Vendor.Visible = False

                End If
            End If


            If e.ColumnIndex = 8 And Trim(cbo_Type.Text) = "DIRECT" Then

                If cbo_Grid_Count.Visible = False Or Val(cbo_Grid_Count.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_Details.Name

                    cbo_Grid_Count.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head Order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Count.DataSource = Dt1
                    cbo_Grid_Count.DisplayMember = "Count_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Count.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_Count.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_Count.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_Count.Height = Rect.Height  ' rect.Height

                    cbo_Grid_Count.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_Count.Tag = Val(e.RowIndex)
                    cbo_Grid_Count.Visible = True

                    cbo_Grid_Count.BringToFront()
                    cbo_Grid_Count.Focus()
                Else

                    cbo_Grid_Count.Visible = False
                End If
            End If

        End With
    End Sub

    Private Sub cbo_Vendor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Vendor.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vendor_AlaisHead", "Vendor_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_Vendor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Vendor.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Vendor, Nothing, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Vendor_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Vendor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Vendor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7)

            End If

        End With
    End Sub

    Private Sub cbo_Vendor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Vendor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Vendor, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Vendor_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7)

            End With

        End If

    End Sub

    Private Sub cbo_Vendor_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Vendor.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Vendor_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Vendor.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Vendor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Vendor.TextChanged
        Try
            If cbo_Grid_Vendor.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Vendor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 5 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Vendor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 4 Then
                        Total_Calculation()
                    End If
                End If
            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                If .CurrentRow.Index = .RowCount - 1 Then
                    For i = 1 To .Columns.Count - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(.CurrentRow.Index)

                End If

                Total_Calculation()

            End With

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

    Private Sub txt_ElectronicRefNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ElectronicRefNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '---- Kalaimagal Sizing (Avinashi)
                cbo_Transport.Focus()
            Else
                If cbo_Type.Text <> "DIRECT" Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                        dgv_Details.CurrentCell.Selected = True

                    Else
                        cbo_Transport.Focus()

                    End If

                Else
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                        dgv_Details.CurrentCell.Selected = True

                    Else
                        cbo_Transport.Focus()

                    End If

                End If
            End If

        End If

        ' End If
    End Sub

    'Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
    '    'If e.KeyCode = 40 Then btn_save.Focus() ' SendKeys.Send("{TAB}")
    '    'If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    'End Sub

    'Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        Cbo_RateFor.Focus()
    '        'If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
    '        '    save_record()
    '        'Else
    '        '    dtp_Date.Focus()
    '        'End If
    '    End If
    'End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBms As Single, TotPcs As Single, TotMtrs As Single, nTotWght As Single

        Sno = 0
        TotBms = 0
        TotPcs = 0
        TotMtrs = 0
        nTotWght = 0

        With dgv_Details
            For i = 0 To .Rows.Count - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Val(Sno)
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotPcs = TotPcs + Val(.Rows(i).Cells(3).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value)
                    nTotWght = nTotWght + Val(.Rows(i).Cells(10).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotBms)
            .Rows(0).Cells(3).Value = Val(TotPcs)
            .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(9).Value = Format(Val(nTotWght), "########0.000")
        End With

        Amount_Calultation()

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Close_Form()
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim n As Integer
        Dim sno As Integer
        Dim EdsNm As String, DupEdsNm As String
        Dim CntNm As String, DupCntNm As String

        dgv_Details.Rows.Clear()

        EdsNm = "" : DupEdsNm = ""
        CntNm = "" : DupCntNm = ""

        pnl_Back.Enabled = True

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(5).Value) = 1 Then

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(4).Value = Format(Val(dgv_Selection.Rows(i).Cells(4).Value), "#########0.00")
                dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(9).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(11).Value

                If InStr(1, Trim(LCase(DupEdsNm)), "~" & Trim(LCase(dgv_Selection.Rows(i).Cells(8).Value)) & "~") = 0 Then
                    EdsNm = Trim(EdsNm) & IIf(Trim(EdsNm) <> "", ", ", "") & Trim(dgv_Selection.Rows(i).Cells(8).Value)
                    DupEdsNm = Trim(DupEdsNm) & "~" & Trim(dgv_Selection.Rows(i).Cells(8).Value) & "~"
                End If

                If InStr(1, Trim(LCase(DupCntNm)), "~" & Trim(LCase(dgv_Selection.Rows(i).Cells(9).Value)) & "~") = 0 Then
                    CntNm = Trim(CntNm) & IIf(Trim(CntNm) <> "", ", ", "") & Trim(dgv_Selection.Rows(i).Cells(9).Value)
                    DupCntNm = Trim(DupCntNm) & "~" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "~"
                End If

            End If

        Next

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
            If dgv_Details.Rows.Count > 18 Then
                MessageBox.Show("Does Not Select More than 18 BeamNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()
                Exit Sub
            End If
        End If
        lbl_Ends.Text = Trim(EdsNm)
        lbl_CountName.Text = Trim(CntNm)

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedNo As Integer
        Dim NewCode As String
        Dim RptCondt As String = ""
        Dim CompIDCondt As String

        If Trim(UCase(cbo_Type.Text)) = "DIRECT" Then Exit Sub


        LedNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If txt_SetNo.Visible = True And Trim(txt_SetNo.Text) <> "" Then
            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Set_No = '" & Trim(txt_SetNo.Text) & " '"
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()
            chk_SelectAll.Checked = False

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where  " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Pavu_Delivery_Code = '" & Trim(NewCode) & "' and  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " (a.ledger_Idno = " & Str(Val(LedNo)) & " or a.ledger_Idno = " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ") order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
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
                    .Rows(n).Cells(5).Value = "1"
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Set_Code").ToString

                    If Trim(Dt1.Rows(i).Item("DeliveryTo_Name").ToString) <> "" Then
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("DeliveryTo_Name").ToString

                    Else
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1163" Then '---- Guru Sizing (Somanur)
                            If Val(Dt1.Rows(i).Item("Vendor_Idno").ToString) <> 0 Then
                                .Rows(n).Cells(7).Value = Common_Procedures.Vendor_IdNoToName(con, Val(Dt1.Rows(i).Item("Vendor_Idno").ToString))
                            End If
                        End If

                    End If

                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Ends_Name").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("METERS_PC").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Net_Weight").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Pavu_Delivery_Code = '' and  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " (a.ledger_Idno = " & Str(Val(LedNo)) & " or a.ledger_Idno = " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ") order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
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
                    .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Set_Code").ToString

                    If Trim(Dt1.Rows(i).Item("DeliveryTo_Name").ToString) <> "" Then
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("DeliveryTo_Name").ToString

                    Else
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1163" Then '---- Guru Sizing (Somanur)
                            If Val(Dt1.Rows(i).Item("Vendor_Idno").ToString) <> 0 Then
                                .Rows(n).Cells(7).Value = Common_Procedures.Vendor_IdNoToName(con, Val(Dt1.Rows(i).Item("Vendor_Idno").ToString))
                            End If
                        End If

                    End If

                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Ends_Name").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("METERS_PC").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Net_Weight").ToString

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

        cnt = 0
    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Pavu(e.RowIndex)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
            If cnt > 18 Then
                MessageBox.Show("Does Not Select More than 18 BeamNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub Select_Pavu(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then
                    .Rows(RwIndx).Cells(5).Value = ""
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                    Next

                Else
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                End If
                If Val(dgv_Selection.CurrentRow.Cells(5).Value) = 1 Then
                    cnt = cnt + 1
                Else
                    cnt = cnt - 1
                End If

            End If

        End With

    End Sub

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(5).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Blue
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_Pavu(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub


    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                Select_Pavu(dgv_Selection.CurrentCell.RowIndex)
                e.Handled = True
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
                    If cnt > 18 Then
                        MessageBox.Show("Does Not Select More than 18 BeamNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()
                        Exit Sub
                    End If

                End If
            End If
        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        dgv_Details.Rows.Add()
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyValue = 38 Then e.Handled = True : txt_Remarks.Focus() ' SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '---
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub txt_SetNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SetNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Selection_Click(sender, e)

        End If
    End Sub

    Private Sub Cbo_DelTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Delivery_Party_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_Delivered, cbo_VehicleNo, "Delivery_Party_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_VehicleNo, "Delivery_Party_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    Common_Procedures.MDI_LedType = ""
        '    Dim f As New Delivery_Party_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()
        'End If
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

            smstxt = "PAVU DELIVERY" & vbCrLf

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                smstxt = smstxt & vbCrLf
            End If

            smstxt = smstxt & "DC.NO-" & Trim(lbl_DcNo.Text) & vbCrLf & "DATE-" & Trim(dtp_Date.Text)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            da2 = New SqlClient.SqlDataAdapter("select Total_Beam,Total_Meters from SizSoft_Pavu_Delivery_Head where Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1
                    smstxt = smstxt & vbCrLf & "Total Pavu  : " & Trim(dt2.Rows(i).Item("Total_Beam").ToString)
                    smstxt = smstxt & vbCrLf & "Total Meter : " & Trim(dt2.Rows(i).Item("Total_Meters").ToString)
                Next i

            End If
            dt2.Clear()



            smstxt = smstxt & " " & vbCrLf & vbCrLf
            smstxt = smstxt & " Thanks! " & vbCrLf
            smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

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

    Private Sub cbo_VendorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VendorName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VendorName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VendorName, cbo_Delivered, cbo_VehicleNo, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VendorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VendorName, cbo_VehicleNo, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
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

    Private Sub txt_Approx_Value_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If (e.KeyValue = 38) Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then
                cbo_VendorName.Focus()
            Else
                cbo_DeliveryTo.Focus()
            End If

        End If
        If (e.KeyValue = 40) Then
            txt_Remarks.Focus()
        End If

    End Sub

    Private Sub txt_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
        If e.KeyValue = 38 Then
            Cbo_RateFor.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save? ", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save? ", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub




    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Amount_Calultation()
    End Sub


    Private Sub Amount_Calultation()
        Dim vTotMtrs As String
        Dim nTotWgt As String

        vTotMtrs = 0
        nTotWgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            nTotWgt = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
        End If

        If Trim(Cbo_RateFor.Text) = "METERS" Then
            lbl_value.Text = Format(Val(vTotMtrs) * Val(txt_Rate.Text), "########0.00")
        Else
            lbl_value.Text = Format(Val(nTotWgt) * Val(txt_Rate.Text), "########0.00")
        End If

    End Sub

    Private Sub cbo_Delivered_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Delivered.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delivered, Nothing, Nothing, "", "", "", "")

        If (e.KeyValue = 40 And cbo_Delivered.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(Common_Procedures.settings.CustomerCode) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                cbo_VendorName.Focus()
            Else
                cbo_DeliveryTo.Focus()
            End If
        End If

        If (e.KeyValue = 38 And cbo_Delivered.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            cbo_Transport.Focus()
        End If
    End Sub

    Private Sub cbo_Delivered_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Delivered.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delivered, cbo_DeliveryTo, "", "", "", "", False)
        If Trim(Common_Procedures.settings.CustomerCode) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            cbo_VendorName.Focus()
        Else
            cbo_DeliveryTo.Focus()
        End If
    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_Ledger, txt_BookNo, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) <> "DIRECT" Then
                If MessageBox.Show("Do you want to select Pavu :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    txt_BookNo.Focus()

                End If

            Else
                txt_BookNo.Focus()


            End If

        End If

    End Sub

    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        With dgv_Details
            If Trim(UCase(cbo_Type.Text)) <> "DIRECT" Then
                dgv_Details.AllowUserToAddRows = False

                .Columns(1).ReadOnly = True
                .Columns(2).ReadOnly = True
                .Columns(3).ReadOnly = True
                .Columns(4).ReadOnly = True
                .Columns(5).ReadOnly = False
                .Columns(7).ReadOnly = True
                .Columns(8).ReadOnly = True
                .Columns(9).ReadOnly = True


            Else
                dgv_Details.AllowUserToAddRows = True

                .Columns(1).ReadOnly = False
                .Columns(2).ReadOnly = False
                .Columns(3).ReadOnly = False
                .Columns(4).ReadOnly = False
                .Columns(5).ReadOnly = False
                .Columns(7).ReadOnly = False
                .Columns(8).ReadOnly = False
                .Columns(9).ReadOnly = False
            End If

        End With
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

            cmd.CommandText = "Update pavu_Delivery_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
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

    Private Sub cbo_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Count, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(9)
            End If

        End With
    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Count, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(9)

            End With

        End If
    End Sub


    Private Sub cbo_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Count_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Count.TextChanged
        Try
            If cbo_Grid_Count.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Count.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 8 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Count.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub



    Private Sub Cbo_RateFor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_RateFor.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub Cbo_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_RateFor.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_RateFor, txt_Remarks, txt_Rate, "", "", "", "")
        'If (e.KeyValue = 40 And cbo_VehicleNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then
        '        cbo_VendorName.Focus()
        '    Else
        '        txt_Rate.Focus()
        '    End If

        'End If
    End Sub

    Private Sub Cbo_RateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_RateFor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_RateFor, txt_Rate, "", "", "", "", False)
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then
        '        cbo_VendorName.Focus()
        '    Else
        '        txt_Rate.Focus()
        '    End If
        'End If
    End Sub

    Private Sub Cbo_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_RateFor.TextChanged
        Amount_Calultation()
    End Sub
End Class