Public Class Sizing_Specification
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private MovSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "SZSPC-"
    Private Pk_Condition4 As String = "SIZSP-"
    Private Pk_Condition2 As String = "SZTDS-"
    Private Pk_Condition3 As String = "SZPRC-"
    Private PkCondition_GST As String = "GSSPC-"
    Private Other_Condition As String = ""
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_PavuDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String
    Private LastNo As String = ""
    Private SaveAll_STS As Boolean = False

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        MovSTS = False

        chk_Verified_Status.Checked = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_BeamDetails.Visible = False

        lbl_NewSTS.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        lbl_BabyWgt.Text = ""

        msk_Date.Text = ""
        dtp_Date.Text = ""
        txt_SetNo.Text = ""
        cbo_Sizing.Text = ""
        cbo_Type.Text = "DIRECT"
        cbo_Sizing.Tag = ""
        cbo_EndsCount.Text = ""
        txt_PcsLength.Text = ""
        txt_ExcessShort.Text = ""
        txt_BabyBag.Text = ""
        txt_BabyWt.Text = ""
        Chk_RWSts.Checked = True
        txt_RwCns.Text = ""
        txt_RWES.Text = ""
        txt_InvNo.Text = ""
        lbl_InvoiceAmt.Text = ""

        msk_InvoiceDate.Text = ""
        dtp_InvoiceDate.Text = ""
        msk_InvoiceDate.Text = ""
        cbo_YarnStock.Text = "CONSUMED YARN"
        txt_ConsumedYarn.Enabled = False

        lbl_Avg_Count.Text = ""
        lbl_Elogation.Text = ""
        txt_PickUp.Text = ""
        txt_TapeLength.Text = ""
        txt_TdsPerc.Text = ""
        txt_RwBags.Text = ""

        lbl_TdsAmount.Text = ""
        lbl_NetAmount.Text = ""

        txt_WarpMtr.Text = ""
        txt_WindingRate.Text = ""
        txt_PackingRate.Text = ""
        txt_ConsumedYarn.Text = ""
        txt_AddLess.Text = ""
        txt_YarnTaken.Text = ""
        txt_InvRate.Text = ""
        txt_TotalBeams.Text = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then '---- Ganesh karthick Textiles (p) Ltd (Somanur)
            cbo_BeamCount_Type.Text = "YARDS"
        Else
            cbo_BeamCount_Type.Text = "METERS"
        End If

       
        lbl_WindingAmt.Text = ""
        lbl_PackingAmt.Text = ""
        lbl_Elogation.Text = ""
        lbl_InvoiceAmt.Text = ""
        lbl_GrossAmt.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_PavuDetails.Rows.Clear()
        dgv_PavuDetails.Rows.Add()

        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        dgv_YarnDetails.Rows.Clear()
        dgv_YarnDetails.Rows(0).Cells(2).Value = "MILL"

        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

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

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
        cbo_Grid_BeamWidth.Text = ""

        cbo_Grid_BeamWidth.Visible = False
        cbo_Grid_BeamWidth.Tag = -1
        cbo_Grid_CountName.Tag = -1
        cbo_Grid_MillName.Tag = -1
        cbo_Grid_YarnType.Tag = -1

        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""
        cbo_Grid_YarnType.Text = ""

        cbo_Type.Enabled = True
        cbo_Type.BackColor = Color.White

        cbo_Sizing.Enabled = True
        cbo_Sizing.BackColor = Color.White

        btn_Selection.Enabled = True

        txt_SetNo.Enabled = True
        txt_SetNo.BackColor = Color.White

        cbo_EndsCount.Enabled = True
        cbo_EndsCount.BackColor = Color.White

        txt_BabyBag.Enabled = True
        txt_BabyBag.BackColor = Color.White

        txt_BabyWt.Enabled = True
        txt_BabyWt.BackColor = Color.White

        Chk_RWSts.Enabled = True
        Chk_RWSts.BackColor = Color.White

        txt_RwBags.Enabled = True
        txt_RwBags.BackColor = Color.White

        txt_RwCns.Enabled = True
        txt_RwCns.BackColor = Color.White

        txt_RWES.Enabled = True
        txt_RWES.BackColor = Color.White

        Grid_Cell_DeSelect()

        dgv_ActiveCtrl_Name = ""

        dgv_BeamDetails.Rows.Clear()

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
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
            msktxbx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_MillName.Name Then
            cbo_Grid_MillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_YarnType.Name Then
            cbo_Grid_YarnType.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_BeamWidth.Name Then
            cbo_Grid_BeamWidth.Visible = False

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
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        Dim babyLockSTS As Boolean = False
        Dim I As Integer = 0, J As Integer = 0

        If Val(no) = 0 Then Exit Sub

        clear()

        MovSTS = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as SizingName, d.EndsCount_Name from Sizing_Specification_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo Where a.Sizing_Specification_Code = '" & Trim(NewCode) & "' and " & Other_Condition, con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("Sizing_Specification_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sizing_Specification_Date").ToString
                msk_Date.Text = dtp_Date.Text

                cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString
                cbo_Sizing.Text = dt1.Rows(0).Item("SizingName").ToString
                txt_SetNo.Text = dt1.Rows(0).Item("Set_No").ToString
                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                txt_PcsLength.Text = dt1.Rows(0).Item("Pcs_Length").ToString

                txt_BabyBag.Text = dt1.Rows(0).Item("BabyCone_bags").ToString
                txt_BabyWt.Text = dt1.Rows(0).Item("BabyCone_Weight").ToString
                If Val(dt1.Rows(0).Item("Rewinding_Status").ToString) = 0 Then
                    Chk_RWSts.Checked = False
                End If
                txt_RwCns.Text = dt1.Rows(0).Item("Rewinding_Cones").ToString
                txt_RwBags.Text = Val(dt1.Rows(0).Item("Rewinding_Bags").ToString)
                txt_RWES.Text = dt1.Rows(0).Item("Rewinding_Excess").ToString

                cbo_YarnStock.Text = dt1.Rows(0).Item("YarnStock_Basis").ToString

                cbo_BeamCount_Type.Text = dt1.Rows(0).Item("BeamCount_Type").ToString
                txt_TapeLength.Text = Val(dt1.Rows(0).Item("Tape_Length").ToString)
                txt_PickUp.Text = Val(dt1.Rows(0).Item("PickUp_Perc").ToString)
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True


                da3 = New SqlClient.SqlDataAdapter("select a.* from Stock_BabyCone_Processing_Details a Where a. Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a. Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt3 = New DataTable
                da3.Fill(dt3)

                If dt3.Rows.Count > 0 Then
                    If (Chk_RWSts.Checked) = False And txt_BabyBag.Text <> 0 Then
                        If Val(dt3.Rows(0).Item("Delivered_Weight").ToString) <> 0 Then
                            babyLockSTS = True
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                                lbl_BabyWgt.Text = Val(dt3.Rows(0).Item("Delivered_Weight").ToString)
                            End If
                        End If
                    End If
                End If

                dt3.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Pavu_Delivery_Code, b.Pavu_Delivery_Increment, b.Beam_Knotting_Code, b.Loom_Idno, b.Production_Meters, b.Close_Status from Sizing_SpecificationPavu_Details a, Stock_SizedPavu_Processing_Details b where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_Specification_Code = '" & Trim(NewCode) & "' and a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                'da2 = New SqlClient.SqlDataAdapter("select * from Stock_SizedPavu_Processing_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by Sl_No", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                dgv_PavuDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        n = dgv_PavuDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_PavuDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_PavuDetails.Rows(n).Cells(1).Value = dt2.Rows(I).Item("Beam_No").ToString
                        If Val(dt2.Rows(I).Item("Noof_Pcs").ToString) <> 0 Then
                            dgv_PavuDetails.Rows(n).Cells(2).Value = Val(dt2.Rows(I).Item("Noof_Pcs").ToString)
                        End If
                        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                            dgv_PavuDetails.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(I).Item("Meters").ToString), "########0.000")

                        Else
                            dgv_PavuDetails.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(I).Item("Meters").ToString), "########0.00")

                        End If
                       
                        dgv_PavuDetails.Rows(n).Cells(4).Value = Common_Procedures.BeamWidth_IdNoToName(con, Val(dt2.Rows(I).Item("Beam_Width_IdNo").ToString))
                        dgv_PavuDetails.Rows(n).Cells(5).Value = ""
                        dgv_PavuDetails.Rows(n).Cells(6).Value = dt2.Rows(I).Item("Sizing_Pavu_Receipt_Code").ToString

                        If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Then
                            If Trim(dt2.Rows(I).Item("Pavu_Delivery_Code").ToString) <> "" Or Val(dt2.Rows(I).Item("Pavu_Delivery_Increment").ToString) <> 0 Or Trim(dt2.Rows(I).Item("Beam_Knotting_Code").ToString) <> "" Or Val(dt2.Rows(I).Item("Production_Meters").ToString) <> 0 Or Val(dt2.Rows(I).Item("Close_Status").ToString) <> 0 Then
                                dgv_PavuDetails.Rows(n).Cells(5).Value = "1"
                                For J = 0 To dgv_PavuDetails.ColumnCount - 1
                                    dgv_PavuDetails.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                                    dgv_PavuDetails.Rows(n).Cells(J).Style.ForeColor = Color.Red
                                Next
                                LockSTS = True
                            End If
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

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Sizing_SpecificationYarn_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Sizing_Specification_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_YarnDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        n = dgv_YarnDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_YarnDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_YarnDetails.Rows(n).Cells(1).Value = dt2.Rows(I).Item("Count_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(2).Value = dt2.Rows(I).Item("Yarn_Type").ToString
                        dgv_YarnDetails.Rows(n).Cells(3).Value = dt2.Rows(I).Item("Mill_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(I).Item("Bags").ToString)
                        dgv_YarnDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(I).Item("Cones").ToString)
                        dgv_YarnDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(I).Item("Weight").ToString), "########0.000")
                        ' dgv_YarnDetails.Rows(n).Cells(7).Value = dt2.Rows(I).Item("Rewinding_Delivery_Code").ToString
                        'If (Chk_RWSts.Checked) = False And txt_BabyBag.Text <> 0 Then
                        '    If Trim(UCase(dgv_YarnDetails.Rows(I).Cells(2).Value)) = "BABY" Then
                        '        For J = 0 To dgv_YarnDetails.ColumnCount - 1
                        '            dgv_YarnDetails.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                        '        Next J
                        '        LockSTS = True
                        '    End If
                        'End If
                    Next I

                End If

                dt2.Clear()

                TotalYarnTaken_Calculation()

                'With dgv_YarnDetails_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                '    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                '    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                'End With

                lbl_Avg_Count.Text = Format(Val(dt1.Rows(0).Item("Average_Count").ToString), "#########0.00")
                lbl_Elogation.Text = Format(Val(dt1.Rows(0).Item("Elongation").ToString), "#########0.00")

                txt_ConsumedYarn.Text = Format(Val(dt1.Rows(0).Item("Consumed_Yarn").ToString), "#########0.000")
                txt_YarnTaken.Text = Format(Val(dt1.Rows(0).Item("Yarn_Taken").ToString), "#########0.000")

                txt_InvNo.Text = dt1.Rows(0).Item("Sizing_Invoice_No").ToString
                dtp_InvoiceDate.Text = dt1.Rows(0).Item("Invoice_Date").ToString
                msk_InvoiceDate.Text = dtp_InvoiceDate.Text
                msk_InvoiceDate.Text = dt1.Rows(0).Item("Invoice_Date").ToString

                txt_InvRate.Text = Format(Val(dt1.Rows(0).Item("Invoice_Rate").ToString), "#########0.00")
                txt_PackingRate.Text = Format(Val(dt1.Rows(0).Item("Packing_Rate").ToString), "#########0.00")
                txt_WarpMtr.Text = Format(Val(dt1.Rows(0).Item("Warp_Meters").ToString), "#########0.00")
                txt_WindingRate.Text = Format(Val(dt1.Rows(0).Item("Winding_Rate").ToString), "#########0.00")

                txt_TotalBeams.Text = Val(dt1.Rows(0).Item("Total_PlanBeams").ToString)

                lbl_GrossAmt.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "#########0.00")
                lbl_WindingAmt.Text = Format(Val(dt1.Rows(0).Item("Winding_Amount").ToString), "#########0.00")
                lbl_PackingAmt.Text = Format(Val(dt1.Rows(0).Item("Packing_Amount").ToString), "#########0.00")


                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("Add_Less").ToString), "#########0.00")

                lbl_InvoiceAmt.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "#########0.00")
                txt_TdsPerc.Text = Val(dt1.Rows(0).Item("Tds_Percentage").ToString)
                lbl_TdsAmount.Text = Format(Val(dt1.Rows(0).Item("Tds_Amount").ToString), "#########0.00")
                lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "#########0.00")

                txt_ExcessShort.Text = Format(Val(dt1.Rows(0).Item("Excess_Short").ToString), "#########0.000")

            Else
                new_record()

            End If

            dt1.Clear()

            If LockSTS = True Then
                cbo_Type.Enabled = False
                cbo_Type.BackColor = Color.LightGray

                cbo_Sizing.Enabled = False
                cbo_Sizing.BackColor = Color.LightGray

                btn_Selection.Enabled = False

                txt_SetNo.Enabled = False
                txt_SetNo.BackColor = Color.LightGray

                cbo_EndsCount.Enabled = False
                cbo_EndsCount.BackColor = Color.LightGray
            End If

            If babyLockSTS = True Then

                cbo_Sizing.Enabled = False
                cbo_Sizing.BackColor = Color.LightGray

                txt_BabyBag.Enabled = False
                txt_BabyBag.BackColor = Color.LightGray

                txt_SetNo.Enabled = False
                txt_SetNo.BackColor = Color.LightGray

                txt_BabyWt.Enabled = False
                txt_BabyWt.BackColor = Color.LightGray

                Chk_RWSts.Enabled = False
                Chk_RWSts.BackColor = Color.LightGray

                txt_RwBags.Enabled = False
                txt_RwBags.BackColor = Color.LightGray

                txt_RwCns.Enabled = False
                txt_RwCns.BackColor = Color.LightGray

                txt_RWES.Enabled = False
                txt_RWES.BackColor = Color.LightGray


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
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub Sizing_Specification_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Sizing.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Sizing.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_BeamWidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_BeamWidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Sizing_Specification_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt1 As New DataTable
        'Dim dt2 As New DataTable
        'Dim dt3 As New DataTable
        'Dim dt4 As New DataTable
        'Dim dt5 As New DataTable
        'Dim dt6 As New DataTable

        Me.Text = ""


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Textiles (Somanur)
            Pk_Condition = Pk_Condition4
        End If

        lbl_AvgCount_Caption.Text = "Avg.Count"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
            lbl_AvgCount_Caption.Text = "Party Dc.No"
            lbl_OrderedBeams.Visible = True
            txt_TotalBeams.Visible = True
            btn_BeamDetail.Visible = True
        End If
        chk_Verified_Status.Visible = False
        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then chk_Verified_Status.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Then
            btn_SaveAll.Visible = True
        End If

        con.Open()

        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'SIZING' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        'da.Fill(dt1)
        'cbo_Sizing.DataSource = dt1
        'cbo_Sizing.DisplayMember = "Ledger_DisplayName"

        'da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
        'da.Fill(dt3)
        'cbo_EndsCount.DataSource = dt3
        'cbo_EndsCount.DisplayMember = "EndsCount_Name"

        'da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", con)
        'da.Fill(dt4)
        'cbo_Grid_MillName.DataSource = dt4
        'cbo_Grid_MillName.DisplayMember = "mill_name"

        'da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
        'da.Fill(dt5)
        'cbo_Grid_CountName.DataSource = dt5
        'cbo_Grid_CountName.DisplayMember = "count_name"

        'da = New SqlClient.SqlDataAdapter("select yarn_type from YarnType_Head order by yarn_type", con)
        'da.Fill(dt6)
        'cbo_Grid_YarnType.DataSource = dt6
        'cbo_Grid_YarnType.DisplayMember = "yarn_type"

        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
            dgv_PavuDetails.Columns(3).HeaderText = "MTR Or WGT"
        End If

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False

        msk_Date.Text = ""
        dtp_Date.Text = ""
        txt_SetNo.Text = ""
        cbo_Sizing.Text = ""
        cbo_Sizing.Tag = ""
        cbo_EndsCount.Text = ""

        cbo_EndsCount.Text = ""

        cbo_BeamCount_Type.Items.Add("")
        cbo_BeamCount_Type.Items.Add("METERS")
        cbo_BeamCount_Type.Items.Add("YARDS")

        cbo_YarnStock.Items.Add("")
        cbo_YarnStock.Items.Add("CONSUMED YARN")
        cbo_YarnStock.Items.Add("YARN TAKEN")
        txt_ConsumedYarn.Enabled = False

        cbo_Type.Items.Add("")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("RECEIPT")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_BeamDetails.Visible = False
        pnl_BeamDetails.Left = (Me.Width - pnl_BeamDetails.Width) \ 2
        pnl_BeamDetails.Top = (Me.Height - pnl_BeamDetails.Height) \ 2
        pnl_BeamDetails.BringToFront()

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_BeamWidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_YarnStock.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_TapeLength.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TdsPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PickUp.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BabyBag.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_BabyWt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ExcessShort.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_InvNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsLength.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RwCns.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RWES.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler Chk_RWSts.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RwBags.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_WarpMtr.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_YarnTaken.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WindingRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PackingRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_InvRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ConsumedYarn.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BeamCount_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalBeams.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_InvoiceDate.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_BeamWidth.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_InvoiceAmt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BabyBag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BabyWt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ExcessShort.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_YarnStock.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_TapeLength.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TdsPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PickUp.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Filter_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsLength.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RwCns.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RWES.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SetNo.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler Chk_RWSts.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RwBags.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_WarpMtr.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_YarnTaken.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WindingRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PackingRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ConsumedYarn.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BeamCount_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalBeams.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_InvoiceDate.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TotalBeams.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BabyWt.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_TdsPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BabyBag.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_InvNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PcsLength.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PickUp.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RWES.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SetNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_InvoiceDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Chk_RWSts.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RwBags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ExcessShort.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WarpMtr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_YarnTaken.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WindingRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PackingRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_InvRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ConsumedYarn.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalBeams.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BabyWt.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler msk_InvoiceDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PickUp.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BabyBag.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_TapeLength.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PcsLength.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RwCns.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RWES.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SetNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Chk_RWSts.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RwBags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ExcessShort.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WarpMtr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_YarnTaken.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WindingRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PackingRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_InvRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ConsumedYarn.KeyPress, AddressOf TextBoxControlKeyPress


        Other_Condition = "(Sizing_Specification_Code NOT LIKE '" & Trim(PkCondition_GST) & "%')"

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Sizing_Specification_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Sizing_Specification_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then


                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_BeamDetails.Visible = True Then
                    btn_BeamClose_Click(sender, e)
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
        Dim i As Integer

        If ActiveControl.Name = dgv_YarnDetails.Name Or ActiveControl.Name = dgv_PavuDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then
            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_YarnDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails

            ElseIf ActiveControl.Name = dgv_PavuDetails.Name Then
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

                If dgv1.Name = dgv_YarnDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_TapeLength.Focus()

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
                                If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                                    cbo_YarnStock.Focus()

                                Else
                                    If dgv_PavuDetails.Rows.Count = 0 Then dgv_PavuDetails.Rows.Add()
                                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                                    dgv_PavuDetails.Focus()

                                End If

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


                ElseIf dgv1.Name = dgv_PavuDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                                    dgv_YarnDetails.Focus()
                                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                                Else
                                    .Rows.Add()
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_PavuDetails.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                dgv_YarnDetails.Focus()
                                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_YarnStock.Focus()

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

    Private Sub get_MillCount_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As Single
        Dim Wgt_Bag As Single
        Dim Wgt_Cn As Single
        Dim CntID As Integer
        Dim MilID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(3).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_YarnDetails

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

                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    If .CurrentCell.ColumnIndex = 4 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = .Rows(.CurrentRow.Index).Cells(4).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(4).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 5 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(.Rows(.CurrentRow.Index).Cells(5).Value * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim EntID As String = ""
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YrnPartcls As String = ""
        Dim pCnt_ID As Integer = 0
        Dim pEdsCnt_Nm As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Sizing_Specification_Entry, New_Entry, Me, con, "Sizing_Specification_Head", "Sizing_Specification_Code", NewCode, "Sizing_Specification_Date", "(Sizing_Specification_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "Sizing_Pavu_Receipt_Head", "Verified_Status", "(Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If



        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details Where SoftwareType_IdNo <> " & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  ( Pavu_Delivery_Code <> '' or Pavu_Delivery_Increment <> 0 or Beam_Knotting_Code <> '' or Loom_Idno <> 0 or Production_Meters <> 0 or Close_Status <> 0)", con)
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

        Da = New SqlClient.SqlDataAdapter("select sum(Delivered_Weight) from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Some Baby Cones Delivered for this order", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            Da = New SqlClient.SqlDataAdapter("select * from Sizing_Pavu_Receipt_Head Where Sizing_Specification_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1
                    If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then

                        EntID = Trim(Pk_Condition3) & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_No").ToString)
                        Partcls = "Pavu Rcpt : Set No. " & Trim(Dt1.Rows(i).Item("Set_No").ToString)
                        PBlNo = Trim(Dt1.Rows(i).Item("Set_No").ToString)

                        pEdsCnt_Nm = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_Name", "(EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("EndsCount_IdNo").ToString)) & ")", , trans)
                        pCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("EndsCount_IdNo").ToString)) & ")", , trans))

                        YrnPartcls = Partcls & ", EndsCount : " & Trim(pEdsCnt_Nm) & ", Beams : " & Trim(Val(Dt1.Rows(i).Item("Total_Beam").ToString)) & ", Meters : " & Trim(Val(Dt1.Rows(i).Item("Total_Meters").ToString))

                        cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition3) & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Code").ToString) & "'"
                        cmd.ExecuteNonQuery()

                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@PavuReceiptDate", Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Date"))

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ) Values ( '" & Trim(Pk_Condition3) & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Code").ToString) & "', " & Str(Val(Dt1.Rows(i).Item("Company_IdNo").ToString)) & ", '" & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_No").ToString) & "', " & Str(Val(Dt1.Rows(i).Item("for_OrderBy").ToString)) & ", @PavuReceiptDate, 0, " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "', 1, " & Str(Val(pCnt_ID)) & ", 'MILL', 0, 0, 0, 0 )"
                        cmd.ExecuteNonQuery()

                    End If

                Next
            End If
            Dt1.Clear()

            cmd.CommandText = "Update Sizing_Pavu_Receipt_Head set Sizing_Specification_Code = '' Where Sizing_Specification_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Loom_Idno = 0 and Production_Meters = 0 and Close_Status = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sizing_SpecificationPavu_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sizing_SpecificationYarn_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(NewCode) & "'"
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

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()
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

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
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

            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_Specification_No from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby, Sizing_Specification_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_Specification_No from Sizing_Specification_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and " & Other_Condition & " Order by for_Orderby, Sizing_Specification_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_Specification_No from Sizing_Specification_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and " & Other_Condition & " Order by for_Orderby desc, Sizing_Specification_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_Specification_No from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and " & Other_Condition & "  Order by for_Orderby desc, Sizing_Specification_No desc", con)
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
        Dim Dt1 As New DataTable

        Try

            clear()

            New_Entry = True
            lbl_NewSTS.Visible = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Sizing_Specification_Head", "Sizing_Specification_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Sizing_Specification_No desc", con)
            Dt1 = New DataTable
            da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("Sizing_Specification_Date").ToString <> "" Then msk_Date.Text = Dt1.Rows(0).Item("Sizing_Specification_Date").ToString
                End If
                If Dt1.Rows(0).Item("Selection_Type").ToString <> "" Then cbo_Type.Text = Dt1.Rows(0).Item("Selection_Type").ToString
                If Dt1.Rows(0).Item("YarnStock_Basis").ToString <> "" Then cbo_YarnStock.Text = Dt1.Rows(0).Item("YarnStock_Basis").ToString
                If Dt1.Rows(0).Item("Tds_Percentage").ToString <> "" Then txt_TdsPerc.Text = Val(Dt1.Rows(0).Item("Tds_Percentage").ToString)
                If Dt1.Rows(0).Item("Rewinding_Status").ToString <> "" Then
                    If Val(Dt1.Rows(0).Item("Rewinding_Status").ToString) = 1 Then
                        Chk_RWSts.Checked = True
                    Else
                        Chk_RWSts.Checked = False
                    End If
                End If

            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            da.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If


        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Set.No.", "FOR FINDING...")

            'RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            'Da = New SqlClient.SqlDataAdapter("select Sizing_Specification_No from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(RecCode) & "'", con)
            'Da.Fill(Dt)

            RecCode = Trim(inpno)

            Da = New SqlClient.SqlDataAdapter("select Sizing_Specification_No from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Set_No = '" & Trim(RecCode) & "' and Sizing_Specification_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            Dt = New DataTable
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
                MessageBox.Show("Set.No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Sizing_Specification_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sizing_Specification_No from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(RecCode) & "'", con)
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
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim pEdsCnt_Nm As String = ""
        Dim Nr As Integer = 0
        Dim Sno As Integer = 0
        Dim Slno As Integer = 0
        Dim Partcls As String = "", YrnPartcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotPvuBms As Single, vTotPvuMtrs As Single
        Dim YCnt_ID As Integer = 0
        Dim vTotPvuStk As Single
        Dim YMil_ID As Integer = 0
        Dim ByCnCnt_ID As Integer = 0
        Dim ByCnMil_ID As Integer = 0
        Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single
        Dim EntID As String = ""
        Dim Dup_SetNoBmNo As String = ""
        Dim Mtr_Pc As Single = 0
        Dim pCnt_ID As Integer
        Dim pEds_Nm As String
        Dim vSetCd As String
        Dim Selc_SetCode As String
        Dim VouBil As String = ""
        Dim Del_ID As Integer, Rec_ID As Integer
        Dim Stock_In As String
        Dim mtrspcs As Single = 0
        Dim RWStatus As Integer = 0
        Dim YrnTyp As String = ""
        Dim YrnBgs As Single = 0
        Dim YrnCns As Single = 0
        Dim YrnWgt As Single = 0
        Dim StkAt_IdNo As Integer = 0
        Dim SizPvuRecCode As String = "", SizPvuRecNo As String = ""
        Dim Bw_ID As Integer = 0
        Dim vSetNo As String = ""
        Dim Usr_ID As Integer = 0
        Dim vVou_BlAmt As Double = 0
        Dim Verified_STS As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Sizing_Specification_Entry, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Sizing_Specification_Entry, New_Entry, Me, con, "Sizing_Specification_Head", "Sizing_Specification_Code", NewCode, "Sizing_Specification_Date", "(Sizing_Specification_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Sizing_Specification_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "Sizing_Specification_Head", "Verified_Status", "(Sizing_Specification_Code = '" & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
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
        lbl_UserName.Text = Common_Procedures.User.IdNo
        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        If EdsCnt_ID = 0 Then
            MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
            Exit Sub
        End If

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1


        If txt_TotalBeams.Visible = True And Val(txt_TotalBeams.Text) = 0 Then
            MessageBox.Show("Invalid Total Order Beams for this Set", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_TotalBeams.Enabled And txt_TotalBeams.Visible Then txt_TotalBeams.Focus()
            Exit Sub
        End If

        Usr_ID = Common_Procedures.User_NameToIdNo(con1, lbl_UserName.Text)

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

        vTotPvuBms = 0 : vTotPvuMtrs = 0
        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTotPvuBms = Val(dgv_PavuDetails_Total.Rows(0).Cells(1).Value())
            vTotPvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(3).Value())
        End If

        For i = 0 To dgv_YarnDetails.RowCount - 1

            If Val(dgv_YarnDetails.Rows(i).Cells(6).Value) <> 0 Then

                YCnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(1).Value)
                If Val(YCnt_ID) = 0 Then
                    MessageBox.Show("Invalid CountName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                If Trim(dgv_YarnDetails.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(2)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                YMil_ID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(3).Value)
                If Val(YMil_ID) = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(3)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

            End If

        Next

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1019" Then '---- SUBHAM Textiles (Somanur)
            If Trim(txt_SetNo.Text) <> "" Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                Da = New SqlClient.SqlDataAdapter("select * from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Ledger_IdNo = " & Str(Val(Led_ID)) & " and Set_No = '" & Trim(txt_SetNo.Text) & "' and Sizing_Specification_Code <> '" & Trim(NewCode) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    MessageBox.Show("Duplicate Set No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If txt_SetNo.Enabled And txt_SetNo.Visible Then txt_SetNo.Focus()
                    Exit Sub
                End If
                Dt1.Clear()
            End If

            If Trim(txt_InvNo.Text) <> "" Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                Da = New SqlClient.SqlDataAdapter("select * from Sizing_Specification_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Ledger_IdNo = " & Str(Val(Led_ID)) & " and Sizing_Invoice_No = '" & Trim(txt_InvNo.Text) & "' and Sizing_Specification_Code <> '" & Trim(NewCode) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    MessageBox.Show("Duplicate Invoice No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If txt_InvNo.Enabled And txt_InvNo.Visible Then txt_InvNo.Focus()
                    Exit Sub
                End If
                Dt1.Clear()
            End If
        End If

        vTotYrnBags = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0
        If dgv_YarnDetails_Total.RowCount > 0 Then
            vTotYrnBags = Val(dgv_YarnDetails_Total.Rows(0).Cells(4).Value())
            vTotYrnCones = Val(dgv_YarnDetails_Total.Rows(0).Cells(5).Value())
            vTotYrnWeight = Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value())
        End If

        If Val(lbl_NetAmount.Text) = 0 Then lbl_NetAmount.Text = "0"

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Sizing_Specification_Head", "Sizing_Specification_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            RWStatus = 0
            If Chk_RWSts.Checked = True Then RWStatus = 1

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))
            If IsDate(msk_InvoiceDate.Text) = True Then
                cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(msk_InvoiceDate.Text))
            End If

            If New_Entry = True Then

                cmd.CommandText = "Insert into Sizing_Specification_Head( Sizing_Specification_Code, Company_IdNo                     , Sizing_Specification_No       , for_OrderBy                                                            , Sizing_Specification_Date, Ledger_IdNo             , Set_No                        , EndsCount_IdNo             ,Pcs_Length                      ,Excess_Short                        , BabyCone_bags               , BabyCone_Weight              , Rewinding_Status    , Rewinding_Bags              ,Rewinding_Cones            ,Rewinding_Excess           ,Sizing_Invoice_No              , Amount                          , Total_Beam                  , Total_Meters                 , Total_Bags                   , Total_Cones                   , Total_Weight                     , Average_Count                     , YarnStock_Basis                    , Tape_Length                           , PickUp_Perc                       , Elongation                          ,Tds_Percentage                      , Tds_Amount                           , Net_Amount                                 , Invoice_Date                                                            , Selection_Type                , Add_Less                           ,Consumed_Yarn                            ,Invoice_Rate                       , Packing_Rate                           ,Warp_Meters                         , Winding_Rate                            , Yarn_Taken                           , BeamCount_Type                          , Winding_Amount                       , Packing_Amount                        , Gross_Amount                       , Total_PlanBeams                     ,  User_idNo         ,Verified_Status            ) " & _
                "Values                                                 ('" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate               , " & Str(Val(Led_ID)) & ", '" & Trim(txt_SetNo.Text) & "', " & Str(Val(EdsCnt_ID)) & ", " & Val(txt_PcsLength.Text) & ", " & Val(txt_ExcessShort.Text) & " ," & Val(txt_BabyBag.Text) & " , " & Val(txt_BabyWt.Text) & " ," & Val(RWStatus) & "," & Val(txt_RwBags.Text) & " ," & Val(txt_RwCns.Text) & ", " & Val(txt_RWES.Text) & ", '" & Trim(txt_InvNo.Text) & "', " & Val(lbl_InvoiceAmt.Text) & ", " & Str(Val(vTotPvuBms)) & ", " & Str(Val(vTotPvuMtrs)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & " ,  '" & Trim(lbl_Avg_Count.Text) & "', '" & Trim(cbo_YarnStock.Text) & "' , " & Str(Val(txt_TapeLength.Text)) & " , " & Str(Val(txt_PickUp.Text)) & " ," & Str(Val(lbl_Elogation.Text)) & " , " & Str(Val(txt_TdsPerc.Text)) & " , " & Str(Val(lbl_TdsAmount.Text)) & " , " & Str(Val(CSng(lbl_NetAmount.Text))) & " , " & IIf(IsDate(msk_InvoiceDate.Text) = True, "@InvoiceDate", "Null") & ", '" & Trim(cbo_Type.Text) & "' , " & Str(Val(txt_AddLess.Text)) & " , " & Str(Val(txt_ConsumedYarn.Text)) & " ," & Str(Val(txt_InvRate.Text)) & " , " & Str(Val(txt_PackingRate.Text)) & " , " & Str(Val(txt_WarpMtr.Text)) & " ,  " & Str(Val(txt_WindingRate.Text)) & " , " & Str(Val(txt_YarnTaken.Text)) & " , '" & Trim(cbo_BeamCount_Type.Text) & "' ," & Str(Val(lbl_WindingAmt.Text)) & " , " & Str(Val(lbl_PackingAmt.Text)) & " ," & Str(Val(lbl_GrossAmt.Text)) & " ," & Str(Val(txt_TotalBeams.Text)) & ", " & Val(lbl_UserName.Text) & ", " & Val(Verified_STS) & " )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Sizing_Specification_Head set Sizing_Specification_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Set_No = '" & Trim(txt_SetNo.Text) & "',  EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Pcs_Length = '" & Trim(txt_PcsLength.Text) & "' ,Excess_Short = " & Val(txt_ExcessShort.Text) & " ,BabyCone_bags = " & Val(txt_BabyBag.Text) & " ,BabyCone_Weight = " & Val(txt_BabyWt.Text) & " ,Rewinding_Status = " & Val(RWStatus) & " ,Rewinding_Bags = " & Val(txt_RwBags.Text) & " , Rewinding_Cones = " & Val(txt_RwCns.Text) & ",Rewinding_Excess = " & Val(txt_RWES.Text) & ",Sizing_Invoice_No = '" & Trim(txt_InvNo.Text) & "', Amount = " & Val(lbl_InvoiceAmt.Text) & " , Total_Beam = " & Str(Val(vTotPvuBms)) & ", Total_Meters = " & Str(Val(vTotPvuMtrs)) & ", Total_Bags = " & Str(Val(vTotYrnBags)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & " , Average_Count = '" & Trim(lbl_Avg_Count.Text) & "' , YarnStock_Basis = '" & Trim(cbo_YarnStock.Text) & "' , Tape_Length = " & Str(Val(txt_TapeLength.Text)) & " , PickUp_Perc =  " & Str(Val(txt_PickUp.Text)) & "   , Elongation = " & Str(Val(lbl_Elogation.Text)) & " ,Tds_Percentage =  " & Str(Val(txt_TdsPerc.Text)) & "   , Tds_Amount =  " & Str(Val(lbl_TdsAmount.Text)) & ", Net_Amount =  " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Invoice_Date =  " & IIf(IsDate(msk_InvoiceDate.Text) = True, "@InvoiceDate", "Null") & ",  Selection_Type = '" & Trim(cbo_Type.Text) & "' , Add_Less = " & Str(Val(txt_AddLess.Text)) & " ,Consumed_Yarn = " & Str(Val(txt_ConsumedYarn.Text)) & " ,Invoice_Rate = " & Str(Val(txt_InvRate.Text)) & " , Packing_Rate = " & Str(Val(txt_PackingRate.Text)) & " , Warp_Meters = " & Str(Val(txt_WarpMtr.Text)) & " , Winding_Rate = " & Str(Val(txt_WindingRate.Text)) & " , Yarn_Taken = " & Str(Val(txt_YarnTaken.Text)) & " , BeamCount_Type = '" & Trim(cbo_BeamCount_Type.Text) & "' , Winding_Amount = " & Str(Val(lbl_WindingAmt.Text)) & " , Packing_Amount = " & Str(Val(lbl_PackingAmt.Text)) & " , Gross_Amount =  " & Str(Val(lbl_GrossAmt.Text)) & "  , Total_PlanBeams =  " & Str(Val(txt_TotalBeams.Text)) & ",  User_idNo = " & Val(lbl_UserName.Text) & " ,Verified_Status= " & Val(Verified_STS) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


                Da = New SqlClient.SqlDataAdapter("select * from Sizing_Pavu_Receipt_Head Where Sizing_Specification_Code = '" & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    For i = 0 To Dt1.Rows.Count - 1
                        If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then

                            EntID = Trim(Pk_Condition3) & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_No").ToString)
                            Partcls = "Pavu Rcpt : Set No. " & Trim(Dt1.Rows(i).Item("Set_No").ToString)
                            PBlNo = Trim(Dt1.Rows(i).Item("Set_No").ToString)

                            pEdsCnt_Nm = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_Name", "(EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("EndsCount_IdNo").ToString)) & ")", , tr)
                            pCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("EndsCount_IdNo").ToString)) & ")", , tr))

                            YrnPartcls = Partcls & ", EndsCount : " & Trim(pEdsCnt_Nm) & ", Beams : " & Trim(Val(Dt1.Rows(i).Item("Total_Beam").ToString)) & ", Meters : " & Trim(Val(Dt1.Rows(i).Item("Total_Meters").ToString))

                            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition3) & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Code").ToString) & "'"
                            cmd.ExecuteNonQuery()

                            cmd.Parameters.Clear()
                            cmd.Parameters.AddWithValue("@PavuReceiptDate", Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Date"))

                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ) Values ( '" & Trim(Pk_Condition3) & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Code").ToString) & "', " & Str(Val(Dt1.Rows(i).Item("Company_IdNo").ToString)) & ", '" & Trim(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_No").ToString) & "', " & Str(Val(Dt1.Rows(i).Item("for_OrderBy").ToString)) & ", @PavuReceiptDate, 0, " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "', 1, " & Str(Val(pCnt_ID)) & ", 'MILL', 0, 0, 0, 0 )"
                            cmd.ExecuteNonQuery()

                        End If

                    Next
                End If
                Dt1.Clear()

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))
                If IsDate(msk_InvoiceDate.Text) = True Then
                    cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(msk_InvoiceDate.Text))
                End If

                cmd.CommandText = "Update Sizing_Pavu_Receipt_Head set Sizing_Specification_Code = '' Where Sizing_Specification_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            Partcls = "Stmt : Set.No. " & Trim(txt_SetNo.Text)
            PBlNo = Trim(txt_SetNo.Text)

            cmd.CommandText = "Delete from Sizing_SpecificationYarn_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Loom_Idno = 0 and Production_Meters = 0 and Close_Status = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Sizing_SpecificationPavu_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Specification_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and Delivered_Weight = 0"
            cmd.ExecuteNonQuery()

            pCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ")", , tr))
            pEds_Nm = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Ends_Name", "(EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ")", , tr))

            StkAt_IdNo = Val(Led_ID)
            If Val(Common_Procedures.settings.SizingSpecification_AutoTransfer_PavuStock_To_Godown) = 1 Then
                StkAt_IdNo = Val(Common_Procedures.CommonLedger.Godown_Ac)
            End If

            With dgv_PavuDetails
                Sno = 0
                For i = 0 To dgv_PavuDetails.RowCount - 1

                    If Val(dgv_PavuDetails.Rows(i).Cells(3).Value) <> 0 Then

                        Sno = Sno + 1
                        Bw_ID = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)
                        Mtr_Pc = 0
                        If Val(.Rows(i).Cells(2).Value) <> 0 Then
                            Mtr_Pc = Format(Val(.Rows(i).Cells(3).Value) / Val(.Rows(i).Cells(2).Value), "#########0.00")
                        End If

                        SizPvuRecCode = ""
                        SizPvuRecNo = ""
                        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                            SizPvuRecCode = Trim(.Rows(i).Cells(6).Value)
                            SizPvuRecNo = Common_Procedures.get_FieldValue(con, "Sizing_Pavu_Receipt_Head", "Sizing_Pavu_Receipt_No", "(Sizing_Pavu_Receipt_Code = '" & Trim(.Rows(i).Cells(6).Value) & "')", , tr)

                            cmd.CommandText = "Update Sizing_Pavu_Receipt_Head set Sizing_Specification_Code = '" & Trim(NewCode) & "'  Where Sizing_Pavu_Receipt_Code = '" & Trim(.Rows(i).Cells(6).Value) & "'"
                            cmd.ExecuteNonQuery()

                            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition3) & Trim(.Rows(i).Cells(6).Value) & "'"
                            cmd.ExecuteNonQuery()

                        End If

                        cmd.CommandText = "Insert into Sizing_SpecificationPavu_Details (    Sizing_Specification_Code, Company_IdNo                     , Sizing_Specification_No         , for_OrderBy                                  ,                  Sizing_Specification_Date,     Ledger_IdNo     ,             Set_Code      ,           Set_No          ,             Sl_No              ,                    Beam_No             ,          Noof_Pcs                       ,          Meters_Pc              ,                      Meters                   ,   Beam_Width_IdNo    ,     Sizing_Pavu_Receipt_No,      Sizing_Pavu_Receipt_Code ) " & _
                                                         "    Values              ( '" & Trim(NewCode) & "'       , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(Led_ID)) & ", '" & Trim(vSetCd) & "', '" & Trim(txt_SetNo.Text) & "',          " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "',     " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(Mtr_Pc)) & "    , " & Str(Val(.Rows(i).Cells(3).Value)) & "     , " & Str(Val(Bw_ID)) & ",'" & Trim(SizPvuRecNo) & "', '" & Trim(SizPvuRecCode) & "' ) "
                        cmd.ExecuteNonQuery()

                        If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Then
                            Nr = 0
                            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Reference_Date = @EntryDate, Sl_No = " & Str(Val(Sno)) & " " & _
                                                " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "' and Beam_No = '" & Trim(.Rows(i).Cells(1).Value) & "'"
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                cmd.CommandText = "Insert into Stock_SizedPavu_Processing_Details (                     Reference_Code          ,              Company_IdNo        ,               Reference_No    ,                               for_OrderBy                              , Reference_Date,         Ledger_IdNo     ,           StockAt_IdNo      ,         Set_Code      ,           Set_No              ,    setcode_forSelection     ,      Ends_Name         ,     count_idno           ,         EndsCount_IdNo     , Mill_IdNo,  Beam_Width_IdNo, Sizing_SlNo,         Sl_No        ,                    Beam_No             ,                               ForOrderBy_BeamNo                                 , Gross_Weight, Tare_Weight, Net_Weight,                      Noof_Pcs            ,          Meters_Pc      ,                      Meters               ) " & _
                                                            "    Values                           ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(Led_ID)) & ", " & Str(Val(StkAt_IdNo)) & ", '" & Trim(vSetCd) & "', '" & Trim(txt_SetNo.Text) & "', '" & Trim(Selc_SetCode) & "', '" & Trim(pEds_Nm) & "', " & Str(Val(pCnt_ID)) & ", " & Str(Val(EdsCnt_ID)) & ",     0    ,  " & Str(Val(Bw_ID)) & " ,    0     , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(1).Value))) & ",      0      ,       0    ,      0    , " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(Mtr_Pc)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & " ) "
                                cmd.ExecuteNonQuery()
                            End If


                        End If

                    End If

                Next

            End With

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

                Del_ID = Val(Led_ID)
                Rec_ID = 0
                If Val(Common_Procedures.settings.SizingSpecification_AutoTransfer_PavuStock_To_Godown) = 1 Then
                    Del_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                    Rec_ID = 0

                Else
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
                        If (Trim(UCase(NewCode)) = "1-27/15-16" And Trim(UCase(txt_SetNo.Text)) = "1607" And Led_ID = 104) Or (Trim(UCase(NewCode)) = "1-5/15-16" And Trim(UCase(txt_SetNo.Text)) = "716" And Led_ID = 117) Then '---- M.K Textiles (Palladam)
                            Del_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                            Rec_ID = 0
                        End If
                    End If

                End If

                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(vTotPvuBms)) & ", " & Str(Val(vTotPvuStk)) & " )"
                cmd.ExecuteNonQuery()

            End If


            With dgv_YarnDetails

                YrnPartcls = Partcls & ", EndsCount : " & Trim(cbo_EndsCount.Text) & ", Beams : " & Trim(Val(vTotPvuBms)) & ", Meters : " & Trim(Val(vTotPvuMtrs))

                Sno = 0
                ByCnCnt_ID = 0
                ByCnMil_ID = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(6).Value) <> 0 Then

                        Sno = Sno + 1

                        YCnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        YMil_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        If ByCnCnt_ID = 0 Then
                            ByCnCnt_ID = YCnt_ID
                            ByCnMil_ID = YMil_ID
                        End If

                        cmd.CommandText = "Insert into Sizing_SpecificationYarn_Details(Sizing_Specification_Code, Company_IdNo, Sizing_Specification_No, for_OrderBy, Sizing_Specification_Date, Sl_No, count_idno, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate,  " & Str(Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " )"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

                If Val(vTotYrnWeight) = 0 Then
                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "', -51, " & Str(Val(pCnt_ID)) & ", 'MILL', 0, 0, 0, 0 )"
                    cmd.ExecuteNonQuery()
                End If

            End With

            If Val(txt_ExcessShort.Text) <> 0 Or Val(txt_BabyWt.Text) <> 0 Then

                If Trim(UCase(cbo_YarnStock.Text)) = "YARN TAKEN" Then

                    If Chk_RWSts.Checked = True Then
                        YrnTyp = "R/W"
                        YrnBgs = Val(txt_RwBags.Text)
                        YrnCns = Val(txt_RwCns.Text)
                        YrnWgt = Val(txt_BabyWt.Text) + Val(txt_RWES.Text)

                    Else
                        YrnTyp = "BABY"
                        YrnBgs = Val(txt_BabyBag.Text)
                        YrnCns = Val(vTotYrnCones)
                        YrnWgt = Val(txt_BabyWt.Text)

                    End If

                Else
                    YrnTyp = "MILL"
                    YrnBgs = 0
                    YrnCns = 0
                    YrnWgt = Val(txt_ExcessShort.Text)

                End If

                Del_ID = 0 : Rec_ID = 0

                If Val(YrnWgt) < 0 Then
                    Rec_ID = Led_ID
                Else
                    Del_ID = Led_ID
                End If

                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', -200, " & Str(Val(ByCnCnt_ID)) & ", '" & Trim(YrnTyp) & "', " & Str(Val(ByCnMil_ID)) & ", " & Str(Val(YrnBgs)) & ", " & Str(Val(YrnCns)) & ", " & Str(Math.Abs(Val(YrnWgt))) & " )"
                cmd.ExecuteNonQuery()

            End If

            Slno = Slno + 1
            If Val(vTotYrnBags) <> 0 Or Val(vTotYrnCones) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Empty_Bags, Empty_Cones, Particulars , Beam_Width_IdNo) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', " & Str(Val(Slno)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", '" & Trim(Partcls) & "', " & Str(Val(Bw_ID)) & ")"
                cmd.ExecuteNonQuery()
            End If
            If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Then

                '    If Val(vTotPvuBms) <> 0 Then
                '        cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Pavu_Beam, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Led_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', 2," & Str(Val(Bw_ID)) & ", " & Str(Val(vTotPvuBms)) & ", '" & Trim(Partcls) & "')"
                '        cmd.ExecuteNonQuery()

                '        cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', 3, " & Str(Val(Bw_ID)) & ", " & Str(Val(vTotPvuBms)) & ", '" & Trim(Partcls) & "')"
                '        cmd.ExecuteNonQuery()
                '    End If
                'cmd.Connection = con

                da1 = New SqlClient.SqlDataAdapter("select a.Beam_Width_IdNo , Count(Beam_No) as Beams from Sizing_SpecificationPavu_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_Specification_Code = '" & Trim(NewCode) & "' group by a.Beam_Width_IdNo", con)
                da1.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    For I = 0 To Dt1.Rows.Count - 1
                        Slno = Slno + 1
                        cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Pavu_Beam, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Led_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', " & Str(Val(Slno)) & " ," & Str(Val(Dt1.Rows(I).Item("Beam_Width_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(I).Item("Beams").ToString)) & " , '" & Trim(Partcls) & "')"
                        cmd.ExecuteNonQuery()
                        Slno = Slno + 1
                        cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', " & Str(Val(Slno)) & " , " & Str(Val(Dt1.Rows(I).Item("Beam_Width_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(I).Item("Beams").ToString)) & " , '" & Trim(Partcls) & "')"
                        cmd.ExecuteNonQuery()
                    Next
                End If
            End If

            If Val(Chk_RWSts.Checked) = 0 Then

                Nr = 0
                cmd.CommandText = "Update Stock_BabyCone_Processing_Details set " & _
                             " DeliveryTo_Idno = " & Str(Val(Led_ID)) & ", " & _
                             " Mill_Idno = " & Str(Val(ByCnMil_ID)) & ", " & _
                            " Count_Idno = " & Str(Val(ByCnCnt_ID)) & ", " & _
                            " Baby_Bags = " & Str(Val(txt_BabyBag.Text)) & ", " & _
                            " Baby_Cones = " & Str(Val(vTotYrnCones)) & ", " & _
                            " Baby_Weight = " & Str(Val(txt_BabyWt.Text)) & " " & _
                            " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and " & _
                            " Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "'"
                Nr = cmd.ExecuteNonQuery()

                If Nr = 0 And Val(txt_BabyWt.Text) <> 0 Then

                    cmd.CommandText = "Insert into Stock_BabyCone_Processing_Details( Reference_Code, " _
                              & "Company_IdNo, Reference_No, For_OrderBy, Reference_Date,DeliveryTo_Idno, ReceivedFrom_Idno, " _
                              & "Set_Code, Set_No, setcode_forSelection, " _
                              & "Ends_Name, Yarn_Type, Mill_Idno, Count_IdNo, Bag_No, Baby_Bags, " _
                              & "Baby_Cones, Baby_Weight, Delivered_Bags, Delivered_Cones, Delivered_Weight) Values ( '" _
                              & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_RefNo.Text)) & "', " _
                              & Str(Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Led_ID) & ", 0,'" _
                              & Trim(vSetCd) & "', '" & Trim(txt_SetNo.Text) & "', '" & Trim(Selc_SetCode) & "', '','BABY', " & Str(ByCnMil_ID) & ", " & Str(ByCnCnt_ID) & ", 1, " _
                              & Str(Val(txt_BabyBag.Text)) & ", " & Str(Val(vTotYrnCones)) & ", " _
                              & Str(Val(txt_BabyWt.Text)) & ", 0, 0, 0)"

                    cmd.ExecuteNonQuery()

                End If

            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""


            If Val(lbl_GrossAmt.Text) <> 0 Then
                vLed_IdNos = Led_ID & "|" & Val(Common_Procedures.CommonLedger.Sizing_Charges_Ac)
                vVou_Amts = Val(lbl_GrossAmt.Text) & "|" & -1 * (Val(lbl_GrossAmt.Text))
                If Common_Procedures.Voucher_Updation(con, "Siz.Spec", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_Date.Text), "Bill No. : " & Trim(txt_InvNo.Text) & ",  Set No. : " & Trim(txt_SetNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                    Throw New ApplicationException(ErrMsg)
                    Exit Sub
                End If
            End If

            If Val(lbl_TdsAmount.Text) <> 0 Then
                vLed_IdNos = Led_ID & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
                vVou_Amts = -1 * Val(lbl_TdsAmount.Text) & "|" & Val(lbl_TdsAmount.Text)
                If Common_Procedures.Voucher_Updation(con, "Siz.Tds", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_Date.Text), "Bill No. : " & Trim(txt_InvNo.Text) & ",  Set No. : " & Trim(txt_SetNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                    Throw New ApplicationException(ErrMsg)
                    Exit Sub
                End If
            End If

            '---Bill Posting

            vVou_BlAmt = Val(CSng(lbl_NetAmount.Text))

            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), msk_Date.Text, Led_ID, Trim(lbl_RefNo.Text), 0, Val(vVou_BlAmt), "CR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            ' MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If
            Else
                move_record(lbl_RefNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("IX_Stock_SizedPavu_Processing_Details_2"))) > 0 Then
                MessageBox.Show("Duplicate SetNo/BeamNo", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("IX_Stock_SizedPavu_Processing_Details_1"))) > 0 Then
                MessageBox.Show("Duplicate SetNo/BeamNo", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("PK_SizedPavu_Processing_Details"))) > 0 Then
                MessageBox.Show("Duplicate SetNo/BeamNo", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus() : msk_Date.SelectionStart = 0

        End Try

    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, dtp_Date, cbo_Sizing, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Sizing, "", "", " ", "", True)
    End Sub

    Private Sub cbo_Sizing_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Sizing_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing, cbo_Type, txt_SetNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Sizing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_Type.Text) = "RECEIPT" Then

                If MessageBox.Show("Do you want to select order:", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    If txt_SetNo.Enabled Then
                        txt_SetNo.Focus()
                    Else
                        txt_TotalBeams.Focus()
                    End If

                End If


            Else
                If txt_SetNo.Enabled Then
                    txt_SetNo.Focus()
                Else
                    txt_TotalBeams.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
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

    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, txt_SetNo, txt_PcsLength, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
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
                Condt = "a.Sizing_Specification_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Sizing_Specification_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sizing_Specification_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sizing_Specification_Code IN (select z1.Sizing_Specification_Code from Sizing_SpecificationYarn_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sizing_Specification_Code IN (select z1.Sizing_Specification_Code from Sizing_SpecificationYarn_Details z1 where z1.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ")"
            End If

            If Val(EdsCnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo))
            End If

            If Trim(txt_Filter_SetNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Set_No = '" & Trim(txt_Filter_SetNo.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as SizingName , c.*  from Sizing_Specification_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head c ON c.EndsCount_IdNo = a.EndsCount_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_Specification_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sizing_Specification_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Sizing_Specification_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Set_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sizing_Specification_Date").ToString), "dd-MM-yyyy")
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


    Private Sub Open_BeamReceiptEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_BeamDetails.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_BeamDetails.Visible = False
        End If

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

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

    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        dgv_PavuDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle
        With dgv_PavuDetails
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
            If e.ColumnIndex = 4 Then

                If cbo_Grid_BeamWidth.Visible = False Or Val(cbo_Grid_BeamWidth.Tag) <> e.RowIndex Then

                    cbo_Grid_BeamWidth.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from Beam_Width_Head order by Beam_Width_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_BeamWidth.DataSource = Dt1
                    cbo_Grid_BeamWidth.DisplayMember = "Beam_Width_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_BeamWidth.Left = .Left + Rect.Left
                    cbo_Grid_BeamWidth.Top = .Top + Rect.Top

                    cbo_Grid_BeamWidth.Width = Rect.Width
                    cbo_Grid_BeamWidth.Height = Rect.Height
                    cbo_Grid_BeamWidth.Text = .CurrentCell.Value

                    cbo_Grid_BeamWidth.Tag = Val(e.RowIndex)
                    cbo_Grid_BeamWidth.Visible = True

                    cbo_Grid_BeamWidth.BringToFront()
                    cbo_Grid_BeamWidth.Focus()



                End If

            Else
                cbo_Grid_BeamWidth.Visible = False

            End If
            If e.RowIndex > 0 And e.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                    .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1
                    .CurrentRow.Cells(2).Value = .Rows(e.RowIndex - 1).Cells(2).Value
                    .CurrentRow.Cells(3).Value = .Rows(e.RowIndex - 1).Cells(3).Value
                    .CurrentRow.Cells(4).Value = .Rows(e.RowIndex - 1).Cells(4).Value
                    '.Rows.Add()
                End If
                If e.ColumnIndex = 1 And e.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(2).Value) = 0 And Val(.CurrentRow.Cells(3).Value) = 0 Then
                    .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1
                    .CurrentRow.Cells(2).Value = .Rows(e.RowIndex - 1).Cells(2).Value
                    .CurrentRow.Cells(3).Value = .Rows(e.RowIndex - 1).Cells(3).Value
                    .CurrentRow.Cells(4).Value = .Rows(e.RowIndex - 1).Cells(4).Value
                    '.Rows.Add()
                End If
            End If

        End With
    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave
        With dgv_PavuDetails
            If .CurrentCell.ColumnIndex = 3 Then
                If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")

                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        Try

            If FrmLdSTS = True Then Exit Sub
            If MovSTS = True Then Exit Sub

            With dgv_PavuDetails
                If .Visible Then
                    If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
                    If e.ColumnIndex = 2 Then
                        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                            .CurrentRow.Cells(3).Value = Format(Val(.CurrentRow.Cells(2).Value) * Val(txt_PcsLength.Text), "#########0.000")
                        Else
                            .CurrentRow.Cells(3).Value = Format(Val(.CurrentRow.Cells(2).Value) * Val(txt_PcsLength.Text), "#########0.00")

                        End If

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

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_PavuDetails = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_PavuDetails_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.GotFocus
        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
    End Sub

    Private Sub dgv_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

                If Val(.Rows(.CurrentRow.Index).Cells(5).Value) = 0 Then

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
        dgv_PavuDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer

        With dgv_PavuDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
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
            .Rows(0).Cells(2).Value = Format(Val(TotPcs), "########0.00")
            If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                .Rows(0).Cells(3).Value = Format(Val(TotMtrs), "########0.000")
            Else
                .Rows(0).Cells(3).Value = Format(Val(TotMtrs), "########0.00")
            End If

        End With
        NetAmount_Calculation()
    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEndEdit

        If Trim(UCase(dgv_YarnDetails.CurrentRow.Cells(2).Value)) = "MILL" Then
            If dgv_YarnDetails.CurrentCell.ColumnIndex = 4 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 5 Then
                get_MillCount_Details()
            End If
        End If

    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle
        Dim LckSTS As Boolean = False


        With dgv_YarnDetails

            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(.CurrentRow.Cells(2).Value) = "" Then
                .CurrentRow.Cells(2).Value = "MILL"
            End If

            If e.ColumnIndex = 1 And Val(lbl_BabyWgt.Text) = 0 Then

                If cbo_Grid_CountName.Visible = False Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                    cbo_Grid_CountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_CountName.DataSource = Dt1
                    cbo_Grid_CountName.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_CountName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_CountName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Grid_CountName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_CountName.Height = rect.Height  ' rect.Height
                    cbo_Grid_CountName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_CountName.Tag = Val(e.RowIndex)
                    cbo_Grid_CountName.Visible = True

                    cbo_Grid_CountName.BringToFront()
                    cbo_Grid_CountName.Focus()

                    'cbo_Grid_MillName.Visible = False
                    'cbo_Grid_YarnType.Visible = False

                End If


            Else

                cbo_Grid_CountName.Visible = False
                'cbo_Grid_CountName.Tag = -1
                'cbo_Grid_CountName.Text = ""

            End If



            If e.ColumnIndex = 2 And Val(lbl_BabyWgt.Text) = 0 Then

                If cbo_Grid_YarnType.Visible = False Or Val(cbo_Grid_YarnType.Tag) <> e.RowIndex Then

                    cbo_Grid_YarnType.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head order by Yarn_Type", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_YarnType.DataSource = Dt2
                    cbo_Grid_YarnType.DisplayMember = "Yarn_Type"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_YarnType.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_YarnType.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_YarnType.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_YarnType.Height = rect.Height  ' rect.Height

                    cbo_Grid_YarnType.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_YarnType.Tag = Val(e.RowIndex)
                    cbo_Grid_YarnType.Visible = True

                    cbo_Grid_YarnType.BringToFront()
                    cbo_Grid_YarnType.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_YarnType.Visible = False

                End If

            Else

                cbo_Grid_YarnType.Visible = False
                'cbo_Grid_YarnType.Tag = -1
                'cbo_Grid_YarnType.Text = ""

            End If



            LckSTS = False
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then                   '---- Kalaimagal Textiles (Avinashi)
                If Val(lbl_BabyWgt.Text) <> 0 Then
                    LckSTS = True
                End If
            End If
            If e.ColumnIndex = 3 And LckSTS = False Then

                If cbo_Grid_MillName.Visible = False Or Val(cbo_Grid_MillName.Tag) <> e.RowIndex Then

                    cbo_Grid_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_MillName.DataSource = Dt3
                    cbo_Grid_MillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_MillName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_MillName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_MillName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_MillName.Height = rect.Height  ' rect.Height

                    cbo_Grid_MillName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_MillName.Tag = Val(e.RowIndex)
                    cbo_Grid_MillName.Visible = True

                    cbo_Grid_MillName.BringToFront()
                    cbo_Grid_MillName.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_Grid_MillName.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If

        End With
    End Sub

    Private Sub dgv_YarnDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellLeave
        With dgv_YarnDetails

            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellValueChanged
        Try
            With dgv_YarnDetails
                If .Visible Then
                    If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub

                    If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                        TotalYarnTaken_Calculation()
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_YarnDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyDown
        On Error Resume Next
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_YarnDetails

                n = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(n)

                End If

                TotalYarnTaken_Calculation()

            End With

        End If

    End Sub

    Private Sub dgv_YarnDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_YarnDetails.LostFocus
        On Error Resume Next
        dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_YarnDetails.RowsAdded
        Dim n As Integer

        With dgv_YarnDetails

            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
            .Rows(n - 1).Cells(2).Value = "MILL"

        End With
    End Sub

    Private Sub Excess_Calculation()
        Dim Exsh As Single = 0

        Exsh = Val(txt_ConsumedYarn.Text) + Val(txt_BabyWt.Text) - Val(txt_YarnTaken.Text)
        txt_ExcessShort.Text = Format(Val(Exsh), "##########0.000")

    End Sub

    Private Sub Elogation_Calculation()
        Dim xx As Single
        Dim Elgmtr As Single
        Dim SizMtr As Single
        Dim aa As Single

        xx = 0
        aa = 0
        Elgmtr = 0
        SizMtr = 0

        If Trim(UCase(cbo_BeamCount_Type.Text)) = "YARDS" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
                aa = 36.5
            Else
                aa = 36
            End If

        Else
            aa = 39.37
        End If

        If Val(txt_TapeLength.Text) <> 0 Then
            xx = Format((Val(txt_WarpMtr.Text) * aa) / (Val(txt_TapeLength.Text)), "########0.00")
        End If

        SizMtr = 0
        If dgv_PavuDetails_Total.Rows.Count > 0 Then
            SizMtr = Val(dgv_PavuDetails_Total.Rows(0).Cells(3).Value)
        End If

        Elgmtr = Val(SizMtr) - Val(xx)

        lbl_Elogation.Text = Format(Val(Elgmtr), "#########0.00")

    End Sub

    Private Sub AverageCount_Calculation()
        Dim xx As Single
        Dim Bmcnt As Single
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ends As Single
        Dim rate As Single = 0
        Dim endsCnt_idno As Integer = 0

        If Trim(UCase(cbo_EndsCount.Text)) <> "" Then
            endsCnt_idno = Common_Procedures.EndsCount_NameToIdNo(con, Trim(cbo_EndsCount.Text))

            da = New SqlClient.SqlDataAdapter("select a.*  from EndsCount_Head a  Where a.EndsCount_IdNo = " & Str(Val(endsCnt_idno)), con)
            dt = New DataTable
            da.Fill(dt)

            ends = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    ends = Val(dt.Rows(0).Item("Ends_Name").ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()
        End If

        Bmcnt = 0
        xx = 0
        If Trim(UCase(cbo_BeamCount_Type.Text)) = "METERS" Then
            xx = (Val(txt_WarpMtr.Text) * Val(ends)) / 1690

        Else
            xx = (Val(txt_WarpMtr.Text) * Val(ends)) / 1848

        End If

        If Val(txt_ConsumedYarn.Text) <> 0 Then
            Bmcnt = Format(Val(xx) / Val(txt_ConsumedYarn.Text), "#########0.00")
        End If

        lbl_Avg_Count.Text = Format(Val(Bmcnt), "#########0.00")

    End Sub

    Private Sub NetAmount_Calculation()
        Dim GrsAmt As Double = 0
        Dim TdsAmt As Double = 0
        Dim NtAmt As Double = 0
        Dim vTDSAssVal As String = ""

        lbl_InvoiceAmt.Text = Format(Val(txt_InvRate.Text) * Val(txt_ConsumedYarn.Text), "#########0.00")
        lbl_PackingAmt.Text = Format(Val(txt_PackingRate.Text) * Val(dgv_PavuDetails_Total.Rows(0).Cells(1).Value), "#########0.00")

        lbl_WindingAmt.Text = ""
        If Chk_RWSts.Checked = True Then
            lbl_WindingAmt.Text = Format(Val(txt_WindingRate.Text) * Val(txt_BabyWt.Text), "#########0.00")
        End If

        GrsAmt = Format(Val(lbl_InvoiceAmt.Text) + Val(lbl_PackingAmt.Text) + Val(lbl_WindingAmt.Text) + Val(txt_AddLess.Text), "###########0")
        lbl_GrossAmt.Text = Format(Val(GrsAmt), "###########0.00")

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1146" Then '---- ASHMITHA TEXTILE (SOMANUR)
            vTDSAssVal = Format(Val(lbl_GrossAmt.Text) - Val(txt_AddLess.Text), "##########0.00")
        Else
            vTDSAssVal = Format(Val(lbl_GrossAmt.Text), "##########0.00")
        End If
        TdsAmt = Format(Val(vTDSAssVal) * Val(txt_TdsPerc.Text) / 100, "#########0")
        lbl_TdsAmount.Text = Format(Val(TdsAmt), "#########0.00")

        NtAmt = Format(Val(lbl_GrossAmt.Text) - Val(lbl_TdsAmount.Text), "##########0")

        lbl_NetAmount.Text = Format(Val(NtAmt), "##########0.00")

        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_NetAmount.Text)))

    End Sub

    Private Sub TotalYarnTaken_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(6).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(4).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(5).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(6).Value)
                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBags)
            .Rows(0).Cells(5).Value = Val(TotCones)
            .Rows(0).Cells(6).Value = Format(Val(TotWeight), "########0.000")
        End With

        If Trim(UCase(cbo_YarnStock.Text)) = "CONSUMED YARN" Then
            txt_ConsumedYarn.Text = Format(Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value), "########0.000")
        Else
            txt_YarnTaken.Text = Format(Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value), "########0.000")
        End If
        NetAmount_Calculation()

    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                    If Val(.CurrentCell.RowIndex) <= 0 Then
                        cbo_YarnStock.Focus()

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)
                        .CurrentCell.Selected = True

                    End If

                Else

                    If Val(.CurrentCell.RowIndex) <= 0 Then
                        If dgv_PavuDetails.Rows.Count = 0 Then dgv_PavuDetails.Rows.Add()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                        dgv_PavuDetails.Focus()
                        dgv_PavuDetails.CurrentCell.Selected = True

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)
                        .CurrentCell.Selected = True

                    End If

                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    txt_TapeLength.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_CountName.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    txt_TapeLength.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End With

        End If


    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then
                With dgv_YarnDetails
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True

            End If

        End With

    End Sub

    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails
                .Focus()
                .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_MillName.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With

        End If
    End Sub

    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnDetails.EditingControlShowing
        dgtxt_YarnDetails = CType(dgv_YarnDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_YarnDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_YarnDetails.Enter
        dgv_ActiveCtrl_Name = dgv_YarnDetails.Name
        dgv_YarnDetails.EditingControl.BackColor = Color.Lime
        dgv_YarnDetails.EditingControl.ForeColor = Color.Blue
        dgv_YarnDetails.SelectAll()
    End Sub

    Private Sub cbo_Grid_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.TextChanged
        Try
            If cbo_Grid_MillName.Visible Then
                With dgv_YarnDetails
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type='')")
    End Sub

    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_YarnType.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_YarnType, Nothing, Nothing, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type = '')")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End If

        End With

    End Sub

    Private Sub cbo_Grid_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_YarnType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_YarnType, Nothing, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type='')")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Focus()
                .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index).Value = Trim(cbo_Grid_YarnType.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End With

        End If


    End Sub


    Private Sub cbo_Grid_YarnType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.TextChanged
        Try
            If cbo_Grid_YarnType.Visible Then
                With dgv_YarnDetails
                    If Val(cbo_Grid_YarnType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_YarnType.Text)
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

    Private Sub cbo_Filter_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, cbo_Filter_EndsCount, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, cbo_Filter_EndsCount, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    btn_Filter_Show_Click(sender, e)
        'End If
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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub txt_BabyWt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then
            If dgv_PavuDetails.Rows.Count = 0 Then dgv_PavuDetails.Rows.Add()
            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
            dgv_PavuDetails.Focus()
            dgv_PavuDetails.CurrentCell.Selected = True
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_RWES_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RWES.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_RwCns_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RwCns.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")

    End Sub

    Private Sub txt_RwCns_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RwCns.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_PcsLength_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsLength.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_ExcessShort_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_BabyWt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If dgv_PavuDetails.Rows.Count = 0 Then dgv_PavuDetails.Rows.Add()
            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
            dgv_PavuDetails.Focus()
            dgv_PavuDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub Chk_RWSts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chk_RWSts.Click
        If Chk_RWSts.Checked = True Then

            txt_RwBags.Enabled = True
            txt_RwCns.Enabled = True
            txt_RWES.Enabled = True
        Else

            txt_RwBags.Enabled = False
            txt_RwCns.Enabled = False
            txt_RWES.Enabled = False

        End If

        If Chk_RWSts.Checked = True And Val(txt_RwCns.Text) = 0 And Val(txt_BabyWt.Text) <> 0 Then
            txt_RwCns.Text = Format(Val(txt_BabyWt.Text) / 1.5, "##########0")
        End If

    End Sub

    Private Sub Chk_RW_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Chk_RWSts.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Filter_SetNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Filter_SetNo.KeyDown
        If e.KeyCode = 40 Then btn_Filter_Show.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then cbo_Filter_EndsCount.Focus()
    End Sub

    Private Sub txt_Filter_SetNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_SetNo.KeyPress
        If Asc(e.KeyChar) = 13 Then btn_Filter_Show_Click(sender, e)
    End Sub

    Private Sub dgtxt_PavuDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.Enter
        dgv_ActiveCtrl_Name = dgv_PavuDetails.Name
        dgv_PavuDetails.EditingControl.BackColor = Color.Lime
        dgv_PavuDetails.EditingControl.ForeColor = Color.Blue
        dgv_PavuDetails.SelectAll()
    End Sub

    Private Sub dgtxt_YarnDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_YarnDetails.KeyPress
        With dgv_YarnDetails

            If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

                If Val(lbl_BabyWgt.Text) <> 0 Then
                    e.Handled = True
                Else
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If

            End If
        End With

    End Sub

    Private Sub dgtxt_yarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_YarnDetails.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_YarnDetails_KeyUp(sender, e)
        End If

    End Sub

    Private Sub dgtxt_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyDown
        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
            e.Handled = True
            e.SuppressKeyPress = True
        Else
            If Val(dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(5).Value) <> 0 Then
                e.SuppressKeyPress = True
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub dgtxt_PavuDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_PavuDetails.KeyPress
        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
            e.Handled = True
        Else
            If Val(dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(5).Value) <> 0 Then
                e.Handled = True
            Else
                If dgv_PavuDetails.CurrentCell.ColumnIndex = 2 Or dgv_PavuDetails.CurrentCell.ColumnIndex = 3 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub dgtxt_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyUp
        If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Then
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                dgv_PavuDetails_KeyUp(sender, e)
            End If
        End If

    End Sub

    Private Sub txt_TapeLength_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TapeLength.KeyDown
        If e.KeyCode = 38 Then
            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

            Else
                txt_TotalBeams.Focus()

            End If
        End If

        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_TapeLength_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TapeLength.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TdsPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TdsPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Elogation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_PickUp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PickUp.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TdsPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TdsPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Amount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Rate As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0
        Dim LedCondt As String = ""
        Dim Cmp_Cond As String = ""

        If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Then Exit Sub

        If New_Entry = False Then
            If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Then
                MessageBox.Show("Invalid Entry Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
                Exit Sub
            End If
        End If

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)

        LedCondt = ""
        If LedIdNo <> 0 Then
            LedCondt = "(a.Ledger_Idno = " & Str(Val(LedIdNo)) & ")"
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then '---- Ganesh karthick Textiles (p) Ltd (Somanur)
            Cmp_Cond = ""
            ' If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 0 Then
            Cmp_Cond = " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and "
            'End If
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name,  d.EndsCount_Name from Sizing_Pavu_Receipt_Head a INNER JOIN Ledger_Head c ON a.Ledger_Idno = c.Ledger_Idno INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where " & Cmp_Cond & " a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and " & LedCondt & IIf(Trim(LedCondt) <> "", " and ", "") & " a.Sizing_Specification_Code = '" & Trim(NewCode) & "'  order by a.Sizing_Pavu_Receipt_Date, a.for_orderby, a.Sizing_Pavu_Receipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Sizing_Pavu_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Total_Beam").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Total_Meters").ToString
                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Code").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.EndsCount_Name from Sizing_Pavu_Receipt_Head a INNER JOIN Ledger_Head c ON a.Ledger_Idno = c.Ledger_Idno INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where " & LedCondt & IIf(Trim(LedCondt) <> "", " and ", "") & " a.Sizing_Specification_Code = '' order by a.Sizing_Pavu_Receipt_Date, a.for_orderby, a.Sizing_Pavu_Receipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Sizing_Pavu_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Total_Beam").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Total_Meters").ToString
                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Sizing_Pavu_Receipt_Code").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Black
                    Next

                Next

            End If
            Dt1.Clear()
        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_PavuReceipt(e.RowIndex)
    End Sub

    Private Sub Select_PavuReceipt(ByVal RwIndx As Integer)

        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(8).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(8).Value = ""

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

                Select_PavuReceipt(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_PavuReceipt_Selection()
    End Sub

    Private Sub Close_PavuReceipt_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim Set_No As String = ""

        If New_Entry = False Then
            If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Then
                MessageBox.Show("Invalid Entry Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
                Exit Sub
            End If
        End If

        dgv_PavuDetails.Rows.Clear()

        Set_No = ""

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                If Trim(Set_No) <> "" Then
                    If Trim(UCase(Set_No)) <> Trim(UCase(dgv_Selection.Rows(i).Cells(3).Value)) Then
                        MessageBox.Show("Select Same SetNo", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Selection.Enabled And dgv_Selection.Visible Then
                            dgv_Selection.Focus()
                            dgv_Selection.CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If
                End If

                Set_No = Trim(dgv_Selection.Rows(i).Cells(3).Value)

            End If

        Next

        pnl_Back.Enabled = True

        MovSTS = True
        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                Da1 = New SqlClient.SqlDataAdapter("select a.* from Sizing_Pavu_Receipt_Details a where a.Sizing_Pavu_Receipt_Code  = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' order by a.sl_no", con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)

                cbo_Type.Text = "RECEIPT"
                txt_SetNo.Text = dgv_Selection.Rows(i).Cells(3).Value
                cbo_Sizing.Text = dgv_Selection.Rows(i).Cells(4).Value
                cbo_EndsCount.Text = dgv_Selection.Rows(i).Cells(5).Value

                If Dt1.Rows.Count > 0 Then

                    txt_PcsLength.Text = Val(Dt1.Rows(0).Item("Meters_Pc").ToString)

                    For j = 0 To Dt1.Rows.Count - 1

                        n = dgv_PavuDetails.Rows.Add()

                        sno = sno + 1
                        dgv_PavuDetails.Rows(n).Cells(0).Value = Val(sno)
                        dgv_PavuDetails.Rows(n).Cells(1).Value = Dt1.Rows(j).Item("Beam_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(2).Value = Dt1.Rows(j).Item("Pcs").ToString
                        dgv_PavuDetails.Rows(n).Cells(3).Value = Dt1.Rows(j).Item("Meters").ToString
                        dgv_PavuDetails.Rows(n).Cells(5).Value = ""
                        dgv_PavuDetails.Rows(n).Cells(6).Value = Dt1.Rows(j).Item("Sizing_Pavu_Receipt_Code").ToString

                    Next

                End If
                Dt1.Clear()

            End If
            Dt1.Clear()

        Next

        MovSTS = False
        TotalPavu_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_SetNo.Enabled Then
            txt_SetNo.Focus()
        Else
            txt_TotalBeams.Focus()
        End If
        'If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
        '    If dgv_PavuDetails.Rows.Count > 0 Then
        '        btn_save.Focus()
        '    Else
        '        dtp_Date.Focus()
        '    End If
        'End If

    End Sub

    Private Sub txt_WarpMtr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WarpMtr.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_WarpMtr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WarpMtr.TextChanged
        Elogation_Calculation()
        AverageCount_Calculation()
    End Sub

    Private Sub txt_TapeLength_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TapeLength.TextChanged
        Elogation_Calculation()
    End Sub

    Private Sub txt_ConsumedYarn_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ConsumedYarn.TextChanged
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then '---- M.K Textiles (Palladam)
            Excess_Calculation()
        End If
        AverageCount_Calculation()
    End Sub

    Private Sub cbo_EndsCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.TextChanged
        AverageCount_Calculation()
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_PackingRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_PackingRate.TextChanged

        NetAmount_Calculation()
    End Sub

    Private Sub txt_WindingRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WindingRate.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_InvRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_InvRate.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_YarnStock_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_YarnStock.TextChanged

        If Trim(UCase(cbo_YarnStock.Text)) = "CONSUMED YARN" Then
            txt_ConsumedYarn.Enabled = False
            txt_YarnTaken.Enabled = True
        Else
            txt_ConsumedYarn.Enabled = True
            txt_YarnTaken.Enabled = False
        End If
        If dgv_YarnDetails_Total.RowCount > 1 Then
            If Trim(UCase(cbo_YarnStock.Text)) = "CONSUMED YARN" Then
                If Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value) <> 0 Then
                    txt_ConsumedYarn.Text = Format(Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value), "########0.000")
                End If
            Else
                If Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value) <> 0 Then
                    txt_YarnTaken.Text = Format(Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value), "########0.000")
                End If
            End If
        End If

    End Sub

    Private Sub txt_RwCns_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_RwCns.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_BabyWt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_BabyWt.TextChanged
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then '---- M.K Textiles (Palladam)
            Excess_Calculation()
        End If
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_YarnStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_YarnStock.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_YarnStock, txt_TotalBeams, Nothing, "", "", "", "")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_YarnStock, txt_PcsLength, txt_TotalBeams, "", "", "", "")

        Try
            With txt_TotalBeams
                If e.KeyValue = 40 Then
                    If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                        dgv_YarnDetails.Focus()
                        dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                    Else

                        dgv_PavuDetails.Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                        dgv_PavuDetails.CurrentCell.Selected = True


                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_YarnStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_YarnStock.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_YarnStock, Nothing, "", "", "", "", False)

        Try
            With txt_TotalBeams
                If Asc(e.KeyChar) = 13 Then
                    If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                        dgv_YarnDetails.Focus()
                        dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                    Else

                        dgv_PavuDetails.Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                        dgv_PavuDetails.CurrentCell.Selected = True


                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_TotalBeams_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TotalBeams.KeyDown
        'Try
        '    With txt_TotalBeams
        '        If e.KeyValue = 40 Then
        '            If Trim(UCase(cbo_Type.Text)) = "DIRECT" Then

        '                dgv_PavuDetails.Focus()
        '                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
        '                dgv_PavuDetails.CurrentCell.Selected = True


        '            Else

        '                dgv_YarnDetails.Focus()
        '                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

        '            End If

        '        End If
        '        If e.KeyValue = 38 Then
        '            cbo_YarnStock.Focus()
        '        End If
        '    End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try
    End Sub

    Private Sub txt_TotalBeams_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TotalBeams.KeyPress
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(cbo_Type.Text)) = "DIRECT" Then
        '        If dgv_PavuDetails.Rows.Count > 0 Then
        '            dgv_PavuDetails.Focus()
        '            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
        '            dgv_PavuDetails.CurrentCell.Selected = True
        '        Else
        '            txt_WarpMtr.Focus()
        '        End If

        '    Else
        '        If dgv_YarnDetails.Rows.Count > 0 Then
        '            dgv_YarnDetails.Focus()
        '            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

        '        Else
        '            txt_WarpMtr.Focus()
        '        End If
        '    End If

        'End If
    End Sub

    Private Sub cbo_BeamCount_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BeamCount_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BeamCount_Type, txt_WarpMtr, txt_ConsumedYarn, "", "", "", "")

    End Sub

    Private Sub cbo_BeamCount_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BeamCount_Type.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BeamCount_Type, txt_ConsumedYarn, "", "", "", "", False)

    End Sub

    Private Sub txt_YarnTaken_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_YarnTaken.TextChanged
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then '---- M.K Textiles (Palladam)
            Excess_Calculation()
        End If
    End Sub

    Private Sub btn_BeamClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_BeamClose.Click
        pnl_Back.Enabled = True
        pnl_BeamDetails.Visible = False
    End Sub

    Private Sub btn_BeamDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_BeamDetail.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim SNo As Integer
        Try

            With dgv_BeamDetails

                .Rows.Clear()
                SNo = 0

                da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name ,  d.EndsCount_Name from Sizing_Specification_Head a INNER JOIN Ledger_Head c ON c.Ledger_Idno  = A.Ledger_Idno INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where a.Total_PlanBeams <> a.Total_Beam order by a.Sizing_Specification_Date, a.for_orderby, a.Sizing_Specification_No", con)
                dt1 = New DataTable
                da.Fill(dt1)

                If dt1.Rows.Count > 0 Then

                    For i = 0 To dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = dt1.Rows(i).Item("Sizing_Specification_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt1.Rows(i).Item("Sizing_Specification_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = dt1.Rows(i).Item("Set_No").ToString
                        .Rows(n).Cells(4).Value = dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(5).Value = dt1.Rows(i).Item("EndsCount_Name").ToString
                        .Rows(n).Cells(6).Value = dt1.Rows(i).Item("Total_PlanBeams").ToString
                        .Rows(n).Cells(7).Value = dt1.Rows(i).Item("Total_Beam").ToString
                        .Rows(n).Cells(8).Value = Val(dt1.Rows(i).Item("Total_PlanBeams").ToString - Val(dt1.Rows(i).Item("Total_Beam").ToString))

                    Next

                End If
                dt1.Clear()

            End With
            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        pnl_BeamDetails.Visible = True
        pnl_Back.Enabled = False
        dgv_BeamDetails.Focus()
    End Sub


    Private Sub dgv_BeamDetails_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BeamDetails.CellDoubleClick
        Open_BeamReceiptEntry()
    End Sub

    Private Sub dgv_BeamDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BeamDetails.KeyDown
        If e.KeyCode = 13 Then
            Open_BeamReceiptEntry()
        End If
    End Sub

    Private Sub cbo_Type_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.LostFocus
        If Trim(cbo_Type.Text) = "" Or (Trim(UCase(cbo_Type.Text)) <> "" And Trim(UCase(cbo_Type.Text)) <> "DIRECT" And Trim(UCase(cbo_Type.Text)) <> "RECEIPT") Then
            cbo_Type.Text = "DIRECT"
        End If
    End Sub

    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        txt_SetNo.Enabled = True
        cbo_EndsCount.Enabled = True
        txt_PcsLength.Enabled = True
        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
            txt_SetNo.Enabled = False
            cbo_EndsCount.Enabled = False
            txt_PcsLength.Enabled = False
        End If
    End Sub
    Private Sub cbo_Grid_BeamWidth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BeamWidth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_BeamWidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BeamWidth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_BeamWidth, Nothing, Nothing, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_PavuDetails

            If (e.KeyValue = 38 And cbo_Grid_BeamWidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_BeamWidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_BeamWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_BeamWidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_BeamWidth, Nothing, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_PavuDetails
                e.Handled = True
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_Grid_BeamWidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BeamWidth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_BeamWidth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_Grid_BeamWidth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BeamWidth.TextChanged
        Try
            If cbo_Grid_BeamWidth.Visible Then
                With dgv_PavuDetails
                    If Val(cbo_Grid_BeamWidth.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BeamWidth.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub dtp_InvoiceDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_InvoiceDate.ValueChanged
        msk_InvoiceDate.Text = dtp_InvoiceDate.Text
    End Sub

    Private Sub dtp_InvoiceDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_InvoiceDate.Enter
        msk_InvoiceDate.Focus()
        msk_InvoiceDate.SelectionStart = 0
    End Sub
    Private Sub dtp_Date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_Date.ValueChanged
        msk_Date.Text = dtp_Date.Text
    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.Enter
        msk_Date.Focus()
        msk_Date.SelectionStart = 0
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
        End If
    End Sub
    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            e.Handled = True

            txt_TdsPerc.Focus()
            'dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(0)
            'dgv_YarnDetails.CurrentCell.Selected = True
            'dgv_YarnDetails.Focus()

            ' SendKeys.Send("+{TAB}")
        End If

    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Date.Text = Date.Today
        'End If
        If IsDate(msk_Date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            End If
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



    Private Sub msk_InvoiceDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_InvoiceDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_InvoiceDate.Text = Date.Today
        End If
        If IsDate(msk_InvoiceDate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_InvoiceDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_InvoiceDate.Text))
            ElseIf e.KeyCode = 109 Then
                msk_InvoiceDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_InvoiceDate.Text))
            End If
        End If
    End Sub

    Private Sub msk_InvoiceDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_InvoiceDate.LostFocus

        If IsDate(msk_InvoiceDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_InvoiceDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_InvoiceDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_InvoiceDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_InvoiceDate.Text)) >= 2000 Then
                    dtp_InvoiceDate.Value = Convert.ToDateTime(msk_InvoiceDate.Text)
                End If
            End If

        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub
    Private Sub msk_InvoiceDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_InvoiceDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
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

        LastNo = lbl_RefNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub cbo_Sizing_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbo_Sizing.SelectedIndexChanged

    End Sub

    Private Sub dgv_PavuDetails_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellContentClick

    End Sub

    Private Sub txt_AddLess_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub


End Class