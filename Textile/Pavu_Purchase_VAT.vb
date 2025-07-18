Public Class Pavu_Purchase_VAT
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private MovSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PAVPC-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_PavuDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1


    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        MovSTS = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        lbl_NewSTS.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        txt_PartyBillNo.Text = ""
        txt_SetNo.Text = ""
        cbo_Sizing.Text = ""
        cbo_Sizing.Tag = ""
        cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.Godown_Ac))
        cbo_DeliveryTo.Tag = ""
        cbo_PurchaseAc.Text = ""
        cbo_PurchaseAc.Tag = ""
        cbo_EndsCount.Text = ""
        txt_PcsLength.Text = ""
        txt_RateMeters.Text = ""
        txt_AddLess.Text = ""
        lbl_GrossAmount.Text = ""
        lbl_NetAmount.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_PavuDetails.Rows.Clear()
        dgv_PavuDetails.Rows.Add()

        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        cbo_Sizing.Enabled = True
        cbo_Sizing.BackColor = Color.White

        cbo_DeliveryTo.Enabled = True
        cbo_DeliveryTo.BackColor = Color.White

        txt_SetNo.Enabled = True
        txt_SetNo.BackColor = Color.White

        cbo_EndsCount.Enabled = True
        cbo_EndsCount.BackColor = Color.White

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_MillName.Text = ""
            txt_Filter_SetNo.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        dgv_ActiveCtrl_Name = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        End If


        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer = 0
        Dim I As Integer = 0, J As Integer = 0
        Dim SNo As Integer
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        MovSTS = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as SizingName, c.Ledger_Name as DeliveryTo_Name, d.EndsCount_Name from Pavu_Purchase_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  INNER JOIN Ledger_Head c ON a.DeliveryTo_IdNo = c.Ledger_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo Where a.Pavu_Purchase_Code = '" & Trim(NewCode) & "' AND a.Entry_VAT_GST_Type <> 'GST'", con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Pavu_Purchase_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Pavu_Purchase_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Sizing.Text = dt1.Rows(0).Item("SizingName").ToString
                cbo_DeliveryTo.Text = dt1.Rows(0).Item("DeliveryTo_Name").ToString
                txt_PartyBillNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                txt_SetNo.Text = dt1.Rows(0).Item("Set_No").ToString
                cbo_PurchaseAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("PurchaseAc_IdNo").ToString))
                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                txt_PcsLength.Text = dt1.Rows(0).Item("Pcs_Length").ToString
                txt_RateMeters.Text = Format(Val(dt1.Rows(0).Item("Rate_Meters").ToString), "########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("Add_Less").ToString), "########0.00")
                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                'If dt1.Rows(0).Item("Sizing_Specification_Code").ToString <> "" Then
                '    LockSTS = True
                'End If

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Pavu_Delivery_Code, b.Pavu_Delivery_Increment, b.Beam_Knotting_Code, b.Loom_Idno, b.Production_Meters, b.Close_Status  from Pavu_Purchase_Details a, Stock_SizedPavu_Processing_Details b where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Pavu_Purchase_Code = '" & Trim(NewCode) & "' and b.Reference_Code = '" & Trim(Pk_Condition) & "' + a.Pavu_Purchase_Code and a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_PavuDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        n = dgv_PavuDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_PavuDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_PavuDetails.Rows(n).Cells(1).Value = dt2.Rows(I).Item("Beam_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(2).Value = Val(dt2.Rows(I).Item("Pcs").ToString)
                        If Val(dgv_PavuDetails.Rows(n).Cells(2).Value) = 0 Then dgv_PavuDetails.Rows(n).Cells(2).Value = ""
                        dgv_PavuDetails.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(I).Item("Meters").ToString), "########0.00")

                        dgv_PavuDetails.Rows(n).Cells(4).Value = ""
                        If Trim(dt2.Rows(I).Item("Pavu_Delivery_Code").ToString) <> "" Or Val(dt2.Rows(I).Item("Pavu_Delivery_Increment").ToString) <> 0 Or Trim(dt2.Rows(I).Item("Beam_Knotting_Code").ToString) <> "" Or Val(dt2.Rows(I).Item("Production_Meters").ToString) <> 0 Or Val(dt2.Rows(I).Item("Close_Status").ToString) <> 0 Then
                            dgv_PavuDetails.Rows(n).Cells(4).Value = "1"
                            For J = 0 To dgv_PavuDetails.ColumnCount - 1
                                dgv_PavuDetails.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                                dgv_PavuDetails.Rows(n).Cells(J).Style.ForeColor = Color.Red
                            Next
                            LockSTS = True
                        End If

                    Next I

                End If
                dt2.Clear()

                TotalPavu_Calculation()

                'With dgv_PavuDetails_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Total_Beam").ToString)
                '    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                'End With

            End If

            dt1.Clear()


            If LockSTS = True Then
                cbo_Sizing.Enabled = False
                cbo_Sizing.BackColor = Color.LightGray

                cbo_DeliveryTo.Enabled = False
                cbo_DeliveryTo.BackColor = Color.LightGray

                txt_SetNo.Enabled = False
                txt_SetNo.BackColor = Color.LightGray

                cbo_EndsCount.Enabled = False
                cbo_EndsCount.BackColor = Color.LightGray
            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da1.Dispose()
            dt2.Dispose()
            da2.Dispose()

            MovSTS = False

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
    End Sub


    Private Sub Pavu_Purchase_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Sizing.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Sizing.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PurchaseAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SALES" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PurchaseAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Pavu_Purchase_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Pavu_Purchase_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Pavu_Purchase_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Text = ""

        con.Open()

        msk_Date.Text = ""
        dtp_Date.Text = ""
        txt_PartyBillNo.Text = ""
        txt_SetNo.Text = ""
        cbo_Sizing.Text = ""
        cbo_Sizing.Tag = ""
        cbo_DeliveryTo.Text = ""
        cbo_DeliveryTo.Tag = ""
        cbo_EndsCount.Text = ""

        cbo_EndsCount.Text = ""
        txt_RateMeters.Text = ""
        txt_AddLess.Text = ""
        cbo_PurchaseAc.Text = ""
        cbo_PurchaseAc.Tag = ""
        lbl_GrossAmount.Text = ""
        lbl_NetAmount.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PurchaseAc.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RateMeters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsLength.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyBillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PurchaseAc.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Filter_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsLength.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyBillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RateMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SetNo.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyBillNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SetNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_PartyBillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SetNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RateMeters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress

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
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer

        If ActiveControl.Name = dgv_PavuDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing
            If ActiveControl.Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_PavuDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1

                If dgv1.Name = dgv_PavuDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                .Rows.Add()
                                'dgv_YarnDetails.Focus()
                                'dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_PavuDetails.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                txt_RateMeters.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_PcsLength.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 2)

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Da = New SqlClient.SqlDataAdapter("select count(*) from Pavu_Purchase_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Purchase_Code = '" & Trim(NewCode) & "' and Sizing_Specification_Code <> ''", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)
        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
        '        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("Already Sizing specifiation Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  ( Pavu_Delivery_Code <> '' or Pavu_Delivery_Increment <> 0 or Beam_Knotting_Code <> '' or Loom_Idno <> 0 or Production_Meters <> 0 or Close_Status <> 0 )", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some Pavu Delivered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Loom_Idno = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Pavu_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'SIZING') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

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
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_EndsCount.Text = ""
            cbo_Filter_MillName.Text = ""
            txt_Filter_SetNo.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_EndsCount.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Purchase_No from Pavu_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Entry_VAT_GST_Type <> 'GST' Order by for_Orderby, Pavu_Purchase_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Purchase_No from Pavu_Purchase_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Entry_VAT_GST_Type <> 'GST' Order by for_Orderby, Pavu_Purchase_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Purchase_No from Pavu_Purchase_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Entry_VAT_GST_Type <> 'GST' Order by for_Orderby desc, Pavu_Purchase_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Purchase_No from Pavu_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Entry_VAT_GST_Type <> 'GST' Order by for_Orderby desc, Pavu_Purchase_No desc", con)
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
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True
            lbl_NewSTS.Visible = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Pavu_Purchase_Head", "Pavu_Purchase_Code", "For_OrderBy", "Entry_VAT_GST_Type <> 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Pavu_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type <> 'GST' Order by for_Orderby desc, Pavu_Purchase_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Pavu_Purchase_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Pavu_Purchase_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

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

            inpno = InputBox("Enter Rec.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Pavu_Purchase_No from Pavu_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Purchase_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Rec No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Pavu_Purchase_No from Pavu_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Purchase_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim DelvTo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Nr As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = "", YrnPartcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim vTotPvuBms As Single = 0
        Dim vTotPvuPcs As Single = 0
        Dim vTotPvuMtrs As Single = 0
        Dim vTotPvuStk As Single = 0
        Dim Dup_SetNoBmNo As String = ""
        Dim Mtr_Pc As Single = 0
        Dim pCnt_ID As Integer = 0
        Dim pEds_Nm As String = ""
        Dim vSetCd As String = ""
        Dim Selc_SetCode As String = ""
        Dim VouBil As String = ""
        Dim Del_ID As Integer = 0, Rec_ID As Integer = 0
        Dim Stock_In As String = ""
        Dim mtrspcs As Integer = 0
        Dim StkAt_IdNo As Integer = 0
        Dim PurcAc_ID As Integer = 0
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Sizing_Specification_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Sizing.Enabled And cbo_Sizing.Visible Then cbo_Sizing.Focus()
            Exit Sub
        End If

        If Trim(txt_SetNo.Text) = "" Then
            MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_SetNo.Enabled And txt_SetNo.Visible Then txt_SetNo.Focus()
            Exit Sub
        End If

        vSetCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(txt_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Selc_SetCode = Trim(txt_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

        DelvTo_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        If DelvTo_ID = 0 Then
            DelvTo_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
            cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, DelvTo_ID)
            'MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'If cbo_Sizing.Enabled And cbo_Sizing.Visible Then cbo_Sizing.Focus()
            'Exit Sub
        End If

        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        If EdsCnt_ID = 0 Then
            MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
            Exit Sub
        End If
        PurcAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAc.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        'If PurcAc_ID = 0 And Val(lbl_NetAmount.Text) <> 0 Then
        '    MessageBox.Show("Invalid Purchase A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_PurchaseAc.Enabled And cbo_PurchaseAc.Visible Then cbo_PurchaseAc.Focus()
        '    Exit Sub
        'End If

        With dgv_PavuDetails


            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If


                    If InStr(1, Trim(.Rows(i).Cells(3).Value), " ") > 0 Then
                        MessageBox.Show("Invalid Beam No, Spaces not allowed in SetNo", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(UCase(Dup_SetNoBmNo)), "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate BeamNo ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_SetNoBmNo = Trim(Dup_SetNoBmNo) & "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~"

                End If

            Next i
        End With

        TotalPavu_Calculation()

        vTotPvuBms = 0 : vTotPvuMtrs = 0
        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTotPvuBms = Val(dgv_PavuDetails_Total.Rows(0).Cells(1).Value())
            vTotPvuPcs = Val(dgv_PavuDetails_Total.Rows(0).Cells(2).Value())
            vTotPvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(3).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Pavu_Purchase_Head", "Pavu_Purchase_Code", "For_OrderBy", "Entry_VAT_GST_Type <> 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then
                cmd.CommandText = "Insert into Pavu_Purchase_Head ( Pavu_Purchase_Code, Company_IdNo, Pavu_Purchase_No, for_OrderBy, Pavu_Purchase_Date, Ledger_IdNo, DeliveryTo_IdNo, Party_DcNo, Set_No,PurchaseAc_IdNo , EndsCount_IdNo, Pcs_Length, Total_Beam,Total_Pcs ,Total_Meters,Rate_Meters,Gross_Amount,Add_Less,Net_Amount,User_idno  ) Values ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Led_ID)) & ", " & Str(Val(DelvTo_ID)) & ", '" & Trim(txt_PartyBillNo.Text) & "', '" & Trim(txt_SetNo.Text) & "'," & Str(Val(PurcAc_ID)) & ", " & Val(EdsCnt_ID) & ", " & Val(txt_PcsLength.Text) & ", " & Str(Val(vTotPvuBms)) & "," & Str(Val(vTotPvuPcs)) & ", " & Str(Val(vTotPvuMtrs)) & "," & Val(txt_RateMeters.Text) & "," & Val(lbl_GrossAmount.Text) & "," & Val(txt_AddLess.Text) & "," & Val(lbl_NetAmount.Text) & " ," & Val(lbl_UserName.Text) & ")"
                cmd.ExecuteNonQuery()

            Else

                'Da = New SqlClient.SqlDataAdapter("select count(*) from Pavu_Purchase_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Purchase_Code = '" & Trim(NewCode) & "' and Sizing_Specification_Code <> ''", con)
                'Da.SelectCommand.Transaction = tr
                'Dt1 = New DataTable
                'Da.Fill(Dt1)
                'If Dt1.Rows.Count > 0 Then
                '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                '        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                '            Throw New ApplicationException("Already Sizing specifiation Prepared")
                '            Exit Sub
                '        End If
                '    End If
                'End If
                'Dt1.Clear()

                cmd.CommandText = "Update Pavu_Purchase_Head set Pavu_Purchase_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", DeliveryTo_IdNo = " & Str(Val(DelvTo_ID)) & ", Party_DcNo = '" & Trim(txt_PartyBillNo.Text) & "',PurchaseAc_IdNo = " & Str(Val(PurcAc_ID)) & ",  Set_No = '" & Trim(txt_SetNo.Text) & "', EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Pcs_Length = '" & Trim(txt_PcsLength.Text) & "', Total_Beam = " & Str(Val(vTotPvuBms)) & " ,Total_pcs = " & Str(Val(vTotPvuPcs)) & " , Total_Meters =  " & Str(Val(vTotPvuMtrs)) & " ,Rate_Meters = " & Val(txt_RateMeters.Text) & ", Gross_Amount = " & Val(lbl_GrossAmount.Text) & ", Add_Less = " & Val(txt_AddLess.Text) & ", Net_Amount = " & Val(lbl_NetAmount.Text) & " , User_idno = " & Val(lbl_UserName.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Purchase_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            Partcls = "Pavu Rcpt : Set No. " & Trim(txt_SetNo.Text)
            PBlNo = Trim(txt_SetNo.Text)

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Production_Meters = 0 and Close_Status = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Pavu_Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            pCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ")", , tr))
            pEds_Nm = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Ends_Name", "(EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ")", , tr))

            StkAt_IdNo = Val(DelvTo_ID)

            With dgv_PavuDetails
                Sno = 0
                For i = 0 To dgv_PavuDetails.RowCount - 1

                    If Val(dgv_PavuDetails.Rows(i).Cells(3).Value) <> 0 Then

                        Sno = Sno + 1

                        Mtr_Pc = 0
                        If Val(.Rows(i).Cells(2).Value) Then
                            Mtr_Pc = Format(Val(.Rows(i).Cells(3).Value) / Val(.Rows(i).Cells(2).Value), "#########0.00")
                        End If

                        cmd.CommandText = "Insert into Pavu_Purchase_Details (  Pavu_Purchase_Code ,              Company_IdNo        ,     Pavu_Purchase_No    ,                               for_OrderBy                              , Pavu_Purchase_Date ,                                  Ledger_IdNo     ,               Set_No          ,         Set_Code      ,             Sl_No    ,                    Beam_No             ,                      Pcs                 ,          Meters_Pc       ,                       Meters               ) " & _
                                                    "    Values                  ( '" & Trim(NewCode) & "'     , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate             ,  " & Str(Val(Led_ID)) & ", '" & Trim(txt_SetNo.Text) & "', '" & Trim(vSetCd) & "', " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ",  " & Str(Val(Mtr_Pc)) & ",  " & Str(Val(.Rows(i).Cells(3).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                        Nr = 0
                        cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Reference_Date = @EntryDate, Sl_No = " & Str(Val(Sno)) & " " & _
                                            " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "' and Beam_No = '" & Trim(.Rows(i).Cells(1).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Stock_SizedPavu_Processing_Details (                     Reference_Code          ,              Company_IdNo        ,               Reference_No    ,                               for_OrderBy                              , Reference_Date,         Ledger_IdNo     ,           StockAt_IdNo      ,         Set_Code      ,           Set_No              ,    setcode_forSelection     ,      Ends_Name         ,     count_idno           ,         EndsCount_IdNo     , Mill_IdNo, Beam_Width_Idno, Sizing_SlNo,         Sl_No        ,                    Beam_No             ,                               ForOrderBy_BeamNo                                 , Gross_Weight, Tare_Weight, Net_Weight,                      Noof_Pcs            ,          Meters_Pc      ,                      Meters               ) " & _
                                                        "    Values                           ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(Led_ID)) & ", " & Str(Val(StkAt_IdNo)) & ", '" & Trim(vSetCd) & "', '" & Trim(txt_SetNo.Text) & "', '" & Trim(Selc_SetCode) & "', '" & Trim(pEds_Nm) & "', " & Str(Val(pCnt_ID)) & ", " & Str(Val(EdsCnt_ID)) & ",     0    ,      0         ,      0     , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(1).Value))) & ",      0      ,       0    ,      0    , " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(Mtr_Pc)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & " ) "
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next

            End With

            Del_ID = Val(DelvTo_ID)
            Rec_ID = Val(Led_ID)

            If Val(vTotPvuMtrs) <> 0 Then

                Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from EndsCount_Head Where EndsCount_IdNo = " & Str(Val(EdsCnt_ID)), con)
                Da.SelectCommand.Transaction = tr
                dt2 = New DataTable
                Da.Fill(dt2)

                Stock_In = ""
                mtrspcs = 0
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

                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(EdsCnt_ID)) & ", " & Val(vTotPvuBms) & ", " & Str(Val(vTotPvuStk)) & " )"
                cmd.ExecuteNonQuery()

                YrnPartcls = Partcls & ", EndsCount : " & Val(EdsCnt_ID) & ", Beams : " & Val(vTotPvuBms) & ", Meters : " & Val(vTotPvuMtrs)

                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "', 1, " & Str(Val(pCnt_ID)) & ", 'MILL', 0, 0, 0, 0 )"
                cmd.ExecuteNonQuery()

            End If

            If Val(vTotPvuBms) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Pavu_Beam, Particulars ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', 2, 0, " & Str(Val(vTotPvuBms)) & ", '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()
            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            'If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            If Val(lbl_NetAmount.Text) <> 0 Then
                vLed_IdNos = Led_ID & "|" & PurcAc_ID
                vVou_Amts = Val(CSng(lbl_NetAmount.Text)) & "|" & -1 * Val(CSng(lbl_NetAmount.Text))
                If Common_Procedures.Voucher_Updation(con, "Pavu.Purc", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(dtp_Date.Text), "Bill No : " & Trim(txt_PartyBillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If
            End If

            'If Val(lbl_CommAmount.Text) <> 0 Then
            '    vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
            '    vVou_Amts = Val(lbl_CommAmount.Text) & "|" & -1 * Val(lbl_CommAmount.Text)
            '    If Common_Procedures.Voucher_Updation(con, "Ag.Comm", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_Date.Text), "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
            '        Throw New ApplicationException(ErrMsg)
            '    End If

            'End If

            'Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, Led_ID, Trim(txt_PartyBillNo.Text), 0, Val(lbl_NetAmount.Text), "CR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If




            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()


            move_record(lbl_RefNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()
            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            e.Handled = True

            'dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(0)
            'dgv_YarnDetails.CurrentCell.Selected = True
            'dgv_YarnDetails.Focus()

            ' SendKeys.Send("+{TAB}")
        End If

    End Sub

    Private Sub cbo_Sizing_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Sizing_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing, msk_Date, txt_PartyBillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Sizing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing, txt_PartyBillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Sizing_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Sizing.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, txt_SetNo, cbo_PurchaseAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_PurchaseAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, cbo_PurchaseAc, txt_PcsLength, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_PcsLength, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub



    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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
                Condt = "a.Pavu_Purchase_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Pavu_Purchase_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Pavu_Purchase_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Pavu_Purchase_Code IN (select z1.Pavu_Purchase_Code from Sizing_SpecificationYarn_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Pavu_Purchase_Code IN (select z1.Pavu_Purchase_Code from Sizing_SpecificationYarn_Details z1 where z1.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ")"
            End If

            If Val(EdsCnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo))
            End If

            If Trim(txt_Filter_SetNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Set_No = '" & Trim(txt_Filter_SetNo.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as SizingName , c.*  from Pavu_Purchase_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head c ON c.EndsCount_IdNo = a.EndsCount_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Pavu_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Pavu_Purchase_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Pavu_Purchase_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Set_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Pavu_Purchase_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("SizingName").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Total_Beam").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Val(dt2.Rows(i).Item("Total_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(9).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'SIZING' )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'SIZING' )", "(Ledger_IdNo = 0)")

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

    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        dgv_PavuDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter
        With dgv_PavuDetails
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.RowIndex > 0 And e.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                    .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1
                    .CurrentRow.Cells(2).Value = .Rows(e.RowIndex - 1).Cells(2).Value
                    .CurrentRow.Cells(3).Value = .Rows(e.RowIndex - 1).Cells(3).Value
                    '.Rows.Add()
                End If
                If e.ColumnIndex = 1 And e.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(2).Value) = 0 And Val(.CurrentRow.Cells(3).Value) = 0 Then
                    .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1
                    .CurrentRow.Cells(2).Value = .Rows(e.RowIndex - 1).Cells(2).Value
                    .CurrentRow.Cells(3).Value = .Rows(e.RowIndex - 1).Cells(3).Value
                    '.Rows.Add()
                End If
            End If

        End With
    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave
        With dgv_PavuDetails
            If .CurrentCell.ColumnIndex = 3 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        Try
            If Not IsNothing(dgv_PavuDetails.CurrentCell) Then
                If FrmLdSTS = True Then Exit Sub
                If MovSTS = True Then Exit Sub

                With dgv_PavuDetails
                    If .Visible Then
                        If e.ColumnIndex = 2 Then
                            .CurrentRow.Cells(3).Value = Format(Val(.CurrentRow.Cells(2).Value) * Val(txt_PcsLength.Text), "#########0.00")
                        End If
                        If e.ColumnIndex = 3 Then
                            TotalPavu_Calculation()
                        End If
                        If (.CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3) And Val(.CurrentCell.Value) <> 0 Then
                            If .CurrentRow.Index = .Rows.Count - 1 Then
                                .Rows.Add()
                            End If
                        End If

                    End If
                End With
            End If

        Catch ex As Exception
            '----

        End Try

    End Sub


    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_PavuDetails = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_PavuDetails_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.GotFocus
        '--
    End Sub

    Private Sub dgv_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

                If Val(.Rows(.CurrentRow.Index).Cells(4).Value) = 0 Then

                    n = .CurrentRow.Index

                    If .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else

                        .Rows.RemoveAt(n)

                    End If

                    TotalPavu_Calculation()

                Else
                    MessageBox.Show("Already Pavu delivered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End With

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
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
        Dim TotBms As Single, TotPcs As Single, TotMtrs As Single

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotBms = 0
        TotPcs = 0
        TotMtrs = 0
        With dgv_PavuDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(3).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotPcs = TotPcs + Val(.Rows(i).Cells(2).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(3).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotBms)
            .Rows(0).Cells(2).Value = Val(TotPcs)
            .Rows(0).Cells(3).Value = Format(Val(TotMtrs), "########0.00")
        End With
        lbl_GrossAmount.Text = Format(Val(txt_RateMeters.Text) * Val(TotMtrs), "##########0.00")
    End Sub
    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub
    Private Sub cbo_Filter_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_EndsCountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsCount, cbo_Filter_MillName, txt_Filter_SetNo, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_EndsCountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsCount, txt_Filter_SetNo, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    btn_Filter_Show_Click(sender, e)
        'End If
    End Sub


    Private Sub txt_PcsLength_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PcsLength.KeyDown
        If e.KeyCode = 40 Then
            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                dgv_PavuDetails.CurrentCell.Selected = True
            Else
                btn_save.Focus()
            End If
        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_PcsLength_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsLength.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                dgv_PavuDetails.CurrentCell.Selected = True
            Else
                btn_save.Focus()
            End If
        End If
    End Sub

    Private Sub dgtxt_PavuDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.Enter
        dgv_ActiveCtrl_Name = dgv_PavuDetails.Name
        dgv_PavuDetails.EditingControl.BackColor = Color.Lime
        dgv_PavuDetails.EditingControl.ForeColor = Color.Blue
        dgv_PavuDetails.SelectAll()
    End Sub

    Private Sub dgtxt_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyDown
        If Val(dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(4).Value) <> 0 Then
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

    Private Sub dgtxt_PavuDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_PavuDetails.KeyPress
        If Val(dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(4).Value) <> 0 Then
            e.Handled = True
        Else
            If dgv_PavuDetails.CurrentCell.ColumnIndex = 2 Or dgv_PavuDetails.CurrentCell.ColumnIndex = 3 Then
                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
            End If
        End If
    End Sub

    Private Sub dgtxt_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_PavuDetails_KeyUp(sender, e)
        End If
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , d.EndsCount_Name from Pavu_Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo  where a.Pavu_Purchase_Code = '" & Trim(NewCode) & "'", con)
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

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
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


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_PageNo = 0
        prn_NoofBmDets = 0
        prn_DetMxIndx = 0
        Erase prn_DetAr

        prn_DetAr = New String(50, 10) {}

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , d.EndsCount_Name ,f.* from Pavu_Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo where a.Pavu_Purchase_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Pavu_Purchase_Details a where Pavu_Purchase_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(i).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(i).Item("Set_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Beam_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt.Rows(i).Item("Pcs").ToString)
                            prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
                            prn_DetAr(prn_DetMxIndx, 5) = Trim(Microsoft.VisualBasic.Left(prn_HdDt.Rows(0).Item("EndsCount_Name").ToString, 15))
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

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 65 : ClArr(2) = 55 : ClArr(3) = 50 : ClArr(4) = 75 : ClArr(5) = 120
        ClArr(6) = 65 : ClArr(7) = 55 : ClArr(8) = 50 : ClArr(9) = 75
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' Try

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
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)

                        prn_NoofBmDets = prn_NoofBmDets + 1

                    End If

                    If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                        prn_NoofBmDets = prn_NoofBmDets + 1

                    End If

                    NoofDets = NoofDets + 1

                Loop

            End If

            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

        End If

        ' Catch ex As Exception

        ' MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

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

        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PAVU RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "From :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Purchase_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Purchase_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + M1 + 4, LnAr(3), LMargin + M1 + 4, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

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
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    End If

                Else

                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    End If

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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + 20, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 5, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 50, LnAr(3))

            'CurY = CurY + TxtHgt - 10
            'If Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Vechile_No No  :  " & Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY
            Common_Procedures.Print_To_PrintDocument(e, "Received The Beams As Per Above Details.", LMargin + 10, CurY + 5, 0, 0, pFont)
            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt


            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then


            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub
    Private Sub cbo_PurchaseAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PurchaseAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAc, cbo_DeliveryTo, cbo_EndsCount, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAc, cbo_EndsCount, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PurchaseAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_RateMeters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RateMeters.KeyDown
        If e.KeyCode = 38 Then
            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                dgv_PavuDetails.CurrentCell.Selected = True
            Else
                txt_PcsLength.Focus()
            End If
        End If

        If e.KeyCode = 40 Then txt_AddLess.Focus()
    End Sub

    Private Sub txt_RateMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RateMeters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_RateMeters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_RateMeters.TextChanged
        TotalPavu_Calculation()
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        lbl_NetAmount.Text = Format(Val(lbl_GrossAmount.Text) + Val(txt_AddLess.Text), "########0.00")
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


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

    Private Sub dgv_PavuDetails_RowDefaultCellStyleChanged(sender As Object, e As DataGridViewRowEventArgs) Handles dgv_PavuDetails.RowDefaultCellStyleChanged

    End Sub
End Class