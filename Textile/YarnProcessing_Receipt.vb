Public Class YarnProcessing_Receipt
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "YNPRC-"
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

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black

        dtp_Date.Text = ""

        cbo_Ledger.Text = ""

        cbo_TransportName.Text = ""

        txt_Frieght.Text = ""
        txt_DelTo.Text = ""
        cbo_Filter_MillName.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        dgv_Details.Rows.Clear()

        Grid_DeSelect()

        cbo_Count.Visible = False
        cbo_Colour.Visible = False

        cbo_MillName.Visible = False

        cbo_Count.Tag = -1
        cbo_Colour.Tag = -1

        cbo_MillName.Tag = -1

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_Colour.Enabled = True
        cbo_Colour.BackColor = Color.White

        cbo_Count.Enabled = True
        cbo_Count.BackColor = Color.White

        chk_LotComplete.Checked = False

        cbo_MillName.Enabled = True
        cbo_MillName.BackColor = Color.White

        cbo_Count.Text = ""
        cbo_Colour.Text = ""

        cbo_MillName.Text = ""


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
        If Me.ActiveControl.Name <> cbo_Count.Name Then
            cbo_Count.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_MillName.Name Then
            cbo_MillName.Visible = False
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Ledger_Name as Transport_Name from YarnProcessing_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo  Where a.YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RecNo.Text = dt1.Rows(0).Item("YarnProcessing_Receipt_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("YarnProcessing_Receipt_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_TransportName.Text = dt1.Rows(0).Item("Transport_Name").ToString
                txt_Frieght.Text = Format(Val(dt1.Rows(0).Item("Freight_Charges").ToString), "########0.00")
                txt_DelTo.Text = dt1.Rows(0).Item("DeliveryTo_Name").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))


                If Val(dt1.Rows(0).Item("Lot_Status").ToString) = 1 Then
                    chk_LotComplete.Checked = True
                Else
                    chk_LotComplete.Checked = False
                End If

                da2 = New SqlClient.SqlDataAdapter("select a.*,C.Count_Name,d.Colour_Name,e.Mill_Name from YarnProcessing_Receipt_Details a  INNER JOIN Count_Head C ON c.Count_Idno = a.Count_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Mill_Head e ON e.Mill_IdNo = a.Mill_IdNo  where a.YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
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
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Mill_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Receipt_Bag").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Receipt_Cone").ToString)
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Receipt_Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("YarnProcessing_Delivery_Code").ToString
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("YarnProcessing_Delivery_Slno").ToString
                        dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("YarnProcessing_Receipt_Slno").ToString
                        dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("YarnProcessing_BillMaking_Code").ToString

                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Excess_Short_Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(13).Value = dt2.Rows(i).Item("LotCode_forSelection").ToString

                        If Trim(dgv_Details.Rows(n).Cells(11).Value) <> "" Then
                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True
                        End If


                        'If IsDBNull(dt2.Rows(0).Item("YarnProcessing_BillMaking_Code").ToString) = False Then
                        '    If Trim(dt2.Rows(0).Item("YarnProcessing_BillMaking_Code").ToString) <> "" Then
                        '        LockSTS = True
                        '    End If
                        'End If

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Bag").ToString)
                    .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Cone").ToString)
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Excess_Weight").ToString), "########0.000")

                End With


                Grid_DeSelect()
                If LockSTS = True Then

                    cbo_Ledger.Enabled = False
                    cbo_Ledger.BackColor = Color.LightGray

                    cbo_Colour.Enabled = False
                    cbo_Colour.BackColor = Color.LightGray

                    cbo_Count.Enabled = False
                    cbo_Count.BackColor = Color.LightGray

                    cbo_MillName.Enabled = False
                    cbo_MillName.BackColor = Color.LightGray

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

    Private Sub YarnProcessing_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_itemgrey.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "GREY ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_itemgrey.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub YarnProcessing_Receipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim Cmp_Name As String

        Me.Text = ""

        con.Open()

        cbo_Count.Visible = False
        cbo_Count.Visible = False
        cbo_Colour.Visible = False

        cbo_MillName.Visible = False

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User.Name))

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Frieght.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Yarn_LotNo.GotFocus, AddressOf ControlGotFocus


        AddHandler chk_LotComplete.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Yarn_LotNo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Frieght.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelTo.LostFocus, AddressOf ControlLostFocus

        AddHandler chk_LotComplete.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus



        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_DelTo.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Frieght.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_DelTo.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_Frieght.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress



        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0



        da = New SqlClient.SqlDataAdapter("select * from Company_Head Where Company_Idno <> 0 order by Company_Name", con)
        da.Fill(dt8)


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then

            Cmp_Name = dt8.Rows(0).Item("Company_Name").ToString



            dgv_Details.Columns(13).Visible = True


        Else

            dgv_Details.Columns(13).Visible = False
            dgv_Details.Columns(3).Width = 240
            dgv_Details.Columns(4).Width = 190
            dgv_Details_Total.Columns(3).Width = 240
            dgv_Details_Total.Columns(4).Width = 190

        End If

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub YarnProcessing_Receipt_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
        Common_Procedures.Hide_CurrentStock_Display()
    End Sub

    Private Sub YarnProcessing_Receipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            ' If .CurrentCell.ColumnIndex >= .ColumnCount - 5 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(2)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 7 Then
                            'ElseIf .CurrentCell.ColumnIndex = .ColumnCount - 7 Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(12)


                        ElseIf .CurrentCell.ColumnIndex = 12 Then

                            If .Columns(13).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(2)
                            End If



                        Else
                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                    save_record()
                                Else
                                    dtp_Date.Focus()
                                End If
                            Else

                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If
                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_Frieght.Focus()

                            ElseIf .CurrentCell.ColumnIndex = 1 Then

                                If .Columns(13).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(13)
                                Else
                                    .Focus()
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(12)
                                End If

                            Else
                                ' .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 5)
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                            End If


                        ElseIf .CurrentCell.ColumnIndex = 12 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(7)

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

        Da = New SqlClient.SqlDataAdapter("select count(*) from YarnProcessing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "' and  YarnProcessing_BillMaking_Code <> ''", con)
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


        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update YarnProcessing_Delivery_Details set Receipt_Bag = a.Receipt_Bag - (b.Receipt_Bag) , Receipt_Cone = a.Receipt_Cone - (b.Receipt_Cone) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight - b.Excess_Short_Weight) from YarnProcessing_Delivery_Details a, YarnProcessing_Receipt_Details b Where b.YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "' and a.YarnProcessing_Delivery_Code = b.YarnProcessing_Delivery_Code and a.YarnProcessing_Delivery_SlNo = b.YarnProcessing_Delivery_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from YarnProcessing_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from YarnProcessing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "'"
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
            cbo_Filter_CountName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 YarnProcessing_Receipt_No from YarnProcessing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, YarnProcessing_Receipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 YarnProcessing_Receipt_No from YarnProcessing_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, YarnProcessing_Receipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 YarnProcessing_Receipt_No from YarnProcessing_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, YarnProcessing_Receipt_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 YarnProcessing_Receipt_No from YarnProcessing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, YarnProcessing_Receipt_No desc", con)
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

            lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "YarnProcessing_Receipt_Head", "YarnProcessing_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

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

            Da = New SqlClient.SqlDataAdapter("select YarnProcessing_Receipt_No from YarnProcessing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code = '" & Trim(RecCode) & "'", con)
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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Rec No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select YarnProcessing_Receipt_No from YarnProcessing_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Cnt_ID As Integer = 0
        Dim Itgry_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""

        Dim PBlNo As String = ""
        Dim vTotBgs As Single, vTotCns As Single
        Dim Proc_ID As Integer = 0
        Dim Mill_ID As Integer = 0
        Dim vTotWeight As Single
        Dim vTotExcessWeight As Single
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
        Dim Usr_idNo As Single = 0
        Dim vLOT_ENT_REFCODE As String = ""


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
        lbl_UserName.Text = Val(UCase(Common_Procedures.User.IdNo))

        lotSts = 0
        If chk_LotComplete.Checked = True Then lotSts = 1

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If Trim(.Rows(i).Cells(2).Value) <> "" Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)

                        End If
                        Exit Sub

                    End If

                    If Trim(dgv_Details.Rows(i).Cells(3).Value) = "" Then
                        MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)

                        End If
                        Exit Sub

                    End If

                    If Trim(dgv_Details.Rows(i).Cells(4).Value) = "" Then
                        MessageBox.Show("Invalid Colour Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)

                        End If
                        Exit Sub

                    End If

                    If Val(dgv_Details.Rows(i).Cells(7).Value) = 0 Then
                        MessageBox.Show("Invalid Weight..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                        Exit Sub
                    End If


                    vLOT_ENT_REFCODE = ""
                    If Trim(.Rows(i).Cells(13).Value) <> "" Then
                        vLOT_ENT_REFCODE = Common_Procedures.YarnLotCodeSelection_To_LotEntryReferenceCode(con, .Rows(i).Cells(13).Value, tr)
                    End If


                End If

            Next

        End With

        Total_Calculation()
        vTotBgs = 0 : vTotCns = 0 : vTotWeight = 0 : vTotExcessWeight = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotBgs = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotCns = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotWeight = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotExcessWeight = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
        End If

        Dt1.Clear()

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "YarnProcessing_Receipt_Head", "YarnProcessing_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@RecDate", dtp_Date.Value.Date)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then

                cmd.CommandText = "Insert into YarnProcessing_Receipt_Head (YarnProcessing_Receipt_Code, Company_IdNo, YarnProcessing_Receipt_No, for_OrderBy, YarnProcessing_Receipt_Date, Ledger_IdNo, Transport_IdNo, Freight_Charges, DeliveryTo_Name,Total_Bag,Total_Cone,  Total_Weight ,Lot_Status , Total_Excess_Weight ,User_idNo) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate, " & Str(Val(Led_ID)) & ",  " & Str(Val(Tr_ID)) & ", " & Str(Val(txt_Frieght.Text)) & ",  '" & Trim(txt_DelTo.Text) & "'," & Str(Val(vTotBgs)) & "," & Val(vTotCns) & " ,  " & Str(Val(vTotWeight)) & " , " & Val(lotSts) & " , " & Val(vTotExcessWeight) & ",'" & Trim(lbl_UserName.Text) & "')"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update YarnProcessing_Receipt_Head set YarnProcessing_Receipt_Date = @RecDate, Ledger_IdNo = " & Val(Led_ID) & ", Transport_IdNo = " & Val(Tr_ID) & ", Freight_Charges = " & Val(txt_Frieght.Text) & ", DeliveryTo_Name = '" & Trim(txt_DelTo.Text) & "', Total_Bag= " & Val(vTotBgs) & " ,Total_Cone = " & Val(vTotCns) & " ,Total_Excess_Weight = " & Val(vTotExcessWeight) & ",  Total_Weight = " & Val(vTotWeight) & " ,Lot_Status = " & Val(lotSts) & " , User_IdNo = '" & Trim(lbl_UserName.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update YarnProcessing_Delivery_Details set Receipt_Bag = a.Receipt_Bag - (b.Receipt_Bag) , Receipt_Cone = a.Receipt_Cone - (b.Receipt_Cone) , Receipt_Weight = a.Receipt_Weight - (b.Receipt_Weight - b.Excess_Short_Weight) from YarnProcessing_Delivery_Details a, YarnProcessing_Receipt_Details b Where b.YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "' and a.YarnProcessing_Delivery_Code = b.YarnProcessing_Delivery_Code and a.YarnProcessing_Delivery_SlNo = b.YarnProcessing_Delivery_SlNo"
                cmd.ExecuteNonQuery()

            End If


            cmd.CommandText = "Delete from YarnProcessing_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "' and YarnProcessing_BillMaking_Code = '' "
            Nr = cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            Partcls = "Rec : Rec.No. " & Trim(lbl_RecNo.Text)
            PBlNo = Trim(lbl_RecNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)


            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(2).Value) <> "" Or Val(.Rows(i).Cells(7).Value) <> 0 Then
                        'Sno = Sno + 1

                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)
                        ' Proc_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)

                        Sno = Sno + 1

                        Nr = 0
                        cmd.CommandText = "Update YarnProcessing_Receipt_Details set YarnProcessing_Receipt_Date = @RecDate , Sl_No = " & Str(Val(Sno)) & " , Dc_Rc_No = '" & Trim(.Rows(i).Cells(1).Value) & "' , Ledger_Idno = " & Val(Led_ID) & ", Count_Idno = " & Str(Val(Cnt_ID)) & ", Mill_Idno = " & Val(Mill_ID) & ", Colour_Idno =  " & Val(Col_ID) & ",Receipt_Bag =  " & Val(.Rows(i).Cells(5).Value) & ",Receipt_Cone = " & Val(.Rows(i).Cells(6).Value) & " , Receipt_Weight =" & Str(Val(.Rows(i).Cells(7).Value)) & " , Excess_Short_Weight =" & Str(Val(.Rows(i).Cells(12).Value)) & "   ,YarnProcessing_Delivery_Code = '" & Trim(.Rows(i).Cells(8).Value) & "', YarnProcessing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(9).Value)) & " , LotCode_forSelection = '" & Trim(.Rows(i).Cells(13).Value) & "' , Lot_Entry_ReferenceCode = '" & Trim(vLOT_ENT_REFCODE) & "'  where Company_IdNo =  " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "' and YarnProcessing_Receipt_SlNo = " & Str(Val(.Rows(i).Cells(10).Value)) & ""
                        Nr = cmd.ExecuteNonQuery()


                        If Nr = 0 Then
                            cmd.CommandText = "Insert into YarnProcessing_Receipt_Details(YarnProcessing_Receipt_Code, Company_IdNo, YarnProcessing_Receipt_No, for_OrderBy, YarnProcessing_Receipt_Date, Sl_No, Dc_Rc_No, Ledger_Idno, Count_Idno,Mill_IdNo , Colour_Idno,Receipt_Bag,Receipt_Cone,Receipt_Weight, YarnProcessing_Delivery_Code , YarnProcessing_Delivery_SlNo ,YarnProcessing_Receipt_SlNo , Excess_Short_Weight , LotCode_forSelection , Lot_Entry_ReferenceCode ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate ,  " & Str(Val(Sno)) & " , '" & Trim(.Rows(i).Cells(1).Value) & "', " & Val(Led_ID) & " ,  " & Str(Val(Cnt_ID)) & ", " & Val(Mill_ID) & ", " & Val(Col_ID) & " , " & Val(.Rows(i).Cells(5).Value) & ", " & Val(.Rows(i).Cells(6).Value) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ",  '" & Trim(.Rows(i).Cells(8).Value) & "', " & Str(Val(.Rows(i).Cells(9).Value)) & " , " & Str(Val(.Rows(i).Cells(10).Value)) & " ,  " & Str(Val(.Rows(i).Cells(12).Value)) & " , '" & Trim(.Rows(i).Cells(13).Value) & "', '" & Trim(vLOT_ENT_REFCODE) & "')"
                            cmd.ExecuteNonQuery()
                        End If


                        Nr = 0
                        cmd.CommandText = "Update YarnProcessing_Delivery_Details set  Receipt_Bag = Receipt_Bag + " & Str(Val(.Rows(i).Cells(5).Value)) & " , Receipt_Cone = Receipt_Cone + " & Str(Val(.Rows(i).Cells(6).Value)) & "  ,  Receipt_Weight = Receipt_Weight + " & Str(Val(.Rows(i).Cells(7).Value)) & " - " & Str(Val(.Rows(i).Cells(12).Value)) & "    Where YarnProcessing_Delivery_Code = '" & Trim(.Rows(i).Cells(8).Value) & "' and YarnProcessing_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(9).Value)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                        Nr = cmd.ExecuteNonQuery()

                        'If Nr = 0 Then
                        '    Throw New ApplicationException("Mismatch of Order and Party Details")
                        'End If

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                             , Reference_Date,     DeliveryTo_Idno     ,                            ReceivedFrom_Idno           , DeliveryToIdno_ForParticulars ,          ReceivedFromIdno_ForParticulars,        Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No      ,        Count_Idno       ,  Yarn_Type,    Mill_IdNo          ,                      Bags                  ,                          Cones             , Weight                             ,              Colour_IdNo   ) " &
                                               " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ",  @RecDate,  " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "," & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "," & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & " ,    'MILL'      , " & Str(Val(Mill_ID)) & ",   " & Str(Val(.Rows(i).Cells(5).Value)) & "," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Col_ID) & ") "
                        Nr = cmd.ExecuteNonQuery()

                    End If


                Next

            End With


            If Val(vTotBgs) <> 0 Or Val(vTotCns) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate,  " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "," & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', 1, 0, 0, " & Str(Val(vTotBgs)) & ", " & Str(Val(vTotCns)) & ", '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()
            End If
            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()
            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub


    Private Sub Total_Calculation()
        Dim vTotBgs As Single, vTotCns As Single, vtotweight As Single, vtotExcessweight As Single

        Dim i As Integer
        Dim sno As Integer

        vTotBgs = 0 : vTotCns = 0 : vtotweight = 0 : vtotExcessweight = 0 : sno = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then

                    vTotBgs = vTotBgs + Val(dgv_Details.Rows(i).Cells(5).Value)
                    vTotCns = vTotCns + Val(dgv_Details.Rows(i).Cells(6).Value)
                    vtotweight = vtotweight + Val(dgv_Details.Rows(i).Cells(7).Value)
                    vtotExcessweight = vtotExcessweight + Val(dgv_Details.Rows(i).Cells(12).Value)

                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(5).Value = Val(vTotBgs)
        dgv_Details_Total.Rows(0).Cells(6).Value = Val(vTotCns)

        dgv_Details_Total.Rows(0).Cells(7).Value = Format(Val(vtotweight), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(8).Value = Format(Val(vtotExcessweight), "#########0.000")

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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        With dgv_Details


            If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 12 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
            Total_Calculation()
            If e.ColumnIndex = 5 Or e.ColumnIndex = 6 Then
                get_MillCount_Details()
            End If
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

            If Val(.Rows(e.RowIndex).Cells(10).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(10).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(10).Value = Val(.Rows(e.RowIndex - 1).Cells(10).Value) + 1
                End If
            End If
            If e.ColumnIndex = 2 Then

                If cbo_Count.Visible = False Or Val(cbo_Count.Tag) <> e.RowIndex Then

                    cbo_Count.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Count.DataSource = Dt1
                    cbo_Count.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Count.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Count.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Count.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Count.Height = rect.Height  ' rect.Height

                    cbo_Count.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Count.Tag = Val(e.RowIndex)
                    cbo_Count.Visible = True

                    cbo_Count.BringToFront()
                    cbo_Count.Focus()


                End If

            Else

                cbo_Count.Visible = False


            End If
            If e.ColumnIndex = 3 Then

                If cbo_MillName.Visible = False Or Val(cbo_MillName.Tag) <> e.RowIndex Then

                    cbo_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_MillName.DataSource = Dt3
                    cbo_MillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_MillName.Left = .Left + rect.Left
                    cbo_MillName.Top = .Top + rect.Top  '
                    cbo_MillName.Width = rect.Width
                    cbo_MillName.Height = rect.Height

                    cbo_MillName.Text = .CurrentCell.Value

                    cbo_MillName.Tag = Val(e.RowIndex)
                    cbo_MillName.Visible = True

                    cbo_MillName.BringToFront()
                    cbo_MillName.Focus()



                End If

            Else

                cbo_MillName.Visible = False


            End If


            If e.ColumnIndex = 4 Then

                If cbo_Colour.Visible = False Or Val(cbo_Colour.Tag) <> e.RowIndex Then

                    cbo_Colour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Colour.DataSource = Dt2
                    cbo_Colour.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Colour.Left = .Left + rect.Left
                    cbo_Colour.Top = .Top + rect.Top
                    cbo_Colour.Width = rect.Width
                    cbo_Colour.Height = rect.Height

                    cbo_Colour.Text = .CurrentCell.Value

                    cbo_Colour.Tag = Val(e.RowIndex)
                    cbo_Colour.Visible = True

                    cbo_Colour.BringToFront()
                    cbo_Colour.Focus()


                End If

            Else

                cbo_Colour.Visible = False


            End If



            If e.ColumnIndex = 13 Then

                If cbo_Grid_Yarn_LotNo.Visible = False Or Val(cbo_Grid_Yarn_LotNo.Tag) <> e.RowIndex Then

                    cbo_Grid_Yarn_LotNo.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select LotCode_forSelection from Yarn_Lot_Head " &
                                                      "where Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_Details.CurrentRow.Cells(2).Value & "') " &
                                                      " and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_Details.CurrentRow.Cells(3).Value & "') order by LotCode_forSelection", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_Yarn_LotNo.DataSource = Dt2
                    cbo_Grid_Yarn_LotNo.DisplayMember = "LotCode_forSelection"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Yarn_LotNo.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_Yarn_LotNo.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_Yarn_LotNo.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_Yarn_LotNo.Height = rect.Height  ' rect.Height

                    cbo_Grid_Yarn_LotNo.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_Yarn_LotNo.Tag = Val(e.RowIndex)
                    cbo_Grid_Yarn_LotNo.Visible = True

                    cbo_Grid_Yarn_LotNo.BringToFront()
                    cbo_Grid_Yarn_LotNo.Focus()

                End If

            Else

                cbo_Grid_Yarn_LotNo.Visible = False

            End If



            'If e.ColumnIndex = 8 And dgv_LevColNo <> 8 Then
            '    Show_Item_CurrentStock(e.RowIndex)
            '    .Focus()
            'End If

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave

        With dgv_Details
            dgv_LevColNo = .CurrentCell.ColumnIndex

            If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 12 Then
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
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 12 Then
                    Total_Calculation()
                End If
                If e.ColumnIndex = 5 Or e.ColumnIndex = 6 Then
                    get_MillCount_Details()
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
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(11).Value) <> "" Then
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


                If Trim(.Rows(.CurrentCell.RowIndex).Cells(11).Value) <> "" Then
                    e.Handled = True


                Else



                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
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

            If Val(.Rows(e.RowIndex).Cells(10).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(10).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(10).Value = Val(.Rows(e.RowIndex - 1).Cells(10).Value) + 1
                End If
            End If
        End With
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_MillName, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
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
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
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
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        Try
            If cbo_Colour.Visible Then
                With dgv_Details
                    If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_Idno=0)")

    End Sub
    Private Sub cbo_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillName, cbo_Count, cbo_Colour, "Mill_Head", "Mill_Name", "", "(Mill_Idno=0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_Idno=0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.TextChanged
        Try
            If cbo_MillName.Visible Then
                With dgv_Details
                    If Val(cbo_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_itemfp_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")

    End Sub

    Private Sub cbo_itemfp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, Nothing, cbo_Colour, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_itemfp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, cbo_Colour, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_itemfp_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_itemfp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.TextChanged
        Try
            If cbo_Count.Visible Then
                With dgv_Details
                    If Val(cbo_Count.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Count.Text)
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
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer, Mill_idno As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mill_idno = 0
            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.YarnProcessing_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.YarnProcessing_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.YarnProcessing_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Trim(cbo_Filter_MillName.Text) <> "" Then
                Mill_idno = Common_Procedures.Mill_NameToIdNo(con, cbo_Filter_MillName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "c.Count_Idno = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Val(Mill_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "c.Mill_Idno = " & Str(Val(Mill_idno)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.*,d.cOUNT_Name,e.Mill_Name from YarnProcessing_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN YarnProcessing_Receipt_Details c ON c.YarnProcessing_Receipt_Code = a.YarnProcessing_Receipt_Code INNER JOIN Count_Head d ON d.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head e ON c.Mill_Idno = e.Mill_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.YarnProcessing_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.YarnProcessing_Receipt_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("YarnProcessing_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("YarnProcessing_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Mill_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Bag").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("total_Cone").ToString)

                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_MillName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ProcessName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_name", "", "(Count_iDNO = 0)")

    End Sub
    Private Sub cbo_Filter_ProcessName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, btn_Filter_Show, "Count_Head", "Count_name", "", "(Count_iDNO = 0)")

    End Sub

    Private Sub cbo_Filter_ProcessName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, btn_Filter_Show, "Count_Head", "Count_name", "", "(Count_iDNO = 0)")
    End Sub
    Private Sub cbo_Filter_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_Idno = 0)")
    End Sub

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, dtp_Filter_ToDate, cbo_Filter_PartyName, "Mill_Head", "Mill_Name", "", "(Mill_Idno = 0)")
    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, cbo_Filter_PartyName, "Mill_Head", "Mill_Name", "", "(Mill_Idno = 0)")
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

            da1 = New SqlClient.SqlDataAdapter("select * from YarnProcessing_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "'", con)
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,d.Ledger_Name as TransportName from YarnProcessing_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Transport_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Colour_Name ,d.Mill_Name  from YarnProcessing_Receipt_Details a LEFT OUTER JOIN Count_Head b on a.Count_Idno = b.Count_Idno LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Mill_Head d ON d.Mill_IdNo = a.Mill_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

        ClArr(1) = Val(35) : ClArr(2) = 110 : ClArr(3) = 200 : ClArr(4) = 150 : ClArr(5) = 90 : ClArr(6) = 90
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

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
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
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Colour_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Bag").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Bag").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Cone").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Cone").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)


                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Colour_Name ,d.Mill_Name   from YarnProcessing_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Mill_Head d ON d.Mill_IdNo = a.Mill_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.YarnProcessing_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "YARNPROCESSING RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strHeight + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString(" Transport : ", pFont).Width
            S1 = e.Graphics.MeasureString("FROM  :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("YarnProcessing_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("YarnProcessing_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Transport", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

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

            If Val(prn_HdDt.Rows(0).Item("Total_bAG").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bag").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Cone").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cone").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
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
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

            'CurY = CurY + TxtHgt - 5
            'If Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString) <> "" Then

            '    Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + 10, CurY, 0, 0, pFont)


            '    CurY = CurY + TxtHgt + 10
            '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            '    LnAr(7) = CurY

            'End If

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

    Private Sub txt_Frieght_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Frieght.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Frieght_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Frieght.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_TransportName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_TransportName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, txt_DelTo, txt_Frieght, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transportname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportName, txt_Frieght, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
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
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bag As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Cone As Single = 0
        Dim Ent_Exc As Single = 0

        Dim nr As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompIDCondt = "(Company_Type <> 'UNACCOUNT')"
            End If
        End If


        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            cmd.Connection = con

            cmd.CommandText = "Truncate table entrytempsub"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into entrytempsub(name1, Int1, Int2, Int3, weight1, weight2) Select a.YarnProcessing_Delivery_Code , a.YarnProcessing_Delivery_SlNo, a.Delivery_Bag, a.Delivery_Cone, a.Delivery_Weight, 0 from YarnProcessing_Delivery_Details a Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.Delivery_Weight > 0 "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into entrytempsub(name1, Int1, Int2, Int3, weight1, weight2) Select a.YarnProcessing_Delivery_Code , a.YarnProcessing_Delivery_SlNo, -1*abs(a.Receipt_Bag), -1*abs(a.Receipt_Cone), -1*abs(a.Receipt_Weight), 0 from YarnProcessing_Receipt_Details a Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.Receipt_Weight <> 0 "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into entrytempsub(name1, Int1, Int2, Int3, weight1, weight2) Select a.YarnProcessing_Delivery_Code, a.YarnProcessing_Delivery_SlNo, abs(a.Receipt_Bag), abs(a.Receipt_Cone), abs(a.Receipt_Weight), abs(a.Receipt_Weight) from YarnProcessing_Receipt_Details a Where a.YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.Receipt_Weight > 0 "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Truncate table entrytemp"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into entrytemp(name1, Int1, Int2, Int3, weight1, weight2) Select name1, Int1, sum(Int2), sum(Int3), sum(weight1), sum(weight2)  from entrytempsub group by name1, Int1 Having sum(weight1) > 0 or sum(weight2) > 0 "
            cmd.ExecuteNonQuery()

            Da = New SqlClient.SqlDataAdapter("select a.* , b.* , tET.Int2 as Pending_Bag, tET.Int3 as Pending_Cone, tET.weight1 as Pending_Weight, e.Ledger_Name as Transportname, h.Receipt_Bag As Ent_Bag,  h.Receipt_Weight As Ent_Wgt, h.Excess_Short_Weight As Ent_exc, h.Receipt_Cone As Ent_COne, g.Count_Name, I.Mill_Name , k.Colour_Name  from YarnProcessing_Delivery_Head a INNER JOIN YarnProcessing_Delivery_Details b ON a.YarnProcessing_Delivery_Code = b.YarnProcessing_Delivery_Code INNER JOIN entrytemp tET ON tET.Name1 = b.YarnProcessing_Delivery_Code and tET.int1 = b.YarnProcessing_Delivery_SlNo and  (tET.weight1 > 0 or tET.weight2 > 0) INNER JOIN Count_Head g ON g.Count_Idno = b.Count_IdNo  LEFT OUTER JOIN Mill_Head i ON b.Mill_IdNo = i.Mill_IdNo  LEFT OUTER JOIN Colour_Head k ON b.Colour_IdNo = k.Colour_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN YarnProcessing_Receipt_Details h ON h.YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "' and b.YarnProcessing_Delivery_Code = h.YarnProcessing_Delivery_Code and b.YarnProcessing_Delivery_SlNo = h.YarnProcessing_Delivery_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and (tET.weight1 > 0 or tET.weight2 > 0) order by a.YarnProcessing_Delivery_Date, a.for_orderby, a.YarnProcessing_Delivery_No", con)
            'Da = New SqlClient.SqlDataAdapter("select a.* , b.* ,  e.Ledger_Name as Transportname, h.Receipt_Bag As Ent_Bag,  h.Receipt_Weight As Ent_Wgt,  h.Excess_Short_Weight As Ent_exc, h.Receipt_Cone As Ent_COne, g.Count_Name , I.Mill_Name , k.Colour_Name  from YarnProcessing_Delivery_Head a INNER JOIN YarnProcessing_Delivery_Details b ON a.YarnProcessing_Delivery_Code = b.YarnProcessing_Delivery_Code INNER JOIN Count_Head g ON g.Count_Idno = b.Count_IdNo  LEFT OUTER JOIN Mill_Head i ON b.Mill_IdNo = i.Mill_IdNo  LEFT OUTER JOIN Colour_Head k ON b.Colour_IdNo = k.Colour_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN YarnProcessing_Receipt_Details h ON h.YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "' and b.YarnProcessing_Delivery_Code = h.YarnProcessing_Delivery_Code and b.YarnProcessing_Delivery_SlNo = h.YarnProcessing_Delivery_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ( b.YarnProcessing_Delivery_SlNo IN (select z1.int1 from entrytemp z1 ) ) order by a.YarnProcessing_Delivery_Date, a.for_orderby, a.YarnProcessing_Delivery_No", con)
            'Da = New SqlClient.SqlDataAdapter("select a.* , b.* ,  e.Ledger_Name as Transportname,h.Receipt_Bag As Ent_Bag,  h.Receipt_Weight As Ent_Wgt,  h.Excess_Short_Weight As Ent_exc, h.Receipt_Cone As Ent_COne, g.Count_Name , I.Mill_Name , k.Colour_Name  from YarnProcessing_Delivery_Head a INNER JOIN YarnProcessing_Delivery_Details b ON a.YarnProcessing_Delivery_Code = b.YarnProcessing_Delivery_Code INNER JOIN Count_Head g ON g.Count_Idno = b.Count_IdNo  LEFT OUTER JOIN Mill_Head i ON b.Mill_IdNo = i.Mill_IdNo  LEFT OUTER JOIN Colour_Head k ON b.Colour_IdNo = k.Colour_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN YarnProcessing_Receipt_Details h ON h.YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "' and b.YarnProcessing_Delivery_Code = h.YarnProcessing_Delivery_Code and b.YarnProcessing_Delivery_SlNo = h.YarnProcessing_Delivery_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Delivery_Weight - b.Receipt_Weight ) > 0 or h.Receipt_Weight > 0 ) order by a.YarnProcessing_Delivery_Date, a.for_orderby, a.YarnProcessing_Delivery_No", con)
            Dt1 = New DataTable
            nr = Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    Ent_Bag = 0
                    Ent_Wgt = 0
                    Ent_Cone = 0
                    Ent_Exc = 0
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Bag").ToString) = False Then
                        Ent_Bag = Val(Dt1.Rows(i).Item("Ent_Bag").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Cone").ToString) = False Then
                        Ent_Cone = Val(Dt1.Rows(i).Item("Ent_Cone").ToString)
                    End If

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Wgt").ToString) = False Then
                        Ent_Wgt = Val(Dt1.Rows(i).Item("Ent_Wgt").ToString)
                    End If

                    If IsDBNull(Dt1.Rows(i).Item("Ent_exc").ToString) = False Then
                        Ent_Exc = Val(Dt1.Rows(i).Item("Ent_exc").ToString)
                    End If

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("YarnProcessing_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("YarnProcessing_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Mill_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Colour_Name").ToString

                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Pending_Bag").ToString), "#########0")
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Pending_Cone").ToString), "#########0")
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Pending_Weight").ToString), "#########0.000")

                    '.Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Delivery_Bag").ToString) - Val(Dt1.Rows(i).Item("Receipt_Bag").ToString) + Val(Ent_Bag), "#########0.00")
                    '.Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Delivery_Cone").ToString) - Val(Dt1.Rows(i).Item("Receipt_Cone").ToString) + Val(Ent_Cone), "#########0.00")
                    '.Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Delivery_Weight").ToString) - Val(Dt1.Rows(i).Item("Receipt_Weight").ToString) + Val(Ent_Wgt) - Val(Ent_Exc), "#########0.000")

                    If Ent_Wgt > 0 Then
                        .Rows(n).Cells(9).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(9).Value = ""

                    End If
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("TransportName").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("YarnProcessing_Delivery_Code").ToString
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("YarnProcessing_Delivery_SlNo").ToString

                    .Rows(n).Cells(13).Value = Ent_Bag
                    .Rows(n).Cells(14).Value = Ent_Cone
                    .Rows(n).Cells(15).Value = Ent_Wgt
                    .Rows(n).Cells(16).Value = Ent_Exc

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        '  pnl_Back.Visible = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(9).Value = (Val(.Rows(RwIndx).Cells(9).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(9).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(9).Value = ""

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
        YarnDelivery_Selection()
    End Sub

    Private Sub YarnDelivery_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(9).Value) = 1 Then


                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value

                cbo_TransportName.Text = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(11).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(12).Value

                If Val(dgv_Selection.Rows(i).Cells(13).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(13).Value
                Else
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(14).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(14).Value
                Else
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(15).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(15).Value
                Else
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If
                If Val(dgv_Selection.Rows(i).Cells(16).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(16).Value
                End If

            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        '  pnl_Back.Visible = True
        pnl_Selection.Visible = False
        If txt_DelTo.Enabled And txt_DelTo.Visible Then txt_DelTo.Focus()

    End Sub

    Private Sub chk_LotComplete_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_LotComplete.KeyDown
        If e.KeyCode = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
            dgv_Details.CurrentCell.Selected = True
        End If
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub chk_LotComplete_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_LotComplete.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub
    Private Sub get_MillCount_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As Single
        Dim Wgt_Bag As Single
        Dim Wgt_Cn As Single
        Dim CntID As Integer
        Dim MilID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(3).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_Details

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        Wgt_Bag = Dt.Rows(0).Item("Weight_Bag").ToString
                        Wgt_Cn = Dt.Rows(0).Item("Weight_Cone").ToString
                        Cn_bag = Dt.Rows(0).Item("Cones_Bag").ToString
                    End If
                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                    If .CurrentCell.ColumnIndex = 5 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = .Rows(.CurrentRow.Index).Cells(5).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(7).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(5).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 6 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(7).Value = Format(.Rows(.CurrentRow.Index).Cells(6).Value * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

    End Sub

    Private Sub dgtxt_details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Yarn_Lot_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Yarn_LotNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Yarn_LotNo, Nothing, "Yarn_Lot_Head", "LotCode_forSelection", "Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_Details.CurrentRow.Cells(2).Value & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_Details.CurrentRow.Cells(3).Value & "')", "(Lot_No = '')")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                If .CurrentCell.ColumnIndex < .ColumnCount - 1 Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    If .CurrentRow.Index < .RowCount - 1 Then
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)
                    End If
                End If

            End With

        End If

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Yarn_LotNo, Nothing, Nothing, "Yarn_Lot_Head", "LotCode_forSelection", "Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_Details.CurrentRow.Cells(2).Value & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_Details.CurrentRow.Cells(3).Value & "')", "(Lot_No = '')")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Yarn_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Yarn_LotNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                If .CurrentCell.ColumnIndex < .ColumnCount - 1 Then
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                Else
                    If .CurrentRow.Index < .RowCount - 1 Then
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_Enter(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_LotNo.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Lot_Head", "LotCode_forSelection", "Count_IdNo = (Select Count_IdNo from Count_Head where Count_Name = '" & dgv_Details.CurrentRow.Cells(2).Value & "') and Mill_IdNo = (Select Mill_IdNo from Mill_Head where Mill_Name = '" & dgv_Details.CurrentRow.Cells(3).Value & "')", "(Lot_No = '')")
    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_LotNo.TextChanged
        Try
            If cbo_Grid_Yarn_LotNo.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                With dgv_Details
                    If Val(cbo_Grid_Yarn_LotNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 13 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(13).Value = Trim(cbo_Grid_Yarn_LotNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class