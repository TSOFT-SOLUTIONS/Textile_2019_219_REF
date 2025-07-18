Public Class Pavu_Sales_VAT
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PVSAL-"
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
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        chk_SelectAll.Checked = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        txt_KuraiPavuBeam.Text = ""
        txt_KuraiPavuMeter.Text = ""
        txt_Freight.Text = ""
        txt_RateMeters.Text = ""
        txt_AddLess.Text = ""
        lbl_GrossAmount.Text = ""
        lbl_NetAmount.Text = ""
        cbo_DelvAt.Text = ""
        cbo_EndsCount.Text = ""
        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        cbo_RecForm.Text = ""
        cbo_SalesAc.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_PavuDetails.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_EndsCountName.Text = ""

            dgv_Filter_Details.Rows.Clear()
        End If

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


        If Me.ActiveControl.Name <> dgv_PavuDetails_Total.Name Then
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
        dgv_PavuDetails.CurrentCell.Selected = False
        dgv_PavuDetails_Total.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_PavuDetails_Total.CurrentCell) Then dgv_PavuDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Weaver_Pavu_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DelvAt.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DelvAt.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RecForm.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_RecForm.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Weaver_Pavu_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Weaver_Pavu_Delivery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type <> '') order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_DelvAt.DataSource = dt1
        cbo_DelvAt.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'TRANSPORT' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Transport.DataSource = dt2
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
        da.Fill(dt3)
        cbo_EndsCount.DataSource = dt3
        cbo_EndsCount.DisplayMember = "EndsCount_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where  Ledger_Type = 'GODOWN'  order by Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_RecForm.DataSource = dt4
        cbo_RecForm.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Vehicle_No from Pavu_Sales_Head order by Vehicle_No", con)
        da.Fill(dt5)
        cbo_VehicleNo.DataSource = dt5
        cbo_VehicleNo.DisplayMember = "Vehicle_No"

        dtp_Date.Text = ""
        msk_date.Text = ""
        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DelvAt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RecForm.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_KuraiPavuBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_KuraiPavuMeter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SetNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RateMeters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAc.GotFocus, AddressOf ControlGotFocus



        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsCountName.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DelvAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RecForm.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuMeter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_BeamNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SetNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RateMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAc.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsCountName.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_KuraiPavuBeam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_KuraiPavuMeter.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_RateMeters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_KuraiPavuBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_KuraiPavuMeter.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_RateMeters.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Weaver_Pavu_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as DelvName, c.Ledger_Name as TransportName, d.EndsCount_Name, e.Ledger_Name as RecFromName from Pavu_Sales_Head a INNER JOIN Ledger_Head b ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.ReceivedFrom_IdNo = e.Ledger_IdNO Where a.Pavu_Sales_Code = '" & Trim(NewCode) & "' AND a.Entry_VAT_GST_Type <> 'GST'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_DcNo.Text = dt1.Rows(0).Item("Pavu_Sales_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Pavu_Sales_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_DelvAt.Text = dt1.Rows(0).Item("DelvName").ToString
                txt_KuraiPavuBeam.Text = Val(dt1.Rows(0).Item("Empty_Beam").ToString)
                txt_KuraiPavuMeter.Text = Val(dt1.Rows(0).Item("Pavu_Meters").ToString)
                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_Transport.Text = dt1.Rows(0).Item("TransportName").ToString
                cbo_RecForm.Text = dt1.Rows(0).Item("RecFromName").ToString
                txt_Freight.Text = Val(dt1.Rows(0).Item("Freight").ToString)

                cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))

                txt_RateMeters.Text = Format(Val(dt1.Rows(0).Item("Rate_Meters").ToString), "########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("Add_Less").ToString), "########0.00")
                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Pavu_Delivery_Increment, c.EndsCount_Name, d.Beam_Width_Name from Pavu_Sales_Details a INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_Idno = d.Beam_Width_Idno where a.Pavu_Sales_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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
                        If Val(dt2.Rows(i).Item("Meters_Pc").ToString) <> 0 Then
                            dgv_PavuDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Meters_Pc").ToString)
                        End If
                        dgv_PavuDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        dgv_PavuDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                        dgv_PavuDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Beam_Width_Name").ToString

                        dgv_PavuDetails.Rows(n).Cells(8).Value = ""
                        dgv_PavuDetails.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Noof_Used").ToString
                        dgv_PavuDetails.Rows(n).Cells(10).Value = dt2.Rows(i).Item("set_code").ToString
                        dgv_PavuDetails.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Pavu_Delivery_Increment").ToString

                        If Val(dgv_PavuDetails.Rows(n).Cells(9).Value) > 0 And Val(dgv_PavuDetails.Rows(n).Cells(9).Value) <> Val(dgv_PavuDetails.Rows(n).Cells(11).Value) Then
                            dgv_PavuDetails.Rows(n).Cells(8).Value = "1"
                        End If

                    Next i

                End If

                With dgv_PavuDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Beam").ToString)
                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                End With
                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")

                dt2.Clear()

            Else
                new_record()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

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
        Dim Nr As Long = 0

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            Da = New SqlClient.SqlDataAdapter("Select * from Pavu_Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(NewCode) & "'", con)
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
                        Throw New ApplicationException("Some Beams Delivered to Others - Beam No : " & Trim(Dt1.Rows(i).Item("Beam_No").ToString))
                        Exit Sub
                    End If

                Next
            End If
            Dt1.Clear()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Pavu_Sales_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Pavu_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where ( Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
            da.Fill(dt3)
            cbo_Filter_EndsCountName.DataSource = dt3
            cbo_Filter_EndsCountName.DisplayMember = "EndsCount_Name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_EndsCountName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1

            cbo_Filter_EndsCountName.SelectedIndex = -1

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

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Sales_No from Pavu_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Entry_VAT_GST_Type <> 'GST' Order by for_Orderby, Pavu_Sales_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Sales_No from Pavu_Sales_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Entry_VAT_GST_Type <> 'GST' Order by for_Orderby, Pavu_Sales_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Sales_No from Pavu_Sales_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Entry_VAT_GST_Type <> 'GST' Order by for_Orderby desc, Pavu_Sales_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Sales_No from Pavu_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Entry_VAT_GST_Type <> 'GST' Order by for_Orderby desc, Pavu_Sales_No desc", con)
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
        Dim dt1 As New DataTable
        Try
            clear()

            New_Entry = True

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Pavu_Sales_Head", "Pavu_Sales_Code", "For_OrderBy", "Entry_VAT_GST_Type <> 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Pavu_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Pavu_Sales_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Pavu_Sales_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Pavu_Sales_Date").ToString
                End If
            End If
            dt1.Clear()


            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus() : msk_date.SelectionStart = 0

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

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Pavu_Sales_No from Pavu_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(RecCode) & "'", con)
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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Pavu_Sales_No from Pavu_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Clo_ID As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim KuPvu_EdsCnt_ID As Integer = 0
        Dim SzPvu_EdsCnt_ID As Integer = 0
        Dim vEdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotPvuBms As Single, vTotPvuMtrs As Single, vTotPvuPcs As Single
        Dim YCnt_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim EntID As String = ""
        Dim Bw_IdNo As Integer = 0
        Dim Pavu_DelvInc As Integer = 0
        Dim Ent_NoofUsed As Integer = 0
        Dim Stock_In As String
        Dim mtrspcs As Integer
        Dim dt2 As New DataTable
        Dim vTotPvuStk As Single = 0
        Dim SalesAc_ID As Integer = 0
        Dim VouBil As String = ""
        Dim Partc_AC As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
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

        KuPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
            If KuPvu_EdsCnt_ID = 0 Then
                MessageBox.Show("Invalid EndsCount Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
                Exit Sub
            End If
        End If

        Delv_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        If Delv_ID = 0 Then
            MessageBox.Show("Invalid Delivery Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_DelvAt.Enabled And cbo_DelvAt.Visible Then cbo_DelvAt.Focus()
            Exit Sub
        End If
        lbl_UserName.Text = Common_Procedures.User.IdNo
        Rec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)
        If Rec_ID = 0 Then Rec_ID = 4

        If Delv_ID = Rec_ID Then
            MessageBox.Show("Invalid Party Name" & Chr(13) & "Does not accept same party", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_DelvAt.Enabled And cbo_DelvAt.Visible Then cbo_DelvAt.Focus()
            Exit Sub
        End If
        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        SalesAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAc.Text)
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        With dgv_PavuDetails

            For i = 0 To .RowCount - 1

                If Val(dgv_PavuDetails.Rows(i).Cells(5).Value) <> 0 Then

                    If Trim(dgv_PavuDetails.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_PavuDetails.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(2)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_PavuDetails.Rows(i).Cells(10).Value) = "" Then
                        MessageBox.Show("Invalid Set Code", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                    vEdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(6).Value)
                    If Val(vEdsCnt_ID) = 0 Then
                        MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(6)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                End If

            Next
        End With

        vTotPvuBms = 0 : vTotPvuMtrs = 0 : vTotPvuPcs = 0
        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTotPvuBms = Val(dgv_PavuDetails_Total.Rows(0).Cells(2).Value())
            vTotPvuPcs = Val(dgv_PavuDetails_Total.Rows(0).Cells(3).Value())
            vTotPvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(5).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Pavu_Sales_Head", "Pavu_Sales_Code", "For_OrderBy", "Entry_VAT_GST_Type <> 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Pavu_Sales_Head ( Pavu_Sales_Code, Company_IdNo, Pavu_Sales_No, for_OrderBy, Pavu_Sales_Date, DeliveryTo_IdNo, ReceivedFrom_IdNo, SalesAc_IdNo,EndsCount_IdNo, Pavu_Meters, Empty_Beam, Vehicle_No , Transport_Idno, Total_Beam , Total_Pcs, Total_Meters ,Freight, Rate_Meters, Gross_Amount, Add_Less, Net_Amount,  User_IdNo  ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Delv_ID)) & ",  " & Val(Rec_ID) & " , " & Str(Val(SalesAc_ID)) & ", " & Str(Val(KuPvu_EdsCnt_ID)) & "," & Val(txt_KuraiPavuMeter.Text) & " , " & Val(txt_KuraiPavuBeam.Text) & ",  '" & Trim(cbo_VehicleNo.Text) & "' , " & Val(Trans_ID) & ", " & Str(Val(vTotPvuBms)) & " , " & Str(Val(vTotPvuPcs)) & " , " & Str(Val(vTotPvuMtrs)) & " ," & Val(txt_Freight.Text) & " ," & Val(txt_RateMeters.Text) & "," & Val(lbl_GrossAmount.Text) & "," & Val(txt_AddLess.Text) & "," & Val(lbl_NetAmount.Text) & " ," & Val(lbl_UserName.Text) & ") "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Pavu_Sales_Head set Pavu_Sales_Date = @EntryDate, DeliveryTo_IdNo = " & Str(Val(Delv_ID)) & ", ReceivedFrom_IdNo = " & Str(Val(Rec_ID)) & ", Empty_Beam = " & Str(Val(txt_KuraiPavuBeam.Text)) & ", Pavu_Meters = " & Str(Val(txt_KuraiPavuMeter.Text)) & ", SalesAc_IdNo =" & Str(Val(SalesAc_ID)) & ",EndsCount_IdNo = " & Str(Val(KuPvu_EdsCnt_ID)) & ", Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' , Transport_Idno = " & Str(Val(Trans_ID)) & ", Freight = " & Str(Val(txt_Freight.Text)) & ", Total_Beam = " & Str(Val(vTotPvuBms)) & ", Total_Pcs = " & Str(Val(vTotPvuPcs)) & ", Total_Meters = " & Str(Val(vTotPvuMtrs)) & " ,Rate_Meters =" & Val(txt_RateMeters.Text) & ",Gross_Amount = " & Val(lbl_GrossAmount.Text) & ",Add_Less = " & Val(txt_AddLess.Text) & ",Net_Amount = " & Val(lbl_NetAmount.Text) & ",User_idNo = " & Val(lbl_UserName.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("Select * from Pavu_Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(NewCode) & "'", con)
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

            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            Partcls = "Delv : Dc.No. " & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)

            cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Pavu_Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(txt_KuraiPavuMeter.Text) <> 0 And Val(KuPvu_EdsCnt_ID) <> 0 Then

                cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(KuPvu_EdsCnt_ID)) & ", " & Str(Val(txt_KuraiPavuBeam.Text)) & ", " & Str(Val(txt_KuraiPavuMeter.Text)) & ")"
                cmd.ExecuteNonQuery()

            End If

            With dgv_PavuDetails
                Sno = 0
                Partc_AC = ""
                For i = 0 To dgv_PavuDetails.RowCount - 1

                    If Val(dgv_PavuDetails.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        SzPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(6).Value, tr)
                        Bw_IdNo = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(i).Cells(7).Value, tr)

                        Partc_AC = Partc_AC & "-" & "Set : " & Trim(.Rows(i).Cells(1).Value) & " ,Beam : " & Trim(.Rows(i).Cells(2).Value) & ", Meter : " & Val(dgv_PavuDetails.Rows(i).Cells(5).Value)

                        Ent_NoofUsed = 0
                        If Val(.Rows(i).Cells(9).Value) = 0 Or (Val(.Rows(i).Cells(9).Value) > 0 And Val(.Rows(i).Cells(9).Value) = Val(.Rows(i).Cells(11).Value)) Then

                            Nr = 0
                            cmd.CommandText = "update Stock_SizedPavu_Processing_Details set StockAt_IdNo = " & Str(Val(Delv_ID)) & ", Pavu_Delivery_Increment = Pavu_Delivery_Increment + 1 " & _
                                                        " Where  Set_Code = '" & Trim(.Rows(i).Cells(10).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and StockAt_IdNo = " & Str(Val(Rec_ID))
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                Throw New ApplicationException("Mismath Received From Name and Beam Details")
                                Exit Sub
                            End If

                            Ent_NoofUsed = Val(Common_Procedures.get_FieldValue(con, "Stock_SizedPavu_Processing_Details", "Pavu_Delivery_Increment", "(Set_Code = '" & Trim(.Rows(i).Cells(10).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' )", , tr))

                        Else
                            Ent_NoofUsed = Val(.Rows(i).Cells(9).Value)

                        End If

                        cmd.CommandText = "Insert into Pavu_Sales_Details ( Pavu_Sales_Code,              Company_IdNo        ,     Pavu_Sales_No     ,                               for_OrderBy                          , Pavu_Sales_Date,         DeliveryTo_IdNo  ,    ReceivedFrom_IdNo     ,          Sl_No        ,                    Set_No              ,                    Beam_No             ,                      Pcs                 ,                      Meters_Pc           ,                      Meters              ,             EndsCount_IdNo       ,      Beam_Width_IdNo     ,              Noof_Used        ,                  Set_Code                ) " & _
                                                    "            Values  (   '" & Trim(NewCode) & "'           , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",        @EntryDate       ,   " & Str(Val(Delv_ID)) & ",  " & Str(Val(Rec_ID)) & ",  " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(SzPvu_EdsCnt_ID)) & ", " & Str(Val(Bw_IdNo)) & ", " & Str(Val(Ent_NoofUsed)) & ", '" & Trim(.Rows(i).Cells(10).Value) & "' ) "
                        cmd.ExecuteNonQuery()


                        cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(SzPvu_EdsCnt_ID)) & ", 1, " & Str(Val(.Rows(i).Cells(5).Value)) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                Next

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

                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(vTotPvuStk)) & " )"
                    cmd.ExecuteNonQuery()

                Next
            End If
            Dt1.Clear()

            If Val(txt_KuraiPavuBeam.Text) <> 0 Or Val(vTotPvuBms) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Entry_ID, Particulars, Sl_No, Beam_Width_IdNo, Empty_Beam, Pavu_Beam) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(PBlNo) & "', '" & Trim(EntID) & "', '" & Trim(Partcls) & "', 1, 0, 1, " & Str(Val(txt_KuraiPavuBeam.Text) + Val(vTotPvuBms)) & ")"
                cmd.ExecuteNonQuery()
            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            If Val(lbl_NetAmount.Text) <> 0 Then
                vLed_IdNos = Led_ID & "|" & SalesAc_ID
                vVou_Amts = -1 * Val(CSng(lbl_NetAmount.Text)) & "|" & (Val(CSng(lbl_NetAmount.Text)))
                If Common_Procedures.Voucher_Updation(con, "Pavu.Sales", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(dtp_Date.Text), Trim(Partc_AC), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If
            End If


            'Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, Led_ID, Trim(lbl_DcNo.Text), 0, Val(lbl_NetAmount.Text), "CR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

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

    Private Sub cbo_DelvAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DelvAt.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_DelvAt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvAt.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelvAt, msk_date, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DelvAt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DelvAt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DelvAt, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DelvAt_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvAt.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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

    Private Sub cbo_RecForm_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RecForm.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_Type = 'GODOWN'", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_RecForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecForm, cbo_DelvAt, cbo_SalesAc, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_Type = 'GODOWN'", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_RecForm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RecForm.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecForm, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_Type = 'GODOWN'", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Pavu :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                cbo_SalesAc.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_RecForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RecForm.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAc, cbo_RecForm, cbo_EndsCount, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAc, cbo_EndsCount, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            'Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")

    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, cbo_RecForm, txt_KuraiPavuMeter, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_KuraiPavuMeter, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
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

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_VehicleNo, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
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

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Pavu_Sales_Head", "Vehicle_No", "", "")

    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, txt_KuraiPavuBeam, cbo_Transport, "Pavu_Sales_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, cbo_Transport, "Pavu_Sales_Head", "Vehicle_No", "", "", False)
    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown

        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            'If dgv_PavuDetails.Rows.Count > 0 Then
            '    dgv_PavuDetails.Focus()
            '    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)

            'Else
            txt_RateMeters.Focus()

            'End If
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            'If dgv_PavuDetails.Rows.Count > 0 Then
            '    dgv_PavuDetails.Focus()
            '    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)

            'Else
            txt_RateMeters.Focus()

            '   End If
        End If

    End Sub

    Private Sub txt_KuraiPavuMeter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_KuraiPavuMeter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_KuraiPavuBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_KuraiPavuBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, EdsCnt_IdNo As Integer
        Dim Condt As String = ""
        Dim EdsEdsCnt_IdNo As Integer, Mil_IdNo As Integer

        Try

            Condt = ""
            Led_IdNo = 0
            EdsCnt_IdNo = 0
            Mil_IdNo = 0
            EdsEdsCnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Pavu_Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Pavu_Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Pavu_Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_EndsCountName.Text) <> "" Then
                EdsCnt_IdNo = Common_Procedures.EndsCount_NameToIdNo(con, cbo_Filter_EndsCountName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.DeliveryTo_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If

            If Val(EdsCnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo)) & " or d.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo)) & ") "
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo)) & " "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.EndsCount_Name from Pavu_Sales_Head a INNER JOIN Pavu_Sales_Details d on a.Pavu_Sales_Code = d.Pavu_Sales_Code INNER JOIN Ledger_Head b on a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head c on d.EndsCount_IdNo = c.EndsCount_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Pavu_Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Pavu_Sales_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Pavu_Sales_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Pavu_Sales_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("EndsCount_Name").ToString

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

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER')", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_EndsCountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER')", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_EndsCountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_EndsCountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsCountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")

    End Sub


    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsCountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsCountName, cbo_Filter_PartyName, btn_Filter_Show, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsCountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsCountName, btn_Filter_Show, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_Details = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_PavuDetails.EditingControl.BackColor = Color.Lime
        dgv_PavuDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        'TotalPavu_Calculation()
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
            If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then
            With dgv_PavuDetails
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 5 Then
                        TotalPavu_Calculation()
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub dgv_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyDown

        On Error Resume Next

        With dgv_PavuDetails

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True
                    txt_Freight.Focus()
                End If
            End If

            If e.KeyCode = Keys.Down Then
                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True

                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    btn_save.Focus()
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

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then
            With dgv_PavuDetails
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With
        End If
    End Sub

    Private Sub TotalPavu_Calculation()
        Dim Sno As Integer
        Dim TotBms As Single, TotMtrs As Single, TotPcs As Single

        Sno = 0
        TotBms = 0
        TotPcs = 0
        TotMtrs = 0
        With dgv_PavuDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(5).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(5).Value)
                    TotPcs = TotPcs + Val(.Rows(i).Cells(3).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotBms)
            .Rows(0).Cells(3).Value = Val(TotPcs)
            .Rows(0).Cells(5).Value = Format(Val(TotMtrs), "########0.00")
        End With
        lbl_GrossAmount.Text = Format(Val(txt_RateMeters.Text) * (Val(dgv_PavuDetails_Total.Rows(0).Cells(5).Value) + Val(txt_KuraiPavuMeter.Text)), "##########0.00")
        lbl_NetAmount.Text = Format(Val(lbl_GrossAmount.Text) + Val(txt_AddLess.Text), "########0.00")
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.* , d.EndsCount_Name , e.Ledger_Name as Trasport_Name from Pavu_Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.ReceivedFrom_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo  where a.Pavu_Sales_Code = '" & Trim(NewCode) & "'", con)
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

        prn_TotCopies = 1
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Sizing (Somanur)
            prn_TotCopies = Val(InputBox("Enter No.of Copies", "FOR SALES PRINTING...", "2"))
            If Val(prn_TotCopies) <= 0 Then
                Exit Sub
            End If
        End If


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
                        '        'e.PageSettings.PaperSize = ps
                        '        PpSzSTS = True
                        '        Exit For
                        '    End If
                        'Next

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

                    PrintDocument1.Print()

                End If
            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

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
        prn_PageNo = 0
        prn_NoofBmDets = 0
        prn_DetMxIndx = 0
        prn_Count = 0
        Erase prn_DetAr

        prn_DetAr = New String(50, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.Ledger_Name as Receiver_Name , d.EndsCount_Name , e.Ledger_Name  as Trasport_Name  , f.* from Pavu_Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.ReceivedFrom_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  where a.Pavu_Sales_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* , d.EndsCount_Name from Pavu_Sales_Details a LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_idno = d.EndsCount_idno where Pavu_Sales_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(i).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(i).Item("EndsCount_Name").ToString, 15))
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Set_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = Trim(prn_DetDt.Rows(i).Item("Beam_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 4) = Val(prn_DetDt.Rows(i).Item("Pcs").ToString)
                            prn_DetAr(prn_DetMxIndx, 5) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
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

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
        Printing_Format1(e)
        'End If
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
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False

        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    'Debug.Print(ps.PaperName)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

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

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 18

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 130 : ClArr(2) = 120 : ClArr(3) = 120 : ClArr(4) = 120
        ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_NoofBmDets < prn_DetMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetIndx = prn_DetIndx + 1

                        CurY = CurY + TxtHgt

                        If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 4))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 5)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)


                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        'If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                        '    prn_NoofBmDets = prn_NoofBmDets + 1

                        'End If

                        NoofDets = NoofDets + 1

                    Loop

                End If




                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin


        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        CurY = TMargin
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

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("INVOICE DATE  :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Invoice.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Sales_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Sales_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No ", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))
        'e.Graphics.DrawLine(Pens.Black, LMargin + M1 + 4, LnAr(3), LMargin + M1 + 4, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        '  Dim m1 As Integer
        Dim BmsInWrds As String
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

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then


                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    End If

                    'Else

                    '    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                    '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    '    End If
                    '    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                    '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    '    End If
                    '    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                    '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    '    End If

                End If

            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

            CurY = CurY + TxtHgt + 10

            Common_Procedures.Print_To_PrintDocument(e, "Rate/Meters ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Rate_Meters").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)


            If Val(prn_HdDt.Rows(0).Item("Add_Less").ToString) <> 0 Then
                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)
                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, "Gross Amount ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Add/Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Add_Less").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                CurY = CurY + TxtHgt + 25
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)



            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")
            Common_Procedures.Print_To_PrintDocument(e, "Rupees   : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'LnAr(7) = CurY
            'Common_Procedures.Print_To_PrintDocument(e, "Received The Beams As Per Above Details.", LMargin + 10, CurY + 5, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)


            CurY = CurY + TxtHgt + 30
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Led_IdNo As Integer
        Dim NewCode As String

        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)
        If Led_IdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            chk_SelectAll.Checked = False

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.noof_used as Ent_NoofUsed, b.*, c.EndsCount_Name, d.Beam_Width_Name from Pavu_Sales_Details a INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON b.Beam_Width_Idno = d.Beam_Width_Idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Pavu_Sales_Code = '" & Trim(NewCode) & "' and a.ReceivedFrom_IdNo = " & Str(Val(Led_IdNo)) & " order by a.for_orderby, a.Set_Code, b.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
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
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Meters_Pc").ToString)
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Ent_NoofUsed").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Pavu_Delivery_Increment").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        If Val(.Rows(n).Cells(9).Value) <> Val(.Rows(n).Cells(11).Value) Then
                            .Rows(i).Cells(j).Style.BackColor = Color.LightGray
                        End If
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name, c.Beam_Width_Name from Stock_SizedPavu_Processing_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head c ON a.Beam_Width_Idno = c.Beam_Width_Idno where  a.Pavu_Delivery_Code = '' and a.Beam_Knotting_Code = '' and a.Close_Status = 0 and a.StockAt_IdNo = " & Str(Val(Led_IdNo)) & " order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
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
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Meters_Pc").ToString)
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = "-9999"
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Pavu_Delivery_Increment").ToString

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

                If Val(.Rows(RwIndx).Cells(9).Value) > 0 Then
                    If Val(.Rows(RwIndx).Cells(9).Value) <> Val(.Rows(RwIndx).Cells(11).Value) Then
                        MessageBox.Show("Cannot deselect" & Chr(13) & "Already this pavu delivered to others")
                        Exit Sub
                    End If
                End If

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

        With dgv_Selection

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If .CurrentCell.RowIndex >= 0 Then
                    Select_Pavu(.CurrentCell.RowIndex)
                    e.Handled = True
                End If
            End If

            If e.KeyCode = Keys.Back Or e.KeyCode = Keys.Delete Then
                If .CurrentCell.RowIndex >= 0 Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(8).Value) = 1 Then
                        Select_Pavu(.CurrentCell.RowIndex)
                        e.Handled = True
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim n As Integer
        Dim sno As Integer

        With dgv_PavuDetails

            .Rows.Clear()

            For i = 0 To dgv_Selection.RowCount - 1

                If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(0).Value = Val(sno)
                    .Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                    .Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                    .Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                    .Rows(n).Cells(4).Value = Format(Val(dgv_Selection.Rows(i).Cells(4).Value), "#########0.00")
                    .Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(5).Value
                    .Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
                    .Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(7).Value

                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = ""

                    If Val(dgv_Selection.Rows(i).Cells(9).Value) > 0 Then

                        If Val(dgv_Selection.Rows(i).Cells(9).Value) <> Val(dgv_Selection.Rows(i).Cells(11).Value) Then
                            .Rows(n).Cells(8).Value = "1"
                        Else
                            .Rows(n).Cells(8).Value = ""
                        End If

                        .Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(9).Value

                    End If

                    .Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(10).Value
                    .Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(11).Value

                End If

            Next

        End With

        TotalPavu_Calculation()

        Grid_Cell_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_SalesAc.Enabled And cbo_SalesAc.Visible Then cbo_SalesAc.Focus()

    End Sub
    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_PavuDetails_KeyUp(sender, e)
        End If
    End Sub

    Private Sub txt_SetNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SetNoSelection.KeyDown
        If (e.KeyValue = 40) Then
            txt_BeamNoSelection.Focus()
        End If
    End Sub

    Private Sub txt_SetNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SetNoSelection.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_BeamNoSelection.Focus()
        End If
    End Sub

    Private Sub txt_BeamNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BeamNoSelection.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
        If (e.KeyValue = 38) Then txt_SetNoSelection.Focus()
    End Sub

    Private Sub txt_BeamNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BeamNoSelection.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_BeamNoSelection.Text) <> "" Or Trim(txt_SetNoSelection.Text) <> "" Then
                btn_Set_Bm_selection_Click(sender, e)

            Else
                If dgv_Selection.Rows.Count > 0 Then
                    dgv_Selection.Focus()
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                    dgv_Selection.CurrentCell.Selected = True
                End If

            End If

        End If
    End Sub

    Private Sub btn_Set_Bm_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Set_Bm_selection.Click
        Dim LtNo As String
        Dim PcsNo As String
        Dim i As Integer

        If Trim(txt_SetNoSelection.Text) <> "" Or Trim(txt_BeamNoSelection.Text) <> "" Then

            LtNo = Trim(txt_SetNoSelection.Text)
            PcsNo = Trim(txt_BeamNoSelection.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) And Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                    Call Select_Pavu(i)

                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 11 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 10

                    Exit For

                End If
            Next

            txt_SetNoSelection.Text = ""
            txt_BeamNoSelection.Text = ""
            If txt_SetNoSelection.Enabled = True Then txt_SetNoSelection.Focus()

        End If
    End Sub

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(8).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
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


    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
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

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

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

    Private Sub txt_RateMeters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RateMeters.KeyDown
        If e.KeyCode = 38 Then
            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                dgv_PavuDetails.CurrentCell.Selected = True
            Else
                txt_Freight.Focus()
            End If
        End If

        If e.KeyCode = 40 Then txt_AddLess.Focus()
    End Sub

    Private Sub txt_RateMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RateMeters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_RateMeters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_RateMeters.TextChanged
        '  lbl_GrossAmount.Text = Format(Val(lbl_GrossAmount.Text) + Val(txt_AddLess.Text), "########0.00")
        lbl_NetAmount.Text = Format(Val(lbl_GrossAmount.Text) + Val(txt_AddLess.Text), "########0.00")

        TotalPavu_Calculation()
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If


    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        lbl_NetAmount.Text = Format(Val(lbl_GrossAmount.Text) + Val(txt_AddLess.Text), "########0.00")
    End Sub


    Private Sub txt_KuraiPavuMeter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_KuraiPavuMeter.TextChanged
        TotalPavu_Calculation()
    End Sub


End Class