Public Class Processing_Receipt_Textile_Simple
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FPRRC-"
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

    Dim Process_Inputs As String
    Dim Process_Outputs As String
    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        txt_JobNo.Text = ""
        cbo_Ledger.Text = ""

        cbo_TransportName.Text = ""

        txt_Frieght.Text = ""
        txt_DelTo.Text = ""
        txt_filterpono.Text = ""


        dgv_Details.Rows.Clear()

        Grid_DeSelect()

        cbo_itemfp.Visible = False
        cbo_Colour.Visible = False
        cbo_LotNo.Visible = False
        cbo_Processing.Visible = False

        cbo_itemfp.Tag = -1
        cbo_Colour.Tag = -1
        cbo_LotNo.Tag = -1
        cbo_Processing.Tag = -1

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        txt_JobNo.Enabled = True
        txt_JobNo.BackColor = Color.White

        cbo_Colour.Enabled = True
        cbo_Colour.BackColor = Color.White

        cbo_itemfp.Enabled = True
        cbo_itemfp.BackColor = Color.White

        cbo_LotNo.Enabled = True
        cbo_LotNo.BackColor = Color.White

        chk_LotComplete.Checked = False

        cbo_Processing.Enabled = True
        cbo_Processing.BackColor = Color.White

        cbo_itemfp.Text = ""
        cbo_Colour.Text = ""
        cbo_LotNo.Text = ""
        cbo_Processing.Text = ""

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
        Dim chkbx As CheckBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is CheckBox Then
            chkbx = Me.ActiveControl
        End If

        If Me.ActiveControl.Name <> cbo_Colour.Name Then
            cbo_Colour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_itemfp.Name Then
            cbo_itemfp.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Processing.Name Then
            cbo_Processing.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_LotNo.Name Then
            cbo_LotNo.Visible = False
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
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Maroon
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Ledger_Name as Transport_Name from Textile_Processing_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo  Where a.ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RecNo.Text = dt1.Rows(0).Item("ClothProcess_Receipt_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("ClothProcess_Receipt_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                txt_JobNo.Text = dt1.Rows(0).Item("Job_No").ToString
                cbo_TransportName.Text = dt1.Rows(0).Item("Transport_Name").ToString
                txt_Frieght.Text = Format(Val(dt1.Rows(0).Item("Freight_Charges").ToString), "########0.00")
                txt_DelTo.Text = dt1.Rows(0).Item("DeliveryTo_Name").ToString

                If Val(dt1.Rows(0).Item("Lot_Status").ToString) = 1 Then
                    chk_LotComplete.Checked = True
                Else
                    chk_LotComplete.Checked = False
                End If

                da2 = New SqlClient.SqlDataAdapter("select a.*,C.Cloth_Name as Fp_Item_Name,d.Colour_Name,e.Lot_No,f.Process_Name from Textile_Processing_Receipt_Details a  INNER JOIN Cloth_Head C ON c.Cloth_Idno = a.Item_To_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Lot_Head e ON e.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno where a.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Dc_Rc_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Fp_Item_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Process_Name").ToString
                        dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Lot_No").ToString
                        dgv_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Receipt_Pcs").ToString)
                        dgv_Details.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Receipt_Qty").ToString)
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Receipt_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Receipt_Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("ExcSht_Meters").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Cloth_Processing_Delivery_Code").ToString
                        dgv_Details.Rows(n).Cells(12).Value = dt2.Rows(i).Item("Cloth_Processing_Delivery_Slno").ToString
                        dgv_Details.Rows(n).Cells(13).Value = dt2.Rows(i).Item("Cloth_Processing_Receipt_Slno").ToString
                        dgv_Details.Rows(n).Cells(14).Value = dt2.Rows(i).Item("Cloth_Processing_BillMaking_Code").ToString
                        dgv_Details.Rows(n).Cells(15).Value = dt2.Rows(i).Item("Processed_Fabric_Inspection_Code").ToString

                        If Trim(dgv_Details.Rows(n).Cells(14).Value) <> "" Then
                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True
                        End If

                        If Trim(dgv_Details.Rows(n).Cells(15).Value) <> "" Then
                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True
                        End If

                        'If IsDBNull(dt2.Rows(0).Item("Cloth_Processing_BillMaking_Code").ToString) = False Then
                        '    If Trim(dt2.Rows(0).Item("Cloth_Processing_BillMaking_Code").ToString) <> "" Then
                        '        LockSTS = True
                        '    End If
                        'End If

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(7).Value = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_ExcessShort").ToString), "########0.00")
                End With


                Grid_DeSelect()
                If LockSTS = True Then


                    cbo_Ledger.Enabled = False
                    cbo_Ledger.BackColor = Color.LightGray


                    If Trim(dgv_Details.Rows(n).Cells(15).Value) <> "" Then
                        txt_JobNo.Enabled = False
                        txt_JobNo.BackColor = Color.LightGray
                    End If

                    cbo_Colour.Enabled = False
                    cbo_Colour.BackColor = Color.LightGray

                    cbo_itemfp.Enabled = False
                    cbo_itemfp.BackColor = Color.LightGray

                    cbo_LotNo.Enabled = False
                    cbo_LotNo.BackColor = Color.LightGray

                    cbo_Processing.Enabled = False
                    cbo_Processing.BackColor = Color.LightGray

                    dgv_Details.AllowUserToAddRows = False

                End If
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

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Processing_Receipt_Textile_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_itemfp.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_itemfp.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_itemgrey.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "GREY ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_itemgrey.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LotNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_LotNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Processing.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PROCESS" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Processing.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Processing_Receipt_Textile_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable

        Me.Text = ""

        con.Open()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
            dgv_Details.Columns(7).Visible = False
            dgv_Details.Columns(9).Visible = False

            dgv_Details.Columns(6).Width = 70
            dgv_Details.Columns(8).Width = 110
            dgv_Details.Columns(2).Width = 200
            dgv_Details.Columns(3).Width = 120


            dgv_Details_Total.Columns(7).Visible = False
            dgv_Details_Total.Columns(9).Visible = False

            dgv_Details_Total.Columns(6).Width = 65
            dgv_Details_Total.Columns(8).Width = 110
            dgv_Details_Total.Columns(2).Width = 200
            dgv_Details_Total.Columns(3).Width = 120

        End If

        cbo_itemfp.Visible = False
        cbo_itemfp.Visible = False
        cbo_Colour.Visible = False
        cbo_LotNo.Visible = False
        cbo_Processing.Visible = False

        'dtp_Date.Text = ""
        'txt_PartyDcNo.Text = ""
        'cbo_Ledger.Text = ""
        'cbo_Ledger.Tag = ""
        'cbo_EndsCount.Text = ""

        'cbo_EndsCount.Text = ""
        'cbo_Cloth.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_itemfp.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Processing.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_filterpono.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Frieght.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_JobNo.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_LotComplete.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_ProcessName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_itemfp.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Processing.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_filterpono.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Frieght.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_JobNo.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_LotComplete.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_ProcessName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_JobNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DelTo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Frieght.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_JobNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DelTo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Frieght.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_filterpono.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_filterpono.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Processing_Receipt_Textile_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
        Common_Procedures.Hide_CurrentStock_Display()
    End Sub

    Private Sub Processing_Receipt_Textile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 6 Then
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(8)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(7)
                            End If

                        ElseIf .CurrentCell.ColumnIndex = 8 Then
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(10)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(9)
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_Frieght.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 6)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 8 Then
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(6)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(7)
                            End If

                        ElseIf .CurrentCell.ColumnIndex = 10 Then
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(8)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(9)
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
        Dim Dt2 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Da = New SqlClient.SqlDataAdapter("select count(*) from Textile_Processing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and  Cloth_Processing_BillMaking_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already BillMaking Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select count(*) from Textile_Processing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and  Processed_Fabric_Inspection_Code <> ''", con)
        Dt2 = New DataTable
        Da.Fill(Dt2)
        If Dt2.Rows.Count > 0 Then
            If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
                If Val(Dt2.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Inspection Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt2.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " & _
                                    " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters-b.ExcSht_Meters) , Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from Textile_Processing_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Textile_Processing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'"
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

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select process_name from process_head order by process_name", con)
            da.Fill(dt2)
            cbo_Filter_ProcessName.DataSource = dt2
            cbo_Filter_ProcessName.DisplayMember = "process_name"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ProcessName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ProcessName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ClothProcess_Receipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ClothProcess_Receipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothProcess_Receipt_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothProcess_Receipt_No desc", con)
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

        Try
            clear()

            New_Entry = True

            lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Processing_Receipt_Head", "ClothProcess_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RecNo.ForeColor = Color.Red

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter Rec.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(RecCode) & "'", con)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Rec No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select ClothProcess_Receipt_No from Textile_Processing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Rec No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RecNo.Text = Trim(UCase(inpno))

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
        Dim Led_ID As Integer = 0
        Dim Col_ID As Integer = 0
        Dim Itfp_ID As Integer = 0
        Dim Itgry_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""

        Dim PBlNo As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vtotqty As Single
        Dim Proc_ID As Integer = 0
        Dim Lot_ID As Integer = 0
        Dim vTotWeight As Single, vExcSrt As Single
        Dim Tr_ID As Integer = 0
        Dim WagesCode As String = ""
        Dim PcsChkCode As String = ""
        Dim Nr As Integer = 0

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim Delv_Mtr As Single = 0
        Dim Excs_Mtr_Rec As Single = 0
        Dim Rec_Mtr As Single = 0
        Dim Excs_Mtr_Retn As Single = 0
        Dim Allow_Sht_Perc As Single = 0
        Dim Retn_Mtr As Single = 0
        Dim Ent_Sht_Perc As Single = 0
        Dim Ent_Sht_Mtr As Single = 0
        Dim Allow_Sht_Mtr As Single = 0
        Dim lotSts As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, New_Entry) = False Then Exit Sub

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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Tr_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)

        lotSts = 0
        If chk_LotComplete.Checked = True Then lotSts = 1

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Or Val(.Rows(i).Cells(9).Value) <> 0 Or Val(.Rows(i).Cells(10).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid FP Item", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)

                        End If
                        Exit Sub

                    End If

                    If Trim(dgv_Details.Rows(i).Cells(3).Value) = "" Then
                        MessageBox.Show("Invalid COLOUR Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)

                        End If
                        Exit Sub

                    End If

                    If Trim(dgv_Details.Rows(i).Cells(4).Value) = "" Then
                        MessageBox.Show("Invalid PROCESS Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)

                        End If
                        Exit Sub

                    End If

                    If Val(dgv_Details.Rows(i).Cells(8).Value) = 0 Then
                        MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(8)
                        Exit Sub
                    End If

                End If

            Next
        End With

        Total_Calculation()
        vTotPcs = 0 : vTotMtrs = 0 : vTotWeight = 0 : vtotqty = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vtotqty = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            vTotWeight = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
            vExcSrt = Val(dgv_Details_Total.Rows(0).Cells(10).Value())
        End If

        Dt1.Clear()

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Processing_Receipt_Head", "ClothProcess_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@RecDate", dtp_Date.Value.Date)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()


            If New_Entry = True Then

                cmd.CommandText = "Insert into Textile_Processing_Receipt_Head (ClothProcess_Receipt_Code, Company_IdNo, ClothProcess_Receipt_No, for_OrderBy, ClothProcess_Receipt_Date, Ledger_IdNo, Job_No, Transport_IdNo, Freight_Charges, DeliveryTo_Name,Total_Pcs,Total_Qty, Total_Meters, Total_Weight ,Total_ExcessShort,Lot_Status ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate, " & Str(Val(Led_ID)) & ", '" & Trim(txt_JobNo.Text) & "', " & Str(Val(Tr_ID)) & ", " & Str(Val(txt_Frieght.Text)) & ",  '" & Trim(txt_DelTo.Text) & "'," & Str(Val(vTotPcs)) & "," & Val(vtotqty) & " , " & Str(Val(vTotMtrs)) & ", " & Str(Val(vTotWeight)) & " , " & Val(vExcSrt) & "," & Val(lotSts) & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Textile_Processing_Receipt_Head set ClothProcess_Receipt_Date = @RecDate, Ledger_IdNo = " & Val(Led_ID) & ", Job_No = '" & Trim(txt_JobNo.Text) & "' , Transport_IdNo = " & Val(Tr_ID) & ", Freight_Charges = " & Val(txt_Frieght.Text) & ", DeliveryTo_Name = '" & Trim(txt_DelTo.Text) & "', Total_Pcs = " & Val(vTotPcs) & " ,Total_Qty = " & Val(vtotqty) & " , Total_Meters = " & Val(vTotMtrs) & ",Total_Weight = " & Val(vTotWeight) & " ,Total_ExcessShort = " & Val(vExcSrt) & " ,Lot_Status = " & Val(lotSts) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = a.Receipt_Meters - (b.Receipt_Meters-b.ExcSht_Meters) , Receipt_Pcs = a.Receipt_Pcs - (b.Receipt_Pcs) , Receipt_Qty = a.Receipt_Qty - (b.Receipt_Qty) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight) from Textile_Processing_Delivery_Details a, Textile_Processing_Receipt_Details b Where b.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Delivery_code = b.Cloth_Processing_Delivery_code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " & _
                                       " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Textile_Processing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_receipt_Code = '" & Trim(NewCode) & "' and Cloth_Processing_BillMaking_Code = '' and Processed_Fabric_inspection_Code=''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Rec : Dc.No. " & Trim(lbl_RecNo.Text)
            PBlNo = Trim(lbl_RecNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)

            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(8).Value) <> 0 Or Val(.Rows(i).Cells(9).Value) <> 0 Or Val(.Rows(i).Cells(10).Value) <> 0 Then
                        Sno = Sno + 1
                        'itgry_id = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(13).Value, tr)
                        Itfp_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        Lot_ID = Common_Procedures.Lot_NoToIdNo(con, .Rows(i).Cells(5).Value, tr)
                        Proc_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)

                        Sno = Sno + 1

                        Nr = 0
                        cmd.CommandText = "Update Textile_Processing_Receipt_Details set Cloth_Processing_Receipt_Date = @RecDate , Sl_No = " & Str(Val(Sno)) & " , Dc_Rc_No = '" & Trim(.Rows(i).Cells(1).Value) & "' , Ledger_Idno = " & Val(Led_ID) & ", Item_Idno = " & Str(Val(.Rows(i).Cells(13).Value)) & " ,Item_To_Idno = " & Str(Val(Itfp_ID)) & ", Colour_Idno = " & Val(Col_ID) & ", Processing_Idno =  " & Val(Proc_ID) & ",Lot_IdNo = " & Val(Lot_ID) & " ,Receipt_Pcs =  " & Val(.Rows(i).Cells(6).Value) & ",Receipt_Qty = " & Val(.Rows(i).Cells(7).Value) & " ,Receipt_Meters = " & Str(Val(.Rows(i).Cells(8).Value)) & ",Receipt_Weight =" & Str(Val(.Rows(i).Cells(9).Value)) & "  ,ExcSht_Meters =  " & Str(Val(.Rows(i).Cells(10).Value)) & " ,  Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(11).Value) & "', Cloth_Processing_Delivery_Slno = " & Str(Val(.Rows(i).Cells(12).Value)) & "  where Company_IdNo =  " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and Cloth_Processing_Receipt_Slno = " & Str(Val(.Rows(i).Cells(13).Value)) & ""
                        cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Textile_Processing_Receipt_Details(Cloth_Processing_Receipt_Code, Company_IdNo, Cloth_Processing_Receipt_No, for_OrderBy, Cloth_Processing_Receipt_Date, Sl_No, Dc_Rc_No, Ledger_Idno, Item_Idno ,Item_To_Idno, Colour_Idno, Processing_Idno,Lot_IdNo,Receipt_Pcs,Receipt_Qty,Receipt_Meters,Receipt_Weight,ExcSht_Meters  , Cloth_Processing_Delivery_code , Cloth_Processing_Delivery_Slno ,Cloth_Processing_Receipt_Slno,Processed_Fabric_Inspection_Code) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate ,  " & Str(Val(.Rows(i).Cells(0).Value)) & " , '" & Trim(.Rows(i).Cells(1).Value) & "', " & Val(Led_ID) & " , " & Str(Val(.Rows(i).Cells(13).Value)) & ", " & Str(Val(Itfp_ID)) & ", " & Val(Col_ID) & ", " & Val(Proc_ID) & " ," & Val(Lot_ID) & " , " & Val(.Rows(i).Cells(6).Value) & ", " & Val(.Rows(i).Cells(7).Value) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & " ," & Str(Val(.Rows(i).Cells(10).Value)) & " , '" & Trim(.Rows(i).Cells(11).Value) & "', " & Str(Val(.Rows(i).Cells(12).Value)) & " , " & Str(Val(.Rows(i).Cells(13).Value)) & ",'" & Trim(.Rows(i).Cells(15).Value) & "')"
                            cmd.ExecuteNonQuery()
                        End If

                        Nr = 0
                        cmd.CommandText = "Update Textile_Processing_Delivery_Details set Receipt_Meters = Receipt_Meters + " & Str(Val(.Rows(i).Cells(8).Value) - Val(.Rows(i).Cells(10).Value)) & " , Receipt_Pcs = Receipt_Pcs + " & Str(Val(.Rows(i).Cells(6).Value)) & " , Receipt_Qty = Receipt_Qty + " & Str(Val(.Rows(i).Cells(7).Value)) & "  ,  Receipt_Weight = Receipt_Weight + " & Str(Val(.Rows(i).Cells(9).Value)) & "  Where Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(11).Value) & "' and Cloth_Processing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(12).Value)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                        Nr = cmd.ExecuteNonQuery()

                        'If Nr = 0 Then
                        '    Throw New ApplicationException("Mismatch of Order and Party Details")
                        'End If

                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                             , Reference_Date,     DeliveryTo_Idno     ,                            ReceivedFrom_Idno              ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No      ,           Cloth_Idno      ,   Folding  ,   Meters_Type1                       ,StockOff_IdNo ,Weight                                   ,Pcs                                      ,Colour_IdNo        , Process_IdNo        , Lot_IdNo ) " & _
                                               " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",  @RecDate,  " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "," & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Itfp_ID)) & " ,     0      , " & Str(Val(.Rows(i).Cells(8).Value)) & ",     4        ," & Str(Val(.Rows(i).Cells(9).Value)) & "," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Col_ID) & "," & Str(Proc_ID) & " ," & Str(Lot_ID) & " ) "
                        cmd.ExecuteNonQuery()

                        '-------------------------------------- -Allow Shortage

                        da1 = New SqlClient.SqlDataAdapter("select sum(a.Receipt_Meters) as Rec_Mtr, sum(a.ExcSht_Meters) as Exc_Mtr from Textile_Processing_Receipt_Details a  Where a.Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(11).Value) & "' and a.Cloth_Processing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(12).Value)) & "", con)
                        da1.SelectCommand.Transaction = tr
                        Dt1 = New DataTable
                        da1.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            Rec_Mtr = Val(Dt1.Rows(0).Item("Rec_Mtr").ToString)
                            '  Excs_Mtr_Rec = Dt1.Rows(0).Item("Exc_Mtr").ToString
                        End If
                        Dt1.Dispose()
                        da1.Dispose()

                        da2 = New SqlClient.SqlDataAdapter("select sum(a.Return_Meters) as Retn_Mtr, sum(a.ExcSht_Meters) as Exc_Mtr from Textile_Processing_Return_Details a  Where a.Cloth_Processing_Delivery_code = '" & Trim(.Rows(i).Cells(11).Value) & "' and a.Cloth_Processing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(12).Value)) & "", con)
                        da2.SelectCommand.Transaction = tr
                        dt2 = New DataTable
                        da2.Fill(dt2)
                        If dt2.Rows.Count > 0 Then
                            If IsDBNull(dt2.Rows(0).Item("Retn_Mtr").ToString) = False Then
                                Retn_Mtr = Val(dt2.Rows(0).Item("Retn_Mtr").ToString)
                            End If

                            ' Excs_Mtr_Retn = dt2.Rows(0).Item("Exc_Mtr").ToString
                        End If
                        dt2.Dispose()
                        da2.Dispose()

                        da1 = New SqlClient.SqlDataAdapter("select a.* , A.Receipt_Meters as Rec_Mtr_Bef , b.* , b.Receipt_Meters as Rec_Mtr ,b.Return_Meters as Retn_Mtr , C.* from Textile_Processing_Receipt_Details a LEFT OUTER JOIN Textile_Processing_Delivery_Details b On a.Cloth_Processing_Delivery_Code = b.Cloth_Processing_Delivery_Code and a.Cloth_Processing_Delivery_SlNo = b.Cloth_Processing_Delivery_SlNo INNER JOIN Cloth_Head c ON c.Cloth_Idno = b.Item_to_IdNo Where a.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and a.Cloth_Processing_Receipt_Slno = " & Str(Val(.Rows(i).Cells(13).Value)) & " And a.Sl_No = " & Str(Val(Sno)), con)
                        da1.SelectCommand.Transaction = tr
                        Dt1 = New DataTable
                        da1.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            Delv_Mtr = Dt1.Rows(0).Item("Delivery_Meters").ToString
                            Allow_Sht_Perc = Dt1.Rows(0).Item("Allow_Shortage_Perc").ToString
                        End If
                        Dt1.Dispose()
                        da1.Dispose()

                        Allow_Sht_Mtr = Delv_Mtr * Allow_Sht_Perc / 100
                        Ent_Sht_Mtr = Delv_Mtr - Rec_Mtr - Retn_Mtr


                        If Ent_Sht_Mtr > Allow_Sht_Mtr Then
                            '  MessageBox.Show("Invalid Shortage Meter" & Chr(13) & "Allowed Shortage (%): " & " " & Str(Val(Allow_Sht_Mtr)) & " "(" " & Str(Val(Allow_Sht_Perc)) & " ")"  " & Chr(13) & "Actual Shortage (%): " & " " & Str(Val(Ent_Sht_Mtr)) & "", "DOES NOT SAVE.!!!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If

                        '----------------------------------------

                    End If

                Next

            End With
            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno           , Item_IdNo, Rack_IdNo ) " & _
                                        " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_StockIdNo, Item_IdNo,     0        from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()
            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    'Private Sub Item_Grey()
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt As New DataTable
    '    Dim GITID As Integer



    '    GITID = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_itemgrey.Text)



    '    If GITID <> 0 Then

    '        Da = New SqlClient.SqlDataAdapter("select * from Processed_Item_Head where Processed_Item_IdNo = " & Str(Val(GITID)) & " and Processed_Item_Type= 'GREY' ", con)
    '        Da.Fill(Dt)


    '        If Dt.Rows.Count > 0 Then

    '            dgv_Details.CurrentRow.Cells(8).Value = Dt.Rows(0).Item("Meter_Qty").ToString

    '        End If


    '        Dt.Clear()
    '        Dt.Dispose()
    '        Da.Dispose()

    '    End If

    'End Sub

    Private Sub Total_Calculation()
        Dim vTotPcs As Single, vTotMtrs As Single, vtotweight As Single, vtotqty As Single, vExcsrt As Single

        Dim i As Integer
        Dim sno As Integer


        vTotPcs = 0 : vTotMtrs = 0 : vtotweight = 0 : sno = 0 : vExcsrt = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(9).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(10).Value) <> 0 Then
                    '.Rows(i).Cells(9).Value = Val(dgv_Details.Rows(i).Cells(7).Value) * Val(dgv_Details.Rows(i).Cells(8).Value)

                    vTotPcs = vTotPcs + Val(dgv_Details.Rows(i).Cells(6).Value)
                    vtotqty = vtotqty + Val(dgv_Details.Rows(i).Cells(7).Value)
                    vTotMtrs = vTotMtrs + Val(dgv_Details.Rows(i).Cells(8).Value)
                    vtotweight = vtotweight + Val(dgv_Details.Rows(i).Cells(9).Value)
                    vExcsrt = vExcsrt + Val(dgv_Details.Rows(i).Cells(10).Value)
                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(6).Value = Val(vTotPcs)
        dgv_Details_Total.Rows(0).Cells(7).Value = Val(vtotqty)
        dgv_Details_Total.Rows(0).Cells(8).Value = Format(Val(vTotMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(9).Value = Format(Val(vtotweight), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(10).Value = Format(Val(vExcsrt), "#########0.00")
    End Sub
    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, txt_DelTo, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_DelTo, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select order:", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                txt_DelTo.Focus()

            End If

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
    Private Sub dgv_Details_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellClick
        With dgv_Details
            If e.ColumnIndex = 8 Then
                Show_Item_CurrentStock(e.RowIndex)
                .Focus()
            End If
        End With
    End Sub
    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        With dgv_Details

            If .CurrentCell.ColumnIndex = 9 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 10 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
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
        Dim Dt4 As New DataTable
        Dim rect As Rectangle

        With dgv_Details
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


            If e.ColumnIndex = 2 Then

                If cbo_itemfp.Visible = False Or Val(cbo_itemfp.Tag) <> e.RowIndex Then

                    cbo_itemfp.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Type = 'FP' order by Processed_Item_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_itemfp.DataSource = Dt1
                    cbo_itemfp.DisplayMember = "Procesed_Item_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_itemfp.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_itemfp.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_itemfp.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_itemfp.Height = rect.Height  ' rect.Height

                    cbo_itemfp.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_itemfp.Tag = Val(e.RowIndex)
                    cbo_itemfp.Visible = True

                    cbo_itemfp.BringToFront()
                    cbo_itemfp.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_YarnType.Visible = False

                End If

            Else

                cbo_itemfp.Visible = False
                'cbo_Grid_YarnType.Tag = -1
                'cbo_Grid_YarnType.Text = ""

            End If

            If e.ColumnIndex = 3 Then

                If cbo_Colour.Visible = False Or Val(cbo_Colour.Tag) <> e.RowIndex Then

                    cbo_Colour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Colour.DataSource = Dt2
                    cbo_Colour.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Colour.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Colour.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Colour.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Colour.Height = rect.Height  ' rect.Height

                    cbo_Colour.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Colour.Tag = Val(e.RowIndex)
                    cbo_Colour.Visible = True

                    cbo_Colour.BringToFront()
                    cbo_Colour.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_Colour.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If


            If e.ColumnIndex = 4 Then

                If cbo_Processing.Visible = False Or Val(cbo_Processing.Tag) <> e.RowIndex Then

                    cbo_Processing.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Process_Name from Process_Head order by Process_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Processing.DataSource = Dt3
                    cbo_Processing.DisplayMember = "Process_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Processing.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Processing.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Processing.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Processing.Height = rect.Height  ' rect.Height

                    cbo_Processing.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Processing.Tag = Val(e.RowIndex)
                    cbo_Processing.Visible = True

                    cbo_Processing.BringToFront()
                    cbo_Processing.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_Processing.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If

            If e.ColumnIndex = 5 Then

                If cbo_LotNo.Visible = False Or Val(cbo_LotNo.Tag) <> e.RowIndex Then

                    cbo_LotNo.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Lot_No from Lot_Head order by Lot_No", con)
                    Dt4 = New DataTable
                    Da.Fill(Dt4)
                    cbo_LotNo.DataSource = Dt4
                    cbo_LotNo.DisplayMember = "Lot_No"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_LotNo.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_LotNo.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_LotNo.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_LotNo.Height = rect.Height  ' rect.Height

                    cbo_LotNo.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_LotNo.Tag = Val(e.RowIndex)
                    cbo_LotNo.Visible = True

                    cbo_LotNo.BringToFront()
                    cbo_LotNo.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_LotNo.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If

            If e.ColumnIndex = 8 And dgv_LevColNo <> 8 Then
                Show_Item_CurrentStock(e.RowIndex)
                .Focus()
            End If

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave

        With dgv_Details
            dgv_LevColNo = .CurrentCell.ColumnIndex
            If .CurrentCell.ColumnIndex = 9 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 10 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Then
                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown

        'On Error Resume Next
        On Error Resume Next

        With dgv_Details

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True
                    txt_Frieght.Focus()

                End If
            End If

            If e.KeyCode = Keys.Down Then
                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True
                    If .CurrentRow.Index = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            save_record()
                        End If
                    End If
                End If
            End If

        End With
    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub
    Private Sub dgtxt_details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyDown
        Try

            With dgv_Details

                If e.KeyValue = Keys.Delete Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
                        e.Handled = True
                    End If
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(15).Value) <> "" Then
                        e.Handled = True
                    End If
                End If
            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress
        Try


            With dgv_Details


                If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
                    e.Handled = True

                ElseIf Trim(.Rows(.CurrentCell.RowIndex).Cells(15).Value) <> "" Then
                    e.Handled = True

                Else

                    If Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 6 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 7 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 8 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 9 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 10 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End With
        Catch ex As Exception

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
    Private Sub cbo_LotNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LotNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

    End Sub

    Private Sub cbo_Lotno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LotNo, cbo_Processing, Nothing, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_lotno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LotNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LotNo, Nothing, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub


    Private Sub cbo_Lotno_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LotNo_creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LotNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_lotno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LotNo.TextChanged
        Try
            If cbo_LotNo.Visible Then
                With dgv_Details
                    If Val(cbo_LotNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 5 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_LotNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_itemfp, cbo_Processing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, cbo_Processing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub


    Private Sub cbo_colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Colour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_Colour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.TextChanged
        Try
            If cbo_Colour.Visible Then
                With dgv_Details
                    If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Processing_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Processing.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_Idno=0)")

    End Sub
    Private Sub cbo_processing_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Processing.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Processing, cbo_Colour, cbo_LotNo, "Process_Head", "Process_Name", "", "(Process_Idno=0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Processing.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Processing.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_processing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Processing.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Processing, cbo_LotNo, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_processing_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Processing.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Processing.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_Processing_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Processing.TextChanged
        Try
            If cbo_Processing.Visible Then
                With dgv_Details
                    If Val(cbo_Processing.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Processing.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_itemfp_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemfp.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")

    End Sub

    Private Sub cbo_itemfp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemfp.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_itemfp, Nothing, cbo_Colour, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_itemfp.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_itemfp.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_itemfp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_itemfp.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_itemfp, cbo_Colour, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_itemfp_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemfp.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_itemfp.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_itemfp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemfp.TextChanged
        Try
            If cbo_itemfp.Visible Then
                With dgv_Details
                    If Val(cbo_itemfp.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_itemfp.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim Led_IdNo As Integer, proc_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            proc_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothProcess_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.ClothProcess_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothProcess_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ProcessName.Text) <> "" Then
                proc_IdNo = Common_Procedures.Process_NameToIdNo(con, cbo_Filter_ProcessName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If


            If Val(proc_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "c.Processing_Idno = " & Str(Val(proc_IdNo)) & ")"
            End If

            If Trim(txt_filterpono.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Job_No = '" & Trim(txt_filterpono.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.*,d.Processed_Item_Name,e.Process_Name from Textile_Processing_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Textile_Processing_Receipt_Details c ON c.Cloth_Processing_Receipt_Code = a.ClothProcess_Receipt_Code INNER JOIN Processed_Item_Head d ON d.Processed_Item_IdNo = c.Item_To_IdNo LEFT OUTER JOIN Process_Head e ON c.Processing_Idno = e.Process_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothProcess_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.ClothProcess_Receipt_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("ClothProcess_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ClothProcess_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Process_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("total_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, txt_filterpono, cbo_Filter_ProcessName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ProcessName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ProcessName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ProcessName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Textile_Process_Head", "Process_name", "", "(Process_iDNO = 0)")

    End Sub
    Private Sub cbo_Filter_ProcessName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ProcessName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ProcessName, cbo_Filter_PartyName, btn_Filter_Show, "Textile_Process_Head", "Process_name", "", "(Process_iDNO = 0)")

    End Sub

    Private Sub cbo_Filter_ProcessName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ProcessName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ProcessName, btn_Filter_Show, "Textile_Process_Head", "Process_name", "", "(Process_iDNO = 0)")
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Textile_Processing_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'", con)
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


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,d.Ledger_Name as Transport_Name from Textile_Processing_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothProcess_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,e.Process_Name  from Textile_Processing_Receipt_Details a LEFT OUTER JOIN CLOTH_Head b on a.Item_To_Idno = b.CLOTH_Idno LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

        ClArr(1) = Val(35) : ClArr(2) = 160 : ClArr(3) = 100 : ClArr(4) = 100 : ClArr(5) = 120 : ClArr(6) = 70 : ClArr(7) = 80
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

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

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Grey_Name").ToString)
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
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Colour_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Lot_No").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Process_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Pcs").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)


                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,e.Process_Name  from Textile_Processing_Delivery_Details a INNER JOIN Processed_Item_Head b on a.Item_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Processing_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "PROCESSING RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strHeight + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString(" P.O.NO  : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothProcess_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothProcess_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Job_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " P.O.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Job_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME(FP)", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PROCESSING", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

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

            If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
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

            CurY = CurY + TxtHgt - 5
            If Val(prn_HdDt.Rows(0).Item("Transport_Name").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + 10, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(7) = CurY

            End If

            'If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 250, CurY, 0, 0, pFont)
            'End If


            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If


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

    Private Sub txt_Frieght_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Frieght.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub cbo_TransportName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_TransportName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, txt_DelTo, txt_JobNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transportname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportName, txt_JobNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_TransportName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_TransportName.Name
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


    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Qty As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim nr As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.* , b.* , b.item_Idno, e.Ledger_Name as Transportname,h.Receipt_Pcs As Ent_Pcs, h.Receipt_Meters as Ent_Mtrs, h.Receipt_Weight As Ent_Wgt, h.Receipt_Qty As Ent_Qty, g.Cloth_Name as Fp_Item_Name , I.Lot_No , j.Process_Name , k.Colour_Name  from Textile_Processing_Delivery_Head a INNER JOIN Textile_Processing_Delivery_Details b ON a.ClothProcess_Delivery_Code = b.Cloth_Processing_Delivery_Code INNER JOIN Cloth_Head g ON g.Cloth_Idno = b.Item_to_IdNo  LEFT OUTER JOIN Lot_Head i ON b.Lot_IdNo = i.Lot_IdNo LEFT OUTER JOIN Process_Head J ON J.Process_IdNo = b.Processing_Idno LEFT OUTER JOIN Colour_Head k ON b.Colour_IdNo = k.Colour_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Textile_Processing_Receipt_Details h ON h.Cloth_Processing_Receipt_Code = '" & Trim(NewCode) & "' and b.Cloth_Processing_Delivery_Code = h.Cloth_Processing_Delivery_Code and b.Cloth_Processing_Delivery_SlNo = h.Cloth_Processing_Delivery_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Delivery_Meters - b.Receipt_Meters - b.Return_Meters) > 0 or h.Receipt_Meters > 0 ) order by a.ClothProcess_Delivery_Date, a.for_orderby, a.ClothProcess_Delivery_No", con)
            Dt1 = New DataTable
            nr = Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()


                    Ent_Qty = 0
                    Ent_Wgt = 0
                    Ent_Pcs = 0
                    Ent_Mtrs = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
                        Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Qty").ToString) = False Then
                        Ent_Qty = Val(Dt1.Rows(i).Item("Ent_Qty").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Mtrs").ToString) = False Then
                        Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_Mtrs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Wgt").ToString) = False Then
                        Ent_Wgt = Val(Dt1.Rows(i).Item("Ent_Wgt").ToString)
                    End If

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cloth_Processing_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Cloth_Processing_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Fp_Item_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Colour_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Process_Name").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Lot_No").ToString
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Delivery_Pcs").ToString) - Val(Dt1.Rows(i).Item("Receipt_Pcs").ToString) - Val(Dt1.Rows(i).Item("Return_pcs").ToString) + Val(Ent_Pcs), "#########0.00")
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Delivery_Qty").ToString) - Val(Dt1.Rows(i).Item("Receipt_Qty").ToString) - Val(Dt1.Rows(i).Item("Return_Qty").ToString) + Val(Ent_Qty), "#########0.00")
                    .Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(i).Item("Delivery_Meters").ToString) - Val(Dt1.Rows(i).Item("Receipt_Meters").ToString) - Val(Dt1.Rows(i).Item("Return_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")
                    .Rows(n).Cells(10).Value = Format(Val(Dt1.Rows(i).Item("Delivery_Weight").ToString) - Val(Dt1.Rows(i).Item("Receipt_Weight").ToString) - Val(Dt1.Rows(i).Item("Return_Weight").ToString) + Val(Ent_Wgt), "#########0.000")

                    If Ent_Mtrs > 0 Then
                        .Rows(n).Cells(11).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(11).Value = ""

                    End If
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Purchase_OrderNo").ToString
                    .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Cloth_Processing_Delivery_code").ToString
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Cloth_Processing_Delivery_Slno").ToString

                    .Rows(n).Cells(16).Value = Ent_Pcs
                    .Rows(n).Cells(17).Value = Ent_Qty
                    .Rows(n).Cells(18).Value = Ent_Mtrs
                    .Rows(n).Cells(19).Value = Ent_Wgt
                    .Rows(n).Cells(20).Value = Val(Dt1.Rows(i).Item("Item_Idno").ToString)

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        pnl_Back.Visible = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(11).Value = (Val(.Rows(RwIndx).Cells(11).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(11).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(11).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

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

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Cloth_Invoice_Selection()
    End Sub

    Private Sub Cloth_Invoice_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(11).Value) = 1 Then

                If txt_JobNo.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(13).Value) <> "" Then
                        txt_JobNo.Text = Trim(dgv_Selection.Rows(i).Cells(13).Value)
                    End If
                End If

                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value

                dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(14).Value
                dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(15).Value
                dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(20).Value

                If Val(dgv_Selection.Rows(i).Cells(16).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(16).Value
                Else
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(17).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(17).Value
                Else
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(18).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(18).Value
                Else
                    dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(9).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(19).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(19).Value
                Else
                    dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(10).Value
                End If


            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Back.Visible = True
        pnl_Selection.Visible = False
        If txt_DelTo.Enabled And txt_DelTo.Visible Then txt_DelTo.Focus()

    End Sub
    Private Sub Show_Item_CurrentStock(ByVal Rw As Integer)
        Dim vItemID As Integer

        If Val(Rw) < 0 Then Exit Sub

        With dgv_Details

            vItemID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(Rw).Cells(2).Value)

            If Val(vItemID) = 0 Then Exit Sub

            If Val(vItemID) <> Val(.Tag) Then
                Common_Procedures.Show_ProcessedItem_CurrentStock_Display(con, Val(lbl_Company.Tag), Val(Common_Procedures.CommonLedger.Godown_Ac), vItemID)
                .Tag = Val(Rw)
            End If

        End With

    End Sub

    Private Sub chk_LotComplete_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_LotComplete.KeyDown
        If e.KeyCode = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub chk_LotComplete_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_LotComplete.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub
End Class