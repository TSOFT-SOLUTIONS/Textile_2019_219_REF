Imports System.IO
Public Class SizSoft_Empty_BeamBagCone_Receipt_Entry_by_Sizing
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = ""
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_Status As Integer
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetAr(50, 10) As String
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private PrntCnt2ndPageSTS As Boolean = False
    Private Print_PDF_Status As Boolean = False
    Private pth As String
    Private pth2 As String
    Private PrnTxt As String = ""
    Private a() As String
    Private fs As FileStream
    Private r As StreamReader
    Private w As StreamWriter
    Private prn_DetSNo As Integer

    Private Hz1 As Integer, Hz2 As Integer, Vz1 As Integer, Vz2 As Integer
    Private Corn1 As Integer, Corn2 As Integer, Corn3 As Integer, Corn4 As Integer
    Private LfCon As Integer, RgtCon As Integer
    Private LnCnt As Integer = 0

    ' PRAKASH    SIZING 
    Private Prnt_HalfSheet_STS As Boolean = False
    Private vPrnt_2Copy_In_SinglePage As Integer = 0
    ' PRAKASH    SIZING 


    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_filter.Visible = False
        pnl_PartyDc_Image.Visible = False
        pnl_back.Enabled = True
        lbl_ReceiptNo.Text = ""
        lbl_ReceiptNo.ForeColor = Color.Black
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_beamwidth.Text = ""
        cbo_vehicleno.Text = ""
        txt_remarks.Text = ""
        cbo_Vendor.Text = ""
        txt_Book_No.Text = ""
        txt_Party_DcNo.Text = ""
        cbo_Received.Text = ""
        chk_UnLoaded.Checked = False
        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False
        Print_PDF_Status = False
        dtp_Time.Text = ""

        pic_PartyDc_Image.BackgroundImage = Nothing

        dgv_Details.Rows.Clear()

        Grid_Cell_DeSelect()
        cbo_beamwidth.Visible = False
        cbo_beamwidth.Tag = -1
        cbo_Vendor.Visible = False
        cbo_Vendor.Tag = -1
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dtp_Date.Enabled = True
        dtp_Date.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_vehicleno.Enabled = True
        cbo_vehicleno.BackColor = Color.White

        lbl_UserName_CreatedBy.Text = ""
        lbl_UserName_ModifiedBy.Text = ""


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
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from SizSoft_Empty_BeamBagCone_Receipt_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_ReceiptNo.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Receipt_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Empty_BeamBagCone_Receipt_Date").ToString
                cbo_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                txt_Party_DcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                txt_Book_No.Text = dt1.Rows(0).Item("Book_No").ToString
                cbo_vehicleno.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                cbo_Received.Text = dt1.Rows(0).Item("Received_By").ToString
                cbo_Location.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("Location_Idno").ToString)

                cbo_Transport.Text = Common_Procedures.Transport_IdNoToName(con, dt1.Rows(0).Item("Transport_Idno").ToString)
                dtp_Time.Text = (dt1.Rows(0).Item("Entry_Time_Text").ToString)
                If Val(dt1.Rows(0).Item("UnLoaded_by_Our_employee").ToString) = 1 Then chk_UnLoaded.Checked = True

                If IsDBNull(dt1.Rows(0).Item("PartyDc_Document_Image")) = False Then
                    Dim imageData4 As Byte() = DirectCast(dt1.Rows(0).Item("PartyDc_Document_Image"), Byte())
                    If Not imageData4 Is Nothing Then
                        Using ms9 As New MemoryStream(imageData4, 0, imageData4.Length)
                            ms9.Write(imageData4, 0, imageData4.Length)
                            If imageData4.Length > 0 Then
                                pic_PartyDc_Image.BackgroundImage = Image.FromStream(ms9)
                            End If
                        End Using
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

                If IsDBNull(dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Beam_Width_Name from SizSoft_Empty_BeamBagCone_Receipt_Details a LEFT OUTER JOIN Beam_Width_Head b ON a.Beam_Width_IdNo = b.Beam_Width_IdNo   Where a.Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        Sno = Sno + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_Details.Rows(n).Cells(1).Value = Format(Val(dt2.Rows(i).Item("Empty_Beam").ToString), "#######0")
                        dgv_Details.Rows(n).Cells(2).Value = Common_Procedures.Ledger_IdNoToName(con, dt2.Rows(i).Item("Vendor_Idno").ToString)
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Beam_Width_Name").ToString

                    Next i

                End If
                dt2.Clear()

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Empty_beam").ToString)
                    '.Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Consumption").ToString), "########0.000")
                End With
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))


                lbl_UserName_CreatedBy.Text = ""
                lbl_UserName_ModifiedBy.Text = ""

                If Val(dt1.Rows(0).Item("created_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("created_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("created_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_CreatedBy.Text = "Created by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("created_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("created_DateTime_Text").ToString)
                    Else
                        lbl_UserName_CreatedBy.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("created_useridno").ToString))))
                    End If
                End If
                If Val(dt1.Rows(0).Item("Last_modified_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("Last_modified_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("Last_modified_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_ModifiedBy.Text = "Last Modified by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("Last_modified_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("Last_modified_DateTime_Text").ToString)
                    End If
                End If


            End If

            dt1.Dispose()
            da1.Dispose()
            If LockSTS = True Then

                dtp_Date.Enabled = False
                dtp_Date.BackColor = Color.LightGray

                cbo_Transport.Enabled = False
                cbo_Transport.BackColor = Color.LightGray

                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_vehicleno.Enabled = False
                cbo_vehicleno.BackColor = Color.LightGray




            End If


            Grid_Cell_DeSelect()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_PartyName.Visible And cbo_PartyName.Enabled Then cbo_PartyName.Focus()

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Empty_BeamBagCone_Delivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
                    Exit Sub

                ElseIf Pnl_DosPrint.Visible = True Then
                    btn_Close_DosPrint_Click(sender, e)
                    Exit Sub

                ElseIf pnl_PartyDc_Image.Visible = True Then
                    btn_Close_PartyDc_Image_Click(sender, e)
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

    Private Sub SizSoft_Empty_BeamBagCone_Receipt_Entry_by_Sizing_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub SizSoft_Empty_BeamBagCone_Receipt_Entry_by_Sizing_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Me.Text = ""

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 and b.Close_Status = 0) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        Da.Fill(Dt1)
        cbo_PartyName.DataSource = Dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        Da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from beam_Width_head order by Beam_Width_Name", con)
        Da.Fill(Dt2)
        cbo_beamwidth.DataSource = Dt2
        cbo_beamwidth.DisplayMember = "Beam_Width_Name"

        Da = New SqlClient.SqlDataAdapter("select distinct(vehicle_No) from SizSoft_Empty_BeamBagCone_Receipt_Head order by Vehicle_No", con)
        Da.Fill(dt3)
        cbo_vehicleno.DataSource = dt3
        cbo_vehicleno.DisplayMember = "Vehicle_No"

        Da = New SqlClient.SqlDataAdapter("select distinct(Received_By) from SizSoft_Empty_BeamBagCone_Receipt_Head order by Received_By", con)
        Da.Fill(dt4)
        cbo_Received.DataSource = dt4
        cbo_Received.DisplayMember = "Received_By"

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0
        chk_UnLoaded.Visible = True
        Pnl_DosPrint.Visible = False
        Pnl_DosPrint.BringToFront()
        Pnl_DosPrint.Left = (Me.Width - Pnl_DosPrint.Width) \ 2
        Pnl_DosPrint.Top = (Me.Height - Pnl_DosPrint.Height) \ 2

        pnl_PartyDc_Image.Visible = False
        pnl_PartyDc_Image.Top = (Me.Height - pnl_PartyDc_Image.Height) \ 2
        pnl_PartyDc_Image.Left = (Me.Width - pnl_PartyDc_Image.Width) \ 2
        pnl_PartyDc_Image.BringToFront()


        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = (Me.Height - pnl_filter.Height) \ 2

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
        If Common_Procedures.settings.CustomerCode = "1282" Then

            chk_UnLoaded.Visible = True
        Else
            chk_UnLoaded.Visible = False
        End If

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_beamwidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vendor.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Party_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Book_No.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vehicleno.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Received.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_FilterFrom_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterTo_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_UnLoaded.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Location.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Location.LostFocus, AddressOf ControlLostFocus


        AddHandler btn_filtershow.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_closefilter.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Preprint.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler Btn_DosPrint.GotFocus, AddressOf ControlGotFocus
        AddHandler Btn_LaserPrint.GotFocus, AddressOf ControlGotFocus
        AddHandler Btn_DosCancel.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_filtershow.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_closefilter.GotFocus, AddressOf ControlGotFocus


        'AddHandler chk_Loaded.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vendor.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_beamwidth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Party_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Book_No.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vehicleno.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Received.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_FilterFrom_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FilterTo_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_filtershow.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_closefilter.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Preprint.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler Btn_DosPrint.LostFocus, AddressOf ControlLostFocus
        AddHandler Btn_LaserPrint.LostFocus, AddressOf ControlLostFocus
        AddHandler Btn_DosCancel.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Party_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Book_No.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_remarks.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_UnLoaded.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Party_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Book_No.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_remarks.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler chk_UnLoaded.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then
        dtp_Time.Visible = True
        ' End If


        FrmLdSTS = True
        new_record()

    End Sub



    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '-------
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

        Try

            'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.EmptyBeam_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.EmptyBeam_Receipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

            inpno = InputBox("Enter New Receipt No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Empty_BeamBagCone_Receipt_No from SizSoft_Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'"
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
                    MessageBox.Show("Invalid Receipt No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_ReceiptNo.Text = Trim(UCase(inpno))

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
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Receipt_No from SizSoft_Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Empty_BeamBagCone_Receipt_No"
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
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Receipt_No from SizSoft_Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Empty_BeamBagCone_Receipt_No desc"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ReceiptNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Receipt_No from SizSoft_Empty_BeamBagCone_Receipt_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Empty_BeamBagCone_Receipt_No"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ReceiptNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Empty_BeamBagCone_Receipt_No from SizSoft_Empty_BeamBagCone_Receipt_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Empty_BeamBagCone_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Empty_BeamBagCone_Receipt_No desc"
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

            lbl_ReceiptNo.Text = Common_Procedures.get_MaxCode(con, "SizSoft_Empty_BeamBagCone_Receipt_Head", "Empty_BeamBagCone_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_ReceiptNo.ForeColor = Color.Red

            dtp_Time.Text = Format(Now, "hh:mm tt").ToString

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

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

            inpno = InputBox("Enter Receipt No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Empty_BeamBagCone_Receipt_No from SizSoft_Empty_BeamBagCone_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Receipt No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '-------
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

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

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vehicleno.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vehicleno, cbo_Transport, "", "", "", "", False)
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vehicleno.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vehicleno, txt_Book_No, cbo_Transport, "", "", "", "")
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_FilterTo_date, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_filtershow_Click(sender, e)
        End If
    End Sub





    Private Sub btn_closefilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_closefilter.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False

    End Sub

    Private Sub btn_filtershow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
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
                Condt = "a.Empty_BeamBagCone_Receipt_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Empty_BeamBagCone_Receipt_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Empty_BeamBagCone_Receipt_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from SizSoft_Empty_BeamBagCone_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Empty_BeamBagCone_Receipt_No", con)
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = " " & dt2.Rows(i).Item("Empty_BeamBagCone_Receipt_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Empty_BeamBagCone_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Empty_beam").ToString

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



    Private Sub dgv_filter_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellEndEdit
        SendKeys.Send("{UP}")
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub dgv_filter_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellEnter
        With dgv_filter

            If Val(.Rows(e.RowIndex).Cells(0).Value) = 0 Then
                .Rows(e.RowIndex).Cells(0).Value = e.RowIndex + 1
            End If
        End With
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
        Dim movno As String

        movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_back.Enabled = True
            pnl_filter.Visible = False
        End If

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, dtp_Date, cbo_Received, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_Received, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
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
                        dtp_Date.Focus()
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
                        dtp_Date.Focus()
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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
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
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize
        Dim inpno As String = ""

        Dim Def_PrntrNm As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from SizSoft_Empty_BeamBagCone_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.Vendor_Idno , a.Empty_Beam AS BEAMS, b.Beam_Width_name,c.Vendor_MainName from SizSoft_Empty_BeamBagCone_Receipt_Details a LEFT OUTER JOIN Beam_Width_hEAD b ON a.Beam_Width_IdNo = b.Beam_Width_IdNo LEFT OUTER JOIN Vendor_hEAD c ON a.Vendor_IdNo = c.Vendor_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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

        '        If PpSzSTS = False Then
        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

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

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Print_PDF_Status = True Then
                    Def_PrntrNm = PrintDocument1.PrinterSettings.PrinterName
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

                    PrintDocument1.PrinterSettings.PrinterName = Trim(Def_PrntrNm)
                Else



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

                AddHandler ppd.Shown, AddressOf PrintPreview_Shown
                ppd.ShowDialog()


            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If


        Print_PDF_Status = False
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt = New DataTable
        prn_PageNo = 0
        prn_Count = 0
        prn_DetIndx = 0
        prn_DetMxIndx = 0
        prn_NoofBmDets = 0
        Erase prn_DetAr

        prn_DetAr = New String(50, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Beam_Width_Name, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from SizSoft_Empty_BeamBagCone_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = csh.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON c.ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_IdNo = d.Beam_Width_IdNo  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                da2 = New SqlClient.SqlDataAdapter("select a.Vendor_Idno , a.Empty_Beam AS BEAMS, b.Beam_Width_name,c.Vendor_MainName from SizSoft_Empty_BeamBagCone_Receipt_Details a LEFT OUTER JOIN Beam_Width_hEAD b ON a.Beam_Width_IdNo = b.Beam_Width_IdNo LEFT OUTER JOIN Vendor_hEAD c ON a.Vendor_IdNo = c.Vendor_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(i).Item("BEAMS").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(Val(prn_DetMxIndx))
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Beam_Width_name").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = (prn_DetDt.Rows(i).Item("Vendor_MainName").ToString)
                            prn_DetAr(prn_DetMxIndx, 4) = (prn_DetDt.Rows(i).Item("BEAMS").ToString)

                        End If
                    Next i
                End If

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

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            Printing_Format4(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Sri Meenakshi Sizing (Somanur)
            Printing_Format5(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '---- BRT  Sizing 
            Printing_Format6(e)
        ElseIf Common_Procedures.settings.Dos_Printing = 1 Then
            If prn_Status = 1 Then
                Printing_Format3()
            Else
                Printing_Format1(e)
            End If
        ElseIf (Common_Procedures.settings.EmptyBeamBagConeReceipt_Print_2Copy_In_SinglePage) = 1 Then
            Printing_Format7(e)
        Else
            Printing_Format1(e)
        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0, CurX As Single = 0
        Dim TxtHgt As Single = 0
        Dim strHeight As Single = 0, strWidth As Single = 0
        'Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String

        Dim PCnt As Integer = 0, PrntCnt As Integer = 0


        If Common_Procedures.settings.CustomerCode = "1038" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1400" Then
            PrntCnt = 1
            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                If PrntCnt2ndPageSTS = False Then
                    PrntCnt = 2
                End If
            End If

            set_PaperSize_For_PrintDocument1()
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
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
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

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        TxtHgt = 18.9 ' 19.75 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

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
            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
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
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        Else
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

        End If


        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "FROM : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        W1 = e.Graphics.MeasureString("PARTY DC.NO : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add1, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)

        If Trim(prn_DetDt.Rows(0).Item("Vendor_MainName").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "VENDOR NAME", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(0).Item("Vendor_MainName").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add2, LMargin + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Book_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add3, LMargin + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "PARTY DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add2, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LOCATION", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.Ledger_IdNoToName(con, prn_HdDt.Rows(0).Item("Location_IdNo"))), LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add4, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then

            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Led_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            End If
        Else
            If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + 10, CurY, 0, 0, pFont)
            End If

        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 5

        ClArr(1) = Val(200) : ClArr(2) = 200
        ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

        Common_Procedures.Print_To_PrintDocument(e, "BEAM WIDTH", LMargin + 100, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO.OF BEAMS", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

        If prn_DetDt.Rows.Count > 0 Then
            For I = 0 To prn_DetDt.Rows.Count - 1
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(I).Item("Beam_Width_Name").ToString, LMargin + 100 + 25, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(I).Item("BEAMS").ToString, LMargin + 100 + ClArr(1) + 25, CurY, 0, 0, pFont)

            Next
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
        BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

        Common_Procedures.Print_To_PrintDocument(e, "We received your " & Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & "(" & BmsInWrds & ") empty beams", LMargin + 100, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "through vehicle no. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Remarks  : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 20, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        End If
        Common_Procedures.Print_To_PrintDocument(e, "Signature of the receiver", LMargin + 20, CurY, 0, 0, pFont)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
        End If

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 35, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                'prn_DetIndx = 0
                prn_PageNo = 0
                ' prn_DetIndx = 0
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
        Dim CurY As Single = 0, CurX As Single = 0
        Dim TxtHgt As Single = 0
        Dim Cnt As Single = 0
        Dim strHeight As Single = 0, strWidth As Single = 0
        'Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        Dim Ledname1 As String
        Dim Ledname2 As String
        Dim i As Integer

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        ''PrintDocument pd = new PrintDocument();
        ''pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        ''pd.Print();

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

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20  ' 50 
            .Right = 55
            .Top = 20   '30
            .Bottom = 35 ' 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        TxtHgt = 17 ' 19.75 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

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
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
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
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        Else
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

        End If


        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "FROM : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        W1 = e.Graphics.MeasureString("PARTY DC.NO : ", pFont).Width

        Ledname1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString)
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

        Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add1, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add2, LMargin + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Book_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add3, LMargin + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "PARTY DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add4, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then

            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "     " & Led_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            End If
        Else
            If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "     " & Led_GstNo, LMargin + 10, CurY, 0, 0, pFont)
            End If

        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 5

        ClArr(1) = Val(100) : ClArr(2) = 180 : ClArr(3) = 70 : ClArr(4) = 110 : ClArr(5) = 180 : ClArr(6) = 70
        'ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

        Common_Procedures.Print_To_PrintDocument(e, "BEAM WIDTH ", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "VENDOR NAME", LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO.OF BEAMS ", LMargin + ClArr(1) + ClArr(2) + 20, CurY, 0, 0, pFont)
        If Trim(Common_Procedures.settings.CustomerCode) <> "1263" Then '----APA TEXTILES INDIA PVT LTD
            Common_Procedures.Print_To_PrintDocument(e, "BEAM WIDTH", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 40, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "VENDOR NAME", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF BEAMS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 20, CurY, 0, 0, pFont)
        End If
        'CurY = CurY + TxtHgt - 5
        'Common_Procedures.Print_To_PrintDocument(e, "WIDTH", LMargin + 20, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "BEAMS", LMargin + ClArr(1) + ClArr(2) + 20, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "WIDTH", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 30, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "BEAMS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 20, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "------------", LMargin + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "---------------------", LMargin + ClArr(1) + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "----------------", LMargin + ClArr(1) + ClArr(2) + 15, CurY, 0, 0, pFont)
        If prn_DetDt.Rows.Count > 5 Then
            Common_Procedures.Print_To_PrintDocument(e, "----------", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 35, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "---------------------", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "----------------", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 15, CurY, 0, 0, pFont)
        End If
        Cnt = 5

        Do While prn_NoofBmDets < prn_DetMxIndx
            prn_DetIndx = prn_DetIndx + 1
            If prn_DetDt.Rows.Count > 0 Then
                ' For I = 0 To prn_DetDt.Rows.Count - 1
                CurY = CurY + TxtHgt
                ' If Cnt < 10 Then
                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + 25, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + 25, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 4))), LMargin + ClArr(1) + ClArr(2) + 25, CurY, 0, 0, pFont)
                    prn_NoofBmDets = prn_NoofBmDets + 1
                End If
                If Val(prn_DetAr(prn_DetIndx + Cnt, 4)) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + Cnt, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 45, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + Cnt, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 25, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + Cnt, 4))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 25, CurY, 0, 0, pFont)
                    prn_NoofBmDets = prn_NoofBmDets + 1
                End If

            End If
        Loop
        'If prn_DetDt.Rows.Count > 11 Then
        '    For I = 0 To prn_DetDt.Rows.Count - 1
        '        CurY = CurY + TxtHgt

        '    Next
        'End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "-----------", LMargin + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "---------------------", LMargin + ClArr(1) + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "----------", LMargin + ClArr(1) + ClArr(2) + 15, CurY, 0, 0, pFont)
        If prn_DetDt.Rows.Count > 5 Then
            Common_Procedures.Print_To_PrintDocument(e, "----------", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 35, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "---------------------", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "---------------", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 15, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt

        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
        BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

        Common_Procedures.Print_To_PrintDocument(e, "We received your " & Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & "(" & BmsInWrds & ") empty beams", LMargin + 50, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "through vehicle no. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 50, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Remarks :   " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 5, CurY, 0, 0, pFont)
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        End If
        Common_Procedures.Print_To_PrintDocument(e, "Signature of the receiver", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 35, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


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
            Common_Procedures.Print_To_PrintDocument(e, "BEAMS RECEIVED NOTE", CurX, CurY, 0, 0, p1Font)


            CurX = LMargin + 80
            CurY = TMargin + 140
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO : " & prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_No").ToString, CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 340
            Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_Date").ToString), "dd-MM-yyyy").ToString, CurX, CurY, 0, 0, p1Font)

            time = TimeOfDay.ToString("h:mm:ss tt")

            CurX = LMargin + 580
            Common_Procedures.Print_To_PrintDocument(e, "TIME : " & (time), CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            CurX = LMargin + 60
            e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 730, CurY)

            CurX = LMargin + 65 ' 40  '150
            CurY = TMargin + 180 ' 122 ' 100
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "To M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, CurX + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, CurX + 20, CurY, 0, 0, pFont)
            End If

            CurX = LMargin + 300 ' 40  '150
            CurY = TMargin + 240 ' 122 ' 100
            Common_Procedures.Print_To_PrintDocument(e, "We have Received  the following", CurX, CurY, 0, 0, pFont)

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

    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        'Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        ''PrintDocument pd = new PrintDocument();
        ''pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        ''pd.Print();

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    'Debug.Print(ps.PaperName)
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
                PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = PS
                e.PageSettings.PaperSize = PS
                Exit For
            End If
        Next

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

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        TxtHgt = 19.75 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

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
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "FROM : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        W1 = e.Graphics.MeasureString("PARTY DC.NO : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Book_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "PARTY DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 5

        ClArr(1) = Val(200) : ClArr(2) = 200
        ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

        Common_Procedures.Print_To_PrintDocument(e, "VENDOR NAME", LMargin + 100, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO.OF BEAMS", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)
        Dim ven_nm As String = ""
        If prn_DetDt.Rows.Count > 0 Then
            For I = 0 To prn_DetDt.Rows.Count - 1
                CurY = CurY + TxtHgt
                ven_nm = ""
                ven_nm = Common_Procedures.Vendor_IdNoToName(con, prn_DetDt.Rows(I).Item("Vendor_Idno").ToString)
                Common_Procedures.Print_To_PrintDocument(e, Trim(ven_nm), LMargin + 100 + 25, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(I).Item("BEAMS").ToString, LMargin + 100 + ClArr(1) + 25, CurY, 0, 0, pFont)
            Next
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
        BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

        Common_Procedures.Print_To_PrintDocument(e, "We received your " & Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & "(" & BmsInWrds & ") empty beams", LMargin + 100, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "through vehicle no. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        End If
        Common_Procedures.Print_To_PrintDocument(e, "Signature of the receiver", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 35, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                'prn_DetIndx = 0
                prn_PageNo = 0
                ' prn_DetIndx = 0
                prn_PageNo = 0
                '  prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

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
                                    dtp_Date.Focus()
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
            If cbo_beamwidth.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from SizSoft_Empty_BeamBagCone_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.Empty_Beam AS BEAMS, b.Beam_Width_name from SizSoft_Empty_BeamBagCone_Receipt_Details a LEFT OUTER JOIN Beam_Width_hEAD b ON a.Beam_Width_IdNo = b.Beam_Width_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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

            PrnTxt = Chr(Vz1) & Space(21) & Chr(14) & Chr(27) & "E" & "EMPTY BEAM RECEIPT" & Chr(27) & "F" & Chr(18) & Space(21) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(LfCon) & StrDup(39, Chr(Hz2)) & Chr(194) & StrDup(38, Chr(Hz2)) & Chr(RgtCon)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(1) & "From & " & Space(31) & Chr(Vz2) & Space(38) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & "M/s." & Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString) & Space(31 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString))) & Chr(Vz2) & Space(1) & "REC NO & " & Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_No").ToString) & Space(28 - Len(Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_No").ToString))) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString))) & Chr(Vz2) & Space(38) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString))) & Chr(Vz2) & Space(1) & "DATE   & " & Trim(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_dATE").ToString), "dd-MM-yyyy").ToString) & Space(28 - Len(Trim(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_DATE").ToString), "dd-MM-yyyy").ToString))) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString))) & Chr(Vz2) & Space(38) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString))) & Chr(Vz2) & Space(1) & "PARTY DC.NO : " & Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) & Space(23 - Len(Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString))) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            'SUB HEADING

            PrnTxt = Chr(LfCon) & StrDup(39, Chr(Hz2)) & Chr(193) & StrDup(38, Chr(Hz2)) & Chr(RgtCon)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(4) & "BEAM WIDTH           NO.OF BEAMS" & Space(42) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & "----------           -----------" & Space(42) & Chr(Vz1)
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

            PrnTxt = Chr(Vz1) & Space(4) & "                     -----------" & Space(42) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(16) & Space(17 - Len(Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))) & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) & Space(45) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & "                     -----------" & Space(42) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(5) & "We received your " & Trim(Str(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))) + " ( " + Trim(BmsInWrds) + " ) empty beams " & Space(38 - Len(Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString & BmsInWrds))) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
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

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub
    Private Sub cbo_Vendor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vendor.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vendor_AlaisHead", "Vendor_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 and (close_status=0))", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_Vendor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vendor.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vendor, Nothing, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 and (close_status=0) )", "(Vendor_IdNo = 0)")
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
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vendor, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 and (close_status=0) )", "(Vendor_IdNo = 0)")
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
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
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

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
            'If Led_IdNo  = 0 Then Exit Sub

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")


            smstxt = "EMPTY BEAM RECEIPT" & vbCrLf
            smstxt = Trim(smstxt) & "-----------------------------" & vbCrLf

            smstxt = smstxt & vbCrLf

            smstxt = Trim(smstxt) & "PARTY NAME : " & Trim(cbo_PartyName.Text) & vbCrLf
            smstxt = Trim(smstxt) & "EMPTYBEAM REC.NO : " & Trim(lbl_ReceiptNo.Text) & vbCrLf & "DATE : " & Trim(dtp_Date.Text)

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then

            '    smstxt = smstxt & "PARTY NAME:" & Trim(cbo_PartyName.Text) & vbCrLf & "EMPTY BEAM REC.NO-" & Trim(lbl_ReceiptNo.Text) & vbCrLf & "DATE-" & Trim(dtp_Date.Text)

            'Else
            '    smstxt = "EMPTY BEAM" & vbCrLf

            '    smstxt = smstxt & "REC.NO-" & Trim(lbl_ReceiptNo.Text) & vbCrLf & "DATE-" & Trim(dtp_Date.Text)

            'End If

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            da2 = New SqlClient.SqlDataAdapter("select Empty_Beam from  SizSoft_Empty_BeamBagCone_Receipt_Head where Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'", con)
            dt2 = New DataTable
            da2.Fill(dt2)
            If dt2.Rows.Count > 0 Then


                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
                    da3 = New SqlClient.SqlDataAdapter("select Beam_Width_Name , Beam_Width_Name as BMwidthName from  SizSoft_Empty_BeamBagCone_Receipt_Details a Left outer join Beam_Width_Head Bwh ON Bwh.Beam_Width_IdNo = a.Beam_Width_IdNo where Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'", con)
                    dt3 = New DataTable
                    da3.Fill(dt3)
                    If dt3.Rows.Count > 0 Then
                        For i = 0 To dt3.Rows.Count - 1

                            smstxt = smstxt & vbCrLf & "No.Of Beam : " & Trim(dt3.Rows(i).Item("Empty_Beam").ToString)
                            If Trim(dt3.Rows(i).Item("BMwidthName").ToString) <> "" Then
                                smstxt = smstxt & "(Width :" & Trim(dt3.Rows(i).Item("BMwidthName").ToString) & ")"
                            End If

                        Next

                    End If
                    dt3.Clear()

                Else

                    smstxt = smstxt & vbCrLf & "No.Of Beam : " & Trim(dt2.Rows(0).Item("Empty_Beam").ToString)

                End If

            End If
            dt2.Clear()

            smstxt = smstxt & vbCrLf & vbCrLf & "Thanks!" & vbCrLf
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
                smstxt = smstxt & "GKT SIZING"

            Else
                smstxt = smstxt & Trim(Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))).ToUpper

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


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Aadhavan Sizing Mill (Somanur)

                Sms_Entry.SMS_TempleteID = "1207162029681635847" 'EBREC1   
            Else
                Sms_Entry.SMS_TempleteID = ""
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
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Received_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Received.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Received, cbo_PartyName, txt_Party_DcNo, "", "", "", "")
    End Sub

    Private Sub cbo_Received_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Received.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Received, txt_Party_DcNo, "", "", "", "")
    End Sub

    Private Sub txt_Party_DcNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Party_DcNo.KeyPress
        If Asc(e.KeyChar) = 40 Then SendKeys.Send("{TAB}")
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

            cmd.CommandText = "Update SizSoft_Empty_BeamBagCone_Receipt_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Receipt_Code = '" & Trim(NewCode) & "'"
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
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_vehicleno, cbo_Location, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_Location, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
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

    Private Sub Printing_Format6(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim pFontBold As Font = New Font("Calibri", 8, FontStyle.Bold)
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0, CurX As Single = 0
        Dim TxtHgt As Single = 0
        Dim strHeight As Single = 0, strWidth As Single = 0
        'Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        Dim cmp_userName As String = "", Cmp_Divi As String = ""
        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        ''PrintDocument pd = new PrintDocument();
        ''pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        ''pd.Print();

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

        ' If PpSzSTS = False Then
        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = PS
        '        e.PageSettings.PaperSize = PS
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next




        If Common_Procedures.settings.CustomerCode = "1282" Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = PS
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    PpSzSTS = True
                    Exit For
                End If
            Next

        Else
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
        ' End If
        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
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

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        TxtHgt = 20 ' 19.75 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        CurY = TMargin
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
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
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
            p1Font = New Font("Calibri", 8.2, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 7, FontStyle.Regular)
        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, p1Font)
        Else
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), p1Font).Width
            If PrintWidth > strWidth Then
                CurX = LMargin + (PrintWidth - strWidth) / 2
            Else
                CurX = LMargin
            End If

            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
            strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, p1Font)

            strWidth = e.Graphics.MeasureString(Cmp_StateNm, p1Font).Width
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, p1Font)

        End If


        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "FROM : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 260
        C2 = PageWidth - (LMargin + C1)

        W1 = e.Graphics.MeasureString("PARTY DC.NO : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add1, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFontBold)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFontBold)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFontBold)

        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)

        p1Font = New Font("Calibri", 8, FontStyle.Bold)

        If Trim(prn_DetDt.Rows(0).Item("Vendor_MainName").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Vendor Name", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(0).Item("Vendor_MainName").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt

        'Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add1, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TIME", LMargin + C1 + 10, CurY, 0, 0, pFontBold)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFontBold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFontBold)

        If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add2, LMargin + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Book_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add3, LMargin + 10, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "PARTY DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add4, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then

            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Led_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            End If
        Else
            If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + 10, CurY, 0, 0, pFont)
            End If

        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 5

        ClArr(1) = Val(200) : ClArr(2) = 200
        ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

        'Common_Procedures.Print_To_PrintDocument(e, "BEAM WIDTH", LMargin + 100, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "NO.OF BEAMS", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO.OF BEAMS", LMargin + 100, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

        If prn_DetDt.Rows.Count > 0 Then
            For I = 0 To prn_DetDt.Rows.Count - 1
                CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(I).Item("Beam_Width_Name").ToString, LMargin + 100 + 25, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(I).Item("BEAMS").ToString, LMargin + 100 + ClArr(1) + 25, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(I).Item("BEAMS").ToString, LMargin + 100 + 25, CurY, 0, 0, pFont)
            Next
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
        BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

        Common_Procedures.Print_To_PrintDocument(e, "We received your " & Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & "(" & BmsInWrds & ") empty beams", LMargin + 100, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt


        If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Remarks  : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 20, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        End If

        'Common_Procedures.Print_To_PrintDocument(e, "through vehicle no. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, p1Font)


        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
        Else
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        End If




        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            cmp_userName = Trim(Common_Procedures.User.Name)
            'Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        End If



        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt - 5
        Cmp_Divi = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Divi, PageWidth - 60, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt - 3
        Common_Procedures.Print_To_PrintDocument(e, cmp_userName, PageWidth - 60, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Signature of the receiver", LMargin + 20, CurY, 0, 0, pFont)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 180, CurY, 0, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
        End If



        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                'prn_DetIndx = 0
                prn_PageNo = 0
                ' prn_DetIndx = 0
                prn_PageNo = 0
                '  prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub


    Private Sub Printing_Format7(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0, CurX As Single = 0
        Dim TxtHgt As Single = 0
        Dim strHeight As Single = 0, strWidth As Single = 0
        'Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0


        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        ''PrintDocument pd = new PrintDocument();
        ''pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        ''pd.Print();

        If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then

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

        Else

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
        End If

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = PS
        '        e.PageSettings.PaperSize = PS
        '        Exit For
        '    End If
        'Next

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

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        TxtHgt = 18.9 ' 19.75 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

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
                Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
            Else
                Led_Name = prn_HdDt.Rows(0).Item("Ledger_Name").ToString
                Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
                Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
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
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt

            Gst_dt = #7/1/2017#
            Entry_dt = dtp_Date.Value

            If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
            Else
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

            End If


            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            CurY = CurY + strHeight
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "FROM : ", LMargin + 10, CurY, 0, 0, pFont)

            C1 = 450
            C2 = PageWidth - (LMargin + C1)

            W1 = e.Graphics.MeasureString("PARTY DC.NO : ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add1, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Empty_BeamBagCone_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)

            If Trim(prn_DetDt.Rows(0).Item("Vendor_MainName").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "VENDOR NAME", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(0).Item("Vendor_MainName").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add2, LMargin + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Book_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add3, LMargin + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PARTY DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add4, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Led_TinNo, LMargin + 10, CurY, 0, 0, pFont)
                End If
            Else
                If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + 10, CurY, 0, 0, pFont)
                End If

            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 5

            ClArr(1) = Val(200) : ClArr(2) = 200
            ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

            Common_Procedures.Print_To_PrintDocument(e, "BEAM WIDTH", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF BEAMS", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

            If prn_DetDt.Rows.Count > 0 Then
                For I = 0 To prn_DetDt.Rows.Count - 1
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(I).Item("Beam_Width_Name").ToString, LMargin + 100 + 25, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(I).Item("BEAMS").ToString, LMargin + 100 + ClArr(1) + 25, CurY, 0, 0, pFont)

                Next
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "-------------------", LMargin + 100 + ClArr(1), CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            Common_Procedures.Print_To_PrintDocument(e, "We received your " & Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & "(" & BmsInWrds & ") empty beams", LMargin + 100, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "through vehicle no. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 100, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Remarks  : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 20, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            End If
            Common_Procedures.Print_To_PrintDocument(e, "Signature of the receiver", LMargin + 20, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
                Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            End If

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, PageWidth - 35, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5
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

                'prn_DetIndx = 0
                prn_PageNo = 0
                ' prn_DetIndx = 0
                prn_PageNo = 0
                '  prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
        Print_PDF_Status = False
    End Sub

    Private Sub cbo_Location_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Location.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Location_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Location.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Location, cbo_vehicleno, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_Location.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1282" Then

                chk_UnLoaded.Focus()

            Else
                txt_remarks.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_Location_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Location.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Location, chk_UnLoaded, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1282" Then

                chk_UnLoaded.Focus()

            Else
                txt_remarks.Focus()

            End If
        End If
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

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub btn_Add_PartyDc_Image_Click(sender As Object, e As EventArgs) Handles btn_Add_PartyDc_Image.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pic_PartyDc_Image.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub btn_Delete_PartyDc_Image_Click(sender As Object, e As EventArgs) Handles btn_Delete_PartyDc_Image.Click
        pic_PartyDc_Image.BackgroundImage = Nothing
    End Sub

    Private Sub btn_Enlarge_PartyDc_Image_Click(sender As Object, e As EventArgs) Handles btn_Enlarge_PartyDc_Image.Click
        Dim f As New Enlarge_Image(pic_PartyDc_Image.BackgroundImage)
        f.MdiParent = MDIParent1
        f.Show()
    End Sub

    Private Sub btn_PartyDc_Image_Click(sender As Object, e As EventArgs) Handles btn_PartyDc_Image.Click
        pnl_PartyDc_Image.Visible = True
        pnl_back.Enabled = False
        btn_Add_PartyDc_Image.Focus()
    End Sub

    Private Sub btn_Close_PartyDc_Image_Click(sender As Object, e As EventArgs) Handles btn_Close_PartyDc_Image.Click
        pnl_back.Enabled = True
        pnl_PartyDc_Image.Visible = False
    End Sub

End Class