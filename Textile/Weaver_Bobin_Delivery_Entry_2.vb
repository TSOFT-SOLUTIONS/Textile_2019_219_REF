Public Class Weaver_Bobin_Delivery_Entry_2
        Implements Interface_MDIActions

        Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Private FrmLdSTS As Boolean = False
        Private New_Entry As Boolean = False
        Private Insert_Entry As Boolean = False
        Private Filter_Status As Boolean = False
        Private Pk_Condition As String = "WBDLV-"
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
        Private WithEvents dgtxt_RequirementDetails As New DataGridViewTextBoxEditingControl
        Private dgv_ActCtrlName As String = ""
        Private dgv_LevColNo As Integer

        Public vmskOldText As String = ""
        Public vmskSelStrt As Integer = -1

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_EndsCount_Stock.Visible = False
        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        msk_date.Text = ""
        cbo_Ledger.Text = ""
        cbo_VechileNo.Text = ""
        cbo_Transport.Text = ""
        txt_Freight.Text = ""
        txt_PartyBobin.Text = ""
        txt_OurBobin.Text = ""
        txt_Remarks.Text = ""
        dgv_BobinDetails.Rows.Clear()
        dgv_KuriDetails.Rows.Clear()
        dgv_YarnStock.Rows.Clear()
        dgv_Req_Details.Rows.Clear()
        dgv_KuriDetails_Total.Rows.Clear()
        dgv_KuriDetails_Total.Rows.Add()
        Grid_DeSelect()

        cbo_BobinEnds.Visible = False
        cbo_BobinEnds.Tag = -1

        cbo_BobinMillName.Visible = False
        cbo_BobinMillName.Tag = -1
        cbo_BobinColour.Visible = False
        cbo_BobinColour.Tag = -1
        cbo_BobinBorderSize.Visible = False
        cbo_BobinBorderSize.Tag = -1

        cbo_KuriCount.Visible = False
        cbo_KuriCount.Tag = -1
        cbo_millName.Visible = False
        cbo_millName.Tag = -1
        cbo_KuriColour.Visible = False
        cbo_KuriColour.Tag = -1
        cbo_KuriBorderSize.Visible = False
        cbo_KuriBorderSize.Tag = -1

        cbo_BobinEnds.Text = ""
        cbo_BobinColour.Text = ""
        cbo_BobinMillName.Text = ""
        cbo_BobinBorderSize.Text = ""

        cbo_KuriCount.Text = ""
        cbo_KuriColour.Text = ""
        cbo_KuriBorderSize.Text = ""

        'dgv_Details.Tag = ""
        'dgv_LevColNo = -1

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        dgv_ActCtrlName = ""

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Req_Details.CurrentCell) Then dgv_Req_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_KuriDetails.CurrentCell) Then dgv_KuriDetails.CurrentCell.Selected = False
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

        If Me.ActiveControl.Name <> cbo_BobinEnds.Name Then
            cbo_BobinEnds.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_BobinColour.Name Then
            cbo_BobinColour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_BobinMillName.Name Then
            cbo_BobinMillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_BobinBorderSize.Name Then
            cbo_BobinBorderSize.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_KuriCount.Name Then
            cbo_KuriCount.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_millName.Name Then
            cbo_millName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_KuriColour.Name Then
            cbo_KuriColour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_KuriBorderSize.Name Then
            cbo_KuriBorderSize.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_BobinDetails.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_KuriDetails.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_BobinDetails.Name Then
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
        If Not IsNothing(dgv_Req_Details.CurrentCell) Then dgv_Req_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_KuriDetails.CurrentCell) Then dgv_KuriDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_BobinDetails_Total.CurrentCell) Then dgv_BobinDetails_Total.CurrentCell.Selected = False
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Weaver_BobinJari_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo   Where a.Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_DcNo.Text = dt1.Rows(0).Item("Bobin_Jari_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Bobin_Jari_Delivery_Date")
                msk_date.Text = dtp_Date.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_VechileNo.Text = dt1.Rows(0).Item("Vechile_No").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight").ToString), "########0.00")
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_PartyBobin.Text = Format(Val(dt1.Rows(0).Item("Party_Bobin").ToString), "########0.00")
                txt_OurBobin.Text = Format(Val(dt1.Rows(0).Item("OurOwn_Bobin").ToString), "########0.00")
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name, c.Colour_Name, d.BorderSize_Name from Weaver_BobinJari_Delivery_Bobin_Details a INNER JOIN Endscount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN BorderSize_Head d ON a.BorderSize_IdNo = d.BorderSize_IdNo Where a.Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_BobinDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_BobinDetails.Rows.Add()


                        SNo = SNo + 1
                        dgv_BobinDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_BobinDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                        dgv_BobinDetails.Rows(n).Cells(2).Value = Common_Procedures.Mill_IdNoToName(con, Val(dt2.Rows(i).Item("Mill_IdNo").ToString))
                        dgv_BobinDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_BobinDetails.Rows(n).Cells(4).Value = dt2.Rows(i).Item("BorderSize_Name").ToString
                        dgv_BobinDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Bobins").ToString)
                        dgv_BobinDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Meter_Bobin").ToString), "########0.00")
                        dgv_BobinDetails.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("METERS").ToString), "########0.00")
                        'dgv_BobinDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Bobin_Jari_Sales_Invoice_Code").ToString
                        'dgv_BobinDetails.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Bobin_Jari_Delivery_Bobin_Slno").ToString

                        'If Val(dgv_BobinDetails.Rows(n).Cells(7).Value) <> 0 Then
                        '    For j = 0 To dgv_BobinDetails.ColumnCount - 1
                        '        dgv_BobinDetails.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                        '    Next j
                        '    LockSTS = True
                        'End If

                    Next i

                End If
                dt2.Clear()

                With dgv_BobinDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Bobins").ToString)
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                End With


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Colour_Name, d.BorderSize_Name from Weaver_BobinJari_Delivery_Jari_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN BorderSize_Head d ON a.BorderSize_IdNo = d.BorderSize_IdNo where a.Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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
                        dgv_KuriDetails.Rows(n).Cells(2).Value = Common_Procedures.Mill_IdNoToName(con, Val(dt2.Rows(i).Item("Mill_IdNo").ToString))
                        dgv_KuriDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_KuriDetails.Rows(n).Cells(4).Value = dt2.Rows(i).Item("BorderSize_Name").ToString
                        dgv_KuriDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Noof_Jumbos").ToString)
                        If Val(dgv_KuriDetails.Rows(n).Cells(5).Value) = 0 Then
                            dgv_KuriDetails.Rows(n).Cells(5).Value = ""
                        End If
                        dgv_KuriDetails.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Noof_Cones").ToString)
                        If Val(dgv_KuriDetails.Rows(n).Cells(6).Value) = 0 Then
                            dgv_KuriDetails.Rows(n).Cells(6).Value = ""
                        End If
                        dgv_KuriDetails.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        'dgv_KuriDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Bobin_Jari_Sales_Invoice_Code").ToString
                        'dgv_KuriDetails.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Bobin_Jari_Delivery_Jari_Slno").ToString

                        'If Val(dgv_KuriDetails.Rows(n).Cells(7).Value) <> 0 Then
                        '    For j = 0 To dgv_KuriDetails.ColumnCount - 1
                        '        dgv_KuriDetails.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                        '    Next j
                        '    LockSTS = True
                        'End If
                    Next i

                End If

                With dgv_KuriDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Jumbos").ToString)
                    .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With


                da2 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Bobin_Delivery_Requirement_Details a  where a.Weaver_Bobin_Delivery_Requirement_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Req_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Req_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Req_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Req_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Requirement_No").ToString
                        dgv_Req_Details.Rows(n).Cells(2).Value = (dt2.Rows(i).Item("Requirement_Date").ToString)
                        dgv_Req_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Loom_No").ToString)
                        dgv_Req_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Mark").ToString
                        dgv_Req_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Noof_Bobin").ToString)

                        dgv_Req_Details.Rows(n).Cells(6).Value = (dt2.Rows(i).Item("Weaver_PavuBobin_Requirement_Code").ToString)

                        dgv_Req_Details.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Weaver_PavuBobin_Requirement_Slno").ToString)
                        dgv_Req_Details.Rows(n).Cells(8).Value = Common_Procedures.EndsCount_IdNoToName(con, Val(dt2.Rows(i).Item("EndsCount_IdNo").ToString))
                    Next i

                End If
            End If
            dt1.Clear()

            If LockSTS = True Then
                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray
            End If

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

        If msk_date.Visible And msk_date.Enabled Then
            msk_date.Focus()
            msk_date.SelectionStart = 0
        End If

    End Sub

    Private Sub Weaver_Bobin_Delivery_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinEnds.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinEnds.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinColour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BORDER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinColour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinBorderSize.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BORDERSIZE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinBorderSize.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_millName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_millName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinMillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinMillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Weaver_Bobin_Delivery_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head where Cotton_Polyester_Jari <>'COTTON' order by EndsCount_Name", con)
        da.Fill(dt1)
        cbo_BobinEnds.DataSource = dt1
        cbo_BobinEnds.DisplayMember = "EndsCount_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Ledger.DataSource = dt2
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_Transport.DataSource = dt3
        cbo_Transport.DisplayMember = "Ledger_DisplayName"


        'da = New SqlClient.SqlDataAdapter("select distinct(Vechile_No) from Weaver_BobinJari_Delivery_Head order by Vechile_No", con)
        'da.Fill(dt4)
        'cbo_VechileNo.DataSource = dt4
        'cbo_VechileNo.DisplayMember = "Vechile_No"


        ' cbo_BobinEnds.Visible = False
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then
        '    dgv_KuriDetails.Columns(7).HeaderText = "MARK"
        '    dgv_KuriDetails.Columns(4).Visible = False
        '    dgv_KuriDetails.Columns(5).Visible = False
        '    dgv_KuriDetails.Columns(6).Visible = False
        'End If

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_EndsCount_Stock.Visible = False
        pnl_EndsCount_Stock.Left = 50
        pnl_EndsCount_Stock.Top = 200
        pnl_EndsCount_Stock.BringToFront()

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinEnds.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinMillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinColour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinBorderSize.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_KuriColour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_KuriCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_KuriBorderSize.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_millName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VechileNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OurBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_EndsName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_BobinEnds.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinColour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinMillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinBorderSize.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_KuriCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_KuriColour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_KuriBorderSize.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_millName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VechileNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OurBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_EndsName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyBobin.KeyPress, AddressOf TextBoxControlKeyPress

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

    Private Sub Weaver_Bobin_Delivery_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
        Common_Procedures.Hide_CurrentStock_Display()
    End Sub

    Private Sub Weaver_Bobin_Delivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

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

        If ActiveControl.Name = dgv_BobinDetails.Name Or ActiveControl.Name = dgv_KuriDetails.Name Or ActiveControl.Name = dgv_Req_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_BobinDetails.Name Then
                dgv1 = dgv_BobinDetails

            ElseIf dgv_BobinDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_BobinDetails

            ElseIf ActiveControl.Name = dgv_KuriDetails.Name Then
                dgv1 = dgv_KuriDetails

            ElseIf dgv_KuriDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_KuriDetails
            ElseIf ActiveControl.Name = dgv_Req_Details.Name Then
                dgv1 = dgv_Req_Details

            ElseIf dgv_Req_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Req_Details

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_BobinDetails.Name.ToString)) Then
                dgv1 = dgv_BobinDetails

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_KuriDetails.Name.ToString)) Then
                dgv1 = dgv_KuriDetails
            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_Req_Details.Name.ToString)) Then
                dgv1 = dgv_Req_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If dgv1.Name = dgv_BobinDetails.Name Then
                                    txt_PartyBobin.Focus()
                                ElseIf dgv1.Name = dgv_KuriDetails.Name Then
                                    If dgv_Req_Details.Rows.Count > 0 Then


                                        dgv_Req_Details.Focus()
                                        dgv_Req_Details.CurrentCell = dgv_Req_Details.Rows(0).Cells(5)
                                    Else
                                        txt_Remarks.Focus()
                                    End If
                                
                                    Else
                                        txt_Remarks.Focus()
                                    End If

                            Else

                                If dgv1.Name = dgv_Req_Details.Name Then
                               
                                   
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(5)
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If
                            End If
                        ElseIf dgv1.Name = dgv_Req_Details.Name Then
                            If .CurrentCell.ColumnIndex >= 5 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    txt_Remarks.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(5)
                                End If

                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If dgv1.Name = dgv_BobinDetails.Name Then
                                    txt_Freight.Focus()

                                ElseIf dgv1.Name = dgv_Req_Details.Name Then
                                    If dgv_KuriDetails.Rows.Count > 0 Then


                                        dgv_KuriDetails.Focus()
                                        dgv_KuriDetails.CurrentCell = dgv_KuriDetails.Rows(0).Cells(1)
                                    Else
                                        If dgv_BobinDetails.Rows.Count > 0 Then


                                            dgv_BobinDetails.Focus()
                                            dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                                        Else
                                            txt_Freight.Focus()
                                        End If
                                    End If
                                Else
                                    If dgv_BobinDetails.Rows.Count > 0 Then


                                        dgv_BobinDetails.Focus()
                                        dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                                    Else
                                        txt_Freight.Focus()
                                    End If
                                    End If

                            Else


                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)
                            End If
                        ElseIf .CurrentCell.ColumnIndex = 5 Then
                            If dgv1.Name = dgv_BobinDetails.Name Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)
                            End If
                        ElseIf .CurrentCell.ColumnIndex = 7 Then
                            If dgv1.Name = dgv_KuriDetails.Name Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)
                            ElseIf dgv1.Name = dgv_BobinDetails.Name Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(6)
                            End If
                        ElseIf .CurrentCell.ColumnIndex = 5 Then
                            If dgv1.Name = dgv_Req_Details.Name And .CurrentCell.RowIndex = 0 Then
                                If dgv_KuriDetails.Rows.Count > 0 Then


                                    dgv_KuriDetails.Focus()
                                    dgv_KuriDetails.CurrentCell = dgv_KuriDetails.Rows(0).Cells(1)
                                Else
                                    If dgv_BobinDetails.Rows.Count > 0 Then


                                        dgv_BobinDetails.Focus()
                                        dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                                    Else
                                        txt_Freight.Focus()
                                    End If
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(5)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Bobin_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Bobin_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            cmd.CommandText = "Update Weaver_PavuBobin_Requirement_Details set Noof_Bobin_Return = a.Noof_Bobin_Return - (b.Noof_Bobin)  from Weaver_PavuBobin_Requirement_Details a, Weaver_Bobin_Delivery_Requirement_Details b Where b.Weaver_Bobin_Delivery_Requirement_Code = '" & Trim(NewCode) & "' and a.Weaver_PavuBobin_Requirement_Code = b.Weaver_PavuBobin_Requirement_Code and a.Weaver_PavuBobin_Requirement_SlNo = b.Weaver_PavuBobin_Requirement_SlNo"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Weaver_Bobin_Delivery_Requirement_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Bobin_Delivery_Requirement_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_BobinJari_Delivery_Bobin_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_BobinJari_Delivery_Jari_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_BobinJari_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"
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

        If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

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

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Jari_Delivery_No from Weaver_BobinJari_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Bobin_Jari_Delivery_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Jari_Delivery_No from Weaver_BobinJari_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Bobin_Jari_Delivery_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Jari_Delivery_No from Weaver_BobinJari_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bobin_Jari_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Jari_Delivery_No from Weaver_BobinJari_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bobin_Jari_Delivery_No desc", con)
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

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_BobinJari_Delivery_Head", "Bobin_Jari_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red
            msk_date.Text = Date.Today.ToShortDateString
            If msk_date.Enabled And msk_date.Visible Then
                msk_date.Focus()
                msk_date.SelectionStart = 0
            End If


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

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Bobin_Jari_Delivery_No from Weaver_BobinJari_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(RecCode) & "'", con)
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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Bobin_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Bobin_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW DELIVERY INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Bobin_Jari_Delivery_No from Weaver_BobinJari_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Our_Bobin As Single = 0
        Dim Ens_ID As Integer = 0
        Dim ReqEns_ID As Integer = 0
        Dim BMill_ID As Integer = 0
        Dim Mill_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Slno As Integer = 0
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
        Dim vEdsCntGrp_ID As Integer = 0
        Dim PBlNo As String = ""
        Dim vTotBbns As Single, vTotMtrs As Single, ReqQty As Single
        Dim vTotJumbo As Single, vTotCns As Single, vTotWgt As Single
        Dim Nr As Integer = 0
        Dim vOrdByNo As Single = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Bobin_Delivery_Entry, New_Entry) = False Then Exit Sub

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
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then
        '    If Negative_Stock() = True Then
        '        MessageBox.Show("Invalid Bobin Stock ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        Exit Sub
        '    End If
        'End If
        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Delv_ID = Led_ID

        Rec_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        With dgv_BobinDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(6).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(6).Value) = 0 Then
                        MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled Then .Focus()
                        .CurrentCell = .Rows(0).Cells(6)
                        Exit Sub
                    End If

                End If

            Next
        End With

        With dgv_KuriDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(7).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then

                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If

                        Exit Sub

                    End If

                    If Val(.Rows(i).Cells(7).Value) = 0 Then


                        MessageBox.Show("Invalid Mark..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        If .Enabled Then .Focus()
                        .CurrentCell = .Rows(i).Cells(7)
                        Exit Sub
                    End If

                End If

            Next

        End With

        Total_Calculation()

        vTotBbns = 0 : vTotMtrs = 0 : ReqQty = 0
        With dgv_Req_Details
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(5).Value) <> 0 Then
                    ReqQty = ReqQty + Val(.Rows(i).Cells(5).Value())
                End If
            Next
        End With
        If dgv_BobinDetails_Total.RowCount > 0 Then
            vTotBbns = Val(dgv_BobinDetails_Total.Rows(0).Cells(5).Value())
            vTotMtrs = Val(dgv_BobinDetails_Total.Rows(0).Cells(7).Value())
        End If

        vTotJumbo = 0 : vTotCns = 0 : vTotWgt = 0
        If dgv_KuriDetails.RowCount > 0 Then
            vTotJumbo = Val(dgv_KuriDetails.Rows(0).Cells(5).Value())
            vTotCns = Val(dgv_KuriDetails.Rows(0).Cells(6).Value())
            vTotWgt = Val(dgv_KuriDetails.Rows(0).Cells(7).Value())
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then
            txt_OurBobin.Text = Val(vTotBbns)


        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1235" Then
            If (Val(txt_OurBobin.Text) + Val(txt_PartyBobin.Text)) <> Val(vTotBbns) Then
                MessageBox.Show("Invalid Bobins..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_PartyBobin.Enabled Then txt_PartyBobin.Focus()
                Exit Sub
            End If
        End If
        'If dgv_Req_Details.Rows.Count > 0 Then
        '    With dgv_BobinDetails

        '        ' For j = 0 To dgv_Req_Details.RowCount - 1
        '        For i = 0 To .RowCount - 1
        '            If Trim(.Rows(i).Cells(1).Value) <> "" Then

        '                If Trim(dgv_Req_Details.Rows(i).Cells(8).Value) <> Trim(.Rows(i).Cells(1).Value) Then

        '                    MessageBox.Show("Mismatch Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '                    If .Enabled And .Visible Then
        '                        .Focus()
        '                        .CurrentCell = .Rows(i).Cells(1)
        '                    End If
        '                    Exit Sub
        '                End If

        '                If Val(dgv_Req_Details.Rows(i).Cells(5).Value) <> Val(.Rows(i).Cells(5).Value) Then

        '                    MessageBox.Show("Mismatch Bobin", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '                    If .Enabled And .Visible Then
        '                        .Focus()
        '                        .CurrentCell = .Rows(i).Cells(5)
        '                    End If
        '                    Exit Sub
        '                End If

        '            End If

        '        Next
        '    End With
        'End If

        'If (Val(ReqQty)) <> Val(vTotBbns) Then
        '    MessageBox.Show("Mismatch Bobins..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If dgv_BobinDetails.Enabled Then dgv_BobinDetails.Focus()
        '    dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(5)
        '    Exit Sub
        'End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_BobinJari_Delivery_Head", "Bobin_Jari_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            'Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_BobinJari_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "' and BobinSales_Invoice_Code <> ''", con)
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
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            vOrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_BobinJari_Delivery_Head ( Bobin_Jari_Delivery_Code, Company_IdNo, Bobin_Jari_Delivery_No, for_OrderBy, Bobin_Jari_Delivery_Date, Ledger_IdNo, Vechile_No, Freight, Transport_IdNo, Total_Bobins, Total_Meters, Total_Jumbos, Total_Cones, Total_Weight, Party_Bobin, OurOwn_Bobin, Remarks ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate, " & Str(Val(Led_ID)) & ",'" & Trim(cbo_VechileNo.Text) & "'," & Str(Val(txt_Freight.Text)) & "," & Str(Val(Trans_ID)) & ", " & Str(Val(vTotBbns)) & " , " & Str(Val(vTotMtrs)) & ",  " & Str(Val(vTotJumbo)) & " , " & Str(Val(vTotCns)) & ",  " & Str(Val(vTotWgt)) & " , " & Str(Val(txt_PartyBobin.Text)) & ", " & Str(Val(txt_OurBobin.Text)) & ", '" & Trim(txt_Remarks.Text) & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Weaver_BobinJari_Delivery_Head set Bobin_Jari_Delivery_Date = @EntryDate, Ledger_IdNo = " & Val(Led_ID) & ", Vechile_No = '" & Trim(cbo_VechileNo.Text) & "', Freight = " & Str(Val(txt_Freight.Text)) & ", Transport_IdNo = " & Str(Val(Trans_ID)) & ", Total_Bobins = " & Val(vTotBbns) & " , Total_Meters = " & Val(vTotMtrs) & ", Total_Jumbos = " & Val(vTotJumbo) & ", Total_Cones = " & Val(vTotCns) & ", Total_Weight = " & Val(vTotWgt) & ", Party_Bobin = " & Str(Val(txt_PartyBobin.Text)) & " , OurOwn_Bobin = " & Str(Val(txt_OurBobin.Text)) & ", Remarks = '" & Trim(txt_Remarks.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Update Weaver_PavuBobin_Requirement_Details set Noof_Bobin_Return = a.Noof_Bobin_Return - (b.Noof_Bobin)  from Weaver_PavuBobin_Requirement_Details a, Weaver_Bobin_Delivery_Requirement_Details b Where b.Weaver_Bobin_Delivery_Requirement_Code = '" & Trim(NewCode) & "' and a.Weaver_PavuBobin_Requirement_Code = b.Weaver_PavuBobin_Requirement_Code and a.Weaver_PavuBobin_Requirement_SlNo = b.Weaver_PavuBobin_Requirement_SlNo"
                cmd.ExecuteNonQuery()
            End If

            Partcls = "Bob/Jari Delv : Dc.No. " & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
                Partcls = "Dc.No. " & Trim(lbl_DcNo.Text) & "-" & Val(vTotBbns) & "Bobin" & "-" & Val(vTotMtrs) & "Mtrs"
            End If

            cmd.CommandText = "Delete from Weaver_BobinJari_Delivery_Bobin_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_BobinJari_Delivery_Jari_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Weaver_Bobin_Delivery_Requirement_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Bobin_Delivery_Requirement_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_BobinDetails
                Sno = 0
                Slno = 100
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(6).Value) <> 0 Then

                        Sno = Sno + 1
                        Slno = Slno + 1

                        Ens_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        vEdsCntGrp_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_Stockunder_IdNo", "(EndsCount_IdNo = " & Str(Val(Ens_ID)) & ")", , tr))
                        BMill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Clr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        BthSz_ID = Common_Procedures.BorderSize_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)

                        'Nr = 0
                        'cmd.CommandText = "Update  Weaver_BobinJari_Delivery_Bobin_Details set Bobin_Jari_Delivery_Date = @EntryDate , Sl_No  = " & Str(Val(Sno)) & " , EndsCount_IdNo = " & Str(Val(Ens_ID)) & "  , Colour_IdNo = " & Str(Val(Clr_ID)) & "  , BorderSize_IdNo = " & Str(Val(BthSz_ID)) & " , Bobins = " & Val(.Rows(i).Cells(4).Value) & " , Meter_Bobin = " & Val(.Rows(i).Cells(5).Value) & " , Meters = " & Val(.Rows(i).Cells(6).Value) & " , Bobin_Jari_Sales_Invoice_Code = '" & Trim(.Rows(i).Cells(7).Value) & "'  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'"
                        'Nr = cmd.ExecuteNonQuery()

                        'If Nr = 0 Then

                        cmd.CommandText = "Insert into Weaver_BobinJari_Delivery_Bobin_Details ( Bobin_Jari_Delivery_Code, Company_IdNo, Bobin_Jari_Delivery_No, for_OrderBy, Bobin_Jari_Delivery_Date, Sl_No, EndsCount_IdNo,Mill_Idno , Colour_IdNo, BorderSize_IdNo, Bobins, Meter_Bobin, Meters  ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate," & Str(Val(Sno)) & ", " & Str(Val(Ens_ID)) & "," & Val(BMill_ID) & " , " & Str(Val(Clr_ID)) & " , " & Str(Val(BthSz_ID)) & ", " & Val(.Rows(i).Cells(5).Value) & ", " & Val(.Rows(i).Cells(6).Value) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " )"
                        cmd.ExecuteNonQuery()

                        'End If

                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, StockOf_IdNo, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Colour_IdNo,Mill_IdNo,Meters_Bobin, Bobins, Meters ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate, 0, " & Str(Val(Rec_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Ens_ID)) & ", " & Str(Val(Clr_ID)) & "," & Str(Val(BMill_ID)) & "," & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " )"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, StockOf_IdNo, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Colour_IdNo,Mill_IdNo,Meters_Bobin, Bobins, Meters ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", 0 , 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Slno)) & ", " & Str(Val(vEdsCntGrp_ID)) & ", " & Str(Val(Clr_ID)) & ", " & Str(Val(BMill_ID)) & "," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

                'Sno = 0
                'For i = 0 To .RowCount - 1

                '    If Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(3).Value) <> 0 Then

                '        Ens_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                '        Sno = Sno + 1
                '        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Ens_ID)) & ", 0 ,  " & Str(Val(.Rows(i).Cells(4).Value)) & " )"
                '        cmd.ExecuteNonQuery()

                '    End If
                'Next

            End With

            With dgv_KuriDetails

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Clr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        BthSz_ID = Common_Procedures.BorderSize_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)

                        'Nr = 0
                        'cmd.CommandText = "Update  Weaver_BobinJari_Delivery_Jari_Details set Bobin_Jari_Delivery_Date = @EntryDate , Sl_No  = " & Str(Val(Sno)) & " , Count_IdNo = " & Str(Val(Cnt_ID)) & "  , Colour_IdNo = " & Str(Val(Clr_ID)) & "  , BorderSize_IdNo = " & Str(Val(BthSz_ID)) & " , Noof_Jumbos = " & Val(.Rows(i).Cells(4).Value) & " , Noof_Cones = " & Val(.Rows(i).Cells(5).Value) & " , Weight = " & Val(.Rows(i).Cells(6).Value) & " , Bobin_Jari_Sales_Invoice_Code = '" & Trim(.Rows(i).Cells(7).Value) & "'  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'  and Bobin_Jari_Delivery_Jari_Slno = " & Str(Val(.Rows(i).Cells(8).Value))
                        'Nr = cmd.ExecuteNonQuery()

                        'If Nr = 0 Then
                        cmd.CommandText = "Insert into Weaver_BobinJari_Delivery_Jari_Details ( Bobin_Jari_Delivery_Code, Company_IdNo, Bobin_Jari_Delivery_No, for_OrderBy, Bobin_Jari_Delivery_Date, Sl_No, Count_IdNo,Mill_IdNo ,  Colour_IdNo, BorderSize_IdNo, Noof_Jumbos, Noof_Cones, Weight ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate," & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & "," & Val(Mill_ID) & " , " & Str(Val(Clr_ID)) & " , " & Str(Val(BthSz_ID)) & ", " & Val(.Rows(i).Cells(5).Value) & ", " & Val(.Rows(i).Cells(6).Value) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ")"
                        cmd.ExecuteNonQuery()

                        'End If

                        'cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Colour_IdNo, Jumbo, Cones, Weight) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", 'MILL', " & Val(Mill_ID) & ", " & Str(Val(Clr_ID)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " )"
                        'cmd.ExecuteNonQuery()

                    End If

                Next

            End With
            With dgv_Req_Details
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(5).Value) <> 0 Then
                        ReqEns_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(8).Value, tr)
                        Sno = Sno + 1
                        Nr = 0

                        cmd.CommandText = "Insert into Weaver_Bobin_Delivery_Requirement_Details ( Weaver_Bobin_Delivery_Requirement_Code, Company_IdNo, Weaver_Bobin_Delivery_Requirement_No, for_OrderBy, Weaver_Bobin_Delivery_Requirement_Date, Sl_No, Ledger_IdNo, Requirement_No, Requirement_Date , loom_No, Mark, Noof_Bobin  ,Weaver_PavuBobin_Requirement_Code, Weaver_PavuBobin_Requirement_Slno ,EndsCount_Idno) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate," & Str(Val(Sno)) & ", " & Str(Val(Led_ID)) & ",'" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ",'" & Trim(.Rows(i).Cells(6).Value) & "', " & Str(Val(.Rows(i).Cells(7).Value)) & "," & Val(ReqEns_ID) & " )"
                        cmd.ExecuteNonQuery()


                        cmd.CommandText = "Update Weaver_PavuBobin_Requirement_Details set  Noof_Bobin_Return = Noof_Bobin_Return + " & Str(Val(.Rows(i).Cells(5).Value)) & "    Where Weaver_PavuBobin_Requirement_Code = '" & Trim(.Rows(i).Cells(6).Value) & "' and Weaver_PavuBobin_Requirement_SlNo = " & Str(Val(.Rows(i).Cells(7).Value)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            Throw New ApplicationException("Mismatch of Requirement and Party Details")
                        End If
                    End If

                Next
            End With
            If Val(txt_OurBobin.Text) <> 0 Or Val(txt_PartyBobin.Text) <> 0 Or Val(vTotJumbo) <> 0 Or Val(vTotCns) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Particulars, Sl_No, Empty_Cones, Empty_Bobin, EmptyBobin_Party, Empty_Jumbo) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(vTotCns)) & ", " & Str(Val(txt_OurBobin.Text)) & ", " & Str(Val(txt_PartyBobin.Text)) & ", " & Str(Val(vTotJumbo)) & ")"
                cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            If InStr(1, Err.Description, "CK_Weaver_PavuBobin_Requirement_Details_1") > 0 Then
                MessageBox.Show("Invalid Delivery No.of.Bobin, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Err.Description, "CK_Weaver_PavuBobin_Requirement_Details_2") > 0 Then
                MessageBox.Show("Invalid  Delivery No.Of.Bobin, Delivery No.of.Bobin must be lesser than Requirement No.of.Bobin", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
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
            If InStr(1, Err.Description, "CK_Weaver_PavuBobin_Requirement_Details_1") > 0 Then
                MessageBox.Show("Invalid Delivery No.of.Bobin, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Err.Description, "CK_Weaver_PavuBobin_Requirement_Details_2") > 0 Then
                MessageBox.Show("Invalid  Delivery No.Of.Bobin, Delivery No.of.Bobin must be lesser than Requirement No.of.Bobin", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        End Try

        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

    End Sub

    Private Sub Total_Calculation()
        Dim vTotBbnS As Single, vTotMtrs As Single
        Dim vTotJumbo As Single, vTotCns As Single, vTotWgt As Single
        Dim i As Integer
        Dim sno As Integer

        vTotBbnS = 0 : vTotMtrs = 0
        With dgv_BobinDetails
            For i = 0 To .Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(.Rows(i).Cells(7).Value) <> 0 Then

                    vTotBbnS = vTotBbnS + Val(.Rows(i).Cells(5).Value)
                    vTotMtrs = vTotMtrs + Val(.Rows(i).Cells(7).Value)

                End If
            Next
        End With

        If dgv_BobinDetails_Total.Rows.Count <= 0 Then dgv_BobinDetails_Total.Rows.Add()
        dgv_BobinDetails_Total.Rows(0).Cells(5).Value = Val(vTotBbnS)
        dgv_BobinDetails_Total.Rows(0).Cells(7).Value = Format(Val(vTotMtrs), "#########0.00")

        vTotJumbo = 0 : vTotCns = 0 : vTotWgt = 0
        sno = 0
        With dgv_KuriDetails
            For i = 0 To .Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                    vTotJumbo = vTotJumbo + Val(.Rows(i).Cells(5).Value)
                    vTotCns = vTotCns + Val(.Rows(i).Cells(6).Value)
                    vTotWgt = vTotWgt + Val(.Rows(i).Cells(7).Value)
                End If

            Next
        End With

        If dgv_KuriDetails_Total.Rows.Count <= 0 Then dgv_KuriDetails_Total.Rows.Add()
        dgv_KuriDetails_Total.Rows(0).Cells(5).Value = Val(vTotJumbo)
        dgv_KuriDetails_Total.Rows(0).Cells(6).Value = Val(vTotCns)
        dgv_KuriDetails_Total.Rows(0).Cells(7).Value = Format(Val(vTotWgt), "#########0.000")

    End Sub

    Private Sub Meters_Calculation()
        Dim i As Integer
        Dim sno As Integer
        Dim vtotMtrs As Single

        vtotMtrs = 0 : sno = 0
        With dgv_BobinDetails
            For i = 0 To dgv_BobinDetails.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno


                vtotMtrs = Val(dgv_BobinDetails.Rows(i).Cells(5).Value) * Val(dgv_BobinDetails.Rows(i).Cells(6).Value)

                dgv_BobinDetails.Rows(i).Cells(7).Value = Format(Val(vtotMtrs), "#########0.00")

            Next
        End With
        Total_Calculation()

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_date, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Requirement:", "FOR REQUIREMENT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                cbo_Transport.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgv_BobinDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEndEdit
        With dgv_BobinDetails

            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If

            Meters_Calculation()

        End With
    End Sub

    Private Sub dgv_BobinDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim rect As Rectangle

        With dgv_BobinDetails

            dgv_ActCtrlName = .Name.ToString

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_BobinEnds.Visible = False Or Val(cbo_BobinEnds.Tag) <> e.RowIndex Then

                    cbo_BobinEnds.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_BobinEnds.DataSource = Dt2
                    cbo_BobinEnds.DisplayMember = "EndsCount_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_BobinEnds.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_BobinEnds.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_BobinEnds.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_BobinEnds.Height = rect.Height  ' rect.Height

                    cbo_BobinEnds.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_BobinEnds.Tag = Val(e.RowIndex)
                    cbo_BobinEnds.Visible = True

                    cbo_BobinEnds.BringToFront()
                    cbo_BobinEnds.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If
            Else
                cbo_BobinEnds.Visible = False


            End If
            If e.ColumnIndex = 2 Then

                If cbo_BobinMillName.Visible = False Or Val(cbo_BobinMillName.Tag) <> e.RowIndex Then

                    cbo_BobinMillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_BobinMillName.DataSource = Dt2
                    cbo_BobinMillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_BobinMillName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_BobinMillName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_BobinMillName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_BobinMillName.Height = rect.Height  ' rect.Height

                    cbo_BobinMillName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_BobinMillName.Tag = Val(e.RowIndex)
                    cbo_BobinMillName.Visible = True

                    cbo_BobinMillName.BringToFront()
                    cbo_BobinMillName.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If
            Else
                cbo_BobinMillName.Visible = False


            End If

            If e.ColumnIndex = 3 Then

                If cbo_BobinColour.Visible = False Or Val(cbo_BobinColour.Tag) <> e.RowIndex Then

                    cbo_BobinColour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_BobinColour.DataSource = Dt2
                    cbo_BobinColour.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_BobinColour.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_BobinColour.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_BobinColour.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_BobinColour.Height = rect.Height  ' rect.Height

                    cbo_BobinColour.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_BobinColour.Tag = Val(e.RowIndex)
                    cbo_BobinColour.Visible = True

                    cbo_BobinColour.BringToFront()
                    cbo_BobinColour.Focus()

                End If

            Else

                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""
                cbo_BobinColour.Visible = False

            End If

            If e.ColumnIndex = 4 Then

                If cbo_BobinBorderSize.Visible = False Or Val(cbo_BobinBorderSize.Tag) <> e.RowIndex Then

                    cbo_BobinBorderSize.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select BorderSize_Name from BorderSize_Head order by BorderSize_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_BobinBorderSize.DataSource = Dt2
                    cbo_BobinBorderSize.DisplayMember = "BorderSize_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_BobinBorderSize.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_BobinBorderSize.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_BobinBorderSize.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_BobinBorderSize.Height = rect.Height  ' rect.Height

                    cbo_BobinBorderSize.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_BobinBorderSize.Tag = Val(e.RowIndex)
                    cbo_BobinBorderSize.Visible = True

                    cbo_BobinBorderSize.BringToFront()
                    cbo_BobinBorderSize.Focus()



                End If

            Else

                cbo_BobinBorderSize.Visible = False


            End If



        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellLeave

        With dgv_BobinDetails

            If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            'If e.ColumnIndex = 1 Then
            '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then
            '        If Negative_Stock() = True Then
            '            MessageBox.Show("Invalid stock", "NEGATVE STOCK", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            '        End If
            '    End If
            'End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub
        With dgv_BobinDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 7 Then
                    Meters_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_BobinDetails.EditingControlShowing
        dgtxt_BobinDetails = CType(dgv_BobinDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_BobinDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BobinDetails.Enter
        dgv_ActCtrlName = dgv_BobinDetails.Name
        dgv_BobinDetails.EditingControl.BackColor = Color.Lime
        dgv_BobinDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_BobinDetails.SelectAll()
    End Sub

    Private Sub dgtxt_BobinDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_BobinDetails.KeyPress

        With dgv_BobinDetails



            If Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = 5 Or Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = 6 Or Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = 7 Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BobinDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_BobinDetails

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

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_BobinDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub
        With dgv_BobinDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub dgv_BobinDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BobinDetails.LostFocus
        dgv_BobinDetails.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_BobinEnds_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinEnds.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "Cotton_Polyester_Jari <>'COTTON'", "(EndsCount_IdNo = 0)")
        Bobin_Stock_Checking()
    End Sub

    Private Sub cbo_Ends_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinEnds.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinEnds, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "Cotton_Polyester_Jari <>'COTTON'", "(EndsCount_IdNo = 0)")

        With dgv_BobinDetails

            If (e.KeyValue = 38 And cbo_BobinEnds.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    txt_Freight.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_BobinEnds.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If dgv_KuriDetails.Rows.Count > 0 Then


                        dgv_KuriDetails.Focus()
                        dgv_KuriDetails.CurrentCell = dgv_KuriDetails.Rows(0).Cells(1)
                    Else
                        If dgv_Req_Details.Rows.Count > 0 Then


                            dgv_Req_Details.Focus()
                            dgv_Req_Details.CurrentCell = dgv_Req_Details.Rows(0).Cells(5)
                        Else
                            txt_Remarks.Focus()
                        End If
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

                End If

        End With

    End Sub

    Private Sub cbo_Ends_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinEnds.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinEnds, Nothing, "EndsCount_Head", "EndsCount_Name", "Cotton_Polyester_Jari <>'COTTON'", "(EndsCount_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_BobinDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_BobinEnds.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    If dgv_KuriDetails.Rows.Count > 0 Then


                        dgv_KuriDetails.Focus()
                        dgv_KuriDetails.CurrentCell = dgv_KuriDetails.Rows(0).Cells(1)
                    Else
                        If dgv_Req_Details.Rows.Count > 0 Then


                            dgv_Req_Details.Focus()
                            dgv_Req_Details.CurrentCell = dgv_Req_Details.Rows(0).Cells(5)
                        Else
                            txt_Remarks.Focus()
                        End If
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End If
    End Sub

    Private Sub cbo_Ends_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinEnds.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinEnds.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinEnds.TextChanged
        Try
            If cbo_BobinEnds.Visible Then
                With dgv_BobinDetails
                    If Val(cbo_BobinEnds.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinEnds.Text)
                    End If
                End With
            End If
            Bobin_Stock_Checking()
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinColour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_BorderName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinColour.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinColour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
        With dgv_BobinDetails

            If (e.KeyValue = 38 And cbo_BobinColour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_BobinColour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(5)
            End If

        End With
    End Sub

    Private Sub cbo_BorderName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinColour.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinColour, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_BobinDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(3).Value = Trim(cbo_BobinColour.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)

            End With
        End If


    End Sub
    Private Sub cbo_BorderName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinColour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinColour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_BorderName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinColour.TextChanged
        Try
            If cbo_BobinColour.Visible Then
                With dgv_BobinDetails
                    If Val(cbo_BobinColour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinColour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_BorderSize_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinBorderSize.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinBorderSize, Nothing, Nothing, "BorderSize_Head", "BorderSize_Name", "", "(BorderSize_IdNo = 0)")

        With dgv_BobinDetails

            If (e.KeyValue = 38 And cbo_BobinBorderSize.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_BobinBorderSize.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub
    Private Sub cbo_BorderSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinBorderSize.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinBorderSize, Nothing, "BorderSize_Head", "BorderSize_Name", "", "(BorderSize_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_BobinDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(4).Value = Trim(cbo_BobinBorderSize.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_BorderSize_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinBorderSize.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New BorderSize_Creation()

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinBorderSize.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_BorderSize_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinBorderSize.TextChanged
        Try
            If cbo_BobinBorderSize.Visible Then
                With dgv_BobinDetails
                    If Val(cbo_BobinBorderSize.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinBorderSize.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VechileNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VechileNo, cbo_Transport, txt_Freight, "Weaver_BobinJari_Delivery_Head", "Vechile_No", "", "")

    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VechileNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VechileNo, txt_Freight, "Weaver_BobinJari_Delivery_Head", "Vechile_No", "", "", False)
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_Ledger, cbo_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
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
        If e.Control = False And e.KeyValue = 17 Then

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

    Private Sub dgv_Details_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BobinDetails.GotFocus
        dgv_BobinDetails.Focus()
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
                Condt = "a.Bobin_Jari_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Bobin_Jari_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Bobin_Jari_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_BobinJari_Delivery_Head IN (select z1.Weaver_BobinJari_Delivery_Head from Weaver_BobinJari_Delivery_Bobin_Details z1 where z1.Ends = '" & Trim(cbo_Filter_EndsName.Text) & "')"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Weaver_BobinJari_Delivery_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Jari_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Bobin_Jari_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Bobin_Jari_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Bobin_Jari_Delivery_Date").ToString), "dd-MM-yyyy")
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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        pnl_Print.Visible = True
        pnl_Back.Enabled = False
        If btn_Print_Delivery.Enabled And btn_Print_Delivery.Visible Then
            btn_Print_Delivery.Focus()
        End If
    End Sub

    Private Sub btn_Print_Delivery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Delivery.Click
        Printing_Delivery()
        btn_print_Close_Click(sender, e)
    End Sub


    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
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
        'Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_BobinJari_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        set_PaperSize_For_PrintDocument1()

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName from Weaver_BobinJari_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                cmd.Connection = con
                cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "( Name1   , Name2          ,Name3     ,Name4          ,Name5 ,Name6      ,Meters1  ) " & _
                                        "select  b.EndscOUNT_Name , c.Colour_name , a.Bobins , ''  , a.Meters        , d.Mill_Name,a.Meter_Bobin from Weaver_BobinJari_Delivery_Bobin_Details a INNER JOIN EndscOUNT_Head b ON a.EndscOUNT_idno = b.endscOUNT_idno LEFT OUTER JOIN Colour_Head c ON a.Colour_idno = c.Colour_idno  LEFT OUTER JOIN Mill_Head d ON a.Mill_idno = d.Mill_idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No"
                nr = cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "( Name1   , Name2          ,Name3     ,Name4          ,Name5   ,   Name6         , Meters1) " & _
                                           "select  b.cOUNT_Name , c.Colour_name , a.Noof_Jumbos , a.Noof_Cones  , a.Weight ,d.Mill_Name , 0      from Weaver_BobinJari_Delivery_Jari_Details a INNER JOIN cOUNT_Head b ON a.cOUNT_idno = b.cOUNT_idno LEFT OUTER JOIN Colour_Head c ON a.Colour_idno = c.Colour_idno LEFT OUTER JOIN Mill_Head d ON a.Mill_idno = d.Mill_idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Jari_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No"
                nr = cmd.ExecuteNonQuery()

                da2 = New SqlClient.SqlDataAdapter("select a.Name1   , a.Name2  , a.Name3  , a.Name4  , a.Name5 ,Name6,Meters1 from  " & Trim(Common_Procedures.EntryTempTable) & " a  Order by a.Name1", con)
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
        Dim ItmNm3 As String, ItmNm4 As String
        Dim SNo As Integer

        set_PaperSize_For_PrintDocument1()

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10 ' 30
            .Right = 40
            .Top = 15 ' 30
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

        NoofItems_PerPage = 4

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        'ClAr(1) = 35 : ClAr(2) = 135 : ClAr(3) = 135 : ClAr(4) = 80 : ClAr(5) = 80
        'ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        ClAr(1) = 35 : ClAr(2) = 150 : ClAr(3) = 220 : ClAr(4) = 70 : ClAr(5) = 70 : ClAr(6) = 80
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

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

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Name1").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 18 Then
                            For I = 18 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 18
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        ItmNm3 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Name6").ToString)
                        ItmNm4 = ""
                        If Len(ItmNm3) > 22 Then
                            For I = 22 To 1 Step -1
                                If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 22
                            ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                            ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Name3").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Name3").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Name4").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Name4").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters1").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters1").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Name5").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Name5").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        End If

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

        da2 = New SqlClient.SqlDataAdapter("select a.* from " & Trim(Common_Procedures.EntryTempTable) & " a ", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "BOBIN DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        'C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then '---- SOMANUR KALPANA COTTON (TETILES) (THOTTIPALAYAM)  
            Common_Procedures.Print_To_PrintDocument(e, "J-" & prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        End If


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bobin_Jari_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10

        ''If prn_HdDt.Rows(0).Item("Party_OrderNo").ToString <> "" Then
        ''    Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        ''    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        ''    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        ''End If

        CurY = CurY + TxtHgt + 20
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        CurY = CurY + TxtHgt - 10
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
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT/", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1), CurY + TxtHgt, 2, ClAr(2), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "MILL", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BOBINS/", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "JUMBO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BOBIN", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "METERS/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)

        CurY = CurY + TxtHgt + TxtHgt
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

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL BOBIN ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bobins").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + s2 + C1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL JUMPO ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Jumbos").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL CONES ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Cones").ToString, LMargin + s2 + C1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL WEIGHT ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#######0.000"), LMargin + s2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        'vprn_BlNos = ""
        'For i = 0 To prn_DetDt.Rows.Count - 1
        '    If Trim(prn_DetDt.Rows(i).Item("Bales_Nos").ToString) <> "" Then
        '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(i).Item("Bales_Nos").ToString
        '    End If
        'Next
        'Common_Procedures.Print_To_PrintDocument(e, "BALES NOS : " & vprn_BlNos, LMargin + 10, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt + 25

        'Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        'p1Font = New Font("Calibri", 12, FontStyle.Bold)


        'Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt
        '   If Val(Common_Procedures.User.IdNo) <> 1 Then
        Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 300, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        ' CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        'Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)


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

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_PartyBobin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PartyBobin.KeyDown
        If e.KeyValue = 38 Then
            If dgv_BobinDetails.Rows.Count > 0 Then
                dgv_BobinDetails.Focus()
                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                dgv_BobinDetails.CurrentCell.Selected = True

            Else
                txt_OurBobin.Focus()

            End If
        End If

        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_PartyBobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PartyBobin.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If dgv_BobinDetails.Rows.Count > 0 Then
                dgv_BobinDetails.Focus()
                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                dgv_BobinDetails.CurrentCell.Selected = True

            Else
                txt_PartyBobin.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If dgv_BobinDetails.Rows.Count > 0 Then
                dgv_BobinDetails.Focus()
                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                dgv_BobinDetails.CurrentCell.Selected = True

            Else
                txt_PartyBobin.Focus()

            End If
        End If
    End Sub

    Private Sub txt_OutBobin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OurBobin.KeyDown
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
    End Sub

    Private Sub txt_OutBobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OurBobin.KeyPress
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
    End Sub

    Private Sub cbo_KuriCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KuriCount.GotFocus
        pnl_EndsCount_Stock.Visible = False
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_KuriCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KuriCount.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_KuriCount, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_KuriDetails

            If (e.KeyValue = 38 And cbo_KuriCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    If dgv_BobinDetails.Rows.Count > 0 Then


                        dgv_BobinDetails.Focus()
                        dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                    Else
                        txt_Freight.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)
                    .CurrentCell.Selected = True

                End If

            End If

            If (e.KeyValue = 40 And cbo_KuriCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If dgv_Req_Details.Rows.Count > 0 Then


                        dgv_Req_Details.Focus()
                        dgv_Req_Details.CurrentCell = dgv_Req_Details.Rows(0).Cells(5)
                    Else
                        txt_Remarks.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_KuriCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_KuriCount.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_KuriCount, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_KuriDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_KuriCount.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    If dgv_Req_Details.Rows.Count > 0 Then


                        dgv_Req_Details.Focus()
                        dgv_Req_Details.CurrentCell = dgv_Req_Details.Rows(0).Cells(5)
                    Else
                        txt_Remarks.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End If
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

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_KuriColour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
        With dgv_KuriDetails

            If (e.KeyValue = 38 And cbo_KuriColour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_KuriColour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(7)
            End If

        End With
    End Sub

    Private Sub cbo_KuriColour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_KuriColour.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_KuriColour, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_KuriDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(3).Value = Trim(cbo_KuriColour.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(7)

            End With
        End If


    End Sub

    Private Sub cbo_KuriColour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KuriColour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_KuriColour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_KuriColour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KuriColour.TextChanged

        Try
            If cbo_KuriColour.Visible Then
                With dgv_KuriDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_KuriColour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_KuriColour.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_millName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_millName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_millName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_millName.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_millName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        With dgv_KuriDetails

            If (e.KeyValue = 38 And cbo_millName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_millName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_millName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_millName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_millName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_KuriDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value = Trim(cbo_millName.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With
        End If


    End Sub

    Private Sub cbo_millName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_millName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_millName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_millName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_millName.TextChanged

        Try
            If cbo_millName.Visible Then
                With dgv_KuriDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_millName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_millName.Text)
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

    End Sub

    Private Sub cbo_KuriBorderSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_KuriBorderSize.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_KuriBorderSize, Nothing, "BorderSize_Head", "BorderSize_Name", "", "(BorderSize_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_KuriDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(4).Value = Trim(cbo_KuriBorderSize.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_KuriBorderSize_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KuriBorderSize.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New BorderSize_Creation()

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_KuriBorderSize.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_KuriBorderSize_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KuriBorderSize.TextChanged

        Try
            If cbo_KuriBorderSize.Visible Then
                With dgv_KuriDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_KuriBorderSize.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
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

                    If .CurrentCell.ColumnIndex = 7 Then
                        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
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

                If cbo_millName.Visible = False Or Val(cbo_millName.Tag) <> e.RowIndex Then

                    dgv_ActCtrlName = dgv_KuriDetails.Name

                    cbo_millName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head Order by Mill_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_millName.DataSource = Dt2
                    cbo_millName.DisplayMember = "Mill_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_millName.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_millName.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_millName.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_millName.Height = Rect.Height  ' rect.Height

                    cbo_millName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_millName.Tag = Val(e.RowIndex)
                    cbo_millName.Visible = True

                    cbo_millName.BringToFront()
                    cbo_millName.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_millName.Visible = False

            End If
            If e.ColumnIndex = 3 Then

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

            If e.ColumnIndex = 4 Then

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

    End Sub

    Private Sub dgv_KuriDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellLeave
        Try
            With dgv_KuriDetails
                If .CurrentCell.ColumnIndex = 7 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
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
                        If .CurrentCell.ColumnIndex = 7 Then
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
        dgtxt_KuriDetails = CType(dgv_KuriDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_KuriDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_KuriDetails.Enter
        dgv_ActCtrlName = dgv_KuriDetails.Name
        dgv_KuriDetails.EditingControl.BackColor = Color.Lime
        dgv_KuriDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_KuriDetails.SelectAll()
    End Sub

    Private Sub dgtxt_KuriDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_KuriDetails.KeyPress

        With dgv_KuriDetails

            If Val(dgv_KuriDetails.CurrentCell.ColumnIndex.ToString) = 5 Or Val(dgv_KuriDetails.CurrentCell.ColumnIndex.ToString) = 6 Or Val(dgv_KuriDetails.CurrentCell.ColumnIndex.ToString) = 7 Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If
            End If

        End With

    End Sub

    Private Sub dgv_KuriDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_KuriDetails.KeyUp
        Dim n As Integer

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

    End Sub

    Private Sub dgv_KuriDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_KuriDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_KuriDetails.CurrentCell) Then Exit Sub
        With dgv_KuriDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With

    End Sub

    Private Sub dgv_KuriDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_KuriDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_KuriDetails.CurrentCell) Then dgv_KuriDetails.CurrentCell.Selected = False
    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_Ledger.Focus()
        End If
    End Sub
    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
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

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_date.Focus()
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
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_Ledger.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Remarks.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bobin As Single = 0

        Dim nr As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT REQUIREMENT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.* ,   h.Noof_Bobin As Ent_Bobin, c.EndsCount_Name  from Weaver_PavuBobin_Requirement_Details a  INNER JOIN EndsCount_Head c ON c.EndsCount_Idno = a.EndsCount_IdNo  LEFT OUTER JOIN Weaver_Bobin_Delivery_Requirement_Details h ON h.Weaver_Bobin_Delivery_Requirement_Code = '" & Trim(NewCode) & "' and a.Weaver_PavuBobin_Requirement_Code = h.Weaver_PavuBobin_Requirement_Code and a.Weaver_PavuBobin_Requirement_SlNo = h.Weaver_PavuBobin_Requirement_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((a.Noof_Bobin - a.Noof_Bobin_Return ) > 0 or h.Noof_Bobin > 0 ) order by a.Weaver_PavuBobin_Requirement_Date, a.for_orderby, a.Weaver_PavuBobin_Requirement_No", con)
            Dt1 = New DataTable
            nr = Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()


                    Ent_Bobin = 0


                    If IsDBNull(Dt1.Rows(i).Item("Ent_Bobin").ToString) = False Then
                        Ent_Bobin = Val(Dt1.Rows(i).Item("Ent_Bobin").ToString)
                    End If


                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Weaver_PavuBobin_Requirement_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_PavuBobin_Requirement_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("loom_No").ToString

                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Mark").ToString

                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Noof_Bobin").ToString) - Val(Dt1.Rows(i).Item("Noof_Bobin_Return").ToString) + Val(Ent_Bobin), "#########0.00")

                    If Ent_Bobin > 0 Then
                        .Rows(n).Cells(6).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(6).Value = ""

                    End If
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Weaver_PavuBobin_Requirement_Code").ToString
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Weaver_PavuBobin_Requirement_Slno").ToString


                    .Rows(n).Cells(9).Value = Ent_Bobin
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString

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

        dgv_Req_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(6).Value) = 1 Then


                n = dgv_Req_Details.Rows.Add()
                sno = sno + 1
                dgv_Req_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Req_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Req_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Req_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Req_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(4).Value


                ' dgv_Req_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Req_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Req_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
                dgv_Req_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(10).Value
                If Val(dgv_Selection.Rows(i).Cells(9).Value) <> 0 Then
                    dgv_Req_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(9).Value
                Else
                    dgv_Req_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(5).Value
                End If


            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        '  pnl_Back.Visible = True
        pnl_Selection.Visible = False
        If cbo_Transport.Enabled And cbo_Transport.Visible Then cbo_Transport.Focus()

    End Sub

    Private Sub dgv_Req_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Req_Details.CellEnter
        With dgv_Req_Details

            dgv_ActCtrlName = .Name.ToString
        End With
    End Sub
    Private Sub dgv_Req_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Req_Details.EditingControlShowing
        dgtxt_RequirementDetails = CType(dgv_Req_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_RequirementDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_RequirementDetails.Enter
        dgv_ActCtrlName = dgv_Req_Details.Name
        dgv_Req_Details.EditingControl.BackColor = Color.Lime
        dgv_Req_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_RequirementDetails.SelectAll()
    End Sub

    Private Sub dgtxt_RequirementDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_RequirementDetails.KeyPress

        With dgv_Req_Details

            If Val(dgv_Req_Details.CurrentCell.ColumnIndex.ToString) = 5 Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If
            End If

        End With

    End Sub
    Private Sub cbo_BobinMillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinMillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_BobinMillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinMillName.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinMillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        With dgv_BobinDetails

            If (e.KeyValue = 38 And cbo_BobinMillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_BobinMillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_BobinMillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinMillName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinMillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_BobinDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value = Trim(cbo_BobinMillName.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With
        End If


    End Sub

    Private Sub cbo_BobinMillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinMillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinMillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_BobinMillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinMillName.TextChanged

        Try
            If cbo_BobinMillName.Visible Then
                With dgv_BobinDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_BobinMillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinMillName.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Function Negative_Stock() As Boolean
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim EndsCnt_IdNo As Integer = 0
        Dim MILL_IdNo As Integer = 0
        Dim CONT As String = ""
        Dim n As Integer
        Dim NewCode As String = 0

        EndsCnt_IdNo = Val(Common_Procedures.EndsCount_NameToIdNo(con, Trim(cbo_BobinEnds.Text)))
        'MILL_IdNo = Val(Common_Procedures.Mill_NameToIdNo(con, Trim(cbo_Grid_MillName.Text)))

        Negative_Stock = False

        Try
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
            cmd.ExecuteNonQuery()

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@invdate", dtp_Date.Value.Date)


            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1, Meters1, meters2 ,Name1) Select sum(a.Bobins), sum(a.Meter_Bobin), sum(a.Meters) , d.Mill_Name from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo   LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNo = d.Mill_IdNo  Where a.Company_IdNo = " & Val(lbl_Company.Tag) & "  and  a.Reference_Date <= @invdate and a.Meters <> 0 and a.EndsCount_IdNo = " & Val(EndsCnt_IdNo) & " and a.DeliveryTo_Idno <> 0  " & IIf(New_Entry = False, " and a.Reference_Code <> '" & Trim(NewCode) & "'", "") & "  group by d.Mill_Name "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1, meters1,meters2,Name1) Select -1*sum(a.Bobins), -1*sum(a.Meter_Bobin), -1*sum(a.Meters) , d.Mill_Name  from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNo <> 0 and a.Mill_IdNo = d.Mill_IdNo Where a.Company_IdNo = " & Val(lbl_Company.Tag) & "  and a.Reference_Date <= @invdate and a.Meters <> 0 and a.EndsCount_IdNo = " & Val(EndsCnt_IdNo) & "  and a.ReceivedFrom_Idno <> 0  " & IIf(New_Entry = False, " and a.Reference_Code <> '" & Trim(NewCode) & "'", "") & " Group by d.Mill_Name "
            cmd.ExecuteNonQuery()



            da = New SqlClient.SqlDataAdapter("select sum(Int1) as Total_Bobin ,Sum(Meters1) as Total_Meters_Bobin, Sum(Meters2)  as Total_Meters  from " & Trim(Common_Procedures.ReportTempSubTable) & "  having Sum(Meters2)  <> 0 ", con)
            da.Fill(dt)

            Negative_Stock = False

            If dt.Rows.Count > 0 Then

                For i = 0 To dt.Rows.Count - 1

                    If Val(dt.Rows(n).Item("Total_Bobin").ToString()) <= 0 Then

                        Negative_Stock = True

                    End If


                Next i

            End If


            dt.Clear()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Function

    Private Sub Bobin_Stock_Checking()


        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim cmd As New SqlClient.SqlCommand
        Dim EndsCnt_IdNo As Integer = 0
        Dim MILL_IdNo As Integer = 0
        Dim CONT As String = ""
        Dim n As Integer, Sno As Integer

        Dim NewCode As String = ""

        EndsCnt_IdNo = Val(Common_Procedures.EndsCount_NameToIdNo(con, Trim(cbo_BobinEnds.Text)))


        If EndsCnt_IdNo = 0 Then
            dgv_YarnStock.Rows.Clear()
            Exit Sub
        End If



        pnl_EndsCount_Stock.Visible = True

        'MILL_IdNo = Val(Common_Procedures.Mill_NameToIdNo(con, Trim(cbo_Grid_MillName.Text)))


        Try
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
            cmd.ExecuteNonQuery()

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@invdate", dtp_Date.Value.Date)


            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1, Meters1, meters2 ,Name1) Select sum(a.Bobins), sum(a.Meters_Bobin), sum(a.Meters) , d.Mill_Name from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo   LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNo = d.Mill_IdNo  Where a.Company_IdNo = " & Val(lbl_Company.Tag) & "  and  a.Reference_Date <= @invdate and a.Meters <> 0 and a.EndsCount_IdNo = " & Val(EndsCnt_IdNo) & " and a.DeliveryTo_Idno <> 0  " & IIf(New_Entry = False, " and a.Reference_Code <> '" & Trim(NewCode) & "'", "") & "  group by d.Mill_Name "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1, meters1,meters2,Name1) Select -1*sum(a.Bobins), -1*sum(a.Meters_Bobin), -1*sum(a.Meters) , d.Mill_Name  from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNo <> 0 and a.Mill_IdNo = d.Mill_IdNo Where a.Company_IdNo = " & Val(lbl_Company.Tag) & "  and a.Reference_Date <= @invdate and a.Meters <> 0 and a.EndsCount_IdNo = " & Val(EndsCnt_IdNo) & "  and a.ReceivedFrom_Idno <> 0  " & IIf(New_Entry = False, " and a.Reference_Code <> '" & Trim(NewCode) & "'", "") & " Group by d.Mill_Name "
            cmd.ExecuteNonQuery()



            da = New SqlClient.SqlDataAdapter("select sum(Int1) as Total_Bobin ,Sum(Meters1) as Total_MetersBobin, Sum(Meters2)  as Total_Meters , Name1 as Mill_Name from " & Trim(Common_Procedures.ReportTempSubTable) & " group by name1 having Sum(Meters2)  <> 0 ", con)
            da.Fill(dt)

            With dgv_YarnStock

                .Rows.Clear()
                Sno = 0

                If dt.Rows.Count > 0 Then

                    For i = 0 To dt.Rows.Count - 1

                        n = .Rows.Add()

                        Sno = Sno + 1
                        .Rows(n).Cells(0).Value = Val(Sno)
                        .Rows(n).Cells(1).Value = dt.Rows(n).Item("Mill_Name").ToString
                        .Rows(n).Cells(2).Value = dt.Rows(n).Item("Total_Bobin").ToString
                        .Rows(n).Cells(3).Value = dt.Rows(n).Item("Total_MetersBobin").ToString
                        .Rows(n).Cells(4).Value = dt.Rows(n).Item("Total_Meters").ToString

                    Next i

                End If

            End With
            dt.Clear()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub
    Private Sub Label12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label12.Click
        pnl_EndsCount_Stock.Visible = False

    End Sub

    Private Sub set_PaperSize_For_PrintDocument1()
        Dim I As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize


        If Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Then
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_A4_As_Default_PaperSize) = 1 Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        Else

            PpSzSTS = False

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

        End If

    End Sub


End Class