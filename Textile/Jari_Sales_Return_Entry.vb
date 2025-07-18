Public Class Jari_Sales_Return_Entry

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "JASDR-"
    Private Pk_Condition1 As String = "JDRFR-"
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
    Private WithEvents dgtxt_BobinDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_KuriDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActCtrlName As String = ""
    Private dgv_LevColNo As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1


        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""

        cbo_Ledger.Text = ""
        cbo_VechileNo.Text = ""
        cbo_Transport.Text = ""
        txt_Freight.Text = ""
        cbo_SalesAcc.Text = ""
        lbl_Net_Amt.Text = ""
        txt_Remarks.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        dgv_KuriDetails.Rows.Clear()
        dgv_KuriDetails_Total.Rows.Clear()
        dgv_KuriDetails_Total.Rows.Add()
        Grid_DeSelect()


        cbo_KuriCount.Visible = False
        cbo_KuriCount.Tag = -1
        cbo_KuriColour.Visible = False
        cbo_KuriColour.Tag = -1
        cbo_KuriBorderSize.Visible = False
        cbo_KuriBorderSize.Tag = -1


        cbo_KuriCount.Text = ""
        cbo_KuriColour.Text = ""
        cbo_KuriBorderSize.Text = ""

        'dgv_Details.Tag = ""
        'dgv_LevColNo = -1


        dgv_ActCtrlName = ""

        chk_NoStockPosting.Checked = False

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_KuriDetails.CurrentCell) Then dgv_KuriDetails.CurrentCell.Selected = False
        '   If Not IsNothing(dgv_KuriDetails.CurrentCell.Selected) = False Then Exit Sub
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


        If Me.ActiveControl.Name <> cbo_KuriCount.Name Then
            cbo_KuriCount.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_KuriColour.Name Then
            cbo_KuriColour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_KuriBorderSize.Name Then
            cbo_KuriBorderSize.Visible = False
        End If


        If Me.ActiveControl.Name <> dgv_KuriDetails.Name Then
            Grid_DeSelect()
        End If

        'If Me.ActiveControl.Name <> dgv_BobinDetails.Name Then
        '    Common_Procedures.Hide_CurrentStock_Display()
        'End If

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
        dgv_KuriDetails.CurrentCell.Selected = False

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
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Jari_SalesDelivery_Return_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo   Where a.Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_DcNo.Text = dt1.Rows(0).Item("Jari_SalesDelivery_Return_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Jari_SalesDelivery_Return_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_VechileNo.Text = dt1.Rows(0).Item("Vechile_No").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight").ToString), "########0.00")
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                lbl_Net_Amt.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")
                If Val(dt1.Rows(0).Item("No_Stock_Posting").ToString) = 1 Then
                    chk_NoStockPosting.Checked = True
                Else
                    chk_NoStockPosting.Checked = False
                End If
                cbo_SalesAcc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Colour_Name, d.BorderSize_Name from Jari_SalesDelivery_Jari_Return_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN BorderSize_Head d ON a.BorderSize_IdNo = d.BorderSize_IdNo where a.Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_KuriDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_KuriDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_KuriDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_KuriDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_KuriDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_KuriDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("BorderSize_Name").ToString
                        dgv_KuriDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Noof_Jumbos").ToString)
                        If Val(dgv_KuriDetails.Rows(n).Cells(4).Value) = 0 Then
                            dgv_KuriDetails.Rows(n).Cells(4).Value = ""
                        End If
                        dgv_KuriDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Noof_Cones").ToString)
                        If Val(dgv_KuriDetails.Rows(n).Cells(5).Value) = 0 Then
                            dgv_KuriDetails.Rows(n).Cells(5).Value = ""
                        End If
                        dgv_KuriDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Gross_Weight").ToString), "########0.000")
                        dgv_KuriDetails.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Tare_Weight").ToString), "########0.000")
                        dgv_KuriDetails.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Net_Weight").ToString), "########0.000")
                        dgv_KuriDetails.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_KuriDetails.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                       
                    Next i

                End If

                With dgv_KuriDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Jumbos").ToString)
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_GrossWeight").ToString), "########0.000")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_TareWeight").ToString), "########0.000")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_NetWeight").ToString), "########0.000")
                    .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.000")
                End With

            End If
            dt1.Clear()


            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dgv_ActCtrlName = ""
            dt1.Dispose()
            da1.Dispose()
            dt2.Dispose()
            da2.Dispose()

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Jari_Sales_Return_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesAcc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SalesAcc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_KuriCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_KuriCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_KuriColour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_KuriColour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_KuriBorderSize.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BORDERSIZE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_KuriBorderSize.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            '----MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Jari_Sales_Return_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Me.Text = ""

        con.Open()


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_KuriColour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_KuriCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_KuriBorderSize.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAcc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VechileNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_EndsName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_KuriCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_KuriColour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAcc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_KuriBorderSize.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VechileNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_EndsName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Remarks.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Jari_Sales_Return_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
        'Common_Procedures.Hide_CurrentStock_Display()
    End Sub

    Private Sub Jari_Sales_Return_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

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

        If ActiveControl.Name = dgv_KuriDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_KuriDetails.Name Then
                dgv1 = dgv_KuriDetails

            ElseIf dgv_KuriDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_KuriDetails

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_KuriDetails.Name.ToString)) Then
                dgv1 = dgv_KuriDetails

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                'If dgv1.Name = dgv_BobinDetails.Name Then
                                '    txt_PartyBobin.Focus()
                                'Else
                                txt_Remarks.Focus()
                                ' End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 7 Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(9)

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If dgv1.Name = dgv_KuriDetails.Name Then
                                    txt_Freight.Focus()
                                Else
                                    If dgv_KuriDetails.Rows.Count > 0 Then
                                        dgv_KuriDetails.Focus()
                                        dgv_KuriDetails.CurrentCell = dgv_KuriDetails.Rows(0).Cells(1)
                                        dgv_KuriDetails.CurrentCell.Selected = True
                                    Else
                                        txt_Freight.Focus()
                                    End If

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 9 Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(7)
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
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Jari_Sales_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Jari_Sales_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Jari_Sales_Return_Entry, New_Entry, Me, con, "Jari_SalesDelivery_Return_Head", "Jari_SalesDelivery_Return_Code", NewCode, "Jari_SalesDelivery_Return_Date", "(Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        'Da = New SqlClient.SqlDataAdapter("select BobinSales_Invoice_Code from Jari_SalesDelivery_Return_Head Where Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "'", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)

        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0).Item("BobinSales_Invoice_Code").ToString) = False Then
        '        If Trim(Dt1.Rows(0).Item("BobinSales_Invoice_Code").ToString) <> "" Then
        '            MessageBox.Show("Already Invoiced", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '            Exit Sub

        '        End If
        '    End If
        'End If
        'Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Jari_SalesDelivery_Return_Head", "Jari_SalesDelivery_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Jari_SalesDelivery_Return_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Jari_SalesDelivery_Return_Details", "Jari_SalesDelivery_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Count_IdNo, Colour_IdNo, BorderSize_IdNo, Noof_Jumbos, Noof_Cones,Gross_Weight  ,   Tare_Weight    ,  Net_Weight    , Rate,Amount", "Sl_No", "Jari_SalesDelivery_Return_Code, For_OrderBy, Company_IdNo, Jari_SalesDelivery_Return_No, Jari_SalesDelivery_Return_Date, Ledger_Idno", trans)

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Jari_Sales_Delivery_Bobin_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Jari_SalesDelivery_Jari_Return_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Jari_SalesDelivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select EndsCount_name from EndsCount_head order by EndsCount_name", con)
            da.Fill(dt2)
            cbo_Filter_EndsName.DataSource = dt2
            cbo_Filter_EndsName.DisplayMember = "EndsCount_name"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_EndsName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_EndsName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Jari_SalesDelivery_Return_No from Jari_SalesDelivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Jari_SalesDelivery_Return_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Jari_SalesDelivery_Return_No from Jari_SalesDelivery_Return_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Jari_SalesDelivery_Return_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Jari_SalesDelivery_Return_No from Jari_SalesDelivery_Return_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Jari_SalesDelivery_Return_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Jari_SalesDelivery_Return_No from Jari_SalesDelivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Jari_SalesDelivery_Return_No desc", con)
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

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Jari_SalesDelivery_Return_Head", "Jari_SalesDelivery_Return_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Jari_SalesDelivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Jari_SalesDelivery_Return_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Jari_SalesDelivery_Return_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Jari_SalesDelivery_Return_Date").ToString
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

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Jari_SalesDelivery_Return_No from Jari_SalesDelivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code = '" & Trim(RecCode) & "'", con)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Jari_Sales_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Jari_Sales_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Jari_Sales_Return_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW DELIVERY INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Jari_SalesDelivery_Return_No from Jari_SalesDelivery_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Ens_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim Cnt_ID As Integer = 0
        Dim Delv_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim Siz_ID As Integer = 0
        Dim Clr_ID As Integer = 0
        Dim BthSz_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim vEdsCnt_ID As Integer = 0
        Dim PBlNo As String = ""
        Dim vTotNetWgt As Single, vTotTareWgt As Single, vTotAmt As Single
        Dim vTotJumbo As Single, vTotCns As Single, vTotGrsWgt As Single
        Dim Nr As Integer = 0

        Dim noStockpost As Integer = 0
        Dim SlAc_ID As Integer = 0

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)




        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Jari_Sales_Delivery_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Jari_Sales_Return_Entry, New_Entry, Me, con, "Jari_SalesDelivery_Return_Head", "Jari_SalesDelivery_Return_Code", NewCode, "Jari_SalesDelivery_Return_Date", "(Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Jari_SalesDelivery_Return_No desc", dtp_Date.Value.Date) = False Then Exit Sub

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

        SlAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAcc.Text)
        If SlAc_ID = 0 Then
            MessageBox.Show("Invalid Sales A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalesAcc.Enabled Then cbo_SalesAcc.Focus()
            Exit Sub
        End If

        noStockpost = 0
        If chk_NoStockPosting.Checked = True Then noStockpost = 1

        Delv_ID = 0  ' Led_ID

        Rec_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo

        With dgv_KuriDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(8).Value) <> 0 Or Val(.Rows(i).Cells(10).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then

                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If

                        Exit Sub

                    End If

                    If Val(.Rows(i).Cells(8).Value) = 0 Then
                        MessageBox.Show("Invalid NetWeight..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled Then .Focus()
                        .CurrentCell = .Rows(0).Cells(8)
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(9).Value) = 0 Then
                        MessageBox.Show("Invalid Rate..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled Then .Focus()
                        .CurrentCell = .Rows(0).Cells(9)
                        Exit Sub
                    End If

                End If

            Next

        End With

        Total_Calculation()


        vTotJumbo = 0 : vTotCns = 0 : vTotGrsWgt = 0 : vTotTareWgt = 0 : vTotNetWgt = 0 : vTotAmt = 0
        If dgv_KuriDetails_Total.RowCount > 0 Then
            vTotJumbo = Val(dgv_KuriDetails_Total.Rows(0).Cells(4).Value())
            vTotCns = Val(dgv_KuriDetails_Total.Rows(0).Cells(5).Value())
            vTotGrsWgt = Val(dgv_KuriDetails_Total.Rows(0).Cells(6).Value())
            vTotTareWgt = Val(dgv_KuriDetails_Total.Rows(0).Cells(7).Value())
            vTotNetWgt = Val(dgv_KuriDetails_Total.Rows(0).Cells(8).Value())
            vTotAmt = Val(dgv_KuriDetails_Total.Rows(0).Cells(10).Value())
        End If

        'If (Val(txt_OurBobin.Text) + Val(txt_PartyBobin.Text)) <> Val(vTotBbns) Then
        '    MessageBox.Show("Invalid Bobins..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_PartyBobin.Enabled Then txt_PartyBobin.Focus()
        '    Exit Sub
        'End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Jari_SalesDelivery_Return_Head", "Jari_SalesDelivery_Return_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            'Da = New SqlClient.SqlDataAdapter("select count(*) from Jari_SalesDelivery_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "' and BobinSales_Invoice_Code <> ''", con)
            'Da.SelectCommand.Transaction = tr
            'Dt1 = New DataTable
            'Da.Fill(Dt1)
            'If Dt1.Rows.Count > 0 Then
            '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
            '        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
            '            Throw New ApplicationException("Already Invoiced")
            '            Exit Sub
            '        End If
            '    End If
            'End If
            'Dt1.Clear()

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))

            vOrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Jari_SalesDelivery_Return_Head ( Jari_SalesDelivery_Return_Code, Company_IdNo, Jari_SalesDelivery_Return_No, for_OrderBy, Jari_SalesDelivery_Return_Date, Ledger_IdNo, Vechile_No, Freight, Transport_IdNo, Total_Jumbos, Total_Cones, Total_GrossWeight,Total_TareWeight,  Total_NetWeight,Total_Amount ,  Remarks,No_Stock_Posting,SalesAc_IdNo , User_IdNo  , Net_Amount  ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate, " & Str(Val(Led_ID)) & ",'" & Trim(cbo_VechileNo.Text) & "'," & Str(Val(txt_Freight.Text)) & "," & Str(Val(Trans_ID)) & ",  " & Str(Val(vTotJumbo)) & " , " & Str(Val(vTotCns)) & ",  " & Str(Val(vTotGrsWgt)) & " , " & Str(Val(vTotTareWgt)) & " , " & Str(Val(vTotNetWgt)) & " , " & Str(Val(vTotAmt)) & " , '" & Trim(txt_Remarks.Text) & "'," & Str(Val(noStockpost)) & ", " & Val(SlAc_ID) & "," & Val(Common_Procedures.User.IdNo) & "," & Str(Val(lbl_Net_Amt.Text)) & "  )"
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Jari_SalesDelivery_Return_Head", "Jari_SalesDelivery_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Jari_SalesDelivery_Return_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Jari_SalesDelivery_Return_Details", "Jari_SalesDelivery_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo, Colour_IdNo, BorderSize_IdNo, Noof_Jumbos, Noof_Cones,Gross_Weight  ,   Tare_Weight    ,  Net_Weight    , Rate,Amount", "Sl_No", "Jari_SalesDelivery_Return_Code, For_OrderBy, Company_IdNo, Jari_SalesDelivery_Return_No, Jari_SalesDelivery_Return_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Jari_SalesDelivery_Return_Head set Jari_SalesDelivery_Return_Date = @EntryDate, Ledger_IdNo = " & Val(Led_ID) & ", SalesAc_IdNo = " & Val(SlAc_ID) & " , Vechile_No = '" & Trim(cbo_VechileNo.Text) & "', Freight = " & Str(Val(txt_Freight.Text)) & ", Transport_IdNo = " & Str(Val(Trans_ID)) & ", Total_Jumbos = " & Val(vTotJumbo) & ", Total_Cones = " & Val(vTotCns) & ", Total_GrossWeight = " & Val(vTotGrsWgt) & ",Total_TareWeight = " & Val(vTotTareWgt) & ",Total_NetWeight = " & Val(vTotNetWgt) & ", Total_Amount = " & Val(vTotAmt) & ", Remarks = '" & Trim(txt_Remarks.Text) & "' ,No_Stock_Posting = " & Str(Val(noStockpost)) & ", User_Idno = " & Val(Common_Procedures.User.IdNo) & ",Net_Amount = " & Str(Val(lbl_Net_Amt.Text)) & " where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Jari_SalesDelivery_Return_Head", "Jari_SalesDelivery_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Jari_SalesDelivery_Return_Code, Company_IdNo, for_OrderBy", tr)

           
            Partcls = "JariDelvRetn : Retn.No. " & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)

            'cmd.CommandText = "Delete from Jari_Sales_Delivery_Bobin_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Jari_SalesDelivery_Jari_Return_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            'cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            With dgv_KuriDetails

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(8).Value) <> 0 Or Val(.Rows(i).Cells(10).Value) <> 0 Then

                        Sno = Sno + 1

                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        Clr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        BthSz_ID = Common_Procedures.BorderSize_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        'Nr = 0
                        'cmd.CommandText = "Update  Jari_SalesDelivery_Jari_Return_Details set Jari_SalesDelivery_Return_Date = @EntryDate , Sl_No  = " & Str(Val(Sno)) & " , Count_IdNo = " & Str(Val(Cnt_ID)) & "  , Colour_IdNo = " & Str(Val(Clr_ID)) & "  , BorderSize_IdNo = " & Str(Val(BthSz_ID)) & " , Noof_Jumbos = " & Val(.Rows(i).Cells(4).Value) & " , Noof_Cones = " & Val(.Rows(i).Cells(5).Value) & " , Gross_Weight = " & Val(.Rows(i).Cells(6).Value) & " ,Tare_Weight = " & Val(.Rows(i).Cells(7).Value) & " ,Net_Weight = " & Val(.Rows(i).Cells(8).Value) & " ,Rate = " & Val(.Rows(i).Cells(9).Value) & " ,Amount = " & Val(.Rows(i).Cells(10).Value) & "   where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "'  and Jari_Sales_Delivery_Details_Slno = " & Str(Val(.Rows(i).Cells(11).Value))
                        'Nr = cmd.ExecuteNonQuery()

                        'If Nr = 0 Then
                        cmd.CommandText = "Insert into Jari_SalesDelivery_Jari_Return_Details ( Jari_SalesDelivery_Return_Code, Company_IdNo, Jari_SalesDelivery_Return_No, for_OrderBy, Jari_SalesDelivery_Return_Date, Sl_No, Count_IdNo, Colour_IdNo, BorderSize_IdNo, Noof_Jumbos, Noof_Cones,Gross_Weight  ,   Tare_Weight    ,  Net_Weight    , Rate,Amount ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate," & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", " & Str(Val(Clr_ID)) & " , " & Str(Val(BthSz_ID)) & ", " & Val(.Rows(i).Cells(4).Value) & ", " & Val(.Rows(i).Cells(5).Value) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & "," & Str(Val(.Rows(i).Cells(10).Value)) & " )"
                        cmd.ExecuteNonQuery()

                        'End If

                        If chk_NoStockPosting.Checked = False Then
                            cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Cones, Empty_Bobin, EmptyBobin_Party, Empty_Jumbo) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate,4, " & Str(Val(Led_ID)) & ",  '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1,  " & Val(.Rows(i).Cells(5).Value) & ", 0  ,0, " & Val(.Rows(i).Cells(4).Value) & " )"
                            cmd.ExecuteNonQuery()

                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Colour_IdNo, Jumbo, Cones, Weight) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate, " & Str(Val(Rec_ID)) & ", " & Str(Val(Delv_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", 'MILL', 0, " & Str(Val(Clr_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " )"
                            cmd.ExecuteNonQuery()
                        End If


                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Jari_SalesDelivery_Return_Details", "Jari_SalesDelivery_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo, Colour_IdNo, BorderSize_IdNo, Noof_Jumbos, Noof_Cones,Gross_Weight  ,   Tare_Weight    ,  Net_Weight    , Rate,Amount", "Sl_No", "Jari_SalesDelivery_Return_Code, For_OrderBy, Company_IdNo, Jari_SalesDelivery_Return_No, Jari_SalesDelivery_Return_Date, Ledger_Idno", tr)

            End With

            'If Val(txt_OurBobin.Text) <> 0 Or Val(txt_PartyBobin.Text) <> 0 Or Val(vTotJumbo) <> 0 Or Val(vTotCns) <> 0 Then
            '    cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Cones, Empty_Bobin, EmptyBobin_Party, Empty_Jumbo) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate, " & Str(Val(Led_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(vTotCns)) & ", " & Str(Val(txt_OurBobin.Text)) & ", " & Str(Val(txt_PartyBobin.Text)) & ", " & Str(Val(vTotJumbo)) & ")"
            '    cmd.ExecuteNonQuery()
            'End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), tr)

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0


            AcPos_ID = Led_ID


            vLed_IdNos = AcPos_ID & "|" & SlAc_ID

            'vVou_Amts = -1 * Val(CSng(lbl_Net_Amt.Text)) & "|" & (Val(CSng(lbl_Net_Amt.Text)))

            vVou_Amts = Val(CSng(lbl_Net_Amt.Text)) & "|" & -1 * (Val(CSng(lbl_Net_Amt.Text)))

            If Common_Procedures.Voucher_Updation(con, "Jari.Sale", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(dtp_Date.Text), "Dc No : " & Trim(lbl_DcNo.Text) & ", Wgt : " & Trim(Format(Val(vTotNetWgt), "#########0.00")), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            vLed_IdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "Jari.Dc.Frgt", Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(dtp_Date.Text), Partcls, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            Dim VouBil As String = ""
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Convert.ToDateTime(dtp_Date.Text), AcPos_ID, Trim(lbl_DcNo.Text), 0, Val(CSng(lbl_Net_Amt.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If



            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

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

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Total_Calculation()
        Dim vTotJumbo As Single, vTotCns As Single, vTotGrosWgt As Single, vTotTareWgt As Single
        Dim vTotNetWgt As Single, vTotAmt As Single
        Dim i As Integer
        Dim sno As Integer

        Try

            vTotJumbo = 0 : vTotCns = 0 : vTotGrosWgt = 0 : vTotTareWgt = 0 : vTotNetWgt = 0 : vTotAmt = 0
            sno = 0
            With dgv_KuriDetails
                For i = 0 To .Rows.Count - 1

                    sno = sno + 1

                    .Rows(i).Cells(0).Value = sno

                    If Val(.Rows(i).Cells(8).Value) <> 0 Or Val(.Rows(i).Cells(10).Value) <> 0 Then
                        vTotJumbo = vTotJumbo + Val(.Rows(i).Cells(4).Value)
                        vTotCns = vTotCns + Val(.Rows(i).Cells(5).Value)
                        vTotGrosWgt = vTotGrosWgt + Val(.Rows(i).Cells(6).Value)
                        vTotTareWgt = vTotTareWgt + Val(.Rows(i).Cells(7).Value)
                        vTotNetWgt = vTotNetWgt + Val(.Rows(i).Cells(8).Value)
                        vTotAmt = vTotAmt + Val(.Rows(i).Cells(10).Value)
                    End If

                Next
            End With

            If dgv_KuriDetails_Total.Rows.Count <= 0 Then dgv_KuriDetails_Total.Rows.Add()
            dgv_KuriDetails_Total.Rows(0).Cells(4).Value = Val(vTotJumbo)
            dgv_KuriDetails_Total.Rows(0).Cells(5).Value = Val(vTotCns)
            dgv_KuriDetails_Total.Rows(0).Cells(6).Value = Format(Val(vTotGrosWgt), "#########0.000")
            dgv_KuriDetails_Total.Rows(0).Cells(7).Value = Format(Val(vTotTareWgt), "#########0.000")
            dgv_KuriDetails_Total.Rows(0).Cells(8).Value = Format(Val(vTotNetWgt), "#########0.000")
            dgv_KuriDetails_Total.Rows(0).Cells(10).Value = Format(Val(vTotAmt), "#########0.00")


            lbl_Net_Amt.Text = Format(Val(vTotAmt), "#########0.00")
        Catch ex As Exception
            '----
        End Try

    End Sub


    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, cbo_SalesAcc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_SalesAcc, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        Try
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

        Catch ex As Exception
            '--------
        End Try

    End Sub

    Private Sub cbo_VechileNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VechileNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Jari_SalesDelivery_Return_Head", "Vechile_No", "", "")
    End Sub



    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VechileNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VechileNo, cbo_Transport, txt_Freight, "Jari_SalesDelivery_Return_Head", "Vechile_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VechileNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VechileNo, txt_Freight, "Jari_SalesDelivery_Return_Head", "Vechile_No", "", "", False)
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_SalesAcc, cbo_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        'If e.KeyValue = 40 And cbo_Transport.DroppedDown = False Then
        '    If dgv_BobinDetails.Rows.Count > 0 Then
        '        dgv_BobinDetails.Focus()
        '        dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
        '        dgv_BobinDetails.CurrentCell.Selected = True

        '    Else
        '        txt_PartyBobin.Focus()

        '    End If
        'End If
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 And cbo_Transport.DroppedDown = False Then
        '    If dgv_BobinDetails.Rows.Count > 0 Then
        '        dgv_BobinDetails.Focus()
        '        dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
        '        dgv_BobinDetails.CurrentCell.Selected = True

        '    Else
        '        txt_PartyBobin.Focus()

        '    End If
        'End If
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
        Dim Led_IdNo As Integer, proc_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            proc_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Jari_SalesDelivery_Return_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Jari_SalesDelivery_Return_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Jari_SalesDelivery_Return_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_EndsName.Text) <> "" Then
                proc_IdNo = Common_Procedures.Process_NameToIdNo(con, cbo_Filter_EndsName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If




            If Trim(cbo_Filter_EndsName.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Jari_SalesDelivery_Return_Head IN (select z1.Jari_SalesDelivery_Return_Head from Jari_Sales_Delivery_Bobin_Details z1 where z1.Ends = '" & Trim(cbo_Filter_EndsName.Text) & "')"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Jari_SalesDelivery_Return_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Jari_SalesDelivery_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Jari_SalesDelivery_Return_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Jari_SalesDelivery_Return_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Jari_SalesDelivery_Return_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Total_Bobins").ToString)
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")


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


    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_EndsName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub


    Private Sub cbo_Filter_EndsName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsName, dtp_Filter_ToDate, cbo_Filter_PartyName, "EndsCount_Head", "EndsCount_name", "", "(endsCount_iDNO = 0)")

    End Sub

    Private Sub cbo_Filter_ProcessName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsName, cbo_Filter_PartyName, "endsCount_Head", "EndsCount_name", "", "(EndsCount_iDNO = 0)")
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


    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        'Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Jari_Sales_delivery_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Jari_SalesDelivery_Return_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "'", con)
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


        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try


                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
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
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dtbl1 As New DataTable
        Dim nr As Integer = 0
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName from Jari_SalesDelivery_Return_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Colour_Name, d.BorderSize_Name from Jari_SalesDelivery_Jari_Return_Details a INNER JOIN Count_Head b ON a.Count_idno = b.Count_idno LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN BorderSize_Head d ON a.BorderSize_IdNo = d.BorderSize_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Jari_SalesDelivery_Return_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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
        'Dim ps As Printing.PaperSize
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
        Dim SNo As Integer

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10 ' 30
            .Right = 40
            .Top = 10 ' 30
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

        NoofItems_PerPage = 7

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 25 : ClAr(2) = 85 : ClAr(3) = 100 : ClAr(4) = 70 : ClAr(5) = 55 : ClAr(6) = 50 : ClAr(7) = 75 : ClAr(8) = 75 : ClAr(9) = 75 : ClAr(10) = 60
        ClAr(11) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10))

        TxtHgt = 17.75 ' 18

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

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            NoofDets = 0
                            e.HasMorePages = True

                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 12 Then
                            For I = 12 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("BorderSize_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Noof_Jumbos").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Noof_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Gross_Weight").ToString), " ############0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Tare_Weight").ToString), " ############0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Net_Weight").ToString), " ############0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), " ############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), " ############0.00"), PageWidth - 10, CurY, 1, 0, pFont)

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
        Dim W1 As Single
        Dim S1, s2 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Colour_Name, d.BorderSize_Name from Jari_SalesDelivery_Jari_Return_Details a INNER JOIN Count_Head b ON a.Count_idno = b.Count_idno LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN BorderSize_Head d ON a.BorderSize_IdNo = d.BorderSize_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Jari_SalesDelivery_Return_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        p1Font = New Font("Calibri", 9, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "JARI DELIVERY RETURN", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
        'C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Jari_SalesDelivery_Return_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Jari_SalesDelivery_Return_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        ''CurY = CurY + TxtHgt + 10
        ''If prn_HdDt.Rows(0).Item("Party_OrderNo").ToString <> "" Then
        ''    Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        ''    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        ''    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        ''End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        'CurY = CurY + TxtHgt - 10
        'Common_Procedures.Print_To_PrintDocument(e, "SALES A/C ", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("SalesAc_IdNo").ToString)), LMargin + s2 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "VECHILE NO  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + s2 + 30, CurY, 0, 0, pFont)

        ' CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "JUMBO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "GRS WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TARE WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NET WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Delivery_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim C1 As Single = 0
        Dim s2 As Single = 0
        Dim vprn_BlNos As String = ""

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("TOTAL BOBIN : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TOTAL BOBIN :  ", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_JumBos").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_GrossWeight").ToString), "########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_TareWeight").ToString), "########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_NetWeight").ToString), "########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))

        'vprn_BlNos = ""
        'For i = 0 To prn_DetDt.Rows.Count - 1
        '    If Trim(prn_DetDt.Rows(i).Item("Bales_Nos").ToString) <> "" Then
        '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(i).Item("Bales_Nos").ToString
        '    End If
        'Next
        'Common_Procedures.Print_To_PrintDocument(e, "BALES NOS : " & vprn_BlNos, LMargin + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "( " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(prn_HdDt.Rows(0).Item("User_IdNo").ToString)))) & " )", LMargin, CurY + 10, 2, PageWidth, p1Font)

        CurY = CurY + TxtHgt + 25

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString


        'Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Receiver Signature", LMargin + 5, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin, CurY, 2, PageWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 38 Then
            If dgv_KuriDetails.Rows.Count > 0 Then
                dgv_KuriDetails.Focus()
                dgv_KuriDetails.CurrentCell = dgv_KuriDetails.Rows(0).Cells(1)


            Else
                txt_Freight.Focus()

            End If
        End If
        If e.KeyValue = 40 Then
            chk_NoStockPosting.Focus()
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        'If Asc(e.KeyChar) = 13 Then
        '    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
        '        save_record()
        '    Else
        '        dtp_Date.Focus()
        '    End If
        'End If
    End Sub



    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        Try
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")

            If e.KeyValue = 40 Then
                If dgv_KuriDetails.Rows.Count > 0 Then
                    dgv_KuriDetails.Focus()
                    dgv_KuriDetails.CurrentCell = dgv_KuriDetails.Rows(0).Cells(1)
                    dgv_KuriDetails.CurrentCell.Selected = True

                Else
                    txt_Remarks.Focus()

                End If

            End If

        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        Try
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
            If Asc(e.KeyChar) = 13 Then
                If dgv_KuriDetails.Rows.Count > 0 Then
                    dgv_KuriDetails.Focus()
                    dgv_KuriDetails.CurrentCell = dgv_KuriDetails.Rows(0).Cells(1)
                    dgv_KuriDetails.CurrentCell.Selected = True

                Else
                    txt_Remarks.Focus()

                End If
            End If

        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub txt_OutBobin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyValue = 40 Then
                If dgv_KuriDetails.Rows.Count > 0 Then
                    dgv_KuriDetails.Focus()
                    dgv_KuriDetails.CurrentCell = dgv_KuriDetails.Rows(0).Cells(1)
                    dgv_KuriDetails.CurrentCell.Selected = True

                Else
                    txt_Remarks.Focus()

                End If
            End If

        Catch ex As Exception
            '----
        End Try
    End Sub



    Private Sub cbo_KuriCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KuriCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_KuriCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KuriCount.KeyDown

        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_KuriCount, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

            With dgv_KuriDetails

                If (e.KeyValue = 38 And cbo_KuriCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                    If Val(.CurrentCell.RowIndex) <= 0 Then

                        txt_Freight.Focus()


                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)
                        .CurrentCell.Selected = True

                    End If

                End If

                If (e.KeyValue = 40 And cbo_KuriCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                        txt_Remarks.Focus()

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                End If

            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_KuriCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_KuriCount.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_KuriCount, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                With dgv_KuriDetails
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_KuriCount.Text)
                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            txt_Remarks.Focus()
                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        End If

                    End If

                End With

            End If

        Catch ex As Exception
            '------

        End Try

    End Sub

    Private Sub cbo_KuriCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KuriCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_KuriCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_KuriCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KuriCount.TextChanged
        Try
            If cbo_KuriCount.Visible Then
                With dgv_KuriDetails
                    If Val(cbo_KuriCount.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_KuriCount.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_KuriColour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KuriColour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_KuriColour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KuriColour.KeyDown
        Dim dep_idno As Integer = 0

        Try
            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_KuriColour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
            With dgv_KuriDetails

                If (e.KeyValue = 38 And cbo_KuriColour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                End If

                If (e.KeyValue = 40 And cbo_KuriColour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_KuriColour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_KuriColour.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_KuriColour, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then
                With dgv_KuriDetails
                    If .Rows.Count > 0 Then
                        .Focus()
                        .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value = Trim(cbo_KuriColour.Text)
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                End With
            End If

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub cbo_KuriColour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KuriColour.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                Dim f As New Color_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_KuriColour.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()

            End If

        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub cbo_KuriColour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KuriColour.TextChanged

        Try
            If cbo_KuriColour.Visible Then
                With dgv_KuriDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_KuriColour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_KuriColour.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_KuriBorderSize_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KuriBorderSize.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "BorderSize_Head", "BorderSize_Name", "", "(BorderSize_IdNo = 0)")
    End Sub

    Private Sub cbo_KuriBorderSize_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KuriBorderSize.KeyDown

        Try
            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_KuriBorderSize, Nothing, Nothing, "BorderSize_Head", "BorderSize_Name", "", "(BorderSize_IdNo = 0)")

            With dgv_KuriDetails

                If (e.KeyValue = 38 And cbo_KuriBorderSize.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                End If

                If (e.KeyValue = 40 And cbo_KuriBorderSize.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With


        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub cbo_KuriBorderSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_KuriBorderSize.KeyPress

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_KuriBorderSize, Nothing, "BorderSize_Head", "BorderSize_Name", "", "(BorderSize_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                With dgv_KuriDetails

                    .Focus()
                    .Rows(.CurrentCell.RowIndex).Cells.Item(3).Value = Trim(cbo_KuriBorderSize.Text)
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End With

            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_KuriBorderSize_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KuriBorderSize.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                Dim f As New BorderSize_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_KuriBorderSize.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()

            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_KuriBorderSize_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KuriBorderSize.TextChanged

        Try
            If cbo_KuriBorderSize.Visible Then
                With dgv_KuriDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_KuriBorderSize.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_KuriBorderSize.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_KuriDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellEndEdit
        Try
            With dgv_KuriDetails

                If .Rows.Count > 0 Then

                    If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then
                        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Then
                        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                        End If
                    End If


                End If

            End With

        Catch ex As Exception
            '-----
        End Try


    End Sub

    Private Sub dgv_KuriDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Rect As Rectangle

        Try


            With dgv_KuriDetails

                dgv_ActCtrlName = .Name.ToString

                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If

                If e.ColumnIndex = 1 Then

                    If cbo_KuriCount.Visible = False Or Val(cbo_KuriCount.Tag) <> e.RowIndex Then

                        dgv_ActCtrlName = dgv_KuriDetails.Name

                        cbo_KuriCount.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head Order by Count_Name", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        cbo_KuriCount.DataSource = Dt2
                        cbo_KuriCount.DisplayMember = "Count_Name"

                        Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_KuriCount.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_KuriCount.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_KuriCount.Width = Rect.Width  ' .CurrentCell.Size.Width
                        cbo_KuriCount.Height = Rect.Height  ' rect.Height

                        cbo_KuriCount.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_KuriCount.Tag = Val(e.RowIndex)
                        cbo_KuriCount.Visible = True

                        cbo_KuriCount.BringToFront()
                        cbo_KuriCount.Focus()

                        'cbo_Grid_CountName.Visible = False
                        'cbo_Grid_MillName.Visible = False

                    End If

                Else

                    cbo_KuriCount.Visible = False

                End If

                If e.ColumnIndex = 2 Then

                    If cbo_KuriColour.Visible = False Or Val(cbo_KuriColour.Tag) <> e.RowIndex Then

                        cbo_KuriColour.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        cbo_KuriColour.DataSource = Dt2
                        cbo_KuriColour.DisplayMember = "Colour_Name"

                        Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_KuriColour.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_KuriColour.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_KuriColour.Width = Rect.Width  ' .CurrentCell.Size.Width
                        cbo_KuriColour.Height = Rect.Height  ' rect.Height

                        cbo_KuriColour.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_KuriColour.Tag = Val(e.RowIndex)
                        cbo_KuriColour.Visible = True

                        cbo_KuriColour.BringToFront()
                        cbo_KuriColour.Focus()

                    End If

                Else

                    'cbo_Grid_MillName.Tag = -1
                    'cbo_Grid_MillName.Text = ""
                    cbo_KuriColour.Visible = False

                End If

                If e.ColumnIndex = 3 Then

                    If cbo_KuriBorderSize.Visible = False Or Val(cbo_KuriBorderSize.Tag) <> e.RowIndex Then

                        cbo_KuriBorderSize.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select BorderSize_Name from BorderSize_Head Order by BorderSize_Name", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        cbo_KuriBorderSize.DataSource = Dt2
                        cbo_KuriBorderSize.DisplayMember = "BorderSize_Name"

                        Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_KuriBorderSize.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_KuriBorderSize.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_KuriBorderSize.Width = Rect.Width  ' .CurrentCell.Size.Width
                        cbo_KuriBorderSize.Height = Rect.Height  ' rect.Height

                        cbo_KuriBorderSize.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_KuriBorderSize.Tag = Val(e.RowIndex)
                        cbo_KuriBorderSize.Visible = True

                        cbo_KuriBorderSize.BringToFront()
                        cbo_KuriBorderSize.Focus()

                    End If

                Else

                    cbo_KuriBorderSize.Visible = False

                End If

            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_KuriDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellLeave
        Try
            With dgv_KuriDetails
                If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If
                If .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    End If
                End If
            End With

        Catch ex As Exception
            '------
        End Try
    End Sub

    Private Sub dgv_KuriDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellValueChanged

        Try
            If IsNothing(dgv_KuriDetails.CurrentCell) Then Exit Sub

            With dgv_KuriDetails
                    If .Visible Then
                        If .Rows.Count > 0 Then
                            If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Then
                                .Rows(.CurrentCell.RowIndex).Cells(8).Value = Format((Val(.Rows(.CurrentCell.RowIndex).Cells(6).Value) - Val(.Rows(.CurrentCell.RowIndex).Cells(7).Value)), "#########0.000")
                                .Rows(.CurrentCell.RowIndex).Cells(10).Value = Format((Val(.Rows(.CurrentCell.RowIndex).Cells(8).Value) * Val(.Rows(.CurrentCell.RowIndex).Cells(9).Value)), "#########0")
                                Total_Calculation()
                            End If
                        End If
                    End If
                End With


        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_KuriDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_KuriDetails.EditingControlShowing
        Try
            dgtxt_KuriDetails = CType(dgv_KuriDetails.EditingControl, DataGridViewTextBoxEditingControl)
        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgtxt_KuriDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_KuriDetails.Enter
        Try
            dgv_ActCtrlName = dgv_KuriDetails.Name
            dgv_KuriDetails.EditingControl.BackColor = Color.Lime
            dgv_KuriDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_KuriDetails.SelectAll()

        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgtxt_KuriDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_KuriDetails.KeyPress
        Try
            With dgv_KuriDetails

                If Val(dgv_KuriDetails.CurrentCell.ColumnIndex.ToString) = 4 Or Val(dgv_KuriDetails.CurrentCell.ColumnIndex.ToString) = 5 Or Val(dgv_KuriDetails.CurrentCell.ColumnIndex.ToString) = 6 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If

            End With

        Catch ex As Exception
            '------
        End Try


    End Sub

    Private Sub dgv_KuriDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_KuriDetails.KeyUp
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_KuriDetails

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

        Catch ex As Exception
            '-----

        End Try


    End Sub

    Private Sub dgv_KuriDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_KuriDetails.RowsAdded
        Dim n As Integer = 0

        Try
            With dgv_KuriDetails
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_KuriDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_KuriDetails.LostFocus
        On Error Resume Next
        dgv_KuriDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgtxt_KuriDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_KuriDetails.KeyUp
        dgv_KuriDetails_KeyUp(sender, e)
    End Sub

    Private Sub chk_NoStockPosting_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_NoStockPosting.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub
    Private Sub cbo_SalesAcc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAcc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_SalesAcc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAcc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAcc, cbo_Ledger, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAcc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAcc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAcc, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAcc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAcc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAcc.Name
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

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub
End Class