Public Class FinishedProduct_Sales_Return_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FPSRN-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer

    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl

    Private dgv_LevColNo As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_Ledger.Text = ""
        cbo_SalesAccount.Text = ""
        txt_Note.Text = ""
        txt_Billamount.Text = ""
        txt_BillNo.Text = ""
        txt_Accesablevalue.Text = ""
        txt_VatAmount.Text = ""
        txt_VehicleNo.Text = ""
        cbo_vataccount.Text = ""
        cbo_RackNo.Text = ""
        dgv_Details.Rows.Clear()

        Grid_DeSelect()

        cbo_ItemName.Visible = False

        cbo_Unit.Visible = False

        cbo_ItemName.Tag = -1
        cbo_Unit.Tag = -1

        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""

        dgv_Details.Tag = ""
        dgv_LevColNo = -1

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
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
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        End If

        If Me.ActiveControl.Name <> cbo_Unit.Name Then
            cbo_Unit.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_ItemName.Name Then
            cbo_ItemName.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Common_Procedures.Hide_CurrentStock_Display()
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

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name ,C.Ledger_Name as VatAC_Name, D.Ledger_Name as PurAc_Name ,e.Rack_No As Rackno from FinishedProduct_Sales_Return_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head C ON a.VatAc_IdNo = C.Ledger_IdNo LEFT OUTER JOIN Ledger_Head D ON a.PurchaseAc_IdNo = D.Ledger_IdNo  Left Outer Join Rack_Head e On a.Rack_Idno = e.Rack_Idno  Where a.Sales_Return_code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("Sales_Return_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sales_Return_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_SalesAccount.Text = dt1.Rows(0).Item("PurAc_Name").ToString
                txt_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_BillNo.Text = dt1.Rows(0).Item("Party_Bill_No").ToString
                txt_Billamount.Text = dt1.Rows(0).Item("Bill_Amount").ToString
                txt_Accesablevalue.Text = dt1.Rows(0).Item("Accessable_Value").ToString
                txt_VatAmount.Text = dt1.Rows(0).Item("Vat_Amount").ToString
                cbo_vataccount.Text = dt1.Rows(0).Item("VatAc_Name").ToString
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                cbo_RackNo.Text = dt1.Rows(0).Item("Rackno").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*,b. Processed_Item_Name  from FinishedProduct_Sales_Return_Details a INNER JOIN Processed_Item_Head b ON  a.Processed_Item_IdNo = b.Processed_Item_Idno where a.Sales_Return_code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Sales_Qty").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Meter_Qty").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meter").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Unit_Name").ToString
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.000")

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(2).Value = dt1.Rows(0).Item("Total_Qty").ToString
                    .Rows(0).Cells(3).Value = dt1.Rows(0).Item("Total_Mtr_Qty").ToString
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meter").ToString), "########0.00")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.000")
                End With

                Grid_DeSelect()


                dt2.Clear()


                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

    End Sub

    Private Sub FinishedProduct_Sales_Return_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FINISHEDPRODUCT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RackNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "RACK" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_RackNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_vataccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_vataccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesAccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SalesAccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub FinishedProduct_Sales_Return_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        cbo_ItemName.Visible = False
        cbo_Unit.Visible = False

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAccount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vataccount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RackNo.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_filter_billNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Accesablevalue.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Billamount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VatAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VehicleNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAccount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vataccount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RackNo.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_filter_billNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Accesablevalue.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Billamount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VatAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VehicleNo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Accesablevalue.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Billamount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_VatAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_filter_billNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Accesablevalue.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Billamount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_VatAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_filter_billNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

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

    Private Sub FinishedProduct_Sales_Return_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
        Common_Procedures.Hide_CurrentStock_Display()
    End Sub

    Private Sub FinishedProduct_Sales_Return_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_BillNo.Focus()

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
                                txt_VehicleNo.Focus()

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.FP_Sales_return_Entry, New_Entry, Me, con, "FinishedProduct_Sales_Return_Head", "Sales_Return_Code", NewCode, "Sales_Return_Date", "(Sales_Return_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub







        
        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sales_Return_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sales_Return_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " & _
                                    " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from FinishedProduct_Sales_Return_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            cmd.CommandText = "delete from FinishedProduct_Sales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then

                If Common_Procedures.Check_is_Negative_Stock_Status(con, trans) = True Then Exit Sub

            End If

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'FP' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
            da.Fill(dt2)
            cbo_Filter_ItemName.DataSource = dt2
            cbo_Filter_ItemName.DisplayMember = "Processed_Item_Name"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            txt_filter_billNo.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
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
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_Return_No from FinishedProduct_Sales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_Return_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_Return_No from FinishedProduct_Sales_Return_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_Return_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_Return_No from FinishedProduct_Sales_Return_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_Return_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Sales_Return_No from FinishedProduct_Sales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_Return_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "FinishedProduct_Sales_Return_Head", "Sales_Return_code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from FinishedProduct_Sales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_Return_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                    If dt1.Rows(0).Item("Sales_Return_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Sales_Return_Date").ToString
                End If
            End If
            dt1.Clear()
            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

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

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sales_Return_No from FinishedProduct_Sales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.FP_Sales_Return_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sales_Return_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.FP_Sales_return_Entry, New_Entry, Me) = False Then Exit Sub




       
        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW RETURN INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sales_Return_No from FinishedProduct_Sales_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW RETURN...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RETURN...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Itfp_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vtotqty As Single
        Dim SalAc_ID As Integer = 0
        Dim Lot_ID As Integer = 0
        Dim vTotAmt As Single, vTotMrQty As Single
        Dim rack_id As Integer = 0

        Dim itgry_id As Integer = 0, vatac_id As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Sales_Return_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.FP_Sales_return_Entry, New_Entry, Me, con, "FinishedProduct_Sales_Return_Head", "Sales_Return_Code", NewCode, "Sales_Return_Date", "(Sales_Return_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Sales_Return_No desc", dtp_Date.Value.Date) = False Then Exit Sub



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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        SalAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAccount.Text)
        If SalAc_ID = 0 And Val(txt_Billamount.Text) <> 0 Then
            MessageBox.Show("Invalid Sales A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalesAccount.Enabled And cbo_SalesAccount.Visible Then cbo_SalesAccount.Focus()
            Exit Sub
        End If

        rack_id = Common_Procedures.Rack_NoToIdNo(con, cbo_RackNo.Text)

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)

                        End If
                        Exit Sub
                    End If

                End If

            Next
        End With

        vatac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_vataccount.Text)
        If vatac_id = 0 And Val(txt_VatAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Vat A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_vataccount.Enabled Then cbo_vataccount.Focus()
            Exit Sub
        End If

        Total_Calculation()

        vTotMtrs = 0 : vTotAmt = 0 : vTotPcs = 0 : vtotqty = 0

        If dgv_Details_Total.RowCount > 0 Then
            vtotqty = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
            vTotMrQty = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If

        tr = con.BeginTransaction

        'Try

        If Insert_Entry = True Or New_Entry = False Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Else

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "FinishedProduct_Sales_Return_Head", "Sales_Return_code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        End If

        cmd.Connection = con
        cmd.Transaction = tr

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@SalesDate", dtp_Date.Value.Date)

        cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
        cmd.ExecuteNonQuery()

        If New_Entry = True Then

            cmd.CommandText = "Insert into FinishedProduct_Sales_Return_Head(Sales_Return_code, Company_IdNo, Sales_Return_No, for_OrderBy, Sales_Return_Date, Ledger_IdNo, PurchaseAc_Idno, Vehicle_No,Party_Bill_No, Bill_Amount , Accessable_Value , VatAc_Idno ,Vat_Amount,  Note,Total_Qty,Total_Mtr_Qty, Total_Meter,Net_Amount , Rack_Idno ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @SalesDate, " & Str(Val(Led_ID)) & "," & Val(SalAc_ID) & " ,'" & Trim(txt_VehicleNo.Text) & "', '" & Trim(txt_BillNo.Text) & "' , " & Val(txt_Billamount.Text) & " ," & Val(txt_Accesablevalue.Text) & "," & Val(vatac_id) & " , " & Val(txt_VatAmount.Text) & ",  '" & Trim(txt_Note.Text) & "'," & Str(Val(vtotqty)) & " ," & Val(vTotMrQty) & ", " & Str(Val(vTotMtrs)) & ", " & Str(Val(vTotAmt)) & " , " & Str(Val(rack_id)) & ")"
            cmd.ExecuteNonQuery()

        Else

            cmd.CommandText = "Update FinishedProduct_Sales_Return_Head set Sales_Return_Date = @SalesDate, Ledger_IdNo = " & Val(Led_ID) & ", PurchaseAc_Idno = " & Val(SalAc_ID) & " ,Vehicle_No = '" & Trim(txt_VehicleNo.Text) & "',Party_Bill_No = '" & Trim(txt_BillNo.Text) & "',Bill_Amount = " & Val(txt_Billamount.Text) & " , Accessable_Value = " & Val(txt_Accesablevalue.Text) & ",VatAc_Idno = " & Val(vatac_id) & " , Vat_Amount = " & Val(txt_VatAmount.Text) & ",  Note = '" & Trim(txt_Note.Text) & "',Total_Qty = " & Val(vtotqty) & ", Total_Mtr_Qty = " & Val(vTotMrQty) & ", Total_Meter = " & Val(vTotMtrs) & ",Net_Amount = " & Val(vTotAmt) & " , Rack_Idno = " & Str(Val(rack_id)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " & _
                                  " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

        End If

        cmd.CommandText = "Delete from FinishedProduct_Sales_Return_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_code = '" & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        Partcls = "Sale.Ret : Inv.No. " & Trim(txt_BillNo.Text)
        PBlNo = Trim(txt_BillNo.Text)
        EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)

        With dgv_Details
            Sno = 0
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    Itfp_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                    Sno = Sno + 1

                    cmd.CommandText = "Insert into FinishedProduct_Sales_Return_Details(Sales_Return_code, Company_IdNo, Sales_Return_No, for_OrderBy, Sales_Return_Date,Sl_No,Processed_Item_Idno,Sales_Qty,Meter_Qty,Meter,Unit_Name,Rate,Amount ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @SalesDate," & Str(Val(Sno)) & ", " & Str(Val(Itfp_ID)) & ",  " & Val(.Rows(i).Cells(2).Value) & "," & Val(.Rows(i).Cells(3).Value) & ", " & Val(.Rows(i).Cells(4).Value) & ", '" & Trim(.Rows(i).Cells(5).Value) & "', " & Str(Val(.Rows(i).Cells(6).Value)) & " ," & Str(Val(.Rows(i).Cells(7).Value)) & ")"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into Stock_Item_Processing_Details (     Reference_Code               ,            Company_IdNo          ,            Reference_No       ,                               For_OrderBy                              ,  Reference_Date , DeliveryTo_StockIdNo,                                 ReceivedFrom_StockIdNo    ,    Delivery_PartyIdNo   , Received_PartyIdNo,        Entry_ID      ,    Party_Bill_No     ,     Particulars        ,          SL_No        ,         Item_IdNo        , Rack_IdNo                      ,                       Quantity                     ,                       Meters                        ) " & _
                                                              " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @SalesDate  ,       " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "     , 0  ,    0   ,      " & Str(Val(Led_ID)) & "           , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',  " & Str(Val(Sno)) & ", " & Str(Val(Itfp_ID)) & ",   " & Str(Val(rack_id)) & "    , " & Str(Math.Abs(Val(.Rows(i).Cells(2).Value))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(4).Value))) & " ) "
                    cmd.ExecuteNonQuery()

                End If

            Next

        End With


        If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then
            cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno           , Item_IdNo, Rack_IdNo ) " & _
                                    " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_StockIdNo, Item_IdNo,     0        from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

        End If

        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
        vLed_IdNos = Led_ID & "|" & SalAc_ID & "|" & vatac_id
        vVou_Amts = Val(txt_Billamount.Text) & "|" & -1 * (Val(txt_Billamount.Text) - Val(txt_VatAmount.Text)) & "|" & -1 * Val(txt_VatAmount.Text)

        If Common_Procedures.Voucher_Updation(con, "Sales.Ret", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Inv No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
            Throw New ApplicationException(ErrMsg)
        End If

        Dim VouBil As String = ""
        VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Value.Date, Led_ID, Trim(lbl_RefNo.Text), 0, Val(txt_Billamount.Text), "CR", Trim(Pk_Condition) & Trim(NewCode), tr)
        If Trim(UCase(VouBil)) = "ERROR" Then
            Throw New ApplicationException("Error on Voucher Bill Posting")
        End If


        tr.Commit()

        Dt1.Dispose()
        Da.Dispose()

        If New_Entry = True Then
            new_record()
        Else
            move_record(lbl_RefNo.Text)
        End If

        MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)


        'Catch ex As Exception
        '    tr.Rollback()
        '    MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()



    End Sub

    Private Sub Total_Calculation()
        Dim vTotPcs As Single, vTotMtrs As Single, vtotAmt As Single, vtotqty As Single, vtotMrqty

        Dim i As Integer
        Dim sno As Integer

        If FrmLdSTS = True Then Exit Sub

        vTotPcs = 0 : vTotMtrs = 0 : vtotAmt = 0 : sno = 0 : vtotMrqty = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then
                    '.Rows(i).Cells(9).Value = Val(dgv_Details.Rows(i).Cells(7).Value) * Val(dgv_Details.Rows(i).Cells(8).Value)

                    vtotqty = vtotqty + Val(dgv_Details.Rows(i).Cells(2).Value)
                    vtotMrqty = vtotMrqty + Val(dgv_Details.Rows(i).Cells(3).Value)
                    vTotMtrs = vTotMtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                    vtotAmt = vtotAmt + Val(dgv_Details.Rows(i).Cells(7).Value)
                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()

        dgv_Details_Total.Rows(0).Cells(2).Value = Val(vtotqty)
        dgv_Details_Total.Rows(0).Cells(4).Value = Format(Val(vTotMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(7).Value = Format(Val(vtotAmt), "#########0.00")
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")
    End Sub



    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, cbo_SalesAccount, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")


    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_SalesAccount, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub dgv_Details_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellClick
        With dgv_Details
            If e.ColumnIndex = 2 Then
                Show_Item_CurrentStock(e.RowIndex)
                Me.Activate()
                .Focus()
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        With dgv_Details

            If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If

            Total_Calculation()

        End With
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_Details
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If



            If e.ColumnIndex = 1 Then

                If cbo_ItemName.Visible = False Or Val(cbo_ItemName.Tag) <> e.RowIndex Then

                    cbo_ItemName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Type = 'FP ' order by Processed_item_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_ItemName.DataSource = Dt1
                    cbo_ItemName.DisplayMember = "Processed_Item_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_ItemName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_ItemName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_ItemName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_ItemName.Height = rect.Height  ' rect.Height
                    cbo_ItemName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_ItemName.Tag = Val(e.RowIndex)
                    cbo_ItemName.Visible = True

                    cbo_ItemName.BringToFront()
                    cbo_ItemName.Focus()



                End If
            Else

                cbo_ItemName.Visible = False

            End If

            If e.ColumnIndex = 5 Then

                If cbo_Unit.Visible = False Or Val(cbo_Unit.Tag) <> e.RowIndex Then

                    cbo_Unit.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Unit_Name from Unit_Head order by Unit_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Unit.DataSource = Dt3
                    cbo_Unit.DisplayMember = "Unit_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Unit.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Unit.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Unit.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Unit.Height = rect.Height  ' rect.Height

                    cbo_Unit.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Unit.Tag = Val(e.RowIndex)
                    cbo_Unit.Visible = True

                    cbo_Unit.BringToFront()
                    cbo_Unit.Focus()


                End If

            Else

                cbo_Unit.Visible = False

            End If

            If e.ColumnIndex = 2 And dgv_LevColNo <> 2 Then
                Show_Item_CurrentStock(e.RowIndex)
                Me.Activate()
                .Focus()
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            dgv_LevColNo = .CurrentCell.ColumnIndex
            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim i As Integer
        Dim q As Single = 0

        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_Details

                If .Visible Then
                    For i = 0 To .Rows.Count - 1
                        If e.ColumnIndex = 2 Or e.ColumnIndex = 3 Or e.ColumnIndex = 4 Or e.ColumnIndex = 6 Then

                            If e.ColumnIndex = 2 Or e.ColumnIndex = 3 Then
                                .Rows(i).Cells(4).Value = Val(dgv_Details.Rows(i).Cells(2).Value) * Val(dgv_Details.Rows(i).Cells(3).Value)
                            End If

                            If e.ColumnIndex = 2 Or e.ColumnIndex = 4 Or e.ColumnIndex = 6 Then
                                If InStr(1, Trim(UCase(.CurrentRow.Cells(5).Value)), "MTR") > 0 Or InStr(1, Trim(UCase(.CurrentRow.Cells(5).Value)), "METER") > 0 Or InStr(1, Trim(UCase(.CurrentRow.Cells(5).Value)), "METRE") > 0 Then
                                    q = Val(.CurrentRow.Cells(4).Value)
                                Else
                                    q = Val(.CurrentRow.Cells(2).Value)
                                End If
                                .CurrentRow.Cells(7).Value = Format(Val(q) * Val(.CurrentRow.Cells(6).Value), "#########0.00")
                            End If

                        End If

                        Total_Calculation()

                    Next i
                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress
        On Error Resume Next
        With dgv_Details
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True

                End If
            End If
        End With
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

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Unit_Head", "Unit_Name", "", "(Unit_Idno=0)")
    End Sub

    Private Sub cbo_unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, Nothing, Nothing, "Unit_Head", "Unit_Name", "", "(Unit_Idno=0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, Nothing, "Unit_Head", "Unit_Name", "", "(Unit_Idno=0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_unit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.TextChanged
        Try
            If cbo_Unit.Visible Then
                With dgv_Details
                    If Trim(cbo_Unit.Tag) = Trim(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 5 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Unit.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")
    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, Nothing, Nothing, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    txt_VehicleNo.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                End If
            End If

            If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_BillNo.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End If

        End With
    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Mtr_Qty As String
        Dim Unt_nm As String
        Dim Rate As String
        Dim Itm_idno As Integer = 0


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, Nothing, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details

                If Val(.Rows(.CurrentRow.Index).Cells(3).Value) = 0 Or Trim(.Rows(.CurrentRow.Index).Cells(5).Value) = "" Or Val(.Rows(.CurrentRow.Index).Cells(6).Value) = 0 Then

                    Itm_idno = Common_Procedures.Processed_Item_NameToIdNo(con, Trim(cbo_ItemName.Text))

                    da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from Processed_Item_Head a left outer join unit_head b on a.unit_idno = b.unit_idno Where a.Processed_Item_IdNo = " & Str(Val(Itm_idno)), con)
                    dt = New DataTable
                    da.Fill(dt)

                    Rate = 0
                    Mtr_Qty = 0
                    Unt_nm = ""
                    If dt.Rows.Count > 0 Then
                        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                            Mtr_Qty = Val(dt.Rows(0).Item("Meter_Qty").ToString)
                            Unt_nm = Trim(dt.Rows(0).Item("unit_name").ToString)
                            Rate = Val(dt.Rows(0).Item("Sales_Rate").ToString)
                        End If
                    End If

                    dt.Dispose()
                    da.Dispose()

                    If Val(Mtr_Qty) <> 0 Then .Rows(.CurrentRow.Index).Cells(3).Value = Format(Val(Mtr_Qty), "#########0.00")
                    .Rows(dgv_Details.CurrentRow.Index).Cells(5).Value = Trim(Unt_nm)
                    If Val(Rate) <> 0 Then .Rows(.CurrentRow.Index).Cells(6).Value = Format(Val(Rate), "#########0.00")

                End If


                If (.CurrentCell.RowIndex = .Rows.Count - 1) And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_BillNo.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End With

        End If


    End Sub

    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New FinishedProduct_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.TextChanged
        Try
            If cbo_ItemName.Visible Then
                With dgv_Details
                    If Val(cbo_ItemName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_ItemName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_vataccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_vataccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_idno = 0 or AccountsGroup_IdNo = 12 and Verified_Status = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_vataccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vataccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vataccount, txt_Accesablevalue, txt_VatAmount, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_idno = 0 or AccountsGroup_IdNo = 12 and Verified_Status = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_vataccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vataccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vataccount, txt_VatAmount, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_idno = 0 or AccountsGroup_IdNo = 12 and Verified_Status = 1) ", "(Ledger_idno = 0)")

    End Sub


    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Proc_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            Proc_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sales_Return_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Sales_Return_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sales_Return_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ItemName.Text) <> "" Then
                Proc_IdNo = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_Filter_ItemName.Text)
            End If

            If Val(Proc_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sales_Return_Code IN (select z1.Sales_Return_Code from FinishedProduct_Sales_Return_Details z1 where z1.Processed_Item_Idno = " & Str(Val(Proc_IdNo)) & ")"
            End If


            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_Idno = " & Str(Val(Led_IdNo))
            End If

            If Trim(txt_filter_billNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Party_Bill_No = '" & Trim(txt_filter_billNo.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.*, d.Processed_Item_Name from FinishedProduct_Sales_Return_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN FinishedProduct_Sales_Return_Details c ON a.Sales_Return_Code = c.Sales_Return_Code LEFT OUTER JOIN Processed_Item_Head d ON c.Processed_Item_Idno = d.Processed_Item_Idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Return_code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_Return_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,C.*,d.Processed_Item_Name from FinishedProduct_Sales_Return_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN FinishedProduct_Sales_Return_Details c ON a.Sales_Return_Code = c.Sales_Return_Code LEFT OUTER JOIN Processed_Item_Head d ON c.Processed_Item_Idno = d.Processed_Item_Idno LEFT OUTER JOIN Rack_Head f ON b.Rack_Idno = f.Rack_Idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Return_code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_Return_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Sales_Return_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sales_Return_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Party_Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Meter").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")
                    'dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, txt_filter_billNo, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")


    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ItemName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")
    End Sub


    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, btn_Filter_Show, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, btn_Filter_Show, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

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

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record
        '    Dim da1 As New SqlClient.SqlDataAdapter
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.FP_Sales_return_Entry, New_Entry) = False Then Exit Sub

        '    Dim dt1 As New DataTable
        '    Dim NewCode As String

        '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '    Try

        '        da1 = New SqlClient.SqlDataAdapter("select * from FinishedProduct_Sales_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Return_code = '" & Trim(NewCode) & "'", con)
        '        dt1 = New DataTable
        '        da1.Fill(dt1)

        '        If dt1.Rows.Count <= 0 Then

        '            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub

        '        End If


        '        dt1.Dispose()
        '        da1.Dispose()

        '    Catch ex As Exception
        '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '    End Try

        '    If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
        '        Try
        '            PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
        '            If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '                PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        '                PrintDocument1.Print()
        '            End If

        '        Catch ex As Exception
        '            MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '        End Try


        '    Else
        '        Try

        '            Dim ppd As New PrintPreviewDialog

        '            ppd.Document = PrintDocument1

        '            ppd.WindowState = FormWindowState.Normal
        '            ppd.StartPosition = FormStartPosition.CenterScreen
        '            ppd.ClientSize = New Size(600, 600)

        '            ppd.ShowDialog()

        '        Catch ex As Exception
        '            MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

        '        End Try

        '    End If

    End Sub




    'Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim NewCode As String
    '    Dim i As Integer


    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    prn_HdDt.Clear()
    '    prn_DetDt.Clear()
    '    prn_DetIndx = 0
    '    prn_DetSNo = 0
    '    prn_PageNo = 0

    '    Try

    '        da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,d.Ledger_Name as Transport_Name from FinishedProduct_Sales_Return_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Return_code = '" & Trim(NewCode) & "'", con)
    '        prn_HdDt = New DataTable
    '        da1.Fill(prn_HdDt)

    '        If prn_HdDt.Rows.Count > 0 Then
    '            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,e.Process_Name  from FinishedProduct_Sales_Return_Details a INNER JOIN Processed_Item_Head b on a.Item_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Return_code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
    '            prn_DetDt = New DataTable
    '            da2.Fill(prn_DetDt)


    '        Else
    '            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End If

    '        da1.Dispose()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
    '    If prn_HdDt.Rows.Count <= 0 Then Exit Sub
    '    'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
    '    Printing_Format1(e)
    '    'End If
    'End Sub

    'Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim EntryCode As String
    '    Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
    '    Dim pFont As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim CurY As Single, TxtHgt As Single
    '    Dim LnAr(15) As Single, ClArr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False
    '    Dim W1 As Single = 0
    '    Dim SNo As Integer
    '    'PrintDocument pd = new PrintDocument();
    '    'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
    '    'pd.Print();

    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '        Debug.Print(ps.PaperName)
    '        If ps.Width = 800 And ps.Height = 600 Then
    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
    '            e.PageSettings.PaperSize = ps
    '            PpSzSTS = True
    '            Exit For
    '        End If
    '    Next

    '    If PpSzSTS = False Then
    '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
    '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '                PrintDocument1.DefaultPageSettings.PaperSize = ps
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
    '                    e.PageSettings.PaperSize = ps
    '                    Exit For
    '                End If
    '            Next
    '        End If

    '    End If

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 30
    '        .Right = 30
    '        .Top = 30
    '        .Bottom = 30
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    pFont = New Font("Calibri", 11, FontStyle.Regular)

    '    e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

    '    With PrintDocument1.DefaultPageSettings.PaperSize
    '        PrintWidth = .Width - RMargin - LMargin
    '        PrintHeight = .Height - TMargin - BMargin
    '        PageWidth = .Width - RMargin
    '        PageHeight = .Height - BMargin
    '    End With
    '    If PrintDocument1.DefaultPageSettings.Landscape = True Then
    '        With PrintDocument1.DefaultPageSettings.PaperSize
    '            PrintWidth = .Height - TMargin - BMargin
    '            PrintHeight = .Width - RMargin - LMargin
    '            PageWidth = .Height - TMargin
    '            PageHeight = .Width - RMargin
    '        End With
    '    End If

    '    NoofItems_PerPage = 8 ' 6

    '    Erase LnAr
    '    Erase ClArr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClArr(1) = Val(35) : ClArr(2) = 160 : ClArr(3) = 100 : ClArr(4) = 100 : ClArr(5) = 120 : ClArr(6) = 70 : ClArr(7) = 80
    '    ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

    '    TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

    '    EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try

    '        If prn_HdDt.Rows.Count > 0 Then

    '            Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


    '            W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

    '            NoofDets = 0

    '            CurY = CurY - 10

    '            If prn_DetDt.Rows.Count > 0 Then

    '                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                    If NoofDets >= NoofItems_PerPage Then
    '                        CurY = CurY + TxtHgt

    '                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

    '                        NoofDets = NoofDets + 1

    '                        Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

    '                        e.HasMorePages = True
    '                        Return

    '                    End If

    '                    prn_DetSNo = prn_DetSNo + 1

    '                    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Grey_Name").ToString)
    '                    ItmNm2 = ""
    '                    If Len(ItmNm1) > 18 Then
    '                        For I = 18 To 1 Step -1
    '                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                        Next I
    '                        If I = 0 Then I = 18
    '                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
    '                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                    End If

    '                    CurY = CurY + TxtHgt
    '                    SNo = SNo + 1
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Colour_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Lot_No").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Process_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
    '                    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Pcs").ToString) <> 0 Then
    '                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
    '                    End If
    '                    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Meters").ToString) <> 0 Then
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Meters").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
    '                    End If
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)


    '                    NoofDets = NoofDets + 1

    '                    If Trim(ItmNm2) <> "" Then
    '                        CurY = CurY + TxtHgt - 5
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
    '                        NoofDets = NoofDets + 1
    '                    End If

    '                    prn_DetIndx = prn_DetIndx + 1

    '                Loop

    '            End If


    '            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


    '        End If

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    '    e.HasMorePages = False

    'End Sub

    'Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim p1Font As Font
    '    Dim strHeight As Single
    '    Dim C1 As Single, W1 As Single, S1 As Single
    '    Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

    '    PageNo = PageNo + 1

    '    CurY = TMargin

    '    da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,e.Process_Name  from FinishedProduct_Sales_Return_Details a INNER JOIN Processed_Item_Head b on a.Item_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Return_code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
    '    da2.Fill(dt2)

    '    If dt2.Rows.Count > NoofItems_PerPage Then
    '        Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    End If
    '    dt2.Clear()

    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY

    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

    '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
    '    End If

    '    CurY = CurY + TxtHgt - 10
    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

    '    CurY = CurY + strHeight - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

    '    CurY = CurY + TxtHgt - 10
    '    p1Font = New Font("Calibri", 16, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "PROCESSING DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



    '    CurY = CurY + strHeight  ' + 150
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    Try
    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
    '        W1 = e.Graphics.MeasureString("P.O.NO  : ", pFont).Width
    '        S1 = e.Graphics.MeasureString("TO :    ", pFont).Width

    '        CurY = CurY + TxtHgt - 10
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Return_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        p1Font = New Font("Calibri", 14, FontStyle.Bold)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Return_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "P.O.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
    '        End If
    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(3) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

    '        CurY = CurY + TxtHgt - 10
    '        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PROCESSING", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(4) = CurY

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    '    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '        Dim p1Font As Font
    '        Dim I As Integer
    '        Dim Cmp_Name As String
    '        Dim W1 As Single = 0

    '        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

    '        Try

    '            For I = NoofDets + 1 To NoofItems_PerPage

    '                CurY = CurY + TxtHgt



    '                prn_DetIndx = prn_DetIndx + 1

    '            Next

    '            CurY = CurY + TxtHgt
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(5) = CurY

    '            CurY = CurY + TxtHgt - 10
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 2, ClAr(4), pFont)
    '            End If

    '            If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
    '                If is_LastPage = True Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '                End If
    '            End If
    '            If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
    '                If is_LastPage = True Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '                End If
    '            End If
    '            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
    '                If is_LastPage = True Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
    '                End If
    '            End If


    '            CurY = CurY + TxtHgt - 15

    '            CurY = CurY + TxtHgt
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(6) = CurY

    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))

    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

    '            CurY = CurY + TxtHgt - 5

    '            Common_Procedures.Print_To_PrintDocument(e, "Transport : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + 10, CurY, 0, 0, pFont)
    '            'If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
    '            '    Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 250, CurY, 0, 0, pFont)
    '            'End If

    '            CurY = CurY + TxtHgt + 10
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(7) = CurY

    '            CurY = CurY + TxtHgt
    '            If Val(Common_Procedures.User.IdNo) <> 1 Then
    '                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
    '            End If


    '            CurY = CurY + TxtHgt
    '            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
    '            p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

    '            CurY = CurY + TxtHgt + 5

    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    '        Catch ex As Exception

    '            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End Try

    '    End Sub




    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub


    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub cbo_SalesAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or AccountsGroup_IdNo = 28 and Verified_Status = 1 )", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_Purchaseaccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAccount, cbo_Ledger, cbo_RackNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or AccountsGroup_IdNo = 28 and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Purchaseaccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAccount, cbo_RackNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or AccountsGroup_IdNo = 28 and Verified_Status = 1) ", "(Ledger_idno = 0)")

    End Sub
    Private Sub txt_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VehicleNo.KeyDown
        If e.KeyCode = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BillNo.KeyDown

        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VehicleNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_VatAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VatAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub



    Private Sub txt_Billamount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Billamount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Accesablevalue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Accesablevalue.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub


    Private Sub Show_Item_CurrentStock(ByVal Rw As Integer)
        Dim vItemID As Integer

        If Val(Rw) < 0 Then Exit Sub

        With dgv_Details

            vItemID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(Rw).Cells(1).Value)

            If Val(vItemID) = 0 Then Exit Sub

            If Val(vItemID) <> Val(.Tag) Then
                Common_Procedures.Show_ProcessedItem_CurrentStock_Display(con, Val(lbl_Company.Tag), Val(Common_Procedures.CommonLedger.Godown_Ac), vItemID)
                .Tag = Val(Rw)
            End If

        End With


    End Sub

    Private Sub cbo_PurchaseAccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_vataccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vataccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_vataccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_RackNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RackNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Rack_Head", "Rack_No", "(Close_Status = 0)", "(Rack_IdNo = 0)")

    End Sub

    Private Sub cbo_RackNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RackNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RackNo, cbo_SalesAccount, txt_VehicleNo, "Rack_Head", "Rack_No", "(Close_Status = 0)", "(Rack_IdNo = 0)")

    End Sub

    Private Sub cbo_RackNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RackNo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RackNo, txt_VehicleNo, "Rack_Head", "Rack_No", "(Close_Status = 0)", "(Rack_IdNo = 0)")

    End Sub
    Private Sub cbo_RackNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RackNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New RackNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RackNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
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

End Class