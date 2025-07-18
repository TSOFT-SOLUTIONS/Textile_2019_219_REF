Public Class JobWork_Cloth_Delivery
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "JCLDC-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(100, 50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_HdAr(100, 10) As String

    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_TotCopies As Integer = 0
    Private prn_DetAr1(200, 10) As String
    Private prn_DetDt1 As New DataTable
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1


    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Print.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_Transport.Text = ""
        cbo_DespTo.Text = ""
        cbo_Grid_ClothName.Text = ""
        cbo_Grid_Clothtype.Text = ""

        txt_VehicleNo.Text = ""
        txt_DelvAdd1.Text = ""
        txt_DelvAdd2.Text = ""
        txt_JJFormNo.Text = ""

        txt_Note.Text = ""


        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_DespTo.Enabled = True
        cbo_DespTo.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        txt_DelvAdd1.Enabled = True
        txt_DelvAdd1.BackColor = Color.White

        txt_DelvAdd2.Enabled = True
        txt_DelvAdd2.BackColor = Color.White

        cbo_Grid_ClothName.Enabled = True
        cbo_Grid_ClothName.BackColor = Color.White

        cbo_Grid_Clothtype.Enabled = True
        cbo_Grid_Clothtype.BackColor = Color.White

        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_ClothName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Grid_ClothName.Visible = False
        cbo_Grid_Clothtype.Visible = False

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Or TypeOf Me.ActiveControl Is Button Then
            Me.ActiveControl.BackColor = Color.SpringGreen
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

        If Me.ActiveControl.Name <> cbo_Grid_ClothName.Name Then
            cbo_Grid_ClothName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Clothtype.Name Then
            cbo_Grid_Clothtype.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
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
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub JobWork_Cloth_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Clothtype.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Clothtype.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub JobWork_Cloth_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub JobWork_Cloth_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try

            If Asc(e.KeyChar) = 27 Then

                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                End If

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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

    Private Sub JobWork_Cloth_Delivery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where ( Ledger_IdNo = 0 or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_PartyName.DataSource = dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Transport.DataSource = dt2
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt4)
        cbo_Grid_ClothName.DataSource = dt4
        cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
        da.Fill(dt5)
        cbo_Grid_Clothtype.DataSource = dt5
        cbo_Grid_Clothtype.DisplayMember = "ClothType_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Despatched_To) from JobWork_Piece_Delivery_Head order by Despatched_To", con)
        da.Fill(dt6)
        cbo_DespTo.DataSource = dt6
        cbo_DespTo.DisplayMember = "Despatched_To"


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DespTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Clothtype.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Delivery.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_FormJJ.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_JJFormNo.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DespTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Clothtype.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Delivery.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_FormJJ.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_JJFormNo.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_VehicleNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DelvAdd1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_JJFormNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_VehicleNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DelvAdd1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_JJFormNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= 7 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                cbo_Transport.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            If .CurrentCell.ColumnIndex = 3 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 3)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_DelvAdd2.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)

                            End If

                        Else

                            If .CurrentCell.ColumnIndex = 6 Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 3)
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

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from JobWork_Piece_Delivery_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_DcNo.Text = dt1.Rows(0).Item("JobWork_Piece_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString
                msk_date.Text = dtp_Date.Text

                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))

                cbo_DespTo.Text = dt1.Rows(0).Item("Despatched_To").ToString

                txt_DelvAdd1.Text = dt1.Rows(0).Item("Delivery_Address1").ToString
                txt_DelvAdd2.Text = dt1.Rows(0).Item("Delivery_Address2").ToString
                txt_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_Note.Text = dt1.Rows(0).Item("Remarks").ToString
                txt_JJFormNo.Text = dt1.Rows(0).Item("JJ_FormNo").ToString


                LockSTS = False


                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Cloth_Name, c.ClothType_Name from JobWork_Piece_Delivery_Details a LEFT OUTER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_IdNo Where a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Folding").ToString)
                            If Val(.Rows(n).Cells(3).Value) = 0 Then
                                .Rows(n).Cells(3).Value = 100
                            End If
                            .Rows(n).Cells(4).Value = "" ' Val(dt2.Rows(i).Item("Bales").ToString)
                            .Rows(n).Cells(5).Value = "" ' dt2.Rows(i).Item("Bales_Nos").ToString
                            .Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Pcs").ToString)

                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            .Rows(n).Cells(8).Value = "" ' dt2.Rows(i).Item("ClothSales_Order_Code").ToString
                            .Rows(n).Cells(9).Value = "" ' dt2.Rows(i).Item("ClothSales_Order_SlNo").ToString
                            .Rows(n).Cells(10).Value = "" ' dt2.Rows(i).Item("ClothSales_Delivery_SlNo").ToString
                            .Rows(n).Cells(11).Value = "" ' dt2.Rows(i).Item("Invoice_Meters").ToString
                            .Rows(n).Cells(12).Value = "" ' dt2.Rows(i).Item("PackingSlip_Codes").ToString

                            If Val(.Rows(n).Cells(11).Value) <> 0 Then
                                For j = 0 To .ColumnCount - 1
                                    .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If


                        Next i

                    End If

                    If .Rows.Count = 0 Then
                        .Rows.Add()

                    End If

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Rolls").ToString)
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Delivery_Meters").ToString), "########0.00")
                End With


                If IsDBNull(dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If
                If IsDBNull(dt1.Rows(0).Item("JobWork_Bill_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("JobWork_Bill_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If
                If IsDBNull(dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

            End If

            Grid_Cell_DeSelect()

            If LockSTS = True Then
                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray


                cbo_DespTo.Enabled = False
                cbo_DespTo.BackColor = Color.LightGray


                cbo_Transport.Enabled = False
                cbo_Transport.BackColor = Color.LightGray


                txt_DelvAdd1.Enabled = False
                txt_DelvAdd1.BackColor = Color.LightGray

                txt_DelvAdd2.Enabled = False
                txt_DelvAdd2.BackColor = Color.LightGray


                cbo_Grid_ClothName.Enabled = False
                cbo_Grid_ClothName.BackColor = Color.LightGray

                cbo_Grid_Clothtype.Enabled = False
                cbo_Grid_Clothtype.BackColor = Color.LightGray

            End If



        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Jobwork_Piece_Delivery_Entry, New_Entry, Me, con, "JobWork_Piece_Delivery_Head", "JobWork_Piece_Delivery_Code", NewCode, "JobWork_Piece_Delivery_Date", "(JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub







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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from JobWork_Piece_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) <> "" Then
                    MessageBox.Show("Already inspection Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
            If IsDBNull(Dt1.Rows(0).Item("JobWork_Bill_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("JobWork_Bill_Code").ToString) <> "" Then
                    MessageBox.Show("Already Bil prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
            If IsDBNull(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                    MessageBox.Show("Already Piece Delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()
        Dt1.Dispose()
        Da.Dispose()

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_Piece_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt2)
            cbo_Filter_ClothName.DataSource = dt2
            cbo_Filter_ClothName.DisplayMember = "Cloth_Name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_ClothName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Piece_Delivery_No from JobWork_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_Piece_Delivery_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Piece_Delivery_No from JobWork_Piece_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_Piece_Delivery_No", con)
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

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Piece_Delivery_No from JobWork_Piece_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Piece_Delivery_No desc", con)
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

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_Piece_Delivery_No from JobWork_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Piece_Delivery_No desc", con)
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

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_Piece_Delivery_Head", "JobWork_Piece_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_DcNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString


            ' dtp_date.Text = Date.Today.ToShortDateString
            Da1 = New SqlClient.SqlDataAdapter("select top 1 * from JobWork_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Piece_Delivery_No desc", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                    If Dt1.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString <> "" Then msk_date.Text = Dt1.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString
                End If
            End If
            Dt1.Clear()
            If msk_date.Enabled And msk_date.Visible Then
                msk_date.Focus()
                msk_date.SelectionStart = 0
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Dc No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select JobWork_Piece_Delivery_No from JobWork_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(InvCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to DELETE", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Jobwork_Piece_Delivery_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW Dc NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select JobWork_Piece_Delivery_No from JobWork_Piece_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(InvCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Dc No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT ...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim clth_ID As Integer = 0
        Dim FP_ID As Integer = 0
        Dim clthtyp_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim Led_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vTotBals As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim Nr As Integer = 0
        Dim OrdCd As String = ""
        Dim OrdSlNo As Long = 0
        Dim DcCd As String = ""
        Dim DcSlNo As Long = 0
        Dim OpYrCode As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, New_Entry) = False Then Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Jobwork_Piece_Delivery_Entry, New_Entry, Me, con, "JobWork_Piece_Delivery_Head", "JobWork_Piece_Delivery_Code", NewCode, "JobWork_Piece_Delivery_Date", "(JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, JobWork_Piece_Delivery_No desc", dtp_Date.Value.Date) = False Then Exit Sub


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


        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)


        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then

                clth_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                If clth_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                    End If
                    Exit Sub
                End If

                clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(i).Cells(2).Value)
                If clthtyp_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                    End If
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(3).Value) = 0 Then
                    MessageBox.Show("Invalid Folding", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                    End If
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(7).Value) = 0 Then
                    MessageBox.Show("Invalid metres", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(7)
                    End If
                    Exit Sub
                End If

            End If

        Next

        NoCalc_Status = False
        Total_Calculation()

        vTotPcs = 0 : vTotMtrs = 0 : vTotBals = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBals = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If

        If vTotMtrs = 0 Then
            MessageBox.Show("Invalid METERS", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dgv_Details.Enabled And dgv_Details.Visible Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
            End If
            Exit Sub
        End If

        If Trim(txt_JJFormNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Da = New SqlClient.SqlDataAdapter("select * from JobWork_Piece_Delivery_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code <> '" & Trim(NewCode) & "' and JobWork_Piece_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and JJ_FormNo = '" & Trim(txt_JJFormNo.Text) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate JJ Form No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_JJFormNo.Enabled And txt_JJFormNo.Visible Then txt_JJFormNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If

        Clo_ID = 0
        With dgv_Details
            If .Rows.Count > 0 Then
                Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(0).Cells(1).Value)
            End If
        End With

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_Piece_Delivery_Head", "JobWork_Piece_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into JobWork_Piece_Delivery_Head ( JobWork_Piece_Delivery_Code,                 Company_IdNo     ,    JobWork_Piece_Delivery_No ,                               for_OrderBy                             , JobWork_Piece_Delivery_Date,           Ledger_IdNo   ,           Cloth_Idno     ,             Despatched_To      ,          Delivery_Address1        ,           Delivery_Address2      ,       Transport_IdNo       ,               Vehicle_No          ,               Remarks        ,          Total_Rolls     ,   Total_Delivery_Meters   ,                               JJ_Form_OrderByNo                           ,               JJ_FormNo           ) " & _
                                    "          Values                      (   '" & Trim(NewCode) & "'  , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",       @EntryDate     , " & Str(Val(Led_ID)) & ",  " & Str(Val(Clo_ID)) & ", '" & Trim(cbo_DespTo.Text) & "',  '" & Trim(txt_DelvAdd1.Text) & "', '" & Trim(txt_DelvAdd2.Text) & "',  " & Str(Val(Trans_ID)) & ", '" & Trim(txt_VehicleNo.Text) & "', '" & Trim(txt_Note.Text) & "', " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & ", " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_JJFormNo.Text))) & ", '" & Trim(txt_JJFormNo.Text) & "' ) "
                cmd.ExecuteNonQuery()

            Else

                Da = New SqlClient.SqlDataAdapter("select * from JobWork_Piece_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) = False Then
                        If Trim(Dt1.Rows(0).Item("JobWork_Inspection_Code").ToString) <> "" Then
                            Throw New ApplicationException("Already inspection Prepared")
                            Exit Sub
                        End If
                    End If
                    If IsDBNull(Dt1.Rows(0).Item("JobWork_Bill_Code").ToString) = False Then
                        If Trim(Dt1.Rows(0).Item("JobWork_Bill_Code").ToString) <> "" Then
                            Throw New ApplicationException("Already Bil prepared")
                            Exit Sub
                        End If
                    End If
                    If IsDBNull(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                        If Trim(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                            Throw New ApplicationException("Already GatePass Prepared")
                            Exit Sub
                        End If
                    End If
                End If
                Dt1.Clear()

                cmd.CommandText = "Update JobWork_Piece_Delivery_Head set JobWork_Piece_Delivery_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Cloth_Idno = " & Str(Val(Clo_ID)) & ", Despatched_To = '" & Trim(cbo_DespTo.Text) & "', Transport_IdNo = " & Str(Val(Trans_ID)) & ", Delivery_Address1 = '" & Trim(txt_DelvAdd1.Text) & "', Delivery_Address2 = '" & Trim(txt_DelvAdd2.Text) & "', Vehicle_No = '" & Trim(txt_VehicleNo.Text) & "', Remarks = '" & Trim(txt_Note.Text) & "', Total_Rolls = " & Str(Val(vTotPcs)) & ", Total_Delivery_Meters = " & Str(Val(vTotMtrs)) & ", JJ_Form_OrderByNo = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_JJFormNo.Text))) & ", JJ_FormNo = '" & Trim(txt_JJFormNo.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)
            Partcls = "JbWrk Clo.Delv : Dc.No. " & Trim(lbl_DcNo.Text)

            cmd.CommandText = "Delete from JobWork_Piece_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        cmd.CommandText = "Insert into JobWork_Piece_Delivery_Details ( JobWork_Piece_Delivery_Code,                 Company_IdNo     ,    JobWork_Piece_Delivery_No ,                               for_OrderBy                             , JobWork_Piece_Delivery_Date,        Ledger_IdNo      ,           Sl_No      ,              Lot_No          , Pcs_No,             Cloth_IdNo   ,            ClothType_IdNo    ,                       Folding             ,                   Pcs                 ,                      Meters              ,          Lot_Code      ,      Entry_PkCondition       ) " & _
                                            "          Values                         (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",         @EntryDate      , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(lbl_DcNo.Text) & "',   '1' , " & Str(Val(clth_ID)) & ",  " & Str(Val(clthtyp_ID)) & ",  " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", '" & Trim(NewCode) & "', '" & Trim(Pk_Condition) & "' ) "
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                             , Reference_Date,         StockOff_IdNo   ,     DeliveryTo_Idno     ,                            ReceivedFrom_Idno              ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No      ,           Cloth_Idno     ,                      Folding             ,   Meters_Type" & Trim(Val(clthtyp_ID)) & ") " & _
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",  @EntryDate, " & Str(Val(Led_ID)) & ", " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(clth_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With






            tr.Commit()

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

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotPcs As Single
        Dim TotBals As Single
        Dim TotMtrs As Single

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotPcs = 0 : TotBals = 0 : TotMtrs = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(7).Value) <> 0 Then

                    TotBals = TotBals + Val(.Rows(i).Cells(4).Value())
                    TotPcs = TotPcs + Val(.Rows(i).Cells(6).Value())
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(7).Value())

                End If

            Next i

        End With


        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBals)
            .Rows(0).Cells(6).Value = Val(TotPcs)
            .Rows(0).Cells(7).Value = Format(Val(TotMtrs), "########0.00")
        End With

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or  Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, dtp_Date, cbo_DespTo, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_DespTo, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_DespTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DespTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "JobWork_Piece_Delivery_Head", "Despatched_To", "", "")
    End Sub

    Private Sub cbo_DespTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DespTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DespTo, cbo_PartyName, txt_DelvAdd1, "JobWork_Piece_Delivery_Head", "Despatched_To", "", "")
    End Sub

    Private Sub cbo_DespTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DespTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DespTo, txt_DelvAdd1, "JobWork_Piece_Delivery_Head", "Despatched_To", "", "", False)
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, Nothing, txt_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True

            Else
                txt_DelvAdd2.Focus()

            End If
        End If

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
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

    Private Sub cbo_Grid_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_ClothName.Text)

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown

        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, txt_DelvAdd2, cbo_Grid_Clothtype, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                txt_DelvAdd2.Focus()
            End If

            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    cbo_Transport.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, cbo_Grid_Clothtype, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    cbo_Transport.Focus()
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Clothtype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_Clothtype.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_Clothtype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Clothtype.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Clothtype, Nothing, Nothing, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Clothtype.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Clothtype.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Clothtype_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Clothtype.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Clothtype, Nothing, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub


    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Val(.Rows(e.RowIndex).Cells(10).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(10).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(10).Value = Val(.Rows(e.RowIndex - 1).Cells(10).Value) + 1
                End If
            End If

            If Trim(.CurrentRow.Cells(2).Value) = "" Then
                .CurrentRow.Cells(2).Value = Common_Procedures.ClothType_IdNoToName(con, 1)
            End If

            If Val(.CurrentRow.Cells(3).Value) = 0 Then
                .CurrentRow.Cells(3).Value = "100"
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + rect.Left
                    cbo_Grid_ClothName.Top = .Top + rect.Top

                    cbo_Grid_ClothName.Width = rect.Width
                    cbo_Grid_ClothName.Height = rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()

                End If

            Else
                cbo_Grid_ClothName.Visible = False

            End If

            If e.ColumnIndex = 2 Then

                If cbo_Grid_Clothtype.Visible = False Or Val(cbo_Grid_Clothtype.Tag) <> e.RowIndex Then

                    cbo_Grid_Clothtype.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Clothtype.DataSource = Dt1
                    cbo_Grid_Clothtype.DisplayMember = "ClothType_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Clothtype.Left = .Left + rect.Left
                    cbo_Grid_Clothtype.Top = .Top + rect.Top

                    cbo_Grid_Clothtype.Width = rect.Width
                    cbo_Grid_Clothtype.Height = rect.Height
                    cbo_Grid_Clothtype.Text = .CurrentCell.Value

                    cbo_Grid_Clothtype.Tag = Val(e.RowIndex)
                    cbo_Grid_Clothtype.Visible = True

                    cbo_Grid_Clothtype.BringToFront()
                    cbo_Grid_Clothtype.Focus()

                End If

            Else

                cbo_Grid_Clothtype.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                        Total_Calculation()
                    End If
                End If
            End With

        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        Try
            dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_Details.SelectAll()
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) <> 0 Or Trim(.Rows(.CurrentCell.RowIndex).Cells(12).Value) <> "" Then
                        e.Handled = True
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        On Error Resume Next
        Dim i As Integer
        Dim n As Integer = -1

        With dgv_Details
            If .Visible Then

                If Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) <> 0 Or Trim(.Rows(.CurrentCell.RowIndex).Cells(12).Value) <> "" Then
                    e.Handled = True

                    'Add_NewRow_ToGrid()

                End If

                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                dgv_Details_KeyUp(sender, e)
            End If

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details

                    If Val(.Rows(.CurrentCell.RowIndex).Cells(11).Value) = 0 Then

                        n = .CurrentRow.Index

                        If n = .Rows.Count - 1 Then
                            For i = 0 To .ColumnCount - 1
                                .Rows(n).Cells(i).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(n)

                        End If

                        Total_Calculation()

                    End If

                End With

            End If

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)

                If Val(.Rows(e.RowIndex).Cells(10).Value) = 0 Then
                    If e.RowIndex = 0 Then
                        .Rows(e.RowIndex).Cells(10).Value = 1
                    Else
                        .Rows(e.RowIndex).Cells(10).Value = Val(.Rows(e.RowIndex - 1).Cells(10).Value) + 1
                    End If
                End If

            End With

        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub txt_DelvAdd2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DelvAdd2.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                cbo_Transport.Focus()

            End If
        End If
    End Sub

    Private Sub txt_DelvAdd2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DelvAdd2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                cbo_Transport.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then
            txt_VehicleNo.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Grid_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try
            If cbo_Grid_ClothName.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                With dgv_Details
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Clothtype_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.TextChanged
        Try
            If cbo_Grid_Clothtype.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                With dgv_Details
                    If Val(cbo_Grid_Clothtype.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Clothtype.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clth_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clth_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobWork_Piece_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.JobWork_Piece_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobWork_Piece_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Clth_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Val(Clth_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Cloth_IdNo = " & Str(Val(Clth_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.ClothType_name, e.Ledger_Name from JobWork_Piece_Delivery_Head a left outer join JobWork_Piece_Delivery_Details b on a.JobWork_Piece_Delivery_Code = b.JobWork_Piece_Delivery_Code left outer join Cloth_head c on b.Cloth_idno = c.Cloth_idno left outer join ClothType_head d on b.ClothType_idno = d.ClothType_idno left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.JobWork_Piece_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by JobWork_Piece_Delivery_Date, for_orderby, JobWork_Piece_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("JobWork_Piece_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Party_OrderNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Delivery_Meters").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String = ""

        Try
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_Back.Enabled = True
                pnl_Filter.Visible = False
            End If

        Catch ex As Exception
            '------

        End Try

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or  Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        pnl_Print.Visible = True
        Pnl_Back.Enabled = False
        If btn_Print_Delivery.Enabled And btn_Print_Delivery.Visible Then
            btn_Print_Delivery.Focus()
        End If
    End Sub

    Private Sub btn_Print_Delivery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Delivery.Click
        Printing_Delivery()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Public Sub Printing_Delivery()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Jobwork_Piece_Delivery_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from JobWork_Piece_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName  from JobWork_Piece_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from JobWork_Piece_Delivery_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_Delivery_Format1(e)

    End Sub

    Private Sub Printing_Delivery_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
            .Left = 40
            .Right = 40
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

        NoofItems_PerPage = 4

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 40 : ClAr(2) = 375 : ClAr(3) = 135
        ClAr(4) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3))

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Delivery_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Delivery_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1, w2 As Single
        Dim S1, s2 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from JobWork_Piece_Delivery_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Piece_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "FABRIC DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10

        'If prn_HdDt.Rows(0).Item("Party_OrderNo").ToString <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        'End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT  ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DESP.TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatched_To").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("vehicle_No").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "AGENT ", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)


        'Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Delivery_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        CurY = CurY + 10

        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""

        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 1
        prn_DetIndx = 0
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_Count = 1
        Erase prn_DetAr
        Erase prn_HdAr

        prn_HdAr = New String(100, 10) {}

        prn_DetAr = New String(100, 50, 10) {}

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, tZ.*, c.Cloth_Name from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                For i = 0 To prn_HdDt.Rows.Count - 1

                    prn_HdMxIndx = prn_HdMxIndx + 1

                    prn_HdAr(prn_HdMxIndx, 1) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                    prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)
                    prn_HdAr(prn_HdMxIndx, 3) = Val(prn_HdDt.Rows(i).Item("Total_Rolls").ToString)
                    prn_HdAr(prn_HdMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Total_Delivery_Meters").ToString), "#########0.00")

                    prn_DetMxIndx = 0

                    da2 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                    If prn_DetDt.Rows.Count > 0 Then
                        For j = 0 To prn_DetDt.Rows.Count - 1
                            If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
                                prn_DetMxIndx = prn_DetMxIndx + 1

                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

                            End If
                        Next j
                    End If

                Next i

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Common_Procedures.Printing_PackingSlip_Format1(PrintDocument2, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)
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

    Private Sub btn_Print_FormJJ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_FormJJ.Click
        Print_FormJJ()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub Print_FormJJ()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from JobWork_Piece_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
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

        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR FORMJJ PRINTING...", "4")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")

        For I = 0 To PrintDocument3.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument3.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument3.PrinterSettings.PaperSizes(I)
                PrintDocument3.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument3.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument3.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument3.Print()
                    End If

                Else
                    PrintDocument3.Print()

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument3


                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument2.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If
    End Sub

    Private Sub PrintDocument3_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument3.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim i As Integer, k As Integer
        Dim W1 As Single = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetMxIndx = 0
        prn_NoofBmDets = 0
        prn_Count = 0

        Erase prn_HdAr
        Erase prn_DetAr

        prn_HdAr = New String(200, 10) {}
        prn_DetAr1 = New String(200, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_TinNo , c.Ledger_CstNo, c.Ledger_Name , c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, d.Ledger_Name as Transport_Name, e.Area_Name from JobWork_Piece_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_Idno = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_Idno = d.Ledger_IdNo  LEFT OUTER JOIN Area_Head e ON b.Area_Idno = e.Area_Idno   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.ClothType_Name, c.Cloth_Name from JobWork_Piece_Delivery_Details a LEFT OUTER JOIN ClothType_Head b on a.ClothType_IdNo = b.ClothType_IdNo INNER JOIN Cloth_Head c on a.Cloth_IdNo = c.Cloth_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                da1 = New SqlClient.SqlDataAdapter("select top 1  c.Cloth_Name, d.ClothType_Name, b.Total_Bales, b.Total_Rolls, b.Total_Delivery_Meters from JobWork_Piece_Delivery_Details a INNER JOIN JobWork_Piece_Delivery_Head b ON a.JobWork_Piece_Delivery_Code = b.JobWork_Piece_Delivery_Code INNER JOIN Cloth_Head c on a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN ClothType_Head d on a.ClothType_IdNo = d.ClothType_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt1 = New DataTable
                da1.Fill(prn_DetDt1)

                k = 0
                If prn_DetDt1.Rows.Count > 0 Then

                    For i = 0 To prn_DetDt1.Rows.Count - 1

                        If Val(prn_DetDt1.Rows(i).Item("Total_Delivery_Meters").ToString) <> 0 Then
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Trim(prn_DetDt1.Rows(i).Item("Cloth_Name").ToString)
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Trim(prn_DetDt1.Rows(i).Item("ClothType_Name").ToString)
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Val(prn_DetDt1.Rows(i).Item("Total_Bales").ToString)
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Val(prn_DetDt1.Rows(i).Item("Total_Rolls").ToString)
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Format(Val(prn_DetDt1.Rows(i).Item("Total_Delivery_Meters").ToString), "#########0.000")

                        End If

                    Next i

                End If
                Dt1.Clear()

                If k > prn_DetMxIndx Then prn_DetMxIndx = k

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument3_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument3.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_FormJJ(e)
    End Sub

    Private Sub Printing_FormJJ(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim flperc As Single = 0
        Dim flmtr As Single = 0
        Dim fmtr As Single = 0
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String


        For I = 0 To PrintDocument3.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument3.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument3.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument2.DefaultPageSettings.Margins
            .Left = 20
            .Right = 65
            .Top = 50 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        pFont = New Font("Calibri", 10, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument3.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If PrintDocument3.DefaultPageSettings.Landscape = True Then
            With PrintDocument3.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 5 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 300 : ClArr(3) = 100 : ClArr(4) = 95 : ClArr(5) = 110 : ClArr(6) = 80
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        ''=========================================================================================================
        ''------  START OF PREPRINT POINTS
        ''=========================================================================================================

        'pFont = New Font("Calibri", 11, FontStyle.Regular)

        'Dim CurX As Single = 0
        'Dim pFont1 As Font

        'pFont1 = New Font("Calibri", 8, FontStyle.Regular)

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

        'Exit Sub

        ''=========================================================================================================
        ''------  END OF PREPRINT POINTS
        ''=========================================================================================================

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_FormJJ_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10
                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_FormJJ_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 30 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 30
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt + 5

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("ClothType_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)



                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_FormJJ_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                If Trim(prn_InpOpts) <> "" Then
                    If prn_Count < Len(Trim(prn_InpOpts)) Then


                        If Val(prn_InpOpts) <> "0" Then
                            prn_DetIndx = 0
                            prn_DetSNo = 0
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If

                    End If
                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_FormJJ_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Double = 0
        Dim C1 As Single, W1, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim S As String = ""
        Dim goods_value As Double = 0
        Dim Fab_Value As Double = 0
        Dim pavu_value As Double = 0
        Dim NewCode As String = ""
        Dim To_Add As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.* from JobWork_Piece_Delivery_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo  where a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        Fab_Value = 0
        If dt2.Rows.Count > 0 Then
            For i = 0 To dt2.Rows.Count - 1
                If Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 1 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Sound_Rate").ToString), "#######0.00")
                ElseIf Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 2 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Seconds_Rate").ToString), "#######0.00")
                ElseIf Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 3 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Bits_Rate").ToString), "#######0.00")
                ElseIf Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 4 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Other_Rate").ToString), "#######0.00")
                ElseIf Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 5 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Reject_Rate").ToString), "#######0.00")
                End If
            Next i
        End If
        dt2.Clear()

        'da2 = New SqlClient.SqlDataAdapter("select Sum(a.Meters * b.Sound_Rate) as Value_Of_Sound ,sum(a.Meters * b.Seconds_Rate) as Value_Of_Sound, Sum(a.Meters * b.Bits_Rate) as Value_Of_Bits,Sum(a.Meters * b.Other_Rate) as Value_Of_Other,Sum(a.Meters * b.Reject_Rate) as Value_Of_Reject from JobWork_Piece_Delivery_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo  where a.JobWork_Piece_Delivery_Code = '" & Trim(NewCode) & "'", con)
        'dt2 = New DataTable
        'da2.Fill(dt2)

        'Fab_Value = 0
        'If dt2.Rows.Count > 0 Then
        '    If Val(prn_DetDt.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) <> 0 Then
        '        Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Sound").ToString), "#######0.00")
        '    ElseIf Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Second").ToString), "#######0.00") Then
        '    ElseIf Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Bits").ToString), "#######0.00") Then
        '    ElseIf Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Others").ToString), "#######0.00") Then
        '    ElseIf Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Rejects").ToString), "#######0.00") Then
        '    End If
        'End If
        'dt2.Clear()

        goods_value = Format(Val(Fab_Value), "#########0.00")

        dt2.Clear()
        dt3.Clear()

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 2 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If
            End If
        End If

        p1Font = New Font("Calibri", 20, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FORM JJ", LMargin + 10, CurY - TxtHgt - 10, 0, 0, p1Font)

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY Remarks", LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "[See rule 15(3), 15(18), 15(19), 15(20), 15(21)]", LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "(for sales / stock transfer / works contract / labour)", LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Asst Year : " & Trim(Common_Procedures.FnYearCode), LMargin, CurY, 2, PrintWidth, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "Asst Year : 15-16", LMargin, CurY, 2, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("JJ_FormNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "NO  :  " & prn_HdDt.Rows(0).Item("JJ_FormNo").ToString, PageWidth - 10, CurY, 1, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "NO  :  " & prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_No").ToString, PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        Try
            C1 = ClArr(1) + ClArr(2) + ClArr(3)

            W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
            S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "1.(a) Name and address of the", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + C1 + 10, CurY, 0, 0, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Consigner", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b).TIN", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c).CST Registration No", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "2.(a) Name and address of the", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "consignee / branch / agent", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b).TIN", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c).CST Registration No.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_CstNo").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "3 Address", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(i) from which goods are consigned.", LMargin + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Area_Name").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Area_Name").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)

            End If

            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                To_Add = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                To_Add = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                To_Add = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)

            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(ii) to which goods are consigned.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, To_Add, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "4.Description of goods consigned", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(a) Name of the goods", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Fabric", LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b) Quantity Or Weight", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString & " Mtrs", LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c) Value of the goods", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Rs. " & Common_Procedures.Currency_Format(Val(goods_value)), LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "5.Purpose of transport", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(a) for sale / purchase", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b) for shipment", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c) transfer to branch/head office", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "/Consignment agent", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(d) for executionof works contract", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(e) FOR LABOUR WORK / PROCESSING", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(e) for labour work / processing", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "6.To Whom delivered for transport", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "and vehicle no, if any", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "7.Remarks, if any", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClArr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CLOTHNAME", LMargin + ClArr(1), CurY, 2, ClArr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClArr(1) + ClArr(2), CurY, 2, ClArr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 2, ClArr(6), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_FormJJ_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim Cmp_Name As String

        Try
            W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClArr(1) + ClArr(2) + 30, CurY, 2, ClArr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Bales").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bales").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Delivery_Meters").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2), CurY, LMargin + ClArr(1) + ClArr(2), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(9))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(9))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(9))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "We certify that to the best of my/our knowledge the particulare are true, correct and complete.", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(12) = CurY

            CurY = CurY + TxtHgt + 5

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Signature :", LMargin + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            'Common_Procedures.Print_To_PrintDocument(e, "Signature :", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Name :", LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Name :", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(13) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Name and signature of the person to whom the goods were", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Name and signature of the consigner /", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "delivered for transporting with status of person signing", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "his employee / his representative", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Place : ", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 20, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Transfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Transfer.Click

    End Sub
End Class