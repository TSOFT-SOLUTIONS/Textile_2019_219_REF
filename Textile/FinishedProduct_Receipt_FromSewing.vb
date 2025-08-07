Imports System.Runtime.Remoting

Public Class FinishedProduct_Receipt_FromSewing
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FBREC-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private dgv_ActiveCtrl_Name As String

    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Public Shared EntFnYrCode As String
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1
        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_Ledger.Text = ""
        txt_PartyDcNo.Text = ""
        cbo_Vechile.Text = ""
        txt_Remarks.Text = ""

        cbo_ClothSales_OrderCode_forSelection.Text = ""


        'cbo_Ledger.Enabled = True
        'cbo_Ledger.BackColor = Color.White

        'cbo_Colour.Enabled = True
        'cbo_Colour.BackColor = Color.White

        'cbo_FpName.Enabled = True
        'cbo_FpName.BackColor = Color.White

        'cbo_Fabric.Enabled = True
        'cbo_Fabric.BackColor = Color.White

        'cbo_Size.Enabled = True
        'cbo_Size.BackColor = Color.White

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()

        Grid_DeSelect()




    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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



        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If



        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Fabric_Receipt_Sewing_Head a  Where a.Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RecNo.Text = dt1.Rows(0).Item("Fabric_Receipt_Sewing_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Fabric_Receipt_Sewing_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Sewing_IdNo").ToString))
                ' lbl_JobNo.Text = dt1.Rows(0).Item("JobCard_Sewing_No").ToString
                txt_PartyDcNo.Text = dt1.Rows(0).Item("Party_Dc_No").ToString
                If Val(dt1.Rows(0).Item("Lot_Complete_Status").ToString) = 1 Then
                    chk_LotComplete.Checked = True
                Else
                    chk_LotComplete.Checked = False
                End If
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                cbo_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString
                cbo_Vechile.Text = dt1.Rows(0).Item("vehicle_No").ToString
                txt_Remarks.Text = Trim(dt1.Rows(0).Item("remarks").ToString)


                da2 = New SqlClient.SqlDataAdapter("select a.*,C.Processed_Item_Name as Fp_Name,d.Colour_Name,e.Size_Name,f.Cloth_Name as Fabric_Name,g.Process_Name from Fabric_Receipt_Sewing_Details a INNER JOIN Processed_Item_Head c ON  c.Processed_Item_IdNo = a.FinishedProduct_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Size_Head e ON e.Size_IdNo = a.Size_IdNo LEFT OUTER JOIN Cloth_Head f ON f.Cloth_idNo = a.Fabric_Idno LEFT OUTER JOIN Process_Head g ON g.Process_idNo = a.Process_Idno where a.Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Fp_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Size_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                        dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Fabric_Name").ToString
                        dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Process_Name").ToString
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Consum_Meter_Pcs").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Consum_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Fabric_Delivery_Sewing_Code").ToString
                        dgv_Details.Rows(n).Cells(10).Value = Val(dt2.Rows(i).Item("Fabric_Delivery_Sewing_Details_SlNo").ToString)
                        dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Consum_weight_per_pcs").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Consum_weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("ExcSht_Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("ExcSht_Quantity").ToString), "########0")

                        If Val(dgv_Details.Rows(n).Cells(7).Value) = 0 Then dgv_Details.Rows(n).Cells(7).Value = ""
                        If Val(dgv_Details.Rows(n).Cells(8).Value) = 0 Then dgv_Details.Rows(n).Cells(8).Value = ""
                        If Val(dgv_Details.Rows(n).Cells(11).Value) = 0 Then dgv_Details.Rows(n).Cells(11).Value = ""
                        If Val(dgv_Details.Rows(n).Cells(12).Value) = 0 Then dgv_Details.Rows(n).Cells(12).Value = ""
                        If Val(dgv_Details.Rows(n).Cells(13).Value) = 0 Then dgv_Details.Rows(n).Cells(13).Value = ""
                        If Val(dgv_Details.Rows(n).Cells(14).Value) = 0 Then dgv_Details.Rows(n).Cells(14).Value = ""

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Consum_Meters").ToString), "########0.00")
                    .Rows(0).Cells(12).Value = Format(Val(dt1.Rows(0).Item("Total_Consum_Weight").ToString), "#########0.000")
                    .Rows(0).Cells(13).Value = Format(Val(dt1.Rows(0).Item("Total_ExcSht_Weight").ToString), "#########0.000")
                    .Rows(0).Cells(14).Value = Format(Val(dt1.Rows(0).Item("Total_ExcSht_Weight").ToString), "#########0")
                End With

                Grid_DeSelect()

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()


            'If LockSTS = True Then

            '    cbo_Ledger.Enabled = False
            '    cbo_Ledger.BackColor = Color.LightGray


            '    cbo_Colour.Enabled = False
            '    cbo_Colour.BackColor = Color.LightGray

            '    cbo_FpName.Enabled = False
            '    cbo_FpName.BackColor = Color.LightGray


            '    cbo_Fabric.Enabled = False
            '    cbo_Fabric.BackColor = Color.LightGray

            '    cbo_Size.Enabled = False
            '    cbo_Size.BackColor = Color.LightGray

            'End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub FinishedProduct_Receipt_FromSewing_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


        FrmLdSTS = False

    End Sub

    Private Sub FinishedProduct_Receipt_FromSewing_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            cbo_ClothSales_OrderCode_forSelection.Visible = True
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = True

            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)

        Else

            cbo_ClothSales_OrderCode_forSelection.Visible = False
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = False

        End If

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Colour.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_FpName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Colour.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Filter_FpName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub FinishedProduct_Receipt_FromSewing_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub FinishedProduct_Receipt_FromSewing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                Else
                    If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub

                    Else
                        Me.Close()
                    End If
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView



        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

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

                    Dim vLASTCOL As Integer

                    vLASTCOL = 14
                    'vLASTCOL = 4

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= vLASTCOL Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                '    save_record()
                                'Else
                                '    msk_Date.Focus()
                                'End If
                                txt_Remarks.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(4)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 4 Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(13)

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 4 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                                    cbo_ClothSales_OrderCode_forSelection.Focus()
                                Else
                                    txt_PartyDcNo.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(vLASTCOL)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 13 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(4)
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
        Dim Qa As Windows.Forms.DialogResult

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_FinishedProduct_ReceiptFrom_Sewing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_FinishedProduct_ReceiptFrom_Sewing, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(EntFnYrCode)

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(EntFnYrCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Update Fabric_Delivery_Sewing_Details set Receipt_Meters = a.Receipt_Meters - (b.Consum_Meters) ,  Receipt_Quantity = a.Receipt_Quantity - (b.Quantity-b.ExcSht_Quantity) ,  Receipt_Weight = a.Receipt_Weight - (b.Consum_weight - b.ExcSht_Weight)  from Fabric_Delivery_Sewing_Details a, Fabric_Receipt_Sewing_Details b Where b.Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "' and a.Fabric_Delivery_Sewing_code = b.Fabric_Delivery_Sewing_code and a.Fabric_Delivery_Sewing_details_SlNo = b.Fabric_Delivery_Sewing_Details_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Fabric_Delivery_Sewing_Details set Lot_Complete_Status = 0 ,  Lot_Complete_FabricReceipt_Sewing_Code = '' Where Lot_Complete_FabricReceipt_Sewing_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Fabric_Receipt_Sewing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Fabric_Receipt_Sewing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "'"
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


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_FpName.Text = ""
            cbo_Filter_Colour.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_FpName.SelectedIndex = -1
            cbo_Filter_Colour.SelectedIndex = -1
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
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Receipt_Sewing_No from Fabric_Receipt_Sewing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Receipt_Sewing_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, Fabric_Receipt_Sewing_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Receipt_Sewing_No from Fabric_Receipt_Sewing_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Receipt_Sewing_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, Fabric_Receipt_Sewing_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Receipt_Sewing_No from Fabric_Receipt_Sewing_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Receipt_Sewing_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, Fabric_Receipt_Sewing_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Receipt_Sewing_No from Fabric_Receipt_Sewing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Receipt_Sewing_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, Fabric_Receipt_Sewing_No desc", con)
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

            lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Fabric_Receipt_Sewing_Head", "Fabric_Receipt_Sewing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode)

            lbl_RecNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Fabric_Receipt_Sewing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Receipt_Sewing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Fabric_Receipt_Sewing_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                    If dt1.Rows(0).Item("Fabric_Receipt_Sewing_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Fabric_Receipt_Sewing_Date").ToString
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

            inpno = InputBox("Enter Job.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select Fabric_Receipt_Sewing_No from Fabric_Receipt_Sewing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Receipt_Sewing_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Job No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_FinishedProduct_ReceiptFrom_Sewing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_FinishedProduct_ReceiptFrom_Sewing, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Job No.", "FOR NEW SEWING INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select Fabric_Receipt_Sewing_No from Fabric_Receipt_Sewing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Receipt_Sewing_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Job No", "DOES NOT INSERT NEW SEWING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RecNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW SEWING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Col_ID As Integer = 0
        Dim Itfp_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim vTotQty As Single, vtotCMtrPs As Single, vTotCMtrs As Single, vtotConswgt As String
        Dim Sz_ID As Integer = 0
        Dim Fb_ID As Integer = 0
        Dim Sew_ID As Integer = 0
        Dim Sals_Id As Integer = 0
        Dim Proc_Id As Integer = 0
        Dim Nr As Integer = 0
        Dim vLOTCMPLT_STS As Integer = 0
        Dim vTot_Exs_Shrt_Wgt = ""
        Dim vTot_Exs_Shrt_Qty = ""
        Dim vCLOSTK_IN = ""
        Dim vSTOCK_POSTING_QTY = ""

        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Da4 As New SqlClient.SqlDataAdapter

        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_FinishedProduct_ReceiptFrom_Sewing, New_Entry) = False Then Exit Sub

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


        Sew_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Sew_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo
        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    If Trim(dgv_Details.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid FINISHEDPRODUCT NAME ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)

                        End If
                        Exit Sub
                    End If


                    If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid COLOUR NAME", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)

                        End If
                        Exit Sub

                    End If

                    'If Trim(dgv_Details.Rows(i).Cells(3).Value) = "" Then
                    '    MessageBox.Show("Invalid SIZE NAME", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If dgv_Details.Enabled And dgv_Details.Visible Then
                    '        dgv_Details.Focus()
                    '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)

                    '    End If
                    '    Exit Sub

                    'End If

                    If Val(dgv_Details.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Quantity..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                        Exit Sub
                    End If

                End If
            Next
        End With

        vLOTCMPLT_STS = 0
        If chk_LotComplete.Checked = True Then vLOTCMPLT_STS = 1

        Total_Calculation()

        vTotCMtrs = 0 : vtotCMtrPs = 0 : vTotQty = 0 : vTot_Exs_Shrt_Wgt = 0 : vTot_Exs_Shrt_Qty = 0
        vtotConswgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotQty = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotCMtrs = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            vtotConswgt = Val(dgv_Details_Total.Rows(0).Cells(12).Value())
            vTot_Exs_Shrt_Wgt = Val(dgv_Details_Total.Rows(0).Cells(13).Value())
            vTot_Exs_Shrt_Qty = Val(dgv_Details_Total.Rows(0).Cells(14).Value())

        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(EntFnYrCode)

            Else

                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Fabric_Receipt_Sewing_Head", "Fabric_Receipt_Sewing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(EntFnYrCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SewingDate", dtp_Date.Value.Date)


            If New_Entry = True Then

                cmd.CommandText = "Insert into Fabric_Receipt_Sewing_Head(Fabric_Receipt_Sewing_Code, Company_IdNo, Fabric_Receipt_Sewing_No, for_OrderBy, Fabric_Receipt_Sewing_Date, Sewing_IdNo,Party_Dc_No,   Total_Quantity,Total_Consum_Meter_Pcs, Total_Consum_Meters , Total_Consum_Weight, Lot_Complete_Status, user_Idno , ClothSales_OrderCode_forSelection , Total_ExcSht_Weight ,Total_ExcSht_Quantity ,vehicle_No ,remarks ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @SewingDate, " & Str(Val(Sew_ID)) & ",'" & Trim(txt_PartyDcNo.Text) & "' , " & Str(Val(vTotQty)) & "," & Str(Val(vtotCMtrPs)) & "," & Val(vTotCMtrs) & "," & Val(vtotConswgt) & ", " & Val(vLOTCMPLT_STS) & " , " & Val(lbl_UserName.Text) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ," & Val(vTot_Exs_Shrt_Wgt) & "," & Val(vTot_Exs_Shrt_Qty) & " , '" & Trim(cbo_Vechile.Text) & "' ,'" & Trim(txt_Remarks.Text) & "' )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Fabric_Receipt_Sewing_Head set Fabric_Receipt_Sewing_Date = @SewingDate, Sewing_IdNo = " & Val(Sew_ID) & ",Party_Dc_No = '" & Trim(txt_PartyDcNo.Text) & "', Total_Quantity = " & Val(vTotQty) & ",Total_Consum_Meter_Pcs = " & Val(vtotCMtrPs) & ",Total_Consum_meters = " & Val(vTotCMtrs) & ", user_idno = " & Val(lbl_UserName.Text) & " ,Total_Consum_Weight = " & Val(vtotConswgt) & " , Lot_Complete_Status = " & Val(vLOTCMPLT_STS) & " , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ,Total_ExcSht_Weight = " & Val(vTot_Exs_Shrt_Wgt) & " ,Total_ExcSht_Quantity = " & Val(vTot_Exs_Shrt_Qty) & " , vehicle_No = '" & Trim(cbo_Vechile.Text) & "' ,remarks='" & Trim(txt_Remarks.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                ' --- CODE BY GOPI 2025-02-04

                cmd.CommandText = "Update Fabric_Delivery_Sewing_Details set Receipt_Meters = a.Receipt_Meters - b.Consum_Meters ,  Receipt_Quantity = a.Receipt_Quantity - (b.Quantity - b.ExcSht_Quantity) ,  Receipt_Weight = a.Receipt_Weight - (b.Consum_weight - b.ExcSht_Weight)  from Fabric_Delivery_Sewing_Details a, Fabric_Receipt_Sewing_Details b Where b.Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "' and a.Fabric_Delivery_Sewing_code = b.Fabric_Delivery_Sewing_code and a.Fabric_Delivery_Sewing_details_SlNo = b.Fabric_Delivery_Sewing_Details_SlNo"
                cmd.ExecuteNonQuery()

                ' --- COMMAND BY GOPI 2025-02-04

                'cmd.CommandText = "Update Fabric_Delivery_Sewing_Details set Receipt_Meters = a.Receipt_Meters - b.Consum_Meters ,  Receipt_Quantity = a.Receipt_Quantity - b.Quantity ,  Receipt_Weight = a.Receipt_Weight - b.Consum_weight from Fabric_Delivery_Sewing_Details a, Fabric_Receipt_Sewing_Details b Where b.Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "' and a.Fabric_Delivery_Sewing_code = b.Fabric_Delivery_Sewing_code and a.Fabric_Delivery_Sewing_details_SlNo = b.Fabric_Delivery_Sewing_Details_SlNo"
                'cmd.ExecuteNonQuery()


                cmd.CommandText = "Update Fabric_Delivery_Sewing_Details set Lot_Complete_Status = 0 ,  Lot_Complete_FabricReceipt_Sewing_Code = '' Where Lot_Complete_FabricReceipt_Sewing_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Fabric_Receipt_Sewing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Sew : Job.No. " & Trim(lbl_RecNo.Text)
            PBlNo = Trim(lbl_RecNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then
                        Sno = Sno + 1
                        Itfp_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Sz_ID = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        Fb_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(5).Value, tr)
                        Proc_Id = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(6).Value, tr)

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Fabric_Receipt_Sewing_Details(Fabric_Receipt_Sewing_Code, Company_IdNo, Fabric_Receipt_Sewing_No, for_OrderBy, Fabric_Receipt_Sewing_Date,Sl_No, Sewing_IdNo,  FinishedProduct_IdNo, Colour_Idno ,Size_IdNo , Quantity,Fabric_IdNo,Process_idNo, Consum_Meter_Pcs, Consum_Meters ,Fabric_Delivery_Sewing_Code,Fabric_Delivery_Sewing_Details_Slno, Consum_weight_per_pcs, Consum_weight , ExcSht_Weight , ExcSht_Quantity ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @SewingDate," & Str(Val(Sno)) & ", " & Str(Val(Sew_ID)) & " ," & Str(Val(Itfp_ID)) & ", " & Str(Val(Col_ID)) & "," & Str(Val(Sz_ID)) & ",  " & Val(.Rows(i).Cells(4).Value) & "," & Str(Val(Fb_ID)) & "," & Str(Val(Proc_Id)) & "," & Val(.Rows(i).Cells(7).Value) & ", " & Val(.Rows(i).Cells(8).Value) & ",'" & Trim(.Rows(i).Cells(9).Value) & "', " & Val(.Rows(i).Cells(10).Value) & ", " & Val(.Rows(i).Cells(11).Value) & ", " & Val(.Rows(i).Cells(12).Value) & ", " & Val(.Rows(i).Cells(13).Value) & " , " & Val(.Rows(i).Cells(14).Value) & " )"
                        cmd.ExecuteNonQuery()

                        Nr = 0
                        cmd.CommandText = "Update Fabric_Delivery_Sewing_Details set Receipt_Meters = Receipt_Meters + " & Str(Val(.Rows(i).Cells(8).Value)) & " ,  Receipt_Quantity = Receipt_Quantity + " & Str(Val(.Rows(i).Cells(4).Value) - Val(.Rows(i).Cells(14).Value)) & " ,  Receipt_Weight = Receipt_Weight + " & Str(Val(.Rows(i).Cells(12).Value) - Val(.Rows(i).Cells(13).Value)) & " Where Fabric_Delivery_Sewing_code = '" & Trim(.Rows(i).Cells(9).Value) & "' and Fabric_Delivery_Sewing_Details_SlNo = " & Str(Val(.Rows(i).Cells(10).Value)) & " and Sewing_IdNo = " & Str(Val(Sew_ID))
                        Nr = cmd.ExecuteNonQuery()

                        If Val(vLOTCMPLT_STS) = 1 Then
                            cmd.CommandText = "Update Fabric_Delivery_Sewing_Details set Lot_Complete_Status = 1 ,  Lot_Complete_FabricReceipt_Sewing_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Where Fabric_Delivery_Sewing_code = '" & Trim(.Rows(i).Cells(9).Value) & "' and Fabric_Delivery_Sewing_Details_SlNo = " & Str(Val(.Rows(i).Cells(10).Value))
                            cmd.ExecuteNonQuery()
                        End If

                        ' ---- CODE BY GOPI 2025-02-04
                        ' --- STOCK POSTING 

                        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & " (        INt1          ,                     Meters1          ,       Meters2                         ,                           Weight1         )" &
                                                                                            " Values  ( " & Val(Itfp_ID) & " ,  " & Val(.Rows(i).Cells(4).Value) & " ,  " & Val(.Rows(i).Cells(8).Value) & " , " & Str(Val(.Rows(i).Cells(12).Value)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With


            ' ----------- CODE BY GOPI 2025-02-04 ' --- FOR SOTEXPA
            ' ---new
            ' ********************* FINISHED PRODUCT STOCK POSTING *********************


            Da2 = New SqlClient.SqlDataAdapter("Select   Int1 as finish_Pro_Id , Sum(Meters1) as Finish_Produ_QTY , Sum(Meters2)  as Meters , Sum(Weight1) as Weight From " & Trim(Common_Procedures.EntryTempTable) & " Group BY Int1 Having Sum(Meters1) > 0 ", con)
            If IsNothing(Da2) = False Then
                Da2.SelectCommand.Transaction = tr
            End If
            Dt2 = New DataTable
            Da2.Fill(Dt2)

            If Dt2.Rows.Count > 0 Then

                For K = 0 To Dt2.Rows.Count - 1

                    ' --- Check Cloth Stk Reconsilation

                    vCLOSTK_IN = ""

                    Finished_Product_Reconsilation_Mtrs_Wgt(Val(Dt2.Rows(K).Item("finish_Pro_Id").ToString), vCLOSTK_IN, tr)

                    vSTOCK_POSTING_QTY = 0

                    If Trim(UCase(vCLOSTK_IN)) = "WEIGHT" Then
                        vSTOCK_POSTING_QTY = Str(Val(Dt2.Rows(K).Item("Weight").ToString))
                    Else
                        vSTOCK_POSTING_QTY = Str(Val(Dt2.Rows(K).Item("Meters").ToString))
                    End If

                    cmd.CommandText = "Insert into Stock_Item_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date , ReceivedFrom_StockIdNo   ,                        DeliveryTo_StockIdNo                   ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,      Sl_No      ,                                   Item_IdNo            ,                                  Quantity                  ,       Meters                         ,                         ClothSales_OrderCode_forSelection ) " &
                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",  @SewingDate     , " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "    , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Val(K) & "  ," & Val(Dt2.Rows(K).Item("finish_Pro_Id").ToString) & " , " & Val(Dt2.Rows(K).Item("Finish_Produ_QTY").ToString) & " ," & Str(Val(vSTOCK_POSTING_QTY)) & "  , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "') "
                    cmd.ExecuteNonQuery()
                Next

            End If

            ' ----------- COMMAND BY GOPI 2025-02-04 ' 
            ' ---old

            'cmd.CommandText = "Insert into Stock_Item_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date , ReceivedFrom_StockIdNo   ,    DeliveryTo_StockIdNo          ,         Entry_ID     ,       Party_Bill_No  ,       Particulars        ,  Sl_No      , Item_IdNo        ,  Quantity   ,  Meters               , ClothSales_OrderCode_forSelection ) " &
            '                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",  @SewingDate     , " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "                , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1           ," & Str(Fb_ID) & " , " & Str(Val(vTotQty)) & " ," & Str(Val(vTotCMtrs)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "') "
            'cmd.ExecuteNonQuery()

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RecNo.Text)
                End If
            Else
                move_record(lbl_RecNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()



    End Sub

    Private Sub Total_Calculation()
        Dim vtotCMtrs As String, vTotQty As Single, vTotWGT As String
        Dim i As Integer
        Dim sno As Integer
        Dim vTot_Exs_Shrt_Wgt = ""
        Dim vTot_Exs_ShrtQty = ""

        vtotCMtrs = 0 : sno = 0 : vTotQty = 0 : vTotWGT = 0 : vTot_Exs_Shrt_Wgt = 0 : vTot_Exs_ShrtQty = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then
                    vTotQty = vTotQty + Val(dgv_Details.Rows(i).Cells(4).Value)
                    vtotCMtrs = Format(Val(vtotCMtrs) + Val(dgv_Details.Rows(i).Cells(8).Value), "##########0.000")
                    vTotWGT = Format(Val(vTotWGT) + Val(dgv_Details.Rows(i).Cells(12).Value), "##########0.000")
                    vTot_Exs_Shrt_Wgt = Format(Val(vTot_Exs_Shrt_Wgt) + Val(dgv_Details.Rows(i).Cells(13).Value), "##########0.000")
                    vTot_Exs_ShrtQty = Format(Val(vTot_Exs_ShrtQty) + Val(dgv_Details.Rows(i).Cells(13).Value), "##########0")
                End If

            Next

        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(4).Value = Val(vTotQty)
        dgv_Details_Total.Rows(0).Cells(8).Value = Format(Val(vtotCMtrs), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(12).Value = Format(Val(vTotWGT), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(13).Value = Format(Val(vTot_Exs_Shrt_Wgt), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(14).Value = Format(Val(vTot_Exs_ShrtQty), "#########0")
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING' )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING'  )", "(Ledger_idno = 0)")



    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING'  ) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Delivery:", "FOR FABRIC DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                txt_PartyDcNo.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "SEWING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub



    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        dgv_Details_CellLeave(sender, e)
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL END EDIT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter

        Try
            With dgv_Details
                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If

                If .Rows.Count > 0 Then


                    If Val(.CurrentCell.ColumnIndex) = 4 Or Val(.CurrentCell.ColumnIndex) = 7 Then
                        .Rows(.CurrentCell.RowIndex).Cells(8).Value = Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(4).Value) * Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(7).Value)
                    End If



                End If

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details

                If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    End If
                End If

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        Try
            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then
                        If Val(.CurrentCell.ColumnIndex) = 4 Or Val(.CurrentCell.ColumnIndex) = 7 Then
                            .Rows(.CurrentCell.RowIndex).Cells(8).Value = Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(4).Value) * Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(7).Value)
                        End If
                        If Val(.CurrentCell.ColumnIndex) = 4 Or Val(.CurrentCell.ColumnIndex) = 11 Then
                            .Rows(.CurrentCell.RowIndex).Cells(12).Value = Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(4).Value) * Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(11).Value)
                        End If
                        Total_Calculation()
                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        Try
            dgv_ActiveCtrl_Name = dgv_Details.Name
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue
            '  dgv_Selection.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgtxt_details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress

        Try


            With dgv_Details


                If Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 4 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
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

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            Total_Calculation()

        End If

    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgv_Details_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.GotFocus
        dgv_Details.Focus()
        'dgv_Details.CurrentCell.Selected = True
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Col_IdNo As Integer, Fp_IdNo
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Col_IdNo = 0
            Fp_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Fabric_Receipt_Sewing_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Fabric_Receipt_Sewing_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Fabric_Receipt_Sewing_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_FpName.Text) <> "" Then
                Fp_IdNo = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_Filter_FpName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Sewing_IdNo = " & Str(Val(Led_IdNo))
            End If


            If Val(Fp_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " c.FinishedProduct_Idno = " & Str(Val(Fp_IdNo))
            End If
            If Trim(cbo_Filter_Colour.Text) <> "" Then
                Col_IdNo = Common_Procedures.Colour_NameToIdNo(con, cbo_Filter_Colour.Text)
            End If
            If Val(Col_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " c.Colour_Idno = " & Str(Val(Col_IdNo))
            End If
            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.*,d.Processed_Item_Name as Fp_Name,e.Colour_Name,f.Cloth_Name from Fabric_Receipt_Sewing_Head a INNER JOIN Ledger_Head b on a.Sewing_IdNo = b.Ledger_IdNo INNER JOIN Fabric_Receipt_Sewing_Details c ON c.Fabric_Receipt_Sewing_Code = a.Fabric_Receipt_Sewing_Code INNER JOIN Processed_Item_Head d ON d.Processed_Item_IdNo = c.FinishedProduct_IdNo INNER JOIN Colour_Head e ON c.Colour_Idno = e.Colour_IdNo LEFT oUTER JOIN Cloth_Head f ON c.Fabric_Idno = f.Cloth_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Receipt_Sewing_Code LIKE '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Fabric_Receipt_Sewing_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Fabric_Receipt_Sewing_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Fabric_Receipt_Sewing_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("FP_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Colour_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Consum_Meter_Pcs").ToString), "##########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING' and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Colour, cbo_Filter_FpName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING'  and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_FpName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING' and Verified_Status = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_FpName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_FpName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")

    End Sub

    Private Sub cbo_Filter_FpName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_FpName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_FpName, cbo_Filter_PartyName, btn_Filter_Show, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")

    End Sub

    Private Sub cbo_Filter_FpName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_FpName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_FpName, btn_Filter_Show, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")
    End Sub
    Private Sub cbo_Filter_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_iDNO = 0)")
    End Sub

    Private Sub cbo_Filter_ColourName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Colour.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Colour, dtp_Filter_ToDate, cbo_Filter_PartyName, "Colour_Head", "Colour_Name", "", "Colour_IdNo = 0")
    End Sub

    Private Sub cbo_Filter_ColourName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Colour, cbo_Filter_PartyName, "Colour_Head", "Colour_Name", "", "Colour_IdNo = 0")
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
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Fabric_Receipt_Sewing_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "'", con)
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

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
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
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(EntFnYrCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Fabric_Receipt_Sewing_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Sewing_IdNo = c.Ledger_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)


            If prn_HdDt.Rows.Count > 0 Then
                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Fp_Name, c.Colour_Name ,d.Cloth_Name as Fabric_Name ,e.Process_Name  from Fabric_Receipt_Sewing_Details a LEFT OUTER JOIN Processed_Item_Head b on a.FinishedProduct_Idno = b.Processed_Item_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Cloth_Head d ON d.Cloth_idNo = a.Fabric_Idno LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Process_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
        Printing_Format1(e)
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
        Dim SNo As Integer
        Dim FpName As String = ""
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
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 8 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 170 : ClArr(3) = 110 : ClArr(4) = 90 : ClArr(5) = 150 : ClArr(6) = 120
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

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

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Fp_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        Dim FpNm1 As String, FpNm2 As String

                        FpNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Fabric_Name").ToString)
                        FpNm2 = ""
                        If Len(FpNm1) > 15 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(FpNm1), I, 1) = "@" Or Mid$(Trim(FpNm1), I, 1) = " " Or Mid$(Trim(FpNm1), I, 1) = "," Or Mid$(Trim(FpNm1), I, 1) = "." Or Mid$(Trim(FpNm1), I, 1) = "-" Or Mid$(Trim(FpNm1), I, 1) = "/" Or Mid$(Trim(FpNm1), I, 1) = "_" Or Mid$(Trim(FpNm1), I, 1) = "(" Or Mid$(Trim(FpNm1), I, 1) = ")" Or Mid$(Trim(FpNm1), I, 1) = "\" Or Mid$(Trim(FpNm1), I, 1) = "[" Or Mid$(Trim(FpNm1), I, 1) = "]" Or Mid$(Trim(FpNm1), I, 1) = "{" Or Mid$(Trim(FpNm1), I, 1) = "}" Or Mid$(Trim(FpNm1), I, 1) = "@" Then Exit For
                            Next I
                            If I = 0 Then I = 15
                            FpNm2 = Microsoft.VisualBasic.Right(Trim(FpNm1), Len(FpNm1) - I)
                            FpNm1 = Microsoft.VisualBasic.Left(Trim(FpNm1), I)
                        End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Colour_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(FpNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Process_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Consum_Meters").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Consum_Meters").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                        End If
                        ' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)


                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Or Trim(FpNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(FpNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        da2 = New SqlClient.SqlDataAdapter("select a.*,C.Processed_Item_Name as Fp_Name,d.Colour_Name,e.Size_Name,f.Cloth_Name as Fabric_Name,g.Process_Name from Fabric_Receipt_Sewing_Details a INNER JOIN Processed_Item_Head c ON  c.Processed_Item_IdNo = a.FinishedProduct_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Size_Head e ON e.Size_IdNo = a.Size_IdNo LEFT OUTER JOIN Cloth_Head f ON f.Cloth_idNo = a.Fabric_Idno LEFT OUTER JOIN Process_Head g ON g.Process_idNo = a.Process_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Receipt_Sewing_Code = '" & Trim(EntryCode) & "' Order by Sl_No", con)
        dt2 = New DataTable
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

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FINISHEDPRODUCT RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strHeight + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 40
            W1 = e.Graphics.MeasureString(" P.O.NO  : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Fabric_Receipt_Sewing_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Fabric_Receipt_Sewing_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Party_Dc_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " P.DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_Dc_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "FP NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "FABRIC NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PROCESS NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONS.MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

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



                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 2, ClAr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Consum_Meters").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Consum_Meters").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
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



            'If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 250, CurY, 0, 0, pFont)
            'End If


            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
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



    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String

        Dim Ent_Qty As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim Ent_WGT As String = 0
        Dim Ent_Excess_WGT As String = 0
        Dim Ent_ExSrt_Qty As String = 0
        Dim nr As Single = 0
        Dim Fabric_Processing_Recons = ""


        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(EntFnYrCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.* , b.* , c.Cloth_Name, d.Colour_Name ,e.Process_Name,f.Processed_Item_Name ,h.Consum_Meters As Ent_Mtrs, h.Quantity as Ent_Qty, h.Consum_weight as Ent_WGT , h.ExcSht_Weight as Ent_ExSrt_WGT , h.ExcSht_Quantity as Ent_ExSrt_Qty from Fabric_Delivery_Sewing_Head a INNER JOIN Fabric_Delivery_Sewing_details b ON a.Fabric_Delivery_Sewing_Code = b.Fabric_Delivery_Sewing_Code LEFT OUTER JOIN Cloth_Head c ON c.Cloth_IdNo = b.Fabric_IdNo  LEFT OUTER JOIN Colour_Head d ON b.Colour_IdNo = d.Colour_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = b.Process_Idno  LEFT OUTER JOIN Processed_Item_Head f ON f.Processed_Item_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Fabric_Receipt_Sewing_Details h ON h.Fabric_Receipt_Sewing_Code = '" & Trim(NewCode) & "'  and b.Fabric_Delivery_Sewing_Code = h.Fabric_Delivery_Sewing_Code and b.Fabric_Delivery_Sewing_details_SlNo = h.Fabric_Delivery_Sewing_Details_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Sewing_Idno = " & Str(Val(LedIdNo)) & " and ( ( (b.Quantity - b.Receipt_Quantity ) > 0 and b.Lot_Complete_Status = 0 ) or h.Quantity > 0 ) order by a.Fabric_Delivery_Sewing_Date, a.for_orderby, a.Fabric_Delivery_Sewing_No", con)
            Dt1 = New DataTable
            nr = Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    Ent_Pcs = 0
                    Ent_Qty = 0
                    Ent_Mtrs = 0
                    Ent_WGT = 0
                    Ent_Excess_WGT = 0
                    Ent_ExSrt_Qty = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Qty").ToString) = False Then
                        Ent_Qty = Val(Dt1.Rows(i).Item("Ent_Qty").ToString)
                    End If

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Mtrs").ToString) = False Then
                        Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_Mtrs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_WGT").ToString) = False Then
                        Ent_WGT = Val(Dt1.Rows(i).Item("Ent_WGT").ToString)
                    End If

                    If IsDBNull(Dt1.Rows(i).Item("Ent_ExSrt_WGT").ToString) = False Then
                        Ent_Excess_WGT = Val(Dt1.Rows(i).Item("Ent_ExSrt_WGT").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_ExSrt_Qty").ToString) = False Then
                        Ent_ExSrt_Qty = Val(Dt1.Rows(i).Item("Ent_ExSrt_Qty").ToString)
                    End If

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Fabric_Delivery_Sewing_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Fabric_Delivery_Sewing_Date").ToString), "dd-MM-yyyy")
                    ' .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("JobCard_Sewing_No").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Processed_Item_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("colour_Name").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Process_Name").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("cloth_Name").ToString
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Quantity").ToString) - Val(Dt1.Rows(i).Item("Receipt_Quantity").ToString) + Val(Ent_Qty), "#########0")

                    ' --- CODE BY GOPI 2025-02-04

                    'Fabric_Processing_Recons = ""

                    'Finished_Product_Reconsilation_Mtrs_Wgt(Dt1.Rows(i).Item("Processed_Item_IdNo").ToString, Fabric_Processing_Recons)

                    ' If Trim(Fabric_Processing_Recons) = "WEIGHT" Then'
                    .Rows(n).Cells(19).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString) - Val(Dt1.Rows(i).Item("Receipt_Weight").ToString) + Val(Ent_WGT), "#########0.00")
                    'Else
                    .Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString) - Val(Dt1.Rows(i).Item("Receipt_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")
                    'End If

                    ''.Rows(n).Cells(10).Value = Val(Dt1.Rows(i).Item("Delivery_Weight").ToString)

                    If Ent_Qty > 0 Then
                        .Rows(n).Cells(10).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(10).Value = ""

                    End If
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Fabric_Delivery_Sewing_Details_Slno").ToString
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Fabric_Delivery_Sewing_code").ToString

                    .Rows(n).Cells(13).Value = Ent_Qty
                    .Rows(n).Cells(14).Value = Ent_Mtrs
                    .Rows(n).Cells(15).Value = Format(Val(Dt1.Rows(i).Item("Meter_Quantity").ToString), "#########0.00")
                    .Rows(n).Cells(16).Value = Format(Val(Dt1.Rows(i).Item("Weight_Quantity").ToString), "#########0.00")
                    .Rows(n).Cells(17).Value = Ent_WGT
                    .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("ClothSales_OrderCode_forSelection").ToString


                    .Rows(n).Cells(20).Value = Ent_Excess_WGT
                    .Rows(n).Cells(21).Value = Ent_ExSrt_Qty

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        ' pnl_Back.Visible = False
        dgv_Selection.Focus()

    End Sub


    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(10).Value = (Val(.Rows(RwIndx).Cells(10).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(10).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next
                    .DefaultCellStyle.SelectionForeColor = Color.Red

                Else
                    .Rows(RwIndx).Cells(10).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next
                    .DefaultCellStyle.SelectionForeColor = Color.Black

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_Piece(n)

                e.Handled = True

            End If
        End If
    End Sub

    Private Sub dgv_Selection_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Selection.CellEnter
        With sender
            If Val(.Rows(e.RowIndex).Cells(10).Value) = 0 Then
                .DefaultCellStyle.SelectionForeColor = Color.Black
            Else
                .DefaultCellStyle.SelectionForeColor = Color.Red
            End If
        End With
    End Sub
    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Fabric_Delivery_Selection()
    End Sub

    Private Sub Fabric_Delivery_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim Fabric_Processing_Recons As String = ""

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(10).Value) = 1 Then


                '   lbl_JobNo.Text = Trim(dgv_Selection.Rows(i).Cells(3).Value)

                If cbo_ClothSales_OrderCode_forSelection.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(18).Value) <> "" Then
                        cbo_ClothSales_OrderCode_forSelection.Text = Trim(dgv_Selection.Rows(i).Cells(18).Value)
                    End If
                End If


                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(5).Value
                'dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(8).Value

                dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(7).Value

                dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(15).Value
                If Val(dgv_Details.Rows(n).Cells(7).Value) = 0 Then dgv_Details.Rows(n).Cells(7).Value = ""

                If Val(dgv_Selection.Rows(i).Cells(13).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(13).Value
                Else
                    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If


                If Val(dgv_Details.Rows(n).Cells(8).Value) = 0 Then dgv_Details.Rows(n).Cells(8).Value = ""

                    dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(12).Value
                    dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(11).Value

                    dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(16).Value
                    If Val(dgv_Details.Rows(n).Cells(11).Value) = 0 Then dgv_Details.Rows(n).Cells(11).Value = ""




                If Val(dgv_Selection.Rows(i).Cells(17).Value) <> 0 Then
                        dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(17).Value
                    Else
                        dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(19).Value ' Format(Val(dgv_Details.Rows(n).Cells(4).Value) * Val(dgv_Details.Rows(n).Cells(11).Value), "##########0.00")
                    End If

                    If Val(dgv_Selection.Rows(i).Cells(14).Value) <> 0 Then
                        dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(14).Value
                    Else
                        dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(9).Value
                    End If



                If Val(dgv_Details.Rows(n).Cells(12).Value) = 0 Then dgv_Details.Rows(n).Cells(12).Value = ""

                dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(20).Value
                dgv_Details.Rows(n).Cells(14).Value = dgv_Selection.Rows(i).Cells(21).Value

                If Val(dgv_Details.Rows(n).Cells(13).Value) = 0 Then dgv_Details.Rows(n).Cells(13).Value = ""
                    If Val(dgv_Details.Rows(n).Cells(14).Value) = 0 Then dgv_Details.Rows(n).Cells(14).Value = ""

                End If

        Next

        Total_Calculation()


        pnl_Back.Enabled = True
        pnl_Back.Visible = True
        pnl_Selection.Visible = False
        If txt_PartyDcNo.Enabled And txt_PartyDcNo.Visible Then txt_PartyDcNo.Focus()

    End Sub

    Private Sub txt_PartyDcNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PartyDcNo.KeyDown
        If e.KeyValue = 38 Then
            cbo_Ledger.Focus()
        End If
        If e.KeyValue = 40 Then
            If cbo_Vechile.Visible Then
                cbo_Vechile.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)

                Else
                    btn_save.Focus()
                End If
            End If


        End If
    End Sub

    Private Sub txt_PartyDcNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PartyDcNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_Vechile.Visible Then
                cbo_Vechile.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)

                Else
                    btn_save.Focus()
                End If
            End If



        End If
    End Sub

    Private Sub dgtxt_details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_details.Text)
                End If
            End With

        Catch ex As Exception
            '---

        End Try
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

    Private Sub btn_Close_Selection2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Selection2.Click
        btn_Close_Selection_Click(sender, e)
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.SelectedIndexChanged

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If Asc(e.KeyChar) = 13 Then
            If Asc(e.KeyChar) = 13 Then

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)

                Else
                    txt_Remarks.Focus()
                End If

            End If
        End If


    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_Vechile, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")


        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then


            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)

            Else
                txt_Remarks.Focus()
            End If


        End If


    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub
    Private Sub Finished_Product_Reconsilation_Mtrs_Wgt(ByVal vCloth_IDNo As Integer, ByRef vFabric_Processing_Recons_Type As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing)

        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        vFabric_Processing_Recons_Type = ""

        Da1 = New SqlClient.SqlDataAdapter("Select Reconsilation_Meter_Weight from Processed_Item_Head Where Processed_Item_IdNo= " & Str(Val(vCloth_IDNo)) & " ", con)
        If IsNothing(sqltr) = False Then
            Da1.SelectCommand.Transaction = sqltr
        End If
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            vFabric_Processing_Recons_Type = Dt1.Rows(0).Item("Reconsilation_Meter_Weight").ToString
        End If

    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Fabric_Receipt_Sewing_Head", "vehicle_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, txt_PartyDcNo, Nothing, "Fabric_Receipt_Sewing_Head", "vehicle_No", "", "")
        If (e.KeyCode = 40 And cbo_Vechile.DroppedDown = False) Or (e.KeyValue = 40 And e.Control = True) Then
            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)

                Else
                    btn_save.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vechile, Nothing, "Fabric_Receipt_Sewing_Head", "vehicle_No", "", "", False)
        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)

                Else
                    btn_save.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If (e.KeyValue = 38) Or (e.Control = True And e.KeyValue = 38) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)

            Else
                If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                    cbo_ClothSales_OrderCode_forSelection.Focus()
                Else
                    cbo_Vechile.Focus()
                End If
            End If

        End If
        If (e.KeyValue = 40) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If


        End If
    End Sub
End Class