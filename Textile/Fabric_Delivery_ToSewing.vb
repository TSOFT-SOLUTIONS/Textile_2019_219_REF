Public Class Fabric_Delivery_ToSewing
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FBDEL-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer

    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_123Count As Integer
    Private prn_HdAr(1000, 10) As String
    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl

    Private prn_Status As Integer = 0
    Private prn_DetAr(1000, 50, 10) As String
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private dgv_LevColNo As Integer

    Public Shared EntFnYrCode As String = ""
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Private vmskOldText As String = ""
    Private vmskSelStrt As Integer = -1
    Private vGRDFPNAME_ENTRCEL As String = ""

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

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_dcNo.Text = ""
        lbl_dcNo.ForeColor = Color.Black
        pnl_BaleSelection_ToolTip.Visible = False
        cbo_RollBundle.Text = "ROLL"
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dtp_Date.Text = ""
        cbo_Ledger.Text = ""

        cbo_Vechile.Text = ""
        txt_Remarks.Text = ""

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_grid_Colour.Enabled = True
        cbo_grid_Colour.BackColor = Color.White

        cbo_grid_FpName.Enabled = True
        cbo_grid_FpName.BackColor = Color.White

        cbo_grid_Fabric.Enabled = True
        cbo_grid_Fabric.BackColor = Color.White

        cbo_Grid_Process.Enabled = True
        cbo_Grid_Process.BackColor = Color.White

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        Grid_DeSelect()

        cbo_grid_Fabric.Visible = False
        cbo_grid_Colour.Visible = False
        cbo_Grid_Process.Visible = False
        cbo_grid_FpName.Visible = False

        cbo_grid_Fabric.Tag = -1
        cbo_grid_FpName.Tag = -1
        cbo_grid_Colour.Tag = -1

        cbo_Grid_Process.Tag = -1
        cbo_grid_Fabric.Text = ""
        cbo_grid_FpName.Text = ""

        cbo_grid_Colour.Text = ""
        cbo_Grid_Process.Text = ""

        cbo_ClothSales_OrderCode_forSelection.Text = ""


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

        If Me.ActiveControl.Name <> cbo_grid_Fabric.Name Then
            cbo_grid_Fabric.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_grid_Colour.Name Then
            cbo_grid_Colour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_grid_Fabric.Name Then
            cbo_grid_Fabric.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_grid_FpName.Name Then
            cbo_grid_FpName.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name And Not (TypeOf ActiveControl Is DataGridViewTextBoxEditingControl) Then
            'Common_Procedures.Hide_CurrentStock_Display()
            pnl_BaleSelection_ToolTip.Visible = False
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
        If FrmLdSTS = True Then Exit Sub
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

            da1 = New SqlClient.SqlDataAdapter("select a.* from Fabric_Delivery_Sewing_Head a  Where a.Fabric_Delivery_Sewing_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_dcNo.Text = dt1.Rows(0).Item("Fabric_Delivery_Sewing_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Fabric_Delivery_Sewing_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Sewing_IdNo").ToString))
                cbo_RollBundle.Text = dt1.Rows(0).Item("Roll_Bundle").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                cbo_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString
                cbo_Vechile.Text = dt1.Rows(0).Item("vehicle_No").ToString
                txt_Remarks.Text = Trim(dt1.Rows(0).Item("remarks").ToString)

                da2 = New SqlClient.SqlDataAdapter("select a.*,d.Colour_Name,f.Cloth_Name as Fabric_Name , g.Process_Name , h.Processed_Item_Name  from Fabric_Delivery_Sewing_Details a INNER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo  LEFT OUTER JOIN Cloth_Head f ON f.Cloth_idNo = a.Fabric_Idno LEFT OUTER JOIN Process_Head g ON a.Process_IdNo = g.Process_IdNo LEFT OUTER JOIN Processed_Item_Head h ON h.Processed_Item_IdNo = a.Processed_Item_IdNo where a.Fabric_Delivery_Sewing_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Fabric_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = (dt2.Rows(i).Item("Process_Name").ToString)
                        dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("No_of_Packs").ToString)
                        dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Packing_Nos").ToString
                        dgv_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Pcs").ToString)
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Meter_Quantity").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(11).Value = Val(dt2.Rows(i).Item("Quantity").ToString)

                        dgv_Details.Rows(n).Cells(12).Value = dt2.Rows(i).Item("Fabric_Delivery_Sewing_SlNo").ToString
                        dgv_Details.Rows(n).Cells(13).Value = dt2.Rows(i).Item("Processed_Fabric_inspection_Code").ToString

                        dgv_Details.Rows(n).Cells(14).Value = dt2.Rows(i).Item("Receipt_Meters").ToString
                        dgv_Details.Rows(n).Cells(15).Value = dt2.Rows(i).Item("Receipt_Quantity").ToString
                        dgv_Details.Rows(n).Cells(16).Value = dt2.Rows(i).Item("Fabric_Delivery_Sewing_Details_SlNo").ToString
                        dgv_Details.Rows(n).Cells(17).Value = dt2.Rows(i).Item("Weight_Quantity").ToString
                        dgv_Details.Rows(n).Cells(18).Value = dt2.Rows(i).Item("Receipt_Weight").ToString

                        If Val(dgv_Details.Rows(n).Cells(14).Value) <> 0 Or Val(dgv_Details.Rows(n).Cells(15).Value) <> 0 Or Val(dgv_Details.Rows(n).Cells(18).Value) <> 0 Then
                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True
                        End If

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_No_of_Packs").ToString)
                    .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    .Rows(0).Cells(11).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                End With

                Grid_DeSelect()

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()


            If LockSTS = True Then

                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray


                'cbo_grid_Colour.Enabled = False
                'cbo_grid_Colour.BackColor = Color.LightGray

                'cbo_grid_FpName.Enabled = False
                'cbo_grid_FpName.BackColor = Color.LightGray


                'cbo_grid_Fabric.Enabled = False
                'cbo_grid_Fabric.BackColor = Color.LightGray

                'cbo_Grid_Process.Enabled = False
                'cbo_Grid_Process.BackColor = Color.LightGray

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

    End Sub

    Private Sub Fabric_Delivery_ToSewing_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_grid_Fabric.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_grid_Fabric.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_grid_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_grid_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_grid_FpName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FINISHEDPRODUCT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_grid_FpName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


        FrmLdSTS = False

    End Sub

    Private Sub Fabric_Delivery_ToSewing_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim OpYrCode As String = ""

        Me.Text = ""

        If Trim(UCase(Common_Procedures.Sewing_Opening_OR_Entry)) = "OPENING" Then
            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            EntFnYrCode = OpYrCode

        Else
            EntFnYrCode = Common_Procedures.FnYearCode

        End If

        con.Open()

        cbo_grid_Fabric.Visible = False
        cbo_grid_Colour.Visible = False

        cbo_grid_FpName.Visible = False
        cbo_Grid_Process.Visible = False


        btn_RollSelection.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" Then '---- Prakash Cottex (Sulur)
            btn_RollSelection.Visible = True
        End If

        pnl_BaleSelection.Visible = False
        pnl_BaleSelection.Left = (Me.Width - pnl_BaleSelection.Width) \ 2
        pnl_BaleSelection.Top = (Me.Height - pnl_BaleSelection.Height) \ 2
        pnl_BaleSelection.BringToFront()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        cbo_RollBundle.Items.Clear()
        cbo_RollBundle.Items.Add(" ")
        cbo_RollBundle.Items.Add("ROLL")
        cbo_RollBundle.Items.Add("BUNDLE")


        pnl_BaleSelection_ToolTip.Visible = False

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
        AddHandler cbo_grid_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_Fabric.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Process.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RollBundle.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_FpName.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_FpName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_Fabric.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Process.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RollBundle.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_FpName.LostFocus, AddressOf ControlLostFocus



        AddHandler cbo_Filter_FpName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Fabric_Delivery_ToSewing_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Fabric_Delivery_ToSewing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_BaleSelection.Visible = True Then
                    btn_Close_BaleSelection_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 6 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                '    save_record()
                                'Else
                                '    dtp_Date.Focus()
                                'End If
                                txt_Remarks.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 11 Then

                            .Focus()
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(17)

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 3 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_Ledger.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 6)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 17 Then

                            .Focus()
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(11)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Sewing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Sewing, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        Da = New SqlClient.SqlDataAdapter("select count(*) from Fabric_Delivery_Sewing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code = '" & Trim(NewCode) & "' and  Receipt_Quantity <> 0", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Finished product received against this DC", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Update Processed_Fabric_inspection_Details set Sales_Invoice_Code = '', SalesInvoice_DetailsSlNo = 0, Sales_Invoice_Increment = Sales_Invoice_Increment - 1 Where Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update JobCard_Sewing_head set Fabric_Delivery_Code = '' Where Fabric_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Fabric_Delivery_Sewing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Fabric_Delivery_Sewing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

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

            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Delivery_Sewing_No from Fabric_Delivery_Sewing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, Fabric_Delivery_Sewing_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_dcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Delivery_Sewing_No from Fabric_Delivery_Sewing_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, Fabric_Delivery_Sewing_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_dcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Delivery_Sewing_No from Fabric_Delivery_Sewing_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, Fabric_Delivery_Sewing_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Delivery_Sewing_No from Fabric_Delivery_Sewing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, Fabric_Delivery_Sewing_No desc", con)
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
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_dcNo.Text = Common_Procedures.get_MaxCode(con, "Fabric_Delivery_Sewing_Head", "Fabric_Delivery_Sewing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Trim(EntFnYrCode))

            lbl_dcNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Fabric_Delivery_Sewing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Fabric_Delivery_Sewing_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                    If dt1.Rows(0).Item("Fabric_Delivery_Sewing_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Fabric_Delivery_Sewing_Date").ToString
                End If
            End If
            dt1.Clear()
            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


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

            Da = New SqlClient.SqlDataAdapter("select Fabric_Delivery_Sewing_No from Fabric_Delivery_Sewing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Job No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Sewing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Sewing, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Job No.", "FOR NEW SEWING INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select Fabric_Delivery_Sewing_No from Fabric_Delivery_Sewing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Job No", "DOES NOT INSERT NEW SEWING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_dcNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW SEWING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Col_ID As Integer = 0
        Dim Itfp_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim vTotPcs As Single, vtotWgt As Single, vTotMtrs As Single, vTotPacks As Single, vTotQty As Single
        Dim Sz_ID As Integer = 0
        Dim Fb_ID As Integer = 0
        Dim Sew_ID As Integer = 0
        Dim Sals_Id As Integer = 0
        Dim Nr As Integer = 0
        Dim Proc_ID As Integer = 0
        Dim Fabric_ID As Integer = 0
        Dim Fp_ID As Integer = 0
        Dim vRECON_IN As String
        Dim vCLOSTK_IN As String
        Dim vSTOCK_POSTING_QTY = ""
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Sewing, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        Sew_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Sew_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If
        lbl_UserName.Text = Common_Procedures.User.IdNo

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1

                If Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Then

                    Debug.Print(dgv_Details.Rows(i).Cells(17).Value)

                    If Trim(dgv_Details.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid FABRIC NAME ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    Fabric_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Fabric_ID = 0 Then
                        MessageBox.Show("Invalid FABRIC NAME ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid COLOUR NAME", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                        End If
                        Exit Sub
                    End If


                    Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(2).Value)
                    If Col_ID = 0 Then
                        MessageBox.Show("Invalid COLOUR NAME", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                        End If
                        Exit Sub
                    End If


                    If Trim(dgv_Details.Rows(i).Cells(3).Value) = "" Then
                        MessageBox.Show("Invalid PROCESS NAME", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                        End If
                        Exit Sub
                    End If


                    Proc_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(3).Value)
                    If Proc_ID = 0 Then
                        MessageBox.Show("Invalid PROCESS NAME", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                        End If
                        Exit Sub
                    End If

                    vCLOSTK_IN = "METER"
                    Da4 = New SqlClient.SqlDataAdapter("Select Stock_In from Cloth_Head Where Cloth_Idno = " & Str(Val(Fabric_ID)), con)
                    Dt4 = New DataTable
                    Da4.Fill(Dt4)
                    If Dt4.Rows.Count > 0 Then
                        vCLOSTK_IN = Dt4.Rows(0).Item("Stock_In").ToString
                    End If
                    Dt4.Clear()

                    If Trim(UCase(vCLOSTK_IN)) = "PCS" Then

                        If Val(dgv_Details.Rows(i).Cells(6).Value) = 0 Then
                            MessageBox.Show("Invalid PCS..", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled Then dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)
                            Exit Sub
                        End If

                    Else

                        If Val(dgv_Details.Rows(i).Cells(7).Value) = 0 Then
                            MessageBox.Show("Invalid METERS..", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled Then dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                            Exit Sub
                        End If

                    End If

                    If Trim(dgv_Details.Rows(i).Cells(9).Value) = "" Then
                        MessageBox.Show("Invalid FP NAME ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(9)
                        End If
                        Exit Sub
                    End If

                    Fp_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(9).Value)
                    If Fp_ID = 0 Then
                        MessageBox.Show("Invalid FP NAME ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(9)
                        End If
                        Exit Sub
                    End If


                    vRECON_IN = "METER"
                    Da4 = New SqlClient.SqlDataAdapter("Select Reconsilation_Meter_Weight from Processed_Item_Head Where Processed_Item_IdNo = " & Str(Val(Fp_ID)), con)
                    Dt4 = New DataTable
                    Da4.Fill(Dt4)
                    If Dt4.Rows.Count > 0 Then
                        vRECON_IN = Dt4.Rows(0).Item("Reconsilation_Meter_Weight").ToString
                    End If
                    Dt4.Clear()

                    If Trim(UCase(vRECON_IN)) = "WEIGHT" Then

                        If Val(dgv_Details.Rows(i).Cells(8).Value) = 0 Then
                            MessageBox.Show("Invalid WEIGHT..", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled Then dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(8)
                            Exit Sub
                        End If

                    Else

                        If Val(dgv_Details.Rows(i).Cells(7).Value) = 0 Then
                            MessageBox.Show("Invalid METERS..", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled Then dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                            Exit Sub
                        End If

                    End If

                    If Val(dgv_Details.Rows(i).Cells(11).Value) = 0 Then
                        MessageBox.Show("Invalid FP QTY..", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(11)
                        Exit Sub
                    End If

                End If

            Next
        End With

        Total_Calculation()

        vTotMtrs = 0 : vTotPcs = 0 : vtotWgt = 0 : vTotPacks = 0 : vTotQty = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotPacks = vTotPacks + Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vtotWgt = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            vTotQty = Val(dgv_Details_Total.Rows(0).Cells(11).Value())
        End If

        tr = con.BeginTransaction

        Try


            If Insert_Entry = True Or New_Entry = False Then

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcNo.Text) & "/" & Trim(EntFnYrCode)

            Else

                lbl_dcNo.Text = Common_Procedures.get_MaxCode(con, "Fabric_Delivery_Sewing_Head", "Fabric_Delivery_Sewing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcNo.Text) & "/" & Trim(EntFnYrCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SewingDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Fabric_Delivery_Sewing_Head(Fabric_Delivery_Sewing_Code, Company_IdNo, Fabric_Delivery_Sewing_No, for_OrderBy, Fabric_Delivery_Sewing_Date, Sewing_IdNo,  Total_Pcs,Total_Meters, Total_Weight , Total_No_of_Packs , Roll_Bundle  , Total_Quantity ,  User_idNo , ClothSales_OrderCode_forSelection  ,vehicle_No ,remarks) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcNo.Text))) & ", @SewingDate, " & Str(Val(Sew_ID)) & ", " & Str(Val(vTotPcs)) & "," & Str(Val(vTotMtrs)) & "," & Val(vtotWgt) & " ," & Val(vTotPacks) & " ,'" & Trim(cbo_RollBundle.Text) & "' , " & Val(vTotQty) & ", " & Val(lbl_UserName.Text) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "', '" & Trim(cbo_Vechile.Text) & "' ,'" & Trim(txt_Remarks.Text) & "' )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Fabric_Delivery_Sewing_Head set Fabric_Delivery_Sewing_Date = @SewingDate , Roll_Bundle = '" & Trim(cbo_RollBundle.Text) & "' ,  Sewing_IdNo = " & Val(Sew_ID) & ", Total_Pcs = " & Val(vTotPcs) & ",Total_Meters = " & Val(vTotMtrs) & ",Total_Weight = " & Val(vtotWgt) & " ,Total_Quantity = " & Val(vTotQty) & " , Total_No_of_Packs =  " & Val(vTotPacks) & " , User_IdNo = " & Val(lbl_UserName.Text) & " , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "', vehicle_No = '" & Trim(cbo_Vechile.Text) & "' ,remarks='" & Trim(txt_Remarks.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update JobCard_Sewing_head set Fabric_Delivery_Code = '' Where Fabric_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Processed_Fabric_inspection_Details set Sales_Invoice_Code = '', SalesInvoice_DetailsSlNo = 0, Sales_Invoice_Increment = Sales_Invoice_Increment - 1 Where Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Fabric_Delivery_Sewing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code = '" & Trim(NewCode) & "' and Receipt_Quantity = 0 "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            Partcls = "Sew : Job.No. " & Trim(lbl_dcNo.Text)
            PBlNo = Trim(lbl_dcNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_dcNo.Text)

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Then

                        Debug.Print(dgv_Details.Rows(i).Cells(17).Value)

                        Sno = Sno + 1

                        Fabric_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Proc_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        Fp_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(9).Value, tr)

                        Nr = 0
                        cmd.CommandText = "Update  Fabric_Delivery_Sewing_Details set Fabric_Delivery_Sewing_Date = @SewingDate , Sl_No  = " & Str(Val(Sno)) & " , Sewing_IdNo = " & Str(Val(Sew_ID)) & " , Fabric_IdNo = " & Str(Val(Fabric_ID)) & " , Colour_Idno = " & Val(Col_ID) & " , Process_IdNo = " & Val(Proc_ID) & " , No_Of_Packs =  " & Val(.Rows(i).Cells(4).Value) & ", Packing_Nos = '" & Trim(.Rows(i).Cells(5).Value) & "'  ,  Pcs = " & Val(.Rows(i).Cells(6).Value) & " ,  Meters = " & Str(Val(.Rows(i).Cells(7).Value)) & " ,    Weight = " & Str(Val(.Rows(i).Cells(8).Value)) & " ,    Processed_Item_IdNo = " & Str(Val(Fp_ID)) & " , Meter_Quantity = " & Str(Val(.Rows(i).Cells(10).Value)) & " , Quantity =" & Str(Val(.Rows(i).Cells(11).Value)) & "  , Weight_Quantity = " & Str(Val(.Rows(i).Cells(17).Value)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code = '" & Trim(NewCode) & "'  and Fabric_Delivery_Sewing_SlNo = " & Val(.Rows(i).Cells(12).Value)
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Fabric_Delivery_Sewing_Details(Fabric_Delivery_Sewing_Code, Company_IdNo, Fabric_Delivery_Sewing_No, for_OrderBy, Fabric_Delivery_Sewing_Date,Sl_No, Sewing_IdNo,  Fabric_IdNo, Colour_Idno ,  Process_IdNo  ,   No_Of_Packs  ,  Packing_Nos  , Pcs , Meters , Weight , Processed_Item_IdNo , Meter_Quantity , Quantity , Fabric_Delivery_Sewing_SlNo , Processed_Fabric_inspection_Code ,Weight_Quantity ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcNo.Text))) & ", @SewingDate," & Str(Val(Sno)) & ", " & Str(Val(Sew_ID)) & " ," & Str(Val(Fabric_ID)) & ", " & Str(Val(Col_ID)) & ", " & Str(Val(Proc_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", '" & Trim(.Rows(i).Cells(5).Value) & "' ,  " & Val(.Rows(i).Cells(6).Value) & "," & Val(.Rows(i).Cells(7).Value) & ", " & Val(.Rows(i).Cells(8).Value) & " ,  " & Str(Val(Fp_ID)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & " , " & Str(Val(.Rows(i).Cells(11).Value)) & "  ," & Str(Val(.Rows(i).Cells(12).Value)) & " , '" & Trim(.Rows(i).Cells(13).Value) & "' , " & Str(Val(.Rows(i).Cells(17).Value)) & ")"
                            cmd.ExecuteNonQuery()
                        End If

                        ' ---- CODE BY GOPI 2025-02-04
                        ' --- STOCK POSTING 

                        cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & " (        INt1            ,             iNT2   ,        Int3         ,         Int4      ,                     Meters1          ,       Meters2                         ,                           Weight1         ,                     Meters3               )" &
                                                                                            " Values  ( " & Val(Fabric_ID) & " ," & Val(Col_ID) & " ," & Val(Proc_ID) & " ," & Val(Fp_ID) & " , " & Val(.Rows(i).Cells(6).Value) & " ,  " & Val(.Rows(i).Cells(7).Value) & " , " & Str(Val(.Rows(i).Cells(8).Value)) & " ," & Str(Val(.Rows(i).Cells(11).Value)) & " )"
                        cmd.ExecuteNonQuery()


                        'cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                             , Reference_Date,     DeliveryTo_Idno  ,                               ReceivedFrom_Idno           ,         StockOff_IdNo                                     , Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No      ,           Cloth_Idno      ,   Colour_IdNo             ,                      Pcs                 ,                      Meters_Type1        ,                     Weight                ) " & _
                        '                       " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcNo.Text))) & ",  @SewingDate, " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Fb_ID)) & " ,     " & Str(Val(Col_ID)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " ) "
                        'cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            If Trim(UCase(cbo_RollBundle.Text)) = "ROLL" And Trim(Common_Procedures.settings.CustomerCode) = "1061" Then ' --- PRAKASH COTTEX

                cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date, DeliveryTo_Idno                                            ,  ReceivedFrom_Idno ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,  Sl_No      , Cloth_Idno        ,   Meters_Type2           ,StockOff_IdNo                                                  , Weight              , Rolls                            ,Colour_IdNo        ,Process_IdNo                               ,    ClothSales_OrderCode_forSelection ) " &
                                              " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcNo.Text))) & ",  @SewingDate     , " & Str(Val(Led_ID)) & ",  " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "               , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 2           ," & Str(Fabric_ID) & " , " & Str(Val(vTotMtrs)) & ",    " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "     ," & Str(Val(vtotWgt)) & "," & Str(Val(vTotPacks)) & "," & Str(Col_ID) & "," & Str(Proc_ID) & ", '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "') "
                cmd.ExecuteNonQuery()

                ' ----------- CODE BY GOPI 2025-02-04 ' --- PRAKASH COTTEX

                cmd.CommandText = "Insert into Stock_Item_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date , DeliveryTo_StockIdNo                                            ,  ReceivedFrom_StockIdNo ,         Entry_ID     ,       Party_Bill_No  ,       Particulars        ,  Sl_No      , Item_IdNo        ,  Quantity   ,  Meters               , ClothSales_OrderCode_forSelection  ) " &
                                           " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcNo.Text))) & ",  @SewingDate     , " & Str(Val(Led_ID)) & ", 0 , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1           ," & Str(Fp_ID) & " , " & Str(Val(vTotQty)) & " ," & Str(Val(vTotMtrs)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "') "
                cmd.ExecuteNonQuery()

            Else

                ' ----------- CODE BY GOPI 2025-02-04 ' --- FOR SOTEXPA
                ' ---- NEW

                ' ********************* CLOTH STOCK POSTING ITEM WISE **********************

                Da = New SqlClient.SqlDataAdapter("Select  INt1 as Cloth_Id , Sum(Meters1) as Pcs , Sum(Meters2) as Meters , Sum(Weight1) as Weight  From " & Trim(Common_Procedures.EntryTempTable) & " Group BY INt1 Having sum(Meters1) > 0  or sum(Meters2) > 0", con)
                If IsNothing(tr) = False Then
                    Da.SelectCommand.Transaction = tr
                End If
                Dt1 = New DataTable
                Da.Fill(Dt1)


                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        ' --- Check Cloth Stk Maintain

                        vCLOSTK_IN = ""

                        Da3 = New SqlClient.SqlDataAdapter("Select Stock_In from Cloth_Head Where Cloth_Idno = " & Val(Dt1.Rows(i).Item("Cloth_Id").ToString) & "", con)
                        If IsNothing(tr) = False Then
                            Da3.SelectCommand.Transaction = tr
                        End If
                        Dt3 = New DataTable
                        Da3.Fill(Dt3)

                        If Dt3.Rows.Count > 0 Then
                            vCLOSTK_IN = Dt3.Rows(0).Item("Stock_In").ToString
                        End If

                        vSTOCK_POSTING_QTY = 0

                        If Trim(UCase(vCLOSTK_IN)) = "PCS" Then
                            vSTOCK_POSTING_QTY = Str(Val(Dt1.Rows(i).Item("Pcs").ToString))
                        Else
                            vSTOCK_POSTING_QTY = Str(Val(Dt1.Rows(i).Item("Meters").ToString))
                        End If

                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (                             Reference_Code  ,         Company_IdNo             ,           Reference_No        ,                               for_OrderBy                              ,  Reference_Date    ,    DeliveryTo_Idno        ,                                       ReceivedFrom_Idno      ,     Entry_ID           ,    Party_Bill_No      ,       Particulars        ,         Sl_No          ,                        Cloth_Idno                    ,                Meters_Type1           ,                               StockOff_IdNo                      ,                              Weight                 ,                                    PCS            ,                                      Weight_Type1     ,             ClothSales_OrderCode_forSelection          ) " &
                                                                           "   Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_dcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcNo.Text))) & "  ,  @SewingDate      ,  " & Str(Val(Led_ID)) & " , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "   ,  '" & Trim(EntID) & "' , '" & Trim(PBlNo) & "' , '" & Trim(Partcls) & "' ,    " & Str(Val(i)) & "  ,  " & Val(Dt1.Rows(i).Item("Cloth_Id").ToString) & "  , " & Str(Val(vSTOCK_POSTING_QTY)) & "  ,    " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "    ," & Str(Val(Dt1.Rows(i).Item("Weight").ToString)) & "," & Str(Val(Dt1.Rows(i).Item("Pcs").ToString)) & " , " & Str(Val(Dt1.Rows(i).Item("Weight").ToString)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ) "
                        cmd.ExecuteNonQuery()


                    Next

                    Da.Dispose()
                    Dt1.Dispose()
                    Dt1.Clear()

                    Da3.Dispose()
                    Dt3.Dispose()
                    Dt3.Clear()


                    ' ----------- CODE BY GOPI 2025-02-04 ' --- FOR SOTEXPA

                    ' ********************* FINISHED PRODUCT STOCK POSTING *********************


                    Da2 = New SqlClient.SqlDataAdapter("Select   Int4 as finish_Pro_Id , Sum(Meters1) as Pcs , Sum(Meters2)  as Meters , Sum(Meters3) as Finish_Produ_QTY , Sum(Weight1) as Weight From " & Trim(Common_Procedures.EntryTempTable) & " Group BY Int4 Having Sum(Meters3) > 0 ", con)
                    If IsNothing(Da2) = False Then
                        Da2.SelectCommand.Transaction = tr
                    End If
                    Dt2 = New DataTable
                    Da2.Fill(Dt2)

                    If Dt2.Rows.Count > 0 Then

                        For K = 0 To Dt2.Rows.Count - 1

                            ' --- Check Cloth Stk Reconsilation

                            vCLOSTK_IN = ""

                            Da4 = New SqlClient.SqlDataAdapter("Select Reconsilation_Meter_Weight from Processed_Item_Head Where Processed_Item_IdNo = " & Val(Dt2.Rows(K).Item("finish_Pro_Id").ToString) & "", con)
                            If IsNothing(tr) = False Then
                                Da4.SelectCommand.Transaction = tr
                            End If
                            Dt4 = New DataTable
                            Da4.Fill(Dt4)

                            If Dt4.Rows.Count > 0 Then
                                vCLOSTK_IN = Dt4.Rows(0).Item("Reconsilation_Meter_Weight").ToString
                            End If

                            vSTOCK_POSTING_QTY = 0

                            If Trim(UCase(vCLOSTK_IN)) = "WEIGHT" Then
                                vSTOCK_POSTING_QTY = Str(Val(Dt2.Rows(K).Item("Weight").ToString))
                            Else
                                vSTOCK_POSTING_QTY = Str(Val(Dt2.Rows(K).Item("Meters").ToString))
                            End If


                            cmd.CommandText = "Insert into Stock_Item_Processing_Details (                           Reference_Code   ,         Company_IdNo             ,           Reference_No        ,                               for_OrderBy                              , Reference_Date  ,        DeliveryTo_StockIdNo  ,                         ReceivedFrom_StockIdNo                  ,         Entry_ID     ,       Party_Bill_No  ,       Particulars        ,     Sl_No           ,                         Item_IdNo                      ,                           Quantity                              ,                   Meters               ,                   ClothSales_OrderCode_forSelection        ) " &
                                                                           " Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcNo.Text))) & ",    @SewingDate     , " & Str(Val(Led_ID)) & "    , 0      , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "'  , " & Str(Val(K)) & " ," & Val(Dt2.Rows(K).Item("finish_Pro_Id").ToString) & " , " & Str(Val(Dt2.Rows(K).Item("Finish_Produ_QTY").ToString)) & " ,  " & Str(Val(vSTOCK_POSTING_QTY)) & "       , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ) "
                            cmd.ExecuteNonQuery()


                        Next

                    End If

                    Da2.Dispose()
                    Dt2.Dispose()
                    Dt2.Clear()

                    Da4.Dispose()
                    Dt4.Dispose()
                    Dt4.Clear()

                End If


                ' ---- COMMAND BY GOPI 2025-02-04 ' --- OLD

                'cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date, DeliveryTo_Idno                                            ,  ReceivedFrom_Idno ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,  Sl_No      , Cloth_Idno        ,   Meters_Type1     ,  StockOff_IdNo                                                    , Weight                      , PCS                            ,Colour_IdNo        ,Process_IdNo         ,                ClothSales_OrderCode_forSelection ) " &
                '                             " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcNo.Text))) & ",  @SewingDate     , " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "                , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 2           ," & Str(Fabric_ID) & " , " & Str(Val(vTotMtrs)) & ",    " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "     ," & Str(Val(vtotWgt)) & "," & Str(Val(vTotPacks)) & "," & Str(Col_ID) & "," & Str(Proc_ID) & ",'" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ) "
                'cmd.ExecuteNonQuery()


                ''cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date, DeliveryTo_Idno                                            ,  ReceivedFrom_Idno ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,  Sl_No      , Cloth_Idno        ,   Meters_Type3     ,  StockOff_IdNo                                                    , Weight                      , Bundle                            ,Colour_IdNo        ,Process_IdNo          ) " & _
                ''                              " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcNo.Text))) & ",  @SewingDate     , " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "                , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 2           ," & Str(Fabric_ID) & " , " & Str(Val(vTotMtrs)) & ",    " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "     ," & Str(Val(vtotWgt)) & "," & Str(Val(vTotPacks)) & "," & Str(Col_ID) & "," & Str(Proc_ID) & ") "
                ''cmd.ExecuteNonQuery()

            End If

            ' ---- COMMAND BY GOPI 2025-02-04 ' --- OLD

            'cmd.CommandText = "Insert into Stock_Item_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date , DeliveryTo_StockIdNo                                            ,  ReceivedFrom_StockIdNo ,         Entry_ID     ,       Party_Bill_No  ,       Particulars        ,  Sl_No      , Item_IdNo        ,  Quantity   ,  Meters               , ClothSales_OrderCode_forSelection  ) " &
            '                                  " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcNo.Text))) & ",  @SewingDate     , " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "                , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1           ," & Str(Fb_ID) & " , " & Str(Val(vTotQty)) & " ," & Str(Val(vTotMtrs)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "') "
            'cmd.ExecuteNonQuery()



            With dgv_BaleSelectionDetails

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 And Trim(.Rows(i).Cells(5).Value) <> "" Then

                        cmd.CommandText = "Update Processed_Fabric_inspection_Details set Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "', SalesInvoice_DetailsSlNo = " & Str(Val(.Rows(i).Cells(0).Value)) & ", Sales_Invoice_Increment = Sales_Invoice_Increment + 1 Where Roll_Code = '" & Trim(.Rows(i).Cells(5).Value) & "'"
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_dcNo.Text)
                End If
            Else
                move_record(lbl_dcNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()



    End Sub



    Private Sub Total_Calculation()
        Dim vTotPcs As Single, vtotMtrs As Single, vTotWgt As Single, vTotPacks As Single

        Dim i As Integer
        Dim sno As Integer
        Dim VTotQty As Integer = 0


        vtotMtrs = 0 : vTotPcs = 0 : sno = 0 : vTotWgt = 0 : vTotPacks = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Then
                    vTotPacks = vTotPacks + Val(dgv_Details.Rows(i).Cells(4).Value)
                    'vTotPacks = vTotPacks + 1
                    vTotPcs = vTotPcs + Val(dgv_Details.Rows(i).Cells(6).Value)
                    vtotMtrs = vtotMtrs + Val(dgv_Details.Rows(i).Cells(7).Value)
                    vTotWgt = vTotWgt + Val(dgv_Details.Rows(i).Cells(8).Value)
                    VTotQty = VTotQty + Val(dgv_Details.Rows(i).Cells(11).Value)
                End If
            Next
        End With

        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(4).Value = Val(vTotPacks)
        dgv_Details_Total.Rows(0).Cells(6).Value = Val(vTotPcs)
        dgv_Details_Total.Rows(0).Cells(7).Value = Format(Val(vtotMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(8).Value = Format(Val(vTotWgt), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(11).Value = Format(Val(VTotQty), "#########0")

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING' )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, cbo_RollBundle, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING'  )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_RollBundle, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING' ) ", "(Ledger_idno = 0)")

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

    Private Sub Set_Max_DetailsSlNo(ByVal RowNo As Integer, ByVal DetSlNo_ColNo As Integer)
        Dim MaxSlNo As Integer = 0
        Dim i As Integer

        With dgv_Details
            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(DetSlNo_ColNo).Value) > Val(MaxSlNo) Then
                    MaxSlNo = Val(.Rows(i).Cells(DetSlNo_ColNo).Value)
                End If
            Next
            .Rows(RowNo).Cells(DetSlNo_ColNo).Value = Val(MaxSlNo) + 1
        End With

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim rect As Rectangle
        Dim vRECON_IN As String = ""
        Dim LckSTS As Boolean = False



        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_Details

                'If Val(.Rows(e.RowIndex).Cells(9).Value) = 0 Then
                '    Set_Max_DetailsSlNo(e.RowIndex, 9)
                '    'If e.RowIndex = 0 Then
                '    '    .Rows(e.RowIndex).Cells(15).Value = 1
                '    'Else
                '    '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
                '    'End If
                'End If

                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If


                If Val(.Rows(e.RowIndex).Cells(12).Value) = 0 Then
                    If e.RowIndex = 0 Then
                        .Rows(e.RowIndex).Cells(12).Value = 1
                    Else
                        .Rows(e.RowIndex).Cells(12).Value = Val(.Rows(e.RowIndex - 1).Cells(12).Value) + 1
                    End If
                End If

                LckSTS = False
                If Val(dgv_Details.Rows(e.RowIndex).Cells(14).Value) <> 0 Or Val(dgv_Details.Rows(e.RowIndex).Cells(15).Value) <> 0 Or Val(dgv_Details.Rows(e.RowIndex).Cells(18).Value) <> 0 Then
                    LckSTS = True
                End If



                If e.ColumnIndex = 1 And LckSTS = False Then

                    If cbo_grid_Fabric.Visible = False Or Val(cbo_grid_Fabric.Tag) <> e.RowIndex Then

                        cbo_grid_Fabric.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head where Cloth_type = 'PROCESSED FABRIC' order by Cloth_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_grid_Fabric.DataSource = Dt1
                        cbo_grid_Fabric.DisplayMember = "Cloth_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_grid_Fabric.Left = .Left + rect.Left
                        cbo_grid_Fabric.Top = .Top + rect.Top
                        cbo_grid_Fabric.Width = rect.Width
                        cbo_grid_Fabric.Height = rect.Height

                        cbo_grid_Fabric.Text = .CurrentCell.Value

                        cbo_grid_Fabric.Tag = Val(e.RowIndex)
                        cbo_grid_Fabric.Visible = True

                        cbo_grid_Fabric.BringToFront()
                        cbo_grid_Fabric.Focus()


                    End If

                Else

                    cbo_grid_Fabric.Visible = False


                End If


                If e.ColumnIndex = 2 And LckSTS = False Then

                    If cbo_grid_Colour.Visible = False Or Val(cbo_grid_Colour.Tag) <> e.RowIndex Then

                        cbo_grid_Colour.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        cbo_grid_Colour.DataSource = Dt2
                        cbo_grid_Colour.DisplayMember = "Colour_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_grid_Colour.Left = .Left + rect.Left
                        cbo_grid_Colour.Top = .Top + rect.Top
                        cbo_grid_Colour.Width = rect.Width
                        cbo_grid_Colour.Height = rect.Height

                        cbo_grid_Colour.Text = .CurrentCell.Value

                        cbo_grid_Colour.Tag = Val(e.RowIndex)
                        cbo_grid_Colour.Visible = True

                        cbo_grid_Colour.BringToFront()
                        cbo_grid_Colour.Focus()



                    End If

                Else

                    cbo_grid_Colour.Visible = False


                End If

                If e.ColumnIndex = 3 And LckSTS = False Then

                    If cbo_Grid_Process.Visible = False Or Val(cbo_Grid_Process.Tag) <> e.RowIndex Then

                        cbo_Grid_Process.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Process_Name from Process_Head order by Process_Name", con)
                        Dt3 = New DataTable
                        Da.Fill(Dt3)
                        cbo_Grid_Process.DataSource = Dt3
                        cbo_Grid_Process.DisplayMember = "Process_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_Process.Left = .Left + rect.Left
                        cbo_Grid_Process.Top = .Top + rect.Top

                        cbo_Grid_Process.Width = rect.Width
                        cbo_Grid_Process.Height = rect.Height
                        cbo_Grid_Process.Text = .CurrentCell.Value

                        cbo_Grid_Process.Tag = Val(e.RowIndex)
                        cbo_Grid_Process.Visible = True

                        cbo_Grid_Process.BringToFront()
                        cbo_Grid_Process.Focus()


                    End If

                Else
                    cbo_Grid_Process.Visible = False

                End If

                If e.ColumnIndex = 9 And LckSTS = False Then

                    If cbo_grid_FpName.Visible = False Or Val(cbo_grid_FpName.Tag) <> e.RowIndex Then


                        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")

                        cbo_grid_FpName.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head order by Processed_Item_Name", con)
                        Dt5 = New DataTable
                        Da.Fill(Dt5)
                        cbo_grid_FpName.DataSource = Dt5
                        cbo_grid_FpName.DisplayMember = "Processed_Item_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_grid_FpName.Left = .Left + rect.Left
                        cbo_grid_FpName.Top = .Top + rect.Top
                        cbo_grid_FpName.Width = rect.Width
                        cbo_grid_FpName.Height = rect.Height

                        cbo_grid_FpName.Text = .CurrentCell.Value

                        cbo_grid_FpName.Tag = Val(e.RowIndex)
                        cbo_grid_FpName.Visible = True
                        'cbo_grid_FpName.Text = .CurrentCell.Value
                        cbo_grid_FpName.BringToFront()
                        cbo_grid_FpName.Focus()


                    End If

                Else

                    cbo_grid_FpName.Visible = False


                End If

                If e.ColumnIndex = 10 Or e.ColumnIndex = 11 Or e.ColumnIndex = 17 Then

                    If Val(.Rows(e.RowIndex).Cells(11).Value) = 0 Then
                        get_FP_MeterPerQty_and_WeightPerQty(.Rows(e.RowIndex).Cells(9).Value)
                    End If

                    vRECON_IN = "METER"
                    Da4 = New SqlClient.SqlDataAdapter("Select Reconsilation_Meter_Weight from Processed_Item_Head Where Processed_Item_Name = '" & Trim(.CurrentRow.Cells(9).Value) & "'", con)
                    Dt4 = New DataTable
                    Da4.Fill(Dt4)
                    If Dt4.Rows.Count > 0 Then
                        vRECON_IN = Dt4.Rows(0).Item("Reconsilation_Meter_Weight").ToString
                    End If
                    Dt4.Clear()

                    If Trim(UCase(vRECON_IN)) = "WEIGHT" Then
                        .Rows(e.RowIndex).Cells(10).Value = ""
                        .Rows(e.RowIndex).Cells(10).ReadOnly = True

                    Else
                        .Rows(e.RowIndex).Cells(17).Value = ""
                        .Rows(e.RowIndex).Cells(17).ReadOnly = True

                    End If

                End If

                If (e.ColumnIndex = 4 Or e.ColumnIndex = 5) And Trim(Common_Procedures.settings.CustomerCode) = "1061" Then

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    pnl_BaleSelection_ToolTip.Left = .Left + rect.Left
                    pnl_BaleSelection_ToolTip.Top = .Top + rect.Top + rect.Height + 3

                    pnl_BaleSelection_ToolTip.Visible = True

                Else
                    pnl_BaleSelection_ToolTip.Visible = False

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try

            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details

                If .CurrentCell.ColumnIndex = 7 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    End If
                End If

                If .CurrentCell.ColumnIndex = 8 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim vQTY As String = 0
        Dim vMTRPERQTY As String = 0
        Dim vWGTPERQTY As String = 0

        Try

            With dgv_Details

                If FrmLdSTS = True Then Exit Sub
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                If .Visible Then

                    If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 11 Then

                        If .CurrentCell.ColumnIndex = 7 Then
                            vMTRPERQTY = ""
                            If Val(.CurrentRow.Cells(11).Value) <> 0 Then
                                vMTRPERQTY = Format(Val(.CurrentRow.Cells(7).Value) / (.CurrentRow.Cells(11).Value), "##########0.00")
                                If Val(vMTRPERQTY) = 0 Then vMTRPERQTY = ""
                            End If
                            .CurrentRow.Cells(10).Value = vMTRPERQTY
                        End If


                        If .CurrentCell.ColumnIndex = 8 Then
                            vWGTPERQTY = ""
                            If Val(.CurrentRow.Cells(11).Value) <> 0 Then
                                vWGTPERQTY = Format(Val(.CurrentRow.Cells(8).Value) / (.CurrentRow.Cells(11).Value), "##########0.000")
                                If Val(vWGTPERQTY) = 0 Then vWGTPERQTY = ""

                            End If
                            .CurrentRow.Cells(17).Value = vWGTPERQTY
                        End If

                        Total_Calculation()

                    End If

                    'If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 17 Then

                    '    If Val(.CurrentRow.Cells(7).Value) <> 0 And Val(.CurrentRow.Cells(10).Value) <> 0 Then
                    '        .CurrentRow.Cells(11).Value = Val(.CurrentRow.Cells(7).Value) / (.CurrentRow.Cells(10).Value)
                    '    End If

                    '    If Val(.CurrentRow.Cells(8).Value) <> 0 And Val(.CurrentRow.Cells(10).Value) <> 0 Then
                    '        .CurrentRow.Cells(17).Value = Val(.CurrentRow.Cells(8).Value) / (.CurrentRow.Cells(10).Value)
                    '    End If

                    'End If

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

            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            With dgv_Details
                vcbo_KeyDwnVal = e.KeyValue
                If e.KeyValue = Keys.Delete Then
                    If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(14).Value) <> 0 Or Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(15).Value) <> 0 Then
                        e.Handled = True
                    End If
                End If
            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress

        Try


            With dgv_Details
                If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(14).Value) <> 0 Or Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(15).Value) <> 0 Then
                    e.Handled = True
                End If
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Then
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
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim n As Integer
        Dim vQTY As String = 0
        Dim vMTRPERQTY As String = 0
        Dim vWGTPERQTY As String = 0
        Dim vRECON_IN As String = ""

        If FrmLdSTS = True Then Exit Sub
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details

            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                e.Handled = True
                e.SuppressKeyPress = True

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

                Total_Calculation()

            End If

        End With

    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)

            'If Val(.Rows(e.RowIndex).Cells(9).Value) = 0 Then
            '    Set_Max_DetailsSlNo(e.RowIndex, 9)
            '    'If e.RowIndex = 0 Then
            '    '    .Rows(e.RowIndex).Cells(15).Value = 1
            '    'Else
            '    '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
            '    'End If
            'End If



            If Val(.Rows(e.RowIndex).Cells(12).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(12).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(12).Value = Val(.Rows(e.RowIndex - 1).Cells(12).Value) + 1
                End If
            End If

        End With
    End Sub
    Private Sub cbo_grid_FpName_Enter(sender As Object, e As EventArgs) Handles cbo_grid_FpName.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")
        vGRDFPNAME_ENTRCEL = cbo_grid_FpName.Text
    End Sub

    Private Sub cbo_grid_FpName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_FpName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_FpName, Nothing, cbo_grid_Colour, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_grid_FpName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

            End If

            If (e.KeyValue = 40 And cbo_grid_FpName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End If

        End With

    End Sub

    Private Sub cbo_grid_FpName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_grid_FpName.KeyPress
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim n As Integer
        Dim vQTY As String = 0
        Dim vMTRPERQTY As String = 0
        Dim vWGTPERQTY As String = 0
        Dim vRECON_IN As String = ""


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_FpName, Nothing, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(vGRDFPNAME_ENTRCEL)) <> Trim(UCase(cbo_grid_FpName.Text)) Then
                vGRDFPNAME_ENTRCEL = cbo_grid_FpName.Text
                If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(11).Value) = 0 Then
                    get_FP_MeterPerQty_and_WeightPerQty(cbo_grid_FpName.Text)
                End If
            End If

            With dgv_Details
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End With

        End If
    End Sub

    Private Sub cbo_grid_FpName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_FpName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New FinishedProduct_Creation_Simple

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grid_FpName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_grid_FpName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_FpName.TextChanged
        Try
            If cbo_grid_FpName.Visible Then
                With dgv_Details
                    If Val(cbo_grid_FpName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 9 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_FpName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_grid_FpName_LostFocus(sender As Object, e As EventArgs) Handles cbo_grid_FpName.LostFocus
        If Trim(UCase(vGRDFPNAME_ENTRCEL)) <> Trim(UCase(cbo_grid_FpName.Text)) Then
            vGRDFPNAME_ENTRCEL = cbo_grid_FpName.Text
            If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(11).Value) = 0 Then
                get_FP_MeterPerQty_and_WeightPerQty(cbo_grid_FpName.Text)
            End If
        End If
    End Sub

    Private Sub cbo_Grid_Process_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Process.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")
    End Sub
    Private Sub cbo_Grid_Process_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Process.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Process, Nothing, Nothing, "Process_Head", "Process_Name", "", "(process_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Process.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Process.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Process_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Process.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Process, Nothing, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                e.Handled = True
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End With

        End If

    End Sub
    Private Sub cbo_Grid_Processs_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Process.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Process.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_Process_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Process.TextChanged
        Try
            If cbo_Grid_Process.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                With dgv_Details
                    If Val(cbo_Grid_Process.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Process.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_grid_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_grid_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_Colour, Nothing, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_grid_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_grid_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_grid_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_grid_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_Colour, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_grid_Colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Colour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grid_Colour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_grid_Colour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Colour.TextChanged
        Try
            If cbo_grid_Colour.Visible Then
                With dgv_Details
                    If Val(cbo_grid_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_grid_Fabric_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Fabric.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_iDNO = 0)")

    End Sub

    Private Sub cbo_grid_Fabric_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Fabric.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_Fabric, Nothing, Nothing, "Cloth_Head", "Cloth_name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_iDNO = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_grid_Fabric.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then

                    If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                        cbo_ClothSales_OrderCode_forSelection.Focus()
                    Else
                        cbo_RollBundle.Focus()
                    End If

                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(17)
                    '.CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(11)
                    .CurrentCell.Selected = True

                End If

            End If

            If (e.KeyValue = 40 And cbo_grid_Fabric.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    '    save_record()
                    'Else
                    '    msk_Date.Focus()

                    'End If
                    txt_Remarks.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End If

        End With
    End Sub

    Private Sub cbo_grid_Fabric_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_grid_Fabric.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_Fabric, Nothing, "Cloth_Head", "Cloth_name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_iDNO = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    '    save_record()
                    'Else
                    '    msk_Date.Focus()

                    'End If
                    txt_Remarks.Focus()
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If

            End With

        End If
    End Sub

    Private Sub cbo_grid_Fabric_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_Fabric.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grid_Fabric.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_grid_Fabric_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grid_Fabric.TextChanged
        Try
            If cbo_grid_Fabric.Visible Then
                With dgv_Details
                    If Val(cbo_grid_Fabric.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_Fabric.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
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
                Condt = "a.Fabric_Delivery_Sewing_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Fabric_Delivery_Sewing_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Fabric_Delivery_Sewing_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.*,d.Cloth_Name as Fabric_Name,e.Colour_Name from Fabric_Delivery_Sewing_Head a INNER JOIN Ledger_Head b on a.Sewing_IdNo = b.Ledger_IdNo INNER JOIN Fabric_Delivery_Sewing_Details c ON c.Fabric_Delivery_Sewing_Code = a.Fabric_Delivery_Sewing_Code INNER JOIN Cloth_Head d ON d.Cloth_IdNo = c.Fabric_IdNo INNER JOIN Colour_Head e ON c.Colour_Idno = e.Colour_IdNo LEFT oUTER JOIN Size_Head f ON c.Size_Idno = f.Size_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Delivery_Sewing_Code LIKE '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Fabric_Delivery_Sewing_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Fabric_Delivery_Sewing_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Fabric_Delivery_Sewing_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Fabric_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Colour_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "##########0.00")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

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

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '-----
    End Sub

    Private Sub Cbo_JobNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "JobCard_Sewing_head", "JobCard_Code_FrSelec", "(Fabric_Delivery_Code = '')", "")

    End Sub

    Private Sub btn_BaleSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RollSelection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim FB_ID As Integer, Colr_ID As Integer, Procs_ID As Integer
        Dim NewCode As String
        ' Dim Fd_Perc As Integer
        Dim CompIDCondt As String
        Dim dgvDet_CurRow As Integer
        Dim dgv_DetSlNo As Long

        Try

            If dgv_Details.CurrentCell.RowIndex < 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                        dgv_Details.CurrentCell.Selected = True
                    End If
                End If
                Exit Sub
            End If

            FB_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value)
            If FB_ID = 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                        If cbo_grid_Fabric.Visible And cbo_grid_Fabric.Enabled Then cbo_grid_Fabric.Focus()
                        'dgv_Details.CurrentCell.Selected = True
                        Exit Sub
                    End If
                End If
                Exit Sub
            End If

            Colr_ID = Common_Procedures.Colour_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(2).Value)
            If Colr_ID = 0 Then
                MessageBox.Show("Invalid Colour Name ", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
                        If cbo_grid_Colour.Visible And cbo_grid_Colour.Enabled Then cbo_grid_Colour.Focus()
                        Exit Sub
                    End If
                End If
                Exit Sub
            End If


            Procs_ID = Common_Procedures.Process_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(3).Value)
            If Procs_ID = 0 Then
                MessageBox.Show("Invalid Process Name ", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                        If cbo_Grid_Process.Visible And cbo_Grid_Process.Enabled Then cbo_Grid_Process.Focus()
                        Exit Sub
                    End If
                End If
                Exit Sub
            End If

            CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
            If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
                CompIDCondt = ""
            End If

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
            dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(12).Value)

            With dgv_BaleSelection
                chk_SelectAll.Checked = False
                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("Select a.* from Processed_Fabric_inspection_Details a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.SalesInvoice_DetailsSlNo = " & Str(Val(dgv_DetSlNo)) & " and a.Fabric_IdNo = " & Str(Val(FB_ID)) & "  and a.Colour_IdNo = " & Str(Val(Colr_ID)) & "   order by a.Processed_Fabric_inspection_Date, a.for_orderby, a.Processed_Fabric_inspection_No, a.Processed_Fabric_inspection_Code", con)
                'old
                ' Da = New SqlClient.SqlDataAdapter("Select a.* from Processed_Fabric_inspection_Details a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.SalesInvoice_DetailsSlNo = " & Str(Val(dgv_DetSlNo)) & " and a.Fabric_IdNo = " & Str(Val(FB_ID)) & "  and a.Colour_IdNo = " & Str(Val(Colr_ID)) & "  and a.Process_IdNo = " & Str(Val(Procs_ID)) & " order by a.Processed_Fabric_inspection_Date, a.for_orderby, a.Processed_Fabric_inspection_No, a.Processed_Fabric_inspection_Code", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Roll_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Pcs_No").ToString
                        .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        .Rows(n).Cells(5).Value = "1"
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Roll_Code").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Roll_Or_Bundle").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("Select a.* from Processed_Fabric_inspection_Details a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Sales_Invoice_Code = '' and a.Fabric_IdNo = " & Str(Val(FB_ID)) & "  and a.Colour_IdNo = " & Str(Val(Colr_ID)) & "  order by a.Processed_Fabric_inspection_Date, a.for_orderby, a.Processed_Fabric_inspection_No, a.Processed_Fabric_inspection_Code", con)
                'old
                'Da = New SqlClient.SqlDataAdapter("Select a.* from Processed_Fabric_inspection_Details a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Sales_Invoice_Code = '' and a.Fabric_IdNo = " & Str(Val(FB_ID)) & "  and a.Colour_IdNo = " & Str(Val(Colr_ID)) & "  and a.Process_IdNo = " & Str(Val(Procs_ID)) & " order by a.Processed_Fabric_inspection_Date, a.for_orderby, a.Processed_Fabric_inspection_No, a.Processed_Fabric_inspection_Code", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Roll_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Pcs_No").ToString
                        .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Roll_Code").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Roll_Or_Bundle").ToString

                    Next

                End If
                Dt1.Clear()

            End With

            pnl_BaleSelection.Visible = True
            pnl_Back.Enabled = False
            dgv_BaleSelection.Focus()
            If dgv_BaleSelection.Rows.Count > 0 Then
                dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(0).Cells(0)
                dgv_BaleSelection.CurrentCell.Selected = True
            End If

        Catch ex As NullReferenceException
            MessageBox.Show("Select the ClothName for Bale Selection", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_BaleSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BaleSelection.CellClick
        Select_Bale(e.RowIndex)
    End Sub

    Private Sub Select_Bale(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_BaleSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then .Rows(RwIndx).Cells(5).Value = ""

                For i = 0 To .ColumnCount - 1
                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                Next

            End If

        End With

    End Sub

    Private Sub dgv_BaleSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BaleSelection.KeyDown
        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
                Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
                e.Handled = True
            End If
        End If

        If e.KeyCode = Keys.Delete Or e.KeyCode = Keys.Back Then
            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
                If Val(dgv_BaleSelection.Rows(dgv_BaleSelection.CurrentCell.RowIndex).Cells(5).Value) = 1 Then
                    e.Handled = True
                    Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
                End If
            End If
        End If

    End Sub

    Private Sub btn_Close_BaleSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_BaleSelection.Click
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim I As Integer, J As Integer
        Dim n As Integer
        Dim sno As Integer
        Dim dgvDet_CurRow As Integer = 0
        Dim dgv_DetSlNo As Integer = 0
        Dim NoofBls As Integer
        Dim FsNo As Single, LsNo As Single
        Dim FsBaleNo As String, LsBaleNo As String
        Dim BlNo As String, PackSlpCodes As String
        Dim Tot_Pcs As Single, Tot_Mtrs As Single, Tot_wGT As Single


        Cmd.Connection = con

        dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
        dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(12).Value)

        With dgv_BaleSelectionDetails


LOOP1:
            For I = 0 To .RowCount - 1

                If Val(.Rows(I).Cells(0).Value) = Val(dgv_DetSlNo) Then

                    If I = .Rows.Count - 1 Then
                        For J = 0 To .ColumnCount - 1
                            .Rows(I).Cells(J).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(I)

                    End If

                    GoTo LOOP1

                End If

            Next I


            'LOOP1:
            '            For I = 0 To .RowCount - 1

            '                If Val(.Rows(I).Cells(0).Value) = Val(dgv_DetSlNo) Then

            '                    If I = .Rows.Count - 1 Then
            '                        For J = 0 To .ColumnCount - 1
            '                            .Rows(I).Cells(J).Value = ""
            '                        Next

            '                    Else
            '                        .Rows.RemoveAt(I)

            '                    End If

            '                    'GoTo LOOP1

            '                End If

            '            Next I


            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()

            NoofBls = 0 : Tot_Pcs = 0 : Tot_Mtrs = 0 : Tot_wGT = 0 : BlNo = "" : PackSlpCodes = ""

            For I = 0 To dgv_BaleSelection.RowCount - 1

                If Val(dgv_BaleSelection.Rows(I).Cells(5).Value) = 1 Then

                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(0).Value = Val(dgv_DetSlNo)
                    .Rows(n).Cells(1).Value = dgv_BaleSelection.Rows(I).Cells(1).Value
                    .Rows(n).Cells(2).Value = Val(dgv_BaleSelection.Rows(I).Cells(2).Value)
                    .Rows(n).Cells(3).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(3).Value), "#########0.00")
                    .Rows(n).Cells(4).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(4).Value), "#########0.000")
                    .Rows(n).Cells(5).Value = dgv_BaleSelection.Rows(I).Cells(6).Value
                    cbo_RollBundle.Text = dgv_BaleSelection.Rows(0).Cells(7).Value

                    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) values ('" & Trim(dgv_BaleSelection.Rows(I).Cells(6).Value) & "', '" & Trim(dgv_BaleSelection.Rows(I).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_BaleSelection.Rows(I).Cells(1).Value))) & " ) "
                    Cmd.ExecuteNonQuery()

                    NoofBls = NoofBls + 1
                    Tot_Pcs = Val(Tot_Pcs) + Val(dgv_BaleSelection.Rows(I).Cells(2).Value)
                    Tot_Mtrs = Val(Tot_Mtrs) + Val(dgv_BaleSelection.Rows(I).Cells(3).Value)
                    Tot_wGT = Val(Tot_wGT) + Val(dgv_BaleSelection.Rows(I).Cells(4).Value)
                    PackSlpCodes = Trim(PackSlpCodes) & IIf(Trim(PackSlpCodes) = "", "~", "") & Trim(dgv_BaleSelection.Rows(I).Cells(6).Value) & "~"

                End If

            Next

            BlNo = ""
            FsNo = 0 : LsNo = 0
            FsBaleNo = "" : LsBaleNo = ""

            Da1 = New SqlClient.SqlDataAdapter("Select Name1 as Bale_Code, Name2 as Bale_No, Meters1 as fororderby_baleno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name2, Name1", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                FsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)
                LsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)

                FsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))
                LsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))

                For I = 1 To Dt1.Rows.Count - 1
                    If LsNo + 1 = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString) Then
                        LsNo = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString)
                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

                    Else
                        If FsNo = LsNo Then
                            BlNo = BlNo & Trim(FsBaleNo) & ","
                        Else
                            BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo) & ","
                        End If
                        FsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString
                        LsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString

                        FsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))
                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

                    End If

                Next

                If FsNo = LsNo Then BlNo = BlNo & Trim(FsBaleNo) Else BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo)

            End If
            Dt1.Clear()

            If Trim(dgv_Details.Rows(dgvDet_CurRow).Cells(13).Value) <> "" Then
                dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(10).Value = ""
            End If
            If Val(NoofBls) <> 0 And Val(Tot_Mtrs) <> 0 Then
                dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = NoofBls
                dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = BlNo
                If Val(Tot_Pcs) <> 0 Then
                    dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = Val(Tot_Pcs)
                End If
                dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = Format(Val(Tot_Mtrs), "#########0.00")
                dgv_Details.Rows(dgvDet_CurRow).Cells(8).Value = Format(Val(Tot_wGT), "#########0.000")
                dgv_Details.Rows(dgvDet_CurRow).Cells(13).Value = PackSlpCodes

            End If

            Add_NewRow_ToGrid()

            Total_Calculation()

        End With

        pnl_Back.Enabled = True
        pnl_BaleSelection.Visible = False
        If dgv_Details.Enabled And dgv_Details.Visible Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If dgv_Details.CurrentCell.RowIndex >= 0 Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(8)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
        End If

    End Sub

    Private Sub Add_NewRow_ToGrid()
        On Error Resume Next

        Dim i As Integer
        Dim n As Integer = -1

        With dgv_Details
            If .Visible Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    n = .Rows.Add()

                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = .Rows(.CurrentCell.RowIndex).Cells(i).Value
                        .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                    Next

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = i + 1
                    Next

                    .CurrentCell = .Rows(n).Cells(.CurrentCell.ColumnIndex)
                    .CurrentCell.Selected = True

                End If


            End If

        End With

    End Sub

    Private Sub dgtxt_details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyUp
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim n As Integer
        Dim vQTY As String = 0
        Dim vMTRPERQTY As String = 0
        Dim vWGTPERQTY As String = 0
        Dim vRECON_IN As String = ""


        Try
            With dgv_Details

                If .Rows.Count > 0 Then

                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                        dgv_Details_KeyUp(sender, e)
                    End If

                    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue And Trim(Common_Procedures.settings.CustomerCode) = "1061" Then
                        If (.CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5) Then
                            e.Handled = True
                            e.SuppressKeyPress = True
                            btn_BaleSelection_Click(sender, e)
                        End If
                    End If

                    If e.Control = False And (e.KeyValue = 8 Or e.KeyValue = 45 Or e.KeyValue = 46 Or (e.KeyValue >= 48 And e.KeyValue <= 57) Or (e.KeyValue >= 96 And e.KeyValue <= 105) Or e.KeyValue = 110 Or e.KeyValue = 127 Or e.KeyValue = 190) Then

                        If .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 17 Then

                            vRECON_IN = "METER"
                            Da4 = New SqlClient.SqlDataAdapter("Select Reconsilation_Meter_Weight from Processed_Item_Head Where Processed_Item_Name = '" & Trim(.CurrentRow.Cells(9).Value) & "'", con)
                            Dt4 = New DataTable
                            Da4.Fill(Dt4)
                            If Dt4.Rows.Count > 0 Then
                                vRECON_IN = Dt4.Rows(0).Item("Reconsilation_Meter_Weight").ToString
                            End If
                            Dt4.Clear()


                            If .CurrentCell.ColumnIndex = 10 Then

                                If Trim(UCase(vRECON_IN)) <> "WEIGHT" Then

                                    vQTY = 0
                                    If Val(.CurrentRow.Cells(10).Value) <> 0 Then
                                        vQTY = Format(Val(.CurrentRow.Cells(7).Value) / (.CurrentRow.Cells(10).Value), "##########0.000")
                                    End If

                                    If Val(vQTY) <> 0 Then
                                        .CurrentRow.Cells(11).Value = Math.Ceiling(Val(vQTY))
                                    Else
                                        .CurrentRow.Cells(11).Value = ""
                                    End If

                                Else

                                    .CurrentRow.Cells(10).Value = ""

                                End If

                            End If


                            If .CurrentCell.ColumnIndex = 17 Then

                                If Trim(UCase(vRECON_IN)) = "WEIGHT" Then

                                    vQTY = 0
                                    If Val(.CurrentRow.Cells(17).Value) <> 0 Then
                                        vQTY = Format(Val(.CurrentRow.Cells(8).Value) / (.CurrentRow.Cells(17).Value), "##########0.00")
                                    End If

                                    If Val(vQTY) <> 0 Then
                                        .CurrentRow.Cells(11).Value = Math.Ceiling(Val(vQTY))
                                    Else
                                        .CurrentRow.Cells(11).Value = ""
                                    End If

                                Else

                                    .CurrentRow.Cells(17).Value = ""

                                End If

                            End If

                            If .CurrentCell.ColumnIndex = 11 Then

                                If Trim(UCase(vRECON_IN)) = "WEIGHT" Then

                                    vWGTPERQTY = 0
                                    If Val(.CurrentRow.Cells(11).Value) <> 0 Then
                                        vWGTPERQTY = Format(Val(.CurrentRow.Cells(8).Value) / (.CurrentRow.Cells(11).Value), "##########0.000")
                                    End If

                                    If Val(vWGTPERQTY) <> 0 Then
                                        .CurrentRow.Cells(17).Value = vWGTPERQTY
                                    Else
                                        .CurrentRow.Cells(17).Value = ""
                                    End If

                                    .CurrentRow.Cells(10).Value = ""

                                Else

                                    vMTRPERQTY = 0
                                    If Val(.CurrentRow.Cells(11).Value) <> 0 Then
                                        vMTRPERQTY = Format(Val(.CurrentRow.Cells(7).Value) / (.CurrentRow.Cells(11).Value), "##########0.00")
                                    End If

                                    If Val(vMTRPERQTY) <> 0 Then
                                        .CurrentRow.Cells(10).Value = vMTRPERQTY
                                    Else
                                        .CurrentRow.Cells(10).Value = ""
                                    End If

                                    .CurrentRow.Cells(17).Value = ""

                                End If

                            End If

                        End If

                    End If

                End If
            End With

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub cbo_RollBundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RollBundle.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RollBundle, cbo_Ledger, cbo_Vechile, "", "", "", "")

        'If e.KeyValue = 40 Then
        '    If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
        '        cbo_ClothSales_OrderCode_forSelection.Focus()
        '    Else
        '        If dgv_Details.Rows.Count > 0 Then
        '            dgv_Details.Focus()
        '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

        '        Else
        '            btn_save.Focus()
        '        End If
        '    End If
        'End If

    End Sub

    Private Sub cbo_RollBundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RollBundle.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RollBundle, cbo_Vechile, "", "", "", "")
        'If Asc(e.KeyChar) = 13 Then

        '    If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
        '        cbo_ClothSales_OrderCode_forSelection.Focus()
        '    Else
        '        If dgv_Details.Rows.Count > 0 Then
        '            dgv_Details.Focus()
        '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

        '        Else
        '            btn_save.Focus()
        '        End If
        '    End If



        'End If
    End Sub
    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim pwd As String = ""

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> "TSSA7417" Then
            MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_dcNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_dcNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcNo.Text) & "/" & EntFnYrCode

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Fabric_Delivery_Sewing_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Delivery_Sewing_Code = '" & Trim(NewCode) & "'", con)
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

        prn_InpOpts = ""
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "123")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument1.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcNo.Text) & "/" & EntFnYrCode

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Fabric_Delivery_Sewing_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Sewing_IdNo = c.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Delivery_Sewing_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)



            If prn_HdDt.Rows.Count > 0 Then
                da2 = New SqlClient.SqlDataAdapter("select a.*, b.CLOTH_Name as Fabric_Name, c.Colour_Name ,d.Process_Name   from Fabric_Delivery_Sewing_Details a LEFT OUTER JOIN CLOTH_Head b on a.Fabric_Idno = b.CLOTH_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Process_Head d ON d.Process_IdNo = a.Process_IdNo   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Delivery_Sewing_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
        Dim p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim PrcsNm1 As String, PrcsNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer
        Dim clrName As String = ""
        Dim Clrln As Integer = 0
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
            .Left = 20
            .Right = 55
            .Top = 35
            .Bottom = 35
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

        NoofItems_PerPage = 7 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(40) : ClArr(2) = 200 : ClArr(3) = 110 : ClArr(4) = 120 : ClArr(5) = 60 : ClArr(6) = 55 : ClArr(7) = 75
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 16.5 '18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcNo.Text) & "/" & Trim(EntFnYrCode)

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

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Fabric_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        Dim ClrNm1 As String, ClrNm2 As String

                        ClrNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_Name").ToString)
                        ClrNm2 = ""
                        If Len(ClrNm1) > 12 Then
                            For I = 12 To 1 Step -1
                                If Mid$(Trim(ClrNm1), I, 1) = "@" Or Mid$(Trim(ClrNm1), I, 1) = " " Or Mid$(Trim(ClrNm1), I, 1) = "," Or Mid$(Trim(ClrNm1), I, 1) = "." Or Mid$(Trim(ClrNm1), I, 1) = "-" Or Mid$(Trim(ClrNm1), I, 1) = "/" Or Mid$(Trim(ClrNm1), I, 1) = "_" Or Mid$(Trim(ClrNm1), I, 1) = "(" Or Mid$(Trim(ClrNm1), I, 1) = ")" Or Mid$(Trim(ClrNm1), I, 1) = "\" Or Mid$(Trim(ClrNm1), I, 1) = "[" Or Mid$(Trim(ClrNm1), I, 1) = "]" Or Mid$(Trim(ClrNm1), I, 1) = "{" Or Mid$(Trim(ClrNm1), I, 1) = "}" Or Mid$(Trim(ClrNm1), I, 1) = "@" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            ClrNm2 = Microsoft.VisualBasic.Right(Trim(ClrNm1), Len(ClrNm1) - I)
                            ClrNm1 = Microsoft.VisualBasic.Left(Trim(ClrNm1), I)
                        End If

                        PrcsNm1 = prn_DetDt.Rows(prn_DetIndx).Item("Process_Name").ToString
                        PrcsNm2 = ""

                        If Len(PrcsNm1) > 12 Then
                            For I = 12 To 1 Step -1
                                If Mid$(Trim(PrcsNm1), I, 1) = "@" Or Mid$(Trim(PrcsNm1), I, 1) = " " Or Mid$(Trim(PrcsNm1), I, 1) = "," Or Mid$(Trim(PrcsNm1), I, 1) = "." Or Mid$(Trim(PrcsNm1), I, 1) = "-" Or Mid$(Trim(PrcsNm1), I, 1) = "/" Or Mid$(Trim(PrcsNm1), I, 1) = "_" Or Mid$(Trim(PrcsNm1), I, 1) = "(" Or Mid$(Trim(PrcsNm1), I, 1) = ")" Or Mid$(Trim(PrcsNm1), I, 1) = "\" Or Mid$(Trim(PrcsNm1), I, 1) = "[" Or Mid$(Trim(PrcsNm1), I, 1) = "]" Or Mid$(Trim(PrcsNm1), I, 1) = "{" Or Mid$(Trim(PrcsNm1), I, 1) = "}" Or Mid$(Trim(PrcsNm1), I, 1) = "@" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            PrcsNm2 = Microsoft.VisualBasic.Right(Trim(PrcsNm1), Len(PrcsNm1) - I)
                            PrcsNm1 = Microsoft.VisualBasic.Left(Trim(PrcsNm1), I)
                        End If


                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)

                        p1Font = New Font("Calibri", 8, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, ClrNm1, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(PrcsNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("No_of_Packs").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Or Trim(ClrNm2) <> "" Or Trim(PrcsNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ClrNm2, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont,, True, LMargin + ClArr(1) + ClArr(2) + ClArr(3))
                            Common_Procedures.Print_To_PrintDocument(e, Trim(PrcsNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont,, True, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))

                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

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

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Fabric_Name, c.Colour_Name ,e.Process_Name  from Fabric_Delivery_Sewing_Details a INNER JOIN Processed_Item_Head b on a.Fabric_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo  LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Process_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Delivery_Sewing_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 4 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

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

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)


        If Cmp_GSTIN_No <> "" Then
            CurY = CurY + TxtHgt - 1
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)



        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SEWING DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin + 10, CurY, 0, 0, pFont)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strHeight  ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("PROCESSING  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Fabric_Delivery_Sewing_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Fabric_Delivery_Sewing_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ROLL/BUNDLE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Roll_Bundle").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            End If


            'If Trim(prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "P.O.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "FABRIC NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PROCESS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Trim(prn_HdDt.Rows(0).Item("Roll_Bundle").ToString) <> "", Trim(UCase(prn_HdDt.Rows(0).Item("Roll_Bundle").ToString)), "ROLL"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 5
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
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim BLNo1 As String
        Dim BLNo2 As String
        Dim NoteStr1 As String = ""
        Dim NoteStr2 As String = ""



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

            If Val(prn_HdDt.Rows(0).Item("Total_No_of_Packs").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_No_of_Packs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
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

            CurY = CurY - 15



            vprn_BlNos = ""
            For I = 0 To prn_DetDt.Rows.Count - 1
                If Trim(prn_DetDt.Rows(I).Item("Packing_Nos").ToString) <> "" Then
                    vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Packing_Nos").ToString
                End If
            Next

            ' CurY = CurY + TxtHgt
            BLNo1 = Trim(vprn_BlNos)
            BLNo2 = ""
            If Len(BLNo1) > 90 Then
                For I = 90 To 1 Step -1
                    If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 90
                BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
                BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I)
            End If

            If Trim(BLNo1) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Pack Nos : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
            End If
            If Trim(BLNo2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Space(Len("Pack Nos : ")) & BLNo2, LMargin + 10, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("vehicle_No").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No : " & Trim(prn_HdDt.Rows(0).Item("vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Note : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

    Private Sub dgtxt_details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_details.TextChanged
        Try
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(sender.Text)
                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_grid_FpName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_grid_FpName.SelectedIndexChanged

    End Sub

    Private Sub get_FP_MeterPerQty_and_WeightPerQty(ByVal vFPNAME As String)
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim n As Integer
        Dim vQTY As String = 0
        Dim vMTRPERQTY As String = 0
        Dim vWGTPERQTY As String = 0
        Dim vRECON_IN As String = ""

        vGRDFPNAME_ENTRCEL = cbo_grid_FpName.Text

        vMTRPERQTY = 0
        vWGTPERQTY = 0
        vRECON_IN = "METER"
        Da4 = New SqlClient.SqlDataAdapter("Select Reconsilation_Meter_Weight, Meter_Qty , Weight_Piece from Processed_Item_Head Where Processed_Item_Name = '" & Trim(vFPNAME) & "'", con)
        Dt4 = New DataTable
        Da4.Fill(Dt4)
        If Dt4.Rows.Count > 0 Then
            vRECON_IN = Dt4.Rows(0).Item("Reconsilation_Meter_Weight").ToString
            vMTRPERQTY = Dt4.Rows(0).Item("Meter_Qty").ToString
            vWGTPERQTY = Dt4.Rows(0).Item("Weight_Piece").ToString
        End If
        Dt4.Clear()


        If Trim(UCase(vRECON_IN)) = "WEIGHT" Then

            If Val(vWGTPERQTY) <> 0 Then
                dgv_Details.CurrentRow.Cells(17).Value = vWGTPERQTY
            Else
                dgv_Details.CurrentRow.Cells(17).Value = ""
            End If
            dgv_Details.CurrentRow.Cells(17).ReadOnly = False

            dgv_Details.CurrentRow.Cells(10).Value = ""
            dgv_Details.CurrentRow.Cells(10).ReadOnly = True

        Else

            If Val(vMTRPERQTY) <> 0 Then
                dgv_Details.CurrentRow.Cells(10).Value = vMTRPERQTY
            Else
                dgv_Details.CurrentRow.Cells(10).Value = ""
            End If
            dgv_Details.CurrentRow.Cells(10).ReadOnly = False

            dgv_Details.CurrentRow.Cells(17).Value = ""
            dgv_Details.CurrentRow.Cells(17).ReadOnly = True

        End If

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If Asc(e.KeyChar) = 13 Then

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_Remarks.Focus()
            End If

        End If



    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_Vechile, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_Remarks.Focus()
            End If

        End If

        'If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
        '    cbo_RollBundle.Focus()
        'End If



    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Fabric_Receipt_Sewing_Head", "vehicle_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, cbo_RollBundle, Nothing, "Fabric_Receipt_Sewing_Head", "vehicle_No", "", "")
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
            With dgv_Details
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(1)
            End With

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