Public Class Garments_PackingSlip_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FPPCK-"
    Private Prec_ActCtrl As New Control
    Private dgv_ActiveCtrl_Name As String
    Private vcbo_KeyDwnVal As Single

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private Filter_RowNo As Integer = -1
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetIndx1 As Integer
    Private prn_DetIndx2 As Integer
    Private prn_HdAr(500, 10) As String
    Private prn_DetAr(500, 500, 10) As String

    Private prn_SIZAr(50) As String
    Private prn_TOTSIZS As Integer
    Private prn_SIZCOLWIDTH As String
    Private prn_HdMxIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_TOTITEMS As Integer
    Private prn_Status As Integer
    Private prn_DetSNo As Integer
    Dim prn_DIC_SIZCOL_LEFT As New Dictionary(Of String, String)
    Dim prn_DIC_SIZCOL_WIDTH As New Dictionary(Of String, String)


    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_OrderSelection.Visible = False

        lbl_CartonNo.Text = ""
        lbl_CartonNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_Ledger.Text = ""
        txt_orderdate.Text = ""
        txt_OrderNo.Text = ""
        cbo_Transport.Text = ""
        lbl_Sales_Order_Code.Text = ""
        lbl_Invoice_Code.Text = ""
        txt_Note.Text = ""
        cbo_Type.Text = "DIRECT"
        cbo_Type.Enabled = False

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Ledger.Tag = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_Grid_ItemName.Visible = False
        cbo_Grid_Size.Visible = False
        cbo_Grid_Unit.Visible = False
        Grid_Cell_DeSelect()
        dgv_ActiveCtrl_Name = ""

    End Sub
    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.SuppressKeyPress = True : e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.SuppressKeyPress = True : e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        On Error Resume Next


        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is CheckBox Then
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
        If Me.ActiveControl.Name <> cbo_Grid_ItemName.Name Then
            cbo_Grid_ItemName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Size.Name Then
            cbo_Grid_Size.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Unit.Name Then
            cbo_Grid_Unit.Visible = False
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False

        'dgv_Details.CurrentCell.Selected = False
        'dgv_Details_Total.CurrentCell.Selected = False
        'dgv_Filter_Details.CurrentCell.Selected = False

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName from Garments_Item_PackingSlip_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where a.Item_PackingSlip_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_CartonNo.Text = dt1.Rows(0).Item("Item_PackingSlip_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Item_PackingSlip_Date").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                txt_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                txt_orderdate.Text = dt1.Rows(0).Item("Order_Date").ToString
                txt_Note.Text = dt1.Rows(0).Item("Remarks").ToString
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                lbl_Sales_Order_Code.Text = dt1.Rows(0).Item("Sales_order_Code").ToString
                lbl_Invoice_Code.Text = dt1.Rows(0).Item("Invoice_Code").ToString

                If IsDBNull(dt1.Rows(0).Item("Invoice_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Invoice_Code").ToString) <> "" Then LockSTS = True
                End If

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name from Garments_Item_PackingSlip_Details a, Item_Head b, Size_Head c where a.Item_PackingSlip_Code = '" & Trim(NewCode) & "' and a.Item_IdNo = b.Item_Idno and a.Size_Idno = c.Size_Idno Order by a.Sl_No", con)
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Name").ToString

                        dgv_Details.Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Sales_order_Code").ToString

                        If IsDBNull(dt1.Rows(0).Item("Invoice_Code").ToString) = False Then
                            If Trim(dt1.Rows(0).Item("Invoice_Code").ToString) <> "" Then
                                For j = 0 To dgv_Details.ColumnCount - 1
                                    dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If
                        End If

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                End With

                'dgv_Details.CurrentCell.Selected = False

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CartonNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If LockSTS = True Then
                    cbo_Ledger.Enabled = False
                    cbo_Ledger.BackColor = Color.LightGray
                End If

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub PackingSlip_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Size.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SIZE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Size.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub PackingSlip_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        FrmLdSTS = True

        Me.Text = ""

        con.Open()


        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("ORDER")


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2



        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_OrderSelection.Visible = False
        pnl_OrderSelection.Left = (Me.Width - pnl_OrderSelection.Width) \ 2
        pnl_OrderSelection.Top = (Me.Height - pnl_OrderSelection.Height) \ 2
        pnl_OrderSelection.BringToFront()

        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_orderdate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Preprint.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Size.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_orderdate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Preprint.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Size.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_orderdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_orderdate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub PackingSlip_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub PackingSlip_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub


                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_OrderSelection.Visible = True Then
                    btn_Close_OrderSelection_Click(sender, e)
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer

        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Details.Name Then
                dgv1 = dgv_Details



            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1

                If dgv1.Name = dgv_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Note.Focus()

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
                                If Trim(cbo_Type.Text) = "ORDER" Then

                                    cbo_Ledger.Focus()
                                Else

                                    cbo_Transport.Focus()
                                End If

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim InvCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT Delete...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CartonNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



        InvCode = Common_Procedures.get_FieldValue(con, "Garments_Item_PackingSlip_Head", "Invoice_Code", "(Item_PackingSlip_Code = '" & Trim(NewCode) & "')", Val(lbl_Company.Tag))
        If Trim(InvCode) <> "" Then
            MessageBox.Show("Already Invoiced", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If Trim(UCase(cbo_Type.Text)) = "ORDER" Then


                cmd.CommandText = "Update Sales_Order_Details set Invoice_Quantity = a.Invoice_Quantity - b.Quantity from Sales_Order_Details a, Garments_Item_PackingSlip_Details b Where b.Item_PackingSlip_Code = '" & Trim(NewCode) & "' and a.Sales_Order_Code = b.Sales_Order_Code and a.Item_IdNo = b.Item_IdNo and a.Size_idNo = b.Size_IdNo and a.Unit_idno = b.Unit_idno"
                cmd.ExecuteNonQuery()
            End If




            cmd.CommandText = "Delete from Garments_Item_PackingSlip_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_PackingSlip_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Garments_Item_PackingSlip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PackingSlip_Code = '" & Trim(NewCode) & "'"
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

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Item_PackingSlip_No from Garments_Item_PackingSlip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PackingSlip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Item_PackingSlip_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_CartonNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Item_PackingSlip_No from Garments_Item_PackingSlip_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PackingSlip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Item_PackingSlip_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_CartonNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Item_PackingSlip_No from Garments_Item_PackingSlip_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PackingSlip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Item_PackingSlip_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Item_PackingSlip_No from Garments_Item_PackingSlip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PackingSlip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Item_PackingSlip_No desc", con)
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
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_CartonNo.Text = Common_Procedures.get_MaxCode(con, "Garments_Item_PackingSlip_Head", "Item_PackingSlip_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_CartonNo.ForeColor = Color.Red
            da1 = New SqlClient.SqlDataAdapter("select Top 1 a.* from Garments_Item_PackingSlip_Head a  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Item_PackingSlip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Item_PackingSlip_No desc", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                If dt1.Rows(0).Item("Selection_Type").ToString <> "" Then cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString

            End If

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt1.Dispose()
        da1.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Carton.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Item_PackingSlip_No from Garments_Item_PackingSlip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PackingSlip_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Bale No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Carton No.", "FOR NEW CARTON INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Item_PackingSlip_No from Garments_Item_PackingSlip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PackingSlip_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Carton No", "DOES NOT INSERT NEW CARTON...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_CartonNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW CARTON...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim led_id As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim vTotQty As Single, vTotMtrs As Single
        Dim InvCode As String = ""
        Dim Itm_ID As Integer = 0
        Dim Sz_Id As Integer = 0
        Dim vOrdDate As String = ""

        Dim Trans_id As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.PackinSlip_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Trans_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Then

                If Trim(dgv_Details.Rows(i).Cells(1).Value) = "" Then
                    MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                        '  dgv_Details.CurrentCell.Selected = True
                    End If
                    Exit Sub
                End If

            End If

        Next

        vTotQty = 0 : vTotMtrs = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotQty = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
        End If
        If Trim(UCase(cbo_Type.Text)) <> "ORDER" Then
            lbl_Sales_Order_Code.Text = ""
        End If

        If Trim(txt_orderdate.Text) = "" Then
            vOrdDate = Trim(dtp_Date.Text)
        Else
            vOrdDate = Trim(txt_orderdate.Text)
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CartonNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_CartonNo.Text = Common_Procedures.get_MaxCode(con, "Garments_Item_PackingSlip_Head", "Item_PackingSlip_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CartonNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PackingDate", dtp_Date.Value.Date)


            If New_Entry = True Then
                cmd.CommandText = "Insert into Garments_Item_PackingSlip_Head ( Item_PackingSlip_Code, Company_IdNo, Item_PackingSlip_No, for_OrderBy, Item_PackingSlip_Date, Ledger_IdNo,Selection_Type, Order_No,Order_Date  ,TransPort_IdNo , Total_Quantity, Remarks,Sales_order_Code ,  Invoice_Code, Invoice_Increment ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_CartonNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_CartonNo.Text))) & ", @PackingDate, " & Str(Val(led_id)) & ", '" & Trim(cbo_Type.Text) & "','" & Trim(txt_OrderNo.Text) & "', '" & Trim(vOrdDate) & "' ," & Val(Trans_id) & " , " & Str(Val(vTotQty)) & ", '" & Trim(txt_Note.Text) & "' ,'" & Trim(lbl_Sales_Order_Code.Text) & "', '', 0)"
                cmd.ExecuteNonQuery()

            Else

                InvCode = Common_Procedures.get_FieldValue(con, "Garments_Item_PackingSlip_Head", "Invoice_Code", "(Item_PackingSlip_Code = '" & Trim(NewCode) & "')", Val(lbl_Company.Tag), tr)
                If Trim(InvCode) <> "" Then
                    tr.Rollback()
                    MessageBox.Show("Already Invoiced", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                    Exit Sub
                End If

                cmd.CommandText = "Update Garments_Item_PackingSlip_Head SET Item_PackingSlip_Date = @PackingDate, Ledger_IdNo = " & Str(Val(led_id)) & ",Selection_Type = '" & Trim(cbo_Type.Text) & "', Order_No = '" & Trim(txt_OrderNo.Text) & "',   Order_date = '" & Trim(vOrdDate) & "', TransPort_IdNo = " & Val(Trans_id) & " ,Total_Quantity = " & Str(Val(vTotQty)) & ",  Remarks = '" & Trim(txt_Note.Text) & "', Sales_order_Code = '" & Trim(lbl_Sales_Order_Code.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_PackingSlip_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
                    cmd.CommandText = "Update Sales_Order_Details set Invoice_Quantity = a.Invoice_Quantity - b.Quantity from Sales_Order_Details a, Garments_Item_PackingSlip_Details b Where b.Item_PackingSlip_Code = '" & Trim(NewCode) & "' and a.Sales_Order_Code = b.Sales_Order_Code and a.Item_IdNo = b.Item_IdNo and a.Size_idNo = b.Size_IdNo and a.Unit_idno = b.Unit_idno"
                    cmd.ExecuteNonQuery()
                End If

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_CartonNo.Text)
            Partcls = "Pack : Bale.No. " & Trim(lbl_CartonNo.Text)
            PBlNo = Trim(lbl_CartonNo.Text)

            cmd.CommandText = "Delete from Garments_Item_PackingSlip_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_PackingSlip_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0
            Dim Nr As Integer = 0
            For i = 0 To dgv_Details.RowCount - 1

                If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Then

                    Itm_ID = Common_Procedures.Item_NameToIdNo1(con, dgv_Details.Rows(i).Cells(1).Value, tr) 'Common_Procedures.Item_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value, tr)

                    Sz_Id = 0 'Common_Procedures.Size_NameToIdNo(con, dgv_Details.Rows(i).Cells(2).Value, tr)
                    Unt_ID = 0 'Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(3).Value, tr)
                    dgv_Details.Rows(i).Cells(3).Value = Trim(lbl_Sales_Order_Code.Text)

                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Garments_Item_PackingSlip_Details ( Item_PackingSlip_Code, Company_IdNo, Item_PackingSlip_No, for_OrderBy, Item_PackingSlip_Date, Ledger_IdNo, Sl_No, Item_IdNo, Size_Idno,Unit_IdNo  , Quantity  , Sales_Order_Code  ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_CartonNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_CartonNo.Text))) & ", @PackingDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(Itm_ID)) & ", " & Str(Val(Sz_Id)) & ", " & Val(Unt_ID) & " ," & Str(Val(dgv_Details.Rows(i).Cells(2).Value)) & " , '" & Trim(dgv_Details.Rows(i).Cells(3).Value) & "' )"
                    cmd.ExecuteNonQuery()

                    If Trim(UCase(cbo_Type.Text)) = "ORDER" Then

                        If Trim(lbl_Sales_Order_Code.Text) <> "" Then
                            Nr = 0
                            cmd.CommandText = "Update Sales_Order_Details Set Invoice_Quantity = Invoice_Quantity + " & Str(Val(dgv_Details.Rows(i).Cells(2).Value)) & " Where Sales_Order_Code = '" & Trim(dgv_Details.Rows(i).Cells(3).Value) & "' and Item_IdNo = " & Str(Val(Itm_ID)) & " and Size_idno = " & Val(Sz_Id) & " and Unit_idNo = " & Val(Unt_ID) & "  and Ledger_IdNo = " & Str(Val(led_id))
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Order and Item Details - " & dgv_Details.Rows(i).Cells(1).Value)
                                Exit Sub
                            End If
                        End If

                    End If

                End If

            Next

            'Dim fpitmnm As String = ""
            'Dim fpSznm As String = ""

            'If Trim(UCase(cbo_Type.Text)) = "ORDER" Then

            '    cmd.CommandText = "truncate table entrytemp"
            '    cmd.ExecuteNonQuery()

            '    cmd.CommandText = "insert into entrytemp(int1,int2 , weight1) select Item_IdNo,Size_idNo, Quantity from Garments_Item_PackingSlip_Details Where Item_PackingSlip_Code = '" & Trim(NewCode) & "'"
            '    cmd.ExecuteNonQuery()

            '    cmd.CommandText = "insert into entrytemp(int1,int2 , weight1) select Item_IdNo,Size_idNo, -1*Quantity from Garments_Item_PackingSlip_Details Where Item_PackingSlip_Code = '" & Trim(NewCode) & "'"
            '    cmd.ExecuteNonQuery()

            '    Da = New SqlClient.SqlDataAdapter("select int1 as Itm_IdNo,int2 as Size_idNo , sum(weight1) from entrytemp group by int1,int2 having sum(weight1) <> 0", con)
            '    Da.SelectCommand.Transaction = tr
            '    Dt1 = New DataTable
            '    Da.Fill(Dt1)
            '    If Dt1.Rows.Count > 0 Then
            '        fpitmnm = Common_Procedures.Item_NameToIdNo(con, Dt1.Rows(0).Item("Itm_IdNo").ToString, tr)
            '        fpSznm = Common_Procedures.Size_NameToIdNo(con, Dt1.Rows(0).Item("Size_IdNo").ToString, tr)
            '        Throw New ApplicationException("Mismatch of Quantity in Invoice and Order Details" & Chr(13) & "ItemName  :  " & Trim(fpitmnm))
            '        Exit Sub
            '    End If
            '    Dt1.Clear()

            'End If


            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            move_record(lbl_CartonNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ck_Sales_Order_Details_1"))) > 0 Then
                MessageBox.Show("Invalid Invoice Quantity, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ck_Sales_Order_Details_2"))) > 0 Then
                MessageBox.Show("Invalid Quantity - Invocie Quantity greater than Order Quantity - ", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                'MessageBox.Show("Invalid Invoice Quantity in Order Details - " & (eXmSG), "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : txt_Note.Focus()
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub



    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_Type, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(cbo_Type.Text)) = "ORDER" Then


                If MessageBox.Show("Do you want to select Order?", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)


                    Else
                        txt_Note.Focus()

                    End If

                End If
            Else
                txt_OrderNo.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "ORDER" Then


                If MessageBox.Show("Do you want to select Order?", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                        '   dgv_Details.CurrentCell.Selected = True

                    Else
                        txt_Note.Focus()

                    End If

                End If
            Else
                txt_OrderNo.Focus()

            End If
        End If

    End Sub

    Private Sub cbo_PreparedBy_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_PreparedBy_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_orderdate, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_PreparedBy_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

        End If
    End Sub
    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_ItemName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ItemName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Item_Head", "Item_Name", "", "(Item_Idno = 0)")

    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, btn_Filter_Show, cbo_Filter_PartyName, "Item_Head", "Item_Name", "", "(Item_Idno = 0)")
    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, "Item_Head", "Item_Name", "", "(Item_Idno = 0)")

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

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        Try
            With dgv_Details
                '   dgv_ActCtrlName = dgv_Details.Name

                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If


                If e.ColumnIndex = 1 Then

                    If (cbo_Grid_ItemName.Visible = False And Trim(UCase(lbl_Invoice_Code.Text)) = "" Or Val(cbo_Grid_ItemName.Tag) <> e.RowIndex) Then

                        cbo_Grid_ItemName.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Item_Name from Item_Head order by Item_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_Grid_ItemName.DataSource = Dt1
                        cbo_Grid_ItemName.DisplayMember = "Item_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_ItemName.Left = .Left + rect.Left
                        cbo_Grid_ItemName.Top = .Top + rect.Top

                        cbo_Grid_ItemName.Width = rect.Width
                        cbo_Grid_ItemName.Height = rect.Height
                        cbo_Grid_ItemName.Text = .CurrentCell.Value

                        cbo_Grid_ItemName.Tag = Val(e.RowIndex)
                        cbo_Grid_ItemName.Visible = True

                        cbo_Grid_ItemName.BringToFront()
                        cbo_Grid_ItemName.Focus()

                    End If

                Else
                    cbo_Grid_ItemName.Visible = False

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT ENTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 2 Then
                            Total_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
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

                Total_Calculation()

            End With



        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotQty As Single, TotMtrs As Single

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotQty = 0
        TotMtrs = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then
                    TotQty = TotQty + Val(.Rows(i).Cells(2).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotQty)
        End With

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Close_Form()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim I As Integer, J As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String = ""
        Dim Ent_OrdCd As String = ""
        Dim Ent_Qty As Single = 0
        Dim Ent_rte As Single = 0
        Dim Ent_amt As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim nr As Single = 0

        If Trim(UCase(cbo_Type.Text)) <> "ORDER" Then
            MessageBox.Show("Invalid Invoice Type", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
            Exit Sub
        End If

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CartonNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Trim(UCase(cbo_Type.Text)) = "ORDER" Then

            With dgv_OrderSelection

                ' lbl_Heading_Selection.Text = "ORDER SELECTION"

                .Rows.Clear()

                SNo = 0

                '---1
                Da = New SqlClient.SqlDataAdapter("Select a.*, e.Ledger_Name as Transportname ," &
                                                    " (select sum(z2.Order_Quantity - z2.order_cancel_quantity - z2.Invoice_Quantity) as Balance_Qty  from Sales_Order_Details z2 where z2.Sales_Order_Code = a.Sales_Order_Code ) as Balance_Qty, " &
                                                    " (select sum(z3.Quantity) from Garments_Item_PackingSlip_Details z3 where  z3.Item_PackingSlip_Code = '" & Trim(NewCode) & "'  ) as Ent_Qty " &
                                                    " from Sales_Order_Head a " &
                                                    " LEFT OUTER JOIN Ledger_Head e ON e.Ledger_IdNo <> 0 and a.Transport_IdNo = e.Ledger_IdNo " &
                                                    " Where " &
                                                    " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.Sales_Order_Code IN (select z1.Sales_Order_Code from Garments_Item_PackingSlip_Details z1 Where z1.Item_PackingSlip_Code = '" & Trim(NewCode) & "' ) " &
                                                    " order by a.Sales_Order_Date, a.for_orderby, a.Sales_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                Ent_OrdCd = "'0'"

                If Dt1.Rows.Count > 0 Then

                    For I = 0 To Dt1.Rows.Count - 1



                        n = .Rows.Add()

                        Ent_OrdCd = Trim(Ent_OrdCd) & IIf(Trim(Ent_OrdCd) <> "", ", ", "") & "'" & Dt1.Rows(I).Item("Sales_Order_Code").ToString & "'"

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("Sales_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(I).Item("Sales_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(I).Item("Party_OrderNo").ToString
                        .Rows(n).Cells(4).Value = Val(Dt1.Rows(I).Item("Total_Order_Quantity").ToString)
                        .Rows(n).Cells(5).Value = Val(Dt1.Rows(I).Item("Balance_Qty").ToString) + Val(Dt1.Rows(I).Item("Ent_Qty").ToString)
                        .Rows(n).Cells(6).Value = "1"
                        .Rows(n).Cells(7).Value = Dt1.Rows(I).Item("Transportname").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(I).Item("Sales_Order_Code").ToString

                        For J = 0 To .ColumnCount - 1
                            .Rows(I).Cells(J).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Ent_OrdCd = "(" & Trim(Ent_OrdCd) & ")"

                '---2
                Da = New SqlClient.SqlDataAdapter("Select a.*, e.Ledger_Name as Transportname, " &
                                                    " (select sum(z2.Order_Quantity - z2.Order_Cancel_Quantity - z2.Invoice_Quantity) as Balance_Qty from Sales_Order_Details z2 where z2.Sales_Order_Code = a.Sales_Order_Code ) as Balance_Qty " &
                                                    " from Sales_Order_Head a " &
                                                    " LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo " &
                                                    " Where a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.Sales_Order_Code IN (select z1.Sales_Order_Code from Sales_Order_Details z1 where z1.Sales_Order_Code NOT IN " & Trim(Ent_OrdCd) & " and (z1.Order_Quantity - z1.Order_Cancel_Quantity - z1.Invoice_Quantity) > 0 ) " &
                                                    " Order by a.Sales_Order_Date, a.for_orderby, a.Sales_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For I = 0 To Dt1.Rows.Count - 1


                        n = .Rows.Add()

                        Ent_OrdCd = Trim(Ent_OrdCd) & IIf(Trim(Ent_OrdCd) <> "", ", ", "") & "'" & Dt1.Rows(I).Item("Sales_Order_Code").ToString & "'"

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("Sales_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(I).Item("Sales_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(I).Item("Party_OrderNo").ToString
                        .Rows(n).Cells(4).Value = Val(Dt1.Rows(I).Item("Total_Order_Quantity").ToString)
                        .Rows(n).Cells(5).Value = Val(Dt1.Rows(I).Item("Balance_Qty").ToString)
                        .Rows(n).Cells(6).Value = ""

                        .Rows(n).Cells(7).Value = Dt1.Rows(I).Item("Transportname").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(I).Item("Sales_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
                If .Rows.Count = 0 Then .Rows.Add()

                pnl_OrderSelection.Visible = True
                pnl_Back.Enabled = False

                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True

            End With
        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'pnl_Print.Visible = True
        'pnl_Back.Enabled = False
        'If btn_Print_Invoice.Enabled And btn_Print_Invoice.Visible Then
        '    btn_Print_Invoice.Focus()
        'End If
        printing_invoice()
    End Sub

    Public Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CartonNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Garments_Item_PackingSlip_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Item_PackingSlip_Code = '" & Trim(NewCode) & "'", con)
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

        'If PpSzSTS = False Then
        Dim ps As Printing.PaperSize
        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        'End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                PrintDocument1.Print()
                'PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                '    PrintDocument1.Print()
                'End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(800, 800)

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
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim NewCode As String
        Dim Itm_id As Integer = 0
        Dim cmd As New SqlClient.SqlCommand
        Dim I, J, K As Integer

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CartonNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetDt1.Clear()
        prn_DetIndx2 = 0
        prn_DetIndx = 0

        prn_PageNo = 0
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0

        Erase prn_HdAr
        prn_HdAr = New String(500, 10) {}

        Erase prn_DetAr
        prn_DetAr = New String(500, 500, 10) {}

        prn_TOTSIZS = 0

        Erase prn_SIZAr
        prn_SIZAr = New String(50) {}

        prn_DIC_SIZCOL_LEFT.Clear()
        prn_DIC_SIZCOL_WIDTH.Clear()

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* , D.Ledger_Name as Transport_name from Garments_Item_PackingSlip_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head D ON a.Transport_IdNo = D.Ledger_IdNo where a.Item_PackingSlip_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                cmd.Connection = con
                cmd.CommandText = "Truncate table EntryTemp"
                cmd.ExecuteNonQuery()

                '--- b.Item_DisplayName

                cmd.CommandText = "Insert into EntryTemp(int1   ,   name1           ,   name2    ,   name3                ,   Name4     , meters1 )    " &
                          " Select                    a.quantity, b.Item_DisplayName, C.Size_Name, a.Item_PackingSlip_Code, d.Style_Name, c.Total_Sqft from Garments_Item_PackingSlip_Details a  INNER JOIN Item_Head b On b.Item_Idno <> 0 and a.Item_Idno = b.Item_Idno LEFT OUTER JOIN Size_Head c ON b.Item_Size_IdNo = c.Size_Idno LEFT OUTER JOIN Style_Head d ON d.Style_IdNo <> 0 and b.Item_Style_IdNo = d.Style_IdNo Where a.Item_PackingSlip_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


                prn_TOTSIZS = 0
                da2 = New SqlClient.SqlDataAdapter("select name2, meters1, count(*) as noofsizes from EntryTemp a Where name1 <> '' and name2 <> '' and int1 <> 0 group by name2, meters1 order by meters1, name2 ", con)
                Dt1 = New DataTable
                da2.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    For i = 0 To Dt1.Rows.Count - 1
                        If Trim(Dt1.Rows(i).Item("name2").ToString) <> "" Then
                            prn_TOTSIZS = prn_TOTSIZS + 1
                            prn_SIZAr(prn_TOTSIZS) = Trim(UCase(Dt1.Rows(i).Item("name2").ToString))
                        End If
                    Next i
                End If
                Dt1.Clear()

                If prn_TOTSIZS = 0 Then prn_TOTSIZS = 1


                prn_HdMxIndx = 0
                da2 = New SqlClient.SqlDataAdapter("Select sum(int1) as Qty, name1, name3, name4 from EntryTemp a Where name1 <> '' and int1 <> 0 Group by name1, name3, name4 Having sum(int1) <> 0 ORDER BY name3, name1, name4", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then

                    For i = 0 To prn_DetDt.Rows.Count - 1

                        prn_HdMxIndx = prn_HdMxIndx + 1

                        prn_HdAr(prn_HdMxIndx, 1) = Trim(Val(prn_HdMxIndx))
                        prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("name1").ToString)
                        prn_HdAr(prn_HdMxIndx, 3) = prn_DetDt.Rows(i).Item("name4").ToString
                        prn_HdAr(prn_HdMxIndx, 4) = prn_DetDt.Rows(i).Item("Qty").ToString

                        prn_DetMxIndx = 0

                        da2 = New SqlClient.SqlDataAdapter("Select sum(int1) as Qty, name1, name2, name3, name4, meters1 from EntryTemp a WHERE name1 = '" & Trim(prn_DetDt.Rows(I).Item("name1").ToString) & "' and name4 = '" & Trim(prn_DetDt.Rows(I).Item("name4").ToString) & "' and name2 <> '' and int1 <> 0 Group by name1 ,name2, name3, name4, meters1 Having sum(int1) <> 0 ORDER BY name3, name1, name4, meters1, name2", con)
                        Dt2 = New DataTable
                        da2.Fill(Dt2)

                        If Dt2.Rows.Count > 0 Then
                            For j = 0 To Dt2.Rows.Count - 1
                                If Val(Dt2.Rows(j).Item("qty").ToString) <> 0 Then

                                    prn_DetMxIndx = prn_DetMxIndx + 1

                                    For k = 1 To prn_TOTSIZS
                                        If Trim(prn_SIZAr(K)) <> "" Then
                                            If Trim(UCase(prn_SIZAr(K))) = Trim(UCase(Dt2.Rows(J).Item("name2").ToString)) Then

                                                prn_DetAr(prn_HdMxIndx, K, 1) = Trim(UCase(Dt2.Rows(J).Item("name2").ToString))
                                                prn_DetAr(prn_HdMxIndx, K, 2) = Trim(Dt2.Rows(J).Item("Qty").ToString)
                                                Exit For

                                            End If
                                        End If


                                    Next k

                                End If

                            Next j

                        End If

                    Next i

                Else

                    MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End If

                da1.Dispose()

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
        Printing_Format1(e)
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, K As Integer
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
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
        Dim SNo As Integer
        Dim CurX As Single = 0, CurX1 As Single = 0, Curx2 As Single = 0
        Dim Itm_Nm As String = ""
        Dim Qty As Single = 0
        Dim Cnt_Value As Integer = 0
        Dim Sz_Nm As String = ""
        Dim vSTYLE_Nm As String = ""
        Dim vCLWDTH As String
        Dim vCLLFT As String


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


        NoofItems_PerPage = 8 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        Dim vSZCOLWDTH As String = 0
        Dim vTOTWDTH As String = 0
        Dim vSIZCOL_TOTWDTH As String = 0


        ClArr(1) = 35 : ClArr(2) = 190 : ClArr(3) = 90 : ClArr(13) = 80
        If prn_TOTSIZS <= 1 Then
            ClArr(1) = ClArr(1) + 5
            ClArr(2) = ClArr(2) + 50
            ClArr(3) = ClArr(3) + 50
            ClArr(13) = ClArr(13) + 50
        ElseIf prn_TOTSIZS <= 2 Then
            ClArr(1) = ClArr(1) + 5
            ClArr(2) = ClArr(2) + 40
            ClArr(3) = ClArr(3) + 40
            ClArr(13) = ClArr(13) + 40
        ElseIf prn_TOTSIZS <= 3 Then
            ClArr(1) = ClArr(1) + 5
            ClArr(2) = ClArr(2) + 30
            ClArr(3) = ClArr(3) + 30
            ClArr(13) = ClArr(13) + 30
        ElseIf prn_TOTSIZS <= 4 Then
            ClArr(1) = ClArr(1) + 5
            ClArr(2) = ClArr(2) + 20
            ClArr(3) = ClArr(3) + 20
            ClArr(13) = ClArr(13) + 20
        ElseIf prn_TOTSIZS <= 5 Then
            ClArr(1) = ClArr(1) + 5
            ClArr(2) = ClArr(2) + 10
            ClArr(3) = ClArr(3) + 10
            ClArr(13) = ClArr(13) + 10
        End If
        ClArr(4) = 0 : ClArr(5) = 0 : ClArr(6) = 0 : ClArr(7) = 0 : ClArr(8) = 0 : ClArr(9) = 0 : ClArr(10) = 0 : ClArr(11) = 0 : ClArr(12) = 0
        vSIZCOL_TOTWDTH = Format(PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(13)), "##########0")

        If prn_TOTSIZS = 0 Then prn_TOTSIZS = 1

        vSZCOLWDTH = Val(vSIZCOL_TOTWDTH) \ prn_TOTSIZS

        vTOTWDTH = 0
        CurX = ClArr(1) + ClArr(2) + ClArr(3)
        K = 3
        For I = 1 To IIf(prn_TOTSIZS <= 9, prn_TOTSIZS, 9)

            If I <= prn_TOTSIZS Then

                If Trim(prn_SIZAr(I)) <> "" Then

                    K = K + 1
                    ClArr(K) = Val(vSZCOLWDTH)

                End If

            End If

        Next I

        ClArr(13) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12))

        TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CartonNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

                        CurY = CurY + TxtHgt

                        prn_DetSNo = prn_DetSNo + 1

                        prn_DetIndx = prn_DetIndx + 1

                        ItmNm1 = Trim(prn_HdAr(prn_DetIndx, 2))  ' Trim(prn_DetDt.Rows(prn_DetIndx).Item("name1").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 18 Then
                            For I = 18 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 18
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdAr(prn_DetIndx, 1))), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdAr(prn_DetIndx, 4)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) - 10, CurY, 1, 0, pFont)

                        vCLLFT = LMargin + ClArr(1) + ClArr(2) + ClArr(3)

                        For I = 1 To 9

                            If I <= prn_TOTSIZS Then

                                If Trim(prn_SIZAr(I)) <> "" Then

                                    vCLWDTH = ClArr(I + 3)

                                    If Val(vCLWDTH) <> 0 Then

                                        vCLLFT = Format(Val(vCLLFT) + Val(vCLWDTH), "##########0.00")

                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, I, 2)), Val(vCLLFT) - 10, CurY, 1, 0, pFont)

                                    End If

                                End If

                            End If

                        Next

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

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
        Dim dt1 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single

        Dim Sz_Nm As String = ""
        Dim Sz_Id As Integer = 0
        Dim New_Code As String = ""
        Dim CurX As Single = 0, CurX1 As Single = 0, CurX2 As Single = 0
        Dim Cnt_Value As Single = 0
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Desc As String
        Dim Cmp_PhNo As String, Cmp_GST As String, Cmp_CstNo As String, Cmp_EMail As String, Cmp_CstNo1 As String
        Dim CInc As Integer
        Dim CstDetAr() As String
        Dim vCLWDTH As String
        Dim vCLLFT As String


        PageNo = PageNo + 1

        CurY = TMargin
        prn_DetIndx1 = 0
        da2 = New SqlClient.SqlDataAdapter("select a.* from Garments_Item_PackingSlip_Details a  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_PackingSlip_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        Sz_Id = 0
        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Size_IdNo from Garments_Item_PackingSlip_Details a  INNER JOIN Item_Head b On b.Item_Idno <> 0 and a.Item_Idno = b.Item_Idno LEFT OUTER JOIN Size_Head c ON b.Item_Size_IdNo = c.Size_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_PackingSlip_Code = '" & Trim(EntryCode) & "' Order by B.Item_Size_IdNo", con)
        'da2 = New SqlClient.SqlDataAdapter("select a.* from Garments_Item_PackingSlip_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_PackingSlip_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt1.Clear()
        da2.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            For i = 0 To dt1.Rows.Count - 1
                If Val(dt1.Rows(i).Item("Item_Size_IdNo").ToString) <> 0 And Sz_Id <> dt1.Rows(i).Item("Item_Size_IdNo").ToString Then
                    Cnt_Value = Cnt_Value + 1
                End If
                Sz_Id = dt1.Rows(i).Item("Item_Size_IdNo").ToString
            Next
        End If

        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PACKING SLIP", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString("P", p1Font).Height

        CurY = CurY + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_PanNo = "" : Cmp_CstNo1 = ""
        Cmp_PhNo = "" : Cmp_GST = "" : Cmp_CstNo = "" : Cmp_EMail = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GST = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        '--e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.CompanyLOGO_RD, Drawing.Image), LMargin + 20, CurY, 112, 80)
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_GST), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "     " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = LMargin + 450
            W1 = e.Graphics.MeasureString("P.O.NO  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Item_PackingSlip_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Item_PackingSlip_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            CurY = CurY + TxtHgt - 5
            If Trim(prn_HdDt.Rows(0).Item("Order_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "ORDER NO : " & prn_HdDt.Rows(0).Item("Order_No").ToString & "          " & "       ORDER DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Trim(Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString))) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT : " & Trim(Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString))), PageWidth - 5, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + 10

            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "STYLE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

            vCLLFT = LMargin + ClAr(1) + ClAr(2) + ClAr(3)

            For I = 1 To 9

                If I <= prn_TOTSIZS Then

                    If Trim(prn_SIZAr(I)) <> "" Then

                        vCLWDTH = ClAr(I + 3)

                        If Val(vCLWDTH) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_SIZAr(I)), Val(vCLLFT), CurY, 2, Val(vCLWDTH), pFont)
                            vCLLFT = Format(Val(vCLLFT) + Val(vCLWDTH), "##########0.00")

                        End If

                    End If

                End If

            Next

            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String, New_Code As String = ""
        Dim W1 As Single = 0, Cnt_Value As Single = 0
        Dim CurX As Single = 0, CurX2 As Single = 0, CurX3 As Single = 0, CurX1 As Single = 0
        Dim vCLWDTH As Single
        Dim vCLLFT As Single


        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width
        prn_DetIndx1 = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
                prn_DetIndx = prn_DetIndx + 1
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 10, CurY, 2, ClAr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt - 15
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))

            vCLLFT = LMargin + ClAr(1) + ClAr(2) + ClAr(3)

            For I = 1 To 9

                If I <= prn_TOTSIZS Then

                    If Trim(prn_SIZAr(I)) <> "" Then

                        vCLWDTH = ClAr(I + 3)

                        If Val(vCLWDTH) <> 0 Then

                            e.Graphics.DrawLine(Pens.Black, vCLLFT, CurY, vCLLFT, LnAr(4))
                            vCLLFT = Format(Val(vCLLFT) + Val(vCLWDTH), "##########0.00")

                        End If

                    End If

                End If

            Next

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(4))


            CurY = CurY + TxtHgt - 5

            If Val(prn_HdDt.Rows(0).Item("Transport_Name").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(7) = CurY

            End If

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_ActiveCtrl_Name = dgv_Details.Name
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgv_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then

                    If Trim(UCase(lbl_Invoice_Code.Text)) <> "" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If


                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        If Trim(UCase(lbl_Invoice_Code.Text)) <> "" Then
            e.Handled = True
        End If
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
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

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Itm_Id As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Itm_Id = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Item_PackingSlip_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Item_PackingSlip_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Item_PackingSlip_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ItemName.Text) <> "" Then
                Itm_Id = Common_Procedures.Item_NameToIdNo(con, cbo_Filter_ItemName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Itm_Id) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " c.Item_IdNo = " & Str(Val(Itm_Id))
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.*,c.*, d.Item_Name , f.Size_Name from Garments_Item_PackingSlip_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Garments_Item_PackingSlip_Details c ON c.Item_PackingSlip_Code = a.Item_PackingSlip_Code INNER JOIN Item_Head d ON d.Item_Idno = c.Item_IdNo LEFT OUTER JOIN Size_Head f ON c.Size_Idno = f.Size_Idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_PackingSlip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Item_PackingSlip_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Item_PackingSlip_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Item_PackingSlip_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Item_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Size_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Common_Procedures.Unit_IdNoToName(con, Val(dt2.Rows(i).Item("Unit_IdNo").ToString))
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                    ' dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")


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

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub


    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            '---

        End Try
    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_orderdate, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_Transport.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                'dgv_Details.CurrentCell.Selected = True

            Else
                txt_Note.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then


            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                ' dgv_Details.CurrentCell.Selected = True

            Else
                txt_Note.Focus()

            End If


        End If

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

    Private Sub dgv_OrderSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_OrderSelection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_OrderSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(6).Value = (Val(.Rows(RwIndx).Cells(6).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(6).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(6).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_OrderSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_OrderSelection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_OrderSelection.CurrentCell.RowIndex >= 0 Then

                n = dgv_OrderSelection.CurrentCell.RowIndex

                Select_Piece(n)

                e.Handled = True

            End If
        End If
    End Sub

    Private Sub btn_Close_OrderSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_OrderSelection.Click
        Order_Selection()
    End Sub

    Private Sub Order_Selection()
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt3 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CartonNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Trim(UCase(cbo_Type.Text)) = "ORDER" Then

            dgv_Details.Rows.Clear()

            For i = 0 To dgv_OrderSelection.RowCount - 1

                If Val(dgv_OrderSelection.Rows(i).Cells(6).Value) = 1 Then

                    txt_OrderNo.Text = dgv_OrderSelection.Rows(i).Cells(1).Value
                    txt_orderdate.Text = dgv_OrderSelection.Rows(i).Cells(2).Value

                    cbo_Transport.Text = dgv_OrderSelection.Rows(i).Cells(7).Value
                    'n = dgv_Details.Rows.Add()
                    'dgv_Details.Rows(n).Cells(5).Value = dgv_OrderSelection.Rows(i).Cells(8).Value

                    lbl_Sales_Order_Code.Text = dgv_OrderSelection.Rows(i).Cells(8).Value

                    Da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name from Garments_Item_PackingSlip_Details a, Item_Head b, Size_Head c where a.Item_PackingSlip_Code = '" & Trim(NewCode) & "' and a.Item_IdNo = b.Item_Idno and a.Size_Idno = c.Size_Idno Order by a.Sl_No", con)
                    Da2.Fill(Dt2)
                    dgv_Details.Rows.Clear()
                    SNo = 0

                    If Dt2.Rows.Count > 0 Then

                        For k = 0 To Dt2.Rows.Count - 1
                            n = dgv_Details.Rows.Add()
                            SNo = SNo + 1
                            dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                            dgv_Details.Rows(n).Cells(1).Value = Dt2.Rows(k).Item("Item_Name").ToString
                            dgv_Details.Rows(n).Cells(2).Value = Val(Dt2.Rows(k).Item("Quantity").ToString)
                            dgv_Details.Rows(n).Cells(3).Value = (Dt2.Rows(k).Item("Sales_order_Code").ToString)
                        Next k
                        Dt2.Clear()
                        Dt2.Dispose()
                        Da2.Dispose()
                    Else
                        Da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name from Sales_Order_Details a, Item_Head b, Size_Head c where a.Sales_Order_Code = '" & Trim(dgv_OrderSelection.Rows(i).Cells(8).Value) & "'  and a.Item_IdNo = b.Item_Idno and a.Size_Idno = c.Size_Idno Order by a.Sl_No", con)
                        Dt2 = New DataTable
                        Da2.Fill(Dt2)

                        If Dt2.Rows.Count > 0 Then
                            For j = 0 To Dt2.Rows.Count - 1
                                If (Val(Dt2.Rows(j).Item("Order_Quantity").ToString) - Val(Dt2.Rows(j).Item("Order_Cancel_Quantity").ToString) - Val(Dt2.Rows(j).Item("Invoice_Quantity").ToString)) <> 0 Then
                                    n = dgv_Details.Rows.Add()
                                    SNo = SNo + 1
                                    dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                                    dgv_Details.Rows(n).Cells(1).Value = Dt2.Rows(j).Item("Item_Name").ToString
                                    dgv_Details.Rows(n).Cells(2).Value = Val(Dt2.Rows(j).Item("Order_Quantity").ToString) - Val(Dt2.Rows(j).Item("Order_Cancel_Quantity").ToString) - Val(Dt2.Rows(j).Item("Invoice_Quantity").ToString)
                                    dgv_Details.Rows(n).Cells(3).Value = (Dt2.Rows(j).Item("Sales_order_Code").ToString)
                                End If
                            Next

                            Dt2.Clear()
                            Dt2.Dispose()
                            Da2.Dispose()
                        End If

                    End If

                End If

            Next


        End If

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_OrderSelection.Visible = False
        If dgv_Details.Enabled And dgv_Details.Visible Then
            dgv_Details.Focus()
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                ' dgv_Details.CurrentCell.Selected = True
            End If
        End If
        '  If txt_DcNo.Enabled And txt_DcNo.Visible Then txt_DcNo.Focus()

    End Sub
    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, dtp_Date, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        If Trim(UCase(cbo_Type.Text)) = "DIRECT" Then
            ' dgv_Details.AllowUserToAddRows = True
            txt_orderdate.Enabled = True
            txt_OrderNo.Enabled = True
            cbo_Transport.Enabled = True
        ElseIf Trim(UCase(cbo_Type.Text)) = "ORDER" Then
            ' dgv_Details.AllowUserToAddRows = True
            txt_orderdate.Enabled = False
            txt_OrderNo.Enabled = False
            cbo_Transport.Enabled = False

        End If
    End Sub

    Private Sub cbo_Grid_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ItemName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Item_Head", "Item_Name", "", "(Item_idNo = 0)")

    End Sub

    Private Sub cbo_Grid_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ItemName.KeyDown

        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ItemName, Nothing, Nothing, "Item_Head", "Item_Name", "", "(Item_idNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
                        cbo_Ledger.Focus()
                    Else
                        cbo_Transport.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)

                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_Note.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(2)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ItemName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ItemName, Nothing, "Item_Head", "Item_Name", "", "(Item_idNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_Note.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ItemName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Size.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_Size_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Size.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Size, Nothing, Nothing, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Size.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Size.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Size, Nothing, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub
    Private Sub cbo_Grid_ItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ItemName.TextChanged
        Try
            If cbo_Grid_ItemName.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_ItemName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ItemName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Size_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Size.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Size.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Size_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Size.TextChanged
        Try
            If FrmLdSTS = True Then Exit Sub

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            If cbo_Grid_Size.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Size.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Size.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                ' dgv_Details.CurrentCell.Selected = True

            Else
                If Trim(cbo_Type.Text) = "ORDER" Then
                    cbo_Ledger.Focus()
                Else

                    cbo_Transport.Focus()
                End If


            End If
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If

    End Sub


    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub
    Private Sub cbo_Grid_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Unit.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Unit_Head", "Unit_Name", "", "(unit_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Unit.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Unit, Nothing, Nothing, "Unit_Head", "Unit_Name", "", "(unit_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Unit, Nothing, "Unit_Head", "Unit_Name", "", "(unit_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_Grid_Unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Unit.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Unit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Unit.TextChanged
        Try
            If cbo_Grid_Unit.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Unit.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Unit.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub



End Class